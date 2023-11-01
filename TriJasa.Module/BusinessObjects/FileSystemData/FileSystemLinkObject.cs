using System;
using System.IO;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Utils;

namespace TriJasa.Module.BusinessObjects
{
    /// <summary>
    /// This class enables you to add soft links to real files instead of saving their contents to the database. It is intended for use in Windows Forms applications only.
    /// </summary>
    [DefaultProperty("FileName")]
    [DeferredDeletion(false), OptimisticLocking(false)]

    public class FileSystemLinkObject : BaseObject, IFileData, IEmptyCheckable, ISupportFullName {
        public static int ReadBytesSize = 0x1000;
        public static string FileSystemStoreLocation = String.Format("{0}FileData", PathHelper.GetApplicationFolder());
        public FileSystemData FileSystemDataModule = new FileSystemData();
        public FileSystemLinkObject(Session session) : base(session) { }
        #region IFileData Members
        [Size(260), Custom("AllowEdit", "False")]
        public string FileName {
            get { return GetPropertyValue<string>("FileName"); }
            set { SetPropertyValue("FileName", value); }
        }
        void IFileData.Clear() {
            Size = 0;
            FileName = string.Empty;
        }
        //Dennis: Fires when uploading a file.
        void IFileData.LoadFromStream(string fileName, Stream source) {
            Size = (int)source.Length;
            FileName = fileName;
        }
        //Dennis: Fires when saving or opening a file.
        void IFileData.SaveToStream(Stream destination) {
            try {
                if (destination == null)
                    FileSystemDataModule.OpenFileWithDefaultProgram(FullName);
                   // OpenFileWithDefaultProgram(FullName);
                else
                    FileSystemDataModule.CopyFileToStream(FullName, destination);
                    //CopyFileToStream(FullName, destination);
            } catch (Exception exc) {
                throw new UserFriendlyException(exc);
            }
        }
        //public static void CopyFileToStream(string sourceFileName, Stream destination)
        //{
        //    if (string.IsNullOrEmpty(sourceFileName) || destination == null) return;
        //    using (Stream source = File.OpenRead(sourceFileName))
        //        CopyStream(source, destination);
        //}
        //public static void OpenFileWithDefaultProgram(string sourceFileName)
        //{
        //    Guard.ArgumentNotNullOrEmpty(sourceFileName, "sourceFileName");
        //    System.Diagnostics.Process.Start(sourceFileName);
        //}
        //public static void CopyStream(Stream source, Stream destination)
        //{
        //    if (source == null || destination == null) return;
        //    byte[] buffer = new byte[ReadBytesSize];
        //    int read = 0;
        //    while ((read = source.Read(buffer, 0, buffer.Length)) > 0)
        //        destination.Write(buffer, 0, read);
        //}

        [Persistent]
        public int Size {
            get { return GetPropertyValue<int>("Size"); }
            private set { SetPropertyValue<int>("Size", value); }
        }
        #endregion
        #region IEmptyCheckable Members
        public bool IsEmpty {
            get { return !File.Exists(FullName); }
        }
        #endregion
        #region ISupportFullName Members
        [Size(260), Custom("AllowEdit", "False")]
        public string FullName {
            get { return GetPropertyValue<string>("FullName"); }
            set { SetPropertyValue<string>("FullName", value); }
        }
        #endregion
    }
}