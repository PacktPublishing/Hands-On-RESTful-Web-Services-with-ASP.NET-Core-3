using System.Collections.Generic;
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
    public class GetItemsHandler : IRequestHandler<GetItemsCommand, IList<ItemResponse>>
    {
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public GetItemsHandler(IItemRepository itemRepository, IMapper mapper, ILogger<GetItemsHandler> logger)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IList<ItemResponse>> Handle(GetItemsCommand command, CancellationToken cancellationToken)
        {
            var result = await _itemRepository.GetAsync();
            _logger.LogInformation(LoggingEvents.GetById, LoggingMessages.NumberOfRecordAffected_modifiedRecords,
                result.Count);

            return _mapper.Map<IList<ItemResponse>>(result);
        }
    }
}
