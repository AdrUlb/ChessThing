namespace ChessLib.Uci;

public abstract class UciEngine(IUciUserInterface userInterface) : IUciEngine
{
	protected abstract string Name { get; }

	protected abstract string? Author { get; }

	protected abstract IReadOnlyList<UciOption> Options { get; }

	public bool IsDebug { get; private set; }

	void IUciEngine.Uci()
	{
		userInterface.Id(Name, Author);
		foreach (var option in Options)
			userInterface.Option(option);

		userInterface.UciOk();
	}

	void IUciEngine.IsReady() => userInterface.ReadyOk();

	void IUciEngine.Debug(bool debugOn)
	{
		IsDebug = debugOn;
		OnIsDebugChanged(debugOn);
	}

	void IUciEngine.SetOption(string name, string? value) => OnSetOption(name, value);

	void IUciEngine.UciNewGame() => OnNewGame();

	void IUciEngine.Position(bool startPosition, string? fen, IReadOnlyList<BoardMove> moves) => OnPosition(startPosition, fen, moves);

	void IUciEngine.Go(UciGoParameters parameters) => OnGo(parameters);

	void IUciEngine.Stop() => OnStop();

	void IUciEngine.PonderHit() => OnPonderHit();

	void IUciEngine.Quit() => OnQuit();

	protected virtual void OnIsDebugChanged(bool isDebug) { }
	protected virtual void OnSetOption(string name, string? value) { }
	protected virtual void OnNewGame() { }
	protected virtual void OnPosition(bool startPosition, string? fen, IReadOnlyList<BoardMove> moves) { }
	protected virtual void OnGo(UciGoParameters parameters) { }
	protected virtual void OnStop() { }
	protected virtual void OnPonderHit() { }
	protected virtual void OnQuit() { }
}
