using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TriJasa.Module.BusinessObjects;

namespace TriJasa.Module.Web.Trijasa
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class NotaPaymentPelunasanVC : ViewController<ListView>
    {
        public NotaPaymentPelunasanVC()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetViewType = ViewType.ListView;
            TargetViewNesting = Nesting.Nested;
            TargetObjectType = typeof(Nota);
            //TargetViewId = "NotaPayment_DetailView_Pelunasan";
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;

            try
            {
                if (View.AllowEdit)
                {
                    Frame.GetController<NewObjectViewController>().ObjectCreating += new EventHandler<ObjectCreatingEventArgs>(ViewController1_ObjectCreating);
                }

                foreach (PropertyEditor editor in ((ListView)View).GetItems<PropertyEditor>())
                {
                    editor.ControlCreated += Editor_ControlCreated;
                }
            }
            catch (Exception)
            { }
        }

        private void ViewController1_ObjectCreating(object sender, ObjectCreatingEventArgs e)
        {
            try
            {
                ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;

                if (editor != null)
                {

                    ASPxGridView gridControl = editor.Grid;


                }
            }
            catch (Exception)
            { }
        }

        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            //throw new NotImplementedException();
            try
            {
                if (View != null)
                {
                    if (View.CurrentObject == e.Object)
                    {
                        //if (txtNomorSM == null || txtNomorSM.PropertyValue.ToString() == "")
                        //if (((NotaPayment)View.CurrentObject). == "")
                        //{

                        //}
                    }
                }
                if (e.Object is Nota)
                {
                    this.ObjectSpace.CommitChanges();
                }
            }
            catch ( Exception)
            { }
        }

        private void Editor_ControlCreated(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (sender is ASPxLookupPropertyEditor)
            {

            }
        }
        private void GridControl_RowUpdated(object sender, DevExpress.Web.Data.ASPxDataUpdatedEventArgs e)
        {

        }
        private void GridControl_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            //   throw new NotImplementedException();
        }

        private void GridControl_BeforeGetCallbackResult(object sender, EventArgs e)
        {

        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
            try
            {
                ASPxGridListEditor listEditor = ((ListView)View).Editor as ASPxGridListEditor;
                //ASPxGridView GridView = ((ASPxGridView)View).Editor as ASPxGridView;
                //listEditor.Grid 

                if (listEditor != null)
                {
                    ASPxGridView gridControl = listEditor.Grid;
                    //listEditor.Grid.

                    gridControl.AfterPerformCallback += GridControl_AfterPerformCallback;
                   
                }
            }
            catch (Exception)
            { }
        }

        private void GridControl_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            // throw new NotImplementedException();
            try
            {
                if (e.CallbackName == "UPDATEEDIT")
                {
                    this.ObjectSpace.CommitChanges();
                    //ASPxGridView grid = sender as ASPxGridView;
                    //try
                    //{
                    //    grid.AddNewRow();
                    //}
                    //catch (Exception)
                    //{ }
                }
            }
            catch (Exception)
            { }
        }
        private void Grid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            try
            {
                if (e.Editor is ASPxComboBox)
                {
                    (e.Editor as ASPxComboBox).ClientSideEvents.KeyDown = "onKeyPress";
                }
            }
            catch (Exception)
            { }
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
