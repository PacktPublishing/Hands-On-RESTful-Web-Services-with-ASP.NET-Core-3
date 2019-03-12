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
    public class EditItemHandler : IRequestHandler<EditItemCommand, ItemResponse>
    {
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public EditItemHandler(IItemRepository itemsRepository, IMapper mapper, ILogger<EditItemHandler> logger)
        {
            _itemRepository = itemsRepository;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<ItemResponse> Handle(EditItemCommand command, CancellationToken cancellationToken)
        {
            var existingRecord = await _itemRepository.GetAsync(command.Id);

            if (existingRecord == null) throw new ArgumentException($"Entity with {command.Id} is not present");

            var entity = _mapper.Map<Entities.Item>(command);
            var result = _itemRepository.Update(entity);

            var modifiedRecords = await _itemRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(LoggingEvents.Edit, LoggingMessages.NumberOfRecordAffected_modifiedRecords,
                modifiedRecords);
            _logger.LogInformation(LoggingEvents.Edit, LoggingMessages.ChangesApplied_id, result?.Id);

            return _mapper.Map<ItemResponse>(result);
        }
    }
}
