using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

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


            base.Update(gameTime);
        }

        private void move() {
            currentKeyboardState = Keyboard.GetState();


        }

        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;
    }
}