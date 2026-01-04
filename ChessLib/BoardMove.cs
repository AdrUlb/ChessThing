using System.Runtime.InteropServices;

namespace ChessLib;

public readonly record struct BoardMove()
{
	public static readonly BoardMove Invalid = new()
	{
		From = BoardSquare.Invalid,
		To = BoardSquare.Invalid,
		Promotion = PieceType.None
	};

	public required BoardSquare From { get; init; }
	public required BoardSquare To { get; init; }
	public PieceType Promotion { get; init; } = PieceType.None;

	public string ToUciString()
	{
		if (From == BoardSquare.Invalid || To == BoardSquare.Invalid)
			return "0000";

		var uci = $"{From.ToUciString()}{To.ToUciString()}";
		if (Promotion != PieceType.None)
		{
			uci += Promotion switch
			{
				PieceType.Queen => "q",
				PieceType.Rook => "r",
				PieceType.Bishop => "b",
				PieceType.Knight => "n",
				_ => ""
			};
		}

		return uci;
	}

	public static bool TryParseUciString(string uciString, out BoardMove move)
	{
		uciString = uciString.Trim();

		move = Invalid;
		if (uciString.Length < 4)
			return false;

		if (!BoardSquare.TryParseUciString(uciString[..2], out var from))
			return false;

		if (!BoardSquare.TryParseUciString(uciString.Substring(2, 2), out var to))
			return false;

		var promotion = PieceType.None;
		if (uciString.Length > 4)
		{
			promotion = uciString[4] switch
			{
				'q' => PieceType.Queen,
				'r' => PieceType.Rook,
				'b' => PieceType.Bishop,
				'n' => PieceType.Knight,
				_ => PieceType.None
			};
		}

		move = new()
		{
			From = from,
			To = to,
			Promotion = promotion
		};

		return true;
	}
}
