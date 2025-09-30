using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class BoughtProductsService : IBoughtProductsService
    {
        private readonly IGroceryListItemsRepository _groceryListItemsRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IGroceryListRepository _groceryListRepository;
        private readonly IProductRepository _productRepository;

        public BoughtProductsService(
            IGroceryListItemsRepository groceryListItemsRepository,
            IClientRepository clientRepository,
            IGroceryListRepository groceryListRepository,
            IProductRepository productRepository)
        {
            _groceryListItemsRepository = groceryListItemsRepository;
            _clientRepository = clientRepository;
            _groceryListRepository = groceryListRepository;
            _productRepository = productRepository;
        }

        public List<BoughtProducts> Get(int? productId)
        {
            var items = _groceryListItemsRepository.GetAll();

            // Filter op productId indien meegegeven
            if (productId.HasValue)
                items = items.Where(i => i.ProductId == productId.Value).ToList();

            var result = new List<BoughtProducts>();

            foreach (var item in items)
            {
                var groceryList = _groceryListRepository.Get(item.GroceryListId);
                if (groceryList == null) continue;

                var client = _clientRepository.Get(groceryList.ClientId);
                if (client == null) continue;

                var product = _productRepository.Get(item.ProductId);
                if (product == null) continue;

                result.Add(new BoughtProducts(client, groceryList, product));
            }

            return result;
        }
    }
}
