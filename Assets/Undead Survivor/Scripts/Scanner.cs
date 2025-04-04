using UnityEngine;

namespace MyVampireSurvivors
{
    public class Scanner : MonoBehaviour
    {
        #region Variables
        // Ž�� ����: �� ���� ������ Ÿ���� Ž���մϴ�.
        public float scanRange;

        // Ž���� Ÿ�� ���̾�: Ÿ���� ���Ե� ���̾ �����Ͽ�, �ش� ���̾��� ������Ʈ�� Ž���� �� �ְ� �մϴ�.
        public LayerMask targetLayer;

        // Ž���� Ÿ�ٵ�: ���� ������ Ž���� ��� Ÿ���� �����մϴ�.
        public RaycastHit2D[] targets;

        // ���� ����� Ÿ��: ���� Ÿ�� �� ���� ����� Ÿ���� �����մϴ�.
        public Transform nearestTarget;
        #endregion

        // �� �����Ӹ��� ������ ����� ���� Ž��
        private void FixedUpdate()
        {
            if (!GameManager.instance.isLive) return;
            // Physics2D.CircleCastAll�� ����Ͽ� ���� ���� ��� Ÿ���� Ž��
            // Ž���� Ÿ���� targets �迭�� ����
            targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);

            // ���� ����� Ÿ���� ã�� nearestTarget�� ����
            nearestTarget = GetNearest();
        }

        // ���� ����� Ÿ���� ã�� �Լ�
        Transform GetNearest()
        {
            // �⺻������ ã�� Ÿ���� ���ٰ� �����ϰ� null�� ����
            Transform result = null;
            // ���� ����� Ÿ�ٰ��� �Ÿ� ���̸� ���ϱ� ���� ����
            float diff = 100;

            // Ž���� ��� Ÿ�ٿ� ���� �ݺ�
            foreach (RaycastHit2D target in targets)
            {
                // ���� Scanner�� ��ġ�� Ÿ���� ��ġ�� ������
                Vector3 myPos = transform.position;
                Vector3 targetPos = target.transform.position;

                // ���� Ÿ�ٰ��� �Ÿ� ���
                float curDiff = Vector3.Distance(myPos, targetPos);

                // ���� Ÿ�ٰ��� �Ÿ��� ������ ����� �Ÿ����� �� ����� ���
                if (curDiff < diff)
                {
                    // ���� ����� Ÿ���� ã����, �� Ÿ���� result�� ����
                    diff = curDiff;
                    result = target.transform;
                }
            }

            // ���� ����� Ÿ���� ��ȯ
            return result;
        }

        // �����Ϳ��� ������ ������ �׷��� �ð������� Ȯ���� �� �ְ� �ϴ� �Լ�
        private void OnDrawGizmos()
        {
            // ������� ������ ������� ����
            Gizmos.color = Color.green;

            // �� ���·� Ž�� ������ �׸��� (2D ȯ�濡�� Ž�� ������ �ð������� ǥ��)
            Gizmos.DrawWireSphere(transform.position, scanRange);
        }
    }
}
