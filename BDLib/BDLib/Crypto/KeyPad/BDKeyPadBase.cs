namespace BDLib.Crypto.KeyPad
{
    public interface BDKeyPadBase
    {
        void EnableLoopOnEK();
        byte[] GetNextKey();
        byte[] GetKey(int num);
        void SetCurrentKey(int to);
    }
}
