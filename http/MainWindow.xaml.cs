using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Windows;
using System.Windows.Controls;


namespace http;


public partial class MainWindow : Window
{

    private List<User> Users;
    private HttpClient _client;
    private int _num = 1;
    private string _selectedItem;

    public MainWindow()
    {
        InitializeComponent();
        Users = [];
        _client = new HttpClient();
        _selectedItem = string.Empty;
    }

    private async void AddOrUpdate(object sender, RoutedEventArgs e) // add | update
    {
        try
        {
            int selectedItemId;
            User? user = null;
            if (_selectedItem != string.Empty)
            {
                selectedItemId = Convert.ToInt32(_selectedItem.Split(' ')[0].Split(':')[1]);
                user = new User { Id = selectedItemId, Name = name.Text, Surname = surname.Text, Age = Convert.ToInt16(age.Text) };
                _selectedItem = string.Empty;
            }
            else
                user = new User { Id = _num++, Name = name.Text, Surname = surname.Text, Age = Convert.ToInt16(age.Text) };

            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            // num count !!!!! response
            HttpResponseMessage response = await _client.PostAsync("http://localhost:27001/", content); // Posting to Server
            var responseString = await response.Content.ReadAsStringAsync(); // Reading back Response from Server (Status posting)


            if (responseString is not null)
                MessageBox.Show(DateTime.Now.ToLongTimeString() + " Response from server: " + responseString);
            else
                MessageBox.Show(DateTime.Now.ToLongTimeString() + " Response from server: NULL");

            GetAllUsers(sender, e);

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
            HttpResponseMessage response = await _client.GetAsync("http://localhost:27001/");

            var responseString = await response.Content.ReadAsStringAsync(); // Reading back Response from Server (Status getting)

            if (responseString is not null)
                MessageBox.Show("Response from server: " + DateTime.Now.ToLongTimeString());
            else
                MessageBox.Show(DateTime.Now.ToLongTimeString() + " Response from server: NULL");


            if (responseString is not null)
            {
                var splitedResponseString_ = responseString.Split('_');

                Users.Clear();

                for (int j = 0; j < splitedResponseString_.Length; j++)
                {

                    Users.Add(new User
                    {
                        Id = Convert.ToInt32(splitedResponseString_[j].Split('-')[0].Split(':')[1]),
                        Name = splitedResponseString_[j].Split('-')[1].Split(':')[1].ToString(),
                        Surname = splitedResponseString_[j].Split('-')[2].Split(':')[1].ToString(),
                        Age = Convert.ToInt32(splitedResponseString_[j].Split('-')[3].Split(':')[1])
                    });
                }
                listView.ItemsSource = null;
                listView.Items.Clear();
                listView.ItemsSource = Users;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private async void DELETE(object sender, RoutedEventArgs e) // DELETE
    {

        try
        {
            int selectedItemId;
            if (_selectedItem == string.Empty)
                MessageBox.Show("Please select Item to Delete");
            else
            {
                selectedItemId = Convert.ToInt32(_selectedItem.Split(' ')[0].Split(':')[1]);

                var content = new StringContent(selectedItemId.ToString(), Encoding.UTF8, "text/plain");

                HttpResponseMessage response = await _client.PutAsync("http://localhost:27001/", content); // Puting to Server
                var responseString = await response.Content.ReadAsStringAsync(); // Reading back Response from Server (Status puting)


                if (responseString is not null)
                    MessageBox.Show(DateTime.Now.ToLongTimeString() + " Response from server: " + responseString);
                else
                    MessageBox.Show(DateTime.Now.ToLongTimeString() + " Response from server: NULL");

                GetAllUsers(sender, e);

                _num = Users.Count + 1;
                _selectedItem = string.Empty;
                
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try
        {
            ListView? _listView = sender as ListView;
            if (_listView is not null && _listView.SelectedItem is not null)
                _selectedItem = _listView.SelectedItem.ToString()!;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
}