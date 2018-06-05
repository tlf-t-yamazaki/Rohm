'==============================================================================
'   Description : シンプルトリマ用モジュールファイル
'
'   V2.0.0.0⑩
'
'　 2014/06/20 First Written by N.Arata(OLFT) 
'
'==============================================================================

Imports LaserFront.Trimmer.DefTrimFnc
Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8①
Imports TKY_ALL_SL432HW.My.Resources    'V4.4.0.0-0

Module SimpleTrimmer
#Region "共通定義"

    Friend WithEvents DataClrButton As System.Windows.Forms.Button
    Friend WithEvents DataSaveButton As System.Windows.Forms.Button
    Friend WithEvents CmdBlockData As System.Windows.Forms.Button
    Friend WithEvents CmdLotNumber As System.Windows.Forms.Button
    Friend WithEvents CmdMainte As System.Windows.Forms.Button
    Friend WithEvents CmdProbeClean As System.Windows.Forms.Button
    Friend WithEvents BPAdjustButton As System.Windows.Forms.Button
    Friend WithEvents DataEditButton As System.Windows.Forms.Button     'V4.0.0.0-84

    Friend WithEvents CheckNGRate As System.Windows.Forms.CheckBox      'V4.8.0.1①


    'V4.0.0.0⑬
    Friend WithEvents BlockNextButton As System.Windows.Forms.Button
    Friend WithEvents BlockRvsButton As System.Windows.Forms.Button
    Friend WithEvents BlockNoLabel As System.Windows.Forms.Label
    Friend WithEvents BlockTrimModeLabel As System.Windows.Forms.Label
    'V4.0.0.0⑬
    'V4.1.0.0⑱
    Friend WithEvents NowBlockNoLabel As System.Windows.Forms.Label
    'V4.1.0.0⑱

    'V4.0.0.0-76
    Friend WithEvents BlockMainButton As System.Windows.Forms.Button

#If START_KEY_SOFT Then
    Friend WithEvents SoftStartButton As System.Windows.Forms.Button
    Friend WithEvents SoftResetButton As System.Windows.Forms.Button
#End If

    Public TrimData As TrimClassLibrary.TrimData
    Public TrimDef As TrimClassLibrary.TrimDef

    Private Structure StatisticsData
        Dim Label As TKY_ALL_SL432HW.CustomTkyLabel
    End Structure

    ' 統計データの表示の為の定義
    Private Const TKY_BORDER_WIDTH As Integer = 2                               ' 統計データ表示罫線太さ
    Private Const TKY_TITLE_SIZE As Integer = 100                               ' 統計データ見出しサイズ
    Private Const TKY_DATA_WIDTH As Integer = 480                               ' 統計データ全体幅
    Private Const TKY_DATA_HIGHT As Integer = 32                                ' 統計データ縦高さ
    Private Const TKY_DATA_X_OFF As Integer = -1                                ' 統計データ横方向シフト時のオフセット
    Private Const TKY_DATA_Y_OFF As Integer = -1                                ' 統計データ横方向シフト時のオフセット
    Private Const TKY_DATA_INTERVAL As Integer = 10                             ' 統計データブロック間インターバル
    Private TKY_LINE_COLOR As System.Drawing.Color = System.Drawing.Color.Black ' 統計データ表示罫線色
    Private StatData(83) As StatisticsData                                       ' ロット番号、開始時間、経過時間、他
    Private Const TKY_DATA_POS As Integer = 35                                  ' 統計データ表示開始位置（５加算すると次の行）
    Private Const TKY_DATA_COL As Integer = 5                                   ' 統計データ表示１行の数

    ' 抵抗データの表示の為の定義
    Private Const RES_BORDER_WIDTH As Integer = 2                               ' 抵抗データ表示罫線太さ
    ''V4.0.0.0⑬    Private Const RES_TITLE_SIZE As Integer = 50                                ' 抵抗データ見出しサイズ
    Private Const RES_TITLE_SIZE As Integer = 40                                ' 抵抗データ見出しサイズ
    Private Const RES_DATA_HIGHT As Integer = 18                                ' 抵抗データ縦高さ
    Private Const RES_DATA_X_OFF As Integer = -1                                ' 抵抗データ横方向シフト時のオフセット
    Private Const RES_DATA_Y_OFF As Integer = -1                                ' 抵抗データ横方向シフト時のオフセット
    Private Const RES_DATA_INTERVAL As Integer = 6                              ' 抵抗データブロック間インターバル
    Private Const RES_TOTAL_COUNTER As Integer = 50                             ' 一度に表示する抵抗数
    Private Const RES_DATA_START As Integer = 3                                 ' 抵抗データ表示の先頭位置
    Private Const RES_DATA_STAT As Integer = 153                                ' 統計データ表示の先頭位置
    Private RES_LINE_COLOR As System.Drawing.Color = System.Drawing.Color.Black ' 抵抗データ表示罫線色
    Private ResistorData(0) As StatisticsData                                   ' ロット番号、開始時間、経過時間、他
    Private HeaderButton(2) As System.Windows.Forms.Label                          ' ヘッダボタン

    '    Private Const BUTTON_INTERVAL As Integer = 50                               ' コマンドボタンのボタン間隔
    Private Const BUTTON_INTERVAL As Integer = 45                               ' コマンドボタンのボタン間隔

    Private bBlockdataDisp As Boolean = False                                   ' ブロックデータの表示中は、True
    Private ResistorDisplayNumber As Integer = 1                                ' 表示中の開始抵抗番号
    Private BlockDisplayNumber As Integer = 0                                   ' 表示中のブロック番号

    Private Enum StatType
        BLOCK_INITIAL
        BLOCK_FINAL
        PLATE_INITIAL
        PLATE_FINAL
        LOT_INITIAL
        LOT_FINAL
    End Enum

#End Region

#Region "シンプルトリマ画面初期化処理"
    '''=========================================================================
    ''' <summary>
    ''' シンプルトリマ画面初期化処理
    ''' </summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub SimpleTrimmerInit()

        Dim setLocation As System.Drawing.Point
        Dim setSize As System.Drawing.Size
        Dim Language As Integer = tkyIni.TMENU.MSGTYP.Get(Of Integer)()
        Dim Location_X As Integer, Location_Y As Integer
        Dim TmpX As Integer, TmpY As Integer, TmpWidth As Integer
        ' ↓↓↓ V3.1.0.0③ 2014/12/02
        'Dim ObjTkyMsg As DllTkyMsgGet.TkyMsgGet                         ' TKY用メッセージファイルリードオブジェクト(DllTkyMsgGet.dll) 
        Dim strBTN(MAX_FNCNO + 1) As String                             ' ボタン名の配列(0 ORG)
        'Dim Count As Integer
        ' ↑↑↑ V3.1.0.0③ 2014/12/02

        Try

            If gKeiTyp <> KEY_TYPE_RS Then
                Call Form1.SetDataDisplayOff()
                Form1.PanelGraphOnOff(False)                        ' V4.0.0.0⑫
                Exit Sub
            End If

            ReDim StatData(83)
            ReDim ResistorData(167)

            ' ↓↓↓ V3.1.0.0③ 2014/12/02
            'ObjTkyMsg = New DllTkyMsgGet.TkyMsgGet                      ' TKY用メッセージファイルリードオブジェクト生成 
            'Count = ObjTkyMsg.Get_Button_Name(gSysPrm.stTMN.giMsgTyp, strBTN)  ' V4.4.0.0-0
            ' ↑↑↑ V3.1.0.0③ 2014/12/02

            'V4.0.0.0⑫ Form1.chkDistributeOnOff.Checked = True     ' 生産グラフ　表示
#If False Then              'V6.0.0.0④
            ' クロスラインサイズ変更
            ' 水平方向
            setSize = Form1.VideoLibrary1.Size
            setSize.Height = Form1.Picture1.Height
            Form1.Picture1.Size = setSize
            Form1.Picture1.Location = New Point(Form1.VideoLibrary1.Location.X, Form1.VideoLibrary1.Location.Y + Form1.VideoLibrary1.Size.Width / 2)
            ' 垂直方向
            setSize = Form1.VideoLibrary1.Size
            setSize.Width = Form1.Picture2.Width
            Form1.Picture2.Size = setSize
            Form1.Picture2.Location = New Point(Form1.VideoLibrary1.Location.X + Form1.VideoLibrary1.Size.Width / 2, Form1.VideoLibrary1.Location.Y)
#End If
            ' 生産グラフ、位置、サイズ変更
            'V4.0.0.0⑫↓
            Form1.PanelGraphOnOff(True)                        ' V4.0.0.0⑫
            Form1.PanelGraph.Top = Form1.VideoLibrary1.Top + SIMPLE_PICTURE_SIZEY + 5
            Form1.PanelGraph.Left = Form1.VideoLibrary1.Left
            InitializeFormGraphPanel()
            'Form1.cmdGraphSave.Text = PIC_TRIM_10
            'Form1.cmdInitial.Text = PIC_TRIM_01
            'Form1.cmdFinal.Text = PIC_TRIM_02
            frmDistribution.Visible = False
            ' V4.0.0.0⑫↑

            ' >>> V3.1.0.0① 2014/11/28
            'Location_X = setLocation.X + gObjFrmDistribute.Width + 10     ' 統計データ表示開始Ｘ座標
            'Location_Y = setLocation.Y
            Location_X = 0
            Location_Y = 0
            ' <<< V3.1.0.0① 2014/11/28
            '            gObjFrmDistribute.Size = setSize

            ' ↓↓↓ V3.1.0.0② 2014/12/01
            ' ログ表示の位置を被らないように下に移動する
            'Form1.txtLog.Location = New Point(setLocation.X, gObjFrmDistribute.Location.Y + gObjFrmDistribute.Height + 10)
            ' ↓↓↓ V3.1.0.0④ 2014/12/05
            'Form1.txtLog.Location = New Point(setLocation.X, gObjFrmDistribute.Location.Y + gObjFrmDistribute.Height + 31)
            '            Form1.txtLog.Location = New Point(setLocation.X, gObjFrmDistribute.Location.Y + gObjFrmDistribute.Height + 11)
            Form1.txtLog.Location = New Point(setLocation.X, Form1.PanelGraph.Location.Y + Form1.PanelGraph.Height + 11)
            ' ↑↑↑ V3.1.0.0④ 2014/12/05
            ' ↑↑↑ V3.1.0.0② 2014/12/01
            setSize = Form1.txtLog.Size
            setSize.Height = Form1.Size.Height - Form1.txtLog.Location.Y
            Form1.txtLog.Size = setSize

            ' 統計データ非表示
            Form1.frmHistoryData.Visible = False

            ' 生産グラフ　表示、非表示ボタンオフ
            Form1.chkDistributeOnOff.Enabled = False
            Form1.chkDistributeOnOff.Visible = False

            ' ボタン表示タブの表示位置変更
            Call GetResDataArea(TmpX, TmpY, TmpWidth)
            Form1.tabCmd.Location = New Point(TmpX, TmpY)
            setSize.Height = Form1.txtLog.Location.Y
            setSize.Width = TmpWidth
            Form1.tabCmd.Size = setSize

            TmpX = (TmpWidth - Form1.CmdLoad.Size.Width) / 2
            TmpY = BUTTON_INTERVAL / 2
            Form1.CmdLoad.Location = New Point(TmpX, TmpY)
            TmpY = TmpY + BUTTON_INTERVAL
            Form1.CmdEdit.Location = New Point(TmpX, TmpY)
            TmpY = TmpY + BUTTON_INTERVAL
            Form1.CmdPattern.Location = New Point(TmpX, TmpY)
            TmpY = TmpY + BUTTON_INTERVAL
            Form1.CmdProbe.Location = New Point(TmpX, TmpY)
            TmpY = TmpY + BUTTON_INTERVAL
            Form1.CmdTx.Location = New Point(TmpX, TmpY)
            TmpY = TmpY + BUTTON_INTERVAL
            Form1.CmdTy.Location = New Point(TmpX, TmpY)
            TmpY = TmpY + BUTTON_INTERVAL
            Form1.CmdTeach.Location = New Point(TmpX, TmpY)
            TmpY = TmpY + BUTTON_INTERVAL
            'V4.10.0.0⑫                 ↓   統合登録・調整ﾎﾞﾀﾝ
            Form1.CmdIntegrated.Location = New Point(TmpX, TmpY)
            TmpY = TmpY + BUTTON_INTERVAL
            'V4.10.0.0⑫                 ↑
            Form1.CmdLoging.Location = New Point(TmpX, TmpY)
            TmpY = TmpY + BUTTON_INTERVAL
            Form1.CmdLaser.Location = New Point(TmpX, TmpY)
            TmpY = TmpY + BUTTON_INTERVAL
            Form1.CmdSave.Location = New Point(TmpX, TmpY)

            ' ロット番号登録ボタン作成処理
            CmdLotNumber = New System.Windows.Forms.Button
            Form1.tabBaseCmnds.Controls.Add(CmdLotNumber)
            CmdLotNumber.BackColor = System.Drawing.Color.FromArgb(192, 255, 255)
            CmdLotNumber.Size = New System.Drawing.Size(145, 30)
            CmdLotNumber.ForeColor = System.Drawing.Color.Black
            CmdLotNumber.Name = "CmdLoad"
            CmdLotNumber.TabIndex = Form1.CmdSave.TabIndex + 1
            CmdLotNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            ' ↓↓↓ V3.1.0.0 2014/12/02
            'CmdLotNumber.Text = "ロット番号"
            'CmdLotNumber.Text = strBTN(F_LOT)
            CmdLotNumber.Text = SimpleTrimmer_036
            ' ↑↑↑ V3.1.0.0 2014/12/02
            TmpY = TmpY + BUTTON_INTERVAL
            CmdLotNumber.Location = New Point(TmpX, TmpY)

            ' 自動運転
            TmpY = TmpY + BUTTON_INTERVAL
            Form1.CmdAutoOperation.Location = New Point(TmpX, TmpY)

            ' ローダ初期化
            TmpY = TmpY + BUTTON_INTERVAL
            Form1.CmdLoaderInit.Location = New Point(TmpX, TmpY)

            ' メンテナンス・ボタン作成処理
            CmdMainte = New System.Windows.Forms.Button
            Form1.tabBaseCmnds.Controls.Add(CmdMainte)
            CmdMainte.BackColor = System.Drawing.Color.FromArgb(192, 255, 255)
            CmdMainte.Size = New System.Drawing.Size(145, 30)
            CmdMainte.ForeColor = System.Drawing.Color.Black
            CmdMainte.Name = "CmdMainte"
            CmdMainte.TabIndex = Form1.CmdSave.TabIndex + 1
            CmdMainte.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            ' ↓↓↓ V3.1.0.0 2014/12/02
            'CmdMainte.Text = "メンテナンス"
            'CmdMainte.Text = strBTN(F_MAINTENANCE)
            CmdMainte.Text = SimpleTrimmer_037
            ' ↑↑↑ V3.1.0.0 2014/12/02
            TmpY = TmpY + BUTTON_INTERVAL
            CmdMainte.Location = New Point(TmpX, TmpY)

            ' プローブクリーニング・ボタン作成処理
            CmdProbeClean = New System.Windows.Forms.Button
            Form1.tabBaseCmnds.Controls.Add(CmdProbeClean)
            CmdProbeClean.BackColor = System.Drawing.Color.FromArgb(192, 255, 255)
            CmdProbeClean.Size = New System.Drawing.Size(160, 30)
            CmdProbeClean.ForeColor = System.Drawing.Color.Black
            CmdProbeClean.Name = "CmdProbeClean"
            CmdProbeClean.TabIndex = Form1.CmdSave.TabIndex + 1
            CmdProbeClean.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            ' ↓↓↓ V3.1.0.0 2014/12/02
            'CmdProbeClean.Text = "プローブクリーニング"
            'CmdProbeClean.Text = strBTN(F_PROBE_CLEANING)
            CmdProbeClean.Text = SimpleTrimmer_038
            ' ↑↑↑ V3.1.0.0 2014/12/02
            TmpY = TmpY + BUTTON_INTERVAL
            CmdProbeClean.Location = New Point(TmpX, TmpY)

            Form1.Controls.Remove(Form1.CmdEnd)
            Form1.tabBaseCmnds.Controls.Add(Form1.CmdEnd)
            TmpY = TmpY + BUTTON_INTERVAL
            Form1.CmdEnd.Location = New Point(TmpX, TmpY)

            ' ブロックデータ表示ボタン［BLOCK DATA］作成処理
            CmdBlockData = New System.Windows.Forms.Button
            Form1.tabBaseCmnds.Controls.Add(CmdBlockData)
            CmdBlockData.BackColor = System.Drawing.SystemColors.Control
            CmdBlockData.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
            CmdBlockData.Cursor = System.Windows.Forms.Cursors.Default
            CmdBlockData.FlatAppearance.BorderColor = System.Drawing.Color.GhostWhite
            CmdBlockData.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            CmdBlockData.ForeColor = System.Drawing.SystemColors.ControlText
            CmdBlockData.Location = New System.Drawing.Point(455, 116)
            CmdBlockData.Name = "cmdBlockData"
            CmdBlockData.RightToLeft = System.Windows.Forms.RightToLeft.No
            CmdBlockData.Size = New System.Drawing.Size(145, 30)
            CmdBlockData.TabIndex = 240
            'If Language = 0 Then 'V4.1.0.0④
            '    CmdBlockData.Text = "ブロックデータ"
            'Else
            '    CmdBlockData.Text = "BLOCK DATA"
            'End If
            CmdBlockData.Text = SimpleTrimmer_002
            CmdBlockData.UseVisualStyleBackColor = False
            CmdBlockData.Enabled = True
            CmdBlockData.Visible = False
            TmpY = TmpY + BUTTON_INTERVAL
            CmdBlockData.Location = New Point(TmpX, TmpY)

            'V4.8.0.1① ↓
            ' チェックボックス処理
            CheckNGRate = New System.Windows.Forms.CheckBox
            Form1.Controls.Add(CheckNGRate)
            CheckNGRate.BackColor = System.Drawing.Color.FromArgb(192, 255, 255)
            CheckNGRate.Size = New System.Drawing.Size(145, 30)
            If Language = 0 Then 'V4.1.0.0④
                CheckNGRate.Text = "不良率"
            Else
                CheckNGRate.Text = "NG Rate"
            End If
            CheckNGRate.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            TmpX = 450
            TmpY = 20
            CheckNGRate.Location = New Point(TmpX, TmpY)
            CheckNGRate.ForeColor = System.Drawing.SystemColors.ControlText
            CheckNGRate.BackColor = System.Drawing.SystemColors.Control
            CheckNGRate.Visible = True
            'V4.8.0.1① ↑

            '----- V6.0.3.0_40 ↓-----
            ' 不良率非表示
            If (giRateDisp = 0) Then
                CheckNGRate.Visible = False
            End If
            '----- V6.0.3.0_40 ↑-----

            ' ↓↓↓ V3.1.0.0 2014/12/02
            ' 後処理
            'ObjTkyMsg = Nothing                                         ' オブジェクト開放
            ' ↑↑↑ V3.1.0.0 2014/12/02

            '-------------------------------------------------------------------------------
            ' 画面中央のデータ表示領域の表示処理
            '-------------------------------------------------------------------------------
            Dim DataWidth1 As Integer = (TKY_DATA_WIDTH - TKY_TITLE_SIZE)
            Dim DataWidth2 As Integer = (TKY_DATA_WIDTH - TKY_TITLE_SIZE) / 2
            Dim DataWidth4 As Integer = (TKY_DATA_WIDTH - TKY_TITLE_SIZE) / 4

            ' >>> V3.1.0.0① 2014/11/28
            ' パネルの高さと幅
            Form1.pnlDataDisplay.Size = New Size(TKY_DATA_WIDTH, (TKY_DATA_HIGHT * (7 + 4 + 12)) + (TKY_DATA_INTERVAL * (1 + 1)) - TKY_DATA_HIGHT / 2)
            ' <<< V3.1.0.0① 2014/11/28

            For Cnt As Integer = 0 To UBound(StatData)
                StatData(Cnt).Label = New TKY_ALL_SL432HW.CustomTkyLabel
                StatData(Cnt).Label.AutoSize = False
                StatData(Cnt).Label.BorderColor = TKY_LINE_COLOR
                StatData(Cnt).Label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
                StatData(Cnt).Label.BorderWidth = TKY_BORDER_WIDTH
                StatData(Cnt).Label.Name = "CustomTkyLabel1"
                StatData(Cnt).Label.Text = ""
                StatData(Cnt).Label.Padding = New System.Windows.Forms.Padding(0)
                StatData(Cnt).Label.TabIndex = 361 + Cnt
                ''V4.0.0.0⑬                StatData(Cnt).Label.Font = New System.Drawing.Font("ＭＳ ゴシック", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
                StatData(Cnt).Label.Font = New System.Drawing.Font("MS UI Gothic", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
                StatData(Cnt).Label.ForeColor = System.Drawing.Color.Black
                StatData(Cnt).Label.BackColor = System.Drawing.SystemColors.Window
                StatData(Cnt).Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter

                Select Case (Cnt)
                    ' 歩留まり表示
                    Case 0
                        StatData(Cnt).Label.Location = New System.Drawing.Point(Location_X, Location_Y)
                        ''V4.1.0.0⑱                        StatData(Cnt).Label.Size = New System.Drawing.Size(TKY_DATA_WIDTH, TKY_DATA_HIGHT)
                        StatData(Cnt).Label.Size = New System.Drawing.Size((TKY_DATA_WIDTH / 2), TKY_DATA_HIGHT) ''V4.1.0.0⑱
                        StatData(Cnt).Label.BorderStyle = System.Windows.Forms.BorderStyle.None
                        StatData(Cnt).Label.BackColor = System.Drawing.SystemColors.Control
                        StatData(Cnt).Label.Padding = New System.Windows.Forms.Padding(10, 0, 0, 0)
                        StatData(Cnt).Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
                        'StatData(Cnt).Label.Font = New System.Drawing.Font("ＭＳ ゴシック", 16.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
                        StatData(Cnt).Label.Font = New System.Drawing.Font(SimpleTrimmer_003, 16.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
                        Location_Y = Location_Y + TKY_DATA_HIGHT
                        StatData(Cnt).Label.Visible = True
                        ' 左のタイトル
                    Case 1, 3, 5, 7, 10, 13, 15, 17, 20, 23, 26, 29, 34, 39, 44, 49, 54, 59, 64, 69, 74, 79
                        If Cnt = 15 Or Cnt = 26 Then
                            Location_Y = Location_Y + TKY_DATA_INTERVAL
                        End If
                        StatData(Cnt).Label.Location = New System.Drawing.Point(Location_X, Location_Y)
                        StatData(Cnt).Label.Size = New System.Drawing.Size(TKY_TITLE_SIZE, TKY_DATA_HIGHT)
                        'StatData(Cnt).Label.Font = New System.Drawing.Font("ＭＳ ゴシック", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
                        StatData(Cnt).Label.Font = New System.Drawing.Font(SimpleTrimmer_003, 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
                        StatData(Cnt).Label.BackColor = Color.LightGreen
                        ' １列のデータ
                    Case 2, 4, 6, 14, 16
                        StatData(Cnt).Label.Location = New System.Drawing.Point(Location_X + TKY_TITLE_SIZE + TKY_DATA_X_OFF, Location_Y)
                        StatData(Cnt).Label.Size = New System.Drawing.Size(DataWidth1 + TKY_DATA_X_OFF * 3, TKY_DATA_HIGHT)
                        Location_Y = Location_Y + TKY_DATA_HIGHT + TKY_DATA_Y_OFF
                        StatData(Cnt).Label.BackColor = Color.GreenYellow
                        StatData(Cnt).Label.BackColor = Color.LemonChiffon
                        ' ２列のデータの１列目
                    Case 8, 11, 18, 21, 24, 27
                        StatData(Cnt).Label.Location = New System.Drawing.Point(Location_X + TKY_TITLE_SIZE + TKY_DATA_X_OFF, Location_Y)
                        StatData(Cnt).Label.Size = New System.Drawing.Size(DataWidth2 + TKY_DATA_X_OFF, TKY_DATA_HIGHT)
                        StatData(Cnt).Label.BackColor = Color.LemonChiffon
                        ' ２列のデータの２列目
                    Case 9, 12, 19, 22, 25, 28
                        StatData(Cnt).Label.Location = New System.Drawing.Point(Location_X + TKY_TITLE_SIZE + TKY_DATA_X_OFF * 3 + DataWidth2, Location_Y)
                        StatData(Cnt).Label.Size = New System.Drawing.Size(DataWidth2 + TKY_DATA_X_OFF, TKY_DATA_HIGHT)
                        Location_Y = Location_Y + TKY_DATA_HIGHT + TKY_DATA_Y_OFF
                        StatData(Cnt).Label.BackColor = Color.LemonChiffon
                        ' ４列のデータの１列目
                    Case 30, 35, 40, 45, 50, 55, 60, 65, 70, 75, 80
                        StatData(Cnt).Label.Location = New System.Drawing.Point(Location_X + TKY_TITLE_SIZE + TKY_DATA_X_OFF, Location_Y)
                        StatData(Cnt).Label.Size = New System.Drawing.Size(DataWidth4, TKY_DATA_HIGHT)
                        StatData(Cnt).Label.BackColor = Color.LemonChiffon
                        ' ４列のデータの２列目
                    Case 31, 36, 41, 46, 51, 56, 61, 66, 71, 76, 81
                        StatData(Cnt).Label.Location = New System.Drawing.Point(Location_X + TKY_TITLE_SIZE + TKY_DATA_X_OFF * 2 + DataWidth4, Location_Y)
                        StatData(Cnt).Label.Size = New System.Drawing.Size(DataWidth4, TKY_DATA_HIGHT)
                        StatData(Cnt).Label.BackColor = Color.LemonChiffon
                        ' ４列のデータの３列目
                    Case 32, 37, 42, 47, 52, 57, 62, 67, 72, 77, 82
                        StatData(Cnt).Label.Location = New System.Drawing.Point(Location_X + TKY_TITLE_SIZE + TKY_DATA_X_OFF * 3 + DataWidth4 * 2, Location_Y)
                        StatData(Cnt).Label.Size = New System.Drawing.Size(DataWidth4, TKY_DATA_HIGHT)
                        StatData(Cnt).Label.BackColor = Color.LemonChiffon
                        ' ４列のデータの４列目
                    Case 33, 38, 43, 48, 53, 58, 63, 68, 73, 78, 83
                        StatData(Cnt).Label.Location = New System.Drawing.Point(Location_X + TKY_TITLE_SIZE + TKY_DATA_X_OFF * 4 + DataWidth4 * 3, Location_Y)
                        StatData(Cnt).Label.Size = New System.Drawing.Size(DataWidth4, TKY_DATA_HIGHT)
                        Location_Y = Location_Y + TKY_DATA_HIGHT + TKY_DATA_Y_OFF
                        StatData(Cnt).Label.BackColor = Color.LemonChiffon
                End Select

                ' >>> V3.1.0.0① 2014/11/28
                ' メインからパネルへ追加するように変更
                'Form1.Controls.Add(StatData(Cnt).Label)
                Form1.pnlDataDisplay.Controls.Add(StatData(Cnt).Label)
                'Form1.pnlDataDisplay.Left = 440    'V4.10.0.0⑫
                ' <<< V3.1.0.0① 2014/11/28
            Next
            Form1.pnlDataDisplay.Left = 438         'V4.10.0.0⑫ tabCmdの左端が隠れるので調整

            'V5.0.0.9⑯                  ↓
            With Form1.GrpStartBlk
                .Location = Point.Subtract(
                    Form1.pnlDataDisplay.PointToScreen(New Point(StatData(2).Label.Right, StatData(2).Label.Top)),
                    .Size)
            End With
            'V5.0.0.9⑯                  ↑

            ' ロット番号、開始時間、経過時間、他
            StatData(0).Label.Text = SimpleTrimmer_004 ' 歩留まり
            StatData(1).Label.Text = SimpleTrimmer_005 ' ロット番号
            StatData(3).Label.Text = SimpleTrimmer_006 ' 開始時間
            StatData(5).Label.Text = SimpleTrimmer_007 ' 経過時間
            StatData(7).Label.Text = SimpleTrimmer_008 ' 処理基板数
            StatData(10).Label.Text = SimpleTrimmer_009 ' 処理抵抗数
            StatData(13).Label.Text = SimpleTrimmer_010 ' レーザパワー

            StatData(15).Label.Text = SimpleTrimmer_011 ' 目標値
            StatData(18).Label.Text = SimpleTrimmer_012 ' 下限
            StatData(19).Label.Text = SimpleTrimmer_013 ' 上限
            StatData(20).Label.Text = SimpleTrimmer_014 ' 初期測定
            StatData(23).Label.Text = SimpleTrimmer_015 ' 最終測定

            StatData(27).Label.Text = SimpleTrimmer_016 ' 基板
            StatData(28).Label.Text = SimpleTrimmer_017 ' ロット
            StatData(30).Label.Text = SimpleTrimmer_014 ' 初期測定
            StatData(31).Label.Text = SimpleTrimmer_015 ' 最終測定
            StatData(32).Label.Text = SimpleTrimmer_014 ' 初期測定
            StatData(33).Label.Text = SimpleTrimmer_015 ' 最終測定

            StatData(34).Label.Text = SimpleTrimmer_018 ' 処理数
            StatData(39).Label.Text = SimpleTrimmer_019 ' ＯＫ(%)
            StatData(44).Label.Text = SimpleTrimmer_020 ' Low NG(%)
            StatData(49).Label.Text = SimpleTrimmer_021 ' High NG(%)
            StatData(54).Label.Text = SimpleTrimmer_022 ' Range Over(%)
            StatData(59).Label.Text = SimpleTrimmer_023 ' 最小値
            StatData(64).Label.Text = SimpleTrimmer_024 ' 最大値
            StatData(69).Label.Text = SimpleTrimmer_025 ' 平均値
            StatData(74).Label.Text = SimpleTrimmer_026 ' ３σ
            StatData(79).Label.Text = SimpleTrimmer_027 ' ＣｐＫ
            If giCpk_Disp_Off Then                      'V5.0.0.4④
                StatData(79).Label.Text = "" ' ＣｐＫ    V5.0.0.4④
            End If                                      'V5.0.0.4④
            'If Language = 0 Then
            '    StatData(0).Label.Text = "歩留まり"
            '    StatData(1).Label.Text = "ロット番号"
            '    StatData(3).Label.Text = "開始時間"
            '    StatData(5).Label.Text = "経過時間"
            '    StatData(7).Label.Text = "処理基板数"
            '    StatData(10).Label.Text = "処理抵抗数"
            '    StatData(13).Label.Text = "レーザパワー"

            '    StatData(15).Label.Text = "目標値"
            '    StatData(18).Label.Text = "下限"
            '    StatData(19).Label.Text = "上限"
            '    StatData(20).Label.Text = "初期測定"
            '    StatData(23).Label.Text = "最終測定"

            '    StatData(27).Label.Text = "基板"
            '    StatData(28).Label.Text = "ロット"
            '    StatData(30).Label.Text = "初期測定"
            '    StatData(31).Label.Text = "最終測定"
            '    StatData(32).Label.Text = "初期測定"
            '    StatData(33).Label.Text = "最終測定"

            '    StatData(34).Label.Text = "処理数"
            '    StatData(39).Label.Text = "ＯＫ(%)"
            '    StatData(44).Label.Text = "Low NG(%)"
            '    StatData(49).Label.Text = "High NG(%)"
            '    StatData(54).Label.Text = "Range Over(%)"
            '    StatData(59).Label.Text = "最小値"
            '    StatData(64).Label.Text = "最大値"
            '    StatData(69).Label.Text = "平均値"
            '    StatData(74).Label.Text = "３σ"
            '    StatData(79).Label.Text = "ＣｐＫ"
            'Else
            '    StatData(0).Label.Text = "YIELD"
            '    StatData(1).Label.Text = "LOT NO"
            '    StatData(3).Label.Text = "START TIME"
            '    StatData(5).Label.Text = "ELAPSED TIME"
            '    StatData(7).Label.Text = "Substrates"
            '    StatData(10).Label.Text = "Resistors"
            '    StatData(13).Label.Text = "Laser Power"

            '    StatData(15).Label.Text = "Nominal"
            '    StatData(18).Label.Text = "Min"
            '    StatData(19).Label.Text = "Max"
            '    StatData(20).Label.Text = "Initial Test"
            '    StatData(23).Label.Text = "Final Test"

            '    StatData(27).Label.Text = "Substrate"
            '    StatData(28).Label.Text = "Lot"
            '    StatData(30).Label.Text = "Initial Test"
            '    StatData(31).Label.Text = "Final Test"
            '    StatData(32).Label.Text = "Initial Test"
            '    StatData(33).Label.Text = "Final Test"

            '    StatData(34).Label.Text = "Resistors"
            '    StatData(39).Label.Text = "OK(%)"
            '    StatData(44).Label.Text = "Low NG(%)"
            '    StatData(49).Label.Text = "High NG(%)"
            '    StatData(54).Label.Text = "Range Over(%)"
            '    StatData(59).Label.Text = "Min"
            '    StatData(64).Label.Text = "Max"
            '    StatData(69).Label.Text = "Average"
            '    StatData(74).Label.Text = "3σ"
            '    StatData(79).Label.Text = "CpK"
            'End If
            '-------------------------------------------------------------------------------
            ' 画面中央のデータ表示領域の表示処理の終了
            '-------------------------------------------------------------------------------

            ' >>> V3.1.0.0① 2014/11/28
            Location_X = setLocation.X + Form1.PanelGraph.Width + 10     ' 統計データ表示開始Ｘ座標
            Location_Y = 788
            ' <<< V3.1.0.0① 2014/11/28

            ' コマンドボタン表示サイズと位置の変更
            Form1.GrpMode.Location = New Point(Location_X, Location_Y)
            '            setSize.Height = Form1.txtLog.Location.Y - Location_Y - 6
            ' ↓↓↓ V3.1.0.0② 2014/12/01
            'setSize.Height = Form1.txtLog.Location.Y - Location_Y + 20
            setSize.Height = Form1.txtLog.Location.Y - Location_Y - 1
            ' ↑↑↑ V3.1.0.0② 2014/12/01
            setSize.Width = TKY_DATA_WIDTH
            Form1.GrpMode.Size = setSize

            ' [DATA CLR]と[DATA SAVE]ボタンの表示 
            DataClrButton = New System.Windows.Forms.Button
            DataSaveButton = New System.Windows.Forms.Button
            BPAdjustButton = New System.Windows.Forms.Button
            DataEditButton = New System.Windows.Forms.Button            'V4.0.0.0-84
            Form1.GrpMode.Controls.Add(DataClrButton)
            Form1.GrpMode.Controls.Add(DataSaveButton)
            Form1.GrpMode.Controls.Add(BPAdjustButton)
            Form1.GrpMode.Controls.Add(DataEditButton)
            ' SimpleTrimmer_003 = ＭＳ ゴシック
            DataClrButton.Font = New System.Drawing.Font(SimpleTrimmer_003, 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            DataSaveButton.Font = New System.Drawing.Font(SimpleTrimmer_003, 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            BPAdjustButton.Font = New System.Drawing.Font(SimpleTrimmer_003, 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            DataEditButton.Font = New System.Drawing.Font(SimpleTrimmer_003, 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte)) 'V4.0.0.0-84
            DataClrButton.TextAlign = ContentAlignment.MiddleCenter
            DataSaveButton.TextAlign = ContentAlignment.MiddleCenter
            BPAdjustButton.TextAlign = ContentAlignment.MiddleCenter
            DataEditButton.TextAlign = ContentAlignment.MiddleCenter 'V4.0.0.0-84
            'If Language = 0 Then 'V4.1.0.0④
            '    DataClrButton.Text = "データクリア"
            '    DataSaveButton.Text = "データセーブ"
            '    BPAdjustButton.Text = "BP 調整"
            '    DataEditButton.Text = "データ編集" 'V4.0.0.0-84
            'Else
            '    DataClrButton.Text = "DATA CLR"
            '    DataSaveButton.Text = "DATA SAVE"
            '    BPAdjustButton.Text = "BP ADJUST"
            '    DataEditButton.Text = "DATA EDIT" 'V4.0.0.0-84
            'End If
            DataClrButton.Text = SimpleTrimmer_028 ' データクリア
            DataSaveButton.Text = SimpleTrimmer_029 ' データセーブ
            BPAdjustButton.Text = SimpleTrimmer_030 ' BP 調整
            DataEditButton.Text = SimpleTrimmer_031 ' データ編集 'V4.0.0.0-84

            DataClrButton.Size = New System.Drawing.Size(143, 29)
            DataSaveButton.Size = New System.Drawing.Size(143, 29)
            BPAdjustButton.Size = New System.Drawing.Size(143, 29)
            DataEditButton.Size = New System.Drawing.Size(143, 29) 'V4.0.0.0-84
            DataClrButton.Location = New Point(Form1.LblDIGSW_HI.Location.X, Form1.LblDIGSW_HI.Location.Y + (Form1.LblDIGSW.Location.Y - Form1.LblDIGSW_HI.Location.Y) * 2)
            DataSaveButton.Location = New Point(DataClrButton.Location.X + DataClrButton.Size.Width + 12, DataClrButton.Location.Y)
            BPAdjustButton.Location = New Point(DataClrButton.Location.X + DataClrButton.Size.Width + 12, DataClrButton.Location.Y + 30)
            DataEditButton.Location = New Point(DataClrButton.Location.X, DataClrButton.Location.Y + 30) 'V4.0.0.0-84

            Form1.BtnADJ.Location = New Point(DataSaveButton.Location.X + DataSaveButton.Size.Width + 12, DataClrButton.Location.Y)
            BPAdjustButton.Visible = False
            DataEditButton.Visible = False 'V4.0.0.0-84
            Form1.btnCycleStop.Location = New Point(Form1.BtnADJ.Location.X, Form1.BtnADJ.Location.Y + Form1.BtnADJ.Size.Height)   'V5.0.0.4①

            '----- V4.11.0.0⑥↓ (WALSIN殿SL436S対応) -----
            ' 「基板投入ボタン」表示位置設定
            TmpX = Form1.BtnADJ.Location.X
            TmpY = Form1.BtnADJ.Location.Y + Form1.BtnADJ.Size.Height + 2
            Form1.BtnSubstrateSet.Location = New Point(TmpX, TmpY)
            '----- V4.11.0.0⑥↑ -----

            ' マガジンUP/Down機能の位置修正
            Form1.GroupBox1.Left = Form1.tabCmd.Left
            Form1.GroupBox1.Top = Form1.tabCmd.Top + Form1.tabCmd.Height + 5
            Form1.txtLog.Width = Form1.txtLog.Width - 200

            ' QRコード/バーコード情報表示位置設定
            'Form1.GrpQrCode.Top = Form1.GrpMode.Top + Form1.GrpMode.Height + 10
            'Form1.GrpQrCode.Left = StatData(0).Label.Left
            Form1.GrpQrCode.Top = Form1.GrpMode.Top + Form1.GrpMode.Height
            Form1.GrpQrCode.Left = Form1.GrpMode.Left

            '----- V4.11.0.0②↓ (WALSIN殿SL436S対応) -----
            ' バーコード情報サイズ設定
            'V5.0.0.9⑮            If (gSysPrm.stCTM.giSPECIAL = customWALSIN) Then
            If (BarcodeType.Walsin = BarCode_Data.Type) Then            'V5.0.0.9⑮
                Form1.GrpQrCode.Size = New Size(220, 50)
                Form1.lblQRData.Size = New Size(210, 32)
            End If
            '----- V4.11.0.0②↑ -----

            'TrimData = New TrimClassLibrary.TrimData()  'V4.3.0.0②

            'V4.0.0.0⑬
            Dim dspOfs As Integer = -2
            ' 測定データ表示用ブロック番号の前後表示切替ボタンの表示 
            ' 次ブロック移動ボタンの追加
            BlockNextButton = New System.Windows.Forms.Button
            Form1.Controls.Add(BlockNextButton)
            BlockNextButton.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            BlockNextButton.TextAlign = ContentAlignment.MiddleCenter
            BlockNextButton.Text = "Next"

            BlockNextButton.Size = New System.Drawing.Size(70, 25)
            BlockNextButton.Location = New Point(Form1.LblRotAtt.Location.X + Form1.LblRotAtt.Width + 10, Form1.LblRotAtt.Location.Y + dspOfs)
            BlockNextButton.Visible = False

            ' 測定データ表示用ブロック番号ラベルの追加
            BlockNoLabel = New System.Windows.Forms.Label
            Form1.Controls.Add(BlockNoLabel)
            BlockNoLabel.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            BlockNoLabel.TextAlign = ContentAlignment.MiddleCenter
            BlockNoLabel.Text = ""
            BlockNoLabel.Size = New System.Drawing.Size(40, 25)
            'V5.0.0.9⑯            BlockNoLabel.Location = New Point(BlockNextButton.Left + BlockNextButton.Width, Form1.LblRotAtt.Location.Y)
            BlockNoLabel.Location = New Point(BlockNextButton.Left + BlockNextButton.Width, BlockNextButton.Top) 'V5.0.0.9⑯
            BlockNoLabel.Visible = False

            ' 前ブロック移動ボタンの追加
            BlockRvsButton = New System.Windows.Forms.Button
            Form1.Controls.Add(BlockRvsButton)
            BlockRvsButton.TextAlign = ContentAlignment.MiddleCenter
            BlockRvsButton.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            BlockRvsButton.Text = "Prev"
            BlockRvsButton.Size = New System.Drawing.Size(70, 25)
            'V5.0.0.9⑯            BlockRvsButton.Location = New Point(BlockNoLabel.Location.X + BlockNoLabel.Size.Width + 5, BlockNoLabel.Location.Y + dspOfs)
            BlockRvsButton.Location = New Point(BlockNoLabel.Location.X + BlockNoLabel.Size.Width + 5, BlockNextButton.Top) 'V5.0.0.9⑯
            BlockRvsButton.Visible = False

            'V4.0.0.0-76
            ' 前ブロック移動ボタンの追加
            BlockMainButton = New System.Windows.Forms.Button
            Form1.Controls.Add(BlockMainButton)
            BlockMainButton.TextAlign = ContentAlignment.MiddleCenter
            BlockMainButton.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            'If Language = 0 Then 'V4.1.0.0④
            '    BlockMainButton.Text = "メイン"
            'Else
            '    BlockMainButton.Text = "Main"
            'End If
            BlockMainButton.Text = SimpleTrimmer_032 ' メイン
            BlockMainButton.Size = New System.Drawing.Size(70, 25)
            'V5.0.0.9⑯            BlockMainButton.Location = New Point(BlockNextButton.Left - 90, Form1.LblRotAtt.Location.Y + dspOfs)
            BlockMainButton.Location = New Point(Form1.tabCmd.Left, Form1.LblRotAtt.Location.Y + dspOfs) 'V5.0.0.9⑯
            BlockMainButton.Visible = False

            'V4.0.0.0-76

            ' 測定したときのトリミングモード表示デバッグ用
            'BlockTrimModeLabel = New System.Windows.Forms.Label
            'Form1.Controls.Add(BlockTrimModeLabel)
            'BlockTrimModeLabel.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            'BlockTrimModeLabel.TextAlign = ContentAlignment.MiddleCenter
            'BlockTrimModeLabel.Text = "A"
            'BlockTrimModeLabel.Size = New System.Drawing.Size(40, 25)
            'BlockTrimModeLabel.Location = New Point(BlockNextButton.Left - 50, Form1.LblRotAtt.Location.Y)
            'BlockTrimModeLabel.Visible = False
            'BlockTrimModeLabel.Visible = True

            'V4.0.0.0⑬

            ' 現在のブロック番号表示用ラベルの追加
            NowBlockNoLabel = New System.Windows.Forms.Label
            Form1.pnlDataDisplay.Controls.Add(NowBlockNoLabel)
            NowBlockNoLabel.Font = New System.Drawing.Font("MS UI Gothic", 14.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            NowBlockNoLabel.TextAlign = ContentAlignment.MiddleCenter
            NowBlockNoLabel.Text = ""
            NowBlockNoLabel.Size = New System.Drawing.Size(140, 25)
            NowBlockNoLabel.Location = New Point((TKY_DATA_WIDTH - 150), StatData(0).Label.Location.Y)
            'NowBlockNoLabel.Location = New Point((TKY_DATA_WIDTH - 150), BlockMainButton.Location.Y + dspOfs) 
            NowBlockNoLabel.Visible = False

#If START_KEY_SOFT Then
            If gbStartKeySoft Then
                SoftStartButton = New System.Windows.Forms.Button
                Form1.Controls.Add(SoftStartButton)
                SoftStartButton.TextAlign = ContentAlignment.MiddleCenter
                SoftStartButton.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
                SoftStartButton.Text = "START"
                SoftStartButton.Size = New System.Drawing.Size(100, 50)
                SoftStartButton.Location = New Point(Form1.GrpQrCode.Left + Form1.GrpQrCode.Width + 12, Form1.GrpQrCode.Top + 12)
                SoftStartButton.BackColor = System.Drawing.Color.LimeGreen
                'SoftStartButton.Location = New Point(0, 0)
                SoftStartButton.Visible = True

                SoftResetButton = New System.Windows.Forms.Button
                Form1.Controls.Add(SoftResetButton)
                SoftResetButton.TextAlign = ContentAlignment.MiddleCenter
                SoftResetButton.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
                SoftResetButton.Text = "RESET"
                SoftResetButton.Size = New System.Drawing.Size(100, 50)
                SoftResetButton.Location = New Point(SoftStartButton.Location.X + SoftStartButton.Size.Width + 12, SoftStartButton.Location.Y)
                SoftResetButton.BackColor = System.Drawing.Color.Peru
                SoftResetButton.Visible = True
            End If
#End If

            Call ResistorDataInitialDisp()

            'V4.0.0.0⑫↓
            gObjFrmDistribute.RedrawGraph()
            'V4.0.0.0⑫↑

            SetSimpleVideoSize()

            'V4.0.0.0⑫↓
            ' ｲﾆｼｬﾙﾃｽﾄ/ﾌｧｲﾅﾙﾃｽﾄ分布図ラベルを設定する
            'Form1.Label18.Text = PIC_TRIM_09                   ' "抵抗数"
            'Form1.Label2.Text = PIC_TRIM_03                     ' "良品"
            'Form1.Label3.Text = PIC_TRIM_04                       ' "不良品"
            'Form1.Label4.Text = PIC_TRIM_05                      ' "最小%"
            'Form1.Label5.Text = PIC_TRIM_06                      ' "最大%"
            'Form1.Label6.Text = PIC_TRIM_07                       ' "平均%"
            'Form1.Label7.Text = PIC_TRIM_08                     ' "標準偏差"
            'V4.0.0.0⑫↑


            'V4.1.0.0⑤
            Form1.ChkButtonDisp()
            'V4.1.0.0⑤

            'V4.10.0.0①              ↓
            With Form1.tabCmd
                Form1.btnUserLogon.Location = New Point(.Left, .Top + .Height - Form1.btnUserLogon.Height - 10)
                Form1.btnUserLogon.Size = New Size(.Width - 2, Form1.btnUserLogon.Height)
            End With
            'V4.10.0.0①              ↑

            Form1.Refresh()
        Catch ex As Exception
            MsgBox("SimpleTrimmer.SimpleTrimmerInit() TRAP ERROR = " + ex.Message)
        End Try

    End Sub
#End Region

#Region "抵抗データ表示開始位置座標と幅の取得"
    Private Sub GetResDataArea(ByRef X As Integer, ByRef Y As Integer, ByRef Width As Integer)
        Dim setLocation As System.Drawing.Point
        Dim setSize As System.Drawing.Size

        Try
            setLocation = Form1.VideoLibrary1.Location
            setSize = Form1.VideoLibrary1.Size

            X = setLocation.X + Form1.PanelGraph.Width + TKY_DATA_WIDTH + 6     ' データ表示開始Ｘ座標
            Y = setLocation.Y
            setSize = Form1.Size
            Width = setSize.Width - X - 2   ' 8は、画面右側の余白
        Catch ex As Exception
            MsgBox("SimpleTrimmer.GetResDataArea() TRAP ERROR = " + ex.Message)
        End Try
    End Sub
#End Region

#Region "抵抗データ初期表示処理"
    ''' <summary>
    ''' 抵抗データ初期表示処理
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ResistorDataInitialDisp()
        Dim Location_X As Integer, Location_Y As Integer
        Dim CntB As Integer
        Dim DataWidth2 As Integer
        Dim CntMax As Integer = UBound(ResistorData)
        Dim Language As Integer = tkyIni.TMENU.MSGTYP.Get(Of Integer)()
        Dim ResDataWidth As Integer         ' データ全体幅
        Dim bFirst As Boolean = False

        Try
            If HeaderButton(0) Is Nothing Then
                For Cnt As Integer = 0 To 2
                    HeaderButton(Cnt) = New System.Windows.Forms.Label
                    Form1.Controls.Add(HeaderButton(Cnt))
                Next
            End If

            Call GetResDataArea(Location_X, Location_Y, ResDataWidth)
            DataWidth2 = (ResDataWidth - RES_TITLE_SIZE) / 2

            For Cnt As Integer = 0 To CntMax
                If ResistorData(Cnt).Label Is Nothing Then
                    bFirst = True
                Else
                    Continue For
                End If
                ResistorData(Cnt).Label = New TKY_ALL_SL432HW.CustomTkyLabel
                ResistorData(Cnt).Label.AutoSize = False
                ResistorData(Cnt).Label.BorderColor = RES_LINE_COLOR
                ResistorData(Cnt).Label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
                ResistorData(Cnt).Label.BorderWidth = RES_BORDER_WIDTH
                ResistorData(Cnt).Label.Name = "Resistor"
                ResistorData(Cnt).Label.Text = ""
                ResistorData(Cnt).Label.Padding = New System.Windows.Forms.Padding(0)
                ResistorData(Cnt).Label.TabIndex = 361 + Cnt
                ResistorData(Cnt).Label.Font = New System.Drawing.Font("MS UI Gothic", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
                ResistorData(Cnt).Label.ForeColor = System.Drawing.Color.Black
                ResistorData(Cnt).Label.BackColor = System.Drawing.SystemColors.Window
                ResistorData(Cnt).Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
                ResistorData(Cnt).Label.Visible = False

                If Cnt >= RES_DATA_START Then
                    AddHandler ResistorData(Cnt).Label.Click, AddressOf ResistorData_Click_NextResistor
                Else
                    AddHandler HeaderButton(Cnt).Click, AddressOf ResistorData_Click_BlockChange
                End If

                CntB = Cnt Mod 3

                Select Case (CntB)
                    ' 歩留まり表示
                    Case 0

                        ResistorData(Cnt).Label.Text = (Cnt / 3).ToString()
                        If Cnt = 153 Then
                            Location_Y = Location_Y + RES_DATA_INTERVAL
                        End If
                        ResistorData(Cnt).Label.Location = New System.Drawing.Point(Location_X, Location_Y)
                        ResistorData(Cnt).Label.Size = New System.Drawing.Size(RES_TITLE_SIZE, RES_DATA_HIGHT)
                        ResistorData(Cnt).Label.Font = New System.Drawing.Font("MS UI Gothic", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
                        ' ２列のデータの１列目
                    Case 1
                        ResistorData(Cnt).Label.Location = New System.Drawing.Point(Location_X + RES_TITLE_SIZE + RES_DATA_X_OFF, Location_Y)
                        ResistorData(Cnt).Label.Size = New System.Drawing.Size(DataWidth2 + RES_DATA_X_OFF, RES_DATA_HIGHT)
                        ' ２列のデータの２列目
                    Case 2
                        ResistorData(Cnt).Label.Location = New System.Drawing.Point(Location_X + RES_TITLE_SIZE + RES_DATA_X_OFF * 3 + DataWidth2, Location_Y)
                        ResistorData(Cnt).Label.Size = New System.Drawing.Size(DataWidth2 + RES_DATA_X_OFF, RES_DATA_HIGHT)
                        Location_Y = Location_Y + RES_DATA_HIGHT + RES_DATA_Y_OFF
                End Select
                Form1.Controls.Add(ResistorData(Cnt).Label)
            Next
            If bFirst Then
                'If Language = 0 Then
                '    ResistorData(0).Label.Text = "抵抗"
                '    ResistorData(1).Label.Text = "初期測定（誤差%）"
                '    ResistorData(2).Label.Text = "最終測定（誤差%）"
                '    ResistorData(RES_DATA_STAT).Label.Text = "最小値"
                '    ResistorData(RES_DATA_STAT + 3).Label.Text = "最大値"
                '    ResistorData(RES_DATA_STAT + 3 * 2).Label.Text = "平均値"
                '    ResistorData(RES_DATA_STAT + 3 * 3).Label.Text = "３σ"
                '    ResistorData(RES_DATA_STAT + 3 * 4).Label.Text = "ＣｐＫ"
                'Else
                '    ResistorData(0).Label.Text = "Res No"
                '    ResistorData(1).Label.Text = "Initial Test（DEV%）"
                '    ResistorData(2).Label.Text = "Final Test（DEV%）"
                '    ResistorData(RES_DATA_STAT).Label.Text = "Min"
                '    ResistorData(RES_DATA_STAT + 3).Label.Text = "Max"
                '    ResistorData(RES_DATA_STAT + 3 * 2).Label.Text = "Average"
                '    ResistorData(RES_DATA_STAT + 3 * 3).Label.Text = "3sigma"
                '    ResistorData(RES_DATA_STAT + 3 * 4).Label.Text = "CpK"
                'End If
                ResistorData(0).Label.Text = SimpleTrimmer_033 ' 抵抗
                ResistorData(1).Label.Text = SimpleTrimmer_034 ' 初期測定（誤差%）
                ResistorData(2).Label.Text = SimpleTrimmer_035 ' 最終測定（誤差%）
                ResistorData(RES_DATA_STAT).Label.Text = SimpleTrimmer_023 ' 最小値
                ResistorData(RES_DATA_STAT + 3).Label.Text = SimpleTrimmer_024 ' 最大値
                ResistorData(RES_DATA_STAT + 3 * 2).Label.Text = SimpleTrimmer_025 ' 平均値
                ResistorData(RES_DATA_STAT + 3 * 3).Label.Text = SimpleTrimmer_026 ' ３σ
                ResistorData(RES_DATA_STAT + 3 * 4).Label.Text = SimpleTrimmer_027 ' ＣｐＫ
                For Cnt As Integer = 0 To 2

                    HeaderButton(Cnt).BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
                    HeaderButton(Cnt).Location = New System.Drawing.Point(ResistorData(Cnt).Label.Location.X, ResistorData(Cnt).Label.Location.Y)
                    HeaderButton(Cnt).Size = New System.Drawing.Size(ResistorData(Cnt).Label.Size.Width, ResistorData(Cnt).Label.Size.Height)
                    HeaderButton(Cnt).Font = New System.Drawing.Font("MS UI Gothic", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
                    HeaderButton(Cnt).Text = ResistorData(Cnt).Label.Text
                    HeaderButton(Cnt).FlatStyle = FlatStyle.Standard
                    HeaderButton(Cnt).TabIndex = 362 + CntMax + Cnt
                    HeaderButton(Cnt).ForeColor = System.Drawing.Color.Black
                    HeaderButton(Cnt).BackColor = System.Drawing.SystemColors.ButtonFace
                    HeaderButton(Cnt).TextAlign = System.Drawing.ContentAlignment.MiddleCenter
                    HeaderButton(Cnt).Enabled = False
                    HeaderButton(Cnt).Visible = False

                    'HeaderButton(Cnt).ForeColor = System.Drawing.Color.Black
                    'HeaderButton(Cnt).BackColor = System.Drawing.SystemColors.Window
                Next
            End If

            'If TrimData.IsTrimmingStopStatus Then
            '    For Cnt As Integer = 0 To 2
            '        ResistorData(Cnt).Label.Enabled = False
            '        ResistorData(Cnt).Label.Visible = False
            '        HeaderButton(Cnt).Enabled = True
            '        HeaderButton(Cnt).Visible = True
            '    Next
            'Else
            '    For Cnt As Integer = 0 To 2
            '        HeaderButton(Cnt).Enabled = False
            '        HeaderButton(Cnt).Visible = False
            '        ResistorData(Cnt).Label.Enabled = True
            '        ResistorData(Cnt).Label.Visible = True
            '    Next
            'End If

        Catch ex As Exception
            MsgBox("SimpleTrimmer.ResistoreDataDisp() TRAP ERROR = " + ex.Message)
        End Try
    End Sub

    Private Sub ResistorData_Click_BlockChange(ByVal sender As Object, ByVal e As EventArgs)
        Debug.WriteLine("抵抗データクリックイベント発生")
        Dim frmBlockDsp As FrmBlockDisp = New FrmBlockDisp()

        frmBlockDsp.Show()

    End Sub

    Private Sub ResistorData_Click_NextResistor(ByVal sender As Object, ByVal e As EventArgs)
        Debug.WriteLine("抵抗データクリック2イベント発生")
        ResistorDisplayNumber = ResistorDisplayNumber + RES_TOTAL_COUNTER
        If Not SimpleTrimmer.ResistorDataDisp(True, BlockDisplayNumber, ResistorDisplayNumber) Then
            ResistorDisplayNumber = 1
            Call SimpleTrimmer.ResistorDataDisp(True, BlockDisplayNumber, ResistorDisplayNumber)
        End If

    End Sub
#End Region

#Region "抵抗データ表示処理"
    ''' <summary>
    ''' 抵抗データ表示処理
    ''' </summary>
    ''' <param name="bDisp">True：表示処理 False:非表示処理</param>
    ''' <param name="BlockNo">ブロック番号 0:現在処理中のブロックを表示</param>
    ''' <param name="ResNo">抵抗番号</param>
    ''' <returns>True;表示データ有り False:表示データ無し</returns>
    ''' <remarks></remarks>
    Public Function ResistorDataDisp(ByVal bDisp As Boolean, ByVal BlockNo As Integer, ByVal ResNo As Integer) As Boolean
        Dim CntMax As Integer = UBound(ResistorData)

        ResistorDataDisp = False
        Try

            ' デジスイッチの読取り
            Dim digL As Integer, digH As Integer, digSW As Integer
            Call Form1.GetMoveMode(digL, digH, digSW)
            Dim cDummy As String = ""
            Dim FinalJudge As Integer = TrimClassLibrary.TrimDef.TRIM_RESULT_OK
            Dim bRtn As Boolean
            Dim bNgOnly As Boolean
            Dim NgResNo As Integer

            If digH = 0 Then        ' 表示無し
                bDisp = False  'V4.0.0.0-46
                'V4.0.0.0-46                Exit Function
            ElseIf digH = 1 Then    ' ＮＧのみ表示
                bNgOnly = True
            Else                    ' 全て表示
                bNgOnly = False
            End If

            If CntMax <> 0 Then
                If bDisp = False Then
                    'V4.0.0.0⑬
                    BlockNoLabel.Visible = False
                    BlockNextButton.Visible = False
                    BlockRvsButton.Visible = False
                    Form1.GroupBox1.Visible = True
                    BlockMainButton.Visible = False 'V4.0.0.0-76
                    'V4.0.0.0⑬
                Else
                    'V4.0.0.0⑬
                    BlockNoLabel.Visible = True
                    BlockNextButton.Visible = True
                    BlockRvsButton.Visible = True
                    Form1.GroupBox1.Visible = False
                    BlockMainButton.Visible = True 'V4.0.0.0-76
                    'V4.0.0.0⑬
                End If
            End If

            For Cnt As Integer = 0 To CntMax
                If bDisp = False Then
                    'If Not bBlockdataDisp Then
                    '    Exit Function
                    'End If
                    If ResistorData(Cnt).Label Is Nothing Then
                        Exit Function
                    Else
                        ResistorData(Cnt).Label.Visible = False
                        ''V4.0.0.0⑬
                        'BlockNoLabel.Visible = False
                        'BlockNextButton.Visible = False
                        'BlockRvsButton.Visible = False
                        'Form1.GroupBox1.Visible = True
                        'BlockMainButton.Visible = False 'V4.0.0.0-76
                        ''V4.0.0.0⑬
                    End If
                    Continue For
                Else
                    ResistorData(Cnt).Label.Visible = True
                    ''V4.0.0.0⑬
                    'BlockNoLabel.Visible = True
                    'BlockNextButton.Visible = True
                    'BlockRvsButton.Visible = True
                    'Form1.GroupBox1.Visible = False
                    'BlockMainButton.Visible = True 'V4.0.0.0-76
                    ''V4.0.0.0⑬
                End If

                If (0 < Cnt And Cnt < RES_DATA_STAT) And (Cnt Mod 3) = 0 Then
                    ResistorData(Cnt + 1).Label.Text = ""
                    ResistorData(Cnt + 2).Label.Text = ""
                    bRtn = False

                    'V4.0.0.0-23　↓
                    bRtn = TrimData.GetResTrimModeData(BlockNo, ResNo, digL)
                    If (bRtn) Then
                        'V4.0.0.0-23 ↑
                        If digL = TRIM_MODE_ITTRFT Then
                            bRtn = TrimData.GetResDispData(BlockNo, ResNo, bNgOnly, NgResNo, gsEDIT_DIGITNUM, "0.00", ResistorData(Cnt + 1).Label.Text, ResistorData(Cnt + 2).Label.Text, FinalJudge)
                            If bRtn Then
                                ResistorDataDisp = True
                            End If
                            If bRtn Then
                                ' ↓↓↓ V3.1.0.0④ 2014/12/05
                                'Select Case (FinalJudge)
                                '    Case TrimClassLibrary.TrimDef.TRIM_RESULT_OK
                                '        ResistorData(Cnt + 1).Label.BackColor = Color.GreenYellow
                                '        ResistorData(Cnt + 2).Label.BackColor = Color.GreenYellow
                                '    Case TrimClassLibrary.TrimDef.TRIM_RESULT_IT_NG, TrimClassLibrary.TrimDef.TRIM_RESULT_IT_HING, TrimClassLibrary.TrimDef.TRIM_RESULT_IT_LONG
                                '        ResistorData(Cnt + 1).Label.BackColor = Color.Red
                                '        ResistorData(Cnt + 2).Label.BackColor = Color.White
                                '    Case Else
                                '        ResistorData(Cnt + 1).Label.BackColor = Color.Red
                                '        ResistorData(Cnt + 2).Label.BackColor = Color.Red
                                'End Select
                                ' イニシャルテスト範囲チェック
                                ''V5.0.0.4⑤↓
                                If IsResistorRange(0, ResistorData(Cnt + 1).Label.Text) = True Then
                                    ''V5.0.0.4⑤    ResistorData(Cnt + 1).Label.BackColor = Color.GreenYellow
                                Else
                                    ''V5.0.0.4⑤    ResistorData(Cnt + 1).Label.BackColor = Color.Red
                                End If
                                If (IsTrimJudge(0, ResNo) = True) Then
                                    ResistorData(Cnt + 1).Label.BackColor = Color.GreenYellow
                                Else
                                    ResistorData(Cnt + 1).Label.BackColor = Color.Red
                                End If
                                ''V5.0.0.4⑤↑

                                ' ファイナルテスト範囲チェック
                                ''V5.0.0.4⑤↓
                                If IsResistorRange(1, ResistorData(Cnt + 2).Label.Text) = True Then
                                    ''V5.0.0.4⑤                                    ResistorData(Cnt + 2).Label.BackColor = Color.GreenYellow
                                Else
                                    ''V5.0.0.4⑤                                    ResistorData(Cnt + 2).Label.BackColor = Color.Red
                                End If
                                If (IsTrimJudge(1, ResNo) = True) Then
                                    ResistorData(Cnt + 2).Label.BackColor = Color.GreenYellow
                                Else
                                    ResistorData(Cnt + 2).Label.BackColor = Color.Red
                                End If
                                ''V5.0.0.4⑤↑

                                ' ↑↑↑ V3.1.0.0④ 2014/12/05
                            Else
                                ResistorData(Cnt + 1).Label.BackColor = Color.White
                                ResistorData(Cnt + 2).Label.BackColor = Color.White
                            End If
                        ElseIf digL = TRIM_MODE_TRFT Or digL = TRIM_MODE_FT Or digL = TRIM_MODE_MEAS Then
                            bRtn = TrimData.GetResDispData(BlockNo, ResNo, bNgOnly, NgResNo, gsEDIT_DIGITNUM, "0.00", cDummy, ResistorData(Cnt + 2).Label.Text, FinalJudge)
                            If bRtn Then
                                ResistorDataDisp = True
                            End If
                            If bRtn Then
                                'V4.1.0.0⑤
                                ResistorData(Cnt + 1).Label.BackColor = Color.White
                                '' '' ''V4.0.0.0-22　↓
                                '' ''ResistorData(Cnt + 2).Label.BackColor = Color.White
                                '' '' ''V4.0.0.0-22　↑
                                'V4.1.0.0⑤

                                ' ↓↓↓ V3.1.0.0④ 2014/12/05
                                'Select Case (FinalJudge)
                                '    Case TrimClassLibrary.TrimDef.TRIM_RESULT_NOTDO
                                '        ResistorData(Cnt + 2).Label.BackColor = Color.White
                                '    Case TrimClassLibrary.TrimDef.TRIM_RESULT_OK
                                '        ResistorData(Cnt + 2).Label.BackColor = Color.GreenYellow
                                '    Case Else
                                '        ResistorData(Cnt + 2).Label.BackColor = Color.Red
                                'End Select

                                ' ファイナルテスト範囲チェック
                                '----- V4.0.0.0⑳↓ -----
                                'If IsResistorRange(1, ResistorData(Cnt + 2).Label.Text) = True Then
                                '    ResistorData(Cnt + 2).Label.BackColor = Color.GreenYellow
                                'Else
                                '    ResistorData(Cnt + 2).Label.BackColor = Color.Red
                                'End If
                                ' ↑↑↑ V3.1.0.0④ 2014/12/05
                                ' x3モード以外は範囲チェックを行う
                                If digL <> TRIM_MODE_MEAS Then
                                    ''V5.0.0.4⑤↓
                                    If IsResistorRange(1, ResistorData(Cnt + 2).Label.Text) = True Then
                                        'V5.0.0.4⑤    ResistorData(Cnt + 2).Label.BackColor = Color.GreenYellow
                                    Else
                                        'V5.0.0.4⑤    ResistorData(Cnt + 2).Label.BackColor = Color.Red
                                    End If
                                    If (IsTrimJudge(1, ResNo) = True) Then
                                        ResistorData(Cnt + 2).Label.BackColor = Color.GreenYellow
                                    Else
                                        ResistorData(Cnt + 2).Label.BackColor = Color.Red
                                    End If
                                    ''V5.0.0.4⑤↑

                                Else
                                    'V4.1.0.0⑤ 場所変更 
                                    ''V4.0.0.0-22　↓
                                    ResistorData(Cnt + 2).Label.BackColor = Color.White
                                    ''V4.0.0.0-22　↑
                                    'V4.1.0.0⑤
                                End If
                                '----- V4.0.0.0⑳↑ -----
                            Else
                                ResistorData(Cnt + 1).Label.BackColor = Color.White
                                ResistorData(Cnt + 2).Label.BackColor = Color.White
                            End If
                        End If
                        If bNgOnly Then
                            'V4.0.0.0-47
                            If NgResNo <> 0 Then
                                ResistorData(Cnt).Label.Text = NgResNo.ToString()
                            Else
                                ResistorData(Cnt).Label.Text = ""
                                ResistorData(Cnt + 1).Label.BackColor = Color.White
                                ResistorData(Cnt + 1).Label.Text = ""
                                ResistorData(Cnt + 2).Label.BackColor = Color.White
                                ResistorData(Cnt + 2).Label.Text = ""
                            End If
                            'V4.0.0.0-47

                        Else
                            ResistorData(Cnt).Label.Text = ResNo.ToString()
                        End If
                        ResNo = ResNo + 1
                    Else 'V4.0.0.0-23
                        ResistorData(Cnt).Label.Text = ""
                        ResistorData(Cnt + 1).Label.BackColor = Color.White
                        ResistorData(Cnt + 2).Label.BackColor = Color.White
                        'V4.0.0.0-23
                    End If
                    'BlockTrimModeLabel.Text = digL.ToString()
                End If
            Next

            If bDisp = False Then
                For Cnt As Integer = 0 To 2
                    HeaderButton(Cnt).Enabled = False
                    HeaderButton(Cnt).Visible = False
                    ResistorData(Cnt).Label.Enabled = False
                    ResistorData(Cnt).Label.Visible = False
                Next
            Else
                If TrimData.IsTrimmingStopStatus And TrimData.GetPlateCounter() > 0 Then    ' 基板を１枚でも処理してから停止時
                    For Cnt As Integer = 0 To 2
                        ResistorData(Cnt).Label.Enabled = False
                        ResistorData(Cnt).Label.Visible = False
                        HeaderButton(Cnt).Enabled = True
                        HeaderButton(Cnt).Visible = True
                    Next
                Else
                    For Cnt As Integer = 0 To 2
                        HeaderButton(Cnt).Enabled = False
                        HeaderButton(Cnt).Visible = False
                        ResistorData(Cnt).Label.Enabled = True
                        ResistorData(Cnt).Label.Visible = True
                    Next
                End If
                'V4.0.0.0⑬↓
                If BlockNo = 0 Then
                    BlockDisplayNumber = TrimData.GetBlockNumber()
                    BlockNoLabel.Text = BlockDisplayNumber
                Else
                    BlockDisplayNumber = BlockNo
                    BlockNoLabel.Text = BlockDisplayNumber
                End If
                'V4.0.0.0⑬↑
            End If

            ' 統計計算表示
            If bDisp = True Then
                If digL = TRIM_MODE_ITTRFT Then
                    TrimData.GetStaticsDataForResistor(BlockNo, gsEDIT_DIGITNUM, ResistorData(RES_DATA_STAT + 1).Label.Text, ResistorData(RES_DATA_STAT + 4).Label.Text, ResistorData(RES_DATA_STAT + 3 * 2 + 1).Label.Text, ResistorData(RES_DATA_STAT + 3 * 3 + 1).Label.Text, ResistorData(RES_DATA_STAT + 3 * 4 + 1).Label.Text, ResistorData(RES_DATA_STAT + 2).Label.Text, ResistorData(RES_DATA_STAT + 5).Label.Text, ResistorData(RES_DATA_STAT + 3 * 2 + 2).Label.Text, ResistorData(RES_DATA_STAT + 3 * 3 + 2).Label.Text, ResistorData(RES_DATA_STAT + 3 * 4 + 2).Label.Text)
                ElseIf digL = TRIM_MODE_TRFT Or digL = TRIM_MODE_FT Or digL = TRIM_MODE_MEAS Then
                    TrimData.GetStaticsDataForResistor(BlockNo, gsEDIT_DIGITNUM, cDummy, cDummy, cDummy, cDummy, cDummy, ResistorData(RES_DATA_STAT + 2).Label.Text, ResistorData(RES_DATA_STAT + 5).Label.Text, ResistorData(RES_DATA_STAT + 3 * 2 + 2).Label.Text, ResistorData(RES_DATA_STAT + 3 * 3 + 2).Label.Text, ResistorData(RES_DATA_STAT + 3 * 4 + 2).Label.Text)
                    'V4.0.0.0-44
                    ''ResistorData(RES_DATA_STAT + 1).Label.Text = ""
                    ''ResistorData(RES_DATA_STAT + 4).Label.Text = ""
                    ''ResistorData(RES_DATA_STAT + 3 * 2 + 1).Label.Text = ""
                    ''ResistorData(RES_DATA_STAT + 3 * 3 + 1).Label.Text = ""
                    ''ResistorData(RES_DATA_STAT + 3 * 4 + 1).Label.Text = ""
                    'V4.0.0.0-44
                End If
            End If
        Catch ex As Exception
            MsgBox("SimpleTrimmer.ResistorDataDisp() TRAP ERROR = " + ex.Message)
        End Try
    End Function

#End Region

#Region "目標値、上下限値の表示"
    '''=========================================================================
    ''' <summary>目標値、上下限値の表示</summary>
    ''' <param name="Target"></param>
    ''' <param name="InitialLo"></param>
    ''' <param name="InitialHi"></param>
    ''' <param name="FinalLo"></param>
    ''' <param name="FinalHi"></param>
    '''=========================================================================
    Public Sub SetTarget(ByVal Target As Double, ByVal InitialLo As Double, ByVal InitialHi As Double, ByVal FinalLo As Double, ByVal FinalHi As Double, ByVal DispTarget As Double)

        '----- V4.0.0.0-31↓ -----
        ' SL436SでなければNOP(SL43xR時はTrimDataオブジェクトが設定されないので下記を実行するとエラーとなる)
        If (giMachineKd <> MACHINE_KD_RS) Then Return
        '----- V4.0.0.0-31↑ -----

        '        TrimData.SetTarget(Target, InitialLo, InitialHi, FinalLo, FinalHi, DispTarget)
        TrimData.SetTarget(Target, InitialLo, InitialHi, FinalLo, FinalHi, DispTarget)
        TrimData.GetTarget(StatData(16).Label.Text, StatData(21).Label.Text, StatData(22).Label.Text, StatData(24).Label.Text, StatData(25).Label.Text)
 
    End Sub
#End Region

#Region "表示データ更新"
    ''' <summary>
    ''' ロットスタート処理（データ初期化）
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub TrimmingStart()
        Try
            TrimData.SetResistorCount(typPlateInfo.intResistCntInBlock) ' １ブロック内抵抗数を保存する。
            TrimData.LotStart()                                         ' ロットデータの初期化
            TrimData.PlateStart()                                       ' 基板データの初期化
            TrimData.GetProductData(StatData(2).Label.Text, StatData(4).Label.Text, StatData(6).Label.Text, StatData(8).Label.Text, StatData(9).Label.Text, StatData(11).Label.Text, StatData(12).Label.Text, StatData(14).Label.Text)
            bBlockdataDisp = False      'トリミングミード開始したら、フラグを落とす　'V4.0.0.0-62
        Catch ex As Exception
            MsgBox("SimpleTrimmer.TrimmingStart() TRAP ERROR = " + ex.Message)
        End Try
    End Sub

    '----- V4.0.0.0-87↓ -----
    Public Sub DspLaserPower()

        Dim LaserPower As Double = 0.0

        Try
            LaserPower = TrimData.GetLaserPower()
            If (LaserPower = 0) Then
                StatData(14).Label.Text = "-.--W"
            Else
                StatData(14).Label.Text = LaserPower.ToString("0.00") + "W"
            End If

        Catch ex As Exception
            MsgBox("SimpleTrimmer.DspLaserPower() TRAP ERROR = " + ex.Message)
        End Try
    End Sub
    '----- V4.0.0.0-87↑ -----

    'V3.1.0.0⑤↓
    ''' <summary>
    ''' ブロック開始時のブロックの統計データの初期化処理
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub BlockStart()
        Try
            TrimData.BlockStart()                           ' ブロックデータの初期化
        Catch ex As Exception
            MsgBox("SimpleTrimmer.BlockStart() TRAP ERROR = " + ex.Message)
        End Try
    End Sub
    'V3.1.0.0⑤↑
#Region "１基板処理終了時のデータ処理"
    '''=========================================================================
    ''' <summary>１基板処理終了時のデータ処理</summary>
    '''=========================================================================
    Public Sub SetPlateEnd()

        Try
            Call TrimData.SetPlateEnd()

            '----- V4.11.0.0④↓ (WALSIN殿SL436S対応) -----
            ' DllTrimClassLibraryに一時停止時間を渡す
            TrimData.SetTrimPauseTime(giTrimTimeOpt, StPauseTime.TotalTime)
            '----- V4.11.0.0④↑ -----

            ' "ロット番号","開始時間","経過時間","処理基板数","処理基板枚数単位時間","処理抵抗数","処理抵抗数単位時間","レーザパワー"表示
            TrimData.GetProductData(StatData(2).Label.Text, StatData(4).Label.Text, StatData(6).Label.Text, StatData(8).Label.Text, StatData(9).Label.Text, StatData(11).Label.Text, StatData(12).Label.Text, StatData(14).Label.Text)

            ' １基板処理後は、［BLOCK DATA］ボタンを表示する。
            If TrimData.GetPlateCounter() > 0 Then
                CmdBlockData.Visible = True
            End If

        Catch ex As Exception
            MsgBox("SimpleTrimmer.SetPlateEnd() TRAP ERROR = " + ex.Message)
        End Try
    End Sub
#End Region

    ''' <summary>
    ''' ロット終了処理
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SetLotEnd()
        Try
            Call TrimData.SetLotEnd()
        Catch ex As Exception
            MsgBox("SimpleTrimmer.SetLotEnd() TRAP ERROR = " + ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 経過時間の更新
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ElapsedTimeUpdate()
        Try
            Dim ColmnOff As Integer = 0
            Dim sDummy As String = ""
            Dim strDAT As String = ""                                   ' V4.11.0.0④

            ' V4.0.0.0-50
            If (IsNothing(TrimData) = True) Then
                Return
            End If

            ' 処理個数は更新しない。            TrimData.GetRealTimeData(StatData(6).Label.Text, StatData(11).Label.Text, StatData(12).Label.Text)
            '            TrimData.GetRealTimeData(StatData(6).Label.Text, sDummy, sDummy)

            '----- V4.11.0.0④↓ (WALSIN殿SL436S対応) -----
            'TrimData.GetRealTimeData(StatData(6).Label.Text, sDummy, sDummy)
            ' 経過時間の表示
            If (giTrimTimeOpt = 0) Then                                 ' タクト表示時に一時停止時間を(0=含める(標準), 1=含めない)
                TrimData.GetRealTimeData(StatData(6).Label.Text, sDummy, sDummy)
            Else
                ' 終了時間を更新する為GetRealTimeDataはCallする
                TrimData.GetRealTimeData(strDAT, sDummy, sDummy)
                ' 経過時間を表示する(一時停止画面表示中は最初の一度のみ表示する)
                If (m_blnElapsedTime = True) Then
                    StatData(6).Label.Text = strDAT
                    m_blnElapsedTime = False
                End If
            End If
            '----- V4.11.0.0④↑ -----

            'V4.8.0.1①↓
            '            TrimData.GetYieldData(StatData(0).Label.Text)
            TrimData.GetYieldData(giRateDisp, StatData(0).Label.Text)
            'V4.8.0.1①↑

        Catch ex As Exception
            'V4.11.0.0② COMのオープンエラー時TRAPとなる
            'MsgBox("SimpleTrimmer.ElapsedTimeUpdate() TRAP ERROR = " + ex.Message) 
        End Try
    End Sub

    ''' <summary>
    ''' 統計値表示データの更新
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SimpleTrim_LoggingStart()
        Try
            Dim ColmnOff As Integer = 0
            ' デジスイッチの読取り
            Dim digL As Integer, digH As Integer, digSW As Integer
            Call Form1.GetMoveMode(digL, digH, digSW)

            For Mode As Integer = StatType.PLATE_INITIAL To StatType.LOT_FINAL
                ColmnOff = TKY_DATA_POS + (Mode - StatType.PLATE_INITIAL)
                ' 'V4.0.0.0-44↓
                'If ((digL = TRIM_MODE_TRFT Or digL = TRIM_MODE_FT Or digL = TRIM_MODE_MEAS) And (Mode = StatType.PLATE_INITIAL Or Mode = StatType.LOT_INITIAL)) Or digL = TRIM_MODE_POSCHK Or digL = TRIM_MODE_CUT Or digL = TRIM_MODE_STPRPT Then
                '    StatData(ColmnOff).Label.Text = ""
                '    StatData(ColmnOff + TKY_DATA_COL).Label.Text = ""
                '    StatData(ColmnOff + TKY_DATA_COL * 2).Label.Text = ""
                '    StatData(ColmnOff + TKY_DATA_COL * 3).Label.Text = ""
                '    StatData(ColmnOff + TKY_DATA_COL * 4).Label.Text = ""
                '    StatData(ColmnOff + TKY_DATA_COL * 5).Label.Text = ""
                '    StatData(ColmnOff + TKY_DATA_COL * 6).Label.Text = ""
                '    StatData(ColmnOff + TKY_DATA_COL * 7).Label.Text = ""
                '    StatData(ColmnOff + TKY_DATA_COL * 8).Label.Text = ""
                '    StatData(ColmnOff + TKY_DATA_COL * 9).Label.Text = ""
                '    Continue For
                'End If
                ' 'V4.0.0.0-44↑
                TrimData.GetStaticsData(Mode, gsEDIT_DIGITNUM, StatData(ColmnOff).Label.Text, StatData(ColmnOff + TKY_DATA_COL).Label.Text, StatData(ColmnOff + TKY_DATA_COL * 2).Label.Text, StatData(ColmnOff + TKY_DATA_COL * 3).Label.Text, StatData(ColmnOff + TKY_DATA_COL * 4).Label.Text, StatData(ColmnOff + TKY_DATA_COL * 5).Label.Text, StatData(ColmnOff + TKY_DATA_COL * 6).Label.Text, StatData(ColmnOff + TKY_DATA_COL * 7).Label.Text, StatData(ColmnOff + TKY_DATA_COL * 8).Label.Text, StatData(ColmnOff + TKY_DATA_COL * 9).Label.Text)
                'V5.0.0.4④ＣＰＫ非表示↓
                If giCpk_Disp_Off Then
                    StatData(ColmnOff + TKY_DATA_COL * 9).Label.Text = ""
                End If
                'V5.0.0.4④↑
            Next
        Catch ex As Exception
            MsgBox("SimpleTrimmer.SimpleTrim_LoggingStart() TRAP ERROR = " + ex.Message)
        End Try
    End Sub

    Public Sub ResistorDataDisplay(ByVal bDisp As Boolean, ByVal BlockNo As Integer, ByVal ResNo As Integer)
        Try
            If Not bBlockdataDisp And bDisp Then    ' 現在ブロックデータ未表示で表示指示の時は、コマンドボタンを消す。
                If giAppMode = APP_MODE_IDLE Then 'V4.0.0.0-75
                    Form1.tabCmd.Visible = False
                Else
                    Call Form1.Form1Button(0)
                End If
            End If
            ' 指定のブロック、抵抗番号のデータを表示する、または消す。
            BlockDisplayNumber = BlockNo
            ResistorDisplayNumber = ResNo
            Call SimpleTrimmer.ResistorDataDisp(bDisp, BlockDisplayNumber, ResistorDisplayNumber)
            If bDisp Then                           ' 表示指示の時、状態フラグを表示に変更する。
                bBlockdataDisp = True
            Else
                bBlockdataDisp = False
                Call Form1.Form1Button(1)           ' 
            End If
        Catch ex As Exception
            MsgBox("SimpleTrimmer.ResistorDataDisplay() TRAP ERROR = " + ex.Message)
        End Try
    End Sub
#End Region

#Region "その他メソッド"
    Public Function IsBlockDataDisp() As Boolean
        IsBlockDataDisp = bBlockdataDisp
    End Function

#End Region

#Region "データクリアボタン処理"
    Private Sub DataClrButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataClrButton.Click
        Dim r As Integer

        ' 累計をクリアしてもよろしいですか？
        r = MsgBox(MSG_108, MsgBoxStyle.YesNo + MsgBoxStyle.SystemModal + MsgBoxStyle.MsgBoxSetForeground)
        If (r = MsgBoxResult.Yes) Then

            Debug.WriteLine("デバッグ・メッセージを出力DataClrButton_Click")
            TrimData.ResetLotData()
            TrimData.SetLaserPower(0.0)     'V4.9.0.0④
            TrimData.GetProductData(StatData(2).Label.Text, StatData(4).Label.Text, StatData(6).Label.Text, StatData(8).Label.Text, StatData(9).Label.Text, StatData(11).Label.Text, StatData(12).Label.Text, StatData(14).Label.Text)
            SimpleTrim_LoggingStart()
            CmdBlockData.Visible = False         ' ［BLOCK DATA］ボタンを非常時にする。
            Call Form1.System1.OperationLogging(gSysPrm, MSG_OPLOG_CLRTOTAL, "MANUAL")
            Call Form1.ClearCounter(1)                              ' 生産管理データのクリア
            Call ClrTrimPrnData()                                   ' ﾄﾘﾐﾝｸﾞ結果印刷項目のﾃﾞｰﾀをｸﾘｱする(ローム殿特注) V1.18.0.0③

            ' 統計表示がONの場合、表示を更新する
            'If Form1.chkDistributeOnOff.Checked = True Then
            gObjFrmDistribute.RedrawGraph() 'V4.0.0.0-81
            'End If
        End If

    End Sub
#End Region

#Region "データ保存ボタン処理"
    Private Sub DataSaveButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataSaveButton.Click
        Debug.WriteLine("デバッグ・メッセージを出力DataSaveButton_Click")

        'V4.0.0.0-58??
        'Dim sFilePath As String = TrimData.GetStaticsDataLogFileName()
        'TrimData.WriteStaticsData(gSysPrm.stLOG.gsLoggingDir & sFilePath, gsEDIT_DIGITNUM)
        SubDataSave()
        'V4.0.0.0-58??
        If (IsNothing(TrimData) = True) Then Return 'V4.0.0.0-68
        Dim sFilePath As String = TrimData.GetStaticsDataLogFileName()
        'If gSysPrm.stTMN.giMsgTyp = 0 Then
        '    MsgBox("保存完了！" & vbCrLf & " (" & gSysPrm.stLOG.gsLoggingDir & sFilePath & ")")
        'Else
        '    MsgBox("Save completion." & vbCrLf & " (" & gSysPrm.stLOG.gsLoggingDir & sFilePath & ")")
        'End If
        MsgBox(SimpleTrimmer_001 & vbCrLf & " (" & gSysPrm.stLOG.gsLoggingDir & sFilePath & ")")

    End Sub

    ''' <summary>
    ''' データ保存         'V4.0.0.0-58??
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SubDataSave()

        If (IsNothing(TrimData) = True) Then Return 'V4.0.0.0-68
        Dim sFilePath As String = TrimData.GetStaticsDataLogFileName()

        TrimData.WriteStaticsData(gSysPrm.stLOG.gsLoggingDir & sFilePath, gsEDIT_DIGITNUM)

    End Sub
#End Region

#Region "ＢＬＯＣＫ ＤＡＴＡボタン処理"
    Public Sub PlateTrimmingEnd()
        Try
            If TrimData.GetPlateCounter() > 0 Then
                CmdBlockData.Visible = True         ' １基板処理後は、［BLOCK DATA］ボタンを表示する。
            End If
        Catch ex As Exception
            MsgBox("SimpleTrimmer.BlockDataButtonDisp() TRAP ERROR = " + ex.Message)
        End Try
    End Sub
    Private Sub cmdBlockData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdBlockData.Click
        Debug.WriteLine("デバッグ・メッセージを出力cmdBlockData_Click")
        BlockDisplayNumber = 1      'V4.0.0.0⑬
        Call ResistorDataDisplay(True, BlockDisplayNumber, ResistorDisplayNumber)
    End Sub
#End Region

#Region "コマンドボタン操作"
    Public Sub CommandButtonTutorial()
        Try
            If gCmpTrimDataFlg = 1 Then ' セーブコマンド 編集中のデータあり
                commandtutorial.SetDataChange()
            Else
                commandtutorial.SetDataSave()
            End If

            Form1.CmdLoad.BackColor = commandtutorial.GetLoadColor()

            Form1.CmdSave.BackColor = commandtutorial.GetSaveColor()

            Form1.CmdTeach.BackColor = commandtutorial.GetTeachColor()


        Catch ex As Exception
            MsgBox("SimpleTrimmer.CommandButtonChange() TRAP ERROR = " + ex.Message)
        End Try
    End Sub
#End Region

#Region "シンプルトリマ追加ボタン処理"
    ''' <summary>
    ''' メンテナンスボタン処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CmdMainte_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdMainte.Click
        Dim r As Integer
        'V6.0.0.0⑱        Dim frmobject As Object
        Dim frmobject As frmMaintenance 'V6.0.0.0⑱

        ' トリマ装置状態を動作中に設定する
        r = Form1.TrimStateOn(F_MAINTENANCE, APP_MODE_MAINTENANCE, "", "")
        If (r <> cFRS_NORMAL) Then Return

        frmobject = New frmMaintenance
        frmobject.Showdialog()
        'V5.0.0.1⑲↓
        If (frmobject Is Nothing = False) Then
            Call frmobject.Close()                                        ' オブジェクト開放
            Call frmobject.Dispose()                                      ' リソース開放
            frmobject = Nothing
        End If
        'V5.0.0.1⑲↑

        ' 終了処理
        Call Form1.TrimStateOff()                                         ' トリマ装置状態を動作中に設定する

    End Sub


    ''' <summary>
    ''' プローブクリーニングボタン処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CmdProbeClean_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdProbeClean.Click
        Dim r As Integer

        ' コマンド実行
        r = Form1.CmdExec_Proc(F_PROBE_CLEANING, APP_MODE_PROBE_CLEANING, MSG_OPLOG_MAINT, "")

    End Sub

#Region "BP Offset 調整ボタン処理"
    '''=========================================================================
    ''' <summary>BP Offset 調整ボタン処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BPAdjustButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BPAdjustButton.Click

        Dim digL As Integer
        Dim digH As Integer
        Dim digSW As Integer

        '----- V4.11.0.0⑤↓ (WALSIN殿SL436S対応) -----
        ' トリミング開始ブロック番号を非表示とする
        Form1.GrpStartBlk.Visible = False
        '----- V4.11.0.0⑤↑ -----

        'デジタルスイッチの値取得
        Call Form1.GetMoveMode(digL, digH, digSW)

        Form1.TimerAdjust.Enabled = False

        gObjADJ = New frmFineAdjust()

        SetTeachVideoSize()                                     'V2.0.0.0⑱
        Form1.Instance.VideoLibrary1.SetTrackBarVisible(True)   'V6.0.0.0⑥

        'V4.0.0.0⑱
        Call SimpleTrimmer.ResistorDataDisp(False, 0, 0)
        'V4.0.0.0⑱

        ' データ表示を非表示にする
        Call Form1.SetDataDisplayOff()                          'V4.0.0.0⑪
        GroupBoxVisibleChange(False)                            'V4.0.0.0⑪

        '#4.12.2.0⑥        Call gObjADJ.SetInitialData(gSysPrm, digL, digH, gCurPlateNo, gCurBlockNo)
        gObjADJ.SetInitialData(gSysPrm, digL, digH, gCurPlateNo, gCurBlockNo,
                               gCurPlateNoX, gCurPlateNoY, gCurBlockNoX, gCurBlockNoY)  '#4.12.2.0⑥
        Call gObjADJ.Focus()
        Call gObjADJ.Show()

    End Sub
#End Region

    ''' <summary>
    ''' Lot番号入力画面の表示 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CmdLotNumber_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdLotNumber.Click
        'V6.0.0.0⑯        gObjADJ = New frmLotNoInput
        'V6.0.0.0⑯        Call gObjADJ.Focus()
        'V6.0.0.0⑯        Call gObjADJ.Show()
        Using frm As New frmLotNoInput()        'V6.0.0.0⑯
            frm.ShowDialog(Form1.Instance)
        End Using
    End Sub

#End Region

#Region "Videoサイズの変更"

    ''' <summary>
    ''' シンプルトリマ用のVideoサイズに変更する 'V2.0.0.0⑱
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SetSimpleVideoSize()
        If gKeiTyp <> KEY_TYPE_RS Then
            Return
        End If

        '        Form1.VideoLibrary1.SetVideoSizeAndCross(SIMPLE_PICTURE_SIZEX, SIMPLE_PICTURE_SIZEY, CROSS_LINEX, CROSS_LINEY)
        'V4.0.0.0⑩        Form1.VideoLibrary1.SetVideoSizeAndCross(SIMPLE_SIZE, SIMPLE_PICTURE_SIZEX, SIMPLE_PICTURE_SIZEY, CROSS_LINEX, CROSS_LINEY)
        Form1.Instance.VideoLibrary1.SetVideoSizeAndCross(SIMPLE_SIZE, NORMAL_PICTURE_SIZEX, NORMAL_PICTURE_SIZEY, SIMPLE_PICTURE_SIZEX, SIMPLE_PICTURE_SIZEY, CROSS_LINEX, CROSS_LINEY)
        'Form1.VideoLibrary1.Width = SIMPLE_PICTURE_SIZEX
        'Form1.VideoLibrary1.Height = SIMPLE_PICTURE_SIZEY
        Form1.Instance.VideoLibrary1.Size = New Size(SIMPLE_PICTURE_SIZEX, SIMPLE_PICTURE_SIZEY)
#If False Then              'V6.0.0.0④
        Form1.Picture1.Top = CROSS_LINEY + Form1.VideoLibrary1.Top
        Form1.Picture2.Left = CROSS_LINEX + Form1.VideoLibrary1.Left

        'Form1.Picture1.Width = SIMPLE_PICTURE_SIZEX / 15
        'Form1.Picture2.Height = SIMPLE_PICTURE_SIZEY / 15
        Form1.Picture1.Width = SIMPLE_PICTURE_SIZEX
        Form1.Picture2.Height = SIMPLE_PICTURE_SIZEY
#Else
        Form1.Instance.VideoLibrary1.SetCrossLineCenter(CROSS_LINEX, CROSS_LINEY)
#End If
        Form1.Instance.PanelGraphOnOff(True)                        ' V4.0.0.0⑫

    End Sub

    ''' <summary>
    ''' ティーチング用に従来のVideoサイズに変更する 'V2.0.0.0⑱
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SetTeachVideoSize()
        If gKeiTyp <> KEY_TYPE_RS Then
            Return
        End If

        '        Form1.VideoLibrary1.SetVideoSizeAndCross(NORMAL_PICTURE_SIZEX, NORMAL_PICTURE_SIZEY, gSysPrm.stDVR.giCrossLineY, gSysPrm.stDVR.giCrossLineX)
        Form1.Instance.VideoLibrary1.SetVideoSizeAndCross(NORMAL_SIZE, NORMAL_PICTURE_SIZEX, NORMAL_PICTURE_SIZEY, NORMAL_PICTURE_SIZEX, NORMAL_PICTURE_SIZEY, gSysPrm.stDVR.giCrossLineY, gSysPrm.stDVR.giCrossLineX)
        'Form1.Instance.VideoLibrary1.Width = NORMAL_PICTURE_SIZEX
        'Form1.Instance.VideoLibrary1.Height = NORMAL_PICTURE_SIZEY
        Form1.Instance.VideoLibrary1.Size = New Size(NORMAL_PICTURE_SIZEX, NORMAL_PICTURE_SIZEY)
#If False Then              'V6.0.0.0④
        Form1.Picture1.Top = gSysPrm.stDVR.giCrossLineX + Form1.VideoLibrary1.Top
        Form1.Picture2.Left = gSysPrm.stDVR.giCrossLineY + Form1.VideoLibrary1.Left

        Form1.Picture1.Width = NORMAL_PICTURE_SIZEX
        Form1.Picture2.Height = NORMAL_PICTURE_SIZEY
#Else
        ' Top, Left と X, Y が逆になっている
        Form1.Instance.VideoLibrary1.SetCrossLineCenter(gSysPrm.stDVR.giCrossLineY, gSysPrm.stDVR.giCrossLineX)
#End If
        Form1.Instance.PanelGraphOnOff(False)                        ' V4.0.0.0⑫

    End Sub
#End Region

#Region "下部グループボックスの表示、非表示を切り替える"
    '''=========================================================================
    '''<summary>下部グループボックスの表示、非表示を切り替える</summary>
    '''<param name="VisibleFlag"> (IN)VisibleFlag</param>
    '''=========================================================================
    Public Sub GroupBoxVisibleChange(ByVal VisibleFlag As Boolean)

        Form1.GrpMode.Visible = VisibleFlag

    End Sub

#End Region

#Region "下部グループボックスの有効、無効を切り替える"
    '''=========================================================================
    '''<summary>下部グループボックスの有効、無効を切り替える</summary>
    '''<param name="EnableFlag"> (IN)EnableFlag</param>
    '''=========================================================================
    Public Sub GroupBoxEnableChange(ByVal EnableFlag As Boolean)

        'V4.0.0.0-55
        '        Form1.GrpMode.Enabled = EnableFlag
        '一時停止用のADJは常に押せる必要があるため、個別に切り替える    
        '        BPAdjustButton.Enabled = True 'V4.11.0.0⑯
        BPAdjustButton.Enabled = EnableFlag 'V4.11.0.0⑯

        DataClrButton.Enabled = EnableFlag
        DataSaveButton.Enabled = EnableFlag
        Form1.CbDigSwH.Enabled = EnableFlag
        Form1.CbDigSwL.Enabled = EnableFlag
        'V4.0.0.0-55
        'V4.0.0.0-82        ' 
        BlockNextButton.Enabled = EnableFlag
        BlockRvsButton.Enabled = EnableFlag
        BlockMainButton.Enabled = EnableFlag
        'V4.0.0.0-82        ' 
        DataEditButton.Enabled = EnableFlag 'V4.0.0.0-84 

    End Sub

#End Region
    ' ↓↓↓ V3.1.0.0④ 2014/12/05
#Region "抵抗値の範囲チェック"
    ''' <summary>
    ''' 抵抗値の範囲チェック
    ''' </summary>
    ''' <param name="nType">種別（0=初期、1=最終）</param>
    ''' <param name="strValue">データ文字列[xxx.xxxxxxx(x.xxx)]</param>
    ''' <returns>True=範囲内、False=範囲外</returns>
    ''' <remarks></remarks>
    Private Function IsResistorRange(ByVal nType As Integer, ByVal strValue As String) As Boolean
        Dim dTmp As Double
        Dim dMin As Double
        Dim dMax As Double
        Dim dTarget As Double
        Dim dValue As Double
        '----- V4.0.0.0⑳↓ -----
        Dim i As Integer
        Dim nIndex As Integer
        Dim bRet As Boolean
        ' UNDONE: TrimData.GetResDispDataの処理も変更が必要
        'Dim strUnit() As String = {"MΩ", "KΩ", "Ω", "mΩ"}   ' 単位
        Dim strUnit() As String = {SimpleTrimmer_040, SimpleTrimmer_041, SimpleTrimmer_042, SimpleTrimmer_043}   ' 単位
        '----- V4.0.0.0⑳↑ -----
        Dim tempTarget As String
        IsResistorRange = False

        Do
            '----- V4.0.0.0⑳↓ -----
            '' データ文字列から抵抗値を取得
            'If Double.TryParse(strValue.Substring(0, strValue.IndexOf("(")), dValue) = False Then
            '    System.Diagnostics.Debug.WriteLine("データ文字列から抵抗値の取得に失敗しました。{0}", strValue)
            '    Exit Do
            'End If

            '' 目標値
            'If Double.TryParse(StatData(16).Label.Text.Substring(0, StatData(16).Label.Text.Length - 1), dTarget) = False Then
            '    System.Diagnostics.Debug.WriteLine("目標値の変換に失敗しました。{0}", StatData(16).Label.Text)
            '    Exit Do
            'End If

            ' データ文字列から抵抗値を取得
            bRet = False
            For i = 0 To strUnit.Length - 1 Step 1
                nIndex = strValue.IndexOf(strUnit(i))
                If nIndex < 0 Then
                    Continue For
                End If

                bRet = Double.TryParse(strValue.Substring(0, nIndex), dValue)
                If bRet = True Then
                    If i = 0 Then
                        dValue = dValue * 1000000.0
                    ElseIf i = 1 Then
                        dValue = dValue * 1000.0
                    ElseIf i = 2 Then
                        dValue = dValue * 1.0
                    ElseIf i = 3 Then
                        dValue = dValue / 1000.0
                    End If
                    Exit For
                End If
            Next i

            If bRet = False Then
                System.Diagnostics.Debug.WriteLine("データ文字列から抵抗値の取得に失敗しました。{0}", strValue)
            End If

            ' 目標値
            bRet = False
            For i = 0 To strUnit.Length - 1 Step 1
                'V4.11.0.0⑮↓
                If (giTargetOfs = 1) Then
                    tempTarget = TrimData.GetChangeOhmUnit(typResistorInfoArray(1).dblTrimTargetVal)
                    nIndex = tempTarget.IndexOf(strUnit(i))
                    If nIndex < 0 Then
                        Continue For
                    End If
                Else
                    nIndex = StatData(16).Label.Text.IndexOf(strUnit(i))
                    If nIndex < 0 Then
                        Continue For
                    End If
                    tempTarget = StatData(16).Label.Text
                End If

                bRet = Double.TryParse(tempTarget.Substring(0, tempTarget.IndexOf(strUnit(i))), dTarget)
                'V4.11.0.0⑮↑
                If bRet = True Then
                    If i = 0 Then
                        dTarget = dTarget * 1000000.0
                    ElseIf i = 1 Then
                        dTarget = dTarget * 1000.0
                    ElseIf i = 2 Then
                        dTarget = dTarget * 1.0
                    ElseIf i = 3 Then
                        dTarget = dTarget / 1000.0
                    End If
                    Exit For
                End If
            Next i

            If bRet = False Then
                System.Diagnostics.Debug.WriteLine("目標値の変換に失敗しました。{0}", StatData(16).Label.Text)
                Exit Do
            End If
            '----- V4.0.0.0⑳↑ -----

            If nType = 0 Then   ' イニシャルテスト
                If Double.TryParse(StatData(21).Label.Text.Substring(0, StatData(21).Label.Text.Length - 1), dTmp) = False Then
                    System.Diagnostics.Debug.WriteLine("イニシャルテストの下限値の変換に失敗しました。{0}", StatData(21).Label.Text)
                    dTmp = 0.0
                End If

                dMin = dTarget + (dTarget * (dTmp / 100.0))

                If Double.TryParse(StatData(22).Label.Text.Substring(0, StatData(22).Label.Text.Length - 1), dTmp) = False Then
                    System.Diagnostics.Debug.WriteLine("イニシャルテストの上限値の変換に失敗しました。{0}", StatData(22).Label.Text)
                    dTmp = 0.0
                End If

                dMax = dTarget + (dTarget * (dTmp / 100.0))

            Else                ' ファイナルテスト
                If Double.TryParse(StatData(24).Label.Text.Substring(0, StatData(24).Label.Text.Length - 1), dTmp) = False Then
                    System.Diagnostics.Debug.WriteLine("ファイナルテストの下限値の変換に失敗しました。{0}", StatData(24).Label.Text)
                    dTmp = 0.0
                End If

                dMin = dTarget + (dTarget * (dTmp / 100.0))

                If Double.TryParse(StatData(25).Label.Text.Substring(0, StatData(25).Label.Text.Length - 1), dTmp) = False Then
                    System.Diagnostics.Debug.WriteLine("ファイナルテストの上限値の変換に失敗しました。{0}", StatData(25).Label.Text)
                    dTmp = 0.0
                End If

                dMax = dTarget + (dTarget * (dTmp / 100.0))

            End If

            ' 範囲チェック
            'System.Diagnostics.Debug.WriteLine("範囲チェック({0}) {1}≦{2}≦{3}", nType, dMin, dValue, dMax)
            If (dMin <= dValue) And (dValue <= dMax) Then
                IsResistorRange = True
            End If

        Loop While False

    End Function
#End Region
    ' ↑↑↑ V3.1.0.0④ 2014/12/05
#Region "シンプルトリマグラフ表示用フォーム初期化"
    '''=========================================================================
    '''<summary>シンプルトリマグラフ表示用フォーム初期化</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub InitializeFormGraphPanel()
        Dim strMSG As String

        Try
            ' 分布図表示用ラベル配列の初期化
            gDistRegNumLblAry(0) = Form1.LblRegN_00              ' 分布グラフ抵抗数配列(0～11)
            gDistRegNumLblAry(1) = Form1.LblRegN_01
            gDistRegNumLblAry(2) = Form1.LblRegN_02
            gDistRegNumLblAry(3) = Form1.LblRegN_03
            gDistRegNumLblAry(4) = Form1.LblRegN_04
            gDistRegNumLblAry(5) = Form1.LblRegN_05
            gDistRegNumLblAry(6) = Form1.LblRegN_06
            gDistRegNumLblAry(7) = Form1.LblRegN_07
            gDistRegNumLblAry(8) = Form1.LblRegN_08
            gDistRegNumLblAry(9) = Form1.LblRegN_09
            gDistRegNumLblAry(10) = Form1.LblRegN_10
            gDistRegNumLblAry(11) = Form1.LblRegN_11

            gDistGrpPerLblAry(0) = Form1.LblGrpPer_00           ' 分布グラフ%配列(0～11)
            gDistGrpPerLblAry(1) = Form1.LblGrpPer_01
            gDistGrpPerLblAry(2) = Form1.LblGrpPer_02
            gDistGrpPerLblAry(3) = Form1.LblGrpPer_03
            gDistGrpPerLblAry(4) = Form1.LblGrpPer_04
            gDistGrpPerLblAry(5) = Form1.LblGrpPer_05
            gDistGrpPerLblAry(6) = Form1.LblGrpPer_06
            gDistGrpPerLblAry(7) = Form1.LblGrpPer_07
            gDistGrpPerLblAry(8) = Form1.LblGrpPer_08
            gDistGrpPerLblAry(9) = Form1.LblGrpPer_09
            gDistGrpPerLblAry(10) = Form1.LblGrpPer_10
            gDistGrpPerLblAry(11) = Form1.LblGrpPer_11

            gDistShpGrpLblAry(0) = Form1.LblShpGrp_00                      ' 分布グラフ配列(0～11)
            gDistShpGrpLblAry(1) = Form1.LblShpGrp_01
            gDistShpGrpLblAry(2) = Form1.LblShpGrp_02
            gDistShpGrpLblAry(3) = Form1.LblShpGrp_03
            gDistShpGrpLblAry(4) = Form1.LblShpGrp_04
            gDistShpGrpLblAry(5) = Form1.LblShpGrp_05
            gDistShpGrpLblAry(6) = Form1.LblShpGrp_06
            gDistShpGrpLblAry(7) = Form1.LblShpGrp_07
            gDistShpGrpLblAry(8) = Form1.LblShpGrp_08
            gDistShpGrpLblAry(9) = Form1.LblShpGrp_09
            gDistShpGrpLblAry(10) = Form1.LblShpGrp_10
            gDistShpGrpLblAry(11) = Form1.LblShpGrp_11

            'V4.0.0.0⑫↓
            gGoodChip = Form1.lblGoodChip2
            gNgChip = Form1.lblNgChip2
            gMaxValue = Form1.lblMaxValue2
            gMinValue = Form1.lblMinValue2
            gAverageValue = Form1.lblAverageValue2
            gDeviationValue = Form1.lblDeviationValue2
            gGraphAccumulationTitle = Form1.lblGraphAccumulationTitle2
            gRegistUnit = Form1.lblRegistUnit2
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

#Region "次ブロックデータボタンを押したときの処理"
    '''=========================================================================
    ''' <summary>    'V4.0.0.0⑬
    ''' 次ブロックデータボタンを押したときの処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub BlockNextButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BlockNextButton.Click
        Try
#If False Then                          'V5.0.0.9⑯
            If (typPlateInfo.intResistDir = 0) Then                     ' 抵抗(ﾁｯﾌﾟ)並び方向(0:X, 1:Y)
                If typPlateInfo.intBlockCntYDir > BlockDisplayNumber Then
                    BlockDisplayNumber = BlockDisplayNumber + 1
                    Call SimpleTrimmer.ResistorDataDisp(True, BlockDisplayNumber, ResistorDisplayNumber)
                    BlockNoLabel.Text = BlockDisplayNumber
                End If
            Else
                If typPlateInfo.intBlockCntXDir > BlockDisplayNumber Then
                    BlockDisplayNumber = BlockDisplayNumber + 1
                    Call SimpleTrimmer.ResistorDataDisp(True, BlockDisplayNumber, ResistorDisplayNumber)
                    BlockNoLabel.Text = BlockDisplayNumber
                End If
            End If
#Else
            If ((typPlateInfo.intBlockCntXDir * typPlateInfo.intBlockCntYDir) > BlockDisplayNumber) Then
                BlockDisplayNumber = BlockDisplayNumber + 1
                SimpleTrimmer.ResistorDataDisp(True, BlockDisplayNumber, ResistorDisplayNumber)
                BlockNoLabel.Text = BlockDisplayNumber
            End If
#End If
        Catch ex As Exception
            MsgBox("SimpleTrimmer.BlockNextButton_Click() TRAP ERROR = " + ex.Message)
        End Try

    End Sub

#End Region

#Region "前ブロックデータボタンを押したときの処理"
    '''=========================================================================
    ''' <summary>    'V4.0.0.0⑬
    ''' 前ブロックデータボタンを押したときの処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub BlockRvsButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BlockRvsButton.Click
        If BlockDisplayNumber > 1 Then
            BlockDisplayNumber = BlockDisplayNumber - 1
            Call SimpleTrimmer.ResistorDataDisp(True, BlockDisplayNumber, ResistorDisplayNumber)
            BlockNoLabel.Text = BlockDisplayNumber
        End If
    End Sub
#End Region

#Region "ラベルのBlockNoを押したときの処理"
    '''=========================================================================
    ''' <summary>    'V4.0.0.0⑬
    ''' ラベルのBlockNoを押したときの処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub BlockNoLabel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BlockNoLabel.Click

    End Sub
#End Region

#Region "ラベルに表示するBlockNoの更新"
    '''=========================================================================
    ''' <summary>    'V4.0.0.0⑬
    ''' ラベルに表示するBlockNoの更新
    ''' </summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub SetBlockDisplayNumber(ByVal BlockNo As Integer,
                                     Optional ByVal doResistorDataDisp As Boolean = False) 'V5.0.0.9⑯
        'V5.0.0.9⑯    Public Sub SetBlockDisplayNumber(ByVal BlockNo As Integer)

        BlockDisplayNumber = BlockNo

        'V5.0.0.9⑯                  ↓
        If (doResistorDataDisp) AndAlso (BlockNoLabel.Visible) Then
            ' 該当ブロックの抵抗データを表示する
            ResistorDataDisp(True, BlockDisplayNumber, ResistorDisplayNumber)
            BlockNoLabel.Text = BlockDisplayNumber
        End If
        'V5.0.0.9⑯                  ↑
    End Sub

    '''=========================================================================
    ''' <summary>    'V4.3.0.0①
    ''' BlockNoの取得
    ''' </summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub GetBlockDisplayNumber(ByRef BlockNo As Integer)

        BlockNo = TrimData.GetBlockNumber()

    End Sub
#End Region

#Region "Lotno表示の更新"
    '''=========================================================================
    ''' <summary>    'V4.0.0.0-42
    ''' ラベルに表示するLotNoの更新
    ''' </summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub UpdateLotNo(ByVal Lotno As String)

        StatData(2).Label.Text = Lotno

    End Sub
#End Region

#Region "現在Lotのの取得"
    '''=========================================================================
    ''' <summary>    'V4.0.0.0-42
    ''' 現在Lotのの取得 
    ''' </summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub GetLotNo(ByRef Lotno As String)

        TrimData.GetLotNo(Lotno)

    End Sub
#End Region

    'V4.0.0.0-76
    Private Sub BlockMainButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BlockMainButton.Click
        'IDLE中のみ反応するように変更する
        If giAppMode = APP_MODE_IDLE Then 'V4.0.0.0-75
            Call SimpleTrimmer.ResistorDataDisplay(False, Integer.Parse(BlockDisplayNumber), 1)
            FrmBlockDisp.Close()
        End If
    End Sub

    ''' <summary>
    ''' メイン画面の一時停止ボタンからデータ編集画面の起動 'V4.0.0.0-84
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DataEditStart() As Integer

        Dim r As Integer
        Try
            ' データ編集プログラムを起動する(一時停止モード)
            r = Form1.ExecEditProgram(1)                                ' 非常停止等のエラー発生時はAPP終了するので戻ってこない 
            Call ZCONRST()                                              ' コンソールキーラッチ解除 ###226
            If (r <> cFRS_NORMAL) Then
                Return r                                            ' 一時停止画面処理終了へ
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
                Return cFRS_COM_FL_ERR
            End If
            '----- ###257↑ -----
            r = SendTrimData()                                          ' トリミングデータをINtime側に送信する
            If (r <> cFRS_NORMAL) Then
                ' "トリミングデータの設定に失敗しました。" & vbCrLf & "トリミングデータに問題がないか確認してください。"
                Call Form1.System1.TrmMsgBox(gSysPrm, MSGERR_SEND_TRIMDATA, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, TITLE_4)
                Return cFRS_FIOERR_INP
            End If
        Catch ex As Exception
            MsgBox("SimpleTrimmer.SimpleTrimmerInit() TRAP ERROR = " + ex.Message)
        End Try


    End Function

    ''' <summary>
    ''' 一時停止画面で、データ編集ボタンを押したときの処理 'V4.0.0.0-84
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub DataEditButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataEditButton.Click

        DataEditStart()

    End Sub

    ''' <summary>
    ''' コマンドボタンの切替え
    ''' </summary>
    ''' <param name="flg"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CommandEnableSet(ByVal flg As Boolean) As Integer

        If gKeiTyp <> KEY_TYPE_RS Then
            Return 0
        End If

        Form1.tabCmd.Enabled = flg

        Return 0

    End Function

    ''' <summary>
    ''' レーザオフをチェックする 'V4.0.0.0-86
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetLaserOffIO(modecheck As Boolean) As Integer 'V5.0.0.1⑫
        '    Public Function GetLaserOffIO() As Integer
        Dim r As Integer
        Dim bit As Integer
        Dim Adr As Integer
        Dim Dat As Integer
        Dim strMsg As String
        Dim digL As Integer
        Dim digH As Integer
        Dim digSW As Integer
        Dim ChkExec As Boolean 'V5.0.0.1⑫

        'V5.0.0.1⑫↓
        ChkExec = True
        If (gKeiTyp = KEY_TYPE_RS) Then
            If modecheck = True Then
                Call Form1.GetMoveMode(digL, digH, digSW)
                If ((digL = 0) Or (digL = 1) Or (digL = 5)) Then
                    ChkExec = True
                Else
                    ChkExec = False
                End If
            End If
            If ChkExec = True Then
                'V5.0.0.1⑫↑
                Adr = &H2102
                bit = &H10
                r = INP16(Adr, Dat)        ' // ' // データリード
                If (bit And Dat) Then
                    If (IsNothing(gObjADJ) = False) Then
                        gObjADJ.TopMost = False
                    End If
                    strMsg = SimpleTrimmer_039 ' LaserをONしてください
                    'V5.0.0.1⑨↓
                    '                    MsgBox(strMsg)
                    MsgBox(strMsg, MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxSetForeground)
                    'MessageBox.Show(strMsg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly)
                    '                    MessageBox.Show(Form1, strMsg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly)
                    If (IsNothing(gObjADJ) = False) Then
                        gObjADJ.TopMost = True
                    End If
                    'V5.0.0.1⑨↑
                    Return 1
                End If
            End If
        End If

        Return 0

    End Function

    ''' <summary>
    ''' プローブクリーニングボタンのVisible設定を行う
    ''' </summary>
    ''' <param name="mode"></param>
    ''' <remarks></remarks>
    Public Sub ProbeBtnVisibleSet(ByVal mode As Boolean)

        If IsNothing(CmdProbeClean) = False Then
            CmdProbeClean.Visible = mode
        End If

    End Sub

    ''' <summary>
    ''' プローブクリーニングボタンのEnable設定を行う
    ''' </summary>
    ''' <param name="mode"></param>
    ''' <remarks></remarks>
    Public Sub ProbeBtnEnableSet(ByVal mode As Boolean)

        If IsNothing(CmdProbeClean) = False Then
            CmdProbeClean.Enabled = mode
        End If

    End Sub

    ''' <summary>
    ''' I/O確認ボタンのVisible設定を行う
    ''' </summary>
    ''' <param name="mode"></param>
    ''' <remarks></remarks>
    Public Sub MaintBtnVisibleSet(ByVal mode As Boolean)
        If IsNothing(CmdMainte) = False Then
            CmdMainte.Visible = mode
        End If
    End Sub

    ''' <summary>
    ''' I/O確認ボタンのEnable設定を行う
    ''' </summary>
    ''' <param name="mode"></param>
    ''' <remarks></remarks>
    Public Sub MaintBtnEnableSet(ByVal mode As Boolean)
        If IsNothing(CmdMainte) = False Then
            CmdMainte.Enabled = mode

        End If

    End Sub

    ''' <summary>
    ''' ブロック番号表示の切替え
    ''' </summary>
    ''' <param name="flg"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function BlockNoBtnVisible(ByVal flg As Boolean) As Integer

        If gKeiTyp <> KEY_TYPE_RS Then
            Return 0
        End If

        '----- V4.11.0.0⑤↓ (WALSIN殿SL436S対応) -----
        ' トリミング開始ブロック番号指定の有効の場合はブロック数は表示しない
        'NowBlockNoLabel.Visible = flg
        If (giStartBlkAss = 0) Then
            NowBlockNoLabel.Visible = flg
        Else
            NowBlockNoLabel.Visible = False
        End If
        '----- V4.11.0.0⑤↑ -----

        Return 0

    End Function

#Region "ラベルに表示するBlockNoの更新"
    '''=========================================================================
    ''' <summary>    'V4.0.0.0⑬
    ''' ラベルに表示するBlockNoの更新
    ''' </summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub SetNowBlockDspNum(ByVal BlockNo As Integer)

        If gKeiTyp <> KEY_TYPE_RS Then
            Return
        End If

        NowBlockNoLabel.Text = "BlockNo:" + BlockNo.ToString() 'V4.2.0.0①

    End Sub
#End Region

#Region "生産管理情報のクリア"
    '''=========================================================================
    ''' <summary>生産管理情報のクリア V6.0.3.0⑨</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub ClearDispData()

        Debug.WriteLine("デバッグ・メッセージを出力DataClrButton_Click")
        TrimData.ResetLotData()
        TrimData.GetProductData(StatData(2).Label.Text, StatData(4).Label.Text, StatData(6).Label.Text, StatData(8).Label.Text, StatData(9).Label.Text, StatData(11).Label.Text, StatData(12).Label.Text, StatData(14).Label.Text)
        SimpleTrim_LoggingStart()
        CmdBlockData.Visible = False                                ' ［BLOCK DATA］ボタンを非常時にする。
        Call Form1.System1.OperationLogging(gSysPrm, MSG_OPLOG_CLRTOTAL, "MANUAL")
        Call Form1.ClearCounter(1)                                  ' 生産管理データのクリア
        Call ClrTrimPrnData()                                       ' ﾄﾘﾐﾝｸﾞ結果印刷項目のﾃﾞｰﾀをｸﾘｱする(ローム殿特注) V1.18.0.0③

        ' 統計表示がONの場合、表示を更新する
        'If Form1.chkDistributeOnOff.Checked = True Then
        gObjFrmDistribute.RedrawGraph()                             ' V4.0.0.0-81

    End Sub
#End Region

    'V4.8.0.1①↓
    ''' <summary>
    ''' 不良率表示を変更したときの処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CheckNGRate_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckNGRate.CheckedChanged

        If CheckNGRate.Checked = True Then
            giRateDisp = 1
        Else
            giRateDisp = 0
        End If
        TrimData.GetYieldData(giRateDisp, StatData(0).Label.Text)

    End Sub
    'V4.8.0.1①↑

    ''' <summary>
    ''' 画面に表示されているHi、Lo、Openの数を返す  'V4.9.0.0①
    ''' </summary>
    ''' <param name="LotPlate"></param>     // 0:ロット、1:基板
    ''' <param name="HiLoOpen"></param>     // 0:Lo-NG , 1:Hi-NG , 2:Open-NG
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetHiLoOpenCount(ByVal LotPlate As Integer, ByVal HiLoOpen As Integer, ByRef It As Double, ByRef Ft As Double) As Integer
        Dim strITCount As String
        Dim strFTCount As String

        GetHiLoOpenCount = 0
        If (HiLoOpen = UNIT_LO_NG) Then  ' Lo-NG

            If (LotPlate = UNIT_PLATE) Then
                ' 基板：初期測定、最終測定：Low-NG
                strITCount = StatData(45).Label.Text
                strFTCount = StatData(46).Label.Text
            Else
                ' Lot：初期測定、最終測定：Low-NG
                strITCount = StatData(47).Label.Text
                strFTCount = StatData(48).Label.Text
            End If

        ElseIf (HiLoOpen = UNIT_HI_NG) Then  ' Hi-NG 

            If (LotPlate = UNIT_PLATE) Then
                ' 基板：初期測定、最終測定：High-NG
                strITCount = StatData(50).Label.Text
                strFTCount = StatData(51).Label.Text
            Else
                ' Lot：初期測定、最終測定：High-NG
                strITCount = StatData(52).Label.Text
                strFTCount = StatData(53).Label.Text
            End If

        Else    'UNIT_OPEN_NG

            If (LotPlate = UNIT_PLATE) Then
                ' 基板：初期測定、最終測定：Open-NG:RangeOver
                strITCount = StatData(55).Label.Text
                strFTCount = StatData(56).Label.Text
            Else
                ' Lot：初期測定、最終測定：Open-NG:RangeOver
                strITCount = StatData(57).Label.Text
                strFTCount = StatData(58).Label.Text
            End If

        End If

        If Trim(strITCount) <> "" Then
            It = Val(strITCount)
        Else
            It = 0
            GetHiLoOpenCount = 1
        End If

        If Trim(strFTCount) <> "" Then
            Ft = Val(strFTCount)
        Else
            Ft = 0
            GetHiLoOpenCount = 1
        End If

    End Function

    ''' <summary>
    ''' NG率等からロットの中断を判断する  'V4.9.0.0①
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function JudgeLotStop() As Integer
        Dim IT As Double
        Dim FT As Double
        Dim strYield As String
        ' V5.0.0.1-30↓
        Dim digL As Integer
        Dim digH As Integer
        Dim digSW As Integer
        ' V5.0.0.1-30↑

        JudgeLotStop = cFRS_NORMAL
        strYield = ""

        ' V5.0.0.1-30↓
        Call Form1.GetMoveMode(digL, digH, digSW)           ' デジスイッチの読取り
        If (digL <> 0) And (digL <> 1) And (digL <> 2) Then
            Return JudgeLotStop
        End If
        ' V5.0.0.1-30↑

        ' Yield判定
        If JudgeNgRate.CheckYeld = True Then
            TrimData.GetYieldData(99, strYield)
            If strYield <> "" Then

                If Double.Parse(strYield) <= JudgeNgRate.ValYield Then
                    JudgeLotStop = 1
                    Return JudgeLotStop
                End If
            End If
        End If

        ' OverRange判定
        If JudgeNgRate.CheckOverRange = True Then
            GetHiLoOpenCount(UNIT_PLATE, UNIT_OPEN_NG, IT, FT)
            If IT >= JudgeNgRate.ValOverRange Then
                JudgeLotStop = 2
                Return JudgeLotStop
            End If
        End If

        ' IT-HI判定
        If JudgeNgRate.CheckITHI = True Then
            'If JudgeNgRate.SelectUnit = UNIT_BLOCK Then
            '    ' Block単位の集計で判定
            '    JudgeLotStop = 3
            '    Return JudgeLotStop

            'Else
            If JudgeNgRate.SelectUnit = UNIT_PLATE Then
                ' Plate単位の集計で判定
                GetHiLoOpenCount(UNIT_PLATE, UNIT_HI_NG, IT, FT)
                If IT >= JudgeNgRate.ValITHI Then
                    JudgeLotStop = 4
                    Return JudgeLotStop
                End If
            Else
                ' LOT単位の集計で判定
                GetHiLoOpenCount(UNIT_LOT, UNIT_HI_NG, IT, FT)
                If IT >= JudgeNgRate.ValITHI Then
                    JudgeLotStop = 5
                    Return JudgeLotStop
                End If

            End If
        End If

        ' IT-LO判定
        If JudgeNgRate.CheckITLO = True Then
            'If JudgeNgRate.SelectUnit = UNIT_BLOCK Then
            '    ' Block単位の集計で判定
            '    JudgeLotStop = 6
            '    Return JudgeLotStop

            'Else
            If JudgeNgRate.SelectUnit = UNIT_PLATE Then
                ' Plate単位の集計で判定
                GetHiLoOpenCount(UNIT_PLATE, UNIT_LO_NG, IT, FT)
                If IT >= JudgeNgRate.ValITLO Then
                    JudgeLotStop = 7
                    Return JudgeLotStop
                End If
            Else
                ' LOT単位の集計で判定
                GetHiLoOpenCount(UNIT_LOT, UNIT_LO_NG, IT, FT)
                If IT >= JudgeNgRate.ValITLO Then
                    JudgeLotStop = 8
                    Return JudgeLotStop
                End If

            End If
        End If

        ' FT-HI判定
        If JudgeNgRate.CheckFTHI = True Then
            'If JudgeNgRate.SelectUnit = UNIT_BLOCK Then
            '    ' Block単位の集計で判定
            '    JudgeLotStop = 9
            '    Return JudgeLotStop
            'Else
            If JudgeNgRate.SelectUnit = UNIT_PLATE Then
                ' Plate単位の集計で判定
                GetHiLoOpenCount(UNIT_PLATE, UNIT_HI_NG, IT, FT)
                If FT >= JudgeNgRate.ValFTHI Then
                    JudgeLotStop = 10
                    Return JudgeLotStop
                End If
            Else
                ' LOT単位の集計で判定
                GetHiLoOpenCount(UNIT_LOT, UNIT_HI_NG, IT, FT)
                If FT >= JudgeNgRate.ValFTHI Then
                    JudgeLotStop = 11
                    Return JudgeLotStop
                End If

            End If
        End If

        ' FT-LO判定
        If JudgeNgRate.CheckFTLO = True Then
            'If JudgeNgRate.SelectUnit = UNIT_BLOCK Then
            '    ' Block単位の集計で判定
            '    JudgeLotStop = 12
            '    Return JudgeLotStop
            'Else
            If JudgeNgRate.SelectUnit = UNIT_PLATE Then
                ' Plate単位の集計で判定
                GetHiLoOpenCount(UNIT_PLATE, UNIT_LO_NG, IT, FT)
                If FT >= JudgeNgRate.ValFTLO Then
                    JudgeLotStop = 13
                    Return JudgeLotStop
                End If
            Else
                ' LOT単位の集計で判定
                GetHiLoOpenCount(UNIT_LOT, UNIT_LO_NG, IT, FT)
                If FT >= JudgeNgRate.ValFTLO Then
                    JudgeLotStop = 14
                    Return JudgeLotStop
                End If

            End If
        End If

    End Function


    ''' <summary>
    ''' 集計をクリアする関数  'V4.9.0.0①
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ClearTotalCount() As Integer

        Debug.WriteLine("デバッグ・メッセージを出力DataClrButton_Click")
        TrimData.ResetLotData()
        'V4.9.0.0④
        TrimData.SetLaserPower(0.0)
        TrimData.GetProductData(StatData(2).Label.Text, StatData(4).Label.Text, StatData(6).Label.Text, StatData(8).Label.Text, StatData(9).Label.Text, StatData(11).Label.Text, StatData(12).Label.Text, StatData(14).Label.Text)
        SimpleTrim_LoggingStart()
        CmdBlockData.Visible = False         ' ［BLOCK DATA］ボタンを非常時にする。
        Call Form1.System1.OperationLogging(gSysPrm, MSG_OPLOG_CLRTOTAL, "MANUAL")
        Call Form1.ClearCounter(1)                              ' 生産管理データのクリア
        Call ClrTrimPrnData()                                   ' ﾄﾘﾐﾝｸﾞ結果印刷項目のﾃﾞｰﾀをｸﾘｱする(ローム殿特注) V1.18.0.0③

        ' 統計表示がONの場合、表示を更新する
        'If Form1.chkDistributeOnOff.Checked = True Then
        gObjFrmDistribute.RedrawGraph() 'V4.0.0.0-81

    End Function

    ''V5.0.0.4⑤↓
    ''' <summary>
    ''' 抵抗ごとの判定色表示用
    ''' </summary>
    ''' <returns>True:正常、False異常</returns>
    ''' <remarks></remarks>
    Public Function IsTrimJudge(ByVal mode As Integer, ByVal ResNo As Integer) As Boolean

        IsTrimJudge = False
        If (mode = 0) Then      ' Initial 
            If ((gwTrimResult(ResNo - 1) = TRIM_RESULT_OK) Or (gwTrimResult(ResNo - 1) = TRIM_RESULT_FT_HING) Or (gwTrimResult(ResNo - 1) = TRIM_RESULT_FT_LONG)) Then
                IsTrimJudge = True
            End If
        Else        ' Final 
            If (gwTrimResult(ResNo - 1) = TRIM_RESULT_OK) Then
                IsTrimJudge = True
            End If
        End If

    End Function
    ''V5.0.0.4⑤↑

#If START_KEY_SOFT Then
#Region "ソフトSTART、RESETボタン処理"


    Private Sub SoftStartButton_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles SoftStartButton.PreviewKeyDown
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            e.IsInputKey = True
            Form1.CbDigSwH.Focus()
        End If
    End Sub

    Private Sub SoftStartButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SoftStartButton.Click
        Try
#If START_KEY_SOFT Then
            If gbStartKeySoft Then
                Call START_SWITCH_ON()
            End If
#End If
        Catch ex As Exception
            MsgBox("SimpleTrimmer.SoftStartButton_Click() TRAP ERROR = " + ex.Message)
        End Try

    End Sub

    Private Sub SoftResetButton_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles SoftResetButton.PreviewKeyDown
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            e.IsInputKey = True
            Form1.CbDigSwH.Focus()
        End If
    End Sub

    Private Sub SoftResetButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SoftResetButton.Click
        Try
#If START_KEY_SOFT Then
            If gbStartKeySoft Then
                Call RESET_SWITCH_ON()
            End If
#End If
        Catch ex As Exception
            MsgBox("SimpleTrimmer.SoftResetButton_Click() TRAP ERROR = " + ex.Message)
        End Try

    End Sub
#End Region

#Region "ソフトボタン表示非表示"
    Public Sub Sub_StartResetButtonDispOn()
        Try
            SoftStartButton.Enabled = True
            SoftStartButton.Visible = True
            SoftResetButton.Enabled = True
            SoftResetButton.Visible = True
        Catch ex As Exception
            MsgBox("SimpleTrimmer.SoftStartButtonDispON() TRAP ERROR = " + ex.Message)
        End Try
    End Sub
    Public Sub Sub_StartResetButtonDispOff()
        Try
            SoftStartButton.Enabled = False
            SoftStartButton.Visible = False
            SoftResetButton.Enabled = False
            SoftResetButton.Visible = False
        Catch ex As Exception
            MsgBox("SimpleTrimmer.SoftStartButtonDispOFF() TRAP ERROR = " + ex.Message)
        End Try
    End Sub
#End Region

#End If

End Module


'=============================== END OF FILE ===============================