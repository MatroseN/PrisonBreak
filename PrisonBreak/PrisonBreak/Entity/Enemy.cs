using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace PrisonBreak.Entity {
    public class Enemy : Entity, Character {
        public Enemy(Game game, Vector2 position, Vector2 blockSize) : base(game, position, blockSize) {
            // TODO: REmove when mapGeneration is popped in
            maxVelocity = 100.0f;
            maxAcceleration = 50.0f;
            Direction = Direction.NORTH;
            Acceleration = new Vector2(0, 0);
            Velocity = new Vector2(0, 0);
        }

        public override void Update(GameTime gameTime) {
            accelerate(gameTime);
            updateVelocity(gameTime);
            move();

            base.Update(gameTime);
        }

        private void move() {
            switch (Direction) {
                case Direction.NORTH:
                    if (Node.Edges.ContainsKey(Direction.NORTH)) {
                        if (Position.Y > Node.Edges[Direction.NORTH].Position.Y) {
                            Acceleration = new Vector2(Acceleration.X, Acceleration.Y - acceleration);
                        } else{
                            Node = Node.Edges[Direction.NORTH];
                        }
                    } else {
                        turn();
                    }
                    break;
                case Direction.SOUTH:
                    if (Node.Edges.ContainsKey(Direction.SOUTH)) {
                        if (Position.Y < Node.Edges[Direction.SOUTH].Position.Y) {
                            Acceleration = new Vector2(Acceleration.X, Acceleration.Y + acceleration);
                        } else {
                            Node = Node.Edges[Direction.SOUTH];
                        }
                    } else {
                        turn();
                    }
                    break;
                case Direction.WEST:
                    if (Node.Edges.ContainsKey(Direction.WEST) && Position == Node.Position) {
                        Node = Node.Edges[Direction.WEST];
                        Acceleration = new Vector2(Acceleration.X - acceleration, Acceleration.Y);
                    } else if (Position == Node.Position && !Node.Edges.ContainsKey(Direction.NORTH)) {
                        turn();
                    }
                    break;
                case Direction.EAST:
                    if (Node.Edges.ContainsKey(Direction.EAST) && Position == Node.Position) {
                        Node = Node.Edges[Direction.EAST];
                        Acceleration = new Vector2(Acceleration.X + acceleration, Acceleration.Y);
                    } else if (Position == Node.Position && !Node.Edges.ContainsKey(Direction.NORTH)) {
                        turn();
                    }
                    break;
            }
        }

        private void turn() {
            Acceleration = new Vector2(0, 0);
            Velocity = new Vector2(0, 0);
            switch (Direction) {
                case Direction.NORTH:
                    Direction = Direction.SOUTH;
                    break;
                case Direction.SOUTH:
                    Direction = Direction.NORTH;
                    break;
                case Direction.WEST:
                    Direction = Direction.EAST;
                    break;
                case Direction.EAST:
                    Direction = Direction.WEST;
                    break;
            }
        }

        public Vector2[] Vision { get; set; }

        private const float acceleration = 2.0f;
    }
}
