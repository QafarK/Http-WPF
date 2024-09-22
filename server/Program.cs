// See https://aka.ms/new-console-template for more information
using server;
using System;
using System.Net;

Console.WriteLine("Hello, World!");
new WebHost(27001).Start();


//MyDelegate obj = HandleRequest;
//obj += HandleRequest1;

//string firstMethodName = obj.GetInvocationList()[1].Method.Name;
//Console.WriteLine(obj.GetInvocationList()[0].Method.Name.);
////Console.WriteLine(obj.Method.Name[0] +'+'+ obj.Method.Name[1]);


//void HandleRequest(HttpListenerContext context)
//{
//    throw new NotImplementedException();
//}


//void HandleRequest1(HttpListenerContext context)
//{
//    throw new NotImplementedException();
//}

//delegate void MyDelegate(HttpListenerContext context);