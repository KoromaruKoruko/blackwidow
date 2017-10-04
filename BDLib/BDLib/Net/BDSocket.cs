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
        public  Encoding       EnCoder;
        private EncryptionType CT;
        private BDCrypto       BDCryptoV1;
        private BDCryptoV2     BDCryptoV2;
        private string         PassHash;
        private Socket         Soc;
        private Thread         ReciveThread;
        private Queue<byte>    DownStream;
        private bool           Run;
        private void           ReadThread()
        {
            while(Soc.Connected)
            {
                if (Soc.Available > 0 && Run)
                {
                    byte[] buffer = new byte[Soc.Available];
                    Soc.Receive(buffer);
                    buffer = Decrypt(buffer);
                    for(int x=0; x<buffer.Length;x++)
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
                switch(CT)
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
            switch(CT)
            {
                case (EncryptionType.BDCryptoV1):
                    BDCryptoV1 = new BDCrypto(Password);
                    break;

                case (EncryptionType.BDCryptoV2):
                    BDCryptoV2 = new BDCryptoV2(Password, BDv2Complexity);
                    break;

                default:
                    throw new ArgumentNullException("No Encryption type Speciffyed");
            }
            Soc = AutoSocket.Tcp;
            OneKeyHasher Hs = new OneKeyHasher()
            {
                TheCharsetUsed = Encoding.ASCII,
                TheHashSize = 1024,
            };
            PassHash=Encoding.ASCII.GetString(Hs.Hash(Password));
            DownStream = new Queue<byte>();
            ReciveThread = new Thread(new ThreadStart(ReadThread));
        }
        public bool Connect(string host, int port)
        {
            Soc.Connect(host, port);
            string[] RPT = (Soc.RemoteEndPoint as IPEndPoint).Address.ToString().Split(new char[] { '.' });
            string[] LPT = (Soc.LocalEndPoint as IPEndPoint).Address.ToString().Split(new char[]{ '.' });

            OneKeyHasher HS = new OneKeyHasher() {TheCharsetUsed=Encoding.ASCII,TheHashSize=1024};
            BDCryptoV2 STM = new BDCryptoV2(Encoding.ASCII.GetString(HS.Hash($"{RPT[0]}.{LPT[3]}.{RPT[1]}.{LPT[2]}.{RPT[2]}.{LPT[1]}.{RPT[3]}.{LPT[0]}")), 2);
            Soc.Send(STM.Compute(Encoding.ASCII.GetBytes($"{BitConverter.GetBytes(PassHash.Length)}{PassHash}#{(CT == EncryptionType.BDCryptoV1).ToString()}"), false));
            DateTime TMR = DateTime.Now;
            while (DateTime.Now - TMR > TimeSpan.FromSeconds(10) && Soc.Available < 0) Thread.Sleep(10);//wait for Return
            Run = true;
            if (Soc.Available < 1) return false;

            byte[] Return = new byte[1];
            Soc.Receive(Return);

            Return = STM.Compute(Return, true);

            switch(Return[0])
            {
                case (157):
                    ReciveThread.Start();
                    return true;

                case (168):
                    Soc.Disconnect(true);
                    return false;

                case (28):
                    throw new Exception("IpBan");

                default:
                    throw new ArgumentException("this is most likly not a BDServer or it is too slow and BDCryptoV1 should be used");
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
            switch(CT)
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
        public string ReadLine(char Breaker='\n')
        {
            string output = "";
            while(true)
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
}
