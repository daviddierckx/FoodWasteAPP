using FoodWaste.Application.Interfaces;
using FoodWaste.Domain;
using FoodWaste.Domain.Enums;
using FoodWaste.Infrastructure.Data;

namespace FoodWast.API.Controllers
{
    public class Query
    {
        private readonly IPakketRepo _pakketRepo;
        private readonly IProductRepo _productRepo;
        public Query(IPakketRepo pakketRepo,IProductRepo productRepo)
        {
            _pakketRepo = pakketRepo;
            _productRepo = productRepo;
        }

        public async Task<IEnumerable<Pakket>> GetPakkets([Service] IPakketRepo pakketRepo)
        {
           var pakkets = pakketRepo.GetAllPackets();
            return await pakkets;
        }

        public IEnumerable<Product> GetProducts([Service] IProductRepo productRepo)
        {
            var products = productRepo.GetAll();
            return products;
        }

    }
}
