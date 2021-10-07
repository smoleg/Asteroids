using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids
{
    class Asteroid : BaseObject
    {
        Random r = new Random();

        public Asteroid(Point pos, Point dir, Size size, Image img) : base(pos, dir, size, img)
        {
            this.pos = pos;
            this.dir = dir;
            this.size = size;
            this.img = img;
        }

        public override bool Collision(ICollision obj)
        {
            if (obj.Rect.IntersectsWith(Rect))
            {
                ResetPosition();
                return true;
            }
            return false;
        }

        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(img, pos.X, pos.Y, size.Width, size.Height);
        }

        public override void ResetPosition()
        {
            pos.X = Game.Width;
            pos.Y = r.Next(Game.Height);
        }

        public override void Update()
        {
            pos.X += dir.X;
            pos.Y += dir.Y;

            if (pos.X < 0) dir.X = -dir.X;
            if (pos.X > Game.Width) dir.X = -dir.X;

            if (pos.Y < 0) dir.Y = -dir.Y;
            if (pos.Y > Game.Width) dir.Y = -dir.Y;
        }
    }
}
