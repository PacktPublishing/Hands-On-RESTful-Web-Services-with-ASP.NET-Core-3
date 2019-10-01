using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Domain.Commands.Item;
using Catalog.Domain.Handlers.Item;
using Catalog.Domain.Mapper;
using Catalog.Fixtures;
using Catalog.Infrastructure.Repositories;
using Shouldly;
using Xunit;

namespace Catalog.Domain.Tests.Handlers.Item
{
    public class GetItemsHandlerTests : IClassFixture<CatalogDataContextFactory>
    {
        private readonly CatalogDataContextFactory _catalogDataContextFactory;

        public GetItemsHandlerTests(CatalogDataContextFactory catalogDataContextFactory)
        {
            _catalogDataContextFactory = catalogDataContextFactory;
        }

  
    }
}