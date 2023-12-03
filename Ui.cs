namespace GameOfLife
{
	internal class Ui
	{
		private const string _consoleTitle = "Игра \"Жизнь\"";
		private const int _cellColumns = 2;
		private const int _extraRows = 4;
		private const char _horizontalBound = (char)0x2550;
		private const char _verticalBound = (char)0x2551;
		private const char _upperLeftCorner = (char)0x2554;
		private const char _upperRightCorner = (char)0x2557;
		private const char _lowerRightCorner = (char)0x255D;
		private const char _lowerLeftCorner = (char)0x255A;

		private readonly string _liveCellDisplay = " O";
		private readonly string _deadCellDisplay = "  ";
		private readonly Life _life;
		private readonly int _rows;
		private readonly int _columns;
		private readonly int _delay;
		private readonly int _bufferWidth;

		private string? _initialConsoleTitle;
		private int _initialConsoleWindowWidth;
		private int _initialConsoleWindowHeight;
		private int _initialConsoleBufferWidth;
		private int _initialConsoleBufferHeight;

		public Ui(Life life, int delay)
		{
			if (delay <= 0)
				throw new ArgumentException("Задержка должна быть больше нуля", nameof(delay));

			_life = life;
			_rows = life.Height;
			_columns = life.Width;
			_delay = delay;
			_bufferWidth = _columns * _cellColumns + 2;

			Console.Title = _consoleTitle;

			PrepareConsoleWindow();
		}

		public bool ShowLife(bool finish = false)
		{
			Thread.Sleep(_delay);
			Console.SetCursorPosition(1, 1);

			var board = _life.Board;
			var generation = _life.Generation;

			for (var i = 0; i < _rows; i++)
			{
				for (var j = 0; j < _columns; j++)
					Console.Write(board[i, j] ? _liveCellDisplay : _deadCellDisplay);

				Console.WriteLine();

				Console.CursorLeft = 1;
			}

			if (finish)
			{
				ShowStatusMessage($"Поколение №{generation}. Игра завершена.");

				return false;
			}

			if (Console.KeyAvailable)
			{
				switch (Console.ReadKey(false).Key)
				{
					case ConsoleKey.Spacebar:
						ShowStatusMessage($"Поколение №{generation}. \"Пробел\" - продолжить, \"Отмена\" - выйти.");

						if (Console.ReadKey().Key == ConsoleKey.Escape)
							return ShowLife(true);

						break;
					case ConsoleKey.Escape:
						return ShowLife(true);
				}
			}
			else
				ShowStatusMessage($"Поколение №{generation}");

			return true;
		}

		public Configuration Greet()
		{
			Console.Clear();

			Console.CursorVisible = false;
			Console.CursorTop = _rows / 2 - 3;

			WriteLineCentered("Добро пожаловать в игру \"Жизнь\" Джона Конвея!", ConsoleColor.Green);

			Console.CursorTop++;

			var indent = _bufferWidth / 2 - 30;

			Console.CursorLeft = indent;

			Console.Write(" Используйте клавишу ");
			Write("Пробел (Space)", ConsoleColor.White);
			Console.Write(" для паузы и ");
			Write("Отмена (Escape)", ConsoleColor.White);
			Console.WriteLine(" для выхода.");

			Console.CursorLeft = indent;

			Console.WriteLine(" Нажмите:");

			Console.CursorLeft = indent;

			Console.Write("  > ");
			Write("Ввод (Enter)", ConsoleColor.White);
			Console.WriteLine(" для запуска глайдерного ружья Госпера,");

			Console.CursorLeft = indent;

			Console.WriteLine("  > любую другую клавишу для запуска случайной конфигурации.");

			Configuration res;

			switch (Console.ReadKey().Key)
			{
				case ConsoleKey.Enter:
					res = Configuration.GosperGliderGun;
					break;
				default:
					res = Configuration.Random;
					break;
			}

			DrawBorder();

			return res;
		}

		public void Bye()
		{
			Console.ResetColor();
			Console.ReadKey();
			RestoreConsoleWindow();

			Console.CursorVisible = true;
		}

		private void ShowStatusMessage(string message)
		{
			Console.SetCursorPosition(0, _rows + 2);
			Console.Write(new string(' ', Console.WindowWidth - 1));
			Console.SetCursorPosition(0, _rows + 2);
			WriteLineCentered(message);
		}

		private void WriteLineCentered(string message, ConsoleColor consoleColor = ConsoleColor.DarkGray)
		{
			Console.ForegroundColor = consoleColor;
			Console.CursorLeft = (_bufferWidth - message.Length) / 2;

			Console.WriteLine(message);
			Console.ResetColor();
		}

		private void Write(string message, ConsoleColor consoleColor = ConsoleColor.DarkGray)
		{
			Console.ForegroundColor = consoleColor;

			Console.Write(message);
			Console.ResetColor();
		}

		private void DrawBorder()
		{
			Console.ForegroundColor = ConsoleColor.Green;

			Console.Clear();
			Console.Write(_upperLeftCorner);
			Console.Write(new string(_horizontalBound, _columns * _cellColumns));
			Console.WriteLine(_upperRightCorner);

			for (var i = 0; i < _rows; i++)
			{
				Console.Write(_verticalBound);

				Console.CursorLeft = _bufferWidth - 1;

				Console.WriteLine(_verticalBound);
			}

			Console.Write(_lowerLeftCorner);
			Console.Write(new string(_horizontalBound, _columns * _cellColumns));
			Console.Write(_lowerRightCorner);
			Console.ResetColor();
		}

		private void PrepareConsoleWindow()
		{
			if (!OperatingSystem.IsWindows())
				return;

			_initialConsoleTitle = Console.Title;
			_initialConsoleWindowWidth = Console.WindowWidth;
			_initialConsoleWindowHeight = Console.WindowHeight;
			_initialConsoleBufferWidth = Console.BufferWidth;
			_initialConsoleBufferHeight = Console.BufferHeight;

			Console.SetWindowSize(_bufferWidth + 1, _rows + _extraRows);
			Console.SetBufferSize(_bufferWidth + 1, _initialConsoleBufferHeight);
		}

		private void RestoreConsoleWindow()
		{
			if (!OperatingSystem.IsWindows())
				return;

			Console.SetWindowSize(_initialConsoleWindowWidth, _initialConsoleWindowHeight);
			Console.SetBufferSize(_initialConsoleBufferWidth, _initialConsoleBufferHeight);
			Console.Clear();

			Console.Title = _initialConsoleTitle!;
		}
	}
}
