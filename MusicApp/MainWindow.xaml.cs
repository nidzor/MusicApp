using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Text.Json;
using System.Collections.Generic;

namespace MusicApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        player.Volume = 0.05;

        if (!Directory.Exists(musicFilesPath))
        {
            Console.WriteLine("created");
            Directory.CreateDirectory(musicFilesPath);
            // music download from links.json (maybe txt)
        }
        
    }

    private string musicFilesPath = "C:\\music";    // deafult
    private int howManySongs;
    Random random = new Random();
    MediaPlayer player = new MediaPlayer();
    private List<int> LastIndex =  new List<int>();
    private int randomIndex = 100;
    private TimeSpan stopPosition;
    private bool stopped = false;
    


    private void Play(object sender, RoutedEventArgs e)
    {
        Player();
    }
    private void Stop(object sender, RoutedEventArgs e)
    {
        stopped = true;
        player.Pause(); // this exists fuckface
    }
    
    private void changeVolume(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        player.Volume = volumeSlider.Value;
    }


    private void LastIndexUpdate(int x)
    {
        if (howManySongs > 5)
        {
            LastIndex.Add(x);
            int temp = howManySongs / 2;
            if (LastIndex.Count > temp)
            {
                LastIndex.RemoveAt(0);
            }
        }
    }
    
    private void Player()
    {
        howManySongs = Directory.GetFiles(musicFilesPath).Length-1;
        if (howManySongs == 0) { MessageBox.Show("check C:/music"); }
        else
        {
            var files = new DirectoryInfo(musicFilesPath).GetFiles();

            if (stopped)
            {
                stopped = false;
                player.Play();
            }
            else
            {   
                while (true){
                    randomIndex = random.Next(0, howManySongs);
                    if (!LastIndex.Contains(randomIndex))
                    {
                        break;
                    }
                }
                

                var path = files[randomIndex].ToString();
                player.Open(new Uri(path));
                player.Play();

                string title = Path.GetFileNameWithoutExtension(path);
                titleBlock.Text = title;
                LastIndexUpdate(randomIndex);
            }

            player.MediaEnded += Loop;
        }
    }

    private void Loop(object sender, EventArgs e)
    {
        if (loopBox.IsChecked == true)
        {
            Player();
        }
    }

    private void Test(object sender, RoutedEventArgs e)
    {
        
    }

    private void OpenWindow(object sender, RoutedEventArgs e)
    {
        LinkModifyWindow temp = new LinkModifyWindow();
        temp.Show();
    }
}