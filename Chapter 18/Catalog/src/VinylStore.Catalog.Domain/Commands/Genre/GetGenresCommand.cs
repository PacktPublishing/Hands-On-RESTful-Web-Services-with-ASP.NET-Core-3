using System.Collections.Generic;
using MediatR;
using VinylStore.Catalog.Domain.Responses.Item;

namespace VinylStore.Catalog.Domain.Commands.Genre
{
    public class GetGenresCommand : IRequest<List<GenreResponse>>
    {
    }
}
