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
using TriJasa.Module.BusinessObjects;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.Persistent.BaseImpl;
// new files
namespace TriJasa.Module.BusinessObjects
{
   public class iGenTriJasa : iGetKey
    {
        public iGenTriJasa()
        {

        }



        public string TagihNomorGet(Session oSession, DateTime oDate)
        {
            string sNumer = "";
            int sRun = 1;
            string sTahunBulan = oDate.ToString("yyMM");
            string sYear = $"CBL{sTahunBulan}.";
            //byte iYear = (byte)((oDate.Year - 2014) + 65);
            //string cYear = Encoding.ASCII.GetString(new byte[] { iYear }).ToString().Trim();
            string sqlQuery = string.Format(" StartsWith(Upper(Nomor),'{0}') ", sYear.Replace("'", "''"));
            CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);

            XPCollection<DaftarTagihan> xpDM = new XPCollection<DaftarTagihan>(oSession, filterOperator);
            //string sNumberMax = (string)xpDM.Max(x => x.NomorDM)
            string sNumberMax = "";
            try
            {
                sNumberMax = xpDM.
               //SelectMany(c => c.).
               //Where(o => o.NomorDM.Substring(0, 5).ToUpper().Trim() == sYear.ToUpper().Trim()).
               Max(o => o.Nomor.Trim());
            }
            catch (Exception e)
            {

            }

            if (sNumberMax != null)
            {
                if (sNumberMax.Length == 12)
                {
                    sNumberMax = sNumberMax.Substring(8, 4);
                    sRun = System.Convert.ToInt32(sNumberMax) + 1;
                }
            }

            
            sNumer = $"{sYear}{sRun.ToString("0000")}";

            return sNumer;

        }

        public void ReportNameSet(IObjectSpace oSession, string ReportID,string ReportName, string Description)
        {
            string sqlQuery = string.Format($" Code == '{ReportID.Trim()}' ");
            CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);
            iReport oiReport = oSession.FindObject<iReport>(filterOperator);

            if (oiReport == null)
            {
                oiReport = oSession.CreateObject<iReport>();
                oiReport.Code = ReportID;
                oiReport.ReportName = ReportName;
                oiReport.Description = Description;
                oiReport.Save();
                oiReport.Session.CommitTransaction();
            }
        }

        public string ReportNameGet(IObjectSpace oSession,string ReportID)
        {
            string ReportName = "";

            string sqlQuery = string.Format($" Code == '{ReportID.Trim()}' " );
            CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);
            iReport oiReport = oSession.FindObject<iReport>(filterOperator);
            if (oiReport !=null)
            {
                ReportName = oiReport.ReportName;
            }

            return ReportName;
        }
        public string NotaNomorGet(Session oSession, DateTime oDate)
        {
            string sNumer = "";
            int sRun = 1;
            string sTahunBulan = oDate.ToString("yyMM");
            string sYear = $"BL{sTahunBulan}";
            //byte iYear = (byte)((oDate.Year - 2014) + 65);
            //string cYear = Encoding.ASCII.GetString(new byte[] { iYear }).ToString().Trim();
            string sqlQuery = string.Format(" StartsWith(Upper(NoNota),'{0}') ", sYear.Replace("'", "''"));
            CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);
           
            XPCollection<Nota> xpDM = new XPCollection<Nota>(oSession, filterOperator);
            //string sNumberMax = (string)xpDM.Max(x => x.NomorDM)
            string sNumberMax = "";
            if (xpDM.Count > 0)
            {
                try
                {
                    sNumberMax = xpDM.
                   //SelectMany(c => c.).
                   Where(o => o.NoNota.Substring(0, 6).ToUpper().Trim() == sYear.ToUpper().Trim()).
                   Max(o => o.NoNota.Trim());
                }
                catch (Exception e)
                {

                }
                if (sNumberMax != null)
                {
                    if (sNumberMax.Length == 10)
                    {
                        sNumberMax = sNumberMax.Substring(6, 4);
                        sRun = System.Convert.ToInt32(sNumberMax) + 1;
                    }
                }
            }
            sNumer = $"{sYear}{sRun.ToString("0000")}";

            return sNumer;

        }
        public string DNNomorGet(Session oSession, DateTime oDate)
        {
            string sNumer = "";
            int sRun = 1;
            byte iYear = (byte)((oDate.Year - 2014) + 65);
            string cYear = Encoding.ASCII.GetString(new byte[] { iYear }).ToString().Trim();
            string sYear = $"DBL.{cYear}";
            XPCollection<DaftarMuatan> xpDM = new XPCollection<DaftarMuatan>(oSession);
            //string sNumberMax = (string)xpDM.Max(x => x.NomorDM)
            string sNumberMax = "";
            try
            {
                sNumberMax = xpDM.
               //SelectMany(c => c.).
               //Where(o => o.NomorDM.Substring(0, 5).ToUpper().Trim() == sYear.ToUpper().Trim()).
               Max(o => o.NomorDM.Trim());
            }
            catch (Exception e)
            {

            }
            if (sNumberMax != null)
            {
                if (sNumberMax.Length == 9)
                {
                    sNumberMax = sNumberMax.Substring(5, 4);
                    sRun = System.Convert.ToInt32(sNumberMax) + 1;
                }
            }

            sNumer = $"DBL.{cYear}{sRun.ToString("0000")}";

            return sNumer;
        }

        public void SupirKlaimSet(Session oSession, eSupirKlaim eKlaim, Supir oSupir, DaftarMuatan oDM , double Amount ,string oKeterangan,DateTime odate)
        {
            CriteriaOperator oCriteria = null;
            //oCriteria = GroupOperator.And(oCriteria, new BetweenOperator(Supir, new OperandProperty("StartDate"), new OperandProperty("EndDate")));
            oCriteria = GroupOperator.And(oCriteria, new BinaryOperator("Supir", oSupir));
            oCriteria = GroupOperator.And(oCriteria, new BinaryOperator("Jenis", eKlaim));
            oCriteria = GroupOperator.And(oCriteria, new BinaryOperator("Keterangan", oKeterangan.Trim()));
            oCriteria = GroupOperator.And(oCriteria, new BinaryOperator("DM", oDM));
            SupirKlaim oSupirKlaim = oSession.FindObject<SupirKlaim>(oCriteria);

            string sqlQuery = string.Format(" Oid == {0} ", oSupir.Oid);
            CriteriaOperator filterOperator = CriteriaOperator.Parse(sqlQuery);
            Supir onSupir = oSession.FindObject<Supir>(filterOperator);

             sqlQuery = string.Format(" Oid == {0} ", oDM.Oid);
             filterOperator = CriteriaOperator.Parse(sqlQuery);
            DaftarMuatan onDM = oSession.FindObject<DaftarMuatan>(filterOperator);


            if (oSupirKlaim==null)
            {
                oSupirKlaim = new SupirKlaim(oSession);
                oSupirKlaim.Supir = onSupir;
                oSupirKlaim.DM = onDM;
                oSupirKlaim.Tanggal = odate;
                oSupirKlaim.Jenis = eKlaim;
                //onSupir.Klaim.Add(oSupirKlaim);


            }
            oSupirKlaim.Jumlah = Amount;
            oSupirKlaim.Keterangan = oKeterangan;
            oSupirKlaim.Save();
            oSupirKlaim.Session.CommitTransaction();
        }
        //public nProductPrice PriceGet(Session oSession, DateTime oDate,nProduct oProduct)
        //{
        //    CriteriaOperator oCriteria = null;
        //    oCriteria = GroupOperator.And(oCriteria, new BetweenOperator(oDate, new OperandProperty("StartDate"), new OperandProperty("EndDate")));
        //    oCriteria = GroupOperator.And(oCriteria, new BinaryOperator("Product", oProduct));
        //    nProductPrice oProductPrice = oSession.FindObject<nProductPrice>(oCriteria);
        //    return oProductPrice;

        //}

        //public Object PeriodGet(Session oSession,DateTime oDate )
        //{
        //    CriteriaOperator oCriteria = null;
        //    //string sqlQuery = $"( #{oDate}#  <= EndDate  &&   #{oDate}# <= StartDate ";
        //    //filterOperator = CriteriaOperator.Parse(sqlQuery);

        //    //filterOperator=
        //    oCriteria = GroupOperator.And(oCriteria,  new BetweenOperator(oDate, new OperandProperty("StartDate") , new OperandProperty("EndDate") ));

        //    nPeriod oPeriod = oSession.FindObject<nPeriod>(oCriteria);
        //    if (oPeriod==null)
        //    {
        //        oPeriod = new nPeriod(oSession);
        //        oPeriod.StartDate = new DateTime(oDate.Year, oDate.Month, 1);
        //        oPeriod.EndDate = oPeriod.StartDate.AddMonths(1).AddDays(-1);
        //        oPeriod.Year = oDate.Year.ToString();
        //        oPeriod.Month = oDate.ToString("MM");
        //        oPeriod.Initial = oDate.ToString("MMM yyyy");
        //        oPeriod.Description= oDate.ToString("MMMM yyyy");
        //        oPeriod.Status = ePeriod.Closed;
        //        oSession.Save(oPeriod);
        //        oSession.CommitTransaction();
        //    }

        //    return oPeriod;
        //}
    }
}
