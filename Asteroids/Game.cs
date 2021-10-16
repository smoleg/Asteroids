using Asteroids.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace Asteroids
{
    static class Game
    {
        static List<Asteroid> _asteroids;
        static BaseObject[] _stars;
        static BaseObject pulsar;
        static List<Bullet> _bullets;
        static SoundPlayer player;
        static Ship _ship;
        static MedKit _medkit;
        static int score = 0;
        static int asteroidsCount = 20;

        static Random _random = new Random();
        static Timer _timer;
        static Stopwatch _stopWatch;
        static TimeSpan ts;
        static StreamWriter streamWriter;


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
            _stopWatch = new Stopwatch();
            _timer = new Timer();
            _context = BufferedGraphicsManager.Current;
            Graphics g = form.CreateGraphics();
            //Task.Factory.StartNew(System.Console);

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
            _stopWatch.Start();

            form.KeyDown += OnFormKeyDown;
        }

        private static void OnFormKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
                _bullets.Add(new Bullet(new Point(_ship.Rect.X + 55, _ship.Rect.Y),
                             new Point(10, 0), new Size(30, 60), Resources.laser));
            if (e.KeyCode == Keys.Up)
                _ship.Up();
            else if (e.KeyCode == Keys.Down)
                _ship.Down();
        }

        private static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }

        public static void Draw()
        {
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
                asteroid.Draw();

            // Корабль
            if (_ship != null)
            {
                _ship.Draw();
                Buffer.Graphics.DrawString($"Energy: {_ship.Energy}", SystemFonts.DefaultFont, Brushes.White, new Point(5, 5));
            }

            Buffer.Graphics.DrawString($"Score: {score}", SystemFonts.DefaultFont, Brushes.White, new Point(5, 20));

            // Лазер
            foreach (var bullet in _bullets)
                bullet.Draw();


            // Аптечка
            _medkit.Draw();

            Buffer.Render();
        }

        public static void Load()
        {
            player = new SoundPlayer(Resources.Explosion);
            _ship = new Ship(new Point(10, Height / 2), new Point(10, 10), new Size(80, 60), Resources.spaceship);
            _ship.Death += OnDeath;
            _medkit = new MedKit(new Point(Width, 150), new Point(-10, 0), new Size(40, 40), Resources.medkit);
            _bullets = new List<Bullet>();

            LoadAsteroids(asteroidsCount);

            _stars = new Star[20];
            for (int i = 0; i < _stars.Length; i++)
            {
                Image[] img = { Resources.star1, Resources.star2, Resources.star3 };
                var size = _random.Next(5, 21);
                var dir = _random.Next(3);
                _stars[i] = new Star(new Point(_random.Next(Width), _random.Next(Height)),
                            new Point(-1 * (_random.Next(2) + 1) * dir, -1 * (_random.Next(2) + 1) * dir),
                            new Size(size, size),
                            img[_random.Next(img.Length)]);
            }

            pulsar = new Pulsar(new Point(450, 300), new Point(3, 3), new Size(120, 120), Resources.Pulsar);

        }

        // Формирование коллекции астероидов
        private static void LoadAsteroids(int count)
        {
            _asteroids = new List<Asteroid>();
            for (int i = 0; i < count; i++)
            {
                Image[] img = { Resources.asteroid1, Resources.asteroid2, Resources.asteroid3 };
                var size = _random.Next(20, 50);
                var dir = _random.Next(4, 7);
                _asteroids.Add(new Asteroid(new Point(Width / 2 + (_random.Next(Width) / 2), _random.Next(Height)),
                                new Point(-1 * (_random.Next(2) + 1) * dir, -1 * (_random.Next(2) + 1) * dir),
                                new Size(size, size),
                                img[_random.Next(img.Length)]));
                _asteroids[i].AstDestruction += OnAstDestruction;
            }
        }

        private static void OnAstDestruction(object sender, EventArgs e)
        {
            using (streamWriter = new StreamWriter("logs.txt", true))
            {
                streamWriter.WriteLine($"Уничтожен астероид {DateTime.Now}");
            }
        }

        private static void OnDeath(object sender, DeathEventArgs e)
        {
            _timer.Stop();
            Buffer.Graphics.DrawString($"Game Over!\nLast damage: {e.LastDamage}",
                                        new Font(FontFamily.GenericSansSerif, 30, FontStyle.Bold),
                                        Brushes.Red,
                                        new Point(Width / 4, Height / 2));
            Buffer.Render();
        }

        public static void Update()
        {
            ts = _stopWatch.Elapsed;
            pulsar.Update();
            if (_medkit.Collision(_ship))
            {
                _ship.Energy += 20;
                _medkit.ResetPosition();
            }

            _medkit.Update();
            if (_medkit.Rect.X < 0)
                _medkit.ResetPosition();

            if (ts.Seconds % 10 == 0)
                _medkit.StartMovement(10);

            foreach (var star in _stars)
                star.Update();

            foreach (var bullet in _bullets)
                bullet.Update();

            for (int i = _asteroids.Count - 1; i >= 0; i--)
            {
                _asteroids[i].Update();
                var asteroidDie = false;
                for (int j = _bullets.Count - 1; j >= 0; j--)
                {

                    // Столкновение пули с атероидом
                    if (_asteroids[i].Collision(_bullets[j]))
                    {
                        player.Play();
                        Buffer.Graphics.DrawImage(Resources.explosion1, _bullets[j].Rect.X, _bullets[j].Rect.Y, 100, 100);
                        _asteroids[i].Logging();
                        _asteroids.Remove(_asteroids[i]);
                        _bullets.Remove(_bullets[j]);
                        score += 100;
                        asteroidDie = true;
                        break;
                    }
                }

                // Столкновение корабля с астероидом
                if (!asteroidDie && _ship.Collision(_asteroids[i]))
                {
                    asteroidDie = false;
                    _asteroids.Remove(_asteroids[i]);
                    _ship.GetHit(_random.Next(1, 6) * 5);
                    SystemSounds.Beep.Play();
                    score -= 200;
                    if (_ship.Energy <= 0)
                    {
                        _ship.Die();
                    }
                }
            }

            if (_asteroids.Count == 0)
            {
                asteroidsCount += 1;
                LoadAsteroids(asteroidsCount);
            }
        }
    }
}


