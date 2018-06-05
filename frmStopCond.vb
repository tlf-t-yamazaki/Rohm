

Imports LaserFront.Trimmer.DefWin32Fnc

Public Class frmStopCond


    '----- API定義 -----
    Private Declare Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpString As String, ByVal lpFileName As String) As Long 'V1.14.0.0②


    ''' <summary>
    ''' 設定内容の更新
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnUpdate_Click(sender As System.Object, e As System.EventArgs) Handles btnUpdate.Click
        Dim sSect As String                                                     ' セクション名
        Dim s As String
        Dim strMSG As String
        Dim sKey As String                                                     ' セクション名
        Dim ret As Integer

        ' 判定タイミングPlateの書き込み
        s = 0
        If CheckPlate.Checked = True Then
            s = 1
        End If
        JudgeNgRate.CheckTimmingPlate = CheckPlate.Checked
        sSect = "JUDGE"
        sKey = "TIMMING_PLATE"
        Call WritePrivateProfileString(sSect, sKey, s, LOT_COND_FILENAME)

        ' 判定タイミングBlockの書き込み
        s = 0
        If CheckBlock.Checked = True Then
            s = 1
        End If
        JudgeNgRate.CheckTimmingBlock = CheckBlock.Checked
        sSect = "JUDGE"
        sKey = "TIMMING_BLOCK"
        Call WritePrivateProfileString(sSect, sKey, s, LOT_COND_FILENAME)

        ' Yieldのチェック書き込み
        s = 0
        If CheckYeild.Checked = True Then
            s = 1
        End If
        JudgeNgRate.CheckYeld = CheckYeild.Checked
        sSect = "JUDGE"
        sKey = "CHECK_YIELD"
        Call WritePrivateProfileString(sSect, sKey, s, LOT_COND_FILENAME)

        ' OverRangeのチェック書き込み
        s = 0
        If CheckOverRange.Checked = True Then
            s = 1
        End If
        JudgeNgRate.CheckOverRange = CheckOverRange.Checked
        sSect = "JUDGE"
        sKey = "CHECK_OVERRANGE"
        Call WritePrivateProfileString(sSect, sKey, s, LOT_COND_FILENAME)

        ' Yieldの％書き込み
        s = ""
        If Trim(txtYield.Text) = "" Then
            If CheckYeild.Checked = True Then
                If (gSysPrm.stTMN.giMsgTyp = 0) Then
                    strMSG = "Yieldを入力してください。 "
                    MsgBox(strMSG)
                Else
                    strMSG = "Please input Yield. "
                    MsgBox(strMSG)
                End If
                Return
            End If
            txtYield.Text = "0"
        Else
        End If
        s = txtYield.Text
        ret = JudgeInputCheck(s)
        If ret <> cFRS_NORMAL Then
            If (gSysPrm.stTMN.giMsgTyp = 0) Then
                strMSG = "Yieldは０．０１～１００の範囲で入力してください。 "
                MsgBox(strMSG)
            Else
                strMSG = "Please input Yield.(0.01-100) "
                MsgBox(strMSG)
            End If
            Return
        End If

        JudgeNgRate.ValYield = Double.Parse(s)
        sSect = "JUDGE"
        sKey = "TEXT_YIELD"
        Call WritePrivateProfileString(sSect, sKey, s, LOT_COND_FILENAME)

        ' Open-NGの％書き込み
        s = ""
        If Trim(txtOverRange.Text) = "" Then
            If CheckOverRange.Checked = True Then
                If (gSysPrm.stTMN.giMsgTyp = 0) Then
                    strMSG = "Open-NGを入力してください。 "
                    MsgBox(strMSG)
                Else
                    strMSG = "Please input Open-NG. "
                    MsgBox(strMSG)
                End If
                Return
            End If
            txtOverRange.Text = "0"
        End If
        s = txtOverRange.Text
        ret = JudgeInputCheck(s)
        If ret <> cFRS_NORMAL Then
            If (gSysPrm.stTMN.giMsgTyp = 0) Then
                strMSG = "Open-NGは０．０１～１００の範囲で入力してください。 "
                MsgBox(strMSG)
            Else
                strMSG = "Please input Open-NG.(0.01-100) "
                MsgBox(strMSG)
            End If
            Return
        End If

        JudgeNgRate.ValOverRange = Double.Parse(s)
        sSect = "JUDGE"
        sKey = "TEXT_OVERRANGE"
        Call WritePrivateProfileString(sSect, sKey, s, LOT_COND_FILENAME)

        'Lot/Plate/Blockの判定書き込み
        If RadioPlate.Checked = True Then
            s = UNIT_PLATE
        Else
            s = UNIT_LOT
            'Else
            '    Block
            '    s = UNIT_BLOCK
        End If
        JudgeNgRate.SelectUnit = s
        sSect = "JUDGE"
        sKey = "JUDGE_UNIT"
        Call WritePrivateProfileString(sSect, sKey, s, LOT_COND_FILENAME)

        ' IT-Highのチェック書き込み
        s = 0
        If CheckITHi.Checked = True Then
            s = 1
        End If
        JudgeNgRate.CheckITHI = CheckITHi.Checked
        sSect = "JUDGE"
        sKey = "CHECK_ITHI"
        Call WritePrivateProfileString(sSect, sKey, s, LOT_COND_FILENAME)

        ' IT-Lowのチェック書き込み
        s = 0
        If CheckITLo.Checked = True Then
            s = 1
        End If
        JudgeNgRate.CheckITLO = CheckITLo.Checked
        sSect = "JUDGE"
        sKey = "CHECK_ITLO"
        Call WritePrivateProfileString(sSect, sKey, s, LOT_COND_FILENAME)

        ' FT-Highのチェック書き込み
        s = 0
        If CheckFTHi.Checked = True Then
            s = 1
        End If
        JudgeNgRate.CheckFTHI = CheckFTHi.Checked
        sSect = "JUDGE"
        sKey = "CHECK_FTHI"
        Call WritePrivateProfileString(sSect, sKey, s, LOT_COND_FILENAME)

        ' FT-Lowのチェック書き込み
        s = 0
        If CheckFTLo.Checked = True Then
            s = 1
        End If
        JudgeNgRate.CheckFTLO = CheckFTLo.Checked
        sSect = "JUDGE"
        sKey = "CHECK_FTLO"
        Call WritePrivateProfileString(sSect, sKey, s, LOT_COND_FILENAME)

        ' IT-HIの％書き込み
        s = ""
        If Trim(txtITHI.Text) = "" Then
            If CheckITHi.Checked = True Then
                If (gSysPrm.stTMN.giMsgTyp = 0) Then
                    strMSG = "IT-HIを入力してください。 "
                    MsgBox(strMSG)
                Else
                    strMSG = "Please input IT-HI "
                    MsgBox(strMSG)
                End If
                Return
            End If
        End If
        s = txtITHI.Text
        ret = JudgeInputCheck(s)
        If ret <> cFRS_NORMAL Then
            If (gSysPrm.stTMN.giMsgTyp = 0) Then
                strMSG = "IT-HIは０．０１～１００の範囲で入力してください。 "
                MsgBox(strMSG)
            Else
                strMSG = "Please input IT-HI.(0.01-100) "
                MsgBox(strMSG)
            End If
            Return
        End If
        JudgeNgRate.ValITHI = Double.Parse(s)
        sSect = "JUDGE"
        sKey = "TEXT_ITHI"
        Call WritePrivateProfileString(sSect, sKey, s, LOT_COND_FILENAME)

        ' IT-LOの％書き込み
        s = ""
        If Trim(txtITLO.Text) = "" Then
            If CheckITLo.Checked = True Then
                If (gSysPrm.stTMN.giMsgTyp = 0) Then
                    strMSG = "IT-LOを入力してください。 "
                    MsgBox(strMSG)
                Else
                    strMSG = "Please input IT-LO "
                    MsgBox(strMSG)
                End If
                Return
            End If
            txtITLO.Text = "0"
        End If
        s = txtITLO.Text
        ret = JudgeInputCheck(s)
        If ret <> cFRS_NORMAL Then
            If (gSysPrm.stTMN.giMsgTyp = 0) Then
                strMSG = "IT-LOは０．０１～１００の範囲で入力してください。 "
                MsgBox(strMSG)
            Else
                strMSG = "Please input IT-LO.(0.01-100) "
                MsgBox(strMSG)
            End If
            Return
        End If
        JudgeNgRate.ValITLO = Double.Parse(s)
        sSect = "JUDGE"
        sKey = "TEXT_ITLO"
        Call WritePrivateProfileString(sSect, sKey, s, LOT_COND_FILENAME)

        ' FT-HIの％書き込み
        s = ""
        If Trim(txtFTHI.Text) = "" Then
            If CheckFTHi.Checked = True Then
                If (gSysPrm.stTMN.giMsgTyp = 0) Then
                    strMSG = "FT-HIを入力してください。 "
                    MsgBox(strMSG)
                Else
                    strMSG = "Please input FT-HI "
                    MsgBox(strMSG)
                End If
                Return
            End If
            txtFTHI.Text = "0"
        End If
        s = txtFTHI.Text
        ret = JudgeInputCheck(s)
        If ret <> cFRS_NORMAL Then
            If (gSysPrm.stTMN.giMsgTyp = 0) Then
                strMSG = "FT-HIは０.０１～１００の範囲で入力してください。 "
                MsgBox(strMSG)
            Else
                strMSG = "Please input FT-HI.(0.01-100) "
                MsgBox(strMSG)
            End If
            Return
        End If
        JudgeNgRate.ValFTHI = Double.Parse(s)
        sSect = "JUDGE"
        sKey = "TEXT_FTHI"
        Call WritePrivateProfileString(sSect, sKey, s, LOT_COND_FILENAME)

        ' FT-LOの％書き込み
        s = ""
        If Trim(txtFTLO.Text) = "" Then
            If CheckFTLo.Checked = True Then
                If (gSysPrm.stTMN.giMsgTyp = 0) Then
                    strMSG = "FT-LOを入力してください。 "
                    MsgBox(strMSG)
                Else
                    strMSG = "Please input FT-LO "
                    MsgBox(strMSG)
                End If
                Return
            End If
            txtFTLO.Text = "0"
        End If
        s = txtFTLO.Text
        ret = JudgeInputCheck(s)
        If ret <> cFRS_NORMAL Then
            If (gSysPrm.stTMN.giMsgTyp = 0) Then
                strMSG = "FT-LOは０.０１～１００の範囲で入力してください。 "
                MsgBox(strMSG)
            Else
                strMSG = "Please input FT-LO.(0.01-100) "
                MsgBox(strMSG)
            End If
            Return
        End If
        JudgeNgRate.ValFTLO = Double.Parse(s)
        sSect = "JUDGE"
        sKey = "TEXT_FTLO"
        Call WritePrivateProfileString(sSect, sKey, s, LOT_COND_FILENAME)

        Close()

    End Sub

    ''' <summary>
    ''' フォームを閉じる
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click

        Close()

    End Sub

    Private Sub CheckBlock_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    ''' <summary>
    ''' フォームを表示するときの処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub frmStopCond_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown

        ReadLotStopData()

        If JudgeNgRate.CheckTimmingPlate = True Then
            CheckPlate.Checked = True
        End If

        If JudgeNgRate.CheckTimmingBlock = True Then
            CheckBlock.Checked = True
        End If

        If JudgeNgRate.CheckYeld = True Then
            CheckYeild.Checked = True
        End If

        If JudgeNgRate.CheckOverRange = True Then
            CheckOverRange.Checked = True
        End If

        If JudgeNgRate.CheckITLO = True Then
            CheckITLo.Checked = True
        End If

        If JudgeNgRate.CheckITHI = True Then
            CheckITHi.Checked = True
        End If

        If JudgeNgRate.CheckFTLO = True Then
            CheckFTLo.Checked = True
        End If

        If JudgeNgRate.CheckFTHI = True Then
            CheckFTHi.Checked = True
        End If

        txtYield.Text = JudgeNgRate.ValYield
        txtOverRange.Text = JudgeNgRate.ValOverRange

        txtITHI.Text = JudgeNgRate.ValITHI
        txtITLO.Text = JudgeNgRate.ValITLO

        txtFTHI.Text = JudgeNgRate.ValFTHI
        txtFTLO.Text = JudgeNgRate.ValFTLO

        'Lot/Plate/Blockの判定書き込み
        If JudgeNgRate.SelectUnit = UNIT_LOT Then
            RadioLot.Checked = True
        Else
            RadioPlate.Checked = True
        End If

    End Sub



    ''' <summary>
    ''' ０～１００入力値の判定
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function JudgeInputCheck(ByVal checkVal As String) As Integer
        Dim CompVal As Double
        JudgeInputCheck = cFRS_NORMAL
        CompVal = Double.Parse(checkVal)
        If (CompVal < 0.01) Or (100 < CompVal) Then
            JudgeInputCheck = cFRS_ERR_RST
        End If
        Return JudgeInputCheck

    End Function


End Class

