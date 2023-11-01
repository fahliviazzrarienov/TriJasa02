// Class Name : fTransKasDtl.cs 
using System; 
using DevExpress.Xpo; 
using DevExpress.Persistent.Base; 
using System.ComponentModel; 
using DevExpress.Persistent.Validation; 
using DevExpress.ExpressApp.DC; 
using DevExpress.ExpressApp.ConditionalAppearance; 
using DevExpress.ExpressApp; 
using DevExpress.Data.Filtering; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks;
using DevExpress.ExpressApp.Editors;

namespace TriJasa.Module.BusinessObjects
{
   [DefaultClassOptions] 
   [ImageName("ModelEditor_Views")] 
   [DefaultProperty("TransKas")]
   [NavigationItem("Inquiry")]
   // Standard Document
   [System.ComponentModel.DisplayName("Transaksi Kas Detail")]
   public class fTransKasDtl : XPObject
   {
     public fTransKasDtl() : base()
     {
     // This constructor is used when an object is loaded from a persistent storage.
     // Do not place any code here.
     }
     public fTransKasDtl(Session session) : base(session) 
     {
       // This constructor is used when an object is loaded from a persistent storage.
       // Do not place any code here.
     }
     public override void AfterConstruction()
     {
       base.AfterConstruction();
       // Place here your initialization code.
       //SecuritySystem.CurrentUserName
       //LastUpdateUser = Session.GetObjectByKey<GPUser>(SecuritySystem.CurrentUserId);
       string tUser = SecuritySystem.CurrentUserName.ToString();
            Debit = 0;
            Credit = 0;
            //LastUpdateUser = Session.FindObject<GPUser>(new BinaryOperator("UserName", SecuritySystem.CurrentUserName.ToString())); 
            // LastUpdateUser = Session.FindObject<GPUser>(new BinaryOperator("UserName", tUser)); 
            // LastUpdate = DateTime.Now; 
            if (Oid <= 0)
            {
                Oid = ((DateTime.Now.Hour * 60 + DateTime.Now.Minute) * 60 + DateTime.Now.Second) * 100 + DateTime.Now.Millisecond;
            }
        } 
     protected override void OnSaving()
     {
        //TransKas.UpdateDetailH(true);
        base.OnSaving();
          
     } 
     protected override void OnDeleting()
     {
       base.OnDeleting();
     } 
     // 
     private fTransKas _TransKas; 
     [XafDisplayName("TransKas"), ToolTip("TransKas")] 
     [Association("TranCash")] 
     public virtual fTransKas TransKas
     { 
       get { return _TransKas; } 
       set {  SetPropertyValue("TransKas", ref _TransKas, value);
                //fTransKas oldTransKas = _TransKas;
                //bool modified = SetPropertyValue(nameof(TransKas), ref _TransKas, value);
                //if (!IsLoading && !IsSaving && oldTransKas != _TransKas && modified)
                //{
                //    oldTransKas = oldTransKas ?? _TransKas;
                //    oldTransKas.UpdateDetail(true);
 
                //}
            }

        } 
     // 
     private fTransCode _TransCode; 
     [XafDisplayName("TransKode"), ToolTip("TransKode")]
     [RuleRequiredField("txnDetailTransCode", DefaultContexts.Save, "TransKode harus di isi")]
     public virtual fTransCode TransCode
     { 
       get { return _TransCode; } 
       set { SetPropertyValue("TransCode", ref _TransCode, value); } 
     } 
     // 
     private string _Decription; 
     [XafDisplayName("Keterangan"), ToolTip("Keterangan")]
        [RuleRequiredField("txnDetailDescription", DefaultContexts.Save, "Keterangan harus di isi")]
        [Size(100)] 
     public virtual string Description
     { 
       get { return _Decription; } 
       set { SetPropertyValue("Decription", ref _Decription, value); } 
     } 
     // 
     private eDC _DC; 
     [XafDisplayName("D/C"), ToolTip("D/C")]
     [Appearance("kdDC", Visibility = ViewItemVisibility.Hide)]
        public virtual eDC DC
     { 
       get {
                try
                {
                    if (Debit > 0)
                    {
                        _DC = eDC.D;
                    }
                    else
                    {
                        _DC = eDC.C;
                    }
                } 
                catch ( Exception e)
                {
                    _DC = eDC.C;
                }
                return _DC; 
            } 
       //set { SetPropertyValue("DC", ref _DC, value); } 
     }

    // 
    private int _Debit;
    [XafDisplayName("Debit"), ToolTip("Debit")]
    //[Appearance("txdDebitEnable", Enabled = false, 
    //Visibility = ViewItemVisibility.Show,Criteria = " Credit>0 " )]
    [ImmediatePostData]
    public virtual int Debit
        {
        get { return _Debit; }
        set { SetPropertyValue("Debit", ref _Debit, value);
                //if (!IsLoading && !IsSaving && TransKas !=null)
                //{
                //    try
                //    {
                //        TransKas.UpdateDetailH(true);
                //    }
                //    catch{ }
                //}
            }
    }
    private int _Credit;
    [XafDisplayName("Credit"), ToolTip("Credit")]
    //[Appearance("txdCreditEnable", Enabled = false,
    // Visibility = ViewItemVisibility.Show, Criteria = " Debit>0 ")]
    [ImmediatePostData]
    public virtual int Credit
    {
        get { return _Credit; }
        set { SetPropertyValue("Credit", ref _Credit, value);
                //if (!IsLoading && !IsSaving && TransKas != null)
                //{
                //    try
                //    {
                //        TransKas.UpdateDetailH(true);
                //    }
                //    catch { }
                //}
            }
    }
    private int _Amount;
    [XafDisplayName("Amount"), ToolTip("Amount")]
        [PersistentAlias("Debit + (Credit*-1)")]
        public virtual int Amount
    {
        get {
                object tempObject = EvaluateAlias(nameof(Amount));
                if (tempObject != null)
                {
                    return (int)tempObject;
                }
                else
                {
                    return 0;

                }
                //_Amount = Debit + (Credit*-1);
                //return _Amount;
            }
        //set { SetPropertyValue("Amount", ref _Amount, value); }
    }

        // 
        //private int _Amount; 
        //[XafDisplayName("Amount"), ToolTip("Amount")]
        //[Appearance("kdAmount", Visibility = ViewItemVisibility.Hide)]
        //   //[ImmediatePostData]
        //   public virtual int Amount
        //{ 
        //  get { 
        //           return Credit+ Debit; } 
        //  //set { //SetPropertyValue("Amount", ref _Amount, value);
        //  //         bool modified = SetPropertyValue("Amount", ref _Amount, value); 
        //  //         if (!IsLoading && !IsSaving && TransKas != null && modified)
        //  //         {
        //  //             // TransKas.UpdateDetail(true);
        //  //             TransKas.UpdateDetailH(true);
        //  //             TransKas.Reload();
        //  //             //Product.UpdateMaximumOrder(true);
        //  //         }
        //  //     } 
        //} 
    }
    public enum eDC
    { 
     [XafDisplayName("Debit" )] 
     D = 0 , 
     [XafDisplayName("Credit" )]
     C = 1 
    } 
} 
