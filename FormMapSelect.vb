'V1.13.0.0⑤ ファイルの追加
Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8①

Public Class FormMapSelect

    '''=========================================================================
    '''<summary>ブロック選択画面のClose処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub BtnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnClose.Click
        Me.Close()
    End Sub

    '''=========================================================================
    '''<summary>ブロック選択画面のLoad</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub FormMapSelect_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim FormsizeX As Integer
        Dim FormsizeY As Integer
        Dim x As Integer
        Dim y As Integer
        Dim localSelect(256, 256) As Integer
        Dim strMSG As String
        Try
            FormsizeX = Me.Width
            FormsizeY = Me.Height - 100

            For x = 0 To typPlateInfo.intBlockCntXDir - 1
                For y = 0 To typPlateInfo.intBlockCntYDir - 1
                    localSelect(x, y) = SelectBlock(x, y)
                Next y
            Next x

            '        Me.TrimMap1.TrimMapInitializeSelectable(0, 0, FormsizeX, FormsizeY, typPlateInfo.intBlockCntXDir, typPlateInfo.intBlockCntYDir, Convert.ToInt32(gSysPrm.stDEV.giBpDirXy), True)
            Me.TrimMap1.TrimMapInitializeSelectable(0, 0, FormsizeX, FormsizeY, typPlateInfo.intBlockCntXDir, typPlateInfo.intBlockCntYDir, Convert.ToInt32(gSysPrm.stDEV.giBpDirXy), True, localSelect, 0)
            '        Me.TrimMap1.TrimMapInitializeSelectable(0, 0, FormsizeX, FormsizeY, typPlateInfo.intBlockCntXDir, typPlateInfo.intBlockCntYDir, Convert.ToInt32(gSysPrm.stDEV.giBpDirXy), True, SelectBlock, 1)

            Me.TopMost = True
        Catch ex As Exception
            strMSG = "shinsyukuhosei - GetHoseiValueY() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub

    '''=========================================================================
    '''<summary>ブロック選択の決定</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Dim localSelect(256, 256) As Integer
        Dim x As Integer
        Dim y As Integer
        Dim strMSG As String
        Try
            Me.TrimMap1.GetBlockSelectedState(localSelect, 0)
            Me.Close()

            For x = 0 To typPlateInfo.intBlockCntXDir - 1
                For y = 0 To typPlateInfo.intBlockCntYDir - 1
                    SelectBlock(x, y) = localSelect(x, y)
                Next y
            Next x
        Catch ex As Exception
            strMSG = "shinsyukuhosei - GetHoseiValueY() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
End Class