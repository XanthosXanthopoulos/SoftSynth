using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synth
{
    public class Algorithm
    {

    }

    public class Node
    {
        public Node parent;
        public HashSet<Node> childs;

        public bool isLeaf;
        public bool isRoot;
        public bool isFeedback;
        public int id;

        public Node()
        {
            childs = new HashSet<Node>();
        }
    }
}
