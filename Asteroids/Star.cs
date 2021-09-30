using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids
{
    class Star : Asteroid
    {
        public Star (Point pos, Point dir, Size size) : base (pos, dir, size)
        {

        }

        public override void Draw()
        {
            Game.Buffer.Graphics.DrawLine(Pens.White, pos.X, pos.Y, pos.X + size.Width, pos.Y + size.Height);
            Game.Buffer.Graphics.DrawLine(Pens.White, pos.X + size.Width, pos.Y, pos.X, pos.Y + size.Height);
            //base.Draw();
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
