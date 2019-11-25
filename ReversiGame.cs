using System;
using System.IO;

namespace C_Reversi
{
	public class ReversiGame
	{
		protected ReversiBoard board { get; set; }
		protected char currentTurn { get; set; }
		protected char enemyTurn { get; set; }

		// INPUT
		public string getMove()
		{
			string move = "";
			try 
			{
				// https://docs.microsoft.com/en-us/dotnet/api/system.console.readline?view=netframework-4.8#System_Console_ReadLine
				move = Console.ReadLine();
			} 
			catch (IOException e) 
			{
				Console.WriteLine(e.Message);
			}
			return move; 
		}

		// GAME LOGIC
		public void play()
		{
			string input;
			Point point;
			while (true)
			{
				board.printBoard();
				if (board.getLiveCells(currentTurn, enemyTurn).Count == 0) 
				{
					Console.Write($"{enemyTurn} HAS WON\n");
					return;
				}
				Console.Write($"CURRENT TURN: {currentTurn}\n");
				input = getMove();
				point = new Point(input);
				if (board.canPlaceAt(currentTurn, enemyTurn, point)) 
				{
					board.placeAt(currentTurn, point);
					Console.Write($"SUCCESSFULLY PLACED {currentTurn} at {point}\n");
					swapTurn();
				} 
				else 
				{
					Console.Write($"FAILED TO PLACED {currentTurn} at {point}\n");
				}
			}
		}

		public void swapTurn() 
		{
			char temp = enemyTurn;
			enemyTurn = currentTurn;
			currentTurn = temp;
		}

		// CONSTRUCTOR
		public ReversiGame() 
		{
			board = new ReversiBoard();
			currentTurn = 'B';
			enemyTurn = 'W';
		}
	}
}