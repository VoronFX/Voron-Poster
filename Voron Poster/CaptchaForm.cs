using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Voron_Poster
{
    public partial class CaptchaForm : Form
    {
        public delegate Task<bool> GetNewCapthca(CaptchaForm CapthaForm);
        public GetNewCapthca func;

        public CaptchaForm()
        {
            InitializeComponent();
        }

        public void Resize(){           
            ClientSize = ClientSize - pictureBox1.Size + pictureBox1.Image.Size;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            func(this);
        }


    }
}
