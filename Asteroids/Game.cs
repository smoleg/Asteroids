using Asteroids.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Asteroids
{
    static class Game
    {
        private static BufferedGraphicsContext _context;
        public static BufferedGraphics Buffer;
        static BaseObject[] _asteroids;
        static BaseObject[] _stars;
        static BaseObject pulsar;
        static BaseObject _bullet;
        static SoundPlayer player;
        public static int Width; 
        public static int Height;

        public static int width
        {
            get
            {
                return Width;
            }
            set
            {
                if (value > 1000 || value < 0) throw new ArgumentOutOfRangeException();
                Width = value;
            }
        }
        public static int height
        {
            get
            {
                return Height;
            }
            set
            {
                if (value > 1000 || value < 0) throw new ArgumentOutOfRangeException();
                Height = value;
            }
        }

        public static void Init(Form form)
        {
            _context = BufferedGraphicsManager.Current;
            Graphics g = form.CreateGraphics();

            try
            {
                width = form.ClientSize.Width;
                height = form.ClientSize.Height;
            }
            catch (ArgumentOutOfRangeException)
            {
                Width = 1000;
                Height = 1000;
            }


            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));

            Load();

            Timer timer = new Timer();
            timer.Interval = 40;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private static void Timer_Tick(object sender, EventArgs e)
        {
            Update();
            Draw();
        }

        public static void Draw()
        {
            // Фон
            Buffer.Graphics.DrawImage(Resources.background, new Rectangle(0, 0, Width, Height));


            // Звезды
            foreach (var star in _stars)
            {
                star.Draw();
            }

            // Пульсар
            pulsar.Draw();

            // Планета
            Buffer.Graphics.DrawImage(Resources.planet, new Rectangle(100, 100, 180, 180));

            // Астероиды
            foreach (var asteroid in _asteroids)
            {
                asteroid.Draw();
            }

            // Лазер
            _bullet.Draw();

            Buffer.Render();
        }

        public static void Load()
        {
            player = new SoundPlayer(Resources.Explosion);
            _bullet = new Bullet(new Point(0, 200), new Point(10, 0), new Size(30, 60), Resources.laser);

            Random random = new Random();

            _asteroids = new Asteroid[15];
            for (int i = 0; i < _asteroids.Length; i++)
            {
                Image[] img = { Resources.asteroid1, Resources.asteroid2, Resources.asteroid3 };
                var size = random.Next(20, 50);
                var dir = random.Next(4, 7);
                _asteroids[i] = new Asteroid(new Point(random.Next(Height), random.Next(Width)),
                    new Point(-1 * (random.Next(2) + 1) * dir, -1 * (random.Next(2) + 1) * dir),
                    new Size(size, size),
                    img[random.Next(img.Length)]);
            }

            _stars = new Star[20];
            for (int i = 0; i < _stars.Length; i++)
            {
                Image[] img = { Resources.star1, Resources.star2, Resources.star3 };
                var size = random.Next(5, 21);
                var dir = random.Next(3);
                _stars[i] = new Star(new Point(random.Next(Width), random.Next(Height)),
                    new Point(-1 * (random.Next(2) + 1) * dir, -1 * (random.Next(2) + 1) * dir),
                    new Size(size, size),
                    img[random.Next(img.Length)]);
            }

            pulsar = new Pulsar(new Point(450, 300), new Point(3, 3), new Size(120, 120), Resources.Pulsar);

        }

        public static void Update()
        {
            foreach (var asteroid in _asteroids)
            {
                asteroid.Update();
                if (asteroid.Collision(_bullet))
                {
                    player.Play();
                    _bullet.ResetPosition();
                }
            }

            foreach (var star in _stars)
            {
                star.Update();
            }

            pulsar.Update();
            _bullet.Update();
        }
    }
}
