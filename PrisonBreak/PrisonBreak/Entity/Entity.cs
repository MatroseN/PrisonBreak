using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PrisonBreak.Graphing;

namespace PrisonBreak.Entity {
    public abstract class Entity : GameComponent{
        public Entity(Game game, Vector2 position, Vector2 blockSize) : base(game) {
            Position = position;
            this.blockSize = blockSize; 
            isTick = false;
            calculatePosition();
        }

        public void setNode(Graph graph) {
            Node = graph.Adjecent[Position];
        }

        protected void tick(GameTime gameTime) {
            isTick = false;
            Delay.Wait(gameTime, () => {
                isTick = true;
            });
        }

        protected void updateVelocity(GameTime gameTime) {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float negativeMaxVelocity = -maxVelocity;
            if (Velocity.X >= maxVelocity) {
                Velocity = new Vector2(maxVelocity, Velocity.Y);
            }

            if (Velocity.X <= negativeMaxVelocity) {
                Velocity = new Vector2(negativeMaxVelocity, Velocity.Y);
            }

            if (Velocity.Y >= maxVelocity) {
                Velocity = new Vector2(Velocity.X, maxVelocity);
            }

            if (Velocity.Y <= negativeMaxVelocity) {
                Velocity = new Vector2(Velocity.X, negativeMaxVelocity);
            }

            Position += Velocity * delta;
        }

        protected void accelerate(GameTime gameTime) {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Velocity += Acceleration * delta;
        }

        private void calculatePosition() {
            Position = new Vector2(Position.X * blockSize.X, Position.Y * blockSize.Y);
        }

        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; protected set; }
        public Vector2 Acceleration { get; protected set; }
        public Direction Direction { get; set; }
        public Texture2D TextureMap { get; set; }
        public Node Node { get; set; }
        public Delay Delay { get; set; }
        protected Vector2 blockSize;

        protected bool isTick;
        protected float maxVelocity;
        protected float maxAcceleration;
    }
}