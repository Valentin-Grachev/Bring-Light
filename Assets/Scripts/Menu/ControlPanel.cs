using UnityEngine;

public class ControlPanel : MonoBehaviour
{
    public static ControlPanel singleton { get; private set; }

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Open()
    {
        anim.SetBool("Open", true);
    }

    public void Close()
    {
        anim.SetBool("Open", false);
    }


    private void Awake()
    {
        singleton = this;
    }
}
