using System.Text;

namespace ChessLib.Uci;

public struct UciGoParameters()
{
	// Restrict search to the given moves only
	// searchmoves <move1> .... <movei>
	public IReadOnlyList<BoardMove>? SearchMoves { get; set; } = null;

	// Search ponder moves during opponent's time (do not stop search, not even on mate)
	// ponder
	public bool Ponder { get; set; } = false;

	// White time left in ms
	// wtime <x>
	public int WhiteTime { get; set; } = -1;

	// Black time left in ms
	// btime <x>
	public int BlackTime { get; set; } = -1;

	// White increment per move in ms
	// winc <x>
	public int WhiteIncrement { get; set; } = -1;

	// Black increment per move in ms
	// binc <x>
	public int BlackIncrement { get; set; } = -1;

	// Number of moves to the next time control
	// movestogo <x>
	public int MovesToGo { get; set; } = -1;

	// Search to a fixed depth
	// depth <x>
	public int Depth { get; set; } = -1;

	// Search a fixed number of nodes
	// nodes <x>
	public int Nodes { get; set; } = -1;

	// Search for mate in x moves
	// mate <x>
	public int Mate { get; set; } = -1;

	// Search for a fixed amount of time
	// movetime <x>
	public int MoveTime { get; set; } = -1;

	// Search until the 'stop' command is sent
	// infinite
	public bool Infinite { get; set; } = false;

	public string ToUciString()
	{
		var builder = new StringBuilder();
		if (SearchMoves is { Count: > 0 })
		{
			builder.Append(" searchmoves");
			foreach (var move in SearchMoves)
				builder.Append(' ').Append(move.ToUciString());
		}

		if (Ponder)
			builder.Append(" ponder");

		if (WhiteTime != -1)
			builder.Append(" wtime ").Append(WhiteTime);

		if (BlackTime != -1)
			builder.Append(" btime ").Append(BlackTime);

		if (WhiteIncrement != -1)
			builder.Append(" winc ").Append(WhiteIncrement);

		if (BlackIncrement != -1)
			builder.Append(" binc ").Append(BlackIncrement);

		if (MovesToGo != -1)
			builder.Append(" movestogo ").Append(MovesToGo);

		if (Depth != -1)
			builder.Append(" depth ").Append(Depth);

		if (Nodes != -1)
			builder.Append(" nodes ").Append(Nodes);

		if (Mate != -1)
			builder.Append(" mate ").Append(Mate);

		if (MoveTime != -1)
			builder.Append(" movetime ").Append(MoveTime);

		if (Infinite)
			builder.Append(" infinite");

		return builder.ToString();
	}

	public static bool TryParseUciString(string uciString, out UciGoParameters parameters)
	{
		parameters = new();
		var tokens = uciString.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
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
						return false;

					parameters.WhiteTime = wtime;
					i++;
					break;
				case "btime":
					if (i + 1 >= tokens.Length || !int.TryParse(tokens[i + 1], out var btime))
						return false;

					parameters.BlackTime = btime;
					i++;
					break;
				case "winc":
					if (i + 1 >= tokens.Length || !int.TryParse(tokens[i + 1], out var winc))
						return false;

					parameters.WhiteIncrement = winc;
					i++;
					break;
				case "binc":
					if (i + 1 >= tokens.Length || !int.TryParse(tokens[i + 1], out var binc))
						return false;

					parameters.BlackIncrement = binc;
					i++;
					break;
				case "movestogo":
					if (i + 1 >= tokens.Length || !int.TryParse(tokens[i + 1], out var movestogo))
						return false;

					parameters.MovesToGo = movestogo;
					i++;
					break;
				case "depth":
					if (i + 1 >= tokens.Length || !int.TryParse(tokens[i + 1], out var depth))
						return false;

					parameters.Depth = depth;
					i++;
					break;
				case "nodes":
					if (i + 1 >= tokens.Length || !int.TryParse(tokens[i + 1], out var nodes))
						return false;

					parameters.Nodes = nodes;
					i++;
					break;
				case "mate":
					if (i + 1 >= tokens.Length || !int.TryParse(tokens[i + 1], out var mate))
						return false;

					parameters.Mate = mate;
					i++;
					break;
				case "movetime":
					if (i + 1 >= tokens.Length || !int.TryParse(tokens[i + 1], out var movetime))
						return false;

					parameters.MoveTime = movetime;
					i++;
					break;
				case "infinite":
					parameters.Infinite = true;
					break;
				default:
					return false;
			}
		}

		return true;
	}
}
