using ChessLib.Uci;
using ChessThing;

var output = new UciEngineOutputWriter(Console.Write);
var engine = new Engine(output);
var reader = new UciEngineInputParser(engine);

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
