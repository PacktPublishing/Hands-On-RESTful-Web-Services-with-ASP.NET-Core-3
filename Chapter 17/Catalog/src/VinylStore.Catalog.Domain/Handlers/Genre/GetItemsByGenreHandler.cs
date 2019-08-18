using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using VinylStore.Catalog.Domain.Commands.Genre;
using VinylStore.Catalog.Domain.Infrastructure.Repositories;
using VinylStore.Catalog.Domain.Responses.Item;

namespace VinylStore.Catalog.Domain.Handlers.Genre
{
    public class GetItemsByGenreHandler : IRequestHandler<GetItemsByGenreCommand, IList<ItemResponse>>
    {
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;

        public GetItemsByGenreHandler(IItemRepository itemRepository, IMapper mapper)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
        }

        public async Task<IList<ItemResponse>> Handle(GetItemsByGenreCommand command,
            CancellationToken cancellationToken)
        {
            if (command?.Id == null) throw new ArgumentNullException();
            var result = await _itemRepository.GetItemByGenreIdAsync(command.Id);
            return _mapper.Map<List<ItemResponse>>(result);
        }
    }
}
