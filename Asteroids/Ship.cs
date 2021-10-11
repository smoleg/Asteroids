using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids
{
    class Ship : BaseObject
    {
        public event EventHandler<DeathEventArgs> Death;

        protected int _energy;
        int _lastDamage;

        public int Energy
        {
            get { return _energy; }
            set
            {
                if (_energy < 100 && value < 100 - _energy)
                    _energy = value;
                else _energy = 100;
            }
        }

        public Ship(Point pos, Point dir, Size size, Image img) : base(pos, dir, size, img)
        {
            _energy = 100;
            _lastDamage = 0;
        }

        public void GetHit(int damage)
        {
            _lastDamage = damage;
            _energy -= damage;
            //System.Console.WriteLine($"Получен урон: {damage}");
        }

        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(img, pos.X, pos.Y, size.Width, size.Height);
        }

        public void Up()
        {
            if (pos.Y > 0) pos.Y -= dir.Y;
        }

        public void Down()
        {
            if (pos.Y + size.Height < Game.Height) pos.Y += dir.Y;
        }

        public override void Update() { }

        public void Die()
        {
            if (Death != null)
            {
                Death.Invoke(this, new DeathEventArgs(_lastDamage));
            }
        }
    }
}
