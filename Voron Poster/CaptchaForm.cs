using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Voron_Poster
{
    public partial class CaptchaForm : Form
    {
        public Func<Task<Bitmap>> RefreshFunction;
        public Action CancelFunction;
        private bool Resize = true;
        public AutoResetEvent IsFree;

        public CaptchaForm()
        {
            InitializeComponent();
            IsFree = new AutoResetEvent(true);
        }

        private async void buttonRefresh_Click(object sender, EventArgs e)
        {
            buttonRefresh.Enabled = false;
            try
            {
             Picture.Image = await RefreshFunction();
             if (Resize)
             ClientSize = ClientSize - Picture.Size + Picture.Image.Size;
             Resize = false;
            }
            catch (Exception Error)
            {
                if (Error is OperationCanceledException) {
                    this.Close();
                } else 
                MessageBox.Show(Error.Message, "Ошибка");
            }
            buttonRefresh.Enabled = true;
        }

        private void CaptchaForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            IsFree.Set();
            Picture.Image = null;
            Resize = true;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            CancelFunction();
        }

        private void CaptchaForm_Shown(object sender, EventArgs e)
        {
            buttonRefresh_Click(sender, e);
        }


    }
}
