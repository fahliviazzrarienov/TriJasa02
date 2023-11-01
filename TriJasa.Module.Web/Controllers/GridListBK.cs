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
namespace TriJasa.Module.Web.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class GridListBK : ViewController
    {
        public GridListBK()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetViewType = ViewType.ListView;
            TargetViewNesting = Nesting.Nested;
            //TargetObjectType = typeof(fTransKasDtl);
            TargetViewId = "NotaPayment_ListNota_ListView_Entry";
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
        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            //  throw new NotImplementedException();
        }

        private void Editor_ControlCreated(object sender, EventArgs e)
        {
            if (sender is ASPxLookupPropertyEditor)
            {

            }
        }

        void ViewController1_ObjectCreating(object sender, ObjectCreatingEventArgs e)
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

        private void GridControl_RowUpdated(object sender, DevExpress.Web.Data.ASPxDataUpdatedEventArgs e)
        {

        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
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
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridControl, "BeginCallback", "function(s,e){s.command=e.command;}", "myScript1");
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridControl, "EndCallback", "function(s,e){debugger;if(s.command=='UPDATEEDIT'|| s.command=='ADDNEWROW') firstColumn.Focus();}", "myScript1");
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridControl, "OnBatchEditStartEditing", @" if (e.visibleIndex < 0) {
                                                                       //set row/line number
                                                                        s.batchEditApi.SetCellValue(e.visibleIndex, 'LINENUMBER', gridControl.GetVisibleRowsOnPage());
                                                                    }
                                                                    //Set focus to second column since first column is not editable
                                                                    if (e.focusedColumn.fieldName == 'LINENUMBER') {
                                                                        e.cancel = true;
                                                                        //setTimeout is necessary for some reason as without it focus doesn't happen (I will inestigate as to why later)
                                                                        window.setTimeout(function () { gridControl.batchEditApi.StartEdit(e.visibleIndex, 2); }, 0);
                                                                    }", "myScript1");


                    foreach (GridViewColumn column in listEditor.Grid.Columns)
                    {
                        if (column is GridViewCommandColumn)
                        {
                            GridViewCommandColumn commandColumn = (GridViewCommandColumn)column;
                            if (!commandColumn.ShowSelectCheckbox)
                            {
                                commandColumn.VisibleIndex = 1000;
                            }
                            //if (commandColumn.Name)
                        }
                        //column.Width
                        //column.Name

                    }


                    //listEditor.Grid.Columns["JenisBarang"].Width = 100;
                    //listEditor.Grid.Columns["Kuantum"].Width = 10;
                    //listEditor.Grid.Columns["Unit"].Width = 10;
                    //listEditor.Grid.Columns["Kg"].Width = 10;
                    //listEditor.Grid.Columns["VolM3"].Width = 10;
                    //listEditor.Grid.Columns["Ongkos"].Width = 10;
                    //listEditor.Grid.Columns["Jumlah"].Width = 10;
                    //listEditor.Grid.Columns["OngkosOriginal"].Width = 10;
                    //listEditor.Grid.Columns["JumlahOriginal"].Width = 10;

                    //listEditor.Grid.AddNewRow();
                }
            }
            catch (Exception)
            { }
        }
        private void GridControl_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            //   throw new NotImplementedException();
        }

        private void GridControl_BeforeGetCallbackResult(object sender, EventArgs e)
        {

        }

        private void GridControl_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            try
            {
                if (e.CallbackName == "UPDATEEDIT")
                {
                    //this.ObjectSpace.CommitChanges();
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
