using ChessLib;
using ChessLib.Uci;

var output = new UciEngineOutputWriter { Output = Console.Write };

var eventSource = new UciEngineEventSource(output)
{
	Name = "ChessThing",
	Author = "AdrUlb",
	Options =
	[
		new() // Hash size in MB
		{
			Name = "Hash",
			Type = UciOptionType.Spin,
			DefaultValue = "16",
			MinValue = "1",
			MaxValue = (16 * 1024).ToString()
		},
		new() // Testing Combo option
		{
			Name = "Test Combo",
			Type = UciOptionType.Combo,
			DefaultValue = "Option1",
			Vars = ["Option1", "Option2", "Option3"]
		},
	]
};

eventSource.Quit += () => Environment.Exit(0);

var reader = new UciEngineInputParser(eventSource);

var inputThread = new Thread(() =>
{
	while (true)
	{
		var input = Console.ReadLine();
		if (input == null)
			break;

		reader.Parse(input);
	}
});

inputThread.Start();
