using UnityEngine;

public class CameraCombatSansInclinaison : MonoBehaviour
{
    [Header("Cibles")]
    public Transform joueur1;
    public Transform joueur2;
    
    [Header("Zoom")]
    public float orthoSizeMin = 5f;
    public float orthoSizeMax = 20f;
    public float distanceMinJoueurs = 5f;
    public float distanceMaxJoueurs = 30f;
    
    [Header("Lissage")]
    public float tempsDeLissagePosition = 0.25f;
    public float tempsDeLissageSize = 0.15f;
    
    [Header("Position")]
    public float hauteurOffset = 0f;
    public float profondeurZ = -10f;
    
    private Camera cam;
    private Vector3 velocityPosition;
    private float velocitySize;
    
    void Start()
    {
        cam = GetComponent<Camera>();
        cam.orthographic = true;
        
        // ===== FIXER LA ROTATION DÈS LE DÉPART =====
        transform.rotation = Quaternion.Euler(0, 0, 0); // Caméra droite
    }
    
    void LateUpdate()
    {
        if (joueur1 == null || joueur2 == null) return;
        
        Vector3 pointMilieu = (joueur1.position + joueur2.position) / 2f;
        float distance = Vector3.Distance(joueur1.position, joueur2.position);
        float ratio = Mathf.InverseLerp(distanceMinJoueurs, distanceMaxJoueurs, distance);
        float tailleDesired = Mathf.Lerp(orthoSizeMin, orthoSizeMax, ratio);
        
        Vector3 positionCible = new Vector3(pointMilieu.x, pointMilieu.y + hauteurOffset, profondeurZ);
        
        // Position
        transform.position = Vector3.SmoothDamp(transform.position, positionCible, ref velocityPosition, tempsDeLissagePosition);
        
        // Size
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, tailleDesired, ref velocitySize, tempsDeLissageSize);
        
        // ===== FORCER LA ROTATION À RESTER FIXE =====
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}