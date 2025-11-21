using UnityEngine;

public class CatAnimationController : MonoBehaviour
{
    public static CatAnimationController Instance;

    private Animator anim;

    private void Awake()
    {
        Instance = this;
        anim = GetComponent<Animator>();
    }


    public void UpdateHearts(int health)
    {
        if (health <= 0)
        {
            anim.SetInteger("HeartState", 0);
            return;
        }

        anim.SetInteger("HeartState", health);
    }


    public void PlayDamageAnimation()
    {
        anim.SetTrigger("TakeDamage");
    }

    public void PlayDeathAnimation()
    {
        anim.SetTrigger("Dead");
        anim.SetInteger("HeartState", 0);
    }
}
