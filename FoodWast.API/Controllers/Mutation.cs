using FoodWaste.Application.Interfaces;
using FoodWaste.Domain;

namespace FoodWast.API.Controllers
{
    public class Mutation
    {
        private readonly IPakketRepo _pakketRepo;
        private readonly IProductRepo _productRepo;
        public Mutation(IPakketRepo pakketRepo, IProductRepo productRepo)
        {
            _pakketRepo = pakketRepo;
            _productRepo = productRepo;
        }

    }
}
