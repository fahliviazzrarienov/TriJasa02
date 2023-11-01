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
// new files
namespace TriJasa.Module.BusinessObjects
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class vcScreenFile : ViewController
    {
        public vcScreenFile()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetViewType = ViewType.DashboardView;
            TargetObjectType = typeof(ImportFiles);
            SimpleAction ImportDataAction = new SimpleAction(this, "ImportDatasAction", PredefinedCategory.View)
            {
                Caption = "Import Datas",
                ConfirmationMessage = "Are you sure you want to Import Datas?",
                ImageName = "Action_Clear"
            };
            ImportDataAction.Execute += ImportDataAction_Execute;
        }
        private void ImportDataAction_Execute(Object sender, SimpleActionExecuteEventArgs e)
        {
            //while (((Contact)View.CurrentObject).Tasks.Count > 0)
            //{
            //    ((Contact)View.CurrentObject).Tasks.Remove(((Contact)View.CurrentObject).Tasks[0]);
            //}
            ObjectSpace.SetModified(View.CurrentObject);
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
