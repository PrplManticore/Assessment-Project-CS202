using UnityEngine;

public class SlashScript : MonoBehaviour
{
    Rigidbody slashRB;
    bool damageDealt = false;
    SkeletonScript PlayerInAttackRange;

    private void Awake()
    {
        slashRB = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (PlayerInAttackRange)
        {
            damageDealt = true;
        }
    }
}
