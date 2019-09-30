using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Domain.Commands.Item;
using Catalog.Domain.Infrastructure.Repositories;
using Catalog.Domain.Responses.Item;
using MediatR;

namespace Catalog.Domain.Handlers.Item
{
    public class GetItemHandler : IRequestHandler<GetItemCommand, ItemResponse>
    {
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;

        public GetItemHandler(IItemRepository itemRepository, IMapper mapper)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
        }

        public async Task<ItemResponse> Handle(GetItemCommand command, CancellationToken cancellationToken)
        {
            if (command?.Id == null) throw new ArgumentNullException();
            return _mapper.Map<ItemResponse>(await _itemRepository.GetAsync(command.Id));
        }
    }
}