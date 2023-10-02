using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Net; 

namespace devkit
{
    public class main
    {
        static string EncryptString(string plainText, string password)
        {
            byte[] salt = GenerateRandomSalt();
            byte[] key = new Rfc2898DeriveBytes(password, salt).GetBytes(32);
            byte[] encryptedBytes;
            using (var rijndael = new RijndaelManaged())
            {
                rijndael.KeySize = 256;
                rijndael.BlockSize = 128;
                rijndael.Mode = CipherMode.CBC;

                using (var encryptor = rijndael.CreateEncryptor(key, salt))
                {
                    byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var cryptoStream = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                        }
                        encryptedBytes = msEncrypt.ToArray(); // Moved this line inside the using block
                    }

                    byte[] result = new byte[salt.Length + encryptedBytes.Length];

                    Array.Copy(salt, 0, result, 0, salt.Length);
                    Array.Copy(encryptedBytes, 0, result, salt.Length, encryptedBytes.Length);

                    return Convert.ToBase64String(result);
                }
            }
        }

        static string DecryptString(string encryptedText, string password)
        {
            byte[] allBytes = Convert.FromBase64String(encryptedText);
            byte[] salt = new byte[32]; // Assuming the salt size is known
            byte[] encryptedBytes = new byte[allBytes.Length - salt.Length];

            Array.Copy(allBytes, 0, salt, 0, salt.Length);
            Array.Copy(allBytes, salt.Length, encryptedBytes, 0, encryptedBytes.Length);

            byte[] key = new Rfc2898DeriveBytes(password, salt).GetBytes(32);

            using (var rijndael = new RijndaelManaged())
            {
                rijndael.KeySize = 256;
                rijndael.BlockSize = 128;
                rijndael.Mode = CipherMode.CBC;

                using (var decryptor = rijndael.CreateDecryptor(key, salt))
                {
                    using (var msDecrypt = new MemoryStream(encryptedBytes))
                    {
                        using (var cryptoStream = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(cryptoStream))
                            {
                                return srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }

        public static string GetPublicIP()
        {
            try
            {
                String address = "";
                WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
                using (WebResponse response = request.GetResponse())
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    address = stream.ReadToEnd();
                }

                int first = address.IndexOf("Address: ") + 9;
                int last = address.LastIndexOf("</body>");
                address = address.Substring(first, last - first);

                return address;
            }
            catch
            {
                return "fail";
            }
        }
        public static string GetUniqueIdentifier()
        {
            Guid myuuid = Guid.NewGuid();
            string input = EncryptString(myuuid.ToString(), myuuid.ToString());

            // Convert ASCII to hex
            byte[] asciiBytes = Encoding.ASCII.GetBytes(input);
            string hexString = BitConverter.ToString(asciiBytes).Replace("-", "");

            // Calculate MD5 hash
            using (MD5 md5 = MD5.Create())
            {
                byte[] data = Encoding.UTF8.GetBytes(hexString);
                byte[] hashBytes = md5.ComputeHash(data);

                // Convert hash to hex string
                StringBuilder hashBuilder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    hashBuilder.Append(b.ToString("x2"));
                }

                string md5Hash = hashBuilder.ToString();
                return md5Hash;
            }
        }

        public static void EncryptFile(string inputFile, string outputFile, string password)
        {
            byte[] salt = GenerateRandomSalt();
            byte[] key = new Rfc2898DeriveBytes(password, salt).GetBytes(32);

            using (var rijndael = new RijndaelManaged())
            {
                rijndael.KeySize = 256;
                rijndael.BlockSize = 128;
                rijndael.Mode = CipherMode.CBC;

                using (var encryptor = rijndael.CreateEncryptor(key, salt))
                {
                    using (var inputStream = new FileStream(inputFile, FileMode.Open))
                    {
                        using (var outputStream = new FileStream(outputFile, FileMode.Create))
                        {
                            outputStream.Write(salt, 0, salt.Length);

                            using (var cryptoStream = new CryptoStream(outputStream, encryptor, CryptoStreamMode.Write))
                            {
                                byte[] buffer = new byte[1024];
                                int bytesRead;

                                while ((bytesRead = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    cryptoStream.Write(buffer, 0, bytesRead);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void DecryptFile(string inputFile, string outputFile, string password)
        {
            using (var rijndael = new RijndaelManaged())
            {
                rijndael.KeySize = 256;
                rijndael.BlockSize = 128;
                rijndael.Mode = CipherMode.CBC;

                using (var inputStream = new FileStream(inputFile, FileMode.Open))
                {
                    byte[] salt = new byte[32];
                    inputStream.Read(salt, 0, salt.Length);

                    byte[] key = new Rfc2898DeriveBytes(password, salt).GetBytes(32);

                    using (var decryptor = rijndael.CreateDecryptor(key, salt))
                    {
                        using (var outputStream = new FileStream(outputFile, FileMode.Create))
                        {
                            using (var cryptoStream = new CryptoStream(inputStream, decryptor, CryptoStreamMode.Read))
                            {
                                byte[] buffer = new byte[1024];
                                int bytesRead;

                                while ((bytesRead = cryptoStream.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    outputStream.Write(buffer, 0, bytesRead);
                                }
                            }
                        }
                    }
                }
            }
        }

        private static byte[] GenerateRandomSalt()
        {
            byte[] salt = new byte[32];

            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            
            rng.GetBytes(salt);

            return salt;
        }

        public static string EncodeString64(string input)
        {
            byte[] output = System.Text.Encoding.UTF8.GetBytes(input);
            return System.Convert.ToBase64String(output);
        }

        public static string DecodeString64(string input)
        {
            byte[] output = System.Convert.FromBase64String(input);
            return System.Text.Encoding.UTF8.GetString(output);
        }

        public static string HttpGet(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return response + " -@- " + responseString;
        }

        public static string HttpPost(string url, Dictionary<string, string> postData)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);

            StringBuilder postDataString = new StringBuilder();
            foreach (var kvp in postData)
            {
                postDataString.AppendFormat("{0}={1}&", Uri.EscapeDataString(kvp.Key), Uri.EscapeDataString(kvp.Value));
            }

            byte[] data = Encoding.ASCII.GetBytes(postDataString.ToString());

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return response + " -@- " + responseString;
        }
     }
}
