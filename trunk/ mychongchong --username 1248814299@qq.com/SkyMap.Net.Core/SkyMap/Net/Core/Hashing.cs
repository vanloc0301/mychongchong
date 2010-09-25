namespace SkyMap.Net.Core
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public class Hashing
    {
        private static string ComputeHash(string inputText, HashingTypes hashingType)
        {
            HashAlgorithm algorithm = getHashAlgorithm(hashingType);
            byte[] bytes = new UTF8Encoding().GetBytes(inputText);
            return Convert.ToBase64String(algorithm.ComputeHash(bytes));
        }

        private static HashAlgorithm getHashAlgorithm(HashingTypes hashingType)
        {
            switch (hashingType)
            {
                case HashingTypes.SHA:
                    return new SHA1CryptoServiceProvider();

                case HashingTypes.SHA256:
                    return new SHA256Managed();

                case HashingTypes.SHA384:
                    return new SHA384Managed();

                case HashingTypes.SHA512:
                    return new SHA512Managed();

                case HashingTypes.MD5:
                    return new MD5CryptoServiceProvider();
            }
            return new MD5CryptoServiceProvider();
        }

        public static string Hash(string inputText)
        {
            return ComputeHash(inputText, HashingTypes.MD5);
        }

        public static string Hash(string inputText, HashingTypes hashingType)
        {
            return ComputeHash(inputText, hashingType);
        }

        public static bool isHashEqual(string inputText, string hashText)
        {
            return (Hash(inputText) == hashText);
        }

        public static bool isHashEqual(string inputText, string hashText, HashingTypes hashingType)
        {
            return (Hash(inputText, hashingType) == hashText);
        }

        public enum HashingTypes
        {
            SHA,
            SHA256,
            SHA384,
            SHA512,
            MD5
        }
    }
}

