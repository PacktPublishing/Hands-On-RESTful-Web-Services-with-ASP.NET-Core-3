using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using VinylStore.Catalog.Domain.Commands.Item;
using VinylStore.Catalog.Domain.Infrastructure;
using VinylStore.Catalog.Domain.Infrastructure.Repositories;
using VinylStore.Catalog.Domain.Responses.Item;

namespace VinylStore.Catalog.Domain.Handlers.Item
{
    public class GetItemHandler : IRequestHandler<GetItemCommand, ItemResponse>
    {
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public GetItemHandler(IItemRepository itemRepository, IMapper mapper, ILogger<GetItemHandler> logger)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ItemResponse> Handle(GetItemCommand command, CancellationToken cancellationToken)
        {
            if (command?.Id == null) throw new ArgumentNullException();

            var result = await _itemRepository.GetAsync(command.Id);
            _logger.LogInformation(LoggingEvents.GetById, LoggingMessages.TargetEntityChanged_id, result?.Id);

            return _mapper.Map<ItemResponse>(result);
        }
    }
}
