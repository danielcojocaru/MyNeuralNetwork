using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gui
{
    public partial class UiForm : Form
    {
        public UiWorker W;

        public UiForm()
        {
            InitializeComponent();
            InitializeWorker();
        }

        private void UiForm_Load(object sender, EventArgs e)
        {
        }

        private void InitializeWorker()
        {
            W = new UiWorker();
            W.Create(this);
            W.Start();
        }
    }
}
