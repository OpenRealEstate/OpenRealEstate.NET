using System;
using FluentValidation.TestHelper;
using OpenRealEstate.Core.Models;
using OpenRealEstate.Validation;
using Xunit;

namespace OpenRealEstate.Tests.Validators
{
    public class LinksValidatorTests
    {
        private readonly LinksValidator _linksValidator;

        public LinksValidatorTests()
        {
            _linksValidator = new LinksValidator();    
        }

        [Theory(Skip = "TODO: Need to figure out how to compile the code, below.")]
        [InlineData("Http://www.SomeDomain.com")]
        [InlineData("https://www.SomeDomain.com")]
        [InlineData("http://www.SomeDomain.com.au")]
        public void GivenAValidUri_Validate_ShouldNotHaveAValidationError(string uri)
        {
            // Arrange.

            // Act & Assert.
            //_linksValidator.ShouldNotHaveValidationErrorFor(l => l, uri);
        }
    }
}