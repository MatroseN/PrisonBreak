using Microsoft.Xna.Framework;
using System;

namespace PrisonBreak {
    public class Delay {
        public Delay(double DelayTime) {
            this.DelayTime = DelayTime;
            this.Timer = 0.0;
        }

        public void Wait(GameTime gt, Action Action) {
            if (this.Timer <= gt.TotalGameTime.TotalMilliseconds) {
                Timer = gt.TotalGameTime.TotalMilliseconds + DelayTime;
                Action.Invoke();
            }
        }

        public double Timer { get; set; }
        public double DelayTime { get; set; }
    }
}