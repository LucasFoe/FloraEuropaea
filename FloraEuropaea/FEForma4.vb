Public Class FEForma4

    Private Sub FEForma4_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        PropertyGrid1.SelectedObject = sett
        ' PropertyGridController.SetLabelWidth(PropertyGrid1, 0.1)
    End Sub

    Private Sub btSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btSave.Click
        sett.SaveSettings()
        sett = New Setting
        Me.Close()
    End Sub

    Private Sub btCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btCancel.Click
        Me.Close()
    End Sub

End Class