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
	using System.Xml.Serialization;
	using Simetron.Data;
	using Simetron.Data.Geometry;
	
	public class Segment : Identified
	{
		int id;
		int defaultSpeedLimit;
		double freeSpeed;
		double grade;
		Segment upSegment;
		int upSegmentID = -1;
		Segment downSegment;
		Link parent;
		ArrayList lanes;
		int[] laneIDs;
		// the geometric object
		Point ctlPoint;
		Point upPoint;
		Point downPoint;

		public Segment ()
		{
			lanes = new ArrayList ();
		}

		[XmlAttribute]
		public int ID {
			get { return id; }
			set { id = value; }
		}

		[XmlIgnoreAttribute]
		public Link Parent {
			get { return parent; }
			set { parent = value;}
		}				

		[XmlIgnoreAttribute]
		public Segment UpSegment {
			get { return upSegment; }
			set { 
				upSegment = value; 
				if (value != null) {
					value.downSegment = this;
					upSegmentID = upSegment.ID;
				}
			}
		}

		// enables Xml serialization
		public int UpSegmentID {
			get { 
				return upSegmentID;
			}
			set {
				upSegmentID = value;
			}
		}

		[XmlIgnoreAttribute]
		public Segment DownSegment {
			get { return downSegment; }
		}

		// enables Xml serialization
		public int[] LaneIDs {
			get {
				if (lanes.Count > 0) {
					int[] r = new int[lanes.Count];
					int index = 0;
					foreach (Lane l in lanes) {
						r[index++] = l.ID;
					}
					return r;
				} else {
					return laneIDs;
				}
			}
			set {
				laneIDs = value;       
			}
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

		public Point ControlPoint {
			get { return ctlPoint; }
			set { ctlPoint = value; }
		}

		public Point DownPoint {
			get { return downPoint; }
			set { downPoint = value; }
		}

		public Point UpPoint {
			get { return upPoint; }
			set { upPoint = value; }
		}

		public void AddLane (Lane lane)
		{
			if (lane != null && !lanes.Contains (lane)) {
				lanes.Add (lane);
				lane.Parent = this;
			}
		}
	}
}
