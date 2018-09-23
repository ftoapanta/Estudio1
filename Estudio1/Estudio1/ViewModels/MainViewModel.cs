using Estudio1.Models;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;

namespace Estudio1.ViewModels
{
    public class MainViewModel
    {
        #region Propiedades
        public string Monto { get; set; }
        public ObservableCollection<Cotizacion> Cotizaciones { get; set; }
        public Cotizacion CotizacionOrigen { get; set; }
        public Cotizacion CotizacionDestino { get; set; }
        public bool EstaCorriendo { get; set; }
        public bool EstaHabilitado { get; set; }
        public string Resultado { get; set; }

        #endregion

        #region Comandos
        public ICommand ConvertirCommand {
            get
            {
                return new RelayCommand(Convertir);
            }
        }

        public void Convertir()
        {
            Resultado = "Listo";
            //throw new NotImplementedException();
        }
        #endregion

        public MainViewModel()
        {
            EstaCorriendo = true;
            EstaHabilitado = true;
            Resultado = "......";
        }
    }
}
