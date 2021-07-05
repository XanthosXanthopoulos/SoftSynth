using Dna;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PDADesktop
{
    public class RegisterPageViewModel : BaseViewModel
    {
        #region Private Members

        private string errorMessage;
        private string username;
        private string email;
        private bool registerIsRunning;
        private bool registerSuccesfull = true;

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
        /// The useranme of the user
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
        /// The email of the user
        /// </summary>
        public string Email
        {
            get
            {
                return email;
            }

            set
            {
                if (email == value) return;

                email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        /// <summary>
        /// A flag indicating if the login command is running
        /// </summary>
        public bool RegisterIsRunning
        {
            get
            {
                return registerIsRunning;
            }

            set
            {
                if (registerIsRunning == value) return;

                registerIsRunning = value;
                OnPropertyChanged(nameof(RegisterIsRunning));
            }
        }

        /// <summary>
        /// A flag indicating if the register is succesfull.
        /// </summary>
        public bool RegisterSuccesfull
        {
            get
            {
                return registerSuccesfull;
            }

            set
            {
                if (registerSuccesfull == value) return;

                registerSuccesfull = value;
                OnPropertyChanged(nameof(RegisterSuccesfull));
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// The command to login
        /// </summary>
        public ICommand RegisterCommand { get; set; }

        /// <summary>
        /// The command to go to the login page
        /// </summary>
        public ICommand GoToLoginCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public RegisterPageViewModel()
        {
            RegisterCommand = new RelayParameterizedCommand<object>(async (parameter) => await Register(parameter));
            GoToLoginCommand = new RelayCommand(() => GoToLogin());
        }

        #endregion

        /// <summary>
        /// Attempt to register the user.
        /// </summary>
        /// <param name="parameter">The parameter passed in for extracting the user credentials.</param>
        /// <returns></returns>
        public async Task Register(object parameter)
        {
            RegisterSuccesfull = true;

            await RunCommand(() => RegisterIsRunning, async () =>
            {
                // Call the server and attempt to register with the provided credentials
                var result = await WebRequests.PostAsync<ApiResponse<UserProfileApiModel>>("https://localhost:5001/api/register", new RegisterCredentialsApiModel
                    {
                        Username = Username,
                        Email = Email,
                        Password = (parameter as IHavePassword).SecurePassword.Unsecure(),
                        ConfirmPassword = (parameter as IHavePassword).ConfirmSecurePassword.Unsecure()
                    });

                //If there was no response, bad data or a responce with an error message...
                if (result == null || result.ServerResponse == null || !result.ServerResponse.Successful)
                {
                    //Set default error message and login flag to false
                    RegisterSuccesfull = false;
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
        public void GoToLogin()
        {
            IoCContainer.Get<ApplicationViewModel>().GoToPage(ApplicationPage.Login);
        }
    }
}
