// Class Name : nOutlet.cs 
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
// new files
namespace TriJasa.Module.BusinessObjects
{ 
   [DefaultClassOptions] 
   [ImageName("ModelEditor_Views")] 
   [DefaultProperty("ObjectName")]
   [NavigationItem("Master")]
   // Standard Document
   [System.ComponentModel.DisplayName("Import Object")]
   public class ImportObject : XPObject
   {
     public ImportObject() : base()
     {
     // This constructor is used when an object is loaded from a persistent storage.
     // Do not place any code here.
     }
     public ImportObject(Session session) : base(session) 
     {
       // This constructor is used when an object is loaded from a persistent storage.
       // Do not place any code here.
     }
     public override void AfterConstruction()
     {
       base.AfterConstruction();
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
      

     private string _ObjectName; 
     [XafDisplayName("Object Name"), ToolTip("Object Name")]
      //[Appearance("ImportColomnObjectName", Enabled = false, Visibility = ViewItemVisibility.Show)]
        public virtual string ObjectName
        { 
       get { return _ObjectName; } 
       set { SetPropertyValue("ObjectName", ref _ObjectName, value); } 
     }

    private string _FullObjectName;
    [XafDisplayName("FullObjectName"), ToolTip("FullObjectName")]
        public virtual string FullObjectName
        {
        get { return _FullObjectName; }
        set { SetPropertyValue("FullObjectName", ref _FullObjectName, value); }
    }

      

    } 

} 
