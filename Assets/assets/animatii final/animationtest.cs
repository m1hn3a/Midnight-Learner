using UnityEngine;

public class TestAnimatorPlay : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            animator.Play("idle_anim");

        if (Input.GetKeyDown(KeyCode.Alpha2))
            animator.Play("spate");

        if (Input.GetKeyDown(KeyCode.Alpha3))
            animator.Play("fata");

        if (Input.GetKeyDown(KeyCode.Alpha4))
            animator.Play("stangaA");

        if (Input.GetKeyDown(KeyCode.Alpha5))
            animator.Play("dreapta");
    }
}
