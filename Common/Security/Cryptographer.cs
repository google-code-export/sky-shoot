using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Common.IO;

namespace Common.Security
{
	/// <summary>
	/// Use this class to encrypt sensitive data and calculate hashes. Class uses SHA256 for hash
	/// calculations and AES for encryption.
	/// </summary>
	public static class Cryptographer
	{
		private static byte[] _key;
		private static byte[] _iv;

		/// <summary>
		/// Initializes the cryptographer infrastructure with the specified configuration values.
		/// </summary>
		/// <param name="key">Encryption key.</param>
		/// <param name="iv">Initialization vector for the encryption algorithm.</param>
		public static void Initialize(byte[] key, byte[] iv)
		{
			Debug.Assert(key != null, "key != null");
			Debug.Assert(iv != null, "iv != null");

			_key = key;
			_iv = iv;

			Trace.TraceInformation("Cryptographic system has been successfully initialized");
		}

		#region Hashing

		/// <summary>
		/// Generates secure hash for the specified string. Use this method whenever you need one
		/// way encryption in the system.
		/// </summary>
		/// <param name="plainString">The plain string to generate hash for.</param>
		/// <param name="salt">Set of random bytes to use as hashing salt.</param>
		/// <returns>Hash for the specified string.</returns>
		public static byte[] GenerateHash(string plainString, byte[] salt = null)
		{
			Debug.Assert(plainString != null, "plainString != null");

			SHA256 hash = SHA256.Create();
			try
			{
				byte[] data = Encoding.UTF8.GetBytes(plainString);
				byte[] dataWithSalt;

				if (null != salt)
				{
					dataWithSalt = new byte[data.Length + salt.Length];

					for (int i = 0; i < data.Length; i++)
						dataWithSalt[i] = data[i];

					for (int i = data.Length, j = 0; i < dataWithSalt.Length; i++, j++)
						dataWithSalt[i] = salt[j];
				}
				else
					dataWithSalt = data;

				return hash.ComputeHash(dataWithSalt);
			}
			finally
			{
				hash.Dispose();
			}
		}

		/// <summary>
		/// Generates encryption key and initialization vector that can be used with Encrypt method.
		/// </summary>
		/// <param name="key">The encryption key to use.</param>
		/// <param name="initializationVector">The initialization vector to use.</param>
		public static void GenerateKey(out byte[] key, out byte[] initializationVector)
		{
			RijndaelManaged keyProvider = new RijndaelManaged();
			key = keyProvider.Key;
			initializationVector = keyProvider.IV;
			keyProvider.Dispose();
		}

		#endregion

		#region Encryption

		/// <summary>
		/// Encrypts the specified data. Use this method along with Decrypt whenever you need two way
		/// encryption in the system.
		/// </summary>
		/// <param name="data">Data to encrypt.</param>
		/// <param name="key">
		/// Key to use for encryption. If <paramref name="key"/> is <see langword="null"/> the default key
		/// will be used.
		/// </param>
		/// <param name="iv">
		/// Initialization vector to use for encryption. If <paramref name="iv"/> is <see langword="null"/>
		/// the default initialization vector will be used.
		/// </param>
		/// <returns>Encrypted string.</returns>
		public static byte[] Encrypt(byte[] data, byte[] key = null, byte[] iv = null)
		{
			Debug.Assert(data != null, "data != null");

			var cryptographer = new RijndaelManaged();
			var buffer = new MemoryStream();

			try
			{
				ICryptoTransform cryptor = cryptographer.CreateEncryptor(key ?? _key, iv ?? _iv);

				CryptoStream cryptedBuffer = new CryptoStream(buffer, cryptor, CryptoStreamMode.Write);
				cryptedBuffer.Write(data, 0, data.Length);
				cryptedBuffer.Dispose();

				return buffer.ToArray();
			}
			finally
			{
				buffer.Dispose();
				cryptographer.Dispose();
			}
		}

		/// <summary>
		/// Encrypts the specified plain string. Use this method along with Decrypt whenever you need two way
		/// encryption in the system.
		/// </summary>
		/// <param name="plainString">The plain string to encrypt.</param>
		/// <param name="key">
		/// Key to use for encryption. If <paramref name="key"/> is <see langword="null"/> the default key
		/// will be used.
		/// </param>
		/// <param name="iv">
		/// Initialization vector to use for encryption. If <paramref name="iv"/> is <see langword="null"/>
		/// the default initialization vector will be used.
		/// </param>
		/// <returns>Encrypted data.</returns>
		public static byte[] Encrypt(string plainString, byte[] key = null, byte[] iv = null)
		{
			Debug.Assert(plainString != null, "plainString != null");

			return Encrypt(Encoding.UTF8.GetBytes(plainString), key, iv);
		}

		/// <summary>
		/// Decrypts the specified bytes array. Use this method along with Encrypt whenever you need two way
		/// encryption in the system.
		/// </summary>
		/// <param name="encryptedData">The encrypted bytes array.</param>
		/// <param name="key">
		/// Key to use for decryption. If <paramref name="key"/> is <see langword="null"/> the default key
		/// will be used.
		/// </param>
		/// <param name="iv">
		/// Initialization vector to use for decryption. If <paramref name="iv"/> is <see langword="null"/>
		/// the default initialization vector will be used.
		/// </param>
		/// <returns>Decrypted string in UTF8 encoding.</returns>
		/// <exception cref="CryptographicException">
		/// Indicates that specified bytes are invalid.
		/// </exception>
		public static byte[] Decrypt(byte[] encryptedData, byte[] key = null, byte[] iv = null)
		{
			Debug.Assert(encryptedData != null, "encryptedData != null");

			RijndaelManaged cryptographer = new RijndaelManaged();
			MemoryStream buffer = new MemoryStream(encryptedData);
			CryptoStream cryptedBuffer = null;

			try
			{
				ICryptoTransform decryptor = cryptographer.CreateDecryptor(key ?? _key, iv ?? _iv);
				cryptedBuffer = new CryptoStream(buffer, decryptor, CryptoStreamMode.Read);

				return cryptedBuffer.ReadToEnd();
			}
			finally
			{
				if (null != cryptedBuffer)
					cryptedBuffer.Dispose();
				buffer.Dispose();
				cryptographer.Dispose();
			}
		}

		/// <summary>
		/// Decrypts the specified encrypted string. Use this method along with Encrypt whenever you need two
		/// way encryption in the system.
		/// </summary>
		/// <param name="encryptedValue">The encrypted string.</param>
		/// <param name="isBase64String">
		/// Indicates whether <paramref name="encryptedValue"/> is a base64 string or a hexadecimal string.
		/// </param>
		/// <returns>Decrypted string in UTF8 encoding.</returns>
		/// <param name="key">
		/// Key to use for decryption. If <paramref name="key"/> is <see langword="null"/> the default key
		/// will be used.
		/// </param>
		/// <param name="iv">
		/// Initialization vector to use for decryption. If <paramref name="iv"/> is <see langword="null"/>
		/// the default initialization vector will be used.
		/// </param>
		/// <exception cref="CryptographicException">
		/// Indicates that specified bytes are invalid.
		/// </exception>
		public static string Decrypt(string encryptedValue,
									 bool isBase64String = true,
									 byte[] key = null,
									 byte[] iv = null)
		{
			Debug.Assert(encryptedValue != null, "encryptedValue != null");

			byte[] encryptedData;
			if (isBase64String)
				encryptedData = Convert.FromBase64String(encryptedValue);
			else
			{
				int numChars = encryptedValue.Length;
				encryptedData = new byte[numChars / 2];
				for (int i = 0; i < numChars; i += 2)
					encryptedData[i / 2] = Convert.ToByte(encryptedValue.Substring(i, 2), 16);
			}

			byte[] rawData = Decrypt(encryptedData, key, iv);

			return Encoding.UTF8.GetString(rawData);
		}

		#endregion
	}
}