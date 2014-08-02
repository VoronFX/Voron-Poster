using System.Collections.Generic;
using System.Linq;

namespace css2xpath {
    public class XPathExprOr : XPathExpr {
        private readonly IEnumerable<XPathExpr> _items;
        private readonly string _prefix;

        public XPathExprOr(IEnumerable<XPathExpr> items, string prefix = null) {
            this._items = items;
            this._prefix = prefix;
        }

        public override string ToString() {
            string prefix = this._prefix ?? "";
            return string.Join(" | ", this._items.Select(item => prefix + item.ToString()));
        }
    }
}