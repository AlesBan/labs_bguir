using Lab_5_ComputerPurchase.Models;

namespace Lab_5_ComputerPurchase.Interfaces;

public interface IComputerStorage
{
    ComputerBox? GetComputerBoxByTitle(string title);
    IEnumerable<ComputerBox> GetComputerBoxList();
    void UpdateComputerList(IEnumerable<ComputerBox> computerBoxList);
    void AddComputerList(List<Computer> computerList);
    void RemoveComputers(List<Computer> computers);
}