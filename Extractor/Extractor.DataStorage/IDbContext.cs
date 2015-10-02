using System.Data.Entity;
using Xtrmstep.Extractor.Core.Model;

namespace Xtrmstep.Extractor.Core
{
    public interface IDbContext
    {
        IDbSet<WebPage> Pages
        {
            get;
            set;
        }
    }
}