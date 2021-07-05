using System.Windows.Input;

namespace Synth
{
    public class ApplicationViewModel : BaseViewModel
    {
        #region Private Members

        private ApplicationPage currentPage = ApplicationPage.SynthPage;
        private bool sideMenuVisible = false;
        private BaseViewModel currentPageViewModel;

        #endregion

        #region Public Properties

        /// <summary>
        /// The current page of the application
        /// </summary>
        public ApplicationPage CurrentPage
        {
            get
            {
                return currentPage;
            }

            private set
            {
                if (currentPage == value) return;

                currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
            }
        }

        /// <summary>
        /// True if the side menu should be shown
        /// </summary>
        public bool SideMenuVisible
        {
            get
            {
                return sideMenuVisible;
            }

            set
            {
                if (sideMenuVisible == value) return;

                sideMenuVisible = value;
                OnPropertyChanged(nameof(SideMenuVisible));
            }
        }

        public BaseViewModel CurrentPageViewModel
        {
            get
            {
                return currentPageViewModel;
            }

            set
            {
                if (currentPageViewModel == value) return;

                currentPageViewModel = value;
                OnPropertyChanged(nameof(CurrentPageViewModel));
            }
        }


        public bool Logged { get; set; } = false;

        public string Username { get; set; }

        public string Token { get; set; }

        #endregion

        #region Public Commands

        public ICommand GoToPageCommand { get; set; }

        public ICommand SaveCommand { get; set; }

        #endregion

        #region Constructors

        public ApplicationViewModel()
        {
            GoToPageCommand = new RelayParameterizedCommand<ApplicationPage>((parameter) => GoToPage(parameter));
        }

        #endregion

        /// <summary>
        /// Navigate to the specified page
        /// </summary>
        /// <param name="page">The page to go to</param>
        public void GoToPage(ApplicationPage page)
        {
            //Set the current page
            CurrentPage = page;
        }
    }
}
