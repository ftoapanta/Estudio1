namespace Estudio1.Services
{
    using System.Threading.Tasks;
    //using Helpers; //cundo pongamos lo del lenguaje
    using Xamarin.Forms;
    public class DialogService
    {
        public async Task ShowMessage(string title, string messsage)
        {
            await Application.Current.MainPage.DisplayAlert(
                title,
                messsage,
               //Lenguages.Accept);  //cuano pongamos lo del lenguaje
               "Aceptar");
        }
    }
}
