// Class Name : DaftarMuatan.cs 
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
using DevExpress.ExpressApp.Model;

namespace TriJasa.Module.BusinessObjects
{ 
   [DefaultClassOptions] 
   [ImageName("ModelEditor_Views")] 
   [DefaultProperty("NomorDM")]
   [NavigationItem("Transaksi")]
   // Standard Document
   [System.ComponentModel.DisplayName("DaftarMuatan")]
    [RuleCombinationOfPropertiesIsUnique("DaftarMuatanRule", DefaultContexts.Save, "NomorDM")]
    //[PropertyEditor(typeof(Double), EditorAliases.d, true)]
    public class DaftarMuatan : XPObject
   {
     public DaftarMuatan() : base()
     {
     // This constructor is used when an object is loaded from a persistent storage.
     // Do not place any code here.
     }
     public DaftarMuatan(Session session) : base(session) 
     {
       // This constructor is used when an object is loaded from a persistent storage.
       // Do not place any code here.
     }
     public override void AfterConstruction()
     {
       base.AfterConstruction();
       string tUser = SecuritySystem.CurrentUserName.ToString();
       Tanggal = new DateTime(System.DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
       Komisi = 0.1;
       NomorDM = Number(Tanggal);
            //}


        }

        public void SMGenerate()
        {
            try
            {
                int i = 1;
                bool bfind = true;

                    // while (bfind)
                    //{
                    //    bfind = false;
                    //    foreach (SuratMuatan sm in this.SuratMuatan)
                    //    {
                    //        if (sm.Pelanggan == null)
                    //        {
                    //            sm.Delete();
                    //            sm.Save();
                    //            bfind = true;
                    //            break;
                    //        }
                    //    }
                    //}
               foreach (SuratMuatan sm in this.SuratMuatan)
                {
                    // sm.Pelanggan==null 
                    if (sm.Pelanggan != null && sm.Dikerimke != null)
                    {
                        if (sm.NomorSM == "" || sm.NomorSM == null)
                        {
                            sm.NomorSM = $"{i.ToString("000")}";
                            // sm.Save();
                        }
                    }
                    i++;
                }
            }
            catch (Exception e)
            {

            }
        }
        public virtual string Number(DateTime sDate)
        {
            //TAHUN +BULAN + NOMOR URUT / NOMOR URUT SM / TTB
            //  DBL.2102.001 / 012 / TANDATERIMA

            string sNumer = "";
            int sRun = 1;
            byte iYear = (byte)((sDate.Year - 2014) + 65);
            string sYear = $"DBL.{sDate.ToString("yyMM")}";
            XPCollection<DaftarMuatan> xpDM = new XPCollection<DaftarMuatan>(Session);
            //string sNumberMax = (string)xpDM.Max(x => x.NomorDM)
            string sNumberMax = "";
            try
            {
                sNumberMax = xpDM
                    //SelectMany(c => c.).
               .Where(o => o.NomorDM.Substring(0, 8).ToUpper().Trim() == sYear.ToUpper().Trim())
               .Max(o => o.NomorDM.Trim());
            }
            catch (Exception e)
            {
                sNumberMax = "";
            }
            if (sNumberMax != null)
            {
                if (sNumberMax.Length == 12)
                {
                    sNumberMax = sNumberMax.Substring(9, 3);
                    sRun = System.Convert.ToInt32(sNumberMax) + 1;
                }
            }

            sNumer = $"{sYear}.{sRun.ToString("000")}";

            return sNumer;
        }
        public string Numberold(DateTime sDate)
        {
            string sNumer = "";
            int sRun = 1;
            byte iYear = (byte)((sDate.Year - 2014) + 65);
            string cYear = Encoding.ASCII.GetString(new byte[] { iYear }).ToString().Trim();
            string sYear = $"DBL.{cYear}";
            XPCollection<DaftarMuatan> xpDM = new XPCollection<DaftarMuatan>(Session);
            //string sNumberMax = (string)xpDM.Max(x => x.NomorDM)
            string sNumberMax = "";
            try
            {
                 sNumberMax = xpDM.
                //SelectMany(c => c.).
                //Where(o => o.NomorDM.Substring(0, 5).ToUpper().Trim() == sYear.ToUpper().Trim()).
                Max(o => o.NomorDM.Trim());
            }
            catch (Exception e)
            {

            }
            if(sNumberMax !=null)
            {
                if (sNumberMax.Length==9)
                {
                    sNumberMax= sNumberMax.Substring(5, 4);
                    sRun = System.Convert.ToInt32(sNumberMax)+1;
                }
            }

            sNumer = $"DBL.{cYear}{sRun.ToString("0000")}";

            return sNumer;
        }
     protected override void OnSaving()
     {
            if (NomorDM == "" || NomorDM ==null)
            {
                NomorDM = Number(Tanggal);
            }
            //SMGenerate();
            base.OnSaving();
     } 
     protected override void OnDeleting()
     {
       base.OnDeleting();
     }
        //protected override void OnLoaded()
        //{
        //    base.OnLoaded();
        //}
        // 
      private string _NomorDM; 
     [XafDisplayName("Nomor DM"), ToolTip("Nomor DM")] 
     [Size(15)]
    [Appearance("DaftarMuatNomorDM", Enabled =false)]
    [IsSearch(true)]
     public virtual string NomorDM
     { 
       get { return _NomorDM; } 
       set { SetPropertyValue("NomorDM", ref _NomorDM, value); } 
     } 
     // 
     private DateTime _Tanggal; 
     [XafDisplayName("Tanggal"), ToolTip("Tanggal")]
        [ModelDefault("DisplayFormat", "{0: dd/MM/yyyy}")]
        public virtual DateTime Tanggal
     { 
       get { return _Tanggal; } 
       set { SetPropertyValue("Tanggal", ref _Tanggal, value); } 
     } 
     // 
     private Truck _NoTruck; 
     [XafDisplayName("Nomor Truck"), ToolTip("Nomor Truck")] 
     [ImmediatePostData]
        [RuleRequiredField(DefaultContexts.Save)]
     public virtual Truck NoTruck
     { 
       get { return _NoTruck; } 
       set {
                if ( !IsSaving && !IsLoading && value!=null)
                { 
                    if ( _NoTruck != value)
                    {
                        NamaSpr = value.Supir;
                      //  Session.Save(this);
                    }
                }
                SetPropertyValue("NoTruck", ref _NoTruck, value); } 
     } 
     // 
     private Supir _NamaSpr; 
     [XafDisplayName("Nama Sopir"), ToolTip("Nama Sopir")]
        [RuleRequiredField(DefaultContexts.Save)]
        public virtual Supir NamaSpr
     { 
       get { return _NamaSpr; } 
       set {
                if (!IsSaving && !IsLoading && value != null)
                {
                    if (_NamaSpr != value)
                    {
                        HpSpr = value.NomorHP;
                        
                    }
                }
                SetPropertyValue("NamaSpr", ref _NamaSpr, value); } 
     } 
     // 
     private string _HpSpr; 
     [XafDisplayName("Nomor Handpone Sopir"), ToolTip("Nomor Handpone Sopir")] 
     [Size(50)] 
     public virtual string HpSpr
     { 
       get { return _HpSpr; } 
       set { SetPropertyValue("HpSpr", ref _HpSpr, value); } 
     } 
     // 
     //private string _NomorSM; 
     //[XafDisplayName("Nomor"), ToolTip("Nomor")] 
     //[Size(6)] 
     //public virtual string NomorSM
     //{ 
     //  get { return _NomorSM; } 
     //  set { SetPropertyValue("NomorSM", ref _NomorSM, value); } 
     //} 
     // 
     //private Pelanggan _Pengirim; 
     //[XafDisplayName("Pengirim"), ToolTip("Pengirim")] 
     //public virtual Pelanggan Pengirim
     //{ 
     //  get { return _Pengirim; } 
     //  set { SetPropertyValue("Pengirim", ref _Pengirim, value); } 
     //} 
     // 
     //private string _TTB; 
     //[XafDisplayName("Tanda Terima Barang"), ToolTip("Tanda Terima Barang")] 
     //[Size(15)] 
     //public virtual string TTB
     //{ 
     //  get { return _TTB; } 
     //  set { SetPropertyValue("TTB", ref _TTB, value); } 
     //} 
     // 
     //private Pelanggan _Penerima; 
     //[XafDisplayName("Penerima"), ToolTip("Penerima")] 
     //public virtual Pelanggan Penerima
     //{ 
     //  get { return _Penerima; } 
     //  set { SetPropertyValue("Penerima", ref _Penerima, value); } 
     //} 
     // 
     [XafDisplayName("SuratMuatan"), ToolTip("SuratMuatan")] 
     [Association("DM")]
     [IsHide]
     [DevExpress.Xpo.Aggregated]
        //[RuleRequiredField(DefaultContexts.Save)]
        public XPCollection<SuratMuatan> SuratMuatan
     {
     get
       {
         return GetCollection<SuratMuatan>("SuratMuatan"); 
       }
     }
     // 
     private double _JumlahSM; 
     [XafDisplayName("Jumlah Surat Muatan"), ToolTip("Jumlah Surat Muatan")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        [IsHide]
        public virtual double JumlahSM
     { 
       get {
                _JumlahSM = 0;
                if (!IsLoading && !IsSaving && SuratMuatan != null)
                {
                    try
                    {
                        if (SuratMuatan.Count > 0)
                        {
                            _JumlahSM = (double)SuratMuatan.Count();
                        }
                    }
                    catch (Exception e)
                    { }

                    try
                    {
                        int i = 1;
                        foreach ( SuratMuatan sm  in this.SuratMuatan)
                        {
                            
                            if ( sm.NomorSM=="" || sm.NomorSM ==null)
                            {
                                sm.NomorSM = $"{i.ToString("000")}"; 
                            }
                            i++;
                        }
                    }
                    catch ( Exception e)
                    {

                    }
                }
                return _JumlahSM; 
            } 
       //set { SetPropertyValue("JumlahSM", ref _JumlahSM, value); } 
     } 
     // 
     private double _JmlColly; 
     [XafDisplayName("Jumlah Colly"), ToolTip("Jumlah Colly")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        [IsHide]
        public virtual double JmlColly
     { 
       get {
                _JmlColly = 0;
                if (!IsLoading && !IsSaving && SuratMuatan != null)
                {
                    try
                    {
                        if (SuratMuatan.Count > 0)
                        {
                            _JmlColly = (double)SuratMuatan.Sum(x => x.Colly);
                        }
                    }
                    catch ( Exception e)
                    { }
                }
                
                return _JmlColly; 
            } 
       //set { SetPropertyValue("JmlColly", ref _JmlColly, value); } 
     } 
   
        public void UpdateTotalJumlah(bool forceChangeEvents)
        {
            //double? oldJmlTagihan = _JmlTagihan;
            //double? tempJmlTagihan = 0;
            ////foreach (SuratMuatan detail in SuratMuatan)
            ////    tempJmlTagihan += detail.TotalJml;
            //tempJmlTagihan = (double)SuratMuatan.Sum(x => x.TotalJml);
            //_JmlTagihan = tempJmlTagihan;
            //if (forceChangeEvents)
            //{
            //    OnChanged(nameof(JmlTagihan), oldJmlTagihan, _JmlTagihan);
            //}
        }
        // 
      
     // 
     private double _BonSementara; 
     [XafDisplayName("Bon Sementara"), ToolTip("Bon Sementara")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        [ImmediatePostData]
     public virtual double BonSementara
     { 
       get { return _BonSementara; } 
       set { SetPropertyValue("BonSementara", ref _BonSementara, value); } 
     }

        private double Komisi;
     // 
     private double _KomisiPersen; 
     [XafDisplayName("Komisi 10%"), ToolTip("Komisi 10%")]
        [ImmediatePostData]
        public virtual double KomisiPersen
        { 
       get { return _KomisiPersen; } 
       set { SetPropertyValue("KomisiPersen", ref _KomisiPersen, value); } 
     } 
     // 
     private double _OngkosMuatan; 
     [XafDisplayName("Ongkos Muatan"), ToolTip("Ongkos Muatan")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        [ImmediatePostData]
        public virtual double OngkosMuatan
     { 
       get { return _OngkosMuatan; } 
       set { SetPropertyValue("OngkosMuatan", ref _OngkosMuatan, value); } 
     } 
     // 
     private double _BiayaLain1; 
     [XafDisplayName("Biaya Lain-Lain 1"), ToolTip("Biaya Lain-Lain 1")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        [ImmediatePostData]
        public virtual double BiayaLain1
     { 
       get { return _BiayaLain1; } 
       set {
                //if (SetPropertyValue(nameof(BiayaLain1), ref _BiayaLain1, value))
                //   OnChanged(nameof(TotalBiayaLain));
                SetPropertyValue("BiayaLain1", ref _BiayaLain1, value);
                //TotalBiayaLL();
            } 
     } 
     // 
     private double _Klaim; 
     [XafDisplayName("Klaim"), ToolTip("Klaim")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        [ImmediatePostData]
        public virtual double Klaim
        { 
       get { return _Klaim; } 
       set {
                //if (SetPropertyValue(nameof(BiayaLain2), ref _BiayaLain2, value))
                //  OnChanged(nameof(TotalBiayaLain));
                SetPropertyValue("Klaim", ref _Klaim, value);
                //TotalBiayaLL();
            } 
     } 
     // 

        private void TotalBiayaLL()
        {
            if (!IsSaving && !IsLoading)
            {
               // TotalBiayaLain = BiayaLain1 + BiayaLain2 + BiayaLain3;
            }
        }
        private double _TotalBiayaLain;
        [XafDisplayName("Total Biaya Lain-Lain"), ToolTip("Total Biaya Lain-Lain")]
        [IsHide]
        [PersistentAlias("Klaim+BiayaLain1")]
        public virtual double TotalBiayaLain
        {
            get
            {
                object tempObject = EvaluateAlias(nameof(TotalBiayaLain));
                if (tempObject != null)
                {
                    return (double)tempObject;
                }
                else
                {
                    return 0;
                    // return _TotalBiayaLain; 
                }
            }
            //set { SetPropertyValue("TotalBiayaLain", ref _TotalBiayaLain, value); }
        }

        // 
     private double _Administrasi; 
     [XafDisplayName("Administrasi"), ToolTip("Administrasi")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        [IsHide]
        //[ImmediatePostData]
        [PersistentAlias("Administrasi1+Administrasi2")]
        public virtual double Administrasi
     { 
       get {
                object tempObject = EvaluateAlias(nameof(Administrasi));
                if (tempObject != null)
                {
                    return (double)tempObject;
                }
                else
                {
                    return 0;

                }
                //    return _Administrasi; 
            } 
      // set { SetPropertyValue("Administrasi", ref _Administrasi, value); } 
     } 
     // 
     private double _Administrasi1; 
     [XafDisplayName("Administrasi1"), ToolTip("Administrasi1")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        [ImmediatePostData]
        public virtual double Administrasi1
        { 
       get { return _Administrasi1; } 
       set {
                //if (SetPropertyValue(nameof(OngkosLain1), ref _OngkosLain1, value))
                //  OnChanged(nameof(TotalOngkosLain));
                SetPropertyValue("Administrasi1", ref _Administrasi1, value);
                //TotalOngkosLL();
            } 
     }

        private double _Administrasi2;
        [XafDisplayName("Administrasi2"), ToolTip("Administrasi2")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        [ImmediatePostData]
        public virtual double Administrasi2
        {
            get { return _Administrasi2; }
            set
            {
                //if (SetPropertyValue(nameof(OngkosLain1), ref _OngkosLain1, value))
                //  OnChanged(nameof(TotalOngkosLain));
                SetPropertyValue("Administrasi2", ref _Administrasi2, value);
                //TotalOngkosLL();
            }
        }
        private void TotalOngkosLL()
        {
            if (!IsSaving && !IsLoading)
            {
                //TotalOngkosLain = BiayaLain1 + BiayaLain2 + BiayaLain3;
            }
        }
        private double _TotalOngkosLain;
        [XafDisplayName("Total Ongkos Lain-Lain"), ToolTip("Total Ongkos Lain-Lain")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        //[PersistentAlias("OngkosLain3+OngkosLain2+OngkosLain1")]
        public virtual double TotalOngkosLain
        {
            get {
          
                return _TotalOngkosLain; 
            }
            set { SetPropertyValue("TotalOngkosLain", ref _TotalOngkosLain, value); }
        }

        // 
     private double _Total; 
     [XafDisplayName("Total"), ToolTip("Total")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        [IsHide]
        [PersistentAlias("KomisiPersen+OngkosMuatan+TotalBiayaLain+Administrasi+TotalOngkosLain")]
        public virtual double Total
     { 
       get {


                object tempObject = EvaluateAlias(nameof(Total));
                if (tempObject != null)
                {
                    return (double)tempObject;
                }
                else
                {
                    return 0;
                    // return _TotalBiayaLain; 
                }
                //_Total = KomisiPersen + OngkosMuatan + TotalBiayaLain + Administrasi + TotalOngkosLain;
                //return _Total;
            }
            //set
            //{
            //    SetPropertyValue("Total", ref _Total, value);
            //}
        } 
     // 
     private double _Dibayar; 
     [XafDisplayName("Dibayar"), ToolTip("Dibayar")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        [IsHide]
        [PersistentAlias("Total+BonSementara")]
        public virtual double Dibayar
     { 
       get {

                object tempObject = EvaluateAlias(nameof(Dibayar));
                if (tempObject != null)
                {
                    return (double)tempObject;
                }
                else
                {
                    return 0;
                    // return _TotalBiayaLain; 
                }
                //return _Dibayar;
            }
            //set { SetPropertyValue("Dibayar", ref _Dibayar, value); }
        }
        private double _SisaFL;
        [XafDisplayName("Sisa F/L"), ToolTip("Sisa F/L")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        [IsHide]
        //[PersistentAlias("Dibayar- (Loco")]
        public virtual double SisaFL
        {
            get {
                _SisaFL = 0;
                if (!IsLoading && !IsSaving && SuratMuatan != null)
                {
                    if (SuratMuatan.Count > 0)
                    {
                        _SisaFL = (double)SuratMuatan.Where(x => x.Pembayaran != ePembayaran.TagihDitujuan).Sum(x => x.TotalJml);
                    }
                }
                _SisaFL =( Dibayar - _SisaFL) *-1;
                return _SisaFL;

                }
            //set { SetPropertyValue("SisaFL", ref _SisaFL, value); }
        }
        // 
        private double _Loco;
        [XafDisplayName("Loco"), ToolTip("Loco")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        [IsHide]
        public virtual double Loco
        {
            get {
                _Loco = 0;
                if (!IsLoading && !IsSaving && SuratMuatan != null)
                {
                    if (SuratMuatan.Count > 0)
                    {
                        _Loco = (double)SuratMuatan.Where(x => x.Pembayaran == ePembayaran.TagihDitujuan).Sum(x => x.TotalJml);
                    }
                }
                return _Loco; 
               }
            //set { SetPropertyValue("Loco", ref _Loco, value); }
        }

 

        //   [Persistent("JmlTagihan")]
        private double _JmlTagihan;
        [XafDisplayName("Jumlah Tagihan"), ToolTip("Jumlah Tagihan")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        [IsHide]
        //   [PersistentAlias(nameof(_JmlTagihan))]
        public virtual double JmlTagihan
        {
            get
            {
                _JmlTagihan = 0;
                if (!IsLoading && !IsSaving && SuratMuatan != null)
                {
                    try
                    {
                        if (SuratMuatan.Count > 0)
                        {
                            _JmlTagihan = (double)SuratMuatan.Sum(x => x.TotalJml);
                        }
                    }
                    catch ( Exception e)
                    { }
                }
                return _JmlTagihan;
            }
        }

        private double _JmlTagihanOrg;
        [XafDisplayName("Jumlah Tagihan Org"), ToolTip("Jumlah Tagihan Org")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        [IsHide]
        //   [PersistentAlias(nameof(_JmlTagihan))]
        public virtual double JmlTagihanOrg
        {
            get
            {
                _JmlTagihanOrg = 0;
                if (!IsLoading && !IsSaving && SuratMuatan != null)
                {
                    try
                    {
                        if (SuratMuatan.Count > 0)
                        {
                            _JmlTagihanOrg = (double)SuratMuatan.Sum(x => x.TotalJmlOrg);
                        }
                    }
                    catch (Exception e)
                    { }
                }
                return _JmlTagihanOrg;
            }
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



        [Action(Caption = "Buat Nota",ImageName = "NewTask_16x16")]
        public void NotaGenerate()
        {
            Session newSession = Session;
            // form select by pelanggan
            //var query = SuratMuatan
            //            .Where(sm => sm.Pembayaran==ePembayaran.Franco )
            //            .GroupBy(sm => new { sm.Pelanggan})
            //            .Select(group => new { SMNota = group.Key, NomorSM = group.Min( sm => sm.NomorSM) 
            //            , TotalSM = group.Count()  });

            var query = SuratMuatan
                        .Where(sm => sm.Pembayaran == ePembayaran.Franco);
            foreach (var item in query)
            {

                //string  sqlQuery = string.Format(" Oid == {0} ", item.SMNota.Pelanggan.Oid);
                string sqlQuery = string.Format(" Oid == {0} ", item.Pelanggan.Oid);
                CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);
                Pelanggan oPelanggan = newSession.FindObject<Pelanggan>(filterOperator);

                CriteriaOperator oOLCriteria = null;
                // oOLCriteria = GroupOperator.And(oOLCriteria, new BinaryOperator("Detailer", item.saleout.Detailer));

                oOLCriteria = GroupOperator.And(oOLCriteria, new BinaryOperator("Pelangan", oPelanggan));
                oOLCriteria = GroupOperator.And(oOLCriteria, new BinaryOperator("NoNota", $"{this.NomorDM}.{item.NomorSM}".Replace("DBL.", "")));
                Nota oNota = newSession.FindObject<Nota>(oOLCriteria);
                if (oNota == null)
                {
                    //oNota.NoNota

                    oNota = new Nota(newSession);
                    oNota.Pelangan = oPelanggan;
                    oNota.DM = this;
                    oNota.NoNota = $"{this.NomorDM}.{item.NomorSM}".Replace("DBL.", "");
                    oNota.Tanggal = Tanggal;// new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    oNota.WithPPN = oPelanggan.PPN;
                    //oNota.SuratMuat= 
                    var querySM = SuratMuatan
                     //  .Where(sm => sm.Pelanggan == item.SMNota.Pelanggan && sm.Pembayaran == ePembayaran.Franco     ) ;
                     // .Where(sm => sm.Pelanggan == item.Pelanggan && sm.Pembayaran == ePembayaran.Franco);
                     .Where(sm => sm.Oid == item.Oid && sm.Pembayaran == ePembayaran.Franco);
                    string NotaTTDSM = "";
                    foreach (var itemSM in querySM)
                    {
                        sqlQuery = string.Format(" Oid == {0} ", itemSM.Oid);
                        filterOperator = CriteriaOperator.Parse(sqlQuery);
                        SuratMuatan oSM = newSession.FindObject<SuratMuatan>(filterOperator);
                        NotaTTDSM += oSM.TandaTerimaNo;// + " & ";
                        oNota.SuratMuats.Add(oSM);
                        oNota.SuratMuat = oSM;
                    }

                    //if (NotaTTDSM.Length > 2)
                    //{
                    //    oNota.NoTTDSM = NotaTTDSM.Substring(1, NotaTTDSM.Length - 2);
                    //}
                    //else

                    //{
                    oNota.NoTTDSM = NotaTTDSM;
                    //}
                    oNota.Save();
                    oNota.Session.CommitTransaction();
                }
                else if (oNota != null)
                {
                    if (oNota.NoTTDSM == null || oNota.NoTTDSM == "")
                    {
                      var querySM = SuratMuatan
                     //.Where(sm => sm.Pelanggan == item.Pelanggan && sm.Pembayaran == ePembayaran.Franco);
                     .Where(sm => sm.Oid == item.Oid && sm.Pembayaran == ePembayaran.Franco);
                        string NotaTTDSM = "";
                        foreach (var itemSM in querySM)
                        {
                            sqlQuery = string.Format(" Oid == {0} ", itemSM.Oid);
                            filterOperator = CriteriaOperator.Parse(sqlQuery);
                            SuratMuatan oSM = newSession.FindObject<SuratMuatan>(filterOperator);
                            NotaTTDSM += oSM.TandaTerimaNo;// + " & ";
                            //oNota.SuratMuats.Add(oSM);
                        }
                      
                        oNota.NoTTDSM = NotaTTDSM;
                        oNota.Save();
                        oNota.Session.CommitTransaction();
                    }
                }


                //oNota.NoNota
            }

            KlaimUpdate();
        }
        public void NotaGenerateOld()
     {
            Session newSession = Session;
            // form select by pelanggan
            //var query = SuratMuatan
            //            .Where(sm => sm.Pembayaran==ePembayaran.Franco )
            //            .GroupBy(sm => new { sm.Pelanggan})
            //            .Select(group => new { SMNota = group.Key, NomorSM = group.Min( sm => sm.NomorSM) 
            //            , TotalSM = group.Count()  });

            var query = SuratMuatan
                        .Where(sm => sm.Pembayaran == ePembayaran.Franco);
            foreach (var item in query)
            {

                //string  sqlQuery = string.Format(" Oid == {0} ", item.SMNota.Pelanggan.Oid);
                string  sqlQuery = string.Format(" Oid == {0} ", item.Pelanggan.Oid);
                CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);
                Pelanggan oPelanggan = newSession.FindObject<Pelanggan>(filterOperator);

                CriteriaOperator oOLCriteria = null;
                // oOLCriteria = GroupOperator.And(oOLCriteria, new BinaryOperator("Detailer", item.saleout.Detailer));

                oOLCriteria = GroupOperator.And(oOLCriteria, new BinaryOperator("Pelangan", oPelanggan));
                oOLCriteria = GroupOperator.And(oOLCriteria, new BinaryOperator("NoNota", $"{this.NomorDM}.{item.NomorSM}".Replace("DBL.","") ));
                Nota oNota = newSession.FindObject<Nota>(oOLCriteria);
                if (oNota==null)
                {
                    //oNota.NoNota

                    oNota = new Nota(newSession);
                    oNota.Pelangan = oPelanggan;
                    oNota.DM = this;
                    oNota.NoNota = $"{this.NomorDM}.{item.NomorSM}".Replace("DBL.","");
                    oNota.Tanggal = Tanggal;// new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    oNota.WithPPN = oPelanggan.PPN;
                    var querySM = SuratMuatan
                     //  .Where(sm => sm.Pelanggan == item.SMNota.Pelanggan && sm.Pembayaran == ePembayaran.Franco     ) ;
                     .Where(sm => sm.Pelanggan == item.Pelanggan && sm.Pembayaran == ePembayaran.Franco);
                    string NotaTTDSM = "";
                    foreach (var itemSM in querySM)
                    {
                        sqlQuery = string.Format(" Oid == {0} ", itemSM.Oid);
                        filterOperator = CriteriaOperator.Parse(sqlQuery);
                        SuratMuatan oSM = newSession.FindObject<SuratMuatan>(filterOperator);
                        NotaTTDSM += oSM.TandaTerimaNo + " & ";
                        oNota.SuratMuats.Add(oSM);
                    }

                    if (NotaTTDSM.Length > 2)
                    {
                        oNota.NoTTDSM = NotaTTDSM.Substring(1, NotaTTDSM.Length - 2);
                    }
                    else

                    {
                        oNota.NoTTDSM = "";
                    }
                    oNota.Save();
                    oNota.Session.CommitTransaction();
                }
                else if (oNota != null)
                {
                    if (oNota.NoTTDSM == null || oNota.NoTTDSM=="")
                    {
                        var querySM = SuratMuatan
                     .Where(sm => sm.Pelanggan == item.Pelanggan && sm.Pembayaran==ePembayaran.Franco);
                        string NotaTTDSM = "";
                        foreach (var itemSM in querySM)
                        {
                            sqlQuery = string.Format(" Oid == {0} ", itemSM.Oid);
                            filterOperator = CriteriaOperator.Parse(sqlQuery);
                            SuratMuatan oSM = newSession.FindObject<SuratMuatan>(filterOperator);
                            NotaTTDSM += oSM.TandaTerimaNo + " & ";
                            //oNota.SuratMuats.Add(oSM);
                        }
                        if (NotaTTDSM.Length>0)
                        {
                            NotaTTDSM.Substring(0, NotaTTDSM.Length - 3);
                        }
                        oNota.NoTTDSM = NotaTTDSM;
                        oNota.Save();
                        oNota.Session.CommitTransaction();
                    }
                }


                    //oNota.NoNota
            }

            KlaimUpdate();
      }

        public void NotaGen(SuratMuatan item)
        {
           

                //oNota.NoNota
          
        }
        public void KlaimUpdate()
     {
            iGenTriJasa oGenTriJasa = new iGenTriJasa();
            Session newSession = Session;
            oGenTriJasa.SupirKlaimSet(Session, eSupirKlaim.DepositDM, this.NamaSpr,  this, this.Klaim, this.NoTruck.NomorTruck,Tanggal);
    }
   } 
   
} 
