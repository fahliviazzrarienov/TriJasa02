using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TriJasa.Module.BusinessObjects.TriJasa
{
    [DomainComponent]
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113594.aspx.
    public class BankTranRO : ReportParametersObjectBase, INotifyPropertyChanged
    {
        public BankTranRO(IObjectSpaceCreator provider) : base(provider) { }
        protected override IObjectSpace CreateObjectSpace()
        {
            return objectSpaceCreator.CreateObjectSpace(null);
        }
        private string sampleProperty;
        public string SampleProperty
        {
            get { return sampleProperty; }
            setr
            {
                if (sampleProperty != value)
                {
                    sampleProperty = value;
                    OnPropertyChanged();
                }
            }
        }
        public override CriteriaOperator GetCriteria()
        {
            CriteriaOperator criteria = null;
            // Specify the criteria used to filter the report's data source. Example:
            //CriteriaOperator criteria = new BinaryOperator(nameof(YourBusinessObject.YourProperty), SampleProperty);
            return criteria;
        }
        public override SortProperty[] GetSorting()
        {
            List<SortProperty> sorting = new List<SortProperty>();
            // Specify the options used to sort the report's data source. Example:
            //sorting.Add(new SortProperty(nameof(YourBusinessObject.YourProperty), SortingDirection.Descending));
            return sorting.ToArray();
        }
        #region INotifyPropertyChanged members (see http://msdn.microsoft.com/en-us/library/system.componentmodel.inotifypropertychanged(v=vs.110).aspx)
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}