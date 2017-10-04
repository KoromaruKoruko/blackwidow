using System.Net.Sockets;
using System.Text;
using BDLib.Crypto;
using BDLib.BDLibInfo;
using System;
using System.Threading;
using System.Collections.Generic;

namespace BDLib.Net
{
    public enum EncryptionType
    {
        BDCryptoV1 = 2,
        BDCryptoV2 = 3,
    }
    public class BDSocket : IDisposable
    {
        private EncryptionType CT;
        private BDCrypto   BDCryptoV1;
        private BDCryptoV2 BDCryptoV2;

        public Encoding EnCoder;
        private Socket Soc;
        private Thread ReciveThread;
        private void ReadThread()
        {

            while(Soc.Connected)
            {

            }
        }


        public BDSocket(string Password, EncryptionType Crypto, int BDv2Complexity = 10)
        {
            throw new NotImplementedException("Comming soon");

            if (!Info.Moduls.Contains("Net/BDSocket.cs"))
                Info.Moduls.Add("Net/BDSocket.cs");
            
            CT = Crypto;
            switch(CT)
            {
                case (EncryptionType.BDCryptoV1):
                    BDCryptoV1 = new BDCrypto(Password);
                    break;

                case (EncryptionType.BDCryptoV2):
                    BDCryptoV2 = new BDCryptoV2(Password, BDv2Complexity);
                    break;
            }
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
        private string Decrypt(byte[] Data)
        {
            switch (CT)
            {
                case (EncryptionType.BDCryptoV1):
                    return EnCoder.GetString(BDCryptoV1.Compute(Data, true));

                case (EncryptionType.BDCryptoV2):
                    return EnCoder.GetString(BDCryptoV2.Compute(Data, true));
            }
            throw new System.Exception("RunTime Error, if you see this your BDLib has most likely been Curropted/Hacked IE this may be dagerus to use");
        }


        public void Send(byte[] message)
        {
            byte[] buffer = Encrypt(message);
        }

        public void Read(int count)
        {
            throw new NotImplementedException("Comming soon");
        }

        public void Dispose()
        {
            //needed soon
        }
    }
}
