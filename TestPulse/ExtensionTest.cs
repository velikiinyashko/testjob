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
                City = "Красноярск",
                Name = "Аптека №1",
                Address = "Красноярский рабочий 30а",
                Phone = "3912181818"
            };           

            var q = pulse.Extension.Extension.GetCreateQuery(entity);

            Assert.AreEqual(q, "INSERT INTO Retail (Name,City,Address,Phone) VALUES (N\"Аптека №1\",N\"Красноярск\",N\"Красноярский рабочий 30а\",N\"3912181818\")");
        }
    }
}