﻿using System.ComponentModel.DataAnnotations;

namespace EricJansen.Client.Models;

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
