using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Asteroids
{
    static class Game
    {
        private static BufferedGraphicsContext _context;
        public static BufferedGraphics Buffer;
        static Asteroid[] _asteroids;
        static Asteroid[] _stars;

        public static int Width { get; set; }
        public static int Height { get; set; }

        public static void Init(Form form)
        {
            _context = BufferedGraphicsManager.Current;
            Graphics g = form.CreateGraphics();

            Width = form.ClientSize.Width;
            Height = form.ClientSize.Height;

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
            Buffer.Graphics.Clear(Color.Black);

            foreach (var star in _stars)
            {
                star.Draw();
            }

            Buffer.Graphics.FillEllipse(Brushes.Red, new Rectangle(100, 100, 200, 200));

            foreach (var asteroid in _asteroids)
            {
                asteroid.Draw();
            }

            Buffer.Render();
        }

        public static void Load()
        {
            Random random = new Random();
            _asteroids = new Asteroid[15];

            for (int i = 0; i < _asteroids.Length; i++)
            {
                var size = random.Next(10, 40);
                _asteroids[i] = new Asteroid(new Point(600, i * 20 + 20), new Point(-i - 3, -i - 3), new Size(size, size));
            }

            _stars = new Asteroid[20];

            for (int i = 0; i < _stars.Length; i++)
            {
                _stars[i] = new Star(new Point(600, i * 10 + 7), new Point(i + 1, i + 1), new Size(4, 4));
            }
        }

        public static void Update()
        {
            foreach (var asteroid in _asteroids)
            {
                asteroid.Update();
            }

            foreach (var star in _stars)
            {
                star.Update();
            }
        }
    }
}
