using DevExpress.ExpressApp.DC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
// new files
namespace TriJasa.Module.BusinessObjects
{
    //[AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public class IsHide : Attribute
    {
        public bool isHide { get; set; }
        public IsHide(bool ishide = true)
        {
            isHide = ishide;
        }
    }
    public class IsParent : Attribute
    {
        public eIsParent isParent { get; set; } = eIsParent.None;
        public IsParent(eIsParent isparent =eIsParent.None)
        {
            isParent = isparent;
        }
    }
    public class IsSearch : Attribute
    {
        // Private fields 
        //private string title;
        //private string description;
        //private bool isSearch;
        //private bool allKey;

        public bool isSearch { get; set; }
        public bool allKey { get; set; }
        public string description { get; set; }

        public string title { get; set; }

        // Parameterised Constructor 
        public IsSearch(bool issearch = true, bool allkey = false, string Title = "", string Description = "")
        {
            title = Title;
            description = Description;
            isSearch = issearch;
            allKey = allkey;
        }
        public InfoOfClass CreateriaClassAdd<T>(T oType, string pName, bool isKey = false, bool allKey = false) where T : Type
        {
            InfoOfClass oClass = new InfoOfClass();
            PropertyInfo info = oType.GetProperty(pName);
            oClass.Class = oType.Name;
            oClass.ForSearch = info.Name;
            oClass.Name = info.Name;
            oClass.isKey = isKey;
            oClass.Datatype = info.PropertyType.Name;
            oClass.allKey = allKey;
            return oClass;
        }

        
        public string CreateriaClasstoSQLQuery(List<InfoOfClass> olInfoOfClass)
        {
            string sQuery = "";
            //sQuery = CreateriaClasstoSQLQuery(CreateriaClass(T), oSetDataObject);
             List<InfoOfClass> query = olInfoOfClass.Where(a => a.isKey).ToList();
           //  olInfoOfClass = query;
            foreach (InfoOfClass value in query)
            {
                sQuery += " " + value.ForSearchValue + " &&";
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

        public string CreateriaClasstoSQLQuery(Type T, eSetDataObject oSetDataObject = eSetDataObject.None)
        {
            string sQuery = "";
            sQuery = CreateriaClasstoSQLQuery(CreateriaClass(T), oSetDataObject);
            return sQuery;
        }
            public string CreateriaClasstoSQLQuery(List<InfoOfClass> aList, eSetDataObject oSetDataObject = eSetDataObject.None)
        {
            string sQuery = "";

            if (oSetDataObject == eSetDataObject.Key)
            {
                List<InfoOfClass> query = aList.Where(a => a.isKey).ToList();
                aList = query;
            }

            foreach (InfoOfClass value in aList)
            {
                sQuery += " " + value.ForSearch + " == '{0}' ||";
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
            List<InfoOfClass> LCreateriaClass = new List<InfoOfClass>();
            //iGetKey oGetKey = new iGetKey();
            //XPClassInfo classInfo = session.Dictionary.GetClassInfo(oType);
            //if (oType.Name.ToLower() == "fbank".ToLower())
            //{
            //    LCreateriaClass.Add(oGetKey.CreateriaClassAdd(oType, "BankKey", true));
            //    //LCreateriaClass.Add(CreateriaClassAdd(oType, "BankName"));
            //    //LCreateriaClass.Add(CreateriaClassAdd(oType, "BankNumber"));
            //}
            PropertyInfo[] properties = oType.GetProperties();
            for (int i = 0; i < properties.GetLength(0); i++)
            {
                object[] attributesArray = properties[i].GetCustomAttributes(true);

                foreach (Attribute item in attributesArray)
                {
                    if (item is IsSearch )
                    {

                        //// Display the fields of the NewAttribute 
                        IsSearch attributeObject = (IsSearch)item;
                        //Console.WriteLine("{0} - {1}, {2} ", properties[i].Name,
                        //attributeObject.title, attributeObject.description);
                        if (attributeObject.isSearch)
                        {
                            LCreateriaClass.Add(CreateriaClassAdd(oType, properties[i].Name, true));
                        }
                    }
                }

            }

                return LCreateriaClass;
        }
        /*
            public static void AttributeDisplay(Type classType)
        {
            Console.WriteLine("Methods of class {0}", classType.Name);

            // Array to store all methods of a class 
            // to which the attribute may be applied 

            MethodInfo[] methods = classType.GetMethods();
            PropertyInfo[] properties = classType.GetProperties();

            // for loop to read through all methods 

            for (int i = 0; i < methods.GetLength(0); i++)
            {

                // Creating object array to receive  
                // method attributes returned 
                // by the GetCustomAttributes method 

                object[] attributesArray = methods[i].GetCustomAttributes(true);

                // foreach loop to read through  
                // all attributes of the method 
                foreach (Attribute item in attributesArray)
                {
                    if (item is NewAttribute)
                    {

                        // Display the fields of the NewAttribute 
                        oAttributeIsSearch attributeObject = (oAttributeIsSearch)item;
                        Console.WriteLine("{0} - {1}, {2} ", methods[i].Name,
                         attributeObject.title, attributeObject.description);
                    }
                }
                // Methods of class Employer
                //getId - Accessor, Gives value of Employer Id
                //getName - Accessor, Gives value of Employer Name
                /// Applying the custom attribute
                //    // NewAttribute to the getName method 
                //    [NewAttribute("Accessor", "Gives value of Employee Name")] 
                //    public string getName()
                //                {
                //                    return name;
                //                }
            }
        }
        */

       
    }

    public enum eSetDataObject
    {
        [XafDisplayName("None")]
        None = 0,
        [XafDisplayName("Key")]
        Key = 1,
        [XafDisplayName("Oid")]
        Oid = 2,
        [XafDisplayName("AllKey")]
        AllKey = 3
    }
    
    /*
    public class NewAttribute : Attribute
    {

        // Private fields 
        private string title;
        private string description;

        // Parameterised Constructor 
        public NewAttribute(string t, string d)
        {
            title = t;
            description = d;
        }

        // Method to show the Fields  
        // of the NewAttribute 
        // using reflection 
        public static void AttributeDisplay(Type classType)
        {
            Console.WriteLine("Methods of class {0}", classType.Name);

            // Array to store all methods of a class 
            // to which the attribute may be applied 

            MethodInfo[] methods = classType.GetMethods();

            // for loop to read through all methods 

            for (int i = 0; i < methods.GetLength(0); i++)
            {

                // Creating object array to receive  
                // method attributes returned 
                // by the GetCustomAttributes method 

                object[] attributesArray = methods[i].GetCustomAttributes(true);

                // foreach loop to read through  
                // all attributes of the method 
                foreach (Attribute item in attributesArray)
                {
                    if (item is NewAttribute)
                    {

                        // Display the fields of the NewAttribute 
                        NewAttribute attributeObject = (NewAttribute)item;
                        Console.WriteLine("{0} - {1}, {2} ", methods[i].Name,
                         attributeObject.title, attributeObject.description);
                    }
                }
            }
        }
    }
    */
}

