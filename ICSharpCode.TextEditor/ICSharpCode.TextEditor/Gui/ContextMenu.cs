using ICSharpCode.TextEditor.Actions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ICSharpCode.TextEditor {



    public partial class ContextMenu : ContextMenuStrip {

      
        TextAreaControl parent;
        List<ToolStripItem> def = new List<ToolStripItem>();
        public ContextMenu(TextAreaControl parent) {
            this.parent = parent;
            InitializeComponent();

            undo.Click += OnClickUndo;
            cut.Click += OnClickCut;
            copy.Click += OnClickCopy;
            paste.Click += OnClickPaste;
            selectAll.Click += OnSelectAll;
            foreach (ToolStripItem item in Items) 
                def.Add(item);
        }



        void OnClickCut(object sender, EventArgs e) {
            new Cut().Execute(parent.TextArea);
            parent.TextArea.Focus();
        }

        void OnClickUndo(object sender, EventArgs e) {
            parent.Undo();
            parent.TextArea.Focus();
        }

        void OnClickCopy(object sender, EventArgs e) {
            new Copy().Execute(parent.TextArea);
            parent.TextArea.Focus();
        }

        void OnClickPaste(object sender, EventArgs e) {
            new Paste().Execute(parent.TextArea);
            parent.TextArea.Focus();
        }

        void OnSelectAll(object sender, EventArgs e) {
            new SelectWholeDocument().Execute(parent.TextArea);
            parent.TextArea.Focus();
        }


        public struct ContextMenuCreateEventArgs
        {
            public ContextMenu contextMenu { get; set; }
        }
        public event ContextMenuShownEventHandler ContextMenuShown;

        public delegate void ContextMenuShownEventHandler(object sender, ContextMenuCreateEventArgs e);

        void OnOpening(object sender, CancelEventArgs e) {
            undo.Enabled = parent.Document.UndoStack.CanUndo;
            cut.Enabled = copy.Enabled = delete.Enabled = parent.SelectionManager.HasSomethingSelected;
            paste.Enabled =  parent.TextArea.ClipboardHandler.EnablePaste;
            selectAll.Enabled = !string.IsNullOrEmpty(parent.Document.TextContent);
     
            this.Items.Clear();
            foreach (var toolStripItem in def)
            {
                this.Items.Add(toolStripItem);
            }
            if (ContextMenuShown != null)
            {
                ContextMenuShown.Invoke(this, new ContextMenuCreateEventArgs() { contextMenu = this });
            }

        
        }

    }
}
