namespace Simetron.GUI.Core 
{
	using System;
	using System.Collections;

	public sealed class Document 
	{
		static int count = 0;
		// the filename for this object
		string filename;
		// the name
		string name;
		// any object
		object document;

		public Document (object document, string filename, string name)
		{
			this.FileName = filename;
			this.Object = document;
			this.Name = name;
		}

		public string FileName {
			get { 
				return filename;
			}
			set {
				filename = value;
			}
		}

		public string Name {
			get {
				return name;
			}
			set {
				// if value is null, assign a default name
				if (value != null) {
					name = value;
				} else {
					name = "Unnamed-" + ++count;
				}
			}
		}

		public object Object {
			get {
				return document;
			}
			set {
				document = value;
			}
		}
	}
}
