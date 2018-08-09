using Microsoft.Practices.Prism.Mvvm;

namespace ViewSwitchingNavigation.Configuration.Model
{
    public class ClassSubConfigurationTopModel : BindableBase//, IDataErrorInfo
    {
        private string sclass;
        private int? subId;
        private string subject;
        private int? mm;
        private int? term;
        private int? mmd;
        private int? mm1;
        private int? mm2;
        private int? mm3;
        private bool isEnableSaveButton=true;
        private bool isEnableEditButton=true;
        private bool isReadOnly;

        public string Sclass
        {
            get { return sclass; }
            set { SetProperty(ref this.sclass, value); }
        }
        public int? SubId
        {
            get { return subId; }
            set { SetProperty(ref this.subId, value); }
        }
        public string Subject
        {
            get { return subject; }
            set { SetProperty(ref this.subject, value); }
        }
        public int? Mm
        {
            get { return mm; }
            set { SetProperty(ref this.mm, value); }
        }
        public int? Term
        {
            get { return term; }
            set { SetProperty(ref this.term, value); }
        }
        public int? Mmd
        {
            get { return mmd; }
            set { SetProperty(ref this.mmd, value); }
        }
        public int? Mm1
        {
            get { return mm1; }
            set { SetProperty(ref this.mm1, value); }
        }
        public int? Mm2
        {
            get { return mm2; }
            set { SetProperty(ref this.mm2, value); }
        }
        public int? Mm3
        {
            get { return mm3; }
            set { SetProperty(ref this.mm3, value); }
        }
        public bool IsEnableSaveButton
        {
            get { return isEnableSaveButton; }
            set { SetProperty(ref this.isEnableSaveButton, value); }
        }
        public bool IsEnableEditButton
        {
            get { return isEnableEditButton; }
            set { SetProperty(ref this.isEnableEditButton, value); }
        }
        public bool IsReadOnly
        {
            get { return isReadOnly; }
            set { SetProperty(ref this.isReadOnly, value); }
        }
        public void Reset()
        {
            Sclass = "";
            SubId = null;
            Subject = "";
            Mm = 100;
            Term = 2;
            Mmd = 2;
            Mm1 = 30;
            Mm2 = 70;
            Mm3 = 0;
            IsEnableSaveButton = true;
            IsEnableEditButton = true;
        }
        //public event PropertyChangedEventHandler PropertyChanged;
        //protected void OnPropertyChanged(string propertyName)
        //{
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //    }

        //}
        //public string this[string columnName]
        //{
        //    get
        //    {
        //        if (columnName.In("Sclass", "SubId", "Term", "Mm", "Mm1", "Mm2") && CheckCondition())
        //        {
        //                IsEnableSaveButton = true;
        //                return "";
        //        }
        //        else
        //        {
        //            IsEnableSaveButton = false;
        //            return "";
        //        }
                        
        //        //switch (columnName)
        //        //{
        //        //    case "Sclass":
        //        //        if (CheckCondition())
        //        //        {
        //        //            IsEnableSaveButton = false;
        //        //            return "";
        //        //        }
        //        //        break;
        //        //    case "SubId":
        //        //        if (CheckCondition())
        //        //        {
        //        //            IsEnableSaveButton = false;
        //        //            return "";
        //        //        }
        //        //        break;
        //        //    case "Term":
        //        //        if (CheckCondition())
        //        //        {
        //        //            IsEnableSaveButton = false;
        //        //            return "";
        //        //        }
        //        //        break;
        //        //    case "Mmd":
        //        //        if (CheckCondition())
        //        //        {
        //        //            IsEnableSaveButton = false;
        //        //            return "";
        //        //        }
        //        //        break;
        //        //    case "Mm1":
        //        //        if (CheckCondition())
        //        //        {
        //        //            IsEnableSaveButton = false;
        //        //            return "";
        //        //        }
        //        //        break;
        //        //    case "Mm2":
        //        //        if (CheckCondition())
        //        //        {
        //        //            IsEnableSaveButton = false;
        //        //            return "";
        //        //        }
        //        //        break;
        //        //}

        //        //IsEnableSaveButton = true;
        //        //return "";
        //    }
        //}
        //public bool CheckCondition()
        //{
        //    if (string.IsNullOrWhiteSpace(this.Sclass) || this.SubId == null || this.Term == null || this.Mmd < 0 || this.Mm1 == null || this.Mm2 == null)
        //        return false;
        //    else
        //        return true;
            

        //}

        //public string Error
        //{
        //    get { throw new NotImplementedException(); }
        //}
    }
}
