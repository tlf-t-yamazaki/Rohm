'===============================================================================
'   Description  : 分布図表示処理
'
'   Copyright(C) : OMRON LASERFRONT INC. 2010
'
'===============================================================================
Option Strict Off
Option Explicit On

Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8①
Imports TKY_ALL_SL432HW.My.Resources    'V4.4.0.0-0

Friend Class frmDistribution
	Inherits System.Windows.Forms.Form
#Region "プライベート定数定義"
    '===========================================================================
    '   定数定義
    '===========================================================================
    ''----- 画面ｺﾋﾟ ｰ----
    '' ｷｰｽﾄﾛｰｸをｼｭﾐﾚｰﾄする
    'Private Declare Sub keybd_event Lib "user32.dll" (ByVal bVk As Byte, ByVal bScan As Byte, ByVal dwFlags As Integer, ByVal dwExtraInfo As Integer)
    'Private Const VK_SNAPSHOT As Short = &H2CS          ' PrtSc key
    'Private Const VK_LMENU As Short = &HA4S             ' Alt key
    'Private Const KEYEVENTF_KEYUP As Short = &H2S       ' ｷｰはUP状態
    'Private Const KEYEVENTF_EXTENDEDKEY As Short = &H1S ' ｽｷｬﾝは拡張ｺｰﾄﾞ

    ' 画面表示位置オフセット
    'Private Const DISP_X_OFFSET As Integer = 4                         '###065
    'Private Const DISP_Y_OFFSET As Integer = 20                        '###065
    Private Const DISP_X_OFFSET As Integer = 0                          '###065
    Private Const DISP_Y_OFFSET As Integer = 0                          '###065

#End Region

#Region "メンバ変数定義"
    '===========================================================================
    '   メンバ変数定義
    '===========================================================================
    Private m_bInitDistForm As Boolean
    Private m_bFgDispGrp As Boolean                                ' 表示ｸﾞﾗﾌ種別(TRUE:IT FALSE:FT)
#End Region

#Region "フォーム初期化"
    '''=========================================================================
    '''<summary>ﾌｫｰﾑ初期化時処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub InitializeForm()
        Dim strMSG As String

        Try
            ' 分布図表示用ラベル配列の初期化
            gDistRegNumLblAry(0) = Me.LblRegN_00             ' 分布グラフ抵抗数配列(0～11)
            gDistRegNumLblAry(1) = Me.LblRegN_01
            gDistRegNumLblAry(2) = Me.LblRegN_02
            gDistRegNumLblAry(3) = Me.LblRegN_03
            gDistRegNumLblAry(4) = Me.LblRegN_04
            gDistRegNumLblAry(5) = Me.LblRegN_05
            gDistRegNumLblAry(6) = Me.LblRegN_06
            gDistRegNumLblAry(7) = Me.LblRegN_07
            gDistRegNumLblAry(8) = Me.LblRegN_08
            gDistRegNumLblAry(9) = Me.LblRegN_09
            gDistRegNumLblAry(10) = Me.LblRegN_10
            gDistRegNumLblAry(11) = Me.LblRegN_11

            gDistGrpPerLblAry(0) = Me.LblGrpPer_00           ' 分布グラフ%配列(0～11)
            gDistGrpPerLblAry(1) = Me.LblGrpPer_01
            gDistGrpPerLblAry(2) = Me.LblGrpPer_02
            gDistGrpPerLblAry(3) = Me.LblGrpPer_03
            gDistGrpPerLblAry(4) = Me.LblGrpPer_04
            gDistGrpPerLblAry(5) = Me.LblGrpPer_05
            gDistGrpPerLblAry(6) = Me.LblGrpPer_06
            gDistGrpPerLblAry(7) = Me.LblGrpPer_07
            gDistGrpPerLblAry(8) = Me.LblGrpPer_08
            gDistGrpPerLblAry(9) = Me.LblGrpPer_09
            gDistGrpPerLblAry(10) = Me.LblGrpPer_10
            gDistGrpPerLblAry(11) = Me.LblGrpPer_11

            gDistShpGrpLblAry(0) = Me.LblShpGrp_00                      ' 分布グラフ配列(0～11)
            gDistShpGrpLblAry(1) = Me.LblShpGrp_01
            gDistShpGrpLblAry(2) = Me.LblShpGrp_02
            gDistShpGrpLblAry(3) = Me.LblShpGrp_03
            gDistShpGrpLblAry(4) = Me.LblShpGrp_04
            gDistShpGrpLblAry(5) = Me.LblShpGrp_05
            gDistShpGrpLblAry(6) = Me.LblShpGrp_06
            gDistShpGrpLblAry(7) = Me.LblShpGrp_07
            gDistShpGrpLblAry(8) = Me.LblShpGrp_08
            gDistShpGrpLblAry(9) = Me.LblShpGrp_09
            gDistShpGrpLblAry(10) = Me.LblShpGrp_10
            gDistShpGrpLblAry(11) = Me.LblShpGrp_11

            'V4.0.0.0⑫↓
            gGoodChip = lblGoodChip
            gNgChip = lblNgChip
            gMaxValue = lblMaxValue
            gMinValue = lblMinValue
            gAverageValue = lblAverageValue
            gDeviationValue = lblDeviationValue
            gGraphAccumulationTitle = lblGraphAccumulationTitle
            gRegistUnit = lblRegistUnit
            'V4.0.0.0⑫↑

            'DistRegItLblAry(i) = New System.Windows.Forms.Label     ' 分布グラフ抵抗数(IT)配列
            'DistRegFtLblAry(i) = New System.Windows.Forms.Label     ' 分布グラフ抵抗数(FT)配列

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "frmDistribution.InitializeForm() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "イニシャル/ファイナル分布図の表示状態"
    Public Function DisplayInitialMode() As Boolean
        Return m_bFgDispGrp
    End Function
#End Region

#Region "分布図保存ボタン押下時処理"
    '''=========================================================================
    '''<summary>分布図保存ボタン押下時処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub cmdGraphSave_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdGraphSave.Click

        ' ボタン制御
        cmdGraphSave.Enabled = False
        cmdInitial.Enabled = False
        cmdFinal.Enabled = False

        ' 画面をハードコピーし印刷する
        Call SaveWindowPic(True, False)

        ' 完了メッセージ

        ' ボタン制御
        cmdGraphSave.Enabled = True
        cmdInitial.Enabled = True
        cmdFinal.Enabled = True

    End Sub
#End Region

#Region "分布図保存処理"
    '''=========================================================================
    '''<summary>分布図保存ボタン押下時処理</summary>
    '''<remarks>PrintScreenキー押下時と同等の処理を行う</remarks>
    '''=========================================================================
    Private Sub SaveWindowPic(Optional ByRef ActWind As Boolean = True, Optional ByRef PrintOn As Boolean = False)

        Dim msg As String               'V4.7.0.0③

        Try
            If (String.IsNullOrEmpty(typPlateInfo.strDataName)) Then Exit Sub 'V4.7.0.0③

            Dim fileName As String
            Dim bFileSave As Boolean
            Dim bitMap As New Bitmap(Me.Width, Me.Height)
            bFileSave = False
            fileName = ""

            ''アクティブなWindowをクリップボードへコピー
            'SendKeys.SendWait("%{PRTSC}")

            '' クリップボードからデータ取得
            'Dim obj As IDataObject = Clipboard.GetDataObject()

            'If obj IsNot Nothing Then
            '    Dim dispImage As Image = DirectCast(obj.GetData(DataFormats.Bitmap), Image)

            '    If dispImage IsNot Nothing Then
            '        If m_bFgDispGrp = True Then
            '            fileName = gSysPrm.stLOG.gsLoggingDir & "IT_MAP" & Now.ToString("yyMMddhhmmss") & ".BMP"
            '        Else
            '            fileName = gSysPrm.stLOG.gsLoggingDir & "FT_MAP" & Now.ToString("yyMMddhhmmss") & ".BMP"
            '        End If

            '        dispImage.Save(fileName)
            '        bFileSave = True
            '    End If
            'End If

            ' ｸﾘｯﾌﾟﾎﾞｰﾄﾞにﾃｷｽﾄ(Bitmap以外？)がｺﾋﾟｰされている状態だと
            ' dispImageがNothingとなって保存されないため変更              'V4.7.0.0③
            Dim ITFT As String
            If (True = m_bFgDispGrp) Then
                ITFT = "_IT_MAP"
            Else
                ITFT = "_FT_MAP"
            End If

            fileName = gSysPrm.stLOG.gsLoggingDir & _
                IO.Path.GetFileNameWithoutExtension(typPlateInfo.strDataName) & _
                ITFT & Now.ToString("yyMMddHHmmss") & ".BMP"

            Using bmp As New Bitmap(Me.Width, Me.Height)
                Me.DrawToBitmap(bmp, New Rectangle(0, 0, Me.Width, Me.Height))
                bmp.Save(fileName)
                bFileSave = True
            End Using

            '結果の表示
            If (bFileSave = True) Then
                'If gSysPrm.stTMN.giMsgTyp = 0 Then
                '    MsgBox("保存完了！" & vbCrLf & " (" & fileName & ")")
                'Else
                '    MsgBox("Save completion." & vbCrLf & " (" & fileName & ")")
                'End If
                msg = frmDistribution_001 & vbCrLf & " (" & fileName & ")"
            Else
                'If gSysPrm.stTMN.giMsgTyp = 0 Then
                '    MsgBox("保存できませんでした。")
                'Else
                '    MsgBox("I was not able to save it.")
                'End If
                msg = frmDistribution_002
            End If

            'V6.0.2.0③　Exit Sub

        Catch ex As Exception
            'If gSysPrm.stTMN.giMsgTyp = 0 Then
            '    MsgBox("保存できませんでした。")
            'Else
            '    MsgBox("I was not able to save it.")
            'End If
            msg = frmDistribution_002
        End Try

        ' 後ろに隠れないように対応       'V4.7.0.0③
        MessageBox.Show(msg, cmdGraphSave.Text, MessageBoxButtons.OK, MessageBoxIcon.None,
                        MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly)

        Me.TopMost = True ''V6.0.2.0④

    End Sub
#End Region

#Region "ファイナルテスト分布図表示ボタン押下時処理"
    '''=========================================================================
    '''<summary>ファイナルテスト分布図表示ボタン押下時処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub cmdFinal_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdFinal.Click
        m_bFgDispGrp = False
        Call RedrawGraph()                                              ' 分布図表示処理
    End Sub
#End Region

#Region "イニシャルテスト分布図表示ボタン押下時処理"
    '''=========================================================================
    '''<summary>イニシャルテスト分布図表示ボタン押下時処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub cmdInitial_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdInitial.Click
        m_bFgDispGrp = True
        Call RedrawGraph()
    End Sub
#End Region

#Region "フォームロード時処理"
    '''=========================================================================
    '''<summary>フォームロード時処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub frmDistribution_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        'Dim utdClientPoint As tagPOINT
        'Dim lngWin32apiResultCode As Integer
        Dim setLocation As System.Drawing.Point

        '初期化実行
        If (m_bInitDistForm = False) Then
            InitializeForm()
            m_bInitDistForm = True
        End If

        'bFgfrmDistribution = True                           ' 生産ｸﾞﾗﾌ表示ﾌﾗｸﾞON

        'Videoの上に表示する。
        setLocation = Form1.VideoLibrary1.Location
        setLocation.X = setLocation.X + DISP_X_OFFSET
        setLocation.Y = setLocation.Y + DISP_Y_OFFSET
        Me.Location = setLocation

        'lblRegistTitle.Text = PIC_TRIM_09
        'lblGoodTitle.Text = PIC_TRIM_03
        'lblNgTitle.Text = PIC_TRIM_04
        'lblMinTitle.Text = PIC_TRIM_05
        'lblMaxTitle.Text = PIC_TRIM_06
        'lblAverage.Text = PIC_TRIM_07
        'lblDeviation.Text = PIC_TRIM_08
        'cmdInitial.Text = PIC_TRIM_01
        'cmdFinal.Text = PIC_TRIM_02

        ' 分布図ﾋﾞｯﾄﾏｯﾌﾟ保存
        cmdGraphSave.Visible = True
        'cmdGraphSave.Text = PIC_TRIM_10
        RedrawGraph()

        '常に最前面に表示する。
        Me.TopMost = True
    End Sub
#End Region

#Region "フォーカスを失った時の処理"
    '''=========================================================================
    '''<summary>ロギング開始(標準)</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub frmDistribution_LostFocus(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.LostFocus
        '    Unload Me
    End Sub
#End Region

#Region "フォームアンロード時処理"
    '''=========================================================================
    '''<summary>フォームアンロード時処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub frmDistribution_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        'bFgfrmDistribution = False                      ' 生産ｸﾞﾗﾌ表示ﾌﾗｸﾞOFF
        Form1.chkDistributeOnOff.Checked = False

        'If (gSysPrm.stTMN.giMsgTyp = 0) Then
        '    Form1.chkDistributeOnOff.Text = "生産グラフ　表示"
        'Else
        '    Form1.chkDistributeOnOff.Text = "Distribute ON"
        'End If
        Form1.chkDistributeOnOff.Text = frmDistribution_003
    End Sub
#End Region

#Region "分布図表示処理"
    '''=========================================================================
    '''<summary>分布図表示処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub RedrawGraph()

        Dim iCnt As Short                                   ' ｶｳﾝﾀ
        Dim lMax As Integer
        Dim lScale As Integer
        Dim lScaleMax As Integer
        Dim dblGraphDiv As Double
        Dim dblGraphTop As Double
        Dim dtemp As Double         ' ###203 

        lMax = 0
        If (m_bFgDispGrp) Then

            'V4.0.0.0⑫↓
            gGraphAccumulationTitle.Text = MSG_TRIM_04                ' "イニシャルテスト　分布図"
            gMinValue.Text = dblMinIT.ToString("0.000")               ' 最小値
            gMaxValue.Text = dblMaxIT.ToString("0.000")               ' 最大値
            'V4.0.0.0⑫↑
            For iCnt = 0 To (MAX_SCALE_RNUM - 1)
                glRegistNum(iCnt) = glRegistNumIT(iCnt)                 ' 分布グラフ抵抗数
                If lMax < glRegistNum(iCnt) Then
                    lMax = glRegistNum(iCnt)
                End If
                'V4.0.0.0⑫↓
                gDistRegNumLblAry(iCnt).Text = CStr(glRegistNum(iCnt)) ' 分布グラフ抵抗数
                'V4.0.0.0⑫↑
            Next

            'OK/NG数の表示
            'V4.0.0.0⑫↓
            gGoodChip.Text = CStr(gITNx_cnt + 1)                      ' OK数
            gNgChip.Text = CStr(gITNg_cnt + 1)                        ' NG数
            'V4.0.0.0⑫↑
        Else
            'V4.0.0.0⑫↓
            gGraphAccumulationTitle.Text = MSG_TRIM_05                ' "ファイナルテスト　分布図"
            gMinValue.Text = dblMinFT.ToString("0.000")               ' 最小値
            gMaxValue.Text = dblMaxFT.ToString("0.000")               ' 最大値
            For iCnt = 0 To (MAX_SCALE_RNUM - 1)

                glRegistNum(iCnt) = glRegistNumFT(iCnt)

                If lMax < glRegistNum(iCnt) Then
                    lMax = glRegistNum(iCnt)
                End If
                gDistRegNumLblAry(iCnt).Text = CStr(glRegistNum(iCnt)) ' 分布グラフ抵抗数
            Next
            'OK/NG数の表示
            'V4.0.0.0⑫↓
            gGoodChip.Text = CStr(gFTNx_cnt + 1)                      ' OK数
            gNgChip.Text = CStr(gFTNg_cnt + 1)                        ' NG数
            'V4.0.0.0⑫↑
        End If

        'lblGoodChip.Text = CStr(lOkChip)                               ' OK数
        'lblNgChip.Text = CStr(lNgChip)                                 ' NG数


        '■■■■■■
        ' 誤差ﾃﾞｰﾀがある(IT)
        '' '' ''Call Form1.GetMoveMode(digL, digH, digSW)
        If gITNx_cnt >= 0 Then
            'If (gDigL = 0) Then                                        ' x0モード ?
            '' '' ''If (digL = 0) Then                                  ' x0モード ?
            '###154 計算は結果取得時にその都度実行する
            '' 平均値取得
            'dblAverageIT = Form1.Utility1.GetAverage(gITNx, gITNx_cnt + 1)
            '' 標準偏差の取得
            'dblDeviationIT = Form1.Utility1.GetDeviation(gITNx, gITNx_cnt + 1, dblAverageIT)
            'TotalDeviationDebug = TotalDeviationDebug '###154
            'TotalAverageDebug = TotalAverageDebug '###154
            '' '' ''End If
        End If

        ' 誤差ﾃﾞｰﾀがある(FT)
        If gFTNx_cnt >= 0 Then
            '###154            ' 平均値取得
            '###154            dblAverageFT = Form1.Utility1.GetAverage(gFTNx, gFTNx_cnt + 1)
            '###154     ' 標準偏差の取得
            '###154         dblDeviationFT = Form1.Utility1.GetDeviation(gFTNx, gFTNx_cnt + 1, dblAverageFT)
            'dblAverageFT = TotalAverageDebug '###154
            'dblDeviationFT = TotalDeviationDebug '###154
        End If
        '■■■■■■■

        If (m_bFgDispGrp) Then
            'V4.0.0.0⑫↓
            gDeviationValue.Text = dblDeviationIT.ToString("0.000000") ' 標準偏差(IT)
            'V4.0.0.0⑫↑

        Else
            'V4.0.0.0⑫↓
            gDeviationValue.Text = dblDeviationFT.ToString("0.000000") ' 標準偏差(FT)
            'V4.0.0.0⑫↑
        End If

        If (m_bFgDispGrp) Then
            dblAverage = dblAverageIT
        Else
            dblAverage = dblAverageFT
        End If
        'V4.0.0.0⑫↓
        gAverageValue.Text = dblAverage.ToString("0.000")     ' 平均値
        'V4.0.0.0⑫↑
        lScaleMax = 0                                           ' オートスケーリング
        lScale = 100
        Do
            If (lScale > lMax) Then                             ' lScale < 抵抗数 ?
                lScaleMax = lScale
            ElseIf ((lScale * 2) > lMax) Then
                lScaleMax = (lScale * 2)
            ElseIf ((lScale * 5) > lMax) Then
                lScaleMax = (lScale * 5)
            End If
            lScale = lScale * 10
        Loop While (0 = lScaleMax) And (MAX_SCALE_NUM > lScale)

        If (0 = lScaleMax) Then
            lScaleMax = MAX_SCALE_NUM + 1
        End If

        If (m_bFgDispGrp) Then
            If ((0 >= typResistorInfoArray(1).dblInitTest_LowLimit) And (0 <= typResistorInfoArray(1).dblInitTest_HighLimit)) Then
                dblGraphDiv = (typResistorInfoArray(1).dblInitTest_HighLimit * 1.5 - typResistorInfoArray(1).dblInitTest_LowLimit * 1.5) / 10
                dblGraphTop = typResistorInfoArray(1).dblInitTest_HighLimit * 1.5
            ElseIf ((0 >= typResistorInfoArray(1).dblInitTest_LowLimit) And (0 > typResistorInfoArray(1).dblInitTest_HighLimit)) Then
                dblGraphDiv = (typResistorInfoArray(1).dblInitTest_HighLimit / 1.5 - typResistorInfoArray(1).dblInitTest_LowLimit * 1.5) / 10
                dblGraphTop = typResistorInfoArray(1).dblInitTest_HighLimit * 1.5
            ElseIf ((0 < typResistorInfoArray(1).dblInitTest_LowLimit) And (0 <= typResistorInfoArray(1).dblInitTest_HighLimit)) Then
                dblGraphDiv = (typResistorInfoArray(1).dblInitTest_HighLimit * 1.5 - typResistorInfoArray(1).dblInitTest_LowLimit / 1.5) / 10
                dblGraphTop = typResistorInfoArray(1).dblInitTest_HighLimit * 1.5
            Else
                dblGraphDiv = 0.3
                dblGraphTop = 1.5
            End If
        Else
            If ((0 >= typResistorInfoArray(1).dblFinalTest_LowLimit) And (0 <= typResistorInfoArray(1).dblFinalTest_HighLimit)) Then
                dblGraphDiv = (typResistorInfoArray(1).dblFinalTest_HighLimit * 1.5 - typResistorInfoArray(1).dblFinalTest_LowLimit * 1.5) / 10
                dblGraphTop = typResistorInfoArray(1).dblFinalTest_HighLimit * 1.5
            ElseIf ((0 >= typResistorInfoArray(1).dblFinalTest_LowLimit) And (0 > typResistorInfoArray(1).dblFinalTest_HighLimit)) Then
                dblGraphDiv = (typResistorInfoArray(1).dblFinalTest_HighLimit / 1.5 - typResistorInfoArray(1).dblFinalTest_LowLimit * 1.5) / 10
                dblGraphTop = typResistorInfoArray(1).dblFinalTest_HighLimit * 1.5
            ElseIf ((0 < typResistorInfoArray(1).dblFinalTest_LowLimit) And (0 <= typResistorInfoArray(1).dblFinalTest_HighLimit)) Then
                dblGraphDiv = (typResistorInfoArray(1).dblFinalTest_HighLimit * 1.5 - typResistorInfoArray(1).dblFinalTest_LowLimit / 1.5) / 10
                dblGraphTop = typResistorInfoArray(1).dblFinalTest_HighLimit * 1.5
            Else
                dblGraphDiv = 0.3
                dblGraphTop = 1.5
            End If
        End If

        'V4.0.0.0⑫↓
        gDistGrpPerLblAry(0).Text = frmDistribution_005 & dblGraphTop.ToString("0.00")
        'V4.0.0.0⑫↑
        For iCnt = 1 To 11
            'gDistGrpPerLblAry(iCnt).Text = (dblGraphTop - (dblGraphDiv * (iCnt - 1)).ToString("0.00")) & "～"
            ' ###203 
            dtemp = (dblGraphTop - (dblGraphDiv * (iCnt - 1)))
            If ((-0.001 < dtemp) And (dtemp < 0.001)) Then
                'V4.0.0.0⑫↓
                gDistGrpPerLblAry(iCnt).Text = "0" & frmDistribution_005
                'V4.0.0.0⑫↑
            Else
                'V4.0.0.0⑫↓
                gDistGrpPerLblAry(iCnt).Text = (dtemp.ToString("0.00")) & frmDistribution_005
                'V4.0.0.0⑫↑
            End If
            ' ###203
        Next

        picGraphAccumulationDrawLine(lScaleMax)
        Call picGraphAccumulationPrintRegistNum()           ' 分布グラフに抵抗数を設定する

    End Sub
#End Region

#Region "分布図表示サブ"
    '''=========================================================================
    '''<summary>分布図表示サブ</summary>
    '''<param name="lScaleMax">(INP)スケール</param>
    '''=========================================================================
    Private Sub picGraphAccumulationDrawLine(ByRef lScaleMax As Integer)

        Dim i As Short
        Dim x As Short

        For i = 0 To (MAX_SCALE_RNUM - 1)
            '            x = CShort((glRegistNum(i) * 473) \ lScaleMax)   ' 分布グラフ抵抗数
            x = CShort((glRegistNum(i) * 250) \ lScaleMax)   ' 分布グラフ抵抗数
            'If (473 < x) Then
            If (250 < x) Then
                '                x = 473
                x = 250
            End If
            'V4.0.0.0⑫↓
            gDistShpGrpLblAry(i).Width = x
            'V4.0.0.0⑫↑
        Next
        gRegistUnit.Text = CStr(lScaleMax \ 2)            ' 抵抗数の半分の数 
    End Sub
#End Region

#Region "分布グラフに抵抗数を設定する"
    '''=========================================================================
    '''<summary>分布グラフに抵抗数を設定する</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub picGraphAccumulationPrintRegistNum()

        Dim i As Short
        'V4.0.0.0⑫
        For i = 0 To (MAX_SCALE_RNUM - 1)
            gDistRegNumLblAry(i).Text = CStr(glRegistNum(i))  ' 分布グラフ抵抗数
        Next
        'V4.0.0.0⑫

    End Sub
#End Region

    Private Sub picGraphAccumulation_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles picGraphAccumulation.Paint

    End Sub

#Region "イニシャル、ファイナルフラグの設定"
    '''=========================================================================
    ''' <summary>'V4.0.0.0⑫
    ''' イニシャル、ファイナルフラグの設定
    ''' </summary>
    ''' <param name="flg"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub SetDispGrp(ByVal flg As Boolean)
        m_bFgDispGrp = flg
    End Sub
#End Region

#Region "イニシャル、ファイナルフラグの取得"
    '''=========================================================================
    ''' <summary>'V4.0.0.0⑫
    ''' イニシャル、ファイナルフラグの取得
    ''' </summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Function GetDispGrp() As Boolean
        GetDispGrp = m_bFgDispGrp
    End Function

#End Region




    ''' <summary>
    ''' グラフを前面に設定する 'V6.0.2.0④
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ShowGraph()

        Try

            Me.TopMost = True
            Me.Visible = True
        Catch ex As Exception

        End Try

    End Sub


End Class