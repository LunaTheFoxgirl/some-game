using System;

namespace Minesweeper
{
	public class Tile
	{
		public int x, y;
		public int value;
		public bool bomb;
		public bool hidden;
		public bool flagged;
		public bool initialized = false;

		public Tile(int x, int y, int value, bool bomb)
		{
			this.hidden = true;
			this.x = x;
			this.y = y;
			this.value = value;
			this.bomb = bomb;
		}

		public string GetValueStr()
		{
			if (hidden)
				return "◾";
			else if (hidden)
				return "?";
			else if (value == 0)
				return "◽";
			else
				return value.ToString();
		}

		public void AssignValues(int sizex, int sizey, int x, int y, ref Tile[,] tiles)
		{
			MainClass.Avail[,] areas = MainClass.GetAvailableAreas(sizex, sizey, x, y);

			for (int x2 = -1; x2 < 2; x2++)
			{
				for (int y2 = -1; y2 < 2; y2++)
				{
					if (areas[x2 + 1, y2 + 1] == MainClass.Avail.InBounds)
					{
						if (tiles[x + x2, y + y2].bomb)
						{
							this.value++;
						} 
					}
				}
			}
			for (int x2 = -1; x2 < 2; x2++)
			{
				for (int y2 = -1; y2 < 2; y2++)
				{
					if (areas[x2 + 1, y2 + 1] == MainClass.Avail.InBounds)
					{
						if (tiles[x + x2, y + y2].initialized == false)
						{
							this.initialized = true;
							tiles[x + x2, y + y2].AssignValues(sizex, sizey, x + x2, y + y2, ref tiles);
						}
					}
				}
			}	
		}

		public void FloodFillNone(int sizex, int sizey, int x, int y, ref Tile[,] tiles)
		{
			this.hidden = false;
			MainClass.Avail[,] areas = MainClass.GetAvailableAreas(sizex, sizey, x, y);
			for (int x2 = -1; x2 < 2; x2++)
			{
				for (int y2 = -1; y2 < 2; y2++)
				{
					if (areas[x2 + 1, y2 + 1] == MainClass.Avail.InBounds)
					{
						if (tiles[x + x2, y + y2].value < 2)
						{
							if (tiles[x + x2, y + y2].hidden == true)
							{
								tiles[x + x2, y + y2].FloodFillNone(sizex, sizey, x + x2, y + y2, ref tiles);
							}
						}
					}
				}
			}
		}
	}
}

