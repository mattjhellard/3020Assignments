using System;
using Node = Rope; // iirc "nodes" in this context are same as "ropes" and vice versa, this makes it official, can refactor away if need be
using System.IO; //NOTE: Rope does NOT make use of this at all, only utilized in Main to quickly insert HUGE strings as inputs for Rope

public class Rope
{
    private const int maxLeafLength = 10;
    private const String parentValue = "";
    //^ note: the compiler is smart enough to not have duplicates of const values in every instance of the class, iirc all instances of the class share one copy of the consts
    private int length;
    private Node leftChild, rightChild;
    private String value;

    // (5 marks) Create a balanced rope from a given string S
    public Rope(string S) // due to the recursive nature of the rope, this being the only constructor, and the fact that Build has to create a new Node somehow, we arrive at the conclusion that the constructor is going to be called within that recursion, and as such handles much of the recursion logic itself
    {
        length = S.Length;
        if (length <= maxLeafLength) //base case for recursion / leaf determination, a bonus of putting this in the constructor is handling inputs too small to break up is very easy
        { //we check base case first as technically the majority of nodes are leafs
            value = S;
            leftChild = rightChild = null;
        }
        else //if the given string still needs to be broken down some more (we are handling a parent to-be)
        {
            value = parentValue; //note: as of writing the value held in a parent node is never actually used
            int splitIndex = (int)length / 2; //this cast effectively rounds down, the result of this is if totalLength is odd then the right child always gets the extra char
            leftChild = Build(S, 0, splitIndex); //build basically returns whatever THIS constructor makes when the substring defined in the arguments is fed back into it
            rightChild = Build(S, splitIndex, length); // TODO: explain why the end of leftChild should have the same value as the start of rightChild
        }
    }

    // Recursively build a balanced rope for S[i,j] and return its root
    private Node Build(string s, int i, int j)
    { //as this is a private method we can assume methods calling it will simply use it properly, and then go make sure that's definitely the case
        string sPartial = s.Substring(i, j - i); //we need to actually create a substring here as the constructor doesn't take any substring index arguments
        Node root = new Node(sPartial); //recursive constructor call
        return root;
    }

    // (3 marks) Return the root of the rope constructed by concatenating two ropes with roots p and q
    private Node Concatenate(Node p, Node q)
    {
        Node root = new Node(parentValue); //a (necessary) side effect of the short length of the parent node value is that when we create a rope with this value we are guaranteed that it will have no children of its own,
        root.leftChild = p; //, though even if root was given non-null children they would just be overwritten here anyway 
        root.rightChild = q;
        return root;
    }

    // (9 marks) Split the rope with root p at index i and return the root of the right subtree
    private Node Split(Node p, int i)
    {
        // TODO: implement method Split (unclaimed)
        return null; //placeholder
    }
    // (9 marks) Rebalance the rope using the algorithm found on pages 1319-1320 of Boehm et al.
    private Node Rebalance()
    {
        // TODO: implement method Rebalance (unclaimed)
        return null; //placeholder
    }
    // (5 marks) Insert string S at index i
    public void Insert(string S, int i)
    {
        // TODO: implement method Insert (unclaimed)
    }
    // (5 marks) Delete the substring S[i,j]
    public void Delete(int i, int j)
    {
        // TODO: implement method Delete (unclaimed)
    }
    // (6 marks) Return the substring S[i,j]
    public string Substring(int i, int j)
    { // TODO: solve potential off by one error where calling S[i,j] returns S[i,j-1] (THIS COULD BE WORKING AS INTENDED, MAKE SURE BEFORE REALLY LOOKING FOR FIX)
        if (i > j)
        {
            throw new ArgumentException();
        }
        if (leftChild == null && rightChild == null) // base case, we assume indices have already been appropriately recontextualized for the current root
        {
            return value.Substring(i, j - i);
        }
        //if not in base case then we need to know which of the two children to recurse into, there are only three options so we just manually decide with explicit logic
        //if both indices are less than leftchild length then we should only recurse into leftchild as the whole substring is in there
        if (leftChild != null && i < leftChild.length && j < leftChild.length)
        {
            return leftChild.Substring(i, j);
        }
        //if i is greater than or equal (indices zero based) to leftchild length then the whole substring is in rightchild (i <= j)
        if (rightChild != null && i >= leftChild.length)
        { //must adjust indices for removal of leftChild from context
            return rightChild.Substring(i - leftChild.Length, j - leftChild.Length);
        }
        //if i < leftChild length while j > leftChild length then the substring is split between both children, we append what we get back from left with what we get back from right
        if (leftChild != null && i < leftChild.length && rightChild != null && j >= leftChild.length)
        {
            String subString = leftChild.Substring(i, leftChild.length);
            subString += rightChild.Substring(0, j - leftChild.length);
            return subString;
        }
        //we shouldn't get to this point if the indices can be found
        throw new ArgumentOutOfRangeException();
    }

    // (9 marks) Return the index of the first occurrence of S; -1 otherwise
    public int Find(string S)
    { // TODO: do this better
        if (S.Length == 0) //immediately obvious fail case(s)
        { //failing when given string is empty both makes some sense semantically and makes the process much easier
            return -1;
        }
        //these variables are "shared" by all instances of the BuildS function
        int SIndex = 0; //tracks where in S we are currently looking
        int GlobalIndex = 0; //tracks where in the rope we currently are (critical for returning an index on success)
        BuildS(this);
        void BuildS(Node root)
        {
            if (root.leftChild == null && root.rightChild == null) //base case
            { //when at a leaf, we loop through the value to the end or stop when we detect success
                for (int i = 0; i < root.value.Length && SIndex < S.Length; i++)
                {
                    if (root.value[i] == S[SIndex]) //if the current char matches the char we want to see
                    {
                        SIndex++; //we move SIndex so we know to start looking for the next char
                    }
                    else //if current char doesn't match what we want to see
                    {
                        SIndex = 0; //we reset SIndex because we don't have a match, since we don't run the loop once we find a full match no issues present themselves
                    }
                    GlobalIndex++; //each loop, regardless of success or failure, shifts our position in the full rope, so we increment accordingly
                }
            }
            //traversing rope recursively
            if (root.leftChild != null && SIndex < S.Length)
            {
                BuildS(root.leftChild);
            }
            if (root.rightChild != null && SIndex < S.Length)
            {
                BuildS(root.rightChild);
            }
        }
        //when BuildS has finished including recursions, if SIndex equals S length then there exists an unbroken sequence of chars in the rope that matches S, so S is in the rope
        if (SIndex == S.Length)
        {
            return GlobalIndex - SIndex;
        }
        else //if this is not the case, then there is not an unbroken chain of chars that matches S, so S is not in the rope 
        {
            return -1;
        }
    }
    // (3 marks) Return the character at index i
    public char CharAt(int i)
    {
        if (leftChild == null && rightChild == null && value.Length >= i && value != "") //base case + error prevention
        {
            return value[i];
        }
        if (leftChild != null && i < leftChild.length)
        {//whenever we go left, we don't have to modify the index we're searching for because basically there's nothing left of left so if we go left the number is already appropriate for left child being the new root
            return leftChild.CharAt(i);
        }
        if (rightChild != null)
        { //whenever we go right, we need to modify the index for the context of the rightChild being the next root, as the initial i is given with leftChild in mind, but we're removing leftChild from the context when we make rightChild the new root
            return rightChild.CharAt(i - leftChild.length);
        }
        //if we get here then the method failed to find a char for the given index, which shouldn't be possible if the index isn't out of bounds
        throw new ArgumentOutOfRangeException();
    }

    // (4 marks) Return the index of the first occurrence of character c
    public int IndexOf(char c)
    {
        return FirstIndexOfC(this); //the local function does all the work
        //our recursive local function need not use c as a parameter because we are always within scope of the public method 
        int FirstIndexOfC(Node root) //I understand we could just overload IndexOf for the name but I found that a little hard to parse visually tbh, likely due to same parameter count
        { // TODO: (optional) maybe do this more readably, perhaps by making better use of totalLength somehow? 
            int index;
            if (root.leftChild != null && (index = FirstIndexOfC(root.leftChild)) != -1)
            {
                return index;
                //locally, when found in the left child, the index returned from the base case needs no modification to be accurate, as there was nothing to the left of it anyway
            }
            if (root.rightChild != null && (index = FirstIndexOfC(root.rightChild)) != -1)
            {
                return index + (root.leftChild != null ? root.leftChild.length : 0);
                //^ elusive ternary operator, statement expands to: if leftChild not null return index+leftChild.totalLength, else return index+0
                //locally, when found in right child, the index returned should be added to the length of the left child (if it exists), to keep the index accurate to the local root
            }
            //actually checking for value after searching children, i.e., postorder search, note that this means a parent node in the rope context will never be searched before all it's leaves have been searched
            for (int i = 0; i < root.value.Length; i++) //note: as of writing parent nodes skip this loop almost entirely as their value fails the conditional as soon as it's checked
            {
                if (root.value[i] == c) //c# was nice enough to allow strings to be treated as arrays of chars in some ways, since that's probably still what they are under the hood
                {
                    return i;
                }
            }
            return -1; //if no children of the local root (or the root itself) contain the char, we're assuming -1 is an appropriate return value for no results
        }
    }

    // (5 marks) Reverse the string represented by the current rope
    public void Reverse()
    {
        //reversing children
        Node rightOrphan = rightChild;
        rightChild = leftChild;
        leftChild = rightOrphan;

        //reversing value if leaf
        if (leftChild == null && rightChild == null)
        {
            String valueReverse = "";
            for (int i = value.Length - 1; i >= 0; i--)
            {
                valueReverse += value[i];
            }
            value = valueReverse;
        }
        else //if not leaf then repeat process with children if they're not null (they can't both be null at this point, but one of them still could be)
        {
            if (leftChild != null)
            {
                leftChild.Reverse();
            }
            if (rightChild != null)
            {
                rightChild.Reverse();
            }
        }
    }

    // (1 mark) Return the length of the string
    public int Length //modified into property for ease of use by user and to make it function more like a typical string
    { //note: while there's nothing explicitely preventing a child node from having this called, a user has no way of accessing children of the ultimate root of the rope they created, so they can only call this from the ultimate root
      //also, any local methods should just get length directly from the variable
        get { return length; }
    }

    // (4 marks) Return the string represented by the current scope
    public override string ToString() //note: all objects have a (basic) ToString method, inherited from object, so this method is technically an override 
    {
        if (leftChild == null && rightChild == null) //if we're at a leaf we just return the value
        {
            return value;
        }
        //otherwise we recursively append the right child to the left child and return that
        String fullString = "";
        if (leftChild != null) // TODO: double check that these conditionals would ever be necessary
        {
            fullString += leftChild.ToString();
        }
        if (rightChild != null)
        {
            fullString += rightChild.ToString();
        }
        return fullString;
    }

    // (4 marks) Print the augmented binary tree of the current rope
    public void PrintRope()
    {
        Console.WriteLine("(Rope): Printing rope...");
        PrintRope(0, this); //we simply use the public method to initiate the private recursive function
        //using local function vs private method as the function exists only to be used by the parent method, so making usage by other methods possible is semantically incorrect
        void PrintRope(int indentation, Node root)
        { //this approach sort-of breaks when the string is absurdly large as the console window can't get wide enough to show the max indendation and there's no way to turn off word wrapping in the console afaik so it just becomes unreadable, I'd blame MS before ourselves for that though
            String indent = new String('\t', indentation);
            //^ this uses an obscure overload of the String constructor to make our sequence of indendations for us
            //by building the indent string discretely uptop, we simplify the logic of printing what we want with it's required indentations
            //as strings are reference types, it would likely be quite the headache to have an actual string passed in the arguments, so building a new one for each instance of the method is basically the only option 

            if (root == null) //if we allow ourselves to step into null children and then just check at the start of each recursion, the code is a lot simpler (one check vs two similar checks), although admittedly the overhead of up to two unnecessary recursions (per node) is probably a lot more expensive than one extra conditional, so we're sacrificing performance for looks here
            {
                return;
            }
            if (root.leftChild == null && root.rightChild == null) //case when at leaf node
            {
                Console.WriteLine(indent + "* " + root.value);
            }
            else //case when at parent node
            { //recall inorder search
                PrintRope(indentation + 1, root.rightChild); //going right to left so that the printed tree more accurately represents a 90 degree counter-clockwise rotation of the true tree
                Console.WriteLine(indent + "*"); //we use * to indicate a node in print, indentation should guarantee that visually segregating parents from leafs is never too challenging regardless
                PrintRope(indentation + 1, root.leftChild);
            }
        }
    }
}

public static class User
{
    public static void Main(string[] args) // TODO: let's consider implementing accepting initial arguments, if we don't do that then remove parameters from main before submission
    {
        Console.WriteLine("3020 Assignment 2\nby\nBenjamin Macintosh\nMatthew Hellard\nInput (help) for commands listing\n");
        // we want these variables to persist between loop iterations
        bool run = true;
        Rope rope = new Rope(""); //we instantiate to something to minimize errors
        while (run)
        {
            Console.Write("Input :");
            //we tokenize the user input (space separated) to enable inputting all required data for a command at once, similar to any real CLI
            //like a CLI, we want to include options (ex: -l)
            // TODO: IF it proves more work than it's worth to implement decently, remove options functionality, disregard this otherwise
            String[] input = Console.ReadLine().Trim(' ').Split(' ');

            //this section defines the default values for all potential arguments, if the value changes from the default, then it was successfully given as an argument
            //the names may look a little cryptic but they're just the relevant command and relevant flag stuck together
            String NRp = null;

            //this loop runs through the user input and assigns arguments for flags as appropriate, it does not make particular effort to protect the user from data entry errors
            for (int i = 1; i < input.Length - 1; i++) //we need not check the first arg (always a command) or the last arg (never a flag), not entering the loop with the last arg also helps prevent index out of bounds errors
            {
                switch (input[i])
                {
                    case "-p":
                        NRp = input[i + 1];
                        i++; //we just assigned i+1 as an argument for a flag, so we need not run this loop for i+1 at all
                        break;
                }
            }
            switch (input[0].ToLower()) //we don't want to change case of whole input as data inputted should allow for case-sensitivity, but command (first substring) can ignore this and modify case for convenience
            {
                case "nr":
                case "new rope":
                    if (input.Length == 2) //if one arg given after command we assume we are to use it as the input for the rope 
                    {
                        rope = new Rope(input[1]);
                    }
                    else if (NRp != null) //if path was given
                    {
                        String path;
                        //NOTE: the following logic assumes NTFS filesystem (the one modern Windows uses), running this code on Linux, or macOS, or something weird, could and probably would produce errors here, though that probably would be the case even if this wasn't here, idek if you can compile c# outside windows, why would MS bother writing a non windows compiler, let alone anyone else?
                        try
                        {
                            if (NRp.Length >= 2 && NRp[1] == ':') //if it looks like the user input an absolute path, first conditional just avoids out of bounds exceptions
                            //^ : are illegal in filenames and folders on Windows and drives are always designated as a single char so a : in the 2nd position almost certainly refers to a drive, in which case the given path is certainly absolute
                            {
                                path = @NRp; //prepending string with @ has the string be taken as verbatim literal, which means \ are treated literally rather than as escape characters 
                            }
                            else //if given path isn't absolute we assume it's relative to local root, which depends on where compiled code is running from
                            { //TODO: remove this comment by submission: current directory when running from VS will by default be: ...\3020Assignments\Assignment 2\bin\Debug
                                path = @Directory.GetCurrentDirectory() + @"\" + @NRp;
                            }
                            if (!File.Exists(path)) //we escape the structure if we can't find the file by throwing an exception and catching it outside the structure
                            {
                                throw new ArgumentException("!: file not found");
                            }
                            //if found, we try to read in the file in whole
                            StreamReader sr = new StreamReader(path);
                            String fileContents = sr.ReadToEnd();
                            sr.Close();
                            rope = new Rope(fileContents);
                        }
                        catch (ArgumentException e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("!: unknown file read error");
                        }
                    }
                    else //default case
                    {
                        Console.Write("!: Input new rope string :");
                        rope = new Rope(Console.ReadLine());
                    }
                    break;
                case "gsr":
                case "get substring in range":
                    String iString;
                    String jString;
                    if (input.Length >= 3)
                    {
                        iString = input[1];
                        jString = input[2];
                    }
                    else
                    {
                        Console.Write("!: Input first int :");
                        iString = Console.ReadLine();
                        Console.Write("!: Input second int :");
                        jString = Console.ReadLine();
                    }
                    try
                    {
                        int i = Convert.ToInt32(iString);
                        int j = Convert.ToInt32(jString);
                        Console.WriteLine("!: Substring from {0} to {1} is :{2}", i, j, rope.Substring(i, j));
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("!: Could not convert argument(s) to int");
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        Console.WriteLine("!: Given arguments out of range");
                    }
                    catch (ArgumentException)
                    {
                        Console.WriteLine("!: Start index must be less than end index");
                    }
                    break;
                case "fs":
                case "find substring":
                    string inputString = "";
                    if (input.Length >= 2)
                    {
                        //this loop is a consequence of us tokenizing the initial input by space, to enable inputting strings with spaces we have to first rebuild it
                        for (int i = 1; i < input.Length - 1; i++)
                        {
                            inputString += input[i] + " "; //we have to re-add in the spaces as the tokenization didn't keep them
                        }
                        //we skip the final bit of the array in the loop so we can treat it differently (not append with space)
                        inputString += input[input.Length - 1];
                    }
                    else
                    {
                        Console.Write("!: Input substring to find :");
                        inputString = Console.ReadLine();
                    }
                    int subStringIndex = rope.Find(inputString);
                    if (subStringIndex == -1)
                    {
                        Console.WriteLine("Could not find string \"{0}\" in rope", inputString);
                    }
                    else
                    {
                        Console.WriteLine("String \"{0}\" found starting at index {1}", inputString, subStringIndex);
                    }
                    break;
                case "gci":
                case "get character at index":
                    String gciInput;
                    if (input.Length >= 2)
                    {
                        gciInput = input[1];
                    }
                    else
                    {
                        Console.Write("!: Input int index to search at :");
                        gciInput = Console.ReadLine();
                    }
                    try
                    {
                        int indexToFind = Convert.ToInt32(gciInput);
                        Console.WriteLine("!: Char at index {0} is '{1}'", indexToFind, rope.CharAt(indexToFind));
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        Console.WriteLine("!: Given argument out of range of rope");
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("!: Could not convert argument to int");
                    }
                    break;
                case "gic":
                case "get index of character":
                    char inputToUse; //this is declared separately because we use the user determined value several times in this case
                    if (input.Length >= 2) //gic only accepts one arg and no flags, but if more than 1 arg given we'll still just take the first char of the first arg
                    {
                        inputToUse = input[1][0]; //first char of user input string, we only want a char and this guarantees we get it
                    }
                    else
                    {
                        Console.Write("!: Enter char to look for :");
                        inputToUse = Console.ReadLine()[0]; //there are other ways to get chars specifically from user input but this one is simple and involves the fewest potential exceptions being thrown
                    }
                    int charIndex = rope.IndexOf(inputToUse); //we want to print the index AND check its value so we must store it after the call
                    if (charIndex == -1)
                    {
                        Console.WriteLine("!: Could not find char '{0}' in rope", inputToUse);
                    }
                    else
                    {
                        Console.WriteLine("!: '{0}' found at index {1}", inputToUse, charIndex);
                    }
                    break;
                case "rr":
                case "reverse rope":
                    rope.Reverse();
                    break;
                case "pr":
                case "print rope":
                    rope.PrintRope();
                    break;
                case "gl":
                case "get length":
                    Console.WriteLine("!: Length of rope is: " + rope.Length);
                    break;
                case "tts":
                case "translate to string":
                    Console.WriteLine("!: Rope to string: " + rope.ToString());
                    break;
                case "q":
                case "qq":
                case "qqq":
                case "quit":
                case "exit":
                    run = false;
                    break;
                case "help":
                case "h":
                case "?":
                    Console.WriteLine(
                        "!: All subsequent arguments/options past command optional, order of options shown is only valid order of input\n" +
                        "-Help (h)\n" + //implemented
                        "-New Rope (nr) <string to use> || (nr) -p <path to file containing string>\n" + //implemented
                        "-Insert Substring at Index (isi)\n" +
                        "-Delete Substring in Range (dsr)\n" +
                        "-Get Substring in Range (gsr) <start index> <end index>\n" + //implemented
                        "-Find first Substring (fs) <substring to find>\n" + //implemented
                        "-Get Character at Index (gci) <int index to search in>\n" + //implemented
                        "-Get first Index of Character (gic) <character to look for>\n" + //implemented
                        "-Reverse Rope (rr)\n" + //implemented
                        "-Get Length (gl)\n" + //implemented
                        "-Translate To String (tts)\n" + //implemented
                        "-Print Rope (pr)\n" + //implemented
                        "-Quit (q)"); //implemented
                    break;
                default:
                    Console.WriteLine("!: unknown input, use (help) for list of valid commands");
                    break;
            }
        }
    }
}