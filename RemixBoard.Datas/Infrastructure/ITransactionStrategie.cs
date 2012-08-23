namespace RemixBoard.Datas
{
    public interface ITransactionStrategie {
        void Begin();
        void End();
        void RollBack();
    }
}