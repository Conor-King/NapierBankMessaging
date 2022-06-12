using System;
using System.Windows;
using NapierBankMessaging.Classes;

namespace NapierBankMessaging.Pages
{
    /// <summary>
    /// Author: Conor King - 40532386
    /// Partial Class <c>ListPage</c> handles the events produce by the GUI xaml counterpart.
    /// This class is a subclass of <c>Page</c> class.
    /// </summary>
    public partial class ListPage
    {
        private Controller _controller;
        private MainWindow _mainWindow;
        public ListPage(MainWindow mainWindow, Controller controller, MessageInfo messageInfo)
        {
            InitializeComponent();
            _controller = controller;
            _mainWindow = mainWindow;

            TrendingListbox.Items.Add(String.Format("{0,5} {1,30}", "Hashtag", "Count"));
            foreach (var item in messageInfo.TrendingList)
            {
                TrendingListbox.Items.Add(String.Format("{0,-30} {1}", item.Key, item.Value));
            }

            MentionsListbox.Items.Add(String.Format("{0,5} {1,30}", "Twitter ID", "Count"));
            foreach (var item in messageInfo.MentionsList)
            {
                MentionsListbox.Items.Add(String.Format("{0,-30} {1}", item.Key, item.Value));
            }

            SIRListbox.Items.Add(String.Format("{0,5} {1,30}", "Sort Code", "Nature of Incident"));
            foreach (var item in messageInfo.SIRIncidentList)
            {
                SIRListbox.Items.Add(String.Format("{0,-30} {1}", item.Key, item.Value));
                
            }
        }
        
        /// <summary>
        /// Method <c>ExitButton_OnClick</c> runs when the user selects the save and exit button.
        /// Calls the <c>ViewChange</c> method to start the close action.
        /// </summary>
        private void ExitButton_OnClick(object sender, RoutedEventArgs e)
        {
            _controller.ViewChange(_mainWindow, _controller, "Close");
        }

        /// <summary>
        /// Method <c>BackButton_OnClick</c> runs when the user selects the back button.
        /// Calls the <c>ViewChange</c> method to change the view back to the input page.
        /// </summary>
        private void BackButton_OnClick(object sender, RoutedEventArgs e)
        {
            _controller.ViewChange(_mainWindow, _controller, "InputPage");
        }
    }
}