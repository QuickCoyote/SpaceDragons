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
        Vector3 direction = target - transform.position;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        transform.Translate(transform.up * speed * Time.smoothDeltaTime, Space.World);
    }

    private void Start()
    {
        base.Start();
        hp = GetComponent<Health>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Health collidedHP = collision.gameObject.GetComponent<Health>();
        if (collidedHP)
        {
            collidedHP.DealDamage(attackDamage);
            hp.healthCount = 0;
        }
    }
}
