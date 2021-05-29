<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FEForma4
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FEForma4))
        Me.PropertyGrid1 = New System.Windows.Forms.PropertyGrid()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.btCancel = New System.Windows.Forms.Button()
        Me.btSave = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'PropertyGrid1
        '
        Me.PropertyGrid1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PropertyGrid1.Location = New System.Drawing.Point(0, 0)
        Me.PropertyGrid1.Name = "PropertyGrid1"
        Me.PropertyGrid1.Size = New System.Drawing.Size(696, 376)
        Me.PropertyGrid1.TabIndex = 0
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.btCancel)
        Me.GroupBox1.Controls.Add(Me.btSave)
        Me.GroupBox1.Location = New System.Drawing.Point(0, 372)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(696, 59)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.UseCompatibleTextRendering = True
        '
        'btCancel
        '
        Me.btCancel.Location = New System.Drawing.Point(93, 19)
        Me.btCancel.Name = "btCancel"
        Me.btCancel.Size = New System.Drawing.Size(54, 21)
        Me.btCancel.TabIndex = 1
        Me.btCancel.Text = "Cancel"
        Me.btCancel.UseVisualStyleBackColor = True
        '
        'btSave
        '
        Me.btSave.Location = New System.Drawing.Point(24, 19)
        Me.btSave.Name = "btSave"
        Me.btSave.Size = New System.Drawing.Size(54, 21)
        Me.btSave.TabIndex = 0
        Me.btSave.Text = "Save"
        Me.btSave.UseVisualStyleBackColor = True
        '
        'FEForma4
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(696, 429)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.PropertyGrid1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FEForma4"
        Me.ShowInTaskbar = False
        Me.Text = "Settings"
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PropertyGrid1 As System.Windows.Forms.PropertyGrid
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btSave As System.Windows.Forms.Button
    Friend WithEvents btCancel As System.Windows.Forms.Button
End Class
