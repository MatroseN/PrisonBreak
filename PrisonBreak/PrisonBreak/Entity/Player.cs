using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PrisonBreak.Entity {
    public class Player : Entity, Character{
        public Player(Game game, Vector2 position) : base(game, position) {

        }

        public override void Initialize() {
            currentKeyboardState = Keyboard.GetState();
            previousKeyboardState = currentKeyboardState;

            base.Initialize();
        }

        public override void Update(GameTime gameTime) {
            move();

            base.Update(gameTime);
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

        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;
    }
}