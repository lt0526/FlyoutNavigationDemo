using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

namespace FlyoutNavigationDemo.iOS
{
    public class SwipableTableView : UITableViewController
    {
        public SwipableTableView()
        {

        }
        public override void LoadView()
        {
            base.LoadView();
            TableView.Source = new MyDataSource();
        }

        class MyDataSource : UITableViewSource
        {
            public List<string> Rows;
            public MyDataSource()
            {
                Rows = Enumerable.Range(0, 20).Select(x => string.Format("Row {0}", x)).ToList();
            }
            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                var cell = tableView.DequeueReusableCell("cell") ?? new UITableViewCell(UITableViewCellStyle.Default, "cell");
                cell.TextLabel.Text = Rows[indexPath.Row];
                return cell;
            }
            public override nint RowsInSection(UITableView tableview, nint section)
            {
                return (nint)Rows.Count;
            }

            public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, NSIndexPath indexPath)
            {
                return UITableViewCellEditingStyle.Delete;
            }
            public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
            {
                return true;
            }
            public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
            {
                if (editingStyle == UITableViewCellEditingStyle.Delete)
                {
                    Rows.RemoveAt(indexPath.Row);
                }
            }
        }
    }
}
