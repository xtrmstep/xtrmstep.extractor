namespace Xtrmstep.Extractor.Core
{
    public interface IUnitOfWork
    {
        void Commit();
        void Rollback();
    }
}