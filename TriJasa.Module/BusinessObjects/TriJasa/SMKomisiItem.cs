// Class Name : DaftarTagihanNota.cs 
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
using DevExpress.ExpressApp.Model;

namespace TriJasa.Module.BusinessObjects
{
    [DefaultClassOptions]
    [ImageName("ModelEditor_Views")]
    [DefaultProperty("Nota")]
    [NavigationItem("Transaksi")]
    // Standard Document
    [System.ComponentModel.DisplayName("SMKomisi List Nota")]
    public class SMKomisiItem : XPObject
    {
        public SMKomisiItem() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }
        public SMKomisiItem(Session session) : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            string tUser = SecuritySystem.CurrentUserName.ToString();
            if (Oid <= 0)
            {
                Oid = ((DateTime.Now.Hour * 60 + DateTime.Now.Minute) * 60 + DateTime.Now.Second) * 100 + DateTime.Now.Millisecond;
            }
            TglUpdate = DateTime.Now;
                        UserUpdate = Session.FindObject<UserLogin>(new BinaryOperator("UserName", tUser));
        }
        protected override void OnSaving()
        {
            //Nota.Save();
            string tUser = SecuritySystem.CurrentUserName.ToString();
            UserUpdate = Session.FindObject<UserLogin>(new BinaryOperator("UserName", tUser));
            TglUpdate = DateTime.Now;
            base.OnSaving();
        }
        protected override void OnDeleting()
        {
            base.OnDeleting();
        }
        // 
        private SMKomisi _SMKomisi;
        [XafDisplayName("SM Komisi"), ToolTip("SM Komisi")]
        [Association("SMKomisiItemAssc")]
        public virtual SMKomisi smKomisi
        {
            get { return _SMKomisi; }
            set { SetPropertyValue("smKomisi", ref _SMKomisi, value); }
        }
        // 


        private XPCollection<SuratMuatan> availableListNota;
        [Browsable(false)]
        public XPCollection<SuratMuatan> ListSM
        {
            get
            {
                //CriteriaOperator oCriteria;
                //DaftarTagihanNota b;
                string sqlQuery="";
               // string sqlQuery = string.Format($" [Status] == 1 || [Status] == 4 ");// &&  Status != {eTagihan.Kolektor.GetHashCode()}  ");

                CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);
                XPCollection<SuratMuatan> ListSM = new XPCollection<SuratMuatan>(Session, filterOperator);
                return ListSM;
            }
        }

        //    List<string> oids = new List<string>();  
        //            foreach (OrderItem oi in OrderItems)  
        //            {  
        //                oids.Add(oi.Oid.ToString());  
        //            }
        //XPCollection<Delivery> deliveries = new XPCollection<Delivery>(Session, CriteriaOperator.Parse("DeliveryItems[OrderItem.Oid in (?)]", oids.ToArray<string>()));

        private SuratMuatan _SM;
        [XafDisplayName("SM"), ToolTip("SM")]
        [RuleRequiredField("SMKomisiItemSM", DefaultContexts.Save, "Harus Diisi")]
        [DataSourceProperty(nameof(ListSM))]
        [ImmediatePostData]
        public virtual SuratMuatan SM
        {
            get { return _SM; }
            set
            {
                SetPropertyValue("SM", ref _SM, value);
                if (!IsLoading && !IsSaving)
                {
                   // Status = eTagihan.Kolektor;
                }
            }
        }
        private int _Dibayarkan;
        [XafDisplayName("Dibayarkan"), ToolTip("Dibayarkan")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        [Size(150)]
        public virtual int Dibayarkan
        {
            get { return _Dibayarkan; }
            set { SetPropertyValue("Dibayarkan", ref _Dibayarkan, value); }
        }

        private string _Notes;
        [XafDisplayName("Notes"), ToolTip("Notes")]
        [Size(150)]
        public virtual string Notes
        {
            get { return _Notes; }
            set { SetPropertyValue("Notes", ref _Notes, value); }
        }
        private DateTime _TglUpdate;
        [XafDisplayName("Tgl Update"), ToolTip("Tgl Update")]
        [ModelDefault("DisplayFormat", "{0: dd/MM/yyyy}")]
        [Appearance("SMKomisiNotaTglUpdate", Enabled = false)]
        public virtual DateTime TglUpdate
        {
            get { return _TglUpdate; }
            set { SetPropertyValue("TglUpdate", ref _TglUpdate, value); }
        }
        private UserLogin _UserUpdate;
        [XafDisplayName("User Update"), ToolTip("User Update")]
        [Appearance("SMKimisiItemUserLogin", Enabled = false)]
        public virtual UserLogin UserUpdate
        {
            get { return _UserUpdate; }
            set { SetPropertyValue("UserUpdate", ref _UserUpdate, value); }
        }


        //     // 
        //     private eTagihan _Status; 
        //  [XafDisplayName("Status"), ToolTip("Status")]
        //     [Appearance("SMKimisiItemStatus", Enabled =false)]
        //     public virtual eTagihan Status
        //  { 
        //    get {

        //    return _Status; } 
        //    set { SetPropertyValue("Status", ref _Status, value);
        //             //if (!IsLoading && !IsSaving)
        //             //{
        //             //    if (Nota != null)
        //             //    {
        //             //        Nota.Status = value;
        //             //    }
        //             //}
        //         } 
        //  } 
        //}
    }
    public enum eKomisi
    {
        [XafDisplayName("Baru")]
        Belum = 0,
        [XafDisplayName("Print")]
        Dibayar = 1
        
    }
} 
