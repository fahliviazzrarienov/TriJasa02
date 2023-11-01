using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace TriJasa.Module.Web
{
    [PropertyEditor(typeof(Double), false)]
    public class CustomDoubleEditor : ASPxDoublePropertyEditor
    {
        public CustomDoubleEditor(Type objectType, IModelMemberViewItem info) :
            base(objectType, info)
        { }
        protected override WebControl CreateEditModeControlCore()
        {
            ASPxSpinEdit spinEdit = (ASPxSpinEdit)base.CreateEditModeControlCore();
            spinEdit.SpinButtons.ShowIncrementButtons = false;
            spinEdit.HorizontalAlign = HorizontalAlign.Right;
            return spinEdit;
        }
    }
}
