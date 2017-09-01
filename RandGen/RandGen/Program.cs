using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.IO;
using System;

namespace RandGen
{
    class Program
    {
        static readonly char[] Letters = "qwertzuiopasdfghjklyxcvbnmöäüQWERTZUIOPASDFGHJKLYXCVBNMÖÄÜ1234567890ß".ToCharArray();
        static byte[] buffer = new byte[1024];
        static List<FileStream> FStreams = new List<FileStream>();
        static List<Socket> Connections = new List<Socket>();
        static List<Thread> LisThreads = new List<Thread>();
        static bool ipv6 = false;

        static void Main(string[] args)
        {
            bool help=false,Silent=false;
            int seed = -1;
            List<int> ServerPorts = new List<int>();
            List<string> Files = new List<string>();
            List<IPEndPoint> Sockets = new List<IPEndPoint>();
            List<DnsEndPoint> _Dns = new List<DnsEndPoint>();

            void Write(string message)
            {
                if (!Silent)
                    Console.WriteLine(message);
            }

            /*
             * Silent       = --silent
             * help         = -h, --help, /?
             * seeded       = -s [int], --seed [int]
             * fileoutput   = -f [FILE], --file [FILE]
             * Socketoutput = -o [IP:Port], --socket [IP:Port]
             * Serveroutput = -m [Port], --host [Port], --server [Port]
             * Dnsoutput    = -d [Host:Port], --dns [Host:Port]
             * Buffer       = -b [int], --buffersize [int]
             * useIpv6      = -6, --ipv6, --use-ipv6
             */
            for(int x = 0; x < args.Length; x++)
            {
                if (args[x] == "--silent")
                {
                    Silent = true;
                }
                else if (args[x] == "-h" || args[x] == "--help")
                {
                    help = true;
                    break;
                }
                else if (args[x] == "-s" || args[x] == "--seed")
                {
                    if (seed == -1)
                    {
                        x++;
                        int.TryParse(args[x], out seed);
                        if (seed == -1)
                            Write($"Ignoring Seed due to invalid value 'Value:{args[x]} Pos:{x}'");
                    }
                    else
                    {
                        Write($"Ignoring Seed due to seed beeing set 'pos{x}'");
                        x++;
                    }
                }
                else if (args[x] == "-f" || args[x] == "--file")
                {
                    x++;
                    Files.Add(args[x]);
                }
                else if (args[x] == "-o" || args[x] == "--socket")
                {
                    x++;
                    string tmp = args[x];
                    if (tmp.Contains(":"))
                    {
                        int port = -1;
                        IPAddress ip = IPAddress.None;

                        int.TryParse(tmp.Substring(tmp.IndexOf(':') + 1), out port);
                        IPAddress.TryParse(tmp.Substring(0, tmp.IndexOf(":")), out ip);
                        
                        if(port <= 0 || port > 65535 || ip == IPAddress.None)
                        {
                            Write($"Ignoring socket due to invalid value 'Value:{tmp} Pos:{port}'");
                        }
                        else
                        {
                            Sockets.Add(new IPEndPoint(ip, port));
                        }
                    }
                    else
                        Write($"Ignoring Socket due to invalid value 'Value:{tmp} Pos:{x}'");

                }
                else if (args[x] == "-m" || args[x] == "--server")
                {
                    x++;
                    int port = -1;
                    int.TryParse(args[x], out port);

                    if(port <= 0 || port > 65535)
                    {
                        Write($"Ignoring server due to invalid value 'Value:{port} Pos:{x}'");
                    }
                    else if(ServerPorts.Contains(port))
                    {
                        Write($"Ignoring server due to duplicat value 'Value:{port} Pos:{x}'");
                    }
                    else
                    {
                        ServerPorts.Add(port);
                    }
                }
                else if (args[x] == "-d" || args[x] == "--dns")
                {
                    x++;
                    string host="";
                    int port=-1;
                    string tmp = args[x];
                    IPAddress[] ips;

                    int.TryParse(tmp.Substring(tmp.IndexOf(':') + 1), out port);
                    host = tmp.Substring(0, tmp.IndexOf(":"));
                    try
                    {
                        ips = Dns.GetHostAddresses(host);
                        if(ips.Length == 0)
                        {
                            Write($"Ignoring dns due to having no ips on host");
                        }
                    }
                    catch
                    {
                        Write($"Ignoring dns due to invalid host 'Value:{host} Pos:{x}'");
                    }

                    if(port <= 0 || port > 65535)
                    {
                        Write($"Ignoring dns due to invalid port 'Value:{port} Pos:{x}'");
                    }
                    else
                    {
                        _Dns.Add(new DnsEndPoint(host, port));
                        
                    }
                    
                }
                else if (args[x] == "-b" || args[x] == "--buffersize")
                {
                    int size=-1;
                    int.TryParse(args[x], out size);
                    if(size < 0)
                    {
                        Write($"Ignoring buffersize due to invalid value 'Value:{args[x]} Pos:{x}'");
                    }
                    else
                    {
                        buffer = new byte[size];
                    }
                }
                else if (args[x] == "-6" || args[x] == "--ipv6" || args[x] == "--use-ipv6")
                {
                    ipv6 = true;
                }
                else
                {
                    Write($"Ignoring invalid arg 'Arg:{args[x]} Pos:{x}'");
                }
            }

            if(help || args.Length == 0)
            {
                Console.WriteLine();
                Console.WriteLine("#############[Help]#############");
                Console.WriteLine();
                Console.WriteLine("Args:");
                Console.WriteLine(" -h  --help                 displays this text");
                Console.WriteLine(" -s  --seed   [int]         set the seed");
                Console.WriteLine(" --silent                   disables console output");
                Console.WriteLine(" -f  --file   [File]        add file to outputs, write to file");
                Console.WriteLine(" -o  --socket [IP]:[Port]   send output to IP:Port");
                Console.WriteLine(" -m  --host --server [Port] open port for more connections");
                Console.WriteLine(" -d  --dns [Host]:[Port]    send output to domain");
                Console.WriteLine(" -b  --buffer [BufferSize]  change buffer size");
                Console.WriteLine(" -6  --ipv6 --use-ipv6      use ipv6 on connection and servers");
                Console.WriteLine();
                Console.WriteLine("Examples:");
                Console.WriteLine("RandGen -s 12512 -f log -o 0.0.0.0:7912");
                Console.WriteLine("RandGen -s 124 -b 2048 -6 -d www.example.com:90");
                Console.WriteLine("RandGen -m 251 -o 0.0.0.0:894");
                Console.WriteLine();
                Console.WriteLine("Small Tips:");
                Console.WriteLine("if you put somthing invalid in the args it will be ignored.");
                Console.WriteLine("if a client disconnects randgen wont log it.");
                Console.WriteLine("silent wont disable help output");
                Console.WriteLine("when debugging input args randgen will output the value and pos of the ignored input");
                Console.WriteLine("pos is the amount of spaces to get to the value");
                Console.WriteLine("if no args are given help text will be displayed");
                Console.WriteLine();
            }
            else
            {
                //Validate Files
                for(int x = 0;  x < Files.Count; x++)
                {
                    if(File.Exists(Files[x]))
                    {
                        try
                        {
                            FStreams.Add(new FileStream(Files[x], FileMode.Append));
                            Write($"File '{Files[x]}' allready exists and will be appended");
                        }
                        catch
                        {
                            Write($"Failed to open file 'File:{Files[x]}' Ignoring and continuing");
                        }
                    }
                    else
                    {
                        FStreams.Add(new FileStream(Files[x], FileMode.CreateNew));
                    }
                }

                //Create Threads for Listening
                for(int x = 0; x < ServerPorts.Count; x++)
                {
                    LisThreads.Add(new Thread(new ParameterizedThreadStart(ListenOn)));
                }

                //connect Dns
                for (int x = 0; x < _Dns.Count; x++)
                {
                    Socket con = new Socket(_Dns[x].AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    try
                    {
                        con.Connect(_Dns[x]);
                        con.SendTimeout = 500;
                        Connections.Add(con);
                    }
                    catch
                    {
                        Write($"Failed to connect to 'Host:{_Dns[x].Host} Port:{_Dns[x].Port}'");
                    }
                }

                //connect socket
                for(int x = 0; x < Sockets.Count; x++)
                {
                    Socket Soc = new Socket((ipv6) ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    try
                    {
                        Soc.Connect(Sockets[x]);
                        Soc.SendTimeout = 500;
                        Connections.Add(Soc);
                    }
                    catch
                    {
                        Write($"Failed to connect to 'Address:{Sockets[x].Address} Port:{Sockets[x].Port}'");
                    }
                }

                //Start Listening Threads
                for (int x = 0; x < LisThreads.Count; x++)
                {
                    LisThreads[x].Start(ServerPorts[x]);
                }

                //Start RandGen
                if (seed != -1)
                    Start(new Random(seed));
                else
                    Start(new Random());
            }
        }

        static void Start(Random rnd)
        {
            while (true)
            {
                rnd.NextBytes(buffer);

                for (int x = 0; x < Connections.Count; x++)
                {
                    try
                    {
                        Connections[x].Send(buffer);
                    }
                    catch
                    {
                        Connections.RemoveAt(x);
                    }
                }
                Thread.Sleep(15);
            }
        }

        static void ListenOn(object Port)
        {
            Socket Lis = new Socket((ipv6) ? AddressFamily.InterNetworkV6 : (AddressFamily.InterNetwork), SocketType.Stream, ProtocolType.Tcp);
            Lis.Bind(new IPEndPoint(IPAddress.Any, (int)Port));
            Lis.Listen(20);

            while (true)
            {
                Socket x = Lis.Accept();
                x.SendTimeout = 500;
                Connections.Add(x);
            }//add more clients
            
        }
    }
}
