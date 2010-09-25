namespace SkyMap.Net.POP3
{
    using System;
    using System.Collections;
    using System.Text;

    public class Pop3MessageComponents
    {
        private ArrayList m_component;

        public Pop3MessageComponents(string[] lines, long startOfBody, string multipartBoundary, string mainContentType)
        {
            StringBuilder builder;
            long num2;
            this.m_component = new ArrayList();
            long length = lines.Length;
            if (multipartBoundary == null)
            {
                builder = new StringBuilder();
                for (num2 = startOfBody; num2 < length; num2 += 1L)
                {
                    builder.Append(lines[(int) ((IntPtr) num2)].Replace("\n", "").Replace("\r", ""));
                }
                this.m_component.Add(new Pop3Component(mainContentType, builder.ToString()));
            }
            else
            {
                string boundary = multipartBoundary;
                bool flag = true;
                num2 = startOfBody;
                while (num2 < length)
                {
                    string str9;
                    bool flag2 = true;
                    string contentType = null;
                    string name = null;
                    string filename = null;
                    string contentTransferEncoding = null;
                    string contentDescription = null;
                    string contentDisposition = null;
                    string data = null;
                    if (flag)
                    {
                        flag2 = false;
                        flag = false;
                        while (num2 < length)
                        {
                            if (Pop3Parse.GetSubHeaderLineType(lines[(int) ((IntPtr) num2)].Replace("\n", "").Replace("\r", ""), boundary) == -97)
                            {
                                flag2 = true;
                                num2 += 1L;
                                break;
                            }
                            num2 += 1L;
                        }
                    }
                    if (!flag2)
                    {
                        throw new Pop3MissingBoundaryException("Missing multipart boundary: " + boundary);
                    }
                    bool flag3 = false;
                    while (num2 < length)
                    {
                        str9 = lines[(int) ((IntPtr) num2)].Replace("\n", "").Replace("\r", "");
                        switch (Pop3Parse.GetSubHeaderLineType(str9, boundary))
                        {
                            case 0:
                                contentType = Pop3Parse.ContentType(str9);
                                break;

                            case 1:
                                contentTransferEncoding = Pop3Parse.ContentTransferEncoding(str9);
                                break;

                            case 2:
                                contentDescription = Pop3Parse.ContentDescription(str9);
                                break;

                            case 3:
                                contentDisposition = Pop3Parse.ContentDisposition(str9);
                                break;

                            case -98:
                                flag3 = true;
                                break;
                        }
                        num2 += 1L;
                        if (flag3)
                        {
                            break;
                        }
                        while (num2 < length)
                        {
                            if (!str9.Substring(str9.Length - 1, 1).Equals(";"))
                            {
                                break;
                            }
                            string line = lines[(int) ((IntPtr) num2)].Replace("\r", "").Replace("\n", "");
                            switch (Pop3Parse.GetSubHeaderNextLineType(line))
                            {
                                case 0:
                                    name = Pop3Parse.Name(line);
                                    break;

                                case 1:
                                    filename = Pop3Parse.Filename(line);
                                    break;

                                case -98:
                                    flag3 = true;
                                    break;
                            }
                            if (!flag3)
                            {
                                str9 = line;
                                num2 += 1L;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    flag2 = false;
                    builder = new StringBuilder();
                    bool flag4 = false;
                    while (num2 < length)
                    {
                        str9 = lines[(int) ((IntPtr) num2)].Replace("\n", "");
                        if (Pop3Parse.GetSubHeaderLineType(str9, boundary) == -97)
                        {
                            flag2 = true;
                            num2 += 1L;
                            break;
                        }
                        if (Pop3Parse.GetSubHeaderLineType(str9, boundary) == -96)
                        {
                            flag4 = true;
                            break;
                        }
                        builder.Append(lines[(int) ((IntPtr) num2)]);
                        num2 += 1L;
                    }
                    if (builder.Length > 0)
                    {
                        data = builder.ToString();
                    }
                    this.m_component.Add(new Pop3Component(contentType, name, filename, contentTransferEncoding, contentDescription, contentDisposition, data));
                    if (flag4)
                    {
                        break;
                    }
                }
            }
        }

        public IEnumerator ComponentEnumerator
        {
            get
            {
                return this.m_component.GetEnumerator();
            }
        }

        public int NumberOfComponents
        {
            get
            {
                return this.m_component.Count;
            }
        }
    }
}

