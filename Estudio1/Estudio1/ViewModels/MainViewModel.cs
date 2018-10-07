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

namespace Estudio1.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        //para disparar el binding a la vista se lo hace utilizando en las propiedades
        public event PropertyChangedEventHandler PropertyChanged;

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
                await Application.Current.MainPage.DisplayAlert("Error", "Ingrese monto", "Aceptar");
                return;
            }
            decimal monto=0;
            if(!decimal.TryParse(Monto, out monto))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Ingrese un valor numérico", "Aceptar");
                return;
            }
            if (CotizacionOrigen==null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Seleccione Moneda Origen", "Aceptar");
                return;
            }
            if (CotizacionDestino == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Seleccione Moneda Destino", "Aceptar");
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

        public MainViewModel()
        {
            CargarCotizaciones();
        }

        async void CargarCotizaciones()
        {
            EstaCorriendo = true;
            Resultado = "Cargando monedas...";
            var url = @"http://api.nbp.pl";
            var controlador = @"/api/exchangerates/tables/A";
            try
            {
                var cliente = new HttpClient();
                cliente.BaseAddress = new Uri(url);
                cliente.DefaultRequestHeaders.Accept.Add(
                  new MediaTypeWithQualityHeaderValue("application/json"));

                var respuesta = await cliente.GetAsync(controlador);
                var textoRespuesta = await respuesta.Content.ReadAsStringAsync();
                if (!respuesta.IsSuccessStatusCode)
                {
                    EstaCorriendo = false;
                    Resultado = textoRespuesta;
                }
                var respuestaCotizaciones = JsonConvert.DeserializeObject<List<Rootobject>>(textoRespuesta);
                List<rate> listaCotizaciones = respuestaCotizaciones[0].rates;

                //Solo para cuando se ejecuta desde Xamarin Live
                var EsXamarinLive = false;
                if (EsXamarinLive) //Falla solo en Xamarin Live
                {
                    listaCotizaciones = new List<rate>();
                    listaCotizaciones.Add(new rate { code = "EUR", mid = 0.85, currency = "Euro" });
                    listaCotizaciones.Add(new rate { code = "COP", mid = 2200, currency = "Peso Colombino" });
                    listaCotizaciones.Add(new rate { code = "PEN", mid = 2.8, currency = "Nuevo Sol Peruano" });
                }
                

                Cotizaciones = new ObservableCollection<Cotizacion>();
                int id = 0;
                foreach (var item in listaCotizaciones)
                {
                    try
                    {
                        id++;
                        Cotizaciones.Add(new Cotizacion
                        {
                            CotizacionId = id,
                            code = item.code,
                            currency = item.currency,
                            mid = item.mid
                        });
                    }
                    catch(Exception ex) {
                        Resultado = ex.Message;
                        EstaCorriendo = false;
                        EstaHabilitado = false;
                        return;
                    };
                }
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
    }

}
