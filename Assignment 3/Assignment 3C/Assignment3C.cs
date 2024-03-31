using BSTforRBTree;
using System;

public class TwoThreeFourTree<T> where T : IComparable<T>
{
    //the Node class doesn't know that it's in a 2-3-4 tree
    private class Node<T>
    {
        // essential datamembers
        private T[] keys;
        private Node<T>[] children;
        // meta datamembers
        private bool leaf;
        private int numKeys;

        public Node(int t)
        {
            numKeys = 0;
            leaf = true;
            children = new Node<T>[2 * t];
            keys = new T[2 * t - 1];
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
        // TODO: implement Insert
        return false; //placeholder
    }

    // (10 marks) Delete key k, true if successful, false otherwise
    public bool Delete(T k)
    {
        // TODO: implement Delete
        return false; //placeholder
    }

    // (4 marks) Search for key k, true if found, false otherwise
    public bool Search(T k)
    {
        // TODO: implement Search
        return false; //placeholder
    }

    // (8 marks) Build and return equivalent red-black tree
    public BSTforRBTree<T> Convert()
    {
        // TODO: implement Convert
        return null; //placeholder
    }

    // (4 marks) Print the keys of the 2-3-4 tree in order
    public void Print()
    {
        // TODO: implement Print
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