// Class Name : fTransCode.cs 
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
   [DefaultProperty("CodeKeterangan")]
   [NavigationItem("Master")]
   // Standard Document
   [System.ComponentModel.DisplayName("Account Code")]
   public class fTransCode : XPObject
   {
     public fTransCode() : base()
     {
     // This constructor is used when an object is loaded from a persistent storage.
     // Do not place any code here.
     }
     public fTransCode(Session session) : base(session) 
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
        private string _Code;
        [XafDisplayName("Kode"), ToolTip("Kode")]
        [Size(10)]
        public virtual string Code
        {
            get { return _Code; }
            set { SetPropertyValue("Code", ref _Code, value); }
        }
     private string _Description; 
     [XafDisplayName("Keterangan"), ToolTip("Keterangan")] 
     [Size(500)] 
     public virtual string Description
     { 
       get { return _Description; } 
       set { SetPropertyValue("Description", ref _Description, value); } 
     }
        [XafDisplayName("Kode-Keterangan"), ToolTip("Kode-Keterangan")]
        //[Size(10)]
        public virtual string CodeKeterangan
        {
            get { return $"{Code}-{Description}";; }
          ///  set { SetPropertyValue("Code", ref _Code, value); }
        }

        private eDC _TypicalBalance;
    [XafDisplayName("Tipe Balance"), ToolTip("Tipe Balance")]
    
    public virtual eDC TypicalBalance
        {
        get { return _TypicalBalance; }
        set { SetPropertyValue("TypicalBalance", ref _TypicalBalance, value); }
    }

    private eTransCode _Type;
    [XafDisplayName("Type"), ToolTip("Type")]

    public virtual eTransCode Type
    {
        get { return _Type; }
        set { SetPropertyValue("Type", ref _Type, value); }
    }


    private fTransCategory _Category;
    [XafDisplayName("Kategori"), ToolTip("Kategori")]

    public virtual fTransCategory Category
    {
        get { return _Category; }
        set { SetPropertyValue("Category", ref _Category, value); }
    }

    private fTransCode _Header;
    [XafDisplayName("Header"), ToolTip("Header")]

    public virtual fTransCode Header
        {
        get { return _Header; }
        set { SetPropertyValue("Header", ref _Header, value); }
    }



    }
    public enum eTransCode
    {
        [XafDisplayName("Balance Sheet")]
        BS = 1,
        [XafDisplayName("Profit & Loss")]
        PL = 0

        //[XafDisplayName("Shift 1")]
        //Shift1 = 5,
        //[XafDisplayName("Shift 2")]
        //Shift2 = 6,
        //[XafDisplayName("Karyawan")]
        //Karyawan = 2,
        //[XafDisplayName("ATK")]
        //ATK= 3,
        //[XafDisplayName("Administrasi")]
        //Administrasi = 4,
        //[XafDisplayName("Kas")]
        //Kas = 1,
        //[XafDisplayName("Lain-Lain")]
        //LainLain = 0
    }
} 
