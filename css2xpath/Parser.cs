using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using css2xpath.Selectors;

namespace css2xpath {
    public static class Parser {
        public static ISelector Parse(string input) {
            var stream = new TokenStream(Tokenizer.Tokenize(input).ToList());
            return ParseSelectorGroup(stream);
        }

        public static ISelector ParseSelectorGroup(TokenStream stream) {
            var result = new List<ISelector>();
            while (true) {
                result.Add(ParseSelector(stream));
                if (stream.Peek().Contents == ",") {
                    stream.Next();
                } else {
                    break;
                }
            }

            return result.Count == 1 ? result.Single() : new Or(result);
        }

        public static ISelector ParseSelector(TokenStream stream) {
            var result = ParseSimpleSelector(stream);

            while (true) {
                var peek = stream.Peek();

                if (peek.Contents == "" || peek.Contents == ",") {
                    return result;
                }

                var combinator = peek.Contents.In(new[] {"+", ">", "~"}) ? stream.Next().Contents[0] : ' ';

                var nextSelector = ParseSimpleSelector(stream);
                result = new CombinedSelector(result, combinator, nextSelector);
            }
        }

        public static ISelector ParseSimpleSelector(TokenStream stream) {
            var peek = stream.Peek();
            string element;
            string xNamespace;
            if (peek.Contents != "*" && !(peek is Symbol)) {
                element = "*";
                xNamespace = "*";
            } else {
                var next = stream.Next();
                if (next.Contents != "*" && !(next is Symbol)) {
                    throw new SelectorSyntaxException(string.Format("Expected symbol, got {0}", next.GetType().Name));
                }

                if (stream.Peek().Contents == "|") {
                    xNamespace = next.Contents;
                    stream.Next();
                    element = stream.Next().Contents;
                    if (element != "*" && !(next is Symbol)) {
                        throw new SelectorSyntaxException(string.Format("Expected symbol, got {0}", next.GetType().Name));
                    }
                } else {
                    xNamespace = "*";
                    element = next.Contents;
                }
            }

            ISelector result = new Element(xNamespace, element);
            var hasHash = false;

            while (true) {
                peek = stream.Peek();

                if (peek.Contents == "#") {
                    if (hasHash) {
                        break;
                    }

                    stream.Next();
                    result = new Hash(result, stream.Next().Contents);
                    hasHash = true;
                } else if (peek.Contents == ".") {
                    stream.Next();
                    result = new Class(result, stream.Next().Contents);
                } else if (peek.Contents == "[") {
                    stream.Next();
                    result = ParseAttribute(result, stream);
                    var next = stream.Next();
                    if (next.Contents != "]") {
                        throw new SelectorSyntaxException(string.Format("] expected, got {0}", next.Contents));
                    }
                } else if (peek.Contents == ":" || peek.Contents == "::") {
                    stream.Next();
                    var function = stream.Next();
                    if (!(function is Symbol)) {
                        throw new SelectorSyntaxException(string.Format("Expected symbol, got {0}", function));
                    }

                    if (stream.Peek().Contents == "(") {
                        stream.Next();
                        peek = stream.Peek();

                        Object selector;
                        int asInt;
                        if (peek is Str) {
                            selector = stream.Next().Contents;
                        } else if (peek is Symbol && int.TryParse(peek.Contents, out asInt)) {
                            selector = asInt;
                            stream.Next();
                        } else {
                            selector = ParseSimpleSelector(stream);
                        }

                        var next = stream.Next();
                        if (next.Contents != ")") {
                            throw new SelectorSyntaxException(string.Format("Expected ), got {0} and {1}", next.Contents, selector));
                        }

                        result = new Function(result, function.Contents, selector);
                    } else {
                        result = new Pseudo(result, function.Contents);
                    }
                } else {
                    if (peek.Contents == " ") {
                        stream.Next();
                    }

                    break;
                }
            }

            return result;
        }

        public static Attrib ParseAttribute(ISelector selector, TokenStream stream) {
            var attrib = stream.Next();
            string xNamespace;
            if (stream.Peek().Contents == "|") {
                xNamespace = attrib.Contents;
                stream.Next();
                attrib = stream.Next();
            } else {
                xNamespace = "*";
            }

            if (stream.Peek().Contents == "]") {
                return new Attrib(selector, xNamespace, attrib.Contents, "exists", null);
            }

            var oper = stream.Next().Contents;
            if (!oper.In(new[] {"^=", "$=", "*=", "=", "~=", "|=", "!="})) {
                throw new SelectorSyntaxException(string.Format("Operator expected, got {0}", oper));
            }

            var value = stream.Next();
            if (value is Symbol || value is Str) {
                return new Attrib(selector, xNamespace, attrib.Contents, oper, value.Contents);
            }

            throw new SelectorSyntaxException(string.Format("Expected string or symbol, got {0}", value));
        }

        public static Tuple<int, int> ParseSeries(object obj) {
            if (obj is int) {
                return new Tuple<int, int>(0, (int) obj);
            }

            var text = obj.ToString().Replace(" ", "");
            if (text == "*") {
                return new Tuple<int, int>(0, 0);
            }

            if (text == "odd") {
                return new Tuple<int, int>(2, 1);
            }

            if (text == "even") {
                return new Tuple<int, int>(2, 0);
            }

            if (text == "n") {
                return new Tuple<int, int>(1, 0);
            }

            if (!text.Contains('n')) {
                return new Tuple<int, int>(0, int.Parse(text));
            }

            var regex = new Regex(@"^([+\-]?\d+)?n([+\-]\d+)$");
            var match = regex.Match(text.Trim());
            if (!match.Success) {
                throw new ExpressionException(string.Format("Invalid series: {0}", text));
            }

            var a = int.Parse(match.Groups[1].Value);
            var b = int.Parse(match.Groups[2].Value);
            return new Tuple<int, int>(a, b);
        }
    }
}