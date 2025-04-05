using UnityEngine;
using UnityEngine.InputSystem;      // New Input System�� ����ϱ� ���� �߰�

namespace MyVampireSurvivors
{
    public class Player : MonoBehaviour
    {
        #region Variables
        // �÷��̾��� �Է� ���� (�̵� ����)
        public Vector2 inputVec;
        // �÷��̾��� �̵� �ӵ�
        public float speed;

        public Scanner scanner;

        public Hand[] hands;

        public RuntimeAnimatorController[] animCon;

        // ������ ó���� ���� Rigidbody2D ������Ʈ
        Rigidbody2D rb2d;

        // ��������Ʈ ������, �÷��̾��� ��������Ʈ�� �ٷ�� ������Ʈ
        SpriteRenderer spriteRenderer;

        // �ִϸ����� ������Ʈ, �ִϸ��̼��� �����ϴ� �� ���
        Animator animator;
        #endregion

        // �ʱ�ȭ �۾�, �ʿ��� ������Ʈ�� ����
        private void Awake()
        {
            // Rigidbody2D ������Ʈ�� ������
            rb2d = GetComponent<Rigidbody2D>();
            // SpriteRenderer ������Ʈ�� ������
            spriteRenderer = GetComponent<SpriteRenderer>();
            // Animator ������Ʈ�� ������
            animator = GetComponent<Animator>();

            scanner = GetComponent<Scanner>();

            //��Ȱ��ȭ�� ������Ʈ�� �������� (true)
            hands = GetComponentsInChildren<Hand>(true);
        }

        private void OnEnable()
        {
            speed *= Character.Speed; // �÷��̾��� �ӵ��� ����
            animator.runtimeAnimatorController = animCon[GameManager.instance.playerId];
        }

        // ������ ������Ʈ �Լ� (�� ������ ������ �ð� �������� ȣ��)
        private void FixedUpdate()
        {
            if (!GameManager.instance.isLive) return;
            // �Էµ� ���� ���Ϳ� �ӵ��� ���Ͽ� ���� �̵��� ���� ���
            Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;

            // Rigidbody2D�� MovePosition�� ����Ͽ� �÷��̾� ��ġ �̵�
            rb2d.MovePosition(rb2d.position + nextVec);
        }

        // �Է� �ý��ۿ��� �Է°��� ���� �� ȣ��Ǵ� �Լ�
        void OnMove(InputValue value)
        {
            // �Էµ� ���� ���� inputVec�� ����
            inputVec = value.Get<Vector2>();
        }

        // ������ �� ������Ʈ �Լ� (�ִϸ��̼ǰ� ��������Ʈ ȸ�� ��)
        private void LateUpdate()
        {
            if (!GameManager.instance.isLive) return;
            // �ִϸ����Ϳ� "Speed" �Ķ���͸� �����Ͽ� �ִϸ��̼� ��ȯ
            animator.SetFloat("Speed", inputVec.magnitude);

            // X������ �̵� ���̶�� ��������Ʈ�� flipX �� ���� (����/������ ���� ��ȯ)
            if (inputVec.x != 0)
            {
                spriteRenderer.flipX = inputVec.x < 0;
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (!GameManager.instance.isLive) return;

            GameManager.instance.health -= Time.deltaTime * 10f;

            if (GameManager.instance.health <= 0)
            {
                for (int i = 2; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
                animator.SetTrigger("Dead");
                GameManager.instance.GameOver();
            }
        }
    }
}
