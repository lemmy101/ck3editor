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
using DarkUI.Forms;
using JominiParse;

namespace CK3ScriptEditor
{

 

    public partial class LoadingDialog : DarkForm, IProgressFeedback
    {
        public static LoadingDialog Instance;

        private string lockVar = "";
        private string progressTextToSet;
        private int minimum = 1;
        private int maximum = 100;
        private int accumulatedProgressSinceLastUpdate;
        private int progressLastProgress;

        private Thread thread = null;

        public LoadingDialog()
        {
            Instance = this;

            InitializeComponent();
        }

        public void Init(Control parent, ThreadStart start)
        {
            thread = new Thread(start);
            thread.Start();
            ShowDialog(parent);
        }

        public void Init(Control parent, ParameterizedThreadStart start, object param)
        {
            thread = new Thread(start);
            thread.Start(param);
            ShowDialog(parent);
        }

        public void StartNewJob(string text, int maximum)
        {
            lock (lockVar)
            {
                progressTextToSet = text;
                this.maximum = maximum;
            }
        }

        public void AddProgress(int done)
        {
            lock (lockVar)
            {
                accumulatedProgressSinceLastUpdate += done;
            }
        }

        ~LoadingDialog()
        {
            Instance = null;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lock (lockVar)
            {
                if (accumulatedProgressSinceLastUpdate > 0)
                {
                    for(int x=0;x<accumulatedProgressSinceLastUpdate;x++)
                        progressBar.PerformStep();

                    accumulatedProgressSinceLastUpdate = 0;
                }

                if (progressTextToSet != null)
                {
                    progressBar.Maximum = maximum;
                    progressBar.Value = 0;
                    progressBar.PerformStep();
                    progressText.Text = progressTextToSet;
                    progressTextToSet = null;
                }
            }

            if (!thread.IsAlive)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
