using System;
using System.Collections.Generic;
using System.Linq;
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class GroceryListItemsService : IGroceryListItemsService
    {
        private readonly IGroceryListItemsRepository _groceriesRepository;
        private readonly IProductRepository _productRepository;

        public GroceryListItemsService(IGroceryListItemsRepository groceriesRepository, IProductRepository productRepository)
        {
            _groceriesRepository = groceriesRepository;
            _productRepository = productRepository;
        }

        public List<GroceryListItem> GetAll()
        {
            List<GroceryListItem> groceryListItems = _groceriesRepository.GetAll();
            FillService(groceryListItems);
            return groceryListItems;
        }

        public List<GroceryListItem> GetAllOnGroceryListId(int groceryListId)
        {
            List<GroceryListItem> groceryListItems = _groceriesRepository.GetAll().Where(g => g.GroceryListId == groceryListId).ToList();
            FillService(groceryListItems);
            return groceryListItems;
        }

        public GroceryListItem Add(GroceryListItem item)
        {
            return _groceriesRepository.Add(item);
        }

        public GroceryListItem? Delete(GroceryListItem item)
        {
            throw new NotImplementedException();
        }

        public GroceryListItem? Get(int id)
        {
            return _groceriesRepository.Get(id);
        }

        public GroceryListItem? Update(GroceryListItem item)
        {
            return _groceriesRepository.Update(item);
        }

        // Implementatie voor UC11 - Meest verkochte producten
        public List<BestSellingProducts> GetBestSellingProducts(int topX = 5)
        {
            // Haal alle grocery list items op
            var items = _groceriesRepository.GetAll();
            if (items == null || items.Count == 0) return new List<BestSellingProducts>();

            // Groepeer per product en bereken totaal aantal verkochte stuks per product
            // We gaan ervan uit dat de hoeveelheid in de property 'Amount' van GroceryListItem zit.
            var grouped = items
                .GroupBy(g => g.ProductId)
                .Select(g =>
                {
                    var total = g.Sum(x => x.Amount);
                    var product = _productRepository.Get(g.Key) ?? new Product(g.Key, "Onbekend", 0);
                    return new
                    {
                        ProductId = g.Key,
                        Name = product.Name,
                        Stock = product.Stock,
                        NrOfSells = total
                    };
                })
                .OrderByDescending(x => x.NrOfSells)
                .Take(topX)
                .ToList();

            // Zet ranking en maak BestSellingProducts objecten
            var result = new List<BestSellingProducts>();
            int rank = 1;
            foreach (var g in grouped)
            {
                result.Add(new BestSellingProducts(g.ProductId, g.Name, g.Stock, g.NrOfSells, rank));
                rank++;
            }

            return result;
        }

        private void FillService(List<GroceryListItem> groceryListItems)
        {
            foreach (GroceryListItem g in groceryListItems)
            {
                g.Product = _productRepository.Get(g.ProductId) ?? new Product(0, "", 0);
            }
        }
    }
}