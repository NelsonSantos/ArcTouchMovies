using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace ArcTouchMovies.CustomControls
{
    public class ListViewEx : Xamarin.Forms.ListView
    {

        private object m_LastObject = null;

        public ListViewEx()
            : base(ListViewCachingStrategy.RecycleElementAndDataTemplate)
        {
            this.ItemTapped += this.OnItemTapped;
            this.ItemAppearing += this.OnItemAppearing;
            this.On<Android>().SetIsFastScrollEnabled(true);
        }

        private void OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (this.IsRefreshing) return;
            var _itens = this.ItemsSource as IList;
            if (_itens == null) return;
            if (e.Item != _itens[_itens.Count - 1]) return;
            if (e.Item.Equals(m_LastObject)) return;
            if (this.PageLoadCommand != null && this.PageLoadCommand.CanExecute(null))
            {
                m_LastObject = e.Item;
                this.PageLoadCommand.Execute(null);
            }
        }

#pragma warning disable CS0618 // Type or member is obsolete
        public static BindableProperty ItemTappedCommandProperty = BindableProperty.Create<ListViewEx, ICommand>(x => x.ItemTappedCommand, null);
#pragma warning restore CS0618 // Type or member is obsolete

        public ICommand ItemTappedCommand
        {
            get { return (ICommand)this.GetValue(ItemTappedCommandProperty); }
            set { this.SetValue(ItemTappedCommandProperty, value); }
        }

        public static BindableProperty PageLoadCommandProperty = BindableProperty.Create<ListViewEx, ICommand>(x => x.PageLoadCommand, null);

        public ICommand PageLoadCommand
        {
            get { return (ICommand)this.GetValue(PageLoadCommandProperty); }
            set { this.SetValue(PageLoadCommandProperty, value); }
        }

        public static readonly BindableProperty IsSelectedItemNullOnTappedProperty =
            BindableProperty.Create(nameof(ListViewEx.IsSelectedItemNullOnTapped), typeof(bool),
                typeof(ListViewEx), true, BindingMode.OneWay,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    var control = bindable as ListViewEx;
                    control.IsSelectedItemNullOnTapped = (bool)newValue;
                });

        public bool IsSelectedItemNullOnTapped
        {
            get { return (bool)this.GetValue(IsSelectedItemNullOnTappedProperty); }
            set { this.SetValue(IsSelectedItemNullOnTappedProperty, value); }
        }

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null && this.ItemTappedCommand != null && this.ItemTappedCommand.CanExecute(e.Item))
            {
                this.ItemTappedCommand.Execute(e.Item);
            }

            if (this.IsSelectedItemNullOnTapped)
                this.SelectedItem = null;
        }

    }

}
