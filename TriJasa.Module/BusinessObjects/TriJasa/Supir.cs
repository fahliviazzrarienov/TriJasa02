// Class Name : Supir.cs 
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
   //[DefaultProperty("ID")]
   [NavigationItem("Master")]
   // Standard Document
   [System.ComponentModel.DisplayName("Supir")]

    public class Supir : Karyawan
   {
     public Supir() : base()
     {
     // This constructor is used when an object is loaded from a persistent storage.
     // Do not place any code here.
     }
     public Supir(Session session) : base(session) 
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
            //this.Nama
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

    [XafDisplayName("Truck"), ToolTip("Truck")]
    [IsParent(eIsParent.Member)]
    [Association("SupirTruck")]
    public XPCollection<Truck> Trucks
    {
        get
        {
            return GetCollection<Truck>("Trucks");
        }
    }

    [XafDisplayName("Klaim"), ToolTip("Klaim")]
    [Association("KlaimSupir")]
    public XPCollection<SupirKlaim> Klaim
    {
        get
        {
            return GetCollection<SupirKlaim>("Klaim");
        }
    }

        private SupirGroup _Group;
        [XafDisplayName("Group"), ToolTip("Group")]
        //[Size(20)]
        //[IsSearch(true)]
        [Association("GrpSupir")]
        public virtual SupirGroup Group
        {
            get { return _Group; }
            set { SetPropertyValue("Group", ref _Group, value); }
        }
    } 
   
} 
