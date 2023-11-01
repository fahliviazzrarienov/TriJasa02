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
   [System.ComponentModel.DisplayName("Daftar Tagihan List Nota")]
   public class DaftarTagihanNota : XPObject
   {
     public DaftarTagihanNota() : base()
     {
     // This constructor is used when an object is loaded from a persistent storage.
     // Do not place any code here.
     }
     public DaftarTagihanNota(Session session) : base(session) 
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
     private DaftarTagihan _DaftarTagihan; 
     [XafDisplayName("DaftarTagihan"), ToolTip("DaftarTagihan")]
     [Association("TagihnotaAssc")]
     public virtual DaftarTagihan DaftarTagihan
     { 
       get { return _DaftarTagihan; } 
       set { SetPropertyValue("DaftarTagihan", ref _DaftarTagihan, value); } 
     }
        // 

     
        private XPCollection<Nota> availableListNota;
        [Browsable(false)]
        public XPCollection<Nota> ListNota
        {
            get
            {
                //CriteriaOperator oCriteria;
                //DaftarTagihanNota b;

                string sqlQuery = string.Format($" [Status] == 1 || [Status] == 4 ");// &&  Status != {eTagihan.Kolektor.GetHashCode()}  ");
                CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);
                //XPCollection<DaftarTagihanNota> aNotatagih = new XPCollection<DaftarTagihanNota>(Session, filterOperator);
                //List<int> oids = new List<int>();
                //foreach (DaftarTagihanNota oi in aNotatagih)
                //{
                //    oids.Add(oi.Oid);
                //}
                //oCriteria = null;
                //oCriteria = GroupOperator.And(oCriteria, new  InOperator("Oid", oids));
               // oCriteria = GroupOperator.And(oCriteria,  new   InOperator();
                XPCollection<Nota> listNota = new XPCollection<Nota>(Session, filterOperator);
                   // RefreshCompany1DataSource();
                return listNota;
            }
        }

    //    List<string> oids = new List<string>();  
    //            foreach (OrderItem oi in OrderItems)  
    //            {  
    //                oids.Add(oi.Oid.ToString());  
    //            }
    //XPCollection<Delivery> deliveries = new XPCollection<Delivery>(Session, CriteriaOperator.Parse("DeliveryItems[OrderItem.Oid in (?)]", oids.ToArray<string>()));

    private Nota _Nota; 
     [XafDisplayName("Nota"), ToolTip("Nota")]
     [RuleRequiredField("DaftarTagihanNota",DefaultContexts.Save,"Harus Diisi")]
        //[DataSourceProperty("[Status] == 0 || [Status] == 3 ")]
        //[DataSourceCriteriaProperty("[Status] == 0 && [Status] == 3 ")]
        /// [DataSourceProperty("Nota", DataSourcePropertyIsNullMode.SelectNothing)]
        //[DataSourceCriteria("[Status] = 0")]
        //[DataSourceProperty("[Status] = 0")]
        //[DataSourceCriteria("IsGlobal = true")]
        // [DataSourceCriteriaProperty("[Status] = 0 ")]
        [DataSourceProperty(nameof(ListNota))]
        [ImmediatePostData]
     public virtual Nota Nota
     { 
       get { return _Nota; } 
       set { SetPropertyValue("Nota", ref _Nota, value); 
            if( !IsLoading && !IsSaving)
                {
                    Status = eTagihan.Kolektor;
                }
            } 
     }
    //private string _Pelanggan;
    //[XafDisplayName("Pelanggan"), ToolTip("Pelanggan")]
    //[Size(30)]
    //public virtual string Pelanggan
    //    {
    //        get {
    //            _Pelanggan = "";
    //              if (!IsLoading && !IsSaving && Nota != null)
    //               {
    //                _Pelanggan = Nota.PelanganNama;
    //               }
    //            return _Pelanggan; }
    //    //set { SetPropertyValue("Notes", ref _Notes, value); }
    //}

    //private string _Alamat1;
    //[XafDisplayName("Alamat1"), ToolTip("Alamat1")]
    //[Size(30)]
    //public virtual string Alamat1
    //    {
    //    get
    //    {
    //       _Alamat1 = "";
    //        if (!IsLoading && !IsSaving && Nota != null)
    //        {
    //                _Alamat1 = Nota.PelangganAlamat1;
    //        }
    //        return _Alamat1;
    //    }
    //    //set { SetPropertyValue("Notes", ref _Notes, value); }
    //}
    //    private string _Alamat2;
    //    [XafDisplayName("Alamat2"), ToolTip("Alamat2")]
    //    [Size(30)]
    //    public virtual string Alamat2
    //    {
    //        get
    //        {
    //            _Alamat2 = "";
    //            if (!IsLoading && !IsSaving && Nota != null)
    //            {
    //                _Alamat2 = Nota.PelangganAlamat2;
    //            }
    //            return _Alamat2;
    //        }
    //        //set { SetPropertyValue("Notes", ref _Notes, value); }
    //    }

    //    private string _Alamat3;
    //    [XafDisplayName("Alamat3"), ToolTip("Alamat3")]
    //    [Size(30)]
    //    public virtual string Alamat3
    //    {
    //        get
    //        {
    //            _Alamat3 = "";
    //            if (!IsLoading && !IsSaving && Nota != null)
    //            {
    //                _Alamat3 = Nota.PelangganAlamat3;
    //            }
    //            return _Alamat3;
    //        }
    //        //set { SetPropertyValue("Notes", ref _Notes, value); }
    //    }
    //    private double _Tagihan;
    //    [XafDisplayName("Tagihan"), ToolTip("Tagihan")]
    //    [Size(30)]
    //    public virtual double Tagihan
    //    {
    //        get
    //        {
    //            _Tagihan = 0;
    //            if (!IsLoading && !IsSaving && Nota != null)
    //            {
    //                _Tagihan = Nota.TotalBiaya;
    //            }
    //            return _Tagihan;
    //        }
    //        //set { SetPropertyValue("Notes", ref _Notes, value); }
    //    }

        // 
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
        [Appearance("DaftarTagihNotaTglUpdate", Enabled = false)]
        public virtual DateTime TglUpdate
        {
            get { return _TglUpdate; }
            set { SetPropertyValue("TglUpdate", ref _TglUpdate, value); }
        }
        private UserLogin _UserUpdate;
        [XafDisplayName("User Update"), ToolTip("User Update")]
        [Appearance("DaftarTagihNotaUserLogin", Enabled = false)]
        public virtual UserLogin UserUpdate
        {
            get { return _UserUpdate; }
            set { SetPropertyValue("UserUpdate", ref _UserUpdate, value); }
        }


        // 
        private eTagihan _Status; 
     [XafDisplayName("Status Sampai/Tidak"), ToolTip("Status Sampai/Tidak")]
        [Appearance("DaftarTagihNotaStatus", Enabled =false)]
        public virtual eTagihan Status
     { 
       get {
                
                return _Status; } 
       set { SetPropertyValue("Status", ref _Status, value);
                if (!IsLoading && !IsSaving)
                {
                    if (Nota != null)
                    {
                        Nota.Status = value;
                    }
                }
            } 
     } 
   }
    public enum eTagihan
    {
        [XafDisplayName("Baru")]
        Baru = 0,
        [XafDisplayName("Print")]
        Print = 1,
        [XafDisplayName("Kolektor")]
        Kolektor = 2,
        [XafDisplayName("Diterima")]
        Diterima = 3,
        [XafDisplayName("Kembali")]
        Kembali = 4,
        [XafDisplayName("Dibayar")]
        Dibayar = 5,
        [XafDisplayName("Lunas")]
        Lunas = 6
    }

} 
