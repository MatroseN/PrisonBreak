using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PrisonBreak.Graphing;
using System.Collections.Generic;
using System.Linq;

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

        // Only for the enemies for now since it has some issues if you would run it for the player
        public void guidedMovement(Graph graph, Dictionary<Node, Node> allPaths, Node playerNode) {
            if (!once) {
                maxVelocity = 250;
                acceleration = 10.0f;
                Velocity = new Vector2(0, 0);
                Acceleration = new Vector2(0, 0);
                nextNode = Node;
                Position = nextNode.Position;
                once = true;
            }

            if (Node == nextNode) {
                if (Direction != PrevDirection) {
                    Acceleration = new Vector2(0, 0);
                    Velocity = new Vector2(0, 0);
                }
                nextNode = allPaths[Node];

                Direction = Node.Edges.FirstOrDefault(x => x.Value == nextNode).Key;
            }

            if (Direction != PrevDirection) {
                Velocity = new Vector2(0, 0);
                Acceleration = new Vector2(0, 0);
            }

            if (Node.ID != playerNode.ID) {
                switch (Direction) {
                    case Direction.NORTH:
                        PrevDirection = Direction;
                        if (Position.Y > nextNode.Position.Y) {
                            Acceleration = new Vector2(Acceleration.X, Acceleration.Y - acceleration);
                        } else {
                            Node = nextNode;
                            Position = Node.Position;
                        }
                        break;
                    case Direction.SOUTH:
                        PrevDirection = Direction;
                        if (Position.Y < nextNode.Position.Y) {
                            Acceleration = new Vector2(Acceleration.X, Acceleration.Y + acceleration);
                        } else {
                            Node = nextNode;
                            Position = Node.Position;
                        }
                        break;
                    case Direction.WEST:
                        PrevDirection = Direction;
                        if (Position.X > nextNode.Position.X) {
                            Acceleration = new Vector2(Acceleration.X - acceleration, Acceleration.Y);
                        } else {
                            Node = nextNode;
                            Position = Node.Position;
                        }
                        break;
                    case Direction.EAST:
                        PrevDirection = Direction;
                        if (Position.X < nextNode.Position.X) {
                            Acceleration = new Vector2(Acceleration.X + acceleration, Acceleration.Y);
                        } else {
                            Node = nextNode;
                            Position = Node.Position;
                        }
                        break;
                }
            } else {
                playerCaught = true;
                Position = Node.Position;
            }
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
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }
        public Direction Direction { get; set; }
        public Texture2D TextureMap { get; set; }
        public Node Node { get; set; }
        public Delay Delay { get; set; }
        public bool isGuided { get; set; }
        public Direction PrevDirection { get; set; }

        protected Vector2 blockSize;

        protected bool isTick;
        protected float maxVelocity;
        protected float maxAcceleration;

        private float acceleration = 2.0f;
        private bool once = false;
        private Node nextNode;
        protected bool playerCaught;
    }
}