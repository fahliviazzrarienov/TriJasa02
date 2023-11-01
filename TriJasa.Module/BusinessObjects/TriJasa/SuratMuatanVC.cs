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
    public partial class SuratMuatanVC : ViewController
    {
        DialogController dlgController;
        public SuratMuatanVC()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            //this.TargetObjectType = typeof(SuratMuatan);
            TargetObjectType = typeof(SuratMuatan);
            TargetViewId = "SuratMuatan_DetailView_Entry";
            TargetViewType = ViewType.DetailView;

            SimpleAction SMVDEPrintAction = new SimpleAction(this, "SMVDEPrint", PredefinedCategory.Unspecified)
            {
                Caption = "Print",
                //ConfirmationMessage = "Are you sure you want to clear the Tasks list?",
                ImageName = "Action_Printing_Print"
            };
            SMVDEPrintAction.Category = "Cetho";
            SMVDEPrintAction.TargetViewType = ViewType.DetailView;
            SMVDEPrintAction.Execute += SMVDEPrintAction_Execute;

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
        private void SMVDEPrintAction_Execute(Object sender, SimpleActionExecuteEventArgs e)
        {
            //while (((SuratMuatan)View.CurrentObject).Tasks.Count > 0)
            //{
            //    ((Contact)View.CurrentObject).Tasks.Remove(((Contact)View.CurrentObject).Tasks[0]);
            //}
            // ObjectSpace.SetModified(View.CurrentObject);

            var controller = Frame.GetController<ModificationsController>();
            if (controller != null)
            {
                controller.SaveAction.DoExecute();
            }
            
            ObjectSpace.CommitChanges();

            SuratMuatan currentObject = View.CurrentObject as SuratMuatan;
            if (currentObject != null)
            {
                currentObject.Save();
                currentObject.Session.CommitTransaction();
            }
            currentObject.Save();
            currentObject.Session.CommitTransaction();
            string ReportName = "";
            iGenTriJasa oGen = new iGenTriJasa();
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(SuratMuatan));
            ArrayList aListDM = new ArrayList();
            CriteriaOperator oCriteria = null;
            //((SuratMuatan)View.CurrentObject).NomorDM.Save();
            SuratMuatan oSM = ((SuratMuatan)View.CurrentObject);
            oSM.UpdateJenisBarang(true);
            
            oSM.Save();
            oSM.Session.CommitTransaction();
            objectSpace.CommitChanges();
            View.ObjectSpace.CommitChanges();


            string sqlQuery = string.Format("SuratMuat.Oid = {0}  ", ((SuratMuatan)View.CurrentObject).Oid);
            CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);

            ////oCriteria = GroupOperator.And(oCriteria, new InOperator("NomorDM.Oid", aListDM));
            ReportName = oGen.ReportNameGet(objectSpace, "DMPPrintSM");
            PrintReports(ReportName, filterOperator);
            //this.ObjectSpace.Refresh();
        }
       void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            if (e.Object != null && View !=null)
            {

                try
                {

                    System.DateTime oDate = System.DateTime.Now; // ((fParkTrans)View.CurrentObject).TimeOut;
                                                                 //GetTotalToday(oSession, oDate);
                    /// tiket di ketemukan
                    // jam in ada data nya
                    //stKeluar.Text = System.DateTime.Now.ToString();
                    if (View.CurrentObject == e.Object)
                    {
                        //if (txtNomorSM == null || txtNomorSM.PropertyValue.ToString() == "")
                        if (txtNomorSM.PropertyValue.ToString() == "")
                            if (((SuratMuatan)View.CurrentObject).NomorSM == "")
                            {
                                DaftarMuatan DM = ((SuratMuatan)View.CurrentObject).NomorDM;
                                DM.Save();
                                DM.Session.CommitTransaction();
                                string NomorSM = "";
                                if (DM != null)
                                {
                                    DevExpress.Xpo.Session session = ((XPObjectSpace)ObjectSpace).Session;
                                    string sqlQuery = $" Oid =={DM.Oid} ";
                                    CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);
                                    //fParkClosed oParkClosed = session.FindObject<fParkClosed>(filterOperator);
                                    DaftarMuatan oDM = session.FindObject<DaftarMuatan>(filterOperator);
                                    string sMaxNo = "";
                                    try
                                    {
                                        if (oDM != null)
                                        {
                                            sMaxNo = oDM.SuratMuatan.Max(x => x.NomorSM);
                                        }

                                    }
                                    catch (Exception)
                                    {
                                        sMaxNo = "";
                                    }

                                    int i = 1;
                                    if (sMaxNo != null && sMaxNo != "")
                                    {
                                        i = int.Parse(sMaxNo) + 1;
                                        NomorSM = $"{i.ToString("000")}";
                                    }
                                    else
                                    {
                                        NomorSM = $"{i.ToString("000")}";
                                    }
                                }
                            ((SuratMuatan)View.CurrentObject).NomorSM = NomorSM;
                            }
                        if (e.PropertyName == "TotalJmlOrg")
                        {

                            if (((SuratMuatan)View.CurrentObject).TotalJmlOrg <= (((SuratMuatan)View.CurrentObject).TotalDP))
                            {
                                ((SuratMuatan)View.CurrentObject).Lunas = true;
                            }
                        }

                        if (this.ObjectSpace.IsModified)
                        {
                            PropertyEditor peTandaTerimaNo = ((DetailView)View).FindItem("TandaTerimaNo") as PropertyEditor;
                            PropertyEditor pePelanggan = ((DetailView)View).FindItem("Pelanggan") as PropertyEditor;
                            PropertyEditor peDikerimke = ((DetailView)View).FindItem("Dikerimke") as PropertyEditor;

                            if (peTandaTerimaNo.PropertyValue.ToString() != "" && pePelanggan.PropertyValue != null && peDikerimke.PropertyValue != null)
                            {

                                this.ObjectSpace.CommitChanges();
                            }
                        }
                    }
                }
                catch (Exception)
                { }

                //if (View.CurrentObject == e.Object &&
                //       e.PropertyName == "Department" &&
                //       ObjectSpace.IsModified &&
                //       e.OldValue != e.NewValue)
                //{
                //    Contact changedContact = (Contact)e.Object;
                //    if (changedContact.Department != null)
                //    {
                //        changedContact.Office = changedContact.Department.Office;
                //    }
                //}

            }
        }
  
        private FocusDefaultDetailViewItemController focusDefaultDetailViewItemController;
        private DevExpress.Xpo.Session oSession;
        private PropertyEditor txtNomorSM;
        private PropertyEditor DM;
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.

   

            txtNomorSM = ((DetailView)View).FindItem("NomorSM") as PropertyEditor;
            DM = ((DetailView)View).FindItem("NomorDM") as PropertyEditor;

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
            //throw new NotImplementedException();
        }

        private void AcceptAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

            
            this.ObjectSpace.CommitChanges();
          //  throw new NotImplementedException();
        }

        private void SaveAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Set Active ChangeSet for Auditing  
            if (View.CurrentObject is SuratMuatan)
            {

                //Parent parent = (Parent)View.CurrentObject;
                //parent.Children.Add(ObjectSpace.CreateObject<Child>());
                //((DetailView)View).FindItem("Children").Refresh();
                IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(SuratMuatan));
                
                SuratMuatan oSuratMuatan = (SuratMuatan)View.CurrentObject;
                oSuratMuatan.UpdateJenisBarang(true);
                oSuratMuatan.Save();

                //IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(SuratMuatan));
                //oSuratMuatan.ob
                oSuratMuatan.Session.CommitTransaction();
                string sqlQuery = string.Format($" Oid == '{oSuratMuatan.NomorDM.Oid}' ");
                CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);
                DaftarMuatan oDM = oSession.FindObject<DaftarMuatan>(filterOperator);
                if (oDM != null)
                {
                    try
                    {
                        if (((SuratMuatan)View.CurrentObject).Pelanggan != null
                       && (((SuratMuatan)View.CurrentObject).Dikerimke != null))
                        {
                            if (this.ObjectSpace.IsModified)
                            {
                                try
                                {
                                    oSuratMuatan.NomorDM = oDM;
                                    oSuratMuatan.UpdateJenisBarang(true);
                                    oSuratMuatan.Save();
                                    oDM.Save();
                                    oSuratMuatan.Session.CommitTransaction();
                                    oDM.Session.CommitTransaction();
                                    this.ObjectSpace.CommitChanges();
                                }
                                catch (Exception)
                                {

                                }
                            }
                        }

                        
                    }
                    catch ( Exception)
                    { }
                    View.Refresh();
                    //IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(Person)

                }
                //if (((SuratMuatan)View.CurrentObject).Pelanggan != null
                //    && (((SuratMuatan)View.CurrentObject).Dikerimke != null))
                //{
                //    if (this.ObjectSpace.IsModified)
                //    {
                //        try
                //        {
                //            this.ObjectSpace.CommitChanges();
                //        }
                //        catch ( Exception)
                //        {

                //        }
                //    }
                //}
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
        private void ViewController_CustomFocusDetailViewItem(object sender, CustomFocusDetailViewItemEventArgs e)
        {
            if (e.DetailView.Id == "SuratMuatan_DetailView_Entry")
            {
                e.DetailViewItemToFocus = e.DetailView.FindItem("TandaTerimaNo");

            }
        }
        void ObjectSpace_ModifiedChanged(object sender, EventArgs e)
        {


            UpdateActionState();
        }
        protected virtual void UpdateActionState()
        {
            // action1.Enabled["ObjectSpaceIsModified"] = !ObjectSpace.IsModified;
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
            if (dlgController != null)
            {
                dlgController.AcceptAction.Execute -= AcceptAction_Execute;
                
                dlgController.CancelAction.Execute -= CancelAction_Execute;
                dlgController = null;
            }
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
