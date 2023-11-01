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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TriJasa.Module.BusinessObjects
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class SuratMuatanvcPelunasan : ViewController
    {

        public SuratMuatanvcPelunasan()
        {
            InitializeComponent();
            TargetViewType = ViewType.ListView;
            TargetViewId = "BankTran_ListView_Pelunasan";

            SimpleAction PelunasanAction = new SimpleAction(this, "Pelunasan", "Edit");
            PelunasanAction.SelectionDependencyType = SelectionDependencyType.RequireMultipleObjects;
            PelunasanAction.TargetObjectType = typeof(PenerimaanKasir);
            
            PelunasanAction.Execute += PelunasanAction_Execute;
            Actions.Add(PelunasanAction);

        }
        private BankTran obj;
        private ArrayList SelectedPenerimaanKasirs;
        private IObjectSpace objectSpaceInternal;
        private bool isSave = false;
        void PelunasanAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            //BankTran task = (BankTran)View.CurrentObject;
            isSave = false;
            double TotalUang = 0;
            string Keterangan = "";
            string Tanggal = System.DateTime.Now.ToString("dd MMM yy");
            int Total = 0;
            SelectedPenerimaanKasirs = new ArrayList();
            //if ((e.SelectedObjects.Count > 0) && (e.SelectedObjects[0] is IObjectRecord))
            if ((e.SelectedObjects.Count > 0))
            {
                foreach (SuratMuatan selectedObject in e.SelectedObjects)
                {
                    if (selectedObject.TotalTagihan != selectedObject.TotalTagihan)
                    {
                        SelectedPenerimaanKasirs.Add((SuratMuatan)ObjectSpace.GetObject(selectedObject));
                        TotalUang += selectedObject.TotalTagihan;
                        Total++;
                        Tanggal = selectedObject.Tanggal.ToString("dd MMM yy");
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
                obj.Tipe = eBankTran.Penerimaan;
                obj.Description = Keterangan;
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
                obj.CreateJournal(TotalUang, oTransCode);

                //DetailView confirmationDetailView = Application.CreateDetailView(objectSpaceInternal, obj);
                DetailView confirmationDetailView = Application.CreateDetailView(objectSpaceInternal, "BankTran_DetailView_Entry", false, obj);
                confirmationDetailView.ViewEditMode = ViewEditMode.View;
                confirmationDetailView.Model.AllowEdit = false;
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
            obj.Save();
            obj.Session.CommitTransaction();
            foreach (PenerimaanKasir selectedContact in SelectedPenerimaanKasirs)
            {
                //DateTime now = DateTime.Now;
                //selectedContact.Notes += "\r\n[INFO] Your salary is transfered " +
                //    now.ToString("M/d/yy") + " at " + now.ToString("hh:mm");

                string sqlQuery = string.Format(" Oid == {0} ", selectedContact.Oid.ToString());
                CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);
                SuratMuatan oPenerimaanKasir = objectSpaceInternal.FindObject<SuratMuatan>(filterOperator);

                //oPenerimaanKasir.Kasbank = obj;
                //oPenerimaanKasir.Disetor = true;
                //oPenerimaanKasir.Save();
                //oPenerimaanKasir.Session.CommitTransaction();
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

        }

        private Frame masterFrame;

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();

        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
