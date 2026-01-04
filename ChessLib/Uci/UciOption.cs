namespace ChessLib.Uci;

public readonly struct UciOption
{
	public string Name { get; init; }
	public UciOptionType Type { get; init; }
	public string? DefaultValue { get; init; }
	public string? MinValue { get; init; }
	public string? MaxValue { get; init; }
	public IReadOnlyList<string>? Vars { get; init; }
}
