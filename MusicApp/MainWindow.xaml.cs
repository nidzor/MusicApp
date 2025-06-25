using System.IO;
using System.Windows;
using System.Windows.Media;

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
            Directory.CreateDirectory(musicFilesPath);
            // music download from links.json (maybe txt)
        }
        
    }

    private string musicFilesPath = "C:\\music";    // deafult
    private int howManySongs;
    Random random = new Random();
    MediaPlayer player = new MediaPlayer();
    private int currentIndex;
    private int randomIndex = 100;
    private TimeSpan stopPosition;
    private bool stopped = false;
    


    private void PlayRandom(object sender, RoutedEventArgs e)
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

    private void Player()
    {
        howManySongs = Directory.GetFiles(musicFilesPath).Length;
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
                randomIndex = random.Next(0, howManySongs);
                if (currentIndex == randomIndex)
                {
                    try
                    {
                        currentIndex += 1;
                    }
                    catch (Exception e)
                    {
                        currentIndex -= 1;
                    }
                }
                else currentIndex = randomIndex;

                var path = files[randomIndex].ToString();
                player.Open(new Uri(path));
                player.Play();

                string title = Path.GetFileNameWithoutExtension(path);
                titleBlock.Text = title;
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
        TimeSpan duration = player.NaturalDuration.TimeSpan;
        MessageBox.Show("Audio length: " + duration.ToString(@"ss"));
        
    }
}