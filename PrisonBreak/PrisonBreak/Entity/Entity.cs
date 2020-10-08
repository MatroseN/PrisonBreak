using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PrisonBreak.Graphing;

namespace PrisonBreak.Entity {
    public abstract class Entity : GameComponent{
        public Entity(Game game, Vector2 position) : base(game) {
            Position = position;
        }

        public void setNode(Graph graph) {
            Node = graph.Adjecent[Position];
        }

        public Vector2 Position { get; set; }
        public Direction Direction { get; set; }
        public Texture2D TextureMap { get; set; }
        public Node Node { get; set; }
    }
}