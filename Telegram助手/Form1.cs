using AutoMapper;
using Microsoft.Playwright;
using Newtonsoft.Json;
using ShaMoGuNameSpace;
using System.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;
using TinyPinyin;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using RestSharp;
using System.Text;
using HtmlAgilityPack;
using System.Data.Entity;
using System.Net.Mime;
using System.Net.Http;
using System.Text.Encodings.Web;

namespace Telegram助手
{
    public partial class Form : System.Windows.Forms.Form
    {
        IPlaywright _playwright = null!;
        List<AccountContext> _accountContexts = new();
        HttpClient _httpClient = new();
        string _domain = "http://156.247.9.173/";

        DateTime FormLoadTime = DateTime.Now;
        Settings? _settings;
        //第二天
        DateTime _tomorrow;
        //登录备份数
        int _loginBackups = 20;
        //一次同时执行数
        int _actions = 5;
        /// <summary>
        /// 换IP方式 
        ///<para>0 拨号换动态IP</para>
        ///<para>1 免费代理换IP</para>
        ///<para>2 付费代理换IP</para>
        /// </summary>
        int _changeIpType = 0;
        HashSet<string> _removeKeywords = new();
        HashSet<string> _names = new();
        HashSet<string> _introduces = new();
        HashSet<string> _userAgents = new();
        HashSet<string> _keywords = new();
        // 不用再采集的用户Id
        HashSet<long> _noNeedCollectionUserIds = new();
        public Form()
        {
            InitializeComponent();
            //创建控制台
            Window.AllocConsole();
            //隐藏控制台
            Window.ShowWindow(Window.GetConsoleWindow(), 0);
            //隐藏控制台窗口的关闭按钮
            Window.HideConsoleCloseButton();
        }

        //启动
        private async void Form_Load(object sender, EventArgs e)
        {
            this.Enabled = false;
            _httpClient.Timeout = TimeSpan.FromMinutes(20);
            comboBox_changeIp.SelectedIndex = 0;
            _playwright = await Playwright.CreateAsync();
            _names = (await File.ReadAllLinesAsync("names.txt")).ToHashSet();
            _removeKeywords = (await File.ReadAllLinesAsync(Application.StartupPath + "removeKeywords.txt")).ToHashSet();
            _introduces = (await File.ReadAllLinesAsync("introduces.txt")).ToHashSet();
            _userAgents = (await File.ReadAllLinesAsync(Application.StartupPath + "UserAgents.txt")).ToHashSet();
            var newGroups = JsonConvert.DeserializeObject<List<GroupChannelBotUser>>(await File.ReadAllTextAsync("newCollectionLinks.json"));

            var ncids = await System.IO.File.ReadAllLinesAsync("nonCompliantUserIds.txt");
            for (int i = 0; i < ncids.Length; i++)
            {
                if (long.TryParse(ncids[i], out long parsedValue))
                    _noNeedCollectionUserIds.Add(parsedValue);
            }
            //var allUserRes = string.Empty;
            //try
            //{
            //    allUserRes = await _httpClient.GetStringAsync("http://156.247.9.173/GroupChannelBotUser?categoryEnum=1&type=3");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("请求缅甸成员时出错" + ex.Message);
            //    return;
            //}

            //var allUser = JsonConvert.DeserializeObject<List<GroupChannelBotUser>>(allUserRes);
            //foreach (var user in allUser)
            //{
            //    var userId = Convert.ToInt64(user.Number);
            //    if (!_noNeedCollectionUserIds.Contains(userId))
            //        _noNeedCollectionUserIds.Add(userId);
            //}

            #region 是否设置开机启动            
            var isSetStartUp = false;
            if (!File.Exists(Application.StartupPath + "Settings.json"))
            {
                _settings = new Settings { IsStartUp = true };
                await File.WriteAllTextAsync(Application.StartupPath + "Settings.json", JsonConvert.SerializeObject(_settings));
            }
            else
            {
                var readSettings = await File.ReadAllTextAsync(Application.StartupPath + "Settings.json");

                try
                {
                    _settings = JsonConvert.DeserializeObject<Settings>(readSettings);
                    isSetStartUp = _settings.IsStartUp;
                }
                catch
                {
                    _settings = new Settings { IsStartUp = true };
                }
            }

            if (_settings.IsStartUp)
            {
                if (!Helper.IsStartupSet())
                    Helper.SetStartup();

                await File.WriteAllTextAsync(Application.StartupPath + "Settings.json", JsonConvert.SerializeObject(_settings));
                StartRunToolStripMenuItem.Text = "关闭自动启动";
            }
            else
            {
                StartRunToolStripMenuItem.Text = "开机自启动";
            }
            #endregion

            var accounts = await Helper.GetServiceAccounts();
            AddAccountToGlobal(accounts);

            if (File.Exists("缅甸群关键词.txt"))
                _keywords = (await File.ReadAllLinesAsync("缅甸群关键词.txt")).ToHashSet();
            if (File.Exists("groups.json"))
            {
                var g = await File.ReadAllTextAsync("groups.json");
                _groups = JsonConvert.DeserializeObject<HashSet<GroupChannelBotUser>>(g);
                button_getAllGroups.Text = "更新全部群";
                dataGridView_groups.RowCount = _groups.Count;
            }

            #region 刷新数据
            var data = async () =>
            {
                if (_accountContexts.Any())
                {
                    var accounts = _accountContexts.Count();
                    var addedUsers = _accountContexts.Select(u => u.AddMembers).Sum();
                    var todayAddedUsers = _accountContexts.Select(u => u.TodayAddedUsers).Sum();
                    var efficientAccounts = _accountContexts.Count(u => u.DeadlineLimitedTime == null && !u.IsExpired);
                    var limitAccounts = _accountContexts.Count(u => u.DeadlineLimitedTime != null);
                    var totalTime = Helper.FormatTimeSpan(DateTime.Now - FormLoadTime);
                    var noBackups = _accountContexts.Count(u => u.LoginBackups < _loginBackups + 1 && !u.IsExpired);
                    var waitUpdates = _accountContexts.Count(u => u.IsExpired && u.LoginBackups > 0);

                    try
                    {
                        var systemJson = await _httpClient.GetStringAsync(_domain + "System");
                        dynamic system = JsonConvert.DeserializeObject(systemJson);
                        toolStripStatusLabel1.Text = $"运行{totalTime}  |  账号{accounts}   有效{efficientAccounts}   待备{noBackups}   待更{waitUpdates}   受限{limitAccounts}   |   待拉{system.WaitAddUsers}   已拉{addedUsers}   日拉{todayAddedUsers}  |  群数{system.Groups}  缅群{system.GroupCategoryEnum1}  下载群{system.DownloadChatGroups}  未知链{system.NoKnowns}  封群{system.BannedGroups}  封频{system.BannedChannels}";
                    }
                    catch (Exception ec)
                    {
                        Debug.WriteLine("更新底部状态栏时出错:" + ec.Message);
                    }

                    //如果是第二天,就刷新整个列表的今日拉人数
                    if (DateTime.Now.Date == _tomorrow.Date && dataGridView.Rows.Count > 0)
                    {
                        foreach (var item in _accountContexts)
                        {
                            item.TodayAddedUsers = 0;
                            AddConsoleLog(item, "第二天换天了,把今日添加用户数设为0");
                        }

                        _tomorrow = DateTime.Today.AddDays(1);
                    }
                }

            };
            await data();
            //第二天日期
            _tomorrow = DateTime.Today.AddDays(1);
            var timer = new System.Timers.Timer(10000);
            timer.Elapsed += async (sender, e) =>
            {
                await data();
            };
            timer.Start();
            #endregion

            if (isSetStartUp)
                button_joinUser_Click(sender, e);

            this.Enabled = true;
        }
        //添加数据至全局和表格
        private async void AddAccountToGlobal(List<AccountUser> accountUsers)
        {
            foreach (var user in accountUsers)
            {
                //已存在
                if (_accountContexts.Any(u => u.UserId == user.UserId))
                    continue;

                await AddAccount(new MapperConfiguration(cfg => { cfg.CreateMap<AccountUser, AccountContext>(); }).CreateMapper().Map<AccountContext>(user));
            }
        }
        //添加账号
        private async Task<AccountContext> AddAccount(AccountContext ac)
        {
            if (ac.DeadlineLimitedTime != null && ac.DeadlineLimitedTime < DateTime.Now)
                ac.DeadlineLimitedTime = null;

            var _ac = _accountContexts.Find(u => u.UserId == ac.UserId);
            //已经存在
            if (_ac != null)
            {
                _ac.IsRuning = false;
                _ac.Logs.Clear();
                _ac.IsAccountSeted = ac.IsAccountSeted;
                _ac.Phone = ac.Phone;
                _ac.UserName = ac.UserName;
                _ac.Name = ac.Name;
                _ac.AddMembers = ac.AddMembers;
                _ac.DeadlineLimitedTime = ac.DeadlineLimitedTime;
                _ac.IsExpired = ac.IsExpired;
                _ac.ContextOptions = ac.ContextOptions;
                _ac.TodayAddedUsers = ac.TodayAddedUsers;
                _ac.LastAddedUserTime = ac.LastAddedUserTime;
                _ac.IsBanned = ac.IsBanned;
                await DisposeAccountBrowser(ac, false);
                return _ac;
            }
            else
            {
                dataGridView.Invoke((MethodInvoker)delegate { dataGridView.RowCount++; });

                if (_accountContexts.Count(u => u.ContextOptions?.Proxy?.Server == ac.ContextOptions?.Proxy?.Server) >= 3 || string.IsNullOrEmpty(ac.ContextOptions?.Proxy?.Server))
                    ac.ContextOptions.Proxy = await GetProxy(ac);

                _accountContexts.Add(ac);

                if (_accountContexts.Count < 10)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        this.MaximumSize = new Size(this.Width, this.Height + 25);
                        this.Height += 25;
                    });
                }
                return ac;
            }
        }
        //渲染账号表格数据
        private void dataGridView_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView.RowCount && e.ColumnIndex >= 0 && e.ColumnIndex < dataGridView.ColumnCount && _accountContexts.Count > e.RowIndex)
            {
                var ac = _accountContexts[e.RowIndex];
                var dgv = (DataGridView)sender;
                var row = dgv.Rows[e.RowIndex];
                var cell = row.Cells[e.ColumnIndex];

                switch (e.ColumnIndex)
                {
                    //序号
                    case 0:
                        e.Value = e.RowIndex + 1;
                        break;
                    //Id
                    case 1:
                        e.Value = ac.Id;
                        break;
                    //手机号
                    case 2:
                        e.Value = ac.Phone;
                        break;
                    //用户名
                    case 3:
                        e.Value = ac.UserName;
                        break;
                    //名称
                    case 4:
                        e.Value = ac.Name;
                        break;
                    //拉人受限截止时间
                    case 5:
                        if (ac.DeadlineLimitedTime != null && ac.DeadlineLimitedTime < DateTime.Now)
                            ac.DeadlineLimitedTime = null;

                        e.Value = ac.DeadlineLimitedTime == null ? "" : ac.DeadlineLimitedTime.Value.ToString("M月d日 H:mm");
                        break;
                    //封禁
                    case 6:
                        e.Value = ac.IsBanned ? "封禁" : "";
                        break;
                    //日志
                    case 7:
                        e.Value = ac.IsShowLog;
                        break;
                    //窗口
                    case 8:
                        e.Value = ac.IsShowBrowser;
                        cell.Style.BackColor = cell.ReadOnly ? Color.LightGray : Color.White;
                        break;
                    //备份登录
                    case 9:
                        cell.ReadOnly = ac.IsExpired || ac.IsBanned || ac.LoginBackups >= _loginBackups;
                        cell.Style.BackColor = cell.ReadOnly ? Color.LightGray : Color.White;
                        if (string.IsNullOrEmpty(ac.LoginBackupState))
                        {
                            ac.LoginBackupState = ac.IsBanned || ac.LoginBackups >= _loginBackups ? "不可备份" : "开始备份";
                        }
                        else
                        {
                            ac.LoginBackupState = ac.LoginBackupState;
                        }
                        e.Value = ac.LoginBackupState + ac.LoginBackups;
                        break;
                    //执行
                    case 10:
                        cell.ReadOnly = ac.IsExpired || ac.IsBanned;
                        cell.Style.BackColor = cell.ReadOnly ? Color.LightGray : Color.White;
                        if (string.IsNullOrEmpty(ac.ActionState))
                        {
                            ac.ActionState = ac.IsExpired || ac.IsBanned ? "不可用" : "开始";
                        }
                        else
                        {
                            ac.ActionState = ac.ActionState;
                        }
                        e.Value = ac.ActionState;
                        break;
                    //今日拉人数、操作次数
                    case 11:
                        e.Value = ac.TodayAddedUsers + "/" + ac.AddMembers;
                        break;
                    default:
                        break;
                }
                if (ac.IsRuning)
                {
                    row.DefaultCellStyle.BackColor = Color.LightGreen;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = !ac.IsExpired ? Color.White : Color.LightGray;
                }
            }
        }
        //生成浏览器参数
        private BrowserNewContextOptions CreatOptions()
        {
            var GetRandomBool = () => new Random().Next(2) == 0;
            var GetRandomDouble = (double minValue, double maxValue) => new Random().NextDouble() * (maxValue - minValue) + minValue;
            var screenSize = new ScreenSize { Width = new Random().Next(1400, 1600), Height = new Random().Next(800, 1000) };

            return new BrowserNewContextOptions
            {
                DeviceScaleFactor = (float)GetRandomDouble(1, 3),
                HasTouch = GetRandomBool(),
                UserAgent = _userAgents.ElementAt(new Random().Next(_userAgents.Count)),
                AcceptDownloads = GetRandomBool(),
                ViewportSize = new ViewportSize { Width = new Random().Next(1380, screenSize.Width), Height = new Random().Next(800, 1000) },
                ScreenSize = screenSize
            };
        }

        //从后台返回的代理集合
        List<ProxyIp> resultProxys = new();
        //检索本地账号里的代理IP集索引
        int findLocalIndex = 0;
        //检索服务器返回的代理IP集索引
        int findServerIndex = 0;
        //获取代理IP
        private async Task<Proxy> GetProxy(AccountContext ac)
        {
            //if (_changeIpType == 1)
            //{

            //}
            //else if (_changeIpType == 2)
            //{

            //}

            Proxy? proxy = null;
            if (_accountContexts != null && _accountContexts.Any())
            {
                //获取代理IP时,优先从没到3个账号的本地代理里获取(还是没有再获取后台)
                var localProxy = _accountContexts
                    .Where(u => u.ContextOptions?.Proxy?.Server != ac.ContextOptions?.Proxy?.Server)
                    .GroupBy(x => x.ContextOptions?.Proxy?.Server)
                    .Where(g => g.Count() < 3)
                    .SelectMany(g => g)
                    .OrderBy(o => Guid.NewGuid())
                    .Skip(findLocalIndex)
                    .FirstOrDefault();

                if (localProxy != null)
                {
                    AddConsoleLog(ac, "从本地获取到IP代理:" + localProxy.ContextOptions?.Proxy?.Server);
                    proxy = localProxy?.ContextOptions?.Proxy;
                    findLocalIndex++;
                    return proxy;
                }
            }

        reacquire:
            if (resultProxys == null || !resultProxys.Any())
            {
                string result = string.Empty;
                try
                {
                    result = await _httpClient.GetStringAsync(_domain + "ProxyIp");
                }
                catch
                {
                }

                if (string.IsNullOrEmpty(result))
                {
                    AddConsoleLog(ac, "未成功从服务器获取到IP代理,返回的是空字符串");
                    goto reacquire;
                }

                var res = JsonConvert.DeserializeObject<List<ProxyIp>>(result);

                if (res == null || !res.Any())
                {
                    AddConsoleLog(ac, "未成功从服务器获取到IP代理,解析代理集合字符串时未发现有代理IP地址");
                    goto reacquire;
                }
                foreach (var item in res)
                {
                    var service = item.Ip + ":" + item.Port;
                    //不存在,或者使用数在三次以内的
                    if (_accountContexts == null || _accountContexts.Count == 0 || _accountContexts.Count(u => u?.ContextOptions?.Proxy?.Server == service) < 3)
                    {
                        resultProxys?.Add(item);
                    }
                }

                AddConsoleLog(ac, "成功从服务器获取到" + resultProxys?.Count + "个可选IP代理");
                findServerIndex = 0;
            }

            if (resultProxys != null && resultProxys.Any())
            {
                for (int i = findServerIndex; i < resultProxys.Count; i++)
                {
                    var service = resultProxys[i].Ip + ":" + resultProxys[i].Port;
                    //没有用到3个账号的,就可以用
                    if (_accountContexts == null || _accountContexts.Count == 0 || _accountContexts.Count(u => u.ContextOptions != null && u.ContextOptions.Proxy != null && u.ContextOptions.Proxy.Server == service) < 3)
                    {
                        proxy = new Proxy { Server = service };
                        AddConsoleLog(ac, $"从服务器集合的第[{i}]个索引里获取到代理IP:{service}");
                        findServerIndex = i + 1;
                        break;
                    }
                }
            }

            if (proxy == null)
            {
                AddConsoleLog(ac, "返回的代理IP集合里没发现一个合适的可用代理IP");
                resultProxys = new List<ProxyIp>();
                goto reacquire;
            }

            return proxy;
        }

        //新建页面
        private async Task NewAccountBrowser(AccountContext ac)
        {
        Start:
            await DisposeAccountBrowser(ac, false);
            if (_changeIpType == 0)
            {
                ac.ContextOptions.Proxy = null;
                AddConsoleLog(ac, "清空Proxy，不使用代理");
            }

            ac.Browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false, Proxy = ac.ContextOptions.Proxy });
            ac.Context = await ac.Browser.NewContextAsync(ac.ContextOptions);
            ac.Page = await ac.Context.NewPageAsync();
            ac.CloseHandler = async (sender, e) =>
            {
                if (ac.UserId != 0)
                {
                    await NewAccountBrowser(ac);
                }
                else
                {
                    await DisposeAccountBrowser(ac, false);
                }
            };
            ac.Page.Close += ac.CloseHandler;
            ac.Page.SetDefaultNavigationTimeout(60000);
            //显示浏览器窗口:添加账号时||勾选了显示窗口时||全部开始时勾选了显示浏览器
            var showBrowser = ac.UserId == 0 || ac.IsShowBrowser;
            await Task.Delay(500);
            ac.IntPrt = Process.GetProcessesByName("Chrome").Last(u => Window.IsWindowVisible(u.MainWindowHandle)).MainWindowHandle;
            Window.ShowWindow(ac.IntPrt, showBrowser ? 5 : 0);

            var opens = 0;
            IResponse? response = null;
        openPage:
            try
            {
                response = await ac.Page.GotoAsync("https://web.telegram.org/k/", new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle });
            }
            catch
            {
            }

            //成功初始化浏览器
            if (response != null && response.Ok)
            {
                AddConsoleLog(ac, "初始化浏览器成功");
            }
            else
            {
                AddConsoleLog(ac, "初始化浏览器失败,进行新一轮重新打开");
                opens++;
                if (opens < 4)
                {
                    AddConsoleLog(ac, $"继续重试第{opens}次");
                    goto openPage;
                }

                if (_changeIpType != 0)
                {
                    ac.ContextOptions.Proxy = await GetProxy(ac);
                    AddConsoleLog(ac, $"换代理IP{ac.ContextOptions.Proxy.Server}重试");
                    goto Start;
                }
                else
                {
                    AddConsoleLog(ac, $"拨号换IP重试");
                    ac.ContextOptions.Proxy = null;
                    await ChangeIp();
                    goto openPage;
                }
            }
        }
        //导航至
        private async Task<bool> GotoAsync(AccountContext ac, string url)
        {
            //重试打开网页次数:超过10次还打不开就要换IP了
            var openPageRetrys = 0;
        openPage:
            try
            {
                await ac.Page.GotoAsync("about:blank");
                await ac.Page.GotoAsync(url, new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle, Timeout = 80000 });
                AddConsoleLog(ac, "打开了:" + url);
            }
            catch
            {
                //继续重试
                if (openPageRetrys <= 11)
                {
                    openPageRetrys++;
                    AddConsoleLog(ac, "未打开" + url + ",进行第" + openPageRetrys + "次重试");
                }
                else
                {
                    AddConsoleLog(ac, "未打开" + url + ",10次重试都未打开,换IP继续");

                    if (_changeIpType != 0)
                        ac.ContextOptions.Proxy = await GetProxy(ac);
                    await NewAccountBrowser(ac);
                }
                goto openPage;
            }

            try
            {
                await ac.Page.EvaluateAsync($"document.title = '{ac.Name}'");
            }
            catch
            {
            }

            if (ac.UserId != 0)
            {
                try
                {
                    await ac.Page.GetByText("Qr Code").WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 15000 });
                    ac.IsExpired = true;
                    AddConsoleLog(ac, "登录失效:发现二维码页面");
                }
                catch
                {
                    ac.IsExpired = false;
                    AddConsoleLog(ac, "确认有效:未发现二维码");
                }

                if (ac.IsExpired)
                {
                    if (ac.LoginBackups > 0)
                    {
                        var isUpdateSuccess = await Update(ac);
                        if (isUpdateSuccess)
                        {
                            //备份登录状态
                            await Blackup(ac);
                            goto openPage;
                        }
                    }

                    try
                    {
                        await _httpClient.GetStringAsync(_domain + "Accounts/Expired?id=" + ac.UserId);
                        AddConsoleLog(ac, $"成功向后台告知【{ac.Name}:{ac.UserName}】账号登录已失效");
                    }
                    catch (Exception ex)
                    {
                        AddConsoleLog(ac, $"未成功向后台告知【{ac.Name}:{ac.UserName}】账号登录已失效:" + ex.Message);
                    }

                    ac.IsRuning = false;
                    ac.ActionState = "开始";
                    ac.LoginBackupState = "不可备份";
                    await DisposeAccountBrowser(ac, true);
                    return false;
                }
            }

            return true;
        }
        //点击添加账号
        private async void button_creatUser_Click(object sender, EventArgs e)
        {
            this.Enabled = true;
            Console.Clear();
            this.WindowState = FormWindowState.Minimized;

            //一次性登录20个
            for (int i = 0; i < 1; i++)
            {
                new Thread(async () =>
                {
                    var ac = new AccountContext();
                    ac.ContextOptions = CreatOptions();

                    //if (_changeIpType == 0)
                    //{
                    //    button_creatUser.Text = "换IP中";
                    //    await ChangeIp();
                    //    button_creatUser.Text = "添加账号";
                    //}
                    //else
                    //{
                    //    ac.ContextOptions.Proxy = await GetProxy(ac);
                    //}

                    await NewAccountBrowser(ac);

                    var isLogin = false;
                    try
                    {
                        await ac.Page.WaitForSelectorAsync("button.btn-circle.rp.btn-corner.z-depth-1.btn-menu-toggle", new PageWaitForSelectorOptions { State = WaitForSelectorState.Attached, Timeout = 3600000 });
                        isLogin = true;
                    }
                    catch
                    {
                    }

                    if (!isLogin)
                    {
                        await DisposeAccountBrowser(ac, true);
                    }
                    else
                    {
                        Thread.Sleep(2500);
                        var storageState = await ac.Context.StorageStateAsync();
                        var storage = JsonConvert.DeserializeObject<Storage>(storageState);
                        var ls = storage.origins[0].localStorage;
                        ls = ls.Where(u => !u.name.Equals("tt-global-state")).ToList();
                        ac.ContextOptions.StorageState = JsonConvert.SerializeObject(storage);
                        var userAuth = ls.Where(u => u.name.Equals("user_auth")).First().value;
                        ac.UserId = Convert.ToInt64(JsonConvert.DeserializeAnonymousType(userAuth, new { dcID = "", id = "" }).id);

                        var existAc = _accountContexts.FirstOrDefault(u => u.UserId == ac.UserId && !u.IsExpired);
                        if (existAc == null)
                        {
                            AddConsoleLog(ac, "登录成功");
                            Window.ShowWindow(Window.GetConsoleWindow(), 0);
                            #region 设置基本资料
                            //判断More按钮
                            await ac.Page.WaitForSelectorAsync("button.btn-icon.rp.btn-menu-toggle.sidebar-tools-button.is-visible");
                            await ac.Page.ClickAsync("button.btn-icon.rp.btn-menu-toggle.sidebar-tools-button.is-visible");
                            //进入设置
                            await ac.Page.GetByText("Settings").First.ClickAsync();

                            await ac.Page.WaitForSelectorAsync("div.profile-name", new PageWaitForSelectorOptions { State = WaitForSelectorState.Visible, Timeout = 600000 });
                            ac.Phone = await ac.Page.InnerTextAsync("div.sidebar-left-section-content>div.row>.row-title", new PageInnerTextOptions { Timeout = 2000 });
                            ac.Phone = ac.Phone.Replace(" ", "");

                            //进入设置用户信息
                            await ac.Page.ClickAsync(".sidebar-header__title+button.btn-icon");
                            //等待5秒从后台加载用户信息
                            Thread.Sleep(5000);

                            //上传头像
                            //var fileChooser = await ac.Page.RunAndWaitForFileChooserAsync(async () => { await ac.Page.ClickAsync("div.AvatarEditable > label"); });
                            //await fileChooser.SetFilesAsync(Application.StartupPath + "左道机器人.gif");
                            //await ac.Page.WaitForSelectorAsync("button.Button.confirm-button.default.primary.round");
                            //await ac.Page.ClickAsync("button.Button.confirm-button.default.primary.round", new PageClickOptions { Delay = 10000 });

                            ac.Name = _names.ElementAt(Randomly.Number(0, _names.Count - 1, danshuang.All));
                            //设置名
                            await ac.Page.FillAsync(".avatar-edit+.input-wrapper>div:nth-child(1)>.input-field-input", ac.Name);
                            //签名
                            var introduce = _introduces.ElementAt(Randomly.Number(0, _introduces.Count - 1, danshuang.All));
                            await ac.Page.FillAsync(".avatar-edit+.input-wrapper>div:nth-child(3)>.input-field-input", introduce);
                            //设置用户名
                            ac.UserName = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(PinyinHelper.GetPinyin(Randomly.Surname() + Randomly.Name(Gender.All), " ").ToLower()).Replace(" ", "") + "" + Randomly.Number(0, 999, danshuang.All);
                            await ac.Page.FillAsync("input[name='username']", ac.UserName);
                            //保存
                            await ac.Page.WaitForSelectorAsync("button.btn-circle.btn-corner.z-depth-1.rp.is-visible");
                            await ac.Page.ClickAsync("button.btn-circle.btn-corner.z-depth-1.rp.is-visible");
                            await Task.Delay(2000);
                            #endregion

                            //添加联系人
                            //var addContact = async (string userName) =>
                            //{
                            //openGrouper:
                            //    try
                            //    {
                            //        await ac.Page.GotoAsync("https://web.telegram.org/k/#?tgaddr=tg%3A%2F%2Fresolve%3Fdomain%3D" + userName);
                            //    }
                            //    catch
                            //    {
                            //        goto openGrouper;
                            //    }

                            //    try
                            //    {
                            //        var more = "button.tgico-more";
                            //        await ac.Page.WaitForSelectorAsync(more, new PageWaitForSelectorOptions { State = WaitForSelectorState.Visible });
                            //        await ac.Page.ClickAsync(more);
                            //        await ac.Page.GetByText("Add to contacts").ClickAsync();
                            //        await ac.Page.ClickAsync("button.btn-circle.btn-corner.z-depth-1.rp.is-visible");
                            //        AddConsoleLog(ac, $"成功添加{userName}为好友");
                            //    }
                            //    catch
                            //    {
                            //        AddConsoleLog(ac, $"添加{userName}为好友失败");
                            //        goto openGrouper;
                            //    }
                            //};

                            ////加群主
                            //await addContact("ZuoDao_MuYan");
                            ////加韩冰
                            //await addContact("ZuoDao_HanBing");

                            #region 加入群
                            //openGroup:
                            //    try
                            //    {
                            //        ac.Page.GotoAsync("https://web.telegram.org/k/#?tgaddr=tg%3A%2F%2Fresolve%3Fdomain%3DZuoDaoMianDian");
                            //    }
                            //    catch
                            //    {
                            //        goto openGroup;
                            //    }

                            //    try
                            //    {
                            //        await ac.Page.WaitForSelectorAsync("div.topics-slider ul.chatlist>a:first-child");
                            //        await ac.Page.ClickAsync("div.topics-slider ul.chatlist>a:first-child");
                            //        await ac.Page.GetByText("JOIN").WaitForAsync();
                            //        await ac.Page.GetByText("JOIN").ClickAsync();
                            //        AddConsoleLog(ac, "加入了群");
                            //    }
                            //    catch
                            //    {
                            //        AddConsoleLog(ac, "加群失败");
                            //        goto openGroup;
                            //    }
                            //    await Task.Delay(2000);
                            #endregion

                            //#region 关注受限机器人
                            //openSpamBot:
                            //    try
                            //    {
                            //        await ac.Page.GotoAsync("https://www.google.com");
                            //        ac.Page.GotoAsync("https://web.telegram.org/k/#@SpamBot");
                            //    }
                            //    catch
                            //    {
                            //        goto openSpamBot;
                            //    }

                            //    try
                            //    {
                            //        await ac.Page.WaitForSelectorAsync("button.btn-primary.btn-transparent.text-bold.chat-input-control-button.rp");
                            //        await ac.Page.ClickAsync("button.btn-primary.btn-transparent.text-bold.chat-input-control-button.rp");
                            //        await Task.Delay(2000);
                            //        AddConsoleLog(ac, "关注了受限机器人");
                            //    }
                            //    catch
                            //    {
                            //        goto openSpamBot;
                            //    }

                            //    #endregion

                            ac.IsBanned = false;
                            ac.IsExpired = false;
                            ac.IsAccountSeted = true;
                            await PostAccountLogin(ac);
                            await AddAccount(ac);
                        }
                        else
                        {
                            existAc.IsBanned = false;
                            existAc.IsExpired = false;
                            // 发送POST请求并获取响应
                            HttpResponseMessage? response = null;
                            try
                            {
                                var url = _domain + "Accounts/LoginBackup/" + existAc.UserId;
                                response = await _httpClient.PostAsync(url, new StringContent(JsonConvert.SerializeObject(ac.ContextOptions), System.Text.Encoding.UTF8, "application/json"));
                            }
                            catch
                            {
                            }

                            // 检查响应状态码
                            if (response != null && response.IsSuccessStatusCode)
                            {
                                existAc.LoginBackups++;
                                AddConsoleLog(existAc, "成功备份登录状态");
                            }
                            else
                            {
                                AddConsoleLog(existAc, "未成功备份登录状态:" + response.StatusCode);
                            }
                        }
                        await DisposeAccountBrowser(ac, false);
                    }

                }).Start();
            }
        }
        //单元格点击
        private async void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var rowIndex = e.RowIndex;
            if (rowIndex == -1)
                return;

            var row = dataGridView.Rows[rowIndex];
            var ac = _accountContexts[rowIndex];
            var cell = row.Cells[e.ColumnIndex];

            if (cell.ReadOnly)
            {
                if (cell is DataGridViewButtonCell or DataGridViewCheckBoxCell)
                {
                    dataGridView.EndEdit();
                    dataGridView.CurrentCell = null;
                    MessageBox.Show("操作有误");
                    return;
                }
            }

            //显示日志
            if (e.ColumnIndex == 7)
            {
                if ((bool)row.Cells[7].EditedFormattedValue)
                {
                    Parallel.ForEach(_accountContexts, ac => { ac.IsShowLog = false; });
                    ac.IsShowLog = true;
                    Console.Clear();
                    Console.Title = ac.Name + ":脚本运行日志";
                    //显示控制台
                    Window.ShowWindow(Window.GetConsoleWindow(), 5);
                    var logs = ac.Logs.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var log in logs)
                    {
                        Console.WriteLine(log);
                    }
                }
                else
                {
                    ac.IsShowLog = false;
                    //隐藏控制台
                    Window.ShowWindow(Window.GetConsoleWindow(), 0);
                }
            }
            //显示浏览器
            else if (e.ColumnIndex == 8)
            {
                row.Cells[8].ReadOnly = true;
                ac.IsShowBrowser = !ac.IsShowBrowser;

                if (ac.Browser == null || !ac.Browser.IsConnected || ac.Context == null || ac.Page == null || ac.Page.IsClosed)
                {
                    if (_changeIpType == 0)
                    {
                        ac.ContextOptions.Proxy = null;
                    }
                    else
                    {
                        ac.ContextOptions.Proxy = await GetProxy(ac);
                    }
                    await NewAccountBrowser(ac);
                }

                Window.ShowWindow(ac.IntPrt, ac.IsShowBrowser ? 5 : 0);
                row.Cells[8].ReadOnly = false;
            }
            //备份登录
            else if (e.ColumnIndex == 9)
            {
                if (ac.LoginBackupState == "开始备份")
                {
                    await Blackup(ac);

                }
                else
                {
                    ac.LoginBackupState = "开始备份";
                }
                AddConsoleLog(ac, "备份完成");
            }
            //开始/停止
            else if (e.ColumnIndex == 10)
            {
                //开始
                if (cell.EditedFormattedValue.ToString().Equals("开始"))
                {
                    ac.ActionState = "启动中";

                    if (ac.Browser == null || !ac.Browser.IsConnected || ac.Context == null || ac.Page == null || ac.Page.IsClosed)
                        await NewAccountBrowser(ac);

                    await Action(ac);
                }
                //停止
                else if (cell.EditedFormattedValue.ToString().Equals("停止"))
                {
                    ac.IsRuning = false;
                    ac.ActionState = "停止中";
                    if (ac.CancelTimerSource != null && !ac.CancelTimerSource.IsCancellationRequested)
                    {
                        ac.CancelTimerSource.Cancel();
                    }

                }
                else
                {
                    MessageBox.Show("请稍后,勿操作");
                }
            }
        }
        //更新登录
        private async Task<bool> Update(AccountContext ac)
        {
            AddConsoleLog(ac, "正在更新登录");

            var row = dataGridView.Rows.Cast<DataGridViewRow>().FirstOrDefault(row => row.Cells[1].Value?.ToString() == ac.Id.ToString());

        getLoginBackup:
            HttpResponseMessage? response = null;
            try
            {
                response = await _httpClient.GetAsync(_domain + "Accounts/GetLoginBackup/" + ac.UserId);
            }
            catch (Exception ec)
            {
                AddConsoleLog(ac, $"UserId:{ac.UserId}获取登录备份时出错:" + ec.Message);
            }

            if (response == null || response != null && !response.IsSuccessStatusCode)
            {
                AddConsoleLog(ac, $"UserId:{ac.UserId}获取登录备份时不成功");
                if (response != null && response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    ac.LoginBackups = 0;
                }
                else
                {
                    if (ac.LoginBackups > 0)
                    {
                        goto getLoginBackup;
                    }
                }
                return false;
            }

            var content = await response.Content.ReadAsStringAsync();
            BrowserNewContextOptions? options = null;
            try
            {
                options = JsonConvert.DeserializeObject<BrowserNewContextOptions>(content);
            }
            catch
            {
                AddConsoleLog(ac, "把返回的ContextOptions字符串转换为BrowserNewContextOptions时不成功");
                if (ac.LoginBackups > 0)
                {
                    goto getLoginBackup;
                }
                return false;
            }

            if (options == null)
            {
                AddConsoleLog(ac, "返回的字符串有误");
                if (ac.LoginBackups > 0)
                    goto getLoginBackup;

                return false;
            }

            ac.ContextOptions = options;
            ac.LoginBackups--;
            ac.IsExpired = false;
            await NewAccountBrowser(ac);
        home:
            try
            {
                await ac.Page.GotoAsync("https://web.telegram.org/a/", new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle });
            }
            catch (Exception e)
            {
                AddConsoleLog(ac, "打开home页时失败，重新打开:" + e.Message);
                goto home;
            }
            ac.IsExpired = await IsExpired(ac);
            if (!ac.IsExpired)
            {
                await PostAccountLogin(ac);
                ac.LoginBackupState = "开始备份";
                AddConsoleLog(ac, "更新成功,未发现二维码");
                return true;
            }
            else
            {
                ac.LoginBackupState = "不可备份";
                AddConsoleLog(ac, "更新失败:更新后还是出现二维码");

                if (ac.LoginBackups > 0)
                {
                    goto getLoginBackup;
                }

                return false;
            }
        }
        //判断是否过期
        private async Task<bool> IsExpired(AccountContext ac)
        {
            try
            {
                await ac.Page.GetByText("Qr Code").WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Attached, Timeout = 15000 });
                Debug.WriteLine("登录失效:页面重定向至二维码登录页");
                ac.IsExpired = true;
                return true;
            }
            catch
            {
                ac.IsExpired = false;
                return false;
            }
        }
        //提交账号登录
        private async Task PostAccountLogin(AccountContext ac)
        {
            // 发送POST请求并获取响应
            HttpResponseMessage? response = null;
            try
            {
                var content = JsonConvert.SerializeObject(new Account
                {
                    UserId = ac.UserId,
                    IsAccountSeted = ac.IsAccountSeted,
                    Phone = ac.Phone,
                    UserName = ac.UserName,
                    Name = ac.Name,
                    DeadlineLimitedTime = null,
                    IsExpired = false,
                    IsBanned = false,
                    ContextOptions = JsonConvert.SerializeObject(ac.ContextOptions),
                });

                Debug.WriteLine(content);
                response = await _httpClient.PostAsync(_domain + "Accounts", new StringContent(content, System.Text.Encoding.UTF8, "application/json"));
            }
            catch (Exception e)
            {
                AddConsoleLog(ac, "上传提交登录状态出错:" + e.Message);
            }

            // 检查响应状态码
            if (response != null && response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var resultAccount = JsonConvert.DeserializeObject<Account>(result);
                AddConsoleLog(ac, "保存登录状态");
            }
            else
            {
                AddConsoleLog(ac, "未保存登录状态:" + response.StatusCode);
            }
        }

        //封号检测
        private async Task BannedTest(AccountContext ac)
        {
            var row = dataGridView.Rows.Cast<DataGridViewRow>().FirstOrDefault(row => row.Cells[1].Value?.ToString() == ac.Id.ToString());
            var loginBrowser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
            var loginContext = await loginBrowser.NewContextAsync(CreatOptions());
            var loginPage = await loginContext.NewPageAsync();
        GetCode:
            try
            {
                await loginPage.GotoAsync("about:blank");
                await loginPage.GotoAsync("https://web.telegram.org/k/", new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle, Timeout = 60000 });
            }
            catch
            {
                goto GetCode;
            }

            try
            {
                //点击切换为手机号登录
                await loginPage.ClickAsync("#auth-pages > div > div.tabs-container.auth-pages__container > div.tabs-tab.page-signQR.active > div > div.input-wrapper > button", new PageClickOptions { Strict = true, Timeout = 60000 });
                AddConsoleLog(ac, "成功切换为手机号登录");
            }
            catch
            {
                AddConsoleLog(ac, "未成功切换为手机号登录,重新打开登录页");
                goto GetCode;
            }

            try
            {
                //输入号码
                await loginPage.TypeAsync("#auth-pages > div > div.tabs-container.auth-pages__container > div.tabs-tab.page-sign.active > div > div.input-wrapper > div.input-field.input-field-phone > div.input-field-input", ac.Phone);
                AddConsoleLog(ac, "成功输入手机号");
            }
            catch
            {
                AddConsoleLog(ac, "未找到输入手机号的元素,重新打开登录页");
                goto GetCode;
            }

            try
            {
                //点击确认号码 (这里可能长时间不切换到输入验证码页面)
                await loginPage.ClickAsync("#auth-pages > div > div.tabs-container.auth-pages__container > div.tabs-tab.page-sign.active > div > div.input-wrapper > button.btn-primary.btn-color-primary.rp");
                AddConsoleLog(ac, "点击了确认号码");
            }
            catch
            {
                AddConsoleLog(ac, "未找到确认号码的按钮元素,重新打开登录页");
                goto GetCode;
            }

            AddConsoleLog(ac, "开始检测账号是否被封禁");
            try
            {
                await loginPage.GetByText("PHONE_NUMBER_BANNED").WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
                ac.IsBanned = true;
            }
            catch
            {
                AddConsoleLog(ac, "账号未封禁");
                ac.IsBanned = false;
            }

            if (ac.IsBanned)
            {
                ac.IsBanned = true;
                ac.IsExpired = true;
                ac.LoginBackups = 0;
                try
                {
                    var res = await _httpClient.GetStringAsync(_domain + "Accounts/Banned/" + ac.Id);
                    AddConsoleLog(ac, "向后台告知账号已封禁");
                }
                catch (Exception ex)
                {
                    AddConsoleLog(ac, "未成功向后台告知账号封禁:" + ex.Message);
                }
                AddConsoleLog(ac, "账号已封禁");
            }
            await loginBrowser.DisposeAsync();
        }
        //批量 封号检测
        private async void button_detection_Click(object sender, EventArgs e)
        {
            var acs = _accountContexts.Where(u => u.IsExpired || u.LoginBackups == 0);
            if (!acs.Any())
            {
                MessageBox.Show("没有可检测的账号");
                return;
            }
            button_detection.Enabled = false;
            button_detection.Text = "检测中";
            this.Enabled = false;
            foreach (var ac in acs)
            {
                await BannedTest(ac);
            }
            button_detection.Text = "封号检测";
            button_detection.Enabled = true;
            this.Enabled = true;
            MessageBox.Show("封号检测完成,共有" + _accountContexts.Count(u => u.IsBanned) + "个账号被封禁");
        }

        //备份登录
        private async Task Blackup(AccountContext ac)
        {
            ac.LoginBackupState = "停止备份";

            if (ac.Browser == null || !ac.Browser.IsConnected || ac.Context == null || ac.Page == null || ac.Page.IsClosed)
                await NewAccountBrowser(ac);

            //已经登录的账号页面里获取验证码
            var page = await ac.Context.NewPageAsync();
            for (int i = ac.LoginBackups; i < _loginBackups + 1; i++)
            {
                //登录窗口
                var options = new BrowserNewContextOptions();
                options = CreatOptions();

                var loginBrowser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
                var loginContext = await loginBrowser.NewContextAsync(options);
                var loginPage = await loginContext.NewPageAsync();
                //结束执行
                var isEnd = async () =>
                {
                    if (ac.LoginBackupState == "开始备份")
                    {
                        await loginBrowser.DisposeAsync();
                        await page.CloseAsync();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                };

                if (await isEnd()) break;
                var isSuccess = false;

                try
                {
                    await page.GotoAsync("https://web.telegram.org/k/#?tgaddr=tg%3A%2F%2Fresolve%3Fphone%3D42777", new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle });

                    try
                    {
                        await page.GetByText("Qr Code").WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Attached, Timeout = 15000 });
                        Debug.WriteLine("登录失效:页面重定向至二维码登录页");
                        ac.IsExpired = true;
                        isSuccess = false;
                    }
                    catch
                    {
                        ac.IsExpired = false;
                        isSuccess = true;
                        AddConsoleLog(ac, "成功打开验证码机器人");
                    }
                }
                catch
                {
                    i--;
                    AddConsoleLog(ac, "没打开验证码机器人");
                    await loginBrowser.DisposeAsync();
                    continue;
                }

                if (!isSuccess)
                {
                    continue;
                }

                if (await isEnd()) break;
                try
                {
                    await page.ClickAsync("div.chat-info-container button.btn-icon.rp.btn-menu-toggle");
                    AddConsoleLog(ac, "点击了验证机器下拉按钮");
                }
                catch
                {
                    i--;
                    AddConsoleLog(ac, "验证机器人那里没发现more的下拉按钮");
                    await loginBrowser.DisposeAsync();
                    continue;
                }

                if (await isEnd()) break;

                var isExistDeleteChatBtn = false;
                try
                {
                    await page.GetByText("Delete Chat").WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
                    AddConsoleLog(ac, "验证机器人的下拉按钮有Delete Chat按钮");
                    isExistDeleteChatBtn = true;
                }
                catch
                {
                    AddConsoleLog(ac, "验证机器人下拉菜单中没Delete Chat按钮");
                }


                if (await isEnd()) break;
                if (isExistDeleteChatBtn)
                {
                    try
                    {
                        await page.GetByText("Delete Chat").ClickAsync();
                        AddConsoleLog(ac, "点击了下拉按钮Delete Chat");
                    }
                    catch
                    {
                        i--;
                        AddConsoleLog(ac, "下拉菜单没发现可删除信息的按钮选项");
                        await loginBrowser.DisposeAsync();
                        continue;
                    }
                    if (await isEnd()) break;
                    try
                    {
                        await page.ClickAsync("body > div.popup.popup-peer.popup-delete-chat.active > div > label");
                        AddConsoleLog(ac, "勾选同时删除Telegram服务器信息复选框");
                    }
                    catch
                    {
                        i--;
                        AddConsoleLog(ac, "没发现可同时删除Telegram服务器的消息复选框");
                        await loginBrowser.DisposeAsync();
                        continue;
                    }
                    if (await isEnd()) break;
                    try
                    {
                        await page.ClickAsync("body > div.popup.popup-peer.popup-delete-chat.active > div > div.popup-buttons > button.btn.danger.rp");
                        AddConsoleLog(ac, "点击了确认删除聊天按钮");

                    }
                    catch (Exception)
                    {
                        i--;
                        AddConsoleLog(ac, "没发现确认删除聊天按钮");
                        await loginBrowser.DisposeAsync();
                        continue;
                    }
                }

            GetCode:
                try
                {
                    await loginPage.GotoAsync("about:blank");
                    await loginPage.GotoAsync("https://web.telegram.org/k/", new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle, Timeout = 60000 });
                }
                catch
                {
                    goto GetCode;
                }

                try
                {
                    //点击切换为手机号登录
                    await loginPage.ClickAsync("#auth-pages > div > div.tabs-container.auth-pages__container > div.tabs-tab.page-signQR.active > div > div.input-wrapper > button", new PageClickOptions { Strict = true, Timeout = 30000 });
                    AddConsoleLog(ac, "成功切换为手机号登录");
                }
                catch
                {
                    AddConsoleLog(ac, "未成功切换为手机号登录,重新打开登录页");
                    goto GetCode;
                }

                if (await isEnd()) break;
                try
                {
                    //输入号码
                    await loginPage.TypeAsync("#auth-pages > div > div.tabs-container.auth-pages__container > div.tabs-tab.page-sign.active > div > div.input-wrapper > div.input-field.input-field-phone > div.input-field-input", ac.Phone);
                    AddConsoleLog(ac, "成功输入手机号");
                }
                catch
                {
                    AddConsoleLog(ac, "未找到输入手机号的元素,重新打开登录页");
                    goto GetCode;
                }
                if (await isEnd()) break;
                try
                {
                    //点击确认号码 (这里可能长时间不切换到输入验证码页面)
                    await loginPage.ClickAsync("#auth-pages > div > div.tabs-container.auth-pages__container > div.tabs-tab.page-sign.active > div > div.input-wrapper > button.btn-primary.btn-color-primary.rp");
                    AddConsoleLog(ac, "点击了确认号码");
                }
                catch
                {
                    AddConsoleLog(ac, "未找到确认号码的按钮元素");
                    goto GetCode;
                }
                if (await isEnd()) break;

                try
                {
                    await loginPage.GetByText("Please wait").WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Hidden, Timeout = 60000 });
                }
                catch
                { }
                if (await isEnd()) break;
                AddConsoleLog(ac, "等待按钮消失");
                //要求等会再进行登录
                var isWait = false;
                try
                {
                    await loginPage.GetByText("FLOOD_WAIT").WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 5000 });
                    isWait = true;
                }
                catch
                { }
                if (await isEnd()) break;
                if (isWait)
                {
                    AddConsoleLog(ac, "出现了防洪按钮,结束");
                    break;
                }

                AddConsoleLog(ac, "切换到输入验证码页面");
                var isShowCodeInput = false;
                try
                {
                    //等待出现验证码输入框
                    await loginPage.WaitForSelectorAsync("#auth-pages > div > div.tabs-container.auth-pages__container > div.tabs-tab.page-authCode.active > div > div.input-wrapper > div > input", new PageWaitForSelectorOptions { State = WaitForSelectorState.Visible, Timeout = 25000 });
                    isShowCodeInput = true;
                }
                catch
                { }
                if (await isEnd()) break;
                if (!isShowCodeInput)
                {
                    AddConsoleLog(ac, "开始检测账号是否被封禁");
                    try
                    {
                        await loginPage.GetByText("PHONE_NUMBER_BANNED").WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
                    }
                    catch
                    {
                        AddConsoleLog(ac, "账号未封禁");
                        ac.IsBanned = false;
                    }

                    if (ac.IsBanned)
                    {
                        ac.IsBanned = true;
                        try
                        {
                            await _httpClient.GetStringAsync(_domain + "Accounts/Banned/" + ac.Id);
                            AddConsoleLog(ac, "向后台告知账号已封禁");
                        }
                        catch (Exception ex)
                        {
                            AddConsoleLog(ac, "未成功向后台告知账号封禁:" + ex.Message);
                        }
                        AddConsoleLog(ac, "账号已封禁");
                        await loginBrowser.DisposeAsync();
                        break;
                    }

                    if (await isEnd()) break;
                    AddConsoleLog(ac, "判断输入的号码是否有效");
                    var isPhoneNumber = false;
                    try
                    {
                        await loginPage.GetByText("Phone Number Invalid").WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
                        isPhoneNumber = true;
                    }
                    catch
                    { }
                    if (await isEnd()) break;
                    if (!isPhoneNumber)
                    {
                        AddConsoleLog(ac, "手机号无效");
                        goto GetCode;
                    }
                }

                if (await isEnd()) break;
                AddConsoleLog(ac, "显示了输入验证码页面,延迟20秒");
                await Task.Delay(20000);

            navCodeBot:
                if (await isEnd()) break;
                await page.GotoAsync("https://web.telegram.org/k/#?tgaddr=tg%3A%2F%2Fresolve%3Fphone%3D42777", new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle });
                try
                {
                    await page.GetByText("Qr Code").WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Attached, Timeout = 15000 });
                    Debug.WriteLine("登录失效:页面重定向至二维码登录页");
                    ac.IsExpired = true;
                    isSuccess = false;
                }
                catch
                {
                    ac.IsExpired = false;
                    isSuccess = true;
                    AddConsoleLog(ac, "成功打开验证码机器人");
                }

                if (!isSuccess)
                {
                    continue;
                }

                var endMsg = "";
                try
                {
                    endMsg = await page.TextContentAsync("section.bubbles-date-group:last-of-type>div.bubbles-group:last-of-type>div:last-of-type", new PageTextContentOptions { Timeout = 30000 });
                }
                catch
                { }

                var match = Regex.Match(endMsg, @"code: (\d{5})");

                if (!match.Success)
                {
                    AddConsoleLog(ac, "没获取到最后一条信息,检查CSS选择器");
                    await loginBrowser.DisposeAsync();
                    i--;
                    continue;
                }

                try
                {
                    var code = match.Groups[1].Value;
                    AddConsoleLog(ac, "获取到了验证码:" + code + " 输入验证码");
                    await loginPage.TypeAsync("#auth-pages > div > div.tabs-container.auth-pages__container > div.tabs-tab.page-authCode.active > div > div.input-wrapper > div > input", code);
                }
                catch
                {
                    AddConsoleLog(ac, "没获取到可输入验证码的输入框元素");
                    await loginBrowser.DisposeAsync();
                    i--;
                    continue;
                }

                var isInvalidCode = true;

                //登录成功
                var isLogin = false;
                try
                {
                    await loginPage.WaitForSelectorAsync("div.chatlist-top>ul.chatlist>a", new PageWaitForSelectorOptions { State = WaitForSelectorState.Attached, Timeout = 60000 });
                    isLogin = true;
                }
                catch
                {
                    AddConsoleLog(ac, "没登录成功");
                    await loginBrowser.DisposeAsync();
                    i--;
                    continue;
                }

                if (isLogin)
                {
                    var storageState = await loginContext.StorageStateAsync();
                    var storage = JsonConvert.DeserializeObject<Storage>(storageState);
                    storage.origins[0].localStorage = storage?.origins[0].localStorage.Where(u => !u.name.Equals("tt-global-state")).ToList();
                    options.StorageState = JsonConvert.SerializeObject(storage);

                    // 发送POST请求并获取响应
                    HttpResponseMessage? response = null;
                    try
                    {
                        var content = JsonConvert.SerializeObject(options);
                        var url = _domain + "Accounts/LoginBackup/" + ac.UserId;
                        response = await _httpClient.PostAsync(url, new StringContent(content, System.Text.Encoding.UTF8, "application/json"));
                    }
                    catch (Exception ex)
                    {
                        AddConsoleLog(ac, "提交登录状态时出现异常:" + ex.Message);
                    }

                    // 检查响应状态码
                    if (response != null && response.IsSuccessStatusCode)
                    {
                        AddConsoleLog(ac, "成功备份登录状态");
                        ac.LoginBackups++;
                        if (ac.LoginBackups >= _loginBackups)
                        {
                            ac.LoginBackupState = "不可备份";
                        }

                    }
                    else
                    {
                        AddConsoleLog(ac, "未成功备份登录状态:" + response.StatusCode);
                    }
                }
                else
                {
                    AddConsoleLog(ac, "没登录成功");
                    i--;
                }

                await loginBrowser.DisposeAsync();
                if (await isEnd()) break;
            }
            await page.CloseAsync();
            if (!ac.IsRuning)
                await DisposeAccountBrowser(ac, true);

            ac.LoginBackupState = ac.LoginBackups >= _loginBackups ? "不可备份" : "开始备份";

            if (!_accountContexts.Any(a => a.LoginBackupState == "停止备份"))
            {
                button_backupLogin.Text = "备份登录";
                AddConsoleLog(ac, "登录状态备份完成");
            }
        }
        //批量 备份登录
        private async void button_backupLogin_Click(object sender, EventArgs e)
        {
            var acs = _accountContexts.Where(u => !u.IsExpired && u.LoginBackups < _loginBackups);
            if (!acs.Any())
            {
                MessageBox.Show("没有可备份的账号");
                return;
            }

            if (button_backupLogin.Text == "备份登录")
            {
                button_backupLogin.Text = "停止备份";

                for (int i = 0; i < (acs.Count() / _actions); i++)
                {
                    if (button_backupLogin.Text == "停止备份")
                    {
                        Parallel.ForEach(acs.Skip(i * _actions).Take(_actions), ac =>
                        {
                            new Thread(async () => { await Blackup(ac); }).Start();
                        });
                        await Task.Delay(10000);
                        while (_accountContexts.Any(a => a.LoginBackupState == "停止备份"))
                        {
                            Debug.WriteLine("还有运行中的备份任务");
                            await Task.Delay(1000);
                        }

                        if (_changeIpType == 0)
                            await ChangeIp();
                    }
                }
                MessageBox.Show("备份完成");
            }
            else
            {
                var rows = _accountContexts.Where(a => a.LoginBackupState == "停止备份");
                foreach (var item in rows)
                {
                    item.LoginBackupState = "开始备份";
                }
            }
        }

        //拉人
        private async Task Action(AccountContext ac)
        {
            comboBox_changeIp.Enabled = false;
            var rowIndex = _accountContexts.FindIndex(u => u.UserId == ac.UserId);
            var row = dataGridView.Rows[rowIndex];

            if (Console.Title.Contains(ac.Name))
                Console.Clear();
            AddConsoleLog(_accountContexts[rowIndex], "开始执行任务");
            ac.IsRuning = true;
            button_start.Invoke((MethodInvoker)delegate { button_start.Text = "停止拉人"; button_start.Enabled = true; });
            ac.ActionState = "停止";
            //拉入失败次数超过7次结束
            var fails = 0;
            //结束后执行
            var end = async () =>
            {
                await DisposeAccountBrowser(ac, false);
                ac.IsRuning = false;
                ac.ActionState = "开始";
                ac.IsShowBrowser = false;
                AddConsoleLog(_accountContexts[rowIndex], "本账号执行完毕！");
                //if (!_accountContexts.Any(u => u.IsRuning))
                //{
                //    if (button_start.Text == "开始拉人" || button_start.Text == "停止中")
                //    {
                //        comboBox_changeIp.Enabled = true;
                //        button_start.Enabled = true;
                //    }
                //}
            };

            //延迟匿名函数
            var delay = async Task (string msg, int second) =>
            {
                //该值需要是 -1（表示无限超时），0 或者一个正整数
                var IsValidTimeoutValue = (int value) =>
                {
                    if (value == -1 || value == 0 || value > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                };

                if (IsValidTimeoutValue(second))
                {
                    AddConsoleLog(ac, msg);
                    //如果取消了,就延迟也会被取消
                    ac.CancelTimerSource = new CancellationTokenSource();

                    try
                    {
                        await Task.Delay(second * 1000, ac.CancelTimerSource.Token);
                        AddConsoleLog(ac, "倒计时延迟完成结束");
                    }
                    catch
                    {
                        ac.IsRuning = false;
                        AddConsoleLog(ac, "倒计时延迟意外中断(有可能是您手动停止,或者是延迟时间达到了int类型的最大值),任务中止!");
                        await end();
                        return;
                    }
                }
            };

        #region 受限机器人
        spamBot:
            bool isOpenSpamBotSuccess = false;
            try
            {
                isOpenSpamBotSuccess = await GotoAsync(ac, "https://web.telegram.org/k/#?tgaddr=tg%3A%2F%2Fresolve%3Fdomain%3DSpamBot");

            }
            catch (Exception ec)
            {
                Debug.WriteLine("打开机器人网页失败:" + ec.Message);
                goto spamBot;
            }

            if (!isOpenSpamBotSuccess)
            {
                Debug.WriteLine("未成功打开机器人网页");
                await end();
                return;
            }

            await ac.Page.EvaluateAsync($"document.title = '{ac.Name}-机器人'");
            Debug.WriteLine("打开机器人网页");

            try
            {
                await ac.Page.TypeAsync("div.input-message-input[data-placeholder='Message']", "/start");
                AddConsoleLog(ac, "输入了/start指令");
            }
            catch
            {
                AddConsoleLog(ac, "输入/start指令时未找到输入框");
                goto spamBot;
            }

            try
            {
                await ac.Page.ClickAsync("#column-center > div > div > div.chat-input > div > div.btn-send-container > button");
                AddConsoleLog(ac, "点击了发送/start指令的按钮,等待5秒");
                await Task.Delay(5000);
            }
            catch
            {
                AddConsoleLog(ac, "点击发送/start指令的按钮时出错");
                goto spamBot;
            }

            var endMsg = string.Empty;
            try
            {
                endMsg = await ac.Page.TextContentAsync("section.bubbles-date-group:last-of-type>div.bubbles-group:last-of-type>div:last-of-type");
                AddConsoleLog(ac, "获取到了约束限制机器人最后一条信息:" + endMsg);
            }
            catch
            {
                AddConsoleLog(ac, "获取约束限制机器人最后一条信息时出错");
                goto spamBot;
            }

            Match match = Regex.Match(endMsg, @"(\d{1,2}\s\w+\s\d{4},\s\d{2}:\d{2})");
            if (match.Success)
            {
                AddConsoleLog(ac, @"获取到了受限截止时间字符串");
                var dateTimeString = match.Groups[1].Value;
                var format = "d MMM yyyy, HH:mm";
                DateTime dateTime;
                ac.DeadlineLimitedTime = null;
                bool success = DateTime.TryParseExact(dateTimeString, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dateTime);
                if (success)
                {
                    if (dateTime > DateTime.Now)
                    {
                        ac.DeadlineLimitedTime = dateTime;
                        AddConsoleLog(ac, @"本账号受限截止:" + dateTime.ToString());
                    }
                }
                else
                {
                    AddConsoleLog(ac, "无法解析日期时间字符串:" + dateTimeString);
                }
            }
            else
            {
                AddConsoleLog(ac, "恭喜您,账号未受限");
            }

            if (ac.DeadlineLimitedTime != null)
            {
                try
                {
                    await _httpClient.GetStringAsync(_domain + "Accounts/DeadlineLimited?id=" + ac.Id + "&deadlineLimitedTime=" + ac.DeadlineLimitedTime);
                    AddConsoleLog(ac, "成功向后台告知账号已受限");
                }
                catch (Exception ex)
                {
                    AddConsoleLog(ac, "未成功向后台告知账号已受限:" + ex.Message);
                }
                await end();
                return;
            }
        #endregion

        openGroupPage:
            if (ac.IsRuning)
            {
                var isOpenGroupSuccess = await GotoAsync(ac, "https://web.telegram.org/k/#?tgaddr=tg%3A%2F%2Fresolve%3Fdomain%3DZuoDaoMianDian");
                if (!isOpenGroupSuccess || !ac.IsRuning || ac.Page == null)
                {
                    await end();
                    return;
                }

                try
                {
                    await ac.Page.EvaluateAsync($"document.title = '{ac.Name}-左道群'");
                    //选项卡切换到当前页
                    await ac.Page.BringToFrontAsync();
                }
                catch
                {
                }

                if (!ac.IsExpired)
                {
                    ILocator? moreBtn = null;
                    try
                    {
                        moreBtn = ac.Page.Locator(".topics-slider button.btn-icon.btn-menu-toggle");
                    }
                    catch
                    {
                    }

                    if (moreBtn != null)
                    {
                        #region 打开群添加成员页面
                        try
                        {
                            await moreBtn.WaitForAsync();
                            await moreBtn.ClickAsync();
                            AddConsoleLog(ac, "存在展开Group info的按钮");
                        }
                        catch
                        {
                            AddConsoleLog(ac, "未点击到Group info的按钮,重新打开左道群");
                            goto openGroupPage;
                        }

                        try
                        {
                            await ac.Page.GetByText("Group Info").ClickAsync();
                            AddConsoleLog(ac, "进入群信息页");
                        }
                        catch
                        {
                            AddConsoleLog(ac, "未进入群信息页,重新打开左道群");
                            goto openGroupPage;
                        }
                        #endregion
                    }
                }
            }

        run:
            if (ac.IsRuning)
            {
                AddConsoleLog(ac, $"================================ 新一轮:重复失败{fails}次 =================================");
                try
                {
                    await ac.Page.GetByText("Add Members").WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 1500 });
                    await ac.Page.ClickAsync("button.btn-icon.tgico-left.sidebar-close-button");
                    AddConsoleLog(ac, "返回Members列表页");
                }
                catch
                {
                }

                try
                {
                    await ac.Page.ClickAsync(".scrollable+button.btn-circle.btn-corner.z-depth-1.rp", new PageClickOptions { Timeout = 2000 });
                    AddConsoleLog(ac, "点击了+添加成员按钮");
                }
                catch
                {
                    AddConsoleLog(ac, "未点击到+添加成员按钮");
                    goto openGroupPage;
                }

                //获取需要加入群的用户
                var member = await GetCollectionUser(ac);
                AddConsoleLog(ac, $"服务器返回的UserName:{member.UserName} 头像:{member.IsHead} 名称:{member.Name}");
                try
                {
                    var inputSelector = "#column-left > div > div.tabs-tab.sidebar-slider-item.scrolled-top.scrolled-bottom.scrollable-y-bordered.add-members-container.active > div.sidebar-content > div > div.sidebar-left-section-container > div > div > div > div > div > input";
                    await ac.Page.FillAsync(inputSelector, "");
                    await ac.Page.TypeAsync(inputSelector, member.UserName);
                    AddConsoleLog(ac, $"填写了UserName:{member.UserName}");
                }
                catch
                {
                    AddConsoleLog(ac, "未填写到用户名");
                    goto run;
                }

                IElementHandle? selectUser = null;
                try
                {
                    AddConsoleLog(ac, "填写完用户名后,判断 data-peer-id='" + member.UserId + "' 是否出现");
                    await ac.Page.WaitForSelectorAsync("ul.chatlist>a[data-peer-id='" + member.UserId + "']", new PageWaitForSelectorOptions { State = WaitForSelectorState.Visible, Timeout = 25000 });
                    selectUser = await ac.Page.QuerySelectorAsync("ul.chatlist>a[data-peer-id='" + member.UserId + "']");
                }
                catch
                {
                }

                if (selectUser == null)
                {
                    //try
                    //{
                    //    await ac.Page.WaitForSelectorAsync("ul.chatlist>a", new PageWaitForSelectorOptions { State = WaitForSelectorState.Visible, Timeout = 3000 });
                    //}
                    //catch
                    //{
                    //}

                    //IReadOnlyList<IElementHandle>? searchUsers = null;
                    ////搜索到的用户列表项
                    //try
                    //{
                    //    searchUsers = await ac.Page.QuerySelectorAllAsync("div.add-members-container ul > a");
                    //    AddConsoleLog(ac, "出现了列表");
                    //}
                    //catch
                    //{ }


                    //if (searchUsers == null || !searchUsers.Any())
                    //{
                    //    AddConsoleLog(ac, "填写完用户名后,60秒未出现可选择列表");
                    //    goto run;
                    //}


                    //foreach (var searchUser in searchUsers)
                    //{
                    //    var status = await searchUser.QuerySelectorAsync("div.dialog-subtitle>div.row-subtitle>span");
                    //    if (status == null)
                    //        continue;

                    //    var statusText = await status.TextContentAsync();
                    //    if (string.IsNullOrEmpty(statusText))
                    //        continue;

                    //    if (statusText.Contains("bot") || statusText.Contains("long time") || statusText.Contains("month"))
                    //        continue;

                    //    var id = await searchUser.GetAttributeAsync("data-peer-id");
                    //    if (string.IsNullOrEmpty(id))
                    //        continue;

                    //    try
                    //    {
                    //        await ac.Page.GotoAsync("https://web.telegram.org/k/#" + id, new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle, Timeout = 5000 });
                    //    }
                    //    catch
                    //    {
                    //        continue;
                    //    }

                    //    await Task.Delay(1500);
                    //    var urlUserName = GetText.Getright(ac.Page.Url.ToLower(), "#").Replace("@", "");
                    //    Debug.WriteLine("真实的UserName:" + member.UserName + "  当前项UserName:" + urlUserName);
                    //    if (urlUserName != member.UserName.ToLower())
                    //        continue;

                    //    selectUser = searchUser;
                    //    break;
                    //}

                    //if (selectUser == null)
                    //{
                    AddConsoleLog(ac, "填写完用户名后,60秒未出现可选择列表");

                    //await DeleteGroupChannelBotUser(member.Id);
                    //AddConsoleLog(ac, member.Id + "对其删除");
                    goto run;
                    //}
                }

                //选择要拉入的用户
                try
                {
                    await selectUser.ClickAsync();
                    AddConsoleLog(ac, "点击了搜索到的用户");
                }
                catch
                {
                    AddConsoleLog(ac, "填写完用户名后虽然出现了列表项，但没点成功");
                    goto run;
                }

                try
                {
                    //等待确认选择到了
                    await ac.Page.WaitForSelectorAsync("div.selector-user.scale-in > div.selector-user-avatar-container");
                    AddConsoleLog(ac, "点击到了列表用户,在输入框里出现了用户头像和名称");
                }
                catch
                {
                    AddConsoleLog(ac, "在输入框里没有出现用户头像和名称");
                    goto run;
                }

                try
                {
                    //添加成员            
                    await ac.Page.ClickAsync("button.btn-circle.is-visible");
                    AddConsoleLog(ac, "点击确认添加成员按钮");
                }
                catch
                {
                    AddConsoleLog(ac, "未点击到确认添加成员按钮");
                    goto run;
                }

                try
                {
                    //确认添加到群组
                    await ac.Page.ClickAsync("body > div.popup.popup-peer.popup-add-members.active > div > div.popup-buttons > button:nth-child(1)");
                    AddConsoleLog(ac, "在弹出的模态框里再次确认添加成员至群里");
                }
                catch
                {
                    AddConsoleLog(ac, "未在弹出的模态框里点击到再次确认添加按钮");
                    goto run;
                }

                try
                {
                    await ac.Page.GetByText("user's privacy settings").WaitForAsync(new LocatorWaitForOptions { Timeout = 3000 });
                    try
                    {
                        await _httpClient.GetStringAsync(_domain + "GroupChannelBotUser/SetOnlyContactAddToGroup/" + member.Id);
                        AddConsoleLog(ac, "隐私设置限制了他人拉其入群，并向后台SetOnlyCatactAddToGroup.继续拉下一个用户");
                    }
                    catch
                    {
                        AddConsoleLog(ac, "隐私设置限制了他人拉其入群,但并未SetOnlyContactAddToGroup.继续拉下一个用户");
                    }
                    goto run;
                }
                catch
                {
                }

                //是否拉入失败
                var isFail = false;
                try
                {
                    await ac.Page.WaitForSelectorAsync("#column-left > div > div.tabs-tab.sidebar-slider-item.scrolled-top.scrolled-bottom.scrollable-y-bordered.shared-media-container.profile-container.can-add-members.active > div.sidebar-content > button", new PageWaitForSelectorOptions { Timeout = 4000 });
                }
                catch
                {
                    isFail = true;
                }

                if (isFail)
                {
                    AddConsoleLog(ac, "拉入失败");
                    fails++;
                    if (fails >= 7)
                        ac.IsRuning = false;
                    goto run;
                }
                else
                {
                    fails = 0;
                    ac.DeadlineLimitedTime = null;
                    try
                    {
                        await _httpClient.GetStringAsync(_domain + "Accounts/DeadlineLimited?id=" + ac.Id + "&deadlineLimitedTime=" + ac.DeadlineLimitedTime);
                        AddConsoleLog(ac, "成功向后台告知账号没有被受限");
                    }
                    catch (Exception ex)
                    {
                        AddConsoleLog(ac, "未成功向后台告知账号没有被受限:" + ex.Message);
                    }
                }

                HttpResponseMessage? setAddedGroupResponse = null;
                try
                {
                    setAddedGroupResponse = await _httpClient.GetAsync(_domain + "GroupChannelBotUser/SetAddedGroup/" + member.Id);
                }
                catch
                {
                }

                if (setAddedGroupResponse != null && setAddedGroupResponse.IsSuccessStatusCode)
                {
                    AddConsoleLog(ac, "将[" + member.UserName + "]用户成功拉入本群，并向后台SetAddedGroup");
                }
                else
                {
                    AddConsoleLog(ac, "将[" + member.UserName + "]用户成功拉入本群,但并未SetAddedGroup");
                }

                //是否存在今日数据
                var now = DateTime.Now;
                var isExistTodayData = ac.LastAddedUserTime != null && ac.LastAddedUserTime.Value.Date == now.Date;
                ac.TodayAddedUsers = !isExistTodayData ? 1 : ac.TodayAddedUsers + 1;
                ac.AddMembers++;
                ac.LastAddedUserTime = now;
                goto run;
            }
            await end();
        }
        //批量 拉人
        private async void button_joinUser_Click(object sender, EventArgs e)
        {
            var isStart = button_start.Text == "开始拉人";

            //无可用账号
            if (isStart && !_accountContexts.Any(u => !u.IsExpired))
            {
                MessageBox.Show("开始失败，无可用账号");
                return;
            }

            if (isStart)
            {
                comboBox_changeIp.Enabled = false;
                button_start.Enabled = false;
                button_start.Text = "启动中";
            start:
                var acs = _accountContexts
                        .Where(ac => ac.DeadlineLimitedTime == null || ac.DeadlineLimitedTime <= DateTime.Now)
                        .Where(ac => !ac.IsExpired && !ac.IsBanned);

                for (int i = 0; i < (_accountContexts.Count / _actions); i++)
                {
                    //if (button_start.Text != "开始拉人" && button_start.Text != "停止中")
                    //{
                    var fors = 0;
                    Parallel.ForEach(acs.Skip(i * _actions).Take(_actions), async ac =>
                    {
                        ac.IsRuning = true;
                        ac.ActionState = "启动中";
                        fors++;
                        await Task.Delay(1500 * fors);
                        ac.IsShowBrowser = checkBox_browser.Checked && button_start.Text != "开始拉人";
                        if (ac.Browser == null || !ac.Browser.IsConnected || ac.Context == null || ac.Page == null || ac.Page.IsClosed)
                            await NewAccountBrowser(ac);

                        await Action(ac);
                        ac.IsShowBrowser = false;
                    });

                    await Task.Delay(10000);

                    while (_accountContexts.Any(u => u.IsRuning))
                        await Task.Delay(1000);

                    if (_changeIpType == 0)
                        await ChangeIp();
                    //}
                }

                if (button_start.Text != "开始拉人" && button_start.Text != "停止中")
                {
                    goto start;
                }

                button_start.Text = "开始拉人";
                button_start.Enabled = true;
                comboBox_changeIp.Enabled = true;
                MessageBox.Show("执行完成");
            }
            else
            {
                button_start.Enabled = false;
                button_start.Text = "停止中";

                foreach (var ac in _accountContexts)
                {
                    if (ac.IsRuning)
                    {
                        ac.IsRuning = false;

                        if (ac.CancelTimerSource != null && !ac.CancelTimerSource.IsCancellationRequested)
                        {
                            ac.CancelTimerSource.Cancel();
                        }

                        ac.ActionState = "停止中";
                    }
                }
            }
        }
        //浏览器释放
        private async Task DisposeAccountBrowser(AccountContext ac, bool isClearStorageState)
        {
            if (ac.CloseHandler != null && ac.Page != null)
            {
                ac.Page.Close -= ac.CloseHandler;
                ac.CloseHandler = null;
            }

            if (ac.Page != null)
            {
                await ac.Page.CloseAsync();
                ac.Page = null;
            }

            if (ac.Context != null)
            {
                try
                {
                    await ac.Context.DisposeAsync();
                }
                catch
                {
                }
                ac.Context = null;
            }


            if (ac.Browser != null)
            {
                try
                {
                    await ac.Browser.DisposeAsync();
                }
                catch
                {
                }
                ac.Browser = null;
            }

            if (isClearStorageState)
                ac.ContextOptions.StorageState = null;
        }
        //状态栏双击
        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }
        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;
            notifyIcon.Dispose();
            _playwright.Dispose();
            // 获取所有进程并终止它们
            foreach (var process in Process.GetProcessesByName("chrome"))
            {
                process.Kill();
            }
            foreach (var item in _accountContexts)
            {
                item.Logs.Clear();
            }
            if (File.Exists("config.txt"))

                Application.Exit();
        }
        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                MessageBox.Show("请稍后,程序正在后台执行逻辑中...");
                return;
            }

            this.Show();
            this.WindowState = FormWindowState.Normal;
        }
        //窗口关闭
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // 如果是通过控制台窗口关闭应用程序，则取消关闭操作
            if (e.CloseReason == CloseReason.UserClosing && _accountContexts.Any(u => u.IsRuning))
            {
                e.Cancel = true;
                Hide();
            }
            else if (File.Exists("config.txt"))
            {

            }

            base.OnFormClosing(e);
        }
        //添加日志
        private void AddConsoleLog(AccountContext ac, string msg)
        {
            // 如果行数超过最大行数，则删除首行
            if (ac.Logs.ToString().Split('\n').Length > 100)
            {
                int index = ac.Logs.ToString().IndexOf('\n');
                ac.Logs.Remove(0, index + 1);
            }

            msg = DateTime.Now.ToString("M月d日 H:m s") + "  " + msg;
            ac.Logs.AppendLine(msg);
            Debug.WriteLine(msg);
            if (Window.IsWindowVisible(Window.GetConsoleWindow()))
            {
                if (!string.IsNullOrEmpty(ac.Name) && Console.Title.Contains(ac.Name) || ac.UserId == 0 && Console.Title.Contains("代理Ip"))
                    Console.WriteLine(msg);
            }
        }
        //不选择行
        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            dataGridView.ClearSelection();
        }
        //开机自启动
        private async void StartRunToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _settings.IsStartUp = StartRunToolStripMenuItem.Text == "开机自启动";

            StartRunToolStripMenuItem.Text = _settings.IsStartUp ? "关闭自动启动" : "开机自启动";

            if (_settings.IsStartUp)
            {
                if (!Helper.IsStartupSet())
                {
                    Helper.SetStartup();
                }
            }
            else
            {
                if (Helper.IsStartupSet())
                {
                    Helper.RemoveStartup();
                }
            }

            await File.WriteAllTextAsync(Application.StartupPath + "Settings.json", JsonConvert.SerializeObject(_settings));
        }
        //重置数据
        private void button_restConfig_Click(object sender, EventArgs e)
        {
            if (!_accountContexts.Any())
            {
                MessageBox.Show("无可重置账号");
                return;
            }

            for (int i = 0; i < _accountContexts.Count; i++)
            {
                var item = _accountContexts[i];
                item.AddMembers = 0;
                item.LastAddedUserTime = null;
                item.TodayAddedUsers = 0;
            }


            MessageBox.Show("重置成功");
        }
        //获取需拉入群的成员
        private async Task<UserHtml> GetCollectionUser(AccountContext ac)
        {
        GetCollectionUser:
            var urs = string.Empty;
            try
            {
                urs = await _httpClient.GetStringAsync(_domain + "GroupChannelBotUser?type=3&categoryEnum=1&isAddedGroup=false&isOnlyContactAddToGroup=false&size=1");
            }
            catch (Exception ex)
            {
                AddConsoleLog(ac, "获取需拉入群的用户时出错:" + ex.Message);
            }

            if (string.IsNullOrEmpty(urs))
            {
                await Task.Delay(10000);
                goto GetCollectionUser;
            }

            IEnumerable<GroupChannelBotUser>? users = null;
            GroupChannelBotUser? user = null;
            try
            {
                users = JsonConvert.DeserializeObject<IEnumerable<GroupChannelBotUser>>(urs);
                if (users == null || !users.Any())
                {
                    AddConsoleLog(ac, "未获取到需拉入群的用户");
                    goto GetCollectionUser;
                }
                user = users.First();
                AddConsoleLog(ac, "从远程获取拉入群的用户:" + user.Url);
            }
            catch
            {
                AddConsoleLog(ac, "系列化采集到的用户时出错");
                goto GetCollectionUser;
            }

            //判断是否存在
            var isExistUser = false;
            var u = new UserHtml();
            u.Id = user.Id;
            u.UserId = (long)user.Number;
            u.Name = user.Name;
            var html = string.Empty;
            try
            {
                html = await _httpClient.GetStringAsync("https://t.me/" + user.Url);
            }
            catch
            {
                goto GetCollectionUser;
            }

            if (html.Contains("tgme_page_extra"))
            {
                // 创建HtmlDocument对象
                var htmlDoc = new HtmlAgilityPack.HtmlDocument();
                // 加载HTML内容（可以从文件、字符串或Web页面加载）
                htmlDoc.LoadHtml(html);
                u.UserName = user.Url;
                u.IsHead = html.Contains("telegram-cdn.org");
                var desc = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='tgme_page_description ']");
                AddConsoleLog(ac, "HTTP协议判断 存在userName:" + user.Url);
                //判断是否合规
                if (_removeKeywords.Any(c => u.Name.Contains(c))
                    || desc != null && !string.IsNullOrEmpty(desc.InnerText) && _removeKeywords.Any(c => desc.InnerText.Contains(c))
                    || !TextHelper.IsChinese(u.Name) && !TextHelper.IsChinese(desc?.InnerText))
                {
                    AddConsoleLog(ac, $"名称或者简介不合规:Name:{u?.Name},Desc:{desc?.InnerText},对其删除");
                    await DeleteGroupChannelBotUser(user.Id);
                    goto GetCollectionUser;
                }

                u.Introduce = desc?.InnerText;
                AddConsoleLog(ac, "用户名称和简介合规,开始拉入群操作");
                isExistUser = true;
            }
            else
            {
                AddConsoleLog(ac, "HTTP协议判断 不存在userName:" + user.Url);
            }

            if (!isExistUser)
            {
                await DeleteGroupChannelBotUser(user.Id);
                AddConsoleLog(ac, user.Id + "对其删除");
                goto GetCollectionUser;
            }

            return u;
        }
        //删除采集的用户
        private async Task DeleteGroupChannelBotUser(int id)
        {
            try
            {
                var r = await _httpClient.GetAsync(_domain + "GroupChannelBotUser/Delete/" + id);
                if (r.IsSuccessStatusCode)
                {
                    Debug.WriteLine(id + ":成功在服务器对其进行删除");
                }
                else
                {
                    Debug.WriteLine(id + ":服务器对其进行删除时出错:" + r.StatusCode);
                }
            }
            catch
            {
                Debug.WriteLine(id + ":删除其时出错");
            }
        }
        //鼠标移入行
        private void dataGridView_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0) // 确保在行内操作
            {
                // 获取鼠标所在的行
                DataGridViewRow row = dataGridView.Rows[e.RowIndex];

                // 设置行的字体样式为粗体
                row.DefaultCellStyle.Font = new Font(dataGridView.DefaultCellStyle.Font, FontStyle.Bold);
            }
        }
        //鼠标离开行
        private void dataGridView_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0) // 确保在行内操作
            {
                // 获取离开的行
                DataGridViewRow row = dataGridView.Rows[e.RowIndex];

                // 恢复行的字体样式为默认值
                row.DefaultCellStyle.Font = dataGridView.DefaultCellStyle.Font;
            }
        }
        //换IP
        private async Task<string> ChangeIp()
        {
            var now = DateTime.Now;
        start:
            var client = new RestClient(new RestClientOptions { MaxTimeout = -1 });
            var request = new RestRequest("http://192.168.188.1/goform/formJsonAjaxReq", Method.Post);
            request.AddHeader("Cookie", "userLanguage=CN; username=admin");
            request.AddHeader("Content-Type", "text/plain");
            var body = @"{""action"":""do_reboot""}";
            request.AddParameter("text/plain", body, ParameterType.RequestBody);
            RestResponse response = await client.ExecuteAsync(request);
            Debug.WriteLine("响应体:" + response.Content);
            Debug.WriteLine("重启路由换IP中...");
            await Task.Delay(20000);
            var http = new HttpClient();
            http.Timeout = TimeSpan.FromSeconds(1);
            var nextIp = string.Empty;
            while (string.IsNullOrEmpty(nextIp))
            {
                try
                {
                    nextIp = await http.GetStringAsync(_domain + "System/Ip");
                    Debug.WriteLine("更换到了IP");
                }
                catch
                {
                    if ((DateTime.Now - now).TotalSeconds > 90)
                    {
                        Debug.WriteLine("超过90秒没更换成功IP，继续重启路由更换IP");
                        break;
                    }
                }
            }
            http.Dispose();
            if (string.IsNullOrEmpty(nextIp))
            {
                goto start;
            }
            Debug.WriteLine("更换IP至" + nextIp + " 花了" + (DateTime.Now - now).TotalSeconds + "秒");
            return nextIp;
        }
        //获取当前IP
        private async Task<string> GetCurrentIp()
        {
            var http = new HttpClient();
            http.Timeout = TimeSpan.FromSeconds(0.65);
            var ip = string.Empty;
        getIp:
            try
            {
                ip = await http.GetStringAsync(_domain + "System/Ip");
            }
            catch
            {
                goto getIp;
            }
            http.Dispose();
            return ip;
        }

        //拨号换IP方式
        private void comboBox_changeIp_SelectedIndexChanged(object sender, EventArgs e)
        {
            _changeIpType = comboBox_changeIp.SelectedIndex;
        }

        //勾选取消浏览器可视化窗口
        private void checkBox_browser_CheckedChanged(object sender, EventArgs e)
        {
            var acs = _accountContexts.Where(u => u.IsRuning);
            foreach (var ac in acs)
            {
                ac.IsShowBrowser = checkBox_browser.Checked;
                Window.ShowWindow(ac.IntPrt, checkBox_browser.Checked ? 5 : 0);
            }
        }

        private GroupChannelBotUser? CheckUrl(string url)
        {
            var item = new GroupChannelBotUser();
            item.Url = url;
            HtmlAgilityPack.HtmlDocument doc;
        getHtml:
            //如果断网了
            try
            {
                doc = web.Load("https://t.me/" + item.Url);
            }
            catch (Exception ec)
            {
                Thread.Sleep(1000);
                Debug.WriteLine("×    断网中,重复请求中..." + ec.Message);
                goto getHtml;
            }

            var document = doc.DocumentNode;
            var extra = document.QuerySelector(".tgme_page_extra");

            //不存在
            if (extra == null)
                return null;

            item.Name = document.QuerySelector(".tgme_page_title")?.InnerText?.Trim();
            item.Describle = document.QuerySelector(".tgme_page_description")?.InnerText?.Trim();

            //0群组  1频道  2机器人 3用户
            if (extra.InnerText.Contains("subscriber"))
            {
                var str = GetText.Get123(extra.InnerText);
                item.Members = string.IsNullOrEmpty(str) ? 1 : Convert.ToInt32(str);
                item.Type = 1;
                Debug.WriteLine("频道 https://t.me/" + item.Url + "  " + item.Members + "订阅 名称:" + item.Name);

            }
            else if (extra.InnerText.Contains("member"))
            {
                //有在线数
                if (extra?.InnerText.IndexOf("online") > -1)
                {
                    //会员数
                    var memberStr = GetText.Getleft(extra.InnerText, "members").Replace(" ", "");
                    item.Members = Convert.ToInt32(memberStr);

                    //在线数
                    var onlinesStr = GetText.GetBetween(extra.InnerText.Replace(" ", ""), "members,", "online");
                    item.Onlines = Convert.ToInt32(onlinesStr);
                }
                else
                {
                    var str = GetText.Get123(extra.InnerText);
                    item.Members = string.IsNullOrEmpty(str) ? 1 : Convert.ToInt32(str);
                    item.Onlines = 0;
                }

                item.Type = 0;
                Debug.WriteLine("群组 https://t.me/" + item.Url + "  " + item.Onlines + "在线 " + item.Members + "成员 群名:" + item.Name);
            }
            else if (extra.InnerText.IndexOf("@") > -1 && document.QuerySelector("title")?.InnerText?.ToLower()?.IndexOf("bot") > -1)
            {
                item.Type = 2;
                item.Members = 0;
                item.Onlines = 0;
                Debug.WriteLine("机器人 https://t.me/" + item.Url + " 名称:" + item.Name);
            }
            else
            {
                item.Type = 3;
                Debug.WriteLine("用户 https://t.me/" + item.Url + " 昵称:" + item.Name);
            }

            return item;
        }

        #region 全部群
        HashSet<GroupChannelBotUser> _groups = new();
        HtmlWeb web = new();
        private async void button_getAllGroups_ClickAsync(object sender, EventArgs e)
        {
            button_getAllGroups.Enabled = false;
            button_getAllGroups.Text = "获取群中..";
            var res = string.Empty;
            try
            {
                res = await _httpClient.GetStringAsync(_domain + "GroupChannelBotUser?type=0");
            }
            catch (Exception ex)
            {
                MessageBox.Show("获取全部群时出错:" + ex.Message);
                return;
            }

            _groups = JsonConvert.DeserializeObject<HashSet<GroupChannelBotUser>>(res);
            await File.WriteAllTextAsync("groups.json", JsonConvert.SerializeObject(_groups));
            button_getAllGroups.Text = "已获全部群";
            dataGridView_groups.RowCount = _groups.Count;
        }
        private void dataGridView_groups_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView_groups.RowCount && e.ColumnIndex >= 0 && e.ColumnIndex < dataGridView_groups.ColumnCount && _groups.Count > e.RowIndex)
            {
                var g = _groups.ElementAt(e.RowIndex);
                var dgv = (DataGridView)sender;
                var row = dgv.Rows[e.RowIndex];
                var cell = row.Cells[e.ColumnIndex];

                switch (e.ColumnIndex)
                {
                    case 0:
                        e.Value = e.RowIndex + 1;
                        cell.ToolTipText = "GroupId:" + g.Id + "会员数:" + g.Members + "  在线数:" + g.Onlines;
                        break;
                    case 1:
                        e.Value = g.Name;
                        break;
                    case 2:
                        e.Value = g.Describle;
                        break;
                    case 3:
                        e.Value = "选择";
                        break;
                }

                if (_collectionGroups.Any(u => u.Id.Equals(g.Id)))
                {
                    row.DefaultCellStyle.BackColor = Color.LightGreen;
                }
                else
                {
                    if (e.ColumnIndex == 3 && !string.IsNullOrEmpty(g.Name) && _keywords.Any(u => g.Name.Contains(u))
                  || e.ColumnIndex == 4 && !string.IsNullOrEmpty(g.Describle) && _keywords.Any(u => g.Describle.Contains(u)))
                    {
                        row.DefaultCellStyle.BackColor = Color.LightGray;
                    }
                }
            }
        }
        private void dataGridView_groups_SelectionChanged(object sender, EventArgs e)
        {
            dataGridView_groups.ClearSelection();
        }
        private void dataGridView_groups_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0) // 确保在行内操作
            {
                // 获取鼠标所在的行
                var row = dataGridView_groups.Rows[e.RowIndex];

                // 设置行的字体样式为粗体
                row.DefaultCellStyle.Font = new Font(dataGridView_groups.DefaultCellStyle.Font, FontStyle.Bold);
            }
        }
        private void dataGridView_groups_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // 确保在行内操作
            {
                // 获取离开的行
                var row = dataGridView_groups.Rows[e.RowIndex];

                // 恢复行的字体样式为默认值
                row.DefaultCellStyle.Font = dataGridView_groups.DefaultCellStyle.Font;
            }
        }
        private void button_loadSelect_Click(object sender, EventArgs e)
        {
            if (!_collectionGroups.Any())
            {
                MessageBox.Show("请先获取缅甸群");
                return;
            }

            button_loadSelect.Enabled = false;
            Debug.WriteLine("开始筛选");
            var result = _groups
                .Where(group => _keywords.Any(c => group.Name.Contains(c) || !string.IsNullOrEmpty(group.Describle) && group.Describle.Contains(c)))
                .Where(u => u.Describle == null && !u.Name.Contains("腾龙") || u.Describle != null && !u.Describle.Contains("腾龙") && !u.Name.Contains("腾龙")).ToHashSet();

            _groups = new HashSet<GroupChannelBotUser>();
            dataGridView_groups.RowCount = 0;
            dataGridView_groups.Refresh();
            _groups = result;
            Debug.WriteLine("筛选完毕");
            dataGridView_groups.RowCount = _groups.Count;
            dataGridView_groups.Refresh();
        }
        private async void dataGridView_groups_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var rowIndex = e.RowIndex;
            if (rowIndex == -1)
                return;

            if (e.ColumnIndex == 4)
            {
                var gId = _groups.ElementAt(e.RowIndex).Id;
                Debug.WriteLine(gId);
                try
                {
                    var response = await _httpClient.GetAsync(_domain + "GroupChannelBotUser/SetCategory?id=" + gId + "&categoryEnum=1");

                    if (!response.IsSuccessStatusCode)
                    {
                        Debug.WriteLine("提交不成功");
                        return;
                    }

                    _groups.ElementAt(e.RowIndex).CategoryEnum = 1;
                    dataGridView_collectionGroups.RowCount++;
                    _collectionGroups.Add(_groups.ElementAt(e.RowIndex));
                }
                catch
                {

                }

            }
        }
        private async void button_checkGroups_Click(object sender, EventArgs e)
        {
            button_checkGroups.Enabled = false;
            var scope = _groups.Count / 3;
            List<List<GroupChannelBotUser>> tenList = new();
            for (int i = 0; i < 3; i++)
            {
                int startNumber = i == 0 ? 0 : i * scope + 1;
                int endNumber = startNumber + scope;
                var r = _groups.ToList().GetRange(startNumber, scope);
                tenList.Add(r);
            }

            var endCount = 0;
            foreach (var items in tenList)
            {
                var t = new Thread(async () =>
                {
                    for (int i = items.Count - 1; i > -1; i--)
                    {
                        var item = CheckUrl(items[i].Url);
                        if (item == null || item.Type == 3)
                        {
                            await DeleteGroupChannelBotUser(items[i].Id);
                            Debug.WriteLine("https://t.me/" + items[i].Url + " 不存在,或者是用户.进行删除");
                            continue;
                        }
                        item.Id = items[i].Id;
                        items[i] = item;
                        try
                        {
                            var response = await _httpClient.PutAsync(_domain + "GroupChannelBotUser", new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json"));
                            if (!response.IsSuccessStatusCode)
                                Debug.WriteLine($"信息更新出错: {response.StatusCode}");
                        }
                        catch
                        {
                        }
                    }
                    endCount++;
                });

                t.Start();
            }

            while (endCount >= 3)
            {
                button_checkGroups.Enabled = true;
                MessageBox.Show("全部群检查完成");
                await Task.Delay(1000);
            }
        }
        #endregion

        #region 缅甸群       
        List<GroupChannelBotUser> _collectionGroups = new();
        //新采集的，等待归类的
        List<GroupChannelBotUser> _newCollectionGroups = new();
        private async void button_getCollectionGroups_Click(object sender, EventArgs e)
        {
            button_getCollectionGroups.Enabled = false;
            button_getCollectionGroups.Text = "获取群中..";
            var res = string.Empty;
            try
            {
                res = await _httpClient.GetStringAsync(_domain + "GroupChannelBotUser?type=0&categoryEnum=1");
            }
            catch (Exception ex)
            {
                MessageBox.Show("获取缅甸群时出错:" + ex.Message);
                return;
            }

            _collectionGroups = new List<GroupChannelBotUser>();
            _collectionGroups = JsonConvert.DeserializeObject<List<GroupChannelBotUser>>(res);
            button_getCollectionGroups.Text = "已获全部群";
            dataGridView_collectionGroups.RowCount = _collectionGroups.Count;
            dataGridView_collectionGroups.Refresh();
        }
        private void dataGridView_collectionGroups_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView_collectionGroups.RowCount && e.ColumnIndex >= 0 && e.ColumnIndex < dataGridView_collectionGroups.ColumnCount && _collectionGroups.Count > e.RowIndex)
            {
                var cg = _collectionGroups[e.RowIndex];
                var dgv = (DataGridView)sender;
                var row = dgv.Rows[e.RowIndex];
                var cell = row.Cells[e.ColumnIndex];

                switch (e.ColumnIndex)
                {
                    case 0:
                        e.Value = cg.Id;
                        break;
                    case 1:
                        e.Value = cg.Url;
                        break;
                    case 2:
                        e.Value = cg.Name;
                        cell.ToolTipText = e.RowIndex.ToString() + "  GroupId:" + cg.Number + "  人数:" + cg.Members + "  在线:" + cg.Onlines;
                        break;
                    case 3:
                        e.Value = cg.Describle;
                        break;
                    case 4:
                        e.Value = "删除";
                        break;
                }
            }
        }

        private void dataGridView_collectionGroups_SelectionChanged(object sender, EventArgs e)
        {
            dataGridView_collectionGroups.ClearSelection();
        }

        private void dataGridView_collectionGroups_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // 确保在行内操作
            {
                // 获取离开的行
                var row = dataGridView_collectionGroups.Rows[e.RowIndex];

                // 恢复行的字体样式为默认值
                row.DefaultCellStyle.Font = dataGridView_collectionGroups.DefaultCellStyle.Font;
            }
        }

        private void dataGridView_collectionGroups_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0) // 确保在行内操作
            {
                // 获取鼠标所在的行
                var row = dataGridView_collectionGroups.Rows[e.RowIndex];

                // 设置行的字体样式为粗体
                row.DefaultCellStyle.Font = new Font(dataGridView_collectionGroups.DefaultCellStyle.Font, FontStyle.Bold);
            }
        }
        //删除缅甸群列表项记录
        private async void dataGridView_collectionGroups_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var rowIndex = e.RowIndex;
            if (rowIndex == -1)
                return;

            var row = dataGridView_collectionGroups.Rows[rowIndex];
            var cg = _collectionGroups[rowIndex];
            var cell = row.Cells[e.ColumnIndex];

            if (e.ColumnIndex == 4)
            {
                var response = await _httpClient.GetAsync(_domain + "GroupChannelBotUser/SetCategory?id=" + cg.Id);
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("删除失败:" + response.StatusCode);
                    return;
                }

                dataGridView_collectionGroups.RowCount--;
                _collectionGroups.Remove(cg);
                dataGridView_collectionGroups.Refresh();
            }
        }

        private async void button_collectionGroupMember_Click(object sender, EventArgs e)
        {
            var isStart = button_collectionGroupMember.Text == "采集群员";
            if (isStart)
            {
                //无可用账号
                if (!_accountContexts.Any(u => !u.IsExpired))
                {
                    MessageBox.Show("采集群员失败，无可用账号");
                    return;
                }

                button_collectionGroupMember.Enabled = false;
                button_collectionGroupMember.Text = "采集群员中";
                var acs = _accountContexts
                        .Where(ac => ac.DeadlineLimitedTime == null || ac.DeadlineLimitedTime <= DateTime.Now)
                        .Where(ac => !ac.IsExpired && !ac.IsBanned).Take(5);

                var index = 0;
                var fors = 0;
                Parallel.ForEach(acs, async ac =>
                {
                    ac.IsRuning = true;
                    fors++;
                    await Task.Delay(2000 * fors);
                    ac.IsShowBrowser = checkBox_browser.Checked;
                    if (ac.Browser == null || !ac.Browser.IsConnected || ac.Context == null || ac.Page == null || ac.Page.IsClosed)
                        await NewAccountBrowser(ac);

                    while (_collectionGroups.Count > index && isStart)
                    {
                        Debug.WriteLine(index + " 当前索引");
                        var group = _collectionGroups[index];
                        index++;
                        if (group.LastCollectionUserTime != null)
                        {
                            Debug.WriteLine(index + " 已经采集过群员了,跳过");
                            continue;
                        }

                        var members = await GetGroupMembers(ac, group.Url);
                        //通知本群已采集成员
                        try
                        {
                            await _httpClient.GetStringAsync("http://156.247.9.173/GroupChannelBotUser/SetLastCollectionUserTime/" + group.Id);
                            AddConsoleLog(ac, "成功向后台告知已采集本群");
                        }
                        catch
                        {
                            AddConsoleLog(ac, "未成功向后台告知已采集本群");
                        }
                        if (members == null || !members.Any())
                        {
                            AddConsoleLog(ac, "未采集到用户集合");
                            continue;
                        }

                        var postUsers = new List<GroupChannelBotUser>();
                        foreach (var member in members)
                        {
                            member.Type = 3;
                            member.CategoryEnum = 1;

                        openGroupPage:
                            try
                            {
                                await ac.Page.GotoAsync("https://web.telegram.org/k/#" + member.Number, new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle, Timeout = 1500 });
                            }
                            catch
                            {
                                goto openGroupPage;
                            }

                            try
                            {
                                await ac.Page.Locator($"[data-peer-id='{member.Number}']").WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 1000 });
                                member.Url = await ac.Page.TextContentAsync("div.tgico-username div.row-title", new PageTextContentOptions { Timeout = 200 });
                                if (!string.IsNullOrEmpty(member.Url))
                                {
                                    member.Url = member?.Url?.ToLower();
                                    member.PhoneNumber = await ac.Page.TextContentAsync("div.tgico-phone div.row-title", new PageTextContentOptions { Timeout = 100 });
                                }
                            }
                            catch
                            {
                            }

                            if (string.IsNullOrEmpty(member.Url) && ac.Page.Url.Contains("@") && !postUsers.Any(u => u.Number == member.Number))
                            {
                                string bodyContent = string.Empty;
                                try
                                {
                                    bodyContent = await ac.Page.InnerTextAsync("body");
                                }
                                catch
                                {
                                }

                                if (!string.IsNullOrEmpty(bodyContent))
                                {

                                    if (!_noNeedCollectionUserIds.Contains((long)member.Number))
                                        _noNeedCollectionUserIds.Add((long)member.Number);
                                    continue;
                                }

                                member.Url = ac.Page.Url.Replace("https://web.telegram.org/k/#@", "").ToLower();
                            }

                            if (string.IsNullOrEmpty(member.Url) || postUsers.Any(u => u.Url == member.Url || u.Number == member.Number))
                            {
                                if (!_noNeedCollectionUserIds.Contains((long)member.Number))
                                    _noNeedCollectionUserIds.Add((long)member.Number);
                                continue;
                            }

                            var html = string.Empty;
                        checkMember:
                            try
                            {
                                html = await _httpClient.GetStringAsync("https://t.me/" + member.Url);
                            }
                            catch
                            {
                                goto checkMember;
                            }

                            // 创建HtmlDocument对象
                            var htmlDoc = new HtmlAgilityPack.HtmlDocument();
                            // 加载HTML内容（可以从文件、字符串或Web页面加载）
                            htmlDoc.LoadHtml(html);
                            var desc = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='tgme_page_description ']");

                            //判断是否合规
                            if (!string.IsNullOrEmpty(member.Name) && _removeKeywords.Any(c => member.Name.Contains(c))
                                || desc != null && !string.IsNullOrEmpty(desc.InnerText) && _removeKeywords.Any(c => desc.InnerText.Contains(c))
                                || !TextHelper.IsChinese(member.Name) && !TextHelper.IsChinese(desc?.InnerText))
                            {
                                AddConsoleLog(ac, member.Url + ":用户名或者介绍不合规：" + member.Name + "===" + desc?.InnerText);
                                if (!_noNeedCollectionUserIds.Contains((long)member.Number))
                                    _noNeedCollectionUserIds.Add((long)member.Number);
                                continue;
                            }

                            member.Describle = desc?.InnerText;
                            postUsers.Add(member);
                        }

                        if (!postUsers.Any())
                        {
                            AddConsoleLog(ac, "没有可提交的用户");
                            continue;
                        }

                        AddConsoleLog(ac, $"本次可提交成员数:" + postUsers.Count);
                        var isOk = await PostAsync(postUsers);
                        if (isOk)
                        {
                            try
                            {
                                await System.IO.File.WriteAllLinesAsync("nonCompliantUserIds.txt", _noNeedCollectionUserIds.Select(u => u.ToString()), Encoding.UTF8);
                            }
                            catch
                            {
                            }
                        }
                    }

                    ac.IsShowBrowser = false;
                    ac.IsRuning = false;
                    ac.ActionState = "";
                    if (ac.CancelTimerSource != null && !ac.CancelTimerSource.IsCancellationRequested)
                        ac.CancelTimerSource.Cancel();
                });

                while (_accountContexts.Any(a => a.IsRuning))
                    await Task.Delay(1000);

                button_collectionGroupMember.Text = "采集群员";
                button_collectionGroupMember.Enabled = true;
                MessageBox.Show("执行完成");
            }
            else
            {
                button_collectionGroupMember.Enabled = true;
                button_collectionGroupMember.Text = "采集群员";

                foreach (var ac in _accountContexts)
                {
                    if (ac.IsRuning)
                    {
                        ac.IsRuning = false;
                        ac.ActionState = "";

                        if (ac.CancelTimerSource != null && !ac.CancelTimerSource.IsCancellationRequested)
                            ac.CancelTimerSource.Cancel();
                    }
                }
            }
        }

        //获取群成员的Ids
        private async Task<HashSet<GroupChannelBotUser>> GetGroupMembers(AccountContext ac, string url)
        {
            var users = new HashSet<GroupChannelBotUser>();

        opengroup:
            try
            {
                await ac.Page.GotoAsync("about:blank");
                await ac.Page.GotoAsync("https://web.telegram.org/k/#@" + url, new PageGotoOptions { Timeout = 120000 });
            }
            catch
            {
                AddConsoleLog(ac, "网速不给力,未打开网页");
                goto opengroup;
            }

            try
            {
                var title = ac.Page.Locator("div.person>div.content>div.top");
                await title.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 20000 });
                await title.ClickAsync(new LocatorClickOptions { Delay = 5000 });
            }
            catch
            {
                AddConsoleLog(ac, "20秒都未发现标题头,跳过当前群");
                return users;
            }

            try
            {
                await ac.Page.EvaluateHandleAsync(@"(selector) => { document.querySelector(selector).remove(); }", "#column-left");
            }
            catch
            {
            }

            //最后的HTML
            var endHtmlLength = 0;
            //重复滚动了多少次
            var getLists = 0;

        getList:
            if (getLists == 0)
            {
                try
                {
                    var title = "div.chat-info-container > div.chat-info";
                    await ac.Page.WaitForSelectorAsync(title, new PageWaitForSelectorOptions { State = WaitForSelectorState.Attached });
                    await ac.Page.ClickAsync(title);
                    AddConsoleLog(ac, "点击了群信息头");
                }
                catch
                {
                    AddConsoleLog(ac, "没发现群信息头");
                    goto opengroup;
                }

                try
                {
                    await ac.Page.WaitForSelectorAsync("#column-right div.search-super-container-members.tabs-tab.active > div > ul > a", new PageWaitForSelectorOptions { State = WaitForSelectorState.Attached, Timeout = 15000 });
                    AddConsoleLog(ac, "获取到了用户列表");
                }
                catch
                {
                    AddConsoleLog(ac, "没有用户列表");
                    return users;
                }

                await ac.Page.EvaluateHandleAsync(@"(selector) => { document.querySelector(selector).remove(); }", "#column-center");
                Thread.Sleep(1500);

                try
                {
                    await ac.Page.HoverAsync("div.search-super-container-members.tabs-tab.active > div > ul");
                }
                catch
                {
                    AddConsoleLog(ac, "没有获取到滚动区域");
                    return users;
                }
            }
            else
            {
                Thread.Sleep(500);
            }
            //拉到最下面
            await ac.Page.Mouse.WheelAsync(0, 100000);
            await ac.Page.WaitForLoadStateAsync();
            IReadOnlyList<IElementHandle>? members = null;
            try
            {
                members = await ac.Page.QuerySelectorAllAsync("#column-right ul>a");
            }
            catch (Exception ex)
            {
                AddConsoleLog(ac, "获取群成员列表时出错：" + ex.Message);
                goto opengroup;
            }

            getLists = endHtmlLength > 0 && endHtmlLength == members.Count ? getLists + 1 : 1;
            if (getLists < 11)
            {
                AddConsoleLog(ac, getLists > 1 ? "重复第" + getLists + "次翻页" : "滚动条向下拉");
                endHtmlLength = members.Count;
                goto getList;
            }
            AddConsoleLog(ac, "滚动完成,本群有" + members?.Count + "个群成员");
            if (members != null && members.Any())
            {
                foreach (var member in members)
                {
                    var user = new GroupChannelBotUser();
                    string aText = string.Empty;
                    try
                    {
                        aText = await member.TextContentAsync();
                    }
                    catch
                    {
                        continue;
                    }

                    try
                    {
                        var uid = await member.GetAttributeAsync("data-peer-id");
                        var userId = Convert.ToInt64(uid);
                        user.Number = userId;
                        if (_noNeedCollectionUserIds.Contains(userId)) continue;
                        _noNeedCollectionUserIds.Add(userId);
                    }
                    catch
                    {
                        continue;
                    }


                    if (_removeKeywords.Any(u => aText.Contains(u))) continue;
                    user.Name = aText.Replace("last seen recently", "").Replace("last seen within a week", "");
                    users.Add(user);
                }

                members = null;
                AddConsoleLog(ac, "有" + users?.Count + "个合格群成员");
                try
                {
                    await ac.Page.EvaluateHandleAsync(@"(selector) => { document.querySelector(selector).remove(); }", "#emoji-dropdown");
                    await ac.Page.EvaluateHandleAsync(@"(selector) => { document.querySelector(selector).remove(); }", "#svg-defs");
                    await ac.Page.EvaluateHandleAsync(@"(selector) => { document.querySelector(selector).remove(); }", "head");
                    await ac.Page.EvaluateHandleAsync(@"(selector) => { document.querySelector(selector).style=''; }", "html");
                }
                catch
                {
                }
            }
            return users;
        }

        //提交群成员到后台
        private async Task<bool> PostAsync(List<GroupChannelBotUser> postUsers)
        {
            HttpResponseMessage? response = null;
            try
            {
                response = await _httpClient.PostAsync("http://156.247.9.173/GroupChannelBotUser",
                    new StringContent(
                        JsonConvert.SerializeObject(new { GroupChannelBotUsers = postUsers }),
                        Encoding.UTF8,
                        "application/json"));
            }
            catch
            { }

            if (response == null)
                return false;

            // 确保响应成功
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("√ 把采集的群成员向后台提交成功");
            }
            else
            {
                Console.WriteLine("× 提交群成员到服务器时失败，状态码：" + response.StatusCode);
            }

            return response.IsSuccessStatusCode;
        }


        private void button_botJoinGroup_Click(object sender, EventArgs e)
        {

        }
        #endregion

        private async void ChangeIp(object sender, EventArgs e)
        {
            button_changIp.Enabled = false;
            await ChangeIp();
            button_changIp.Enabled = true;
        }

        //校正备份
        private async void button_correctionBackups_Click(object sender, EventArgs e)
        {
            button_correctionBackups.Enabled = false;
            try
            {
                await _httpClient.GetStringAsync(_domain + "Accounts/CorrectionBackups");
            }
            catch (Exception ex)
            {
                MessageBox.Show("校正失败:" + ex.Message);
            }
            button_correctionBackups.Enabled = true;

            Application.Restart();
        }
    }

    public class UserHtml
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public string? UserName { get; set; }
        public string? Name { get; set; }
        public string? Introduce { get; set; }
        public bool IsHead { get; set; }
    }
}