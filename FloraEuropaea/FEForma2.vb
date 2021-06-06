Imports LfUtilities

Public Class FEForma2

    Private mcurrentKey As Key
    Private mcurrentKeyElement As KeyElement
    Private mSwitchCellStyle As DataGridViewCellStyle
    Private mPrevNextCellStyle As DataGridViewCellStyle
    Private mLinkCellStyle As DataGridViewCellStyle
    Private mSearchCellStyle As DataGridViewCellStyle
    Private mDefaultCellStyle As DataGridViewCellStyle

    Private mhistory As New List(Of Key)

    Private Sub addToHistory(ByVal k As Key)
        Dim n As Long, i As Long
        mhistory.Add(k)
        cbHistory.Items.Add(k.KeyName)
        n = mhistory.Count - 8
        If n > 3 Then
            For i = 0 To n
                Try
                    cbHistory.Items.RemoveAt(0)
                    mhistory.RemoveAt(0)
                Catch ex As Exception
                End Try
            Next
        End If
    End Sub

    Public Property CurrentKey() As Key
        Get
            Return mcurrentKey
        End Get
        Set(ByVal value As Key)
            If mcurrentKey Is value Then
            Else
                mcurrentKey = value
                addToHistory(mcurrentKey)
                InitializeDataGridView()
            End If
        End Set
    End Property

    Public Property ParentKey() As Key
        Get
            Return CurrentKey.ParentKey
        End Get
        Set(ByVal value As Key)
            CurrentKey.ParentKey = value
            If CurrentKey.ParentKey Is Nothing Then
                Me.btParentKey.Enabled = False
            Else
                Me.btParentKey.Enabled = True
            End If
        End Set
    End Property

    Public Sub setCellStyle(ByVal cidx As Integer, ByVal ridx As Long, ByVal cs As DataGridViewCellStyle)
        Try
            If ridx >= 0 Then
                DataGridView1.Rows(ridx).Cells(cidx).Style = cs
            End If
        Catch ex As Exception
        End Try
    End Sub

    Public Sub SetKeyFormat(ByVal idx As Long)
        ' Current Row
        setCellStyle(1, idx, mSwitchCellStyle)
        setCellStyle(1, mcurrentKeyElement.KeySwitchIndex, mSwitchCellStyle)
        setCellStyle(1, mcurrentKeyElement.KeyPrevIndex, mPrevNextCellStyle)
        setCellStyle(1, mcurrentKeyElement.KeyNextIndex, mPrevNextCellStyle)
    End Sub

    Public Sub ResetKeyFormat()
        If mcurrentKeyElement IsNot Nothing Then
            setCellStyle(1, mcurrentKeyElement.KeyIndex, mDefaultCellStyle)
            setCellStyle(1, mcurrentKeyElement.KeySwitchIndex, mDefaultCellStyle)
            setCellStyle(1, mcurrentKeyElement.KeyPrevIndex, mDefaultCellStyle)
            setCellStyle(1, mcurrentKeyElement.KeyNextIndex, mDefaultCellStyle)
        End If
    End Sub

    Public Sub FormatKey(ByVal idx)
        ResetKeyFormat()
        mcurrentKey.IdxCurrentKeyElement = idx
        mcurrentKeyElement = mcurrentKey.GetKeyElement()

        SetKeyFormat(idx)
        If Util.IsEmpty(mcurrentKeyElement.KeyLink) Then
            btLink.Enabled = False
            btLink.Text = "Link"
            btNext.Enabled = True
        Else
            btNext.Enabled = False
            If Me.mcurrentKeyElement.KeyLinkRef Is Nothing Then
                btLink.Enabled = True
                btLink.Text = "Lookup"
            Else
                btLink.Enabled = True
                btLink.Text = "-> Key"
            End If
        End If
    End Sub

    Public Sub SetCurrentRow(ByVal idx As Long)
        If idx >= 0 Then
            CurrentKey.IdxUsedKeyElement = idx
            DataGridView1.CurrentCell = DataGridView1.Rows(idx).Cells(0)
            FormatKey(idx)

            If idx > 5 Then
                DataGridView1.FirstDisplayedScrollingRowIndex = idx - 5
            Else
                DataGridView1.FirstDisplayedScrollingRowIndex = 0
            End If

        End If
    End Sub

    Private Sub DataGridView1_RowEnter(ByVal sender As Object,
        ByVal e As DataGridViewCellEventArgs) _
        Handles DataGridView1.RowEnter
        FormatKey(e.RowIndex)
    End Sub

    Private Sub InitializeDataGridView()
        If CurrentKey IsNot Nothing Then
            Me.Cursor = Cursors.WaitCursor
            CurrentKey.FillDGV(DataGridView1)
            Me.Text = "Flora Europaea Key Browser: " & CurrentKey.KeyName.ToUpper
            SetCurrentRow(CurrentKey.IdxUsedKeyElement)
            Me.Cursor = Cursors.Default
        End If
        If Me.ParentKey Is Nothing Then
            btParentKey.Enabled = False
        Else
            btParentKey.Enabled = True
        End If
    End Sub

    Private Sub btPrev_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btPrev.Click
        Dim idx As Long
        idx = mcurrentKeyElement.KeyPrevIndex
        SetCurrentRow(idx)
    End Sub

    Private Sub btSwitch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btSwitch.Click
        Dim idx As Long
        idx = mcurrentKeyElement.KeySwitchIndex
        SetCurrentRow(idx)
    End Sub

    Private Sub btNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btNext.Click
        Dim idx As Long
        idx = mcurrentKeyElement.KeyNextIndex
        If idx < 0 Then
            DataGridView1.CurrentCell = DataGridView1.Rows(mcurrentKeyElement.KeyIndex).Cells(2)
        Else
            SetCurrentRow(idx)
        End If

    End Sub

    Private Sub LinkToSubKey()
        mcurrentKeyElement.KeyLinkRef.ParentKey = CurrentKey
        CurrentKey = mcurrentKeyElement.KeyLinkRef
    End Sub

    Private Sub LinkToText()
        Dim t As String, s As String, x() As String
        t = mcurrentKeyElement.KeyLink
        If Util.IsEmpty(t) Then
            ' do nothing
            Exit Sub
        End If
        If mcurrentKeyElement.KeyLevel = "GEN" Then
            FEForma1.txtFilter.Text = mcurrentKey.KeyName
            FEForma1.SetFlt()
            FEForma1.lbIndex.SelectedIndex = 0
            FEForma1.LbIndexAction()
            x = Util.Matches(t, "^[().\d-]+\s([\w\s-]+)$")
            If x IsNot Nothing Then
                If x.Length > 1 Then
                    s = x(x.Length - 1)
                    FEForma1.FindlbSpec(s)
                End If
            End If
            FEForma1.LbSpecAction()
            FEForma1.Activate()
        Else
            x = Util.Matches(t, "^[().\d-]+\s([\w\s-]+)$")
            If x IsNot Nothing Then
                If x.Length > 1 Then
                    s = x(x.Length - 1)
                    FEForma1.txtFilter.Text = s
                    FEForma1.SetFlt()
                    FEForma1.lbIndex.SelectedIndex = 0
                    FEForma1.LbIndexAction()
                    FEForma1.Activate()
                End If
            End If
        End If
    End Sub

    Private Sub LinkTo()
        If mcurrentKeyElement.KeyLinkRef Is Nothing Then
            LinkToText()
        Else
            LinkToSubKey()
        End If
    End Sub

    Private Sub btLink_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btLink.Click
        LinkTo()
    End Sub

    Private Sub btClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btClose.Click
        Me.Hide()
        Me.ParentKey = Nothing
        Me.mhistory.Clear()
        Me.cbHistory.Items.Clear()
        Me.cbHistory.Text = "History"
        FEForma1.Activate()
    End Sub

    Private Sub DataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        Dim ridx As Long, cidx As Long
        ridx = e.RowIndex
        cidx = e.ColumnIndex
        SetCurrentRow(ridx)
        If cidx = 2 Then
            LinkTo()
        End If
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.DataGridView1.DoubleBuffered(True)

        ' Add any initialization after the InitializeComponent() call.
        mSwitchCellStyle = New DataGridViewCellStyle(DataGridView1.DefaultCellStyle)
        mPrevNextCellStyle = New DataGridViewCellStyle(DataGridView1.DefaultCellStyle)
        mLinkCellStyle = New DataGridViewCellStyle(DataGridView1.DefaultCellStyle)
        mSearchCellStyle = New DataGridViewCellStyle(DataGridView1.DefaultCellStyle)
        mDefaultCellStyle = New DataGridViewCellStyle(DataGridView1.DefaultCellStyle)

        With mSwitchCellStyle
            .BackColor = Color.Khaki
        End With

        With mPrevNextCellStyle
            .BackColor = Color.LightGreen
        End With

    End Sub

    Private Sub btParentKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btParentKey.Click
        If Me.CurrentKey.ParentKey IsNot Nothing Then
            CurrentKey = Me.CurrentKey.ParentKey
        End If
    End Sub

    Private Sub cbHistory_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbHistory.SelectedIndexChanged
        Dim i As Long
        i = cbHistory.SelectedIndex()
        Me.CurrentKey = Me.mhistory(i)
    End Sub

End Class