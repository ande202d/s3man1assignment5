using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ass5tcpServer
{
    public class Server
    {
        private static List<Book> Books = new List<Book>(){
            new Book("Harry Potter", "JK Rowling", 666, "abcdefghijkl1"),
            new Book("Moby Dick", "Big Dick 69", 69, "abcdefghijkl2"),
            new Book("Anders Selvbiografi", "Anders Hansen", 123, "abcdefghijkl3")
        };

        public void Start()
        {
            //IP configuration
            IPAddress ipa = IPAddress.Parse("127.0.0.1");
            //ipa = IPAddress.Parse("192.168.24.237");
            TcpListener tcp = new TcpListener(ipa, 4646);
            
            //Starting Server and sending the accepted client to a "DoClient"
            tcp.Start();
            Console.WriteLine("Server Started");

            while (true)
            {
                Task.Run(() =>
                {
                    TcpClient tempSocket = tcp.AcceptTcpClient();
                    EndPoint clientIP = tempSocket.Client.RemoteEndPoint;
                    Console.WriteLine(clientIP + ": CONNECTED");

                    DoClient(tempSocket);

                    if (!tempSocket.Connected) Console.WriteLine(clientIP + ": DISCONNECTED");
                    tempSocket.Close();
                });
            }

            tcp.Stop();
        }

        public void DoClient(TcpClient socket)
        {
            NetworkStream ns = socket.GetStream();

            StreamReader sr = new StreamReader(ns);
            StreamWriter sw = new StreamWriter(ns);
            sw.AutoFlush = true;

            string message;

            while (true)
            {
                try
                {
                    message = sr.ReadLine();
                    if (string.IsNullOrWhiteSpace(message)) break;

                    List<string> stuffToPrint = Answer(message);
                    if (stuffToPrint.Count > 0)
                    {
                        foreach (string s in stuffToPrint)
                        {
                            sw.WriteLine(s);
                        }
                    }
                    //sw.WriteLine(Answer(message));
                }
                catch (IOException e)
                {
                    break;
                }
            }

            ns.Close();
        }

        public List<string> Answer(string s)
        {
            List<string> listToReturn = new List<string>();
            string[] stringArray = s.Split(' ');

            //GETALL
            if (stringArray[0] == "GetAll")
            {
                listToReturn.Add("Getting All Books:");
                foreach (Book b in Books)
                {
                    listToReturn.Add(JsonSerializer.Serialize(b));
                }
                listToReturn.Add("\n");
            }
            //GET
            else if (stringArray[0] == "Get")
            {
                if (stringArray.Length > 1)
                {
                    listToReturn.Add($"Trying to Get {stringArray[1]}");
                    listToReturn.Add(JsonSerializer.Serialize(Books.Find(i => i.ISBN13 == stringArray[1])));
                    listToReturn.Add("\n");
                }
                else listToReturn.Add("Please specify an ESBN13");
            }
            //SAVE
            else if (stringArray[0] == "Save")
            {
                if (stringArray.Length > 1)
                {
                    listToReturn.Add("Trying to save the JSON object to the list");
                    Book b = null;
                    string stringToDeserialize = "";
                    try
                    {
                        for (int i = 0; i < stringArray.Length; i++)
                        {
                            if (i >= 1 && i <= (stringArray.Length - 1))
                            {
                                stringToDeserialize += (stringArray[i] + " ");
                            } 
                            else if (i == stringArray.Length - 1)
                            {
                                stringToDeserialize += (stringArray[i]);
                            }
                        }
                        b = JsonSerializer.Deserialize<Book>(stringToDeserialize);
                    }
                    catch (Exception e)
                    {
                        listToReturn.Add("Something went wrong, error message:");
                        listToReturn.Add(e.Message);
                    }

                    if (b != null)
                    {
                        Books.Add(b);
                        listToReturn.Add("Successfully converted and added to the list");
                    }
                }
            }

            return listToReturn;
        }
    }
}
