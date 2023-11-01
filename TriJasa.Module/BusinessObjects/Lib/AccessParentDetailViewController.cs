using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nucleus.Module.BusinessObjects
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class AccessParentDetailViewController : ViewController
    {
        public AccessParentDetailViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            RegisterActions(components);
            TargetViewType = ViewType.ListView;
            TargetViewNesting = Nesting.Nested;
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            View.CurrentObjectChanged += new EventHandler(View_CurrentObjectChanged);
            UpdateDetailViewCaption();
        }
        void View_CurrentObjectChanged(object sender, EventArgs e)
        {
            UpdateDetailViewCaption();
        }
        private void UpdateDetailViewCaption()
        {
            try
            {
                if (Frame is NestedFrame)
                {
                    //if (CurrentObject != null)
                    //{
                    //((NestedFrame)Frame).ViewItem.View.Caption = CurrentObject.Name;
                    ((NestedFrame)Frame).ViewItem.View.Refresh();
                    //}
                }
            }
            catch ( Exception e)
            {

            }
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            View.CurrentObjectChanged -= new EventHandler(View_CurrentObjectChanged);
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
