using System;
using System.Collections.Generic;
using System.Linq;

namespace AStarWith8StonesPuzzle
{
    public class AStarSolver
    {
        public List<Node> Solve(int[,] startState, int[,] goalState)
        {
            var openList = new List<Node>();
            var closedList = new List<Node>();

            var startNode = new Node(startState, 0, CalculateH(startState, goalState), null);
            openList.Add(startNode);

            while (openList.Any())
            {
                var currentNode = openList.OrderBy(n => n.F).First();

                if (currentNode.IsEqual(new Node(goalState, 0, 0, null)))
                {
                    return ReconstructPath(currentNode);
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                foreach (var neighbor in GetNeighbors(currentNode, goalState))
                {
                    if (closedList.Any(n => n.IsEqual(neighbor)))
                        continue;

                    var existingNode = openList.FirstOrDefault(n => n.IsEqual(neighbor));

                    if (existingNode == null || neighbor.G < existingNode.G)
                    {
                        if (existingNode != null)
                            openList.Remove(existingNode);

                        openList.Add(neighbor);
                    }
                }
            }

            return null;
        }

        private int CalculateH(int[,] state, int[,] goalState)
        {
            int h = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (state[i, j] != goalState[i, j])
                        h++;
                }
            }
            return h;
        }

        private List<Node> GetNeighbors(Node node, int[,] goalState)
        {
            var neighbors = new List<Node>();
            var (x, y) = FindEmptyTile(node.State);

            var moves = new List<(int dx, int dy)> { (0, -1), (0, 1), (-1, 0), (1, 0) };

            foreach (var (dx, dy) in moves)
            {
                if (x + dx >= 0 && x + dx < 3 && y + dy >= 0 && y + dy < 3)
                {
                    var newState = (int[,])node.State.Clone();
                    newState[x, y] = newState[x + dx, y + dy];
                    newState[x + dx, y + dy] = 0;

                    var neighbor = new Node(newState, node.G + 1, CalculateH(newState, goalState), node);
                    neighbors.Add(neighbor);
                }
            }

            return neighbors;
        }

        private (int x, int y) FindEmptyTile(int[,] state)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (state[i, j] == 0)
                        return (i, j);
                }
            }
            return (-1, -1);
        }

        private List<Node> ReconstructPath(Node node)
        {
            var path = new List<Node>();
            while (node != null)
            {
                path.Add(node);
                node = node.Parent;
            }
            path.Reverse();
            return path;
        }
    }
}