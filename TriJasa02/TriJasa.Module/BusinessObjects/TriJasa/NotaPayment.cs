// Class Name : NotaPayment.cs 
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
   [System.ComponentModel.DisplayName("Penerimaan Piutang Dagang")]

    [RuleCriteria("NotaPaymentSelisihMinus", DefaultContexts.Save, "Selisih < 0 ",
   "Selisih tidak boleh minus", SkipNullOrEmptyValues = false)]
    public class NotaPayment : XPObject
   {
     public NotaPayment() : base()
     {
     // This constructor is used when an object is loaded from a persistent storage.
     // Do not place any code here.
     }
     public NotaPayment(Session session) : base(session) 
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
            //UserUpdate = (User) Session.FindObject<User>(new BinaryOperator("UserName", (User) SecuritySystem.CurrentUserId)); 
             UserUpdate = Session.FindObject<UserLogin>(new BinaryOperator("UserName", tUser));
            Nomor = Number(Tanggal);
            // LastUpdate = DateTime.Now; 
            //UserUpdate = (User)SecuritySystem.CurrentUserId;
            if (UserUpdate != null)
            {
                if (UserUpdate.Perusahaan != null)
                {
                    Bank = UserUpdate.Perusahaan.BankPenerimaanPiutang;
                    KodeAcc = UserUpdate.Perusahaan.AccountPiutang;

                }
            }
     }

        public string Number(DateTime sDate)
        {
            //TAHUN +BULAN + NOMOR URUT / NOMOR URUT SM / TTB
            //  DBL.2102.001 / 012 / TANDATERIMA

            string sNumer = "";
            int sRun = 1;
            byte iYear = (byte)((sDate.Year - 2014) + 65);
            string sYear = $"NPM.{sDate.ToString("yyMM")}";
            XPCollection<NotaPayment> xpDM = new XPCollection<NotaPayment>(Session);
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
        protected override void OnSaving()
     {
            string tUser = SecuritySystem.CurrentUserName.ToString();
            UserUpdate = Session.FindObject<UserLogin>(new BinaryOperator("UserName", tUser));
            lastUpdate = System.DateTime.Now;
            if (Reference != null)
            {
                Reference.Pelanggan = Pelanggan;
              
            }
            base.OnSaving();
     } 
     protected override void OnDeleting()
     {
       base.OnDeleting();
     }
        // 

        private UserLogin _UserUpdate;
        [XafDisplayName("UserUpdate"), ToolTip("lastUpdate")]
        [Size(20)]
        public virtual UserLogin UserUpdate
        {
            get { return _UserUpdate; }
            set { SetPropertyValue("UserUpdate", ref _UserUpdate, value); }
        }

        private DateTime _lastUpdate;
        [ModelDefault("DisplayFormat", "{0: dd/MM/yyyy}")]

        [XafDisplayName("lastUpdate"), ToolTip("lastUpdate")]
        [Size(20)]
        public virtual DateTime lastUpdate
        {
            get { return _lastUpdate; }
            set { SetPropertyValue("lastUpdate", ref _lastUpdate, value); }
        }


     private string _Nomor; 
     [XafDisplayName("Nomor"), ToolTip("Nomor")] 
     [Size(20)] 
     public virtual string Nomor
     { 
       get {
                if (!IsLoading && !IsSaving)
                {
                    if (_Nomor == null)
                    {
                        _Nomor = "";
                    }
                }
                    return _Nomor; } 
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
     private string _Referensi; 
     [XafDisplayName("Referensi"), ToolTip("Referensi")] 
     [Size(20)] 
     public virtual string Referensi
     { 
       get { return _Referensi; } 
       set { SetPropertyValue("Referensi", ref _Referensi, value); } 
     } 
     // 
     private double _Jmlditerima; 
     [XafDisplayName("Jumlah Yang Diterima"), ToolTip("Jumlah Yang Diterima")] 
     [ImmediatePostData]
     public virtual double Jmlditerima
     { 
       get { return _Jmlditerima; } 
       set { SetPropertyValue("Jmlditerima", ref _Jmlditerima, value); } 
     }
        // 
        private BankTran _Reference;
        [XafDisplayName("Bank Reference"), ToolTip("Bank Reference")]
        public virtual BankTran Reference
        {
            get { return _Reference; }
            set { SetPropertyValue("Reference", ref _Reference, value); }
        }
        private Bank _Bank; 
     [XafDisplayName("Diterima Pada"), ToolTip("Diterima Pada")] 
     public virtual Bank Bank
     { 
       get { return _Bank; } 
       set { SetPropertyValue("Bank", ref _Bank, value); } 
     } 
     // 
     private DateTime _BankTglTerima; 
     [XafDisplayName("Tanggal diterima"), ToolTip("Tanggal diterima")] 
     public virtual DateTime BankTglTerima
     { 
       get { return _BankTglTerima; } 
       set { SetPropertyValue("BankTglTerima", ref _BankTglTerima, value); } 
     } 
     // 
     private string _NoCekBG; 
     [XafDisplayName("Nomor Cek/BG"), ToolTip("Nomor Cek/BG")] 
     [Size(25)] 
     public virtual string NoCekBG
     { 
       get { return _NoCekBG; } 
       set { SetPropertyValue("NoCekBG", ref _NoCekBG, value); } 
     } 
     // 
     private DateTime _BankTglTempo; 
     [XafDisplayName("Tanggal Jatuh Tempo"), ToolTip("Tanggal Jatuh Tempo")] 
     public virtual DateTime BankTglTempo
     { 
       get { return _BankTglTempo; } 
       set { SetPropertyValue("BankTglTempo", ref _BankTglTempo, value); } 
     } 
     // 
     private string _Catatan; 
     [XafDisplayName("Catatan"), ToolTip("Catatan")] 
     [Size(40)] 
     public virtual string Catatan
     { 
       get { return _Catatan; } 
       set { SetPropertyValue("Catatan", ref _Catatan, value); } 
     } 
     // 
     private fTransCode _KodeAcc; 
     [XafDisplayName("Kode Account"), ToolTip("Kode Account")] 
     [Size(15)] 
     public virtual fTransCode KodeAcc
     { 
       get { return _KodeAcc; } 
       set { SetPropertyValue("KodeAcc", ref _KodeAcc, value); } 
     } 
     // 
     private Pelanggan _Pelanggan; 
     [XafDisplayName("Kode Pelangan"), ToolTip("Kode Pelangan")] 
     public virtual Pelanggan Pelanggan
     { 
       get { return _Pelanggan; } 
       set { SetPropertyValue("Pelanggan", ref _Pelanggan, value); } 
     }

        [XafDisplayName("Nota"), ToolTip("Nota")]
        [Association("PaymentNotaList")]
        [DataSourceProperty(nameof(AvailableTasks))]
        public XPCollection<Nota> ListNota
        {
            get
            {
                return GetCollection<Nota>("ListNota");
            }
        }
        private XPCollection<Nota> availableTasks;
        [Browsable(false)] // Prohibits showing the AvailableTasks collection separately
        public XPCollection<Nota> AvailableTasks
        {
            get
            {
                if (availableTasks == null)
                {
                    // Retrieve all Task objects
                    availableTasks = new XPCollection<Nota>(Session);
                }
                // Filter the retrieved collection according to the current conditions
                RefreshAvailableTasks();
                // Return the filtered collection of Task objects
                return availableTasks;
            }
        }

        private void RefreshAvailableTasks()
        {
            if (availableTasks == null)
                return;
            string sqlQuery = string.Format($" [Status] == 3 && Sisa!=0 ");// &&  Status != {eTagihan.Kolektor.GetHashCode()}  ");
            CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);

            //Nota b;
            
           //b.Status=eTagihan.
            //Remove the applied filter
            availableTasks.Criteria = filterOperator;

        }
        // 
      [XafDisplayName("Nota"), ToolTip("Nota")] 
     [Association("NotaPayAssc")] 
     public XPCollection<NotaPaymentDtl> NotaPaymentDtl
     {
     get
       {
         return GetCollection<NotaPaymentDtl>("NotaPaymentDtl"); 
       }
     }
     // 
     private double _Jumlah; 
     [XafDisplayName("Jumlah"), ToolTip("Jumlah")] 
     public virtual double Jumlah
     { 
       get {
                _Jumlah = 0;
                if (!IsLoading && !IsSaving && ListNota != null)
                {
                    try
                    {
                        //PayDetail();
                        if (ListNota.Count > 0)
                        {
                            _Jumlah = (double)ListNota.Sum(x => x.PmtDiterima);
                        }
                    }
                    catch (Exception e)
                    { }
                }

                return _Jumlah; 
            } 
       //set { SetPropertyValue("Jumlah", ref _Jumlah, value); } 
     }

        private double _JumlahBank;
        [XafDisplayName("JumlahBank"), ToolTip("JumlahBank")]
        public virtual double JumlahBank
        {
            get
            {
                _JumlahBank = 0;
                if (!IsLoading && !IsSaving && NotaPaymentDtl != null)
                {
                    try
                    {
                        //PayDetail();
                        if (ListNota.Count > 0)
                        {
                            _JumlahBank = (double)NotaPaymentDtl.Sum(x => x.Diterima);
                        }

                    }
                    catch (Exception e)
                    { }
                }

                return _JumlahBank;
            }
            //set { SetPropertyValue("Jumlah", ref _Jumlah, value); } 
        }
        public void PayDetail()
        {
            List<NotaPaymentDtl> todelete = new List<NotaPaymentDtl>();

            foreach (NotaPaymentDtl aPaymentDtl in NotaPaymentDtl)
            {
                aPaymentDtl.Diterima = 0;
                todelete.Add(aPaymentDtl);
            }

            foreach (Nota aNota in ListNota)
            {
                bool isFind = false;
                foreach (NotaPaymentDtl aTagihNota in NotaPaymentDtl)
                {
                    if (aTagihNota.Faktur == aNota)
                    {
                        aTagihNota.Diterima = aNota.PmtDiterima;
                        aTagihNota.Pot = aNota.PmtPotongan;
                        aTagihNota.Adjustment = aNota.PmtAdjutment;
                        if (aTagihNota.Sisa==0)
                        {
                            aNota.Status = eTagihan.Dibayar;
                            aNota.Save();
                        }
                        isFind = true;
                    }
                }
                if (isFind == false)
                {
                    NotaPaymentDtl aDFNota = new NotaPaymentDtl(Session);
                    aDFNota.Faktur = aNota;
                    aDFNota.Diterima = aNota.PmtDiterima;
                    aDFNota.Pot = aNota.PmtPotongan;
                    aDFNota.Adjustment = aNota.PmtAdjutment;
                    //aDFNota.Status = eTagihan.Kolektor;
                    aDFNota.NotaPayment = this;
                    if (aDFNota.Sisa == 0)
                    {
                        aNota.Status = eTagihan.Dibayar;
                        aNota.Save();
                    }
                    aDFNota.Save();
                    //aDFNota.Session.CommitTransaction();
                    //ListNotaTagih.Add(aDFNota);
                }
              
            }
            //foreach (NotaPaymentDtl aNota in todelete)
            //{
            //    foreach (NotaPaymentDtl aTagihNota in NotaPaymentDtl)
            //    {
            //        if (aTagihNota == aNota && aTagihNota.Diterima ==0)
            //        {
            //            aTagihNota.Delete();
            //            aTagihNota.Session.CommitTransaction();
            //            break;
            //        }
            //    }
            //}
        }
     // 
     private double _Selisih; 
     [XafDisplayName("Selisih"), ToolTip("Selisih")]
        [PersistentAlias("Jmlditerima-Jumlah ")]
        public virtual double Selisih
     { 
       get {
                object tempObject = EvaluateAlias(nameof(Selisih));
                if (tempObject != null)
                {
                    return (double)tempObject;
                }
                else
                {
                    return 0;

                }
                //return _Selisih; 
            } 
       //set { SetPropertyValue("Selisih", ref _Selisih, value); } 
     } 
   } 
   
} 
