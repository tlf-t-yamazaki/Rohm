'===============================================================================
'   Description  : サーキットティーチング画面処理(NET用)
'
'   Copyright(C) : OMRON LASERFRONT INC. 2011
'
'===============================================================================
Option Strict Off
Option Explicit On

Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8①
Imports TKY_ALL_SL432HW.My.Resources    'V4.4.0.0-0
'Imports VB = Microsoft.VisualBasic

Friend Class frmCircuitTeach
    Inherits System.Windows.Forms.Form
    Implements ICommonMethods              'V6.0.0.0⑪

#Region "プライベート変数定義"
    '===========================================================================
    '   変数定義
    '===========================================================================
    '----- 変数定義 -----
    Private stJOG As JOG_PARAM                                          ' 矢印画面(BPのJOG操作)用パラメータ
    Private mExit_flg As Short                                          ' 終了フラグ
    Private Dspx As Double
    Private Dspy As Double                                              ' 座標表示用
    Private cpCirAxisInfoArray(MaxCntCircuit + 1) As CirAxisInfo        ' ｻｰｷｯﾄ座標

    '----- その他 -----
    Private dblTchMoval(3) As Double                                    ' ﾋﾟｯﾁ退避域(0=ﾋﾟｯﾁ, 1=HIGHﾋﾟｯﾁ, 2=Pause Time(Sec))
#End Region

#Region "終了結果を返す"
    ' '''=========================================================================
    ' '''<summary>終了結果を返す</summary>
    ' '''<returns>cFRS_ERR_START=OK(STARTｷｰ)
    ' '''         cFRS_ERR_RST=Cancel(RESETｷｰ)
    ' '''         -1以下      =エラー</returns>
    ' '''=========================================================================
    'Public ReadOnly Property sGetReturn() As Integer Implements ICommonMethods.sGetReturn  'V6.0.0.0⑪
    '    'V6.0.0.0⑪       'Public ReadOnly Property sGetReturn() As Short
    '    Get
    '        sGetReturn = mExit_flg
    '    End Get
    'End Property
#End Region

#Region "ﾌｫｰﾑ初期化時処理"
    '''=========================================================================
    '''<summary>ﾌｫｰﾑ初期化時処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub Form_Initialize_Renamed()
        MessageBox.Show(Me.GetType().Name & " では、FlexGrid（FGridCir）を" & Environment.NewLine & _
                        "DataGridView（dgvCir）に置き換えていますが" & Environment.NewLine & _
                        "実機でのデバッグをおこなっていません。")

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
    Private Sub frmCircuitTeach_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

    End Sub
#End Region

#Region "ﾌｫｰﾑﾛｰﾄﾞ時処理"
    '''=========================================================================
    '''<summary>ﾌｫｰﾑﾛｰﾄﾞ時処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub frmCircuitTeach_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        Dim i As Short

        ' 初期処理
        Call CirSetMessage()                                            ' ラベル等を設定する(日本語/英語)
        mExit_flg = -1                                                  ' 終了フラグ = 初期化

        ' ｳｲﾝﾄﾞｳをｺﾝﾄﾛｰﾙに重ねる
        Me.Top = Form1.Text4.Top + DISPOFF_SUBFORM_TOP
        Me.Left = Form1.Text4.Left

        lblCirInfo.Text = ""
        TxtPosX.Text = ""
        TxtPosY.Text = ""

        ' ｻｰｷｯﾄ座標ﾃﾞｰﾀのﾊﾞｯｸｱｯﾌﾟ
        For i = 1 To MaxCntCircuit
            cpCirAxisInfoArray(i) = typCirAxisInfoArray(i)
        Next i

        ' ﾌﾚｷｼﾌﾞﾙｸﾞﾘｯﾄﾞ初期化
        Call CirGridInitialize()

    End Sub
#End Region

#Region "ラベル等を設定する(日本語/英語)"
    '''=========================================================================
    '''<summary>メッセージ設定</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub CirSetMessage()

        'If gSysPrm.stTMN.giMsgTyp = 0 Then
        '    lblTitle.Text = "サーキットティーチング"
        '    GrpCir1.Text = "ティーチングポイント"
        '    cmdCancel.Text = "Cancel"
        '    cmdOK.Text = "OK"
        '    lblCirInfo.Text = ""

        'Else
        '    lblTitle.Text = "Circuit Standard Position Teaching"
        '    GrpCir1.Text = "Teaching Point"
        '    cmdCancel.Text = "Cancel"
        '    cmdOK.Text = "OK"
        '    lblCirInfo.Text = ""
        'End If
        lblCirInfo.Text = ""

        ' ラベル
        'LblCir1X.Text = LBL_Ex_Cam_01                                   '"Ｘ軸"
        'LblCir1Y.Text = LBL_Ex_Cam_02                                   '"Ｙ軸"

    End Sub
#End Region

#Region "Form Activated時処理"
#If False Then                          'V6.0.0.0⑬ Execute()でおこなう
    '''=========================================================================
    '''<summary>Form Activated時処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub frmCircuitTeach_Activated(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Activated

        Dim strMSG As String

        Try
            ' TY2ﾃｨｰﾁﾝｸﾞ開始
            If (mExit_flg <> -1) Then Exit Sub
            mExit_flg = 0                                               ' 終了フラグ = 0
            mExit_flg = CirMain()                                       ' ｻｰｷｯﾄﾃｨｰﾁﾝｸﾞ開始

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "frmCircuitTeach.frmCircuitTeach_Activated() TRAP ERROR = " + ex.Message
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
            ' TY2ﾃｨｰﾁﾝｸﾞ開始
            If (mExit_flg <> -1) Then Exit Function
            mExit_flg = 0                                               ' 終了フラグ = 0
            mExit_flg = CirMain()                                       ' ｻｰｷｯﾄﾃｨｰﾁﾝｸﾞ開始

            ' トラップエラー発生時
        Catch ex As Exception
            Dim strMSG As String = "frmCircuitTeach.Execute() TRAP ERROR = " & ex.Message
            MsgBox(strMSG)
            mExit_flg = cERR_TRAP                                       ' Return値 = 例外エラー
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

#Region "サーキットティーチング処理"
    '''=========================================================================
    '''<summary>ｻｰｷｯﾄﾃｨｰﾁﾝｸﾞﾒｲﾝ</summary>
    '''<returns>0=OK, 1=NG</returns>
    '''=========================================================================
    Private Function CirMain() As Short

        Dim Cdir As Short                                               ' ﾁｯﾌﾟ並び方向
        Dim CirNum As Short                                             ' 1ｸﾞﾙｰﾌﾟ内ｻｰｷｯﾄ数
        Dim Tofx As Double                                              ' ﾃｰﾌﾞﾙ位置ｵﾌｾｯﾄ
        Dim Tofy As Double
        Dim Bofx As Double                                              ' ﾋﾞｰﾑﾎﾟｼﾞｼｮﾅ位置ｵﾌｾｯﾄ
        Dim Bofy As Double
        Dim ADJX As Double                                              ' ｱｼﾞｬｽﾄﾎﾟｲﾝﾄ位置ｵﾌｾｯﾄ
        Dim ADJY As Double
        Dim BSX As Double                                               ' ﾌﾞﾛｯｸ ｻｲｽﾞ
        Dim BSY As Double

        Dim CNO As Short                                                ' ｻｰｷｯﾄ数ｶｳﾝﾀ
        Dim i As Short                                                  ' ｶｳﾝﾀｰ
        Dim rtn As Short                                                ' CirMain戻値 
        Dim r As Short
        Dim strMSG As String

        Try
            '-------------------------------------------------------------------
            '   初期処理
            '-------------------------------------------------------------------
            ' 各種ﾃﾞｰﾀ取得
            rtn = 0                                                     ' CirMain戻値 
            Cdir = typPlateInfo.intResistDir                            ' チップ並び方向取得(CHIP-NETのみ)
            CirNum = typPlateInfo.intCircuitCntInBlock                  ' 1ﾌﾞﾛｯｸ内ｻｰｷｯﾄ数
            ' ﾃｰﾌﾞﾙ位置ｵﾌｾｯﾄX,Y
            Tofx = typPlateInfo.dblTableOffsetXDir
            Tofy = typPlateInfo.dblTableOffsetYDir
            ' BP位置ｵﾌｾｯﾄX,Y
            Bofx = typPlateInfo.dblBpOffSetXDir
            Bofy = typPlateInfo.dblBpOffSetYDir
            ' ｱｼﾞｬｽﾄﾎﾟｲﾝﾄ位置ｵﾌｾｯﾄX,Y
            ADJX = typPlateInfo.dblAdjOffSetXDir
            ADJY = typPlateInfo.dblAdjOffSetYDir
            ' ﾌﾞﾛｯｸｻｲｽﾞX,Y
            Call CalcBlockSize(BSX, BSY)

            ' 先頭ﾌﾞﾛｯｸへXYﾃｰﾌﾞﾙ移動
            'r = XYTableMoveBlock(0, 1, 1, 1, 1)
            ' BPをオフセット位置に移動する
            Call BpMoveOrigin_Ex()
            r = Form1.System1.EX_MOVE(gSysPrm, ADJX, ADJY, 1)
            If (r < cFRS_NORMAL) Then
                Return (r)                                              ' エラーリターン
            End If

            ' 矢印画面(XYﾃｰﾌﾞﾙのJOG操作)用パラメータを初期化する
            stJOG.Md = MODE_BP                                          ' モード(1:BP移動)
            stJOG.Md2 = MD2_BUTN                                        ' 入力モード(0:画面ﾎﾞﾀﾝ入力, 1:ｺﾝｿｰﾙ入力)
            stJOG.Opt = CONSOLE_SW_RESET + CONSOLE_SW_START + CONSOLE_SW_HALT ' キーの有効(1)/無効(0)指定
            stJOG.PosX = 0.0                                            ' BP X位置/XYﾃｰﾌﾞﾙ X位置
            stJOG.PosY = 0.0                                            ' BP Y位置/XYﾃｰﾌﾞﾙ Y位置
            stJOG.BpOffX = ADJX + Bofx                                  ' BPｵﾌｾｯﾄX 
            stJOG.BpOffY = ADJY + Bofy                                  ' BPｵﾌｾｯﾄY 
            stJOG.BszX = BSX                                            ' ﾌﾞﾛｯｸｻｲｽﾞX 
            stJOG.BszY = BSY                                            ' ﾌﾞﾛｯｸｻｲｽﾞY
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
            Call Me.Focus()
            CNO = 1                                                     ' ｻｰｷｯﾄ数ｶｳﾝﾀ

STP_RETRY:
            '-------------------------------------------------------------------
            '   １ブロック内のサーキット座標のティーチング処理を行う
            '-------------------------------------------------------------------
            stJOG.Flg = -1                                              ' 親画面のOK/Cancelﾎﾞﾀﾝ押下ﾌﾗｸﾞ初期化
            stJOG.Md = MODE_BP                                          ' モード(1:BP移動)

            Do
                ' サーキット数分の処理完了？
                System.Windows.Forms.Application.DoEvents()
                If (CNO > CirNum) Then                                  ' サーキット数分ﾙｰﾌﾟ ?
                    Exit Do
                End If

                ' 第nｻｰｷｯﾄのBP座標取得
                With cpCirAxisInfoArray(CNO)
                    Dspx = .dblCaP2                                     ' ｻｰｷｯﾄ基準座標X
                    Dspy = .dblCaP3                                     ' ｻｰｷｯﾄ基準座標Y
                End With
                ' 第nｻｰｷｯﾄ座標XYにBP絶対値移動
                r = Form1.System1.EX_MOVE(gSysPrm, Dspx, Dspy, 1)
                If (r < cFRS_NORMAL) Then
                    Return (r)                                          ' エラーリターン
                End If

                ' ｸﾞﾘｯﾄﾞ表示(選択)
                Call FlxGridSet(CNO, 1, Dspx, 1)
                Call FlxGridSet(CNO, 2, Dspy, 1)

                ' ティーチング処理
                lblCirInfo.Text = CIRTEACH_MSG01                        ' "サーキット基準点ＸＹを合わせてから｢START｣キーを押して下さい"
                stJOG.PosX = Dspx                                       ' BP X位置
                stJOG.PosY = Dspy                                       ' BP Y位置
                'V6.0.0.0⑪                r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
                r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval, Me)  'V6.0.0.0⑪ 

                ' ｸﾞﾘｯﾄﾞ表示(非選択)
                Call FlxGridSet(CNO, 1, stJOG.PosX, 0)
                Call FlxGridSet(CNO, 2, stJOG.PosY, 0)

                ' ティーチング処理の戻値をチェックする
                If (r < cFRS_NORMAL) Then                               ' エラー ?
                    Return (r)                                          ' エラーリターン
                ElseIf (r = cFRS_ERR_RST) Then                          ' RESET SW ?
                    GoTo STP_END                                        ' 終了確認メッセージ表示へ
                ElseIf (r = cFRS_ERR_HALT) Then                         ' HALT SW ?
                    ' ｸﾞﾘｯﾄﾞ操作(非選択)
                    Call FlxGridSet(CNO, 1, cpCirAxisInfoArray(CNO).dblCaP2, 0)
                    Call FlxGridSet(CNO, 2, cpCirAxisInfoArray(CNO).dblCaP3, 0)
                    GoTo STP_HALT                                       ' １つ前のｻｰｷｯﾄに戻る

                ElseIf (r = cFRS_ERR_START) Then                        ' START SW ?
                    ' ｻｰｷｯﾄ基準座標更新
                    With cpCirAxisInfoArray(CNO)
                        .dblCaP2 = stJOG.PosX                           ' ｻｰｷｯﾄ基準座標X
                        .dblCaP3 = stJOG.PosY                           ' ｻｰｷｯﾄ基準座標Y
                    End With
                    CNO = CNO + 1                                       ' ｻｰｷｯﾄ数ｶｳﾝﾀ更新
                End If

            Loop While (stJOG.Flg = -1)

            ' 当画面からOK(START)/RESET(Cancel)ﾎﾞﾀﾝ押下ならrに戻値を設定する
            If (stJOG.Flg <> -1) Then
                r = stJOG.Flg
                GoTo STP_END                                            '終了確認メッセージ表示へ
            End If

            '-------------------------------------------------------------------
            '   STARTキー/RESET/HALTキー押下待ち
            '-------------------------------------------------------------------
            ' "[START]キーを押して下さい [HALT]キーで１つ前のデータに戻れます"
            lblCirInfo.Text = INFO_MSG10
            stJOG.Md = MODE_KEY                                         ' モード=キー入力待ちモード
            stJOG.Flg = -1
            Do
                ' STARTキー/RESET/HALTキー押下待ち
                'V6.0.0.0⑪                 r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
                r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval, Me)  'V6.0.0.0⑪
                If (r = cFRS_ERR_START) Then                            ' OK(STARTキー) ?
                    Exit Do
                ElseIf (r = cFRS_ERR_RST) Then                          ' Cancel(RESETｷｰ) ?
                    Exit Do
                ElseIf (r = cFRS_ERR_HALT) Then                         ' HALTｷｰ ?
                    ' ｸﾞﾘｯﾄﾞ操作(非選択)
                    Call FlxGridSet(CirNum, 1, cpCirAxisInfoArray(CirNum).dblCaP2, 0)
                    Call FlxGridSet(CirNum, 2, cpCirAxisInfoArray(CirNum).dblCaP3, 0)
                    Exit Do
                Else                                                    ' その他のエラー 
                    Return (r)
                End If
            Loop While (stJOG.Flg = -1)

            ' 当画面からOK(START)/RESET(Cancel)ﾎﾞﾀﾝ押下ならrに戻値を設定する
            If (stJOG.Flg <> -1) Then
                r = stJOG.Flg
                GoTo STP_END                                            '終了確認メッセージ表示へ
            End If

            '-------------------------------------------------------------------
            '   HALTｷｰ押下時は１つ前のｻｰｷｯﾄに戻る
            '-------------------------------------------------------------------
            If (r = cFRS_ERR_HALT) Then                                 ' HALTｷｰ ?
STP_HALT:
                CNO = CNO - 1                                           ' ｻｰｷｯﾄ数ｶｳﾝﾀ
                If CNO <= 0 Then CNO = 1
                ' ｸﾞﾘｯﾄﾞ操作(前へ)
                Call FlxGridSet(CNO, 1, cpCirAxisInfoArray(CNO).dblCaP2, 1)
                Call FlxGridSet(CNO, 2, cpCirAxisInfoArray(CNO).dblCaP3, 1)
                GoTo STP_RETRY
            End If

STP_END:
            '-------------------------------------------------------------------
            '   終了確認＆トリミングデータ更新
            '-------------------------------------------------------------------
            ' "前の画面に戻ります。よろしいですか？　
            rtn = Form1.System1.TrmMsgBox(gSysPrm, MSG_105, MsgBoxStyle.OkCancel, MSG_OPLOG_FUNC30)
            If (rtn = cFRS_ERR_RST) Then
                '「いいえ」選択なら１つ前のｻｰｷｯﾄに戻る
                ' ｸﾞﾘｯﾄﾞ操作(前へ)
                'If (CNO >= FGridCir.Rows - 1) Then CNO = FGridCir.Rows - 1
                If (dgvCir.CurrentRow.Index < CNO) Then CNO = dgvCir.CurrentRow.Index ' V4.0.0.0⑧
                Call FlxGridSet(CNO, 1, cpCirAxisInfoArray(CNO).dblCaP2, 0)
                Call FlxGridSet(CNO, 2, cpCirAxisInfoArray(CNO).dblCaP3, 0)
                CNO = CNO + 1
                GoTo STP_HALT
            Else
                ' OKﾎﾞﾀﾝ(STARTｷｰ)押下時で「はい」選択でならデータ更新して終了
                If (r = cFRS_ERR_START) Then                            ' OKﾎﾞﾀﾝ(STARTｷｰ)押下時 ?
                    ' ｻｰｷｯﾄ座標更新
                    For i = 1 To MaxCntCircuit
                        typCirAxisInfoArray(i) = cpCirAxisInfoArray(i)
                    Next i
                    gCmpTrimDataFlg = 1                                 ' データ更新フラグ = 1(更新あり)
                End If
            End If

            Return (r)                                                  ' Return値設定

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "frmCircuitTeach.CirMain() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return値 = 例外エラー
        End Try
    End Function
#End Region

#Region "ﾌﾚｷｼﾌﾞﾙｸﾞﾘｯﾄﾞ初期化"
    '''=========================================================================
    '''<summary>ﾌﾚｷｼﾌﾞﾙｸﾞﾘｯﾄﾞ初期化</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub CirGridInitialize()

        'Dim TOP_TITLE As String                             ' ﾀｲﾄﾙ
        'Dim TOP_TITLE_() As String                          ' ﾀｲﾄﾙ    V4.0.0.0⑧
        Dim i As Short                                      ' ｶｳﾝﾀ
        Dim CirNum As Short                                 ' ｻｰｷｯﾄ数
        Dim BNX As Short                                    ' ﾌﾞﾛｯｸ数X
        Dim BNY As Short                                    ' ﾌﾞﾛｯｸ数Y
        Dim Cdir As Short                                   ' ﾁｯﾌﾟ並び方向
        Dim num As Short
        Dim strMSG As String

        Try
            ' ﾀｲﾄﾙ
            'TOP_TITLE = ""
            'If gSysPrm.stTMN.giMsgTyp = 0 Then
            '    TOP_TITLE = " ｻｰｷｯﾄ番号 " & "|" & "基準座標 X" & "|" & "基準座標 Y"
            '    TOP_TITLE_ = New String() {" ｻｰｷｯﾄ番号 ", "基準座標 X", "基準座標 Y"}
            'ElseIf gSysPrm.stTMN.giMsgTyp = 1 Then
            '    TOP_TITLE = "CIRCUIT No." & "|" & "POSITION X" & "|" & "POSITION Y"
            '    TOP_TITLE_ = New String() {"CIRCUIT No.", "POSITION X", "POSITION Y"}
            'Else
            '    TOP_TITLE_ = New String() {String.Empty, String.Empty, String.Empty}
            'End If

            ' ﾃﾞｰﾀ取得
            CirNum = typPlateInfo.intCircuitCntInBlock          ' 1ﾌﾞﾛｯｸ内ｻｰｷｯﾄ数
            BNX = typPlateInfo.intGroupCntInBlockXBp            ' グループ数(ﾌﾞﾛｯｸ数)X,Y
            BNY = typPlateInfo.intGroupCntInBlockYStage
            Cdir = typPlateInfo.intResistDir                    ' チップ並び方向取得(CHIP-NETのみ)
            If Cdir = 0 Then
                num = CirNum * BNX
            ElseIf Cdir = 0 Then
                num = CirNum * BNY
            End If

            'With FGridCir
            '    .FormatString = TOP_TITLE                       ' ﾀｲﾄﾙ
            '    .Cols = 3                                       ' 最大列数
            '    .FixedCols = 1                                  ' 固定列数
            '    .Rows = CirNum + 1                              ' 最大行数
            '    .FixedRows = 1                                  ' 固定行数

            '    ' 文字列配置
            '    For i = 1 To CirNum
            '        .Row = i
            '        .Col = 1
            '        .CellAlignment = MSFlexGridLib.AlignmentSettings.flexAlignRightCenter
            '    Next

            'End With

            '' 初期ﾃﾞｰﾀを設定
            'For i = 1 To CirNum
            '    With typCirAxisInfoArray(i)
            '        'FGridCir.set_TextMatrix(i, 0, .intCaP1.ToString("      0")) ' 番号
            '        FGridCir.set_TextMatrix(i, 0, i.ToString("      0"))        ' 番号
            '        FGridCir.set_TextMatrix(i, 1, .dblCaP2.ToString("0.0000"))  ' 座標X
            '        FGridCir.set_TextMatrix(i, 2, .dblCaP3.ToString("0.0000"))  ' 座標Y
            '    End With
            'Next i

            With dgvCir                         ' V4.0.0.0⑧
                '.Columns(0).HeaderText = TOP_TITLE_(0)
                '.Columns(1).HeaderText = TOP_TITLE_(1)
                '.Columns(2).HeaderText = TOP_TITLE_(2)
                .Rows.Add(CirNum)
            End With

            ' 初期ﾃﾞｰﾀを設定
            For i = 0 To (CirNum - 1) Step 1    ' V4.0.0.0⑧
                With typCirAxisInfoArray(i + 1)
                    dgvCir(0, i).Value = (i + 1).ToString()          ' 番号
                    dgvCir(1, i).Value = .dblCaP2.ToString("0.0000") ' 座標X
                    dgvCir(2, i).Value = .dblCaP3.ToString("0.0000") ' 座標Y
                End With
            Next i

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "frmCircuitTeach.CirGridInitialize() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "ﾌﾚｷｼﾌﾞﾙｸﾞﾘｯﾄﾞへ表示"
    '''=========================================================================
    '''<summary>ﾌﾚｷｼﾌﾞﾙｸﾞﾘｯﾄﾞへ表示</summary>
    '''<param name="r">      (INP)行</param>
    '''<param name="c">      (INP)列</param>
    '''<param name="dblDATA">(INP)出力データ</param> 
    '''<param name="col">    (INP)</param>  
    '''=========================================================================
    Public Sub FlxGridSet(ByVal r As Integer, ByVal c As Integer, ByVal dblDATA As Double, ByVal col As Integer)

        'FGridCir.set_TextMatrix(r, c, dblDATA.ToString("0.0000"))
        dgvCir(c, r - 1).Value = dblDATA.ToString("0.0000")             ' V4.0.0.0⑧
        Call FlxGridColSet(r, c, col)

    End Sub
#End Region

#Region "ﾌﾚｷｼﾌﾞﾙｸﾞﾘｯﾄﾞﾊﾞｯｸｶﾗｰ変更"
    '''=========================================================================
    '''<summary>ﾌﾚｷｼﾌﾞﾙｸﾞﾘｯﾄﾞﾊﾞｯｸｶﾗｰ変更</summary>
    '''<param name="rr">  (INP)行</param>
    '''<param name="CC">  (INP)列</param>
    '''<param name="MODE">(INP) 対象のカット番号</param> 
    '''=========================================================================
    Public Sub FlxGridColSet(ByVal rr As Integer, ByVal CC As Integer, ByVal MODE As Integer)

        'With FGridCir
        '    .Col = CC                                   ' 列
        '    .Row = rr                                   ' 行
        '    If MODE = 0 Then                            ' Normal ?
        '        .CellBackColor = System.Drawing.Color.White
        '    Else
        '        '.CellBackColor = System.Drawing.Color.LightCyan
        '        .CellBackColor = System.Drawing.Color.Yellow
        '        If rr <= 5 Then
        '            .TopRow = 1
        '        Else
        '            .TopRow = rr - 5
        '        End If
        '    End If
        'End With

        With dgvCir(CC, rr - 1)         ' V4.0.0.0⑧
            If (0 = MODE) Then
                .Style.BackColor = SystemColors.Window
            Else
                .Style.BackColor = Color.Yellow
            End If
        End With

        If (rr < 5) Then                ' V4.0.0.0⑧
            dgvCir.FirstDisplayedScrollingRowIndex = 0
        Else
            dgvCir.FirstDisplayedScrollingRowIndex = (rr - 4)
        End If

    End Sub
#End Region

#Region "DataGridViewが選択状態の色になるのをキャンセルする"
    '''=========================================================================
    ''' <summary>DataGridViewが選択状態の色になるのをキャンセルする</summary>
    '''=========================================================================
    Private Sub dgvCir_SelectionChanged(sender As System.Object, e As System.EventArgs) Handles dgvCir.SelectionChanged
        dgvCir.Rows(dgvCir.CurrentRow.Index).Selected = False ' V4.0.0.0⑧
    End Sub
#End Region

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
    '   テンキー入力処理
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
        Dim KeyCode As Keys = e.KeyCode                     'V6.0.0.0⑪
        Console.WriteLine("frmCircuitTeach.frmTy2Teach_KeyDown()")

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
    Private Sub frmTy2Teach_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        JogKeyUp(e)                     'V6.0.0.0⑪
    End Sub

    Public Sub JogKeyUp(ByVal e As KeyEventArgs) Implements ICommonMethods.JogKeyUp        'V6.0.0.0⑪
        'V6.0.0.0⑪        Dim KeyCode As Short = e.KeyCode
        Dim KeyCode As Keys = e.KeyCode             'V6.0.0.0⑪
        Console.WriteLine("frmCircuitTeach.frmTy2Teach_KeyKeyUp()")
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

End Class