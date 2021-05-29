<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FEForma2
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
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FEForma2))
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.Code = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.KeyDesc = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.KeyLink = New System.Windows.Forms.DataGridViewLinkColumn()
        Me.KeyIdx = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.KeyRef = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.cbHistory = New System.Windows.Forms.ComboBox()
        Me.btParentKey = New System.Windows.Forms.Button()
        Me.btLink = New System.Windows.Forms.Button()
        Me.btNext = New System.Windows.Forms.Button()
        Me.btPrev = New System.Windows.Forms.Button()
        Me.btSwitch = New System.Windows.Forms.Button()
        Me.btClose = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.HelpProvider1 = New System.Windows.Forms.HelpProvider()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.AllowUserToResizeColumns = False
        Me.DataGridView1.AllowUserToResizeRows = False
        Me.DataGridView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.DataGridView1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.ColumnHeadersVisible = False
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Code, Me.KeyDesc, Me.KeyLink, Me.KeyIdx, Me.KeyRef})
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridView1.DefaultCellStyle = DataGridViewCellStyle4
        Me.DataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.HelpProvider1.SetHelpKeyword(Me.DataGridView1, "html\hs50.htm")
        Me.HelpProvider1.SetHelpNavigator(Me.DataGridView1, System.Windows.Forms.HelpNavigator.Topic)
        Me.DataGridView1.Location = New System.Drawing.Point(0, 0)
        Me.DataGridView1.MultiSelect = False
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.RowHeadersVisible = False
        Me.DataGridView1.RowHeadersWidth = 10
        Me.DataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.HelpProvider1.SetShowHelp(Me.DataGridView1, True)
        Me.DataGridView1.Size = New System.Drawing.Size(749, 404)
        Me.DataGridView1.TabIndex = 0
        '
        'Code
        '
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.Gainsboro
        DataGridViewCellStyle1.NullValue = Nothing
        DataGridViewCellStyle1.Padding = New System.Windows.Forms.Padding(0, 0, 3, 0)
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Yellow
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Red
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Code.DefaultCellStyle = DataGridViewCellStyle1
        Me.Code.HeaderText = "Code"
        Me.Code.Name = "Code"
        Me.Code.ReadOnly = True
        Me.Code.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Code.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Code.Width = 5
        '
        'KeyDesc
        '
        Me.KeyDesc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle2.NullValue = Nothing
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.White
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.KeyDesc.DefaultCellStyle = DataGridViewCellStyle2
        Me.KeyDesc.HeaderText = "KeyDesc"
        Me.KeyDesc.Name = "KeyDesc"
        Me.KeyDesc.ReadOnly = True
        Me.KeyDesc.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'KeyLink
        '
        Me.KeyLink.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        DataGridViewCellStyle3.BackColor = System.Drawing.Color.Gainsboro
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.KeyLink.DefaultCellStyle = DataGridViewCellStyle3
        Me.KeyLink.HeaderText = "KeyLink"
        Me.KeyLink.Name = "KeyLink"
        Me.KeyLink.ReadOnly = True
        Me.KeyLink.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.KeyLink.Width = 5
        '
        'KeyIdx
        '
        Me.KeyIdx.HeaderText = "KeyIdx"
        Me.KeyIdx.Name = "KeyIdx"
        Me.KeyIdx.ReadOnly = True
        Me.KeyIdx.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.KeyIdx.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.KeyIdx.Visible = False
        Me.KeyIdx.Width = 5
        '
        'KeyRef
        '
        Me.KeyRef.HeaderText = "KeyRef"
        Me.KeyRef.Name = "KeyRef"
        Me.KeyRef.ReadOnly = True
        Me.KeyRef.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.KeyRef.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.KeyRef.Visible = False
        Me.KeyRef.Width = 5
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.cbHistory)
        Me.GroupBox1.Controls.Add(Me.btParentKey)
        Me.GroupBox1.Controls.Add(Me.btLink)
        Me.GroupBox1.Controls.Add(Me.btNext)
        Me.GroupBox1.Controls.Add(Me.btPrev)
        Me.GroupBox1.Controls.Add(Me.btSwitch)
        Me.GroupBox1.Controls.Add(Me.btClose)
        Me.HelpProvider1.SetHelpKeyword(Me.GroupBox1, "html\hs60.htm      ")
        Me.HelpProvider1.SetHelpNavigator(Me.GroupBox1, System.Windows.Forms.HelpNavigator.Topic)
        Me.GroupBox1.Location = New System.Drawing.Point(3, 410)
        Me.GroupBox1.Name = "GroupBox1"
        Me.HelpProvider1.SetShowHelp(Me.GroupBox1, True)
        Me.GroupBox1.Size = New System.Drawing.Size(743, 49)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        '
        'cbHistory
        '
        Me.cbHistory.FormattingEnabled = True
        Me.cbHistory.Location = New System.Drawing.Point(432, 19)
        Me.cbHistory.Name = "cbHistory"
        Me.cbHistory.Size = New System.Drawing.Size(201, 21)
        Me.cbHistory.TabIndex = 6
        Me.cbHistory.Text = "History"
        '
        'btParentKey
        '
        Me.btParentKey.Enabled = False
        Me.btParentKey.Location = New System.Drawing.Point(344, 19)
        Me.btParentKey.Name = "btParentKey"
        Me.btParentKey.Size = New System.Drawing.Size(76, 21)
        Me.btParentKey.TabIndex = 5
        Me.btParentKey.Text = "Parent Key"
        Me.btParentKey.UseVisualStyleBackColor = True
        '
        'btLink
        '
        Me.btLink.Enabled = False
        Me.btLink.Location = New System.Drawing.Point(263, 19)
        Me.btLink.Name = "btLink"
        Me.btLink.Size = New System.Drawing.Size(68, 21)
        Me.btLink.TabIndex = 4
        Me.btLink.Text = "Link"
        Me.btLink.UseVisualStyleBackColor = True
        '
        'btNext
        '
        Me.btNext.Location = New System.Drawing.Point(177, 19)
        Me.btNext.Name = "btNext"
        Me.btNext.Size = New System.Drawing.Size(65, 21)
        Me.btNext.TabIndex = 3
        Me.btNext.Text = ">"
        Me.btNext.UseVisualStyleBackColor = True
        '
        'btPrev
        '
        Me.btPrev.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btPrev.Location = New System.Drawing.Point(101, 19)
        Me.btPrev.Name = "btPrev"
        Me.btPrev.Size = New System.Drawing.Size(65, 21)
        Me.btPrev.TabIndex = 2
        Me.btPrev.Text = "<"
        Me.btPrev.UseVisualStyleBackColor = True
        '
        'btSwitch
        '
        Me.btSwitch.Location = New System.Drawing.Point(16, 19)
        Me.btSwitch.Name = "btSwitch"
        Me.btSwitch.Size = New System.Drawing.Size(65, 21)
        Me.btSwitch.TabIndex = 1
        Me.btSwitch.Text = "Switch"
        Me.btSwitch.UseVisualStyleBackColor = True
        '
        'btClose
        '
        Me.btClose.Location = New System.Drawing.Point(649, 19)
        Me.btClose.Name = "btClose"
        Me.btClose.Size = New System.Drawing.Size(55, 21)
        Me.btClose.TabIndex = 0
        Me.btClose.Text = "Close"
        Me.btClose.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.GroupBox1)
        Me.Panel1.Controls.Add(Me.DataGridView1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(749, 462)
        Me.Panel1.TabIndex = 2
        '
        'HelpProvider1
        '
        Me.HelpProvider1.HelpNamespace = "ContextHelp.chm"
        '
        'FEForma2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(749, 462)
        Me.Controls.Add(Me.Panel1)
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(550, 300)
        Me.Name = "FEForma2"
        Me.Text = "Flora Europaea Key Browser"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btClose As System.Windows.Forms.Button
    Friend WithEvents btSwitch As System.Windows.Forms.Button
    Friend WithEvents btNext As System.Windows.Forms.Button
    Friend WithEvents btPrev As System.Windows.Forms.Button
    Friend WithEvents btLink As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btParentKey As System.Windows.Forms.Button
    Friend WithEvents cbHistory As System.Windows.Forms.ComboBox
    Friend WithEvents Code As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents KeyDesc As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents KeyLink As System.Windows.Forms.DataGridViewLinkColumn
    Friend WithEvents KeyIdx As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents KeyRef As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents HelpProvider1 As System.Windows.Forms.HelpProvider
End Class
