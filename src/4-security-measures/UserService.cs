// In this class we implement a register and login method. The register method registers a new user aslong as we supply a secret registering key (from application configuration). It then hashes and encrypts the password. It then stores the password in the database. For the login method we compare the hashed password to accept the login or not.

// Further down a JWT token is created with authorization and authentication, using a Bearer Token. This is used for authentication for the endpoints with authorization based on a role.

public sealed class UserService
{
    private readonly DbContext _authContext;
    private readonly AppSettings _appSettings;

    public UserService(AuthContext authContext, IOptions<AppSettings> appSettings)
    {
        _authContext = authContext;
        _appSettings = appSettings.Value;
    }

    public bool Register(string username, string password, string registerKey)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(registerKey) || registerKey != _appSettings.RegisteringKey)
        {
            return false;
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

        if (passwordHash == null)
        {
            return false;
        }

        var apiUser = new ApiUser()
        {
            Username = username,
            PasswordHash = passwordHash,
            IsActive = true,
        };

        _authContext.ApiUsers.Add(apiUser);
        _authContext.SaveChanges();

        return true;
    }

    public string Login(string username, string password)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            throw new ArgumentException("Invalid username or password.");
        }

        var apiUser = _authContext.ApiUsers
            .Include(apiUsers => apiUsers.ApiUserRoles)
            .ThenInclude(apiUserRoles => apiUserRoles.ApiRole)
            .FirstOrDefault(x => x.Username == username);

        if (apiUser == null)
        {
            throw new ArgumentException("User or password incorrect.");
        }

        if (!BCrypt.Net.BCrypt.Verify(password, apiUser.PasswordHash))
        {
            throw new ArgumentException("User or password incorrect.");
        }

        if (apiUser.ApiUserRoles?.Count == 0)
        {
            throw new MethodAccessException("User has no roles.");
        }

        var userRoles = apiUser.ApiUserRoles?.Select(d => d.ApiRole.Name).ToList() ?? new List<string>();

        return CreateToken(apiUser.Username, userRoles);
    }

    private string CreateToken(string username, List<string> roles)
    {
        if (roles == null)
        {
            throw new MethodAccessException("User has no roles.");
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, username),
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JwtCreationKey));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}
