using ChessLib;
using ChessLib.Uci;
using System.Diagnostics;

namespace ChessThing;

public sealed class Engine(IUciUserInterface userInterface) : UciEngine(userInterface), IDisposable
{
	protected override string Name => "ChessThing";

	protected override string? Author => "AdrUlb";

	private CancellationTokenSource? _searchCancellation;
	private Task? _searchTask;

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

	protected override void OnQuit()
	{
		StopSearch();
		IsQuitting = true;
	}

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
		StopSearch();
		StartSearch(parameters);
	}

	protected override void OnStop() => StopSearch();

	protected override void OnNewGame()
	{
		Program.Log("TODO: new game");
	}

	protected override void OnPonderHit()
	{
		Program.Log("TODO: ponder hit");
	}

	private void StartSearch(UciGoParameters parameters)
	{
		_searchCancellation = new();
		_searchTask = Task.Run(() => SearchProc(parameters, _searchCancellation.Token), _searchCancellation.Token);
	}

	private void StopSearch()
	{
		try
		{
			_searchCancellation?.Cancel();
			_searchTask?.Wait();
		}
		catch (AggregateException e)
		{
			Program.Log("Exception in search task: " + e.InnerException);
		}
		finally
		{
			_searchCancellation?.Dispose();
			_searchCancellation = null;
			_searchTask = null;
		}
	}

	private void SearchProc(UciGoParameters parameters, CancellationToken cancellationToken)
	{
		Program.Log("Starting started.");

		try
		{
			var move = BoardMove.Invalid;

			for (var depth = 1; depth <= 5; depth++)
			{
				cancellationToken.ThrowIfCancellationRequested();

				Thread.Sleep(100);

				move = new()
				{
					From = new() { BoardFile = BoardFile.E, BoardRank = 7 },
					To = new() { BoardFile = BoardFile.E, BoardRank = 5 },
				};

				UserInterface.Info(new()
				{
					SearchDepthPlies = depth,
					BestLine = [move]
				});
			}

			UserInterface.BestMove(move);
		}
		catch (OperationCanceledException)
		{
			Program.Log("Search cancelled.");
			UserInterface.BestMove(BoardMove.Invalid);
		}
		catch (Exception ex)
		{
			Program.Log("Exception during search: " + ex);
			UserInterface.BestMove(BoardMove.Invalid);
		}

		Program.Log("Search ended.");
	}

	public void Dispose()
	{
		_searchCancellation?.Cancel();
		_searchTask?.Wait(TimeSpan.FromSeconds(1));
		_searchCancellation?.Dispose();
	}
}
