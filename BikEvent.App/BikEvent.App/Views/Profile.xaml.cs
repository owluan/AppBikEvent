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
        List<Comment> comments = new List<Comment>
            {
                new Comment { UserName = "Usuário1", CommentText = "Este é o 1 comentário.Este é o primeiro comentário.Este é o primeiro comentário.Este é o primeiro comentário.Este é o primeiro comentário.Este é o primeiro comentário.Este é o primeiro comentário.Este é o primeiro comentário.Este é o primeiro comentário.Este é o primeiro comentário.Este é o primeiro comentário.Este é o primeiro comentário." },
                new Comment { UserName = "Usuário1", CommentText = "Este é o 2 comentário." },
                new Comment { UserName = "Usuário1", CommentText = "Este é o 3 comentário." },
                new Comment { UserName = "Usuário1", CommentText = "Este é o 4 comentário." },
                new Comment { UserName = "Usuário1", CommentText = "Este é o 5 comentário." },
                new Comment { UserName = "Usuário1", CommentText = "Este é o 6 comentário." },
                new Comment { UserName = "Usuário1", CommentText = "Este é o 7 comentário." },
                new Comment { UserName = "Usuário1", CommentText = "Este é o 8 comentário." },
                new Comment { UserName = "Usuário1", CommentText = "Este é o 9 comentário." },
                new Comment { UserName = "Usuário1", CommentText = "Este é o 10 comentário." },
            };

        public Profile()
        {
            InitializeComponent();
            CommentsListView.ItemsSource = comments;
            UpdateListViewHeight();
        }

        private void UpdateListViewHeight()
        {
            double itemHeight = 100;
            double listViewHeight = comments.Count * itemHeight;
            CommentsListView.HeightRequest = listViewHeight;
        }
    }
}