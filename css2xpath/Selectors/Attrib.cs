using System;

namespace css2xpath.Selectors {
    public class Attrib : ISelector {
        private readonly string _attrib;
        private readonly string _namespace;
        private readonly string _operator;
        private readonly ISelector _selector;
        private readonly string _value;

        public Attrib(ISelector selector, string xNamespace, string attrib, string oper, string value) {
            this._selector = selector;
            this._namespace = xNamespace;
            this._attrib = attrib;
            this._operator = oper;
            this._value = value;
        }

        public XPathExpr GetXPath() {
            var result = this._selector.GetXPath();
            var attribute = this._namespace == "*" ? ("@" + this._attrib) : string.Format("@{0}:{1}", this._namespace, this._attrib);

            switch (this._operator) {
                case "exists":
                    if (!string.IsNullOrEmpty(this._value)) {
                        throw new ExpressionException("Value should be empty");
                    }
                    result.AddCondition(attribute);
                    break;
                case "=":
                    result.AddCondition(string.Format("{0} = '{1}'", attribute, this._value));
                    break;
                case "!=":
                    if (string.IsNullOrEmpty(this._value)) {
                        result.AddCondition(string.Format("{0} != '{1}'", attribute, this._value));
                    } else {
                        result.AddCondition(string.Format("not({0}) or {1} != '{2}'", attribute, attribute, this._value));
                    }
                    break;
                case "~=":
                    result.AddCondition(string.Format("contains(concat(' ', normalize-space({0}), ' '), ' {1} ')", attribute, this._value));
                    break;
                case "|=":
                    result.AddCondition(string.Format("{0} = '{1}' or starts-with({2}, '{3}-')", attribute, this._value, attribute, this._value));
                    break;
                case "^=":
                    result.AddCondition(string.Format("starts-with({0}, '{1}')", attribute, this._value));
                    break;
                case "$=":
                    result.AddCondition(string.Format("substring({0}, string-length({1})-{2}) = '{3}'", attribute, attribute, this._value.Length - 1, this._value));
                    break;
                case "*=":
                    result.AddCondition(string.Format("contains({0}, '{1}')", attribute, this._value));
                    break;
                default:
                    throw new NotImplementedException(string.Format("Operator {0} is not supported", this._operator));
            }

            return result;
        }
    }
}