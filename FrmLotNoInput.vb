Imports TKY_ALL_SL432HW.My.Resources

Public Class frmLotNoInput

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Dim LotNo As String = ""

        If txtLotNo.Text <> "" Then
            LotNo = txtLotNo.Text
        Else
            'V5.0.0.1⑱
            'MsgBox(frmLotNoInput_001) ' Lot No を入力してください。
            LotNo = ""
            '            Return
            'V5.0.0.1⑱
        End If
        TrimData.SetLotNumber(LotNo)
        SimpleTrimmer.UpdateLotNo(LotNo)    'V4.0.0.0-42


        Me.Close()

    End Sub

    ''' <summary>
    ''' Lot番号入力画面の表示'V4.0.0.0-42
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub frmLotNoInput_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim cLotNo As String

        cLotNo = ""
        '常に最前面に表示する。
        Me.TopMost = True
        'V4.0.0.0-42
        SimpleTrimmer.GetLotNo(cLotNo)
        txtLotNo.Text = cLotNo
        'V4.0.0.0-42
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub frmLotNoInput_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown

        'V4.0.0.0-42
        txtLotNo.Focus()


    End Sub
End Class