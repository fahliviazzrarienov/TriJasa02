// Class Name : fTransKas.cs 
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
using DevExpress.CodeParser;
using DevExpress.ExpressApp.Model;

namespace TriJasa.Module.BusinessObjects
{
    [DefaultClassOptions]
    [ImageName("ModelEditor_Views")]
    [DefaultProperty("Reference")]
    [NavigationItem("Transaksi")]
    // Standard Document
    [System.ComponentModel.DisplayName("Transaksi Kas")]

   // [RuleCriteria("TrasKasSave", DefaultContexts.Save, " isBalance ==true",
   //"Debit & Credit Harus sama", SkipNullOrEmptyValues = false)]
    public class fTransKas : XPObject
    {
        public fTransKas() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }
        public fTransKas(Session session) : base(session)
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
            Date = new DateTime( DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            CashType = eCashType.Penerimaan;
            Description = "";
            //LastUpdateUser = Session.FindObject<GPUser>(new BinaryOperator("UserName", SecuritySystem.CurrentUserName.ToString())); 
            // LastUpdateUser = Session.FindObject<GPUser>(new BinaryOperator("UserName", tUser)); 
            // LastUpdate = DateTime.Now; 
        }
        protected override void OnSaving()
        {
            if (Reference == "" || Reference== null)
            {
                Reference = RefNumber();
            }
            // UpdateDetailH(true);
            base.OnSaving();
           
        }
        protected override void OnDeleting()
        {
            //XPCollection colDelete = new XPCollection(typeof(YourPersistentType), new BinaryOperator("YourPropertyName", true)); // Objects for deletion.  
            Session.Delete(TransKasDtl);
            Session.Save(TransKasDtl); 
             base.OnDeleting();
        }
        // 
        //private eCashType _CashType;
        //[XafDisplayName("CashType"), ToolTip("CashType")]
        //public virtual eCashType CashType
        //{
        //    get { return _CashType; }
        //    set { SetPropertyValue("CashType", ref _CashType, value); }

        //}
        // 
        private DateTime _Date;
        [ModelDefault("DisplayFormat", "{0: dd/MM/yyyy}")]
        [XafDisplayName("Tanggal"), ToolTip("Tanggal")]
        [ImmediatePostData]

        public virtual DateTime Date
        {
            get { return _Date; }
            set
            {
                SetPropertyValue("Date", ref _Date, value);
                //if (value != null && !IsLoading && !IsSaving && Date == null && ApplyTran == false)
                //{
                //  //  GetTotalToday();
                //    ApplyTran = true;
                //}
            }
        }

        
     [Appearance("txdDebitisBalance", Enabled = false,
    Visibility = ViewItemVisibility.Show, Criteria = " Debit == Credit && Debit>0 &&  Credit >0 ", FontColor = "green")]
        [Appearance("txdDebitisBalancered", Enabled = false,
    Visibility = ViewItemVisibility.Show, Criteria = " Debit != Credit ", FontColor =  "Red")]
        public virtual bool isBalance 
        {  
            get {

                bool bbalance = false;
                int oDebit = 0;
                int oCredit = 0;

                //var Glist = from c in TransKasDtl
                //                //where ((c.TimeOut >= fdate & c.TimeOut < tdate)
                //                //         & c.Operator == Operator)
                //                // group c by new { DC = (c.DC) } into cc //, mbr =( c.Member != null ? "Yes" :"No" ).ToString()  c.VechType,
                //                // orderby cc.Key.mbr
                //                // select new { DC = cc.Key.DC, Amount = cc.Sum(c1 => c1.Amount) }; // Member = cc.Key.mbr,  // , amt =cc.Sum( c1 =>c1.TotPrice)
                //            select c;
                //Debit = Glist.Select(x => x.Debit).Sum();
                //Credit = Glist.Select(x => x.Credit).Sum();

                //oDebit = Glist.Sum(x => x.Debit);
                //oCredit = Glist.Sum(x => x.Credit);

                if (Debit == Credit && Debit> 0 && Credit>0)
                {
                    bbalance = true;
                }

                return bbalance;
            }
        }
        // 
        private bool _ApplyTran;
        [XafDisplayName("ApplyTran"), ToolTip("ApplyTran")]
        [Appearance("txnKasApplyTran", Enabled = false, Visibility = ViewItemVisibility.Hide)]
        public virtual bool ApplyTran
        {
            get { return _ApplyTran; }
            set { SetPropertyValue("ApplyTran", ref _ApplyTran, value); }
        }
        private string _Reference;
        //[RuleRequiredField(DefaultContexts.Save)]
        [XafDisplayName("No Referensi"), ToolTip("No Referensi")]
        [Appearance("txnKasReference", Enabled = false, Visibility = ViewItemVisibility.Show)]
        public virtual string Reference
        {
            get {
                //if (!IsLoading && !IsSaving)
                //{
                //    _Reference = RefNumber();
                //}
                return _Reference; }
            set { SetPropertyValue("Reference", ref _Reference, value); }
        }

        protected  string RefNumber()
        {
            string sNum="";
            XPCollection<fTransKas> aTransKas = new XPCollection<fTransKas>(Session);
            var list = from c in aTransKas
                       where (c.Date.ToString("yyyyMM")== Date.ToString("yyyyMM"))
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
                    sNum = $"JV{Date.ToString("yyyyMM")}-{sNumber}";
                }
                else
                {
                    sNum = $"JV{Date.ToString("yyyyMM")}-00001";
                }
            }
            catch (Exception e)
            {
                sNum = $"JV{Date.ToString("yyyyMM")}-00001";
            }
            return sNum;
        }

        private string _Description;
        [RuleRequiredField(DefaultContexts.Save)]
        [XafDisplayName("Keterangan"), ToolTip("Keterangan")]
        
        public virtual string Description
        {
            get { return _Description; }
            set { SetPropertyValue("Description", ref _Description, value); }
        }

        //private int _BankTransfer;
        //[XafDisplayName("BankTransfer"), ToolTip("BankTransfer")]
        //public virtual int BankTransfer
        //{
        //    get { return _BankTransfer; }
        //    set { SetPropertyValue("BankTransfer", ref _BankTransfer, value); }
        //}

        private void GetTotalToday()
        {
            DateTime oDate = Date;
            DateTime fdate = new DateTime(oDate.Year, oDate.Month, oDate.Day, 0, 1, 1);
            DateTime tdate = fdate.AddDays(1);
            DateTime shift = fdate.AddHours(15);
            //XPQuery<fParkIn> qParkIn = Session.Query<fParkIn>();
            //var list = from f in qParkIn
            //           where (f.TimeEntry >= fdate & f.TimeEntry < tdate)
            //           // & f.Operator == Operator)
            //           //orderby f.Oid
            //           select f;
            //int iKendaraanMasuk = list.Count();
            //int Shift1 = (from x in list where x.TimeEntry < shift select x.NoTrans).Count();
            //int Shift2 = (from x in list where x.TimeEntry >= shift select x.NoTrans).Count();

            //// add to detail
            //fTransKasDtl odetail = new fTransKasDtl(Session);
            //odetail.Description = "SHIFT I : " + Shift1.ToString();
            //odetail.DC = eDC.D;
            //odetail.TransCode = GetTranCode(eTransCode.Shift1);
            //odetail.TransKas = this;
            //odetail.Save();
            //odetail = new fTransKasDtl(Session);
            //odetail.Description = "SHIFT 2 : " + Shift2.ToString();
            //odetail.DC = eDC.D;
            //odetail.TransCode = GetTranCode(eTransCode.Shift2);
            //odetail.TransKas = this;
            //odetail.Save();

            //  odetail.

        }
        private fTransCode GetTranCode(eTransCode oCashType)
        {
            string sqlQuery = string.Format(" Type == {0}  ", oCashType.ToString());
            CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);
            fTransCode oTransCode = Session.FindObject<fTransCode>(filterOperator);
            return oTransCode;

        }

        private eCashType CashType;
        private void CheckData()
        {
            // penerimaan
            if (CashType == eCashType.Penerimaan)
            {
                //1.check apakah data sudah ada
                try
                {
                    DateTime fdate = new DateTime(Date.Year, Date.Month, Date.Day);
                    DateTime tdate = fdate.AddDays(1);
                    string sqlQuery = string.Format(" CashType == 0 && Date >= #{0}# & Date < #{1}# ", fdate.ToString(), tdate.ToString());
                    CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);
                    fTransKas oTransKas = Session.FindObject<fTransKas>(filterOperator);
                    if (oTransKas != null)
                    {
                        this.Oid = oTransKas.Oid;
                        this.Date = oTransKas.Date;
                    }
                }
                catch (Exception e)
                {

                }
                //2.update data dari transaksi in
                //3.hitung shift 1 & shift 2
            }
        }
        // 
        [XafDisplayName("TranCash"), ToolTip("TranCash")]
        [Association("TranCash"), DevExpress.Xpo.Aggregated]
        [ImmediatePostData]
        public XPCollection<fTransKasDtl> TransKasDtl
        {
            get
            {
                return GetCollection<fTransKasDtl>("TransKasDtl");
            }
        }

        private int _Debit;
        [XafDisplayName("Dr"), ToolTip("Dr")]
        [Appearance("txnKasDebit", Enabled = false, Visibility = ViewItemVisibility.Show)]
        public virtual int Debit
        {
       
            get
            {
                _Debit = 0;

                if (!IsLoading && !IsSaving && this.TransKasDtl != null)
                {
                    try
                    {
                        if (this.TransKasDtl.Count > 0)
                        {
                            _Debit = (int)this.TransKasDtl.Sum(x => x.Debit);
                            //TotalTagihan = _TotalJml;
                        }
                    }
                    catch (Exception e)
                    { }
                }
                return _Debit;
            }

            //set { SetPropertyValue("Debit", ref _Debit, value); }



        }
        private int _Credit;
        [XafDisplayName("Cr"), ToolTip("Cr")]
        [Appearance("txnKasCredit", Enabled = false, Visibility = ViewItemVisibility.Show)]
        public virtual int Credit
        {
            get {
                _Credit = 0;
                if (!IsLoading && !IsSaving && this.TransKasDtl != null)
                {
                    try
                    {
                        if (this.TransKasDtl.Count > 0)
                        {
                            _Credit = (int)this.TransKasDtl.Sum(x => x.Credit);
                            //TotalTagihan = _TotalJml;
                        }
                    }
                    catch (Exception e)
                    { }
                }
                return _Credit; 
               }
            //set { SetPropertyValue("Credit", ref _Credit, value); }

        }

        private int _Selisih;
        [XafDisplayName("Selisih"), ToolTip("Selisih")]
        [PersistentAlias("Debit-Credit")]
        public virtual int Selisih
        {
            get
            {
                _Selisih = 0;
                if (!IsLoading && !IsSaving && this.TransKasDtl != null)
                {
                    try
                    {
                        if (this.TransKasDtl.Count > 0)
                        {
                            _Selisih = (int)this.TransKasDtl.Sum(x => x.Debit- x.Credit);
                            if (_Selisih <0)
                            {
                                _Selisih = _Selisih * -1;
                            }

                        }
                    }
                    catch (Exception e)
                    { }
                }
                return _Selisih;
            }
            //set { SetPropertyValue("TotalBiayaLain", ref _TotalBiayaLain, value); }
        }

        //private int fDebit;
        ////[Persistent("Debit")]
        ////[PersistentAlias(nameof(fDebit))]
        //[Appearance("khDebit", Visibility = ViewItemVisibility.Hide)]
        //public int? Debit
        //{
        //    get
        //    {
        //        if (!IsLoading && !IsSaving && fDebit == null)
        //        {
        //            UpdateDetail(false);
        //        }
        //        return fDebit;
        //    }
        //}
        //private int? fCredit = null;
        ////[Persistent("Credit")]
        ////[PersistentAlias(nameof(fCredit))]
        //[Appearance("khCredit", Visibility = ViewItemVisibility.Hide)]
        //public int? Credit
        //{
        //    get
        //    {
        //        if (!IsLoading && !IsSaving && fCredit == null)
        //        {
        //            UpdateDetail(false);
        //        }
        //        return fCredit;
        //    }
        //}

        //private int Debit;
        //private int Credit;
        public void UpdateDetailH(bool forceChangeEvents)
        {
            int oldDebit = Debit;
            int oldCredit = Credit;
            int tempDebit = 0;
            int tempCredit = 0;

            
            var Glist = from c in TransKasDtl
                            //where ((c.TimeOut >= fdate & c.TimeOut < tdate)
                            //         & c.Operator == Operator)
                            // group c by new { DC = (c.DC) } into cc //, mbr =( c.Member != null ? "Yes" :"No" ).ToString()  c.VechType,
                            // orderby cc.Key.mbr
                            // select new { DC = cc.Key.DC, Amount = cc.Sum(c1 => c1.Amount) }; // Member = cc.Key.mbr,  // , amt =cc.Sum( c1 =>c1.TotPrice)
                        select c;
            if (Glist.Count() > 0)
            {
                //Debit = Glist.Select(x => x.Debit).Sum();
                //Credit = Glist.Select(x => x.Credit).Sum();
            }
            //foreach (var detail in Glist)
            //{
            //    if (detail.DC == eDC.D)
            //    {
            //        tempDebit += detail.Amount;
            //    }
            //    else
            //    {
            //        tempCredit += detail.Amount;
            //    }
            //}

            //Debit = tempDebit;
            //Credit = tempCredit;
            //if (forceChangeEvents)
            //{
            //    OnChanged(nameof(Debit), oldDebit, _Debit);
            //    OnChanged(nameof(Credit), oldCredit, _Credit);
            //}
        }
        //public void UpdateDetail(bool forceChangeEvents)
        //{
        //    int? oldDebit = fDebit;
        //    int? oldCredit = fCredit;
        //    int tempDebit = 0;
        //    int tempCredit = 0;
        //    foreach (fTransKasDtl detail in TransKasDtl)
        //    {
        //        if (detail.DC == eDC.D)
        //        {
        //            tempDebit += detail.Amount;
        //        }
        //        else
        //        {
        //            tempCredit += detail.Amount;
        //        }
        //    }
        //    fDebit = tempDebit;
        //    fCredit = tempCredit;
        //    if (forceChangeEvents)
        //    {
        //        OnChanged(nameof(Debit), oldDebit, fDebit);
        //        OnChanged(nameof(Credit), oldCredit, fCredit);
        //    }
        //}
        protected override void OnLoaded()
        {
           // Reset();
            base.OnLoaded();
        }
        private void Reset()
        {
            //Debit = 0;
            //Credit = 0;

        }
    }
    public enum eCashType
    {
        [XafDisplayName("Penerimaan")]
        Penerimaan = 0,
        [XafDisplayName("Pengeluran")]
        Pengeluran = 1

    }

}
