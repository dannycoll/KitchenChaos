using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
  [SerializeField] private GameObject counterGameObject;
  [SerializeField] private Image barImage;

  private IHasProgress counter;
  private void Start()
  {
    counter = counterGameObject.GetComponent<IHasProgress>();
    counter.OnProgressChanged += Counter_OnProgressChanged;
    barImage.fillAmount = 0f;
    Hide();
  }

  private void Counter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
  {
    barImage.fillAmount = e.progressNormalized;
    if (e.progressNormalized == 0f || e.progressNormalized == 1f)
    {
      Hide();
    }
    else
    {
      Show();
    }
  }

  private void Show()
  {
    gameObject.SetActive(true);
  }
  private void Hide()
  {
    gameObject.SetActive(false);
  }
}
