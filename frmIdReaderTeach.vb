'===============================================================================
'   Description  : ＩＤリーダーティーチング画面処理 V1.13.0.0⑥
'
'   Copyright(C) : OMRON LASERFRONT INC. 2013
'
'　frmTxTyTeach.vbをベースに改造
'
'  2013/11/07  Written by N.Arata
'
'===============================================================================
Option Strict Off
Option Explicit On

Imports LaserFront.Trimmer.DefTrimFnc
Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8①
Imports TKY_ALL_SL432HW.My.Resources    'V4.4.0.0-0

Friend Class frmIDReaderTeach
    Inherits System.Windows.Forms.Form
    Implements ICommonMethods              'V6.0.0.0⑪

    <System.Runtime.InteropServices.DllImport("user32.dll")> _
    Private Shared Function SetWindowPos(ByVal hWnd As IntPtr, _
         ByVal hWndInsertAfter As IntPtr, ByVal x As Integer, ByVal y As Integer, _
             ByVal width As Integer, ByVal height As Integer, _
         ByVal flags As Integer) As UInt32
    End Function

    <System.Runtime.InteropServices.DllImport("user32.dll")> _
    Private Shared Function _
      SetForegroundWindow(ByVal hWnd As IntPtr) As Boolean
    End Function

#Region "プライベート変数定義"
    Private Declare Function GetKeyState Lib "user32" (ByVal nVirtKey As Integer) As Integer

    '===========================================================================
    '   変数定義
    '===========================================================================
    '----- オブジェクト定義 -----
    Private stJOG As JOG_PARAM                                      ' 矢印画面(BPのJOG操作)用パラメータ
    Private mExit_flg As Short                                      ' ＩＤリーダーティーチング結果

    '----- その他 -----
    Private dblTchMoval(3) As Double                                ' ﾋﾟｯﾁ退避域(0=ﾋﾟｯﾁ, 1=HIGHﾋﾟｯﾁ, 2=Pause Time(Sec))


    '----- トリミングパラメータ -----
    Private dblIDReadPos1X As Double                        ' IDﾘｰﾄﾞ読み取りﾎﾟｼﾞｼｮﾝ 1X
    Private dblIDReadPos1Y As Double                        ' IDﾘｰﾄﾞ読み取りﾎﾟｼﾞｼｮﾝ 1Y
    Private dblIDReadPos2X As Double                        ' IDﾘｰﾄﾞ読み取りﾎﾟｼﾞｼｮﾝ 2X
    Private dblIDReadPos2Y As Double                        ' IDﾘｰﾄﾞ読み取りﾎﾟｼﾞｼｮﾝ 2Y

    Public ObjOmronIDReader As New TrimClassLibrary.OmronIDReader

    Private TouchFinderProcessId As Integer = 0

    Private IdReaderPos As Integer = 1
#End Region

#Region "終了結果を返す"
    ' '''=========================================================================
    ' '''　<summary>終了結果を返す</summary>
    ' '''　<returns>cFRS_ERR_START = OK(STARTｷｰ)
    ' '''  　       cFRS_ERR_RST   = Cancel(RESETｷｰ)
    ' '''    　     cFRS_TxTy      = TX2(Teach)/TY2押下
    ' '''      　   -1以下         = エラー</returns>
    ' '''=========================================================================
    'Public ReadOnly Property sGetReturn() As Integer Implements ICommonMethods.sGetReturn  'V6.0.0.0⑪
    '    'V6.0.0.0⑪    Public ReadOnly Property sGetReturn() As Short
    '    Get
    '        sGetReturn = mExit_flg
    '    End Get
    'End Property
#End Region

    '=========================================================================
    '   フォームの初期化/終了処理
    '=========================================================================
#Region "Form Initialize時処理"
    '''=========================================================================
    '''<summary>Form Initialize時処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub Form_Initialize_Renamed()

        Dim strMSG As String

        Try
            'V6.0.0.0⑫                  ↓
            stJOG.TenKey = New Button() {BtnJOG_0, BtnJOG_1, BtnJOG_2, BtnJOG_3,
                                         BtnJOG_4, BtnJOG_5, BtnJOG_6, BtnJOG_7, BtnHI}
            'V6.0.0.0⑫                  ↑

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "frmIDReaderTeach.Form_Initialize_Renamed() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "Form 終了時処理"
    '''=========================================================================
    '''<summary>Form 終了時処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub frmIDReaderTeach_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        Dim strMSG As String

        Try
            If Not TouchFinderProcess.HasExited Then    ' タッチファインダーが起動したままなら終了させる。
                TouchFinderProcess.Kill()
            End If

            ObjOmronIDReader = Nothing

        Catch ex As System.InvalidOperationException
            ' ”このオブジェクトに関連付けられているプロセスはありません。”は何もしない
        Catch ex As Exception
            strMSG = "frmIDReaderTeach.frmIDReaderTeach_FormClosed() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "Form Load時処理"
    '''=========================================================================
    '''<summary>Form Load時処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub frmIDReaderTeach_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        Dim strMSG As String

        Try
            ' 初期処理
            Call SetDispMsg()                                       ' ラベル等を設定する(日本語/英語)
            mExit_flg = -1                                          ' 終了フラグ = 初期化

            ' ｳｲﾝﾄﾞｳをｺﾝﾄﾛｰﾙに重ねる
            Me.Top = Form1.VideoLibrary1.Top - Me.lblTitle2.Top
            Me.Left = Form1.VideoLibrary1.Right

            ' トリミングデータより必要なパラメータを取得する
            Call SetTrimData()

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "frmIDReaderTeach.frmIDReaderTeach_Load() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "ラベル等を設定する(日本語/英語)"
    '''=========================================================================
    '''<summary>ラベル等を設定する(日本語/英語)</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub SetDispMsg()

        Dim strMSG As String

        Try
            ' ラベル等を設定する
            With Me
                '.lblTitle.Text = LBL_IDREADER_TEACH_01                  ' ﾀｲﾄﾙ
                .lblTitle2.Text = ""

                ' ＩＤリーダーティーチング共通
                ' frame
                '.GrpFram_0.Text = LBL_IDREADER_TEACH_02                 ' 第１ＩＤ読み取り位置
                '.GrpFram_1.Text = LBL_IDREADER_TEACH_03                 ' 第２ＩＤ読み取り位置
                '.GrpFram_2.Text = LBL_IDREADER_TEACH_04                 ' ＩＤリーダー
                ' button
                '.cmdOK.Text = "OK"
                '.cmdCancel.Text = CMD_CANCEL
            End With

            ' 表示関連初期化
            Call InitDisp()

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "frmIDReaderTeach.SetDispMsg() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "表示関連初期化"
    '''=========================================================================
    '''<summary>表示関連初期化</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub InitDisp()

        Dim strMSG As String

        Try
            ' 座標表示クリア
            With Me
                .TxtPosX.Text = ""                                ' 第1ＩＤリーダー位置XY
                .TxtPosY.Text = ""
                .TxtPos2X.Text = ""                               ' 第2ＩＤリーダー位置XY
                .TxtPos2Y.Text = ""

            End With

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "frmIDReaderTeach.InitDisp() TRAP ERROR = " + ex.Message
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
    Private Sub frmIDReaderTeach_Activated(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Activated

        Dim strMSG As String

        Try
            ' ＩＤリーダーティーチング開始
            If (mExit_flg <> -1) Then Exit Sub
            mExit_flg = 0                                           ' 終了フラグ = 0
            mExit_flg = IdReaderMainProc()                              ' ＴＸまたはＴＹティーチング開始

            ' 補正クロスラインを非表示とする
            'V6.0.0.0④            Form1.CrosLineX.Visible = False
            'V6.0.0.0④            Form1.CrosLineY.Visible = False
            Form1.Instance.VideoLibrary1.SetCorrCrossVisible(False)     'V6.0.0.0④

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "frmIDReaderTeach.frmIDReaderTeach_Activated() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            mExit_flg = cERR_TRAP                                   ' Return値 = 例外エラー
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
            ' ＩＤリーダーティーチング開始
            If (mExit_flg <> -1) Then Exit Function
            mExit_flg = 0                                           ' 終了フラグ = 0
            mExit_flg = IdReaderMainProc()                              ' ＴＸまたはＴＹティーチング開始

            ' 補正クロスラインを非表示とする
            'V6.0.0.0④            Form1.CrosLineX.Visible = False
            'V6.0.0.0④            Form1.CrosLineY.Visible = False
            Form1.Instance.VideoLibrary1.SetCorrCrossVisible(False)     'V6.0.0.0④

            ' トラップエラー発生時
        Catch ex As Exception
            Dim strMSG As String = "frmIDReaderTeach.Execute() TRAP ERROR = " & ex.Message
            MsgBox(strMSG)
            mExit_flg = cERR_TRAP                                   ' Return値 = 例外エラー
        End Try

        Me.Close()
        Return mExit_flg                ' sGetReturn 取り込み   'V6.0.0.0⑬

    End Function
#End Region

#Region "OKﾎﾞﾀﾝ押下時処理"
    '''=========================================================================
    '''<summary>OKﾎﾞﾀﾝ押下時処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub cmdOK_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOK.Click

        stJOG.Flg = cFRS_ERR_START                                      ' OK(STARTｷｰ)

    End Sub
#End Region

#Region "ｷｬﾝｾﾙﾎﾞﾀﾝ押下時処理"
    '''=========================================================================
    '''<summary>ｷｬﾝｾﾙﾎﾞﾀﾝ押下時処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCancel.Click

        stJOG.Flg = cFRS_ERR_RST                           ' Cancel(RESETｷｰ)

    End Sub
#End Region

#Region "ＴＸまたはＴＹティーチングのメイン処理"
    '''=========================================================================
    '''<summary>ＴＸまたはＴＹティーチングのメイン処理"</summary>
    '''<returns>cFRS_ERR_START=OK(STARTｷｰ)
    '''         cFRS_ERR_RST  =Cancel(RESETｷｰ)
    '''         cFRS_TxTy     =TX2/TY2押下
    '''         -1以下        =エラー</returns>
    '''=========================================================================
    Private Function IdReaderMainProc() As Short

        Dim mdBpOffx As Double                              ' BP位置ｵﾌｾｯﾄX
        Dim mdBpOffy As Double                              ' BP位置ｵﾌｾｯﾄY
        Dim mdAdjx As Double                                ' ｱｼﾞｬｽﾄ位置X(未使用)
        Dim mdAdjy As Double                                ' ｱｼﾞｬｽﾄ位置Y(未使用)
        Dim dStepOffx As Double                             ' ｽﾃｯﾌﾟｵﾌｾｯﾄ量X
        Dim dStepOffy As Double                             ' ｽﾃｯﾌﾟｵﾌｾｯﾄ量Y
        Dim mdBSx As Double                                 ' ﾌﾞﾛｯｸｻｲｽﾞX
        Dim mdBSy As Double                                 ' ﾌﾞﾛｯｸｻｲｽﾞY
        Dim KJPosX As Double                                ' 先頭ブロックの先頭基準位置(BP位置X/ﾃｰﾌﾞﾙ移動座標X)
        Dim KJPosY As Double                                ' 先頭ブロックの先頭基準位置(BP位置Y/ﾃｰﾌﾞﾙ移動座標Y)
        Dim r As Short
        Dim StepNum As Integer = 0
        Dim strMSG As String

        Try
            '-------------------------------------------------------------------
            '   初期処理
            '-------------------------------------------------------------------
            mdBpOffx = typPlateInfo.dblBpOffSetXDir                     ' BP位置ｵﾌｾｯﾄX,Y設定
            mdBpOffy = typPlateInfo.dblBpOffSetYDir
            dStepOffx = typPlateInfo.dblStepOffsetXDir                  ' ｽﾃｯﾌﾟｵﾌｾｯﾄ量X,Y(TYﾃｨｰﾁ用)
            dStepOffy = typPlateInfo.dblStepOffsetYDir
            Call CalcBlockSize(mdBSx, mdBSy)                            ' ブロックサイズXY設定


            Call InitDisp()                                             ' 座標表示クリア
            r = Form1.System1.EX_MOVE(gSysPrm, 0.0, 0.0, 1)             ' BPを( 0.0 , 0.0 )に移動

            ' 矢印画面(BPのJOG操作)用パラメータを初期化する
            stJOG.Md = MODE_STG                                     ' モード(0:XYテーブル移動)
            stJOG.Md2 = MD2_BUTN                                        ' 入力モード(0:画面ﾎﾞﾀﾝ入力, 1:ｺﾝｿｰﾙ入力)
            '                                                           ' キーの有効(1)/無効(0)指定
            stJOG.Opt = CONSOLE_SW_RESET + CONSOLE_SW_START + CONSOLE_SW_HALT
            stJOG.PosX = KJPosX                                         ' BP X位置/XYﾃｰﾌﾞﾙ X位置
            stJOG.PosY = KJPosY                                         ' BP Y位置/XYﾃｰﾌﾞﾙ Y位置
            stJOG.BpOffX = mdAdjx + mdBpOffx                            ' BPｵﾌｾｯﾄX 
            stJOG.BpOffY = mdAdjy + mdBpOffy                            ' BPｵﾌｾｯﾄY 
            stJOG.BszX = mdBSx                                          ' ﾌﾞﾛｯｸｻｲｽﾞX 
            stJOG.BszY = mdBSy                                          ' ﾌﾞﾛｯｸｻｲｽﾞY
            stJOG.TextX = TxtPosX                                       ' BP X位置/XYﾃｰﾌﾞﾙ X位置表示用ﾃｷｽﾄﾎﾞｯｸｽ
            stJOG.TextY = TxtPosY                                       ' BP Y位置/XYﾃｰﾌﾞﾙ Y位置表示用ﾃｷｽﾄﾎﾞｯｸｽ
            stJOG.cgX = 0.0                                             ' 移動量X 
            stJOG.cgY = 0.0                                             ' 移動量Y 
            stJOG.BtnHI = BtnHI                                         ' HIボタン
            stJOG.BtnZ = BtnZ                                           ' Zボタン
            stJOG.BtnSTART = BtnSTART                                   ' STARTボタン
            stJOG.BtnHALT = BtnHALT                                     ' HALTボタン
            stJOG.BtnRESET = BtnRESET                                   ' RESETボタン
            Call JogEzInit(stJOG, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
            stJOG.Flg = -1                                              ' 親画面のOK/Cancelﾎﾞﾀﾝ押下ﾌﾗｸﾞ
            Call Me.Focus()

            '-------------------------------------------------------------------
            '   ＩＤリーダーのティーチング処理
            '-------------------------------------------------------------------
            r = Sub_Jog1()
            Timer1.Enabled = False
            If (r < cFRS_NORMAL) Then                                   ' エラーならエラーリターン
                Return (r)
            End If
            If (r <> cFRS_NORMAL) Then                                  ' 処理継続以外(RESET SW(Cancelﾎﾞﾀﾝ)押下/OKボタン押下)ならトリミングデータ更新へ
                GoTo STP_END
            End If

STP_END:
            '-------------------------------------------------------------------
            '   トリミングデータ更新
            '-------------------------------------------------------------------
            If (r <> cFRS_ERR_RST) Then                                 ' Cancel(RESET SW)押下以外 ?
                Call SetTrimParameter()
                gCmpTrimDataFlg = 1                                     ' データ更新フラグ = 1(更新あり)
            End If
            Return (r)                                                  ' Return値設定

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "frmIDReaderTeach.TxTyMainProc() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return値 = 例外エラー
        End Try
    End Function
#End Region

#Region "ＩＤリーダー位置ティーチング処理"

    '''=========================================================================
    ''' <summary>
    ''' ＩＤリーダー位置ティーチング処理
    ''' </summary>
    ''' <returns>cFRS_ERR_START = OK(STARTｷｰ)押下
    '''          cFRS_ERR_RST   = Cancel(RESETｷｰ)押下
    '''          cFRS_NORMAL    = 正常(グループ間インターバル処理へ)
    '''          -1以下         = エラー</returns>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Function Sub_Jog1() As Short

        Dim PosX As Double                                              ' 現在のBP位置X/ﾃｰﾌﾞﾙ移動座標X(絶対値)
        Dim PosY As Double                                              ' 現在のBP位置Y/ﾃｰﾌﾞﾙ移動座標Y(絶対値)
        Dim OffPosX As Double                                              ' 現在のBP位置X/ﾃｰﾌﾞﾙ移動座標X(絶対値)
        Dim OffPosY As Double                                              ' 現在のBP位置Y/ﾃｰﾌﾞﾙ移動座標Y(絶対値)
        Dim iPos As Short
        Dim r As Short
        Dim rtn As Short                                                ' 戻値 
        Dim strMSG As String
        Dim DirX As Integer = 1, DirY As Integer = 1

        Try
            ' 初期処理
            Timer1.Enabled = True
            iPos = 0                                                    ' iPos = 第1ＩＤリーダー位置(0:第1ＩＤリーダー位置位置, 1:第2ＩＤリーダー位置位置)
            IdReaderPos = iPos + 1
            Call ObjOmronIDReader.SetIDReaderNo(IdReaderPos)
            TrimClassCommon.GetDirectioinByBpDirXy(DirX, DirY)

            Me.lblTitle2.Text = LBL_IDREADER_TEACH_01

            stJOG.Flg = -1                                              ' 親画面のOK/Cancelﾎﾞﾀﾝ押下ﾌﾗｸﾞ
            Me.GrpFram_1.Visible = True                                 ' 第二基準点表示(Sub_Jog3()で非表示にするため)
            Call InitDisp()                                             ' 座標表示クリア

STP_RETRY:
            Call Me.Focus()
            Do
                ObjOmronIDReader.GetIDReaderPotision(gSysPrm.stDEV.gfTrimX, gSysPrm.stDEV.gfTrimY, typPlateInfo.dblTableOffsetXDir, typPlateInfo.dblTableOffsetYDir, gfCorrectPosX, gfCorrectPosY, OffPosX, OffPosY)
                If (iPos = 0) Then                                  ' 第1ＩＤリーダー位置
                    PosX = dblIDReadPos1X * DirX + OffPosX
                    PosY = dblIDReadPos1Y * DirY + OffPosY
                Else                                                ' 第2ＩＤリーダー位置
                    PosX = dblIDReadPos2X * DirX + OffPosX
                    PosY = dblIDReadPos2Y * DirY + OffPosY
                End If
                r = Form1.System1.XYtableMove(gSysPrm, PosX, PosY)
                If (r <> cFRS_NORMAL) Then                              ' エラー ?
                    Return (r)                                          ' エラーリターン
                End If

                ' 表示メッセージ等を設定する 
                If (iPos = 0) Then                                      ' 第1ＩＤリーダー位置 ? 
                    '"第1グループ、第1抵抗基準位置のティーチング"+"基準位置を合わせて下さい。"+"移動:[矢印]  決定:[START]  中断:[RESET]" "
                    lblInfo.Text = INFO_MSG19 & (iPos + 1).ToString(0) & LBL_IDREADER_TEACH_04 & vbCrLf & INFO_MSG16 & vbCrLf & INFO_MSG17
                    stJOG.TextX = TxtPosX                               ' 第1ＩＤリーダー位置座標X位置表示用ﾃｷｽﾄﾎﾞｯｸｽ
                    stJOG.TextY = TxtPosY                               ' 第1ＩＤリーダー位置座標Y位置表示用ﾃｷｽﾄﾎﾞｯｸｽ
                    TxtPosX.Enabled = True
                    TxtPosY.Enabled = True
                Else                                                    ' 第2ＩＤリーダー位置 
                    '"第nグループ、最終抵抗基準位置のティーチング"+"基準位置を合わせて下さい。"+"移動:[矢印]  決定:[START]  中断:[RESET]" & vbCrLf & "[HALT]で１つ前の処理に戻ります。"
                    lblInfo.Text = INFO_MSG19 & (iPos + 1).ToString(0) & LBL_IDREADER_TEACH_04 & vbCrLf & INFO_MSG16 & vbCrLf & INFO_MSG17
                    stJOG.TextX = TxtPos2X                              ' 第2ＩＤリーダー位置座標X位置表示用ﾃｷｽﾄﾎﾞｯｸｽ
                    stJOG.TextY = TxtPos2Y                              ' 第2ＩＤリーダー位置座標Y位置表示用ﾃｷｽﾄﾎﾞｯｸｽ
                    TxtPos2X.Enabled = True
                    TxtPos2Y.Enabled = True
                End If

                ' 第1ＩＤリーダー位置/第2ＩＤリーダー位置のティーチング"処理
                stJOG.PosX = PosX                                       ' BP XまたはXYﾃｰﾌﾞﾙ X絶対位置
                stJOG.PosY = PosY                                       ' BP YまたはXYﾃｰﾌﾞﾙ Y絶対位置
                stJOG.Flg = -1                                          ' 親画面のOK/Cancelﾎﾞﾀﾝ押下ﾌﾗｸﾞ
                'V6.0.0.0⑪                r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
                r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval, Me)  'V6.0.0.0⑪
                If (r < cFRS_NORMAL) Then                               ' エラーなら終了
                    Return (r)
                End If

                ' コンソールキーチェック
                If (r = cFRS_ERR_START) Then                            ' START SW押下 ?
                    ' ステップオフセット位置更新(第1ＩＤリーダー位置/第2ＩＤリーダー位置)
                    TrimClassCommon.GetDirectioinByBpDirXy(DirX, DirY)
                    If (iPos = 0) Then                                  ' 第1ＩＤリーダー位置
                        dblIDReadPos1X = dblIDReadPos1X + stJOG.cgX * DirX
                        dblIDReadPos1Y = dblIDReadPos1Y + stJOG.cgY * DirY
                    Else                                                ' 第2ＩＤリーダー位置
                        dblIDReadPos2X = dblIDReadPos2X + stJOG.cgX * DirX
                        dblIDReadPos2Y = dblIDReadPos2Y + stJOG.cgY * DirY
                    End If

                    If (iPos >= 1) Then                                 ' 第1ＩＤリーダー位置/第2ＩＤリーダー位置のﾃｨｰﾁﾝｸﾞ終了 ?
                        r = cFRS_NORMAL                             ' Return値 = グループ間インターバル処理へ 
                        Exit Do
                    Else
                        If (stJOG.Flg = -1) Then
                            iPos = 1                                    ' iPos = 第2ＩＤリーダー位置のﾃｨｰﾁﾝｸﾞ処理
                            IdReaderPos = iPos + 1
                            Call ObjOmronIDReader.SetIDReaderNo(IdReaderPos)
                        End If
                    End If

                    ' HALT SW押下時は１つ前のティーチングへ戻る
                ElseIf (r = cFRS_ERR_HALT) Then                         ' HALT SW押下 ?
                    If (iPos = 0) Then                                  ' 第1ＩＤリーダー位置のティーチングなら処理続行

                    Else                                                ' 第2ＩＤリーダー位置なら第1ＩＤリーダー位置のティーチングへ
                        iPos = 0                                        ' iPos = 第1ＩＤリーダー位置
                        IdReaderPos = iPos + 1
                        Call ObjOmronIDReader.SetIDReaderNo(IdReaderPos)
                    End If

                    '  RESET SW押下時は終了確認メッセージ表示へ
                ElseIf (r = cFRS_ERR_RST) Then                          ' RESET SW押下 ?
                    Exit Do
                End If

                If System.Windows.Forms.Form.ActiveForm IsNot Nothing Then
                    If System.Windows.Forms.Form.ActiveForm.Text <> "IDREADERTEACH" Then
                        Call ClearInpKey()
                    End If
                End If
                Call ZCONRST()                                          ' ｺﾝｿｰﾙｷｰﾗｯﾁ解除 
            Loop While (stJOG.Flg = -1)

            ' 当画面からOK/Cancelﾎﾞﾀﾝ押下ならrに戻値を設定する
            If (stJOG.Flg <> -1) Then
                r = stJOG.Flg
            End If

            '-------------------------------------------------------------------
            '   終了確認メッセージ表示 
            '-------------------------------------------------------------------
            ' "前の画面に戻ります。よろしいですか？　　　
            rtn = Form1.System1.TrmMsgBox(gSysPrm, MSG_105, MsgBoxStyle.OkCancel, LBL_IDREADER_TEACH_01)

            ' Cancel(RESETｷｰ)押下時は処理を継続する
            If (rtn = cFRS_ERR_RST) Then
                GoTo STP_RETRY                                          ' 処理継続へ
            End If

STP_END:
            Return (r)                                                  ' Return値設定

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "frmIDReaderTeach.Sub_Jog1() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return値 = 例外エラー
        End Try
    End Function
#End Region

    '=========================================================================
    '   トリミングパラメータの取得/更新処理
    '=========================================================================
#Region "トリミングデータより必要なパラメータを取得する(ＩＤリーダーティーチング)"
    '''=========================================================================
    '''<summary>トリミングデータより必要なパラメータを取得する</summary>
    '''<remarks>各インターバルデータを取得する</remarks>
    '''=========================================================================
    Private Sub SetTrimData()

        Dim strMSG As String

        Try
            '---------------------------------------------------------------
            '   ＩＤリード読み取りポジションを取得する。
            '---------------------------------------------------------------
            dblIDReadPos1X = typPlateInfo.dblIDReadPos1X
            dblIDReadPos1Y = typPlateInfo.dblIDReadPos1Y
            dblIDReadPos2X = typPlateInfo.dblIDReadPos2X
            dblIDReadPos2Y = typPlateInfo.dblIDReadPos2Y

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "frmIDReaderTeach.SetTrimData() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "トリミングパラメータ更新(ＩＤリーダーティーチング)"
    '''=========================================================================
    ''' <summary>
    '''トリミングパラメータ更新
    ''' </summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub SetTrimParameter()

        Dim OffSet As Double = 0.0
        Dim strMSG As String

        Try
            '---------------------------------------------------------------
            '   トリミングパラメータ更新(ＴＹティーチング時)
            '---------------------------------------------------------------
            typPlateInfo.dblIDReadPos1X = dblIDReadPos1X
            typPlateInfo.dblIDReadPos1Y = dblIDReadPos1Y
            typPlateInfo.dblIDReadPos2X = dblIDReadPos2X
            typPlateInfo.dblIDReadPos2Y = dblIDReadPos2Y

            Return

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "frmIDReaderTeach.SetTrimParameter() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

    '========================================================================================
    '   ボタン押下時処理
    '========================================================================================
#Region "HALTボタン押下時処理"
    '''=========================================================================
    '''<summary>HALTボタン押下時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnHALT_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnHALT.Click
        'Call SubBtnHALT_Click() ' '###265
    End Sub
#End Region

#Region "STARTボタン押下時処理"
    '''=========================================================================
    '''<summary>STARTボタン押下時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnSTART_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSTART.Click
        'Call SubBtnSTART_Click() ' '###265
    End Sub
#End Region

#Region "RESETボタン押下時処理"
    '''=========================================================================
    '''<summary>RESETボタン押下時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnRESET_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnRESET.Click
        'Call SubBtnRESET_Click() '###265
    End Sub
#End Region

    Private Sub BtnSTART_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnSTART.MouseDown
        Call SubBtnSTART_Click() ' '###265
    End Sub

    Private Sub BtnHALT_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnHALT.MouseDown
        Call SubBtnHALT_Click() ' '###265
    End Sub

    Private Sub BtnRESET_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnRESET.MouseDown
        Call SubBtnRESET_Click() ' '###265
    End Sub


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
        'Call SubBtnHI_Click(stJOG) ' V1.13.0.0⑭
    End Sub
#End Region
    Private Sub BtnHI_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnHI.MouseDown
        Call SubBtnHI_Click(stJOG) '###265
    End Sub

#Region "矢印ボタン押下時"
    '''=========================================================================
    '''<summary>矢印ボタン押下時</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub BtnJOG_0_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_0.MouseDown
        Timer1.Enabled = False '###228
        Call SubBtnJOG_0_MouseDown(stJOG) 'V6.0.0.0-22                                    ' +Y ON
    End Sub
    Private Sub BtnJOG_0_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_0.MouseUp
        Call SubBtnJOG_0_MouseUp(stJOG) 'V6.0.0.0-22                                      ' +Y OFF
        Timer1.Enabled = True '###228
    End Sub

    Private Sub BtnJOG_1_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_1.MouseDown
        Timer1.Enabled = False '###228
        Call SubBtnJOG_1_MouseDown(stJOG) 'V6.0.0.0-22                                    ' -Y ON
    End Sub
    Private Sub BtnJOG_1_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_1.MouseUp
        Call SubBtnJOG_1_MouseUp(stJOG) 'V6.0.0.0-22                                      ' -Y OFF
        Timer1.Enabled = True '###228
    End Sub
    Private Sub BtnJOG_2_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_2.MouseDown
        Timer1.Enabled = False '###228
        Call SubBtnJOG_2_MouseDown(stJOG) 'V6.0.0.0-22                                    ' +X ON
    End Sub
    Private Sub BtnJOG_2_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_2.MouseUp
        Call SubBtnJOG_2_MouseUp(stJOG) 'V6.0.0.0-22                                      ' +X OFF
        Timer1.Enabled = True '###228
    End Sub

    Private Sub BtnJOG_3_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_3.MouseDown
        Timer1.Enabled = False '###228
        Call SubBtnJOG_3_MouseDown(stJOG) 'V6.0.0.0-22                                    ' -X ON
    End Sub
    Private Sub BtnJOG_3_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_3.MouseUp
        Call SubBtnJOG_3_MouseUp(stJOG) 'V6.0.0.0-22                                      ' -X OFF
        Timer1.Enabled = True '###228
    End Sub

    Private Sub BtnJOG_4_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_4.MouseDown
        Timer1.Enabled = False '###228
        Call SubBtnJOG_4_MouseDown(stJOG) 'V6.0.0.0-22                                    ' -X -Y ON
    End Sub
    Private Sub BtnJOG_4_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_4.MouseUp
        Call SubBtnJOG_4_MouseUp(stJOG) 'V6.0.0.0-22                                      ' -X -Y OFF
        Timer1.Enabled = True '###228
    End Sub

    Private Sub BtnJOG_5_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_5.MouseDown
        Timer1.Enabled = False '###228
        Call SubBtnJOG_5_MouseDown(stJOG) 'V6.0.0.0-22                                    ' +X -Y ON
    End Sub
    Private Sub BtnJOG_5_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_5.MouseUp
        Call SubBtnJOG_5_MouseUp(stJOG) 'V6.0.0.0-22                                      ' +X -Y OFF
        Timer1.Enabled = True '###228
    End Sub

    Private Sub BtnJOG_6_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_6.MouseDown
        Timer1.Enabled = False '###228
        Call SubBtnJOG_6_MouseDown(stJOG) 'V6.0.0.0-22                                    ' +X +Y ON
    End Sub
    Private Sub BtnJOG_6_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_6.MouseUp
        Call SubBtnJOG_6_MouseUp(stJOG) 'V6.0.0.0-22                                      ' +X +Y OFF
        Timer1.Enabled = True '###228
    End Sub

    Private Sub BtnJOG_7_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_7.MouseDown
        Timer1.Enabled = False '###228
        Call SubBtnJOG_7_MouseDown(stJOG) 'V6.0.0.0-22                                    ' -X +Y ON
    End Sub
    Private Sub BtnJOG_7_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_7.MouseUp
        Call SubBtnJOG_7_MouseUp(stJOG) 'V6.0.0.0-22                                      ' -X +Y OFF
        Timer1.Enabled = True '###228
    End Sub
#End Region

    '========================================================================================
    '   テンキー入力処理
    '========================================================================================
#Region "キーダウン時処理"
    '''=========================================================================
    '''<summary>キーダウン時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub frmIDReaderTeach_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        JogKeyDown(e)                   'V6.0.0.0⑪
    End Sub

    Public Sub JogKeyDown(ByVal e As KeyEventArgs) Implements ICommonMethods.JogKeyDown    'V6.0.0.0⑪
        'V6.0.0.0⑪        Dim KeyCode As Short = e.KeyCode
        Dim KeyCode As Keys = e.KeyCode                 'V6.0.0.0⑪
        Console.WriteLine("frmIDReaderTeach.frmIDReaderTeach_KeyDown()")

        ' テンキーダウンならInpKeyにテンキーコードを設定する
        'V6.0.0.0⑫        GrpMain.Enabled = False                                         ' ###085
        'V6.0.0.0⑫       'Call Sub_10KeyDown(KeyCode)
        Sub_10KeyDown(KeyCode, stJOG)             'V6.0.0.0⑫
        If (KeyCode = System.Windows.Forms.Keys.NumPad5) Then           ' 5ｷｰ (KeyCode = 101(&H65)
            Call SubBtnHI_Click(stJOG)                                  ' V1.13.0.0⑭
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
    Private Sub frmIDReaderTeach_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        JogKeyUp(e)                     'V6.0.0.0⑪
    End Sub

    Public Sub JogKeyUp(ByVal e As KeyEventArgs) Implements ICommonMethods.JogKeyUp    'V6.0.0.0⑪
        'V6.0.0.0⑪        Dim KeyCode As Short = e.KeyCode
        Dim KeyCode As Keys = e.KeyCode                 'V6.0.0.0⑪

        Console.WriteLine("frmIDReaderTeach.frmIDReaderTeach_KeyKeyUp()")
        ' テンキーアップならInpKeyのテンキーコードをOFFする
        'V6.0.0.0⑫        GrpMain.Enabled = True                                          ' ###085
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
    '   トラックバー処理
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

#Region "キーステータス取得"
    '''=========================================================================
    ''' <summary>キーステータス取得</summary>
    ''' <param name="Code">(INP)キーコード</param>
    ''' <returns></returns>
    '''=========================================================================
    Private Function Sub_GetKeyState(ByVal Code As Integer) As Integer

        Dim keyState As Integer

        keyState = GetKeyState(Code)
        Return (keyState)
    End Function
#End Region

#Region "キーステータスチェックタイマー"
    '''=========================================================================
    ''' <summary>キーステータスチェックタイマー</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
#If False Then                          'V6.0.0.0-22
        Dim KeyCode As Integer
        Dim keyState As Integer
        Dim Count As Integer
        Dim strMSG As String
        Try
            Timer1.Enabled = False

            KeyCode = System.Windows.Forms.Keys.NumPad1
            For Count = 1 To 9                                              ' 1-9まで繰り返す 
                keyState = Sub_GetKeyState(KeyCode)
                If (keyState < 0) Then
                    'Call Sub_10KeyDown(KeyCode)
                    'If (KeyCode = System.Windows.Forms.Keys.NumPad5) Then   ' 5ｷｰ (KeyCode = 101(&H65)
                    '    Call BtnHI_Click(sender, e)                         ' HIボタン ON/OFF
                    'End If
                Else
                    ' テンキーアップならInpKeyのテンキーコードをOFFする
                    'V6.0.0.0⑫                    GrpMain.Enabled = True
                    'V6.0.0.0⑫        Call Sub_10KeyUp(KeyCode)
                    Sub_10KeyUp(KeyCode, buttons)                   'V6.0.0.0⑫
                    'V6.0.0.0⑫                    Call Me.Focus()
                End If
                KeyCode = KeyCode + 1
            Next

            'strMSG = "Keys.NumPad9=" + keyState.ToString("")
            'Console.WriteLine(strMSG)
            'MsgBox(strMSG, MsgBoxStyle.OkOnly)

            Timer1.Enabled = True

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "frmIDReaderTeach.Timer1_Tick() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
#End If
    End Sub
#End Region

#Region "ＩＤリーダー読み込み処理"
    '''=========================================================================
    ''' <summary>
    ''' ＩＤリーダー読み込み処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub btnRead_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnRead.MouseDown
        '   End Sub
        '    Private Sub btnRead_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRead.Click
        Dim cData As String = ""
        Try

            If Not ObjOmronIDReader.IDRead(IdReaderPos, cData) Then
                ' エラー処理
            End If

            TextIdReadData.Text = cData
        Catch ex As Exception
            MsgBox("frmIDReaderTeach.btnRead_Click() TRAP ERROR = " + ex.Message)
        End Try

    End Sub
#End Region

#Region "オムロン製ＩＤリーダー用ツールタッチファインダーの起動"
    '''=========================================================================
    ''' <summary>
    ''' オムロン製ＩＤリーダー用ツールタッチファインダーの起動
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub btnTouchFinder_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnTouchFinder.MouseDown
        'End Sub
        'Private Sub btnTouchFinder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTouchFinder.Click
        'Const SWP_NOSIZE As Integer = &H1
        'Const SWP_NOMOVE As Integer = &H2
        'Const SWP_NOACTIVATE As Integer = &H10
        'Const SWP_SHOWWINDOW As Integer = &H40
        Try

            'Me.TopMost = False

            Dim ps As System.Diagnostics.Process() = System.Diagnostics.Process.GetProcessesByName("TouchFinder")
            If 0 < ps.Length Then
                If TouchFinderProcessId = ps(0).Id Then
                    Microsoft.VisualBasic.Interaction.AppActivate(ps(0).Id)
                End If
                Exit Sub
            End If

            TouchFinderProcess.Start()
            'Dim pName As String = TouchFinderProcess.ProcessName()
            TouchFinderProcessId = TouchFinderProcess.Id

            'SetWindowPos(TouchFinderProcess.MainWindowHandle, IntPtr.Zero, 0, 0, 0, 0, _
            '        'SWP_NOACTIVATE Or SWP_NOMOVE Or SWP_NOSIZE Or SWP_SHOWWINDOW)


            'Me.TopMost = False
        Catch ex As Exception
            MsgBox("frmIDReaderTeach.btnTouchFinder_Click() TRAP ERROR = " + ex.Message)
        End Try

    End Sub
#End Region
    Public Sub ToucFinderUp()
        Try

            Dim ps As System.Diagnostics.Process() = System.Diagnostics.Process.GetProcessesByName("TouchFinder")
            If 0 < ps.Length Then
                ' If TouchFinderProcessId = ps(0).Id Then
                Microsoft.VisualBasic.Interaction.AppActivate(ps(0).Id)
                'End If
            End If
            Exit Sub
        Catch ex As Exception
            ' エラーは表示しない            MsgBox("frmIDReaderTeach.ToucFinderUp() TRAP ERROR = " + ex.Message)
        End Try
    End Sub

    Private Sub btnTouchFinder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTouchFinder.Click

    End Sub
End Class

'=============================== END OF FILE ===============================