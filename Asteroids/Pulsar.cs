using Asteroids.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids
{
    class Pulsar : BaseObject
    {
        public Pulsar(Point pos, Point dir, Size size, Image img) : base(pos, dir, size, img) { }

        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(img, pos.X, pos.Y, size.Width, size.Height); 
        }

        public override void Update()
        {
            if (size.Width > 200) 
            {
                dir.X = -dir.X;
                dir.Y = -dir.Y;
            }
            else if (size.Width < 100)
            {
                dir.X = -dir.X;
                dir.Y = -dir.Y;
            }   
            size.Width += dir.X;
            size.Height += dir.Y;
            pos.X = pos.X - dir.X / 2;
            pos.Y = pos.Y - dir.Y / 2;
        }
    }
}
