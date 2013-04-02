<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDNRWATStickDiagrams
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDNRWATStickDiagrams))
        Me.gboLayer = New System.Windows.Forms.GroupBox
        Me.cboXSecNameField = New System.Windows.Forms.ComboBox
        Me.lblXSecNameField = New System.Windows.Forms.Label
        Me.cboXSecLyr = New System.Windows.Forms.ComboBox
        Me.lblXSecLyr = New System.Windows.Forms.Label
        Me.cboXSecField = New System.Windows.Forms.ComboBox
        Me.lblXSecField = New System.Windows.Forms.Label
        Me.cboYField = New System.Windows.Forms.ComboBox
        Me.lblY = New System.Windows.Forms.Label
        Me.cboXField = New System.Windows.Forms.ComboBox
        Me.lblX = New System.Windows.Forms.Label
        Me.cboWellLyr = New System.Windows.Forms.ComboBox
        Me.lblWellLyr = New System.Windows.Forms.Label
        Me.chkCreateLith = New System.Windows.Forms.CheckBox
        Me.chkCreateCon = New System.Windows.Forms.CheckBox
        Me.chkCreateSurf = New System.Windows.Forms.CheckBox
        Me.gboDBFiles = New System.Windows.Forms.GroupBox
        Me.comConDBOpen = New System.Windows.Forms.Button
        Me.ilsSDForm = New System.Windows.Forms.ImageList(Me.components)
        Me.tboInConDB = New System.Windows.Forms.TextBox
        Me.lblInConDB = New System.Windows.Forms.Label
        Me.comLithDBOpen = New System.Windows.Forms.Button
        Me.tboInLithDB = New System.Windows.Forms.TextBox
        Me.lblInLithDB = New System.Windows.Forms.Label
        Me.lblBatch = New System.Windows.Forms.Label
        Me.tboBatch = New System.Windows.Forms.TextBox
        Me.comBatch = New System.Windows.Forms.Button
        Me.lblVertEx = New System.Windows.Forms.Label
        Me.tboVertEx = New System.Windows.Forms.TextBox
        Me.comCancel = New System.Windows.Forms.Button
        Me.comCreateDiagrams = New System.Windows.Forms.Button
        Me.flddlgCSDBrowseFolders = New System.Windows.Forms.FolderBrowserDialog
        Me.gboLayer.SuspendLayout()
        Me.gboDBFiles.SuspendLayout()
        Me.SuspendLayout()
        '
        'gboLayer
        '
        Me.gboLayer.Controls.Add(Me.cboXSecNameField)
        Me.gboLayer.Controls.Add(Me.lblXSecNameField)
        Me.gboLayer.Controls.Add(Me.cboXSecLyr)
        Me.gboLayer.Controls.Add(Me.lblXSecLyr)
        Me.gboLayer.Controls.Add(Me.cboXSecField)
        Me.gboLayer.Controls.Add(Me.lblXSecField)
        Me.gboLayer.Controls.Add(Me.cboYField)
        Me.gboLayer.Controls.Add(Me.lblY)
        Me.gboLayer.Controls.Add(Me.cboXField)
        Me.gboLayer.Controls.Add(Me.lblX)
        Me.gboLayer.Controls.Add(Me.cboWellLyr)
        Me.gboLayer.Controls.Add(Me.lblWellLyr)
        Me.gboLayer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gboLayer.Location = New System.Drawing.Point(12, 12)
        Me.gboLayer.Name = "gboLayer"
        Me.gboLayer.Size = New System.Drawing.Size(398, 136)
        Me.gboLayer.TabIndex = 0
        Me.gboLayer.TabStop = False
        Me.gboLayer.Text = "Layer inputs"
        '
        'cboXSecNameField
        '
        Me.cboXSecNameField.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cboXSecNameField.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboXSecNameField.BackColor = System.Drawing.SystemColors.Control
        Me.cboXSecNameField.Enabled = False
        Me.cboXSecNameField.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboXSecNameField.FormattingEnabled = True
        Me.cboXSecNameField.Location = New System.Drawing.Point(304, 105)
        Me.cboXSecNameField.Name = "cboXSecNameField"
        Me.cboXSecNameField.Size = New System.Drawing.Size(86, 21)
        Me.cboXSecNameField.TabIndex = 11
        '
        'lblXSecNameField
        '
        Me.lblXSecNameField.AutoSize = True
        Me.lblXSecNameField.Enabled = False
        Me.lblXSecNameField.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblXSecNameField.Location = New System.Drawing.Point(203, 108)
        Me.lblXSecNameField.Name = "lblXSecNameField"
        Me.lblXSecNameField.Size = New System.Drawing.Size(95, 13)
        Me.lblXSecNameField.TabIndex = 10
        Me.lblXSecNameField.Text = "Cross section field:"
        '
        'cboXSecLyr
        '
        Me.cboXSecLyr.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cboXSecLyr.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboXSecLyr.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboXSecLyr.FormattingEnabled = True
        Me.cboXSecLyr.Location = New System.Drawing.Point(6, 105)
        Me.cboXSecLyr.Name = "cboXSecLyr"
        Me.cboXSecLyr.Size = New System.Drawing.Size(163, 21)
        Me.cboXSecLyr.TabIndex = 9
        '
        'lblXSecLyr
        '
        Me.lblXSecLyr.AutoSize = True
        Me.lblXSecLyr.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblXSecLyr.Location = New System.Drawing.Point(3, 89)
        Me.lblXSecLyr.Name = "lblXSecLyr"
        Me.lblXSecLyr.Size = New System.Drawing.Size(166, 13)
        Me.lblXSecLyr.TabIndex = 8
        Me.lblXSecLyr.Text = "Choose cross section layer (lines):"
        '
        'cboXSecField
        '
        Me.cboXSecField.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cboXSecField.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboXSecField.BackColor = System.Drawing.SystemColors.Control
        Me.cboXSecField.Enabled = False
        Me.cboXSecField.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboXSecField.FormattingEnabled = True
        Me.cboXSecField.Location = New System.Drawing.Point(304, 65)
        Me.cboXSecField.Name = "cboXSecField"
        Me.cboXSecField.Size = New System.Drawing.Size(86, 21)
        Me.cboXSecField.TabIndex = 7
        '
        'lblXSecField
        '
        Me.lblXSecField.AutoSize = True
        Me.lblXSecField.Enabled = False
        Me.lblXSecField.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblXSecField.Location = New System.Drawing.Point(203, 68)
        Me.lblXSecField.Name = "lblXSecField"
        Me.lblXSecField.Size = New System.Drawing.Size(95, 13)
        Me.lblXSecField.TabIndex = 6
        Me.lblXSecField.Text = "Cross section field:"
        '
        'cboYField
        '
        Me.cboYField.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cboYField.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboYField.BackColor = System.Drawing.SystemColors.Control
        Me.cboYField.Enabled = False
        Me.cboYField.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboYField.FormattingEnabled = True
        Me.cboYField.Location = New System.Drawing.Point(304, 40)
        Me.cboYField.Name = "cboYField"
        Me.cboYField.Size = New System.Drawing.Size(86, 21)
        Me.cboYField.TabIndex = 5
        '
        'lblY
        '
        Me.lblY.AutoSize = True
        Me.lblY.Enabled = False
        Me.lblY.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblY.Location = New System.Drawing.Point(228, 44)
        Me.lblY.Name = "lblY"
        Me.lblY.Size = New System.Drawing.Size(70, 13)
        Me.lblY.TabIndex = 4
        Me.lblY.Text = "Y coordinate:"
        '
        'cboXField
        '
        Me.cboXField.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cboXField.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboXField.BackColor = System.Drawing.SystemColors.Control
        Me.cboXField.Enabled = False
        Me.cboXField.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboXField.FormattingEnabled = True
        Me.cboXField.Location = New System.Drawing.Point(304, 15)
        Me.cboXField.Name = "cboXField"
        Me.cboXField.Size = New System.Drawing.Size(86, 21)
        Me.cboXField.TabIndex = 3
        '
        'lblX
        '
        Me.lblX.AutoSize = True
        Me.lblX.Enabled = False
        Me.lblX.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblX.Location = New System.Drawing.Point(228, 20)
        Me.lblX.Name = "lblX"
        Me.lblX.Size = New System.Drawing.Size(70, 13)
        Me.lblX.TabIndex = 2
        Me.lblX.Text = "X coordinate:"
        '
        'cboWellLyr
        '
        Me.cboWellLyr.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cboWellLyr.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboWellLyr.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboWellLyr.FormattingEnabled = True
        Me.cboWellLyr.Location = New System.Drawing.Point(6, 40)
        Me.cboWellLyr.Name = "cboWellLyr"
        Me.cboWellLyr.Size = New System.Drawing.Size(163, 21)
        Me.cboWellLyr.TabIndex = 1
        '
        'lblWellLyr
        '
        Me.lblWellLyr.AutoSize = True
        Me.lblWellLyr.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWellLyr.Location = New System.Drawing.Point(3, 24)
        Me.lblWellLyr.Name = "lblWellLyr"
        Me.lblWellLyr.Size = New System.Drawing.Size(129, 13)
        Me.lblWellLyr.TabIndex = 0
        Me.lblWellLyr.Text = "Choose well layer (points):"
        '
        'chkCreateLith
        '
        Me.chkCreateLith.AutoSize = True
        Me.chkCreateLith.Checked = True
        Me.chkCreateLith.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkCreateLith.Enabled = False
        Me.chkCreateLith.Location = New System.Drawing.Point(121, 154)
        Me.chkCreateLith.Name = "chkCreateLith"
        Me.chkCreateLith.Size = New System.Drawing.Size(189, 17)
        Me.chkCreateLith.TabIndex = 1
        Me.chkCreateLith.Text = "Create well lithology stick diagrams"
        Me.chkCreateLith.UseVisualStyleBackColor = True
        '
        'chkCreateCon
        '
        Me.chkCreateCon.AutoSize = True
        Me.chkCreateCon.Checked = True
        Me.chkCreateCon.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkCreateCon.Enabled = False
        Me.chkCreateCon.Location = New System.Drawing.Point(121, 177)
        Me.chkCreateCon.Name = "chkCreateCon"
        Me.chkCreateCon.Size = New System.Drawing.Size(209, 17)
        Me.chkCreateCon.TabIndex = 2
        Me.chkCreateCon.Text = "Create well construction stick diagrams"
        Me.chkCreateCon.UseVisualStyleBackColor = True
        '
        'chkCreateSurf
        '
        Me.chkCreateSurf.AutoSize = True
        Me.chkCreateSurf.Checked = True
        Me.chkCreateSurf.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkCreateSurf.Enabled = False
        Me.chkCreateSurf.Location = New System.Drawing.Point(121, 201)
        Me.chkCreateSurf.Name = "chkCreateSurf"
        Me.chkCreateSurf.Size = New System.Drawing.Size(147, 17)
        Me.chkCreateSurf.TabIndex = 3
        Me.chkCreateSurf.Text = "Create well surface points"
        Me.chkCreateSurf.UseVisualStyleBackColor = True
        '
        'gboDBFiles
        '
        Me.gboDBFiles.Controls.Add(Me.comConDBOpen)
        Me.gboDBFiles.Controls.Add(Me.tboInConDB)
        Me.gboDBFiles.Controls.Add(Me.lblInConDB)
        Me.gboDBFiles.Controls.Add(Me.comLithDBOpen)
        Me.gboDBFiles.Controls.Add(Me.tboInLithDB)
        Me.gboDBFiles.Controls.Add(Me.lblInLithDB)
        Me.gboDBFiles.Enabled = False
        Me.gboDBFiles.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gboDBFiles.Location = New System.Drawing.Point(13, 224)
        Me.gboDBFiles.Name = "gboDBFiles"
        Me.gboDBFiles.Size = New System.Drawing.Size(397, 71)
        Me.gboDBFiles.TabIndex = 4
        Me.gboDBFiles.TabStop = False
        Me.gboDBFiles.Text = "DB inputs"
        '
        'comConDBOpen
        '
        Me.comConDBOpen.Enabled = False
        Me.comConDBOpen.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.comConDBOpen.ImageIndex = 0
        Me.comConDBOpen.ImageList = Me.ilsSDForm
        Me.comConDBOpen.Location = New System.Drawing.Point(366, 41)
        Me.comConDBOpen.Name = "comConDBOpen"
        Me.comConDBOpen.Size = New System.Drawing.Size(25, 22)
        Me.comConDBOpen.TabIndex = 5
        Me.comConDBOpen.UseVisualStyleBackColor = True
        '
        'ilsSDForm
        '
        Me.ilsSDForm.ImageStream = CType(resources.GetObject("ilsSDForm.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ilsSDForm.TransparentColor = System.Drawing.Color.Fuchsia
        Me.ilsSDForm.Images.SetKeyName(0, "imgFolderOpen.bmp")
        '
        'tboInConDB
        '
        Me.tboInConDB.BackColor = System.Drawing.SystemColors.Control
        Me.tboInConDB.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tboInConDB.Location = New System.Drawing.Point(190, 42)
        Me.tboInConDB.Name = "tboInConDB"
        Me.tboInConDB.ReadOnly = True
        Me.tboInConDB.Size = New System.Drawing.Size(176, 20)
        Me.tboInConDB.TabIndex = 4
        '
        'lblInConDB
        '
        Me.lblInConDB.AutoSize = True
        Me.lblInConDB.Enabled = False
        Me.lblInConDB.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInConDB.Location = New System.Drawing.Point(6, 45)
        Me.lblInConDB.Name = "lblInConDB"
        Me.lblInConDB.Size = New System.Drawing.Size(186, 13)
        Me.lblInConDB.TabIndex = 3
        Me.lblInConDB.Text = "Input location for the construction DB:"
        '
        'comLithDBOpen
        '
        Me.comLithDBOpen.Enabled = False
        Me.comLithDBOpen.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.comLithDBOpen.ImageIndex = 0
        Me.comLithDBOpen.ImageList = Me.ilsSDForm
        Me.comLithDBOpen.Location = New System.Drawing.Point(366, 16)
        Me.comLithDBOpen.Name = "comLithDBOpen"
        Me.comLithDBOpen.Size = New System.Drawing.Size(25, 22)
        Me.comLithDBOpen.TabIndex = 2
        Me.comLithDBOpen.UseVisualStyleBackColor = True
        '
        'tboInLithDB
        '
        Me.tboInLithDB.BackColor = System.Drawing.SystemColors.Control
        Me.tboInLithDB.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tboInLithDB.Location = New System.Drawing.Point(173, 17)
        Me.tboInLithDB.Name = "tboInLithDB"
        Me.tboInLithDB.ReadOnly = True
        Me.tboInLithDB.Size = New System.Drawing.Size(193, 20)
        Me.tboInLithDB.TabIndex = 1
        '
        'lblInLithDB
        '
        Me.lblInLithDB.AutoSize = True
        Me.lblInLithDB.Enabled = False
        Me.lblInLithDB.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInLithDB.Location = New System.Drawing.Point(6, 20)
        Me.lblInLithDB.Name = "lblInLithDB"
        Me.lblInLithDB.Size = New System.Drawing.Size(166, 13)
        Me.lblInLithDB.TabIndex = 0
        Me.lblInLithDB.Text = "Input location for the lithology DB:"
        '
        'lblBatch
        '
        Me.lblBatch.AutoSize = True
        Me.lblBatch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBatch.Location = New System.Drawing.Point(10, 312)
        Me.lblBatch.Name = "lblBatch"
        Me.lblBatch.Size = New System.Drawing.Size(104, 13)
        Me.lblBatch.TabIndex = 5
        Me.lblBatch.Text = "Batch save location:"
        '
        'tboBatch
        '
        Me.tboBatch.BackColor = System.Drawing.SystemColors.Window
        Me.tboBatch.Location = New System.Drawing.Point(121, 309)
        Me.tboBatch.Name = "tboBatch"
        Me.tboBatch.ReadOnly = True
        Me.tboBatch.Size = New System.Drawing.Size(264, 20)
        Me.tboBatch.TabIndex = 6
        '
        'comBatch
        '
        Me.comBatch.ImageIndex = 0
        Me.comBatch.ImageList = Me.ilsSDForm
        Me.comBatch.Location = New System.Drawing.Point(385, 307)
        Me.comBatch.Name = "comBatch"
        Me.comBatch.Size = New System.Drawing.Size(25, 22)
        Me.comBatch.TabIndex = 7
        Me.comBatch.UseVisualStyleBackColor = True
        '
        'lblVertEx
        '
        Me.lblVertEx.AutoSize = True
        Me.lblVertEx.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVertEx.Location = New System.Drawing.Point(10, 345)
        Me.lblVertEx.Name = "lblVertEx"
        Me.lblVertEx.Size = New System.Drawing.Size(109, 13)
        Me.lblVertEx.TabIndex = 8
        Me.lblVertEx.Text = "Vertical exaggeration:"
        '
        'tboVertEx
        '
        Me.tboVertEx.Location = New System.Drawing.Point(121, 343)
        Me.tboVertEx.Name = "tboVertEx"
        Me.tboVertEx.Size = New System.Drawing.Size(70, 20)
        Me.tboVertEx.TabIndex = 9
        '
        'comCancel
        '
        Me.comCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.comCancel.Location = New System.Drawing.Point(229, 340)
        Me.comCancel.Name = "comCancel"
        Me.comCancel.Size = New System.Drawing.Size(75, 23)
        Me.comCancel.TabIndex = 10
        Me.comCancel.Text = "Cancel"
        Me.comCancel.UseVisualStyleBackColor = True
        '
        'comCreateDiagrams
        '
        Me.comCreateDiagrams.Enabled = False
        Me.comCreateDiagrams.Location = New System.Drawing.Point(310, 340)
        Me.comCreateDiagrams.Name = "comCreateDiagrams"
        Me.comCreateDiagrams.Size = New System.Drawing.Size(100, 23)
        Me.comCreateDiagrams.TabIndex = 11
        Me.comCreateDiagrams.Text = "Create Diagrams"
        Me.comCreateDiagrams.UseVisualStyleBackColor = True
        '
        'flddlgCSDBrowseFolders
        '
        Me.flddlgCSDBrowseFolders.Description = "Choose a directory from the list."
        Me.flddlgCSDBrowseFolders.RootFolder = System.Environment.SpecialFolder.MyComputer
        '
        'frmDNRWATStickDiagrams
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(422, 372)
        Me.Controls.Add(Me.comCreateDiagrams)
        Me.Controls.Add(Me.comCancel)
        Me.Controls.Add(Me.tboVertEx)
        Me.Controls.Add(Me.lblVertEx)
        Me.Controls.Add(Me.comBatch)
        Me.Controls.Add(Me.tboBatch)
        Me.Controls.Add(Me.lblBatch)
        Me.Controls.Add(Me.gboDBFiles)
        Me.Controls.Add(Me.chkCreateSurf)
        Me.Controls.Add(Me.chkCreateCon)
        Me.Controls.Add(Me.chkCreateLith)
        Me.Controls.Add(Me.gboLayer)
        Me.Name = "frmDNRWATStickDiagrams"
        Me.ShowIcon = False
        Me.Text = "Create Stick Diagrams"
        Me.gboLayer.ResumeLayout(False)
        Me.gboLayer.PerformLayout()
        Me.gboDBFiles.ResumeLayout(False)
        Me.gboDBFiles.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents gboLayer As System.Windows.Forms.GroupBox
    Friend WithEvents cboXSecNameField As System.Windows.Forms.ComboBox
    Friend WithEvents lblXSecNameField As System.Windows.Forms.Label
    Friend WithEvents cboXSecLyr As System.Windows.Forms.ComboBox
    Friend WithEvents lblXSecLyr As System.Windows.Forms.Label
    Friend WithEvents cboXSecField As System.Windows.Forms.ComboBox
    Friend WithEvents lblXSecField As System.Windows.Forms.Label
    Friend WithEvents cboYField As System.Windows.Forms.ComboBox
    Friend WithEvents lblY As System.Windows.Forms.Label
    Friend WithEvents cboXField As System.Windows.Forms.ComboBox
    Friend WithEvents lblX As System.Windows.Forms.Label
    Friend WithEvents cboWellLyr As System.Windows.Forms.ComboBox
    Friend WithEvents lblWellLyr As System.Windows.Forms.Label
    Friend WithEvents chkCreateLith As System.Windows.Forms.CheckBox
    Friend WithEvents chkCreateCon As System.Windows.Forms.CheckBox
    Friend WithEvents chkCreateSurf As System.Windows.Forms.CheckBox
    Friend WithEvents gboDBFiles As System.Windows.Forms.GroupBox
    Friend WithEvents comConDBOpen As System.Windows.Forms.Button
    Friend WithEvents tboInConDB As System.Windows.Forms.TextBox
    Friend WithEvents lblInConDB As System.Windows.Forms.Label
    Friend WithEvents comLithDBOpen As System.Windows.Forms.Button
    Friend WithEvents tboInLithDB As System.Windows.Forms.TextBox
    Friend WithEvents lblInLithDB As System.Windows.Forms.Label
    Friend WithEvents lblBatch As System.Windows.Forms.Label
    Friend WithEvents tboBatch As System.Windows.Forms.TextBox
    Friend WithEvents comBatch As System.Windows.Forms.Button
    Friend WithEvents lblVertEx As System.Windows.Forms.Label
    Friend WithEvents tboVertEx As System.Windows.Forms.TextBox
    Friend WithEvents comCancel As System.Windows.Forms.Button
    Friend WithEvents comCreateDiagrams As System.Windows.Forms.Button
    Friend WithEvents ilsSDForm As System.Windows.Forms.ImageList
    Friend WithEvents flddlgCSDBrowseFolders As System.Windows.Forms.FolderBrowserDialog
End Class
