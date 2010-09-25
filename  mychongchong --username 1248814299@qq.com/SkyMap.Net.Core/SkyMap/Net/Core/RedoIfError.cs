namespace SkyMap.Net.Core
{
    using System;

    public static class RedoIfError
    {
        public static T Execute<T>(RedoAction<T> action, int exeCount)
        {
            Exception exception = null;
            for (int i = 0; i < exeCount; i++)
            {
                try
                {
                    return action();
                }
                catch (Exception exception2)
                {
                    exception = exception2;
                }
            }
            throw exception;
        }

        public static void Execute(RedoAction action, int exeCount)
        {
            Exception exception = null;
            for (int i = 0; i < exeCount; i++)
            {
                try
                {
                    action();
                }
                catch (Exception exception2)
                {
                    exception = exception2;
                }
            }
            throw exception;
        }
    }
}

