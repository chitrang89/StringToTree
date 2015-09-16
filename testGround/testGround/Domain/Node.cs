using System.Collections.Generic;

namespace testGround.Domain
{
    public class Node
    {
        public string Name { get; set; }
        public string ParentName { get; set; }

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

    public class Noded
    {
        public string Name { get; set; }
        //public string ParentName { get; set; }

        public IList<Noded> Children { get; set; }

        public Noded(string name)
        {
            Name = name;
            //ParentName = parentName;
            Children = new List<Noded>();
        }

        public Noded()
            : this(string.Empty)
        {
        }
    }
}
