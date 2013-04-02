Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.DataSourcesFile
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.Display
Imports ESRI.ArcGIS.DataSourcesGDB
Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Catalog
Imports System.Windows.Forms
Imports System.Drawing

Public Class frmDNRWATXYZCollector

  ' Name:     frmDNRWATXYZcollector
  '
  ' Author:   Greg Massaro
  '           DNR Ecological and Water Resources
  '           500 Lafayette Road
  '           St. Paul, MN 55155
  '           greg.massaro@state.mn.us
  '           (651-259-5693)
  '
  ' Date: Nov 01 2005
  ' Revised by:     Greg Massaro
  ' Revision Date:  Jan 25 2011
  ' Revisions:
  ' -----------------------------------------------------------------------------
  ' Description:
  '           Pulls the vertices from a group of line shapefiles in a given
  '           directory and assigns attributes to them and real world XYZ
  '           coordinates based on an input cross section layer in the active
  '           data frame.  The vertices are copied to a new shapefile.
  '           The line shapefiles represent cross sectional profiles in
  '           relative cartesian XY coordinates with a vertical exagerration.
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
  Private strTHEInPath As String, strTHEOutPath As String
  Private blnTHEOverWrite As Boolean
  Private sglTHEProgress As Single
  Private strTHEProfileName As String
  Private intTHEstart As Integer, intTHElength As Integer

  Private Sub frmDNRWATXYZCollector_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    g_blnRunning = False
    tboVertEx.Text = 50
  End Sub

  Private Sub cboXSecLayer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboXSecLayer.SelectedIndexChanged
    Call ComboBoxChange(cboXSecLayer)
  End Sub

  Private Sub ComboBoxChange(ByRef cbo As ComboBox)
    'Enables certain buttons/comboboxes on the userform based on inputs.
    If cbo.Text = "" Then
      Exit Sub
    End If

    Dim i As Long
    Dim pMap As IMap
    pMap = My.ArcMap.Document.FocusMap 'pMxDoc.FocusMap
    Dim pLayer As ILayer

    For i = 0 To pMap.LayerCount - 1
      pLayer = pMap.Layer(i)
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

          Call GetXYZButtonEnabler()
          Exit Sub
        End If
      End If
    Next i
    If cbo.Name = cboXSecLayer.Name Then
      lblXSecLabel.Enabled = False
      cboXSecLabel.Items.Clear()
      cboXSecLabel.Enabled = False
      cboXSecLabel.BackColor = SystemColors.Control
    End If
    comGetXYZPoints.Enabled = False
  End Sub

  Private Sub comBatchIn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles comBatchIn.Click
    Call FileBrowseButton(tboBatchIn, strTHEInPath)
  End Sub

  Private Sub tboProName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tboProName.KeyUp
    Call GetSelectedLabel()
  End Sub

  Private Sub tboProName_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tboProName.MouseUp
    Call GetSelectedLabel()
  End Sub

  Private Sub GetSelectedLabel()
    strTHEProfileName = tboProName.Text
    intTHEstart = tboProName.SelectionStart
    intTHElength = tboProName.SelectionLength

    tboSelText.Text = tboProName.SelectedText
    Call GetXYZButtonEnabler()
  End Sub

  Private Sub comOutput_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles comOutput.Click
    Call FileBrowseButton(tboOutput, strTHEOutPath)
  End Sub

  Private Sub tboVertEx_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tboVertEx.TextChanged
    Call GetXYZButtonEnabler()
  End Sub

  Private Sub comGetXYZPoints_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles comGetXYZPoints.Click
    Call XYZPointsFromLines()
  End Sub

  Private Sub FileBrowseButton(ByRef Tbo As TextBox, ByRef strPath As String)
    'This subroutine enables the user to select the output batch location from
    '  an user defined dialog.
    flddlgXYZPoints.ShowDialog()
    strPath = flddlgXYZPoints.SelectedPath
    Tbo.Text = strPath
    Call GetXYZButtonEnabler()
  End Sub

  Private Sub GetXYZButtonEnabler()
    If tboOutput.Text <> "" And tboBatchIn.Text <> "" And _
          IsNumeric(tboVertEx.Text) And cboXSecLayer.Text <> "" And _
          tboSelText.Text <> "" Then
      comGetXYZPoints.Enabled = True
      sglTheVertEx = tboVertEx.Text
    Else : comGetXYZPoints.Enabled = False
    End If
  End Sub

  Private Sub XYZPointsFromLines()
    On Error GoTo EH
    'If no layer is currently selected in the xsec combobox, then an error message
    '   is displayed and the sub is exited.
    If cboXSecLayer.SelectedIndex = -1 Then
      MsgBox("Please select the cross section layer.", , "Select a Layer")
      Exit Sub
    End If
    'If no field has been selected in the Xsec Label combobox, then an
    '   error message displays and the sub is exited.
    If cboXSecLabel.SelectedIndex = -1 Then
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

    Dim strRootFileName As String
    Dim pMap As IMap
    pMap = My.ArcMap.Document.FocusMap 'pMxDoc.FocusMap
    Dim pFlayer As IFeatureLayer
    Dim pFLayer2 As IFeatureLayer2
    Dim pFClass As IFeatureClass

    Dim pFSelection As IFeatureSelection
    Dim pSelectionSet As ISelectionSet

    Dim l As Long, i As Integer, ii As Integer, p As Long
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
      .Text = "Collecting XYZ Points"
      .lblPercent.Text = "0 % completed."
      .Show()
    End With
    Application.DoEvents()

    Dim sglNumFeatures As Single, sglStepUp As Single
    Dim sglNumSegments As Single, sglNumPoints As Single
    Dim sglNumFilesSkipped As Single

    sglTHEProgress = 0
    blnTHEOverWrite = True

    Dim colFiles As New Collection
    Call GetBatchFiles(colFiles)

    'Establish the shapefile workspace factory
    Dim pWorkspaceFactory As IWorkspaceFactory
    Dim pInFeatureWorkspace As IFeatureWorkspace

    'Create a new ShapefileWorkspaceFactory object and open the batch folder
    pWorkspaceFactory = New ShapefileWorkspaceFactory
    pInFeatureWorkspace = pWorkspaceFactory.OpenFromFile(strTHEInPath, 0)

    Dim strTempArray() As String
    Dim strFileName As String

    Dim pPointFClass As IFeatureClass

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
            Else : pFCur = pFClass.Search(Nothing, True)
              sglNumFeatures = pFClass.FeatureCount(Nothing)
            End If
            pFeature = pFCur.NextFeature
            Dim strLabel As String

            Dim pXSecLine As ICurve
            Dim dblDistance As Double

            Dim pSpatialRef As ISpatialReference
            pSpatialRef = New UnknownCoordinateSystem

            pPointFClass = basDNRWATGW.MakeFC(strTHEOutPath, pFlayer.Name & "_pts", _
              esriGeometryType.esriGeometryPoint, pSpatialRef, sglTheVertEx, True)
            If pPointFClass Is Nothing Then
              blnTHEOverWrite = False
            End If

            Dim pPointFCur As IFeatureCursor
            If blnTHEOverWrite Then
              'add two fields
              pPointFClass.AddField(basDNRWATGW.MakeField("UNIT", esriFieldType.esriFieldTypeString))
              pPointFClass.AddField(basDNRWATGW.MakeField("XSEC", esriFieldType.esriFieldTypeString))
              pPointFClass.AddField(basDNRWATGW.MakeField("Z_FEET", esriFieldType.esriFieldTypeDouble))
              pPointFClass.AddField(basDNRWATGW.MakeField("SOURCE", esriFieldType.esriFieldTypeString))
              pPointFClass.AddField(basDNRWATGW.MakeField("WELL_UNNUM", esriFieldType.esriFieldTypeString))


              pPointFCur = pPointFClass.Insert(True)
              Dim pPointFeatBuff As IFeatureBuffer
              pPointFeatBuff = pPointFClass.CreateFeatureBuffer

              Debug.WriteLine("")
              Debug.Print("Files that match cross sections:")

              Do Until pFeature Is Nothing
                'Make code here to open the shapefile that matches the label field, convert it to points,
                'then export X,Y,Z, and attribute fields to a new shapefile
                'assumes XSec line has only 2 vertices
                pXSecLine = pFeature.Shape
                dblDistance = pXSecLine.Length

                strLabel = pFeature.Value(pFCur.FindField(strLabelField))

                strRootFileName = "*\" & Microsoft.VisualBasic.Left(strTHEProfileName, intTHEstart) & _
                      basDNRWATGW.StripString(strLabel) & _
                      Microsoft.VisualBasic.Right(strTHEProfileName, Len(strTHEProfileName) - _
                      (intTHEstart + intTHElength)) & "*"

                'Loop through the batch of strat files
                For i = 1 To colFiles.Count
                  Dim pLineFCur As IFeatureCursor
                  If UCase(colFiles.Item(i)) Like UCase(strRootFileName) Then
                    Debug.Print(colFiles.Item(i))
                    pfrmProgress.lblProcess.Text = "Processing cross section: " & _
                      strLabel '& " (" & lngXSec & " of " & sglNumFeatures & ")"

                    strTempArray = Split(colFiles.Item(i), "\")
                    strFileName = strTempArray(UBound(strTempArray))

                    'Find the corresponding lithology stick diagram file
                    Dim strLixFName As String
                    For ii = 1 To colFiles.Count
                      If UCase(colFiles.Item(ii)) Like UCase("*_" & strLabel & "_lixpy*") Then
                        strTempArray = Split(colFiles.Item(ii), "\")
                        strLixFName = strTempArray(UBound(strTempArray))
                      End If
                    Next ii

                    Dim pLineFeature As IFeature
                    Dim pGeometry As IGeometry
                    Dim pStratLine As IPointCollection
                    Dim pPoint As IPoint, pPointOnLine As IPoint, pNewPoint As IPoint
                    Dim dblX As Double, dblZ As Double
                    Dim strSource As String, strWellNum As String

                    'Create a new FeatureLayer and assign the strat shapefile to it
                    Dim pInStratFLayer As IFeatureLayer
                    pInStratFLayer = New FeatureLayer
                    pInStratFLayer.FeatureClass = _
                      pInFeatureWorkspace.OpenFeatureClass(Microsoft.VisualBasic.Left(strFileName, Len(strFileName) - 4))

                    'Create another FeatureLayer and assign the lith stick diagram polys to it
                    Dim pInLithFLayer As IFeatureLayer
                    pInLithFLayer = New FeatureLayer
                    pInLithFLayer.FeatureClass = _
                      pInFeatureWorkspace.OpenFeatureClass(Microsoft.VisualBasic.Left(strLixFName, Len(strLixFName) - 4))

                    'Get first line feature from Strat Line shapefile...
                    pLineFCur = pInStratFLayer.FeatureClass.Search(Nothing, True)
                    pLineFeature = pLineFCur.NextFeature
                    sglNumSegments = pInStratFLayer.FeatureClass.FeatureCount(Nothing)

                    'then loop through all the line features in the Input strat file
                    Do Until pLineFeature Is Nothing
                      'get the points from the line feature
                      pGeometry = pLineFeature.ShapeCopy
                      pStratLine = pGeometry
                      sglNumPoints = pStratLine.PointCount

                      'Setup the temporary selectionset to keep track of wells
                      Dim pTempSelSet1 As ISelectionSet, pTempSelSet2 As ISelectionSet
                      Dim pTempSelSet As ISelectionSet
                      Dim pScratchWkSpFactory As IScratchWorkspaceFactory
                      pScratchWkSpFactory = New ScratchWorkspaceFactory

                      pTempSelSet1 = pInLithFLayer.FeatureClass.Select(Nothing, _
                          esriSelectionType.esriSelectionTypeHybrid, _
                          esriSelectionOption.esriSelectionOptionEmpty, _
                          pScratchWkSpFactory.DefaultScratchWorkspace)

                      'loop through each point and add it to the new shapefile as a XYZ point along
                      'with UNIT and LABEL attributes.
                      Dim pLithFCur As IFeatureCursor
                      Dim pLithFeature As IFeature
                      Dim pSpatialF As ISpatialFilter

                      For p = 0 To pStratLine.PointCount - 1
                        pPoint = pStratLine.Point(p)
                        pPointOnLine = New ESRI.ArcGIS.Geometry.Point
                        pNewPoint = New ESRI.ArcGIS.Geometry.Point

                        'Check to see if point falls within any well lith poly
                        pSpatialF = New SpatialFilter
                        With pSpatialF
                          .Geometry = pPoint
                          .GeometryField = "SHAPE"
                          .SpatialRel = esriSpatialRelEnum.esriSpatialRelWithin
                        End With

                        pLithFCur = pInLithFLayer.FeatureClass.Search(pSpatialF, True)
                        pLithFeature = pLithFCur.NextFeature

                        If pLithFeature Is Nothing Then
                          'Must convert from feet to meters then consider vertical exagerration
                          dblX = pPoint.X * 0.3048 * sglTheVertEx

                          If dblX > pXSecLine.Length Then
                            dblX = pXSecLine.Length
                          End If

                          pXSecLine.QueryPoint(esriSegmentExtension.esriNoExtension, dblX, False, pPointOnLine)

                          pNewPoint.X = pPointOnLine.X
                          pNewPoint.Y = pPointOnLine.Y

                          strSource = "stratline vertex"
                          strWellNum = ""
                        Else
                          'Get the X and Y UTM coords from the original well if non-zero
                          'otherwise use the stratline vertex instead.
                          If pLithFeature.Value(pLithFCur.FindField("UTMX")) = 0 _
                              And pLithFeature.Value(pLithFCur.FindField("UTMY")) = 0 Then
                            'Must convert from feet to meters then consider vertical exagerration
                            dblX = pPoint.X * 0.3048 * sglTheVertEx

                            If dblX > pXSecLine.Length Then
                              dblX = pXSecLine.Length
                            End If

                            pXSecLine.QueryPoint(esriSegmentExtension.esriNoExtension, dblX, False, pPointOnLine)

                            pNewPoint.X = pPointOnLine.X
                            pNewPoint.Y = pPointOnLine.Y

                            strSource = "stratline vertex"
                            strWellNum = pLithFeature.Value(pLithFCur.FindField("UNNUM")) & _
                                  " has no UTME, UTMN"
                          Else
                            'Add to selectionset of wells that contain a vertex.
                            pTempSelSet2 = pInLithFLayer.FeatureClass.Select(pSpatialF, _
                              esriSelectionType.esriSelectionTypeHybrid, esriSelectionOption.esriSelectionOptionNormal, _
                              pScratchWkSpFactory.DefaultScratchWorkspace)

                            pTempSelSet1.Combine(pTempSelSet2, esriSetOperation.esriSetUnion, pTempSelSet)
                            pTempSelSet1 = pTempSelSet

                            pNewPoint.X = pLithFeature.Value(pLithFCur.FindField("UTMX"))
                            pNewPoint.Y = pLithFeature.Value(pLithFCur.FindField("UTMY"))

                            strSource = "well contains stratline vertex"
                            strWellNum = pLithFeature.Value(pLithFCur.FindField("UNNUM"))
                          End If
                        End If

                        dblZ = pPoint.Y '(multiply here by 0.3048 if one wants meters)

                        pPointFeatBuff.Shape = pNewPoint
                        pPointFeatBuff.Value(pPointFCur.FindField("UNIT")) = _
                          pLineFeature.Value(pLineFCur.FindField("UNIT"))
                        pPointFeatBuff.Value(pPointFCur.FindField("XSEC")) = _
                          strLabel
                        pPointFeatBuff.Value(pPointFCur.FindField("Z_FEET")) = dblZ
                        pPointFeatBuff.Value(pPointFCur.FindField("SOURCE")) = _
                          strSource
                        pPointFeatBuff.Value(pPointFCur.FindField("WELL_UNNUM")) = _
                          strWellNum

                        pPointFCur.InsertFeature(pPointFeatBuff)

                        sglStepUp = 100 / sglNumFeatures / sglNumSegments / sglNumPoints
                        'Update the progress bar by one step increase.
                        sglTHEProgress = sglTHEProgress + sglStepUp
                        Call basDNRWATGW.UpdateProgress(sglTHEProgress, pfrmProgress)

                        'VB.Net may have trouble with releasing the cursor if so, use instead
                        'System.Runtime.InteropServices.Marshal.FinalReleaseComObject(pLithFCur)
                        pLithFCur = Nothing

                      Next p
                      pPointFCur.Flush()

                      'Now account for the intersections of the strat line with wells where
                      'no vertex exists.
                      Dim pAllWellsSet As ISelectionSet
                      Dim pSelWellSet As ISelectionSet

                      With pSpatialF
                        .Geometry = pStratLine
                        .SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects
                      End With

                      pAllWellsSet = pInLithFLayer.FeatureClass.Select(pSpatialF, _
                          esriSelectionType.esriSelectionTypeHybrid, _
                          esriSelectionOption.esriSelectionOptionNormal, _
                          pScratchWkSpFactory.DefaultScratchWorkspace)

                      pAllWellsSet.Combine(pTempSelSet1, esriSetOperation.esriSetDifference, pTempSelSet)
                      pSelWellSet = pTempSelSet

                      '                    MsgBox "Wells Intersecting this strat line: " & pAllWellsSet.Count & _
                      '                        vbCrLf & "Wells containing vertices from the strat line: " & _
                      '                        pTempSelSet1.Count & vbCrLf & "Wells with no vertices: " & pSelWellSet.Count

                      'Create cursor from the resulting selset of wells that do not contain
                      'a vertex from the strat line.
                      If pSelWellSet.Count <> 0 Then
                        pSelWellSet.Search(Nothing, False, pLithFCur)
                        pLithFeature = pLithFCur.NextFeature

                        'Cycle throught the lith polys that intersect
                        Do Until pLithFeature Is Nothing
                          'If the lithpoly has no utm x or y values then skip to next poly in the cursor.
                          If pLithFeature.Value(pLithFCur.FindField("UTMX")) <> 0 _
                              And pLithFeature.Value(pLithFCur.FindField("UTMY")) <> 0 Then
                            pNewPoint = New ESRI.ArcGIS.Geometry.Point
                            pNewPoint.X = pLithFeature.Value(pLithFCur.FindField("UTMX"))
                            pNewPoint.Y = pLithFeature.Value(pLithFCur.FindField("UTMY"))

                            'Find the y value of the line at the well's centroided x...
                            'this will be the z value for the new point
                            If Not pLithFeature.Shape.IsEmpty Then
                              Dim pArea As IArea, pLine As ICurve
                              Dim pLithPoly As IPolygon

                              pArea = pLithFeature.Shape
                              pLithPoly = pLithFeature.Shape

                              Dim dblYtop As Double, dblYbot As Double
                              dblYtop = pLithPoly.Envelope.YMax
                              dblYbot = pLithPoly.Envelope.YMin

                              If pArea.Area = 0 Then
                                dblX = (pLithPoly.Envelope.XMax + pLithPoly.Envelope.XMin) / 2
                              Else : dblX = pArea.Centroid.X
                              End If

                              pLine = pLineFeature.Shape

                              dblZ = GetY(pLine, dblX) '(multiply here by 0.3048 if one wants meters)

                              'Check to see if y value of the strat line still falls within the
                              'given lith poly when using the centroided x value.
                              If dblYbot <= dblZ <= dblYtop Then
                                strSource = "well intersects stratline"
                                strWellNum = pLithFeature.Value(pLithFCur.FindField("UNNUM"))

                                pPointFeatBuff.Shape = pNewPoint
                                pPointFeatBuff.Value(pPointFCur.FindField("UNIT")) = _
                                  pLineFeature.Value(pLineFCur.FindField("UNIT"))
                                pPointFeatBuff.Value(pPointFCur.FindField("XSEC")) = _
                                  strLabel
                                pPointFeatBuff.Value(pPointFCur.FindField("Z_FEET")) = dblZ
                                pPointFeatBuff.Value(pPointFCur.FindField("SOURCE")) = _
                                  strSource
                                pPointFeatBuff.Value(pPointFCur.FindField("WELL_UNNUM")) = _
                                  strWellNum

                                pPointFCur.InsertFeature(pPointFeatBuff)
                              End If
                            End If
                          End If
                          pLithFeature = pLithFCur.NextFeature
                        Loop
                      End If
                      'VB.Net may have trouble with releasing the cursor if so, use instead
                      'System.Runtime.InteropServices.Marshal.FinalReleaseComObject(pLithFCur)
                      pLithFCur = Nothing
                      pPointFCur.Flush()
                      pLineFeature = pLineFCur.NextFeature
                    Loop
                  Else : sglNumFilesSkipped = sglNumFilesSkipped + 1
                  End If
                  'VB.Net may have trouble with releasing the cursor if so, use instead
                  'System.Runtime.InteropServices.Marshal.FinalReleaseComObject(pLineFCur)
                  pLineFCur = Nothing
                Next i
                If sglNumFilesSkipped = colFiles.Count Then
                  sglStepUp = 100 / sglNumFeatures
                  'Update the progress bar by one step increase.
                  sglTHEProgress = sglTHEProgress + sglStepUp
                  Call basDNRWATGW.UpdateProgress(sglTHEProgress, pfrmProgress)
                End If
                pFeature = pFCur.NextFeature
                sglNumFilesSkipped = 0
              Loop
            End If
            'VB.Net may have trouble with releasing the cursor if so, do not use
            'pPointFCur = Nothing
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(pPointFCur)

          End If
        End If
      End If
      'VB.Net may have trouble with releasing the cursor if so, use instead
      'System.Runtime.InteropServices.Marshal.FinalReleaseComObject(pFCur)
      pFCur = Nothing
    Next l

    g_blnRunning = False
    pfrmProgress.Close()
    pfrmProgress = Nothing

    If blnTHEOverWrite Then
      Call Add2TheDataFrame(pPointFClass)
    Else : MsgBox("Bailing out of operation...", vbOKOnly, "Get XYZ Points")
    End If

    pPointFClass = Nothing
    Exit Sub

EH:
    pfrmProgress.Close()
    pfrmProgress = Nothing
    MsgBox(Err.Number & "  " & Err.Description)
  End Sub

  Private Sub GetBatchFiles(ByRef colFiles As Collection)
    Dim intNumDir As Integer
    'Change path as appropriate
    Call FindFiles(strTHEInPath, "*.shp", colFiles, intNumDir)
    Debug.Print("Total directories scanned: " & intNumDir)
    Debug.Print("Total files found: " & colFiles.Count)
    Dim i As Integer
    For i = 1 To colFiles.Count
      Debug.Print(colFiles.Item(i))
    Next
  End Sub

  Private Sub FindFiles(ByVal strFol As String, ByRef strFileExtension As String, _
          ByRef colFiles As Collection, ByRef intNumDir As Integer)

    Dim theFileSysObj As Object
    theFileSysObj = CreateObject("Scripting.FileSystemObject")

    Dim curFolder
    Dim strFileName As String
    curFolder = theFileSysObj.GetFolder(strFol)

    'Get first file matching pattern
    strFileName = Dir(theFileSysObj.BuildPath(curFolder.Path, strFileExtension), vbNormal Or _
      vbHidden Or vbSystem Or vbReadOnly)
    'Loop through all files returned
    While Len(strFileName) <> 0
      'add name to collection
      colFiles.Add(theFileSysObj.BuildPath(curFolder.Path, strFileName))
      'Get next file matching pattern
      strFileName = Dir()
      Application.DoEvents()
    End While

    '  'Do this recursively for all sub folders
    '  Dim subFolder
    '  intNumDir = intNumDir + 1
    '  If curFolder.SubFolders.Count > 0 Then
    '    For Each subFolder In curFolder.SubFolders
    '      DoEvents
    '      FindFiles subFolder.Path, strFileExtension, colFiles, intNumDir
    '    Next
    '  End If
  End Sub

  Private Sub Add2TheDataFrame(ByRef pAddFClass As IFeatureClass)
    'Prompts the user if the new shapefile should be added to the active data frame.
    Dim pMxDoc As IMxDocument
    pMxDoc = My.ArcMap.Document
    Dim pMap As IMap
    Dim pLayer As ILayer
    Dim pFlayer As IFeatureLayer
    Dim ResponseDataFrame
    '  'The following commented lines are for adding the shapefile to a new data frame...
    '  Dim pCommandBars As ICommandBars
    '  Set pCommandBars = ThisDocument.CommandBars
    '  Dim pCommandItem As ICommandItem
    '  Set pCommandItem = pCommandBars.Find(ArcId.PageLayout_NewMap)
    ResponseDataFrame = MsgBox("Add the output shapefile to the data frame?", _
          vbYesNo, "Add Shapefile")
    If ResponseDataFrame = vbYes Then
      '    pCommandItem.Execute
      pMap = My.ArcMap.Document.FocusMap 'pMxDoc.FocusMap

      pFlayer = New FeatureLayer
      pFlayer.FeatureClass = pAddFClass
      pLayer = pFlayer
      pLayer.Name = pAddFClass.AliasName
      pMap.AddLayer(pFlayer)

      pMxDoc.ActiveView.Refresh()
      pMxDoc.UpdateContents()
    End If
  End Sub

  Function GetY(ByRef pPolyline As IPolyline, ByRef X As Double) As Double
    Dim pPointColl As IPointCollection
    pPointColl = New Polyline
    pPointColl.AddPoint(MakePoint(X, pPolyline.Envelope.LowerLeft.Y, pPolyline.SpatialReference))
    pPointColl.AddPoint(MakePoint(X, pPolyline.Envelope.UpperRight.Y, pPolyline.SpatialReference))

    Dim pTopoOp As ITopologicalOperator
    pTopoOp = pPointColl

    Dim pGeom As IGeometry
    pGeom = pTopoOp.Intersect(pPolyline, esriGeometryDimension.esriGeometry0Dimension)
    If Not pGeom.IsEmpty Then
      Dim pIntersections As IPointCollection
      pIntersections = pGeom
      If pIntersections.PointCount > 0 Then
        GetY = pIntersections.Point(0).Y
      End If
    End If
  End Function

  Function MakePoint(ByRef X As Double, ByRef Y As Double, ByRef pSR As ISpatialReference) As IPoint
    MakePoint = New ESRI.ArcGIS.Geometry.Point
    MakePoint.PutCoords(X, Y)
    MakePoint.SpatialReference = pSR
  End Function

  Private Sub comProName_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles comProName.Click
    Dim strCaption As String
    strCaption = "Select example profile name"
    tboProName.Text = GetExampleProfile(strCaption, tboProName)
  End Sub

  Private Function GetExampleProfile(ByRef strCaption As String, ByRef tboIn As TextBox) As String
    Dim pGxDialog As IGxDialog
    pGxDialog = New GxDialog
    Dim pGxObjFilter As IGxObjectFilter
    'Specifies Feature classes as the filter (SHPs, coverages, feature classes, etc...).
    pGxObjFilter = New GxFilterFeatureClasses
    Dim blnOK As Boolean
    Dim pEnumGxObject As IEnumGxObject
    Dim pGxObject As IGxObject
    With pGxDialog
      .Title = strCaption
      .ButtonCaption = "Open"
      If strTHEInPath <> "" Then
        .StartingLocation = strTHEInPath
      End If
      .ObjectFilter = pGxObjFilter
      blnOK = .DoModalOpen(0, pEnumGxObject)
    End With
    If blnOK Then
      pGxObject = pEnumGxObject.Next
      'If the user selects nothing, then the textbox is reset to contain nothing.
      If pGxObject Is Nothing Then
        tboIn.Text = ""
        Exit Function
      End If
      GetExampleProfile = pGxObject.Name
      tboIn.Text = pGxObject.FullName
    End If
  End Function

End Class