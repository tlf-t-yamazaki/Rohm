Public Class frmMaintenance
    Inherits System.Windows.Forms.Form
    Implements ICommonMethods           'V6.0.0.0⑪

    Private Sub frmMaintenance_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        '常に最前面に表示する。
        Me.TopMost = True

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Close()
    End Sub

#Region "インターフェース実装(このフォームではNOP)"
    ''' <summary></summary>
    ''' <returns></returns>
    ''' <remarks>'V6.0.0.0⑬</remarks>
    Public Function Execute() As Integer Implements ICommonMethods.Execute
        ' DO NOTHING
    End Function

    ''' <summary></summary>
    ''' <param name="e"></param>
    ''' <remarks>'V6.0.0.0⑪</remarks>
    Public Sub JogKeyDown(e As KeyEventArgs) Implements ICommonMethods.JogKeyDown
        ' DO NOTHING
    End Sub

    ''' <summary></summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Sub JogKeyUp(e As KeyEventArgs) Implements ICommonMethods.JogKeyUp
        ' DO NOTHING
    End Sub

    ''' <summary></summary>
    ''' <param name="distanceX"></param>
    ''' <param name="distanceY"></param>
    ''' <remarks>'V6.0.0.0⑪</remarks>
    Public Sub MoveToCenter(distanceX As Decimal, distanceY As Decimal) _
        Implements ICommonMethods.MoveToCenter
        ' DO NOTHING
    End Sub
#End Region

End Class

