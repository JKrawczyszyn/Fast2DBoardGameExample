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

        ServiceLocator.Instance.Initialize();

        BindControllers();

        BindViews();
    }

    private void BindControllers()
    {
        boardConfig.Initialize();
        ServiceLocator.Instance.Bind(boardConfig);

        BoardAlgorithmService boardAlgorithmService = new();
        boardAlgorithmService.Initialize(boardConfig.startMaxSearchDistance);
        ServiceLocator.Instance.Bind(boardAlgorithmService);

        SpawnController spawnController = new();
        ServiceLocator.Instance.Bind(spawnController);

        BoardFactory factory = new();
        BoardModel model = factory.Get();
        BoardController boardController = new(model);
        ServiceLocator.Instance.Bind(boardController);
    }

    private void BindViews()
    {
        ServiceLocator.Instance.Bind(viewConfig);

        ServiceLocator.Instance.Bind(camera);

        ServiceLocator.Instance.Bind(inputView);

        CoordConverter coordConverter = new(camera, boardConfig.Width, boardConfig.Height);
        ServiceLocator.Instance.Bind(coordConverter);

        if (viewConfig.fields.optimized)
            Destroy(fieldsView.gameObject);
        else
            Destroy(optimizedFieldsView.gameObject);

        ServiceLocator.Instance.Bind(new ItemsPooler(itemsParent));

        ServiceLocator.Instance.Bind(new ItemsFactory());

        if (!viewConfig.showFps)
            Destroy(fpsView.gameObject);

        if (!viewConfig.debugView)
            Destroy(debugView.gameObject);
    }
}
