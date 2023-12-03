namespace GameOfLife
{
	internal static class BoardConfigurator
	{
		public static void Configure(Life life, Configuration configuration)
		{
			switch (configuration)
			{
				case Configuration.Random:
					ConfigureRandom(life);
					break;
				case Configuration.Glider:
					ConfigureGlider(life);
					break;
				case Configuration.GosperGliderGun:
					ConfigureGosperGliderGun(life);
					break;
			}
		}

		private static void ConfigureRandom(Life life)
		{
			const int minHeight = 10;
			const int minWidth = 10;

			if (life.Width < minWidth || life.Height < minHeight)
				throw new ArgumentException($"Слишком маленькое поле, необходимо {minHeight}x{minWidth} и больше", nameof(life));

			var random = new Random();
			var width = life.Width;
			var height = life.Height;
			var board = life.Board;

			for (var i = 0; i < height; i++)
			{
				for (var j = 0; j < width; j++)
					board[i, j] = random.Next() % 2 == 0;
			}
		}

		private static void ConfigureGlider(Life life)
		{
			const int minHeight = 8;
			const int minWidth = 35;

			if (life.Width < minWidth || life.Height < minHeight)
				throw new ArgumentException($"Слишком маленькое поле, необходимо {minHeight}x{minWidth} и больше", nameof(life));

			var board = life.Board;
			var middleRow = life.Height / 2;
			var middleColumn = life.Width / 2;

			board[middleRow - 1, middleColumn - 1] = true;
			board[middleRow - 1, middleColumn] = true;
			board[middleRow - 1, middleColumn + 1] = true;
			board[middleRow, middleColumn + 1] = true;
			board[middleRow + 1, middleColumn] = true;
		}

		private static void ConfigureGosperGliderGun(Life life)
		{
			const int minHeight = 8;
			const int minWidth = 35;

			if (life.Width < 3 || life.Height < 3)
				throw new ArgumentException($"Слишком маленькое поле, необходимо {minHeight}x{minWidth} и больше", nameof(life));

			var board = life.Board;
			var left = (life.Width - minWidth) / 2;
			var top = (life.Height - minHeight) / 2;

			board[left + 4, top + 0] = true;
			board[left + 5, top + 0] = true;
			board[left + 4, top + 1] = true;
			board[left + 5, top + 1] = true;

			board[left + 2, top + 12] = true;
			board[left + 2, top + 13] = true;
			board[left + 3, top + 15] = true;
			board[left + 4, top + 16] = true;
			board[left + 5, top + 16] = true;
			board[left + 5, top + 17] = true;
			board[left + 6, top + 16] = true;
			board[left + 7, top + 15] = true;
			board[left + 8, top + 13] = true;
			board[left + 8, top + 12] = true;
			board[left + 7, top + 11] = true;
			board[left + 6, top + 10] = true;
			board[left + 5, top + 10] = true;
			board[left + 4, top + 10] = true;
			board[left + 3, top + 11] = true;
			board[left + 5, top + 14] = true;

			board[left + 0, top + 24] = true;
			board[left + 1, top + 24] = true;
			board[left + 5, top + 24] = true;
			board[left + 6, top + 24] = true;
			board[left + 5, top + 22] = true;
			board[left + 4, top + 21] = true;
			board[left + 4, top + 20] = true;
			board[left + 3, top + 20] = true;
			board[left + 2, top + 20] = true;
			board[left + 2, top + 21] = true;
			board[left + 1, top + 22] = true;
			board[left + 3, top + 21] = true;

			board[left + 2, top + 34] = true;
			board[left + 2, top + 35] = true;
			board[left + 3, top + 35] = true;
			board[left + 3, top + 34] = true;
		}
	}
}
