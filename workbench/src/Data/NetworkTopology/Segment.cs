//
// Segment.cs  - A representation of a topological segment
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
	using Simetron.Data.Geometry;
	
	public class Segment : Arc, Identified
	{
		int id;
		int defaultSpeedLimit;
		double freeSpeed;
		double grade;
		Segment upSegment;
		Segment downSegment;
		Link parent;
		ArrayList lanes;
		
		public Segment (int id, Point upPoint, 	Point downPoint, 
				Point ctlPoint, Link parent) 
			: base (upPoint, downPoint, ctlPoint)
		{
			this.id = id;
			this.parent = parent;
			lanes = new ArrayList ();
			parent.AddSegment (this);
		}

		public int ID {
			get { return id; }
		}

		public Link Parent {
			get { return parent; }
			set { parent = value; }
		}				

		public Segment UpSegment {
			get { return upSegment; }
			set { upSegment = value; }
		}

		public Segment DownSegment {
			get { return downSegment; }
			set { downSegment = value; }
		}

		public IEnumerable Lanes {
			get { return lanes; }
		}

		public int DefaultSpeedLimit {
			get { return defaultSpeedLimit; }
			set { defaultSpeedLimit = value; }
		}

		public double FreeSpeed {
			get { return freeSpeed; }
			set { freeSpeed = value; }
		}

		public double Grade {
			get { return grade; }
			set { grade = value; }
		}

		public void AddLane (Lane lane)
		{
			if (lane != null && !lanes.Contains (lane)) {
				lanes.Add (lane);
			}
		}
	}
}
