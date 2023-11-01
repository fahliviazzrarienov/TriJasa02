using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
// new files
namespace TriJasa.Module.BusinessObjects
{
    [DomainComponent]
    [FileAttachmentAttribute("InputFile")]
    public class FileInputParameter
    {
        public FileInputParameter()
        {
            this.InputFile = new NPFileData(); // required in versions prior to 18.1.7  
        }
        public NPFileData InputFile { get; set; }
    }

    [DomainComponent]
    public class NPFileData : IFileData
    {
        public NPFileData() { }
        public string FileName { get; set; }
        [Browsable(false)]
        public byte[] Content { get; set; }
        public int Size
        {
            get { return Content == null ? 0 : Content.Length; }
        }
        public void LoadFromStream(string fileName, System.IO.Stream stream)
        {
            Guard.ArgumentNotNull(stream, "stream");
            Guard.ArgumentNotNullOrEmpty(fileName, "fileName");
            this.FileName = fileName;
            byte[] array = new byte[stream.Length];
            stream.Read(array, 0, array.Length);
            this.Content = array;
        }
        public void SaveToStream(System.IO.Stream stream)
        {
            if (string.IsNullOrEmpty(this.FileName))
            {
                throw new InvalidOperationException();
            }
            stream.Write(this.Content, 0, this.Size);
            stream.Flush();
        }
        public void Clear()
        {
            this.Content = null;
            this.FileName = string.Empty;
        }
        public override string ToString()
        {
            return this.FileName;
        }
    }

    public class UploadFile : FileData, ISupportFullName
    {
        public UploadFile(Session session) : base(session)
        {
        }

        [Custom("AllowEdit", "False")]
        public string FullName
        {
            get; set;
        }
    }
    [DomainComponent]
    public class FileDataDetails
    {

        [NonPersistent]
        public UploadFile Files
        {
            get; set;
        }
    }
}
