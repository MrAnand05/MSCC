using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace ViewSwitchingNavigation
{
    public partial class LogOn : Window, INotifyPropertyChanged {

        #region Private Fields

        private int _attempts;
        public IEnumerable<int> FinancialYearList { get; private set; }
        #endregion

        #region Public Properties

        public int Attempts {
            get { return _attempts; }
            set {
                if (value != _attempts) {
                    _attempts = value;
                    OnPropertyChanged("Attempts");
                }
            }
        }

        public Visibility ShowInvalidCredentials {
            get { 
                if (_attempts > 0) {
                    return Visibility.Visible;
                }
                return Visibility.Hidden;
            }
        }

        public string UserName {
            get { return txtUsername.Text; }
        }

        public string Password {
            get { return txtPassword.Password; }
        }
        public int FinancialYear
        {

            get
            {
                if (cmbBoxFinancialYearList.SelectedItem != null)
                { return (int)cmbBoxFinancialYearList.SelectedItem; }
                else
                {
                    return 0;
                }
            }
        
        }
        
        #endregion

        public LogOn() {
            InitializeComponent();
            this.FinancialYearList= new[] { 15,16,17,18,19,20};
            DataContext = this;
            txtUsername.Focus();
         }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged {
            add { propertyChangedEvent += value; }
            remove { propertyChangedEvent -= value; }
        }

        #endregion

        private void LogonClick(object sender, RoutedEventArgs e) {
            DialogResult = true;
            Close();
        }

        private void CredentialsFocussed(object sender, RoutedEventArgs e) {
            TextBoxBase tb = sender as TextBoxBase;
            if (tb == null) {
                //ComboBox cmb = sender as ComboBox;
                
                PasswordBox pwb = sender as PasswordBox;
                pwb.SelectAll();
            }
            else {
                tb.SelectAll();
            }
        }

        private event PropertyChangedEventHandler propertyChangedEvent;

        protected void OnPropertyChanged(string prop) {
            if (propertyChangedEvent != null)
                propertyChangedEvent(this, new PropertyChangedEventArgs(prop));
        }
    }
}