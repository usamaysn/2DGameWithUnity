using NUnit.Framework;
using Altom.AltUnityDriver;
using System.Threading;

public class PlayerMovementTest
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
    public void Player_Movement_Editor_Test_Level1()
    {


        var player = AltUnityDriver.FindObject(By.NAME, "Player");
        AltUnityVector3 cubeInitialPostion = new AltUnityVector3(player.worldX, player.worldY, player.worldZ);

        AltUnityDriver.PressKey(AltUnityKeyCode.D, 1, 2);
        AltUnityDriver.PressKey(AltUnityKeyCode.RightArrow, 1, 2);

        AltUnityDriver.PressKey(AltUnityKeyCode.A, 1, 2);
        AltUnityDriver.PressKey(AltUnityKeyCode.LeftArrow, 1, 2);

        AltUnityDriver.PressKey(AltUnityKeyCode.Space, 1, 2);

        Thread.Sleep(2000);

        player = AltUnityDriver.FindObject(By.NAME, "Player");
        AltUnityVector3 cubeFinalPosition = new AltUnityVector3(player.worldX, player.worldY, player.worldZ);

        Assert.AreNotEqual(cubeInitialPostion, cubeFinalPosition);
    }

}