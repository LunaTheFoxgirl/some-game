
using System;
using System.Threading;


namespace Minesweeper
{
	public class MainClass
	{
		private static bool generated = false;
		public static Random ran = new Random();
		public static Tile[,] tiles;
		public const int sizex = 10;
		public const int sizey = 20;
		public const int bombCount = 9;

		public static int p1PosX = 0;
		public static int p1PosY = 0;

		public static ConsoleColor defaultCol = ConsoleColor.Gray;

		public static void Main(string[] args)
		{
			bool Running = true;
			Console.CursorVisible = false;
			populateGame(sizex, sizey, -1);
			drawGrid(sizex, sizey);
				
			while (Running)
			{
				bool Update = false;
				ConsoleKey currentKey = System.Console.ReadKey(true).Key;
				if (currentKey == ConsoleKey.RightArrow)
				{
					if (p1PosX < sizex - 1)
						p1PosX++;
				}
				if (currentKey == ConsoleKey.LeftArrow)
				{
					if (p1PosX > 0)
						p1PosX--;
				}
					
				if (currentKey == ConsoleKey.DownArrow)
				{
					if (p1PosY < sizey - 1)
						p1PosY++;
				}
				if (currentKey == ConsoleKey.UpArrow)
				{
					if (p1PosY > 0)
						p1PosY--;
				}

				if (currentKey == ConsoleKey.Spacebar || currentKey == ConsoleKey.Enter)
				{
					if (!generated)
					{
						populateGame(sizex, sizey, bombCount);
						generated = true;
						tiles[p1PosX, p1PosY].hidden = false;
					}
					if (tiles[p1PosX, p1PosY].bomb)
					{  		
						foreach (Tile t in tiles)
							t.hidden = false;
						drawGrid(sizex, sizey);				
						Running = false;
						Console.ForegroundColor = ConsoleColor.Red;
						Console.CursorLeft = (Console.BufferWidth / 2) - (sizex / 2);
						Console.CursorTop = (Console.BufferHeight / 2) - (sizey / 2);
						Console.CursorTop = Console.CursorTop - 1;
						Console.Write("╔═════════════════════╖");
						Console.CursorTop = (Console.BufferHeight / 2) - (sizey / 2);
						Console.CursorLeft = sizex;
						Console.Write("║GAME OVER! YOU SUCK. ║");
						Console.CursorTop = Console.CursorTop + 1;
						Console.CursorLeft = sizex;
						Console.Write("╚═════════════════════╝");
						Console.ForegroundColor = defaultCol;
					}
					else
					{
						if (tiles[p1PosX, p1PosY].value == 0)
						{
							tiles[p1PosX, p1PosY].FloodFillNone(sizex, sizey, p1PosX, p1PosY, ref tiles);
						}
						else
						{
							tiles[p1PosX, p1PosY].hidden = false;
						}
						drawGrid(sizex, sizey);
					}

				}
				if (Running)
					drawGrid(sizex, sizey);
				Thread.Sleep(1);
			}

			Console.ReadKey();

			
		}

		public static void populateGame(int sizex, int sizey, int bombCount)
		{
			if (bombCount > 0)
			{
				tiles = new Tile[sizex, sizey];
				for (int y = 0; y < sizey; y++)
					for (int x = 0; x < sizex; x++)
					{
						tiles[x, y] = new Tile(x, y, 0, false);
					}
				

				//Bomb placement

				int bombAmount = 0;
				while (bombAmount < bombCount)
				{
					int x = ran.Next(sizex);
					int y = ran.Next(sizey);
					if (tiles[x, y].bomb == false && x != p1PosX && y != p1PosY)
					{
						tiles[x, y].value = 666;
						tiles[x, y].bomb = true;
						bombAmount++;				
					}

				}
				tiles[0, 0].AssignValues(sizex, sizey, 0, 0, ref tiles);
			}
			else
			{
				tiles = new Tile[sizex, sizey];
				for (int y = 0; y < sizey; y++)
					for (int x = 0; x < sizex; x++)
					{
						tiles[x, y] = new Tile(x, y, -1, false);
					}
			}
		}

		public static void drawGrid(int sizex, int sizey)
		{
			
			for (int y = 0; y < sizey; y++)
			{
				Console.CursorTop = y;
				Console.CursorLeft = 0;
				for (int x = 0; x < sizex; x++)
				{
					if (tiles[x, y].bomb == true)
					{
						if (!tiles[x, y].hidden)
						{
							if (x == p1PosX && y == p1PosY)
							{
								Console.ForegroundColor = ConsoleColor.Yellow;
								Console.Write(" * ");
								Console.ForegroundColor = defaultCol;
							}
							else
							{
								Console.ForegroundColor = ConsoleColor.Red;
								Console.Write(" * ");
								Console.ForegroundColor = defaultCol;
							}
						}
						else
						{
							if (x == p1PosX && y == p1PosY)
							{
								Console.ForegroundColor = ConsoleColor.Yellow;
								Console.Write(">{0}<", tiles[x, y].GetValueStr());
								Console.ForegroundColor = defaultCol;
							}
							else
								Console.Write(" {0} ", tiles[x, y].GetValueStr());
						}
					}
					else
					{			
						if (x == p1PosX && y == p1PosY)
						{
							Console.ForegroundColor = ConsoleColor.Yellow;
							Console.Write(">{0}<", tiles[x, y].GetValueStr());
							Console.ForegroundColor = defaultCol;
						}
						else
							Console.Write(" {0} ", tiles[x, y].GetValueStr());

								
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

		public static Avail[,] GetAvailableAreas(int sizex, int sizey, int x, int y)
		{
			Avail[,] result = new Avail[3, 3];
			for (int ax = 0; ax < 3; ax++)
				for (int ay = 0; ay < 3; ay++)
					result[ax, ay] = Avail.InBounds;

			if ((x - 1) < 0)
			{
				for (int i = 0; i < 3; i++)
					result[0, i] = Avail.OutOfBounds;
			}
			if ((x + 1) > (sizex - 1))
			{
				for (int i = 0; i < 3; i++)
					result[2, i] = Avail.OutOfBounds;
			}
			if ((y - 1) < 0)
			{
				for (int i = 0; i < 3; i++)
					result[i, 0] = Avail.OutOfBounds;
			}	
			if ((y + 1) > (sizey - 1))
			{
				for (int i = 0; i < 3; i++)
					result[i, 2] = Avail.OutOfBounds;
			}	
			return result;
		}
	}
}
