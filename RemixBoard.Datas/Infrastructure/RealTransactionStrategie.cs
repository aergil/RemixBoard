using NHibernate;

namespace RemixBoard.Datas.Infrastructure
{
    public class RealTransactionStrategie : ITransactionStrategie
    {
        private ITransaction transaction;

        public void Begin() {
            transaction = NhSessionManagement.Session.BeginTransaction();
        }

        public void End() {
            transaction.Commit();
            transaction.Dispose();
        }

        public void RollBack() {
            if (transaction != null) {
                transaction.Rollback();
                transaction.Dispose();
                NhSessionManagement.Session = null;
            }
        }
    }
}