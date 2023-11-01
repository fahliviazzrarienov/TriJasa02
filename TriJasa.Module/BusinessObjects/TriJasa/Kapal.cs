// Class Name : Truck.cs 
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
    [DefaultProperty("kapal")]
    [NavigationItem("Master")]
    // Standard Document
    //[RuleCombinationOfPropertiesIsUnique("KapalRule", DefaultContexts.Save, "KapalTruck")]
    [System.ComponentModel.DisplayName("Kapal")]
    public class Kapal : Truck
    {
        public Kapal() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }
        public Kapal(Session session) : base(session)
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
        private string _NomorTruck;
        [XafDisplayName("Kapal"), ToolTip("Kapal")]
        [Size(20)]
        [IsSearch(true)]
        public new string kapal
        {
            get
            {        return base.NomorTruck;
            }
            set { base.NomorTruck = value;  }
        }
    }
} 
