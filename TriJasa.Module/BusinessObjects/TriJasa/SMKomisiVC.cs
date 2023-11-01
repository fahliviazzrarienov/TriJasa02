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
    public partial class SMKomisiVC : ViewController
    {
        public SMKomisiVC()
        {

            InitializeComponent();
            TargetObjectType = typeof(SMKomisi);
            SingleChoiceAction SMKPrintAction = new SingleChoiceAction(this, "SMKPrint", PredefinedCategory.Unspecified);
            SMKPrintAction.Caption = "Print";
            SMKPrintAction.ImageName = "Action_Printing_Print";
            SMKPrintAction.ItemType = SingleChoiceActionItemType.ItemIsOperation;
            ChoiceActionItem SMKPPrint01 = new ChoiceActionItem("SMKPPrint01", "Pengajuan Komisi", null);
            ChoiceActionItem SMKPPrint02 = new ChoiceActionItem("SMKPPrint02", "Komisi Status", null);
            ChoiceActionItem SMKPPrint03 = new ChoiceActionItem("SMKPPrint03", "Pembayaran Komisi", null);
            SMKPrintAction.TargetViewType = ViewType.ListView;
            SMKPrintAction.TargetObjectType = typeof(SMKomisi);
            SMKPrintAction.Items.Add(SMKPPrint01);
            SMKPrintAction.Items.Add(SMKPPrint02);
            SMKPrintAction.Items.Add(SMKPPrint03);
            SMKPrintAction.Execute += SMKPrintAction_Execute;
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }

        private void SMKPrintAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            /// throw new NotImplementedException();
            /// 
            //if (View.CurrentObject != null)
            //{
            string ReportName = "";
            iGenTriJasa oGen = new iGenTriJasa();
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(SMKomisi));
            ArrayList aListDM = new ArrayList();
            CriteriaOperator oCriteria = null;
            if ((e.SelectedObjects.Count > 0))
            {
                foreach (SMKomisi selectedObject in e.SelectedObjects)
                {
                    aListDM.Add(selectedObject.Oid);
                   // selectedObject.Printed();
                    selectedObject.Save();
                    selectedObject.Session.CommitTransaction();
                }
                if (e.SelectedChoiceActionItem.Id == "SMKPPrint01")
                {

                    oCriteria = GroupOperator.And(oCriteria, new InOperator("DaftarTagihan.Oid", aListDM));
                    ReportName = oGen.ReportNameGet(objectSpace, "SMKPPrint01");
                }
                else if (e.SelectedChoiceActionItem.Id == "SMKPPrint02")
                {
                    //SuratMuatan o;
                    //o.NotaSuratMuat
                    oCriteria = GroupOperator.And(oCriteria, new InOperator("DaftarTagihan.Oid", aListDM));
                    ReportName = oGen.ReportNameGet(objectSpace, "SMKPPrint02");
                }
                else if (e.SelectedChoiceActionItem.Id == "SMKPPrint03")
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
                //if (((SMKomisi)View.CurrentObject).ListSMKomisiItem.Count>0)
                //    {
                //    if (this.ObjectSpace.IsModified)
                //    {
                //        ((SMKomisi)View.CurrentObject).AddNota();

                //        this.ObjectSpace.CommitChanges();
                //    }
                //}
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
