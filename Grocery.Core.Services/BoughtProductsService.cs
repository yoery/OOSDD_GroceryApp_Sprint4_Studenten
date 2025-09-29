using Grocery.Core.Models;
using Grocery.Core.Data.Repositories;

namespace Grocery.Core.Services
{
    public class BoughtProductsService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IProductRepository _productRepository;
        private readonly IShoppingListRepository _shoppingListRepository;

        public BoughtProductsService(
            IClientRepository clientRepository,
            IProductRepository productRepository,
            IShoppingListRepository shoppingListRepository)
        {
            _clientRepository = clientRepository;
            _productRepository = productRepository;
            _shoppingListRepository = shoppingListRepository;
        }

        public List<BoughtProductInfo> Get(int productId)
        {
            var result = new List<BoughtProductInfo>();
            var product = _productRepository.Get(productId);

            if (product == null)
                return result;

            var shoppingLists = _shoppingListRepository.GetAll();

            foreach (var shoppingList in shoppingLists)
            {
                if (shoppingList.Products.Any(p => p.Id == productId))
                {
                    var client = _clientRepository.Get(shoppingList.ClientId);
                    if (client != null)
                    {
                        result.Add(new BoughtProductInfo
                        {
                            Client = client,
                            ShoppingList = shoppingList,
                            Product = product
                        });
                    }
                }
            }

            return result;
        }
    }

    public class BoughtProductInfo
    {
        public Client Client { get; set; }
        public ShoppingList ShoppingList { get; set; }
        public Product Product { get; set; }
    }
}