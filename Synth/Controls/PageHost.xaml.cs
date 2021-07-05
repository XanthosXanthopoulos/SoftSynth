using System;
using System.Collections.Generic;
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

namespace Synth
{
    /// <summary>
    /// Interaction logic for PageHost.xaml
    /// </summary>
    /// <summary>
    /// Interaction logic for PageHost.xaml
    /// </summary>
    public partial class PageHost : UserControl
    {
        #region Depedency Properties

        /// <summary>
        /// The current page to show in the page host
        /// </summary>
        public BasePage CurrentPage
        {
            get { return (BasePage)GetValue(curentPageProperty); }
            set { SetValue(curentPageProperty, value); }
        }

        /// <summary>
        /// Register <see cref="CurrentPage"/> as a depedency property
        /// </summary>
        public static readonly DependencyProperty curentPageProperty = DependencyProperty.Register(nameof(CurrentPage), typeof(BasePage), typeof(PageHost), new UIPropertyMetadata(CurrentPagePropertyChanged));

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public PageHost()
        {
            InitializeComponent();
        }

        #endregion

        #region Property Changed Events

        /// <summary>
        /// Called when the <see cref="CurrentPage"/> value has changed
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void CurrentPagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //Get the frames
            Frame oldPageFrame = (d as PageHost).OldPage;
            Frame newPageFrame = (d as PageHost).NewPage;

            //Store the current page content as the old page
            object oldPageContent = newPageFrame.Content;

            //Remove current page from new page frame
            newPageFrame.Content = null;

            //Move the previous page into the old page frame
            oldPageFrame.Content = oldPageContent;

            //Animate out previous page when the loaded event fires
            if (oldPageContent is BasePage oldPage) oldPage.ShouldAnimateOut = true;

            //Set the new page content
            newPageFrame.Content = e.NewValue;
        }

        #endregion
    }
}
