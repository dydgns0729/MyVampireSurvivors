using UnityEngine;
using UnityEngine.Tilemaps;

namespace MyVampireSurvivors
{
    public class TilemapManager : MonoBehaviour
    {
        #region Variables
        public GameObject grid;                        // 타일맵들이 배치될 부모 오브젝트
        Transform player;                              // 플레이어 Transform 참조
        Vector3Int playerGridPosition;                 // 현재 플레이어의 그리드 좌표
        Vector3Int lastPlayerGridPosition;             // 이전 프레임의 플레이어 그리드 좌표

        Vector2 tilemapSize;                           // 각 타일맵 하나의 크기 (가로 x 세로)
        #endregion

        void Start()
        {
            player = GameManager.instance.player.transform;

            // 원본 타일맵 하나 가져오기 (자식 0번)
            var tile = grid.transform.GetChild(0);

            // 타일맵의 크기 계산 (너비, 높이)
            tilemapSize = CalculateTileSize(tile.GetComponent<Tilemap>());

            // 타일맵을 3개 복사해서 grid에 붙이기 (총 1 + 3 = 4개가 됨)
            for (int i = 0; i < 3; i++)
            {
                Instantiate(tile).transform.SetParent(grid.transform);
            }

            int index = 0;

            // 3x3 격자 형태로 타일맵들을 배치
            for (float x = -tilemapSize.x / 2; x <= tilemapSize.x / 2; x += tilemapSize.x)
            {
                for (float y = -tilemapSize.y / 2; y <= tilemapSize.y / 2; y += tilemapSize.y)
                {
                    Debug.Log("index = " + index);
                    // 각 타일맵의 위치를 정사각형 형태로 배치
                    grid.transform.GetChild(index++).position = new Vector3(x, y, 0);
                }
            }

            // 시작 시 플레이어의 위치를 그리드 좌표로 저장
            playerGridPosition = GetGridPosition(player.position);
            lastPlayerGridPosition = playerGridPosition;
        }

        void Update()
        {
            // 매 프레임마다 현재 플레이어 위치를 그리드 좌표로 변환
            playerGridPosition = GetGridPosition(player.position);

            // 플레이어가 다른 그리드 칸으로 이동했는지 확인
            if (playerGridPosition != lastPlayerGridPosition)
            {
                UpdateTilemaps(); // 타일맵 위치 재배치
                lastPlayerGridPosition = playerGridPosition;
            }
        }

        // 월드 좌표를 타일맵 단위 그리드 좌표로 변환
        Vector3Int GetGridPosition(Vector3 position)
        {
            return new Vector3Int(
                Mathf.RoundToInt(position.x / tilemapSize.x),
                Mathf.RoundToInt(position.y / tilemapSize.y),
                0
            );
        }

        // 현재 타일맵의 실제 사용 영역을 계산해서 크기 반환
        Vector2 CalculateTileSize(Tilemap tilemap)
        {
            BoundsInt bounds = tilemap.cellBounds;

            Vector3Int? min = null;
            Vector3Int? max = null;

            // 타일이 실제로 존재하는 영역의 최소/최대 좌표를 계산
            foreach (var pos in bounds.allPositionsWithin)
            {
                if (tilemap.HasTile(pos))
                {
                    if (min == null)
                    {
                        min = pos;
                        max = pos;
                    }
                    else
                    {
                        min = Vector3Int.Min(min.Value, pos);
                        max = Vector3Int.Max(max.Value, pos);
                    }
                }
            }

            // 타일이 존재하는 실제 가로/세로 칸 수 계산
            int width = -1;
            int height = -1;

            if (min.HasValue && max.HasValue)
            {
                width = max.Value.x - min.Value.x + 1;
                height = max.Value.y - min.Value.y + 1;
            }

            return new Vector2(width, height);
        }

        // 플레이어 이동에 따라 타일맵 전체를 이동시킴
        void UpdateTilemaps()
        {
            // 이전 위치 대비 현재 위치의 그리드 오프셋 계산
            Vector3Int offset = playerGridPosition - lastPlayerGridPosition;

            // 모든 타일맵을 그 오프셋만큼 이동시켜서 맵이 무한히 이어지는 것처럼 보이게 만듦
            foreach (Transform child in grid.transform)
            {
                child.position += new Vector3(offset.x * tilemapSize.x, offset.y * tilemapSize.y, 0);
            }
        }
    }
}
