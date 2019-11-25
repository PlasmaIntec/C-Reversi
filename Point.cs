using System;

namespace C_Reversi
{
	public class Point 
	{
		public int x
		{
			get;
			set;
		}
		public int y
		{
			get;
			set;
		}

		// https://docs.microsoft.com/en-us/dotnet/api/system.object.tostring?view=netframework-4.8
		public override string ToString() 
		{
			return $"{x}@{y}";
		}

		// https://coding.abel.nu/2014/09/net-and-equals/
		public Boolean equals(Point aPoint)
		{
			return x == aPoint.x && y == aPoint.y;
		}

		// UTILTY
		public Point add(Point aPoint)
		{
			int newX = x + aPoint.x;
			int newY = y + aPoint.y;
			return new Point(newX, newY);
		}

		public Point directionTo(Point aPoint)
		{
			int dx = aPoint.x - x;
			int dy = aPoint.y - y;
			if (dx != 0) 
			{
				dx = dx / Math.Abs(dx);
			}
			if (dy != 0)
			{
				dy = dy / Math.Abs(dy);
			}
			return new Point(dx, dy);
		}

		// CONSTRUCTORS
		public Point(int x, int y) 
		{
			this.x = x;
			this.y = y;
		}

		public Point(string s)
		{
			string[] parts = s.Split('@');
			this.x = int.Parse(parts[0]);
			this.y = int.Parse(parts[1]);
		}
	}
}