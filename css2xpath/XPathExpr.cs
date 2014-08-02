namespace css2xpath {
    public class XPathExpr {
        private string _condition;
        private string _element;
        private string _path;
        private string _prefix;
        private bool _starPrefix;

        public XPathExpr(string prefix = null, string path = null, string element = "*", string condition = null, bool starPrefix = false) {
            this._prefix = prefix;
            this._path = path;
            this._element = element;
            this._condition = condition;
            this._starPrefix = starPrefix;
        }

        public override string ToString() {
            string path = "";
            path += this._prefix ?? "";
            path += this._path ?? "";
            path += this._element;

            if (this._condition != null) {
                path += string.Format("[{0}]", this._condition);
            }

            return path;
        }

        public void AddPrefix(string prefix) {
            if (this._prefix != null) {
                this._prefix = prefix + this._prefix;
            } else {
                this._prefix = prefix;
            }
        }

        public void AddCondition(string condition) {
            if (this._condition != null) {
                this._condition = string.Format("{0} and ({1})", this._condition, condition);
            } else {
                this._condition = condition;
            }
        }

        public void AddPath(string part) {
            if (this._path == null) {
                this._path = this._element;
            } else {
                this._path += this._element;
            }

            this._element = part;
        }

        public void AddNameTest() {
            if (this._element == "*") {
                return;
            }

            this.AddCondition(string.Format("name() = '{0}'", this._element));
            this._element = "*";
        }

        public void AddStarPrefix() {
            if (this._path != null) {
                this._path += "*/";
            } else {
                this._path = "*/";
            }

            this._starPrefix = true;
        }

        public void Join(string combiner, XPathExpr other) {
            string prefix = this + combiner;
            string path = (other._prefix ?? "") + (other._path ?? "");

            if (other._starPrefix && path == "*/") {
                path = "";
            }

            this._prefix = prefix;
            this._path = path;
            this._element = other._element;
            this._condition = other._condition;
        }

        public string GetElement() {
            return this._element;
        }

        public string GetCondition() {
            return this._condition;
        }
    }
}