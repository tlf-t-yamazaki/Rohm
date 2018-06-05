'===============================================================================
'   Description  : TY2ティーチング画面処理
'
'   Copyright(C) : OMRON LASERFRONT INC. 2011
'
'===============================================================================
Option Strict Off
Option Explicit On

Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8①
Imports TKY_ALL_SL432HW.My.Resources    'V4.4.0.0-0

Friend Class frmTy2Teach
    Inherits System.Windows.Forms.Form
    Implements ICommonMethods              'V6.0.0.0⑪

#Region "プライベート定数/変数定義"
    '===========================================================================
    '   定数/変数定義
    '===========================================================================
    '----- オブジェクト定義 -----
    Private stJOG As JOG_PARAM                              ' 矢印画面(BPのJOG操作)用パラメータ
    Private mExit_flg As Short                              ' 終了フラグ

    Private EffBN As Short                                  ' 有効ﾌﾞﾛｯｸ数
    Private EffGN As Short                                  ' 有効ｸﾞﾙｰﾌﾟ数
    Private TBLx As Double                                  ' ﾃｰﾌﾞﾙ移動座標X(絶対値)
    Private TBLy As Double                                  ' ﾃｰﾌﾞﾙ移動座標Y(絶対値)
    Private piPositionNum As Short                          ' ｽﾃｯﾌﾟ幅ﾃｰﾌﾞﾙのｴﾝﾄﾘ数-1
    Private pfStartPos(MaxCntResist) As Double              ' ｽﾃｯﾌﾟ幅ﾃｰﾌﾞﾙ(1ｵﾘｼﾞﾝ)
    Private pfSaveStartPos(1, MaxCntResist) As Double       ' ｽﾀｰﾄ位置X(0),Y(1)ﾃｰﾌﾞﾙ(1ｵﾘｼﾞﾝ)
    Private pfIntvalAry(MaxCntResist) As Double             ' 差分テーブル(1ｵﾘｼﾞﾝ) ####119

    '----- トリミングデータ -----
    Private mdTbOffx As Double                              ' ﾃｰﾌﾞﾙ位置ｵﾌｾｯﾄX
    Private mdTbOffy As Double                              ' ﾃｰﾌﾞﾙ位置ｵﾌｾｯﾄY
    Private mdBpOffx As Double                              ' BP位置ｵﾌｾｯﾄX
    Private mdBpOffy As Double                              ' BP位置ｵﾌｾｯﾄY
    Private mdAdjx As Double                                ' ｱｼﾞｬｽﾄ位置ｵﾌｾｯﾄX
    Private mdAdjy As Double                                ' ｱｼﾞｬｽﾄ位置ｵﾌｾｯﾄY
    Private miBNx As Short                                  ' ﾌﾞﾛｯｸ数X
    Private miBNy As Short                                  ' ﾌﾞﾛｯｸ数Y
    Private miCdir As Short                                 ' ﾁｯﾌﾟ並び方向
    Private miGNx As Short                                  ' ｸﾞﾙｰﾌﾟ数X
    Private miGNy As Short                                  ' ｸﾞﾙｰﾌﾟ数Y
    Private mdBSx As Double                                 ' ﾌﾞﾛｯｸｻｲｽﾞX
    Private mdBSy As Double                                 ' ﾌﾞﾛｯｸｻｲｽﾞY
    Private mdSpx As Double                                 ' ｽﾀｰﾄﾎﾟｲﾝﾄX
    Private mdSpy As Double                                 ' ｽﾀｰﾄﾎﾟｲﾝﾄY
    Private mdStageX As Double                              ' ｽﾃｰｼﾞ位置X
    Private mdStageY As Double                              ' ｽﾃｰｼﾞ位置Y

    '----- その他 -----
    Private dblTchMoval(3) As Double                            ' ﾋﾟｯﾁ退避域(0=ﾋﾟｯﾁ, 1=HIGHﾋﾟｯﾁ, 2=Pause Time(Sec))
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
    Private Sub Ty2Teach_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed


    End Sub
#End Region

#Region "Form Load時処理"
    '''=========================================================================
    '''<summary>Form Load時処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub Ty2Teach_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        ' 初期処理
        Call TySetMessages()                                ' ラベル等を設定する(日本語/英語)
        mExit_flg = -1                                      ' 終了フラグ = 初期化

        ' ｳｲﾝﾄﾞｳをｺﾝﾄﾛｰﾙに重ねる
        Me.Top = Form1.Text4.Top + DISPOFF_SUBFORM_TOP
        Me.Left = Form1.Text4.Left

        ' トリミングデータより必要なパラメータを取得する
        Call SetTrimData()

        ' ブロックXYのスタートポジションをgBlkStagePosX()/gBlkStagePosY()に設定する ###119
        Call CalcBlockXYStartPos()

    End Sub
#End Region

#Region "ラベル等を設定する(日本語/英語)"
    '''=========================================================================
    '''<summary>ラベル等を設定する(日本語/英語)</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub TySetMessages()

        'Me.lblTyTitle.Text = TITLE_TY2                                 ' ﾀｲﾄﾙ="ステップサイズ(TY2)ティーチング"
        'Me.GrpTyTeach_1.Text = FRAME_TY2_01                             ' "ティーチングポイント" 
        'Me.GrpTyTeach_2.Text = LBL_TXTY_TEACH_03                       ' "補正量" '###086
        'Me.GrpTyTeach_2.Text = TITLE_TY2                                ' ﾀｲﾄﾙ="ステップサイズ(TY2)ティーチング" '###086
        Me.LblInterval.Text = LBL_TXTY_TEACH_11                         ' "ステップインターバル"
        'Me.cmdCancel.Text = CMD_CANCEL                                  ' "キャンセル (&Q)"
        Me.lblTyInfo.Text = ""

    End Sub
#End Region

#Region "Form Activated時処理"
#If False Then                          'V6.0.0.0⑬ Execute()でおこなう
    '''=========================================================================
    '''<summary>Form Activated時処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub Ty2Teach_Activated(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Activated

        Dim strMSG As String

        Try
            ' TY2ﾃｨｰﾁﾝｸﾞ開始
            If (mExit_flg <> -1) Then Exit Sub
            mExit_flg = 0                                   ' 終了フラグ = 0
            mExit_flg = Ty2Main()                           ' TY2ﾃｨｰﾁﾝｸﾞ開始

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "frmTy2Teach.Ty2Teach_Activated() TRAP ERROR = " + ex.Message
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
            ' TY2ﾃｨｰﾁﾝｸﾞ開始
            If (mExit_flg <> -1) Then Exit Function
            mExit_flg = 0                                   ' 終了フラグ = 0
            mExit_flg = Ty2Main()                           ' TY2ﾃｨｰﾁﾝｸﾞ開始

            ' トラップエラー発生時
        Catch ex As Exception
            Dim strMSG As String = "frmTy2Teach.Execute() TRAP ERROR = " & ex.Message
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

#Region "TY2ティーチング処理"
    '''=========================================================================
    '''<summary>TY2ティーチング処理</summary>
    '''<returns>cFRS_ERR_START=OK(STARTｷｰ)
    '''         cFRS_ERR_RST=Cancel(RESETｷｰ)
    '''         -1以下      =エラー</returns>
    '''=========================================================================
    Private Function Ty2Main() As Short

        Dim BlockCnt As Short                                           ' ﾌﾞﾛｯｸ数ｶｳﾝﾀ
        Dim i As Short
        Dim r As Short
        Dim rtn As Short                                                ' TyMainProc戻値 
        Dim strMSG As String

        Try
            '-------------------------------------------------------------------
            '   初期処理
            '-------------------------------------------------------------------
            ' ｽﾃｯﾌﾟ幅ﾃｰﾌﾞﾙ作成
            rtn = 0                                                     ' TyMain戻値 
            Call TyTeachingInitialize()

            ' ｽﾃｯﾌﾟｸﾞﾘｯﾄﾞの初期化
            Call GridInitialize()

            ' ﾁｯﾌﾟ方向からﾌﾞﾛｯｸ数を取得
            If miCdir = 0 Then
                ' ﾁｯﾌﾟ並びX方向:ﾌﾞﾛｯｸ数Y/ｸﾞﾙｰﾌﾟ数Yをｾｯﾄ
                EffBN = miBNy                                           ' ﾌﾞﾛｯｸ数Y
                EffGN = miGNy                                           ' ｸﾞﾙｰﾌﾟ数Y
            Else
                ' ﾁｯﾌﾟ並びY方向:ﾌﾞﾛｯｸ数X/ｸﾞﾙｰﾌﾟ数Xをｾｯﾄ
                EffBN = miBNx                                           ' ﾌﾞﾛｯｸ数X
                EffGN = miGNx                                           ' ｸﾞﾙｰﾌﾟ数X
            End If

            ' 先頭ﾌﾞﾛｯｸの先頭基準位置を取得する
            Call BpMoveOrigin_Ex()                                      ' BPをﾌﾞﾛｯｸの右上に移動する
            r = Form1.System1.EX_MOVE(gSysPrm, mdSpx, mdSpy, 1)
            If (r < cFRS_NORMAL) Then
                Return (r)                                              ' Return値 = エラー
            End If

            ' 矢印画面(XYﾃｰﾌﾞﾙのJOG操作)用パラメータを初期化する
            stJOG.Md = MODE_STG                                         ' モード(0:XYテーブル移動)
            stJOG.Md2 = MD2_BUTN                                        ' 入力モード(0:画面ﾎﾞﾀﾝ入力, 1:ｺﾝｿｰﾙ入力)
            stJOG.Opt = CONSOLE_SW_RESET + CONSOLE_SW_START             ' キーの有効(1)/無効(0)指定
            stJOG.PosX = 0.0                                            ' BP X位置/XYﾃｰﾌﾞﾙ X位置
            stJOG.PosY = 0.0                                            ' BP Y位置/XYﾃｰﾌﾞﾙ Y位置
            stJOG.BpOffX = mdAdjx + mdBpOffx                            ' BPｵﾌｾｯﾄX 
            stJOG.BpOffY = mdAdjy + mdBpOffy                            ' BPｵﾌｾｯﾄY 
            stJOG.BszX = mdBSx                                          ' ﾌﾞﾛｯｸｻｲｽﾞX 
            stJOG.BszY = mdBSy                                          ' ﾌﾞﾛｯｸｻｲｽﾞY
            stJOG.TextX = TxtPosX                                       ' XYﾃｰﾌﾞﾙ X位置表示用ﾃｷｽﾄﾎﾞｯｸｽ
            stJOG.TextY = TxtPosY                                       ' XYﾃｰﾌﾞﾙ Y位置表示用ﾃｷｽﾄﾎﾞｯｸｽ
            stJOG.cgX = 0.0                                             ' 移動量X 
            stJOG.cgY = 0.0                                             ' 移動量Y 
            stJOG.BtnHI = BtnHI                                         ' HIボタン
            stJOG.BtnZ = BtnZ                                           ' Zボタン
            stJOG.BtnSTART = BtnSTART                                   ' STARTボタン
            stJOG.BtnHALT = BtnHALT                                     ' HALTボタン
            stJOG.BtnRESET = BtnRESET                                   ' RESETボタン
            Call JogEzInit(stJOG, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
            Me.KeyPreview = True
            Call Me.Focus()

STP_RETRY:
            '-------------------------------------------------------------------
            '   Ｙ方向の各ブロックのステップ幅のティーチング処理を行う
            '-------------------------------------------------------------------
            BlockCnt = 1                                                ' ﾌﾞﾛｯｸ数ｶｳﾝﾀ
            stJOG.Flg = -1                                              ' 親画面のOK/Cancelﾎﾞﾀﾝ押下ﾌﾗｸﾞ初期化
            stJOG.Md = MODE_STG                                         ' モード(0:XYテーブル移動)

            Do
                ' ｸﾞﾙｰﾌﾟ数分の処理完了？
                If (BlockCnt > EffBN) Then
                    Exit Do
                End If

                ' 座標表示クリア
                Me.TxtPosX.Text = ""
                Me.TxtPosY.Text = ""

                ' XYﾃｰﾌﾞﾙ移動(第[BlockCnt]ﾌﾞﾛｯｸ)
                r = XYTableMoveSetBlock(BlockCnt)                       ' ﾃｰﾌﾞﾙ位置 → TBLx, TBLy
                If (r < cFRS_NORMAL) Then
                    Return (r)                                          ' エラーリターン
                End If

                If (BlockCnt = EffBN) Then
                    '"最終"+"ブロックのティーチング"+"基準位置を合わせて下さい。"+"移動：[矢印]  決定：[START]  中断：[RESET]" 
                    Me.lblTyInfo.Text = INFO_MSG26 & INFO_MSG25 & vbCrLf & INFO_MSG16 & vbCrLf & INFO_MSG17
                Else
                    '"第"BlockCnt"ブロックのティーチング"+"基準位置を合わせて下さい。"+"移動：[矢印]  決定：[START]  中断：[RESET]" 
                    Me.lblTyInfo.Text = INFO_MSG24 & BlockCnt & INFO_MSG25 & vbCrLf & INFO_MSG16 & vbCrLf & INFO_MSG17
                End If

                ' ティーチング処理
                stJOG.PosX = TBLx                                       ' XYﾃｰﾌﾞﾙ X位置
                stJOG.PosY = TBLy                                       ' XYﾃｰﾌﾞﾙ Y位置
                TxtPosX.Enabled = True
                TxtPosY.Enabled = True
                'V6.0.0.0⑪                r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
                r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval, Me)  'V6.0.0.0⑪
                If (r < cFRS_NORMAL) Then                               ' エラーなら終了
                    Return (r)
                ElseIf (r = cFRS_ERR_RST) Then                          ' RESET SW ?
                    GoTo STP_END                                        ' 終了確認メッセージ表示へ
                ElseIf (r = cFRS_ERR_START) Then                        ' START SW ?
                    ' 第[BlockCnt]ﾌﾞﾛｯｸのXYﾃｰﾌﾞﾙ絶対値座標更新 ###119
                    If (BlockCnt > 1) Then
                        Call SetStartPosTbl(BlockCnt)                   ' 差分テーブル更新
                        ' グリッドに1つ前の値を設定する
                        Me.FGridTy2Teach.set_TextMatrix(BlockCnt - 1, 1, (pfStartPos(BlockCnt - 1) + pfIntvalAry(BlockCnt - 1)).ToString("0.0000 "))
                        dgvTy2Teach(1, BlockCnt - 2).Value = (pfStartPos(BlockCnt - 1) + pfIntvalAry(BlockCnt - 1)).ToString("0.0000 ")     ' V4.0.0.0⑧
                        'If (BlockCnt = EffBN) Then
                        '    ' 最終ブロックの場合
                        '    Call SetStartPosTbl(BlockCnt)               ' ステップ幅テーブル更新
                        '    ' グリッドに値を設定する。
                        '    Me.FGridTy2Teach.set_TextMatrix(BlockCnt - 1, 1, pfStartPos(BlockCnt - 1).ToString("0.0000 "))
                        'Else
                        '    ' 最終ブロックでない場合
                        '    Call SetStartPosTbl(BlockCnt)               ' 差分テーブル更新
                        '    ' グリッドに値を設定する
                        '    Me.FGridTy2Teach.set_TextMatrix(BlockCnt, 1, (pfStartPos(BlockCnt) + pfIntvalAry(BlockCnt)).ToString("0.0000 "))
                        'End If
                    End If

                    ' グリッドの先頭行番号を設定する
                    If BlockCnt <= 5 Then
                        Call SetTopRow(1)
                    Else
                        Call SetTopRow(BlockCnt - 5)
                    End If

                    ' ﾌﾞﾛｯｸ数ｶｳﾝﾀ更新
                    BlockCnt = BlockCnt + 1
                End If

            Loop While (stJOG.Flg = -1)

            ' 当画面からOK(START)/RESET(Cancel)ﾎﾞﾀﾝ押下ならrに戻値を設定する
            If (stJOG.Flg <> -1) Then
                r = stJOG.Flg
                If (r = cFRS_ERR_RST) Then                              ' RESET(Cancel)ﾎﾞﾀﾝ押下なら終了確認メッセージ表示へ
                    GoTo STP_END
                End If
            End If

            '-------------------------------------------------------------------
            '   STARTキー/RESETキー押下待ち
            '-------------------------------------------------------------------
            lblTyInfo.Text = INFO_REC                                   ' "登録:[START] キャンセル:[RESET]"
            stJOG.Md = MODE_KEY                                         ' モード=キー入力待ちモード
            stJOG.Flg = -1                                              ' 親画面のOK/Cancelﾎﾞﾀﾝ押下ﾌﾗｸﾞ初期化
            Do
                ' STARTｷｰ/RESETｷｰ 押下待ち
                'V6.0.0.0⑪                r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
                r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval, Me)  'V6.0.0.0⑪
                If (r = cFRS_ERR_START) Then                            ' OK(STARTキー) ?
                    r = cFRS_ERR_START
                    Exit Do
                ElseIf (r = cFRS_ERR_RST) Then                          ' Cancel(RESETｷｰ) ?
                    r = cFRS_ERR_RST
                    Exit Do
                Else                                                    ' その他のエラー 
                    Return (r)
                End If
            Loop
STP_END:
            '-------------------------------------------------------------------
            '   終了確認＆トリミングデータ更新
            '-------------------------------------------------------------------
            ' "前の画面に戻ります。よろしいですか？　
            rtn = Form1.System1.TrmMsgBox(gSysPrm, MSG_105, MsgBoxStyle.OkCancel, MSG_OPLOG_TY2_START)
            If (rtn = cFRS_ERR_START) Then
                '「はい」選択ならﾃﾞｰﾀ更新して終了
                If (r = cFRS_ERR_START) Then                            ' OKボタン押下時 ?
                    Call sUpdateStartPosition()                         ' データ更新
                    gCmpTrimDataFlg = 1                                 ' データ更新フラグ = 1(更新あり)
                End If
            Else
                '「いいえ」選択なら先頭の処理に戻る
                ' 補正値クリア
                For i = 1 To Me.FGridTy2Teach.Rows - 1
                    Me.FGridTy2Teach.set_TextMatrix(i, 1, "")
                Next i
                For i = 0 To (dgvTy2Teach.Rows.Count - 1) Step 1        ' V4.0.0.0⑧
                    dgvTy2Teach(1, i).Value = String.Empty
                Next i
                ' ティーチングしたデータをクリア
                Call sDefSetData()                                      ' ステップ距離のグリッド値をトリミングデータより表示する
                Call SetTopRow(1)
                GoTo STP_RETRY
            End If

            Return (r)                                                  ' Return値設定

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "frmTy2Teach.Ty2Main() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return値 = 例外エラー
        End Try
    End Function
#End Region

#Region "指定ブロックのステップ幅テーブルを更新する"
    '''=========================================================================
    ''' <summary>指定ブロックのステップ幅テーブルを更新する</summary>
    ''' <param name="bn">   (INP)ブロック番号(1 ORG)</param>
    '''=========================================================================
    Private Sub SetStartPosTbl(ByVal bn As Short)

        Dim strMSG As String
        Dim Idx As Short
        Dim wkDBL As Double = 0.0

        Try
            '------ ###119↓ -----
            ' ブロック番号が１以上の時に設定する
            If (bn <= 1) Then Exit Sub

            ' ティーチング座標の差分を求める
            If (miCdir = 0) Then                                        ' チップ並びX方向
                wkDBL = CDbl(Me.TxtPosY.Text) - pfSaveStartPos(1, bn - 1)
                pfIntvalAry(bn - 1) = wkDBL - pfStartPos(bn - 1)

            Else
                'pfIntvalAry(bn - 1) = pfSaveStartPos(0, bn) - CDbl(Me.TxtPosX.Text)
                wkDBL = CDbl(Me.TxtPosX.Text) - pfSaveStartPos(0, bn - 1)
                pfIntvalAry(bn - 1) = wkDBL - pfStartPos(bn - 1)
            End If

            ' 少数桁4桁にする(ゴミが入る場合がある為)
            strMSG = pfIntvalAry(bn - 1).ToString("0.0000")
            pfIntvalAry(bn - 1) = Double.Parse(strMSG)

            ' ティーチング座標配列を更新する
            For Idx = bn To MaxTy2                                  ' ブロック数分繰り返す 
                If (miCdir = 0) Then                                    ' チップ並び方向 = X方向の場合
                    pfSaveStartPos(1, Idx) = pfSaveStartPos(1, Idx) + pfIntvalAry(bn - 1)
                Else
                    pfSaveStartPos(0, Idx) = pfSaveStartPos(0, Idx) + pfIntvalAry(bn - 1)
                End If
            Next

            '' ブロック番号が１以上の時に設定する
            'If (bn <= 1) Then Exit Sub

            'If (miCdir = 0) Then                            ' ﾁｯﾌﾟ並びX方向
            '    pfStartPos(bn - 1) = pfSaveStartPos(1, bn) - pfSaveStartPos(1, bn - 1)
            '    Select Case gSysPrm.stDEV.giBpDirXy
            '        Case 0 ' x←, y↓
            '            pfStartPos(bn - 1) = pfStartPos(bn - 1)
            '        Case 1 ' x→, y↓
            '            pfStartPos(bn - 1) = pfStartPos(bn - 1)
            '        Case 2 ' x←, y↑
            '            pfStartPos(bn - 1) = -pfStartPos(bn - 1)
            '        Case 3 ' x→, y↑
            '            pfStartPos(bn - 1) = -pfStartPos(bn - 1)
            '    End Select
            'Else                                            ' ﾁｯﾌﾟ並びY方向
            '    pfStartPos(bn - 1) = pfSaveStartPos(0, bn) - pfSaveStartPos(0, bn - 1)
            '    Select Case gSysPrm.stDEV.giBpDirXy
            '        Case 0 ' x←, y↓
            '            pfStartPos(bn - 1) = pfStartPos(bn - 1)
            '        Case 1 ' x→, y↓
            '            pfStartPos(bn - 1) = -pfStartPos(bn - 1)
            '        Case 2 ' x←, y↑
            '            pfStartPos(bn - 1) = pfStartPos(bn - 1)
            '        Case 3 ' x→, y↑
            '            pfStartPos(bn - 1) = -pfStartPos(bn - 1)
            '    End Select
            'End If
            '------ ###119↑ -----

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "frmTy2Teach.SetStartPosTbl() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "トリミングデータより必要なパラメータを取得する"
    '''=========================================================================
    ''' <summary>トリミングデータより必要なパラメータを取得する</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub SetTrimData()

        Dim strMSG As String = ""

        ' ﾃｰﾌﾞﾙ位置ｵﾌｾｯﾄX,Yの取得
        mdTbOffx = typPlateInfo.dblTableOffsetXDir
        mdTbOffy = typPlateInfo.dblTableOffsetYDir
        ' BP位置ｵﾌｾｯﾄX,Y設定
        mdBpOffx = typPlateInfo.dblBpOffSetXDir
        mdBpOffy = typPlateInfo.dblBpOffSetYDir
        ' ｱｼﾞｬｽﾄﾎﾟｲﾝﾄ位置ｵﾌｾｯﾄX,Y設定
        mdAdjx = typPlateInfo.dblAdjOffSetXDir
        mdAdjy = typPlateInfo.dblAdjOffSetYDir
        ' ﾌﾞﾛｯｸ数X,Y
        miBNx = typPlateInfo.intBlockCntXDir
        miBNy = typPlateInfo.intBlockCntYDir
        miCdir = typPlateInfo.intResistDir                  ' チップ並び方向取得(CHIP-NETのみ)

        miGNx = typPlateInfo.intGroupCntInBlockXBp          ' グループ数X,Y
        miGNy = typPlateInfo.intGroupCntInBlockYStage

        Call CalcBlockSize(mdBSx, mdBSy)                    ' ﾌﾞﾛｯｸｻｲｽﾞ
        Call GetCutStartPoint(1, 1, mdSpx, mdSpy)           ' ｽﾀｰﾄﾎﾟｲﾝﾄ

        mdStageX = gSysPrm.stDEV.gfTrimX + mdTbOffx + gfCorrectPosX
        mdStageY = gSysPrm.stDEV.gfTrimY + mdTbOffy + gfCorrectPosY

        Select Case gSysPrm.stDEV.giBpDirXy
            Case 0 ' x←, y↓
                mdStageX = mdStageX + (mdBSx / 2)
                mdStageY = mdStageY + (mdBSy / 2)
            Case 1 ' x→, y↓
                mdStageX = mdStageX - (mdBSx / 2)
                mdStageY = mdStageY + (mdBSy / 2)
            Case 2 ' x←, y↑
                mdStageX = mdStageX + (mdBSx / 2)
                mdStageY = mdStageY - (mdBSy / 2)
            Case 3 ' x→, y↑
                mdStageX = mdStageX - (mdBSx / 2)
                mdStageY = mdStageY - (mdBSy / 2)
        End Select

        ' 少数桁4桁にする(ゴミが入る場合がある為) ###119
        strMSG = mdStageX.ToString("0.0000")
        mdStageX = Double.Parse(strMSG)
        strMSG = mdStageY.ToString("0.0000")
        mdStageY = Double.Parse(strMSG)

    End Sub
#End Region

#Region "トリミングデータを更新する"
    '''=========================================================================
    '''<summary>トリミングデータを更新する</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub sUpdateStartPosition()

        Dim intForCnt As Short

        ' TY2データのステップ距離を更新する
        For intForCnt = 1 To (MaxTy2 - 1)                               ' ブロック数分設定する 
            ' TY2データのステップ距離にはインターバル値(チップサイズ等を引いた分)を設定する ###119
            'typTy2InfoArray(intForCnt).dblTy22 = pfStartPos(intForCnt)
            typTy2InfoArray(intForCnt).dblTy22 = typTy2InfoArray(intForCnt).dblTy22 + pfIntvalAry(intForCnt)
        Next

        ' ブロックのX方向、Y方向の開始位置を再設定する ###119
        Call CalcBlockXYStartPos()

    End Sub
#End Region

#Region "ｽﾃｯﾌﾟ幅ﾃｰﾌﾞﾙ作成"
    '''=========================================================================
    ''' <summary>ｽﾃｯﾌﾟ幅ﾃｰﾌﾞﾙ作成</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub TyTeachingInitialize()

        'Dim i As Short
        'Dim rn As Short

        '----- ###119↓ -----
        ' ステップ距離配列(pfStartPos())を設定する 
        Call SetStartPosAry()
        piPositionNum = MaxTy2                                          ' ｽﾃｯﾌﾟ幅ﾃｰﾌﾞﾙのｴﾝﾄﾘ数 

        ' ティーチング座標配列(pfSaveStartPos())に全ブロックのXY座標を設定する
        Call SetSaveStartPosAry()

        '' ステップ幅ﾃｰﾌﾞﾙ作成
        'piPositionNum = 0
        'For i = 1 To MaxTy2
        '    rn = typTy2InfoArray(i).intTy21                             ' ブロック番号
        '    If rn < 1 Then Exit For
        '    piPositionNum = piPositionNum + 1
        '    ' ステップ距離
        '    pfStartPos(piPositionNum) = typTy2InfoArray(i).dblTy22      ' ステップ距離
        'Next
        'piPositionNum = piPositionNum - 1
        '----- ###119↑ -----

    End Sub
#End Region

#Region "ﾌﾞﾛｯｸ移動(ﾃｰﾌﾞﾙ移動)"
    '''=========================================================================
    ''' <summary>ﾌﾞﾛｯｸ移動(ﾃｰﾌﾞﾙ移動)</summary>
    ''' <param name="intBlockNum">(INP)ブロック番号(1 ORG)</param>
    '''=========================================================================
    Private Function XYTableMoveSetBlock(ByRef intBlockNum As Short) As Short

        'Dim X As Double
        'Dim Y As Double
        'Dim intForCnt As Short
        'Dim dblInterval As Double
        Dim strMSG As String
        Dim r As Short

        Try
            '------ ###119↓ -----
            ' XYテーブルを指定のブロック位置に移動する
            TBLx = pfSaveStartPos(0, intBlockNum)                       ' テーブル位置X,Yをグローバル域に設定する
            TBLy = pfSaveStartPos(1, intBlockNum)
            r = Form1.System1.XYtableMove(gSysPrm, TBLx, TBLy)          ' テーブル移動(絶対値)
            Return (r)                                                  ' Return値設定

            '' 次のブロックへXY移動
            'dblInterval = 0.0
            'Select Case gSysPrm.stDEV.giBpDirXy
            '    Case 0 ' x←, y↓
            '        ' ﾁｯﾌﾟ方向のﾁｪｯｸを行う。
            '        If (0 = miCdir) Then                        ' X方向
            '            X = mdStageX
            '            For intForCnt = 1 To intBlockNum
            '                dblInterval = dblInterval + pfStartPos(intForCnt)
            '            Next
            '            Y = mdStageY + dblInterval
            '        Else                                        ' Y方向
            '            For intForCnt = 1 To intBlockNum
            '                dblInterval = dblInterval + pfStartPos(intForCnt)
            '            Next
            '            X = mdStageX + dblInterval
            '            Y = mdStageY
            '        End If

            '    Case 1 ' x→, y↓
            '        ' ﾁｯﾌﾟ方向のﾁｪｯｸを行う。
            '        If (0 = miCdir) Then                        ' X方向
            '            X = mdStageX
            '            For intForCnt = 1 To intBlockNum
            '                dblInterval = dblInterval + pfStartPos(intForCnt)
            '            Next
            '            Y = mdStageY + dblInterval
            '        Else                                        ' Y方向
            '            For intForCnt = 1 To intBlockNum
            '                dblInterval = dblInterval + pfStartPos(intForCnt)
            '            Next
            '            X = mdStageX - dblInterval
            '            Y = mdStageY
            '        End If

            '    Case 2 ' x←, y↑
            '        ' ﾁｯﾌﾟ方向のﾁｪｯｸを行う。
            '        If (0 = miCdir) Then                        ' X方向
            '            X = mdStageX
            '            For intForCnt = 1 To intBlockNum
            '                dblInterval = dblInterval + pfStartPos(intForCnt)
            '            Next
            '            Y = mdStageY - dblInterval
            '        Else                                        ' Y方向
            '            For intForCnt = 1 To intBlockNum
            '                dblInterval = dblInterval + pfStartPos(intForCnt)
            '            Next
            '            X = mdStageX + dblInterval
            '            Y = mdStageY
            '        End If
            '    Case 3 ' x→, y↑
            '        ' ﾁｯﾌﾟ方向のﾁｪｯｸを行う。
            '        If (0 = miCdir) Then                        ' X方向
            '            X = mdStageX
            '            For intForCnt = 1 To intBlockNum
            '                dblInterval = dblInterval + pfStartPos(intForCnt)
            '            Next
            '            Y = mdStageY - dblInterval
            '        Else                                        ' Y方向
            '            For intForCnt = 1 To intBlockNum
            '                dblInterval = dblInterval + pfStartPos(intForCnt)
            '            Next
            '            X = mdStageX - dblInterval
            '            Y = mdStageY
            '        End If
            'End Select

            'TBLx = X
            'TBLy = Y
            'r = Form1.System1.XYtableMove(gSysPrm, TBLx, TBLy)
            'Return (r)                                      ' Return値設定
            '------ ###119↑ -----

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "frmTy2Teach.XYTableMoveSetBlock() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                              ' Return値 = 例外エラー
        End Try

    End Function
#End Region

#Region "ｸﾞﾘｯﾄﾞの初期化"
    '''=========================================================================
    '''<summary>ｸﾞﾘｯﾄﾞの初期化</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub GridInitialize()

        Dim head(4) As String
        Dim strHead As String
        Dim intForCntRows As Short
        Dim maxRows As Short

        If (0 = gSysPrm.stTMN.giMsgTyp) Then    'V4.4.0.0-0 FlexGrid削除時に削除する
            head(0) = "ﾌﾞﾛｯｸ番号"                                 ' ﾌﾟﾛｯｸ番号
            head(1) = "ｽﾃｯﾌﾟ距離"                                 ' ｽﾃｯﾌﾟ距離
        Else
            head(0) = "BLOCK NO."                                 ' ﾌﾟﾛｯｸ番号
            head(1) = "Step Interval"                                 ' ｽﾃｯﾌﾟ距離
        End If

        ' 見出し
        'head(0) = LBL_TY2_1                                 ' ﾌﾟﾛｯｸ番号
        'head(1) = LBL_TY2_2                                 ' ｽﾃｯﾌﾟ距離
        strHead = head(0) & "|" & head(1)

        ' グリッドの最大行数設定
        If piPositionNum < 25 Then                          ' ｽﾃｯﾌﾟ幅ﾃｰﾌﾞﾙのｴﾝﾄﾘ数 
            maxRows = 25
        Else
            maxRows = piPositionNum + 1
        End If

        ' グリッドの初期設定
        With Me.FGridTy2Teach
            .FormatString = strHead                         ' 見出し
            .Cols = 2                                       ' 最大列数
            .FixedCols = 1                                  ' 固定列数
            .Rows = maxRows                                 ' 最大行数
            .FixedRows = 1                                  ' 固定行数

            For intForCntRows = 1 To maxRows - 1
                .Row = intForCntRows
                .Col = 1
                .CellAlignment = MSFlexGridLib.AlignmentSettings.flexAlignRightCenter
            Next

            ' ブロック番号/ｽﾃｯﾌﾟ幅の初期値表示
            For intForCntRows = 1 To piPositionNum
                .set_TextMatrix(intForCntRows, 0, CStr(intForCntRows))
                .set_TextMatrix(intForCntRows, 1, pfStartPos(intForCntRows).ToString("0.0000 "))
            Next intForCntRows

        End With

        ' グリッドの初期設定
        With dgvTy2Teach                ' V4.0.0.0⑧
            .SuspendLayout()
            '.Columns(0).HeaderText = head(0)
            '.Columns(1).HeaderText = head(1)
            .Rows.Add(maxRows - 1)

            ' ブロック番号/ｽﾃｯﾌﾟ幅の初期値表示
            For i As Integer = 1 To piPositionNum Step 1
                With .Rows(i - 1)
                    .Cells(0).Value = i.ToString()
                    .Cells(1).Value = pfStartPos(i).ToString("0.0000 ")
                End With
            Next i
            .ResumeLayout()
        End With

        Call SetTopRow(1)                                   ' グリッドの先頭行番号を設定する

    End Sub
#End Region

#Region "グリッドの先頭行番号を設定する"
    '''=========================================================================
    '''<summary>グリッドの先頭行番号を設定する</summary>
    '''<param name="iRow">(INP)行番号</param>
    '''=========================================================================
    Private Sub SetTopRow(ByVal iRow As Integer)

        With Me.FGridTy2Teach
            .TopRow = iRow
        End With

        dgvTy2Teach.FirstDisplayedScrollingRowIndex = (iRow - 1) ' V4.0.0.0⑧

    End Sub
#End Region

#Region "グリッドの指定行の背景色を黄色にする"
    '''=========================================================================
    '''<summary>指定行の背景色を黄色にする</summary>
    '''<param name="iRow">(INP)行</param>
    '''=========================================================================
    Private Sub SetBackColor(ByVal iRow As Integer)

        Static iPrevColumn As Integer = 0

        'If IsNothing(iPrevColumn) Then
        '    iPrevColumn = 0
        'End If

        With Me.FGridTy2Teach

            If iPrevColumn >= 0 Then
                If .Rows > iPrevColumn Then
                    .Row = iPrevColumn
                    .Col = 1
                    .CellBackColor = System.Drawing.Color.White
                    .Col = 2
                    .CellBackColor = System.Drawing.Color.White
                End If
            End If

            .Row = iRow
            .Col = 1
            .CellBackColor = System.Drawing.Color.Yellow
            .Col = 2
            .CellBackColor = System.Drawing.Color.Yellow

        End With

        With dgvTy2Teach                ' V4.0.0.0⑧
            .SuspendLayout()
            If (0 < iPrevColumn) Then
                If (iPrevColumn < .CurrentRow.Index) Then
                    With .Rows(iPrevColumn - 1)
                        '.Cells(0).Style.BackColor = SystemColors.Window
                        .Cells(1).Style.BackColor = SystemColors.Window
                    End With
                End If
            End If
            With .Rows(iRow - 1)
                '.Cells(0).Style.BackColor = Color.Yellow
                .Cells(1).Style.BackColor = Color.Yellow
            End With
            .ResumeLayout()
        End With

        iPrevColumn = iRow

    End Sub
#End Region

#Region "グリッドの値を更新する"
    '''=========================================================================
    '''<summary>ステップ距離のグリッドの値を更新する</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub sUpdateGridData()

        Dim intForCnt As Integer

        ' ステップ距離のグリッドの値を更新する
        For intForCnt = 1 To MaxTy2 - 1
            Me.FGridTy2Teach.set_TextMatrix(intForCnt, 1, pfStartPos(intForCnt).ToString("0.0000 "))

            dgvTy2Teach(1, intForCnt - 1).Value = pfStartPos(intForCnt).ToString("0.0000 ")         ' V4.0.0.0⑧
        Next

    End Sub
#End Region

#Region "ステップ距離のグリッド値をトリミングデータより表示する"
    '''=========================================================================
    '''<summary>ステップ距離のグリッド値をトリミングデータより表示する</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub sDefSetData()

        Dim intForCnt As Integer

        ' ステップ距離配列(pfStartPos())を設定する ###119
        Call SetStartPosAry()

        ' ティーチング座標配列(pfSaveStartPos())に全ブロックのXY座標を設定する ###119
        Call SetSaveStartPosAry()

        ' ステップ距離のグリッドの値を表示する
        'For intForCnt = 1 To MaxTy2                                     ' ブロック数分設定する  
        For intForCnt = 1 To MaxTy2 - 1                                 ' ブロック数分設定する        V4.0.0.0⑧
            'pfStartPos(intForCnt) = typTy2InfoArray(intForCnt).dblTy22 ' ###119 
            Me.FGridTy2Teach.set_TextMatrix(intForCnt, 1, pfStartPos(intForCnt).ToString("0.0000 "))

            dgvTy2Teach(1, intForCnt - 1).Value = pfStartPos(intForCnt).ToString("0.0000 ")         ' V4.0.0.0⑧
        Next

    End Sub
#End Region

#Region "ステップ距離配列(pfStartPos())を設定する"
    '''=========================================================================
    ''' <summary>ステップ距離配列(pfStartPos())を設定する ###119</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub SetStartPosAry()

        Dim Idx As Short
        Dim strMSG As String

        ' ステップ距離配列(pfStartPos())をgBlkStagePosX()/gBlkStagePosY()から設定する
        For Idx = 1 To MaxTy2 - 1                                       ' ブロック数分設定する 
            If (miCdir = 0) Then                                        ' チップ並び方向 = X方向の場合
                pfStartPos(Idx) = gBlkStagePosY(Idx) - gBlkStagePosY(Idx - 1)
            Else                                                        ' チップ並び方向 = Y方向の場合
                pfStartPos(Idx) = gBlkStagePosX(Idx) - gBlkStagePosX(Idx - 1)
            End If

            ' 少数桁4桁にする(ゴミが入る場合がある為)
            strMSG = pfStartPos(Idx).ToString("0.0000")
            pfStartPos(Idx) = Double.Parse(strMSG)
        Next Idx

        ' ステップ距離配列(最終ブロック)は初期化する
        pfStartPos(MaxTy2) = 0.0

    End Sub
#End Region

#Region "ティーチング座標配列(pfSaveStartPos())を設定する"
    '''=========================================================================
    ''' <summary>ティーチング座標配列(pfSaveStartPos())を設定する ###119</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub SetSaveStartPosAry()

        Dim Idx As Short

        ' ティーチング座標配列に全ブロックのXY座標を設定する
        For Idx = 0 To (MaxTy2 - 1)                                     ' ブロック数分繰り返す 
            If (miCdir = 0) Then                                        ' チップ並び方向 = X方向の場合
                pfSaveStartPos(0, Idx + 1) = mdStageX
                pfSaveStartPos(1, Idx + 1) = mdStageY + gBlkStagePosY(Idx)
            Else
                pfSaveStartPos(0, Idx + 1) = mdStageX + gBlkStagePosX(Idx)
                pfSaveStartPos(1, Idx + 1) = mdStageY
            End If

            ' 差分テーブル(1ｵﾘｼﾞﾝ)初期化
            pfIntvalAry(Idx) = 0.0
        Next Idx
        pfIntvalAry(MaxTy2) = 0.0

        '' ティーチング座標配列0に第1ブロックのXY座標を設定する
        'pfSaveStartPos(0, 0) = pfSaveStartPos(0, 1)
        'pfSaveStartPos(1, 0) = pfSaveStartPos(1, 1)

    End Sub
#End Region

#Region "DataGridViewが選択状態の色になるのをキャンセルする"
    '''=========================================================================
    ''' <summary>DataGridViewが選択状態の色になるのをキャンセルする</summary>
    '''=========================================================================
    Private Sub dgvTy2Teach_SelectionChanged(sender As System.Object, e As System.EventArgs) Handles dgvTy2Teach.SelectionChanged
        dgvTy2Teach.Rows(dgvTy2Teach.CurrentRow.Index).Selected = False ' V4.0.0.0⑧
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
    Private Sub frmTy2Teach_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        JogKeyDown(e)                   'V6.0.0.0⑪
    End Sub

    Public Sub JogKeyDown(ByVal e As KeyEventArgs) Implements ICommonMethods.JogKeyDown    'V6.0.0.0⑪
        'V6.0.0.0⑪        Dim KeyCode As Short = e.KeyCode
        Dim KeyCode As Keys = e.KeyCode             'V6.0.0.0⑪
        Console.WriteLine("frmTy2Teach.frmTy2Teach_KeyDown()")

        ' テンキーダウンならInpKeyにテンキーコードを設定する
        'V6.0.0.0⑫        GrpTyTeach_2.Enabled = False                                    ' ###086
        'V6.0.0.0⑫       'Call Sub_10KeyDown(KeyCode)
        Sub_10KeyDown(KeyCode, stJOG)             'V6.0.0.0⑫
        If (KeyCode = System.Windows.Forms.Keys.NumPad5) Then           ' 5ｷｰ (KeyCode = 101(&H65)
            'V6.0.0.0⑪        Dim KeyCode As Short = e.KeyCode
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
    Private Sub frmTy2Teach_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        JogKeyUp(e)                     'V6.0.0.0⑪
    End Sub

    Public Sub JogKeyUp(ByVal e As KeyEventArgs) Implements ICommonMethods.JogKeyUp        'V6.0.0.0⑪
        'V6.0.0.0⑪        Dim KeyCode As Short = e.KeyCode
        Dim KeyCode As Keys = e.KeyCode             'V6.0.0.0⑪
        Console.WriteLine("frmTy2Teach.frmTy2Teach_KeyKeyUp()")
        ' テンキーアップならInpKeyのテンキーコードをOFFする
        'V6.0.0.0⑫        GrpTyTeach_2.Enabled = True                                     ' ###086
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

End Class