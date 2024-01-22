using BikEvent.App.Models;
using BikEvent.App.Resources.Converters;
using BikEvent.App.Resources.Load;
using BikEvent.App.Services;
using BikEvent.Domain.Models;
using BikEvent.Domain.Utility.Enums;
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
                EventDate = DatePicker.Date,
                NextEventDate = DatePicker.Date,
                RepeatInterval = (RBNone.IsChecked) ? RepeatInterval.None :
                 (RBWeekly.IsChecked) ? RepeatInterval.Weekly :
                 (RBBiWeekly.IsChecked) ? RepeatInterval.BiWeekly :
                 (RBMonthly.IsChecked) ? RepeatInterval.Monthly :
                 RepeatInterval.None,
                //FinalSalary = TextToDoubleConverter.ToDouble(TxtFinalSalary.Text),
                EventType = (RBTrilha.IsChecked) ? "Trilha" : (RBPedal.IsChecked) ? "Pedal" :
                (RBPasseio.IsChecked) ? "Passeio" : (RBCompeticao.IsChecked) ? "Competição" : (RBEncontro.IsChecked) ? "Encontro" : (RBOutro.IsChecked) ? "Outro" : "Default",
                Tag = TxtTags.Text,
                SocialMedia = TxtSocialMedia.Text,
                Description = TxtDescription.Text,
                Difficulty = (RBIniciante.IsChecked) ? "Iniciante" : (RBIntermediario.IsChecked) ? "Intermediário" :
                (RBAvançado.IsChecked) ? "Avançado" : "Default",
                Benefits = TxtBenefits.Text,
                PhoneNumber = TxtPhoneNumber.Text,
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