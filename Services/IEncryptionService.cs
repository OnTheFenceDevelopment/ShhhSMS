using ShhhSMS.Models;
using System;
using System.Threading.Tasks;

namespace ShhhSMS.Services
{
    public interface IEncryptionService
    {
        Task<string> DecryptMessage(string message);
        Task<EncryptedPackage> EncryptMessage(string message, string recipientId, string recipientsPublicKey);
        Task<bool> PasswordExists();
        Task<bool> PublicKeyExists();
        Task SetPassword(string password);
        Task SetPublicKey(string publicKey);
        Task<string> GetQRCodeContent();
    }
}