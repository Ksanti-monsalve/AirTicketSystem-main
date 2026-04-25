// src/shared/UI/SessionContext.cs
using AirTicketSystem.modules.user.Domain.aggregate;

namespace AirTicketSystem.shared.UI;

public sealed class SessionContext
{
    public User?   CurrentUser { get; private set; }
    public string? CurrentRole { get; private set; }
    public string? CurrentUserName { get; private set; }

    public bool IsAuthenticated => CurrentUser is not null;
    public bool IsAdmin  => CurrentRole == "ADMIN";
    public bool IsClient => CurrentRole == "CLIENTE";

    public int CurrentUserId => CurrentUser?.Id ?? 0;

    public void Login(User user, string roleName)
    {
        CurrentUser     = user;
        CurrentRole     = roleName;
        CurrentUserName = user.Username.Valor;
    }

    public void Logout()
    {
        CurrentUser     = null;
        CurrentRole     = null;
        CurrentUserName = null;
    }
}
