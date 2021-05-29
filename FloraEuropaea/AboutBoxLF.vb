Public NotInheritable Class AboutBoxLF

    Private Sub AboutBox1_Load(ByVal sender As System.Object, ByVal e As EventArgs) Handles MyBase.Load
        ' Set the title of the form.
        Dim ApplicationTitle As String
        If My.Application.Info.Title <> "" Then
            ApplicationTitle = My.Application.Info.Title
        Else
            ApplicationTitle = System.IO.Path.GetFileNameWithoutExtension(My.Application.Info.AssemblyName)
        End If
        Me.Text = String.Format("About {0}", ApplicationTitle)
        ' Initialize all of the text displayed on the About Box.
        ' TODO: Customize the application's assembly information in the "Application" pane of the project 
        '    properties dialog (under the "Project" menu).
        Me.LabelProductName.Text = My.Application.Info.ProductName
        Me.LabelVersion.Text = String.Format("Version {0}", My.Application.Info.Version.ToString)
        Me.LabelCopyright.Text = My.Application.Info.Copyright
        Me.LabelCompanyName.Text = My.Application.Info.CompanyName
        Me.TextBoxDescription.Text = My.Application.Info.Description
    End Sub

    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles OKButton.Click
        FEWeb.Close()
        Me.Close()
    End Sub

    Private Sub LabelVersion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelVersion.Click

    End Sub

    Private Sub LabelCopyright_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelCopyright.Click

    End Sub

    Private Sub LogoPictureBox_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LogoPictureBox.Click
        FEWeb.WebBrowser1.Url = New Uri("http://www.foerderer.ch/")
        FEWeb.Show()
        FEWeb.Activate()
    End Sub


    Private Sub AboutBox1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LostFocus
        FEWeb.Close()
    End Sub


End Class
