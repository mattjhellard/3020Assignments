using System;
// implemented using Dr. Patrick's code for Binomial Heaps as starting point especially the data structure and Binomial Link method
// TODO: Ensure commenting and formatting is consistent with Matthew's coding (Ben)

namespace LazyBinomialHeap
{
    // Structure for our "Lazy" nodes (same as regular Binomial Nodes)
    // Left-Child, Right-Sibling implementation
    public class LazyBinomialNode<T>
    {
        public T Item { get; set; }
        public int Degree { get; set; }
        public LazyBinomialNode<T> Child { get; set; }
        public LazyBinomialNode<T> Sibling { get; set; }

        // Constructor
        public LazyBinomialNode(T item)
        {
            Item = item;
            Degree = 0;
            Child = null;
            Sibling = null;
        }
    }
   //--------------------------------------------------------------------------------------------------------------------------------
    // Used Dr.Patrick's code class from but removed MakeEmpty() since it was not used
    public interface IContainer<T> // Common Interface for all non-linear data structures
    {
        bool Empty(); //Test if an instance is empty
        int Size();  //Return the number of items in an instance
    }

    public interface ILazyBinomialHeap<T> : IContainer<T> where T : IComparable
    {
        void Add(T item); // Add an item to the binomial heap at B[0]
        void Remove();    // Remove the item of the highest priority
        T Front();        // Returns the item with the highest priority
        void Print();     // Prints the Binomial Heap

    }
    //-------------------------------------------------------------------------------------------------------------------------------
   // Lazy Binomial Heap (variation of Dr. Patrick's Binomial Heap from Class)
   // Implementation: Child (left-child), Sibling (right-sibling)
    public class LazyBinomialHeap<T> : ILazyBinomialHeap<T> where T : IComparable
    {
        private LazyBinomialNode<T>[] B;    // B[k] holds binomial trees of size 2^k
        private LazyBinomialNode<T> highP;  // Reference to the node (item with the highest priority)
        private int size;                   // Size of the lazy binomial heap

        // Constructor
        public LazyBinomialHeap()
        {
            
            B = new LazyBinomialNode<T>[10]; //Each index holds 2^k values, with the exception of B[0] until remove is called
            for (int i = 0; i<B.Length; i++)
            {
                B[i] = new LazyBinomialNode<T>(default(T)); //need to set default values
            }
            highP = null;
            size = 0;
        }
        //Re-implementation of Add (2 marks)
        //Pretty similar to Dr. Patrick's method, but updating/checking highP as nodes are added
        public void Add(T item)
        {
            LazyBinomialNode<T> newNode = new LazyBinomialNode<T>(item);
            newNode.Sibling = B[0].Sibling;
            B[0].Sibling = newNode;
            if (highP == null || newNode.Item.CompareTo(highP.Item) > 0)
                highP = newNode;
            size++;
        }
        //Re-implementation front (2 marks)
        // Done in constant time thanks to highP
        public T Front()
        {
            if (highP == null)
                throw new Exception("The Heap is currently empty");
            return highP.Item;
        }
        // Re-implementation of remove (5 marks)
        // Uses Dr. Patrick's Remove for Binomial Heap as a starting point
        // Removes the highest priority node from the heap, places children of the node into their respective indices in B
        // Then calls Coalesce() and UpdateHighP()
        public void Remove()
        {
            if (Empty())
            {
                throw new InvalidOperationException("!: Error the heap is currently empty");
            }
            else
            {
                LazyBinomialNode<T> children = new LazyBinomialNode<T>(default(T));
                LazyBinomialNode<T> p;
                LazyBinomialNode<T> q = new LazyBinomialNode<T>(default(T));
                int cost = 0;
                bool found = false;

                //loop to find highP in heap
                for (int i = 0; i < B.Length; i++)
                {
                    q = B[i];
                    while (q.Sibling != null && !found)
                    {
                        if (q.Sibling == highP)
                        {
                            found = true;
                        }
                        else
                        {
                            q = q.Sibling;
                        }
                    }
                    if (found)
                    {
                        break;
                    }
                }
                p = q.Sibling; // highest priority item
                q.Sibling = q.Sibling.Sibling; // cut high P

                //Add binomial subtrees of p in reverse order into H
                p = p.Child; 
                while (p != null)
                {
                    q = p.Sibling;

                    //Splice P into children
                    p.Sibling = children.Sibling;
                    children.Sibling = p;
                    cost++;
                    p = q;   
                }
                LazyBinomialNode<T> kids = children.Sibling;
                
                // Since we don't have merge we need to put the children into the proper spots
                for (int i = 0; i<cost; i++)
                {
                    LazyBinomialNode<T> morekids = kids;
                    kids = kids.Sibling;
                    morekids.Sibling = B[i].Sibling;
                    B[i].Sibling = morekids;
                }
                size--;
                Coalesce();     //Combines trees of same degree
                UpdateHighP();  //Method to update highP (keeping front constant time      
            }
        }
        // Re-implementation of Coalesce (9 marks)
        // Uses Dr. Patrick's Consolidate as a starting point
        // Coalesces (combines) binomial trees at the same index in B
        private void Coalesce()
        {
            if (Empty())
            {
                return;
            }

            // Declare nodes for traversal and placement 
            LazyBinomialNode<T> curr, first, second; 
            
            // Loop through heap
            // When we get to the max size of B (B[9] in this case) we have nowhere else to put nodes
            // so we stop at Length - 2 so that we can still access B[9] but not exceed
            for (int i = 0; i < B.Length - 2; i++)
            {
                curr = B[i];

                // Similar to loop in Dr. Patrick's Consolidate but we don't need to compare degrees
                while (curr.Sibling != null && curr.Sibling.Sibling != null)
                {
                    first = curr.Sibling;
                    second = first.Sibling;

                    // Remove first and second by replacing with third (second sibling)
                    B[i].Sibling = second.Sibling;

                    // Combine tree based on priority
                    // When combining two trees of size 2^k we get a tree of size 2^(k+1)
                    // If first has a higher priority, second becomes its child or vice versa
                    if (first.Item.CompareTo(second.Item) >= 0)
                    {     
                        LazyBinomialLink(second, first);
                        first.Sibling = B[i + 1].Sibling;
                        B[i + 1].Sibling = first;         
                    }
                    else
                    {                      
                        LazyBinomialLink(first, second);                                             
                        second.Sibling = B[i + 1].Sibling;
                        B[i + 1].Sibling = second;                                              
                    }
                }
            }
        }
        // LazyBinomial Link
        // Makes child the child of root and returns root
        // Uses Dr. Patrick's Link from Binomial Heap code
        private void LazyBinomialLink(LazyBinomialNode<T> child, LazyBinomialNode<T> root) 
        {
            child.Sibling = root.Child;
            root.Child = child;
            root.Degree++;
        }
        // Called by remove after Coalesce(), used to updated highP to the highest priority item in the heap
        // Only need to search the first node at each index 
        private void UpdateHighP()
        {
            highP = null;
            for (int i = 0; i < B.Length; i++)
            {
                if (B[i].Sibling == null)
                {
                    continue;
                }
                else if (highP == null || B[i].Sibling.Item.CompareTo(highP.Item) > 0)
                {
                    highP = B[i].Sibling;
                }
            }
        }
        // Re-implementation of print (5 marks)
        // Recursively calls private print
        public void Print()
        {
            if (Empty())
            {
                Console.WriteLine("!: The lazy heap is currently empty");
            }
            for (int i = 0; i < B.Length; i++)
            {
                if (B[i].Sibling == null)
                {
                    continue;
                }

                Console.WriteLine("!: Tree at index B[{0}] of Size 2^{0}", i);
                // Calls private print to recursively print
                Print(B[i].Sibling);
            }
        }


        // Private Print
  
        private void Print(LazyBinomialNode<T> node)
        {
            Console.WriteLine(node.Item);
            if (node.Sibling != null)
            {
                Console.Write(node.Item + " Has Sibling: ");
                Print(node.Sibling);
            }
            if (node.Child != null)
            {
                Console.Write (node.Item + " Has Child: ");
                Print(node.Child);
            }
            
        }
        // Returns true if the binomial heap is empty, false otherwise
        public bool Empty()
        {
            return size == 0;
        }
        // Returns number of items in heap
        public int Size()
        {
            return size;
        }
    }
    //--------------------------------------------------------------------------------------------------------------------------------
    
    // Used by class lazyBinomialHeap<T> (from Dr. Patrick's code on Binomial Heaps)
    // Implements IComparale and overrides ToSting (from Object)
    public class PriorityClass : IComparable
    {
        private int priorityValue;
        private char letter;

        public PriorityClass(int priority, char letter)
        {
            this.letter = letter;
            priorityValue = priority;
        }

        public int CompareTo(Object obj)
        {
            PriorityClass other = (PriorityClass)obj;    // Explicit cast
            return priorityValue - other.priorityValue;  // High values have higher priority
        }

        public override string ToString()
        {
            return letter.ToString() + " with priority " + priorityValue;
        }
    }
    //--------------------------------------------------------------------------------------------------------------------------------
    // test class
    // Formating of main and user prompts now more in line with other submissions
    public class Test
    {
        public static void Main()
        {
            LazyBinomialHeap<int> lazyheap = new LazyBinomialHeap<int>();
            bool run = true;

            Console.WriteLine("3020 Assignment 3 Part B\nInput help for commands\n");
            while (run)
            {
                Console.Write("!: Input :");
                string[] toDo = Console.ReadLine().Trim(' ').Split(' ');
                    switch (toDo[0].ToLower())
                    {
                        case "?":
                        case "h":
                        case "help":
                            Console.WriteLine(
                                "!: All options :\n" +
                                "-Help  (?), (h), or (help) for all commands\n" +
                                "-Add (+), (a), or (add) to add a new integer to the heap\n" +
                                "-Remove (-), (r) or (remove) to remove the item of highest priority from the heap\n" +
                                "-Print (p) or (print) to print the heap\n" +
                                "-Front (f) or (front) to print the item of highest priority in the heap\n" +
                                "-Exit (e), (q), (exit), or (quit) to quit the program\n"
                                );
                            break;
                        case "+":
                        case "a":
                        case "add":
                            string iString;
                            int value;
                            if (toDo.Length == 2)
                            {
                            iString = toDo[1];
                            value = Convert.ToInt32(iString);
                            }
                            else
                            {
                                Console.WriteLine("!: Enter an integer to add :");
                                int.TryParse(Console.ReadLine(), out value);
                            }
                            lazyheap.Add(value);
                            break;
                        case "-":
                        case "r":
                        case "remove":
                            lazyheap.Remove();
                            break;
                        case "p":
                        case "print":
                            lazyheap.Print();
                            break;
                        case "f":
                        case "front":
                            Console.WriteLine("!: Current highest priority in the heap is: " + lazyheap.Front());
                            break;
                        case "e":
                        case "q":
                        case "exit":
                        case "quit":
                            run = false;
                            break;
                        default:
                            Console.WriteLine("!: Unknown Input, use (help) for list of commands");
                            break;

                    }
            }         
        }
    }
}
