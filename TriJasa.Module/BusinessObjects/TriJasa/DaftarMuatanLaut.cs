// Class Name : DaftarMuatan.cs 
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
using DevExpress.ExpressApp.Model;

namespace TriJasa.Module.BusinessObjects
{ 
   [DefaultClassOptions] 
   [ImageName("ModelEditor_Views")] 
   [DefaultProperty("NomorDM")]
   [NavigationItem("Transaksi")]
   // Standard Document
   [System.ComponentModel.DisplayName("DaftarMuatanLaut")]
    [RuleCombinationOfPropertiesIsUnique("DaftarMuatanLautRule", DefaultContexts.Save, "NomorDM")]
    //[PropertyEditor(typeof(Double), EditorAliases.d, true)]
    public class DaftarMuatanLaut : DaftarMuatan
   {
     public DaftarMuatanLaut() : base()
     {
     // This constructor is used when an object is loaded from a persistent storage.
     // Do not place any code here.
     }
     public DaftarMuatanLaut(Session session) : base(session) 
     {
       // This constructor is used when an object is loaded from a persistent storage.
       // Do not place any code here.
     }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            NomorDM = Number(Tanggal);
        }
        public override string Number(DateTime sDate)
        {
            //TAHUN +BULAN + NOMOR URUT / NOMOR URUT SM / TTB
            //  DBL.2102.001 / 012 / TANDATERIMA

            string sNumer = "";
            int sRun = 1;
            byte iYear = (byte)((sDate.Year - 2014) + 65);
            string sYear = $"DBL.{sDate.ToString("yyMM")}";
            XPCollection<DaftarMuatanLaut> xpDM = new XPCollection<DaftarMuatanLaut>(Session);
            //string sNumberMax = (string)xpDM.Max(x => x.NomorDM)
            string sNumberMax = "";
            try
            {
                sNumberMax = xpDM
               //SelectMany(c => c.).
               .Where(o => o.NomorDM.Substring(0, 8).ToUpper().Trim() == sYear.ToUpper().Trim())
               .Max(o => o.NomorDM.Trim());
            }
            catch (Exception e)
            {
                sNumberMax = "";
            }
            if (sNumberMax != null)
            {
                if (sNumberMax.Length == 12)
                {
                    sNumberMax = sNumberMax.Substring(10, 2);
                    sRun = System.Convert.ToInt32(sNumberMax) + 1;
                }
            }

            sNumer = $"{sYear}.L{sRun.ToString("00")}";

            return sNumer;
        }
        protected override void OnSaving()
     {

            base.OnSaving();
     } 
     protected override void OnDeleting()
     {
       base.OnDeleting();
     }
        //protected override void OnLoaded()


    private Supplier _supplier;
    [XafDisplayName("Supplier"), ToolTip("Supplier")]
    [Size(50)]
    public new Supplier supplier
        {
        get { return  (Supplier) base.NamaSpr ; }
        set { base.NamaSpr =(Supir)value;  }
    }

        private Kapal _Kapal;
        [XafDisplayName("Kapal"), ToolTip("Kapal")]
        [Size(50)]
        public new Kapal Kapal
        {
            get { return (Kapal)base.NoTruck; }
            set { base.NoTruck = (Truck)value; }
        }

        // 
        private Tujuan _Tujuan; 
     [XafDisplayName("Tujuan"), ToolTip("Tujuan")] 
     [Size(50)] 
     public virtual Tujuan tujuan
        { 
       get { return _Tujuan; } 
       set { SetPropertyValue("tujuan", ref _Tujuan, value); } 
     } 
     
   
   } 
   
} 
