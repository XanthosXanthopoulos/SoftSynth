using System.Collections.ObjectModel;

namespace PDADesktop
{
    public class CompanyPageViewModel : BaseViewModel
    {
        #region Private Members



        #endregion


        #region Public Properties

        public ObservableCollection<Company> Companies { get; set; }

        #endregion

        #region Constructors

        public CompanyPageViewModel()
        {
            Companies = new ObservableCollection<Company>();
            Companies.Add(new Company {
                Name = "Company_1",
                Address = "1234 Kamatrelo Street",
                City = "Athens"
            });
            Companies.Add(new Company
            {
                Name = "Company_2",
                Address = "1234 Kamatrelo Street",
                City = "Athens"
            });
            Companies.Add(new Company
            {
                Name = "Company_3",
                Address = "12234 Kamatrelo Street",
                City = "Athens"
            });
            Companies.Add(new Company
            {
                Name = "Company_4",
                Address = "12 Kamatrelo Street",
                City = "Larisa"
            });
            Companies.Add(new Company
            {
                Name = "Company_5",
                Address = "134 Stonecity Street",
                City = "Athens"
            });
        }

        #endregion
    }
}
