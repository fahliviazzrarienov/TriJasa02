using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// new files
namespace TriJasa.Module.BusinessObjects
{
     public class ENumXlsIndex
    {

        private int _IdxEnum;
        public int IdxEnum
        {
            get { return _IdxEnum; }
            set { _IdxEnum = value; }
        }
        private string  _Name;
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        private int _ColomXls;
        public int ColomXls
        {
            get { return _ColomXls; }
            set { _ColomXls = value; }
        }
        private bool _HasData;
        public bool HasData
        {
            get { return _HasData; }
            set { _HasData = value; }
        }

        private bool _Required;
        public bool Required
        {
            get { return _Required; }
            set { _Required = value; }
        }
    }
}
