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
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TriJasa.Module.BusinessObjects;

namespace TriJasa.Module.Web.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class SMvcLaut : ViewController
    {
        public SMvcLaut()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetObjectType = typeof(SuratMuatan);
            TargetViewId = "SuratMuatan_DetailView_Laut";
            TargetViewType = ViewType.DetailView;

            SimpleAction SMVLDEPrintAction = new SimpleAction(this, "SMVLDEPrintAction", PredefinedCategory.Unspecified)
            {
                Caption = "Print",
                //ConfirmationMessage = "Are you sure you want to clear the Tasks list?",
                ImageName = "Action_Printing_Print"
            };
            SMVLDEPrintAction.Category = "Cetho";
            SMVLDEPrintAction.TargetViewType = ViewType.DetailView;
            SMVLDEPrintAction.Execute += SMVLDEPrintAction_Execute;
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
        private void SMVLDEPrintAction_Execute(Object sender, SimpleActionExecuteEventArgs e)
        {
            //while (((SuratMuatan)View.CurrentObject).Tasks.Count > 0)
            //{
            //    ((Contact)View.CurrentObject).Tasks.Remove(((Contact)View.CurrentObject).Tasks[0]);
            //}
            // ObjectSpace.SetModified(View.CurrentObject);

            var controller = Frame.GetController<ModificationsController>();
            if (controller != null)
            {
                controller.SaveAction.DoExecute();
            }

            ObjectSpace.CommitChanges();

            SuratMuatan currentObject = View.CurrentObject as SuratMuatan;
            if (currentObject != null)
            {
                currentObject.Save();
                currentObject.Session.CommitTransaction();
            }
            currentObject.Save();
            currentObject.Session.CommitTransaction();
            string ReportName = "";
            iGenTriJasa oGen = new iGenTriJasa();
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(SuratMuatan));
            ArrayList aListDM = new ArrayList();
            CriteriaOperator oCriteria = null;
            //((SuratMuatan)View.CurrentObject).NomorDM.Save();
            SuratMuatan oSM = ((SuratMuatan)View.CurrentObject);
            oSM.UpdateJenisBarang(true);

            oSM.Save();
            oSM.Session.CommitTransaction();
            objectSpace.CommitChanges();
            View.ObjectSpace.CommitChanges();


            string sqlQuery = string.Format("SuratMuat.Oid = {0}  ", ((SuratMuatan)View.CurrentObject).Oid);
            CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);

            ////oCriteria = GroupOperator.And(oCriteria, new InOperator("NomorDM.Oid", aListDM));
            ReportName = oGen.ReportNameGet(objectSpace, "DMPPrintSM");
            PrintReports(ReportName, filterOperator);
            //this.ObjectSpace.Refresh();
        }

        private void DeleteSMList()
        {

        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            ObjectSpace.ModifiedChanged += ObjectSpace_ModifiedChanged;
            UpdateActionState();
            DialogController dialogController = Frame.GetController<DialogController>();
            if (dialogController != null)
            {
                dialogController.AcceptAction.Execute += AcceptAction_Execute;
                dialogController.CancelAction.Execute += CancelAction_Execute;
                
                dialogController.AcceptAction.ActionMeaning = DevExpress.ExpressApp.Actions.ActionMeaning.Unknown;
            }
            if (Frame is DevExpress.ExpressApp.Web.PopupWindow && View.Id == "DaftarMuatanLaut_DetailView_Entry")
            {
                SuratMuatan currentObject = (SuratMuatan)View.CurrentObject;
                if (currentObject.NomorDM != null)
                {
                    parentObjectId = View.ObjectSpace.GetKeyValue(currentObject.NomorDM);
                }
            }
     
        }
        private void CancelAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (Application.MainWindow.View != null && Application.MainWindow.View.Id == "DaftarMuatanLaut_DetailView_Entry")
            {
                DetailView parentDetailView = (DetailView)Application.MainWindow.View;

                DaftarMuatanLaut oDm = ((DaftarMuatanLaut)Application.MainWindow.View.CurrentObject);
               // oDm.Save();
                //parentDetailView.ObjectSpace.CommitChanges();
                if (e.CurrentObject is SuratMuatan)
                {
                    SuratMuatan oSM = ((SuratMuatan)e.CurrentObject);
                    if (oSM.Oid < 0 || oSM.Pelanggan==null)
                    {
                        //parentDetailView.ObjectSpace.Delete(oSM);
                        oDm.SuratMuatan.Remove(oSM);
                        oDm.Save();
                        oDm.Session.CommitTransaction();



                    }
                }
                parentDetailView.ObjectSpace.CommitChanges();
                parentDetailView.ObjectSpace.Refresh();
                parentDetailView.Refresh();
                //parentDetailView.ObjectSpace.ReloadObject();
                //Application.MainWindow.View.RefreshDataSource();



            }
            //View.ObjectSpace.Dispose();
            //throw new NotImplementedException();
            //DeleteSM();
            //View.ObjectSpace.Rollback();
        }

        private void DeleteSM()
        {
            if (Application.MainWindow.View != null && Application.MainWindow.View.Id == "DaftarMuatanLaut_DetailView_Entry")
            {
                // Application.MainWindow.View.ObjectSpace.CommitChanges();
                // Application.MainWindow.View.ObjectSpace.Refresh();
                //Application.MainWindow.View.Refresh();
                DetailView parentDetailView = (DetailView)Application.MainWindow.View;
                //      ((DetailView)Application.MainWindow.View.ObjectSpace.Owner).FindItem("SuratMuatan").Refresh();
                ListPropertyEditor peListSuratMuatan = parentDetailView.FindItem("SuratMuatan") as ListPropertyEditor;



                XPCollection<SuratMuatan> oListSuratMuatan = (XPCollection<SuratMuatan>)peListSuratMuatan.PropertyValue;
               

                List<SuratMuatan> lsSuratMuatan = oListSuratMuatan.Where(a => a.Pelanggan==null).ToList();
                DaftarMuatanLaut oDM = ((DaftarMuatanLaut)Application.MainWindow.View.CurrentObject);
                oDM.SMGenerate();
       
                peListSuratMuatan.Refresh();
                parentDetailView.ObjectSpace.CommitChanges();
                 parentDetailView.ObjectSpace.Refresh();
                parentDetailView.Refresh();


            }
        }
        private void SaveDM()
        {

     

            if (Application.MainWindow.View != null && Application.MainWindow.View.Id == "DaftarMuatanLaut_DetailView_Entry")
            {
                // Application.MainWindow.View.ObjectSpace.CommitChanges();
                // Application.MainWindow.View.ObjectSpace.Refresh();
                //Application.MainWindow.View.Refresh();
                DetailView parentDetailView = (DetailView)Application.MainWindow.View;
                //      ((DetailView)Application.MainWindow.View.ObjectSpace.Owner).FindItem("SuratMuatan").Refresh();
                ListPropertyEditor peListSuratMuatan = parentDetailView.FindItem("SuratMuatan") as ListPropertyEditor;
                XPCollection<SuratMuatan> oListSuratMuatan = (XPCollection<SuratMuatan>)peListSuratMuatan.PropertyValue;

                parentDetailView.ObjectSpace.CommitChanges();
                parentDetailView.ObjectSpace.Refresh();
                parentDetailView.Refresh();


            }
        }
        private void AcceptAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

            oParentDetailView = (DetailView)Application.MainWindow.View;

            SaveDM();
            //View.Close();
            //owNewView();
            //  this.ObjectSpace.CommitChanges();
            //  throw new NotImplementedException();
        }

        private DetailView oParentDetailView;
        void ShowNewView()
        {

          
         

            ListPropertyEditor lpe = oParentDetailView.FindItem("SuratMuatan") as ListPropertyEditor;

            //  View.Close(false);

            NewObjectViewController targetNewController = lpe.Frame.GetController<NewObjectViewController>();
            if (targetNewController != null)
            {
                ChoiceActionItem targetNewItem = targetNewController.NewObjectAction.Items[0];
                targetNewController.NewObjectAction.DoExecute(targetNewItem);
            }
            //var svp = new ShowViewParameters();
            //var app = Application;
            //svp.CreatedView = app.CreateDetailView(ObjectSpaceInMemory.CreateNew(), new MessageBoxTextMessage("Request Id: XXX"));
            //svp.CreatedView.Caption = "Request Submitted";
            //svp.TargetWindow = TargetWindow.NewModalWindow;
            //svp.Context = TemplateContext.PopupWindow;
            //svp.CreateAllControllers = true;
            //var dc = app.CreateController<DialogController>();
            //svp.Controllers.Add(dc);
            //app.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
        }
        void ObjectSpace_ModifiedChanged(object sender, EventArgs e)
        {
            UpdateActionState();
        }
        protected virtual void UpdateActionState()
        {
            //AddAndNew.Enabled["ObjectSpaceIsModified"] = !ObjectSpace.IsModified;
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
            ObjectSpace.ModifiedChanged -= ObjectSpace_ModifiedChanged;
        }
        object parentObjectId = null;
        private void AddAndNew_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            Validator.RuleSet.ValidateAll(View.ObjectSpace, View.ObjectSpace.GetObjectsToSave(false), ContextIdentifier.Save);


            if (View.ObjectSpace.IsModified)
            {
                var controller1 = Frame.GetController<ModificationsController>();
                if (controller1 != null)
                {
                    controller1.SaveAction.DoExecute();
                }
                View.ObjectSpace.CommitChanges();
            }
            View.Close(false);

            DetailView parentDetailView = (DetailView)Application.MainWindow.View;
            ListPropertyEditor lpe = parentDetailView.FindItem("SuratMuatan") as ListPropertyEditor;
            NewObjectViewController targetNewController = lpe.Frame.GetController<NewObjectViewController>();
            if (targetNewController != null)
            {
                ChoiceActionItem targetNewItem = targetNewController.NewObjectAction.Items[0];
                targetNewController.NewObjectAction.DoExecute(targetNewItem);
            }

           
            /*

            //View.Close(false);
            bool isSave = false;
            if (Application.MainWindow.View != null && Application.MainWindow.View.Id == "DaftarMuatan_DetailView_Entry")
            {

                DaftarMuatan oDm = ((DaftarMuatan)Application.MainWindow.View.CurrentObject);
                //parentDetailView.ObjectSpace.CommitChanges();
                if (View.CurrentObject is SuratMuatan)
                {
                    SuratMuatan oSM = ((SuratMuatan)View.CurrentObject);
                    if (oSM.Oid < 0 && oSM.Pelanggan != null && oSM.Dikerimke!= null)
                    {
                        //parentDetailView.ObjectSpace.Delete(oSM);
                        oSM.Save();
                        oDm.SuratMuatan.Add(oSM);
                        oDm.Save();
                        oDm.Session.CommitTransaction();
                        Application.MainWindow.View.ObjectSpace.CommitChanges();
                        Application.MainWindow.View.ObjectSpace.Refresh();
                        Application.MainWindow.View.Refresh();
                        isSave = true;
                        var controller1 = Frame.GetController<ModificationsController>();
                        if (controller1 != null)
                        {
                            controller1.SaveAction.DoExecute();
                        }
                         View.ObjectSpace.CommitChanges();

                    }
                }

               //pplication.MainWindow.View.ObjectSpace.CommitChanges();
               //pplication.MainWindow.View.ObjectSpace.Refresh();
               //pplication.MainWindow.View.Refresh();
               //(DetailView)Application.MainWindow.View.ObjectSpace.Owner).FindItem("SuratMuatan").Refresh();

                DetailView parentDetailView = (DetailView)Application.MainWindow.View;
                parentDetailView.ObjectSpace.CommitChanges();
                parentDetailView.ObjectSpace.Refresh();
                
                parentDetailView.Refresh();

                ListPropertyEditor lpe = parentDetailView.FindItem("SuratMuatan") as ListPropertyEditor;
               // RefreshController refrh =lpe.Frame.GetController<RefreshController>();
               // if (refrh != null)
               // {
                    //ChoiceActionItem targetNewItem = refrh.RefreshAction.
                    //targetNewController.NewObjectAction.DoExecute(targetNewItem);
               // }

              
              //  View.Close(false);
               
                NewObjectViewController targetNewController = lpe.Frame.GetController<NewObjectViewController>();
                if (targetNewController != null)
                {
                    ChoiceActionItem targetNewItem = targetNewController.NewObjectAction.Items[0];
                    targetNewController.NewObjectAction.DoExecute(targetNewItem);
                }
            }
            */
            /*
            DetailView parentDetailView = (DetailView)Application.MainWindow.View;
            FrameRoudMapController frameRoudMap = Application.MainWindow.GetController<FrameRoudMapController>();
            Frame parentFrame = frameRoudMap.ParentFrame;
            if (parentFrame != null)
            {
                NewObjectViewController targetNewController = parentFrame.GetController<NewObjectViewController>();
                ChoiceActionItem targetNewItem = targetNewController.NewObjectAction.Items[0];
                targetNewController.NewObjectAction.DoExecute(targetNewItem);
            }
            */
            /*
            SuratMuatan currentObject = (SuratMuatan)View.CurrentObject;
            currentObject.NomorDM = View.ObjectSpace.GetObjectByKey<DaftarMuatan>(parentObjectId);
            View.ObjectSpace.CommitChanges();

            INestedObjectSpace nestedObjectSpace = View.ObjectSpace as INestedObjectSpace;
            if (nestedObjectSpace != null)
            {
                IObjectSpace parentObjectSpace = nestedObjectSpace.ParentObjectSpace;
                parentObjectSpace.CommitChanges();
            }
            View.ObjectSpace.Refresh();
            if (Application.MainWindow.View != null && Application.MainWindow.View.Id == "DaftarMuatan_DetailView_Entry")
            {
                Application.MainWindow.View.ObjectSpace.CommitChanges();
                Application.MainWindow.View.ObjectSpace.Refresh();
            }
            ViewShortcut shortcut = View.CreateShortcut();
            View newView = Application.ProcessShortcut(shortcut);
            newView.CurrentObject = newView.ObjectSpace.CreateObject(newView.ObjectTypeInfo.Type);
            e.ShowViewParameters.CreatedView = newView;
            */
        }
    }
}
