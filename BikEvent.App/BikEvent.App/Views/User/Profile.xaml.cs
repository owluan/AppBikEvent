using BikEvent.Domain.Models;
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
    public partial class Profile : ContentPage
    {
        string about = "🚵‍♂️ Piloto de Downhill | 🌲 Trail Builder | 👨‍🏫 Instrutor de Pilotagem | 🚴‍♂️ Ciclista desde 20XX \r\n\r\n    Desbravando trilhas íngremes e desafiadoras há mais de duas décadas! 🏔️💨 Sou apaixonado por cada curva, raiz e salto que a natureza proporciona. Como piloto de downhill, encontro na adrenalina e na precisão o combustível para superar os obstáculos mais intrépidos.\r\n\r\n    Além de pilotar, sou um construtor de trilhas dedicado. 🌳💪 Criar novos caminhos que desafiam os limites da gravidade e da imaginação é minha paixão. Cada canto da natureza é uma tela em branco, pronta para se tornar um parque de diversões para ciclistas destemidos.\r\n\r\n    Como instrutor de pilotagem, compartilho minha experiência e conhecimento para ajudar outros a dominar as técnicas necessárias para descer montanhas com segurança e habilidade. 📚👨‍🏫 Ver meus alunos superando seus limites e conquistando novos horizontes é minha maior recompensa.\r\n\r\n    E quando não estou desafiando montanhas, estou explorando novos lugares sobre duas rodas. 🌄🚴‍♂️ O ciclismo é mais do que um hobby, é um estilo de vida que me mantém conectado à natureza e aos meus próprios limites.\r\n\r\n    Junte-se a mim nessa jornada cheia de aventura, superação e paixão pela bicicleta! 🚵‍♂️✨ \r\n\r\n #DownhillLife #TrailBuilder #CyclingAdventures";

        public Profile()
        {
            InitializeComponent();
            aboutLabel.Text = about;
        }

        private void InstagramButtonClicked(object sender, EventArgs e)
        {

        }

        private void FacebookButtonClicked(object sender, EventArgs e)
        {

        }

        private void StravaButtonClicked(object sender, EventArgs e)
        {

        }
    }
}