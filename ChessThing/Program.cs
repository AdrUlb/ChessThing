using System.Diagnostics;
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
		LogFile = File.AppendText(logFilePath);
	}

	public static void Main(string[] args)
	{
		try
		{
			var output = new UciEngineOutputWriter(static str =>
			{
				Log(string.Join("\n", str.Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(str => "O: " + str)));
				Console.Write(str);
			});
			
			Task.Run(() =>
			{
				using var engine = new Engine(output);
				var reader = new UciEngineInputParser(engine);

				while (!engine.IsQuitting)
				{
					var input = Console.ReadLine();
					if (input == null)
						break;

					Log($"I: {input}");
					reader.Parse(input);
				}
			}).Wait();
		}
		finally
		{
			LogFile.Flush();
		}
	}

	[Conditional("DEBUG")]
	public static void Log(string message)
	{
		var timestamp = DateTime.UtcNow.ToString("HH:mm:ss.fff");
		message = $"[{timestamp}] {message}";
		LogFile.WriteLine(message);
		LogFile.Flush();
	}
}
