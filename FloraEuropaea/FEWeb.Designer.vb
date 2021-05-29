<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FEWeb
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FEWeb))
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.backToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.forwardToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.stopToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.refreshToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.searchToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.toolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.homeToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.WebBrowser1 = New System.Windows.Forms.WebBrowser()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolStrip1
        '
        Me.ToolStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.backToolStripButton, Me.forwardToolStripButton, Me.stopToolStripButton, Me.refreshToolStripButton, Me.searchToolStripButton, Me.toolStripSeparator2, Me.homeToolStripButton})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(1056, 27)
        Me.ToolStrip1.TabIndex = 0
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'backToolStripButton
        '
        Me.backToolStripButton.Enabled = False
        Me.backToolStripButton.Image = CType(resources.GetObject("backToolStripButton.Image"), System.Drawing.Image)
        Me.backToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.backToolStripButton.Name = "backToolStripButton"
        Me.backToolStripButton.Size = New System.Drawing.Size(64, 24)
        Me.backToolStripButton.Text = "Back"
        '
        'forwardToolStripButton
        '
        Me.forwardToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.forwardToolStripButton.Enabled = False
        Me.forwardToolStripButton.Image = CType(resources.GetObject("forwardToolStripButton.Image"), System.Drawing.Image)
        Me.forwardToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.forwardToolStripButton.Name = "forwardToolStripButton"
        Me.forwardToolStripButton.Size = New System.Drawing.Size(24, 24)
        Me.forwardToolStripButton.Text = "Forward"
        '
        'stopToolStripButton
        '
        Me.stopToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.stopToolStripButton.Image = CType(resources.GetObject("stopToolStripButton.Image"), System.Drawing.Image)
        Me.stopToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.stopToolStripButton.Name = "stopToolStripButton"
        Me.stopToolStripButton.Size = New System.Drawing.Size(24, 24)
        Me.stopToolStripButton.Text = "Stop"
        '
        'refreshToolStripButton
        '
        Me.refreshToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.refreshToolStripButton.Image = CType(resources.GetObject("refreshToolStripButton.Image"), System.Drawing.Image)
        Me.refreshToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.refreshToolStripButton.Name = "refreshToolStripButton"
        Me.refreshToolStripButton.Size = New System.Drawing.Size(24, 24)
        Me.refreshToolStripButton.Text = "Refresh"
        '
        'searchToolStripButton
        '
        Me.searchToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.searchToolStripButton.Image = CType(resources.GetObject("searchToolStripButton.Image"), System.Drawing.Image)
        Me.searchToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.searchToolStripButton.Name = "searchToolStripButton"
        Me.searchToolStripButton.Size = New System.Drawing.Size(24, 24)
        Me.searchToolStripButton.Text = "Search"
        '
        'toolStripSeparator2
        '
        Me.toolStripSeparator2.Name = "toolStripSeparator2"
        Me.toolStripSeparator2.Size = New System.Drawing.Size(6, 27)
        '
        'homeToolStripButton
        '
        Me.homeToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.homeToolStripButton.Image = CType(resources.GetObject("homeToolStripButton.Image"), System.Drawing.Image)
        Me.homeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.homeToolStripButton.Name = "homeToolStripButton"
        Me.homeToolStripButton.Size = New System.Drawing.Size(24, 24)
        Me.homeToolStripButton.Text = "Home"
        '
        'WebBrowser1
        '
        Me.WebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.WebBrowser1.Location = New System.Drawing.Point(0, 27)
        Me.WebBrowser1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.WebBrowser1.MinimumSize = New System.Drawing.Size(27, 25)
        Me.WebBrowser1.Name = "WebBrowser1"
        Me.WebBrowser1.Size = New System.Drawing.Size(1056, 542)
        Me.WebBrowser1.TabIndex = 1
        '
        'FEWeb
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1056, 569)
        Me.Controls.Add(Me.WebBrowser1)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(1061, 481)
        Me.Name = "FEWeb"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "Web"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Private WithEvents backToolStripButton As System.Windows.Forms.ToolStripButton
    Private WithEvents forwardToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents WebBrowser1 As System.Windows.Forms.WebBrowser
    Private WithEvents stopToolStripButton As System.Windows.Forms.ToolStripButton
    Private WithEvents refreshToolStripButton As System.Windows.Forms.ToolStripButton
    Private WithEvents toolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents homeToolStripButton As System.Windows.Forms.ToolStripButton
    Private WithEvents searchToolStripButton As System.Windows.Forms.ToolStripButton
End Class
