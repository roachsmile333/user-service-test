namespace UserService.Models.User
{
    public class UserViewModel : IUser
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
