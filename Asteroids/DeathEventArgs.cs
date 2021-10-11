using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids
{
    class DeathEventArgs : EventArgs
    {
        public int LastDamage { get; set; }

        public DeathEventArgs (int lastDamage)
        {
            LastDamage = lastDamage;
        }
    }
}
