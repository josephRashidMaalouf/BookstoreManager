using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;


namespace Common.Models;

public class StoreModel : ObservableObject
{
    private ObservableCollection<BookModel> _books;
    public string Name { get; set; } = null!;

    public int Id { get; set; } 

    public virtual List<InventoryBalanceModel> IventoryBalances { get; set; } = new ();

    public override string ToString()
    {
        return Name;
    }
}