//
// Node.cs  - A representation of a topological node
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
	using System.Xml.Serialization;
	using Simetron.Data;

	public enum NodeType {
		CENTRIOD,
		EXTERNAL,
		INTERSECTION,
		ORIGIN,
		DESTINATION
	}

	public class Node : INamed, Identified
	{
		int id;
		string name;
		NodeType type;
		Hashtable upLinks;
		Hashtable downLinks;

		public Node ()
		{
			upLinks = new Hashtable ();
			downLinks = new Hashtable ();
		}

		public Node (int id, NodeType type, string name) : this ()
		{
			this.id = id;
			this.type = type;
			this.name = name;
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

		public NodeType Type {
			get { return type; }
			set { type = value; }
		}

		public void AddUpLink (Link link) 
		{
			if (link != null && !upLinks.Contains (link.ID)) {
				upLinks[link.ID] = link;
			}			
		}

		public void AddDownLink (Link link)
		{
			if (link != null && !downLinks.Contains (link.ID)) {
				downLinks[link.ID] = link;
			}
		}
	}
}
