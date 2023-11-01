// Class Name : NotaPaymentDtl.cs 
using System; 
using DevExpress.Xpo; 
using DevExpress.Persistent.Base; 
using System.ComponentModel; 
using DevExpress.Persistent.Validation; 
using DevExpress.ExpressApp.DC; 
using DevExpress.ExpressApp.ConditionalAppearance; 
using DevExpress.ExpressApp; 
using DevExpress.Data.Filtering; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks;
using DevExpress.ExpressApp.Model;

namespace TriJasa.Module.BusinessObjects
{ 
   [DefaultClassOptions] 
   [ImageName("ModelEditor_Views")] 
   [DefaultProperty("Referensi")]
   [NavigationItem("Inquiry")]
   // Standard Document
   [System.ComponentModel.DisplayName("PenerimaanPiutangDagang")]
   public class NotaPaymentDtl : XPObject
   {
     public NotaPaymentDtl() : base()
     {
     // This constructor is used when an object is loaded from a persistent storage.
     // Do not place any code here.
     }
     public NotaPaymentDtl(Session session) : base(session) 
     {
       // This constructor is used when an object is loaded from a persistent storage.
       // Do not place any code here.
     }
     public override void AfterConstruction()
     {
       base.AfterConstruction();
       // Place here your initialization code.
       //SecuritySystem.CurrentUserName
       //LastUpdateUser = Session.GetObjectByKey<GPUser>(SecuritySystem.CurrentUserId);
       string tUser = SecuritySystem.CurrentUserName.ToString();
            UserUpdate = Session.FindObject<UserLogin>(new BinaryOperator("UserName", tUser));
            lastUpdate = System.DateTime.Now;
            Referensi = "";
            //LastUpdateUser = Session.FindObject<GPUser>(new BinaryOperator("UserName", SecuritySystem.CurrentUserName.ToString())); 
            // LastUpdateUser = Session.FindObject<GPUser>(new BinaryOperator("UserName", tUser)); 
            // LastUpdate = DateTime.Now; 
            XPCollection<NotaPaymentDtl> xpDM = new XPCollection<NotaPaymentDtl>(Session);
            int OidTem = xpDM.Max(x => Oid);
            Oid = OidTem + 1;
            if (Oid < 1)
            {
                Oid = ((DateTime.Now.Hour * 60 + DateTime.Now.Minute) * 60 + DateTime.Now.Second) * 100 + DateTime.Now.Millisecond;
            }
        } 
     protected override void OnSaving()
     {
            string tUser = SecuritySystem.CurrentUserName.ToString();
            UserUpdate = Session.FindObject<UserLogin>(new BinaryOperator("UserName", tUser));
            base.OnSaving();
     } 
     protected override void OnDeleting()
     {
       base.OnDeleting();
     } 
     // 
     private NotaPayment _NotaPayment; 
     [XafDisplayName("NotaPayment"), ToolTip("NotaPayment")] 
     [Association("NotaPayAssc")] 
     public virtual NotaPayment NotaPayment
     { 
       get { return _NotaPayment; } 
       set { SetPropertyValue("NotaPayment", ref _NotaPayment, value); } 
     }
        private Nota _Faktur;
        [XafDisplayName("Nota"), ToolTip("Nota")]
        [ImmediatePostData]
        //[Association("NotaPayAssc")]
        public virtual Nota Faktur
        {
            get { return _Faktur; }
            set { 
                SetPropertyValue("Faktur", ref _Faktur, value); 
                if ( !IsLoading && !IsSaving )
                  {
                    if (NotaPayment !=null )
                    {
                        try
                        {
                            CriteriaOperator oCriteria = null;
                            oCriteria = GroupOperator.And(oCriteria, new BinaryOperator("Faktur", this.Faktur));
                            XPCollection<NotaPaymentDtl> oNotaPaymentDtl = new XPCollection<NotaPaymentDtl>(Session, oCriteria);
                            // double oTelahDiTerima = oNotaPaymentDtl.Where(x => x.Faktur == value).Sum(x => x.Diterima + x.Pot);
                            double oTelahDiTerima = oNotaPaymentDtl.Sum(x => x.Diterima + x.Pot);
                            double oSisa = value.TotalBiaya - oTelahDiTerima;
                            if (oSisa < NotaPayment.Selisih)
                            {
                                Diterima = oSisa;
                            }
                            else
                            {
                                Diterima = NotaPayment.Selisih;
                            }
                            NotaPayment.Pelanggan = value.Pelangan;
                        }
                        catch ( Exception e)
                        {

                        }
                    }
                  }
                }
        }


        private DateTime _Tanggal;
        [XafDisplayName("Tanggal Nota"), ToolTip("Tanggal Nota")]
        [ModelDefault("DisplayFormat", "{0: dd/MM/yyyy}")]

        [PersistentAlias("Faktur.Tanggal")]
        public virtual DateTime Tanggal
        {
            get {
                
                //_Tanggal ;
                object tempObject = EvaluateAlias(nameof(Tanggal));
                if (tempObject != null)
                {
                    return (DateTime)tempObject;
                }
                else
                {
                    return new DateTime();
                    // return _TotalBiayaLain; 
                }
                //return _Tanggal; 
              }
            //set { SetPropertyValue("Tanggal", ref _Tanggal, value); }
        }
        private DateTime _lastUpdate;
        [ModelDefault("DisplayFormat", "{0: dd/MM/yyyy}")]

        [XafDisplayName("Tgl"), ToolTip("Tgl")]
        [Size(20)]
        //[ImmediatePostData]
        public virtual DateTime lastUpdate
        {
            get { return _lastUpdate; }
            set { SetPropertyValue("lastUpdate", ref _lastUpdate, value); }
        }
        private UserLogin _UserUpdate;
        [XafDisplayName("User"), ToolTip("User")]
        public virtual UserLogin UserUpdate
        {
            get { return _UserUpdate; }
            set { SetPropertyValue("UserUpdate", ref _UserUpdate, value); }
        }
        private string _Referensi;

     

        [XafDisplayName("Referensi"), ToolTip("Referensi")] 
     [Size(20)] 
     public virtual string Referensi
     { 
       get { return _Referensi; } 
       set { SetPropertyValue("Referensi", ref _Referensi, value); } 
     }
        // 
        private double _Jumlah;
        [XafDisplayName("Jumlah Nota"), ToolTip("Jumlah Nota")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        [PersistentAlias("Faktur.TotalBiaya")]
        public virtual double Jumlah
        {
            
            get {
                 
                object tempObject = EvaluateAlias(nameof(Jumlah));
                if (tempObject != null)
                {
                    return (double)tempObject;
                }
                else
                {
                    return 0;
                    // return _TotalBiayaLain; 
                }
                  //  return _Jumlah;
                }
            //set { SetPropertyValue("Jumlah", ref _Jumlah, value); }
        }

     private double _TelahDiTerima; 
     [XafDisplayName("Telah di Terima"), ToolTip("Telah di Terima")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        public virtual double TelahDiTerima
     { 
       get {
                _TelahDiTerima = 0;
                if ( !IsLoading  && !IsSaving)
                {
                    CriteriaOperator oCriteria = null;
                    oCriteria = GroupOperator.And(oCriteria, new BinaryOperator("Faktur", this.Faktur));
                    XPCollection<NotaPaymentDtl> oNotaPaymentDtl = new XPCollection<NotaPaymentDtl>(Session, oCriteria);
                    //_TelahDiTerima = oNotaPaymentDtl.Where(x => x.Faktur == Faktur).Sum(x => x.Diterima);
                    if (oNotaPaymentDtl.Count > 0)
                    {
                        _TelahDiTerima = oNotaPaymentDtl.Sum(x => x.Diterima);
                    }
                    //if (tempObject != null)
                    //{
                    //    return (double)tempObject;
                    //}
                }
                
                return _TelahDiTerima; } 
       //set { SetPropertyValue("TelahDiTerima", ref _TelahDiTerima, value); } 
     } 
     // 
     private double _Pot; 
     [XafDisplayName("Potongan"), ToolTip("Potongan")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        [ImmediatePostData]
     public virtual double Pot
     { 
       get { return _Pot; } 
       set { SetPropertyValue("Pot", ref _Pot, value);

                if (!IsLoading && !IsSaving)
                {
                    if (NotaPayment != null)
                    {
                        try
                        {
                            CriteriaOperator oCriteria = null;
                            oCriteria = GroupOperator.And(oCriteria, new BinaryOperator("Faktur", this.Faktur));
                            XPCollection<NotaPaymentDtl> oNotaPaymentDtl = new XPCollection<NotaPaymentDtl>(Session, oCriteria);
                            // double oTelahDiTerima = oNotaPaymentDtl.Where(x => x.Faktur == this.Faktur).Sum(x => x.Diterima + x.Pot + x.Adjustment);
                            double oTelahDiTerima = oNotaPaymentDtl.Sum(x => x.Diterima + x.Pot + x.Adjustment);
                            double oSisa = this.Faktur.TotalBiaya -( oTelahDiTerima+value);
                            if (oSisa < NotaPayment.Selisih)
                            {
                                Diterima = oSisa;
                            }
                            else
                            {
                                Diterima = NotaPayment.Selisih;
                            }
                          //  NotaPayment.Pelanggan = value.Pelangan;
                        }
                        catch (Exception e)
                        {

                        }
                    }
                }

            } 
     }
        private double _Adjustment;
        [XafDisplayName("Penyesuaian"), ToolTip("Penyesuaian")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        [ImmediatePostData]
        public virtual double Adjustment
        {
            get { return _Adjustment; }
            set
            {
                SetPropertyValue("Adjustment", ref _Adjustment, value);

                if (!IsLoading && !IsSaving)
                {
                    if (NotaPayment != null)
                    {
                        try
                        {
                            CriteriaOperator oCriteria = null;
                            oCriteria = GroupOperator.And(oCriteria, new BinaryOperator("Faktur", this.Faktur));

                            XPCollection<NotaPaymentDtl> oNotaPaymentDtl = new XPCollection<NotaPaymentDtl>(Session, oCriteria);
                            //double oTelahDiTerima = oNotaPaymentDtl.Where(x => x.Faktur == this.Faktur).Sum(x => x.Diterima + x.Pot  +x.Adjustment);
                            double oTelahDiTerima = oNotaPaymentDtl.Sum(x => x.Diterima + x.Pot + x.Adjustment);
                            double oSisa = this.Faktur.TotalBiaya - (oTelahDiTerima + value);
                            if (oSisa < NotaPayment.Selisih)
                            {
                                Diterima = oSisa;
                            }
                            else
                            {
                                Diterima = NotaPayment.Selisih;
                            }
                            //  NotaPayment.Pelanggan = value.Pelangan;
                        }
                        catch (Exception e)
                        {

                        }
                    }
                }

            }
        }

        // 
   private double _Sisa; 
     [XafDisplayName("Sisa"), ToolTip("Sisa")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        [PersistentAlias("Jumlah- (TelahDiTerima +Pot+Adjustment) ")]
        public virtual double Sisa
     { 
       get {
                object tempObject = EvaluateAlias(nameof(Sisa));
                if (tempObject != null)
                {
                    return (double)tempObject;
                }
                else
                {
                    return 0;

                }
                //return _Sisa; 
            } 
       //set { SetPropertyValue("Sisa", ref _Sisa, value); } 
     } 
     // 
     private double _Diterima; 
     [XafDisplayName("Sekarang diTerima"), ToolTip("Sekarang diTerima")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        [ImmediatePostData]
     public virtual double Diterima
     { 
       get { return _Diterima; } 
       set { SetPropertyValue("Diterima", ref _Diterima, value); } 
     } 
   } 
   
   
} 
