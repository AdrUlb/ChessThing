using ChessLib.Uci;

namespace ChessThing;

internal static class Program
{
#if DEBUG
	public static readonly StreamWriter LogFile;
#endif

	static Program()
	{
		var logFilePath = Path.Combine("Logs", $"{DateTime.UtcNow:yyyy-MM-dd_HH-mm-ss}.txt");
		Directory.CreateDirectory("Logs");
#if DEBUG
		LogFile = File.AppendText(logFilePath);
#endif
	}

	public static void Main(string[] args)
	{

		var output = new UciEngineOutputWriter(static str =>
		{
#if DEBUG
			Log(string.Join("\n", str.Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(str => "< " + str)));
#endif
			Console.Write(str);
		});

		var engine = new Engine(output);
		var reader = new UciEngineInputParser(engine);

		var inputThread = new Thread(() =>
		{
			while (!engine.IsQuitting)
			{
				var input = Console.ReadLine();
				if (input == null)
					continue;

#if DEBUG
				Log($"> {input}");
#endif
				reader.Parse(input);
			}
		});

		inputThread.Start();

		inputThread.Join();
		LogFile.Flush();
	}

	public static void Log(string message)
	{
		LogFile.WriteLine(message);
		LogFile.Flush();
	}
}
