//
// BSpline.cs  - A beta cublic natural spline
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

	public class BSpline
	{
		Point[] points; // P0, P1, P2, P3

		public BSpline ()
		{
			points = new Point [4];
		}
		
		public Point[] ControlPoints {
			get { return points; }
		}

		// returns numSteps points in the segment P1-P2
		public Point[] Evaluate (int numSteps)
		{
			Point[] polygon = new Point [numSteps];
			for (int j=0; j <= numSteps; j++) {
				polygon[j] = eval ((float)j/(float)numSteps);
			}
			return polygon;				
		}

		private Point eval (float t)
		{
			// may throw null ref. exception if points are null
			float x = 0f;
			float y = 0f;
			for (int j = 0; j <= 3; j++) {
				float b = Base (j,t);
				x += b*points[j].X;
				y += b*points[j].Y;
			}
			return new Point(x,y);
		}

		protected virtual float Base (int i, float t)
		{
			switch (i) {
			case 0:
				return (((-t+3)*t-3)*t+1)/6;
			case 1:
				return (((3*t-6)*t)*t+4)/6;
			case 2:
				return (((-3*t+3)*t+3)*t+1)/6;
			case 3:
				return (t*t*t)/6;
			}
			return 0; 
		}
	}
}

