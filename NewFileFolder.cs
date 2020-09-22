#region

using DarkUI.Forms;
using JominiParse;

#endregion

namespace CK3ScriptEditor
{
    public partial class NewFileFolder : DarkForm
    {
        public NewFileFolder()
        {
            InitializeComponent();
        }

        public RefFilename Dir { get; set; }
    }
}