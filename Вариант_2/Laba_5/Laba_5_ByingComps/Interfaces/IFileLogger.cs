using System.Collections.Generic;

namespace Laba_5_ByingComps.Interfaces
{
    public interface IFileLogger
    {
        void Log(IEnumerable<string> compTitleList, int purchasePrice);
    }
};

