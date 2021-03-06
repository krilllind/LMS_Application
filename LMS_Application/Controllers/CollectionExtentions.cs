﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS_Application.Controllers
{
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> newItems)
        {
            foreach (T item in newItems)
            {
                collection.Add(item);
            }
        }
    }
}