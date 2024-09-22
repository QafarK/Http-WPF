using System.Net;
using System.Text.Json;
namespace server;

public class WebHost
{

    private int _port;
    private HttpListener? _listener;

    private delegate void MyDelegate(HttpListenerContext context);

    private List<MyDelegate> _delegates;

    public WebHost(int port)
    {
        _port = port;
        _delegates = [];
        //_delegates.Append(HandleRequest);

    }
    //obj[0] = HandleRequest1;

    //obj[0].Invoke(null);

    public void Run()
    {
        _listener = new HttpListener();
        _listener.Prefixes.Add($"http://localhost:{_port}/");
        _listener.Start();

        Console.WriteLine($"Http server started on {_port}");

        while (true)
        {
            try
            {
                var context = _listener.GetContext();

                if (context.Request.HttpMethod == "POST")
                    _ = Task.Run(() => HandlePostRequest(context));
                //
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling request: {ex.Message}");
            }
        }

    }


    private async void HandlePostRequest(HttpListenerContext context)
    {
        try
        {
            // Read the request body asynchronously
            using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
            {
                string requestBody = await reader.ReadToEndAsync();
                Console.WriteLine("Request Body: " + requestBody); // Request's full body

                var user = JsonSerializer.Deserialize<User>(requestBody);
                if (user is not null)
                    Console.WriteLine($"Received User: Id={user.Id}, Name={user.Name}, Surname={user.Surname}, Age={user.Age}");
            }
            // READING ENDED

            // Respond to the client
            HttpListenerResponse responseToClient = context.Response;
            responseToClient.ContentType = "application/json";
            responseToClient.ContentEncoding = System.Text.Encoding.UTF8;

            string messageToClient = "Data received successfully";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(messageToClient);
            responseToClient.ContentLength64 = buffer.Length;

            await responseToClient.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            responseToClient.OutputStream.Close();

            Console.WriteLine("Response sent...\t\t" + DateTime.Now.ToLongTimeString());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing request: {ex.Message}");
        }

    }
}

