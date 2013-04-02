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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDNRWATProfiler))
        Me.gboLayers = New System.Windows.Forms.GroupBox
        Me.cboXSecLabel = New System.Windows.Forms.ComboBox
        Me.lblXSecLabel = New System.Windows.Forms.Label
        Me.cboXSecLayer = New System.Windows.Forms.ComboBox
        Me.lblXSecLayer = New System.Windows.Forms.Label
        Me.lblRasterLayer = New System.Windows.Forms.Label
        Me.cboRasterLayer = New System.Windows.Forms.ComboBox
        Me.obuSurface = New System.Windows.Forms.RadioButton
        Me.obuRaster = New System.Windows.Forms.RadioButton
        Me.lblOutput = New System.Windows.Forms.Label
        Me.tboOutput = New System.Windows.Forms.TextBox
        Me.comOutput = New System.Windows.Forms.Button
        Me.ilsEPForm = New System.Windows.Forms.ImageList(Me.components)
        Me.lblBaseName = New System.Windows.Forms.Label
        Me.tboBaseName = New System.Windows.Forms.TextBox
        Me.lblVertEx = New System.Windows.Forms.Label
        Me.tboVertEx = New System.Windows.Forms.TextBox
        Me.comCancel = New System.Windows.Forms.Button
        Me.comGenProfiles = New System.Windows.Forms.Button
        Me.flddlgEPBrowseFolders = New System.Windows.Forms.FolderBrowserDialog
        Me.gboLayers.SuspendLayout()
        Me.SuspendLayout()
        '
        'gboLayers
        '
        Me.gboLayers.Controls.Add(Me.cboXSecLabel)
        Me.gboLayers.Controls.Add(Me.lblXSecLabel)
        Me.gboLayers.Controls.Add(Me.cboXSecLayer)
        Me.gboLayers.Controls.Add(Me.lblXSecLayer)
        Me.gboLayers.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gboLayers.Location = New System.Drawing.Point(12, 12)
        Me.gboLayers.Name = "gboLayers"
        Me.gboLayers.Size = New System.Drawing.Size(288, 67)
        Me.gboLayers.TabIndex = 0
        Me.gboLayers.TabStop = False
        Me.gboLayers.Text = "Input cross sections"
        '
        'cboXSecLabel
        '
        Me.cboXSecLabel.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cboXSecLabel.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboXSecLabel.BackColor = System.Drawing.SystemColors.Control
        Me.cboXSecLabel.Enabled = False
        Me.cboXSecLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboXSecLabel.FormattingEnabled = True
        Me.cboXSecLabel.Location = New System.Drawing.Point(179, 34)
        Me.cboXSecLabel.Name = "cboXSecLabel"
        Me.cboXSecLabel.Size = New System.Drawing.Size(103, 21)
        Me.cboXSecLabel.TabIndex = 3
        '
        'lblXSecLabel
        '
        Me.lblXSecLabel.AutoSize = True
        Me.lblXSecLabel.Enabled = False
        Me.lblXSecLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
        Me.cboXSecLayer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboXSecLayer.FormattingEnabled = True
        Me.cboXSecLayer.Location = New System.Drawing.Point(9, 34)
        Me.cboXSecLayer.Name = "cboXSecLayer"
        Me.cboXSecLayer.Size = New System.Drawing.Size(146, 21)
        Me.cboXSecLayer.TabIndex = 1
        '
        'lblXSecLayer
        '
        Me.lblXSecLayer.AutoSize = True
        Me.lblXSecLayer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblXSecLayer.Location = New System.Drawing.Point(6, 17)
        Me.lblXSecLayer.Name = "lblXSecLayer"
        Me.lblXSecLayer.Size = New System.Drawing.Size(136, 13)
        Me.lblXSecLayer.TabIndex = 0
        Me.lblXSecLayer.Text = "Choose cross section layer:"
        '
        'lblRasterLayer
        '
        Me.lblRasterLayer.AutoSize = True
        Me.lblRasterLayer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRasterLayer.Location = New System.Drawing.Point(6, 92)
        Me.lblRasterLayer.Name = "lblRasterLayer"
        Me.lblRasterLayer.Size = New System.Drawing.Size(63, 13)
        Me.lblRasterLayer.TabIndex = 1
        Me.lblRasterLayer.Text = "Input raster:"
        '
        'cboRasterLayer
        '
        Me.cboRasterLayer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cboRasterLayer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboRasterLayer.FormattingEnabled = True
        Me.cboRasterLayer.Location = New System.Drawing.Point(89, 89)
        Me.cboRasterLayer.Name = "cboRasterLayer"
        Me.cboRasterLayer.Size = New System.Drawing.Size(211, 21)
        Me.cboRasterLayer.TabIndex = 2
        '
        'obuSurface
        '
        Me.obuSurface.AutoSize = True
        Me.obuSurface.Checked = True
        Me.obuSurface.Enabled = False
        Me.obuSurface.Location = New System.Drawing.Point(17, 116)
        Me.obuSurface.Name = "obuSurface"
        Me.obuSurface.Size = New System.Drawing.Size(133, 17)
        Me.obuSurface.TabIndex = 3
        Me.obuSurface.TabStop = True
        Me.obuSurface.Text = "Use generated surface"
        Me.obuSurface.UseVisualStyleBackColor = True
        '
        'obuRaster
        '
        Me.obuRaster.AutoSize = True
        Me.obuRaster.Enabled = False
        Me.obuRaster.Location = New System.Drawing.Point(153, 116)
        Me.obuRaster.Name = "obuRaster"
        Me.obuRaster.Size = New System.Drawing.Size(148, 17)
        Me.obuRaster.TabIndex = 4
        Me.obuRaster.Text = "Use original raster (slower)"
        Me.obuRaster.UseVisualStyleBackColor = True
        '
        'lblOutput
        '
        Me.lblOutput.AutoSize = True
        Me.lblOutput.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOutput.Location = New System.Drawing.Point(6, 143)
        Me.lblOutput.Name = "lblOutput"
        Me.lblOutput.Size = New System.Drawing.Size(77, 13)
        Me.lblOutput.TabIndex = 5
        Me.lblOutput.Text = "Save batch to:"
        '
        'tboOutput
        '
        Me.tboOutput.BackColor = System.Drawing.SystemColors.Window
        Me.tboOutput.Location = New System.Drawing.Point(89, 140)
        Me.tboOutput.Name = "tboOutput"
        Me.tboOutput.ReadOnly = True
        Me.tboOutput.Size = New System.Drawing.Size(188, 20)
        Me.tboOutput.TabIndex = 6
        '
        'comOutput
        '
        Me.comOutput.ImageIndex = 0
        Me.comOutput.ImageList = Me.ilsEPForm
        Me.comOutput.Location = New System.Drawing.Point(277, 139)
        Me.comOutput.Name = "comOutput"
        Me.comOutput.Size = New System.Drawing.Size(23, 22)
        Me.comOutput.TabIndex = 7
        Me.comOutput.UseVisualStyleBackColor = True
        '
        'ilsEPForm
        '
        Me.ilsEPForm.ImageStream = CType(resources.GetObject("ilsEPForm.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ilsEPForm.TransparentColor = System.Drawing.Color.Fuchsia
        Me.ilsEPForm.Images.SetKeyName(0, "imgFolderOpen.bmp")
        '
        'lblBaseName
        '
        Me.lblBaseName.AutoSize = True
        Me.lblBaseName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBaseName.Location = New System.Drawing.Point(6, 170)
        Me.lblBaseName.Name = "lblBaseName"
        Me.lblBaseName.Size = New System.Drawing.Size(112, 13)
        Me.lblBaseName.TabIndex = 8
        Me.lblBaseName.Text = "Default file basename:"
        '
        'tboBaseName
        '
        Me.tboBaseName.Location = New System.Drawing.Point(124, 167)
        Me.tboBaseName.Name = "tboBaseName"
        Me.tboBaseName.Size = New System.Drawing.Size(176, 20)
        Me.tboBaseName.TabIndex = 9
        '
        'lblVertEx
        '
        Me.lblVertEx.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVertEx.Location = New System.Drawing.Point(6, 201)
        Me.lblVertEx.Name = "lblVertEx"
        Me.lblVertEx.Size = New System.Drawing.Size(91, 35)
        Me.lblVertEx.TabIndex = 10
        Me.lblVertEx.Text = "Vertical exaggeration:"
        '
        'tboVertEx
        '
        Me.tboVertEx.Location = New System.Drawing.Point(89, 205)
        Me.tboVertEx.Name = "tboVertEx"
        Me.tboVertEx.Size = New System.Drawing.Size(52, 20)
        Me.tboVertEx.TabIndex = 11
        '
        'comCancel
        '
        Me.comCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.comCancel.Location = New System.Drawing.Point(147, 203)
        Me.comCancel.Name = "comCancel"
        Me.comCancel.Size = New System.Drawing.Size(48, 23)
        Me.comCancel.TabIndex = 12
        Me.comCancel.Text = "Cancel"
        Me.comCancel.UseVisualStyleBackColor = True
        '
        'comGenProfiles
        '
        Me.comGenProfiles.Enabled = False
        Me.comGenProfiles.Location = New System.Drawing.Point(201, 203)
        Me.comGenProfiles.Name = "comGenProfiles"
        Me.comGenProfiles.Size = New System.Drawing.Size(99, 23)
        Me.comGenProfiles.TabIndex = 13
        Me.comGenProfiles.Text = "Generate Profiles"
        Me.comGenProfiles.UseVisualStyleBackColor = True
        '
        'flddlgEPBrowseFolders
        '
        Me.flddlgEPBrowseFolders.Description = "Choose a directory from the list."
        Me.flddlgEPBrowseFolders.RootFolder = System.Environment.SpecialFolder.MyComputer
        '
        'frmDNRWATProfiler
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(312, 237)
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
        Me.ShowIcon = False
        Me.Text = "Extract Profiles"
        Me.gboLayers.ResumeLayout(False)
        Me.gboLayers.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

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
End Class
