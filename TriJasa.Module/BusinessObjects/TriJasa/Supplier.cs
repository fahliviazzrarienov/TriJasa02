// Class Name : Supplier.cs 
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

namespace TriJasa.Module.BusinessObjects
{ 
   [DefaultClassOptions] 
   [ImageName("ModelEditor_Views")] 
   [DefaultProperty("Nama")]
   [NavigationItem("Master")]
   // Standard Document
   [System.ComponentModel.DisplayName("Supplier")]
   public class Supplier : Supir
   {
     public Supplier() : base()
     {
     // This constructor is used when an object is loaded from a persistent storage.
     // Do not place any code here.
     }
     public Supplier(Session session) : base(session) 
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

        //private string _ID;
        //[XafDisplayName("Kode"), ToolTip("Kode")]
        //[Size(50)]
        ////[IsSearch(true)]
        //public new string ID
        //{
        //    get
        //    {
        //        if (base.Oid > 0)
        //        {
        //            _ID = "S" + base.Oid.ToString("000000");
        //        }
        //        return _ID;
        //    }
        //    //    return _Nama; }
        //    //set { SetPropertyValue("Nama", ref _Nama, value); }
        //}
        // 
        private string _ContactPerson;
        [XafDisplayName("Contact Person"), ToolTip("Contact Person")]
        [Size(100)]
        public virtual string ContactPerson
        {
            get { return _ContactPerson; }
            set { SetPropertyValue("ContactPerson", ref _ContactPerson, value); }
        }
        // 

        private string _NoTelp;
        [XafDisplayName("No. Telp"), ToolTip("No. Telp")]
        [Appearance("PelangganNotelp", Visibility = ViewItemVisibility.Hide)]
        [Size(50)]
        public virtual string NoTelp
        {
            get { return _NoTelp; }
            set { SetPropertyValue("No.Telp", ref _NoTelp, value); }
        }

        private string _Alamat1;
        [XafDisplayName("Alamat1"), ToolTip("Alamat1")]
        [Size(150)]
        public virtual string Alamat1
        {
            get
            {
                return _Alamat1;
            }
            set { SetPropertyValue("Alamat1", ref _Alamat1, value); }
        }
        // 
        private string _Alamat2;
        [XafDisplayName("Alamat2"), ToolTip("Alamat2")]
        [Size(150)]
        public virtual string Alamat2
        {
            get
            {
                         return _Alamat2;
            }
            set { SetPropertyValue("Alamat2", ref _Alamat2, value); }
        }
        // 
        private string _Alamat3;
        [XafDisplayName("Alamat3"), ToolTip("Alamat3")]
        [Size(150)]
        public virtual string Alamat3
        {
            get
            {
                return _Alamat3;
            }
            set { SetPropertyValue("Alamat3", ref _Alamat3, value); }
        }
        // 
        private string _Alamat4;
        [XafDisplayName("Alamat4"), ToolTip("Alamat4")]
        [Size(150)]
        public virtual string Alamat4
        {
            get
            {
                return _Alamat4;
            }
            set { SetPropertyValue("Alamat4", ref _Alamat4, value); }
        }
        // 
        private string _Kota;
        [XafDisplayName("Kota"), ToolTip("Kota")]
        [Size(150)]
        public virtual string Kota
        {
            get { return _Kota; }
            set { SetPropertyValue("Kota", ref _Kota, value); }
        }
        // 
        private string _Propinsi;
        [XafDisplayName("Propinsi"), ToolTip("Propinsi")]
        [Size(50)]
        public virtual string Propinsi
        {
            get { return _Propinsi; }
            set { SetPropertyValue("Propinsi", ref _Propinsi, value); }
        }

        private bool _PPN;
        [XafDisplayName("PPN"), ToolTip("PPN")]
        [Size(50)]
        public virtual bool PPN
        {
            get { return _PPN; }
            set { SetPropertyValue("PPN", ref _PPN, value); }
        }



    } // 

} 
   
 
