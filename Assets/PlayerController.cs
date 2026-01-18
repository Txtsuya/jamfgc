using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    public float speed = 5f;
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        // Détecter les inputs de déplacement
        float horizontal = Input.GetAxisRaw("Horizontal"); // A/D ou Flèches
        
        // Si le joueur appuie sur une touche de déplacement
        bool isWalking = horizontal != 0;
        
        // Mettre à jour l'animator
        animator.SetBool("isWalking", isWalking);
        
        // Déplacer le personnage
        if (isWalking)
        {
            transform.Translate(Vector3.right * horizontal * speed * Time.deltaTime);
        }
    }
}
