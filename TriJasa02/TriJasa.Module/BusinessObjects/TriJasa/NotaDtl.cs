// Class Name : NotaDtl.cs 
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
   [DefaultProperty("NomorSM")]
   [NavigationItem("Inquiry")]
   // Standard Document
   [System.ComponentModel.DisplayName("Nota Detail")]
   public class NotaDtl : XPObject
   {
     public NotaDtl() : base()
     {
     // This constructor is used when an object is loaded from a persistent storage.
     // Do not place any code here.
     }
     public NotaDtl(Session session) : base(session) 
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
     //private Nota _Nota; 
     //[XafDisplayName("Nomor Nota"), ToolTip("Nomor Nota")] 
     //[Association("NotaDtlinfo")] 
     //public virtual Nota Nota
     //{ 
     //  get { return _Nota; } 
     //  set { SetPropertyValue("Nota", ref _Nota, value); } 
     //} 
     // 
     private string _NomorSM; 
     [XafDisplayName("Nomor Surat Muatan"), ToolTip("Nomor Surat Muatan")] 
     [Size(20)] 
     public virtual string NomorSM
     { 
       get { return _NomorSM; } 
       set { SetPropertyValue("NomorSM", ref _NomorSM, value); } 
     } 
     // 
     private DateTime _TglJatuhTempo; 
     [XafDisplayName("Jatuh Tempo"), ToolTip("Jatuh Tempo")] 
     public virtual DateTime TglJatuhTempo
     { 
       get { return _TglJatuhTempo; } 
       set { SetPropertyValue("TglJatuhTempo", ref _TglJatuhTempo, value); } 
     } 
     // 
     private string _NamaBrg; 
     [XafDisplayName("Nama Barang"), ToolTip("Nama Barang")] 
     [Size(20)] 
     public virtual string NamaBrg
     { 
       get { return _NamaBrg; } 
       set { SetPropertyValue("NamaBrg", ref _NamaBrg, value); } 
     } 
     // 
     private string _Tujuan; 
     [XafDisplayName("Tujuan"), ToolTip("Tujuan")] 
     [Size(100)] 
     public virtual string Tujuan
     { 
       get { return _Tujuan; } 
       set { SetPropertyValue("Tujuan", ref _Tujuan, value); } 
     } 
     // 
     private string _Penerima; 
     [XafDisplayName("Penerima"), ToolTip("Penerima")] 
     [Size(100)] 
     public virtual string Penerima
     { 
       get { return _Penerima; } 
       set { SetPropertyValue("Penerima", ref _Penerima, value); } 
     } 
     // 
     private string _Pengirim; 
     [XafDisplayName("Pengirim"), ToolTip("Pengirim")] 
     [Size(30)] 
     public virtual string Pengirim
     { 
       get { return _Pengirim; } 
       set { SetPropertyValue("Pengirim", ref _Pengirim, value); } 
     } 
     // 
     private string _Keterangan; 
     [XafDisplayName("Keterangan"), ToolTip("Keterangan")] 
     [Size(30)] 
     public virtual string Keterangan
     { 
       get { return _Keterangan; } 
       set { SetPropertyValue("Keterangan", ref _Keterangan, value); } 
     } 
     // 
     private double _TotalJml; 
     [XafDisplayName("Total Jumlah"), ToolTip("Total Jumlah")] 
     public virtual double TotalJml
     { 
       get { return _TotalJml; } 
       set { SetPropertyValue("TotalJml", ref _TotalJml, value); } 
     } 
   }
    
} 
