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
   [System.ComponentModel.DisplayName("Komisi")]
   public class SMKomisi : XPObject
   {
     public SMKomisi() : base()
     {
     // This constructor is used when an object is loaded from a persistent storage.
     // Do not place any code here.
     }
     public SMKomisi(Session session) : base(session) 
     {
       // This constructor is used when an object is loaded from a persistent storage.
       // Do not place any code here.
     }
     public override void AfterConstruction()
     {
       base.AfterConstruction();
       string tUser = SecuritySystem.CurrentUserName.ToString();
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

            foreach (SMKomisiItem oSM in ListSMKomisiItem)
            {
             //   oSM.Status = eTagihan.Print;
                oSM.Save();
                oSM.Session.CommitTransaction();
            }

           
            Session.Delete(ListSMKomisiItem);
            Session.Save(ListSMKomisiItem);
            Session.CommitTransaction();

            
        }
     // 
     private string _Nomor; 
     [XafDisplayName("Nomor"), ToolTip("Nomor")]
    [RuleRequiredField("SMKomisiNomor", DefaultContexts.Save, "Harus Diisi")]
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
                        Nomor = igen.SMKomisiNomorGet(Session, value);

                    }
                }
                SetPropertyValue("Tanggal", ref _Tanggal, value);

            } 
     } 
     // 
     private string  _Penerima; 
     [XafDisplayName("Penerima"), ToolTip("Penerima")]
     [RuleRequiredField("SMKomisiPenerimar", DefaultContexts.Save, "Harus Diisi")]
        public virtual string Penerima
        { 
       get { return _Penerima; } 
       set { SetPropertyValue("Penerima", ref _Penerima, value); } 
     }

        [XafDisplayName("SM Komisi"), ToolTip("SM Komisi")]
        [Association("SMKomisiItemAssc")]
        //[DataSourceProperty(nameof(AvailableTasks))]
        public XPCollection<SMKomisiItem> ListSMKomisiItem
        {
            get
            {
                return GetCollection<SMKomisiItem>("ListSMKomisiItem");
            }
        }



        private int _JumlahKomisi;
        [XafDisplayName("Jumlah SM"), ToolTip("Jumlah SM")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        [Size(30)]
        public virtual int JumlahKomisi
        {
            get {
                _JumlahKomisi = 0;
                if (!IsLoading && !IsSaving && ListSMKomisiItem != null)
                {
                    try
                    {
                        if (ListSMKomisiItem.Count > 0)
                        {
                            _JumlahKomisi = (int)ListSMKomisiItem.Count;
                        }
                    }
                    catch (Exception e)
                    { }
                }
                return _JumlahKomisi; 
                }
           // set { SetPropertyValue("JumlahNota", ref _JumlahNota, value); }
        }

      
        private double _TotalDibayarKan;
        [XafDisplayName("Total Dibayar Kan"), ToolTip("Total Dibayar Kan")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        [Size(30)]
        public virtual double TotalDibayarKan
        {
            get {
                _TotalDibayarKan = 0;
                if (!IsLoading && !IsSaving && ListSMKomisiItem != null)
                {
                    try
                    {
                        
                        if (ListSMKomisiItem.Count > 0)
                        {

                            _TotalDibayarKan = (int)ListSMKomisiItem.Sum(x => x.Dibayarkan);
                          
                        }
                    }
                    catch (Exception e)
                    { }
                }

                return _TotalDibayarKan; 
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

       
        private UserLogin _UserUpdate;
        [XafDisplayName("User Update"), ToolTip("User Update")]
        [Appearance("SMKomisiUserLogin", Enabled = false)]
        public virtual UserLogin UserUpdate
        {
            get { return _UserUpdate; }
            set { SetPropertyValue("UserUpdate", ref _UserUpdate, value); }
        }
      
    } 
   
} 
