using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Simetron.Logging;

namespace Simetron.Data.NetworkTopology {
	public class Network : IdentifiedElement {
		private ArrayList children;

		public Network () : base () {
			children = new ArrayList();
		}

		[XmlIgnoreAttribute]
		public override IdentifiedElement[] Children {
			get {
				return (IdentifiedElement[]) 
					children.ToArray (typeof (IdentifiedElement));
			}
		}

		public Node[] Nodes {
			get {
				ArrayList nodes = new ArrayList ();
				foreach (IdentifiedElement e in children) {
					Node n = e as Node;
					if (n != null) {
						nodes.Add (n);
					}
				}
				return (Node[]) nodes.ToArray (typeof (Node));
			}
			set {
				RemoveAllNodes ();
				foreach (Node n in value) {
					Add (n);
				}
			}
		}

		public Link[] Links {
			get {
				ArrayList links = new ArrayList ();
				foreach (IdentifiedElement e in children) {
					Link l = e as Link;
					if (l != null) {
						links.Add (l);
					}
				}
				return (Link[]) links.ToArray (typeof (Link));
			}
			set {
				RemoveAllLinks ();
				foreach (Link l in value) {
					Add (l);
				}
			}
		}

		private void RemoveAllNodes () {
			foreach (IdentifiedElement e in children) {
				Node n = e as Node;
				if (n != null) {
					children.Remove (n);
				}
			}
		}

		private void RemoveAllLinks () {
			foreach (IdentifiedElement e in children) {
				Link l = e as Link;
				if (l != null) {
					children.Remove (l);
				}
			}
		}

		public void Connect () {
			foreach (IdentifiedElement e in children) {
				Link l = e as Link;
				if (l != null) {	
					Node origin = (Node) GetChild (l.OriginID);
					Logger.Assert (origin != null, "origin != null");
					l.Origin = origin;
					Node destination = (Node) GetChild (l.DestinationID);
					Logger.Assert (destination != null, "destination != null");
					l.Destination = destination;
				}
			}
		}

		
		public override void Add (IdentifiedElement child) {
			Logger.Assert (child != null, "child != null");
			if (!children.Contains(child)) {
				children.Add (child);
			}
		}

		public override void Remove (IdentifiedElement child) {
			children.Remove (child);
		}

		public override IdentifiedElement[] GetChildrenAt (Point p) {
			ArrayList elements = new ArrayList ();
			foreach (IdentifiedElement e in children) {
				if (e.Intersects (p)) {
					elements.Add (e);
				}
			}
			return (IdentifiedElement[]) 
				elements.ToArray (typeof (IdentifiedElement));
		}

		public override IdentifiedElement GetChild (string id) {
			foreach (IdentifiedElement e in children) {
				if (e.ID == id) {
					return e;
				}
			}
			return null;
		}

		// geometric methods
		public override bool Intersects (Point p) {
			return false;
		}

		public override void MoveTo (Point p) {
		}		
	}
}
