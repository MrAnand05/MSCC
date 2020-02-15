using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace ViewSwitchingNavigation.Exam.Views
{
    /// <summary>
    /// Interaction logic for ExamView.xaml
    /// </summary>
    [Export("ExamView")]
    public partial class ExamView : UserControl
    {
        public ExamView()
        {
            InitializeComponent();
        }
    }
}
