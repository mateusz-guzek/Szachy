using System.Text.RegularExpressions;

namespace Szachy
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Game board = new();
			board.PrintOut();
			while (true)
			{
				Console.Write("Make a move: ");
				string input = Console.ReadLine();
				if (board.TryMove(input))
				{
					board.PrintOut();
				}
			}
		}
	}

	struct Pos
	{
		public int X { get; set; }
		public int Y { get; set; }
		
		public Pos(char x, char y)
		{

			this.Y = 8 - (int)(char.GetNumericValue(y));
			x = char.ToLower(x);
			this.X = int.Parse(((int)x - (int)'a').ToString());
		}

		public override string ToString()
		{
			return $"{X} {Y}";
		}

	}


	class Game
	{
		public char[,] Board { get; set; }

		private const string START_FEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

		public Game()
		{
			Board = new char[8, 8];
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					Board[i, j] = ' ';
				}
			}
			FenFill(START_FEN);
		}




		public bool TryMove(string moveNotation)
		{
			// example moveNotation:  'a7 a5' or 'A7 A5'
			// the piece on a7 goes to a5
			// start and end destinations MUST be separated by at least one space
			// figure out how to user another notation or just leave it how it is

			Regex regex = new Regex("[a-g][1-8][ ]+[a-g][1-8]", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			if ( !regex.Match(moveNotation).Success ) { return false; }

			string[] splitted = moveNotation.Split(" ", StringSplitOptions.TrimEntries);
			string startStr = splitted[0];
			string endStr = splitted[1];

			Pos start = new(startStr[0], startStr[1]);
			Pos end = new(endStr[0], endStr[1]);

			char piece = Board[start.X,start.Y];
			Board[start.X, start.Y] = ' ';
			Board[end.X, end.Y] = piece;

            return true;

		}

		private void FenFill(string fenString)
		{
			string[] fields = fenString.Split(' ');

			string positions = fields[0];
			char startingSide = char.Parse(fields[1]); // 'w' or 'b'	// implement later
			string castlingRules = fields[2]; // roszada				// implement later

			// Data[x,y]

			int x = 0;
			int y = 0;
            foreach (char pos in positions)
            {
				if(char.IsNumber(pos))
				{
					x += int.Parse(pos.ToString())-1;
					continue;
				}
				if(pos == '/')
				{
					x = 0;
					y++;
				}
				else
				{
					Board[x,y] = pos;
					x++;
				}
            }
        }

		public void PrintOut()
		{
			ConsoleColor DefaultBackgroundColor = Console.BackgroundColor;
			ConsoleColor DefaultForegroundColor = Console.ForegroundColor;


			Console.Clear();

			Console.Write("   ");                               // horizontal coords
			for (int i = 0; i < 8; i++)
			{
				Console.Write($" {(char)('A' + i)} ");
			}
			Console.WriteLine();                                //

			for (int y = 0; y < 8; y++)                                 // proper board
			{
				// vertical coords
				Console.BackgroundColor = DefaultBackgroundColor;
				Console.Write($" {(8-y)} ");

				for (int x = 0; x < 8; x++)
				{
					if ((x + y) % 2 == 0)
					{
						Console.BackgroundColor = ConsoleColor.DarkCyan;
					} else
					{ 
						Console.BackgroundColor = ConsoleColor.Red;
					}
					if (char.IsUpper(Board[x,y]))
					{
						Console.ForegroundColor = ConsoleColor.White;
					} else
					{
						Console.ForegroundColor = ConsoleColor.Black;
					}
					Console.Write($" {Board[x, y]} ");
					Console.ForegroundColor = ConsoleColor.Gray;
				}

				Console.BackgroundColor = DefaultBackgroundColor;   // vertical coords
				Console.Write($" {(8 - y)} ");						//

				Console.WriteLine();
			}

			Console.Write("   ");								// horizontal coords
			for (int i = 0; i < 8; i++)							
			{													
				Console.Write($" {(char)('B' + i)} ");			
			}													
			Console.WriteLine();								//
			Console.BackgroundColor = DefaultBackgroundColor;
			Console.ForegroundColor = DefaultForegroundColor;

		}
	}
}