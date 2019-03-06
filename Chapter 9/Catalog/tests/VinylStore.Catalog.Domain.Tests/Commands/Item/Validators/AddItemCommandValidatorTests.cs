using FluentValidation.TestHelper;
using VinylStore.Catalog.Domain.Commands.Item;
using VinylStore.Catalog.Domain.Commands.Item.Validators;
using VinylStore.Catalog.Domain.Entities;
using Xunit;

namespace VinylStore.Catalog.Domain.Tests.Commands.Item.Validators
{
    public class AddItemCommandValidatorTests
    {
        public AddItemCommandValidatorTests()
        {
            _validator = new AddItemCommandValidator();
        }

        private readonly AddItemCommandValidator _validator;

        [Fact]
        public void should_have_error_when_ArtistId_is_null()
        {
            var addItemCommand = new AddItemCommand { Price = new Money() };
            _validator.ShouldHaveValidationErrorFor(x => x.GenreId, addItemCommand);
        }

        [Fact]
        public void should_have_error_when_GenreId_is_null()
        {
            var addItemCommand = new AddItemCommand { Price = new Money() };
            _validator.ShouldHaveValidationErrorFor(x => x.GenreId, addItemCommand);
        }
    }
}