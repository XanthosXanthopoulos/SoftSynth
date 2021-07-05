using Dna;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PDADesktop
{
    public class LoginPageViewModel : BaseViewModel
    {
        #region Private Members
        
        private string errorMessage;
        private string username;
        private bool loginIsRunning;
        private bool loginSuccesfull = true;

        #endregion

        #region Public Properties

        /// <summary>
        /// The error message to show the user in case of problem with server
        /// </summary>
        public string ErrorMessage
        {
            get
            {
                return errorMessage;
            }

            set
            {
                if (errorMessage == value) return;

                errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        /// <summary>
        /// The email of the user
        /// </summary>
        public string Username
        {
            get
            {
                return username;
            }

            set
            {
                if (username == value) return;

                username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        /// <summary>
        /// A flag indicating if the login command is running
        /// </summary>
        public bool LoginIsRunning
        {
            get
            {
                return loginIsRunning;
            }

            set
            {
                if (loginIsRunning == value) return;

                loginIsRunning = value;
                OnPropertyChanged(nameof(LoginIsRunning));
            }
        }

        /// <summary>
        /// A flag indicating if the login is succesfull.
        /// </summary>
        public bool LoginSuccesfull
        {
            get
            {
                return loginSuccesfull;
            }

            set
            {
                if (loginSuccesfull == value) return;

                loginSuccesfull = value;
                OnPropertyChanged(nameof(LoginSuccesfull));
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// The command to login
        /// </summary>
        public ICommand LoginCommand { get; set; }

        /// <summary>
        /// The command to go to the login page
        /// </summary>
        public ICommand GoToRegisterCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public LoginPageViewModel()
        {
            LoginCommand = new RelayParameterizedCommand<object>(async (parameter) => await Login(parameter));
            GoToRegisterCommand = new RelayCommand(() => GoToRegister());
        }

        #endregion

        /// <summary>
        /// Attempt to log the user in
        /// </summary>
        /// <param name="parameter">The parameter passed in for extracting the user credentials.</param>
        /// <returns></returns>
        public async Task Login(object parameter)
        {
            LoginSuccesfull = true;

            await RunCommand(() => LoginIsRunning, async () =>
            {
                //Call the server and attempt to log in with credentials 
                //TODO: Move all URLs and routes to static class in core
                var result = await WebRequests.PostAsync<ApiResponse<UserProfileApiModel>>("https://localhost:5001/api/login", new LogInCredentialsApiModel
                {
                    Username = Username,
                    Password = (parameter as IHavePassword).SecurePassword.Unsecure()
                });

                //If there was no response, bad data or a responce with an error message...
                if (result == null || result.ServerResponse == null || !result.ServerResponse.Successful)
                {
                    //Set default error message and login flag to false
                    LoginSuccesfull = false;
                    ErrorMessage = "Unknown error from server call";

                    //If we got a response from server...
                    if (result?.ServerResponse != null)
                    {
                        ErrorMessage = result.ServerResponse.ErrorMessage;
                    }
                    //Else if we have result but deserialize failed...
                    else if (!string.IsNullOrWhiteSpace(result?.RawServerResponse))
                    {
                        ErrorMessage = $"Unexpected response from server. {result.RawServerResponse}";
                    }
                    //Else if we have a result but no server response...
                    else if (result != null)
                    {
                        ErrorMessage = $"Failed to communicate with server. Status code {result.StatusCode}. {result.StatusDescription}";
                    }

                    return;
                }       
                else
                {
                    IoCContainer.Get<ApplicationViewModel>().Token = result.ServerResponse.Response.Token;
                    IoCContainer.Get<ApplicationViewModel>().GoToPage(ApplicationPage.Overview);
                }
            });
        }

        /// <summary>
        /// Gets the user to the register page
        /// </summary>
        public void GoToRegister()
        {
            IoCContainer.Get<ApplicationViewModel>().GoToPage(ApplicationPage.Register);
        }
    }
}
