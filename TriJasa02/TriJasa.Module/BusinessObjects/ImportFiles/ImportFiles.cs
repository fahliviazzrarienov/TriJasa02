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
using System.Reflection;
using DevExpress.Spreadsheet;
using DevExpress.Xpo.Metadata;
//using System.Collections;
// new files
namespace TriJasa.Module.BusinessObjects
{
    [DefaultClassOptions]
    [ImageName("ModelEditor_Views")]
    [DefaultProperty("Name")]
    [NavigationItem("Master")]
    // Standard Document
    [System.ComponentModel.DisplayName("Import File")]
    public class ImportFiles : XPObject
    {
        public ImportFiles() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }
        public ImportFiles(Session session) : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            UpdateByTime();
            RowHeader = 1;
            //ListAdd();
           // Updateby = (User)SecuritySystem.CurrentUser;
        }
        private void UpdateByTime()
        {
            string tUser = SecuritySystem.CurrentUserName.ToString();
            Updateby = Session.FindObject<UserLogin>(new BinaryOperator("UserName", tUser));
            Updatedate = DateTime.Now;
        }


        //private void ListAdd()
        //{
        //  aDataType = new BindingList<string>();


        //    aDataType.Add("CustClass");
        //    aDataType.Add("CustType");
        //    //aDataType = oDataType;
        //}
        //[Browsable(false)]  // Hide the entity identifier from UI.
        //public BindingList<string> aDataType { get; set; }
        protected override void OnSaving()
        {
            UpdateByTime();
            base.OnSaving();
        }
        protected override void OnDeleting()
        {
            base.OnDeleting();
        }

        [XafDisplayName("FileUpload"), ToolTip("FileUpload")]
        public FileSystemStoreObject FileUpload
        {
            get { return GetPropertyValue<FileSystemStoreObject>("FileUpload"); }
            set
            {
                // string oldfileName = FileUpload.FileName;

                SetPropertyValue<FileSystemStoreObject>("FileUpload", value);

                if (!IsLoading && !IsSaving)
                {
                    //RowHeader = 0;
                    //if (value.FileName != "")
                    //{
                    //    XlsHeader();
                    //}
                }
            }
        }
        private xlsFileSheet _Sheet;
        [XafDisplayName("Sheet"), ToolTip("Sheet")]
        [ImmediatePostData]
        [DataSourceProperty("xlsSheet", DataSourcePropertyIsNullMode.SelectAll)]
        //[Appearance("FileDataType", Enabled = false, Visibility = ViewItemVisibility.Hide)]
        public virtual xlsFileSheet Sheet
        {
            get { return _Sheet; }
            set { SetPropertyValue("Sheet", ref _Sheet, value); }
        }

        private int _RowHeader;
        [XafDisplayName("Row Header"), ToolTip("Row Header")]
        [ImmediatePostData]
        public virtual int RowHeader
        {
            get { return _RowHeader; }
            set
            {
                if (value >= 0)
                {

                    SetPropertyValue("RowHeader", ref _RowHeader, value);
                    if (!IsLoading && !IsSaving)
                    {
                        //ValidateStatus = false;
                        //Validatexls();
                      //  XlsHeader();
                    }
                }
                else
                {
                    _RowHeader = 0;
                }

            }
        }

        private string  _Name;
        [XafDisplayName("Name"), ToolTip("Name")]
        //[Appearance("ImportFileObjectName", Enabled = false, Visibility = ViewItemVisibility.Hide)]
        public virtual string Name
        {
            get { return _Name; }
            set { SetPropertyValue("Name", ref _Name, value); }
        }

        private ImportObject _ObjectName;
        [XafDisplayName("Object Name"), ToolTip("Object Name")]
        //[Appearance("ImportFileObjectName", Enabled = false, Visibility = ViewItemVisibility.Hide)]
        public virtual ImportObject ObjectName
        {
            get { return _ObjectName; }
            set { SetPropertyValue("ObjectName", ref _ObjectName, value);
                 
                 if (!IsLoading && !IsSaving)
                {
                    try
                    {
                        if (value != null)
                        {
                            DataType = value.ObjectName;
                            UpdateTempate();
                        }
                        else
                        {
                            SetObjectNameEmpty();
                        }
                    } 
                    catch ( Exception e)
                    {
                        SetObjectNameEmpty();
                    }
                    
                }
                 }
        }

        private void SetObjectNameEmpty()
         {
            DataType = "";
            ObjectFullNameType = "";
            Session.Delete(Colomn);
            Session.Save(Colomn);

        }
        private string _DataType;
        [XafDisplayName("Data Type"), ToolTip("Data Type")]
        [Appearance("FileDataType", Enabled = false, Visibility = ViewItemVisibility.Hide)]
  
        public virtual string DataType
        {
            get { return _DataType; }
            set { SetPropertyValue("DataType", ref _DataType, value); }
        }

        private string _ObjectFullNameType;
        [XafDisplayName("Object Type"), ToolTip("Object Type")]
        [Appearance("FileObjectType", Enabled = false, Visibility = ViewItemVisibility.Hide)]
        public virtual string ObjectFullNameType
        {
            get { return _ObjectFullNameType; }
            set { SetPropertyValue("ObjectFullNameType", ref _ObjectFullNameType, value); }
        }


        [XafDisplayName("Object Colomn"), ToolTip("Object Colomn")]
        [Association("ImpObject")]
        public XPCollection<ImportColomn> Colomn
        {
            get
            {
                return GetCollection<ImportColomn>("Colomn");
            }
        }
        [XafDisplayName("Excel Colomn"), ToolTip("Excel Colomn")]
        [Association("ImpColom")]
        [Appearance("ImportFilesxlsColomn", Enabled = false, Visibility = ViewItemVisibility.Show)]
        public XPCollection<xlsFileColomn> xlsColomn
        {
            get
            {
                return GetCollection<xlsFileColomn>("xlsColomn");
            }
        }
        [XafDisplayName("Excel Sheet"), ToolTip("Excel Sheet")]
        [Association("ImpSheet")]
        [Appearance("ImportFilesxlsSheet", Enabled = false, Visibility = ViewItemVisibility.Show)]
        public XPCollection<xlsFileSheet> xlsSheet
        {
            get
            {
                return GetCollection<xlsFileSheet>("xlsSheet");
            }
        }

        public void UpdateTempate(string sDataType = "")
        {
            DataType = sDataType;
            Session.Delete(Colomn);
            Session.Save(Colomn);
            if (ObjectName.ObjectName != null)
            {
                if (ObjectName.ObjectName !=""  )
                {
                    string objectType = "TriJasa.Module.BusinessObjects." + ObjectName.FullObjectName;
                    Type oType = Type.GetType(objectType);
                    UpdateTempate(oType);
                }
               // ImportData(oType, "", "", Sheet.SheetName, RowHeader);
            }
            
        }
        public void UpdateTempate<T>(T ObjType, string objMaster = null, string sMasterPropertyName = "") where T : Type
        {
            //DataType = ObjType.Name.Substring(1, ObjType.Name.Length-1);
            ObjectFullNameType = ObjType.FullName;
            DataType = ObjType.Name.Substring(1, ObjType.Name.Length - 1);
            iGetKey oGetKey = new iGetKey();
            // IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(Person));
            List<string> aNotlist = oGetKey.FieldNoshow();
            PropertyInfo[] properties = ObjType.GetProperties();
            
            // add colom to oject 
            foreach (PropertyInfo item in properties)
            {
                object[] attributesArray = item.GetCustomAttributes(true);

                foreach (Attribute at in attributesArray)
                {
                    if (at is IsHide)
                    {
                        aNotlist.Add(item.Name.ToUpper());
                    }
                }

                // if (!oGetKey.FieldNoshow().Contains(item.Name.ToUpper()))
                if (!aNotlist.Contains(item.Name.ToUpper()))
                {
                    ImportColomn oImportColom = new ImportColomn(Session);
                    //ImportColom iImportColom = new ImportColom();
                    oImportColom.ObjectName = item.Name;
                    oImportColom.ImportFile = this;
                    oImportColom.Save();
                    oImportColom.Session.CommitTransaction();
                    //ImportColoms.Add(iImportColom);
                }
            }
            // XPClassInfo ObjTypeClassInfo = Session.GetClassInfo(ObjType);
        }
        [Action(Caption = "Get Sheet")]
        public void XlsSheet()
        {
            if (FileUpload != null)
            {
                if (FileUpload.FileName != "")
                {
                    XlsSheet(FileUpload);

                }
            }
        }
        [Action(Caption ="Get Header")]
        public void XlsHeader()
        {
            if (FileUpload != null)
            {
                if (Sheet != null)
                {
                    if (RowHeader > 0 && FileUpload.FileName != "" && Sheet.SheetName != "")
                    {
                        int iRowHeader = RowHeader - 1;
                        XlsHeader(FileUpload, Sheet.SheetName, iRowHeader);
                        MatchColom();
                    }
                }
            }
        }


        private void MatchColom()
        {
            foreach( ImportColomn iColom in Colomn)
            {
                foreach( xlsFileColomn  xlCol in xlsColomn)
                {
                     if ( iColom.ObjectName== xlCol.ColomName )
                    {
                        iColom.xlsName = xlCol;
                        iColom.Save();
                        iColom.Session.CommitTransaction();
                        break;
                    }
                }
            }
        }
        private void xlsSheetAdd(string worksheets)
        {
            int maxloop = 100;
            // xlsColoms.Clear();
            // xlsColoms = new XPCollection<xlsFileColomn>(oSession);
            int lastEmpty = 0;
            if (worksheets != null)
            {
                foreach (string item in worksheets.Split('|').ToList())
                {
                    if (item != "")
                    {
                        xlsFileSheet oColom = new xlsFileSheet(Session);
                        oColom.SheetName = item;
                        oColom.ImportFile = this;
                        oColom.Save();
                        oColom.Session.CommitTransaction();
                    }
                }
            }
        }
        private void xlsColomnAdd(Worksheet worksheet, int oRowheader=0)
        {
            int maxloop = 100;
            // xlsColoms.Clear();
            // xlsColoms = new XPCollection<xlsFileColomn>(oSession);
            int lastEmpty = 0;
            if (worksheet != null)
            {
                for (int i = 0; i < maxloop; i++)
                {
                    string xlsValue;
                    xlsValue = worksheet.Cells[oRowheader, i].Value.ToString().Trim();
                    if (xlsValue != "")
                    {
                        xlsFileColomn oColom = new xlsFileColomn(Session);
                        string sCol = "";
                        if ( i <10)
                        {
                            sCol = "0" + i.ToString();
                        }
                        else
                        {
                            sCol = i.ToString();
                        }
                        oColom.ColomName = $"{xlsValue}";
                        oColom.ColomIdx = i;
                        oColom.ImportFile = this;
                        oColom.Save();
                        oColom.Session.CommitTransaction();
                        // xlsColoms.Add(oColom);

                    }
                    else
                    {
                        lastEmpty++;

                        if (lastEmpty > 15)
                        {
                            maxloop = 100;
                        }

                    }
                }
            }
        }
        private void XlsSheet(FileSystemStoreObject xlsFiles)
        {
            if (xlsFiles != null)
            {
                iGetKey oGetKey = new iGetKey();

                xlsFiles.isSaveAlready();
                var oFile = oGetKey.OpenFileWorkSheet(xlsFiles);
                if (oFile.Item1)
                {
                    //Worksheet worksheet = oFile.Item2;
                    // get header files
                    //xlsHeader
                    Session.Delete(xlsColomn);
                    Session.Save(xlsColomn);
                    Session.Delete(xlsSheet);
                    Session.Save(xlsSheet);
                    Sheet = null;

                    xlsSheetAdd(oFile.Item2);
                    // List<InfoOfClass> aoInfoOfClass = oGetKey.GetObjProperty(ObjType, Session, worksheet);
                }
            }
        }
        private void XlsHeader(FileSystemStoreObject xlsFiles, string sheet, int oRowheader)
        {
            if (xlsFiles != null)
            {
                iGetKey oGetKey = new iGetKey();

                xlsFiles.isSaveAlready();
                var oFile = oGetKey.Openfiles(xlsFiles, sheet);
                if (oFile.Item1)
                {
                    //Worksheet worksheet = oFile.Item2;
                    // get header files
                    //xlsHeader
                    Session.Delete(xlsColomn);
                    Session.Save(xlsColomn);
                    xlsColomnAdd(oFile.Item2, oRowheader);
                    // List<InfoOfClass> aoInfoOfClass = oGetKey.GetObjProperty(ObjType, Session, worksheet);
                }
            }
        }
        [Action(Caption ="Import Data")]
        public  void ImportData()
        {

            //XPClassInfo classInfo = this.ClassInfo;
            //string oAssemblyName = classInfo.AssemblyName.ToString();

            ////XPClassInfo classInfo = session.DefaultSession.Dictionary.GetClassInfo("TriJasa.Module.BusinessObjects",objectType);
            // XPClassInfo oclassInfo = Session.GetClassInfo("TriJasa.Module" , objectType);

            //// Type type = classInfo.GetType();
            //Type oType = oclassInfo.GetType();
            //Type Objecttype = Type.GetType("GPN.Module.BusinessObjects." + iObjectMap.GPN)
            //this.GetType().Assembly.GetName()
            if (ObjectName.ObjectName != null)
            {

                string b = this.GetType().ToString().Replace("ImportFiles","");
                //string objectType = "TriJasa.Module.BusinessObjects." + ObjectName.FullObjectName;
                string objectType = this.GetType().ToString().Replace("ImportFiles", "") + ObjectName.FullObjectName;
                Type oType = Type.GetType(objectType);
                ImportData(oType, "", "", Sheet.SheetName, RowHeader);
                Session.Save(this);
                Session.CommitTransaction();
            }
        }
        public  void ImportData<T>(T ObjType, string objMaster="", string sMasterPropertyName = "", string oSheet="" ,int oRowHeader=0) where T : Type
        {

            iGetKey oGetKey = new iGetKey();
            int maxloop = 2147483640;
            // open file xls
            var fCheck = oGetKey.Openfiles(FileUpload, oSheet);
            int blankid = 0;
            // check if the xls has data
            if (fCheck.Item1)
            {
                Worksheet worksheet = fCheck.Item2;
                string sqlQuery = "";
                string xlsValue;

                //try
                //{
                    // remove sementara
                  //  Session.BeginTransaction();
                
                    Type objMasterType = objMaster.GetType();
                    //eIdxfCountry eC;
                    //eC = eIdxfCountry.ID;

                    //== > MAPPING OBJECT WITH XSL COLOM
                    List<InfoOfClass> aoInfoOfClass = oGetKey.GetObjProperty(ObjType, Session, Colomn);
                    // ==> GET CLASS INFO OF XPO
                    XPClassInfo ObjTypeClassInfo = Session.GetClassInfo(ObjType);

                   // ==> HAS MAPPING WITH XLS ONLY
                   List<InfoOfClass> aoInfoOfClassHasXlsData = aoInfoOfClass.Where(a => a.ColomXls>=0 ).ToList();
                // Start Exels untuk di imports
                int TotalUpload = 0;
                try
                {
                    for (int i = oRowHeader; i < maxloop; i++)
                    {
                        TotalUpload = i;
                        //icolName = eIdxfCountry.Code.GetHashCode() - 1;
                        //icolName = alEnumXls[eIdxfCountry.Code.GetHashCode()].ColomXls;

                        xlsValue = worksheet.Cells[i, 0].Value.ToString();
                        //colXls = alEnumXls[eIdxfCountry.Description.GetHashCode()].ColomXls;

                        //if (xlsValue.Trim().Length > 0
                        //    &&
                        //  (worksheet.Cells[i, 1].Value.ToString().Length > 2 || objMaster != null))
                        List<InfoOfClass> ObjectToSave = new List<InfoOfClass>();
                        IsSearch oIsSearch = new IsSearch();

                        // 
                        foreach (InfoOfClass eInfoOfClass in aoInfoOfClassHasXlsData)
                        {


                            InfoOfClass oInfoOfClass = eInfoOfClass;
                            oInfoOfClass.ClassInfo = ObjTypeClassInfo;

                            try
                            {
                                // ==> IS DATA READY TO UPLOAD ?
                                string sData = worksheet.Cells[i, eInfoOfClass.ColomXls].Value.ToString();
                                oInfoOfClass.setDataObject(Session, sData);
                                ObjectToSave.Add(oInfoOfClass);
                            }
                            catch (Exception e)
                            {

                            }
                        }
                        // ==> is any data 
                        if (oGetKey.IsAnyData(ObjectToSave))
                        {
                            // ==> CREATE QUERY STRING 
                            sqlQuery = oIsSearch.CreateriaClasstoSQLQuery(ObjectToSave);
                            CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);
                            object objClass = Session.FindObject(ObjType, CriteriaOperator.Parse(sqlQuery));
                            if (objClass == null)
                            {
                                
                                   objClass = ObjTypeClassInfo.CreateNewObject(Session);
                            }
                            Type otype = objClass.GetType();
                            List<InfoOfClass> lObjectToSave = ObjectToSave.Where(a => a.HasData).ToList();
                            foreach (InfoOfClass oInfoOfClass in lObjectToSave)
                            {
                                PropertyInfo info = otype.GetProperty(oInfoOfClass.Name);
                                info.SetValue(objClass, oInfoOfClass.Value);

                            }
                            Session.Save(objClass);
                            Session.CommitTransaction();

                        }
                        else
                        {
                            blankid = blankid + 1;
                            if (blankid > 100)
                            {
                                i = maxloop + 1;
                            }

                        }



                        // ============= BREACK HERE TO NEW ======================
                        //if (xlsValue.Trim().Length < 0)
                        //{

                        //    //List<InfoOfClass> sCreteria = oGetKey.CreateriaClass(ObjType);

                        //    //sqlQuery = string.Format(oGetKey.CreateriaClasstoSQLQuery(sCreteria, eSetDataObject.Key),
                        //    //xlsValue.Trim().Replace("'", "''"));

                        //    // IsSearch oIsSearch = new IsSearch();
                        //    //string sFind = oIsSearch.CreateriaClasstoSQLQuery(ObjType, eSetDataObject.Key);
                        //    sqlQuery = string.Format(oIsSearch.CreateriaClasstoSQLQuery(ObjType, eSetDataObject.Key),
                        //    xlsValue.Trim().Replace("'", "''"));
                        //    CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);
                        //    object objClass = Session.FindObject(ObjType, CriteriaOperator.Parse(sqlQuery));

                        //    // create jika data tidak di ketemukan

                        //    if (objClass == null)
                        //    {
                        //        objClass = ObjTypeClassInfo.CreateNewObject(Session);
                        //    }
                        //    Type otype = objClass.GetType();

                        //    //==> mulai melakukam import data 
                        //    foreach (InfoOfClass eInfoOfClass in aoInfoOfClass)
                        //    {
                        //        PropertyInfo info = otype.GetProperty(eInfoOfClass.Name);

                        //        //info.CustomAttributes.ToList();
                        //        //info.PropertyType.Attributes(info.GetCustomAttributes)

                        //        // info.PropertyType.GetProperty()

                        //        //info.CustomAttributes.GetType(XPObject.s SizeAttribute)

                        //        //  Enumerable b = info.CustomAttributes;
                        //        // ====> check apakah ada data nya  =====
                        //        if (eInfoOfClass.HasData)
                        //        {
                        //            // ===> assign data 
                        //            string sData = worksheet.Cells[i, eInfoOfClass.ColomXls].Value.ToString();
                        //            //List<CustomAttribute> olist = info.CustomAttributes.ToList(CustomAttribute);
                        //            //  foreach ( CustomAttribute b in olist)
                        //            //{
                        //            //     if ( b.Name=="")
                        //            //    {

                        //            //    }
                        //            //}
                        //            //foreach (CustomAttribute attr in System.Attribute.GetCustomAttributes(info))
                        //            //{
                        //            //    if (attr.Name == "")
                        //            //    {
                        //            //    }
                        //            //}
                        //            //==> check leng of data
                        //            // ==> convert data 
                        //            object oData = oGetKey.setDataObject(Session, info.PropertyType, sData);
                        //            // ==== REMARKS 
                        //            //info.SetValue(objClass, oData);
                        //        }


                        //    }
                        //    if (objMaster != "" && sMasterPropertyName != "")
                        //    {
                        //        PropertyInfo info = otype.GetProperty(sMasterPropertyName);
                        //        object oData = oGetKey.setDataObject(Session, info.PropertyType, objMaster, eSetDataObject.Oid);
                        //        // object oData = oGetKey.setDataObject(Session, info.PropertyType, objMaster,eSetDataObject.Oid);
                        //        info.SetValue(objClass, oData);
                        //    }
                        //    Session.Save(objClass);
                        //    //Session.SaveAsync(objClass);

                        //}
                        //else
                        //{
                        //    blankid = blankid + 1;
                        //    if (blankid > 100)
                        //    {
                        //        i = maxloop + 1;
                        //    }
                        //}
                    }
                    // Session.CommitTransactionAsync();
                    //  Session.CommitTransaction();
                    //}
                    
                }
                catch (Exception ex)
                {
                    //  Session.RollbackTransaction();

                    //iEmailService.WriteToFile($"  Upload Budget  --> Indicator :  Rollback  {ex.Message} // {ex.StackTrace}  ");
                }

            }
        }
        private UserLogin _Updateby;
        [XafDisplayName("Update by"), ToolTip("Update by")]
        public virtual UserLogin Updateby
        {
            get { return _Updateby; }
            set { SetPropertyValue("Updateby", ref _Updateby, value); }
        }
        // 
        private DateTime _Updatedate;
        [XafDisplayName("Update date"), ToolTip("Update date")]
        public virtual DateTime Updatedate
        {
            get
            {
                if (_Updatedate != null)
                {
                    _Updatedate = DateTime.Now;
                }
                return _Updatedate;
            }
            set { SetPropertyValue("Updatedate", ref _Updatedate, value); }
        }
    }

}
