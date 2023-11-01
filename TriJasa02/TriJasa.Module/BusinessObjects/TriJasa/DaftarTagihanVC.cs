using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace TriJasa.Module.BusinessObjects.TriJasa
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class DaftarTagihanVC : ViewController
    {
        public DaftarTagihanVC()
        {

            InitializeComponent();
            TargetObjectType = typeof(DaftarTagihan);
            SingleChoiceAction DTPrintAction = new SingleChoiceAction(this, "DTPrint", PredefinedCategory.Unspecified);
            DTPrintAction.Caption = "Print";
            DTPrintAction.ImageName = "Action_Printing_Print";
            DTPrintAction.ItemType = SingleChoiceActionItemType.ItemIsOperation;
            ChoiceActionItem DTPPrint01 = new ChoiceActionItem("DTPPrint01", "Print Kolektor", null);
            ChoiceActionItem DTPPrint02 = new ChoiceActionItem("DTPPrint02", "Print Bali", null);
            ChoiceActionItem DTPPrint03 = new ChoiceActionItem("DTPPrint03", "Print Lombok", null);
            DTPrintAction.TargetViewType = ViewType.ListView;
            DTPrintAction.TargetObjectType = typeof(DaftarTagihan);
            DTPrintAction.Items.Add(DTPPrint01);
            DTPrintAction.Items.Add(DTPPrint02);
            DTPrintAction.Items.Add(DTPPrint03);
            DTPrintAction.Execute += DTPrintAction_Execute;
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }

        private void DTPrintAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            /// throw new NotImplementedException();
            /// 
            //if (View.CurrentObject != null)
            //{
            string ReportName = "";
            iGenTriJasa oGen = new iGenTriJasa();
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(DaftarTagihan));
            ArrayList aListDM = new ArrayList();
            CriteriaOperator oCriteria = null;
            if ((e.SelectedObjects.Count > 0))
            {
                foreach (DaftarTagihan selectedObject in e.SelectedObjects)
                {
                    aListDM.Add(selectedObject.Oid);
                   // selectedObject.Printed();
                    selectedObject.Save();
                    selectedObject.Session.CommitTransaction();
                }
                if (e.SelectedChoiceActionItem.Id == "DTPPrint01")
                {

                    oCriteria = GroupOperator.And(oCriteria, new InOperator("DaftarTagihan.Oid", aListDM));
                    ReportName = oGen.ReportNameGet(objectSpace, "DTPPrint01");
                }
                else if (e.SelectedChoiceActionItem.Id == "DTPPrint02")
                {
                    //SuratMuatan o;
                    //o.NotaSuratMuat
                    oCriteria = GroupOperator.And(oCriteria, new InOperator("DaftarTagihan.Oid", aListDM));
                    ReportName = oGen.ReportNameGet(objectSpace, "DTPPrint02");
                }
                else if (e.SelectedChoiceActionItem.Id == "DTPPrint03")
                {
                    //DaftarTagihanNota o;
                    //o.DaftarTagihan.Oid
                    oCriteria = GroupOperator.And(oCriteria, new InOperator("DaftarTagihan.Oid", aListDM));
                    ReportName = oGen.ReportNameGet(objectSpace, "DTPPrint03");
                }
                PrintReports(ReportName, oCriteria);
            }
            objectSpace.Refresh();

        }
        private void PrintReports(string ReportName, CriteriaOperator oCriteria)
        {
            IObjectSpace objectSpace =
                 ReportDataProvider.ReportObjectSpaceProvider.CreateObjectSpace(typeof(ReportDataV2));
            IReportDataV2 reportData =
                objectSpace.FindObject<ReportDataV2>(
                CriteriaOperator.Parse($"[DisplayName] = '{ReportName}'"));
            string handle = ReportDataProvider.ReportsStorage.GetReportContainerHandle(reportData);
            ReportServiceController controller = Frame.GetController<ReportServiceController>();

            if (controller != null)
            {

                controller.ShowPreview(handle, oCriteria);
            }

        }
        private DevExpress.Xpo.Session oSession;
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            oSession = ((XPObjectSpace)ObjectSpace).Session;

            ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
            ObjectSpace.ModifiedChanged += ObjectSpace_ModifiedChanged;
            ObjectSpace.ObjectSaving += ObjectSpace_ObjectSaving;

            Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executing += DeleteAction_Executing;
            ObjectSpace.CustomDeleteObjects += new EventHandler<CustomDeleteObjectsEventArgs>(ObjectSpace_CustomDeleteObjects);
            if (View is ListView && !View.IsRoot)
            {
                DeleteObjectsViewController deleteController = Frame.GetController<DeleteObjectsViewController>();
                if (deleteController != null)
                {
                    deleteController.Deleting += DeleteController_Deleting;
                }
            }

        }

        private void DeleteController_Deleting(object sender, DeletingEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void ObjectSpace_CustomDeleteObjects(object sender, CustomDeleteObjectsEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void DeleteAction_Executing(object sender, CancelEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void ObjectSpace_ObjectSaving(object sender, ObjectManipulatingEventArgs e)
        {
            ///throw new NotImplementedException();
            //if (View.CurrentObject == e.Object)
            //{
            //    if (this.ObjectSpace.IsModified)
            //    {
            //        this.ObjectSpace.CommitChanges();
            //    }
            //}
        }

        private void ObjectSpace_ModifiedChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            //if (View.CurrentObject == e.Object)
            //{
            //    if (this.ObjectSpace.IsModified)
            //    {
            //        this.ObjectSpace.CommitChanges();
            //    }
            //}
        }

        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            //throw new NotImplementedException();
            if (View.CurrentObject == e.Object)
            {
                if (((DaftarTagihan)View.CurrentObject).NamaColl!= null)
                    {
                    if (this.ObjectSpace.IsModified)
                    {
                        ((DaftarTagihan)View.CurrentObject).AddNota();

                        this.ObjectSpace.CommitChanges();
                    }
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

            ObjectSpace.CustomDeleteObjects -= new EventHandler<CustomDeleteObjectsEventArgs>(ObjectSpace_CustomDeleteObjects);
            Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executing -= DeleteAction_Executing;
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
