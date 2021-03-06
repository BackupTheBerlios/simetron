//
// Point.cs  - A geometric point of float precision
//
// Author:
//   Bruno Fernandez-Ruiz (brunofr@olympum.com)
//
// Copyright (c) 2003 The Olympum Group,  http://www.olympum.com
// All Rights Reserved
//

namespace Simetron.Data.Geometry
{
	using System.Xml.Serialization;

	// this class is similar to the struct System.Drawing.PointF
	// structs are not optimized of being used in collections
	// hence the reasoning for the class
	public class Point
	{
		float x; // the x-coordinate
		float y; // the y-coordinate
		
		public Point ()
		{
			// allows XmlSerialization
		}

		public Point (float x, float y)
		{
			this.x = x;
			this.y = y;
		}

		public static Point operator + (Point point, Point size)
		{
			return new Point (point.X + size.X, point.Y + size.Y);
		}
		
		public static Point operator - (Point point, Point size)
		{
			return new Point (point.X - size.X, point.Y - size.Y);
		}

		public static bool operator == (Point point, Point other)
		{
			return ((point.X == other.X) && (point.Y == other.Y));
		}
		
		public static bool operator != (Point point, Point other)
		{
			return ((point.X != other.X) || (point.Y != other.Y));
		}
			       				
		[XmlAttribute]
		public float X {
			get { return x;	}
			set { x = value; }
		}
				
		[XmlAttribute]
		public float Y {
			get { return y; }
			set { y = value; }
		}

		public override bool Equals (object other)
		{
			if (!(other is Point)) {
				return false;
			}

			return (this == (Point) other);
		}

		
		public override int GetHashCode ()
		{
			return (int) x ^ (int) y;
		}

		
		public override string ToString ()
		{
			return string.Format ("({0},{1})", x, y);
		}
	}
}
