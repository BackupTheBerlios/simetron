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
	using System.Collections;
	using Simetron.Data;

	public enum LinkType {
		FREEWAY,
		RAMP,
		URBAN
	}

	public class Link : INamed
	{
		int id;
		string name;
		Node upNode;
		Node downNode;
		LinkType type;
		ArrayList segments;

		public Link (int id, LinkType type, 
			     Node upNode, Node downNode,
			     string name)
		{
			this.id = id;
			this.type = type;
			this.upNode = upNode;
			this.downNode = downNode;
			this.name = name;
			segments = new ArrayList ();
		}
		
		public int ID {
			get { return id; }
		}

		public string Name {
			get { return name; }
			set { name = value; }
		}

		public LinkType Type {
			get { return type; }
		}

		public Node UpNode {
			get { return upNode; }
		}

		public Node DownNode {
			get { return downNode; }
		}

		public IEnumerable Segments {
			get { return segments; }
		}

		public void AddSegment (Segment segment)
		{
			if (segment != null && !segments.Contains (segment)) {
				segments.Add (segment);
			}
		}
	}
}
