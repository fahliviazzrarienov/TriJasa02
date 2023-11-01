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

namespace TriJasa.Module.BusinessObjects.TriJasa
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class KasBonVc : ViewController
    {
        public KasBonVc()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetViewType = ViewType.ListView;
            //TargetViewId = "BankTran_ListView_Pelunasan";
            TargetObjectType = typeof(KasBon);
            SingleChoiceAction KasbonPrintAction = new SingleChoiceAction(this, "KasbonPrint", PredefinedCategory.Unspecified);
            KasbonPrintAction.Caption = "Print";
            KasbonPrintAction.ImageName = "Action_Printing_Print";
            KasbonPrintAction.ItemType = SingleChoiceActionItemType.ItemIsOperation;
            ChoiceActionItem KasbonPPrint01 = new ChoiceActionItem("KasbonPPrint01", "Kartu Kasbon & Klaim", null);
            ChoiceActionItem KasbonPPrint02 = new ChoiceActionItem("KasbonPPrint02", "Rekap Kasbon & Klaim", null);
            ///ChoiceActionItem KasbonPPrint03 = new ChoiceActionItem("KasbonPPrint03", "Print Nota PPN ", null);
            ///ChoiceActionItem KasbonPPrint04 = new ChoiceActionItem("KasbonPPrint04", "Rekap Status Nota ", null);

            KasbonPrintAction.TargetViewType = ViewType.ListView;
            KasbonPrintAction.TargetObjectType = typeof(KasBon);
            KasbonPrintAction.Items.Add(KasbonPPrint01);
            KasbonPrintAction.Items.Add(KasbonPPrint02);
            //KasbonPrintAction.Items.Add(KasbonPPrint03);
            //KasbonPrintAction.Items.Add(KasbonPPrint04);
            KasbonPrintAction.Execute += KasbonPrintAction_Execute;

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
        private void KasbonPrintAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            //if (View.CurrentObject != null)
            //{
            string ReportName = "";
            iGenTriJasa oGen = new iGenTriJasa();
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(KasBon));
            ArrayList aListKasBon = new ArrayList();
            CriteriaOperator oCriteria = null;
            if ((e.SelectedObjects.Count > 0))
            {
                foreach (KasBon selectedObject in e.SelectedObjects)
                {
                    aListKasBon.Add(selectedObject.Oid);
                    //selectedObject.Printed();
                    //selectedObject.Save();
                    //selectedObject.Session.CommitTransaction();

                }
                if (e.SelectedChoiceActionItem.Id == "KasbonPPrint01x")
                {

                    oCriteria = GroupOperator.And(oCriteria, new InOperator("NotaSuratMuat.Oid", aListKasBon));
                    ReportName = oGen.ReportNameGet(objectSpace, "KasbonPPrint01");
                }
                else if (e.SelectedChoiceActionItem.Id == "KasbonPPrint02x")
                {
                    //SuratMuatan o;
                    //o.NotaSuratMuat
                    ///oCriteria = GroupOperator.And(oCriteria, new InOperator("NotaSuratMuat.Oid", aListDM));
                    ReportName = oGen.ReportNameGet(objectSpace, "NotaPPrint02");
                }
                else if (e.SelectedChoiceActionItem.Id == "NotaPPrint03")
                {
                    //SuratMuatDtl o;
                    //o.SuratMuat.NomorDM
                    oCriteria = GroupOperator.And(oCriteria, new InOperator("NotaSuratMuat.Oid", aListKasBon));
                    ReportName = oGen.ReportNameGet(objectSpace, "NotaPPrint03");
                }
                PrintReports(ReportName, oCriteria);
            }
            if (e.SelectedChoiceActionItem.Id == "KasbonPPrint01")
            {

                oCriteria = null;
                ReportName = oGen.ReportNameGet(objectSpace, "KasbonPPrint01");
                PrintReports(ReportName, oCriteria);
            }
            else if (e.SelectedChoiceActionItem.Id == "KasbonPPrint02")
            {
                oCriteria = null;
                ReportName = oGen.ReportNameGet(objectSpace, "KasbonPPrint02");
                PrintReports(ReportName, oCriteria);
            }
            objectSpace.Refresh();

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
