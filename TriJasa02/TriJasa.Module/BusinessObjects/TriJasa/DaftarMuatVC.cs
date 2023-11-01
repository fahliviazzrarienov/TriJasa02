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
using System.Linq;
using System.Text;

namespace TriJasa.Module.BusinessObjects
{ 
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class DaftarMuatVC : ViewController
    {
        private ChoiceActionItem setPriorityItem;
        private ChoiceActionItem setStatusItem;
        DialogController dlgController;
        public DaftarMuatVC()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            //TargetViewType = ViewType.DetailView;
//            TargetViewType = ViewType.ListView;
            //TargetViewId = "BankTran_ListView_Pelunasan";

            TargetObjectType = typeof(DaftarMuatan);
            SingleChoiceAction DMPrintAction = new SingleChoiceAction(this, "DMPrint", PredefinedCategory.Unspecified);
            DMPrintAction.Caption = "Print";
            DMPrintAction.ImageName = "Action_Printing_Print";
            DMPrintAction.ItemType = SingleChoiceActionItemType.ItemIsOperation;
            ChoiceActionItem DMPPrintDM = new ChoiceActionItem("DMPPrintDM", "Print DM", null);
            ChoiceActionItem DMPPrintSM = new ChoiceActionItem("DMPPrintSM", "Print SM", null);
            ChoiceActionItem DMPPrintDMRekap = new ChoiceActionItem("DMPPrintDMRekap", "Print Rekap DM", null);
            ChoiceActionItem DMPPrintSelisih = new ChoiceActionItem("DMPPrintSelisih", "Print Selisih Ongkos", null);


            DMPrintAction.TargetViewType = ViewType.ListView;
            DMPrintAction.TargetObjectType = typeof(DaftarMuatan);
            DMPrintAction.Items.Add(DMPPrintDM);
            DMPrintAction.Items.Add(DMPPrintSM);
            DMPrintAction.Items.Add(DMPPrintDMRekap);
            DMPrintAction.Items.Add(DMPPrintSelisih);

            DMPrintAction.Execute += DMPrintAction_Execute;
            //this.components = new System.ComponentModel.Container();
            //DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem1 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            //DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem2 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            //this.scAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            //// 
            //// scAction
            //// 
            //this.scAction.Caption = "Import";
            //this.scAction.ConfirmationMessage = null;
            //this.scAction.DefaultItemMode = DevExpress.ExpressApp.Actions.DefaultItemMode.LastExecutedItem;
            //this.scAction.Id = "scImport";
            //this.scAction.ImageName = "Import";
            //choiceActionItem1.Caption = "Import";
            //choiceActionItem1.Id = "Import";
            //choiceActionItem1.ImageName = "Import";
            //choiceActionItem1.Shortcut = null;
            //choiceActionItem1.ToolTip = null;
            //choiceActionItem2.Caption = "Template for Import";
            //choiceActionItem2.Id = "Export";
            //choiceActionItem2.ImageName = "Export";
            //choiceActionItem2.Shortcut = null;
            //choiceActionItem2.ToolTip = null;
            //this.scAction.Items.Add(choiceActionItem1);
            //this.scAction.Items.Add(choiceActionItem2);
            //this.scAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            //this.scAction.QuickAccess = true;
            //this.scAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            //this.scAction.ToolTip = null;
            //this.scAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            //this.scAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.scAction_Execute);
            //// 
            //// VCImportData
            //// 
            //this.Actions.Add(this.scAction);
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
                //CriteriaOperator oCriteria = null;
                ////oCriteria = GroupOperator.And(oCriteria, new BinaryOperator("TransKas", (fTransKas)this));
                //oCriteria = GroupOperator.And(oCriteria, new InOperator("NomorDM.Oid", aListDM));
                controller.ShowPreview(handle, oCriteria);
            }

        }
        private void DMPrintAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            //if (View.CurrentObject != null)
            //{
            string ReportName = "";
            iGenTriJasa oGen = new iGenTriJasa();
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(DaftarMuatan));
            ArrayList aListDM = new ArrayList();
            CriteriaOperator oCriteria = null;
            if ((e.SelectedObjects.Count > 0))
            {
                foreach (DaftarMuatan selectedObject in e.SelectedObjects)
                {
                    aListDM.Add(selectedObject.Oid);
                    foreach (SuratMuatan oSM in selectedObject.SuratMuatan)
                    {
                        oSM.UpdateJenisBarang(true);
                        oSM.Save();
                        oSM.Session.CommitTransaction();

                    }
                    //objectSpace.CommitChanges();
                    //objectSpace.Refresh();

                }
                if (e.SelectedChoiceActionItem.Id == "DMPPrintDM")
                {
                  
                    oCriteria = GroupOperator.And(oCriteria, new InOperator("NomorDM.Oid", aListDM));
                    ReportName = oGen.ReportNameGet(objectSpace, "DMPPrintDM");
                }
                else if (e.SelectedChoiceActionItem.Id == "DMPPrintSM")
                {
                    //SuratMuatDtl o;
                    //o.SuratMuat.NomorDM
                    oCriteria = GroupOperator.And(oCriteria, new InOperator("SuratMuat.NomorDM.Oid", aListDM));
                    ReportName = oGen.ReportNameGet(objectSpace, "DMPPrintSM");
                }
                if (ReportName != "")
                {
                    PrintReports(ReportName, oCriteria);
                }
            }
           if (e.SelectedChoiceActionItem.Id == "DMPPrintDMRekap")
            {
                ReportName = oGen.ReportNameGet(objectSpace, "DMPPrintDMRekap");
                PrintReports(ReportName, oCriteria);
            }
            else if (e.SelectedChoiceActionItem.Id == "DMPPrintSelisih")
            {
                ReportName = oGen.ReportNameGet(objectSpace, "DMPPrintSelisih");
                PrintReports(ReportName, oCriteria);
            }
            //    if (e.SelectedChoiceActionItem.Id == "DMPPrintDM")
            //    {



            //        if ((e.SelectedObjects.Count > 0))
            //        {
            //            foreach (DaftarMuatan selectedObject in e.SelectedObjects)
            //            {
            //                    aListDM.Add(selectedObject.Oid);
            //            }

            //        CriteriaOperator oCriteria = null;
            //        //oCriteria = GroupOperator.And(oCriteria, new BinaryOperator("TransKas", (fTransKas)this));
            //        oCriteria = GroupOperator.And(oCriteria, new InOperator("NomorDM.Oid", aListDM));

            //        ReportName = oGen.ReportNameGet(objectSpace, "DMPPrintDM");

            //        PrintReports(ReportName, oCriteria);

            //        }
            //}
            //    else if (e.SelectedChoiceActionItem.Id == "DMPPrintSM")
            //    {
            //        if ((e.SelectedObjects.Count > 0))
            //        {
            //            foreach (DaftarMuatan selectedObject in e.SelectedObjects)
            //            {
            //                //BankTranOid = selectedObject.Oid;
            //                //Application.ShowViewStrategy.ShowMessage($"tidak ada data {selectedObject.Reference}");
            //                //break;
            //            }


            //            //Keterangan = $"Penerimaan Kasir tgl {Tanggal}, sebanyak {Total} TTB, Rp. {TotalUang.ToString("N0")} ";
            //        }
            //        //View.ObjectSpace.Delete(View.CurrentObject);
            //        //View.ObjectSpace.CommitChanges();
            //        //View.Refresh(true);
            //    }
            //}
        }
        //protected virtual void New(SingleChoiceActionExecuteEventArgs args)
        //{
        //    Type newObjectType;
        //    if (args.SelectedChoiceActionItem == null)
        //    {
        //        newObjectType = ((ObjectView)View).ObjectTypeInfo.Type;
        //    }
        //    else
        //    {
        //        newObjectType = (Type)args.SelectedChoiceActionItem.Data;
        //    }
        //    IObjectSpace objectSpaceForNewObject = Application.CreateObjectSpace(newObjectType);
        //    CreateNewObject(newObjectType, objectSpaceForNewObject, args);
        //}
        void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            if (View.CurrentObject == e.Object)
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(DaftarMuatan));
                if (((DaftarMuatan)View.CurrentObject).NoTruck != null
                    && (((DaftarMuatan)View.CurrentObject).NamaSpr != null))
                {
                    if (this.ObjectSpace.IsModified)
                    {
                        try
                        {
                            DaftarMuatan oSuratMuatan = (DaftarMuatan)View.CurrentObject;
                            oSuratMuatan.Save();
                            oSuratMuatan.Session.CommitTransaction();
                            this.ObjectSpace.CommitChanges();
                            View.Refresh();
                            
                            //this.ObjectSpace.Refresh();
                        }
                        catch(Exception)
                        { }
                    }
                   
                }

                
            }
        }
        private FocusDefaultDetailViewItemController focusDefaultDetailViewItemController;
        private DevExpress.Xpo.Session oSession;
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            oSession = ((XPObjectSpace)ObjectSpace).Session;

            ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;

            focusDefaultDetailViewItemController = Frame.GetController<FocusDefaultDetailViewItemController>();
            if (focusDefaultDetailViewItemController != null)
                focusDefaultDetailViewItemController.CustomFocusDetailViewItem += ViewController_CustomFocusDetailViewItem;

            ModificationsController controller = Frame.GetController<ModificationsController>();
            if (controller != null)
            {

                //controller.SaveAction.Execute += SaveAction_Execute;  
                controller.SaveAction.Executing += SaveAction_Executing;
                controller.SaveAction.Executed += SaveAction_Executed;

                controller.SaveAndCloseAction.Executing += SaveAction_Executing;
                controller.SaveAndCloseAction.Executed += SaveAction_Executed;

                controller.SaveAndNewAction.Executing += SaveAction_Executing;
                controller.SaveAndNewAction.Executed += SaveAction_Executed;

            }

            dlgController = Frame.GetController<DialogController>();
            if (dlgController != null)
            {
                dlgController.AcceptAction.Execute += AcceptAction_Execute;
                dlgController.CancelAction.Execute += CancelAction_Execute;
                dlgController.AcceptAction.ActionMeaning = ActionMeaning.Unknown;

            }
        }

        private void CancelAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
           // throw new NotImplementedException();
        }

        private void AcceptAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void ViewController_CustomFocusDetailViewItem(object sender, CustomFocusDetailViewItemEventArgs e)
        {
            if (e.DetailView.Id == "DaftarMuatan_DetailView_Entry")
            {
               // e.DetailViewItemToFocus = e.DetailView.FindItem("VechType");

            }
        }
        void ObjectSpace_ModifiedChanged(object sender, EventArgs e)
        {
            UpdateActionState();
        }
        protected virtual void UpdateActionState()
        {
            // action1.Enabled["ObjectSpaceIsModified"] = !ObjectSpace.IsModified;

            //if (this.ObjectSpace.IsModified)
            //{
            //    PropertyEditor peNomorDM = ((DetailView)View).FindItem("NomorDM") as PropertyEditor;
            //    PropertyEditor peNoTruck = ((DetailView)View).FindItem("NoTruck") as PropertyEditor;
            //    PropertyEditor peNamaSpr = ((DetailView)View).FindItem("NamaSpr") as PropertyEditor;
                



            //    if (peNomorDM.PropertyValue.ToString() != "" && peNoTruck.PropertyValue != null && peNamaSpr.PropertyValue != null)
            //    {

            //        this.ObjectSpace.CommitChanges();
            //    }
            //}

        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            ModificationsController controller = Frame.GetController<ModificationsController>();
            if (controller != null)
            {
                controller.SaveAction.Executing -= SaveAction_Executing;
                controller.SaveAction.Executed -= SaveAction_Executed;

                controller.SaveAndCloseAction.Executing -= SaveAction_Executing;
                controller.SaveAndCloseAction.Executed -= SaveAction_Executed;
                controller.SaveAndNewAction.Executing -= SaveAction_Executing;
                controller.SaveAndNewAction.Executed -= SaveAction_Executed;

            }
            dlgController = Frame.GetController<DialogController>();
            if (dlgController != null)
            {
                dlgController.AcceptAction.Execute -= AcceptAction_Execute;
                dlgController.CancelAction.Execute -= CancelAction_Execute;
                dlgController = null;

            }
            ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
        private void SaveAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Set Active ChangeSet for Auditing  
            if (View.CurrentObject is DaftarMuatan)
            {
                //SuratMuatan oSuratMuatan = (SuratMuatan)View.CurrentObject;
                //oSuratMuatan.NomorDM.Save();
                //oSuratMuatan.NomorDM.Session.CommitTransaction();

                //if (billableObj?.ChangeSet != null)
                //{
                //    if (AuditDataItemPersistentExt.ActiveChangeSet == null)
                //    {
                //        AuditDataItemPersistentExt.ActiveChangeSet = ValueManager.GetValueManager<ChangeSet>("Active");
                //    }
                //    AuditDataItemPersistentExt.ActiveChangeSet.Value = billableObj.ChangeSet;
                //}
            }
        }

        private void SaveAction_Executed(object sender, DevExpress.ExpressApp.Actions.ActionBaseEventArgs e)
        {
            // Clear Active ChangeSet for Auditing  
            //if (AuditDataItemPersistentExt.ActiveChangeSet != null)
            //{
            //    AuditDataItemPersistentExt.ActiveChangeSet.Value = null;
            //}
        }
    }

    public enum Priority
    {
        [ImageName("State_Priority_Low")]
        Low = 0,
        [ImageName("State_Priority_Normal")]
        Normal = 1,
        [ImageName("State_Priority_High")]
        High = 2
    }
}
