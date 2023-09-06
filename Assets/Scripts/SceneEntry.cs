using Controller;
using Model;
using UnityEngine;
using Utilities;
using View;
using View.Assets;

public class SceneEntry : MonoBehaviour
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

        BindConfig();
        BindControllers();
        BindViews();
    }

    private void BindConfig()
    {
        boardConfig.Initialize();
        DiManager.Instance.Bind(boardConfig);
    }

    private void BindControllers()
    {
        SpawnController spawnController = new();
        DiManager.Instance.Bind(spawnController);

        BoardFactory factory = new();
        BoardModel model = factory.Get();
        BoardController boardController = new(model);
        DiManager.Instance.Bind(boardController);
    }

    private void BindViews()
    {
        DiManager.Instance.Bind(camera);
        DiManager.Instance.Bind(assetsRepository);
        DiManager.Instance.Bind(inputView);

        CoordConverter coordConverter = new(camera, boardConfig.Width, boardConfig.Height);
        DiManager.Instance.Bind(coordConverter);
    }
}
