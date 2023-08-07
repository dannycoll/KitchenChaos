using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerSingleUITests
{
    private TestUI   _deliveryManager;
    private RecipeSO _testRecipe;

    private class TestUI : DeliveryManagerSingleUI
    {
        public Transform IconTemplate
        {
            set => iconTemplate = value;
        }
        public Transform IconContainer
        {
            get => iconContainer;
            set => iconContainer = value;
        }

        public TextMeshProUGUI RecipeNameText
        {
            get => recipeNameText;
            set => recipeNameText = value;
        }
    }

    [SetUp]
    public void SetUp()
    {
        GameObject iconTemplateGO = new GameObject();
        iconTemplateGO.AddComponent<RectTransform>();
        iconTemplateGO.AddComponent<Image>();
        GameObject go = new GameObject();
        go.SetActive(false);
        go.AddComponent<TestUI>();
        go.GetComponent<TestUI>().IconTemplate = iconTemplateGO.transform;
        _deliveryManager = go.GetComponent<TestUI>();
        _deliveryManager.gameObject.SetActive(true);
        _deliveryManager.IconTemplate = iconTemplateGO.transform;

        // Create a temporary icon container and assign it to the DeliveryManagerSingleUI
        GameObject iconContainerGO = new GameObject("IconContainer");
        iconContainerGO.AddComponent<RectTransform>();
        iconTemplateGO.transform.parent = iconContainerGO.transform;
        _deliveryManager.IconContainer = iconContainerGO.transform;

        // Create a temporary TextMeshProUGUI and assign it to the DeliveryManagerSingleUI
        GameObject recipeNameTextGO = new GameObject("RecipeNameText");
        recipeNameTextGO.AddComponent<RectTransform>();
        _deliveryManager.RecipeNameText = recipeNameTextGO.AddComponent<TextMeshProUGUI>();

        // Create a test RecipeSO with some kitchenObjectSOList
        _testRecipe = ScriptableObject.CreateInstance<RecipeSO>();
        _testRecipe.name = "Test Recipe";

        // Add a few KitchenObjectSO objects to the recipe
        _testRecipe.kitchenObjectSOList = new List<KitchenObjectSO>();
        for (int i = 0; i < 3; i++)
        {
            _testRecipe.kitchenObjectSOList.Add(ScriptableObject.CreateInstance<KitchenObjectSO>());
            _testRecipe.kitchenObjectSOList[i].sprite = null; // You can assign a sprite if needed
        }
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up the test objects
        Object.Destroy(_deliveryManager.gameObject);
        Object.Destroy(_testRecipe);
    }

    [Test]
    public void SetRecipeSO_Should_SetRecipeNameText()
    {
        // Arrange
        string recipeName = "Test Recipe";

        // Act
        _deliveryManager.SetRecipeSO(_testRecipe);

        // Assert
        Assert.AreEqual(recipeName, _deliveryManager.RecipeNameText.text);
    }

    [Test]
    public void SetRecipeSO_Should_InstantiateIcons()
    {
        // Act
        _deliveryManager.SetRecipeSO(_testRecipe);

        // Assert
        Assert.AreEqual(3, _deliveryManager.IconContainer.childCount - 1);
    }

    [Test]
    public void SetRecipeSO_Should_SetIconSprites()
    {
        // Act
        _deliveryManager.SetRecipeSO(_testRecipe);

        // Assert
        for (int i = 0; i < _testRecipe.kitchenObjectSOList.Count; i++)
        {
            Sprite expectedSprite = _testRecipe.kitchenObjectSOList[i].sprite;
            Transform iconTransform = _deliveryManager.IconContainer.GetChild(i);
            Image iconImage = iconTransform.GetComponent<Image>();
            Assert.AreEqual(expectedSprite, iconImage.sprite);
        }
    }
}
