using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using Controllers;
using Models;
using Utilities;
#endif

namespace Views
{
    public class DebugView : MonoBehaviour
    {
#if UNITY_EDITOR
        private BoardController boardController;
        private CoordConverter coordConverter;

        private void Awake()
        {
            boardController = ServiceLocator.Instance.Resolve<BoardController>();
            coordConverter = ServiceLocator.Instance.Resolve<CoordConverter>();
        }

        public void OnDrawGizmos()
        {
            if (boardController is null)
                return;

            GUIStyle labelStyle = new(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 14,
                fontStyle = FontStyle.Bold,
                normal = new GUIStyleState { textColor = Color.black },
            };

            for (var x = 0; x < boardController.Width; x++)
                for (var y = 0; y < boardController.Height; y++)
                {
                    BoardPosition position = new(x, y);
                    Vector3 worldPosition = coordConverter.BoardToWorld(position);

                    ItemType type = boardController.GetItem(position);
                    bool clear = boardController.GetClear(position);

                    var text = $"{position}\n{type}\n{clear}";
                    Handles.Label(worldPosition, text, labelStyle);
                }
        }
#endif
    }
}
