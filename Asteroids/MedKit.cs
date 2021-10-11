using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids
{
    class MedKit : BaseObject
    {
        Random r = new Random();

        public MedKit(Point pos, Point dir, Size size, Image img) : base(pos, dir, size, img) { }

        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(img, pos.X, pos.Y, size.Width, size.Height);
        }

        public override void Update()
        {
            pos.X = pos.X + dir.X;
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

        public override void ResetPosition()
        {
            pos.X = Game.Width;
            pos.Y = r.Next(50, Game.Height - 50);
            dir.X = 0;
        }

        public void StartMovement(int speed)
        {
            dir.X = -speed;
        }
    }
}
