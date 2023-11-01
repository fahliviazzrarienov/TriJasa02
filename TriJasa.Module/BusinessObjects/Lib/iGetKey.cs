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
//using DevExpress.ClipboardSource.SpreadsheetML/*/*;*/*/
using DevExpress.Spreadsheet;
using System.IO;
//using System.Activities.Expressions;
using DevExpress.Xpo.Metadata;
using System.Reflection;
//using System.Collections;
// new files
namespace TriJasa.Module.BusinessObjects
{
    //[DomainComponent]
    // [DefaultClassOptions]
    //[ImageName("BO_Unknown")]
    //[DefaultProperty("SampleProperty")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class iGetKey
    {


        //public nRegion Region(Session session, string sCountry, string sRegion)
        //{
        //    nRegion oRegion = null;
        //    string sqlQuery = string.Format(" Code == '{0}' || AltrnCountryKey  == '{0}' || Description  == '{0}' || NameLong  == '{0}'", sCountry.Replace("'", "''"));
        //    CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);
        //    fCountry oCountry = session.FindObject<fCountry>(filterOperator);
        //    if (oCountry != null)
        //    {
        //        sqlQuery = string.Format(" ( Code == '{0}' || ShortText  == '{0}' || Description  == '{0}' || Capital  == '{0}' ) && Country.Oid == {1} "
        //                 , sRegion.Replace("'", "''"), oCountry.Oid.ToString());
        //        filterOperator = CriteriaOperator.Parse(sqlQuery);
        //        oRegion = session.FindObject<fRegion>(filterOperator);

        //    }

        //    return oRegion;
        //}
        public iGetKey()
        {

        }
        public Worksheet worksheet { get; set; }

        public XPCollection<xlsFileColomn> xlsColoms { get; set; }
        public XPCollection<xlsFileColomn> xlsHeader(Session oSession, int Rowheader = 1)
        {

            int maxloop = 100;
           // xlsColoms.Clear();
            xlsColoms = new XPCollection<xlsFileColomn>(oSession);
            int lastEmpty = 0;
            if (worksheet != null)
            {
                for (int i = 0; i < maxloop; i++)
                {
                    string xlsValue;
                    xlsValue = worksheet.Cells[Rowheader, i].Value.ToString().Trim();
                    if (xlsValue != "")
                    {
                        xlsFileColomn oColom = new xlsFileColomn(oSession);
                        oColom.ColomName = xlsValue;
                        xlsColoms.Add(oColom);

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
            return xlsColoms;
        }

        //public void UpdateByAndTime<T>(T ObjType, Session session) where T : Type
        //{
        //    string tUser = SecuritySystem.CurrentUserName.ToString();
        //    ObjType.Updateby = Session.FindObject<User>(new BinaryOperator("UserName", tUser));
        //    Updatedate = DateTime.Now;
        //}
        public Tuple<bool, string> OpenFileWorkSheet(FileSystemStoreObject Attachfiles)
        {
            //BindingList<ImportColom> iColomName;
            string arlist = "";
            bool bhasFile = false;
            if (Attachfiles != null && Attachfiles.FileName != "")
            {
                bhasFile = true;
                Workbook workbook = new Workbook();
                // Load a workbook from the stream. 
                using (FileStream stream = new FileStream(Attachfiles.RealFileName, FileMode.Open))
                {
                    workbook.LoadDocument(stream, DevExpress.Spreadsheet.DocumentFormat.OpenXml);
                }
                foreach (Worksheet iW in workbook.Worksheets)
                {
                    arlist += iW.Name + "|";
                }
                //worksheet = workbook.Worksheets[0];
            }
            return new Tuple<bool, string>(bhasFile, arlist);
        }
        public Tuple<bool, Worksheet> Openfiles(FileSystemStoreObject Attachfiles, string Worksheet="")
        {
            //BindingList<ImportColom> iColomName;
            bool bhasFile = false;
            // Worksheet worksheet = null;
            //NPFileData  Attachfiles;
            //Attachfiles = Attachfiles;
            if (Attachfiles != null && Attachfiles.FileName != "")
            {
                bhasFile = true;
                Workbook workbook = new Workbook();
                // Load a workbook from the stream. 
                using (FileStream stream = new FileStream(Attachfiles.RealFileName, FileMode.Open))
                {
                    workbook.LoadDocument(stream, DevExpress.Spreadsheet.DocumentFormat.OpenXml);
                }


                // workbook.LoadDocument(Attachfiles.LoadFromStream, DevExpress.Spreadsheet.DocumentFormat.OpenXml);

                //System.IO.Stream stream= new System.IO.Stream();
                //Attachfiles.LoadFromStream(Attachfiles.FileName, stream);
                //workbook.LoadDocument(Attachfiles.Content, DevExpress.Spreadsheet.DocumentFormat.OpenXml);
                if (Worksheet != "")
                {
                    worksheet = workbook.Worksheets[Worksheet];
                }
                else
                {
                    worksheet = workbook.Worksheets[0];
                }
            }
            return new Tuple<bool, Worksheet>(bhasFile, worksheet);
        }
        public Tuple<bool, int> isInt(string oValue)
        {
            int rValue;
            Boolean bValue = false;
            oValue = oValue.Trim();
            if (oValue == "")
            {
                oValue = "0";
            }

            if (int.TryParse(oValue, out rValue))
            {
                bValue = true;
            }

            return new Tuple<bool, int>(bValue, rValue);
        }



        //public List<ENumXlsIndex> lEnumXls(eIdxfAcctsGroup eObj, Worksheet worksheet)
        //{
        //    List<ENumXlsIndex> olEnumXls = new List<ENumXlsIndex>();
        //    foreach (var value in Enum.GetValues(typeof(eIdxfAcctsGroup)))
        //    {
        //        ENumXlsIndex oCls = new ENumXlsIndex();
        //        oCls.IdxEnum = (int)value;
        //        oCls.Name = value.ToString();
        //        oCls.HasData = false;
        //        oCls.ColomXls = 1;
        //        olEnumXls.Add(oCls);
        //    }
        //    //string xlsValue;

        //    olEnumXls = lEnumXls(olEnumXls, worksheet);
        //    return olEnumXls;
        //}


        //public enum Days { Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday }
        //var result = EnumToclientEnum(typeof(Days)); //IEnumerable
        //var listified = result.ToList(); //List
        public static IEnumerable<clientEnum> EnumToclientEnumShort<T>(T enumType) where T : Type
        {
            foreach (var value in Enum.GetValues(enumType))
                yield return new clientEnum
                {
                    Key = (int)Enum.Parse(enumType,
                                       value.ToString()),
                    Value = value.ToString()
                };
        }

        public List<string> FieldNoshow()
        {
            List<string> list = new List<string>();

            list.Add("OID".ToUpper());
            list.Add("OptimisticLockField".ToUpper());
            list.Add("GCRecord".ToUpper());
            list.Add("ClassInfo".ToUpper());
            list.Add("IsDeleted".ToUpper());
            list.Add("IsLoading".ToUpper());
            list.Add("Loading".ToUpper());
            list.Add("Session".ToUpper());
            list.Add("This".ToUpper());
            list.Add("Updateby".ToUpper());
            list.Add("Updatedate".ToUpper());
            list.Add("Createby".ToUpper());
            list.Add("Createdate".ToUpper());
            
             return list;
        }

        public List<string> ListOfEnum()
        {
            List<string> list = new List<string>();

            list.Add("eNumAcctsCtrlIntgration");
            list.Add("eNumPLBS");
            list.Add("eNumDebtCredit");
            list.Add("eNumAccountType");
            list.Add("eSex");

            //list.Add("eNumAcctsCtrlIntgration");

            return list;
        }

        public List<string> ListOfNumeric()
        {
            List<string> list = new List<string>();
            
            list.Add("System.Int16");
            list.Add("System.Int32");
            list.Add("System.Int64");
            list.Add("System.Double");
            list.Add("Int16");
            list.Add("Int32");
            list.Add("Int64");
            list.Add("Double");

            return list;
        }

        public List<string> EnumToListString<T>(T enumType, bool IsNotIdx = true) where T : Type
        {
            List<string> list = new List<string>();

            foreach (var value in Enum.GetValues(enumType))
            {
                var name = value.ToString();
                var number = Enum.Parse(enumType, name);
                // list.Add(new clientEnum { Key = (int)number, Value = name });
                if (IsNotIdx)
                {
                    if ((int)number > 0)
                    {
                        list.Add(name);
                    }

                }
                else
                {
                    list.Add(name);
                }
            }
            return list;
        }

     
        public List<ENumXlsIndex> lEnumXls(List<ENumXlsIndex> olEnumXls, Worksheet worksheet)
        {

            string xlsValue;
            for (int i = 0; i < olEnumXls.Count; i++)
            {

                xlsValue = worksheet.Cells[0, i].Value.ToString().Trim();

                int id = 0;
                foreach (ENumXlsIndex value in olEnumXls)
                {
                    if (xlsValue.ToLower() == value.Name.ToLower())
                    {
                        olEnumXls[id].ColomXls = i;
                        olEnumXls[id].HasData = true;
                        //break;
                    }
                    id++;

                }
            }
            return olEnumXls;
        }

        public Tuple<bool, DateTime> isDate(string oDate)
        {
            DateTime dateValue;
            Boolean bdate = false;
            if (DateTime.TryParse(oDate, out dateValue))
            {
                bdate = true;
            }

            return new Tuple<bool, DateTime>(bdate, dateValue);
        }
        public Tuple<bool, bool> isBool(string oBool)
        {
            bool bValue;
            Boolean bBool = false;
            if (oBool == "1")
            {
                oBool = "True";
            }
            else if (oBool == "0")
            {
                oBool = "False";
            }

            if (Boolean.TryParse(oBool, out bValue))
            {
                bBool = true;
            }
            else
            {
                bValue = false;
            }
            return new Tuple<bool, bool>(bBool, bValue);
        }
        public bool setDataBoolean(string oBool)
        {
            bool bValue;
            oBool = oBool.Trim().ToLower();
            Boolean bBool = false;
            if (oBool == "1" ||
                oBool == "yes" ||
                oBool == "a"
               )
            {
                oBool = "True";
            }
            else if (oBool == "0" ||
                    oBool == "no" ||
                    oBool == "t"
                   )
            {
                oBool = "False";
            }


            if (Boolean.TryParse(oBool, out bValue))
            {
                bBool = true;
            }
            else
            {
                bValue = false;
            }
            return  bValue;
        }

        public object GetObject<T>(Session session, T oType,int oid) where T : Type
        {
   
            string sqlQuery = $" Oid== {oid} ";
            object objClass = session.FindObject(oType, CriteriaOperator.Parse(sqlQuery));
            return objClass;
        }
        public  object setDataObject<T>(Session session, T oType,string oValue,eSetDataObject oSetDataObject =eSetDataObject.None) where T : Type
        {
            //DateTime dateValue;
            object oDataValue =null;
            //Type oDataValue = oType.GetType();

            Boolean bdate = false;
            if (oType.Name == "DateTime")
            {
                DateTime oValueOut;
                if (DateTime.TryParse(oValue, out oValueOut))
                {
                    bdate = true;
                }
                oDataValue = oValueOut;
            }

            else if (oType.Name == "Boolean")
            {
                oDataValue = setDataBoolean(oValue);
            }

            else if (oType.Name == "String")
            {
                
                oDataValue = oValue;
            }
            else if (ListOfNumeric().Contains(oType.Name))
            {
                double oIntOut;
                if (!Double.TryParse(oValue, out oIntOut))
                {
                    // it is a number
                    oIntOut = 0;
                }
                object iValue = Convert.ChangeType(oIntOut, oType);
                //Convert.ChangeType()
                oDataValue = iValue;
            }
            // enum data
            else if (ListOfEnum().Contains(oType.Name))
            {
                //// oData = worksheet.Cells[i, colXls].Value.ToString();
                //if (oType.Name == "eNumPLBS")
                //{
                //    oDataValue = PLBS(oValue);
                //}
                //else if (oType.Name == "eNumAcctsCtrlIntgration")
                //{
                //    oDataValue = AcctsCtrlIntgration(oValue);
                //}
                //else if (oType.Name == "eNumAcctsCtrlIntgration")
                //{
                //    oDataValue = AcctsCtrlIntgration(oValue);
                //}
                //else if (oType.Name == "eNumDebtCredit")
                //{
                //    oDataValue = DebtCredit(oValue);
                //}
                //else if (oType.Name == "eNumAccountType")
                //{
                //    oDataValue = AccountType(oValue);
                //}
                //oDataValue = Enum.Parse(oType, oValue);

            }
            else
            /// object data
            {

                IsSearch isSearch = new IsSearch();

                List<InfoOfClass> sCreteria = isSearch.CreateriaClass(oType);
                string sqlQuery = "";
                // XPClassInfo ObjTypeClassInfo = session.GetClassInfo(oType);
                
                if(oSetDataObject == eSetDataObject.Oid)
                {
                    sqlQuery = string.Format(" Oid== {0} ",
                    oValue.Trim().Replace("'", "''"));
                }
                else
                {
                     sqlQuery = string.Format(CreateriaClasstoSQLQuery(sCreteria, oSetDataObject),
                     oValue.Trim().Replace("'", "''"));
                }
                CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);
                object objClass = session.FindObject(oType, CriteriaOperator.Parse(sqlQuery));
                oDataValue = objClass;
            }

            return oDataValue;
        }
        public bool IsAnyData(List<InfoOfClass> olInfoOfClass)
        {
            bool isAnyData = false;

            int total = olInfoOfClass.Count;
            try
            {
                List<InfoOfClass> query;
                //query = olInfoOfClass.Where(a => a.Value.ToString() == "").ToList();
                query = olInfoOfClass.Where(a => a.Value !=null).ToList();
                if (query != null)
                {
                    query = olInfoOfClass.Where(a => a.isKey && a.Value.ToString() == "").ToList();
                    if (query == null)
                    {
                        isAnyData = true;
                    }
                    else if (query.Count == 0)
                    {
                        query = olInfoOfClass.Where(a => a.isKey ).ToList();
                        query = query.Where(a => a.Value.ToString() != "").ToList();
                        if (query.Count > 0)
                        {
                            isAnyData = true;
                        }
                    }
                   
                }
              
            }
            catch (Exception e)
            {

            }

            return isAnyData;
        }

        public List<Type> GetClassList()
        {
            List<Type> types = new List<Type>();
            var assembly = Assembly.GetExecutingAssembly();
            foreach (var type in assembly.GetTypes())
            {
                if (type.Namespace == "TriJasa.Module.BusinessObject")
                {
                    types.Add(type);
                }
            }
            return types;
        }
        //public T GetEnumIndex<T>(T oEnum ) where T : Type
        //{
        //    T oValue;

        //    foreach (var value in Enum.GetValues(oEnum))
        //    {
        //        ENumXlsIndex oCls = new ENumXlsIndex();
        //        oCls.IdxEnum = (int)value;
        //        oCls.Name = value.ToString();
        //        oCls.HasData = false;
        //        oCls.ColomXls = 1;

        //    }
        //    return (T) oValue;
        //}
        public string CreateriaClasstoSQLQuery(List<InfoOfClass> aList, eSetDataObject oSetDataObject = eSetDataObject.None)
        {
            string sQuery = "";
            
            if (oSetDataObject==eSetDataObject.Key)
            {
                List<InfoOfClass> query = aList.Where(a => a.isKey).ToList();
                aList = query;


            }

            foreach (InfoOfClass value in aList)
            {
                sQuery += " Lower(" + value.ForSearch + ") == '{0}' ||";
            }
            try
            {
                sQuery = sQuery.Substring(1, sQuery.Length - 3);
            }
            catch (Exception ex)
            {
                sQuery = "";
            }
          return sQuery;

        }
        public List<InfoOfClass> CreateriaClass<T>(T oType) where T : Type
        {
            List<InfoOfClass> LCreateriaClass= new List<InfoOfClass>();
            //XPClassInfo classInfo = session.Dictionary.GetClassInfo(oType);
            if (oType.Name.ToLower() == "fbank".ToLower())
            {
                LCreateriaClass.Add(CreateriaClassAdd(oType, "BankKey",true));
                LCreateriaClass.Add(CreateriaClassAdd(oType, "BankName"));
                LCreateriaClass.Add(CreateriaClassAdd(oType, "BankNumber"));
            }
            else if (oType.Name.ToLower() == "fCompany".ToLower())
            {
                LCreateriaClass.Add(CreateriaClassAdd(oType, "Code", true));
                LCreateriaClass.Add(CreateriaClassAdd(oType, "Description"));
                LCreateriaClass.Add(CreateriaClassAdd(oType, "NameofCompany2"));
                LCreateriaClass.Add(CreateriaClassAdd(oType, "SearchTerm"));
             
            }
            else if (oType.Name.ToLower() == "fCountry".ToLower())
            {
                LCreateriaClass.Add(CreateriaClassAdd(oType, "Code", true));
                LCreateriaClass.Add(CreateriaClassAdd(oType, "AltrnCountryKey"));
                LCreateriaClass.Add(CreateriaClassAdd(oType, "Description"));
                LCreateriaClass.Add(CreateriaClassAdd(oType, "NameLong"));
                

            }
            else if (oType.Name.ToLower() == "fLanguage".ToLower())
            {
                LCreateriaClass.Add(CreateriaClassAdd(oType, "Code", true));
                LCreateriaClass.Add(CreateriaClassAdd(oType, "Description"));
                LCreateriaClass.Add(CreateriaClassAdd(oType, "ShortText"));
                
            }
            else if (oType.Name.ToLower() == "fCurrency".ToLower())
            {
                LCreateriaClass.Add(CreateriaClassAdd(oType, "Code", true));
                LCreateriaClass.Add(CreateriaClassAdd(oType, "Description"));
                LCreateriaClass.Add(CreateriaClassAdd(oType, "ShortText"));
                LCreateriaClass.Add(CreateriaClassAdd(oType, "ISOCode"));
                LCreateriaClass.Add(CreateriaClassAdd(oType, "AlternativeKey"));
            }
            else if (oType.Name.ToLower() == "fRegion".ToLower())
            {
                LCreateriaClass.Add(CreateriaClassAdd(oType, "Code", true));
                LCreateriaClass.Add(CreateriaClassAdd(oType, "ShortText"));
                LCreateriaClass.Add(CreateriaClassAdd(oType, "Description"));

            }
            else if (oType.Name.ToLower() == "fChartOfAccts".ToLower())
            {
                LCreateriaClass.Add(CreateriaClassAdd(oType, "Code", true));
                LCreateriaClass.Add(CreateriaClassAdd(oType, "Description"));

            }
            else if (oType.Name.ToLower() == "fAcctsControl".ToLower())
            {
                //LCreateriaClass.Add(CreateriaClassAdd(oType, ""));
                //LCreateriaClass.Add(CreateriaClassAdd(oType, ""));
                //LCreateriaClass.Add(CreateriaClassAdd(oType, ""));
                //LCreateriaClass.Add(CreateriaClassAdd(oType, ""));
                //LCreateriaClass.Add(CreateriaClassAdd(oType, ""));
                //LCreateriaClass.Add(CreateriaClassAdd(oType, ""));
                //LCreateriaClass.Add(CreateriaClassAdd(oType, ""));
            }
            else if (oType.Name.ToLower() == "fAcctsGroup".ToLower())
            {
                LCreateriaClass.Add(CreateriaClassAdd(oType, "AcctGroup", true));
                LCreateriaClass.Add(CreateriaClassAdd(oType, "Name"));
                LCreateriaClass.Add(CreateriaClassAdd(oType, "FromAcct"));
                LCreateriaClass.Add(CreateriaClassAdd(oType, "ToAcct"));

            }
            else if (oType.Name.ToLower() == "fGLAcct".ToLower())
            {
                LCreateriaClass.Add(CreateriaClassAdd(oType, "GLAcct", true));
                LCreateriaClass.Add(CreateriaClassAdd(oType, "ShortText"));
                LCreateriaClass.Add(CreateriaClassAdd(oType, "LongText"));

            }
            else if (oType.Name.ToLower() == "fTaxCategory".ToLower())
            {
                LCreateriaClass.Add(CreateriaClassAdd(oType, "TaxCatgr", true));
                LCreateriaClass.Add(CreateriaClassAdd(oType, "ShortText"));
                LCreateriaClass.Add(CreateriaClassAdd(oType, "LongText"));

            }
            else if (oType.Name.ToLower() == "fBankAcct".ToLower())
            {
                LCreateriaClass.Add(CreateriaClassAdd(oType, "BankAcctName", true));
                LCreateriaClass.Add(CreateriaClassAdd(oType, "AlternativeAcctNo"));
                LCreateriaClass.Add(CreateriaClassAdd(oType, "ControlKey"));

            }
            else if (oType.Name.ToLower() == "fBankGroup".ToLower())
            {
                LCreateriaClass.Add(CreateriaClassAdd(oType, "AcctGroup", true));
                LCreateriaClass.Add(CreateriaClassAdd(oType, "Name"));
                LCreateriaClass.Add(CreateriaClassAdd(oType, "FromAcct"));
                LCreateriaClass.Add(CreateriaClassAdd(oType, "ToAcct"));

            }
            return LCreateriaClass;
        }
        public InfoOfClass CreateriaClassAdd<T>(T oType, string pName, bool isKey=false) where T : Type
        {
            InfoOfClass oClass = new InfoOfClass();
            PropertyInfo info = oType.GetProperty(pName);
            oClass.Class = oType.Name;
            oClass.ForSearch = info.Name;
            oClass.isKey = isKey;
            oClass.Datatype = info.PropertyType.Name;
            return oClass;
        }
        public static T GetTfromString<T>(string mystring)
        {
            var foo = TypeDescriptor.GetConverter(typeof(T));
            return (T)(foo.ConvertFromInvariantString(mystring));

            // usage
            //bool b = GetTfromString<bool>("true");</ bool >
        }

        public List<InfoOfClass> GetObjProperty<T>(T oType, Session session, XPCollection<ImportColomn> oXPColom) where T : Type
        {
            // ==> CREATE OBJECT TO HOLD THE STRUCTURE
            List<InfoOfClass> alist = new List<InfoOfClass>();
            // ==> GET INFORAMTION OF XPO CLASS
            XPClassInfo classInfo = session.Dictionary.GetClassInfo(oType);
            // ==> GET NAME OF FIELD NOT MATCH 
            List<string> aNotlist = FieldNoshow();

            foreach (XPMemberInfo info in classInfo.PersistentProperties)
            {
                string fieldName = info.MappingField;
                // check the name is not default system name
                if ( !aNotlist.Contains(fieldName.ToUpper()))
                {
                    // ==ADD PROPERTY TO OBJECT STURCTURE
                    InfoOfClass oInfoOfClass = new InfoOfClass();
                    oInfoOfClass.Name = fieldName;  
                    oInfoOfClass.Datatype = info.MemberType.ToString();

                    List<ImportColomn> loXPColom = oXPColom.Where(a => a.xlsName !=null && a.xlsName.ToString() !=""  ).ToList();

                    foreach (ImportColomn icolName in loXPColom)
                    {
                        if (icolName.ObjectName.ToLower() == oInfoOfClass.Name.ToLower())
                        {
                            oInfoOfClass.ColomXls = icolName.xlsName.ColomIdx;
                            oInfoOfClass.HasData = true;
                            // ==> CHHECK ATRRRIBUT 
                            Attribute[] listOfAttribut = info.Attributes;
                            foreach (Attribute ItemAtribut in listOfAttribut)
                            {
                                if (ItemAtribut is IsSearch)
                                {
                                    IsSearch attributeObject = (IsSearch)ItemAtribut;


                                    oInfoOfClass.isKey = attributeObject.isSearch;
                                    oInfoOfClass.allKey = attributeObject.allKey;
                                }
                                else if (ItemAtribut is SizeAttribute)
                                {

                                    SizeAttribute attributeObject = (SizeAttribute)ItemAtribut;
                                    oInfoOfClass.MaxStringValue = attributeObject.Size;

                                }
                                else if (ItemAtribut is IsParent)
                                {

                                    IsParent attributeObject = (IsParent)ItemAtribut;

                                    oInfoOfClass.Parent = attributeObject.isParent;
                                    

                                }

                                //oInfoOfClass.isKey
                            }
                        }
                    }
                    alist.Add(oInfoOfClass);
                }
            }

            return alist;
        }
        public List<InfoOfClass>  GetObjProperty<T>(T oType, Session session, Worksheet worksheet) where T : Type
        {

            List<InfoOfClass> alist= new List<InfoOfClass>();
            
            XPClassInfo classInfo = session.Dictionary.GetClassInfo(oType);
            List<string> aNotlist =FieldNoshow();
            int i = 10;
            
            foreach (XPMemberInfo info in classInfo.PersistentProperties)
            {
                i++;
                string fieldName = info.MappingField;
                // check the name is not default system name
                if (!aNotlist.Contains(fieldName))
                {
                    InfoOfClass oInfoOfClass = new InfoOfClass();
                    oInfoOfClass.Name = fieldName;
                    oInfoOfClass.Datatype= info.MemberType.ToString();
                   string xlsValue;
                    for (int j = 0; j <  i; j++)
                    {
                        xlsValue = worksheet.Cells[0, j].Value.ToString().Trim();
                        // check is the name is same with the excel value
                       if (xlsValue.ToLower() == oInfoOfClass.Name.ToLower())
                            {
                            // assign colom of xls
                            oInfoOfClass.ColomXls = j;
                            oInfoOfClass.HasData = true;
                           // j = i;
                                //break;
                            }
                    }
                    alist.Add(oInfoOfClass);
                }

            }
            return alist;
        }
        public List<InfoOfClass> GetObjExcelHeader(List<InfoOfClass> oInfoOfClass, Worksheet worksheet)
        {

            string xlsValue;
            for (int i = 0; i < oInfoOfClass.Count; i++)
            {

                xlsValue = worksheet.Cells[0, i].Value.ToString().Trim();

                int id = 0;
                foreach (InfoOfClass value in oInfoOfClass)
                {
                    if (xlsValue.ToLower() == value.Name.ToLower())
                    {
                        oInfoOfClass[id].ColomXls = i;
                        oInfoOfClass[id].HasData = true;
                        //break;
                    }
                    id++;

                }
            }
            return oInfoOfClass;
        }
    }

    //public enum eSetDataObject
    //{
    //    [XafDisplayName("None")]
    //    None = 0,
    //    [XafDisplayName("Key")]
    //    Key = 1,
    //    [XafDisplayName("Oid")]
    //    Oid = 2
    //}

    public class clientEnum
    {
        public string Value { get; set; }
        public int Key { get; set; }
        public string Datatype { get; set; }
    }
   

}