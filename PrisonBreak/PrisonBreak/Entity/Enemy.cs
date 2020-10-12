using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace PrisonBreak.Entity {
    public class Enemy : Entity, Character {
        public Enemy(Game game, Vector2 position, Vector2 blockSize) : base(game, position, blockSize) {
            // TODO: REmove when mapGeneration is popped in
            playerCaught = false;
            isGuided = false;
            maxVelocity = 100.0f;
            maxAcceleration = 50.0f;
            Direction = Direction.EAST;
            Acceleration = new Vector2(0, 0);
            Velocity = new Vector2(0, 0);
            Rays = new List<List<Vector2>>();
            lineOfSight = new LineOfSight();
        }

        public override void Update(GameTime gameTime) {
            if (!isGuided) {
                calculateVision();
            }
            if (!playerCaught) {
                accelerate(gameTime);
                updateVelocity(gameTime);
            }

            if (!isGuided) {
                move();
            }

            base.Update(gameTime);
        }

        public bool checkIfPlayerInVision(Vector2[] playerHitbox) {
            foreach (List<Vector2> ray in Rays) {
                foreach (Vector2 point in ray) {
                    foreach (Vector2 corner in playerHitbox) {
                        if (point == corner) {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void move() {
            PrevDirection = Direction;
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
                    if (Node.Edges.ContainsKey(Direction.WEST)) {
                        if (Position.X > Node.Edges[Direction.WEST].Position.X) {
                            Acceleration = new Vector2(Acceleration.X - acceleration, Acceleration.Y);
                        } else {
                            Node = Node.Edges[Direction.WEST];
                        }
                    } else {
                        turn();
                    }
                    break;
                case Direction.EAST:
                    if (Node.Edges.ContainsKey(Direction.EAST)) {
                        if (Position.X < Node.Edges[Direction.EAST].Position.X) {
                            Acceleration = new Vector2(Acceleration.X + acceleration, Acceleration.Y);
                        } else {
                            Node = Node.Edges[Direction.EAST];
                        }
                    } else {
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

        private void calculateVision() {
            calculateLineOfSight();
            setVertexPositions();
            createTriangles();
        }
        

        private void calculateLineOfSight() {
            Rays = lineOfSight.calculateLineOfSight((int)Position.X, (int)Position.Y, Direction, 10, blockSize);
        }

        private void setVertexPositions() {
            Triangles = new List<VertexPositionColor[]>();
            VertexPositions = new List<List<Vector2>>();
            foreach (List<Vector2> ray in Rays) {
                Vector2 anchor = ray[0];
                Vector2 final = ray[ray.Count - 1];
                List<Vector2> points = new List<Vector2>();
                points.Add(anchor);
                points.Add(final);

                VertexPositions.Add(points);
            }
        }

        private void createTriangles() {
            for (int i = VertexPositions.Count - 1; i > 0; i--) {

                VertexPositionColor[] vertecies = new VertexPositionColor[3];
                vertecies[0].Color = new Color(50, 50, 0, 0);
                vertecies[1].Color = new Color(50, 50, 0, 0);
                vertecies[2].Color = new Color(50, 50, 0, 0);

                // Current
                vertecies[0].Position = new Vector3(VertexPositions[i][0].X, VertexPositions[i][0].Y, 0);
                vertecies[1].Position = new Vector3(VertexPositions[i][1].X, VertexPositions[i][1].Y, 0);

                // Next
                vertecies[2].Position = new Vector3(VertexPositions[i - 1][1].X, VertexPositions[i - 1][1].Y, 0);

                Triangles.Add(vertecies);
            }
        }

        public List<List<Vector2>> Rays { get; set; }
        public List<VertexPositionColor[]> Triangles { get; set; }
        private List<List<Vector2>> VertexPositions { get; set; }

        private const float acceleration = 2.0f;
        private LineOfSight lineOfSight;
    }
}
