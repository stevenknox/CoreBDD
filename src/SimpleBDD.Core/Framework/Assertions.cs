using Xunit;

namespace SimpleBDD
{
    public static class Assertions
    {
        public static void ShouldBe(this object expected, object actual)
        {
            Assert.Equal(expected, actual);
        }
    }
}