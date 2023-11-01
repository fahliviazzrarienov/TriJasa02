// Class Name : TarifSM.cs 
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
   [DefaultProperty("pelanggan")]
   [NavigationItem("Master")]
   // Standard Document
   [System.ComponentModel.DisplayName("Tarim SM")]
   public class TarifSM : XPObject
   {
     public TarifSM() : base()
     {
     // This constructor is used when an object is loaded from a persistent storage.
     // Do not place any code here.
     }
     public TarifSM(Session session) : base(session) 
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
        TglUpdate = new DateTime(System.DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        User = Session.FindObject<UserLogin>(new BinaryOperator("UserName", SecuritySystem.CurrentUserName.ToString())); 
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
        // 
        private Pelanggan _pelanggan;
        [XafDisplayName("Pelanggan"), ToolTip("Pelanggan")]
        public virtual Pelanggan pelanggan
        {
            get { return _pelanggan; }
            set { SetPropertyValue("pelanggan", ref _pelanggan, value); }
        }
        
        // 
     [XafDisplayName("Tarif Unit"), ToolTip("Tarif Unit")] 
     [Association("TarifSMAsc")] 
     public XPCollection<TarifSMHarga> tarifSMUnit
     {
     get
       {
         return GetCollection<TarifSMHarga>("tarifSMUnit"); 
       }
     }
     // 
     private UserLogin _User; 
     [XafDisplayName("User"), ToolTip("User")] 
     public virtual UserLogin User
     { 
       get { return _User; } 
       set { SetPropertyValue("User", ref _User, value); } 
     } 
     // 
     private DateTime _TglUpdate; 
     [XafDisplayName("TanggalUpdate"), ToolTip("TanggalUpdate")] 
     public virtual DateTime TglUpdate
     { 
       get { return _TglUpdate; } 
       set { SetPropertyValue("TglUpdate", ref _TglUpdate, value); } 
     } 
   } 
 
} 
