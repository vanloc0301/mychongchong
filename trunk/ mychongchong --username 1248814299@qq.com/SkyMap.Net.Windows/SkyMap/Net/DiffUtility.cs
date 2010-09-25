namespace SkyMap.Net
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class DiffUtility
    {
        public static int Compare<T>(IList<T> a, IList<T> b)
        {
            return Compare<T>(a, b, Comparer.Default);
        }

        public static int Compare(IList a, IList b)
        {
            return Compare(a, b, Comparer.Default);
        }

        public static int Compare(SortedList a, SortedList b)
        {
            return Compare(a, b, Comparer.Default);
        }

        public static int Compare<T>(IList<T> a, IList<T> b, IComparer comparer)
        {
            if ((a == null) || (b == null))
            {
                return 1;
            }
            if (a.Count != b.Count)
            {
                return Math.Sign((int) (a.Count - b.Count));
            }
            int num = (a.Count < b.Count) ? a.Count : b.Count;
            for (int i = 0; i < num; i++)
            {
                if ((a[i] is IComparable) && (b[i] is IComparable))
                {
                    int num3 = comparer.Compare(a[i], b[i]);
                    if (num3 != 0)
                    {
                        return num3;
                    }
                }
            }
            return (a.Count - b.Count);
        }

        public static int Compare(IList a, IList b, IComparer comparer)
        {
            if ((a == null) || (b == null))
            {
                return 1;
            }
            if (a.Count != b.Count)
            {
                return Math.Sign((int) (a.Count - b.Count));
            }
            int num = (a.Count < b.Count) ? a.Count : b.Count;
            for (int i = 0; i < num; i++)
            {
                if ((a[i] is IComparable) && (b[i] is IComparable))
                {
                    int num3 = comparer.Compare(a[i], b[i]);
                    if (num3 != 0)
                    {
                        return num3;
                    }
                }
            }
            return (a.Count - b.Count);
        }

        public static int Compare(SortedList a, SortedList b, IComparer comparer)
        {
            if ((a == null) || (b == null))
            {
                return 1;
            }
            int num2 = (a.Count < b.Count) ? a.Count : b.Count;
            for (int i = 0; i < num2; i++)
            {
                int num;
                if (0 != (num = comparer.Compare(a.GetByIndex(i), b.GetByIndex(i))))
                {
                    return num;
                }
            }
            return (a.Count - b.Count);
        }

        private static bool Contains(IList list, object value, IComparer comparer)
        {
            foreach (object obj2 in list)
            {
                if (0 == comparer.Compare(obj2, value))
                {
                    return true;
                }
            }
            return false;
        }

        public static int GetAddedItems(IList original, IList changed, IList result)
        {
            return GetAddedItems(original, changed, result, Comparer.Default);
        }

        public static int GetAddedItems(IList original, IList changed, IList result, IComparer comparer)
        {
            int num = 0;
            if ((changed != null) && (result != null))
            {
                if (original == null)
                {
                    foreach (object obj2 in changed)
                    {
                        result.Add(obj2);
                    }
                    return changed.Count;
                }
                foreach (object obj2 in changed)
                {
                    if (!Contains(original, obj2, comparer))
                    {
                        result.Add(obj2);
                        num++;
                    }
                }
            }
            return num;
        }

        public static int GetRemovedItems(IList original, IList changed, IList result)
        {
            return GetRemovedItems(original, changed, result, Comparer.Default);
        }

        public static int GetRemovedItems(IList original, IList changed, IList result, IComparer comparer)
        {
            return GetAddedItems(changed, original, result, comparer);
        }
    }
}

