using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xam.Plugin.Badger.Abstractions;
using System.Windows.Input;

namespace Estudio1.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainView : ContentPage
	{
        IBadgerService _badgeService;

        public MainView ()
		{
			InitializeComponent ();
		}
        public ICommand SetBadgeCountCommand
            => new Command<string>((count) => _badgeService.SetCount(int.Parse(count)));

    }
}