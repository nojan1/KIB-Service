using KIB_Service.Models.dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace KIB_Service.Tests.Models
{
    public class TournamentDtoValidationTests
    {
        [Theory]
        [InlineData("", "", false)]
        [InlineData("Name", "", false)]
        [InlineData("", "2016-11-23 18:00" , false)]
        [InlineData("Name", "2016-11-23 18:00", true)]
        public void ModelValidatesCorrectly(string name, string eventDateString, bool shouldBeValid)
        {
            DateTimeOffset eventDate;
            
            var model = new TournamentDto
            {
                Name = name,
                EventDate = DateTimeOffset.TryParse(eventDateString, out eventDate) ? new DateTimeOffset?(eventDate) : null
            };

            var context = new ValidationContext(model, null, null);
            var result = new List<ValidationResult>();

            var valid = Validator.TryValidateObject(model, context, result, true);

            Assert.Equal(shouldBeValid, valid);
        }
    }
}
