namespace SkyMap.Net.Commands
{
    using Interop.CAPICOM;
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.OGM;
    using SkyMap.Net.Security;
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;
    using System.Windows.Forms;

    public class LoginByCert : AbstractCommand
    {
        private ICertificate2 GetCert()
        {
            try
            {
                Store store = new StoreClass();
                CAPICOM_STORE_LOCATION storeLocation = CAPICOM_STORE_LOCATION.CAPICOM_CURRENT_USER_STORE;
                NameValueCollection section = (NameValueCollection) ConfigurationManager.GetSection("securitySettings");
                string storeName = null;
                if (section != null)
                {
                    string str2 = section["CAPICOM_STORE_LOCATION"];
                    if (str2 != null)
                    {
                        try
                        {
                            storeLocation = (CAPICOM_STORE_LOCATION) Enum.Parse(typeof(CAPICOM_STORE_LOCATION), str2, true);
                        }
                        catch
                        {
                        }
                        storeName = section["StoreName"];
                    }
                }
                else
                {
                    LoggingService.Error("没有找到Security配置节!");
                }
                storeName = (storeName == null) ? "MY" : storeName;
                store.Open(storeLocation, storeName, CAPICOM_STORE_OPEN_MODE.CAPICOM_STORE_OPEN_MAXIMUM_ALLOWED);
                Certificates certificates = ((ICertificates2) store.Certificates).Select("置信自动化办公系统", "请选择你使用的私有数字证书", false);
                ICertificate2 certificate = (ICertificate2) store.Certificates[1];
                if (!certificate.HasPrivateKey())
                {
                    MessageBox.Show("该证书没有私钥，请确认是否是你的私有数字证书！");
                    return null;
                }
                if (certificate.PublicKey().Algorithm.FriendlyName != "RSA")
                {
                    MessageBox.Show("你选择的证书采用算法不是：RSA算法，不能使用！");
                    return null;
                }
                CspParameters parameters = new CspParameters();
                parameters.KeyContainerName = certificate.PrivateKey.ContainerName;
                parameters.ProviderName = certificate.PrivateKey.ProviderName;
                parameters.ProviderType = Convert.ToInt32(certificate.PrivateKey.ProviderType);
                switch (certificate.PrivateKey.KeySpec)
                {
                    case CAPICOM_KEY_SPEC.CAPICOM_KEY_SPEC_KEYEXCHANGE:
                        parameters.KeyNumber = 1;
                        break;

                    case CAPICOM_KEY_SPEC.CAPICOM_KEY_SPEC_SIGNATURE:
                        parameters.KeyNumber = 2;
                        break;
                }
                if (certificate.PrivateKey.IsMachineKeyset())
                {
                    parameters.Flags = CspProviderFlags.UseMachineKeyStore;
                }
                new RSACryptoServiceProvider(parameters).SignData(Convert.FromBase64String("test"), typeof(SHA1));
                return certificate;
            }
            catch (Exception exception)
            {
                MessageBox.Show("发生错误:" + exception.Message);
                return null;
            }
        }

        private static X509Certificate GetX509Certificate(ICertificate2 cert)
        {
            string str = cert.Export(CAPICOM_ENCODING_TYPE.CAPICOM_ENCODE_BASE64);
            char[] chArray2 = new char[3];
            chArray2[0] = '\r';
            chArray2[1] = '\n';
            char[] trimChars = chArray2;
            return new X509Certificate(Convert.FromBase64String(str.TrimEnd(trimChars)));
        }

        public override void Run()
        {
            ICertificate2 cert = this.GetCert();
            if (cert != null)
            {
                string name = GetX509Certificate(cert).GetName();
                if (name != string.Empty)
                {
                    foreach (CStaff staff in OGMService.GetStaffs())
                    {
                        if (name.EndsWith("=" + staff.Description))
                        {
                            OGMService.SetSmPrincipal(staff);
                            OGMService.CurrentUserCert = cert;
                            return;
                        }
                    }
                }
                MessageHelper.ShowInfo("不能验证你的证书到本系统用户，不能使用!");
            }
        }
    }
}

