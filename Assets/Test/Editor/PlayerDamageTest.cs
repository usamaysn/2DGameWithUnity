using NUnit.Framework;
using Altom.AltUnityDriver;
using System.Threading;

public class PlayerDamageTest
{
    public AltUnityDriver AltUnityDriver;
    //Before any test it connects with the socket
    [OneTimeSetUp]
    public void SetUp()
    {
        AltUnityDriver =new AltUnityDriver();
    }

    //At the end of the test closes the connection with the socket
    [OneTimeTearDown]
    public void TearDown()
    {
        AltUnityDriver.Stop();
    }

    [Test]
    public void Player_Damage_Test()
    {
        var player = AltUnityDriver.FindObject(By.NAME, "Player");
        const string componentName = "Move2D";
        const string proeprtyName = "Health";
        var health = player.GetComponentProperty(componentName, proeprtyName);
        Thread.Sleep(2000);
        Assert.AreNotEqual(health, "100");
    }

}