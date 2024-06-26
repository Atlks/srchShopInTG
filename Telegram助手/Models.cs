using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram助手
{

    //软件应用设置
    public class Settings
    {
        /// <summary>
        /// 是否开机自启动
        /// </summary>
        public bool IsStartUp { get; set; } = true;
    }
    public class Storage
    {
        public List<Origins> origins { get; set; } = null!;
    }

    public class Origins
    {
        public string origin { get; set; } = null!;
        public List<LocalStorage> localStorage { get; set; } = null!;
    }

    public class LocalStorage
    {
        public string name { get; set; } = null!;
        public string value { get; set; } = null!;
    }

    public class AccountContext : AccountUser
    {
        /// <summary>
        /// 当前浏览器句柄
        /// </summary>
        public IntPtr IntPrt { get; set; }
        public IBrowser? Browser { get; set; }
        public IBrowserContext? Context { get; set; }
        //主页面
        public IPage? Page { get; set; }
        public EventHandler<IPage>? CloseHandler { get; set; }

        //是否运行中
        public bool IsRuning { get; set; }
        //是否设置账号中
        public bool IsSetting { get; set; }
       
        /// <summary>
        /// 取消延迟的令牌 
        /// </summary>
        public CancellationTokenSource CancelTimerSource { get; set; } = new CancellationTokenSource();

        /// <summary>
        /// 控制台日志
        /// </summary>
        public StringBuilder Logs { get; set; } = new StringBuilder();

        public bool IsShowLog { get; set; }
        public bool IsShowBrowser { get; set; }
        /// <summary>
        /// 备份登录状态:开始备份|停止备份|不可备份
        /// </summary>
        public string LoginBackupState { get; set; }= "";

        /// <summary>
        /// 开始|启动中|停止|停止中|不可用
        /// </summary>
        public string ActionState { get; set;  } = "";      
    }

    public class AccountUser
    {
        public int Id { get; set; }
        public long UserId { get; set; } = 0;
        public bool IsAccountSeted { get; set; }
        public string Phone { get; set; } = null!;
        public string? UserName { get; set; }
        public string Name { get; set; } = "";
        public int AddMembers { get; set; } = 0;

        /// <summary>
        /// 受限截止时间
        /// </summary>
        public DateTime? DeadlineLimitedTime { get; set; }
        /// <summary>
        /// 是否封禁
        /// </summary>
        public bool IsBanned { get; set; }
        /// <summary>
        /// 是否过期
        /// </summary>
        public bool IsExpired { get; set; } = false;     
        public BrowserNewContextOptions ContextOptions { get; set; } = null!;

        /// <summary>
        /// 今天添加用户数
        /// </summary>
        public int TodayAddedUsers { get; set; } = 0;
        /// <summary>
        /// 最后拉人成功时间
        /// </summary>
        public DateTime? LastAddedUserTime { get; set; }
        /// <summary>
        /// 登录备份数
        /// </summary>
        public int LoginBackups { get; set; }
    }

    public class NetAccountUser : AccountUser
    {
        public new String ContextOptions { get; set; } = "";
    }
}
