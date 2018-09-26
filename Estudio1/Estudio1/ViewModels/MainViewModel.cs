using Estudio1.Models;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using Xamarin.Forms;
using System.ComponentModel;

namespace Estudio1.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        //para que se active el binding
        public event PropertyChangedEventHandler PropertyChanged;
        #region Atributos Para ue se ejecute los binding
        bool estaCorriendo { get; set; }
        string resultado { get; set; }
        #endregion

        #region Propiedades
        public bool EstaCorriendo
        {
            get
            {
                return estaCorriendo;
            }
            set
            {
                if (estaCorriendo != value)
                {
                    estaCorriendo = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EstaCorriendo)));
                }

            }
        }
        public string Resultado
        {
            get
            {
                return resultado;
            }
            set
            {
                if (resultado != value)
                {
                    resultado = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Resultado)));
                }

            }
        }
        public string Monto { get; set; }
        public ObservableCollection<Cotizacion> Cotizaciones { get; set; }
        public Cotizacion CotizacionOrigen { get; set; }
        public Cotizacion CotizacionDestino { get; set; }
        public bool EstaHabilitado { get; set; }

        #endregion

        #region Comandos
        public ICommand ConvertirCommand { 
            get
            {
                return new RelayCommand(Convertir);
            }
        }
        void Convertir()
        {
            Resultado = "Listo";
            //throw new NotImplementedException();
        }
        #endregion

        public MainViewModel()
        {
            CargarCotizaciones();
        }

        void CargarCotizaciones()
        {
            EstaCorriendo = true;
            Resultado = "Cargando monedas...";
        }
    }
}
