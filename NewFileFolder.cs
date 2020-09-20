using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DarkUI.Forms;

namespace CK3ScriptEditor
{
    public partial class NewFileFolder : DarkForm
    {
        public NewFileFolder()
        {
            InitializeComponent();
        }

        public string Dir { get; set; }
    }
}
