'===============================================================================
'   Description  : ローダアラーム画面処理
'
'   Copyright(C) : OMRON LASERFRONT INC. 2011
'
'===============================================================================

Imports LaserFront.Trimmer.DefTrimFnc
Imports LaserFront.Trimmer.DllSystem        'V6.0.0.0⑱
Imports TKY_ALL_SL432HW.My.Resources        'V4.4.0.0-0

Public Class FormLoaderAlarm

#Region "【変数定義】"
    '===========================================================================
    '   変数定義
    '===========================================================================
    Private mExitFlag As Integer                                        ' 終了フラグ
    Private mMode As Short                                              ' Start/Resetキー ###074
    Private mAlarmKind As Integer                                       ' アラーム種類 ###144
    'V6.0.0.0⑱    Private ObjSys As Object                                            ' OcxSystemオブジェクト

    Private mAlarmLevel As Integer                                      ' アラームレベル保存用        ''V5.0.0.7①

    '----- ローダアラーム情報 -----
    'Private AlarmKind As Integer                                        ' アラーム種類(全停止異常, サイクル停止, 軽故障, アラーム無し)
    'Private AlarmCount As Integer                                       ' 発生アラーム数 
    'Private strLoaderAlarm(LALARM_COUNT) As String                      ' アラーム文字列
    'Private strLoaderAlarmInfo(LALARM_COUNT) As String                  ' アラーム情報1
    Private strLoaderAlarmExec(LALARM_COUNT) As String                  ' アラーム情報(対策)

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
    ''' <param name="Owner">             (INP)未使用</param>
    ''' <param name="ObjSystem">         (INP)OcxSystemオブジェクト</param>
    ''' <param name="AlarmKind">         (INP)アラーム種類(全停止異常, サイクル停止, 軽故障, アラーム無し)</param>
    ''' <param name="AlarmCount">        (INP)発生アラーム数</param>
    ''' <param name="strLoaderAlarm">    (INP)アラーム文字列</param>
    ''' <param name="strLoaderAlarmInfo">(INP)アラーム情報1(※未使用)</param>
    ''' <param name="pstrLoaderAlarmExec">(INP)アラーム情報(対策)</param>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Overloads Sub ShowDialog(ByVal Owner As IWin32Window, ByVal ObjSystem As SystemNET, ByVal AlarmKind As Integer, ByVal AlarmCount As Integer, _
                                    ByRef strLoaderAlarm() As String, ByRef strLoaderAlarmInfo() As String, ByRef pstrLoaderAlarmExec() As String)
        'Public Overloads Sub ShowDialog(ByVal Owner As IWin32Window, ByVal ObjSystem As Object, ByVal AlarmKind As Integer, ByVal AlarmCount As Integer, _
        '                                ByRef strLoaderAlarm() As String, ByRef strLoaderAlarmInfo() As String, ByRef pstrLoaderAlarmExec() As String)

        Dim strMSG As String

        Try
            ' 初期処理
            mExitFlag = -1                                              ' 終了フラグ = 初期化
            'V6.0.0.0⑱            ObjSys = ObjSystem                                          ' OcxSystemオブジェクト
            strLoaderAlarmExec = pstrLoaderAlarmExec                    ' アラーム情報(対策)を退避する(クリックイベント処理で使用するため) 

            mAlarmKind = AlarmKind                                      ' ###144
            '----- ###196↓ -----
            ' 「軽故障発生中」または「サイクル停止」ならCancelボタンを表示する
            'If (AlarmKind = cFRS_ERR_LDR3) Then                         ' 軽故障発生中ならCancelボタンを表示する ###073
            If (AlarmKind = cFRS_ERR_LDR3) Or (AlarmKind = cFRS_ERR_LDR2) Then
                BtnCancel.Visible = True                                ' Cancelボタン表示
                mMode = cFRS_ERR_START + cFRS_ERR_RST                   ' START/RESETキー押下待ち ###074
            Else
                BtnCancel.Visible = False                               ' Cancelボタン非表示
                mMode = cFRS_ERR_START                                  ' STARTキー押下待ち ###074
            End If
            '----- ###196↑ -----
            ' ローダアラーム情報を設定する
            Call SetAlarmList(AlarmKind, AlarmCount, strLoaderAlarm, strLoaderAlarmInfo, strLoaderAlarmExec)

            ' ----- V1.18.0.0③↓ -----
            ' 印刷用アラーム開始情報を設定する(ローム殿特注)
            If (AlarmKind <> cFRS_NORMAL) Then
                ' アラーム発生回数とアラーム停止開始時間を設定する
                Call SetAlmStartTime()
                ' アラームファイルにアラームデータを書き込む
                Call WriteAlarmData(gFPATH_QR_ALARM, strLoaderAlarm(0), stPRT_ROHM.AlarmST_time, AlarmKind)
            End If
            ' ----- V1.18.0.0③↑ -----

            ' 画面表示
            Me.ShowDialog()                                             ' 画面表示
            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormLoaderAlarm.ShowDialog() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "Form_Load時処理"
    '''=========================================================================
    ''' <summary>Form_Load時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub FormLoaderAlarm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim strMSG As String

        Try
            ' ラベル名設定
            'If (gSysPrm.stTMN.giMsgTyp = 0) Then
            '    LblAlarmList.Text = "アラームリスト"
            '    LblExec.Text = "対策"
            'Else
            '    LblAlarmList.Text = "Alarm List"
            '    LblExec.Text = "Measures"
            'End If
            '----- ###158↓ -----
            '"装置内に基板が残っている場合は", "取り除いてください"
            LblMSG.Text = MSG_LOADER_29 & MSG_LOADER_18
            '----- ###158↑ -----

            '----- ###144↓ -----
            ' ボタン名設定
            'BtnTableClampOff.Text = MSG_LOADER_24                           '"載物台クランプ解除" ボタン
            'BtnTableVacumeOff.Text = MSG_LOADER_25                          '"載物台吸着解除" ボタン
            'BtnHandVacumeOff.Text = MSG_LOADER_26                           '"ハンド吸着解除" ボタン
            'BtnHandVacumeOff.Text = MSG_LOADER_27                           '"供給ハンド吸着解除" ボタン ###158
            'BtnHand2VacumeOff.Text = MSG_LOADER_28                          '"収納ハンド吸着解除" ボタン ###158

            ' ボタン表示/非表示設定
            '----- ###196↓ -----
            'If (mAlarmKind = cFRS_ERR_LDR3) Then                            ' 軽故障発生中なら
            If (mAlarmKind = cFRS_ERR_LDR3) Or (mAlarmKind = cFRS_ERR_LDR2) Then
                BtnTableVacumeOff.Visible = False                           ' 載物台吸着解除ボタン非表示
                BtnTableClampOff.Visible = False                            ' 載物台クランプ解除ボタン非表示
                BtnHandVacumeOff.Visible = False                            ' 供給ハンド吸着解除非表示
                BtnHand2VacumeOff.Visible = False                           ' 収納ハンド吸着解除非表示 ###158
                LblMSG.Visible = False                                      ' ###158
            Else
                BtnTableVacumeOff.Visible = True                            ' 載物台吸着解除ボタン表示
                BtnTableClampOff.Visible = True                             ' 載物台クランプ解除ボタン表示
                BtnHandVacumeOff.Visible = True                             ' 供給ハンド吸着解除ボタン表示
                BtnHand2VacumeOff.Visible = True                            ' 収納ハンド吸着解除ボタン表示 ###158
                LblMSG.Visible = True                                       ' ###158
            End If
            '----- ###144↑ -----
            '----- ###196↑ -----

            '----- V1.18.0.6②↓ -----
            ' フォームの表示位置を設定する ###158 ###074
            'Me.Location = Form1.chkDistributeOnOff.Location
            'Me.Location = Form1.GrpNgBox.Location
            Dim Pos As System.Drawing.Point
            Pos.Y = Form1.Size.Height - Me.Size.Height
            Pos.X = 0
            Me.Location = Pos
            '----- V1.18.0.6②↑ -----

            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormLoaderAlarm.FormLoaderAlarm_Load() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "Form_Shown時処理"
    '''=========================================================================
    ''' <summary>Form_Shown時処理 ###074</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub FormLoaderAlarm_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
        Dim strMSG As String

        Try
            ' Start/Resetキー押下待ち
            mExitFlag = Sub_WaitStartRestKey(mMode)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormLoaderAlarm.FormLoaderAlarm_Shown() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
        Me.Close()                                                      ' フォームを閉じる
    End Sub
#End Region

#Region "Start/Resetキー押下待ちｻﾌﾞﾙｰﾁﾝ"
    '''=========================================================================
    ''' <summary>Start/Resetキー押下待ちｻﾌﾞﾙｰﾁﾝ ###074</summary>
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
        Dim OutBit As Integer = 0                                   ' ###208
        Dim lData As Long                                           'V5.0.0.7①
        Dim AlarmCount As Integer = 0                               'V5.0.0.7①
        Dim strLoaderAlarm(128) As String                           'V5.0.0.7①
        Dim strLoaderAlarmInfo(128) As String                       'V5.0.0.7①
        Dim pstrLoaderAlarmExec(128) As String                      'V5.0.0.7①
        Dim iData(2) As UShort                                      'V5.0.0.7①

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
            mExitFlag = -1
            '----- ###208 -----
            Call ZATLDRED(OutBit)
            Call SetLoaderIO(&H0, LOUT_SUPLY)
            '----- ###208 -----

            Do
                r = STARTRESET_SWCHECK(False, sts)                  ' START/RESET SW押下チェック
                If (sts = cFRS_ERR_RST) And ((Md = cFRS_ERR_RST) Or (Md = cFRS_ERR_START + cFRS_ERR_RST)) Then
                    mExitFlag = cFRS_ERR_RST                        ' ExitFlag = Cancel(RESETキー)
                ElseIf (sts = cFRS_ERR_START) And ((Md = cFRS_ERR_START) Or (Md = cFRS_ERR_START + cFRS_ERR_RST)) Then
                    mExitFlag = cFRS_ERR_START                      ' ExitFlag = OK(STARTキー)
                End If

                System.Windows.Forms.Application.DoEvents()         ' メッセージポンプ
                Call System.Threading.Thread.Sleep(250)             ' Wait(msec)
                If Form1.System1.EmergencySwCheck() Then            ' 非常停止 ?
                    mExitFlag = cFRS_ERR_EMG                        ' Return値 = 非常停止検出
                End If
                ''V5.0.0.7①↓
                ' 軽故障発生中に重故障が発生した場合には、重故障を優先として表示する 
                If mMode = (cFRS_ERR_START + cFRS_ERR_RST) Then       '軽故障の場合：START,RESET待ち

                    ' ローダ部アラーム状態をチェックする
                    r = W_Read(LOFS_W110, lData)                            ' ローダアラーム状態取得(W110.08-10)v
                    iData(0) = lData
                    If (lData And LARM_ARM3) Then                        ' 全停止異常発生中
                        BtnCancel.Visible = False                               ' Cancelボタン非表示
                        mMode = cFRS_ERR_START                                  ' STARTキー押下待ち ###074
                        If (lData <> cFRS_NORMAL) Then                          ' アラーム発生 ?
                            r = W_Read(LOFS_W115, lData)                        ' ローダアラーム詳細取得(W115.00-W115.15(続行不可))
                            iData(0) = lData
                            r = W_Read(LOFS_W116, lData)                        ' ローダアラーム詳細取得(W116.00-W116.15(続行可))
                            iData(1) = lData
                            SetAlarmLevel(cFRS_ERR_LDR1)
                            ' ローダアラームメッセージを作成する(AlmCount = 発生アラーム数)
                            AlarmCount = Loader_MakeAlarmStrings(iData, strLoaderAlarm, strLoaderAlarmInfo, strLoaderAlarmExec)

                            ' ローダアラーム情報を設定する
                            Call SetAlarmList(cFRS_ERR_LDR1, AlarmCount, strLoaderAlarm, strLoaderAlarmInfo, strLoaderAlarmExec)
                        End If
                    End If

                End If
                ''V5.0.0.7①↑

                Me.BringToFront()                                   ' 最前面に表示 
            Loop While (mExitFlag = -1)

            '----- ###208 -----
            If (OutBit And LOUT_SUPLY) Then
                Call SetLoaderIO(LOUT_SUPLY, &H0)
            End If
            '----- ###208 -----

            Return (mExitFlag)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FrmReset.Sub_WaitRestKey() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                      ' Retuen値 = ﾄﾗｯﾌﾟｴﾗｰ発生
        End Try
    End Function
#End Region

#Region "OKボタン押下時"
    '''=========================================================================
    ''' <summary>OKボタン押下時</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnOK.Click

        Dim strMSG As String

        Try
            ' アラームリセット送出 
            Call W_RESET()                                              ' ###172

            mExitFlag = cFRS_NORMAL
            Me.Close()                                                  ' フォームを閉じる

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormLoaderAlarm.BtnOK_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "Cancelボタン押下時"
    '''=========================================================================
    ''' <summary>Cancelボタン押下時 ###073</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click

        Dim strMSG As String

        Try
            '----- ###144↓ -----
            ' 載物台ワーク/ハンドワーク解除ボタンを表示して、STARTキー押下待ちとする
            BtnTableVacumeOff.Visible = True                            ' 載物台吸着解除ボタン表示
            BtnTableClampOff.Visible = True                             ' 載物台クランプ解除ボタン表示
            BtnHandVacumeOff.Visible = True                             ' 供給ハンド吸着解除ボタン表示
            BtnHand2VacumeOff.Visible = True                            ' 収納ハンド吸着解除ボタン表示 ###158
            LblMSG.Visible = True                                       ' "装置内に基板が残っている場合は", "取り除いてください" ###158
            BtnCancel.Visible = False                                   ' Cancelボタン非表示

            mExitFlag = Sub_WaitStartRestKey(cFRS_ERR_START)            ' STARTキー押下待ち
            If (mExitFlag = cFRS_NORMAL) Then                           ' STARTキー押下なら
                mExitFlag = cFRS_ERR_RST                                ' Retuen値 = Cancelボタン押下
            End If
            'mExitFlag = cFRS_ERR_RST
            '----- ###144↑ -----

            ''V6.0.5.0⑦↓'V4.12.2.2③
            'STARTキーが押されたらキャンセル処理を実行する
            If (mExitFlag = cFRS_ERR_START) Then                           ' STARTキー押下なら
                mExitFlag = cFRS_ERR_RST                                ' Retuen値 = Cancelボタン押下
            End If
            ''V6.0.5.0⑦↑'V4.12.2.2③

            Me.Close()                                                  ' フォームを閉じる

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormLoaderAlarm.BtnCancel_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "供給ハンド吸着解除ボタン押下時"
    '''=========================================================================
    ''' <summary>ハンド吸着解除ボタン押下時 ###158 ###144</summary>
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

            ' ハンド１/ハンド２吸着ＯＦＦ
            Call W_HAND1_VACUME()                                       ' ハンド１吸着ＯＦＦ
            'Call W_HAND2_VACUME()                                      ' ハンド２吸着ＯＦＦ ###158

            ' アラームリセット送出 ###147
            Call W_RESET()

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormLoaderAlarm.BtnFreeHand_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "収納ハンド吸着解除ボタン押下時"
    '''=========================================================================
    ''' <summary>収納ハンド吸着解除ボタン押下時 ###158</summary>
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

            ' アラームリセット送出
            Call W_RESET()

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormLoaderAlarm.BtnHand2VacumeOff_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "載物台クランプ解除ボタン押下時"
    '''=========================================================================
    ''' <summary>載物台クランプ解除ボタン押下時 ###144</summary>
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

            ' アラームリセット送出 ###147
            Call W_RESET()

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormLoaderAlarm.BtnFreeTable_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "載物台吸着解除ボタン押下時"
    '''=========================================================================
    ''' <summary>載物台吸着解除ボタン押下時 ###144</summary>
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

            ' アラームリセット送出 ###147
            Call W_RESET()

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormLoaderAlarm.BtnFreeTable_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "Buzzer Offボタン押下時(タッチパネル用)"
    '''=========================================================================
    ''' <summary>Buzzer Offボタン押下時 ###073</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnBZOff_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnBZOff.Click

        Dim strMSG As String

        Try
            ' ブザーOFF
            Call W_BzOff()

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormLoaderAlarm.BtnBZOff_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
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
            ' アラーム種別を設定する
            Select Case AlarmKind
                Case cFRS_NORMAL                                                    ' アラームなし
                    LblKind.Text = MSG_LOADER_16 & "(" & "---" & ")"                ' ローダアラームリスト(---)

                Case cFRS_ERR_LDR1 ' 全停止異常発生中
                    LblKind.Text = MSG_LOADER_16 & "(" & MSG_SPRASH19 & ")"         ' ローダアラームリスト(全停止異常発生中)

                Case cFRS_ERR_LDR2 ' ｻｲｸﾙ停止異常発生中
                    LblKind.Text = MSG_LOADER_16 & "(" & MSG_SPRASH20 & ")"         ' ローダアラームリスト(サイクル停止異常発生中)

                Case cFRS_ERR_LDR3  ' 軽故障発生中
                    LblKind.Text = MSG_LOADER_16 & "(" & MSG_SPRASH21 & ")"         ' ローダアラームリスト(軽故障発生中)

                Case cFRS_ERR_PLC   ' PLCステータス異常
                    LblKind.Text = MSG_LOADER_16 & "(" & MSG_SPRASH30 & ")"         ' ローダアラームリスト(PLCステータス異常)
            End Select

            '----- V6.1.1.0⑬↓ -----
            If (giAlmTimeDsp = 1) Then                                              ' ローダアラーム時の時間表示の有無(0=表示なし, 1=表示あり)　
                Call Get_NowYYMMDDHHMMSS(strMSG)
                LblKind.Text = LblKind.Text + " " + strMSG
            End If
            '----- V6.1.1.0⑬↑ -----

            ' ローダアラームリストを設定する
            Call ListAlarm.Items.Clear()
            'txtInfo.Text = ""
            TxtExec.Text = ""
            For i = 0 To (AlarmCount - 1)
                ListAlarm.Items.Add(strLoaderAlarm(i))                              ' アラーム文字列
            Next
            'ListAlarm.SelectedIndex = 0

            ' 最初は先頭を表示
            If (AlarmCount > 0) Then
                'txtInfo.Text = strLoaderAlarmInfo(0)        
                TxtExec.Text = strLoaderAlarmExec(0)
            End If
            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormLoaderAlarm.SetAlarmList() TRAP ERROR = " + ex.Message
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
            strMSG = "FormLoaderAlarm.ListAlarm_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "アラームレベルの保存"
    ''' <summary>
    ''' アラームレベルの保存 'V5.0.0.7①
    ''' </summary>
    ''' <param name="AlarmLevel"></param>
    ''' <remarks></remarks>
    Public Sub SetAlarmLevel(ByVal AlarmLevel As Integer)

        mAlarmLevel = AlarmLevel

    End Sub

#End Region

#Region "アラームレベルの取得"
    ''' <summary>
    ''' アラームレベルの取得 'V5.0.0.7①
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAlarmLevel() As Integer

        Return (mAlarmLevel)

    End Function


#End Region

#End Region

End Class