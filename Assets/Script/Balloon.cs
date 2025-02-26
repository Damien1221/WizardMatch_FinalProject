using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Balloon : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; // Reference to the sprite renderer
    public string shapeName; // Name of the shape
    public Sprite[] shapeSprites; // Array to hold shape sprites (set in Inspector)

    public void SetShape(string shape)
    {
        shapeName = shape;
        Sprite newSprite = GetShapeSprite(shape);
        if (newSprite != null)
        {
            spriteRenderer.sprite = newSprite; // Change the sprite to match the shape
        }
    }

    private Sprite GetShapeSprite(string shape)
    {
        foreach (Sprite sprite in shapeSprites)
        {
            if (sprite.name.ToLower() == shape.ToLower()) // Match by name
            {
                return sprite;
            }
        }
        return null;
    }
}
