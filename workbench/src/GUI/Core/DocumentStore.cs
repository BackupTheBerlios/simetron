using System;
using System.Collections;

namespace Simetron.GUI.Core {
	public delegate void DocumentAddedEventHandler (object obj, EventArgs args);

	public sealed class DocumentStore {
		public event DocumentAddedEventHandler OnDocumentAdded;

		private Hashtable documents = new Hashtable ();
		
		public DocumentStore () {
			documents = new Hashtable ();
			Initialize ();
		}

		private void Initialize () {
			// the document store is backed up by the filesystem
			
		}

		public object this [object key] {
			get {
				return documents[key];
			}
			set {
				documents[key] = value;
				if (OnDocumentAdded != null) {
					OnDocumentAdded (value, new EventArgs ());
				}
			}
		}		
	}
}
