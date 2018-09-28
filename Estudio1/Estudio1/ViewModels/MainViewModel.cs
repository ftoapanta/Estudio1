using Estudio1.Models;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using Xamarin.Forms;
using System.ComponentModel;
using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Estudio1.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        //para que se active el binding
        public event PropertyChangedEventHandler PropertyChanged;

        #region Atributos Para ue se ejecute los binding
        bool estaCorriendo { get; set; }
        bool estaHabilitado { get; set; }
        string resultado { get; set; }
        public ObservableCollection<Rate> cotizaciones { get; set; }
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
        public ObservableCollection<Rate> Cotizaciones
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
        public string Monto { get; set; }
        public ObservableCollection<RootObject> CotizacionesObject { get; set; }
        public Cotizacion CotizacionOrigen { get; set; }
        public Cotizacion CotizacionDestino { get; set; }

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
                var respuesta = await cliente.GetAsync(controlador);
                var textoRespuesta = await respuesta.Content.ReadAsStringAsync();
                if(!respuesta.IsSuccessStatusCode)
                {
                    EstaCorriendo = false;
                    Resultado = textoRespuesta;
                }
                var respuestaCotizaciones = JsonConvert.DeserializeObject<List<RootObject>>(textoRespuesta);
                var listaCotizaciones = respuestaCotizaciones[0].rates;
                Cotizaciones = new ObservableCollection<Rate>();
                Resultado = respuestaCotizaciones[0].rates[0].currency.ToString();
/*                Cotizaciones.Add(new Rate
                {
                    code = listaCotizaciones[0].code,
                    currency = listaCotizaciones[0].currency,
                    mid = listaCotizaciones[0].mid

                });
                foreach (var item in listaCotizaciones)
                {
                    try
                    {
                        Cotizaciones.Add(new Rate
                        {
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
                }*/
                //Resultado = "Listo para convertir";
                EstaCorriendo = false;
                EstaHabilitado = true;

            }
            catch (Exception ex)
            {
                EstaCorriendo = false;
                Resultado = ex.Message;
            }
        }
    }
}
