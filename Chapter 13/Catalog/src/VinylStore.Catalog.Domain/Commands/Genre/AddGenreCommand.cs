using MediatR;
using VinylStore.Catalog.Domain.Responses.Item;

namespace VinylStore.Catalog.Domain.Commands.Genre
{
    public class AddGenreCommand : IRequest<GenreResponse>
    {
        public string GenreDescription { get; set; }
    }
}