namespace SkyMap.Net.Core
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using System.Xml;

    public sealed class CryptoHelper
    {
        private const CryptoTypes CRYPT_DEFAULT_METHOD = CryptoTypes.encTypeRijndael;
        private const string CRYPT_DEFAULT_PASSWORD = "abcd!@#";
        private CryptoTypes mCryptoType;
        private byte[] mIV;
        private byte[] mKey;
        private string mPassword;
        private byte[] SaltByteArray;

        public CryptoHelper()
        {
            this.mKey = new byte[] { 
                1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 0x10, 
                0x11, 0x12, 0x13, 20, 0x15, 0x16, 0x17, 0x18
             };
            this.mIV = new byte[] { 0x41, 110, 0x44, 0x1a, 0x45, 0xb2, 200, 0xdb };
            this.SaltByteArray = new byte[] { 0x49, 0x76, 0x61, 110, 0x20, 0x4d, 0x65, 100, 0x76, 0x65, 100, 0x65, 0x76 };
            this.mCryptoType = CryptoTypes.encTypeRijndael;
            this.mPassword = "abcd!@#";
            this.calculateNewKeyAndIV();
        }

        public CryptoHelper(CryptoTypes CryptoType)
        {
            this.mKey = new byte[] { 
                1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 0x10, 
                0x11, 0x12, 0x13, 20, 0x15, 0x16, 0x17, 0x18
             };
            this.mIV = new byte[] { 0x41, 110, 0x44, 0x1a, 0x45, 0xb2, 200, 0xdb };
            this.SaltByteArray = new byte[] { 0x49, 0x76, 0x61, 110, 0x20, 0x4d, 0x65, 100, 0x76, 0x65, 100, 0x65, 0x76 };
            this.mCryptoType = CryptoTypes.encTypeRijndael;
            this.mPassword = "abcd!@#";
            this.CryptoType = CryptoType;
        }

        private void calculateNewKeyAndIV()
        {
            PasswordDeriveBytes bytes = new PasswordDeriveBytes(this.mPassword, this.SaltByteArray);
            SymmetricAlgorithm algorithm = this.selectAlgorithm();
            this.mKey = bytes.GetBytes(algorithm.KeySize / 8);
            this.mIV = bytes.GetBytes(algorithm.BlockSize / 8);
        }

        public string Decrypt(string inputText)
        {
            if (string.IsNullOrEmpty(inputText))
            {
                return string.Empty;
            }
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] inputBytes = Convert.FromBase64String(inputText);
            return encoding.GetString(this.EncryptDecrypt(inputBytes, false));
        }

        public string Decrypt(string inputText, CryptoTypes cryptoType)
        {
            this.CryptoType = cryptoType;
            return this.Decrypt(inputText);
        }

        public string Decrypt(string inputText, string password)
        {
            this.Password = password;
            return this.Decrypt(inputText);
        }

        public string Decrypt(string inputText, string password, CryptoTypes cryptoType)
        {
            this.mCryptoType = cryptoType;
            return this.Decrypt(inputText, password);
        }

        public string Encrypt(string inputText)
        {
            if (string.IsNullOrEmpty(inputText))
            {
                return string.Empty;
            }
            byte[] bytes = new UTF8Encoding().GetBytes(inputText);
            return Convert.ToBase64String(this.EncryptDecrypt(bytes, true));
        }

        public string Encrypt(string inputText, CryptoTypes cryptoType)
        {
            this.CryptoType = cryptoType;
            return this.Encrypt(inputText);
        }

        public string Encrypt(string inputText, string password)
        {
            this.Password = password;
            return this.Encrypt(inputText);
        }

        public string Encrypt(string inputText, string password, CryptoTypes cryptoType)
        {
            this.mCryptoType = cryptoType;
            return this.Encrypt(inputText, password);
        }

        private byte[] EncryptDecrypt(byte[] inputBytes, bool Encrpyt)
        {
            byte[] buffer2;
            ICryptoTransform transform = this.getCryptoTransform(Encrpyt);
            MemoryStream stream = new MemoryStream();
            try
            {
                CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Write);
                stream2.Write(inputBytes, 0, inputBytes.Length);
                stream2.FlushFinalBlock();
                byte[] buffer = stream.ToArray();
                stream2.Close();
                buffer2 = buffer;
                return buffer2;
            }
            catch (Exception exception)
            {
               throw new Exception("Error in symmetric engine. Error : " + exception.Message, exception);
            }            
        }

        private ICryptoTransform getCryptoTransform(bool encrypt)
        {
            SymmetricAlgorithm algorithm = this.selectAlgorithm();
            algorithm.Key = this.mKey;
            algorithm.IV = this.mIV;
            if (encrypt)
            {
                return algorithm.CreateEncryptor();
            }
            return algorithm.CreateDecryptor();
        }

        public XmlTextReader GetDecryptXmlReader(string filename)
        {
            StreamReader reader = File.OpenText(filename);
            string inputText = reader.ReadToEnd();
            reader.Close();
            return new XmlTextReader(new StringReader(this.Decrypt(inputText)));
        }

        private SymmetricAlgorithm selectAlgorithm()
        {
            switch (this.mCryptoType)
            {
                case CryptoTypes.encTypeDES:
                    return DES.Create();

                case CryptoTypes.encTypeRC2:
                    return RC2.Create();

                case CryptoTypes.encTypeRijndael:
                    return Rijndael.Create();

                case CryptoTypes.encTypeTripleDES:
                    return TripleDES.Create();
            }
            return TripleDES.Create();
        }

        public CryptoTypes CryptoType
        {
            get
            {
                return this.mCryptoType;
            }
            set
            {
                if (this.mCryptoType != value)
                {
                    this.mCryptoType = value;
                    this.calculateNewKeyAndIV();
                }
            }
        }

        public string Password
        {
            get
            {
                return this.mPassword;
            }
            set
            {
                if (this.mPassword != value)
                {
                    this.mPassword = value;
                    this.calculateNewKeyAndIV();
                }
            }
        }
    }
}

