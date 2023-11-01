using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp.Web.Templates.ActionContainers;

public partial class Default : BaseXafPage {
    protected override ContextActionsMenu CreateContextActionsMenu() {
        return new ContextActionsMenu(this, "Edit", "RecordEdit", "ObjectsCreation", "ListView", "Reports");
    }
    protected void Page_Init()
    {
        CustomizeTemplateContent += (s, e) => {
            IHeaderImageControlContainer content = TemplateContent as IHeaderImageControlContainer;
            if (content == null) return;
            content.HeaderImageControl.DefaultThemeImageLocation = "Images";
            content.HeaderImageControl.ImageName = "trijasa2.svg";
            //content.HeaderImageControl.Width = Unit.Pixel(150);
            //content.HeaderImageControl.Height = Unit.Pixel(40);
        };
    }
    public override Control InnerContentPlaceHolder {
        get {
            return Content;
        }
    }
}