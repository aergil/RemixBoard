namespace RemixBoard.Datas.Infrastructure
{
    public interface ITransactionStrategie {
        void Begin();
        void End();
        void RollBack();
    }
}