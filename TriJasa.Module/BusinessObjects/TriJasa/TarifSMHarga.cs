// Class Name : TarifSMUnit.cs 
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
   [DefaultProperty("TarifSM")]
   [NavigationItem("Master")]
   // Standard Document
   [System.ComponentModel.DisplayName("Tarif")]
   public class TarifSMHarga : XPObject
   {
     public TarifSMHarga() : base()
     {
     // This constructor is used when an object is loaded from a persistent storage.
     // Do not place any code here.
     }
     public TarifSMHarga(Session session) : base(session) 
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
     private TarifSM _TarifSM; 
     [XafDisplayName("TarifSM"), ToolTip("TarifSM")] 
     [Association("TarifSMAsc")] 
     public virtual TarifSM TarifSM
     { 
       get { return _TarifSM; } 
       set { SetPropertyValue("TarifSM", ref _TarifSM, value); } 
     }

        // 
        private string _Keterangan;
        [XafDisplayName("Keterangan"), ToolTip("Keterangan")]
        public virtual string Keterangan
        {
            get { return _Keterangan; }
            set { SetPropertyValue("Keterangan", ref _Keterangan, value); }
        }

        // 
        private DateTime _Berlaku;
        [XafDisplayName("Berlaku Mulai"), ToolTip("Berlaku Mulai")]
        public virtual DateTime Berlaku
        {
            get { return _Berlaku; }
            set { SetPropertyValue("Berlaku", ref _Berlaku, value); }
        }
        // 
        private double _HargaPerKG;
        [XafDisplayName("Per KG"), ToolTip("Per KG")]
        public virtual double HargaPerKG
        {
            get { return _HargaPerKG; }
            set { SetPropertyValue("HargaPerKG", ref _HargaPerKG, value); }
        }
        // 
        private double _HargaPerM3;
        [XafDisplayName("Per M3"), ToolTip("Per M3")]
        public virtual double HargaPerM3
        {
            get { return _HargaPerM3; }
            set { SetPropertyValue("HargaPerM3", ref _HargaPerM3, value); }
        }

        private double _HargaPerKGOrg;
        [XafDisplayName("Per KG Original"), ToolTip("Per KG Original")]
        public virtual double HargaPerKGOrg
        {
            get { return _HargaPerKGOrg; }
            set { SetPropertyValue("HargaPerKGOrg", ref _HargaPerKGOrg, value); }
        }
        // 
        private double _HargaPerM3Org;
        [XafDisplayName("Per M3 Original"), ToolTip("Per M3 Original")]
        public virtual double HargaPerM3Org
        {
            get { return _HargaPerM3Org; }
            set { SetPropertyValue("HargaPerM3Org", ref _HargaPerM3Org, value); }
        }
    } 

} 
