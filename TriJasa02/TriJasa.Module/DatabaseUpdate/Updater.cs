using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using TriJasa.Module.BusinessObjects;
namespace TriJasa.Module.DatabaseUpdate {
    // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Updating.ModuleUpdater
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion) {
        }

        private  void ObjectImport(string sName, string sObject)
        {
            ImportObject oImportObject = ObjectSpace.FindObject<ImportObject>
                                        (new BinaryOperator("ObjectName", sName));
            if (oImportObject == null)
            {
                oImportObject = ObjectSpace.CreateObject<ImportObject>();
                oImportObject.ObjectName = sName;
                oImportObject.FullObjectName = sObject;
                oImportObject.Save();
            }
        }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            //string name = "MyName";
            //DomainObject1 theObject = ObjectSpace.FindObject<DomainObject1>(CriteriaOperator.Parse("Name=?", name));
            //if(theObject == null) {
            //    theObject = ObjectSpace.CreateObject<DomainObject1>();
            //    theObject.Name = name;
            //}

            iGenTriJasa oGen = new iGenTriJasa();

            oGen.ReportNameSet(ObjectSpace, "DMPPrintDM", "DM", "Screen DM unutk print DM");
            oGen.ReportNameSet(ObjectSpace, "DMPPrintSM", "SM", "Screen DM unutk print SM");
            oGen.ReportNameSet(ObjectSpace, "NotaPPrint01", "Nota", "Screen Nota Print Nota");
            oGen.ReportNameSet(ObjectSpace, "NotaPPrint02", "", "Screen Nota Print Nota 2");
            oGen.ReportNameSet(ObjectSpace, "NotaPPrint03", "", "Screen Nota Print Faktur");
            oGen.ReportNameSet(ObjectSpace, "NotaPPrint04", "Rekap Nota", "Rekap Status Nota");
            oGen.ReportNameSet(ObjectSpace, "PK01Print", "nota kontan", "tanda terima Penerimaan kas");
            oGen.ReportNameSet(ObjectSpace, "PKV1Print", "", "Tanda terima Penerimaan kas Rekap");
            oGen.ReportNameSet(ObjectSpace, "DTPPrint01", "", "Print Kolektor");
            oGen.ReportNameSet(ObjectSpace, "DTPPrint02", "", "Print Bali");
            oGen.ReportNameSet(ObjectSpace, "DTPPrint03", "", "Print Lombok");
            oGen.ReportNameSet(ObjectSpace, "DMPPrintDMRekap", "", "Rekap DM");
            oGen.ReportNameSet(ObjectSpace, "DMPPrintSelisih", "", "Rekap Selisih");
            oGen.ReportNameSet(ObjectSpace, "PKVLPrint", "", "Print Kas Kecil");
            oGen.ReportNameSet(ObjectSpace, "PKVLPrintRekap", "", "Print Rekap Penerimaan Kasir");
            oGen.ReportNameSet(ObjectSpace, "BKTansPrint", "", "Print Bank & Kas");
            oGen.ReportNameSet(ObjectSpace, "SKPPPrint01", "", "Print Supir Klaim");



            ObjectImport("Supir", "Supir");
            ObjectImport("Truck", "Truck");
            ObjectImport("Pelanggan", "Pelanggan");
            ObjectImport("PelangganTerima", "PelangganTerima");
            ObjectImport("Karyawan", "Karyawan");
            ObjectImport("Kolektor", "Kolektor");
            ObjectImport("DaftarMuatan", "DaftarMuatan");
            ObjectImport("SuratMuatan", "SuratMuatan");
            ObjectImport("SuratMuatDtl", "SuratMuatDtl");
            ObjectImport("Nota", "Nota");
            ObjectImport("NotaPayment", "NotaPayment");
            ObjectImport("NotaPaymentDtl", "NotaPaymentDtl");
            ObjectImport("fTransCode", "fTransCode");

            //  PermissionPolicyUser sampleUser = ObjectSpace.FindObject<PermissionPolicyUser>(new BinaryOperator("UserName", "User"));
              UserLogin sampleUser = ObjectSpace.FindObject<UserLogin>(new BinaryOperator("UserName", "User"));
            if (sampleUser == null) {
                sampleUser = ObjectSpace.CreateObject<UserLogin>();
                sampleUser.UserName = "User";
                sampleUser.Name = "User";
                sampleUser.SetPassword("");
            }
            PermissionPolicyRole defaultRole = CreateDefaultRole();
            sampleUser.Roles.Add(defaultRole);

            // PermissionPolicyUser userAdmin = ObjectSpace.FindObject<PermissionPolicyUser>(new BinaryOperator("UserName", "Admin"));
            UserLogin userAdmin = ObjectSpace.FindObject<UserLogin>(new BinaryOperator("UserName", "Admin"));
            if (userAdmin == null) {
                userAdmin = ObjectSpace.CreateObject<UserLogin>();
                userAdmin.Name = "Admin";
                userAdmin.UserName = "Admin";
                // Set a password if the standard authentication type is used
                userAdmin.SetPassword("");
            }
			// If a role with the Administrators name doesn't exist in the database, create this role
            PermissionPolicyRole adminRole = ObjectSpace.FindObject<PermissionPolicyRole>(new BinaryOperator("Name", "Administrators"));
            if(adminRole == null) {
                adminRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                adminRole.Name = "Administrators";
            }
            adminRole.IsAdministrative = true;
			userAdmin.Roles.Add(adminRole);
            ObjectSpace.CommitChanges(); //This line persists created object(s).
        }
        public override void UpdateDatabaseBeforeUpdateSchema() {
            base.UpdateDatabaseBeforeUpdateSchema();
            //if(CurrentDBVersion < new Version("1.1.0.0") && CurrentDBVersion > new Version("0.0.0.0")) {
            //    RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName");
            //}
        }
        private PermissionPolicyRole CreateDefaultRole() {
            PermissionPolicyRole defaultRole = ObjectSpace.FindObject<PermissionPolicyRole>(new BinaryOperator("Name", "Default"));
            if(defaultRole == null) {
                defaultRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                defaultRole.Name = "Default";

                //defaultRole.AddObjectPermission<PermissionPolicyUser>(SecurityOperations.Read, "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                defaultRole.AddObjectPermission<UserLogin>(SecurityOperations.Read, "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                defaultRole.AddNavigationPermission(@"Application/NavigationItems/Items/Default/Items/MyDetails", SecurityPermissionState.Allow);
                //defaultRole.AddMemberPermission<PermissionPolicyUser>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                defaultRole.AddMemberPermission<UserLogin>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                //defaultRole.AddMemberPermission<PermissionPolicyUser>(SecurityOperations.Write, "StoredPassword", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                defaultRole.AddMemberPermission<UserLogin>(SecurityOperations.Write, "StoredPassword", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.Read, SecurityPermissionState.Deny);
                defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
				defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create, SecurityPermissionState.Allow);
            }
            return defaultRole;
        }
    }
}
