using UnityEngine;

namespace MyVampireSurvivors
{
    public class Hand : MonoBehaviour
    {
        #region Variables
        // �޼����� ������������ ��Ÿ���� ����
        public bool isLeft;

        // �տ� �ش��ϴ� ��������Ʈ ������ (���� �̹����� ǥ�õǴ� UI ���)
        public SpriteRenderer spriter;

        // �÷��̾��� ��������Ʈ ������ (�÷��̾��� ���⿡ ���� ���� ������ ����)
        SpriteRenderer player;

        // ������ ��ġ (�⺻ ��ġ�� ������ ��ġ)
        Vector3 rightPos = new Vector3(0.35f, -0.15f, 0);
        Vector3 rightPosReverse = new Vector3(0f, -0.15f, 0);

        // �޼� ȸ�� (�⺻ ȸ���� ������ ȸ��)
        Quaternion leftRot = Quaternion.Euler(0, 0, -35);
        Quaternion leftRotReverse = Quaternion.Euler(0, 0, -135);
        #endregion

        // �ʱ�ȭ �۾�: �÷��̾��� ��������Ʈ �������� ������
        private void Awake()
        {
            // �÷��̾��� ��������Ʈ �������� �θ� ������Ʈ���� ������
            player = GetComponentsInParent<SpriteRenderer>()[1];
        }

        // �� ������ �� ȣ��Ǵ� �Լ� (���� ��ġ�� ������ ������Ʈ)
        private void LateUpdate()
        {
            // �÷��̾ ������ �������� Ȯ�� (�÷��̾��� `flipX` ���� ���� ���� ������ ����)
            bool isReverse = player.flipX;

            // �޼��� ��� (���� ������ ���)
            if (isLeft)
            {
                // �÷��̾ ������ �����̸� �޼��� ȸ���� ������ ������ ����, �׷��� ������ �⺻ ȸ���� ���
                transform.localRotation = isReverse ? leftRotReverse : leftRot;
                // ���� ��������Ʈ ������ ������Ŵ (�÷��̾ �����Ǹ� �յ� ����)
                spriter.flipY = isReverse;
                // ���� ������ ������ ���� (�����Ǹ� �ٸ� ���̾ ��ġ)
                spriter.sortingOrder = isReverse ? 4 : 6;
            }
            else // ������ (���Ÿ� ������ ���)
            {
                // �÷��̾ ������ �����̸� �������� ��ġ�� ������ ��ġ�� ����, �׷��� ������ �⺻ ��ġ ���
                transform.localPosition = isReverse ? rightPosReverse : rightPos;
                // ���� ��������Ʈ ������ ������Ŵ (�÷��̾ �����Ǹ� �յ� ����)
                spriter.flipX = isReverse;
                // ���� ������ ������ ���� (�����Ǹ� �ٸ� ���̾ ��ġ)
                spriter.sortingOrder = isReverse ? 6 : 4;
            }
        }
    }
}
