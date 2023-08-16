using System.Collections;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class LoaderTests
{
    [UnityTest]
    public IEnumerator Load_SetsTargetSceneAndLoadsLoadingScene()
    {
        Loader.Load(Loader.Scene.GameScene);

        // Wait for the loading scene to be fully loaded
        yield return new WaitForSeconds(0.001f);

        Assert.AreEqual(Loader.Scene.GameScene, Loader.GetTargetScene());
        Assert.AreEqual(Loader.Scene.LoadingScene.ToString(), SceneManager.GetActiveScene().name);
    }

    [UnityTest]
    public IEnumerator LoaderCallback_LoadsTargetScene()
    {
        Loader.Scene targetScene = Loader.Scene.MainMenuScene;
        Loader.Load(targetScene);

        yield return new WaitForSeconds(0.1f); // Wait for scene to load

        Loader.LoaderCallback();

        yield return new WaitForSeconds(0.1f); // Wait for scene to load

        Assert.AreEqual(targetScene.ToString(), SceneManager.GetActiveScene().name);
    }
}