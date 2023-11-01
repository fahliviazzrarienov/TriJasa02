// Class Name : fVechType.cs 
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using System.Linq;

namespace TriJasa.Module.BusinessObjects
{
    [DefaultClassOptions]
    [ImageName("ModelEditor_Views")]
    [DefaultProperty("Code")]
    [NavigationItem("Master")]
    // Standard Document
    [System.ComponentModel.DisplayName("Repot Setting")]
    public class iReport : XPObject
    {
        public iReport() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }
        public iReport(Session session) : base(session)
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
        //
        private string _Code;
        [XafDisplayName("Kode"), ToolTip("Kode")]
        [Size(50)]
        public virtual string Code
        {
            get { return _Code; }
            set { SetPropertyValue("Code", ref _Code, value); }
        }
        //

        private string _ReportName;
        [XafDisplayName("ReportName"), ToolTip("ReportName")]
        [Size(150)]
        public virtual string ReportName
        {
            get { return _ReportName; }
            set { SetPropertyValue("ReportName", ref _ReportName, value); }
        }

        private string _Description;
        [XafDisplayName("Description"), ToolTip("Description")]
        [Size(150)]
        public virtual string Description
        {
            get { return _Description; }
            set { SetPropertyValue("Description", ref _Description, value); }
        }

    }

}