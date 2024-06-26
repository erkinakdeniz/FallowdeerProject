﻿using Core.Persistence.Repositories;

namespace Core.Security.Entities;

public class EmailAuthenticator : Entity<int>
{
    public Guid UserId { get; set; }
    public string? ActivationKey { get; set; }
    public bool IsVerified { get; set; }

    public virtual User User { get; set; } = null!;

    public EmailAuthenticator() { }

    public EmailAuthenticator(Guid userId, bool isVerified)
    {
        UserId = userId;
        IsVerified = isVerified;
    }

    public EmailAuthenticator(int id, Guid userId, bool isVerified)
        : base(id)
    {
        UserId = userId;
        IsVerified = isVerified;
    }
}
