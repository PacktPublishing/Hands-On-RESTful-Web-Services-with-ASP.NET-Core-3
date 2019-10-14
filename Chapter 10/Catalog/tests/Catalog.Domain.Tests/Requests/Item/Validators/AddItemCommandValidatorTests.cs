using System;
using System.Threading;
using Catalog.Domain.Entities;
using Catalog.Domain.Requests.Artists;
using Catalog.Domain.Requests.Genre;
using Catalog.Domain.Requests.Item;
using Catalog.Domain.Requests.Item.Validators;
using Catalog.Domain.Responses;
using Catalog.Domain.Services;
using FluentValidation.TestHelper;
using Moq;
using Xunit;

namespace Catalog.Domain.Tests.Requests.Item.Validators
{
    public class AddItemCommandValidatorTests
    {
        private readonly Mock<IArtistService> _artistServiceMock;
        private readonly Mock<IGenreService> _genreServiceMock;
        private readonly AddItemRequestValidator _validator;

        public AddItemCommandValidatorTests()
        {
            _artistServiceMock = new Mock<IArtistService>();
            _artistServiceMock
                .Setup(x => x.GetArtistAsync(It.IsAny<GetArtistRequest>(), CancellationToken.None))
                .ReturnsAsync(() => new ArtistResponse());

            _genreServiceMock = new Mock<IGenreService>();
            _genreServiceMock
                .Setup(x => x.GetGenreAsync(It.IsAny<GetGenreRequest>(), CancellationToken.None))
                .ReturnsAsync(() => new GenreResponse());

            _validator = new AddItemRequestValidator(_artistServiceMock.Object, _genreServiceMock.Object);
        }


        [Fact]
        public void should_have_error_when_ArtistId_is_null()
        {
            var addItemCommand = new AddItemRequest { Price = new Price() };
            _validator.ShouldHaveValidationErrorFor(x => x.ArtistId, addItemCommand);
        }

        [Fact]
        public void should_have_error_when_GenreId_is_null()
        {
            var addItemCommand = new AddItemRequest { Price = new Price() };
            _validator.ShouldHaveValidationErrorFor(x => x.GenreId, addItemCommand);
        }

        [Fact]
        public void should_have_error_when_ArtistId_doesnt_exist()
        {
            _artistServiceMock
                .Setup(x => x.GetArtistAsync(It.IsAny<GetArtistRequest>(), CancellationToken.None))
                .ReturnsAsync(() => null);

            var addItemCommand = new AddItemRequest { Price = new Price(), ArtistId = Guid.NewGuid() };
            _validator.ShouldHaveValidationErrorFor(x => x.ArtistId, addItemCommand);
        }

        [Fact]
        public void should_have_error_when_GenreId_doesnt_exist()
        {
            _genreServiceMock
                .Setup(x => x.GetGenreAsync(It.IsAny<GetGenreRequest>(), CancellationToken.None))
                .ReturnsAsync(() => null);

            var addItemCommand = new AddItemRequest { Price = new Price(), GenreId = Guid.NewGuid() };
            _validator.ShouldHaveValidationErrorFor(x => x.GenreId, addItemCommand);
        }
    }
}