namespace ShhhSMS.Models
{
    public class DecryptionPackage
    {
        public byte[] Message { get; private set; }
        public byte[] Nonce { get; private set; }
        public byte[] PublicKey { get; private set; }

        public DecryptionPackage(byte[] message, byte[] nonce, byte[] publicKey)
        {
            Message = message;
            Nonce = nonce;
            PublicKey = publicKey;
        }
    }
}