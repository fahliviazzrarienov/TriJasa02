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
    public partial class PenerimaanKasirPrint : ViewController
    {
        public PenerimaanKasirPrint()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetObjectType = typeof(PenerimaanKasir);
            TargetViewType = ViewType.DetailView;


            //TargetViewId = "SuratMuatan_DetailView_Entry";

            SimpleAction PKVEPrintAction = new SimpleAction(this, "PKVEPrint", PredefinedCategory.Unspecified)
            {
                Caption = "Print",
                //ConfirmationMessage = "Are you sure you want to clear the Tasks list?",
                ImageName = "Action_Printing_Print"
            };
            PKVEPrintAction.TargetViewType = ViewType.DetailView;
            PKVEPrintAction.Category = "CethoKasir";
            PKVEPrintAction.Execute += PKVEPrintAction_Execute;

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

        private void PKVEPrintAction_Execute(Object sender, SimpleActionExecuteEventArgs e)
        {

            string ReportName = "";
            iGenTriJasa oGen = new iGenTriJasa();
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(PenerimaanKasir));
            ((PenerimaanKasir)View.CurrentObject).Save();
            ((PenerimaanKasir)View.CurrentObject).Session.CommitTransaction();

            if ( this.ObjectSpace.IsModified)
            {
                this.ObjectSpace.CommitChanges();
            }

            ArrayList aListDM = new ArrayList();
            CriteriaOperator oCriteria = null;
            PenerimaanKasir B;
            //B.Oid
            string sqlQuery = string.Format("Oid ={0}", ((PenerimaanKasir)View.CurrentObject).Oid);
            //string sqlQuery = string.Format($"Oid={(PenerimaanKasir)View.CurrentObject).Oid}");
            CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);
            //oCriteria = GroupOperator.And(oCriteria, new BinaryOperator("Oid", this));
            //oCriteria = GroupOperator.And(oCriteria, new InOperator("Oid", aListDM));
            ReportName = oGen.ReportNameGet(objectSpace, "PK01Print");
            PrintReports(ReportName, filterOperator);

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
