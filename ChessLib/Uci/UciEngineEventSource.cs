namespace ChessLib.Uci;

public class UciEngineEventSource(IUciUserInterface userInterface) : IUciEngine
{
	public required string Name { get; init; }
	public required string Author { get; init; }
	public required IReadOnlyList<UciOption> Options { get; init; }

	public delegate void DebugModeChangedHandler(bool isOn);
	public delegate void SetOptionHandler(string name, string? value);
	public delegate void NewGameHandler();
	public delegate void PositionHandler(string? fen, IReadOnlyList<BoardMove> moves);
	public delegate void GoHandler(UciGoParameters parameters);
	public delegate void StopHandler();
	public delegate void PonderHitHandler();
	public delegate void QuitHandler();

	public event DebugModeChangedHandler? DebugModeChanged;
	public event SetOptionHandler? SetOption;
	public event NewGameHandler? NewGame;
	public event PositionHandler? Position;
	public event GoHandler? Go;
	public event StopHandler? Stop;
	public event PonderHitHandler? PonderHit;
	public event QuitHandler? Quit;

	public void UciUci()
	{
		userInterface.UciId(Name, Author);
		foreach (var option in Options)
			userInterface.UciOption(option);

		userInterface.UciUciOk();
	}

	public void UciIsReady() => userInterface.ReadyOk();

	public void UciDebug(bool on) => DebugModeChanged?.Invoke(on);

	public void UciSetOption(string name, string? value) => SetOption?.Invoke(name, value);

	public void UciNewGame() => NewGame?.Invoke();

	public void UciPosition(string? fen, IReadOnlyList<BoardMove> moves) => Position?.Invoke(fen, moves);

	public void UciGo(UciGoParameters parameters) => Go?.Invoke(parameters);

	public void UciStop() => Stop?.Invoke();

	public void UciPonderHit() => PonderHit?.Invoke();

	public void UciQuit() => Quit?.Invoke();
}
