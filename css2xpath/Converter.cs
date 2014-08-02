using System.Text.RegularExpressions;
using css2xpath.Selectors;

namespace css2xpath {
    public static class Converter {
        private static readonly Regex ElementRegex = new Regex(@"^\w+\s*$");
        private static readonly Regex IdRegex = new Regex(@"^(\w*)#(\w+)\s*$");
        private static readonly Regex ClassRegex = new Regex(@"^(\w*)\.(\w+)\s*$");

        public static string CSSToXPath(string expr, string prefix = "descendant-or-self::") {
            var elementMatch = ElementRegex.Match(expr);
            if (elementMatch.Success) {
                return string.Format("{0}{1}", prefix, elementMatch.Value.Trim());
            }

            var idMatch = IdRegex.Match(expr);
            if (idMatch.Success) {
                return string.Format("{0}{1}[@id = '{2}']", prefix, (string.IsNullOrEmpty(idMatch.Groups[1].Value) ? "*" : idMatch.Groups[1].Value), idMatch.Groups[2].Value);
            }

            var classMatch = ClassRegex.Match(expr);
            if (classMatch.Success) {
                return string.Format("{0}{1}[contains(concat(' ', normalize-space(@class), ' '), ' {2} ')]", prefix, (string.IsNullOrEmpty(classMatch.Groups[1].Value) ? "*" : classMatch.Groups[1].Value), classMatch.Groups[2].Value);
            }

            var selector = Parser.Parse(expr);
            var xpath = selector.GetXPath();
            if (!string.IsNullOrEmpty(prefix)) {
                xpath.AddPrefix(prefix);
            }

            return xpath.ToString();
        }
    }
}
