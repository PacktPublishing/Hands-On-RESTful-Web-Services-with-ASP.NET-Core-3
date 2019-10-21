using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Cart.Domain.Commands.Cart;
using Cart.Domain.Infrastructure.Repositories;
using Cart.Domain.Responses.Cart;
using Cart.Domain.Services;
using MediatR;

namespace Cart.Domain.Handlers.Cart
{
    public class GetCartHandler : IRequestHandler<GetCartCommand, CartExtendedResponse>
    {
        private readonly ICatalogService _catalogService;
        private readonly IMapper _mapper;
        private readonly ICartRepository _repository;


        public GetCartHandler(
            ICartRepository repository,
            IMapper mapper,
            ICatalogService catalogService)
        {
            _repository = repository;
            _mapper = mapper;
            _catalogService = catalogService;
        }

        public async Task<CartExtendedResponse> Handle(GetCartCommand command, CancellationToken cancellationToken)
        {
            var result = await _repository.GetAsync(command.Id);
            var extendedResponse = _mapper.Map<CartExtendedResponse>(result);

            var tasks = extendedResponse.Items
                .Select(async x => await _catalogService.EnrichCartItem(x, cancellationToken));

            extendedResponse.Items = await Task.WhenAll(tasks);
            return extendedResponse;
        }
    }
}