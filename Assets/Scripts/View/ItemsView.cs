using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Controller;
using Model;
using UnityEngine;
using UnityEngine.Assertions;
using Utilities;
using View.Config;

namespace View
{
    public class ItemsView : MonoBehaviour
    {
        private ViewConfig viewConfig;
        private BoardController controller;
        private CoordConverter coordConverter;

        private readonly List<Item> items = new();

        private CancellationTokenSource animationsCts = new();

        private ItemsPooler pooler;

        private void Awake()
        {
            Inject();
        }

        private void Inject()
        {
            viewConfig = DiManager.Instance.Resolve<ViewConfig>();
            controller = DiManager.Instance.Resolve<BoardController>();
            coordConverter = DiManager.Instance.Resolve<CoordConverter>();
        }

        private void Start()
        {
            pooler = new ItemsPooler(transform);

            controller.OnItemSpawned += ItemSpawned;
            controller.OnRefreshItems += RefreshItems;
        }

        private void ItemSpawned(BoardPosition start, BoardPosition end, ItemType type)
        {
            Item item = CreateItem(type);

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

        private void RefreshItems(IEnumerable<(BoardPosition position, ItemType type)> currentItems)
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

        private void CreateItems(IEnumerable<(BoardPosition position, ItemType type)> currentItems)
        {
            foreach ((BoardPosition position, ItemType type) in currentItems)
            {
                Item item = CreateItem(type);

                item.transform.localPosition = coordConverter.BoardToWorld(position);
            }
        }

        private Item CreateItem(ItemType type)
        {
            Item item = pooler.Get(type);

            Assert.IsNotNull(item);

            item.Initialize(type);

            items.Add(item);

            return item;
        }

        private void RemoveItems()
        {
            foreach (Item item in items)
                pooler.Release(item);

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
