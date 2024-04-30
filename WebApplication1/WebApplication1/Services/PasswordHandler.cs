using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;
using System.Text;

namespace WeatherResearcher.Services
{
	public class PasswordHandler
	{
		byte[] salt;
		private readonly byte[] encryptionKey = {1,255,44,88,23,34,54,2,145,178,0,0,163,14,3,177};
		public string HashPassword(string password, byte[] salt)
		{
			this.salt = salt;
			string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
				password: password,
				salt: salt,
				prf: KeyDerivationPrf.HMACSHA1,
				iterationCount: 1000,
				numBytesRequested: 256 / 8));

			return hashed;
		}

		public string EncryptString(string text)
		{
			byte[] keyBytes = encryptionKey;
			using (var aes = Aes.Create())
			{
				aes.Key = keyBytes;
				aes.GenerateIV(); // Generate a random IV for each encryption
				byte[] textBytes = Encoding.UTF8.GetBytes(text);
				byte[] encryptedBytes;

				using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
				{
					encryptedBytes = encryptor.TransformFinalBlock(textBytes, 0, textBytes.Length);
				}

				// Combine IV and encrypted data for decryption
				byte[] combinedBytes = new byte[aes.IV.Length + encryptedBytes.Length];
				Array.Copy(aes.IV, 0, combinedBytes, 0, aes.IV.Length);
				Array.Copy(encryptedBytes, 0, combinedBytes, aes.IV.Length, encryptedBytes.Length);

				return Convert.ToBase64String(combinedBytes);
			}
		}

		public string DecryptString(string encryptedText)
		{
			byte[] encryptedBytesWithIV = Convert.FromBase64String(encryptedText);
			byte[] encryptedIV = new byte[16];
			byte[] encryptedBytes = new byte[encryptedBytesWithIV.Length - 16];

			Array.Copy(encryptedBytesWithIV, 0, encryptedIV, 0, 16);
			Array.Copy(encryptedBytesWithIV, 16, encryptedBytes, 0, encryptedBytes.Length);

			using (var aes = Aes.Create())
			{
				aes.Key = encryptionKey;
				aes.IV = encryptedIV;

				using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
				using (var memoryStream = new MemoryStream(encryptedBytes))
				using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
				using (var streamReader = new StreamReader(cryptoStream))
				{
					return streamReader.ReadToEnd();
				}
			}
		}
	}
}