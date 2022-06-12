using System;
using System.Windows;
using Microsoft.Win32;
using NapierBankMessaging.Classes;

namespace NapierBankMessaging.Pages
{
    /// <summary>
    /// Author: Conor King - 40532386
    /// Partial Class <c>InputPage</c> handles the events produced by the GUI page counterpart.
    /// This class is a subclass of <c>Page</c> class.
    /// </summary>
    public partial class InputPage
    {
        private OpenFileDialog _fileDialog;
        private Controller _controller = new Controller();
        private MainWindow _mainWindow;
        
        public InputPage(MainWindow main)
        {
            InitializeComponent();
            _mainWindow = main;
        }
        
        public InputPage(MainWindow main, Controller controller)
        {
            InitializeComponent();
            _mainWindow = main;
            _controller = controller;
        }
        
        /// <summary>
        /// Method <c>BrowseButton_OnClick</c> will run when the user selects the browse button.
        /// Takes the selected file name and inputs it into the associated textbox.
        /// </summary>
        private void BrowseButton_OnClick(object sender, RoutedEventArgs e)
        {
            _fileDialog = new OpenFileDialog();
            if (_fileDialog.ShowDialog() == true)
            {
                FileTextbox.Text = _fileDialog.SafeFileName;
            }
        }
        
        /// <summary>
        /// Method <c>FileSubmitButton_OnClick</c> will run when the user selects the submit file button.
        /// Trys to submit the file then catches the null reference exception if cast which means no file selected.
        /// If successful it will call the <c>InputFile</c> method from the controller class.
        /// </summary>
        private void FileSubmitButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _controller.InputFile(_fileDialog.FileName);
            }
            catch (NullReferenceException exception)
            {
                MessageBox.Show("No file selected", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.Write(exception);
            }
            
        }
        
        /// <summary>
        /// Method <c>SubmitMessageButton_OnClick</c> will run when the user selects the submit message button.
        /// Takes the text from the message header and message body fields and calls the <c>MainMethod</c> from
        /// the controller class
        /// </summary>
        private void SubmitMessageButton_OnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            var messageHeader = MessageHeaderTextbox.Text;
            var messageBody = MessageBodyTextbox.Text;

            _controller.MainMethod(messageHeader, messageBody);
        }
        
        /// <summary>
        /// Method <c>StopInputButton_OnClick</c> will run when the user selects the stop input button.
        /// Calls the <c>ViewChange</c> method from the controller class passing in the MainWindow and Controller instance
        /// and also a string with the value "ListPage" to switch views.
        /// </summary>
        private void StopInputButton_OnClick(object sender, RoutedEventArgs e)
        {
            _controller.ViewChange(_mainWindow, _controller,"ListPage");
        }
    }
}