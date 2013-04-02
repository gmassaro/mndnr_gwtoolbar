Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.Catalog
Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.DataSourcesFile
Imports System.Windows.Forms
Imports System.Drawing

Public Class frmDNRWATStickDiagrams

  ' Name:     frmDNRWATStickDiagrams
  '
  ' Author:   Greg Massaro
  '           DNR Ecological and Water Resources
  '           500 Lafayette Road
  '           St. Paul, MN 55155
  '           greg.massaro@state.mn.us
  '           (651-259-5693)
  '
  '           Based on the ArcView 3.3 DNR.Cwilix_nxl and DNR.Cwicons, which
  '           was written in Avenue by Randy McGregor and Todd Petersen of
  '           MNDNR Waters. Rewritten for ArcGIS.
  '
  ' Date: Jun 12 2008
  ' Revised by: Greg Massaro
  ' Revision Date: Jan 25 2011
  ' Revisions: 
  ' -----------------------------------------------------------------------------
  ' Description:
  '           Contains the code for the user form which is displayed when
  '           basDNRWATGW.DNR_WAT_StickDiagrams runs.
  '
  '           Creates well Lithology/Construction stick diagrams and well
  '           surface points from a database file that contains appropriate
  '           CWI fields.  The user needs to have the directionality of all xsec
  '           lines already established by checking from and to nodes on each
  '           xsec.  Furthermore, xsec lines with one or more bends must have
  '           their segments already snapped to each other.  In addition, the
  '           user must have a field established in the well layer which
  '           indicates the label of the xsec(s) each well belongs to before
  '           running this code.
  '
  '
  ' Requires:
  ' Runs:     frmDNRWATProgressbar, basDNRWATGW
  ' Run by:   basDNRWATGW
  ' Returns:
  '==============================================================================
  '
  'This variable defines the input CWI point and Xsec shapefiles.
  Private pTHEWellLayer As ILayer
  Private pTHEXSecLayer As ILayer
  'This variable defines the input CWI point and Xsec shapefile's fields.
  Private pTHEWellFields As IFields
  Private pTHEXSecFields As IFields
  'This variable is the name of the field in the wells feature class that contains
  'the name of the cross section each well belongs to.
  Private strTHEXsecField As String
  'This variable keeps track of the well layer's data type (PGDB, shapefile, etc).
  Private strTHEWellDataType As String
  'These variable define the tables for the input DBs.
  Private pTHEInLithDBTable As ITable
  Private pTHEInConDBTable As ITable
  'This variable defines the path for the batch output location.
  Private strTHEBatchLoc As String
  'This variable stores the value for the vertical exaggeration.
  Private sglTheVertEx As Single
  'Used to exit this code if overwriting of files is not desired.
  Private blnTHEOverWrite As Boolean
  Private blnTHEAskAgain As Boolean
  'Used to skip to next cross section during an error.
  Private blnTHESkipXsec As Boolean
  'These are used to update the progressbar.
  Private lngTHENumXSec As Long
  Private lngTHEXSec As Long
  'These variables keep track of the progressbar value.
  Private sglTHEProgress As Single
  Private intTHEPhases As Integer

  Private Sub frmDNRWATStickDiagrams_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    tboVertEx.Text = 50
    g_blnRunning = False
  End Sub

  Private Sub cboWellLyr_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboWellLyr.SelectedIndexChanged
    Call ComboBoxLayerChange(cboXSecField, lblXSecField, cboWellLyr, cboXField, _
                              cboYField, lblX, lblY)
  End Sub

  Private Sub cboXSecLyr_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboXSecLyr.SelectedIndexChanged
    Call ComboBoxLayerChange(cboXSecNameField, lblXSecNameField, cboXSecLyr, Nothing, _
                              Nothing, Nothing, Nothing)
  End Sub

  Private Sub ComboBoxLayerChange(ByRef cboField As ComboBox, ByRef lblField As Label, ByRef cboLayer As ComboBox, _
          ByRef cboXField As ComboBox, ByRef cboYField As ComboBox, _
          ByRef lblX As Label, ByRef lblY As Label)
    On Error GoTo EH
    'Resets the field combobox to contain nothing.

    If cboLayer.Name = cboWellLyr.Name Then
      cboXField.Items.Clear()
      cboYField.Items.Clear()
    End If

    cboField.Items.Clear()
    'Exits the subroutine if the user does not select a layer.
    If cboLayer.Text = "" Then
      Exit Sub
    End If

    'Populates the Field combobox with actual fields from the
    '   selected layer.
    Dim pMap As IMap
    pMap = My.ArcMap.Document.FocusMap 'pMxDoc.FocusMap
    Dim pLayer As ILayer
    'These boolean flags will keep track if the user enters anything in the
    '   combobox that does not match with the layer names in the active data frame.
    '   As long as the combobox value does not match any existing layer, then
    '   the field combobox is disabled.
    Dim blnFlags As Boolean, blnFlag As Boolean
    blnFlags = True
    blnFlag = True
    'This loop checks to see if the current value in the layers combobox matches the
    '   name of a layer from the active data frame. If yes, then a public variable
    '   is set to the selected layer, and the flag is set to false.
    Dim i As Long
    For i = 0 To pMap.LayerCount - 1
      pLayer = pMap.Layer(i)
      If pLayer.Name = cboLayer.Text Then
        If cboLayer.Name = cboXSecLyr.Name Then
          pTHEXSecLayer = pLayer
        ElseIf cboLayer.Name = cboWellLyr.Name Then
          pTHEWellLayer = pLayer
        End If
        blnFlag = False
      End If
      blnFlags = blnFlags * blnFlag
    Next i
    'Exits the subroutine and disables the X and Y field comboboxes
    '  if the product of the flags is true.
    If blnFlags Then

      If cboLayer.Name = cboWellLyr.Name Then
        cboXField.Enabled = False
        cboYField.Enabled = False
        cboXField.Enabled = False

        lblX.Enabled = False
        lblY.Enabled = False
        cboXField.BackColor = SystemColors.Control
        cboYField.BackColor = SystemColors.Control

        chkCreateCon.Enabled = False
        chkCreateLith.Enabled = False
        chkCreateSurf.Enabled = False
        chkCreateCon.Checked = True
        chkCreateLith.Checked = True
        chkCreateSurf.Checked = True
        gboDBFiles.Enabled = False
        Call UpdateTheForm(False, tboInLithDB, lblInLithDB, comLithDBOpen)
        Call UpdateTheForm(False, tboInConDB, lblInConDB, comConDBOpen)
      End If

      cboField.Enabled = False
      lblField.Enabled = False
      cboField.BackColor = SystemColors.Control
      Exit Sub
    End If
    Dim pFlayer As IFeatureLayer

    If cboLayer.Name = cboXSecLyr.Name Then
      pFlayer = pTHEXSecLayer
    ElseIf cboLayer.Name = cboWellLyr.Name Then
      pFlayer = pTHEWellLayer
    End If

    Dim pFClass As IFeatureClass
    pFClass = pFlayer.FeatureClass
    'Sets a public variable to the fields in the selected layer.

    Dim pFields As IFields
    If cboLayer.Name = cboXSecLyr.Name Then
      pTHEXSecFields = pFClass.Fields
    ElseIf cboLayer.Name = cboWellLyr.Name Then
      pTHEWellFields = pFClass.Fields
    End If
    pFields = pFClass.Fields

    Dim pField As IField
    'Enables the field combobox and populates it with the names of fields
    '   from the active layer.

    If cboLayer.Name = cboWellLyr.Name Then
      cboXField.Enabled = True
      cboYField.Enabled = True
      lblX.Enabled = True
      lblY.Enabled = True
      cboXField.BackColor = SystemColors.Window
      cboYField.BackColor = SystemColors.Window
    End If

    cboField.Enabled = True
    lblField.Enabled = True
    cboField.BackColor = SystemColors.Window
    Dim f As Long
    For f = 0 To pFields.FieldCount - 1
      pField = pFields.Field(f)
      If cboLayer.Name = cboWellLyr.Name Then
        cboXField.Items.Add(pField.Name)
        cboYField.Items.Add(pField.Name)
      End If
      cboField.Items.Add(pField.Name)
    Next f
    If cboLayer.Name = cboWellLyr.Name Then
      chkCreateCon.Enabled = True
      chkCreateLith.Enabled = True
      chkCreateSurf.Enabled = True
      chkCreateCon.Checked = True
      chkCreateLith.Checked = True
      chkCreateSurf.Checked = True
      gboDBFiles.Enabled = True
      Call UpdateTheForm(True, tboInLithDB, lblInLithDB, comLithDBOpen)
      Call UpdateTheForm(True, tboInConDB, lblInConDB, comConDBOpen)
    End If
    Exit Sub

EH:
    MsgBox(Err.Number & "  " & Err.Description)
  End Sub

  Private Sub chkCreateLith_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCreateLith.CheckedChanged
    Dim blnEnabled As Boolean
    blnEnabled = chkCreateLith.Checked
    Call UpdateTheForm(blnEnabled, tboInLithDB, lblInLithDB, comLithDBOpen)
    Call CreateDiagramEnabler(tboInLithDB.Text, tboInConDB.Text, _
          tboBatch.Text)
  End Sub

  Private Sub chkCreateCon_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCreateCon.CheckedChanged
    Dim blnEnabled As Boolean
    blnEnabled = chkCreateCon.Checked
    Call UpdateTheForm(blnEnabled, tboInConDB, lblInConDB, comConDBOpen)
    Call CreateDiagramEnabler(tboInLithDB.Text, tboInConDB.Text, _
          tboBatch.Text)
  End Sub

  Private Sub chkCreateSurf_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCreateSurf.CheckedChanged
    Call CreateDiagramEnabler(tboInLithDB.Text, tboInConDB.Text, _
          tboBatch.Text)
  End Sub

  Private Sub comBatch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles comBatch.Click
    'This subroutine enables the user to select the output batch location from
    '  a user defined dialog.
    flddlgCSDBrowseFolders.ShowDialog()
    strTHEBatchLoc = flddlgCSDBrowseFolders.SelectedPath
    tboBatch.Text = strTHEBatchLoc
    Call CreateDiagramEnabler(tboInLithDB.Text, tboInConDB.Text, _
          tboBatch.Text)
  End Sub

  Private Sub comLithDBOpen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles comLithDBOpen.Click
    Dim strCaption As String
    strCaption = "Open CWI Lithology Database"
    pTHEInLithDBTable = GetTheDBTable(strCaption, tboInLithDB)
  End Sub

  Private Sub comConDBOpen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles comConDBOpen.Click
    Dim strCaption As String
    strCaption = "Open CWI Construction Database"
    pTHEInConDBTable = GetTheDBTable(strCaption, tboInConDB)
  End Sub

  Private Function GetTheDBTable(ByRef strCaption As String, ByRef tboInDB As TextBox) As ITable
    Dim pGxDialog As IGxDialog
    pGxDialog = New GxDialog
    Dim pGxObjFilter As IGxObjectFilter
    'Specifies Tables files as the filter (DBFs, TXTs, PGDB Tables, etc...).
    pGxObjFilter = New GxFilterTables
    Dim blnOK As Boolean
    Dim pEnumGxObject As IEnumGxObject
    Dim pGxObject As IGxObject
    Dim pTable As ITable
    Dim pName As IName
    With pGxDialog
      .Title = strCaption
      .ButtonCaption = "Open"
      .ObjectFilter = pGxObjFilter
      blnOK = .DoModalOpen(0, pEnumGxObject)
    End With
    If blnOK Then
      pGxObject = pEnumGxObject.Next
      'If the user selects nothing, then the textbox is reset to contain nothing.
      If pGxObject Is Nothing Then
        tboInDB.Text = ""
        Exit Function
      End If
      pName = pGxObject.InternalObjectName
      pTable = pName.Open
      GetTheDBTable = pTable
      tboInDB.Text = pGxObject.FullName
    End If
  End Function

  Private Sub tboInConDB_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tboInConDB.TextChanged
    Call CreateDiagramEnabler(tboInLithDB.Text, tboInConDB.Text, _
          tboBatch.Text)
  End Sub

  Private Sub tboInLithDB_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tboInLithDB.TextChanged
    Call CreateDiagramEnabler(tboInLithDB.Text, tboInConDB.Text, _
          tboBatch.Text)
  End Sub

  Private Sub tboVertEx_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tboVertEx.TextChanged
    'As long as the entered value is numeric, then the code checks to see if the
    '   input and output paths have already been assigned.  If yes, then the Create
    '   Diagram button is enabled.  Otherwise, it remains disabled.
    If IsNumeric(tboVertEx.Text) Then
      Call CreateDiagramEnabler(tboInLithDB.Text, tboInConDB.Text, _
          tboBatch.Text)
      sglTheVertEx = tboVertEx.Text
    Else : comCreateDiagrams.Enabled = False
    End If
  End Sub

  Private Sub UpdateTheForm(ByRef blnEnabled As Boolean, ByRef Tbo As TextBox, _
          ByRef Lbl As Label, ByRef Com As Button)
    'Disables or enables certain textboxes and buttons on the form
    'and changes their system colors.
    If blnEnabled Then
      Tbo.Enabled = True
      Tbo.BackColor = SystemColors.Window
      If Not Lbl Is Nothing Then
        Lbl.Enabled = True
      End If
      Com.Enabled = True
    Else
      Tbo.Text = ""
      Tbo.Enabled = False
      Tbo.BackColor = SystemColors.Control
      If Not Lbl Is Nothing Then
        Lbl.Enabled = False
      End If
      Com.Enabled = False
    End If
  End Sub

  Private Sub CreateDiagramEnabler(ByRef strLithDB As String, ByRef strConDB As String, _
          ByRef strBatchLoc As String)
    'This sub checks the conditions for all three checkboxes and enables the Create
    'diagram button if the conditions are met.

    Dim intLithVal As Integer, intConVal As Integer, _
          intSurfVal As Integer, intSum As Integer
    Select Case chkCreateLith.Checked
      Case True
        If (strLithDB = "" Or IsDBNull(strLithDB) Or strBatchLoc = "" _
                Or IsDBNull(strBatchLoc)) Then
          intLithVal = 1
        Else : intLithVal = 4
        End If
      Case False
        intLithVal = 0
    End Select

    Select Case chkCreateCon.Checked
      Case True
        If (strConDB = "" Or IsDBNull(strConDB) Or strBatchLoc = "" _
                Or IsDBNull(strBatchLoc)) Then
          intConVal = 1
        Else : intConVal = 4
        End If
      Case False
        intConVal = 0
    End Select

    Select Case chkCreateSurf.Checked
      Case True
        If strBatchLoc = "" Or IsDBNull(strBatchLoc) Then
          intSurfVal = 1
        Else : intSurfVal = 4
        End If
      Case False
        intSurfVal = 0
    End Select
    intSum = intLithVal + intConVal + intSurfVal

    If intSum = 0 Then
      comCreateDiagrams.Enabled = False
      Exit Sub
    ElseIf (intSum = 4 Or intSum = 8 Or intSum = 12) Then
      comCreateDiagrams.Enabled = True
      Exit Sub
    End If
    comCreateDiagrams.Enabled = False
  End Sub

  Private Sub comCreateDiagrams_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles comCreateDiagrams.Click
    On Error GoTo EH
    'If no layer is currently selected in the well layers combobox, then an error message
    '   is displayed and the sub is exited.
    If cboWellLyr.SelectedIndex = -1 Then
      MsgBox("Please select the CWI layer.", , "Select a Layer")
      Exit Sub
    End If
    'If no layer is currently selected in the xsec combobox, then an error message
    '   is displayed and the sub is exited.
    If cboXSecLyr.SelectedIndex = -1 Then
      MsgBox("Please select the cross section layer.", , "Select a Layer")
      Exit Sub
    End If
    'If no fields have been selected in either the X or Y field comboboxes, then an
    '   error message displays and the sub is exited.
    If cboXField.SelectedIndex = -1 Or cboYField.SelectedIndex = -1 Then
      MsgBox("Please select fields for" & vbCrLf & _
        "X and Y coordinates.", , "Select Fields")
      Exit Sub
    End If
    'If the same fields have been selected in the X and Y field comboboxes, then an
    '   error message displays and the sub is exited.
    If cboXField.SelectedIndex = cboYField.SelectedIndex Then
      MsgBox("The X and Y coordinate fields" & vbCrLf & "are the same.  " & _
              vbCrLf & vbCrLf & "Please select differing fields for" & vbCrLf & _
              "X and Y coordinates.", , "Select Fields")
      Exit Sub
    End If
    'If either the well or xsec layer variable contains nothing, then the sub is exited.
    If pTHEWellLayer Is Nothing Or pTHEXSecLayer Is Nothing Then
      Exit Sub
    End If
    'If no field has been selected in the Xsec CWI combobox, then an
    '   error message displays and the sub is exited.
    If cboXSecField.SelectedIndex = -1 Then
      MsgBox("Please select the field indicating which" & vbCrLf & _
        "cross section each well belongs to.", , "Select Field")
      Exit Sub
    End If
    'If no field has been selected in the Xsec Name combobox, then an
    '   error message displays and the sub is exited.
    If cboXSecNameField.SelectedIndex = -1 Then
      MsgBox("Please select the field containing" & vbCrLf & _
        "the label for each cross section.", , "Select Field")
      Exit Sub
    End If

    Dim pWellFLayer As IFeatureLayer, pXSecFLayer As IFeatureLayer
    pWellFLayer = pTHEWellLayer
    strTHEWellDataType = pWellFLayer.DataSourceType
    pXSecFLayer = pTHEXSecLayer
    Dim pWellFClass As IFeatureClass, pXSecFClass As IFeatureClass
    pWellFClass = pWellFLayer.FeatureClass
    pXSecFClass = pXSecFLayer.FeatureClass
    'If the CWI layer is not a point shapefile, then an error message is displayed
    '   and the code exits.
    If Not (pWellFClass.ShapeType = esriGeometryType.esriGeometryPoint) Then
      MsgBox("Please select a point shapefile for the CWI layer.", , _
              "Well Layer")
      Exit Sub
    End If

    'If the XSec layer is not a line shapefile, then an error message is displayed
    '   and the code exits.
    If Not (pXSecFClass.ShapeType = esriGeometryType.esriGeometryPolyline) Then
      MsgBox("Please select a line shapefile for the cross section layer.", , _
              "Cross Section Layer")
      Exit Sub
    End If

    Dim pFSelection As IFeatureSelection
    pFSelection = pWellFLayer
    Dim pField As IField
    Dim pXField As IField, pYField As IField, pXSecField As IField
    Dim pXSecNameField As IField
    'Sets the X,Y,XSec, and XSec Name field variables to the selected fields.
    Dim f As Long
    For f = 0 To pTHEWellFields.FieldCount - 1
      pField = pTHEWellFields.Field(f)
      If cboXField.Text = pField.Name Then
        pXField = pField
      End If
      If cboYField.Text = pField.Name Then
        pYField = pField
      End If
      If cboXSecField.Text = pField.Name Then
        pXSecField = pField
        strTHEXsecField = pField.Name
      End If
    Next f
    For f = 0 To pTHEXSecFields.FieldCount - 1
      pField = pTHEXSecFields.Field(f)
      If cboXSecNameField.Text = pField.Name Then
        pXSecNameField = pField
      End If
    Next f

    'If the selected fields in either the X or Y field comboboxes are not of the
    '   following types {smallinteger, integer, single, or double}, then
    '   an error message displays and the sub is exited.
    If pXField.Type > 3 Or pYField.Type > 3 Then
      MsgBox("Either the X or Y coordinate field is not numeric.  " & _
              vbCrLf & vbCrLf & "Please select fields for the X and Y coordinates" & _
              vbCrLf & "which contain numeric values (in UTM meters).", , "Select Fields")
      Exit Sub
    End If
    'If the selected fields in either the XSec or XsecName field comboboxes are not of
    '   the string type, then an error message displays and the sub is exited.
    If pXSecField.Type <> 4 Or pXSecNameField.Type <> 4 Then
      MsgBox("Either the XSec or XSec Name field is not a string.  " & _
              vbCrLf & vbCrLf & "Please select fields for the XSec and Xsec Name" & _
              vbCrLf & "which contain string values.", , "Select Fields")
      Exit Sub
    End If

    Me.Hide()
    Application.DoEvents()

    Dim pfrmProgress As frmDNRWATProgressbar
    pfrmProgress = New frmDNRWATProgressbar

    g_blnRunning = True

    With pfrmProgress
      .pbaProcess.Maximum = 101
      .Text = "Generating Stick Diagrams"
      .lblProcess.Text = "Cross section: "
      sglTHEProgress = 0
      .Show()
      Call basDNRWATGW.UpdateProgress(sglTHEProgress, pfrmProgress)
    End With

    Dim pWkSpFactory As IWorkspaceFactory
    pWkSpFactory = New ShapefileWorkspaceFactory

    Dim strTempFileName As String, strTempFolder As String
    strTempFolder = System.IO.Path.GetTempPath

    Dim pTempWkSpace As IWorkspace
    pTempWkSpace = pWkSpFactory.OpenFromFile(strTempFolder, 0)
    Dim pTempSortTable As ITable

    Dim pXSecFSel As IFeatureSelection
    Dim pXSecSelSet As ISelectionSet
    Dim pXSecFCursor As IFeatureCursor

    pXSecFSel = pXSecFLayer
    pXSecSelSet = pXSecFSel.SelectionSet

    If pXSecSelSet.Count = 0 Then
      pXSecFCursor = pXSecFClass.Search(Nothing, True)
      lngTHENumXSec = pXSecFClass.FeatureCount(Nothing)
    Else
      pXSecSelSet.Search(Nothing, True, pXSecFCursor)
      lngTHENumXSec = pXSecSelSet.Count
    End If

    'Determines the number of phases that the program will run (7 total is possible
    'for each xsec line: creating a temporary table, creating the sorted table, and
    'creating the shapefiles.
    intTHEPhases = 0
    If chkCreateLith.Checked Then
      intTHEPhases = intTHEPhases + 3
    End If
    If chkCreateCon.Checked Then
      intTHEPhases = intTHEPhases + 3
    End If
    If chkCreateSurf.Checked Then
      intTHEPhases = intTHEPhases + 1
    End If
    intTHEPhases = intTHEPhases * lngTHENumXSec

    Dim pXSecFeature As IFeature
    Dim strLabel As String
    Dim pGeometry As IGeometry
    'This array contains all transformation variables for rotation and translating points.
    Dim dblTransArray(,) As Double
    'This array will contain the coordinates for all vertices and nodes on the xsec line.
    Dim dblVertexCoords(,) As Double
    Dim intNumPoints As Integer
    Dim pWellsSelSet As ISelectionSet
    Dim strName As String, strShapeFileName As String
    Dim pManyTable As ITable
    Dim GeoType As esriGeometryType
    Dim strTopField As String, strBotField As String

    'Loop through each cross section in the Xsec layer and generate appropriate files.
    pXSecFeature = pXSecFCursor.NextFeature
    lngTHEXSec = 1
    blnTHEOverWrite = True
    blnTHEAskAgain = True
    Do Until pXSecFeature Is Nothing
      strLabel = pXSecFeature.Value(pXSecFCursor.FindField(pXSecNameField.Name))
      pGeometry = pXSecFeature.ShapeCopy
      Dim pPointCollection As IPointCollection
      Dim pSegmentCollection As ISegmentCollection
      If TypeOf pGeometry Is IPointCollection Then
        pPointCollection = pGeometry
        pSegmentCollection = pGeometry
      Else
        MsgBox("Error getting point collection for cross section line.", , "Error")
        Exit Sub
      End If

      'If pGeometry is a multipart line, ask the user if wish to continue...
      'continuing may result in errors... skip to next xsec line if no...
      If pPointCollection.PointCount > pSegmentCollection.SegmentCount + 1 Then
        If MsgBox("Cross section " & UCase(strLabel) & " is a Multipart Line." & _
          vbCrLf & "Continuing this process may lead to errors in the location of" & _
          vbCrLf & "wells in the final shapefile." & vbCrLf & vbCrLf & _
          "Continue with the generation of shapefiles for cross section " & _
          UCase(strLabel) & "?" & vbCrLf & "(selecting 'No' will skip this cross " & _
          "section only)", vbYesNo, "Multipart Line Detected") = vbNo Then

          Dim sglStepUp As Single
          sglStepUp = 100 / lngTHENumXSec
          'Update the progress bar by one step increase.
          sglTHEProgress = sglTHEProgress + sglStepUp
          Call basDNRWATGW.UpdateProgress(sglTHEProgress, pfrmProgress)
          GoTo NextXSec
        End If
      End If

      Call CreateTransMatrix(dblTransArray, dblVertexCoords, intNumPoints, pPointCollection)

      'Generate lithology, construction, and surface point shapefiles (depending on
      'the check boxes on the form).
      Dim i As Integer
      Dim blnCreateIt As Boolean
      blnCreateIt = False
      blnTHESkipXsec = False
      For i = 1 To 3
        If i = 1 And chkCreateLith.Checked Then
          blnCreateIt = True
          strTempFileName = "lixtemp"
          strName = "x" & Int(sglTheVertEx) & "_" & UCase(basDNRWATGW.StripString(strLabel)) _
                      & "_lixpy"
          strShapeFileName = "lixpy"
          pManyTable = pTHEInLithDBTable
          GeoType = esriGeometryType.esriGeometryPolygon
          strTopField = "stratopel"
          strBotField = "drilbotel"
        ElseIf i = 2 And chkCreateCon.Checked Then
          blnCreateIt = True
          strTempFileName = "contemp"
          strName = "x" & Int(sglTheVertEx) & "_" & UCase(basDNRWATGW.StripString(strLabel)) _
                      & "_conpy"
          strShapeFileName = "conpy"
          pManyTable = pTHEInConDBTable
          GeoType = esriGeometryType.esriGeometryPolygon
          strTopField = "constopel"
          strBotField = "compbotel"
        ElseIf i = 3 And chkCreateSurf.Checked Then
          blnCreateIt = True
          strName = "x" & Int(sglTheVertEx) & "_" & UCase(basDNRWATGW.StripString(strLabel)) _
                      & "_surfpt"
          strShapeFileName = "surfpt"
          GeoType = esriGeometryType.esriGeometryPoint
          strTopField = ""
          strBotField = ""
        End If

        'Create Temp Tables
        If i <> 3 And blnCreateIt And Not (blnTHESkipXsec) Then
          Call CreateTempTables(dblTransArray, dblVertexCoords, intNumPoints, strLabel, _
            pTempWkSpace, strTempFileName, strTempFolder, pManyTable, pWellFClass, _
            pXField, pYField, pTempSortTable, pfrmProgress)
        End If

        'Create Shapefiles
        If blnCreateIt And Not (blnTHESkipXsec) Then
          Call CreateShapefiles(dblTransArray, dblVertexCoords, intNumPoints, strLabel, _
            strTHEBatchLoc, strName, GeoType, sglTheVertEx, pWellFClass, strTopField, _
            strBotField, strShapeFileName, pXField, pYField, pTempSortTable, _
            pfrmProgress)
        End If
        pTempSortTable = Nothing
        blnCreateIt = False
      Next i
NextXSec:
      'Exit this code if the user does not wish to overwrite shapefiles.
      If blnTHEOverWrite = False Then Exit Do

      pXSecFeature = pXSecFCursor.NextFeature
      lngTHEXSec = lngTHEXSec + 1
    Loop

    Erase dblTransArray
    Erase dblVertexCoords
    pXSecFCursor = Nothing

    'Delete the temporary files used.
    If System.IO.File.Exists(strTempFolder & "\lixtemp.dbf") Then
      System.IO.File.Delete(strTempFolder & "\lixtemp.dbf")
    End If
    If System.IO.File.Exists(strTempFolder & "\lixtempsort.dbf") Then
      System.IO.File.Delete(strTempFolder & "\lixtempsort.dbf")
    End If
    If System.IO.File.Exists(strTempFolder & "\contemp.dbf") Then
      System.IO.File.Delete(strTempFolder & "\contemp.dbf")
    End If
    If System.IO.File.Exists(strTempFolder & "\contempsort.dbf") Then
      System.IO.File.Delete(strTempFolder & "\contempsort.dbf")
    End If

    g_blnRunning = False
    pfrmProgress.Close()
    pfrmProgress = Nothing

    'reset module level variables
    pTHEWellLayer = Nothing
    pTHEXSecLayer = Nothing
    pTHEWellFields = Nothing
    pTHEXSecFields = Nothing
    pTHEInLithDBTable = Nothing
    pTHEInConDBTable = Nothing

    Exit Sub
EH:
    pfrmProgress.Dispose()
    pfrmProgress = Nothing

    MsgBox(Err.Number & "  " & Err.Description & vbCrLf & vbCrLf & _
          "(This error may have been caused by" & vbCrLf & _
          "a problem with one of the input DBs)")
  End Sub

  Private Sub CreateTempTables(ByRef dblTransArray(,) As Double, ByRef dblVertexCoords(,) As Double, _
          ByRef intNumPoints As Integer, ByRef strLabel As String, ByRef pTempWkSpace As IWorkspace, _
          ByRef strTempFileName As String, ByRef strTempFolder As String, ByRef pManyTable As ITable, _
          ByRef pFClass As IFeatureClass, ByRef pXField As IField, ByRef pYField As IField, _
          ByRef pTempSortTable As ITable, ByRef pfrmProgress As frmDNRWATProgressbar)

    If pManyTable Is Nothing Then
      MsgBox("Error finding " & strTempFileName.Substring(0, 3) & " DB. Skipping...", , _
              "DB Is Nothing")
      Exit Sub
    End If
    Dim pFields As IFields
    Dim pTempFWkSpace As IFeatureWorkspace
    pFields = New Fields
    pTempFWkSpace = pTempWkSpace

    'Create the non-sorted temporary table in the user's system TEMP directory.
    pFields = FieldsToBeAdded(pFields, strTempFileName)
    If System.IO.File.Exists(strTempFolder & "\" & strTempFileName & ".dbf") Then
      Debug.Print("temp filename already exists...must delete to continue")
      Debug.Print("deleting " & strTempFolder & "\" & strTempFileName & ".dbf")
      System.IO.File.Delete(strTempFolder & "\" & strTempFileName & ".dbf")
    End If

    Dim pOneTable As ITable
    pOneTable = pFClass
    Dim pTempTable As ITable

    pTempTable = pTempFWkSpace.CreateTable(strTempFileName, pFields, Nothing, Nothing, "")

    ' Create virtual relate
    Dim pMemRelCFact As IMemoryRelationshipClassFactory
    pMemRelCFact = New MemoryRelationshipClassFactory
    Dim pRelClass As IRelationshipClass
    pRelClass = pMemRelCFact.Open("TabletoLayer", pOneTable, "Relateid", pManyTable, _
          "Relateid", "forward", "backward", esriRelCardinality.esriRelCardinalityOneToMany)

    Dim pRelatedSelSet As ISelectionSet
    Dim pSelectionSet As ISelectionSet


    'Here's where we get the selection of wells for each cross section
    Dim pQueryFilter As IQueryFilter
    pQueryFilter = GetQueryFilter(strTHEXsecField, strLabel)
    pSelectionSet = pOneTable.Select(pQueryFilter, esriSelectionType.esriSelectionTypeHybrid, _
          esriSelectionOption.esriSelectionOptionNormal, pTempWkSpace)

    If pSelectionSet.Count = 0 Then
      MsgBox("No wells match the label from cross section " & strLabel & "." & vbCrLf & _
              "Skipping to next process...", , _
              "No associated wells")
      blnTHESkipXsec = True
      Exit Sub
    End If

    pRelatedSelSet = GetRelSelection(pSelectionSet, pManyTable, _
          pRelClass, pTempWkSpace)
    Dim pOneFCursor As IFeatureCursor, pManyCursor As ICursor, pTempInsCursor As ICursor
    pOneFCursor = pFClass.Search(pQueryFilter, True)
    pTempInsCursor = pTempTable.Insert(True)
    Dim strOneID As String, strManyID As String
    Dim pFeature As IFeature
    pFeature = pOneFCursor.NextFeature
    Dim pRow As IRow, pRowBuffer As IRowBuffer
    Dim dblXTrans As Double, dblYTrans As Double
    Dim vElevation
    Dim strStrat As String, strPrimlith As String, strSeclith As String, strColor As String
    Dim strPsclab As String, strConstype As String, strSortX As String, strSortRow As String
    Dim strDeptop As String, dblSorter As Double, intRowNum As Integer
    intRowNum = 1

    'Sets up a variable to be used with the progressbar value.
    pfrmProgress.lblProcess.Text = "Cross section: " & UCase(strLabel) & _
                      " (" & lngTHEXSec & " of " & lngTHENumXSec & ")"
    Dim sglStepUp As Single
    sglStepUp = 100 / intTHEPhases / pSelectionSet.Count

    Do Until pFeature Is Nothing
      pRelatedSelSet.Search(Nothing, False, pManyCursor)
      pRow = pManyCursor.NextRow
      Do Until pRow Is Nothing
        strOneID = pFeature.Value(pOneFCursor.FindField("Relateid"))
        strManyID = pRow.Value(pManyCursor.FindField("Relateid"))
        If UCase(strOneID) = UCase(strManyID) Then
          '_________________________________________________________
          '   CWI INDEX DATABASE (One record per well)
          '_________________________________________________________
          '
          pRowBuffer = pTempTable.CreateRowBuffer
          pRowBuffer.Value(pTempInsCursor.FindField("unnum")) = _
                  pFeature.Value(pOneFCursor.FindField("Unique_no"))
          pRowBuffer.Value(pTempInsCursor.FindField("unsernum")) = "1"

          Call XYTransRotate(dblTransArray, dblXTrans, dblYTrans, pFeature, pOneFCursor, _
                              pXField, pYField, intNumPoints, dblVertexCoords, False)
          pRowBuffer.Value(pTempInsCursor.FindField("xfield")) = dblXTrans
          pRowBuffer.Value(pTempInsCursor.FindField("yfield")) = dblYTrans
          vElevation = pFeature.Value(pOneFCursor.FindField("Elevation"))
          Select Case strTempFileName
            Case "lixtemp"
              If pOneFCursor.FindField("Depth_drll") <> -1 Then
                pRowBuffer.Value(pTempInsCursor.FindField("drilbotel")) = _
                  vElevation - pFeature.Value(pOneFCursor.FindField("Depth_drll"))
              ElseIf pOneFCursor.FindField("Depth_dril") <> -1 Then
                pRowBuffer.Value(pTempInsCursor.FindField("drilbotel")) = _
                  vElevation - pFeature.Value(pOneFCursor.FindField("Depth_dril"))
              End If
            Case "contemp"
              pRowBuffer.Value(pTempInsCursor.FindField("compbotel")) = _
                vElevation - pFeature.Value(pOneFCursor.FindField("Depth_comp"))
          End Select
          '_________________________________________________________
          '           CWI STRATIGRAPHY DATABASE or
          '           CWI CONSTRUCTION DATABASE
          '             (Many records per well)
          '_________________________________________________________
          Select Case strTempFileName
            Case "lixtemp"
              pRowBuffer.Value(pTempInsCursor.FindField("stratopel")) = _
                vElevation - pRow.Value(pManyCursor.FindField("Depth_top"))
              strStrat = pRow.Value(pManyCursor.FindField("Strat"))
              If strStrat = " " Or strStrat = "" Or IsDBNull(strStrat) Then
                strStrat = "--"
              End If
              pRowBuffer.Value(pTempInsCursor.FindField("strat")) = strStrat
              strPrimlith = pRow.Value(pManyCursor.FindField("Lith_prim"))
              If strPrimlith = " " Or strPrimlith = "" Or IsDBNull(strPrimlith) Then
                strPrimlith = "--"
              End If
              pRowBuffer.Value(pTempInsCursor.FindField("primlith")) = strPrimlith
              strSeclith = pRow.Value(pManyCursor.FindField("Lith_sec"))
              If strSeclith = " " Or strSeclith = "" Or IsDBNull(strSeclith) Then
                strSeclith = "--"
              End If
              pRowBuffer.Value(pTempInsCursor.FindField("seclith")) = strSeclith
              strColor = pRow.Value(pManyCursor.FindField("Color"))
              If strColor = " " Or strColor = "" Or IsDBNull(strColor) Then
                strColor = "--"
              End If
              pRowBuffer.Value(pTempInsCursor.FindField("color")) = strColor
              strPsclab = strPrimlith & " " & strSeclith & " " & strColor
              pRowBuffer.Value(pTempInsCursor.FindField("psclab")) = strPsclab
              pRowBuffer.Value(pTempInsCursor.FindField("utmx")) = _
                  pFeature.Value(pOneFCursor.FindField(pXField.Name))
              pRowBuffer.Value(pTempInsCursor.FindField("utmy")) = _
                  pFeature.Value(pOneFCursor.FindField(pYField.Name))

            Case "contemp"

              pRowBuffer.Value(pTempInsCursor.FindField("constopel")) = _
                vElevation - pRow.Value(pManyCursor.FindField("From_depth"))
              strConstype = pRow.Value(pManyCursor.FindField("Constype"))
              If strConstype = " " Or strConstype = "" Or IsDBNull(strConstype) Then
                strConstype = "--"
              End If
              pRowBuffer.Value(pTempInsCursor.FindField("constype")) = strConstype

          End Select

          'The next section begins the calculations for the sorter field
          strSortX = Math.Round(dblXTrans)
          strSortRow = intRowNum
          If strSortRow < 10 Then
            strSortRow = "000" & strSortRow
          ElseIf strSortRow < 100 Then
            strSortRow = "00" & strSortRow
          ElseIf strSortRow < 1000 Then
            strSortRow = "0" & strSortRow
          End If
          Select Case strTempFileName
            Case "lixtemp"
              strDeptop = pRow.Value(pManyCursor.FindField("Depth_top"))
            Case "contemp"
              strDeptop = pRow.Value(pManyCursor.FindField("From_depth"))
          End Select
          If strDeptop = 0 Then
            strDeptop = "000"
          ElseIf strDeptop < 10 Then
            strDeptop = "00" & strDeptop
          ElseIf strDeptop < 100 Then
            strDeptop = "0" & strDeptop
          End If
          dblSorter = (strSortX & strSortRow & strDeptop) / 1000
          pRowBuffer.Value(pTempInsCursor.FindField("sorter")) = dblSorter
          pTempInsCursor.InsertRow(pRowBuffer)
        End If
        pRow = pManyCursor.NextRow
      Loop
      pManyCursor = Nothing
      pFeature = pOneFCursor.NextFeature
      intRowNum = intRowNum + 1

      'Update the progress bar by one step increase.
      sglTHEProgress = sglTHEProgress + sglStepUp
      Call basDNRWATGW.UpdateProgress(sglTHEProgress, pfrmProgress)
    Loop
    pTempInsCursor.Flush()
    'VB.Net may have trouble with releasing the cursor if so, do not use
    'pTempInsCursor = Nothing
    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(pTempInsCursor)

    pOneFCursor = Nothing

    If pTempTable.RowCount(Nothing) = 0 Then
      MsgBox("No related records in the input DB." & vbCrLf & _
        "Skipping to next process...", , "No related wells")
      Exit Sub
    End If

    'Updates the step increment.
    sglStepUp = 100 / intTHEPhases / pTempTable.RowCount(Nothing)

    'Create a temporary sorted table from the temporary table.
    strTempFileName = strTempFileName & "sort"
    If System.IO.File.Exists(strTempFolder & "\" & strTempFileName & ".dbf") Then
      System.IO.File.Delete(strTempFolder & "\" & strTempFileName & ".dbf")
    End If
    pTempSortTable = pTempFWkSpace.CreateTable(strTempFileName, pTempTable.Fields, _
          Nothing, Nothing, "")
    Dim pSortedCursor As ICursor
    Dim pTableSort As ITableSort
    pTableSort = New TableSort
    pTableSort.Table = pTempTable
    pTableSort.Fields = "sorter"
    pTableSort.Ascending("sorter") = True
    pTableSort.Sort(Nothing)
    pSortedCursor = pTableSort.Rows
    Dim pTempSortInsCursor As ICursor
    pTempSortInsCursor = pTempSortTable.Insert(True)
    pRow = pSortedCursor.NextRow
    Dim intSernum As Integer, strPrevunnum As String, strSernum As String, _
          intUnnumField As Integer, strUnsernum As String
    intSernum = 1
    strPrevunnum = ""
    intUnnumField = pTempSortInsCursor.FindField("unnum")
    Dim f As Long
    Do Until pRow Is Nothing
      pRowBuffer = pTempSortTable.CreateRowBuffer
      For f = 0 To pSortedCursor.Fields.FieldCount - 1
        If pRow.Fields.Field(f).Editable Then
          If IsDBNull(pRow.Value(f)) Then
            pRowBuffer.Value(f) = DBNull.Value
          Else
            pRowBuffer.Value(f) = pRow.Value(f)
          End If
        End If
      Next f
      If pRowBuffer.Value(intUnnumField) = strPrevunnum Then
        intSernum = intSernum + 1
        strSernum = "0" & intSernum
      Else
        intSernum = 1
        strSernum = "01"
      End If
      strPrevunnum = pRowBuffer.Value(intUnnumField)
      strUnsernum = pRowBuffer.Value(intUnnumField) & strSernum
      pRowBuffer.Value(pTempSortInsCursor.FindField("unsernum")) = strUnsernum
      pTempSortInsCursor.InsertRow(pRowBuffer)
      pRow = pSortedCursor.NextRow

      'Update the progressbar by one step increase.
      sglTHEProgress = sglTHEProgress + sglStepUp
      Call basDNRWATGW.UpdateProgress(sglTHEProgress, pfrmProgress)
    Loop
    pTempSortInsCursor.Flush()
    pSortedCursor = Nothing
    'VB.Net may have trouble with releasing the cursor if so, do not use
    'pTempSortInsCursor = Nothing
    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(pTempSortInsCursor)

  End Sub

  Private Function FieldsToBeAdded(ByRef pFields As IFields, ByRef strTempFileName As String) As IFields
    Dim pFieldsEdit As IFieldsEdit
    pFieldsEdit = pFields
    pFieldsEdit.AddField(basDNRWATGW.MakeField("UNNUM", esriFieldType.esriFieldTypeString))
    pFieldsEdit.AddField(basDNRWATGW.MakeField("UNSERNUM", esriFieldType.esriFieldTypeString))
    pFieldsEdit.AddField(basDNRWATGW.MakeField("XFIELD", esriFieldType.esriFieldTypeDouble))
    pFieldsEdit.AddField(basDNRWATGW.MakeField("YFIELD", esriFieldType.esriFieldTypeDouble))

    Select Case strTempFileName
      Case "lixtemp"
        pFieldsEdit.AddField(basDNRWATGW.MakeField("STRATOPEL", esriFieldType.esriFieldTypeInteger))
        pFieldsEdit.AddField(basDNRWATGW.MakeField("DRILBOTEL", esriFieldType.esriFieldTypeInteger))
        pFieldsEdit.AddField(basDNRWATGW.MakeField("STRAT", esriFieldType.esriFieldTypeString))
        pFieldsEdit.AddField(basDNRWATGW.MakeField("PRIMLITH", esriFieldType.esriFieldTypeString))
        pFieldsEdit.AddField(basDNRWATGW.MakeField("SECLITH", esriFieldType.esriFieldTypeString))
        pFieldsEdit.AddField(basDNRWATGW.MakeField("COLOR", esriFieldType.esriFieldTypeString))
        pFieldsEdit.AddField(basDNRWATGW.MakeField("PSCLAB", esriFieldType.esriFieldTypeString))
        pFieldsEdit.AddField(basDNRWATGW.MakeField("UTMX", esriFieldType.esriFieldTypeDouble))
        pFieldsEdit.AddField(basDNRWATGW.MakeField("UTMY", esriFieldType.esriFieldTypeDouble))
      Case "contemp"
        pFieldsEdit.AddField(basDNRWATGW.MakeField("CONSTOPEL", esriFieldType.esriFieldTypeInteger))
        pFieldsEdit.AddField(basDNRWATGW.MakeField("COMPBOTEL", esriFieldType.esriFieldTypeInteger))
        pFieldsEdit.AddField(basDNRWATGW.MakeField("CONSTYPE", esriFieldType.esriFieldTypeString))
    End Select

    pFieldsEdit.AddField(basDNRWATGW.MakeField("SORTER", esriFieldType.esriFieldTypeDouble))
    FieldsToBeAdded = pFields
  End Function

  Private Function GetRelSelection(ByRef pFrSelSet As ISelectionSet, ByRef pToTable As ITable, _
          ByRef pRelClass As IRelationshipClass, ByRef pSelectionContainer As IWorkspace) As ISelectionSet

    ' Get the set of the selected rows
    Dim pInSet As ISet
    Dim pCursor As ICursor
    Dim pRow As IRow
    pInSet = New ESRI.ArcGIS.esriSystem.Set
    pFrSelSet.Search(Nothing, False, pCursor)
    pRow = pCursor.NextRow
    Do While Not pRow Is Nothing
      pInSet.Add(pRow)
      pRow = pCursor.NextRow
    Loop
    pInSet.Reset()

    ' Get the set of related rows and build an OID list
    Dim pOutSet As ISet
    Dim pOIDList() As Long
    Dim intOIDIndex As Integer
    Dim intCount As Integer
    pOutSet = pRelClass.GetObjectsRelatedToObjectSet(pInSet)
    If pOutSet.Count <> 0 Then
      pRow = pOutSet.Next
      ReDim pOIDList(pOutSet.Count - 1)
      intOIDIndex = pRow.Fields.FindField(pToTable.OIDFieldName)
      If intOIDIndex = -1 Then
        MsgBox("Input DB missing ObjectID field." & vbCrLf & _
              "Try exporting input DB to a suppoted format" & vbCrLf & _
              "that allows for tabular relates before running or add" & vbCrLf & _
              "an ObjectID field with unique sequential numbers." & vbCrLf & vbCrLf & _
              "See 'About tabular data sources' in the ArcGIS help.", , "Missing OID")
      End If
      intCount = 0
      Do While Not pRow Is Nothing
        pOIDList(intCount) = pRow.Value(intOIDIndex)
        pRow = pOutSet.Next
        intCount = intCount + 1
      Loop
    End If

    ' make a selectionset and add the OID's from the OID list
    Dim pOutSelSet As ISelectionSet
    Dim lngOID As Long
    pOutSelSet = pToTable.Select(Nothing, esriSelectionType.esriSelectionTypeHybrid, _
          esriSelectionOption.esriSelectionOptionEmpty, pSelectionContainer)
    If pOutSet.Count <> 0 Then
      For lngOID = 0 To pOutSet.Count - 1
        pOutSelSet.Add(pOIDList(lngOID))
      Next
    End If
    pCursor = Nothing
    GetRelSelection = pOutSelSet
  End Function

  Private Sub XYTransRotate(ByRef dblTransArray(,) As Double, ByRef dblXTrans As Double, ByRef dblYTrans As Double, _
          ByRef pFeature As IFeature, ByRef pOneFCursor As IFeatureCursor, ByRef pXField As IField, _
          ByRef pYField As IField, ByRef intNumPoints As Integer, ByRef dblVertexCoords(,) As Double, _
          ByRef blnReflex As Boolean)
    Dim dblX As Double, dblY As Double
    Dim dblUTMxRot As Double, dblUTMyRot As Double
    Dim intNumSegments As Integer
    intNumSegments = intNumPoints - 1
    'dblX, dblY in UTM meters
    dblX = pFeature.Value(pOneFCursor.FindField(pXField.Name))
    dblY = pFeature.Value(pOneFCursor.FindField(pYField.Name))

    'Perform number of transformations equal to number of line segments on the cross section
    'and add the new x' y' values to a seperate array.
    Dim dblPointArray(,) As Double
    ReDim dblPointArray(intNumSegments - 1, 1)

    Dim bytIndexArray() As Byte
    ReDim bytIndexArray(intNumSegments - 1)

    Dim dblRadAngle As Double, dblXRot As Double, dblYRot As Double, dblDistance As Double
    Dim intIndex As Integer
    Dim dblHiYValue As Double, dblOldHiYValue As Double
    dblOldHiYValue = 1.7E+308
    intIndex = -1
    Dim i As Integer
    For i = 0 To intNumSegments - 1
      dblRadAngle = dblTransArray(i, (2 * (intNumSegments + 1)) + 1)
      dblXRot = dblTransArray(i, (2 * (intNumSegments + 1)) + 2)
      dblYRot = dblTransArray(i, (2 * (intNumSegments + 1)) + 3)
      'rotate both x and y coordinates for the well
      dblUTMxRot = (Math.Cos(-dblRadAngle) * dblX) - (Math.Sin(-dblRadAngle) * dblY)
      dblUTMyRot = (Math.Sin(-dblRadAngle) * dblX) + (Math.Cos(-dblRadAngle) * dblY)
      'transform both x and y coordinates to the new origin (the very first
      '   well in the cross section)
      dblPointArray(i, 0) = dblUTMxRot - dblXRot
      dblPointArray(i, 1) = dblUTMyRot - dblYRot

      'Perform comparisons to determine which transformation to use (ie what segment of the
      'cross section does each well belong to).

      'Populate the Condition array to see which transformation variables to use on the point.
      'A well can be a member of multiple segments if it's transformed x coordinates are the same
      'distance from multiple segments.  If this is the case, then associate the well to the segment
      'using the least y distance (if y's are also equal, then associate the well with the first
      'segment in order: from to to-node of the polyline).
      If (dblPointArray(i, 0) = 0 And i = 0) Or _
          ((0 < dblPointArray(i, 0)) And (dblPointArray(i, 0) <= dblTransArray(i, 2 * (i + 1)))) Then
        bytIndexArray(i) = 1
      Else
        bytIndexArray(i) = 0
      End If
      If bytIndexArray(i) = 1 Then
        dblHiYValue = dblPointArray(i, 1)
        If dblHiYValue < dblOldHiYValue Then
          dblOldHiYValue = dblHiYValue
          intIndex = i
        End If
      End If
    Next i

    'There are four conditions where a well is situated in comparison to the cross section line.
    '1) the well is closest to one segment and could not be confused as belonging to another
    '   line segment.
    '2) the well lies close to a bend in the cross section line and on the non-reflex (<180 degree)
    '   side...thus it could be associated with 2+ segments
    '3) the well lies close to a bend in the cross section line and on the reflex (>180 degree)
    '   side...thus it would not be associated with any segment as it does not lie within the
    '   x' ranges for the segments (see criteria above which populates the bytIndexArray)
    '4) the well lies beyond the endpoints of the cross section line (before the start or after
    '   the end of the line)

    If intIndex <> -1 Then
      '1st and 2nd condition (when intIndex <> -1):
      dblDistance = dblTransArray(intIndex, 2 * (intNumSegments + 1))
      dblXTrans = dblPointArray(intIndex, 0) + dblDistance
      dblYTrans = dblPointArray(intIndex, 1)
    Else
      '3rd and 4th condition (when intIndex = -1)
      For i = 0 To intNumPoints - 1
        dblDistance = Math.Sqrt(((dblVertexCoords(i, 0) - dblX) ^ 2) + ((dblVertexCoords(i, 1) - dblY) ^ 2))
        If dblDistance < dblOldHiYValue Then
          dblOldHiYValue = dblDistance
          intIndex = i
        End If
      Next i
      'Covers the case when a well lies beyond the end of the cross section line or occurs before
      'the start of the cross section line (condition 4).
      If intIndex = 0 Or intIndex = intNumPoints - 1 Then
        If intIndex = 0 Then
          dblDistance = dblTransArray(intIndex, 2 * (intNumSegments + 1))
          dblXTrans = dblPointArray(intIndex, 0) + dblDistance
          dblYTrans = dblPointArray(intIndex, 1)
        Else
          dblDistance = dblTransArray(intNumSegments - 1, 2 * (intNumSegments + 1))
          dblXTrans = dblPointArray(intNumSegments - 1, 0) + dblDistance
          dblYTrans = dblPointArray(intNumSegments - 1, 1)
        End If
        'Covers the case when a well lies on the reflex (>180 degrees) side of a bend in the cross
        'section (condition 3).
      Else
        dblDistance = dblTransArray(intIndex - 1, 2 * (intNumSegments + 1))
        dblXTrans = dblTransArray(intIndex - 1, (2 * intIndex)) + dblDistance
        dblYTrans = dblOldHiYValue 'distance from vertex is not perpindicular to cross section, but
        'is the shortest distance to the vertex.
        blnReflex = True
      End If
    End If

    'convert both new coordinates to feet from meters (0.3048 m per ft)
    dblXTrans = dblXTrans / 0.3048
    dblYTrans = dblYTrans / 0.3048
    'apply the vertical exaggeration only to the x coordinate
    dblXTrans = dblXTrans / sglTheVertEx

    Erase dblPointArray
    Erase bytIndexArray
  End Sub

  Private Sub CreateShapefiles(ByRef dblTransArray(,) As Double, ByRef dblVertexCoords(,) As Double, _
          ByRef intNumPoints As Integer, ByRef strLabel As String, ByRef strPath As String, ByRef strName As String, _
          ByRef GeoType As esriGeometryType, ByRef sglTheVertEx As Single, ByRef pFClass As IFeatureClass, _
          ByRef strTopField As String, ByRef strBotField As String, ByRef strShapeFileName As String, _
          ByRef pXField As IField, ByRef pYField As IField, ByRef pTable As ITable, _
          ByRef pfrmProgress As frmDNRWATProgressbar)

    If pTable Is Nothing And (strShapeFileName <> "surfpt") Then Exit Sub

    'Create the shapefiles
    pfrmProgress.lblProcess.Text = "Cross section: " & UCase(strLabel) & _
                      " (" & lngTHEXSec & " of " & lngTHENumXSec & ")"
    Dim sglStepUp As Single

    Dim pNewFClass As IFeatureClass
    Dim pSpatialRef As ISpatialReference
    pSpatialRef = New UnknownCoordinateSystem

    Dim blnDatasetField As Boolean
    blnDatasetField = False
    If pFClass.FindField("dataset") <> -1 Then
      blnDatasetField = True
    End If

    pNewFClass = basDNRWATGW.MakeFC(strPath, strName, GeoType, pSpatialRef, _
                                  sglTheVertEx, blnTHEAskAgain)
    If pNewFClass Is Nothing Then
      blnTHEOverWrite = False
      Exit Sub
    End If
    blnTHEAskAgain = False

    If strShapeFileName = "lixpy" Or strShapeFileName = "conpy" Then
      pNewFClass.AddField(basDNRWATGW.MakeField("UNSERNUM", esriFieldType.esriFieldTypeString))
    End If

    If strShapeFileName = "surfpt" Or strShapeFileName = "lixpy" _
          Or strShapeFileName = "conpy" Then
      pNewFClass.AddField(basDNRWATGW.MakeField("UNNUM", esriFieldType.esriFieldTypeString))
    End If

    If strShapeFileName = "surfpt" Then
      pNewFClass.AddField(basDNRWATGW.MakeField("X_COORD", esriFieldType.esriFieldTypeDouble))
      pNewFClass.AddField(basDNRWATGW.MakeField("ELEVATION", esriFieldType.esriFieldTypeInteger))
      pNewFClass.AddField(basDNRWATGW.MakeField("Y_PRIME", esriFieldType.esriFieldTypeDouble))
      pNewFClass.AddField(basDNRWATGW.MakeField("REFLEX", esriFieldType.esriFieldTypeString))
      If blnDatasetField Then
        pNewFClass.AddField(basDNRWATGW.MakeField("DATASET", esriFieldType.esriFieldTypeString))
      End If
    End If

    If strShapeFileName = "lixpy" Then
      pNewFClass.AddField(basDNRWATGW.MakeField("STRAT", esriFieldType.esriFieldTypeString))
      pNewFClass.AddField(basDNRWATGW.MakeField("PRIMLITH", esriFieldType.esriFieldTypeString))
      pNewFClass.AddField(basDNRWATGW.MakeField("SECLITH", esriFieldType.esriFieldTypeString))
      pNewFClass.AddField(basDNRWATGW.MakeField("LITHCOL", esriFieldType.esriFieldTypeString))
      pNewFClass.AddField(basDNRWATGW.MakeField("PSCLAB", esriFieldType.esriFieldTypeString))
      pNewFClass.AddField(basDNRWATGW.MakeField("UTMX", esriFieldType.esriFieldTypeDouble))
      pNewFClass.AddField(basDNRWATGW.MakeField("UTMY", esriFieldType.esriFieldTypeDouble))
    End If

    If strShapeFileName = "conpy" Then
      pNewFClass.AddField(basDNRWATGW.MakeField("CONSTYPE", esriFieldType.esriFieldTypeString))
    End If

    Dim pNewFCursor As IFeatureCursor
    pNewFCursor = pNewFClass.Insert(True)
    Dim pFBuffer As IFeatureBuffer
    Dim dblX As Double

    If strShapeFileName = "lixpy" Or strShapeFileName = "conpy" Then
      'Set the increment value to update the progressbar.
      sglStepUp = 100 / intTHEPhases / pTable.RowCount(Nothing)

      Dim pEnvelope As IEnvelope
      Dim pSegmentColl As ISegmentCollection
      Dim pPolygon As IPolygon
      Dim dblYtop As Double, dblYbot As Double
      Dim pTempSort1Cursor As ICursor, pTempSort2Cursor As ICursor
      pTempSort1Cursor = pTable.Search(Nothing, True)
      pTempSort2Cursor = pTable.Search(Nothing, True)
      Dim pRow1 As IRow, pRow2 As IRow
      pRow1 = pTempSort1Cursor.NextRow
      pRow2 = pTempSort2Cursor.NextRow
      pRow2 = pTempSort2Cursor.NextRow
      Do Until pRow1 Is Nothing
        pFBuffer = pNewFClass.CreateFeatureBuffer

        dblX = pRow1.Value(pTempSort1Cursor.FindField("xfield"))
        dblYtop = pRow1.Value(pTempSort1Cursor.FindField(strTopField))
        'dblYBot will be either the next row's "stratopel"/"constopel" value or the
        '   present row's "drilbotel"/"compbotel" value.
        If Not pRow2 Is Nothing Then
          If pRow2.Value(pTempSort2Cursor.FindField("unnum")) = _
                pRow1.Value(pTempSort1Cursor.FindField("unnum")) Then
            dblYbot = pRow2.Value(pTempSort2Cursor.FindField(strTopField))
          Else
            dblYbot = pRow1.Value(pTempSort1Cursor.FindField(strBotField))
          End If
        Else
          dblYbot = pRow1.Value(pTempSort1Cursor.FindField(strBotField))
        End If

        pEnvelope = New Envelope
        pEnvelope.PutCoords(dblX - 2, dblYbot, dblX + 2, dblYtop)
        pSegmentColl = New Polygon
        pSegmentColl.SetRectangle(pEnvelope)
        pPolygon = pSegmentColl
        pPolygon.Close()
        pFBuffer.Shape = pPolygon
        pFBuffer.Value(pNewFCursor.FindField("unsernum")) = _
              pRow1.Value(pTempSort1Cursor.FindField("unsernum"))
        pFBuffer.Value(pNewFCursor.FindField("unnum")) = _
              pRow1.Value(pTempSort1Cursor.FindField("unnum"))

        If pTempSort1Cursor.FindField("constype") <> -1 Then
          pFBuffer.Value(pNewFCursor.FindField("constype")) = _
              pRow1.Value(pTempSort1Cursor.FindField("constype"))
        ElseIf pTempSort1Cursor.FindField("psclab") <> -1 Then
          pFBuffer.Value(pNewFCursor.FindField("strat")) = _
              pRow1.Value(pTempSort1Cursor.FindField("strat"))
          pFBuffer.Value(pNewFCursor.FindField("primlith")) = _
              pRow1.Value(pTempSort1Cursor.FindField("primlith"))
          pFBuffer.Value(pNewFCursor.FindField("seclith")) = _
              pRow1.Value(pTempSort1Cursor.FindField("seclith"))
          pFBuffer.Value(pNewFCursor.FindField("lithcol")) = _
              pRow1.Value(pTempSort1Cursor.FindField("color"))
          pFBuffer.Value(pNewFCursor.FindField("psclab")) = _
              pRow1.Value(pTempSort1Cursor.FindField("psclab"))
          pFBuffer.Value(pNewFCursor.FindField("utmx")) = _
              pRow1.Value(pTempSort1Cursor.FindField("utmx"))
          pFBuffer.Value(pNewFCursor.FindField("utmy")) = _
              pRow1.Value(pTempSort1Cursor.FindField("utmy"))
        End If

        pNewFCursor.InsertFeature(pFBuffer)
        pRow1 = pTempSort1Cursor.NextRow
        pRow2 = pTempSort2Cursor.NextRow

        'Update the progressbar.
        sglTHEProgress = sglTHEProgress + sglStepUp
        Call basDNRWATGW.UpdateProgress(sglTHEProgress, pfrmProgress)
      Loop
      pNewFCursor.Flush()
      'VB.Net may have trouble with releasing the cursor if so, do not use
      'pNewFCursor = Nothing
      System.Runtime.InteropServices.Marshal.FinalReleaseComObject(pNewFCursor)

      pTempSort1Cursor = Nothing
      pTempSort2Cursor = Nothing

    End If

    If strShapeFileName = "surfpt" Then
      Dim dblXTrans As Double, dblYTrans As Double
      Dim pFeature As IFeature
      Dim pPoint As IPoint
      Dim dblY As Double
      Dim pFCursor As IFeatureCursor

      'Get the wells for the cross section.
      Dim pQueryFilter As IQueryFilter
      pQueryFilter = GetQueryFilter(strTHEXsecField, strLabel)

      If pFClass.FeatureCount(pQueryFilter) = 0 Then
        MsgBox("No wells match the label from cross section " & strLabel & "." & vbCrLf & _
              "Skipping to next process...", , _
              "No associated wells")
        blnTHESkipXsec = True
        Exit Sub
      End If

      'Set the increment value to update the progressbar.
      sglStepUp = 100 / intTHEPhases / pFClass.FeatureCount(pQueryFilter)

      pFCursor = pFClass.Search(pQueryFilter, True)
      pFeature = pFCursor.NextFeature
      Dim blnReflex As Boolean
      blnReflex = False

      Do Until pFeature Is Nothing
        pFBuffer = pNewFClass.CreateFeatureBuffer
        dblY = pFeature.Value(pFCursor.FindField("Elevation"))
        Call XYTransRotate(dblTransArray, dblXTrans, dblYTrans, pFeature, pFCursor, _
                              pXField, pYField, intNumPoints, dblVertexCoords, blnReflex)
        pPoint = New ESRI.ArcGIS.Geometry.Point
        pPoint.X = dblXTrans
        pPoint.Y = dblY
        pFBuffer.Shape = pPoint
        pFBuffer.Value(pNewFCursor.FindField("unnum")) = _
                  pFeature.Value(pFCursor.FindField("Unique_no"))
        pFBuffer.Value(pNewFCursor.FindField("x_coord")) = dblXTrans
        pFBuffer.Value(pNewFCursor.FindField("elevation")) = _
                  pFeature.Value(pFCursor.FindField("Elevation"))
        pFBuffer.Value(pNewFCursor.FindField("y_prime")) = dblYTrans
        If blnReflex Then
          'Sets an indicator that this well was situated on the reflex side of the bend
          'in the cross section....thus, the y' prime number is not the perpindicular
          'distance from the cross section, but the distance to the bend in the cross section
          'at an angle on the reflex side that does not allow for the well to be associated
          'with any given segment of the cross section line.
          pFBuffer.Value(pNewFCursor.FindField("reflex")) = "R"
        End If
        If pFCursor.FindField("Dataset") <> -1 Then
          pFBuffer.Value(pNewFCursor.FindField("dataset")) = _
                  pFeature.Value(pFCursor.FindField("Dataset"))
        End If
        pNewFCursor.InsertFeature(pFBuffer)
        blnReflex = False
        pFeature = pFCursor.NextFeature

        'Update the progressbar.
        sglTHEProgress = sglTHEProgress + sglStepUp
        Call basDNRWATGW.UpdateProgress(sglTHEProgress, pfrmProgress)
      Loop
      pNewFCursor.Flush()

      pNewFClass = Nothing

      pFCursor = Nothing
      'VB.Net may have trouble with releasing the cursor if so, do not use
      'pNewFCursor = Nothing
      System.Runtime.InteropServices.Marshal.FinalReleaseComObject(pNewFCursor)
    End If
  End Sub

  Private Sub CreateTransMatrix(ByRef dblTransArray(,) As Double, ByRef dblVertexCoords(,) As Double, _
          ByRef intNumPoints As Integer, ByRef pPointCollection As IPointCollection)

    Dim intNumRows As Integer, intNumCols As Integer
    Dim pFromPoint As IPoint, pToPoint As IPoint

    Dim dblXRot As Double, dblYRot As Double ', dblXTrans As Double, dblYTrans As Double
    Dim dblMeasure As Double, dblRadAngle As Double, dblRatio As Double
    Dim dblUTMxRot As Double, dblUTMyRot As Double

    'Creates the transformation matrix with the following values:
    '(for each cross section, N is number of points on the cross section, thus N-1 is the
    'number of line segments on the cross section).  The matrix is a two dimensional array with number
    'of rows = N-1 (one row for each segment of the cross section which contains the transformation
    'information needed to rotate and translate points associated with a given segment) and number
    'of columns = 2N + 4.
    '2 columns for each point on the cross section representing new values for x and y
    'using angle,translation, and offset values for a given segment.
    'and...
    '4 columns at the end of the matrix containing: 1) offset to add to the X
    'value of translated points (derived from the measure of each line segment); 2) radian angle to
    'rotate; 3) New Xn' Rotated Value; and, 4) New Yn' Rotated Value.



    'Also creates the Vertex Array holding the original coordinate values for each point along the
    'cross section line.  This array has 3 columns, the first two represent x and y values, the
    'third is set to 1 for the endpoints and 0 for the midpoints and will be used later to determine
    'which point is closest to a given well after not meeting certain criteria...

    intNumPoints = pPointCollection.PointCount
    intNumRows = intNumPoints - 2
    intNumCols = (2 * intNumPoints) + 3

    ReDim dblTransArray(intNumRows, intNumCols)
    ReDim dblVertexCoords(intNumPoints - 1, 2)
    Dim i1 As Integer, i2 As Integer
    dblMeasure = 0
    For i1 = 0 To intNumPoints - 2
      'The formula to rotate a point (x,y) around the origin (0, 0) through an angle
      '(theta) where the new point's coordinates (x', y') are given by:
      '       x' = cos(theta)*x - sin(theta)*y
      '       y' = sin(theta)*x + cos(theta)*y
      '
      'To transform (translate) a point (x,y) based on origin (0, 0) to another point (x', y')
      ' based on another origin (0x, 0y), simply do:
      '       x' = x - 0x
      '       y' = y - 0y
      '
      pFromPoint = pPointCollection.Point(i1)
      pToPoint = pPointCollection.Point(i1 + 1)

      dblVertexCoords(i1, 0) = pFromPoint.X
      dblVertexCoords(i1, 1) = pFromPoint.Y

      'first endpoint on cross section line:from node
      If i1 = 0 Then
        dblVertexCoords(i1, 2) = 1
      End If

      'Determine which among 8 cardinal directions the line segment is oriented to: 1) from S to N;
      '  2) N to S; 3) E to W; 4) W to E; 5) SW to NE; 6) NW to SE; 7) NE to SW; and 8) SE to NW.
      '  Case 0) refers to a line segment with two endpoints that are exactly the same coordinates.
      '
      '            |
      '            1
      '      8     |    5
      '            |
      '----3-------0-------4----
      '            |
      '      7     |    6
      '            2
      '            |
      '

      'Declares the value for the constant pi. The tangent of 45 degrees is 1.
      '   the arctangent of 1 is pi/4 in radians.  Thus, the following...
      Dim pi As Double
      pi = 4 * Math.Atan(1)

      'For cases 0, 1, and 2:
      If (pToPoint.X - pFromPoint.X) = 0 Then
        'Case 1
        If (pToPoint.Y - pFromPoint.Y) > 0 Then
          dblRadAngle = pi / 2
          'Cases 0 and 2
        Else : dblRadAngle = -(pi / 2)
        End If
      End If

      'For cases 0, 3, and 4:
      If (pToPoint.Y - pFromPoint.Y) = 0 Then
        'Case 3
        If (pToPoint.X - pFromPoint.X) < 0 Then
          dblRadAngle = pi
          'Cases 0 and 4
        Else : dblRadAngle = 0
        End If
      End If

      'For cases 5 and 6:
      If (pToPoint.X - pFromPoint.X) > 0 Then
        dblRatio = (pToPoint.Y - pFromPoint.Y) / (pToPoint.X - pFromPoint.X)
        dblRadAngle = Math.Atan(dblRatio)
      End If

      'For cases 7 and 8:
      If (pToPoint.X - pFromPoint.X) < 0 Then
        dblRatio = (pToPoint.Y - pFromPoint.Y) / (pToPoint.X - pFromPoint.X)
        dblRadAngle = Math.Atan(dblRatio) + pi
      End If

      dblXRot = (Math.Cos(-dblRadAngle) * pFromPoint.X) - (Math.Sin(-dblRadAngle) * pFromPoint.Y)
      dblYRot = (Math.Sin(-dblRadAngle) * pFromPoint.X) + (Math.Cos(-dblRadAngle) * pFromPoint.Y)
      dblTransArray(i1, intNumCols - 2) = dblRadAngle
      dblTransArray(i1, intNumCols - 1) = dblXRot
      dblTransArray(i1, intNumCols) = dblYRot
      For i2 = 0 To intNumPoints - 1
        dblUTMxRot = (Math.Cos(-dblRadAngle) * pPointCollection.Point(i2).X) - _
                      (Math.Sin(-dblRadAngle) * pPointCollection.Point(i2).Y)
        dblUTMyRot = (Math.Sin(-dblRadAngle) * pPointCollection.Point(i2).X) + _
                      (Math.Cos(-dblRadAngle) * pPointCollection.Point(i2).Y)
        dblTransArray(i1, (2 * i2)) = dblUTMxRot - dblXRot
        dblTransArray(i1, (2 * i2) + 1) = dblUTMyRot - dblYRot
        If i2 - 1 = i1 Then
          dblTransArray(i1, intNumCols - 3) = dblMeasure
          dblMeasure = dblMeasure + dblUTMxRot - dblXRot
        End If
      Next i2
    Next i1
    pFromPoint = pPointCollection.Point(intNumPoints - 1)
    dblVertexCoords(intNumPoints - 1, 0) = pFromPoint.X
    dblVertexCoords(intNumPoints - 1, 1) = pFromPoint.Y
    'last endpoint on cross section line:to node
    dblVertexCoords(intNumPoints - 1, 2) = 1

    'Print the values from the filled arrays in the immediate window.
    Debug.WriteLine("")
    Debug.WriteLine("dblVertexCoords Array")
    For i1 = 0 To intNumPoints - 1
      For i2 = 0 To 2
        If i2 < 2 Then
          Debug.Write(dblVertexCoords(i1, i2) & " ,")
        Else
          Debug.WriteLine(dblVertexCoords(i1, i2))
        End If
      Next i2
    Next i1

    Debug.WriteLine("")
    Debug.WriteLine("dblTransArray Array")
    For i1 = 0 To intNumRows
      For i2 = 0 To intNumCols
        If i2 < intNumCols Then
          Debug.Write(dblTransArray(i1, i2) & " ,")
        Else
          Debug.WriteLine(dblTransArray(i1, i2))
        End If
      Next i2
    Next i1
    Debug.WriteLine("")

  End Sub

  Private Function GetQueryFilter(ByRef strFieldName As String, ByRef strLabel As String) As IQueryFilter
    Dim pQueryFilter As IQueryFilter
    pQueryFilter = New QueryFilter
    Dim strXNUpper As String, strXNLower As String
    strXNUpper = UCase(strLabel)
    strXNLower = LCase(strLabel)
    'The first part of the if/then is for personal geodatabases (using [] and *),
    'the second fits most types of layers (shapefiles,coverages, etc using %).
    If strTHEWellDataType = "Personal Geodatabase Feature Class" Then
      pQueryFilter.WhereClause = "[" & strFieldName & "] LIKE '" & strXNUpper & ",*' OR " & _
                              "[" & strFieldName & "] LIKE '*," & strXNUpper & ",*' OR " & _
                              "[" & strFieldName & "] LIKE '*," & strXNUpper & "' OR " & _
                              "[" & strFieldName & "] = '" & strXNUpper & "' OR " & _
                              "[" & strFieldName & "] LIKE '" & strXNLower & ",*' OR " & _
                              "[" & strFieldName & "] LIKE '*," & strXNLower & ",*' OR " & _
                              "[" & strFieldName & "] LIKE '*," & strXNLower & "' OR " & _
                              "[" & strFieldName & "] = '" & strXNLower & "'"
    Else
      pQueryFilter.WhereClause = strFieldName & " LIKE '" & strXNUpper & ",%' OR " & _
                              strFieldName & " LIKE '%," & strXNUpper & ",%' OR " & _
                              strFieldName & " LIKE '%," & strXNUpper & "' OR " & _
                              strFieldName & " = '" & strXNUpper & "' OR " & _
                              strFieldName & " LIKE '" & strXNLower & ",%' OR " & _
                              strFieldName & " LIKE '%," & strXNLower & ",%' OR " & _
                              strFieldName & " LIKE '%," & strXNLower & "' OR " & _
                              strFieldName & " = '" & strXNLower & "'"
    End If
    GetQueryFilter = pQueryFilter
  End Function

End Class