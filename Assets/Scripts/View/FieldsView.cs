using Model;
using UnityEngine;

namespace View
{
    public class FieldsView : MonoBehaviour
    {
        private AssetsRepository assetsRepository;
        private BoardConfig boardConfig;

        private void Awake()
        {
            Inject();
        }

        private void Start()
        {
            CreateBoard(boardConfig.Width, boardConfig.Height);
        }

        private void Inject()
        {
            assetsRepository = DiManager.Instance.Resolve<AssetsRepository>();
            boardConfig = DiManager.Instance.Resolve<BoardConfig>();
        }

        private void CreateBoard(int width, int height)
        {
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    Vector3 position = new(x - ((width - 1) / 2f), y - ((height - 1) / 2f), 0f);

                    Field field = Instantiate(assetsRepository.fieldsConfig.prefab, position, Quaternion.identity,
                        transform);

                    field.Initialize(assetsRepository, FieldType.Normal, x, y);
                }
            }
        }
    }
}
