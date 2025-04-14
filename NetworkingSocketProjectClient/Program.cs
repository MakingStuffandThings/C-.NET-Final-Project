using System;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

class Client
{
    static void Main()
    {
        string server = "127.0.0.1"; // Loopback address
        int port = 11627; // Same port as server my student ID is too long for this api so i shortened it
        bool isLoggedIn=false;
        string currentUser="none";


        string userNamePattern= "^[a-zA-Z0-9]{3,32}$";//Regex for username, must be alphanum upper or lower, must be bewtwen 3-32 chars
        string userPasswordPattern= "^[a-zA-Z0-9]{4,8}$";//Regex for password  must be alphanum upper or lower, must be bewtwen 4-8 chars
        string messagePattern = "^[a-zA-Z0-9]{1,256}";//Regex for message.  must be alphanum upper or lower, must be bewtwen 1-265 chars



        try
        {
            using (TcpClient client = new TcpClient(server, port))
            {
                NetworkStream stream = client.GetStream();
               // Console.WriteLine("Connected to server!");
                string control = "enter";


                Console.WriteLine("My chat room client. Version One.\n");

              

                   
                   
                while (control != "logout")
                {
                    Console.Write(">");
                    control = Console.ReadLine();
                    //handle logic here to validate user input
                    string[] response=control.Split(' ');


                    bool isValidResponse=false;

                    while (!isValidResponse)
                    {   //login client input validation
                        if (response.Length == 3 && !isLoggedIn && response[0] == "login" && Regex.IsMatch(response[1], userNamePattern) && Regex.IsMatch(response[2], userPasswordPattern))
                        {
                           // Console.WriteLine("Username and password is in valid formatt");
                            break;
                        }
                        else { //Console.WriteLine("Username and Password ARE NOT in valid formatt and loged in is"+isLoggedIn); 
                        }

                        //newuser client input validation
                        if (response.Length == 3 && !isLoggedIn && response[0] == "newuser" && Regex.IsMatch(response[1], userNamePattern) && Regex.IsMatch(response[2], userPasswordPattern))
                        {
                           // Console.WriteLine(" NEW Username and password is in valid formatt");
                            break;
                        }
                        else { //Console.WriteLine(" NEW Username and Password ARE NOT in valid formattand loged in is" + isLoggedIn);
                              }
                        

                        //send
                        if (response.Length >=2 && isLoggedIn && response[0] == "send"  && Regex.IsMatch(response[1],messagePattern) )
                        {
                            //Console.WriteLine("sending your msg");
                            break;
                        }
                        else if(!isLoggedIn)
                        {
                            Console.WriteLine("Denied. Please login first"); 
                            //Console.WriteLine("Didnt send your msgand loged in is" + isLoggedIn);
                        
                        }

                        if (control.Trim() == "logout")
                        { break; }








                        Console.Write(">");
                        control = Console.ReadLine();
                        response = control.Split(' ');

                        

                    }

                    if (control.Trim() == "logout")
                    { break; }











                    control = control + "\n";
                    // Send message to server
                    string message = control;
                    byte[] data = Encoding.ASCII.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                   // Console.WriteLine("Sent: " + message);

                    // Receive response from server
                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string serverResponse = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    
                    
                    
                    
                    
                    //Console.WriteLine(serverResponse);
                    
                    
                    
                    
                    
                    control = control.Trim();
                    string[] serverResponseTokens = serverResponse.Split(' ');

                    //data sent back from server, handle for login, and new user verification
                    //response pattern not defined by document
                    //standard response pattern will be as follows
                    //"Username" "requestType" "granted/denied"
                    //example
                    //george login granted
                    //george newuser denied
                    //no response from server about message denial request as client handles verification for valid send or not
                    if (serverResponseTokens.Length == 3 && serverResponseTokens[1] == "login" && serverResponseTokens[2] == "granted")
                    {//if server validates login, set islogged in to true
                        isLoggedIn=true;
                        currentUser = serverResponseTokens[0];
                        //Console.WriteLine("Welcome: " + serverResponseTokens[0] + " You have successfully logged in"); 
                        Console.WriteLine("> login confirmed");
                    }
                    else if(serverResponseTokens.Length == 3 && serverResponseTokens[1] == "login" && serverResponseTokens[2] == "denied")
                    {
                        Console.WriteLine("> Denied. User name or password incorrect.");
                    }

                    if (serverResponseTokens.Length == 3 && serverResponseTokens[1] == "newuser" && serverResponseTokens[2] == "granted")
                    {//if server validates newuser
                        //Console.WriteLine("Welcome: " + serverResponseTokens[0] + " You have successfully made a new account, please log in");
                        Console.WriteLine("> New user account created. Please login.");
                    }
                    else if (serverResponseTokens.Length == 3 && serverResponseTokens[1] == "newuser" && serverResponseTokens[2] == "denied")
                    {
                        Console.WriteLine("> Denied. User account already exists.");
                    }
                    
                                    
                    if(serverResponseTokens.Length >=3 && serverResponseTokens[0]=="send" && serverResponseTokens[1]==currentUser)
                    {


                        Console.WriteLine(">"+string.Join(" ", serverResponseTokens[1])+": "+string.Join(" ",serverResponseTokens[3..]).TrimEnd());
                        //Console.WriteLine("REACHED HERE");
                        //Console.WriteLine(serverResponse.Remove(serverResponse.IndexOf("send"), "send".Length).Trim().Remove(server.IndexOf(currentUser),currentUser.Length));
                    }
                    //Console.WriteLine("ASDASDASDASD   " + serverResponseTokens[0]+" current user:"+currentUser);






                }
                Console.WriteLine(">" + currentUser + " left");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: " + e.Message);
        }
    }
}
