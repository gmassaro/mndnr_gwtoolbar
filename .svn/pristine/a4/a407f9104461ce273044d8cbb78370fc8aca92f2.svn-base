Public Class frmDNRWATProgressbar

  ' Name:     frmDNRWATProgressbar
  '
  ' Author:   Greg Massaro
  '           DNR Ecological and Waters Resources
  '           500 Lafayette Road
  '           St. Paul, MN 55155
  '           greg.massaro@state.mn.us
  '           (651-259-5693)
  '
  ' Date: Jun 10 2008
  ' Revised by:
  ' Revision Date:
  ' Revisions:
  ' -----------------------------------------------------------------------------
  ' Description:
  '           Displays a progress bar and label which are updated for the user.
  '
  '
  ' Requires:
  ' Runs:
  ' Run by:   frmDNRWATStickDiagrams, frmDNRWATProfiler, frmDNRWATXYZCollector,
  '           basDNRWATGW
  ' Returns:
  '==============================================================================
  '

  Private Sub frmDNRWATProgressbar_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
    'Prevent user from closing with the Close box in the title bar.
    If g_blnRunning Then
      e.Cancel = True
    Else : e.Cancel = False
    End If
  End Sub
End Class