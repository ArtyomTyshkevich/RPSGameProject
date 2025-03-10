﻿using Auth.BLL.DTOs.Identity;
using MediatR;

namespace Auth.BLL.Commands
{
    public class RegisterCommand : IRequest<AuthResponse>
    {
        public RegisterRequest RegisterRequest { get; set; }
    }
} 