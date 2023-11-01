// Class Name : Supir.cs 
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
   //[DefaultProperty("ID")]
   [NavigationItem("Transaksi")]
   // Standard Document
   [System.ComponentModel.DisplayName("Supir Klaim")]

    public class SupirKlaim : XPObject
    {
     public SupirKlaim() : base()
     {
     // This constructor is used when an object is loaded from a persistent storage.
     // Do not place any code here.
     }
     public SupirKlaim(Session session) : base(session) 
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
            //this.Nama
            //LastUpdateUser = Session.FindObject<GPUser>(new BinaryOperator("UserName", SecuritySystem.CurrentUserName.ToString())); 
            // LastUpdateUser = Session.FindObject<GPUser>(new BinaryOperator("UserName", tUser)); 
            // LastUpdate = DateTime.Now; 
            Tanggal = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
     } 
     protected override void OnSaving()
     {
       base.OnSaving();
     } 
     protected override void OnDeleting()
     {
       base.OnDeleting();
     }
        private string _NomorKlaim;
        public virtual string NomorKlaim
        {
            get
            {
                if (Oid > 0)
                {
                    _NomorKlaim = "SK" + Oid.ToString("000000");
                }
                return _NomorKlaim;
            }
            //    return _Nama; }
            //set { SetPropertyValue("Nama", ref _Nama, value); }
        }


        private DateTime _Tanggal;
        [XafDisplayName("Tanggal"), ToolTip("Tanggal")]
        [ModelDefault("DisplayFormat", "{0: dd/MM/yyyy}")]

        [Size(50)]
        //[IsSearch(true)]
        public virtual DateTime Tanggal
        {
            get { return _Tanggal; }
            set { SetPropertyValue("Tanggal", ref _Tanggal, value); }
        }

    private string _Keterangan;
    [XafDisplayName("Keterangan"), ToolTip("Keterangan")]
    [Size(100)]
    //[IsSearch(true)]
    public virtual string Keterangan
        {
        get { return _Keterangan; }
        set { SetPropertyValue("Keterangan", ref _Keterangan, value); }
    }
    private eSupirKlaim _Jenis;
    [XafDisplayName("Jenis"), ToolTip("Jenis")]
    [ImmediatePostData]
    [Size(50)]
    //[IsSearch(true)]
    public virtual eSupirKlaim Jenis
    {
        get { return _Jenis; }
        set { SetPropertyValue("Jenis", ref _Jenis, value); }
    }

    private Supir _Supir;
    [XafDisplayName("Supir"), ToolTip("Supir")]
      [Association("KlaimSupir")]
    //    [Size(50)]
    //[IsSearch(true)]
    public virtual Supir Supir
     {
        get { return _Supir; }
        set { SetPropertyValue("Supir", ref _Supir, value); }
    }
    private DaftarMuatan _DM;
    [XafDisplayName("DM"), ToolTip("DM")]
    //[Appearance("DisableProperty", Criteria = "1=1", Enabled = false)]
    [Appearance("SupirKlaimDMEnable", Enabled = false, Criteria = "Jenis in (1,2) ", Context = "DetailView")]
    [Size(50)]
    //[IsSearch(true)]
    public virtual DaftarMuatan DM
        {
        get { return _DM; }
        set { SetPropertyValue("DM", ref _DM, value); }
    }

        private SuratMuatan _SM;
        [XafDisplayName("SM"), ToolTip("SM")]
        [ImmediatePostData]
        [Size(50)]
        //[IsSearch(true)]
        public virtual SuratMuatan SM
        {
            get { return _SM; }
            set { SetPropertyValue("SM", ref _SM, value);
                 if (!IsLoading && !IsSaving)
                {
                    if (value !=null)
                    {
                        DM = value.NomorDM;
                    }
                }
                }
        }
        private double _Jumlah;
    [XafDisplayName("Jumlah"), ToolTip("Jumlah")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        [Size(50)]
    //[IsSearch(true)]
    public virtual double Jumlah
        {
        get { return _Jumlah; }
        set {
                if ( !IsLoading || !IsSaving)
                {
                    if (Jenis == eSupirKlaim.Pelanggan || Jenis == eSupirKlaim.Pembayaran)
                    {
                        if(value>0)
                        {
                            value = value * -1;
                        }
                    }
                }

                SetPropertyValue("Jumlah", ref _Jumlah, value);
               
             }
    }

    } 
   
    public enum eSupirKlaim
    {

        [XafDisplayName("Deposit DM")]
        DepositDM = 0,
        [XafDisplayName("Pelanggan Komplain")]
        Pelanggan = 1,
        [XafDisplayName("Potogan Pembayaran")]
        Pembayaran = 2,
        [XafDisplayName("lain Lain")]
        LainLain = 3
    }
} 
