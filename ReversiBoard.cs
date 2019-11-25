using System;
using System.Collections.Generic;

namespace C_Reversi
{
    public class ReversiBoard {
		protected char[,] board;
		protected char playerOnePiece;
		protected char playerTwoPiece;
		protected char emptyPiece;

		// CONSTRUCTORS
		public ReversiBoard() 
		{
			board = new char[8, 8];
			playerOnePiece = 'B';
			playerTwoPiece = 'W';
			emptyPiece = '_';
			// for rectangular arrays
			// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/arrays/multidimensional-arrays
			for (int row = 0; row < board.GetLength(0); row++) 
			{
				for (int col = 0; col < board.GetLength(1); col++) 
				{
					put(emptyPiece, new Point(row, col));
				}
			}
			put(playerOnePiece, new Point(3, 3));
			put(playerTwoPiece, new Point(3, 4));
			put(playerTwoPiece, new Point(4, 3));
			put(playerOnePiece, new Point(4, 4));
		}

		// PRINTING
		public void printBoard()
		{
			for (int row = 0; row < board.GetLength(0); row++) 
			{
				for (int col = 0; col < board.GetLength(1); col++) 
				{
					Console.Write($"{board[row, col]} ");
				}
				Console.WriteLine();
			}
		}

		// ACCESSORS
		public char[,] getBoard()
		{
			return board;
		}

		public char at(Point aPoint)
		{
			return board[aPoint.x, aPoint.y];
		}

		static public List<Point> getNeighboringPoints()
		{
			List<Point> points = new List<Point>();
			for (int x = -1; x < 2; x++) 
			{
				for (int y = -1; y < 2; y++) 
				{
					Point point = new Point(x, y);
					points.Add(point);
				}
			}
			points.RemoveAll(point => point.ToString() == "0@0");
			return points;
		}

		public Boolean hasAlliesFromPointInDirection(char piece, Point origin, Point direction)
		{
			Point current = origin.add(direction);
			while (current.x < board.GetLength(0) && current.y < board.GetLength(1) &&
				current.x >= 0 && current.y >= 0)
			{
				if (at(current) == piece)
				{
					return true;
				}
				current = current.add(direction);
			}
			return false;
		}

		public Point getAllyFromPointWithDirection(char piece, Point origin, Point direction)
		{
			Point current = origin.add(direction);
			while (current.x < board.GetLength(0) && current.y < board.GetLength(1) &&
				current.x >= 0 && current.y >= 0)
			{
				if (at(current) == piece)
				{
					return current;
				}
				current = current.add(direction);
			}
			return current; // should never reach this line
		}

		public List<Point> getSupportingAllies(char piece, Point origin)
		{
			List<Point> neighboringPoints = ReversiBoard.getNeighboringPoints();
			List<Point> supportingAllies = new List<Point>();
			foreach (Point neighbor in neighboringPoints)
			{
				if (hasAlliesFromPointInDirection(piece, origin, neighbor))
				{
					Point ally = getAllyFromPointWithDirection(piece, origin, neighbor);
					supportingAllies.Add(ally);
				}
			}
			return supportingAllies;
		}

		public List<Point> getPiecesInRange(Point origin, Point destination)
		{
			List<Point> pieces = new List<Point>();
			Point direction = origin.directionTo(destination);
			Point current = origin;
			while (!current.equals(destination)) 
			{
				current = current.add(direction);
				pieces.Add(current);
			}
			return pieces;
		}

		public int getNumberOfCaptures(char attacker, char defender, Point origin)
		{
			List<Point> supportingAllies = getSupportingAllies(attacker, origin);
			int captures = 0;
			foreach (Point ally in supportingAllies) 
			{
				List<Point> pieces = getPiecesInRange(origin, ally);
				if (isUnbroken(attacker, defender, pieces)) 
				{
					foreach (Point piece in pieces) 
					{
						if (at(piece) == defender) 
						{
							captures++;
						}
					}
				}
			}
			return captures;
		}

		public List<Point> getLiveCells(char attacker, char defender) 
		{
			List<Point> liveCells = new List<Point>();
			for (int row = 0; row < board.GetLength(0); row++) 
			{
				for (int col = 0; col < board.GetLength(1); col++) 
				{
					Point point = new Point(row, col);
					if (canPlaceAt(attacker, defender, point)) 
					{
							liveCells.Add(point);
					}
				}
			}
			return liveCells;
		}

		// TESTING
		public Boolean isOccupiedAt(Point aPoint)
		{
			return at(aPoint) != emptyPiece;
		}

		public Boolean isUnbroken(char attacker, char defender, List<Point> pieces) 
		{
			foreach (Point piece in pieces) 
			{
				if (at(piece) != attacker && at(piece) != defender) 
				{
					return false;
				}
			}
			return true;
		}

		public Boolean hasAllySupportingAt(char ally, Point aPoint)
		{
			return getSupportingAllies(ally, aPoint).Count > 0;
		}

		public Boolean hasNeighboringEnemyAt(char enemy, Point origin) 
		{
			List<Point> neighboringPoints = ReversiBoard.getNeighboringPoints();
			foreach (Point neighbor in neighboringPoints) 
			{
				Point point = origin.add(neighbor);
				if (point.x < board.GetLength(0) && point.y < board.GetLength(1) &&
					point.x >= 0 && point.y >= 0 &&
					at(point) == enemy) 
				{
					return true;
				}
			}
			return false;
		}

		public Boolean canPlaceAt(char attacker, char defender, Point point) 
		{
			return !isOccupiedAt(point) && 
				hasAllySupportingAt(attacker, point) && 
				hasNeighboringEnemyAt(defender, point) && 
				getNumberOfCaptures(attacker, defender, point) > 0;
		}


		// MUTATORS
		public void put(char piece, Point point) 
		{
			board[point.x, point.y] = piece;
		}

		public void convertInRange(char piece, Point origin, Point destination) 
		{
			Point direction = origin.directionTo(destination);
			Point current = origin;
			while (!current.equals(destination)) 
			{
				put(piece, current);
				current = current.add(direction);
			}
			put(piece, current);
		}

		public void placeAt(char piece, Point point) 
		{
			List<Point> supportingAllies = getSupportingAllies(piece, point);
			foreach (Point ally in supportingAllies) 
			{
				convertInRange(piece, point, ally);
			}
		}
    }
}
