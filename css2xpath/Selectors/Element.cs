namespace css2xpath.Selectors {
    public class Element : ISelector {
        private readonly string _element;
        private readonly string _namespace;

        public Element(string xNamespace, string element) {
            this._namespace = xNamespace;
            this._element = element;
        }

        public XPathExpr GetXPath() {
            var element = this._element.ToLower();
            if (this._namespace != "*") {
                element = string.Format("{0}:{1}", this._namespace, this._element);
            }

            return new XPathExpr(element: element);
        }

        public override string ToString() {
            if (this._namespace == "*") {
                return this._element;
            }

            return string.Format("{0}|{1}", this._namespace, this._element);
        }
    }
}