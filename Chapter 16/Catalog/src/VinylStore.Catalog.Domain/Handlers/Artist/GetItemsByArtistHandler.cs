using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using VinylStore.Catalog.Domain.Commands.Artists;
using VinylStore.Catalog.Domain.Infrastructure.Repositories;
using VinylStore.Catalog.Domain.Responses.Item;

namespace VinylStore.Catalog.Domain.Handlers.Artist
{
    public class GetItemsByArtistHandler : IRequestHandler<PaginatedItemsResponseModel, IList<ItemResponse>>
    {
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;

        public GetItemsByArtistHandler(IItemRepository itemRepository, IMapper mapper)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
        }

        public async Task<IList<ItemResponse>> Handle(PaginatedItemsResponseModel command,
            CancellationToken cancellationToken)
        {
            if (command?.Id == null) throw new ArgumentNullException();
            var result = await _itemRepository.GetItemByArtistIdAsync(command.Id);
            return _mapper.Map<List<ItemResponse>>(result);
        }
    }
}
