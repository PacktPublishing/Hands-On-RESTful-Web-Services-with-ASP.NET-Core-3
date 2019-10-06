using Catalog.Domain.Entities;
using Catalog.Domain.Requests.Item;
using Catalog.Domain.Requests.Item.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace Catalog.Domain.Tests.Requests.Item.Validators
{
    public class AddItemRequestValidatorTests
    {
        private readonly AddItemRequestValidator _validator;

        public AddItemRequestValidatorTests()
        {
            _validator = new AddItemRequestValidator();
        }

        [Fact]
        public void should_have_error_when_ArtistId_is_null()
        {
            var addItemRequest = new AddItemRequest { Price = new Price() };
            _validator.ShouldHaveValidationErrorFor(x => x.GenreId, addItemRequest);
        }

        [Fact]
        public void should_have_error_when_GenreId_is_null()
        {
            var addItemRequest = new AddItemRequest { Price = new Price() };
            _validator.ShouldHaveValidationErrorFor(x => x.GenreId, addItemRequest);
        }
    }
}