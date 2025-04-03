using UnityEngine;

namespace MyVampireSurvivors
{
    public class Bullet : MonoBehaviour
    {
        #region Variables
        // �Ѿ��� ���ط�
        public float damage;
        // �Ѿ��� ���� Ƚ��
        public int per;

        // Rigidbody2D ������Ʈ, ������ �̵��� ���� ���
        Rigidbody2D rb2d;
        #endregion

        // ��ü�� �ʱ�ȭ�� �� ȣ��Ǵ� �Լ�
        private void Awake()
        {
            // Rigidbody2D ������Ʈ�� ������
            rb2d = GetComponent<Rigidbody2D>();
        }

        // �Ѿ��� �ʱ�ȭ�ϴ� �Լ� (���ط�, ���� Ƚ��, �̵� ����)
        public void Init(float damage, int per, Vector3 dir)
        {
            // ���ط��� ���� Ƚ�� �ʱ�ȭ
            this.damage = damage;
            this.per = per;

            // ���� Ƚ���� -1�� �ƴ� ���, �Ѿ˿� �ӵ� ������ ����
            if (per > -1)
            {
                // Rigidbody2D�� linearVelocity�� �̿��� �Ѿ��� �̵� ����� �ӵ��� ����
                rb2d.linearVelocity = dir * 15f;
            }
        }

        // �浹�� �߻����� �� ȣ��Ǵ� �Լ�
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // �浹�� ������Ʈ�� "Enemy" �±װ� �ƴϰų�, ���� Ƚ���� -1�� ���� ����
            if (!collision.CompareTag("Enemy") || per == -1)
                return;

            // ���� Ƚ���� �ϳ� ����
            per--;

            // ���� Ƚ���� -1�� �Ǹ�, �Ѿ��� ���߰� ��Ȱ��ȭ
            if (per == -1)
            {
                // �ӵ��� 0���� ���� �Ѿ��� �̵��� ����
                rb2d.linearVelocity = Vector2.zero;
                // �Ѿ��� ��Ȱ��ȭ (���� ������Ʈ�� ���� �� �̻� �浹���� �ʰ� ��)
                gameObject.SetActive(false);
            }
        }
    }
}
