namespace css2xpath {
    public class Token {
        private readonly string _contents;

        public Token(string contents) {
            this._contents = contents;
        }

        public string Contents {
            get { return this._contents; }
        }
    }

    public class Symbol : Token {
        public Symbol(string contents) : base(contents) { }
    }

    public class Str : Token {
        public Str(string contents) : base(contents) { }
    }
}