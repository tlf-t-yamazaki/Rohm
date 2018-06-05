'===============================================================================
'   Description  : 原点復帰画面処理
'
'   Copyright(C) : OMRON LASERFRONT INC. 2011
'
'===============================================================================

Imports LaserFront.Trimmer.DllSystem    'V4.4.0.0-0
Imports LaserFront.Trimmer.DefTrimFnc
Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8①
Imports TKY_ALL_SL432HW.My.Resources    'V4.4.0.0-0

Public Class FrmReset

#Region "【変数定義】"
    '===========================================================================
    '   変数定義
    '===========================================================================
    '-------------------------------------------------------------------------------
    '   ランプ ON/OFF制御用ランプ番号(コンソール制御)
    '-------------------------------------------------------------------------------
    Public Const LAMP_START As Short = 0                            ' STARTランプ
    Public Const LAMP_RESET As Short = 1                            ' RESETランプ
    Public Const LAMP_PRB As Short = 2                              ' Zランプ
    Public Const LAMP_HALT As Short = 5                             ' HALTランプ
    Public Const LATCH_CLR As Short = 11                            ' B11 : カバー開ラッチクリア

    '-------------------------------------------------------------------------------
    '   インターロック状態
    '-------------------------------------------------------------------------------
    '----- インターロック状態 -----
    Public Const INTERLOCK_STS_DISABLE_NO As Integer = 0            ' インターロック状態（解除なし）
    Public Const INTERLOCK_STS_DISABLE_PART As Integer = 1          ' インターロック一部解除（ステージ動作可能）
    Public Const INTERLOCK_STS_DISABLE_FULL As Integer = 2          ' インターロック全解除

    '----- アクチュエータ入力ビット -----
    Public Const BIT_SLIDE_COVER_OPEN As UShort = &H1               ' スライドカバー開(=1)
    Public Const BIT_SLIDE_COVER_CLOSE As UShort = &H2              ' スライドカバー閉(=1)
    Public Const BIT_SLIDE_COVER_MOVING As UShort = &H4             ' スライドカバー動作中(=1)
    Public Const BIT_SOURCE_AIR_CHECK As UShort = &H8               ' 供給元エアー：0/1=異常/正常
    Public Const BIT_MAIN_COVER_OPENCLOSE As UShort = &H10          ' 固定カバー：0/1=開/閉
    Public Const BIT_COVER_OPEN_RATCH As UShort = &H20              ' カバー開ラッチ
    Public Const BIT_INTERLOCK_NO1_RELEASE As UShort = &H100        ' インターロック解除1：0/1=無効/有効
    Public Const BIT_INTERLOCK_NO2_RELEASE As UShort = &H200        ' インターロック解除2：0/1=無効/有効
    Public Const BIT_EMERGENCY_STATUS_ONOFF As UShort = &H400       ' 非常停止状態：0/1=異常/正常 (※非常停止はH/Wが落ちるので返って来ない)

    '-------------------------------------------------------------------------------
    '   その他
    '-------------------------------------------------------------------------------
    '----- Formの幅/高さ -----
    Private Const WIDTH_NOMAL As Integer = 570                      ' Formの幅
    Private Const WEIGHT_NOMAL As Integer = 203                     ' Formの高さ(通常モード)
    'Private Const WEIGHT_LDALM As Integer = 460                    ' Formの高さ(ローダアラームモード)
    Private Const WEIGHT_LDALM As Integer = 203 + 129 + 460         ' Formの高さ(ローダアラームモード) ###161
    Private Const WEIGHT_LDALM2 As Integer = 203 + 129 + 2          ' Formの高さ(解除ボタン表示モード) ###161

    Private stSzNML As System.Drawing.Size = New System.Drawing.Size(WIDTH_NOMAL, WEIGHT_NOMAL)
    Private stSzLDE As System.Drawing.Size = New System.Drawing.Size(WIDTH_NOMAL, WEIGHT_LDALM)
    Private stSzLDE2 As System.Drawing.Size = New System.Drawing.Size(WIDTH_NOMAL, WEIGHT_LDALM2) ' ###161

    '----- Cancelボタン表示位置 -----                               '###073
    Private LocCanBtnOrg As System.Drawing.Point = New System.Drawing.Point(215, 163)
    Private LocCanBtn2 As System.Drawing.Point = New System.Drawing.Point(303, 163)

    '----- 変数定義 -----
    Private mExitFlag As Integer                                    ' 終了フラグ
    Private gMode As Integer                                        ' 処理モード
    'Private ObjSys As Object                                        ' OcxSystemオブジェクト
    Private ObjSys As SystemNET                                     ' OcxSystemオブジェクト           'V4.4.0.0-0

    '----- ローダアラーム情報 -----
    Private AlarmCount As Integer
    Private strLoaderAlarm(LALARM_COUNT) As String                  ' アラーム文字列
    Private strLoaderAlarmInfo(LALARM_COUNT) As String              ' アラーム情報1
    Private strLoaderAlarmExec(LALARM_COUNT) As String              ' アラーム情報(対策)

    '----- 指定メッセージ表示用 -----  ###089
    Private Const MSGARY_NO As Integer = 3                          ' 表示メッセージの最大数
    Private DspWaitKey As Integer                                   ' WaitKey 
    Private DspBtnDsp As Boolean                                    ' ボタン表示する/しない
    Private strMsgAry(MSGARY_NO) As String                          ' 表示メッセージ１－３
    'V6.0.0.0⑱    Private ColColAry(MSGARY_NO) As Object                          ' メッセージ色１－３
    Private ColColAry(MSGARY_NO) As Color                          ' メッセージ色１－３              'V6.0.0.0⑱

#End Region

#Region "【メソッド定義】"
#Region "終了結果を返す"
    '''=========================================================================
    ''' <summary>終了結果を返す</summary>
    ''' <returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Public ReadOnly Property sGetReturn() As Integer
        Get
            Return (mExitFlag)
        End Get
    End Property
#End Region

#Region "ShowDialogメソッドに独自の引数を追加する"
    '''=========================================================================
    ''' <summary>ShowDialogメソッドに独自の引数を追加する</summary>
    ''' <param name="Owner">    (INP)未使用</param>
    ''' <param name="iGmode">   (INP)処理モード</param>
    ''' <param name="ObjSystem">(INP)OcxSystemオブジェクト</param>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Overloads Sub ShowDialog(ByVal Owner As IWin32Window, ByVal iGmode As Integer, ByVal ObjSystem As SystemNET) 'V4.4.0.0-0
        'Public Overloads Sub ShowDialog(ByVal Owner As IWin32Window, ByVal iGmode As Integer, ByVal ObjSystem As Object)

        Dim strMSG As String

        Try
            ' 初期処理
            Console.WriteLine("パラメータ = " + gMode.ToString)
            mExitFlag = -1                                          ' 終了フラグ = 初期化
            gMode = iGmode                                          ' 処理モード
            ObjSys = ObjSystem                                      ' OcxSystemオブジェクト
            LblCaption.Text = ""
            Label1.Text = ""
            Label2.Text = ""
            Call SetControlName()                                   ' ###186 ボタン名等を設定する(日本語/英語)

            ' 画面表示
            Me.Size = stSzNML                                       ' Formの幅/高さを通常モード用にする
            Me.ShowDialog()                                         ' 画面表示
            Me.BringToFront()                                       ' 最前面に表示 
            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FrmReset.ShowDialog() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "ShowDialogメソッドに独自の引数を追加する(指定メッセージ表示用)"
    '''=========================================================================
    ''' <summary>ShowDialogメソッドに独自の引数を追加する(指定メッセージ表示用) ###089</summary>
    ''' <param name="Owner">    (INP)未使用</param>
    ''' <param name="iGmode">   (INP)処理モード</param>
    ''' <param name="ObjSystem">(INP)OcxSystemオブジェクト</param>
    ''' <param name="MsgAry">   (INP)表示メッセージ１－３</param>
    ''' <param name="ColAry">   (INP)メッセージ色１－３</param>
    ''' <param name="Md">       (INP)cFRS_ERR_START                = STARTキー押下待ち
    '''                              cFRS_ERR_RST                  = RESETキー押下待ち
    '''                              cFRS_ERR_START + cFRS_ERR_RST = START/RESETキー押下待ち</param>
    ''' <param name="BtnDsp">   (INP)ボタン表示する/しない</param>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Overloads Sub ShowDialog(ByVal Owner As IWin32Window, ByVal iGmode As Integer, ByVal ObjSystem As SystemNET, _
                                    ByVal MsgAry() As String, ByVal ColAry() As Color, ByVal Md As Integer, ByVal BtnDsp As Boolean) 'V6.0.0.0⑱
        'Public Overloads Sub ShowDialog(ByVal Owner As IWin32Window, ByVal iGmode As Integer, ByVal ObjSystem As Object, _
        '                                ByVal MsgAry() As String, ByVal ColAry() As Object, ByVal Md As Integer, ByVal BtnDsp As Boolean)

        Dim Idx As Integer
        Dim strMSG As String

        Try
            ' 初期処理
            Console.WriteLine("パラメータ = " + gMode.ToString)
            mExitFlag = -1                                              ' 終了フラグ = 初期化
            gMode = iGmode                                              ' 処理モード
            ObjSys = ObjSystem                                          ' OcxSystemオブジェクト
            LblCaption.Text = ""
            Label1.Text = ""
            Label2.Text = ""
            Call SetControlName()                                       ' ###186 ボタン名等を設定する(日本語/英語)

            ' パラメータを取得する
            For Idx = 0 To (MSGARY_NO - 1)
                strMsgAry(Idx) = ""
                ColColAry(Idx) = System.Drawing.SystemColors.ControlText
            Next Idx
            DspWaitKey = Md                                             ' WaitKey
            DspBtnDsp = BtnDsp                                          ' ボタン表示する/しない

            ' 表示メッセージ１－３
            For Idx = 0 To (MsgAry.Length - 1)
                If (Idx > (MSGARY_NO - 1)) Then Exit For
                strMsgAry(Idx) = MsgAry(Idx)
            Next Idx

            ' メッセージ色１－３
            For Idx = 0 To (ColAry.Length - 1)
                If (Idx > (MSGARY_NO - 1)) Then Exit For
                ColColAry(Idx) = ColAry(Idx)
            Next Idx

            '----- V4.11.0.0⑥↓ (WALSIN殿SL436S対応) -----
            ' MG1 Downボタンを表示する
            BtnMg1Down.Visible = False
            If (strMsgAry(0) = MSG_LOADER_50) Then
                BtnMg1Down.Visible = True
            End If
            '----- V4.11.0.0⑥↑ -----

            ' 画面表示
            Me.Size = stSzNML                                           ' Formの幅/高さを通常モード用にする
            Me.ShowDialog()                                             ' 画面表示
            Me.BringToFront()                                           ' 最前面に表示 
            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FrmReset.ShowDialog() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- ###186↓ -----
#Region "ボタン名等を設定する(日本語/英語)"
    '''=========================================================================
    ''' <summary>ボタン名等を設定する(日本語/英語)</summary>
    '''=========================================================================
    Private Sub SetControlName()

        Dim strMSG As String

        Try
            ' ボタン名を設定する(日本語/英語)
            'BtnTableClampOff.Text = MSG_LOADER_24                       '"載物台クランプ解除" ボタン
            'BtnTableVacumeOff.Text = MSG_LOADER_25                      '"載物台吸着解除" ボタン
            'BtnHandVacumeOff.Text = MSG_LOADER_26                       '"ハンド吸着解除" ボタン
            'BtnHandVacumeOff.Text = MSG_LOADER_27                       '"供給ハンド吸着解除" ボタン 
            'BtnHand2VacumeOff.Text = MSG_LOADER_28                      '"収納ハンド吸着解除" ボタン 

            ' ラベル名を設定する(日本語/英語)
            '"装置内に基板が残っている場合は", "取り除いてください"
            LblMSG.Text = MSG_LOADER_29 + MSG_LOADER_18

            'If (gSysPrm.stTMN.giMsgTyp = 0) Then
            '    GrpLdAlarm.Text = "アラームリスト"
            'Else
            '    GrpLdAlarm.Text = "Alarm List"
            'End If

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FrmReset.SetControlName() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- ###186↑ -----
#Region "フォームが表示された時の処理"
    '''=========================================================================
    ''' <summary>フォームが表示された時の処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub FrmReset_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown

        Dim r As Integer = cFRS_NORMAL
        Dim strMSG As String

        Try
            ' 画面処理メイン
            r = FrmReset_Main(gMode)
            mExitFlag = r                                           ' mExitFlagに戻り値を設定する 

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FrmReset.FrmReset_Shown() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
        Me.Close()                                                  ' フォームを閉じる
    End Sub
#End Region

#Region "Cancel(or OK)ボタン押下時処理"
    '''=========================================================================
    ''' <summary>Cancel(or OK)ボタン押下時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click

        Dim strMSG As String

        Try
            'V4.9.0.0①
            '            If (BtnCancel.Text = "Cancel") Then
#If START_KEY_SOFT Then
            If (BtnCancel.Text = "Cancel") Or (BtnCancel.Text = "CANCEL") Or (BtnCancel.Text = "RESET") Or (BtnCancel.Text = "中止") Then
#Else
            If (BtnCancel.Text = "Cancel") Or (BtnCancel.Text = "中止") Then
#End If
                mExitFlag = cFRS_ERR_RST
            Else
                mExitFlag = cFRS_ERR_START
            End If
            '----- V6.0.3.0_27↓ -----
            If (BtnCancel.Text = MSG_SPRASH58) Then                     ' "継続"  ?
                mExitFlag = cFRS_ERR_RST
            End If
            '----- V6.0.3.0_27↑ -----

#If START_KEY_SOFT Then
            If gbStartKeySoft Then
                Call RESET_SWITCH_ON()
            End If
#End If
            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FrmReset.BtnCancel_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "OKボタン押下時処理"
    '''=========================================================================
    ''' <summary>OKボタン押下時処理 ###073</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnOK.Click
        Dim strMSG As String

        Try
            mExitFlag = cFRS_ERR_START
#If START_KEY_SOFT Then
            If gbStartKeySoft Then
                Call START_SWITCH_ON()
            End If
#End If
            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FrmReset.BtnOK_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- ###161↓ -----
#Region "載物台クランプ解除ボタン押下時"
    '''=========================================================================
    ''' <summary>載物台クランプ解除ボタン押下時</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnTableClampOff_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnTableClampOff.Click

        Dim r As Integer                                                    ' V1.23.0.0⑩
        Dim strMSG As String

        Try
            ' アラームリセット送出 
            Call W_RESET()

            '----- V1.23.0.0⑩↓ -----
            ' 自動運転中断信号送出
            r = Send_AutoStopToLoader()
            '----- V1.23.0.0⑩↑ -----

            ' ローダ手動モード切替え
            Call SetLoaderIO(&H0, LOUT_AUTO)                            ' ローダ出力(ON=なし, OFF=自動)

            ' クランプＯＦＦ
            'Call W_CLMP_ONOFF()                                        ' クランプＯＦＦ信号送出 V1.16.0.0⑤
            Call W_CLMP_ONOFF(0)                                        ' クランプＯＦＦ信号送出 V1.16.0.0⑤

            '' アラームリセット送出
            'Call W_RESET()

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormReset.BtnTableClampOff_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "載物台吸着解除ボタン押下時"
    '''=========================================================================
    ''' <summary>載物台吸着解除ボタン押下時</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnTableVacumeOff_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnTableVacumeOff.Click

        Dim r As Integer                                                    ' V1.23.0.0⑩
        Dim strMSG As String

        Try
            ' アラームリセット送出 
            Call W_RESET()

            '----- V1.23.0.0⑩↓ -----
            ' 自動運転中断信号送出
            r = Send_AutoStopToLoader()
            '----- V1.23.0.0⑩↑ -----

            ' ローダ手動モード切替え
            Call SetLoaderIO(&H0, LOUT_AUTO)                            ' ローダ出力(ON=なし, OFF=自動)

            ' 吸着ＯＦＦ
            'Call W_VACUME_ONOFF()                                      ' 吸着ＯＦＦ信号送出 V1.18.0.0⑮
            Call W_VACUME_ONOFF(0)                                      ' 吸着ＯＦＦ信号送出 V1.18.0.0⑮

            '' アラームリセット送出
            'Call W_RESET()

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormReset.BtnTableVacumeOff_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "供給ハンド吸着解除ボタン押下時"
    '''=========================================================================
    ''' <summary>供給ハンド吸着解除ボタン押下時</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnHandVacumeOff_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnHandVacumeOff.Click

        Dim strMSG As String

        Try
            ' アラームリセット送出 
            Call W_RESET()

            ' ローダ手動モード切替え
            Call SetLoaderIO(&H0, LOUT_AUTO)                            ' ローダ出力(ON=なし, OFF=自動)

            ' ハンド１吸着ＯＦＦ
            Call W_HAND1_VACUME()                                       ' ハンド１吸着ＯＦＦ

            '' アラームリセット送出
            'Call W_RESET()

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormReset.BtnHandVacumeOff_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "収納ハンド吸着解除ボタン押下時"
    '''=========================================================================
    ''' <summary>収納ハンド吸着解除ボタン押下時</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnHand2VacumeOff_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnHand2VacumeOff.Click

        Dim strMSG As String

        Try
            ' アラームリセット送出 
            Call W_RESET()

            ' ローダ手動モード切替え
            Call SetLoaderIO(&H0, LOUT_AUTO)                            ' ローダ出力(ON=なし, OFF=自動)

            ' ハンド２吸着ＯＦＦ
            Call W_HAND2_VACUME()                                       ' ハンド２吸着ＯＦＦ

            '' アラームリセット送出
            'Call W_RESET()

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormReset.BtnHand2VacumeOff_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- ###161↑ -----
    '----- V4.11.0.0⑥↓ (WALSIN殿SL436S対応) -----
#Region "MG1 MouseDown処理"
    '''=========================================================================
    ''' <summary>MG1 MouseDown処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnMg1Down_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnMg1Down.MouseDown

        Dim Mg1 As Integer = 1
        Dim strMSG As String

        Try
            ' マガジン下移動動作
            Call MGMoveJog(Mg1, MG_DOWN)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormReset.BtnMg1Down_MouseDown() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "MG1 MouseUp処理"
    '''=========================================================================
    ''' <summary>MG1 MouseUp処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnMg1Down_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnMg1Down.MouseUp

        Dim strMSG As String

        Try
            ' マガジン上下動作停止
            Call MGStopJog()

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormReset.BtnMg1Down_MouseUp() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- V4.11.0.0⑥↑ -----
#Region "画面処理メイン"
    '''=========================================================================
    ''' <summary>画面処理メイン</summary>
    ''' <param name="gMode">(INP)処理モード</param>
    ''' <returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function FrmReset_Main(ByVal gMode As Integer) As Integer

        Dim r As Integer = cFRS_NORMAL
        Dim strMSG As String
        Dim InitError As Integer = 0        'V5.0.0.1-21

        Try
            '-------------------------------------------------------------------
            '   処理モードに対応する処理を行う
            '-------------------------------------------------------------------
            Select Case gMode
                '   '-----------------------------------------------------------
                '   '   原点復帰処理
                '   '-----------------------------------------------------------
                Case cGMODE_ORG                                                 ' 原点復帰処理
#If START_KEY_SOFT Then
                    If gbStartKeySoft Then
                        Call Sub_StartResetButtonDispOn()
                    End If
#End If
                    Call ObjSys.SetOrgFlg(True)                                 ' 原点復帰処理中フラグをONに設定する
                    r = Sub_OriginBack()                                        ' 原点復帰処理
                    Call ObjSys.SetOrgFlg(False)                                ' 原点復帰処理中フラグをOFFに設定する

                    'V5.0.0.1-21
                    '初期化時のタイムアウト、ローダエラーはソフト終了にする
                    InitError = r
                    'V5.0.0.1-21

                    ' INtime側エラー
                    If (System.Math.Abs(r) >= ERR_INTIME_BASE) Then             ' INtime側エラー ?
                        gMode = System.Math.Abs(r)                              ' gMode = メッセージ番号
                        GoTo STP_INTRIM                                         ' メッセージ表示へ
                    End If
#If START_KEY_SOFT Then
                    If gbStartKeySoft Then
                        Call Sub_StartResetButtonDispOff()
                    End If
#End If
                    ' 他に下記のエラーで返る
                    Select Case (r)
                        Case cFRS_ERR_EMG                                       ' 非常停止
                            GoTo STP_EMERGENCY                                  ' 非常停止メッセージ表示

                        Case cFRS_ERR_LDR, cFRS_ERR_LDR1, cFRS_ERR_LDR2, cFRS_ERR_LDR3, cFRS_ERR_LDRTO
                            GoTo STP_LDRALARM                                   ' ローダアラームメッセージ表示

                        Case cFRS_ERR_DUST                                      ' 集塵機異常検出
                            GoTo STP_ARMDUST                                    ' 集塵機アラームメッセージ表示

                        Case cFRS_ERR_AIR                                       ' エアー圧エラー検出
                            GoTo STP_AIRVALVE                                   ' エアー圧低下検出メッセージ表示

                        Case cFRS_ERR_MVC                                       ' ﾏｽﾀｰﾊﾞﾙﾌﾞ回路状態エラー検出
                            ' (メッセージ表示済み)

                        Case cFRS_TO_SCVR_ON                                    ' タイムアウト(ｽﾗｲﾄﾞｶﾊﾞｰｽﾄｯﾊﾟｰ行待ち)
                            ' (メッセージ表示済み)

                        Case cFRS_TO_SCVR_OP                                    ' タイムアウト(スライドカバー開待ち)
                            ' (メッセージ表示済み)
                    End Select

                    '-----------------------------------------------------------
                    '   ローダ原点復帰
                    '-----------------------------------------------------------
                Case cGMODE_LDR_ORG
                    ' "ローダ原点復帰中", "", ""
                    Call Sub_SetMessage(MSG_SPRASH24, "", "", System.Drawing.SystemColors.ControlText, System.Drawing.SystemColors.ControlText, System.Drawing.SystemColors.ControlText)
                    Me.Refresh()
                    r = Sub_Loader_OrgBack(cGMODE_LDR_ORG)
                    Select Case (r)
                        Case cFRS_ERR_EMG
                            GoTo STP_EMERGENCY                                  ' 非常停止メッセージ表示
                        Case cFRS_ERR_LDR, cFRS_ERR_LDR1, cFRS_ERR_LDR2, cFRS_ERR_LDR3, cFRS_ERR_LDRTO
                            GoTo STP_LDRALARM                                   ' ローダアラームメッセージ表示
                    End Select

                    '-----------------------------------------------------------
                    '   連続HI-NGエラー発生時
                    '-----------------------------------------------------------
                Case cGMODE_ERR_HING                                            ' 連続HI-NGエラー時
                    r = Sub_TrimError(cGMODE_ERR_HING)
                    Select Case (r)
                        Case cFRS_ERR_EMG
                            GoTo STP_EMERGENCY                                  ' 非常停止メッセージ表示
                        Case cFRS_ERR_LDR, cFRS_ERR_LDR1, cFRS_ERR_LDR2, cFRS_ERR_LDR3, cFRS_ERR_LDRTO
                            GoTo STP_LDRALARM                                   ' ローダアラームメッセージ表示
                        Case Else
                            r = cFRS_ERR_HING                                   ' Return値 = 連続NG-HIGHｴエラー発生
                    End Select

                    '-----------------------------------------------------------
                    '   再プロービング失敗時
                    '-----------------------------------------------------------
                Case cGMODE_ERR_REPROBE                                         ' 再プロービング失敗時
                    r = Sub_TrimError(cGMODE_ERR_REPROBE)
                    Select Case (r)
                        Case cFRS_ERR_EMG
                            GoTo STP_EMERGENCY                                  ' 非常停止メッセージ表示
                        Case cFRS_ERR_LDR, cFRS_ERR_LDR1, cFRS_ERR_LDR2, cFRS_ERR_LDR3, cFRS_ERR_LDRTO
                            GoTo STP_LDRALARM                                   ' ローダアラームメッセージ表示
                        Case Else
                            r = cFRS_ERR_REPROB                                 ' Return値 = 再プロービング失敗
                    End Select

                    '----- V6.0.3.0⑳↓ -----
                    '-----------------------------------------------------------
                    '   自動カットオフ制御に失敗したときのエラーメッセージ
                    '-----------------------------------------------------------
                Case cGMODE_ERR_CUTOFF_TURNING                                  '自動カットオフ調整に失敗
                    r = Sub_CutOffTurnError(cGMODE_ERR_CUTOFF_TURNING)
                    Select Case (r)
                        Case cFRS_ERR_EMG
                            GoTo STP_EMERGENCY                                  ' 非常停止メッセージ表示
                        Case Else
                    End Select
                    '----- V6.0.3.0⑳↑ -----

                    '-----------------------------------------------------------
                    '   ローダアラーム発生時
                    '-----------------------------------------------------------
                Case cGMODE_LDR_ALARM                                           ' ローダアラーム発生時
STP_LDRALARM:
                    r = Sub_LdrAlarm(cGMODE_LDR_ALARM)
                    'V5.0.0.1-21
                    If r = 0 Then
                        r = InitError
                    End If
                    'V5.0.0.1-21

                    '-----------------------------------------------------------
                    '   非常停止メッセージ表示
                    '-----------------------------------------------------------
                Case cGMODE_EMG
STP_EMERGENCY:
                    bEmergencyOccurs = True 'V1.25.0.5②
#If START_KEY_SOFT Then
                    If gbStartKeySoft Then
                        Call Sub_ResetButtonDispOn()
                    End If
#End If
                    giTrimErr = giTrimErr Or &H10                   ' ﾄﾘﾏｰ ｴﾗｰ ﾌﾗｸﾞ(非常停止)
                    'V5.0.0.9⑭ ↓
                    '　r = ObjSys.SetSignalTower(0, &HFFFF)            ' ｼｸﾞﾅﾙﾀﾜｰ制御(On=0, Off=全ﾋﾞｯﾄ)
                    Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALL_OFF)
                    'V5.0.0.9⑭ ↑
                    Call EXTOUT1(0, &HFFFF)                         ' EXTBIT (On=0, Off=全ビット)
                    Call EXTOUT2(0, &HFFFF)                         ' EXTBIT2(On=0, Off=全ビット)
                    r = Sub_DispEmergencyMsg()                      ' 非常停止メッセージ表示

                    '-----------------------------------------------------------
                    '   集塵機異常メッセージ表示
                    '-----------------------------------------------------------
                Case cGMODE_ERR_DUST
STP_ARMDUST:
#If START_KEY_SOFT Then
                    If gbStartKeySoft Then
                        Call Sub_ResetButtonDispOn()
                    End If
#End If
                    Call LAMP_CTRL(LAMP_START, False)               ' STARTﾗﾝﾌﾟ消灯
                    ' メッセージ表示
                    ' "集塵機異常が発生しました", "RESETキーを押すとプログラムを終了します", ""
                    Call Sub_SetMessage(MSG_SPRASH17, MSG_SPRASH16, "", System.Drawing.Color.Red, System.Drawing.Color.Red, System.Drawing.SystemColors.ControlText)
                    Me.Refresh()

                    ' ----- V1.18.0.0③↓ -----
                    ' 印刷用アラーム開始情報を設定する(ローム殿特注)
                    ' アラーム発生回数とアラーム停止開始時間を設定する
                    Call SetAlmStartTime()
                    ' アラームファイルにアラームデータを書き込む
                    Call WriteAlarmData(gFPATH_QR_ALARM, MSG_SPRASH17, stPRT_ROHM.AlarmST_time, cFRS_ERR_DUST)
                    ' ----- V1.18.0.0③↑ -----

                    ' メッセージ表示してRESETキー押下待ち
                    r = Sub_WaitStartRestKey(cFRS_ERR_RST)
                    ' ----- V1.18.0.0③↓ -----
                    ' アラーム停止情報を設定する(ローム殿特注)
                    Call SetAlmEndTime()
                    ' ----- V1.18.0.0③↑ -----
                    If (r = cFRS_ERR_EMG) Then GoTo STP_EMERGENCY
                    r = cFRS_ERR_DUST                                           ' Return値 = 集塵機異常検出

                    '-----------------------------------------------------------
                    '   エアー圧低下検出メッセージ表示
                    '-----------------------------------------------------------
                Case cGMODE_ERR_AIR
STP_AIRVALVE:
#If START_KEY_SOFT Then
                    If gbStartKeySoft Then
                        Call Sub_ResetButtonDispOn()
                    End If
#End If
                    Call LAMP_CTRL(LAMP_START, False)                   ' STARTﾗﾝﾌﾟ消灯
                    ' メッセージ表示
                    ' "エアー圧低下検出", "RESETキーを押すとプログラムを終了します", ""
                    Call Sub_SetMessage(MSG_SPRASH12, MSG_SPRASH16, "", System.Drawing.Color.Red, System.Drawing.Color.Red, System.Drawing.SystemColors.ControlText)
                    Me.Refresh()

                    ' RESETキー押下待ち
                    r = Sub_WaitStartRestKey(cFRS_ERR_RST)
                    If (r = cFRS_ERR_EMG) Then GoTo STP_EMERGENCY
                    r = cFRS_ERR_AIR                                            ' Return値 = エアー圧エラー検出

                    '-----------------------------------------------------------
                    '   ローダ通信タイムアウトメッセージ表示
                    '-----------------------------------------------------------
                Case cGMODE_LDR_TMOUT
STP_LDTOUT:
#If START_KEY_SOFT Then
                    If gbStartKeySoft Then
                        Call Sub_ResetButtonDispOn()
                    End If
#End If
                    Call LAMP_CTRL(LAMP_START, False)                   ' STARTﾗﾝﾌﾟ消灯

                    '----- ###205↓ -----
                    ' シグナルタワーを赤点滅+ブザーONする
                    Select Case (gSysPrm.stIOC.giSignalTower)
                        Case SIGTOWR_NORMAL                             ' 標準(赤点滅+ブザーON)
                            'V5.0.0.9⑭ ↓ V6.0.3.0⑧
                            ' Call ObjSys.SetSignalTower(SIGOUT_RED_BLK Or SIGOUT_BZ1_ON, &HFFFF)
                            Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALARM)
                            'V5.0.0.9⑭ ↑ V6.0.3.0⑧ 

                        Case SIGTOWR_SPCIAL                             ' 特注(赤点滅+ブザー１)
                            'r = ObjSys.SetSignalTower(EXTOUT_RED_BLK Or EXTOUT_BZ1_ON, &HFFFF)
                    End Select
                    '----- ###205↑ -----

                    ' メッセージ表示
                    ' "ローダ通信タイムアウトエラー", "RESETキーを押すと処理を終了します", ""
                    Call Sub_SetMessage(MSG_SPRASH18, MSG_SPRASH33, "", System.Drawing.Color.Red, System.Drawing.Color.Red, System.Drawing.SystemColors.ControlText)
                    Me.Refresh()

                    ' RESETキー押下待ち
                    r = Sub_WaitStartRestKey(cFRS_ERR_RST)
                    '----- ###205↓ -----
                    ' シグナルタワーを赤点滅+ブザーOFFする
                    Select Case (gSysPrm.stIOC.giSignalTower)
                        Case SIGTOWR_NORMAL                             ' 標準(赤点滅+ブザーOFF)
                            'V5.0.0.9⑭ ↓ V6.0.3.0⑧
                            ' Call ObjSys.SetSignalTower(0, SIGOUT_RED_BLK Or SIGOUT_BZ1_ON)
                            Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALL_OFF)
                            'V5.0.0.9⑭ ↑ V6.0.3.0⑧

                        Case SIGTOWR_SPCIAL                             ' 特注(赤点滅+ブザー１ OFF)
                            'r = ObjSys.SetSignalTower(0, EXTOUT_RED_BLK Or EXTOUT_BZ1_ON)
                    End Select
                    '----- ###205↑ -----
                    If (r = cFRS_ERR_EMG) Then GoTo STP_EMERGENCY
                    r = cFRS_ERR_LDRTO                                          ' Return値 = ローダ通信タイムアウト

                    '-----------------------------------------------------------
                    '   自動運転開始(STARTｷｰ押下待ち)
                    '-----------------------------------------------------------
                Case cGMODE_LDR_START
#If START_KEY_SOFT Then
                    If gbStartKeySoft Then
                        Call Sub_StartResetButtonDispOn()
                    End If
#End If
                    ' メッセージ表示
                    ' "STARTキーを押すと自動運転を開始します", "", ""
                    Call Sub_SetMessage(MSG_SPRASH22, "", "", System.Drawing.SystemColors.ControlText, System.Drawing.SystemColors.ControlText, System.Drawing.SystemColors.ControlText)
                    Me.Refresh()

                    ' STARTキー押下待ち
                    r = Sub_WaitStartRestKey(cFRS_ERR_START + cFRS_ERR_RST)
                    If (r = cFRS_ERR_EMG) Then GoTo STP_EMERGENCY
                    '                                                           ' Return値 = cFRS_ERR_START(STARTキー押下)/cFRS_ERR_RST(RESETキー押下)

                    '-----------------------------------------------------------
                    '   自動運転終了(STARTｷｰ押下待ち)
                    '-----------------------------------------------------------
                Case cGMODE_LDR_END

                    Dim DSP_MSG As String

                    ' メッセージ表示
                    BtnCancel.Visible = True                                    ' Cancel(OK)ボタン表示
                    BtnCancel.Text = "OK"
                    DSP_MSG = MSG_frmLimit_07

#If START_KEY_SOFT Then
                    If gbStartKeySoft Then
                        DSP_MSG = MSG_frmLimit_08
                    End If
#End If

                    ' "自動運転終了", "STARTキーを押すか、OKボタンを押して下さい。", ""
                    Call Sub_SetMessage(MSG_LOADER_15, DSP_MSG, "", System.Drawing.SystemColors.ControlText, System.Drawing.SystemColors.ControlText, System.Drawing.SystemColors.ControlText)
                    Me.Refresh()

                    ' STARTキー押下待ち
                    r = Sub_WaitStartRestKey(cFRS_ERR_START)
                    BtnCancel.Visible = False                                   ' Cancel(OK)ボタン非表示
                    BtnCancel.Text = "Cancel"
                    If (r = cFRS_ERR_EMG) Then GoTo STP_EMERGENCY
                    '                                                           ' Return値 = cFRS_ERR_START(STARTキー押下)

                    '-----------------------------------------------------------
                    '   残基板取り除きメッセージ(STARTｷｰ押下待ち)
                    '-----------------------------------------------------------
                Case cGMODE_LDR_WKREMOVE, cGMODE_LDR_RSTAUTO, cGMODE_LDR_WKREMOVE2   ' ###175 ###124
                    ' メッセージ表示
                    BtnCancel.Visible = True                                    ' Cancel(OK)ボタン表示
                    BtnCancel.Text = "OK"

                    '----- ###161↓ -----
                    Me.Size = stSzLDE2                                          ' Formの幅/高さを解除ボタン表示モード用にする
                    Call COVERCHK_ONOFF(COVER_CHECK_OFF)                        ' 「固定カバー開チェックなし」にする
                    '----- ###161↑ -----

                    '----- V1.18.0.1⑧↓ -----
                    ' 電磁ロック(観音扉右側ロック)を解除する(ローム殿特注)
                    r = EL_Lock_OnOff(EX_LOK_MD_OFF)
                    If (r = cFRS_TO_EXLOCK) Then                                ' 「前面扉ロック解除タイムアウト」なら戻り値を「RESET」にする
                        Return (cFRS_ERR_RST)
                    End If
                    If (r = cFRS_ERR_EMG) Then                                  ' 非常停止 ?
                        GoTo STP_EMERGENCY                                      ' 非常停止メッセージ表示へ
                    End If
                    If (r < cFRS_NORMAL) Then                                   ' 異常終了レベルのエラー ? 
                        Return (r)
                    End If
                    '----- V1.18.0.1⑧↑ -----

                    '----- ###124↓ -----
                    If (gMode = cGMODE_LDR_WKREMOVE) Then
                        ' "載物台上に基板が残っている場合は", "取り除いてください", "STARTキー又はOKボタン押下で原点復帰します。"
                        Call Sub_SetMessage(MSG_LOADER_17, MSG_LOADER_18, MSG_LOADER_22, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.SystemColors.ControlText)

                        '' "載物台上に基板が残っている場合は", "取り除いてください", "STARTキーを押すか、OKボタンを押して下さい。"
                        'Call Sub_SetMessage(MSG_LOADER_17, MSG_LOADER_18, MSG_frmLimit_07, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.SystemColors.ControlText)

                        '----- ###175↓ -----
                    ElseIf (gMode = cGMODE_LDR_WKREMOVE2) Then
                        ' "装置内に基板が残っている場合は", "取り除いてください", "OKボタン押下でアプリケーションを終了します"
                        Call Sub_SetMessage(MSG_LOADER_29, MSG_LOADER_18, MSG_LOADER_31, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.SystemColors.ControlText)
                        '----- ###175↑ -----

                    Else
                        '' "自動運転を中止します", "", "STARTキー又はOKボタン押下で原点復帰します。"
                        'Call Sub_SetMessage(MSG_LOADER_23, "", MSG_LOADER_22, System.Drawing.Color.Blue, System.Drawing.SystemColors.ControlText, System.Drawing.SystemColors.ControlText)
                        ' ###161 "自動運転を中止します", "筐体カバーを閉じて", "STARTキー又はOKボタン押下で原点復帰します。"
                        Call Sub_SetMessage(MSG_LOADER_23, MSG_SPRASH36, MSG_LOADER_22, System.Drawing.Color.Blue, System.Drawing.SystemColors.ControlText, System.Drawing.SystemColors.ControlText)
                    End If
                    '----- ###124↑ -----
                    Me.Refresh()

                    ' STARTキー押下待ち
                    r = Sub_WaitStartRestKey(cFRS_ERR_START)
                    BtnCancel.Visible = False                                   ' Cancel(OK)ボタン非表示
                    BtnCancel.Text = "Cancel"
                    If (r = cFRS_ERR_EMG) Then GoTo STP_EMERGENCY
                    '                                                           ' Return値 = cFRS_ERR_START(STARTキー押下)
                    '----- ###161↓ -----
                    If (gMode = cGMODE_LDR_WKREMOVE2) Then                      ' cGMODE_LDR_WKREMOVE2は筐体カバー閉を確認なし ###175
                        Return (r)
                    End If

                    ' 筐体カバー閉を確認する
                    Call COVERLATCH_CLEAR()                                     ' カバー開ラッチのクリア
                    Call COVERCHK_ONOFF(COVER_CHECK_ON)                         ' 「固定カバー開チェックあり」にする
                    Me.Size = stSzNML                                           ' Formの幅/高さを標準モード用にする
                    Me.Visible = False                                          ' メッセージ表示を消す 
                    r = Sub_CoverCheck()
                    If (r = cFRS_ERR_EMG) Then                                  ' 非常停止 ?
                        GoTo STP_EMERGENCY                                      ' 非常停止メッセージ表示へ
                    End If
                    '----- V1.18.0.1⑧↓ -----
                    ' 電磁ロック(観音扉右側ロック)する(ローム殿特注)
                    r = EL_Lock_OnOff(EX_LOK_MD_ON)
                    If (r = cFRS_TO_EXLOCK) Then                                ' 「前面扉ロック解除タイムアウト」なら戻り値を「RESET」にする
                        Return (cFRS_ERR_RST)
                    End If
                    If (r = cFRS_ERR_EMG) Then                                  ' 非常停止 ?
                        GoTo STP_EMERGENCY                                      ' 非常停止メッセージ表示へ
                    End If
                    If (r < cFRS_NORMAL) Then                                   ' 異常終了レベルのエラー ? 
                        Return (r)
                    End If
                    '----- V1.18.0.1⑧↑ -----

                    r = cFRS_ERR_START
                    '----- ###161↑ -----

                    '----- ###188↓ -----
                    '-----------------------------------------------------------
                    '   ステージを原点に戻す(残基板取り除くため)
                    '-----------------------------------------------------------
                Case cGMODE_LDR_STAGE_ORG
                    ' "ステージ原点移動中", "", ""
                    Call Sub_SetMessage(MSG_SPRASH38, "", "", System.Drawing.SystemColors.ControlText, System.Drawing.SystemColors.ControlText, System.Drawing.SystemColors.ControlText)
                    Me.Refresh()
                    r = Sub_XY_OrgBack()
                    Select Case (r)
                        Case cFRS_ERR_EMG
                            GoTo STP_EMERGENCY                                  ' 非常停止メッセージ表示
                    End Select
                    '----- ###188↑ -----
                    '----- V1.18.0.0⑨↓ -----
                    '-----------------------------------------------------------
                    '   マガジン交換メッセージ(STARTｷｰ押下待ち)
                    '-----------------------------------------------------------
                Case cGMODE_LDR_MAGAGINE_EXCHG
                    r = Sub_MagazineExchange()                                  ' マガジン交換メッセージ表示
                    If (r = cFRS_ERR_EMG) Then                                  ' 非常停止 ?
                        GoTo STP_EMERGENCY                                      ' 非常停止メッセージ表示へ
                    End If
                    '----- V1.18.0.0⑨↑ -----
                    'V4.9.0.0①↓
                Case cGMODE_ERR_RATE_NG        '良品率が悪くなった場合のエラー表示画面
                    r = Sub_NgRateAlarm()

                Case cGMODE_ERR_TOTAL_CLEAR    '集計をクリアするかの確認メッセージ
                    r = Sub_QuestTotalClear()
                    'V4.9.0.0①↑

                    '-----------------------------------------------------------
                    '   トリミング中のｽﾗｲﾄﾞｶﾊﾞｰ開/筐体ｶﾊﾞｰ開メッセージ表示(STARTｷｰ押下待ち)
                    '-----------------------------------------------------------
                Case cGMODE_SCVR_OPN, cGMODE_CVR_OPN
#If START_KEY_SOFT Then
                    If gbStartKeySoft Then
                        Call Sub_ResetButtonDispOn()
                    End If
#End If
                    r = Sub_CvrOpen(gMode)                                      ' ｽﾗｲﾄﾞｶﾊﾞｰ開/筐体ｶﾊﾞｰ開ﾒｯｾｰｼﾞ表示
                    If (r = cFRS_ERR_EMG) Then                                  ' 非常停止 ?
                        GoTo STP_EMERGENCY                                      ' 非常停止メッセージ表示へ
                    End If                                                      ' ※ﾄﾘﾐﾝｸﾞ中の筐体ｶﾊﾞｰ開は原点復帰は行わない

                    '-----------------------------------------------------------
                    '   指定メッセージ表示(STARTキー/RESETキー押下待ち) ###089
                    '-----------------------------------------------------------
                Case cGMODE_MSG_DSP
                    ' OK/Cancelボタン表示設定
                    BtnCancel.Visible = False                                   ' Cancelボタン非表示
                    BtnOK.Visible = False                                       ' OKボタン非表示
                    If (DspBtnDsp = True) Then                                  ' ボタン表示する ?
                        If (DspWaitKey = (cFRS_ERR_START + cFRS_ERR_RST)) Then  ' OK/Cancelボタンを表示する ?
                            BtnCancel.Location = LocCanBtn2                     ' Cancelボタン表示位置を右にずらす 
                            BtnCancel.Visible = True                            ' Cancelボタン表示
                            BtnOK.Visible = True                                ' OKボタン表示
                        End If
                        If (DspWaitKey = cFRS_ERR_RST) Then                     ' Cancelボタンを表示する ?
                            BtnCancel.Text = "Cancel"
                            BtnCancel.Visible = True                            ' Cancelボタン表示
                        End If
                        If (DspWaitKey = cFRS_ERR_START) Then                   ' OKボタンを表示する ?
                            BtnCancel.Text = "OK"
                            BtnCancel.Visible = True                            ' Cancel(OK)ボタン表示
                        End If
                        'V4.11.0.0⑪
                        If (DspWaitKey = cFRS_ERR_BTN_START) Then               ' OKボタンを表示する ?
                            BtnCancel.Text = "START"
                            BtnCancel.Visible = True                            ' Cancel(OK)ボタン表示
                        End If
                        If (DspWaitKey = cFRS_ERR_BTN_START + cFRS_ERR_RST) Then                   ' OKボタンを表示する ?
                            BtnCancel.Location = LocCanBtn2                     ' Cancelボタン表示位置を右にずらす 
                            BtnCancel.Visible = True                            ' Cancelボタン表示
                            BtnCancel.Text = "CANCEL"
                            BtnOK.Text = "START"                                ' OKボタン表示
                            BtnOK.Visible = True                                ' OKボタン表示
                        End If
                        'V4.11.0.0⑪

                    End If

                    ' 指定メッセージを表示する
                    Call Sub_SetMessage(strMsgAry(0), strMsgAry(1), strMsgAry(2), ColColAry(0), ColColAry(1), ColColAry(2))
                    Me.Refresh()

                    ' STARTキー/RESETキー押下待ち
                    r = Sub_WaitStartRestKey(DspWaitKey)
                    BtnCancel.Visible = False                                   ' Cancelボタン非表示
                    BtnOK.Visible = False                                       ' OKボタン非表示
                    BtnCancel.Text = "Cancel"
                    If (r = cFRS_ERR_EMG) Then GoTo STP_EMERGENCY
                    '                                                           ' Return値 = cFRS_ERR_START(STARTキー押下)

                    ''V4.0.0.0-83
                    ' ステージHome位置移動
                Case cGMODE_STAGE_HOMEMOVE
                    'V4.0.0.0-83↓
                    LblCaption.Text = MSG_SPRASH47
                    Me.Refresh()
                    r = SubHomeMove()
                    If (r = cFRS_ERR_EMG) Then GoTo STP_EMERGENCY
                    'V4.0.0.0-83

                    '----- V6.0.3.0_27↓ -----
                    '-----------------------------------------------------------
                    '   自動運転ボタンを押したときに新規か継続の選択を行う 
                    '-----------------------------------------------------------
                Case cGMODE_QUEST_NEW_CONTINUE

                    BtnCancel.Visible = True                                    ' Cancelボタン表示
                    BtnOK.Visible = True                                        ' OKボタン表示
                    BtnCancel.Location = LocCanBtn2                             ' Cancelボタン表示位置を右にずらす 

                    ' メッセージ表示
                    ' "新規：STARTボタン、継続：RESETボタン", "", ""
                    Call Sub_SetMessage(MSG_SPRASH74, "", "", System.Drawing.SystemColors.ControlText, System.Drawing.SystemColors.ControlText, System.Drawing.SystemColors.ControlText)

                    Dim save_OKBtn As String = BtnOK.Text
                    Dim save_CancelBtn As String = BtnCancel.Text

                    BtnOK.Text = MSG_SPRASH75                                   ' "新規" 
                    BtnCancel.Text = MSG_SPRASH76                               ' "継続" 
                    Me.Refresh()

                    ' STARTキー押下待ち
                    r = Sub_WaitStartRestKey(cFRS_ERR_START + cFRS_ERR_RST)

                    BtnOK.Text = save_OKBtn
                    BtnCancel.Text = save_CancelBtn

                    If (r = cFRS_ERR_EMG) Then GoTo STP_EMERGENCY
                    '----- V6.0.3.0_27↑ -----

                    '-----------------------------------------------------------
                    '   INtime側からエラー戻りしたエラーメッセージを表示する
                    '-----------------------------------------------------------
                Case Else
STP_INTRIM:
                    ' INtime側エラー時
                    If (gMode >= ERR_INTIME_BASE) Then                          ' INtime側エラー ?
                        '   ' ソフトリミットエラーの場合
                        If (ObjSys.IsSoftLimitCode(gMode)) Then                 ' ソフトリミットエラー
                            'r = Sub_ErrSoftLimit(gMode, giTrimErr)              ' メッセージ表示&STARTキー押下待ち ' ###008 

                            ' ソフトリミットエラー以外の場合
                        Else
                            ' シグナルタワー3色制御あり(特注) ?
                            If (gSysPrm.stIOC.giSigTwr2Flag = 1) Then
                                ' シグナルタワー３色制御(赤点滅) (EXTOUT(OnBit, OffBit))
                                Call ObjSys.EXTIO_Out_Sub(gSysPrm.stIOC.glSigTwr2_Out_Adr, gSysPrm.stIOC.glSigTwr2_Red_Blnk, _
                                                           gSysPrm.stIOC.glSigTwr2_Red_On Or gSysPrm.stIOC.glSigTwr2_Yellow_On Or gSysPrm.stIOC.glSigTwr2_Yellow_Blnk)
                                ' ブザー制御あり(特注) ?
                                If (gSysPrm.stIOC.giBuzerCtrlFlag = 1) Then
                                    ' ブザー音2(ピ～ピッピ) (EXTOUT(OnBit, OffBit))
                                    Call ObjSys.EXTIO_Out_Sub(gSysPrm.stIOC.glBuzerCtrl_Out_Adr, gSysPrm.stIOC.glBuzerCtrl_Out2, gSysPrm.stIOC.glBuzerCtrl_Out1)
                                End If
                            End If

                            ' メッセージ表示 & STARTキー押下待ち
                            'r = Sub_ErrAxis(System.Math.Abs(r), giTrimErr)     ' ###008 
                        End If

                    End If

                    'If (r = cFRS_ERR_EMG) Then GoTo STP_EMERGENCY              ' ###008 

                    ' メッセージ表示 & STARTキー押下待ち                        ' ###008 
                    r = Sub_ErrAxis(gMode, giTrimErr)
                    If (r = cFRS_ERR_EMG) Then                                  ' 非常停止検出 ?
                        GoTo STP_EMERGENCY                                      ' 非常停止メッセージ表示へ
                    End If

                    r = -1 * gMode                                              ' Return値 = = gMode(-xxxで戻る)

            End Select

            ' 終了処理
            Return (r)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FrmReset.FrmReset_Main() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Function
#End Region

#Region "原点復帰処理"
    '''=========================================================================
    ''' <summary>原点復帰処理</summary>
    ''' <returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function Sub_OriginBack() As Integer

        Dim r As Integer
        Dim bR As Boolean
        Dim sts As Long
        Dim InterlockSts As Integer
        Dim strMSG As String
        Dim lData As Long

        Try
            ' シグナルタワー制御(On=原点復帰中,Off=全ﾋﾞｯﾄ) ###007
            Select Case (gSysPrm.stIOC.giSignalTower)
                Case SIGTOWR_NORMAL                                     ' 標準(原点復帰中(緑点滅))
                    'V5.0.0.9⑭ ↓ V6.0.3.0⑧(ローム殿仕様は無点灯)
                    ' r = ObjSys.SetSignalTower(SIGOUT_GRN_BLK, &HFFFF)
                    Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ZRN)
                    'V5.0.0.9⑭ ↑ V6.0.3.0⑧

                Case SIGTOWR_SPCIAL                                     ' 特注(原点復帰中(黄色点滅))
                    r = ObjSys.SetSignalTower(EXTOUT_YLW_BLK, &HFFFF)
            End Select

            ' ローダ通信チェック
            r = W_Read(LOFS_W110, lData)                               ' ローダアラーム状態取得(W110.08-10)
            If (r <> 0) Then
                r = cFRS_ERR_LDRTO
                GoTo STP_END
            End If

            ' 吸着,クランプ制御等
            Call ObjSys.AutoLoaderFlgReset()                            ' ｵｰﾄﾛｰﾀﾞｰﾌﾗｸﾞﾘｾｯﾄ
            Call ZSLCOVEROPEN(0)                                        ' ｽﾗｲﾄﾞｶﾊﾞｰｵｰﾌﾟﾝﾊﾞﾙﾌﾞOFF
            Call ZSLCOVERCLOSE(0)                                       ' ｽﾗｲﾄﾞｶﾊﾞｰｸﾛｰｽﾞﾊﾞﾙﾌﾞOFF
            ' クランプ及びバキュームOFF
            If (bFgAutoMode = False) Then                               ' 自動運転時はクランプ及びバキュームOFFしない ###107
                r = ObjSys.ClampVacume_Ctrl(gSysPrm, 0, giAppMode, giTrimErr)
            End If

            Call ZCONRST()                                              ' コンソールキーラッチ解除
            If (r <> cFRS_NORMAL) Then                                  ' エラー ?
                If (r = cFRS_ERR_EMG) Then GoTo STP_EMERGENCY '         ' 非常停止ならEMERGENCYへ
                GoTo STP_END                                            ' その他のエラーリターン
            End If

            ' 非常停止 ?
            If (ObjSys.EmergencySwCheck() = True) Then
STP_EMERGENCY:
                r = cFRS_ERR_EMG                                        ' Return値 = 非常停止検出
                GoTo STP_END
            End If

            ' マスターバルブ回路状態チェック(特注)
            r = ObjSys.Master_Valve_Check(gSysPrm)                      ' マスターバルブチェック
            If (r = 2) Then                                             ' エラー検出でCancel ?
                r = cFRS_ERR_MVC                                        ' Return値 = ﾏｽﾀｰﾊﾞﾙﾌﾞ回路状態エラー検出
                GoTo STP_END
            End If

            ' エアー圧チェック
            bR = ObjSys.Air_Valve_Check(gSysPrm)
            If (bR = False) Then                                        ' エアー圧エラー ?
                r = cFRS_ERR_AIR                                        ' Return値 = エアー圧エラー検出
                GoTo STP_END
            End If

            ' 筐体カバー/スライドカバーチェック
STP_COVEROPEN:
            Call COVERLATCH_CLEAR()                                     ' カバー開ラッチのクリア
            r = CoverCheck(0, True)                                     ' 筐体カバー/スライドカバーチェック(RESETキー無効指定, 原点復帰処理中)
            If (r = cFRS_ERR_RST) Then GoTo STP_COVEROPEN '             ' RESET キー押下？
            If (r = cFRS_ERR_EMG) Then GoTo STP_EMERGENCY '             ' 非常停止ならEMERGENCYへ
            If (r <> cFRS_NORMAL) Then                                  ' その他のエラー ? 
                GoTo STP_END                                            ' その他のエラーリターン
            End If

            '原点復帰動作開始処理
            ' "STARTキーを押してください", "", ""
            Call Sub_SetMessage(MSG_SPRASH4, "", "", System.Drawing.SystemColors.ControlText, System.Drawing.SystemColors.ControlText, System.Drawing.SystemColors.ControlText)
            Me.Refresh()

#If cOFFLINEcDEBUG Then                                                 ' OffLineﾃﾞﾊﾞｯｸﾞON ?
            MsgBox("START SW CHECK", vbOKOnly, "DEBUG")
#Else
            Call ZCONRST()                                              ' コンソールキーラッチ解除
            Call LAMP_CTRL(LAMP_START, False)                           ' STARTﾗﾝﾌﾟOFF

            ' STARTキー押下待ち 又は ｽﾗｲﾄﾞｶﾊﾞｰ/筐体ｶﾊﾞｰｸﾛｰｽﾞﾁｪｯｸ
            ' STARTキー押下待ち(インターック解除時)
            r = ORG_INTERLOCK_CHECK(InterlockSts, sts)                  ' インターロック状態取得
            If (InterlockSts = INTERLOCK_STS_DISABLE_FULL) Or _
                (gSysPrm.stSPF.giWithStartSw = 1) Or _
                (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then
                Call ORG_STARTRESET_SWWAIT(sts)                         ' STARTキー押下待ち
                If (sts <> cFRS_ERR_START) Then                         ' STARTキー押下以外 ? 
                    Select Case sts
                        Case ERR_OPN_CVR                                ' 筐体カバー開検出
                            GoTo STP_COVEROPEN
                        Case ERR_EMGSWCH                                ' 非常停止
                            GoTo STP_EMERGENCY
                        Case ERR_OPN_SCVR                               ' スライドカバー開検出
                            GoTo STP_COVEROPEN
                        Case Else
                            r = sts                                     ' INtime側からのReturn値設定
                            GoTo STP_END
                    End Select
                End If
            End If

            ' ラッチ解除
            Call ZCONRST()                                              ' コンソールキーラッチ解除
            Call COVERLATCH_CLEAR()                                     ' ｶﾊﾞｰ開ﾗｯﾁｸﾘｱ
#End If
            ' "原点復帰中"
            'V4.0.0.0-83↓
            If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then
                LblCaption.Text = MSG_SPRASH24
            Else
                LblCaption.Text = MSG_SPRASH0
            End If
            'V4.0.0.0-83↑
#If START_KEY_SOFT Then
            If gbStartKeySoft Then
                Call Sub_StartResetButtonDispOff()                          ' スタートボタンOFF
            End If
#End If

            'V5.0.0.1-24
            ' 筐体カバー閉を確認する
            r = Sub_CoverCheck()
            'V5.0.0.1-24

            Me.Refresh()
            Call LAMP_CTRL(LAMP_START, True)                            ' STARTﾗﾝﾌﾟON

            ' ローダ原点復帰処理 
            r = Sub_Loader_OrgBack(cGMODE_ORG)
            If (r <> cFRS_NORMAL) Then                                  ' エラー ? 
                GoTo STP_END
            End If

            ' シグナルタワー制御(On=原点復帰中,Off=全ﾋﾞｯﾄ) ###117
            Select Case (gSysPrm.stIOC.giSignalTower)
                Case SIGTOWR_NORMAL                                     ' 標準(原点復帰中(緑点滅))
                    'V5.0.0.9⑭ ↓ V6.0.3.0⑧(ローム殿仕様は無点灯)
                    ' r = ObjSys.SetSignalTower(SIGOUT_GRN_BLK, &HFFFF)
                    Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ZRN)
                    'V5.0.0.9⑭ ↑ V6.0.3.0⑧

                Case SIGTOWR_SPCIAL                                     ' 特注(原点復帰中(黄色点滅))
                    r = ObjSys.SetSignalTower(EXTOUT_YLW_BLK, &HFFFF)
            End Select
            'V4.0.0.0-83↓
            If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then
                LblCaption.Text = MSG_SPRASH46
                Me.Refresh()
            End If
            'V4.0.0.0-83↑

            ''V6.0.1.022↓通常の速度を転送する。 
            SetXYStageSpeed(StageSpeed.NormalSpeed)
            ''V6.0.1.022↑

            ' XYZθ軸初期化
            r = ObjSys.EX_SYSINIT(gSysPrm, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet)
            Call ObjSys.SetOrgFlg(False)                                ' 原点復帰処理中フラグをOFFに設定する
            If (r <> cFRS_NORMAL) Then                                  ' エラー ? (※メッセージは表示済)
                If (r = cFRS_ERR_EMG) Then GoTo STP_EMERGENCY ' 非常停止ならEMERGENCYへ
                If (r = cFRS_ERR_CVR) Or (r = cFRS_ERR_SCVR) Then GoTo STP_COVEROPEN
                GoTo STP_END                                            ' その他のエラーリターン 
            End If

            ' XYZ移動速度切替え
            r = ORG_INTERLOCK_CHECK(InterlockSts, sts)                  ' インターロック状態取得
            If (InterlockSts <> INTERLOCK_STS_DISABLE_NO) Then          ' インターロック状態(解除なし)でない ? ###108
                Call ZSELXYZSPD(1)                                      ' XYZ Slow speed
            Else
                Call ZSELXYZSPD(0)                                      ' XYZ Normal speed
            End If

            ' ロータリーアッテネータ制御  ※INtimeとはパラメータ順序が異なるので注意
            If (gSysPrm.stRMC.giRmCtrl2 >= 2) Then                       ' RMCTRL2対応 ?
                Call LATTSET(gSysPrm.stRAT.giAttFix, gSysPrm.stRAT.giAttRot)
            End If
            If ObjSys.SlideCoverCheck() Then GoTo STP_EMERGENCY '       ' ｽﾗｲﾄﾞｶﾊﾞｰｸﾛｰｽﾞでない ?

            ' 補正値等設定
            Call ObjSys.SetRangeCorrectionValue(gSysPrm)                ' レンジ補正値設定
            Call PROCPOWER(gSysPrm.stSPF.giProcPower2)                  ' 定電流値切り替え(250mA, 500mA)
            Call ZBPLOGICALCOORD(gSysPrm.stDEV.giBpDirXy)               ' BP座標系（象限）の設定
            Call ObjSys.SetBpLinearityData(gSysPrm)                     ' BPリニアリティー補正データ設定

            ' ｽﾗｲﾄﾞｶﾊﾞｰ自動ｵｰﾌﾟﾝ
            Call ZSLCOVEROPEN(0)                                        ' ｽﾗｲﾄﾞｶﾊﾞｰｵｰﾌﾟﾝﾊﾞﾙﾌﾞOFF
            Call ZSLCOVERCLOSE(0)                                       ' ｽﾗｲﾄﾞｶﾊﾞｰｸﾛｰｽﾞﾊﾞﾙﾌﾞOFF
            ' 吸着OFF
            Call ObjSys.AbsVaccume(gSysPrm, 0, giAppMode, giTrimErr)

            ' ｽﾀｰﾄSW押下待ちの場合はｽﾗｲﾄﾞｶﾊﾞｰ自動ｵｰﾌﾟﾝしない(ｵﾌﾟｼｮﾝ)
            If ((gSysPrm.stSPF.giWithStartSw = 0) And _
                 (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL432)) Then    ' スタートスイッチ待ちでなく、SL432系の場合。
                r = ObjSys.Z_COPEN(gSysPrm, 0, giTrimErr, False)        ' ｽﾗｲﾄﾞｶﾊﾞｰ自動ｵｰﾌﾟﾝ
                If (r <> 0) Then                                        ' エラー ?
                    If (r = cFRS_ERR_EMG) Then GoTo STP_EMERGENCY '     ' 非常停止ならEMERGENCYへ
                    GoTo STP_END                                        ' Return値 = ﾀｲﾑｱｳﾄ(ｽﾗｲﾄﾞｶﾊﾞｰ開待/ｽﾗｲﾄﾞｶﾊﾞｰｽﾄｯﾊﾟｰ行待)
                End If
            End If

            ' ﾗﾝﾌﾟ設定
            Call LAMP_CTRL(LAMP_START, False)                           ' STARTﾗﾝﾌﾟOFF
            Call LAMP_CTRL(LAMP_RESET, False)                           ' RESETﾗﾝﾌﾟOFF
            Call LAMP_CTRL(LAMP_PRB, False)                             ' PRBﾗﾝﾌﾟOFF
            Call ZCONRST()                                              ' コンソールキーラッチ解除
            If ObjSys.InterLockCheck() Then GoTo STP_EMERGENCY '        ' 非常停止 or 筐体ｶﾊﾞｰ開 ?

            ' 原点復帰終了処理
            giTrimErr = 0                                               ' ﾄﾘﾏｰ ｴﾗｰ ﾌﾗｸﾞ初期化
            ' シグナルタワー制御(On=原点復帰中,Off=全ﾋﾞｯﾄ) ###007
            Select Case (gSysPrm.stIOC.giSignalTower)
                Case SIGTOWR_NORMAL                                     ' 標準(無点灯)
                    'V5.0.0.9⑭ ↑ V6.0.3.0⑧(ローム殿仕様は無点灯)
                    '  r = ObjSys.SetSignalTower(0, &HEFFF)
                    Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALL_OFF)
                    'V5.0.0.9⑭ ↑ V6.0.3.0⑧(ローム殿仕様は無点灯)

                Case SIGTOWR_SPCIAL                                     ' 特注(黄色点灯)
                    r = ObjSys.SetSignalTower(EXTOUT_YLW_ON, &HEFFF)
            End Select

            gbInitialized = True                                        ' 原点復帰済
            r = cFRS_NORMAL                                             ' Return値 = 正常 
STP_END:
            Return (r)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FrmReset.Sub_OriginBack() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return値 = トラップエラー
        End Try
    End Function
#End Region

#Region "ローダアラーム発生時処理"
    '''=========================================================================
    ''' <summary>ローダアラーム発生時処理</summary>
    ''' <param name="Mode">(INP)処理モード</param>
    ''' <returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function Sub_LdrAlarm(ByVal Mode As Integer) As Integer

        Dim r As Integer
        Dim rtCode As Integer
        Dim strMSG As String
        Dim rtCodelock As Integer 'V4.1.0.0⑪ 

        Try
            ' 操作ログ出力
            Call LAMP_CTRL(LAMP_START, False)                           ' STARTランプOFF
            Call ObjSys.OperationLogging(gSysPrm, "ALARM", "LOADER")    ' 操作ログ出力
            Call Sub_SetMessage("", "", "", System.Drawing.SystemColors.ControlText, System.Drawing.SystemColors.ControlText, System.Drawing.SystemColors.ControlText)

            ' ローダ送信(ON=なし, OFF=ローダ原点復帰+トリミングＮＧ+パターン認識ＮＧ)
            'Call SetLoaderIO(&H0, LOUT_ORG_BACK + LOUT_TRM_NG + LOUT_PTN_NG)   '###070
            Call SetLoaderIO(&H0, LOUT_ORG_BACK + LOUT_TRM_NG)                  '###070

            ' ローダアラームチェック(False=アラームリストは表示しない)
            rtCode = Loader_AlarmCheck(ObjSys, False, AlarmCount, strLoaderAlarm, strLoaderAlarmInfo, strLoaderAlarmExec)

            ' ローダアラーム情報を設定する
            Me.Size = stSzLDE                                           ' Formの幅/高さをローダアラームモード用にする
            Call COVERCHK_ONOFF(COVER_CHECK_OFF)                        ' 「固定カバー開チェックなし」にする ###163
            '----- V1.18.0.1⑧↓ -----
            ' 電磁ロック(観音扉右側ロック)を解除する(ローム殿特注)
            'V4.1.0.0⑪↓
            ' rtCode = EL_Lock_OnOff(EX_LOK_MD_OFF)
            rtCodelock = EL_Lock_OnOff(EX_LOK_MD_OFF)
            If (rtCodelock = cFRS_TO_EXLOCK) Then                           ' 「前面扉ロック解除タイムアウト」なら戻り値を「RESET」にする
                rtCode = cFRS_ERR_RST
                GoTo STP_END                                            ' 終了処理へ
            End If
            'V4.1.0.0⑪↑
            If (rtCode < cFRS_NORMAL) Then                              ' 異常終了レベルのエラー ? 
                GoTo STP_END                                            ' アプリケーション強制終了
            End If
            '----- V1.18.0.1⑧↑ -----
            Call SetAlarmList(rtCode, AlarmCount, strLoaderAlarm, strLoaderAlarmInfo, strLoaderAlarmExec)

            ' メッセージ表示 '###073
            'If (rtCode = cFRS_ERR_LDR3) Then                            ' ローダアラーム検出(軽故障) ?
            If (rtCode = cFRS_ERR_LDR3) Or (rtCode = cFRS_ERR_LDR2) Then ' ローダアラーム/検出(軽故障, サイクル停止) ?
                ' OK/Cancelボタン表示
                BtnCancel.Location = LocCanBtn2                         ' Cancelボタン表示位置を右にずらす 
                BtnCancel.Visible = True                                ' Cancelボタン表示
                BtnOK.Visible = True                                    ' OKボタン表示
                ' "ローダエラー", "STARTキー：処理続行，RESETキー：処理終了", ""
                Call Sub_SetMessage(MSG_LOADER_05, MSG_SPRASH35, "", System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlText)

            Else                                                        ' ローダアラーム検出(軽故障以外) 
                ' Cancelボタン表示
                BtnCancel.Visible = True                                ' Cancelボタン表示
                ' "ローダエラー", "RESETキーを押すと処理を終了します", ""
                Call Sub_SetMessage(MSG_LOADER_05, MSG_SPRASH33, "", System.Drawing.Color.Red, System.Drawing.Color.Red, System.Drawing.SystemColors.ControlText)
            End If
            Me.Refresh()

            ' ----- V1.18.0.0③↓ -----
            ' 印刷用アラーム開始情報を設定する(ローム殿特注)
            ' アラーム発生回数とアラーム停止開始時間を設定する
            Call SetAlmStartTime()
            ' アラームファイルにアラームデータを書き込む
            Call WriteAlarmData(gFPATH_QR_ALARM, strLoaderAlarm(0), stPRT_ROHM.AlarmST_time, rtCode)
            ' ----- V1.18.0.0③↑ -----

            ' メッセージ表示してSTART/RESETｷｰ押下待ち '###073
#If START_KEY_SOFT Then
            If gbStartKeySoft Then
                Call Sub_ResetButtonDispOn()
            End If
#End If
            r = Sub_WaitStartRestKey(cFRS_ERR_START + cFRS_ERR_RST)     ' START/RESETｷｰ押下待ち
            ' ----- V1.18.0.0③↓ -----
            ' アラーム停止情報を設定する(ローム殿特注)
            Call SetAlmEndTime()
            ' ----- V1.18.0.0③↑ -----
            If (r = cFRS_ERR_EMG) Then                                  ' 非常停止検出 ?
                rtCode = cFRS_ERR_EMG                                   ' Return値 = 非常停止検出
                GoTo STP_END                                            ' 非常停止メッセージ表示へ
            ElseIf (r = cFRS_ERR_RST) Then
                'If (rtCode = cFRS_ERR_LDR3) Then                       ' 軽故障でRESETｷｰ押下なら
                If (rtCode = cFRS_ERR_LDR3) Or (rtCode = cFRS_ERR_LDR2) Then ' ###196 軽故障,サイクル停止 でRESETｷｰ押下なら
                    rtCode = cFRS_ERR_RST                               ' Return値 = RESETｷｰ押下で返す
                End If
            End If

            '----- ###163↓ -----
            ' 筐体カバー閉を確認する
            Call COVERLATCH_CLEAR()                                     ' カバー開ラッチのクリア
            Call COVERCHK_ONOFF(COVER_CHECK_ON)                         ' 「固定カバー開チェックあり」にする
            Me.Size = stSzNML                                           ' Formの幅/高さを標準モード用にする
            Me.Visible = False                                          ' メッセージ表示を消す 
            r = Sub_CoverCheck()
            If (r = cFRS_ERR_EMG) Then                                  ' 非常停止 ?
                rtCode = cFRS_ERR_EMG                                   ' Return値 = 非常停止検出
                GoTo STP_END                                            ' 非常停止メッセージ表示へ
            End If
            '----- ###163↑ -----
            '----- V1.18.0.1⑧↓ -----
            ' 電磁ロック(観音扉右側ロック)する(ローム殿特注)
            rtCode = EL_Lock_OnOff(EX_LOK_MD_ON)
            If (rtCode = cFRS_TO_EXLOCK) Then                           ' 「前面扉ロックタイムアウト」なら戻り値を「RESET」にする
                rtCode = cFRS_ERR_RST
                GoTo STP_END                                            ' 終了処理へ
            End If
            If (rtCode < cFRS_NORMAL) Then                              ' 異常終了レベルのエラー ? 
                GoTo STP_END                                            ' アプリケーション強制終了
            End If
            '----- V1.18.0.1⑧↑ -----
STP_END:
            BtnCancel.Location = LocCanBtnOrg                           ' Cancelボタン表示位置を戻す 
            BtnCancel.Text = "Cancel"
            BtnCancel.Visible = False                                   ' Cancelボタン非表示
            BtnOK.Visible = False                                       ' OKボタン非表示

            Me.Size = stSzNML                                           ' Formの幅/高さを通常モード用にする
            Return (rtCode)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FrmReset.Sub_LdrAlarm() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                      ' Retuen値 = ﾄﾗｯﾌﾟｴﾗｰ発生
        End Try
    End Function
#End Region

#Region "ローダアラーム情報を設定する"
    '''=========================================================================
    ''' <summary>ローダアラーム情報を設定する</summary>
    ''' <param name="AlarmKind">         (INP)アラーム種類(全停止異常, サイクル停止, 軽故障, アラーム無し)</param>
    ''' <param name="AlarmCount">        (INP)発生アラーム数</param>
    ''' <param name="strLoaderAlarm">    (INP)アラーム文字列</param>
    ''' <param name="strLoaderAlarmInfo">(INP)アラーム情報1(※未使用)</param>
    ''' <param name="strLoaderAlarmExec">(INP)アラーム情報(対策)</param>
    '''=========================================================================
    Private Sub SetAlarmList(ByVal AlarmKind As Integer, ByVal AlarmCount As Integer, _
                             ByRef strLoaderAlarm() As String, ByRef strLoaderAlarmInfo() As String, ByRef strLoaderAlarmExec() As String)

        Dim i As Integer
        Dim strMSG As String = ""

        Try
            ' ローダアラームリスト グループボックスのテキストを設定する
            Select Case AlarmKind
                Case cFRS_NORMAL                                                    ' アラームなし
                    GrpLdAlarm.Text = MSG_LOADER_16 & "(" & "---" & ")"             ' ローダアラームリスト(---)

                Case cFRS_ERR_LDR1 ' 全停止異常発生中
                    GrpLdAlarm.Text = MSG_LOADER_16 & "(" & MSG_SPRASH19 & ")"      ' ローダアラームリスト(全停止異常発生中)

                Case cFRS_ERR_LDR2 ' ｻｲｸﾙ停止異常発生中
                    GrpLdAlarm.Text = MSG_LOADER_16 & "(" & MSG_SPRASH20 & ")"      ' ローダアラームリスト(サイクル停止異常発生中)

                Case cFRS_ERR_LDR3  ' 軽故障発生中
                    GrpLdAlarm.Text = MSG_LOADER_16 & "(" & MSG_SPRASH21 & ")"      ' ローダアラームリスト(軽故障発生中)

                Case cFRS_ERR_PLC   ' PLCステータス異常
                    GrpLdAlarm.Text = MSG_LOADER_16 & "(" & MSG_SPRASH30 & ")"      ' ローダアラームリスト(PLCステータス異常)
            End Select

            '----- V6.1.1.0⑬↓ -----
            If (giAlmTimeDsp = 1) Then                                              ' ローダアラーム時の時間表示の有無(0=表示なし, 1=表示あり)　
                Call Get_NowYYMMDDHHMMSS(strMSG)
                GrpLdAlarm.Text = GrpLdAlarm.Text + " " + strMSG
            End If
            '----- V6.1.1.0⑬↑ -----

            ' ローダアラームをリストボックスに設定する
            Call ListAlarm.Items.Clear()
            'txtInfo.Text = ""
            TxtExec.Text = ""
            For i = 0 To (AlarmCount - 1)
                ListAlarm.Items.Add(strLoaderAlarm(i))                              ' アラーム文字列
            Next
            If (AlarmCount <= 0) Then Exit Sub

            ' アラーム情報(対策)をテキストボックスに設定する(最初は先頭を表示)
            ListAlarm.SelectedIndex = 0
            If (AlarmCount > 0) Then
                'txtInfo.Text = strLoaderAlarmInfo(0)        
                TxtExec.Text = strLoaderAlarmExec(0)
            End If
            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FrmReset.SetAlarmList() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "ローダアラームリストボックスクリックイベント処理"
    '''=========================================================================
    ''' <summary>ローダアラームリストボックスクリックイベント処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub ListAlarm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListAlarm.Click

        Dim Idx As Integer
        Dim strMSG As String

        Try
            ' リストボックスで選択されたローダアラームに対応するアラーム情報(対策)をテキストボックスに設定する
            Idx = ListAlarm.SelectedIndex
            'txtInfo.Text = strLoaderAlarmInfo(Idx)
            TxtExec.Text = strLoaderAlarmExec(Idx)                          ' アラーム情報(対策)をテキストボックスに設定する
            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FrmReset.ListAlarm_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "トリマエラー発生時処理"
    '''=========================================================================
    ''' <summary>トリマエラー発生時処理</summary>
    ''' <param name="Mode">(INP)処理モード</param>
    ''' <returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function Sub_TrimError(ByVal Mode As Integer) As Integer

        Dim r As Integer
        Dim ErrCode As Short                                            ' V1.18.0.0③
        Dim strMSG As String
        Dim strMSG2 As String

        Try
            ' 操作ログ出力
            If (Mode = cGMODE_ERR_HING) Then
                strMSG = "ALARM"
                strMSG2 = "HI-NG"
            Else
                strMSG = "ALARM"
                strMSG2 = "RE-PROBING-NG"
            End If
            Call ObjSys.OperationLogging(gSysPrm, strMSG, strMSG2)

            '----- ###216↓ -----
            ''----- ###181↓ -----
            'If (bFgAutoMode = True) Then                                   ' 自動運転 ?
            '    ' ローダ送信(ON=なし, OFF=ローダ原点復帰+トリミングＮＧ+パターン認識ＮＧ)
            '    'Call SetLoaderIO(&H0, LOUT_ORG_BACK + LOUT_TRM_NG + LOUT_PTN_NG)   '###070
            '    Call SetLoaderIO(&H0, LOUT_ORG_BACK + LOUT_TRM_NG)                  '###070
            'End If
            ''----- ###181↑ -----
            '----- ###216↑ -----

            ' シグナルタワー制御(On=異常+ﾌﾞｻﾞｰ1, Off=全ﾋﾞｯﾄ) ###007
            Select Case (gSysPrm.stIOC.giSignalTower)
                Case SIGTOWR_NORMAL                                     ' 標準(赤点滅+ブザー１)
                    'V5.0.0.9⑭ ↓ V6.0.3.0⑧(ローム殿仕様は赤点滅＋ブザーＯＮ)
                    ' Call Form1.System1.SetSignalTower(SIGOUT_RED_BLK Or SIGOUT_BZ1_ON, &HFFFF)
                    Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALARM)
                    'V5.0.0.9⑭ ↑ V6.0.3.0⑧

                Case SIGTOWR_SPCIAL                                     ' 特注(赤点滅+ブザー１)
                    'r = Form1.System1.SetSignalTower(EXTOUT_RED_BLK Or EXTOUT_BZ1_ON, &HFFFF)
            End Select

            ' メッセージ表示
            If (Mode = cGMODE_ERR_HING) Then
                ErrCode = cFRS_ERR_HING                                 ' V1.18.0.0③
                strMSG = MSG_SPRASH13                                   ' "連続NG-HIエラー"
                strMSG2 = MSG_frmLimit_07                               ' "STARTキーかOKボタンを押してください"
            Else
                ErrCode = cFRS_ERR_REPROB                               ' V1.18.0.0③
                strMSG = MSG_SPRASH14                                   ' "再プロービング失敗"
                strMSG2 = MSG_frmLimit_07                               ' "STARTキーかOKボタンを押してください"
            End If
            Call Sub_SetMessage(strMSG, strMSG2, "", System.Drawing.Color.Red, System.Drawing.SystemColors.ControlText, System.Drawing.SystemColors.ControlText)
            Me.Refresh()
            BtnCancel.Text = "OK"
            BtnCancel.Visible = True                                    ' OKボタン表示

            ' ----- V1.18.0.0③↓ -----
            ' 印刷用アラーム開始情報を設定する(ローム殿特注)
            ' アラーム発生回数とアラーム停止開始時間を設定する
            Call SetAlmStartTime()
            ' アラームファイルにアラームデータを書き込む
            Call WriteAlarmData(gFPATH_QR_ALARM, strMSG, stPRT_ROHM.AlarmST_time, ErrCode)
            ' ----- V1.18.0.0③↑ -----

            ' メッセージ表示してSTARTキー押下待ち(OKボタンも有効)
            r = Sub_WaitStartRestKey(cFRS_ERR_START)                    ' STARTｷｰ押下待ち
            ' ----- V1.18.0.0③↓ -----
            ' アラーム停止情報を設定する(ローム殿特注)
            Call SetAlmEndTime()
            ' ----- V1.18.0.0③↑ -----
            If (r = cFRS_ERR_EMG) Then                                  ' 非常停止検出 ?
                GoTo STP_END                                            ' 非常停止メッセージ表示へ
            End If

            '----- ###181↓ -----
            ' シグナルタワー制御(On=なし, Off=異常+ﾌﾞｻﾞｰ1) 
            Select Case (gSysPrm.stIOC.giSignalTower)
                Case SIGTOWR_NORMAL                                     ' 標準
                    'V5.0.0.9⑭ ↓ V6.0.3.0⑧
                    ' Call Form1.System1.SetSignalTower(0, SIGOUT_RED_BLK Or SIGOUT_BZ1_ON)
                    Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALL_OFF)
                    'V5.0.0.9⑭ ↑ V6.0.3.0⑧

                Case SIGTOWR_SPCIAL                                     ' 特注
                    'r = Form1.System1.SetSignalTower(0, EXTOUT_RED_BLK Or EXTOUT_BZ1_ON)
            End Select

            '----- ###216↓ -----
            '' ローダ用トリマエラー発生時処理(SL432R系はNOP) 
            'If (bFgAutoMode = True) Then                                   ' 自動運転 ?
            '    r = Sub_Loader_TrimError()
            '    If (r <> cFRS_NORMAL) Then                                  ' エラー ? 
            '        GoTo STP_END
            '    End If
            'End If
            '----- ###216↑ -----
            '----- ###181↑ -----
STP_END:
            BtnCancel.Text = "Cancel"
            BtnCancel.Visible = False                                   ' Cancelボタン非表示
            Return (r)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FrmReset.Sub_TrimError() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Retuen値 = ﾄﾗｯﾌﾟｴﾗｰ発生
        End Try
    End Function
#End Region

#Region "トリミング中のｽﾗｲﾄﾞｶﾊﾞｰ開/筐体ｶﾊﾞｰ開メッセージ表示処理"
    '''=========================================================================
    ''' <summary>トリミング中のｽﾗｲﾄﾞｶﾊﾞｰ開/筐体ｶﾊﾞｰ開メッセージ表示</summary>
    ''' <param name="Mode">(INP)処理モード</param>
    ''' <returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function Sub_CvrOpen(ByVal Mode As Integer) As Integer

        Dim r As Integer
        Dim rtnCode As Integer
        Dim strMSG As String

        Try
            ' シグナルタワー制御(On=異常+ﾌﾞｻﾞｰ1, Off=全ﾋﾞｯﾄ) ###007
            Select Case (gSysPrm.stIOC.giSignalTower)
                Case SIGTOWR_NORMAL                                     ' 標準(赤点滅+ブザー１)
                    'V5.0.0.9⑭ ↓ V6.0.3.0⑧
                    ' Call Form1.System1.SetSignalTower(SIGOUT_RED_BLK Or SIGOUT_BZ1_ON, &HFFFF)
                    Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALL_OFF)
                    'V5.0.0.9⑭ ↑ V6.0.3.0⑧
                Case SIGTOWR_SPCIAL                                     ' 特注(赤点滅+ブザー１)
                    'r = Form1.System1.SetSignalTower(EXTOUT_RED_BLK Or EXTOUT_BZ1_ON, &HFFFF)
            End Select

            Call LAMP_CTRL(LAMP_START, False)                           ' STARTﾗﾝﾌﾟOFF

            ' メッセージ表示
            If (gMode = cGMODE_CVR_OPN) Then                            ' 筐体ｶﾊﾞｰ開 ?
                ' "筐体カバーが開きました", "RESETキーを押すとプログラムを終了します", ""
                strMSG = MSG_SPRASH27                                   ' V1.18.0.0③
                Call Sub_SetMessage(MSG_SPRASH27, MSG_SPRASH16, "", System.Drawing.Color.Red, System.Drawing.Color.Red, System.Drawing.SystemColors.ControlText)
                rtnCode = cFRS_ERR_CVR                                  ' Return値 = 筐体カバー開検出
            Else
                ' "スライドカバーが開きました", "RESETキーを押すとプログラムを終了します", ""
                strMSG = MSG_SPRASH28                                   ' V1.18.0.0③
                Call Sub_SetMessage(MSG_SPRASH28, MSG_SPRASH16, "", System.Drawing.Color.Red, System.Drawing.Color.Red, System.Drawing.SystemColors.ControlText)
                rtnCode = cFRS_ERR_SCVR                                 ' Return値 = スライドカバー開検出
            End If
            Me.Refresh()

            ' ----- V1.18.0.0③↓ -----
            ' 印刷用アラーム開始情報を設定する(ローム殿特注)
            ' アラーム発生回数とアラーム停止開始時間を設定する
            Call SetAlmStartTime()
            ' アラームファイルにアラームデータを書き込む
            Call WriteAlarmData(gFPATH_QR_ALARM, strMSG, stPRT_ROHM.AlarmST_time, rtnCode)
            ' ----- V1.18.0.0③↑ -----

            ' メッセージ表示してRESETｷｰ押下待ち
            r = Sub_WaitStartRestKey(cFRS_ERR_RST)                      ' RESETｷｰ押下待ち
            ' ----- V1.18.0.0③↓ -----
            ' アラーム停止情報を設定する(ローム殿特注)
            Call SetAlmEndTime()
            ' ----- V1.18.0.0③↑ -----
            If (r = cFRS_ERR_EMG) Then                                  ' 非常停止検出 ?
                rtnCode = cFRS_ERR_EMG
                GoTo STP_END                                            ' 非常停止メッセージ表示へ
            End If

STP_END:
            Return (rtnCode)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FrmReset.Sub_CvrOpen() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                      ' Retuen値 = ﾄﾗｯﾌﾟｴﾗｰ発生
        End Try
    End Function
#End Region
    '----- V1.18.0.0⑨↓ -----
#Region "マガジン交換メッセージ表示処理(ローム殿特注)"
    '''=========================================================================
    ''' <summary>マガジン交換メッセージ表示(ローム殿特注)</summary>
    ''' <returns>cFRS_ERR_START  = STARTキー又はOKボタン押下
    '''          cFRS_ERR_RST    = RESETキー又はCancelボタン押下
    '''          上記以外        = エラー</returns>
    '''=========================================================================
    Private Function Sub_MagazineExchange() As Integer

        Dim Md As Integer = 0
        Dim sts As Long = 0
        Dim r As Long = 0
        Dim strMSG As String

        Try
            '-------------------------------------------------------------------
            '   初期処理
            '-------------------------------------------------------------------
            ' OK/Cancelボタン表示設定
            BtnCancel.Location = LocCanBtn2                             ' Cancelボタン表示位置を右にずらす 
            BtnCancel.Visible = True                                    ' Cancelボタン表示
            BtnOK.Visible = True                                        ' OKボタン表示
            '----- V4.0.0.0⑲↓ -----
            ' SL436Rのローム殿特注以外はOKボタンは表示する
            'BtnOK.Enabled = False                                      ' OKボタン非活性化
            BtnOK.Enabled = True                                        ' OKボタン非活性化
            If ((gSysPrm.stCTM.giSPECIAL = customROHM) And (giMachineKd <> MACHINE_KD_RS)) Then
                BtnOK.Enabled = False                                    ' OKボタン非活性化
            End If
            '----- V4.0.0.0⑲↑ -----

            Call COVERCHK_ONOFF(COVER_CHECK_OFF)                        ' 「固定カバー開チェックなし」にする

            '----- V1.18.0.1⑧↓ -----
            ' 電磁ロック(観音扉右側ロック)を解除する(ローム殿特注(SL436R/SL436S))
            r = EL_Lock_OnOff(EX_LOK_MD_OFF)
            If (r = cFRS_TO_EXLOCK) Then                                ' 「前面扉ロック解除タイムアウト」なら戻り値を「RESET」にする
                mExitFlag = cFRS_ERR_RST
                GoTo STP_EXIT                                           ' 終了処理へ
            End If
            If (r < cFRS_NORMAL) Then                                   ' 異常終了レベルのエラー ? 
                mExitFlag = r
                GoTo STP_EXIT                                           ' アプリケーション強制終了
            End If
            '----- V1.18.0.1⑧↑ -----
            '----- V4.0.0.0-25↓ -----
            ' マガジン終了時のシグナルタワー制御
            If (giMachineKd = MACHINE_KD_RS) Then
                ' シグナルタワー制御(On=緑点滅+ブザー１, Off=全ﾋﾞｯﾄ) SL436S時
                'V5.0.0.9⑭ ↓ V6.0.3.0⑧(ローム殿仕様は緑点滅＋ブザーＯＮ)
                ' Call Form1.System1.SetSignalTower(SIGOUT_GRN_BLK Or SIGOUT_BZ1_ON, &HFFFF)
                Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_OPERATION_END)
                'V5.0.0.9⑭ ↑ V6.0.3.0⑧

            Else
                ' シグナルタワー制御(On=赤点滅+ブザー１, Off=全ﾋﾞｯﾄ) SL436R時
                'Call Form1.System1.SetSignalTower(SIGOUT_RED_BLK Or SIGOUT_BZ1_ON, &HFFFF)     'V6.1.1.0④
                Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALARM)               'V6.1.1.0④
            End If

            '' シグナルタワー制御(On=赤点滅+ブザー１, Off=全ﾋﾞｯﾄ)
            'Call Form1.System1.SetSignalTower(SIGOUT_RED_BLK Or SIGOUT_BZ1_ON, &HFFFF)
            '----- V4.0.0.0-25↑ -----

            ' メッセージ表示
            ' "マガジン終了", "OKボタン押下で自動運転を続行します", "Cancelボタン押下で自動運転を終了します"
            Call Sub_SetMessage(MSG_LOADER_44, MSG_LOADER_45, MSG_LOADER_46, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
            Me.Refresh()
            Call ZCONRST()                                              ' コンソールキーラッチ解除
            mExitFlag = 0
            Md = 0

            '-------------------------------------------------------------------
            '   マガジン交換確認
            '-------------------------------------------------------------------
            Do
                ' マガジン交換確認のため、マガジン2無しチェック後、マガジン2有かつ表チェックを行う
                If (BtnOK.Enabled = False) Then                         ' OKボタンが活性化ならマガジン無/有チェックは行わない
                    r = Sub_MagazineExitCheck(Md)                       ' マガジン無/有チェック
                    If (r = cFRS_NORMAL) Then
                        If (Md = 0) Then                                ' マガジン無しチェックOKなら 
                            Md = 1                                      ' マガジン2有チェックを行う
                        Else                                            ' マガジン2有かつ表チェックOKなら  
                            BtnOK.Enabled = True                        ' OKボタンを活性化する
                        End If
                    End If
                End If

                ' START/RESETキー押下待ち
                r = STARTRESET_SWCHECK(False, sts)                      ' START/RESET SW押下チェック
                If (r = cFRS_ERR_EMG) Then
                    mExitFlag = cFRS_ERR_EMG
                    GoTo STP_EXIT
                End If

                ' RESETキー押下 ?
                If (sts = cFRS_ERR_RST) Then                            ' RESETキー又はCancelボタン押下 ? 
                    mExitFlag = cFRS_ERR_RST                            ' Retuen値 = RESETキー又はCancelボタン押下
                    Exit Do                                             ' 筐体カバー閉確認へ
                End If

                ' STARTキー押下 ?
                If ((BtnOK.Enabled = True) And (sts = cFRS_ERR_START)) Then ' STARTキー又はOKボタン押下 ? V1.18.0.7②
                    mExitFlag = cFRS_ERR_START                          ' Retuen値 = STARTキー又はOKボタン押下
                    Exit Do                                             ' 筐体カバー閉確認へ
                End If

                System.Windows.Forms.Application.DoEvents()             ' メッセージポンプ
                Call System.Threading.Thread.Sleep(100)                 ' Wait(msec)
                If ObjSys.EmergencySwCheck() Then                       ' 非常停止 ?
                    mExitFlag = cFRS_ERR_EMG                            ' Retuen値 = 非常停止
                    GoTo STP_EXIT                                       ' 非常停止メッセージ表示へ
                End If
                Me.BringToFront()                                       ' 最前面に表示 

            Loop While (mExitFlag = 0)

            '----- V6.0.3.0_21↓ -----           ↓
            ' 自動運転終了が選択された場合、投入基板枚数と処理基板枚数の差により終了を再確認する
            If ((mExitFlag = cFRS_ERR_RST) AndAlso (FormLotEnd.DoConfirm)) Then
                Using frm As New FormLotEnd(gSysPrm.stTMN.giMsgTyp)
                    Me.Enabled = False
                    frm.Show(Me)
                    mExitFlag = frm.Execute(ObjSys)
                    frm.Close()
                    Me.Enabled = True
                    If (cFRS_ERR_EMG = mExitFlag) Then
                        GoTo STP_EXIT                                   ' 非常停止メッセージ表示へ
                    End If
                End Using
            End If
            '----- V6.0.3.0_21↑ -----          ↑

            ' シグナルタワー制御(On=なし, Off=赤点滅+ブザー１)
            'V5.0.0.9⑭ ↓ V6.0.3.0⑧
            'Call Form1.System1.SetSignalTower(0, SIGOUT_RED_BLK Or SIGOUT_BZ1_ON)                      ' V4.0.0.0-25
            ' Call Form1.System1.SetSignalTower(0, SIGOUT_RED_BLK Or SIGOUT_BZ1_ON Or SIGOUT_GRN_BLK)     ' V4.0.0.0-25
            Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_OPERATION_END)
            'V5.0.0.9⑭ ↑ V6.0.3.0⑧

            '-------------------------------------------------------------------
            '   終了処理
            '-------------------------------------------------------------------
            ' 筐体カバー閉を確認する
            Call COVERLATCH_CLEAR()                                     ' カバー開ラッチのクリア
            Call COVERCHK_ONOFF(COVER_CHECK_ON)                         ' 「固定カバー開チェックあり」にする
            Me.Size = stSzNML                                           ' Formの幅/高さを標準モード用にする
            Me.Visible = False                                          ' メッセージ表示を消す 
            r = Sub_CoverCheck()                                        ' 筐体カバー閉を確認する
            If (r = cFRS_ERR_EMG) Then                                  ' 非常停止 ?
                mExitFlag = cFRS_ERR_EMG                                ' Retuen値 = 非常停止
                GoTo STP_EXIT                                           ' 非常停止メッセージ表示へ
            End If

            '----- V1.18.0.1⑧↓ -----
            ' 電磁ロック(観音扉右側ロック)する(ローム殿特注(SL436R/SL436S))
            r = EL_Lock_OnOff(EX_LOK_MD_ON)
            If (r = cFRS_TO_EXLOCK) Then                                ' 「前面扉ロックタイムアウト」なら戻り値を「RESET」にする
                mExitFlag = cFRS_ERR_RST
                GoTo STP_EXIT                                           ' 終了処理へ
            End If
            If (r < cFRS_NORMAL) Then                                   ' 異常終了レベルのエラー ? 
                mExitFlag = r
                GoTo STP_EXIT                                           ' アプリケーション強制終了
            End If
            '----- V1.18.0.1⑧↑ -----

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FrmReset.Sub_MagazineExchange() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            mExitFlag = cERR_TRAP                                       ' Retuen値 = ﾄﾗｯﾌﾟｴﾗｰ発生
        End Try

STP_EXIT:
        BtnCancel.Visible = False                                       ' Cancelボタン非表示
        BtnOK.Visible = False                                           ' OKボタン非表示
        BtnCancel.Text = "Cancel"
        Return (mExitFlag)
    End Function
#End Region
    '----- V1.18.0.0⑨↑ -----
    '----- ###188↓ -----
#Region "ステージを原点に戻す(残基板取り除くため)"
    '''=========================================================================
    ''' <summary>ステージを原点に戻す(残基板取り除くため)</summary>
    ''' <returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function Sub_XY_OrgBack() As Integer

        Dim r As Integer
        Dim rtnCode As Integer = cFRS_NORMAL
        Dim strMSG As String

        Try
            ' ハンドを上昇させる(ハンドが下がっている場合)
            W_HAND1_UP()                                                ' 供給ハンド
            W_HAND2_UP()                                                ' 収納ハンド

            ' ステージを原点に戻す(XYZθ軸初期化)
            r = Form1.System1.EX_SYSINIT(gSysPrm, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet)
            If (r <> cFRS_NORMAL) Then                                  ' エラー ? (※メッセージは表示済)
                Return (r)
            End If

            Return (rtnCode)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FrmReset.Sub_XY_OrgBack() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                      ' Retuen値 = ﾄﾗｯﾌﾟｴﾗｰ発生
        End Try
    End Function
#End Region
    '----- ###188↑ -----
#Region "フォームに表示するメッセージを設定する"
    '''=========================================================================
    ''' <summary>フォームに表示するメッセージを設定する</summary>
    ''' <param name="strMSG1">(INP)LblCaptionに表示する文字列</param>
    ''' <param name="strMSG2">(INP)Label1に表示する文字列</param>
    ''' <param name="strMSG3">(INP)Label2に表示する文字列</param>
    ''' <param name="Color1"> (INP)LblCaptionの文字の色</param>
    ''' <param name="Color2"> (INP)Label1の文字の色</param>
    ''' <param name="Color3"> (INP)Label2の文字の色</param>
    '''=========================================================================
    Private Sub Sub_SetMessage(ByVal strMSG1 As String, ByVal strMSG2 As String, ByVal strMSG3 As String, _
                                 ByVal Color1 As Color, ByVal Color2 As Color, ByVal Color3 As Color)           'V6.0.0.0⑱
        'Private Sub Sub_SetMessage(ByVal strMSG1 As String, ByVal strMSG2 As String, ByVal strMSG3 As String, _
        '                             ByVal Color1 As Object, ByVal Color2 As Object, ByVal Color3 As Object)

        Dim strMSG As String

        Try
            ' メッセージ設定
            LblCaption.ForeColor = Color1
            Label1.ForeColor = Color2
            Label2.ForeColor = Color3
            LblCaption.Text = strMSG1
            Label1.Text = strMSG2
            Label2.Text = strMSG3
            'V5.0.0.4①↓メッセージを改行させて表示する時文字が隠れない様にシフトさせる。
            If Label1.Text.Contains(ControlChars.CrLf) Then
                Label1.Location = New System.Drawing.Point(Label1.Location.X, Label1.Location.Y - 15)
                Label1.Size = New System.Drawing.Size(Label1.Size.Width, Label1.Size.Height + 35)
                '                Label2.Location = New System.Drawing.Point(Label2.Location.X, Label2.Location.Y)
            End If
            If Label2.Text.Contains(ControlChars.CrLf) Then
                Label1.Location = New System.Drawing.Point(Label1.Location.X, Label1.Location.Y - 5)
                Label2.Location = New System.Drawing.Point(Label2.Location.X, Label2.Location.Y - 20)
                Label2.Size = New System.Drawing.Size(Label2.Size.Width, Label2.Size.Height + 35)
            End If
            'V5.0.0.4①↑
            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FrmReset.Sub_SetMessage() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "Start/Resetキー押下待ちｻﾌﾞﾙｰﾁﾝ"
    '''=========================================================================
    ''' <summary>Start/Resetキー押下待ちｻﾌﾞﾙｰﾁﾝ</summary>
    ''' <param name="Md">(INP)cFRS_ERR_START                = STARTキー押下待ち
    '''                       cFRS_ERR_RST                  = RESETキー押下待ち
    '''                       cFRS_ERR_START + cFRS_ERR_RST = START/RESETキー押下待ち
    ''' </param>
    ''' <returns>cFRS_ERR_START = STARTキー押下
    '''          cFRS_ERR_RST   = RESETキー押下
    '''          上記以外=エラー
    ''' </returns>
    '''=========================================================================
    Private Function Sub_WaitStartRestKey(ByVal Md As Integer) As Integer

        Dim sts As Long = 0
        Dim r As Long = 0
        Dim strMSG As String

        Try
            ' パラメータチェック
            If (Md = 0) Then
                Return (-1 * ERR_CMD_PRM)                           ' パラメータエラー
            End If

#If cOFFLINEcDEBUG Then                                             ' OffLineﾃﾞﾊﾞｯｸﾞON ?(↓FormResetが最前面表示なので下記のようにしないとMsgBoxが最前面表示されない)
            Dim Dr As System.Windows.Forms.DialogResult
            Dr = MessageBox.Show("START SW CHECK", "Debug", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly)
            If (Dr = System.Windows.Forms.DialogResult.OK) Then
                mExitFlag = cFRS_ERR_START                          ' Return値 = STARTキー押下
            Else
                mExitFlag = cFRS_ERR_RST                            ' Return値 = RESETキー押下
            End If
            Return (mExitFlag)
#End If

            ' START/RESETキー押下待ち(Ok/Cancelボタンも有効)
            Call ZCONRST()                                          ' コンソールキーラッチ解除
            mExitFlag = 0
            Do
                r = STARTRESET_SWCHECK(False, sts)                  ' START/RESET SW押下チェック
                If (sts = cFRS_ERR_RST) And ((Md = cFRS_ERR_RST) Or (Md = cFRS_ERR_START + cFRS_ERR_RST)) Then
                    mExitFlag = cFRS_ERR_RST                        ' ExitFlag = Cancel(RESETキー)
                ElseIf (sts = cFRS_ERR_START) And ((Md = cFRS_ERR_START) Or (Md = cFRS_ERR_START + cFRS_ERR_RST)) Then
                    mExitFlag = cFRS_ERR_START                      ' ExitFlag = OK(STARTキー)
                End If

                System.Windows.Forms.Application.DoEvents()         ' メッセージポンプ
                Call System.Threading.Thread.Sleep(1)               ' Wait(msec)
                If ObjSys.EmergencySwCheck() Then                   ' 非常停止 ?
                    mExitFlag = cFRS_ERR_EMG                        ' Return値 = 非常停止検出
                End If
                Me.BringToFront()                                   ' 最前面に表示 
            Loop While (mExitFlag = 0)

            Return (mExitFlag)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FrmReset.Sub_WaitRestKey() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                      ' Retuen値 = ﾄﾗｯﾌﾟｴﾗｰ発生
        End Try
    End Function
#End Region

#Region "非常停止メッセージ表示"
    '''=========================================================================
    ''' <summary>非常停止メッセージ表示</summary>
    ''' <returns></returns>
    '''=========================================================================
    Private Function Sub_DispEmergencyMsg() As Integer

        Dim strMSG As String

        Try
            ' シグナルタワー3色制御あり(特注) ?
            If (gSysPrm.stIOC.giSigTwr2Flag = 1) Then
                ' シグナルタワー３色制御(赤点滅) (EXTOUT(OnBit, OffBit))
                Call ObjSys.EXTIO_Out_Sub(gSysPrm.stIOC.glSigTwr2_Out_Adr, gSysPrm.stIOC.glSigTwr2_Red_Blnk, _
                                           gSysPrm.stIOC.glSigTwr2_Red_On Or gSysPrm.stIOC.glSigTwr2_Yellow_On Or gSysPrm.stIOC.glSigTwr2_Yellow_Blnk)
                ' ブザー制御あり(特注) ?
                If (gSysPrm.stIOC.giBuzerCtrlFlag = 1) Then
                    ' ブザー音2(ピ～ピッピ) (EXTOUT(OnBit, OffBit))
                    Call ObjSys.EXTIO_Out_Sub(gSysPrm.stIOC.glBuzerCtrl_Out_Adr, gSysPrm.stIOC.glBuzerCtrl_Out2, gSysPrm.stIOC.glBuzerCtrl_Out1)
                End If
            End If

            ' ----- V1.18.0.0③↓ -----
            ' 印刷用アラーム開始情報を設定する(ローム殿特注)
            ' アラーム発生回数とアラーム停止開始時間を設定する
            Call SetAlmStartTime()
            ' アラームファイルにアラームデータを書き込む(非常停止)
            Call WriteAlarmData(gFPATH_QR_ALARM, MSG_SPRASH6, stPRT_ROHM.AlarmST_time, cFRS_ERR_EMG)
            ' ----- V1.18.0.0③↑ -----

            ' メッセージ表示
            ' "非常停止しました", "Cancelボタン押下でプログラムを終了します", ""
            ''V4.0.0.0-71            Call Sub_SetMessage(MSG_SPRASH6, "", MSG_SPRASH17, System.Drawing.Color.Red, System.Drawing.Color.Red, System.Drawing.Color.Red)
            Call Sub_SetMessage(MSG_SPRASH6, MSG_SPRASH45, MSG_SPRASH17, System.Drawing.Color.Red, System.Drawing.Color.Red, System.Drawing.Color.Red)
            Me.Refresh()
            mExitFlag = cFRS_NORMAL                                     ' ExitFlg = 初期化
            BtnCancel.Visible = True                                    ' Cancelボタン表示

            ' Cancelボタンの押下を待つ
            Do
                System.Windows.Forms.Application.DoEvents()             ' メッセージポンプ
                Call System.Threading.Thread.Sleep(1)                   ' Wait(msec)
            Loop While (mExitFlag = cFRS_NORMAL)
            BtnCancel.Visible = False                                   ' Cancelボタン非表示

            ' ----- V1.18.0.0③↓ -----
            ' アラーム停止情報を設定する(ローム殿特注)
            Call SetAlmEndTime()
            ' ----- V1.18.0.0③↑ -----

            Return (cFRS_ERR_EMG)                                       ' Retuen値 = 非常停止

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FrmReset.Sub_DispEmergencyMsg() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Retuen値 = ﾄﾗｯﾌﾟｴﾗｰ発生
        End Try
    End Function
#End Region

#Region "リミットセンサー& 軸エラー & タイムアウト等"
    '''=========================================================================
    ''' <summary>リミットセンサー/軸エラー/タイムアウト等
    '''          INtime側からのエラーメッセージを表示する</summary>
    ''' <param name="gMode">    (INP)エラー番号</param>
    ''' <param name="giTrimErr">(INP)トリマエラーフラグ(0=ﾄﾘﾏｰｴﾗｰでない, 0以外=ﾄﾘﾏｰｴﾗｰ)</param>
    ''' <returns>gMode</returns>
    '''=========================================================================
    Private Function Sub_ErrAxis(ByVal gMode As Integer, ByVal giTrimErr As Integer) As Integer

        Dim bMsgDspMode As Boolean
        Dim r As Integer
        Dim Md As Integer
        Dim strMSG As String

        Try
            ' 初期処理
            r = gMode                                                   ' Return値設定
            Call LAMP_CTRL(LAMP_START, False)                           ' STATRﾗﾝﾌﾟOFF

            ' シグナルタワー制御(On=異常+ﾌﾞｻﾞｰ1, Off=全ﾋﾞｯﾄ) ###007
            If (ObjSys.IsSoftLimitCode(gMode)) Then                     ' ソフトリミットエラー ?
                ' ｼｸﾞﾅﾙﾀﾜｰ制御なし
            Else
                ' ｼｸﾞﾅﾙﾀﾜｰ制御(On=異常+ﾌﾞｻﾞｰ1, Off=全ﾋﾞｯﾄ)
                'V5.0.0.9⑭ ↓ V6.0.3.0⑧(ローム殿仕様は赤点滅＋ブザーＯＮ)
                ' r = ObjSys.SetSignalTower(SIGOUT_RED_BLK Or SIGOUT_BZ1_ON, &HFFFF) ' 標準(赤点滅+ブザー１) ###007
                Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALARM)
                'V5.0.0.9⑭ ↑ V6.0.3.0⑧
            End If

            ObjSys.GetMsgDispMode(bMsgDspMode)
            If (bMsgDspMode = False) Then Exit Function '               ' メッセージ表示なしならNOP

            '---------------------------------------------------------------------------
            '   メッセージ設定 ###008
            '---------------------------------------------------------------------------
            strMSG = ""
            Select Case (gMode)
                Case ERR_SOFTLIMIT_X                                    ' X軸ソフトリミット
                    strMSG = MSG_frmLimit_03                            ' "X軸リミット"
                Case ERR_SOFTLIMIT_Y                                    ' Y軸ソフトリミット
                    strMSG = MSG_frmLimit_04                            ' "Y軸リミット"
                Case ERR_SOFTLIMIT_Z                                    ' Z軸ソフトリミット
                    strMSG = MSG_frmLimit_05                            ' "Z軸リミット
                Case ERR_SOFTLIMIT_Z2                                   ' Z2軸ソフトリミット
                    strMSG = MSG_frmLimit_06                            ' "Z2軸リミット"
                Case ERR_BP_XLIMIT                                      ' BP X軸ソフトリミットエラー
                    strMSG = MSG_frmLimit_01                            ' "BPリミット"
                Case ERR_BP_YLIMIT                                      ' BP Y軸ソフトリミットエラー
                    strMSG = MSG_frmLimit_01                            ' "BPリミット"

                Case ERR_BP_LIMITOVER                                   ' BP移動距離設定：リミットオーバー
                    strMSG = MSG_BP_LIMITOVER                           ' "BP移動距離設定リミットオーバー"
                Case ERR_BP_HARD_LIMITOVER_LO                           ' BP移動距離設定：リミットオーバー（最小可動範囲以下）
                    strMSG = MSG_BP_HARD_LIMITOVER_LO                   ' "BPリミットオーバー（最小可動範囲以下）"
                Case ERR_BP_HARD_LIMITOVER_HI                           ' BP移動距離設定：リミットオーバー（最大可動範囲以上）
                    strMSG = MSG_BP_HARD_LIMITOVER_HI                   ' "BPリミットオーバー（最大可動範囲以上）"
                Case ERR_BP_SOFT_LIMITOVER                              ' ソフト可動範囲オーバー
                    strMSG = MSG_BP_SOFT_LIMITOVER                      ' "BPソフト可動範囲オーバー"
                Case ERR_BP_BSIZE_OVER                                  ' ブロックサイズ設定オーバー（ソフト可動範囲オーバー）
                    strMSG = MSG_BP_BSIZE_OVER                          ' "ブロックサイズ設定オーバー（ソフト可動範囲外）"
                Case ERR_BP_MOVE_TIMEOUT
                    strMSG = MSG_BP_MOVE_TIMEOUT                        ' "BP タイムアウト"
                Case ERR_BP_GRV_ALARM_X
                    strMSG = MSG_BP_GRV_ALARM_X                         ' "ガルバノアラームX"
                Case ERR_BP_GRV_ALARM_Y
                    strMSG = MSG_BP_GRV_ALARM_Y                         ' "ガルバノアラームY"

                Case ERR_SRV_ALM
                    strMSG = MSG_SRV_ALM                                ' "サーボアラーム"
                Case ERR_AXS_LIM_X                                      ' X軸リミット検出
                    strMSG = MSG_AXS_LIM_X
                Case ERR_AXS_LIM_Y                                      ' Y軸リミット検出
                    strMSG = MSG_AXS_LIM_Y
                Case ERR_AXS_LIM_Z                                      ' Z軸リミット検出
                    strMSG = MSG_AXS_LIM_Z
                Case ERR_AXS_LIM_T                                      ' θ軸リミット検出
                    strMSG = MSG_AXS_LIM_T
                Case ERR_AXS_LIM_ATT                                    ' ATT軸リミット検出
                    strMSG = MSG_AXS_LIM_ATT
                Case ERR_AXS_LIM_Z2                                     ' Z2軸リミット検出
                    strMSG = MSG_AXS_LIM_Z2
                Case ERR_OPN_CVR                                        ' 筐体カバー開検出
                    strMSG = MSG_OPN_CVR
                Case ERR_OPN_SCVR                                       ' スライドカバー開検出
                    strMSG = MSG_OPN_SCVR
                Case ERR_OPN_CVRLTC                                     ' カバー開ラッチ検出
                    strMSG = MSG_OPN_CVRLTC

                Case ERR_AXIS_X_SERVO_ALM
                    strMSG = MSG_AXIS_X_SERVO_ALM                       ' "X軸サーボアラーム"
                Case ERR_AXIS_Y_SERVO_ALM
                    strMSG = MSG_AXIS_Y_SERVO_ALM                       ' "Y軸サーボアラーム"
                Case ERR_AXIS_Z_SERVO_ALM
                    strMSG = MSG_AXIS_Z_SERVO_ALM                       ' "Z軸サーボアラーム"
                Case ERR_AXIS_T_SERVO_ALM
                    strMSG = MSG_AXIS_T_SERVO_ALM                       ' "θ軸サーボアラーム"
                Case ERR_TIMEOUT_AXIS_X
                    strMSG = MSG_TIMEOUT_AXIS_X                         ' "X軸タイムアウトエラー"
                Case ERR_TIMEOUT_AXIS_Y
                    strMSG = MSG_TIMEOUT_AXIS_Y                         ' "Y軸タイムアウトエラー"
                Case ERR_TIMEOUT_AXIS_Z
                    strMSG = MSG_TIMEOUT_AXIS_Z                         ' "Z軸タイムアウトエラー"
                Case ERR_TIMEOUT_AXIS_T
                    strMSG = MSG_TIMEOUT_AXIS_T                         ' "θ軸タイムアウトエラー"
                Case ERR_TIMEOUT_AXIS_Z2
                    strMSG = MSG_TIMEOUT_AXIS_Z2                        ' "Z2軸タイムアウトエラー"
                Case ERR_TIMEOUT_ATT
                    strMSG = MSG_TIMEOUT_ATT                            ' "ロータリアッテネータタイムアウトエラー"
                Case ERR_TIMEOUT_AXIS_XY
                    strMSG = MSG_TIMEOUT_AXIS_XY                        ' "XY軸タイムアウトエラー"

                Case ERR_STG_SOFTLMT_PLUS                               ' プラスリミットオーバー
                    strMSG = MSG_STG_SOFTLMT_PLUS                       ' "軸プラスリミットオーバー"
                Case ERR_STG_SOFTLMT_MINUS                              ' マイナスリミットオーバー
                    strMSG = MSG_STG_SOFTLMT_MINUS                      ' "軸マイナスリミットオーバー"
                Case ERR_STG_SOFTLMT_PLUS_AXIS_X
                    strMSG = MSG_STG_SOFTLMT_PLUS_AXIS_X                ' "X軸プラスリミットオーバー"
                Case ERR_STG_SOFTLMT_PLUS_AXIS_Y
                    strMSG = MSG_STG_SOFTLMT_PLUS_AXIS_Y                ' "Y軸プラスリミットオーバー"
                Case ERR_STG_SOFTLMT_PLUS_AXIS_Z
                    strMSG = MSG_STG_SOFTLMT_PLUS_AXIS_Z                ' "Z軸プラスリミットオーバー"
                Case ERR_STG_SOFTLMT_PLUS_AXIS_T
                    strMSG = MSG_STG_SOFTLMT_PLUS_AXIS_T                ' "θ軸プラスリミットオーバー"
                Case ERR_STG_SOFTLMT_PLUS_AXIS_Z2
                    strMSG = MSG_STG_SOFTLMT_PLUS_AXIS_Z2               ' "Z2軸プラスリミットオーバー"
                Case ERR_STG_SOFTLMT_MINUS_AXIS_X
                    strMSG = MSG_STG_SOFTLMT_MINUS_AXIS_X               ' "X軸マイナスリミットオーバー"
                Case ERR_STG_SOFTLMT_MINUS_AXIS_Y
                    strMSG = MSG_STG_SOFTLMT_MINUS_AXIS_Y               ' "Y軸マイナスリミットオーバー"
                Case ERR_STG_SOFTLMT_MINUS_AXIS_Z
                    strMSG = MSG_STG_SOFTLMT_MINUS_AXIS_Z               ' "ZZ軸マイナスリミットオーバー"
                Case ERR_STG_SOFTLMT_MINUS_AXIS_T
                    strMSG = MSG_STG_SOFTLMT_MINUS_AXIS_T               ' "θ軸マイナスリミットオーバー"
                Case ERR_STG_SOFTLMT_MINUS_AXIS_Z2
                    strMSG = MSG_STG_SOFTLMT_MINUS_AXIS_Z2              ' "Z2軸マイナスリミットオーバー"

                Case ERR_OPN_CVR                                        ' 筐体カバー開検出
                    strMSG = MSG_SPRASH27                               ' "筐体カバーが開きました"
                Case ERR_OPN_SCVR                                       ' スライドカバー開検出
                    strMSG = MSG_SPRASH28                               ' "スライドカバーが開きました"
                Case ERR_OPN_CVRLTC                                     ' カバー開ラッチ検出
                    strMSG = MSG_SPRASH34                               ' "筐体カバーまたはスライドカバーが開きました"
                Case ERR_LSR_STATUS_STANBY
                    strMSG = MSG_INTIME_ERROR + " 833:LASER IS NOT READY"
                Case Else
                    ' INtime側エラー(Code=xxxx)
                    strMSG = MSG_INTIME_ERROR + "(Code=" + gMode.ToString("0") + ")"

            End Select

            '---------------------------------------------------------------------------
            '   メッセージ表示
            '---------------------------------------------------------------------------
            If (ObjSys.IsSoftLimitCode(gMode)) Then                     ' ソフトリミットエラー ?
                Call LAMP_CTRL(LAMP_START, 1)                           ' STARTﾗﾝﾌﾟON
                Call LAMP_CTRL(LAMP_RESET, 0)                           ' RESETﾗﾝﾌﾟOFF

                ' "メッセージ","","STARTキーかOKボタンを押してください"
                Call Sub_SetMessage(strMSG, "", MSG_frmLimit_07, System.Drawing.Color.Blue, System.Drawing.SystemColors.ControlText, System.Drawing.SystemColors.ControlText)
                Me.Refresh()
                BtnCancel.Text = "OK"
                BtnCancel.Visible = True                                ' OKボタン表示
                Md = cFRS_ERR_START                                     ' Md = STARTキー押下待ち
            Else
                Call LAMP_CTRL(LAMP_START, 0)                           ' STARTﾗﾝﾌﾟOFF
                Call LAMP_CTRL(LAMP_RESET, 1)                           ' RESETﾗﾝﾌﾟON

                ' "メッセージ","","RESETキーを押すとプログラムを終了します"
#If START_KEY_SOFT Then
                If gbStartKeySoft Then
                    Call Sub_ResetButtonDispOn()
                End If
#End If
                Call Sub_SetMessage(strMSG, "", MSG_SPRASH16, System.Drawing.Color.Red, System.Drawing.Color.Red, System.Drawing.Color.Red)
                Me.Refresh()
                BtnCancel.Visible = True                                ' Cancelボタン表示
                Md = cFRS_ERR_RST                                       ' Md = RESETキー押下待ち
            End If

            ' ----- V1.18.0.0③↓ -----
            ' 印刷用アラーム開始情報を設定する(ローム殿特注)
            ' アラーム発生回数とアラーム停止開始時間を設定する
            Call SetAlmStartTime()
            ' アラームファイルにアラームデータを書き込む
            Call WriteAlarmData(gFPATH_QR_ALARM, strMSG, stPRT_ROHM.AlarmST_time, gMode)
            ' ----- V1.18.0.0③↑ -----

            ' メッセージ表示してSTART(RESET)キー押下待ち(OK(Cancel)ボタンも有効) ###008
            r = Sub_WaitStartRestKey(Md)
            If (r <> Md) Then
                gMode = r                                               ' Return値設定
            End If

            BtnCancel.Text = "Cancel"
            BtnCancel.Visible = False                                   ' Cancelボタン非表示

            ' ----- V1.18.0.0③↓ -----
            ' アラーム停止情報を設定する(ローム殿特注)
            Call SetAlmEndTime()
            ' ----- V1.18.0.0③↑ -----

            If (ObjSys.IsSoftLimitCode(gMode)) Then                     ' ソフトリミットエラー ?
                Call LAMP_CTRL(LAMP_RESET, True)                        ' RESETﾗﾝﾌﾟ ON
            End If

            Return (r)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FrmReset.Sub_ErrAxis() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Retuen値 = ﾄﾗｯﾌﾟｴﾗｰ発生
        End Try
    End Function
#End Region

#Region "筐体カバー閉/スライドカバー閉チェック処理"
    '''=========================================================================
    ''' <summary>筐体カバー閉/スライドカバー閉チェック処理</summary>
    ''' <param name="iRESETiARBORT">(INP)1=Reset SW有効</param>
    ''' <param name="bOrgFlg">      (INP)True=原点復帰処理中, False=原点復帰処理中以外</param>
    ''' <returns>0 = 正常, -1 = 非常停止, 2 = RESET キー押下, その他</returns>
    '''========================================================================
    Public Function CoverCheck(ByVal iRESETiARBORT As Integer, ByVal bOrgFlg As Boolean) As Long

        Dim sw As Long = 0
        Dim sw1 As Long = 0
        Dim fclamp As Boolean = False
        Dim Flg As Boolean = False
        Dim r As Long = 0
        Dim InterlockSts As Integer = 0
        Dim sldcvrSts As Long = 0
        Dim strMSG As String
        Dim coverSts As Long = 0                                           ' ###101

        Try
            '---------------------------------------------------------------------------
            '   初期処理
            '---------------------------------------------------------------------------
#If cOFFLINEcDEBUG Then                                                 ' OffLineﾃﾞﾊﾞｯｸﾞON ?
            Return (cFRS_NORMAL)                                        ' Return値 = 正常
#End If
            ' インターロック解除ならNOP
            If (bOrgFlg = True) Then                                    ' 原点復帰処理中 ?
                r = ORG_INTERLOCK_CHECK(InterlockSts, sw)               ' インターロック状態取得
            Else
                r = INTERLOCK_CHECK(InterlockSts, sw)                   ' インターロック状態取得
            End If
            'If (InterlockSts = INTERLOCK_STS_DISABLE_FULL) Then        ' インターロック全解除 ?    ###101
            If (InterlockSts <> INTERLOCK_STS_DISABLE_NO) Then          ' インターロック中でない ?  ###101
                Return (cFRS_NORMAL)                                    ' Return値 = 正常
            End If

            '---------------------------------------------------------------------------
            '   筐体カバー閉チェック
            '---------------------------------------------------------------------------
STP_COVEROPEN:
            Flg = False                                                 ' Flg = メッセージ未表示
            '            Do                                             ' ###101
            System.Windows.Forms.Application.DoEvents()

            ' インターロック状態取得
            If (bOrgFlg = True) Then                                    ' 原点復帰処理中 ?
                r = ORG_INTERLOCK_CHECK(InterlockSts, sw)
            Else
                r = INTERLOCK_CHECK(InterlockSts, sw)                   ' インターロック状態取得
            End If
            If (InterlockSts <> INTERLOCK_STS_DISABLE_NO) Then          ' インターロック中でない ?  ###101
                Return (cFRS_NORMAL)                                    ' Return値 = 正常           ###101
            End If

            '----- ###101(変更 ここから) -----
            ' カバー開チェック
            r = COVER_CHECK(coverSts)
            If (r <> cFRS_NORMAL) Then
                Return (r)
            End If

            ' 筐体カバー閉でなければエラー戻りする
            If (coverSts <> 1) Then
                Return (ERR_OPN_CVR)                                    ' Return値 = 筐体カバー開検出
            End If

            '    ' 戻り値の判定
            '    If (r = ERR_OPN_CVRLTC) Then
            '        If (bOrgFlg <> True) Then
            '            Return (r)                                  ' Return値 = カバー開ラッチ
            '        End If
            '        If (Flg = True) Then
            '            Flg = False
            '            '本チェックループにて、一度カバーが開いた後はラッチクリア
            '            Call COVERLATCH_CLEAR()
            '        End If
            '    ElseIf (r = ERR_OPN_CVR) Then
            '        If (Flg = False) Then                           ' メッセージ表示済み ?
            '            Flg = True                                  ' Flg = メッセージ表示済み
            '            LblCaption.ForeColor = System.Drawing.SystemColors.ControlText
            '            Label2.ForeColor = System.Drawing.SystemColors.ControlText
            '            LblCaption.Text = MSG_SPRASH10              ' "筐体カバーを閉じてください"
            '            Label2.Text = ""
            '            Me.Refresh()
            '        End If
            '    ElseIf (r = cFRS_NORMAL) Then
            '        If (sw And BIT_MAIN_COVER_OPENCLOSE) Then           ' カバークローズ
            '            Exit Do
            '        End If
            '    Else                                                ' 以上発生時
            '        Return (r)                                      ' Return値 = INTRIMからのエラーコード
            '    End If
            '    'If (r = cFRS_ERR_CVR) Then                          ' 筐体カバー開検出 ?
            '    '    If (Flg = False) Then                           ' メッセージ表示済み ?
            '    '        Flg = True                                  ' Flg = メッセージ表示済み
            '    '        LblCaption.ForeColor = System.Drawing.SystemColors.ControlText
            '    '        Label2.ForeColor = System.Drawing.SystemColors.ControlText
            '    '        LblCaption.Text = MSG_SPRASH10              ' "筐体カバーを閉じてください"
            '    '        Label2.Text = ""
            '    '        Me.Refresh()
            '    '    End If
            '    'End If
            'Loop While (1)
            '----- ###101(変更 ここまで) -----

            '---------------------------------------------------------------------------
            '   終了処理
            '---------------------------------------------------------------------------
            Return (cFRS_NORMAL)                                    ' Return値 = 正常
            Exit Function

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FrmReset.CoverCheck() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
        CoverCheck = cERR_TRAP                                      ' Retuen値 = ﾄﾗｯﾌﾟｴﾗｰ発生
    End Function
#End Region

#Region "筐体カバー閉を確認する"
    '''=========================================================================
    ''' <summary>筐体カバー閉を確認する ###161</summary>
    ''' <returns>cFRS_NORMAL = 筐体カバー閉
    '''          上記以外  = エラー</returns>
    '''=========================================================================
    Public Function Sub_CoverCheck() As Integer ' ###185

        Dim sw As Long = 0
        Dim coverSts As Long = 0
        Dim InterlockSts As Integer = 0
        Dim r As Integer = cFRS_NORMAL
        Dim strMSG As String

        Try
            ' インターロック解除ならNOP
            r = INTERLOCK_CHECK(InterlockSts, sw)                       ' インターロック状態取得
            If (InterlockSts <> INTERLOCK_STS_DISABLE_NO) Then          ' インターロック中でない ?
                Return (cFRS_NORMAL)                                    ' Return値 = 正常
            End If

            Do
                System.Threading.Thread.Sleep(100)                      ' Wait(ms)
                System.Windows.Forms.Application.DoEvents()
                '----- ###190↓ -----
                r = INTERLOCK_CHECK(InterlockSts, sw)                   ' インターロック状態取得
                If (InterlockSts <> INTERLOCK_STS_DISABLE_NO) Then      ' インターロック中でない ?
                    Return (cFRS_NORMAL)                                ' Return値 = 正常
                End If
                '----- ###190↑ -----

                ' 筐体カバー閉を確認する
                r = COVER_CHECK(coverSts)
                If (r <> cFRS_NORMAL) Then
                    Return (r)                                          ' 非常停止等のエラー
                End If
                ' 筐体カバー閉なら正常戻りする
                If (coverSts = 1) Then Exit Do

                ' "筐体カバーを閉じて","","STARTキーを押すか、OKボタンを押して下さい。"
                r = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_START, True, _
                        MSG_SPRASH36, "", MSG_frmLimit_07, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
                ' 非常停止等のエラーならアプリ強制終了へ(エラーメッセージは表示済み) 
                If (r < cFRS_NORMAL) Then Return (r)
                If (r = cFRS_ERR_START) Then
                    ''V5.0.0.1-27
                    Call ZCONRST()                                              ' コンソールキーラッチ解除
                End If
                Call COVERLATCH_CLEAR()                                 ' カバー開ラッチのクリア

            Loop While (1)

            Return (cFRS_NORMAL)                                        ' Return

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "FrmReset.Sub_CoverCheck() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return値 = 例外エラー 
        End Try
    End Function
#End Region

    ''' <summary>
    '''  'Home位置移動
    ''' </summary>
    ''' <remarks></remarks>
    Public Function SubHomeMove() As Integer
        Dim r As Integer

        'V5.0.0.6②        r = Form1.System1.EX_SMOVE2(gSysPrm, gdblStg2ndOrgX, gdblStg2ndOrgY)
        r = Form1.System1.EX_SMOVE2(gSysPrm, GetLoaderBordTableOutPosX(), GetLoaderBordTableOutPosY())
        If (r < cFRS_NORMAL) Then                           ' エラー ?(※エラーメッセージは表示済み) 
            SubHomeMove = r                                 ' Return値設定
            Exit Function
        End If

    End Function

#End Region

    ''' <summary>
    ''' ３個目のボタンを押したときの処理    'V4.9.0.0①
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnOther_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnOther.Click
        Dim strMSG As String

        Try
            mExitFlag = cFRS_ERR_OTHER

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FrmReset.BtnOther_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub

#Region "ＮＧ率による表示処理"
    '''=========================================================================
    ''' <summary>ＮＧ率による表示処理表示</summary>
    ''' <returns>cFRS_ERR_START  = STARTキー又はOKボタン押下
    '''          cFRS_ERR_RST    = RESETキー又はCancelボタン押下
    '''          cFRS_ERR_OTHER  = OTHERボタン押下
    '''          上記以外      = エラー</returns>
    '''=========================================================================
    Private Function Sub_NgRateAlarm() As Integer

        'V5.0.0.9⑰        Dim Md As Integer = 0
        Dim sts As Long = 0
        Dim r As Long = 0
        Dim strMSG As String

        Try
            '-------------------------------------------------------------------
            '   初期処理
            '-------------------------------------------------------------------
            ' OK/Cancelボタン表示設定
            BtnCancel.Location = LocCanBtn2                             ' Cancelボタン表示位置を右にずらす 
            BtnCancel.Visible = True                                    ' Cancelボタン表示
            BtnOK.Visible = True                                        ' OKボタン表示
            'If (gSysPrm.stTMN.giMsgTyp = 0) Then
            '    BtnOK.Text = "続行"                                         ' ボタン表示「続行」
            '    BtnCancel.Text = "中止"                                     ' ボタン表示「中止」
            '    BtnOther.Text = "クリーニング"                              ' ボタン表示「クリーニング」
            'Else
            '    BtnOK.Text = "CONTINUE"                                         ' ボタン表示「続行」
            '    BtnCancel.Text = "Cancel"                                     ' ボタン表示「中止」
            '    BtnOther.Text = "CLEANING"                              ' ボタン表示「クリーニング」
            'End If

            BtnOK.Text = MSG_SPRASH59                                         ' ボタン表示「続行」
            BtnCancel.Text = MSG_SPRASH60                                     ' ボタン表示「中止」
            BtnOther.Text = MSG_SPRASH61                              ' ボタン表示「クリーニング」

            BtnOther.Visible = True                                     ' Otherボタン表示
            BtnOther.Visible = False
            BtnOK.Left = 50
            BtnOther.Left = BtnOK.Left + BtnOK.Width + 40                            ' ボタン表示「クリーニング」
            BtnCancel.Left = BtnOther.Left + BtnOther.Width + 40

            Call COVERCHK_ONOFF(COVER_CHECK_OFF)                        ' 「固定カバー開チェックなし」にする

            'V5.0.0.9⑭ ↓
            '' マガジン終了時のシグナルタワー制御
            'If (giMachineKd = MACHINE_KD_RS) Then
            '    ' シグナルタワー制御(On=緑点滅+ブザー１, Off=全ﾋﾞｯﾄ) SL436S時
            '    Call Form1.System1.SetSignalTower(SIGOUT_RED_BLK Or SIGOUT_BZ1_ON, &HFFFF)

            'Else
            '    ' シグナルタワー制御(On=赤点滅+ブザー１, Off=全ﾋﾞｯﾄ) SL436R時
            '    Call Form1.System1.SetSignalTower(SIGOUT_RED_BLK Or SIGOUT_BZ1_ON, &HFFFF)
            'End If
            Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALARM)
            'V5.0.0.9⑭ ↑

            Me.Top = 800
            Me.Left = Me.Left
            Me.Refresh()
            ' メッセージ表示
            strMSG = MSG_SPRASH52
            If NGJudgeResult = 1 Then
                strMSG = strMSG + " (Yield)"
            ElseIf NGJudgeResult = 2 Then
                strMSG = strMSG + " (OpenNG)"
            ElseIf NGJudgeResult = 4 Then
                strMSG = strMSG + " (Sub-IT-HI)"
            ElseIf NGJudgeResult = 5 Then
                strMSG = strMSG + " (Lot-IT-HI)"
            ElseIf NGJudgeResult = 7 Then
                strMSG = strMSG + " (Sub-IT-LO)"
            ElseIf NGJudgeResult = 8 Then
                strMSG = strMSG + " (Lot-IT-LO)"
            ElseIf NGJudgeResult = 10 Then
                strMSG = strMSG + " (Sub-FT-HI)"
            ElseIf NGJudgeResult = 11 Then
                strMSG = strMSG + " (LOT-FT-HI)"
            ElseIf NGJudgeResult = 13 Then
                strMSG = strMSG + " (Sub-FT-LO)"
            ElseIf NGJudgeResult = 14 Then
                strMSG = strMSG + " (LOT-FT-LO)"
            End If
            ' "ＮＧ率による停止", "OKボタン押下で自動運転を続行します", "Cancelボタン押下で自動運転を終了します"
            Call Sub_SetMessage(strMSG, MSG_LOADER_45, MSG_LOADER_46, System.Drawing.Color.Red, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
            Me.Refresh()
            Call ZCONRST()                                              ' コンソールキーラッチ解除
            mExitFlag = 0
            'V5.0.0.9⑰            Md = 0

            '-------------------------------------------------------------------
            '   ボタン待ち処理
            '-------------------------------------------------------------------
            Dim md As Short = Globals_Renamed.giAppMode                 'V5.0.0.9⑰
            Globals_Renamed.giAppMode = APP_MODE_IDLE                   'V5.0.0.9⑰
            Do

                ' START/RESETキー押下待ち
                r = STARTRESET_SWCHECK(False, sts)                      ' START/RESET SW押下チェック
                If (r = cFRS_ERR_EMG) Then
                    mExitFlag = cFRS_ERR_EMG
                    GoTo STP_EXIT
                End If

                ' RESETキー押下 ?
                If (sts = cFRS_ERR_RST) Then                            ' RESETキー又はCancelボタン押下 ? 
                    mExitFlag = cFRS_ERR_RST                            ' Retuen値 = RESETキー又はCancelボタン押下
                    Exit Do                                             ' 筐体カバー閉確認へ
                End If

                ' STARTキー押下 ?
                If ((BtnOK.Enabled = True) And (sts = cFRS_ERR_START)) Then ' STARTキー又はOKボタン押下 ? V1.18.0.7②
                    mExitFlag = cFRS_ERR_START                          ' Retuen値 = STARTキー又はOKボタン押下
                    Exit Do                                             ' 筐体カバー閉確認へ
                End If

                System.Windows.Forms.Application.DoEvents()             ' メッセージポンプ
                Call System.Threading.Thread.Sleep(100)                 ' Wait(msec)
                If ObjSys.EmergencySwCheck() Then                       ' 非常停止 ?
                    mExitFlag = cFRS_ERR_EMG                            ' Retuen値 = 非常停止
                    GoTo STP_EXIT                                       ' 非常停止メッセージ表示へ
                End If
                Me.BringToFront()                                       ' 最前面に表示 

            Loop While (mExitFlag = 0)
            Globals_Renamed.giAppMode = md                              'V5.0.0.9⑰

            ' シグナルタワー制御(On=なし, Off=赤点滅+ブザー１)
            'V5.0.0.9⑭ ↓
            ' Call Form1.System1.SetSignalTower(0, SIGOUT_RED_BLK Or SIGOUT_BZ1_ON)     ' V4.0.0.0-25
            Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALL_OFF)
            'V5.0.0.9⑭ ↑

            '-------------------------------------------------------------------
            '   終了処理
            '-------------------------------------------------------------------
            ' 筐体カバー閉を確認する
            Call COVERLATCH_CLEAR()                                     ' カバー開ラッチのクリア
            Call COVERCHK_ONOFF(COVER_CHECK_ON)                         ' 「固定カバー開チェックあり」にする
            Me.Size = stSzNML                                           ' Formの幅/高さを標準モード用にする
            Me.Visible = False                                          ' メッセージ表示を消す 
            r = Sub_CoverCheck()                                        ' 筐体カバー閉を確認する
            If (r = cFRS_ERR_EMG) Then                                  ' 非常停止 ?
                mExitFlag = cFRS_ERR_EMG                                ' Retuen値 = 非常停止
                GoTo STP_EXIT                                           ' 非常停止メッセージ表示へ
            End If

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FrmReset.Sub_NgRateAlarm() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            mExitFlag = cERR_TRAP                                       ' Retuen値 = ﾄﾗｯﾌﾟｴﾗｰ発生
        End Try

STP_EXIT:
        BtnCancel.Visible = False                                       ' Cancelボタン非表示
        BtnOK.Visible = False                                           ' OKボタン非表示
        BtnCancel.Text = "Cancel"
        Return (mExitFlag)
    End Function
#End Region

#Region "集計内容をクリアするかの確認メッセージ"
    '''=========================================================================
    ''' <summary>集計内容をクリアするかの確認メッセージ</summary>
    ''' <returns>cFRS_ERR_START  = STARTキー又はOKボタン押下
    '''          cFRS_ERR_RST    = RESETキー又はCancelボタン押下
    '''          上記以外      = エラー</returns>
    '''=========================================================================
    Private Function Sub_QuestTotalClear() As Integer

        Dim Md As Integer = 0
        Dim sts As Long = 0
        Dim r As Long = 0
        Dim strMSG As String

        Try
            '-------------------------------------------------------------------
            '   初期処理
            '-------------------------------------------------------------------
            ' OK/Cancelボタン表示設定
            BtnCancel.Location = LocCanBtn2                             ' Cancelボタン表示位置を右にずらす 
            BtnCancel.Visible = False                                   ' Cancelボタン表示
            BtnOK.Visible = True                                        ' OKボタン表示
            BtnOK.Text = MSG_SPRASH54                                   ' ボタン表示「クリア」

            BtnOther.Size = New Point(BtnOK.Size.Width, BtnOK.Size.Height)

            BtnOther.Visible = True                                     ' Otherボタン表示
            BtnOther.Text = MSG_SPRASH55                                ' ボタン表示「クリアしない」
            BtnOK.Left = 100
            BtnOther.Left = BtnOK.Left + BtnOK.Width + 100


            Call COVERCHK_ONOFF(COVER_CHECK_OFF)                        ' 「固定カバー開チェックなし」にする

            ' マガジン終了時のシグナルタワー制御
            'V5.0.0.9⑭ ↓
            'If (giMachineKd = MACHINE_KD_RS) Then
            '    ' シグナルタワー制御(On=緑点滅+ブザー１, Off=全ﾋﾞｯﾄ) SL436S時
            '    Call Form1.System1.SetSignalTower(SIGOUT_RED_BLK Or SIGOUT_BZ1_ON, &HFFFF)
            'Else
            '    ' シグナルタワー制御(On=赤点滅+ブザー１, Off=全ﾋﾞｯﾄ) SL436R時
            '    Call Form1.System1.SetSignalTower(SIGOUT_RED_BLK Or SIGOUT_BZ1_ON, &HFFFF)
            'End If
            Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALARM)
            'V5.0.0.9⑭ ↑


            ' メッセージ表示
            ' "集計をクリアしますか？", " ", " "
            Call Sub_SetMessage("", MSG_SPRASH56, MSG_SPRASH57, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
            Me.Refresh()
            Call ZCONRST()                                              ' コンソールキーラッチ解除
            mExitFlag = 0
            Md = 0

            '-------------------------------------------------------------------
            '   ボタン待ち処理
            '-------------------------------------------------------------------

            Do

                ' START/RESETキー押下待ち
                r = STARTRESET_SWCHECK(False, sts)                      ' START/RESET SW押下チェック
                If (r = cFRS_ERR_EMG) Then
                    mExitFlag = cFRS_ERR_EMG
                    GoTo STP_EXIT
                End If

                ' RESETキー押下 ?
                If (sts = cFRS_ERR_RST) Then                            ' RESETキー又はCancelボタン押下 ? 
                    mExitFlag = cFRS_ERR_OTHER                            ' Retuen値 = RESETキー又はCancelボタン押下
                    Exit Do                                             ' 筐体カバー閉確認へ
                End If

                ' STARTキー押下 ?
                If ((BtnOK.Enabled = True) And (sts = cFRS_ERR_START)) Then ' STARTキー又はOKボタン押下 ? V1.18.0.7②
                    mExitFlag = cFRS_ERR_START                          ' Retuen値 = STARTキー又はOKボタン押下
                    Exit Do                                             ' 筐体カバー閉確認へ
                End If

                System.Windows.Forms.Application.DoEvents()             ' メッセージポンプ
                Call System.Threading.Thread.Sleep(100)                 ' Wait(msec)
                If ObjSys.EmergencySwCheck() Then                       ' 非常停止 ?
                    mExitFlag = cFRS_ERR_EMG                            ' Retuen値 = 非常停止
                    GoTo STP_EXIT                                       ' 非常停止メッセージ表示へ
                End If
                Me.BringToFront()                                       ' 最前面に表示 

            Loop While (mExitFlag = 0)

            ' シグナルタワー制御(On=なし, Off=赤点滅+ブザー１)
            'V5.0.0.9⑭ ↓
            ' Call Form1.System1.SetSignalTower(0, SIGOUT_RED_BLK Or SIGOUT_BZ1_ON)     ' V4.0.0.0-25
            Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALARM)
            'V5.0.0.9⑭ ↑

            '-------------------------------------------------------------------
            '   終了処理
            '-------------------------------------------------------------------
            ' 筐体カバー閉を確認する
            Call COVERLATCH_CLEAR()                                     ' カバー開ラッチのクリア
            Call COVERCHK_ONOFF(COVER_CHECK_ON)                         ' 「固定カバー開チェックあり」にする
            Me.Size = stSzNML                                           ' Formの幅/高さを標準モード用にする
            Me.Visible = False                                          ' メッセージ表示を消す 
            r = Sub_CoverCheck()                                        ' 筐体カバー閉を確認する
            If (r = cFRS_ERR_EMG) Then                                  ' 非常停止 ?
                mExitFlag = cFRS_ERR_EMG                                ' Retuen値 = 非常停止
                GoTo STP_EXIT                                           ' 非常停止メッセージ表示へ
            End If

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FrmReset.Sub_NgRateAlarm() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            mExitFlag = cERR_TRAP                                       ' Retuen値 = ﾄﾗｯﾌﾟｴﾗｰ発生
        End Try

STP_EXIT:
        BtnCancel.Visible = False                                       ' Cancelボタン非表示
        BtnOK.Visible = False                                           ' OKボタン非表示
        BtnCancel.Text = "Cancel"
        Return (mExitFlag)
    End Function
#End Region
    '----- V6.0.3.0⑳↓ -----
#Region "カットオフ調整でエラー発生時処理"
    '''=========================================================================
    ''' <summary>カットオフ調整でエラー発生時処理</summary>
    ''' <param name="Mode">(INP)処理モード</param>
    ''' <returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function Sub_CutOffTurnError(ByVal Mode As Integer) As Integer

        Dim r As Integer
        Dim ErrCode As Short                                            ' V1.18.0.0③
        Dim strMSG As String
        Dim strMSG2 As String

        Try
            strMSG = "ALARM"
            strMSG2 = "CutOff"
            Call ObjSys.OperationLogging(gSysPrm, strMSG, strMSG2)

            ' シグナルタワー制御(On=異常+ﾌﾞｻﾞｰ1, Off=全ﾋﾞｯﾄ) 
            Select Case (gSysPrm.stIOC.giSignalTower)
                Case SIGTOWR_NORMAL                                     ' 標準(赤点滅+ブザー１)
                    Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALARM)

                Case SIGTOWR_SPCIAL                                     ' 特注(赤点滅+ブザー１)

            End Select

            ErrCode = cFRS_ERR_REPROB                                   '
            strMSG = MSG_SPRASH70                                       ' "カットオフ調整に失敗しました。" 'V6.0.3.0⑳
            strMSG2 = MSG_SPRASH71                                      ' "START:再試行、RESET:ロット中断" 'V6.0.3.0⑳

            Call Sub_SetMessage(strMSG, strMSG2, "", System.Drawing.Color.Red, System.Drawing.SystemColors.ControlText, System.Drawing.SystemColors.ControlText)
            Me.Refresh()
            'BtnCancel.Text = "START"
            'BtnCancel.Visible = True                                    ' OKボタン表示

            ' ----- V1.18.0.0③↓ -----
            ' 印刷用アラーム開始情報を設定する(ローム殿特注)
            ' アラーム発生回数とアラーム停止開始時間を設定する
            Call SetAlmStartTime()
            ' アラームファイルにアラームデータを書き込む
            Call WriteAlarmData(gFPATH_QR_ALARM, strMSG, stPRT_ROHM.AlarmST_time, ErrCode)
            ' ----- V1.18.0.0③↑ -----

            ' メッセージ表示してSTARTキー押下待ち(OKボタンも有効)
            r = Sub_WaitStartRestKey(cFRS_ERR_START + cFRS_ERR_RST)     ' STARTｷｰ押下待ち
            ' ----- V1.18.0.0③↓ -----
            ' アラーム停止情報を設定する(ローム殿特注)
            Call SetAlmEndTime()
            ' ----- V1.18.0.0③↑ -----
            If (r = cFRS_ERR_EMG) Then                                  ' 非常停止検出 ?
                GoTo STP_END                                            ' 非常停止メッセージ表示へ
            End If

            '----- ###181↓ -----
            ' シグナルタワー制御(On=なし, Off=異常+ﾌﾞｻﾞｰ1) 
            Select Case (gSysPrm.stIOC.giSignalTower)
                Case SIGTOWR_NORMAL                                     ' 標準(赤点滅+ブザー１)
                    Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALL_OFF)

                Case SIGTOWR_SPCIAL                                     ' 特注(赤点滅+ブザー１)

            End Select

STP_END:
            BtnCancel.Text = "Cancel"
            BtnCancel.Visible = False                                   ' Cancelボタン非表示
            Return (r)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FrmReset.Sub_CutOffTurnError() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Retuen値 = ﾄﾗｯﾌﾟｴﾗｰ発生
        End Try
    End Function
#End Region
    '----- V6.0.3.0⑳↑ -----

#If START_KEY_SOFT Then

#Region "ボタン表示、非表示処理"

    Private Sub Sub_StartResetButtonDispOn()
        Try
            Sub_StartResetButtonDispOff()
            BtnCancel.Location = LocCanBtn2                     ' Cancelボタン表示位置を右にずらす 
            BtnCancel.Enabled = True                            ' Cancelボタン表示
            BtnCancel.Visible = True                            ' Cancelボタン表示
            BtnOK.Enabled = True                                ' OKボタン表示
            BtnOK.Visible = True                                ' OKボタン表示
            BtnOther.Enabled = False                            ' Otherボタン非表示
            BtnOther.Visible = False                            ' Otherボタン非表示
            BtnOK.Text = "START"                                ' OKボタン表示
            BtnCancel.Text = "RESET"                            ' Cancelボタン表示
            BtnOK.Size = New System.Drawing.Size(130, 40)
            BtnCancel.Size = New System.Drawing.Size(130, 40)
        Catch ex As Exception
            MsgBox("FrmReset.Sub_StartResetButtonDispOn() TRAP ERROR = " + ex.Message)
        End Try
    End Sub

    Private Sub Sub_StartButtonDispOn()
        Try
            Sub_StartResetButtonDispOff()
            BtnOK.Enabled = True                                ' OKボタン表示
            BtnOK.Visible = True                                ' OKボタン表示
            BtnOK.Text = "START"                                ' OKボタン表示
            BtnOK.Size = New System.Drawing.Size(130, 40)
        Catch ex As Exception
            MsgBox("FrmReset.Sub_StartButtonDispOn() TRAP ERROR = " + ex.Message)
        End Try
    End Sub

    Private Sub Sub_ResetButtonDispOn()
        Try
            Sub_StartResetButtonDispOff()
            BtnCancel.Location = LocCanBtnOrg                   ' Cancelボタン表示位置を右にずらす 
            BtnCancel.Enabled = True                            ' Cancelボタン表示
            BtnCancel.Visible = True                            ' Cancelボタン表示
            BtnCancel.Text = "RESET"                            ' Cancelボタン表示
            BtnCancel.Size = New System.Drawing.Size(130, 40)
        Catch ex As Exception
            MsgBox("FrmReset.Sub_ResetButtonDispOn() TRAP ERROR = " + ex.Message)
        End Try
    End Sub

    Private Sub Sub_StartResetButtonDispOff()
        Try
            BtnCancel.Enabled = False                           ' Cancelボタン表示
            BtnCancel.Visible = False                           ' Cancelボタン表示
            BtnOK.Enabled = False                               ' OKボタン表示
            BtnOK.Visible = False                               ' OKボタン表示
            'BtnOther.Enabled = False                            ' Otherボタン非表示
            BtnOther.Visible = False                            ' Otherボタン非表示
        Catch ex As Exception
            MsgBox("FrmReset.Sub_StartResetButtonOff() TRAP ERROR = " + ex.Message)
        End Try
    End Sub
#End Region

#End If

    '#Region "ＮＧ数による表示処理"
    '    '''=========================================================================
    '    ''' <summary>ＮＧ数による表示処理表示</summary>
    '    ''' <returns>cFRS_ERR_START  = STARTキー又はOKボタン押下
    '    '''          cFRS_ERR_RST    = RESETキー又はCancelボタン押下
    '    '''          cFRS_ERR_OTHER  = OTHERボタン押下
    '    '''          上記以外      = エラー</returns>
    '    '''=========================================================================
    '    Private Function Sub_NgCountAlarm() As Integer

    '        Dim sts As Long = 0
    '        Dim r As Long = 0
    '        Dim strMSG As String

    '        Try
    '            '-------------------------------------------------------------------
    '            '   初期処理
    '            '-------------------------------------------------------------------
    '            ' OK/Cancelボタン表示設定
    '            BtnCancel.Location = LocCanBtn2                             ' Cancelボタン表示位置を右にずらす 
    '            BtnCancel.Visible = True                                    ' Cancelボタン表示
    '            BtnOK.Visible = True                                        ' OKボタン表示

    '            'BtnOK.Text = MSG_SPRASH59                                         ' ボタン表示「続行」
    '            'BtnCancel.Text = MSG_SPRASH60                                     ' ボタン表示「中止」
    '            'BtnOther.Text = MSG_SPRASH61                              ' ボタン表示「クリーニング」

    '            'BtnOther.Visible = True                                     ' Otherボタン表示
    '            'BtnOther.Visible = False
    '            BtnOK.Left = 50
    '            BtnOther.Left = BtnOK.Left + BtnOK.Width + 40                            ' ボタン表示「クリーニング」
    '            BtnCancel.Left = BtnOther.Left + BtnOther.Width + 40

    '            Call COVERCHK_ONOFF(COVER_CHECK_OFF)                        ' 「固定カバー開チェックなし」にする

    '            'V5.0.0.9⑭ ↓
    '            '' マガジン終了時のシグナルタワー制御
    '            Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALARM)
    '            'V5.0.0.9⑭ ↑

    '            Me.Top = 800
    '            Me.Left = Me.Left
    '            Me.Refresh()

    '            ' メッセージ表示
    '            ' "ＮＧ率による停止", "OKボタン押下で自動運転を続行します", "Cancelボタン押下で自動運転を終了します"
    '            Call Sub_SetMessage(strMSG, MSG_LOADER_45, MSG_LOADER_46, System.Drawing.Color.Red, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
    '            Me.Refresh()
    '            Call ZCONRST()                                              ' コンソールキーラッチ解除
    '            mExitFlag = 0
    '            'V5.0.0.9⑰            Md = 0

    '            '-------------------------------------------------------------------
    '            '   ボタン待ち処理
    '            '-------------------------------------------------------------------
    '            Dim md As Short = Globals_Renamed.giAppMode                 'V5.0.0.9⑰
    '            Globals_Renamed.giAppMode = APP_MODE_IDLE                   'V5.0.0.9⑰
    '            Do

    '                ' START/RESETキー押下待ち
    '                r = STARTRESET_SWCHECK(False, sts)                      ' START/RESET SW押下チェック
    '                If (r = cFRS_ERR_EMG) Then
    '                    mExitFlag = cFRS_ERR_EMG
    '                    GoTo STP_EXIT
    '                End If

    '                ' RESETキー押下 ?
    '                If (sts = cFRS_ERR_RST) Then                            ' RESETキー又はCancelボタン押下 ? 
    '                    mExitFlag = cFRS_ERR_RST                            ' Retuen値 = RESETキー又はCancelボタン押下
    '                    Exit Do                                             ' 筐体カバー閉確認へ
    '                End If

    '                ' STARTキー押下 ?
    '                If ((BtnOK.Enabled = True) And (sts = cFRS_ERR_START)) Then ' STARTキー又はOKボタン押下 ? V1.18.0.7②
    '                    mExitFlag = cFRS_ERR_START                          ' Retuen値 = STARTキー又はOKボタン押下
    '                    Exit Do                                             ' 筐体カバー閉確認へ
    '                End If

    '                System.Windows.Forms.Application.DoEvents()             ' メッセージポンプ
    '                Call System.Threading.Thread.Sleep(100)                 ' Wait(msec)
    '                If ObjSys.EmergencySwCheck() Then                       ' 非常停止 ?
    '                    mExitFlag = cFRS_ERR_EMG                            ' Retuen値 = 非常停止
    '                    GoTo STP_EXIT                                       ' 非常停止メッセージ表示へ
    '                End If
    '                Me.BringToFront()                                       ' 最前面に表示 

    '            Loop While (mExitFlag = 0)
    '            Globals_Renamed.giAppMode = md                              'V5.0.0.9⑰

    '            ' シグナルタワー制御(On=なし, Off=赤点滅+ブザー１)
    '            'V5.0.0.9⑭ ↓
    '            Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALL_OFF)
    '            'V5.0.0.9⑭ ↑

    '            '-------------------------------------------------------------------
    '            '   終了処理
    '            '-------------------------------------------------------------------
    '            ' 筐体カバー閉を確認する
    '            Call COVERLATCH_CLEAR()                                     ' カバー開ラッチのクリア
    '            Call COVERCHK_ONOFF(COVER_CHECK_ON)                         ' 「固定カバー開チェックあり」にする
    '            Me.Size = stSzNML                                           ' Formの幅/高さを標準モード用にする
    '            Me.Visible = False                                          ' メッセージ表示を消す 
    '            r = Sub_CoverCheck()                                        ' 筐体カバー閉を確認する
    '            If (r = cFRS_ERR_EMG) Then                                  ' 非常停止 ?
    '                mExitFlag = cFRS_ERR_EMG                                ' Retuen値 = 非常停止
    '                GoTo STP_EXIT                                           ' 非常停止メッセージ表示へ
    '            End If

    '            ' トラップエラー発生時 
    '        Catch ex As Exception
    '            strMSG = "FrmReset.Sub_NgRateAlarm() TRAP ERROR = " + ex.Message
    '            MsgBox(strMSG)
    '            mExitFlag = cERR_TRAP                                       ' Retuen値 = ﾄﾗｯﾌﾟｴﾗｰ発生
    '        End Try

    'STP_EXIT:
    '        BtnCancel.Visible = False                                       ' Cancelボタン非表示
    '        BtnOK.Visible = False                                           ' OKボタン非表示
    '        BtnCancel.Text = "Cancel"
    '        Return (mExitFlag)
    '    End Function
    '#End Region

End Class

'=============================== END OF FILE ===============================