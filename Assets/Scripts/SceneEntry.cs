using Controller;
using Model;
using UnityEngine;
using Utilities;
using View;
using View.Config;

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

        fieldsView.gameObject.SetActive(!viewConfig.fields.optimized);
        optimizedFieldsView.gameObject.SetActive(viewConfig.fields.optimized);

        DiManager.Instance.Bind(new ItemsPooler(itemsParent));

        DiManager.Instance.Bind(new ItemsFactory());
    }
}
