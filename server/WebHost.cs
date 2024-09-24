using System.Linq;
using System.Net;
using System.Text.Json;
namespace server;

public class WebHost
{

    private int _port;
    private HttpListener? _listener;

    List<User> Users;

    public WebHost(int port)
    {
        _port = port;
        Users = [];

    }


    public void Start()
    {
        _listener = new HttpListener();
        _listener.Prefixes.Add($"http://localhost:{_port}/");
        _listener.Start();


        Console.WriteLine($"Http server started on {_port}\n");

        while (true)
        {
            try
            {

                var context = _listener.GetContext();

                if (context.Request.HttpMethod == "POST")
                    _ = Task.Run(() => HandlePostRequest(context));
                else if (context.Request.HttpMethod == "GET")
                    _ = Task.Run(() => HandleGetRequest(context));
                else if (context.Request.HttpMethod == "PUT")
                    _ = Task.Run(() => HandlePutRequest(context));
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
            // Reading the request body asynchronously
            using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
            {
                string requestBody = await reader.ReadToEndAsync();
                //Console.WriteLine("Request Body: " + requestBody); // Request's full body

                var user = JsonSerializer.Deserialize<User>(requestBody);
                if (user is not null)
                {
                    if (user.Id <= Users.Count) { }
                    else
                        user.Id = Users.Count + 1;
                    Console.WriteLine($"Received User: Id={user.Id}, Name={user.Name}, Surname={user.Surname}, Age={user.Age}");
                    if (Users.SingleOrDefault((u => u.Id == user.Id)) is not null)
                    {
                        int index = Users.FindIndex(u => u.Id == user.Id);
                        if (index != -1)
                            Users[index] = user;
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("User index not founed");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                    else
                        Users.Add(user);
                    Console.WriteLine(Users.Count);
                }
                else
                    throw new ArgumentNullException("user is null");
            }
            // READING ENDED

            // Respond message to the Client
            HttpListenerResponse responseToClient = context.Response;
            responseToClient.ContentType = "text/plain";
            responseToClient.ContentEncoding = System.Text.Encoding.UTF8;

            string messageToClient = $"Data received successfully\n";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(messageToClient);
            responseToClient.ContentLength64 = buffer.Length;

            await responseToClient.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            responseToClient.OutputStream.Close();

            Console.WriteLine("Response message to the Client sent at " + DateTime.Now.ToLongTimeString() + '\n');
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing request: {ex.Message}");
        }


    }
    private async void HandlePutRequest(HttpListenerContext context)
    {
        try
        {
            using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
            {
                var requestBody = await reader.ReadToEndAsync();
                //Console.WriteLine("Request Body: " + requestBody); // Request's full body
                int id = Convert.ToInt32(requestBody);

                if (id != 0 && id <= Users.Count)
                {

                    Console.WriteLine($"Received User: Id={id}");
                    if (Users.SingleOrDefault((u => u.Id == id)) is not null)
                    {
                        int index = Users.FindIndex(u => u.Id == id);
                        if (index != -1)
                        {
                            Users.Remove(Users[index]);

                            for (int i = index; i < Users.Count; i++)
                            {
                                var a = i;
                                Users[i].Id = a + 1;
                            }
                            reader.Close();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("User index not founed");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                }
                else
                    throw new ArgumentNullException("user id not founded");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing request: {ex.Message}");
        }
    }


    private async void HandleGetRequest(HttpListenerContext context)
    {
        string combinedString = string.Join("_", Users);
        await Console.Out.WriteLineAsync(combinedString);
        await Console.Out.WriteLineAsync("Users Count: " + Users.Count.ToString());

        // Respond message to the Client
        HttpListenerResponse responseToClient = context.Response;
        responseToClient.ContentType = "application/json";
        responseToClient.ContentEncoding = System.Text.Encoding.UTF8;


        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(combinedString);
        responseToClient.ContentLength64 = buffer.Length;

        await responseToClient.OutputStream.WriteAsync(buffer, 0, buffer.Length);
        responseToClient.OutputStream.Close();

        Console.WriteLine("Response message to the Client sent at " + DateTime.Now.ToLongTimeString() + '\n');

    }

}

