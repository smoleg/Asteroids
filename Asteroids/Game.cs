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
        static BaseObject[] _asteroids;
        static BaseObject[] _stars;
        static BaseObject pulsar;
        static BaseObject _bullet;
        static SoundPlayer player;
        static Ship _ship;
        static Random _random = new Random();
        static Timer _timer;


        public static int Width;
        public static int Height;
        private static BufferedGraphicsContext _context;
        public static BufferedGraphics Buffer;


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
            _timer = new Timer();
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

            
            _timer.Interval = 40;
            _timer.Start();
            _timer.Tick += Timer_Tick;


            form.KeyDown += Form_KeyDown;
        }

        private static void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey && _bullet == null)
                _bullet = _bullet = new Bullet(new Point(_ship.Rect.X + 55, _ship.Rect.Y), new Point(10, 0), new Size(30, 60), Resources.laser);
            if (e.KeyCode == Keys.Up)
                _ship.Up();
            else if (e.KeyCode == Keys.Down)
                _ship.Down();

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
                if (asteroid != null)
                    asteroid.Draw();

            // Корабль
            if (_ship != null)
            {
                _ship.Draw();
                Buffer.Graphics.DrawString($"Energy: {_ship.Energy}", SystemFonts.DefaultFont, Brushes.White, new Point(5, 5));
            }


            // Лазер
            if (_bullet != null)
                _bullet.Draw();

            Buffer.Render();
        }

        public static void Load()
        {
            player = new SoundPlayer(Resources.Explosion);
            _ship = new Ship(new Point(10, 300), new Point(10, 10), new Size(80, 60), Resources.spaceship);
            _ship.Death += OnDeath;

            Random random = new Random();

            _asteroids = new Asteroid[15];
            for (int i = 0; i < _asteroids.Length; i++)
            {
                Image[] img = { Resources.asteroid1, Resources.asteroid2, Resources.asteroid3 };
                var size = random.Next(20, 50);
                var dir = random.Next(4, 7);
                _asteroids[i] = new Asteroid(new Point(Width / 2 + (random.Next(Width) / 2), random.Next(Height)),
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

        private static void OnDeath(object sender, EventArgs e)
        {
            _timer.Stop();
            Buffer.Graphics.DrawString("Game Over!", new Font(FontFamily.GenericSansSerif, 60, FontStyle.Bold), Brushes.Red, new Point(Width / 3, Height / 2));
            Buffer.Render();
        }

        public static void Update()
        {
            pulsar.Update();

            for (int i = 0; i < _asteroids.Length; i++)
            {
                if (_asteroids[i] == null)
                    continue;

                _asteroids[i].Update();
                if (_bullet != null && _asteroids[i].Collision(_bullet))
                {
                    player.Play();
                    _bullet = null;
                    _asteroids[i] = null;
                    continue;
                }
                if (_ship != null && _asteroids[i].Collision(_ship))
                {
                    _ship.GetHit(_random.Next(1, 6) * 5);
                    SystemSounds.Beep.Play();
                    if (_ship.Energy <= 0)
                        _ship.Die();
                }
            }

            foreach (var star in _stars)
                star.Update();

            if (_bullet != null)
            {
                _bullet.Update();
                if (_bullet.Rect.X > Width)
                    _bullet = null;
            }
        }
    }
}
