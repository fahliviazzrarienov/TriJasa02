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
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TriJasa.Module.BusinessObjects
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class BankTranPelunasanVC : ViewController
    {
        public BankTranPelunasanVC()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetViewType = ViewType.ListView;
            TargetViewId = "BankTran_ListView_Pelunasan";
            TargetObjectType = typeof(BankTran);
            //TargetObjectType = typeof(Note);
            // Target required Views (via the TargetXXX properties) and create their Actions.
            SimpleAction BankPelunasanAction = new SimpleAction(this, "BankPelunasan", "Edit");
            
            BankPelunasanAction.Caption = "Pelunasan";
            BankPelunasanAction.ImageName = "NewTask_16x16";
            //BankPelunasanAction.CustomizePopupWindowParams += PopupWindow_CustomizePopupWindowParams;

            BankPelunasanAction.SelectionDependencyType = SelectionDependencyType.RequireMultipleObjects;
            BankPelunasanAction.TargetObjectType = typeof(BankTran);
            BankPelunasanAction.Execute += BankPelunasanAction_Execute;
            Actions.Add(BankPelunasanAction);

            

            //SimpleAction BankRekonsilasiAction = new SimpleAction(this, "BankRekonsilasi", "Edit");
            //BankRekonsilasiAction.Caption = "Rekonsilasi";
            //BankRekonsilasiAction.ImageName = "BO_Task_16x16";
            //BankRekonsilasiAction.SelectionDependencyType = SelectionDependencyType.RequireMultipleObjects;
            //BankRekonsilasiAction.TargetObjectType = typeof(BankTran);

            //BankRekonsilasiAction.Execute += BankRekonsilasiAction_Execute;
            //Actions.Add(BankRekonsilasiAction);

        }

        private void PelunasanPrintAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            // throw new NotImplementedException();
            //string ReportName = "";
            //iGenTriJasa oGen = new iGenTriJasa();
            //IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(BankTran));
            //ArrayList aListDM = new ArrayList();
            //CriteriaOperator oCriteria = null;
           
            //if (e.SelectedChoiceActionItem.Id == "PelunasanPrint01")
            //{

            //    oCriteria = null;
            //    ReportName = oGen.ReportNameGet(objectSpace, "PelunasanPrint01");
            //    PrintReports(ReportName, oCriteria);
            //}
            //objectSpace.Refresh();
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
                // controller.SetupBeforePrint.p
                controller.ShowPreview(handle, oCriteria);

            }

        }
        void BankRekonsilasiAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if ((e.SelectedObjects.Count > 0))
            {
                foreach (BankTran selectedObject in e.SelectedObjects)
                {
                    selectedObject.Rekonsilasi = true;
                    selectedObject.Save();
                    selectedObject.Session.CommitTransaction();
                }

                ObjectSpace.Refresh();
                //Keterangan = $"Penerimaan Kasir tgl {Tanggal}, sebanyak {Total} TTB, Rp. {TotalUang.ToString("N0")} ";
            }

        }
        private NotaPayment obj;
        private ArrayList SelectedPenerimaanKasirs;
        private IObjectSpace objectSpaceInternal;
        private bool isSave = false;
        int BankTranOid = -1;
        void BankPelunasanAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            //BankTran task = (BankTran)View.CurrentObject;
            isSave = false;
            double TotalUang = 0;
            string Keterangan = "";
            string Tanggal = System.DateTime.Now.ToString("dd MMM yy");
            int Total = 0;
             BankTranOid = -1;
            DateTime Tanggaltxn = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day);
            SelectedPenerimaanKasirs = new ArrayList();
            //if ((e.SelectedObjects.Count > 0) && (e.SelectedObjects[0] is IObjectRecord))
            if ((e.SelectedObjects.Count > 0))
            {
                foreach (BankTran selectedObject in e.SelectedObjects)
                {
                    BankTranOid = selectedObject.Oid;
                    //Application.ShowViewStrategy.ShowMessage($"tidak ada data {selectedObject.Reference}");
                    break;
                }

                Keterangan = $"Penerimaan Kasir tgl {Tanggal}, sebanyak {Total} TTB, Rp. {TotalUang.ToString("N0")} ";
            }
            //else
            //{
            //    SelectedPenerimaanKasirs = (ArrayList)e.SelectedObjects;
            //}

            //ObjectSpace.CommitChanges();
            //ObjectSpace.Refresh();
            if (BankTranOid > 0)
            {
                objectSpaceInternal = Application.CreateObjectSpace(typeof(NotaPayment));
                string tUser = SecuritySystem.CurrentUserName.ToString();
                UserLogin OUser = objectSpaceInternal.FindObject<UserLogin>(new BinaryOperator("UserName", tUser));
                obj = objectSpaceInternal.CreateObject<NotaPayment>();
             
                string sqlQuery = $" Oid == {BankTranOid} ";
                CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);
                BankTran oBank = objectSpaceInternal.FindObject<BankTran>(filterOperator);
                if (oBank != null)
                {
                    //XPCollection<NotaPayment> oNotaPayment = new XPCollection<NotaPayment>(obj.Session);
                    //double oTelahDiTerima = oNotaPayment.Where(x => x.Reference == oBank).Sum(x => x.Jumlah);

                    //oBank.BankAlokasi = oTelahDiTerima;
                    obj.Bank = oBank.Bank;
                    obj.Reference = oBank;
                    obj.Referensi = oBank.Reference;
                    obj.NoCekBG = oBank.Giro;
                    obj.Pelanggan = oBank.Pelanggan;
                    obj.Jmlditerima = oBank.Jumlah- oBank.BankAlokasi;
                    obj.Tanggal = Tanggaltxn;
                    obj.BankTglTerima = oBank.Date;
                    obj.BankTglTempo = oBank.GiroTanggal;
                    obj.Nomor = obj.Number(Tanggaltxn);


                }


                //obj.Date = Tanggaltxn;
                //obj.Tipe = eBankTran.Penerimaan;
                //obj.Description = Keterangan;
                //obj.Jumlah = TotalUang;
             
                // SET  PENERIMAAN KAS
                if (OUser != null)
                {
                    if (OUser.Perusahaan != null)
                    {
                        if (OUser.Perusahaan.BankPenerimaanDP != null)
                        {
                            //Bank oBank = objectSpaceInternal.FindObject<Bank>(new BinaryOperator("This", OUser.Perusahaan.BankPenerimaanDP));
                            //obj.Bank = oBank;

                        }
                    }
                }

                //fTransCode oTransCode = objectSpaceInternal.FindObject<fTransCode>(new BinaryOperator("This", OUser.Perusahaan.PenerimaanTunai));
                fTransCode oTransCode = OUser.Perusahaan.PenerimaanTunai;
              //  obj.CreateJournal(TotalUang, oTransCode);

                //DetailView confirmationDetailView = Application.CreateDetailView(objectSpaceInternal, obj);
                DetailView confirmationDetailView = Application.CreateDetailView(objectSpaceInternal, "NotaPayment_DetailView_Pelunasan", false, obj);
                confirmationDetailView.ViewEditMode = ViewEditMode.View;
                confirmationDetailView.Model.AllowEdit = true;
                confirmationDetailView.Model.AllowDelete = false;
                confirmationDetailView.Model.AllowNew = false;

                confirmationDetailView.Caption = confirmationDetailView.Caption;// + " " + lv.Caption;
                Application.ShowViewStrategy.ShowViewInPopupWindow(confirmationDetailView, OkDelegate, CancelDelegate);
            }
            else
            {
                Application.ShowViewStrategy.ShowMessage($"tidak ada data ");
            }
          
        }
        public void OkDelegate()
        {
            isSave = true;
            //.GetType().typeof(oImportFiles));
            Pelanggan oPelanggan = obj.NotaPaymentDtl
                        .GroupBy(x => new { x.NotaPayment.Pelanggan })
                       .Select(group => new { Payment = group.Key })
                       .Max(x => x.Payment.Pelanggan);
            obj.Pelanggan = oPelanggan;
            iGenTriJasa oGenTriJasa = new iGenTriJasa();
            Session newSession = obj.Session;
            IList aodtl = obj.NotaPaymentDtl;
            foreach (NotaPaymentDtl odtl in aodtl)
            {
                if (odtl.Pot != 0)
                {
                    DaftarMuatan oDM = odtl.Faktur.SuratMuats
                    .GroupBy(x => new { x.NomorDM })
                    .Select(group => new { DM = group.Key })
                    .Max(x => x.DM.NomorDM);

                    //string sQuery = $" Oid == {BankTranOid} ";
                    //CriteriaOperator ofilterOperator = CriteriaOperator.Parse(sQuery);
                    //DaftarMuatan oSupir = objectSpaceInternal.FindObject<BankTran>(ofilterOperator);
                    string sKeterangan = $"Nota:{ odtl.Faktur.NoNota}  pelanggan{odtl.Faktur.PelanganNama} ";
                    oGenTriJasa.SupirKlaimSet(newSession, eSupirKlaim.Pembayaran, oDM.NamaSpr, oDM, odtl.Pot, sKeterangan, obj.Tanggal);
                }
            }

            //obj.Jumlah
            obj.Save();
            obj.Session.CommitTransaction();

            string sqlQuery = $" Oid == {BankTranOid} ";
            CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);
            BankTran oBank = objectSpaceInternal.FindObject<BankTran>(filterOperator);
            oBank.Pelanggan = oPelanggan;

            if ( oBank.Jumlah==oBank.BankAlokasi)
            {
                oBank.Rekonsilasi = true;
            }
            oBank.Save();
            try
            {
                oBank.Session.CommitTransaction();
                this.ObjectSpace.CommitChanges();
            }
            catch(Exception  )
            { }
            //ObjectSpace.Refresh();
            //XPCollection<NotaPayment> oNotaPayment = new XPCollection<NotaPayment>(obj.Session);
            //double oTelahDiTerima = oNotaPayment.Where(x => x.Reference == oBank).Sum(x => x.Jumlah);
            //oBank.BankAlokasi = oTelahDiTerima;

            //foreach (PenerimaanKasir selectedContact in SelectedPenerimaanKasirs)
            //{
            //    //DateTime now = DateTime.Now;
            //    //selectedContact.Notes += "\r\n[INFO] Your salary is transfered " +
            //    //    now.ToString("M/d/yy") + " at " + now.ToString("hh:mm");

            //    string sqlQuery = string.Format(" Oid == {0} ", selectedContact.Oid.ToString());
            //    CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);
            //    PenerimaanKasir oPenerimaanKasir = objectSpaceInternal.FindObject<PenerimaanKasir>(filterOperator);

            //    oPenerimaanKasir.Kasbank = obj;
            //    oPenerimaanKasir.Disetor = true;
            //    oPenerimaanKasir.Save();
            //    oPenerimaanKasir.Session.CommitTransaction();
            //    //selectedContact.Kasbank = obj;
            //    //selectedContact.Save();

            //}
            /////selectedContact.Session.CommitTransaction();

            //Application.ShowViewStrategy.ShowMessage($"Data di Simpan {obj.Reference}" + View.ObjectTypeInfo.Type.Name);
            //ObjectSpace.Refresh();
        }
        public void CancelDelegate()
        {
            //Application.ShowViewStrategy.ShowMessage("The message is canceled!" + View.ObjectTypeInfo.Type.Name);
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
