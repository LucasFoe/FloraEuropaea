Imports System.Data.OleDb
Imports LfUtilities

Public Class KeyElement

    Private mKeyLevel As String
    Private mKeyName As String
    Private mKeyIndex As Long
    Private mUKeyName As String
    Private mCode As Long
    Private mKeyDesc As String
    Private mKeyLink As String
    Private mKeyLinkRef As Key

    Private mKeySwitchIndex As Long
    Private mKeyPrevIndex As Long
    Private mKeyNextIndex As Long

    Public Property KeySwitchIndex() As Long
        Get
            Return mKeySwitchIndex
        End Get
        Set(ByVal value As Long)
            mKeySwitchIndex = value
        End Set
    End Property

    Public Property KeyPrevIndex() As Long
        Get
            Return mKeyPrevIndex
        End Get
        Set(ByVal value As Long)
            mKeyPrevIndex = value
        End Set
    End Property

    Public Property KeyNextIndex() As Long
        Get
            Return mKeyNextIndex
        End Get
        Set(ByVal value As Long)
            mKeyNextIndex = value
        End Set
    End Property

    Public Property KeyName() As String
        Get
            Return mKeyName
        End Get
        Set(ByVal value As String)
            mKeyName = value
            UKeyName = value
        End Set
    End Property

    ' GEN/FAM/ABOVE
    Public Property KeyLevel() As String
        Get
            Return mKeyLevel
        End Get
        Set(ByVal value As String)
            mKeyLevel = value
        End Set
    End Property

    Public Property UKeyName() As String
        Get
            Return mUKeyName
        End Get
        Set(ByVal value As String)
            mUKeyName = value.ToUpper
        End Set
    End Property

    Public Property Code() As Long
        Get
            Return mCode
        End Get
        Set(ByVal value As Long)
            mCode = value
        End Set
    End Property

    Public Property KeyDesc() As String
        Get
            Return mKeyDesc
        End Get
        Set(ByVal value As String)
            mKeyDesc = value.Trim
        End Set
    End Property

    Public Property KeyLink() As String
        Get
            Return mKeyLink
        End Get
        Set(ByVal value As String)
            mKeyLink = value
        End Set
    End Property

    Public Property KeyLinkRef() As Key
        Get
            Return mKeyLinkRef
        End Get
        Set(ByVal value As Key)
            mKeyLinkRef = value
        End Set
    End Property

    Public Property KeyIndex() As Long
        Get
            Return mKeyIndex
        End Get
        Set(ByVal value As Long)
            mKeyIndex = value
        End Set
    End Property

    Public Sub New(ByVal keyname As String, ByVal code As Long, ByVal keydesc As String, ByVal keyindex As Long, ByVal keylevel As String)
        Dim x(1) As String, nel As Long, key As Key, linkkname As String
        Me.KeyName = keyname
        Me.KeyIndex = keyindex
        Me.Code = code
        Me.KeyDesc = keydesc
        KeyLink = ""
        Me.KeyLevel = keylevel
        KeyLinkRef = Nothing
        If keyname = "TAXA" Then
            If Me.KeyDesc.ToUpper.EndsWith("Pteridophyta".ToUpper) Then
                x(0) = "Pteridophyta"
            ElseIf Me.KeyDesc.ToUpper.EndsWith("Gymnospermae".ToUpper) Then
                x(0) = "Gymnospermae"
            ElseIf Me.KeyDesc.ToUpper.EndsWith("Angiospermae".ToUpper) Then
                x(0) = "Angiospermae"
            Else
                x = Nothing
            End If
            If x IsNot Nothing Then
                x(1) = x(0)
            End If
        Else
            x = Util.Matches(keydesc, "^.+\s+(\d{1,4}\.\s+(\w+))\s*$")
            If x Is Nothing Then
                x = Util.Matches(keydesc, "^.*\s+(\(\d{1,4}-\d{1,4}\)\.\s+([\w\s-]+))\s*$")
            End If
        End If

        If x IsNot Nothing Then
            nel = x.Length
            If nel > 1 Then
                linkkname = Trim(x(nel - 2))
                If nel > 2 Then
                    Me.KeyDesc = Replace(Me.KeyDesc, x(nel - 2), "").Trim
                End If
                key = kc.GetKey(linkkname)
                KeyLink = linkkname
                If key IsNot Nothing Then
                    KeyLinkRef = key
                End If
            End If
        End If

    End Sub

End Class

Public Class Key
    Implements IDisposable

    Private mParentKey As Key
    Private mKeyLevel As String
    Private mKeyName As String
    Private mUKeyName As String
    Private mKeyElmtList As New List(Of KeyElement)
    Private mNrOfKeyElmt As Long
    Private mIdxUsedKeyElement As Long
    Private mIdxCurrentKeyElement As Long

    Private conn As OleDbConnection
    Private sql As String
    Private dv As DataView
    Private dt As New DataTable("KEY")
    Private da As OleDbDataAdapter
    Private fillkeyflag = False


    Public Property IdxCurrentKeyElement() As Long
        Get
            Return mIdxCurrentKeyElement
        End Get
        Set(ByVal value As Long)
            mIdxCurrentKeyElement = value

        End Set
    End Property

    Public Property IdxUsedKeyElement() As Long
        Get
            Return mIdxUsedKeyElement
        End Get
        Set(ByVal value As Long)
            mIdxUsedKeyElement = value
        End Set
    End Property

    Public Property ParentKey() As Key
        Get
            Return mParentKey
        End Get
        Set(ByVal value As Key)
            mParentKey = value
        End Set
    End Property

    Public Property KeyName() As String
        Get
            Return mKeyName
        End Get
        Set(ByVal value As String)
            mKeyName = value
            UKeyName = value
        End Set
    End Property

    ' GEN/FAM/ABOVE
    Public Property KeyLevel() As String
        Get
            Return mKeyLevel
        End Get
        Set(ByVal value As String)
            mKeyLevel = value
        End Set
    End Property

    Public Property UKeyName() As String
        Get
            Return mUKeyName
        End Get
        Set(ByVal value As String)
            mUKeyName = value.ToUpper
        End Set
    End Property

    Public Property KeyElmtList() As List(Of KeyElement)
        Get
            Return mKeyElmtList
        End Get
        Set(ByVal value As List(Of KeyElement))
            mKeyElmtList = value
        End Set
    End Property

    Public Property NrOfKeyElmt() As Long
        Get
            Return mNrOfKeyElmt
        End Get
        Set(ByVal value As Long)
            mNrOfKeyElmt = value
        End Set
    End Property

    Public Sub New(ByVal keyname As String, ByVal sql As String, ByVal keylevel As String)
        Me.KeyName = keyname
        Me.conn = Nothing
        Me.sql = sql
        Me.KeyLevel = keylevel
        Me.IdxUsedKeyElement = 0
    End Sub

    Public Sub New(ByVal keyname As String, ByRef conn As OleDbConnection, ByVal sql As String, ByVal keylevel As String)
        Me.KeyName = keyname
        Me.conn = conn
        Me.sql = sql
        Me.KeyLevel = keylevel
        Me.IdxUsedKeyElement = 0
    End Sub

    Public Sub FillKey()
        If Not fillkeyflag Then
            Dim i As Long, rowView As DataRowView
            Dim code As String, keyDesc As String
            da = New OleDbDataAdapter(sql, conn)
            da.Fill(dt)
            dv = dt.DefaultView
            For i = 0 To Me.dv.Count - 1
                rowView = dv(i)
                code = rowView.Item("CODE")
                keyDesc = Util.RemoveDblspaces(rowView.Item("KEY"))
                Dim ke As New KeyElement(Me.KeyName, code, keyDesc, i, Me.KeyLevel)
                mKeyElmtList.Add(ke)
            Next
            dt = Nothing
            da = Nothing
            mNrOfKeyElmt = mKeyElmtList.Count
            fillkeyflag = True
        End If
        completeKey()
    End Sub

    Private Sub completeKey()
        Dim k As KeyElement, cidx As Long
        For Each k In mKeyElmtList
            cidx = k.KeyIndex
            k.KeyNextIndex = getNextIdx(cidx)
            k.KeyPrevIndex = getPrevIdx(cidx)
            k.KeySwitchIndex = getSwitchIdx(cidx)
        Next
    End Sub

    Private Function getNextIdx(ByVal cidx As Long)
        Dim nidx As Long, i As Long, ke As KeyElement, ccode As Long
        ke = mKeyElmtList(cidx)
        nidx = -1
        ccode = ke.Code
        For i = cidx To Me.NrOfKeyElmt - 1
            ke = mKeyElmtList(i)
            If ke.Code > ccode Then
                nidx = i
                Exit For
            ElseIf ke.Code < ccode Then
                nidx = -1
                Exit For
            End If
        Next
        Return nidx
    End Function

    Private Function getPrevIdx(ByVal cidx As Long)
        Dim pidx As Long, i As Long, ke As KeyElement, ccode As Long
        pidx = -1
        If cidx > 0 Then
            ke = mKeyElmtList(cidx)
            ccode = ke.Code
            For i = cidx To 0 Step -1
                ke = mKeyElmtList(i)
                If ke.Code < ccode Then
                    pidx = i
                    Exit For
                End If
            Next
        End If
        Return pidx
    End Function

    Private Function getSwitchIdx(ByVal cidx As Long)
        Dim sidx As Long, i As Long, ke As KeyElement, ccode As Long
        sidx = -1
        ke = mKeyElmtList(cidx)
        ccode = ke.Code
        For i = 0 To Me.NrOfKeyElmt - 1
            If i <> cidx Then
                ke = mKeyElmtList(i)
                If ke.Code = ccode Then
                    sidx = i
                    Exit For
                End If
            End If
        Next
        Return sidx
    End Function

    Public Function GetFirstRow() As String()
        mIdxCurrentKeyElement = 0
        Return GetCurrentRow()
    End Function

    Public Function GetCurrentRow() As String()
        Dim r(4) As String, kel As KeyElement
        kel = mKeyElmtList(mIdxCurrentKeyElement)
        r(0) = kel.Code
        r(1) = kel.KeyDesc
        r(2) = kel.KeyLink
        r(3) = mIdxCurrentKeyElement
        If kel.KeyLinkRef Is Nothing Then
            r(4) = "S"
        ElseIf kel.KeyLink.Trim.Length > 0 Then
            r(4) = "T"
        Else
            r(4) = ""
        End If
        Return r
    End Function

    Public Function GetNextRow() As String()
        mIdxCurrentKeyElement = mIdxCurrentKeyElement + 1
        Return GetCurrentRow()
    End Function

    Public Sub FillDGV(ByRef dgv As DataGridView)

        ' Set the column header names.
        FillKey()
        Dim row() As String
        dgv.Rows.Clear()
        row = GetFirstRow()

        dgv.Rows.Add(row)
        Do While mIdxCurrentKeyElement < mNrOfKeyElmt - 1
            row = GetNextRow()
            dgv.Rows.Add(row)
        Loop

    End Sub

    Public Function GetKeyElement(ByVal idx As Long) As KeyElement
        Return Me.mKeyElmtList(idx)
    End Function
    Public Function GetKeyElement() As KeyElement
        Return Me.mKeyElmtList(Me.IdxCurrentKeyElement)
    End Function

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
                DisposeIfNotNull(conn)
                DisposeIfNotNull(dv)
                DisposeIfNotNull(dt)
                DisposeIfNotNull(da)
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
            mKeyElmtList.Clear()
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class

Public Class KeyCollection

    Private keycollection As New Dictionary(Of String, Key)
    Private mcurrentKey As Key

    Public Property CurrentKey() As Key
        Get
            Return mcurrentKey
        End Get
        Set(ByVal value As Key)
            mcurrentKey = value
        End Set
    End Property

    Public Sub AddKeySet(ByRef conn As OleDbConnection, ByRef dv As DataView, ByVal level As String)
        Dim k As String, tplsql As String, sql As String, i As Long
        Dim rowView As DataRowView
        tplsql = "SELECT CODE, KEY FROM [$$KEYNAME$]"

        For i = 0 To dv.Count - 1
            rowView = dv(i)
            k = rowView.Item("TABLE_NAME")
            sql = Replace(tplsql, "$$KEYNAME$", k)
            Dim key As New Key(k, conn, sql, level)
            AddKey(key)
        Next

    End Sub

    Public Sub AddKey(ByRef k As Key)
        Dim key = k.UKeyName.Trim
        If keycollection.ContainsKey(key) Then
            keycollection.Remove(key)
        End If
        keycollection.Add(key, k)
    End Sub

    Public Function GetKey(ByVal k As String) As Key
        Dim x() As String, wk As String = "", wk2 As String = "", wk3 As String = "", ky As Key = Nothing

        If Util.IsEmpty(k) Then
            Return Nothing
        End If

        Try
            wk = k.Trim.ToUpper
            x = Util.Matches(wk, "\d{1,4}\.\s+(\w+)")
            If x IsNot Nothing Then
                wk = x(x.Length - 1)
            End If
            If keycollection.ContainsKey(wk) Then
                ky = keycollection(wk)
            Else
                ky = Nothing
            End If
            If ky Is Nothing Then
                wk2 = sett.GetMetaSyn(wk)
                If Not Util.IsEmpty(wk2) And keycollection.ContainsKey(wk2) Then
                    Try
                        ky = keycollection(wk2)
                    Catch ex As Exception
                        ky = Nothing
                    End Try
                End If
            End If
            If ky Is Nothing Then
                wk3 = sett.GetLinkTaxa(wk)
                If Util.IsEmpty(wk3) Then
                    wk3 = sett.GetLinkTaxa(wk2)
                End If
                If Not Util.IsEmpty(wk3) And keycollection.ContainsKey(wk3) Then
                    Try
                        ky = keycollection.Item(wk3)
                    Catch ex As Exception
                        ky = Nothing
                    End Try
                End If
            End If
        Catch ex As Exception
            k = Nothing
        End Try
        Return ky

    End Function

End Class
