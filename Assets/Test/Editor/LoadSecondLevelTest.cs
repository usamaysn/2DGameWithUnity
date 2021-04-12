using NUnit.Framework;
using Altom.AltUnityDriver;

public class LoadSecondLevelTest
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
    public void Load_Second_Level_On_Obstacle_Collide()
    {
        var player = AltUnityDriver.FindObject(By.NAME, "Player");
        const string componentName = "Move2D";
        const string proeprtyName = "isObstacleColliderForLevelCompleted";
        var isLevelComplete = player.GetComponentProperty(componentName, proeprtyName);
        Assert.AreEqual(isLevelComplete, "true");
    }

}