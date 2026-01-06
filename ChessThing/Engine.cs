using ChessLib.Uci;

namespace ChessThing;

public sealed class Engine(IUciUserInterface userInterface) : UciEngine(userInterface)
{
	protected override string Name => "ChessThing";

	protected override string? Author => "AdrUlb";

	protected override IReadOnlyList<UciOption> Options { get; } =
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
		}
	];
}
 