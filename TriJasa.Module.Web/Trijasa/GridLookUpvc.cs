using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TriJasa.Module.BusinessObjects;

namespace TriJasa.Module.Web.Trijasa
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class GridLookUpvc : ViewController<DetailView>
    {
        public GridLookUpvc()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            foreach (ASPxGridLookupPropertyEditor propertyEditor in View.GetItems<ASPxGridLookupPropertyEditor>())
            {
                propertyEditor.ControlCreated += PropertyEditor_ControlCreated;
            }
            //foreach (ASPxDoublePropertyEditor propertyEditor in View.GetItems<ASPxDoublePropertyEditor>())
            //{
            //    propertyEditor.ControlCreated += PropertyEditor_ControlCreated;
            //    //propertyEditor.SetControlAlignment(System.Web.UI.WebControls.HorizontalAlign.Right);
            //    //  propertyEditor.CurrentObject.
            //    // propertyEditor.Editor.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
            //}

        }
        //private void PropertyEditor_ControlCreatedDBL(object sender, EventArgs e)
        //{
        //    ASPxGridLookupPropertyEditor propertyEditor = (ASPxGridLookupPropertyEditor)sender;
        //    propertyEditor.Editor.Load += (s, args) => {
        //        string oldScript = "s.GetGridView().SetWidth(s.GetWidth());";
        //        string newScript = "s.GetGridView().SetWidth(700);";
        //        propertyEditor.Editor.ClientSideEvents.Init = propertyEditor.Editor.ClientSideEvents.Init.Replace(oldScript, newScript);
        //        propertyEditor.Editor.ClientSideEvents.DropDown = propertyEditor.Editor.ClientSideEvents.DropDown.Replace(oldScript, newScript);
        //    };
        //}

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        private void PropertyEditor_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                ASPxGridLookupPropertyEditor propertyEditor = (ASPxGridLookupPropertyEditor)sender;

                if (propertyEditor.Editor != null)
                {
                    propertyEditor.Editor.Load += (s, args) =>
                    {
                        string oldScript = "s.GetGridView().SetWidth(s.GetWidth());";
                        string newScript = "s.GetGridView().SetWidth(700);";
                        propertyEditor.Editor.ClientSideEvents.Init = propertyEditor.Editor.ClientSideEvents.Init.Replace(oldScript, newScript);
                        propertyEditor.Editor.ClientSideEvents.DropDown = propertyEditor.Editor.ClientSideEvents.DropDown.Replace(oldScript, newScript);
                    };
                }

                //ASPxDoublePropertyEditor propertyEditordbl = (ASPxDoublePropertyEditor)sender;
                //if (propertyEditordbl.Editor != null)
                //{
                //    propertyEditordbl.Editor.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                //}

            }
            catch (Exception)
            {

            }
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            foreach (ASPxGridLookupPropertyEditor propertyEditor in View.GetItems<ASPxGridLookupPropertyEditor>())
            {
                propertyEditor.ControlCreated -= PropertyEditor_ControlCreated;
            }
            
            base.OnDeactivated();
        }
    }
}
