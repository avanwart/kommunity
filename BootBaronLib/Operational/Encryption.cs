//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Configuration;

///<see>http://69.10.233.10/KB/recipes/Encrypt_an_string.aspx</see>

namespace BootBaronLib.Operational.Encryption
{
    /// <summary>
    /// RSA Encryption Services
    /// </summary>
    public class RSAEncryption
    {
        public RSAEncryption() { }

        /// <summary>
        /// Encryption to RSA format
        /// </summary>
        /// <param name="strPublicKeyXML"></param>
        /// <param name="strDataToEncrypt"></param>
        /// <returns></returns>
        public static String EncryptData(String strPublicKeyXML, String strDataToEncrypt)
        {
            using (RSACryptoServiceProvider toRSA = new RSACryptoServiceProvider(2048))
            {
                toRSA.FromXmlString(strPublicKeyXML);

                return Convert.ToBase64String(toRSA.Encrypt(Encoding.UTF8.GetBytes(strDataToEncrypt), true));
            }
        }
    }




    /// <summary>
    /// This is the main class to use
    /// </summary>
    public class EncryptionAndDecryption
    {

        /// Converts a hexadecimal string to a byte array. Used to convert encryption key values from the configuration.
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        private static byte[] HexToByte(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
            {
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            return returnBytes;
        }


        public static string HMACSHA1(string password, MachineKeySection machineKey)
        {
            HMACSHA1 hash = new HMACSHA1 { Key = HexToByte(machineKey.ValidationKey) };
         
            return  Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(password)));

        }


        /// <summary>
        /// Create an md5 sum string of this string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// <see cref="http://blog.stevex.net/?page_id=529"/>
         public static string GetMd5Sum(string str)
        {
            // First we need to convert the string into bytes, which
            // means using a text encoder.
            Encoder enc =  Encoding.ASCII.GetEncoder();

            // Create a buffer large enough to hold the string
            byte[] unicodeText = new byte[str.Length * 2];
            enc.GetBytes(str.ToCharArray(), 0, str.Length, unicodeText, 0, true);

            // Now that we have a byte array we can ask the CSP to hash it
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(unicodeText);

            // Build the final string by converting each byte
            // into hex and appending it to a StringBuilder
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString("X2"));
            }

            // And return it
            return sb.ToString();
        }

         public static string CreateMD5Hash(string input)
         {
             // Use input string to calculate MD5 hash
             MD5 md5 = System.Security.Cryptography.MD5.Create();
             byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
             byte[] hashBytes = md5.ComputeHash(inputBytes);

             // Convert the byte array to hexadecimal string
             StringBuilder sb = new StringBuilder();
             for (int i = 0; i < hashBytes.Length; i++)
             {
                 sb.Append(hashBytes[i].ToString("X2"));
                 // To force the hex string to lower-case letters instead of
                 // upper-case, use he following line instead:
                 // sb.Append(hashBytes[i].ToString("x2")); 
             }
             return sb.ToString();
         }

        ///// <summary>
        ///// Get the MD5 hash
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        // /// <see>http://blog.brezovsky.net/en-text-2.html</see>
        // public static string GetMD5Hash(string input)
        // {
        //     MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
        //    // byte[] bs = Encoding.UTF8.GetBytes(input);
        //     byte[] bs = Encoding.ASCII.GetBytes(input);
        //     bs = x.ComputeHash(bs);
        //     StringBuilder s = new StringBuilder();
        //     foreach (byte b in bs)
        //     {
        //         s.Append(b.ToString("x2").ToLower());
        //     }
        //     return s.ToString();
        // }

         public static string GetMD5Hash(string originalPassword)
         {
             //Declarations
             Byte[] originalBytes;
             Byte[] encodedBytes;
             MD5 md5;

             //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)
             md5 = new MD5CryptoServiceProvider();
             originalBytes = ASCIIEncoding.Default.GetBytes(originalPassword);
             encodedBytes = md5.ComputeHash(originalBytes);

             //Convert encoded bytes back to a 'readable' string
             return BitConverter.ToString(encodedBytes);
         }

         #region Encrypt/ Decrypt String

         /// <summary>
         /// Encrypt a string
         /// </summary>
         /// <param name="theString"></param>
         /// <param name="privateKey"></param>
         /// <returns></returns>
         public static string EncryptMaker(string theString, string privateKey, int encryptionType, out string intVect)
         {
             //0 - DES
             //1 - RC2 
             //2 - Rijndeal
             //3 - TripleDes
              EncryptionAlgorithm alg = ( EncryptionAlgorithm)encryptionType;

             string result = string.Empty;
             string iv = string.Empty;

             try
             {
                 Encryptor en = new Encryptor(alg, privateKey);
                 result = en.Encrypt(theString);
                 iv = Convert.ToBase64String(en.IV);

             }
             catch (Exception ex)
             {
                Utilities.LogError(ex);
             }
             intVect = iv;
             return result;
         }

        /// <summary>
        /// Returns a 4 pipe deliminted string for the 3 values
        /// </summary>
        /// <param name="unencrypted"></param>
        /// <returns></returns>
         public static string EncryptMaker(string unencrypted)
         {
            string intVector;
            string privateKey;
            string key3;

            EncryptMaker(unencrypted, out intVector, out privateKey, out key3);

            return string.Format("{0}|||{1}|||{2}", intVector, privateKey, key3);
         }

         public static void EncryptMaker(
             string unencrypted,
             out string intVector,
             out string privateKey, 
             out string key3)
         {
             // make 2 random numbers
             int randNum1 = new Random().Next(100000000, 999999999);
             int randNum2 = new Random().Next(100000000, 999999999);


             privateKey = BootBaronLib.Operational.Encryption.EncryptionAndDecryption.EncryptMaker(
                Convert.ToString(randNum1),
                Convert.ToString(randNum2),
                1,
                out intVector);

             key3 = Encryption.EncryptionAndDecryption.EncryptMaker(unencrypted, 
                privateKey, 2, out intVector);
 
         }


         /// <summary>
         /// Decrypt 
         /// </summary>
         /// <param name="theString">the encoded value</param>
         /// <param name="privateKey">the key</param>
         /// <param name="intVect">the initialization vector</param>
         /// <returns>the unencrypted string</returns>
         public static string DecryptMaker(string theString, string privateKey, int encryptionType, string intVect)
         {
             //0 - DES
             //1 - RC2 
             //2 - Rijndeal
             //3 - TripleDes
              EncryptionAlgorithm alg = ( EncryptionAlgorithm)encryptionType;
             string result = string.Empty;
             try
             {
                 byte[] IV = Convert.FromBase64String(intVect);
                 Decryptor dec = new Decryptor(alg, IV);
                 result = dec.Decrypt(theString, privateKey);
             }
             catch  
             {
//                 LogError(ex);
             }

             return result;
         }


         public static string DecryptMaker(string theString, string privateKey, string intVect)
         {
             //0 - DES
             //1 - RC2 
             //2 - Rijndeal
             //3 - TripleDes
             EncryptionAlgorithm alg = (EncryptionAlgorithm)2;
             string result = string.Empty;
             try
             {
                 byte[] IV = Convert.FromBase64String(intVect);
                 Decryptor dec = new Decryptor(alg, IV);
                 result = dec.Decrypt(theString, privateKey);
             }
             catch 
             {
                 //                 LogError(ex);
             }

             return result;
         }

        /// <summary>
        /// Decrypt the triple pipe delimited string
        /// </summary>
        /// <param name="encrypted"></param>
        /// <returns></returns>
         /// <see cref="http://dotnetperls.com/string-split"/>
         /// <see cref="http://www.webdeveloper.com/forum/showthread.php?t=134508"/>
         public static string DecryptMaker(string encrypted)
         {
             try
             {
                 // intVector, privateKey, key3);
                 string[] parts = Regex.Split(encrypted, @"\|\|\|");

                 return DecryptMaker(parts[2], parts[1],  parts[0] );
             }
             catch  
             {
                 return string.Empty;
             }
         }


 
         #endregion 

 
    }

    /// <summary>
    /// This class can generate random passwords, which do not include ambiguous 
    /// characters, such as I, l, and 1. The generated password will be made of
    /// 7-bit ASCII symbols. Every four characters will include one lower case
    /// character, one upper case character, one number, and one special symbol
    /// (such as '%') in a random order. The password will always start with an
    /// alpha-numeric character; it will not start with a special symbol (we do
    /// this because some back-end systems do not like certain special
    /// characters in the first position).
    /// </summary>
    /// <see>http://www.obviex.com/Samples/Password.aspx</see>
    public class RandomPassword
    {
        // Define default min and max password lengths.
        private static int DEFAULT_MIN_PASSWORD_LENGTH = 8;
        private static int DEFAULT_MAX_PASSWORD_LENGTH = 10;

        // Define supported password characters divided into groups.
        // You can add (or remove) characters to (from) these groups.
        private static string PASSWORD_CHARS_LCASE = "abcdefgijkmnopqrstwxyz";
        private static string PASSWORD_CHARS_UCASE = "ABCDEFGHJKLMNPQRSTWXYZ";
        private static string PASSWORD_CHARS_NUMERIC = "23456789";
        //private static string PASSWORD_CHARS_SPECIAL= "*$-+?_&=!%{}/";

        /// <summary>
        /// Generates a random password.
        /// </summary>
        /// <returns>
        /// Randomly generated password.
        /// </returns>
        /// <remarks>
        /// The length of the generated password will be determined at
        /// random. It will be no shorter than the minimum default and
        /// no longer than maximum default.
        /// </remarks>
        public static string Generate()
        {
            return Generate(DEFAULT_MIN_PASSWORD_LENGTH,
                            DEFAULT_MAX_PASSWORD_LENGTH);
        }

        /// <summary>
        /// Generates a random password of the exact length.
        /// </summary>
        /// <param name="length">
        /// Exact password length.
        /// </param>
        /// <returns>
        /// Randomly generated password.
        /// </returns>
        public static string Generate(int length)
        {
            return Generate(length, length);
        }

        /// <summary>
        /// Generates a random password.
        /// </summary>
        /// <param name="minLength">
        /// Minimum password length.
        /// </param>
        /// <param name="maxLength">
        /// Maximum password length.
        /// </param>
        /// <returns>
        /// Randomly generated password.
        /// </returns>
        /// <remarks>
        /// The length of the generated password will be determined at
        /// random and it will fall with the range determined by the
        /// function parameters.
        /// </remarks>
        public static string Generate(int minLength,
                                      int maxLength)
        {
            // Make sure that input parameters are valid.
            if (minLength <= 0 || maxLength <= 0 || minLength > maxLength)
                return null;

            // Create a local array containing supported password characters
            // grouped by types. You can remove character groups from this
            // array, but doing so will weaken the password strength.
            char[][] charGroups = new char[][] 
        {
            PASSWORD_CHARS_LCASE.ToCharArray(),
            PASSWORD_CHARS_UCASE.ToCharArray(),
            PASSWORD_CHARS_NUMERIC.ToCharArray(),
            //PASSWORD_CHARS_SPECIAL.ToCharArray()
        };

            // Use this array to track the number of unused characters in each
            // character group.
            int[] charsLeftInGroup = new int[charGroups.Length];

            // Initially, all characters in each group are not used.
            for (int i = 0; i < charsLeftInGroup.Length; i++)
                charsLeftInGroup[i] = charGroups[i].Length;

            // Use this array to track (iterate through) unused character groups.
            int[] leftGroupsOrder = new int[charGroups.Length];

            // Initially, all character groups are not used.
            for (int i = 0; i < leftGroupsOrder.Length; i++)
                leftGroupsOrder[i] = i;

            // Because we cannot use the default randomizer, which is based on the
            // current time (it will produce the same "random" number within a
            // second), we will use a random number generator to seed the
            // randomizer.

            // Use a 4-byte array to fill it with random bytes and convert it then
            // to an integer value.
            byte[] randomBytes = new byte[4];

            // Generate 4 random bytes.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);

            // Convert 4 bytes into a 32-bit integer value.
            int seed = (randomBytes[0] & 0x7f) << 24 |
                        randomBytes[1] << 16 |
                        randomBytes[2] << 8 |
                        randomBytes[3];

            // Now, this is real randomization.
            Random random = new Random(seed);

            // This array will hold password characters.
            char[] password = null;

            // Allocate appropriate memory for the password.
            if (minLength < maxLength)
                password = new char[random.Next(minLength, maxLength + 1)];
            else
                password = new char[minLength];

            // Index of the next character to be added to password.
            int nextCharIdx;

            // Index of the next character group to be processed.
            int nextGroupIdx;

            // Index which will be used to track not processed character groups.
            int nextLeftGroupsOrderIdx;

            // Index of the last non-processed character in a group.
            int lastCharIdx;

            // Index of the last non-processed group.
            int lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;

            // Generate password characters one at a time.
            for (int i = 0; i < password.Length; i++)
            {
                // If only one character group remained unprocessed, process it;
                // otherwise, pick a random character group from the unprocessed
                // group list. To allow a special character to appear in the
                // first position, increment the second parameter of the Next
                // function call by one, i.e. lastLeftGroupsOrderIdx + 1.
                if (lastLeftGroupsOrderIdx == 0)
                    nextLeftGroupsOrderIdx = 0;
                else
                    nextLeftGroupsOrderIdx = random.Next(0,
                                                         lastLeftGroupsOrderIdx);

                // Get the actual index of the character group, from which we will
                // pick the next character.
                nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];

                // Get the index of the last unprocessed characters in this group.
                lastCharIdx = charsLeftInGroup[nextGroupIdx] - 1;

                // If only one unprocessed character is left, pick it; otherwise,
                // get a random character from the unused character list.
                if (lastCharIdx == 0)
                    nextCharIdx = 0;
                else
                    nextCharIdx = random.Next(0, lastCharIdx + 1);

                // Add this character to the password.
                password[i] = charGroups[nextGroupIdx][nextCharIdx];

                // If we processed the last character in this group, start over.
                if (lastCharIdx == 0)
                    charsLeftInGroup[nextGroupIdx] =
                                              charGroups[nextGroupIdx].Length;
                // There are more unprocessed characters left.
                else
                {
                    // Swap processed character with the last unprocessed character
                    // so that we don't pick it until we process all characters in
                    // this group.
                    if (lastCharIdx != nextCharIdx)
                    {
                        char temp = charGroups[nextGroupIdx][lastCharIdx];
                        charGroups[nextGroupIdx][lastCharIdx] =
                                    charGroups[nextGroupIdx][nextCharIdx];
                        charGroups[nextGroupIdx][nextCharIdx] = temp;
                    }
                    // Decrement the number of unprocessed characters in
                    // this group.
                    charsLeftInGroup[nextGroupIdx]--;
                }

                // If we processed the last group, start all over.
                if (lastLeftGroupsOrderIdx == 0)
                    lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
                // There are more unprocessed groups left.
                else
                {
                    // Swap processed group with the last unprocessed group
                    // so that we don't pick it until we process all groups.
                    if (lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx)
                    {
                        int temp = leftGroupsOrder[lastLeftGroupsOrderIdx];
                        leftGroupsOrder[lastLeftGroupsOrderIdx] =
                                    leftGroupsOrder[nextLeftGroupsOrderIdx];
                        leftGroupsOrder[nextLeftGroupsOrderIdx] = temp;
                    }
                    // Decrement the number of unprocessed groups.
                    lastLeftGroupsOrderIdx--;
                }
            }

            // Convert password characters into a string and return the result.
            return new string(password);
        }
    }

    public class DecryptTransformer
    {
        EncryptionAlgorithm algorithmID;
        string SecurityKey = "";
        Byte[] IV;
        bool bHasIV = false;
        public DecryptTransformer( EncryptionAlgorithm algID)
        {
            algorithmID = algID;
        }
        public DecryptTransformer( EncryptionAlgorithm algID, byte[] iv)
        {
            algorithmID = algID;
            IV = iv;
            bHasIV = true;
        }

        public EncryptionAlgorithm EncryptionAlgorithm
        {
            get
            {
                return algorithmID;
            }
            set
            {
                algorithmID = value;
            }
        }

        public void SetSecurityKey(string Key)
        {
            SecurityKey = Key;
        }
        public ICryptoTransform GetCryptoTransform()
        {
            bool bHasSecuityKey = false;
            if (SecurityKey.Length != 0)
                bHasSecuityKey = true;

            byte[] key = Encoding.ASCII.GetBytes(SecurityKey);
            switch (algorithmID)
            {
                case EncryptionAlgorithm.DES:
                    DES des = new DESCryptoServiceProvider();
                    if (bHasSecuityKey) des.Key = key;
                    if (bHasIV) des.IV = IV;
                    return des.CreateDecryptor();

                case EncryptionAlgorithm.Rc2:
                    RC2 rc = new RC2CryptoServiceProvider();
                    if (bHasSecuityKey) rc.Key = key;
                    if (bHasIV) rc.IV = IV;
                    return rc.CreateDecryptor();
                case EncryptionAlgorithm.Rijndael:
                    Rijndael rj = new RijndaelManaged();
                    if (bHasSecuityKey) rj.Key = key;
                    if (bHasIV) rj.IV = IV; ;
                    return rj.CreateDecryptor();
                case EncryptionAlgorithm.TripleDes:
                    TripleDES tDes = new TripleDESCryptoServiceProvider();
                    if (bHasSecuityKey) tDes.Key = key;
                    if (bHasIV) tDes.IV = IV;
                    return tDes.CreateDecryptor();
                default:
                    throw new CryptographicException("Algorithm ID '" + algorithmID + "' not supported.");
            }
        }

    }

    public class Decryptor
    {
         EncryptionAlgorithm AlgoritmID;
        byte[] IV;
        public Decryptor( EncryptionAlgorithm algID)
        {
            AlgoritmID = algID;
        }
        public Decryptor( EncryptionAlgorithm algID, byte[] iv)
        {
            AlgoritmID = algID;
            IV = iv;
        }

        public DecryptTransformer DecryptTransformer
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public string Decrypt(string MainString, string key)
        {
            DecryptTransformer dt = new DecryptTransformer(AlgoritmID, IV);
            dt.SetSecurityKey(key);

            byte[] buffer = Convert.FromBase64String(MainString.Trim());
            MemoryStream ms = new MemoryStream(buffer);

            // Create a CryptoStream using the memory stream and the 
            // CSP DES key. 
            CryptoStream encStream = new CryptoStream(ms, dt.GetCryptoTransform(), CryptoStreamMode.Read);

            // Create a StreamReader for reading the stream.
            StreamReader sr = new StreamReader(encStream);

            // Read the stream as a string.
            string val = sr.ReadLine();

            // Close the streams.
            sr.Close();
            encStream.Close();
            ms.Close();
            ms.Dispose();

            return val;


        }
    }

    public class Encryptor
    {
        EncryptEngine engin;
        public byte[] IV;
        public Encryptor( EncryptionAlgorithm algID, string key)
        {
            engin = new EncryptEngine(algID, key);
        }

        public EncryptEngine EncryptEngine
        {
            get
            {
                return engin;
            }
            set
            {
                engin = value;
            }
        }

        public string Encrypt(string MainString)
        {
            MemoryStream memory = new MemoryStream();
            CryptoStream stream = new CryptoStream(memory, engin.GetCryptTransform(), CryptoStreamMode.Write);
            StreamWriter streamwriter = new StreamWriter(stream);
            streamwriter.WriteLine(MainString);
            streamwriter.Close();
            stream.Close();
            IV = engin.Vector;
            byte[] buffer = memory.ToArray();
            memory.Close();
            memory.Dispose();
            return Convert.ToBase64String(buffer);

        }
    }

    public class EncryptEngine
    {
        bool bWithKey = false;
         EncryptionAlgorithm AlgoritmID;
        string keyword = "";
        public byte[] Vector;
        public EncryptEngine( EncryptionAlgorithm AlgID, string Key)
        {
            if (Key.Length == 0)
                bWithKey = false;
            else
                bWithKey = true;

            keyword = Key;
            AlgoritmID = AlgID;
        }

        public  EncryptionAlgorithm EncryptionAlgorithm
        {
            get
            {
                return AlgoritmID;
            }
            set
            {
                AlgoritmID = value;
            }
        }

        public ICryptoTransform GetCryptTransform()
        {
            byte[] key = Encoding.ASCII.GetBytes(keyword);

            switch (AlgoritmID)
            {
                case EncryptionAlgorithm.DES:
                    DES des = new DESCryptoServiceProvider();
                    des.Mode = CipherMode.CBC;
                    if (bWithKey) des.Key = key;
                    Vector = des.IV;
                    return des.CreateEncryptor();
                case EncryptionAlgorithm.Rc2:
                    RC2 rc = new RC2CryptoServiceProvider();
                    rc.Mode = CipherMode.CBC;
                    if (bWithKey) rc.Key = key;
                    Vector = rc.IV;
                    return rc.CreateEncryptor();
                case EncryptionAlgorithm.Rijndael:
                    Rijndael rj = new RijndaelManaged();
                    rj.Mode = CipherMode.CBC;
                    if (bWithKey) rj.Key = key;
                    Vector = rj.IV;
                    return rj.CreateEncryptor();
                case EncryptionAlgorithm.TripleDes:
                    TripleDES tDes = new TripleDESCryptoServiceProvider();
                    tDes.Mode = CipherMode.CBC;
                    if (bWithKey) tDes.Key = key;
                    Vector = tDes.IV;
                    return tDes.CreateEncryptor();
                default:
                    throw new CryptographicException("Algorithm " + AlgoritmID + " Not Supported!");
            }
        }
        public static bool ValidateKeySize( EncryptionAlgorithm algID, int Lenght)
        {
            switch (algID)
            {
                case EncryptionAlgorithm.DES:
                    DES des = new DESCryptoServiceProvider();
                    return des.ValidKeySize(Lenght);
                case EncryptionAlgorithm.Rc2:
                    RC2 rc = new RC2CryptoServiceProvider();
                    return rc.ValidKeySize(Lenght);
                case EncryptionAlgorithm.Rijndael:
                    Rijndael rj = new RijndaelManaged();
                    return rj.ValidKeySize(Lenght);
                case EncryptionAlgorithm.TripleDes:
                    TripleDES tDes = new TripleDESCryptoServiceProvider();
                    return tDes.ValidKeySize(Lenght);
                default:
                    throw new CryptographicException("Algorithm " + algID + " Not Supported!");
            }
        }
    }

    public enum EncryptionAlgorithm
    {
        DES = 0, Rc2, Rijndael, TripleDes
    }

}