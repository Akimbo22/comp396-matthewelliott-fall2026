using UnityEngine;

public class CanonBall : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                Destroy(other.gameObject);
            }
            if (other.gameObject.CompareTag("Ground"))
            {
                Destroy(gameObject);
            }
        }
}