using System.Diagnostics;
using System.Text;

namespace ChessLib.Uci;

public readonly struct UciInfoParameters()
{
	// Search depth in plies
	// depth <x>
	public int SearchDepthPlies { get; init; } = -1;

	// Selective search depth in plies, must be sent with "depth
	// seldepth <x>
	public int SelectiveSearchDepthPlies { get; init; } = -1;

	// Time spent searching (in milliseconds), should be sent with "pv"
	// time <x>
	public int TimeSearchedMillis { get; init; } = -1;

	// Total nodes searched
	// nodes <x>
	public int NumNodesSearched { get; init; } = -1;

	// Best line (principal variation) found so far
	// pv <move1> ... <movei>
	public IReadOnlyList<BoardMove>? BestLine { get; init; } = null;

	// For multi-PV mode (1-based index of the line being reported)
	// multipv <num>
	public int MultiPv { get; init; } = -1;

	// Score
	// score cp <x> [lowerbound | upperbound]
	// score mate <x> [lowerbound | upperbound]
	public int ScoreCp { get; init; } = -1; // Centipawn score
	public int ScoreMate { get; init; } = -1; // Mate in N moves
	public bool ScoreIsLowerBound { get; init; } = false; // True if the score is only a lower bound
	public bool ScoreIsUpperBound { get; init; } = false; // True if the score is only an upper bound

	// The move currently being searched
	// currmove <move>
	public BoardMove CurrentMove { get; init; } = BoardMove.Invalid;

	// Number of the move currently being searched (1-based index)
	// currmovenumber <x>
	public int CurrentMoveNumber { get; init; } = -1;

	// The hash table is x permille full
	// hashfull <x>
	public int HashFullPermille { get; init; } = -1;

	// Nodes per second searched
	// nps <x>
	public int NodesPerSecond { get; init; } = -1;

	// Number of tablebase hits
	// tbhits <x>
	public int EndgameTableHits { get; init; } = -1;

	// CPU usage in permille
	// cpuload <x>
	public int CpuLoadPermille { get; init; } = -1;

	// Arbitrary string to be displayed
	// string <str>
	public string? String { get; init; } = null;

	// Refutation line (only send if option UCI_ShowRefutations is enabled)
	// 	// refutation <refutedMove> <move1> ... <movei>
	public IReadOnlyList<BoardMove>? Refutation { get; init; } = null;

	// Current search line
	public IReadOnlyList<BoardMove>? CurrentLine { get; init; } = null;

	// CPU number for current search line
	// currline <cpunr> <move1> ... <movei>
	public int CurrentLineCpu { get; init; } = -1;
}
