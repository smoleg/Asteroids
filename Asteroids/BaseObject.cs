using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids
{
    abstract class BaseObject : ICollision
    {
        protected Point pos;
        protected Point dir;
        protected Size size;
        protected Image img;
        protected Point startposition;

        public BaseObject(Point pos, Point dir, Size size, Image img)
        {
            this.pos = pos;
            this.dir = dir;
            this.size = size;
            this.img = img;
        }

        public Rectangle Rect
        {
            get
            {
                return new Rectangle(pos, size);
            }            
        }

        public bool Collision(ICollision obj)
        {
            return obj.Rect.IntersectsWith(Rect);
        }

        public abstract void Draw();

        public virtual void Update()
        {
            pos.X += dir.X;
            pos.Y += dir.Y;

            if (pos.X < 0) dir.X = -dir.X;
            if (pos.X > Game.Width) dir.X = -dir.X;

            if (pos.Y < 0) dir.Y = -dir.Y;
            if (pos.Y > Game.Width) dir.Y = -dir.Y;
        }

        public virtual void ResetPosition() { }

    }
}
