using System;
using System.IO;

namespace BDLib.Crypto.KeyPad
{
    public class BDKeyPadV1 : BDKeyPadBase
    {
        private static readonly byte[] MagicNumbers = { 0xbd, 0xc7, 0x19, 0x20, 0xe4, 0x41 };
        private const int HeaderSize = 8;
        private Stream FSTM;
        private int KeySize;
        private int CURRKEY;

        /// <summary>
        /// If on the event that i reach the last key should i go back to the start or not
        /// </summary>
        public bool LoopOnEK;//Loop On EndKey
        public void EnableLoopOnEK() { LoopOnEK = true; }

        /// <summary>
        /// sets the current position '(((KeySize * to) - KeySize) + HeaderSize), "to" being the key ID'
        /// </summary>
        public void SetCurrentKey(int to)
        {
            if ((((KeySize * to) - KeySize) + HeaderSize) > FSTM.Length - KeySize) throw new ArgumentOutOfRangeException("File does not contaion that many Keys");
            if (to < 0) throw new ArgumentOutOfRangeException("keys are from 0 to <n>");
            CURRKEY = to;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Path">keypad file</param>
        public BDKeyPadV1(string Path)
        {
            FSTM = File.OpenRead(Path);

            byte[] HeaderInfo = new byte[8];
            FSTM.Read(HeaderInfo, 0, 8);

            if (HeaderInfo[0] != MagicNumbers[0] ||
                HeaderInfo[1] != MagicNumbers[1] ||
                HeaderInfo[2] != MagicNumbers[2] ||
                HeaderInfo[3] != MagicNumbers[3] ||
                HeaderInfo[4] != MagicNumbers[4] ||
                HeaderInfo[5] != MagicNumbers[5])
                throw new ArgumentException("this file is not a supported PAD FILE");
            CURRKEY = 0;
            KeySize = BitConverter.ToInt32(HeaderInfo, 6);

        }

        /// <summary>
        /// Creates a KeyPad file
        /// </summary>
        /// <param name="OutputPath">outputpath for keypad file</param>
        /// <param name="KeySize">size of each key</param>
        /// <param name="Keys">amount of keys</param>
        public static void CreatePad(string OutputPath, int KeySize, long Keys)
        {
            if (File.Exists(OutputPath)) throw new ArgumentException("File Exists");

            File.Create(OutputPath);
            Stream FSTM = File.OpenWrite(OutputPath);
            /*File Format
             * _v:magicnum --6bytes  :6
             * _v:KeySize  --2bytes  :8
             * ~DATA
             */

            FSTM.Write(MagicNumbers, 0, 6);
            FSTM.Write(BitConverter.GetBytes(KeySize), 0, 2);
            byte[] buf = new byte[KeySize];
            Random RND = new Random(KeySize * OutputPath.Length);

            for (int key = 0; key < Keys; key++)
            {
                RND.NextBytes(buf);
                FSTM.Write(buf, 0, KeySize);
            }
        }

        /// <summary>
        /// gets the next in position key
        /// </summary>
        /// <returns>byte[this.KeySize]</returns>
        public byte[] GetNextKey()
        {
            if (((KeySize * CURRKEY) - KeySize) + HeaderSize > FSTM.Length - KeySize)
            {
                if (LoopOnEK)
                {
                    FSTM.Position = HeaderSize;
                    CURRKEY = 0;
                }
                else
                {
                    throw new IndexOutOfRangeException("No more keys in keypad");
                }
            }
            else FSTM.Position = ((KeySize * CURRKEY) - KeySize) + HeaderSize;

            byte[] key = new byte[KeySize];
            FSTM.Read(key, 0, KeySize);
            CURRKEY++;
            FSTM.Position = 0;
            return key;
        }

        /// <summary>
        /// gets the key at position X
        /// </summary>
        /// <param name="num">Key ID</param>
        /// <returns>byte[this.KeySize]</returns>
        public byte[] GetKey(int num)
        {
            FSTM.Position = ((KeySize * num) - KeySize) + HeaderSize;//set to key pos
            byte[] key = new byte[KeySize];
            FSTM.Read(key, 0, KeySize);
            FSTM.Position = 0;
            return key;
        }

        public byte[] GetAllKeys()
        {
            FSTM.Position = 10;
            byte[] buffer = new byte[FSTM.Length - 10];
            FSTM.Read(buffer, 0, buffer.Length);
            return buffer;
        }
    }
}
