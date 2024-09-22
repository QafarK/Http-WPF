using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Windows;


namespace http;


public partial class MainWindow : Window
{

    List<User> Users;
    public MainWindow()
    {
        InitializeComponent();
        Users = [];
        listView.ItemsSource = Users;//!------------------------

    }

    private async void AddOrUpdate(object sender, RoutedEventArgs e) // add | update
    {
        try
        {

            var client = new HttpClient();

            var user = new User { Id = Users.Count + 1, Name = name.Text, Surname = surname.Text, Age = Convert.ToInt16(age.Text) };

            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("http://localhost:27001/", content); // Posting to Server
            Users.Add(user);
            var responseString = await response.Content.ReadAsStringAsync(); // Reading back Response from Server (Status posting)

            if (responseString is not null)
                MessageBox.Show("Response from server: " + responseString + ' ' + DateTime.Now.ToLongTimeString());
            else
                MessageBox.Show("Response from server: NULL " + DateTime.Now.ToLongTimeString());

        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private async void GetAllUsers(object sender, RoutedEventArgs e) // get all users
    {
        try
        {
            //var str = await client.GetStringAsync("http://localhost:27001/");
            //MessageBox.Show(str);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void DELETE(object sender, RoutedEventArgs e) // DELETE
    {

    }

}