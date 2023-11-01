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
using DevExpress.ExpressApp.Web.Controls;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace TriJasa.Module.Web.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppWindowControllertopic.aspx.
    public partial class CustomizePopupSizeController : WindowController
    {
        public CustomizePopupSizeController()
        {
            InitializeComponent();
            // Target required Windows (via the TargetXXX properties) and create their Actions.
            this.TargetWindowType = WindowType.Main;
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target Window.
            ((WebApplication)Application).PopupWindowManager.PopupShowing +=
PopupWindowManager_PopupShowing;
        }

        private void PopupWindowManager_PopupShowing(object sender, PopupShowingEventArgs e)
        {
            e.PopupControl.CustomizePopupWindowSize += XafPopupWindowControl_CustomizePopupWindowSize;
        }

        private void XafPopupWindowControl_CustomizePopupWindowSize(object sender,
            CustomizePopupWindowSizeEventArgs e)
        {

            e.Width =  Unit.Percentage(95); //new per Unit(600);
            e.Height = Unit.Percentage(95);
            e.Handled = true;
        }

        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
