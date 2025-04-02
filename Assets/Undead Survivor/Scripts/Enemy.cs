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

        // �ִϸ��̼��� ���� Animator ������Ʈ
        Animator animator;

        // ��������Ʈ �������� ���� SpriteRenderer ������Ʈ
        SpriteRenderer spriter;
        #endregion

        // �ʱ�ȭ �۾�: ������Ʈ���� ��������
        private void Awake()
        {
            // Rigidbody2D ������Ʈ�� ������
            rb2d = GetComponent<Rigidbody2D>();
            // SpriteRenderer ������Ʈ�� ������
            spriter = GetComponent<SpriteRenderer>();
            // Animator ������Ʈ�� ������
            animator = GetComponent<Animator>();
        }

        // ������ ������Ʈ: �� FixedUpdate() ȣ�� �� ���� �̵��� ó��
        private void FixedUpdate()
        {
            // ���� ������� ������ �̵��� ó������ ����
            if (!isLive)
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
            // ���� Ȱ��ȭ�� �� ����ִ� ���·� �ʱ�ȭ
            isLive = true;
            // ���� Ÿ���� �÷��̾�� ����
            target = GameManager.instance.player.GetComponent<Rigidbody2D>();

            // ���� ü���� �ִ� ü������ ����
            health = maxHealth;
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
    }
}
