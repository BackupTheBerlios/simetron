namespace Simetron.Data.Providers.Mitsim 
{
	using System;
	using System.Collections;
	using System.IO;
	using System.Text;

	class MitsimLexer : yyParser.yyInput
	{
		private readonly Hashtable identifiers;
					
		private StreamReader reader;
		private int putback_char;
		private Object val;
		
		public MitsimLexer (Stream s) 
		{
			identifiers = new Hashtable ();
			reader = new StreamReader (s);
			putback_char = -1;
		}	
		
		public Hashtable Identifiers {
			get { return identifiers; }
		}

		private int getChar ()
		{
			if (putback_char != -1) {
				int x = putback_char;
				putback_char = -1;
				return x;
			}
			return reader.Read ();
		}

		private int peekChar ()
		{
			if (putback_char != -1) {
				return putback_char;
			}
			putback_char = reader.Read ();
			return putback_char;
		}

		public bool advance()
		{
			return peekChar () != -1;
		}

		public Object value() {
			return val;
		}


		public int token()
		{
			return nextToken ();
		}

		private int nextToken () {
			val = null;
			int tok =0;
			int c = 0;
			while ((c = getChar ()) != -1) {
				// eat spaces and other special chars
				if (c == ' ' || c == '\t' || c == '\f' || 
				    c == '\v' || c == '\r' || c == 0xa0) {
					continue;
				}
				if (c == '\n') {
					continue;
				}
				// handle # comment
				if (c == '#') {
					int d;
					while ((d = getChar ()) != -1 && 
					       (d != '\n') && d != '\r');
					continue;
				}
				// handle // and /* ... */ comments
				if (c == '/') {
					int d = peekChar ();
					if (d == '/') {
						getChar ();
						while ((d = getChar ()) != -1 && 
						       (d != '\n') && d != '\r');
						continue;
					} else if (d == '*') {
						getChar ();
						while ((d = getChar ()) != -1){
							if (d == '*' && 
							    peekChar () == '/'){
								getChar ();
								break;
							}
						}
						continue;
					}
				}
				// a hex
				if (c == '0') {
					int d = peekChar ();
					if (d == 'x' || d == 'X') {
						return getHexNumber ();
					}
				}
				// a number with sign or .
				if (c == '-' ||  c == '+' || c == '.') {
					int d = peekChar ();
					if (Char.IsDigit ((char)d)) {
						return getNumber (c);
					} else {
						return Token.ERROR;
					}
				}
				// a number without sign
				if (Char.IsDigit ((char)c)) {
					return getNumber (c);
				}
				if (c == '"') {
					int d = peekChar ();
					if (Char.IsLetter ((char)d)) {
						return getLitteral (c, true);
					}
				}
				// a string
				if (Char.IsLetter ((char)c)) {
					return getLitteral (c, false);
				}
				switch (c) {
				case '{':
					return Token.LCB;
				case '}':
					return Token.RCB;
				case '[':
					return Token.LSB;
				case ']':
					return Token.RSB;
				case ':':
					return Token.COL;
				case '-':
					return Token.HYP;
				}
				return Token.ERROR;
			}
			return Token.EOF;			
		}

		private int getHexNumber () 
		{
			getChar (); // get rid of x or X
			int d = 0;
			StringBuilder builder = new StringBuilder ();
			while ((d = getChar ()) != -1) {
				if (is_hex (d)) {
					builder.Append ((char) d);
				} else {
					putback_char = d;
					break;
				}
			}
			string chars = builder.ToString ();
			try {
				val = System.Int32.Parse (chars);
			} catch (OverflowException) {
				val = System.Int64.Parse (chars);
			}
			return Token.INTEGER;
		}
		
		private bool is_hex (int e)
		{
			return (e >= '0' && e <= '9') || 
				(e >= 'A' && e <= 'F') || 
				(e >= 'a' && e <= 'f');
		}
		
		private int getNumber (int s) 
		{
			bool is_real = false;
			bool is_sci = false;
			bool is_esigned = false;
			StringBuilder builder = new StringBuilder ();
			if (s == '.') {
				is_real = true;
			}
			builder.Append ((char) s);
			int c = 0;
			while ((c = getChar ()) != -1) {
				if (c >= '0' && c <= '9') {
					builder.Append ((char) c);
				} else if (c == '.') {
					if (!is_real) {
						builder.Append ((char) c);
						is_real = true;
					} else {
						return Token.ERROR;
					}
				} else if (c == 'e' || c == 'E') {
					if (!is_sci) {
						builder.Append ((char) c);
						is_sci = true;
						is_real = true;
					} else {
						return Token.ERROR;
					}
				} else if (c == '+' || c == '-') {
					if (is_sci && !is_esigned) {
						builder.Append((char) c);
						is_esigned = true;
					} else {
						return Token.ERROR;
					}
				} else {
					putback_char = c;
					break;
				}
			}
			string chars = builder.ToString ();
			if (!is_real) {
				try {
					val = System.Int32.Parse (chars);
				} catch (OverflowException) {
					val = System.Int64.Parse (chars);
				}
				return Token.INTEGER;
			} else {
				val = System.Single.Parse (chars);
				return Token.REAL;
			}
		}

		private int getLitteral (int s,  bool quoted) {
			int c = 0;
			int d = 0;
			StringBuilder builder = new StringBuilder ();
			if (!quoted) {
				builder.Append ((char)s);
			}
			while ((c = getChar ()) != -1) {
				if (quoted) {
					if (c != '"') {
						builder.Append ((char) c);
					} else {
						putback_char = -1;
						break;
					}
				} else {
					d = peekChar ();
					if (c >= 'a' && c <='z' ||
					    c >= 'A' && c <='Z' ||
					    c >= '0' && c <='9' ||
					    c == ' ' && d != -1 && Char.IsLetter ((char)d) ||
					    c == '-' && d != -1 && Char.IsLetter ((char)d)) {
						builder.Append ((char)c);
					} else {
						putback_char = c;
						break;
					}
				}
			}

			string chars = builder.ToString ();
			if (identifiers.ContainsKey (chars)) {
				return (int) identifiers[chars];
			}
			val = chars;
			return Token.LITTERAL;
		}
	}
}
