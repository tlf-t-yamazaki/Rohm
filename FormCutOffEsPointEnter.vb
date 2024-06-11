'==============================================================================
'   Description : カットオフ値とＥＳポイント値の設定画面
'
'　 2019/02/21 First Written by N.Arata(TLF) 'V4.7.3.5①
'
'==============================================================================
Imports LaserFront.Trimmer.DefWin32Fnc
Imports LaserFront.Trimmer.TrimData.DataManager

Public Class FormCutOffEsPointEnter
    Private CutOffExitFlag As Short

    ''' <summary>
    ''' ＯＫボタンクリック時・データ更新して終了
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles OKButton.Click
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
            MessageBox.Show("FormCutOffEsPointEnter.OKButton_Click() TRAP ERROR = " & ex.Message)
        End Try
    End Sub
    ''' <summary>
    ''' ＥＮＴＲＹボタンクリック時・データセーブしてエントリーして終了
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub EntryButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles EntryButton.Click
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
                                EntryFileName = (ENTRY_PATH & typQRDATAInfo.sLotNumber & "-" & typQRDATAInfo.sTargetValue.Replace(" ", "") & "_" & i.ToString("00") & sExtended)
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
                                    'V6.1.4.5①↓
                                Else
                                    MessageBox.Show(Me, ("エラー発生!!" & vbCrLf & System.IO.Path.GetFileName(EntryFileName) & "をエントリー出来ませんでした。"), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                    Call Form1.System1.OperationLogging(gSysPrm, "エラー発生!!" & System.IO.Path.GetFileName(EntryFileName) & "をエントリー出来ませんでした。", "MANUAL")
                                    'V6.1.4.5①↑
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
            MessageBox.Show("FormCutOffEsPointEnter.EntryButton_Click() TRAP ERROR = " & ex.Message)
        End Try
    End Sub
    ''' <summary>
    ''' ＣＡＮＣＥＬボタンクリック時・何もしないで終了
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CancelButton.Click
        Try
            Timer1.Stop()
            Me.Close()                              ' フォームを閉じる
        Catch ex As Exception
            MessageBox.Show("FormCutOffEsPointEnter.CancelButton_Click() TRAP ERROR = " & ex.Message)
        End Try
    End Sub


    Protected Sub ctlArray_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        Try
            '0～9と、バックスペース以外の時は、イベントをキャンセルする
            If (e.KeyChar < "0"c OrElse "9"c < e.KeyChar) AndAlso e.KeyChar <> "-"c AndAlso e.KeyChar <> "."c AndAlso e.KeyChar <> ControlChars.Back Then
                e.Handled = True
            End If
        Catch ex As Exception
            MessageBox.Show("FormCutOffEsPointEnter.ctlArray_KeyPress() TRAP ERROR = " & ex.Message)
        End Try
    End Sub

    Protected Sub ctlArray_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Try
            Dim KeyCode As Integer = e.KeyCode
            Debug.WriteLine("DATA=" & KeyCode.ToString())

            If ((KeyCode And Keys.KeyCode) = Keys.Return) AndAlso _
                ((KeyCode And (Keys.Alt Or Keys.Control)) = Keys.None) Then
                'Tabキーを押した時と同じ動作をさせる
                'Shiftキーが押されている時は、逆順にする
                Me.ProcessTabKey((KeyCode And Keys.Shift) <> Keys.Shift)
                '本来の処理はさせない
            End If
        Catch ex As Exception
            MessageBox.Show("FormCutOffEsPointEnter.ctlArray_KeyDown() TRAP ERROR = " & ex.Message)
        End Try

    End Sub
    ''' <summary>
    ''' 画面Load処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub FormCutOffEsPointEnter_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Try
            Const BLANK As String = "----"

            ' 表示を初期化する
            'V6.1.4.5①↓
            gDataUpdateFlag = False                      'カットオフ値、ＥＳポイント値入力画面でデータ更新した時Trueにする。
            If (giAppMode = APP_MODE_FINEADJ) Then
                EntryButton.Enabled = False
                EntryButton.Visible = False
            Else
                EntryButton.Enabled = True
                EntryButton.Visible = True
            End If
            'V6.1.4.5①↑

            CutOffExitFlag = cFRS_NORMAL
            LabelErrorMessage.Text = ""
            lblFrdNomVal.Text = BLANK
            With Me.tlpFirstResData
                For i As Integer = 1 To 6 Step 1
                    .Controls("lblFrdC" & i).Text = BLANK
                    .Controls("lblFrdE" & i).Text = BLANK
                    .Controls("lblFrdC" & i).Enabled = False
                    .Controls("lblFrdE" & i).Enabled = False
                    AddHandler .Controls("lblFrdC" & i).KeyPress, AddressOf ctlArray_KeyPress
                    AddHandler .Controls("lblFrdE" & i).KeyPress, AddressOf ctlArray_KeyPress
                    AddHandler .Controls("lblFrdC" & i).KeyDown, AddressOf ctlArray_KeyDown
                    AddHandler .Controls("lblFrdE" & i).KeyDown, AddressOf ctlArray_KeyDown
                Next i
            End With

            If (typResistorInfoArray Is Nothing) Then Return

            Dim tmpRes As ResistorInfo = typResistorInfoArray(1)
            lblFrdNomVal.Text = String.Format("{0:F5}", tmpRes.dblTrimTargetVal)

            Dim tmpCut() As CutList = tmpRes.ArrCut
            With Me.tlpFirstResData
                If (tmpCut IsNot Nothing) AndAlso (tmpRes.intCutCount <= tmpCut.Length) Then
                    Dim min As Integer = Math.Min(tmpRes.intCutCount, tmpCut.Length)
                    min = Math.Min(min, 6)
                    ' 最大6ｶｯﾄまで第1抵抗のｶｯﾄﾃﾞｰﾀを表示する
                    For i As Integer = 1 To min Step 1
                        .Controls("lblFrdC" & i).Text = String.Format("{0:F3}", tmpCut(i).dblCutOff)
                        .Controls("lblFrdC" & i).Enabled = True
                        If tmpCut(i).strCutType = CNS_CUTP_ES Or tmpCut(i).strCutType = CNS_CUTP_ES2 Then
                            .Controls("lblFrdE" & i).Text = String.Format("{0:F2}", tmpCut(i).dblESPoint)
                            .Controls("lblFrdE" & i).Enabled = True
                        End If
                    Next i
                End If
            End With
            Timer1.Start()

        Catch ex As Exception
            MessageBox.Show("FormCutOffEsPointEnter.FormCutOffEsPointEnter_Load() TRAP ERROR = " & ex.Message)
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
            Dim dInvalidData As Double = 9999999999.999
            Dim dCutOffMin As Double = GetPrivateProfileString_S("SPECIALFUNCTION", "CUTOFF_MIN", "C:\TRIM\tky.ini", dInvalidData.ToString())
            Dim dCutOffMax As Double = GetPrivateProfileString_S("SPECIALFUNCTION", "CUTOFF_MAX", "C:\TRIM\tky.ini", dInvalidData.ToString())
            Dim dEspointMin As Double = GetPrivateProfileString_S("SPECIALFUNCTION", "ESPOINT_MIN", "C:\TRIM\tky.ini", dInvalidData.ToString())
            Dim dEspointMax As Double = GetPrivateProfileString_S("SPECIALFUNCTION", "ESPOINT_MAX", "C:\TRIM\tky.ini", dInvalidData.ToString())
            Dim dData As Double
            Dim sErrorMessage As String = ""

            LabelErrorMessage.Text = sErrorMessage

            With Me.tlpFirstResData
                For i As Integer = 1 To 6 Step 1

                    If .Controls("lblFrdC" & i).Enabled = True Then
                        If Double.TryParse(.Controls("lblFrdC" & i).Text, dData) Then
                            dData = Math.Round(dData, 3, MidpointRounding.AwayFromZero)
                            .Controls("lblFrdC" & i).Text = dData.ToString("0.000")
                            If dCutOffMin <> dInvalidData AndAlso dData < dCutOffMin Then
                                sErrorMessage = "下限値[" & dCutOffMin.ToString() & "]以下の値が設定されています。"
                                .Controls("lblFrdC" & i).Focus()
                                DirectCast(.Controls("lblFrdC" & i), TextBox).SelectAll()
                                bRtn = False
                                Exit For
                            End If

                            If dCutOffMax <> dInvalidData AndAlso dData > dCutOffMax Then
                                sErrorMessage = "上限値[" & dCutOffMax.ToString() & "]以上の値が設定されています。"
                                .Controls("lblFrdC" & i).Focus()
                                DirectCast(.Controls("lblFrdC" & i), TextBox).SelectAll()
                                bRtn = False
                                Exit For
                            End If
                        Else
                            sErrorMessage = "数字に変換できません。"
                            .Controls("lblFrdC" & i).Focus()
                            DirectCast(.Controls("lblFrdC" & i), TextBox).SelectAll()
                            bRtn = False
                            Exit For
                        End If
                    End If

                    If .Controls("lblFrdE" & i).Enabled = True Then
                        If Double.TryParse(.Controls("lblFrdE" & i).Text, dData) Then
                            dData = Math.Round(dData, 2, MidpointRounding.AwayFromZero)
                            .Controls("lblFrdE" & i).Text = dData.ToString("0.00")
                            If dEspointMin <> dInvalidData AndAlso dData < dEspointMin Then
                                sErrorMessage = "下限値[" & dEspointMin.ToString() & "]以下の値が設定されています。"
                                .Controls("lblFrdE" & i).Focus()
                                DirectCast(.Controls("lblFrdE" & i), TextBox).SelectAll()
                                bRtn = False
                                Exit For
                            End If

                            If dEspointMax <> dInvalidData AndAlso dData > dEspointMax Then
                                sErrorMessage = "上限値[" & dEspointMax.ToString() & "]以上の値が設定されています。"
                                .Controls("lblFrdE" & i).Focus()
                                DirectCast(.Controls("lblFrdE" & i), TextBox).SelectAll()
                                bRtn = False
                                Exit For
                            End If
                        Else
                            sErrorMessage = "数字に変換できません。"
                            .Controls("lblFrdE" & i).Focus()
                            DirectCast(.Controls("lblFrdE" & i), TextBox).SelectAll()
                            bRtn = False
                            Exit For
                        End If
                    End If

                Next i
            End With

            LabelErrorMessage.Text = sErrorMessage

            Return (bRtn)

        Catch ex As Exception
            MessageBox.Show("FormCutOffEsPointEnter.DataUpDate() TRAP ERROR = " & ex.Message)
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
            Dim min As Integer

            If (typResistorInfoArray Is Nothing) Then
                Return (False)
            End If

            Dim tmpRes As ResistorInfo = typResistorInfoArray(1)

            Dim tmpCut() As CutList = tmpRes.ArrCut
            With Me.tlpFirstResData
                ' intCutCountは、実際のカット数、Lengthは、配列の最大数
                If (tmpCut IsNot Nothing) AndAlso (tmpRes.intCutCount <= tmpCut.Length) Then
                    min = Math.Min(tmpRes.intCutCount, tmpCut.Length)
                    min = Math.Min(min, 6)
                    ' 最大6ｶｯﾄまで第1抵抗のｶｯﾄﾃﾞｰﾀを表示する
                    For i As Integer = 1 To min Step 1
                        tmpCut(i).dblCutOff = Double.Parse(.Controls("lblFrdC" & i).Text)
                        If tmpCut(i).strCutType = CNS_CUTP_ES Or tmpCut(i).strCutType = CNS_CUTP_ES2 Then
                            tmpCut(i).dblESPoint = Double.Parse(.Controls("lblFrdE" & i).Text)
                        End If
                    Next i
                End If


                ' 抵抗数のリピート展開処理
                For Rn As Short = 2 To typPlateInfo.intResistCntInBlock              ' １ブロック内抵抗数分チェックする 
                    For i As Integer = 1 To min Step 1
                        typResistorInfoArray(Rn).ArrCut(i).dblCutOff = tmpCut(i).dblCutOff
                        If tmpCut(i).strCutType = CNS_CUTP_ES Or tmpCut(i).strCutType = CNS_CUTP_ES2 Then
                            typResistorInfoArray(Rn).ArrCut(i).dblESPoint = tmpCut(i).dblESPoint
                        End If
                    Next i
                Next

            End With

            Return (True)

        Catch ex As Exception
            MessageBox.Show("FormCutOffEsPointEnter.DataUpDate() TRAP ERROR = " & ex.Message)
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
            MessageBox.Show("FormCutOffEsPointEnter.Timer1_Tick() TRAP ERROR = " & ex.Message)
        End Try

    End Sub
End Class

'=============================== END OF FILE ===============================
