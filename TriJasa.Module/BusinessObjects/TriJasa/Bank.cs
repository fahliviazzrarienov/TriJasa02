// Class Name : Bank.cs 
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
   [DefaultProperty("Nama")]
   [NavigationItem("Master")]
   // Standard Document
   [System.ComponentModel.DisplayName("Bank")]
   public class Bank : XPObject
   {
     public Bank() : base()
     {
     // This constructor is used when an object is loaded from a persistent storage.
     // Do not place any code here.
     }
     public Bank(Session session) : base(session) 
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
     private string _Nama; 
     [XafDisplayName("Nama"), ToolTip("Nama")] 
     [Size(20)] 
     public virtual string Nama
     { 
       get { return _Nama; } 
       set { SetPropertyValue("Nama", ref _Nama, value); } 
     } 
     // 
     private string _NomorRekening; 
     [XafDisplayName("Nomor Rekening"), ToolTip("Nomor Rekening")] 
     [Size(20)] 
     public virtual string NomorRekening
     { 
       get { return _NomorRekening; } 
       set { SetPropertyValue("NomorRekening", ref _NomorRekening, value); } 
     }

    private fTransCode _AccountCode;
    [XafDisplayName("AccountCode"), ToolTip("AccountCode")]
    [Size(20)]
    public virtual fTransCode AccountCode
        {
        get { return _AccountCode; }
        set { SetPropertyValue("AccountCode", ref _AccountCode, value); }
    }
        private string _ReferenceId;
        [XafDisplayName("ReferenceId"), ToolTip("ReferenceId")]
        [Size(5)]
        public virtual string ReferenceId
        {
            get { return _ReferenceId; }
            set { SetPropertyValue("ReferenceId", ref _ReferenceId, value); }
        }

    } 
  
} 
