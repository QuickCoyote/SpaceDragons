using UnityEngine;

public class HumanEnemy : Enemy
{
    protected override void Attack()
    {
        //
    }

    protected override void Move()
    {
        target = Player.transform.position;

        direction = target - transform.position;
        angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        rotation = Quaternion.AngleAxis(-angle, Vector3.forward);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        transform.Translate(transform.up * speed * Time.smoothDeltaTime, Space.World);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Health collidedHP = collision.gameObject.GetComponent<Health>();
        if (collidedHP)
        {
            collidedHP.DealDamage(attackDamage);
            GetComponent<Health>().healthCount = 0;
        }
    }
}
