namespace ChessLib.Uci;

public interface IUciUserInterface
{
	public void Id(string name, string? author);

	public void UciOk();

	public void ReadyOk();

	public void BestMove(BoardMove bestMove, BoardMove? ponderMove = null);

	public void Info(UciInfoParameters infoParameters);
	
	public void Option(UciOption option);
}
