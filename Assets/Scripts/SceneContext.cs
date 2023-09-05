using Model;
using UnityEngine;
using View;

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

        BindModels();
        BindViews();
    }

    private void BindModels()
    {
        BindConfig();
        BindBoardModel();
    }

    private void BindConfig()
    {
        boardConfig.Initialize();
        DiManager.Instance.Bind(boardConfig);
    }

    private void BindBoardModel()
    {
        BoardFactory factory = new();
        BoardModel model = factory.Get();
        DiManager.Instance.Bind(model);
    }

    private void BindViews()
    {
        DiManager.Instance.Bind(camera);
        DiManager.Instance.Bind(assetsRepository);
        DiManager.Instance.Bind(inputView);
    }
}
