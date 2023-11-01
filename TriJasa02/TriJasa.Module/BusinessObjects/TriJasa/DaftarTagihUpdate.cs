using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace TriJasa.Module.BusinessObjects
{
    [DomainComponent]
    [DefaultClassOptions]
    //[ImageName("BO_Unknown")]
    //[DefaultProperty("SampleProperty")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class DaftarTagihUpdate : IXafEntityObject, IObjectSpaceLink, INotifyPropertyChanged
    {
        private IObjectSpace objectSpace;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public DaftarTagihUpdate()
        {
            Oid = Guid.NewGuid();
        }
 
        [DevExpress.ExpressApp.Data.Key]
        [Browsable(false)]  // Hide the entity identifier from UI.
        public Guid Oid { get; set; }

        private DateTime _Tanggal;
        [ModelDefault("DisplayFormat", "{0: dd/MM/yyyy}")]
        [XafDisplayName("Tanggal"), ToolTip("Tanggal")]
        //[ModelDefault("EditMask", "(000)-00"), VisibleInListView(false)]
        //[RuleRequiredField(DefaultContexts.Save)]
        public DateTime Tanggal
        {
            get { return _Tanggal; }
            set
            {
                if (_Tanggal != value)
                {
                    _Tanggal = value;
                    //OnPropertyChanged();
                }
            }
        }

        private string _Keterangan;
        [XafDisplayName("Keterangan"), ToolTip("Keterangan")]
        [FieldSize(150)]
        public string Keterangan
        {
            get { return _Keterangan; }
            set
            {
                if (_Keterangan != value)
                {
                    _Keterangan = value;
                    //OnPropertyChanged();
                }
            }
        }

        private int _Total;
        [XafDisplayName("Total"), ToolTip("Total")]
        [Appearance("DaftarTagihUpdateTotal", Enabled = false)]
        public int Total
        {
            get { return _Total; }
            set
            {
                if (_Total != value)
                {
                    _Total = value;
                }
            }
        }

        private double _Jumlah;
        [XafDisplayName("Jumlah"), ToolTip("Jumlah")]
        [Appearance("DaftarTagihUpdateJumlah", Enabled = false)]
        public double Jumlah
        {
            get { return _Jumlah; }
            set
            {
                if (_Jumlah != value)
                {
                    _Jumlah = value;
                }
            }
        }
        private eTagihan _Status;
        [XafDisplayName("Status"), ToolTip("Status")]
        [Appearance("DaftarTagihUpdateStatus", Enabled = false)]
        public eTagihan Status
        {
            get { return _Status; }
            set
            {
                if (_Status != value)
                {
                    _Status = value;
                }
            }
        }

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
}