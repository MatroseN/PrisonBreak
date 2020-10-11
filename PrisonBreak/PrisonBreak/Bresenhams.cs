using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace PrisonBreak {
    public class Bresenhams {
        public List<Vector2> plotLine(int x0, int y0, int x1, int y1) {
            List<Vector2> ray = new List<Vector2>();
            int dx = Math.Abs(x1 - x0);
            int sx = x0 < x1 ? 1 : -1;
            int dy = -Math.Abs(y1 - y0);
            int sy = y0 < y1 ? 1 : -1;
            int err = dx + dy, e2;

            int i = 0;
            for (; ;) {
                ray.Add(new Vector2(x0, y0));
                i++;

                if (x0 == x1 && y0 == y1) {
                    break;
                }

                e2 = 2 * err;

                if (e2 >= dy) {
                    err += dy; x0 += sx;
                }

                if (e2 <= dx) {
                    err += dx; y0 += sy;
                }
            }
            return ray;
        }

    }
}