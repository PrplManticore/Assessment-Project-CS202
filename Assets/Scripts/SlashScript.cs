using UnityEngine;

public class SlashScript : MonoBehaviour
{
    Rigidbody slashRB;

    private void Awake()
    {
        slashRB = GetComponent<Rigidbody>();
    }
}
