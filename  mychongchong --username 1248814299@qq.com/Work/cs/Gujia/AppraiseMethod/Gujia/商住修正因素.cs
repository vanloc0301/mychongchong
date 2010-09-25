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
   public class 商住修正因素
   {
      #region 构造函数
      public 商住修正因素()
      {}

      public 商住修正因素(double 土地级别,string s2,string s3,string s4,string s5,string s6,string s7,string s8,string s9,string s10,string s11,string s12,string s13,string s14,string s15,string s16,string s17,string s18,string s19,string s20,string s21,string s22,string s23,string s24,string s25,string s26,string s27,string s28,string s29,string s30,string s31,string s32,string s33,string s34,string s35,string s36,string s37,string s38,string s39,string s40,string s41,string s42,string s43,string s44,string s45,string s46,string s47,string s48,string s49,string s50,string s51,string s52,string s53,string s54,string s55,string s56,string s57,string s58,string s59,string s60,string s61,string s62,string s63,string s64,string s65,string s66,string s67,string s68,string s69,string s70,string s71,string s72,string s73,string s74,string s75,string s76,string s77,string s78,string s79,string s80,string s81,string s82,string s83,string s84,string s85,string s86,string s87,string s88,string s89,string s90,string s91,string s92,string s93,string s94,string s95,string s96,string s97,string s98,string s99,string s100,string s101,string s102,string s103,string s104,string s105,string s106,string s107,string s108,string s109,string s110,string s111,string s112,string s113,string s114,string s115,string s116)
      {
         this.m_土地级别=土地级别;
         this.m_s2=s2;
         this.m_s3=s3;
         this.m_s4=s4;
         this.m_s5=s5;
         this.m_s6=s6;
         this.m_s7=s7;
         this.m_s8=s8;
         this.m_s9=s9;
         this.m_s10=s10;
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
      protected IList m_商住区片_土地级别;
      private double m_土地级别;
      private string m_s2;
      private string m_s3;
      private string m_s4;
      private string m_s5;
      private string m_s6;
      private string m_s7;
      private string m_s8;
      private string m_s9;
      private string m_s10;
      private string m_s11;
      private string m_s12;
      private string m_s13;
      private string m_s14;
      private string m_s15;
      private string m_s16;
      private string m_s17;
      private string m_s18;
      private string m_s19;
      private string m_s20;
      private string m_s21;
      private string m_s22;
      private string m_s23;
      private string m_s24;
      private string m_s25;
      private string m_s26;
      private string m_s27;
      private string m_s28;
      private string m_s29;
      private string m_s30;
      private string m_s31;
      private string m_s32;
      private string m_s33;
      private string m_s34;
      private string m_s35;
      private string m_s36;
      private string m_s37;
      private string m_s38;
      private string m_s39;
      private string m_s40;
      private string m_s41;
      private string m_s42;
      private string m_s43;
      private string m_s44;
      private string m_s45;
      private string m_s46;
      private string m_s47;
      private string m_s48;
      private string m_s49;
      private string m_s50;
      private string m_s51;
      private string m_s52;
      private string m_s53;
      private string m_s54;
      private string m_s55;
      private string m_s56;
      private string m_s57;
      private string m_s58;
      private string m_s59;
      private string m_s60;
      private string m_s61;
      private string m_s62;
      private string m_s63;
      private string m_s64;
      private string m_s65;
      private string m_s66;
      private string m_s67;
      private string m_s68;
      private string m_s69;
      private string m_s70;
      private string m_s71;
      private string m_s72;
      private string m_s73;
      private string m_s74;
      private string m_s75;
      private string m_s76;
      private string m_s77;
      private string m_s78;
      private string m_s79;
      private string m_s80;
      private string m_s81;
      private string m_s82;
      private string m_s83;
      private string m_s84;
      private string m_s85;
      private string m_s86;
      private string m_s87;
      private string m_s88;
      private string m_s89;
      private string m_s90;
      private string m_s91;
      private string m_s92;
      private string m_s93;
      private string m_s94;
      private string m_s95;
      private string m_s96;
      private string m_s97;
      private string m_s98;
      private string m_s99;
      private string m_s100;
      private string m_s101;
      private string m_s102;
      private string m_s103;
      private string m_s104;
      private string m_s105;
      private string m_s106;
      private string m_s107;
      private string m_s108;
      private string m_s109;
      private string m_s110;
      private string m_s111;
      private string m_s112;
      private string m_s113;
      private string m_s114;
      private string m_s115;
      private string m_s116;
      #endregion


      #region 属性
      public  virtual double 土地级别
      {
         get {  return m_土地级别; }
         set {  m_土地级别 = value; }
      }

      public  virtual string s2
      {
         get {  return m_s2; }
         set {  m_s2 = value; }
      }

      public  virtual string s3
      {
         get {  return m_s3; }
         set {  m_s3 = value; }
      }

      public  virtual string s4
      {
         get {  return m_s4; }
         set {  m_s4 = value; }
      }

      public  virtual string s5
      {
         get {  return m_s5; }
         set {  m_s5 = value; }
      }

      public  virtual string s6
      {
         get {  return m_s6; }
         set {  m_s6 = value; }
      }

      public  virtual string s7
      {
         get {  return m_s7; }
         set {  m_s7 = value; }
      }

      public  virtual string s8
      {
         get {  return m_s8; }
         set {  m_s8 = value; }
      }

      public  virtual string s9
      {
         get {  return m_s9; }
         set {  m_s9 = value; }
      }

      public  virtual string s10
      {
         get {  return m_s10; }
         set {  m_s10 = value; }
      }

      public  virtual string s11
      {
         get {  return m_s11; }
         set {  m_s11 = value; }
      }

      public  virtual string s12
      {
         get {  return m_s12; }
         set {  m_s12 = value; }
      }

      public  virtual string s13
      {
         get {  return m_s13; }
         set {  m_s13 = value; }
      }

      public  virtual string s14
      {
         get {  return m_s14; }
         set {  m_s14 = value; }
      }

      public  virtual string s15
      {
         get {  return m_s15; }
         set {  m_s15 = value; }
      }

      public  virtual string s16
      {
         get {  return m_s16; }
         set {  m_s16 = value; }
      }

      public  virtual string s17
      {
         get {  return m_s17; }
         set {  m_s17 = value; }
      }

      public  virtual string s18
      {
         get {  return m_s18; }
         set {  m_s18 = value; }
      }

      public  virtual string s19
      {
         get {  return m_s19; }
         set {  m_s19 = value; }
      }

      public  virtual string s20
      {
         get {  return m_s20; }
         set {  m_s20 = value; }
      }

      public  virtual string s21
      {
         get {  return m_s21; }
         set {  m_s21 = value; }
      }

      public  virtual string s22
      {
         get {  return m_s22; }
         set {  m_s22 = value; }
      }

      public  virtual string s23
      {
         get {  return m_s23; }
         set {  m_s23 = value; }
      }

      public  virtual string s24
      {
         get {  return m_s24; }
         set {  m_s24 = value; }
      }

      public  virtual string s25
      {
         get {  return m_s25; }
         set {  m_s25 = value; }
      }

      public  virtual string s26
      {
         get {  return m_s26; }
         set {  m_s26 = value; }
      }

      public  virtual string s27
      {
         get {  return m_s27; }
         set {  m_s27 = value; }
      }

      public  virtual string s28
      {
         get {  return m_s28; }
         set {  m_s28 = value; }
      }

      public  virtual string s29
      {
         get {  return m_s29; }
         set {  m_s29 = value; }
      }

      public  virtual string s30
      {
         get {  return m_s30; }
         set {  m_s30 = value; }
      }

      public  virtual string s31
      {
         get {  return m_s31; }
         set {  m_s31 = value; }
      }

      public  virtual string s32
      {
         get {  return m_s32; }
         set {  m_s32 = value; }
      }

      public  virtual string s33
      {
         get {  return m_s33; }
         set {  m_s33 = value; }
      }

      public  virtual string s34
      {
         get {  return m_s34; }
         set {  m_s34 = value; }
      }

      public  virtual string s35
      {
         get {  return m_s35; }
         set {  m_s35 = value; }
      }

      public  virtual string s36
      {
         get {  return m_s36; }
         set {  m_s36 = value; }
      }

      public  virtual string s37
      {
         get {  return m_s37; }
         set {  m_s37 = value; }
      }

      public  virtual string s38
      {
         get {  return m_s38; }
         set {  m_s38 = value; }
      }

      public  virtual string s39
      {
         get {  return m_s39; }
         set {  m_s39 = value; }
      }

      public  virtual string s40
      {
         get {  return m_s40; }
         set {  m_s40 = value; }
      }

      public  virtual string s41
      {
         get {  return m_s41; }
         set {  m_s41 = value; }
      }

      public  virtual string s42
      {
         get {  return m_s42; }
         set {  m_s42 = value; }
      }

      public  virtual string s43
      {
         get {  return m_s43; }
         set {  m_s43 = value; }
      }

      public  virtual string s44
      {
         get {  return m_s44; }
         set {  m_s44 = value; }
      }

      public  virtual string s45
      {
         get {  return m_s45; }
         set {  m_s45 = value; }
      }

      public  virtual string s46
      {
         get {  return m_s46; }
         set {  m_s46 = value; }
      }

      public  virtual string s47
      {
         get {  return m_s47; }
         set {  m_s47 = value; }
      }

      public  virtual string s48
      {
         get {  return m_s48; }
         set {  m_s48 = value; }
      }

      public  virtual string s49
      {
         get {  return m_s49; }
         set {  m_s49 = value; }
      }

      public  virtual string s50
      {
         get {  return m_s50; }
         set {  m_s50 = value; }
      }

      public  virtual string s51
      {
         get {  return m_s51; }
         set {  m_s51 = value; }
      }

      public  virtual string s52
      {
         get {  return m_s52; }
         set {  m_s52 = value; }
      }

      public  virtual string s53
      {
         get {  return m_s53; }
         set {  m_s53 = value; }
      }

      public  virtual string s54
      {
         get {  return m_s54; }
         set {  m_s54 = value; }
      }

      public  virtual string s55
      {
         get {  return m_s55; }
         set {  m_s55 = value; }
      }

      public  virtual string s56
      {
         get {  return m_s56; }
         set {  m_s56 = value; }
      }

      public  virtual string s57
      {
         get {  return m_s57; }
         set {  m_s57 = value; }
      }

      public  virtual string s58
      {
         get {  return m_s58; }
         set {  m_s58 = value; }
      }

      public  virtual string s59
      {
         get {  return m_s59; }
         set {  m_s59 = value; }
      }

      public  virtual string s60
      {
         get {  return m_s60; }
         set {  m_s60 = value; }
      }

      public  virtual string s61
      {
         get {  return m_s61; }
         set {  m_s61 = value; }
      }

      public  virtual string s62
      {
         get {  return m_s62; }
         set {  m_s62 = value; }
      }

      public  virtual string s63
      {
         get {  return m_s63; }
         set {  m_s63 = value; }
      }

      public  virtual string s64
      {
         get {  return m_s64; }
         set {  m_s64 = value; }
      }

      public  virtual string s65
      {
         get {  return m_s65; }
         set {  m_s65 = value; }
      }

      public  virtual string s66
      {
         get {  return m_s66; }
         set {  m_s66 = value; }
      }

      public  virtual string s67
      {
         get {  return m_s67; }
         set {  m_s67 = value; }
      }

      public  virtual string s68
      {
         get {  return m_s68; }
         set {  m_s68 = value; }
      }

      public  virtual string s69
      {
         get {  return m_s69; }
         set {  m_s69 = value; }
      }

      public  virtual string s70
      {
         get {  return m_s70; }
         set {  m_s70 = value; }
      }

      public  virtual string s71
      {
         get {  return m_s71; }
         set {  m_s71 = value; }
      }

      public  virtual string s72
      {
         get {  return m_s72; }
         set {  m_s72 = value; }
      }

      public  virtual string s73
      {
         get {  return m_s73; }
         set {  m_s73 = value; }
      }

      public  virtual string s74
      {
         get {  return m_s74; }
         set {  m_s74 = value; }
      }

      public  virtual string s75
      {
         get {  return m_s75; }
         set {  m_s75 = value; }
      }

      public  virtual string s76
      {
         get {  return m_s76; }
         set {  m_s76 = value; }
      }

      public  virtual string s77
      {
         get {  return m_s77; }
         set {  m_s77 = value; }
      }

      public  virtual string s78
      {
         get {  return m_s78; }
         set {  m_s78 = value; }
      }

      public  virtual string s79
      {
         get {  return m_s79; }
         set {  m_s79 = value; }
      }

      public  virtual string s80
      {
         get {  return m_s80; }
         set {  m_s80 = value; }
      }

      public  virtual string s81
      {
         get {  return m_s81; }
         set {  m_s81 = value; }
      }

      public  virtual string s82
      {
         get {  return m_s82; }
         set {  m_s82 = value; }
      }

      public  virtual string s83
      {
         get {  return m_s83; }
         set {  m_s83 = value; }
      }

      public  virtual string s84
      {
         get {  return m_s84; }
         set {  m_s84 = value; }
      }

      public  virtual string s85
      {
         get {  return m_s85; }
         set {  m_s85 = value; }
      }

      public  virtual string s86
      {
         get {  return m_s86; }
         set {  m_s86 = value; }
      }

      public  virtual string s87
      {
         get {  return m_s87; }
         set {  m_s87 = value; }
      }

      public  virtual string s88
      {
         get {  return m_s88; }
         set {  m_s88 = value; }
      }

      public  virtual string s89
      {
         get {  return m_s89; }
         set {  m_s89 = value; }
      }

      public  virtual string s90
      {
         get {  return m_s90; }
         set {  m_s90 = value; }
      }

      public  virtual string s91
      {
         get {  return m_s91; }
         set {  m_s91 = value; }
      }

      public  virtual string s92
      {
         get {  return m_s92; }
         set {  m_s92 = value; }
      }

      public  virtual string s93
      {
         get {  return m_s93; }
         set {  m_s93 = value; }
      }

      public  virtual string s94
      {
         get {  return m_s94; }
         set {  m_s94 = value; }
      }

      public  virtual string s95
      {
         get {  return m_s95; }
         set {  m_s95 = value; }
      }

      public  virtual string s96
      {
         get {  return m_s96; }
         set {  m_s96 = value; }
      }

      public  virtual string s97
      {
         get {  return m_s97; }
         set {  m_s97 = value; }
      }

      public  virtual string s98
      {
         get {  return m_s98; }
         set {  m_s98 = value; }
      }

      public  virtual string s99
      {
         get {  return m_s99; }
         set {  m_s99 = value; }
      }

      public  virtual string s100
      {
         get {  return m_s100; }
         set {  m_s100 = value; }
      }

      public  virtual string s101
      {
         get {  return m_s101; }
         set {  m_s101 = value; }
      }

      public  virtual string s102
      {
         get {  return m_s102; }
         set {  m_s102 = value; }
      }

      public  virtual string s103
      {
         get {  return m_s103; }
         set {  m_s103 = value; }
      }

      public  virtual string s104
      {
         get {  return m_s104; }
         set {  m_s104 = value; }
      }

      public  virtual string s105
      {
         get {  return m_s105; }
         set {  m_s105 = value; }
      }

      public  virtual string s106
      {
         get {  return m_s106; }
         set {  m_s106 = value; }
      }

      public  virtual string s107
      {
         get {  return m_s107; }
         set {  m_s107 = value; }
      }

      public  virtual string s108
      {
         get {  return m_s108; }
         set {  m_s108 = value; }
      }

      public  virtual string s109
      {
         get {  return m_s109; }
         set {  m_s109 = value; }
      }

      public  virtual string s110
      {
         get {  return m_s110; }
         set {  m_s110 = value; }
      }

      public  virtual string s111
      {
         get {  return m_s111; }
         set {  m_s111 = value; }
      }

      public  virtual string s112
      {
         get {  return m_s112; }
         set {  m_s112 = value; }
      }

      public  virtual string s113
      {
         get {  return m_s113; }
         set {  m_s113 = value; }
      }

      public  virtual string s114
      {
         get {  return m_s114; }
         set {  m_s114 = value; }
      }

      public  virtual string s115
      {
         get {  return m_s115; }
         set {  m_s115 = value; }
      }

      public  virtual string s116
      {
         get {  return m_s116; }
         set {  m_s116 = value; }
      }

      public IList 商住区片
      {
         get {  return m_商住区片_土地级别; }
         set {  m_商住区片_土地级别 = value; }
      }

      #endregion

   }
}
