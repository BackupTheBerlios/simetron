//
// Arc.cs  - A geometric arc
//
// Author:
//   Bruno Fernandez-Ruiz (brunofr@olympum.com)
//
// Copyright (c) 2003 The Olympum Group,  http://www.olympum.com
// All Rights Reserved
//

namespace Simetron.Data.Geometry
{
	using System;

	public class Arc
	{
		Point upPoint;
		Point downPoint;
		Point ctlPoint;
		
		public Arc (Point upPoint, Point downPoint, Point ctlPoint)
		{
			if (upPoint == downPoint) {
				throw new ArgumentException (
					"Cannot create zero length arc");
			}
			this.upPoint = upPoint;
			this.downPoint = downPoint;
			this.ctlPoint = ctlPoint;
		}

		public Point UpPoint {
			get { return upPoint; }
		}

		public Point DownPoint {
			get { return downPoint; }
		}

		public Point ControlPoint {
			get { return ctlPoint; }
		}
	}
}
