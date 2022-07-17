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

            Assert.AreEqual(q, "INSERT INTO Retail (Name,City,Address,Phone) VALUES (N\"������ �1\",N\"����������\",N\"������������ ������� 30�\",N\"3912181818\")");
        }
    }
}