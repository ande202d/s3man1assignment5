using System;

namespace ass5tcpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Server s = new Server();
            s.Start();
            Console.ReadKey();
        }
    }
}
