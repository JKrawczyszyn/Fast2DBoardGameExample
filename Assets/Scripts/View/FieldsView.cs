using Model;
using UnityEngine;

namespace View
{
    public class FieldsView : MonoBehaviour
    {
        private AssetsRepository assetsRepository;
        private BoardModel model;

        private void Awake()
        {
            Inject();
        }

        private void Start()
        {
            CreateBoard(model.Width, model.Height);
        }

        private void Inject()
        {
            assetsRepository = DiManager.Instance.Resolve<AssetsRepository>();
            model = DiManager.Instance.Resolve<BoardModel>();
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

                    FieldType type = model.GetField(x, y);
                    field.Initialize(assetsRepository, type, x, y);
                }
            }
        }
    }
}
