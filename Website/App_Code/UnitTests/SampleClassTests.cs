using NUnit.Framework;

namespace ASP.App_Code.UnitTests
{
    [TestFixture]
    public class SampleClassTests
    {
        [Test]
        public void ShouldReturnTrue()
        {
            ISampleClass sampleClass = new SampleClass();
            Assert.IsTrue(sampleClass.DoesThisContainThat("The cat in the hat.", "cat"));
        }

        [Test]
        public void ShouldReturnFalse()
        {
            ISampleClass sampleClass = new SampleClass();
            Assert.IsFalse(sampleClass.DoesThisContainThat("The cat in the hat.", "dog"));
        }

        [Test]
        public void ShouldNotPassForDemonstrationPurposes()
        {
            ISampleClass sampleClass = new SampleClass();
            Assert.IsNotNull(sampleClass.GetNull(), "This error is on purpose to demonstrate what a failing test looks like.");
        }
    }
}