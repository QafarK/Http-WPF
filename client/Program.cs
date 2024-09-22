// See https://aka.ms/new-console-template for more information




using System.Text;
using System.Text.Json;

int a = 3;
while (a>0)
{
    await SendRequestAsync();
    Console.WriteLine(a);
    a--;
}

async Task SendRequestAsync()
{
    try
    {

  
    var client = new HttpClient();

        var json = JsonSerializer.Serialize(new User { Id = 1, Name = "John", Surname = "Doe", Age = 30 });
    var content = new StringContent(json, Encoding.UTF8, "application/json");

    var response = await client.PostAsync("http://localhost:27001/",content);
    //int b = 0;
    //Console.WriteLine(5/b);
    var responseString = await response.Content.ReadAsStringAsync();

    Console.WriteLine("Response from server: " + responseString + DateTime.Now.ToLongTimeString());
    }
    catch (Exception)
    {

        throw;
    }
    //// Create JSON data
    //var json = JsonSerializer.Serialize(new User { Id = 1, Name = "John", Surname = "Doe", Age = 30 });
    //var content = new StringContent(json, Encoding.UTF8, "application/json");

    //// Create a new HttpRequestMessage
    //var request = new HttpRequestMessage
    //{
    //    Method = HttpMethod.Post, // Change to any method you need (POST in this case)
    //    RequestUri = new Uri("http://localhost:27001/"),
    //    Content = content
    //};

    //// Send the request
    //HttpResponseMessage result = await client.SendAsync(request);

    //// Check the response
    //if (result.IsSuccessStatusCode)
    //{
    //    Console.WriteLine("Request sent successfully!");
    //    var responseBody = await result.Content.ReadAsStringAsync();
    //    Console.WriteLine(responseBody);
    //}
    //else
    //{
    //    Console.WriteLine($"Request failed with status code: {result.StatusCode}");
    //}
}
