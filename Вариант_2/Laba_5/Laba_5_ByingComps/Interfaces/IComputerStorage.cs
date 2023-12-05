using System.Collections.Generic;
using Laba_5_ByingComps.Models;

namespace Laba_5_ByingComps.Interfaces
{
    public interface IComputerStorage
    {
        ComputerCategory? GetComputerCategoryByTitle(string title);
        IEnumerable<ComputerCategory> GetComputerCategoryList();
        void UpdateComputerList(IEnumerable<ComputerCategory> computerCategoryList);
        void AddComputerList(List<Computer> computerList);
        void RemoveComputers(List<Computer> computers);
    }
}

