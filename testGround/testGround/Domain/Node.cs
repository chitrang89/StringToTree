using System.Collections.Generic;

namespace testGround.Domain
{
    public class Node
    {
        public string Name { get; }
        public string ParentName { get; }

        public IList<Node> Children { get; set; }

        public Node(string name, string parentName)
        {
            Name = name;
            ParentName = parentName;
            Children = new List<Node>();
        }

        public Node() : this(string.Empty, string.Empty)
        {
        }        
    }
}
