using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Notepad
{
    class Note
    {
        private string path;
        public string Path
        {
            get => path;
            set { path = value; }
        }

        private string noteName;
        public string NoteName 
        {
            get { return System.IO.Path.GetFileName(Path); }
            set { noteName = value; }
        }

        public Note(string noteName, string path)
        {
            this.noteName = noteName;
            this.path = path;
        }
    }
}
