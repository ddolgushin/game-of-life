namespace GameOfLife
{
	internal class Life
	{
		private const int _minRows = 10;
		private const int _minColumns = 10;

		private readonly int _rows;
		private readonly int _columns;

		private bool[,] _current;
		private bool[,] _next;
		private int _generation;

		public int Width => _columns;
		public int Height => _rows;
		public bool[,] Board => _current;
		public int Generation => _generation;

		public Life(int rows, int columns)
		{
			if (rows <= _minRows)
				throw new ArgumentException($"Количество строк должно быть больше {_minRows}", nameof(rows));
			if (columns <= _minColumns)
				throw new ArgumentException($"Количество столбцов должно быть больше {_minColumns}", nameof(columns));

			_rows = rows;
			_columns = columns;
			_current = new bool[rows, columns];
			_next = new bool[rows, columns];
		}

		public bool Iterate()
		{
			var stateChanged = false;
			var current = _current;
			var next = _next;

			for (var i = 0; i < _rows; i++)
			{
				for (var j = 0; j < _columns; j++)
				{
					var neighbours = CountNeighbours(current, i, j);

					if (current[i, j])
						next[i, j] = neighbours == 2 || neighbours == 3;
					else
						next[i, j] = neighbours == 3;

					if (!stateChanged)
						stateChanged = current[i, j] != next[i, j];
				}
			}

			_next = current;
			_current = next;

			_generation++;

			return stateChanged;
		}

		private int CountNeighbours(bool[,] buf, int i, int j)
		{
			var res = 0;
			var nonBottom = i < _rows - 1;
			var nonTop = i > 0;
			var nonLeftBound = j > 0;
			var nonRightBound = j < _columns - 1;

			if (nonTop && nonLeftBound && buf[i - 1, j - 1])
				res++;
			if (nonTop && buf[i - 1, j])
				res++;
			if (nonTop && nonRightBound && buf[i - 1, j + 1])
				res++;
			if (nonRightBound && buf[i, j + 1])
				res++;
			if (nonBottom && nonRightBound && buf[i + 1, j + 1])
				res++;
			if (nonBottom && buf[i + 1, j])
				res++;
			if (nonBottom && nonLeftBound && buf[i + 1, j - 1])
				res++;
			if (nonLeftBound && buf[i, j - 1])
				res++;

			return res;
		}
	}
}
