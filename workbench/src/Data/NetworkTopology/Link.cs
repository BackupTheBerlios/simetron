//
// Link.cs  - A representation of a topological link
//
// Author:
//   Bruno Fernandez-Ruiz (brunofr@olympum.com)
//
// Copyright (c) 2003 The Olympum Group,  http://www.olympum.com
// All Rights Reserved
//

namespace Simetron.Data.NetworkTopology 
{
	using System;
	using System.Xml.Serialization;
	using System.Collections;
	using Simetron.Data;

	public enum LinkType {
		FREEWAY,
		RAMP,
		URBAN
	}

	public class Link : INamed, Identified
	{
		int id;
		string name;
		Node upNode;
		int upNodeID;
		int downNodeID;
		Node downNode;
		LinkType type;
		ArrayList segments;
		int[] segmentIDs;

		public Link ()
		{
			segments = new ArrayList ();
		}
		
		[XmlAttribute]
		public int ID {
			get { return id; }
			set { id = value; }
		}

		public string Name {
			get { return name; }
			set { name = value; }
		}

		public LinkType Type {
			get { return type; }
			set { type = value; }
		}

		[XmlIgnoreAttribute]
		public Node UpNode {
			get { 
				return upNode; 
			}
			set {
				upNode = value;
				upNode.AddDownLink (this);
			}
		}

		// enables Xml serialization
		public int UpNodeID {
			get { 
				if (upNode != null) {
					return upNode.ID;
				} else {
					return upNodeID;
				}
			}
			set { 
				upNodeID = value;
			}	
		}

		[XmlIgnoreAttribute]
		public Node DownNode {
			get { 
				return downNode; 
			}
			set {
				downNode = value;
				downNode.AddUpLink (this);
			}
		}

		// enables Xml serialization
		public int DownNodeID {
			get {
 				if (downNode != null) {
					return downNode.ID;
				} else {
					return downNodeID; 
				}
			}
			set { 
				downNodeID = value; 
			}
		}


		// enables Xml serialization
		public int[] SegmentIDs {
			get {
				if (segments.Count > 0) {
					int[] r = new int[segments.Count];
					int index = 0;
					foreach (Segment s in segments) {
						r[index++] = s.ID;
					}
					return r;
				} else {
					return segmentIDs;
				}
			}
			set {
				segmentIDs = value;       
			}
		}

		public void AddSegment (Segment segment)
		{
			if (segment != null && !segments.Contains (segment)) {
				segments.Add (segment);
				segment.Parent = this;
			}
		}
	}
}
