using System;
using System.Collections;

namespace Gujia.Method
{
   /// <summary>
   ///功能描述    :    
   ///开发者      :    
   ///建立时间    :    2010-2-1 16:30:29
   ///修订描述    :    
   ///进度描述    :    
   ///版本号      :    1.0
   ///最后修改时间:    2010-2-1 16:30:29
   ///
   ///Function Description :    
   ///Developer                :    
   ///Builded Date:    2010-2-1 16:30:29
   ///Revision Description :    
   ///Progress Description :    
   ///Version Number        :    1.0
   ///Last Modify Date     :    2010-2-1 16:30:29
   /// </summary>
   public class 商住修正系数
   {
      #region 构造函数
      public 商住修正系数()
      {}

      public 商住修正系数(double ID,商住区片 商住区片,double s2,double s3,double s4,double s5,double s6,double s7,double s8,double s9,double ss10,double s11,double s12,double s13,double s14,double s15,double s16,double s17,double s18,double s19,double s20,double s21,double s22,double s23,double s24,double s25,double s26,double s27,double s28,double s29,double s30,double s31,double s32,double s33,double s34,double s35,double s36,double s37,double s38,double s39,double s40,double s41,double s42,double s43,double s44,double s45,double s46,double s47,double s48,double s49,double s50,double s51,double s52,double s53,double s54,double s55,double s56,double s57,double s58,double s59,double s60,double s61,double s62,double s63,double s64,double s65,double s66,double s67,double s68,double s69,double s70,double s71,double s72,double s73,double s74,double s75,double s76,double s77,double s78,double s79,double s80,double s81,double s82,double s83,double s84,double s85,double s86,double s87,double s88,double s89,double s90,double s91,double s92,double s93,double s94,double s95,double s96,double s97,double s98,double s99,double s100,double s101,double s102,double s103,double s104,double s105,double s106,double s107,double s108,double s109,double s110,double s111,double s112,double s113,double s114,double s115,double s116)
      {
         this.m_ID=ID;
         this.m_商住区片=商住区片;
         this.m_s2=s2;
         this.m_s3=s3;
         this.m_s4=s4;
         this.m_s5=s5;
         this.m_s6=s6;
         this.m_s7=s7;
         this.m_s8=s8;
         this.m_s9=s9;
         this.m_ss10=ss10;
         this.m_s11=s11;
         this.m_s12=s12;
         this.m_s13=s13;
         this.m_s14=s14;
         this.m_s15=s15;
         this.m_s16=s16;
         this.m_s17=s17;
         this.m_s18=s18;
         this.m_s19=s19;
         this.m_s20=s20;
         this.m_s21=s21;
         this.m_s22=s22;
         this.m_s23=s23;
         this.m_s24=s24;
         this.m_s25=s25;
         this.m_s26=s26;
         this.m_s27=s27;
         this.m_s28=s28;
         this.m_s29=s29;
         this.m_s30=s30;
         this.m_s31=s31;
         this.m_s32=s32;
         this.m_s33=s33;
         this.m_s34=s34;
         this.m_s35=s35;
         this.m_s36=s36;
         this.m_s37=s37;
         this.m_s38=s38;
         this.m_s39=s39;
         this.m_s40=s40;
         this.m_s41=s41;
         this.m_s42=s42;
         this.m_s43=s43;
         this.m_s44=s44;
         this.m_s45=s45;
         this.m_s46=s46;
         this.m_s47=s47;
         this.m_s48=s48;
         this.m_s49=s49;
         this.m_s50=s50;
         this.m_s51=s51;
         this.m_s52=s52;
         this.m_s53=s53;
         this.m_s54=s54;
         this.m_s55=s55;
         this.m_s56=s56;
         this.m_s57=s57;
         this.m_s58=s58;
         this.m_s59=s59;
         this.m_s60=s60;
         this.m_s61=s61;
         this.m_s62=s62;
         this.m_s63=s63;
         this.m_s64=s64;
         this.m_s65=s65;
         this.m_s66=s66;
         this.m_s67=s67;
         this.m_s68=s68;
         this.m_s69=s69;
         this.m_s70=s70;
         this.m_s71=s71;
         this.m_s72=s72;
         this.m_s73=s73;
         this.m_s74=s74;
         this.m_s75=s75;
         this.m_s76=s76;
         this.m_s77=s77;
         this.m_s78=s78;
         this.m_s79=s79;
         this.m_s80=s80;
         this.m_s81=s81;
         this.m_s82=s82;
         this.m_s83=s83;
         this.m_s84=s84;
         this.m_s85=s85;
         this.m_s86=s86;
         this.m_s87=s87;
         this.m_s88=s88;
         this.m_s89=s89;
         this.m_s90=s90;
         this.m_s91=s91;
         this.m_s92=s92;
         this.m_s93=s93;
         this.m_s94=s94;
         this.m_s95=s95;
         this.m_s96=s96;
         this.m_s97=s97;
         this.m_s98=s98;
         this.m_s99=s99;
         this.m_s100=s100;
         this.m_s101=s101;
         this.m_s102=s102;
         this.m_s103=s103;
         this.m_s104=s104;
         this.m_s105=s105;
         this.m_s106=s106;
         this.m_s107=s107;
         this.m_s108=s108;
         this.m_s109=s109;
         this.m_s110=s110;
         this.m_s111=s111;
         this.m_s112=s112;
         this.m_s113=s113;
         this.m_s114=s114;
         this.m_s115=s115;
         this.m_s116=s116;
      }
      #endregion

      #region 成员
      private double m_ID;
      protected 商住区片 m_商住区片;
      private double m_s2;
      private double m_s3;
      private double m_s4;
      private double m_s5;
      private double m_s6;
      private double m_s7;
      private double m_s8;
      private double m_s9;
      private double m_ss10;
      private double m_s11;
      private double m_s12;
      private double m_s13;
      private double m_s14;
      private double m_s15;
      private double m_s16;
      private double m_s17;
      private double m_s18;
      private double m_s19;
      private double m_s20;
      private double m_s21;
      private double m_s22;
      private double m_s23;
      private double m_s24;
      private double m_s25;
      private double m_s26;
      private double m_s27;
      private double m_s28;
      private double m_s29;
      private double m_s30;
      private double m_s31;
      private double m_s32;
      private double m_s33;
      private double m_s34;
      private double m_s35;
      private double m_s36;
      private double m_s37;
      private double m_s38;
      private double m_s39;
      private double m_s40;
      private double m_s41;
      private double m_s42;
      private double m_s43;
      private double m_s44;
      private double m_s45;
      private double m_s46;
      private double m_s47;
      private double m_s48;
      private double m_s49;
      private double m_s50;
      private double m_s51;
      private double m_s52;
      private double m_s53;
      private double m_s54;
      private double m_s55;
      private double m_s56;
      private double m_s57;
      private double m_s58;
      private double m_s59;
      private double m_s60;
      private double m_s61;
      private double m_s62;
      private double m_s63;
      private double m_s64;
      private double m_s65;
      private double m_s66;
      private double m_s67;
      private double m_s68;
      private double m_s69;
      private double m_s70;
      private double m_s71;
      private double m_s72;
      private double m_s73;
      private double m_s74;
      private double m_s75;
      private double m_s76;
      private double m_s77;
      private double m_s78;
      private double m_s79;
      private double m_s80;
      private double m_s81;
      private double m_s82;
      private double m_s83;
      private double m_s84;
      private double m_s85;
      private double m_s86;
      private double m_s87;
      private double m_s88;
      private double m_s89;
      private double m_s90;
      private double m_s91;
      private double m_s92;
      private double m_s93;
      private double m_s94;
      private double m_s95;
      private double m_s96;
      private double m_s97;
      private double m_s98;
      private double m_s99;
      private double m_s100;
      private double m_s101;
      private double m_s102;
      private double m_s103;
      private double m_s104;
      private double m_s105;
      private double m_s106;
      private double m_s107;
      private double m_s108;
      private double m_s109;
      private double m_s110;
      private double m_s111;
      private double m_s112;
      private double m_s113;
      private double m_s114;
      private double m_s115;
      private double m_s116;
      #endregion

      #region 属性
      public  virtual double ID
      {
         get {  return m_ID; }
         set {  m_ID = value; }
      }

      public  virtual 商住区片 商住区片
      {
         get {  return m_商住区片; }
         set {  m_商住区片 = value; }
      }

      public  virtual double s2
      {
         get {  return m_s2; }
         set {  m_s2 = value; }
      }

      public  virtual double s3
      {
         get {  return m_s3; }
         set {  m_s3 = value; }
      }

      public  virtual double s4
      {
         get {  return m_s4; }
         set {  m_s4 = value; }
      }

      public  virtual double s5
      {
         get {  return m_s5; }
         set {  m_s5 = value; }
      }

      public  virtual double s6
      {
         get {  return m_s6; }
         set {  m_s6 = value; }
      }

      public  virtual double s7
      {
         get {  return m_s7; }
         set {  m_s7 = value; }
      }

      public  virtual double s8
      {
         get {  return m_s8; }
         set {  m_s8 = value; }
      }

      public  virtual double s9
      {
         get {  return m_s9; }
         set {  m_s9 = value; }
      }

      public  virtual double ss10
      {
         get {  return m_ss10; }
         set {  m_ss10 = value; }
      }

      public  virtual double s11
      {
         get {  return m_s11; }
         set {  m_s11 = value; }
      }

      public  virtual double s12
      {
         get {  return m_s12; }
         set {  m_s12 = value; }
      }

      public  virtual double s13
      {
         get {  return m_s13; }
         set {  m_s13 = value; }
      }

      public  virtual double s14
      {
         get {  return m_s14; }
         set {  m_s14 = value; }
      }

      public  virtual double s15
      {
         get {  return m_s15; }
         set {  m_s15 = value; }
      }

      public  virtual double s16
      {
         get {  return m_s16; }
         set {  m_s16 = value; }
      }

      public  virtual double s17
      {
         get {  return m_s17; }
         set {  m_s17 = value; }
      }

      public  virtual double s18
      {
         get {  return m_s18; }
         set {  m_s18 = value; }
      }

      public  virtual double s19
      {
         get {  return m_s19; }
         set {  m_s19 = value; }
      }

      public  virtual double s20
      {
         get {  return m_s20; }
         set {  m_s20 = value; }
      }

      public  virtual double s21
      {
         get {  return m_s21; }
         set {  m_s21 = value; }
      }

      public  virtual double s22
      {
         get {  return m_s22; }
         set {  m_s22 = value; }
      }

      public  virtual double s23
      {
         get {  return m_s23; }
         set {  m_s23 = value; }
      }

      public  virtual double s24
      {
         get {  return m_s24; }
         set {  m_s24 = value; }
      }

      public  virtual double s25
      {
         get {  return m_s25; }
         set {  m_s25 = value; }
      }

      public  virtual double s26
      {
         get {  return m_s26; }
         set {  m_s26 = value; }
      }

      public  virtual double s27
      {
         get {  return m_s27; }
         set {  m_s27 = value; }
      }

      public  virtual double s28
      {
         get {  return m_s28; }
         set {  m_s28 = value; }
      }

      public  virtual double s29
      {
         get {  return m_s29; }
         set {  m_s29 = value; }
      }

      public  virtual double s30
      {
         get {  return m_s30; }
         set {  m_s30 = value; }
      }

      public  virtual double s31
      {
         get {  return m_s31; }
         set {  m_s31 = value; }
      }

      public  virtual double s32
      {
         get {  return m_s32; }
         set {  m_s32 = value; }
      }

      public  virtual double s33
      {
         get {  return m_s33; }
         set {  m_s33 = value; }
      }

      public  virtual double s34
      {
         get {  return m_s34; }
         set {  m_s34 = value; }
      }

      public  virtual double s35
      {
         get {  return m_s35; }
         set {  m_s35 = value; }
      }

      public  virtual double s36
      {
         get {  return m_s36; }
         set {  m_s36 = value; }
      }

      public  virtual double s37
      {
         get {  return m_s37; }
         set {  m_s37 = value; }
      }

      public  virtual double s38
      {
         get {  return m_s38; }
         set {  m_s38 = value; }
      }

      public  virtual double s39
      {
         get {  return m_s39; }
         set {  m_s39 = value; }
      }

      public  virtual double s40
      {
         get {  return m_s40; }
         set {  m_s40 = value; }
      }

      public  virtual double s41
      {
         get {  return m_s41; }
         set {  m_s41 = value; }
      }

      public  virtual double s42
      {
         get {  return m_s42; }
         set {  m_s42 = value; }
      }

      public  virtual double s43
      {
         get {  return m_s43; }
         set {  m_s43 = value; }
      }

      public  virtual double s44
      {
         get {  return m_s44; }
         set {  m_s44 = value; }
      }

      public  virtual double s45
      {
         get {  return m_s45; }
         set {  m_s45 = value; }
      }

      public  virtual double s46
      {
         get {  return m_s46; }
         set {  m_s46 = value; }
      }

      public  virtual double s47
      {
         get {  return m_s47; }
         set {  m_s47 = value; }
      }

      public  virtual double s48
      {
         get {  return m_s48; }
         set {  m_s48 = value; }
      }

      public  virtual double s49
      {
         get {  return m_s49; }
         set {  m_s49 = value; }
      }

      public  virtual double s50
      {
         get {  return m_s50; }
         set {  m_s50 = value; }
      }

      public  virtual double s51
      {
         get {  return m_s51; }
         set {  m_s51 = value; }
      }

      public  virtual double s52
      {
         get {  return m_s52; }
         set {  m_s52 = value; }
      }

      public  virtual double s53
      {
         get {  return m_s53; }
         set {  m_s53 = value; }
      }

      public  virtual double s54
      {
         get {  return m_s54; }
         set {  m_s54 = value; }
      }

      public  virtual double s55
      {
         get {  return m_s55; }
         set {  m_s55 = value; }
      }

      public  virtual double s56
      {
         get {  return m_s56; }
         set {  m_s56 = value; }
      }

      public  virtual double s57
      {
         get {  return m_s57; }
         set {  m_s57 = value; }
      }

      public  virtual double s58
      {
         get {  return m_s58; }
         set {  m_s58 = value; }
      }

      public  virtual double s59
      {
         get {  return m_s59; }
         set {  m_s59 = value; }
      }

      public  virtual double s60
      {
         get {  return m_s60; }
         set {  m_s60 = value; }
      }

      public  virtual double s61
      {
         get {  return m_s61; }
         set {  m_s61 = value; }
      }

      public  virtual double s62
      {
         get {  return m_s62; }
         set {  m_s62 = value; }
      }

      public  virtual double s63
      {
         get {  return m_s63; }
         set {  m_s63 = value; }
      }

      public  virtual double s64
      {
         get {  return m_s64; }
         set {  m_s64 = value; }
      }

      public  virtual double s65
      {
         get {  return m_s65; }
         set {  m_s65 = value; }
      }

      public  virtual double s66
      {
         get {  return m_s66; }
         set {  m_s66 = value; }
      }

      public  virtual double s67
      {
         get {  return m_s67; }
         set {  m_s67 = value; }
      }

      public  virtual double s68
      {
         get {  return m_s68; }
         set {  m_s68 = value; }
      }

      public  virtual double s69
      {
         get {  return m_s69; }
         set {  m_s69 = value; }
      }

      public  virtual double s70
      {
         get {  return m_s70; }
         set {  m_s70 = value; }
      }

      public  virtual double s71
      {
         get {  return m_s71; }
         set {  m_s71 = value; }
      }

      public  virtual double s72
      {
         get {  return m_s72; }
         set {  m_s72 = value; }
      }

      public  virtual double s73
      {
         get {  return m_s73; }
         set {  m_s73 = value; }
      }

      public  virtual double s74
      {
         get {  return m_s74; }
         set {  m_s74 = value; }
      }

      public  virtual double s75
      {
         get {  return m_s75; }
         set {  m_s75 = value; }
      }

      public  virtual double s76
      {
         get {  return m_s76; }
         set {  m_s76 = value; }
      }

      public  virtual double s77
      {
         get {  return m_s77; }
         set {  m_s77 = value; }
      }

      public  virtual double s78
      {
         get {  return m_s78; }
         set {  m_s78 = value; }
      }

      public  virtual double s79
      {
         get {  return m_s79; }
         set {  m_s79 = value; }
      }

      public  virtual double s80
      {
         get {  return m_s80; }
         set {  m_s80 = value; }
      }

      public  virtual double s81
      {
         get {  return m_s81; }
         set {  m_s81 = value; }
      }

      public  virtual double s82
      {
         get {  return m_s82; }
         set {  m_s82 = value; }
      }

      public  virtual double s83
      {
         get {  return m_s83; }
         set {  m_s83 = value; }
      }

      public  virtual double s84
      {
         get {  return m_s84; }
         set {  m_s84 = value; }
      }

      public  virtual double s85
      {
         get {  return m_s85; }
         set {  m_s85 = value; }
      }

      public  virtual double s86
      {
         get {  return m_s86; }
         set {  m_s86 = value; }
      }

      public  virtual double s87
      {
         get {  return m_s87; }
         set {  m_s87 = value; }
      }

      public  virtual double s88
      {
         get {  return m_s88; }
         set {  m_s88 = value; }
      }

      public  virtual double s89
      {
         get {  return m_s89; }
         set {  m_s89 = value; }
      }

      public  virtual double s90
      {
         get {  return m_s90; }
         set {  m_s90 = value; }
      }

      public  virtual double s91
      {
         get {  return m_s91; }
         set {  m_s91 = value; }
      }

      public  virtual double s92
      {
         get {  return m_s92; }
         set {  m_s92 = value; }
      }

      public  virtual double s93
      {
         get {  return m_s93; }
         set {  m_s93 = value; }
      }

      public  virtual double s94
      {
         get {  return m_s94; }
         set {  m_s94 = value; }
      }

      public  virtual double s95
      {
         get {  return m_s95; }
         set {  m_s95 = value; }
      }

      public  virtual double s96
      {
         get {  return m_s96; }
         set {  m_s96 = value; }
      }

      public  virtual double s97
      {
         get {  return m_s97; }
         set {  m_s97 = value; }
      }

      public  virtual double s98
      {
         get {  return m_s98; }
         set {  m_s98 = value; }
      }

      public  virtual double s99
      {
         get {  return m_s99; }
         set {  m_s99 = value; }
      }

      public  virtual double s100
      {
         get {  return m_s100; }
         set {  m_s100 = value; }
      }

      public  virtual double s101
      {
         get {  return m_s101; }
         set {  m_s101 = value; }
      }

      public  virtual double s102
      {
         get {  return m_s102; }
         set {  m_s102 = value; }
      }

      public  virtual double s103
      {
         get {  return m_s103; }
         set {  m_s103 = value; }
      }

      public  virtual double s104
      {
         get {  return m_s104; }
         set {  m_s104 = value; }
      }

      public  virtual double s105
      {
         get {  return m_s105; }
         set {  m_s105 = value; }
      }

      public  virtual double s106
      {
         get {  return m_s106; }
         set {  m_s106 = value; }
      }

      public  virtual double s107
      {
         get {  return m_s107; }
         set {  m_s107 = value; }
      }

      public  virtual double s108
      {
         get {  return m_s108; }
         set {  m_s108 = value; }
      }

      public  virtual double s109
      {
         get {  return m_s109; }
         set {  m_s109 = value; }
      }

      public  virtual double s110
      {
         get {  return m_s110; }
         set {  m_s110 = value; }
      }

      public  virtual double s111
      {
         get {  return m_s111; }
         set {  m_s111 = value; }
      }

      public  virtual double s112
      {
         get {  return m_s112; }
         set {  m_s112 = value; }
      }

      public  virtual double s113
      {
         get {  return m_s113; }
         set {  m_s113 = value; }
      }

      public  virtual double s114
      {
         get {  return m_s114; }
         set {  m_s114 = value; }
      }

      public  virtual double s115
      {
         get {  return m_s115; }
         set {  m_s115 = value; }
      }

      public  virtual double s116
      {
         get {  return m_s116; }
         set {  m_s116 = value; }
      }

      #endregion

   }
}
