namespace Simetron.Data {
	using System;
	using System.Collections;
	using System.Xml.Serialization;

	public class Container {
		private Hashtable children;

		public Container () {
			children = new Hashtable ();
		}
		
		[XmlIgnoreAttribute]
		public object this[Reference reference] {
			get {
				return children[reference];
			}
			set {
				children[reference] = value;
			}
		}
		
		public Reference[] References {
			get {
				ICollection keyCollection = children.Keys;
				int count = keyCollection.Count;
				Reference[] references = new Reference[count];
				keyCollection.CopyTo (references, 0);
				return references;
			}
			set {
				children.Clear ();
				foreach (Reference reference in value) {
					children[reference] = null;
				}
			}
		}

		[XmlIgnoreAttribute]
		public string[] Names {
			get {
				Reference[] references = this.References;
				string[] names = new string[references.Length];
				for (int i = references.Length; i-- > 0; ) {
					Reference reference = references[i];
					names[i] = reference.Name;
				}
				return names;
			}
		}

		[XmlIgnoreAttribute]
		public StoreMode[] Modes {
			get {
				Reference[] references = this.References;
				StoreMode[] modes = new StoreMode[references.Length];
				for (int i = references.Length; i-- > 0; ) {
					Reference reference = references[i];
					modes[i] = reference.Mode;
				}
				return modes;
			}
		}

	}
}
