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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TriJasa.Module.BusinessObjects
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class SupirKlaimvc : ViewController
    {
        public SupirKlaimvc()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetObjectType = typeof(SupirKlaim);
            //TargetViewId = "DaftarMuatan_DetailView_Entry";
            //TargetViewType = ViewType.DetailView;
            SingleChoiceAction SKPPrintAction = new SingleChoiceAction(this, "SKPPrint", PredefinedCategory.Unspecified);
            SKPPrintAction.Caption = "Print";
            SKPPrintAction.ImageName = "Action_Printing_Print";
            SKPPrintAction.ItemType = SingleChoiceActionItemType.ItemIsOperation;
            ChoiceActionItem SKPPPrint01 = new ChoiceActionItem("SKPPPrint01", "Supir Klaim", null);
            ChoiceActionItem SKPPPrint02 = new ChoiceActionItem("SKPPPrint02", "Rekap Klaim Supir", null);
            SKPPrintAction.TargetViewType = ViewType.ListView;
            SKPPrintAction.TargetObjectType = typeof(SupirKlaim);
            SKPPrintAction.Items.Add(SKPPPrint01);
            SKPPrintAction.Items.Add(SKPPPrint02);
            //DMEPrintAction.Items.Add(DMEPPrintSM);
            SKPPrintAction.Execute += SKPPrintAction_Execute;
        }

        private void SKPPrintAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            //  throw new NotImplementedException();
            string ReportName = "";
            CriteriaOperator filterOperator=null;
             iGenTriJasa oGen = new iGenTriJasa();
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(SupirKlaim));
            //((PenerimaanKasir)View.CurrentObject).Save();
            //((PenerimaanKasir)View.CurrentObject).Session.CommitTransaction();

            //if (this.ObjectSpace.IsModified)
            //{
            //    this.ObjectSpace.CommitChanges();
            //}

            ArrayList aListDM = new ArrayList();
            CriteriaOperator oCriteria = null;
            //PenerimaanKasir B;
            //B.Oid
            //string sqlQuery = string.Format("Oid ={0}", ((SupirKlaim)View.CurrentObject).Oid);
            //CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);
            //ReportName = oGen.ReportNameGet(objectSpace, "SKPPPrint01");
            //PrintReports(ReportName, filterOperator);
            if (e.SelectedChoiceActionItem.Id == "SKPPPrint01")
            {

                oCriteria = null;
                ReportName = oGen.ReportNameGet(objectSpace, "SKPPPrint01");
                PrintReports(ReportName, oCriteria);
            }
            else if (e.SelectedChoiceActionItem.Id == "SKPPPrint02")
            {
                oCriteria = null;
                ReportName = oGen.ReportNameGet(objectSpace, "SKPPPrint02");
                PrintReports(ReportName, oCriteria);
            }
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
