using System;
using System.Collections;
using System.Text;
using System.Xml.Serialization;
using Simetron.Logging;

namespace Simetron.Data.NetworkTopology {
	public class Node : IdentifiedElement {
		private Point p;
		private ArrayList inLinks;
		private ArrayList outLinks;

		#region Constructors
		public Node () {
		}

		public Node (Point p) : base () {
			inLinks = new ArrayList ();
			outLinks = new ArrayList ();
			this.p = p;
		}
		#endregion


		#region Properties
		public Point Point {
			get {
				return p;
			}
			set {
				p = value;
			}
		}

		[XmlIgnoreAttribute]
		public Link[] InLinks {
			get {
				return (Link[]) inLinks.ToArray (typeof (Link));
			}
			set {
				Logger.Assert (value != null);
				inLinks = new ArrayList (value);
			}
		}

		[XmlIgnoreAttribute]
		public Link[] OutLinks {
			get {
				return (Link[]) outLinks.ToArray (typeof (Link));
			}
			set {
				Logger.Assert (value != null);
				outLinks = new ArrayList (value);
			}
		}

		[XmlIgnoreAttribute]
		public override IdentifiedElement[] Children {
			get {
				return null;
			}
		}
		#endregion

		#region Methods
		private void AddInLink (Link l) {
			Logger.Assert (l != null);
			Logger.Assert (!(inLinks.Contains (l)));
			inLinks.Add (l);
		}

		private void AddOutLink (Link l) {
			Logger.Assert (l != null);
			Logger.Assert (!outLinks.Contains(l));
			outLinks.Add (l);
		}

		private void RemoveInLink (Link l) {
			inLinks.Remove (l);
		}

		private void RemoveOutLink (Link l) {
			outLinks.Remove (l);
		}
		
		private void ThrowNotInNetworkException (Link l) {
			StringBuilder message = new StringBuilder ();
			message.AppendFormat ("link {0} does not contain node {1}", l, this);
			throw new ArgumentException (message.ToString ());
		}

		public void AddLink (Link l) {
			if (l.Origin == this) {
				AddOutLink (l);
			} else if (l.Destination == this) {
				AddInLink (l);
			} else {
				ThrowNotInNetworkException (l);
			}			
		}

		public void RemoveLink (Link l) {
			if (l.Origin == this) {
				RemoveOutLink (l);
			} else if (l.Destination == this) {
				RemoveInLink (l);
			} else {
				ThrowNotInNetworkException (l);
			}			
		}

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

		public override bool Intersects (Point p) {
			// TODO
			return this.Point.Intersects (p);
		}
		
		public override void MoveTo (Point p) {
			// TODO
			this.Point.MoveTo (p);
		}

		#endregion
	}

}
