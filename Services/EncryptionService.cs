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
            var foo = await Xamarin.Essentials.SecureStorage.GetAsync(Constants.Identifiers.UserPassword);

            return foo != null;
        }

        public async Task SetPassword(string password)
        {
            await Xamarin.Essentials.SecureStorage.SetAsync(Constants.Identifiers.UserPassword, password);
        }

        public async Task<EncryptedPackage> EncryptMessage(string message)
        {
            // One Time Nonce - needs to be included in Message
            var nonce = Sodium.PublicKeyBox.GenerateNonce();
            var keyPair = await GenerateKeyPair();

            var encryptedMessage = Sodium.PublicKeyBox.Create(message, nonce, keyPair.PrivateKey, keyPair.PublicKey);

            return new EncryptedPackage(encryptedMessage, nonce, keyPair.PublicKey);
        }

        public async Task<string> DecryptMessage(string message)
        {
            // One Time Nonce - needs to be included in Message
            var packetElements = message.Split("|");
            var publicKey = packetElements[0];
            var nonce = packetElements[1];
            var encryptedMessage = packetElements[2];

            var keyPair = await GenerateKeyPair();

            var decryptedMessage = Sodium.PublicKeyBox
                .Open(
                    Convert.FromBase64String(encryptedMessage),
                    Convert.FromBase64String(nonce),
                    keyPair.PrivateKey,
                    Convert.FromBase64String(publicKey)
                );

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

        public async Task<string> GetQRCodeContent()
        {
            var contactId = await Xamarin.Essentials.SecureStorage.GetAsync(Constants.Identifiers.ContactId);
            var publicKey = await Xamarin.Essentials.SecureStorage.GetAsync(Constants.Identifiers.PublicKey);

            return $"{contactId}|{publicKey}";
        }
    }
}