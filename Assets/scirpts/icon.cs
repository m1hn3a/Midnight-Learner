using UnityEngine;

public class InteractionZoneHandler : MonoBehaviour
{
    public GameObject prompt1;
    public GameObject prompt2;
    public GameObject prompt3;

    public Collider2D zone1;
    public Collider2D zone2;
    public Collider2D zone3;

    private bool inZone1 = false;
    private bool inZone2 = false;
    private bool inZone3 = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == zone1)
        {
            inZone1 = true;
            UpdatePrompts();
        }
        else if (other == zone2)
        {
            inZone2 = true;
            UpdatePrompts();
        }
        else if (other == zone3)
        {
            inZone3 = true;
            UpdatePrompts();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other == zone1)
        {
            inZone1 = false;
            UpdatePrompts();
        }
        else if (other == zone2)
        {
            inZone2 = false;
            UpdatePrompts();
        }
        else if (other == zone3)
        {
            inZone3 = false;
            UpdatePrompts();
        }
    }

    private void UpdatePrompts()
    {
        if (inZone1)
        {
            prompt1.SetActive(true);
            prompt2.SetActive(false);
            prompt3.SetActive(false);
        }
        else if (inZone2)
        {
            prompt1.SetActive(false);
            prompt2.SetActive(true);
            prompt3.SetActive(false);
        }
        else if (inZone3)
        {
            prompt1.SetActive(false);
            prompt2.SetActive(false);
            prompt3.SetActive(true);
        }
        else
        {
            prompt1.SetActive(false);
            prompt2.SetActive(false);
            prompt3.SetActive(false);
        }
    }
}
