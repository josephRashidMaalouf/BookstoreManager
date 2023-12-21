

using CommunityToolkit.Mvvm.ComponentModel;

namespace Common.Models;

public class InventoryBalanceModel : ObservableObject
{
    private int _quantity;
    public int StoreId { get; set; }

    public string Isbn13 { get; set; } = null!;

    public int Quantity
    {
        get => _quantity;
        set
        {
            if (value == _quantity) return;
            _quantity = value;
            OnPropertyChanged();
        }
    }

    public virtual BookModel Isbn13Navigation { get; set; } = null!;

    public virtual StoreModel Store { get; set; } = null!;
}