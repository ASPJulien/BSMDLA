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
        public MainWindow()
        {
        
        }
         private void Button_Click(object sender, RoutedEventArgs e)
         {
             if (Program.BSRCorrect)
                 Program.DownloadUrl(Program.mapdetails.downloadURL);
         }    
         
        private void MapKey_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            string enteredBSR;
            enteredBSR = @"https://maps.beatsaberplus.com/api/maps/detail/" + MapKey.Text;
            Program.ReadLink(enteredBSR);
            if (Program.BSRCorrect)
            {
                MapName.Content = Program.mapdetails.name;
                var image = new Image();
                var fullFilePath = Program.mapdetails.coverURL;

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
    }
}
