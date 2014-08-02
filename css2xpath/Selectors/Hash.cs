namespace css2xpath.Selectors {
    public class Hash : ISelector {
        private readonly string _id;
        private readonly ISelector _selector;

        public Hash(ISelector selector, string id) {
            this._selector = selector;
            this._id = id;
        }

        public XPathExpr GetXPath() {
            var path = this._selector.GetXPath();
            path.AddCondition(string.Format("@id = '{0}'", this._id));
            return path;
        }
    }
}