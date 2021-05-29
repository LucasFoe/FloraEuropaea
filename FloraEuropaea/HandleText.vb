Imports LfUtilities

Public Class HandleText

    Dim txtDir As String
    Dim vol1 As List(Of String)
    Dim vol2 As List(Of String)
    Dim vol3 As List(Of String)
    Dim vol4 As List(Of String)
    Dim vol5 As List(Of String)

    Dim curVolume As String
    Dim curVol As Long
    Dim curIdx As Long
    Dim curTxt As String
    Dim curFile As String
    Dim curPage As Long
    Dim curFilePath As String
    Dim firstFile As String
    Dim lastFile As String
    Dim firstIdx As Long
    Dim lastIdx As Long

    Public ReadOnly Property Text() As String
        Get
            Text = curTxt
        End Get
    End Property

    Public ReadOnly Property TextDir() As String
        Get
            TextDir = txtDir
        End Get
    End Property

    Public Property CurrentVolume() As String
        Get
            CurrentVolume = curVolume
        End Get
        Set(ByVal value As String)
            Select Case value
                Case "Volume 1", "1"
                    DoVolX("A1")
                    curVol = 1
                    setFirst(vol1)
                    setLast(vol1)
                    curVolume = "Volume 1"
                Case "Volume 2", "2"
                    DoVolX("A2")
                    curVol = 2
                    setFirst(vol2)
                    setLast(vol2)
                    curVolume = "Volume 2"
                Case "Volume 3", "3"
                    DoVolX("A3")
                    curVol = 3
                    setFirst(vol3)
                    setLast(vol3)
                    curVolume = "Volume 3"
                Case "Volume 4", "4"
                    DoVolX("A4")
                    curVol = 4
                    setFirst(vol4)
                    setLast(vol4)
                    curVolume = "Volume 4"
                Case "Volume 5", "5"
                    DoVolX("A5")
                    curVol = 5
                    setFirst(vol5)
                    setLast(vol5)
                    curVolume = "Volume 5"
            End Select
        End Set
    End Property

    Public Property CurrentFile() As String
        Get
            CurrentFile = curFile
        End Get
        Set(ByVal value As String)
            curFile = value
            CurrentFilePath = lf_filename(Me.TextDir, curFile)
        End Set
    End Property

    Public Property CurrentPage() As Long
        Get
            CurrentPage = curPage
        End Get
        Set(ByVal value As Long)
            curPage = value
            CurrentIndex = curPage
        End Set
    End Property

    Public Property CurrentFilePath() As String
        Get
            CurrentFilePath = curFilePath
        End Get
        Set(ByVal value As String)
            curFilePath = value
        End Set
    End Property

    Public Property CurrentIndex() As Long
        Get
            CurrentIndex = curIdx
        End Get
        Set(ByVal value As Long)
            curIdx = value
            curPage = curIdx
            setfile()
        End Set
    End Property

    Public ReadOnly Property Volume1() As List(Of String)
        Get
            Volume1 = vol1
        End Get
    End Property

    Public ReadOnly Property Volume2() As List(Of String)
        Get
            Volume2 = vol2
        End Get
    End Property

    Public ReadOnly Property Volume3() As List(Of String)
        Get
            Volume3 = vol3
        End Get
    End Property

    Public ReadOnly Property Volume4() As List(Of String)
        Get
            Volume4 = vol4
        End Get
    End Property

    Public ReadOnly Property Volume5() As List(Of String)
        Get
            Volume5 = vol5
        End Get
    End Property

    Sub New(ByVal tdir As String)
        Me.txtDir = tdir
        DoVolX("A1")
        DoVolX("A2")
        DoVolX("A3")
        DoVolX("A4")
        DoVolX("A5")
        CurrentVolume = "Volume 1"
    End Sub

    Function findFirst(ByRef volx As List(Of String)) As Long
        Dim i As Long
        For i = 0 To 100
            If Not Util.IsEmpty(volx(i)) Then
                Return i
            End If
        Next
        Return 0
    End Function

    Sub setFirst(ByRef volx As List(Of String))
        Dim i As Long
        i = findFirst(volx)
        Me.firstFile = volx(i)
        CurrentFile = volx(i)
        CurrentIndex = i
        Me.firstIdx = Me.curIdx
    End Sub

    Sub setLast(ByRef volx As List(Of String))
        Me.lastIdx = volx.Count - 1
        Me.lastFile = volx(Me.lastIdx)
    End Sub

    Public Function NextIndex() As Long
        If CurrentIndex < Me.lastIdx Then
            CurrentIndex = CurrentIndex + 1
        End If
        Return CurrentIndex
    End Function

    Public Function PrevIndex() As Long
        If CurrentIndex > Me.firstIdx Then
            CurrentIndex = CurrentIndex - 1
        End If
        Return CurrentIndex
    End Function

    Public Function FirstIndex() As Long
        CurrentIndex = Me.firstIdx
        Return CurrentIndex
    End Function

    Public Function LastIndex() As Long
        CurrentIndex = Me.lastIdx
        Return CurrentIndex
    End Function

    Public Sub DoVolX(ByRef X As String)
        Dim i As Long, filename As String, cnt As Object
        Dim wi As Long
        Dim vol As New List(Of String)
        cnt = Nothing
        Select Case X
            Case "A1"
                cnt = Me.vol1
            Case "A2"
                cnt = Me.vol2
            Case "A3"
                cnt = Me.vol3
            Case "A4"
                cnt = Me.vol4
            Case "A5"
                cnt = Me.vol5
        End Select

        If cnt Is Nothing Then

            Dim maxcento As Long
            For i = 1 To 800 Step 100
                filename = X & i & ".RTF"
                If lf_fileExists2(txtDir, filename) = 1 Then
                    maxcento = i
                End If
            Next

            For i = 0 To maxcento + 100
                filename = X & i & ".RTF"
                If i < 20 Then
                    Select Case lf_fileExists2(txtDir, filename)
                        Case 1
                            vol.Add(filename)
                            wi = i
                        Case Else
                            vol.Add("")
                    End Select
                ElseIf i > maxcento Then
                    Select Case lf_fileExists2(txtDir, filename)
                        Case 1
                            vol.Add(filename)
                            wi = i
                        Case Else
                            Exit For
                    End Select
                Else
                    vol.Add(filename)
                    wi = i
                End If
            Next i

            Select Case X
                Case "A1"
                    vol1 = vol
                Case "A2"
                    vol2 = vol
                Case "A3"
                    vol3 = vol
                Case "A4"
                    vol4 = vol
                Case "A5"
                    vol5 = vol
            End Select
        End If

    End Sub

    Private Sub checkPageIdx(ByRef v As List(Of String))

        Dim i As Integer, s As String

        If curIdx >= v.Count Then
            curIdx = v.Count - 1
        ElseIf curIdx < 10 Then

            For i = curIdx To 10
                s = Trim(v(i))
                If s.Length > 0 Then
                    curIdx = i
                    Exit For
                End If
            Next

        End If
    End Sub

    Sub setfile()

        If curIdx < 1 Then
            curIdx = 1
        End If
        Select Case curVol
            Case 1
                checkPageIdx(vol1)
                CurrentFile = Me.vol1(curIdx)
            Case 2
                checkPageIdx(vol2)
                CurrentFile = Me.vol2(curIdx)
            Case 3
                checkPageIdx(vol3)
                CurrentFile = Me.vol3(curIdx)
            Case 4
                checkPageIdx(vol4)
                CurrentFile = Me.vol4(curIdx)
            Case 5
                checkPageIdx(vol5)
                CurrentFile = Me.vol5(curIdx)
        End Select

        Me.curPage = curIdx

    End Sub

End Class
