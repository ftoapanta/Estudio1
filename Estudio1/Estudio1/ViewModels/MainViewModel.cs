using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using Xamarin.Forms;
using System.ComponentModel;
using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Estudio1.Models;
using Estudio1.Services;
using System.Threading.Tasks;

namespace Estudio1.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        //para disparar el binding a la vista se lo hace utilizando en las propiedades
        public event PropertyChangedEventHandler PropertyChanged;

        #region Servicios
        ApiService apiService;
        DialogService dialogService;
        DataService dataService;
        #endregion

        #region Atributos Para ue se ejecute los binding
        bool estaCorriendo { get; set; }
        bool estaHabilitado { get; set; }
        string resultado { get; set; }
        public ObservableCollection<Cotizacion> cotizaciones { get; set; }
        public Cotizacion cotizacionOrigen { get; set; }
        public Cotizacion cotizacionDestino { get; set; }
        public string monto { get; set; }
        #endregion

        #region Propiedades
        List<rate> listaRates;
        List<Cotizacion> listaCotizaciones;
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
        public bool EstaHabilitado
        {
            get
            {
                return estaHabilitado;
            }
            set
            {
                if (estaHabilitado != value)
                {
                    estaHabilitado = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EstaHabilitado)));
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
        public ObservableCollection<Cotizacion> Cotizaciones
        {
            get
            {
                return cotizaciones;
            }
            set
            {
                if (cotizaciones != value)
                {
                    cotizaciones = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Cotizaciones)));
                }

            }
        }
        public string Monto
        {
            get
            {
                return monto;
            }
            set
            {
                if (monto != value)
                {
                    monto = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Monto)));
                }

            }
        }
        public Cotizacion CotizacionOrigen
        {
            get
            {
                return cotizacionOrigen;
            }
            set
            {
                if (cotizacionOrigen != value)
                {
                    cotizacionOrigen = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CotizacionOrigen)));
                }

            }
        }
        public Cotizacion CotizacionDestino
        {
            get
            {
                return cotizacionDestino;
            }
            set
            {
                if (cotizacionDestino != value)
                {
                    cotizacionDestino = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CotizacionDestino)));
                }

            }
        }

        #endregion

        #region Comandos
        public ICommand IntercambiarCommand
        {
            get
            {
                return new RelayCommand(Intercambiar);
            }
        }
        public ICommand ConvertirCommand { 
            get
            {
                return new RelayCommand(Convertir);
            }
        }

        async void Convertir() //async para que se pueda liberar los mensajes
        {
            if (string.IsNullOrEmpty(Monto))
            {
                await dialogService.ShowMessage("Error", "Ingrese monto");
                return;
            }
            decimal monto=0;
            if(!decimal.TryParse(Monto, out monto))
            {
                await dialogService.ShowMessage("Error", "Ingrese un valor numérico");
                return;
            }
            if (CotizacionOrigen==null)
            {
                await dialogService.ShowMessage("Error", "Seleccione Moneda Origen");
                return;
            }
            if (CotizacionDestino == null)
            {
                await dialogService.ShowMessage("Error", "Seleccione Moneda Destino");
                return;
            }

            var respuesta = (Math.Round(Convert.ToDouble(Monto) * (CotizacionOrigen.mid/ CotizacionDestino.mid),2)).ToString();
            Resultado = String.Format("{0:C2} {1} = {2:c2} {3}",Monto,CotizacionOrigen.code,respuesta,CotizacionDestino.code);
        }

        void Intercambiar()
        {
            var aux = CotizacionOrigen;
            CotizacionOrigen = CotizacionDestino;
            CotizacionDestino = aux;
            Convertir();
        }
        #endregion

        #region Constructores
        public MainViewModel()
        {
            apiService = new ApiService();
            dialogService = new DialogService();
            dataService = new DataService();
            CargarCotizaciones();
        }
        #endregion

        async void CargarCotizaciones()
        {
            EstaCorriendo = true;
            Resultado = "Cargando monedas...";

            var connection = await apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                LoadLocalData();
            }
            else
            {
                await LoadDataFromAPI();
            }

            if (listaCotizaciones.Count == 0)
            {
                EstaCorriendo = false;
                EstaHabilitado = false;
                Resultado = "There are not internet connection and not load " +
                    "previously rates. Please try again with internet " +
                    "connection.";
                //var status = "No rates loaded.";
                return;
            }
            try
            {

                Cotizaciones = new ObservableCollection<Cotizacion>();
                listaCotizaciones.ForEach(x => Cotizaciones.Add(new Cotizacion { code = x.code, CotizacionId = x.CotizacionId, currency = x.currency, mid = x.mid }));

                EstaCorriendo = false;
                EstaHabilitado = true;
                Resultado = "Listo para convertir...";
             }
            catch (Exception ex)
            {
                EstaCorriendo = false;
                Resultado = ex.Message;
            }
        }
        void LoadLocalData()
        {
            listaCotizaciones = dataService.Get<Cotizacion>();

            Resultado = "Rates loaded from local data.";
        }
        async Task LoadDataFromAPI()
        {
            var url = "http://api.nbp.pl"; //Application.Current.Resources["URLAPI"].ToString();

            var response = await apiService.GetList<Rootobject>(
                url,
                "/api/exchangerates/tables/A");

            if (!response.IsSuccess)
            {
                LoadLocalData();
                return;
            }

            var respuestaCotizaciones = ((List<Rootobject>)response.Result);
            listaRates = respuestaCotizaciones[0].rates;
            
            #region Solo en Xamarin Live
            //Solo para cuando se ejecuta desde Xamarin Live
            var EsXamarinLive = false;
            if (EsXamarinLive) //Falla solo en Xamarin Live
            {
                listaRates = new List<rate>();
                listaRates.Add(new rate { code = "EUR", mid = 0.85, currency = "Euro" });
                listaRates.Add(new rate { code = "COP", mid = 2200, currency = "Peso Colombino" });
                listaRates.Add(new rate { code = "PEN", mid = 2.8, currency = "Nuevo Sol Peruano" });
            }
            #endregion

            int id = 0;
            listaCotizaciones = new List<Cotizacion>();
            foreach (var item in listaRates)
            {
                try
                {
                    id++;
                    listaCotizaciones.Add(new Cotizacion
                    {
                        CotizacionId = id,
                        code = item.code,
                        currency = item.currency,
                        mid = item.mid
                    });
                }
                catch (Exception ex)
                {
                    Resultado = ex.Message;
                    EstaCorriendo = false;
                    EstaHabilitado = false;
                    return;
                };
            }


            // Storage data local
            dataService.DeleteAll<Cotizacion>();
            dataService.Save(listaCotizaciones);

            //Status = "Rates loaded from Internet.";
        }
    }
}
