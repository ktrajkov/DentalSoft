namespace DentalSoft.Web.Controllers
{
    using DentalSoft.Data.Models;
    using DentalSoft.Data.Repository.Interfaces;
    using DentalSoft.Web.Controllers.Base;
    using System.Linq;
    using System.Web.Mvc;

    [Authorize]
    public class BaseAutorizationController : BaseController
    {
        public BaseAutorizationController(IBaseRepository<User> userRepository)
        {
            this.userRepository = userRepository;
        }
        protected User CurrentUser
        {
            get
            {
                if (currentUser == null && this.User != null && this.User.Identity.IsAuthenticated)
                {
                    this.currentUser = this.userRepository.All().Where(u => u.UserName == this.User.Identity.Name).FirstOrDefault();
                }
                return this.currentUser;
            }
            set
            {
                this.currentUser = value;
            }
        }

        private readonly IBaseRepository<User> userRepository;

        private User currentUser { get; set; }
    }
}