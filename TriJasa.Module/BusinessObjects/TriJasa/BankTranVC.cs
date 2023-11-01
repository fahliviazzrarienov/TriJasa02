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
    public partial class BankTranVC : ViewController<ListView>
    {
        public BankTranVC()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetObjectType = typeof(BankTran);
            TargetViewType = ViewType.ListView;
            SingleChoiceAction BKTPrintAction = new SingleChoiceAction(this, "BankTransPrint", PredefinedCategory.Unspecified);
            BKTPrintAction.Caption = "Print";
            BKTPrintAction.ImageName = "Action_Printing_Print";
            BKTPrintAction.ItemType = SingleChoiceActionItemType.ItemIsOperation;
            ChoiceActionItem BKTansPrint = new ChoiceActionItem("BKTansPrint", "Bank & Kas Print", null);
            ChoiceActionItem PelunasanPPrint01 = new ChoiceActionItem("PelunasanPrint01", "Laporan Pelunasan", null);
            ChoiceActionItem BKPrint01 = new ChoiceActionItem("BKPrint01", "Rekap Pengeluaran", null);



            BKTPrintAction.TargetViewType = ViewType.ListView;
            BKTPrintAction.TargetObjectType = typeof(BankTran);
            BKTPrintAction.Items.Add(BKTansPrint);
            BKTPrintAction.Items.Add(PelunasanPPrint01);
            BKTPrintAction.Items.Add(BKPrint01);
            ///PKLPrintAction.Items.Add(PKVLPrintRekap);
            BKTPrintAction.Execute += BKTPrintAction_Execute;

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
        private void BKTPrintAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            string ReportName = "";
            iGenTriJasa oGen = new iGenTriJasa();
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(BankTran));
            ArrayList aListDM = new ArrayList();
            CriteriaOperator oCriteria = null;
            
            if (e.SelectedChoiceActionItem.Id == "BKTansPrint")
            {
                ReportName = oGen.ReportNameGet(objectSpace, "BKTansPrint");
                PrintReports(ReportName, oCriteria);
            }
            else if (e.SelectedChoiceActionItem.Id == "PelunasanPrint01")
            {

                oCriteria = null;
                ReportName = oGen.ReportNameGet(objectSpace, "PelunasanPrint01");
                PrintReports(ReportName, oCriteria);
            }
            else if (e.SelectedChoiceActionItem.Id == "BKPrint01")
            {

                oCriteria = null;
                ReportName = oGen.ReportNameGet(objectSpace, "BKPrint01");
                PrintReports(ReportName, oCriteria);
            }
            //else if (e.SelectedChoiceActionItem.Id == "PKVLPrintRekap")
            //{
            //    ReportName = oGen.ReportNameGet(objectSpace, "PKVLPrintRekap");
            //    PrintReports(ReportName, oCriteria);
            //}
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;

        }

        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            if (View.CurrentObject == e.Object)
            {
                //if (((DaftarTagihan)View.CurrentObject).NamaColl != null)
                //{
                //if (this.ObjectSpace.IsModified) 
                //    {
                //    BankTran oBankTran = (BankTran)((DetailView)View).CurrentObject;
                //    PropertyEditor peDescription = ((DetailView)View).FindItem("Description") as PropertyEditor;
                //    ListPropertyEditor peBankTranDtl = ((DetailView)View).FindItem("TransKasDtl") as ListPropertyEditor;
                //    XPCollection<fTransKasDtl> oBankTranDtl = (XPCollection<fTransKasDtl>)peBankTranDtl.PropertyValue;
                //    int i= 0;
                //    string sket = peDescription.PropertyValue.ToString();
                //    foreach (fTransKasDtl oTransKasDtl in oBankTranDtl)
                //    {
                //        if (i==0)
                //        {
                //            if (oTransKasDtl.Description.Contains(sket)==false )
                //            {


                //            }
                //        }


                //    }
                //        //((DaftarTagihan)View.CurrentObject).AddNota();

                //        //this.ObjectSpace.CommitChanges();
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
            // Unsubscribe from previously subscribed events and release other references and resources.
            ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
            base.OnDeactivated();
        }
    }
}
