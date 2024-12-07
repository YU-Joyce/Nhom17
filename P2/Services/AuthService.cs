using P2.Models;

namespace P2.Services
{
    public class AuthService
    {
        public User ValidateUser(string username, string password)
        {
            // Kiểm tra người dùng với cơ sở dữ liệu hoặc danh sách tạm thời
            if (username == "test" && password == "password") // Thay thế bằng phương thức mã hóa thực tế
            {
                return new User { IdUser = 1, UserName = username, Password = password };
            }
            return null;
        }
    }
}
