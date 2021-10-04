using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids
{
    class Bullet : BaseObject
    {
        public Bullet(Point pos, Point dir, Size size, Image img) : base(pos, dir, size, img) 
        {
            startposition.X = pos.X;
            startposition.Y = pos.Y;
        }

        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(img, pos.X, pos.Y, size.Width, size.Height);
        }

        public override void Update()
        {
            pos.X = pos.X + dir.X;
        }

        public override void ResetPosition()
        {
            pos.X = startposition.X;
            pos.Y = startposition.Y;
        }
    }
}
