﻿using System;
//implemented using Dr. Patrick's code for Binomial Heaps as starting point
//TODO: Ensure commenting and formatting is consistent with Matthew's coding (Ben)

namespace LazyBinomialHeap
{
    public class LazyBinomialNode<T>
    {
        public T Item { get; set; }
        public int Degree { get; set; }
        public LazyBinomialNode<T> Child { get; set; }
        public LazyBinomialNode<T> Sibling { get; set; }

        public LazyBinomialNode(T item)
        {
            Item = item;
            Degree = 0;
            Child = null;
            Sibling = null;
        }
    }
    public interface IContainer<T>
    {
    }

    public interface ILazyBinomialHeap<T> : IContainer<T> where T : IComparable
    {
        void Add(T item);
        void Remove();
        T Front();
        void Print();

    }
    public class LazyBinomialHeap<T> : ILazyBinomialHeap<T> where T : IComparable
    {
        private LazyBinomialNode<T>[] B; //B[k] is the head of the root list for binomial trees of size 2^k
        private LazyBinomialNode<T> highP; //Reference to the node (item with the highest priority)
        private int size;

        public LazyBinomialHeap()
        {
            //not sure if this is correct or if I should use a larger number
            B = new LazyBinomialNode<T>[10];
            highP = null;
            size = 0;
        }
        //TODO: Re-implement add (2 marks)
        //When an item is added to the Lazy Binomial Heap, it is placed in a new binomial
        //tree of size 1 and added to the root list at location B[0].  Hence, the length
        //of the root list at B[0] is equal to the number of additions
        //(assuming no removals in the interim)
        public void Add(T item)
        {
            var newNode = new LazyBinomialNode<T>(item);
            if (highP == null || newNode.Item.CompareTo(highP.Item) > 0)
                highP = newNode;
            if (B[0] == null)
            {
                B[0] = newNode;
            }
            else
            {
                newNode.Sibling = B[0];
                B[0] = newNode;
            }
            size++;
        }
        //TODO: Re-implement front (2 marks)
        public T Front()
        {
            if (highP == null)
                throw new Exception("High priority is currently null");
            return highP.Item;
        }
        //TODO: Re-implement remove (5 marks)
        //From Dr. Patrick's annoucement:
        //  The Remove method deletes the item with the highest priority.
        //  Suppose the item is found at the root of a binomial tree of size 2^k, i.e.
        //  found in a binomial tree in the root list of B[k].  When the root (item) is removed,
        //  the children of the root, by definition, are binomial trees of size  2^(k-1),  2^(k-2),  ... 1
        //  which are then stored in the root lists of  B[k-1],  B[k-2],  ... B[0], respectively.
        //  Only now does Remove call Coalesce.  The Add and Front methods never call Coalesce.
        public void Remove()
        {
            if (highP == null)
            {
                throw new Exception("Lazy heap is currently empty");
            }
            LazyBinomialNode<T> prev = null;
            LazyBinomialNode<T> curr = null; //start at the begining of the root

            //using for loop to traverse our lazy heap looking for the highest priority
            for (int i = 0; i < B.Length; i++)
            {
                curr = B[i];
                while (curr != null && !curr.Equals(highP))
                {
                    prev = curr;
                    curr = curr.Sibling;
                }
                if (curr != null && curr.Equals(highP))
                {
                    break; //ends for loop, as we have found highest
                }
            }
            if (curr == null) // if not found
            {
                throw new Exception("Item not found in Lazy heap");
            }
            if (prev != null)
            {
                prev.Sibling = curr.Sibling; //sets previous sibling to be current's
                //sibling, since current is the node we want to delete
            }
            else //this would be if the node we want to delete has no prior siblings
            {
                B[curr.Degree] = curr.Sibling;
            }


            //Move children to appropriate root lists if they exist
            LazyBinomialNode<T> child = curr.Child;
            while (child != null)
            {
                LazyBinomialNode<T> next = child.Sibling;
                child.Sibling = null;

                int degree = child.Degree;
                while (B[degree] != null)
                {
                    LazyBinomialNode<T> existing = B[degree];

                    if (child.Item.CompareTo(existing.Item) > 0)
                    {
                        // Swap child and existing
                        LazyBinomialNode<T> temp = child;
                        child = existing;
                        existing = temp;
                    }

                    // Link existing as child of child
                    existing.Sibling = child.Child;
                    child.Child = existing;
                    child.Degree++;

                    // Remove existing from B[degree]
                    B[degree] = null;
                    degree++;
                }
                B[degree] = child;
                child = next;
            }
            size--;
            // update highP before calling coalesce, updating highP here because it is used in coalesce
            //need to reavaluate this as it only takes into account the first element in each index
            //I.E. wont work as intended if we have 7-4-13 it will set 7 as highP 
            highP = null;
            for (int i = 0; i < B.Length; i++)
            {
                if (B[i] != null && (highP == null || B[i].Item.CompareTo(highP.Item) > 0))
                {
                    highP = B[i];
                }
            }
            //TODO: add check if the heap is empty we dont need coalesce
            if (highP != null)
            {
                Coalesce(); //only called by remove, cleans up the root mess left by add
            }
            else
            {
                return;
            }
        }
        //TODO: Implement Coalesce (9 marks)
        // The Coalesce method is similar to Consolidation.  Starting with binomial trees of size 1,
        // it combines pairs of trees of the same size until there is at most one instance of a
        // binomial tree of any given size.  Note that when two trees of size 2^k in the root list of B[k]
        // are combined, the resultant tree has size 2^(k+1) and is placed in the root list of B[k+1].
        private void Coalesce()
        {
            //setting up our pointers
            LazyBinomialNode<T> next, curr;
            //if the tree is empty, dont need to coalesce
            if (B.Length == 0)
                return;
            for (int i = 0; i < B.Length; i++) //check loop logic
            {
                if (B[i] == null) // if there is nothing at this index we dont need to do anything
                {
                    continue;
                }
                else if (B[i].Sibling == null) //if there is no sibling we don't need to do anything at this index
                {
                    continue;
                }
                else
                {
                    curr = B[i];
                    next = curr.Sibling;
                }
                //merge trees with same degree until each degree has at most one tree
                while (B[i] != null)
                {
                    //need a check for when to break this otherwise it doesn't work
                    if (B[i].Sibling != null)
                    {
                        //determine which node should become the root/parent
                        //if current has higherP make next the child
                        if (curr.Item.CompareTo(next.Item) > 0)
                        {
                            //make next a child of curr
                            curr.Sibling = next.Sibling;
                            curr.Child = next;
                            curr.Degree++;
                        }
                        else
                        {
                            //make curr a child of next
                            next.Child = curr;
                            curr = next;
                            curr.Degree++;
                        }
                        B[i] = null; // remove merged tree from root list
                    }
                    else //maybe dont need this just putting here because we may need stuff here
                    {
                        break;
                    }
                }
                if (B[curr.Degree] == null) // if the degree is empty we can put the new tree there no problem
                {
                    B[curr.Degree] = curr;
                }
                else
                {
                    B[curr.Degree].Sibling = curr; // need to look into this just a stop gap measure
                }
            }

        }
        //TODO: implement print (5 marks)
        //need to do it in a way that properly highlights the data structure
        public void Print()
        {
            for (int i = 0; i <B.Length; i++)
            {

            }
        }
    }
    //test class
    //TODO: update formatting to be in line with past assignments
    public class Test
    {
        public static void Main(string[] args)
        {
            LazyBinomialHeap<int> lazyheap = new LazyBinomialHeap<int>();
            bool TorF = false;

            while (!TorF)
            {
                Console.WriteLine("Enter operation (add, remove, print, front, and exit):");
                string toDo = Console.ReadLine().ToLower().Trim();

                switch (toDo)
                {
                    case "add":
                        //TODO: checks around user input
                        Console.WriteLine("Enter a value to add:");
                        int.TryParse(Console.ReadLine(), out int value);
                        lazyheap.Add(value);
                        break;
                    case "remove":
                        lazyheap.Remove();
                        break;
                    case "print":
                        lazyheap.Print();
                        break;
                    case "front":
                        Console.WriteLine("Current front is: " + lazyheap.Front());
                        break;
                    case "exit":
                        TorF = true;
                        break;
                    default:
                        Console.WriteLine("Invalid input please try again");
                        break;

                }
            }
        }
    }
}