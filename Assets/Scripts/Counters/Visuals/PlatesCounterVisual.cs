using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{

  [SerializeField] private PlatesCounter platesCounter;
  [SerializeField] private Transform counterTopPoint;
  [SerializeField] private Transform platePrefab;

  private List<GameObject> _plates;

  private void Awake()
  {
    _plates = new List<GameObject>();
  }
  private void Start()
  {
    platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned;
    platesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved;
  }

  private void PlatesCounter_OnPlateSpawned(object sender, System.EventArgs e)
  {
    Transform plateVisual = Instantiate(platePrefab, counterTopPoint);

    float plateOffsetY = .1f;
    plateVisual.localPosition = new Vector3(0, plateOffsetY * _plates.Count, 0);
    _plates.Add(plateVisual.gameObject);
  }

  private void PlatesCounter_OnPlateRemoved(object sender, System.EventArgs e)
  {
    GameObject plate = _plates[_plates.Count - 1];
    _plates.Remove(plate);
    Destroy(plate);
  }
}
