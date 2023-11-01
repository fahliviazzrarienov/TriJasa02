// Class Name : Nota.cs 
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
   [DefaultProperty("NoNota")]
   [NavigationItem("Transaksi")]
   // Standard Document
   [System.ComponentModel.DisplayName("Nota")]
   public class Nota : XPObject
   {
     public Nota() : base()
     {
     // This constructor is used when an object is loaded from a persistent storage.
     // Do not place any code here.
     }
     public Nota(Session session) : base(session) 
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
            WithPPN = false;
            Tanggal = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            NoTTDSM = "";

     } 
     protected override void OnSaving()
     {
            if (NoNota=="" || NoNota ==null )
            {

                string xSMMax = SuratMuats.Max(x => x.NomorSM);
                string NotaTTDSM = "";
                foreach ( SuratMuatan oSM in SuratMuats)
                {
                    if  (DM==null)
                    {
                       this.DM= oSM.NomorDM;
                       this.Pelangan = oSM.Pelanggan;
                        this.PelanganNama= oSM.Pelanggan.Nama;
                        this.PelangganAlamat1 = oSM.PelangganAlamat1;
                        this.PelangganAlamat2 = oSM.PelangganAlamat2;
                        this.PelangganAlamat3 = oSM.PelangganAlamat3;
                        this.Tanggal = oSM.Tanggal;// new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                       this.WithPPN = oSM.Pelanggan.PPN;
                       this.NoTTDSM = oSM.TandaTerimaNo;
                        NotaTTDSM += oSM.TandaTerimaNo + "& ";
                    }

                }
                if (DM != null)
                {
                    string xNoNota = $"{DM.NomorDM}.{xSMMax}".Replace("DBL.", "");
                    NoNota = xNoNota;
                    if (NotaTTDSM.Length > 2)
                    {
                        NoTTDSM = NotaTTDSM.Substring(0, NotaTTDSM.Length - 2);
                    }
                    else

                    {
                        NoTTDSM = "";
                    }
                }
            }
            
                foreach (SuratMuatan oSM in SuratMuats)
                {
                    SuratMuat = oSM;
                }
    
       base.OnSaving();
     } 
     protected override void OnDeleting()
     {
       base.OnDeleting();
     }

        public void DeleteDetail()
        {

        }
        private DaftarMuatan _DM;
        [XafDisplayName("Daftar Muatan"), ToolTip("DaftarMuatan")]
        //[Size(15)]
        public virtual DaftarMuatan DM
        {
            get { return _DM; }
            set { SetPropertyValue("DM", ref _DM, value); }
        }

        // 
     private string _NoNota; 
     [XafDisplayName("Nomor Nota"), ToolTip("Nomor Nota")] 
     [Size(15)] 
     public virtual string NoNota
     { 
       get { return _NoNota; } 
       set { SetPropertyValue("NoNota", ref _NoNota, value); } 
     } 
     // 
     private DateTime _Tanggal; 
     [XafDisplayName("Tanggal"), ToolTip("Tanggal")]
        [ModelDefault("DisplayFormat", "{0: dd/MM/yyyy}")]
        [ImmediatePostData]
     public virtual DateTime Tanggal
     { 
       get { return _Tanggal; } 
       set {
                if (!IsLoading && !IsSaving && _Tanggal != null)
                {
                    if ((value.ToString("yyyyMMdd") != _Tanggal.ToString("yyyyMMdd"))
                        && ( NoNota =="" || NoNota==null))
                        
                    {
                        //iGenTriJasa igen = new iGenTriJasa();
                       // NoNota = igen.NotaNomorGet(Session, value);

                    }
                }
                SetPropertyValue("Tanggal", ref _Tanggal, value); 
                
            } 
     }

        private string _NoTTDSM;
        [XafDisplayName("Nomor SM/TTD"), ToolTip("Nomor SM/TTD")]
        [Size(100)]
        public virtual string NoTTDSM
        {
            get {
                //if (!IsSaving && !IsLoading)
                //{
                //    if (_NoTTDSM==null || _NoTTDSM=="")
                //    {

                //    }
                //}
                    return _NoTTDSM; }
            set { SetPropertyValue("NoTTDSM", ref _NoTTDSM, value); }
        }

        // 
        private Pelanggan _Pelangan; 
     [XafDisplayName("Pelanggan"), ToolTip("Pelanggan")] 
     [ImmediatePostData]
     public virtual Pelanggan Pelangan
     { 
       get { return _Pelangan; } 
       set { SetPropertyValue("Pelangan", ref _Pelangan, value);
                if (!IsSaving && !IsLoading)
                {
                    if (value != null)
                    {
                        PelanganNama = value.Nama;
                        
                        PelangganAlamat1 = value.PelangganAlamat1;
                        PelangganAlamat2 = value.PelangganAlamat2;
                        PelangganAlamat3 = value.PelangganAlamat3;
                       // PelangganTelp = value.NoTelp;

                    }
                }
            } 
     }
        // 
        private double _TelahDiTerima;
        [XafDisplayName("Telah di Terima"), ToolTip("Telah di Terima")]
        public virtual double TelahDiTerima
        {
            get
            {
                _TelahDiTerima = 0;
                if (!IsLoading && !IsSaving)
                {
                    CriteriaOperator oCriteria = null;
                    oCriteria = GroupOperator.And(oCriteria, new BinaryOperator("Faktur", this));
                    XPCollection<NotaPaymentDtl> oNotaPaymentDtl = new XPCollection<NotaPaymentDtl>(Session, oCriteria);
                    //  _TelahDiTerima = oNotaPaymentDtl.Where(x => x.Faktur == this).Sum(x => (x.Diterima+x.Pot +x.Adjustment ) );
                    _TelahDiTerima = oNotaPaymentDtl.Sum(x => (x.Diterima + x.Pot + x.Adjustment));
                    //if (tempObject != null)
                    //{
                    //    return (double)tempObject;
                    //}
                }

                return _TelahDiTerima;
            }
            //set { SetPropertyValue("TelahDiTerima", ref _TelahDiTerima, value); } 
        }
        private string _PelanganNama; 
     [XafDisplayName("Nama Pelanggan"), ToolTip("Nama Pelanggan")] 
     [Size(50)] 
     public virtual string PelanganNama
     { 
       get { return _PelanganNama; } 
       set { SetPropertyValue("PelanganNama", ref _PelanganNama, value); } 
     } 
     // 
     private string _PelangganAlamat1; 
     [XafDisplayName("Pelanggan Alamat1"), ToolTip("Pelanggan Alamat1")] 
     [Size(150)] 
     public virtual string PelangganAlamat1
     { 
       get { return _PelangganAlamat1; } 
       set { SetPropertyValue("PelangganAlamat1", ref _PelangganAlamat1, value); } 
     } 
     // 
     private string _PelangganAlamat2; 
     [XafDisplayName("Pelanggan Alamat2"), ToolTip("Pelanggan Alamat2")] 
     [Size(150)] 
     public virtual string PelangganAlamat2
     { 
       get { return _PelangganAlamat2; } 
       set { SetPropertyValue("PelangganAlamat2", ref _PelangganAlamat2, value); } 
     } 
     // 
     private string _PelangganAlamat3; 
     [XafDisplayName("Pelanggan Alamat3"), ToolTip("Pelanggan Alamat3")] 
     [Size(150)] 
     public virtual string PelangganAlamat3
     { 
       get { return _PelangganAlamat3; } 
       set { SetPropertyValue("PelangganAlamat3", ref _PelangganAlamat3, value); } 
     } 
     // 
     private string _PelangganTelp; 
     [XafDisplayName("PelangganTelp"), ToolTip("PelangganTelp")] 
     [Size(50)] 
     public virtual string PelangganTelp
     { 
       get { return _PelangganTelp; } 
       set { SetPropertyValue("PelangganTelp", ref _PelangganTelp, value); } 
     }

        private SuratMuatan _SuratMuat;
        [XafDisplayName("SuratMuat"), ToolTip("PelangganSuratMuatTelp")]

        public virtual SuratMuatan SuratMuat
        {
            get { return _SuratMuat; }
            set { SetPropertyValue("SuratMuat", ref _SuratMuat, value); }
        }

        // 
        [XafDisplayName("SuratMuatan"), ToolTip("SuratMuatan")]
        [Association("NotaSMinfo")]
        //[ImmediatePostData]
        public XPCollection<SuratMuatan> SuratMuats
        {
            get
            {
                return GetCollection<SuratMuatan>("SuratMuats");
            }
        }

        [XafDisplayName("Daftar Tagih"), ToolTip("Daftar Tagih")]
        [Association("TagihNotaList")]
        public XPCollection<DaftarTagihan> daftarTagih
        {
            get
            {
                return GetCollection<DaftarTagihan>("daftarTagih");
            }
        }

        [XafDisplayName("Pembayaran"), ToolTip("Pembayaran")]
        [Association("PaymentNotaList")]
        public XPCollection<NotaPayment> Pembayaran
        {
            get
            {
                return GetCollection<NotaPayment>("Pembayaran");
            }
        }
        //   // 
        //[XafDisplayName("Nota Detail"), ToolTip("Nota Detail")] 
        //[Association("NotaDtlinfo")] 
        //public XPCollection<NotaDtl> NotaDtl
        //{
        //get
        //  {
        //    return GetCollection<NotaDtl>("NotaDtl"); 
        //  }
        //}
        // 

        private double _TotalSM;
        [XafDisplayName("Surat Muat"), ToolTip("Surat Muat")]
        public virtual double TotalSM
        {
            get
            {
                _TotalSM = 0;
                if (!IsLoading && !IsSaving && this.SuratMuats != null)
                {
                    try
                    {
                        if (this.SuratMuats.Count > 0)
                        {
                            _TotalSM = (double)this.SuratMuats.Count;
                        }
                    }
                    catch (Exception e)
                    { }
                }
                return _TotalSM;
                //    return _TotalJml; 
            }
            //  set { SetPropertyValue("TotalJml", ref _TotalJml, value); } 
        }

        private double _TotalJml; 
     [XafDisplayName("Total Jumlah"), ToolTip("Total Jumlah")] 
     public virtual double TotalJml
     { 
       get {
                _TotalJml = 0;
                if (!IsLoading && !IsSaving && this.SuratMuats != null)
                {
                    try
                    {
                        if (this.SuratMuats.Count > 0)
                        {
                            _TotalJml = (double)this.SuratMuats.Sum(x => x.TotalTagihan);
                        }
                    }
                    catch (Exception e)
                    { }
                }
                return _TotalJml;
                //    return _TotalJml; 
            } 
     //  set { SetPropertyValue("TotalJml", ref _TotalJml, value); } 
     } 
     // 
     private double _TotalBiaya; 
     [XafDisplayName("Total Biaya"), ToolTip("Total Biaya")]
     [PersistentAlias("Nett+PPN")]
      public virtual double TotalBiaya
     { 
       get {
                object tempObject = EvaluateAlias(nameof(TotalBiaya));
                if (tempObject != null)
                {
                    return (double)tempObject;
                }
                else
                {
                    return 0;
                    // return _TotalBiayaLain; 
                }
               // return _TotalBiaya; 
            } 

      // set { SetPropertyValue("TotalBiaya", ref _TotalBiaya, value); } 
     } 
     // 
     private double _UangMuka; 
     [XafDisplayName("Uang Muka"), ToolTip("Uang Muka")] 
     public virtual double UangMuka
     { 
       get {
                _UangMuka = 0;
                if (!IsLoading && !IsSaving && this.SuratMuats != null)
                {
                    try
                    {
                        if (this.SuratMuats.Count > 0)
                        {
                            _UangMuka = (double)this.SuratMuats.Sum(x => x.TotalDP);
                        }
                    }
                    catch (Exception e)
                    { }
                }
                return _UangMuka; } 
       //set { SetPropertyValue("UangMuka", ref _UangMuka, value); } 
     } 
     // 
     private double _Lainlain; 
     [XafDisplayName("Lain - Lain"), ToolTip("Lain - Lain")] 
     [ImmediatePostData]
     public virtual double Lainlain
     { 
       get { return _Lainlain; } 
       set { SetPropertyValue("Lainlain", ref _Lainlain, value); } 
     } 
     // 
     private double _Nett; 
     [XafDisplayName("Nett"), ToolTip("Nett")]
     [PersistentAlias("TotalJml-(Lainlain+UangMuka)")]
        public virtual double Nett
     { 
       get {
                object tempObject = EvaluateAlias(nameof(Nett));
                if (tempObject != null)
                {
                    return (double)tempObject;
                }
                else
                {
                    return 0;
                    // return _TotalBiayaLain; 
                }

             //   return _Nett;
             } 
       //set { SetPropertyValue("Nett", ref _Nett, value); } 
     }
        private double _PPN;
        [XafDisplayName("PPN"), ToolTip("PPN")]
        [PersistentAlias("Nett")]
        public virtual double PPN
        {
            get { 
                if (WithPPN)
                {
                    //object tempObject = EvaluateAlias(nameof(PPN));
                    //if (tempObject != null)
                    //{
                    //    _PPN= (double)tempObject * 0.1;
                    //    return _PPN;
                    //}
                    //else
                    //{
                    //    return 0;
                    //    // return _TotalBiayaLain; 
                    //}
                    return Nett * 0.01;
                }
                else
                {
                    return 0;
                }
                //  return _PPN; 
               }
           // set { SetPropertyValue("PPN", ref _PPN, value); }
        }

        private bool _WithPPN;
        [XafDisplayName("PPN"), ToolTip("PPN")]
        [ImmediatePostData]
        //[Size(30)]
        public virtual bool WithPPN
        {
            get { return _WithPPN; }
            set { SetPropertyValue("WithPPN", ref _WithPPN, value); }
        }

        // 
     private string _Note1; 
     [XafDisplayName("Note 1"), ToolTip("Note 1")] 
     [Size(30)] 
     public virtual string Note1
     { 
       get { return _Note1; } 
       set { SetPropertyValue("Note1", ref _Note1, value); } 
     } 
     // 
     private string _Note2; 
     [XafDisplayName("Note 2"), ToolTip("Note 2")] 
     [Size(30)] 
     public virtual string Note2
     { 
       get { return _Note2; } 
       set { SetPropertyValue("Note2", ref _Note2, value); } 
     } 
     // 
     private string _Note3; 
     [XafDisplayName("Note 3"), ToolTip("Note 3")] 
     [Size(30)] 
     public virtual string Note3
     { 
       get { return _Note3; } 
       set { SetPropertyValue("Note3", ref _Note3, value); } 
     }
        private eTagihan _Status;
        [XafDisplayName("Status Sampai/Tidak"), ToolTip("Status Sampai/Tidak")]
        public virtual eTagihan Status
        {
            get {
                if (!IsLoading && !IsSaving)
                {
                    if ( Sisa<=0)
                    {
                        _Status = eTagihan.Lunas;
                    }
                }
                return _Status; }
            set { SetPropertyValue("Status", ref _Status, value); }
        }

        private int _Print;
        [XafDisplayName("Print"), ToolTip("Print")]
        public virtual int Print
        {
            get { return _Print; }
            set { SetPropertyValue("Print", ref _Print, value); }
        }
        /*
         * =================================== payment =============================
         */ 
        private int _PmtPotongan;
        [XafDisplayName("Potongan"), ToolTip("Potongan")]
        [ImmediatePostData]
        public virtual int PmtPotongan
        {
            get { return _PmtPotongan; }
            set { SetPropertyValue("PmtPotongan", ref _PmtPotongan, value); }
        }
        private int _PmtAdjutment;
        [XafDisplayName("Penyesuaian"), ToolTip("Penyesuaian")]
        [ImmediatePostData]
        public virtual int PmtAdjutment
        {
            get { return _PmtAdjutment; }
            set { SetPropertyValue("PmtAdjutment", ref _PmtAdjutment, value); }
        }

        private double _Sisa;
        [XafDisplayName("Sisa"), ToolTip("Sisa")]
        [PersistentAlias("TotalBiaya- (TelahDiTerima +PmtPotongan+PmtAdjutment) ")]
        public virtual double Sisa
        {
            get
            {
                
                _Sisa = 0;
                object tempObject = EvaluateAlias(nameof(Sisa));
                if (tempObject != null)
                {
                    _Sisa= (double)tempObject;

                    if (PmtDiterimaUpdate == false)
                    {
                        PmtDiterimaUpdate = false;
                        PmtDiterima = (int)_Sisa;
                    }
                    return _Sisa;
                }
                else
                {
                    //PmtDiterima = (int)_Sisa;
                    if (PmtDiterimaUpdate == false)
                    {
                        PmtDiterimaUpdate = false;
                        PmtDiterima = (int)_Sisa;
                    }
                    return 0;

                }
                
                //return _Sisa; 
            }
            //set { SetPropertyValue("Sisa", ref _Sisa, value); } 
        }

        private bool PmtDiterimaUpdate ;
        private int _PmtDiterima;
        [XafDisplayName("Diterima"), ToolTip("Diterima")]
        [ImmediatePostData]
        public virtual int PmtDiterima
        {
            get { return _PmtDiterima; }
            set { SetPropertyValue("PmtDiterima", ref _PmtDiterima, value);
                if (!IsLoading && !IsSaving)
                {
                    PmtDiterimaUpdate = true;
                }
                 }
        }

        public void Printed()
        {
            if (Status==eTagihan.Baru)
            {
                Status = eTagihan.Print;
            }
            Print +=  1;
        }
        private string _SysImportNo;
        [XafDisplayName("SysImportNo"), ToolTip("SysImportNo")]
        [Size(50)]
        [IsSearch(true)]
        public virtual string SysImportNo
        {
            get { return _SysImportNo; }
            set
            {

                SetPropertyValue("SysImportNo", ref _SysImportNo, value);
            }
        }


        //[XafDisplayName("Tagihan"), ToolTip("Tagihan")]
        //[Association("NotatagihAssc")]
        //public XPCollection<DaftarTagihan> Tagihan
        //{
        //    get
        //    {
        //        return GetCollection<DaftarTagihan>("Tagihan");
        //    }
        //}
    } 
   
} 
