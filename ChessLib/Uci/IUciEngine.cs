namespace ChessLib.Uci;

public interface IUciEngine
{
	public void Uci();

	public void IsReady();

	public void Debug(bool debugOn);

	public void SetOption(string name, string? value);

	public void UciNewGame();

	public void Position(bool startPosition, string? fen, IReadOnlyList<BoardMove> moves);

	public void Go(UciGoParameters parameters);

	public void Stop();

	public void PonderHit();

	public void Quit();
}
