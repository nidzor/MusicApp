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



    private Dictionary<string, List<string>> loadJson()
    {
        string filePath = musicFilesPath + "/Links/Links.json";
        string json = File.ReadAllText(filePath);
        var data = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json);
        return data;
    }

    private void saveJson(Dictionary<string, List<string>> data)
    {
        string filePath = musicFilesPath + "/Links/Links.json";
        var options = new JsonSerializerOptions { WriteIndented = true };
        string newJson = JsonSerializer.Serialize(data, options);
        File.WriteAllText(filePath, newJson);
    }
    
    
    private void ReturnCurrentLists(ListView x)
    {
        var data = loadJson();
        x.ItemsSource = data.Keys;
    }

    private void LinkAdd(string key, string url)
    {
        var data = loadJson();
        if (!data[key].Contains(url))
        {
            data[key].Add(url);
        }
        saveJson(data);
    }

    private void AddGroup(object sender, RoutedEventArgs e)
    {
        string name = NewGroupNameBox.Text;
        var data = loadJson();
        if (!data.Keys.Contains(name))
        {
            data.Add(name, new List<string>());
        }
        saveJson(data);
        Outputs.Text = "Group added";
    }

    private void RemoveGroup(object sender, RoutedEventArgs e)
    {
        var data = loadJson();
        string item =  DeleteGroupList.SelectedItem.ToString();
        data.Remove(item);
        saveJson(data);
        ReturnCurrentLists(DeleteGroupList);
        Outputs.Text = "Group deleted";
    }

    private void AddLink(object sender, RoutedEventArgs e)
    {
        string group;
        var data = loadJson();
        try
        {
            group = AddLinkGroupList.SelectedItem.ToString();
        }
        catch (Exception exception)
        {
            Outputs.Text = "select a group";
            return;
        }
        data[group].Add(NewLinkBox.Text);
        saveJson(data);
        Outputs.Text = "Link added";
    }

    private void SelectGroup(object sender, RoutedEventArgs e)
    {
         var data = loadJson();
         string key = DeleteLinkGroupList.SelectedItem.ToString();
         DeleteLinkLinkList.ItemsSource = data[key];
    }

    private void DeleteLink(object sender, RoutedEventArgs e)
    {
        var data = loadJson();
        string key = DeleteLinkGroupList.SelectedItem.ToString();
        string url = DeleteLinkLinkList.SelectedItem.ToString();
        data[key].Remove(url);
        saveJson(data);
        DeleteLinkLinkList.ItemsSource = data[key];
        Outputs.Text = "Link deleted";
    }
    
    private void ShowGrid(object sender, RoutedEventArgs e)
    {
        AddGroupGrid.Visibility = Visibility.Hidden;
        DeleteGroupGrid.Visibility = Visibility.Hidden;
        AddLinkGrid.Visibility = Visibility.Hidden;
        DeleteLinkGrid.Visibility = Visibility.Hidden;
        
        Button button = (Button)sender;
        switch (button.Content.ToString())
        {
            case "Add Group":
                AddGroupGrid.Visibility = Visibility.Visible;
                break;
            case "Delete Group":
                ReturnCurrentLists(DeleteGroupList);
                DeleteGroupGrid.Visibility = Visibility.Visible;
                break;
            case "Add Link":
                ReturnCurrentLists(AddLinkGroupList);
                AddLinkGrid.Visibility = Visibility.Visible;
                break;
            case "Delete Link":
                ReturnCurrentLists(DeleteLinkGroupList);
                DeleteLinkGrid.Visibility = Visibility.Visible;
                break;
        }
    }
}