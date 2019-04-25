

Public Class Form1
    Dim MonitorName As String = ""
    Public flagR As Boolean
    Public Function GetProcNames() As String()
        Dim processes = System.Diagnostics.Process.GetProcesses()
        Dim prcNames(processes.Length) As String
        For i = 0 To processes.Length - 1
            Dim Thetext = processes(i).ToString()
            Thetext = Thetext.Substring(Thetext.IndexOf("(") + 1)
            Thetext = Mid(Thetext, 1, Thetext.Length - 1)
            prcNames(i) = Thetext.Trim
        Next i
        Return prcNames
    End Function
    Public Sub Listen()
        Dim prcNames As String() = GetProcNames()
        For Each thename In prcNames
            If thename = MonitorName Then
                flagR = True
            End If
        Next
        If flagR = False Then
            Sed()
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        MonitorName = InputBox("请输入录屏进程名, 不含"".exe""", MonitorName)
    End Sub

    Sub Readfile(ByRef FileName As String)
        Dim strData As String()
        Try
            strData = System.IO.File.ReadAllLines(FileName)
        Catch e As Exception
            MsgBox("读取文件失败")
            Exit Sub
        End Try

        ListBox1.Items.Clear()
        For Each s In strData
            If s = Nothing Then Continue For
            ListBox1.Items.Add(s)
        Next

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Listen()
    End Sub

    Function Start() As Boolean
        If MonitorName = "" Then
            MsgBox("获取录屏进程名失败")
            Return False
        Else
            Timer1.Enabled = True
            Button1.Text = "停止运行"
            Return True
        End If
    End Function


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Timer1.Enabled = True Then
            Timer1.Enabled = False
            Button1.Text = "开始运行"
        Else
            Start()
        End If
    End Sub

    Public Sub Sed()
        Dim prcNames As String() = GetProcNames()

        For Each thename In prcNames
            If thename = Nothing Then Continue For
            If ListBox1.Items.IndexOf(thename) <> -1 Then
                Timer1.Enabled = False
                MsgBox("请运行录屏!", MsgBoxStyle.OkOnly + MsgBoxStyle.SystemModal, "注意")
                Timer1.Enabled = True
                Exit For
            End If
        Next
    End Sub

    Private Sub BtnDebug_Click(sender As Object, e As EventArgs)
        Listen()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim Name = InputBox("请输入想要监控的进程名, 不含"".exe""", "")
        Name = Name.Trim
        If (Name = "") Then Return
        If ListBox1.FindString(Name) <> -1 Then Return
        ListBox1.Items.Add(Name)
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ListBox1.Items.Add("POWERPNT")
        ListBox1.Items.Add("MicrosoftEdge")
        ListBox1.Items.Add("devenv")
        ListBox1.Items.Add("gcc")
        ListBox1.Items.Add("javac")
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If (ListBox1.SelectedItem <> -1) Then ListBox1.Items.Remove(ListBox1.SelectedItem)
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim Filename = InputBox("请输入监控列表").Trim()
        Readfile(Filename)
    End Sub



    Private Sub Form1_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize

        If Me.WindowState = FormWindowState.Minimized Then
            Me.Hide()
        End If

        '这里实现窗口不允许更改大小（就是在改变窗口大小的时候锁定某一数值，实现无法改变大小的效果。另外要把最大化按钮设置为false）
        Me.Width = 376
        Me.Height = 240

    End Sub
    Private Sub NotifyIcon1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles NotifyIcon1.DoubleClick

        Me.ShowInTaskbar = True
        Me.Show()

        Me.WindowState = FormWindowState.Normal
    End Sub

End Class
