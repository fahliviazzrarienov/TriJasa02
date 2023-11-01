using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TriJasa.Module.Web.Trijasa
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppWindowControllertopic.aspx.
    public partial class CustomizeCloseDialogController : WindowController
    {
        public CustomizeCloseDialogController()
        {
            InitializeComponent();
            // Target required Windows (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target Window.
            try
            {
                PopupWindow popupWindow = Window as PopupWindow;
                if (popupWindow != null)
                {
                    DialogController dialogController = popupWindow.GetController<DialogController>();
                    if (dialogController != null)
                    {
                        dialogController.Cancelling += previewReportDialogController_Cancelling;
                    }
                }
            }
            catch(Exception)
            {

            }
        }
        private void previewReportDialogController_Cancelling(object sender, EventArgs e)
        {
            if (Application.MainWindow.View != null && Application.MainWindow.View.Id == "DaftarMuatan_DetailView_Entry")
            {

                Window.Close(true);
            }
        }
        protected override void OnDeactivated()
        {
            try
            {

                PopupWindow popupWindow = Window as PopupWindow;
                if (popupWindow != null)
                {
                    DialogController dialogController = popupWindow.GetController<DialogController>();
                    if (dialogController != null)
                    {
                           dialogController.Cancelling -= previewReportDialogController_Cancelling;
                    }
                }
            }
            catch(Exception)
            { }
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
