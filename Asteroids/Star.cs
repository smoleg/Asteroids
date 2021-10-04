using Asteroids.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids
{
    class Star : BaseObject
    {
        public Star (Point pos, Point dir, Size size, Image img) : base (pos, dir, size, img) { }

        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(img, pos.X, pos.Y, size.Width, size.Height);
        }
    }
}
