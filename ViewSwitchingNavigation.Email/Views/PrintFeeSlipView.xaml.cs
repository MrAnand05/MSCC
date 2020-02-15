using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ViewSwitchingNavigation.Email.Model;

namespace ViewSwitchingNavigation.Email.Views
{
    /// <summary>
    /// Interaction logic for PrintFeeSlipView.xaml
    /// </summary>
    public partial class PrintFeeSlipView : Window
    {
        public PrintFeeSlipView(PayFeeModel payFeeModelObject, ObservableCollection<PayFeeModel> YearlyFeePaid, ObservableCollection<PayMonthlyFeeModel> MonthlyFeePaid)
        {
            InitializeComponent();
            //string StudentClass;
            //string Section;
            //if (payFeeModelObject.CurrentClass.ToString().Length > 4)
            //    StudentClass = payFeeModelObject.CurrentClass.ToString().Substring(4);
            //else
            //    StudentClass = payFeeModelObject.CurrentClass.ToString();
            //if (payFeeModelObject.Section.ToString() != "")
            //    Section = "/" + payFeeModelObject.Section.ToString();
            //else
            //    Section =payFeeModelObject.Section.ToString();

            txtAdminNo.Text = payFeeModelObject.AdminNo.ToString();
            txtName.Text = payFeeModelObject.Name.ToString();
            txtFName.Text ="Shri."+ payFeeModelObject.FatherName.ToString();
            txtClassSection.Text = payFeeModelObject.CurrentClass + payFeeModelObject.Section;
            txtAddress.Text = payFeeModelObject.Address.ToString();
            if(YearlyFeePaid.Count!=0)
            {
                int i = 0;
                
                foreach(PayFeeModel PFM in YearlyFeePaid )
                {
                    PayMonthlyFeeModel PMFM = new PayMonthlyFeeModel();    
                    PMFM.MonthName = PFM.Description;
                    PMFM.RequiredMonthlyFee=PFM.RequiredFee;
                    PMFM.MonthlyRebateGiven = PFM.RebateGiven;
                    PMFM.Monthlypaid = PFM.Paid;
                    PMFM.MontlyLastPaymentDate = PFM.LastPaymentDate;
                    PMFM.MonthlyBalance = PFM.Balance;
                    PMFM.MonthlyRebate = PFM.Rebate;
                    PMFM.MonthlyToPay = PFM.ToPay;
                    PMFM.MonthlyRebateRemark = PFM.RebateRemark;
                    MonthlyFeePaid.Insert(i,PMFM);
                    i=i+1;
                }
            }
            txtTotalFee.Text = MonthlyFeePaid.Sum(x => x.MonthlyBalance).ToString();
            txtTotalLess.Text = MonthlyFeePaid.Sum(x => x.MonthlyRebate).ToString();
            txtTotalAmountToPay.Text = MonthlyFeePaid.Sum(x => x.MonthlyToPay).ToString();
            txtBalance.Text = (Convert.ToInt32(txtTotalFee.Text) - Convert.ToInt32(txtTotalLess.Text) - Convert.ToInt32(txtTotalAmountToPay.Text)).ToString();
            lstViewMonthlyFeeDetails.ItemsSource = MonthlyFeePaid;
            //lstViewMonthlyFeeDetails2.ItemsSource = MonthlyFeePaid;
        }
        public void SetOtherField(int? PaySlipNo,DateTime DOP,string EditedFinancialYear)
        {
            txtFeeSlipNo.Text = PaySlipNo.ToString();
            txtDOP.Text = DOP.ToString();
            txtFinancialYear.Text = EditedFinancialYear;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            PrintDialog printDialog = new PrintDialog();
            
            if (printDialog.ShowDialog().GetValueOrDefault(false))
            {
                printDialog.PrintVisual(DublicateFeeSlip, "FeeSlip");
            }
            
        }
    }
}
