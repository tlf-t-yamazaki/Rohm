'==============================================================================
'   Description : カットオフ値の設定画面（ＫＯＡ・ＴＫＹＮＥＴ用)）'V6.1.4.14①
'
'　 2021/07/13 First Written by N.Arata(TLF)
'
'==============================================================================
Imports LaserFront.Trimmer.DefWin32Fnc
Imports LaserFront.Trimmer.TrimData.DataManager
Imports TKY_ALL_SL432HW.My.Resources

Public Class FormCutOffEnter
    Private Const BLANK As String = "----"
    Private Const MAX_CUT_NUM As Integer = 10
    Private CutOffExitFlag As Short
    Private dInvalidData As Double = 9999999999.999
    Private dCutOffMin As Double
    Private dCutOffMax As Double
    Private ResistorInfoLocal(MaxCntResist) As ResistorInfo
    Private iNowEditResNo As Integer = 0

    ''' <summary>
    ''' ＯＫボタンクリック時・データ更新して終了
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub OKButton_Click(sender As Object, e As EventArgs) Handles OKButton.Click
        Try
            If DataCheck() Then
                If DataUpDate() Then
                    Form1.SetFirstResData()
                    gDataUpdateFlag = True                      'V6.1.4.5① カットオフ値、ＥＳポイント値入力画面でデータ更新した時Trueにする。
                    Timer1.Stop()
                    Me.Close()                              ' フォームを閉じる
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("FormCutOffEnter.OKButton_Click() TRAP ERROR = " & ex.Message)
        End Try
    End Sub
    ''' <summary>
    ''' ＥＮＴＲＹボタンクリック時・データセーブしてエントリーして終了
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub EntryButton_Click(sender As Object, e As EventArgs) Handles EntryButton.Click
        Try
            Dim rtn As Integer = cERR_TRAP
            Dim ENTRY_PATH As String = "C:\TRIMDATA\ENTRYLOT\"
            Dim sFilePath As String = System.IO.Path.GetDirectoryName(typPlateInfo.strDataName)
            Dim sFileName As String = System.IO.Path.GetFileNameWithoutExtension(typPlateInfo.strDataName)
            Dim sExtended As String = System.IO.Path.GetExtension(typPlateInfo.strDataName)

            If DataCheck() Then
                If DataUpDate() Then
                    rtn = File_Save(typPlateInfo.strDataName)                   ' トリムデータ書込み(SAVE)
                    If (rtn = cFRS_NORMAL) Then
                        gCmpTrimDataFlg = 0                                     ' データ更新フラグ(0=更新なし, 1=更新あり)
                        Dim SaveFileName As String = typPlateInfo.strDataName
                        Dim EntryFileName As String
                        For i As Integer = 1 To 99 Step 1
                            If typQRDATAInfo.bStatus = True Then                'QRコード情報が有る場合
                                EntryFileName = (ENTRY_PATH & typQRDATAInfo.sLotNumber & "_" & i.ToString("00") & sExtended)
                            Else
                                EntryFileName = (ENTRY_PATH & sFileName & "_" & i.ToString("00") & sExtended)
                            End If
                            If (False = IO.File.Exists(EntryFileName)) Then
                                If typQRDATAInfo.bStatus = False Then
                                    MessageBox.Show(Me, (SaveFileName & vbCrLf & vbTab & "↓" & vbCrLf & "QRコードの情報が保存されていません。" & vbCrLf & "元のファイル名でエントリします。"), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                End If
                                typPlateInfo.strDataName = EntryFileName        'トリミングデータ名設定
                                rtn = File_Save(EntryFileName)                  'トリムデータ書込み
                                typPlateInfo.strDataName = SaveFileName         'トリミングデータ名（現在名に戻す）
                                If (rtn = cFRS_NORMAL) Then
                                    ' エントリーファイルへ登録
                                    Using WS As New IO.StreamWriter("C:\TRIMDATA\ENTRYLOT\SAVE_ENTRY.TMP", True, System.Text.Encoding.GetEncoding("Shift-JIS"))
                                        WS.WriteLine(EntryFileName)
                                    End Using

                                    MessageBox.Show(Me, (System.IO.Path.GetFileName(EntryFileName) & "をエントリーしました。"), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                                Else
                                    MessageBox.Show(Me, ("エラー発生!!" & vbCrLf & System.IO.Path.GetFileName(EntryFileName) & "をエントリー出来ませんでした。"), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                    Call Form1.System1.OperationLogging(gSysPrm, "エラー発生!!" & System.IO.Path.GetFileName(EntryFileName) & "をエントリー出来ませんでした。", "MANUAL")
                                End If
                                Exit For
                            End If
                        Next
                    End If
                End If
            End If

            If rtn = cFRS_NORMAL Then                       ' 正常終了
                gDataUpdateFlag = True                      'V6.1.4.5① カットオフ値、ＥＳポイント値入力画面でデータ更新した時Trueにする。
                Form1.SetFirstResData()
                Timer1.Stop()
                Me.Close()                                  ' フォームを閉じる
            End If

        Catch ex As Exception
            MessageBox.Show("FormCutOffEnter.EntryButton_Click() TRAP ERROR = " & ex.Message)
        End Try
    End Sub
    ''' <summary>
    ''' ＣＡＮＣＥＬボタンクリック時・何もしないで終了
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelButton.Click
        Try
            Timer1.Stop()
            Me.Close()                              ' フォームを閉じる
        Catch ex As Exception
            MessageBox.Show("FormCutOffEnter.CancelButton_Click() TRAP ERROR = " & ex.Message)
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub ctlArray_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        Try
            '0～9と、バックスペース以外の時は、イベントをキャンセルする
            If (e.KeyChar < "0"c OrElse "9"c < e.KeyChar) AndAlso e.KeyChar <> "-"c AndAlso e.KeyChar <> "."c AndAlso e.KeyChar <> ControlChars.Back Then
                e.Handled = True
            End If
        Catch ex As Exception
            MessageBox.Show("FormCutOffEnter.ctlArray_KeyPress() TRAP ERROR = " & ex.Message)
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub ctlArray_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Try
            Dim KeyCode As Integer = e.KeyCode
            Debug.WriteLine("DATA=" & KeyCode.ToString())

            If ((KeyCode And Keys.KeyCode) = Keys.Return) AndAlso
                ((KeyCode And (Keys.Alt Or Keys.Control)) = Keys.None) Then
                'Tabキーを押した時と同じ動作をさせる
                'Shiftキーが押されている時は、逆順にする
                Me.ProcessTabKey((KeyCode And Keys.Shift) <> Keys.Shift)
                '本来の処理はさせない
            End If
        Catch ex As Exception
            MessageBox.Show("FormCutOffEnter.ctlArray_KeyDown() TRAP ERROR = " & ex.Message)
        End Try

    End Sub
    ''' <summary>
    ''' 画面Load処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub FormCutOffEnter_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try

            dCutOffMin = GetPrivateProfileString_S("SPECIALFUNCTION", "CUTOFF_MIN", "C:\TRIM\tky.ini", dInvalidData.ToString())
            dCutOffMax = GetPrivateProfileString_S("SPECIALFUNCTION", "CUTOFF_MAX", "C:\TRIM\tky.ini", dInvalidData.ToString())
            gDataUpdateFlag = False                      'カットオフ値、ＥＳポイント値入力画面でデータ更新した時Trueにする。
            If (giAppMode = APP_MODE_FINEADJ) Then
                EntryButton.Enabled = False
                EntryButton.Visible = False
            Else
                EntryButton.Enabled = True
                EntryButton.Visible = True
            End If

            CutOffExitFlag = cFRS_NORMAL
            LabelErrorMessage.Text = ""
            lblNETNomVal.Text = BLANK
            With Me.tlpFirstResDataNET
                For i As Integer = 1 To MAX_CUT_NUM Step 1
                    AddHandler .Controls("ResCut" & i).KeyPress, AddressOf ctlArray_KeyPress
                    AddHandler .Controls("ResCut" & i).KeyDown, AddressOf ctlArray_KeyDown
                Next i
            End With

            ' 抵抗データのワークエリアへのコピー
            Call CopyResistorInfoArray(ResistorInfoLocal, typResistorInfoArray)

            ComboBoxResNo.Items.Clear()
            For i As Integer = 1 To typPlateInfo.intResistCntInGroup Step 1
                ComboBoxResNo.Items.Add(i.ToString(" 0"))
            Next
            ComboBoxResNo.SelectedIndex = 0

            Me.ActiveControl = Me.ComboBoxResNo

            Timer1.Start()

        Catch ex As Exception

        End Try
    End Sub

    ''' <summary>
    ''' 入力データチェック処理
    ''' </summary>
    ''' <returns>True:正常 False：異常</returns>
    ''' <remarks></remarks>
    Private Function DataCheck() As Boolean
        Try
            Dim bRtn As Boolean = True
            Dim dData As Double
            Dim sErrorMessage As String = ""

            LabelErrorMessage.Text = sErrorMessage

            With Me.tlpFirstResDataNET
                For i As Integer = 1 To Math.Min(typResistorInfoArray(iNowEditResNo).intCutCount, MAX_CUT_NUM) Step 1

                    If .Controls("ResCut" & i).Enabled = True Then
                        If Double.TryParse(.Controls("ResCut" & i).Text, dData) Then
                            dData = Math.Round(dData, 3, MidpointRounding.AwayFromZero)
                            .Controls("ResCut" & i).Text = dData.ToString("0.000")
                            If dCutOffMin <> dInvalidData AndAlso dData < dCutOffMin Then
                                sErrorMessage = "下限値[" & dCutOffMin.ToString() & "]以下の値が設定されています。"
                                .Controls("ResCut" & i).Focus()
                                DirectCast(.Controls("ResCut" & i), TextBox).SelectAll()
                                bRtn = False
                                Exit For
                            End If

                            If dCutOffMax <> dInvalidData AndAlso dData > dCutOffMax Then
                                sErrorMessage = "上限値[" & dCutOffMax.ToString() & "]以上の値が設定されています。"
                                .Controls("ResCut" & i).Focus()
                                DirectCast(.Controls("ResCut" & i), TextBox).SelectAll()
                                bRtn = False
                                Exit For
                            End If
                        Else
                            sErrorMessage = "数字に変換できません。"
                            .Controls("ResCut" & i).Focus()
                            DirectCast(.Controls("ResCut" & i), TextBox).SelectAll()
                            bRtn = False
                            Exit For
                        End If
                        ResistorInfoLocal(iNowEditResNo).ArrCut(i).dblCutOff = dData
                    End If

                Next i
            End With

            LabelErrorMessage.Text = sErrorMessage

            Return (bRtn)

        Catch ex As Exception
            MessageBox.Show("FormCutOffEnter.DataUpDate() TRAP ERROR = " & ex.Message)
            Return (False)
        End Try
    End Function

    ''' <summary>
    ''' メモリー上のトリミングデータ更新処理
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataUpDate() As Boolean
        Try

            'intCircuitCntInBlock
            'ResistorInfoLocal(iNowEditResNo).ArrCut(i).dblCutOff = dData
            'typPlateInfo.intResistCntInGroup

            Dim iNowRes As Integer

            ' サーキット数分
            For iCircuit As Integer = 1 To typPlateInfo.intGroupCntInBlockXBp
                ' サーキット内抵抗数分
                For iRes As Integer = 1 To typPlateInfo.intResistCntInGroup
                    iNowRes = iRes + (iCircuit - 1) * typPlateInfo.intResistCntInGroup
                    For iCut As Integer = 1 To typResistorInfoArray(iNowRes).intCutCount
                        typResistorInfoArray(iNowRes).ArrCut(iCut).dblCutOff = ResistorInfoLocal(iRes).ArrCut(iCut).dblCutOff
                    Next
                Next
            Next iCircuit
            Return (True)

        Catch ex As Exception
            MessageBox.Show("FormCutOffEnter.DataUpDate() TRAP ERROR = " & ex.Message)
            Return (False)
        End Try
    End Function

    ''' <summary>
    ''' 非常停止を検出する
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Try
            Dim r As Short
            Timer1.Enabled = False
            If CutOffExitFlag = cFRS_NORMAL Then
                r = Form1.System1.SysErrChk_ForVBNET(APP_MODE_EDIT)
                If (r <> cFRS_NORMAL) Then
                    Call Form1.System1.OperationLogging(gSysPrm, "カットオフ値編集中非常停止受信=[" & r.ToString("0") & "]", "MANUAL")
                    CutOffExitFlag = r
                    Call Form1.AppEndDataSave()
                    Call Form1.AplicationForcedEnding()
                    Timer1.Stop()
                    End
                End If
                Timer1.Enabled = True
            End If

        Catch ex As Exception
            MessageBox.Show("FormCutOffEnter.Timer1_Tick() TRAP ERROR = " & ex.Message)
        End Try

    End Sub

    ''' <summary>
    ''' 引数の抵抗データに対応するカットオフ値の表示データ設定
    ''' </summary>
    ''' <param name="ResNo"></param>
    Private Sub DispDataSet(ByVal ResNo As Integer)
        Try
            Dim iCutNum As Integer = Math.Min(typResistorInfoArray(ResNo).intCutCount, MAX_CUT_NUM)

            If (typResistorInfoArray(ResNo).intTargetValType = TARGET_TYPE_RATIO) Then
                lblNETNomVal.Text = String.Format("{0:F5} ", typResistorInfoArray(ResNo).dblTrimTargetVal) + MSG_229 + "(" + NSG_230 + "=" + typResistorInfoArray(ResNo).intBaseResNo.ToString("0") + ")"
            Else
                lblNETNomVal.Text = String.Format("{0:F5}", typResistorInfoArray(ResNo).dblTrimTargetVal)
            End If

            With Me.tlpFirstResDataNET
                For j As Integer = 1 To iCutNum Step 1
                    .Controls("ResCut" & j).Text = String.Format("{0:F3}", ResistorInfoLocal(ResNo).ArrCut(j).dblCutOff)
                    .Controls("ResCut" & j).Enabled = True
                Next j
                For i As Integer = (iCutNum + 1) To MAX_CUT_NUM Step 1
                    .Controls("ResCut" & i).Text = BLANK
                    .Controls("ResCut" & i).Enabled = False
                Next i
            End With

            iNowEditResNo = ResNo

        Catch ex As Exception
            MessageBox.Show("FormCutOffEnter.DispDataSet() TRAP ERROR = " & ex.Message)
        End Try
    End Sub
    ''' <summary>
    ''' 抵抗番号変更時
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ComboBoxResNo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxResNo.SelectedIndexChanged
        Try

            If (ComboBoxResNo.SelectedIndex <> iNowEditResNo - 1) AndAlso DataCheck() Then
                Call DispDataSet(ComboBoxResNo.SelectedIndex + 1)
            Else
                ComboBoxResNo.SelectedIndex = iNowEditResNo - 1
            End If

        Catch ex As Exception
            MessageBox.Show("FormCutOffEnter.ComboBoxResNo_SelectedIndexChanged() TRAP ERROR = " & ex.Message)
        End Try
    End Sub
#Region "抵抗データのコピー処理"
    Public Sub CopyResistorInfo(ByRef ToRes As ResistorInfo, ByRef FromRes As ResistorInfo)

        Try
            ToRes = FromRes
            ToRes.ArrCut = DirectCast(ToRes.ArrCut.Clone(), CutList())

        Catch ex As Exception
            MessageBox.Show("FormCutOffEnter.CopyResistorInfo() TRAP ERROR = " & ex.Message)
        End Try

    End Sub
    Public Sub CopyResistorInfoArray(ByRef ToRes As ResistorInfo(), ByRef FromRes As ResistorInfo())

        Try
            For i As Integer = 1 To typPlateInfo.intResistCntInGroup
                Call CopyResistorInfo(ToRes(i), FromRes(i))
            Next

        Catch ex As Exception
            MessageBox.Show("FormCutOffEnter.CopyResistorInfoArray() TRAP ERROR = " & ex.Message)
        End Try

    End Sub
    Private Sub ComboBoxResNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles ComboBoxResNo.KeyPress
        Try
            Debug.Print("ComboBoxResNo_KeyPress" + ComboBoxResNo.Text)
            Debug.Print("ComboBoxResNo_KeyPress" + e.KeyChar)

            If (e.KeyChar = vbCr) Then
                Dim iNo As Integer
                If Integer.TryParse(ComboBoxResNo.Text, iNo) Then
                    If iNo <= typPlateInfo.intResistCntInGroup Then
                        ComboBoxResNo.SelectedIndex = iNo - 1
                        Me.ActiveControl = Me.ResCut1
                    Else
                        ComboBoxResNo.SelectedIndex = iNowEditResNo - 1
                    End If
                Else
                    ComboBoxResNo.SelectedIndex = iNowEditResNo - 1
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("FormCutOffEnter.ComboBoxResNo_KeyPress() TRAP ERROR = " & ex.Message)
        End Try
    End Sub
    Private Sub ComboBoxResNo_Leave(sender As Object, e As EventArgs) Handles ComboBoxResNo.Leave
        Try
            Debug.Print("ComboBoxResNo_Leave" + ComboBoxResNo.Text)
            Dim iNo As Integer
            If Integer.TryParse(ComboBoxResNo.Text, iNo) Then
                If iNo <= typPlateInfo.intResistCntInGroup Then
                    ComboBoxResNo.SelectedIndex = iNo - 1
                    'Me.ActiveControl = Me.ResCut1
                Else
                    ComboBoxResNo.SelectedIndex = iNowEditResNo - 1
                    Me.ActiveControl = Me.ComboBoxResNo
                End If
            Else
                ComboBoxResNo.SelectedIndex = iNowEditResNo - 1
                Me.ActiveControl = Me.ComboBoxResNo
            End If
        Catch ex As Exception
            MessageBox.Show("FormCutOffEnter.ComboBoxResNo_Leave() TRAP ERROR = " & ex.Message)
        End Try
    End Sub

#End Region
End Class
'=============================== END OF FILE ===============================
