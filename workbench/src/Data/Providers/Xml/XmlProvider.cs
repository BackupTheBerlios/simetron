//
// XmlProvider.cs  - An abstract provider for Simetron's format
//
// Author:
//   Bruno Fernandez-Ruiz (brunofr@olympum.com)
//
// Copyright (c) 2003 The Olympum Group,  http://www.olympum.com
// All Rights Reserved
//

namespace Simetron.Data.Providers.Xml
{
	using System;
	using System.IO;
	using System.Xml.Serialization;
	using Simetron.Data.Providers;
	using Simetron.Logging;

	public abstract class XmlProvider : FileProvider 
	{
		XmlSerializer serializer;
		Type type;

		public XmlProvider (Type t) {
			this.type = t;
			serializer = new XmlSerializer (t);
			serializer.UnknownNode+= 
				new XmlNodeEventHandler(OnUnknownNode);
			serializer.UnknownAttribute+= 
				new XmlAttributeEventHandler(OnUnknownAttribute);
		}

		public override ProviderMode Mode {
			get { return ProviderMode.Xml; }
		}
		
		public override object Read () 
		{
			Stream s = this.Stream;
			Logger.Assert (s != null, "Open connection first!");
			object obj = serializer.Deserialize (s);
			s.Close ();
			Logger.Assert (obj != null, "Could not read from stream");
			PostReadTemplate (obj);
			return obj;
		}

		public override void Write (object obj) 
		{
			Stream s = this.Stream;
			Logger.Assert (s != null, "Open connection first!");
			if (obj.GetType () != type) {
				throw new ArgumentException ("Wrong type: " + type);
			}
			serializer.Serialize (s, obj);
			s.Flush ();
			PostWriteTemplate (obj);
		}

		protected abstract void PostReadTemplate (object obj);
		protected abstract void PostWriteTemplate (object obj);

		private void OnUnknownNode (object sender, XmlNodeEventArgs e) 
		{
			// TODO: should be handled as an exception to the user
			Logger.Fail ("Unknown Node:" +   e.Name + "\t" + e.Text);
		}

		private void OnUnknownAttribute (object sender, XmlAttributeEventArgs e) 
		{
			System.Xml.XmlAttribute attr = e.Attr;
			// TODO: should be handled as an exception to the user
			Logger.Fail ("Unknown attribute " + 
				    attr.Name + "='" + attr.Value + "'");
		}
	}
}
