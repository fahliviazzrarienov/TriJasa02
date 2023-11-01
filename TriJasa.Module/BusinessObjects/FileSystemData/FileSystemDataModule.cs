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
using System.IO;
using DevExpress.ExpressApp.Utils;
namespace TriJasa.Module.BusinessObjects
{
    public class FileSystemData
    {
        public FileSystemData()
        {
            //InitializeComponent();
            //BaseObject.OidInitializationMode = OidInitializationMode.AfterConstruction;
        }

        public  int ReadBytesSize = 0x1000;
        public  string FileSystemStoreLocation = String.Format("{0}FileData", PathHelper.GetApplicationFolder());
        public  void CopyFileToStream(string sourceFileName, Stream destination)
        {
            if (string.IsNullOrEmpty(sourceFileName) || destination == null) return;
            using (Stream source = File.OpenRead(sourceFileName))
                CopyStream(source, destination);
        }
        public  void OpenFileWithDefaultProgram(string sourceFileName)
        {
            Guard.ArgumentNotNullOrEmpty(sourceFileName, "sourceFileName");
            System.Diagnostics.Process.Start(sourceFileName);
        }
        public  void CopyStream(Stream source, Stream destination)
        {
            if (source == null || destination == null) return;
            byte[] buffer = new byte[ReadBytesSize];
            int read = 0;
            while ((read = source.Read(buffer, 0, buffer.Length)) > 0)
                destination.Write(buffer, 0, read);
        }
    }
}
