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

namespace TriJasa.Module.BusinessObjects.TriJasa
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ShowListViewController : ViewController
    {
        public ShowListViewController()
        {
            InitializeComponent();
            TargetViewId = "SMKomisi_ListSMKomisiItem_ListView_Entry";
            // Target required Views (via the TargetXXX properties) and create their Actions.
            PopupWindowShowAction showListViewAction = new PopupWindowShowAction(this, "SM",
            PredefinedCategory.View);
            showListViewAction.CustomizePopupWindowParams += ShowListViewAction_CustomizePopupWindowParams;
            showListViewAction.Execute += ShowListViewAction_Execute;
            
        }

        private void ShowListViewAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            // throw new NotImplementedException();
            if (Application.MainWindow.View != null && Application.MainWindow.View.Id == "SMKomisi_DetailView_Entry")
            {
                SMKomisi oSMKomisi = ((SMKomisi)Application.MainWindow.View.CurrentObject);
                
                IObjectSpace KomisiObjectSpace = Application.CreateObjectSpace(typeof(SMKomisi));
                if (e.PopupWindowViewSelectedObjects.Count > 0)
                {
                    foreach (SuratMuatan selectedObject in e.PopupWindowViewSelectedObjects)
                    {

                        List<SMKomisiItem> oKomisiItem = oSMKomisi.ListSMKomisiItem.ToList();

                        int inList = oSMKomisi.ListSMKomisiItem.Where(x => x.SM.Oid== selectedObject.Oid).Count();


                        if (inList == 0)
                        {
                            SMKomisiItem oKMItem = ObjectSpace.CreateObject<SMKomisiItem>();
                            string sqlQuery = string.Format($" Oid == {selectedObject.Oid} ");
                            CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);
                            SuratMuatan oSuratMuatan = ObjectSpace.FindObject<SuratMuatan>(filterOperator);
                            oKMItem.SM = oSuratMuatan;
                            oKMItem.Dibayarkan = (int)selectedObject.KomisiSisa;
                            oKMItem.Save();
                            oSMKomisi.ListSMKomisiItem.Add(oKMItem);
                            oSMKomisi.Save();
                        }
                    }
                }
            }
        }

        private void ShowListViewAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            //e.View = Application.CreateListView(  objectType, true);
            Type objectType = typeof(SuratMuatan);
            IObjectSpace newObjectSpace = Application.CreateObjectSpace(objectType);
            string listViewId = "SuratMuatan_LookupListView_Komisi";// Application.FindLookupListViewId(objectType);
            CollectionSourceBase collectionSource = Application.CreateCollectionSource(
                newObjectSpace, objectType, listViewId);
            string sqlQuery = "";
            sqlQuery = string.Format($" [Status] In (3,5)  || ( Status == 6 && Sisa!=0 )");
            CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);

            collectionSource.Criteria["Filter1"] = CriteriaOperator.Parse("KomisiSisa !=0");
            e.View = Application.CreateListView(listViewId, collectionSource, true);
            e.View.Model.AllowNew = false;
            e.View.Model.AllowEdit = false;
            e.View.Model.AllowDelete = false;
            
            e.View.IsRoot = true;
            //  SuratMuatan currentObject = View.CurrentObject as SuratMuatan;

            //ListView oList = Application.CreateListView(listViewId, collectionSource, true);
            //Application.ShowViewStrategy.ShowViewInPopupWindow(confirmationDetailView, OkDelegate, CancelDelegate);
            // Application.ShowViewStrategy.ShowViewInPopupWindow(oList, OkDelegate);

        }
        public void OkDelegate()
        {
        }
        public void CancelDelegate()
        {
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
