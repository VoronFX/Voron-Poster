namespace css2xpath.Selectors {
    public class Class : ISelector {
        private readonly string _className;
        private readonly ISelector _selector;

        public Class(ISelector selector, string className) {
            this._selector = selector;
            this._className = className;
        }

        public XPathExpr GetXPath() {
            var result = this._selector.GetXPath();
            result.AddCondition(string.Format("contains(concat(' ', normalize-space(@class), ' '), ' {0} ')", this._className));
            return result;
        }
    }
}