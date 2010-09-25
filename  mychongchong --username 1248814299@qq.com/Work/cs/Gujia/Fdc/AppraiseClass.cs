using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
namespace ZBPM
{
    public static class AppraiseClass
    {
        public static double S4J5(double value, int digit)
        {
            double vt = Math.Pow(10, digit);
            double vx = value * vt;
            vx += 0.5;
            return (Math.Floor(vx) / vt);
        }

        public static double S4J5(string value)
        {
            return S4J5(double.Parse(value), 0);
        }

        //评估结果精确到个十百千万位
        public static double QuZhen(double value, int i)
        {
            if (i == 0) return value;
            string s = String.Format("{0:##0}", value);
            if (int.Parse(s.Substring(s.Length - i, 1)) >= 5)
            {
                s = (int.Parse(s.Substring(0, s.Length - i)) + 1).ToString();
            }
            else
            {
                s = s.Substring(0, s.Length - i);
            }
            if (i == 1)
            {
                s = s + "0";
            }
            else if (i == 2)
            {
                s = s + "00";
            }
            else if (i == 3)
            {
                s = s + "000";
            }
            else if (i == 4)
            {
                s = s + "0000";
            }
            else if (i == 5)
            {
                s = s + "00000";
            }

            return double.Parse(s);
        }

        //剩余年限
        public static double ShengYuNianXian(DateTime t1, DateTime t2)
        {
            System.Diagnostics.Debug.Assert(t2 > t1);
            double ireturn = 0;
            DateTime date1 = t1;
            DateTime date2 = t2;

            if (date2.Month < date1.Month)
            {

                int iyear = date2.Year - date1.Year - 1;
                int imonth = date2.Month - date1.Month + 12;
                ireturn = S4J5(double.Parse(iyear.ToString()) + double.Parse(imonth.ToString()) / 12, 1);
            }
            else
            {
                int iyear = date2.Year - date1.Year;
                int imonth = date2.Month - date1.Month;
                ireturn = S4J5(double.Parse(iyear.ToString()) + double.Parse(imonth.ToString()) / 12, 1);
            }
            return ireturn;
        }

        public static DateTime BiaoGaoYouXiaoQi(DateTime t1, int i)
        {
            DateTime tReturn;
            tReturn = t1.AddYears(i).AddDays(-1);
            return tReturn;
        }

        //检测xml是否包含某元素，true为包含，false为不包含;
        public static bool HasElement(XElement x, XNamespace ns, string e)
        {
            try
            {
                string tmp = x.Element(ns + e).Value.ToString();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
    public class XMLEncodeHelper
    {
        public static string Encode(string value)
        {
            StringBuilder builder = new StringBuilder(value);
            builder.Replace("&", "&amp;");
            builder.Replace(">", "&gt;");
            builder.Replace("<", "&lt;");
            builder.Replace("'", "&apos;");
            builder.Replace("\"", "&quot;");
            return builder.ToString();
        }

        public static string Decode(string value)
        {
            StringBuilder builder = new StringBuilder(value);
            builder.Replace("&amp;", "&");
            builder.Replace("&gt;", ">");
            builder.Replace("&lt;", "<");
            builder.Replace("&apos;", "'");
            builder.Replace("&quot;", "\"");
            return builder.ToString();
        }
    }
    public static class AppraiseReport
    {
        public static object MoneyXx(string aa)
        {
            object obj2 = aa;
            if (obj2 == null)
            {
                return "";
            }
            double num3 = double.Parse(obj2.ToString());
            return long.Parse(num3.ToString("0")).ToString("#,#");
        }

        public static object MoneyDx(string aa)
        {
            object obj2 = aa;
            if (obj2 == null)
            {
                return "";
            }
            long num = long.Parse((double.Parse(obj2.ToString())).ToString("0"));
            return BaseConvert.GetDay("{ct}", num);
        }

        public static object SetValue(string type, string aa)
        {
            switch (type)
            {
                case "开始日期(短)":
                    return BaseConvert.GetYearMonth(DateTime.Parse(aa));

                case "开始日期(长)":
                    return BaseConvert.GetYearMonthDay(DateTime.Parse(aa));

                case "开始日期(长数字)":
                    return string.Format("{0}年{1}月{2}日", DateTime.Parse(aa).Year, DateTime.Parse(aa).Month, DateTime.Parse(aa).Day);
               
                case "报告到期时间(长)":
                    return BaseConvert.GetYearMonthDay(DateTime.Parse(aa));

                case "报告到期时间(长数字)":
                    return string.Format("{0}年{1}月{2}日", DateTime.Parse(aa).Year, DateTime.Parse(aa).Month, DateTime.Parse(aa).Day);
                
                case "土地终止日期(长)":
                    return BaseConvert.GetYearMonthDay(DateTime.Parse(aa));

                case "土地终止日期(长数字)":
                    return string.Format("{0}年{1}月{2}日", DateTime.Parse(aa).Year, DateTime.Parse(aa).Month, DateTime.Parse(aa).Day);

                case "结束日期(长)":
                    return BaseConvert.GetYearMonthDay(DateTime.Parse(aa));

                case "结束日期(短)":
                    return BaseConvert.GetYearMonth(DateTime.Parse(aa));

                case "结束日期(长数字)":
                    return string.Format("{0}年{1}月{2}日", DateTime.Parse(aa).Year, DateTime.Parse(aa).Month, DateTime.Parse(aa).Day);

                case "估价期日(长)":
                    return BaseConvert.GetYearMonthDay(DateTime.Parse(aa));
                case "估价期日(长数字)":
                    return string.Format("{0}年{1}月{2}日", DateTime.Parse(aa).Year, DateTime.Parse(aa).Month, DateTime.Parse(aa).Day);

                case "有效年期(长)":
                    //if ((this.LimitDate % 12) == 0)
                    //{
                    return BaseConvert.GetDay("{cs}", int.Parse(aa));
                //}
                //return BaseConvert.GetDay("{cs}个月", (long) this.LimitDate);

                case "价值类型":
                    return aa;

                case "价值定义":
                    return aa;

                case "长估价目的":
                    return aa;

                case "附件":
                    return "";

                default:
                    {
                        return null;
                    }
            }

        }

        public static object ProjectBh(string aa)
        {
            string tmpstring, tmp;
            tmp = aa.Substring(aa.IndexOf("-") + 6);
            tmpstring = aa.Substring(aa.IndexOf("-") + 1, 4);
            tmpstring = string.Format("中置信评字第【{0}】{1}号", tmpstring, tmp.Substring(0, 4));
            return tmpstring;
        }

        public static object ReportBh(string aa)
        {
            string tmpstring, tmp;
            tmp = aa.Substring(aa.IndexOf("-") + 6);
            tmpstring = aa.Substring(aa.IndexOf("-") + 1, 4);
            tmpstring = string.Format("中置信评字第【{0}】{1}{2}号", tmpstring, tmp.Substring(0, 4), tmp.Substring(4));
            return tmpstring;
        }

        //{0}权利人 {1}地址 {2}报告类型 {3}价值类型 {4}短估价目的
        public static object ReportPurpose(string qlr, string dz, string bglx, string jzlx, string md)
        {
            string tmp = "确定{0}位于{1}的{2}{3}，为委托方{2}{4}提供参考。";
            System.Collections.Hashtable purpose = new System.Collections.Hashtable();
            purpose.Add("证据保全", "确定{0}位于{1}的{2}{3}，为委托方{2}{4}提供参考。");
            purpose.Add("土地出让", "确定{0}位于{1}的{2}{3}，为委托方{2}{4}提供参考。");
            purpose.Add("债权价值分析", "确定{0}位于{1}的{2}{3}，为委托方{2}{4}提供参考。");
            purpose.Add("交易纳税", "为{2}转让课税提供价值参考依据而评估{2}{3}");
            purpose.Add("资信担保", "确定{0}位于{1}的{2}{3}，为委托方{2}{4}提供参考。");
            purpose.Add("破产清算", "确定{0}位于{1}的{2}{3}，为委托方{2}{4}提供参考。");
            purpose.Add("收购资产", "确定{0}位于{1}的{2}{3}，为委托方{2}{4}提供参考。");
            purpose.Add("验资", "确定{0}位于{1}的{2}{3}，为委托方{2}{4}提供参考。");
            purpose.Add("赠与", "确定{0}位于{1}的{2}{3}，为委托方{2}{4}提供参考。");
            purpose.Add("拍卖", "为确定{2}的招标底价提供价值参考依据而评估{2}的{3}。");
            purpose.Add("按揭", "确定{0}位于{1}的{2}{3}，为委托方{2}{4}提供参考。");
            purpose.Add("地价监测", "确定{0}位于{1}的{2}{3}，为委托方{2}{4}提供参考。");
            purpose.Add("重组上市", "确定{0}位于{1}的{2}{3}，为委托方{2}{4}提供参考。");
            purpose.Add("清算", "确定{0}位于{1}的{2}{3}，为委托方{2}{4}提供参考。");
            purpose.Add("偿债能力", "确定{0}位于{1}的{2}{3}，为委托方{2}{4}提供参考。");
            purpose.Add("继承", "确定{0}位于{1}的{2}{3}，为委托方{2}{4}提供参考。");
            purpose.Add("企业改制", "确定{0}位于{1}的{2}{3}，为委托方{2}{4}提供参考。");
            purpose.Add("司法裁决", "确定{0}位于{1}的{2}{3}，为委托方{2}{4}提供参考。");
            purpose.Add("抵押贷款", "为确定抵押物的抵押贷款额度提供价值参考依据而评估其{3}。");
            purpose.Add("了解资产现值", "确定{0}位于{1}的{2}{3}，为委托方{2}{4}提供参考。");
            purpose.Add("资产重组", "确定{0}位于{1}的{2}{3}，为委托方{2}{4}提供参考。");
            purpose.Add("处置资产", "确定{0}位于{1}的{2}{3}，为委托方{2}{4}提供参考。");
            purpose.Add("招标", "确定{0}位于{1}的{2}{3}，为委托方{2}{4}提供参考。");
            purpose.Add("租金调查", "确定{0}位于{1}的{2}{3}，为委托方{2}{4}提供参考。");
            purpose.Add("挂牌出让", "确定{0}位于{1}的{2}{3}，为委托方{2}{4}提供参考。");
            purpose.Add("房地产拆迁", "确定{0}位于{1}的被拆迁房屋货币补偿金额而评估其房地产估{3}。");
            purpose.Add("土地拆迁", "确定{0}位于{1}的被征收土地使用权补偿金额而评估其土地{3}。");
            foreach (string aa in purpose.Keys)
            {
                if (aa == md)
                {
                    tmp = purpose[aa].ToString();
                }
            }
            return string.Format(tmp, qlr, dz, bglx, jzlx, md);
        }

        //镇区描述
        public static System.Collections.ArrayList Zqms(string zq, string lx)
        {
            System.Collections.ArrayList tmp = new System.Collections.ArrayList();
            tmp.Clear();
            if (zq.Equals("石岐区"))
            {
                tmp.Add("石岐区属于中山市北部冲积平原与中南部丘陵相接地带，地势平缓。石岐山（烟墩山）、西山、月山居其中，莲峰山、迎阳山、员峰山环立。最高的莲峰山海拔为78.8米，山地总面积近3平方公里。石岐区地处市城区中心，东至起湾道与东区接壤，南到白石涌与南区毗邻，西临石岐河与西区相望，北至东明北路的横河与港口镇相连，总面积49.72平方公里。石岐河环绕本区西北两面，往东北经火炬开发区出东河口水闸，注入横门水道。 石岐区地理位置优越，道路网络四通八达，水陆交通方便，是中山市的交通枢纽。105国道和中江高速公路、京珠高速公路的出入口与横跨岐江两岸的东明大桥、青溪大桥和外环路相连接，广珠轻轨即将开通，并在境内东河北设立了站口。区中心距高速公路仅5公里，距中山港只有15公里，距珠海、澳门57公里，距广州番禺40公里。");
            }
            else if (zq.Equals("西区"))
            {
                tmp.Add("西区是中山市城区的西大门，面积26.7平方公里，辖9个城市社区、6万多人。西区具有深厚的商业文化底蕴，早在20世纪80年代，已经是中山重要的商贸集散地，岐江河西岸车水马龙，人流如鲫、货如轮转。经过20多年的发展，岐江畔矗立起一颗璀璨的明珠—富华道商业圈。 这里商贸繁华，文化教育、医疗卫生、旅游观光设施完善，购物区、特色公园、健身广场等商务、娱乐场所林立。这里交通、通信十分便利，105国道、广珠公路、中江公路、北外环、博爱路、富华道、中山路等主干道路纵横穿越。这里政府服务诚信高效，企业创业空间广阔，是海内外客商投资置业的首选之地。2006年，全国强镇发展论坛公布了第二届全国小城镇综合发展水平1000强名单，西区名列第87位，是上升最快的镇区之一。 西区成功创建为市经济强区， 形成了西区片商贸文化产业组团， 以及沙朗片以工业、汽贸、农副产品批发为主导的产业组团，适宜居住适宜创业的氛围日益浓厚。");
            }
            else if (zq.Equals("火炬开发区"))
            {
                tmp.Add("中山火炬高技术产业开发区于1990年由国家科技部、广东省人民政府、中山市人民政府联合创立，1991年经国务院批准为首批国家级高技术产业开发区。开发区位于中山市的东部，面积70平方公里。东临珠江口，与深圳、香港隔海相望，南与与珠海、澳门毗邻，西连江门、佛山，北与广州、顺德相邻。水陆交通十分便利，京珠高速公路、广珠东线公路与区内的逸仙公路、环茂公路、中山港大道、沿江公路等纵横交错，形成发达的道路网；江尾头村位于开发区中部、东镇大道的南侧，对外交通便捷，附近有翡翠酒店、开发区医院、国祥花园等、商服业繁荣，基础设施及公共配套设施完善。");
            }
            else if (zq.Equals("东区"))
            {
                tmp.Add("东区是中山市政治、经济、文化、信息中心，市委、市政府所在地，面积71.4平方公里，辖10个社区居委会，常住人口约12万人。全区有旅居32个国家和地区的海外华侨、港澳台同胞2.53万人，是中山侨乡之一。区内路网纵横，交通顺畅；设施完善，物阜民丰；政治清明，文化兴盛。孙文公园、紫马岭公园遥相呼应，市全民健身广场、体育场、文化艺术中心和会展中心等大型设施坐落其中，区位优势日益突显。");
            }
            else if (zq.Equals("南区"))
            {
                tmp.Add("南区地处中山市中心城区西南面，东与东区相连，东南是五桂山脉，以大尖山为界与五桂山镇相隔，西南与板芙镇为临，西临石岐河，与沙溪镇、大涌镇相望，北与石岐区接壤。境内东南部和中部是低山丘陵地，石岐河绕过西北境，形成-平原。南区是中山市重点侨乡之一。区内工业发达，市属10家规模较大的企业分别设在区内，初步形成以工业为主体，门类齐全的工业体系。区境交通四通八达，105国道自北向南纵贯境内，投资环境优越，供水、供电充足。");
                tmp.Add("南区土地面积48平方公里，下辖8个行政村和2个居委会，户籍人口2﹒3万，外来人口1﹒5万。电梯产业、纺织产业、汽配产业、饮食产业是南区传统的特色产业，以蒂森电梯和三菱电梯企业已经发展到8家，年总产值突破了11亿元，成为广东省主要的电梯生产基地之一。全区2004年实现国内生产总值14﹒3亿元，工业总产值60亿元。");
                tmp.Add("南区经过近10年大规模的市政道路建设已经形成了以105国道、南环路、城南路、东环路、西环路和兴福路为核心的城市道路网络，地方一级以上公路总里程达到80多公里，是中山市中心城区向南向西辐射的主要交通枢纽。另外，省正在规划建设的广珠高速西线也将贯穿南区全境。近年随着交通网络的完善，南区加大了招商引资工作，吸引了万科城市风景、永安新城、翠茵庭等多个大型楼盘落户，商服业日益完善，已逐步发展成为中山市新的商住板快雏形。");
            }
            else if (zq.Equals("民众镇"))
            {
                tmp.Add("民众镇属亚热带季风性气候，气温四季怡人；其土地肥沃，水质优良，可发展种植业和养殖业。民众镇地处珠江三角洲的核心位置，东临珠江口，南与中山港隔江望，西连中山火炬高新技术产业开发区，北枕广州番禺。番中公路贯穿全镇17公里，有两个京珠高速公路出入口与镇公路相连，村村通公路，形成快速便捷的交通网络。民众北距虎门、南沙，南至中山港，西至容奇港都只有10至30多公里，距香港仅60海里，走京珠高速公路，半个小时可分别抵达广州和澳门，一个小时可达深圳，地理位置十分优越。同时，民众镇位于虎门南沙港、中山港、容奇港三个对外港口的中心点，濒临珠江口，水运交通十分方便。");
            }
            else if (zq.Equals("古镇镇 "))
            {
                tmp.Add("古镇镇位于中山市西北部，处于中山、江门佛山的交汇位置，地理位置十分优越。镇区以经营灯饰为主，已经形成一定的生产灯饰的产业规模；待估房产位于东岸公路旁，东岸公路是古镇海州区的主要干道之一，周围聚集较多的生产灯饰的工厂，交通便捷，产业聚集程度非常好。");
            }
            else if (zq.Equals("坦洲镇"))
            {
                tmp.Add(" 坦洲镇处于珠江口西岸，地处孙中山先生故乡的中山市最南端，属亚热带气候，年平均气温21.8度，年降雨量1600-1800毫米。地域东接105国道，南靠珠海经济特区，西临珠海西部开发区，与澳门相距约12公里，水陆交通十分方便。全镇总面积为136平方公里，土地资源丰富。常住人口5万多人。改革开放以来，坦洲镇致力发展经济，采取各种优惠政策吸引外商投资。坦洲投资的外商来自日、美、法、台、港、澳等国家和地区，目前，全镇有外资企业245家，其中：“三来一补”企业121家，“三资”企业124家，其中日资14家，台资36家。引进外资达4亿多美元。主要行业有化工、纺织、制药、服装、皮革制品、玩具、家具、电子、五金、首饰、注塑等。");
            }
            else if (zq.Equals("沙溪镇"))
            {
                tmp.Add("沙溪镇紧邻城区西部，面积55平方公里，辖15个村民委员会和1个居民委员会。户籍人口6.1万人，非户籍人口8万多人，侨居海外和港澳台的乡亲8万多人。是珠三角著名的侨乡和“文化之乡”，也是国家经济综合开发示范镇、广东省中心镇和广东省科技创新试点专业镇。近年来，荣获了“中国休闲服装名镇”、“国家卫生镇”、“全国民间艺术之乡”、“全国群众体育先进单位”、“广东省文明镇”、“中山市工业强镇”和“中山市文明镇”等多项殊荣。沙溪镇工业产品以“休闲服装”、“沙溪凉茶”享有盛名。沙溪镇以纺织服装为支柱，电子、化工、家具、家电、制药、食品、饮料等行业共同发展的工业体系。沙溪是中国重要的休闲服装生产基地。在服装界素有“休闲服装看沙溪”的美誉。沙溪休闲服装生产形成产业集群效应，有6000多亩的服装工业区；有200多家与制衣相关的纺织、漂染、印花、水洗、织唛、纽扣、机械配件等配套企业；有与服装生产销售相配套的布匹面料市场、制衣机械市场、辅料市场、布碎市场等专业市场。沙溪镇建有云汉布匹市场、龙瑞小商品市场、隆都家私城等专业市场；建有面积1.5万平方米的星宝国际展览中心，以及丽港商业步行街、星宝时尚大道等商业特色街。");
            }
            else
            {
                tmp.Add("");
            }
            return tmp;
        }


        //证件描述
        public static System.Collections.ArrayList Zjms(string stryxzj, string strtdz, string strfcz, string strfcgyz)
        {
            int i = 1;
            System.Collections.ArrayList tmp = new System.Collections.ArrayList();
            tmp.Clear();
            if (stryxzj.IndexOf("国有土地使用证") >= 0)
            {
                tmp.Add(string.Format("（{0}）国有土地使用证证号：{1}；", i.ToString(), strtdz));
                i++;
            }
            else if (stryxzj.IndexOf("集体土地使用证") >= 0)
            {
                tmp.Add(string.Format("（{0}）集体土地使用证证号：{1}；", i.ToString(), strtdz));
                i++;
            }

            if (stryxzj.IndexOf("广东省房地产权证") >= 0)
            {
                tmp.Add(string.Format("（{0}）广东省房地产权证证号：{1}；", i.ToString(), strfcz));
                i++;
            }
            else if (stryxzj.IndexOf("房地产权证") >= 0)
            {
                tmp.Add(string.Format("（{0}）房地产权证证号：{1}；", i.ToString(), strfcz));
                i++;
                if (stryxzj.IndexOf("房地产权共有（用）证") >= 0)
                {
                    tmp.Add(string.Format("（{0}）房地产权共有（用）证证号：{1}；", i.ToString(), strfcgyz));
                }
                i++;
            }

            if (stryxzj.IndexOf("其它") >= 0)
            {
                tmp.Add(string.Format("（{0}）其它证：{1}；", i.ToString(), strtdz));
                i++;
            }

            string tmpstr = tmp[tmp.Count - 1].ToString();
            tmp[tmp.Count-1] = tmpstr.Replace("；", "。");
            return tmp;
        }


        //附件描述
        public static System.Collections.ArrayList Fjms(string stryxzj, string strtdz, string strfcz, string strfcgyz)
        {
            int i = 1;
            System.Collections.ArrayList tmp = new System.Collections.ArrayList();
            tmp.Clear();
            tmp.Add(string.Format("附录{0}、现场拍摄相片", BaseConvert.GetDay("{cs}", i)));
            i++;
            if (stryxzj.IndexOf("国有土地使用证") >= 0)
            {
                tmp.Add(string.Format("附录{0}、国有土地使用证复印件", BaseConvert.GetDay("{cs}", i)));
                i++;
            }
            else if (stryxzj.IndexOf("集体土地使用证") >= 0)
            {
                tmp.Add(string.Format("附录{0}、集体土地使用证复印件", BaseConvert.GetDay("{cs}", i)));
                i++;
            }

            if (stryxzj.IndexOf("广东省房地产权证") >= 0)
            {
                tmp.Add(string.Format("附录{0}、广东省房地产权证复印件", BaseConvert.GetDay("{cs}", i)));
                i++;
            }
            else if (stryxzj.IndexOf("房地产权证") >= 0)
            {
                tmp.Add(string.Format("附录{0}、房地产权证复印件", BaseConvert.GetDay("{cs}", i)));
                i++;
                if (stryxzj.IndexOf("房地产权共有（用）证") >= 0)
                {
                    tmp.Add(string.Format("附录{0}、房地产权共有（用）证复印件", BaseConvert.GetDay("{cs}", i)));
                    i++;
                }
                
            }

            tmp.Add(string.Format("附录{0}、估价师资格证书复印件", BaseConvert.GetDay("{cs}", i)));
            i++;
            tmp.Add(string.Format("附录{0}、估价机构资质证书复印件", BaseConvert.GetDay("{cs}", i)));
            i++;
            tmp.Add(string.Format("附录{0}、估价机构营业执照复印件", BaseConvert.GetDay("{cs}", i)));


            return tmp;
        }

        //特殊替换
        public static string TF(string s1, string strstart, string strend)
        {
            string s2, s3, s4, s5;
            s2 = s1.Substring(s1.IndexOf("@@@") + 3, s1.IndexOf("###") - s1.IndexOf("@@@") - 3);
            byte[] output = Convert.FromBase64String(s2);
            s2 = Encoding.Default.GetString(output);
            // MessageBox.Show(s2);
            s3 = s1.Substring(0, s1.IndexOf(strstart) + strstart.Length);
            s4 = s1.Substring(s1.IndexOf(strend));
            s5 = s3 + s2 + s4;
            // MessageBox.Show(s3);
            // MessageBox.Show(s4);
            return s5;
        }

    }   
}
