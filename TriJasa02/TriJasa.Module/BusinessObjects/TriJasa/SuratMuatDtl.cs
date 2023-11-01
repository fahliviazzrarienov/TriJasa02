// Class Name : SuratMuatDtl.cs 
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
   [DefaultProperty("JenisBarang")]
   [NavigationItem("Inquiry")]
   // Standard Document
   [System.ComponentModel.DisplayName("SuratMuatDtl")]
   public class SuratMuatDtl : XPObject
   {
     public SuratMuatDtl() : base()
     {
     // This constructor is used when an object is loaded from a persistent storage.
     // Do not place any code here.
     }
     public SuratMuatDtl(Session session) : base(session) 
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
            Jumlah = 0;
            JumlahOriginal = 0;
            Ongkos = 0;
            OngkosOriginal = 0;
            Kuantum = 0;
            Kg = 0;
            VolM3 = 0;
            JenisBarang = "";
            //try
            //{ }
            //XPCollection<SuratMuatDtl> xpDM = new XPCollection<SuratMuatDtl>(Session);

            //int OidTem= xpDM.Max(x => Oid) ;
            //Oid = OidTem + 1;
            if (Oid<=0)
            {
                Oid = ((DateTime.Now.Hour * 60 + DateTime.Now.Minute) * 60 + DateTime.Now.Second) * 100 + DateTime.Now.Millisecond;
            }
            //Oid = ((DateTime.Now.Hour * 60 + DateTime.Now.Minute) * 60 + DateTime.Now.Second) * 100 + DateTime.Now.Millisecond;
            //Oid &= Convert.ToInt32(Session.Evaluate(SuratMuatDtl)(DevExpress.Data.Filtering.CriteriaOperator.Parse("Max(EmpKey)"), nothing)) + 1
        } 
     protected override void OnSaving()
     {
            //ValidateValue();
          //  SuratMuat.Save();
       base.OnSaving();
     } 
     protected override void OnDeleting()
     {
       base.OnDeleting();
     } 
     // 
     private SuratMuatan _SuratMuat; 
     [XafDisplayName("Surat Muatan"), ToolTip("Surat Muatan")] 
     [Association("SM")] 
     //[IsSearch(true)]
     public virtual SuratMuatan SuratMuat
     { 
       get { return _SuratMuat; }
            set
            {
                //   SetPropertyValue("SuratMuat", ref _SuratMuat, value); 
                SuratMuatan oldSuratMuat = _SuratMuat;
                bool modified = SetPropertyValue(nameof(SuratMuat), ref _SuratMuat, value);
                if (!IsLoading && !IsSaving && oldSuratMuat != _SuratMuat && modified)
                {
                    oldSuratMuat = oldSuratMuat ?? _SuratMuat;
                    //oldSuratMuat.UpdateOrdersCount(true);
                    oldSuratMuat.UpdateTotalJumlah(true);
                    //oldSuratMuat.UpdateMaximumOrder(true);

                }
            }
     } 
     // 
     private string _JenisBarang; 
     [XafDisplayName("Jenis Barang"), ToolTip("Jenis Barang")]
        //[RuleRequiredField(DefaultContexts.Save)]
        [Size(60)] 
     [ImmediatePostData]
     public virtual string JenisBarang
     { 
       get { return _JenisBarang; } 
       set {
                //SetPropertyValue("JenisBarang", ref _JenisBarang, value);
                bool modified = SetPropertyValue(nameof(JenisBarang), ref _JenisBarang, value);
                if (!IsLoading && !IsSaving && SuratMuat != null && modified)
                {

                    SuratMuat.UpdateJenisBarang(true);
                    // SuratMuat.UpdateMaximumOrder(true);
                }
            } 
     } 
     // 
     private double _Kuantum; 
     [XafDisplayName("Kuantum"), ToolTip("Kuantum")] 
     [ImmediatePostData]
     public virtual double Kuantum
     { 
       get { return _Kuantum; } 
       set {
                SetPropertyValue("Kuantum", ref _Kuantum, value);
            } 
     } 
     // 
     private string _Unit; 
     [XafDisplayName("Unit"), ToolTip("Unit")] 
     [Size(20)] 
     public virtual string Unit
     { 
       get { return _Unit; } 
       set { SetPropertyValue("Unit", ref _Unit, value); } 
     } 
     // 
     private double _Kg; 
     [XafDisplayName("Kg"), ToolTip("Kg")]
        [ImmediatePostData]
        public virtual double Kg
     { 
       get { return _Kg; } 
       set {    SetPropertyValue("Kg", ref _Kg, value);
                bool modified = SetPropertyValue(nameof(Jumlah), ref _Jumlah, value);
                if (!IsLoading && !IsSaving )
                {

                    Jumlah = Ongkos * (value + VolM3);
                    JumlahOriginal = OngkosOriginal * (value + VolM3);
                    //JumlahOriginal = OngkosOriginal * (value + VolM3);
                }
            }
        } 
     // 
     private double _VolM3; 
     [XafDisplayName("M3"), ToolTip("M3")]
     [ImmediatePostData]
        public virtual double VolM3
     { 
       get { return _VolM3; } 
       set { SetPropertyValue("VolM3", ref _VolM3, value);
                if (!IsLoading && !IsSaving )
                {

                    Jumlah = Ongkos * (Kg + value);
                    JumlahOriginal = OngkosOriginal * (Kg + value);
                    //JumlahOriginal = OngkosOriginal * (Kg + value);
                }
            } 
     } 
     // 
     private double _Ongkos; 
     [XafDisplayName("Ongkos"), ToolTip("Ongkos")]
     [ImmediatePostData]
     public virtual double Ongkos
     { 
       get { return _Ongkos; } 
       set { SetPropertyValue("Ongkos", ref _Ongkos, value);

                if (!IsLoading && !IsSaving )
                {

                    Jumlah = value * (Kg + VolM3);
                    if ( SuratMuat!=null)
                    {
                        try
                        {
                            if (SuratMuat.Loco == true)
                            {
                                OngkosOriginal = value;
                            }
                        }
                        catch( Exception e)
                        {

                        }
                    }
                    //JumlahOriginal = OngkosOriginal * (Kg + VolM3);
                }
            } 
     } 
     // 
     private double _Jumlah; 
     [XafDisplayName("Jumlah"), ToolTip("Jumlah")]
        //   [PersistentAlias("Ongkos * (Kg + VolM3)")]
        //[ImmediatePostData]
        public virtual double Jumlah
     {
            get { return _Jumlah; }
            //get
            //{
            //    object tempObject = EvaluateAlias(nameof(Jumlah));
            //    if (tempObject != null)
            //    {
            //        return (double)tempObject;
            //    }
            //    else
            //    {
            //        return 0;
            //        // return _TotalBiayaLain; 
            //    }
            //}
            set
            {

                SetPropertyValue("Jumlah", ref _Jumlah, value);
                //    //bool modified = SetPropertyValue(nameof(Jumlah), ref _Jumlah, value);
                if (!IsLoading && !IsSaving && SuratMuat != null )
                {
                    if (SuratMuat.Loco == true)
                    {
                        

                    }
                    // SuratMuat.UpdateMaximumOrder(true);
                }
            }
        } 
     // 
     private double _OngkosOriginal; 
     [XafDisplayName("Ongkos Original"), ToolTip("Ongkos Original")] 
     [ImmediatePostData]
     public virtual double OngkosOriginal
     { 
       get { return _OngkosOriginal; } 
       set { SetPropertyValue("OngkosOriginal", ref _OngkosOriginal, value);
                if (!IsLoading && !IsSaving )
                {

                    // Jumlah = Ongkos * (Kg + VolM3);
                    JumlahOriginal = OngkosOriginal * (Kg + VolM3);
                }
            } 
     } 
     // 
     private double _JumlahOriginal; 
     [XafDisplayName("Jumlah Original"), ToolTip("Jumlah Original")]
        //[PersistentAlias("OngkosOriginal * (Kg + VolM3)")]
        //[PersistentAlias("OngkosOriginal * Hours")]
        //[ImmediatePostData]
        public virtual double JumlahOriginal
       {
            get { return _JumlahOriginal; }
            //get
            //{
            //    object tempObject = EvaluateAlias(nameof(JumlahOriginal));
            //    if (tempObject != null)
            //    {
            //        return (double)tempObject;
            //    }
            //    else
            //    {
            //        return 0;
            //        // return _TotalBiayaLain; 
            //    }
            //}
            set { SetPropertyValue("JumlahOriginal", ref _JumlahOriginal, value); }
        } 
     // 
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
        private string _SysImportNo;
        [XafDisplayName("SysImportNo"), ToolTip("SysImportNo")]
        [Size(50)]
        [IsSearch(true)]
        public virtual string SysImportNo
        {
            get { return _SysImportNo; }
            set
            {

                SetPropertyValue("SysImportNo", ref _SysImportNo, value);
            }
        }
        public void ValidateValueNotes_()
        {
            //if ( Jumlah >0 && Ongkos==0)
            //{
            //    Ongkos = Jumlah / Kuantum;
            //}
            //else if ( Jumlah==0 && Ongkos>0)
            //{
            //   // Jumlah = Ongkos * Kuantum;
            //}
            //if (JumlahOriginal > 0 && OngkosOriginal == 0)
            //{
            //    OngkosOriginal = JumlahOriginal / Kuantum;
            //}
            //else if (JumlahOriginal == 0 && OngkosOriginal > 0)
            //{
            //  //  JumlahOriginal = OngkosOriginal * Kuantum;
            //}

        }
   } 
   
} 
