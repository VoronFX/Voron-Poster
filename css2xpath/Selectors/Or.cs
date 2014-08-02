using System.Collections.Generic;
using System.Linq;

namespace css2xpath.Selectors {
    public class Or : ISelector {
        private readonly IEnumerable<ISelector> _items;

        public Or(IEnumerable<ISelector> items) {
            this._items = items;
        }

        public XPathExpr GetXPath() {
            var paths = this._items.Select(item => item.GetXPath());
            return new XPathExprOr(paths);
        }
    }
}