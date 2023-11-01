// Class Name : BankTrasfer.cs 
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
   [DefaultProperty("ID")]
   [NavigationItem("Transaksi")]
   // Standard Document
   [System.ComponentModel.DisplayName("BK Mutasi")]
   public class BankTrasfer : XPObject
    {
     public BankTrasfer() : base()
     {
     // This constructor is used when an object is loaded from a persistent storage.
     // Do not place any code here.
     }
     public BankTrasfer(Session session) : base(session) 
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
            User = Session.FindObject<UserLogin>(new BinaryOperator("UserName", tUser));
            Tanggal = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day);
        } 
     protected override void OnSaving()
     {
            string tUser = SecuritySystem.CurrentUserName.ToString();
            User = Session.FindObject<UserLogin>(new BinaryOperator("UserName", tUser));
            if (Reference == "" || Reference == null)
            {
                Reference = RefNumber();
            }
            //Transfer();
            Transfer();
            Edit();
            base.OnSaving();
        

        }


        protected override void OnSaved()
        {
            //string tUser = SecuritySystem.CurrentUserName.ToString();
            //User = Session.FindObject<UserLogin>(new BinaryOperator("UserName", tUser));
           
            base.OnSaved();
            /// Transfer();

        }
        protected override void OnDeleting()
     {
       base.OnDeleting();
     } 
     // 
     public void Edit()
        {
            CriteriaOperator filterOperator;
            string sqlQuery;
            sqlQuery = string.Format(" Oid == {0} ", this.KeBankRef.Oid.ToString());
            filterOperator = CriteriaOperator.Parse(sqlQuery);
            BankTran oBankKe = Session.FindObject<BankTran>(filterOperator);

            sqlQuery = string.Format(" Oid == {0} ", this.DariBankRef.Oid.ToString());
            filterOperator = CriteriaOperator.Parse(sqlQuery);
            BankTran oBankDari = Session.FindObject<BankTran>(filterOperator);
            if (oBankKe != null)
            {
                if (oBankKe.Bank != BankKe)
                {
                    this.KeBankRef = oBankDari;
                    oBankDari.Tipe = eBankTran.Penerimaan;
                    oBankDari.CreateJournal();
                    oBankDari.Save();
                    //oBankDari.Session.CommitTransaction();

                    this.DariBankRef = oBankKe;
                    oBankKe.Tipe = eBankTran.Pengeluaran;
                    oBankKe.CreateJournal();
                    oBankKe.Save();
                    //oBankKe.Session.CommitTransaction();
                    // oBankKe.Bank= BankDari;



                }
            }
            //sqlQuery = string.Format(" Oid == {0} ", this.Oid.ToString());
            //filterOperator = CriteriaOperator.Parse(sqlQuery);
            //BankTrasfer oBT = Session.FindObject<BankTrasfer>(filterOperator);



        }
     private Bank _BankDari; 
     [XafDisplayName("Dari Kas/Bank"), ToolTip("Dari Kas/Bank")] 
     public virtual Bank BankDari
     { 
       get { return _BankDari; } 
       set { SetPropertyValue("BankDari", ref _BankDari, value); } 
     } 
     // 
     private Bank _BankKe; 
     [XafDisplayName("Ke  Kas/Bank"), ToolTip("Ke  Kas/Bank")] 
     public virtual Bank BankKe
     { 
       get { return _BankKe; } 
       set { SetPropertyValue("BankKe", ref _BankKe, value); } 
     } 
     // 
     private DateTime _Tanggal;
        [ModelDefault("DisplayFormat", "{0: dd/MM/yyyy}")]
        [XafDisplayName("Tanggal"), ToolTip("Tanggal")] 
     public virtual DateTime Tanggal
     { 
       get { return _Tanggal; } 
       set { SetPropertyValue("Tanggal", ref _Tanggal, value); } 
     } 
     // 
     private double _Jumlah; 
     [XafDisplayName("Jumlah"), ToolTip("Jumlah")] 
     public virtual double Jumlah
     { 
       get { return _Jumlah; } 
       set { SetPropertyValue("Jumlah", ref _Jumlah, value); } 
     } 

     // 
     private string _Keterangan; 
     [XafDisplayName("Keterangan"), ToolTip("Keterangan")] 
     [Size(50)] 
     public virtual string Keterangan
     { 
       get { return _Keterangan; } 
       set { SetPropertyValue("Keterangan", ref _Keterangan, value); } 
     }
        private BankTran _DariBankRef;
        [XafDisplayName("DariBankRef"), ToolTip("Keterangan")]
        [Appearance("BankTransferDariBankRefEnable", Enabled = false)]
        public virtual BankTran DariBankRef
        {
            get { return _DariBankRef; }
            set { SetPropertyValue("DariBankRef", ref _DariBankRef, value); }
        }
        private BankTran _KeBankRef;
        [XafDisplayName("KeBankRef"), ToolTip("KeBankRef")]
        [Appearance("BankTransferKeBankRefEnable", Enabled = false)]
        public virtual BankTran KeBankRef
        {
            get { return _KeBankRef; }
            set { SetPropertyValue("KeBankRef", ref _KeBankRef, value); }
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
        private string _Reference;
        //[RuleRequiredField(DefaultContexts.Save)]
        [XafDisplayName("No Referensi"), ToolTip("No Referensi")]
        [Appearance("BankTransferReferenceEnable", Enabled = false, Visibility = ViewItemVisibility.Show)]
        public virtual string Reference
        {
            get
            {
                //if (!IsLoading && !IsSaving)
                //{
                //    _Reference = RefNumber();
                //}
                return _Reference;
            }
            set { SetPropertyValue("Reference", ref _Reference, value); }
        }
        private string RefNumber()
        {
            string sNum = "";
            XPCollection<BankTrasfer> aTransKas = new XPCollection<BankTrasfer>(Session);
            var list = from c in aTransKas
                       where (c.Tanggal.ToString("yyMM") == Tanggal.ToString("yyMM"))
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
                    sNum = $"TR{Tanggal.ToString("yyMM")}-{sNumber}";
                }
                else
                {
                    sNum = $"TR{Tanggal.ToString("yyMM")}-00001";
                }
            }
            catch (Exception e)
            {
                sNum = $"TR{Tanggal.ToString("yyMM")}-00001";
            }
            return sNum;
        }

        public void VoidBank (BankTrasfer oBankTrasfer)
        {

            CriteriaOperator filterOperator;
            string sqlQuery;
            sqlQuery = string.Format(" Oid == {0} ", oBankTrasfer.BankKe.Oid.ToString());
            filterOperator = CriteriaOperator.Parse(sqlQuery);
            Bank oBankDari = Session.FindObject<Bank>(filterOperator);

            sqlQuery = string.Format(" Oid == {0} ", oBankTrasfer.BankDari.Oid.ToString());
            filterOperator = CriteriaOperator.Parse(sqlQuery);
            Bank oBankKe = Session.FindObject<Bank>(filterOperator);

            sqlQuery = string.Format(" Oid == {0} ", oBankTrasfer.Oid.ToString());
            filterOperator = CriteriaOperator.Parse(sqlQuery);
            BankTrasfer oBT = Session.FindObject<BankTrasfer>(filterOperator);

            oBT.Hapus = true;
            oBT.Save();
            // Hapus = true;
            BankDari = oBankDari;
            BankKe = oBankKe;
            Tanggal = oBankTrasfer.Tanggal;
            Jumlah = oBankTrasfer.Jumlah;
            Keterangan = $"#Delete# {oBankTrasfer.Keterangan}";
            Giro = oBankTrasfer.Giro;
            GiroTanggal = oBankTrasfer.GiroTanggal;
        }
        public void Transfer()
        {
            // get account .

            fTransCode oBankTransferAcc;
            BankTran oBankFrom;
            BankTran oBankTo;


                if (User.Perusahaan.BankTransfer != null)
                {
                    //string sqlQuery = string.Format(" Oid == {0} ", User.Perusahaan.BankTransfer.Oid.ToString());
                    //CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);
                    //oBankTransferAcc = Session.FindObject<fTransCode>(filterOperator);
                     oBankTransferAcc= User.Perusahaan.BankTransfer;
                }
                if (DariBankRef == null)
                {
                     oBankFrom = new BankTran(Session);
                    oBankFrom.Description = Keterangan;
                    oBankFrom.Date = Tanggal;
                    oBankFrom.Giro = Giro;
                    oBankFrom.GiroTanggal = GiroTanggal;
                    oBankFrom.Tipe = eBankTran.Pengeluaran;
                    oBankFrom.Jumlah = Jumlah;
                    oBankFrom.Bank = BankDari;
                    oBankFrom.TransCode = BankDari.AccountCode;
                    oBankFrom.BankAccount(Jumlah,true);
                    oBankTransferAcc = User.Perusahaan.BankTransfer;
                    oBankFrom.CreateJournal(Jumlah, oBankTransferAcc);
                    DariBankRef = oBankFrom;
                    oBankFrom.Save();
                    //oBankFrom.Session.CommitTransaction();
                   
                }
                else if  (DariBankRef.Oid>0)
                {
                string sqlQuery = string.Format(" Oid == {0} ", DariBankRef.Oid.ToString());
                CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);
                oBankFrom = Session.FindObject<BankTran>(filterOperator);
                oBankFrom.Jumlah = Jumlah;
                oBankFrom.Description = Keterangan;
                oBankFrom.Save();
                //oBankFrom.Session.CommitTransaction();

               }
                if (KeBankRef == null)
                {
                     oBankTo = new BankTran(Session);
               
                oBankTo.Description = Keterangan;
                oBankTo.Date = Tanggal;
                oBankTo.Giro = Giro;
                oBankTo.GiroTanggal = GiroTanggal;
                oBankTo.Tipe = eBankTran.Penerimaan;
                oBankTo.Jumlah = Jumlah;
                oBankTo.Bank = BankKe;
                oBankTo.TransCode = BankKe.AccountCode;
                oBankTo.BankAccount(Jumlah,true);
                oBankTransferAcc = User.Perusahaan.BankTransfer;
                oBankTo.CreateJournal(Jumlah, oBankTransferAcc);
                KeBankRef = oBankTo;
                oBankTo.Save();
                //Session.CommitTransaction();
                    //oBankTo.Session.CommitTransaction();
                  
                }
                else if (KeBankRef.Oid > 0)
                {
                    string sqlQuery = string.Format(" Oid == {0} ", KeBankRef.Oid.ToString());
                    CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);
                    oBankTo = Session.FindObject<BankTran>(filterOperator);
                    oBankTo.Jumlah = Jumlah;
                    oBankTo.Description = Keterangan;
                    oBankTo.Save();
                   // oBankFrom.Session.CommitTransaction();
            }

        }
        private bool _Hapus;
        [XafDisplayName("Delete"), ToolTip("Delete")]
        [Appearance("BankTrasferDeleteEnable", Enabled = false)]
        public virtual bool Hapus
        {
            get { return _Hapus; }
            set { SetPropertyValue("Hapus", ref _Hapus, value); }
        }

        // 
        private UserLogin _User; 
     [XafDisplayName("User"), ToolTip("User")]
     [Appearance("BankTrasferUserEnable", Enabled = false)]
        public virtual UserLogin User
     { 
       get { return _User; } 
       set { SetPropertyValue("User", ref _User, value); } 
     } 
   } 
  
} 
