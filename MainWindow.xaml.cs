using Syncfusion.Data;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.TreeGrid;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SfDataGrid_MVVM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Dictionary<string, bool> GroupStates = new Dictionary<string, bool>();
        public MainWindow()
        {
            InitializeComponent();
            this.dataGrid.Loaded += OnDataGrid_Loaded;
        }

        private void OnDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            this.dataGrid.View.CollectionChanged += OnView_CollectionChanged;
            this.dataGrid.GroupExpanded += OnDataGrid_GroupExpanded;
            this.dataGrid.GroupCollapsed += OnDataGrid_GroupCollapsed;
        }

        private void OnDataGrid_GroupCollapsed(object sender, GroupChangedEventArgs e)
        {
            GroupStates[e.Group.Key.ToString()] = false;
        }

        private void OnDataGrid_GroupExpanded(object sender, GroupChangedEventArgs e)
        {
            GroupStates[e.Group.Key.ToString()] = true;
        }

        private void OnView_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var collectionViewWrapper = sender as GridQueryableCollectionViewWrapper;
            if (collectionViewWrapper != null)
            {
                if (collectionViewWrapper.Groups != null && collectionViewWrapper.Groups.Count > 0)
                {
                    foreach (var group in collectionViewWrapper.Groups)
                    {
                        var grp = group as Group;
                        var key = grp.Key.ToString();
                        if (GroupStates.ContainsKey(key))
                        {
                            grp.IsExpanded = GroupStates[key];
                            while (!grp.IsBottomLevel)
                            {
                                var innergroups = grp.Groups;
                                if (innergroups != null && innergroups.Count > 0)
                                {
                                    foreach (var innerGroup in innergroups)
                                    {
                                        if (GroupStates.ContainsKey(innerGroup.Key.ToString()))
                                            innerGroup.IsExpanded = GroupStates[innerGroup.Key.ToString()];
                                        else
                                            GroupStates.Add(innerGroup.Key.ToString(), innerGroup.IsExpanded);
                                        grp = innerGroup;
                                    }
                                }
                            }
                        }
                        else
                        {
                            GroupStates.Add(key, grp.IsExpanded);
                        }
                    }
                }
            }
        }
    }
}
