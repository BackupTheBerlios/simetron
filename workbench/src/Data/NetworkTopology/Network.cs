//
// Network.cs  - A representation of a topological network
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

	public class Network : INamed
	{
		Hashtable nodes;
		Hashtable links;
		string name;

		public Network (string name)
		{
			this.name = name;
			nodes = new Hashtable ();
			links = new Hashtable ();
		}

		public string Name {
			get { return name; }
			set { name = value; }
		}

		public ICollection Nodes {
			get { return nodes.Values; }
		}

		public ICollection Links {
			get { return links.Values; }
		}
		
		public void AddNode (Node node)
		{
			if (node != null && !nodes.Contains (node.ID)) {
				nodes[node.ID] = node;
			}
		}

		public void AddLink (Link link)
		{
			if (link != null && !links.Contains (link.ID)) {
				links[link.ID] = link;
			}
		}

		public Node GetNode (int id)
		{
			return (Node) nodes[id];
		}

		public Link GetLink (int id)
		{
			return (Link) links[id];
		}
	}
}
