//
// Lane.cs  - A representation of a topological lane
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

	public enum LaneRule {
		CANNOT_CHANGE,
		CHANGE_RIGHT,
		CHANGE_LEFT
	}

	public class Lane : Identified
	{		
		public const float WIDTH = 3.5f;
		int id;
		float width = WIDTH;
		LaneRule rule;
		ArrayList upLanes;
		int[] upLaneIDs;
		ArrayList downLanes;
		Segment parent;

		public Lane ()
		{
			upLanes = new ArrayList ();
			downLanes = new ArrayList ();
		}

		[XmlAttribute]
		public int ID {
			get { return id; }
			set { id = value; }
		}

		public float Width {
			get { return width; }
			set { width = value; }
		}

		public LaneRule Rule {
			get { return rule; }
			set { rule = value; }
		}

		public int[] UpLaneIDs {
			get {
				if (upLanes.Count > 0) {
					int[] r = new int [upLanes.Count];
					int i = 0;
					foreach (Lane l in upLanes) {
						r[i++] = l.ID;
					}
					return r;
				} else {
					return upLaneIDs;
				}
			}
			set {
				upLaneIDs = value;
			}
		}

		[XmlIgnoreAttribute]
		public Segment Parent {
			get { return parent; }
			set { parent = value; }
		}

		public void AddUpLane (Lane a)
		{
			if (a != null && !upLanes.Contains (a)) {
				upLanes.Add (a);
				a.downLanes.Add (this);
			}
		}
	}
}
