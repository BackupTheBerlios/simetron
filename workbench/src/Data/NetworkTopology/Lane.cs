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

	public enum LaneRule {
		CANNOT_CHANGE,
		CHANGE_RIGHT,
		CHANGE_LEFT
	}

	public class Lane : Identified
	{		
		int id;
		const float DEFAULT_WIDTH = 3.66;
		float width;
		LaneRule rule;
		ArrayList upLanes;
		ArrayList downLanes;
		Segment parent;

		public Lane (int id, LaneRule rule, Segment parent)
		{
			this.id = id;
			this.rule = rule;
			this.parent = parent;
			width = DEFAULT_WIDTH;
			upLanes = new ArrayList ();
			downLanes = new ArrayList ();
			parent.AddLane (this);
		}

		public int ID {
			get { return id; }
		}

		public float Width {
			get { return width; }
			set { width = value; }
		}

		public LaneRule Rule {
			get { return rule; }
		}

		public IEnumerable UpLanes {
			get { return upLanes; }
		}

		public IEnumerable DownLanes {
			get { return downLanes; }
		}

		public Segment Parent {
			get { return parent; }
		}

		public void AddUpLane (Lane a)
		{
			if (a != null && !upLanes.Contains (a)) {
				upLanes.Add (a);
			}
		}

		public void AddDownLane (Lane a)
		{
			if (a != null && !downLanes.Contains (a)) {
				downLanes.Add (a);
			}
		}
	}
}
