// Class Name : fVechType.cs 
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using System.Linq;

namespace TriJasa.Module.BusinessObjects
{
    [DefaultClassOptions]
    [ImageName("ModelEditor_Views")]
    [DefaultProperty("Code")]
    [NavigationItem("Master")]
    // Standard Document
    [System.ComponentModel.DisplayName("Perusahaan")]
    public class fCompany : XPObject
    {
        public fCompany() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }
        public fCompany(Session session) : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            string tUser = SecuritySystem.CurrentUserName.ToString();
            TelegramBot = "";
        }
        protected override void OnSaving()
        {
            base.OnSaving();
        }
        protected override void OnDeleting()
        {
            base.OnDeleting();
        }
        //
        private string _Code;
        [XafDisplayName("Kode"), ToolTip("Kode")]
        [Size(10)]
        public virtual string Code
        {
            get { return _Code; }
            set { SetPropertyValue("Code", ref _Code, value); }
        }
        //
        private string _Name;
        [XafDisplayName("Nama"), ToolTip("Nama")]
        [Size(150)]
        public virtual string Name
        {
            get { return _Name; }
            set { SetPropertyValue("Name", ref _Name, value); }
        }
        private string _Address;
        [XafDisplayName("Address"), ToolTip("Address")]
        [Size(150)]
        public virtual string Address
        {
            get { return _Address; }
            set { SetPropertyValue("Address", ref _Address, value); }
        }
        private string _City;
        [XafDisplayName("Kota"), ToolTip("Kota")]
        [Size(150)]
        public virtual string City
        {
            get { return _City; }
            set { SetPropertyValue("City", ref _City, value); }
        }

        private string _TelegramBot;
        [XafDisplayName("TelegramBot"), ToolTip("TelegramBot")]
        [Size(150)]
        public virtual string TelegramBot
        {
            get { return _TelegramBot; }
            set { SetPropertyValue("TelegramBot", ref _TelegramBot, value); }
        }

        private MediaDataObject _Logo;

        public MediaDataObject Logo
        {
            get { return _Logo; }
            set { SetPropertyValue("Logo", ref _Logo, value); }
        }
        private double _Komisi;
        [XafDisplayName("Komisi"), ToolTip("Komisi")]
        [Size(150)]
        public virtual double Komisi
        {
            get { return _Komisi; }
            set { SetPropertyValue("Komisi", ref _Komisi, value); }
        }

        private fTransCode _AccountPiutang;
        [XafDisplayName("Account Piutang Dagang"), ToolTip("Account Piutang Dagang")]
        //[Size(150)]
        public virtual fTransCode AccountPiutang
        {
            get { return _AccountPiutang; }
            set { SetPropertyValue("AccountPiutang", ref _AccountPiutang, value); }
        }

        private fTransCode _AccountDP;
        [XafDisplayName("Account Penerimaan DP"), ToolTip("Account Penerimaan DP")]
        //[Size(150)]
        public virtual fTransCode AccountDP
        {
            get { return _AccountDP; }
            set { SetPropertyValue("AccountDP", ref _AccountDP, value); }
        }

        private Bank _BankPenerimaanPiutang;
        [XafDisplayName("Bank Piutang Dagang"), ToolTip("Bank Piutang Dagang")]
        //[Size(150)]
        public virtual Bank BankPenerimaanPiutang
        {
            get { return _BankPenerimaanPiutang; }
            set { SetPropertyValue("BankPenerimaanPiutang", ref _BankPenerimaanPiutang, value); }
        }

        private Bank _BankPenerimaanDP;
        [XafDisplayName("Kas Penerimaan DP"), ToolTip("Kas Penerimaan DP")]
        //[Size(150)]
        public virtual Bank BankPenerimaanDP
        {
            get { return _BankPenerimaanDP; }
            set { SetPropertyValue("BankPenerimaanDP", ref _BankPenerimaanDP, value); }
        }
        private fTransCode _AccountKomisi;
        [XafDisplayName("Account Komisi"), ToolTip("Account Komisi")]
        //[Size(150)]
        public virtual fTransCode AccountKomisi
        {
            get { return _AccountKomisi; }
            set { SetPropertyValue("AccountKomisi", ref _AccountKomisi, value); }
        }

        private fTransCode _PenerimaanTunai;
        [XafDisplayName("Penerimaan Tunai"), ToolTip("Penerimaan Tunai")]
        //[Size(150)]
        public virtual fTransCode PenerimaanTunai
        {
            get { return _PenerimaanTunai; }
            set { SetPropertyValue("PenerimaanTunai", ref _PenerimaanTunai, value); }
        }

        private fTransCode _BankTransfer;
        [XafDisplayName("Bank Transfer"), ToolTip("Bank Transfer")]
        //[Size(150)]
        public virtual fTransCode BankTransfer
        {
            get { return _BankTransfer; }
            set { SetPropertyValue("BankTransfer", ref _BankTransfer, value); }
        }

        //[XafDisplayName("Gate"), ToolTip("Gate")]
        //[Association("CompanyGate")]
        //public XPCollection<fGates> Gates
        //{
        //    get
        //    {
        //        return GetCollection<fGates>("Gates");
        //    }
        //}

    }

}