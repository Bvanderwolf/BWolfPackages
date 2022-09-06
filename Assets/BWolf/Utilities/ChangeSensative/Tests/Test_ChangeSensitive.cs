using NUnit.Framework;

namespace BWolf.ChangeSensitivity.Tests
{
    public class Test_ChangeSensitive
    {
        [Test]
        public void Test_On()
        {
            // Arrange.
            ChangeSensitive<bool> boolean = new ChangeSensitive<bool>(true);
            bool actual = true;
            boolean.On(false, value => actual = value);
            
            // Act.
            boolean = false;
            
            // Assert.
            Assert.AreEqual(boolean.Value, actual);
        }
    }
}
