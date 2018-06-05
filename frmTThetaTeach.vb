'===============================================================================
'   Description  : Ｔθティーチング画面処理
'
'   Copyright(C) : OMRON LASERFRONT INC. 2010
'
'===============================================================================
Option Strict Off
Option Explicit On

Imports LaserFront.Trimmer.DefTrimFnc
Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8①
Imports TKY_ALL_SL432HW.My.Resources    'V4.4.0.0-0

Friend Class frmTThetaTeach
    Inherits System.Windows.Forms.Form
    Implements ICommonMethods              'V6.0.0.0⑪

#Region "プライベート変数定義"
    '===========================================================================
    '   変数定義
    '===========================================================================
    Private stJOG As JOG_PARAM                              ' 矢印画面(BPのJOG操作)用パラメータ
    Private mExit_flg As Short                              ' 終了フラグ
    'Private objArrow As FrmArrow                        ' 矢印画面ｵﾌﾞｼﾞｪｸﾄ 
    'Private stBPMV As FrmArrow.JOG_PARAM                ' 矢印画面(BPのJOG操作)用パラメータ

    '----- トリミングパラメータ -----
    Private mdBpOffx As Double                          ' BP位置ｵﾌｾｯﾄX
    Private mdBpOffy As Double                          ' BP位置ｵﾌｾｯﾄY
    Private mdAdjx As Double                            ' ｱｼﾞｬｽﾄ位置ｵﾌｾｯﾄX
    Private mdAdjy As Double                            ' ｱｼﾞｬｽﾄ位置ｵﾌｾｯﾄY
    Private miBNx As Short                              ' ﾌﾞﾛｯｸ数X
    Private miBNy As Short                              ' ﾌﾞﾛｯｸ数Y
    Private miCdir As Short                             ' ﾁｯﾌﾟ並び方向
    Private miChpNum As Short                           ' ﾁｯﾌﾟ数
    Private miGNx As Short                              ' ｸﾞﾙｰﾌﾟ数X
    Private miGNy As Short                              ' ｸﾞﾙｰﾌﾟ数Y
    Private mdBSx As Double                             ' ﾌﾞﾛｯｸｻｲｽﾞX
    Private mdBSy As Double                             ' ﾌﾞﾛｯｸｻｲｽﾞY
    Private mdTThetaOff As Double                       ' Tθｵﾌｾｯﾄ
    Private mdTThetaPos1X As Double                     ' Tθ基準位置1X
    Private mdTThetaPos1Y As Double                     ' Tθ基準位置1Y
    Private mdTThetaPos2X As Double                     ' Tθ基準位置2X
    Private mdTThetaPos2Y As Double                     ' Tθ基準位置2Y

#End Region

#Region "終了結果を返す"
    ' '''=========================================================================
    ' '''<summary>終了結果を返す</summary>
    ' '''<returns>cFRS_ERR_START=OK(STARTｷｰ)
    ' '''         cFRS_ERR_RST  =Cancel(RESETｷｰ)
    ' '''         -1以下        =エラー</returns>
    ' '''=========================================================================
    'Public ReadOnly Property sGetReturn() As Integer Implements ICommonMethods.sGetReturn  'V6.0.0.0⑪
    '    'V6.0.0.0⑪    Public ReadOnly Property sGetReturn() As Short
    '    Get
    '        sGetReturn = mExit_flg
    '    End Get
    'End Property
#End Region

#Region "Form Initialize時処理"
    '''=========================================================================
    '''<summary>Form Initialize時処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub Form_Initialize_Renamed()

        'objArrow = New FrmArrow()                           ' 矢印画面ｵﾌﾞｼﾞｪｸﾄ生成

    End Sub
#End Region

#Region "Form 終了時処理"
    '''=========================================================================
    '''<summary>Form 終了時処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub frmTThetaTeach_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        Dim strMSG As String

        Try
            '' オブジェクト開放
            'Call objArrow.Close()                               ' 矢印画面オブジェクト開放
            'Call objArrow.Dispose()                             ' 矢印画面リソース開放
            'objArrow = Nothing

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "frmTThetaTeach.frmTThetaTeach_FormClosed() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "Form Load時処理"
    '''=========================================================================
    '''<summary>Form Load時処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub frmTThetaTeach_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        Dim strMSG As String

        Try
            ' 初期処理
            Call SetDispMsg()                                   ' ラベル等を設定する(日本語/英語)
            mExit_flg = -1                                      ' 終了フラグ = 初期化

            ' ｳｲﾝﾄﾞｳをｺﾝﾄﾛｰﾙに重ねる
            Me.Top = Form1.Text4.Top + DISPOFF_SUBFORM_TOP
            Me.Left = Form1.Text4.Left

            ' トリミングデータより必要なパラメータを取得する
            Call SetTrimData()

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "frmTThetaTeach.frmTThetaTeach_Load() TRAP ERROR = " + ex.Message
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

        With Me
            ' label
            '.lblTitle2.Text = TITLE_T_Theta                 ' ﾀｲﾄﾙ="Tθティーチング"
            '.LblDisp_9.Text = LBL_TXTY_TEACH_15             ' 角度補正(deg)
            ' frame
            '.GrpFram_0.Text = LBL_TXTY_TEACH_12             ' 第１基準点
            '.GrpFram_1.Text = LBL_TXTY_TEACH_13             ' 第２基準点
            '.GrpFram_2.Text = LBL_TXTY_TEACH_03             ' 補正量
            ' button
            '.cmdOK.Text = "OK"
            '.cmdCancel.Text = CMD_CANCEL
        End With

        Call InitDisp()

    End Sub
#End Region

#Region "表示関連初期化"
    '''=========================================================================
    '''<summary>表示関連初期化</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub InitDisp()

        With Me
            .TxtPosX.Text = ""
            .TxtPosY.Text = ""
            .TxtPos2X.Text = ""
            .TxtPos2Y.Text = ""
            .LblResult_1.Text = ""
            .LblResult_2.Text = ""
        End With

    End Sub
#End Region

#Region "必要なﾄﾘﾐﾝｸﾞﾊﾟﾗﾒｰﾀを取得"
    '''=========================================================================
    '''<summary>必要なﾄﾘﾐﾝｸﾞﾊﾟﾗﾒｰﾀを取得</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub SetTrimData()

        Dim strMSG As String

        Try
            ' 必要なﾄﾘﾐﾝｸﾞﾊﾟﾗﾒｰﾀを取得
            mdBpOffx = typPlateInfo.dblBpOffSetXDir             ' BP位置ｵﾌｾｯﾄX,Y設定
            mdBpOffy = typPlateInfo.dblBpOffSetYDir
            mdAdjx = typPlateInfo.dblAdjOffSetXDir              ' ｱｼﾞｬｽﾄﾎﾟｲﾝﾄ位置ｵﾌｾｯﾄX,Y設定
            mdAdjy = typPlateInfo.dblAdjOffSetYDir
            miBNx = typPlateInfo.intBlockCntXDir                ' ﾌﾞﾛｯｸ数X,Y
            miBNy = typPlateInfo.intBlockCntYDir
            miCdir = typPlateInfo.intResistDir                  ' チップ並び方向取得(CHIP-NETのみ)
            'Call GetChipNum(miChpNum)                           ' ﾁｯﾌﾟ数
            miChpNum = typPlateInfo.intResistCntInBlock         ' ブロック内抵抗数
            miGNx = typPlateInfo.intGroupCntInBlockXBp          ' グループ数X,Y
            miGNy = typPlateInfo.intGroupCntInBlockYStage
            Call CalcBlockSize(mdBSx, mdBSy)                    ' ﾌﾞﾛｯｸｻｲｽﾞX,Y
            mdTThetaOff = typPlateInfo.dblTThetaOffset          ' Tθｵﾌｾｯﾄ
            mdTThetaPos1X = typPlateInfo.dblTThetaBase1XDir     ' Tθ基準位置1XY
            mdTThetaPos1Y = typPlateInfo.dblTThetaBase1YDir
            mdTThetaPos2X = typPlateInfo.dblTThetaBase2XDir     ' Tθ基準位置2XY
            mdTThetaPos2Y = typPlateInfo.dblTThetaBase2YDir

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "frmTThetaTeach.SetTrimData() TRAP ERROR = " + ex.Message
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
    Private Sub frmTThetaTeach_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Dim strMSG As String

        Try
            ' Tθﾃｨｰﾁﾝｸﾞ開始
            If (mExit_flg <> -1) Then Exit Sub
            mExit_flg = 0                                       ' 終了フラグ = 0
            mExit_flg = T_ThetaMainProc()                       ' Tθﾃｨｰﾁﾝｸﾞ開始

            ' 補正クロスラインを非表示とする
            'V6.0.0.0④            Form1.CrosLineX.Visible = False
            'V6.0.0.0④            Form1.CrosLineY.Visible = False
            Form1.Instance.VideoLibrary1.SetCorrCrossVisible(False)     'V6.0.0.0④

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "frmTThetaTeach.frmTThetaTeach_Activated() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            mExit_flg = cERR_TRAP                           ' Return値 = 例外エラー
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
            ' Tθﾃｨｰﾁﾝｸﾞ開始
            If (mExit_flg <> -1) Then Exit Function
            mExit_flg = 0                                       ' 終了フラグ = 0
            mExit_flg = T_ThetaMainProc()                       ' Tθﾃｨｰﾁﾝｸﾞ開始

            ' 補正クロスラインを非表示とする
            'V6.0.0.0④            Form1.CrosLineX.Visible = False
            'V6.0.0.0④            Form1.CrosLineY.Visible = False
            Form1.Instance.VideoLibrary1.SetCorrCrossVisible(False)     'V6.0.0.0④

            ' トラップエラー発生時
        Catch ex As Exception
            Dim strMSG As String = "frmTThetaTeach.Execute() TRAP ERROR = " & ex.Message
            MsgBox(strMSG)
            mExit_flg = cERR_TRAP                           ' Return値 = 例外エラー
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

        'stBPMV.Flg = cFRS_ERR_START                           ' OK(STARTｷｰ)

    End Sub
#End Region

#Region "ｷｬﾝｾﾙﾎﾞﾀﾝ押下時処理"
    '''=========================================================================
    '''<summary>ｷｬﾝｾﾙﾎﾞﾀﾝ押下時処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCancel.Click

        'stBPMV.Flg = cFRS_ERR_RST                           ' Cancel(RESETｷｰ)

    End Sub
#End Region

#Region "Ｔθティーチング処理"
    '''=========================================================================
    '''<summary>Ｔθティーチング処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Function T_ThetaMainProc() As Short

        Dim i As Short
        Dim POSX As Double                                  ' 基準位置X
        Dim POSY As Double                                  ' 基準位置X
        Dim workTThetaOff As Double                         ' Tθｵﾌｾｯﾄ
        Dim EffBN As Short                                  ' 有効ﾌﾞﾛｯｸ数
        Dim EffGN As Short                                  ' 有効ｸﾞﾙｰﾌﾟ数
        Dim iPos As Short                                   ' 表示位置
        Dim ZRPosX(1) As Double                             ' ずれ量X(0:第1基準点, 1:第2基準点)
        Dim ZRPosY(1) As Double                             ' ずれ量Y(0:第1基準点, 1:第2基準点)
        Dim CSPointX(1) As Double                           ' PosX算出用(0:第1基準点　1:第2基準点)
        Dim CSPointY(1) As Double                           ' PosY算出用(0:第1基準点　1:第2基準点)
        Dim tmpTThetaOff As Double                      ' 仮Tθオフセット

        Dim TBLx As Double                                  ' ﾃｰﾌﾞﾙ移動座標X(絶対値)
        Dim TBLy As Double                                  ' ﾃｰﾌﾞﾙ移動座標Y(絶対値)

        Dim rtn As Short                                    ' 戻値 
        Dim r As Short
        Dim strMSG As String

        Try
            '-------------------------------------------------------------------
            '   初期処理
            '-------------------------------------------------------------------
            ' ﾁｯﾌﾟ方向から抵抗数を取得
            If miCdir = 0 Then                              ' ﾁｯﾌﾟ並びX方向
                ' 抵抗数X/ｸﾞﾙｰﾌﾟ数Xをｾｯﾄ
                If miGNx = 1 Then
                    EffBN = miChpNum                        ' 抵抗数をｾｯﾄ
                Else
                    EffBN = typGrpInfoArray(1).intGP2       ' ｸﾞﾙｰﾌﾟﾃﾞｰﾀの抵抗数(No.1)をｾｯﾄ
                End If
                EffGN = miGNx
            Else                                            ' ﾁｯﾌﾟ並びY方向
                ' 抵抗数Y/ｸﾞﾙｰﾌﾟ数Yをｾｯﾄ
                If miGNy = 1 Then
                    EffBN = miChpNum                        ' 抵抗数をｾｯﾄ
                Else
                    EffBN = typGrpInfoArray(1).intGP2       ' ｸﾞﾙｰﾌﾟﾃﾞｰﾀの抵抗数(No.1)をｾｯﾄ
                End If
                EffGN = miGNy
            End If
            tmpTThetaOff = mdTThetaOff                      ' HALT処理用にｽﾄｯｸ

            ' 抵抗数が｢1｣の場合は処理できない為、ｴﾗｰとする
            If EffBN <= 1 Then
                ' 抵抗数が１のためこのコマンドは実行できません！
                MsgBox(ERR_TXNUM_E, MsgBoxStyle.Exclamation, "")
                Return (cFRS_ERR_RST)                       ' Return値 = Cancel(RESETｷｰ)
            End If

            ' ずれ量初期化
            For i = 0 To 1
                ZRPosX(i) = 0.0#
                ZRPosY(i) = 0.0#
            Next i

            ' BPを第1ﾌﾞﾛｯｸ、第1抵抗ｽﾀｰﾄﾎﾟｲﾝﾄに移動する
            Call BpMoveOrigin_Ex()                          ' BPをﾌﾞﾛｯｸ右上に移動(原点設定)

            ' 矢印画面(BPのJOG操作)用パラメータを初期化する
            'stBPMV.Md = MODE_BP                             ' モード(1:BP移動)
            'stBPMV.Md2 = 0                                  ' 入力モード(0:画面ﾎﾞﾀﾝ入力, 1:ｺﾝｿｰﾙ入力)
            'stBPMV.Opt = 0                                  ' オプション(0:HALTキー無効, 1:HALTキー有効)
            'stBPMV.BpOffX = mdAdjx + mdBpOffx               ' BPｵﾌｾｯﾄX 
            'stBPMV.BpOffY = mdAdjy + mdBpOffy               ' BPｵﾌｾｯﾄY 
            'stBPMV.BszX = mdBSx                             ' ﾌﾞﾛｯｸｻｲｽﾞX 
            'stBPMV.BszY = mdBSy                             ' ﾌﾞﾛｯｸｻｲｽﾞY
            'stBPMV.TextX = TxtPosX                          ' BP X位置表示用ﾃｷｽﾄﾎﾞｯｸｽ
            'stBPMV.TextY = TxtPosY                          ' BP Y位置表示用ﾃｷｽﾄﾎﾞｯｸｽ
            'objArrow.Show(Me)                               ' 矢印画面表示 

STP_RETRY:
            '-------------------------------------------------------------------
            '   Ｔθティーチング処理
            '-------------------------------------------------------------------
            ' 第1基準点/第2基準点のﾃｨｰﾁﾝｸﾞ
            Call InitDisp()                                 ' 座標表示(ｸﾘｱ)
            iPos = 0                                        ' iPos = 第1基準点
            stJOG.Flg = -1                                  ' 親画面のOK/Cancelﾎﾞﾀﾝ押下ﾌﾗｸﾞ

            Do
                ' 表示メッセージ等を設定する 
                If (iPos = 0) Then                          ' 第1基準点 ? 
                    '"第1グループ、第1抵抗基準位置のティーチング"+"基準位置を合わせて下さい。"+"移動:[矢印]  決定:[START]  中断:[RESET]" "
                    lblInfo.Text = INFO_MSG18 & vbCrLf & INFO_MSG16 & vbCrLf & INFO_MSG17
                    'stBPMV.TextX = TxtPosX                  ' 第1基準点座標X位置表示用ﾃｷｽﾄﾎﾞｯｸｽ
                    'stBPMV.TextY = TxtPosY                  ' 第1基準点座標Y位置表示用ﾃｷｽﾄﾎﾞｯｸｽ
                    TxtPosX.Enabled = True
                    TxtPosY.Enabled = True
                    TBLx = mdTThetaPos1X                    ' 第1基準点座標X
                    TBLy = mdTThetaPos1Y                    ' 第1基準点座標Y
                Else
                    '"第nグループ、最終抵抗基準位置のティーチング"+"基準位置を合わせて下さい。"+"移動:[矢印]  決定:[START]  中断:[RESET]" & vbCrLf & "[HALT]で１つ前の処理に戻ります。"
                    lblInfo.Text = INFO_MSG19 & iPos & INFO_MSG20 & vbCrLf & INFO_MSG16 & vbCrLf & INFO_MSG17
                    'stBPMV.TextX = TxtPos2X                 ' 第2基準点座標X位置表示用ﾃｷｽﾄﾎﾞｯｸｽ
                    'stBPMV.TextY = TxtPos2Y                 ' 第2基準点座標Y位置表示用ﾃｷｽﾄﾎﾞｯｸｽ
                    TxtPos2X.Enabled = True
                    TxtPos2Y.Enabled = True
                    TBLx = mdTThetaPos2X                    ' 第2基準点座標X
                    TBLy = mdTThetaPos2Y                    ' 第2基準点座標Y
                End If

                ' BP絶対値移動(第1基準位置/第2基準位置)
                r = Form1.System1.EX_MOVE(gSysPrm, TBLx, TBLy, 1)
                If (r <> cFRS_NORMAL) Then
                    Return (r)                              ' エラーリターン
                End If

                ' Cancelﾎﾞﾀﾝにﾌｫｰｶｽを設定する
                Me.cmdCancel.Focus()

                ' 第1基準点/第2基準点のﾃｨｰﾁﾝｸﾞ処理
                'stBPMV.PosX = TBLx + ZRPosX(iPos)           ' BP X絶対位置
                'stBPMV.PosY = TBLy + ZRPosY(iPos)           ' BP Y絶対位置
                'stBPMV.Flg = 0                              ' 親画面のOK/Cancelﾎﾞﾀﾝ押下ﾌﾗｸﾞ
                'r = objArrow.JogEzMove(stBPMV)              ' ﾃｨｰﾁﾝｸﾞ処理

                ' ｽﾃｰﾀｽ ﾁｪｯｸ(ｺﾝｿｰﾙｷｰ)
                If (r = cFRS_ERR_START) Then                  ' START SW ?
                    ' 第1基準点/第2基準点の絶対値座標更新
                    'CSPointX(iPos) = stBPMV.PosX
                    'CSPointY(iPos) = stBPMV.PosY
                    ' 第1基準点/第2基準点の絶対値座標更新
                    'ZRPosX(iPos) = ZRPosX(iPos) + stBPMV.cgX
                    'ZRPosY(iPos) = ZRPosY(iPos) + stBPMV.cgY

                    If (iPos >= 1) Then                     ' 第1基準点/第2基準点のﾃｨｰﾁﾝｸﾞ終了 ?
                        Exit Do
                    Else
                        iPos = 1                            ' iPos = 第2基準点のﾃｨｰﾁﾝｸﾞ処理
                    End If

                ElseIf (r = cFRS_ERR_RST) Then              ' RESET SW ?
                    GoTo STP_END                            ' 終了確認メッセージ表示へ
                ElseIf (r < cFRS_NORMAL) Then
                    Return (r)                              ' エラーリターン
                End If
                Call ZCONRST()                              ' ｺﾝｿｰﾙｷｰﾗｯﾁ解除 
            Loop While (stJOG.Flg = -1)

            ' Cancelﾎﾞﾀﾝ押下なら終了確認メッセージ表示へ
            'If (stBPMV.Flg = cFRS_ERR_RST) Then
            '    r = cFRS_ERR_RST
            '    GoTo STP_END
            'End If

            ' PosXYの距離を算出し、Tθ角度を計算
            POSX = System.Math.Abs(CDbl(CSPointX(1) - CSPointX(0)))
            POSY = (CDbl(CSPointY(1) - CSPointY(0)))
            ' θの回転方向は逆になるため反転させる
            workTThetaOff = -(System.Math.Atan(POSY / POSX) * (180 / 3.141592))
            mdTThetaOff = mdTThetaOff + workTThetaOff

            '-------------------------------------------------------------------
            '   結果を表示する
            '-------------------------------------------------------------------
            With Me
                ' 補正角度(補正前/補正後)
                LblResult_1.Text = tmpTThetaOff.ToString("0.00000")
                LblResult_2.Text = mdTThetaOff.ToString("0.00000")
            End With
            r = cFRS_ERR_START                                ' OK(STARTｷｰ) 

STP_END:
            '-------------------------------------------------------------------
            '   終了確認＆ﾄﾘﾐﾝｸﾞﾃﾞｰﾀ更新
            '-------------------------------------------------------------------
            ' 終了確認メッセージ表示 
            ' "前の画面に戻ります。よろしいですか？　　　
            rtn = Form1.System1.TrmMsgBox(gSysPrm, MSG_105, MsgBoxStyle.OkCancel, TITLE_TX)
            If (rtn = cFRS_ERR_START) Then
                '「はい」選択ならﾃﾞｰﾀ更新して終了
                If (r = cFRS_ERR_START) Then                  ' OKボタン押下時 ?
                    ' Tθｵﾌｾｯﾄ更新
                    typPlateInfo.dblTThetaOffset = mdTThetaOff
                    gCmpTrimDataFlg = 1                     ' データ更新フラグ = 1(更新あり)
                End If
            Else
                '「いいえ」選択なら先頭の処理に戻る
                ' ﾃｨｰﾁﾝｸﾞしたﾃﾞｰﾀをｸﾘｱ
                mdTThetaOff = tmpTThetaOff
                For i = 0 To 1
                    ZRPosX(i) = 0.0#
                    ZRPosY(i) = 0.0#
                Next i
                GoTo STP_RETRY
            End If

            Return (rtn)                                    ' Return値設定

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "frmTThetaTeach.T_ThetaMainProc() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                              ' Return値 = 例外エラー
        End Try

    End Function
#End Region

#Region "インターフェース実装(このフォームではNOP)"
    ''' <summary></summary>
    ''' <param name="e"></param>
    ''' <remarks>'V6.0.0.0⑪</remarks>
    Public Sub JogKeyDown(e As KeyEventArgs) Implements ICommonMethods.JogKeyDown
        ' DO NOTHING
    End Sub

    ''' <summary></summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Sub JogKeyUp(e As KeyEventArgs) Implements ICommonMethods.JogKeyUp
        ' DO NOTHING
    End Sub

    ''' <summary></summary>
    ''' <param name="distanceX"></param>
    ''' <param name="distanceY"></param>
    ''' <remarks>'V6.0.0.0⑪</remarks>
    Public Sub MoveToCenter(distanceX As Decimal, distanceY As Decimal) _
        Implements ICommonMethods.MoveToCenter
        ' DO NOTHING
    End Sub
#End Region

End Class