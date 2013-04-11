<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDNRWATProfiler
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDNRWATProfiler))
        Me.gboLayers = New System.Windows.Forms.GroupBox()
        Me.cboXSecLabel = New System.Windows.Forms.ComboBox()
        Me.lblXSecLabel = New System.Windows.Forms.Label()
        Me.cboXSecLayer = New System.Windows.Forms.ComboBox()
        Me.lblXSecLayer = New System.Windows.Forms.Label()
        Me.lblRasterLayer = New System.Windows.Forms.Label()
        Me.cboRasterLayer = New System.Windows.Forms.ComboBox()
        Me.obuSurface = New System.Windows.Forms.RadioButton()
        Me.obuRaster = New System.Windows.Forms.RadioButton()
        Me.lblOutput = New System.Windows.Forms.Label()
        Me.tboOutput = New System.Windows.Forms.TextBox()
        Me.comOutput = New System.Windows.Forms.Button()
        Me.ilsEPForm = New System.Windows.Forms.ImageList(Me.components)
        Me.lblBaseName = New System.Windows.Forms.Label()
        Me.tboBaseName = New System.Windows.Forms.TextBox()
        Me.lblVertEx = New System.Windows.Forms.Label()
        Me.tboVertEx = New System.Windows.Forms.TextBox()
        Me.comCancel = New System.Windows.Forms.Button()
        Me.comGenProfiles = New System.Windows.Forms.Button()
        Me.flddlgEPBrowseFolders = New System.Windows.Forms.FolderBrowserDialog()
        Me.lboRasterLayers = New System.Windows.Forms.ListBox()
        Me.gboLayers.SuspendLayout
        Me.SuspendLayout
        '
        'gboLayers
        '
        Me.gboLayers.Controls.Add(Me.cboXSecLabel)
        Me.gboLayers.Controls.Add(Me.lblXSecLabel)
        Me.gboLayers.Controls.Add(Me.cboXSecLayer)
        Me.gboLayers.Controls.Add(Me.lblXSecLayer)
        Me.gboLayers.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.gboLayers.Location = New System.Drawing.Point(12, 12)
        Me.gboLayers.Name = "gboLayers"
        Me.gboLayers.Size = New System.Drawing.Size(288, 67)
        Me.gboLayers.TabIndex = 0
        Me.gboLayers.TabStop = false
        Me.gboLayers.Text = "Input cross sections"
        '
        'cboXSecLabel
        '
        Me.cboXSecLabel.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cboXSecLabel.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboXSecLabel.BackColor = System.Drawing.SystemColors.Control
        Me.cboXSecLabel.Enabled = false
        Me.cboXSecLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cboXSecLabel.FormattingEnabled = true
        Me.cboXSecLabel.Location = New System.Drawing.Point(179, 34)
        Me.cboXSecLabel.Name = "cboXSecLabel"
        Me.cboXSecLabel.Size = New System.Drawing.Size(103, 21)
        Me.cboXSecLabel.TabIndex = 3
        '
        'lblXSecLabel
        '
        Me.lblXSecLabel.AutoSize = true
        Me.lblXSecLabel.Enabled = false
        Me.lblXSecLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblXSecLabel.Location = New System.Drawing.Point(176, 17)
        Me.lblXSecLabel.Name = "lblXSecLabel"
        Me.lblXSecLabel.Size = New System.Drawing.Size(58, 13)
        Me.lblXSecLabel.TabIndex = 2
        Me.lblXSecLabel.Text = "Label field:"
        '
        'cboXSecLayer
        '
        Me.cboXSecLayer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cboXSecLayer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboXSecLayer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cboXSecLayer.FormattingEnabled = true
        Me.cboXSecLayer.Location = New System.Drawing.Point(9, 34)
        Me.cboXSecLayer.Name = "cboXSecLayer"
        Me.cboXSecLayer.Size = New System.Drawing.Size(146, 21)
        Me.cboXSecLayer.TabIndex = 1
        '
        'lblXSecLayer
        '
        Me.lblXSecLayer.AutoSize = true
        Me.lblXSecLayer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblXSecLayer.Location = New System.Drawing.Point(6, 17)
        Me.lblXSecLayer.Name = "lblXSecLayer"
        Me.lblXSecLayer.Size = New System.Drawing.Size(136, 13)
        Me.lblXSecLayer.TabIndex = 0
        Me.lblXSecLayer.Text = "Choose cross section layer:"
        '
        'lblRasterLayer
        '
        Me.lblRasterLayer.AutoSize = true
        Me.lblRasterLayer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblRasterLayer.Location = New System.Drawing.Point(6, 92)
        Me.lblRasterLayer.Name = "lblRasterLayer"
        Me.lblRasterLayer.Size = New System.Drawing.Size(74, 13)
        Me.lblRasterLayer.TabIndex = 1
        Me.lblRasterLayer.Text = "Input raster(s):"
        '
        'cboRasterLayer
        '
        Me.cboRasterLayer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cboRasterLayer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboRasterLayer.FormattingEnabled = true
        Me.cboRasterLayer.Location = New System.Drawing.Point(89, 89)
        Me.cboRasterLayer.Name = "cboRasterLayer"
        Me.cboRasterLayer.Size = New System.Drawing.Size(211, 21)
        Me.cboRasterLayer.TabIndex = 200
        Me.cboRasterLayer.TabStop = false
        '
        'obuSurface
        '
        Me.obuSurface.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.obuSurface.AutoSize = true
        Me.obuSurface.Checked = true
        Me.obuSurface.Enabled = false
        Me.obuSurface.Location = New System.Drawing.Point(17, 177)
        Me.obuSurface.Name = "obuSurface"
        Me.obuSurface.Size = New System.Drawing.Size(133, 17)
        Me.obuSurface.TabIndex = 3
        Me.obuSurface.TabStop = true
        Me.obuSurface.Text = "Use generated surface"
        Me.obuSurface.UseVisualStyleBackColor = true
        '
        'obuRaster
        '
        Me.obuRaster.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.obuRaster.AutoSize = true
        Me.obuRaster.Enabled = false
        Me.obuRaster.Location = New System.Drawing.Point(153, 177)
        Me.obuRaster.Name = "obuRaster"
        Me.obuRaster.Size = New System.Drawing.Size(148, 17)
        Me.obuRaster.TabIndex = 4
        Me.obuRaster.Text = "Use original raster (slower)"
        Me.obuRaster.UseVisualStyleBackColor = true
        '
        'lblOutput
        '
        Me.lblOutput.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.lblOutput.AutoSize = true
        Me.lblOutput.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblOutput.Location = New System.Drawing.Point(6, 204)
        Me.lblOutput.Name = "lblOutput"
        Me.lblOutput.Size = New System.Drawing.Size(77, 13)
        Me.lblOutput.TabIndex = 5
        Me.lblOutput.Text = "Save batch to:"
        '
        'tboOutput
        '
        Me.tboOutput.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.tboOutput.BackColor = System.Drawing.SystemColors.Window
        Me.tboOutput.Location = New System.Drawing.Point(89, 201)
        Me.tboOutput.Name = "tboOutput"
        Me.tboOutput.ReadOnly = true
        Me.tboOutput.Size = New System.Drawing.Size(188, 20)
        Me.tboOutput.TabIndex = 6
        '
        'comOutput
        '
        Me.comOutput.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.comOutput.ImageIndex = 0
        Me.comOutput.ImageList = Me.ilsEPForm
        Me.comOutput.Location = New System.Drawing.Point(277, 200)
        Me.comOutput.Name = "comOutput"
        Me.comOutput.Size = New System.Drawing.Size(23, 22)
        Me.comOutput.TabIndex = 7
        Me.comOutput.UseVisualStyleBackColor = true
        '
        'ilsEPForm
        '
        Me.ilsEPForm.ImageStream = CType(resources.GetObject("ilsEPForm.ImageStream"),System.Windows.Forms.ImageListStreamer)
        Me.ilsEPForm.TransparentColor = System.Drawing.Color.Fuchsia
        Me.ilsEPForm.Images.SetKeyName(0, "imgFolderOpen.bmp")
        '
        'lblBaseName
        '
        Me.lblBaseName.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.lblBaseName.AutoSize = true
        Me.lblBaseName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblBaseName.Location = New System.Drawing.Point(6, 231)
        Me.lblBaseName.Name = "lblBaseName"
        Me.lblBaseName.Size = New System.Drawing.Size(112, 13)
        Me.lblBaseName.TabIndex = 8
        Me.lblBaseName.Text = "Default file basename:"
        '
        'tboBaseName
        '
        Me.tboBaseName.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.tboBaseName.Location = New System.Drawing.Point(124, 228)
        Me.tboBaseName.Name = "tboBaseName"
        Me.tboBaseName.Size = New System.Drawing.Size(176, 20)
        Me.tboBaseName.TabIndex = 9
        '
        'lblVertEx
        '
        Me.lblVertEx.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.lblVertEx.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblVertEx.Location = New System.Drawing.Point(6, 262)
        Me.lblVertEx.Name = "lblVertEx"
        Me.lblVertEx.Size = New System.Drawing.Size(91, 35)
        Me.lblVertEx.TabIndex = 10
        Me.lblVertEx.Text = "Vertical exaggeration:"
        '
        'tboVertEx
        '
        Me.tboVertEx.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.tboVertEx.Location = New System.Drawing.Point(89, 266)
        Me.tboVertEx.Name = "tboVertEx"
        Me.tboVertEx.Size = New System.Drawing.Size(52, 20)
        Me.tboVertEx.TabIndex = 11
        '
        'comCancel
        '
        Me.comCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.comCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.comCancel.Location = New System.Drawing.Point(147, 264)
        Me.comCancel.Name = "comCancel"
        Me.comCancel.Size = New System.Drawing.Size(48, 23)
        Me.comCancel.TabIndex = 12
        Me.comCancel.Text = "Cancel"
        Me.comCancel.UseVisualStyleBackColor = true
        '
        'comGenProfiles
        '
        Me.comGenProfiles.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.comGenProfiles.Enabled = false
        Me.comGenProfiles.Location = New System.Drawing.Point(201, 264)
        Me.comGenProfiles.Name = "comGenProfiles"
        Me.comGenProfiles.Size = New System.Drawing.Size(99, 23)
        Me.comGenProfiles.TabIndex = 13
        Me.comGenProfiles.Text = "Generate Profiles"
        Me.comGenProfiles.UseVisualStyleBackColor = true
        '
        'flddlgEPBrowseFolders
        '
        Me.flddlgEPBrowseFolders.Description = "Choose a directory from the list."
        Me.flddlgEPBrowseFolders.RootFolder = System.Environment.SpecialFolder.MyComputer
        '
        'lboRasterLayers
        '
        Me.lboRasterLayers.FormattingEnabled = true
        Me.lboRasterLayers.Location = New System.Drawing.Point(86, 124)
        Me.lboRasterLayers.Name = "lboRasterLayers"
        Me.lboRasterLayers.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.lboRasterLayers.Size = New System.Drawing.Size(214, 43)
        Me.lboRasterLayers.TabIndex = 2
        '
        'frmDNRWATProfiler
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(312, 294)
        Me.Controls.Add(Me.lboRasterLayers)
        Me.Controls.Add(Me.comGenProfiles)
        Me.Controls.Add(Me.comCancel)
        Me.Controls.Add(Me.tboVertEx)
        Me.Controls.Add(Me.lblVertEx)
        Me.Controls.Add(Me.tboBaseName)
        Me.Controls.Add(Me.lblBaseName)
        Me.Controls.Add(Me.comOutput)
        Me.Controls.Add(Me.tboOutput)
        Me.Controls.Add(Me.lblOutput)
        Me.Controls.Add(Me.obuRaster)
        Me.Controls.Add(Me.obuSurface)
        Me.Controls.Add(Me.cboRasterLayer)
        Me.Controls.Add(Me.lblRasterLayer)
        Me.Controls.Add(Me.gboLayers)
        Me.Name = "frmDNRWATProfiler"
        Me.ShowIcon = false
        Me.Text = "Extract Profiles"
        Me.gboLayers.ResumeLayout(false)
        Me.gboLayers.PerformLayout
        Me.ResumeLayout(false)
        Me.PerformLayout

End Sub
    Friend WithEvents gboLayers As System.Windows.Forms.GroupBox
    Friend WithEvents cboXSecLabel As System.Windows.Forms.ComboBox
    Friend WithEvents lblXSecLabel As System.Windows.Forms.Label
    Friend WithEvents cboXSecLayer As System.Windows.Forms.ComboBox
    Friend WithEvents lblXSecLayer As System.Windows.Forms.Label
    Friend WithEvents lblRasterLayer As System.Windows.Forms.Label
    Friend WithEvents cboRasterLayer As System.Windows.Forms.ComboBox
    Friend WithEvents obuSurface As System.Windows.Forms.RadioButton
    Friend WithEvents obuRaster As System.Windows.Forms.RadioButton
    Friend WithEvents lblOutput As System.Windows.Forms.Label
    Friend WithEvents tboOutput As System.Windows.Forms.TextBox
    Friend WithEvents comOutput As System.Windows.Forms.Button
    Friend WithEvents lblBaseName As System.Windows.Forms.Label
    Friend WithEvents tboBaseName As System.Windows.Forms.TextBox
    Friend WithEvents lblVertEx As System.Windows.Forms.Label
    Friend WithEvents tboVertEx As System.Windows.Forms.TextBox
    Friend WithEvents comCancel As System.Windows.Forms.Button
    Friend WithEvents comGenProfiles As System.Windows.Forms.Button
    Friend WithEvents ilsEPForm As System.Windows.Forms.ImageList
    Friend WithEvents flddlgEPBrowseFolders As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents lboRasterLayers As System.Windows.Forms.ListBox
End Class
