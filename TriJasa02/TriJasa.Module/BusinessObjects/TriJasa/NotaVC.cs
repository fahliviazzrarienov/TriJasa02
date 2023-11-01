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
    public partial class NotaVC : ViewController
    {
       
        public NotaVC()
        {
            InitializeComponent();
            TargetViewType = ViewType.ListView;
            //TargetViewId = "BankTran_ListView_Pelunasan";
            TargetObjectType = typeof(Nota);
            SingleChoiceAction NotaPrintAction = new SingleChoiceAction(this, "NotaPrint", PredefinedCategory.Unspecified);
            NotaPrintAction.Caption = "Print";
            NotaPrintAction.ImageName = "Action_Printing_Print";
            NotaPrintAction.ItemType = SingleChoiceActionItemType.ItemIsOperation;
            ChoiceActionItem NotaPPrint01 = new ChoiceActionItem("NotaPPrint01", "Print Nota Biasa", null);
            ChoiceActionItem NotaPPrint02 = new ChoiceActionItem("NotaPPrint02", "Print Nota Alamat", null);
            ChoiceActionItem NotaPPrint03 = new ChoiceActionItem("NotaPPrint03", "Print Nota PPN ", null);
            ChoiceActionItem NotaPPrint04 = new ChoiceActionItem("NotaPPrint04", "Rekap Status Nota ", null);

            NotaPrintAction.TargetViewType = ViewType.ListView;
            NotaPrintAction.TargetObjectType = typeof(Nota);
            NotaPrintAction.Items.Add(NotaPPrint01);
            NotaPrintAction.Items.Add(NotaPPrint02);
            NotaPrintAction.Items.Add(NotaPPrint03);
            NotaPrintAction.Items.Add(NotaPPrint04);
            NotaPrintAction.Execute += NotaPrintAction_Execute;


            // Target required Views (via the TargetXXX properties) and create their Actions.
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
        private void NotaPrintAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            //if (View.CurrentObject != null)
            //{
            string ReportName = "";
            iGenTriJasa oGen = new iGenTriJasa();
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(Nota));
            ArrayList aListDM = new ArrayList();
            CriteriaOperator oCriteria = null;
            if ((e.SelectedObjects.Count > 0))
            {
                foreach (Nota selectedObject in e.SelectedObjects)
                {
                    aListDM.Add(selectedObject.Oid);
                    selectedObject.Printed();
                    selectedObject.Save();
                    selectedObject.Session.CommitTransaction();
                }
                if (e.SelectedChoiceActionItem.Id == "NotaPPrint01")
                {

                    oCriteria = GroupOperator.And(oCriteria, new InOperator("NotaSuratMuat.Oid", aListDM));
                    ReportName = oGen.ReportNameGet(objectSpace, "NotaPPrint01");
                }
                else if (e.SelectedChoiceActionItem.Id == "NotaPPrint02")
                {
                    //SuratMuatan o;
                    //o.NotaSuratMuat
                    oCriteria = GroupOperator.And(oCriteria, new InOperator("NotaSuratMuat.Oid", aListDM));
                    ReportName = oGen.ReportNameGet(objectSpace, "NotaPPrint02");
                }
                else if (e.SelectedChoiceActionItem.Id == "NotaPPrint03")
                {
                    //SuratMuatDtl o;
                    //o.SuratMuat.NomorDM
                    oCriteria = GroupOperator.And(oCriteria, new InOperator("NotaSuratMuat.Oid", aListDM));
                    ReportName = oGen.ReportNameGet(objectSpace, "NotaPPrint03");
                }
                PrintReports(ReportName, oCriteria);
            }
            if (e.SelectedChoiceActionItem.Id == "NotaPPrint04")
            {

                oCriteria = null;
                ReportName = oGen.ReportNameGet(objectSpace, "NotaPPrint04");
                PrintReports(ReportName, oCriteria);
            }
            objectSpace.Refresh();

        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;

            var linkController = Frame.GetController<LinkUnlinkController>();
            if (linkController != null)
            {
                linkController.UnlinkAction.Executed += UnlinkAction_Executed;
                linkController.LinkAction.Executed += LinkAction_Executed;
            }
        }
        private void LinkAction_Executed(object sender, ActionBaseEventArgs e)
        {
            //throw new NotImplementedException();
            //var linkedItems = ((SimpleActionExecuteEventArgs)e).SelectedObjects;
            //foreach (var selectedObject in linkedItems)
            //{
            //    //aListDM.Add(selectedObject.Oid);
            //    Application.ShowViewStrategy.ShowMessage($" link {selectedObject} ");
            //}
           
        }

        private void UnlinkAction_Executed(object sender, ActionBaseEventArgs e)
        {
            //var unlinkedItems = ((SimpleActionExecuteEventArgs)e).SelectedObjects;
            //Application.ShowViewStrategy.ShowMessage($" unlink {unlinkedItems.Count} ");
            //foreach (var selectedObject in unlinkedItems)
            //{
            //    //aListDM.Add(selectedObject.Oid);
            //    Application.ShowViewStrategy.ShowMessage($" link {selectedObject} ");
            //}

        }
        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            try
            {
                if (View != null)
                {
                    if (View.CurrentObject == e.Object)
                    {
                        if (this.ObjectSpace.IsModified)
                        {
                            this.ObjectSpace.CommitChanges();
                        }
                    }
                }
            }
            catch( Exception)
            { }
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            var linkController = Frame.GetController<LinkUnlinkController>();
            if (linkController != null)
            {
                linkController.UnlinkAction.Executed -= UnlinkAction_Executed;
                linkController.LinkAction.Executed -= LinkAction_Executed;
            }
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
