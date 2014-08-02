using System;

namespace css2xpath.Selectors {
    public class Pseudo : ISelector {
        private readonly ISelector _element;
        private string _name;

        public Pseudo(ISelector element, string name) {
            this._element = element;
            this._name = name;
        }

        public XPathExpr GetXPath() {
            var result = this._element.GetXPath();

            switch (this._name) {
                case "checked":
                    result.AddCondition("(@selected or @checked) and (name(.) = 'input' or name(.) = 'option')");
                    break;
                case "first-child":
                    result.AddStarPrefix();
                    result.AddNameTest();
                    result.AddCondition("position() = 1");
                    break;
                case "last-child":
                    result.AddStarPrefix();
                    result.AddNameTest();
                    result.AddCondition("position() = last()");
                    break;
                case "first-of-type":
                    if (result.GetElement() == "*") {
                        throw new NotImplementedException("*:first-of-type is not implemented");
                    }
                    result.AddStarPrefix();
                    result.AddCondition("position() = 1");
                    break;
                case "last-of-type":
                    if (result.GetElement() == "*") {
                        throw new NotImplementedException("*:last-of-type is not implemented");
                    }
                    result.AddStarPrefix();
                    result.AddCondition("position() = last()");
                    break;
                case "only-child":
                    result.AddNameTest();
                    result.AddStarPrefix();
                    result.AddCondition("last() = 1");
                    break;
                case "only-of-type":
                    if (result.GetElement() == "*") {
                        throw new NotImplementedException("*:only-of-type is not implemented");
                    }
                    result.AddCondition("last() = 1");
                    break;
                case "empty":
                    result.AddCondition("not(*) and not(normalize-space())");
                    break;
                default:
                    throw new NotImplementedException(string.Format("The pseudo-selector, {0}, is not implemented", this._name));
            }

            return result;
        }
    }
}