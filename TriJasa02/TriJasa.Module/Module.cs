using System;
using System.Text;
using System.Linq;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using System.Collections.Generic;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.DomainLogics;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.Xpo;

namespace TriJasa.Module {
    // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ModuleBase.
    public sealed partial class TriJasaModule : ModuleBase {
        public TriJasaModule() {
            InitializeComponent();
        }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB) {
            ModuleUpdater updater = new DatabaseUpdate.Updater(objectSpace, versionFromDB);
            return new ModuleUpdater[] { updater };
        }
        public override void Setup(XafApplication application) {
            base.Setup(application);
            // Manage various aspects of the application UI and behavior at the module level.
            //application.SetupComplete += Application_SetupComplete;
            application.ObjectSpaceCreated += Application_ObjectSpaceCreated;
        }
        public override void CustomizeTypesInfo(ITypesInfo typesInfo) {
            base.CustomizeTypesInfo(typesInfo);
            CalculatedPersistentAliasHelper.CustomizeTypesInfo(typesInfo);
        }
        private void Application_ObjectSpaceCreated(object sender, ObjectSpaceCreatedEventArgs e)
        {
            //var nonPersistentObjectSpace = e.ObjectSpace as NonPersistentObjectSpace;
            //if (nonPersistentObjectSpace != null)
            //{
            //    IObjectSpace additionalObjectSpace = Application.CreateObjectSpace(typeof(Person));
            //    nonPersistentObjectSpace.AdditionalObjectSpaces.Add(additionalObjectSpace);
            //    nonPersistentObjectSpace.AutoDisposeAdditionalObjectSpaces = true;
            //}
        }

    }
}
