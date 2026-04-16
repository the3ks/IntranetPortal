using System.Security.Cryptography;
using System.Text;

namespace IntranetPortal.Api.Security
{
    public interface IChallengeCryptoService
    {
        string Algorithm { get; }
        string ExportPublicKeyPem();
        string DecryptPassword(string encryptedPasswordBase64);
    }

    public sealed class ChallengeCryptoService : IChallengeCryptoService, IDisposable
    {
        private readonly RSA _rsa;

        public string Algorithm => "RSA-OAEP-256+OneTimeChallenge-v1";

        public ChallengeCryptoService(IConfiguration configuration)
        {
            _rsa = RSA.Create();

            var privateKeyPem = configuration["ChallengeEncryption:PrivateKeyPem"];
            if (!string.IsNullOrWhiteSpace(privateKeyPem))
            {
                _rsa.ImportFromPem(privateKeyPem);
            }
            else
            {
                _rsa.KeySize = 2048;
            }
        }

        public string ExportPublicKeyPem()
        {
            return _rsa.ExportSubjectPublicKeyInfoPem();
        }

        public string DecryptPassword(string encryptedPasswordBase64)
        {
            var cipherBytes = Convert.FromBase64String(encryptedPasswordBase64);
            var plainBytes = _rsa.Decrypt(cipherBytes, RSAEncryptionPadding.OaepSHA256);
            return Encoding.UTF8.GetString(plainBytes);
        }

        public void Dispose()
        {
            _rsa.Dispose();
        }
    }
}