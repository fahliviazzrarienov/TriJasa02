// Class Name : PenerimaanKasir.cs 
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
   [DefaultProperty("Nomor")]
   [NavigationItem("Transaksi")]
   // Standard Document
   [System.ComponentModel.DisplayName("Penerimaan Kasir")]
   public class PenerimaanKasir : XPObject
   {
     public PenerimaanKasir() : base()
     {
     // This constructor is used when an object is loaded from a persistent storage.
     // Do not place any code here.
     }
     public PenerimaanKasir(Session session) : base(session) 
     {
       // This constructor is used when an object is loaded from a persistent storage.
       // Do not place any code here.
     }
        public string Number(DateTime sDate)
        {
            //TAHUN +BULAN + NOMOR URUT / NOMOR URUT SM / TTB
            //  DBL.2102.001 / 012 / TANDATERIMA

            string sNumer = "";
            int sRun = 1;
            byte iYear = (byte)((sDate.Year - 2014) + 65);
            string sYear = $"KAS.{sDate.ToString("yyMM")}";
            XPCollection<PenerimaanKasir> xpDM = new XPCollection<PenerimaanKasir>(Session);
            //string sNumberMax = (string)xpDM.Max(x => x.NomorDM)
            string sNumberMax = "";
            try
            {
                sNumberMax = xpDM
               //SelectMany(c => c.).
               .Where(o => o.Nomor.Substring(0, 8).ToUpper().Trim() == sYear.ToUpper().Trim())
               .Max(o => o.Nomor.Trim());
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
        public override void AfterConstruction()
     {
       base.AfterConstruction();
       // Place here your initialization code.
       //SecuritySystem.CurrentUserName
       //LastUpdateUser = Session.GetObjectByKey<GPUser>(SecuritySystem.CurrentUserId);
       string tUser = SecuritySystem.CurrentUserName.ToString();
       Tanggal = new DateTime(System.DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
       Nomor = Number(Tanggal);

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
     private string _Nomor; 
     [XafDisplayName("Nomor Penerimaan Cash"), ToolTip("Nomor Penerimaan Cash")]
     [Appearance("PenerimanKasirNomorEnable", Enabled = false)]
     [Size(15)] 
     public virtual string Nomor
     { 
       get { return _Nomor; } 
       set { SetPropertyValue("Nomor", ref _Nomor, value); } 
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
     private string _NoTTB; 
     [XafDisplayName("Nomor TTB"), ToolTip("Nomor TTB")] 
     [Size(20)] 
     [ImmediatePostData]
     public virtual string NoTTB
     { 
       get { return _NoTTB; } 
       set {
                if (!IsLoading && !IsSaving)
                {
                    value = value.ToUpper();
                }
                SetPropertyValue("NoTTB", ref _NoTTB, value);
              
            } 
     } 
     // 
     private Pelanggan _Pengirim; 
     [XafDisplayName("Pengirim"), ToolTip("Pengirim")] 
     [Size(25)] 
     public virtual Pelanggan Pengirim
     { 
       get { return _Pengirim; } 
       set { SetPropertyValue("Pengirim", ref _Pengirim, value);
                if ( !IsLoading && !IsSaving)
                {
                    TandaBuktiPembayaran = value.PembayaranSementara;
                }
             } 
     } 
     // 
     private double _JumlahAlokasi; 
     [XafDisplayName("Alokasi"), ToolTip("Alokasi")] 
     public virtual double JumlahAlokasi
        { 
       get {
                _JumlahAlokasi = 0;
                if (!IsLoading && !IsSaving )
                {
                    try
                    {
                        //string sqlQuery = string.Format(" Oid == {0} ", oTransCode.Oid.ToString());
                        //CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);
                        CriteriaOperator oCriteria = null;
                        //oCriteria = GroupOperator.And(oCriteria, new BetweenOperator(Supir, new OperandProperty("StartDate"), new OperandProperty("EndDate")));
                        oCriteria = GroupOperator.And(oCriteria, new BinaryOperator("TerimaKasir", this));
                        XPCollection<SuratMuatan> oSM = new XPCollection<SuratMuatan>(Session, oCriteria);
                        if (oSM.Count > 0)
                        {
                            _JumlahAlokasi = (double)oSM.Where(x => x.TerimaKasir == this).Sum(x => x.TotalDP);
                        }
                    }
                    catch (Exception e)
                    { }
                }
                return _JumlahAlokasi;


            } 
       //set { SetPropertyValue("JumlahAlokasi", ref _JumlahAlokasi, value); } 
     } 


     // 
     private double _JumlahTerima; 
     [XafDisplayName("Jumlah Penerimaan"), ToolTip("Jumlah Penerimaan")] 
     public virtual double JumlahTerima
     { 
       get { return _JumlahTerima; } 
       set { SetPropertyValue("JumlahTerima", ref _JumlahTerima, value); } 
     }
        private bool  _TandaBuktiPembayaran;
        [XafDisplayName("Nota Pelunasan"), ToolTip("Nota Pelunasan")]
        public virtual bool TandaBuktiPembayaran
        {
            get { return _TandaBuktiPembayaran; }
            set { SetPropertyValue("TandaBuktiPembayaran", ref _TandaBuktiPembayaran, value); }
        }
        private bool _Lunas;
        [XafDisplayName("Lunas"), ToolTip("Lunas")]
        public virtual bool Lunas
        {
            get { return _Lunas; }
            set { SetPropertyValue("Lunas", ref _Lunas, value); }
        }
        private bool _Transfer;
        [XafDisplayName("Transfer"), ToolTip("Transfer")]
        public virtual bool Transfer
        {
            get { return _Transfer; }
            set { SetPropertyValue("Transfer", ref _Transfer, value); }
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
     [Size(300)] 
     public virtual string Note2
     { 
       get { return _Note2; } 
       set { SetPropertyValue("Note2", ref _Note2, value); } 
     }
        private BankTran _Kasbank;
        [XafDisplayName("Kasbank"), ToolTip("Kasbank")]
        [Size(30)]
        public virtual BankTran Kasbank
        {
            get { return _Kasbank; }
            set { SetPropertyValue("Kasbank", ref _Kasbank, value); }
        }

        private bool _Disetor;
        [XafDisplayName("Disetor"), ToolTip("Disetor")]
        [Size(30)]
        public virtual bool Disetor
        {
            get { return _Disetor; }
            set { SetPropertyValue("Disetor", ref _Disetor, value); }
        }

        //private SuratMuatan _SuratMuat;
        //[XafDisplayName("SuratMuat"), ToolTip("SuratMuat")]
        //[Size(30)]
        //public virtual SuratMuatan SuratMuat
        //{
        //    get { return _SuratMuat; }
        //    set { SetPropertyValue("SuratMuat", ref _SuratMuat, value); }
        //}

        //private BankTran _NoTransaksiKas;
        //[XafDisplayName("NoTransaksiKas"), ToolTip("NoTransaksiKas")]
        //[Size(30)]
        //public virtual BankTran NoTransaksiKas
        //{
        //    get { return _NoTransaksiKas; }
        //    set { SetPropertyValue("NoTransaksiKas", ref _NoTransaksiKas, value); }
        //}

    } 
   
} 
