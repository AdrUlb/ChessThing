namespace ChessLib.Uci;

public interface IUciUserInterface
{
	public void UciId(string name, string author);

	public void UciUciOk();

	public void ReadyOk();

	public void UciBestMove(BoardMove bestMove, BoardMove? ponderMove = null);

	public void UciInfo(UciInfoParameters infoParameters);
	
	public void UciOption(UciOption option);
}
