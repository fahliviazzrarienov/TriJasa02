// Class Name : fCurrency.cs 
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
using DevExpress.Spreadsheet;
using System.IO;
using System.Diagnostics;
using DevExpress.Xpo.Metadata;
using DevExpress.CodeParser;
using System.Reflection;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using DevExpress.Persistent.BaseImpl;
using System.Security;
//using System.Reflection;
// new files
namespace TriJasa.Module.BusinessObjects
{
    [DefaultClassOptions]
    [ImageName("ModelEditor_Views")]
    [DefaultProperty("Description")]
    [NavigationItem("Master")]

    [System.ComponentModel.DisplayName("Import Files")]
    public class oImportFiles : XPObject
    {
        //private bool stsSave;
        public oImportFiles() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }
        public oImportFiles(Session session) : base(session)
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

          //  UpdateTempate("cetho.Ecosys.BusinessObjects.fBank");

            //LastUpdateUser = Session.FindObject<GPUser>(new BinaryOperator("UserName", SecuritySystem.CurrentUserName.ToString()));
            // LastUpdateUser = Session.FindObject<GPUser>(new BinaryOperator("UserName", tUser));
            // LastUpdate = DateTime.Now;
        }
        protected override void OnSaving()
        {
            //stsSave = false;
            //Attachfiles.toSave();
            base.OnSaving();
             //Import(); 
        }
        protected override void OnDeleting()
        {
            base.OnDeleting();
        }

        //private List<string> lTemplate()
        //{
        //    List<string> sTemplate = new List<string>();

        //    return sTemplate;

        // }

        public void Import<T>(T ObjType,string objMaster=null, string sMasterPropertyName = "") where T:Type
        {
            ImportData(ObjType, objMaster, sMasterPropertyName);
        }
        public void Import(string MasterOid="")
        {
           

            switch (DataType)
            {
                //case "fLanguage":
                //    //fLanguage oLanguage = new fLanguage(Session);
                //    ImportLanguage();//(oLanguage);
                //    break;
                //case "fCountry":
                //    ImportCountry();
                //    //      StopService();
                //    break;
                    
                //case "fCurrency":
                //    ImportCurrency();
                //    //      StopService();
                //    break;
                //case "fRegion":
                //     ImportRegion();
                //    //      StopService();
                //    break;
                //case "fCompany":
                   
                //    if (MasterObject == "fChartOfAccts")
                //    {
                //        ImportCompany(MasterOid);
                //    }
                //    else
                //    {
                //        ImportCompany();
                //    }
                //    break;
                //case "fChartOfAccts":
                //    ImportChartOfAccts();
                //    break;
                //case "fAcctsGroup":
                //    if (MasterObject == "fChartOfAccts")
                //    {
                //        ImportAcctsGroup(MasterOid);
                //    }
                //    //else
                //    //{
                //    //   // ImportAcctsGroup();
                //    //}
                //    break;

                case "fGLAcct":

                    //ImportData(typeof(fGLAcct));
                    break;
                default:
                    Console.WriteLine(String.Format("Unknown command: "));
                    break;
            }

            //Object objClass;
            //ImportData oImp = new ImportData(Session);
            //Attachfiles.toSave();
            //fLanguage ob;
            //if (!stsSave)
            //{
            //    stsSave = true;
            //   // oImp.Import(this);
            //}
            //if (DataType=="f")
            //{

            //    oImp.Import(this, fLanguage objClass, Session);
            //}


        }


        
        private Tuple<bool, Worksheet> Openfiles()  
        {
            bool bhasFile = false;
            Worksheet worksheet=null;
            FileSystemStoreObject Attachfiles;
            Attachfiles = this.Attachfiles;
            if (Attachfiles != null && Attachfiles.RealFileName != "" && Attachfiles.RealFileName != null)
            {
                bhasFile = true;
                Workbook workbook = new Workbook();
                // Load a workbook from the stream. 
                using (FileStream stream = new FileStream(Attachfiles.RealFileName, FileMode.Open))
                {
                    workbook.LoadDocument(stream, DevExpress.Spreadsheet.DocumentFormat.OpenXml);
                }

                worksheet = workbook.Worksheets[0];
            }
                return new Tuple<bool, Worksheet>(bhasFile, worksheet);
        }

        public void ImportData<T>(T ObjType, string objMaster,string sMasterPropertyName="") where T : Type
        {
            
            int maxloop = 2147483640;
            // open file xls
            var fCheck = Openfiles();
            int blankid = 0;
            // check if the xls has data
            if (fCheck.Item1)
            {
                Worksheet worksheet = fCheck.Item2;
                string sqlQuery = "";
                string xlsValue;


                try
                {
                    // remove sementara
                    Session.BeginTransaction();
                  
                    iGetKey oGetKey = new iGetKey();
                    Type objMasterType=objMaster.GetType();
                    //eIdxfCountry eC;
                    //eC = eIdxfCountry.ID;
                    
                    List<InfoOfClass> aoInfoOfClass = oGetKey.GetObjProperty(ObjType, Session, worksheet);
                    XPClassInfo ObjTypeClassInfo = Session.GetClassInfo(ObjType);

                    for (int i = 1; i < maxloop; i++)
                    {
                        //icolName = eIdxfCountry.Code.GetHashCode() - 1;
                        //icolName = alEnumXls[eIdxfCountry.Code.GetHashCode()].ColomXls;

                        xlsValue = worksheet.Cells[i, 0].Value.ToString();
                        //colXls = alEnumXls[eIdxfCountry.Description.GetHashCode()].ColomXls;

                        if (xlsValue.Trim().Length > 0 && 
                            (worksheet.Cells[i, 1].Value.ToString().Length > 2 || objMaster!=null))
                        {

                            //List<InfoOfClass> sCreteria = oGetKey.CreateriaClass(ObjType);

                            //sqlQuery = string.Format(oGetKey.CreateriaClasstoSQLQuery(sCreteria, eSetDataObject.Key),
                            //xlsValue.Trim().Replace("'", "''"));

                            IsSearch oIsSearch = new IsSearch();
                            //string sFind = oIsSearch.CreateriaClasstoSQLQuery(ObjType, eSetDataObject.Key);
                            sqlQuery = string.Format(oIsSearch.CreateriaClasstoSQLQuery(ObjType, eSetDataObject.Key),
                            xlsValue.Trim().Replace("'", "''"));
                            CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);
                            object objClass = Session.FindObject(ObjType, CriteriaOperator.Parse(sqlQuery));
                            
                            if (objClass == null )
                            {
                                objClass = ObjTypeClassInfo.CreateNewObject(Session);
                            }
                            Type otype = objClass.GetType();
                            foreach (InfoOfClass eInfoOfClass in aoInfoOfClass)
                            {
                                PropertyInfo info = otype.GetProperty(eInfoOfClass.Name);

                                //info.CustomAttributes.ToList();
                                //info.PropertyType.Attributes(info.GetCustomAttributes)
                                
                                // info.PropertyType.GetProperty()

                                //info.CustomAttributes.GetType(XPObject.s SizeAttribute)
                                //  Enumerable b = info.CustomAttributes;
                                if (eInfoOfClass.HasData)
                                    {
                                       string sData = worksheet.Cells[i, eInfoOfClass.ColomXls].Value.ToString();
                                    //List<CustomAttribute> olist = info.CustomAttributes.ToList(CustomAttribute);
                                    //  foreach ( CustomAttribute b in olist)
                                    //{
                                    //     if ( b.Name=="")
                                    //    {

                                    //    }
                                    //}
                                    foreach (CustomAttribute attr in System.Attribute.GetCustomAttributes(info))
                                    {
                                        if (attr.Name == "")
                                        { 
                                        }
                                    }
                                    object oData = oGetKey.setDataObject(Session, info.PropertyType, sData);
                                    info.SetValue(objClass, oData);
                                    }
                                    

                            }
                            if (objMaster != "" && sMasterPropertyName != "")
                            {
                                PropertyInfo info = otype.GetProperty(sMasterPropertyName);
                                object oData = oGetKey.setDataObject(Session, info.PropertyType, objMaster,eSetDataObject.Oid);
                                // object oData = oGetKey.setDataObject(Session, info.PropertyType, objMaster,eSetDataObject.Oid);
                                info.SetValue(objClass, oData);
                            }
                            //Session.Save(objClass);
                            Session.SaveAsync(objClass);
                            
                        }
                        else
                        {
                            blankid = blankid + 1;
                            if (blankid > 100)
                            {
                                i = maxloop + 1;
                            }
                        }
                    }
                    Session.CommitTransactionAsync();
                    //Session.CommitTransaction();
                }
                catch (Exception ex)
                {
                Session.RollbackTransaction();
                
                //iEmailService.WriteToFile($"  Upload Budget  --> Indicator :  Rollback  {ex.Message} // {ex.StackTrace}  ");
                }
        }
        }
        private PropertyInfo PropertyInfoByName(string name)
        {
            Type type = this.GetType();
            PropertyInfo info = type.GetProperty(name);
            if (info == null)
                throw new Exception(String.Format(
                  "A property called {0} can't be accessed for type {1}.",
                  name, type.FullName));
            return info;
        }

        public object this[string name]
        {
            get
            {
                PropertyInfo info = PropertyInfoByName(name);
                return info.GetValue(this, null);
            }
            set
            {
                PropertyInfo info = PropertyInfoByName(name);
                info.SetValue(this, value, null);
            }
        }

        private string _Description;
        [XafDisplayName("Description"), ToolTip("Description")]
        [VisibleInDetailView(false)]
        [Size(250)]
        public virtual string Description
        {
            get { return _Description; }
            set { SetPropertyValue("Description", ref _Description, value); }
        }
    
        private string _DataType;
        [XafDisplayName("DataType"), ToolTip("DataType")]
        [VisibleInDetailView(false)]
        public virtual string DataType
        {
            get { return _DataType; }
            set { SetPropertyValue("DataType", ref _DataType, value); }
        }
        private string _MasterObject;
        [XafDisplayName("MasterObject"), ToolTip("MasterObject")]
        [VisibleInDetailView(false)]
        public virtual string MasterObject
        {
            get { return _MasterObject; }
            set { SetPropertyValue("MasterObject", ref _MasterObject, value); }
        }


        //[Action("Template")]
       
        //private byte[] data;
        //[EditorAlias(EditorAliases.SpreadsheetPropertyEditor)]
        //public byte[] Data
        //{
        //    get { return data; }
        //    set { SetPropertyValue(nameof(Data), ref data, value); }
        //}

        [XafDisplayName("File"), ToolTip("File")]
        public FileSystemStoreObject Attachfiles
        {
            get { return GetPropertyValue<FileSystemStoreObject>("Attachfiles"); }
            set
            {
                SetPropertyValue<FileSystemStoreObject>("Attachfiles", value);
                if (!IsLoading && !IsSaving)
                {
                    //ValidateStatus = false;
                    //Validatexls();
                }
            }
        }

        //[Association("ImportColomns")]
        //public XPCollection<ImportColomn> Coloms
        //{
        //    get
        //    {
        //        return GetCollection<ImportColomn>(nameof(ImportColomn));
        //    }
        //}

        public void UpdateTempate(string sDataType)
        {
            DataType = sDataType;
            UpdateTempate();

        }
        private  string FileSystemStoreLocation = String.Format("{0}FileData", PathHelper.GetApplicationFolder());
        public void UpdateTempate()
        {

            // ...
            Worksheet worksheet;
            Workbook workbook = new Workbook();
            workbook.Worksheets[0].Name = DataType.Replace("TriJasa.Module.BusinessObjects.", "");
            worksheet = workbook.Worksheets[0];
            //worksheet.Name= "worksheet"
            // Add a reference to the DevExpress.Docs.dll assembly.
            List<string> sTemplate = new List<string>();
            iGetKey oGetKey = new iGetKey();
            Type myType = Type.GetType(DataType);
            GetFieldname(myType, worksheet);

            //Workbook workbook = new Workbook();
            //worksheet.Import(oList, 0, 1, true);
            //workbook.Worksheets[0].Import(sTemplate, 0, 0, false);

            // Save a document to a byte array to store it in a database.
            try
            {
                string sfileName = DataType.Replace("TriJasa.Module.BusinessObjects.", "") + ".xlsx";
                workbook.SaveDocument(Path.Combine(FileSystemStoreLocation, sfileName), DocumentFormat.Xlsx);
                byte[] docBytes = workbook.SaveDocument(DocumentFormat.Xlsx);
            }
            catch(Exception e)
            {

            }
            //Template = workbook.SaveDocument(DocumentFormat.Xlsx);
          
            //using (var stream = new MemoryStream())
            //{
            //    file.SaveToStream(stream);
            //    //currentFile.SaveToStream(stream);
            //    stream.Position = 0;
            //    file.LoadFromStream(Path.Combine(FileSystemStoreLocation, "SavedDocument.xlsx"), stream);
            //}

            //file.LoadFromStream
         //   File.FileName = Path.Combine(FileSystemStoreLocation, "SavedDocument.xlsx");
           // File.LoadFromStream( "xls.x.s", Template);
            //File.LoadFromStream(docBytes);
            // File.LoadFromStream(currentFile.FileName, stream);
            // ...
            //FileData currentFile = docBytes;// workbook.SaveDocument(DocumentFormat.Xlsx);

            // Load the saved document from a byte array into the Workbook instance.
            //workbook.LoadDocument(docBytes);


        }
        
        private void GetFieldname<T>(T oType, Worksheet ws) where T : Type
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
                if (!alist.Contains(fieldName))
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
        


    }

   

    }
