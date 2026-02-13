namespace ShadowViewer.Sdk.Models;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public class ObservableCollectionFast<T> : ObservableCollection<T>
{
    private bool isBatchUpdating;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="range"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public void AddRange(IEnumerable<T> range)
    {
        if (range == null) throw new ArgumentNullException(nameof(range));
        isBatchUpdating = true;

        foreach (var item in range)
        {
            Items.Add(item);
        }

        isBatchUpdating = false;

        OnPropertyChanged(new PropertyChangedEventArgs("Count"));
        OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="range"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public void ReplaceAll(IEnumerable<T> range)
    {
        if (range == null) throw new ArgumentNullException(nameof(range));

        // 开启批量更新锁，防止 Clear 和 Add 触发碎片的通知
        isBatchUpdating = true;

        try
        {
            Items.Clear();
            foreach (var item in range)
            {
                Items.Add(item);
            }
        }
        finally
        {
            isBatchUpdating = false;
        }

        OnPropertyChanged(new PropertyChangedEventArgs("Count"));
        OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    /// <inheritdoc />
    protected override void InsertItem(int index, T item)
    {
        if (isBatchUpdating)
        {
            CheckReentrancy();
            Items.Insert(index, item);
            return;
        }

        base.InsertItem(index, item);
    }
}