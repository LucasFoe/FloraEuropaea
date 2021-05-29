Public Class FEWeb

    Private Sub refreshState()
        If WebBrowser1.CanGoBack Then
            backToolStripButton.Enabled = True
        Else
            backToolStripButton.Enabled = False
        End If
        If WebBrowser1.CanGoForward Then
            forwardToolStripButton.Enabled = True
        Else
            forwardToolStripButton.Enabled = False
        End If
    End Sub

    Private Sub ToolStripContainer1_ContentPanel_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
        WebBrowser1.GoSearch()
    End Sub

    Private Sub backToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles backToolStripButton.Click
        WebBrowser1.GoBack()
    End Sub

    Private Sub forwardToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles forwardToolStripButton.Click
        WebBrowser1.GoForward()
    End Sub

    Private Sub stopToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles stopToolStripButton.Click
        WebBrowser1.Stop()
    End Sub

    Private Sub refreshToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles refreshToolStripButton.Click
        WebBrowser1.Refresh()
    End Sub

    Private Sub homeToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles homeToolStripButton.Click
        Me.WebBrowser1.Url = New Uri("http://www.foerderer.ch/")
    End Sub

    Private Sub searchToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles searchToolStripButton.Click
        WebBrowser1.GoSearch()
    End Sub

    Private Sub WebBrowser1_DocumentCompleted(ByVal sender As System.Object, ByVal e As System.Windows.Forms.WebBrowserDocumentCompletedEventArgs) Handles WebBrowser1.DocumentCompleted
        refreshToolStripButton.Enabled = True
    End Sub

    Private Sub WebBrowser1_CommandStateChange(ByVal sender As System.Object, ByVal e As System.Windows.Forms.WebBrowserNavigatedEventArgs) Handles WebBrowser1.Navigated
        refreshState()
    End Sub

End Class