using Microsoft.Xna.Framework;

namespace PrisonBreak.Entity {
    public class Enemy : Entity, Character {
        public Enemy(Game game, Vector2 position) : base(game, position) {

        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
        }

        private void move() {

        }
    }
}
