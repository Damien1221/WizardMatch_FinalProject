using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Balloon : MonoBehaviour
{
    public string shapeName;

    [SerializeField] private TextMeshProUGUI textMesh;

    void Start()
    {
        AssignRandomShape();
    }

    void AssignRandomShape()
    {
        string[] shapes = { "Circle", "Square", "Triangle", "ArrowUp" };
        shapeName = shapes[Random.Range(0, shapes.Length)];

        if (textMesh != null)
        {
            textMesh.text = shapeName; // Display shape name on balloon
        }
    }
}
