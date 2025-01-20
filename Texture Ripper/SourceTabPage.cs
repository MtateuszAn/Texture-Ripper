using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Texture_Ripper
{
    public class SourceTabPage : TabPage
    {
        public PictureBox pictureBox { get; private set; }
        public Image image { get; private set; }
        public List<Selection> selections { get; private set; }

        public float scaleFactor = 1;

        public SourceTabPage(string title, Image image)
        {
            selections = new List<Selection>();
            this.Text = title;
            this.image = image;
            this.pictureBox = new PictureBox
            {
                Image = image,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Width = image.Width,
                Height = image.Height
            };

            this.Controls.Add(pictureBox);
        }

    }
}
