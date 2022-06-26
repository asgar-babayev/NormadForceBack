﻿using System.ComponentModel.DataAnnotations;

namespace NormadForceProject.Areas.Manage.ViewModels
{
    public class SignInVm
    {
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
