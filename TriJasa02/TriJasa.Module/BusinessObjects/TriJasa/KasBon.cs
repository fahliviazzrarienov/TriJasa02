// Class Name : KasBon.cs 
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

namespace TriJasa.Module.BusinessObjects
{ 
   [DefaultClassOptions] 
   [ImageName("ModelEditor_Views")] 
   [DefaultProperty("ID")]
   [NavigationItem("Transaksi")]
   // Standard Document
   [System.ComponentModel.DisplayName("Kasbon & Klaim")]
   public class KasBon : XPObject
   {
     public KasBon() : base()
     {
     // This constructor is used when an object is loaded from a persistent storage.
     // Do not place any code here.
     }
     public KasBon(Session session) : base(session) 
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
        User = Session.FindObject<UserLogin>(new BinaryOperator("UserName", tUser));
       Tanggal = new DateTime(System.DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        } 
     protected override void OnSaving()
     {
            if (ReferensiNo == "" || ReferensiNo == null)
            {
                ReferensiNo = RefNumber();
            }
            base.OnSaving();
     }
        private string RefNumber()
        {
            string sNum = "";
            string prefi = "";
            if (Nama != null)
            {
                prefi = "K"+Nama.Oid;
            }
            if (prefi == null)
            {
                prefi = "";
            }
            XPCollection<KasBon> aKasBon = new XPCollection<KasBon>(Session);
            var list = from c in aKasBon
                       where ((c.Tanggal.ToString("yyMM") == Tanggal.ToString("yyMM")) && c.ReferensiNo.StartsWith(prefi))
                       select c;
            try
            {

                if (list.Count() > 0)
                {
                    sNum = list.Select(x => x.ReferensiNo).Max();
                    sNum = sNum.Substring(sNum.Length - 5, 5);
                    int lasNo = System.Convert.ToInt32(sNum) + 1;
                    //int lasNo = System.Convert.ToInt32(sNum.Substring(sNum.Length - 1, 5)) + 1;
                    string sNumber = "00000".Substring(1, 5 - lasNo.ToString().Length) + lasNo.ToString();
                    sNum = $"{prefi}{Tanggal.ToString("yyMM")}-{sNumber}";
                }
                else
                {
                    sNum = $"{prefi}{Tanggal.ToString("yyMM")}-00001";
                }
            }
            catch (Exception e)
            {
                sNum = $"{prefi}{Tanggal.ToString("yyMM")}-00001";
            }
            return sNum;
        }
        protected override void OnDeleting()
     {
       base.OnDeleting();
     } 
     // 
     private DateTime _Tanggal; 
     [XafDisplayName("Tanggal"), ToolTip("Tanggal")] 
     public virtual DateTime Tanggal
     { 
       get { return _Tanggal; } 
       set { SetPropertyValue("Tanggal", ref _Tanggal, value); } 
     } 
     // 
     private eDC _Jenis; 
     [XafDisplayName("Debit/Credit"), ToolTip("Debit/Credit")] 
     public virtual eDC Jenis
     { 
       get { return _Jenis; } 
       set { SetPropertyValue("Jenis", ref _Jenis, value); } 
     } 
     // 
     private string _ReferensiNo; 
     [XafDisplayName("No Referensi"), ToolTip("No Referensi")] 
     [Size(30)] 
     public virtual string ReferensiNo
     { 
       get { return _ReferensiNo; } 
       set { SetPropertyValue("ReferensiNo", ref _ReferensiNo, value); } 
     } 
     // 
     private Karyawan _Nama; 
     [XafDisplayName("Nama"), ToolTip("Nama")] 
     public virtual Karyawan Nama
     { 
       get { return _Nama; } 
       set { SetPropertyValue("Nama", ref _Nama, value); } 
     } 
     // 
     private string _Keterangan; 
     [XafDisplayName("Keterangan"), ToolTip("Keterangan")] 
     [Size(500)] 
     public virtual string Keterangan
     { 
       get { return _Keterangan; } 
       set { SetPropertyValue("Keterangan", ref _Keterangan, value); } 
     } 
     // 
     private double _Jumlah; 
     [XafDisplayName("Jumlah"), ToolTip("Jumlah")] 
     public virtual double Jumlah
     { 
       get { return _Jumlah; } 
       set { SetPropertyValue("Jumlah", ref _Jumlah, value); } 
     }
        private BankTran _KasRef;
        [XafDisplayName("KasRef"), ToolTip("KasRef")]
        public virtual BankTran KasRef
        {
            get { return _KasRef; }
            set { SetPropertyValue("KasRef", ref _KasRef, value); }
        }
        // 
     private UserLogin _User; 
     [XafDisplayName("User"), ToolTip("User")] 
     public virtual UserLogin User
     { 
       get { return _User; } 
       set { SetPropertyValue("User", ref _User, value); } 
     } 
   } 
   
} 
