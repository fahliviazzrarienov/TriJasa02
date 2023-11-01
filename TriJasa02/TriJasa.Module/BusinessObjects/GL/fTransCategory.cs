// Class Name : fVechType.cs 
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
    [DefaultProperty("Code")]
    [NavigationItem("Master")]
    // Standard Document
    [System.ComponentModel.DisplayName("Trans Kategory")]
    public class fTransCategory : XPObject
    {
        public fTransCategory() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }
        public fTransCategory(Session session) : base(session)
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
        private string _Code;
        [XafDisplayName("Kode"), ToolTip("Kode")]
        [Size(10)]
        public virtual string Code
        {
            get { return _Code; }
            set { SetPropertyValue("Code", ref _Code, value); }
        }
        //
        private string _Name;
        [XafDisplayName("Keterangan"), ToolTip("Keterangan")]
        [Size(100)]
        public virtual string Name
        {
            get { return _Name; }
            set { SetPropertyValue("Name", ref _Name, value); }
        }
        
    }
    
}