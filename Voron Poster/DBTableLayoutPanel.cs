using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Voron_Poster
{
    public class DBTableLayoutPanel : TableLayoutPanel
    {
        public DBTableLayoutPanel()
        {
            this.DoubleBuffered = true;
        }

        bool LayoutSuspended = false;
        public void SuspendLayoutSafe(){
            if (!LayoutSuspended)
            {
                LayoutSuspended = true;
                base.SuspendLayout();
            }
        }

        public void ResumeLayoutSafe()
        {
            if (LayoutSuspended)
            {
                LayoutSuspended = false;
                base.ResumeLayout();
            }
        }
        
    }
}
