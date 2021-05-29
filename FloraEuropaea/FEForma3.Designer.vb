<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FEForma3
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FEForma3))
        Me.lbGlossary = New System.Windows.Forms.ListBox()
        Me.txtGlossary = New System.Windows.Forms.TextBox()
        Me.btSetFilter = New System.Windows.Forms.Button()
        Me.lbFilter = New System.Windows.Forms.Label()
        Me.ckFilter = New System.Windows.Forms.CheckBox()
        Me.txtFilter = New System.Windows.Forms.TextBox()
        Me.btClose = New System.Windows.Forms.Button()
        Me.HelpProvider1 = New System.Windows.Forms.HelpProvider()
        Me.SuspendLayout()
        '
        'lbGlossary
        '
        Me.lbGlossary.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.lbGlossary.FormattingEnabled = True
        Me.HelpProvider1.SetHelpKeyword(Me.lbGlossary, "html\hs70.htm")
        Me.HelpProvider1.SetHelpNavigator(Me.lbGlossary, System.Windows.Forms.HelpNavigator.Topic)
        Me.HelpProvider1.SetHelpString(Me.lbGlossary, "")
        Me.lbGlossary.ItemHeight = 19
        Me.lbGlossary.Location = New System.Drawing.Point(4, 45)
        Me.lbGlossary.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.lbGlossary.Name = "lbGlossary"
        Me.HelpProvider1.SetShowHelp(Me.lbGlossary, True)
        Me.lbGlossary.Size = New System.Drawing.Size(242, 422)
        Me.lbGlossary.TabIndex = 0
        '
        'txtGlossary
        '
        Me.txtGlossary.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtGlossary.BackColor = System.Drawing.SystemColors.Window
        Me.txtGlossary.Font = New System.Drawing.Font("Times New Roman", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.HelpProvider1.SetHelpKeyword(Me.txtGlossary, "html\hs90.htm")
        Me.HelpProvider1.SetHelpNavigator(Me.txtGlossary, System.Windows.Forms.HelpNavigator.Topic)
        Me.txtGlossary.Location = New System.Drawing.Point(251, 1)
        Me.txtGlossary.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.txtGlossary.Multiline = True
        Me.txtGlossary.Name = "txtGlossary"
        Me.txtGlossary.ReadOnly = True
        Me.txtGlossary.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.HelpProvider1.SetShowHelp(Me.txtGlossary, True)
        Me.txtGlossary.Size = New System.Drawing.Size(540, 519)
        Me.txtGlossary.TabIndex = 1
        '
        'btSetFilter
        '
        Me.HelpProvider1.SetHelpKeyword(Me.btSetFilter, "html\hs80.htm")
        Me.HelpProvider1.SetHelpNavigator(Me.btSetFilter, System.Windows.Forms.HelpNavigator.Topic)
        Me.btSetFilter.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btSetFilter.Location = New System.Drawing.Point(186, 14)
        Me.btSetFilter.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btSetFilter.Name = "btSetFilter"
        Me.HelpProvider1.SetShowHelp(Me.btSetFilter, True)
        Me.btSetFilter.Size = New System.Drawing.Size(51, 24)
        Me.btSetFilter.TabIndex = 48
        Me.btSetFilter.Text = "Set"
        Me.btSetFilter.UseVisualStyleBackColor = True
        '
        'lbFilter
        '
        Me.lbFilter.AutoSize = True
        Me.lbFilter.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lbFilter.Location = New System.Drawing.Point(9, 18)
        Me.lbFilter.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbFilter.Name = "lbFilter"
        Me.lbFilter.Size = New System.Drawing.Size(33, 15)
        Me.lbFilter.TabIndex = 47
        Me.lbFilter.Text = "Filter"
        '
        'ckFilter
        '
        Me.ckFilter.AutoSize = True
        Me.ckFilter.Enabled = False
        Me.HelpProvider1.SetHelpKeyword(Me.ckFilter, "html\hs80.htm")
        Me.HelpProvider1.SetHelpNavigator(Me.ckFilter, System.Windows.Forms.HelpNavigator.Topic)
        Me.ckFilter.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.ckFilter.Location = New System.Drawing.Point(156, 17)
        Me.ckFilter.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.ckFilter.Name = "ckFilter"
        Me.HelpProvider1.SetShowHelp(Me.ckFilter, True)
        Me.ckFilter.Size = New System.Drawing.Size(15, 14)
        Me.ckFilter.TabIndex = 46
        Me.ckFilter.UseVisualStyleBackColor = True
        '
        'txtFilter
        '
        Me.HelpProvider1.SetHelpKeyword(Me.txtFilter, "html\hs80.htm")
        Me.HelpProvider1.SetHelpNavigator(Me.txtFilter, System.Windows.Forms.HelpNavigator.Topic)
        Me.txtFilter.Location = New System.Drawing.Point(52, 14)
        Me.txtFilter.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.txtFilter.Name = "txtFilter"
        Me.HelpProvider1.SetShowHelp(Me.txtFilter, True)
        Me.txtFilter.Size = New System.Drawing.Size(84, 23)
        Me.txtFilter.TabIndex = 45
        '
        'btClose
        '
        Me.HelpProvider1.SetHelpKeyword(Me.btClose, "")
        Me.HelpProvider1.SetHelpNavigator(Me.btClose, System.Windows.Forms.HelpNavigator.TopicId)
        Me.btClose.Location = New System.Drawing.Point(78, 484)
        Me.btClose.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btClose.Name = "btClose"
        Me.HelpProvider1.SetShowHelp(Me.btClose, False)
        Me.btClose.Size = New System.Drawing.Size(94, 24)
        Me.btClose.TabIndex = 49
        Me.btClose.Text = "Close"
        Me.btClose.UseVisualStyleBackColor = True
        '
        'HelpProvider1
        '
        Me.HelpProvider1.HelpNamespace = "ContextHelp.chm"
        '
        'FEForma3
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(792, 519)
        Me.Controls.Add(Me.btClose)
        Me.Controls.Add(Me.btSetFilter)
        Me.Controls.Add(Me.lbFilter)
        Me.Controls.Add(Me.ckFilter)
        Me.Controls.Add(Me.txtFilter)
        Me.Controls.Add(Me.txtGlossary)
        Me.Controls.Add(Me.lbGlossary)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(100, 100)
        Me.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(808, 558)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(808, 558)
        Me.Name = "FEForma3"
        Me.ShowInTaskbar = False
        Me.Text = "Browse Glossary"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lbGlossary As System.Windows.Forms.ListBox
    Friend WithEvents txtGlossary As System.Windows.Forms.TextBox
    Friend WithEvents btSetFilter As System.Windows.Forms.Button
    Friend WithEvents lbFilter As System.Windows.Forms.Label
    Friend WithEvents ckFilter As System.Windows.Forms.CheckBox
    Friend WithEvents txtFilter As System.Windows.Forms.TextBox
    Friend WithEvents btClose As System.Windows.Forms.Button
    Friend WithEvents HelpProvider1 As System.Windows.Forms.HelpProvider
End Class
