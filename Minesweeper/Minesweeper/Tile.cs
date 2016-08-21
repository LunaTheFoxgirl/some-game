using System;

namespace Minesweeper
{
	public class Tile
	{
		public int x, y;
		public int value;
		public bool bomb;

		public Tile (int x, int y, int value, bool bomb)
		{
			this.x = x;
			this.y = y;
			this.value = value;
			this.bomb = bomb;
		}
	}
}

