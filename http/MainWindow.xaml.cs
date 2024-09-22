using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Windows;


namespace http;


public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void AddOrUpdate(object sender, RoutedEventArgs e) // add | update
    {
        try
        {

            var client = new HttpClient();

            var json = JsonSerializer.Serialize(new User { Id = 1, Name = "John", Surname = "Doe", Age = 30 });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("http://localhost:27001/", content); // Posting to Server

            var responseString = await response.Content.ReadAsStringAsync();

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

    private void GetAllUsers(object sender, RoutedEventArgs e) // get all users
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