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
	using System.Xml.Serialization;
	using Simetron.Data;

	public class Network : INamed
	{
		Hashtable nodes;
		Hashtable links;
		Hashtable segments;
		Hashtable lanes;
		string name;

		public Network ()
		{
			nodes = new Hashtable ();
			links = new Hashtable ();
			segments = new Hashtable ();
			lanes = new Hashtable ();
		}

		public string Name {
			get { return name; }
			set { name = value; }
		}

		// xml serialization property
		public Node[] Nodes {
			get { 
				ICollection values = nodes.Values;
				Node[] array = new Node [values.Count];
				values.CopyTo (array, 0);
				return array;
			}
			set {
				foreach (Node n in value) {
					AddNode (n);
				}
			}
		}

		// xml serialization property
		public Lane[] Lanes {
			get {
				ICollection values = lanes.Values;
				Lane[] array = new Lane [values.Count];
				values.CopyTo (array, 0);
				return array;
			}
			set {
				foreach (Lane l in value) {
					AddLane (l);
				}
				foreach (Lane l in value) {
					int[] upLaneIDs = l.UpLaneIDs;
					if (upLaneIDs == null) {
						continue;
					}
					foreach (int id in upLaneIDs) {
						Lane upLane = GetLane (id);
						l.AddUpLane (upLane);
					}
				}
			}
		}

		// xml serialization property
		public Segment[] Segments {
			get {
				ICollection values = segments.Values;
				Segment[] array = new Segment [values.Count];
				values.CopyTo (array, 0);
				return array;
			}
			set {
				foreach (Segment s in value) {
					AddSegment (s);
					int[] laneIDs = s.LaneIDs;
					if (laneIDs == null) {
						continue;
					}
					foreach (int id in laneIDs) {
						Lane l = GetLane (id);
						s.AddLane (l);
					}
				}
				foreach (Segment s in value) {
					int id = s.UpSegmentID;
					if (id == -1) {
						continue;
					}
					Segment upSegment = GetSegment (id);
					s.UpSegment = upSegment;
				}
			}
		}

		// xml serialization property
		public Link[] Links {
			get { 
				ICollection values = links.Values;
				Link[] array = new Link [values.Count];
				values.CopyTo (array, 0);
				return array;
			}
			set {
				foreach (Link l in value) {
					AddLink (l);
					l.UpNode = GetNode (l.UpNodeID);
					l.DownNode = GetNode (l.DownNodeID);
					int[] segmentIDs = l.SegmentIDs;
					if (segmentIDs == null) {
						continue;
					}
					foreach (int id in segmentIDs) {
						Segment s = GetSegment (id);
						l.AddSegment (s);
					}
				}
			}
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

		public void AddSegment (Segment segment)
		{
			if (segment != null && !segments.Contains (segment.ID)) {
				segments[segment.ID] = segment;
			}
		}

		public void AddLane (Lane lane)
		{
			if (lane != null && !lanes.Contains (lane.ID)) {
				lanes[lane.ID] = lane;
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

		public Segment GetSegment (int id)
		{
			return (Segment) segments[id];
		}

		public Lane GetLane (int id)
		{
			return (Lane) lanes[id];
		}
	}
}
