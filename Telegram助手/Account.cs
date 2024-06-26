using System;
using System.Collections.Generic;

namespace Telegram助手;

public partial class Account
{
    public int Id { get; set; }

    public long UserId { get; set; }

    public bool IsAccountSeted { get; set; }

    public string Phone { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string ContextOptions { get; set; } = null!;

    public bool IsBanned { get; set; }

    public bool IsExpired { get; set; }

    public DateTime? DeadlineLimitedTime { get; set; }

    public int LoginBackups { get; set; }
}
