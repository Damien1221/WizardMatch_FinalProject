using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedObject : MonoBehaviour
{
    public GameObject HighLight;
    public GameObject HighLight_Small;

    private GameObject newSelection;
    private bool is_Selection;

    private static bool isMouseHeld = false; // Shared across all objects to track mouse state

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isMouseHeld = true; // Mark that the mouse is pressed
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isMouseHeld = false; // Mark that the mouse is released
        }
    }

    private void OnMouseEnter()
    {
        if (isMouseHeld) return; // Don't highlight if mouse is being held

        if (gameObject.name == "Flying_Fire(Clone)" || gameObject.name == "Flying_Leaf(Clone)" || gameObject.name == "Flying_Lighting(Clone)")
        {
            if (newSelection == null)
            {
                newSelection = Instantiate(HighLight, transform.position, Quaternion.identity);
                newSelection.transform.SetParent(gameObject.transform);
                newSelection.SetActive(false);
            }
        }

        if (gameObject.name == "Fly_PowerFire(Clone)" || gameObject.name == "Fly_PowerLeaf(Clone)" || gameObject.name == "Fly_PowerLighting(Clone)") 
        {
            if (newSelection == null)
            {
                newSelection = Instantiate(HighLight_Small, transform.position, Quaternion.identity);
                newSelection.transform.SetParent(gameObject.transform);
                newSelection.SetActive(false);
            }
        }

        is_Selection = true;
        newSelection.SetActive(true);
    }

    private void OnMouseExit()
    {
        if (isMouseHeld) return; // Don't un-highlight if mouse is being held

        is_Selection = false;
        if (newSelection != null)
        {
            newSelection.SetActive(false);
        }
    }
}
