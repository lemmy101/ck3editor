using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DarkUI.Controls;
using DarkUI.Forms;
using JominiParse;

namespace CK3ScriptEditor
{
    public partial class NewLocalizationFileObjectDialog : DarkForm
    {
        public NewLocalizationFileObjectDialog()
        {
            InitializeComponent();
        }

        internal String ChosenFilename;

        public void Init()
        {
            OK.Enabled = false;
         
            string dirFromContext = "localization/english/";

            DarkTreeNode root = new DarkTreeNode("localization");
      
            List<string> str = Core.Instance.ModCK3Library.GetDirectoryListFromLocalization();
            for (var index = 0; index < str.Count; index++)
            {
                var item = root;
                var file = str[index];

                file = file.Replace(dirFromContext, "");
                var parts = file.Split('/');
                
                foreach (var part in parts)
                {
                    DarkTreeNode next = new DarkTreeNode(part);
                    var f = item.Nodes.Where(a => a.Text == part);
                    if (f.Any())
                    {
                        next = f.First();
                    }
                    else
                    {
                        item.Nodes.Add(next);

                    }
                    item = next;
                    item.Expanded = true;
                    if(part.Contains(".yml"))
                        item.Tag = file;
                }
            }

            if(root.Nodes.Count > 0)
                existingFileView.Nodes.Add(root);
            
            root.Expanded = true;
        }

    
        public ScriptContext Context { get; set; }

        private void newFileRadio_CheckedChanged(object sender, EventArgs e)
        {
            cloneToExisting.Checked = !newFileRadio.Checked;
        }

        private void cloneToExisting_CheckedChanged(object sender, EventArgs e)
        {
            newFileRadio.Checked = !cloneToExisting.Checked;
        }

        private void filenameButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "yml files (*.yml)|*.yml";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.InitialDirectory = (Globals.CK3ModPath + Core.Instance.ModCK3Library.Name + "/" + "localization/english/");
            saveFileDialog1.InitialDirectory = saveFileDialog1.InitialDirectory.Replace("/", "\\");

            string dir = Globals.CK3ModPath + Core.Instance.ModCK3Library.Name + "/" + "localization/english/";

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            bool done = false;

            while (!done)
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (!saveFileDialog1.FileName.Replace("\\", "/").Contains(Globals.CK3ModPath + Core.Instance.ModCK3Library.Name + "/" + "localization/english/"))
                    {
                        MessageBox.Show("Error: File must be in " + Globals.CK3ModPath + Core.Instance.ModCK3Library.Name + "/" + "localization/english/" +
                                        " directory", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        done = true;
                        string f = saveFileDialog1.FileName.Replace("\\", "/");
                        string r = Globals.CK3ModPath + Core.Instance.ModCK3Library.Name + "/" +
                                   "localization/english" + "/";

                        chosenFilename.Text = saveFileDialog1.FileName = f.Replace(r, "");
                        newFileRadio.Checked = true;
                        ChosenFilename = chosenFilename.Text;
                        OK.Enabled = true;
                    }
                }
                else
                {
                    done = true;
                }
            }

            
        }

        private void OK_Click(object sender, EventArgs e)
        {
            // do the clone!!!


       
            
        }

        private void Cancel_Click(object sender, EventArgs e)
        {

        }

        private void existingFileView_SelectedNodesChanged(object sender, EventArgs e)
        {

            if (existingFileView.SelectedNodes.Count > 0 && existingFileView.SelectedNodes[0].Tag != null)
            {
                ChosenFilename = existingFileView.SelectedNodes[0].Tag.ToString();
                cloneToExisting.Checked = true;
                OK.Enabled = true;
            }
            
        }
    }
}
