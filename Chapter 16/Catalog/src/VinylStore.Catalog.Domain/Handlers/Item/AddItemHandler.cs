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
    public class AddItemHandler : IRequestHandler<AddItemCommand, ItemResponse>
    {
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public AddItemHandler(IItemRepository itemsRepository, IMapper mapper, ILogger<AddItemHandler> logger)
        {
            _itemRepository = itemsRepository;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<ItemResponse> Handle(AddItemCommand command, CancellationToken cancellationToken)
        {
            var item = _mapper.Map<Entities.Item>(command);

            var result = _itemRepository.Add(item);

            var modifiedRecords = await _itemRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(LoggingEvents.Add, LoggingMessages.NumberOfRecordAffected_modifiedRecords,
                modifiedRecords);
            _logger.LogInformation(LoggingEvents.Add, LoggingMessages.ChangesApplied_id, result?.Id);

            return _mapper.Map<ItemResponse>(result);
        }
    }
}
