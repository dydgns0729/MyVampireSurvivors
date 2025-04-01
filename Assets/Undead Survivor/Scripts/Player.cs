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
        }

        // ������ ������Ʈ �Լ� (�� ������ ������ �ð� �������� ȣ��)
        private void FixedUpdate()
        {
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
            // �ִϸ����Ϳ� "Speed" �Ķ���͸� �����Ͽ� �ִϸ��̼� ��ȯ
            animator.SetFloat("Speed", inputVec.magnitude);

            // X������ �̵� ���̶�� ��������Ʈ�� flipX �� ���� (����/������ ���� ��ȯ)
            if (inputVec.x != 0)
            {
                spriteRenderer.flipX = inputVec.x < 0;
            }
        }
    }
}
