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
using TriJasa.Module.BusinessObjects;


namespace TriJasa.Module.Web.Trijasa
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class KomisiVC : ViewController<DetailView>
    {
        public KomisiVC()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetViewType = ViewType.DetailView;
            TargetObjectType = typeof(SMKomisi);
            TargetViewId = "SMKomisi_DetailView_Entry";
            //PopupWindowShowAction showListViewAction = new PopupWindowShowAction(this, "SMKomisi_ListSM_ListView_Entry",
            //PredefinedCategory.Edit);
            //showListViewAction.CustomizePopupWindowParams += ShowListViewAction_CustomizePopupWindowParams;
        }
        private void ShowListViewAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            //SuratMuatan objectType = typeof();
            //e.View = Application.CreateListView(objectType, true);
        }
        protected override void OnActivated()
        {
            base.OnActivated();

            if (View.ViewEditMode == ViewEditMode.View)
            {
               // View.ViewEditMode = ViewEditMode.Edit;
               // ObjectSpace.SetModified(null);
            }
            else if (View.ViewEditMode == ViewEditMode.Edit)
            {

            }
            // Perform various tasks depending on the target View.
            //if (View.v. ViewEditMode == ViewEditMode.Edit)
            //{

                //}
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
