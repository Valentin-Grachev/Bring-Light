using System.Collections.Generic;
using UnityEngine;

public class Player : Creature
{

    private bool right = true;                      // ������� �� ����� �������
    private CheckParameters checkParameters;        // ������� �������� ����������
    private Inventory inventory;                    // ���������
    public List<Animator> animat;

    protected override void Start()
    {
        base.Start();
        animAgent = GetComponent<AnimatorAgent>();
        rb = GetComponent<Rigidbody2D>();
        checkParameters = GameObject.FindWithTag("Script").GetComponent<CheckParameters>();
        inventory = GameObject.FindWithTag("Script").GetComponent<Inventory>();
    }

    protected override void Update()
    {
        if (!Move() && !stunned) rb.velocity = Vector2.zero; // ���� ����� �� ��������� � �� �������, �� �� ���������������
        //CheckFace();
    }



    private bool Move()
    {
        if (currentTimeStunning > 0) return false;  
        float InputX = Input.GetAxisRaw("Horizontal");
        float InputY = Input.GetAxisRaw("Vertical");
        
        
        if (InputX == 0 && InputY == 0)
        {
            foreach (var item in animat)
            {
                item.SetBool("Walk", false);
            }
            return false;
        }
        foreach (var item in animat)
        {
            item.SetFloat("HorizontalMovement", InputX);
            item.SetFloat("VerticalMovement", InputY);
        }

        Vector2 moveDirection = new Vector2(InputX, InputY).normalized;
        rb.velocity = moveDirection * speed;
        foreach (var item in animat)
        {
            item.SetBool("Walk", true);
        }

        return true;
    }


    private void Flip() // ������� ������ � ������ �������
    {
        right = !right;
        Vector2 scale = new Vector2(transform.localScale.x, transform.localScale.y);
        scale.x *= -1;
        transform.localScale = scale;
    }
    private void CheckFace() // �������� � ����� ������� ������� �����
    {
        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x < 0 && right) Flip();
        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x > 0 && !right) Flip();
    }

    public override void Death()
    {
        base.Death();
        Weapon weapon = GetComponentInChildren<Weapon>();
        if (weapon != null) weapon.enabled = false;
        inventory.enabled = false;
        rb.velocity = Vector2.zero;
        enabled = false;

    }

    public override void GetDamage(AttackParameters attack, Transform attacking, Transform bullet = null)
    {
        // � ������-������ ���������� ����� ��������� ������� � �� - ������� ��������� ��� �� ���, �� ��� ������ ������� � ��
        int realDamage = GetRealDamage(attack); // ������� ��������� �����
        health -= realDamage;

        if (bullet != null) PushBack(attack.pushForce, bullet, attack.timeStunning); // ���� ���� �� ������� - ������ �� �������
        else PushBack(attack.pushForce, attacking, attack.timeStunning);            // ���� ���������� ���� - ������ �� ����������

        if (health <= 0 && !death) // �������� ���� ��� ����� 0 - �������� �������
        {
            health = 0;
            Death();
        }
        if (!death) animAgent.anim.SetTrigger("GetDamage"); // ���� �������� �� ������, �� ����������� �������� ��������� �����
        checkParameters.UpdateParameters();     // ��������� ��������� � UI ��� ��������� �����
    }
}
