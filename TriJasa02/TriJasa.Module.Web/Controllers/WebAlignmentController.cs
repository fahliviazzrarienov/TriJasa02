using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using TriJasa.Module.BusinessObjects;

namespace TriJasa.Module.Web.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class WebAlignmentController : ViewController
    {
        private void SetCenterAlignment(WebPropertyEditor propertyEditor)
        {
            //if (propertyEditor.ViewEditMode == ViewEditMode.Edit)
            //{
            //    ASPxSpinEdit spinEdit = propertyEditor.Editor as ASPxSpinEdit;
            //    if (spinEdit != null)
            //    {
            //        spinEdit.HorizontalAlign = HorizontalAlign.Right;
            //    }
            //}
            ASPxSpinEdit spinEdit = propertyEditor.Editor as ASPxSpinEdit;
            if (spinEdit != null)
            {
                spinEdit.HorizontalAlign = HorizontalAlign.Right;
            }
        }
        private void propertyEditor_ControlCreated(object sender, EventArgs e)
        {
            SetCenterAlignment((WebPropertyEditor)sender);
        }
        public WebAlignmentController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            //TargetViewType = ViewType.ListView;
            TargetViewType = ViewType.DetailView;
            TargetObjectType = typeof(DaftarMuatan);

        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            View.ControlsCreated += new EventHandler(View_ControlsCreated);
            WebPropertyEditor propertyEditor = ((DetailView)View).FindItem("JmlTagihan") as WebPropertyEditor;
            if (propertyEditor != null)
            {
                if (propertyEditor.Control != null)
                {
                    SetCenterAlignment(propertyEditor);
                }
                else
                {
                    propertyEditor.ControlCreated += new EventHandler<EventArgs>(propertyEditor_ControlCreated);
                }
            }
        }

        private void View_ControlsCreated(object sender, EventArgs e)
        {
            foreach (PropertyEditor editor in ((DetailView)View).GetItems<PropertyEditor>())
            {
                var control = editor.Control as WebPropertyEditor;
                if (control != null)
                {
                    SetCenterAlignment(control);
                    //control.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                }
            }
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            ViewItem propertyEditor = ((DetailView)View).FindItem("JmlTagihan");
            if (propertyEditor != null)
            {
                propertyEditor.ControlCreated -= new EventHandler<EventArgs>(propertyEditor_ControlCreated);
            }
            View.ControlsCreated -= new EventHandler(View_ControlsCreated);
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
