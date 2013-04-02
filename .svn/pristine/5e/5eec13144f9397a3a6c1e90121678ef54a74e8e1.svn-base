Public Class DNRGetXYZPoints
  Inherits ESRI.ArcGIS.Desktop.AddIns.Button

  Public Sub New()

  End Sub

  Protected Overrides Sub OnClick()
    '
    '  launch XYZ Points code
    '
    basDNRWATGW.DNR_WAT_XYZPoints()
    My.ArcMap.Application.CurrentTool = Nothing
  End Sub

  Protected Overrides Sub OnUpdate()
    Enabled = My.ArcMap.Application IsNot Nothing
  End Sub
End Class
