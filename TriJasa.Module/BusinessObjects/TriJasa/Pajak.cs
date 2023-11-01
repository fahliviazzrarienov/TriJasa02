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
    [DefaultProperty("Kode")]
    [NavigationItem("Master")]
    // Standard Document
    [RuleCombinationOfPropertiesIsUnique("PajakRule", DefaultContexts.Save, "Kode")]
    [System.ComponentModel.DisplayName("PPN")]
    public class Pajak : XPObject
    {
        public Pajak() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }
        public Pajak(Session session) : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Kode = "";
            Keterangan = ""; 
            string tUser = SecuritySystem.CurrentUserName.ToString();

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

        private string _Kode;
        [XafDisplayName("Kode"), ToolTip("Kode")]
        [Size(20)]
        [IsSearch(true)]
        public virtual string Kode
        {
            get
            {
                return _Kode;
            }
            set { SetPropertyValue("Kode", ref _Kode, value); }
        }
        // 
        private string _Keterangan;
        [XafDisplayName("Keterangan"), ToolTip("Keterangan")]
        [Size(50)]
        public virtual string Keterangan
        {
            get
            {

                return _Keterangan;
            }
            set { SetPropertyValue("Keterangan", ref _Keterangan, value); }
        }
        private double _Pct;
        [XafDisplayName("Pct"), ToolTip("Pct")]
        [Size(50)]
        public virtual double Pct
        {
            get
            {

                return _Pct;
            }
            set { SetPropertyValue("Pct", ref _Pct, value); }
        }

    }
} 
