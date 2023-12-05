using Lab_5_ComputerPurchase.Interfaces;

namespace Lab_5_ComputerPurchase.Models;

public class PurchaseManager : IPurchaseManager
{
    private readonly ComputerStorage _computerStorage;

    public PurchaseManager(ComputerStorage computerStorage)
    {
        _computerStorage = computerStorage;
    }

    public IEnumerable<ComputerBox> GetAllComputers()
    {
        var computerBoxList = _computerStorage.GetComputerBoxList();
        return computerBoxList;
    }

    public ComputerBox GetComputerBoxByTitle(string title)
    {
        var compBox = _computerStorage.GetComputerBoxByTitle(title);
        return compBox;
    }

    public void PurchaseComputers(List<Computer> computerBoxList)
    {
        _computerStorage.AddComputerList(computerBoxList);
    }

    public void WriteOffComputers(List<Computer> computers)
    {
        _computerStorage.RemoveComputers(computers);
    }
}