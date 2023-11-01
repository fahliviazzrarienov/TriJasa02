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

        public partial class TestController : ObjectViewController
        {
        public TestController()
        {
            InitializeComponent();
            TargetObjectType = typeof(fTransKasDtl);
            TargetViewType = ViewType.Any;
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }

        protected override void OnActivated()
            {
                base.OnActivated();
                View.ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
            }
            private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
            {
                if (e.PropertyName != "TransCode") return;
                ASPxGridLookupPropertyEditor lookup = View.FindItem("fTransCode") as ASPxGridLookupPropertyEditor;
                if (lookup != null)
                {
                    lookup.RefreshDataSource();
                }
            }
            protected override void OnDeactivated()
            {
                View.ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
                base.OnDeactivated();
            }
        }
    
}
