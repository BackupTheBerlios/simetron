using System;
using System.Xml.Serialization;
using Simetron.Data;

namespace Simetron.Data.NetworkTopology {
	public delegate void AddedEventHandler (object element, EventArgs args);
	public delegate void ChangedEventHandler (object element, EventArgs args);
	public delegate void RemovedEventHandler (object element, EventArgs args);

	public abstract class IdentifiedElement : IdentifiedObject {
		// composite methods
		public abstract void Add (IdentifiedElement child);
		public abstract void Remove (IdentifiedElement child);
		public abstract IdentifiedElement[] GetChildrenAt (Point p);
		public abstract IdentifiedElement GetChild (string id);
		public abstract IdentifiedElement[] Children {get;}
		// geometric methods
		public abstract bool Intersects (Point p);
		public abstract void MoveTo (Point p);
		// need to add other geometric transformations (rotate, scale, etc.)
	}
}
