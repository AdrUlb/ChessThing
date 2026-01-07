using ChessLib;
using ChessLib.Uci;

namespace ChessThing;

public sealed class Engine(IUciUserInterface userInterface) : UciEngine(userInterface)
{
	protected override string Name => "ChessThing";

	protected override string? Author => "AdrUlb";

	protected override IReadOnlyList<UciOption> Options { get; } =
	[
		/*
		new()
		{
			Name = "Hash",
			Type = UciOptionType.Spin,
			DefaultValue = "16",
			MinValue = "1",
			MaxValue = (16 * 1024).ToString()
		},
		new()
		{
			Name = "Test Combo",
			Type = UciOptionType.Combo,
			DefaultValue = "Option1",
			Vars = ["Option1", "Option2", "Option3"]
		}
		*/
	];

	public bool IsQuitting { get; private set; } = false;

	protected override void OnQuit() => IsQuitting = true;

	protected override void OnSetOption(string name, string? value)
	{
		Program.Log($"TODO: setoption {name} = {value}");
	}

	protected override void OnIsDebugChanged(bool isDebug)
	{
		Program.Log($"TODO: debug {(isDebug ? "on" : "off")}");
	}

	protected override void OnPosition(bool startPosition, string? fen, IReadOnlyList<BoardMove> moves)
	{
		Program.Log("TODO: position");
	}

	protected override void OnGo(UciGoParameters parameters)
	{
		Program.Log("TODO: go");
	}

	protected override void OnStop()
	{
		Program.Log("TODO: stop");
	}

	protected override void OnNewGame()
	{
		Program.Log("TODO: new game");
	}

	protected override void OnPonderHit()
	{
		Program.Log("TODO: ponder hit");
	}
}
