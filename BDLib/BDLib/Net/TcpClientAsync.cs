using System;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace BDLib.Net
{
    //delegates
    public delegate void TcpClientConnectEvent(object Sender, TcpClientConnectEventArgs Args);
    public delegate void TcpClientDisconnectEvent(object Sender, TcpClientDisconnectEventArgs Args);
    public delegate void TcpClientRecivedEvent(object Sender, TcpClientRecivedEventArgs Args);
    

    //delegate Args
    public struct TcpClientConnectEventArgs
    {
        public EndPoint IP { get { return __IP; } }
        public bool ConnectSucceeded { get { return __Succeeded; } }
        internal bool __Succeeded;
        internal EndPoint __IP;
    }
    public struct TcpClientDisconnectEventArgs
    {
        public bool IsSocketDestroyed;
        public int ReturnCode;
        public Exception ExceptionThrown;
    }
    public struct TcpClientRecivedEventArgs
    {
        public byte[] B_Data;
        public string S_Data;
    }

    public class TcpClientAsync
    {
        //events
        private event TcpClientConnectEvent _ConnectEvent;
        private event TcpClientDisconnectEvent _DisconnectEvent;
        private event TcpClientRecivedEvent _RecivedEvent;

        //evnet Calls
        private void OnConnect(TcpClientConnectEventArgs Args)
        {
            if (_ConnectEvent != null)
                _ConnectEvent.Invoke(this, Args);
        }
        private void OnDisconnect(TcpClientDisconnectEventArgs Args)
        {
            if (_DisconnectEvent != null)
                _DisconnectEvent.Invoke(this, Args);
        }
        private void OnRecived(byte[] Data)
        {
            if (_RecivedEvent != null)
                _RecivedEvent.Invoke(this, new TcpClientRecivedEventArgs() {
                    B_Data = (_ReturnStringOnly) ? null : Data,//if 'ReturnStringOnly' Dont Return B_Data
                    S_Data = _MyEncoding.GetString(Data),
                });
        }

        //members
        private Socket _Soc;
        private Encoding _MyEncoding;
        private bool _ReturnStringOnly;
        private Thread _ReciveTHR;
        private bool _Listning;
        private bool _NoDelay;
        private int _BufferSize;

        //Constructor
        public TcpClientAsync()
        {
            _Soc = AutoSocket.Tcp;
            _MyEncoding = Encoding.ASCII;
            _ReturnStringOnly = false;
            _ReciveTHR = new Thread(new ThreadStart(ReciveThread));
            _NoDelay = false;
            _BufferSize = -1;
        }
        public TcpClientAsync(Encoding MyEncoding)
        {
            _Soc = AutoSocket.Tcp;
            this._MyEncoding = MyEncoding;
            _ReturnStringOnly = false;
            _ReciveTHR = new Thread(new ThreadStart(ReciveThread));
            _NoDelay = false;
            _BufferSize = -1;
        }
        public TcpClientAsync(bool ReturnStringOnly)
        {
            _Soc = AutoSocket.Tcp;
            _MyEncoding = Encoding.ASCII;
            this._ReturnStringOnly = ReturnStringOnly;
            _ReciveTHR = new Thread(new ThreadStart(ReciveThread));
            _NoDelay = false;
            _BufferSize = -1;
        }
        public TcpClientAsync(Encoding MyEncoding, bool ReturnStringOnly)
        {
            _Soc = AutoSocket.Tcp;
            this._MyEncoding = MyEncoding;
            this._ReturnStringOnly = ReturnStringOnly;
            _ReciveTHR = new Thread(new ThreadStart(ReciveThread));
            _NoDelay = false;
            _BufferSize = -1;
        }

        //Functions
        public void Connect(string Host, int Port)
        {
            try
            {
                _Soc.Connect(Host, Port);
                _Listning = true;
                _ReciveTHR.Start();

                OnConnect(new TcpClientConnectEventArgs(){
                    __IP = _Soc.RemoteEndPoint,
                    __Succeeded = _Soc.Connected,
                });
            }
            catch
            {
                OnConnect(new TcpClientConnectEventArgs() {
                    __Succeeded = false,
                    __IP = null
                });
            }
        }
        public void Disconnect(bool ReuseME)
        {
            try
            {
                _Listning = false;
                while (_ReciveTHR.IsAlive) ;//wait till ReciveThread is Closed (no timeout)
                _ReciveTHR.Join();//prep for next use

                _Soc.Disconnect(ReuseME);
                OnDisconnect(new TcpClientDisconnectEventArgs()
                {
                    ExceptionThrown = null,
                    IsSocketDestroyed = ReuseME,
                    ReturnCode = 0,
                });
            }
            catch(SocketException e)
            {
                OnDisconnect(new TcpClientDisconnectEventArgs{
                    ReturnCode = e.ErrorCode,
                    ExceptionThrown = e,
                    IsSocketDestroyed = false,
                });
            }
            catch(Exception e)
            {
                OnDisconnect(new TcpClientDisconnectEventArgs
                {
                    ReturnCode = -1,
                    ExceptionThrown = e,
                    IsSocketDestroyed = false,
                });
            }
        }

        public void Send(byte[] Data)
        {
            if (_Soc.Connected)
                _Soc.Send(Data);
            else throw new InvalidOperationException("Socket must be connected");
        }
        public void Send(string Data)
        {
            if (_Soc.Connected)
                _Soc.Send(_MyEncoding.GetBytes(Data));
            else throw new InvalidOperationException("Socket must be connected");
        }

        public void Bind(EndPoint localEP)
        {
            _Soc.Bind(localEP);
        }

        //Threads
        private void ReciveThread()
        {
            while(_Listning)
            {
                try
                {
                    if (!_Soc.Connected)
                    {
                        OnDisconnect(new TcpClientDisconnectEventArgs()
                        {
                            ExceptionThrown = null,
                            IsSocketDestroyed = false,
                            ReturnCode = -2,
                        });
                        _Listning = false;
                        break;
                    }

                    if (_Soc.Available > 0)
                    {
                        byte[] Buffer = new byte[(_BufferSize > 0) ? _Soc.Available % _BufferSize : _Soc.Available];
                        _Soc.Receive(Buffer);
                        OnRecived(Buffer);
                    }

                    if (!_NoDelay)
                        Thread.Sleep(20);
                }
                catch(Exception e)
                {
                    OnDisconnect(new TcpClientDisconnectEventArgs(){
                        ExceptionThrown = e,
                        ReturnCode = -3,
                        IsSocketDestroyed = true,
                    });
                    break;
                }
            }
        }
        
        //get/set add/remove
        public event TcpClientConnectEvent    ConnectEvent
        {
            add { _ConnectEvent += value; }
            remove { _ConnectEvent -= value; }
        }
        public event TcpClientDisconnectEvent DisconnectEvent
        {
            add { _DisconnectEvent += value; }
            remove { _DisconnectEvent -= value; }
        }
        public event TcpClientRecivedEvent    RecivedEvent
        {
            add { _RecivedEvent += value; }
            remove { _RecivedEvent -= value; }
        }

        public Encoding Encoding         { get { return _MyEncoding; } }
        public bool     ReturnStringOnly { get { return _ReturnStringOnly; } }
        public bool     IsRunning        { get { return _Listning; } }
        public bool     IsBound          { get { return _Soc.IsBound; } }
        public Socket   Client           { get { return _Soc; } }

        public bool NoDelay
        {
            get { return _NoDelay; }
            set
            {
                _NoDelay = value;
                _Soc.NoDelay = value;
            }
        }
        public int MaxBufferSize
        {
            get { return _BufferSize; }
            set { _BufferSize = value; }
        }
    }
}
