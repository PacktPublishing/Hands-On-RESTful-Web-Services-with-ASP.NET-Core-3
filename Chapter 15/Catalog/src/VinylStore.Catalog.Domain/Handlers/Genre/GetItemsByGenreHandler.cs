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
        private readonly IItemRepository _itemsRepository;
        private readonly IMapper _mapper;

        public GetItemsByGenreHandler(IItemRepository itemsRepository, IMapper mapper)
        {
            _itemsRepository = itemsRepository;
            _mapper = mapper;
        }

        public async Task<IList<ItemResponse>> Handle(GetItemsByGenreCommand command,
            CancellationToken cancellationToken)
        {
            if (command?.Id == null) throw new ArgumentNullException();
            var result = await _itemsRepository.GetItemByGenreIdAsync(command.Id);
            return _mapper.Map<List<ItemResponse>>(result);
        }
    }
}
