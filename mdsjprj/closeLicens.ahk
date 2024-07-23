#Persistent  ; 保持脚本一直运行
SetTimer, CloseLicensesWindow, 1000  ; 每10秒（10000毫秒）运行一次CloseLicensesWindow函数
Return

CloseLicensesWindow:
  ; 查找类名为 "SunAwtDialog" 的窗口
    WinClose, ahk_class SunAwtDialog
    ; 查找标题为 "Licenses" 的窗口
    WinClose, Licenses
      WinClose,Exit GoLand Without License Activation
    Return