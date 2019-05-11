using UnityEngine;

/// <summary>
/// 生成墙的触发器
/// </summary>
public class WallSpownTrigger : MonoBehaviour
{
    #pragma warning disable 0649 // 这个版本的Unity和VS匹配不太好，加了[SerializeField]的属性不初始化也会出现未初始化警告，需要加上这个【预处理器指令】（用#开头的一些特定的语句）
    [SerializeField]
    WallSpowner _wallSpowner;

    Transform _transform;
    Vector2Int _lastPosition;

    private void Awake()
    {
        _transform = transform;
    }

    private void Start()
    {
        _lastPosition = GetCurrentPosition();
        _wallSpowner.UpdateWalls(_lastPosition);
    }

    private void Update()
    {
        Vector2Int currentPosition = GetCurrentPosition();
        if (!currentPosition.Equals(_lastPosition))
        {
            _lastPosition = currentPosition;
            _wallSpowner.UpdateWalls(_lastPosition);
        }
    }

    Vector2Int GetCurrentPosition()
    {
        return new Vector2Int((int)(_transform.position.x + 0.5f), (int)(_transform.position.z + 0.5f));
    }
}
