
using System;
using System.Threading;


namespace Minesweeper
{
	class MainClass
	{
		public static Random ran = new Random();
		public static Tile[,] tiles;
		public const int size = 20;
		public const int bombCount = 25;

		public static int p1PosX = 0;
		public static int p1PosY = 0;

		public static ConsoleColor defaultCol = ConsoleColor.Gray;

		public static void Main (string[] args)
		{
			bool Running = true;
			Console.CursorVisible = false;
				populateGame (size, bombCount);
				drawGrid (size);

				
			while(Running)
			{
				bool Update = false;
				ConsoleKey currentKey = System.Console.ReadKey (true).Key;
				if (currentKey == ConsoleKey.RightArrow) {
					if (p1PosX < size - 1)
						p1PosX++;
				}
				if (currentKey == ConsoleKey.LeftArrow) {
						if (p1PosX > 0)
							p1PosX--;
					}
					
				if (currentKey == ConsoleKey.DownArrow) {
					if (p1PosY < size - 1)
						p1PosY++;
				}
				if (currentKey == ConsoleKey.UpArrow) {
					if (p1PosY > 0)
						p1PosY--;
				}


					Update = true;


				if (Update)
					drawGrid (size);
				
				if (currentKey == ConsoleKey.Spacebar) {
					if (tiles [p1PosX, p1PosY].bomb) {  						
						Running = false;
						Console.CursorLeft = size;
						Console.CursorTop = size / 2;
						Console.CursorTop = Console.CursorTop - 1;
						Console.Write ("╔═════════════════════╖");
						Console.CursorTop = size / 2;
						Console.CursorLeft = size;
						Console.Write ("║GAME OVER! YOU SUCK. ║");
						Console.CursorTop = Console.CursorTop + 1;
						Console.CursorLeft = size;
						Console.Write ("╚═════════════════════╝");

					}

				}


				Thread.Sleep (1);
			}

				Console.ReadKey ();

			
		}

		public static void populateGame (int gridSize, int bombCount)
		{
			tiles = new Tile[gridSize, gridSize];
			for (int y = 0; y < gridSize; y++)
				for (int x = 0; x < gridSize; x++) {
					tiles [x, y] = new Tile (x, y, 0, false);
				}


			//Bomb placement

			int bombAmount = 0;
			while (bombAmount < bombCount) {
				int x = ran.Next (gridSize);
				int y = ran.Next (gridSize);
				if (tiles [x, y].bomb == false) {
					tiles [x, y].bomb = true;
					bombAmount++;				
				}

			}
				
		}

		public static void drawGrid(int gridSize)
		{
			
			for (int y = 0; y < gridSize; y++) {
				Console.CursorTop = y;
				Console.CursorLeft = 0;
				for (int x = 0; x < gridSize; x++) {
					if (tiles [x, y].bomb == true) {
						if (x == p1PosX && y == p1PosY) {
							Console.ForegroundColor = ConsoleColor.Yellow;
							Console.Write (" * ");
							Console.ForegroundColor = defaultCol;
						} else {
							Console.ForegroundColor = ConsoleColor.Red;
							Console.Write (" * ");
							Console.ForegroundColor = defaultCol;
						}
					} else {
						int value = 0;
						Avail[,] areas = GetAvailableAreas (gridSize, x, y);
						for (int x2 = -1; x2 < 2; x2++) {
							for (int y2 = -1; y2 < 2; y2++) {
								if (areas [x2+1, y2+1] == Avail.InBounds) {
									if (tiles [x + x2, y + y2].bomb) {
										value++;
									}
								}
							}
						}			

						tiles [x, y].value = value;

						if (x == p1PosX && y == p1PosY) {
							Console.ForegroundColor = ConsoleColor.Yellow;
							Console.Write (" {0} ", tiles [x, y].value);
							Console.ForegroundColor = defaultCol;
						}
						else
							Console.Write (" {0} ", tiles [x, y].value);

								
					}

				}

			}
			Console.CursorLeft = 0;
			Console.CursorTop = 0;

		}
		public enum Avail
		{
			OutOfBounds = 0,
			InBounds = 1
		}

		public static Avail[,] GetAvailableAreas(int size, int x, int y)
		{
			Avail[,] result = new Avail[3, 3];
			for (int ax = 0; ax < 3; ax++)
				for (int ay = 0; ay < 3; ay++)
					result [ax, ay] = Avail.InBounds;

			if ((x - 1) < 0) {
				for (int i = 0; i < 3; i++)
					result [0, i] = Avail.OutOfBounds;
			}
			if ((x + 1) > (size - 1)) {
				for (int i = 0; i < 3; i++)
					result [2, i] = Avail.OutOfBounds;
			}
			if ((y - 1) < 0) {
				for (int i = 0; i < 3; i++)
					result [i, 0] = Avail.OutOfBounds;
			}	
			if ((y + 1) > (size - 1)) {
				for (int i = 0; i < 3; i++)
					result [i, 2] = Avail.OutOfBounds;
			}	
			return result;
		}
	}
}
