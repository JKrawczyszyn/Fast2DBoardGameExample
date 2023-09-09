using Controllers;
using Models;
using UnityEngine;
using Utilities;
using Views;
using Views.Config;

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

    [SerializeField]
    private FieldsView fieldsView;

    [SerializeField]
    private OptimizedFieldsView optimizedFieldsView;

    [SerializeField]
    private Transform itemsParent;

    [SerializeField]
    private FpsView fpsView;

    [SerializeField]
    private DebugView debugView;

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

        BoardAlgorithmService boardAlgorithmService = new();
        boardAlgorithmService.Initialize(boardConfig.startMaxSearchDistance);
        DiManager.Instance.Bind(boardAlgorithmService);

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

        if (viewConfig.fields.optimized)
            Destroy(fieldsView.gameObject);
        else
            Destroy(optimizedFieldsView.gameObject);

        DiManager.Instance.Bind(new ItemsPooler(itemsParent));

        DiManager.Instance.Bind(new ItemsFactory());

        if (!viewConfig.showFps)
            Destroy(fpsView.gameObject);

        if (!viewConfig.debugView)
            Destroy(debugView.gameObject);
    }
}
