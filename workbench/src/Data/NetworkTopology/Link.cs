using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Simetron.Logging;


namespace Simetron.Data.NetworkTopology {
	public class Link : IdentifiedElement {
		private Node origin;
		private string originID;
		private Node destination;
		private string destinationID;

		public Link () {}

		public Link (Node origin, Node destination) {
			this.Origin = origin;
			this.Destination = destination;
		}

		[XmlIgnoreAttribute]
		public Node Origin {
			get {
				return origin;
			}
			set {
				Logger.Assert (value != null, "null value");
				if (origin != null) {
					origin.RemoveLink (this);
				}
				origin = value;
				originID = origin.ID;
				origin.Add (this);
			}
		}

		[XmlIgnoreAttribute]		
		public Node Destination {
			get {
				return destination;
			}
			set {
				Logger.Assert (value != null, "null value");
				if (destination != null) {
					destination.RemoveLink (this);
				}
				destination  = value;
				destinationID = destination.ID;
				destination.Add (this);
			}
		}

		public string OriginID {
			get {
				return originID;
			}
			set {
				originID = value;
			}
		}
		
		public string DestinationID {
			get {
				return destinationID;
			}
			set {
				destinationID = value;
			}
		}

		[XmlIgnoreAttribute]
		public override IdentifiedElement[] Children {
			get {
				return null;
			}
		}

		// composite methods
		public override void Add (IdentifiedElement child) {
			// TODO
		}

		public override void Remove (IdentifiedElement child) {
			// TODO
		}

		public override IdentifiedElement[] GetChildrenAt (Point p) {
			// TODO
			return null;
		}

		public override IdentifiedElement GetChild (string id) {
			// TODO
			return null;
		}

		// graphic methods
		public override bool Intersects (Point p) {
			return false;
		}

		public override void MoveTo (Point p) {
			// TODO
		}
	}
}
