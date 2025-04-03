using System;
using System.Collections;
using UnityEngine;

namespace MyVampireSurvivors
{
    public class Enemy : MonoBehaviour
    {
        #region Variables
        // ���� �̵� �ӵ�
        public float speed;

        // �ִϸ��̼� ��Ʈ�ѷ� �迭 (���� ��������Ʈ Ÿ�Կ� ���� �ٸ��� ����)
        public RuntimeAnimatorController[] animatorControllers;

        // ���� ü�°� �ִ� ü��
        public float health;
        public float maxHealth;

        // ���� ������ Ÿ�� (�÷��̾�)
        public Rigidbody2D target;

        // ���� ����ִ��� ���θ� ��Ÿ���� ����
        bool isLive;

        // ������ �̵��� ���� Rigidbody2D ������Ʈ
        Rigidbody2D rb2d;

        Collider2D collider2d;

        // �ִϸ��̼��� ���� Animator ������Ʈ
        Animator animator;

        // ��������Ʈ �������� ���� SpriteRenderer ������Ʈ
        SpriteRenderer spriter;

        WaitForFixedUpdate wait;
        #endregion

        // �ʱ�ȭ �۾�: ������Ʈ���� ��������
        private void Awake()
        {
            // Rigidbody2D ������Ʈ�� ������ (���� ������ �̵��� ���)
            rb2d = GetComponent<Rigidbody2D>();
            collider2d = GetComponent<Collider2D>();
            // SpriteRenderer ������Ʈ�� ������ (���� ��������Ʈ �������� ���)
            spriter = GetComponent<SpriteRenderer>();
            // Animator ������Ʈ�� ������ (�ִϸ��̼� ��� ���)
            animator = GetComponent<Animator>();

            wait = new WaitForFixedUpdate();
        }

        // ������ ������Ʈ: �� FixedUpdate() ȣ�� �� ���� �̵��� ó��
        private void FixedUpdate()
        {
            // ���� ������� ������ �̵��� ó������ ����
            if (!isLive || animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
                return;

            // Ÿ��(�÷��̾�)�� �� ���� ���� ���� ���
            Vector2 dirVec = target.position - rb2d.position;

            // ���� ���͸� ����ȭ�ϰ�, �ӵ��� �ð��� ��Ÿ ���� ���Ͽ� �̵��� �Ÿ� ���
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;

            // Rigidbody2D�� MovePosition �޼��带 ����� ���������� ���� �̵���Ŵ
            rb2d.MovePosition(rb2d.position + nextVec);

            // �ӵ� ���� (���� �ӵ� �� �ʱ�ȭ)
            rb2d.linearVelocity = Vector2.zero;
        }

        // ��ó�� ������Ʈ: �ִϸ��̼��̳� ���� ��ȯ�� ó��
        private void LateUpdate()
        {
            // ���� ������� ������ ���� ��ȯ�� ó������ ����
            if (!isLive)
                return;

            // Ÿ��(�÷��̾�)�� X ��ǥ�� ���� X ��ǥ���� ������ ��������Ʈ�� ������
            // �̴� ���� �÷��̾ �ٶ󺸴� ������ �ݿ��ϱ� ���� ó��
            spriter.flipX = target.position.x < rb2d.position.x;
        }

        // ���� Ȱ��ȭ�� �� ȣ��Ǵ� �޼���
        private void OnEnable()
        {
            // ���� Ÿ���� �÷��̾�� ����
            target = GameManager.instance.player.GetComponent<Rigidbody2D>();

            // ���� ü���� �ִ� ü������ ����
            health = maxHealth;

            // ���� Ȱ��ȭ�� �� ����ִ� ���·� �ʱ�ȭ
            isLive = true;
            collider2d.enabled = true;
            rb2d.simulated = true;
            spriter.sortingOrder = 2;
            animator.SetBool("Dead", false);
        }

        // �� �ʱ�ȭ �޼���: SpawnData�� ���� ���� ���� �ʱ�ȭ
        public void Init(SpawnData spawnData)
        {
            // �ִϸ��̼� ��Ʈ�ѷ��� ���� �����Ϳ� �°� ����
            animator.runtimeAnimatorController = animatorControllers[spawnData.spriteType];
            // �̵� �ӵ��� ü���� ���� �����Ϳ� �°� ����
            speed = spawnData.speed;
            maxHealth = spawnData.health;
            health = spawnData.health;
        }

        // �浹 ó��: �Ѿ˰� �浹 �� ó��
        public void OnTriggerEnter2D(Collider2D collision)
        {
            // �浹�� ������Ʈ�� "Bullet" �±׸� ������ ������ ó������ ����
            if (!collision.CompareTag("Bullet") || !isLive)
                return;

            // �Ѿ��� ���ظ� �޾� ü�� ����
            health -= collision.GetComponent<Bullet>().damage;

            StartCoroutine(KnockBack());

            // ü���� 0 ���ϰ� �Ǹ� ���� ó��
            if (health <= 0)
            {
                // ���� ���·� ��ȯ
                isLive = false;
                collider2d.enabled = false;
                rb2d.simulated = false;
                spriter.sortingOrder = 1;
                animator.SetBool("Dead", true);
                GameManager.instance.kill++;
                GameManager.instance.GetExp();
            }
            else
            {
                animator.SetTrigger("Hit");
            }
        }

        IEnumerator KnockBack()
        {
            //���� ���� �����ӱ��� ������
            yield return wait;

            Vector3 playerPos = GameManager.instance.player.transform.position;
            Vector3 dirVec = transform.position - playerPos;
            rb2d.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);

        }

        // ���� �׾��� �� ȣ��Ǵ� �Լ�
        private void Dead()
        {
            // ���� ���� ������Ʈ�� ��Ȱ��ȭ
            gameObject.SetActive(false);

        }
    }
}
