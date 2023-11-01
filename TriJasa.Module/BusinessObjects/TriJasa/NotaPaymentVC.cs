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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TriJasa.Module.BusinessObjects
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class NotaPaymentVC : ViewController
    {
        DialogController dlgController;
        //SimpleAction UnlinkAction;
        public NotaPaymentVC()
        {
            InitializeComponent();
            TargetObjectType = typeof(NotaPayment);
            //TargetViewNesting = Nesting.Nested;
            TargetViewType = ViewType.DetailView;
            TargetViewId = "NotaPayment_DetailView_Pelunasan";
            //TargetObjectType = typeof(Nota);
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            isDeactivated = false;
            // Perform various tasks depending on the target View.
            ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
            ListPropertyEditor listPropertyEditor = ((DetailView)View).FindItem("ListNota") as ListPropertyEditor;
            
            
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
            // dlgController.Actions["DialogOK"].Caption = "okokk";
           
        }

        private void AcceptAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
             PropertyEditor peSelisih = ((DetailView)View).FindItem("Selisih") as PropertyEditor;

          

            if ((double)peSelisih.PropertyValue <0)
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
            

        }
        private void CancelAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            //throw new NotImplementedException();
            //dlgController.CanCloseWindow = false;
            // dlgController.SaveOnAccept = false;
            //Application.ShowViewStrategy.ShowMessage($" cancel  ");

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
        private void LinkAction_Executed(object sender, ActionBaseEventArgs e)
        {
            
            var linkedItems = ((SimpleActionExecuteEventArgs)e).SelectedObjects;
            Application.ShowViewStrategy.ShowMessage($" link {linkedItems.Count} ");
        }

        private void UnlinkAction_Executed(object sender, ActionBaseEventArgs e)
        {
            var unlinkedItems = ((SimpleActionExecuteEventArgs)e).SelectedObjects;
            Application.ShowViewStrategy.ShowMessage($" unlink {unlinkedItems.Count} ");
        }

        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            //throw new NotImplementedException();
            if (View !=null)
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(NotaPayment));

                NotaPayment oNotaPayment = ((NotaPayment)View.CurrentObject);
                int diterima = 0;
                int Klaim = 0;
                int Penyesuaian = 0;
                double Selisih = 0;

                if (((NotaPayment)View.CurrentObject).ListNota !=null )
                {

                    XPCollection<Nota> lsNota = ((NotaPayment)View.CurrentObject).ListNota;

                    foreach ( Nota itemNota in lsNota)
                    {
                        if  (itemNota.PmtDiterima==0 && itemNota.PmtPotongan==0 && itemNota.PmtAdjutment==0)
                        {
                            if (isDeactivated == false)
                            {
                                itemNota.PmtDiterima = (int)itemNota.Sisa;
                            }
                        }
                        if (oNotaPayment.Pelanggan==null)
                        {
                            oNotaPayment.Pelanggan = itemNota.Pelangan;
                        }
                        diterima += itemNota.PmtDiterima;
                        Klaim += itemNota.PmtPotongan;
                        Penyesuaian += itemNota.PmtAdjutment;
                        Selisih = oNotaPayment.Selisih;
                        //itemNota.Save();
                        //itemNota.Save();
                        //oNotaPayment.ListNota.
                        //oNotaPayment.ListNota.Load();
                    }
                   // oNotaPayment.PayDetail();
                     ListPropertyEditor editor = ((DetailView)View).FindItem("ListNota") as ListPropertyEditor;
                    PropertyEditor peSelisih = ((DetailView)View).FindItem("Selisih") as PropertyEditor;
                    //peSelisih.ReadValue();
                    //this.ObjectSpace.CommitChanges();
                   
                    PropertyEditor peJumlah = ((DetailView)View).FindItem("Jumlah") as PropertyEditor;
                    //((DetailView)View).ObjectSpace.CommitChanges();
                    //this.View.ObjectSpace.CommitChanges();
                    //this.View.ObjectSpace.Refresh();
                    ////if ((double)peJumlah.PropertyValue != diterima)
                    //{
                    //    //oNotaPayment.lastUpdate = System.DateTime.Now;
                    //    //this.ObjectSpace.CommitChanges();
                    //    //this.ObjectSpace.Refresh();

                    //    //                        ((NotaPayment)View.CurrentObject).


                    //    //double juml = oNotaPayment.Jmlditerima;
                    //    //oNotaPayment.Jmlditerima = juml;


                    //    //Application.ShowViewStrategy.ShowMessage($" {peSelisih.PropertyValue} Selisih  {peJumlah.PropertyValue} tidak boleh minus {diterima}");
                    //    //peSelisih.PropertyValue = diterima;

                    //    //dlgController.CanCloseWindow = false;
                    //}

                    //((DetailView)View).Refresh();
                    //peSelisih.Refresh();
                    // this.ObjectSpace.Refresh();
                    //peSelisih.PropertyValue = diterima;

                    //editor.PropertyValue()
                    ///this.ObjectSpace.Refresh();
                    if (((NotaPayment)View.CurrentObject).ListNota.Count!= ((NotaPayment)View.CurrentObject).NotaPaymentDtl.Count)
                    {
                        // Application.ShowViewStrategy.ShowMessage($"ListNota");
                        // ((NotaPayment)View.CurrentObject).PayDetail();
                        //NotaPayment oNotaPayment = (NotaPayment)View.CurrentObject;
                        // oNotaPayment.PayDetail();
                     //   this.ObjectSpace.Refresh();

                    }
                    //((DetailView)View).Refresh();
                }

                if (View.CurrentObject == e.Object)
                {

                   /// dlgController.Actions["DialogOK"].Enabled[]

                    // DaftarMuatan DM = ((SuratMuatan)View.CurrentObject).NomorDM;
                    if (e.PropertyName == "ListNota")
                    {
                        ListPropertyEditor editor = ((DetailView)View).FindItem("ListNota") as ListPropertyEditor;
                        Application.ShowViewStrategy.ShowMessage($"ListNota");
                    }

                   
                }
            }
        }
        private void PerformLogicWithCurrentListViewObject(Object obj)
        {

            if (obj==null)
            {

            }
            // Use the object in the nested List View as required.
            //foreach (PenerimaanKasir selectedObject in obj.SelectedObjects)
            //{
            //    aListDM.Add(selectedObject.Oid);
            //}
            //Application.ShowViewStrategy.ShowMessage($"{obj }");
        }
        private void PerformLogicInNestedListViewController(Frame nestedFrame)
        {
            // Use the nested Frame as required.
            
            //if (nestedFrame.View != null)
            //{
            //    ListView editor = ((ListView)View).FindItem("Terparkir") as PropertyEditor;
            //    //if (View.Curren == nestedFrame.)
            //    //{
            //    //    if (e.PropertyName == "ListNota")
            //    //    {
            //    //        PropertyEditor editor = ((DetailView)View).FindItem("Terparkir") as PropertyEditor;
            //    //        Application.ShowViewStrategy.ShowMessage($"ListNota");
            //    //    }
            //    //}
            //}
        }
        //protected virtual void Accept(SimpleActionExecuteEventArgs args)
        //{
        //    this.acceptCancelled = false;
        //    DialogControllerAcceptingEventArgs dialogControllerAcceptingEventArgs = new DialogControllerAcceptingEventArgs(args.ShowViewParameters, args);
        //    dialogControllerAcceptingEventArgs.Cancel = false;
        //    if (this.Accepting != null)
        //    {
        //        this.Accepting(this, dialogControllerAcceptingEventArgs);
        //        this.acceptCancelled = dialogControllerAcceptingEventArgs.Cancel;
        //    }
        //    if (!dialogControllerAcceptingEventArgs.Cancel && base.Window != null && base.Window.Context == TemplateContext.PopupWindow && this.saveOnAccept)
        //    {
        //        ModificationsController controller = base.Window.GetController<ModificationsController>();
        //        if (controller != null && controller.SaveAction.Available)
        //        {
        //            this.acceptCancelled = !controller.SaveAction.DoExecute();
        //        }
        //    }
        //}
        private void ProcessListPropertyEditor(ListPropertyEditor listPropertyEditor)
        {
            ListView nestedListView = listPropertyEditor.ListView;
            PerformLogicWithCurrentListViewObject(nestedListView.CurrentObject);
            PerformLogicInNestedListViewController(listPropertyEditor.Frame);
            nestedListView.CurrentObjectChanged += nestedListView_CurrentObjectChanged;
        }
        private void listPropertyEditor_ControlCreated(object sender, EventArgs e)
        {
            ProcessListPropertyEditor((ListPropertyEditor)sender);
        }
        private void nestedListView_CurrentObjectChanged(object sender, EventArgs e)
        {
            PerformLogicWithCurrentListViewObject((Nota)((ListView)sender).CurrentObject);
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }

        private bool isDeactivated;
        protected override void OnDeactivated()
        {
            isDeactivated = true;
            var linkController = Frame.GetController<LinkUnlinkController>();
            if (linkController != null)
            {
                linkController.UnlinkAction.Executed -= UnlinkAction_Executed;
                linkController.LinkAction.Executed -= LinkAction_Executed;
            }

            ListPropertyEditor peListnote = ((DetailView)View).FindItem("ListNota") as ListPropertyEditor;
            XPCollection<Nota> oListNota = (XPCollection<Nota>)peListnote.PropertyValue;
            foreach (Nota iNota in oListNota)
            {
                iNota.PmtPotongan = 0;
                iNota.PmtDiterima = 0;
                iNota.PmtAdjutment = 0;
                iNota.Save();
                iNota.Session.CommitTransaction();


            }
            ListPropertyEditor listPropertyEditor = ((DetailView)View).FindItem("ListNota") as ListPropertyEditor;
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
