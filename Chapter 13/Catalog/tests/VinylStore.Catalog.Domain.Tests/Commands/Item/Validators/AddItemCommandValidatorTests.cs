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
    public class AddItemCommandValidatorTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly AddItemCommandValidator _validator;

        public AddItemCommandValidatorTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _mediatorMock
                .Setup(x => x.Send(It.IsAny<GetArtistCommand>(), CancellationToken.None))
                .ReturnsAsync(() => new ArtistResponse());

            _mediatorMock
                .Setup(x => x.Send(It.IsAny<GetGenreCommand>(), CancellationToken.None))
                .ReturnsAsync(() => new GenreResponse());

            _validator = new AddItemCommandValidator(_mediatorMock.Object);
        }


        [Fact]
        public void should_have_error_when_ArtistId_is_null()
        {
            var addItemCommand = new AddItemCommand { Price = new Money() };
            _validator.ShouldHaveValidationErrorFor(x => x.ArtistId, addItemCommand);
        }

        [Fact]
        public void should_have_error_when_GenreId_is_null()
        {
            var addItemCommand = new AddItemCommand { Price = new Money() };
            _validator.ShouldHaveValidationErrorFor(x => x.GenreId, addItemCommand);
        }

        [Fact]
        public void should_have_error_when_ArtistId_doesnt_exist()
        {
            _mediatorMock
                .Setup(x => x.Send(It.IsAny<GetArtistCommand>(), CancellationToken.None))
                .ReturnsAsync(() => null);

            var addItemCommand = new AddItemCommand { Price = new Money(), ArtistId = Guid.NewGuid() };
            _validator.ShouldHaveValidationErrorFor(x => x.ArtistId, addItemCommand);
        }

        [Fact]
        public void should_have_error_when_GenreId_doesnt_exist()
        {
            _mediatorMock
                .Setup(x => x.Send(It.IsAny<GetGenreCommand>(), CancellationToken.None))
                .ReturnsAsync(() => null);

            var addItemCommand = new AddItemCommand { Price = new Money(), GenreId = Guid.NewGuid() };
            _validator.ShouldHaveValidationErrorFor(x => x.GenreId, addItemCommand);
        }
    }
}