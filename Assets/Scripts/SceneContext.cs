using UnityEngine;

public class SceneContext : MonoBehaviour
{
    [SerializeField]
    private Camera camera;

    private void Awake()
    {
        DiManager.Instance.Initialize();

        RegisterViews();
    }

    private void RegisterViews()
    {
        DiManager.Instance.Register(camera);
    }
}
