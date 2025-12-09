public class UserService
{
    public async Task<User> GetUserAsync(int id)
    {
        return new User { Id = id, Name = "Sample" };
    }
}