'===============================================================================
'   Description  : トリミング実行時一時停止処理
'
'   Copyright(C) : OMRON LASERFRONT INC. 2012
'
'===============================================================================
Option Strict Off
Option Explicit On

Imports LaserFront.Trimmer.DllSysPrm.SysParam
Imports LaserFront.Trimmer.DefTrimFnc
Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8①
Imports TKY_ALL_SL432HW.My.Resources    'V4.4.0.0-0

Friend Class frmFineAdjust
    Inherits System.Windows.Forms.Form
    Implements ICommonMethods              'V6.0.0.0⑪

    '========================================================================================
    '   定数・変数定義
    '========================================================================================
#Region "定数・変数定義"
    '===========================================================================
    '   定数定義
    '===========================================================================
    Public Const MOVE_NEXT As Integer = 0
    Public Const MOVE_NOT As Integer = 1

    '----- 処理モード -----
    Private Const MD_INI As Integer = 0                                 ' 初期エントリモード
    Private Const MD_CHK As Integer = 1                                 ' 継続エントリモード

    '===========================================================================
    '   メンバ変数定義
    '===========================================================================
    Private m_BlockSizeX As Double
    Private m_BlockSizeY As Double
    Private m_bpOffX As Double
    Private m_bpOffY As Double
    Private m_sysPrm As SYSPARAM_PARAM
    Private stJOG As JOG_PARAM                                          ' 矢印画面(BPのJOG操作)用パラメータ '###046(Globals.vbの共通関数を使用)
    Private dblTchMoval(3) As Double                                    ' ﾋﾟｯﾁ退避域(0=ﾋﾟｯﾁ, 1=HIGHﾋﾟｯﾁ, 2=Pause Time(Sec))
    Private mExit_flg As Short                                          ' 結果
    Private mMd As Integer = MD_INI                                     ' 処理モード
    Private m_TenKeyFlg As Boolean = False                              ' ###139
    Private m_LaserOnOffFlag As Boolean = False                         ' ###237

    Private _procBlockSelected As Boolean                               'V4.12.2.0③
#End Region

    '========================================================================================
    '   メソッド定義
    '========================================================================================
#Region "初期値設定処理"
    '''=========================================================================
    ''' <summary>初期値設定処理</summary>
    ''' <param name="SysPrm"></param>
    ''' <param name="digL"></param>
    ''' <param name="digH"></param>
    ''' <param name="curPltNo"></param>
    ''' <param name="curBlkNo"></param>
    '''=========================================================================
    Public Sub SetInitialData(ByRef SysPrm As SYSPARAM_PARAM,
                              ByVal digL As Integer, ByVal digH As Integer,
                              ByVal curPltNo As Integer, ByVal curBlkNo As Integer,
                              ByVal curPltNoX As Integer, ByVal curPltNoY As Integer,
                              ByVal curBlkNoX As Integer, ByVal curBlkNoY As Integer)   '#4.12.2.0⑥
        '#4.12.2.0⑥    Public Sub SetInitialData(ByRef SysPrm As SYSPARAM_PARAM,
        '#4.12.2.0⑥                        ByVal digL As Integer, ByVal digH As Integer,
        '#4.12.2.0⑥                        ByRef curPltNo As Integer, ByRef curBlkNo As Integer)

        Try
            CbDigSwH.SelectedIndex = digH
            CbDigSwL.SelectedIndex = digL
            gCurBlockNo = curBlkNo
            gCurPlateNo = curPltNo
            '#4.12.2.0⑥                 ↓
            Globals_Renamed.gCurPlateNoX = curPltNoX
            Globals_Renamed.gCurPlateNoY = curPltNoY
            Globals_Renamed.gCurBlockNoX = curBlkNoX
            Globals_Renamed.gCurBlockNoY = curBlkNoY
            '#4.12.2.0⑥                 ↑
            m_sysPrm = SysPrm
            'gFrmEndStatus = cFRS_NORMAL

            If (gbChkboxHalt = True) Then                                       '###009
                BtnADJ.Text = "ADJ ON"                                          '###009
                BtnADJ.BackColor = System.Drawing.Color.Yellow                  '###009
            Else                                                                '###009
                BtnADJ.Text = "ADJ OFF"                                         '###009
                BtnADJ.BackColor = System.Drawing.SystemColors.Control          '###009
            End If                                                              '###009

            ' ラベル名設定(日本語/英語)
            'BtnEdit.Text = LBL_FINEADJ_001                                      ' "データ編集" ###014
            '-----###204 -----
            'Me.Label3.Text = LBL_FINEADJ_002                                    ' "調整" 
            'CbDigSwH.Items(0) = LBL_FINEADJ_003
            'CbDigSwH.Items(1) = LBL_FINEADJ_004
            'CbDigSwH.Items(2) = LBL_FINEADJ_005
            '-----###204 -----
            '----- ###268↓ -----
            '「Ten Key On/Off」ボタンの初期値をシスパラより設定する
            If (giTenKey_Btn = 0) Then                                          ' 一時停止画面での「Ten Key On/Off」ボタンの初期値(0:ON(既定値), 1:OFF)
                gbTenKeyFlg = True
                BtnTenKey.Text = "Ten Key On"
                BtnTenKey.BackColor = System.Drawing.Color.Pink
                'V6.0.1.0⑪                Form1.SetTrimMapVisible(False)                          'V6.0.1.0⑩
            Else
                gbTenKeyFlg = False
                BtnTenKey.Text = "Ten Key Off"
                BtnTenKey.BackColor = System.Drawing.SystemColors.Control
            End If
            'V6.0.1.0⑪            Form1.SetMapOnOffButtonEnabled(Not gbTenKeyFlg)             'V4.12.2.0①

            'gbTenKeyFlg = True                                                 ' 「Ten Key On」状態 ###242
            '----- ###268↑ -----
            '----- ###269↓ -----
            ' 一時停止画面でのシスパラ「BPオフセット調整する/しない」指定により矢印ボタン等を設定する
            Call Sub_SetBtnArrowEnable()
            '----- ###269↑ -----

            '----- V6.0.3.0_50↓ -----
            stJOG.TenKey = New Button() {BtnJOG_0, BtnJOG_1, BtnJOG_2, BtnJOG_3,
                                         BtnJOG_4, BtnJOG_5, BtnJOG_6, BtnJOG_7, BtnHI}
            '----- V6.0.3.0_50↑ -----

            'for　抵抗数分
            '目標値
            'カットオフ
            'スピード
            '加工条件番号
            'next
            'txtExCamPosX.Text = m_sysPrm.stDEV.gfExCmX.ToString
#If False Then  'V6.0.1.3①
            _procBlockSelected = (0 < Globals_Renamed.GetProcBlockCount())  'V4.12.2.0③
            Form1.SetMapBorder(gCurBlockNo, Color.Black)                    'V4.12.2.0①
#Else
            ' プレート対応    'V6.0.1.3①
            'If (Form1.ChkMapEnable()) Then
            _procBlockSelected = Form1.TrimMapSelected
                If (_procBlockSelected) Then
                    Form1.GetNextSelectBlock(gCurPlateNo, gCurBlockNo, gCurPlateNo, gCurBlockNo)
                End If
                Form1.SetMapBorder(gCurPlateNo, gCurBlockNo, Color.Black)

            'End If
#End If
        Catch ex As Exception
            Dim strMsg As String
            strMsg = "frmFineAdjust.Form_Initialize_Renamed() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try

    End Sub
#End Region
    '-----###269↓-----
#Region "矢印ボタンを活性化/非活性化する"
    '''=========================================================================
    ''' <summary>矢印ボタンを活性化/非活性化する</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub Sub_SetBtnArrowEnable()

        Dim bFlg As Boolean
        Dim strMsg As String

        Try
            '  一時停止画面でのシスパラ「BPオフセット調整する/しない」指定により矢印ボタン等を設定する
            If (giBpAdj_HALT = 0) Then                                          ' BPオフセット調整する ?
                bFlg = True

                Form1.Instance.SetActiveJogMethod(AddressOf Me.JogKeyDown,
                                                  AddressOf Me.JogKeyUp,
                                                  AddressOf Me.MoveToCenter)    'V6.0.0.0⑩
            Else                                                                ' BPオフセット調整しない
                bFlg = False
                gbTenKeyFlg = False
                BtnTenKey.Enabled = False                                       '「Ten Key Off」ボタン非活性化
                BtnTenKey.Text = "Ten Key Off"
                BtnTenKey.BackColor = System.Drawing.SystemColors.Control

                Form1.Instance.SetActiveJogMethod(Nothing, Nothing, Nothing)    'V6.0.0.0⑩
            End If

            ' 矢印ボタン活性化/非活性化
            BtnJOG_0.Enabled = bFlg
            BtnJOG_1.Enabled = bFlg
            BtnJOG_2.Enabled = bFlg
            BtnJOG_3.Enabled = bFlg
            BtnJOG_4.Enabled = bFlg
            BtnJOG_5.Enabled = bFlg
            BtnJOG_6.Enabled = bFlg
            BtnJOG_7.Enabled = bFlg
            BtnHI.Enabled = bFlg

            ' Moving Pitch活性化/非活性化
            GrpPithPanel.Enabled = bFlg

        Catch ex As Exception
            strMsg = "frmFineAdjust.Sub_SetBtnArrowEnable() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Sub
#End Region
    '----- ###269↑-----
    '----- ###260↓-----
#Region "タイマー停止"
    '''=========================================================================
    ''' <summary>タイマー停止</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Function Sub_StopTimer() As Integer

        TmKeyCheck.Enabled = False

    End Function
#End Region
    '----- ###260↑-----

#Region "タイマー開始"
    '''=========================================================================
    ''' <summary>タイマー開始</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Function Sub_StartTimer() As Integer

        TmKeyCheck.Enabled = True

    End Function
#End Region

#Region "ステージポジション取得処理"
    '''=========================================================================
    ''' <summary>ステージポジション取得処理（実行後に取得）</summary>
    '''=========================================================================
    Public Sub GetStagePosInfo(ByRef pltNo As Integer, ByRef blkNo As Integer,
                               ByRef pltNoX As Integer, ByRef pltNoY As Integer,
                               ByRef blkNoX As Integer, ByRef blkNoY As Integer)    '#4.12.2.0⑥
        '#4.12.2.0⑥    Public Sub GetStagePosInfo(ByRef pltNo As Integer, ByRef blkNo As Integer)
        Try
            pltNo = gCurPlateNo
            blkNo = gCurBlockNo
            '#4.12.2.0⑥                 ↓
            pltNoX = Globals_Renamed.gCurPlateNoX
            pltNoY = Globals_Renamed.gCurPlateNoY
            blkNoX = Globals_Renamed.gCurBlockNoX
            blkNoY = Globals_Renamed.gCurBlockNoY
            '#4.12.2.0⑥                 ↑
        Catch ex As Exception
            Dim strMsg As String
            strMsg = "frmFineAdjust.GetStagePosInfo() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Sub
#End Region

#Region "終了戻り値取得処理"
    '''=========================================================================
    ''' <summary>終了戻り値取得処理（実行後に取得）</summary>
    '''=========================================================================
    Public Function GetReturnVal() As Integer
        Try
            Return (mExit_flg)

        Catch ex As Exception
            Dim strMsg As String
            strMsg = "frmFineAdjust.GetReturnVal() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Function
#End Region

    '========================================================================================
    '   画面処理
    '========================================================================================
#Region "フォームロード処理"

    'V6.0.0.0⑪    Private Sub frmFineAdjust_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
    'V6.0.0.0⑪Dim r As Integer                                                ' ###237
    'V6.0.0.0⑪        r = r

    'V6.0.0.0⑪    End Sub

    '''=========================================================================
    ''' <summary>フォームロード処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub frmFineAdjust_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim stPos As System.Drawing.Point
        Dim stGetPos As System.Drawing.Point
        Dim r As Integer                                                ' ###237
        Dim strMsg As String
        Dim stSize As System.Drawing.Size

        Try
            ' 表示位置の調整
            stPos = Form1.Text4.PointToScreen(stGetPos)
            stPos.X = stPos.X - 2
            stPos.Y = stPos.Y - 2
            Me.Location = stPos

            ' BpOffsetの現在値設定
            GetBpOffset(m_bpOffX, m_bpOffY)
            txtBpOffX.Text = m_bpOffX.ToString
            txtBpOffY.Text = m_bpOffY.ToString

            ' BlockSizeの現在値取得
            GetBlockSize(m_BlockSizeX, m_BlockSizeY)

            '----- ###139↓ -----
            'V6.0.2.0②↓
            '' メイン画面の「生産グラフ表示/非表示ボタン」から当画面の「生産グラフ表示/非表示ボタン」を設定する
            'If (gTkyKnd = KND_CHIP Or gTkyKnd = KND_NET) Then
            '    chkDistributeOnOff.Text = Form1.chkDistributeOnOff.Text
            '    chkDistributeOnOff.Checked = Form1.chkDistributeOnOff.Checked
            '    'V4.0.0.0⑫
            '    If gKeiTyp <> KEY_TYPE_RS Then
            '        GrpDistribute.Visible = True                        '「生産グラフボタン」表示
            '    Else
            '        GrpDistribute.Visible = False                        '「生産グラフボタン」表示
            '    End If
            '    'V4.0.0.0⑫
            'Else
            '    GrpDistribute.Visible = False                       '「生産グラフボタン」非表示
            'End If
            'V6.0.2.0②↑
            '----- ###139↑ -----

            '----- ###237↓ -----
            ' 加工条件番号を設定する(FL時)
            If (gSysPrm.stRAT.giOsc_Res = OSCILLATOR_FL) Then
                Call QRATE(stCND.Freq(ADJ_CND_NUM))                     ' Qレート設定(KHz)
                r = FLSET(FLMD_CNDSET, ADJ_CND_NUM)                     ' 加工条件番号設定(一時停止画面用)
            Else
                Call QRATE(gSysPrm.stDEV.gfLaserQrate)                  ' Qレート設定(KHz) ※レーザ調整用Qレートを設定
            End If
            '----- ###237↑ -----

            '----- V1.23.0.0⑥↓ -----
            ' トリミングデータで「基板ディレイ」指定時は
            ' 「Block Move」「データ編集」「MoveMode」は無効とする
            If (typPlateInfo.intDelayTrim = 2) Then                     ' 基板ディレイ(ディレイトリム２) ?
                BlockMove.Enabled = False
                BtnEdit.Enabled = False
                CbDigSwL.Enabled = False
            Else
                BlockMove.Enabled = True
                BtnEdit.Enabled = True
                CbDigSwL.Enabled = True
            End If
            '----- V1.23.0.0⑥↑ -----

            'V4.0.0.0-84↓
            If gSysPrm.stTMN.giKeiTyp = KEY_TYPE_RS Then
                btnClose.Visible = True
                BtnSTART.Visible = True
                stSize.Height = Me.Height + 100
                stSize.Width = Me.Width
                Me.Size = stSize
            Else
                btnClose.Visible = False
                BtnSTART.Visible = False
            End If
            'V4.0.0.0-84↑

            'V5.0.0.9⑯                  ↓
            If (1 < giStartBlkAss) Then
                Dim x, y As Integer : Form1.Get_StartBlkNum(x, y)

                ' トリミング開始ブロック番号Xコンボボックスの設定
                With Me.cmbBlockMoveX
                    .BeginUpdate()
                    .Items.Clear()
                    For Idx As Integer = 1 To typPlateInfo.intBlockCntXDir
                        .Items.Add(Idx.ToString(0).PadLeft(3))
                    Next Idx
                    .SelectedIndex = (x - 1)
                    .EndUpdate()
                End With

                ' トリミング開始ブロック番号Yコンボボックスの設定
                With Me.cmbBlockMoveY
                    .BeginUpdate()
                    .Items.Clear()
                    For Idx As Integer = 1 To typPlateInfo.intBlockCntYDir
                        .Items.Add(Idx.ToString(0).PadLeft(3))
                    Next Idx
                    .SelectedIndex = (y - 1)
                    .EndUpdate()
                End With
            Else
                Me.lblBlockMoveX.Visible = False
                Me.cmbBlockMoveX.Enabled = False
                Me.cmbBlockMoveX.Visible = False

                Me.lblBlockMoveY.Visible = False
                Me.cmbBlockMoveY.Enabled = False
                Me.cmbBlockMoveY.Visible = False
            End If
            'V5.0.0.9⑯                  ↑

            ' フォーカスの設定(これによってテンキーのイベントが取得できる)
            Me.KeyPreview = True
            Me.Activate()                                               ' ###046

        Catch ex As Exception
            strMsg = "frmFineAdjust.frmFineAdjust_Load() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Sub
#End Region

#Region "フォームが表示された時の処理"
    '''=========================================================================
    ''' <summary>フォームが表示された時の処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub frmFineAdjust_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown

        Dim r As Integer = cFRS_NORMAL
        Dim strMSG As String

        Try
            '----- V6.0.3.0_50↓ -----
            'V6.0.0.0⑫                  ↓
            ' Form.Load() は Show()時とShowDialog()時とで一度・都度と発生状況が異なるため、呼ばれ方に影響を受けないShown()でおこなう
            'stJOG.TenKey = New Button() {BtnJOG_0, BtnJOG_1, BtnJOG_2, BtnJOG_3,
            '                             BtnJOG_4, BtnJOG_5, BtnJOG_6, BtnJOG_7, BtnHI}
            'V6.0.0.0⑫                  ↑
            '----- V6.0.3.0_50↑ -----

            'V6.0.2.0②↓
            ' メイン画面の「生産グラフ表示/非表示ボタン」から当画面の「生産グラフ表示/非表示ボタン」を設定する
            If (gTkyKnd = KND_CHIP Or gTkyKnd = KND_NET) Then
                chkDistributeOnOff.Text = Form1.chkDistributeOnOff.Text
                chkDistributeOnOff.Checked = Form1.chkDistributeOnOff.Checked
                'V4.0.0.0⑫
                If gKeiTyp <> KEY_TYPE_RS Then
                    GrpDistribute.Visible = True                        '「生産グラフボタン」表示
                Else
                    GrpDistribute.Visible = False                        '「生産グラフボタン」表示
                End If
                'V4.0.0.0⑫
            Else
                GrpDistribute.Visible = False                       '「生産グラフボタン」非表示
            End If
            'V6.0.2.0②↑

            ' 一時停止画面処理メインをCallする
            mExit_flg = 0                                               ' 終了フラグ = 0
            Call ZCONRST()                                              ' ｺﾝｿｰﾙｷｰﾗｯﾁ解除
            TmKeyCheck.Interval = 10
            TmKeyCheck.Enabled = True                                   ' タイマー開始
            lblBlockNo.Text = "BlockNo=" + gCurBlockNo.ToString("0")    'V4.9.0.0⑤
            gfrmAdjustDisp = 1                                           'V5.0.0.1-29
            Return

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "frmFineAdjust.frmFineAdjust_Shown() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            mExit_flg = cERR_TRAP                                       ' Return値 = 例外エラー
        End Try

        gbExitFlg = True                                                ' 終了フラグON
        Call LASEROFF()                                                 ' ###237
        gfrmAdjustDisp = 0                                           'V5.0.0.1-29
        Me.Close()
    End Sub
#End Region

#Region "メイン処理実行"
    ''' <summary>メイン処理実行</summary>
    ''' <returns>実行結果</returns>
    ''' <remarks>'V6.0.0.0⑬</remarks>
    Public Function Execute() As Integer Implements ICommonMethods.Execute
        ' DO NOTHING
    End Function
#End Region

#Region "キー入力チェックタイマー処理"
    '''=========================================================================
    ''' <summary>キー入力チェックタイマー処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub TmKeyCheck_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles TmKeyCheck.Tick

        Dim r As Short
        Dim strMSG As String

        Try
            ' キー入力チェック処理
            TmKeyCheck.Enabled = False                                  ' タイマー停止
            ' フォームが表示されていないときには、タイマーは何もしない(画面はないのにタイマーが消えていないことがある)
            If (IsNothing(gObjADJ) = False) Then
                If (gObjADJ.Visible = False) Then
                    Return
                End If
            End If

            r = MainProc(mMd)                                           ' 一時停止画面処理
            If (r = cFRS_NORMAL) Then                                   ' 正常戻り 
                TmKeyCheck.Enabled = True                               ' タイマー開始
                Return
            End If

            '----- ###219↓ -----
            ' Z キー押下なら Z On/OFFする 
            If (r = cFRS_ERR_Z) Then                                    ' Z SW押下 ?
                If (stJOG.bZ = True) Then                               ' Z ON ? 
                    r = PROBON()
                    r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)   ' エラーならメッセージを表示する
                    If (r <> cFRS_NORMAL) Then
                        mExit_flg = r                                   ' エラーリターン 
                        Return
                    End If
                    '----- V1.13.0.0④↓ -----
                    ' Z2軸を移動する。
                    If IsUnderProbe() Then
                        r = Z2move(Z2ON)                                ' Z2軸動作指令
                        If (r <> cFRS_NORMAL) Then
                            mExit_flg = r                               ' エラーリターン 
                            Return
                        End If
                        Call ZSTOPSTS2()                                ' Z2軸動作停止待ち
                    End If
                    '----- V1.13.0.0④↑ -----

                Else                                                    ' Z OFF
                    '----- V1.13.0.0④↓ -----
                    ' Z2を待機位置に移動
                    If IsUnderProbe() Then                              ' 下方プローブ有りの場合
                        r = Z2move(Z2OFF)                               ' Z2をDOWN位置に移動
                        If (r <> cFRS_NORMAL) Then                      ' エラーならエラーリターン(メッセージ表示済み)
                            mExit_flg = r                               ' エラーリターン 
                            Return
                        End If
                        Call ZSTOPSTS2()
                    End If
                    '----- V1.13.0.0④↑ -----
                    r = PROBOFF_EX(typPlateInfo.dblZWaitOffset)         ' EX_STARTのZOFF位置をzWaitPosとする
                    ' エラーならメッセージを表示してエラーリターン
                    r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)   ' エラーならメッセージを表示する
                    If (r <> cFRS_NORMAL) Then
                        mExit_flg = r                                   ' エラーリターン 
                        Return
                    End If
                End If

                ' Zランプの点灯/消灯
                If (stJOG.bZ = True) Then
                    Call LAMP_CTRL(LAMP_Z, True)
                Else
                    Call LAMP_CTRL(LAMP_Z, False)
                End If

                TmKeyCheck.Enabled = True                               ' タイマー開始
                Return
            End If
            '----- ###219↑ -----

            ' START/RESETキー押下またはエラーなら終了
            If (r = cFRS_ERR_START) Then r = cFRS_NORMAL

            mExit_flg = r

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "frmFineAdjust.TmKeyCheck_Tick() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            mExit_flg = cERR_TRAP                                       ' Return値 = 例外エラー
        End Try

        gbExitFlg = True                                                ' 終了フラグON
        Call LASEROFF()                                                 ' ###237

        'V5.0.0.1①↓
        If gSysPrm.stTMN.giKeiTyp = KEY_TYPE_RS Then
            'V5.0.0.9⑯            If (giStartBlkAss = 1) Then
            If (giStartBlkAss <> 0) Then    'V5.0.0.9⑯
                Call Form1.Set_StartBlkNum(gCurBlockNo, 1)
            End If
        End If
        'V5.0.0.1①↑

        gfrmAdjustDisp = 0                                           'V5.0.0.1-29
        Me.Close()
    End Sub
#End Region

#Region "メイン処理"
    '''=========================================================================
    ''' <summary>メイン処理"</summary>
    ''' <param name="Md">(I/O)処理モード
    ''' 　　　　　　　　　　　MD_INI=初期エントリ, MD_CHK=継続エントリ</param>
    ''' <returns>cFRS_NORMAL   = OK(STARTｷｰ)
    '''          cFRS_ERR_RST  = Cancel(RESETｷｰ)
    '''          -1以下        = エラー</returns>
    '''=========================================================================
    Private Function MainProc(ByRef Md As Integer) As Short

        Dim mdAdjx As Double = 0.0                                      ' ｱｼﾞｬｽﾄ位置X(未使用)
        Dim mdAdjy As Double = 0.0                                      ' ｱｼﾞｬｽﾄ位置Y(未使用)
        Dim r As Short
        Dim strMSG As String

        Try
            '-------------------------------------------------------------------
            '   初期処理
            '-------------------------------------------------------------------
            If (Md = MD_INI) Then                                       ' 初期エントリ
                ' JOGパラメータ設定 
                stJOG.Md = MODE_BP                                      ' モード(1:BP移動)
                stJOG.Md2 = MD2_BUTN                                    ' 入力モード(0:画面ﾎﾞﾀﾝ入力, 1:ｺﾝｿｰﾙ入力)
                '                                                       ' キーの有効(1)/無効(0)指定
                'stJOG.Opt = CONSOLE_SW_RESET + CONSOLE_SW_START
                stJOG.Opt = CONSOLE_SW_RESET + CONSOLE_SW_START + CONSOLE_SW_ZSW ' ###219
                stJOG.PosX = 0.0                                        ' BP X位置(BPｵﾌｾｯﾄX)
                stJOG.PosY = 0.0                                        ' BP Y位置(BPｵﾌｾｯﾄY)
                stJOG.BpOffX = mdAdjx + m_bpOffX                        ' BPｵﾌｾｯﾄX 
                stJOG.BpOffY = mdAdjy + m_bpOffY                        ' BPｵﾌｾｯﾄY 
                stJOG.BszX = m_BlockSizeX                               ' ﾌﾞﾛｯｸｻｲｽﾞX 
                stJOG.BszY = m_BlockSizeY                               ' ﾌﾞﾛｯｸｻｲｽﾞY
                txtBpOffX.ShortcutsEnabled = False                      ' ###047 右クリックメニューを表示しない 
                txtBpOffY.ShortcutsEnabled = False                      '  
                stJOG.TextX = txtBpOffX                                 ' BP X位置表示用ﾃｷｽﾄﾎﾞｯｸｽ
                stJOG.TextY = txtBpOffY                                 ' BP Y位置表示用ﾃｷｽﾄﾎﾞｯｸｽ
                stJOG.cgX = m_bpOffX                                    ' 移動量X (BPｵﾌｾｯﾄX)
                stJOG.cgY = m_bpOffY                                    ' 移動量Y (BPｵﾌｾｯﾄY)
                stJOG.BtnHI = BtnHI                                     ' HIボタン
                stJOG.BtnZ = BtnZ                                       ' Zボタン
                stJOG.BtnSTART = BtnSTART                               ' STARTボタン
                stJOG.BtnRESET = BtnRESET                               ' RESETボタン
                stJOG.BtnHALT = BtnHALT                                 ' HALTボタン
                Call JogEzInit(stJOG, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
                stJOG.Flg = -1                                          ' 親画面のOK/Cancelﾎﾞﾀﾝ押下ﾌﾗｸﾞ
                Md = MD_CHK
                stJOG.bZ = False                                        ' JogのZキー状態 = Z Off ###219
                Call LAMP_CTRL(LAMP_Z, False)                           ' ###219 
            End If

STP_RETRY:
            'Call Me.Focus()                                            ' ← これをやるとテンキーのKeyUp/KeyDownイベントが入ってこなくなる

            ' 非常停止等チェック
            r = Form1.System1.Sys_Err_Chk_EX(gSysPrm, giAppMode)
            If (r <> cFRS_NORMAL) Then                                  ' 非常停止等検出 ?
                Return (r)
            End If

            '----- ###209↓ -----
            ' カバー閉を確認する(SL436R時で手動モード時)
            If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) And (bFgAutoMode = False) Then
                Call COVERLATCH_CLEAR()                                 ' カバー開ラッチのクリア
                r = FrmReset.Sub_CoverCheck()
                If (r < cFRS_NORMAL) Then                               ' 非常停止等検出 ?
                    Return (r)
                End If
            ElseIf gKeiTyp = KEY_TYPE_RS Then   'SL436Sの場合はBP調整画面が表示されていたらチェックする
                'Call COVERLATCH_CLEAR()                                 ' カバー開ラッチのクリア
                'r = FrmReset.Sub_CoverCheck()
                'If (r < cFRS_NORMAL) Then                               ' 非常停止等検出 ?
                '    Return (r)
                'End If
            End If
            '----- ###209↑ -----

            ' コンソールキー等の入力待ち
            'stJOG.Flg = -1                                             ' 親画面のOK/Cancelﾎﾞﾀﾝ押下ﾌﾗｸﾞ
            r = JogEzMove_Ex(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
            If (r < cFRS_NORMAL) Then                                   ' エラーなら終了
                Return (r)
            End If

            ' コンソールキーチェック
            If (r = cFRS_ERR_START) Then                                ' START SW押下 ?
                ' DIG-SW設定
                Call Form1.SetMoveMode(CbDigSwL.SelectedIndex, CbDigSwH.SelectedIndex)
                ' BPオフセット更新(タイミングによって空白で入ってくる場合トラップエラーとなるのでチェックする ###014)
                If (txtBpOffX.Text <> "") And (txtBpOffY.Text <> "") Then
                    Call SetBpOffset(Double.Parse(txtBpOffX.Text), Double.Parse(txtBpOffY.Text))
                End If
                Return (cFRS_ERR_START)

            ElseIf (r = cFRS_ERR_RST) Then                              ' RESET SW押下 ?
                Return (cFRS_ERR_RST)

                '----- ###219↓ -----
            ElseIf (r = cFRS_ERR_Z) Then                                ' Z SW押下 ?
                Return (cFRS_ERR_Z)
                '----- ###219↑ -----
            End If

            'Loop While (stJOG.Flg = -1)

            '' 当画面からOK/Cancelﾎﾞﾀﾝ押下ならrに戻値を設定する
            'If (stJOG.Flg <> -1) Then
            '    r = stJOG.Flg
            'End If

STP_END:
            Return (r)                                                  ' Return値設定

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "frmFineAdjust.MainProc() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return値 = 例外エラー
        End Try
    End Function
#End Region

    '========================================================================================
    '   メイン画面のボタン押下時処理
    '========================================================================================
#Region "ADJﾎﾞﾀﾝ押下時処理"
    '''=========================================================================
    ''' <summary>ADJﾎﾞﾀﾝ押下時処理 ###009</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub BtnADJ_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnADJ.Click
        Dim strMSG As String

        Try
            If (BtnADJ.Text = "ADJ OFF") Then
                gbChkboxHalt = True
                BtnADJ.Text = "ADJ ON"
                BtnADJ.BackColor = System.Drawing.Color.Yellow
            Else
                gbChkboxHalt = False
                BtnADJ.Text = "ADJ OFF"
                BtnADJ.BackColor = System.Drawing.SystemColors.Control
            End If

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FrmFineAdjust.BtnADJ_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "前ブロック移動処理(Prevボタン押下時)"
    '''=========================================================================
    ''' <summary>前ブロック移動処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub btnBlkPrvMove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBlkPrvMove.Click

        Dim intRet As Integer
        Dim workBlockNo As Integer
        Dim workPlateNo As Integer

        Try
            ' 先頭プレート、先頭ブロックであれば移動しない
            TmKeyCheck.Enabled = False                                  ' タイマー停止
            If (gCurPlateNo <= 1) And (gCurBlockNo <= 1) Then
                GoTo STP_END
            ElseIf (gCurBlockNo <= 1) Then
                ' 先頭ブロックであれば一つ前のプレートの最終ブロックへ
                workPlateNo = gCurPlateNo - 1
                workBlockNo = GetBlockCnt()
            Else
                ' ブロックを1つ前に移動する
                workBlockNo = gCurBlockNo - 1
                workPlateNo = gCurPlateNo
            End If

            'V4.12.2.0③             ↓
            If (_procBlockSelected) Then
#If Not True Then   'V6.0.1.3①
                workBlockNo = Globals_Renamed.GetPrevProcBlock(workBlockNo)
                If (workBlockNo < 1) Then
                    If (1 < gCurPlateNo) Then
                        ' 先頭選択ブロックであれば一つ前のプレートの最終選択ブロックへ
                        workPlateNo = gCurPlateNo - 1
                        workBlockNo = Globals_Renamed.GetPrevProcBlock(GetBlockCnt())
                    Else
                        ' 先頭プレート、先頭選択ブロックであれば移動しない
                        GoTo STP_END
                    End If
                End If
#Else
                ' プレート対応    'V6.0.1.3①
                Dim hasPrev As Boolean = Form1.GetPrevSelectBlock(
                    workPlateNo, workBlockNo, workPlateNo, workBlockNo)

                If (False = hasPrev) Then
                    ' 先頭プレート、先頭選択ブロックであれば移動しない
                    GoTo STP_END
                End If
#End If
            End If
            'V4.12.2.0③             ↑

            ' ステージ移動
            intRet = MoveTargetStagePos(workPlateNo, workBlockNo)
            If (intRet = MOVE_NEXT) Then
                ' 移動後のプレート番号、ブロック番号を保存する
                gCurBlockNo = workBlockNo
                gCurPlateNo = workPlateNo
                lblBlockNo.Text = "BlockNo=" + gCurBlockNo.ToString("0")    'V4.9.0.0⑤
                'V4.1.0.0⑱
                If gKeiTyp = KEY_TYPE_RS Then
                    SetNowBlockDspNum(gCurBlockNo)
                End If
                'V4.1.0.0⑱

                'V5.0.0.9⑯              ↓
                Dim x, y As Integer : Form1.Get_StartBlkNum(x, y)
                Me.cmbBlockMoveX.SelectedIndex = (x - 1)
                Me.cmbBlockMoveY.SelectedIndex = (y - 1)
                'V5.0.0.9⑯              ↑

                'V6.0.1.3①                Form1.SetMapBorder(gCurBlockNo, Color.Black)            'V4.12.2.0①
                Form1.SetMapBorder(gCurPlateNo, gCurBlockNo, Color.Black)   'V6.0.1.3①
            End If
STP_END:
            TmKeyCheck.Enabled = True                                   ' タイマー開始

        Catch ex As Exception
            Dim strMsg As String
            strMsg = "frmFineAdjust.btnBlkPrvMove_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Sub
#End Region

#Region "次ブロック移動処理(Nextボタン押下時)"
    '''=========================================================================
    ''' <summary>次ブロック移動処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub btnBlkNextMove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBlkNextMove.Click

        Dim intRet As Integer
        Dim workBlockNo As Integer
        Dim workPlateNo As Integer
        Dim plateCnt As Integer
        Dim blockCnt As Integer

        Try
            ' プレート数、ブロック数を取得する
            TmKeyCheck.Enabled = False                                  ' タイマー停止
            plateCnt = GetPlateCnt()
            blockCnt = GetBlockCnt()

            If (gCurPlateNo >= plateCnt) And (gCurBlockNo >= blockCnt) Then
                ' 最終プレート、最終ブロックであれば移動しない
                GoTo STP_END
            ElseIf (gCurBlockNo >= blockCnt) Then
                ' 最終ブロックであれば、次のプレートの先頭へ
                workPlateNo = gCurPlateNo + 1
                workBlockNo = 1
            Else
                ' 次のブロックへ移動する
                workPlateNo = gCurPlateNo
                workBlockNo = gCurBlockNo + 1
            End If

            'V4.12.2.0③             ↓
            If (_procBlockSelected) Then
#If False Then  'V6.0.1.3①
                workBlockNo = Globals_Renamed.GetNextProcBlock(workBlockNo)
                If (blockCnt < workBlockNo) Then
                    If (gCurPlateNo < plateCnt) Then
                        ' 最終選択ブロックであれば、次のプレートの先頭選択ブロックへ
                        workPlateNo = gCurPlateNo + 1
                        workBlockNo = Globals_Renamed.GetNextProcBlock(1)
                    Else
                        ' 最終プレート、最終選択ブロックであれば移動しない
                        GoTo STP_END
                    End If
                End If
#Else
                Dim hasNext As Boolean =
                    Form1.GetNextSelectBlock(workPlateNo, workBlockNo, workPlateNo, workBlockNo)

                If (False = hasNext) Then
                    ' 最終プレート、最終選択ブロックであれば移動しない
                    GoTo STP_END
                End If
#End If
            End If
            'V4.12.2.0③             ↑

            ' ステージ移動
            intRet = MoveTargetStagePos(workPlateNo, workBlockNo)
            If (intRet = MOVE_NEXT) Then
                ' 移動後のプレート番号、ブロック番号を保存する
                gCurBlockNo = workBlockNo
                gCurPlateNo = workPlateNo
                lblBlockNo.Text = "BlockNo=" + gCurBlockNo.ToString("0")    'V4.9.0.0⑤
                'V4.0.0.0⑬↓
                If gKeiTyp = KEY_TYPE_RS Then
                    'V5.0.0.9⑯                    SetBlockDisplayNumber(gCurBlockNo)
                    If (1 = giStartBlkAss) Then SetBlockDisplayNumber(gCurBlockNo) 'V5.0.0.9⑯
                    SetNowBlockDspNum(gCurBlockNo)                           'V4.1.0.0⑱
                End If
                'V4.0.0.0⑬↑

                'V5.0.0.9⑯              ↓
                Dim x, y As Integer : Form1.Get_StartBlkNum(x, y)
                Me.cmbBlockMoveX.SelectedIndex = (x - 1)
                Me.cmbBlockMoveY.SelectedIndex = (y - 1)
                'V5.0.0.9⑯              ↑

                'V6.0.1.3①                Form1.SetMapBorder(gCurBlockNo, Color.Black)            'V4.12.2.0①
                Form1.SetMapBorder(gCurPlateNo, gCurBlockNo, Color.Black)       'V6.0.1.3①
            End If
STP_END:
            TmKeyCheck.Enabled = True                                   ' タイマー開始

        Catch ex As Exception
            Dim strMsg As String
            strMsg = "frmFineAdjust.btnBlkNextMove_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Sub
#End Region

#Region ""
    ''' <summary></summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>'V5.0.0.9⑯</remarks>
    Private Sub cmbBlockMoveXY_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cmbBlockMoveY.SelectionChangeCommitted, cmbBlockMoveX.SelectionChangeCommitted
        Try
            TmKeyCheck.Enabled = False                                  ' タイマー停止

            Dim x As Integer = Me.cmbBlockMoveX.SelectedIndex + 1
            Dim y As Integer = Me.cmbBlockMoveY.SelectedIndex + 1
            Dim workBlockNo As Integer = basTrimming.GetProcessingOrder(x, y)

            ' ステージ移動
            Dim intRet As Integer = basTrimming.MoveTargetStagePos(Globals_Renamed.gCurPlateNo, workBlockNo)
            If (intRet = frmFineAdjust.MOVE_NEXT) Then
                ' 移動後のブロック番号を保存する
                Globals_Renamed.gCurBlockNo = workBlockNo
                lblBlockNo.Text = "BlockNo=" + gCurBlockNo.ToString("0")

                If (Globals_Renamed.gKeiTyp = Globals_Renamed.KEY_TYPE_RS) Then
                    SimpleTrimmer.SetNowBlockDspNum(gCurBlockNo)
                End If
            End If

        Catch ex As Exception
            Dim strMsg As String
            strMsg = "frmFineAdjust.cmbBlockMoveXY_SelectionChangeCommitted() TRAP ERROR = " & ex.Message
            MessageBox.Show(Me, strMsg)
        Finally
            TmKeyCheck.Enabled = True                                   ' タイマー開始
        End Try
    End Sub
#End Region

#Region "Ten Key On/Offボタン押下時処理"
    '''=========================================================================
    ''' <summary>Ten Key On/Offボタン押下時処理 ###057</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnTenKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnTenKey.Click

        'Dim InpKey As UShort
        Dim strMSG As String

        Try
            Call SubBtnTenKey_Click()                                   ' ###139

            '' InpKeyのHI SW以外はOFFする' ###139
            'GetInpKey(InpKey)
            'If (InpKey And cBIT_HI) Then                                ' HI SW ON ?
            '    InpKey = cBIT_HI
            'Else
            '    InpKey = 0
            'End If
            'PutInpKey(InpKey)

            '' Ten Key On/Offボタン設定
            'If (BtnTenKey.Text = "Ten Key Off") Then
            '    gbTenKeyFlg = True
            '    BtnTenKey.Text = "Ten Key On"
            '    BtnTenKey.BackColor = System.Drawing.Color.Pink
            'Else
            '    gbTenKeyFlg = False
            '    BtnTenKey.Text = "Ten Key Off"
            '    BtnTenKey.BackColor = System.Drawing.SystemColors.Control
            'End If

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FrmFineAdjust.BtnTenKey_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "Ten Key On/Offボタン押下時処理"
    '''=========================================================================
    ''' <summary>Ten Key On/Offボタン押下時処理 ###139</summary>
    '''=========================================================================
    Private Sub SubBtnTenKey_Click()

        Dim InpKey As UShort
        Dim strMSG As String

        Try
            ' InpKeyのHI SW以外はOFFする
            GetInpKey(InpKey)
            If (InpKey And cBIT_HI) Then                                ' HI SW ON ?
                InpKey = cBIT_HI
            Else
                InpKey = 0
            End If
            PutInpKey(InpKey)

            ' Ten Key On/Offボタン設定
            If (BtnTenKey.Text = "Ten Key Off") Then
                gbTenKeyFlg = True
                BtnTenKey.Text = "Ten Key On"
                BtnTenKey.BackColor = System.Drawing.Color.Pink
                'V6.0.1.0⑪                Form1.SetTrimMapVisible(False)                          'V6.0.1.0⑩
            Else
                gbTenKeyFlg = False
                BtnTenKey.Text = "Ten Key Off"
                BtnTenKey.BackColor = System.Drawing.SystemColors.Control
            End If
            Sub_10KeyUp(Keys.None, stJOG)       'V6.0.1.0③
            'V6.0.1.0⑪            Form1.SetMapOnOffButtonEnabled(Not gbTenKeyFlg)             'V4.12.2.0①

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FrmFineAdjust.SubBtnTenKey_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "データ編集ボタン押下時処理"
    '''=========================================================================
    ''' <summary>データ編集ボタン押下時処理 ###063</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnEdit.Click

        Dim r As Integer
        Dim strMSG As String

        Try
            ' 初期処理
            GrpArrow.Visible = False                                    ' JOGコントロールを非表示にする 
            Me.Hide()                                                   ' 一時停止画面を非表示とする 

            ' データ編集プログラムを起動する(一時停止モード)
            r = Form1.ExecEditProgram(1)                                ' 非常停止等のエラー発生時はAPP終了するので戻ってこない 
            Call ZCONRST()                                              ' コンソールキーラッチ解除 ###226
            If (r <> cFRS_NORMAL) Then
                GoTo STP_ERR                                            ' 一時停止画面処理終了へ
            End If

            ' プレートのスタートポジション設定                          ' ###079 File_Read()をCallするとgBlkStagePosX,Y()がクリアされるので再設定する
            Call CalcPlateXYStartPos()
            ' ブロックのスタートポジション設定                          ' ###079
            Call CalcBlockXYStartPos()

            ' トリミングデータをINtime側に送信する ###087
            Call TRIMEND()                                              ' INtime内のメモリ解放
            '----- ###257↓ -----
            ' FL側から現在の加工条件を受信する
            r = TrimCondInfoRcv(stCND)
            If (r <> SerialErrorCode.rRS_OK) Then
                ' "ＦＬ側加工条件のリードに失敗しました。"
                Call Form1.System1.TrmMsgBox(gSysPrm, MSG_141, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, TITLE_4)
                GoTo STP_ERR                                            ' 一時停止画面処理終了へ
            End If
            '----- ###257↑ -----
            r = SendTrimData()                                          ' トリミングデータをINtime側に送信する
            If (r <> cFRS_NORMAL) Then
                ' "トリミングデータの設定に失敗しました。" & vbCrLf & "トリミングデータに問題がないか確認してください。"
                Call Form1.System1.TrmMsgBox(gSysPrm, MSGERR_SEND_TRIMDATA, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, TITLE_4)
                GoTo STP_ERR                                            ' 一時停止画面処理終了へ
            End If

            'V6.0.1.0⑱↓
            'カット位置補正パラメータの初期化：一時停止時の編集で変更されたときに対応できるように
            '---------------------------------------------------------------------------
            '   カット位置補正用パターン登録情報テーブルを設定する【TKY用】
            '   ※TKY時のみ有効だがCHIP/NET時もテーブル設定(初期化)はする
            '---------------------------------------------------------------------------
            giCutPosRNum = CutPosCorrectInit(gRegistorCnt, stCutPos)
            'V6.0.1.0⑱↑


            ' 後処理
            GrpArrow.Visible = True                                     ' JOGコントロールを表示する 

            Call ZCONRST()                                              'V4.7.0.0⑱ｺﾝｿｰﾙｷｰﾗｯﾁ解除
            TmKeyCheck.Interval = 10                                    'V4.7.0.0⑱
            TmKeyCheck.Enabled = True                                   'V4.7.0.0⑱　タイマー開始

            Me.Show()                                                   ' 一時停止画面を表示する 
            Return

            ' エラー発生時
STP_ERR:
            mExit_flg = cFRS_ERR_RST                                    ' Return値 = Cancel(RESETｷｰ)  
            gbExitFlg = True                                            ' 終了フラグON
            gfrmAdjustDisp = 0                                           'V5.0.0.1-29
            Me.Close()

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FrmFineAdjust.BtnEdit_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "CLRボタン押下時処理"
    '''=========================================================================
    ''' <summary>CLRボタン押下時処理 ###139</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub btnCounterClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCounterClear.Click

        Dim r As Short
        Dim strMSG As String

        Try
            ' 累計をクリアしてもよろしいですか？
            r = MsgBox(MSG_108, MsgBoxStyle.YesNo + MsgBoxStyle.SystemModal + MsgBoxStyle.MsgBoxSetForeground)
            If (r = MsgBoxResult.Yes) Then
                Call Form1.System1.OperationLogging(gSysPrm, MSG_OPLOG_CLRTOTAL, "MANUAL")
                Call Form1.ClearCounter(1)                              ' 生産管理データのクリア
                Call ClrTrimPrnData()                                   ' ﾄﾘﾐﾝｸﾞ結果印刷項目のﾃﾞｰﾀをｸﾘｱする(ローム殿特注) V1.18.0.0③

                ' 統計表示がONの場合、表示を更新する
                If Form1.chkDistributeOnOff.Checked = True Then
                    gObjFrmDistribute.RedrawGraph()
                End If
            End If

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "frmFineAdjust.btnCounterClear_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "グラフ表示/非表示ボタン押下時処理"
    '''=========================================================================
    ''' <summary>グラフ表示/非表示ボタン押下時処理 ###139</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub chkDistributeOnOff_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDistributeOnOff.CheckedChanged

        Dim strMSG As String

        Try
            ' 統計グラフ表示のON/OFF
            If chkDistributeOnOff.Checked = True Then
                ' 統計グラフ表示のON
                Form1.chkDistributeOnOff.Checked = True                 ' ボタン =「生産グラフ表示」 
                Form1.changefrmDistStatus(1)                            ' グラフ表示 
                gObjFrmDistribute.RedrawGraph()                         ' 表示を更新する

                'ボタン表示の変更
                'If (gSysPrm.stTMN.giMsgTyp = 0) Then
                '    Form1.chkDistributeOnOff.Text = "生産グラフ　非表示"
                '    chkDistributeOnOff.Text = Form1.chkDistributeOnOff.Text
                'Else
                '    Form1.chkDistributeOnOff.Text = "Distribute OFF"
                '    chkDistributeOnOff.Text = Form1.chkDistributeOnOff.Text
                'End If
                Form1.chkDistributeOnOff.Text = frmDistribution_004
                chkDistributeOnOff.Text = Form1.chkDistributeOnOff.Text

                ' 矢印ボタン非活性化
                Call SetBtnArowEnable(False)

                '----- ###150↓ -----
                ' 統計表示時は統計表示上のボタンを有効にする
                gObjFrmDistribute.cmdGraphSave.Enabled = True
                gObjFrmDistribute.cmdInitial.Enabled = True
                gObjFrmDistribute.cmdFinal.Enabled = True
                '----- ###150↑ -----

            Else
                ' 統計グラフ表示のOFF
                Form1.chkDistributeOnOff.Checked = False            ' ボタン =「生産グラフ表示」 
                Form1.changefrmDistStatus(0)                        ' グラフ非表示 

                ' ボタン表示の変更
                'If (gSysPrm.stTMN.giMsgTyp = 0) Then
                '    Form1.chkDistributeOnOff.Text = "生産グラフ　表示"
                '    chkDistributeOnOff.Text = Form1.chkDistributeOnOff.Text
                'Else
                '    Form1.chkDistributeOnOff.Text = "Distribute ON"
                '    chkDistributeOnOff.Text = Form1.chkDistributeOnOff.Text
                'End If
                Form1.chkDistributeOnOff.Text = frmDistribution_003
                chkDistributeOnOff.Text = Form1.chkDistributeOnOff.Text

                ' 矢印ボタン活性化
                Call SetBtnArowEnable(True)
                Call Sub_SetBtnArrowEnable()                        ' V1.16.0.0⑦

            End If

        Catch ex As Exception
            strMSG = "frmFineAdjust.chkDistributeOnOff_CheckedChanged() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- ###237↓ -----
#Region "LASERボタン押下時処理"
    '''=========================================================================
    ''' <summary>LASERボタン押下時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnLaser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnLaser.Click

        Dim r As Integer
        Dim strMSG As String

        Try
            ' LASER射出可能/不可の切り替え
            If (BtnLaser.BackColor = System.Drawing.SystemColors.Control) Then
                ' LASER射出可能とする
                BtnLaser.BackColor = System.Drawing.Color.OrangeRed
            Else
                ' LASER射出不可とする
                BtnLaser.BackColor = System.Drawing.SystemColors.Control
                r = LASEROFF()
                m_LaserOnOffFlag = False
            End If

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "frmFineAdjust.BtnLaser_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- ###237↑ -----

    '    '========================================================================================
    '    '   共通関数
    '    '========================================================================================
    '#Region "ステージ移動処理"
    '    '''=========================================================================
    '    ''' <summary>ステージ移動処理</summary>
    '    ''' <param name="pltNo"></param>
    '    ''' <param name="blkNo"></param>
    '    ''' <returns></returns>
    '    '''=========================================================================
    '    Public Function MoveTargetStagePos(ByVal pltNo As Integer, ByVal blkNo As Integer) As Integer
    '        '    Private Function MoveTargetStagePos(ByVal pltNo As Integer, ByVal blkNo As Integer) As Integer

    '        Dim intRet As Integer
    '        Dim nextStgX As Double
    '        Dim nextStgY As Double
    '        Dim dispPltX As Integer
    '        Dim dispPltY As Integer
    '        Dim dispBlkX As Integer
    '        Dim dispBlkY As Integer
    '        'Dim retBlkNoX As Integer
    '        'Dim retBlkNoY As Integer
    '        Dim dispCurStgGrpNoX As Integer
    '        Dim dispCurStgGrpNoY As Integer
    '        Dim dispCurBlkNoX As Integer
    '        Dim dispCurBlkNoY As Integer
    '        Dim dispCurPltNoX As Integer
    '        Dim dispCurPltNoY As Integer
    '        Dim StgX As Double = 0.0 ' V4.0.0.0-40
    '        Dim StgY As Double = 0.0 ' V4.0.0.0-40

    '        Try
    '            MoveTargetStagePos = MOVE_NEXT
    '            intRet = GetTargetStagePos(pltNo, blkNo, nextStgX, nextStgY, dispPltX, dispPltY, dispBlkX, dispBlkY)
    '            If intRet = BLOCK_END Then
    '                ' 何もしないで終了
    '                MoveTargetStagePos = MOVE_NOT
    '                Exit Function
    '            ElseIf intRet = PLATE_BLOCK_END Then
    '                ' 何もしないで終了
    '                MoveTargetStagePos = MOVE_NOT
    '                Exit Function
    '            End If

    '            '---------------------------------------------------------------------
    '            '   表示用各ポジションの番号を設定（プレート/ステージグループ/ブロック）
    '            '---------------------------------------------------------------------
    '            Dim bRet As Boolean
    '            bRet = GetDisplayPosInfo(dispBlkX, dispBlkY, _
    '                            dispCurStgGrpNoX, dispCurStgGrpNoY, dispCurBlkNoX, dispCurBlkNoY)

    '            '---------------------------------------------------------------------
    '            '   ログ表示文字列の設定
    '            '---------------------------------------------------------------------
    '            dispCurPltNoX = dispPltX : dispCurPltNoY = dispPltY         '###056
    '            Call DisplayStartLog(dispCurPltNoX, dispCurPltNoY, _
    '                            dispCurStgGrpNoX, dispCurStgGrpNoY, dispCurBlkNoX, dispCurBlkNoY)
    '            ' ステージの動作
    '            '----- V1.13.0.0③↓ -----
    '            ' 伸縮補正用パラメータの設定
    '            GetShinsyukuData(dispBlkX, dispBlkY, nextStgX, nextStgY)
    '            '----- V2.0.0.0⑨↓ -----
    '            If (giMachineKd = MACHINE_KD_RS) Then
    '                '----- V4.0.0.0-40↓ -----
    '                ' SL36S時でステージYの原点位置が上の場合、ブロックサイズの1/2は加算しない
    '                '↓
    '                'V4.6.0.0④　If (giStageYOrg = STGY_ORG_UP) Then
    '                StgX = nextStgX + typPlateInfo.dblTableOffsetXDir + gfCorrectPosX + gfStgOfsX
    '                StgY = nextStgY + typPlateInfo.dblTableOffsetYDir + gfCorrectPosY + gfStgOfsY
    '                'V4.6.0.0④　Else
    '                'V4.6.0.0④　StgX = nextStgX + typPlateInfo.dblTableOffsetXDir + gfCorrectPosX + gfStgOfsX
    '                'V4.6.0.0④　StgY = nextStgY + typPlateInfo.dblTableOffsetYDir + gfCorrectPosY + gfStgOfsY ' + (typPlateInfo.dblBlockSizeYDir / 2)
    '                'V4.6.0.0④　End If
    '                'V4.6.0.0④　↑
    '                intRet = Form1.System1.EX_START(gSysPrm, StgX, StgY, 0)

    '                'intRet = Form1.System1.EX_START(gSysPrm, nextStgX + typPlateInfo.dblTableOffsetXDir + gfCorrectPosX + gfStgOfsX, _
    '                '                        nextStgY + typPlateInfo.dblTableOffsetYDir + gfCorrectPosY + gfStgOfsY + (typPlateInfo.dblBlockSizeYDir / 2), 0)
    '                '----- V4.0.0.0-40↑ -----
    '            Else
    '                intRet = Form1.System1.EX_START(gSysPrm, nextStgX + typPlateInfo.dblTableOffsetXDir + gfCorrectPosX + gfStgOfsX, _
    '                                        nextStgY + typPlateInfo.dblTableOffsetYDir + gfCorrectPosY + gfStgOfsY, 0)
    '            End If
    '            'intRet = Form1.System1.EX_START(gSysPrm, nextStgX + typPlateInfo.dblTableOffsetXDir + gfCorrectPosX + gfStgOfsX, _
    '            '                        nextStgY + typPlateInfo.dblTableOffsetYDir + gfCorrectPosY + gfStgOfsY, 0)
    '            '----- V2.0.0.0⑨↑ ----
    '            '----- V1.13.0.0③↑ -----

    '        Catch ex As Exception
    '            Dim strMsg As String
    '            strMsg = "frmFineAdjust.btnTrimming_Click() TRAP ERROR = " + ex.Message
    '            MsgBox(strMsg)
    '        End Try
    '    End Function
    '#End Region

#Region "矢印ボタン活性化/非活性化"
    '''=========================================================================
    ''' <summary>矢印ボタン活性化/非活性化 ###139</summary>
    ''' <param name="OnOff"></param>
    '''=========================================================================
    Private Sub SetBtnArowEnable(ByVal OnOff As Boolean)

        Dim strMSG As String

        Try
            ' 矢印ボタン活性化/非活性化
            BtnJOG_0.Enabled = OnOff
            BtnJOG_1.Enabled = OnOff
            BtnJOG_2.Enabled = OnOff
            BtnJOG_3.Enabled = OnOff
            BtnJOG_4.Enabled = OnOff
            BtnJOG_5.Enabled = OnOff
            BtnJOG_6.Enabled = OnOff
            BtnJOG_7.Enabled = OnOff
            BtnHI.Enabled = OnOff

            ' Ten Keyボタン活性化/非活性化
            BtnTenKey.Enabled = OnOff

            ' Ten KeyボタンをOn/Offにする
            If (OnOff = False) Then
                ' 矢印ボタン非活性化ならTen KeyボタンをOffにしてテンキー入力を不可とする
                If (BtnTenKey.Text = "Ten Key On") Then
                    m_TenKeyFlg = True
                    Call SubBtnTenKey_Click()
                End If

                Form1.Instance.SetActiveJogMethod(Nothing, Nothing, Nothing)  'V6.0.0.0⑩

            Else
                ' Ten KeyボタンをOffにした場合はTen KeyボタンをOnにしてテンキー入力を可とする
                If (m_TenKeyFlg = True) Then
                    m_TenKeyFlg = False
                    Call SubBtnTenKey_Click()
                End If
            End If
            Sub_10KeyUp(Keys.None, stJOG)                               'V6.0.1.0②

        Catch ex As Exception
            strMSG = "frmFineAdjust.SetBtnArowEnable() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

    '===============================================================================
    '   Description  : ＪＯＧ操作画面処理
    '
    '   Copyright(C) : OMRON LASERFRONT INC. 2012
    '
    '===============================================================================
    '========================================================================================
    '   ボタン押下時処理
    '========================================================================================
#Region "RESETボタン押下時処理"
    '''=========================================================================
    ''' <summary>RESETボタン押下時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnRESET_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnRESET.Click
        mExit_flg = cFRS_ERR_RST                                        ' Return値 = Cancel(RESETｷｰ)  
        gbExitFlg = True                                                ' 終了フラグON
        gfrmAdjustDisp = 0                  'V5.0.0.1-29
        Me.Close()
    End Sub
#End Region

#Region "STARTボタン押下時処理"
    '''=========================================================================
    ''' <summary>STARTボタン押下時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnSTART_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSTART.Click
        Dim r As Integer

        If (gKeiTyp = KEY_TYPE_RS) Then
            r = GetLaserOffIO(True) 'V5.0.0.1⑫
            If r = 1 Then
                ''V5.0.0.1⑨↓
                r = cFRS_NORMAL
                Call ZCONRST()
                ''V5.0.0.1⑨↑
            Else
                mExit_flg = cFRS_ERR_START                                        ' Return値 = START 
                gbExitFlg = True                                                ' 終了フラグON
                gfrmAdjustDisp = 0                  'V5.0.0.1-29
                Me.Close()
            End If
        Else
            mExit_flg = cFRS_ERR_START                                        ' Return値 = START 
            gbExitFlg = True                                                ' 終了フラグON
            gfrmAdjustDisp = 0                  'V5.0.0.1-29
            Me.Close()
        End If

    End Sub
#End Region


#Region "HIボタン押下時処理"
    '''=========================================================================
    ''' <summary>HIボタン押下時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnHI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnHI.Click
        Call SubBtnHI_Click(stJOG)
    End Sub
#End Region

#Region "矢印ボタンのマウスクリック時処理"
    '''=========================================================================
    ''' <summary>矢印ボタンのマウスクリック時処理</summary>
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
    '----- ###219 -----
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
    '----- ###219 -----

    '========================================================================================
    '   テンキー入力処理
    '========================================================================================
#Region "キーダウン時処理"
    '''=========================================================================
    ''' <summary>キーダウン時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub frmFineAdjust_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles Me.KeyDown 'V6.0.0.0⑪
        Me.JogKeyDown(e)                'V6.0.0.0⑪
    End Sub

    Public Sub JogKeyDown(ByVal e As KeyEventArgs) Implements ICommonMethods.JogKeyDown    'V6.0.0.0⑪
        'V6.0.0.0⑪        Dim KeyCode As Short = e.KeyCode
        Dim KeyCode As Keys = e.KeyCode             'V6.0.0.0⑪
        Dim r As Integer

        '----- ###237↓ -----
        ' LASER射出可能で「*キー」押下ならLASER射出する()
        If (BtnLaser.BackColor = System.Drawing.Color.OrangeRed) And (KeyCode = System.Windows.Forms.Keys.Multiply) Then
            ' レーザON
            If (m_LaserOnOffFlag = False) Then
                ' DIG-SW設定 
                Call Form1.SetMoveMode(CbDigSwL.SelectedIndex, CbDigSwH.SelectedIndex) 'V5.0.0.1⑫

                ''V4.0.0.0-86
                r = GetLaserOffIO(False)
                If r = 1 Then
                    Me.ShowInTaskbar = False 'V5.0.0.1⑫
                    Me.Activate()  'V5.0.0.1⑫
                    'frmFineAdjust_KeyUp(sender, e)

                    Exit Sub
                End If
                ''V4.0.0.0-86
                Call LASERON()
                m_LaserOnOffFlag = True
                Console.WriteLine("frmFineAdjust_KeyDown() Laser On")
            End If
        End If
        '----- ###237↑ -----

        ' テンキー入力フラグがOFFならNOP ###057
        If (gbTenKeyFlg = False) Then Exit Sub

        ' テンキーダウンならInpKeyにテンキーコードを設定する
        'V6.0.0.0⑫       'Call Sub_10KeyDown(KeyCode)
        Sub_10KeyDown(KeyCode, stJOG)             'V6.0.0.0⑫
        If (KeyCode = System.Windows.Forms.Keys.NumPad5) Then       ' 5ｷｰ (KeyCode = 101(&H65)
            'Call BtnHI_Click(sender, e)                             ' HIボタン ON/OFF
            Call BtnHI_Click(BtnHI, e)                              ' HIボタン ON/OFF     'V6.0.0.0⑩
        End If
        'Call Me.Focus()

    End Sub
#End Region

#Region "キーアップ時処理"
    '''=========================================================================
    ''' <summary>キーアップ時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub frmFineAdjust_KeyUp(ByVal sender As Object, ByVal e As KeyEventArgs) Handles Me.KeyUp 'V6.0.0.0⑪
        Me.JogKeyUp(e)                  'V6.0.0.0⑪
    End Sub

    Public Sub JogKeyUp(ByVal e As KeyEventArgs) Implements ICommonMethods.JogKeyUp        'V6.0.0.0⑪
        'V6.0.0.0⑪        Dim KeyCode As Short = e.KeyCode
        Dim KeyCode As Keys = e.KeyCode             'V6.0.0.0⑪

        '----- ###237↓ -----
        ' LASER Offする
        If (m_LaserOnOffFlag = True) Then
            Call LASEROFF()
            m_LaserOnOffFlag = False
            Console.WriteLine("frmFineAdjust_KeyUp() Laser Off")
        End If
        '----- ###237↑ -----

        ' テンキー入力フラグがOFFならNOP ###057
        'V6.0.1.0③        If (gbTenKeyFlg = False) Then Exit Sub
        If (False = gbTenKeyFlg) Then       'V6.0.1.0③
            Sub_10KeyUp(Keys.None, stJOG)   'V6.0.1.0③
        Else
            ' テンキーアップならInpKeyのテンキーコードをOFFする
            'V6.0.0.0⑫        Call Sub_10KeyUp(KeyCode)
            Sub_10KeyUp(KeyCode, stJOG)                   'V6.0.0.0⑫
            'Call Me.Focus()
        End If

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

    'V4.0.0.0-84
    ''' <summary>
    ''' 一時停止画面でCloseボタンを押したときの処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click

        If gKeiTyp = KEY_TYPE_RS Then
            ' DIG-SW設定  // 'V4.1.0.0⑭
            Call Form1.SetMoveMode(CbDigSwL.SelectedIndex, CbDigSwH.SelectedIndex)

            'V4.8.0.1②↓
            ' BPオフセット更新(タイミングによって空白で入ってくる場合トラップエラーとなるのでチェックする ###014)
            If (txtBpOffX.Text <> "") And (txtBpOffY.Text <> "") Then
                Call SetBpOffset(Double.Parse(txtBpOffX.Text), Double.Parse(txtBpOffY.Text))
            End If
            'V4.8.0.1②↑

            SetADJButton()      'V4.1.0.0⑮

            ' データ表示を非表示にする
            Call Form1.SetDataDisplayOn()
            GroupBoxVisibleChange(True)
            SetSimpleVideoSize()
            Form1.TimerAdjust.Enabled = True

            'V5.0.0.9⑯                  ↓
            'V5.0.0.9⑯            Call SimpleTrimmer.ResistorDataDisp(True, 0, 1)
            Dim blkNo As Integer = TrimData.GetBlockNumber()
            If (0 = blkNo) Then blkNo = 1
            SimpleTrimmer.ResistorDataDisp(True, blkNo, 1)
            'V5.0.0.9⑯                  ↑

            Call Sub_StopTimer()                                ' ###260
            '----- V4.11.0.0⑤↓ (WALSIN殿SL436S対応) -----
            ' トリミング開始ブロック番号を表示する
            'V5.0.0.9⑯            If (giStartBlkAss = 1) Then                             ' トリミング開始ブロック番号指定の有効/無効(0=無効, 1=有効)　
            If (giStartBlkAss <> 0) Then                             ' トリミング開始ブロック番号指定の有効/無効(0=無効, 1=有効)    'V5.0.0.9⑯
                Form1.GrpStartBlk.Visible = True
                'V5.0.0.1①↓
                'V5.0.0.9⑯                Call Form1.Set_StartBlkNum(gCurBlockNo, 1)
                If (1 = giStartBlkAss) Then Form1.Set_StartBlkNum(gCurBlockNo, 1) 'V5.0.0.9⑯
                SetNowBlockDspNum(gCurBlockNo)
                'V5.0.0.1①↑
            End If
            '----- V4.11.0.0⑤↑ -----

            gfrmAdjustDisp = 0                  'V5.0.0.1-29
            Close()

        End If
    End Sub

    'V5.0.0.1⑫
    Private Sub CbDigSwL_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CbDigSwL.SelectedIndexChanged

        ' DIG-SW設定  
        Call Form1.SetMoveMode(CbDigSwL.SelectedIndex, CbDigSwH.SelectedIndex)

    End Sub

    Private Sub lblBlockNo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblBlockNo.Click
        lblBlockNo = lblBlockNo
    End Sub


    Private Sub frmFineAdjust_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        lblBlockNo = lblBlockNo
    End Sub

    'V6.0.0.0⑳    Private Sub frmFineAdjust_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseDown

    'V6.0.0.0⑳    End Sub

    ''' <summary></summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>'V6.0.0.0⑳</remarks>
    Private Sub frmFineAdjust_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Form1.Instance.SetActiveJogMethod(Nothing, Nothing, Nothing)

        ' 最後に表示したブロックの枠線を消す   'V4.12.2.0①
        Form1.ClearMapBorder()

        'V6.0.1.0⑪        ' MAP ON/OFF ボタンを非表示にする     'V4.12.2.0①
        'V6.0.1.0⑪        Form1.SetMapOnOffButtonEnabled(False)
    End Sub

End Class