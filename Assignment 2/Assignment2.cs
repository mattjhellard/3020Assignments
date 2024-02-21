using System;
using Node = Rope; // iirc "nodes" in this context are same as "ropes" and vice versa, this makes it official, can refactor away if need be

public class Rope
{
    private const int maxLeafLength = 10; // TODO: consider refactoring this into just 10 at submission, it is a useless datamember by compilation time, if it was c we could just use a #define but this is c#
    private int totalLength;
    private Node leftChild, rightChild;
    private String value; // TODO: find better name for this data member

    // (5 marks) Create a balanced rope from a given string S
    public Rope(string S) // due to the recursive nature of the rope, this being the only constructor, and the fact that Build has to create a new Node somehow, we arrive at the conclusion that the constructor is going to be called within that recursion, and as such handles much of the recursion logic itself
    {
        totalLength = S.Length;
        if (totalLength <= maxLeafLength) //base case for recursion / leaf determination, a bonus of putting this in the constructor is handling inputs too small to break up is very easy
        { //we check base case first as technically the majority of nodes are leafs
            value = S;
            leftChild = rightChild = null;
        }
        else //if the given string still needs to be broken down some more
        {
            value = ""; //we use the empty string to represent parent nodes' lack of a value for this datamember, though as of writing this value is never used in a parent
            int splitIndex = (int)totalLength / 2; //this cast effectively rounds down, the result of this is if totalLength is odd then the right child always gets the extra char
            leftChild = Build(S, 0, splitIndex); //build basically returns whatever THIS constructor makes when the substring defined in the arguments is fed back into it
            rightChild = Build(S, splitIndex, totalLength); // TODO: explain why the end of leftChild should have the same value as the start of rightChild
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
        // TODO: implement method Concatenate (unclaimed)
        return null; //placeholder
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
    {
        // TODO: implement method Substring (unclaimed)
        return null; //placeholder
    }
    // (9 marks) Return the index of the first occurrence of S; -1 otherwise
    public int Find(string S)
    {
        // TODO: implement method Find (unclaimed)
        return -1; //placeholder
    }
    // (3 marks) Return the character at index i
    public char CharAt(int i)
    {
        // TODO: implement method CharAt (unclaimed)
        return '\0'; //placeholder
    }
    // (4 marks) Return the index of the first occurrence of character c
    public int IndexOf(char c)
    {
        // TODO: implement method IndexOf (unclaimed)
        return '\0'; //placeholder
    }
    // (5 marks) Reverse the string represented by the current rope
    public void Reverse()
    {
        // TODO: implement method Reverse (unclaimed)
    }
    // (1 mark) Return the length of the string
    public int Length()
    { //note: while there's nothing explicitely preventing a child node from having this called, a user has no way of accessing children of the ultimate root of the rope they created, so they can only call this from the ultimate root 
        return totalLength;
    }
    // (4 marks) Return the string represented by the current scope
    public override string ToString() //note: all objects have a (basic) ToString method, usually inherited, so this method is technically an override 
    {
        // TODO: implement method ToString (unclaimed)
        return null; //placeholder
    }
    // (4 marks) Print the augmented binary tree of the current rope
    public void PrintRope()
    {
        PrintRope(0, this); //we simply use the public method to initiate the private recursive function
        //using local function vs private method as the function exists only to be used by the parent method, so making usage by other methods possible is semantically incorrect
        void PrintRope(int indentation, Node root)
        {
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
                Console.WriteLine(indent+ "* " + root.value);
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
    public static void Main(string[] args) // NOTE: let's consider implementing accepting initial arguments, if we don't do that then remove parameters from main before submission
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
                        rope = new Rope(input[input.Length - 1]);
                    }
                    else if (NRp != null) //if path was given
                    {
                        // TODO: add file i/o support, note: we don't want to use file i/o within the rope class, so all that stuff must not leave the User class, save importing the libraries at the start of the file
                        Console.WriteLine(NRp); //placeholder
                    }
                    else //default case
                    {
                        Console.Write("Input new rope string :");
                        rope = new Rope(Console.ReadLine());
                    }
                    break;
                case "pr":
                case "print rope":
                    rope.PrintRope();
                    break;
                case "q":
                case "quit":
                case "exit":
                    run = false;
                    break;
                case "help":
                case "h":
                    Console.WriteLine(
                        "!: All subsequent arguments/options past command optional, order of options shown is only valid order of input\n" +
                        "-Help (h)\n" + //implemented
                        "-New Rope (nr) <string to use> || (nr) -p <path to file containing string>\n" + //semi-implemented
                        "-Insert Substring at Index (isi)\n" +
                        "-Delete Substring in Range (dsr)\n" +
                        "-Get Substring in Range (gsr)\n" +
                        "-Find first Substring (fs)\n" +
                        "-Get Character at Index (gci)\n" +
                        "-Get first Index of Character (gic)\n" +
                        "-Reverse Rope (rr)\n" +
                        "-Get Length (gl)\n" +
                        "-Translate To String (tts)\n" +
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