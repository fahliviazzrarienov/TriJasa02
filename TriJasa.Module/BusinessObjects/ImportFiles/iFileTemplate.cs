using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Editors;
using DevExpress.Spreadsheet;
using System.Data.SqlTypes;
using DevExpress.XtraPrinting.Native;
using DevExpress.XtraRichEdit.Model;
using DevExpress.Xpo.Metadata;
//using System.Activities.Expressions;
// new files
namespace TriJasa.Module.BusinessObjects
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    [DefaultProperty("Name")]
    [NavigationItem("Master")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class iFileTemplate : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public iFileTemplate(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        private string _DataType;
        [XafDisplayName("DataType"), ToolTip("DataType")]
        public virtual string DataType
        {
            get { return _DataType; }
            set { SetPropertyValue("DataType", ref _DataType, value); }
        }
        private string _Object;
        [XafDisplayName("Object"), ToolTip("Object")]
        public virtual string Object
        {
            get { return _Object; }
            set { SetPropertyValue("Object", ref _Object, value); }
        }
        //private string _Name;
        //[XafDisplayName("Name"), ToolTip("Name")]
        //public virtual string Name
        //{
        //    get { return _Name; }
        //    set { SetPropertyValue("Name", ref _Name, value); }
        //}

        //[Action(Caption = "Update")]
        public void UpdateTempate()
        {
           
            // ...
            Worksheet worksheet;
            Workbook workbook = new Workbook();
            worksheet =  workbook.Worksheets[0];
            // Add a reference to the DevExpress.Docs.dll assembly.
            List<string> sTemplate = new List<string>();
            iGetKey oGetKey = new iGetKey();
            Type myType = Type.GetType(DataType);
            GetFieldname(myType, worksheet);

            //Workbook workbook = new Workbook();
            //worksheet.Import(oList, 0, 1, true);
            //workbook.Worksheets[0].Import(sTemplate, 0, 0, false);

            // Save a document to a byte array to store it in a database.
            byte[] docBytes = workbook.SaveDocument(DocumentFormat.Xlsx);
            Template = workbook.SaveDocument(DocumentFormat.Xlsx);
            // ...

            // Load the saved document from a byte array into the Workbook instance.
            //workbook.LoadDocument(docBytes);
            

        }

        private byte[] _Template;
        [EditorAlias(EditorAliases.SpreadsheetPropertyEditor)]
        public byte[] Template
        {
            get { return _Template; }
            set { SetPropertyValue(nameof(Template), ref _Template, value); }
        }

       
        private void GetFieldname<T>(T oType,Worksheet ws ) where T : Type
        {
            iGetKey gKey = new iGetKey();
            List<string> alist = gKey.FieldNoshow();
            //XPClassInfo classInfo = Session.DefaultSession.Dictionary.GetClassInfo(typeof(XPObject));
            XPClassInfo classInfo = Session.Dictionary.GetClassInfo(oType);
            //string tableName = classInfo.TableName;
            int i = 0;
            foreach (XPMemberInfo info in classInfo.PersistentProperties)
            {
                string fieldName = info.MappingField;
                if ( ! alist.Contains(fieldName))
                  {
                    ws.Columns.Insert(i);
                    Cell oCell = ws[0, i];
                    oCell.Value = fieldName;
                    //Cell oCell1 = ws[1, i];
                    //oCell1.Value = info.DisplayName;
                    //Cell oCell2 = ws[2, i];
                    //oCell2.Value = info.GetType().ToString();
                    //Cell oCell3 = ws[3, i];
                    //oCell3.Value = info.MemberType.ToString();
                    i++;
                }
                //string fieldName = info.ma

            }
        }

        public void ExportTemplate()
        {
            //Worksheet worksheet = spreadsheetControl1.Document.Worksheets[0];
            // Worksheet worksheet;
            #region #ImportList
            // Create a List object containing string values.
            List<string> sTemplate = new List<string>();
            //sTemplate.Add("New York");
            //sTemplate.Add("Rome");
            //sTemplate.Add("Beijing");
            //sTemplate.Add("Delhi");
            //iGetKey oGetKey = new iGetKey();
            //System.ComponentModel.TypeConverter rectConverter=new TypeConverter();
            iGetKey oGetKey = new iGetKey();
            switch (DataType)
            {
                case "fLanguage":
                    //fLanguage oLanguage = new fLanguage(Session);
                    //ImportLanguage();//(oLanguage);
                    //sTemplate = oGetKey.EnumToListString(typeof(eIdxfLanguage));
                    break;
                    
            }
            string sqlQuery = string.Format(" DataType == '{0}' ||  Name == '{0}' ",
                                 DataType.Trim().Replace("'", "''"));
            CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);
            iFileTemplate oFileTemplate = Session.FindObject<iFileTemplate>(filterOperator);
            if (oFileTemplate == null)
            {
                oFileTemplate = new iFileTemplate(Session);
            }

            //oFileTemplate.Object = sObjectName;
            //oFileTemplate.UpdateTempate(sTemplate);
            // Import the list into the worksheet and insert it vertically, starting with the B1 cell.
            // worksheet.Import(cities, 0, 1, true);
            #endregion #ImportList
        }
    }
}