#Persistent  ; 保持脚本一直运行
SetTimer, CloseLicensesWindow, 10000  ; 每10秒（10000毫秒）运行一次CloseLicensesWindow函数
Return

CloseLicensesWindow:
    ; 查找标题为 "Licenses" 的窗口
    WinClose, Licenses
    Return