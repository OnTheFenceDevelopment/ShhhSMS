using System;

namespace ShhhSMS.Models
{
    public class EncryptedPackage
    {
        public string Message { get; private set; }
        public string Nonce { get; private set; }
        public string RecipientId { get; private set; }

        public EncryptedPackage(byte[] message, byte[] nonce, string recipientId)
        {
            Message = Convert.ToBase64String(message);
            Nonce = Convert.ToBase64String(nonce);
            RecipientId = recipientId;
        }

        public override string ToString()
        {
            return $"{RecipientId}|{Nonce}|{Message}";
        }
    }
}