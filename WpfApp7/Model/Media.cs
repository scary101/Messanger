using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp7.Model
{
    internal class Media
    {
        private SoundPlayer player;

        public Media()
        {
            string exepath = AppDomain.CurrentDomain.BaseDirectory;
            string path = Directory.GetParent(exepath)?.Parent?.Parent?.Parent?.FullName + "\\Voice\\prikol.wav";
            this.player = new SoundPlayer(path);
        }

        public void Play()
        {
            player.Play();
        }

        public void Stop()
        {
            player.Stop();
        }
    }
}
