using System;
using System.Text;
using System.Xml.Serialization;
using Simetron.Logging;

namespace Simetron.Data.NetworkTopology {
	public class Point {
		private double x;
		private double y;

		public static double Threshold = 1E6;

		public Point () {
		}

		public Point (double x, double y) {
			this.x = x;
			this.y = y;
		}

		public Point (Point p) {
			this.x = p.X;
			this.y = p.Y;
		}

		[XmlAttribute]
		public double X {
			get {
				return x;
			}
			set {
				x = value;
			}
		}

		[XmlAttribute]
		public double Y {
			get {
				return y;
			}
			set {
				y = value;
			}
		}

		public double DistanceTo (Point p) {
			Logger.Assert (p != null);
			double dx = p.X - x;
			double dy = p.Y - y;
			return Math.Sqrt (dx*dx + dy*dy);
		}

		public bool Intersects (Point p) {
			return DistanceTo (p) <= Point.Threshold;
		}
		
		public void MoveTo (Point p) {
			x = p.X;
			y = p.Y;
		}

		public override bool Equals (object obj) {
			Point other = obj as Point;
			if (other == null) {
				return false;
			} 
			return (this.X == other.X && this.Y == other.Y);
		}

		public override int GetHashCode () {
			return x.GetHashCode() ^ y.GetHashCode();
		}

		public override string ToString () {
			StringBuilder message = new StringBuilder ();
			message.AppendFormat ("(x,y)=({0},{1})", x, y);
			return message.ToString();
		}
	}
}
