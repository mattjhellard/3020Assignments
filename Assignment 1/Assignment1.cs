using System.Collections.Generic;

public class ServerGraph // Working on this today, see branch -MH
{
    // 3 marks
    private class WebServer
    {
        public string Name;
        public List<WebPage> P;
    }
    private WebServer[] V;
    private bool[,] E;
    private int NumServers;

    // 2 marks
    // Create an empty server graph
    public ServerGraph()
    {

    }
    // 2 marks
    // Return the index of the server with the given name; otherwise return -1
    private int FindServer(string name)
    {
        // PLACEHOLDER:
        return -1;
    }
    // 3 marks
    // Double the capacity of the server graph with the respect to web servers
    private void DoubleCapacity()
    {

    }
    // 3 marks
    // Add a server with the given name and connect it to the other server
    // Return true if successful; otherwise return false
    public bool AddServer(string name, string other)
    {
        // PLACEHOLDER:
        return false;
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
        // PLACEHOLDER
        return false;
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
        Name = name;
        Server = host;
        E = new List<WebPage>();
    }
    public int FindLink(string name)
    {
        if (E.Count == 0) //if E is empty we can give up the search immediately, we can also avoid potential errors from dealing with an empty list
        {
            return -1;
        }
        //by using predicates we can simplify how we interact with the List E, note that this way of doing things isn't an upgrade in efficiency, the list is still ultimately traversed one thing at a time until the desired result is found
        bool findName(WebPage page) { return page.Name == name; }
        return E.FindIndex(findName); // using the above local function, FindIndex will return the first index that returns true,or -1 if none of them do
    }
}
public class WebGraph // Getting started on this, but feel free to jump in on it -MH
{
    private List<WebPage> P;

    // 2 marks
    // Create an empty WebGraph
    public WebGraph()
    {
        P = new List<WebPage>();
    }

    // 2 marks
    // Return the index of the webpage with the given name; otherwise return -1
    private int FindPage(string name)
    {
        bool findName(WebPage page) { return page.Name == name; }
        return P.FindIndex(findName); // using the above local function, FindIndex will return the first index that returns true,or -1 if none of them do
    }

    // 4 marks
    // Add a webpage with the given name and store it on the host server
    // Return true if successful; otherwise return false
    public bool AddPage(string name, string host)
    {
        if (FindPage(name) != -1)
        { //if the page name already exists,
            return false; //since we don't want duplicate names
        }
        WebPage newPage = new WebPage(name, host);
        P.Add(newPage);
        return true;
        //IMPORTANT NOTE: The webpage isn't actually connected to the server yet, as there isn't really a proper way of doing that here
        //The handler of the specific ServerGraph and WebGraph objects MUST immediately turn around and make the connection in ServerGraph
    }

    // 8 marks
    // Remove the webpage with the given name, including the hyperlinks
    // from and to the webpage
    // Return true if successful; otherwise return false
    public bool RemovePage(string name)
    {
        if (P.Count == 0)
        {
            return false; //if there are no webpages then we shouldn't bother doing all the work to delete one
        }
        int PageIndexToRemove = FindPage(name); //creating variable outside following check so we can use the value later if found
        if (PageIndexToRemove == -1)
        {
            return false; //if the specified webpage doesn't exist
        }
        //first checking for and removing links TO the page, this will technically also remove loops should any exist when it passes the webpage to delete
        foreach (WebPage page in P)
        {
            RemoveLink(page,name); //using overloaded RemoveLink, see after the regular method for functionality
        }
        //deleting the page itself, this should take the outgoing hyperlinks with it
        P.RemoveAt(PageIndexToRemove);
        return true;
    }

    // 3 marks
    // Add a hyperlink from one webpage to another
    // Return true if successful; otherwise return false
    public bool AddLink(string from, string to)
    {
        int FromIndex = FindPage(from);
        if(FromIndex == -1)
        {
            return false; //if the from page wasn't found
        }
        int ToIndex = FindPage(to);
        if(ToIndex == -1)
        {
            return false; //if the to page wasn't found
        }
        P[FromIndex].E.Add(P[ToIndex]); //adding the to page to the list of pages on the from page
        return true;
    }

    // 3 marks
    // Remove a hyperlink from one webpage to another
    // Return true if successful; otherwise return false
    public bool RemoveLink(string from, string to)
    {
        int FromIndex = FindPage(from);
        if (FromIndex == -1)
        {
            return false; //if from index not found then we can't proceed
        }
        WebPage page = P[FromIndex];
        RemoveLink(page,to);
        return true;
    }

    // This custom private overloaded RemoveLink lets the presets RemoveLink and RemovePage minimize redundancy between eachother without sacrificing too much efficiency from either
    private void RemoveLink(WebPage page, string to)
    {
        int ToIndex;
        while ((ToIndex = page.FindLink(to)) != -1) //we basically check repeatedly for the specified hyperlink and then remove it if found, then stop when no more found
        { //this while loop makes this method compatible with parallel edges but that probably isn't necessary IIRC, can replace while with a single if once we're certain we won't be handling parallel edges
            page.E.RemoveAt(ToIndex);
        } //note that by doing things like this we don't return false if the page to delete isn't found, I think this is okay because the page being deleted or the page never existing is the same thing once the method is finished
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