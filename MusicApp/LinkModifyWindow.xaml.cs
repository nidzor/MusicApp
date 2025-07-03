using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Text.Json;
using System.Collections.Generic;
using System.Windows.Controls;

namespace MusicApp;

public partial class LinkModifyWindow : Window
{
    public LinkModifyWindow()
    { InitializeComponent(); }
    
    private string musicFilesPath = "C:\\music";    // deafult
    
    
    
    
    private void ReturnCurrentLists()
    {
        string filePath = musicFilesPath + "/Links/Links.json";
        string json = File.ReadAllText(filePath);
        var data = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json);
    
        DeleteGroupList.ItemsSource = data.Keys;
    }

    private void LinkAdd(string key, string url)
    {
        string filePath = musicFilesPath + "/Links/Links.json";
        string json = File.ReadAllText(filePath);
        var data = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json);

        if (!data[key].Contains(url))
        {
            data[key].Add(url);
        }

        var options = new JsonSerializerOptions { WriteIndented = true };
        string newJson = JsonSerializer.Serialize(data, options);
        File.WriteAllText(filePath, newJson);
    }

    private void ShowGrid(object sender, RoutedEventArgs e)
    {
        AddGroupGrid.Visibility = Visibility.Hidden;
        DeleteGroupGrid.Visibility = Visibility.Hidden;
        
        Button button = (Button)sender;
        switch (button.Content.ToString())
        {
            case "Add Group":
                AddGroupGrid.Visibility = Visibility.Visible;
                break;
            case "Delete Group":
                ReturnCurrentLists();
                DeleteGroupGrid.Visibility = Visibility.Visible;
                break;
            case "Add Link":
                
                break;
            case "Delete Link":
                
                break;
        }
    }
}