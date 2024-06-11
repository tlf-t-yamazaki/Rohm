'===============================================================================
'   Description  : データ選択画面処理(ロット切り替え自動運転用)
'                  KOA EW特注版のFormDataSelectをFormDataSelect2として流用
'                  
'   Copyright(C) : OMRON LASERFRONT INC. 2011
'           
'===============================================================================
Imports TKY_ALL_SL432HW.My.Resources                ' V4.4.0.0-0
Imports System.Text                                 ' For StringBuilder()               V6.1.4.0①
Imports System.Collections.Generic                  ' For List()                        V6.1.4.0①
Imports System.IO                                   ' For FileStream()                  V6.1.4.0①
Imports System.Globalization                        ' For CultureInfo他                 V6.1.4.0①
Imports LaserFront.Trimmer.DefTrimFnc               ' For ZCONRST()他                   V6.1.4.0①
Imports LaserFront.Trimmer.TrimData.DataManager     ' For トリムデータ他　              V6.1.4.0①
Imports LaserFront.Trimmer.DefWin32Fnc              ' For GetPrivateProfileString_S()   V6.1.4.0①

Public Class FormDataSelect2
#Region "【変数定義】"
    '===========================================================================
    '   変数定義
    '===========================================================================
    Private Const DATA_DIR_PATH As String = "C:\TRIMDATA\DATA"          ' データファイルフォルダ(既定値)
    Private Const ENTRY_PATH As String = "C:\TRIMDATA\ENTRYLOT\"
    Private Const ENTRY_TMP_FILE As String = "SAVE_ENTRY.TMP"
    Private Const OPERATOR_PATH As String = "C:\TRIMDATA\OPERATOR\"     ' オペレータエントリーリスト格納フォルダ  'V6.1.4.0_34
    Private Const OPERATOR_LIST_FILE As String = "OPERATOR_LIST.TXT"    ' オペレータエントリーリスト              'V6.1.4.0_34

    'V6.1.4.0⑤        ↓
    Private Const _resultPath As String = "C:\TRIMDATA\RESULTDATA\"
    Private Shared ReadOnly Property RESULT_PATH() As String
        Get
            If (False = IO.Directory.Exists(_resultPath)) Then
                IO.Directory.CreateDirectory(_resultPath)
            End If
            Return _resultPath
        End Get
    End Property

    Private Shared _resultFile As String
    Private Shared ReadOnly Property RESULT_FILE() As String
        Get
            If (String.IsNullOrEmpty(_resultFile)) Then
                _resultFile = RESULT_PATH & "RESULTDATA" & Date.Now.ToString("yyyyMMddHHmmss") & ".TXT"
            End If
            Return _resultFile
        End Get
    End Property
    Private Shared _resultWritten As Boolean
    'V6.1.4.0⑤        ↑

    'V6.1.4.0_34↓
#Region "選択されたオペレータ"
    Private Shared _operator As String
    '''=========================================================================
    ''' <summary>選択されたオペレータ</summary>
    ''' <returns>選択されたオペレータ</returns>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Property [Operator] As String
        Get
            Return _operator
        End Get
        Set(value As String)
            _operator = value
            ToolTipOperator.SetToolTip(ComboBoxOperatorList, value)
        End Set
    End Property
#End Region
    'V6.1.4.0_34↑
    '----- (KOA EW殿SL432RD対応)【ロット切替え機能】-----
    '----- 連続運転用(SL432R用) -----                       ' V6.1.4.0②
    Public gbFgAutoOperation432 As Boolean = False          ' 自動運転フラグ(True:自動運転中, False:自動運転中でない)
    Public giAutoDataFileNum432 As Integer = 0              ' 連続運転登録デーファイル数

    '----- 変数定義 -----
    Private mExitFlag As Integer                            ' 終了フラグ
    Private m_mainEdit As Form1                             ' ﾒｲﾝ画面への参照
    Private sLogFileName As String
    Private sPlateDataFileName As String
    Private InitiallNgCount As Long
    Private NowlNgCount As Long
    Private bLotChange As Boolean
    Private NowExecuteLotNo As Integer
    Private AutoOpeCancel As Boolean
    Private CancelReason As Integer
    Private Const NO_MORE_ENTRY As Integer = 1
    Private Const LOAD_FAILED As Integer = 2
    Private sSavePLTNUM As String, sSaveREGNUM As String
    Private sSaveGO As String, sSaveNG As String
    Private sSaveITHING As String, sSaveITLONG As String, sSaveITHINGP As String, sSaveITLONGP As String
    Private sSaveFTLONG As String, sSaveFTHING As String, sSaveFTHINGP As String, sSaveFTLONGP As String
    Private sSaveOVER As String, sSaveNGPER As String, sSaveOVERP As String

#End Region

#Region "コンストラクタ"
    '''=========================================================================
    ''' <summary>コンストラクタ</summary>
    ''' <param name="mainEdit">(INP)メイン画面</param>
    '''=========================================================================
    Friend Sub New(ByRef mainEdit As Form1)

        ' この呼び出しは、Windows フォーム デザイナで必要です。
        InitializeComponent()

        ' InitializeComponent() 呼び出しの後で初期化を追加します。
        m_mainEdit = mainEdit ' ﾒｲﾝ画面への参照を設定

        ' Me.Operator と ComboBoxOperatorList.SelectedValue を紐つける      'V6.1.4.0_34
        ComboBoxOperatorList.DataBindings.Add("SelectedValue", Me, "Operator", True, DataSourceUpdateMode.OnPropertyChanged)    'V6.1.4.0_34
    End Sub
#End Region

#Region "【メソッド定義】"
#Region "終了結果を返す"
    '''=========================================================================
    ''' <summary>終了結果を返す</summary>
    ''' <returns>cFRS_ERR_START = OKボタン押下
    '''          cFRS_ERR_RST   = Cancelボタン押下</returns>
    '''=========================================================================
    Public Function sGetReturn() As Integer

        Dim strMSG As String

        Try
            Return (mExitFlag)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormDataSelect2.sGetReturn() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Function
#End Region

#Region "ShowDialogメソッドに独自の引数を追加する"
    '''=========================================================================
    ''' <summary>ShowDialogメソッドに独自の引数を追加する</summary>
    ''' <param name="Owner">(INP)未使用</param>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Overloads Sub ShowDialog(ByVal Owner As IWin32Window)

        Dim strMSG As String

        Try
            ' 初期処理
            mExitFlag = -1                                              ' 終了フラグ = 初期化

            ' 画面表示
            Me.ShowDialog()                                             ' 画面表示
            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormDataSelect2.ShowDialog() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "Form_Load時処理"
    '''=========================================================================
    ''' <summary>Form_Load時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub FormDataSelect2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim strMSG As String

        Try
#If cOFFLINEcDEBUG Then
            Me.TopMost = False
#End If
            ' フォーム名 
            Me.Text = MSG_AUTO_14                                       ' "データ登録"

            ' ラベル名・ボタン名 
            LblDataFile.Text = MSG_AUTO_05                              ' "データファイル"
            LblListList.Text = MSG_AUTO_06                              ' "登録済みデータファイル"
            BtnUp.Text = MSG_AUTO_07                                    ' "リストの1つ上へ"
            BtnDown.Text = MSG_AUTO_08                                  ' "リストの1つ下へ"
            BtnDelete.Text = MSG_AUTO_09                                ' "リストから削除"
            BtnClear.Text = MSG_AUTO_10                                 ' "リストをクリア"
            BtnSelect.Text = MSG_AUTO_11                                ' "↓登録↓"
            BtnOK.Text = MSG_AUTO_12                                    ' "OK"
            BtnCancel.Text = MSG_AUTO_13                                ' "キャンセル"

            ' リストボックスクリア
            Call ListList.Items.Clear()                                 ' 「登録済みデータファイル」リストボックスクリア

            ' 「データファイル」リストボックスに日付付きファイル名を表示する
            DrvListBox.Drive = "C:"                                     ' ドライブ 
            DirListBox.Path = DATA_DIR_PATH                             ' ディレクトリリストボックス既定値
            'V6.1.4.14②↓' ＮＥＴの時のディレクトリリストボックス既定値
            If gTkyKnd = KND_NET Then
                DirListBox.Path = GetPrivateProfileString_S("QR_CODE", "TKYNET_TRIMMING_DATA_FOLDER", "C:\TRIM\tky.ini", "C:\TRIMDATA\DATA_NET\")
            End If
            'V6.1.4.14②↑
            MakeFileList()                                              ' ←通常はDirListBox_Change()イベントが発生するので不要だが
            '                                                           ' カレントが"C:\TRIMDATA\DATA"だと発生しないので必要

            ' 登録済みﾃﾞｰﾀﾌｧｲﾙﾌｫﾙﾀﾞの有無を確認する
            If (False = Directory.Exists(ENTRY_PATH)) Then
                Directory.CreateDirectory(ENTRY_PATH)                   ' ﾌｫﾙﾀﾞが存在しなければ作成する
            End If

            Call LoadPlateDataFileFullPath()

            'V6.1.4.0_34↓
            If gbQRCodeReaderUse Then
                ' オペレータエントリーリストのエラーでMe.Close()すると
                ' Shown()が実行されずにClosed()が実行されるため、frmDistVisibleに不整合がおきるのでこちらへ移動
                If (Globals_Renamed.gObjFrmDistribute IsNot Nothing) AndAlso
                    (False = Globals_Renamed.gObjFrmDistribute.IsDisposed) Then
                    frmDistVisible = Globals_Renamed.gObjFrmDistribute.Visible
                    Globals_Renamed.gObjFrmDistribute.Visible = False
                Else
                    frmDistVisible = False
                End If

                ' オペレータ選択コンボボックスの設定をおこなう
                If (False = InitializeComboBoxOperatorList()) Then
                    ' オペレータエントリーリストを確認してください。
                    MessageBox.Show(Me, MSG_AUTO_23 & Environment.NewLine & OPERATOR_PATH & OPERATOR_LIST_FILE,
                                    Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    mExitFlag = cFRS_ERR_RST
                    Me.Close()
                End If
            Else
                PanelComboBoxBorder.Enabled = False
                PanelComboBoxBorder.Visible = False
                ComboBoxOperatorList.Enabled = False
                ComboBoxOperatorList.Visible = False
            End If
            'V6.1.4.0_34↑

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormDataSelect2.FormDataSelect_Load() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

    '========================================================================================
    '   ボタン押下時の処理
    '========================================================================================
#Region "ﾃﾞｰﾀ設定ﾎﾞﾀﾝ・編集ﾎﾞﾀﾝ押下時処理"
    '''=========================================================================
    ''' <summary>ﾃﾞｰﾀ設定ﾎﾞﾀﾝ</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub cmdLotInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdLotInfo.Click
        'V6.1.4.0②        Call LoadAndEditData(0)
    End Sub

    '''=========================================================================
    ''' <summary>編集ﾎﾞﾀﾝ押下時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub cmdEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEdit.Click
        ' パスワード入力
        Try                             'V6.1.4.0②
            Me.Enabled = False

            Me.TopMost = False      'V4.7.3.5①
            If (FormMain.Func_Password(F_EDIT) <> True) Then         ' パスワード入力ｴﾗｰならEXIT
                Return
            End If

            Call LoadAndEditData(1)

            MakeFileList()              ' DirListBoxで選択されているﾌｫﾙﾀﾞに保存された場合、ﾘｽﾄの更新が必要   'V6.1.4.0⑭

            Me.TopMost = True       'V4.7.3.5①

        Finally
            Application.DoEvents()      ' 編集画面が表示されるまでの間のﾎﾞﾀﾝ2度押し対策
            Me.Enabled = True
        End Try

    End Sub
#End Region

#Region "選択中の登録済みﾃﾞｰﾀﾌｧｲﾙを読み込んでﾃﾞｰﾀ設定画面または編集画面を開く"
    '''=========================================================================
    ''' <summary>選択中の登録済みﾃﾞｰﾀﾌｧｲﾙを読み込んでﾃﾞｰﾀ設定画面または編集画面を開く</summary>
    ''' <param name="button">0=ﾃﾞｰﾀ設定ﾎﾞﾀﾝ,1=編集ﾎﾞﾀﾝ</param>
    '''=========================================================================
    Private Sub LoadAndEditData(ByVal button As Integer)
        Dim rslt As Boolean
        'Dim s As String
        Dim r As Integer

        ' 登録済みのﾃﾞｰﾀﾌｧｲﾙがなければNOP
        If (ListList.Items.Count < 1) OrElse (ListList.SelectedIndex < 0) Then Exit Sub
        Try
            '-----------------------------------------------------------------------
            '   初期処理
            '-----------------------------------------------------------------------
            giAppMode = APP_MODE_LOAD                       ' ｱﾌﾟﾘﾓｰﾄﾞ = ファイルロード(F1)

            ' パスワード入力(オプション)
            rslt = FormMain.Func_Password(F_LOAD)
            If (rslt <> True) Then
                Exit Try                                    ' ﾊﾟｽﾜｰﾄﾞ入力ｴﾗｰならEXIT
            End If

            ' ﾃﾞｰﾀﾌｧｲﾙ名設定
            Dim gsDataFileName As String                    'V6.1.4.0②
            With ListList
                gsDataFileName = (ENTRY_PATH & Convert.ToString(.Items(.SelectedIndex)))
            End With
#If False Then  'V6.1.4.0②
            ' 旧設定の装置の電圧をOFFする
            r = V_Off()                                     ' DC電源装置 電圧OFF処理

            ' トリミングデータ設定
            r = UserVal()                                   ' データ初期設定
            If (r <> 0) Then                                ' エラー ?
                gLoadDTFlag = False                           ' データロード済フラグ = False
                s = "Data load Error : " & gsDataFileName & vbCrLf
                Me.LblFullPath.Text = s
                Call _frmMain.Z_PRINT(s)
            Else
                _frmMain.txtLog.Text = "  "               ' データロードでログ画面クリア
                gDspCounter = 0                             ' ログ画面表示基板枚数カウンタクリア
                gLoadDTFlag = True                            ' データロード済フラグ = True
                s = "Data loaded : " & gsDataFileName & vbCrLf
                Call _frmMain.Z_PRINT(s)

                Call _frmMain.System1.OperationLogging( _
                        gSysPrm, MSG_OPLOG_FUNC01, "File='" & gsDataFileName & "' MANUAL")

                ' ファイルパス名の表示
                If (gSysPrm.stTMN.giMsgTyp = 0) Then
                    _frmMain.LblDataFileName.Text = "データファイル名 " & gsDataFileName
                Else
                    _frmMain.LblDataFileName.Text = "File name " & gsDataFileName
                End If
                '-----------------------------------------------------------------------
                '   FL側へ加工条件を送信する(FL時で加工条件ファイルがある場合)
                '-----------------------------------------------------------------------
                Call _frmMain.SendFlParam(gsDataFileName)

                '###1040⑥                Call _frmMain.SetATTRateToScreen(True)    ' ###1040③ アッテネータの設定
            End If

            '-----------------------------------------------------------------------
            '   ﾛｰﾄﾞ終了処理
            '-----------------------------------------------------------------------
            ChDrive("C")                                    ' ChDriveしないと次起動時FDドライブを見に行って,
            ChDir(My.Application.Info.DirectoryPath)        ' "MVCutil.dllがない"となり起動できなくなる
#End If
            FormMain.Sub_FileLoad(gsDataFileName)           'V6.1.4.0②

            ' ======================================================================
            '   ﾃﾞｰﾀ設定画面・編集画面呼び出し
            ' ======================================================================
            ' ﾃﾞｰﾀﾛｰﾄﾞﾁｪｯｸ (ﾄﾘﾐﾝｸﾞﾃﾞｰﾀ初期設定:UserVal() のｴﾗｰﾁｪｯｸ)
            If gLoadDTFlag = False Then
                's = ConstMessage.MSG_45                    ' ﾃﾞｰﾀ未ﾛｰﾄﾞ V6.1.4.0① メッセージが変(MSG_45("加工条件データをロードしてください")
                'Call FormMain.Z_PRINT(s)                      '                    そもそもSub_FileLoad()でメッセージ表示済
                'Call Beep()
                Exit Try
            End If
#If False Then  'V6.1.4.0②
            If (0 = button) Then
                ' ﾃﾞｰﾀ設定画面
                giAppMode = APP_MODE_LOTNO                  ' ｱﾌﾟﾘﾓｰﾄﾞ = ロット番号設定中
                ' データ編集
                Call _frmMain.System1.OperationLogging(gSysPrm, MSG_OPLOG_LOTSET, "")

                Dim fLotInf As New FormEdit.frmLotInfoInput()
                fLotInf.ShowDialog(Me)
                fLotInf.Dispose()
            Else
#End If
            ' 編集画面
            giAppMode = APP_MODE_EDIT                   ' ｱﾌﾟﾘﾓｰﾄﾞ = 編集画面表示
            Call Form1.System1.OperationLogging(gSysPrm, MSG_OPLOG_FUNC03, "")

            'V6.1.4.0⑩            r = FormMain.ExecEditProgram(1)
            r = FormMain.ExecEditProgram(0)             'V6.1.4.0⑩ Mode１から０へ変更
#If False Then  'V6.1.4.0②
            FlgUpdGPIB = 0                              ' GPIBデータ更新Flag Off
            Dim fForm As New FormEdit.frmEdit           ' frmｵﾌﾞｼﾞｪｸﾄ生成
            fForm.ShowDialog()                          ' データ編集
            fForm.Dispose()                             ' frmｵﾌﾞｼﾞｪｸﾄ開放

            ' GPIBデータ更新ならGPIB初期化を行う
            If (FlgUpdGPIB = 1) Then
                Call GPIB_Init()
            End If
            'End If

            If (True = FlgUpd) Then
                '-----------------------------------------------------------------------
                '   データファイルをセーブする
                '-----------------------------------------------------------------------
                If rData_save(gsDataFileName) <> 0 Then       ' データファイルセーブ
                    Exit Try
                Else
                    Call _frmMain.Z_PRINT("Data saved : " & gsDataFileName & vbCrLf)
                End If

                '-----------------------------------------------------------------------
                '   操作ログ等を出力する
                '-----------------------------------------------------------------------
                Call _frmMain.System1.OperationLogging( _
                    gSysPrm, MSG_OPLOG_FUNC02, "File='" & gsDataFileName & "' MANUAL")

                FlgUpd = Convert.ToInt16(TriState.False)    ' データ更新 Flag OFF
            End If

            ChDrive("C")                                    ' ChDriveしないと次起動時FDドライブを見に行って,"MVCutil.dllがない"となり起動できなくなる
            ChDir(My.Application.Info.DirectoryPath)
#End If
            ' トラップエラー発生時
        Catch ex As Exception
            MsgBox("FormDataSelect2.LoadAndEditData() TRAP ERROR = " + ex.Message)
        Finally
            Call ZCONRST()                                  ' ｺﾝｿｰﾙｷｰ ﾗｯﾁ解除
            giAppMode = APP_MODE_LOTCHG                     ' ｱﾌﾟﾘﾓｰﾄﾞ = ロット切替
        End Try

    End Sub
#End Region

#Region "OKボタン押下時処理"
    '''=========================================================================
    ''' <summary>OKボタン押下時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnOK.Click

        Dim Idx As Integer
        Dim strMSG As String = ""

        Try
            gbFgAutoOperation432 = False            ' 自動運転フラグ = False(自動運転中でない)
            SaveTrimLoggingData()                   ' 現在の生産管理情報を退避する

            ' 選択リスト1以上有りかチェックする ?
            If (ListList.Items.Count < 1) Then
                '"データファイルを選択してください。"
                Call MsgBox(MSG_AUTO_18, MsgBoxStyle.OkOnly)
                Exit Sub
            End If

            'V6.1.4.0_34↓' オペレータが「未選択」ではないか                  
            If gbQRCodeReaderUse And (0 = ComboBoxOperatorList.SelectedIndex) Then
                ' オペレータを選択してください。
                MessageBox.Show(Me, MSG_AUTO_21, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                ComboBoxOperatorList.Select()
                Exit Sub
            End If
            'V6.1.4.0_34↑

#If cOSCILLATORcFLcUSE Then
        Dim r As Integer
        Dim strDAT As String
            ' 選択データに対応する加工条件ファイルが存在するかチェックする(FL時)
            If (gSysPrm.stRAT.giOsc_Res = OSCILLATOR_FL) Then
                For Idx = 0 To ListList.Items.Count - 1
                    strDAT = (ENTRY_PATH & ListList.Items(Idx))
                    r = GetFLCndFileName(strDAT, strMSG, True)              ' 存在チェック 
                    If (r <> SerialErrorCode.rRS_OK) Then                   ' 加工条件ファイルが存在しない ?
                        ' "加工条件ファイルが存在しません。(加工条件ファイル名)"
                        strMSG = MSG_AUTO_20 + "(" + strMSG + ")"
                        Call MsgBox(strMSG, MsgBoxStyle.OkOnly, "")
                        ListList.SelectedIndex = Idx
                        Call ListList_SelectedIndexChanged(sender, e)                      ' データファイル名をフルパスでラベルテキストボックスに設定する
                        Exit Sub
                    End If
                Next Idx
            End If
#End If
            ' 連続運転用のデータファイル数とデータファイル名配列をグローバル領域に設定する
            giAutoDataFileNum432 = ListList.Items.Count                    ' データファイル数
            ReDim gsAutoDataFileFullPath(giAutoDataFileNum432 - 1)
            For Idx = 0 To giAutoDataFileNum432 - 1                        ' データファイル名
                gsAutoDataFileFullPath(Idx) = (ENTRY_PATH & Convert.ToString(ListList.Items(Idx)))
            Next

            If (cFRS_NORMAL <> m_mainEdit.Sub_FileLoad(gsAutoDataFileFullPath(0))) Then   'V6.1.4.0②
                ' "自動運転時トリミングデータファイルＬＯＡＤエラー = [データファイル名]"
                strMSG = MSG_AUTO_25 + "= [" + gsAutoDataFileFullPath(0) + "]" + vbCrLf
                Call m_mainEdit.Z_PRINT(strMSG)
                Exit Sub
            Else
                Call InitialAutoOperation()
                Call m_mainEdit.ClearCounter(1)     ' 生産管理データのクリア
                ' 統計表示がONの場合、表示を更新する
                If Form1.chkDistributeOnOff.Checked = True Then
                    gObjFrmDistribute.RedrawGraph()
                End If
                gbFgAutoOperation432 = True
                giAutoCalibCounter = 0                                  'V6.1.4.2①トリミングカット位置ズレ暫定ソフト[自動キャリブレーション補正実行] 一番最初に実行する。
                giAutoCalibPlateCounter = 0                             'V6.1.4.2①トリミングカット位置ズレ暫定ソフト[自動キャリブレーション補正実行] 処理基板を初期化
                'V6.1.4.0_35↓
                ' レーザーパワーのモニタリングフラグ(0：無し,1：自動運転開始時,2：エントリーロット毎)
                If (giLaserrPowerMonitoring >= 1) Then
                    gbLaserPowerMonitoring = True
                Else
                    gbLaserPowerMonitoring = False
                End If
                'V6.1.4.0_35↑
            End If

            mExitFlag = cFRS_ERR_START                                  ' Return値 = OKボタン押下 

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormDataSelect2.BtnOK_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
        Me.Close()                                                      ' フォームを閉じる
    End Sub
#End Region

#Region "Cancelボタン押下時処理"
    '''=========================================================================
    ''' <summary>Cancelボタン押下時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click

        Dim strMSG As String

        Try
            gbFgAutoOperation432 = False

            mExitFlag = cFRS_ERR_RST                                    ' Return値 = Cancelボタン押下

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormDataSelect2.BtnCancel_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
        Me.Close()                                                      ' フォームを閉じる
    End Sub
#End Region

#Region "「リストの１つ上へ」ボタン押下時処理"
    '''=========================================================================
    ''' <summary>「リストの１つ上へ」ボタン押下時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnUp.Click

        Dim Idx As Integer
        Dim strMSG As String

        Try
            Idx = ListList.SelectedIndex
            ' 先頭が選択されている場合NOP
            If (Idx <= 0) Then Exit Sub
            Call SwapList(Idx, (Idx - 1))       ' ﾘｽﾄを入れ替え
            ListList.SelectedIndex = (Idx - 1)  ' １つ上のｲﾝﾃﾞｯｸｽを指定する

            ' データファイル名をフルパスでラベルテキストボックスに設定する
            Call ListList_SelectedIndexChanged(sender, e)
            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormDataSelect2.BtnUp_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "「リストの１つ下へ」ボタン押下時処理"
    '''=========================================================================
    ''' <summary>「リストの１つ下へ」ボタン押下時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnDown.Click

        Dim Idx As Integer
        Dim strMSG As String

        Try
            Idx = ListList.SelectedIndex
            ' 最後が選択されている場合NOP
            If ((Idx + 1) >= ListList.Items.Count) Then Exit Sub
            Call SwapList(Idx, (Idx + 1))       ' ﾘｽﾄを入れ替え
            ListList.SelectedIndex = (Idx + 1)  ' １つ下のｲﾝﾃﾞｯｸｽを指定する

            ' データファイル名をフルパスでラベルテキストボックスに設定する
            Call ListList_SelectedIndexChanged(sender, e)
            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormDataSelect2.BtnDown_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "登録済みﾃﾞｰﾀﾌｧｲﾙの項目を入れ替える"
    ''' <summary>登録済みﾃﾞｰﾀﾌｧｲﾙの項目を入れ替える</summary>
    ''' <param name="iSrc">元位置</param>
    ''' <param name="iDst">移動先位置</param>
    Private Sub SwapList(ByVal iSrc As Integer, ByVal iDst As Integer)
        Dim tmpStr As String
        tmpStr = Convert.ToString(ListList.Items(iSrc))
        ListList.Items.RemoveAt(iSrc)
        ListList.Items.Insert(iDst, tmpStr)

    End Sub
#End Region

#Region "「リストから削除」ボタン押下時処理"
    '''=========================================================================
    ''' <summary>「リストから削除」ボタン押下時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnDelete.Click

        Dim Idx As Integer
        Dim strMSG As String

        Try
            ' 「登録済みデータファイル」リストボックスから1つ削除する
            Idx = ListList.SelectedIndex
            If (Idx < 0) Then Exit Sub
            IO.File.Delete(Convert.ToString(ListList.Items(Idx)))       ' 選択されているﾌｧｲﾙを削除する
            ListList.Items.RemoveAt(Idx)                                '「登録済みデータファイル」リストボックスから1項目削除(※Remove()は文字列指定)

            ' データファイル名をフルパスでラベルテキストボックスに設定する(削除の１つ前のデータを選択状態とする)
            If (0 <= Idx) Then
                Idx = (Idx - 1)
                ' ﾘｽﾄの先頭が削除された場合に他のﾃﾞｰﾀがあれば選択する
                If (Idx < 0) AndAlso (0 < ListList.Items.Count) Then Idx = 0
                ListList.SelectedIndex = Idx    ' ｲﾍﾞﾝﾄにより登録済みﾃﾞｰﾀの選択中ﾌｧｲﾙﾌﾙﾊﾟｽを再表示
            Else
                ' 登録済みﾃﾞｰﾀﾌｧｲﾙがなくなった場合
                Call ListList_SelectedIndexChanged(sender, e)  ' 登録済みﾃﾞｰﾀの選択中ﾌｧｲﾙﾌﾙﾊﾟｽを再表示
            End If

            Call DirListBox_Change(sender, e)   ' ﾃﾞｨﾚｸﾄﾘﾂﾘｰを再表示

            ' エンドレスモード処理
            Call DspEndless()

            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormDataSelect2.BtnDelete_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "「リストをクリア」ボタン押下時処理"
    '''=========================================================================
    ''' <summary>「リストをクリア」ボタン押下時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>「登録済みデータファイル」リストボックスから全て削除</remarks>
    '''=========================================================================
    Private Sub BtnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnClear.Click

        'Dim r As Integer
        Dim r As DialogResult
        Dim strMSG As String
        Try
            If (ListList.Items.Count < 1) Then
                ' 登録済みﾃﾞｰﾀﾌｧｲﾙﾘｽﾄに項目がない場合、ENTRYLOT ﾌｫﾙﾀﾞ内のﾌｧｲﾙをすべて削除する
                For Each tmpFile As String In (Directory.GetFiles(ENTRY_PATH))
                    IO.File.Delete(tmpFile)
                Next
                Exit Sub
            Else
                ' 登録済みﾃﾞｰﾀﾌｧｲﾙﾘｽﾄに項目がある場合、削除確認ﾒｯｾｰｼﾞを表示する
                ' "登録リストを全て削除します。" & vbCrLf & "よろしいですか？"
                strMSG = MSG_AUTO_15 & vbCrLf & MSG_AUTO_16
                'r = MsgBox(strMSG, MsgBoxStyle.OkCancel, "")
                'If (r <> MsgBoxResult.Ok) Then Exit Sub ' ｷｬﾝｾﾙ
                r = MessageBox.Show(Me, strMSG, Me.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
                If (r <> DialogResult.OK) Then Exit Sub ' ｷｬﾝｾﾙ

                ' ENTRYLOT ﾌｫﾙﾀﾞ内のﾌｧｲﾙをすべて削除する
                For Each tmpFile As String In (Directory.GetFiles(ENTRY_PATH))
                    IO.File.Delete(tmpFile)
                Next

                ' 「登録済みデータファイル」リストボックスクリア
                Call ListList.Items.Clear() '「登録済みデータファイル」リストボックスクリア

                ' データファイル名をフルパスでラベルテキストボックスに設定する(クリアする)
                Call ListList_SelectedIndexChanged(sender, e)
                Call DirListBox_Change(sender, e) ' ﾃﾞｨﾚｸﾄﾘﾂﾘｰを再表示する

                ' エンドレスモード処理
                Call DspEndless()

                Exit Sub
            End If

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormDataSelect2.BtnClear_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "登録ボタン押下時処理"
    '''=========================================================================
    ''' <summary>登録ボタン押下時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSelect.Click

        Dim Idx As Integer
        Dim Sz As Integer
        Dim Pos As Integer
        Dim strDAT As String
        Dim strMSG As String
        Try
            '「データファイル」リストボックスインデックス無効ならNOP
            Idx = ListFile.SelectedIndex
            If (Idx < 0) Then Exit Sub
            ' エンドレスモードで選択リスト1以上有りならNOP

            ' 指定のデータファイル名を「登録済みデータファイル」リストボックスに追加する
            strDAT = Convert.ToString(ListFile.Items(Idx))                      ' 日付時刻付きファイル名なのでファイル名のみ取り出す 
            Sz = strDAT.Length
            'V6.1.4.0⑱後ろから検索するとファイル名の中に空白が使えない。　Pos = strDAT.LastIndexOf(" ")
            Pos = strDAT.IndexOf(" ")           'V6.1.4.0⑱
            If (Pos = -1) Then Exit Sub
            Pos = strDAT.IndexOf(" ", Pos + 1)  'V6.1.4.0⑱
            If (Pos = -1) Then Exit Sub
            strDAT = strDAT.Substring(Pos + 1, Sz - Pos - 1)
            Idx = ListList.Items.Count

            Dim sFromFilePath As String = ""
            Dim sCopyFilePath As String = ""
            If (False = CopyEntryFileToWorkFolder(sFromFilePath, sCopyFilePath, strDAT)) Then ' 選択ﾌｧｲﾙをｺﾋﾟｰする
                ' TODO: ｴﾗｰﾒｯｾｰｼﾞ
                'MsgBox((sFromFilePath & vbCrLf & vbTab & "↓" & vbCrLf & sCopyFilePath), _
                '    DirectCast((MsgBoxStyle.Critical + MsgBoxStyle.OkOnly), MsgBoxStyle))
                MessageBox.Show(Me, (sFromFilePath & vbCrLf & vbTab & "↓" & vbCrLf & sCopyFilePath), _
                                Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                Call ListList.Items.Add(strDAT)
                ListList.SelectedIndex = Idx

                ' データファイル名をフルパスでラベルテキストボックスに設定する
                'Call ListList_SelectedIndexChanged(sender, e)

            End If

            ' エンドレスモード処理
            Call DspEndless()
            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormDataSelect2.BtnSelect_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "登録ﾌｧｲﾙをENTRYLOTﾌｫﾙﾀﾞにｺﾋﾟｰする"
    '''=========================================================================
    ''' <summary>登録ﾌｧｲﾙをENTRYLOTﾌｫﾙﾀﾞにｺﾋﾟｰする</summary>
    ''' <param name="sFromFilePath">IN="",OUT=ｺﾋﾟｰ元ﾌﾙﾊﾟｽ</param>
    ''' <param name="sCopyFilePath">IN="",OUT=ｺﾋﾟｰ先ﾌﾙﾊﾟｽ</param>
    ''' <param name="sCopyFile">IN=ｺﾋﾟｰするﾌｧｲﾙ名.拡張子,OUT=ｺﾋﾟｰしたﾌｧｲﾙ名.拡張子</param>
    ''' <returns>True=成功,False=失敗</returns>
    ''' <remarks>ﾌｧｲﾙ名に名に_01,_02と連番を付加する</remarks>
    '''=========================================================================
    Private Function CopyEntryFileToWorkFolder(ByRef sFromFilePath As String, _
                ByRef sCopyFilePath As String, ByRef sCopyFile As String) As Boolean

        Dim sTmpFile As String                                          ' ﾌｧｲﾙ名
        Dim sExtended As String                                         ' 拡張子
        '----- V6.1.4.0_33↓ -----
        Dim bLotAndRes As Boolean = False                               ' "伝票Ｎｏ"-"抵抗値"のフォーマットにする
        Dim targetValue As String = ""
        Dim lotNumber As String = ""
        Dim strMSG As String = ""
        '----- V6.1.4.0_33↑ -----

        CopyEntryFileToWorkFolder = False
        Try
            'sTmpFile = sCopyFile.Split("."c)(0)                        ' ﾌｧｲﾙ名に"."が複数あるとNG
            'sExtended = "." & sCopyFile.Split("."c)(1)                 ' ﾌｧｲﾙ名に"."が複数あるとNG
            sTmpFile = Path.GetFileNameWithoutExtension(sCopyFile)      ' V6.1.4.0②
            sExtended = Path.GetExtension(sCopyFile)                    ' V6.1.4.0②

            '----- V6.1.4.0_33↓ -----
            If gbQRCodeReaderUse OrElse gbQRCodeReaderUseTKYNET Then        'V6.1.4.14② gbQRCodeReaderUseTKYNET追加
                bLotAndRes = True
                sFromFilePath = (FileLstBox.Path & "\" & sTmpFile & sExtended)
                If (False = IO.File.Exists(sFromFilePath)) Then
                    'sCopyFilePath = LOAD_MSG01                         ' 指定したファイルが見つかりません。V6.1.4.0①
                    sCopyFilePath = MSG_164                             ' 指定したファイルが見つかりません。V6.1.4.0①
                    Return False
                End If

                ' ファイル読み込み
                Dim lines As New List(Of String)(
                    IO.File.ReadAllLines(sFromFilePath, Encoding.GetEncoding("shift-jis")))

                ' [QRDATA]セクション検索
                Dim qrData As Integer = lines.LastIndexOf("[QRDATA]")
                If (qrData < 0) Then
                    sCopyFilePath = MSG_AUTO_24                         ' QRコードの情報が保存されていません。
                    ' "ｺﾋﾟｰ元ﾌﾙﾊﾟｽ ↓ ｺﾋﾟｰ先ﾌﾙﾊﾟｽ 元のファイル名でエントリします。"
                    strMSG = sFromFilePath + vbCrLf + vbTab + "↓" + vbCrLf + sCopyFilePath + vbCrLf + MSG_219
                    MessageBox.Show(Me, strMSG, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    bLotAndRes = False
                Else
                    qrData += 1                                         ' [QRDATA]の次行からがデータ行
                    targetValue = lines(qrData + QRDATAInfoRowOffset.TargetValue).Replace(" "c, "")
                    lotNumber = lines(qrData + QRDATAInfoRowOffset.LotNumber)
                End If
            End If
            '----- V6.1.4.0_33↑ -----

            For i As Integer = 1 To 99 Step 1
                If bLotAndRes Then                                  ' V6.1.4.0_33
                    If gTkyKnd = KND_CHIP Then                      'V6.1.4.14②
                        ' 伝票No(ロット番号)-抵抗値(目標値)_連番.拡張子 ' V6.1.4.0_33
                        sCopyFilePath = (ENTRY_PATH & lotNumber & "-" & targetValue & "_" & i.ToString("00") & sExtended) ' V6.1.4.0_33
                        'V6.1.4.14②↓
                    Else            ' NET
                        sCopyFilePath = (ENTRY_PATH & lotNumber & "_" & i.ToString("00") & sExtended)
                    End If
                    'V6.1.4.14②↑
                Else
                    ' 連番を追加したﾌｧｲﾙ名の作成
                    sCopyFilePath = (ENTRY_PATH & sTmpFile & "_" & i.ToString("00") & sExtended)
                End If                                              ' V6.1.4.0_33

                Debug.Print(sCopyFilePath)

                ' 同名ﾌｧｲﾙの存在確認
                If (False = IO.File.Exists(sCopyFilePath)) Then
                    ' 存在しなければﾌｧｲﾙをｺﾋﾟｰ
                    If Not bLotAndRes Then                          ' V6.1.4.0_33
                        sFromFilePath = (FileLstBox.Path & "\" & sTmpFile & sExtended)
                    End If                                          ' V6.1.4.0_33
                    IO.File.Copy(sFromFilePath, sCopyFilePath)
                    If (IO.File.Exists(sCopyFilePath)) Then
                        If bLotAndRes Then                                  ' V6.1.4.0_33
                            sCopyFile = Path.GetFileName(sCopyFilePath)     ' V6.1.4.0_33
                        Else                                                ' V6.1.4.0_33
                            sCopyFile = (sTmpFile & "_" & i.ToString("00") & sExtended)
                        End If                                              ' V6.1.4.0_33
                        CopyEntryFileToWorkFolder = True
                    End If
                    Exit Function
                End If
            Next i

        Catch ex As Exception
            strMSG = "FormDataSelect2.CopyEntryFileToWorkFolder() TRAP ERROR = " + ex.Message ' V6.1.4.0_33
            MsgBox(strMSG)
        End Try

    End Function
#End Region

    '========================================================================================
    '   リストボックスのクリックイベント処理
    '========================================================================================
#Region "「データファイル」リストボックスダブルクリックイベント処理"
    '''=========================================================================
    ''' <summary>「データファイル」リストボックスダブルクリックイベント処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub ListFile_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListFile.DoubleClick
        Dim strMSG As String

        Try
            ' 登録ボタン押下時処理へ
            Call BtnSelect_Click(sender, e)
            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormDataSelect2.ListFile_DoubleClick() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "「登録済みデータファイル」ﾘｽﾄのｲﾝﾃﾞｯｸｽ変更ｲﾍﾞﾝﾄ"
    '''=========================================================================
    ''' <summary>「登録済みデータファイル」ﾘｽﾄのｲﾝﾃﾞｯｸｽ変更ｲﾍﾞﾝﾄ</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub ListList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                Handles ListList.SelectedIndexChanged

        Dim Idx As Integer
        Dim strMSG As String

        Try
            ' 「登録済みデータファイル」リストボックスで選択されたデータファイル名をフルパスでラベルテキストボックスに設定する
            Idx = ListList.SelectedIndex
            If (Idx < 0) Then
                LblFullPath.Text = ""
            Else
                LblFullPath.Text = (ENTRY_PATH & Convert.ToString(ListList.Items(Idx)))
            End If
            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormDataSelect2.ListList_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "「ドライブリストボックス」の SelectedIndexChanged 処理"
    '''=========================================================================
    ''' <summary>「ドライブリストボックス」の SelectedIndexChanged 処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>DriveListBoxは非標準コントロールなのでツールボックスに追加する必要有り</remarks>
    '''=========================================================================
    Private Sub DrvListBox_SelectedIndexChanged( _
        ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DrvListBox.SelectedIndexChanged

        Try
            ' ディレクトリリストボックスの選択ドライブを変更する
            Dim tmpDrv As String = DrvListBox.Drive
            If (0 = (String.Compare(tmpDrv, "C:", True))) Then tmpDrv = DATA_DIR_PATH
            DirListBox.Path = tmpDrv
            Call DirListBox_Change(sender, e)   ' ﾃﾞｨﾚｸﾄﾘﾂﾘｰを再表示する
            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            Dim strMSG As String = ex.Message
            MsgBox(strMSG)
            DrvListBox.Drive = "C:"
        End Try
    End Sub
#End Region

#Region "「ディレクトリリストボックス」の変更時処理"
    '''=========================================================================
    ''' <summary>「ディレクトリリストボックス」の変更時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>DirListBoxは非標準コントロールなのでツールボックスに追加する必要有り</remarks>
    '''=========================================================================
    Private Sub DirListBox_Change(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DirListBox.Change

        Dim strMSG As String

        Try
            ' 選択ディレクトリを変更する(FileLstBoxは作業用のDummy)
            FileLstBox.Path = DirListBox.Path

            ' 「データファイル」リストボックスに日付時刻付きファイル名を表示する
            MakeFileList()
            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormDataSelect2.DirListBox_Change() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "「ディレクトリリストボックス」クリック時処理"
    '''=========================================================================
    ''' <summary>ﾃﾞｨﾚｸﾄﾘﾎﾞｯｸｽｸﾘｯｸ時の処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>この処理により DirListBox_Change ｲﾍﾞﾝﾄが発生し、ﾃﾞｨﾚｸﾄﾘﾂﾘｰを再表示する</remarks>
    '''=========================================================================
    Private Sub DirListBox_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DirListBox.Click
        Try
            With DirectCast(sender, VB6.DirListBox)
                If (.Path <> .DirList(.DirListIndex)) Then
                    .Path = .DirList(.DirListIndex) ' 選択したﾃﾞｨﾚｸﾄﾘをﾊﾟｽに設定する
                End If
            End With
        Catch ex As Exception
            Dim strMSG As String = "FormDataSelect2.DirListBox_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

    '========================================================================================
    '   動作モードラジオボタン変更時の処理
    '========================================================================================
#Region "マガジンモード選択処理"
    '''=========================================================================
    ''' <summary>マガジンモード選択処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnMdMagazine_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Call DspEndless()
    End Sub
#End Region

#Region "ロットモード選択処理"
    '''=========================================================================
    ''' <summary>ロットモード選択処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnMdLot_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Call DspEndless()
    End Sub
#End Region

#Region "エンドレスモード選択処理"
    '''=========================================================================
    ''' <summary>エンドレスモード選択処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnMdEndless_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Call DspEndless()
    End Sub
#End Region

    '========================================================================================
    '   共通関数定義
    '========================================================================================
#Region "「データファイル」リストボックスに日付時刻付きファイル名を表示する"
    '''=========================================================================
    ''' <summary>「データファイル」リストボックスに日付時刻付きファイル名を表示する</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub MakeFileList()

        Dim Count As Integer
        Dim i As Integer
        Dim strWK As String = String.Empty
        Dim strDAT As String
        Dim strMSG As String

        Try
            ' 「データファイル」リストボックスに日付時刻付きファイル名を表示する
            Call ListFile.Items.Clear()                                                 '「データファイル」リストボックスクリア
            FileLstBox.Refresh()                                                        ' ファイルリストを更新する  'V6.1.4.0⑭
            Count = FileLstBox.Items.Count                                              ' ファイルの数 
            For i = 0 To (Count - 1)
                ' V6.1.4.0②          ↓
                strDAT = Convert.ToString(FileLstBox.Items(i))
                Dim r As Integer = Rs232c.GetSaveFileName(strDAT, strWK)
                ' 対象の拡張子でなければSKIP(大文字、小文字を区別しない)
                If (SerialErrorCode.rRS_OK <> r) OrElse (0 <> String.Compare(strDAT, strWK, True)) Then GoTo STP_NEXT
                ' V6.1.4.0②          ↑

                ' 日付時刻付きファイルリスト作成 
                Dim tmpFile As String = FileLstBox.Path & "\" & strDAT                  ' V6.1.4.0②
                If (False = (IO.File.Exists(tmpFile))) Then Continue For '              ' ﾌｧｲﾙの存在確認
                Dim Dt As Date = IO.File.GetLastWriteTime(tmpFile)                      ' 更新日時 V6.1.4.0_31
                strDAT = Dt.ToString("yyyy/MM/dd HH:mm:ss") & " " & strDAT              ' 日付時刻の長さを合わせる 
                Call ListFile.Items.Add(strDAT)                                         ' 日付時刻付きファイル名を表示する
STP_NEXT:
            Next i
            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormDataSelect2.MakeFileList() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "エンドレスモード処理"
    '''=========================================================================
    ''' <summary>エンドレスモード処理</summary>
    ''' <remarks>エンドレスモード時はデータファイルを１つしか選択できない</remarks>
    '''=========================================================================
    Private Sub DspEndless()

        Dim strMSG As String

        Try
            ' エンドレスモードで選択リスト1以上有りなら下記のボタン等を非活性化にする
            'If (BtnMdEndless.Checked = True) And (ListList.Items.Count >= 1) Then
            '    ListFile.Enabled = False                                ' データファイルリストボックス非活性化
            '    BtnSelect.Enabled = False                               ' 登録ボタン非活性化 
            '    BtnUp.Enabled = False                                   '「リストの１つ上へ」ボタン非活性化
            '    BtnDown.Enabled = False                                 '「リストの１つ下へ」ボタン非活性化
            'Else
            ListFile.Enabled = True                                 ' データファイルリストボックス活性化
            BtnSelect.Enabled = True                                ' 登録ボタン活性化 
            BtnUp.Enabled = True                                    '「リストの１つ上へ」ボタン活性化
            BtnDown.Enabled = True                                  '「リストの１つ下へ」ボタン活性化
            'End If

            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormDataSelec.2t.DspEndless() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "自動運転用ファンクション"
    '===========================================================================
    '   自動運転用ファンクション
    '===========================================================================
#Region "プレートデータファイル名の設定"
    '=========================================================================
    '【機　能】プレートデータファイル名の設定
    '【引　数】0:設定 1:設定無し
    '【戻り値】連続自動運転のログファイル名を生成する。
    '=========================================================================
    Public Function PlateDataFileName(ByVal mode As Integer, ByVal sName As String) As String

        If mode = 0 Then
            sPlateDataFileName = sName
        End If

        PlateDataFileName = sPlateDataFileName

    End Function
#End Region

#Region "プレートデータ・ファイルを削除する"
    '=========================================================================
    '【機　能】プレートデータ・ファイルを削除する。
    '【引　数】無し
    '【戻り値】無し
    '=========================================================================
    Private Shared Sub SavePlateDataFileDelete()

        Dim sFolder As String

        Try

            sFolder = ENTRY_PATH & ENTRY_TMP_FILE

            If IO.File.Exists(sFolder) = True Then  ' ファイルが有れば削除する。
                IO.File.Delete(sFolder)
            End If

        Catch ex As Exception
            FormMain.Z_PRINT("FormDataSelect2.SavePlateDataFileDelete() TRAP ERROR = " & ex.Message & vbCrLf)
        End Try
    End Sub
#End Region

#Region "途中で終了した時にプレートデータを保存する"
    '=========================================================================
    '【機　能】途中で終了した時にプレートデータを保存する。
    '【引　数】プレートデータ配列、スタート(0 Origin)、終了
    '【戻り値】無し
    '=========================================================================
    Private Shared Sub SavePlateDataFileFullPath(ByRef sPath() As String, ByVal iStart As Integer, ByVal iEnd As Integer)   'V6.1.4.0②
        'Public Sub SavePlateDataFileFullPath(ByRef sPath() As String, ByVal iStart As Integer, ByVal iEnd As Integer)
        Dim sFolder As String
        Dim iFileNo As Integer
        'Dim WS As IO.StreamWriter

        Try

            sFolder = ENTRY_PATH & ENTRY_TMP_FILE

            Call SavePlateDataFileDelete()

            Using WS As New IO.StreamWriter(sFolder, True, System.Text.Encoding.GetEncoding("Shift-JIS"))
                For iFileNo = iStart To iEnd
                    WS.WriteLine(sPath(iFileNo))
                Next

                'WS.Close()
            End Using

        Catch ex As Exception
            FormMain.Z_PRINT("FormDataSelect2.SavePlateDataFileFullPath() TRAP ERROR = " & ex.Message & vbCrLf)
        End Try

    End Sub
#End Region

#Region "途中で終了した時に保存されたプレートデータをロードする"
    '=========================================================================
    '【機　能】途中で終了した時に保存されたプレートデータをロードする。
    '【引　数】無し
    '【戻り値】無し
    '========================================================================
    Private Sub LoadPlateDataFileFullPath() ' private V6.1.4.0⑭
        'Public Sub LoadPlateDataFileFullPath()
        Dim sFolder As String
        Dim sPathData As String = ""

        sFolder = ENTRY_PATH & ENTRY_TMP_FILE

        If IO.File.Exists(sFolder) = True Then  ' ファイルが読み取る。
            Using sr As New System.IO.StreamReader(sFolder, System.Text.Encoding.GetEncoding("Shift-JIS"))
                Do While Not sr.EndOfStream
                    Dim sPath As String = sr.ReadLine
                    If sPath <> "" Then
                        'ListList.Items.Add(sPath)
                        ListList.Items.Add(IO.Path.GetFileName(sPath))  'V6.1.4.0②
                    Else
                        MsgBox("ファイルが存在しませんでした =" & sPathData, vbOKOnly Or vbExclamation Or vbSystemModal Or vbMsgBoxSetForeground, "Warning")
                    End If
                Loop
            End Using
            If (0 < ListList.Items.Count) Then ListList.SelectedIndex = 0 'V6.1.4.0②
        End If

    End Sub
#End Region

#Region "連続自動運転用連続トリミングＮＧ枚数カウンター"
    '=========================================================================
    '【機　能】連続自動運転用連続トリミングＮＧ枚数カウンター
    '【第１引数】0:初期化、1:NGカウント設定 その他：現在カウンターの取得
    '【第２引数】ＮＧカウンター値
    '【戻り値】ＮＧカウンター値
    '=========================================================================
    Private Function NGCountData(ByVal mode As Integer, ByVal lNgCount As Long) As Long

        If mode = 0 Then
            InitiallNgCount = lNgCount
            NowlNgCount = 0
        ElseIf mode = 1 Then
            NowlNgCount = lNgCount - InitiallNgCount
        End If

        NGCountData = NowlNgCount
        Debug.Print("InitiallNgCount=" & InitiallNgCount & "NowlNgCount=" & NowlNgCount)
    End Function
#End Region

#Region "連続トリミングＮＧ枚数カウンター設定初期化"
    '=========================================================================
    '【機　能】連続トリミングＮＧ枚数カウンター設定初期化
    '【引　数】ＮＧカウンター値
    '【戻り値】無し
    '=========================================================================
    Public Sub InitNGCountForContinueAuto(ByVal lNgCount As Long)
        Call NGCountData(0, lNgCount)
    End Sub
#End Region

#Region "連続トリミングＮＧ枚数カウンター設定"
    '=========================================================================
    '【機　能】連続トリミングＮＧ枚数カウンター設定
    '【引　数】ＮＧカウンター値
    '【戻り値】無し
    '=========================================================================
    Public Sub SetNGCountForContinueAuto(ByVal lNgCount As Long)
        Call NGCountData(1, lNgCount)
    End Sub
#End Region

#Region "ロット切り替え判定初期化"
    '=========================================================================
    '【機　能】ロット切り替え判定初期化
    '【引　数】無し
    '【戻り値】無し
    '=========================================================================
    Public Sub InitLotChangeJudge()
        bLotChange = False
    End Sub
#End Region

#Region "ロット切り替え判定セット"
    '=========================================================================
    '【機　能】ロット切り替え判定セット
    '【引　数】無し
    '【戻り値】無し
    '=========================================================================
    Public Sub SetLotChangeJudge()
        bLotChange = True
    End Sub
#End Region

#Region "ロット切り替え判定取得"
    '=========================================================================
    '【機　能】ロット切り替え判定取得
    '【引　数】無し
    '【戻り値】無し
    '=========================================================================
    Public Function GetLotChangeJudge() As Boolean
        GetLotChangeJudge = bLotChange
        If bLotChange Then
            bLotChange = False
        End If
    End Function
#End Region

#Region "連続自動運転モードの初期化"
    '=========================================================================
    '【機　能】連続自動運転モードの初期化
    '【引　数】無し
    '【戻り値】無し
    '=========================================================================
    Public Sub InitialAutoOperation()
        AutoOpeCancel = False
        NowExecuteLotNo = 0
        CancelReason = 0
        _resultFile = String.Empty
        Call PlateDataFileName(0, gsAutoDataFileFullPath(0))  ' プレートデータファイル名を保存
        Call InitLotChangeJudge()
    End Sub
#End Region

#Region "ロット切り替え判定セット"
    '=========================================================================
    '【機　能】ロット切り替え判定セット
    '【引　数】無し
    '【戻り値】無し
    '=========================================================================
    Public Function GetAutoOpeCancelStatus() As Boolean
        If gbFgAutoOperation432 = True Then
            GetAutoOpeCancelStatus = AutoOpeCancel
        Else
            GetAutoOpeCancelStatus = False
        End If
    End Function
#End Region

#Region "連続自動運転中止"
    '=========================================================================
    ''' <summary>連続自動運転中止</summary>
    ''' <returns>True=成功</returns>
    '=========================================================================
    Public Function SetAutoOpeCancel() As Boolean
        Call SetLoaderIO(COM_STS_TRM_NG, &H0)                   ' ローダ出力(ON=トリミングＮＧ, OFF=なし)トリミング不良信号を連続自動運転中止通知に使用
        AutoOpeCancel = True
    End Function
#End Region

#Region "ロット切り替え処理可否チェック"
    '=========================================================================
    ''' <summary>ロット切り替え処理可否チェック</summary>
    ''' <returns>True=ロット切替え処理可, False=ロット切替え処理否</returns>
    '=========================================================================
    Public Function LotChangeExecuteCheck() As Boolean

        If NowExecuteLotNo + 1 >= giAutoDataFileNum432 Then
            LotChangeExecuteCheck = False
        Else
            LotChangeExecuteCheck = True
        End If
    End Function
#End Region

#Region "ロット切り替え処理"
    '=========================================================================
    '【機　能】ロット切り替え処理
    '【引　数】無し
    '【戻り値】無し
    '=========================================================================
    Public Function LotChangeExecute() As Boolean
        Try
            Dim iRtn As Integer

            If LotChangeExecuteCheck() Then
                Call PrintTrimLoggingData()
                NowExecuteLotNo = NowExecuteLotNo + 1
                iRtn = FormMain.Sub_FileLoad(gsAutoDataFileFullPath(NowExecuteLotNo))
                If iRtn <> cFRS_NORMAL Then
                    CancelReason = LOAD_FAILED
                    Call SetAutoOpeCancel()
                    LotChangeExecute = False
                Else
                    Call PlateDataFileName(0, gsAutoDataFileFullPath(NowExecuteLotNo))  ' プレートデータファイル名を保存
                    LotChangeExecute = True
                End If
            Else
                'Call FormMain.Z_PRINT("ロット切り替え信号を受けましたが、次のエントリーが有りません。" & vbCrLf)
                CancelReason = NO_MORE_ENTRY
                Call SetAutoOpeCancel()
                LotChangeExecute = False
            End If
        Catch ex As Exception
            Call FormMain.Z_PRINT("FormDataSelect2.LotChangeExecute() TRAP ERROR = " & ex.Message & vbCrLf)
        End Try
    End Function
#End Region

#Region "連続自動運転終了処理"
    '=========================================================================
    '【機　能】連続自動運転終了処理
    '【引　数】無し
    '【戻り値】無し
    '=========================================================================
    Public Sub AutoOperationEnd()

        Try
            If gbFgAutoOperation432 = True Then
                Call SaveTrimLoggingData()                  ' 現在の生産管理情報を退避する
                Call TrimLoggingData(1)                     ' 生産管理データログ出力
            End If

            Call SavePlateDataFileDelete()

            NowExecuteLotNo = NowExecuteLotNo + 1
            If AutoOpeCancel Or (NowExecuteLotNo < giAutoDataFileNum432) Then
                If AutoOpeCancel Then
                    NowExecuteLotNo = NowExecuteLotNo - 1   ' 現在のプレートデータから保存する。
                End If
                If NowExecuteLotNo < giAutoDataFileNum432 Then
                    Call SavePlateDataFileFullPath(gsAutoDataFileFullPath, NowExecuteLotNo, giAutoDataFileNum432 - 1)
                End If
            End If

            AutoOpeCancel = False
            gbFgAutoOperation432 = False
        Catch ex As Exception
            MessageBox.Show("FormDataSelect2.AutoOperationEnd() TRAP ERROR = " & ex.Message)
        End Try

    End Sub
#End Region
#End Region
    'V6.1.4.0_34↓
#Region "オペレータ選択コンボボックスの設定をおこなう"
    '''=========================================================================
    ''' <summary>オペレータ選択コンボボックスの設定をおこなう</summary>
    ''' <returns>True:OK, False:NG</returns>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Function InitializeComboBoxOperatorList() As Boolean
        Dim ret As Boolean
        Dim fp As String = OPERATOR_PATH & OPERATOR_LIST_FILE

        Try
            If (False = Directory.Exists(OPERATOR_PATH)) Then
                Directory.CreateDirectory(OPERATOR_PATH)                ' ﾌｫﾙﾀﾞが存在しなければ作成する
                IO.File.Create(fp)                                      ' ファイルを作成する
                ret = False

            ElseIf (False = IO.File.Exists(fp)) Then
                IO.File.Create(fp)                                      ' ファイルを作成する
                ret = False

            Else
                Dim entry As New List(Of ComboBoxValueMember(Of String))
                entry.Add(New ComboBoxValueMember(Of String)(MSG_AUTO_22))
                [Operator] = entry(0).ValueMember                       ' 未選択にする

                ' オペレータエントリーリストを読み込む
                Dim lines() As String =
                    IO.File.ReadAllLines(fp, Encoding.GetEncoding("shift-jis"))

                Dim maxWidth As Integer = 0
                For i As Integer = 0 To (lines.Length - 1) Step 1
                    Dim line As String = lines(i).Trim()

                    ' 空行は無視する
                    If (Not String.IsNullOrEmpty(line)) Then
                        entry.Add(New ComboBoxValueMember(Of String)(line))
                        maxWidth = Math.Max(maxWidth, TextRenderer.MeasureText(line, ComboBoxOperatorList.Font).Width)
                    End If
                Next i

                ' 未選択 + 1 以上
                If (2 <= entry.Count) Then
                    ComboBoxOperatorList.DropDownWidth =
                        maxWidth + SystemInformation.VerticalScrollBarWidth

                    ' データソースに設定する
                    ComboBoxOperatorList.DataSource = entry
                    ComboBoxOperatorList.DisplayMember = "ValueMember"  ' Display,Valueは同値
                    ComboBoxOperatorList.ValueMember = "ValueMember"

                    ret = True
                Else
                    ' オペレータがエントリーされていない、ファイルがおかしい
                    ret = False
                End If
            End If

        Catch ex As Exception
            ret = False
        End Try

        Return ret

    End Function
#End Region
    'V6.1.4.0_34↑
#End Region

#Region "生産管理ﾃﾞｰﾀﾌｫﾙﾀﾞをｵｰﾌﾟﾝする"
    '=========================================================================
    ''' <summary>生産管理ﾃﾞｰﾀﾌｫﾙﾀﾞをｵｰﾌﾟﾝする</summary>
    ''' <remarks>'V6.1.4.0⑥</remarks>
    '=========================================================================
    Public Sub ProductControlFolderOpen()
        Try
            Process.Start(RESULT_PATH)

        Catch ex As Exception
            Dim strMSG As String = "FormDataSelect2.ProductControlFolderOpen() TRAP ERROR = " & ex.Message
            MessageBox.Show(strMSG)
        End Try
    End Sub
#End Region

#Region "生産管理データログ出力"
    '=========================================================================
    ''' <summary>生産管理データログ出力</summary>
    ''' <param name="iMode">(INP)0=現在の生産管理情報を退避する
    '''                          1=生産管理データログ出力
    ''' </param>
    '=========================================================================
    Private Sub TrimLoggingData(ByVal iMode As Integer)

        Try
            If iMode = 0 Then
                ' データの保存
                sSavePLTNUM = Fram1LblAry(FRAM1_ARY_PLTNUM).Text
                sSaveREGNUM = Fram1LblAry(FRAM1_ARY_REGNUM).Text
                sSaveGO = Fram1LblAry(FRAM1_ARY_GO).Text
                sSaveNG = Fram1LblAry(FRAM1_ARY_NG).Text
                sSaveITHING = Fram1LblAry(FRAM1_ARY_ITHING).Text
                sSaveITLONG = Fram1LblAry(FRAM1_ARY_ITLONG).Text
                sSaveITHINGP = Fram1LblAry(FRAM1_ARY_ITHINGP).Text
                sSaveITLONGP = Fram1LblAry(FRAM1_ARY_ITLONGP).Text
                sSaveFTLONG = Fram1LblAry(FRAM1_ARY_FTLONG).Text
                sSaveFTHING = Fram1LblAry(FRAM1_ARY_FTHING).Text
                sSaveFTHINGP = Fram1LblAry(FRAM1_ARY_FTHINGP).Text
                sSaveFTLONGP = Fram1LblAry(FRAM1_ARY_FTLONGP).Text
                sSaveOVER = Fram1LblAry(FRAM1_ARY_OVER).Text
                sSaveNGPER = Fram1LblAry(FRAM1_ARY_NGPER).Text
                sSaveOVERP = Fram1LblAry(FRAM1_ARY_OVERP).Text
            Else
                ' 生産管理データログ出力
                Call ProductControlFileWrite()
            End If
        Catch ex As Exception
            MessageBox.Show("FormDataSelect2.TrimLoggingData() TRAP ERROR = " & ex.Message)
        End Try
    End Sub
    'Public Sub SaveTrimLoggingData(ByVal sL7 As String, ByVal sL4 As String, ByVal sL14 As String, ByVal sL21 As String, ByVal sL22 As String, ByVal sL17 As String, ByVal sL5 As String, ByVal sL15 As String, ByVal sL23 As String, ByVal sL6 As String, ByVal sL29 As String, ByVal sL30 As String, ByVal sL31 As String, ByVal sL32 As String, ByVal sL33 As String)
    Public Sub SaveTrimLoggingData()
        Call TrimLoggingData(0)                 ' 現在の生産管理情報を退避する
    End Sub
#End Region

#Region "生産管理データのクリア及び表示の更新"
    '=========================================================================
    ''' <summary>生産管理データのクリア及び表示の更新</summary>
    ''' <remarks></remarks>
    '=========================================================================
    Public Sub PrintTrimLoggingData()
        Try
            Call SaveTrimLoggingData()          ' 現在の生産管理情報を退避する
            Call TrimLoggingData(1)             ' 生産管理データログ出力

            Call m_mainEdit.ClearCounter(1)     ' 生産管理データのクリア
            ' 統計表示がONの場合、表示を更新する
            If Form1.chkDistributeOnOff.Checked = True Then
                gObjFrmDistribute.RedrawGraph()
            End If

        Catch ex As Exception
            MessageBox.Show("FormDataSelect2.PrintTrimLoggingData() TRAP ERROR = " & ex.Message)
        End Try
    End Sub
#End Region

#Region "生産管理データログ出力"
    '''=========================================================================
    ''' <summary>生産管理データログ出力</summary>
    ''' <remarks>'V4.7.0.0③1</remarks>
    '''=========================================================================
    Public Sub ProductControlFileWrite()

        Try
            Const W As Integer = (-18)  ' 各項目の幅(左詰)
            Dim bcDiff As Integer       ' 文字列の全角半角ﾊﾞｲﾄ差分
            Dim sb As New StringBuilder(512)

            ' ------------------------------------------------
            sb.AppendLine(New String("-"c, 48))

            ' 2015/07/02 16:29:38
            sb.AppendLine(Date.Now.ToString("yyyy/MM/dd HH:mm:ss"))

            '----- V6.1.4.0_39↓ -----
            If gbQRCodeReaderUse And typQRDATAInfo.bStatus Then
                sb.AppendLine(String.Format("オペレータ= {0}", frmAutoObj.Operator))
                sb.AppendLine(String.Format("伝票No.= {0} 抵抗値= {1} タイプ= {2}", typQRDATAInfo.sLotNumber, ObjQRCodeReader.ChangeDisplayFormatValue(typQRDATAInfo.sTargetValue), typQRDATAInfo.sType))
                sb.AppendLine(String.Format("品種= {0} 加工必要数量= {1}", typQRDATAInfo.sSeihinSyurui, typQRDATAInfo.sKakouHitsuyoSuuryo))
                sb.AppendLine(String.Format("パターン= {0} 膜厚= {1} ランク= {2}", typQRDATAInfo.sPattern, typQRDATAInfo.sMakuatsu, typQRDATAInfo.sRank))
                'sb.AppendLine(String.Format("減衰率= {0} 最大出力= {1}", typQRDATAInfo.dAttenuaterValue.ToString("0.00") + " %", GetPrivateProfileString_S("ROT_ATT", "MES_MAX_POWER", "C:\TRIM\TKYSYS.ini", "未測定") + " W"))'V6.1.4.0_22
                sb.AppendLine(String.Format("減衰率= {0} 最大出力= {1}", CDbl(typQRDATAInfo.dAttenuaterValue).ToString("0.00") + " %", GetPrivateProfileString_S("ROT_ATT", "MES_MAX_POWER", Form1.TKYSYSPARAMPATH, "未測定") + " W")) 'V6.1.4.0_22
            End If
            '----- V6.1.4.0_39↑ -----
            ' PLATE DATA FILE = C:\TRIMDATA\ENTRYLOT\20150520-60.4Ω_22.tdc
            sb.AppendLine(String.Format("PLATE DATA FILE = {0}", typPlateInfo.strDataName))         ' ﾃﾞｰﾀﾌｧｲﾙ名

            'V6.1.4.0_22↓
            'V6.1.4.0_39  If gbQRCodeReaderUse And typQRDATAInfo.bStatus Then
            'V6.1.4.0_39      sb.AppendLine(String.Format("LOT NO={0} RES={1} ATT RATE={2}", typQRDATAInfo.sLotNumber, ObjQRCodeReader.ChangeDisplayFormatValue(typQRDATAInfo.sTargetValue), typQRDATAInfo.dAttenuaterValue.ToString("0.00") + " %"))         ' ﾃﾞｰﾀﾌｧｲﾙ名
            'V6.1.4.0_39  End If
            'V6.1.4.0_22↑

            ' 抵抗単位
            sb.AppendLine(MSG_TOTAL_REGISTOR)

            ' 基板枚数=0        RESISTOR=0        IT H NG率=0.00
            bcDiff = Encoding.GetEncoding( _
                CultureInfo.CurrentUICulture.TextInfo.OEMCodePage).GetByteCount(MSG_MAIN_LABEL01) - MSG_MAIN_LABEL01.Length
            sb.AppendFormat("{0," & (W + bcDiff) & "}", _
                            String.Format(MSG_MAIN_LABEL01 & "{0}", sSavePLTNUM))        ' 基板枚数
            If gTkyKnd = KND_CHIP Then  'V6.1.4.9⑤
                'V6.1.4.9⑤↓
                sb.AppendFormat("{0," & W & "}", String.Format("RESISTOR={0}", sSaveREGNUM)) ' RESISTOR
            Else
                sb.AppendFormat("{0," & W & "}", String.Format("CIRCUIT={0}", sSaveREGNUM)) ' CIRCUIT
            End If
            'V6.1.4.9⑤↑
            sb.AppendFormat(MSG_MAIN_LABEL03 & "{0}", sSaveITHINGP)                      ' IT H NG率
            sb.Append(Environment.NewLine)

            '       OK=0              NG=0        IT L NG率=0.00
            sb.AppendFormat("{0," & W & "}", String.Format("      OK={0}", sSaveGO))     ' OK
            sb.AppendFormat("{0," & W & "}", String.Format("      NG={0}", sSaveNG))     ' NG
            sb.AppendFormat(MSG_MAIN_LABEL04 & "{0}", sSaveITLONGP)                      ' IT L NG率
            sb.Append(Environment.NewLine)

            ' IT HI NG=0        FT HI NG=0        FT H NG率=0.00
            sb.AppendFormat("{0," & W & "}", String.Format("IT HI NG={0}", sSaveITHING)) ' IT HI NG
            sb.AppendFormat("{0," & W & "}", String.Format("FT HI NG={0}", sSaveFTHING)) ' FT HI NG
            sb.AppendFormat(MSG_MAIN_LABEL05 & "{0}", sSaveFTHINGP)                      ' FT H NG 率
            sb.Append(Environment.NewLine)

            ' IT LO NG=0        FT LO NG=0        FT L NG率=0.00
            sb.AppendFormat("{0," & W & "}", String.Format("IT LO NG={0}", sSaveITLONG)) ' IT LO NG
            sb.AppendFormat("{0," & W & "}", String.Format("FT LO NG={0}", sSaveFTLONG)) ' FT LO NG
            sb.AppendFormat(MSG_MAIN_LABEL06 & "{0}", sSaveFTLONGP)                      ' FT L NG率
            sb.Append(Environment.NewLine)

            '     OVER=0            NG率=0.00     OVER NG率=0.00
            sb.AppendFormat("{0," & W & "}", String.Format("    OVER={0}", sSaveOVER))   ' OVER
            bcDiff = Encoding.GetEncoding( _
                CultureInfo.CurrentUICulture.TextInfo.OEMCodePage).GetByteCount(MSG_MAIN_LABEL02) - MSG_MAIN_LABEL02.Length
            sb.AppendFormat("{0," & (W + bcDiff) & "}", _
                            String.Format("   " & New String(" "c, bcDiff) & MSG_MAIN_LABEL02 & "{0}", _
                                          (sSaveNGPER).TrimEnd("%"c)))                   ' NG率
            sb.AppendFormat(MSG_MAIN_LABEL07 & "{0}", sSaveOVERP)                        ' OVER NG率
            'V6.1.4.0_22 sb.AppendLine(Environment.NewLine)
            sb.Append(Environment.NewLine)                                               ' 改行１つに変更 V6.1.4.0_22

            If (gTkyKnd = KND_CHIP) Then            'V6.1.4.9⑤
                'V6.1.4.0_22↓
                For Cn As Integer = 1 To typResistorInfoArray(1).intCutCount                ' 抵抗内カット数分チェックする
                    sb.AppendLine("C" & Cn.ToString("0") & " CUTOFF=" & typResistorInfoArray(1).ArrCut(Cn).dblCutOff.ToString("0.00"))
                Next
                sb.Append(Environment.NewLine)
                'V6.1.4.0_22↑
                'V6.1.4.9⑤↓
            Else
                For CirCuitRn As Integer = 1 To typPlateInfo.intResistCntInGroup                 'サーキット内抵抗数
                    For Cn As Integer = 1 To typResistorInfoArray(CirCuitRn).intCutCount                ' 抵抗内カット数分チェックする
                        sb.AppendLine("R" & CirCuitRn.ToString("0") & "C" & Cn.ToString("0") & " CUTOFF=" & typResistorInfoArray(CirCuitRn).ArrCut(Cn).dblCutOff.ToString("0.00"))
                    Next
                Next
                sb.Append(Environment.NewLine)
            End If
            'V6.1.4.9⑤↑

            Debug.Print(sb.ToString() & sb.Length)

            Using fs As New FileStream(RESULT_FILE, FileMode.Append, FileAccess.Write, FileShare.Read)
                Using sw As New StreamWriter(fs, Encoding.UTF8)
                    sw.Write(sb.ToString())
                    _resultWritten = True
                End Using
            End Using

        Catch ex As Exception
            Dim strMSG As String = "FormDataSelect2.ProductControlFileWrite() TRAP ERROR = " & ex.Message
            MessageBox.Show(strMSG)
        End Try

    End Sub
#End Region
    'V6.1.4.0②          ↓   ﾌｫｰﾑが隠れないようにする
#Region "ﾌｫｰﾑが隠れないようにする"
    Private frmDistVisible As Boolean
#If False Then                          ' Form_Loadに移動 V6.1.4.0_33
    Private Sub FormDataSelect2_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
        If (Globals_Renamed.gObjFrmDistribute IsNot Nothing) AndAlso _
            (False = Globals_Renamed.gObjFrmDistribute.IsDisposed) Then
            frmDistVisible = Globals_Renamed.gObjFrmDistribute.Visible
            Globals_Renamed.gObjFrmDistribute.Visible = False
        End If
    End Sub
#End If
    Private Sub FormDataSelect2_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        If (Globals_Renamed.gObjFrmDistribute IsNot Nothing) AndAlso _
            (False = Globals_Renamed.gObjFrmDistribute.IsDisposed) Then
            Globals_Renamed.gObjFrmDistribute.Visible = frmDistVisible
        End If
    End Sub
#End Region
    'V6.1.4.0②          ↑
End Class
'V6.1.4.0_34↓
#Region "コンボボックス項目用クラス"
'===============================================================================
'   項目の実際の値として使用するプロパティを取得または設定します。
'
'===============================================================================
Public Class ComboBoxValueMember(Of T)

    ''' <summary>項目の実際の値として使用するプロパティ</summary>
    ''' <returns>項目の実際の値</returns>
    Public Property ValueMember() As T

    Public Sub New(ByVal valueMember As T)
        Me.ValueMember = valueMember
    End Sub
End Class

'===============================================================================
'   表示するプロパティを取得または設定します。
'
'===============================================================================
Public Class ComboBoxItem(Of T)
    Inherits ComboBoxValueMember(Of T)

    ''' <summary>表示するプロパティ</summary>
    ''' <returns>表示</returns>
    Public Property DisplayMember() As String

    Public Sub New(ByVal displayMember As String, ByVal valueMember As T)
        MyBase.New(valueMember)
        Me.DisplayMember = displayMember
    End Sub
End Class
#End Region

#Region "DropDownListをComboBox.Rightに合わせて表示するコンボボックス"
'===============================================================================
'   DropDownListをComboBox.Rightに合わせて表示するコンボボックス
'   V6.1.4.0_33
'===============================================================================
Public Class ComboBoxRightAlign
    Inherits ComboBox

    Protected Overrides Sub WndProc(ByRef m As Message)
        Const WM_CTLCOLORLISTBOX As UInt32 = &H134

        Select Case (m.Msg)
            Case WM_CTLCOLORLISTBOX
                Dim rect As Rectangle = Me.RectangleToScreen(Me.ClientRectangle)
                Dim ddLeft As Integer = rect.Left - (Me.DropDownWidth - Me.Width) + 1

                ' DropDownListをComboBox.Rightに合わせて移動する
                LaserFront.Trimmer.DefWin32Fnc.SetWindowPos(
                    m.LParam, IntPtr.Zero, ddLeft, rect.Bottom, 0, 0,
                    LaserFront.Trimmer.DefWin32Fnc.SWP_NOSIZE)
        End Select

        MyBase.WndProc(m)

    End Sub

#Region "Designer.vb"
    'Control は、コンポーネント一覧に後処理を実行するために、dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'コントロール デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    ' メモ: 以下のプロシージャはコンポーネント デザイナーで必要です。
    ' コンポーネント デザイナーを使って変更できます。
    ' コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        components = New System.ComponentModel.Container()
    End Sub
#End Region

End Class
#End Region
'V6.1.4.0_34↑
'=============================== END OF FILE ===============================
