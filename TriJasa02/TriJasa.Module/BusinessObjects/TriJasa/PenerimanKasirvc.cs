using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using System;
using System.Collections;
namespace TriJasa.Module.BusinessObjects
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class PenerimanKasirvc : ViewController
    {
        DialogController dlgController;
        public PenerimanKasirvc()
        {
            InitializeComponent();
            TargetViewType = ViewType.ListView;
            TargetViewId = "PenerimaanKasir_ListView;PenerimaanKasir_ListView_BelumDisetor;PenerimaanKasir_ListView_Disetor";
            //TargetObjectType = typeof(Note);
            // Target required Views (via the TargetXXX properties) and create their Actions.
            SimpleAction myAction = new SimpleAction(this, "Masukan Ke Kas", "Edit");
            myAction.SelectionDependencyType = SelectionDependencyType.RequireMultipleObjects;
            myAction.ImageName = "NewTask_16x16";
            myAction.TargetObjectType = typeof(PenerimaanKasir);
            myAction.Execute += myAction_Execute;
            Actions.Add(myAction);

            SingleChoiceAction PKLPrintAction = new SingleChoiceAction(this, "PKLPrint", PredefinedCategory.Unspecified);
            PKLPrintAction.Caption = "Print";
            PKLPrintAction.ImageName = "Action_Printing_Print";
            PKLPrintAction.ItemType = SingleChoiceActionItemType.ItemIsOperation;
            ChoiceActionItem PKVLPrint = new ChoiceActionItem("PKVLPrint", "Print Kas Kecil", null);
            ChoiceActionItem PKVLPrintRekap = new ChoiceActionItem("PKVLPrintRekap", "Print Rekap Penerimaan Kasir", null);



            PKLPrintAction.TargetViewType = ViewType.ListView;
            PKLPrintAction.TargetObjectType = typeof(PenerimaanKasir);
            PKLPrintAction.Items.Add(PKVLPrint);
            PKLPrintAction.Items.Add(PKVLPrintRekap);
            PKLPrintAction.Execute += PKLPrintAction_Execute;

            //SimpleAction PKVLPrintAction = new SimpleAction(this, "PKVLPrint", PredefinedCategory.Unspecified)
            //{
            //    Caption = "Print",
            //    //ConfirmationMessage = "Are you sure you want to clear the Tasks list?",
            //    ImageName = "Action_Printing_Print"
            //};
            ////PKVEPrintAction.Category = "CethoKasir";
            //PKVLPrintAction.Execute += PKVLPrintAction_Execute;

            //var popupAction = new PopupWindowShowAction(this, "ShowPopup", PredefinedCategory.View);
            //popupAction.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            //popupAction.CustomizePopupWindowParams += PopupAction_CustomizePopupWindowParams;

        }

        private void PKLPrintAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            // throw new NotImplementedException();
            string ReportName = "";
            iGenTriJasa oGen = new iGenTriJasa();
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(PenerimaanKasir));
            ArrayList aListDM = new ArrayList();
            CriteriaOperator oCriteria = null;
            //if ((e.SelectedObjects.Count > 0))
            //{
            //    foreach (PenerimaanKasir selectedObject in e.SelectedObjects)
            //    {
            //        aListDM.Add(selectedObject.Oid);
            //    }
            //    if (e.SelectedChoiceActionItem.Id == "DMPPrintDM")
            //    {

            //        oCriteria = GroupOperator.And(oCriteria, new InOperator("NomorDM.Oid", aListDM));
            //        ReportName = oGen.ReportNameGet(objectSpace, "DMPPrintDM");
            //    }
              
            //    if (ReportName != "")
            //    {
            //        PrintReports(ReportName, oCriteria);
            //    }
            //}
            if (e.SelectedChoiceActionItem.Id == "PKVLPrint")
            {
                ReportName = oGen.ReportNameGet(objectSpace, "PKVLPrint");
                PrintReports(ReportName, oCriteria);
            }
            else if (e.SelectedChoiceActionItem.Id == "PKVLPrintRekap")
            {
                ReportName = oGen.ReportNameGet(objectSpace, "PKVLPrintRekap");
                PrintReports(ReportName, oCriteria);
            }
        }

        private void PKVLPrintAction_Execute(Object sender, SimpleActionExecuteEventArgs e)
        {
     
            string ReportName = "";
            iGenTriJasa oGen = new iGenTriJasa();
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(PenerimaanKasir));
            ArrayList aListDM = new ArrayList();
            CriteriaOperator oCriteria = null;
;
            //PrintReports(ReportName, filterOperator);

            //if ((e.SelectedObjects.Count > 0))
            //{
            //    foreach (PenerimaanKasir selectedObject in e.SelectedObjects)
            //    {
            //        aListDM.Add(selectedObject.Oid);
            //    }

            //    oCriteria = GroupOperator.And(oCriteria, new InOperator("Oid", aListDM));
            oCriteria = null;
            ReportName = oGen.ReportNameGet(objectSpace, "PKV1Print");

                PrintReports(ReportName, oCriteria);
            //}

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

        private void PopupAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            //IObjectSpace objectSpace = e.Application.CreateObjectSpace(typeof(BankTran));
            //BankTran currentObject = objectSpace.GetObject(ViewCurrentObject);
            //DetailView detailView = e.Application.CreateDetailView(objectSpace, currentObject);
            //detailView.ViewEditMode = ViewEditMode.Edit;
            //e.View = detailView;
            //e.Maximized = true;
        }
        private BankTran obj;
        private ArrayList SelectedPenerimaanKasirs;
        private IObjectSpace objectSpaceInternal;
        private bool isSave = false;
        void myAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            //BankTran task = (BankTran)View.CurrentObject;
            isSave = false;
            double TotalUang = 0;
            string Keterangan="";
            string Tanggal = System.DateTime.Now.ToString("dd MMM yy");
            int Total=0;
            DateTime Tanggaltxn= System.DateTime.Now;
             SelectedPenerimaanKasirs = new ArrayList();
            //if ((e.SelectedObjects.Count > 0) && (e.SelectedObjects[0] is IObjectRecord))
            if ((e.SelectedObjects.Count > 0) )
            {
                foreach (PenerimaanKasir selectedObject in e.SelectedObjects)
                {
                    if (selectedObject.Disetor == false)
                    {
                        SelectedPenerimaanKasirs.Add((PenerimaanKasir)ObjectSpace.GetObject(selectedObject));
                        TotalUang += selectedObject.JumlahTerima;
                        Total++;
                        Tanggal = selectedObject.Tanggal.ToString("dd MMM yy");
                        Tanggaltxn = selectedObject.Tanggal;
                    }
                }

                Keterangan = $"Penerimaan Kasir tgl {Tanggal}, sebanyak {Total} TTB, Rp. {TotalUang.ToString("N0")} ";
            }
            //else
            //{
            //    SelectedPenerimaanKasirs = (ArrayList)e.SelectedObjects;
            //}

            //ObjectSpace.CommitChanges();
            //ObjectSpace.Refresh();
            if (TotalUang > 0)
            {
                objectSpaceInternal = Application.CreateObjectSpace(typeof(BankTran));
                obj = objectSpaceInternal.CreateObject<BankTran>();
                obj.Description = Keterangan;
                obj.Date = Tanggaltxn;
                obj.Tipe = eBankTran.Penerimaan;
                obj.Jumlah = TotalUang;
                string tUser = SecuritySystem.CurrentUserName.ToString();
                UserLogin OUser = objectSpaceInternal.FindObject<UserLogin>(new BinaryOperator("UserName", tUser));
                // SET  PENERIMAAN KAS
                if (OUser != null)
                {
                    if (OUser.Perusahaan != null)
                    {
                        if (OUser.Perusahaan.BankPenerimaanDP != null)
                        {
                            Bank oBank = objectSpaceInternal.FindObject<Bank>(new BinaryOperator("This", OUser.Perusahaan.BankPenerimaanDP));
                            obj.Bank = oBank;
                            

                        }
                    }
                }

                //fTransCode oTransCode = objectSpaceInternal.FindObject<fTransCode>(new BinaryOperator("This", OUser.Perusahaan.PenerimaanTunai));
                fTransCode oTransCode = OUser.Perusahaan.PenerimaanTunai;
                obj.TransCode = oTransCode;
                obj.CreateJournal(TotalUang, oTransCode);

                //DetailView confirmationDetailView = Application.CreateDetailView(objectSpaceInternal, obj);
                DetailView confirmationDetailView = Application.CreateDetailView(objectSpaceInternal, "PKBankTran_DetailView_PenerimanKasir", false, obj);
                confirmationDetailView.ViewEditMode = ViewEditMode.View;
                confirmationDetailView.Model.AllowEdit = true;
                confirmationDetailView.Model.AllowDelete = false;
                confirmationDetailView.Model.AllowNew = false;

                confirmationDetailView.Caption = confirmationDetailView.Caption;// + " " + lv.Caption;
                Application.ShowViewStrategy.ShowViewInPopupWindow(confirmationDetailView, OkDelegate, CancelDelegate);
            }
            else
            {
                Application.ShowViewStrategy.ShowMessage($"tidak ada data " );
            }
            //IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(BankTran));
            //string noteListViewId = Application.FindLookupListViewId(typeof(BankTran));
            //CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(Note), noteListViewId);
            //e.View = Application.CreateListView(noteListViewId, collectionSource, true);
            //if ( isSave)
            //{
            //    obj.Save();
            //    obj.Session.CommitTransaction();
            //    foreach (PenerimaanKasir selectedContact in SelectedPenerimaanKasirs)
            //    {
            //        //DateTime now = DateTime.Now;
            //        //selectedContact.Notes += "\r\n[INFO] Your salary is transfered " +
            //        //    now.ToString("M/d/yy") + " at " + now.ToString("hh:mm");

            //        string sqlQuery = string.Format(" Oid == {0} ", selectedContact.Oid.ToString());
            //        CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);
            //        PenerimaanKasir oPenerimaanKasir = objectSpaceInternal.FindObject<PenerimaanKasir>(filterOperator);

            //        oPenerimaanKasir.Kasbank = obj;
            //        //oPenerimaanKasir.NoTransaksiKas = obj.Reference;
            //        oPenerimaanKasir.Disetor = true;
            //        oPenerimaanKasir.Save();
            //        oPenerimaanKasir.Session.CommitTransaction();
            //        //selectedContact.Kasbank = obj;
            //        //selectedContact.Save();
            //        //selectedContact.Session.CommitTransaction();

            //    }
            //    Application.ShowViewStrategy.ShowMessage($"Data di Simpan {obj.Reference}" + View.ObjectTypeInfo.Type.Name);
            //}
        }

        public  void OkDelegate()
        {
            isSave = true;
            //.GetType().typeof(oImportFiles));
            obj.Save();
            obj.Session.CommitTransaction();
            foreach (PenerimaanKasir selectedContact in SelectedPenerimaanKasirs)
            {
                //DateTime now = DateTime.Now;
                //selectedContact.Notes += "\r\n[INFO] Your salary is transfered " +
                //    now.ToString("M/d/yy") + " at " + now.ToString("hh:mm");

                string sqlQuery = string.Format(" Oid == {0} ", selectedContact.Oid.ToString());
                CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);
                PenerimaanKasir oPenerimaanKasir = objectSpaceInternal.FindObject<PenerimaanKasir>(filterOperator);

                oPenerimaanKasir.Kasbank = obj;
                oPenerimaanKasir.Disetor = true;
                oPenerimaanKasir.Save();
                oPenerimaanKasir.Session.CommitTransaction();
                //selectedContact.Kasbank = obj;
                //selectedContact.Save();

            }
            ///selectedContact.Session.CommitTransaction();

            Application.ShowViewStrategy.ShowMessage($"Data di Simpan {obj.Reference}" + View.ObjectTypeInfo.Type.Name);
            ObjectSpace.Refresh();
        }
        public void CancelDelegate()
        {
            //Application.ShowViewStrategy.ShowMessage("The message is canceled!" + View.ObjectTypeInfo.Type.Name);
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
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
            //throw new NotImplementedException();
        }

        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            if (View.CurrentObject == e.Object)
            {
                if (this.ObjectSpace.IsModified)
                {
                    this.ObjectSpace.CommitChanges();
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
            dlgController = Frame.GetController<DialogController>();
            if (dlgController != null)
            {
                dlgController.AcceptAction.Execute += AcceptAction_Execute;
                dlgController.CancelAction.Execute += CancelAction_Execute;
                dlgController = null;

            }
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
