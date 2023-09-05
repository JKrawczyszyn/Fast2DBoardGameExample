using UnityEngine;

public class SceneContext : MonoBehaviour
{
    [SerializeField]
    private BoardConfig boardConfig;

    [SerializeField]
    private Camera camera;

    [SerializeField]
    private AssetsRepository assetsRepository;

    [SerializeField]
    private InputView inputView;

    private void Awake()
    {
        DiManager.Instance.Initialize();

        RegisterModels();
        RegisterViews();
    }

    private void RegisterModels()
    {
        boardConfig.Initialize();
        DiManager.Instance.Register(boardConfig);
    }

    private void RegisterViews()
    {
        DiManager.Instance.Register(camera);
        DiManager.Instance.Register(assetsRepository);
        DiManager.Instance.Register(inputView);
    }
}
