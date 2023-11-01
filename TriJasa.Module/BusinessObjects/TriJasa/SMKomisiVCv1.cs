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
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TriJasa.Module.BusinessObjects
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class SMKomisiVCv1 : ViewController
    {
        DialogController dlgController;
        public SMKomisiVCv1()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetObjectType = typeof(SMKomisi);
            //TargetViewNesting = Nesting.Nested;
            TargetViewType = ViewType.DetailView;
            TargetViewId = "SMKomisi_DetailView_Entry";
        }
        private bool isDeactivated;
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            isDeactivated = false;
            // Perform various tasks depending on the target View.
            ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
            ListPropertyEditor listPropertyEditor = ((DetailView)View).FindItem("ListSM") as ListPropertyEditor;


            if (listPropertyEditor != null)
            {
                if (listPropertyEditor.Control != null)
                {
                    ProcessListPropertyEditor(listPropertyEditor);
                }
                else
                {
                    listPropertyEditor.ControlCreated += listPropertyEditor_ControlCreated;
                }
            }
            var linkController = Frame.GetController<LinkUnlinkController>();
            if (linkController != null)
            {
                linkController.UnlinkAction.Executed += UnlinkAction_Executed;
                linkController.LinkAction.Executed += LinkAction_Executed;
            }
            dlgController = Frame.GetController<DialogController>();
            if (dlgController != null)
            {
                dlgController.AcceptAction.Execute += AcceptAction_Execute;
                dlgController.CancelAction.Execute += CancelAction_Execute;

            }
        }

        private void CancelAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            //throw new NotImplementedException();
            if (dlgController != null)
            {
                dlgController.CanCloseWindow = true;
            }
            else
            {
                //    ListPropertyEditor peListnote = ((DetailView)View).FindItem("ListNota") as ListPropertyEditor;
                //    XPCollection<Nota> oListNota = (XPCollection<Nota>)peListnote.PropertyValue;
                //    foreach (Nota iNota in oListNota)
                //    {
                //        iNota.PmtPotongan = 0;
                //        iNota.PmtDiterima = 0;
                //        iNota.PmtAdjutment = 0;
                //    }
            }
        }

        private void AcceptAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            //  throw new NotImplementedException();
            /*
            PropertyEditor peSelisih = ((DetailView)View).FindItem("Selisih") as PropertyEditor;



            if ((double)peSelisih.PropertyValue < 0)
            {
                Application.ShowViewStrategy.ShowMessage($" Selisih tidak boleh minus ");
                dlgController.CanCloseWindow = false;
            }
            else
            {
                NotaPayment oNotaPayment = (NotaPayment)((DetailView)View).CurrentObject;

                // ambil list view
                ListPropertyEditor peListnote = ((DetailView)View).FindItem("ListNota") as ListPropertyEditor;
                XPCollection<Nota> oListNota = (XPCollection<Nota>)peListnote.PropertyValue;
                if (oListNota.Count > 0 && ((double)peSelisih.PropertyValue >= 0))
                {
                    Session oSession = (((DetailView)View).CurrentObject as XPObject).Session;
                    IObjectSpace objectSpaceInternal = Application.CreateObjectSpace(typeof(NotaPaymentDtl));
                    string sqlQuery = string.Format(" Oid == {0} ", oNotaPayment.Oid);
                    CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);
                    NotaPayment oNotaPay = oSession.FindObject<NotaPayment>(filterOperator);


                    foreach (Nota iNota in oListNota)
                    {

                        sqlQuery = string.Format(" Oid == {0} ", iNota.Oid);
                        filterOperator = CriteriaOperator.Parse(sqlQuery);
                        Nota oNota = oSession.FindObject<Nota>(filterOperator);
                        //try
                        //{
                        //NotaPaymentDtl obj = objectSpaceInternal.CreateObject<NotaPaymentDtl>();

                        if (iNota.PmtAdjutment != 0 || iNota.PmtPotongan != 0 || iNota.PmtDiterima != 0)
                        {
                            NotaPaymentDtl obj = new NotaPaymentDtl(oSession);

                            obj.Faktur = oNota;
                            obj.Pot = oNota.PmtPotongan;
                            obj.Adjustment = oNota.PmtAdjutment;
                            obj.Diterima = oNota.PmtDiterima;
                            if (oNotaPay.Reference != null)
                            {
                                obj.Referensi = oNotaPay.Reference.Reference;
                            }
                            //obj.Tanggal = oNotaPay.Tanggal;
                            obj.NotaPayment = oNotaPay;

                            // ADD SUPIR CLAIM
                            if (oNota.PmtPotongan != 0)
                            {
                                if (oNota.DM != null)
                                {
                                    if (oNota.DM.NamaSpr != null)
                                    {

                                        if (oNota.DM.NamaSpr.Group != null)
                                        {
                                            sqlQuery = string.Format(" Oid == {0} ", oNota.DM.NamaSpr.Group.Oid);
                                        }
                                        else
                                        {
                                            sqlQuery = string.Format(" Oid == {0} ", oNota.DM.NamaSpr.Oid);
                                        }
                                        filterOperator = CriteriaOperator.Parse(sqlQuery);
                                        Supir oSupir = oSession.FindObject<Supir>(filterOperator);

                                        SupirKlaim OSupirKlaim = new SupirKlaim(oSession);
                                        OSupirKlaim.Jumlah = oNota.PmtPotongan;
                                        OSupirKlaim.Jenis = eSupirKlaim.Pelanggan;
                                        OSupirKlaim.Supir = oSupir;
                                        OSupirKlaim.Keterangan = $"Potongan SM {oNota.NoTTDSM}";
                                        OSupirKlaim.Tanggal = oNotaPay.Tanggal;
                                        OSupirKlaim.DM = oNota.DM;
                                        OSupirKlaim.Save();
                                        OSupirKlaim.Session.CommitTransaction();
                                    }
                                }

                            }

                            oNota.PmtPotongan = 0;
                            oNota.PmtAdjutment = 0;
                            oNota.PmtDiterima = 0;
                            obj.Save();
                            obj.Session.CommitTransaction();

                        }
                        //}
                        //catch (Exception)
                        //{ }
                    }
                    this.ObjectSpace.CommitChanges();
                    this.ObjectSpace.Refresh();
                    Application.ShowViewStrategy.ShowMessage($" Data tersimpan");
                    //ListPropertyEditor listPaymentDtl = ((DetailView)View).FindItem("NotaPaymentDtl") as ListPropertyEditor;
                }
                dlgController.CanCloseWindow = true;
            }
            */
        }

        private void LinkAction_Executed(object sender, ActionBaseEventArgs e)
        {
            // throw new NotImplementedException();
            var linkedItems = ((SimpleActionExecuteEventArgs)e).SelectedObjects;
            Application.ShowViewStrategy.ShowMessage($" link {linkedItems.Count} ");
        }

        private void UnlinkAction_Executed(object sender, ActionBaseEventArgs e)
        {
            //throw new NotImplementedException();
            var unlinkedItems = ((SimpleActionExecuteEventArgs)e).SelectedObjects;
            Application.ShowViewStrategy.ShowMessage($" unlink {unlinkedItems.Count} ");
        }

        private void listPropertyEditor_ControlCreated(object sender, EventArgs e)
        {
            ProcessListPropertyEditor((ListPropertyEditor)sender);
            // throw new NotImplementedException();
        }

        private void ProcessListPropertyEditor(ListPropertyEditor listPropertyEditor)
        {
            //throw new NotImplementedException();
            ListView nestedListView = listPropertyEditor.ListView;
            PerformLogicWithCurrentListViewObject(nestedListView.CurrentObject);
            PerformLogicInNestedListViewController(listPropertyEditor.Frame);
            nestedListView.CurrentObjectChanged += nestedListView_CurrentObjectChanged;
        }

        private void nestedListView_CurrentObjectChanged(object sender, EventArgs e)
        {
            PerformLogicWithCurrentListViewObject((Nota)((ListView)sender).CurrentObject);
            // throw new NotImplementedException();
        }

        private void PerformLogicInNestedListViewController(Frame frame)
        {
            throw new NotImplementedException();
        }

        private void PerformLogicWithCurrentListViewObject(object currentObject)
        {
            throw new NotImplementedException();
        }

        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            isDeactivated = true;
            var linkController = Frame.GetController<LinkUnlinkController>();
            if (linkController != null)
            {
                linkController.UnlinkAction.Executed -= UnlinkAction_Executed;
                linkController.LinkAction.Executed -= LinkAction_Executed;
            }

            ListPropertyEditor peListnote = ((DetailView)View).FindItem("ListSM") as ListPropertyEditor;
            XPCollection<SuratMuatan> oListNota = (XPCollection<SuratMuatan>)peListnote.PropertyValue;
            foreach (SuratMuatan iNota in oListNota)
            {
                //iNota.PmtPotongan = 0;
                //iNota.PmtDiterima = 0;
                //iNota.PmtAdjutment = 0;
                iNota.Save();
                iNota.Session.CommitTransaction();


            }
            ListPropertyEditor listPropertyEditor = ((DetailView)View).FindItem("ListSM") as ListPropertyEditor;
            if (listPropertyEditor != null)
            {
                listPropertyEditor.ControlCreated -= new EventHandler<EventArgs>(listPropertyEditor_ControlCreated);
            }
            if (dlgController != null)
            {
                dlgController.AcceptAction.Execute -= AcceptAction_Execute;
                dlgController.CancelAction.Execute -= CancelAction_Execute;
                dlgController = null;
            }
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
