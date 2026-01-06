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
				engine.Uci();
				break;
			case "isready":
				engine.IsReady();
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

					engine.SetOption(name, value);
					break;
				}
			case "ucinewgame":
				engine.UciNewGame();
				break;
			case "position":
				{
					var startPos = false;
					string? fen = null;
					var args = arguments.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
					if (args[0] == "startpos")
					{
						startPos = true;
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

					engine.Position(startPos, fen, moves);
					break;
				}
			case "go":
				{
					var parameters = new UciGoParameters();
					var tokens = arguments.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
					var i = 0;
					while (i < tokens.Length)
					{
						switch (tokens[i])
						{
							case "searchmoves":
								var moves = new List<BoardMove>();
								i++;
								while (i < tokens.Length && BoardMove.TryParseUciString(tokens[i], out var move))
								{
									moves.Add(move);
									i++;
								}

								parameters.SearchMoves = moves;
								continue;
							case "ponder":
								parameters.Ponder = true;
								break;
							case "wtime":
								if (i + 1 >= tokens.Length || !int.TryParse(tokens[i + 1], out var wtime))
									break;

								parameters.WhiteTime = wtime;
								i++;
								break;
							case "btime":
								if (i + 1 >= tokens.Length || !int.TryParse(tokens[i + 1], out var btime))
									break;

								parameters.BlackTime = btime;
								i++;
								break;
							case "winc":
								if (i + 1 >= tokens.Length || !int.TryParse(tokens[i + 1], out var winc))
									break;

								parameters.WhiteIncrement = winc;
								i++;
								break;
							case "binc":
								if (i + 1 >= tokens.Length || !int.TryParse(tokens[i + 1], out var binc))
									break;

								parameters.BlackIncrement = binc;
								i++;
								break;
							case "movestogo":
								if (i + 1 >= tokens.Length || !int.TryParse(tokens[i + 1], out var movestogo))
									break;

								parameters.MovesToGo = movestogo;
								i++;
								break;
							case "depth":
								if (i + 1 >= tokens.Length || !int.TryParse(tokens[i + 1], out var depth))
									break;

								parameters.Depth = depth;
								i++;
								break;
							case "nodes":
								if (i + 1 >= tokens.Length || !int.TryParse(tokens[i + 1], out var nodes))
									break;

								parameters.Nodes = nodes;
								i++;
								break;
							case "mate":
								if (i + 1 >= tokens.Length || !int.TryParse(tokens[i + 1], out var mate))
									break;

								parameters.Mate = mate;
								i++;
								break;
							case "movetime":
								if (i + 1 >= tokens.Length || !int.TryParse(tokens[i + 1], out var movetime))
									break;

								parameters.MoveTime = movetime;
								i++;
								break;
							case "infinite":
								parameters.Infinite = true;
								break;
						}
					}

					engine.Go(parameters);
					break;
				}
			case "stop":
				engine.Stop();
				break;
			case "ponderhit":
				engine.PonderHit();
				break;
			case "quit":
				engine.Quit();
				break;
		}
	}
}
