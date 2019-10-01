using System;
using System.Threading;
using Catalog.Domain.Entities;
using Catalog.Domain.Requests.Artists;
using Catalog.Domain.Requests.Genre;
using Catalog.Domain.Requests.Item;
using Catalog.Domain.Requests.Item.Validators;
using Catalog.Domain.Responses.Item;
using Catalog.Domain.Services;
using FluentValidation.TestHelper;
using Moq;
using Xunit;

namespace Catalog.Domain.Tests.Commands.Item.Validators
{
    public class EditItemCommandValidatorTests
    {
        private readonly Mock<IArtistService> _artistServiceMock;
        private readonly Mock<IGenreService> _genreServiceMock;
        private readonly EditItemRequestValidator _validator;

        public EditItemCommandValidatorTests()
        {
            _artistServiceMock = new Mock<IArtistService>();
            _artistServiceMock
                .Setup(x => x.GetArtistAsync(It.IsAny<GetArtistRequest>(), CancellationToken.None))
                .ReturnsAsync(() => new ArtistResponse());

            _genreServiceMock = new Mock<IGenreService>();
            _genreServiceMock
                .Setup(x => x.GetGenreAsync(It.IsAny<GetGenreRequest>(), CancellationToken.None))
                .ReturnsAsync(() => new GenreResponse());

            _validator = new EditItemRequestValidator(_artistServiceMock.Object, _genreServiceMock.Object);
        }

        [Fact]
        public void should_have_error_when_Id_is_null()
        {
            var editItemCommand = new EditItemRequest { Price = new Price() };
            _validator.ShouldHaveValidationErrorFor(x => x.Id, editItemCommand);
        }

        [Fact]
        public void should_have_error_when_ArtistId_is_null()
        {
            var editItemCommand = new EditItemRequest { Price = new Price() };
            _validator.ShouldHaveValidationErrorFor(x => x.ArtistId, editItemCommand);
        }

        [Fact]
        public void should_have_error_when_GenreId_is_null()
        {
            var editItemCommand = new EditItemRequest { Price = new Price() };
            _validator.ShouldHaveValidationErrorFor(x => x.GenreId, editItemCommand);
        }

        [Fact]
        public void should_have_error_when_ArtistId_doesnt_exist()
        {
            _artistServiceMock
                .Setup(x => x.GetArtistAsync(It.IsAny<GetArtistRequest>(), CancellationToken.None))
                .ReturnsAsync(() => null);

            var editItemCommand = new EditItemRequest { Price = new Price(), ArtistId = Guid.NewGuid() };
            _validator.ShouldHaveValidationErrorFor(x => x.ArtistId, editItemCommand);
        }

        [Fact]
        public void should_have_error_when_GenreId_doesnt_exist()
        {
            _genreServiceMock
                .Setup(x => x.GetGenreAsync(It.IsAny<GetGenreRequest>(), CancellationToken.None))
                .ReturnsAsync(() => null);

            var editItemCommand = new EditItemRequest { Price = new Price(), GenreId = Guid.NewGuid() };
            _validator.ShouldHaveValidationErrorFor(x => x.GenreId, editItemCommand);
        }
    }
}