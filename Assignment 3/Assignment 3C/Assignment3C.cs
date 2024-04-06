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
                    p.keys[p.numKeys] = p.children[p.numKeys].keys[t - 1];
                    p.numKeys++;
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
            full.leaf = false;
            Node<T> copy = full;
            full = new Node<T>(t);
            Node<T> right = new Node<T>(t);
            for (int i = 0, j = 0, l = 0; i < 2 * t - 1; i++)
            {
                if (i < t - 1)
                {
                    full.keys[j] = copy.keys[i];
                    j++;
                }
                else if (i > t - 1)
                {
                    right.keys[l] = copy.keys[i];
                    l++;
                }
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
            if(curr.numKeys == 0) //we can't delete what's not there
            { //can only get here if we recurse into an empty node which can only happen if the value we're trying to delete isn't in the tree
                return false;
            }
            for(int i=0; i<curr.numKeys; i++)
            {
                int diff = k.CompareTo(curr.keys[i]);
                if(diff < 0)
                { //if we should recurse
                    return DeleteRecurse(curr.children[i]);
                }
                if(diff == 0) //hit
                {
                    //shift everything left and overwrite the location of the hit to delete
                    for(int j=i; j<curr.numKeys-1; j++)
                    {
                        curr.keys[j] = curr.keys[j + 1];
                        curr.children[j] = curr.children[j + 1];
                    }
                    curr.children[curr.numKeys-1] = curr.children[curr.numKeys];
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
            if (p.numKeys == 0) //if we've recursed into an empty Node it means the value can't be found
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
            if(curr.numKeys > 0) //unless there are no keys, we always want to add the 1st key, if there is only one key it will naturally become the root this way
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
                for(int i=0; i<=curr.numKeys; i++) // we actually want <= to ensure the last child which is at [numKeys] gets picked up
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
            String indent = new string('\t', indentation); //obscure overload of string that builds sequence of given char to given length
            if (!p.leaf)
            {
                PrintRecurse(p.children[p.numKeys], indentation + 1);
            }
            for (int i = p.numKeys - 1; i <= 0; i--)
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

    }
}