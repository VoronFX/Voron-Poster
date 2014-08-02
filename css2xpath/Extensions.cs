using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace css2xpath {
    public static class Extensions {
        public static int End(this Match match) {
            return match.Index + match.Length;
        }

        public static bool In<T>(this T source, IEnumerable<T> items) {
            if (null == source) {
                throw new ArgumentNullException("source");
            }
            return items.Contains(source);
        }
    }
}