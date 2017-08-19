using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using System;
using System.Collections.Generic;
using BDLib.Text;
using System.Text;

namespace BDLib.Net.Http
{

    public struct HttpConnection
    {
        internal TcpClient Soc;
        internal int Index;
        internal string[] _Headers;
        internal string _Request;

        public string[] Headers { get { return _Headers; } }
        public string   Request { get { return _Request; } }

    }

    public enum HttpStatusCodes
    {
        none                            = 0,
        Continue                        = 100,
        SwitchingProtocols              = 101,
        Processing                      = 102,

        OK                              = 200,
        Created                         = 201,
        Accepted                        = 202,
        NonAuthoritativeInformation     = 203,
        NoContent                       = 204,
        ResetContent                    = 205,
        PartialContent                  = 206,
        MultiStatus                     = 207,
        AlreadyReported                 = 208,
        IMUsed                          = 226,

        MultipleChoices                 = 300,
        MovedPermanently                = 301,
        Found                           = 302,
        SeeOther                        = 303,
        NotModified                     = 304,
        UseProxy                        = 305,
        SwitchProxy                     = 306,
        TemporaryRedirect               = 307,
        PermanentRedirect               = 308,

        BadRequest                      = 400,
        Unauthorized                    = 401,
        PaymentRequired                 = 402,
        Forbidden                       = 403,
        NotFound                        = 404,
        MethodNotAllowed                = 405,
        NotAcceptable                   = 406,
        ProxyAuthenticationRequired     = 407,
        RequestTimeout                  = 408,
        Conflict                        = 409,
        Gone                            = 410,
        LengthRequired                  = 411,
        PreconditionFailed              = 412,
        PayloadTooLarge                 = 413,
        URITooLong                      = 414,
        UnsupportedMediaType            = 415,
        RangeNotSatisfiable             = 416,
        ExpectationFailed               = 417,
        ImATeapot                       = 418,
        MisdirectedRequest              = 421,
        UnprocessableEntity             = 422,
        Locked                          = 423,
        FailedDependency                = 424,
        UpgradeRequired                 = 426,
        PreconditionRequired            = 428,
        TooManyRequests                 = 429,
        RequestHeaderFieldsTooLarge     = 431,
        UnavailableForLegalReasons      = 451,

        InternalServerError             = 500,
        NotImplemented                  = 501,
        BadGateway                      = 502,
        ServiceUnavailable              = 503,
        GatewayTimeout                  = 504,
        HTTPVersionNotSupported         = 505,
        VariantAlsoNegotiates           = 506,
        InsufficientStorage             = 507,
        LoopDetected                    = 508,
        NotExtended                     = 510,
        NetworkAuthenticationRequired   = 511,
    }

    public struct HttpResponse
    {
        public HttpStatusCodes StatusCode;
        public string[] Headers;
        public byte[] Data;
        public string DataType;
    }

    public delegate void HttpConAcceptEvent(object sender, HttpConnection NewCon);

    public class HttpServer : IDisposable
    {
        //settings
        public bool AutoClose = false;
        public Encoding ResponseEncoding;

        private TcpListener[] Lis;
        private Thread THR;
        private bool Running;
        private bool FirstRun;
        private event HttpConAcceptEvent RequestEvent;
        private int RequestEvents = 0;
        private HttpConnection[] Socs;

        private void OnConnectionEvent(HttpConnection NewCon)
        {
            if (RequestEvent != null)
                RequestEvent(this, NewCon);
            else
                throw new InvalidOperationException("CODE FAILURE Class='Net\\Http\\HttpServer' Function='OnConnectionEvent' ErrorType='ValueIsNULL and should never be NULL when Function is CALLED' Please Message the dev team becouse this is a very important problem");
        }


        public HttpServer()
        {

            Lis = new TcpListener[2];//one on 80 and one on 8080

            Lis[0] = new TcpListener(IPAddress.Any, 80);
            Lis[1] = new TcpListener(IPAddress.Any, 8080);

            Running = false;
            FirstRun = true;
            Socs = new HttpConnection[20];
            THR = new Thread(new ThreadStart(ListenThread));
        }

        public void Bind(string ip)
        {
            if (THR.IsAlive)
                throw new InvalidOperationException("this must be run beffore you start listening");//stupid coder -_-

            if (IPAddress.TryParse(ip, out IPAddress x))
            {
                Lis[0] = new TcpListener(x, 80);
                Lis[1] = new TcpListener(x, 8080);//set endpoint
            }
        }

        public void Start()
        {
            Start(20);
        }//alias for 'Start(20);'
        public void Start(int Backlog)
        {
            if (!THR.IsAlive)
            {
                Socs = new HttpConnection[Backlog];
                Running = true;
                if (FirstRun)
                {
                    Lis[0].Start(Backlog);//set backlog
                    Lis[1].Start(Backlog);
                }
                THR.Start();
            }
            else throw new InvalidOperationException("there must be at least one Request Event to run (more can be added later)");
        }

        public void Stop()
        {
            Running = false;
        }

        private void ListenThread()
        {
            do//run thread for port 80
            {
                if(Lis[0].Pending())
                    EventText(Lis[0]);
            } while (Running);

            do//run thread for port 8080
            {
                if (Lis[1].Pending())
                    EventText(Lis[1]);
            } while (Running);

            while (Running) Thread.Sleep(100);//wait till Running is false
            //IE wait till 'Stop' is ran
        }

        public void Response(HttpConnection con, HttpResponse Res)
        {
            string output = "";
            output+= "HTTP/1.1 "+GetStatusCode(Res.StatusCode)+"\r\n";
            if(Res.Headers.Length > 0)
                for (int x = 0; x < Res.Headers.Length; x++)//        join headers together
                    output+= Res.Headers[x]+"\r\n";

            output += $"Date: {DateTime.Today.ToShortDateString()} - {DateTime.Now.ToShortTimeString()}\r\n"+
                      $"Server: BDServer\r\n"+
                      $"Content-Length: {Res.Data.Length}\r\n"+
                      $"Connection: Closed\r\n"+
                      $"Content-Type: {Res.DataType}\r\n";


            output += "\r\n";

            byte[] Data = new byte[ResponseEncoding.GetByteCount(output) + Res.Data.Length];//create new container

            ResponseEncoding.GetBytes(output).CopyTo(Data, 0);//combine data together
            Res.Data.CopyTo(Data, ResponseEncoding.GetByteCount(output));//data must be sent as one for most web clients to function properly
            if(con.Soc.Connected)
                con.Soc.Client.Send(Data);//send data
        }

        public static string GetStatusCode(HttpStatusCodes code)
        {
            switch (code)
            {
                //-------------------------------------------------------------------------------------------1XX
                #region StatusCodes 1XX

                case (HttpStatusCodes.Continue):
                    return "100 Continue";

                case (HttpStatusCodes.SwitchingProtocols):
                    return "101 Switching Protocols";

                case (HttpStatusCodes.Processing):
                    return "102 Processing";

                #endregion
                //-------------------------------------------------------------------------------------------2XX
                #region StatusCodes 2XX

                case (HttpStatusCodes.OK):
                    return "200 OK";

                case (HttpStatusCodes.Created):
                    return "201 Created";

                case (HttpStatusCodes.Accepted):
                    return "202 Accepted";

                case (HttpStatusCodes.NonAuthoritativeInformation):
                    return "203 Non-Authoritative Information";

                case (HttpStatusCodes.NoContent):
                    return "204 No Content";

                case (HttpStatusCodes.ResetContent):
                    return "205 Reset Content";

                case (HttpStatusCodes.PartialContent):
                    return "206 Partial Content";

                case (HttpStatusCodes.MultiStatus):
                    return "207 Multi Status";

                case (HttpStatusCodes.AlreadyReported):
                    return "208 Already Reported";

                case (HttpStatusCodes.IMUsed):
                    return "226 IM Used";

                #endregion
                //-------------------------------------------------------------------------------------------3XX
                #region StatusCodes 3XX

                case (HttpStatusCodes.MultipleChoices):
                    return "300 Multiple Choices";

                case (HttpStatusCodes.MovedPermanently):
                    return "301 Moved Permanently";

                case (HttpStatusCodes.Found):
                    return "302 Found";

                case (HttpStatusCodes.SeeOther):
                    return "303 See Other";

                case (HttpStatusCodes.NotModified):
                    return "304 Not Modified";

                case (HttpStatusCodes.UseProxy):
                    return "305 Use Proxy";

                case (HttpStatusCodes.SwitchProxy):
                    return "306 Switch Proxy";

                case (HttpStatusCodes.TemporaryRedirect):
                    return "307 Temporary Redirect";

                case (HttpStatusCodes.PermanentRedirect):
                    return "308 Permanent Redirect";

                #endregion
                //-------------------------------------------------------------------------------------------4XX
                #region StatusCodes 4XX
                case (HttpStatusCodes.BadRequest):
                    return "400 Bad Request";

                case (HttpStatusCodes.Unauthorized):
                    return "401 Unauthorized";

                case (HttpStatusCodes.PaymentRequired):
                    return "402 Payment Required";

                case (HttpStatusCodes.Forbidden):
                    return "403 Forbidden";

                case (HttpStatusCodes.NotFound):
                    return "404 Not Found";

                case (HttpStatusCodes.MethodNotAllowed):
                    return "405 Method Not Allowed";

                case (HttpStatusCodes.NotAcceptable):
                    return "406 Not Acceptable";

                case (HttpStatusCodes.ProxyAuthenticationRequired):
                    return "407 Proxy Authentication Required";

                case (HttpStatusCodes.RequestTimeout):
                    return "408 Request Timeout";

                case (HttpStatusCodes.Conflict):
                    return "409 Conflict";

                case (HttpStatusCodes.Gone):
                    return "410 Gone";

                case (HttpStatusCodes.LengthRequired):
                    return "411 Length Required";

                case (HttpStatusCodes.PreconditionFailed):
                    return "412 Precondition Failed";

                case (HttpStatusCodes.PayloadTooLarge):
                    return "413 Payload Too Large";

                case (HttpStatusCodes.URITooLong):
                    return "414 URI Too Long";

                case (HttpStatusCodes.UnsupportedMediaType):
                    return "415 Unsupported Media Type";

                case (HttpStatusCodes.RangeNotSatisfiable):
                    return "416 Range Not Satisfiable";

                case (HttpStatusCodes.ExpectationFailed):
                    return "417 Expectation Failed";

                case (HttpStatusCodes.ImATeapot):
                    return "418 ImATeapot";

                case (HttpStatusCodes.MisdirectedRequest):
                    return "421 Misdirected Request";

                case (HttpStatusCodes.UnprocessableEntity):
                    return "422 Unprocessable Entity";

                case (HttpStatusCodes.Locked):
                    return "423 Locked";

                case (HttpStatusCodes.FailedDependency):
                    return "424 Failed Dependency";

                case (HttpStatusCodes.UpgradeRequired):
                    return "426 Upgrade Required";

                case (HttpStatusCodes.PreconditionRequired):
                    return "428 Precondition Required";

                case (HttpStatusCodes.TooManyRequests):
                    return "429 Too Many Requests";

                case (HttpStatusCodes.RequestHeaderFieldsTooLarge):
                    return "431 Request Header Fields Too Large";

                case (HttpStatusCodes.UnavailableForLegalReasons):
                    return "451 Unavailable For Legal Reasons";
                #endregion
                //-------------------------------------------------------------------------------------------5XX
                #region StatusCodes 5XX
                case (HttpStatusCodes.InternalServerError):
                    return "500 Internal Server Error";

                case (HttpStatusCodes.NotImplemented):
                    return "501 Not Implemented";

                case (HttpStatusCodes.BadGateway):
                    return "502 Bad Gateway";

                case (HttpStatusCodes.ServiceUnavailable):
                    return "503 Service Unavailable";

                case (HttpStatusCodes.GatewayTimeout):
                    return "504 Gateway Timeout";

                case (HttpStatusCodes.HTTPVersionNotSupported):
                    return "505 HTTP Version Not Supported";

                case (HttpStatusCodes.VariantAlsoNegotiates):
                    return "506 Variant Also Negotiates";

                case (HttpStatusCodes.InsufficientStorage):
                    return "507 Insufficient Storage";

                case (HttpStatusCodes.LoopDetected):
                    return "508 Loop Detected";

                case (HttpStatusCodes.NotExtended):
                    return "510 Not Extended";

                case (HttpStatusCodes.NetworkAuthenticationRequired):
                    return "511 Network Authentication Required";
                #endregion

                default:
                    return "500 Internal Server Error";
            }
        }

        public void Close(HttpConnection con)
        {
            Socs[con.Index].Soc.Close();//clsoe soc
            Socs[con.Index]._Headers = null;//null data
            Socs[con.Index]._Request = null;
            Socs[con.Index].Index = 0;
            Socs[con.Index].Soc = null;
        }

        private void EventText(TcpListener e)
        {
            int x = GetSocket();
            if (x >= 0)
            {
                Socs[x] = new HttpConnection()
                {
                    Soc = e.AcceptTcpClient(),
                    Index = x,
                    _Request = "",
                    _Headers = null,
                };
                
                List<string> Headers = new List<string>();
                using (StreamReader R = new StreamReader(Socs[x].Soc.GetStream()))
                {
                    while (Socs[x].Soc.Available > 0)
                    {
                        string Line = R.ReadLine();
                        if (Line.StartsWith("GET ") || Line.StartsWith("POST ") || Line.StartsWith("HEAD ")
                            || Line.StartsWith("PUT ") || Line.StartsWith("DELETE ") || Line.StartsWith("CONNECT ")
                            || Line.StartsWith("OPTIONS ") || Line.StartsWith("TRACE "))
                        {
                            Socs[x]._Request = Line;
                        }
                        else
                        {
                            Headers.Add(Line);
                        }
                    }
                }
                //read request

                Socs[x]._Headers = Headers.ToArray();
                Headers = null;//move Headers to Socs then remove unnecessary data

                if (StringHelpers.IsWhiteSpaceOrNull(Socs[x]._Request))
                {
                    if(Socs[x].Soc.Connected)
                    Socs[x].Soc.Client.Send(ResponseEncoding.GetBytes(
                        $"HTTP/1.1 400 Bad Request/Invalid Request Format\r\nDate: {DateTime.Today}\r\nContent-Length: 0\r\nContent-Type: text/html\r\nConnection: Closed\r\n\r\n"));
                    
                    Socs[x].Soc.Close();
                    Socs[x]._Request = null;
                    Socs[x]._Headers = null;//cleanup
                    Socs[x].Index = 0;
                    Socs[x].Soc = null;
                }
                else RequestEvent.Invoke(this, Socs[x]);
                if (AutoClose && Socs[x].Soc.Connected)
                    Close(Socs[x]);

            }
            else
                e.AcceptSocket().Disconnect(false);//auto close conn
            //becuse max connections has been reached
        }

        private int GetSocket()
        {
            for(int x = 0; x < Socs.Length; x++)
            {
                if(Socs[x].Soc == null || !Socs[x].Soc.Connected)//finds the first open Connection to use
                    return x;
            }
            return -1;
        }
        
        public void Dispose()
        {
            Running = false;
            FirstRun = false;
            while (THR.IsAlive);
            THR = null;
            Lis = null;
        }


        //gets/sets, add/remove
        public event HttpConAcceptEvent OnRequest { add     { RequestEvent += value; RequestEvents++; }
                                                    remove  { RequestEvent -= value; RequestEvents--; } }

    }
}
