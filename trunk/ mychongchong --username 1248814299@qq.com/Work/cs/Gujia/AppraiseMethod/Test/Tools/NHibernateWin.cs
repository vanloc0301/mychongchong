using System;
using NHibernate.Cfg;
using NHibernate;
using System.Windows.Forms;
namespace Utility
{
  /// <summary>
  ///
  ///功能描述:    
  ///开发者:  
  ///建立时间:  2010-2-1 0:00:00
  ///修订描述:    
  ///进度描述:    
  /// </summary>
  public static  class NHibernateSession
  {
         private static readonly ISessionFactory sessionFactory;
         private static ISession currentSession;
         static NHibernateSession()
         {
                 sessionFactory = new Configuration().Configure(Application.StartupPath + "\\hibernate.cfg.config" ).BuildSessionFactory();
         }

         /// <summary>
         /// 取得当前Session
         /// </summary>
         /// <returns></returns>
         public static ISession GetCurrentSession()
         {
                if (currentSession == null)
                {
                        currentSession = sessionFactory.OpenSession();
                }
                return currentSession;
         }

         /// <summary>
         /// 关闭当前Session
         /// </summary>
         public static void CloseSession()
          {
                 if (currentSession == null)
                 {
                        // No current session
                        return;
                 }
                currentSession.Close();
          }

         /// <summary>
         /// 关闭Factory
         /// </summary>
         public static void  CloseSessionFactory()
          {
                 if (sessionFactory != null)
                 {
                        sessionFactory.Close();
                 }
          }
  }
}
