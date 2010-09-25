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
   public class 容积率修正
   {
      #region 构造函数
      public 容积率修正()
      {}

      public 容积率修正(int id,string 土地类型,double 容积率,double 修正系数)
      {
         this.m_id=id;
         this.m_土地类型=土地类型;
         this.m_容积率=容积率;
         this.m_修正系数=修正系数;
      }
      #endregion

      #region 成员
      private int m_id;
      private string m_土地类型;
      private double m_容积率;
      private double m_修正系数;
      #endregion


      #region 属性
      public  virtual int id
      {
         get {  return m_id; }
         set {  m_id = value; }
      }

      public  virtual string 土地类型
      {
         get {  return m_土地类型; }
         set {  m_土地类型 = value; }
      }

      public  virtual double 容积率
      {
         get {  return m_容积率; }
         set {  m_容积率 = value; }
      }

      public  virtual double 修正系数
      {
         get {  return m_修正系数; }
         set {  m_修正系数 = value; }
      }

      #endregion

   }
}
