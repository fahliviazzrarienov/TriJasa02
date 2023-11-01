// Class Name : BankTran.cs 
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
   [DefaultProperty("Reference")]
   [NavigationItem("Transaksi")]
   // Standard Document
   [System.ComponentModel.DisplayName("Bank dan Kas")]
    [RuleCriteria("BanTransbankAccount", DefaultContexts.Save, " isBankAccount==true ",
    "Mohon Dientry Bank Transaction kode bank", SkipNullOrEmptyValues = false)]
    [RuleCriteria("BanTransbankAmount", DefaultContexts.Save, " isBankAmount ==true ",
    "jumlah tidak sama dengan transaksi Bank", SkipNullOrEmptyValues = false)]

    public class BankTran : fTransKas
   {
     public BankTran() : base()
     {
     // This constructor is used when an object is loaded from a persistent storage.
     // Do not place any code here.
     }
     public BankTran(Session session) : base(session) 
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
           /// Date = new DateTime(System.DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            string tUser = SecuritySystem.CurrentUserName.ToString();

            //LastUpdateUser = Session.FindObject<GPUser>(new BinaryOperator("UserName", SecuritySystem.CurrentUserName.ToString())); 
            // LastUpdateUser = Session.FindObject<GPUser>(new BinaryOperator("UserName", tUser)); 
            // LastUpdate = DateTime.Now; 
            UserUpdate = Session.FindObject<UserLogin>(new BinaryOperator("UserName", tUser));
            bSaveKasbon = false;
        } 
     protected override void OnSaving()
     {
            //KeteranganUpdate();
            if (Reference == "" || Reference == null)
            {
                Reference = RefNumber();
                KasbonAdd();
            }
            else
            {
                KasbonUpdate();
            }
   
            CreateJournal();
          
            base.OnSaving();
     }

        private bool bSaveKasbon; 

        protected override void OnSaved()
        {
            //KeteranganUpdate();
          
            base.OnSaved();
            
        }

        public void CreateJournal()
        {
            bool cjBank = false;
            bool cjDistribusi = false;
            int totalJournal = 0;
            foreach (fTransKasDtl odtl in this.TransKasDtl)
            {
                if ( odtl.TransCode== this.Bank.AccountCode && !cjBank)
                {
                    odtl.TransCode = this.Bank.AccountCode;
                    odtl.Description = Description;
                    if (this.Tipe == eBankTran.Pengeluaran)
                    {
                        odtl.Credit =(int) this.Jumlah;
                    }
                    else
                    {
                        odtl.Debit = (int)this.Jumlah;
                    }

                    cjBank = true;

                }
                else if (cjBank && !cjDistribusi)
                {
                    odtl.TransCode = TransCode;
                    odtl.Description = Description;
                    if (this.Tipe == eBankTran.Pengeluaran)
                    {
                        odtl.Debit = (int)this.Jumlah;
                    }
                    else
                    {
                        odtl.Credit = (int)this.Jumlah;
                    }

                    cjDistribusi = true;
                }
            }

            if ( !cjBank)
            {
                fTransKasDtl odtlBank = new fTransKasDtl(Session);
                odtlBank.TransCode = this.Bank.AccountCode;
                odtlBank.Description = Description;
                if (this.Tipe == eBankTran.Pengeluaran)
                {
                    odtlBank.Credit = (int)this.Jumlah;
                }
                else
                {
                    odtlBank.Debit = (int)this.Jumlah;
                }

                this.TransKasDtl.Add(odtlBank);
            }
            if (!cjDistribusi)
            {
                fTransKasDtl odtlBank = new fTransKasDtl(Session);
                
                odtlBank.TransCode = TransCode;
                odtlBank.Description = Description;
                if (this.Tipe == eBankTran.Pengeluaran)
                {
                    odtlBank.Debit = (int)this.Jumlah;
                }
                else
                {
                    odtlBank.Credit = (int)this.Jumlah;
                }

                this.TransKasDtl.Add(odtlBank);
            }
        }
     protected override void OnDeleting()
     {
            try
            {
                KasbonDelete();
            }
            catch(Exception)
            { }
       base.OnDeleting();
     }

        public void KasbonDelete()
        {
            CriteriaOperator oCriteria = null;
            oCriteria = GroupOperator.And(oCriteria, new BinaryOperator("KasRef", this));

            KasBon oKasBon = Session.FindObject<KasBon>(oCriteria);
            try
            {
                if (oKasBon != null)
                {
                    oKasBon.Delete();
                    oKasBon.Save();
                }
            }
            catch(Exception)
            { }
            //oKasBon.Session.CommitTransaction();
        }

        public void KasbonUpdate()
        {
            CriteriaOperator oCriteria = null;
            oCriteria = GroupOperator.And(oCriteria, new BinaryOperator("KasRef", this));
            KasBon oKasBon = Session.FindObject<KasBon>(oCriteria);

            if (oKasBon != null)
            {
                if (Krani != null)
                {
                    oKasBon.Tanggal = this.Date;
                    oKasBon.Nama = Krani;
                    oKasBon.Jumlah = this.Jumlah;
                    oKasBon.KasRef = this;
                    if (Tipe == eBankTran.Pengeluaran)
                    {
                        oKasBon.Jenis = eDC.C;
                    }
                    else
                    {
                        oKasBon.Jenis = eDC.D;
                    }
                    oKasBon.Keterangan = Description;
                    oKasBon.Save();
                }
                else
                {
                    oKasBon.Delete();
                    oKasBon.Save();
                }
            }
            //else if (Oid>0)
            //{
            //    KasbonAdd();
            //}
                
            
        }
            public void KasbonAdd()
        {
            CriteriaOperator oCriteria = null;
            //string sqlQuery = string.Format($" KasRef.Reference == '{Reference}' ");
            ////oCriteria = CriteriaOperator.Parse(sqlQuery);
            oCriteria = GroupOperator.And(oCriteria, new BinaryOperator("KasRef", this));
            // IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(Person));
            //Session oSession = new Session();
            //KasBon objClass = KasBon.cre
            KasBon oKasBon = Session.FindObject<KasBon>(oCriteria);

            if ( Krani !=null )
            {
                // cari transaksi kas
               
                if (oKasBon == null)
                {
                    oKasBon = new KasBon(Session);
                    
                }
                oKasBon.Tanggal = this.Date;
                oKasBon.Nama = Krani;
                oKasBon.Jumlah = this.Jumlah;
                oKasBon.KasRef = this;
                if (Tipe == eBankTran.Pengeluaran)
                {
                    oKasBon.Jenis = eDC.C;
                }
                else
                {
                    oKasBon.Jenis = eDC.D;
                }
                oKasBon.Keterangan = Description;
                oKasBon.Save();
                //oKasBon.Session.CommitTransaction();

                //oKasBon.Session.CommitTransaction();
            }
            else
            {
                if (oKasBon != null)
                {
                    try
                    {
                        oKasBon.Delete();
                        oKasBon.Save();
                        oKasBon.Session.CommitTransaction();
                    }
                    catch (Exception e)
                    { }
                }
            }

        }
     // 
     private Bank _Bank; 
     [XafDisplayName("Kas/Bank"), ToolTip("Kas/Bank")]
        [RuleRequiredField(DefaultContexts.Save)]
        [ImmediatePostData]
        public virtual Bank Bank
     { 
       get { return _Bank; } 
       set { SetPropertyValue("Bank", ref _Bank, value);
                if (!IsSaving && !IsLoading && value != null)
                {
                    BankAccount(Jumlah);
                    TransAccount(Jumlah);
                }
            } 
     }
        private DateTime _Date;
        [XafDisplayName("Tanggal"), ToolTip("Tanggal")]
        [ModelDefault("DisplayFormat", "{0: dd/MM/yyyy}")]
        [ImmediatePostData]

        public override DateTime Date
        {
            get { return _Date; }
            set
            {
                SetPropertyValue("Date", ref _Date, value);
            }
        }
        private Karyawan _Krani;
        [XafDisplayName("Krani"), ToolTip("Krani")]
        ///[ModelDefault("DisplayFormat", "{0: dd/MM/yyyy}")]
        [ImmediatePostData]

        public virtual Karyawan Krani
        {
            get { return _Krani; }
            set
            {
                SetPropertyValue("Krani", ref _Krani, value);
            }
        }

        private int _CR;
        [XafDisplayName("Kredit"), ToolTip("Kredit")]
        ///[ModelDefault("DisplayFormat", "{0: dd/MM/yyyy}")]
        public virtual int CR
        {
            get {
                _CR = 0;
                if (!IsSaving && !IsLoading )
                {
                    if ( Tipe==eBankTran.Pengeluaran)
                    {
                        _CR =(int)Jumlah;
                    }
                }

                return _CR; }

        }
        private int _DR;
        [XafDisplayName("Debit"), ToolTip("Debit")]
        ///[ModelDefault("DisplayFormat", "{0: dd/MM/yyyy}")]
        public virtual int DR
        {
            get
            {
                _DR = 0;
                if (!IsSaving && !IsLoading)
                {
                    if (Tipe != eBankTran.Pengeluaran)
                    {
                        _DR = (int)Jumlah;
                    }
              
                }

                return _DR;
            }

        }
        //private DateTime _Date;
        ////[RuleRequiredField(DefaultContexts.Save)]
        //[XafDisplayName("Keterangan"), ToolTip("Keterangan")]
        //[ImmediatePostData]

        //public override string  Description
        //{
        //    get { return base.Description; }
        //    set
        //    {
        //        SetPropertyValue("Description", ref base.Description, value);
        //    }
        //}
        public virtual bool isBankAccount
        {
            get {
                bool bankAcc = false;
                try
                {
                    int disBank = TransKasDtl
                        .Where(sm => sm.TransCode.Code == Bank.AccountCode.Code).Count();
                    if (disBank > 0)
                    {
                        bankAcc = true;
                    }
                }
                catch (Exception e)
                {

                }
                return bankAcc;
            }
        }

        public virtual bool isBankAmount
        {
            get {
                bool bankAmount = false;
                double disBank = 0;
                try
                {
                    if (Tipe == eBankTran.Pengeluaran)
                    {
                        disBank = TransKasDtl
                           .Where(sm => sm.TransCode.Code == Bank.AccountCode.Code).Sum(x => x.Credit);
                    }
                    else
                    {
                        disBank = TransKasDtl
                           .Where(sm => sm.TransCode.Code == Bank.AccountCode.Code).Sum(x => x.Debit);
                    }

                    if (disBank == Jumlah)
                    {
                        bankAmount = true;
                    }
                }
                catch (Exception e)
                {

                }
                return bankAmount;
            }
        }
        // 
      private eBankTran _Tipe; 
     [XafDisplayName("Tipe"), ToolTip("Tipe")]
        [RuleRequiredField(DefaultContexts.Save)]
        [ImmediatePostData]
     public virtual eBankTran Tipe
     { 
       get { return _Tipe; } 
       set { SetPropertyValue("Tipe", ref _Tipe, value); 
             if( !IsSaving && !IsLoading &&  Bank !=null)
                {
                    BankAccount( Jumlah);
                    TransAccount(Jumlah);
                }
            
            } 
     }

        private fTransCode _TransCode;
        [XafDisplayName("Kode Trans"), ToolTip("Kode Trans")]
        [RuleRequiredField(DefaultContexts.Save)]
        //[ImmediatePostData]
        public virtual fTransCode TransCode
        {
            get { return _TransCode; }
            set
            {
                SetPropertyValue("TransCode", ref _TransCode, value);
                //if (!IsSaving && !IsLoading && Bank != null)
                //{
                //    //BankAccount(Jumlah);
                //    //TransAccount(Jumlah);
                //}

            }
        }


        public void KeteranganUpdate()
        {
            foreach (fTransKasDtl oTransDtl in TransKasDtl)
            {
                if (oTransDtl.Description=="")
                {
                    oTransDtl.Description = Description;
                    oTransDtl.Save();
                }
            }
        }
        public void TransAccount(double iJumlah)
        {
            try
            {
                if (!IsSaving && !IsLoading && Bank != null)
                {
                    if (UserUpdate.Perusahaan != null)
                    {
                        if (UserUpdate.Perusahaan.AccountPiutang != null)
                        {
                            if (Tipe == eBankTran.Pelanggan)
                            {
                                TransCode = UserUpdate.Perusahaan.AccountPiutang;
                                var disBank = TransKasDtl
                                .Where(sm => sm.TransCode == UserUpdate.Perusahaan.AccountPiutang).Count();

                                CriteriaOperator oCriteria = null;
                                oCriteria = GroupOperator.And(oCriteria, new BinaryOperator("TransKas", (fTransKas)this));
                                oCriteria = GroupOperator.And(oCriteria, new BinaryOperator("TransCode", UserUpdate.Perusahaan.AccountPiutang));
                                fTransKasDtl oTransKasDtl = Session.FindObject<fTransKasDtl>(oCriteria);
                                // if (oTransKasDtl==null)
                                if (disBank == 0)
                                {
                                    oTransKasDtl = new fTransKasDtl(Session);
                                    oTransKasDtl.TransCode = UserUpdate.Perusahaan.AccountPiutang;
                                    oTransKasDtl.Description = UserUpdate.Perusahaan.AccountPiutang.Description;

                                    if (iJumlah > 0)
                                    {
                                        oTransKasDtl.Credit = (int)iJumlah;

                                    }
                                    this.TransKasDtl.Add(oTransKasDtl);
                                }
                                else if (iJumlah > 0)
                                {
                                    foreach (fTransKasDtl oTransDtl in TransKasDtl)
                                    {
                                        if (oTransDtl.TransCode == UserUpdate.Perusahaan.AccountPiutang)
                                        {
                                            oTransDtl.Credit = (int)iJumlah;
                                            break;
                                            //oTransKasDtl.Save();
                                            //oTransKasDtl.Session.CommitTransaction();
                                        }
                                    }
                                }


                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
        }

        public void BankAccount(double iJumlah,bool isTransfer)
        {
            int disBank = TransKasDtl
               .Where(sm => sm.TransCode.Code == Bank.AccountCode.Code).Count();

            // var odisBank = this.TransKasDtl
            //.Where(sm => sm.TransCode.Code == Bank.AccountCode.Code).ToList();



            CriteriaOperator oCriteria = null;
            oCriteria = GroupOperator.And(oCriteria, new BinaryOperator("TransKas", (fTransKas)this));
            oCriteria = GroupOperator.And(oCriteria, new BinaryOperator("TransCode", Bank.AccountCode));
            fTransKasDtl oTransKasDtl = Session.FindObject<fTransKasDtl>(oCriteria);

            // CREATE ACCOUNT BANK
            if (disBank == 0) //(oTransKasDtl == null)
            {
                oTransKasDtl = new fTransKasDtl(Session);
                oTransKasDtl.TransCode = Bank.AccountCode;
                if (Description == "")
                {
                    oTransKasDtl.Description = Bank.AccountCode.Description;
                }
                else
                {
                    oTransKasDtl.Description = Description;
                }
                if (iJumlah > 0)
                {
                    if (Tipe == eBankTran.Pengeluaran)
                    {
                        oTransKasDtl.Credit = (int)iJumlah;
                    }
                    else if (Tipe == eBankTran.Pelanggan || Tipe == eBankTran.Penerimaan)
                    {
                        oTransKasDtl.Debit = (int)iJumlah;
                    }
                }
                this.TransKasDtl.Add(oTransKasDtl);

            }

            if (iJumlah > 0 && disBank > 0)
            {
                foreach (fTransKasDtl odtl in this.TransKasDtl)
                {
                    if (odtl.TransCode == Bank.AccountCode)
                    {
                        if (Tipe == eBankTran.Pengeluaran)
                        {
                            odtl.Credit = (int)iJumlah;
                        }
                        else if (Tipe == eBankTran.Pelanggan || Tipe == eBankTran.Penerimaan)
                        {
                            odtl.Debit = (int)iJumlah;
                        }
                    }
                }
                //oTransKasDtl.Save();
                //oTransKasDtl.Session.CommitTransaction();
            }

        }
        public void BankAccount(double iJumlah)
        {
            if (!IsSaving && !IsLoading && Bank != null) 
            {
                BankAccount(iJumlah,false);


            }
        }

    private double _Jumlah;
    [XafDisplayName("Jumlah"), ToolTip("Jumlah")]
    [ImmediatePostData]
        [RuleRequiredField(DefaultContexts.Save)]
        public virtual double Jumlah
        {
        get { return _Jumlah; }
        set { SetPropertyValue("Jumlah", ref _Jumlah, value);
                if (!IsSaving && !IsLoading && Bank != null)
                {

                    BankAccount((int)value);
                    TransAccount((int)value);
                }
            }
    }

    private string _Giro;
    [XafDisplayName("Giro/check"), ToolTip("Giro")]
    public virtual string Giro
        {
        get { return _Giro; }
        set { SetPropertyValue("Giro", ref _Giro, value); }
    }

        private DateTime _GiroTanggal;
        [ModelDefault("DisplayFormat", "{0: dd/MM/yyyy}")]
        [XafDisplayName("Tanggal Giro"), ToolTip("Tanggal Giro")]
        public virtual DateTime GiroTanggal
        {
            get { return _GiroTanggal; }
            set { SetPropertyValue("GiroTanggal", ref _GiroTanggal, value); }
        }

        // 
     private Pelanggan _Pelanggan; 
     [XafDisplayName("Pelanggan"), ToolTip("Pelanggan")] 
     public virtual Pelanggan Pelanggan
     { 
       get { return _Pelanggan; } 
       set { SetPropertyValue("Pelanggan", ref _Pelanggan, value); } 
     }

    private bool _Rekonsilasi;
    [XafDisplayName("Rekonsiliasi"), ToolTip("Rekonsiliasi")]
    public virtual bool Rekonsilasi
    {
        get
            {
                if (!IsLoading && !IsSaving)
                {
                    if ( Jumlah == BankAlokasi)
                    {
                        _Rekonsilasi = true;
                    }
                }
                return _Rekonsilasi; }
        set { SetPropertyValue("Rekonsilasi", ref _Rekonsilasi, value); }
    }

    [XafDisplayName("Sisa Alokasi"), ToolTip("Sisa Alokasi")]
    public virtual double SisaAlokas
    {
        get { return Jumlah- BankAlokasi; }
        //set { SetPropertyValue("Rekonsilasi", ref _Rekonsilasi, value); }
    }


        private double _BankAlokasi;
        [XafDisplayName("Bank Alokasi"), ToolTip("Bank Alokasi")]
        public virtual double BankAlokasi
{
            get
            {
                _BankAlokasi = 0;
                if (!IsLoading && !IsSaving )
                {
                    try
                    {
                        CriteriaOperator oCriteria = null;
                        oCriteria = GroupOperator.And(oCriteria, new BinaryOperator("Reference", this));

                        XPCollection<NotaPayment> oNotaPayment = new XPCollection<NotaPayment>(Session, oCriteria);
                        if (oNotaPayment.Count > 0)
                        {
                            // _BankAlokasi = (double)oNotaPayment.Where(x => x.Reference == this).Sum(x => x.JumlahBank);
                            _BankAlokasi = (double)oNotaPayment.Sum(x => x.JumlahBank);
                        }
                    }
                    catch (Exception e)
                    { }
                }

                return _BankAlokasi;
            }
            //set { SetPropertyValue("JmlColly", ref _JmlColly, value); } 
        }

        private  string RefNumber()
        {
            string sNum = "";
            string prefi = "";
            if (Bank != null)
            {
                prefi = Bank.ReferenceId;
            }
            if (prefi==null)
            {
                prefi = "";
            }
            XPCollection<BankTran> aTransKas = new XPCollection<BankTran>(Session);
            var list = from c in aTransKas
                       where ((c.Date.ToString("yyMM") == Date.ToString("yyMM"))  &&  c.Reference.StartsWith(prefi))
                       select c;
            try
            {
                
                if (list.Count() > 0)
                {
                    sNum = list.Select(x => x.Reference).Max();
                    sNum = sNum.Substring(sNum.Length - 5, 5);
                    int lasNo = System.Convert.ToInt32(sNum) + 1;
                    //int lasNo = System.Convert.ToInt32(sNum.Substring(sNum.Length - 1, 5)) + 1;
                    string sNumber = "00000".Substring(1, 5 - lasNo.ToString().Length) + lasNo.ToString();
                    sNum = $"{prefi}{Date.ToString("yyMM")}-{sNumber}";
                }
                else
                {
                    sNum = $"{prefi}{Date.ToString("yyMM")}-00001";
                }
            }
            catch (Exception e)
            {
                sNum = $"{prefi}{Date.ToString("yyMM")}-00001";
            }
            return sNum;
        }
        private UserLogin _UserUpdate;

        [XafDisplayName("UserUpdate"), ToolTip("UserUpdate")]
        public virtual UserLogin UserUpdate
        {
            get { return _UserUpdate; }
            set { SetPropertyValue("UserUpdate", ref _UserUpdate, value); }
        }

     public void CreateJournal(double iJumlah, fTransCode oTransCode)
        {
            CriteriaOperator oCriteria = null;
            
            if (oTransCode != null)
            {
                //oCriteria = null;
                //oCriteria = GroupOperator.And(oCriteria, new BinaryOperator("TransKas", (fTransKas)this));
                //oCriteria = GroupOperator.And(oCriteria, new BinaryOperator("TransCode", oTransCode));
                //fTransKasDtl oTransKasDtlTran = Session.FindObject<fTransKasDtl>(oCriteria);

                string sqlQuery = string.Format(" Oid == {0} ", oTransCode.Oid.ToString());
                CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);
                fTransCode ovTransCode = Session.FindObject<fTransCode>(filterOperator);
                

                //oCriteria = null;
                //oCriteria = GroupOperator.And(oCriteria, new BinaryOperator("TransCode", oTransCode));
                //fTransCode ovTransCode = Session.FindObject<fTransCode>(oCriteria);

                //oTransKasDtlTran = new fTransKasDtl(Session);
                //oTransKasDtlTran.TransCode = ovTransCode;
                //oTransKasDtlTran.Description = Description;
                int i = 0;
                foreach (fTransKasDtl odtl in this.TransKasDtl)
                {
                    if (odtl.TransCode.Oid == oTransCode.Oid)
                    {
                        i += 1;
                        if (iJumlah > 0)
                        {
                            if (Tipe == eBankTran.Pengeluaran)
                            {
                                odtl.Debit = (int)iJumlah;
                            }
                            else if (Tipe == eBankTran.Pelanggan || Tipe == eBankTran.Penerimaan)
                            {
                                odtl.Credit = (int)iJumlah;
                            }
                        }
                    }
                }
                if (i==0)
                {
                    fTransKasDtl oTransKasDtlTran = new fTransKasDtl(Session);
                    oTransKasDtlTran.TransCode = ovTransCode;
                    oTransKasDtlTran.Description = Description;
                    if (iJumlah > 0)
                    {
                        if (Tipe == eBankTran.Pengeluaran)
                        {
                            oTransKasDtlTran.Debit = (int)iJumlah;
                        }
                        else if (Tipe == eBankTran.Pelanggan || Tipe == eBankTran.Penerimaan)
                        {
                            oTransKasDtlTran.Credit = (int)iJumlah;
                        }
                    }
                    this.TransKasDtl.Add(oTransKasDtlTran);
                    
                }
            }
        }
        private double _Saldo;
        public virtual double Saldo
        {
            get
            {
                _Saldo = 0;
                DateTime qTgl= Date;
                if (!IsLoading && !IsSaving)
                {
                    if (this.Bank != null)
                    {
                        try
                        {
                            string sqlQuery = string.Format($" Bank.Oid == {Bank.Oid} ");// &&  Status != {eTagihan.Kolektor.GetHashCode()}  ");
                            CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);
                            
                            XPCollection<BankTran> oBankTran = new XPCollection<BankTran>(Session,filterOperator);
                            if (oBankTran.Count > 0 && Bank != null)
                            {
                                // double debit = (double)oBankTran.Where(x => x.isBalance == true && x.Bank == Bank && x.Tipe!=eBankTran.Pengeluaran && x.Date <= qTgl).Sum(x => x.Jumlah);
                                // double credit = (double)oBankTran.Where(x => x.isBalance == true && x.Bank == Bank && x.Tipe == eBankTran.Pengeluaran && x.Date <= qTgl).Sum(x => x.Jumlah);
                                _Saldo = (double)oBankTran.Where(x => x.isBalance == true && x.Date <= qTgl).Sum(x => (x.Tipe == eBankTran.Pengeluaran ? x.Jumlah * -1 : x.Jumlah));
                                //_Saldo = debit - credit;
                            }
                        }
                        catch (Exception e)
                        { }
                    }
                }

                return _Saldo;
            }
            //set { SetPropertyValue("JmlColly", ref _JmlColly, value); } 
        }
    } 
   public enum eBankTran
    {
        [XafDisplayName("Penerimaan")]
        Penerimaan = 0,
        [XafDisplayName("Pengeluaran")]
        Pengeluaran =1,
        [XafDisplayName("Pelanggan")]
        Pelanggan = 2
        //[XafDisplayName("Kasbon")]
        //Kasbon = 3,
        //[XafDisplayName("Penerimaan Kasbon")]
        //PenerimaanKasbon = 4
        //[XafDisplayName("Komisi")]
        //Komisi = 3 
    } 
} 
