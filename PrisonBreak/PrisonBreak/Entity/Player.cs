using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PrisonBreak.Entity {
    public class Player : Entity, Character{
        public Player(Game game, Vector2 position, Vector2 blockSize) : base(game, position, blockSize) {
            isGuided = false;
            Direction = Direction.EAST;
        }

        public override void Initialize() {
            currentKeyboardState = Keyboard.GetState();
            previousKeyboardState = currentKeyboardState;

            base.Initialize();
        }

        public override void Update(GameTime gameTime) {
            updateHitbox();
            move();
            base.Update(gameTime);
        }

        private void updateHitbox() {
            HitBox = new Vector2[4];
            HitBox[0] = Position;
            HitBox[1] = new Vector2(Position.X + blockSize.X, Position.Y);
            HitBox[2] = new Vector2(Position.X, Position.Y + blockSize.Y);
            HitBox[3] = new Vector2(Position.X + blockSize.X, Position.Y + blockSize.Y);
        }

        private void move() {
            currentKeyboardState = Keyboard.GetState();

            if ((currentKeyboardState.IsKeyDown(Keys.A) && previousKeyboardState.IsKeyUp(Keys.A)) || (currentKeyboardState.IsKeyDown(Keys.Left) && previousKeyboardState.IsKeyUp(Keys.Left))) {
                Direction = Direction.WEST;
                if (Node.Edges.ContainsKey(Direction)) {
                    Node = Node.Edges[Direction];
                    this.Position = Node.Position;
                }
            }

            if ((currentKeyboardState.IsKeyDown(Keys.D) && previousKeyboardState.IsKeyUp(Keys.D)) || (currentKeyboardState.IsKeyDown(Keys.Right) && previousKeyboardState.IsKeyUp(Keys.Right))) {
                Direction = Direction.EAST;
                if (Node.Edges.ContainsKey(Direction)) {
                    Node = Node.Edges[Direction];
                    this.Position = Node.Position;
                }
            }

            if ((currentKeyboardState.IsKeyDown(Keys.W) && previousKeyboardState.IsKeyUp(Keys.W)) || (currentKeyboardState.IsKeyDown(Keys.Up) && previousKeyboardState.IsKeyUp(Keys.Up))) {
                Direction = Direction.NORTH;
                if (Node.Edges.ContainsKey(Direction)) {
                    Node = Node.Edges[Direction];
                    this.Position = Node.Position;
                }
            }

            if ((currentKeyboardState.IsKeyDown(Keys.S) && previousKeyboardState.IsKeyUp(Keys.S)) || (currentKeyboardState.IsKeyDown(Keys.Down) && previousKeyboardState.IsKeyUp(Keys.Down))) {
                Direction = Direction.SOUTH;
                if (Node.Edges.ContainsKey(Direction)) {
                    Node = Node.Edges[Direction];
                    this.Position = Node.Position;
                }
            }

            previousKeyboardState = currentKeyboardState;
        }

        public Vector2[] HitBox { get; set; }

        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;
    }
}