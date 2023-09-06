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
    private ViewConfig viewConfig;

    [SerializeField]
    private Camera camera;

    [SerializeField]
    private InputView inputView;

    private void Awake()
    {
        Application.targetFrameRate = viewConfig.targetFrameRate;

        DiManager.Instance.Initialize();

        BindControllers();
        BindViews();
    }

    private void BindControllers()
    {
        boardConfig.Initialize();
        DiManager.Instance.Bind(boardConfig);

        SpawnController spawnController = new();
        DiManager.Instance.Bind(spawnController);

        BoardFactory factory = new();
        BoardModel model = factory.Get();
        BoardController boardController = new(model);
        DiManager.Instance.Bind(boardController);
    }

    private void BindViews()
    {
        DiManager.Instance.Bind(viewConfig);

        DiManager.Instance.Bind(camera);

        DiManager.Instance.Bind(inputView);

        CoordConverter coordConverter = new(camera, boardConfig.Width, boardConfig.Height);
        DiManager.Instance.Bind(coordConverter);
    }
}
