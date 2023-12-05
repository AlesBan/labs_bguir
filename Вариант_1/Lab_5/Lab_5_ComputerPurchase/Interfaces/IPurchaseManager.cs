using Lab_5_ComputerPurchase.Models;

namespace Lab_5_ComputerPurchase.Interfaces
{
    public interface IPurchaseManager
    {
        IEnumerable<ComputerBox> GetAllComputers();
        ComputerBox GetComputerBoxByTitle(string title);
        void PurchaseComputers(List<Computer> computerList);
        void WriteOffComputers(List<Computer> computers);
    }
}