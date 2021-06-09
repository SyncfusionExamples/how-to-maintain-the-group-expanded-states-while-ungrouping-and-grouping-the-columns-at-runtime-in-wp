# How to maintain the group expanded states while ungrouping and grouping the columns at runtime in WPF DataGrid (SfDataGrid)?

## About the sample

This example illustrates how to maintain the group expanded states while ungrouping and grouping the columns at runtime in WPF DataGrid.

[WPF DataGrid](https://www.syncfusion.com/wpf-controls/datagrid) (SfDataGrid) allows you to maintain the expanded state of the group while ungrouping and grouping the columns at runtime by handling the events SfDataGrid.View.CollectionChanged, SfDataGrid.GroupExpanded and SfDataGrid.GroupCollapsed.

```C#

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

```

![Grouping](Grouping.gif)

## Requirements to run the demo 

Visual Studio 2015 and above versions.