using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlyoutNavigation;
using FlyoutNavigationDemo;
using FlyoutNavigationDemo.iOS;
using Foundation;
using MonoTouch.Dialog;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MainPage), typeof(MainPageRenderer))]
namespace FlyoutNavigationDemo.iOS
{
    public class MainPageRenderer : TabletMasterDetailRenderer
    {
        FlyoutNavigationController navigation;

        // Data we'll use to create our flyout menu and views:
        string[] Tasks = {
            "Get Xamarin",
            "Learn C#",
            "Write Killer App",
            "Add Platforms",
            "Profit",
            "Meet Obama",
        };

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Create the flyout view controller, make it large,
            // and add it as a subview:
            navigation = new FlyoutNavigationController();
            navigation.Position = FlyOutNavigationPosition.Left;
            navigation.View.Frame = UIScreen.MainScreen.Bounds;
            View.AddSubview(navigation.View);
            this.AddChildViewController(navigation);

            var taskList = new Section("Task List");
            taskList.AddAll(Tasks.Select(x => new StringElement(x)));
            // Create the menu:
            navigation.NavigationRoot = new RootElement("Task List") {
                taskList,
                new Section ("Extras")
                {
                    new BooleanElement("Toggle",true),
                    new StringElement("Swipable Table"),
                    new StringElement("Storyboard"),
                }
            };

            // Create an array of UINavigationControllers that correspond to your
            // menu items:
            var viewControllers = Tasks.Select(x => new UINavigationController(new TaskPageController(navigation, x))).ToList();
            //Add null for the toggle switch
            viewControllers.Add(null);
            viewControllers.Add(new UINavigationController(new SwipableTableView()));
            //Load from Storyboard
            var storyboardVc = CreateViewController<StoryboardViewController>("Main", "StoryboardViewController");
            viewControllers.Add(new UINavigationController(storyboardVc));
            navigation.ViewControllers = viewControllers.ToArray();
        }
        static T CreateViewController<T>(string storyboardName, string viewControllerStoryBoardId = "") where T : UIViewController
        {
            var storyboard = UIStoryboard.FromName(storyboardName, null);
            return string.IsNullOrEmpty(viewControllerStoryBoardId) ? (T)storyboard.InstantiateInitialViewController() : (T)storyboard.InstantiateViewController(viewControllerStoryBoardId);
        }
        class TaskPageController : DialogViewController
        {
            public TaskPageController(FlyoutNavigationController navigation, string title) : base(null)
            {
                Root = new RootElement(title) {
                    new Section {
                        new CheckboxElement (title)
                    }
                };
                NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Action, delegate {
                    navigation.ToggleMenu();
                });
            }
        }
    }
}