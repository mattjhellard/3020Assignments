using System;
using System.Collections.Generic;
using System.Linq;

public class ServerGraph // Working on this today, see branch -MH
{
    // 3 marks
    private class WebServer
    {
        public string Name;
        public List<WebPage> P;

        public WebServer(string name)
        {
            Name = name;
            P = new List<WebPage>();
        }
    }
    private WebServer[] V;
    private bool[,] E;
    private int NumServers;

    // 2 marks
    // Create an empty server graph
    public ServerGraph()
    {
        //we want to start with the smallest sizes reasonably possible for an "empty" graph, will automatically grow as needed without user intervention
        V = new WebServer[1];
        E = new bool[1, 1];
        NumServers = 0; // observe that numServers is currently 0 while the length of V is 1, this discrepancy is why the NumServers data member is necessary
    }

    // 2 marks
    // Return the index of the server with the given name; otherwise return -1
    private int FindServer(string name)
    {
        for (int i = 0; i < NumServers; i++) //using NumServers here as it will often be smaller than V.Length and there will never be a server at an i>NumServers 
        {
            if (V[i].Name == name)
            {
                return i; //if the current server name matches the supplied name return the current index  
            }
        }
        return -1; //if the process gets here then there are no servers in the array with the supplied name, so return -1
    }

    // 3 marks
    // Double the capacity of the server graph with the respect to web servers
    private void DoubleCapacity()
    {
        // creating newV to populate with V values before replacing V with newV
        WebServer[] newV = new WebServer[V.Length * 2];
        // populating newV with V values
        for (int i = 0; i < V.Length; i++) //No need to iterate past V.Length, in fact we shouldn't as the index would go out of bounds
        {
            newV[i] = V[i]; //Note: because WebServer is a reference type you may consider messing with cloning to achieve this, however if you draw out what happens without cloning it actually makes more sense than with cloning for the context of servers
        }
        // we're going to hold off on reassigning V until E is also successfully prepared for reassignment, though V is now prepared at this point

        bool[,] newE = new bool[newV.Length, newV.Length]; // E will always be a square matrix with width/height equal to V, this is also why we worked with V first btw
        for (int ix = 0; ix < V.Length; ix++)
        { //again, no need to iterate past V.length and it is also a bad idea to do so
            for (int iy = 0; iy < V.Length; iy++) //classic nested for to traverse multidimensional array
            {
                newE[ix, iy] = E[ix, iy]; //because bools are NOT reference types there is no need (or option?) to consider cloning here 
            }
        }
        //At this point both V and E are ready to be reassigned
        V = newV;
        E = newE;
    }

    // 3 marks
    // Add a server with the given name and connect it to the other server
    // Return true if successful; otherwise return false
    public bool AddServer(string name, string other)
    {
        WebServer newServer; //we could avoid using newServer like this by wrapping everything after this if in an else but why do that? Isn't this nice as is? Certainly nicer than gratuitous wrapping in my opinion
        if(NumServers == 0) //handling first server add by just creating designated other before moving on
        {
            newServer = new WebServer(other);
            // because there are no pre-existing servers if the process gets here, and because the class is constructed with capacity for at least one server, we need not check if we need to first double our capacities
            V[0] = newServer;
        }
        if(FindServer(name) != -1) //checking that desired server name is unique
        { //this should come after adding the first server when starting from 0 servers so that the check can apply to the initial "other" server
            return false;
        }
        //adding designated server
        newServer = new WebServer(name);
        if (V.Length == NumServers) // if this is true then V is full and needs to have its capacity expanded
        {
            DoubleCapacity();
        }
        V[NumServers] = newServer; //recall that arrays are 0 indexxed but NumServers is not, i.e, if there were, say, 7 servers, placing a new one at [7] would place it in the 8th position
        NumServers++;
        //new server successfully created at this point
        //connecting new server to "other" server
        //utilizing included AddConnection method to reduce code redundancy, this technically forgoes information that could make the connection easier, namely the from server having an already known index (end of V), but I think the reduced redundancy is worth that price
        if (AddConnection(name, other))
        {
            return true; //if the method call was a success
        }
        else //this else isn't strictly necessary but improves readability well enough to justify the extra wrap in my opinion
        {
            return false; //if the method call was a failure
        }
    }

    // 3 marks
    // Add a webpage to the server with the given name
    // Return true if successful; other return false
    public bool AddWebPage(WebPage w, string name)
    {
        // PLACEHOLDER
        return false;
    }
    // 4 marks
    // Remove the server with the given name by assigning its connections
    // and webpages to the other server
    // Return true if successful; otherwise return false
    public bool RemoveServer(string name, string other)
    {
        // PLACEHOLDER
        return false;
    }

    // 3 marks
    // Add a connection from one server to another
    // Return true if successful; otherwise return false
    // Note that each server is connected to at least one other server
    public bool AddConnection(string from, string to)
    {
        int FromIndex = FindServer(from); //getting index of "from" server
        if(FromIndex == -1)
        {
            return false; //if supplied "from" server not found
        }
        int ToIndex = FindServer(to); //same as above but for "to" server
        if(ToIndex == -1)
        {
            return false;
        }
        //there's probably a way to do the above more efficiently (we're doing basically the same thing twice in a row), but it would probably also be very convoluted and this way makes specific error messages, for example, very easy to implement
        if(FromIndex == ToIndex)
        { //if a loop has been requested, we currently disallow loops as a server connecting to itself doesn't make sense
            return false;
        }
        E[FromIndex,ToIndex] = true;
        E[ToIndex,FromIndex] = true;
        //updating both locations on the matrix as this is an undirected graph and that's the simplest way of handling it with an adjacency matrix
        return true;
    }
    
    // 10 marks
    // Return all servers that would disconnect the server graph into
    // two or more disjoint graphs if ever one of them would go down
    // Hint: Use a variation of the depth-first search
    public string[] CriticalServers()
    {
        // PLACEHOLDER
        return null;
    }
    // 6 marks
    // Return the shortest path from one server to another
    // Hint: Use a variation of the breadth-first search
    public int ShortestPath(string from, string to)
    {
        // PLACEHOLDER
        return -1;
    }
    // 4 marks
    // Print the name and connections of each server as well as
    // the names of the webpages it hosts
    public void PrintGraph()
    {

    }
}

// 5 marks
public class WebPage
{
    public string Name { get; set; }
    public string Server { get; set; }
    public List<WebPage> E { get; set; }
    public WebPage(string name, string host)
    {
    
    }
    public int FindLink(string name)
    {
        // PLACEHOLDER
        return -1;
    }
}
public class WebGraph
{
    private List<WebPage> P;

    // 2 marks
    // Create an empty WebGraph
    public WebGraph()
    {

    }
    // 2 marks
    // Return the index of the webpage with the given name; otherwise return -1
    private int FindPage(string name)
    {
        // PLACEHOLDER
        return -1;
    }
    // 4 marks
    // Add a webpage with the given name and store it on the host server
    // Return true if successful; otherwise return false
    public bool AddPage(string name, string host)
    {
        // PLACEHOLDER
        return false;
    }
    // 8 marks
    // Remove the webpage with the given name, including the hyperlinks
    // from and to the webpage
    // Return true if successful; otherwise return false
    public bool RemovePage(string name)
    {
        // PLACEHOLDER
        return false;
    }
    // 3 marks
    // Add a hyperlink from one webpage to another
    // Return true if successful; otherwise return false
    public bool AddLink(string from, string to)
    {
        //PLACEHOLDER
        return false;
    }
    // 3 marks
    // Remove a hyperlink from one webpage to another
    // Return true if successful; otherwise return false
    public bool RemoveLink(string from, string to)
    {
        // PLACEHOLDER
        return false;
    }
    // 6 marks
    // Return the average length of the shortest paths from the webpage with
    // given name to each of its hyperlinks
    // Hint: Use the method ShortestPath in the class ServerGraph
    public float AvgShortestPaths(string name)
    {
        // PLACEHOLDER
        return 0f;
    }
    // 3 marks
    // Print the name and hyperlinks of each webpage
    public void PrintGraph()
    {

    }
}