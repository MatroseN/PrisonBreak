using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace PrisonBreak.Graphing {
    public class Graph {
        public Graph() {
            Adjecent = new Dictionary<Vector2, Node>();
        }

        // Generates a Graph from grid data
        public void gridToGraph(Vector2[] grid, Vector2 blockSize) {
            foreach (Vector2 pos in grid) {
                addNode(pos);
            }

            foreach (Node node in Adjecent.Values) {
                setEdges(node, blockSize);
            }
        }

        private void addNode(Vector2 pos) {
            Adjecent.Add(pos, new Node(pos));
        }

        // Sets the individual nodes edges
        private void setEdges(Node node, Vector2 blockSize) {
            // Add more positions to create more edges. Look at the Direction enum to see what directions you can use. For now it only uses 4 directions.
            Vector2 north = new Vector2(node.Position.X, node.Position.Y - blockSize.Y);
            Vector2 south = new Vector2(node.Position.X, node.Position.Y + blockSize.Y);
            Vector2  east = new Vector2(node.Position.X + blockSize.X, node.Position.Y);
            Vector2  west = new Vector2(node.Position.X - blockSize.X, node.Position.Y);

            // Each statement checks if there exists a node at that position and if it does it adds that node as an edge for the specific node
            if (Adjecent.ContainsKey(north)) 
                node.Edges.Add(Direction.NORTH, Adjecent[north]);

            if (Adjecent.ContainsKey(south)) 
                node.Edges.Add(Direction.SOUTH, Adjecent[south]);

            if (Adjecent.ContainsKey(east)) 
                node.Edges.Add(Direction.EAST, Adjecent[east]);

            if (Adjecent.ContainsKey(west)) 
                node.Edges.Add(Direction.WEST, Adjecent[west]);
        }

        public Dictionary<Vector2, Node> Adjecent { get; set; }
        public Vector2[] Grid { get; set; }
    }
}