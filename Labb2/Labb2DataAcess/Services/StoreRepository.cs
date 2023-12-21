using System.Collections.ObjectModel;
using Common.Models;
using Labb2DataAcess.Entities;

namespace Labb2DataAcess.Services;

public class StoreRepository
{
    private readonly Labb1Bokhandel2Context _context;

    public StoreRepository(Labb1Bokhandel2Context context)
    {
        _context = context;
    }


    public List<StoreModel> GetAllStores()
    {
        var stores = _context
            .Stores
            .Select
            (
                s => new StoreModel()
                {
                    Name = s.Name,
                    Id = s.Id,
                    IventoryBalances = s.IventoryBalances
                        .Select(ib => new InventoryBalanceModel()
                        {
                            Isbn13 = ib.Isbn13,
                            Quantity = ib.Quantity,
                            StoreId = ib.StoreId
                        }).ToList()

                }
            ).ToList();

        return stores;

    }

    


}