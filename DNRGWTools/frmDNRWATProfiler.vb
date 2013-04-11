Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.Analyst3D
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.DataSourcesRaster
Imports System.Windows.Forms
Imports System.Drawing

Public Class frmDNRWATProfiler

  ' Name:     frmDNRWATprofiler
  '
  ' Author:   Greg Massaro
  '           DNR Ecological and Water Resources
  '           500 Lafayette Road
  '           St. Paul, MN 55155
  '           greg.massaro@state.mn.us
  '           (651-259-5693)
  '
  ' Date: Jun 12 2008
  ' Revised by: Greg Massaro
  ' Revision Date: Jan 25 2011
  ' Revisions:
  '
  ' Revised by: Matthew Rantala, Minnesota Geological Survey, mjrantal@umn.edu
  ' Revision Date: April 5 2013
  ' Revisions: Revised the code to allowe user to run the process on multiple rasters
  ' at one time.  I also stored the output path so that it could be re-used each time
  ' frmDNRWATprofiler is opened during an ArcMap session.
  '
  ' I marked each of my revisions with "V1.2" 
  '
  ' I was using a 64-bit Windows machine (Windows Server 2008 r2) and had trouble
  ' Compiling the code.  The following ESRI and Microsoft articles led me to a
  ' solution:
  ' http://support.esri.com/es/knowledgebase/techarticles/detail/37879
  ' http://support.microsoft.com/kb/2028833
  ' http://social.msdn.microsoft.com/Forums/en-US/msbuild/thread/e5900710-9849-4d10-aa28-48b734d06bf2
  '
  ' So I had to run corflags on resgen.exe:  corflags.exe resgen.exe /32BIT+ /Force
  ' And had to add this line to DNRGWTools.vbproj:
  '   <ResGenToolArchitecture>Managed32Bit</ResGenToolArchitecture>
  ' (It is the fourth line in the file, first item in the <PropertyGroup> section.
  '
  '
  ' -----------------------------------------------------------------------------
  ' Description:
  '           Runs a batch process to generate shapefiles from any raster layer.
  '           Specifically, the code is designed to extract topographic profiles for
  '           a given set of cross section lines from a Digital Elevation Model.  The
  '           program generates a point shapefile representing the surface elevation
  '           at even intervals based on the raster cell size and then creates a line
  '           shapefile connecting these points.
  '
  '
  ' Requires:
  ' Runs:     frmDNRWATProgressbar, basDNRWATGW
  ' Run by:   basDNRWATGW
  ' Returns:
  '==============================================================================
  '
  Private sglTheVertEx As Single
  Private pTHEXSecs As ILayer
  Private pTHEXSecFields As IFields
  Private pTHERLayer As IRasterLayer
  Private pTHERaster As IRaster
  Private pTHERasterList As List(Of IRasterLayer) = New List(Of IRasterLayer)
  Private strTHEPath As String
  Private blnTHEOverWrite As Boolean
  Private blnTHEAskAgain As Boolean
  Private sglTHEProgress As Single

  Private Sub frmDNRWATProfiler_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    g_blnRunning = False
    tboBaseName.Text = "%vx_%l_%r" 'V1.2 topo
    tboVertEx.Text = 50 
    ttpToolTip.SetToolTip(Me.tboBaseName, "Output Naming Scheme, use '%v' to include Vertical Exaggeration, '%l' for the label value, and '%r' for the raster name") 'V1.2 Added
  End Sub

  Private Sub cboXSecLayer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboXSecLayer.SelectedIndexChanged
    Call ComboBoxChange(cboXSecLayer)
  End Sub

  'V1.2 Commented this out
  'Private Sub cboRasterLayer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboRasterLayer.SelectedIndexChanged
   ' Call ComboBoxChange(cboRasterLayer)
  'End Sub

  'V1.2 Added the Sub below
  Private Sub lboRasterLayers_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lboRasterLayers.SelectedIndexChanged
    'Making this temp Combox to pass in as the required input parameter.
    'Probably should have re-factored ComboBoxChange to accept any Windows Form Control.
    Try
        Dim pTempCombobox As ComboBox = New ComboBox 
        pTempCombobox.Name = "lboRasterLayersTemp"
        pTempCombobox.Text = "pTempCombobox"
        Call ComboBoxChange(pTempCombobox)
    Catch ex As Exception
        MsgBox(ex.Message.ToString)
    End Try

  End Sub

  Private Sub ComboBoxChange(ByVal cbo As ComboBox)
    'Enables certain buttons/comboboxes on the userform based on inputs.
    If cbo.Text = "" Then
      Exit Sub
    End If
    'V1.2 Added this If Block
    If cbo.Name = "lboRasterLayersTemp" Then
        pTHERasterList.Clear
    End If

    Dim i As Long
    Dim pMap As IMap
    pMap = My.ArcMap.Document.FocusMap 'pMxDoc.FocusMap
    Dim pLayer As ILayer

    For i = 0 To pMap.LayerCount - 1
      pLayer = pMap.Layer(i)
      'V1.2 Added this If Block
      If cbo.Name = "lboRasterLayersTemp" Then
        If lboRasterLayers.SelectedItems.Contains(pLayer.Name) Then
            'MsgBox(pLayer.GetType.ToString+vbNewLine+pLayer.GetType.BaseType.ToString+vbNewLine+pLayer.GetType.DeclaringType.ToString+pLayer.GetType.ReflectedType.ToString)

            pTHERasterList.Add(pLayer)
            pTHERLayer = pLayer
            pTHERaster = pTHERLayer.Raster
            obuSurface.Enabled = True
            obuRaster.Enabled = True
            Call ExtractProfileEnabler()
        End If

      End If

      If pLayer.Name = cbo.Text Then
        If cbo.Name = cboXSecLayer.Name Then
          pTHEXSecs = pLayer
          lblXSecLabel.Enabled = True
          cboXSecLabel.Enabled = True
          cboXSecLabel.BackColor = SystemColors.Window

          Dim pFlayer As IFeatureLayer
          Dim pFClass As IFeatureClass
          pFlayer = pLayer
          pFClass = pFlayer.FeatureClass

          Dim pFields As IFields
          Dim pField As IField
          pTHEXSecFields = pFClass.Fields
          pFields = pFClass.Fields
          Dim f As Long
          cboXSecLabel.Items.Clear()
          For f = 0 To pFields.FieldCount - 1
            pField = pFields.Field(f)
            cboXSecLabel.Items.Add(pField.Name)
          Next f

          Call ExtractProfileEnabler()
          Exit Sub
        'V1.2 Commented this out
        'ElseIf cbo.Name = cboRasterLayer.Name Then
        '  pTHERLayer = pLayer
        '  pTHERaster = pTHERLayer.Raster
        '  obuSurface.Enabled = True
        '  obuRaster.Enabled = True
        '  Call ExtractProfileEnabler()
        '  Exit Sub
        End If
      End If
    Next i
    If cbo.Name = cboXSecLayer.Name Then
      lblXSecLabel.Enabled = False
      cboXSecLabel.Items.Clear()
      cboXSecLabel.Enabled = False
      cboXSecLabel.BackColor = SystemColors.Control
    End If
    If cbo.Name <> "lboRasterLayersTemp" Then 'V1.2 Added this condition because the lboRasterLayersTemp does not have an Exit Sub
        comGenProfiles.Enabled = False
    End If
  End Sub

  Private Sub comOutput_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles comOutput.Click
    'This subroutine enables the user to select the output batch location from
    '  an user defined dialog.
    'V1.2 Add the IF block below so the output path defaults to the previous run.
    If Not (strTHEPath is Nothing) and (strTHEPath <> "") then
      flddlgEPBrowseFolders.SelectedPath = strTHEPath
    End If
    
    flddlgEPBrowseFolders.ShowDialog()
    strTHEPath = flddlgEPBrowseFolders.SelectedPath
    tboOutput.Text = strTHEPath
    Call ExtractProfileEnabler()
  End Sub

  Private Sub tboBaseName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tboBaseName.TextChanged
    Dim strChar As String, strBaseName As String
    Dim intLen As Integer
    strBaseName = tboBaseName.Text
    intLen = Len(strBaseName)
    'Test the basename for an invalid character {\,/,:,*,?,",<,>,|} and if
    'it is present, inform the user and remove it from the basename.
    strChar = Microsoft.VisualBasic.Right(strBaseName, 1)
    Select Case strChar
      Case "\", "/", ":", "*", "?", """", "<", ">", "|"
        MsgBox("Please enter a base filename that does" & vbCrLf & _
            "not contain the following characters:" & vbCrLf & _
            "\ / : * ? "" < > |", , "Bad Filename")
        tboBaseName.Text = Microsoft.VisualBasic.Left(strBaseName, intLen - 1)
    End Select
    Call ExtractProfileEnabler()
  End Sub

  Private Sub tboVertEx_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tboVertEx.TextChanged
    Call ExtractProfileEnabler()
  End Sub

  Private Sub comGenProfiles_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles comGenProfiles.Click

    'V1.2 Move the Call of ExtractProfiles into this loop
    For Each pRasterLayer As IRasterLayer in pTHERasterList 'V1.2
        Try
            Call ExtractProfile(sglTheVertEx, pRasterLayer, pRasterLayer.Raster, strTHEPath,pRasterLayer.FilePath.ToString = pTHERasterList(pTHERasterList.Count-1).FilePath.ToString)
        Catch ex As Exception
            MsgBox("Error Try")
        End Try
    Next
    
    'sglTheVertEx = Nothing
    pTHEXSecs = Nothing
    pTHEXSecFields = Nothing
    pTHERaster = Nothing
    'strTHEPath = Nothing
    'blnTHEOverWrite = Nothing
    'blnTHEAskAgain = Nothing
    'sglTHEProgress = Nothing
  End Sub

  Private Sub ExtractProfileEnabler()
    'V1.2 Removed the first condition of this line
    'If cboRasterLayer.Text <> "" And tboOutput.Text <> "" And tboBaseName.Text <> "" _
    If tboOutput.Text <> "" And tboBaseName.Text <> "" _
          And IsNumeric(tboVertEx.Text) And cboXSecLayer.Text <> "" _
          And lboRasterLayers.SelectedItems.Count > 0 Then  'V1.2 Added this final condition
      'V1.2 Added this first IF statement.  If user is running on more than one raster, then
      'MUST include %r (raster name) in the naming scheme.
      If (lboRasterLayers.SelectedItems.Count>1) then
        If (tboBaseName.Text.Contains("%r")) then 
          comGenProfiles.Enabled = True
        End If
      Else
        comGenProfiles.Enabled = True
      End If
      
      sglTheVertEx = tboVertEx.Text
    Else : comGenProfiles.Enabled = False
    End If
  End Sub

  Private Sub ExtractProfile(ByRef sglTheVertEx As Single, ByRef pRLayer As IRasterLayer, _
                             ByRef pRaster As IRaster, ByRef strPath As String, Optional ByVal blnDisplayFinalMessage As Boolean = True)
    On Error GoTo EH
    'If no layer is currently selected in the xsec combobox, then an error message
    '   is displayed and the sub is exited.
    If cboXSecLayer.SelectedIndex = -1 Then
      MsgBox("Please select the cross section layer.", , "Select a Layer")
      Exit Sub
    End If
    'V1.2 Commented out the block below
    ''If no layer is currently selected in the raster combobox, then an error message
    ''   is displayed and the sub is exited.
    'If cboRasterLayer.SelectedIndex = -1 Then
    '  MsgBox("Please select the raster layer.", , "Select a Layer")
    '  Exit Sub
    'End If
    'V1.2 Added If Block below
    'If no layers are currently selected in the raster listbox, then an error message
    '   is displayed and the sub is exited.
    If lboRasterLayers.SelectedItems.Count = 0 Then
      MsgBox("Please select the raster layer(s).", , "Select a Layer")
      Exit Sub
    End If
    'If no field has been selected in the Xsec Label combobox, then an
    '   error message displays and the sub is exited.
    If cboXSecLabel.SelectedIndex = -1 Then
      MsgBox("Please select the field containing" & vbCrLf & _
        "the label for each cross section.", , "Select Field")
      Exit Sub
    End If
    'V1.2 Added If Block below
    If pTHEXSecFields is nothing Then
      MsgBox("Please select the field containing" & vbCrLf & _
        "the label for each cross section.", , "Select Field")
      Exit Sub
    End If

    Dim pField As IField, pLabelField As IField
    Dim f As Long
    For f = 0 To pTHEXSecFields.FieldCount - 1
      pField = pTHEXSecFields.Field(f)
      If cboXSecLabel.Text = pField.Name Then
        pLabelField = pField
      End If
    Next f

    'If the selected fields in the Xsec Label field comboboxes is not of
    '   the string type, then an error message displays and the sub is exited.
    If pLabelField.Type <> 4 Then
      MsgBox("The label field is not a string.  " & _
              vbCrLf & vbCrLf & "Please select a field for the label" & _
              vbCrLf & "which contains string values.", , "Select Field")
      Exit Sub
    End If

    Dim strLabelField As String
    strLabelField = cboXSecLabel.Text

    Dim strFileName As String, strProfileName As String

    If pRaster Is Nothing Then Exit Sub
    Dim pRasterSurface As IRasterSurface, pSurface As ISurface
    pRasterSurface = New RasterSurface
    pRasterSurface.PutRaster(pRaster, 0)
    pSurface = pRasterSurface

    Dim dCellSize As Double
    dCellSize = GetCellSize(pRaster)

    Dim pCurve As ICurve
    Dim pFC As IFeatureClass

    Dim pMap As IMap
    pMap = My.ArcMap.Document.FocusMap 'pMxDoc.FocusMap
    Dim pFlayer As IFeatureLayer
    Dim pFLayer2 As IFeatureLayer2
    Dim pFClass As IFeatureClass

    Dim pFSelection As IFeatureSelection
    Dim pSelectionSet As ISelectionSet

    Dim l As Long
    Dim pFCur As IFeatureCursor
    Dim pFeature As IFeature

    'Sets up the progress bar according to number of layers, selected features, and number
    'of points to sample the raster along the feature.

    Me.Hide()
    Application.DoEvents()

    Dim pfrmProgress As frmDNRWATProgressbar
    pfrmProgress = New frmDNRWATProgressbar

    g_blnRunning = True

    With pfrmProgress
      .pbaProcess.Maximum = 101
      .Text = "Extracting Profiles"
      .lblPercent.Text = "0 % completed."
      .Show()
    End With

    Application.DoEvents()

    Dim sglNumFeatures As Single
    sglTHEProgress = 0

    blnTHEOverWrite = True
    blnTHEAskAgain = True

    For l = 0 To pMap.LayerCount - 1
      If TypeOf pMap.Layer(l) Is IFeatureLayer Then
        pFLayer2 = pMap.Layer(l)
        pFlayer = pFLayer2
        If pFLayer2.ShapeType = esriGeometryType.esriGeometryPolyline Then
          If pFlayer.Name = cboXSecLayer.Text Then
            pFClass = pFLayer2.FeatureClass
            pFSelection = pFLayer2
            pSelectionSet = pFSelection.SelectionSet
            If pSelectionSet.Count > 0 Then
              pSelectionSet.Search(Nothing, False, pFCur)
              sglNumFeatures = pSelectionSet.Count
            Else : pFCur = pFClass.Search(Nothing, False)
              sglNumFeatures = pFClass.FeatureCount(Nothing)
            End If
            pFeature = pFCur.NextFeature
            Dim lngXSec As Long
            Dim strLabel As String
            lngXSec = 1
            Do Until pFeature Is Nothing
              strLabel = pFeature.Value(pFCur.FindField(strLabelField))
              If CheckIfMultiPartLine(pFeature, strLabel, sglNumFeatures, pfrmProgress) Then
                GoTo NextXSec
              End If
              pCurve = GetPolyline(pFeature)
              'V1.2 strFileName = "x" & Int(sglTheVertEx) & "_" & _
              'V1.2  basDNRWATGW.StripString(strLabel) & "_" & tboBaseName.Text
              Dim pBaseName As String = tboBaseName.Text 'V1.2
              If Not (pBaseName.Contains("%l")) then 'V1.2 Added this to append the %l to the basename if it was not already there.
                pBaseName+= "_%l"
              End If
              strFileName = pBaseName.Replace("%v",Int(sglTheVertEx).tostring).Replace("%r",basDNRWATGW.StripString(pRLayer.Name)).Replace("%l",basDNRWATGW.StripString(strLabel)) 'V1.2
              pfrmProgress.lblProcess.Text = "Processing cross section: " & _
                strLabel & " (" & lngXSec & " of " & sglNumFeatures & ")"
              pFC = Line2XYZMPoints(pCurve, pRaster, pSurface, strPath, strFileName, strLabel, _
                                        sglNumFeatures, dCellSize, sglTheVertEx, pfrmProgress, _
                                        pRLayer)

              'Exit this code if the user does not wish to overwrite shapefiles.
              If blnTHEOverWrite = False Then Exit Do
NextXSec:
              pFeature = pFCur.NextFeature
              lngXSec = lngXSec + 1
            Loop
            'VB.Net may have trouble with releasing the cursor if so, use instead
            'System.Runtime.InteropServices.Marshal.FinalReleaseComObject(pFCur)
            pFCur = Nothing
          End If
        End If
      End If
    Next l
    g_blnRunning = False
    pfrmProgress.Close()
    pfrmProgress = Nothing
    If blnTHEOverWrite Then
        If (blnDisplayFinalMessage=True) 'V1.2 Added this condition
          MsgBox("Finished Processing", vbOKOnly, "Extract Profiles")
        End If
    Else : MsgBox("Bailing out of extract profiles...", vbOKOnly, "Extract Profiles")
    End If
    Exit Sub

EH:
    pfrmProgress.Close()
    pfrmProgress = Nothing
    MsgBox(Err.Number & "  " & Err.Description)
  End Sub

  Private Function GetCellSize(ByRef pRasterProps As IRasterProps) As Double
    ' assume cells are square
    GetCellSize = pRasterProps.MeanCellSize.X
  End Function

  Private Function CheckIfMultiPartLine(ByRef pFeature As IFeature, ByRef strLabel As String, _
          ByRef sglNumFeatures As Single, ByRef pfrmProgress As frmDNRWATProgressbar) As Boolean
    Dim pGeometry As IGeometry
    pGeometry = pFeature.ShapeCopy

    Dim pPointCollection As IPointCollection
    Dim pSegmentCollection As ISegmentCollection

    If TypeOf pGeometry Is IPointCollection Then
      pPointCollection = pGeometry
      pSegmentCollection = pGeometry
    Else
      MsgBox("Error getting point collection for cross section line.", , "Error")
      Exit Function
    End If

    'If pGeometry is a multipart line, ask the user if they wish to continue...
    'continuing may result in errors... skip to next xsec line if no...
    If pPointCollection.PointCount > pSegmentCollection.SegmentCount + 1 Then
      If MsgBox("Cross section " & UCase(strLabel) & " is a Multipart Line." & _
        vbCrLf & "Continuing this process may lead to errors in the final shapefiles." _
        & vbCrLf & vbCrLf & "Continue with the generation of shapefiles for cross" & _
        " section " & UCase(strLabel) & "?" & vbCrLf & "(selecting 'No' will skip " & _
        "this cross " & "section only)", vbYesNo, "Multipart Line Detected") = vbNo Then

        Dim sglStepUp As Single
        sglStepUp = 100 / sglNumFeatures
        'Update the progress bar by one step increase.
        sglTHEProgress = sglTHEProgress + sglStepUp
        Call basDNRWATGW.UpdateProgress(sglTHEProgress, pfrmProgress)

        CheckIfMultiPartLine = True
        Exit Function
      End If
    End If
    CheckIfMultiPartLine = False
  End Function

  Private Function GetPolyline(ByRef pFeature As IFeature) As ICurve
    GetPolyline = pFeature.Shape
  End Function

  Private Function Line2XYZMPoints(ByRef pCurve As ICurve, ByRef pRaster As IRaster, ByRef pSurface As ISurface, _
          ByRef strPath As String, ByRef strName As String, ByRef strLabel As String, ByRef sglNumFeatures As Single, _
          ByRef dStepSize As Double, ByRef sglTheVertEx As Single, ByRef pfrmProgress As frmDNRWATProgressbar, _
          ByRef pRLayer As IRasterLayer) As IFeatureClass

    ' creates a point shapefile with x,y,z and m by
    ' stepping along a polyline (pCurve)
    '      x & y are point coordinates
    '      z is the raster value (first raster band),
    '      d is the distance along the curve

    Dim strRName As String
    strRName = pRLayer.Name

    Dim pPixelBlock As IPixelBlock3
    pPixelBlock = pRaster.CreatePixelBlock(MakePnt(1.0#, 1.0#))
    pPixelBlock.PixelType(0) = rstPixelType.PT_SHORT

    Dim pSpatialRef As ISpatialReference
    pSpatialRef = New UnknownCoordinateSystem
    'Dim pPointFC As IFeatureClass 'uncomment this line and others below marked "points" to create point file
    Dim pPolyLineFC As IFeatureClass

    ''POINTS
    'pPointFC = basDNRWATGW.MakeFC(strPath, strName & "pt", esriGeometryType.esriGeometryPoint, _
    '                                  pSpatialRef, sglTheVertEx, blnTHEAskAgain)
    'If pPointFC Is Nothing Then
    '  blnTHEOverWrite = False
    '  Exit Function
    'End If

    blnTHEAskAgain = False

    ''POINTS
    'pPointFC.AddField(basDNRWATGW.MakeField("DIST_" & Math.Round(sglTheVertEx) & "X", esriFieldType.esriFieldTypeDouble))
    'pPointFC.AddField(basDNRWATGW.MakeField("ELEVATION", esriFieldType.esriFieldTypeDouble))
    'pPointFC.AddField(basDNRWATGW.MakeField("ORDER", esriFieldType.esriFieldTypeInteger))

    pPolyLineFC = basDNRWATGW.MakeFC(strPath, strName & "ln", esriGeometryType.esriGeometryPolyline, _
                                      pSpatialRef, sglTheVertEx, blnTHEAskAgain)
    'Add XSec and Unit fields
    pPolyLineFC.AddField(basDNRWATGW.MakeField("XSEC", esriFieldType.esriFieldTypeString))
    pPolyLineFC.AddField(basDNRWATGW.MakeField("UNIT", esriFieldType.esriFieldTypeString))


    ''POINTS
    'Dim pPTFCur As IFeatureCursor
    'pPTFCur = pPointFC.Insert(False)
    'Dim pPTFeatBuff As IFeatureBuffer
    'pPTFeatBuff = pPointFC.CreateFeatureBuffer

    Dim d As Double, pPoint As IPoint, pPolyline As IPointCollection
    Dim pPolyLineFeature As IFeature

    Dim dblElevation As Double
    pPoint = New ESRI.ArcGIS.Geometry.Point
    pPolyline = New Polyline

    Dim sglNumPoints As Single
    sglNumPoints = 100 / sglNumFeatures / (pCurve.Length / dStepSize)
    Dim v As Object

    Dim blnNullPoint As Boolean
    blnNullPoint = False

    For d = 0 To pCurve.Length Step dStepSize
      pCurve.QueryPoint(esriSegmentExtension.esriNoExtension, d, False, pPoint)
      dblElevation = pSurface.GetElevation(pPoint)
      If (pSurface.IsVoidZ(dblElevation)) Then
        blnNullPoint = True
        '      dblElevation = 0
        If pPolyline.PointCount > 0 Then
          If pPolyline.PointCount >= 2 Then
            pPolyLineFeature = pPolyLineFC.CreateFeature
            pPolyLineFeature.Shape = pPolyline

            'add attribute values
            pPolyLineFeature.Value(pPolyLineFeature.Fields.FindField("XSEC")) = strLabel
            pPolyLineFeature.Value(pPolyLineFeature.Fields.FindField("UNIT")) = strRName

            pPolyLineFeature.Store()
          End If
          pPolyline = New Polyline
        End If
      End If

      If Not blnNullPoint Then
        'get values from the raster directly or a surface:
        If obuSurface.Checked Then
          pPoint.Y = dblElevation
        Else
          pRaster.Read(GetPixelPnt(pPoint, pRaster), pPixelBlock)
          v = pPixelBlock.PixelData(0)
          pPoint.Y = v(0, 0)
        End If

        'X value needs to be converted from meters to feet and altered for vertical exaggeration
        pPoint.X = d / 0.3048 / sglTheVertEx

        ''POINTS
        'pPTFeatBuff.Shape = pPoint
        'pPTFeatBuff.Value(pPTFCur.FindField("DIST_" & Math.Round(sglTheVertEx) & "X")) = pPoint.X
        'pPTFeatBuff.Value(pPTFCur.FindField("ELEVATION")) = pPoint.Y
        'pPTFeatBuff.Value(pPTFCur.FindField("ORDER")) = d
        'pPTFCur.InsertFeature(pPTFeatBuff)

        pPolyline.AddPoint(pPoint)
        Debug.Print(pPoint.X, d)
      End If

      blnNullPoint = False

      sglTHEProgress = sglTHEProgress + sglNumPoints
      Call basDNRWATGW.UpdateProgress(sglTHEProgress, pfrmProgress)
    Next d
    If d - dStepSize <> pCurve.Length Then
      pCurve.QueryPoint(esriSegmentExtension.esriNoExtension, pCurve.Length, False, pPoint)
      dblElevation = pSurface.GetElevation(pPoint)
      If (pSurface.IsVoidZ(dblElevation)) Then
        blnNullPoint = True
        '      dblElevation = 0
      End If

      If Not blnNullPoint Then
        'get values from the raster directly or a surface:
        If obuSurface.Checked Then
          pPoint.Y = dblElevation
        Else
          pRaster.Read(GetPixelPnt(pPoint, pRaster), pPixelBlock)
          v = pPixelBlock.PixelData(0)
          pPoint.Y = v(0, 0)
        End If

        'X value needs to be converted from meters to feet and altered for vertical exaggeration
        pPoint.X = pCurve.Length / 0.3048 / sglTheVertEx

        ''POINTS
        'pPTFeatBuff.Shape = pPoint
        'pPTFeatBuff.Value(pPTFCur.FindField("DIST_" & Math.Round(sglTheVertEx) & "X")) = pPoint.X
        'pPTFeatBuff.Value(pPTFCur.FindField("ELEVATION")) = pPoint.Y
        'pPTFeatBuff.Value(pPTFCur.FindField("ORDER")) = Math.Round(pCurve.Length + 1)
        'pPTFCur.InsertFeature(pPTFeatBuff)

        pPolyline.AddPoint(pPoint)
      End If
    End If

    If pPolyline.PointCount > 1 Then
      pPolyLineFeature = pPolyLineFC.CreateFeature
      pPolyLineFeature.Shape = pPolyline

      'add attribute values
      pPolyLineFeature.Value(pPolyLineFeature.Fields.FindField("XSEC")) = strLabel
      pPolyLineFeature.Value(pPolyLineFeature.Fields.FindField("UNIT")) = strRName

      pPolyLineFeature.Store()
    End If

    ''POINTS
    'pPTFCur.Flush()
    'Line2XYZMPoints = pPointFC
    'Comment out next line if using points:
    Line2XYZMPoints = pPolyLineFC

    'VB.Net having trouble with releasing the point cursor
    'pPTFCur = Nothing

    ''POINTS
    'System.Runtime.InteropServices.Marshal.FinalReleaseComObject(pPTFCur)

  End Function

  Private Function MakePnt(ByRef dX As Double, ByRef dY As Double) As IPnt
    MakePnt = New DblPnt
    MakePnt.SetCoords(dX, dY)
  End Function

  Private Function GetPixelPnt(ByRef pPoint As IPoint, ByRef pRasterProps As IRasterProps) As IPnt
    ' make a point based on the raster's row & column
    Dim pPnt As IPnt, PixelWidth As Double, PixelHeight As Double
    pPnt = pRasterProps.MeanCellSize
    PixelWidth = pPnt.X
    PixelHeight = pPnt.Y
    Dim dTop As Double, dLeft As Double
    dTop = pRasterProps.Extent.YMax
    dLeft = pRasterProps.Extent.XMin
    GetPixelPnt = New DblPnt
    GetPixelPnt.SetCoords( _
      Math.Round(Math.Abs(pPoint.X - dLeft - (0.5 * PixelWidth)) / PixelWidth), _
      Math.Round(Math.Abs(pPoint.Y - dTop + (0.5 * PixelHeight)) / PixelHeight))
  End Function

End Class