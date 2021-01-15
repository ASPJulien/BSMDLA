using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.UriKind;

namespace BS__MapDownloader
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SolidColorBrush colorWhite = new SolidColorBrush(Color.FromRgb(255, 248, 225));
        private SolidColorBrush colorDark = new SolidColorBrush(Color.FromRgb(96, 125, 139));

        public MainWindow()
        {
        
        }
         private void Button_Click(object sender, RoutedEventArgs e)
         {
             if (Program.BSRCorrect)
                 Program.DownloadUrl(new Uri(Program.urlBase + Program.mapdetails.downloadURL));
         }    
         
        private void MapKey_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            string enteredBSR;
            enteredBSR = @"https://beatsaver.com/api/maps/detail/" + MapKey.Text;
            Program.ReadLink(enteredBSR);
            if (Program.BSRCorrect)
            {
                MapName.Content = Program.mapdetails.name;
                var image = new Image();
                var fullFilePath = new Uri(Program.urlBase + Program.mapdetails.coverURL);

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = fullFilePath;
                bitmap.EndInit();
                MapImage.Source = bitmap;            
            }
        }

        private void Playlist_Button_Click(object sender, RoutedEventArgs e)
        {
            Program.DecodePlaylist();
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            if (DarkThemeButton.IsChecked == false)
            {
                MainGrid.Background = colorWhite;
            }

            if (DarkThemeButton.IsChecked == true)
            {
                MainGrid.Background = colorDark;
            }
        }
    }
}
