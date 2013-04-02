Public Class DNRStickDiagrams
  Inherits ESRI.ArcGIS.Desktop.AddIns.Button

  Public Sub New()

  End Sub

  Protected Overrides Sub OnClick()
    '
    '  launch Stick Diagrams code
    '
    basDNRWATGW.DNR_WAT_StickDiagrams()
    My.ArcMap.Application.CurrentTool = Nothing
  End Sub

  Protected Overrides Sub OnUpdate()
    Enabled = My.ArcMap.Application IsNot Nothing
  End Sub
End Class
