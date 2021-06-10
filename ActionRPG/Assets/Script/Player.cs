using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public int curHp;
    public int maxHp;

    public float moveSpeed;
    public float jumpForce;

    public float attackRange;
    public int damage;
    private bool isAttacking;

    public Rigidbody rig;
    public Animator anim;

    private void Update()
    {
        if(!isAttacking)
        Move();

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        if (Input.GetMouseButton(0) && !isAttacking)
            Attack();

        if (!isAttacking)
            UpdateAnimator();
           
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 dir = transform.right * x + transform.forward * z;
        dir *= moveSpeed;
        dir.y = rig.velocity.y;

        rig.velocity = dir;
    }

    void UpdateAnimator()
    {
        anim.SetBool("MovingForward", false);
        anim.SetBool("MovingBackwards", false);
        anim.SetBool("MovingLeft", false);
        anim.SetBool("MovingRight", false);

        Vector3 localVel = transform.InverseTransformDirection(rig.velocity);   //finding out the direction in which the player is moving locally

        if (localVel.z > 0.1f)
            anim.SetBool("MovingForward", true);
        else if (localVel.z < -0.1f)
            anim.SetBool("MovingBackwards", true);
        else if (localVel.x > 0.1f)
            anim.SetBool("MovingRight", true);
        else if (localVel.x < -0.1f)
            anim.SetBool("MovingLeft", true);
    }

    void Jump()
    {
        if (CanJump())
        {
            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    bool CanJump()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if(Physics.Raycast(ray,out hit, 0.1f))
        {
            return hit.collider != null;
        }
        return false;
    }

    public void TakeDamage(int damageToTake)
    {
        curHp -= damageToTake;

        //update the UI health bar

        if (curHp <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); ;
        }

    }

    void Attack()
    {
        isAttacking = true;
        anim.SetTrigger("Attack");

        Invoke("TryDamage", 0.7f);
        Invoke("DisableIsAttacking", 1.5f);
    }

    void TryDamage()
    {
        Ray ray = new Ray(transform.position + transform.forward, transform.forward);
        RaycastHit[] hits = Physics.SphereCastAll(ray, attackRange, 1 << 7);

        foreach(RaycastHit hit in hits)
        {
            hit.collider.GetComponent<Enemy>()?.TakeDamage(damage);
        }
    
    }

    void DisableIsAttacking()
    {
        isAttacking = false;
    }
}
