namespace SkyMap.Net.Gui
{
    using System;
    using System.Collections;

    internal class StackArray : ArrayList
    {
        private int m_iCount = 0;
        private ArrayList m_syncList = null;

        public StackArray()
        {
            this.m_syncList = ArrayList.Synchronized((ArrayList) this);
        }

        public object Peek(StackMode stackMode)
        {
            if (this.m_syncList.Count > 0)
            {
                return this.m_syncList[this.m_syncList.Count - 1];
            }
            return null;
        }

        public void Pop(object targetForm, StackMode stackMode)
        {
            if (stackMode != StackMode.None)
            {
                if (stackMode == StackMode.FirstAvailable)
                {
                    for (int i = 0; i < this.m_syncList.Count; i++)
                    {
                        if (this.m_syncList[i] == targetForm)
                        {
                            this.m_syncList[i] = null;
                            this.m_iCount--;
                            break;
                        }
                    }
                }
                else if ((stackMode == StackMode.Top) && this.m_syncList.Contains(targetForm))
                {
                    this.m_syncList.Remove(targetForm);
                    this.m_iCount--;
                }
                if (this.m_iCount == 0)
                {
                    this.m_syncList.Clear();
                }
            }
        }

        public object Push(object newObject, StackMode stackMode)
        {
            bool flag = false;
            object obj2 = null;
            if (stackMode == StackMode.None)
            {
                return null;
            }
            if (stackMode != StackMode.FirstAvailable)
            {
                if (stackMode == StackMode.Top)
                {
                    if (this.m_syncList.Count > 0)
                    {
                        obj2 = this.m_syncList[this.m_syncList.Count - 1];
                    }
                    this.m_syncList.Add(newObject);
                }
            }
            else
            {
                for (int i = 0; i < this.m_syncList.Count; i++)
                {
                    if (this.m_syncList[i] == null)
                    {
                        this.m_syncList[i] = newObject;
                        flag = true;
                        break;
                    }
                    obj2 = this.m_syncList[i];
                }
                if (!flag)
                {
                    this.m_syncList.Add(newObject);
                }
            }
            this.m_iCount++;
            return obj2;
        }
    }
}

