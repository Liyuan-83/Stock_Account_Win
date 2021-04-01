using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ListViewStyle
{
    public class ListViewStyleSelector : StyleSelector
    {
        public event MouseButtonEventHandler DoubleClickHandler;

        public override Style SelectStyle(object item, DependencyObject container)
        {
            Style st = new Style();
            st.TargetType = typeof(DataGridRow);
            Setter backGroundSetter = new Setter();
            backGroundSetter.Property = DataGridRow.BackgroundProperty;
            Setter textColorSetter = new Setter();
            textColorSetter.Property = DataGridRow.ForegroundProperty;
            DataGrid listView =
                ItemsControl.ItemsControlFromItemContainer(container)
                  as DataGrid;
            int index =
                listView.ItemContainerGenerator.IndexFromContainer(container);

            if (index % 2 == 0)
            {
                backGroundSetter.Value = Brushes.DeepSkyBlue;
            }
            else
            {
                backGroundSetter.Value = Brushes.LightBlue;
            }
            textColorSetter.Value = Brushes.Black;
            st.Setters.Add(backGroundSetter);
            st.Setters.Add(textColorSetter);

            EventSetter eventSetter = new EventSetter();
            eventSetter.Event = DataGridRow.MouseDoubleClickEvent;
            eventSetter.Handler = DoubleClickHandler;
            st.Setters.Add(eventSetter);
            return st;
        }
    }
}
