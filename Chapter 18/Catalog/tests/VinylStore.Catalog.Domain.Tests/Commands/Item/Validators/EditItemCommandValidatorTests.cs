using System;
using System.Threading;
using FluentValidation.TestHelper;
using MediatR;
using Moq;
using VinylStore.Catalog.Domain.Commands.Artists;
using VinylStore.Catalog.Domain.Commands.Genre;
using VinylStore.Catalog.Domain.Commands.Item;
using VinylStore.Catalog.Domain.Commands.Item.Validators;
using VinylStore.Catalog.Domain.Entities;
using VinylStore.Catalog.Domain.Responses.Item;
using Xunit;

namespace VinylStore.Catalog.Domain.Tests.Commands.Item.Validators
{
    public class EditItemCommandValidatorTests
    {
        public EditItemCommandValidatorTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _mediatorMock
                .Setup(x => x.Send(It.IsAny<GetArtistCommand>(), CancellationToken.None))
                .ReturnsAsync(() => new ArtistResponse());

            _mediatorMock
                .Setup(x => x.Send(It.IsAny<GetGenreCommand>(), CancellationToken.None))
                .ReturnsAsync(() => new GenreResponse());

            _validator = new EditItemCommandValidator(_mediatorMock.Object);
        }

        private readonly Mock<IMediator> _mediatorMock;
        private readonly EditItemCommandValidator _validator;

        [Fact]
        public void should_have_error_when_ArtistId_doesnt_exist()
        {
            _mediatorMock
                .Setup(x => x.Send(It.IsAny<GetArtistCommand>(), CancellationToken.None))
                .ReturnsAsync(() => null);

            var editItemCommand = new EditItemCommand { Price = new Money(), ArtistId = Guid.NewGuid() };
            _validator.ShouldHaveValidationErrorFor(x => x.ArtistId, editItemCommand);
        }

        [Fact]
        public void should_have_error_when_ArtistId_is_null()
        {
            var editItemCommand = new EditItemCommand { Price = new Money() };
            _validator.ShouldHaveValidationErrorFor(x => x.ArtistId, editItemCommand);
        }

        [Fact]
        public void should_have_error_when_GenreId_doesnt_exist()
        {
            _mediatorMock
                .Setup(x => x.Send(It.IsAny<GetGenreCommand>(), CancellationToken.None))
                .ReturnsAsync(() => null);

            var editItemCommand = new EditItemCommand { Price = new Money(), GenreId = Guid.NewGuid() };
            _validator.ShouldHaveValidationErrorFor(x => x.GenreId, editItemCommand);
        }

        [Fact]
        public void should_have_error_when_GenreId_is_null()
        {
            var editItemCommand = new EditItemCommand { Price = new Money() };
            _validator.ShouldHaveValidationErrorFor(x => x.GenreId, editItemCommand);
        }

        [Fact]
        public void should_have_error_when_Id_is_null()
        {
            var editItemCommand = new EditItemCommand { Price = new Money() };
            _validator.ShouldHaveValidationErrorFor(x => x.Id, editItemCommand);
        }
    }
}
