using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
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
        public static void ReadLink(string enteredUrl)
        {
            using (WebClient wc = new WebClient())
                {
                    var client = new WebClient();
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
    
        public static void DownloadUrl(Uri url)
        {
            try
            {
                using (var client = new WebClient())
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.FileName = mapdetails.name + ".zip";
                    saveFileDialog.Filter = "Beatsaber map zip (*.zip)|*.zip";
                    if (saveFileDialog.ShowDialog() == true)
                        client.DownloadFile(url, saveFileDialog.FileName);
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
                                string response = "";
                                try
                                {
                                    response = client.DownloadString(@"https://maps.beatsaberplus.com/api/maps/by-hash/" + map.hash);
                                }
                                catch (Exception e)
                                {
                                    MessageBox.Show("Error");
                                }
                                MapDetails mapdet = new MapDetails();
                                mapdet = JsonConvert.DeserializeObject<MapDetails>(response);
                                client.DownloadFile(mapdet.downloadURL, FolderPath + @"\" + map.songName + ".zip");
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