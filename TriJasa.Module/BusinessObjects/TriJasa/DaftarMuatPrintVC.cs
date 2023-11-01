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
    public partial class DaftarMuatPrintVC : ViewController
    {
        public DaftarMuatPrintVC()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetObjectType = typeof(DaftarMuatan);
            //TargetViewId = "DaftarMuatan_DetailView_Entry";
            //TargetViewType = ViewType.DetailView;
            SingleChoiceAction DMEPrintAction = new SingleChoiceAction(this, "DMEPrint", PredefinedCategory.Unspecified);
            DMEPrintAction.Caption = "Print";
            DMEPrintAction.ImageName = "Action_Printing_Print";
            DMEPrintAction.ItemType = SingleChoiceActionItemType.ItemIsOperation;
            ChoiceActionItem DMEPPrintDM = new ChoiceActionItem("DMEPPrintDM", "Print DM", null);
            //ChoiceActionItem DMEPPrintSM = new ChoiceActionItem("DMEPPrintSM", "Print SM", null);
            DMEPrintAction.TargetViewType = ViewType.DetailView;
            DMEPrintAction.TargetObjectType = typeof(DaftarMuatan);
            DMEPrintAction.Items.Add(DMEPPrintDM);
            //DMEPrintAction.Items.Add(DMEPPrintSM);
            DMEPrintAction.Execute += DMEPrintAction_Execute;


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
        private void DMEPrintAction_Execute(Object sender, SimpleActionExecuteEventArgs e)
        {
            //while (((SuratMuatan)View.CurrentObject).Tasks.Count > 0)
            //{
            //    ((Contact)View.CurrentObject).Tasks.Remove(((Contact)View.CurrentObject).Tasks[0]);
            //}
            // ObjectSpace.SetModified(View.CurrentObject);


            string ReportName = "";
            iGenTriJasa oGen = new iGenTriJasa();
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(DaftarMuatan));
            ArrayList aListDM = new ArrayList();
            CriteriaOperator oCriteria = null;

            if (View.CurrentObject is DaftarMuatan)
            {
                DaftarMuatan oDaftarMuatan = (DaftarMuatan)View.CurrentObject;

                foreach (SuratMuatan oSM in oDaftarMuatan.SuratMuatan)
                {
                    oSM.UpdateJenisBarang(true);
                    oSM.Save();
                    oSM.Session.CommitTransaction();

                }
                oDaftarMuatan.Save();
                oDaftarMuatan.Session.CommitTransaction();
                objectSpace.CommitChanges();
                objectSpace.Refresh();
            }

                string sqlQuery = string.Format("NomorDM.Oid = {0}  ", ((DaftarMuatan)View.CurrentObject).Oid);
            CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);


            oCriteria = GroupOperator.And(oCriteria, new InOperator("NomorDM.Oid", aListDM));
            ReportName = oGen.ReportNameGet(objectSpace, "DMPPrintDM");
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
