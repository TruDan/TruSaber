using System;
using System.Collections.ObjectModel;
using JetBrains.Annotations;

namespace TruSaber
{
  /// <summary>
  /// Arguments for the <see cref="E:Microsoft.Xna.Framework.GameComponentCollection.ComponentAdded" /> and
  /// <see cref="E:Microsoft.Xna.Framework.GameComponentCollection.ComponentRemoved" /> events.
  /// </summary>
  public class NotifyingCollectionEventArgs<T> : EventArgs
  {
    private T _item;

    /// <summary>
    /// Create a <see cref="T:Microsoft.Xna.Framework.GameComponentCollectionEventArgs" /> instance.
    /// </summary>
    /// <param name="item">The <see cref="T:Microsoft.Xna.Framework.IGameComponent" /> that the event notifies about.</param>
    public NotifyingCollectionEventArgs(T item) => this._item = item;

    /// <summary>
    /// The <see cref="T:Microsoft.Xna.Framework.IGameComponent" /> that the event notifies about.
    /// </summary>
    public T Item => this._item;
  }

  /// <summary>
  /// A collection of <see cref="T" /> instances.
  /// </summary>
  public sealed class NotifyingCollection<T> : Collection<T>
  {
    /// <summary>
    /// Event that is triggered when a <see cref="T" /> is added
    /// to this <see cref="NotifyingCollection{T}" />.
    /// </summary>
    public event EventHandler<NotifyingCollectionEventArgs<T>> ItemAdded;

    /// <summary>
    /// Event that is triggered when a <see cref="T" /> is removed
    /// from this <see cref="NotifyingCollection{T}" />.
    /// </summary>
    public event EventHandler<NotifyingCollectionEventArgs<T>> ItemRemoved;

    /// <summary>
    /// Removes every <see cref="T" /> from this <see cref="NotifyingCollection{T}" />.
    /// Triggers <see cref="M:TruSaber.NotifyingCollection.OnItemRemoved(TruSaber.NotifyingCollectionEventArgs)" /> once for each <see cref="T" /> removed.
    /// </summary>
    protected override void ClearItems()
    {
      for (int index = 0; index < this.Count; ++index)
        this.OnItemRemoved(new NotifyingCollectionEventArgs<T>(this[index]));
      base.ClearItems();
    }

    protected override void InsertItem(int index, T item)
    {
      if (this.IndexOf(item) != -1)
        throw new ArgumentException("Cannot Add Same Item Multiple Times");
      base.InsertItem(index, item);
      if (item == null)
        return;
      this.OnItemAdded(new NotifyingCollectionEventArgs<T>(item));
    }

    private void OnItemAdded(NotifyingCollectionEventArgs<T> eventArgs) => EventHelpers.Raise<NotifyingCollectionEventArgs<T>>((object) this, this.ItemAdded, eventArgs);

    private void OnItemRemoved([CanBeNull] NotifyingCollectionEventArgs<T> eventArgs) => EventHelpers.Raise<NotifyingCollectionEventArgs<T>>((object) this, this.ItemRemoved, eventArgs);

    protected override void RemoveItem(int index)
    {
      T item = this[index];
      base.RemoveItem(index);
      if (item == null)
        return;
      this.OnItemRemoved(new NotifyingCollectionEventArgs<T>(item));
    }

    protected override void SetItem(int index, T item) => throw new NotSupportedException();
  }
}