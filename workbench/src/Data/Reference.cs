namespace Simetron.Data {
	using System;
	using System.Collections;
	using System.Xml.Serialization;

	public class Reference {
		private string name;
		private StoreMode mode;

		public Reference (string name, StoreMode mode) {
			this.name = name;
			this.mode = mode;
		}

		public Reference () {
		}

		[XmlAttribute]
		public string Name {
			get { return name; }
			set { name = value; }
		}

		[XmlAttribute]
		public StoreMode Mode {
			get { return mode; }
			set { mode = value; }
		}

		// required to insert in hashed collections
		public override bool Equals (object obj) {
			Reference other = obj as Reference;
			if (other == null) {
				return false;
			}		
			// check name
			if (this.name != null) {
				if (other.name == null) {
					return false;
				} else {					
					if (! (this.name.Equals (other.name))) {
						return false;
					}
				}						
			} else {
				if (other.name != null) {
					return false;
				}
			}

			// check mode
			if (this.mode != other.mode) {
				return false;
			}
			return true;
		}

		// required because Equals
		public override int GetHashCode () {
			int a1 = 0;
			if (name != null) {
				a1 = name.GetHashCode ();
			}
			int a2 = mode.GetHashCode ();
			return a1 ^ a2;
		}
		
	}
}
