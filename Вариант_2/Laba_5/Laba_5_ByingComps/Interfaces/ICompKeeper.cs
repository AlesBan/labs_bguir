using System.Collections.Generic;
using Laba_5_ByingComps.Models;

namespace Laba_5_ByingComps.Interfaces
{
    public interface ICompKeeper
    {
        IEnumerable<ComputerCategory> GetAllComputers();
        ComputerCategory GetComputerCategoryByTitle(string title);
        void AddComputers(List<Computer> computerList);
        void RemoveComputers(List<Computer> computers);
    }
}