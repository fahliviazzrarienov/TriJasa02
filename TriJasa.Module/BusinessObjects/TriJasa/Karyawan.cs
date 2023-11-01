// Class Name : Karyawan.cs 
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
   [DefaultProperty("Nama")]
   [NavigationItem("Master")]
   // Standard Document
   [System.ComponentModel.DisplayName("Karyawan")]
    [RuleCombinationOfPropertiesIsUnique("KaryawanRuleUnique", DefaultContexts.Save, "Nama")]
    public class Karyawan : XPObject
   {
     public Karyawan() : base()
     {
     // This constructor is used when an object is loaded from a persistent storage.
     // Do not place any code here.
     }
     public Karyawan(Session session) : base(session) 
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

    private string _ID;
    [XafDisplayName("Kode"), ToolTip("Kode")]
    [Size(50)]
    //[IsSearch(true)]
    public virtual string ID
    {
        get {
                if (Oid > 0)
                {
                    _ID = "K" + Oid.ToString("000000");
                }
                return _ID;
            }
            //    return _Nama; }
        //set { SetPropertyValue("Nama", ref _Nama, value); }
    }

     private string _Nama; 
     [XafDisplayName("Nama"), ToolTip("Nama")] 
     [Size(150)] 
     [IsSearch(true)]
     public virtual string Nama
     { 
       get { return _Nama; } 
       set { SetPropertyValue("Nama", ref _Nama, value); } 
     } 
     // 
     private string _NomorHP; 
     [XafDisplayName("Nomor HP"), ToolTip("Nomor HP")] 
     [Size(50)] 
     public virtual string NomorHP
     { 
       get {
                if (!IsLoading && !IsSaving)
                {
                    if (_NomorHP == null)
                    {
                        _NomorHP = "";
                     }
                }
                return _NomorHP; } 
       set { SetPropertyValue("NomorHP", ref _NomorHP, value); } 
     } 
     // 
     private string _NomorSM; 
     [XafDisplayName("Nomor SIM"), ToolTip("Nomor SIM")] 
     [Size(25)] 
     public virtual string NomorSM
     { 
       get { return _NomorSM; } 
       set { SetPropertyValue("NomorSM", ref _NomorSM, value); } 
     } 
   } 
   
} 
