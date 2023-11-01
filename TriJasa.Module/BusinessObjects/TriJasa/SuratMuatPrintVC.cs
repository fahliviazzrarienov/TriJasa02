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
    public partial class SuratMuatPrintVC : ViewController
    {
        public SuratMuatPrintVC()
        {
            InitializeComponent();

            TargetObjectType = typeof(SuratMuatan);
            TargetViewType = ViewType.ListView;
            // Target required Views (via the TargetXXX properties) and create their Actions.
            SimpleAction SMEPrintAction = new SimpleAction(this, "SMELPrint", PredefinedCategory.Unspecified)
            {
                Caption = "Print",
                //ConfirmationMessage = "Are you sure you want to clear the Tasks list?",
                ImageName = "Action_Printing_Print"
            };
            SMEPrintAction.Execute += SMEPrintAction_Execute;

    

        }
        private void UnlinkAction_Executed(object sender, ActionBaseEventArgs e)
        {
            if (View != null)
            {
                if (View.Id == "SMKomisi_ListSM_ListView_Entry")
                {
                    var unlinkedItems = ((SimpleActionExecuteEventArgs)e).SelectedObjects;
                    //Application.ShowViewStrategy.ShowMessage($" unlink {unlinkedItems.Count} ");
                    foreach (SuratMuatan selectedObject in unlinkedItems)
                    {
                        selectedObject.Komisi = 0;
                        //    //aListDM.Add(selectedObject.Oid);
                        //    Application.ShowViewStrategy.ShowMessage($" link {selectedObject} ");
                    }
                }
            }
            //Nota oNota = 
        }
        private void LinkAction_Executed(object sender, ActionBaseEventArgs e)
        {
            //throw new NotImplementedException();
            if (View != null)
            {
                if (View.Id == "SMKomisi_ListSM_ListView_Entry")
                {

                    var linkedItems = ((SimpleActionExecuteEventArgs)e).SelectedObjects;
                    //Application.ShowViewStrategy.ShowMessage($" unlink {linkedItems.Count} ");
                    foreach (SuratMuatan selectedObject in linkedItems)
                    {
                       // double aSisa = selectedObject.KomisiSisa;
                       // selectedObject.Komisi = aSisa;
                        //aListDM.Add(selectedObject.Oid);
                        // Application.ShowViewStrategy.ShowMessage($" link {selectedObject} ");
                    }
                }
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
        private void SMEPrintAction_Execute(Object sender, SimpleActionExecuteEventArgs e)
        {
            //while (((SuratMuatan)View.CurrentObject).Tasks.Count > 0)
            //{
            //    ((Contact)View.CurrentObject).Tasks.Remove(((Contact)View.CurrentObject).Tasks[0]);
            //}
            // ObjectSpace.SetModified(View.CurrentObject);

            

            string ReportName = "";
            iGenTriJasa oGen = new iGenTriJasa();
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(SuratMuatan));
            ArrayList aListDM = new ArrayList();
            CriteriaOperator oCriteria = null;

            //string sqlQuery = string.Format("SuratMuat.Oid = {0}  ", ((SuratMuatan)View.CurrentObject).Oid);
            //CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);

            ////oCriteria = GroupOperator.And(oCriteria, new InOperator("NomorDM.Oid", aListDM));
            // ReportName = oGen.ReportNameGet(objectSpace, "DMPPrintSM");
            //PrintReports(ReportName, filterOperator);

            if ((e.SelectedObjects.Count > 0))
            {
                foreach (SuratMuatan selectedObject in e.SelectedObjects)
                {
                    aListDM.Add(selectedObject.Oid);
                }

                oCriteria = GroupOperator.And(oCriteria, new InOperator("SuratMuat.Oid", aListDM));
                ReportName = oGen.ReportNameGet(objectSpace, "DMPPrintSM");

                PrintReports(ReportName, oCriteria);
            }

        }

        private NewObjectViewController Newcontroller;
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            NewObjectViewController Newcontroller = Frame.GetController<NewObjectViewController>();
            if (Newcontroller != null)
            {
                Newcontroller.NewObjectAction.Execute += NewObjectAction_Execute;
            }
            var linkController = Frame.GetController<LinkUnlinkController>();
            if (linkController != null)
            {
                linkController.UnlinkAction.Executed += UnlinkAction_Executed;
                linkController.LinkAction.Executed += LinkAction_Executed;
            }
        }

        private void NewObjectAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if (Application.MainWindow.View != null && Application.MainWindow.View.Id == "DaftarMuatan_DetailView_Entry")
            {
                //  throw new NotImplementedException();


                // Application.MainWindow.View.ObjectSpace.CommitChanges();
                // Application.MainWindow.View.ObjectSpace.Refresh();
                //Application.MainWindow.View.Refresh();
                DetailView parentDetailView = (DetailView)Application.MainWindow.View;
                //      ((DetailView)Application.MainWindow.View.ObjectSpace.Owner).FindItem("SuratMuatan").Refresh();
                ListPropertyEditor peListSuratMuatan = parentDetailView.FindItem("SuratMuatan") as ListPropertyEditor;
                XPCollection<SuratMuatan> oListSuratMuatan = (XPCollection<SuratMuatan>)peListSuratMuatan.PropertyValue;
                //if (oListSuratMuatan.Count > 0)
                //{
                //    int Total = 0;
                //    foreach (SuratMuatan oSM in oListSuratMuatan)
                //    {
                //        if (oSM.Pelanggan == null)
                //        {
                //            Total++;
                //          ///  oSM.Delete();

                //        }
                //        if (Total > 1)
                //        {
                //            oSM.Save();
                //            oSM.Session.CommitTransaction();
                //        }
                //    }

                }
                //parentDetailView.ObjectSpace.CommitChanges();
                //p//arentDetailView.ObjectSpace.Refresh();
                ///parentDetailView.Refresh();

            }
        

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
            //if (Application.MainWindow.View != null && Application.MainWindow.View.Id == "DaftarMuatan_DetailView_Entry")
            //{
            //    //  throw new NotImplementedException();
            //    //ASPxGridListEditor listEditor = ((ListView)View).Editor as ASPxGridListEditor;

            //    // Application.MainWindow.View.ObjectSpace.CommitChanges();
            //    // Application.MainWindow.View.ObjectSpace.Refresh();
            //    //Application.MainWindow.View.Refresh();
            //    DetailView parentDetailView = (DetailView)Application.MainWindow.View;

            //    if (parentDetailView.FindItem("NoTruck") != null && parentDetailView.FindItem("NamaSpr") !=null)
            //    {
            //        //      ((DetailView)Application.MainWindow.View.ObjectSpace.Owner).FindItem("SuratMuatan").Refresh();
            //        //ListPropertyEditor peListSuratMuatan = parentDetailView.FindItem("SuratMuatan") as ListPropertyEditor;
            //        //XPCollection<SuratMuatan> oListSuratMuatan = (XPCollection<SuratMuatan>)peListSuratMuatan.PropertyValue;
            //        //if (oListSuratMuatan.Count > 0)
            //        //{
            //        //    int Total = 0;
            //        //    foreach (SuratMuatan oSM in oListSuratMuatan)
            //        //    {
            //        //        if (oSM.Pelanggan == null)
            //        //        {
            //        //            Total++;
            //        //            ///  oSM.Delete();

            //        //        }
            //        //        if (Total > 1)
            //        //        {
            //        //            oSM.Save();
            //        //            oSM.Session.CommitTransaction();
            //        //        }
            //        //    }

            //        //}
            //        //this.ObjectSpace.CommitChanges();
            //        //this.ObjectSpace.Refresh();
            //        //parentDetailView.ObjectSpace.CommitChanges();
            //        //parentDetailView.ObjectSpace.Refresh();
            //        //parentDetailView.Refresh();
            //    }
            //}
        }
        protected override void OnDeactivated()
        {
            var linkController = Frame.GetController<LinkUnlinkController>();
            if (linkController != null)
            {
                linkController.UnlinkAction.Executed -= UnlinkAction_Executed;
                linkController.LinkAction.Executed -= LinkAction_Executed;
            }
            if (Newcontroller != null)
            {
                Newcontroller.NewObjectAction.Execute += NewObjectAction_Execute;
            }
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
