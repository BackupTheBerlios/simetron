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

	public class CatmullRomBSpline : BSpline
	{		
		protected override float Base (int i, float t)
		{
			switch (i) {
			case 0:
				return ((-t+2)*t-1)*t/2;
			case 1:
				return (((3*t-5)*t)*t+2)/2;
			case 2:
				return ((-3*t+4)*t+1)*t/2;
			case 3:
				return ((t-1)*t*t)/2;
			}
			return 0;
		}
	}
}
