using System.Security.Cryptography;
using System.Text;
using System;
using System.IO;
internal class Program

{


    class AESSample
    {


        public static void EncryptTextToFile(string text, string path, byte[] key, byte[] iv)
        {
            try
            {
                // Create or open the specified file.
                using (FileStream fStream = File.Open(path, FileMode.Create))
                // Create a new TripleDES object.
                using (Aes Aes = Aes.Create())
                // Create a TripleDES encryptor from the key and IV
                using (ICryptoTransform encryptor = Aes.CreateEncryptor(key, iv))
                // Create a CryptoStream using the FileStream and encryptor
                using (var cStream = new CryptoStream(fStream, encryptor, CryptoStreamMode.Write))
                {
                    // Convert the provided string to a byte array.
                    byte[] toEncrypt = Encoding.UTF8.GetBytes(text);

                    // Write the byte array to the crypto stream.
                    cStream.Write(toEncrypt, 0, toEncrypt.Length);
                }

                // асинхронное чтение
                using (StreamReader reader = new StreamReader(path, System.Text.Encoding.UTF8))
                {
                    string text1 = reader.ReadToEnd();
                    Console.WriteLine("encrypted text: " + text1);
                }

            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                throw;
            }
        }

        public static string DecryptTextFromFile(string path, byte[] key, byte[] iv)
        {
            try
            {
                // Open the specified file
                using (FileStream fStream = File.OpenRead(path))
                // Create a new TripleDES object.
                using (Aes Aes = Aes.Create())
                // Create a TripleDES decryptor from the key and IV
                using (ICryptoTransform decryptor = Aes.CreateDecryptor(key, iv))
                // Create a CryptoStream using the FileStream and decryptor
                using (var cStream = new CryptoStream(fStream, decryptor, CryptoStreamMode.Read))
                // Create a StreamReader to turn the bytes back into text
                using (StreamReader reader = new StreamReader(cStream, Encoding.UTF8))
                {
                    // Read the stream as a string
                    return reader.ReadToEnd();
                }


            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                throw;
            }
        }


        static void Main()
        {
            try
            {
                byte[] key;
                byte[] iv;

                // Create a new TripleDES object to generate a random key
                // and initialization vector (IV).
                using (Aes Aes = Aes.Create())
                {
                    key = Aes.Key;
                    iv = Aes.IV;
                }

                // Create a string to encrypt.
                string original = "Palaznik Arseniy";

                // The name/path of the file to write.
                string filename = "CText.txt";
                Console.WriteLine("Original text: {0}", original);
                // Encrypt the string to a file.
                EncryptTextToFile(original, filename, key, iv);

                // Decrypt the file back to a string.
                string decrypted = DecryptTextFromFile(filename, key, iv);
                // Display the decrypted string to the console.
                Console.WriteLine("Decrypted text: " + decrypted);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            byte[] hashValue = new byte[128];

            using (SHA1 sha = SHA1.Create())
            {
                hashValue = sha.ComputeHash(File.ReadAllBytes("CText.txt"));
                File.WriteAllBytes("hash.txt", hashValue);

            }

        }
    }

}

