using com.scprojekt.port.util;

namespace com.scprojekt.port.util
{
    [TestClass]
    public class OptionalTest
    {
        [TestMethod]
        public void IfOptionalIsCreatedEmptyItShouldBeEmpty()
        {
            var stringOptional = Optional<string>.Empty();
            
            Assert.IsNotNull(stringOptional);
            Assert.AreEqual(stringOptional, Optional<string>.Empty());

            stringOptional = Optional<string>.Of("String");

            Assert.IsNotNull(stringOptional);
            Assert.AreEqual(stringOptional.Get(), "String");
        }

        [TestMethod]
        public void IfOptionalIsCreatedWithOfItContainsAValue()
        {

            var stringOptional = Optional<string>.Of("String");

            Assert.IsNotNull(stringOptional.Get());
            Assert.AreEqual(stringOptional.Get(), "String");
        }

        [TestMethod]
        public void IfOptionalIsCreatedWithOfNullableItContainsAValue()
        {

            var stringOptional = Optional<string>.OfNullable("String");

            Assert.IsNotNull(stringOptional.Get());
            Assert.AreEqual(stringOptional.Get(), "String");

            string? nullString = null;
            var nullOptional = Optional<string>.OfNullable(nullString);

            Assert.IsNotNull(nullOptional);
            Assert.IsTrue(nullOptional.IsEmpty());
        }

        [TestMethod]
        public void IfOptionalIsCreatedWithOrElseItContainsAValue()
        {
            string? nullString = null;
            var stringOptional = Optional<string>.OfNullable(nullString).OrElse("Auto");

            Assert.IsNotNull(stringOptional);
            Assert.AreEqual(stringOptional, "Auto");
        }
    }
}