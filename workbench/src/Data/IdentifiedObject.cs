using System;
using System.Xml.Serialization;

namespace Simetron.Data {
	public abstract class IdentifiedObject {
		[XmlAttribute]
		public string ID;
		[XmlAttribute]
		public string Label;
 
		public IdentifiedObject () {
			this.ID = System.Guid.NewGuid().ToString();
			this.Label = "Unnamed";
		}
	}
}
