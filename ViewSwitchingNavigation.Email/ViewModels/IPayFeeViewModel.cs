using System.Windows.Input;
using ViewSwitchingNavigation.Email.Model;

namespace ViewSwitchingNavigation.Email.ViewModels
{
    public interface IPayFeeViewModel : IHeaderInfoProvider<string>
    {
        //IObservablePosition Position { get; }

        ICommand BuyCommand { get; }

        //ICommand SellCommand { get; }
    }
}
