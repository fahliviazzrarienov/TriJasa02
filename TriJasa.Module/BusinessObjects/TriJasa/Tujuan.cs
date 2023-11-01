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
    [DefaultProperty("Keterangan")]
    [NavigationItem("Master")]
    // Standard Document
    [RuleCombinationOfPropertiesIsUnique("TujuanRule", DefaultContexts.Save, "KodeTujuan")]
    [System.ComponentModel.DisplayName("Tujuan")]
    public class Tujuan : XPObject
    {
        public Tujuan() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }
        public Tujuan(Session session) : base(session)
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

        private string _KodeTujuan;
        [XafDisplayName("KodeTujuan"), ToolTip("KodeTujuan")]
        [Size(20)]
        [IsSearch(true)]
        public virtual string KodeTujuan
        {
            get
            {
                if (!IsLoading && !IsSaving)
                {
                    if (_KodeTujuan == null)
                    {
                        _KodeTujuan = "";
                    }
                }

                return _KodeTujuan;
            }
            set { SetPropertyValue("KodeTujuan", ref _KodeTujuan, value); }
        }
        // 
        private string _Keterangan;
        [XafDisplayName("Keterangan"), ToolTip("Keterangan")]
        [Size(50)]
        public virtual string Keterangan
        {
            get
            {
                if (!IsLoading && !IsSaving)
                {
                    if (_Keterangan == null)
                    {
                        _Keterangan = "";
                    }
                }
                return _Keterangan;
            }
            set { SetPropertyValue("Keterangan", ref _Keterangan, value); }
        }


    }
} 
