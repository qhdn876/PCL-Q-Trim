Public Class PageOtherLeft

    Private IsLoad As Boolean = False
    Private IsPageSwitched As Boolean = False '如果在 Loaded 前切换到其他页面，会导致触发 Loaded 时再次切换一次
    Private Sub PageOtherLeft_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        QuanLayerApply(Me, True)
        '更多页只剩“关于与鸣谢”一个子页面，首次加载时直接选中
        If IsLoad Then Return
        IsLoad = True
        If IsPageSwitched Then Return
        ItemAbout.SetChecked(True, False, False)
    End Sub
    Private Sub PageOtherLeft_Unloaded(sender As Object, e As RoutedEventArgs) Handles Me.Unloaded
        IsPageSwitched = False
    End Sub

#Region "页面切换"

    ''' <summary>
    ''' 当前页面的编号。从 0 开始计算。
    ''' </summary>
    Public PageID As FormMain.PageSubType
    Public Sub New()
        InitializeComponent()
        PageID = FormMain.PageSubType.OtherAbout
    End Sub

    ''' <summary>
    ''' 勾选事件改变页面。
    ''' </summary>
    Private Sub PageCheck(sender As FrameworkElement, e As RouteEventArgs) Handles ItemAbout.Check
        '尚未初始化控件属性时，sender.Tag 为 Nothing，会导致切换到页面 0
        If sender.Tag IsNot Nothing Then PageChange(Val(sender.Tag))
    End Sub

    Public Function PageGet(Optional ID As FormMain.PageSubType = -1)
        If ID = -1 Then ID = PageID
        If FrmOtherAbout Is Nothing Then FrmOtherAbout = New PageOtherAbout
        Return FrmOtherAbout
    End Function

    ''' <summary>
    ''' 切换现有页面。
    ''' </summary>
    Public Sub PageChange(ID As FormMain.PageSubType)
        If PageID = ID Then Return
        AniControlEnabled += 1
        IsPageSwitched = True
        Try
            PageChangeRun(PageGet(ID))
            PageID = ID
        Catch ex As Exception
            Logger.Error(ex, $"切换分页面失败（ID {ID}）")
        Finally
            AniControlEnabled -= 1
        End Try
    End Sub
    Private Shared Sub PageChangeRun(Target As MyPageRight)
        AniStop("FrmMain PageChangeRight") '停止主页面的右页面切换动画，防止它与本动画一起触发多次 PageOnEnter
        If Target.Parent IsNot Nothing Then Target.SetValue(ContentPresenter.ContentProperty, Nothing)
        FrmMain.PageRight = Target
        CType(FrmMain.PanMainRight.Child, MyPageRight).PageOnExit()
        AniStart({
                         AaCode(Sub()
                                    CType(FrmMain.PanMainRight.Child, MyPageRight).PageOnForceExit()
                                    FrmMain.PanMainRight.Child = FrmMain.PageRight
                                    FrmMain.PageRight.Opacity = 0
                                End Sub, 130),
                         AaCode(Sub()
                                    '延迟触发页面通用动画，以使得在 Loaded 事件中加载的控件得以处理
                                    FrmMain.PageRight.Opacity = 1
                                    FrmMain.PageRight.PageOnEnter()
                                End Sub, 30, True)
                     }, "PageLeft PageChange")
    End Sub

#End Region

End Class
