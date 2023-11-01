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
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace TriJasa.Module.BusinessObjects
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class BankTrasferVC : ViewController
    {
        public BankTrasferVC()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
           
            TargetObjectType = typeof(BankTrasfer);
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
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

        private void DeleteAction_Executing(object sender, CancelEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(BankTrasfer));
            foreach (BankTrasfer oBankTrasfer in View.SelectedObjects)
            {
                e.Cancel = true;
                if (!oBankTrasfer.Hapus)
                {
                    //if (contact.Position != null && contact.Position.Title == "Manager")
                    //{
                    //    e.Cancel = true;
                    //}
                   

                    BankTrasfer oBTrasfer = objectSpace.CreateObject<BankTrasfer>();
                    /*
                    CriteriaOperator filterOperator;
                    string sqlQuery;
                    sqlQuery = string.Format(" Oid == {0} ", oBankTrasfer.BankKe.Oid.ToString());
                    filterOperator = CriteriaOperator.Parse(sqlQuery);
                    Bank oBankDari = objectSpace.FindObject<Bank>(filterOperator);

                    sqlQuery = string.Format(" Oid == {0} ", oBankTrasfer.BankDari.Oid.ToString());
                    filterOperator = CriteriaOperator.Parse(sqlQuery);
                    Bank oBankKe = objectSpace.FindObject<Bank>(filterOperator);

                    oBTrasfer.BankDari = oBankDari;
                    oBTrasfer.BankKe = oBankKe;
                    oBTrasfer.Tanggal = oBankTrasfer.Tanggal;
                    oBTrasfer.Jumlah = oBankTrasfer.Jumlah;
                    oBTrasfer.Keterangan = $"#Delete# {oBankTrasfer.Keterangan}";
                    oBTrasfer.Giro = oBankTrasfer.Giro;
                    oBTrasfer.GiroTanggal = oBankTrasfer.GiroTanggal;
                    */
                    oBTrasfer.VoidBank(oBankTrasfer);
                    oBTrasfer.Save();
                    oBTrasfer.Session.CommitTransaction();
                    //objectSpace.CommitChanges();
                    this.ObjectSpace.CommitChanges();
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage($"nomor {oBankTrasfer.Reference} Sudah dihapus");
                }
            }
            ObjectSpace.Refresh();
        }



        private void ObjectSpace_CustomDeleteObjects(object sender, CustomDeleteObjectsEventArgs e)
        {
            // throw new NotImplementedException();
          //  Application.ShowViewStrategy.ShowMessage($"Custom datele ");
        }

        private void DeleteController_Deleting(object sender, DeletingEventArgs e)
        {
            // throw new NotImplementedException();
            e.Cancel = true;
            //ObjectSpace.Delete(e.Objects);
            //Application.ShowViewStrategy.ShowMessage($"coba di delete ");
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            ObjectSpace.CustomDeleteObjects -= new EventHandler<CustomDeleteObjectsEventArgs>(ObjectSpace_CustomDeleteObjects);
            Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executing -= DeleteAction_Executing;
            base.OnDeactivated();
        }
    }
}
