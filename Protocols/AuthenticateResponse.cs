using ApiSkeleton.Entities;

namespace ApiSkeleton.Protocols
{
    public class AuthenticateResponse
    {
        public long Id { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }

        public AuthenticateResponse(User user, string token)
        {
            Id = user.Id;
            NickName = user.NickName;
            Email = user.Email;
            Token = token;
        }
    }
}