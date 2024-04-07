using BSTforRBTree;
using System;

public class TwoThreeFourTree<T> where T : IComparable<T>
{
    //the Node class doesn't know that it's in a 2-3-4 tree specifically, but it knows it's a private member of a btree, so its datamembers are open to that tree
    private class Node<I>
    { // ^ we've chosen to use a separate type parameter for Node even though it will always ultimately be made the same as the outer tree class
        // essential datamembers
        public I[] keys;
        public Node<I>[] children;
        // meta datamembers
        public bool leaf; // TODO: check this datamember is used before submission, else, remove
        public int numKeys;

        public Node(int t)
        {
            numKeys = 0;
            leaf = true;
            children = new Node<I>[2 * t];
            keys = new I[2 * t - 1];
        }
    }

    private Node<T> root;
    private const int t = 2; //a 2-3-4 tree has t=2 by definition, so t should not be dynamic, we still have t because it's better form than throwing a bunch of twos all over the place

    // (2 marks) Initialize empty 2-3-4 tree
    public TwoThreeFourTree()
    {
        root = new Node<T>(t);
        //we don't need to set t as it's always 2
    }

    // (6 marks) Insert key k, true if successful, false otherwise
    public bool Insert(T k)
    {
        if (root.numKeys == 2 * t - 1)
        {
            //we have to separately define split-adjacent behavior between when root is full and another node is full as the required behavior is different
            Node<T> newRoot = new Node<T>(t);
            newRoot.keys[0] = root.keys[t - 1];
            newRoot.children[1] = Split(root);
            newRoot.children[0] = root;
            newRoot.numKeys = 1;
            newRoot.leaf = false;
            root = newRoot;
        }
        return InsertRecurse(root);

        bool InsertRecurse(Node<T> p)
        {
            //keep in mind we've already handled the case where root is full and since we ultimately start at root we can assume the starting node is never full (in recursions we'll handle full nodes before recursing into them)
            if (p.leaf)
            {
                //if we're at a leaf then the key should go somewhere in here, keep in mind it can't be full at this point
                for (int i = 0; i < p.numKeys; i++)
                {
                    int diff = k.CompareTo(p.keys[i]);
                    if (diff < 0)
                    {
                        for (int j = p.numKeys - 1; j > i; j--)
                        { //we're shifting the array at and above i right to make room for the new key
                            p.keys[j] = p.keys[j - 1];
                        }
                        //everything has been shifted right so i is now "unoccupied"
                        p.keys[i] = k;
                        p.numKeys++;
                        return true;
                    }
                    if (diff == 0)
                    { //if we find the key already in the tree then we should fail out
                        return false;
                    }
                }
                //if we get to the end of the (not full) leaf without finding a place for the key, it means the key goes at the end
                p.keys[p.numKeys] = k; //if there are already two keys then k goes at [2], recall zero indexing
                p.numKeys++;
                return true;
            }
            else //if we're not at a leaf we should traverse to one
            {
                for (int i = 0; i < p.numKeys; i++)
                {
                    int diff = k.CompareTo(p.keys[i]);
                    if (diff < 0)
                    {
                        if (p.children[i].numKeys == 2 * t - 1)
                        { //if node to descend to is full,
                            for (int j = p.numKeys - 1; j > i; j--)
                            { //make room for pulled up key,
                                p.keys[j] = p.keys[j - 1];
                                p.children[j] = p.children[j - 1];
                            }
                            p.keys[i] = p.children[i].keys[t - 1]; //place pulled up key
                            p.numKeys++;
                            p.leaf = false;
                            p.children[i + 1] = Split(p.children[i]); //make the split (pulled up key is dropped from lower table in the operation),
                            if (k.CompareTo(p.keys[i]) > 0)
                            { //if the pulled up key should be to the left of our key we increment i to traverse right rather than left of it
                                i++; //as we're about to return (after a recursion or many) we can abuse our index value like this wihtout concern
                            }
                        }
                        return InsertRecurse(p.children[i]);
                    }
                    if (diff == 0)
                    {
                        return false;
                    }
                }
                //if we should recurse into the final branch of the node
                if (p.children[p.numKeys].numKeys == 2 * t - 1)
                { //if final branch is full, note the logic here is slightly different from if previous branches are full
                    for (int j = 0; j < p.numKeys; j++)
                    { //make room for pulled up key, we shift left as we're pulling up to the end
                        p.keys[j] = p.keys[j + 1];
                        p.children[j] = p.children[j + 1];
                    }
                    p.keys[p.numKeys-1] = p.children[p.numKeys].keys[t - 1];
                    p.numKeys++;
                    p.leaf = false;
                    Node<T> splitRight = Split(p.children[p.numKeys]);
                    p.children[p.numKeys - 1] = p.children[p.numKeys];
                    p.children[p.numKeys] = splitRight;
                    if (k.CompareTo(p.keys[p.numKeys - 1]) < 0)
                    {
                        return InsertRecurse(p.children[p.numKeys - 1]);
                    }
                }
                return InsertRecurse(p.children[p.numKeys]);
            }
        }
        //returns right node resulting from split operation (passed node BECOMES left node) (we drop the key to pull up and assume the caller handles that)
        Node<T> Split(Node<T> full)
        {
            Node<T> copy = full;
            full = new Node<T>(t);
            Node<T> right = new Node<T>(t);
            //we originally tried a more generalized solution but we ran out of time so now we abuse the known properties of a full 2-3-4 tree (3 keys to handle, 4 children to handle (the children can be empty))
            full.keys[0] = copy.keys[0];
            full.children[0] = copy.children[0];
            full.children[1] = copy.children[1];
            full.numKeys = 1;
            if (full.children[0] != null || full.children[1] != null)
            {
                full.leaf = false;
            }
            right.keys[0] = copy.keys[2];
            right.children[0] = copy.children[2];
            right.children[1] = copy.children[3];
            right.numKeys = 1;
            if (right.children[0] != null || right.children[1] != null)
            {
                right.leaf = false;
            }
            return right;
        }
    }

    // (10 marks) Delete key k, true if successful, false otherwise
    public bool Delete(T k)
    {
        return DeleteRecurse(root);

        bool DeleteRecurse(Node<T> curr)
        {
            if (curr == null || curr.numKeys == 0) //we can't delete what's not there
            { //can only get here if we recurse into an empty node which can only happen if the value we're trying to delete isn't in the tree
                return false;
            }
            for (int i = 0; i < curr.numKeys; i++)
            {
                int diff = k.CompareTo(curr.keys[i]);
                if (diff < 0)
                { //if we should recurse
                    return DeleteRecurse(curr.children[i]);
                }
                if (diff == 0) //hit
                {
                    //shift everything left and overwrite the location of the hit to delete
                    for (int j = i; j < curr.numKeys; j++)
                    {
                        curr.keys[j] = curr.keys[j + 1];
                        curr.children[j] = curr.children[j + 1];
                    }
                    curr.children[curr.numKeys - 1] = curr.children[curr.numKeys];
                    curr.numKeys--;
                    return true;
                }
            }
            //if we get here then the only thing left to do is to recurse into the rightmost child
            return DeleteRecurse(curr.children[curr.numKeys]);
        }
    }

    // (4 marks) Search for key k, true if found, false otherwise
    public bool Search(T k)
    {
        return SearchRecurse(root);

        //using a local function so we can run recursively in a semantically sound way (a seperate private method that is only called by this method would be the alternative) and so we can call the recursion without re-passing k
        bool SearchRecurse(Node<T> p)
        {
            if ( p == null || p.numKeys == 0) //if we've recursed into an empty Node it means the value can't be found
            {
                return false;
            }
            for (int i = 0; i < p.numKeys; i++)
            {
                int diff = k.CompareTo(p.keys[i]);
                if (diff == 0) //hit at current key
                {
                    return true;
                }
                if (diff < 0) //if the given key is less than the currently checked key, 
                { //, then we should recurse into the child at i (in a visualization, that's the child immediately LEFT of the current key)
                    return SearchRecurse(p.children[i]);
                }
            }
            //if we escape the for loop then the only spot left to search is the final child
            return SearchRecurse(p.children[p.numKeys]);
        }
    }

    // (8 marks) Build and return equivalent red-black tree
    public BSTforRBTree<T> Convert()
    {
        BSTforRBTree<T> RBTree = new BSTforRBTree<T>();
        ConvertRecurse(root); //void function builds the tree recursively
        return RBTree;

        void ConvertRecurse(Node<T> curr)
        {
            if (curr.numKeys >= 2) //if there are 2 or 3 keys, we should make the second key the root
            {
                RBTree.Add(curr.keys[1], Color.BLACK);
            }
            if (curr.numKeys > 0) //unless there are no keys, we always want to add the 1st key, if there is only one key it will naturally become the root this way
            {
                RBTree.Add(curr.keys[0], (curr.numKeys == 2) ? Color.RED : Color.BLACK);
            }                           //^ ternary operator, ensures, in order to maintain balance, that in the event of there being two keys it goes in red
            if (curr.numKeys == 3)
            { //believe it or not but we only want to add a third key if there are three keys
                RBTree.Add(curr.keys[2], Color.BLACK);
            }
            //recursing if need be
            if (!curr.leaf)
            {
                for (int i = 0; i <= curr.numKeys; i++) // we actually want <= to ensure the last child which is at [numKeys] gets picked up
                {
                    ConvertRecurse(curr.children[i]);
                }
            }
        }
    }

    // (4 marks) Print the keys of the 2-3-4 tree in order
    public void Print()
    {
        PrintRecurse(root, 0);
        void PrintRecurse(Node<T> p, int indentation)
        { //we go right to left to accurately represent 90-degree counter-clockwise rotation of tree
            if (p == null)
            {
                return;
            }
            String indent = new string('\t', indentation); //obscure overload of string that builds sequence of given char to given length
            if (!p.leaf)
            {
                PrintRecurse(p.children[p.numKeys], indentation + 1);
            }
            for (int i = p.numKeys - 1; i >= 0; i--)
            {
                Console.WriteLine(indent + p.keys[i].ToString());
                if (!p.leaf)
                {
                    PrintRecurse(p.children[i], indentation + 1);
                }
            }
        }
    }
}

//we've pasted the whole namespace (minus Program), we considered compressing the namespace into just the primary class but the enum is needed externally for external add calls and the interface "stabilises" said calls
namespace BSTforRBTree
{

    public enum Color { RED, BLACK };       // Colors of the red-black tree

    public interface ISearchable<T>
    {
        void Add(T item, Color rb);
        void Print();
    }

    //-------------------------------------------------------------------------

    // Implementation:  BSTforRBTree

    public class BSTforRBTree<T> : ISearchable<T> where T : IComparable<T>
    {

        // Common generic node class for a BSTforRBTree

        private class Node
        {
            // Read/write properties

            public T Item;
            public Color RB;
            public Node Left;
            public Node Right;

            public Node(T item, Color rb)
            {
                Item = item;
                RB = rb;
                Left = Right = null;
            }
        }

        private Node root;

        public BSTforRBTree()
        {
            root = null;    // Empty BSTforRBTree
        }

        // Add 
        // Insert an item into a BSTforRBTRee
        // Duplicate items are not inserted
        // Worst case time complexity:  O(log n) 
        // since the maximum depth of a red-black tree is O(log n)

        public void Add(T item, Color rb)
        {
            Node curr;
            bool inserted = false;

            if (root == null)
                root = new Node(item, rb);   // Create a root
            else
            {
                curr = root;
                while (!inserted)
                {
                    if (item.CompareTo(curr.Item) < 0)
                    {
                        if (curr.Left == null)              // Empty spot
                        {
                            curr.Left = new Node(item, rb);
                            inserted = true;
                        }
                        else
                            curr = curr.Left;               // Move left
                    }
                    else
                        if (item.CompareTo(curr.Item) > 0)
                    {
                        if (curr.Right == null)         // Empty spot
                        {
                            curr.Right = new Node(item, rb);
                            inserted = true;
                        }
                        else
                            curr = curr.Right;          // Move right
                    }
                    else
                        inserted = true;                // Already inserted
                }
            }
        }

        public void Print()
        {
            Print(root, 0);                // Call private, recursive Print
            Console.WriteLine();
        }

        // Print
        // Inorder traversal of the BSTforRBTree
        // Time complexity:  O(n)

        private void Print(Node node, int k)
        {
            string s;
            string t = new string(' ', k);

            if (node != null)
            {
                Print(node.Right, k + 4);
                s = node.RB == Color.RED ? "R" : "B";
                Console.WriteLine(t + node.Item.ToString() + s);
                Print(node.Left, k + 4);
            }
        }
    }
}

public static class User
{
    public static void Main()
    {
        Console.WriteLine("3020 Assignment 3: C\nby\nMatthew Hellard\nBenjamin Macintosh\nInput (help) for commands listing\n");
        bool run = true;
        TwoThreeFourTree<int> tree = new TwoThreeFourTree<int>(); //we're exclusively using ints in main just to keep things simple, the code doesn't care what type is used as long as it implements the requisite interface(s)
        while (run)
        {
            Console.Write("Input :");
            String[] input = Console.ReadLine().Trim(' ').Split(' ');
            int k;
            switch (input[0].ToLower())
            {
                case "nt":
                case "new tree":
                    tree = new TwoThreeFourTree<int>();
                    break;
                case "insert":
                case "insert key":
                case "ik":
                    try
                    {
                        if (input.Length >= 2)
                        {
                            k = Convert.ToInt32(input[1]);
                        }
                        else
                        {
                            Console.Write("Input int to insert :");
                            k = Convert.ToInt32(Console.ReadLine());
                        }
                        if (!tree.Insert(k))
                        {
                            Console.WriteLine("!: Failed to insert new key " + k);
                        }
                    }
                    catch (Exception e)
                    {
                        if (e is FormatException || e is OverflowException)
                        {
                            Console.WriteLine("!: Invalid input");
                        }
                        else
                        { //we want to pass on the exception if it isn't one we expect
                            throw;
                        }
                    }
                    break;
                case "delete":
                case "delete key":
                case "dk":
                    try
                    {
                        if (input.Length >= 2)
                        {
                            k = Convert.ToInt32(input[1]);
                        }
                        else
                        {
                            Console.Write("Input int to delete :");
                            k = Convert.ToInt32(Console.ReadLine());
                        }
                        if (!tree.Delete(k))
                        {
                            Console.WriteLine("!: Failed to delete key " + k);
                        }
                    }
                    catch (Exception e)
                    {
                        if (e is FormatException || e is OverflowException)
                        {
                            Console.WriteLine("!: Invalid input");
                        }
                        else
                        { //we want to pass on the exception if it isn't one we expect
                            throw;
                        }
                    }
                    break;
                case "search":
                case "search for key":
                case "search key":
                case "sk":
                    try
                    {
                        if (input.Length >= 2)
                        {
                            k = Convert.ToInt32(input[1]);
                        }
                        else
                        {
                            Console.Write("Input key to find :");
                            k = Convert.ToInt32(Console.ReadLine());
                        }
                        if (!tree.Search(k))
                        {
                            Console.WriteLine("!: Failed to find key " + k);
                        }
                        else
                        {
                            Console.WriteLine("!: Found key " + k);
                        }

                    }
                    catch (Exception e)
                    {
                        if (e is FormatException || e is OverflowException)
                        {
                            Console.WriteLine("!: Invalid input");
                        }
                        else
                        { //we want to pass on the exception if it isn't one we expect
                            throw;
                        }
                    }
                    break;
                case "p":
                case "print":
                    tree.Print();
                    break;
                case "convert":
                case "c":
                    tree.Convert().Print(); //this print call runs on the rb tree that is made in the conversion
                    break;
                case "q":
                case "quit":
                case "exit":
                case "qq":
                case "qqq":
                    run = false;
                    break;
                case "help":
                case "h":
                case "?":
                    Console.WriteLine(
                        "!: All subsequent arguments past commands optional\n" +
                        "-Help (h)\n" + //implemented
                        "-New Tree (nt)\n" + // implemented
                        "-Insert Key (ik) <int to insert>\n" +
                        "-Delete Key (dk) <int to delete>\n" +
                        "-Search for Key (sk) <int to search for>\n" +
                        "-Convert to red-black tree (c)\n" +
                        "-Print current tree (p)\n" +
                        "-Quit (q)\n");
                    break;
                default:
                    Console.WriteLine("!: unknown input, use (help) for list of valid commands");
                    break;
            }
        }
    }
}