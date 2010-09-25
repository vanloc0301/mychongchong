using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZBPM.yd
{
    public interface iyd
    {
        double Fjqpj { get; set; }   //房价区片价
        double Jglxsz { get; set; } //结构类型修正系数
        double Cxsz { get; set; }   //朝向修正系数
        double Llsz { get; set; }   //楼龄修正系数
        double Lnqksz { get; set; } //临路情况修正系数
        double Calu();              //计算样点结果
    }

    public interface iyddj    //单家
    {
        double Jtsz { get; set; } //交通修正系数
        double Rjlsz { get; set; }//容积率修正系数
    }

    public interface iydfdj  //非单家
    {
        //建筑面积修正系数  楼型修正系数 楼层修正 物业管理修正 复式修正 公摊修正
        double Jzmjsz { get; set; }
        double Lxsz { get; set; }
        double Lcsz { get; set; }
        double Wyglsz { get; set; }
        double Fssz { get; set; }
        double Gtsz { get; set; }
        double Dtsz { get; set; }
    }

    public interface iydsearch
    {
        string Where();
    }
}
