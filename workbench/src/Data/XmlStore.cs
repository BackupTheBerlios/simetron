using System;
using System.IO;
using System.Xml.Serialization;
using Simetron.Logging;

namespace Simetron.Data {
	public abstract class XmlStore : IStore {
		XmlSerializer serializer;
		FileStream fs;
		protected string filename;
		Type type;
		
		public XmlStore (Type t) {
			this.type = t;
			serializer = new XmlSerializer (t);
			serializer.UnknownNode+= new 
				XmlNodeEventHandler(OnUnknownNode);
			serializer.UnknownAttribute+= new 
				XmlAttributeEventHandler(OnUnknownAttribute);
		}

		public StoreMode Mode {
			get { return StoreMode.XML; }
		}

		public void OpenConnection (string connectionString) {
			Logger.Assert (connectionString != null, "A connection string must be set");
			if (fs != null) {
				CloseConnection ();
			}
			Logger.Debug ("Open connection: " +  connectionString);
			FileInfo finfo = new FileInfo (connectionString);
			DirectoryInfo dinfo = finfo.Directory;
			if (!dinfo.Exists) {
				dinfo.Create ();
				Logger.Debug ("Creating directory " + dinfo);
			}
			fs = new FileStream (connectionString, FileMode.OpenOrCreate);
			filename = connectionString;
		}
		
		public void CloseConnection () {
			if (fs != null) {
				fs.Close ();
				fs = null;
				Logger.Debug ("Close connection: " + filename);
				filename = null;
			}
		}

		public object Read () {
			Logger.Assert (fs != null, "Open connection first!");
			object obj = serializer.Deserialize (fs);
			fs.Close ();
			Logger.Assert (obj != null, "Could not read " + filename);
			PostReadTemplate (obj);
			return obj;
		}

		protected abstract void PostReadTemplate (object obj);
		protected abstract void PostWriteTemplate (object obj);

		public void Write (object obj) {
			Logger.Assert (fs != null, "No connection open!");
			if (obj.GetType () != type) {
				throw new ArgumentException ("This store is only for type " +
							     type);
			}
			serializer.Serialize (fs, obj);
			fs.Flush ();
			PostWriteTemplate (obj);
		}

		private void OnUnknownNode (object sender, XmlNodeEventArgs e) {
			// TODO: should be handled as an exception to the user
			Logger.Fail ("Unknown Node:" +   e.Name + "\t" + e.Text);
		}

		private void OnUnknownAttribute (object sender, XmlAttributeEventArgs e) {
			System.Xml.XmlAttribute attr = e.Attr;
			// TODO: should be handled as an exception to the user
			Logger.Fail ("Unknown attribute " + 
				    attr.Name + "='" + attr.Value + "'");
		}
	}
}
