using BikEvent.App.Models;
using BikEvent.App.Resources.Converters;
using BikEvent.App.Resources.Load;
using BikEvent.App.Services;
using BikEvent.Domain.Models;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BikEvent.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterEvent : ContentPage
    {
        private EventService _eventService;
        public RegisterEvent()
        {
            InitializeComponent();
            _eventService = new EventService();
        }

        private void GoBack(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private async void Save(object sender, EventArgs e)
        {
            TxtMessages.Text = String.Empty;

            User user = JsonConvert.DeserializeObject<User>((string)App.Current.Properties["User"]);

            Event _event = new Event()
            {
                Company = user.Name,
                EventTitle = TxtEventTitle.Text,
                CityState = TxtCityState.Text,
                InitialSalary = TextToDoubleConverter.ToDouble(TxtInitialSalary.Text),
                FinalSalary = TextToDoubleConverter.ToDouble(TxtFinalSalary.Text),
                EventType = (RBTrilha.IsChecked) ? "Trilha" : (RBPedal.IsChecked) ? "Pedal" :
                (RBPasseio.IsChecked) ? "Passeio" : (RBCompeticao.IsChecked) ? "Competição" : (RBEncontro.IsChecked) ? "Encontro" : (RBOutro.IsChecked) ? "Outro" : "Default",
                Tag = TxtTags.Text,
                CompanyDescription = TxtCompanyDescription.Text,
                Description = TxtDescription.Text,
                Benefits = TxtBenefits.Text,
                InterestedSendEmailTo = TxtInterestedSendEmailTo.Text,
                PublicationDate = DateTime.Now,
                UserId = user.Id
            };



            await Navigation.PushPopupAsync(new Loading());

            ResponseService<Event> responseService = await _eventService.AddEvent(_event);

            if (responseService.IsSuccess)
            {
                await Navigation.PopAllPopupAsync();
                await DisplayAlert("Cadastro de Evento", "Evento cadastrado com sucesso!", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                if (responseService.StatusCode == 400)
                {
                    StringBuilder stringBuilder = new StringBuilder();

                    foreach (var itemKey in responseService.Errors)
                    {
                        foreach (var message in itemKey.Value)
                        {
                            stringBuilder.AppendLine(message);
                        }
                    }
                    TxtMessages.Text = stringBuilder.ToString();
                }
                else
                {
                    await DisplayAlert("Erro", "Oops! Ocorreu um erro inesperado, tente novamente mais tarde.", "OK");
                }
                await Navigation.PopAllPopupAsync();
            }
        }
    }
}