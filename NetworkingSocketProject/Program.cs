using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;

class Server
{
    static void Main()
    {
        string filepath = "../../../users.txt";
        Dictionary<string, string> userData = new Dictionary<string, string>();
        string currentUser = "none";


        if (File.Exists(filepath))
        {
            foreach (string item in File.ReadAllLines(filepath))
            {
                string fixedString=item.Replace("(", "").Replace(")", "").Replace(" ","");
                string userName=fixedString.Split(",")[0];
                string userPassword = fixedString.Split(",")[1];
                userData.Add(userName, userPassword);

               // Console.WriteLine(fixedString+"Username=: "+userName+" Password=: "+userPassword);
            }

        }
        else 
        {
            Console.WriteLine("File does not exist,check your filepath, you are in :"+Directory.GetCurrentDirectory()); 
        }
        




        int port = 11627; //port had to be shorter
        TcpListener listener = new TcpListener(IPAddress.Loopback, port);
        try
        {


            listener.Start();
            Console.WriteLine("Server is listening...");
            // Accept client connection
            TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine("Client connected!");
            // Handle data
            NetworkStream stream = client.GetStream();

            while (true)
            {
                try
                {

                        byte[] buffer = new byte[1024];
                        int bytesRead = stream.Read(buffer, 0, buffer.Length);
                        if(bytesRead == 0 )
                        {
                    
                            Console.WriteLine("Client disconnected");
                            //break;
                        }
                        string receivedMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                        

                        if(receivedMessage == "shutdown")
                        {
                            Console.WriteLine("Shutdown recieved, server shutting down");
                            break;
                        }

                        if (string.IsNullOrEmpty(receivedMessage.Trim() )  )
                        {
                            Console.WriteLine("Empty msg from client");
                            string response = "Empty msg from client";
                            byte[] responseBytes = Encoding.ASCII.GetBytes(response);
                            stream.Write(responseBytes, 0, responseBytes.Length);
                        }
                        else
                        {
                            Console.WriteLine($"Received: {receivedMessage}");

                            // Respond to client
                          /*  string response = "Message received!";
                            byte[] responseBytes = Encoding.ASCII.GetBytes(response);
                            stream.Write(responseBytes, 0, responseBytes.Length);
                        */}
                    //response pattern not defined by document
                    //standard response pattern for output will be as follows
                    //"Username" "requestType" "granted/denied"
                    //example
                    //george login granted
                    //george newuser denied



                    //for login, pattern should be as follows
                    //"login" "userName" "userPassword"
                    string[] receivedMessageTokens=receivedMessage.Split(" ");
                    Console.WriteLine(receivedMessageTokens.Length);
                    for (int i = 0; i < receivedMessageTokens.Length; i++)
                    {
                        receivedMessageTokens[i] = receivedMessageTokens[i].Trim();
                    }
                    //segment that validates login
                    if (receivedMessageTokens.Length == 3 && receivedMessageTokens[0] == "login" && userData.GetValueOrDefault(receivedMessageTokens[1]) == receivedMessageTokens[2])
                    { //checks if login message is of right length, checks if message starts with login, uses 2nd token as key to look in dictionary
                      //if retrived value is equal to supplied password, then login is successful
                        Console.WriteLine($"Login Sucessful for: {receivedMessage}");
                        currentUser = receivedMessageTokens[1];
                        // Respond to client
                        string response = receivedMessageTokens[1] + " " + "login granted";
                        byte[] responseBytes = Encoding.ASCII.GetBytes(response);
                        stream.Write(responseBytes, 0, responseBytes.Length);
                    }
                    else if(receivedMessageTokens.Length == 3 && receivedMessageTokens[0] == "login")
                    { 
                        Console.WriteLine($"Login NOT sucessful for: {receivedMessage}");
                        //currentUser = receivedMessageTokens[1];
                        // Respond to client
                        string response = receivedMessageTokens[1] + " " + "login denied";
                        byte[] responseBytes = Encoding.ASCII.GetBytes(response);
                        stream.Write(responseBytes, 0, responseBytes.Length);
                    }

                    //segment that validates newuser
                    //formatt is as follows
                    //newuser user pass
                    if (receivedMessageTokens.Length == 3 && receivedMessageTokens[0] == "newuser" && !userData.ContainsKey(receivedMessageTokens[1]) )
                    { //checks if newuser message is of right length, checks if message starts with newuser, uses 2nd token as key to look in dictionary
                      //if retrived value is already contained in discionary, thats bad, and we cant make a new user
                        Console.WriteLine($"newuser Sucessful for: {receivedMessage}");

                        //currentUser = receivedMessageTokens[1];
                        userData.Add(receivedMessageTokens[1], receivedMessageTokens[2]);
                        File.AppendAllText(filepath,"\n"+"(" + receivedMessageTokens[1] + ", " + receivedMessageTokens[2]+")"   );
                        // Respond to client with good news
                        string response = receivedMessageTokens[1] + " " + "newuser granted";
                        byte[] responseBytes = Encoding.ASCII.GetBytes(response);
                        stream.Write(responseBytes, 0, responseBytes.Length);
                    }
                    else if(receivedMessageTokens.Length == 3 && receivedMessageTokens[0] == "newuser")
                    {
                        Console.WriteLine($"newuser NOT sucessful for: {receivedMessage}");
                        //currentUser = receivedMessageTokens[1];
                        // Respond to client with bad news :(
                        string response = receivedMessageTokens[1] + " " + "newuser denied";
                        byte[] responseBytes = Encoding.ASCII.GetBytes(response);
                        stream.Write(responseBytes, 0, responseBytes.Length);
                    }
                    
                    
                    //validate send message 
                    if (receivedMessageTokens.Length >= 2 && receivedMessageTokens[0] == "send")
                    { //checks if send message is of right length, checks if message starts with send
                        Console.WriteLine($"send Sucessful for: {receivedMessage}");

                        //currentUser = receivedMessageTokens[1];
                        // Respond to client with good news

                        string response ="send "+currentUser+" "+receivedMessage;
                        //string response= ">"+currentUser+": " + receivedMessage.Replace(currentUser,"").Remove(receivedMessage.IndexOf("send"),"send".Length).Trim();
                        byte[] responseBytes = Encoding.ASCII.GetBytes(response);
                        stream.Write(responseBytes, 0, responseBytes.Length);
                    }
                    else if (receivedMessageTokens.Length >=2 && receivedMessageTokens[0] == "send")
                    {
                        Console.WriteLine($"send NOT sucessful for: {receivedMessage}");
                        //currentUser = receivedMessageTokens[1];
                        // Respond to client with bad news :(
                        string response = receivedMessageTokens[1] + " " + "newuser denied";
                        byte[] responseBytes = Encoding.ASCII.GetBytes(response);
                        stream.Write(responseBytes, 0, responseBytes.Length);
                    }


                    





                }
                catch (Exception e)
                {
                    Console.WriteLine("Client Disconnected Unexpectedly Error:" );
                    Console.WriteLine("Listening");
                    client = listener.AcceptTcpClient();
                    Console.WriteLine("Client connected!");
                    // Handle data
                    stream = client.GetStream();
                    //throw;
                }



            }
            Console.WriteLine("Shutting down server");
            client.Close(); // Close the client connection
        }
        catch (Exception e)
        {

            Console.Write("Error, exception" + e);
        }
    }
}
