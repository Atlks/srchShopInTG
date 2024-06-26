using System;
using System.Collections.Generic;

namespace Telegram助手;

/// <summary>
/// Type 0群组  1频道  2机器人 3用户
/// </summary>
public partial class GroupChannelBotUser
{
    public int Id { get; set; }

    public int? Type { get; set; }

    public int? CategoryEnum { get; set; }

    public long? Number { get; set; }

    public string Url { get; set; } = null!;

    public string? Name { get; set; }

    public string? Describle { get; set; }

    public string? PhoneNumber { get; set; }

    public int? Members { get; set; }

    public int? Onlines { get; set; }

    public bool? IsBanned { get; set; }

    public bool? IsAddedGroup { get; set; }

    public bool? IsOnlyContactAddToGroup { get; set; }

    public DateTime? LastCollectionUserTime { get; set; }

    public DateTime? LastDownloadChatTime { get; set; }
}
