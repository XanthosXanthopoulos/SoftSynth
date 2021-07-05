using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Synth
{
    /// <summary>
    /// A base page for all pages to gain basic page functionality
    /// </summary>
    public class BasePage : Page
    {
        #region Public Properties

        /// <summary>
        /// The animation to play when the page is first loaded
        /// </summary>
        public PageAnimation PageLoadAnimation { get; set; } = PageAnimation.ExpandAndFadeIn;

        /// <summary>
        /// The animation to play when the page is unloaded
        /// </summary>
        public PageAnimation PageUnloadAnimation { get; set; } = PageAnimation.ShrinkAndFadeOut;

        /// <summary>
        /// The time any animation takes to complete
        /// </summary>
        public double AnimationSeconds { get; set; } = 0.8;

        /// <summary>
        /// A flag to indicate if this page should animate out on load. Useful for when we are moving the page to another frame
        /// </summary>
        public bool ShouldAnimateOut { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public BasePage()
        {
            //If we are animating in, hide to begin with
            if (PageLoadAnimation != PageAnimation.None) Visibility = Visibility.Collapsed;

            //Listen out for the page loading
            Loaded += BasePage_Loaded;
        }

        #endregion

        #region Animation Load/Unload

        /// <summary>
        /// Once the page is loaded, perform any required animation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BasePage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            //If we are setup to animate out on load
            if (ShouldAnimateOut)
                //Animate the page out
                await AnimateOut();
            else
                //Animate the page in
                await AnimateIn();
        }

        public async Task AnimateIn()
        {
            //Return if no animation required
            if (PageLoadAnimation == PageAnimation.None) return;

            switch (PageLoadAnimation)
            {
                case PageAnimation.ExpandAndFadeIn:
                    await this.ExpandAndFadeIn(0.4, 0.6, 1);
                    break;
            }
        }

        public async Task AnimateOut()
        {
            //Return if no animation required
            if (PageUnloadAnimation == PageAnimation.None) return;

            switch (PageUnloadAnimation)
            {
                case PageAnimation.ShrinkAndFadeOut:
                    await this.ShrinkAndFadeOut(0.4, 1, 0.6);
                    break;
            }
        }

        #endregion
    }

    /// <summary>
    /// A base page with added view model support
    /// </summary>
    /// <typeparam name="VM"></typeparam>
    public class BasePage<VM> : BasePage where VM : BaseViewModel, new()
    {
        #region Private Members

        private VM viewModel;

        #endregion

        #region Public Properties

        /// <summary>
        /// The view model associated with this page
        /// </summary>
        public VM ViewModel
        {
            get
            {
                return viewModel;
            }

            set
            {
                //If nothing has changed return
                if (viewModel == value) return;

                //Update the view model
                viewModel = value;

                //Set the data context for this page
                DataContext = viewModel;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public BasePage() : base()
        {
            //Create default view model
            ViewModel = IoCContainer.Get<VM>();
        }

        #endregion
    }
}
