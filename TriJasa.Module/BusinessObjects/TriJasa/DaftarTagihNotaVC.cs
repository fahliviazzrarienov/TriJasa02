using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Xpo;
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
    public partial class DaftarTagihNotaVC : ViewController
    {
        public DaftarTagihNotaVC()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetViewType = ViewType.ListView;
            //TargetViewId = "BankTran_ListView_Pelunasan";
            TargetObjectType = typeof(DaftarTagihanNota);
            SingleChoiceAction NotaTagihAction = new SingleChoiceAction(this, "NotaTagih", PredefinedCategory.Unspecified);
            NotaTagihAction.Caption = "Nota Tagih";
            NotaTagihAction.ImageName = @"Office2013\Issue_16x16";
            
            NotaTagihAction.ItemType = SingleChoiceActionItemType.ItemIsOperation;
            ChoiceActionItem NotaTagih01 = new ChoiceActionItem("NotaTagih01", "Kembali", null);
            NotaTagih01.ImageName = "Action_Cancel_16x16";
            ChoiceActionItem NotaTagih02 = new ChoiceActionItem("NotaTagih02", "Diterima", null);
            NotaTagih02.ImageName = "BO_Validation_16x16";
            NotaTagihAction.TargetViewType = ViewType.ListView;
            NotaTagihAction.TargetObjectType = typeof(DaftarTagihanNota);
            NotaTagihAction.Items.Add(NotaTagih01);
            NotaTagihAction.Items.Add(NotaTagih02);
            NotaTagihAction.Execute += NotaTagihAction_Execute;
        }
        private DaftarTagihUpdate obj;
        private ArrayList SelectedDaftarTagihNota;
        private eTagihan SelectedStatus ;
        private void NotaTagihAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            //if (View.CurrentObject != null)
            //{
            string ReportName = "";
            iGenTriJasa oGen = new iGenTriJasa();
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(DaftarTagihanNota));
         //   ArrayList aListDM = new ArrayList();
            CriteriaOperator oCriteria = null;
            if ((e.SelectedObjects.Count > 0))
            {
                SelectedDaftarTagihNota = new ArrayList();
                double Jumlah = 0;
                int total = 0;
                foreach (DaftarTagihanNota selectedObject in e.SelectedObjects)
                {
                    // aListDM.Add(selectedObject.Oid);
                    //selectedObject.Printed();
                    SelectedDaftarTagihNota.Add((DaftarTagihanNota)ObjectSpace.GetObject(selectedObject));
                    Jumlah += selectedObject.Nota.TotalBiaya;
                    total++;
                    if (e.SelectedChoiceActionItem.Id == "NotaTagih01")
                    {
                        SelectedStatus = eTagihan.Kembali;
                       // selectedObject.Status = eTagihan.Kembali;
                        // oCriteria = GroupOperator.And(oCriteria, new InOperator("NotaSuratMuat.Oid", aListDM));
                        //  ReportName = oGen.ReportNameGet(objectSpace, "NotaPPrint01");
                    }
                    else if (e.SelectedChoiceActionItem.Id == "NotaTagih02")
                    {
                        SelectedStatus = eTagihan.Diterima;
                        //selectedObject.Status = eTagihan.Diterima;
                    }
                    //selectedObject.Save();
                    //selectedObject.Session.CommitTransaction();
                }

                IObjectSpace objectSpaceInternal = Application.CreateObjectSpace(typeof(DaftarTagihUpdate));
                obj = objectSpaceInternal.CreateObject<DaftarTagihUpdate>();
                obj.Tanggal = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day);
                obj.Jumlah = Jumlah;
                obj.Total = total;
                obj.Status = SelectedStatus;

                // DetailView confirmationDetailView = Application.CreateDetailView(objectSpaceInternal, "NotaPayment_DetailView_Pelunasan", false, obj);
                DetailView confirmationDetailView = Application.CreateDetailView(objectSpaceInternal, obj);
                confirmationDetailView.ViewEditMode = ViewEditMode.View;
                confirmationDetailView.Model.AllowEdit = true;
                confirmationDetailView.Model.AllowDelete = false;
                confirmationDetailView.Model.AllowNew = false;

                confirmationDetailView.Caption = confirmationDetailView.Caption;// + " " + lv.Caption;
                Application.ShowViewStrategy.ShowViewInPopupWindow(confirmationDetailView, OkDelegate, CancelDelegate);
                
            }
            //objectSpace.Refresh();

        }

        private void CancelDelegate()
        {
            //throw new NotImplementedException();
            Application.ShowViewStrategy.ShowMessage($"Cancel");
        }

        private void OkDelegate()
        {
            // throw new NotImplementedException();
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(DaftarTagihanNota));
            foreach (DaftarTagihanNota selectedNota in SelectedDaftarTagihNota)
            {
                
                string sqlQuery = string.Format(" Oid == {0} ", selectedNota.Oid.ToString());
                CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);
                DaftarTagihanNota oNota = objectSpace.FindObject<DaftarTagihanNota>(filterOperator);

                oNota.TglUpdate = obj.Tanggal;
                oNota.Notes = obj.Keterangan;
                oNota.Status = SelectedStatus;
                oNota.Save();
                oNota.Session.CommitTransaction();
            }
            this.ObjectSpace.CommitChanges();
            this.ObjectSpace.Refresh();
            //objectSpace.Refresh();
            Application.ShowViewStrategy.ShowMessage($" Update {SelectedStatus.ToString()} Sebanyak ");
        }

        private DevExpress.Xpo.Session oSession;
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            oSession = ((XPObjectSpace)ObjectSpace).Session;

            ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
        }

        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            if (View.CurrentObject == e.Object)
            {
                //if (((DaftarTagihanNota)View.CurrentObject) != null)
                //{
                    if (this.ObjectSpace.IsModified)
                    {
                        //((DaftarTagihan)View.CurrentObject).AddNota();

                        this.ObjectSpace.CommitChanges();
                    }
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
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
