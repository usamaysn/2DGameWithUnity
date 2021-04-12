using NUnit.Framework;
using Altom.AltUnityDriver;

public class LoadStartScene
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

    [SetUp]
    public void LoadLevel()
    {
        AltUnityDriver.LoadScene("StartScreen", true);
    }

    [Test]
    public void Test_Get_Current_Scene()
    {
        Assert.AreEqual("StartScreen", AltUnityDriver.GetCurrentScene());
    }

    [Test]
    public void Play_Button_Click_To_Load_Megaman_Scene()
    {
        AltUnityDriver.FindObject(By.NAME, "PlayButton").ClickEvent();
        AltUnityDriver.WaitForCurrentSceneToBe("Megaman");
        Assert.AreEqual("Megaman", AltUnityDriver.GetCurrentScene());
    }
}