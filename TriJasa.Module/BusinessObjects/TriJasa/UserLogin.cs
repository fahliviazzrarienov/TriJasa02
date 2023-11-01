// Class Name : cUser.cs 
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
using DevExpress.Persistent.BaseImpl.PermissionPolicy;

namespace TriJasa.Module.BusinessObjects
{
    [DefaultClassOptions]
    [ImageName("ModelEditor_Views")]
    [DefaultProperty("Name")]
    //[NavigationItem("Master")]
    // Standard Document
    [System.ComponentModel.DisplayName("User Login")]
    public class UserLogin : PermissionPolicyUser
    {
        //public cUser() : base()
        //{
        //    // This constructor is used when an object is loaded from a persistent storage.
        //    // Do not place any code here.
        //}
        public UserLogin(Session session) : base(session)
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
        private string _Name;
        [XafDisplayName("Name"), ToolTip("Full Name")]
        [Size(50)]
        public virtual string Name
        {
            get { return _Name; }
            set { SetPropertyValue("Name", ref _Name, value); }
        }
        private string _TelegramID;
        [XafDisplayName("TelegramID"), ToolTip("TelegramID")]
        [Size(100)]
        public virtual string TelegramID
        {
            get { return _TelegramID; }
            set { SetPropertyValue("TelegramID", ref _TelegramID, value); }
        }

        private string _PhoneNumber;
        [XafDisplayName("PhoneNumber"), ToolTip("PhoneNumber")]
        [Size(100)]
        public virtual string PhoneNumber
        {
            get { return _PhoneNumber; }
            set { SetPropertyValue("PhoneNumber", ref _PhoneNumber, value); }
        }
        private string _EmailAddress;
        [XafDisplayName("EmailAddress"), ToolTip("EmailAddress")]
        [Size(100)]
        public virtual string EmailAddress
        {
            get { return _EmailAddress; }
            set { SetPropertyValue("EmailAddress", ref _EmailAddress, value); }
        }

        private fCompany _Perusahaan;
        [XafDisplayName("Perusahaan"), ToolTip("Perusahaan")]
        [RuleRequiredField("Perusahaan Wajib Di Isi",
       DefaultContexts.Save)]
        public virtual fCompany Perusahaan
        {
            get { return _Perusahaan; }
            set { SetPropertyValue("Perusahaan", ref _Perusahaan, value); }
        }




    }
    public enum eIdxcUser
    {
        [XafDisplayName("ID")]
        ID = 0,
        [XafDisplayName("Name")]
        Name = 1,
        [XafDisplayName("Last Update")]
        LastUpdate = 2
    }
}

