using System;
using MediatR;
using VinylStore.Catalog.Domain.Responses.Item;

namespace VinylStore.Catalog.Domain.Commands.Genre
{
    public class GetGenreCommand : IRequest<GenreResponse>
    {
        public Guid Id { get; set; }
    }
}
