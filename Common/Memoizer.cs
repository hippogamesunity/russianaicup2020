﻿using System;
using System.Collections.Generic;

namespace aicup2020.Common
{
    public class Memoizer
    {
        public static Func<TReturn> Memoize<TReturn>(Func<TReturn> func)
        {
            object cache = null;

            return () =>
            {
                if (cache == null) cache = func();

                return (TReturn) cache;
            };
        }

        public static Func<TSource, TReturn> Memoize<TSource, TReturn>(Func<TSource, TReturn> func)
        {
            var cache = new Dictionary<TSource, TReturn>();

            return s =>
            {
                if (!cache.ContainsKey(s))
                {
                    cache[s] = func(s);
                }

                return cache[s];
            };
        }

        public static Func<TSource1, TSource2, TReturn> Memoize<TSource1, TSource2, TReturn>(Func<TSource1, TSource2, TReturn> func)
        {
            var cache = new Dictionary<string, TReturn>();

            return (s1, s2) =>
            {
                var key = s1.GetHashCode() + "." + s2.GetHashCode();

                if (!cache.ContainsKey(key))
                {
                    cache[key] = func(s1, s2);
                }

                return cache[key];
            };
        }

        public static Func<TSource1, TSource2, TReturn> MemoizeFast<TSource1, TSource2, TReturn>(Func<TSource1, TSource2, TReturn> func)
        {
            var cache = new Dictionary<int, TReturn>();

            return (s1, s2) =>
            {
                var key = 1000 * s1.GetHashCode() + s2.GetHashCode();

                if (!cache.ContainsKey(key))
                {
                    cache[key] = func(s1, s2);
                }

                return cache[key];
            };
        }
    }
}
