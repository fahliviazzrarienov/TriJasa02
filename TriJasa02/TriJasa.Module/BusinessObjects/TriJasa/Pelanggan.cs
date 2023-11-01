// Class Name : Pelanggan.cs 
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
using DevExpress.ExpressApp.Editors;

namespace TriJasa.Module.BusinessObjects
{ 
   [DefaultClassOptions] 
   [ImageName("ModelEditor_Views")] 
   [DefaultProperty("Nama")]
   [NavigationItem("Master")]
   // Standard Document
   [System.ComponentModel.DisplayName("Pengirim")]
   public class Pelanggan : XPObject
   {
     public Pelanggan() : base()
     {
     // This constructor is used when an object is loaded from a persistent storage.
     // Do not place any code here.
     }
     public Pelanggan(Session session) : base(session) 
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
       //LastUpdateUser = Session.FindObject<GPUser>(new BinaryOperator("UserName", SecuritySystem.CurrentUserName.ToString())); 
       // LastUpdateUser = Session.FindObject<GPUser>(new BinaryOperator("UserName", tUser)); 
       // LastUpdate = DateTime.Now; 
     } 
     protected override void OnSaving()
     {
       base.OnSaving();
     } 
     protected override void OnDeleting()
     {
       base.OnDeleting();
     }
        // 

    private string _Kode;
    [XafDisplayName("Kode"), ToolTip("Kode")]
    [Size(7)]
    //[IsSearch(true)]
    public virtual string Kode
        {
        get {
                _Kode = "";
                if (!IsLoading && !IsSaving)
                {
                    if (Oid > 0)
                    {
                        _Kode = "P" + Oid.ToString("000000");
                    }
                }
                return _Kode; }
       /// set { SetPropertyValue("Kode", ref _Kode, value); }
    }

        private string _KodeNama;
        [XafDisplayName("Kode & Nama"), ToolTip("Kode & Nama")]
        [Appearance("PelangganKodeNameV", Visibility = ViewItemVisibility.Hide)]
        // [Size(150)]
        [IsSearch(true)]
        public virtual string KodeNama
        {
            get {
                _KodeNama = "";
                if (!IsLoading && !IsSaving)
                {
                    if (Oid > 0)
                    {
                        _KodeNama =$"{Kode}-{Nama} " ;
                    }
                }
                return _KodeNama;
               }
            //set
            //{
            //    SetPropertyValue("Nama", ref _Nama, value);
            //}
        }


      private string _Nama; 
     [XafDisplayName("Nama"), ToolTip("Nama")] 
     [Size(150)] 
     [IsSearch(true)]
     public virtual string Nama
     { 
       get { return _Nama; } 
       set {
                if (!IsLoading && !IsSaving)
                {
                    value = value.ToUpper();
                }
                SetPropertyValue("Nama", ref _Nama, value); 
            } 
     } 
     // 
     private string _ContactPerson; 
     [XafDisplayName("Contact Person"), ToolTip("Contact Person")] 
     [Size(100)] 
     public virtual string ContactPerson
     { 
       get { return _ContactPerson; } 
       set { SetPropertyValue("ContactPerson", ref _ContactPerson, value); } 
     }
        // 
        
        private string _NoTelp;
        [XafDisplayName("No. Telp"), ToolTip("No. Telp")]
        [Appearance("PelangganNotelp", Visibility = ViewItemVisibility.Hide)]
        [Size(50)]
        public virtual string NoTelp
        {
            get { return _NoTelp; }
            set { SetPropertyValue("No.Telp", ref _NoTelp, value); }
        }

     private string _PelangganAlamat1; 
     [XafDisplayName("PelangganAlamat1"), ToolTip("PelangganAlamat1")] 
     [Size(150)] 
     public virtual string PelangganAlamat1
     { 
       get {
                if ( !IsLoading && !IsSaving)
                {
                    if (_PelangganAlamat1==null)
                    {
                        _PelangganAlamat1 = "";
                    }
                }
                return _PelangganAlamat1; } 
       set { SetPropertyValue("PelangganAlamat1", ref _PelangganAlamat1, value); } 
     } 
     // 
     private string _PelangganAlamat2; 
     [XafDisplayName("PelangganAlamat2"), ToolTip("PelangganAlamat2")] 
     [Size(150)] 
     public virtual string PelangganAlamat2
     { 
       get {
                if (!IsLoading && !IsSaving)
                {
                    if (_PelangganAlamat2 == null)
                    {
                        _PelangganAlamat2 = "";
                    }
                }
                return _PelangganAlamat2; } 
       set { SetPropertyValue("PelangganAlamat2", ref _PelangganAlamat2, value); } 
     } 
     // 
     private string _PelangganAlamat3; 
     [XafDisplayName("PelangganAlamat3"), ToolTip("PelangganAlamat3")] 
     [Size(150)] 
     public virtual string PelangganAlamat3
     { 
       get
            {
                if (!IsLoading && !IsSaving)
                {
                    if (_PelangganAlamat3 == null)
                    {
                        _PelangganAlamat3 = "";
                    }
                }
                return _PelangganAlamat3; } 
       set { SetPropertyValue("PelangganAlamat3", ref _PelangganAlamat3, value); } 
     } 
     // 
     private string _PelangganAlamat4; 
     [XafDisplayName("PelangganAlamat4"), ToolTip("PelangganAlamat4")] 
     [Size(150)] 
     public virtual string PelangganAlamat4
     { 
       get {

                if (!IsLoading && !IsSaving)
                {
                    if (_PelangganAlamat4 == null)
                    {
                        _PelangganAlamat4 = "";
                    }
                }
                return _PelangganAlamat4; } 
       set { SetPropertyValue("PelangganAlamat4", ref _PelangganAlamat4, value); } 
     } 
     // 
     private Pelanggan _Penagihan; 
     [XafDisplayName("Alamat Penagihan"), ToolTip("Alamat Penagihan")] 
     public virtual Pelanggan Penagihan
     { 
       get { return _Penagihan; } 
       set { SetPropertyValue("Penagihan", ref _Penagihan, value); } 
     } 
     // 
     private string _Kota; 
     [XafDisplayName("Kota"), ToolTip("Kota")] 
     [Size(150)] 
     public virtual string Kota
     { 
       get { return _Kota; } 
       set { SetPropertyValue("Kota", ref _Kota, value); } 
     } 
     // 
     private string _Propinsi; 
     [XafDisplayName("Propinsi"), ToolTip("Propinsi")] 
     [Size(50)] 
     public virtual string Propinsi
     { 
       get { return _Propinsi; } 
       set { SetPropertyValue("Propinsi", ref _Propinsi, value); } 
     }

        // 
    private int _Term;
    [XafDisplayName("Term"), ToolTip("Term")]
    //[Size(50)]
    public virtual int Term
        {
        get {  
                //if ( _Term==null)
                //{
                //    _Term = 0;
                //}
                return _Term; }
        set { SetPropertyValue("Term", ref _Term, value); }
    }
        private bool _PPN;
        [XafDisplayName("PPN"), ToolTip("PPN")]
        [Size(50)]
        public virtual bool PPN
        {
            get { return _PPN; }
            set { SetPropertyValue("PPN", ref _PPN, value); }
        }

        private bool _PembayaranSementara;
        [XafDisplayName("Pembayaran Sementara"), ToolTip("Pembayaran Sementara")]
        //[Size(50)]
        public virtual bool PembayaranSementara
        {
            get { return _PembayaranSementara; }
            set { SetPropertyValue("PembayaranSementara", ref _PembayaranSementara, value); }
        }

    } 
   
} 
