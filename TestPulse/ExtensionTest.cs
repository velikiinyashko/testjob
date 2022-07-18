
namespace TestPulse
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestGetCreateQuery()
        {

            var entity = new pulse.Model.Retail()
            {
                City = "����������",
                Name = "������ �1",
                Address = "������������ ������� 30�",
                Phone = "3912181818"
            };

            var q = pulse.Extension.Extension.GetCreateQuery(entity);

            Assert.AreEqual("INSERT INTO Retail (Name,City,Address,Phone) VALUES (N'������ �1',N'����������',N'������������ ������� 30�',N'3912181818')", q);
        }

        [TestCase("3.5")]
        [TestCase("3,5")]
        [TestCase("35")]
        [TestCase("3we")]
        public void TestToDecimal(string value)
        {
            var i = value.ToDecimal();
            Assert.Pass(i.ToString());

        }
    }
}