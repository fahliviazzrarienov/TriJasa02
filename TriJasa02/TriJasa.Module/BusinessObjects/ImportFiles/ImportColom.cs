using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Nucleus.Module.BusinessObjects
{
    [DomainComponent]
    [DefaultClassOptions]
    //[ImageName("BO_Unknown")]
    //[DefaultProperty("SampleProperty")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ImportColom : IXafEntityObject, IObjectSpaceLink, INotifyPropertyChanged
    {
        private IObjectSpace objectSpace;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ImportColom()
        {
            Oid = Guid.NewGuid();

            //xlsColoms = new BindingList<xlsFileColomName>();
            ////xlsColoms.Add( "fdsf");
            ////xlsColoms.Add("dfs");
            //xlsFileColomName v = new xlsFileColomName();
            //v.ColomName = "fsfsafa";
            //xlsColoms.Add(v);
            //xlsFileColomName v1 = new xlsFileColomName();
            //v1.ColomName = "fsfsafaerwe";
            //xlsColoms.Add(v1);
        }

        [DevExpress.ExpressApp.Data.Key]
        [Browsable(false)]  // Hide the entity identifier from UI.
        public Guid Oid { get; set; }
        [Browsable(false)]  // Hide the entity identifier from UI.
        public BindingList<xlsFileColomName> xlsColoms { get; set; }
        [Appearance("ImportColomObjectName", Enabled = false, Visibility = ViewItemVisibility.Show)]
        public string ObjectName { get; set; }
        //public XLColom xlsName { get; set; }
        private xlsFileColomName _xlsName;
        [XafDisplayName("xlsName"), ToolTip("xlsName")]
        //[ModelDefault("EditMask", "(000)-00"), VisibleInListView(false)]
        //[RuleRequiredField(DefaultContexts.Save)]
        [DataSourceProperty("xlsColoms", DataSourcePropertyIsNullMode.SelectAll)]
        public xlsFileColomName xlsName
        {
            get { return _xlsName; }
            // set { SetPropertyValue("Company", ref _Company, value); }
            set
            {
                if (_xlsName != value)
                {
                    _xlsName = value;
                    //OnPropertyChanged();
                }
            }
        }

        //private string sampleProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), VisibleInListView(false)]
        //[RuleRequiredField(DefaultContexts.Save)]
        //public string SampleProperty
        //{
        //    get { return sampleProperty; }
        //    set
        //    {
        //        if (sampleProperty != value)
        //        {
        //            sampleProperty = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.SampleProperty = "Paid";
        //}

        #region IXafEntityObject members (see https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppIXafEntityObjecttopic.aspx)
        void IXafEntityObject.OnCreated()
        {
            // Place the entity initialization code here.
            // You can initialize reference properties using Object Space methods; e.g.:
            // this.Address = objectSpace.CreateObject<Address>();
        }
        void IXafEntityObject.OnLoaded()
        {
            // Place the code that is executed each time the entity is loaded here.
        }
        void IXafEntityObject.OnSaving()
        {
            // Place the code that is executed each time the entity is saved here.
        }
        #endregion

        #region IObjectSpaceLink members (see https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppIObjectSpaceLinktopic.aspx)
        // Use the Object Space to access other entities from IXafEntityObject methods (see https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113707.aspx).
        IObjectSpace IObjectSpaceLink.ObjectSpace
        {
            get { return objectSpace; }
            set { objectSpace = value; }
        }
        #endregion

        #region INotifyPropertyChanged members (see http://msdn.microsoft.com/en-us/library/system.componentmodel.inotifypropertychanged(v=vs.110).aspx)
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }

    //public class xlsFileColomName
    //{
    //    public xlsFileColomName()
    //    {

    //    }
    //    public string ColomName { get; set; }
    //}
}