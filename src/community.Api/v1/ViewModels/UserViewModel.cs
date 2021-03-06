﻿using community.Core.Models;

namespace community.Api.v1.ViewModels
{
    // TypeScript: user.ts
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Token { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public UserViewModel()
        {
        }

        public UserViewModel(User user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
        }
    }
}