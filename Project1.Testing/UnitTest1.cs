using System;
using Xunit;
using Project1.Library.Models;

namespace Project1.Testing
{
    public class UnitTest1
    {

        //bool checkUserExists(string fName, string lName)
        [Theory]
        [InlineData("Lance", "Von Ah", true)]
        [InlineData("Lance", "Von Ah69", true)]
        [InlineData("La nce", "Von Ah", true)]
        [InlineData(" Lance", "Von Ah", true)]
        [InlineData("Lance", " Von Ah", true)]
        [InlineData("Lance@", "Von Ah@", true)]
        [InlineData("", "", true)]
        public void IsCheckUserExistsWorking(string fName, string lName, bool actual)
        {
            var test = new Order { Purchaser = new User { FirstName = fName, LastName = lName } };
            bool result = test.checkUserExists(fName, lName);
            Assert.Equal(actual, result);
        }

        [Theory]
        [InlineData("123 Grove St.", true)]
        [InlineData("21 Jump St.", true)]
        [InlineData("221B Baker St.", true)]
        [InlineData("", true)]
        public void IsCheckLocationWorking(string address, bool actual)
        {
            var test = new Order { OrderLocation = new Location { Address = address } };
            bool result = test.checkLocation(address);
            Assert.Equal(actual, result);
        }
    }
}
