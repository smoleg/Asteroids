using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Asteroids
{
    static class Program
    {

        [STAThread]
        static void Main()
        {
            Form form = new Form();
            form.MinimumSize = new System.Drawing.Size(800, 700);
            form.MaximumSize = new System.Drawing.Size(800, 700);
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Text = "Asteroids";

            Game.Init(form);          
            Application.Run(form);
        }
    }
}
