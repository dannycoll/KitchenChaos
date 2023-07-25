using System;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
  [SerializeField] private GameObject counterGameObject;
  [SerializeField] private Image barImage;

  private IHasProgress _counter;
  private void Start()
  {
    _counter = counterGameObject.GetComponent<IHasProgress>();
    _counter.OnProgressChanged += Counter_OnProgressChanged;
    barImage.fillAmount = 0f;
    Hide();
  }

  private void Counter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
  {
    barImage.fillAmount = e.ProgressNormalized;
    if (e.ProgressNormalized == 0f || Math.Abs(e.ProgressNormalized - 1f) < 0.001f)
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
