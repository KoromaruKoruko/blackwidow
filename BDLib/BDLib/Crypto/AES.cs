using System.Security.Cryptography;
using System.IO;

namespace BDLib.Crypto
{
    public class AESStream
    {
        public bool AutoGenKey = false;

        public byte[] _Key, _IVKey;
        public byte[] Key   { get { return _Key;   } }
        public byte[] IVKey { get { return _IVKey; } }
        private AesManaged AesMa;
        private MemoryStream MS_Encryptor;
        private CryptoStream Encryptor;
        private MemoryStream MS_Decryptor;
        private CryptoStream Decryptor;
        
        public AESStream(byte[] Key, byte[] VIKey, CipherMode CMode, PaddingMode PMode, bool AutoGenKey = false)
        {
            AesManaged AesMa = new AesManaged();

            if (Key != null && !AutoGenKey)
            {
                _Key = Key;
                AesMa.Key = _Key;
            }
            else
            {
                AesMa.GenerateKey();
                _Key = AesMa.Key;
            }
                    
            if(VIKey != null && !AutoGenKey)
            {
                _IVKey = VIKey;
                AesMa.IV = _IVKey;
            }
            else
            {
                AesMa.GenerateIV();
                _IVKey = AesMa.IV;
            }

            AesMa.Mode = CMode;
            AesMa.Padding = PMode;
        }

        public void OpenStream()
        {
            MS_Encryptor = new MemoryStream();
            Encryptor = new CryptoStream(MS_Encryptor, AesMa.CreateEncryptor(), CryptoStreamMode.Write);
            MS_Decryptor = new MemoryStream();
            Decryptor = new CryptoStream(MS_Decryptor, AesMa.CreateDecryptor(), CryptoStreamMode.Read);
        }
        public void CloseStream()
        {
            Encryptor.Dispose();
            MS_Encryptor.Dispose();
            Decryptor.Dispose();
            MS_Decryptor.Dispose();
        }
        public void FullyCloseStream()
        {
            CloseStream();
            AesMa.Dispose();
        }

        public byte[] Encrypt(byte[] Data)
        {
            using (StreamWriter W = new StreamWriter(MS_Encryptor))
                W.Write(Data);
            return MS_Encryptor.ToArray();
            
        }
        public byte[] Decrypt(byte[] Data)
        {
            using (StreamWriter W = new StreamWriter(MS_Decryptor))
                W.Write(Data);
            return MS_Decryptor.ToArray();
        }
    }
}
