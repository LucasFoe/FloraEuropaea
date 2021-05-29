Imports System.Windows.Forms

Public Class FEUser

    Private selUserCmd As UserCmd

    Public ReadOnly Property SelectedUserCmdProperty() As UserCmd
        Get
            Return selUserCmd
        End Get
    End Property

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub FEUser_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.lbCmd.Items.Clear()
        Me.lbCmd.Items.AddRange(StartMain.ucList.UserCmdListProperty.ToArray)
    End Sub

    Private Sub lbCmd_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCmd.DoubleClick
        selUserCmd = lbCmd.SelectedItem
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub lbCmd_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCmd.SelectedIndexChanged
        selUserCmd = lbCmd.SelectedItem
    End Sub

End Class
