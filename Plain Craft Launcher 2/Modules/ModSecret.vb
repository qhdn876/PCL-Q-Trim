'由于包含加解密等安全信息，本文件中的部分代码已被删除

Friend Module ModSecret

    '标注 PCL 的不同分支，仅用于替换标记
    Public Const VersionBranchMain As String = "OpenSource"
    '在开源版的注册表与常规版的注册表隔离，以防数据冲突
    Public Const RegFolder As String = "PCLDebug"
    '用于微软登录的 ClientId
    Public OAuthClientId As String = If(Environment.GetEnvironmentVariable("PCL_MS_CLIENT_ID"), "")
    'CurseForge API Key
    Public CurseForgeAPIKey As String = If(Environment.GetEnvironmentVariable("PCL_CURSEFORGE_API_KEY"), "")
    '用于匿名数据收集的腾讯云日志服务上报 URL，形如 https://{region}.cls.tencentcs.com/track?topic_id={topic_id}
    Public Const ClsBaseUrl As String = ""

#Region "网络鉴权"

    Friend Function SecretCdnSign(UrlWithMark As String) As String
        If Not UrlWithMark.EndsWithF("{CDN}") Then Return UrlWithMark
        Return UrlWithMark.Replace("{CDN}", "").Replace(" ", "%20")
    End Function
    ''' <summary>
    ''' 设置 Headers 的 UA、Referer。
    ''' </summary>
    Friend Sub SecretHeadersSign(Url As String, ByRef Req As HttpRequestMessage, Optional SimulateBrowserHeaders As Boolean = False)
        If Not Req.Headers.UserAgent.Any Then
            If Url.Contains("baidupcs.com") OrElse Url.Contains("baidu.com") Then
                Req.Headers.Add("User-Agent", "LogStatistic")  '#4951
            ElseIf SimulateBrowserHeaders Then
                Req.Headers.Add("User-Agent", $"PCL2/{VersionBaseName}.{CInt(BuildType)} Mozilla/5.0 AppleWebKit/537.36 Chrome/63.0.3239.132 Safari/537.36")
            Else
                Req.Headers.Add("User-Agent", $"PCL2/{VersionBaseName}.{CInt(BuildType)}")
            End If
        End If
        If Not SimulateBrowserHeaders Then Req.Headers.Add("Referer", $"http://{VersionCode}.open.pcl2.server/")
        If Url.Contains("api.curseforge.com") OrElse Url.Contains("forgecdn.net") Then Req.Headers.Add("x-api-key", CurseForgeAPIKey)
    End Sub

#End Region

#Region "主题"

    'PCL Quan 固定深色雾面配色（无亮暗切换）
    Public Color1 As New MyColor(58, 74, 71)          '控件描边 #3A4A47
    Public Color2 As New MyColor(110, 203, 189)       '强调悬停 #6ECBBD
    Public Color3 As New MyColor(85, 185, 172)        '强调 #55B9AC
    Public Color4 As New MyColor(63, 157, 145)        '强调按下 #3F9D91
    Public Color5 As New MyColor(167, 183, 178)       '次要文字 #A7B7B2
    Public Color6 As New MyColor(228, 236, 233)       '主文字 #E4ECE9
    Public Color7 As New MyColor(42, 53, 51)          '分隔线 #2A3533
    Public Color8 As New MyColor(27, 36, 34)          '底色 #1B2422
    Public ColorBg0 As New MyColor(30, 39, 37)        '卡片底 #1E2725
    Public ColorBg1 As New MyColor(46, 59, 57)        '选中卡片底 #2E3B39
    Public ColorGray1 As New MyColor(228, 236, 233)   '主文字
    Public ColorGray2 As New MyColor(167, 183, 178)   '次要文字
    Public ColorGray3 As New MyColor(154, 171, 166)   '辅助文字
    Public ColorGray4 As New MyColor(107, 124, 120)   '禁用文字
    Public ColorGray5 As New MyColor(58, 74, 71)      '控件描边
    Public ColorGray6 As New MyColor(42, 53, 51)      '分隔线/禁用底
    Public ColorGray7 As New MyColor(35, 45, 43)      '悬停底
    Public ColorGray8 As New MyColor(27, 36, 34)      '嵌底
    Public ColorSemiTransparent As New MyColor(1, Color8)

    Public ThemeNow As Integer = -1
    Public ColorHue As Integer = 210, ColorSat As Integer = 85, ColorLightAdjust As Integer = 0, ColorHueTopbarDelta As OneOf(Of Integer, Integer()) = 0
    Public ThemeDontClick As Integer = 0

    Public Sub ThemeRefresh(Optional NewTheme As Integer = -1)
        Try
            If ThemeNow = NewTheme AndAlso NewTheme >= 0 Then Return
            If NewTheme >= 0 Then ThemeNow = NewTheme

            'PCL Quan：固定深色雾面配色，不随主题或赞助等级变化
            Color1 = New MyColor(58, 74, 71)
            Color2 = New MyColor(110, 203, 189)
            Color3 = New MyColor(85, 185, 172)
            Color4 = New MyColor(63, 157, 145)
            Color5 = New MyColor(167, 183, 178)
            Color6 = New MyColor(228, 236, 233)
            Color7 = New MyColor(42, 53, 51)
            Color8 = New MyColor(27, 36, 34)
            ColorBg0 = New MyColor(30, 39, 37)
            ColorBg1 = New MyColor(46, 59, 57)

            ColorSemiTransparent = New MyColor(1, Color8)
            Application.Current.Resources("ColorBrushGray1") = New SolidColorBrush(ColorGray1)
            Application.Current.Resources("ColorBrushGray2") = New SolidColorBrush(ColorGray2)
            Application.Current.Resources("ColorBrushGray3") = New SolidColorBrush(ColorGray3)
            Application.Current.Resources("ColorBrushGray4") = New SolidColorBrush(ColorGray4)
            Application.Current.Resources("ColorBrushGray5") = New SolidColorBrush(ColorGray5)
            Application.Current.Resources("ColorBrushGray6") = New SolidColorBrush(ColorGray6)
            Application.Current.Resources("ColorBrushGray7") = New SolidColorBrush(ColorGray7)
            Application.Current.Resources("ColorBrushGray8") = New SolidColorBrush(ColorGray8)
            '深色主题下语义翻转：白色系输入底 → 深色半透明面板；弹层底 → 实色深卡
            Application.Current.Resources("ColorBrushHalfWhite") = New SolidColorBrush(New MyColor(150, 30, 39, 37))
            Application.Current.Resources("ColorBrushSemiWhite") = New SolidColorBrush(New MyColor(230, 30, 39, 37))
            Application.Current.Resources("ColorBrushPopupDark") = New SolidColorBrush(New MyColor(255, 36, 48, 45))
            Application.Current.Resources("ColorBrush1") = New SolidColorBrush(Color1)
            Application.Current.Resources("ColorBrush2") = New SolidColorBrush(Color2)
            Application.Current.Resources("ColorBrush3") = New SolidColorBrush(Color3)
            Application.Current.Resources("ColorBrush4") = New SolidColorBrush(Color4)
            Application.Current.Resources("ColorBrush5") = New SolidColorBrush(Color5)
            Application.Current.Resources("ColorBrush6") = New SolidColorBrush(Color6)
            Application.Current.Resources("ColorBrush7") = New SolidColorBrush(Color7)
            Application.Current.Resources("ColorBrush8") = New SolidColorBrush(Color8)
            Application.Current.Resources("ColorBrushBg0") = New SolidColorBrush(ColorBg0)
            Application.Current.Resources("ColorBrushBg1") = New SolidColorBrush(ColorBg1)
            Application.Current.Resources("ColorBrushHoverLight") = New SolidColorBrush(New MyColor(42, 52, 51))
            Application.Current.Resources("ColorBrushActive") = New SolidColorBrush(New MyColor(49, 61, 59))
            Application.Current.Resources("ColorObject1") = CType(Color1, Color)
            Application.Current.Resources("ColorObject2") = CType(Color2, Color)
            Application.Current.Resources("ColorObject3") = CType(Color3, Color)
            Application.Current.Resources("ColorObject4") = CType(Color4, Color)
            Application.Current.Resources("ColorObject5") = CType(Color5, Color)
            Application.Current.Resources("ColorObject6") = CType(Color6, Color)
            Application.Current.Resources("ColorObject7") = CType(Color7, Color)
            Application.Current.Resources("ColorObject8") = CType(Color8, Color)
            Application.Current.Resources("ColorObjectBg0") = CType(ColorBg0, Color)
            Application.Current.Resources("ColorObjectBg1") = CType(ColorBg1, Color)
            ThemeRefreshMain()
        Catch ex As Exception
            Logger.Error(ex, "刷新主题颜色失败", LogBehavior.Toast)
        End Try
    End Sub
    Public Sub ThemeRefreshMain()
        RunInUi(
        Sub()
            If Not FrmMain.IsLoaded Then Return
            '顶部条背景：深色雾面顶栏
            FrmMain.PanTitle.Background = New SolidColorBrush(Color.FromArgb(0, 0, 0, 0))
            FrmMain.PanTitle.Background.Freeze()
            '主页面背景：深色底，壁纸（预糊化）叠在上面
            FrmMain.PanForm.Background = New SolidColorBrush(Color.FromArgb(0, 0, 0, 0))
            FrmMain.PanForm.Background.Freeze()
        End Sub)
    End Sub
    'PCL Quan：页面根网格挂半透深色罩（tokens --canvas .55 / --side .66），只设视觉属性
    Public ReadOnly QuanCanvasBrush As New SolidColorBrush(Color.FromArgb(0, 0, 0, 0))
    Public ReadOnly QuanSideBrush As New SolidColorBrush(Color.FromArgb(120, 20, 42, 38))
    
    Public Sub QuanWallpaperInit()
        Try
            Dim bmp As New System.Windows.Media.Imaging.BitmapImage()
            bmp.BeginInit()
            bmp.UriSource = New Uri("pack://application:,,,/Images/wallpaper.png", UriKind.Absolute)
            bmp.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad
            bmp.EndInit()
            bmp.Freeze()
            Dim Wp As New ImageBrush(bmp) With {.Stretch = Stretch.UniformToFill}
            Wp.Freeze()
            FrmMain.ImgBack.Background = Wp
        Catch ex As Exception
            Logger.Warn(ex, "内嵌壁纸加载失败")
        End Try
    End Sub
    Public Sub QuanLayerApply(Page As Object, IsLeft As Boolean)
        Try
            Dim Root = TryCast(Page, System.Windows.Controls.Grid)
            If Root IsNot Nothing Then
                Root.Background = If(IsLeft, QuanSideBrush, QuanCanvasBrush)
            ElseIf TypeOf Page Is FrameworkElement Then
                AddHandler CType(Page, FrameworkElement).Loaded, Sub() QuanLayerReloader(Page, IsLeft)
            End If
        Catch ex As Exception
            Logger.Warn(ex, "Quan 分层背景应用失败")
        End Try
    End Sub
    Private Sub QuanLayerReloader(Page As Object, IsLeft As Boolean)
        Try
            Dim Root = TryCast(Page, System.Windows.Controls.Grid)
            If Root IsNot Nothing Then Root.Background = If(IsLeft, QuanSideBrush, QuanCanvasBrush)
        Catch ex As Exception
            Logger.Warn(ex, "Quan 延迟分层失败")
        End Try
    End Sub
    Friend Sub ThemeCheckAll(EffectSetup As Boolean)
    End Sub
    Friend Function ThemeCheckOne(Id As Integer) As Boolean
        Return True
    End Function
    Friend Function ThemeUnlock(Id As Integer, Optional ShowDoubleHint As Boolean = True, Optional UnlockHint As String = Nothing) As Boolean
        Return False
    End Function

#End Region

#Region "更新"

    Friend Sub UpdateCheckByButton()
        Hint("该版本中不包含更新功能……")
    End Sub

    Friend IsUpdateWaitingRestart As Boolean = False
    Public Sub UpdateRestart(TriggerRestartAndByEnd As Boolean)
    End Sub
    Public Sub UpdateReplace(ProcessId As Integer, OldFileName As String, NewFileName As String, TriggerRestart As Boolean)
    End Sub

    ''' <summary>
    ''' 确保 PathTemp/Latest.exe 是最新正式版的 PCL，它会被用于整合包打包。
    ''' 如果不是，则下载一个。
    ''' </summary>
    Friend Sub DownloadLatestPCL(Optional LoaderToSyncProgress As LoaderBase = Nothing)
        '注意：如果要自行实现这个功能，请换用另一个文件路径，以免与官方版本冲突
    End Sub

#End Region

#Region "联网配置"

    ''' <summary>
    ''' 联网获取的配置信息。
    ''' 若获取失败或仍在获取中，可能为 Nothing。
    ''' </summary>
    Public ServerConfig As JObject

    Public ServerLoader As New LoaderTask(Of Integer, Integer)("PCL 配置更新", Sub() Logger.Info("该版本中不包含更新通知功能……"), Priority:=ThreadPriority.BelowNormal) With
        {.ReloadTimeout = 1000 * 60 * 60} '超时 1 小时

#End Region

#Region "赞助等级"

    Public ReadOnly Property CurrentRank As DonationRank
        Get
            Return DonationRank.None
        End Get
    End Property

    Public Sub InputPotatoCode(IsUpdating As Boolean)
    End Sub
    Friend Sub GeneratePotatoCode()
    End Sub

    ''' <summary>
    ''' 获取设备识别码。
    ''' </summary>
    Friend Function GetIdentify() As String
        Return "0000-0000-0000-0000"
    End Function

#End Region

End Module
