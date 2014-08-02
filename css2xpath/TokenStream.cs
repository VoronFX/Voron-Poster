using System;
using System.Collections.Generic;

namespace css2xpath
{
    public class TokenStream : Queue<Token>
    {
        public TokenStream(IEnumerable<Token> items) : base(items) { }

        public Token Next()
        {
            if (base.Count == 0) return new Token("");
            return this.Dequeue();
        }

        public new Token Peek()
        {
            if (base.Count == 0) return new Token("");
            return base.Peek();
        }
    }
}