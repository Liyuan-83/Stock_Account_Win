using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ListViewStyle
{
    public class ListViewStyleSelector : StyleSelector
    {
        public ListViewStyleSelector() {
        }


        public override Style SelectStyle(object item, DependencyObject container)
        {
            Style st = new Style();
            st.TargetType = typeof(ListViewItem);
            Setter backGroundSetter = new Setter();
            backGroundSetter.Property = ListViewItem.BackgroundProperty;
            Setter textColorSetter = new Setter();
            textColorSetter.Property = ListViewItem.ForegroundProperty;
            ListView listView =
                ItemsControl.ItemsControlFromItemContainer(container)
                  as ListView;
            int index =
                listView.ItemContainerGenerator.IndexFromContainer(container);
           if (index % 2 == 0)
            {
                backGroundSetter.Value = Brushes.LightBlue;
            }
            else
            {
                backGroundSetter.Value = Brushes.LightGray;
            }
            textColorSetter.Value = Brushes.Black;
            st.Setters.Add(backGroundSetter);
            st.Setters.Add(textColorSetter);

            Setter selectedSetter = new Setter();
            selectedSetter.Property = ListViewItem.IsSelectedProperty;


            return st;
        }
    }
}
