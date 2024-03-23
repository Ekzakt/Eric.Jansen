﻿using System.ComponentModel.DataAnnotations;

namespace Eric.Jansen.Application.Models;

public class ContactViewModel
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Message { get; set; } = string.Empty;
}
