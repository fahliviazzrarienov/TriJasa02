using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Reflection;
using DevExpress.Spreadsheet;

namespace Nucleus.Module.BusinessObjects
{
    [DomainComponent]
    [DefaultClassOptions]
    [FileAttachmentAttribute("InputFile")]
    //[ImageName("BO_Unknown")]
    //[DefaultProperty("SampleProperty")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ImportFile : IXafEntityObject, IObjectSpaceLink, INotifyPropertyChanged
    {
        private IObjectSpace objectSpace;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ImportFile()
        {
            Oid = Guid.NewGuid();
            ImportColoms = new BindingList<ImportColom>();
            RowHeader = 1;
        }

        [DevExpress.ExpressApp.Data.Key]
        [Browsable(false)]  // Hide the entity identifier from UI.
        public Guid Oid { get; set; }

        //private BindingList<ImportColom> ImportColoms;
        //[XafDisplayName("Colom Mapping"), ToolTip("Colom Mapping")]
        //public BindingList<ImportColom> ColomMap { get { return ImportColoms; } }

        private NPFileData _InputFile;

        [XafDisplayName("File"), ToolTip("File")]
        [ImmediatePostData]
        public NPFileData InputFile 
        {
            get { return _InputFile; }
            set
            {
                if (_InputFile != value)
                {
                    _InputFile = value;
                    OnPropertyChanged();
                   // XlsHeader(value);
                }

            }
        }

        private int _RowHeader;
        [XafDisplayName("Row Header"), ToolTip("Row Header")]
       // [VisibleInDetailView(false)]
       [ImmediatePostData]
        public virtual int RowHeader
        {
            get { return _RowHeader; }
            set
            {
                if (_RowHeader != value)
                {
                    _RowHeader = value;
                    OnPropertyChanged();
                    XlsHeader();
                }

            }
        }

        [Action("Get Header")]
        public void XlsHeader()
        {
            XlsHeader(InputFile);
        }

        private void XlsHeader(NPFileData xlsFiles)
        {
            if (xlsFiles != null )
            {
                iGetKey oGetKey = new iGetKey();

                var oFile = oGetKey.Openfiles(xlsFiles);
                if (oFile.Item1)
                {
                    //Worksheet worksheet = oFile.Item2;
                    // get header files
                    //xlsHeader
                    BindingList<xlsFileColomName> oxlsHeader = oGetKey.xlsHeader(RowHeader);

                    foreach (ImportColom oItem in ColomMap)
                    {
                        oItem.xlsColoms = oxlsHeader;
                        // break;
                    }
                    // List<InfoOfClass> aoInfoOfClass = oGetKey.GetObjProperty(ObjType, Session, worksheet);
                }
            }
        }
        private string _DataType;
        [XafDisplayName("DataType"), ToolTip("DataType")]
        [VisibleInDetailView(false)]
        public virtual string DataType
        {
            get { return _DataType; }
            set
            {
                if (_DataType != value)
                {
                    _DataType = value;
                    OnPropertyChanged();
                }

            }
        }
        public void UpdateTempate(string sDataType="")
        {
            DataType = sDataType;
            //xlsFileColomName oColomName = objectSpace.CreateObject<xlsFileColomName>();
            //oColomName.ColomName= "ss";
            
            //objectSpace.CommitChanges();

            //UpdateTempate();

        }

        //public xlsFileColomName GetColomName()
        //{
        // //   xlsFileColomName 
        //}
        public void UpdateTempate<T>( T ObjType, string objMaster = null, string sMasterPropertyName = "") where T : Type
        {
            DataType = ObjType.Name;
            // IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(Person));
            PropertyInfo[] properties = ObjType.GetProperties();
            iGetKey oGetKey = new iGetKey();
            // add colom to oject 
            foreach (PropertyInfo item in properties )
            {
                if (!oGetKey.FieldNoshow().Contains(item.Name.ToUpper()) )
                {
                    //ImportColom iImportColom = new ImportColom();
                    //iImportColom.ObjectName = item.Name;
                    //ImportColoms.Add(iImportColom);
                }
            }
           // XPClassInfo ObjTypeClassInfo = Session.GetClassInfo(ObjType);
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
}