using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace PrisonBreak.Graphing {
    public class Node {
        public Node(Vector2 pos) {
            Position = pos;
            Neighbours = new Dictionary<Direction, Node>();
        }
        public Vector2 Position { get; set; }
        public Dictionary<Direction, Node> Neighbours { get; set; }
    }
}