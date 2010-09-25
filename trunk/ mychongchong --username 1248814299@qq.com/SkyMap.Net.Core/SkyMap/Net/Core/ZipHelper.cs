namespace SkyMap.Net.Core
{
    using System;
    using System.IO;
    using System.IO.Compression;

    public static class ZipHelper
    {
        public static byte[] GZCompress(byte[] source)
        {
            byte[] buffer;
            if ((source == null) || (source.Length == 0))
            {
                throw new ArgumentNullException("source");
            }
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    using (GZipStream stream2 = new GZipStream(stream, CompressionMode.Compress, true))
                    {
                        Console.WriteLine("Compression");
                        stream2.Write(source, 0, source.Length);
                        stream2.Flush();
                        stream2.Close();
                        Console.WriteLine("Original size: {0}, Compressed size: {1}", source.Length, stream.Length);
                        stream.Position = 0L;
                        buffer = stream.ToArray();
                    }
                }
            }
            catch (Exception exception)
            {
                LoggingService.Error("GZip压缩时出错：", exception);
                buffer = source;
            }
            return buffer;
        }

        public static byte[] GZDecompress(byte[] source)
        {
            byte[] buffer2;
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    MemoryStream stream2 = new MemoryStream(source);
                    stream2.Position = 0L;
                    using (GZipStream stream3 = new GZipStream(stream2, CompressionMode.Decompress, true))
                    {
                        bool flag2;
                        byte[] buffer = new byte[0x1000];
                        bool flag = false;
                        long num2 = 0L;
                        int num3 = 0;
                        goto Label_0096;
                    Label_0052:
                        num3 = stream3.ReadByte();
                        if (num3 != -1)
                        {
                            if (!flag)
                            {
                                flag = true;
                            }
                            num2 += 1L;
                            stream.WriteByte((byte) num3);
                        }
                        else if (flag)
                        {
                            goto Label_009B;
                        }
                    Label_0096:
                        flag2 = true;
                        goto Label_0052;
                    Label_009B:
                        stream3.Close();
                        Console.WriteLine("Original size: {0}, DeCompressed size: {1}", source.Length, stream.Length);
                    }
                    buffer2 = stream.ToArray();
                }
            }
            catch (Exception exception)
            {
                LoggingService.Error("GZip解压缩时出错：", exception);
                buffer2 = source;
            }
            return buffer2;
        }
    }
}

