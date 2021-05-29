Imports System.io
Public Class FEForma3

    Private Sub FEForma3_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtFilter.Text = gl.Filter
        gl.Fillcontrol(lbGlossary)
        If lbGlossary.Items.Count > 0 Then
            lbGlossary.SelectedIndex = 0
        Else
            lbGlossary.SelectedIndex = -1
        End If
    End Sub

    Private Sub ckFilter_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ckFilter.CheckedChanged
        If txtFilter.Text.ToString.Length > 0 Then
            If ckFilter.Checked Then
                gl.setFilter(txtFilter.Text.ToString.Trim)
                gl.setfilterOn()
                gl.Fillcontrol(lbGlossary)
                lbGlossary.SelectedIndex = -1
            Else
                gl.setfilterOff()
                gl.Fillcontrol(lbGlossary)
                lbGlossary.SelectedIndex = -1
            End If
        End If
    End Sub

    Private Sub txtFilter_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFilter.TextChanged
        If txtFilter.Text.ToString.Length > 0 Then
            ckFilter.Enabled = True
            btSetFilter.Enabled = True
        Else
            ckFilter.Enabled = False
            btSetFilter.Enabled = False
            gl.setfilterOff()
            gl.Fillcontrol(lbGlossary)
            lbGlossary.SelectedIndex = -1
        End If
    End Sub

    Private Sub btSetFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btSetFilter.Click
        If txtFilter.Text.ToString.Length > 0 Then
            If Not ckFilter.Checked Then
                ckFilter.Checked = True
            Else
                gl.setFilter(txtFilter.Text.ToString.Trim)
                gl.setfilterOn()
                gl.Fillcontrol(lbGlossary)
                lbGlossary.SelectedIndex = -1
            End If
        End If
    End Sub

    Private Sub lbGlossary_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbGlossary.SelectedIndexChanged
        Dim d As String, t1(1) As String
        Try
            d = gl.Searchterm2(lbGlossary.SelectedItem.ToString)
            t1(0) = lbGlossary.SelectedItem.ToString
            t1(1) = d
            txtGlossary.Lines = t1
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btClose.Click
        Me.Close()
    End Sub

    Private Sub txtGlossary_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtGlossary.KeyPress
        If e.KeyChar = Convert.ToChar(1) Then
            DirectCast(sender, TextBox).SelectAll()
            e.Handled = True
        End If
    End Sub

End Class