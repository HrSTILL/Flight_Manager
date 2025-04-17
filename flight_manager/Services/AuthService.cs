using System.Linq;
using flight_manager.Models;

namespace flight_manager.Services
{
    public class AuthService
    {
        private readonly FlightManagerDbContext _context;

        public AuthService(FlightManagerDbContext context)
        {
            _context = context;
        }

        public StaffMembers? Login(LoginModel loginModel, out string token)
        {
            var plainPassword = loginModel.Password;

            var user = _context.StaffMembers
                .FirstOrDefault(u => u.Username == loginModel.Username && u.Password == plainPassword);

            if (user == null)
            {
                token = "";
                return null;
            }

            Guid guid = Guid.NewGuid();
            token = guid.ToString();

            _context.LoginTokens.Add(new LoginToken { Token = token, UID = user.Id });
            _context.SaveChanges();

            ManageLoginTokens();

            return user;
        }

        public string GetRankFromTokenCookie(HttpRequest request)
        {
            var cookie = request.Cookies.FirstOrDefault(x => x.Key == "login_tok");

            if (cookie.Key == null || cookie.Value == null)
            {
                return "guest";
            }

            string tok = cookie.Value;

            LoginToken? tokModel = _context.LoginTokens.Where(x => x.Token == tok).FirstOrDefault();
            if (tokModel == null)
            {
                return "guest";
            }

            StaffMembers? member = _context.StaffMembers.Where(x => x.Id == tokModel.UID).FirstOrDefault();
            if (member == null)
            {
                return "guest";
            }

            return member.Rank;
        }

        private void ManageLoginTokens()
        {
            var tokenCount = _context.LoginTokens.Count();

            if (tokenCount > 100)
            {
                var tokensToDelete = tokenCount - 1;
                var tokensToRemove = _context.LoginTokens
                    .OrderBy(t => t.Token) 
                    .Take(tokensToDelete)
                    .ToList();

                _context.LoginTokens.RemoveRange(tokensToRemove);
                _context.SaveChanges();
            }
        }

        private string HashPassword(string password)
        {
            return password;
        }
    }
}
