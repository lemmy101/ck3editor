# ICSharpCode.TextEditor
ICSharpCode.TextEditor for WinForms from SharpDevelop 3.2, extended by a couple of new options

* Auto-hides Scrollbars when they are not needed
* Highlighting drop-down selectable from Forms Editor
* Right-click cut/copy/paste menu
* Removed ugly dotted line
* SQL Highlighting

# Using with Visual Studio #
 - Start **Visual Studio** and create/open your Windows Forms project
 - Search for and install the NuGet package **ICSharpCode.TextEditor.Extended**
 - Open the main Form of your application and show the **Toolbox**
 - Right click the **Toolbox** and **Add Tab**, give it the name **ICSharpCode**
 - Right click inside the new tab and select **Choose Toolbox Items**
 - Use the **Browse** button and go the NuGet **package** directory of your solution, navigate to the *ICSharpCode.TextEditor.Extended.version\lib* directory and select the **ICSharpCode.TextEditor** assembly
 - Select **OK** and now you have the control in the **Toolbox**