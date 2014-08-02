using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace css2xpath {
    public class Tokenizer {
        private static readonly Regex CommentsRegex = new Regex(@"/\*.*?\*/");
        private static readonly Regex WhitespaceRegex = new Regex(@"\G\s+");
        private static readonly Regex CountRegex = new Regex(@"\G[+-]?\d*n(?:[+-]\d+)?");
        private static readonly Regex IllegalSymbolRegex = new Regex(@"[^\w\\-]");

        public static IEnumerable<Token> Tokenize(string input) {
            var pos = 0;

            // Strip comments
            input = CommentsRegex.Replace(input, "");

            while (true) {
                var whitespaceMatch = WhitespaceRegex.Match(input, pos);
                int precedingWhitespacePos;
                if (whitespaceMatch.Success) {
                    precedingWhitespacePos = pos;
                    pos = whitespaceMatch.End();
                } else {
                    precedingWhitespacePos = 0;
                }

                if (pos >= input.Length) {
                    yield break;
                }

                var countMatch = CountRegex.Match(input, pos);
                if (countMatch.Success && countMatch.Value != "n") {
                    yield return new Symbol(input.Substring(pos, countMatch.End() - pos));
                    pos = countMatch.End();
                    continue;
                }

                var c = input[pos];
                var c2 = (pos + 2 > input.Length) ? "" : input.Substring(pos, 2);

                if (c2.In(new[] {"~=", "|=", "^=", "$=", "*=", "::", "!="})) {
                    yield return new Token(c2);
                    pos += 2;
                    continue;
                }

                if (">+~,.*=[]()|:#".Contains(c)) {
                    if (".#".Contains(c) && precedingWhitespacePos > 0) {
                        yield return new Token(" ");
                    }

                    yield return new Token(c.ToString());
                    pos += 1;
                    continue;
                }

                if (c == '"' || c == '\'') {
                    yield return TokenizeEscapedString(input, ref pos);
                    continue;
                }

                yield return TokenizeSymbol(input, ref pos);
            }
        }

        public static Str TokenizeEscapedString(string input, ref int pos) {
            var quote = input[pos];
            var start = pos + 1;
            var next = input.IndexOf(quote, start);
            if (next == -1) {
                throw new SelectorSyntaxException(string.Format("Expected closing {0} for string in: {1}", quote, input.Substring(start)));
            }

            // TODO: Still need to actually perform the escaping of backslashes?

            pos = next + 1;
            return new Str(input.Substring(start, next - start));
        }

        public static Symbol TokenizeSymbol(string input, ref int pos) {
            var start = pos;
            var match = IllegalSymbolRegex.Match(input, pos);
            if (!match.Success) {
                pos = input.Length;
                return new Symbol(input.Substring(start));
            }

            if (match.Index == pos) {
                throw new SelectorSyntaxException(string.Format("Unexpected symbol: {0} at {1}", input[pos], pos));
            }

            string result;

            if (!match.Success) {
                result = input.Substring(start);
                pos = input.Length;
            } else {
                result = input.Substring(start, match.Index - start);
                pos = match.Index;
            }

            // TODO: More backslash escaping stuff?

            return new Symbol(result);
        }
    }
}