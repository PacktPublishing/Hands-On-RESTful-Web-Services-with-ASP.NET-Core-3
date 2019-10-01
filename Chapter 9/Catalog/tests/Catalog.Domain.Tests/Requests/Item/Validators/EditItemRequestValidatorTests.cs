using Catalog.Domain.Entities;
using Catalog.Domain.Requests.Item;
using Catalog.Domain.Requests.Item.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace Catalog.Domain.Tests.Requests.Item.Validators
{
    public class EditItemRequestValidatorTests
    {
        private readonly EditItemRequestValidator _validator;

        public EditItemRequestValidatorTests()
        {
            _validator = new EditItemRequestValidator();
        }

        [Fact]
        public void should_have_error_when_Id_is_null()
        {
            var editItemRequest = new EditItemRequest { Price = new Money() };
            _validator.ShouldHaveValidationErrorFor(x => x.Id, editItemRequest);
        }

        [Fact]
        public void should_have_error_when_ArtistId_is_null()
        {
            var editItemRequest = new EditItemRequest { Price = new Money() };
            _validator.ShouldHaveValidationErrorFor(x => x.GenreId, editItemRequest);
        }

        [Fact]
        public void should_have_error_when_GenreId_is_null()
        {
            var editItemRequest = new EditItemRequest { Price = new Money() };
            _validator.ShouldHaveValidationErrorFor(x => x.GenreId, editItemRequest);
        }
    }
}