Public Class PclLogger
    Inherits FileLogger

    ''' <inheritdoc/>
    Public Overrides Function Format(Text As String, Level As LogLevel, FilePath As String, Ex As Exception) As String
        Text = MyBase.Format(Text, Level, FilePath, Ex)
        Text = FilterUserName(FilterAccessToken(Text, "*"c), "*"c)
        Return Text
    End Function

    ''' <inheritdoc/>
    Public Overrides Sub HandleBehavior(RawMessage As String, FormattedMessage As String, Behavior As LogBehavior, Ex As Exception)
        If IsProgramEnding Then Return
        MyBase.HandleBehavior(RawMessage, FormattedMessage, Behavior, Ex)
        Dim BriefText = If(Ex Is Nothing, RawMessage, If(RawMessage Is Nothing, "", $"{RawMessage}：") & Ex.GetDisplay(False))
        BriefText = FilterUserName(FilterAccessToken(BriefText, "*"c), "*"c)
        Dim DetailText = If(Ex Is Nothing, RawMessage, If(RawMessage Is Nothing, "", $"{RawMessage}：") & Ex.GetDisplay(True))
        DetailText = FilterUserName(FilterAccessToken(DetailText, "*"c), "*"c)
        Select Case Behavior
            Case LogBehavior.None
                '啥也不干
            Case LogBehavior.ToastIfDebug
                If BuildType = BuildTypes.Debug OrElse ModeDebug Then Hint("[调试模式] " & BriefText, HintType.Blue, False)
            Case LogBehavior.Toast
                Hint(BriefText, HintType.Red, False)
            Case LogBehavior.Alert
                MyMsgBox(DetailText, "错误", IsWarn:=True)
            Case LogBehavior.AlertThenFeedback
                MyMsgBox(DetailText, "错误", IsWarn:=True)
            Case LogBehavior.AlertThenCrash
                Static FirstTrigger As Boolean = True
                If FirstTrigger Then
                    '首次触发
                    FirstTrigger = False
                    MsgBox(DetailText, MsgBoxStyle.Critical, "错误")
                Else
                    '多次触发，直接使程序崩溃（这通常代表着在其他线程循环触发严重异常）
                    Thread.Sleep(2000)
                End If
                FormMain.EndProgramForce(ProcessReturnValues.Exception)
        End Select
    End Sub

End Class
