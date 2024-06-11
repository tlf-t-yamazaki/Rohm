'===============================================================================
'   Description  : カット位置補正画面処理【CHIP/NET 外部カメラ】
'
'   Copyright(C) : OMRON LASERFRONT INC. 2011
'
'===============================================================================
Option Strict Off
Option Explicit On

Imports LaserFront.Trimmer.DefTrimFnc
Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8①
Imports TKY_ALL_SL432HW.My.Resources    'V4.4.0.0-0

Friend Class FrmCutPosCorrect
    Inherits System.Windows.Forms.Form
    Implements ICommonMethods              'V6.0.0.0⑪

#Region "プライベート定数/変数定義"
    '===========================================================================
    '   定数/変数定義
    '===========================================================================
    '----- 変数定義 -----
    Private stJOG As JOG_PARAM                                          ' 矢印画面(JOG操作)用パラメータ
    Private mExit_flg As Short                                          ' 終了フラグ
    Private dblTchMoval(3) As Double                                    ' ピッチ退避域(0=ﾋﾟｯﾁ, 1=HIGHﾋﾟｯﾁ, 2=Pause Time(Sec))

    '----- カット位置補正用 -----
    Private iMaxDataNum As Short                                        ' 処理対象データ数
    Private iResistorNumber(MaxCntResist + 1) As Short                  ' 抵抗番号
    Private pfStartPos(2, MaxCntResist + 1) As Double                   ' ティーチングテーブル(X,Y 最大抵抗数(1 ORG))
    Private pfStartPosTeachPoint(2, MaxCntResist + 1) As Double         ' ティーチングテーブル前回補正値保存用(X,Y 最大抵抗数(1 ORG))V5.0.0.6⑩
#End Region

    '========================================================================================
    '   Ｆｏｒｍ処理
    '========================================================================================
#Region "終了結果を返す"
    ' '''=========================================================================
    ' '''<summary>終了結果を返す</summary>
    ' '''<returns>cFRS_NORMAL   = 正常
    ' '''         cFRS_ERR_RST  = Cancel(RESETｷｰ)
    ' '''         -1以下        = エラー</returns>
    ' '''=========================================================================
    'Public ReadOnly Property sGetReturn() As Integer Implements ICommonMethods.sGetReturn  'V6.0.0.0⑪
    '    'V6.0.0.0⑪    Public ReadOnly Property sGetReturn() As Short
    '    Get
    '        If (mExit_flg = cFRS_ERR_START) Then
    '            sGetReturn = cFRS_NORMAL
    '        Else
    '            sGetReturn = mExit_flg
    '        End If
    '    End Get
    'End Property
#End Region

#Region "ﾌｫｰﾑ初期化時処理"
    '''=========================================================================
    '''<summary>ﾌｫｰﾑ初期化時処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub Form_Initialize_Renamed()

        'V6.0.0.0⑫                  ↓
        stJOG.TenKey = New Button() {BtnJOG_0, BtnJOG_1, BtnJOG_2, BtnJOG_3,
                                     BtnJOG_4, BtnJOG_5, BtnJOG_6, BtnJOG_7, BtnHI}
        'V6.0.0.0⑫                  ↑

    End Sub
#End Region

#Region "Form FormClosed時処理"
    '''=========================================================================
    '''<summary>Form Closed時処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub CutPosCorrect_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

    End Sub
#End Region

#Region "Form Load時処理"
    '''=========================================================================
    '''<summary>Form Load時処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub CutPosCorrect_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        Dim strMSG As String

        Try
            ' 初期処理
            Call SetMessages()                                          ' ラベル等を設定する(日本語/英語)
            mExit_flg = -1                                              ' 終了フラグ = 初期化

            ' ｳｲﾝﾄﾞｳをｺﾝﾄﾛｰﾙに重ねる
            Me.Top = Form1.Text4.Top + DISPOFF_SUBFORM_TOP
            Me.Left = Form1.Text4.Left

            ' トリミングデータより必要なパラメータを取得する
            Call GetTrimData()

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "FrmCutPosCorrect.FrmCutPosCorrect_Load() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "ラベル等を設定する(日本語/英語)"
    '''=========================================================================
    '''<summary>ラベル等を設定する(日本語/英語)</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub SetMessages()

        'Dim intChipNum As Short
        Dim strMSG As String

        Try
            ' ラベル等を設定する(日本語/英語)
            'Call PrepareMessagesCutPosCorrect_Calibration(gSysPrm.stTMN.giMsgTyp)
            'Me.frmTitle.Text = FRM_CUT_POS_CORRECT_TITLE                                    ' "カット位置補正"
            'Me.lblGroupResistorTitle.Text = LBL_CUT_POS_CORRECT_GROUP_RESISTOR_NUMBER       ' "グループ内抵抗数"
            'Me.lblCutCorrectOffsetXTitle.Text = LBL_CUT_POS_CORRECT_OFFSET_X                ' "カット位置補正テーブルオフセットＸ[mm]"
            'Me.lblCutCorrectOffsetYTitle.Text = LBL_CUT_POS_CORRECT_OFFSET_Y                ' "カット位置補正テーブルオフセットＹ[mm]"
            'V1.25.0.0⑫'V5.0.0.6⑪↓
            If iExcamCutBlockNo_X > 0 Then
                strMSG = LBL_CUT_POS_CORRECT_BLOCK_NO_X + "(1-" + iExcamCutBlockNo_X.ToString("0") + ")"
            Else
                'V1.25.0.0⑫'V5.0.0.6⑪↑
                strMSG = LBL_CUT_POS_CORRECT_BLOCK_NO_X + "(1-" + typPlateInfo.intBlockCntXDir.ToString("0") + ")"
            End If      'V1.25.0.0⑫'V5.0.0.6⑪
            Me.lblBlockNoX.Text = strMSG                                                    ' "ブロックNo X軸(1～99）"
            strMSG = LBL_CUT_POS_CORRECT_BLOCK_NO_Y + "(1-" + typPlateInfo.intBlockCntYDir.ToString("0") + ")"
            Me.lblBlockNoY.Text = strMSG                                                    ' "ブロックNo Y軸(1～99）"

            ' 表示データに初期値を設定する
            'Call GetChipNum(intChipNum)
            'Me.lblGroupResistor.Text = intChipNum.ToString("0")                             ' グループ内抵抗数
            Me.lblGroupResistor.Text = typPlateInfo.intResistCntInBlock.ToString("0")       ' ブロック内抵抗数 
            Me.lblCutCorrectOffsetX.Text = typPlateInfo.dblCutPosiReviseOffsetXDir.ToString("##0.0000")     ' ｶｯﾄ位置補正ﾃｰﾌﾞﾙｵﾌｾｯﾄX
            Me.lblCutCorrectOffsetY.Text = typPlateInfo.dblCutPosiReviseOffsetYDir.ToString("##0.0000")     ' ｶｯﾄ位置補正ﾃｰﾌﾞﾙｵﾌｾｯﾄY
            Me.txtBlockNoX.Text = "1"                                                       ' ブロック番号(X軸)
            Me.txtBlockNoY.Text = "1"                                                       ' ブロック番号(Y軸)

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "FrmCutPosCorrect.SetMessages() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "Form Activated時処理"
#If False Then                          'V6.0.0.0⑬ Execute()でおこなう
    '''=========================================================================
    '''<summary>Form Activated時処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub CutPosCorrect_Activated(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Activated

        Dim strMSG As String

        Try
            ' カット位置補正実行
            If (mExit_flg <> -1) Then Exit Sub
            mExit_flg = 0                                               ' 終了フラグ = 0
            mExit_flg = CutPosCorrectMain()                             ' カット位置補正実行

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "FrmCutPosCorrect.CutPosCorrect_Activated() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            mExit_flg = cERR_TRAP                                       ' Return値 = 例外エラー
        End Try
        Me.Close()

    End Sub
#End If
#End Region

#Region "メイン処理実行"
    ''' <summary>メイン処理実行</summary>
    ''' <returns>実行結果</returns>
    ''' <remarks>'V6.0.0.0⑬</remarks>
    Public Function Execute() As Integer Implements ICommonMethods.Execute
        Try
            ' カット位置補正実行
            If (mExit_flg <> -1) Then Exit Function
            mExit_flg = 0                                               ' 終了フラグ = 0
            mExit_flg = CutPosCorrectMain()                             ' カット位置補正実行

            ' トラップエラー発生時
        Catch ex As Exception
            Dim strMSG As String = "FrmCutPosCorrect.Execute() TRAP ERROR = " & ex.Message
            MsgBox(strMSG)
            mExit_flg = cERR_TRAP                                       ' Return値 = 例外エラー
        End Try

        Dim ret As Integer
        If (mExit_flg = cFRS_ERR_START) Then    ' sGetReturn 取り込み   'V6.0.0.0⑬
            ret = cFRS_NORMAL
        Else
            ret = mExit_flg
        End If

        Me.Close()
        Return ret

    End Function
#End Region

    '========================================================================================
    '   ボタン押下時処理
    '========================================================================================
#Region "OKボタン押下時処理"
    '''=========================================================================
    '''<summary>OKボタン押下時処理</summary>
    '''<remarks>手動ティーチング処理用</remarks>
    '''=========================================================================
    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        stJOG.Flg = cFRS_NORMAL                                        ' 正常を設定する
    End Sub
#End Region

#Region "Cancelボタン押下時処理"
    '''=========================================================================
    '''<summary>Cancelボタン押下時処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        stJOG.Flg = cFRS_ERR_RST                                        ' Cancel(RESETｷｰ)
    End Sub
#End Region

    '========================================================================================
    '   カット位置補正処理
    '========================================================================================
#Region "カット位置補正メイン処理"
    '''=========================================================================
    '''<summary>カット位置補正メイン処理</summary>
    '''<returns>cFRS_ERR_START=OK(STARTｷｰ)
    '''         cFRS_ERR_RST  =Cancel(RESETｷｰ)
    '''         -1以下        =エラー</returns>
    '''=========================================================================
    Private Function CutPosCorrectMain() As Integer

        Dim mdBSx As Double                                                     ' ﾌﾞﾛｯｸｻｲｽﾞX
        Dim mdBSy As Double                                                     ' ﾌﾞﾛｯｸｻｲｽﾞY
        Dim BlkNumX As Integer
        Dim BlkNumY As Integer
        Dim StepNum As Integer
        Dim r As Integer
        Dim rtn As Integer
        Dim strMSG As String

        Try
            '---------------------------------------------------------------------------
            '   初期処理
            '---------------------------------------------------------------------------
            BlkNumX = 1                                                         ' ブロック番号X,Y初期化
            BlkNumY = 1
            txtBlockNoX.Text = BlkNumX.ToString("0")
            txtBlockNoY.Text = BlkNumY.ToString("0")
            Call CalcBlockSize(mdBSx, mdBSy)                                    ' ブロックサイズXY設定
            Call LAMP_CTRL(LAMP_START, True)                                    ' STARTランプON
            Call LAMP_CTRL(LAMP_RESET, True)                                    ' RESETランプON

            ' 矢印画面(JOG操作)用パラメータを初期化する
            stJOG.Md = MODE_KEY                                                 ' モード(キー入力待ちモード)
            stJOG.Md2 = MD2_BUTN                                                ' 入力モード(0:画面ﾎﾞﾀﾝ入力, 1:ｺﾝｿｰﾙ入力)
            stJOG.Opt = CONSOLE_SW_RESET + CONSOLE_SW_START                     ' キーの有効(1)/無効(0)指定
            stJOG.PosX = 0.0                                                    ' BP X位置/XYﾃｰﾌﾞﾙ X位置
            stJOG.PosY = 0.0                                                    ' BP Y位置/XYﾃｰﾌﾞﾙ Y位置
            stJOG.BpOffX = typPlateInfo.dblBpOffSetXDir                         ' BPｵﾌｾｯﾄX 
            stJOG.BpOffY = typPlateInfo.dblBpOffSetYDir                         ' BPｵﾌｾｯﾄY 
            stJOG.BszX = mdBSx                                                  ' ﾌﾞﾛｯｸｻｲｽﾞX 
            stJOG.BszY = mdBSy                                                  ' ﾌﾞﾛｯｸｻｲｽﾞY
            'V4.7.0.0⑩            stJOG.TextX = fgrdPoints                                            ' BP位置表示用グリッド
            '            stJOG.TextX = dgvPoints                                             ' BP位置表示用グリッド
            'V6.0.0.0⑪            stJOG.TextX = 0                                                     ' BP位置表示用グリッド
            stJOG.TextX = Nothing                                               ' BP位置表示用グリッド   'V6.0.0.0⑪
            'V6.0.0.0⑪            stJOG.TextY = 0                                                     ' 
            stJOG.TextY = Nothing                                               '   'V6.0.0.0⑪
            stJOG.cgX = 0.0                                                     ' 移動量X 
            stJOG.cgY = 0.0                                                     ' 移動量Y 
            stJOG.BtnHI = BtnHI                                                 ' HIボタン
            stJOG.BtnZ = BtnZ                                                   ' Zボタン
            stJOG.BtnSTART = BtnSTART                                           ' STARTボタン
            stJOG.BtnHALT = BtnHALT                                             ' HALTボタン
            stJOG.BtnRESET = BtnRESET                                           ' RESETボタン
            Call JogEzInit(stJOG, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
            Call Me.Focus()                                                     ' フォーカスを設定する(テンキー入力用) 

            '---------------------------------------------------------------------------
            '   ブロック番号入力処理
            '---------------------------------------------------------------------------
STP_BLKINP:
            ' ガイダンスメッセージ表示
            Me.lblInfo.Text = MSG_CUT_POS_CORRECT_014                           ' "ブロック番号を入力後、[START]キーを押してください"
            StepNum = 0                                                         ' StepNum = ブロック番号入力
            Call SetTxtBlkNumBackColor(True)                                    ' ブロック番号テキストボックスの活性化

            Form1.Instance.VideoLibrary1.SetTrackBarEnabled(True)       'V6.0.0.0-21
            ' コンソール入力(JOG画面のSTART/RESETキー入力待ち)
            r = WaitJogStartResetKey(stJOG, CONSOLE_SW_RESET + CONSOLE_SW_START)
            If (r < cFRS_NORMAL) Then                                           ' エラーならエラーリターン(メッセージ表示済み)
                Return (r)
            End If
            ' RESETキーなら終了確認メッセージ表示へ
            If (r = cFRS_ERR_RST) Then                                          ' RESETキー押下 ?
                Call LAMP_CTRL(LAMP_RESET, True)                                ' RESETランプON
                GoTo STP_ENDMSG                                                 ' 終了確認メッセージ表示へ

                ' STARTキーならブロック番を入力する
            Else
                r = InputBlkNum(BlkNumX, BlkNumY)
                If (r <> cFRS_NORMAL) Then GoTo STP_BLKINP '                    ' エラーならブロック番号入力へ

            End If
            Call SetTxtBlkNumBackColor(False)                                   ' ブロック番号テキストボックスの非活性化
            Form1.Instance.VideoLibrary1.SetTrackBarEnabled(False)      'V6.0.0.0-21

            '---------------------------------------------------------------------------
            '   指定のブロック番号位置にテーブルを移動する(内部カメラ下)
            '---------------------------------------------------------------------------
            ' テーブルを指定ブロック位置に移動する(内部カメラ下)
            r = XYTableMoveBlock(0, 1, 1, BlkNumX, BlkNumY)
            If (r <> cFRS_NORMAL) Then                                          ' エラーならエラーリターン(メッセージ表示済み)
                If (Form1.System1.IsSoftLimitXY(r) = False) Then
                    Return (r)
                End If
            End If

            '-------------------------------------------------------------------
            '   指定ブロックの全ての抵抗に十字カットを行う(内部カメラ下)
            '-------------------------------------------------------------------
            StepNum = 1                                                         ' StepNum = 十字カット
            r = CutPosCrossCut(typPlateInfo)
            If (r = cFRS_ERR_RST) Then
                ' ブロック番号入力指定まで処理を戻す
                GoTo STP_BLKINP
            ElseIf (r <> cFRS_NORMAL) Then                                          ' エラーならエラーリターン(メッセージ表示済み)
                ' エラーならエラーリターン(メッセージ表示済み)
                Return (r)
            End If

            '-------------------------------------------------------------------
            '   十字カットした抵抗のパターン認識(外部カメラ)を行いずれ量を算出する
            '-------------------------------------------------------------------
            StepNum = 2                                                         ' StepNum = ずれ量算出
            r = CutPosCorrect(typPlateInfo, BlkNumX, BlkNumY)
            GoTo STP_END                                                        ' 処理終了 

            '-------------------------------------------------------------------
            '   終了確認メッセージを表示する
            '-------------------------------------------------------------------
STP_ENDMSG:
            ' "前の画面に戻ります。よろしいですか？　
            rtn = Form1.System1.TrmMsgBox(gSysPrm, MSG_105, MsgBoxStyle.OkCancel, MSG_OPLOG_CUT_POS_CORRECT_START)
            If (rtn = cFRS_ERR_RST) Then                                        '「いいえ」選択ならティーチングに戻る
                If (StepNum = 0) Then                                           ' StepNum = ブロック番号入力 ?
                    GoTo STP_BLKINP                                             ' ブロック番号入力へ
                End If
            End If

            '---------------------------------------------------------------------------
            '   終了処理
            '---------------------------------------------------------------------------
STP_END:
            Call ZCONRST()                                                      ' ラッチ解除
            Call LAMP_CTRL(LAMP_RESET, False)                                   ' RESETランプOFF
            Call LAMP_CTRL(LAMP_Z, False)                                       ' PRBランプOFF
            Return (r)

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "FrmCutPosCorrect.CutPosCorrectMain() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                                  ' Return値 = 例外エラー

        Finally
            Form1.Instance.VideoLibrary1.SetTrackBarEnabled(True)       'V6.0.0.0-21
        End Try
    End Function
#End Region

#Region "指定ブロックの全ての抵抗に十字カットを行う"
    '''=========================================================================
    ''' <summary>指定ブロックの全ての抵抗に十字カットを行う</summary>
    ''' <param name="stPLT">  (INP)プレートデータ</param>
    ''' <returns>cFRS_NORMAL  = 正常
    '''          cFRS_ERR_RST = RESET(Cancel)キー
    '''          上記以外     = エラー</returns>
    '''=========================================================================
    Private Function CutPosCrossCut(ByRef stPLT As PlateInfo) As Integer

        Dim r As Integer
        Dim rtn As Integer
        Dim StepNum As Integer
        Dim SwitchChk As Integer = 0
        Dim PosX As Double
        Dim PosY As Double
        Dim dQrate As Double
        Dim CondNum As Integer
        Dim ObjProc As Process = Nothing
        Dim bFlg As Boolean = False
        Dim bHALT As Boolean = False
        Dim strMSG As String

        Try
            '---------------------------------------------------------------------------
            '   初期処理
            '---------------------------------------------------------------------------
#If VIDEO_CAPTURE = 0 Then
            Call Form1.VideoLibrary1.ChangeCamera(INTERNAL_CAMERA)                    ' カメラ切替(内部カメラ)
#End If
            ' ↓↓↓ V3.1.0.0② 2014/12/01
            'r = Exec_GazouProc(ObjProc, DISPGAZOU_PATH, DISPGAZOU_WRK, 0) ' 画像表示プログラムを起動する
            'V6.0.0.0⑤            r = Exec_GazouProc(ObjProc, DISPGAZOU_PATH, DISPGAZOU_WRK, 0, 0) ' 画像表示プログラムを起動する
            ' ↑↑↑ V3.1.0.0② 2014/12/01
            SendCrossLineMsgToDispGazou()                               ' V6.1.4.18①

            StepNum = 0                                                 ' StepNum = 加工条件入力
            stJOG.CurrentNo = 1                                         ' 処理中の行
            Call BpMoveOrigin_Ex()
STP_INPCND:
            ' 加工条件を入力する(FL時)
            CondNum = 0                                                 ' 加工条件番号 
            dQrate = stPLT.dblCutPosiReviseCutQRate                     ' Qﾚｰﾄ = カット位置補正用Qレート 
            r = Sub_FlCond(CondNum, dQrate, Me)
            If (r <> cFRS_NORMAL) Then
                'If (r = cFRS_ERR_RST) Then
                '    Return r '                                          ' 前の状態に戻す
                'End If
                GoTo STP_END
            End If
            Me.Refresh()                                                ' ※加工条件入力画面表示を消すため
            StepNum = 1                                                 ' StepNum = 十字カット

            '---------------------------------------------------------------------------
            '   全ての抵抗に十字カットを行う
            '---------------------------------------------------------------------------
STP_CROSCUT:
            stJOG.Flg = -1
            Do
                ' BP移動
                'V5.0.0.6⑩↓外部カメラカット位置補正量の加算
                If giTeachpointUse = 1 Then
                    PosX = pfStartPos(0, stJOG.CurrentNo) + typPlateInfo.dblCutPosiReviseOffsetXDir + pfStartPosTeachPoint(0, stJOG.CurrentNo)  ' ﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄX
                    PosY = pfStartPos(1, stJOG.CurrentNo) + typPlateInfo.dblCutPosiReviseOffsetYDir + pfStartPosTeachPoint(1, stJOG.CurrentNo)  ' ﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄY
                Else
                    'V5.0.0.6⑩↑
                    PosX = pfStartPos(0, stJOG.CurrentNo) + typPlateInfo.dblCutPosiReviseOffsetXDir
                    PosY = pfStartPos(1, stJOG.CurrentNo) + typPlateInfo.dblCutPosiReviseOffsetYDir
                End If                                                  'V5.0.0.6⑩
                r = Form1.System1.EX_MOVE(gSysPrm, PosX, PosY, 1)       ' BP絶対値移動
                If (r < cFRS_NORMAL) Then                               ' エラーならエラーリターン(メッセージ表示済み)
                    GoTo STP_END
                End If

                ' ガイダンスメッセージ表示
                Me.lblInfo.Text = MSG_CUT_POS_CORRECT_004               '  "【警告】" & "[START]：十字カット実行，[RESET]：中止"
                Call Set_BackColor(stJOG.CurrentNo)                     ' グリッドの現在行の背景を黄色にする

                ' HALT SWチェック
                r = HALT_SWCHECK(SwitchChk)
                If (SwitchChk <> 0) Then                                ' HALT SWが押された ?
                    Console.WriteLine("HALT SW ON")
                    If (bHALT = False) Then                             ' HALT SW状態 = OFFなら
                        bHALT = True                                    ' HALT SW状態をONにする
                        Call LAMP_CTRL(LAMP_HALT, True)                 ' HALTランプON
                    Else                                                ' HALT SW状態 = ONなら
                        bHALT = False                                   ' HALT SW状態をOFFにする
                        Call LAMP_CTRL(LAMP_HALT, False)                ' HALTランプOFF
                    End If
                End If

                ' コンソール入力(JOG画面のSTART/RESET/HALTキー入力待ち)
                If (bFlg = False) Or (bHALT = True) Then                ' 初回又はHALT SWがONの時にSTART/RESETキー入力待ち
                    bFlg = True
STP_INPCONS:
                    Form1.Instance.VideoLibrary1.SetTrackBarEnabled(True)       'V6.0.0.0-21

                    ' コンソール入力(JOG画面のSTART/RESET/HALTキー入力待ち)
                    r = WaitJogStartResetKey(stJOG, CONSOLE_SW_RESET + CONSOLE_SW_START + CONSOLE_SW_HALT)
                    If (r < cFRS_NORMAL) Then                           ' エラーならエラーリターン(メッセージ表示済み)
                        GoTo STP_END
                    End If
                    ' RESETキーなら終了確認メッセージ表示へ
                    If (r = cFRS_ERR_RST) Then                          ' RESETキー押下 ?
                        Call LAMP_CTRL(LAMP_RESET, True)                ' RESETランプON
                        GoTo STP_ENDMSG                                 ' 終了確認メッセージ表示へ

                    ElseIf (r = cFRS_ERR_HALT) Then                     ' HALTキー押下 ?
                        If (bHALT = False) Then                         ' HALT SW状態 = OFFなら
                            bHALT = True                                ' HALT SW状態をONにする
                            Call LAMP_CTRL(LAMP_HALT, True)             ' HALTランプON
                        Else                                            ' HALT SW状態 = ONなら
                            bHALT = False                               ' HALT SW状態をOFFにする
                            Call LAMP_CTRL(LAMP_HALT, False)            ' HALTランプOFF
                        End If
                        GoTo STP_INPCONS                                ' コンソール入力(JOG画面のSTART/RESET/HALTキー入力待ち)へ
                    End If

                    Form1.Instance.VideoLibrary1.SetTrackBarEnabled(False)      'V6.0.0.0-21
                End If

                ' 十字カットを実行する
                Me.lblInfo.Text = MSG_CUT_POS_CORRECT_005               ' "十字カット実行中" & vbCrLf & "[HALT]:一時停止"
                Me.lblInfo.Refresh()
                r = CrossCutExec(PosX, PosY, CondNum, dQrate, stPLT.dblCutPosiReviseCutLength, stPLT.dblCutPosiReviseCutSpeed)
                If (r < cFRS_NORMAL) Then                               ' エラーならエラーリターン(メッセージ表示済み)
                    GoTo STP_END
                End If

                ' 十字カット後はBPをカット位置中央へ戻す
                r = Form1.System1.EX_MOVE(gSysPrm, PosX, PosY, 1)       ' BP絶対値移動
                If (r < cFRS_NORMAL) Then                               ' エラーならエラーリターン(メッセージ表示済み)
                    GoTo STP_END
                End If

                ' システムエラーチェック
                System.Windows.Forms.Application.DoEvents()             ' メッセージポンプ
                r = Form1.System1.SysErrChk_ForVBNET(giAppMode)         ' システムエラーチェック
                If (r <> cFRS_NORMAL) Then
                    GoTo STP_END
                End If
                Call System.Threading.Thread.Sleep(1)                   ' Wait(ms)

                ' 次の行へ
                Call Reset_BackColor(stJOG.CurrentNo)                   ' グリッドの現在行の背景色を元に戻す
                stJOG.CurrentNo = stJOG.CurrentNo + 1                   ' 処理中の行 + 1
                If (stJOG.CurrentNo > iMaxDataNum) Then                 ' 処理対象データ数分十字カットした ?
                    Me.lblInfo.Text = MSG_CUT_POS_CORRECT_011           ' "十字カット終了
                    r = cFRS_NORMAL                                     ' 正常終了
                    GoTo STP_END
                End If
                If (stJOG.CurrentNo <= 5) Then                          ' 5行目以降は自動ｽｸﾛｰﾙ
                    Call Set_TopRow(1)                                  ' グリッドの指定行を先頭にする 
                Else
                    Call Set_TopRow(stJOG.CurrentNo - 5)                ' グリッドの指定行を先頭にする 
                End If

            Loop While (stJOG.Flg = -1)

            ' 当画面からOK(START)/RESET(Cancel)ﾎﾞﾀﾝ押下ならrに戻値を設定する
            If (stJOG.Flg <> -1) Then
                r = stJOG.Flg
                If (r = cFRS_ERR_RST) Then                              ' RESET(Cancel)ﾎﾞﾀﾝ押下なら終了確認メッセージ表示へ
                    GoTo STP_ENDMSG
                End If
            End If

            '-------------------------------------------------------------------
            '   終了確認メッセージを表示する
            '-------------------------------------------------------------------
STP_ENDMSG:
            ' "前の画面に戻ります。よろしいですか？　
            rtn = Form1.System1.TrmMsgBox(gSysPrm, MSG_105, MsgBoxStyle.OkCancel, MSG_OPLOG_CUT_POS_CORRECT_START)
            If (rtn = cFRS_ERR_RST) Then                                '「いいえ」選択なら処理を継続する
                ' HALT SW状態がONならHALTランプONとする (※TrmMsgBox()でラッチ解除&HALTランプがOFFされる)
                If (bHALT = True) Then
                    Call LAMP_CTRL(LAMP_HALT, True)
                End If
                If (StepNum = 0) Then                                   ' StepNum = 加工条件入力 ?
                    GoTo STP_INPCND                                     ' 加工条件番号入力へ
                Else
                    GoTo STP_CROSCUT                                    ' 十字カット実行へ
                End If
            End If

            ' 終了処理 
STP_END:
            Call Reset_AllBackColor(iMaxDataNum)                        'V4.7.0.0⑩
            'V6.0.0.0⑤            Call End_GazouProc(ObjProc)                                 ' 画像表示プログラムを終了する
            Call LAMP_CTRL(LAMP_HALT, False)                            ' HALTランプOFF
            Return (r)

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "FrmCutPosCorrect.CutPosCrossCut() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return値 = 例外エラー

        Finally                         'V6.0.0.0-21
            Form1.Instance.VideoLibrary1.SetTrackBarEnabled(True)
        End Try
    End Function
#End Region

#Region "十字カットした抵抗のパターン認識(外部カメラ)を行い、ずれ量を算出する"
    '''=========================================================================
    ''' <summary>十字カットした抵抗のパターン認識(外部カメラ)を行い、ずれ量を算出する</summary>
    ''' <param name="stPLT">  (INP)プレートデータ</param>
    ''' <param name="iBlockX"></param>
    ''' <param name="iBlockY"></param>
    ''' <returns>cFRS_NORMAL  = 正常
    '''          cFRS_ERR_RST = RESET(Cancel)キー
    '''          上記以外     = エラー</returns>
    '''=========================================================================
    Private Function CutPosCorrect(ByRef stPLT As PlateInfo, ByRef iBlockX As Short, ByRef iBlockY As Short) As Integer

        Dim Idx As Integer
        Dim EndNo As Integer
        Dim r As Integer
        Dim rtn As Integer
        Dim StepNum As Integer
        Dim intGroup As Short
        Dim intTemp As Short
        Dim dblGapX(MaxCntResist + 1) As Double                         ' ずれ量X(最大抵抗数(1 ORG))
        Dim dblGapY(MaxCntResist + 1) As Double                         ' ずれ量Y(最大抵抗数(1 ORG))
        Dim ObjProc As Process = Nothing
        Dim fcoeff As Double                                            ' 相関値
        Dim crx As Double                                               ' ずれ量X(ワーク)
        Dim cry As Double                                               ' ずれ量Y(ワーク)
        Dim strMSG As String

        Try
            '---------------------------------------------------------------------------
            '   初期処理
            '---------------------------------------------------------------------------
            Call gparModules.CrossLineDispOff()    'V5.0.0.6⑫
            StepNum = 0                                                 ' StepNum = コンソール入力(START/RESETキー入力待ち)
            stJOG.CurrentNo = 1                                         ' 処理中の行
            intGroup = stPLT.intCutPosiReviseGroupNo                    ' ﾊﾟﾀｰﾝﾏｯﾁﾝｸﾞｸﾞﾙｰﾌﾟ番号
            intTemp = stPLT.intCutPosiRevisePtnNo                       ' ﾊﾟﾀｰﾝﾏｯﾁﾝｸﾞﾃﾝﾌﾟﾚｰﾄ番号

            For Idx = 1 To MaxCntResist                                 ' ずれ量初期化
                dblGapX(Idx) = 0.0                                      ' ずれ量X
                dblGapY(Idx) = 0.0                                      ' ずれ量Y
            Next Idx

            Call Form1.System1.EX_BPOFF(gSysPrm, 0.0#, 0.0#)            ' BPオフセット(外部ｶﾒﾗではBPは使用できないので初期化しておく)
            r = Form1.System1.EX_MOVE(gSysPrm, 0.0#, 0.0#, 1)           ' 最初のｶｯﾄ位置中央へ移動
            If (r < cFRS_NORMAL) Then                                   ' エラーならエラーリターン(メッセージ表示済み)
                Return (r)
            End If

            ' テーブルを外部カメラ下に移動
            r = XYTableMoveBlockFirst(1, 1, 1, iBlockX, iBlockY, stJOG.CurrentNo)
            If (r < cFRS_NORMAL) Then                                   ' エラーならエラーリターン(メッセージ表示済み)
                GoTo STP_END
            End If

            Call Reset_AllBackColor(iMaxDataNum)                        ' グリッドの全行の背景色を元に戻す
            Call Set_TopRow(stJOG.CurrentNo)                            ' グリッドの指定行を先頭にする 

#If VIDEO_CAPTURE = 0 Then
            Call Form1.VideoLibrary1.ChangeCamera(EXTERNAL_CAMERA)                    ' カメラ切替(外部カメラ)
#End If
            ' 画像表示プログラムを起動する(外部カメラ)
            'r = Exec_GazouProc(ObjProc, DISPGAZOU_PATH, DISPGAZOU_WRK, 1)

STP_INPCONS:
            ' ガイダンスメッセージ表示
            Me.lblInfo.Text = MSG_CUT_POS_CORRECT_007                   ' "[START]：画像認識実行" & vbCrLf & "[RESET]：中止"
            StepNum = 0                                                 ' StepNum = ブロック番号入力

            Form1.Instance.VideoLibrary1.SetTrackBarEnabled(True)       'V6.0.0.0-21

            ' コンソール入力(START/RESETキー入力待ち)
            r = WaitJogStartResetKey(stJOG, CONSOLE_SW_RESET + CONSOLE_SW_START)
            If (r < cFRS_NORMAL) Then                                   ' エラーならエラーリターン(メッセージ表示済み)
                GoTo STP_END
            End If
            ' RESETキーなら終了確認メッセージ表示へ
            If (r = cFRS_ERR_RST) Then                                  ' RESETキー押下 ?
                Call LAMP_CTRL(LAMP_RESET, True)                        ' RESETランプON
                GoTo STP_ENDMSG                                         ' 終了確認メッセージ表示へ
            End If
            StepNum = 1                                                 ' StepNum = パターン認識実行

            Form1.Instance.VideoLibrary1.SetTrackBarEnabled(False)      'V6.0.0.0-21
            System.Windows.Forms.Application.DoEvents()                 ' メッセージポンプ

            '---------------------------------------------------------------------------
            '   パターン認識(外部カメラ)を実行し、ずれ量を算出する
            '---------------------------------------------------------------------------
STP_PTNRECOG:
            stJOG.Flg = -1
            Do
                ' ガイダンスメッセージ表示
                Me.lblInfo.Text = MSG_CUT_POS_CORRECT_016               ' "画像認識実行実行中"
                Me.lblInfo.Refresh()
                Call Set_BackColor(stJOG.CurrentNo)                     ' グリッドの指定行の背景色を黄色にする 

                ' テーブルを外部カメラ下に移動
                r = XYTableMoveBlockFirst(1, 1, 1, iBlockX, iBlockY, stJOG.CurrentNo)
                If (r < cFRS_NORMAL) Then                               ' エラーならエラーリターン(メッセージ表示済み)
                    GoTo STP_END
                End If
                Call System.Threading.Thread.Sleep(1000)                ' テーブル移動後画像安定待ち時間(ms)

                ' 外部カメラでパターン認識を行う
                r = Sub_PatternMatching(intGroup, intTemp, crx, cry, fcoeff)
                If (r = cFRS_NORMAL) Then                               ' 正常 ? 
                    'crx = dblGapX(stJOG.CurrentNo)                     '###152
                    'cry = dblGapY(stJOG.CurrentNo)                     '###152
                    dblGapX(stJOG.CurrentNo) = crx
                    dblGapY(stJOG.CurrentNo) = cry
                Else
                    If (r = cFRS_ERR_PT2) Then                          ' 画像マッチングエラー(閾値エラー) ?
                        '"画像マッチングエラー (相関係数=x.xxx)" + "矢印スイッチ:位置調整, [START]:次項, [HALT]:前項"
                        strMSG = MSG_CUT_POS_CORRECT_013 + " (" + MSG_CUT_POS_CORRECT_018 + fcoeff.ToString("0.000") + ")" + vbCrLf + MSG_CUT_POS_CORRECT_017
                    Else                                                ' パターン認識エラー(パターンが見つからなかった)
                        '"画像が見つかりません" + "矢印スイッチ:位置調整, [START]:次項, [HALT]:前項"
                        strMSG = MSG_CUT_POS_CORRECT_019 + vbCrLf + MSG_CUT_POS_CORRECT_017
                    End If
                    Me.lblInfo.Text = strMSG                            ' "画像マッチングエラー" + "矢印スイッチ:位置調整, [START]:次項, [HALT]:前項"
                    Me.lblInfo.Refresh()
                    EndNo = stJOG.CurrentNo                             ' 最終行 = 処理中の行番号(処理中の行番号以前までの手動ティーチング処理を行う)  
STP_MANUAL:
                    Form1.Instance.VideoLibrary1.SetTrackBarEnabled(True)       'V6.0.0.0-21

                    ' 手動ティーチング処理
                    r = ManualTeach(stPLT, iBlockX, iBlockY, stJOG.CurrentNo, EndNo, dblGapX, dblGapY)
                    If (r <> cFRS_NORMAL) Then                          ' 正常以外なら終了
                        GoTo STP_END
                    Else                                                ' 正常リターン時 
                        If (StepNum = 2) Then                           ' StepNum = 手動ティーチング処理なら「保存確認メッセージ」表示へ
                            GoTo STP_EXITMSG
                        End If
                    End If

                    Form1.Instance.VideoLibrary1.SetTrackBarEnabled(False)      'V6.0.0.0-21
                End If

                ' ずれ量を表示する
                Call DispGapXY(stJOG.CurrentNo, dblGapX(stJOG.CurrentNo), dblGapY(stJOG.CurrentNo))

                ' 次の行へ
                Call Reset_BackColor(stJOG.CurrentNo)                   ' グリッドの指定行の背景色を元に戻す
                stJOG.CurrentNo = stJOG.CurrentNo + 1                   ' 処理中の行 + 1
                ' 最終行の場合
                If (stJOG.CurrentNo > iMaxDataNum) Then                 ' 処理対象データ数分画像認識した ?
                    Me.lblInfo.Text = MSG_CUT_POS_CORRECT_012           ' "画像認識終了"
                    stJOG.CurrentNo = stJOG.CurrentNo - 1               ' 処理中の行 - 1
STP_EXITMSG:
                    ' "この情報を保存して前の画面に戻ります。よろしいですか？"　
                    r = Form1.System1.TrmMsgBox(gSysPrm, MSG_106, MsgBoxStyle.OkCancel, MSG_OPLOG_CUT_POS_CORRECT_START)
                    If (r = cFRS_ERR_RST) Then                          '「いいえ」選択なら処理を継続する
                        StepNum = 2                                     ' StepNum = 手動ティーチング
                        EndNo = iMaxDataNum                             ' 最終行 = 処理対象データ数 
                        Me.lblInfo.Text = MSG_CUT_POS_CORRECT_017       ' "矢印スイッチ:位置調整, [START]:次項, [HALT]:前項"
                        GoTo STP_MANUAL                                 ' 手動ティーチング処理へ
                    End If

                    ' 「はい」選択ならデータ更新して終了
                    Call UpdateData(dblGapX, dblGapY)                   ' データ更新
                    GoTo STP_END                                        ' 正常終了 
                End If

                ' 最終行以外の場合
                If (stJOG.CurrentNo <= 5) Then                          ' 5行目以降はグリッド自動スクロール
                    Call Set_TopRow(1)                                  ' グリッドの指定行を先頭にする 
                Else
                    Call Set_TopRow(stJOG.CurrentNo - 5)                ' グリッドの指定行を先頭にする 
                End If

                ' システムエラーチェック
                System.Windows.Forms.Application.DoEvents()             ' メッセージポンプ
                r = Form1.System1.SysErrChk_ForVBNET(giAppMode)         ' システムエラーチェック
                If (r <> cFRS_NORMAL) Then
                    GoTo STP_END
                End If
                Call System.Threading.Thread.Sleep(1)                   ' Wait(ms)

            Loop While (stJOG.Flg = -1)

            ' 当画面からOK(START)/RESET(Cancel)ﾎﾞﾀﾝ押下ならrに戻値を設定する
            If (stJOG.Flg <> -1) Then
                r = stJOG.Flg
                If (r = cFRS_ERR_RST) Then                              ' RESET(Cancel)ﾎﾞﾀﾝ押下なら終了確認メッセージ表示へ
                    GoTo STP_ENDMSG
                End If
            End If

            '-------------------------------------------------------------------
            '   終了確認メッセージを表示する
            '-------------------------------------------------------------------
STP_ENDMSG:
            ' "前の画面に戻ります。よろしいですか？　
            rtn = Form1.System1.TrmMsgBox(gSysPrm, MSG_105, MsgBoxStyle.OkCancel, MSG_OPLOG_CUT_POS_CORRECT_START)
            If (rtn = cFRS_ERR_RST) Then                                '「いいえ」選択なら処理を継続する
                If (StepNum = 0) Then                                   ' StepNum = コンソール入力(START/RESETキー入力待ち) ?
                    GoTo STP_INPCONS                                    ' コンソール入力(START/RESETキー入力待ち)へ
                ElseIf (StepNum = 1) Then                               ' StepNum = パターン認識実行 ?
                    GoTo STP_PTNRECOG                                   ' パターン認識実行へ
                Else
                    GoTo STP_MANUAL                                     ' 手動ティーチング処理へ
                End If
            End If

            ' 終了処理
STP_END:
#If VIDEO_CAPTURE = 0 Then
            Call Form1.VideoLibrary1.ChangeCamera(INTERNAL_CAMERA)                    ' カメラ切替(内部カメラ)
#End If
            'Call End_GazouProc(ObjProc)                                 ' 画像表示プログラムを終了する
            Call Form1.VideoLibrary1.PatternDisp(False)                 ' パターンマッチング時の検索範囲枠(黄色枠と青色枠)を非表示とする
            Return (r)

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "FrmCutPosCorrect.CutPosCorrect() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return値 = 例外エラー

        Finally
            Form1.Instance.VideoLibrary1.SetTrackBarEnabled(True)       'V6.0.0.0-21
        End Try
    End Function
#End Region

#Region "手動ティーチング処理"
    '''=========================================================================
    ''' <summary>手動ティーチング処理</summary>
    ''' <param name="stPLT">      (INP)プレートデータ</param>
    ''' <param name="BlkX">       (INP)ブロック番号X</param>
    ''' <param name="BlkY">       (INP)ブロック番号Y</param>
    ''' <param name="piCurrentNo">(INP)処理中の行番号</param>
    ''' <param name="EndNo">      (INP)最終行番号</param>
    ''' <param name="dblGapX">    (I/O)ずれ量X</param>
    ''' <param name="dblGapY">    (I/O)ずれ量Y</param>
    ''' <returns>cFRS_NORMAL    = 正常
    '''          cFRS_ERR_RST   = Cancel(RESETｷｰ)
    '''          -1以下         = エラー</returns>
    '''=========================================================================
    Private Function ManualTeach(ByRef stPLT As PlateInfo, ByVal BlkX As Integer, ByVal BlkY As Integer, ByRef piCurrentNo As Integer, ByVal EndNo As Integer, ByRef dblGapX() As Double, ByRef dblGapY() As Double) As Integer

        Dim mdBSx As Double                                                     ' ﾌﾞﾛｯｸｻｲｽﾞX
        Dim mdBSy As Double                                                     ' ﾌﾞﾛｯｸｻｲｽﾞY
        Dim X As Double
        Dim Y As Double
        Dim POSX As Double
        Dim POSY As Double
        Dim r As Integer
        Dim rtn As Integer
        Dim strMSG As String

        Try
            '---------------------------------------------------------------------------
            '   初期処理
            '---------------------------------------------------------------------------
            Call Reset_AllBackColor(iMaxDataNum)                        ' グリッドの全行の背景色を元に戻す
            Call Set_TopRow(stJOG.CurrentNo)                            ' グリッドの指定行を先頭にする 
            cmdOK.Visible = True                                        ' OKボタン表示
            Call CalcBlockSize(mdBSx, mdBSy)                            ' ブロックサイズXY設定

            ' 矢印画面(JOG操作)用パラメータを初期化する
            'Call ZGETPHPOS(POSX, POSY)                                  ' X,Yテーブル位置取得 V1.14.0.0⑦
            Call ZGETPHPOS2(POSX, POSY)                                  ' X,Yテーブル位置取得 V1.14.0.0⑦
            stJOG.Md = MODE_STG                                         ' モード(XYテーブルモード)
            stJOG.Md2 = MD2_BUTN                                        ' 入力モード(0:画面ﾎﾞﾀﾝ入力, 1:ｺﾝｿｰﾙ入力)
            stJOG.Opt = CONSOLE_SW_RESET + CONSOLE_SW_START + CONSOLE_SW_HALT   ' キーの有効(1)/無効(0)指定
            stJOG.PosX = POSX                                           ' テーブルX座標
            stJOG.PosY = POSY                                           ' テーブルY座標
            stJOG.BpOffX = stPLT.dblBpOffSetXDir                        ' BPｵﾌｾｯﾄX 
            stJOG.BpOffY = stPLT.dblBpOffSetYDir                        ' BPｵﾌｾｯﾄY 
            stJOG.BszX = mdBSx                                          ' ﾌﾞﾛｯｸｻｲｽﾞX 
            stJOG.BszY = mdBSy                                          ' ﾌﾞﾛｯｸｻｲｽﾞY
            'V4.7.0.0⑩            stJOG.TextX = fgrdPoints                                    ' ずれ量X表示用ラベル(グリッド)
            stJOG.TextX = dgvPoints                                     ' ずれ量X表示用ラベル(グリッド)
            'V6.0.0.0⑪            stJOG.TextY = 0                                             ' ずれ量Y表示用ラベル(未使用)
            stJOG.TextY = Nothing                                       ' ずれ量Y表示用ラベル(未使用)   'V6.0.0.0⑪
            stJOG.cgX = dblGapX(piCurrentNo)                            ' 移動量X(ずれ量X)
            stJOG.cgY = dblGapY(piCurrentNo)                            ' 移動量Y(ずれ量Y)
            stJOG.BtnHI = BtnHI                                         ' HIボタン
            stJOG.BtnZ = BtnZ                                           ' Zボタン
            stJOG.BtnSTART = BtnSTART                                   ' STARTボタン
            stJOG.BtnHALT = BtnHALT                                     ' HALTボタン
            stJOG.BtnRESET = BtnRESET                                   ' RESETボタン
            stJOG.CurrentNo = piCurrentNo                               ' 処理中の行番号
            Call JogEzInit(stJOG, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
            Call Me.Focus()                                             ' フォーカスを設定する(テンキー入力用) 

            '---------------------------------------------------------------------------
            '   カットスタート位置ティーチング処理
            '---------------------------------------------------------------------------
STP_TEACH:
            stJOG.Flg = -1
            Do
                Call Set_BackColor(stJOG.CurrentNo)                     ' グリッドの指定行の背景色を黄色にする 

                ' テーブル絶対値移動(外部カメラ下)
                r = XYTableMoveBlockFirst(1, 1, 1, BlkX, BlkY, stJOG.CurrentNo)
                If (r <> cFRS_NORMAL) Then                              ' エラーならエラーリターン(メッセージ表示済み)
                    GoTo STP_END
                End If
                Call System.Threading.Thread.Sleep(1000)                ' テーブル移動後画像安定待ち時間(ms)

                '-------------------------------------------------------------------
                '   JOG操作画面処理
                '-------------------------------------------------------------------
                'Call ZGETPHPOS(POSX, POSY)                              ' X,Yテーブル位置取得 V1.14.0.0⑦
                Call ZGETPHPOS2(POSX, POSY)                              ' X,Yテーブル位置取得 V1.14.0.0⑦
                'V5.0.0.6⑳ ADD START↓
                If dblGapX(stJOG.CurrentNo) <> 0.0 Or dblGapX(stJOG.CurrentNo) <> 0.0 Then
                    POSX = POSX + dblGapX(stJOG.CurrentNo)
                    POSY = POSY + dblGapY(stJOG.CurrentNo)
                    r = Form1.System1.XYtableMove(gSysPrm, POSX, POSY)
                    If (r <> cFRS_NORMAL) Then                      ' エラーならエラーリターン(メッセージ表示済み)
                        If (Form1.System1.IsSoftLimitXY(r) = False) Then
                            GoTo STP_END
                        End If
                    End If
                End If
                'V5.0.0.6⑳ ADD END↓
                stJOG.PosX = POSX                                       ' テーブルX座標
                stJOG.PosY = POSY                                       ' テーブルY座標
                stJOG.cgX = dblGapX(stJOG.CurrentNo)                    ' 移動量X(ずれ量X)
                stJOG.cgY = dblGapY(stJOG.CurrentNo)                    ' 移動量Y(ずれ量Y)
                'V6.0.0.0⑪                r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
                r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval, Me)  'V6.0.0.0⑪
                If (r < cFRS_NORMAL) Then                               ' エラーならエラーリターン(メッセージ表示済み)
                    GoTo STP_END
                End If
                ' STARTキー押下以外なら現在のテーブル位置(BP位置)を取得する
                If (r <> cFRS_ERR_START) Then
                    X = stJOG.cgX                                       ' 移動量X(ずれ量X)
                    Y = stJOG.cgY                                       ' 移動量Y(ずれ量Y)
                End If

                '-----------------------------------------------------------------------
                '   HALTキー押下時処理(１つ前に移動)
                '-----------------------------------------------------------------------
                If (r = cFRS_ERR_HALT) Then                             ' HALTキー押下 ?
                    ' 先頭行でなければ前行へ
                    If (stJOG.CurrentNo > 1) Then                       ' 先頭行ならNOP
                        Call Reset_BackColor(stJOG.CurrentNo)           ' グリッドの指定行の背景色を元に戻す
                        stJOG.CurrentNo = stJOG.CurrentNo - 1           ' 行番号 - 1
                        If (stJOG.CurrentNo <= 5) Then
                            Call Set_TopRow(1)
                        Else
                            Call Set_TopRow(stJOG.CurrentNo - 5)
                        End If

                        ' テーブル絶対値移動
                        X = pfStartPos(1, stJOG.CurrentNo)
                        Y = pfStartPos(2, stJOG.CurrentNo)
                        r = Form1.System1.XYtableMove(gSysPrm, POSX + X, POSY + Y)
                        If (r <> cFRS_NORMAL) Then                      ' エラーならエラーリターン(メッセージ表示済み)
                            If (Form1.System1.IsSoftLimitXY(r) = False) Then
                                GoTo STP_END
                            End If
                        End If
                        Call System.Threading.Thread.Sleep(1000)        ' テーブル移動後画像安定待ち時間(ms)
                    End If

                    '-----------------------------------------------------------------------
                    '   RESETキー押下時処理(ﾃｨｰﾁﾝｸﾞCancel終了)
                    '-----------------------------------------------------------------------
                ElseIf (r = cFRS_ERR_RST) Then                          ' RESETキー押下 ?
                    Call LAMP_CTRL(LAMP_RESET, True)                    ' RESETランプON
                    GoTo STP_ENDMSG                                     ' 終了確認メッセージ表示へ

                    '-----------------------------------------------------------------------
                    '  STARTキー押下時処理(次行へ移動)
                    '-----------------------------------------------------------------------
                ElseIf (r = cFRS_ERR_START) Then                        ' STARTキー押下 ?
                    ' 現在のテーブル位置(BP位置)を取得する
                    Call System.Threading.Thread.Sleep(1000)            ' Wait(ms)
                    dblGapX(stJOG.CurrentNo) = stJOG.cgX                ' 移動量X(ずれ量X)
                    dblGapY(stJOG.CurrentNo) = stJOG.cgY                ' 移動量Y(ずれ量Y)
                    X = dblGapX(stJOG.CurrentNo)
                    Y = dblGapY(stJOG.CurrentNo)
                    Call DispGapXY(stJOG.CurrentNo, X, Y)               'V4.7.0.0⑩ ずれ量X,Yを表示する
                    ' 最終行 ?
                    If (stJOG.CurrentNo + 1 > EndNo) Then
                        r = cFRS_NORMAL                                 ' 最終行なら正常リターン 
                        GoTo STP_END

                        ' 最終行でなければ次行へ
                    Else
                        Call Reset_BackColor(stJOG.CurrentNo)           ' グリッドの指定行の背景色を元に戻す
                        stJOG.CurrentNo = stJOG.CurrentNo + 1
                        If (stJOG.CurrentNo <= 5) Then
                            Call Set_TopRow(1)
                        Else
                            Call Set_TopRow(stJOG.CurrentNo - 5)
                        End If

                        X = dblGapX(stJOG.CurrentNo)
                        Y = dblGapY(stJOG.CurrentNo)

                        ' テーブル絶対値移動(最終抵抗)
                        'V5.0.0.6⑳コメント↓
                        'r = Form1.System1.XYtableMove(gSysPrm, POSX + X, POSY + Y)
                        'If (r <> cFRS_NORMAL) Then                      ' エラーならエラーリターン(メッセージ表示済み)
                        '    If (Form1.System1.IsSoftLimitXY(r) = False) Then
                        '        GoTo STP_END
                        '    End If
                        'End If
                        'V5.0.0.6⑳コメント↑
                        Call System.Threading.Thread.Sleep(1000)        ' テーブル移動後画像安定待ち時間(ms)
                    End If
                    Call ZCONRST()                                      ' ラッチ解除
                    'V4.7.0.0⑩ Call DispGapXY(stJOG.CurrentNo, X, Y)               ' ずれ量X,Yを表示する

                End If

                ' メッセージポンプ
                System.Windows.Forms.Application.DoEvents()
                Call System.Threading.Thread.Sleep(1)                   ' Wait(ms)

                ' 非常停止等チェック
                r = Form1.System1.SysErrChk_ForVBNET(APP_MODE_TEACH)
                If (r <> cFRS_NORMAL) Then
                    GoTo STP_END
                End If
            Loop While (stJOG.Flg = -1)

            ' 当画面からOK/RESET(Cancel)ﾎﾞﾀﾝ押下ならrに戻値を設定する
            If (stJOG.Flg <> -1) Then
                r = stJOG.Flg
                stJOG.Flg = -1                                          ' 親画面の為にFLGを初期化する 
                If (r = cFRS_NORMAL) Then                               ' OKボタン押下なら正常終了
                    dblGapX(stJOG.CurrentNo) = stJOG.cgX                ' 移動量X(ずれ量X)
                    dblGapY(stJOG.CurrentNo) = stJOG.cgY                ' 移動量Y(ずれ量Y)
                    GoTo STP_END
                End If
            End If

            '-------------------------------------------------------------------
            '   終了確認メッセージを表示する
            '-------------------------------------------------------------------
STP_ENDMSG:
            ' "前の画面に戻ります。よろしいですか？　
            rtn = Form1.System1.TrmMsgBox(gSysPrm, MSG_105, MsgBoxStyle.OkCancel, MSG_OPLOG_CUT_POS_CORRECT_START)
            If (rtn = cFRS_ERR_RST) Then                                '「いいえ」選択ならティーチングに戻る
                GoTo STP_TEACH                                          ' カットスタート位置ティーチングへ
            End If

            '---------------------------------------------------------------------------
            '   終了処理
            '---------------------------------------------------------------------------
STP_END:
            cmdOK.Visible = False                                       ' OKボタン非表示
            Call ZCONRST()                                              ' ラッチ解除
            Call LAMP_CTRL(LAMP_RESET, False)                           ' RESETランプOFF
            Call LAMP_CTRL(LAMP_Z, False)                               ' PRBランプOFF
            Return (r)

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "FrmCutPosCorrect.ManualTeach() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return値 = 例外エラー
        End Try
    End Function
#End Region

#Region "コンソール入力(JOG画面のキー入力(START/RESET等)待ち)"
    '''=========================================================================
    ''' <summary>コンソール入力(JOG画面のキー入力(START/RESET等)待ち)</summary>
    ''' <param name="stJOG"> (INP)矢印画面(JOG操作)用パラメータ</param>
    ''' <param name="KeyOpt">(INP)入力待ちするキー</param>
    ''' <returns>cFRS_ERR_START = START(OK)キー
    '''          cFRS_ERR_RST   = RESET(Cancel)キー
    '''          cFRS_ERR_HALT  = HALTキー
    '''          上記以外       = エラー</returns>
    '''=========================================================================
    Private Function WaitJogStartResetKey(ByRef stJOG As JOG_PARAM, ByVal KeyOpt As UShort) As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' コンソール入力(JOG画面のキー入力待ち)
            Call ZCONRST()                                              ' ラッチ解除
            Call LAMP_CTRL(LAMP_START, True)                                    ' STARTランプON
            Call LAMP_CTRL(LAMP_RESET, True)                                    ' RESETランプON
            stJOG.Md = MODE_KEY                                         ' モード(キー入力待ちモード)
            stJOG.Opt = KeyOpt                                          ' キーの有効(1)/無効(0)指定
            'V6.0.0.0⑪            r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
            r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval, Me)  'V6.0.0.0⑪
            If (r < cFRS_NORMAL) Then                                   ' エラーならエラーリターン(メッセージ表示済み)
                Return (r)
            End If
            If (r = cFRS_ERR_RST) Then                                  ' RESETキー押下 ?
                Call LAMP_CTRL(LAMP_RESET, True)                        ' RESETランプON
                Return (r)                                              ' Return値 = RESET(Cancel)キー
            End If
            Return (r)                                                  ' Return値 = START(OK)キー他

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "FrmCutPosCorrect.WaitJogStartResetKey() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return値 = 例外エラー
        End Try
    End Function
#End Region

    '========================================================================================
    '   トリミングデータの入出力処理
    '========================================================================================
#Region "トリミングデータより必要なパラメータを取得する"
    '''=========================================================================
    '''<summary>トリミングデータより必要なパラメータを取得する</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub GetTrimData()

        Dim i As Short
        Dim intChipNum As Short
        Dim intRegNum As Short
        Dim dblX As Double
        Dim dblY As Double
        Dim strMSG As String
        Dim bRtn As Boolean         'V5.0.0.6⑩

        Try
            ' 内部変数初期化
            For i = 1 To MaxCntResist
                iResistorNumber(i) = 0                                  ' 抵抗番号
                pfStartPos(0, i) = 0.0                                  ' ﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄX
                pfStartPos(1, i) = 0.0                                  ' ﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄY
            Next

            ' ティーチングテーブルにテーブル座標を設定する
            iMaxDataNum = 0                                             ' 処理対象データ数 = 0 
            'Call GetChipNum(intChipNum)                                 ' ｸﾞﾙｰﾌﾟ内抵抗数取得
            intChipNum = typPlateInfo.intResistCntInBlock               ' ブロック内抵抗数取得
            For i = 1 To intChipNum                                     ' 抵抗数分繰り返す
                If (GetRegNum(i, intRegNum)) Then                       ' i番目の抵抗番号取得
                    If ((1 <= intRegNum) And (MARKING_RESNO_SET > intRegNum)) Then   ' ﾏｰｷﾝｸﾞは対象外
                        ' 抵抗番号の最初のカットデータのスタートポイントX,Yを取得する
                        'V5.0.0.6⑩            If (GetCutTeachPoint(intRegNum, 1, dblX, dblY)) Then
                        'V4.7.0.0⑩             If (GetCutStartPoint(intRegNum, 1, dblX, dblY)) Then
                        'V5.0.0.6⑩↓
                        If giTeachpointUse = 0 Then                     ' KOA EW時
                            bRtn = GetCutTeachPoint(intRegNum, 1, dblX, dblY)
                        Else
                            bRtn = GetCutStartPoint(intRegNum, 1, dblX, dblY)
                        End If
                        'V5.0.0.6⑩↑
                        If bRtn Then
                            iMaxDataNum = iMaxDataNum + 1               ' 処理対象データ数+1
                            iResistorNumber(iMaxDataNum) = intRegNum    ' 抵抗番号
                            pfStartPos(0, iMaxDataNum) = dblX           ' ﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄX
                            pfStartPos(1, iMaxDataNum) = dblY           ' ﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄY

                            'V5.0.0.6⑩↓外部カメラカット位置補正量の加算
                            pfStartPosTeachPoint(0, iMaxDataNum) = 0.0          ' 初期化
                            pfStartPosTeachPoint(1, iMaxDataNum) = 0.0
                            If giTeachpointUse = 1 Then
                                bRtn = GetCutTeachPoint(intRegNum, 1, dblX, dblY)
                                If bRtn Then    'カット位置補正値をティーチングポイントに格納した場合で前の補正値を読み出して保存しておく
                                    pfStartPosTeachPoint(0, iMaxDataNum) = dblX         ' ﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄX
                                    pfStartPosTeachPoint(1, iMaxDataNum) = dblY         ' ﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄY
                                Else
                                End If
                            End If
                            'V5.0.0.6⑩↑
                        End If
                    End If
                End If
            Next

            ' 表示用グリッドの初期化
            Call GridInitialize()

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "FrmCutPosCorrect.GetTrimData() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "トリミングデータ更新(ずれ量をスタートポイントに反映)"
    '''=========================================================================
    ''' <summary>トリミングデータ更新(ずれ量をｽﾀｰﾄﾎﾟｲﾝﾄに反映)</summary>
    ''' <param name="dblGapX">(INP)ずれ量X</param>
    ''' <param name="dblGapY">(INP)ずれ量Y</param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub UpdateData(ByRef dblGapX() As Double, ByRef dblGapY() As Double)

        Dim intChipNum As Short
        Dim bRetc As Boolean = False
        Dim i As Short
        Dim j As Short
        Dim intRegNum As Short
        Dim intCutNum As Short
        Dim dblX As Double
        Dim dblY As Double
        Dim strMSG As String
        Dim dBpSizeX As Double, dBpSizeY As Double  'V4.7.0.0⑩ 
        Dim dGapX As Double, dGapY As Double, dStartX As Double, dStartY As Double, dTeachX As Double, dTeachY As Double       'V5.0.0.6⑩

        Try
            'V4.7.0.0⑩ ADD START↓
            If (gSysPrm.stDEV.giBpSize = BPSIZE_6060) Then
                dBpSizeX = 60.0
                dBpSizeY = 60.0
            ElseIf (gSysPrm.stDEV.giBpSize = BPSIZE_6030) Then
                dBpSizeX = 60.0
                dBpSizeY = 30.0
            ElseIf (gSysPrm.stDEV.giBpSize = BPSIZE_90) Then    'V4.3.0.0②
                dBpSizeX = 90.0
                dBpSizeY = 90.0
            Else
                dBpSizeX = 80.0
                dBpSizeY = 80.0
            End If
            'V4.7.0.0⑩ ADD START↑
            'Call GetChipNum(intChipNum)
            intChipNum = typPlateInfo.intResistCntInBlock               ' ブロック内抵抗数 
            For i = 1 To intChipNum                                     ' 抵抗数分ｺﾋﾟｰ
                If (GetRegNum(i, intRegNum)) Then                       ' i番目の抵抗番号取得
                    If ((1 <= intRegNum) And (999 >= intRegNum)) Then   ' ﾏｰｷﾝｸﾞは対象外
                        If (GetRegCutNum(intRegNum, intCutNum)) Then    ' ｶｯﾄ数取得
                            For j = 1 To intCutNum
                                'V5.0.0.6⑩↓
                                If giTeachpointUse = 1 Then             ' 補正量をティーチングポイントに格納する処理の場合
                                    If (GetCutStartPoint(intRegNum, j, dStartX, dStartY)) Then ' 抵抗番号の最初のｶｯﾄﾃﾞｰﾀ
                                        If (GetCutTeachPoint(intRegNum, j, dTeachX, dTeachY)) Then
                                            dGapX = dblGapX(i) * -1.0 + dTeachX
                                            dGapY = dblGapY(i) * -1.0 + dTeachY
                                            dblX = dStartX + dGapX            ' スタートポイント = カット位置 + ずれ量X
                                            dblY = dStartY + dGapY
                                        Else
                                            MsgBox("FrmCutPosCorrect.UpdateData.GetCutTeachPoint ERROR REG=[" + intRegNum.ToString() + "] CUT=[" + j.ToString() + "]")
                                        End If

                                        If (-dBpSizeX > dblX) Then
                                            dblX = -dBpSizeX  ' 超えている場合は最大値、最小値に補正
                                            dGapX = dblX - dStartX
                                            bRetc = True
                                        End If
                                        If (dBpSizeX < dblX) Then
                                            dblX = dBpSizeX
                                            dGapX = dblX - dStartX
                                            bRetc = True
                                        End If
                                        If (-dBpSizeY > dblY) Then
                                            dblY = -dBpSizeY
                                            dGapY = dblY - dStartY
                                            bRetc = True
                                        End If
                                        If (dBpSizeY < dblY) Then
                                            dblY = dBpSizeY
                                            dGapY = dblY - dStartY
                                            bRetc = True
                                        End If
                                        ' スタートポイントをずれ量で更新する
                                        Call SetCutTeachPoint(intRegNum, j, dGapX, dGapY)
                                        Console.WriteLine("S " & intRegNum & " " & j & " " & dGapX & " " & dGapY)
                                    Else
                                        MsgBox("FrmCutPosCorrect.UpdateData.GetCutStartPoint ERROR REG=[" + intRegNum.ToString() + "] CUT=[" + j.ToString() + "]")
                                    End If
                                Else
                                    'V5.0.0.6⑩↑
                                    'If (GetCutTeachPoint(intRegNum, j, dblX, dblY)) Then ' 抵抗番号の最初のｶｯﾄﾃﾞｰﾀ
                                    If (GetCutStartPoint(intRegNum, j, dblX, dblY)) Then
                                        'V4.7.0.0⑩                                If (GetCutStartPoint(intRegNum, j, dblX, dblY)) Then
                                        dblX = dblX + dblGapX(i)            ' スタートポイント = カット位置 + ずれ量X
                                        dblY = dblY + dblGapY(i)
                                        dblX = RoundOff(dblX)               ' 四捨五入
                                        dblY = RoundOff(dblY)

                                        ' 上限下限ﾁｪｯｸ
                                        'V4.7.0.0⑩ ADD START↓
                                        If (-dBpSizeX > dblX) Then
                                            dblX = -dBpSizeX  ' 超えている場合は最大値、最小値に補正
                                            bRetc = True
                                        End If
                                        If (dBpSizeX < dblX) Then
                                            dblX = dBpSizeX
                                            bRetc = True
                                        End If
                                        If (-dBpSizeY > dblY) Then
                                            dblY = -dBpSizeY
                                            bRetc = True
                                        End If
                                        If (dBpSizeY < dblY) Then
                                            dblY = dBpSizeY
                                            bRetc = True
                                        End If
                                        'V4.7.0.0⑩ コメント START↑

                                        'V4.7.0.0⑩ ADD START↓
                                        'If (-gSysPrm.stDEV.giBpSize > dblX) Then
                                        '    dblX = -gSysPrm.stDEV.giBpSize  ' 超えている場合は最大値、最小値に補正
                                        '    bRetc = True
                                        'End If
                                        'If (gSysPrm.stDEV.giBpSize < dblX) Then
                                        '    dblX = gSysPrm.stDEV.giBpSize
                                        '    bRetc = True
                                        'End If
                                        'If (-gSysPrm.stDEV.giBpSize > dblY) Then
                                        '    dblY = -gSysPrm.stDEV.giBpSize
                                        '    bRetc = True
                                        'End If
                                        'If (gSysPrm.stDEV.giBpSize < dblY) Then
                                        '    dblY = gSysPrm.stDEV.giBpSize
                                        '    bRetc = True
                                        'End If
                                        'V4.7.0.0⑩ コメント START↑

                                        ' スタートポイントを更新する
                                        Call SetCutStartPoint(intRegNum, j, dblX, dblY)
                                        Console.WriteLine("S " & intRegNum & " " & j & " " & dblX & " " & dblY)
                                    End If
                                    'V4.7.0.0⑩ ADD START↓
                                    If (GetCutTeachPoint(intRegNum, j, dblX, dblY)) Then ' 抵抗番号の最初のｶｯﾄﾃﾞｰﾀ
                                        'V4.7.0.0⑩                                If (GetCutStartPoint(intRegNum, j, dblX, dblY)) Then
                                        dblX = dblX + dblGapX(i)            ' スタートポイント = カット位置 + ずれ量X
                                        dblY = dblY + dblGapY(i)
                                        dblX = RoundOff(dblX)               ' 四捨五入
                                        dblY = RoundOff(dblY)

                                        If (-dBpSizeX > dblX) Then
                                            dblX = -dBpSizeX  ' 超えている場合は最大値、最小値に補正
                                            bRetc = True
                                        End If
                                        If (dBpSizeX < dblX) Then
                                            dblX = dBpSizeX
                                            bRetc = True
                                        End If
                                        If (-dBpSizeY > dblY) Then
                                            dblY = -dBpSizeY
                                            bRetc = True
                                        End If
                                        If (dBpSizeY < dblY) Then
                                            dblY = dBpSizeY
                                            bRetc = True
                                        End If
                                        ' スタートポイントを更新する
                                        Call SetCutTeachPoint(intRegNum, j, dblX, dblY)
                                        Console.WriteLine("S " & intRegNum & " " & j & " " & dblX & " " & dblY)
                                    End If
                                    'V4.7.0.0⑩ ADD START↓
                                End If  'V5.0.0.6⑩
                            Next
                        End If
                    End If
                End If
            Next

            If (bRetc) Then
                ' "補正値更新エラー" & vbCrLf & "更新結果が範囲を超えています。" & vbCrLf & "最大値、最小値に補正されます。"
                MsgBox(MSG_CUT_POS_CORRECT_010, MsgBoxStyle.OkOnly)
            End If

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "FrmCutPosCorrect.UpdateData() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

    '========================================================================================
    '   ブロック番号処理
    '========================================================================================
#Region "ブロック番号テキストボックスの活性化/非活性化"
    '''=========================================================================
    ''' <summary>ブロック番号テキストボックスの活性化/非活性化</summary>
    ''' <param name="mode">(INP)True=活性化, False=非活性化</param>
    '''=========================================================================
    Private Sub SetTxtBlkNumBackColor(ByVal mode As Boolean)

        If (mode = True) Then
            txtBlockNoX.Enabled = True
            txtBlockNoX.BackColor = System.Drawing.Color.Yellow
            txtBlockNoY.Enabled = True
            txtBlockNoY.BackColor = System.Drawing.Color.Yellow
        Else
            txtBlockNoX.Enabled = False
            txtBlockNoX.BackColor = System.Drawing.SystemColors.Control
            txtBlockNoY.Enabled = False
            txtBlockNoY.BackColor = System.Drawing.SystemColors.Control
        End If

    End Sub
#End Region

#Region "ブロック番号(X軸)変更時"
    '''=========================================================================
    '''<summary>ブロック番号(X軸)変更時</summary>
    '''<remarks>txtBlockNoX.TextChanged は、フォームが初期化されたときに発生します。</remarks>
    '''=========================================================================
    Private Sub txtBlockNoX_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtBlockNoX.TextChanged

        If (Not IsNumeric(Me.txtBlockNoX.Text)) Then                    ' 数字以外なら
            Me.txtBlockNoX.Text = "1"
        End If

    End Sub
#End Region

#Region "ブロック番号(Y軸)変更時"
    '''=========================================================================
    '''<summary>ブロック番号(Y軸)変更時</summary>
    '''<remarks>txtBlockNoY.TextChanged は、フォームが初期化されたときに発生します。</remarks>
    '''=========================================================================
    Private Sub txtBlockNoY_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtBlockNoY.TextChanged

        If (Not IsNumeric(Me.txtBlockNoX.Text)) Then                    ' 数字以外なら
            Me.txtBlockNoX.Text = "1"
        End If

    End Sub
#End Region

#Region "ブロック番号入力"
    '''=========================================================================
    ''' <summary>ブロック番号入力</summary>
    ''' <param name="BlkNumX">(OUT)ブロック番号X</param>
    ''' <param name="BlkNumY">(OUT)ブロック番号Y</param>
    ''' <returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function InputBlkNum(ByRef BlkNumX As Integer, ByRef BlkNumY As Integer) As Integer

        Dim Blk As Integer
        Dim Idx As Integer
        Dim strMSG As String

        Try
            ' ブロック番号範囲チェック
            Idx = 0
            Blk = CInt(txtBlockNoX.Text)
            If (Blk < 1) Or (Blk > typPlateInfo.intBlockCntXDir) Then GoTo STP_ERR
            'V1.25.0.0⑫'V5.0.0.6⑪↓
            If iExcamCutBlockNo_X > 0 Then
                If (Blk < 1) Or (Blk > iExcamCutBlockNo_X) Then GoTo STP_ERR
            End If
            'V1.25.0.0⑫'V5.0.0.6⑪↑
            Idx = 1
            Blk = CInt(txtBlockNoY.Text)
            If (Blk < 1) Or (Blk > typPlateInfo.intBlockCntYDir) Then GoTo STP_ERR

            ' ブロック番号を返す
            BlkNumX = CInt(txtBlockNoX.Text)
            BlkNumY = CInt(txtBlockNoY.Text)
            Return (cFRS_NORMAL)

STP_ERR:
            ' "ブロック番号入力エラー"
            'Call Form1.System1.TrmMsgBox(gSysPrm, MSG_CUT_POS_CORRECT_015, vbOKOnly, FRM_CUT_POS_CORRECT_TITLE)
            Call Form1.System1.TrmMsgBox(gSysPrm, MSG_CUT_POS_CORRECT_015, vbOKOnly, Me.frmTitle.Text)
            If (Idx = 0) Then
                txtBlockNoX.Focus()
            Else
                txtBlockNoY.Focus()
            End If
            Return (-1)

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "FrmCutPosCorrect.InputBlkNum() TRAP ERROR = " + ex.Message
            GoTo STP_ERR
        End Try
    End Function
#End Region

#Region "ステップインターバル(XまたはY方向)を求める"
    '''=========================================================================
    ''' <summary>ステップインターバル(XまたはY方向)を求める</summary>
    ''' <param name="stPLT"> (INP)プレートデータ</param>
    ''' <param name="stSTP"> (INP)ステップデータ</param>
    ''' <param name="BlkNum">(INP)ブロック番号XorY</param>
    ''' <param name="Intval">(OUT)ステップインターバル</param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Function CalcStepIntervel(ByRef stPLT As PlateInfo, ByRef stSTP() As StepInfo, ByVal BlkNum As Integer, ByRef Intval As Double) As Double

        Dim intStpMax As Integer
        Dim intBMax As Integer
        Dim IDX As Integer
        Dim strMSG As String

        Try
            ' グループ数(ステップ数)取得
            If (stPLT.intResistDir = 0) Then                                    ' 抵抗並び方向 = X方向
                intStpMax = stPLT.intGroupCntInBlockXBp                         ' ブロック内BPグループ数(X方向）
            Else
                intStpMax = stPLT.intGroupCntInBlockYStage                      ' ブロック内Stageグループ数(Y方向）
            End If

            ' ステップインターバル(XまたはY方向)を求める
            Intval = 0
            intBMax = 0
            For IDX = 1 To intStpMax - 1
                intBMax = intBMax + stSTP(IDX).intSP2                           ' ブロック数
                If (BlkNum <= intBMax) Then
                    Exit For
                End If
                Intval = Intval + stSTP(IDX).dblSP3                             ' ステップ間インターバル
            Next IDX
            Return (cFRS_NORMAL)

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "FrmCutPosCorrect.CalcStepIntervel() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "指定ブロックの抵抗先頭ティーチング位置へテーブルを移動する"
    '''=========================================================================
    '''<summary>指定ブロックの抵抗先頭ティーチング位置へテーブルを移動する</summary>
    '''<param name="intCamera">(INP)ｶﾒﾗ種類(0:内部ｶﾒﾗ 1:外部ｶﾒﾗ)</param>
    '''<param name="iXPlate">(INP)XPlateNo</param> 
    '''<param name="iYPlate">(INP)YPlateNo</param>  
    '''<param name="iXBlock">(INP)XBlockNo</param> 
    '''<param name="iYBlock">(INP)YBlockNo</param> 
    '''<param name="iRegNo"> (INP)抵抗番号</param>   
    '''<remarks>BP Offset と ｶｯﾄ位置補正ﾃｰﾌﾞﾙｵﾌｾｯﾄを考慮</remarks>
    '''=========================================================================
    Private Function XYTableMoveBlockFirst(ByRef intCamera As Short, ByRef iXPlate As Short, ByRef iYPlate As Short, ByRef iXBlock As Short, ByRef iYBlock As Short, ByRef iRegNo As Short) As Integer

        Dim r As Integer
        Dim dblX As Double
        Dim dblY As Double
        Dim dblRotX As Double
        Dim dblRotY As Double
        Dim dblPSX As Double
        Dim dblPSY As Double
        Dim dblBsoX As Double
        Dim dblBsoY As Double
        Dim dblBSX As Double
        Dim dblBSY As Double
        Dim intCDir As Short
        Dim dblTrimPosX As Double
        Dim dblTrimPosY As Double
        Dim dblTOffsX As Double
        Dim dblTOffsY As Double
        Dim dblStepInterval As Double
        Dim dblExtTablePosOffsX As Double
        Dim dblExtTablePosOffsY As Double
        Dim Del_x As Double
        Dim Del_y As Double
        Dim dblBpOffsetX As Double
        Dim dblBpOffsetY As Double
        Dim dblCutPosCorrectOffsetX As Double
        Dim dblCutPosCorrectOffsetY As Double
        Dim strMSG As String

        Try
            dblRotX = 0
            dblRotY = 0

            ' ﾄﾘﾑﾎﾟｼﾞｼｮﾝX,Y取得
            dblTrimPosX = gSysPrm.stDEV.gfTrimX
            dblTrimPosY = gSysPrm.stDEV.gfTrimY

            ' ﾃｰﾌﾞﾙ位置ｵﾌｾｯﾄX,Yの取得
            dblTOffsX = typPlateInfo.dblTableOffsetXDir
            dblTOffsY = typPlateInfo.dblTableOffsetYDir

            Call CalcBlockSize(dblBSX, dblBSY) ' ﾌﾞﾛｯｸｻｲｽﾞ算出

            ' BP位置ｵﾌｾｯﾄX,Y設定
            dblBpOffsetX = typPlateInfo.dblBpOffSetXDir
            dblBpOffsetY = typPlateInfo.dblBpOffSetYDir
            ' ｶｯﾄ位置補正ｵﾌｾｯﾄX,Y
            dblCutPosCorrectOffsetX = typPlateInfo.dblCutPosiReviseOffsetXDir
            dblCutPosCorrectOffsetY = typPlateInfo.dblCutPosiReviseOffsetYDir

            ' ﾌﾞﾛｯｸｻｲｽﾞｵﾌｾｯﾄ算出(ﾌﾞﾛｯｸｻｲｽﾞ/2 ﾌﾞﾛｯｸの象限はXYともに1 ﾃｰﾌﾞﾙの象限も1)
            dblBsoX = (dblBSX / 2) * 1 * 1                  ' Table.BDirX * Table.dir
            dblBsoY = (dblBSY / 2) * 1                      ' Table.BDirY;

            ' θ補正ﾄﾘﾑｵﾌｾｯﾄX,Y
            Del_x = gfCorrectPosX
            Del_y = gfCorrectPosY

            ' giBpDirXy 座標系の設定(ｼｽﾃﾑ設定)
            ' 0:XY NOM(右上)  1:X REV(左上)  2:Y REV(右下)  3:XY REV(左下)
            ' ﾄﾘﾐﾝｸﾞ位置座標 (+or-) 回転半径 + ﾃｰﾌﾞﾙｵﾌｾｯﾄ (+or-) ﾌﾞﾛｯｸｻｲｽﾞｵﾌｾｯﾄ + ﾃｰﾌﾞﾙ補正量
            Select Case gSysPrm.stDEV.giBpDirXy
                Case 0 ' x←, y↓
                    dblX = dblTrimPosX + dblRotX + dblTOffsX + dblBsoX + Del_x
                    dblY = dblTrimPosY + dblRotY + dblTOffsY + dblBsoY + Del_y

                Case 1 ' x→, y↓
                    dblX = dblTrimPosX - dblRotX + dblTOffsX - dblBsoX + Del_x
                    dblY = dblTrimPosY + dblRotY + dblTOffsY + dblBsoY + Del_y

                Case 2 ' x←, y↑
                    dblX = dblTrimPosX + dblRotX + dblTOffsX + dblBsoX + Del_x
                    dblY = dblTrimPosY - dblRotY + dblTOffsY - dblBsoY + Del_y

                Case 3 ' x→, y↑
                    dblX = dblTrimPosX - dblRotX + dblTOffsX - dblBsoX + Del_x
                    dblY = dblTrimPosY - dblRotY + dblTOffsY - dblBsoY + Del_y
            End Select

            If (1 = intCamera) Then                         ' 外部ｶﾒﾗ位置加算 ?
                dblExtTablePosOffsX = gSysPrm.stDEV.gfExCmX ' Externla Camera Offset X(mm)
                dblExtTablePosOffsY = gSysPrm.stDEV.gfExCmY ' Externla Camera Offset Y(mm)
                dblX = dblX + dblExtTablePosOffsX
                dblY = dblY + dblExtTablePosOffsY
            End If

            ' ｽﾃｯﾌﾟ間隔の算出
            intCDir = typPlateInfo.intResistDir               ' チップ並び方向取得(CHIP-NETのみ)
            If intCDir = 0 Then ' X方向
                dblStepInterval = CalcStepInterval(iYBlock) ' ｽﾃｯﾌﾟｲﾝﾀｰﾊﾞﾙ算出(Y軸)
                If gSysPrm.stDEV.giBpDirXy = 0 Or gSysPrm.stDEV.giBpDirXy = 1 Then ' ﾃｰﾌﾞﾙY方向反転なし
                    dblY = dblY + dblStepInterval
                Else ' ﾃｰﾌﾞﾙY方向反転
                    dblY = dblY - dblStepInterval
                End If
            Else ' Y方向
                dblStepInterval = CalcStepInterval(iXBlock) ' ｽﾃｯﾌﾟｲﾝﾀｰﾊﾞﾙ算出(X軸)
                If gSysPrm.stDEV.giBpDirXy = 0 Or gSysPrm.stDEV.giBpDirXy = 2 Then ' ﾃｰﾌﾞﾙX方向反転なし
                    dblX = dblX + dblStepInterval
                Else ' ﾃｰﾌﾞﾙX方向反転
                    dblX = dblX - dblStepInterval
                End If
            End If

            ' ﾌﾟﾚｰﾄ/ﾌﾞﾛｯｸ位置の相対座標計算
            dblPSX = 0.0 : dblPSY = 0.0                         ' ﾌﾟﾚｰﾄｻｲｽﾞ取得(0固定)

            Select Case gSysPrm.stDEV.giBpDirXy
                Case 0 ' x←, y↓
                    dblX = dblX + ((dblPSX * CInt(iXPlate - 1)) + (dblBSX * CInt(iXBlock - 1)))
                    dblX = dblX + pfStartPos(0, iRegNo) + dblBpOffsetX + dblCutPosCorrectOffsetX - (dblBSX / 2)
                    dblY = dblY + ((dblPSY * CInt(iYPlate - 1)) + (dblBSY * CInt(iYBlock - 1)))
                    dblY = dblY + pfStartPos(1, iRegNo) + dblBpOffsetY + dblCutPosCorrectOffsetY - (dblBSY / 2)

                Case 1 ' x→, y↓
                    dblX = dblX - ((dblPSX * CInt(iXPlate - 1)) + (dblBSX * CInt(iXBlock - 1)))
                    dblX = dblX - pfStartPos(0, iRegNo) - dblBpOffsetX - dblCutPosCorrectOffsetX + (dblBSX / 2)
                    dblY = dblY + ((dblPSY * CInt(iYPlate - 1)) + (dblBSY * CInt(iYBlock - 1)))
                    dblY = dblY + pfStartPos(1, iRegNo) + dblBpOffsetY + dblCutPosCorrectOffsetY - (dblBSY / 2)

                Case 2 ' x←, y↑
                    dblX = dblX + ((dblPSX * CInt(iXPlate - 1)) + (dblBSX * CInt(iXBlock - 1)))
                    dblX = dblX + pfStartPos(0, iRegNo) + dblBpOffsetX + dblCutPosCorrectOffsetX - (dblBSX / 2)
                    dblY = dblY - ((dblPSY * CInt(iYPlate - 1)) + (dblBSY * CInt(iYBlock - 1)))
                    dblY = dblY - pfStartPos(1, iRegNo) + dblBpOffsetY + dblCutPosCorrectOffsetY + (dblBSY / 2)

                Case 3 ' x→, y↑
                    dblX = dblX - ((dblPSX * CInt(iXPlate - 1)) + (dblBSX * CInt(iXBlock - 1)))
                    dblX = dblX - pfStartPos(0, iRegNo) + dblBpOffsetX + dblCutPosCorrectOffsetX + (dblBSX / 2)
                    dblY = dblY - ((dblPSY * CInt(iYPlate - 1)) + (dblBSY * CInt(iYBlock - 1)))
                    dblY = dblY - pfStartPos(1, iRegNo) + dblBpOffsetY + dblCutPosCorrectOffsetY + (dblBSY / 2)
            End Select

            ' 指定ﾌﾟﾚｰﾄ/ﾌﾞﾛｯｸ位置に移動
            r = Form1.System1.XYtableMove(gSysPrm, dblX, dblY)
            Return (r)

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "FrmCutPosCorrect.XYTableMoveBlockFirst() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return値 = 例外エラー
        End Try
    End Function
#End Region

    '========================================================================================
    '   グリッド処理
    '========================================================================================
#Region "スタートポイントグリッドの初期化"
    '''=========================================================================
    '''<summary>スタートポイントグリッドの初期化</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub GridInitialize()

        Dim strHead(6) As String
        Dim i As Short
        Dim intChipNum As Short
        Dim strMSG As String

        Try

            'Call GetChipNum(intChipNum)                                 ' ｸﾞﾙｰﾌﾟ内抵抗数
            intChipNum = typPlateInfo.intResistCntInBlock               ' ブロック内抵抗数 
            strHead(0) = ""
            strHead(1) = "R No"
            strHead(2) = FrmCutPosCorrect_001
            strHead(3) = FrmCutPosCorrect_002
            strHead(4) = FrmCutPosCorrect_003
            strHead(5) = FrmCutPosCorrect_004
            'If (0 = gSysPrm.stTMN.giMsgTyp) Then
            '    strHead(2) = "ﾃｨｰﾁﾝｸﾞX"
            '    strHead(3) = "ﾃｨｰﾁﾝｸﾞY"
            '    strHead(4) = "ずれ量X "
            '    strHead(5) = "ずれ量Y "
            '    'Me.cmdBlockMove.Text = "ブロック移動"
            'Else
            '    strHead(2) = "Teaching X"
            '    strHead(3) = "Teaching Y"
            '    strHead(4) = " Gap X  "
            '    strHead(5) = " Gap Y  "
            '    'Me.cmdBlockMove.Text = "Block Move"
            'End If

            'For i = 0 To strHead.Length - 1 Step 1
            For i = 0 To 4 Step 1
                dgvPoints.Columns(i).HeaderText = strHead(i + 1)    ' V4.0.0.0⑧
            Next i

            For i = 1 To 5                                              ' 見出し作成
                strHead(0) = strHead(0) & strHead(i)
                If (i <> 5) Then
                    strHead(0) = strHead(0) & "|"
                End If
            Next

            'UPGRADE_ISSUE: 定数 vbTwips はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
            'UPGRADE_ISSUE: Form プロパティ CutPosCorrect.ScaleMode はサポートされません。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="8027179A-CB3B-45C0-9863-FAA1AF983B59"' をクリックしてください。
            'Me.ScaleMode = vbTwips
            'V4.7.0.0⑩ コメントアウト開始↓
            'With Me.fgrdPoints
            '    .FormatString = strHead(0)

            '    .Rows = intChipNum + 1                                  ' ｸﾞﾘｯﾄﾞ行数(+1:ﾀｲﾄﾙ行)
            '    .Row = 1                                                ' ｶｰｿﾙ位置
            '    .Col = 2
            '    'UPGRADE_ISSUE: VBControlExtender メソッド fgrdPoints.FillStyle はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
            '    .FillStyle = MSFlexGridLib.FillStyleSettings.flexFillSingle
            '    For i = 0 To 4                                          ' ﾃﾞｰﾀ部分は右寄せ
            '        .set_ColAlignment(i, MSFlexGridLib.AlignmentSettings.flexAlignRightCenter)
            '    Next

            '    .Row = 0
            '    For i = 0 To 4                                          ' ﾀｲﾄﾙ部分のみ中央よせ
            '        .Col = i
            '        .CellAlignment = MSFlexGridLib.AlignmentSettings.flexAlignCenterCenter
            '    Next

            '    For i = 1 To intChipNum
            '        If (1 <= CInt(iResistorNumber(i))) AndAlso (MARKING_RESNO_SET > CInt(iResistorNumber(i))) Then
            '            .set_TextMatrix(i, 0, CShort(iResistorNumber(i)))               ' 抵抗番号
            '            .set_TextMatrix(i, 1, pfStartPos(0, i).ToString("##0.0000"))    ' ﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄX
            '            .set_TextMatrix(i, 2, pfStartPos(1, i).ToString("##0.0000"))    ' ﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄY
            '            .set_TextMatrix(i, 3, "")                                       ' ずれ量X
            '            .set_TextMatrix(i, 4, "")                                       ' ずれ量Y
            '        End If
            '    Next
            'End With
            'V4.7.0.0⑩ コメントアウト終了↑

            With dgvPoints              ' V4.0.0.0⑧
                .SuspendLayout()
                .Rows.Add(intChipNum)
                For i = 1 To intChipNum Step 1
                    With .Rows(i - 1)
                        If (1 <= CInt(iResistorNumber(i))) AndAlso _
                            (MARKING_RESNO_SET > CInt(iResistorNumber(i))) Then
                            .Cells(0).Value = iResistorNumber(i)                      ' 抵抗番号
                            .Cells(1).Value = pfStartPos(0, i).ToString("##0.0000")   ' ﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄX
                            .Cells(2).Value = pfStartPos(1, i).ToString("##0.0000")   ' ﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄY
                        End If
                    End With
                Next i
                .ResumeLayout()
            End With

            Call Set_TopRow(1)                                          ' グリッドの1行目を先頭にする 

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "FrmCutPosCorrect.GridInitialize() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "スタートポイントグリッドの指定行を先頭にする"
    '''=========================================================================
    '''<summary>ｽﾀｰﾄﾎﾟｲﾝﾄｸﾞﾘｯﾄﾞの指定行を先頭にする</summary>
    '''<param name="iRow">(INP)行番号(1-)</param>
    '''=========================================================================
    Private Sub Set_TopRow(ByVal iRow As Integer)

        Dim strMSG As String

        Try
            'V4.7.0.0⑩ Me.fgrdPoints.TopRow = iRow

            dgvPoints.FirstDisplayedScrollingRowIndex = (iRow - 1) ' V4.0.0.0⑧

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "FrmCutPosCorrect.Set_TopRow() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "スタートポイントグリッドの指定行の背景を黄色にする"
    '''=========================================================================
    '''<summary>スタートポイントグリッドの指定行の背景を黄色にする</summary>
    '''<param name="iRow">(INP)行番号(1-)</param>
    '''=========================================================================
    Private Sub Set_BackColor(ByVal iRow As Integer)

        Dim strMSG As String

        Try
            'V4.7.0.0⑩ コメントアウト開始↓
            'With Me.fgrdPoints
            '    ' 背景色を黄色に設定する
            '    .Row = iRow                                                 ' Row = 指定行 
            '    .Col = 1 : .CellBackColor = System.Drawing.Color.Yellow
            '    .Col = 2 : .CellBackColor = System.Drawing.Color.Yellow
            '    .Col = 3 : .CellBackColor = System.Drawing.Color.Yellow
            '    .Col = 4 : .CellBackColor = System.Drawing.Color.Yellow
            'End With
            'Me.fgrdPoints.Refresh()
            'V4.7.0.0⑩ コメントアウト終了↑

            With dgvPoints
                .SuspendLayout()
                With .Rows(iRow - 1)   ' V4.0.0.0⑧
                    ' 背景色を黄色に設定する
                    .Cells(1).Style.BackColor = Color.Yellow
                    .Cells(2).Style.BackColor = Color.Yellow
                    .Cells(3).Style.BackColor = Color.Yellow
                    .Cells(4).Style.BackColor = Color.Yellow
                End With
                .ResumeLayout()
                .Refresh()
            End With

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "FrmCutPosCorrect.Set_BackColor() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "スタートポイントグリッドの全行の背景色を元に戻す"
    '''=========================================================================
    '''<summary>スタートポイントグリッドの指定行の背景色を元に戻す</summary>
    '''<param name="DatCount">(INP)データ数</param>
    '''=========================================================================
    Private Sub Reset_AllBackColor(ByVal DatCount As Integer)

        'V4.7.0.0⑩        Dim Count As Integer
        Dim strMSG As String

        Try
            'V4.7.0.0⑩ コメントアウト開始↓
            'With Me.fgrdPoints
            '    For Count = 1 To DatCount
            '        .Row = Count                                        ' Row = 指定行
            '        .Col = 1 : .CellBackColor = System.Drawing.Color.White
            '        .Col = 2 : .CellBackColor = System.Drawing.Color.White
            '        .Col = 3 : .CellBackColor = System.Drawing.Color.White
            '        .Col = 4 : .CellBackColor = System.Drawing.Color.White
            '    Next Count
            'End With
            'V4.7.0.0⑩ コメントアウト終了↑

            With dgvPoints              ' V4.0.0.0⑧
                .SuspendLayout()
                For i As Integer = 0 To (.Rows.Count - 1) Step 1
                    .Rows(i).Cells(1).Style.BackColor = SystemColors.Window
                    .Rows(i).Cells(2).Style.BackColor = SystemColors.Window
                    .Rows(i).Cells(3).Style.BackColor = SystemColors.Window
                    .Rows(i).Cells(4).Style.BackColor = SystemColors.Window
                Next i
                .ResumeLayout()
                .Refresh()
            End With

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "FrmCutPosCorrect.Reset_AllBackColor() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "スタートポイントグリッドの指定行の背景色を元に戻す"
    '''=========================================================================
    '''<summary>スタートポイントグリッドの指定行の背景色を元に戻す</summary>
    '''<param name="iRow">(INP)行番号(1-)</param>
    '''=========================================================================
    Private Sub Reset_BackColor(ByVal iRow As Integer)

        Dim strMSG As String

        Try
            'V4.7.0.0⑩ コメントアウト開始↓
            'With Me.fgrdPoints
            '    .Row = iRow                                             ' Row = 指定行
            '    .Col = 1 : .CellBackColor = System.Drawing.Color.White
            '    .Col = 2 : .CellBackColor = System.Drawing.Color.White
            '    .Col = 3 : .CellBackColor = System.Drawing.Color.White
            '    .Col = 4 : .CellBackColor = System.Drawing.Color.White
            'End With
            'Me.fgrdPoints.Refresh()
            'V4.7.0.0⑩ コメントアウト終了↑

            With dgvPoints              ' V4.0.0.0⑧
                .SuspendLayout()
                With .Rows(iRow - 1)
                    .Cells(1).Style.BackColor = SystemColors.Window
                    .Cells(2).Style.BackColor = SystemColors.Window
                    .Cells(3).Style.BackColor = SystemColors.Window
                    .Cells(4).Style.BackColor = SystemColors.Window
                End With
                .ResumeLayout()
                .Refresh()
            End With

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "FrmCutPosCorrect.Reset_BackColor() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "スタートポイントグリッドのずれ量X,Y表示を初期化する"
    '''=========================================================================
    '''<summary>スタートポイントグリッドのずれ量X,Y表示を初期化する</summary>
    '''<param name="num">(INP)対象データ数(1-)</param>
    '''=========================================================================
    Private Sub DispGapXYInit(ByVal num As Integer)

        'V4.7.0.0⑩        Dim Idx As Integer
        Dim strMSG As String

        Try
            'V4.7.0.0⑩ コメントアウト開始↓
            'With Me.fgrdPoints
            '    For Idx = 1 To num
            '        .set_TextMatrix(Idx, 3, "")                         ' ずれ量X
            '        .set_TextMatrix(Idx, 4, "")                         ' ずれ量Y
            '    Next Idx
            'End With
            'Me.fgrdPoints.Refresh()
            'V4.7.0.0⑩ コメントアウト終了↑

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "FrmCutPosCorrect.DispGapXYInit() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "スタートポイントグリッドの指定行にずれ量X,Yを表示する"
    '''=========================================================================
    '''<summary>スタートポイントグリッドの指定行にずれ量X,Yを表示する</summary>
    '''<param name="row">(INP)行番号(1-)</param>
    ''' <param name="X"> (INP)ずれ量X</param>
    ''' <param name="Y"> (INP)ずれ量Y</param>
    '''=========================================================================
    Private Sub DispGapXY(ByVal row As Integer, ByVal X As Double, ByVal Y As Double)

        Dim strMSG As String

        Try
            'V4.7.0.0⑩ コメントアウト開始↓
            'With Me.fgrdPoints
            '    .set_TextMatrix(row, 3, X.ToString("##0.0000"))                     ' ずれ量X
            '    .set_TextMatrix(row, 4, Y.ToString("##0.0000"))                     ' ずれ量Y
            'End With
            'Me.fgrdPoints.Refresh()
            'V4.7.0.0⑩ コメントアウト終了↑

            With dgvPoints.Rows(row - 1)
                .Cells(3).Value = X.ToString("##0.0000")                ' ずれ量X
                .Cells(4).Value = Y.ToString("##0.0000")                ' ずれ量Y
            End With
            dgvPoints.Refresh()

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "FrmCutPosCorrect.DispGapXY() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "DataGridViewが選択状態の色になるのをキャンセルする"
    '''=========================================================================
    ''' <summary>DataGridViewが選択状態の色になるのをキャンセルする</summary>
    '''=========================================================================
    Private Sub dgvPoints_SelectionChanged(sender As System.Object, e As System.EventArgs) Handles dgvPoints.SelectionChanged
        dgvPoints.Rows(dgvPoints.CurrentRow.Index).Selected = False
    End Sub
#End Region

    '========================================================================================
    '   ボタン押下時処理(ＪＯＧ操作画面)
    '========================================================================================
#Region "HALTボタン押下時処理"
    '''=========================================================================
    '''<summary>HALTボタン押下時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnHALT_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnHALT.Click
        Call SubBtnHALT_Click()
    End Sub
#End Region

#Region "STARTボタン押下時処理"
    '''=========================================================================
    '''<summary>STARTボタン押下時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnSTART_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSTART.Click
        Call SubBtnSTART_Click()
    End Sub
#End Region

#Region "RESETボタン押下時処理"
    '''=========================================================================
    '''<summary>RESETボタン押下時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnRESET_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnRESET.Click
        Call SubBtnRESET_Click()
    End Sub
#End Region

#Region "Zボタン押下時処理"
    '''=========================================================================
    '''<summary>RESETボタン押下時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnZ_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnZ.Click
        Call SubBtnZ_Click(stJOG)
    End Sub
#End Region

#Region "HIボタン押下時処理"
    '''=========================================================================
    '''<summary>HIボタン押下時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnHI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnHI.Click
        Call SubBtnHI_Click(stJOG)
    End Sub
#End Region

#Region "矢印ボタン押下時"
    '''=========================================================================
    '''<summary>矢印ボタン押下時</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub BtnJOG_0_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_0.MouseDown
        Call SubBtnJOG_0_MouseDown(stJOG) 'V6.0.0.0-22                                    ' +Y ON
    End Sub
    Private Sub BtnJOG_0_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_0.MouseUp
        Call SubBtnJOG_0_MouseUp(stJOG) 'V6.0.0.0-22                                      ' +Y OFF
    End Sub

    Private Sub BtnJOG_1_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_1.MouseDown
        Call SubBtnJOG_1_MouseDown(stJOG) 'V6.0.0.0-22                                    ' -Y ON
    End Sub
    Private Sub BtnJOG_1_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_1.MouseUp
        Call SubBtnJOG_1_MouseUp(stJOG) 'V6.0.0.0-22                                      ' -Y OFF
    End Sub

    Private Sub BtnJOG_2_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_2.MouseDown
        Call SubBtnJOG_2_MouseDown(stJOG) 'V6.0.0.0-22                                    ' +X ON
    End Sub
    Private Sub BtnJOG_2_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_2.MouseUp
        Call SubBtnJOG_2_MouseUp(stJOG) 'V6.0.0.0-22                                      ' +X OFF
    End Sub

    Private Sub BtnJOG_3_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_3.MouseDown
        Call SubBtnJOG_3_MouseDown(stJOG) 'V6.0.0.0-22                                    ' -X ON
    End Sub
    Private Sub BtnJOG_3_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_3.MouseUp
        Call SubBtnJOG_3_MouseUp(stJOG) 'V6.0.0.0-22                                      ' -X OFF
    End Sub

    Private Sub BtnJOG_4_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_4.MouseDown
        Call SubBtnJOG_4_MouseDown(stJOG) 'V6.0.0.0-22                                    ' -X -Y ON
    End Sub
    Private Sub BtnJOG_4_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_4.MouseUp
        Call SubBtnJOG_4_MouseUp(stJOG) 'V6.0.0.0-22                                      ' -X -Y OFF
    End Sub

    Private Sub BtnJOG_5_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_5.MouseDown
        Call SubBtnJOG_5_MouseDown(stJOG) 'V6.0.0.0-22                                    ' +X -Y ON
    End Sub
    Private Sub BtnJOG_5_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_5.MouseUp
        Call SubBtnJOG_5_MouseUp(stJOG) 'V6.0.0.0-22                                      ' +X -Y OFF
    End Sub

    Private Sub BtnJOG_6_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_6.MouseDown
        Call SubBtnJOG_6_MouseDown(stJOG) 'V6.0.0.0-22                                    ' +X +Y ON
    End Sub
    Private Sub BtnJOG_6_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_6.MouseUp
        Call SubBtnJOG_6_MouseUp(stJOG) 'V6.0.0.0-22                                      ' +X +Y OFF
    End Sub

    Private Sub BtnJOG_7_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_7.MouseDown
        Call SubBtnJOG_7_MouseDown(stJOG) 'V6.0.0.0-22                                    ' -X +Y ON
    End Sub
    Private Sub BtnJOG_7_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_7.MouseUp
        Call SubBtnJOG_7_MouseUp(stJOG) 'V6.0.0.0-22                                      ' -X +Y OFF
    End Sub
#End Region

    '========================================================================================
    '   テンキー入力処理(ＪＯＧ操作画面)
    '========================================================================================
#Region "キーダウン時処理"
    '''=========================================================================
    '''<summary>キーダウン時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub FrmCutPosCorrect_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        JogKeyDown(e)                   'V6.0.0.0⑪
    End Sub

    Public Sub JogKeyDown(ByVal e As KeyEventArgs) Implements ICommonMethods.JogKeyDown    'V6.0.0.0⑪
        'V6.0.0.0⑪        Dim KeyCode As Short = e.KeyCode
        Dim KeyCode As Keys = e.KeyCode                 'V6.0.0.0⑪
        Console.WriteLine("FrmCutPosCorrect.FrmCutPosCorrect_KeyDown()")

        ' テンキーダウンならInpKeyにテンキーコードを設定する
        'V6.0.0.0⑫        Call Sub_10KeyDown(KeyCode)
        Sub_10KeyDown(KeyCode, stJOG)             'V6.0.0.0⑫
        If (KeyCode = System.Windows.Forms.Keys.NumPad5) Then           ' 5ｷｰ (KeyCode = 101(&H65)
            'V6.0.0.0⑪            Call BtnHI_Click(sender, e)                                 ' HIボタン ON/OFF
            BtnHI_Click(BtnHI, e)                                 ' HIボタン ON/OFF    'V6.0.0.0⑪
        End If
        'V6.0.0.0⑪        Call Me.Focus()

    End Sub
#End Region

#Region "キーアップ時処理"
    '''=========================================================================
    '''<summary>キーアップ時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub FrmCutPosCorrect_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        JogKeyUp(e)                     'V6.0.0.0⑪
    End Sub

    Public Sub JogKeyUp(ByVal e As KeyEventArgs) Implements ICommonMethods.JogKeyUp        'V6.0.0.0⑪
        'V6.0.0.0⑪        Dim KeyCode As Short = e.KeyCode
        Dim KeyCode As Keys = e.KeyCode                 'V6.0.0.0⑪
        Console.WriteLine("FrmCutPosCorrect.FrmCutPosCorrect_KeyUp()")
        ' テンキーアップならInpKeyのテンキーコードをOFFする
        'V6.0.0.0⑫        Call Sub_10KeyUp(KeyCode)
        Sub_10KeyUp(KeyCode, stJOG)                   'V6.0.0.0⑫
        'V6.0.0.0⑫        Call Me.Focus()

    End Sub

#End Region

#Region "カメラ画像クリック位置を画像センターに移動する処理"
    ''' <summary>カメラ画像クリック位置を画像センターに移動する処理</summary>
    ''' <param name="distanceX">画像センターからの距離X</param>
    ''' <param name="distanceY">画像センターからの距離Y</param>
    ''' <remarks>'V6.0.0.0⑪</remarks>
    Public Sub MoveToCenter(ByVal distanceX As Decimal, ByVal distanceY As Decimal) _
        Implements ICommonMethods.MoveToCenter

        Globals_Renamed.MoveToCenter(distanceX, distanceY, stJOG)
    End Sub
#End Region

    '========================================================================================
    '   トラックバー処理(ＪＯＧ操作画面)
    '========================================================================================
#Region "トラックバーのスライダー移動イベント"
    '''=========================================================================
    ''' <summary>トラックバーのスライダー移動イベント</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub TBarLowPitch_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TBarLowPitch.Scroll
        Call SetSliderPitch(IDX_PIT, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
    End Sub

    Private Sub TBarHiPitch_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TBarHiPitch.Scroll
        Call SetSliderPitch(IDX_HPT, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
    End Sub

    Private Sub TBarPause_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TBarPause.Scroll
        Call SetSliderPitch(IDX_PAU, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
    End Sub
#End Region


    '--- これ以下は削除

    '#Region "ﾌﾞﾛｯｸ移動ﾎﾞﾀﾝ押下時処理"
    '    '''=========================================================================
    '    '''<summary>ﾌﾞﾛｯｸ移動ﾎﾞﾀﾝ押下時処理</summary>
    '    '''<remarks></remarks>
    '    '''=========================================================================
    '    Private Sub cmdBlockMove_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs)

    '        Dim iXBlock As Short
    '        Dim iYBlock As Short
    '        Dim iRetc As Short
    '        Dim r As Short

    '        Me.cmdBlockMove.Enabled = False
    '        Me.cmdExit.Enabled = False
    '        iXBlock = CShort(Me.txtBlockNoX.Text)
    '        iYBlock = CShort(Me.txtBlockNoY.Text)

    '        ' ﾌﾞﾛｯｸはX,Yともに１～９９
    '        If ((1 <= iXBlock) And (99 >= iXBlock)) And ((1 <= iYBlock) And (99 >= iYBlock)) Then
    '            iRetc = 1
    '            If (1 = iRetc) Then ' STARTなら次処理

    '                iRetc = 0

    '                'gbfReset_flg = 1 '(START要求の有無)

    '                'θ軸補正実行
    '                r = CorrectTheta()
    '                If (r = cFRS_ERR_RST) Then
    '                    iRetc = 0
    '                End If

    '                If gbRotCorrectCancel = 0 Then
    '                    iRetc = 1 'ok
    '                Else
    '                    'errore!
    '                    iRetc = 0 'error
    '                End If
    '#If VIDEO_CAPTURE = 0 Then
    '                Call Form1.VideoLibrary1.ChangeCamera(INTERNAL_CAMERA)                         ' カメラ切替(内部ｶﾒﾗ)
    '#End If

    '                If (1 = iRetc) Then
    '                    pfCurrentNo = 1 ' 処理抵抗番号
    '                    Call Set_BackColor(pfCurrentNo) ' 先頭行

    '                    iRetc = XYTableMoveBlock(0, 1, 1, iXBlock, iYBlock) ' 指定ﾌﾞﾛｯｸの先頭抵抗先頭ﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄへ移動

    '                    If (iRetc) Then
    '                        'iRetc = CutPosCorrectCrossExec() ' ｶｯﾄ位置補正実行(十字ｶｯﾄ)
    '                        If (1 = iRetc) Then
    '                            'iRetc = CutPosCorrectExec(iXBlock, iYBlock) ' ｶｯﾄ位置補正実行(画像認識)
    '                        End If
    '                    End If
    '                End If
    '            End If
    '            If (1 = iRetc) Or (0 = iRetc) Then ' 正常終了orCancelは原点復帰
    '                r = sResetTrim() ' 原点復帰(すぐに移動)
    '            End If

    '            ''''        'ﾊﾞｷｭｰﾑ/ｸﾗﾝﾌﾟ制御(OFF)
    '            ''''        Call Form1.VacClampCtrl(False)

    '            If (1 = iRetc) Then ' 正常終了
    '                Me.cmdBlockMove.Enabled = True
    '                Me.cmdExit.Enabled = True
    '                Me.Close()
    '            ElseIf (0 = iRetc) Then  ' RESETはｷｬﾝｾﾙ
    '                'Call Form1.System1.sLampOnOff(LAMP_RESET, True)
    '                'Call Form1.System1.sLampOnOff(LAMP_RESET, False)
    '                Call LAMP_CTRL(LAMP_RESET, True)
    '                Call LAMP_CTRL(LAMP_RESET, False)
    '                Me.cmdBlockMove.Enabled = True
    '                Me.cmdExit.Enabled = True
    '                Me.Close()
    '            ElseIf (-1 = iRetc) Then  ' 非常停止はﾌｫｰﾑ消去
    '                Me.cmdBlockMove.Enabled = True
    '                Me.cmdExit.Enabled = True
    '                Me.Close()
    '            End If
    '        Else
    '            MsgBox(MSG_CUT_POS_CORRECT_001, MsgBoxStyle.OkOnly, "Attention")
    '            Me.cmdBlockMove.Enabled = True
    '            Me.cmdExit.Enabled = True
    '        End If
    '    End Sub
    '#End Region

    '#Region "指定ﾌﾞﾛｯｸの中央へ移動"
    '    '''=========================================================================
    '    '''<summary>指定ﾌﾞﾛｯｸの中央へ移動</summary>
    '    '''<param name="intCamera">(INP)ｶﾒﾗ種類(0:内部ｶﾒﾗ 1:外部ｶﾒﾗ)</param>
    '    '''<param name="iXPlate">(INP)XPlateNo</param> 
    '    '''<param name="iYPlate">(INP)YPlateNo</param>  
    '    '''<param name="iXBlock">(INP)XBlockNo</param> 
    '    '''<param name="iYBlock">(INP)YBlockNo</param>   
    '    '''<remarks>十字ｶｯﾄ位置はﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄからﾌﾟﾚｰﾄﾃﾞｰﾀ
    '    '''         のPP47の値分ずれたところが中心となる
    '    '''         現状はﾌﾟﾚｰﾄを指定しても意味なし</remarks>
    '    '''=========================================================================
    '    Private Function XYTableMoveBlock(ByRef intCamera As Short, ByRef iXPlate As Short, ByRef iYPlate As Short, ByRef iXBlock As Short, ByRef iYBlock As Short) As Boolean

    '        Dim bRetc As Boolean
    '        Dim dblX As Double
    '        Dim dblY As Double
    '        Dim dblRotX As Double
    '        Dim dblRotY As Double
    '        Dim dblPSX As Double
    '        Dim dblPSY As Double
    '        Dim dblBsoX As Double
    '        Dim dblBsoY As Double
    '        Dim dblBSX As Double
    '        Dim dblBSY As Double
    '        Dim intTableType As Short
    '        Dim intCDir As Short
    '        Dim dblTrimPosX As Double
    '        Dim dblTrimPosY As Double
    '        Dim dblTOffsX As Double
    '        Dim dblTOffsY As Double
    '        Dim dblStepInterval As Double
    '        Dim dblExtTablePosOffsX As Double
    '        Dim dblExtTablePosOffsY As Double
    '        Dim Del_x As Double
    '        Dim Del_y As Double

    '        bRetc = False
    '        dblRotX = 0
    '        dblRotY = 0

    '        ' ﾄﾘﾑﾎﾟｼﾞｼｮﾝX,Y取得
    '        ''''(2010/11/16) 動作確認後下記コメントは削除
    '        'dblTrimPosX = gStartX
    '        'dblTrimPosY = gStartY
    '        dblTrimPosX = gSysPrm.stDEV.gfTrimX
    '        dblTrimPosY = gSysPrm.stDEV.gfTrimY

    '        ' ﾃｰﾌﾞﾙ位置ｵﾌｾｯﾄの取得
    '        dblTOffsX = typPlateInfo.dblTableOffsetXDir : dblTOffsY = typPlateInfo.dblTableOffsetYDir

    '        Call CalcBlockSize(dblBSX, dblBSY) ' ﾌﾞﾛｯｸｻｲｽﾞ算出
    '        intTableType = gSysPrm.stDEV.giXYtbl

    '        ' ﾌﾞﾛｯｸｻｲｽﾞｵﾌｾｯﾄ算出　ﾌﾞﾛｯｸｻｲｽﾞ/2 ﾌﾞﾛｯｸの象限はXYともに1 ﾃｰﾌﾞﾙの象限も1
    '        dblBsoX = (dblBSX / 2.0#) * 1 * 1 ' Table.BDirX * Table.dir
    '        dblBsoY = (dblBSY / 2) * 1 ' Table.BDirY;
    '        ' θ補正ﾄﾘﾑｵﾌｾｯﾄX,Y
    '        Del_x = gfCorrectPosX
    '        Del_y = gfCorrectPosY

    '        ' giBpDirXy 座標系の設定(ｼｽﾃﾑ設定)
    '        ' 0:XY NOM(右上)  1:X REV(左上)  2:Y REV(右下)  3:XY REV(左下)
    '        ' ﾄﾘﾐﾝｸﾞ位置座標 (+or-) 回転半径 + ﾃｰﾌﾞﾙｵﾌｾｯﾄ (+or-) ﾌﾞﾛｯｸｻｲｽﾞｵﾌｾｯﾄ + ﾃｰﾌﾞﾙ補正量
    '        Select Case gSysPrm.stDEV.giBpDirXy

    '            Case 0 ' x←, y↓
    '                dblX = dblTrimPosX + dblRotX + dblTOffsX + dblBsoX + Del_x
    '                dblY = dblTrimPosY + dblRotY + dblTOffsY + dblBsoY + Del_y

    '            Case 1 ' x→, y↓
    '                dblX = dblTrimPosX - dblRotX + dblTOffsX - dblBsoX + Del_x
    '                dblY = dblTrimPosY + dblRotY + dblTOffsY + dblBsoY + Del_y

    '            Case 2 ' x←, y↑
    '                dblX = dblTrimPosX + dblRotX + dblTOffsX + dblBsoX + Del_x
    '                dblY = dblTrimPosY - dblRotY + dblTOffsY - dblBsoY + Del_y

    '            Case 3 ' x→, y↑
    '                dblX = dblTrimPosX - dblRotX + dblTOffsX - dblBsoX + Del_x
    '                dblY = dblTrimPosY - dblRotY + dblTOffsY - dblBsoY + Del_y

    '        End Select

    '        If (1 = intCamera) Then                         ' 外部ｶﾒﾗ位置加算 ?
    '            dblExtTablePosOffsX = gSysPrm.stDEV.gfExCmX ' Externla Camera Offset X(mm)
    '            dblExtTablePosOffsY = gSysPrm.stDEV.gfExCmY ' Externla Camera Offset Y(mm)
    '            dblX = dblX + dblExtTablePosOffsX
    '            dblY = dblY + dblExtTablePosOffsY
    '        End If

    '        ' ｽﾃｯﾌﾟ間隔の算出
    '        intCDir = typPlateInfo.intResistDir             ' チップ並び方向取得(CHIP-NETのみ)
    '        If intCDir = 0 Then                             ' X方向
    '            dblStepInterval = CalcStepInterval(iYBlock) ' ｽﾃｯﾌﾟｲﾝﾀｰﾊﾞﾙ算出(Y軸)
    '            If gSysPrm.stDEV.giBpDirXy = 0 Or gSysPrm.stDEV.giBpDirXy = 1 Then ' ﾃｰﾌﾞﾙY方向反転なし
    '                dblY = dblY + dblStepInterval
    '            Else                                        ' ﾃｰﾌﾞﾙY方向反転
    '                dblY = dblY - dblStepInterval
    '            End If
    '        Else                                            ' Y方向
    '            dblStepInterval = CalcStepInterval(iXBlock) ' ｽﾃｯﾌﾟｲﾝﾀｰﾊﾞﾙ算出(X軸)
    '            If gSysPrm.stDEV.giBpDirXy = 0 Or gSysPrm.stDEV.giBpDirXy = 2 Then ' ﾃｰﾌﾞﾙX方向反転なし
    '                dblX = dblX + dblStepInterval
    '            Else ' ﾃｰﾌﾞﾙX方向反転
    '                dblX = dblX - dblStepInterval
    '            End If
    '        End If

    '        'ﾌﾟﾚｰﾄ/ﾌﾞﾛｯｸ位置の相対座標計算
    '        dblPSX = 0.0 : dblPSY = 0.0                         ' ﾌﾟﾚｰﾄｻｲｽﾞ取得(0固定)

    '        Select Case gSysPrm.stDEV.giBpDirXy

    '            Case 0 ' x←, y↓
    '                dblX = dblX + ((dblPSX * CInt(iXPlate - 1)) + (dblBSX * CInt(iXBlock - 1)))
    '                dblY = dblY + ((dblPSY * CInt(iYPlate - 1)) + (dblBSY * CInt(iYBlock - 1)))

    '            Case 1 ' x→, y↓
    '                dblX = dblX - ((dblPSX * CInt(iXPlate - 1)) + (dblBSX * CInt(iXBlock - 1)))
    '                dblY = dblY + ((dblPSY * CInt(iYPlate - 1)) + (dblBSY * CInt(iYBlock - 1)))

    '            Case 2 ' x←, y↑
    '                dblX = dblX + ((dblPSX * CInt(iXPlate - 1)) + (dblBSX * CInt(iXBlock - 1)))
    '                dblY = dblY - ((dblPSY * CInt(iYPlate - 1)) + (dblBSY * CInt(iYBlock - 1)))

    '            Case 3 ' x→, y↑
    '                dblX = dblX - ((dblPSX * CInt(iXPlate - 1)) + (dblBSX * CInt(iXBlock - 1)))
    '                dblY = dblY - ((dblPSY * CInt(iYPlate - 1)) + (dblBSY * CInt(iYBlock - 1)))

    '        End Select

    '        '指定ﾌﾟﾚｰﾄ/ﾌﾞﾛｯｸ位置に移動
    '        If (0 = Form1.System1.XYtableMove(gSysPrm, dblX, dblY)) Then
    '            bRetc = True
    '        Else
    '            bRetc = False
    '        End If

    '        XYTableMoveBlock = bRetc

    '    End Function
    '#End Region

    '#Region "指定ﾌﾞﾛｯｸの抵抗全てに十字ｶｯﾄを行う"
    '    '''=========================================================================
    '    '''<summary>指定ﾌﾞﾛｯｸの抵抗全てに十字ｶｯﾄを行う</summary>
    '    '''<returns> </returns>
    '    '''=========================================================================
    '    Private Function CutPosCorrectCrossExec() As Short

    '        Dim iRetc As Short
    '        Dim r As Short
    '        Dim X As Double
    '        Dim y As Double
    '        Dim intChipNum As Short
    '        Dim bFgLoop As Boolean
    '        Dim dblCutPosCorrectOffsetX As Double
    '        Dim dblCutPosCorrectOffsetY As Double
    '        Dim errChk As Integer

    '        Call GetChipNum(intChipNum) ' ｸﾞﾙｰﾌﾟ内抵抗数
    '        ' ｶｯﾄ位置補正ｵﾌｾｯﾄX,Y
    '        dblCutPosCorrectOffsetX = typPlateInfo.dblCutPosiReviseOffsetXDir
    '        dblCutPosCorrectOffsetY = typPlateInfo.dblCutPosiReviseOffsetYDir

    '        Call ZCONRST() ' ラッチ解除
    '        Call BpMoveOrigin_Ex()
    '        '    Call BpMoveOrigin                               ' BPをﾌﾞﾛｯｸ右上に移動(原点設定)
    '        '    Call BpMove(0, 0, 1)
    '        '    Call Form1.System1.EX_MOVE(gSysPrm, 0, 0, 1)
    '        ' 先頭座標取得
    '        X = pfStartPos(0, pfCurrentNo) + dblCutPosCorrectOffsetX
    '        y = pfStartPos(1, pfCurrentNo) + dblCutPosCorrectOffsetY
    '        '    Call BpMove(x, y, 1)                            ' 最初のｶｯﾄ位置中央へ移動
    '        Call Form1.System1.EX_MOVE(gSysPrm, X, y, 1)

    '        Me.lblInfo.Text = MSG_CUT_POS_CORRECT_004 ' WARNING & vbCrLf & [START]Key Cross Cut & vbCrLf & [RESET]Key CANCEL
    '        '    iRetc = InputADVRESETKey()                      ' ｷｰ入力
    '        iRetc = Form1.System1.AdvResetSw() ' ｷｰ入力
    '        If (iRetc = 1) Then
    '            ' START
    '        ElseIf (iRetc = 2) Then
    '            ' RESET
    '        ElseIf (-1 = iRetc) Then
    '            GoTo EMERGENCY ' EMERGENCY
    '        End If
    '        If (1 = iRetc) Then ' START
    '            iRetc = 0
    '            bFgLoop = True
    '            Do
    '                System.Windows.Forms.Application.DoEvents() ' メッセージポンプ
    '                '            If Form1.System1.EmergencySwCheck() Then GoTo EMERGENCY
    '                ' システムエラーチェック
    '                errChk = Form1.System1.Sys_Err_Chk_EX(gSysPrm, giAppMode)
    '                If cFRS_NORMAL <> errChk Then GoTo EMERGENCY

    '                '            Call BpMove(x, y, 1)                    ' ｶｯﾄ前は位置中央へ移動
    '                Call Form1.System1.EX_MOVE(gSysPrm, X, y, 1) ' ｶｯﾄ前は位置中央へ移動

    '                ' ADJｷｰ有りなら待ち
    '                r = 1
    '                Call ZCONRST()
    '                If Form1.System1.AdjReqSw() Then
    '                    If pfCurrentNo < intChipNum Then ' ｸﾞﾙｰﾌﾟ内抵抗数分
    '                        Me.lblInfo.Text = MSG_CUT_POS_CORRECT_006 ' [START]十字カット実行" & vbCrLf & "[RESET]中止" & vbCrLf & "[ADJ]一時停止解除"
    '                    Else
    '                        Me.lblInfo.Text = MSG_CUT_POS_CORRECT_011 ' 十字カット終了"
    '                    End If

    '                    'Call Form1.System1.sLampOnOff(LAMP_HALT, True)
    '                    Call LAMP_CTRL(LAMP_HALT, True)
    '                    Do
    '                        System.Windows.Forms.Application.DoEvents()
    '                        '                    ' Emergency check
    '                        '                    If Form1.System1.EmergencySwCheck() Then GoTo EMERGENCY
    '                        ' システムエラーチェック
    '                        errChk = Form1.System1.Sys_Err_Chk_EX(gSysPrm, giAppMode)
    '                        If cFRS_NORMAL <> errChk Then GoTo EMERGENCY

    '                        r = Form1.System1.AdvResetSw()

    '#If cOFFLINEcDEBUG = 1 Then
    '                        If (MsgBoxResult.Ok = MsgBox("OK=START CANCEL=RESET", MsgBoxStyle.OkCancel)) Then
    '                            r = cSTS_STARTSW_ON 
    '                        Else
    '                            r = cSTS_RESETSW_ON 
    '                        End If
    '#End If

    '                        'If r = 1 Or r = 3 Then ' START(ADV)
    '                        If r = cSTS_STARTSW_ON Then ' START(ADV)
    '                            Exit Do
    '                        End If
    '                        If r = cSTS_RESETSW_ON Then ' RESET
    '                            'Call Form1.System1.sLampOnOff(LAMP_RESET, True)
    '                            Call LAMP_CTRL(LAMP_RESET, True)
    '                            '                        mMsgbox.Label1.Caption = "OK:[START] CANCEL:[HALT]"
    '                            If (ExitCheck("OK:[START(ADV)] CANCEL:[HALT]")) Then ' 終了確認
    '                                iRetc = 0
    '                                bFgLoop = False
    '                            Else
    '                                r = 0
    '                            End If
    '                            'Call Form1.System1.sLampOnOff(LAMP_RESET, False)
    '                            Call LAMP_CTRL(LAMP_RESET, False)
    '                        End If
    '                    Loop While (0 = r)
    '                    'Call Form1.System1.sLampOnOff(LAMP_HALT, False)
    '                    Call LAMP_CTRL(LAMP_HALT, False)
    '                End If

    '                If (cSTS_STARTSW_ON = r) Then ' START(ADV)
    '                    Me.lblInfo.Text = MSG_CUT_POS_CORRECT_005 ' 十字カット実行中" & vbCrLf & "[ADJ]一時停止"

    '                    ' 十字ｶｯﾄ
    '                    Call CrossCutExec(X, y)

    '                    '                Call BpMove(x, y, 1)                ' ｶｯﾄ後は位置中央へ移動
    '                    Call Form1.System1.EX_MOVE(gSysPrm, X, y, 1) ' ｶｯﾄ後は位置中央へ移動
    '                End If

    '                If (cSTS_STARTSW_ON = r) Then
    '                    If pfCurrentNo < intChipNum Then ' ｸﾞﾙｰﾌﾟ内抵抗数分
    '                        pfCurrentNo = pfCurrentNo + 1
    '                        If pfCurrentNo <= 5 Then ' 5個目以降は自動ｽｸﾛｰﾙ
    '                            Call Set_TopRow(1) ' グリッドの指定行を先頭にする 
    '                        Else
    '                            Call Set_TopRow(pfCurrentNo - 5) ' グリッドの指定行を先頭にする 
    '                        End If
    '                        Call Set_BackColor(pfCurrentNo) ' 次の抵抗の背景色変更
    '                        ' 次の座標取得
    '                        X = pfStartPos(0, pfCurrentNo) + dblCutPosCorrectOffsetX
    '                        y = pfStartPos(1, pfCurrentNo) + dblCutPosCorrectOffsetY
    '                        '                    Call BpMove(x, y, 1)                ' 次のｶｯﾄ位置へ移動
    '                        Call Form1.System1.EX_MOVE(gSysPrm, X, y, 1) ' 次のｶｯﾄ位置へ移動
    '                    Else
    '                        iRetc = 1 ' 正常終了
    '                        bFgLoop = False ' ｶｯﾄ終了
    '                    End If
    '                End If

    '            Loop While bFgLoop = True
    '        End If
    'EXIT_LOOP:

    '        Call ZCONRST()
    '        CutPosCorrectCrossExec = iRetc
    '        Exit Function

    'EMERGENCY:
    '        Call ZCONRST()

    '        '    gMode = 5
    '        '    frmReset.Show vbModal
    '        '    r = Form1.System1.Form_Reset(cGMODE_EMG, gSysPrm, giAppMode, gbInitialized, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, LoadDTFlag)
    '        iRetc = -1
    '        If (errChk <= cFRS_ERR_EMG) Then
    '            '強制終了
    '            Call Form1.AppEndDataSave()
    '            Call Form1.AplicationForcedEnding()
    '        End If
    '    End Function
    '#End Region

    '#Region "ﾊﾟﾀｰﾝ認識"
    '    '''=========================================================================
    '    '''<summary>ﾊﾟﾀｰﾝ認識</summary>
    '    '''<param name="iGroup">(INP)ｸﾞﾙｰﾌﾟ番号</param>
    '    '''<param name="iTemp">(INP)ﾃﾝﾌﾟﾚｰﾄ番号</param> 
    '    '''<param name="GapX"> (INP) </param>
    '    '''<param name="GapY"> (INP) </param>  
    '    '''<returns> </returns>
    '    '''=========================================================================
    '    Private Function PatternRecognition(ByRef iGroup As Short, ByRef iTemp As Short, ByRef GapX As Double, ByRef GapY As Double) As Boolean
    '        ' Video.ocxを使用するように変更 
    '        '        Dim bRetc As Boolean
    '        '        Dim dblPixel2umX As Double
    '        '        Dim dblPixel2umY As Double

    '        '        Call GetPixel2um(dblPixel2umX, dblPixel2umY)        ' ﾋﾟｸｾﾙ値X,Y

    '        '        bRetc = False
    '        '#If VIDEO_CAPTURE = 0 Then
    '        '        Dim r As Short
    '        '        Dim fcoeff As Double
    '        '        Dim lTblCnt As Integer

    '        '		Call PatternCrossLine(False)                        ' 十字表示消去
    '        '		' ほかの設定はForm1で初期済み
    '        '		Call MvcPt2_SetShrink(3)                            ' シュリンク回数
    '        '		Call MvcPt2_SetSrchOffset(4, 4)                     ' オフセット
    '        '		Call MvcPt2_SetCorrThresh(PATTERN_TRESH_INITVAL)     ' しきい値の再設定
    '        '		Call PatternAreaDisp(False)

    '        '        'Form1.picGazou.AutoRedraw = True
    '        '        Call Mvc10_PaintDIB(Form1.picGazou.Handle, mtDest, mlHSKDib, mtSrc)
    '        '        'Form1.picGazou.AutoRedraw = False

    '        '		' ﾃﾝﾌﾟﾚｰﾄ一括読み込み
    '        '		If ReadTemplateGourp(iGroup) Then
    '        '		Call MvcPt2_SetTempNo(iTemp)                ' カレントのテンプレート番号を指定
    '        '		'検索範囲設定
    '        '		Call MvcPt2_SetSrchWndRect(gtSearch.tDest)
    '        '		r = RunPatternMaching(iTemp, True, fcoeff)  ' ﾊﾟﾀｰﾝﾏｯﾁﾝｸﾞ実行

    '        '		If r = 0 Then                               ' ﾏｯﾁﾝｸﾞ0個
    '        '                Me.lblInfo.Text = MSG_CUT_POS_CORRECT_013
    '        '		GapX = 0
    '        '		GapY = 0
    '        '		bRetc = False
    '        '		Else
    '        '		lTblCnt = 1                             ' ずれ量算出、格納

    '        '		Select Case gSysPrm.Stdev.giBpDirXy

    '        '		Case 0      ' x←, y↓
    '        '		GapX = (RoundOff((gpoResult(lTblCnt).X - 320#) * (dblPixel2umX / 1000#)))
    '        '		GapY = -(RoundOff((gpoResult(lTblCnt).y - 238#) * (dblPixel2umY / 1000#)))

    '        '		Case 1      ' x→, y↓
    '        '		GapX = -(RoundOff((gpoResult(lTblCnt).X - 320#) * (dblPixel2umX / 1000#)))
    '        '		GapY = -(RoundOff((gpoResult(lTblCnt).y - 238#) * (dblPixel2umY / 1000#)))

    '        '		Case 2      ' x←, y↑
    '        '		GapX = (RoundOff((gpoResult(lTblCnt).X - 320#) * (dblPixel2umX / 1000#)))
    '        '		GapY = (RoundOff((gpoResult(lTblCnt).y - 238#) * (dblPixel2umY / 1000#)))

    '        '		Case 3      ' x→, y↑
    '        '		GapX = -(RoundOff((gpoResult(lTblCnt).X - 320#) * (dblPixel2umX / 1000#)))
    '        '		GapY = (RoundOff((gpoResult(lTblCnt).y - 238#) * (dblPixel2umY / 1000#)))

    '        '		End Select

    '        '		bRetc = True
    '        '		End If
    '        '		End If
    '        '		Call PatternCrossLine(True)                     ' 十字表示
    '        '#End If
    '        '        PatternRecognition = bRetc

    '    End Function
    '#End Region

    '#Region "十字ｶｯﾄを外部ｶﾒﾗで処理"
    '    '''=========================================================================
    '    '''<summary>十字ｶｯﾄを外部ｶﾒﾗで処理</summary>
    '    '''<param name="iBlockX">(INP)</param>
    '    '''<param name="iBlockY">(INP)</param> 
    '    '''<returns> </returns>
    '    '''=========================================================================
    '    Private Function CutPosCorrectExec(ByRef iBlockX As Short, ByRef iBlockY As Short) As Short

    '        Dim bRetc As Boolean
    '        Dim iRetc As Short
    '        Dim lCon As Integer
    '        Dim r As Short
    '        Dim GapX As Double
    '        Dim GapY As Double
    '        Dim bFgLoop As Boolean
    '        Dim bFgLoop2 As Boolean
    '        Dim intChipNum As Short
    '        Dim dblChipSizeX As Double
    '        Dim dblChipSizeY As Double
    '        Dim intGroup As Short
    '        Dim intTemp As Short
    '        Dim dblBpOffsetX As Double
    '        Dim dblBpOffsetY As Double
    '        Dim dblCutPosCorrectOffsetX As Double
    '        Dim dblCutPosCorrectOffsetY As Double

    '        Call GetChipNum(intChipNum) ' ｸﾞﾙｰﾌﾟ内抵抗数

    '        ' ﾁｯﾌﾟｻｲｽﾞ
    '        dblChipSizeX = typPlateInfo.dblChipSizeXDir
    '        dblChipSizeY = typPlateInfo.dblChipSizeYDir

    '        ' BP位置ｵﾌｾｯﾄX,Y設定
    '        dblBpOffsetX = typPlateInfo.dblBpOffSetXDir
    '        dblBpOffsetY = typPlateInfo.dblBpOffSetYDir

    '        ' ｶｯﾄ位置補正ｵﾌｾｯﾄX,Y
    '        dblCutPosCorrectOffsetX = typPlateInfo.dblCutPosiReviseOffsetXDir
    '        dblCutPosCorrectOffsetY = typPlateInfo.dblCutPosiReviseOffsetYDir
    '        ' ﾊﾟﾀｰﾝﾏｯﾁﾝｸﾞｸﾞﾙｰﾌﾟ番号/ﾃﾝﾌﾟﾚｰﾄ番号
    '        intGroup = typPlateInfo.intCutPosiReviseGroupNo
    '        intTemp = typPlateInfo.intCutPosiRevisePtnNo

    '        Call ZCONRST() ' ラッチ解除

    '        Me.lblInfo.Text = MSG_CUT_POS_CORRECT_007 ' [START]画像認識実行" & vbCrLf & "[RESET]中止"

    '        '    iRetc = InputADVRESETKey()                      ' ｷｰ入力
    '        iRetc = Form1.System1.AdvResetSw() ' ｷｰ入力
    '        If (iRetc = 1) Then
    '            ' ADV
    '        ElseIf (iRetc = 2) Then
    '            ' RESET
    '        ElseIf (-1 = iRetc) Then
    '            GoTo EMERGENCY ' EMERGENCY
    '        End If

    '        If (1 = iRetc) Then ' START

    '            pfCurrentNo = 1 ' 画像処理を始めるので先頭へ戻す

    '            Call Set_TopRow(pfCurrentNo) ' グリッドの指定行を先頭にする 
    '            Call Set_BackColor(pfCurrentNo)
    '            ' BPｵﾌｾｯﾄにｶｯﾄ位置補正ｵﾌｾｯﾄ設定
    '            '        Call BpOffset(0, 0)                         ' 外部ｶﾒﾗではBPは使用できないので
    '            '        Call BpMove(0, 0, 1)                        ' 初期化しておく
    '            Call Form1.System1.EX_BPOFF(gSysPrm, 0.0#, 0.0#) ' BPｵﾌｾｯﾄにｶｯﾄ位置補正ｵﾌｾｯﾄ設定
    '            Call Form1.System1.EX_MOVE(gSysPrm, 0.0#, 0.0#, 1) ' 最初のｶｯﾄ位置中央へ移動

    '#If VIDEO_CAPTURE = 0 Then
    '            Call Form1.VideoLibrary1.ChangeCamera(EXTERNAL_CAMERA)                     ' カメラ切替(外部ｶﾒﾗ)
    '#End If
    '            ' 外部ｶﾒﾗへ移動
    '            Call XYTableMoveBlockFirst(1, 1, 1, iBlockX, iBlockY, pfCurrentNo)
    '            Call System.Threading.Thread.Sleep(1000)                       ' テーブル移動後画像安定待ち時間(ms)
    '            iRetc = 0
    '            bFgLoop = True
    '            Do
    '                System.Windows.Forms.Application.DoEvents() ' メッセージポンプ
    '                If Form1.System1.EmergencySwCheck() Then GoTo EMERGENCY

    '                ' ADJｷｰ有りなら待ち
    '                r = 1
    '                Call ZCONRST() ' ラッチ解除
    '                If Form1.System1.AdjReqSw() Then
    '                    If pfCurrentNo < intChipNum Then ' ｸﾞﾙｰﾌﾟ内抵抗数分
    '                        Me.lblInfo.Text = MSG_CUT_POS_CORRECT_009 ' [START]画像認識実行" & vbCrLf & "[RESET]中止" & vbCrLf & "[ADJ]一時停止解除"
    '                    Else
    '                        Me.lblInfo.Text = MSG_CUT_POS_CORRECT_012 ' 画像認識終了
    '                    End If

    '                    'Call Form1.System1.sLampOnOff(LAMP_HALT, True)
    '                    Call LAMP_CTRL(LAMP_HALT, True)
    '                    r = 0
    '                    Do
    '                        Call ZCONRST() ' ラッチ解除
    '                        System.Windows.Forms.Application.DoEvents()
    '                        ' Emergency check
    '                        If Form1.System1.EmergencySwCheck() Then GoTo EMERGENCY

    '#If cOFFLINEcDEBUG = 1 Then
    '                        lCon = MsgBox("YES=START NO=HALT CANCEL=RESET", MsgBoxStyle.YesNoCancel)
    '                        If (MsgBoxResult.Yes = lCon) Then
    '                            lCon = &H4S
    '                        ElseIf (MsgBoxResult.No = lCon) Then
    '                            lCon = &H2S
    '                        Else
    '                            lCon = &H8S
    '                        End If
    '#Else
    '                        '                        lCon = ConsoleInput         ' ｷｰ入力
    '                        Call ZINPSTS(GET_CONSOLE_INPUT, lCon)
    '#End If

    '                        If ((lCon And &H4S) = &H4S) Then ' ADV
    '                            r = cSTS_STARTSW_ON
    '                            Exit Do ' 続行
    '                        ElseIf ((lCon And &H8S) = &H8S) Then  ' RESET
    '                            r = cSTS_RESETSW_ON
    '                            'Call Form1.System1.sLampOnOff(LAMP_RESET, True)
    '                            Call LAMP_CTRL(LAMP_RESET, True)
    '                            '                        mMsgbox.Label1.Caption = "OK:[ADV] CANCEL:[HALT]"
    '                            If (ExitCheck("OK:[START] CANCEL:[HALT]")) Then ' 終了確認
    '                                iRetc = 0
    '                                bFgLoop = False
    '                            Else
    '                                r = 0
    '                            End If
    '                            'Call Form1.System1.sLampOnOff(LAMP_RESET, False)
    '                            Call LAMP_CTRL(LAMP_RESET, False)
    '                        ElseIf ((lCon And &H2S) = &H2S) Then  ' HALT
    '                            r = cSTS_HALTSW_ON
    '                        End If
    '                    Loop While (0 = r)
    '                    'Call Form1.System1.sLampOnOff(LAMP_HALT, False)
    '                    Call LAMP_CTRL(LAMP_HALT, False)
    '                End If

    '                If (cSTS_STARTSW_ON = r) Then
    '                    Me.lblInfo.Text = MSG_CUT_POS_CORRECT_008 ' 画像認識実行実行中" & vbCrLf & "[ADJ]一時停止"

    '                    ' 画像認識
    '                    GapX = 0
    '                    GapY = 0
    '                    bRetc = PatternRecognition(intGroup, intTemp, GapX, GapY)

    '                    If (Not bRetc) Then
    '                        bFgLoop2 = True
    '                        Me.lblInfo.Text = MSG_CUT_POS_CORRECT_013
    '                        Do
    '                            lCon = ExtTableJogX(GapX, GapY) ' 手動ﾓｰﾄﾞ突入
    '                            If ((lCon And &H4S) = &H4S) Then ' ADV
    '                                r = cSTS_STARTSW_ON
    '                                bRetc = True
    '                                bFgLoop2 = False
    '                            ElseIf ((lCon And &H8S) = &H8S) Then  ' RESET
    '                                r = cSTS_RESETSW_ON
    '                                'Call Form1.System1.sLampOnOff(LAMP_RESET, True)
    '                                Call LAMP_CTRL(LAMP_RESET, True)
    '                                '                            mMsgbox.Label1.Caption = "OK:[ADV] CANCEL:[HALT]"
    '                                If (ExitCheck("OK:[START] CANCEL:[HALT]")) Then ' 終了確認
    '                                    iRetc = 0
    '                                    bFgLoop2 = False
    '                                    bFgLoop = False
    '                                End If
    '                                'Call Form1.System1.sLampOnOff(LAMP_RESET, False)
    '                                Call LAMP_CTRL(LAMP_RESET, False)
    '                            ElseIf ((lCon And &H2S) = &H2S) Then  ' HALT
    '                                bFgLoop2 = False
    '                                r = cSTS_HALTSW_ON
    '                            End If
    '                        Loop While (bFgLoop2)
    '                    End If
    '                    If (bRetc) Then
    '                        dblGapX(pfCurrentNo) = GapX ' ずれ量X
    '                        dblGapY(pfCurrentNo) = GapY ' ずれ量Y
    '                        Me.fgrdPoints.set_TextMatrix(pfCurrentNo, 3, GapX.ToString("0.0000"))
    '                        Me.fgrdPoints.set_TextMatrix(pfCurrentNo, 4, GapY.ToString("0.0000"))
    '                    End If
    '                End If

    '                If (cSTS_STARTSW_ON = r) Then
    '                    If pfCurrentNo < intChipNum Then        ' ｸﾞﾙｰﾌﾟ内抵抗数分
    '                        pfCurrentNo = pfCurrentNo + 1
    '                        If pfCurrentNo <= 5 Then            ' 5個目以降は自動ｽｸﾛｰﾙ
    '                            Call Set_TopRow(1)                          ' グリッドの指定行を先頭にする 
    '                        Else
    '                            Call Set_TopRow(pfCurrentNo - 5) ' グリッドの指定行を先頭にする 
    '                        End If
    '                        Call Set_BackColor(pfCurrentNo)  ' 次の抵抗の背景色変更
    '                        Call XYTableMoveBlockFirst(1, 1, 1, iBlockX, iBlockY, pfCurrentNo)
    '                        Call System.Threading.Thread.Sleep(1000) ' テーブル移動後画像安定待ち時間(ms)
    '                    Else
    '                        pfCurrentNo = pfCurrentNo + 1
    '                        Call ZCONRST() ' ラッチ解除
    '                        '                    mMsgbox.Label1.Caption = MSG_106    ' この情報を保存して前の画面に戻ります。よろしいですか？"
    '                        If (ExitCheck(MSG_106)) Then ' 終了確認
    '                            UpdatePoints() ' 各ｽﾀｰﾄﾎﾟｲﾝﾄに反映
    '                            iRetc = 1 ' 正常終了
    '                            bFgLoop = False ' 補正終了
    '                        Else
    '                            r = cSTS_HALTSW_ON  ' 一つ前へ
    '                        End If
    '                    End If
    '                End If
    '                If (cSTS_HALTSW_ON = r) Then
    '                    If pfCurrentNo > 1 Then
    '                        pfCurrentNo = pfCurrentNo - 1
    '                        If pfCurrentNo <= 5 Then
    '                            Call Set_TopRow(1) ' グリッドの指定行を先頭にする 
    '                        Else
    '                            Call Set_TopRow(pfCurrentNo - 5) ' グリッドの指定行を先頭にする 
    '                        End If
    '                        Call Set_BackColor(pfCurrentNo)
    '                        Call XYTableMoveBlockFirst(1, 1, 1, iBlockX, iBlockY, pfCurrentNo)
    '                        Call System.Threading.Thread.Sleep(1000)           ' テーブル移動後画像安定待ち時間(ms)
    '                    End If
    '                End If
    '            Loop While (bFgLoop)
    '        End If
    '        Call ZCONRST()

    '#If VIDEO_CAPTURE = 0 Then
    '        Call Form1.VideoLibrary1.ChangeCamera(INTERNAL_CAMERA)       ' カメラ切替(内部ｶﾒﾗ)
    '        Call PatternCrossLine(False)
    '        Call PatternAreaDisp(False)
    '#End If

    '        CutPosCorrectExec = iRetc
    '        Exit Function

    'EMERGENCY:
    '        Call ZCONRST()

    '        '    gMode = 5
    '        '    frmReset.Show vbModal
    '        r = Form1.System1.Form_Reset(cGMODE_EMG, gSysPrm, giAppMode, gbInitialized, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, LoadDTFlag)
    '        iRetc = -1
    '        If (r <= cFRS_ERR_EMG) Then
    '            '強制終了
    '            Call Form1.AppEndDataSave()
    '            Call Form1.AplicationForcedEnding()
    '        End If

    '#If VIDEO_CAPTURE = 0 Then
    '        Call Form1.VideoLibrary1.ChangeCamera(INTERNAL_CAMERA)       ' カメラ切替(内部ｶﾒﾗ)
    '        Call PatternCrossLine(False)
    '        Call PatternAreaDisp(False)
    '#End If

    '        CutPosCorrectExec = iRetc

    '    End Function
    '#End Region

    '#Region "十字ｶｯﾄを行う"
    '    '''=========================================================================
    '    '''<summary>十字ｶｯﾄを行う</summary>
    '    '''<param name="dx">(INP)BP座標X</param>
    '    '''<param name="dy">(INP)BP座標Y</param> 
    '    '''<remarks>十字ｶｯﾄの中央にBPを移動しておくこと</remarks>
    '    '''=========================================================================
    '    Private Function CrossCutExec(ByRef dx As Double, ByRef dy As Double) As Short

    '        Dim dblCutLength As Double
    '        Dim dblCutSpeed As Double
    '        Dim dblCutQRate As Double
    '        Dim intXDir As Short
    '        Dim intYDir As Short
    '        Dim d As Double

    '        dblCutLength = typPlateInfo.dblCutPosiReviseCutLength   ' ｶｯﾄ長
    '        dblCutSpeed = typPlateInfo.dblCutPosiReviseCutSpeed     ' ｶｯﾄ速度
    '        dblCutQRate = typPlateInfo.dblCutPosiReviseCutQRate     ' ｶｯﾄQﾚｰﾄ

    '        If gSysPrm.stDEV.giBpDirXy = 0 Or gSysPrm.stDEV.giBpDirXy = 2 Then ' X方向反転なし
    '            intXDir = 1
    '        Else ' X方向反転
    '            intXDir = 0
    '        End If

    '        Call QRATE(dblCutQRate)

    '        '    Call BpMove(dx - (dblCutLength / 2), dy, 1)     ' X軸始点へ
    '        Call Form1.System1.EX_MOVE(gSysPrm, dx - (dblCutLength / 2), dy, 1) ' X軸始点へ
    '        ' LASER ON
    '        Call LASERON()
    '        Call cut(dblCutLength, intXDir, dblCutSpeed) ' X軸ｶｯﾄ
    '        ' LASER OFF
    '        Call LASEROFF()
    '        '    Call BpMove(dx, dy, 1)      ' 中心へ戻す
    '        Call Form1.System1.EX_MOVE(gSysPrm, dx, dy, 1) ' 中心へ戻す
    '        Call System.Threading.Thread.Sleep(500)                   ' Wait(500ms)

    '        If gSysPrm.stDEV.giBpDirXy = 0 Or gSysPrm.stDEV.giBpDirXy = 1 Then ' Y方向反転なし
    '            intYDir = 3
    '            d = 1.0#
    '        Else ' Y方向反転
    '            intYDir = 3
    '            d = -1.0#
    '        End If

    '        '    Call BpMove(dx, (dy - (dblCutLength / 2)) * d, 1)   ' Y軸始点へ
    '        Call Form1.System1.EX_MOVE(gSysPrm, dx, (dy - (dblCutLength / 2)) * d, 1) ' Y軸始点へ

    '        ' LASER ON
    '        Call LASERON()
    '        Call cut(dblCutLength, intYDir, dblCutSpeed) ' Y軸ｶｯﾄ
    '        ' LASER OFF
    '        Call LASEROFF()

    '        '    Call BpMove(dx, dy, 1)  ' 中心へ戻す
    '        Call Form1.System1.EX_MOVE(gSysPrm, dx, dy, 1) ' 中心へ戻す
    '        Call System.Threading.Thread.Sleep(500) ' 500ms Wait

    '    End Function
    '#End Region


    '#Region "矢印SWによるテーブル動作(外部カメラ下)"
    '    '''=========================================================================
    '    '''<summary>矢印SWによるテーブル動作(外部カメラ下)</summary>
    '    '''<param name="dblGapX">(INP)テーブル座標X</param>
    '    '''<param name="dblGapY">(INP)テーブル座標Y</param> 
    '    '''<returns> </returns>
    '    '''=========================================================================
    '    Private Function ExtTableJogX(ByRef dblGapX As Double, ByRef dblGapY As Double) As Short

    '        Dim Dspx As Double
    '        Dim Dspy As Double
    '        Dim mPIT As Double '移動ｲﾝﾀｰﾊﾞﾙ
    '        Dim mvx As Double
    '        Dim mvy As Double
    '        Dim rslt As Integer
    '        Dim cin As Integer

    '        'inisialize
    '        mvx = 0 : mvy = 0
    '        Dspx = dblGapX : Dspy = dblGapY
    '        Me.fgrdPoints.set_TextMatrix(pfCurrentNo, 3, Dspx.ToString("0.0000"))
    '        Me.fgrdPoints.set_TextMatrix(pfCurrentNo, 4, Dspy.ToString("0.0000"))

    '        'HALT/RESET/STARTｷｰが押されるまでﾙｰﾌﾟ
    '        Do
    '            If Form1.System1.EmergencySwCheck() Then
    '                Call ZCONRST() 'ﾗｯﾁ解除
    '                rslt = &H10S
    '                Exit Do
    '            End If

    '            System.Windows.Forms.Application.DoEvents()

    '            ' ｺﾝｿｰﾙ入力
    '#If cOFFLINEcDEBUG Then
    '            'Me.Framedebug.Visible = True                'ﾃﾞﾊﾞｯｸﾞ用
    '#Else
    '            '            AppInfo.cmdNo = CMD_Z_INPSTS 'ｺﾝｿｰﾙ入力
    '            '            Call VBINtimeSendMessage(1, AppInfo, ResInfo)
    '            Call ZINPSTS(GET_CONSOLE_INPUT, cin)
    '#End If

    '            'key check!
    '            If cin And &H1E00S Then '矢印SW
    '                '移動ｲﾝﾀｰﾊﾞﾙ
    '                If cin And &H100S Then
    '                    mPIT = gSysPrm.stSYP.gStageHighPIT 'High speed
    '                Else
    '                    mPIT = gSysPrm.stSYP.gPIT 'Normal
    '                End If
    '                '矢印ﾁｪｯｸ!
    '                If cin And &H200S Then '→
    '                    mvx = mPIT
    '                    mvy = 0
    '                ElseIf cin And &H400S Then  '←
    '                    mvx = -1 * mPIT
    '                    mvy = 0
    '                ElseIf cin And &H800S Then  '↑
    '                    mvx = 0
    '                    mvy = mPIT
    '                ElseIf cin And &H1000S Then  '↓
    '                    mvx = 0
    '                    mvy = -1 * mPIT
    '                End If

    '                'ﾃｰﾌﾞﾙ相対移動
    '                '            Call XYtableRelMove(mvx, mvy)
    '                Call Form1.System1.XYtableRelMove(gSysPrm, mvx, mvy)
    '                Call ZWAIT(gSysPrm.stSYP.gPitPause)
    '                cin = 0

    '                'XY現象の考慮(2004/12/02)
    '                Select Case gSysPrm.stDEV.giBpDirXy
    '                    Case 0 ' x←, y↓
    '                        mvx = -mvx
    '                        mvy = -mvy
    '                    Case 1 ' x→, y↓
    '                        mvx = mvx
    '                        mvy = -mvy
    '                    Case 2 ' x←, y↑
    '                        mvx = -mvx
    '                        mvy = mvy
    '                    Case 3 ' x→, y↑
    '                        mvx = mvx
    '                        mvy = mvy
    '                End Select

    '                '座標表示
    '                Dspx = Dspx + mvx
    '                Dspy = Dspy + mvy
    '                Me.fgrdPoints.set_TextMatrix(pfCurrentNo, 3, Dspx.ToString("0.0000"))
    '                Me.fgrdPoints.set_TextMatrix(pfCurrentNo, 4, Dspy.ToString("0.0000"))
    '            ElseIf cin And &H4S Then  'START(ADV) SW
    '                dblGapX = Dspx
    '                dblGapY = Dspy
    '                Call ZCONRST() 'ﾗｯﾁ解除
    '                rslt = &H4S
    '                Exit Do
    '            ElseIf cin And &H8S Then  'RESET SW
    '                Call ZCONRST() 'ﾗｯﾁ解除
    '                rslt = &H8S
    '                Exit Do
    '            ElseIf cin And &H2S Then  'HALT SW
    '                Call ZCONRST() 'ﾗｯﾁ解除
    '                rslt = &H2S
    '                Exit Do
    '            End If
    '        Loop
    '        ExtTableJogX = rslt
    '        cin = 0

    '        ' ﾃﾞﾊﾞｯｸﾞ用
    '#If cOFFLINEcDEBUG Then
    '        'Me.Framedebug.Visible = False
    '#End If

    '    End Function
    '#End Region

    '#Region "終了確認"
    '    '''=========================================================================
    '    '''<summary>終了確認</summary>
    '    '''<returns>TRUE:終了, FALSE:ｷｬﾝｾﾙ</returns>
    '    '''=========================================================================
    '    Private Function ExitCheck(ByRef strMSG As String) As Boolean
    '        ExitFlag = False
    '        '    mMsgbox.Show vbModal
    '        '    ExitCheck = mMsgbox.sGetReturn
    '        ExitCheck = Form1.System1.TrmMsgBox(gSysPrm, strMSG, MsgBoxStyle.OkCancel, FRM_CUT_POS_CORRECT_TITLE)

    '    End Function
    '#End Region

    '#Region "指定のブロック番号からテーブル座標(外部カメラ下)を求める"
    '    '''=========================================================================
    '    ''' <summary>指定のブロック番号からテーブル座標(外部カメラ下)を求める</summary>
    '    ''' <param name="stPLT">(INP)プレートデータ</param>
    '    ''' <param name="BlX">  (INP)ブロック番号X</param>
    '    ''' <param name="BlY">  (INP)ブロック番号Y</param>
    '    ''' <param name="POSX"> (OUT)テーブル座標X</param>
    '    ''' <param name="POSY"> (OUT)テーブル座標Y</param>
    '    ''' <remarks></remarks>
    '    '''=========================================================================
    '    Private Sub GetTablePos(ByRef stPLT As PlateInfo, ByVal BlX As Integer, ByVal BlY As Integer, ByRef POSX As Double, ByRef POSY As Double)

    '        Dim CSx As Double, CSy As Double                                    ' チップサイズ
    '        Dim OfsX As Double, OfsY As Double                                  ' テーブル位置オフセット
    '        Dim BpOfsX As Double, BpOfsY As Double                              ' ＢＰオフセット
    '        Dim Del_x As Double, Del_y As Double                                ' トリムポジション補正値
    '        Dim dblStepIntervel As Double
    '        Dim strMSG As String

    '        Try

    '            ' テーブル位置オフセット
    '            OfsX = stPLT.dblTableOffsetXDir
    '            OfsY = stPLT.dblTableOffsetYDir
    '            ' トリムポジション補正値(θ補正時のXYﾃｰﾌﾞﾙずれ量)
    '            Del_x = gfCorrectPosX
    '            Del_y = gfCorrectPosY
    '            ' ＢＰオフセット
    '            BpOfsX = stPLT.dblBpOffSetXDir
    '            BpOfsY = stPLT.dblBpOffSetYDir
    '            ' チップサイズ
    '            CSx = stPLT.dblChipSizeXDir
    '            CSy = stPLT.dblChipSizeYDir

    '            ' ﾄﾘﾐﾝｸﾞ位置 (+or-) 回転半径 + ﾃｰﾌﾞﾙｵﾌｾｯﾄ + ﾃｰﾌﾞﾙ補正量 + 外部ｶﾒﾗｵﾌｾｯﾄ
    '            Select Case gSysPrm.stDEV.giBpDirXy
    '                Case 0      ' x←, y↓
    '                    POSX = gSysPrm.stDEV.gfTrimX + OfsX + Del_x + gSysPrm.stDEV.gfExCmX + BpOfsX
    '                    POSY = gSysPrm.stDEV.gfTrimY + OfsY + Del_y + gSysPrm.stDEV.gfExCmY + BpOfsY

    '                Case 1      ' x→, y↓
    '                    POSX = gSysPrm.stDEV.gfTrimX + OfsX + Del_x + gSysPrm.stDEV.gfExCmX - BpOfsX      '　左上基準の場合、X方向が逆になるため、減算
    '                    POSY = gSysPrm.stDEV.gfTrimY + OfsY + Del_y + gSysPrm.stDEV.gfExCmY + BpOfsY

    '                Case 2      ' x←, y↑
    '                    POSX = gSysPrm.stDEV.gfTrimX + OfsX + Del_x + gSysPrm.stDEV.gfExCmX + BpOfsX
    '                    POSY = gSysPrm.stDEV.gfTrimY + OfsY + Del_y + gSysPrm.stDEV.gfExCmY - BpOfsY      ' 右下基準の場合、Y方向が逆になるため、減算

    '                Case 3      ' x→, y↑
    '                    POSX = gSysPrm.stDEV.gfTrimX + OfsX + Del_x + gSysPrm.stDEV.gfExCmX - BpOfsX      ' 左下基準の場合、X方向が逆になるため、減算
    '                    POSY = gSysPrm.stDEV.gfTrimY + OfsY + Del_y + gSysPrm.stDEV.gfExCmY - BpOfsY      ' 左下基準の場合、Y方向が逆になるため、減算
    '            End Select

    '            ' ｽﾃｯﾌﾟｲﾝﾀｰﾊﾞﾙ算出(Y軸)(抵抗並び方向 = X方向の場合)
    '            If (stPLT.intResistDir = 0) Then                                        ' 抵抗並び方向 = X方向
    '                Call CalcStepIntervel(typPlateInfo, typStepInfoArray, BlY, dblStepIntervel)
    '                If gSysPrm.stDEV.giBpDirXy = 0 Or gSysPrm.stDEV.giBpDirXy = 1 Then  ' ﾃｰﾌﾞﾙY方向反転なし ?
    '                    POSY = POSY + (CSy * (BlY - 1))
    '                    POSY = POSY + dblStepIntervel
    '                Else                                                                ' ﾃｰﾌﾞﾙY方向反転
    '                    POSY = POSY - (CSy * (BlY - 1))
    '                    POSY = POSY - dblStepIntervel
    '                End If

    '                ' ｽﾃｯﾌﾟｲﾝﾀｰﾊﾞﾙ算出(X軸)(抵抗並び方向 = Y方向の場合)
    '            Else                                                                    ' 抵抗並び方向 = Y方向
    '                Call CalcStepIntervel(typPlateInfo, typStepInfoArray, BlX, dblStepIntervel)
    '                If gSysPrm.stDEV.giBpDirXy = 0 Or gSysPrm.stDEV.giBpDirXy = 2 Then  ' ﾃｰﾌﾞﾙX方向反転なし
    '                    POSX = POSX + (CSx * (BlX - 1))
    '                    POSX = POSX + dblStepIntervel
    '                Else                                                                ' ﾃｰﾌﾞﾙX方向反転
    '                    POSX = POSX - (CSx * (BlX - 1))
    '                    POSX = POSX - dblStepIntervel
    '                End If
    '            End If

    '            Exit Sub

    '            ' トラップエラー発生時
    '        Catch ex As Exception
    '            strMSG = "FrmCutPosCorrect.GetTablePos() TRAP ERROR = " + ex.Message
    '            MsgBox(strMSG)
    '        End Try
    '    End Sub
    '#End Region
    '#Region "スタートポイントグリッドの指定行の背景を黄色にする"
    '    '''=========================================================================
    '    '''<summary>スタートポイントグリッドの指定行の背景を黄色にする</summary>
    '    '''<param name="iCol">(INP)行番号(1-)</param>
    '    '''=========================================================================
    '    Private Sub SetBackColor(ByVal iCol As Short)

    '        Static iPrevColumn As Integer
    '        Dim strMSG As String

    '        Try
    '            If IsNothing(iPrevColumn) Then                  ' 以前選択されていたか
    '                iPrevColumn = 0
    '            End If

    '            With Me.fgrdPoints
    '                If iPrevColumn >= 0 Then                    ' 選択されていたら黒にする
    '                    If .Rows > iPrevColumn Then
    '                        .Row = iPrevColumn
    '                        .Col = 1 : .CellBackColor = System.Drawing.ColorTranslator.FromOle(0)
    '                        .Col = 2 : .CellBackColor = System.Drawing.ColorTranslator.FromOle(0)
    '                        .Col = 3 : .CellBackColor = System.Drawing.ColorTranslator.FromOle(0)
    '                        .Col = 4 : .CellBackColor = System.Drawing.ColorTranslator.FromOle(0)
    '                    End If
    '                End If
    '                .Row = iCol                                 ' 黄色に設定
    '                .Col = 1 : .CellBackColor = System.Drawing.ColorTranslator.FromOle(QBColor(14))
    '                .Col = 2 : .CellBackColor = System.Drawing.ColorTranslator.FromOle(QBColor(14))
    '                .Col = 3 : .CellBackColor = System.Drawing.ColorTranslator.FromOle(QBColor(14))
    '                .Col = 4 : .CellBackColor = System.Drawing.ColorTranslator.FromOle(QBColor(14))
    '            End With
    '            iPrevColumn = iCol

    '            ' トラップエラー発生時
    '        Catch ex As Exception
    '            strMSG = "FrmCutPosCorrect.SetBackColor() TRAP ERROR = " + ex.Message
    '            MsgBox(strMSG)
    '        End Try
    '    End Sub
    '#End Region

End Class