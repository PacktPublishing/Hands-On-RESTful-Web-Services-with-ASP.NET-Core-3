using Catalog.Domain.Entities;
using Catalog.Domain.Responses;

namespace Catalog.Domain.Mappers
{
    public class GenreMapper : IGenreMapper
    {
        public GenreResponse Map(Genre genre)
        {
            if (genre == null) return new GenreResponse();

            return new GenreResponse
            {
                GenreId = genre.GenreId,
                GenreDescription = genre.GenreDescription
            };
        }
    }
}