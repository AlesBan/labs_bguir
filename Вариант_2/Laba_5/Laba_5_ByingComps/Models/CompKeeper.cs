using System.Collections.Generic;
using Laba_5_ByingComps.Interfaces;

namespace Laba_5_ByingComps.Models
{
    public class CompKeeper : ICompKeeper
    {
        private readonly ComputerStorage _computerStorage;

        public CompKeeper(ComputerStorage computerStorage)
        {
            _computerStorage = computerStorage;
        }

        public IEnumerable<ComputerCategory> GetAllComputers()
        {
            var computerCategoryList = _computerStorage.GetComputerCategoryList();
            return computerCategoryList;
        }

        public ComputerCategory GetComputerCategoryByTitle(string title)
        {
            var compCategory = _computerStorage.GetComputerCategoryByTitle(title);
            return compCategory;
        }

        public void AddComputers(List<Computer> computerCategoryList)
        {
            _computerStorage.AddComputerList(computerCategoryList);
        }

        public void RemoveComputers(List<Computer> computers)
        {
            _computerStorage.RemoveComputers(computers);
        }
    }
};