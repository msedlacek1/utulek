using System;
using UTB.Eshop.Application.ViewModels;
using UTB.Eshop.Infrastructure.Identity.Enums;

namespace UTB.Eshop.Application.Abstraction
{
    public interface IAccountService
    {
        Task<string[]> Register(RegisterViewModel vm, Roles role);
        Task<bool> Login(LoginViewModel vm);
        Task Logout();
    }
}

