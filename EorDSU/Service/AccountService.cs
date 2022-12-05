using EorDSU.Models;
using EorDSU.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EorDSU.Service
{
    public class AccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<RegisterViewModel> Register(RegisterViewModel model)
        {
            User user = new User { UserName = model.Login };
            // добавляем пользователя
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // установка куки
                await _signInManager.SignInAsync(user, false);
            }
            return model;
        }

        public async Task<LoginViewModel> Login(LoginViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Login, model.Password, false, false);
            if (result.Succeeded)
            {
                // проверяем, принадлежит ли URL приложению
                if (!string.IsNullOrEmpty(model.ReturnUrl))
                {
                    return model;
                }
            }
            return model;
        }

        public async Task Logout()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
        }
    }
}
