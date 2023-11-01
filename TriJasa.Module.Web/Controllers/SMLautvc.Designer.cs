
namespace TriJasa.Module.Web.Controllers
{
    partial class SMvcLaut
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AddAndNew = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // AddAndNew
            // 
            this.AddAndNew.Caption = "Add And New";
            this.AddAndNew.Category = "PopupActions";
            this.AddAndNew.ConfirmationMessage = null;
            this.AddAndNew.Id = "AddAndNewLaut";
            this.AddAndNew.ToolTip = null;
            this.AddAndNew.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AddAndNew_Execute);
            // 
            // SMvc
            // 
            this.Actions.Add(this.AddAndNew);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction AddAndNew;
    }
}
