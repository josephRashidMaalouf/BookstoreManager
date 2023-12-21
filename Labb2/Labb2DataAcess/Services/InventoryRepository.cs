

using Common.Models;
using Labb2DataAcess.Entities;

namespace Labb2DataAcess.Services;

public class InventoryRepository
{
    private readonly Labb1Bokhandel2Context _context;

    public InventoryRepository(Labb1Bokhandel2Context context)
    {
        _context = context;
    }

    public List<InventoryBalanceModel> GetAllIbs()
    {
        var ibs = _context.IventoryBalances
            .Select
                (i => new InventoryBalanceModel
                    {
                        Isbn13 = i.Isbn13,
                        Quantity = i.Quantity,
                        StoreId = i.StoreId,
                    }
                ).ToList();
        return ibs;
    }

    public int GetAmountOfBookInStore(StoreModel store, string isbn) 
    {
        
        var ib = _context.IventoryBalances.FirstOrDefault(i => i.Isbn13 == isbn);


        return ib.Quantity;
    }

    public void UpdateBookQuantityForStore(StoreModel store, string isbn, int quantity)
    {

        var ib = _context.IventoryBalances.FirstOrDefault(i => (i.Isbn13 == isbn && i.Store.Name == store.Name));

        if (quantity <= 0)
        {
            if ((ib.Quantity += quantity) <= 0)
            {
                ib.Quantity = 0;
                _context.SaveChanges();
                return;
            }
        }

        ib.Quantity += quantity;

        _context.SaveChanges();
    }

    public void AddBookToStore(StoreModel store, string isbn)
    {
        var storeEntity = _context.Stores.FirstOrDefault(s => s.Name == store.Name);

        var newIb = new IventoryBalance()
        {
            Isbn13 = isbn,
            Quantity = 1,
            StoreId = storeEntity.Id
        };
        _context.IventoryBalances.Add(newIb);

        _context.SaveChanges();
    }

    public void DeleteBookFromStore(StoreModel store, string isbn)
    {
        var ib = _context.IventoryBalances.FirstOrDefault(i => (i.Isbn13 == isbn && i.StoreId == store.Id));

        _context.IventoryBalances.Remove(ib);

        _context.SaveChanges();
    }
}