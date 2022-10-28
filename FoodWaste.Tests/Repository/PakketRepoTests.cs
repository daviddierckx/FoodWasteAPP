using FoodWaste.Domain;
using FoodWaste.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodWaste.Tests.Repository
{
    public class PakketRepoTests
    {
        public async Task<ApplicationDbContext> GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new ApplicationDbContext(options);
            databaseContext.Database.EnsureCreated();
            if(await databaseContext.Pakkets.CountAsync() < 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    databaseContext.Pakkets.Add(
                  new Pakket()
                  {
                      BeschrijvendeNaam = "Voedsel Pakket ",
                      SelectedProductId = "1",
                      Stad = "Breda",
                      Kantine = "LA",
                      TijdOphalen = DateTime.Now,
                      TijdTotOphalen = DateTime.Now,
                      Meerderjarig = true,
                      Prijs = 10,
                      TypeMaaltijd = "Brood"

                  });

                    await databaseContext.SaveChangesAsync();
                }
              

            }
            if (await databaseContext.Products.CountAsync() < 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    databaseContext.Products.Add(
                  new Product()
                  {
                      BeschrijvendeNaam = "Product ",
                      Foto = "www.image.com",
                      Id = i,
                      Alcohol = false
                  });

                    await databaseContext.SaveChangesAsync();
                }


            }
            return databaseContext;
        }

    }
}
