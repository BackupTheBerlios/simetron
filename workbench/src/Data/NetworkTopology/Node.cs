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
	using Simetron.Data;

	public enum NodeType {
		CENTRIOD,
		EXTERNAL,
		INTERSECTION,
		ORIGIN,
		DESTINATION
	}

	public class Node : INamed
	{
		int id;
		string name;
		NodeType type;
		Hashtable upLinks;
		Hashtable downLinks;

		public Node (int id, NodeType type, string name)
		{
			this.id = id;
			this.type = type;
			this.name = name;
			upLinks = new Hashtable ();
			downLinks = new Hashtable ();
		}

		public int ID {
			get { return id; }
		}

		public string Name {
			get { return name; }
			set { name = value; }
		}

		public NodeType Type {
			get { return type; }
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
