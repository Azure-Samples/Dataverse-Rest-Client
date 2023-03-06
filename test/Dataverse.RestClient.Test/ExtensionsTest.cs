namespace Dataverse.RestClient.Test
{
    public class ExtensionsTest
    {
        [Fact]
        public void IsJSON_Should_Return_True_For_Valid_JSON()
        {
            @"{ ""a"": 4}".IsJSON().Should().BeTrue();
        }

        [Fact]
        public void IsJSON_Should_Return_False_For_Valid_JSON()
        {
            @"{ ""a"": 4}}".IsJSON().Should().BeFalse();
        }
    }
}