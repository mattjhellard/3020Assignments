using System;

//Pseudo Point Quadtree (not actually a proper implementation of a point quadtree)
public class PPQTree<T> where T : IComparable<T>
{
    private T[] tuple; //<^ we don't really care what the datatype we're working with is, as long as it's comparable to itself
    public PPQTree(T[] values)
    {
        tuple = values;
    }
    public int Length
    {
        get { return tuple.Length; }
    }
    public T this[int index] //this allows us to refer to our private array indirectly, keeping it obscured
    {
        get
        {
            if (index < 0 || index >= tuple.Length)
            {
                throw new ArgumentOutOfRangeException();
            }
            return tuple[index];
        }
    }
    public int Compare(PPQTree<T> newTuple)
    {
        if (newTuple.Length != this.tuple.Length)
        {
            throw new ArgumentOutOfRangeException("Cannot compare differently sized tuples");
        }
        // Core competency of the program ----------------------------
        int result = 0; //we start at 0 to 0 index (1st path is 0, nth path is n-1)
        for (int i = 0; i < this.tuple.Length; i++)
        {
            if (newTuple[i].CompareTo(this[i]) > 0)
            {
                result += TwoToPower(this.tuple.Length - 1 - i); //< in the given example, the first value acts as the most significant bit, it is logical, but means we need to a little more work to convert the binary to an int
                //^ if we are comparing d-dimensional tuples then there are 2^d potential paths,
                //in an unsigned d-bit int, there are 2^d potential values,
                //in an unsigned d-bit int, 0 is represented by a sequence of d 0s
                //in a d-dimensional tuple, the leftmost path (0) would be taken if all d coordinates are less than
                //in an unsigned d-bit int, the largest possible value would be a sequence of d 1s
                //in a d-dimensional tuple, the rightmost path is taken when all d coordinates are greater than
                //you convert a binary integer to a "real" integer by taking the summation of all 2^i in the sequence where the value at i is 1
                //TLDR: the path to take is the same as the integer conversion from the binary sequence of comparison results
            }
        }
        return result;
        // End of core competency of the program ---------------------

        //local function to do exponents, since we're exclusively doing powers of two, it made more sense to create a special local function than to link in a generalized exponent method
        int TwoToPower(int power)
        {
            if (power == 0)
            { //its good that r^0 = 1 for our actually important method, but there isn't a great way of representing this in an exponent algorithm, at least not one that only does integer powers, so we just make a special case for it 
                return 1;
            }
            //relegating power==0 to a special case also lets our general function be implemented very easily
            int value = 2;
            for (int i = 1; i < power; i++)
            {
                value *= 2;
            }
            return value;
        }
    }
}
public class User
{
    public static void Main()
    {
        // Our program is so short and narrow that it's faster to manually modify these values in the source code and recompile for each test rather than bother with any kind of runtime UI
        PPQTree<int> oldTuple = new PPQTree<int>(new[] { 3, 2, 3 });
        PPQTree<int> newTuple = new PPQTree<int>(new[] { 2, 3, 4 });
        Console.WriteLine(oldTuple.Compare(newTuple));
        Console.ReadLine();
    }
}