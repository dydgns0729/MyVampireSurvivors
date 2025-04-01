using UnityEngine;

namespace MyVampireSurvivors
{
    public class Reposition : MonoBehaviour
    {
        #region Variables
        Collider2D coll;
        #endregion

        private void Awake()
        {
            coll = GetComponent<Collider2D>();
        }

        // 2D �ݶ��̴��� ������ ��� �� ȣ��Ǵ� �޼���
        private void OnTriggerExit2D(Collider2D collision)
        {
            // �浹�� ������Ʈ�� "Area" �±׸� ������ �ִ��� Ȯ��
            if (!collision.CompareTag("Area"))
                return; // "Area" �±װ� �ƴϸ� �޼��� ����

            // �÷��̾��� ���� ��ġ�� ������
            Vector3 playerPosition = GameManager.instance.player.transform.position;

            // ���� ������Ʈ�� ��ġ�� ������
            Vector3 myPosition = transform.position;

            // �÷��̾�� ���� ������Ʈ ���� X, Y ��ǥ ���� ���
            float diffX = Mathf.Abs(playerPosition.x - myPosition.x);
            float diffY = Mathf.Abs(playerPosition.y - myPosition.y);

            // �÷��̾��� �Է� ���� ����
            Vector3 playerDir = GameManager.instance.player.inputVec;
            // �÷��̾��� X, Y ������ ���� 1 �Ǵ� -1�� ���� (�̵� ����)
            float dirX = playerDir.x < 0 ? -1 : 1;
            float dirY = playerDir.y < 0 ? -1 : 1;

            // �±׿� ���� �ٸ� ������ ����
            switch (transform.tag)
            {
                // "Ground" �±׸� ���� ������Ʈ�� ���� ó��
                case "Ground":
                    // X�� Y�� ���̰� ����� ��� (�밢�� �������� �̵�)
                    if (Mathf.Abs(diffX - diffY) <= 0.1f)
                    {
                        // �밢�� �̵�
                        transform.Translate(Vector3.up * dirY * 40); // Y������ �̵�
                        transform.Translate(Vector3.right * dirX * 40); // X������ �̵�
                    }
                    // X ���̰� �� ū ��� (X�����θ� �̵�)
                    else if (diffX > diffY)
                    {
                        transform.Translate(Vector3.right * dirX * 40);
                    }
                    // Y ���̰� �� ū ��� (Y�����θ� �̵�)
                    else if (diffX < diffY)
                    {
                        transform.Translate(Vector3.up * dirY * 40);
                    }
                    break;

                // "Enemy" �±׸� ���� ������Ʈ�� ���� ó�� (����� �� ���̽�)
                case "Enemy":
                    if (coll.enabled)
                    {
                        transform.Translate(playerDir * 25 + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0));
                    }
                    break;
            }
        }
    }
}
