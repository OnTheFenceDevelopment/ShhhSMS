using ShhhSMS.Models;
using System;
using System.Text;
using System.Threading.Tasks;

namespace ShhhSMS.Services
{
    public class EncryptionService : IEncryptionService
    {
        public async Task<bool> PublicKeyExists()
        {
            return await Xamarin.Essentials.SecureStorage.GetAsync(Constants.Identifiers.PublicKey) != null;
        }

        public async Task SetPublicKey(string publicKey)
        {
            await Xamarin.Essentials.SecureStorage.SetAsync(Constants.Identifiers.PublicKey, publicKey);
        }

        public async Task<bool> PasswordExists()
        {
            var userPassword = await Xamarin.Essentials.SecureStorage.GetAsync(Constants.Identifiers.UserPassword);

            return userPassword != null;
        }

        public async Task SetPassword(string password)
        {
            await Xamarin.Essentials.SecureStorage.SetAsync(Constants.Identifiers.UserPassword, password);
        }

        public async Task<EncryptedPackage> EncryptMessage(string message, string recipientsPublicKey)
        {
            // One Time Nonce - needs to be included in Message
            var nonce = Sodium.PublicKeyBox.GenerateNonce();
            
            // Generate Users Key Pair (to access Private Key)
            var keyPair = await GenerateKeyPair();

            // Get own Contact Id
            var contactId = await GetContactId();

            // Encrypt Message
            var encryptedMessage = Sodium.PublicKeyBox.Create(message, nonce, keyPair.PrivateKey, Convert.FromBase64String(recipientsPublicKey));

            return new EncryptedPackage(encryptedMessage, nonce, contactId);
        }

        public async Task<string> DecryptMessage(DecryptionPackage decryptionPackage)
        {
            var keyPair = await GenerateKeyPair();

            var decryptedMessage = Sodium.PublicKeyBox
                .Open(decryptionPackage.Message, decryptionPackage.Nonce, keyPair.PrivateKey, decryptionPackage.PublicKey);

            return Encoding.UTF8.GetString(decryptedMessage);
        }

        private async Task<Sodium.KeyPair> GenerateKeyPair()
        {
            var password = await Xamarin.Essentials.SecureStorage.GetAsync(Constants.Identifiers.UserPassword);
            var publicKey = await Xamarin.Essentials.SecureStorage.GetAsync(Constants.Identifiers.PublicKey);
            var deviceId = await Xamarin.Essentials.SecureStorage.GetAsync(Constants.Identifiers.DeviceId);

            var passwordBytes = Sodium.GenericHash.Hash(password.ToString(), deviceId, 32);
            var keyPair = Sodium.PublicKeyBox.GenerateKeyPair(passwordBytes);

            if (publicKey.Equals(Convert.ToBase64String(keyPair.PublicKey)))
            {
                return keyPair;
            }

            return null;
        }

        private async Task<string> GetContactId()
        {
            var deviceId = await Xamarin.Essentials.SecureStorage.GetAsync(Constants.Identifiers.ContactId);
            return deviceId;
        }

        public async Task<string> GetQRCodeContent()
        {
            var contactId = await Xamarin.Essentials.SecureStorage.GetAsync(Constants.Identifiers.ContactId);
            var publicKey = await Xamarin.Essentials.SecureStorage.GetAsync(Constants.Identifiers.PublicKey);

            return $"{contactId}|{publicKey}";
        }

        public bool ClearPassword()
        {
            var result = Xamarin.Essentials.SecureStorage.Remove(Constants.Identifiers.UserPassword);

            return result;
        }
    }
}