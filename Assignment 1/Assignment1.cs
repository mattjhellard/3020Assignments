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
