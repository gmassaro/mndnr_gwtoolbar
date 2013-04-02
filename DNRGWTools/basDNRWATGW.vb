Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.DataSourcesFile
Imports ESRI.ArcGIS.esriSystem
Imports System.Windows.Forms

Module basDNRWATGW

  ' Name:     basDNRWAT
  '
  ' Author:   Greg Massaro
  '           DNR Ecological and Water Resources
  '           500 Lafayette Road
  '           St. Paul, MN 55155
  '           greg.massaro@state.mn.us
  '           (651-259-5693)
  '
  ' Date: Jun 12 2008
  ' Revised by:  Greg Massaro
  ' Revision Date: Jan 25 2011
  ' Revisions: 
  ' -----------------------------------------------------------------------------
  ' Description:
  '           Runs specific code and forms related to the DNR Groundwater
  '           Toolbar Add-In for ArcGIS 10.x.
  '           Contains code common to the DNR Groundwater Toolbar.
  '
  ' Requires:
  ' Runs:     frmDNRWATStickDiagrams, frmDNRWATProfiler,
  '           frmDNRWATXYZCollector
  ' Run by:
  ' Returns:
  '==============================================================================
  Public g_blnRunning As Boolean = False

  Public Sub DNR_WAT_ExtractProfiles()
    Dim pfrmExtractProfiles As frmDNRWATProfiler
    pfrmExtractProfiles = New frmDNRWATProfiler

    'Clears the layer listbox of previous layers in the list.
    pfrmExtractProfiles.cboXSecLayer.Items.Clear()
    pfrmExtractProfiles.cboRasterLayer.Items.Clear()
    Dim pMap As IMap
    pMap = My.ArcMap.Document.FocusMap 'pMxDoc.FocusMap
    Dim pLayer As ILayer
    Dim pFLayer2 As IFeatureLayer2
    'Populates the layers listbox with the names of layers from the active
    '   data frame and then shows the user form.
    Dim i As Integer
    For i = 0 To pMap.LayerCount - 1
      pLayer = pMap.Layer(i)
      If TypeOf pLayer Is IFeatureLayer Then
        pFLayer2 = pLayer
        If (pFLayer2.ShapeType = esriGeometryType.esriGeometryPolyline) Then
          pfrmExtractProfiles.cboXSecLayer.Items.Add(pLayer.Name)
        End If
      ElseIf TypeOf pLayer Is IRasterLayer Then
        pfrmExtractProfiles.cboRasterLayer.Items.Add(pLayer.Name)
      End If
    Next i
    pfrmExtractProfiles.ShowDialog()
    pfrmExtractProfiles = Nothing
  End Sub

  Public Sub DNR_WAT_StickDiagrams()
    Dim pfrmStickDiagrams As frmDNRWATStickDiagrams
    pfrmStickDiagrams = New frmDNRWATStickDiagrams

    'Clears the layer combobox of previous layers in the list.
    pfrmStickDiagrams.cboWellLyr.Items.Clear()
    pfrmStickDiagrams.cboXSecLyr.Items.Clear()

    Dim pMap As IMap
    pMap = My.ArcMap.Document.FocusMap 'pMxDoc.FocusMap
    Dim pLayer As ILayer
    'Populates the layers combobox with the names of layers from the active
    '   data frame and then shows the user form.
    Dim i As Long
    For i = 0 To pMap.LayerCount - 1
      pLayer = pMap.Layer(i)
      If TypeOf pLayer Is IFeatureLayer Then
        pfrmStickDiagrams.cboWellLyr.Items.Add(pLayer.Name)
        pfrmStickDiagrams.cboXSecLyr.Items.Add(pLayer.Name)
      End If
    Next i
    pfrmStickDiagrams.ShowDialog()
    pfrmStickDiagrams = Nothing
  End Sub

  Public Sub DNR_WAT_XYZPoints()
    Dim pfrmXYZ As frmDNRWATXYZCollector
    pfrmXYZ = New frmDNRWATXYZCollector

    'Clears the layer listbox of previous layers in the list.
    pfrmXYZ.cboXSecLayer.Items.Clear()
    Dim pMap As IMap
    pMap = My.ArcMap.Document.FocusMap 'pMxDoc.FocusMap
    Dim pLayer As ILayer
    Dim pFLayer2 As IFeatureLayer2
    'Populates the layers listbox with the names of layers from the active
    '   data frame and then shows the user form.
    Dim i As Integer
    For i = 0 To pMap.LayerCount - 1
      pLayer = pMap.Layer(i)
      If TypeOf pLayer Is IFeatureLayer Then
        pFLayer2 = pLayer
        If (pFLayer2.ShapeType = esriGeometryType.esriGeometryPolyline) Then
          pfrmXYZ.cboXSecLayer.Items.Add(pLayer.Name)
        End If
      End If
    Next i
    pfrmXYZ.ShowDialog()
    pfrmXYZ = Nothing
  End Sub

  'Public Sub DNR_WAT_GetWellInfo()
  '    frm_dnr_wat_wellinfo.Show()
  'End Sub

  Public Function MakeFC(ByVal strPath As String, ByVal strName As String, ByVal GeoType As esriGeometryType, _
          ByVal pSpatialRef As ISpatialReference, ByVal sglTheVertEx As Single, _
          ByVal blnAskAgain As Boolean) As IFeatureClass
    Dim pWSF As IWorkspaceFactory
    pWSF = New ShapefileWorkspaceFactory

    Dim pFWS As IFeatureWorkspace
    pFWS = pWSF.OpenFromFile(strPath, 0)

    Dim pDS As IDataset
    On Error Resume Next
    pDS = pFWS.OpenFeatureClass(strName)
    On Error GoTo EH
    If Not pDS Is Nothing Then
      If blnAskAgain Then
        If MsgBox("Overwrite existing shapefiles?", vbYesNo, "Shapefile exists!") = vbNo Then
          MakeFC = Nothing
          pWSF = Nothing
          pFWS = Nothing
          pDS = Nothing
          Exit Function
        End If
      End If
      Debug.Print("deleting " & pDS.Name)
      pDS.Delete()
    End If

    Dim pFlds As IFields
    pFlds = MakeFCFields(GeoType, pSpatialRef, sglTheVertEx)

    'If MsgBox("Make New Shapefiles?", vbYesNo, "Shapefiles!") = vbYes Then
    MakeFC = pFWS.CreateFeatureClass(strName, pFlds, Nothing, Nothing, _
    esriFeatureType.esriFTSimple, "Shape", "")
    'End If
    pWSF = Nothing
    pFWS = Nothing
    pDS = Nothing

    Exit Function
EH:
    pWSF = Nothing
    pFWS = Nothing
    pDS = Nothing
    MsgBox(Err.Number & "  " & Err.Description)
    Debug.WriteLine("error in MakeFC: " & Err.Number & ", " & Err.Description)
  End Function

  Private Function MakeFCFields(ByVal GeoType As esriGeometryType, _
          ByVal pSpatialRef As ISpatialReference, ByVal sglTheVertEx As Single) As IFields
    Dim pFldsEdit As IFieldsEdit
    pFldsEdit = New Fields
    pFldsEdit.AddField(MakeField("OID", esriFieldType.esriFieldTypeOID))

    Dim pGeomDefEdit As IGeometryDefEdit
    pGeomDefEdit = New GeometryDef

    pGeomDefEdit.GeometryType_2 = GeoType
    pGeomDefEdit.SpatialReference_2 = pSpatialRef

    Dim pFldEdit As IFieldEdit
    pFldEdit = MakeField("Shape", esriFieldType.esriFieldTypeGeometry)
    pFldEdit.GeometryDef_2 = pGeomDefEdit
    pFldsEdit.AddField(pFldEdit)

    MakeFCFields = pFldsEdit
  End Function

  Public Function MakeField(ByVal strFldName As String, ByVal lType As esriFieldType) As IField
    Dim pFldEdit As IFieldEdit
    pFldEdit = New Field
    pFldEdit.Name_2 = strFldName
    pFldEdit.Type_2 = lType
    MakeField = pFldEdit
  End Function

  Public Sub UpdateProgress(ByVal sglProgress As Single, ByRef pfrmProgress As frmDNRWATProgressbar)
    pfrmProgress.lblPercent.Text = Math.Round(sglProgress, 1) & " % completed."
    If sglProgress > pfrmProgress.pbaProcess.Maximum Then
      pfrmProgress.pbaProcess.Value = pfrmProgress.pbaProcess.Maximum
    Else
      pfrmProgress.pbaProcess.Value = sglProgress
    End If
    Application.DoEvents()
  End Sub

  Public Function StripString(ByVal strBase As String) As String
    'Eliminate certain characters that cannot be used in filenames...
    Dim strModified As String, strChar As String
    Dim intLength As Integer, c As Integer
    intLength = Len(strBase)
    For c = 1 To intLength
      strChar = Mid$(strBase, c, 1)
      If strChar = "\" Or strChar = "/" Or strChar = ":" Or strChar = "*" Or strChar = "?" _
          Or strChar = """" Or strChar = "<" Or strChar = ">" Or strChar = "|" Then
        strModified = strModified
      Else : strModified = strModified & strChar
      End If
    Next c
    StripString = strModified
  End Function

End Module
