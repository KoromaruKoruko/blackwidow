using System.Security.Cryptography;
using BDLib.Crypto.Hash;
using BDLib.Text;
using System.Text;
using System.Collections.Generic;
using System;

namespace BDLib.Crypto
{
    public class SpecialBDCrypto
    {

        private Dictionary<byte,byte> Encryption;
        private Dictionary<byte,byte> Decryption;
        private byte[] _Key;


        public SpecialBDCrypto(string Password)
        {
            OneKeyHasher Translator = new OneKeyHasher();

            Encryption = new Dictionary<byte, byte>();
            Decryption = new Dictionary<byte, byte>();

            byte[] TMPStore;

            using (SHA384Cng Seg2 = new SHA384Cng())
            using (HMACSHA384 Seg1 = new HMACSHA384(Encoding.UTF32.GetBytes(Password)))
            {
                Seg1.Initialize();
                Translator.TheHashSize = 512;
                Translator.TheCharsetUsed = Encoding.UTF8;
                TMPStore = Translator.Hash(Password);

                Seg1.TransformBlock(TMPStore, 0, TMPStore.Length, TMPStore, 0);
                Seg1.TransformBlock(TMPStore, 0, TMPStore.Length, TMPStore, 0);
                Seg1.TransformBlock(TMPStore, 0, TMPStore.Length, TMPStore, 0);
                Seg1.TransformBlock(TMPStore, 0, TMPStore.Length, TMPStore, 0);
                TMPStore = Seg1.TransformFinalBlock(TMPStore, 0, TMPStore.Length);
                TMPStore = Seg2.ComputeHash(TMPStore);
            }

            Translator.TheCharsetUsed = Encoding.Unicode;
            Translator.TheHashSize = 255;
            TMPStore = Translator.Hash(TMPStore);

            int pos = 0;
            byte tmp = 0;
            while (true)
            {
                if (!Encryption.ContainsValue(TMPStore[pos]) && !Decryption.ContainsKey(TMPStore[pos]))
                {
                    Encryption.Add(tmp, TMPStore[pos]);
                    Decryption.Add(TMPStore[pos], tmp);
                    if (tmp == 255)
                        break;
                    else
                        tmp++;
                }

                pos++;
                if(pos == TMPStore.Length)
                {
                    TMPStore = Translator.Hash(TMPStore);
                    pos = 0;
                }
            }

            Key = new byte[256];
            Layers = 5;

            byte x = 0;
            while (true)
            {
                Key[x] = Encryption[x];
                if (x == 255)
                    break;
                else
                    x++;
            }
            
        }
        public SpecialBDCrypto(byte[] KEY)
        {
            if (KEY.Length != 256)
                throw new ArgumentException("Key must support 0-255 Ie Length must be 256");

            Encryption = new Dictionary<byte, byte>();
            Decryption = new Dictionary<byte, byte>();


            Key = KEY;
        }

        private byte Translate(byte x,              bool Decrypt)
        {
            return  (Decrypt) ? (Decryption[x]) : (Encryption[x]);
        }

        public byte[] Compute(string message,       bool Decrypt)
        {
            byte[] Output = Encoding.BigEndianUnicode.GetBytes(message);
            return Compute(Output, Decrypt);
        }
        public byte[] Compute(byte[] message,       bool Decrypt)
        {
            byte[] buffer = new byte[message.Length];//this is so that we can Constantly re-set the value
            message.CopyTo(buffer, 0);

            for (int y = 0; y != Layers; y++)
            {
                for (int x = 0; x < message.Length; x++)
                {
                    buffer[x] = Translate(buffer[x], Decrypt);
                }
            }

            return buffer;
        }

        public string Compute_ASCII(string message, bool Decrypt)
        {
            return Encoding.ASCII.GetString(Compute(message, Decrypt));
        }
        public string Compute_UTF7(string message,  bool Decrypt)
        {
            return Encoding.UTF7.GetString(Compute(message, Decrypt));
        }
        public string Compute_UTF8(string message,  bool Decrypt)
        {
            return Encoding.UTF8.GetString(Compute(message, Decrypt));
        }
        public string Compute_UTF32(string message, bool Decrypt)
        {
            return Encoding.UTF32.GetString(Compute(message, Decrypt));
        }

        public string Compute_Hex(string message,   bool Decrypt)
        {
            return Hex.ToHEX(Compute(message, Decrypt));
        }

        public byte[] Key      { get { return _Key; }
                                 set
            {
                if (value.Length != 256)
                    throw new ArgumentOutOfRangeException("Key must support 0-255 Ie Length must be 256");
                else
                {
                    Encryption = new Dictionary<byte, byte>();
                    Decryption = new Dictionary<byte, byte>();


                    _Key = value;
                    byte x = 0;
                    while (true)
                    {
                        try
                        {
                            Encryption.Add(x, value[x]);
                            Decryption.Add(value[x], x);
                        }
                        catch
                        {
                            throw new Exception("INVALID KEY!!!");
                        }

                        if (x == 255)
                            break;
                        else
                            x++;
                    }
                }
            }
        }
        public uint Layers     { get; set; }
    }
}
