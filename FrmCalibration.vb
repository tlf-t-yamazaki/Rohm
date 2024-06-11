'===============================================================================
'   Description  : キャリブレーション実行処理
'
'   Copyright(C) : OMRON LASERFRONT INC. 2011
'
'===============================================================================
Option Strict Off
Option Explicit On

Imports LaserFront.Trimmer.DefTrimFnc
Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8①
Imports TKY_ALL_SL432HW.My.Resources    'V4.4.0.0-0
Imports LaserFront.Trimmer.DefWin32Fnc              'V6.1.4.2①

Friend Class FrmCalibration
    Inherits System.Windows.Forms.Form
    Implements ICommonMethods              'V6.0.0.0⑪

#Region "プライベート変数定義"
    '===========================================================================
    '   変数定義
    '===========================================================================
    '----- 変数定義 -----
    Private stJOG As JOG_PARAM                                          ' 矢印画面(JOG操作)用パラメータ
    Private mExit_flg As Short                                          ' 終了フラグ

    '----- その他 -----
    Private dblTchMoval(3) As Double                                    ' ﾋﾟｯﾁ退避域(0=ﾋﾟｯﾁ, 1=HIGHﾋﾟｯﾁ, 2=Pause Time(Sec))
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
    '    'V6.0.0.0⑪        Public ReadOnly Property sGetReturn() As Short
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

#Region "Form Closed時処理"
    '''=========================================================================
    '''<summary>Form Closed時処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub FrmCalibration_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed

    End Sub
#End Region

#Region "ﾌｫｰﾑﾛｰﾄﾞ時処理"
    '''=========================================================================
    '''<summary>ﾌｫｰﾑﾛｰﾄﾞ時処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub FrmCalibration_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim strMSG As String

        Try
            ' 初期処理
            'Call SetMessages()                                          ' ラベル等を設定する(日本語/英語)
            mExit_flg = -1                                              ' 終了フラグ = 初期化

            ' ｳｲﾝﾄﾞｳをｺﾝﾄﾛｰﾙに重ねる
            Me.Top = Form1.Text4.Top + DISPOFF_SUBFORM_TOP
            Me.Left = Form1.Text4.Left

            ' トリミングデータより表示項目を設定する
            Call SetTrimData()

            Me.TopMost = True      ''V3.0.0.0⑪    'V6.0.0.0⑬ Activateから移動

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "FrmCalibration.FrmCalibration_Load() TRAP ERROR = " + ex.Message
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

        Dim strMSG As String

        Try
            ' ラベル等を設定する(日本語/英語)
            'Call PrepareMessagesCutPosCorrect_Calibration(gSysPrm.stTMN.giMsgTyp)          'V1.20.0.0⑧
            'Me.frmTitle.Text = FRM_CALIBRATION_TITLE                                        ' ｷｬﾘﾌﾞﾚｰｼｮﾝ
            'Me.lblStandardCoordinates1XTitle.Text = LBL_CALIBRATION_STANDERD_COORDINATES1X  ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標1X
            'Me.lblStandardCoordinates1YTitle.Text = LBL_CALIBRATION_STANDERD_COORDINATES1Y  ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標1Y
            'Me.lblStandardCoordinates2XTitle.Text = LBL_CALIBRATION_STANDERD_COORDINATES2X  ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標2X
            'Me.lblStandardCoordinates2YTitle.Text = LBL_CALIBRATION_STANDERD_COORDINATES2Y  ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標2Y
            'Me.lblTableOffsetXTitle.Text = LBL_CALIBRATION_TABLE_OFFSETX                    ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄX
            'Me.lblTableOffsetYTitle.Text = LBL_CALIBRATION_TABLE_OFFSETY                    ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄY

            'Me.lblGap1XTitle.Text = LBL_CALIBRATION_GAP1X                                   ' ｷｬﾘﾌﾞﾚｰｼｮﾝずれ量1X
            'Me.lblGap1YTitle.Text = LBL_CALIBRATION_GAP1Y                                   ' ｷｬﾘﾌﾞﾚｰｼｮﾝずれ量1Y
            'Me.lblGap2XTitle.Text = LBL_CALIBRATION_GAP2X                                   ' ｷｬﾘﾌﾞﾚｰｼｮﾝずれ量2X
            'Me.lblGap2YTitle.Text = LBL_CALIBRATION_GAP2Y                                   ' ｷｬﾘﾌﾞﾚｰｼｮﾝずれ量2Y

            'Me.lblGainXTitle.Text = LBL_CALIBRATION_GAINX                                   ' ｷｬﾘﾌﾞﾚｰｼｮﾝｹﾞｲﾝ補正係数X
            'Me.lblGainYTitle.Text = LBL_CALIBRATION_GAINY                                   ' ｷｬﾘﾌﾞﾚｰｼｮﾝｹﾞｲﾝ補正係数Y
            'Me.lblOffsetXTitle.Text = LBL_CALIBRATION_OFFSETX                               ' ｷｬﾘﾌﾞﾚｰｼｮﾝｵﾌｾｯﾄ補正量X
            'Me.lblOffsetYTitle.Text = LBL_CALIBRATION_OFFSETY                               ' ｷｬﾘﾌﾞﾚｰｼｮﾝｵﾌｾｯﾄ補正量Y

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "FrmCalibration.SetMessages() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "トリミングデータより表示項目を設定する"
    '''=========================================================================
    '''<summary>トリミングデータより表示項目を設定する</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub SetTrimData()

        Dim dblStanderd1X As Double                                     ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標1X
        Dim dblStanderd1Y As Double                                     ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標1Y
        Dim dblStanderd2X As Double                                     ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標2X
        Dim dblStanderd2Y As Double                                     ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標2Y
        Dim dblTableOffsetX As Double                                   ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄX
        Dim dblTableOffsetY As Double                                   ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄY
        Dim strMSG As String

        Try
            ' 表示項目設定
            dblStanderd1X = typPlateInfo.dblCaribBaseCordnt1XDir        ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標1
            dblStanderd1Y = typPlateInfo.dblCaribBaseCordnt1YDir
            dblStanderd2X = typPlateInfo.dblCaribBaseCordnt2XDir        ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標2
            dblStanderd2Y = typPlateInfo.dblCaribBaseCordnt2YDir

            Me.lblStandardCoordinates1X.Text = dblStanderd1X.ToString("##0.0000") ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標1X
            Me.lblStandardCoordinates1Y.Text = dblStanderd1Y.ToString("##0.0000") ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標1Y
            Me.lblStandardCoordinates2X.Text = dblStanderd2X.ToString("##0.0000") ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標2X
            Me.lblStandardCoordinates2Y.Text = dblStanderd2Y.ToString("##0.0000") ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標2Y

            ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄX,Y
            dblTableOffsetX = typPlateInfo.dblCaribTableOffsetXDir
            dblTableOffsetY = typPlateInfo.dblCaribTableOffsetYDir
            Me.lblTableOffsetX.Text = dblTableOffsetX.ToString("##0.0000") ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄX
            Me.lblTableOffsetY.Text = dblTableOffsetY.ToString("##0.0000") ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄY

            Me.lblGap1X.Text = ""                                       ' ずれ量1X
            Me.lblGap1Y.Text = ""                                       ' ずれ量1Y
            Me.lblGap2X.Text = ""                                       ' ずれ量2X
            Me.lblGap2Y.Text = ""                                       ' ずれ量2Y

            Me.lblGainX.Text = ""                                       ' ｹﾞｲﾝ補正係数X
            Me.lblGainY.Text = ""                                       ' ｹﾞｲﾝ補正係数Y
            Me.lblOffsetX.Text = ""                                     ' ｵﾌｾｯﾄ補正量X
            Me.lblOffsetY.Text = ""                                     ' ｵﾌｾｯﾄ補正量Y

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "FrmCalibration.SetTrimData() TRAP ERROR = " + ex.Message
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
    Private Sub FrmCalibration_Activated(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Activated

        Dim strMSG As String

        Try
            ' キャリブレーション実行
            If (mExit_flg <> -1) Then Exit Sub

            'V6.0.0.0⑬            Me.TopMost = True      ''V3.0.0.0⑪    'V6.0.0.0⑬ Loadに移動

            mExit_flg = 0                                               ' 終了フラグ = 0
            mExit_flg = CalibrationMain()                               ' キャリブレーション実行

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "FrmCalibration.FrmCalibration_Activated() TRAP ERROR = " + ex.Message
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
            ' キャリブレーション実行
            If (mExit_flg <> -1) Then Exit Function

            'V6.0.0.0⑬            Me.TopMost = True      ''V3.0.0.0⑪    'V6.0.0.0⑬ Loadに移動

            mExit_flg = 0                                               ' 終了フラグ = 0
            mExit_flg = CalibrationMain()                               ' キャリブレーション実行

            ' トラップエラー発生時
        Catch ex As Exception
            Dim strMSG As String = "FrmCalibration.Execute() TRAP ERROR = " & ex.Message
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
    '   キャリブレーション実行処理
    '========================================================================================
#Region "キャリブレーション実行メイン処理"
    '''=========================================================================
    '''<summary>キャリブレーション実行メイン処理</summary>
    '''<returns>cFRS_ERR_START=OK(STARTｷｰ)
    '''         cFRS_ERR_RST  =Cancel(RESETｷｰ)
    '''         -1以下        =エラー</returns>
    '''=========================================================================
    Private Function CalibrationMain() As Integer

        Dim mdBSx As Double                                             ' ﾌﾞﾛｯｸｻｲｽﾞX
        Dim mdBSy As Double                                             ' ﾌﾞﾛｯｸｻｲｽﾞY
        'Dim dblStanderd1X As Double                                    ' V3.0.0.0⑥(V1.22.0.0⑦)
        'Dim dblStanderd1Y As Double
        'Dim dblStanderd2X As Double
        'Dim dblStanderd2Y As Double
        Dim dblGainX As Double                                          ' ｹﾞｲﾝ補正係数
        Dim dblGainY As Double
        Dim dblOffsetX As Double                                        ' ｵﾌｾｯﾄ補正係数
        Dim dblOffsetY As Double
        Dim dQrate As Double
        Dim CondNum As Integer
        Dim r As Integer
        Dim rtn As Integer
        Dim StpNum As Integer
        Dim ObjProc As Process = Nothing
        Dim strMSG As String

        Try
            '-------------------------------------------------------------------
            '   初期処理
            '-------------------------------------------------------------------
            ' 基準座標取得
            '----- V3.0.0.0⑥(V1.22.0.0⑦)↓ -----
            If (typPlateInfo.intResistDir = 0) Then                     ' 抵抗(ﾁｯﾌﾟ)並び方向(0:X, 1:Y)
                mdBSx = 80.0                                            ' ブロックサイズは80*0とする 
                mdBSy = 0.0
                '----- V1.24.0.0①↓ -----
                If (gSysPrm.stDEV.giBpSize = BPSIZE_6060) Then
                    mdBSx = 60.0                                        ' ブロックサイズは60*0とする 
                ElseIf (gSysPrm.stDEV.giBpSize = BPSIZE_90) Then
                    mdBSx = 90.0                                        ' ブロックサイズは90*0とする   'V4.4.0.0① 
                End If
                '----- V1.24.0.0①↑ -----
            Else
                mdBSx = 0.0                                            ' ブロックサイズは0*80とする 
                mdBSy = 80.0
                '----- V1.24.0.0①↓ -----
                If (gSysPrm.stDEV.giBpSize = BPSIZE_6060) Then
                    mdBSy = 60.0                                        ' ブロックサイズは0*60とする
                ElseIf (gSysPrm.stDEV.giBpSize = BPSIZE_90) Then
                    mdBSy = 90.0                                        ' ブロックサイズは0*90とする   'V4.4.0.0①
                End If
                '----- V1.24.0.0①↑ -----
            End If

            'dblStanderd1X = typPlateInfo.dblCaribBaseCordnt1XDir        ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標1
            'dblStanderd1Y = typPlateInfo.dblCaribBaseCordnt1YDir
            'dblStanderd2X = typPlateInfo.dblCaribBaseCordnt2XDir        ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標2
            'dblStanderd2Y = typPlateInfo.dblCaribBaseCordnt2YDir
            ''----- V1.14.0.0⑦↓ -----
            ''----- V1.20.0.1②↓ -----
            ''mdBSx = 80.0                                                ' ブロックサイズは80角とする 
            ''mdBSy = 80.0
            '' ブロックサイズはそのままとする(Move_Trimposition()でブロックサイズとBPオフセット(0,0)は設定済み) 
            'mdBSx = typPlateInfo.dblBlockSizeXDir
            'mdBSy = typPlateInfo.dblBlockSizeYDir
            '----- V3.0.0.0⑥(V1.22.0.0⑦)↑ -----

            ' BPを基準座標1に移動する
            'r = Form1.System1.EX_MOVE(gSysPrm, dblStanderd1X + typPlateInfo.dblCaribTableOffsetXDir, dblStanderd1Y + typPlateInfo.dblCaribTableOffsetYDir, 1)
            r = CaribBpMove(0, stJOG.PosX, stJOG.PosY)                  ' ※stJOG.PosX,Yはダミー域
            '----- V1.20.0.1②↑ -----
            If (r < cFRS_NORMAL) Then
                Return (r)                                              ' Return値 = エラー
            End If

            'Call CalcBlockSize(mdBSx, mdBSy)                            ' ブロックサイズXY設定
            'Call BpMoveOrigin_Ex()                                      ' BPをﾌﾞﾛｯｸの右上に移動する
            'r = Form1.System1.EX_MOVE(gSysPrm, 0.0, 0.0, 1)
            'If (r < cFRS_NORMAL) Then
            '    Return (r)                                              ' Return値 = エラー
            'End If
            '----- V1.14.0.0⑦↑ -----

            ' 矢印画面(XYﾃｰﾌﾞﾙのJOG操作)用パラメータを初期化する
            stJOG.Md = MODE_KEY                                         ' モード(キー入力待ちモード)
            stJOG.Md2 = MD2_BUTN                                        ' 入力モード(0:画面ﾎﾞﾀﾝ入力, 1:ｺﾝｿｰﾙ入力)
            stJOG.Opt = CONSOLE_SW_RESET + CONSOLE_SW_START             ' キーの有効(1)/無効(0)指定
            stJOG.PosX = 0.0                                            ' BP X位置/XYﾃｰﾌﾞﾙ X位置
            stJOG.PosY = 0.0                                            ' BP Y位置/XYﾃｰﾌﾞﾙ Y位置
            '----- V1.14.0.0⑦↓ -----
            stJOG.BpOffX = 0.0                                          ' BPｵﾌｾｯﾄX = ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄX 
            stJOG.BpOffY = 0.0                                          ' BPｵﾌｾｯﾄY = ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄY
            'stJOG.BpOffX = typPlateInfo.dblBpOffSetXDir                 ' BPｵﾌｾｯﾄX 
            'stJOG.BpOffY = typPlateInfo.dblBpOffSetYDir                 ' BPｵﾌｾｯﾄY 
            '----- V1.14.0.0⑦↑ -----
            stJOG.BszX = mdBSx                                          ' ﾌﾞﾛｯｸｻｲｽﾞX 
            stJOG.BszY = mdBSy                                          ' ﾌﾞﾛｯｸｻｲｽﾞY
            stJOG.TextX = lblGap1X                                      ' X位置表示用ラベル
            stJOG.TextY = lblGap1Y                                      ' Y位置表示用ラベル
            stJOG.cgX = 0.0                                             ' 移動量X 
            stJOG.cgY = 0.0                                             ' 移動量Y 
            stJOG.BtnHI = BtnHI                                         ' HIボタン
            stJOG.BtnZ = BtnZ                                           ' Zボタン
            stJOG.BtnSTART = BtnSTART                                   ' STARTボタン
            stJOG.BtnHALT = BtnHALT                                     ' HALTボタン
            stJOG.BtnRESET = BtnRESET                                   ' RESETボタン
            Call JogEzInit(stJOG, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
            Call Me.Focus()                                             ' フォーカスを設定する(テンキー入力用) 

STP_START:
            '-------------------------------------------------------------------
            '   キャリブレーション実行のために十字カットを行う
            '-------------------------------------------------------------------
            ' システムエラーチェック
            r = Form1.System1.SysErrChk_ForVBNET(giAppMode)
            If (r <> cFRS_NORMAL) Then
                Return (r)
            End If

            ' 内部カメラに切替える
#If VIDEO_CAPTURE = 0 Then
            Call Form1.VideoLibrary1.ChangeCamera(INTERNAL_CAMERA)
#End If
            '' 基準座標1,2の中央へ移動(内部カメラ)   不要 ??
            'r = XYTableMoveCenter(dblStanderd1X, dblStanderd1Y, dblStanderd2X, dblStanderd2Y)
            'If (r <> cFRS_NORMAL) Then
            '    GoTo STP_END
            'End If
            SendCrossLineMsgToDispGazou()                               ' V6.1.4.18①

            ' コンソール入力(START/RESETキー入力待ち)
            Call ZCONRST()                                              ' ラッチ解除
            StpNum = 0                                                  ' StpNum = 0
            Me.lblInfo.Text = MSG_CALIBRATION_001                       ' キャリブレーションを実行します。" & vbCrLf & "[START]を押してください。"
            stJOG.Md = MODE_KEY                                         ' モード(キー入力待ちモード)
            stJOG.Opt = CONSOLE_SW_RESET + CONSOLE_SW_START             ' キーの有効(1)/無効(0)指定
            'V6.0.0.0⑪            r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
            r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval, Me) 'V6.0.0.0⑪
            If (r < cFRS_NORMAL) Then                                   ' エラーならエラーリターン(メッセージ表示済み)
                Return (r)
            End If
            If (r = cFRS_ERR_RST) Then GoTo STP_END '                   ' RESETキー押下なら終了確認メッセージ表示へ

            ' 加工条件を入力する(FL時)
            '----- V1.14.0.0⑧↓ -----
            CondNum = typPlateInfo.intCaribCutCondNo                    ' 加工条件番号
            'CondNum = 0                                                 ' 加工条件番号
            '----- V1.14.0.0⑧↑ -----
            dQrate = typPlateInfo.dblCaribCutQRate                      ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾚｰｻﾞQﾚｰﾄ 
            r = Sub_FlCond(CondNum, dQrate, Me)
            If (r <> cFRS_NORMAL) Then
                If (r = cFRS_ERR_RST) Then GoTo STP_START '             ' RESETキー押下なら終了確認メッセージ表示へ
                Return (r)
            End If
            Me.Refresh()                                                ' ※加工条件入力画面表示を消すため

            ' Intime側にBPキャリブレーション値を送信する(ゲインオフセットのクリア)
            dblGainX = 1
            dblGainY = 1
            dblOffsetX = 0
            dblOffsetY = 0
            r = BP_CALIBRATION(dblGainX, dblGainY, dblOffsetX, dblOffsetY)
            r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
            If (r <> cFRS_NORMAL) Then                                  ' エラーならエラーリターン(メッセージ表示済み)
                Return (r)
            End If

            ' 画像表示プログラムを起動する
            'V6.0.0.0⑤            r = Exec_GazouProc(ObjProc, DISPGAZOU_PATH, DISPGAZOU_WRK, 0, 0)

            ' キャリブレーション実行のために基準座標1と基準座標2に十字カットを行う
            r = CalibrationCrossCut(typPlateInfo, CondNum, dQrate)      ' 十字カット実行
            'V6.0.0.0⑤            Call End_GazouProc(ObjProc)                                 ' 画像表示プログラムを終了する
            If (r <> cFRS_NORMAL) Then
                If (r = cFRS_ERR_RST) Then GoTo STP_END '               ' RESETキー押下なら終了確認メッセージ表示へ
                Return (r)                                              ' エラーならエラーリターン(メッセージ表示済み)
            End If

STP_RETRY:
            '-------------------------------------------------------------------
            '   外部カメラでパターン認識を行い、ずれ量を算出する
            '-------------------------------------------------------------------
            ' コンソール入力(START/RESETキー入力待ち)
            StpNum = 1                                                  ' StpNum = 1
            Me.lblInfo.Text = MSG_CALIBRATION_002                       ' 外部カメラでずれ量を検出します。[START]を押してください。[RESET]：中止"
            'V6.0.0.0⑪            r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
            r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval, Me)  'V6.0.0.0⑪
            If (r < cFRS_NORMAL) Then                                   ' エラーならエラーリターン(メッセージ表示済み)
                Return (r)
            End If
            If (r = cFRS_ERR_RST) Then GoTo STP_END '                   ' RESETキー押下なら終了確認メッセージ表示へ
            Call gparModules.CrossLineDispOff()    'V4.10.0.0③
            ' キャリブレーション処理(外部カメラでパターン認識を行い、ずれ量を算出する)
            r = CalibrationExec(typPlateInfo)
            If (r <> cFRS_NORMAL) Then
                ' 内部カメラに切替える
#If VIDEO_CAPTURE = 0 Then
                Call Form1.VideoLibrary1.ChangeCamera(INTERNAL_CAMERA)
#End If
            End If

            Call Form1.VideoLibrary1.PatternDisp(False)                 ' パターンマッチング時の検索範囲枠(黄色枠と青色枠)を非表示とする　###094

            Return (r)                                                  ' キャリブレーション処理終了 

STP_END:
            '-------------------------------------------------------------------
            '   終了確認メッセージを表示する
            '-------------------------------------------------------------------
            ' "前の画面に戻ります。よろしいですか？
            Call Form1.VideoLibrary1.PatternDisp(False)                 ' パターンマッチング時の検索範囲枠(黄色枠と青色枠)を非表示とする　###094
            If gbAutoCalibration Then                                   'V6.1.4.2①[自動キャリブレーション補正実行]
                r = cFRS_NORMAL                                         'V6.1.4.2①
            Else                                                        'V6.1.4.2①
                rtn = Form1.System1.TrmMsgBox(gSysPrm, MSG_105, MsgBoxStyle.OkCancel, MSG_OPLOG_CALIBRATION_START)
            End If                                                      'V6.1.4.2①
            If (rtn = cFRS_ERR_RST) Then                                '「いいえ」選択なら処理を継続する
                If (StpNum = 0) Then GoTo STP_START '                   ' 十字カット処理へ
                GoTo STP_RETRY                                          ' パターン認識処理へ
            End If

            ' 内部カメラに切替える
#If VIDEO_CAPTURE = 0 Then
            Call Form1.VideoLibrary1.ChangeCamera(INTERNAL_CAMERA)
#End If
            Return (r)                                                  ' キャリブレーション処理終了(RESET(Cancel)キー)

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "FrmCalibration.CalibrationMain() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return値 = 例外エラー
        End Try
    End Function
#End Region

#Region "キャリブレーション用十字カット処理(基準座標1と基準座標2に十字カットを行う)"
    '''=========================================================================
    ''' <summary>キャリブレーション用十字カット処理(基準座標1と基準座標2に十字カットを行う)</summary>
    ''' <param name="stPLT">  (INP)プレートデータ</param>
    ''' <param name="CondNum">(INP)加工条件番号(FL用)</param>
    ''' <param name="dQrate"> (INP)Qレート(KHz)</param>
    ''' <returns>cFRS_ERR_START=OK(STARTｷｰ)
    '''         cFRS_ERR_RST  =Cancel(RESETｷｰ)
    '''         -1以下        =エラー</returns>
    ''' <remarks>V1.14.0.0⑦で前面改定、###104、###105の修正は削除</remarks>
    '''=========================================================================
    Private Function CalibrationCrossCut(ByRef stPLT As PlateInfo, ByVal CondNum As Integer, ByVal dQrate As Double) As Integer

        Dim r As Integer
        Dim Count As Integer
        Dim fPosOffX As Double
        Dim fPosOffY As Double
        Dim dblBpPosX As Double
        Dim dblBpPosY As Double
        Dim dblStdX(2) As Double
        Dim dblStdY(2) As Double
        Dim strMSG As String

        Try
            ' 初期処理
            fPosOffX = stPLT.dblCaribTableOffsetXDir                        ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄX
            fPosOffY = stPLT.dblCaribTableOffsetYDir                        ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄY
            dblStdX(0) = stPLT.dblCaribBaseCordnt1XDir                      ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標1X
            dblStdY(0) = stPLT.dblCaribBaseCordnt1YDir                      ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標1Y
            dblStdX(1) = stPLT.dblCaribBaseCordnt2XDir                      ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標2X
            dblStdY(1) = stPLT.dblCaribBaseCordnt2YDir                      ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標2Y

            ' 基準座標１と基準座標２に十字カットを行う
            For Count = 0 To 1
                ' "【十字カットモード(基準座標x)】" + "【警告】[STARTキー]: 十字カット実行 , [RESETキー]: 中止"
                If (Count = 0) Then
                    strMSG = LBL_CALIBRATION_001 + vbCrLf
                Else
                    strMSG = LBL_CALIBRATION_002 + vbCrLf
                End If
                lblInfo.Text = strMSG + MSG_CUT_POS_CORRECT_004
                lblInfo.Refresh()

                ' BPのカット位置中央の座標を求める(X座標をカット長の1/2戻す)
                'dblBpPosX = dblStdX(Count) - (stPLT.dblCaribCutLength / 2) + fPosOffX
                dblBpPosX = dblStdX(Count) + fPosOffX
                dblBpPosY = dblStdY(Count) + fPosOffY

                ' BPを十字カット位置の中央へ移動する 
                'r = Form1.System1.EX_MOVE(gSysPrm, dblBpPosX, dblBpPosY, 1) ' BP絶対値移動 V1.20.0.1②
                r = CaribBpMove(Count, dblBpPosX, dblBpPosY)                ' V1.20.0.1②
                If (r <> cFRS_NORMAL) Then                                  ' エラー ?   
                    GoTo STP_END
                End If
                Call gparModules.DispCrossLine(0, 0)    'V5.0.0.6⑫
                ' 十字カットを実行する(基準座標１または基準座標２)
                r = Sub_CalibrationCrossCut(dblBpPosX, dblBpPosY, CondNum, dQrate, stPLT.dblCaribCutLength, stPLT.dblCaribCutSpeed)
                If (r <> cFRS_NORMAL) Then                                  ' エラー ?
                    If (r = cFRS_ERR_RST) Then
                        Count = Count - 1
                        Continue For
                    ElseIf r < cFRS_NORMAL Then ' V3.0.0.0③ ADD START
                        Return (r)              ' V3.0.0.0③ ソフト強制終了レベルのエラー
                    End If                      ' V3.0.0.0③
                End If
            Next Count

            ' 終了処理
STP_END:
            Return (r)                                                      ' Return値設定

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "FrmCalibration.CalibrationCrossCut() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                              ' Return値 = 例外エラー
        End Try
    End Function
#End Region

#Region "キャリブレーション用十字カットサブルーチン"
    '''=========================================================================
    '''<summary>キャリブレーション用十字カットサブルーチン</summary>
    ''' <param name="dblBpPosX">   (INP)カット位置X</param>
    ''' <param name="dblBpPosY">   (INP)カット位置Y</param>
    ''' <param name="CondNum">     (INP)加工条件番号(FL用)</param>
    ''' <param name="dQrate">      (INP)Qレート(KHz)</param>
    ''' <param name="dblCutLength">(INP)カット長</param>
    ''' <param name="dblCutSpeed"> (INP)カット速度</param>
    ''' <returns>0 = 正常, 0以外 = エラー</returns>
    ''' <remarks>十字ｶｯﾄの中央にBPを移動しておくこと</remarks>
    '''=========================================================================
    Private Function Sub_CalibrationCrossCut(ByVal dblBpPosX As Double, ByVal dblBpPosY As Double, ByVal CondNum As Integer,
                                             ByVal dQrate As Double, ByVal dblCutLength As Double, ByVal dblCutSpeed As Double) As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' コンソール入力(START/RESETキー入力待ち)
            stJOG.Md = MODE_KEY                                         ' モード(キー入力待ちモード)
            stJOG.Opt = CONSOLE_SW_RESET + CONSOLE_SW_START             ' キーの有効(1)/無効(0)指定
            'V6.0.0.0⑪            r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
            r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval, Me)  'V6.0.0.0⑪
            If (r < cFRS_NORMAL) Then                                   ' エラーならエラーリターン(メッセージ表示済み)
                Return (r)
            End If
            If (r = cFRS_ERR_RST) Then
                ' ###078  　↓ 
                ' Resetが押された場合に、本当にResetするかのメッセージを表示する。
                Me.lblInfo.Text = MSG_CALIBRATION_008                       ' "Resetします。 [Start]:Reset実行 , [Reset]:Cancel実行 "     
                stJOG.Md = MODE_KEY                                         ' モード(キー入力待ちモード)
                stJOG.Opt = CONSOLE_SW_RESET + CONSOLE_SW_START             ' キーの有効(1)/無効(0)指定
                'V6.0.0.0⑪                r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
                r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval, Me)  'V6.0.0.0⑪
                ' ###078  　↑ 
                Return (r)
            End If

            ' 十字カットを実行する
            lblInfo.Text = MSG_CUT_POS_CORRECT_005                      ' "十字カット実行中"
            r = CrossCutExec(dblBpPosX, dblBpPosY, CondNum, dQrate, dblCutLength, dblCutSpeed)
            lblInfo.Text = ""
            Return (r)

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "FrmCalibration.Sub_CalibrationCrossCut() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return値 = 例外エラー
        End Try
    End Function
#End Region

#Region "キャリブレーション処理(外部カメラでパターン認識を行い、ずれ量を算出する)"
    '''=========================================================================
    ''' <summary>キャリブレーション処理(外部カメラでパターン認識を行い、ずれ量を算出する)</summary>
    ''' <param name="stPLT">(INP)プレートデータ</param>
    ''' <returns>cFRS_ERR_START=OK(STARTｷｰ)
    '''         cFRS_ERR_RST  =Cancel(RESETｷｰ)
    '''         -1以下        =エラー</returns>
    '''=========================================================================
    Private Function CalibrationExec(ByRef stPLT As PlateInfo) As Integer

        Dim r As Integer
        Dim rtn As Integer
        Dim Count As Integer
        Dim dblBSX As Double
        Dim dblBSY As Double
        Dim dblCalSizeX As Double
        Dim dblCalSizeY As Double
        Dim PosX(2) As Double
        Dim PosY(2) As Double
        Dim GapX(2) As Double
        Dim GapY(2) As Double
        Dim GrpNo(2) As Short
        Dim TmpNo(2) As Short
        Dim LblGapX(2) As System.Windows.Forms.Label
        Dim LblGapY(2) As System.Windows.Forms.Label
        Dim fStgPosX As Double
        Dim fStgPosY As Double
        Dim fCaribOfsX As Double                                        ' V3.0.0.0⑥(V1.22.0.0⑦)
        Dim fCaribOfsY As Double                                        ' V3.0.0.0⑥(V1.22.0.0⑦)

        Dim dblGainX As Double                                          ' ｹﾞｲﾝ補正係数
        Dim dblGainY As Double
        Dim dblOffsetX As Double                                        ' ｵﾌｾｯﾄ補正係数
        Dim dblOffsetY As Double
        Dim fcoeff As Double                                            ' 相関値
        Dim strMSG As String = Nothing

        Try
            '-------------------------------------------------------------------
            '   初期処理 
            '-------------------------------------------------------------------
            Call CalcBlockSize(dblBSX, dblBSY)                          ' ﾌﾞﾛｯｸｻｲｽﾞ算出

            ' 基準座標1と基準座標2の中間座標を求める
            dblCalSizeX = System.Math.Abs(stPLT.dblCaribBaseCordnt2XDir - stPLT.dblCaribBaseCordnt1XDir)
            dblCalSizeY = System.Math.Abs(stPLT.dblCaribBaseCordnt2YDir - stPLT.dblCaribBaseCordnt1YDir)

            Call Form1.System1.EX_BPOFF(gSysPrm, 0.0#, 0.0#)            ' BPｵﾌｾｯﾄにｶｯﾄ位置補正ｵﾌｾｯﾄ設定
            Call Form1.System1.EX_MOVE(gSysPrm, 0.0#, 0.0#, 1)          ' 最初のｶｯﾄ位置中央へ移動

            ' 十字カット座標取得
            PosX(0) = stPLT.dblCaribBaseCordnt1XDir                     ' 基準座標１
            PosY(0) = stPLT.dblCaribBaseCordnt1YDir
            '----- V1.14.0.0⑦↓ -----
            ' 基準座標２
            PosX(1) = stPLT.dblCaribBaseCordnt2XDir
            PosY(1) = stPLT.dblCaribBaseCordnt2YDir
            'If (stPLT.intResistDir = 0) Then                            ' ﾁｯﾌﾟ並び方向はX方向 ?
            '    PosX(1) = stPLT.dblCaribBaseCordnt2XDir                 ' 基準座標２
            '    PosY(1) = stPLT.dblCaribBaseCordnt1YDir
            'Else
            '    PosX(1) = stPLT.dblCaribBaseCordnt1XDir
            '    PosY(1) = stPLT.dblCaribBaseCordnt2YDir
            'End If
            '----- V1.14.0.0⑦↑ -----
            GrpNo(0) = stPLT.intCaribPtnNo1GroupNo                      ' グループ番号1
            TmpNo(0) = stPLT.intCaribPtnNo1                             ' パターン番号1
            GrpNo(1) = stPLT.intCaribPtnNo2GroupNo                      ' グループ番号2
            TmpNo(1) = stPLT.intCaribPtnNo2                             ' パターン番号2
            GapX(0) = 0.0                                               ' ずれ量1初期化
            GapY(0) = 0.0
            GapX(1) = 0.0                                               ' ずれ量2初期化
            GapY(1) = 0.0
            LblGapX(0) = lblGap1X                                       ' ずれ量1X表示用ラベル
            LblGapY(0) = lblGap1Y                                       ' ずれ量1Y表示用ラベル
            LblGapX(1) = lblGap2X                                       ' ずれ量2X表示用ラベル
            LblGapY(1) = lblGap2Y                                       ' ずれ量2Y表示用ラベル

            ' 外部カメラに切替える
#If VIDEO_CAPTURE = 0 Then
            Call Form1.VideoLibrary1.ChangeCamera(EXTERNAL_CAMERA)
#End If

STP_RETRY:
            '-------------------------------------------------------------------
            '   基準座標１と基準座標２のパターンマッチングを行いずれ量を算出する
            '-------------------------------------------------------------------
            For Count = 0 To 1                                          ' 基準座標１と基準座標２の分、繰り返す 
                ' ガイダンスメッセージ表示
                If (Count = 0) Then
                    strMSG = LBL_CALIBRATION_003 + vbCrLf
                Else
                    strMSG = LBL_CALIBRATION_004 + vbCrLf
                End If
                lblInfo.Text = strMSG + MSG_CALIBRATION_003             ' "【画像認識モード(基準座標x)】" + "[START]: 画像認識実行 , [RESET]: 中止"
                lblInfo.Refresh()

                'V6.0.0.0-28                ' 移動前後に画像更新処理は停止する。
                'V6.0.0.0-28                Call Form1.VideoLibrary1.VideoStop()

                '----- V3.0.0.0⑥(V1.22.0.0⑦)↓ -----
                Select Case (gSysPrm.stDEV.giBpDirXy)
                    Case 0 ' 右上(x←, y↓)
                        fCaribOfsX = typPlateInfo.dblCaribTableOffsetXDir
                        fCaribOfsY = typPlateInfo.dblCaribTableOffsetYDir
                    Case 1 ' 左上(x→, y↓)
                        fCaribOfsX = typPlateInfo.dblCaribTableOffsetXDir * -1
                        fCaribOfsY = typPlateInfo.dblCaribTableOffsetYDir
                    Case 2 ' 右下(x←, y↑)
                        fCaribOfsX = typPlateInfo.dblCaribTableOffsetXDir
                        fCaribOfsY = typPlateInfo.dblCaribTableOffsetYDir * -1
                    Case 3 ' 左下(x→, y↑)
                        fCaribOfsX = typPlateInfo.dblCaribTableOffsetXDir * -1
                        fCaribOfsY = typPlateInfo.dblCaribTableOffsetYDir * -1
                End Select
                'V4.7.1.0①↓ PATTERN_CORRECT_MODE（=0:BP/XYﾃｰﾌﾞﾙとも動かす）モードの時は、外部カメラでオフセットを加算しては駄目
                If gSysPrm.stSPF.giPatternCorectMode = 0 Then
                    fCaribOfsX = 0.0
                    fCaribOfsY = 0.0
                End If
                '                V4.7.1.0①↑
                ' 基準座標１または２へテーブルを絶対値移動する(外部カメラ下)            ''###106
                r = XYTableMoveExt(PosX(Count) + fCaribOfsX, PosY(Count) + fCaribOfsY)
                'r = XYTableMoveExt(PosX(Count) + stPLT.dblCaribTableOffsetXDir, PosY(Count) + stPLT.dblCaribTableOffsetYDir)
                ' r = XYTableMoveExt(PosX(Count) + stPLT.dblCaribTableOffsetXDir + typPlateInfo.dblTableOffsetXDir,PosY(Count) + stPLT.dblCaribTableOffsetYDir + typPlateInfo.dblTableOffsetYDir)
                '----- V3.0.0.0⑥(V1.22.0.0⑦)↑ -----
                If (r <> cFRS_NORMAL) Then
                    Return (r)
                End If

                ' テーブル安定時間 Wait
                If (gPrevInterlockSw = 0) Then
                    Call System.Threading.Thread.Sleep(100)             ' 通常動作時のﾃｰﾌﾞﾙ安定時間 Wait(msec)
                Else
                    Call System.Threading.Thread.Sleep(200)             ' 低速動作時のﾃｰﾌﾞﾙ安定時間 Wait(msec)
                End If

                'V6.0.0.0-28                ' 移動前後に画像更新処理は再開する。
                'V6.0.0.0-28                Call Form1.VideoLibrary1.VideoStart()

                ' コンソール入力(START/RESETキー入力待ち)
                Call ZCONRST()                                          ' ラッチ解除
                stJOG.Md = MODE_KEY                                     ' モード(キー入力待ちモード)
                stJOG.Opt = CONSOLE_SW_RESET + CONSOLE_SW_START         ' キーの有効(1)/無効(0)指定
                'V6.0.0.0⑪                r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
                r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval, Me)      'V6.0.0.0⑪
                If (r < cFRS_NORMAL) Then                               ' エラーならエラーリターン(メッセージ表示済み)
                    Return (r)
                End If
                '                If (r = cFRS_ERR_RST) Then GoTo STP_END '               ' RESETキー押下なら終了確認メッセージ表示へ
                '###095
                If (r = cFRS_ERR_RST) Then
                    ' Resetが押された場合に、本当にResetするかのメッセージを表示する。
                    Me.lblInfo.Text = MSG_CALIBRATION_008                       ' "Resetします。 [Start]:Reset実行 , [Reset]:Cancel実行 "     
                    stJOG.Md = MODE_KEY                                         ' モード(キー入力待ちモード)
                    stJOG.Opt = CONSOLE_SW_RESET + CONSOLE_SW_START             ' キーの有効(1)/無効(0)指定
                    'V6.0.0.0⑪                    r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
                    r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval, Me)  'V6.0.0.0⑪
                    If (r = cFRS_ERR_START) Then
                        GoTo STP_END '               ' RESETキー押下なら終了確認メッセージ表示へ
                    ElseIf (r = cFRS_ERR_RST) Then
                        Count = Count - 1
                        Continue For
                    End If
                End If
                ' ###095  　↑ 

                ' 外部カメラでパターン認識を行う
                r = Sub_PatternMatching(GrpNo(Count), TmpNo(Count), GapX(Count), GapY(Count), fcoeff)
                If (r <> cFRS_NORMAL) Then
                    If (r = cFRS_ERR_PTN) Or (r = cFRS_ERR_PT2) Then
                        '"画像マッチングエラー" "処理を続ける場合は[OK]を、中止する場合は[Cancel]を押して下さい。"
                        '----- V1.14.0.0⑦↓ -----
                        GapX(Count) = 0.0
                        GapY(Count) = 0.0
                        If (r = cFRS_ERR_PT2) Then
                            strMSG = MSG_CALIBRATION_007 + " (Thresh =" + fcoeff.ToString("0.00") + ")"
                        Else
                            strMSG = MSG_CALIBRATION_007
                        End If
                        'V6.1.4.2①↓
                        If gbAutoCalibration Then
                            strMSG = "自動キャリブレーション補正 パターンマッチングエラー[" & r.ToString & "] Thresh =[" & fcoeff.ToString("0.00") & "]"
                            Call Form1.System1.OperationLogging(gSysPrm, strMSG, "CALIBRATION")
                            Form1.Z_PRINT(strMSG)
                            gbAutoCalibrationResult = False
                            r = cFRS_ERR_RST
                            GoTo STP_EXIT
                        End If
                        'V6.1.4.2①↑
                        rtn = Form1.System1.TrmMsgBox(gSysPrm, strMSG, MsgBoxStyle.OkCancel, MSG_OPLOG_CALIBRATION_START)
                        'rtn = Form1.System1.TrmMsgBox(gSysPrm, MSG_CALIBRATION_007, MsgBoxStyle.OkCancel, MSG_OPLOG_CALIBRATION_START)
                        '----- V1.14.0.0⑦↑ -----
                        If (rtn = cFRS_ERR_RST) Then                    '「いいえ」選択なら終了
                            ' ###077 ↓
                            r = cFRS_NORMAL
                            GoTo STP_END
                            ' ###077 ↑
                        End If
                    Else
                        Return (r)
                    End If
                End If

                '-------------------------------------------------------------------
                '   矢印キーによるテーブル動作(外部カメラ下)でずれ量1,2を調整する
                '-------------------------------------------------------------------
                ' ガイダンスメッセージ表示
                If (Count = 0) Then
                    strMSG = MSG_CALIBRATION_004                        ' "外部カメラでずれ量を検出します。(基準座標１)" & vbCrLf & "[START]：決定，[RESET]：中止"
                Else
                    strMSG = MSG_CALIBRATION_005                        ' "外部カメラでずれ量を検出します。(基準座標２)" & vbCrLf & "[START]：決定，[RESET]：中止"
                End If
                lblInfo.Text = strMSG
                lblInfo.Refresh()

                'Call ZGETPHPOS(fStgPosX, fStgPosY)                      ' X,Yテーブル位置取得 V1.14.0.0⑦
                Call ZGETPHPOS2(fStgPosX, fStgPosY)                      ' X,Yテーブル位置取得 V1.14.0.0⑦
                stJOG.Md = MODE_STG                                     ' モード(XYテーブルモード)
                stJOG.Opt = CONSOLE_SW_RESET + CONSOLE_SW_START         ' キーの有効(1)/無効(0)指定
                stJOG.PosX = fStgPosX                                   ' テーブルX座標
                stJOG.PosY = fStgPosY                                   ' テーブルY座標
                stJOG.cgX = GapX(Count)                                 ' 移動量X(ずれ量X)
                stJOG.cgY = GapY(Count)                                 ' 移動量Y(ずれ量Y)
                stJOG.TextX = LblGapX(Count)                            ' ずれ量X表示用ラベル
                stJOG.TextY = LblGapY(Count)                            ' ずれ量Y表示用ラベル
                'V6.0.0.0⑪                r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
                r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval, Me)      'V6.0.0.0⑪
                If (r < cFRS_NORMAL) Then                               ' エラーならエラーリターン(メッセージ表示済み)
                    Return (r)
                End If
                '###095
                If (r = cFRS_ERR_RST) Then
                    ' Resetが押された場合に、本当にResetするかのメッセージを表示する。
                    Me.lblInfo.Text = MSG_CALIBRATION_008                       ' "Resetします。 [Start]:Reset実行 , [Reset]:Cancel実行 "     
                    stJOG.Md = MODE_KEY                                         ' モード(キー入力待ちモード)
                    stJOG.Opt = CONSOLE_SW_RESET + CONSOLE_SW_START             ' キーの有効(1)/無効(0)指定
                    'V6.0.0.0⑪                    r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
                    r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval, Me)  'V6.0.0.0⑪
                    If (r = cFRS_ERR_START) Then
                        GoTo STP_END '               ' RESETキー押下なら終了確認メッセージ表示へ
                    ElseIf (r = cFRS_ERR_RST) Then
                        Count = Count - 1
                        Continue For
                    End If
                End If
                ' ###095  　↑ 
                If (r = cFRS_ERR_RST) Then GoTo STP_END '               ' RESETキー押下なら終了確認メッセージ表示へ

                ' ずれ量を退避する
                GapX(Count) = stJOG.cgX                                 ' 移動量X(ずれ量X)
                GapY(Count) = stJOG.cgY                                 ' 移動量Y(ずれ量Y)

            Next Count

            '-------------------------------------------------------------------
            '   ゲインとオフセットを算出する
            '-------------------------------------------------------------------
            ' ゲインとオフセットを算出する
            Call CalcGainOffset(stPLT, GapX(0), GapY(0), GapX(1), GapY(1), dblGainX, dblGainY, dblOffsetX, dblOffsetY)
            'V6.1.4.2①↓
            If gbAutoCalibration Then
                Dim GapLimit As Double = Double.Parse(GetPrivateProfileString_S("SPECIALFUNCTION", "AUTO_CARIBRATION_RECOG_LIMIT", "C:\TRIM\tky.ini", "0"))
                For i As Integer = 0 To 1
                    If System.Math.Abs(GapX(i)) > GapLimit Then
                        strMSG = "自動キャリブレーション補正　リミットエラー X[" & (i + 1).ToString("0") & "] OFFSET LIMIT ERROR=[" & GapX(i).ToString("0.000") & "]>[" & GapLimit.ToString("0.000") & "]"
                        Call Form1.System1.OperationLogging(gSysPrm, strMSG, "CALIBRATION")
                        Form1.Z_PRINT(strMSG)
                        gbAutoCalibrationResult = False
                        r = cFRS_ERR_RST
                        strMSG = "リミットエラー GapX(0)=[" & GapX(0).ToString("0.000") & "] GapY(0)=[" & GapY(0).ToString("0.000") & "] GapX(1)=[" & GapX(1).ToString("0.000") & "] GapY(1)=[" & GapY(1).ToString("0.000") & "]"
                        Call Form1.System1.OperationLogging(gSysPrm, strMSG, "CALIBRATION")
                        GoTo STP_EXIT
                    End If
                    If System.Math.Abs(GapY(i)) > GapLimit Then
                        strMSG = "自動キャリブレーション補正　リミットエラー Y[" & (i + 1).ToString("0") & "] OFFSET LIMIT ERROR=[" & GapY(i).ToString("0.000") & "]>[" & GapLimit.ToString("0.000") & "]"
                        Call Form1.System1.OperationLogging(gSysPrm, strMSG, "CALIBRATION")
                        Form1.Z_PRINT(strMSG)
                        gbAutoCalibrationResult = False
                        r = cFRS_ERR_RST
                        strMSG = "リミットエラー GapX(0)=[" & GapX(0).ToString("0.000") & "] GapY(0)=[" & GapY(0).ToString("0.000") & "] GapX(1)=[" & GapX(1).ToString("0.000") & "] GapY(1)=[" & GapY(1).ToString("0.000") & "]"
                        Call Form1.System1.OperationLogging(gSysPrm, strMSG, "CALIBRATION")
                        GoTo STP_EXIT
                    End If
                Next
            End If
            'V6.1.4.2①↑

            ' ゲインとオフセットを表示する 
            lblGainX.Text = dblGainX.ToString("0.0000")
            lblGainY.Text = dblGainY.ToString("0.0000")
            lblOffsetX.Text = dblOffsetX.ToString("0.0000")
            lblOffsetY.Text = dblOffsetY.ToString("0.0000")

            If gbAutoCalibration Then                                   'V6.1.4.2①[自動キャリブレーション補正実行]
                frmTitle.Refresh()                                      'V6.1.4.2①
                r = cFRS_NORMAL                                         'V6.1.4.2①
            Else                                                        'V6.1.4.2①
                '"キャリブレーションを終了します。" & "データを保持する場合は[OK]を、データを保持しない場合は[Cancel]を押して下さい。"
                rtn = Form1.System1.TrmMsgBox(gSysPrm, MSG_CALIBRATION_006, MsgBoxStyle.OkCancel, MSG_OPLOG_CALIBRATION_START)
            End If                                                      'V6.1.4.2①
            If (rtn = cFRS_ERR_RST) Then                                '「いいえ」選択なら処理を終了する
                GoTo STP_EXIT
            End If

            ' Intime側にBPキャリブレーション値を送信する
            'V1.20.0.0⑦↓
            dblGainX = dblGainX * dblCalibHoseiX
            'V1.20.0.0⑦↑
            r = BP_CALIBRATION(dblGainX, dblGainY, dblOffsetX, dblOffsetY)
            'V6.1.4.2①↓
            If r <> cFRS_NORMAL Then
                gbAutoCalibrationResult = False
            Else
                strMSG = "CalGainX=[" & dblGainX.ToString("0.0000") & "] CalGainY=[" & dblGainY.ToString("0.0000") & "] CalOffX=[" & dblOffsetX.ToString("0.0000") & "] CalOffY=[" & dblOffsetY.ToString("0.0000") & "]"
                Call Form1.System1.OperationLogging(gSysPrm, strMSG, "CALIBRATION")
                Form1.Z_PRINT(strMSG)
            End If
            'V6.1.4.2①↑
            r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
            If (r <> cFRS_NORMAL) Then                                  ' エラーならエラーリターン(メッセージ表示済み)
                Return (r)
            End If
            Dim OffsetLimit As Double = Double.Parse(GetPrivateProfileString_S("SPECIALFUNCTION", "AUTO_CARIBRATION_OFFSET_LIMIT", "C:\TRIM\tky.ini", "0"))
            If dblOffsetY > OffsetLimit Or dblOffsetX > OffsetLimit Then
                ' '' ''If System.Math.Abs(dblOffsetY) > OffsetLimit Then
                ' '' ''    strMSG = "オフセット補正量Ｙ=[" & dblOffsetY.ToString("0.000") & "]が[" & OffsetLimit.ToString("0.000") & "](mm)を超えました。"
                ' '' ''    Call Form1.System1.OperationLogging(gSysPrm, strMSG, "CALIBRATION")
                ' '' ''    Form1.Z_PRINT(strMSG)
                ' '' ''    Form1.LabelAutoCalibLimit.Text = strMSG
                ' '' ''    Form1.LabelAutoCalibLimit.Visible = True
                ' '' ''End If
                If System.Math.Abs(dblOffsetX) > OffsetLimit Then
                    strMSG = "オフセット補正量Ｘ=[" & dblOffsetX.ToString("0.000") & "]が[" & OffsetLimit.ToString("0.000") & "](mm)を超えました。"
                    Call Form1.System1.OperationLogging(gSysPrm, strMSG, "CALIBRATION")
                    Form1.Z_PRINT(strMSG)
                    Form1.LabelAutoCalibLimit.Text = strMSG
                    Form1.LabelAutoCalibLimit.Visible = True
                End If

            Else
                Form1.LabelAutoCalibLimit.Visible = False
            End If



            GoTo STP_EXIT                                               ' 処理終了へ 

STP_END:
            '-------------------------------------------------------------------
            '   終了確認メッセージを表示する
            '-------------------------------------------------------------------
            ' "前の画面に戻ります。よろしいですか？　
            If gbAutoCalibration Then                                   'V6.1.4.2①[自動キャリブレーション補正実行]
                r = cFRS_NORMAL                                         'V6.1.4.2①
            Else                                                        'V6.1.4.2①
                rtn = Form1.System1.TrmMsgBox(gSysPrm, MSG_105, MsgBoxStyle.OkCancel, MSG_OPLOG_CALIBRATION_START)
            End If                                                      'V6.1.4.2①
            If (rtn = cFRS_ERR_RST) Then                                '「いいえ」選択なら処理を継続する
                GoTo STP_RETRY
            End If

STP_EXIT:
            ' 内部カメラに切替える
#If VIDEO_CAPTURE = 0 Then
            Call Form1.VideoLibrary1.ChangeCamera(INTERNAL_CAMERA)                    ' 内部カメラに切替える
#End If
            Return (r)

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "FrmCalibration.CalibrationExec() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return値 = 例外エラー
        End Try
    End Function
#End Region

#Region "外部カメラの指定座標へテーブルを絶対値移動する"
    '''=========================================================================
    '''<summary>外部カメラの基準座標へXYテーブルを移動する</summary>
    ''' <param name="dblStdX">(INP)基準座標X位置</param>
    ''' <param name="dblStdY">(INP)基準座標Y位置</param>
    ''' <returns>0 = 正常, 0以外 = エラー</returns>
    '''<remarks></remarks>
    '''=========================================================================
    Private Function XYTableMoveExt(ByVal dblStdX As Double, ByVal dblStdY As Double) As Integer

        Dim r As Integer
        Dim dblX As Double
        Dim dblY As Double
        Dim dblBPX As Double
        Dim dblBPY As Double
        Dim dblTrimPosX As Double
        Dim dblTrimPosY As Double
        Dim Del_x As Double
        Dim Del_y As Double
        Dim strMSG As String

        Try
            ' 初期処理
            dblTrimPosX = gSysPrm.stDEV.gfTrimX                         ' トリムポジションX                             '
            dblTrimPosY = gSysPrm.stDEV.gfTrimY                         ' トリムポジションY
            Del_x = gfCorrectPosX                                       ' トリムポジション補正値X(θ補正実行時に設定) 
            Del_y = gfCorrectPosY                                       ' トリムポジション補正値Y(θ補正実行時に設定) 
            dblBPX = dblStdX                                            ' 基準座標X位置
            dblBPY = dblStdY                                            ' 基準座標Y位置

            ' ﾄﾘﾐﾝｸﾞ位置座標 + ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄ + 補正位置  (+or-) ﾃｰﾌﾞﾙ補正量
            Select Case (gSysPrm.stDEV.giBpDirXy)
                Case 0      ' x←, y↓
                    dblX = dblTrimPosX + Del_x + dblBPX
                    dblY = dblTrimPosY + Del_y + dblBPY
                Case 1      ' x→, y↓
                    dblX = dblTrimPosX + Del_x - dblBPX
                    dblY = dblTrimPosY + Del_y + dblBPY
                Case 2      ' x←, y↑
                    dblX = dblTrimPosX + Del_x + dblBPX
                    dblY = dblTrimPosY + Del_y - dblBPY
                Case 3      ' x→, y↑
                    dblX = dblTrimPosX + Del_x - dblBPX
                    dblY = dblTrimPosY + Del_y - dblBPY
            End Select

            ' ＸＹテーブル移動
            dblX = dblX + gSysPrm.stDEV.gfExCmX                         ' 外部カメラオフセット位置を加算
            dblY = dblY + gSysPrm.stDEV.gfExCmY
            r = Form1.System1.XYtableMove(gSysPrm, dblX, dblY)          ' ＸＹテーブル移動(絶対値)
            Return (r)

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "FrmCalibration.XYTableMoveExt() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return値 = 例外エラー
        End Try
    End Function
#End Region
    '----- V1.20.0.1②↓ -----
#Region "基準座標位置1又は基準座標位置2へBPを移動する"
    '''=========================================================================
    ''' <summary>基準座標位置1又は基準座標位置2へBPを移動する</summary>
    ''' <param name="Idx">   (INP)0=基準座標位置1, 1=基準座標位置2</param>
    ''' <param name="dblBPX">(OUT)基準座標位置X</param>
    ''' <param name="dblBPY">(OUT)基準座標位置Y</param>
    ''' <returns>cFRS_NORMAL = 正常
    '''          上記以外  = エラー</returns>
    '''=========================================================================
    Private Function CaribBpMove(ByVal Idx As Integer, ByRef dblBPX As Double, ByRef dblBPY As Double) As Integer

        Dim r As Integer
        Dim BpOffX As Double = 0.0
        Dim BpOffY As Double = 0.0
        Dim strMSG As String
        Dim dblOffsetXDir As Double 'V4.7.1.0①ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄX
        Dim dblOffsetYDir As Double 'V4.7.1.0①ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄY

        'V4.7.1.0①↓ PATTERN_CORRECT_MODE（=0:BP/XYﾃｰﾌﾞﾙとも動かす）モードの時は、外部カメラでオフセットを加算しては駄目
        If gSysPrm.stSPF.giPatternCorectMode = 0 Then
            dblOffsetXDir = typPlateInfo.dblCaribTableOffsetXDir
            dblOffsetYDir = typPlateInfo.dblCaribTableOffsetYDir
        Else
            dblOffsetXDir = 0.0
            dblOffsetYDir = 0.0
        End If
        'V4.7.1.0①↑ 

        Try
            ' 基準座標1,2のBP座標を求める
            '----- V3.0.0.0⑥(V1.22.0.0⑦)↓ -----
            If (Idx = 0) Then                                           ' 基準座標位置1 ?
                If (typPlateInfo.intResistDir = 0) Then                 ' 抵抗(ﾁｯﾌﾟ)並び方向(0:X, 1:Y)
                    dblBPX = typPlateInfo.dblCaribBaseCordnt1XDir
                    dblBPY = 0.0
                Else
                    dblBPX = 0.0
                    dblBPY = typPlateInfo.dblCaribBaseCordnt1YDir
                End If

            Else                                                        ' 基準座標位置2
                If (typPlateInfo.intResistDir = 0) Then                 ' 抵抗(ﾁｯﾌﾟ)並び方向(0:X, 1:Y)
                    dblBPX = typPlateInfo.dblCaribBaseCordnt2XDir
                    dblBPY = 0.0
                    BpOffX = 0.0
                    BpOffY = typPlateInfo.dblCaribBaseCordnt1YDir - typPlateInfo.dblCaribBaseCordnt2YDir
                Else
                    dblBPX = 0.0
                    dblBPY = typPlateInfo.dblCaribBaseCordnt2YDir
                    BpOffX = typPlateInfo.dblCaribBaseCordnt1XDir - typPlateInfo.dblCaribBaseCordnt2XDir
                    BpOffY = 0.0
                End If
            End If
            r = BPOFF(BpOffX, BpOffY)

            'If (Idx = 0) Then
            '    dblBPX = typPlateInfo.dblCaribBaseCordnt1XDir
            '    dblBPY = typPlateInfo.dblCaribBaseCordnt1YDir
            'Else
            '    dblBPX = typPlateInfo.dblCaribBaseCordnt2XDir
            '    dblBPY = typPlateInfo.dblCaribBaseCordnt2YDir
            'End If
            '----- V3.0.0.0⑥(V1.22.0.0⑦)↑ -----

            ' BPにキャリブレーションテーブルオフセットを加算(テーブル(x→, y↑)とは反対方向)
            '----- V3.0.0.0⑥(V1.22.0.0⑦)↓ -----
            'V4.7.1.0①↓コメントアウト
            'Select Case (gSysPrm.stDEV.giBpDirXy)
            '    Case 0 ' 右上(x←, y↓)
            '        If (typPlateInfo.intResistDir = 0) Then                 ' 抵抗(ﾁｯﾌﾟ)並び方向(0:X, 1:Y)
            '            dblBPX = dblBPX + typPlateInfo.dblCaribTableOffsetXDir
            '            'dblBPY = dblBPY + typPlateInfo.dblCaribTableOffsetYDir
            '        Else
            '            'dblBPX = dblBPX + typPlateInfo.dblCaribTableOffsetXDir
            '            dblBPY = dblBPY + typPlateInfo.dblCaribTableOffsetYDir
            '        End If
            '    Case 1 ' 左上(x→, y↓)
            '        If (typPlateInfo.intResistDir = 0) Then                 ' 抵抗(ﾁｯﾌﾟ)並び方向(0:X, 1:Y)
            '            dblBPX = dblBPX - typPlateInfo.dblCaribTableOffsetXDir
            '            'dblBPY = dblBPY + typPlateInfo.dblCaribTableOffsetYDir
            '        Else
            '            'dblBPX = dblBPX - typPlateInfo.dblCaribTableOffsetXDir
            '            dblBPY = dblBPY + typPlateInfo.dblCaribTableOffsetYDir
            '        End If
            '    Case 2 ' 右下(x←, y↑)
            '        If (typPlateInfo.intResistDir = 0) Then                 ' 抵抗(ﾁｯﾌﾟ)並び方向(0:X, 1:Y)
            '            dblBPX = dblBPX + typPlateInfo.dblCaribTableOffsetXDir
            '            'dblBPY = dblBPY - typPlateInfo.dblCaribTableOffsetYDir
            '        Else
            '            'dblBPX = dblBPX + typPlateInfo.dblCaribTableOffsetXDir
            '            dblBPY = dblBPY - typPlateInfo.dblCaribTableOffsetYDir
            '        End If
            '    Case 3 ' 左下(x→, y↑)
            '        If (typPlateInfo.intResistDir = 0) Then                 ' 抵抗(ﾁｯﾌﾟ)並び方向(0:X, 1:Y)
            '            dblBPX = dblBPX - typPlateInfo.dblCaribTableOffsetXDir
            '            'dblBPY = dblBPY - typPlateInfo.dblCaribTableOffsetYDir
            '        Else
            '            'dblBPX = dblBPX - typPlateInfo.dblCaribTableOffsetXDir
            '            dblBPY = dblBPY - typPlateInfo.dblCaribTableOffsetYDir
            '        End If

            '        'Case 0 ' 右上(x←, y↓)
            '        '    dblBPX = dblBPX + typPlateInfo.dblCaribTableOffsetXDir
            '        '    dblBPY = dblBPY + typPlateInfo.dblCaribTableOffsetYDir
            '        'Case 1 ' 左上(x→, y↓)
            '        '    dblBPX = dblBPX - typPlateInfo.dblCaribTableOffsetXDir
            '        '    dblBPY = dblBPY + typPlateInfo.dblCaribTableOffsetYDir
            '        'Case 2 ' 右下(x←, y↑)
            '        '    dblBPX = dblBPX + typPlateInfo.dblCaribTableOffsetXDir
            '        '    dblBPY = dblBPY - typPlateInfo.dblCaribTableOffsetYDir
            '        'Case 3 ' 左下(x→, y↑)
            '        '    dblBPX = dblBPX - typPlateInfo.dblCaribTableOffsetXDir
            '        '    dblBPY = dblBPY - typPlateInfo.dblCaribTableOffsetYDir
            'End Select
            'V4.7.1.0①↑コメントアウト
            '----- V3.0.0.0⑥(V1.22.0.0⑦)↑ -----

            ' BPを十字カット位置の中央へ移動する(絶対値移動)
            'V4.7.1.0①↓
            dblBPX = dblBPX - typPlateInfo.dblCaribTableOffsetXDir
            dblBPY = dblBPY - typPlateInfo.dblCaribTableOffsetYDir
            'V4.7.1.0①↑
            r = Form1.System1.EX_MOVE(gSysPrm, dblBPX, dblBPY, 1)

            Return (r)

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "FrmCalibration.CaribBpMove() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return値 = 例外エラー
        End Try
    End Function
#End Region
    '----- V1.20.0.1②↑ -----
#Region "ゲインとオフセットを算出する(四捨五入付き)"
    '''=========================================================================
    ''' <summary>ゲインとオフセットを算出する(四捨五入付き)</summary>
    ''' <param name="stPLT">     (INP)プレートデータ</param>
    ''' <param name="dblGap1X">  (INP)ずれ量1X</param>
    ''' <param name="dblGap1Y">  (INP)ずれ量1Y</param>
    ''' <param name="dblGap2X">  (INP)ずれ量2X</param>
    ''' <param name="dblGap2Y">  (INP)ずれ量2Y</param>
    ''' <param name="dblGainX">  (OUT)ゲインX補正係数</param>
    ''' <param name="dblGainY">  (OUT)ゲインY補正係数</param>
    ''' <param name="dblOffsetX">(OUT)オフセットX補正係数</param>
    ''' <param name="dblOffsetY">(OUT)オフセットY補正係数</param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub CalcGainOffset(ByRef stPLT As PlateInfo, ByRef dblGap1X As Double, ByRef dblGap1Y As Double, ByRef dblGap2X As Double, ByRef dblGap2Y As Double, _
                               ByRef dblGainX As Double, ByRef dblGainY As Double, ByRef dblOffsetX As Double, ByRef dblOffsetY As Double)

        Dim r As Integer
        'Dim dblTOffsX As Double
        'Dim dblTOffsY As Double
        'Dim dblOf1 As Double
        'Dim dblOf2 As Double
        Dim dblNumerator As Double                                      ' 分子
        Dim dblDenominator As Double                                    ' 分母
        Dim strMSG As String
        Dim dblCurGain(1) As Double
        Dim dblCurCalibOff(1) As Double

        Try
            ' INTRIMに設定されている現状のキャリブレーション補整値を取得
            r = BP_GET_CALIBDATA(dblCurGain(0), dblCurGain(1), dblCurCalibOff(0), dblCurCalibOff(1))

            If (stPLT.intResistDir = 0) Then                            ' チップ並び方向 = X方向 ?
                ' ゲインX
                dblNumerator = stPLT.dblCaribBaseCordnt2XDir - stPLT.dblCaribBaseCordnt1XDir
                dblDenominator = (stPLT.dblCaribBaseCordnt2XDir + dblGap2X) - (stPLT.dblCaribBaseCordnt1XDir + dblGap1X)
                '----- V1.14.0.0⑦↓ -----
                If (0.0# = dblDenominator) Then                         ' 分母 0?
                    dblDenominator = 1.0#
                End If
                '----- V1.14.0.0⑦↑ -----
                ' ゲイン補整値を元に戻す。
                dblDenominator = dblDenominator / dblCurGain(0)
                ' 補整値
                'V4.7.0.0⑯                dblGainX = RoundOff(dblDenominator / dblNumerator)
                dblGainX = RoundOff(dblNumerator / dblDenominator)  'V4.7.0.0⑯

                ' ゲインY
                dblGainY = 1.0#

                ' '' '' '' オフセットX
                '' '' ''dblOf1 = (stPLT.dblCaribBaseCordnt1XDir + dblGap1X) * stPLT.dblCaribBaseCordnt2XDir
                '' '' ''dblOf2 = (stPLT.dblCaribBaseCordnt2XDir + dblGap2X) * stPLT.dblCaribBaseCordnt2XDir
                '' '' ''dblOffsetX = RoundOff((dblOf1 - dblOf2) / dblNumerator)

                ' '' '' '' オフセットY
                '' '' ''dblOffsetY = RoundOff((-dblGap1Y - dblGap2Y) / 2)
            Else
                '(2012/02/23)
                '   なぜキャリブレーションの計算において、XYで計算方法が異なるか不明。(minato)

                ' ゲインX
                dblGainX = 1.0#

                ' ゲインY
                dblNumerator = stPLT.dblCaribBaseCordnt2YDir - stPLT.dblCaribBaseCordnt1YDir
                '----- V1.14.0.0⑦↓ -----
                If (gSysPrm.stDEV.giBpDirXy = 0) Or (gSysPrm.stDEV.giBpDirXy = 1) Then
                    dblGap1Y = dblGap1Y * -1
                    dblGap2Y = dblGap2Y * -1
                    '    dblDenominator = (stPLT.dblCaribBaseCordnt2YDir - stPLT.dblCaribBaseCordnt1YDir) - (dblGap2Y * -1 - dblGap1Y * -1)
                    'Else
                    '    dblDenominator = (stPLT.dblCaribBaseCordnt2YDir - stPLT.dblCaribBaseCordnt1YDir) - (dblGap2Y - dblGap1Y)
                End If
                dblDenominator = (stPLT.dblCaribBaseCordnt2YDir - stPLT.dblCaribBaseCordnt1YDir) - (dblGap2Y - dblGap1Y)
                '----- V1.14.0.0⑦↑ -----
                If (0.0# = dblDenominator) Then                         ' 分母 0?
                    dblDenominator = 1.0#
                End If
                ' ゲイン補整値を元に戻す。
                dblDenominator = dblDenominator / dblCurGain(1)
                ' 補整値
                dblGainY = RoundOff(dblNumerator / dblDenominator)

                '' '' '' オフセットX
                ' '' ''dblOffsetX = RoundOff((-dblGap1X - dblGap2X) / 2)

                '' '' '' オフセットY
                ' '' ''dblOf1 = (stPLT.dblCaribBaseCordnt1YDir + dblTOffsY) * -dblGap2Y * dblGainY
                ' '' ''dblOf2 = (stPLT.dblCaribBaseCordnt2YDir + dblTOffsY) * -dblGap1Y * dblGainY
                ' '' ''dblOffsetY = RoundOff((dblOf1 - dblOf2) / dblNumerator)
            End If

            'オフセット値の補正
            '   オフセットは基準1の外部カメラのズレ量をBPオフセット、キャリブレーションオフセットへ反映 
            dblOffsetX = dblCurCalibOff(0) + (dblGap1X * -1)   'V1.20.0.0⑦ 'V4.7.0.0⑯コメント解除
            'dblOffsetX = 0.0                                    'V1.20.0.0⑦
            dblOffsetY = dblCurCalibOff(1) + (dblGap1Y * -1)   'V1.20.0.0⑦ 'V4.7.0.0⑯コメント解除
            'dblOffsetY = 0.0                                    'V1.20.0.0⑦

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "FrmCalibration.CalcGainOffset() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
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
        'Call SubBtnSTART_Click()                                       'V1.20.0.1③
    End Sub
    Private Sub BtnSTART_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnSTART.MouseUp
        Call SubBtnSTART_Click()                                        'V1.20.0.1③
    End Sub
#End Region

#Region "RESETボタン押下時処理"
    '''=========================================================================
    '''<summary>RESETボタン押下時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnRESET_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnRESET.Click
        'Call SubBtnRESET_Click()                                       'V1.20.0.1③
    End Sub
    Private Sub BtnRESET_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnRESET.MouseUp
        Call SubBtnRESET_Click()                                        'V1.20.0.1③
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
    Private Sub FrmCalibration_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        JogKeyDown(e)                   'V6.0.0.0⑪
    End Sub

    Public Sub JogKeyDown(ByVal e As KeyEventArgs) Implements ICommonMethods.JogKeyDown    'V6.0.0.0⑪
        'V6.0.0.0⑪        Dim KeyCode As Short = e.KeyCode
        Dim KeyCode As Keys = e.KeyCode                 'V6.0.0.0⑪ 
        Console.WriteLine("FrmCalibration.FrmCalibration_KeyDown()")

        ' テンキーダウンならInpKeyにテンキーコードを設定する
        'V6.0.0.0⑫       'Call Sub_10KeyDown(KeyCode)
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
    Private Sub FrmCalibration_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        JogKeyUp(e)                     'V6.0.0.0⑪
    End Sub

    Public Sub JogKeyUp(ByVal e As KeyEventArgs) Implements ICommonMethods.JogKeyUp        'V6.0.0.0⑪
        'V6.0.0.0⑪        Dim KeyCode As Short = e.KeyCode
        Dim KeyCode As Keys = e.KeyCode                 'V6.0.0.0⑪
        Console.WriteLine("FrmCalibration.FrmCalibration_KeyUp()")
        ' テンキーアップならInpKeyのテンキーコードをOFFする
        'V6.0.0.0⑫        Call Sub_10KeyUp(KeyCode)
        Sub_10KeyUp(KeyCode, stJOG)               'V6.0.0.0⑫
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

End Class