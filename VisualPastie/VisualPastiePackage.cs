using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using System.Windows.Forms;
using EnvDTE;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using PastieAPI;
using PastieLangauge = PastieAPI.Language;

namespace Microsoft.VisualPastie
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(GuidList.guidVisualPastiePkgString)]
    public sealed class VisualPastiePackage : Package
    {
        protected override void Initialize()
        {
            base.Initialize();

            var mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;

            if (null != mcs)
            {
                var menuCommandID = new CommandID(GuidList.guidVisualPastieCmdSet, (int)PkgCmdIDList.visualPastieCommand);
                var menuItem = new MenuCommand(MenuItemCallback, menuCommandID);
                mcs.AddCommand(menuItem);
            }
        }

        private void MenuItemCallback(object sender, EventArgs e)
        {
            var view = GetActiveTextView();
            if (view == null || Dte.ActiveDocument == null)
            {
                return;
            }

            var content = GetTextForPastie(view);
            var url = Pastie.Paste(content, GetCurrentEditorLanguage());
            Clipboard.SetText(url);

            SetStatus("Pastie created successfully and url copied to clipboard. " + url);
        }

        private PastieLangauge GetCurrentEditorLanguage()
        {
            var activeFileName = this.Dte.ActiveWindow.Document.Name;
            var extension = new FileInfo(activeFileName).Extension;
            foreach (PastieLangauge value in Enum.GetValues(typeof(PastieLangauge)))
            {
                var extensions = value.GetAttributeValues<FileExtensionAttribute, string>(f => f.Extension);
                if (extensions.Contains(extension))
                {
                    return value;
                }
            }

            return PastieLangauge.PlainText;
        }
        
        private static string GetTextForPastie(ITextView view)
        {
            if (SelectionIsAvailable(view))
            {
                return GetSelectedText(view);
            }

            return view.TextSnapshot.GetText();
        }

        private static bool SelectionIsAvailable(ITextView view)
        {
            if (view == null)
            {
                throw new ArgumentNullException("view");
            }

            return !view.Selection.IsEmpty && view.Selection.SelectedSpans.Count > 0;
        }

        private static string GetSelectedText(ITextView view)
        {
            return view.Selection.SelectedSpans[0].GetText();
        }

        private IWpfTextView GetActiveTextView()
        {
            IWpfTextView view = null;
            IVsTextView vTextView;

            var txtMgr = (IVsTextManager)GetService(typeof(SVsTextManager));
            
            const int mustHaveFocus = 1;

            txtMgr.GetActiveView(mustHaveFocus, null, out vTextView);
           

            var userData = vTextView as IVsUserData;
            
            if (null != userData)
            {
                object holder;

                var guidViewHost = DefGuidList.guidIWpfTextViewHost;
                userData.GetData(ref guidViewHost, out holder);

                var viewHost = (IWpfTextViewHost)holder;
                view = viewHost.TextView;
            }
            
            return view;
        }

        private DTE Dte
        {
            get { return (DTE)GetService(typeof(DTE)); }
        }

        private void SetStatus(string format, params object[] args)
        {
            var uiManager = ((IOleComponentUIManager)GetService(typeof(SOleComponentUIManager)));

            if (uiManager == null)
            {
                return;
            }

            var message = string.Format(format, args);
            uiManager.SetStatus(message, UInt32.Parse("0"));
        }

    }
}
