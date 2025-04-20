using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedObject : MonoBehaviour
{
    public GameObject HighLight;
    public GameObject HighLight_Small;

    private AnimationManager _HandAnim;
    private GameObject newSelection;
    private bool is_Selection;

    private static bool isMouseHeld = false; // Shared across all objects to track mouse state

    void Start()
    {
        _HandAnim = FindObjectOfType<AnimationManager>();
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isMouseHeld = true; // Mark that the mouse is pressed
            _HandAnim.ClosingWideHand();
            _HandAnim.ClosingHand();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isMouseHeld = false; // Mark that the mouse is released
        }
    }

    private void OnMouseEnter()
    {
        if (isMouseHeld) return; // Don't highlight if mouse is being held

        if (gameObject.name == "Flying_Fire(Clone)" || gameObject.name == "Flying_Leaf(Clone)" || gameObject.name == "Flying_Lighting(Clone)" 
       || gameObject.name == "Flying_Badguy(Clone)" || gameObject.name == "Flying_AssistBall(Clone)")
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
        _HandAnim.OpeningWideHand();
    }

    private void OnMouseExit()
    {
        if (isMouseHeld) return; // Don't un-highlight if mouse is being held

        is_Selection = false;
        if (newSelection != null)
        {
            newSelection.SetActive(false);
            _HandAnim.ClosingWideHand();
        }
    }
}
