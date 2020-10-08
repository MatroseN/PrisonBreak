using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace PrisonBreak.Graphing {
    public class Node {
        public Node(Vector2 pos) {
            Position = pos;
            Edges = new Dictionary<Direction, Node>();
        }
        public Vector2 Position { get; set; }
        public Dictionary<Direction, Node> Edges { get; set; }
    }
}