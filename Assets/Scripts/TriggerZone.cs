using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    private EnemyBarrelScript parentBarrel;

    private void Awake()
    {
        parentBarrel = GetComponentInParent<EnemyBarrelScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (gameObject.name == "WakeZone")
            {
                parentBarrel.OnWakeZoneEnter();
            }
            else if (gameObject.name == "AttackZone")
            {
                parentBarrel.OnAttackZoneEnter();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (gameObject.name == "WakeZone")
            {
                parentBarrel.OnWakeZoneExit();
            }
            else if (gameObject.name == "AttackZone")
            {
                parentBarrel.OnAttackZoneExit();
            }
        }
    }
} 