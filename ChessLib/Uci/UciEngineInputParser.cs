namespace ChessLib.Uci;

public class UciEngineInputParser(IUciEngine engine)
{
	public void Parse(string input)
	{
		var splitIndex = input.IndexOf(' ');
		var command = splitIndex == -1 ? input : input[..splitIndex];
		var arguments = splitIndex == -1 ? string.Empty : input[(splitIndex + 1)..];
		command = command.Trim();
		arguments = arguments.Trim();
		switch (command)
		{
			case "uci":
				engine.UciUci();
				break;
			case "isready":
				engine.UciIsReady();
				break;
			case "setoption":
				{
					var garbage = "";
					var name = "";
					var value = "";
					ref var assigningTo = ref garbage;
					foreach (var arg in arguments.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
					{
						switch (arg)
						{
							case "name":
								assigningTo = ref name!;
								continue;
							case "value":
								assigningTo = ref value!;
								continue;
							default:
								if (assigningTo.Length == 0)
									assigningTo += " ";

								assigningTo += arg;
								break;
						}

					}

					engine.UciSetOption(name, value);
					break;
				}
			case "ucinewgame":
				engine.UciNewGame();
				break;
			case "position":
				{
					string? fen = null;
					var args = arguments.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
					if (args[0] == "startpos")
					{
						fen = Constants.InitialFen;
						args = args[1..];
					}
					else if (args[0] == "fen")
					{
						var fenParts = new List<string>();
						for (var i = 1; i < args.Length; i++)
						{
							if (args[i] == "moves")
							{
								args = args[i..];
								break;
							}

							fenParts.Add(args[i]);
						}

						fen = string.Join(' ', fenParts);
					}

					var moves = new List<BoardMove>();
					foreach (var arg in args)
					{
						if (BoardMove.TryParseUciString(arg, out var move))
							moves.Add(move);
					}

					engine.UciPosition(fen, moves);
					break;
				}
			case "go":
				{
					if (UciGoParameters.TryParseUciString(arguments, out var goParams))
						engine.UciGo(goParams);

					break;
				}
			case "stop":
				engine.UciStop();
				break;
			case "ponderhit":
				engine.UciPonderHit();
				break;
			case "quit":
				engine.UciQuit();
				break;
			default:
				// Unknown command
				break;
		}
	}
}
