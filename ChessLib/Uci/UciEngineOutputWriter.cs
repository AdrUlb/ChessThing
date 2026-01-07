using System.Diagnostics;
using System.Text;

namespace ChessLib.Uci;

public sealed class UciEngineOutputWriter(UciEngineOutputWriter.WriteFunc writeFunc) : IUciUserInterface
{
	private sealed class CommandWriter(WriteFunc writeFunc)
	{
		[ThreadStatic] private static StringBuilder? _builder;

		private static StringBuilder Builder => _builder ??= new(256);

		public CommandWriter Append(string str)
		{
			Builder.Append(str);
			return this;
		}

		public CommandWriter Append(char ch)
		{
			Builder.Append(ch);
			return this;
		}

		public CommandWriter Append(long value)
		{
			Builder.Append(value);
			return this;
		}

		public CommandWriter Append(ulong value)
		{
			Builder.Append(value);
			return this;
		}

		public CommandWriter AppendLine()
		{
			Builder.AppendLine();
			return this;
		}

		public CommandWriter AppendLine(string str)
		{
			Builder.AppendLine(str);
			return this;
		}

		public void Write()
		{
			writeFunc(Builder.ToString());
			Builder.Clear();
		}
	}

	public delegate void WriteFunc(string output);

	private readonly CommandWriter _writer = new(writeFunc);

	public void Id(string name, string? author)
	{
		_writer
			.Append("id name ").AppendLine(name);

		if (author != null)
			_writer.Append("id author ").AppendLine(author);

		_writer.Write();
	}

	public void UciOk() => _writer.AppendLine("uciok").Write();

	public void ReadyOk() => _writer.AppendLine("readyok").Write();

	public void BestMove(BoardMove bestMove, BoardMove? ponderMove = null)
	{
		_writer.Append("bestmove ").Append(bestMove.ToUciString());

		if (ponderMove is { } pm && pm != BoardMove.Invalid)
		{
			_writer.Append(" ponder ").Append(pm.ToUciString());
		}

		_writer.AppendLine().Write();
	}

	public void Info(UciInfoParameters infoParameters)
	{
		_writer.Append("info");

		if (infoParameters.SearchDepthPlies != -1)
		{
			_writer.Append(" depth ").Append(infoParameters.SearchDepthPlies.ToString());
		}

		if (infoParameters.SelectiveSearchDepthPlies != -1)
		{
			_writer.Append(" seldepth ").Append(infoParameters.SelectiveSearchDepthPlies.ToString());
		}

		if (infoParameters.TimeSearchedMillis != -1)
		{
			_writer.Append(" time ").Append(infoParameters.TimeSearchedMillis.ToString());
		}

		if (infoParameters.NumNodesSearched != -1)
		{
			_writer.Append(" nodes ").Append(infoParameters.NumNodesSearched.ToString());
		}

		if (infoParameters.BestLine is { Count: > 0 })
		{
			_writer.Append(" pv");
			foreach (var move in infoParameters.BestLine)
				_writer.Append(" ").Append(move.ToUciString());
		}

		if (infoParameters.MultiPv != -1)
			_writer.Append(" multipv ").Append(infoParameters.MultiPv.ToString());

		if (infoParameters.ScoreCp != -1 || infoParameters.ScoreMate != -1)
		{
			_writer.Append(" score");
			if (infoParameters.ScoreCp != -1)
				_writer.Append(" cp ").Append(infoParameters.ScoreCp.ToString());

			if (infoParameters.ScoreMate != -1)
				_writer.Append(" mate ").Append(infoParameters.ScoreMate.ToString());

			if (infoParameters.ScoreIsLowerBound)
				_writer.Append(" lowerbound");

			if (infoParameters.ScoreIsUpperBound)
				_writer.Append(" upperbound");
		}

		if (infoParameters.CurrentMove != BoardMove.Invalid)
			_writer.Append(" currmove ").Append(infoParameters.CurrentMove.ToUciString());

		if (infoParameters.CurrentMoveNumber != -1)
			_writer.Append(" currmovenumber ").Append(infoParameters.CurrentMoveNumber.ToString());

		if (infoParameters.HashFullPermille != -1)
			_writer.Append(" hashfull ").Append(infoParameters.HashFullPermille.ToString());

		if (infoParameters.NodesPerSecond != -1)
			_writer.Append(" nps ").Append(infoParameters.NodesPerSecond.ToString());

		if (infoParameters.EndgameTableHits != -1)
			_writer.Append(" tbhits ").Append(infoParameters.EndgameTableHits.ToString());

		if (infoParameters.CpuLoadPermille != -1)
			_writer.Append(" cpuload ").Append(infoParameters.CpuLoadPermille.ToString());

		if (infoParameters.Refutation is { Count: > 0 })
		{
			Debug.Assert(infoParameters.Refutation.Count >= 2, "Refutation must contain at least the refuted move and one reply.");

			_writer.Append(" refutation");
			foreach (var move in infoParameters.Refutation)
				_writer.Append(" ").Append(move.ToUciString());
		}

		if (infoParameters.CurrentLine is { Count: > 0 })
		{
			_writer.Append(" currline");
			if (infoParameters.CurrentLineCpu != -1)
				_writer.Append(" ").Append(infoParameters.CurrentLineCpu.ToString());

			foreach (var move in infoParameters.CurrentLine)
				_writer.Append(" ").Append(move.ToUciString());
		}

		if (!string.IsNullOrEmpty(infoParameters.String))
		{
			_writer.Append(" string ").Append(infoParameters.String);
		}

		_writer.AppendLine().Write();
	}

	public void Option(UciOption option)
	{
		_writer.Append("option name ").Append(option.Name);
		_writer.Append(" type ").Append(option.Type switch
		{
			UciOptionType.Check => "check",
			UciOptionType.Spin => "spin",
			UciOptionType.Combo => "combo",
			UciOptionType.String => "string",
			UciOptionType.Button => "button",
			_ => "unknown"
		});

		if (option.DefaultValue != null)
			_writer.Append(" default ").Append(option.DefaultValue);

		if (option.MinValue != null)
			_writer.Append(" min ").Append(option.MinValue);

		if (option.MaxValue != null)
			_writer.Append(" max ").Append(option.MaxValue);

		if (option.Vars != null)
			foreach (var v in option.Vars)
				_writer.Append(" var ").Append(v);

		_writer.AppendLine().Write();
	}
}
