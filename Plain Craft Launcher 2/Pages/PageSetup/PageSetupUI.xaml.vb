''' <summary>
''' 原「个性化」设置页已整体移除，导航中不再有入口。
''' 此类型仅保留功能隐藏与背景刷新所需的静态入口，页面本身是一张空壳。
''' </summary>
Public Class PageSetupUI

#Region "功能隐藏"

    Private Shared _HiddenForceShow As Boolean = False
    ''' <summary>
    ''' 是否强制显示被禁用的功能。
    ''' </summary>
    Public Shared Property HiddenForceShow As Boolean
        Get
            Return _HiddenForceShow
        End Get
        Set(value As Boolean)
            _HiddenForceShow = value
            HiddenRefresh()
        End Set
    End Property

    ''' <summary>
    ''' 更新功能隐藏带来的显示变化。
    ''' 设置页已移除，这里只负责联机入口的固定隐藏与横竖栏刷新。
    ''' </summary>
    Public Shared Sub HiddenRefresh()
        If FrmMain.PanTitleSelect Is Nothing OrElse Not FrmMain.PanTitleSelect.IsLoaded Then Return
        Try
            '顶部栏
            FrmMain.PanTitleSelect.Visibility = Visibility.Visible
            FrmMain.BtnTitleSelect1.Visibility = If(Not HiddenForceShow AndAlso Settings.Get(Of Boolean)("UiHiddenPageDownload"), Visibility.Collapsed, Visibility.Visible)
            FrmMain.BtnTitleSelect2.Visibility = Visibility.Collapsed
            FrmMain.BtnTitleSelect3.Visibility = If(Not HiddenForceShow AndAlso Settings.Get(Of Boolean)("UiHiddenPageSetup"), Visibility.Collapsed, Visibility.Visible)
            FrmMain.BtnTitleSelect4.Visibility = If(Not HiddenForceShow AndAlso Settings.Get(Of Boolean)("UiHiddenPageOther"), Visibility.Collapsed, Visibility.Visible)
            '功能
            FrmLaunchLeft.RefreshButtonsUI()
            '版本设置子页面
            If FrmInstanceLeft IsNot Nothing Then
                FrmInstanceLeft.ItemModify.Visibility = If(Not HiddenForceShow AndAlso Settings.Get(Of Boolean)("UiHiddenFunctionInstanceModify"), Visibility.Collapsed, Visibility.Visible)
            End If
            '设置子页面
            If FrmSetupLeft IsNot Nothing Then
                FrmSetupLeft.ItemLaunch.Visibility = If(Not HiddenForceShow AndAlso Settings.Get(Of Boolean)("UiHiddenSetupLaunch"), Visibility.Collapsed, Visibility.Visible)
                FrmSetupLeft.ItemLink.Visibility = Visibility.Collapsed
                FrmSetupLeft.ItemSystem.Visibility = If(Not HiddenForceShow AndAlso Settings.Get(Of Boolean)("UiHiddenSetupSystem"), Visibility.Collapsed, Visibility.Visible)
                '只剩两项时不再显示左栏
                Dim AvaliableCount As Integer = 0
                If Not Settings.Get(Of Boolean)("UiHiddenSetupLaunch") Then AvaliableCount += 1
                If Not Settings.Get(Of Boolean)("UiHiddenSetupSystem") Then AvaliableCount += 1
                FrmSetupLeft.PanItem.Visibility = If(AvaliableCount < 2 AndAlso Not HiddenForceShow, Visibility.Collapsed, Visibility.Visible)
            End If
            '更多子页面只剩“关于与鸣谢”，左栏始终隐藏
            If FrmOtherLeft IsNot Nothing Then FrmOtherLeft.PanItem.Visibility = Visibility.Collapsed
            '各个页面的入口
            If FrmMain.PageCurrent = FormMain.PageType.InstanceSelect Then FrmSelectRight.BtnEmptyDownload_Loaded()
            If FrmMain.PageCurrent = FormMain.PageType.Launch Then FrmLaunchLeft.RefreshButtonsUI()
            If FrmMain.PageCurrent = FormMain.PageType.InstanceSetup AndAlso FrmInstanceModDisabled IsNot Nothing Then FrmInstanceModDisabled.BtnDownload_Loaded()
        Catch ex As Exception
            Logger.Error(ex, "刷新功能隐藏项目失败")
        End Try
    End Sub

#End Region

    ''' <summary>
    ''' 刷新背景图片。壁纸已内嵌且固定，此处仅保留提示。
    ''' </summary>
    ''' <param name="IsHint">是否显示刷新提示。</param>
    ''' <param name="Refresh">是否刷新图片显示。</param>
    Public Shared Sub BackgroundRefresh(IsHint As Boolean, Refresh As Boolean)
        If Refresh AndAlso IsHint Then Hint("背景已固定为内置壁纸。", HintType.Green)
    End Sub

End Class
