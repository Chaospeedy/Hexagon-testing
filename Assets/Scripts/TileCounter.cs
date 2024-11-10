using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TileCounter : MonoBehaviour
{
    [Header("Dynamic")]
    public int tilesRemaining = 50;

    private TextMeshPro uiText;
    // Start is called before the first frame update
    void Start()
    {
        uiText = GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        uiText.text = "Tiles Remaining: " + tilesRemaining;
    }
}
