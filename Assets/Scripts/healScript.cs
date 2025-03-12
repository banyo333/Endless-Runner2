using UnityEngine;

public class healScript : MonoBehaviour
{

    private PlayerScript player;

    void Start()
    {
        player = FindObjectOfType<PlayerScript>();
    }

    // Update is called once per frameW
    void Update()
    {
        
    }
    
    public void OnTriggerEnter2D( Collider2D collision )
    {
        if (collision.CompareTag("Player"))
        { 
            player.characterCurrentHealth += 20;
            Destroy(this.gameObject);
        }
    }
}
