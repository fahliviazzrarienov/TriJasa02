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
using System.Linq;
using System.Text;
//using DevExpress.ExpressApp.SystemModule;
// new files
using DevExpress.XtraPrinting;

namespace TriJasa.Module.BusinessObjects
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class vcImportFile : ViewController
    {
        //private ExportController exportController;
        public vcImportFile()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetViewType = ViewType.DashboardView;
            
            // 
            PopupWindowShowAction ImportFileAction = new PopupWindowShowAction(this, "ImportFileAction", PredefinedCategory.Edit)
            {
                Caption = "Import File"
            };

            ImportFileAction.CustomizePopupWindowParams += ImportFileAction_CustomizePopupWindowParams;
            ImportFileAction.Execute += ImportFileAction_Execute;

            //exportController = Frame.GetController<ExportController>();
            //exportController.CustomExport += CustomExport;
        }

        private void ImportFileAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
           // throw new NotImplementedException();
        }

        private void ImportFileAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {

            var nonPersistentOS = Application.CreateObjectSpace(typeof(ImportFiles));
            ImportFiles oImportFile = nonPersistentOS.CreateObject<ImportFiles>();

            ListView lv = (ListView)View;
            oImportFile.DataType = "";

            PropertyCollectionSource pcs = lv.CollectionSource as PropertyCollectionSource;
            string oid;
            string MasterOid = "";
            string MasterPropertyName = "";
            object objMaster = null;
            if (pcs != null)
            {
                Type otype = pcs.MasterObject.GetType();
                objMaster = pcs.MasterObject;
                string MasterId = pcs.MasterObject.ToString();
                MasterPropertyName = pcs.MemberInfo.AssociatedMemberInfo.Name;
                int start = MasterId.IndexOf("(") + 1;
                int end = MasterId.IndexOf(")");
                MasterOid = MasterId.Substring(start, (end - start));
            }

            // adding colom object 
            //oImportFile.UpdateTempate();
            iGetKey oGetKey = new iGetKey();

            List<Type> B = oGetKey.GetClassList();
            oImportFile.UpdateTempate(lv.ObjectTypeInfo.Type);
            //nonPersistentOS.CommitChanges();
            DetailView detailView = Application.CreateDetailView(nonPersistentOS, oImportFile);
            detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
             e.View = detailView;
            
            //e.ShowViewParameters.CreatedView = dv;
            //e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            //e.ShowViewParameters.Context = TemplateContext.PopupWindow;
            //e.ShowViewParameters.CreateAllControllers = true;
            //You can pass custom Controllers for intercommunication or to provide a standard functionality).
            //DialogController dc = Application.CreateController<DialogController>();
            //e.ShowViewParameters.Controllers.Add(dc);
            //SimpleAction clearTasksAction;
            

            //e.DialogController.SaveOnAccept = false;
            //e.DialogController.CancelAction.Active["NothingToCancel"] = false;

        }

        private void NonPersite()
        {
            ////throw new NotImplementedException();
            //var nonPersistentOS = Application.CreateObjectSpace(typeof(ImportFile));
            //ImportFile oImportFile = nonPersistentOS.CreateObject<ImportFile>();

            //ListView lv = (ListView)View;
            //oImportFile.DataType = "";

            //PropertyCollectionSource pcs = lv.CollectionSource as PropertyCollectionSource;
            //string oid;
            //string MasterOid = "";
            //string MasterPropertyName = "";
            //object objMaster = null;
            //if (pcs != null)
            //{
            //    Type otype = pcs.MasterObject.GetType();
            //    objMaster = pcs.MasterObject;
            //    string MasterId = pcs.MasterObject.ToString();
            //    MasterPropertyName = pcs.MemberInfo.AssociatedMemberInfo.Name;
            //    int start = MasterId.IndexOf("(") + 1;
            //    int end = MasterId.IndexOf(")");
            //    MasterOid = MasterId.Substring(start, (end - start));
            //}

            //// adding colom object 
            //oImportFile.UpdateTempate();
            //oImportFile.UpdateTempate(lv.ObjectTypeInfo.Type);
            ////nonPersistentOS.CommitChanges();
            //DetailView detailView = Application.CreateDetailView(nonPersistentOS, oImportFile);
            //detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;



            //e.View = detailView;
            //e.DialogController.SaveOnAccept = false;
            //e.DialogController.CancelAction.Active["NothingToCancel"] = false;

        }
        protected virtual void CustomExport(object sender, CustomExportEventArgs e)
        {
            //Customize Export Options
            if (e.ExportTarget == ExportTarget.Xls)
            {
                XlsExportOptions options = e.ExportOptions as XlsExportOptions;
                if (options == null)
                {
                    options = new XlsExportOptions();
                }
                options.SheetName = View.Caption;
                options.ShowGridLines = true;
                e.ExportOptions = options;
            }
        }
        
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            //exportController = Frame.GetController<ExportController>();
            //exportController.CustomExport += CustomExport;

        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            //exportController.CustomExport -= new EventHandler<CustomExportEventArgs>(CustomExport);
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
