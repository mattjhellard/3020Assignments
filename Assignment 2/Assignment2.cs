using System;
using Node = Rope; // iirc "nodes" in this context are same as "ropes" and vice versa, this makes it official, can refactor away if need be

public class Rope
{
    // (5 marks) Create a balanced rope from a given string S
    public Rope(string S)
    {
        // TODO: implement constructor (unclaimed)
    }
    // Recursively build a balanced rope for S[i,j] and return its root
    private Node Build(string s, int i, int j)
    {
        // TODO: implement method Build (unclaimed)
        return null; //placeholder
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
    {
        // TODO: implement method Length (unclaimed)
        return -1; //placeholder
    }
    // (4 marks) Return the string represented by the current scope
    public override string ToString()
    {
        // TODO: implement method ToString (unclaimed)
        return null; //placeholder
    }
    // (4 marks) Print the augmented binary tree of the current rope
    public void PrintRope()
    {
        // TODO: implement method PrintRope (unclaimed)
    }
}

public static class User
{
    public static void Main(string[] args) // NOTE: let's consider implementing accepting initial arguments, if we don't do that then remove parameters from main before submission
    {
        Console.WriteLine("3020 Assignment 2\nby\nBenjamin Macintosh\nMatthew Hellard\nInput (help) for commands listing\n");
        // we want these variables to persist between loop iterations
        bool run = true;
        Rope rope;
        while (run)
        {
            Console.Write("Input :");
            //we tokenize the user input (space separated) to enable inputting all required data for a command at once, similar to any real CLI
            //like a CLI, we want to include options (ex: -l), but to make options work nicely we will restrict data inputs that start with '-' in some contexts to protect from likely data entry mistakes
            // TODO: IF it proves more work than it's worth to implement decently, remove options functionality, disregard this otherwise
            String[] input = Console.ReadLine().Split(' ');
            switch (input[0].ToLower()) //we don't want to change case of whole input as data inputted should allow for case-sensitivity, but command (first substring) can ignore this and modify case for convenience
            {
                case "nr":
                case "new rope":
                    if (input.Length == 2 && input[1][0] != '-') //idk if this is common knowledge but apparently c# still treats strings like arrays of chars to the point where you can reference chars at an index in a string with []
                    { //in the case of a user inputting something like "nr -p" with no actual argument given post-option, we want to prevent the option becoming the input
                        rope = new Rope(input[1]);
                    }
                    else if (input.Length == 3 && input[1].Equals("-p")) //-p means path, we will try to find a matching file and use it's contents as the new string input
                    {//TODO: IF file input not implemented remove this whole block before submission
                        // TODO: (optional) implement file input functionality (unclaimed)
                    }
                    else //consider this the default case, if options/arguments past command ultimately fail or aren't given we default to more typical input failure
                    {
                        Console.Write("!: Input new rope string :");
                        rope = new Rope(Console.ReadLine());
                    }
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
                        "-New Rope ((nr) -> <string to use> || -p <path to file containing string>)\n" + //semi-implemented
                        "-Insert Substring at Index (isi)\n" +
                        "-Delete Substring in Range (dsr)\n" +
                        "-Get Substring in Range (gsr)\n" +
                        "-Find first Substring (fs)\n" +
                        "-Get Character at Index (gci)\n" +
                        "-Get first Index of Character (gic)\n" +
                        "-Reverse Rope (rr)\n" +
                        "-Get Length (gl)\n" +
                        "-Translate To String (tts)\n" +
                        "-Print Rope (pr)\n" +
                        "-Quit (q)\n"); //implemented
                    break;
                default:
                    Console.WriteLine("!: unknown input, use (help) for list of valid commands");
                    break;
            }
        }
    }
}