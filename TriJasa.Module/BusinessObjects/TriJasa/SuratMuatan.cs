// Class Name : SuratMuatan.cs 
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
   [DefaultProperty("DMSMNo")]
   [NavigationItem("Transaksi")]
   
    // Standard Document
    [System.ComponentModel.DisplayName("SuratMuatan")]
    //[RuleCombinationOfPropertiesIsUnique("SuratMuatanRule", DefaultContexts.Save, "NomorSM")]
    public class SuratMuatan : XPObject
   {
     public SuratMuatan() : base()
     {
     // This constructor is used when an object is loaded from a persistent storage.
     // Do not place any code here.
     }
     public SuratMuatan(Session session) : base(session) 
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
        Tanggal = new DateTime(System.DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        TandaTerimaNo = "";
            this.NoTruck = "";
            this.Jenis = "";
            this.JenisBarang = "";
            this.Keterangan = "";
            this.KirimAlamat1 = "";
            this.KirimAlamat2 = "";
            this.KirimAlamat3 = "";
            this.KirimAlamat4 = "";
            this.NomorSM = "";
            HargaBongkar = 0;
            //Pembayaran = ePembayaran.TagihDitujuan;
            Loco = true;
           // NomorSM = Number();

        }
        public string Number()
        {
            string sNumer = "DBL";
            int sRun = 1;
            string sYear = "DBL.H";
            if (NomorDM !=null)
            {
                sNumer = NomorDM.NomorDM;
            }

            //XPCollection<DaftarMuatan> xpDM = new XPCollection<DaftarMuatan>(Session);
            ////string sNumberMax = (string)xpDM.Max(x => x.NomorDM)

            //string sNumberMax = xpDM.
            ////SelectMany(c => c.).
            //Where(o => o.NomorDM.Substring(0, 5) == sYear).
            //Max(o => o.NomorDM);

            //if (sNumberMax != null)
            //{
            //    if (sNumberMax.Length == 9)
            //    {
            //        sNumberMax = sNumberMax.Substring(5, 4);
            //        sRun = System.Convert.ToInt32(sNumberMax) + 1;
            //    }
            //}
            //sNumer = $"DBL.H{sRun.ToString("0000")}";

            return sNumer;
        }

        protected override void OnSaving()
     {
            //NomorSMGet();
            // NomorDM.SMGenerate();
            if (NomorDM != null)
            {
                NomorDM.Save();
            }
           // UpdateJenisBarang(true);
            base.OnSaving();
     }

        protected override void OnLoaded()
        {
            base.OnLoaded();
        }
        protected override void OnDeleting()
     {
       base.OnDeleting();
     }
    
        public void DeleteDetail()
        {

        }
        [Action(Caption = "Truk Sampai Tujuan")]
        public void TrukSudahSampai()
     {
            if (this.TglSpiTj < new DateTime(2020, 1, 1))
            {
                this.TglSpiTj = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }
            else
            {
                this.TglSpiTj = System.DateTime.MinValue;
            }
     }
        [Action(Caption ="SM Kembali")]
        public void SMKembali()
        {
            if (this.TglSJKembali < new DateTime(2020, 1, 1))
            {
                this.TglSJKembali = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }
            else
            {
                this.TglSJKembali = System.DateTime.MinValue;
            }
        }
        public void NomorSMGet()
        {
            if (NomorSM == null || NomorSM == "")
            {
                //if (NomorDM != null)
                //{
                //    int bInt = NomorDM.SuratMuatan.Count();

                //    NomorSM = NomorDM.NomorDM + $" {bInt.ToString("00")}";
                //}
                string sNumberMax = "";
                XPCollection<SuratMuatan> xpSM = new XPCollection<SuratMuatan>(new BinaryOperator("NomorDM", this.NomorDM));
                try
                {
                   sNumberMax = xpSM.   
                   Max(o => o.NomorSM.Trim());
                }
                catch (Exception e)
                {

                }

                int sRun = 1;
                if (sNumberMax != null)
                {
                    if (sNumberMax.Length == 3)
                    {
                        //sNumberMax = sNumberMax.Substring(5, 4);
                        sRun = System.Convert.ToInt32(sNumberMax) + 1;
                    }
                }

                NomorSM = $"{sRun.ToString("000")}";

            }
        }
        // 

    private string _SMNoFull;
    [XafDisplayName("Nomor SM"), ToolTip("Total Ongkos Lain-Lain")]
    //[PersistentAlias("NomorDM.NomorDM+'/'+")]
    [Size(24)]
    [IsHide(true)]
    public virtual string SMNoFull
    {
        get
        {
                _SMNoFull = "";
                string noDM="";
                string noSM = "";
                if (!IsLoading && !IsSaving)
                {
                    if (NomorSM == "")
                    {
                        if (NomorDM != null)
                        {
                            string sMaxNo = NomorDM.SuratMuatan.Max(x => x.NomorSM);
                            int i = 1;
                            if (sMaxNo != null && sMaxNo != "")
                            {
                                i = int.Parse(sMaxNo) + 1;
                                NomorSM = $"{i.ToString("000")}";
                            }
                            else
                            {
                                NomorSM = $"{i.ToString("000")}";
                            }
                        }

                        }
                        if (NomorDM != null)
                    {
                        noDM = NomorDM.NomorDM;
                        /// NoTruck = NomorTruk.NomorTruck;
                    }
                    if ( NomorSM!=null )
                    {
                        noSM = NomorSM;
                    }

                    _SMNoFull = $"{noDM.Trim()} / {noSM.Trim()} / {TandaTerimaNo.Trim()}";
                }

                //object tempObject = EvaluateAlias(nameof(SMNoFull));
                //if (tempObject != null)
                //{
                //    return (string)tempObject;
                //}
                //else
                //{
                //    return 0;
                //    // return _TotalBiayaLain; 
                //}
                return _SMNoFull;
            }
            //set { SetPropertyValue("TotalOngkosLain", ref _TotalOngkosLain, value); }
        }
        private string _DMSMNo;
        [XafDisplayName("Nomor DM SM"), ToolTip("Total Ongkos Lain-Lain")]
        //[PersistentAlias("NomorDM.NomorDM+'/'+")]
        [Size(24)]
        [IsHide(true)]
        public virtual string DMSMNo
        {
            get
            {
                _DMSMNo = "";
                string noDM = "";
                string noSM = "";
                if (!IsLoading && !IsSaving)
                {
                    if (NomorDM != null)
                    {
                        noDM = NomorDM.NomorDM;
                        /// NoTruck = NomorTruk.NomorTruck;
                    }
                    if (NomorSM != null)
                    {
                        noSM = NomorSM;
                    }

                    _DMSMNo = $"{noDM.Trim()} / {noSM.Trim()}";
                }

                //object tempObject = EvaluateAlias(nameof(SMNoFull));
                //if (tempObject != null)
                //{
                //    return (string)tempObject;
                //}
                //else
                //{
                //    return 0;
                //    // return _TotalBiayaLain; 
                //}
                return _DMSMNo;
            }
            //set { SetPropertyValue("TotalOngkosLain", ref _TotalOngkosLain, value); }
        }
        private string _NomorSM; 
     [XafDisplayName("Nomor"), ToolTip("Nomor")] 
     [Size(4)]
     [Appearance("SuratMuatNomorSM", Enabled = false)]
     public virtual string NomorSM
     { 
       get {
                if (!IsLoading && !IsSaving)
                {
                    if ( _NomorSM == null)
                    {
                        //if (NomorDM != null)
                        //{
                        //    NomorSMGet();
                        //}
                        // NomorSMGet();
                        _NomorSM = "";
                    }
                }
                return _NomorSM; } 
       set { SetPropertyValue("NomorSM", ref _NomorSM, value); } 
     } 
     // 
     private Truck _NomorTruk; 
     [XafDisplayName("Truck"), ToolTip("Truck")] 
     [ImmediatePostData]
     [IsHide(true)]
        public virtual Truck NomorTruk
     { 
       get {
                if (!IsLoading && !IsSaving)
                {
                    if (NomorDM != null)
                    {
                        if (NomorDM.NoTruck != null)
                        {
                            NomorTruk = NomorDM.NoTruck;
                            Tanggal = NomorDM.Tanggal;
                        }
                       /// NoTruck = NomorTruk.NomorTruck;
                    }
                }

                return _NomorTruk; } 
                set {   
                try
                {
               
                    SetPropertyValue("NomorTruk", ref _NomorTruk, value);
                }
                catch ( Exception e)
                {

                }
            } 
     } 
     // 
     private Pelanggan _Pelanggan; 
     [XafDisplayName("Pelanggan"), ToolTip("Pelanggan")]
        [RuleRequiredField(DefaultContexts.Save)]
        [ImmediatePostData]

     public virtual Pelanggan Pelanggan
     { 
       get { return _Pelanggan; } 
       set {
                if (!IsSaving && !IsLoading && value != null)
                {
                    if (_Pelanggan != value)
                    {
                        PelangganAlamat1 = value.PelangganAlamat1;
                        PelangganAlamat2 = value.PelangganAlamat2;
                        PelangganAlamat3 = value.PelangganAlamat3;
                        PelangganAlamat4 = value.PelangganAlamat4;
                        Term = value.Term;


                    }
                }

                SetPropertyValue("Pelanggan", ref _Pelanggan, value);
                if (!IsSaving && !IsLoading && value != null)
                {
                    string sqlQuery = string.Format($" TarifSM.pelanggan.Oid == {value.Oid} && Berlaku <= #{Tanggal}# ");
                    CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);
                    TarifSMHarga oiHarga = Session.FindObject<TarifSMHarga>(filterOperator);
                    TarifHarga = oiHarga;
                }
            } 
     } 
     // 
     private PelangganTerima _Dikerimke; 
     [XafDisplayName("DIKIRIM KE"), ToolTip("DIKIRIM KE")]
        [RuleRequiredField(DefaultContexts.Save)]
        [ImmediatePostData]
        public virtual PelangganTerima Dikerimke
     { 
       get { return _Dikerimke; } 
       set {
                if (!IsSaving && !IsLoading && value != null)
                {
                    if (_Dikerimke != value)
                    {
                        KirimAlamat1 = value.PelangganAlamat1;
                        KirimAlamat2 = value.PelangganAlamat2;
                        KirimAlamat3 = value.PelangganAlamat3;
                        KirimAlamat4 = value.PelangganAlamat4;
                    }
                }

                SetPropertyValue("Dikerimke", ref _Dikerimke, value);

                
            } 
     }
        // 
    private Nota _NotaSuratMuat;
    [XafDisplayName("No Nota"), ToolTip("No Nota")]
    [Association("NotaSMinfo")]
    [ImmediatePostData]
    public virtual Nota NotaSuratMuat
        {
        get { return _NotaSuratMuat; }
        set { SetPropertyValue("NotaSuratMuat", ref _NotaSuratMuat, value);

            }
    }

    private DateTime _Tanggal; 
     [XafDisplayName("Tanggal"), ToolTip("Tanggal")]
        [RuleRequiredField(DefaultContexts.Save)]
        public virtual DateTime Tanggal
     { 
       get { return _Tanggal; } 
       set { SetPropertyValue("Tanggal", ref _Tanggal, value); } 
     }



        // 
        private string  _TandaTerimaNo; 
     [XafDisplayName("Tanda Terima No"), ToolTip("Tanda Terima No")]
        //[Custom("EditMask", @"[A-Z0-9]{1,12}")]
        //[Custom("EditMaskType", "RegEx")]
        [RuleRequiredField(DefaultContexts.Save)]
      [ImmediatePostData]
        //[IsSearch(true)]
        public virtual string TandaTerimaNo
     { 
       get { return _TandaTerimaNo; } 
       set {
                if (!IsLoading && !IsSaving )
                {
                    if (value != "")
                    {
                        value = value.ToUpper();
                    }
                }
                 SetPropertyValue("TandaTerimaNo", ref _TandaTerimaNo, value);

                if (!IsLoading && !IsSaving )
                {
                    if (value != "")
                    {
                        string sqlQuery = string.Format(" NoTTB == '{0}' ", value);
                        CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);
                        PenerimaanKasir oPenerimaanKas = Session.FindObject<PenerimaanKasir>(filterOperator);
                        if (oPenerimaanKas != null)
                        {
                            if (Pelanggan != null)
                            {
                                oPenerimaanKas.Pengirim = Pelanggan;
                            }
                            else if (oPenerimaanKas.Pengirim != null)
                            {
                                Pelanggan = oPenerimaanKas.Pengirim;
                            }

                            TerimaKasir = oPenerimaanKas;
                            XPCollection<PenerimaanKasir> oxpPenerimaanKas;
                            oxpPenerimaanKas = new XPCollection<PenerimaanKasir>(Session) { Criteria = filterOperator };
                            double  intDP =  oxpPenerimaanKas.Sum(x => x.JumlahAlokasi);
                            TotalDP = intDP;

                           // TotalDP = (oPenerimaanKas.JumlahTerima - oPenerimaanKas.JumlahAlokasi);

                            if (oPenerimaanKas.Lunas)
                            {
                                Lunas = true;
                            }
                            else
                            {
                                Franco = true;
                            }
                        }
                        else
                        {
                            TerimaKasir = null;
                            TotalDP = 0;
                            Loco = true;

                        }
                    }
                }
            } 
     } 
     // 
     private int _Term; 
     [XafDisplayName("Term (Hari)"), ToolTip("Term (Hari)")] 
     [Size(10)] 
     public virtual int Term
     { 
       get { return _Term; } 
       set { SetPropertyValue("Term", ref _Term, value); } 
     } 
     // 
     private ePembayaran _Pembayaran; 
     [XafDisplayName("Pembayaran"), ToolTip("Pembayaran")] 
     //[ImmediatePostData]
     public virtual ePembayaran Pembayaran
     { 
       get {
                if (!IsLoading && !IsSaving)
                {
                    ////if ( Loco==false && Franco==false && Lunas==false)
                    ////{
                    //    if (_Pembayaran==ePembayaran.TagihDitujuan)
                    //    {
                    //        // _Pembayaran = ePembayaran.TagihDitujuan;
                    //        Loco = true;
                    //    }
                    //   else if (_Pembayaran == ePembayaran.Franco)
                    //    {
                    //        // _Pembayaran = ePembayaran.TagihDitujuan;
                    //        Franco = true;
                    //    }
                    //    else if (_Pembayaran == ePembayaran.Lunas)
                    //    {
                    //        // _Pembayaran = ePembayaran.TagihDitujuan;
                    //        Lunas = true;
                    //    }

                    //}
                    if ( Loco )
                    {
                        _Pembayaran = ePembayaran.TagihDitujuan;
                    }
                    else if (Franco)
                    {
                        _Pembayaran = ePembayaran.Franco;
                    }
                    else if (Lunas)
                    {
                        _Pembayaran = ePembayaran.Lunas;
                    }


                }
                return _Pembayaran; } 
       //set { SetPropertyValue("Pembayaran", ref _Pembayaran, value); } 
     } 
     // 
     private string _NoTruck; 
     [XafDisplayName("Nomor Truck"), ToolTip("Nomor Truck")] 
     [Size(20)] 
     public virtual string NoTruck
     { 
       get {
                return _NoTruck; } 
       set { SetPropertyValue("NoTruck", ref _NoTruck, value); } 
     } 
     // 
     private DateTime _TglSpiTj;
        [ModelDefault("DisplayFormat", "{0: dd/MM/yyyy}")]

        [XafDisplayName("Tgl Truck Sampai di Tujuan"), ToolTip("Tgl Truck Sampai di Tujuan")] 
     public virtual DateTime TglSpiTj
     { 
       get { return _TglSpiTj; } 
       set { SetPropertyValue("TglSpiTj", ref _TglSpiTj, value); } 
     } 
     // 
     private DateTime _TglSJKembali;
        [ModelDefault("DisplayFormat", "{0: dd/MM/yyyy}")]

        [XafDisplayName("Tgl Surat Jalan Kembali"), ToolTip("Tgl Surat Jalan Kembali")] 
     public virtual DateTime TglSJKembali
     { 
       get { return _TglSJKembali; } 
       set { SetPropertyValue("TglSJKembali", ref _TglSJKembali, value); } 
     } 
     // 
     private string _PelangganAlamat1; 
     [XafDisplayName("PelangganAlamat1"), ToolTip("PelangganAlamat1")] 
     [Size(150)] 
     public virtual string PelangganAlamat1
     { 
       get { return _PelangganAlamat1; } 
       set { SetPropertyValue("PelangganAlamat1", ref _PelangganAlamat1, value); } 
     } 
     // 
     private string _PelangganAlamat2; 
     [XafDisplayName("PelangganAlamat2"), ToolTip("PelangganAlamat2")] 
     [Size(150)] 
     public virtual string PelangganAlamat2
     { 
       get { return _PelangganAlamat2; } 
       set { SetPropertyValue("PelangganAlamat2", ref _PelangganAlamat2, value); } 
     } 
     // 
     private string _PelangganAlamat3; 
     [XafDisplayName("PelangganAlamat3"), ToolTip("PelangganAlamat3")] 
     [Size(150)] 
     public virtual string PelangganAlamat3
     { 
       get { return _PelangganAlamat3; } 
       set { SetPropertyValue("PelangganAlamat3", ref _PelangganAlamat3, value); } 
     } 
     // 
     private string _PelangganAlamat4; 
     [XafDisplayName("PelangganAlamat4"), ToolTip("PelangganAlamat4")] 
     [Size(150)] 
     public virtual string PelangganAlamat4
     { 
       get { return _PelangganAlamat4; } 
       set { SetPropertyValue("PelangganAlamat4", ref _PelangganAlamat4, value); } 
     } 
     // 
     private string _KirimAlamat1; 
     [XafDisplayName("Dikirim Ke 1"), ToolTip("Dikirim Ke 1")] 
     [Size(150)] 
     public virtual string KirimAlamat1
     { 
       get { return _KirimAlamat1; } 
       set { SetPropertyValue("KirimAlamat1", ref _KirimAlamat1, value); } 
     } 
     // 
     private string _KirimAlamat2; 
     [XafDisplayName("KirimAlamat2"), ToolTip("KirimAlamat2")] 
     [Size(150)] 
     public virtual string KirimAlamat2
     { 
       get { return _KirimAlamat2; } 
       set { SetPropertyValue("KirimAlamat2", ref _KirimAlamat2, value); } 
     } 
     // 
     private string _KirimAlamat3; 
     [XafDisplayName("KirimAlamat3"), ToolTip("KirimAlamat3")] 
     [Size(150)] 
     public virtual string KirimAlamat3
     { 
       get { return _KirimAlamat3; } 
       set { SetPropertyValue("KirimAlamat3", ref _KirimAlamat3, value); } 
     } 
     // 
     private string _KirimAlamat4; 
     [XafDisplayName("KirimAlamat4"), ToolTip("KirimAlamat4")] 
     [Size(150)] 
     public virtual string KirimAlamat4
     { 
       get { return _KirimAlamat4; } 
       set { SetPropertyValue("KirimAlamat4", ref _KirimAlamat4, value); } 
     } 
     // 
     private string _Keterangan; 
     [XafDisplayName("Keterangan Barang"), ToolTip("Keterangan Barang")] 
     [Size(1000)] 
     public virtual string Keterangan
     { 
       get { return _Keterangan; } 
       set { SetPropertyValue("Keterangan", ref _Keterangan, value); } 
     } 
     // 
     [XafDisplayName("Surat Muat Detail"), ToolTip("Surat Muat Detail")] 
     [Association("SM")]
        [DevExpress.Xpo.Aggregated]
        [IsHide(true)]
        public XPCollection<SuratMuatDtl> SuratMuatDtl
     {
     get
       {
         return GetCollection<SuratMuatDtl>("SuratMuatDtl"); 
       }
     }
     // 
     private string _Jenis; 
     [XafDisplayName("Jenis Barang"), ToolTip("Jenis Barang")] 
     [Size(45)] 
     public virtual string Jenis
     { 
       get { return _Jenis; } 
       set { SetPropertyValue("Jenis", ref _Jenis, value); } 
     } 
     // 
     private double _Kuantum; 
     [XafDisplayName("Kuantum"), ToolTip("Kuantum")] 
     public virtual double Kuantum
     { 
       get { return _Kuantum; } 
       set { SetPropertyValue("Kuantum", ref _Kuantum, value); } 
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
     //  
     private double _TotalUnit; 
     [XafDisplayName("Total"), ToolTip("Total")] 
     public virtual double TotalUnit
     { 
       get { return _TotalUnit; } 
       set { SetPropertyValue("TotalUnit", ref _TotalUnit, value); } 
     } 
     // 
     private double _TotalKg; 
     [XafDisplayName("Total Kg"), ToolTip("Total Kg")]
        //[ImmediatePostData]
        public virtual double TotalKg
     { 
       get { return _TotalKg; } 
       set { SetPropertyValue("TotalKg", ref _TotalKg, value); } 
     } 
     // 
     private double _TotalM3; 
     [XafDisplayName("Total M3"), ToolTip("Total M3")] 
     public virtual double TotalM3
     { 
       get { return _TotalM3; } 
       set { SetPropertyValue("TotalM3", ref _TotalM3, value); } 
     } 


        private double _TotalJml;
        [XafDisplayName("Total Jml"), ToolTip("Total Jml")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        [IsHide(true)]
        [ImmediatePostData]
        public virtual double TotalJml
        {
            get
            {
                _TotalJml = 0;
                if (!IsLoading && !IsSaving && this.SuratMuatDtl != null)
                {
                    try
                    {
                        if (this.SuratMuatDtl.Count > 0)
                        {
                            _TotalJml = (double)this.SuratMuatDtl.Sum(x => x.Jumlah);
                           //TotalTagihan = _TotalJml;
                        }
                    }
                    catch (Exception e)
                    { }
                }
                return _TotalJml+ HargaBongkar;
            }
            //set { SetPropertyValue("TotalJmlOrg", ref _TotalJmlOrg, value); }
        }

        private double _TotalJmlOrgInDB;
        [XafDisplayName("Uang Muka"), ToolTip("Uang Muka")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        public virtual double TotalJmlOrgInDB
        {
            get { return _TotalJmlOrgInDB; }
            set { SetPropertyValue("TotalJmlOrgInDB", ref _TotalJmlOrgInDB, value); }
        }

        private double  _TotalJmlOrg;
      
        //[Persistent("TotalJmlOrg")]
        //[PersistentAlias(nameof(TotalJmlOrg))]
        [XafDisplayName("Total Jumlah org"), ToolTip("Total Jumlah Org")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        [IsHide(true)]
        [ImmediatePostData]

        public virtual double  TotalJmlOrg
        {
        get {
                _TotalJmlOrg = 0;
                if (!IsLoading && !IsSaving && this.SuratMuatDtl != null)
                {
                    try
                    {
                        if (this.SuratMuatDtl.Count > 0)
                        {
                            _TotalJmlOrg = (double)this.SuratMuatDtl.Sum(x => x.JumlahOriginal);
                            if ( TerimaKasir !=null)
                            {
                                 if (TotalDP >= _TotalJmlOrg)
                                    {
                                    TotalDP =(double) _TotalJmlOrg;

                                    if ( TerimaKasir.Lunas )
                                    {
                                        Lunas = true;
                                    }
                                    //Pembayaran = ePembayaran.Lunas;

                                }
                                //if (_TotalJmlOrg <= TerimaKasir.JumlahTerima)
                                //{
                                //    Pembayaran = ePembayaran.Lunas;
                                //}
                            }
            
                            TotalTagihan =(double) _TotalJmlOrg+ (double)HargaBongkar;
                        }
                    }
                    catch (Exception e)
                    { }
                }
                _TotalJmlOrg += (double)HargaBongkar;
                TotalJmlOrgInDB = _TotalJmlOrg;
                return _TotalJmlOrg; 
            }
        //set { SetPropertyValue("TotalJmlOrg", ref _TotalJmlOrg, value); }
    }
        // 
     private double _TotalDP; 
     [XafDisplayName("Uang Muka"), ToolTip("Uang Muka")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        public virtual double TotalDP
     { 
       get { return _TotalDP; } 
       set { SetPropertyValue("TotalDP", ref _TotalDP, value); } 
     }

    private double _TotalTagihan;
    [XafDisplayName("Total Tagihan"), ToolTip("Total Tagihan")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        //[ImmediatePostData]
        public virtual double TotalTagihan
        {
        get { return _TotalTagihan; }
        set { SetPropertyValue("TotalTagihan", ref _TotalTagihan, value); }
    }
        private double _TotalPembayaran;
        [XafDisplayName("Total Pembayaran"), ToolTip("Total Pembayaran")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        //[ImmediatePostData]
        public virtual double TotalPembayaran
        {
            get { return _TotalPembayaran; }
            set { SetPropertyValue("TotalPembayaran", ref _TotalPembayaran, value); }
        }


        // 
      private DaftarMuatan _NomorDM; 
     [XafDisplayName("DaftarMuat"), ToolTip("DaftarMuat")]
        [Association("DM")]
        [DevExpress.Xpo.Aggregated]
        //[ImmediatePostData]
        public virtual DaftarMuatan NomorDM
        {
            get {

                return _NomorDM; } 
       set {
                SetPropertyValue("NomorDM", ref _NomorDM, value);
            }
        }

        private double  _KomisiTotal=0;
        [XafDisplayName("Total Komisi"), ToolTip("Total Komisi")]
        [PersistentAlias("(TotalJmlOrg-TotalJml) ")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        public virtual double  KomisiTotal
        {
            get
            {

                _KomisiTotal = 0;
                object tempObject = EvaluateAlias(nameof(KomisiTotal));
                if (tempObject != null)
                {
                    _KomisiTotal = (double)tempObject;


                    return _KomisiTotal;
                }
                else
                {

                    return 0;

                }

                //return _Sisa; 
            }
            //set { SetPropertyValue("Sisa", ref _Sisa, value); } 
        }

        private double ? _KomisiDibayar=0;
        [XafDisplayName("Komisi Dibayar"), ToolTip("Komisi Dibayar")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        public virtual double ? KomisiDibayar
        {
            get {


                _KomisiDibayar = 0;
                if (!IsLoading && !IsSaving)
                {
                    CriteriaOperator oCriteria = null;
                    oCriteria = GroupOperator.And(oCriteria, new BinaryOperator("SM", this));
                    XPCollection<SMKomisiItem> oKomisiItem = new XPCollection<SMKomisiItem>(Session, oCriteria);

                    if (oKomisiItem.Count > 0)
                    {
                        _KomisiDibayar = oKomisiItem.Sum(x => (x.Dibayarkan));
                    }

                }

                return _KomisiDibayar;

            }
          //  set { SetPropertyValue("KomisiDibayar", ref _KomisiDibayar, value); }
        }

        private double  _Komisi;
        [XafDisplayName("Komisi"), ToolTip("Komisi")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        [ImmediatePostData]
        public virtual double  Komisi
        {
            get { return _Komisi; }
            set { SetPropertyValue("Komisi", ref _Komisi, value); 

                }
        }
     

        private double  _KomisiSisa=0;
        [XafDisplayName("Sisa Komisi"), ToolTip("Sisa Komisi")]
        [PersistentAlias("KomisiTotal- (KomisiDibayar + Komisi) ")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        //[Persistent("KomisiSisa")]
        public virtual double  KomisiSisa
        {
            get
            {

                _KomisiSisa = 0;
                object tempObject = EvaluateAlias(nameof(KomisiSisa));
                if (tempObject != null)
                {
                    _KomisiSisa = (double)tempObject;

                    //KomisiSisaSave = _KomisiSisa;
                    return _KomisiSisa;
                }
                else
                {
                    //KomisiSisaSave = 0;
                    return 0;

                }

                //return _Sisa; 
            }
            //set { SetPropertyValue("Sisa", ref _Sisa, value); } 
        }
        // 
        private double _Colly; 
        [XafDisplayName("Colly"), ToolTip("Colly")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        [IsHide(true)]
        public virtual double Colly
     { 
       get {
                _Colly = 0;
                if (!IsLoading && !IsSaving && this.SuratMuatDtl != null)
                {
                    try
                    {
                        if (this.SuratMuatDtl.Count > 0)
                        {
                            _Colly = (double)this.SuratMuatDtl.Sum(x => x.Kuantum);
                        }

                        if (NomorSM == "" || NomorSM==null)
                        {
                            if (NomorDM != null)
                            {
                                string sMaxNo = NomorDM.SuratMuatan.Max(x => x.NomorSM);
                                int i = 1;
                                if (sMaxNo != null && sMaxNo != "")
                                {
                                    i = int.Parse(sMaxNo) + 1;
                                    NomorSM = $"{i.ToString("000")}";
                                }
                                else
                                {
                                    NomorSM = $"{i.ToString("000")}";
                                }
                            }

                        }
                    }
                    catch (Exception e)
                    { }
                }
                return _Colly; 
            } 
       //set { SetPropertyValue("Colly", ref _Colly, value); } 
     } 

     // 
     private string _JenisBarang; 
     [XafDisplayName("Macam Barang"), ToolTip("Macam Barang")] 
     [Size(1000)] 
     public virtual string JenisBarang
     { 
       get { return _JenisBarang; } 
       set {
                //string oldJenisBarang = _JenisBarang;
                //bool modified = SetPropertyValue("JenisBarang", ref _JenisBarang, value);
                //if (!IsLoading && !IsSaving && oldJenisBarang != _JenisBarang && modified)
                //{

                //}
                SetPropertyValue("JenisBarang", ref _JenisBarang, value);
            } 
     }

        private string oJenisBarang;
        
        public void UpdateJenisBarang(bool forceChangeEvents)
        {
            oJenisBarang = "";
            foreach (SuratMuatDtl detail in SuratMuatDtl)
            {
                oJenisBarang +=  detail.JenisBarang + ",";
            }
            if (forceChangeEvents)
            {
                //OnChanged(nameof(JenisBarang), _JenisBarang, oJenisBarang);
                SetPropertyValue("JenisBarang", ref _JenisBarang, oJenisBarang);
                SetPropertyValue("Keterangan", ref _Keterangan, oJenisBarang);

            }
        }
        private PenerimaanKasir _TerimaKasir;
        [XafDisplayName("Terima Kasir"), ToolTip("Terima Kasir")]
        public virtual PenerimaanKasir TerimaKasir
        {
            get { return _TerimaKasir; }
            set { SetPropertyValue("TerimaKasir", ref _TerimaKasir, value); }
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

        public void UpdateTotalJumlah(bool forceChangeEvents)
        {
            double? oldTotalJmll = _TotalJml;
            double tempTotalJml = 0;
            foreach (SuratMuatDtl detail in SuratMuatDtl)
                tempTotalJml += detail.Jumlah;
            //tempTotalJml=(double)SuratMuatDtl.Sum(x => x.Jumlah);
            _TotalJml = tempTotalJml;
            if (forceChangeEvents)
            {
                OnChanged(nameof(TotalJml), oldTotalJmll, _TotalJml);
                if (NomorDM != null)
                {
                    NomorDM.UpdateTotalJumlah(true);
                    UpdateJenisBarang(true);
                }
            }
        }
        public void SyncSMDtlX ()
        {
            //Colly = (double)SuratMuatDtl.Sum(x => x.Kuantum);
            Kuantum = (double)SuratMuatDtl.Sum(x=> x.Kuantum);
            //TotalJml = (double)SuratMuatDtl.Sum(x => x.Jumlah);
            //TotalJmlOrg = (double)SuratMuatDtl.Sum(x => x.JumlahOriginal);
            TotalKg = (double)SuratMuatDtl.Sum(x => x.Kg);
            TotalM3 = (double)SuratMuatDtl.Sum(x => x.VolM3);
            
            //foreach ( SuratMuatDtl SMD in SuratMuatDtl)
            //{

            //}
        }

        private bool _Loco;
        [XafDisplayName("Loco"), ToolTip("Loco")]
        [ImmediatePostData]
        public virtual bool Loco
        {
            get { return _Loco; }
            set { SetPropertyValue("Loco", ref _Loco, value);
                if (!IsLoading && !IsSaving)
                {
                    if (value == true)
                    {
                        Franco = false;
                        Lunas = false;
                        //Pembayaran = ePembayaran.TagihDitujuan;
                    }
                }
                }
        }

        private bool _Franco;
        [XafDisplayName("Franco"), ToolTip("Franco")]
        [ImmediatePostData]
        public virtual bool Franco
        {
            get { return _Franco; }
            set { SetPropertyValue("Franco", ref _Franco, value);
                if (!IsLoading && !IsSaving)
                {
                    if (value == true)
                    {
                        Loco = false;
                        Lunas = false;
                        //Pembayaran = ePembayaran.Franco;
                    }
                }
            }
        }

        private bool _Lunas;
        [XafDisplayName("Lunas"), ToolTip("Lunas")]
        [ImmediatePostData]
        public virtual bool Lunas
        {
            get { return _Lunas; }
            set { SetPropertyValue("Lunas", ref _Lunas, value);
                if (!IsLoading && !IsSaving)
                {
                    if (value == true)
                    {
                        Loco = false;
                        Franco = false;
                        //Pembayaran = ePembayaran.Lunas;
                    }
                }
            }
        }

      /*

        private SMKomisi _KomisiPayment;
        [XafDisplayName("Komisi Bayar"), ToolTip("Komisi Bayar")]
        [Association("SMKomisiList")]
        public virtual SMKomisi KomisiPayment
        {
            get { return _KomisiPayment; }
            set
            {
                SetPropertyValue("KomisiPayment", ref _KomisiPayment, value);
            }
        }
      */
        private TarifSMHarga _TarifHarga;
        [XafDisplayName("Tarif Harga"), ToolTip("Tarif Harga")]
        public virtual TarifSMHarga TarifHarga
        {
            get { return _TarifHarga; }
            set { SetPropertyValue("TarifHarga", ref _TarifHarga, value); }
        }

        private int _HargaBongkar;
        [XafDisplayName("Harga Bongkar"), ToolTip("Harga Bongkar")]
        public virtual int HargaBongkar
        {
            get { return _HargaBongkar; }
            set { SetPropertyValue("HargaBongkar", ref _HargaBongkar, value); }
        }

        private Pajak _PPNPelanggan;
        [XafDisplayName("PPN Pelanggan"), ToolTip("PPN Pelanggan")]
        public virtual Pajak PPNPelanggan
        {
            get { return _PPNPelanggan; }
            set { SetPropertyValue("PPNPelanggan", ref _PPNPelanggan, value); }
        }

        private Pajak _PPHPelanggan;
        [XafDisplayName("PPH Pelanggan"), ToolTip("PPH Pelanggan")]
        public virtual Pajak PPHPelanggan
        {
            get { return _PPHPelanggan; }
            set { SetPropertyValue("PPHPelanggan", ref _PPHPelanggan, value); }
        }

        private Pajak _PPNSuplier;
        [XafDisplayName("PPN Suplier"), ToolTip("PPNSuplier")]
        public virtual Pajak PPNSuplier
        {
            get { return _PPNSuplier; }
            set { SetPropertyValue("PPNSuplier", ref _PPNSuplier, value); }
        }

        private Pajak _PPHSuplier;
        [XafDisplayName("PPH Suplier"), ToolTip("PPH Suplier")]
        public virtual Pajak PPHSuplier
        {
            get { return _PPHSuplier; }
            set { SetPropertyValue("PPHSuplier", ref _PPHSuplier, value); }
        }

        private double _PajakPelanggan;
        [XafDisplayName("Pajak Pelanggan"), ToolTip("Pajak Pelanggan")]
        public virtual double PajakPelanggan
        {
            get { return _PajakPelanggan; }
            set { SetPropertyValue("PajakPelanggan", ref _PajakPelanggan, value); }
        }
        private double _PajakSupplier;
        [XafDisplayName("Pajak Supplier"), ToolTip("Pajak Supplier")]
        public virtual double PajakSupplier
        {
            get { return _PajakSupplier; }
            set { SetPropertyValue("PajakSupplier", ref _PajakSupplier, value); }
        }

    }


    public enum ePembayaran
    { 
     [XafDisplayName("Loco")]
     TagihDitujuan = 1, 
     [XafDisplayName("Franco")]
     Franco = 2 , 
     [XafDisplayName("Lunas")]
     Lunas = 3 
      } 
} 
