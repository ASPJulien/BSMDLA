using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Microsoft.Win32;
using Newtonsoft.Json;
using Ookii.Dialogs.Wpf;

namespace BS__MapDownloader
{
    public static class Program
    {
        public static MapDetails mapdetails = new MapDetails();
        public static bool BSRCorrect;
        public static string urlBase = "https://beatsaver.com";
        public static void ReadLink(string enteredUrl)
        {
            using (WebClient wc = new WebClient())
            {
                var client = new WebClient();
                client.Headers.Add("user-agent","Mozilla/4.0 (compatible; MSIE 6.0; " + "Windows NT 5.2; .NET CLR 1.0.3705;)");
                string response = "";
                try
                {
                    response = client.DownloadString(enteredUrl);
                    BSRCorrect = true;
                }
                catch (Exception e)
                {
                    BSRCorrect = false;
                }
                mapdetails = JsonConvert.DeserializeObject<MapDetails>(response);
            }
        }
    
        public static void DownloadUrl(Uri url, string folderPath = null)
        {
            try
            {
                using (var client = new WebClient())
                {
                    client.Headers.Add("user-agent","Mozilla/4.0 (compatible; MSIE 6.0; " + "Windows NT 5.2; .NET CLR 1.0.3705;)");
                    Thread.Sleep(50); // this cuz beatsaver sucks and send a 403 if too quick
                    if (folderPath == null)
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.FileName = mapdetails.name + ".zip";
                        saveFileDialog.Filter = "Beatsaber map zip (*.zip)|*.zip";
                        if (saveFileDialog.ShowDialog() == true)
                            client.DownloadFile(url, saveFileDialog.FileName);
                    }
                    else
                    {
                        client.DownloadFile(url, folderPath);
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
        
        public static void DecodePlaylist()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string playlistJson = File.ReadAllText(openFileDialog.FileName);
                    MessageBox.Show(playlistJson);
                    PlaylistDetails playlist = JsonConvert.DeserializeObject<PlaylistDetails>(playlistJson);
                    MessageBox.Show(playlist.songs.Count.ToString());
                    SaveFileDialog saveMaFileDialog = new SaveFileDialog();
                    foreach (var map in playlist.songs)
                        map.hash = map.hash.ToLower();
                    VistaFolderBrowserDialog openFolderDialog = new VistaFolderBrowserDialog();
                    if (openFolderDialog.ShowDialog() == true)
                    {
                        string FolderPath = openFolderDialog.SelectedPath;
                        foreach (var map in playlist.songs)
                        {
                            using (WebClient wc = new WebClient())
                            {
                                var client = new WebClient();
                                client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; " + "Windows NT 5.2; .NET CLR 1.0.3705;)");

                                string response = "";
                                try
                                {
                                    Uri mapUri = new Uri(@"https://beatsaver.com/api/maps/by-hash/" + map.hash);
                                    response = client.DownloadString(mapUri);
                                }
                                catch (Exception e)
                                {
                                    MessageBox.Show("Error");
                                }
                                MapDetails mapdet = new MapDetails();
                                mapdet = JsonConvert.DeserializeObject<MapDetails>(response);
                                Uri address = new Uri(urlBase + mapdet.downloadURL);
                                string FileName = FolderPath + @"\"+ map.songName + ".zip"; 
                                DownloadUrl(address, FileName);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }
    }
    
    public class MapDetails
    {
        public Uri downloadURL { get; set; }
        public Uri coverURL { get; set; }
        public string name { get; set; }
    }

    public class PlaylistDetails
    {
        public List<PlaylistMapDetails> songs {get; set;}
    }

    public class PlaylistMapDetails
    {
        public string songName { get; set; }
        public string hash { get; set; }
    }
}