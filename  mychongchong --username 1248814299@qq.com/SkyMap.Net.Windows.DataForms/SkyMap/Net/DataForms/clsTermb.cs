namespace SkyMap.Net.DataForms
{
    using SkyMap.Net.Core;
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    public class clsTermb
    {
        [DllImport("termb.dll", CharSet=CharSet.Auto)]
        private static extern int Authenticate();
        [DllImport("termb.dll", CharSet=CharSet.Auto)]
        private static extern int CloseComm();
        [DllImport("termb.dll", CharSet=CharSet.Auto)]
        private static extern int GetDepartment(byte[] strTmp, int strLen);
        [DllImport("termb.dll", CharSet=CharSet.Auto)]
        private static extern int GetPeopleAddress(byte[] strTmp, int strLen);
        [DllImport("termb.dll", CharSet=CharSet.Auto)]
        private static extern int GetPeopleBirthday(byte[] strTmp, int strLen);
        [DllImport("termb.dll", CharSet=CharSet.Auto)]
        private static extern int GetPeopleIDCode(byte[] strTmp, int strLen);
        [DllImport("termb.dll", CharSet=CharSet.Auto)]
        private static extern int GetPeopleName(byte[] strTmp, int strLen);
        [DllImport("termb.dll", CharSet=CharSet.Auto)]
        private static extern int GetPeopleNation(byte[] strTmp, int strLen);
        [DllImport("termb.dll", CharSet=CharSet.Auto)]
        private static extern int GetPeopleSex(byte[] strTmp, int strLen);
        [DllImport("termb.dll", CharSet=CharSet.Auto)]
        private static extern int InitComm(int Port);
        [DllImport("termb.dll", CharSet=CharSet.Auto)]
        private static extern int Read_Content(int Active);
        public static clsUserInfo ReadInfo()
        {
            clsUserInfo info;
            try
            {
                if (InitComm(0x3e9) == 1)
                {
                    try
                    {
                        if (Authenticate() != 1)
                        {
                            throw new ApplicationException("无法识别正确的身份证读卡权限(请确定你输入的读卡机器验证码是否正确)");
                        }
                        if (Read_Content(2) != 1)
                        {
                            throw new ApplicationException("读卡发生错误");
                        }
                        byte[] buffer = new byte[30];
                        byte[] buffer2 = new byte[0x24];
                        Reset(buffer);
                        Reset(buffer2);
                        GetPeopleName(buffer, buffer.Length);
                        GetPeopleIDCode(buffer2, buffer2.Length);
                        return new clsUserInfo(Encoding.Default.GetString(buffer).Replace("\0", ""), Encoding.Default.GetString(buffer2).Replace("\0", ""));
                    }
                    finally
                    {
                        CloseComm();
                    }
                }
                throw new ApplicationException("找不到身份证读卡器");
            }
            catch (Exception exception)
            {
                LoggingService.Error("读取身份证信息时出错", exception);
                throw exception;
            }
            return info;
        }

        private static void Reset(byte[] p_bye)
        {
            for (int i = 0; i < p_bye.Length; i++)
            {
                p_bye[i] = 0;
            }
        }
    }
}

