Imports LfUtilities
Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms


Public Module WFExtensions
    <Extension()>
    Public Sub DoubleBuffered(ByVal dgv As DataGridView, ByVal setting As Boolean)
        Dim dgvType As Type = dgv.GetType()
        Dim pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance Or BindingFlags.NonPublic)
        pi.SetValue(dgv, setting, Nothing)
    End Sub

    <Extension()>
    Public Function GetRowAsObjectList(ByVal dgvr As DataGridViewRow) As List(Of Object)
        Dim r As List(Of Object) = Nothing
        r = dgvr.Cells.Cast(Of DataGridViewCell)().[Select](Function(c) c.Value).ToList()
        Return r
    End Function

    <Extension()>
    Public Function GetCell(ByVal dgvr As DataGridViewRow, ByVal colName As String) As DataGridViewCell
        Return dgvr.Cells(colName)
    End Function

    <Extension()>
    Public Function GetCell(ByVal dgvr As DataGridViewRow, ByVal colIdx As Integer) As DataGridViewCell
        Return dgvr.Cells(colIdx)
    End Function

    <Extension()>
    Public Function GetCellStringValue(ByVal dcl As DataGridViewCell) As String
        Dim r = ""

        If ReferenceEquals(dcl.Value, DBNull.Value) Then
            r = ""
        Else
            r = dcl.Value.ToString()
        End If

        Return r
    End Function

    <Extension()>
    Public Function IsEmpty(ByVal dcl As DataGridViewCell) As Boolean
        Dim r As Boolean

        If ReferenceEquals(dcl.Value, DBNull.Value) Then
            r = True
        Else
            r = Util.IsEmpty(dcl.Value.ToString())
        End If

        Return r
    End Function

    <Extension()>
    Public Function GetRowAsStringList(ByVal dgvr As DataGridViewRow) As List(Of String)
        Dim r As List(Of String) = New List(Of String)()

        For Each c As DataGridViewCell In dgvr.Cells
            r.Add(c.GetCellStringValue())
        Next

        Return r
    End Function

    <Extension()>
    Public Function GetColumnAsStringList(ByVal dgv As DataGridView, ByVal columnName As String) As List(Of String)
        Dim r As List(Of String) = New List(Of String)()

        For Each row As DataGridViewRow In dgv.Rows
            Dim c = row.GetCell(columnName)
            r.Add(c.GetCellStringValue())
        Next

        Return r
    End Function

    <Extension()>
    Public Function GetColumnIndex(ByVal dgv As DataGridView, ByVal columnName As String) As Integer
        Dim c = dgv.Columns(columnName)
        Return dgv.Columns.IndexOf(c)
    End Function

    <Extension()>
    Public Sub DisableGrid(ByVal dgv As DataGridView)
        dgv.Enabled = False
        dgv.DefaultCellStyle.BackColor = SystemColors.Control
        dgv.DefaultCellStyle.ForeColor = SystemColors.GrayText
        dgv.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Control
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.GrayText
        dgv.ReadOnly = True
        dgv.EnableHeadersVisualStyles = False
        dgv.CurrentCell = Nothing
    End Sub

    <Extension()>
    Public Sub EnableGrid(ByVal dgv As DataGridView)
        dgv.Enabled = True
        dgv.DefaultCellStyle.BackColor = SystemColors.Window
        dgv.DefaultCellStyle.ForeColor = SystemColors.ControlText
        dgv.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Window
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.ControlText
        dgv.ReadOnly = False
        dgv.EnableHeadersVisualStyles = False
    End Sub

    Public Sub WaitCursorOn(Optional fe As Form = Nothing)
        Application.UseWaitCursor = True
        If fe IsNot Nothing Then fe.Cursor = Cursors.WaitCursor
        Cursor.Position = Cursor.Position
    End Sub

    Public Sub WaitCursorOff(Optional fe As Form = Nothing)
        Application.UseWaitCursor = False
        If fe IsNot Nothing Then fe.Cursor = Cursors.Default
        Cursor.Position = Cursor.Position
    End Sub

    Public Sub MoveCursor2End(tb As TextBox)
        tb.SelectionStart = tb.Text.Length
        tb.SelectionLength = 0
    End Sub

    Public Function GetApplicationIcon(Optional exepath As String = Nothing) As Icon
        If LfUtilities.Util.IsEmpty(exepath) Then
            exepath = Assembly.GetEntryAssembly().Location
        End If
        Return Icon.ExtractAssociatedIcon(exepath)
    End Function

End Module

