using System.Text.Json;
using QuikGraph;
using QuikGraph.Graphviz.Dot;

namespace Lab3Apps;
public class SearchResult
{
    public required Node Node { get; init; }
    public int Comparisons { get; init; }
}
public enum NodeColor { Red, Black }

public class Node
{
    public int Key { get; set; }
    public string Data { get; set; }
    public NodeColor Color { get; set; }
    public Node Left { get; set; }
    public Node Right { get; set; }
    public Node Parent { get; set; }

    public Node(int key, string data)
    {
        Key = key;
        Data = data;
        Color = NodeColor.Red;
        Left = null!;
        Right = null!;
        Parent = null!;
    }
}

public class RedBlackTree
{
    public Node NullNode => _nullNode;
    private Node _root;
    private readonly Node _nullNode;

    public RedBlackTree()
    {
        _nullNode = new Node(0, null!) { Color = NodeColor.Black };
        _root = _nullNode;
    }
    
    public void Insert(int key, string data)
    {
        Node node = new(key, data)
        {
            Left = _nullNode,
            Right = _nullNode,
            Parent = null
        };

        Node parent = null;
        Node current = _root;

        while (current != _nullNode)
        {
            parent = current;
            if (node.Key < current.Key)
            {
                current = current.Left;
            }
            else if (node.Key > current.Key)
            {
                current = current.Right;
            }
            else
            {
                throw new ArgumentException("Елемент з таким ключом вже існує.");
            }
        }

        node.Parent = parent;

        if (parent == null)
        {
            _root = node;
        }
        else if (node.Key < parent.Key)
        {
            parent.Left = node;
        }
        else
        {
            parent.Right = node;
        }

        node.Color = NodeColor.Red;

        InsertFix(node);
    }
    
    public void Delete(int key)
    {
        Node nodeToDelete = Search(key).Node;
        if (nodeToDelete == _nullNode)
        {
            throw new KeyNotFoundException("Ключ не знайдено.");
        }

        Node y = nodeToDelete;
        NodeColor yOriginalColor = y.Color;
        Node x;

        if (nodeToDelete.Left == _nullNode)
        {
            x = nodeToDelete.Right;
            Transplant(nodeToDelete, nodeToDelete.Right);
        }
        else if (nodeToDelete.Right == _nullNode)
        {
            x = nodeToDelete.Left;
            Transplant(nodeToDelete, nodeToDelete.Left);
        }
        else
        {
            y = Minimum(nodeToDelete.Right);
            yOriginalColor = y.Color;
            x = y.Right;

            if (y.Parent == nodeToDelete)
            {
                x.Parent = y;
            }
            else
            {
                Transplant(y, y.Right);
                y.Right = nodeToDelete.Right;
                y.Right.Parent = y;
            }

            Transplant(nodeToDelete, y);
            y.Left = nodeToDelete.Left;
            y.Left.Parent = y;
            y.Color = nodeToDelete.Color;
        }

        if (yOriginalColor == NodeColor.Black)
        {
            DeleteFix(x);
        }
    }

    private void DeleteFix(Node x)
    {
        while (x != _root && x.Color == NodeColor.Black)
        {
            if (x == x.Parent.Left)
            {
                Node w = x.Parent.Right;

                if (w.Color == NodeColor.Red)
                {
                    w.Color = NodeColor.Black;
                    x.Parent.Color = NodeColor.Red;
                    LeftRotate(x.Parent);
                    w = x.Parent.Right;
                }

                if (w.Left.Color == NodeColor.Black && w.Right.Color == NodeColor.Black)
                {
                    w.Color = NodeColor.Red;
                    x = x.Parent;
                }
                else
                {
                    if (w.Right.Color == NodeColor.Black)
                    {
                        w.Left.Color = NodeColor.Black;
                        w.Color = NodeColor.Red;
                        RightRotate(w);
                        w = x.Parent.Right;
                    }
                    
                    w.Color = x.Parent.Color;
                    x.Parent.Color = NodeColor.Black;
                    w.Right.Color = NodeColor.Black;
                    LeftRotate(x.Parent);
                    x = _root;
                }
            }
            else
            {
                Node w = x.Parent.Left;

                if (w.Color == NodeColor.Red)
                {
                    w.Color = NodeColor.Black;
                    x.Parent.Color = NodeColor.Red;
                    RightRotate(x.Parent);
                    w = x.Parent.Left;
                }

                if (w.Right.Color == NodeColor.Black && w.Left.Color == NodeColor.Black)
                {
                    w.Color = NodeColor.Red;
                    x = x.Parent;
                }
                else
                {
                    if (w.Left.Color == NodeColor.Black)
                    {
                        w.Right.Color = NodeColor.Black;
                        w.Color = NodeColor.Red;
                        LeftRotate(w);
                        w = x.Parent.Left;
                    }
                    
                    w.Color = x.Parent.Color;
                    x.Parent.Color = NodeColor.Black;
                    w.Left.Color = NodeColor.Black;
                    RightRotate(x.Parent);
                    x = _root;
                }
            }
        }
        x.Color = NodeColor.Black;
    }
    
    public SearchResult Search(int key)
    {
        int comparisons = 0;
        Node resultNode = SearchHelper(_root, key, ref comparisons);
        return new SearchResult { Node = resultNode, Comparisons = comparisons };
    }

    private Node SearchHelper(Node node, int key, ref int comparisons)
    {
        comparisons++;
        if (node == _nullNode || key == node.Key)
        {
            return node;
        }

        if (key < node.Key)
        {
            return SearchHelper(node.Left, key, ref comparisons);
        }
        else
        {
            return SearchHelper(node.Right, key, ref comparisons);
        }
    }
    
    public List<Node> GetAllRecords()
    {
        List<Node> nodes = new();
        InOrderTraversal(_root, nodes);
        return nodes;
    }
    
    public void SaveToFile(string filePath)
    {
        var nodes = GetAllRecords();
        var records = new List<Record>();

        foreach (var node in nodes)
        {
            records.Add(new Record { Key = node.Key, Data = node.Data });
        }

        var json = JsonSerializer.Serialize(records);
        File.WriteAllText(filePath, json);
    }
    
    public void LoadFromFile(string filePath)
    {
        if (!File.Exists(filePath))
            return;

        var json = File.ReadAllText(filePath);
        var records = JsonSerializer.Deserialize<List<Record>>(json);

        if (records != null)
        {
            foreach (var record in records)
            {
                Insert(record.Key, record.Data);
            }
        }
    }
    
    public void Clear()
    {
        _root = _nullNode;
    }

    private void InsertFix(Node node)
    {
        while (node.Parent != null && node.Parent.Color == NodeColor.Red)
        {
            if (node.Parent == node.Parent.Parent.Left)
            {
                Node uncle = node.Parent.Parent.Right;

                if (uncle != null && uncle.Color == NodeColor.Red)
                {
                    node.Parent.Color = NodeColor.Black;
                    uncle.Color = NodeColor.Black;
                    node.Parent.Parent.Color = NodeColor.Red;
                    node = node.Parent.Parent;
                }
                else
                {
                    if (node == node.Parent.Right)
                    {
                        node = node.Parent;
                        LeftRotate(node);
                    }
                    
                    node.Parent.Color = NodeColor.Black;
                    node.Parent.Parent.Color = NodeColor.Red;
                    RightRotate(node.Parent.Parent);
                }
            }
            else
            {
                Node uncle = node.Parent.Parent.Left;

                if (uncle != null && uncle.Color == NodeColor.Red)
                {
                    node.Parent.Color = NodeColor.Black;
                    uncle.Color = NodeColor.Black;
                    node.Parent.Parent.Color = NodeColor.Red;
                    node = node.Parent.Parent;
                }
                else
                {
                    if (node == node.Parent.Left)
                    {
                        node = node.Parent;
                        RightRotate(node);
                    }
                    
                    node.Parent.Color = NodeColor.Black;
                    node.Parent.Parent.Color = NodeColor.Red;
                    LeftRotate(node.Parent.Parent);
                }
            }
        }

        _root.Color = NodeColor.Black;
    }

    private void LeftRotate(Node x)
    {
        Node y = x.Right;
        x.Right = y.Left;

        if (y.Left != _nullNode)
        {
            y.Left.Parent = x;
        }

        y.Parent = x.Parent;

        if (x.Parent == null)
        {
            _root = y;
        }
        else if (x == x.Parent.Left)
        {
            x.Parent.Left = y;
        }
        else
        {
            x.Parent.Right = y;
        }

        y.Left = x;
        x.Parent = y;
    }

    private void RightRotate(Node x)
    {
        Node y = x.Left;
        x.Left = y.Right;

        if (y.Right != _nullNode)
        {
            y.Right.Parent = x;
        }

        y.Parent = x.Parent;

        if (x.Parent == null)
        {
            _root = y;
        }
        else if (x == x.Parent.Right)
        {
            x.Parent.Right = y;
        }
        else
        {
            x.Parent.Left = y;
        }

        y.Right = x;
        x.Parent = y;
    }

    private void Transplant(Node u, Node v)
    {
        if (u.Parent == null)
        {
            _root = v;
        }
        else if (u == u.Parent.Left)
        {
            u.Parent.Left = v;
        }
        else
        {
            u.Parent.Right = v;
        }

        v.Parent = u.Parent;
    }

    private Node SearchHelper(Node node, int key)
    {
        if (node == _nullNode || key == node.Key)
        {
            return node;
        }

        if (key < node.Key)
        {
            return SearchHelper(node.Left, key);
        }

        return SearchHelper(node.Right, key);
    }

    private void InOrderTraversal(Node node, List<Node> nodes)
    {
        if (node != _nullNode)
        {
            InOrderTraversal(node.Left, nodes);
            nodes.Add(new Node(node.Key, node.Data));
            InOrderTraversal(node.Right, nodes);
        }
    }

    private Node Minimum(Node node)
    {
        while (node.Left != _nullNode)
        {
            node = node.Left;
        }

        return node;
    }
    
    private class Record
    {
        public int Key { get; set; }
        public string Data { get; set; }
    }
    
    public AdjacencyGraph<int, Edge<int>> ToAdjacencyGraph()
    {
        var graph = new AdjacencyGraph<int, Edge<int>>();

        if (_root != _nullNode)
        {
            TraverseAndAddEdges(_root, graph);
        }

        return graph;
    }

    private void TraverseAndAddEdges(Node node, AdjacencyGraph<int, Edge<int>> graph)
    {
        if (node == _nullNode)
            return;
        
        graph.AddVertex(node.Key);

        if (node.Left != _nullNode)
        {
            graph.AddVerticesAndEdge(new Edge<int>(node.Key, node.Left.Key));
            TraverseAndAddEdges(node.Left, graph);
        }

        if (node.Right != _nullNode)
        {
            graph.AddVerticesAndEdge(new Edge<int>(node.Key, node.Right.Key));
            TraverseAndAddEdges(node.Right, graph);
        }
    }
    
    public Dictionary<int, GraphvizColor> GetNodeColors()
    {
        var colors = new Dictionary<int, GraphvizColor>();
        CollectNodeColors(_root, colors);
        return colors;
    }

    private void CollectNodeColors(Node node, Dictionary<int, GraphvizColor> colors)
    {
        if (node == _nullNode)
            return;

        colors[node.Key] = node.Color == NodeColor.Red ? GraphvizColor.Red : GraphvizColor.Black;

        CollectNodeColors(node.Left, colors);
        CollectNodeColors(node.Right, colors);
    }
}
