

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO.Compression;

namespace PrisonBreak {
    public class LineOfSight {
        public LineOfSight() {
            bresenhams = new Bresenhams();
        }

        public List<List<Vector2>> calculateLineOfSight(int x0, int y0, Direction direction, int rayAmount) {
            List<List<Vector2>> rays = new List<List<Vector2>>();
            int x1;
            int y1;
            int distance = 0;

            switch (direction) {
                case Direction.NORTH:
                    for (int i = 0; i < rayAmount; i++) {
                        x1 = x0 - 150;
                        y1 = y0 - 250;
                        rays.Add(bresenhams.plotLine(x0, y0, x1 + distance, y1));
                        distance += 35;
                    }
                    break;
                case Direction.SOUTH:
                   
                    break;
                case Direction.WEST:
                   
                case Direction.EAST:
                   
                    break;
            }

            return rays;
        }

        private Bresenhams bresenhams;
    }
}