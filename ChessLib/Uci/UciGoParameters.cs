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
}
