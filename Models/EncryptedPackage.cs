using System;

namespace ShhhSMS.Models
{
    public class EncryptedPackage
    {
        public string Message { get; private set; }
        public string Nonce { get; private set; }
        public string PublicKey { get; private set; }

        public EncryptedPackage(byte[] message, byte[] nonce, byte[] privateKey)
        {
            Message = Convert.ToBase64String(message);
            Nonce = Convert.ToBase64String(nonce);
            PublicKey = Convert.ToBase64String(privateKey);
        }

        public override string ToString()
        {
            return $"{PublicKey}|{Nonce}|{Message}";
        }
    }
}