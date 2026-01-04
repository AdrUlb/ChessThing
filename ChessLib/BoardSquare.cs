using System.Runtime.InteropServices;

namespace ChessLib;

public readonly record struct BoardSquare
{
	public static readonly BoardSquare Invalid = new BoardSquare()
	{
		BoardFile = BoardFile.None,
		BoardRank = 0,
	};

	public required BoardFile BoardFile { get; init; }
	public required byte BoardRank { get; init; }

	public string ToUciString() => $"{BoardFile switch
	{
		BoardFile.A => "a",
		BoardFile.B => "b",
		BoardFile.C => "c",
		BoardFile.D => "d",
		BoardFile.E => "e",
		BoardFile.F => "f",
		BoardFile.G => "g",
		BoardFile.H => "h",
		_ => "0"
	}}{(byte)BoardRank}";
	
	public static bool TryParseUciString(string uciString, out BoardSquare square)
	{
		square = Invalid;
		uciString = uciString.Trim();
		if (uciString.Length != 2)
			return false;

		var fileChar = uciString[0];
		var rankChar = uciString[1];

		var file = fileChar switch
		{
			'a' => BoardFile.A,
			'b' => BoardFile.B,
			'c' => BoardFile.C,
			'd' => BoardFile.D,
			'e' => BoardFile.E,
			'f' => BoardFile.F,
			'g' => BoardFile.G,
			'h' => BoardFile.H,
			_ => BoardFile.None
		};

		if (file == BoardFile.None)
			return false;

		if (!byte.TryParse(rankChar.ToString(), out var rank) || rank < 1 || rank > 8)
			return false;

		square = new()
		{
			BoardFile = file,
			BoardRank = rank
		};

		return true;
	}
}
