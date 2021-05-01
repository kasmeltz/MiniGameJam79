using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using KasJam.MiniJam79.Unity.Behaviours;

[RequireComponent(typeof(TMP_Text))]
public class FlyCounter : MonoBehaviour
{
    [SerializeField] private PlatformerBehaviour _frog;

    private TMP_Text _flyText;

    private void Awake()
    {
        _flyText = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        _frog.FliesEatenHasChanged += OnFliesEatenHasChanged;
    }

    private void OnDisable()
    {
        _frog.FliesEatenHasChanged -= OnFliesEatenHasChanged;
    }

    private void OnFliesEatenHasChanged(int fliesEaten)
    {
        _flyText.text = fliesEaten.ToString();
    }
}
