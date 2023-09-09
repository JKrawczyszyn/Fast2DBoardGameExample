using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Controllers;
using Models;
using UnityEngine;
using Utilities;
using Views.Config;

namespace Views
{
    public class ItemsView : MonoBehaviour
    {
        private ViewConfig viewConfig;
        private BoardController controller;
        private CoordConverter coordConverter;
        private ItemsFactory itemsFactory;

        private readonly List<Item> items = new();

        private CancellationTokenSource animationsCts = new();

        private void Awake()
        {
            Inject();
        }

        private void Inject()
        {
            viewConfig = DiManager.Instance.Resolve<ViewConfig>();
            controller = DiManager.Instance.Resolve<BoardController>();
            coordConverter = DiManager.Instance.Resolve<CoordConverter>();
            itemsFactory = DiManager.Instance.Resolve<ItemsFactory>();
        }

        private void Start()
        {
            controller.OnItemSpawned += ItemSpawned;
            controller.OnRefreshItems += RefreshItems;
        }

        private void ItemSpawned(BoardPosition start, BoardPosition end, ItemType type)
        {
            Item item = itemsFactory.Create(type);

            items.Add(item);

            AnimateMove(item, start, end).Forget();
        }

        private async Task AnimateMove(Item item, BoardPosition start, BoardPosition end)
        {
            Vector3 worldStartPosition = coordConverter.BoardToWorld(start);
            Vector3 worldEndPosition = coordConverter.BoardToWorld(end);

            item.SetOrder(1);

            await item.transform.AnimateMoveLocal(worldStartPosition, worldEndPosition,
                viewConfig.spawnAnimationTime, animationsCts.Token);

            item.SetOrder(0);
        }

        private void RefreshItems((BoardPosition position, ItemType type)[] currentItems)
        {
            CancelAnimations();
            RemoveItems();
            CreateItems(currentItems);
        }

        private void CancelAnimations()
        {
            animationsCts.Cancel();
            animationsCts.Dispose();
            animationsCts = new CancellationTokenSource();
        }

        private void CreateItems((BoardPosition position, ItemType type)[] currentItems)
        {
            foreach ((BoardPosition position, ItemType type) in currentItems)
            {
                Item item = itemsFactory.Create(type);

                item.transform.localPosition = coordConverter.BoardToWorld(position);

                items.Add(item);
            }
        }

        private void RemoveItems()
        {
            foreach (Item item in items)
                itemsFactory.Destroy(item);

            items.Clear();
        }

        private void OnDestroy()
        {
            CancelAnimations();

            controller.OnItemSpawned -= ItemSpawned;
            controller.OnRefreshItems -= RefreshItems;
        }
    }
}
