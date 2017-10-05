using System.Net.Sockets;
using System.Text;
using BDLib.Crypto;
using BDLib.Crypto.Hash;
using BDLib.BDLibInfo;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Net;


namespace BDLib.Net
{
    public enum EncryptionType
    {
        BDCryptoV1 = 2,
        BDCryptoV2 = 3,
    }
    public class BDSocket
    {
        public Encoding EnCoder;
        private EncryptionType CT;
        private BDCrypto BDCryptoV1;
        private BDCryptoV2 BDCryptoV2;
        private string PassHash;
        internal Socket Soc;//internal due to BDSocketListner
        internal Thread ReciveThread;//internal due to BDSocketListner
        private Queue<byte> DownStream;
        internal bool Run;//internal due to BDSocketListner
        private void ReadThread()
        {
            while (Soc.Connected)
            {
                if (Soc.Available > 0 && Run)
                {
                    byte[] buffer = new byte[Soc.Available];
                    Soc.Receive(buffer);
                    buffer = Decrypt(buffer);
                    for (int x = 0; x < buffer.Length; x++)
                        DownStream.Enqueue(buffer[x]);

                }
                else
                    Thread.Sleep(10);
            }
        }

        public uint Layers
        {
            get
            {
                return (CT == EncryptionType.BDCryptoV1) ? BDCryptoV1.Layers : BDCryptoV2.Layers;
            }
            set
            {
                switch (CT)
                {
                    case (EncryptionType.BDCryptoV1):
                        BDCryptoV1.Layers = value;
                        break;

                    case (EncryptionType.BDCryptoV2):
                        BDCryptoV2.Layers = value;
                        break;
                }
                throw new System.Exception("RunTime Error, if you see this your BDLib has most likely been Curropted/Hacked IE this may be dagerus to use");
            }
        }
        public int Available
        {
            get { return Soc.Available; }
        }

        public BDSocket(string Password, EncryptionType Crypto, int BDv2Complexity = 10)
        {
            if (!Info.Moduls.Contains("Net/BDSocket.cs"))
                Info.Moduls.Add("Net/BDSocket.cs");

            CT = Crypto;
            EnCoder = Encoding.ASCII;
            OneKeyHasher Hs = new OneKeyHasher()
            {
                TheCharsetUsed = Encoding.ASCII,
                TheHashSize = 1024,
            };
            PassHash = Encoding.ASCII.GetString(Hs.Hash(Password));
            switch (CT)
            {
                case (EncryptionType.BDCryptoV1):
                    BDCryptoV1 = new BDCrypto(PassHash);
                    break;

                case (EncryptionType.BDCryptoV2):
                    BDCryptoV2 = new BDCryptoV2(PassHash, BDv2Complexity);
                    break;

                default:
                    throw new ArgumentNullException("No Encryption type Speciffyed");
            }
            Soc = AutoSocket.Tcp;
            DownStream = new Queue<byte>();
            ReciveThread = new Thread(new ThreadStart(ReadThread));
        }
        ~BDSocket()
        {
            if (Soc != null && Soc.Connected)
                Soc.Disconnect(false);
            BDCryptoV1 = null;
            BDCryptoV2 = null;
            DownStream.Clear();
            DownStream = null;
            if (ReciveThread.IsAlive)
            {
                Run = false;
                ReciveThread.Join();
            }
        }//clean up

        public bool Connect(string host, int port)
        {
            Soc.Connect(host, port);
            string[] RPT = (Soc.RemoteEndPoint as IPEndPoint).Address.ToString().Split(new char[] { '.' });
            string[] LPT = (Soc.LocalEndPoint as IPEndPoint).Address.ToString().Split(new char[] { '.' });

            OneKeyHasher HS = new OneKeyHasher() { TheCharsetUsed = Encoding.ASCII, TheHashSize = 1024 };
            BDCryptoV2 STM = new BDCryptoV2(Encoding.ASCII.GetString(HS.Hash($"{RPT[0]}.{LPT[3]}.{RPT[1]}.{LPT[2]}.{RPT[2]}.{LPT[1]}.{RPT[3]}.{LPT[0]}")), 2);
            Soc.Send(STM.Compute(Encoding.ASCII.GetBytes($"{BitConverter.GetBytes(PassHash.Length)}{PassHash}{(CT == EncryptionType.BDCryptoV1).ToString()}"), false));
            DateTime TMR = DateTime.Now;
            while (DateTime.Now - TMR < TimeSpan.FromSeconds(10) && Soc.Available < 0) Thread.Sleep(10);//wait for Return
            if (Soc.Available < 1) return false;
            Run = true;

            byte[] Return = new byte[1];
            Soc.Receive(Return);

            Return = STM.Compute(Return, true);

            TMR = DateTime.Now;
            while (DateTime.Now - TMR < TimeSpan.FromSeconds(10) && Soc.Available < 0) Thread.Sleep(10);//wait for Return
            if (Soc.Available < 1) return false;

            switch (Return[0])
            {
                case (157):
                    ReciveThread.Start();
                    return true;

                case (168):
                    Soc.Disconnect(true);//Connection Reject via Crypto NOT accepted
                    return false;

                case (28):
                    throw new Exception("IpBan");

                default:
                    throw new ArgumentException("this is most likly not a BDServer");
            }
        }
        public void Disconnect(bool Reuse)
        {
            Run = false;
            ReciveThread.Join();
            Soc.Disconnect(Reuse);
            BDCryptoV1 = null;
            BDCryptoV2 = null;
        }

        private byte[] Encrypt(string Message)
        {
            switch (CT)
            {
                case (EncryptionType.BDCryptoV1):
                    return BDCryptoV1.Compute(EnCoder.GetBytes(Message), false);

                case (EncryptionType.BDCryptoV2):
                    return BDCryptoV2.Compute(EnCoder.GetBytes(Message), false);
            }
            throw new System.Exception("RunTime Error, if you see this your BDLib has most likely been Curropted/Hacked IE this may be dagerus to use");
        }
        private byte[] Encrypt(byte[] Message)
        {
            switch (CT)
            {
                case (EncryptionType.BDCryptoV1):
                    return BDCryptoV1.Compute(Message, false);

                case (EncryptionType.BDCryptoV2):
                    return BDCryptoV2.Compute(Message, false);
            }
            throw new System.Exception("RunTime Error, if you see this your BDLib has most likely been Curropted/Hacked IE this may be dagerus to use");
        }
        private byte[] Decrypt(byte[] Data)
        {
            switch (CT)
            {
                case (EncryptionType.BDCryptoV1):
                    return BDCryptoV1.Compute(Data, true);

                case (EncryptionType.BDCryptoV2):
                    return BDCryptoV2.Compute(Data, true);
            }
            throw new System.Exception("RunTime Error, if you see this your BDLib has most likely been Curropted/Hacked IE this may be dagerus to use");
        }

        public void Send(byte[] message)
        {
            byte[] buffer = Encrypt(message);
            Soc.Send(buffer);
        }
        public void WriteLine(string message)
        {
            byte[] buffer = Encrypt(message + "\n");
            Soc.Send(buffer);
        }
        public void Write(string message)
        {
            byte[] buffer = Encrypt(message);
            Soc.Send(buffer);
        }

        public byte[] Read(int count)
        {
            byte[] buffer = new byte[count];
            if (DownStream.Count >= count)
                for (int x = 0; x < count; x++)
                    buffer[x] = DownStream.Dequeue();
            else throw new InvalidOperationException("not enuf data to read");

            return buffer;

        }
        public string ReadLine(char Breaker = '\n')
        {
            string output = "";
            while (true)
            {
                if (DownStream.Count == 0)
                    return output;
                else
                {
                    char input = EnCoder.GetChars(new byte[] { DownStream.Dequeue() })[0];
                    if (input == Breaker)
                        return output;
                    else
                        output += input;
                }
            }

        }
        public string StrRead(int count)
        {
            byte[] buffer = new byte[count];
            if (DownStream.Count >= count)
                for (int x = 0; x < count; x++)
                    buffer[x] = DownStream.Dequeue();
            else throw new InvalidOperationException("not enuf data to read");

            return EnCoder.GetString(buffer);
        }
    }

    public struct TryAcceptResult
    {
        public bool Success;
        public BDSocket Output;
    };
    public class BDSocketListener
    {
        private Socket Soc;
        public int Complexity = 10;
        public uint Layers = 5;
        private byte[] EIP;
        List<string> IPBANS;
        private bool CryptoType;
        private bool WhiteListing;

        public BDSocketListener(int port)
        {
            EIP = new byte[4];
            string[] tmp_EIP = IPInfo.GetIPV4RemoteIpAddress().Split(new char[] { '.' });

            for (int x = 0; x < 4; x++)
                EIP[x] = byte.Parse(tmp_EIP[x]);

            Soc = AutoSocket.Tcp;
            IPBANS = new List<string>();
        }
        ~BDSocketListener()
        {
            Soc.Dispose();
        }

        public void Start(int BackLog = 5)
        {
            Soc.Listen(BackLog);
        }
        public void WhitelistCryptoTo(EncryptionType CT)
        {
            CryptoType = (CT == EncryptionType.BDCryptoV1);
            WhiteListing = true;
        }

        public TryAcceptResult TryAccept()
        {
            TryAcceptResult R = new TryAcceptResult();
            R.Output = Accept();
            R.Success = (R.Output != null);
            return R;
        }
        public BDSocket Accept()
        {
            try
            {
                Socket soc = Soc.Accept();
            }
            catch (SocketException e)
            {
                if (e.ErrorCode == 10035)
                    return null;
                else
                    throw e;
            }

            byte[] LIP = new byte[4];
            string[] tmp_LIP = (Soc.RemoteEndPoint as IPEndPoint).Address.ToString().Split(new char[] { '.' });

            for (int x = 0; x < 4; x++)
                LIP[x] = byte.Parse(tmp_LIP[x]);
            OneKeyHasher HS = new OneKeyHasher() { TheCharsetUsed = Encoding.ASCII, TheHashSize = 1024 };
            BDCryptoV2 CSTM = new BDCryptoV2(Encoding.ASCII.GetString(HS.Hash($"{EIP[0]}.{LIP[3]}.{EIP[1]}.{LIP[2]}.{EIP[2]}.{LIP[1]}.{EIP[3]}.{LIP[0]}")), 2);
            if (IPBANS.Contains((Soc.RemoteEndPoint as IPEndPoint).Address.ToString()))
            {
                Soc.Send(CSTM.Compute(new byte[] { 28 }, false));
                return Accept();//try to get a new client that the Coder Gods Want
            }
            LIP = null;
            tmp_LIP = null;
            byte[] input = new byte[4];
            DateTime TMR = DateTime.Now;
            while (DateTime.Now - TMR < TimeSpan.FromSeconds(10) && Soc.Available < 4) Thread.Sleep(10);//wait for Return
            if (Soc.Available < 1) throw new TimeoutException("Client Failed to send Auth Data");

            Soc.Receive(input);
            input = new byte[BitConverter.ToInt32(CSTM.Compute(input, true), 0)];
            TMR = DateTime.Now;
            while (DateTime.Now - TMR < TimeSpan.FromSeconds(10) && Soc.Available < input.Length) Thread.Sleep(10);//wait for Return
            if (Soc.Available < 1) throw new TimeoutException("Client Failed to send Auth Data");
            Soc.Receive(input);
            string pass = Encoding.ASCII.GetString(CSTM.Compute(input, true));
            input = new byte[Soc.Available];
            Soc.Receive(input);
            EncryptionType CT;

            if (!WhiteListing)
            {
                if (Encoding.Default.GetString(CSTM.Compute(input, true)) == "true")
                    CT = EncryptionType.BDCryptoV1;
                else
                    CT = EncryptionType.BDCryptoV2;
            }
            else
            {
                if (Encoding.Default.GetString(CSTM.Compute(input, true)) != CryptoType.ToString())
                {
                    Soc.Send(CSTM.Compute(new byte[] { 157 }, false));
                    return Accept();//MORE FOR THE CODER GODS
                }
                else if (CryptoType)
                    CT = EncryptionType.BDCryptoV1;
                else
                    CT = EncryptionType.BDCryptoV2;

            }

            input = null;
            Soc.Send(CSTM.Compute(new byte[] { 157 }, false));

            return CreateBDSocket(Soc, pass, CT);
        }

        private BDSocket CreateBDSocket(Socket Soc, string password, EncryptionType CT)
        {
            BDSocket BDSoc = new BDSocket(password, CT, Complexity) { Run = true, Soc = Soc };
            BDSoc.ReciveThread.Start();
            return BDSoc;
        }
    }
}
