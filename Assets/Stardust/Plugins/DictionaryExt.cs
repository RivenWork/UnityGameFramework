using System;
using System.Collections.Generic;

namespace Stardust
{
    public static class DictionaryExt
    {
        public static T FindByValue<T, U>(this Dictionary<T, U> dic, Predicate<U> match)
        {
            foreach (var item in dic)
            {
                if (match(item.Value))
                {
                    return item.Key;
                }
            }
            return default(T);
        }
    }
}
