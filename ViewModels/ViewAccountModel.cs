﻿namespace SHOPTV.ViewModels
{
    public class ViewAccountModel
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public IFormFile? AvatarFile { get; set; } 
        public string Avatar { get; set; } = string.Empty;
    }
}
