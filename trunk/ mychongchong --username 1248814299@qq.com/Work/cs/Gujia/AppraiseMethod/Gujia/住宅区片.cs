using System;
using System.Collections;

namespace Gujia.Method
{
   /// <summary>
   ///功能描述    :    
   ///开发者      :    
   ///建立时间    :    2010-2-1 16:30:27
   ///修订描述    :    
   ///进度描述    :    
   ///版本号      :    1.0
   ///最后修改时间:    2010-2-1 16:30:27
   ///
   ///Function Description :    
   ///Developer                :    
   ///Builded Date:    2010-2-1 16:30:27
   ///Revision Description :    
   ///Progress Description :    
   ///Version Number        :    1.0
   ///Last Modify Date     :    2010-2-1 16:30:27
   /// </summary>
   public class 住宅区片
   {
      #region 构造函数
      public 住宅区片()
      {}

      public 住宅区片(string 镇区编号,string 镇区名称,double 区片号,double 区片价,住宅修正因素 住宅修正因素,double 级别价,string 四至,string 道路交通状况,string 基础设施状况,string 环境质量状况,string 地质状况,string 公共服务设施完备程度,string 繁华程度,string 其他)
      {
         this.m_镇区编号=镇区编号;
         this.m_镇区名称=镇区名称;
         this.m_区片号=区片号;
         this.m_区片价=区片价;
         this.m_住宅修正因素=住宅修正因素;
         this.m_级别价=级别价;
         this.m_四至=四至;
         this.m_道路交通状况=道路交通状况;
         this.m_基础设施状况=基础设施状况;
         this.m_环境质量状况=环境质量状况;
         this.m_地质状况=地质状况;
         this.m_公共服务设施完备程度=公共服务设施完备程度;
         this.m_繁华程度=繁华程度;
         this.m_其他=其他;
      }
      #endregion

      #region 成员
      protected IList m_住宅修正系数_区片号;
      private string m_镇区编号;
      private string m_镇区名称;
      private double m_区片号;
      private double m_区片价;
      protected 住宅修正因素 m_住宅修正因素;
      private double m_级别价;
      private string m_四至;
      private string m_道路交通状况;
      private string m_基础设施状况;
      private string m_环境质量状况;
      private string m_地质状况;
      private string m_公共服务设施完备程度;
      private string m_繁华程度;
      private string m_其他;
      #endregion


      #region 属性
      public  virtual string 镇区编号
      {
         get {  return m_镇区编号; }
         set {  m_镇区编号 = value; }
      }

      public  virtual string 镇区名称
      {
         get {  return m_镇区名称; }
         set {  m_镇区名称 = value; }
      }

      public  virtual double 区片号
      {
         get {  return m_区片号; }
         set {  m_区片号 = value; }
      }

      public  virtual double 区片价
      {
         get {  return m_区片价; }
         set {  m_区片价 = value; }
      }

      public  virtual 住宅修正因素 住宅修正因素
      {
         get {  return m_住宅修正因素; }
         set {  m_住宅修正因素 = value; }
      }

      public  virtual double 级别价
      {
         get {  return m_级别价; }
         set {  m_级别价 = value; }
      }

      public  virtual string 四至
      {
         get {  return m_四至; }
         set {  m_四至 = value; }
      }

      public  virtual string 道路交通状况
      {
         get {  return m_道路交通状况; }
         set {  m_道路交通状况 = value; }
      }

      public  virtual string 基础设施状况
      {
         get {  return m_基础设施状况; }
         set {  m_基础设施状况 = value; }
      }

      public  virtual string 环境质量状况
      {
         get {  return m_环境质量状况; }
         set {  m_环境质量状况 = value; }
      }

      public  virtual string 地质状况
      {
         get {  return m_地质状况; }
         set {  m_地质状况 = value; }
      }

      public  virtual string 公共服务设施完备程度
      {
         get {  return m_公共服务设施完备程度; }
         set {  m_公共服务设施完备程度 = value; }
      }

      public  virtual string 繁华程度
      {
         get {  return m_繁华程度; }
         set {  m_繁华程度 = value; }
      }

      public  virtual string 其他
      {
         get {  return m_其他; }
         set {  m_其他 = value; }
      }

      public IList 住宅修正系数
      {
         get {  return m_住宅修正系数_区片号; }
         set {  m_住宅修正系数_区片号 = value; }
      }

      #endregion

   }
}
