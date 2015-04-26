using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Classes.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Classes.Foundation.Extensions
{
    public static class ListRandomExtensions
    {



        public static T Random<T>(this IList<T> l)
        {
            if (l == null || !l.Any()) return default(T);
            return l[UnityEngine.Random.Range(0, l.Count)];

        }

        public static T NonRecurringRandom<T>(this IList<T> l) 
        {
            return NRRListWrappers.GetWrapperForListOrCreateOne(l).GetRandomNonRecurringItem();
        }

        public static void SetAsNotUsed<T>(this IList<T> list, T item)
        {
            NRRListWrappers.GetWrapperForListOrCreateOne(list).UnMarkUsedItem(item);
        }

    }


    public static class NRRListWrappers
    {

        private static readonly IList<object> Wrappers = new List<object>();

        public static NRRListWrapper<T> GetWrapperForListOrCreateOne<T>(IList<T> l)
        {
            var w = GetWrapperForListOrDefault(l);
            if (w != null)
            {
                return w;
            }
            return CreateWrapperForList(l);
        }

        public static NRRListWrapper<T> GetWrapperForListOrDefault<T>(IList<T> l) 
        {
            return Wrappers.FirstOrDefault(wrapper =>
                                           {
                                               var typedWrapper = wrapper as NRRListWrapper<T>;
                                               if (typedWrapper == null) return false;
                                               return typedWrapper.List.Equals(l);
                                           }) as NRRListWrapper<T>;
        }

        public static NRRListWrapper<T> CreateWrapperForList<T>(IList<T> l) 
        {
            var w = new NRRListWrapper<T>(l);
            Wrappers.Add(w);
            return w;
        }
    }

    public class NRRListWrapper<T>
    {
        public NRRListWrapper(IList<T> list)
        {
            List = list;
            unusedItems = new List<T>(List);
        }
        public NRRListWrapper(IList<T> list, IEnumerable<T> alreadyUsedItems)
            : this(list)
        {
            foreach (var alreadyUsedItem in alreadyUsedItems)
            {
                MarkItemAsUsed(alreadyUsedItem);
            }
        }
        public NRRListWrapper(IList<T> list, NRRListStorage storage) : this(list)
        {
            foreach (var index in storage.UnusedItemsIndexes)
            {
                var item = list[index];
                unusedItems.Add(item);
            }
        }


        public IList<T> List { get; private set; }


        public event EventHandler CycleConsumed;
        private void OnCycleConsumed()
        {
            var handler = CycleConsumed;
            if (handler != null) handler(this, EventArgs.Empty);
        }


        public NRRListStorage GetStorage()
        {
            var indexes = new List<int>();

            foreach (var unusedItem in unusedItems)
            {
                indexes.Add(List.IndexOf(unusedItem));
            }

            return new NRRListStorage(indexes);

        }

        private List<T> unusedItems;

        public bool HasUnusedItems
        {
            get { return unusedItems.Any(); }
        }

        public bool IsItemIsUsed(T item)
        {
            return !unusedItems.Contains(item);
        }
        public void MarkItemAsUsed(T item)
        {
            unusedItems.Remove(item);
        }
        public void UnMarkUsedItem(T item)
        {
            if(!unusedItems.Contains(item))
                unusedItems.Add(item);
        }
        public void UnMarkAllUsedItems()
        {
            unusedItems = new List<T>(List);
        }

        public void NotifyItemAdded(T item)
        {
            unusedItems.Add(item);
        }
        public void NotifyItemRemoved(T item)
        {
            unusedItems.Remove(item);
        }

        public T LastUsedItem { get; private set; }

        public T GetRandomUnusedItem()
        {

            return unusedItems[Random.Range(0, unusedItems.Count)];
        }
        public T GetRandomNonRecurringItem()
        {

            if (List == null || !List.Any()) return default(T);
            if (List.Count == 1) return List.FirstOrDefault();


            if (HasUnusedItems)
            {
                var unusedItem = GetRandomUnusedItem();
                MarkItemAsUsed(unusedItem);
                LastUsedItem = unusedItem;
                return unusedItem;
            }

            OnCycleConsumed();
            UnMarkAllUsedItems();
            MarkItemAsUsed(LastUsedItem); // mark last item as used to avoid x-y-y-z issue 
            var firstRandomItem = GetRandomUnusedItem();
            UnMarkUsedItem(LastUsedItem);
            MarkItemAsUsed(firstRandomItem);
            LastUsedItem = firstRandomItem;
            return firstRandomItem;

        }

    }

    public class NRRListStorage
    {
        public NRRListStorage(IEnumerable<int> unusedItemsIndexes)
        {
            UnusedItemsIndexes = unusedItemsIndexes;
        }

        public IEnumerable<int> UnusedItemsIndexes { get; private set; }

        public static NRRListStorage ParseStorageString(string str)
        {
            var indexesStr = str.Split('-');
            var indexes = new List<int>();

            foreach (var s in indexesStr)
            {
                indexes.Add(Convert.ToInt32(s));
            }

            return new NRRListStorage(indexes);


        }

        public static string CreateStorageString(NRRListStorage storage)
        {
            var b = new StringBuilder();
            var isFirst = true;
            foreach (var i in storage.UnusedItemsIndexes)
            {
                if (!isFirst) b.Append('-');
                b.Append(i);
                isFirst = false;
            }
            return b.ToString();
        }

        public static NRRListStorage GetStorageFromDatabase(string id)
        {
            var str = GameCore.Instance.Database.GetString(id);
            if (string.IsNullOrEmpty(str)) return null;
            return ParseStorageString(str);

        }

        public static void SaveStorageToDatabase(string id, NRRListStorage storage)
        {
            var str = CreateStorageString(storage);
            GameCore.Instance.Database.SetString(id, str);
        }

    }

    public class NRRListTests<T>
    {
        public NRRListTests(IList<T> list)
        {
            List = list;
            Wrapper = new NRRListWrapper<T>(list);
        }

        public Action<T> OnItemCallback { get; set; }
        public Action<T> OnItemRecyclingErrorCallback { get; set; }
        public Action OnWrapperUnexpectedBehaviourCallback { get; set; }

        public IList<T> List { get; private set; }
        public NRRListWrapper<T> Wrapper { get; private set; }

   

        public bool Test(int items)
        {
            Wrapper.UnMarkAllUsedItems();

            var isFailed = false;

            var usedItems = new List<T>();
            Wrapper.CycleConsumed += (sender, args) => usedItems.Clear();


            for(var i = 0; i < items; i++)
            {
                var item = Wrapper.GetRandomNonRecurringItem();
                if (OnItemCallback != null) OnItemCallback(item);
                if (usedItems.Contains(item))
                {
                    if (OnItemRecyclingErrorCallback != null) OnItemRecyclingErrorCallback(item);
                    isFailed = true;
                }
                usedItems.Add(item);

            }
            return isFailed;

        }


    }

    public class NRRListTestsWithConsoleOutput<T> : NRRListTests<T>
    {
        public NRRListTestsWithConsoleOutput(IList<T> list)
            : base(list)
        {
            OnItemCallback = OnItem;
            OnItemRecyclingErrorCallback = OnItemRecyclingError;
            OnWrapperUnexpectedBehaviourCallback = OnWrapperUnexpectedBehaviour;
            Wrapper.CycleConsumed += (sender, args) => Debug.Log("Cycle consumed");
        }

        private void OnItem(T item)
        {
            Debug.Log("item: " + item.ToString());
        }

        private void OnItemRecyclingError(T item)
        {
            Debug.LogError("Item Recycling error: " + item);
        }

        private void OnWrapperUnexpectedBehaviour()
        {
            Debug.LogError("Wrapper Unexpected Behaviour!");
        }


    }

}
