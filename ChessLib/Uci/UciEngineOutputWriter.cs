using System.Diagnostics;
using System.Text;

namespace ChessLib.Uci;

public sealed class UciEngineOutputWriter : IUciUserInterface
{
	public delegate void OutputHandler(string output);

	public required OutputHandler Output { get; init; }

	public void UciId(string name, string? author)
	{
		Output("id name ");
		Output(name);
		Output("\n");

		if (author is not null)
		{
			Output("id author ");
			Output(author);
			Output("\n");
		}
	}

	public void UciUciOk() => Output("uciok\n");

	public void ReadyOk() => Output("readyok\n");

	public void UciBestMove(BoardMove bestMove, BoardMove? ponderMove = null)
	{
		Output("bestmove ");
		Output(bestMove.ToUciString());

		if (ponderMove is { } pm && pm != BoardMove.Invalid)
		{
			Output(" ponder ");
			Output(pm.ToUciString());
		}

		Output("\n");
	}

	public void UciInfo(UciInfoParameters infoParameters)
	{
		Output("info");

		if (infoParameters.SearchDepthPlies != -1)
		{
			Output(" depth ");
			Output(infoParameters.SearchDepthPlies.ToString());
		}

		if (infoParameters.SelectiveSearchDepthPlies != -1)
		{
			Output(" seldepth ");
			Output(infoParameters.SelectiveSearchDepthPlies.ToString());
		}

		if (infoParameters.TimeSearchedMillis != -1)
		{
			Output(" time ");
			Output(infoParameters.TimeSearchedMillis.ToString());
		}

		if (infoParameters.NumNodesSearched != -1)
		{
			Output(" nodes ");
			Output(infoParameters.NumNodesSearched.ToString());
		}

		if (infoParameters.BestLine is { Count: > 0 })
		{
			Output(" pv");
			foreach (var move in infoParameters.BestLine)
			{
				Output(" ");
				Output(move.ToUciString());
			}

		}

		if (infoParameters.MultiPv != -1)
		{
			Output(" multipv ");
			Output(infoParameters.MultiPv.ToString());
		}

		if (infoParameters.ScoreCp != -1 || infoParameters.ScoreMate != -1)
		{
			Output(" score");
			if (infoParameters.ScoreCp != -1)
			{
				Output(" cp ");
				Output(infoParameters.ScoreCp.ToString());
			}

			if (infoParameters.ScoreMate != -1)
			{
				Output(" mate ");
				Output(infoParameters.ScoreMate.ToString());
			}

			if (infoParameters.ScoreIsLowerBound)
				Output(" lowerbound");

			if (infoParameters.ScoreIsUpperBound)
				Output(" upperbound");
		}

		if (infoParameters.CurrentMove != BoardMove.Invalid)
		{
			Output(" currmove ");
			Output(infoParameters.CurrentMove.ToUciString());
		}

		if (infoParameters.CurrentMoveNumber != -1)
		{
			Output(" currmovenumber ");
			Output(infoParameters.CurrentMoveNumber.ToString());
		}

		if (infoParameters.HashFullPermille != -1)
		{
			Output(" hashfull ");
			Output(infoParameters.HashFullPermille.ToString());
		}

		if (infoParameters.NodesPerSecond != -1)
		{
			Output(" nps ");
			Output(infoParameters.NodesPerSecond.ToString());
		}

		if (infoParameters.EndgameTableHits != -1)
		{
			Output(" tbhits ");
			Output(infoParameters.EndgameTableHits.ToString());
		}

		if (infoParameters.CpuLoadPermille != -1)
		{
			Output(" cpuload ");
			Output(infoParameters.CpuLoadPermille.ToString());
		}

		if (infoParameters.Refutation is { Count: > 0 })
		{
			Debug.Assert(infoParameters.Refutation.Count >= 2, "Refutation must contain at least the refuted move and one reply.");

			Output(" refutation");
			foreach (var move in infoParameters.Refutation)
			{
				Output(" ");
				Output(move.ToUciString());
			}
		}

		if (infoParameters.CurrentLine is { Count: > 0 })
		{
			Output(" currline");
			if (infoParameters.CurrentLineCpu != -1)
			{
				Output(" ");
				Output(infoParameters.CurrentLineCpu.ToString());
			}

			foreach (var move in infoParameters.CurrentLine)
			{
				Output(" ");
				Output(move.ToUciString());
			}
		}

		if (!string.IsNullOrEmpty(infoParameters.String))
		{
			Output(" string ");
			Output(infoParameters.String);
		}

		Output("\n");
	}

	public void UciOption(UciOption option)
	{
		Output("option name ");
		Output(option.Name);
		Output(" type ");
		Output(option.Type switch
		{
			UciOptionType.Check => "check",
			UciOptionType.Spin => "spin",
			UciOptionType.Combo => "combo",
			UciOptionType.String => "string",
			UciOptionType.Button => "button",
			_ => "unknown"
		});

		if (option.DefaultValue != null)
		{
			Output(" default ");
			Output(option.DefaultValue);
		}

		if (option.MinValue != null)
		{
			Output(" min ");
			Output(option.MinValue);
		}

		if (option.MaxValue != null)
		{
			Output(" max ");
			Output(option.MaxValue);
		}

		if (option.Vars != null)
		{
			foreach (var v in option.Vars)
			{
				Output(" var ");
				Output(v);
			}
		}

		Output("\n");
	}
}
