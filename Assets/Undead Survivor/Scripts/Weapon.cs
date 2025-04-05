using System;
using UnityEngine;

namespace MyVampireSurvivors
{
    public class Weapon : MonoBehaviour
    {
        #region Variables
        // ������ ���� ID (������ ������ Ư¡�� ������ �� ���)
        public int id;

        // ���� ������ ID (Ǯ���� ������ �������� �����ϴ� ID)
        public int prefabId;

        // ������ ���ط� (�� �߻�ü�� ������ ������ ����)
        public float damage;

        // �߻�Ǵ� �Ѿ��� �� (�� ���� �߻�Ǵ� �Ѿ��� ��)
        public int count;

        // ������ ȸ�� �ӵ� (ȸ���ϴ� ������ �ӵ�)
        public float speed;

        // �ð� ���� ���� (�Ѿ� �߻� ������ �����ϱ� ���� ���)
        float timer;

        // �÷��̾� ��ü ���� (���Ⱑ �÷��̾��� ��ġ�� Ÿ���� ����ϱ� ����)
        Player player;
        #endregion

        // �θ� ������Ʈ���� Player ������Ʈ�� ������ player ������ ����
        private void Awake()
        {
            // ���� ������ �θ� �ش��ϴ� Player ������Ʈ�� ã�� �Ҵ�
            player = GameManager.instance.player;
        }

        // �� �����Ӹ��� ȣ��Ǵ� ������Ʈ �Լ�
        private void Update()
        {
            if (!GameManager.instance.isLive) return;

            // ������ id�� ���� ȸ�� ó��
            switch (id)
            {
                case 0:
                    // id�� 0�� ���, ȸ�� �ӵ��� ���� ���Ⱑ �ð�������� ȸ��
                    transform.Rotate(Vector3.back * speed * Time.deltaTime);
                    break;
                default:
                    // id�� 0�� �ƴ� ���, Ÿ�̸Ӹ� �������� ���� �ð��� ������ �߻�
                    timer += Time.deltaTime;

                    // �߻� ������ ������ �� �߻�
                    if (timer > speed)
                    {
                        timer = 0; // Ÿ�̸� ����
                        Fire(); // Fire() �Լ� ȣ���Ͽ� �Ѿ� �߻�
                    }
                    break;
            }

            // V Ű�� ������ �� ������ ó��
            if (Input.GetKeyDown(KeyCode.V))
            {
                // ������: ���ط� 10 ����, �Ѿ� �� 1 ����
                LevelUp(10, 1);
            }
        }

        // ���� ������ �Լ� (���ط��� �Ѿ��� ���� ������Ŵ)
        public void LevelUp(float damage, int count)
        {
            // �������� ���� ������ ���ط��� ������Ʈ
            this.damage = damage * Character.Damage;
            // �߻�Ǵ� �Ѿ��� ���� ������Ŵ
            this.count += count;

            // ���� ������ id�� 0�� ���, �Ѿ��� ��ġ�ϴ� Batch() �Լ� ȣ��
            if (id == 0)
                Batch();

            // "ApplyGear" �޽����� �÷��̾�� �����Ͽ� ���� ������ �ݿ��ϵ��� ��
            player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
        }

        // ���� �ʱ�ȭ �Լ� (���� id�� ���� �ʱ� ����)
        public void Init(ItemData data)
        {
            // ������ �̸��� ������ ID�� ���� ����
            name = "Weapon " + data.itemId;

            // ������ �θ� �÷��̾�� �����Ͽ�, ���Ⱑ �÷��̾�� �Բ� �̵��ϵ��� ����
            transform.parent = player.transform;

            // ������ �ʱ� ��ġ�� �÷��̾��� ��ġ�� �����ϰ� ����
            transform.localPosition = Vector3.zero;

            // ������ �����Ϳ��� ������ itemId�� �̿��� ������ ���� ID ����
            id = data.itemId;

            // �������� �⺻ ���ط��� ����
            damage = data.baseDamage * Character.Damage;

            // �������� �⺻ �Ѿ� ���� ����
            count = data.baseCount + Character.Count;

            // Ǯ���� �ش� �����ۿ� �´� �߻�ü �������� ã�� �� ID�� ����
            for (int i = 0; i < GameManager.instance.poolManager.prefabs.Length; i++)
            {
                if (data.projectile == GameManager.instance.poolManager.prefabs[i])
                {
                    prefabId = i; // �ش� �������� ID ����
                    break; // ã���� �ݺ��� ����
                }
            }

            // ���� id�� ���� ������ �ٸ��� ó��
            switch (id)
            {
                case 0:
                    // id�� 0�� ���, �Ѿ��� �������� ��ġ
                    speed = 150f * Character.WeaponSpeed;
                    Batch(); // id�� 0�� ��, �������� �Ѿ� ��ġ
                    break;
                default:
                    speed = 0.5f * Character.WeaponRate;
                    Debug.Log("Weapon.cs Init() default"); // ����� �α� ���
                    break;
            }

            #region HandSet
            // ������ �����Ϳ��� �տ� �ش��ϴ� ��������Ʈ�� �����ͼ� ����
            Hand hand = player.hands[(int)data.itemType];
            hand.spriter.sprite = data.hand; // �տ� ��������Ʈ ����
            hand.gameObject.SetActive(true); // �� ������Ʈ�� Ȱ��ȭ
            #endregion

            // "ApplyGear" �޽����� �÷��̾�� �����Ͽ� ���⸦ �����ϵ��� ��
            player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
        }

        // �Ѿ� ��ġ �Լ� (���� id�� 0�� �� �Ѿ��� �������� ��ġ)
        void Batch()
        {
            // ������ ��(count)��ŭ �Ѿ��� ��ġ
            for (int i = 0; i < count; i++)
            {
                Transform bullet;

                // ���� ���� �ڽ����� �̹� �����ϴ� �Ѿ��� ���
                if (i < transform.childCount)
                {
                    bullet = transform.GetChild(i); // �ڽ����� �ִ� �Ѿ��� ������
                }
                else
                {
                    // Ǯ���� ���ο� �Ѿ��� �����ͼ� �ڽ����� �߰�
                    bullet = GameManager.instance.poolManager.Get(prefabId).transform;
                    bullet.parent = transform; // �Ѿ��� ������ �ڽ����� ����
                }

                // �Ѿ� ��ġ �ʱ�ȭ (���� ��ġ���� �����ϵ��� ����)
                bullet.localPosition = Vector3.zero;
                bullet.localRotation = Quaternion.identity;

                // �Ѿ��� ȸ�� ������ ����Ͽ� �������� ��ġ
                Vector3 rotVec = Vector3.forward * 360 * i / count;
                bullet.Rotate(rotVec);

                // �Ѿ��� �������� ��ġ�ϱ� ���� �ణ�� �̵�
                bullet.Translate(bullet.up * 1.5f, Space.World);

                // �Ѿ� �ʱ�ȭ (���ط� ����, -1�� ���� ������ �ǹ�)
                bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero);
            }
        }

        #region OriginFire()
        // Ÿ���� ���� �Ѿ��� �߻��ϴ� �Լ�
        private void Fire()
        {
            // �÷��̾ Ÿ���� ������ ���� ������ �߻����� ����
            if (!player.scanner.nearestTarget)
                return;

            // Ÿ���� ��ġ ���
            Vector3 targetPos = player.scanner.nearestTarget.position;
            // ���� ���� ��ġ���� Ÿ�ٱ����� ���� ���� ���
            Vector3 dir = targetPos - transform.position;
            // ���� ���͸� ����ȭ�Ͽ� ������ �ӵ��� �߻�
            dir = dir.normalized;

            // Ǯ���� ���ο� �Ѿ��� ������
            Transform bullet = GameManager.instance.poolManager.Get(prefabId).transform;

            // �Ѿ��� ��ġ�� ������ ��ġ�� ����
            bullet.position = transform.position;
            // �Ѿ��� ȸ�� ������ Ÿ���� ���ϰ� ����
            bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);

            // �Ѿ� �ʱ�ȭ (���ط�, �Ѿ� ��, ���� ����)
            bullet.GetComponent<Bullet>().Init(damage, count, dir);
        }
        #endregion
    }
}
