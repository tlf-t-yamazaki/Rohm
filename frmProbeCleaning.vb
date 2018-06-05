'===============================================================================
'   Description  : プローブクリーニング画面処理
'
'   Copyright(C) : OMRON LASERFRONT INC. 2015
'
'===============================================================================
Option Strict Off
Option Explicit On
'
Imports LaserFront.Trimmer.DefWin32Fnc                                  'V6.0.0.1②
Imports LaserFront.Trimmer.DefTrimFnc
Imports LaserFront.Trimmer.TrimData.DataManager                         'V5.0.0.8①
Imports TKY_ALL_SL432HW.My.Resources                                    'V4.4.0.0-0

Public Class frmProbeCleaning
    Inherits System.Windows.Forms.Form
    Implements ICommonMethods                                           'V6.0.0.0⑪

    Private Declare Function GetKeyState Lib "user32" (ByVal nVirtKey As Integer) As Integer 'V4.0.0.0-53

#Region "【定数・変数定義】"
    '===========================================================================
    '   定数・変数定義
    '===========================================================================
    Private stJOG As JOG_PARAM                                          ' 矢印画面(JOG操作)用パラメータ
    Private dblTchMoval(3) As Double                                    ' ﾋﾟｯﾁ退避域(0=ﾋﾟｯﾁ, 1=HIGHﾋﾟｯﾁ, 2=Pause Time(Sec))
    'V6.0.0.1②                          ↓                               Public -> Private, Const -> ReadOnly
    Private gdblCleaningPosX As Double
    Private gdblCleaningPosY As Double
    'V6.0.0.0⑪    Public gdblCleaningPosZ As Double
    Private Shared gdblCleaningPosZ As Double       'V6.0.0.0⑪
    'V6.0.0.0⑪    Public ProbeCleaningCnt As Integer
    Public Shared ProbeCleaningCnt As Integer       'V6.0.0.0⑪
    Private mExit_flg As Integer

    Private Const CLEANING_OFFSET As Integer = 1
    '----- V4.0.0.0-30↓ -----
    ' プローブ接触回数
    Private ReadOnly CONTACT_MIN As Integer = 0
    Private ReadOnly CONTACT_MAX As Integer = 99
    ' クリーニング自動実行間隔
    Private ReadOnly PROBING_MIN As Integer = 0
    Private ReadOnly PROBING_MAX As Integer = 32767
    '----- V4.0.0.0-30↑ -----
    'V4.10.0.0⑨↓
    Private ReadOnly DISTANCE_MIN As Double = 0.0
    Private ReadOnly DISTANCE_MAX As Double = 10.0
    Private ReadOnly CLEANINGOFFSET_MIN As Double = -10.0
    Private ReadOnly CLEANINGOFFSET_MAX As Double = 10.0
    Private ReadOnly PITCH_MIN As Double = 0.0
    Private ReadOnly PITCH_MAX As Double = 1.0
    Private ReadOnly MOVECNT_MIN As Integer = 0
    Private ReadOnly MOVECNT_MAX As Integer = 99
    Dim savedblPrbDistance As Double                                    ' プローブ間距離（mm）
    Dim savedblPrbCleaningOffset As Double                              ' クリーニングオフセット(mm)
    Dim savedblPrbCleanStagePitchX As Double                            ' ステージ動作ピッチ
    Dim savedblPrbCleanStagePitchY As Double                            ' ステージ動作ピッチ
    Dim saveintPrbCleanStageCountX As Integer                           ' ステージ動作回数
    Dim saveintPrbCleanStageCountY As Integer                           ' ステージ動作回数
    'V4.10.0.0⑨↑

    Private _tenKeyON As Boolean                                        'V6.0.0.0⑯  'V6.0.0.1② _tenKeyFlg -> _tenKeyON
    'V6.0.0.1②                          ↑
#End Region

#Region "【メソッド定義】"

#Region "コンストラクタ"
    ''' <summary>コンストラクタ</summary>
    ''' <param name="dblPrbCleanPosX">typPlateInfo.dblPrbCleanPosX</param>
    ''' <param name="dblPrbCleanPosY">typPlateInfo.dblPrbCleanPosY</param>
    ''' <remarks>'V6.0.0.0⑪</remarks>
    Public Sub New(ByVal dblPrbCleanPosX As Double, ByVal dblPrbCleanPosY As Double)

        ' この呼び出しはデザイナーで必要です。
        InitializeComponent()

        ' InitializeComponent() 呼び出しの後で初期化を追加します。

        gdblCleaningPosX = dblPrbCleanPosX
        gdblCleaningPosY = dblPrbCleanPosY

        'V6.0.0.1②                      ↓
        CONTACT_MIN = Integer.Parse(GetPrivateProfileString_S("MIN_MAX", "PBCLN_CONTACT_MIN", TKY_INI, "0"))
        CONTACT_MAX = Integer.Parse(GetPrivateProfileString_S("MIN_MAX", "PBCLN_CONTACT_MAX", TKY_INI, "99"))
        PROBING_MIN = Integer.Parse(GetPrivateProfileString_S("MIN_MAX", "PBCLN_PROBING_MIN", TKY_INI, "0"))
        PROBING_MAX = Integer.Parse(GetPrivateProfileString_S("MIN_MAX", "PBCLN_PROBING_MAX", TKY_INI, "32767"))
        DISTANCE_MIN = Double.Parse(GetPrivateProfileString_S("MIN_MAX", "PBCLN_DISTANCE_MIN", TKY_INI, "0.0"))
        DISTANCE_MAX = Double.Parse(GetPrivateProfileString_S("MIN_MAX", "PBCLN_DISTANCE_MAX", TKY_INI, "10.0"))
        CLEANINGOFFSET_MIN = Double.Parse(GetPrivateProfileString_S("MIN_MAX", "PBCLN_OFFSET_MIN", TKY_INI, "-10.0"))
        CLEANINGOFFSET_MAX = Double.Parse(GetPrivateProfileString_S("MIN_MAX", "PBCLN_OFFSET_MAX", TKY_INI, "10.0"))
        PITCH_MIN = Double.Parse(GetPrivateProfileString_S("MIN_MAX", "PBCLN_PITCH_MIN", TKY_INI, "0.0"))
        PITCH_MAX = Double.Parse(GetPrivateProfileString_S("MIN_MAX", "PBCLN_PITCH_MAX", TKY_INI, "1.0"))
        MOVECNT_MIN = Integer.Parse(GetPrivateProfileString_S("MIN_MAX", "PBCLN_MOVECNT_MIN", TKY_INI, "0"))
        MOVECNT_MAX = Integer.Parse(GetPrivateProfileString_S("MIN_MAX", "PBCLN_MOVECNT_MAX", TKY_INI, "99"))
        'V6.0.0.1②                      ↑
    End Sub
#End Region
    '========================================================================================
    '   画面処理
    '========================================================================================
#Region "フォームが表示されたときのメイン処理"
#If False Then                          'V6.0.0.0⑬ Execute()でおこなう
    '''=========================================================================
    ''' <summary>
    ''' フォームが表示されたときのメイン処理 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub frmProbeCleaning_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim strMSG As String

        Try
            ' ティーチング開始
            If (mExit_flg <> -1) Then
                Me.GrpMain.Enabled = True 'V4.0.0.0-80
                Exit Sub
            End If

            mExit_flg = 0                                           ' 終了フラグ = 0
            mExit_flg = ProbCleanMainProc()                              ' ＴＸまたはＴＹティーチング開始

            ' 補正クロスラインを非表示とする
            'V6.0.0.0④            Form1.CrosLineX.Visible = False
            'V6.0.0.0④            Form1.CrosLineY.Visible = False
            Form1.Instance.VideoLibrary1.SetCorrCrossVisible(False)     'V6.0.0.0④

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "frmTxTeach.frmTxTeach_Activated() TRAP ERROR = " + ex.Message
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
            ' ティーチング開始
            If (mExit_flg <> -1) Then
                'V6.0.0.0⑫                Me.GrpMain.Enabled = True 'V4.0.0.0-80
                Exit Function
            End If

            _tenKeyON = False           'V6.0.0.1②
            mExit_flg = 0                                           ' 終了フラグ = 0
            mExit_flg = ProbCleanMainProc()                              ' ＴＸまたはＴＹティーチング開始

            ' 補正クロスラインを非表示とする
            'V6.0.0.0④            Form1.CrosLineX.Visible = False
            'V6.0.0.0④            Form1.CrosLineY.Visible = False
            Form1.Instance.VideoLibrary1.SetCorrCrossVisible(False)     'V6.0.0.0④

            ' トラップエラー発生時
        Catch ex As Exception
            Dim strMSG As String = "frmTxTeach.Execute() TRAP ERROR = " & ex.Message
            MsgBox(strMSG)
            mExit_flg = cERR_TRAP                                   ' Return値 = 例外エラー
        End Try

        Me.Close()
        Return cFRS_NORMAL              ' sGetReturn 取り込み   'V6.0.0.0⑬

    End Function
#End Region

#Region "プローブクリーニング機能画面表示"
    '''=========================================================================
    ''' <summary>
    ''' プローブクリーニング機能画面表示 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub frmProbeCleaning_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim stPos As System.Drawing.Point
        Dim stGetPos As System.Drawing.Point

        'スタートスイッチラッチクリア   
        Call ZCONRST()

        giJogButtonEnable = 0 'V4.0.0.0-78

        mExit_flg = -1
        ' 表示位置の調整
        stPos = Form1.Text4.PointToScreen(stGetPos)
        stPos.X = stPos.X - 2
        stPos.Y = stPos.Y - 2
        Me.Location = stPos
        Me.KeyPreview = True 'V4.0.0.0-53

    End Sub

    ''' <summary></summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>'V6.0.0.0⑫</remarks>
    Private Sub frmProbeCleaning_Shown(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Shown

        ' Form.Load() は Show()時とShowDialog()時とで一度・都度と発生状況が異なるため、呼ばれ方に影響を受けないShown()でおこなう
        stJOG.TenKey = New Button() {BtnJOG_0, BtnJOG_1, BtnJOG_2, BtnJOG_3,
                                     BtnJOG_4, BtnJOG_5, BtnJOG_6, BtnJOG_7, BtnHI}
    End Sub
#End Region

#Region "ステージ登録位置への移動ボタン"
    '''=========================================================================
    ''' <summary>
    ''' 登録位置への移動ボタン 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub btnStageRegPosMove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim PosX As Double
        Dim PosY As Double
        Dim r As Integer
        Dim rtnCode As Integer
        Dim strMSG As String

        Try

            PosX = CDbl(txtRegPosX.Text)
            PosY = CDbl(txtRegPosY.Text)

            r = SMOVE2(PosX, PosY)
            If (r <> cFRS_NORMAL) Then                                  ' エラー ? 
                rtnCode = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0) ' メッセージ表示
            End If
        Catch ex As Exception
            strMSG = "frmProbeCleaning.btnStageRegPosMove_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return
        End Try
    End Sub
#End Region

#Region "登録してあるZ軸座標への移動ボタン"
    '''=========================================================================
    ''' <summary>
    ''' 登録してあるZ軸座標への移動 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub btnZRegMove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim PosZ As Double
        Dim ret As Integer
        Dim strMSG As String

        Try

            PosZ = CDbl(txtRegPosZ.Text)

            ' Zを待機位置へ移動する
            ret = EX_ZMOVE(PosZ, MOVE_ABSOLUTE)
            ' エラーはEX_MOVE内で表示済み
            If (ret <> cFRS_NORMAL) Then                                  ' エラー ?

            End If

        Catch ex As Exception
            strMSG = "frmProbeCleaning.btnZRegMove_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return
        End Try

    End Sub
#End Region

#Region "プローブクリーニング機能メインループ処理"
    '''=========================================================================
    ''' <summary>
    ''' プローブクリーニング機能メインループ処理 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Function ProbCleanMainProc() As Integer

        Dim GrpNum As Short                                 ' ｸﾞﾙｰﾌﾟ数
        Dim RnBn As Short                                   ' チップ数(抵抗数)(TX時), ブロック数(TY時)
        Dim GrpCnt As Short                                 ' ｸﾞﾙｰﾌﾟ数ｶｳﾝﾀ
        Dim ChipSize As Double                              ' 補正後のﾁｯﾌﾟｻｲｽﾞ
        Dim tmpChipSize As Double                           ' ﾁｯﾌﾟｻｲｽﾞ退避域
        Dim CSPoint(1) As Double                            ' ﾁｯﾌﾟｻｲｽﾞ算出用(0:第1基準点, 1:第2基準点)
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
        Dim wkContactCount As Integer = 0                               ' V4.0.0.0-30
        Dim wkProbingCount As Integer = 0                               ' V4.0.0.0-30
        Dim br As Boolean = True                                        ' V4.0.0.0-30

        Try

            '-------------------------------------------------------------------
            '   初期処理
            '-------------------------------------------------------------------
            ' JOGパラメータ設定 
            stJOG.Md = MODE_STG                                     ' モード(STAGE移動)
            stJOG.Md2 = MD2_BUTN                                    ' 入力モード(0:画面ﾎﾞﾀﾝ入力, 1:ｺﾝｿｰﾙ入力)
            '                                                       ' キーの有効(1)/無効(0)指定
            'stJOG.Opt = CONSOLE_SW_RESET + CONSOLE_SW_START
            stJOG.Opt = CONSOLE_SW_RESET + CONSOLE_SW_START + CONSOLE_SW_ZSW
            stJOG.PosX = 0.0                                        ' BP X位置(BPｵﾌｾｯﾄX)
            stJOG.PosY = 0.0                                        ' BP Y位置(BPｵﾌｾｯﾄY)
            stJOG.BpOffX = 0                                        ' BPｵﾌｾｯﾄX 
            stJOG.BpOffY = 0                                        ' BPｵﾌｾｯﾄY 
            stJOG.BszX = 0                                          ' ﾌﾞﾛｯｸｻｲｽﾞX 
            stJOG.BszY = 0                                          ' ﾌﾞﾛｯｸｻｲｽﾞY
            stJOG.TextX = txtStagePosX                              ' BP X位置表示用ﾃｷｽﾄﾎﾞｯｸｽ
            stJOG.TextY = txtStagePosY                              ' BP Y位置表示用ﾃｷｽﾄﾎﾞｯｸｽ
            stJOG.cgX = 0                                           ' 移動量X (BPｵﾌｾｯﾄX)
            stJOG.cgY = 0                                           ' 移動量Y (BPｵﾌｾｯﾄY)
            stJOG.BtnHI = BtnHI                                     ' HIボタン
            stJOG.BtnZ = BtnZ                                       ' Zボタン
            stJOG.BtnSTART = BtnSTART                               ' STARTボタン
            stJOG.BtnRESET = BtnRESET                               ' RESETボタン
            stJOG.BtnHALT = BtnHALT                                 ' HALTボタン
            Call JogEzInit(stJOG, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
            stJOG.Flg = -1                                          ' 親画面のOK/Cancelﾎﾞﾀﾝ押下ﾌﾗｸﾞ
            stJOG.bZ = False                                        ' JogのZキー状態 = Z Off
            Call LAMP_CTRL(LAMP_Z, False)

            '-------------------------------------------------------------------
            '   初期処理
            '-------------------------------------------------------------------
            ' グループ数, ブロック数(TY時)またはチップ数(抵抗数)(TX時), チップサイズを取得する
            r = GetChipNumAndSize(giAppMode, GrpNum, RnBn, ChipSize)
            tmpChipSize = ChipSize                                      ' チップサイズ退避
            GrpCnt = 0                                                  ' ｸﾞﾙｰﾌﾟ数ｶｳﾝﾀ
            mdAdjx = 0.0 : mdAdjy = 0.0                                 ' ｱｼﾞｬｽﾄ位置X(未使用)
            mdBpOffx = typPlateInfo.dblBpOffSetXDir                     ' BP位置ｵﾌｾｯﾄX,Y設定
            mdBpOffy = typPlateInfo.dblBpOffSetYDir
            dStepOffx = typPlateInfo.dblStepOffsetXDir                  ' ｽﾃｯﾌﾟｵﾌｾｯﾄ量X,Y(TYﾃｨｰﾁ用)
            dStepOffy = typPlateInfo.dblStepOffsetYDir
            Call CalcBlockSize(mdBSx, mdBSy)                            ' ブロックサイズXY設定
            '----- V2.0.0.0_30↓ -----
            'V4.10.0.0⑨            txtRegPosX.Text = CStr(typPlateInfo.dblPrbCleanPosX)
            'V4.10.0.0⑨            txtRegPosY.Text = CStr(typPlateInfo.dblPrbCleanPosY)
            'V4.10.0.0⑨            txtRegPosZ.Text = CStr(typPlateInfo.dblPrbCleanPosZ)
            txtRegPosX.Text = typPlateInfo.dblPrbCleanPosX.ToString("0.0000")
            txtRegPosY.Text = typPlateInfo.dblPrbCleanPosY.ToString("0.0000")
            txtRegPosZ.Text = typPlateInfo.dblPrbCleanPosZ.ToString("0.000")
            txtProbingCnt.Text = CStr(typPlateInfo.intPrbCleanAutoSubCount) ' プロービング間隔
            txtContactCount.Text = CStr(typPlateInfo.intPrbCleanUpDwCount)  ' プロービング回数 
            '----- V2.0.0.0_30↑ ----
            'V4.10.0.0⑨↓
            savedblPrbDistance = typPlateInfo.dblPrbDistance                ' プローブ間距離（mm）
            savedblPrbCleaningOffset = typPlateInfo.dblPrbCleaningOffset    ' クリーニングオフセット(mm)
            savedblPrbCleanStagePitchX = typPlateInfo.dblPrbCleanStagePitchX    'ステージ動作ピッチ
            savedblPrbCleanStagePitchY = typPlateInfo.dblPrbCleanStagePitchY    'ステージ動作ピッチ
            saveintPrbCleanStageCountX = typPlateInfo.intPrbCleanStageCountX    'ステージ動作回数
            saveintPrbCleanStageCountY = typPlateInfo.intPrbCleanStageCountY    'ステージ動作回数
            If Form1.stFNC(F_PROBE_CLEANING).iDEF = 1 Then               ' = 1 プローブアップダウン
                txtOption1_Y.Enabled = False
                txtOption1_Y.Visible = False
                txtOption2_Y.Enabled = False
                txtOption2_Y.Visible = False
                LblOption2_1.Visible = False
                LblOption2_2.Visible = False
                LblOption1_1.Visible = True
                LblOption1_2.Visible = True
                'V6.0.0.1②                txtOption2_X.Text = CStr(typPlateInfo.dblPrbCleaningOffset)     ' クリーニングオフセット(mm)
                'V6.0.0.1②                txtOption1_X.Text = CStr(typPlateInfo.dblPrbDistance)           ' プローブ間距離（mm）
                txtOption1_X.Text = typPlateInfo.dblPrbDistance.ToString("0.000")       ' プローブ間距離（mm）
                txtOption2_X.Text = typPlateInfo.dblPrbCleaningOffset.ToString("0.000") ' クリーニングオフセット(mm)
            Else                                ' = 2 ステージ移動
                txtOption1_Y.Enabled = True
                txtOption1_Y.Visible = True
                txtOption2_Y.Enabled = True
                txtOption2_Y.Visible = True
                LblOption2_1.Visible = True
                LblOption2_2.Visible = True
                LblOption1_1.Visible = False
                LblOption1_2.Visible = False
                'V6.0.0.1②                txtOption1_X.Text = CStr(typPlateInfo.dblPrbCleanStagePitchX)       'ステージ動作ピッチＸ
                'V6.0.0.1②                txtOption1_Y.Text = CStr(typPlateInfo.dblPrbCleanStagePitchY)       'ステージ動作ピッチＹ
                'V6.0.0.1②                txtOption2_X.Text = CStr(typPlateInfo.intPrbCleanStageCountX)       'ステージ動作ピッチＸ
                'V6.0.0.1②                txtOption2_Y.Text = CStr(typPlateInfo.intPrbCleanStageCountY)       'ステージ動作ピッチＹ
                txtOption1_X.Text = typPlateInfo.dblPrbCleanStagePitchX.ToString("0.000")       'ステージ動作ピッチＸ
                txtOption1_Y.Text = typPlateInfo.dblPrbCleanStagePitchY.ToString("0.000")       'ステージ動作ピッチＹ
                txtOption2_X.Text = typPlateInfo.intPrbCleanStageCountX.ToString("0.000")       'ステージ動作ピッチＸ
                txtOption2_Y.Text = typPlateInfo.intPrbCleanStageCountY.ToString("0.000")       'ステージ動作ピッチＹ
            End If
            'V4.10.0.0⑨↑
            '----- V4.0.0.0-53↓ -----
            Me.txtStagePosX.Text = txtRegPosX.Text
            Me.txtStagePosY.Text = txtRegPosY.Text
            Me.txtZPos.Text = txtRegPosZ.Text
            CleaningPosZ = typPlateInfo.dblPrbCleanPosZ
            '----- V4.0.0.0-53↑ -----
STP_RETRY:
            'Call InitDisp()                                             ' 座標表示クリア
            ChipSize = tmpChipSize                                      ' チップサイズ設定

            r = Form1.System1.BpCenter(gSysPrm)                         ' BPセンターへ移動
            If (r < cFRS_NORMAL) Then
                Return (r)                                              ' Return値 = エラー
            End If

            ' 矢印画面(BPのJOG操作)用パラメータを初期化する
            stJOG.Md = MODE_STG                                         ' モード(0:XYテーブル移動)

            stJOG.Md2 = MD2_BUTN                                        ' 入力モード(0:画面ﾎﾞﾀﾝ入力, 1:ｺﾝｿｰﾙ入力)
            '                                                           ' キーの有効(1)/無効(0)指定
            stJOG.Opt = CONSOLE_SW_RESET + CONSOLE_SW_START + CONSOLE_SW_HALT
            stJOG.PosX = KJPosX                                         ' BP X位置/XYﾃｰﾌﾞﾙ X位置
            stJOG.PosY = KJPosY                                         ' BP Y位置/XYﾃｰﾌﾞﾙ Y位置
            stJOG.BpOffX = mdAdjx + mdBpOffx                            ' BPｵﾌｾｯﾄX 
            stJOG.BpOffY = mdAdjy + mdBpOffy                            ' BPｵﾌｾｯﾄY 
            stJOG.BszX = mdBSx                                          ' ﾌﾞﾛｯｸｻｲｽﾞX 
            stJOG.BszY = mdBSy                                          ' ﾌﾞﾛｯｸｻｲｽﾞY
            stJOG.TextX = txtStagePosX                                  ' BP X位置/XYﾃｰﾌﾞﾙ X位置表示用ﾃｷｽﾄﾎﾞｯｸｽ
            stJOG.TextY = txtStagePosY                                  ' BP Y位置/XYﾃｰﾌﾞﾙ Y位置表示用ﾃｷｽﾄﾎﾞｯｸｽ
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

STP_CHIPSIZE:
            '-------------------------------------------------------------------
            '   チップサイズＸ(TX時)またはチップサイズＹ(TY時)のティーチング処理
            '   ※CSPoint(0:第1基準点, 1:第2基準点)にBPまたはテーブル位置を設定する
            '-------------------------------------------------------------------
            ' チップサイズのティーチング処理
            r = Sub_Jog1()
            Timer1.Enabled = False                                      ' V4.0.0.0-53
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
                '----- V4.0.0.0-30↓ -----
                ' 入力データチェック(プローブ接触回数)
                br = DataCheck_Int(txtContactCount.Text, LblContactCount.Text, CONTACT_MIN, CONTACT_MAX, wkContactCount)
                If (br = False) Then
                    txtContactCount.Text = CStr(typPlateInfo.intPrbCleanUpDwCount)
                    txtContactCount.Focus()
                    GoTo STP_CHIPSIZE
                End If
                ' 入力データチェック(クリーニング自動実行間隔)
                br = DataCheck_Int(txtProbingCnt.Text, LblProbingCnt.Text, PROBING_MIN, PROBING_MAX, wkProbingCount)
                If (br = False) Then
                    txtProbingCnt.Text = CStr(typPlateInfo.intPrbCleanAutoSubCount)
                    txtProbingCnt.Focus()
                    GoTo STP_CHIPSIZE
                End If
                '----- V4.0.0.0-30↑ -----
                'V4.10.0.0⑨↓
                If Form1.stFNC(F_PROBE_CLEANING).iDEF = 1 Then               ' = 1 プローブアップダウン
                    ' プローブ間距離（mm）
                    br = DataCheck_Double(txtOption1_X.Text, LblOption1_1.Text, DISTANCE_MIN, DISTANCE_MAX, typPlateInfo.dblPrbDistance)
                    txtOption1_X.Text = typPlateInfo.dblPrbDistance.ToString("0.0")
                    If (br = False) Then
                        txtOption1_X.Focus()
                        GoTo STP_CHIPSIZE
                    End If
                    ' クリーニングオフセット(mm)
                    br = DataCheck_Double(txtOption2_X.Text, LblOption1_2.Text, CLEANINGOFFSET_MIN, CLEANINGOFFSET_MAX, typPlateInfo.dblPrbCleaningOffset)
                    txtOption2_X.Text = typPlateInfo.dblPrbCleaningOffset.ToString("0.0")
                    If (br = False) Then
                        txtOption2_X.Focus()
                        GoTo STP_CHIPSIZE
                    End If
                Else                                ' = 2 ステージ移動
                    'ステージ動作ピッチ
                    br = DataCheck_Double(txtOption1_X.Text, LblOption2_1.Text, PITCH_MIN, PITCH_MAX, typPlateInfo.dblPrbCleanStagePitchX)
                    txtOption1_X.Text = typPlateInfo.dblPrbCleanStagePitchX.ToString("0.0")
                    If (br = False) Then
                        txtOption1_X.Focus()
                        GoTo STP_CHIPSIZE
                    End If
                    br = DataCheck_Double(txtOption1_Y.Text, LblOption2_1.Text, PITCH_MIN, PITCH_MAX, typPlateInfo.dblPrbCleanStagePitchY)
                    txtOption1_Y.Text = typPlateInfo.dblPrbCleanStagePitchY.ToString("0.0")
                    If (br = False) Then
                        txtOption1_Y.Focus()
                        GoTo STP_CHIPSIZE
                    End If
                    'ステージ動作回数
                    br = DataCheck_Int(txtOption2_X.Text, LblOption2_2.Text, MOVECNT_MIN, MOVECNT_MAX, typPlateInfo.intPrbCleanStageCountX)
                    txtOption2_X.Text = typPlateInfo.intPrbCleanStageCountX.ToString("0")
                    If (br = False) Then
                        txtOption2_X.Focus()
                        GoTo STP_CHIPSIZE
                    End If
                    br = DataCheck_Int(txtOption2_Y.Text, LblOption2_2.Text, MOVECNT_MIN, MOVECNT_MAX, typPlateInfo.intPrbCleanStageCountY)
                    txtOption2_Y.Text = typPlateInfo.intPrbCleanStageCountY.ToString("0")
                    If (br = False) Then
                        txtOption2_Y.Focus()
                        GoTo STP_CHIPSIZE
                    End If
                End If
                'V4.10.0.0⑨↑
                '----- V2.0.0.0_30↓ -----
                ' トリミングデータ更新
                typPlateInfo.dblPrbCleanPosX = gdblCleaningPosX
                typPlateInfo.dblPrbCleanPosY = gdblCleaningPosY
                typPlateInfo.dblPrbCleanPosZ = gdblCleaningPosZ
                typPlateInfo.intPrbCleanUpDwCount = wkContactCount       ' プロービング回数
                typPlateInfo.intPrbCleanAutoSubCount = wkProbingCount    ' プロービング間隔
                'typPlateInfo.intPrbCleanUpDwCount = CLng(txtContactCount.Text)       ' プロービング回数
                'typPlateInfo.intPrbCleanAutoSubCount = CInt(txtProbingCnt.Text)      ' プロービング間隔
                '----- V2.0.0.0_30↑ -----
                gCmpTrimDataFlg = 1                                     ' データ更新フラグ = 1(更新あり)
            End If
            Return (r)                                                  ' Return値設定

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "frmTxTyTeach.TxTyMainProc() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return値 = 例外エラー
        End Try

    End Function
#End Region

#Region "プローブティーチングJog動作処理"
    '''=========================================================================
    ''' <summary>
    ''' プローブティーチングJog動作処理
    ''' </summary>
    ''' <returns>cFRS_ERR_START = OK(STARTｷｰ)押下
    '''          cFRS_ERR_RST   = Cancel(RESETｷｰ)押下
    '''          cFRS_NORMAL    = 正常(グループ間インターバル処理へ)
    '''          -1以下         = エラー</returns>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Function Sub_Jog1() As Short

        Dim r As Short
        Dim rtn As Short                                                ' 戻値 
        Dim strMSG As String
        Dim DirX As Integer = 1, DirY As Integer = 1
        'V6.0.0.0⑪        Dim stJogTextZ As Object

        Try
            ' 初期処理
            'Me.LblHead01.Text = LBL_PROBECLEANING_TEACH                 ' "プローブクリーニング位置ティーチング" V2.0.0.0⑰
            stJOG.Flg = -1                                              ' 親画面のOK/Cancelﾎﾞﾀﾝ押下ﾌﾗｸﾞ
            Timer1.Enabled = True                                       ' V4.0.0.0-53

            'V4.3.0.0③
            globaldblCleaningPosX = typPlateInfo.dblPrbCleanPosX
            globaldblCleaningPosY = typPlateInfo.dblPrbCleanPosY
            'V4.3.0.0③

            ' 現在の登録位置へステージの移動を行う。 
            r = MoveCleaningPosXY(0.0, 0.0)                             'V4.10.0.0⑨  OffsetX,Y追加
            If (r <> cFRS_NORMAL) Then                                  ' エラー ?
                Return (r)                                              ' エラーリターン
            End If
            cmbMoveMode.SelectedIndex = 0                               ' 移動モード = ステージ V4.0.0.0-53

STP_RETRY:
            Call Me.Focus()
            'cmbMoveMode.SelectedIndex = 0                               ' 移動モード = ステージ V4.0.0.0-53

            ' ティーチング"処理
            stJOG.PosX = gdblCleaningPosX                               ' BP XまたはXYﾃｰﾌﾞﾙ X絶対位置
            stJOG.PosY = gdblCleaningPosY                               ' BP YまたはXYﾃｰﾌﾞﾙ Y絶対位置

            stJOG.TextX = txtStagePosX                                  ' 座標X位置表示用ﾃｷｽﾄﾎﾞｯｸｽ
            stJOG.TextY = txtStagePosY                                  ' 座標Y位置表示用ﾃｷｽﾄﾎﾞｯｸｽ
            Me.txtZPos.Text = CleaningPosZ.ToString("0.000")            ' V4.0.0.0-53
            'V6.0.0.0⑪            stJogTextZ = txtZPos                                        ' Z座標表示用 V4.0.0.0-53

            '' 表示メッセージ等を設定する 'V4.0.0.0-30
            ''"プローブクリーニングのステージ位置を合わせて下さい。"+"移動:[矢印]  決定:[START]  中断:[RESET]" "
            'LblMsg.Text = MSG_160 + vbCrLf + MSG_161 'V4.0.0.0-30

            'V6.0.0.0⑯            If (gbTenKeyFlg) Then
            If (_tenKeyON) Then         'V6.0.0.0⑯
                Form1.Instance.SetActiveJogMethod(AddressOf Me.JogKeyDown,
                                                  AddressOf Me.JogKeyUp,
                                                  AddressOf Me.MoveToCenter)    'V6.0.0.0⑪
            Else
                Form1.Instance.SetActiveJogMethod(Nothing,
                                                  Nothing,
                                                  AddressOf Me.MoveToCenter)    'V6.0.0.0⑪
            End If

            Do
                'V6.0.0.0⑪                r = JogEzMoveWithZ(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval, stJogTextZ)
                r = JogEzMoveWithZ(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause,
                                   LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval,
                                   AddressOf DispProbePos)              'V6.0.0.0⑪
                If (r < cFRS_NORMAL) Then                               ' エラーなら終了
                    Return (r)
                End If

                ' コンソールキーチェック
                If (r = cFRS_ERR_START) Then                            ' START SW押下 
                    'V4.0.0.0-80
                    'gdblCleaningPosX = gdblCleaningPosX + stJOG.cgX
                    'gdblCleaningPosY = gdblCleaningPosY + stJOG.cgY
                    'gdblCleaningPosZ = CleaningPosZ
                    'V4.0.0.0-80
                    Exit Do

                    ' HALT SW押下時は１つ前のティーチングへ戻る
                ElseIf (r = cFRS_ERR_HALT) Then                         ' HALT SW押下 ?

                    '  RESET SW押下時は終了確認メッセージ表示へ
                ElseIf (r = cFRS_ERR_RST) Then                          ' RESET SW押下 ?
                    'V4.10.0.0⑨↓
                    typPlateInfo.dblPrbDistance = savedblPrbDistance                ' プローブ間距離（mm）
                    typPlateInfo.dblPrbCleaningOffset = savedblPrbCleaningOffset   ' クリーニングオフセット(mm)
                    typPlateInfo.dblPrbCleanStagePitchX = savedblPrbCleanStagePitchX 'ステージ動作ピッチ
                    typPlateInfo.dblPrbCleanStagePitchY = savedblPrbCleanStagePitchY 'ステージ動作ピッチ
                    typPlateInfo.intPrbCleanStageCountX = saveintPrbCleanStageCountX 'ステージ動作回数
                    typPlateInfo.intPrbCleanStageCountY = saveintPrbCleanStageCountY 'ステージ動作回数
                    'V4.10.0.0⑨↑
                    Exit Do

                    ' Z軸リミットエラー時に軸がOnした状態のままボタンの表示が[Z Off]となり
                    ' ボタンのEnabledもFalseとなってしまうためフラグの設定を追加
                ElseIf (r = cFRS_ERR_Z) Then        'V6.0.0.0⑲
                    stJOG.bZ = (Not stJOG.bZ)
                    'V6.0.0.1②          ↓
                    Double.TryParse(txtStagePosX.Text, gdblCleaningPosX)
                    Double.TryParse(txtStagePosY.Text, gdblCleaningPosY)
                    stJOG.PosX = gdblCleaningPosX                       ' XYﾃｰﾌﾞﾙ X絶対位置
                    stJOG.PosY = gdblCleaningPosY                       ' XYﾃｰﾌﾞﾙ Y絶対位置
                    If (MODE_STG = iGlobalJogMode) Then
                        stJOG.Opt = stJOG.Opt And (Not CONSOLE_SW_ZSW)
                    End If
                    'V6.0.0.1②          ↑
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
            rtn = Form1.System1.TrmMsgBox(gSysPrm, MSG_105, MsgBoxStyle.OkCancel, Me.LblHead01.Text)
            'V4.3.0.0③
            '----- V4.0.0.0-53↓ -----
            'If (iGlobalJogMode = MODE_STG) Then
            'V6.0.0.1②            gdblCleaningPosX = gdblCleaningPosX + stJOG.cgX
            'V6.0.0.1②            gdblCleaningPosY = gdblCleaningPosY + stJOG.cgY
            Double.TryParse(txtStagePosX.Text, gdblCleaningPosX)        'V6.0.0.1②
            Double.TryParse(txtStagePosY.Text, gdblCleaningPosY)        'V6.0.0.1②
            'Else
            gdblCleaningPosZ = CleaningPosZ
            'End If
            '----- V4.0.0.0-53↑ -----
            'V4.3.0.0③
            ' Cancel(RESETｷｰ)押下時は処理を継続する
            If (rtn = cFRS_ERR_RST) Then
                'V4.3.0.0③
                '----- V4.0.0.0-53↓ -----
                'If (iGlobalJogMode = MODE_STG) Then
                '    gdblCleaningPosX = gdblCleaningPosX + stJOG.cgX
                '    gdblCleaningPosY = gdblCleaningPosY + stJOG.cgY
                'Else
                '    gdblCleaningPosZ = CleaningPosZ
                'End If
                '----- V4.0.0.0-53↑ -----
                'V4.3.0.0③
                stJOG.bZ = False                                        'V6.0.0.1②
                stJOG.Opt = stJOG.Opt And (Not CONSOLE_SW_ZSW)          'V6.0.0.1②
                cmbMoveMode.SelectedIndex = 0                           'V4.10.0.0⑨
                GoTo STP_RETRY                                          ' 処理継続へ
            End If

STP_END:
            Return (r)                                                  ' Return値設定

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "frmIDReaderTeach.Sub_Jog1() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return値 = 例外エラー

        Finally                         'V6.0.0.0⑪
            Form1.Instance.SetActiveJogMethod(Nothing, Nothing, Nothing)
        End Try
    End Function
#End Region

#Region "プローブクリーニング位置登録画面での動作モード変更"
    '''=========================================================================
    ''' <summary>
    ''' プローブクリーニング位置登録画面での動作モード変更
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub cmbMoveMode_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMoveMode.SelectedIndexChanged
        Dim r As Integer

        If cmbMoveMode.SelectedIndex = 0 Then
            iGlobalJogMode = MODE_STG
            txtZPos.BackColor = System.Drawing.SystemColors.Control
            txtStagePosX.BackColor = System.Drawing.Color.Yellow 'V4.0.0.0-53
            txtStagePosY.BackColor = System.Drawing.Color.Yellow 'V4.0.0.0-53
            '----- V4.0.0.0-30↓ -----
            '"プローブクリーニングのステージ位置を合わせて下さい。"+"移動:[矢印]  決定:[START]  中断:[RESET]" "
            LblMsg.Text = MSG_160 + vbCrLf + MSG_161
            '----- V4.0.0.0-30↑ -----
            'V4.0.0.0-78
            r = MoveOrgPosZ()
            If (r <> cFRS_NORMAL) Then                                  ' エラー ? 
                r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0) ' メッセージ表示
            End If
            'V6.0.0.1②            BtnZ.BackColor = Color.LightGray
            BtnZ.BackColor = SystemColors.Control                       'V6.0.0.1②
            giJogButtonEnable = 0
            BtnZ.Enabled = False
            'V4.0.0.0-78

            BtnZ.Text = "Z Off"                                         'V6.0.0.0⑲
            'V6.0.0.1②            stJOG.Opt = stJOG.Opt Or (Not CONSOLE_SW_ZSW)               'V6.0.0.0⑲
            stJOG.bZ = False                                            'V6.0.0.1②
        Else
            iGlobalJogMode = MODE_Z
            txtZPos.BackColor = System.Drawing.Color.Yellow 'V4.0.0.0-53
            txtStagePosX.BackColor = System.Drawing.SystemColors.Control
            txtStagePosY.BackColor = System.Drawing.SystemColors.Control
            '----- V4.0.0.0-30↓ -----
            '"プローブクリーニングのプローブ位置を合わせて下さい。"+"移動:[矢印]  決定:[START]  中断:[RESET]" "
            LblMsg.Text = MSG_162 + vbCrLf + MSG_161
            '----- V4.0.0.0-30↑ -----
            BtnZ.Enabled = True 'V4.0.0.0-78
            stJOG.Opt = (stJOG.Opt Or CONSOLE_SW_ZSW)                   'V6.0.0.0⑲
        End If

    End Sub
#End Region

#Region "プローブクリーニングの実行ボタン"
    '''=========================================================================
    ''' <summary>
    ''' プローブクリーニングの実行ボタン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub btnCleaningStart_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnCleaningStart.MouseUp

        Dim strMSG As String
        Dim wkContactCount As Integer = 0                               ' V4.0.0.0-30
        Dim wkProbingCount As Integer = 0                               ' V4.0.0.0-30
        Dim br As Boolean = True                                        ' V4.0.0.0-30
        Dim Ret As Integer

        Try
            'V6.0.0.1②                  ↓
            btnCleaningStart.Parent.Select()
            btnCleaningStart.Enabled = False
            cmdCancel.Enabled = False
            cmdOK.Enabled = False
            GrpArrow.Enabled = False
            BtnTenKey.Enabled = False
            Form1.Instance.VideoLibrary1.SetTrackBar(True, False)       'V6.0.1.0⑦
            Application.DoEvents()
            'V6.0.0.1②                  ↑

            '----- V4.0.0.0-30↓ -----
            ' 入力データチェック(プローブ接触回数)
            br = DataCheck_Int(txtContactCount.Text, LblContactCount.Text, CONTACT_MIN, CONTACT_MAX, wkContactCount)
            If (br = False) Then
                txtContactCount.Text = CStr(typPlateInfo.intPrbCleanUpDwCount)
                txtContactCount.Focus()
                Return
            End If
            ' 入力データチェック(クリーニング自動実行間隔)
            br = DataCheck_Int(txtProbingCnt.Text, LblProbingCnt.Text, PROBING_MIN, PROBING_MAX, wkProbingCount)
            If (br = False) Then
                txtProbingCnt.Text = CStr(typPlateInfo.intPrbCleanAutoSubCount)
                txtProbingCnt.Focus()
                Return
            End If

            ProbeCleaningCnt = wkContactCount
            'ProbeCleaningCnt = CLng(txtContactCount.Text)
            '----- V4.0.0.0-30↑ -----
            'V4.10.0.0⑨↓
            If Form1.stFNC(F_PROBE_CLEANING).iDEF = 1 Then               ' = 1 プローブアップダウン
                ' プローブ間距離（mm）
                br = DataCheck_Double(txtOption1_X.Text, LblOption1_1.Text, DISTANCE_MIN, DISTANCE_MAX, typPlateInfo.dblPrbDistance)
                'V5.0.0.8⑤                txtOption1_X.Text = typPlateInfo.dblPrbDistance.ToString("0.0")
                txtOption1_X.Text = typPlateInfo.dblPrbDistance.ToString("0.000")
                If (br = False) Then
                    txtOption1_X.Focus()
                    Return
                End If
                ' クリーニングオフセット(mm)
                br = DataCheck_Double(txtOption2_X.Text, LblOption1_2.Text, CLEANINGOFFSET_MIN, CLEANINGOFFSET_MAX, typPlateInfo.dblPrbCleaningOffset)
                'V5.0.0.8⑤                txtOption2_X.Text = typPlateInfo.dblPrbCleaningOffset.ToString("0.0")
                txtOption2_X.Text = typPlateInfo.dblPrbCleaningOffset.ToString("0.000")
                If (br = False) Then
                    txtOption2_X.Focus()
                    Return
                End If
            Else                                ' = 2 ステージ移動
                'ステージ動作ピッチ
                br = DataCheck_Double(txtOption1_X.Text, LblOption2_1.Text, PITCH_MIN, PITCH_MAX, typPlateInfo.dblPrbCleanStagePitchX)
                'V5.0.0.8⑤                txtOption1_X.Text = typPlateInfo.dblPrbCleanStagePitchX.ToString("0.0")
                txtOption1_X.Text = typPlateInfo.dblPrbCleanStagePitchX.ToString("0.000")
                If (br = False) Then
                    txtOption1_X.Focus()
                    Return
                End If
                br = DataCheck_Double(txtOption1_Y.Text, LblOption2_1.Text, PITCH_MIN, PITCH_MAX, typPlateInfo.dblPrbCleanStagePitchY)
                'V5.0.0.8⑤                txtOption1_Y.Text = typPlateInfo.dblPrbCleanStagePitchY.ToString("0.0")
                txtOption1_Y.Text = typPlateInfo.dblPrbCleanStagePitchY.ToString("0.000")
                If (br = False) Then
                    txtOption1_Y.Focus()
                    Return
                End If
                'ステージ動作回数
                br = DataCheck_Int(txtOption2_X.Text, LblOption2_2.Text, MOVECNT_MIN, MOVECNT_MAX, typPlateInfo.intPrbCleanStageCountX)
                txtOption2_X.Text = typPlateInfo.intPrbCleanStageCountX.ToString("0")
                If (br = False) Then
                    txtOption2_X.Focus()
                    Return
                End If
                br = DataCheck_Int(txtOption2_Y.Text, LblOption2_2.Text, MOVECNT_MIN, MOVECNT_MAX, typPlateInfo.intPrbCleanStageCountY)
                txtOption2_Y.Text = typPlateInfo.intPrbCleanStageCountY.ToString("0")
                If (br = False) Then
                    txtOption2_Y.Focus()
                    Return
                End If
            End If
            'V4.10.0.0⑨↑
            'V4.3.0.0③
            If (Double.TryParse(Me.txtStagePosX.Text, globaldblCleaningPosX) = False) Then
                strMSG = "frmProbeCleaning.btnCleaningStart_MouseUp() X-Pos ERROR = "
                MsgBox(strMSG)
                Return                  'V6.0.0.1②
            End If
            If (Double.TryParse(Me.txtStagePosY.Text, globaldblCleaningPosY) = False) Then
                strMSG = "frmProbeCleaning.btnCleaningStart_MouseUp() Y-Pos ERROR = "
                MsgBox(strMSG)
                Return                  'V6.0.0.1②
            End If
            If (Double.TryParse(Me.txtZPos.Text, globaldblCleaningPosZ) = False) Then
                strMSG = "frmProbeCleaning.btnCleaningStart_MouseUp() Y-Pos ERROR = "
                MsgBox(strMSG)
                Return                  'V6.0.0.1②
            End If

            'V4.3.0.0③

            Ret = ProbeCleaningStart()
            'V6.0.0.1②                  ↓
            ' プローブクリーニング開始位置にステージを戻す
            Ret = MoveCleaningPosXY(0.0, 0.0)
            giJogButtonEnable = 0
            stJOG.bZ = False
            'V6.0.0.1②                  ↑
            Return

        Catch ex As Exception
            strMSG = "frmProbeCleaning.btnCleaningStart_MouseUp() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)

        Finally                         'V6.0.0.1②
            Application.DoEvents()
            Form1.Instance.VideoLibrary1.SetTrackBar(True, True)        'V6.0.1.0⑦
            BtnTenKey.Enabled = True
            GrpArrow.Enabled = True
            cmdOK.Enabled = True
            cmdCancel.Enabled = True
            btnCleaningStart.Enabled = True
        End Try

    End Sub
#End Region

#Region "データチェック(Integer型)"
    '''=========================================================================
    ''' <summary>データチェック(Integer型) V4.0.0.0-30</summary>
    ''' <param name="strVal">(INP)入力データ</param>
    ''' <param name="strLbl">(INP)入力データのラベル名</param>
    ''' <param name="MinVal">(INP)最小値</param>
    ''' <param name="MaxVal">(INP)最大値</param>
    ''' <param name="Val">   (OUT)チェック後のデータを返す</param>
    ''' <returns>True=正常, False=エラー</returns>
    '''=========================================================================
    Public Function DataCheck_Int(ByRef strVal As String, ByRef strLbl As String, ByVal MinVal As Integer, ByVal MaxVal As Integer, ByRef Val As Integer) As Boolean

        Dim strMSG As String = ""
        Dim wkVal As Integer = 0

        Try
            ' 入力データチェック
            If (Integer.TryParse(strVal, wkVal) = False) OrElse _
                (MinVal > wkVal) OrElse (MaxVal < wkVal) Then
                GoTo STP_ERR1
            End If

            ' チェック後のデータを返す
            Val = wkVal
            Return (True)

            ' データチェックエラー
STP_ERR1:
            'If (gSysPrm.stTMN.giMsgTyp = 0) Then
            '    strMSG = "数値は" & MinVal.ToString("0") & "～" & MaxVal.ToString("0") & "の範囲で入力して下さい。" + " (" + strLbl + ")"
            'Else
            '    strMSG = "Please specify a value within the limits of " & MinVal.ToString("0") & " and " & MaxVal.ToString("0") + " (" + strLbl + ")"
            'End If
            strMSG = String.Format(frmProbeCleaning_001, MinVal.ToString("0"), MaxVal.ToString("0"), strLbl)
            MsgBox(strMSG)
            Return (False)

        Catch ex As Exception
            strMSG = "frmProbeCleaning.DataCheck_Int() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (False)
        End Try
    End Function
#End Region

    'V4.10.0.0⑨↓
#Region "データチェック(Double型)"
    '''=========================================================================
    ''' <summary>データチェック(Double型) V4.0.0.0-30</summary>
    ''' <param name="strVal">(INP)入力データ</param>
    ''' <param name="strLbl">(INP)入力データのラベル名</param>
    ''' <param name="MinVal">(INP)最小値</param>
    ''' <param name="MaxVal">(INP)最大値</param>
    ''' <param name="Val">   (OUT)チェック後のデータを返す</param>
    ''' <returns>True=正常, False=エラー</returns>
    '''=========================================================================
    Public Function DataCheck_Double(ByRef strVal As String, ByRef strLbl As String, ByVal MinVal As Double, ByVal MaxVal As Double, ByRef Val As Double) As Boolean

        Dim strMSG As String = ""
        Dim wkVal As Double = 0

        Try
            ' 入力データチェック
            If (Double.TryParse(strVal, wkVal) = False) OrElse _
                (MinVal > wkVal) OrElse (MaxVal < wkVal) Then
                GoTo STP_ERR1
            End If

            ' チェック後のデータを返す
            Val = wkVal
            Return (True)

            ' データチェックエラー
STP_ERR1:
            strMSG = String.Format(frmProbeCleaning_001, MinVal.ToString("0.0"), MaxVal.ToString("0.0"), strLbl)
            MsgBox(strMSG)
            Return (False)

        Catch ex As Exception
            strMSG = "frmProbeCleaning.DataCheck_Double() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (False)
        End Try
    End Function
#End Region

#Region "プローブクリーニング機能の実行"
    '''=========================================================================
    ''' <summary>
    ''' プローブの下降上昇の実行 
    ''' </summary>
    ''' <param name="CountTimes">ダウンアップの回数</param>
    ''' <param name="OrgX">ステージの現在位置Ｘ</param>
    ''' <param name="OrgY">ステージの現在位置Ｙ</param>
    ''' <param name="TimesX">Ｘ方向にステップする回数</param>
    ''' <param name="TimesY">Ｙ方向にステップする回数</param>
    ''' <param name="MoveX">Ｘ方向にステップする距離</param>
    ''' <param name="MoveY">Ｙ方向にステップする距離</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Shared Function ProbeDownAndUp(ByVal CountTimes As Integer, ByVal OrgX As Double, ByVal OrgY As Double, ByVal TimesX As Integer, ByVal TimesY As Integer, ByVal MoveX As Double, ByVal MoveY As Double) As Integer      'V6.0.0.0⑪
        'V6.0.0.0⑪    Public Function ProbeDownAndUp(ByVal CountTimes As Integer, ByVal OrgX As Double, ByVal OrgY As Double, ByVal TimesX As Integer, ByVal TimesY As Integer, ByVal MoveX As Double, ByVal MoveY As Double) As Integer

        Dim Ret As Integer
        Dim strMSG As String
        Dim i As Integer
        Dim CntX As Integer, CntY As Integer

        Try

            If TimesX > 0 Or TimesY > 0 Then
                Ret = PROBOFF_EX(globaldblCleaningPosZ)
                If Ret <> 0 Then
                    Return (Ret)
                End If
            End If
            ' 指定回数プローブの上下を繰り返す 
            For i = 1 To CountTimes
                ' プローブクリーニング位置へZ軸位置設定 
                Ret = MoveCleaningPosZ()                                    ' プローブクリーニング位置へZ軸位置設定 'V4.3.0.0④
                If Ret <> 0 Then
                    Return (Ret)
                End If
                Application.DoEvents()                                          'V6.0.0.1②
                Call System.Threading.Thread.Sleep(500)

                CntX = 0
                CntY = 0
                Do While (MoveX > 0.0 And CntX < TimesX) Or (MoveY > 0.0 And CntY < TimesY)
                    If CntX < TimesX Then                                   ' Ｘ方向ステージ動作ピッチへ移動
                        Ret = MoveCleaningPosXY(MoveX, 0.0)
                        If Ret <> 0 Then
                            Return (Ret)
                        End If
                        Application.DoEvents()                                  'V6.0.0.1②
                        Call System.Threading.Thread.Sleep(300)
                    End If

                    If CntY < TimesY Then                                   ' Ｙ方向ステージ動作ピッチへ移動
                        Ret = MoveCleaningPosXY(0.0, MoveY)
                        If Ret <> 0 Then
                            Return (Ret)
                        End If
                        Application.DoEvents()                                  'V6.0.0.1②
                        Call System.Threading.Thread.Sleep(300)
                    End If

                    Ret = MoveCleaningPosXY(OrgX, OrgY)                     ' 元の位置に移動
                    If Ret <> 0 Then
                        Return (Ret)
                    End If
                    Application.DoEvents()                                      'V6.0.0.1②

                    CntX = CntX + 1
                    CntY = CntY + 1
                Loop

                Ret = MoveOrgWaitPosZ()                                     ' 原点位置へZ軸位置設定 
                If Ret <> 0 Then
                    Return (Ret)
                End If
                Application.DoEvents()                                          'V6.0.0.1②
                Call System.Threading.Thread.Sleep(500)

            Next i

            If TimesX > 0 Or TimesY > 0 Then
                Ret = PROBOFF_EX(typPlateInfo.dblZWaitOffset)
                If Ret <> 0 Then
                    Return (Ret)
                End If
            End If
        Catch ex As Exception
            strMSG = "frmProbeCleaning.ProbeDownAndUp() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try

    End Function
#End Region
    'V4.10.0.0⑨↑

#Region "プローブクリーニング機能の実行"
    '''=========================================================================
    ''' <summary>
    ''' プローブクリーニング機能の実行 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Shared Function ProbeCleaningStart() As Integer          'V6.0.0.0⑪
        'V6.0.0.0⑪    Public Function ProbeCleaningStart() As Integer

        Dim Ret As Integer
        Dim strMSG As String
        Dim PosX As Double, PosY As Double, MoveX As Double, MoveY As Double

        Try
            'V4.10.0.0⑨ 更新↓
            MoveX = 0.0
            MoveY = 0.0
            ' プローブクリーニング位置へステージの移動
            Ret = MoveCleaningPosXY(MoveX, MoveY)
            If Ret <> 0 Then
                Return (Ret)
            End If

            If Form1.stFNC(F_PROBE_CLEANING).iDEF = 1 Then   ' = 1 アップダウンモード
                If typPlateInfo.intResistDir = 0 Then   ' 抵抗(ﾁｯﾌﾟ)並び方向(0:X, 1:Y)
                    PosX = 0.0
                    PosY = 1.0
                Else
                    PosX = 1.0
                    PosY = 0.0
                End If
                ' 最初のプローブピン
                ' 正規の位置でアップダウン
                Ret = ProbeDownAndUp(ProbeCleaningCnt, MoveX, MoveY, 0, 0, 0.0, 0.0)
                If Ret <> 0 Then
                    Return (Ret)
                End If

                ' クリーニングオフセット分ステージ移動
                If typPlateInfo.dblPrbCleaningOffset <> 0.0 Then
                    MoveX = PosX * typPlateInfo.dblPrbCleaningOffset
                    MoveY = PosY * typPlateInfo.dblPrbCleaningOffset
                    Ret = MoveCleaningPosXY(MoveX, MoveY)
                    If Ret <> 0 Then
                        Return (Ret)
                    End If
                    Ret = ProbeDownAndUp(ProbeCleaningCnt, MoveX, MoveY, 0, 0, 0.0, 0.0)
                    If Ret <> 0 Then
                        Return (Ret)
                    End If
                End If

                ' ２番目のプローブピン
                If typPlateInfo.dblPrbDistance > 0.0 Then
                    MoveX = PosX * typPlateInfo.dblPrbDistance
                    MoveY = PosY * typPlateInfo.dblPrbDistance
                    Ret = MoveCleaningPosXY(MoveX, MoveY)
                    If Ret <> 0 Then
                        Return (Ret)
                    End If
                    Ret = ProbeDownAndUp(ProbeCleaningCnt, MoveX, MoveY, 0, 0, 0.0, 0.0)
                    If Ret <> 0 Then
                        Return (Ret)
                    End If

                    ' クリーニングオフセット分ステージ移動
                    If typPlateInfo.dblPrbCleaningOffset <> 0.0 Then
                        MoveX = PosX * (typPlateInfo.dblPrbCleaningOffset + typPlateInfo.dblPrbDistance)
                        MoveY = PosY * (typPlateInfo.dblPrbCleaningOffset + typPlateInfo.dblPrbDistance)
                        Ret = MoveCleaningPosXY(MoveX, MoveY)
                        If Ret <> 0 Then
                            Return (Ret)
                        End If
                        Ret = ProbeDownAndUp(ProbeCleaningCnt, MoveX, MoveY, 0, 0, 0.0, 0.0)
                        If Ret <> 0 Then
                            Return (Ret)
                        End If
                    End If
                End If          ' ←２番目のプローブピン

            Else                    ' = 2 ステージ移動
                Ret = ProbeDownAndUp(ProbeCleaningCnt, MoveX, MoveY, typPlateInfo.intPrbCleanStageCountX, typPlateInfo.intPrbCleanStageCountY, typPlateInfo.dblPrbCleanStagePitchX, typPlateInfo.dblPrbCleanStagePitchY)
                If Ret <> 0 Then
                    Return (Ret)
                End If
            End If
            'V4.10.0.0⑨ 更新↑

            'V4.10.0.0⑨コメント化↓
            ' プローブクリーニング位置へステージの移動
            'Ret = MoveCleaningPosXY()
            'If Ret <> 0 Then
            '    Return (Ret)
            'End If

            ' 指定回数プローブの上下を繰り返す 
            'For i = 0 To ProbeCleaningCnt - 1
            '    '                For i = 0 To typPlateInfo.intPrbCleanUpDwCount - 1
            '    Call System.Threading.Thread.Sleep(500)                 ' プローブクリーニング位置へZ軸位置設定 'V4.3.0.0④
            '    ' プローブクリーニング位置へZ軸位置設定 
            '    Ret = MoveCleaningPosZ()
            '    If Ret <> 0 Then
            '        Return (Ret)
            '    End If
            '    Call System.Threading.Thread.Sleep(500)                 ' プローブクリーニング位置へZ軸位置設定 
            '    Ret = MoveOrgWaitPosZ()
            '    If Ret <> 0 Then
            '        Return (Ret)
            '    End If

            'Next i
            'V4.10.0.0⑨コメント化↑

        Catch ex As Exception
            strMSG = "frmProbeCleaning.ProbeCleaningStart() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try

    End Function
#End Region

#Region "ステージをプローブクリーニング位置へ移動"
    '''=========================================================================
    ''' <summary>
    ''' ステージをプローブクリーニング位置へ移動 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    '''=========================================================================
    '''V4.10.0.0⑨    Public Function MoveCleaningPosXY() As Integer
    Private Shared Function MoveCleaningPosXY(ByVal OffsetX As Double, ByVal OffsetY As Double) As Integer   'V6.0.0.0⑪
        'V6.0.0.0⑪        Public Function MoveCleaningPosXY(ByVal OffsetX As Double, ByVal OffsetY As Double) As Integer
        Dim PosX As Double
        Dim PosY As Double
        Dim Ret As Integer
        Dim strMSG As String

        Try
            '----- V2.0.0.0_30↓ -----
            'PosX = typPlateInfo.dblPrbCleanPosX
            'PosY = typPlateInfo.dblPrbCleanPosY
            'V4.10.0.0⑨            PosX = globaldblCleaningPosX
            'V4.10.0.0⑨            PosY = globaldblCleaningPosY
            PosX = globaldblCleaningPosX + OffsetX      'V4.10.0.0⑨
            PosY = globaldblCleaningPosY + OffsetY      'V4.10.0.0⑨
            '----- V2.0.0.0_30↑ -----
            '----- V4.0.0.0-53↓ -----
            'If (Double.TryParse(Me.txtStagePosX.Text, PosX) = False) Then
            '    Return (cFRS_NORMAL)
            'End If
            'If (Double.TryParse(Me.txtStagePosY.Text, PosY) = False) Then
            '    Return (cFRS_NORMAL)
            'End If
            '----- V4.0.0.0-53↑ -----
            Ret = Form1.System1.XYtableMove(gSysPrm, PosX, PosY)
            If (Ret <> cFRS_NORMAL) Then                              ' エラー ?
                Return (Ret)                                          ' エラーリターン
            End If

        Catch ex As Exception
            strMSG = "frmProbeCleaning.MoveCleaningPosXY() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try

    End Function
#End Region

#Region "Ｚ軸をプローブクリーニング位置へ移動"
    '''=========================================================================
    ''' <summary>
    ''' Ｚ軸をプローブクリーニング位置へ移動 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Shared Function MoveCleaningPosZ() As Integer    'V6.0.0.0⑪
        'V6.0.0.0⑪    Public Function MoveCleaningPosZ() As Integer

        Dim PosZ As Double
        Dim Ret As Integer
        Dim strMSG As String

        Try
            '----- V2.0.0.0_30↓ -----
            PosZ = globaldblCleaningPosZ
            'V4.3.0.0③ PosZ = typPlateInfo.dblPrbCleanPosZ
            '----- V2.0.0.0_30↑ -----
            '----- V4.0.0.0-53↓ -----
            'If (Double.TryParse(Me.txtZPos.Text, PosZ) = False) Then
            '    Return (cFRS_NORMAL)
            'End If
            '----- V4.0.0.0-53↑ -----

            ' Zをプローブクリーニング位置へ移動する
            Ret = EX_ZMOVE(PosZ, MOVE_ABSOLUTE)
            ' エラーはEX_MOVE内で表示済み
            If (Ret <> cFRS_NORMAL) Then                                  ' エラー ?
                Return (Ret)
            End If

        Catch ex As Exception
            strMSG = "frmProbeCleaning.MoveCleaningPosZ() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try

    End Function
#End Region

#Region "Ｚ軸を原点位置へ移動"
    '''=========================================================================
    ''' <summary>
    ''' Ｚ軸を原点位置へ移動 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Function MoveOrgPosZ() As Integer
        Dim PosZ As Double
        Dim Ret As Integer
        Dim strMSG As String

        Try
            PosZ = 0
            ' Zを待機位置へ移動する
            Ret = EX_ZMOVE(PosZ, MOVE_ABSOLUTE)
            ' エラーはEX_MOVE内で表示済み
            If (Ret <> cFRS_NORMAL) Then                                  ' エラー ?
                Return (Ret)
            End If

        Catch ex As Exception
            strMSG = "frmProbeCleaning.MoveCleaningPosZ() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try

    End Function

    '''=========================================================================
    ''' <summary>
    ''' Ｚ軸を原点位置移動または現在位置より1mm上に上げる 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Shared Function MoveOrgWaitPosZ() As Integer        'V6.0.0.0⑪
        'V6.0.0.0⑪    Public Function MoveOrgWaitPosZ() As Integer
        Dim PosZ As Double
        Dim Ret As Integer
        Dim strMSG As String

        Try
            If (gdblCleaningPosZ > CLEANING_OFFSET) Then
                'V4.10.0.0⑨ このプログラムではgdblCleaningPosZの値は常に０なのでこの処理が行われることは無い。仕様が不明。
                PosZ = gdblCleaningPosZ - CLEANING_OFFSET
            Else
                PosZ = 0
            End If
            ' Zを待機位置へ移動する
            Ret = EX_ZMOVE(PosZ, MOVE_ABSOLUTE)
            ' エラーはEX_MOVE内で表示済み
            If (Ret <> cFRS_NORMAL) Then                                  ' エラー ?
                Return (Ret)
            End If

        Catch ex As Exception
            strMSG = "frmProbeCleaning.MoveCleaningPosZ() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try

    End Function
#End Region

#Region "プローブクリーニング位置を保存して終了"
    '''=========================================================================
    ''' <summary>
    ''' プローブクリーニング位置を保存して終了
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        stJOG.Flg = cFRS_ERR_START                                      ' OK(STARTｷｰ)

    End Sub
#End Region

#Region "プローブクリーニング位置を保存せずに終了"
    '''=========================================================================
    ''' <summary>
    ''' プローブクリーニング位置を保存せずに終了 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        stJOG.Flg = cFRS_ERR_RST                                      ' OK(STARTｷｰ)

    End Sub
#End Region

#Region "初期値の設定"
#If False Then                          'V6.0.0.0⑪ コンストラクタの引数で設定する
    '''=========================================================================
    ''' <summary>
    ''' 初期値の設定  
    ''' </summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Function sSetInitVal() As Integer

        Dim strMSG As String

        Try
            '----- V2.0.0.0_30↓ -----
            gdblCleaningPosX = typPlateInfo.dblPrbCleanPosX     'V6.0.0.0⑪ コンストラクタの引数で設定する
            gdblCleaningPosY = typPlateInfo.dblPrbCleanPosY     'V6.0.0.0⑪ コンストラクタの引数で設定する
            'gdblCleaningPosX = globaldblCleaningPosX
            'gdblCleaningPosY = globaldblCleaningPosY
            '----- V2.0.0.0_30↑ -----

            '----- V4.0.0.0-29↓ -----
            ' ラベル等を設定する(日本語・英語)
            'If (gSysPrm.stTMN.giMsgTyp = 0) Then
            '    ' グループ名
            '    grpBpOff.Text = "位置情報"
            '    GroupBox1.Text = "現在位置"
            '    GroupBox2.Text = "登録位置"
            '    GroupBox3.Text = "プローブクリーニングパラメータ"

            '    ' ラベル
            '    LblContactCount.Text = "プローブ接触回数"
            '    LblProbingCnt.Text = "クリーニング自動実行間隔"
            '    LblRemark01.Text = "(0:クリーニング無し)"

            '    ' ボタン
            '    btnCleaningStart.Text = "クリーニング実行"
            '    btnStageRegPosMove.Text = "ステージ登録位置移動"
            '    btnZRegMove.Text = "Z軸登録位置移動"

            'Else
            '    ' グループ名
            '    grpBpOff.Text = "POSITION INFORMATION"
            '    GroupBox1.Text = "CURRENT POSITION"
            '    GroupBox2.Text = "RESISTRATION POSITION"
            '    GroupBox3.Text = "PROBE CLEANING PARAMETER"

            '    ' ラベル
            '    LblContactCount.Text = "UP AND DOWN COUNT"
            '    LblProbingCnt.Text = "EXECUTION NUMBER"
            '    LblRemark01.Text = "(0:NO EXECUTE)"

            '    ' ボタン
            '    btnCleaningStart.Text = "CLEANING START"
            '    btnStageRegPosMove.Text = "MOVE STAGE POSITION"
            '    btnZRegMove.Text = "MOVE Z POSITION"
            'End If

            '----- V4.0.0.0-29↑ -----

            Return cFRS_NORMAL

        Catch ex As Exception
            strMSG = "frmProbeCleaning.sSetInitVal() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cFRS_NORMAL)
        End Try
    End Function
#End If
#End Region

#Region "実行結果の取得"
    ' '''=========================================================================
    ' ''' <summary>
    ' ''' 実行結果の取得 
    ' ''' </summary>
    ' ''' <remarks></remarks>
    ' '''=========================================================================
    'Public ReadOnly Property sGetReturn() As Integer Implements ICommonMethods.sGetReturn  'V6.0.0.0⑪
    '    'V6.0.0.0⑪        Public Function sGetReturn() As Integer
    '    Get
    '        Return cFRS_NORMAL
    '    End Get
    'End Property
#End Region

#Region "プローブクリーニング位置の更新"
    '''=========================================================================
    ''' <summary>
    ''' プローブクリーニング位置の更新 
    ''' </summary>
    ''' <param name="CleaningPosZ"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub DispProbePos(ByVal CleaningPosZ As Double)                 ' 位置表示(Z)
        Dim strMSG As String

        Try
            Me.txtZPos.Text = CleaningPosZ.ToString("0.000")            ' V4.0.0.0-53
            Me.txtZPos.BackColor = System.Drawing.Color.White

            Me.Refresh()

        Catch ex As Exception
            strMSG = "frmProbeCleaning.DispProbePos() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return
        End Try

    End Sub
#End Region

    '----- V4.0.0.0-53↓ -----
    '========================================================================================
    '   ボタン押下時処理
    '========================================================================================
#Region "OKﾎﾞﾀﾝ押下時処理"
    '''=========================================================================
    ''' <summary>OKﾎﾞﾀﾝ押下時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub cmdOK_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles cmdOK.MouseUp  'V1.16.0.0⑬

        stJOG.Flg = cFRS_ERR_START                                      ' OK(STARTｷｰ)

    End Sub
#End Region

#Region "ｷｬﾝｾﾙﾎﾞﾀﾝ押下時処理"
    '''=========================================================================
    ''' <summary>ｷｬﾝｾﾙﾎﾞﾀﾝ押下時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub cmdCancel_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles cmdCancel.MouseUp 'V1.16.0.0⑬

        stJOG.Flg = cFRS_ERR_RST                           ' Cancel(RESETｷｰ)

    End Sub
#End Region

#Region "HIボタン押下時処理"
    '''=========================================================================
    '''<summary>HIボタン押下時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnHI_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnHI.MouseDown
        Call SubBtnHI_Click(stJOG)
    End Sub
#End Region

#Region "矢印ボタンのマウスクリック時処理"
    '''=========================================================================
    ''' <summary>矢印ボタンのマウスクリック時処理</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub BtnJOG_0_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_0.MouseDown
        Timer1.Enabled = False
        Call SubBtnJOG_0_MouseDown(stJOG) 'V6.0.0.0-22                                    ' +Y ON
    End Sub
    Private Sub BtnJOG_0_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_0.MouseUp
        Call SubBtnJOG_0_MouseUp(stJOG) 'V6.0.0.0-22                                      ' +Y OFF
        Timer1.Enabled = True
    End Sub

    Private Sub BtnJOG_1_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_1.MouseDown
        Timer1.Enabled = False
        Call SubBtnJOG_1_MouseDown(stJOG) 'V6.0.0.0-22                                    ' -Y ON
    End Sub
    Private Sub BtnJOG_1_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_1.MouseUp
        Call SubBtnJOG_1_MouseUp(stJOG) 'V6.0.0.0-22                                      ' -Y OFF
        Timer1.Enabled = True
    End Sub
    Private Sub BtnJOG_2_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_2.MouseDown
        Timer1.Enabled = False
        Call SubBtnJOG_2_MouseDown(stJOG) 'V6.0.0.0-22                                    ' +X ON
    End Sub
    Private Sub BtnJOG_2_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_2.MouseUp
        Call SubBtnJOG_2_MouseUp(stJOG) 'V6.0.0.0-22                                      ' +X OFF
        Timer1.Enabled = True
    End Sub

    Private Sub BtnJOG_3_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_3.MouseDown
        Timer1.Enabled = False
        Call SubBtnJOG_3_MouseDown(stJOG) 'V6.0.0.0-22                                    ' -X ON
    End Sub
    Private Sub BtnJOG_3_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_3.MouseUp
        Call SubBtnJOG_3_MouseUp(stJOG) 'V6.0.0.0-22                                      ' -X OFF
        Timer1.Enabled = True
    End Sub

    Private Sub BtnJOG_4_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_4.MouseDown
        Timer1.Enabled = False
        Call SubBtnJOG_4_MouseDown(stJOG) 'V6.0.0.0-22                                    ' -X -Y ON
    End Sub
    Private Sub BtnJOG_4_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_4.MouseUp
        Call SubBtnJOG_4_MouseUp(stJOG) 'V6.0.0.0-22                                      ' -X -Y OFF
        Timer1.Enabled = True
    End Sub

    Private Sub BtnJOG_5_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_5.MouseDown
        Timer1.Enabled = False
        Call SubBtnJOG_5_MouseDown(stJOG) 'V6.0.0.0-22                                    ' +X -Y ON
    End Sub
    Private Sub BtnJOG_5_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_5.MouseUp
        Call SubBtnJOG_5_MouseUp(stJOG) 'V6.0.0.0-22                                      ' +X -Y OFF
        Timer1.Enabled = True
    End Sub

    Private Sub BtnJOG_6_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_6.MouseDown
        Timer1.Enabled = False
        Call SubBtnJOG_6_MouseDown(stJOG) 'V6.0.0.0-22                                    ' +X +Y ON
    End Sub
    Private Sub BtnJOG_6_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_6.MouseUp
        Call SubBtnJOG_6_MouseUp(stJOG) 'V6.0.0.0-22                                      ' +X +Y OFF
        Timer1.Enabled = True
    End Sub

    Private Sub BtnJOG_7_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_7.MouseDown
        Timer1.Enabled = False
        Call SubBtnJOG_7_MouseDown(stJOG) 'V6.0.0.0-22                                    ' -X +Y ON
    End Sub
    Private Sub BtnJOG_7_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_7.MouseUp
        Call SubBtnJOG_7_MouseUp(stJOG) 'V6.0.0.0-22                                      ' +X +Y OFF
        Timer1.Enabled = True
    End Sub
#End Region

#Region "モード変更時"
    '''=========================================================================
    ''' <summary>モード変更時</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub cmbMoveMode_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles cmbMoveMode.MouseDown
        Timer1.Enabled = False
    End Sub

    Private Sub cmbMoveMode_SelectionChangeCommitted(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMoveMode.SelectionChangeCommitted
        Timer1.Enabled = True
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
    Private Sub frmProbeCleaning_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        JogKeyDown(e)                   'V6.0.0.0⑪
    End Sub

    Public Sub JogKeyDown(e As KeyEventArgs) Implements ICommonMethods.JogKeyDown   'V6.0.0.0⑪
        If (False = _tenKeyON) Then Return 'V6.0.0.1②
        'V6.0.0.0⑪        Dim KeyCode As Short = e.KeyCode
        Dim KeyCode As Keys = e.KeyCode                 'V6.0.0.0⑪
        Console.WriteLine("frmProbeCleaning.frmProbeCleaning_KeyDown()")

        ' テンキーダウンならInpKeyにテンキーコードを設定する
        'V4.10.0.0⑨        GrpMain.Enabled = False
        'V6.0.0.0⑫       'Call Sub_10KeyDown(KeyCode)
        Sub_10KeyDown(KeyCode, stJOG)             'V6.0.0.0⑫
        If (KeyCode = System.Windows.Forms.Keys.NumPad5) Then           ' 5ｷｰ (KeyCode = 101(&H65)
            Call SubBtnHI_Click(stJOG)                                  ' HIボタン ON/OFF
        End If
        'V4.10.0.0⑨        Call Me.Focus()

    End Sub
#End Region

#Region "キーアップ時処理"
    '''=========================================================================
    '''<summary>キーアップ時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub frmProbeCleaning_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        JogKeyUp(e)                     'V6.0.0.0⑪
    End Sub

    Public Sub JogKeyUp(e As KeyEventArgs) Implements ICommonMethods.JogKeyUp       'V6.0.0.0⑪
        If (False = _tenKeyON) Then Return 'V6.0.0.1②
        'V6.0.0.0⑪        Dim KeyCode As Short = e.KeyCode
        Dim KeyCode As Keys = e.KeyCode                 'V6.0.0.0⑪

        Console.WriteLine("frmProbeCleaning.frmProbeCleaning_KeyKeyUp()")
        ' テンキーアップならInpKeyのテンキーコードをOFFする
        'V4.10.0.0⑨        GrpMain.Enabled = True
        'V6.0.0.0⑫        Call Sub_10KeyUp(KeyCode)
        Sub_10KeyUp(KeyCode, stJOG)                   'V6.0.0.0⑫
        'V4.10.0.0⑨        Call Me.Focus()

    End Sub
#End Region

#Region "カメラ画像クリック位置をセンターに移動する処理"
    ''' <summary>カメラ画像クリック位置をセンターに移動する処理</summary>
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
        Dim hit As Integer
        Try
            Timer1.Enabled = False
            hit = 1

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
                    'V6.0.0.0⑫                    grpBpOff.Enabled = True
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
            strMSG = "frmTxTyTeach.Timer1_Tick() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
#End If
    End Sub
#End Region
    '----- V4.0.0.0-53↑ -----
#End Region

    ''' <summary>
    ''' プローブティーチング画面でZボタンが押されたときの処理 　'V4.0.0.0-78
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnZ_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnZ.MouseDown
        Dim Ret As Integer

        If cmbMoveMode.SelectedIndex = 0 Then
            'XYモードのときは無効
            giJogButtonEnable = 0
        Else
            If BtnZ.BackColor = Color.Yellow Then
                giJogButtonEnable = 0
                'V6.0.0.1②                BtnZ.BackColor = Color.LightGray
                BtnZ.BackColor = SystemColors.Control                   'V6.0.0.1②
                Ret = MoveOrgPosZ()
                If Ret <> 0 Then
                    MsgBox("Z Axis Org move Error. ")
                End If
                BtnZ.Text = "Z Off"     'V4.10.0.0⑨
                stJOG.bZ = False        'V6.0.0.0⑲
            Else
                giJogButtonEnable = 1
                'V4.0.0.0-78
                BtnZ.BackColor = Color.Yellow
                ' プローブクリーニング位置へZ軸位置設定 
                'Ret = MoveCleaningPosZ()
                Ret = MoveTempCleaningPosZ()
                If Ret <> 0 Then
                    MsgBox("Z Axis move Error. ")
                End If
                BtnZ.Text = "Z On"      'V4.10.0.0⑨
                stJOG.bZ = True         'V6.0.0.0⑲
            End If
        End If
    End Sub



#Region "Ｚ軸を一時的なプローブクリーニング位置へ移動"
    '''=========================================================================
    ''' <summary>
    ''' Ｚ軸を一時的なプローブクリーニング位置へ移動 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Function MoveTempCleaningPosZ() As Integer

        Dim PosZ As Double
        Dim Ret As Integer
        Dim strMSG As String

        Try
            '----- V2.0.0.0_30↓ -----
            'PosZ = globaldblCleaningPosZ
            PosZ = CleaningPosZ
            '----- V2.0.0.0_30↑ -----
            '----- V4.0.0.0-53↓ -----
            'If (Double.TryParse(Me.txtZPos.Text, PosZ) = False) Then
            '    Return (cFRS_NORMAL)
            'End If
            '----- V4.0.0.0-53↑ -----

            ' Zをプローブクリーニング位置へ移動する
            Ret = EX_ZMOVE(PosZ, MOVE_ABSOLUTE)
            ' エラーはEX_MOVE内で表示済み
            If (Ret <> cFRS_NORMAL) Then                                  ' エラー ?
                Return (Ret)
            End If

        Catch ex As Exception
            strMSG = "frmProbeCleaning.MoveCleaningPosZ() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try

    End Function
#End Region

    'V4.10.0.0⑨↓
#Region "テンキーＯＮ時の処理"
    Private Sub BtnTenKey_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnTenKey.MouseUp
        'Dim InpKey As UShort
        Dim strMSG As String

        Try
            ' InpKeyのHI SW以外はOFFする
            'GetInpKey(InpKey)
            'If (InpKey And cBIT_HI) Then                                ' HI SW ON ?
            '    InpKey = cBIT_HI
            'Else
            '    InpKey = 0
            'End If
            'PutInpKey(InpKey)

            ' Ten Key On/Offボタン設定
            If (BtnTenKey.Text = "Ten Key Off") Then
                'V6.0.0.0⑯                gbTenKeyFlg = True
                _tenKeyON = True       'V6.0.0.0⑯
                BtnTenKey.Text = "Ten Key On"
                BtnTenKey.BackColor = System.Drawing.Color.Pink
                AddHandler Me.KeyDown, AddressOf frmProbeCleaning_KeyDown
                AddHandler Me.KeyUp, AddressOf frmProbeCleaning_KeyUp
                Form1.Instance.SetActiveJogMethod(AddressOf Me.JogKeyDown,
                                                  AddressOf Me.JogKeyUp,
                                                  AddressOf Me.MoveToCenter)  'V6.0.0.0⑪
                GroupBox3.Enabled = False
            Else
                'V6.0.0.0⑯                gbTenKeyFlg = False
                _tenKeyON = False      'V6.0.0.0⑯
                BtnTenKey.Text = "Ten Key Off"
                BtnTenKey.BackColor = System.Drawing.SystemColors.Control
                RemoveHandler Me.KeyDown, AddressOf frmProbeCleaning_KeyDown
                RemoveHandler Me.KeyUp, AddressOf frmProbeCleaning_KeyUp
                Form1.Instance.SetActiveJogMethod(Nothing,
                                                  Nothing,
                                                  AddressOf Me.MoveToCenter)  'V6.0.0.0⑪
                GroupBox3.Enabled = True
            End If
            Sub_10KeyUp(Keys.None, stJOG)                               'V6.0.1.0②

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FrmFineAdjust.SubBtnTenKey_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region
    'V4.10.0.0⑨↑
End Class

