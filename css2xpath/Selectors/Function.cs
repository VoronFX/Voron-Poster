using System;
using System.Collections.Generic;

namespace css2xpath.Selectors {
    public class Function : ISelector {
        private readonly string _name;
        private readonly object _param;
        private readonly ISelector _selector;

        public Function(ISelector selector, string name, object param) {
            this._selector = selector;
            this._name = name;
            this._param = param;
        }

        public XPathExpr GetXPath() {
            XPathExpr result = null;

            switch (this._name) {
                case "nth-child":
                    return this.MakeNthChild();
                case "nth-last-child":
                    return this.MakeNthChild(last: true);
                case "nth-of-type":
                    if (this._selector.GetXPath().GetElement() == "*") {
                        throw new NotImplementedException("*:nth-of-type() is not implemented");
                    }

                    return this.MakeNthChild(addNameTest: false);
                case "nth-last-of-type":
                    return this.MakeNthChild(last: true, addNameTest: false);
                case "contains":
                    result = this._selector.GetXPath();
                    result.AddCondition(string.Format("contains(css:lower-case(string(.)), '{0}')", this._param.ToString().ToLower()));
                    return result;
                case "not":
                    var condition = ((ISelector) this._param).GetXPath().GetCondition();
                    result = this._selector.GetXPath();
                    result.AddCondition(string.Format("not({0})", condition));
                    return result;
                default:
                    throw new ExpressionException(string.Format("Unsupported function: {0}", this._name));
            }
        }

        private XPathExpr MakeNthChild(bool last = false, bool addNameTest = true) {
            var result = this._selector.GetXPath();
            var series = Parser.ParseSeries(this._param);

            if (!last && series.Item1 == 0 && series.Item2 == 0) {
                result.AddCondition("false() and position() = 0");
                return result;
            }

            if (addNameTest) {
                result.AddNameTest();
            }

            result.AddStarPrefix();

            if (series.Item1 == 0) {
                var bText = series.Item2.ToString();
                if (last) {
                    bText = string.Format("last() - {0}", bText);
                }
                result.AddCondition(string.Format("position() = {0}", bText));
                return result;
            }

            var a = series.Item1;
            var b = series.Item2;
            if (last) {
                a = -a;
                b = -b;
            }

            var bNeg = (-b).ToString();
            if (b <= 0) {
                bNeg = string.Format("+{0}", bNeg);
            }

            var expr = new List<string>();
            if (a != 1) {
                expr.Add(string.Format("(position() {0}) mod {1} = 0", bNeg, a));
            }

            if (b >= 0) {
                expr.Add(string.Format("position() >= {0}", b));
            } else if (b < 0 && last) {
                expr.Add(string.Format("position() < (last() {0})", b));
            }

            var exprString = string.Join(" and ", expr);
            if (expr.Count > 0) {
                result.AddCondition(exprString);
            }

            return result;
        }
    }
}