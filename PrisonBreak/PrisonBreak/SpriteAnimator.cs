using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PrisonBreak {
    public class SpriteAnimator {

        public Rectangle textureChooser(Texture2D textureMap, Direction direction, Vector2 textureSize) {
            Rectangle rect = new Rectangle(0, 0, (int)textureSize.X, (int)textureSize.Y);

            // chooses the correct position of the sprite map based on the direction and texture size
            switch (direction) {
                case Direction.NORTH:
                    rect = new Rectangle(0, 0, (int)textureSize.X, (int)textureSize.Y);
                    break;
                case Direction.SOUTH:
                    rect = new Rectangle((int)textureSize.X, 0, (int)textureSize.X, (int)textureSize.Y);
                    break;
                case Direction.EAST:
                    rect = new Rectangle((int)textureSize.X, (int)textureSize.Y, (int)textureSize.X, (int)textureSize.Y);
                    break;
            }
            return rect;
        }
    }
}