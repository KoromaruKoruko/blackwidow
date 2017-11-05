using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using BDLib.Crypto.KeyPad;
using BDLib.Crypto;

namespace BDLib.Net
{
    //this is for Extream Cases
    public class BDSocketV2 
    {
        internal Socket Soc;// internal due to Listener
        private BDKeyPadBase KeyPadRX,KeyPadTX;
        private BDCryptoV2 Crypto;
        
        public int Available => Soc.Available;
        public bool Connected => Soc.Connected;

        public BDSocketV2(BDKeyPadBase KeyPad)
        {
            throw new NotImplementedException();
            KeyPad.SetCurrentKey(0);
            KeyPad.EnableLoopOnEK();

            this.KeyPadRX = KeyPad;
            this.KeyPadTX = KeyPad;
        }

        public bool Connect(string host, int port)
        {
            try
            {
                Soc.Connect(host, port);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void Disconnect(bool Reuse)
        {
            Soc.Disconnect(Reuse);
        }

        //TODO make pad encryption and implament it
        private byte[] Encrypt(byte[] data)
        {
            Crypto.Instructions = KeyPadTX.GetNextKey();
            return Crypto.Compute(data, false);
        }
        private byte[] Decrypt(byte[] data)
        {
            Crypto.Instructions = KeyPadRX.GetNextKey();
            return Crypto.Compute(data, true);
        }

        public string ReadPackage()
        {
            Queue<byte> input = new Queue<byte>();
            byte[] onebyte = new byte[1];
            while(Soc.Available > 0)
            {
                Soc.Receive(onebyte);
                if (onebyte[0] == 0) break;
                else
                    input.Enqueue(onebyte[0]);
            }
            return Encoding.ASCII.GetString(input.ToArray());
        }
        public void SendPackage(string message)
        {
            Soc.Send(Encrypt(Encoding.ASCII.GetBytes(message)));
        }
    }
    public class BDSocketV2Listener
    {
        private static readonly byte[] MagicNumbers = new byte[] { 0x12, 0x12, 0xff, 0x54, 0xae, 0x00, 0x76 };
        private BDKeyPadBase KeyPad;

        public BDSocketV2Listener(BDKeyPadBase KeyPad)
        {
            this.KeyPad = KeyPad;
            throw new NotImplementedException("comming soon");
        }
        private BDSocketV2 CreateBDSocketV2(Socket soc)
        {
            return new BDSocketV2(KeyPad) { Soc = soc };
        }
    }
}
