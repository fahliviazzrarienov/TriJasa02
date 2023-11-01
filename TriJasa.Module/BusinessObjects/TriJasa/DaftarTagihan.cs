// Class Name : DaftarTagihan.cs 
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
   [DefaultProperty("Nomor")]
   [NavigationItem("Transaksi")]
   // Standard Document
   [System.ComponentModel.DisplayName("DaftarTagihan")]
   public class DaftarTagihan : XPObject
   {
     public DaftarTagihan() : base()
     {
     // This constructor is used when an object is loaded from a persistent storage.
     // Do not place any code here.
     }
     public DaftarTagihan(Session session) : base(session) 
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
            Tanggal = new DateTime(System.DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        } 
     protected override void OnSaving()
     {
       base.OnSaving();
            string tUser = SecuritySystem.CurrentUserName.ToString();
            UserUpdate = Session.FindObject<UserLogin>(new BinaryOperator("UserName", tUser));

        } 
     protected override void OnDeleting()
     {
            DeleteDetails();
            base.OnDeleting();
     } 

     public  void DeleteDetails()
        {

            foreach (DaftarTagihanNota oNota in ListNotaTagih)
            {
                oNota.Status = eTagihan.Print;
                oNota.Save();
                oNota.Session.CommitTransaction();
            }

            while (ListNota.Count > 0)
            {
                ListNota.Remove(ListNota[0]);
            }
            Session.Delete(ListNotaTagih);
            Session.Save(ListNotaTagih);
            Session.CommitTransaction();

            
        }
     // 
     private string _Nomor; 
     [XafDisplayName("Nomor"), ToolTip("Nomor")]
    [RuleRequiredField("DaftarTagihanNomor", DefaultContexts.Save, "Harus Diisi")]
        [Size(15)] 
     public virtual string Nomor
     { 
       get { return _Nomor; } 
       set { SetPropertyValue("Nomor", ref _Nomor, value); } 
     } 
     // 
     private DateTime _Tanggal; 
     [XafDisplayName("Tanggal"), ToolTip("Tanggal")]
        [ModelDefault("DisplayFormat", "{0: dd/MM/yyyy}")]
        [ImmediatePostData]
     public virtual DateTime Tanggal
     { 
       get { return _Tanggal; } 
       set {
                if (!IsLoading && !IsSaving && _Tanggal != null)
                {
                    if ((value.ToString("yyyyMMdd") != _Tanggal.ToString("yyyyMMdd"))
                        && (Nomor == "" || Nomor == null))

                    {
                        iGenTriJasa igen = new iGenTriJasa();
                        Nomor = igen.TagihNomorGet(Session, value);

                    }
                }
                SetPropertyValue("Tanggal", ref _Tanggal, value);

            } 
     } 
     // 
     private Kolektor _NamaColl; 
     [XafDisplayName("Nama Collector"), ToolTip("Nama Collector")]
     [RuleRequiredField("DaftarTagihanNamaKolektor", DefaultContexts.Save, "Harus Diisi")]
        public virtual Kolektor NamaColl
     { 
       get { return _NamaColl; } 
       set { SetPropertyValue("NamaColl", ref _NamaColl, value); } 
     }

        [XafDisplayName("Nota Tagih"), ToolTip("Nota Tagih")]
        [Association("TagihnotaAssc")]
        //[DataSourceProperty(nameof(AvailableTasks))]
        public XPCollection<DaftarTagihanNota> ListNotaTagih
        {
            get
            {
                return GetCollection<DaftarTagihanNota>("ListNotaTagih");
            }
        }

        [XafDisplayName("Nota"), ToolTip("Nota")]
        [Association("TagihNotaList")]
        [DataSourceProperty(nameof(AvailableTasks))]
        public XPCollection<Nota> ListNota
        {
            get
            {
                return GetCollection<Nota>("ListNota");
            }
        }

        //[XafDisplayName("Nota / Faktur"), ToolTip("Nota / Faktur")]
        //[Association("NotatagihAssc")]
        //public XPCollection<Nota> Notas
        //{
        //    get
        //    {
        //        return GetCollection<Nota>("Notas");
        //    }
        //}


        // 
        private int _JumlahNota;
        [XafDisplayName("Jumlah Nota"), ToolTip("JumlahNota")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        [Size(30)]
        public virtual int JumlahNota
        {
            get {
                _JumlahNota = 0;
                if (!IsLoading && !IsSaving && ListNota != null)
                {
                    try
                    {
                        if (ListNota.Count > 0)
                        {
                            _JumlahNota = (int)ListNota.Count;
                        }
                    }
                    catch (Exception e)
                    { }
                }
                return _JumlahNota; 
                }
           // set { SetPropertyValue("JumlahNota", ref _JumlahNota, value); }
        }

        public void AddNota()
        {
            List<DaftarTagihanNota> todelete = new List<DaftarTagihanNota>();
            
            foreach ( DaftarTagihanNota aTagihNota in ListNotaTagih)
            {
                aTagihNota.Status = eTagihan.Kembali;
                todelete.Add(aTagihNota);
            }

            foreach ( Nota aNota in ListNota)
            {
                bool isFind = false;
                foreach (DaftarTagihanNota aTagihNota in ListNotaTagih)
                {
                    if (aTagihNota.Nota == aNota)
                    {
                        aTagihNota.Status = eTagihan.Kolektor;
                        isFind = true;
                    }
                }
                if (isFind==false)
                {
                    DaftarTagihanNota aDFNota = new DaftarTagihanNota(Session);
                    aDFNota.Nota = aNota;
                    aDFNota.Status = eTagihan.Kolektor;
                    aDFNota.DaftarTagihan = this;
                    aDFNota.Save();
                    aDFNota.Session.CommitTransaction();
                    //ListNotaTagih.Add(aDFNota);
                }
            }


            foreach (DaftarTagihanNota aNota in todelete)
            {
                foreach (DaftarTagihanNota aTagihNota in ListNotaTagih)
                {
                    if (aTagihNota == aNota && aTagihNota.Status==eTagihan.Kembali)
                    {
                        aTagihNota.Delete();
                        aTagihNota.Session.CommitTransaction();
                        break;
                    }
                }
            }
        }
        private double _TotalTagih;
        [XafDisplayName("Jumlah Tagih"), ToolTip("Jumlah Tagih")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        [Size(30)]
        public virtual double TotalTagih
        {
            get {
                _TotalTagih = 0;
                if (!IsLoading && !IsSaving && ListNota != null)
                {
                    try
                    {
                        
                        if (ListNota.Count > 0)
                        {
                          
                               _TotalTagih = (int)ListNota.Sum(x => x.TotalBiaya);
                          
                        }
                    }
                    catch (Exception e)
                    { }
                }

                return _TotalTagih; 
            }
           // set { SetPropertyValue("TotalTagih", ref _TotalTagih, value); }
        }
        private string _Note1; 
     [XafDisplayName("Note 1"), ToolTip("Note 1")] 
     [Size(30)] 
     public virtual string Note1
     { 
       get { return _Note1; } 
       set { SetPropertyValue("Note1", ref _Note1, value); } 
     } 
     // 
     private string _Note2; 
     [XafDisplayName("Note 2"), ToolTip("Note 2")] 
     [Size(30)] 
     public virtual string Note2
     { 
       get { return _Note2; } 
       set { SetPropertyValue("Note2", ref _Note2, value); } 
     } 
     // 
     private string _Note3; 
     [XafDisplayName("Note 3"), ToolTip("Note 3")] 
     [Size(30)] 
     public virtual string Note3
     { 
       get { return _Note3; } 
       set { SetPropertyValue("Note3", ref _Note3, value); } 
     }

        private XPCollection<Nota> availableTasks;
        [Browsable(false)] // Prohibits showing the AvailableTasks collection separately
        public XPCollection<Nota> AvailableTasks
        {
            get
            {
                if (availableTasks == null)
                {
                    // Retrieve all Task objects
                    availableTasks = new XPCollection<Nota>(Session);
                }
                // Filter the retrieved collection according to the current conditions
                RefreshAvailableTasks();
                // Return the filtered collection of Task objects
                return availableTasks;
            }
        }
        private UserLogin _UserUpdate;
        [XafDisplayName("User Update"), ToolTip("User Update")]
        [Appearance("DaftarTagihUserLogin", Enabled = false)]
        public virtual UserLogin UserUpdate
        {
            get { return _UserUpdate; }
            set { SetPropertyValue("UserUpdate", ref _UserUpdate, value); }
        }
        private void RefreshAvailableTasks()
        {
            if (availableTasks == null)
                return;
            string sqlQuery = string.Format($" [Status] == 1 || [Status] == 4 ");// &&  Status != {eTagihan.Kolektor.GetHashCode()}  ");
            CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);
            //Remove the applied filter
            availableTasks.Criteria = filterOperator;

        }
    } 
   
} 
