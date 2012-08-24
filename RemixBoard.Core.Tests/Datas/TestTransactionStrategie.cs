using NHibernate;
using RemixBoard.Datas.Infrastructure;

namespace RemixBoard.Core.Tests.Datas
{
    public class TestTransactionStrategie : ITransactionStrategie
    {
        private ITransaction transaction;

        public void Begin() {
            if(transaction == null)
                transaction = NhSessionManagement.Session.BeginTransaction();
        }

        public void End() {
            // ici, on ne commit pas, on se contente de pousser les donner dans la transaction
            // la classe de test se charge de faire un rollback() en fin de test (TearDown)
            NhSessionManagement.Session.Flush();
            NhSessionManagement.Session.Clear();
        }

        public void RollBack() {
            if (transaction != null) {
                transaction.Rollback();
                transaction.Dispose();
                transaction = null;
            }
        }
    }
}