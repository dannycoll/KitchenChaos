using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.UI;

public class PlateIconsUITests
{
    private GameObject       _testGameObject;
    private TestPlateIconsUI _plateIconsUI;

    private class TestPlateIconsUI : PlateIconsUI
    {
        public PlateKitchenObject PlateKitchenObject
        {
            set => plateKitchenObject = value;
        }

        public Transform IconTemplate
        {
            set => iconTemplate = value;
        }
    }

    private class TestPlateSingleIconUI : PlateSingleIconUI
    {
        public Image Image
        {
            set => image = value;
        }
    }

    [SetUp]
    public void Setup()
    {
        _testGameObject = new GameObject();
        _testGameObject.gameObject.SetActive(false);
        
        _plateIconsUI = _testGameObject.AddComponent<TestPlateIconsUI>();
        var mockIconTemplate = new GameObject();
        mockIconTemplate.AddComponent<TestPlateSingleIconUI>();
        mockIconTemplate.GetComponent<TestPlateSingleIconUI>().Image = new GameObject().AddComponent<Image>();
        _plateIconsUI.IconTemplate = mockIconTemplate.transform;
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(_testGameObject);
    }

    [Test]
    public void UpdateVisual_DestroysPreviousIconsAndCreatesNewOnes()
    {
        // Set up a mock PlateKitchenObject and KitchenObjectSOs
        PlateKitchenObject mockPlateKitchenObject = new GameObject().AddComponent<PlateKitchenObject>();
        List<KitchenObjectSO> mockKitchenObjectSOs = new List<KitchenObjectSO>
                                                     {
                                                         ScriptableObject.CreateInstance<KitchenObjectSO>(),
                                                         ScriptableObject.CreateInstance<KitchenObjectSO>()
                                                     };
        mockPlateKitchenObject.SetKitchenObjectSOs(mockKitchenObjectSOs);
        _plateIconsUI.PlateKitchenObject = mockPlateKitchenObject;

        // Call UpdateVisual
        _plateIconsUI.UpdateVisual();

        var childCount = _plateIconsUI.transform.childCount;
        // Check if new icons are created
        Assert.AreEqual(2, childCount, "New icons should have been created");
    }
}
