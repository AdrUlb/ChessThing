namespace ChessLib.Uci;

public interface IUciEngine
{
	public void UciUci();

	public void UciIsReady();

	public void UciDebug(bool on);

	public void UciSetOption(string name, string? value);

	public void UciNewGame();

	public void UciPosition(string? fen, IReadOnlyList<BoardMove> moves);

	public void UciGo(UciGoParameters parameters);

	public void UciStop();

	public void UciPonderHit();

	public void UciQuit();
}
