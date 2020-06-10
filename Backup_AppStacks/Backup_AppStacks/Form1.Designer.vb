<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.pb2 = New System.Windows.Forms.PictureBox()
        Me.lstAS = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ShowInstalledApplicationsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmdpopAS = New System.Windows.Forms.Button()
        Me.txtAVManager = New System.Windows.Forms.TextBox()
        Me.lblAVManager = New System.Windows.Forms.Label()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.sessionstatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.pb1 = New System.Windows.Forms.PictureBox()
        Me.tv_vcenter = New System.Windows.Forms.TreeView()
        Me.cmdConnecttoVC = New System.Windows.Forms.Button()
        Me.txtvc = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cmdCreateVM = New System.Windows.Forms.Button()
        Me.cmdAttachAS = New System.Windows.Forms.Button()
        Me.cmdDeleteVM = New System.Windows.Forms.Button()
        Me.cmdRemoveDrives = New System.Windows.Forms.Button()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GroupBox1.SuspendLayout()
        CType(Me.pb2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.pb1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.pb2)
        Me.GroupBox1.Controls.Add(Me.lstAS)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.cmdpopAS)
        Me.GroupBox1.Controls.Add(Me.txtAVManager)
        Me.GroupBox1.Controls.Add(Me.lblAVManager)
        Me.GroupBox1.Location = New System.Drawing.Point(11, 21)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(569, 235)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "App Volumes"
        '
        'pb2
        '
        Me.pb2.BackColor = System.Drawing.SystemColors.Window
        Me.pb2.Image = CType(resources.GetObject("pb2.Image"), System.Drawing.Image)
        Me.pb2.InitialImage = CType(resources.GetObject("pb2.InitialImage"), System.Drawing.Image)
        Me.pb2.Location = New System.Drawing.Point(251, 122)
        Me.pb2.Name = "pb2"
        Me.pb2.Size = New System.Drawing.Size(64, 64)
        Me.pb2.TabIndex = 10
        Me.pb2.TabStop = False
        Me.pb2.Visible = False
        '
        'lstAS
        '
        Me.lstAS.CheckBoxes = True
        Me.lstAS.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5, Me.ColumnHeader6})
        Me.lstAS.ContextMenuStrip = Me.ContextMenuStrip1
        Me.lstAS.GridLines = True
        Me.lstAS.Location = New System.Drawing.Point(15, 69)
        Me.lstAS.Name = "lstAS"
        Me.lstAS.Size = New System.Drawing.Size(538, 158)
        Me.lstAS.SmallImageList = Me.ImageList1
        Me.lstAS.TabIndex = 6
        Me.lstAS.UseCompatibleStateImageBehavior = False
        Me.lstAS.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "AppStack Name"
        Me.ColumnHeader1.Width = 210
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Location"
        Me.ColumnHeader2.Width = 110
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Size (MB)"
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Attachments"
        Me.ColumnHeader4.Width = 75
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "ID"
        Me.ColumnHeader5.Width = 0
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "Status"
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ShowInstalledApplicationsToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(220, 26)
        '
        'ShowInstalledApplicationsToolStripMenuItem
        '
        Me.ShowInstalledApplicationsToolStripMenuItem.Name = "ShowInstalledApplicationsToolStripMenuItem"
        Me.ShowInstalledApplicationsToolStripMenuItem.Size = New System.Drawing.Size(219, 22)
        Me.ShowInstalledApplicationsToolStripMenuItem.Text = "Show Installed Applications"
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "computer(2).ico")
        Me.ImageList1.Images.SetKeyName(1, "database(1).ico")
        Me.ImageList1.Images.SetKeyName(2, "pen_write_edit.ico")
        Me.ImageList1.Images.SetKeyName(3, "vmware_vsphere_client_high_def_icon_by_flakshack_d4o96dy_b1S_icon.ico")
        Me.ImageList1.Images.SetKeyName(4, "server.ico")
        Me.ImageList1.Images.SetKeyName(5, "server_database.ico")
        Me.ImageList1.Images.SetKeyName(6, "database_error.ico")
        Me.ImageList1.Images.SetKeyName(7, "tick.ico")
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(17, 50)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(70, 13)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "AppStacks:"
        '
        'cmdpopAS
        '
        Me.cmdpopAS.Location = New System.Drawing.Point(394, 19)
        Me.cmdpopAS.Name = "cmdpopAS"
        Me.cmdpopAS.Size = New System.Drawing.Size(127, 23)
        Me.cmdpopAS.TabIndex = 4
        Me.cmdpopAS.Text = "Populate AppStacks"
        Me.cmdpopAS.UseVisualStyleBackColor = True
        '
        'txtAVManager
        '
        Me.txtAVManager.Location = New System.Drawing.Point(155, 20)
        Me.txtAVManager.Name = "txtAVManager"
        Me.txtAVManager.Size = New System.Drawing.Size(233, 21)
        Me.txtAVManager.TabIndex = 3
        '
        'lblAVManager
        '
        Me.lblAVManager.AutoSize = True
        Me.lblAVManager.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAVManager.Location = New System.Drawing.Point(12, 20)
        Me.lblAVManager.Name = "lblAVManager"
        Me.lblAVManager.Size = New System.Drawing.Size(136, 13)
        Me.lblAVManager.TabIndex = 2
        Me.lblAVManager.Text = "App Volumes Manager:"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.sessionstatus})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 575)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(587, 22)
        Me.StatusStrip1.TabIndex = 1
        '
        'sessionstatus
        '
        Me.sessionstatus.Name = "sessionstatus"
        Me.sessionstatus.Size = New System.Drawing.Size(0, 17)
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.pb1)
        Me.GroupBox2.Controls.Add(Me.tv_vcenter)
        Me.GroupBox2.Controls.Add(Me.cmdConnecttoVC)
        Me.GroupBox2.Controls.Add(Me.txtvc)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Location = New System.Drawing.Point(11, 321)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(569, 250)
        Me.GroupBox2.TabIndex = 2
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Virtual Center"
        '
        'pb1
        '
        Me.pb1.BackColor = System.Drawing.SystemColors.Window
        Me.pb1.Image = CType(resources.GetObject("pb1.Image"), System.Drawing.Image)
        Me.pb1.InitialImage = CType(resources.GetObject("pb1.InitialImage"), System.Drawing.Image)
        Me.pb1.Location = New System.Drawing.Point(251, 106)
        Me.pb1.Name = "pb1"
        Me.pb1.Size = New System.Drawing.Size(64, 64)
        Me.pb1.TabIndex = 9
        Me.pb1.TabStop = False
        Me.pb1.Visible = False
        '
        'tv_vcenter
        '
        Me.tv_vcenter.CheckBoxes = True
        Me.tv_vcenter.ImageIndex = 0
        Me.tv_vcenter.ImageList = Me.ImageList1
        Me.tv_vcenter.Location = New System.Drawing.Point(15, 49)
        Me.tv_vcenter.Name = "tv_vcenter"
        Me.tv_vcenter.SelectedImageIndex = 0
        Me.tv_vcenter.Size = New System.Drawing.Size(538, 195)
        Me.tv_vcenter.TabIndex = 8
        '
        'cmdConnecttoVC
        '
        Me.cmdConnecttoVC.Location = New System.Drawing.Point(394, 19)
        Me.cmdConnecttoVC.Name = "cmdConnecttoVC"
        Me.cmdConnecttoVC.Size = New System.Drawing.Size(127, 23)
        Me.cmdConnecttoVC.TabIndex = 4
        Me.cmdConnecttoVC.Text = "Populate VC Data"
        Me.cmdConnecttoVC.UseVisualStyleBackColor = True
        '
        'txtvc
        '
        Me.txtvc.Location = New System.Drawing.Point(155, 20)
        Me.txtvc.Name = "txtvc"
        Me.txtvc.Size = New System.Drawing.Size(233, 21)
        Me.txtvc.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(61, 24)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(88, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Virtual Center:"
        '
        'cmdCreateVM
        '
        Me.cmdCreateVM.Enabled = False
        Me.cmdCreateVM.Location = New System.Drawing.Point(302, 287)
        Me.cmdCreateVM.Name = "cmdCreateVM"
        Me.cmdCreateVM.Size = New System.Drawing.Size(261, 27)
        Me.cmdCreateVM.TabIndex = 7
        Me.cmdCreateVM.Text = "Create a Backup VM"
        Me.cmdCreateVM.UseVisualStyleBackColor = True
        '
        'cmdAttachAS
        '
        Me.cmdAttachAS.Location = New System.Drawing.Point(27, 259)
        Me.cmdAttachAS.Name = "cmdAttachAS"
        Me.cmdAttachAS.Size = New System.Drawing.Size(261, 27)
        Me.cmdAttachAS.TabIndex = 3
        Me.cmdAttachAS.Text = "Attach Selected AppStacks to Backup VM"
        Me.cmdAttachAS.UseVisualStyleBackColor = True
        '
        'cmdDeleteVM
        '
        Me.cmdDeleteVM.Location = New System.Drawing.Point(27, 287)
        Me.cmdDeleteVM.Name = "cmdDeleteVM"
        Me.cmdDeleteVM.Size = New System.Drawing.Size(261, 27)
        Me.cmdDeleteVM.TabIndex = 4
        Me.cmdDeleteVM.Text = "Delete Selected Backup VM"
        Me.cmdDeleteVM.UseVisualStyleBackColor = True
        '
        'cmdRemoveDrives
        '
        Me.cmdRemoveDrives.Location = New System.Drawing.Point(302, 259)
        Me.cmdRemoveDrives.Name = "cmdRemoveDrives"
        Me.cmdRemoveDrives.Size = New System.Drawing.Size(261, 27)
        Me.cmdRemoveDrives.TabIndex = 8
        Me.cmdRemoveDrives.Text = "Detach Selected AppStacks from Backup VM"
        Me.cmdRemoveDrives.UseVisualStyleBackColor = True
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(587, 24)
        Me.MenuStrip1.TabIndex = 9
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AboutToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(107, 22)
        Me.AboutToolStripMenuItem.Text = "About"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(107, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(587, 597)
        Me.Controls.Add(Me.cmdRemoveDrives)
        Me.Controls.Add(Me.cmdDeleteVM)
        Me.Controls.Add(Me.cmdCreateVM)
        Me.Controls.Add(Me.cmdAttachAS)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Backup App Volumes"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.pb2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.pb1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents cmdpopAS As System.Windows.Forms.Button
    Friend WithEvents txtAVManager As System.Windows.Forms.TextBox
    Friend WithEvents lblAVManager As System.Windows.Forms.Label
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents sessionstatus As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents lstAS As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents cmdConnecttoVC As System.Windows.Forms.Button
    Friend WithEvents txtvc As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cmdCreateVM As System.Windows.Forms.Button
    Friend WithEvents tv_vcenter As System.Windows.Forms.TreeView
    Friend WithEvents cmdAttachAS As System.Windows.Forms.Button
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents cmdDeleteVM As System.Windows.Forms.Button
    Friend WithEvents cmdRemoveDrives As System.Windows.Forms.Button
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ShowInstalledApplicationsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pb1 As System.Windows.Forms.PictureBox
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pb2 As System.Windows.Forms.PictureBox
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader

End Class
