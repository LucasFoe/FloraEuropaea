Imports System.IO
Imports LfUtilities

Public Class UserCmd

    Private key As String
    Private label As String
    Private cmdType As String
    Private cmd As String
    Private par As String ' xxPx is replaced by content Variable par
    Private wrkDir As String = ""
    Private clPar As String = ""

    Public Property ParProperty() As String
        Get
            Return par
        End Get
        Set(ByVal value As String)
            par = value
        End Set
    End Property

    Public ReadOnly Property KeyProperty() As String
        Get
            Return key
        End Get
    End Property

    Public ReadOnly Property LabelProperty() As String
        Get
            Return label
        End Get
    End Property

    Public ReadOnly Property CmdTypeProperty() As String
        Get
            Return cmdType
        End Get
    End Property

    Public ReadOnly Property CmdProperty() As String
        Get
            Return cmd
        End Get
    End Property

    Public Sub New(ByVal key As String, ByVal label As String, ByVal cmdType As String, ByVal cmd As String)
        Me.key = key
        Me.label = label
        Me.cmdType = cmdType
        Me.cmd = cmd
    End Sub

    Public Sub New(ByVal cmdLine As String, ByVal cmdType As String)
        Me.cmdType = cmdType

        ParseCmdLine(cmdLine)
        If Me.cmdType = "STARTPRG" And Util.IsEmpty(Me.wrkDir) Then
            Me.wrkDir = Path.GetDirectoryName(cmd)
        End If

    End Sub

    Private Sub ParseCmdLine(ByVal cmdLine As String)
        Dim m() As String
        ' #* CMD = xxx.exe, LABEL(Text), CLPAR(Command Line Parameters), WORKDIR(Arbeitsverzeichnis 
        ' ATLASFE = C:\floracd\atlasfloraeuropaea\AFE99.EXE, LABEL(Altas Flora Europaea)
        cmdLine = Trim(cmdLine)
        Dim pos = cmdLine.IndexOf(","), wcmdline As String = cmdLine
        If pos > 0 Then
            wcmdline = Util.Left(cmdLine, pos)
        End If
        m = RegExUtil.Matches(wcmdline, "^([\w-]+)\s*\=\s*(.*)$", True, False)
        If m IsNot Nothing Then
            If m.Length > 1 Then
                Me.key = m(1)
            End If
            If m.Length > 2 Then
                Me.cmd = m(2)
                Me.label = Me.cmd
            End If
        End If
        m = RegExUtil.Matches(cmdLine, "^.*LABEL\((.*)\).*$", True, False)
        If m IsNot Nothing Then
            If m.Length > 1 Then
                Me.label = m(1)
            End If
        End If
        m = RegExUtil.Matches(cmdLine, "^.*CLPAR\((.*)\).*$", True, False)
        If m IsNot Nothing Then
            If m.Length > 1 Then
                Me.clPar = m(1)
            End If
        End If
        m = RegExUtil.Matches(cmdLine, "^.*WORKDIR\((.*)\).*$", True, False)
        If m IsNot Nothing Then
            If m.Length > 1 Then
                Me.wrkDir = m(1)
            End If
        End If
    End Sub

    Public Sub ExecUserCmd()
        'https://www.google.com/search?q=" & queryBox.Text
        Dim wcmd As String = cmd
        wcmd = wcmd.Replace("xxPx", Me.par)
        If Me.cmdType = "LINK" Then
            Dim webAddress As String = wcmd
            OpenUrl(webAddress)
        ElseIf Me.cmdType = "STARTPRG" Then
            StartCmd(wcmd, Me.clPar, Me.wrkDir)
        End If
    End Sub

    Public Overrides Function ToString() As String
        Return Me.key & ": " & Me.label
    End Function

End Class

Public Class UserCmdList

    Private ucList As List(Of UserCmd) = New List(Of UserCmd)
    Private inifn As String

    Public ReadOnly Property UserCmdListProperty() As List(Of UserCmd)
        Get
            Return ucList
        End Get
    End Property

    Public ReadOnly Property IniFileProperty() As String
        Get
            Return inifn
        End Get
    End Property

    Public Sub New(ByVal inifn As String)
        Me.inifn = inifn
        ReadIni()
    End Sub

    Private Sub ReadIni()

        Dim st As StreamReader
        Try
            st = New StreamReader(Me.inifn)
        Catch ex As Exception
            Exit Sub
        End Try

        Dim line As String, cmdtype As String = ""

        While (st.Peek() >= 0)

            line = st.ReadLine().Trim

            If Not Util.IsEmpty(line) Then
                If line.StartsWith("#*") Then
                    ' Kommentar
                ElseIf line.StartsWith("#") Then
                    cmdtype = line.Substring(1)
                Else
                    Dim uc As New UserCmd(line, cmdtype)
                    ucList.Add(uc)
                End If
            End If

        End While

    End Sub

End Class


