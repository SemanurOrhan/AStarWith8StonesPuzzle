using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStarWith8StonesPuzzle
{
    public class Node
    {
        public int[,] State { get; set; }
        public int G { get; set; }
        public int H { get; set; }
        public int F => G + H;
        public Node Parent { get; set; }

        public Node(int[,] state, int g, int h, Node parent)
        {
            State = state;
            G = g;
            H = h;
            Parent = parent;
        }
        public bool IsEqual(Node other)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (State[i, j] != other.State[i, j])
                        return false;
                }
            }
            return true;
        }
    }
}
