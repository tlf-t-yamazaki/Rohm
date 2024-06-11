<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class Form1
#Region "Windows フォーム デザイナによって生成されたコード "
    <System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
        MyBase.New()
        'この呼び出しは、Windows フォーム デザイナで必要です。
        Globals_Renamed.FormMain = Me               'V6.1.4.0②
        File.ConvertFileEncoding(SYSPARAMPATH)      'V4.4.0.0-0
        SetCurrentUICulture()                       'V4.4.0.0-0
        InitializeComponent()
        Form1.Instance = Me                         'V5.0.0.9⑰
        Form_Initialize_Renamed()
    End Sub
    'Form は、コンポーネント一覧に後処理を実行するために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
        If Disposing Then
            If Not components Is Nothing Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(Disposing)
    End Sub
    'Windows フォーム デザイナで必要です。
    Private components As System.ComponentModel.IContainer
    Public WithEvents CmdCircuitTeach As System.Windows.Forms.Button
    Public WithEvents cmdEsLog As System.Windows.Forms.Button
    Public WithEvents CmdCnd As System.Windows.Forms.Button
    Public WithEvents CMdIX2Log As System.Windows.Forms.Button
    Public WithEvents TimerAlarm As System.Windows.Forms.Timer
    Public WithEvents btnTrimming As System.Windows.Forms.Button
    Public WithEvents CmdTx As System.Windows.Forms.Button
    Public WithEvents CmdTy As System.Windows.Forms.Button
    Public WithEvents CmdExCam As System.Windows.Forms.Button
    Public WithEvents CmdExCam1 As System.Windows.Forms.Button
    Public WithEvents CmdCutPosCorrect As System.Windows.Forms.Button
    Public WithEvents CmdCalibration As System.Windows.Forms.Button
    Public WithEvents CmdPtnCalibration As System.Windows.Forms.Button
    Public WithEvents CmdPtnCutPosCorrect As System.Windows.Forms.Button
    Public WithEvents mnuHelpAbout As System.Windows.Forms.Button
    Public WithEvents Timer1 As System.Windows.Forms.Timer
    Public comDlgOpen As System.Windows.Forms.OpenFileDialog
    Public comDlgSave As System.Windows.Forms.SaveFileDialog
    Public WithEvents CmdEnd As System.Windows.Forms.Button
    Public WithEvents CmdPattern As System.Windows.Forms.Button
    Public WithEvents CmdTeach As System.Windows.Forms.Button
    Public WithEvents CmdProbe As System.Windows.Forms.Button
    Public WithEvents CmdLoging As System.Windows.Forms.Button
    Public WithEvents CmdLaser As System.Windows.Forms.Button
    Public WithEvents CmdLoad As System.Windows.Forms.Button
    Public WithEvents lblGraphClick As System.Windows.Forms.Label
    Public WithEvents lblMinValue As System.Windows.Forms.Label
    Public WithEvents lblMaxValue As System.Windows.Forms.Label
    Public WithEvents lblAverageValue As System.Windows.Forms.Label
    Public WithEvents lblDeviationValue As System.Windows.Forms.Label
    Public WithEvents lblNgChip As System.Windows.Forms.Label
    Public WithEvents lblGoodChip As System.Windows.Forms.Label
    Public WithEvents lblDeviation As System.Windows.Forms.Label
    Public WithEvents lblAverage As System.Windows.Forms.Label
    Public WithEvents lblMaxTitle As System.Windows.Forms.Label
    Public WithEvents lblMinTitle As System.Windows.Forms.Label
    Public WithEvents lblNgTitle As System.Windows.Forms.Label
    Public WithEvents lblGoodTitle As System.Windows.Forms.Label
    Public WithEvents lblRegistUnit As System.Windows.Forms.Label
    Public WithEvents _lbglRegistNum_11 As System.Windows.Forms.Label
    Public WithEvents _lbglRegistNum_10 As System.Windows.Forms.Label
    Public WithEvents _lbglRegistNum_9 As System.Windows.Forms.Label
    Public WithEvents _lbglRegistNum_8 As System.Windows.Forms.Label
    Public WithEvents _lbglRegistNum_7 As System.Windows.Forms.Label
    Public WithEvents _lbglRegistNum_6 As System.Windows.Forms.Label
    Public WithEvents _lbglRegistNum_5 As System.Windows.Forms.Label
    Public WithEvents _lbglRegistNum_4 As System.Windows.Forms.Label
    Public WithEvents _lbglRegistNum_3 As System.Windows.Forms.Label
    Public WithEvents _lbglRegistNum_2 As System.Windows.Forms.Label
    Public WithEvents _lbglRegistNum_1 As System.Windows.Forms.Label
    Public WithEvents _lbglRegistNum_0 As System.Windows.Forms.Label
    Public WithEvents lblRegistTitle As System.Windows.Forms.Label
    Public WithEvents _lblGraphPercent_11 As System.Windows.Forms.Label
    Public WithEvents _lblGraphPercent_10 As System.Windows.Forms.Label
    Public WithEvents _lblGraphPercent_9 As System.Windows.Forms.Label
    Public WithEvents _lblGraphPercent_8 As System.Windows.Forms.Label
    Public WithEvents _lblGraphPercent_7 As System.Windows.Forms.Label
    Public WithEvents _lblGraphPercent_6 As System.Windows.Forms.Label
    Public WithEvents _lblGraphPercent_5 As System.Windows.Forms.Label
    Public WithEvents _lblGraphPercent_4 As System.Windows.Forms.Label
    Public WithEvents _lblGraphPercent_3 As System.Windows.Forms.Label
    Public WithEvents _lblGraphPercent_2 As System.Windows.Forms.Label
    Public WithEvents _lblGraphPercent_1 As System.Windows.Forms.Label
    Public WithEvents _lblGraphPercent_0 As System.Windows.Forms.Label
    Public WithEvents lblGraphUnit As System.Windows.Forms.Label
    Public WithEvents lblGraphAccumulationTitle As System.Windows.Forms.Label
    Public WithEvents picGraphAccumulation As System.Windows.Forms.Panel
    Public WithEvents lblInterLockMSG As System.Windows.Forms.Label
    Public WithEvents CmdTy2 As System.Windows.Forms.Button
    Public WithEvents CmdT_Theta As System.Windows.Forms.Button
    Public WithEvents Text4 As System.Windows.Forms.TextBox
    Public WithEvents LblMes As System.Windows.Forms.Label
    Public WithEvents LblCur As System.Windows.Forms.Label
    Public WithEvents lblLoginResult As System.Windows.Forms.Label
    Public WithEvents LblDataFileName As System.Windows.Forms.Label
    Public WithEvents Line1 As System.Windows.Forms.Label
    Public WithEvents lblLogging As System.Windows.Forms.Label
    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使って変更できます。
    'コード エディタを使用して、変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.CmdCircuitTeach = New System.Windows.Forms.Button()
        Me.cmdEsLog = New System.Windows.Forms.Button()
        Me.CmdCnd = New System.Windows.Forms.Button()
        Me.CMdIX2Log = New System.Windows.Forms.Button()
        Me.btnTrimming = New System.Windows.Forms.Button()
        Me.CmdTx = New System.Windows.Forms.Button()
        Me.CmdTy = New System.Windows.Forms.Button()
        Me.CmdExCam = New System.Windows.Forms.Button()
        Me.CmdExCam1 = New System.Windows.Forms.Button()
        Me.CmdCutPosCorrect = New System.Windows.Forms.Button()
        Me.CmdCalibration = New System.Windows.Forms.Button()
        Me.CmdPtnCalibration = New System.Windows.Forms.Button()
        Me.CmdPtnCutPosCorrect = New System.Windows.Forms.Button()
        Me.mnuHelpAbout = New System.Windows.Forms.Button()
        Me.CmdEnd = New System.Windows.Forms.Button()
        Me.CmdPattern = New System.Windows.Forms.Button()
        Me.CmdTeach = New System.Windows.Forms.Button()
        Me.CmdProbe = New System.Windows.Forms.Button()
        Me.CmdLoging = New System.Windows.Forms.Button()
        Me.CmdLaser = New System.Windows.Forms.Button()
        Me.CmdLoad = New System.Windows.Forms.Button()
        Me.picGraphAccumulation = New System.Windows.Forms.Panel()
        Me.lblGraphAccumulationTitle = New System.Windows.Forms.Label()
        Me.lblGraphClick = New System.Windows.Forms.Label()
        Me.lblMinValue = New System.Windows.Forms.Label()
        Me.lblMaxValue = New System.Windows.Forms.Label()
        Me.lblAverageValue = New System.Windows.Forms.Label()
        Me.lblDeviationValue = New System.Windows.Forms.Label()
        Me.lblNgChip = New System.Windows.Forms.Label()
        Me.lblGoodChip = New System.Windows.Forms.Label()
        Me.lblDeviation = New System.Windows.Forms.Label()
        Me.lblAverage = New System.Windows.Forms.Label()
        Me.lblMaxTitle = New System.Windows.Forms.Label()
        Me.lblMinTitle = New System.Windows.Forms.Label()
        Me.lblNgTitle = New System.Windows.Forms.Label()
        Me.GrpMatrix = New System.Windows.Forms.GroupBox()
        Me.lblGoodTitle = New System.Windows.Forms.Label()
        Me.lblRegistUnit = New System.Windows.Forms.Label()
        Me._lbglRegistNum_11 = New System.Windows.Forms.Label()
        Me._lbglRegistNum_10 = New System.Windows.Forms.Label()
        Me._lbglRegistNum_9 = New System.Windows.Forms.Label()
        Me._lbglRegistNum_8 = New System.Windows.Forms.Label()
        Me._lbglRegistNum_7 = New System.Windows.Forms.Label()
        Me._lbglRegistNum_6 = New System.Windows.Forms.Label()
        Me._lbglRegistNum_5 = New System.Windows.Forms.Label()
        Me._lbglRegistNum_4 = New System.Windows.Forms.Label()
        Me._lbglRegistNum_3 = New System.Windows.Forms.Label()
        Me._lbglRegistNum_2 = New System.Windows.Forms.Label()
        Me._lbglRegistNum_1 = New System.Windows.Forms.Label()
        Me._lbglRegistNum_0 = New System.Windows.Forms.Label()
        Me.lblRegistTitle = New System.Windows.Forms.Label()
        Me._lblGraphPercent_11 = New System.Windows.Forms.Label()
        Me._lblGraphPercent_10 = New System.Windows.Forms.Label()
        Me._lblGraphPercent_9 = New System.Windows.Forms.Label()
        Me._lblGraphPercent_8 = New System.Windows.Forms.Label()
        Me._lblGraphPercent_7 = New System.Windows.Forms.Label()
        Me._lblGraphPercent_6 = New System.Windows.Forms.Label()
        Me._lblGraphPercent_5 = New System.Windows.Forms.Label()
        Me._lblGraphPercent_4 = New System.Windows.Forms.Label()
        Me._lblGraphPercent_3 = New System.Windows.Forms.Label()
        Me._lblGraphPercent_2 = New System.Windows.Forms.Label()
        Me._lblGraphPercent_1 = New System.Windows.Forms.Label()
        Me._lblGraphPercent_0 = New System.Windows.Forms.Label()
        Me.lblGraphUnit = New System.Windows.Forms.Label()
        Me.lblInterLockMSG = New System.Windows.Forms.Label()
        Me.CmdTy2 = New System.Windows.Forms.Button()
        Me.CmdT_Theta = New System.Windows.Forms.Button()
        Me.Text4 = New System.Windows.Forms.TextBox()
        Me.LblMes = New System.Windows.Forms.Label()
        Me.LblCur = New System.Windows.Forms.Label()
        Me.lblLoginResult = New System.Windows.Forms.Label()
        Me.LblDataFileName = New System.Windows.Forms.Label()
        Me.Line1 = New System.Windows.Forms.Label()
        Me.lblLogging = New System.Windows.Forms.Label()
        Me.LblRotAtt = New System.Windows.Forms.Label()
        Me.CmdCutPos = New System.Windows.Forms.Button()
        Me.Line2 = New System.Windows.Forms.Label()
        Me.Line3 = New System.Windows.Forms.Label()
        Me.btnCounterClear = New System.Windows.Forms.Button()
        Me.LblGO = New System.Windows.Forms.Label()
        Me.LblTNG = New System.Windows.Forms.Label()
        Me.LblUnit = New System.Windows.Forms.Label()
        Me.LblPlate = New System.Windows.Forms.Label()
        Me.LblPLTNUM = New System.Windows.Forms.Label()
        Me.LblITHING = New System.Windows.Forms.Label()
        Me.LblITLONG = New System.Windows.Forms.Label()
        Me.LblOVER = New System.Windows.Forms.Label()
        Me.LblREGNUM = New System.Windows.Forms.Label()
        Me.LblFTHING = New System.Windows.Forms.Label()
        Me.LblFTLONG = New System.Windows.Forms.Label()
        Me.LblNGPER = New System.Windows.Forms.Label()
        Me.LblFTHINGP = New System.Windows.Forms.Label()
        Me.LblFTLONGP = New System.Windows.Forms.Label()
        Me.LblOVERP = New System.Windows.Forms.Label()
        Me.LblcOK = New System.Windows.Forms.Label()
        Me.LblcITHING = New System.Windows.Forms.Label()
        Me.LblcFTHING = New System.Windows.Forms.Label()
        Me.LblcOVER = New System.Windows.Forms.Label()
        Me.LblcREGNUM = New System.Windows.Forms.Label()
        Me.LblcNG = New System.Windows.Forms.Label()
        Me.LblcITLONG = New System.Windows.Forms.Label()
        Me.LblcFTLONG = New System.Windows.Forms.Label()
        Me.LblcNGPER = New System.Windows.Forms.Label()
        Me.LblcITHINGP = New System.Windows.Forms.Label()
        Me.LblcITLONGP = New System.Windows.Forms.Label()
        Me.LblcFTHINGP = New System.Windows.Forms.Label()
        Me.LblcFTLONGP = New System.Windows.Forms.Label()
        Me.frmHistoryData = New System.Windows.Forms.GroupBox()
        Me.LblcRESVALUE = New System.Windows.Forms.Label()
        Me.LblcLOTNUMBER = New System.Windows.Forms.Label()
        Me.LblITLONGP = New System.Windows.Forms.Label()
        Me.LblITHINGP = New System.Windows.Forms.Label()
        Me.LblcOVERP = New System.Windows.Forms.Label()
        Me.PanelGraph = New System.Windows.Forms.Panel()
        Me.cmdFinal = New System.Windows.Forms.Button()
        Me.LblGrpPer_00 = New System.Windows.Forms.Label()
        Me.LblRegN_00 = New System.Windows.Forms.Label()
        Me.cmdGraphSave = New System.Windows.Forms.Button()
        Me.LblShpGrp_00 = New System.Windows.Forms.Label()
        Me.cmdInitial = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.lblGoodChip2 = New System.Windows.Forms.Label()
        Me.lblNgChip2 = New System.Windows.Forms.Label()
        Me.lblDeviationValue2 = New System.Windows.Forms.Label()
        Me.lblAverageValue2 = New System.Windows.Forms.Label()
        Me.lblMaxValue2 = New System.Windows.Forms.Label()
        Me.lblMinValue2 = New System.Windows.Forms.Label()
        Me.LblShpGrp_11 = New System.Windows.Forms.Label()
        Me.LblShpGrp_10 = New System.Windows.Forms.Label()
        Me.LblShpGrp_09 = New System.Windows.Forms.Label()
        Me.LblShpGrp_08 = New System.Windows.Forms.Label()
        Me.LblShpGrp_07 = New System.Windows.Forms.Label()
        Me.LblShpGrp_06 = New System.Windows.Forms.Label()
        Me.LblShpGrp_05 = New System.Windows.Forms.Label()
        Me.LblShpGrp_04 = New System.Windows.Forms.Label()
        Me.LblShpGrp_03 = New System.Windows.Forms.Label()
        Me.LblShpGrp_02 = New System.Windows.Forms.Label()
        Me.LblShpGrp_01 = New System.Windows.Forms.Label()
        Me.LblGrpPer_11 = New System.Windows.Forms.Label()
        Me.LblGrpPer_10 = New System.Windows.Forms.Label()
        Me.LblGrpPer_09 = New System.Windows.Forms.Label()
        Me.LblGrpPer_08 = New System.Windows.Forms.Label()
        Me.LblGrpPer_07 = New System.Windows.Forms.Label()
        Me.LblGrpPer_06 = New System.Windows.Forms.Label()
        Me.LblGrpPer_05 = New System.Windows.Forms.Label()
        Me.LblGrpPer_04 = New System.Windows.Forms.Label()
        Me.LblGrpPer_03 = New System.Windows.Forms.Label()
        Me.LblGrpPer_02 = New System.Windows.Forms.Label()
        Me.LblGrpPer_01 = New System.Windows.Forms.Label()
        Me.LblRegN_11 = New System.Windows.Forms.Label()
        Me.LblRegN_10 = New System.Windows.Forms.Label()
        Me.LblRegN_09 = New System.Windows.Forms.Label()
        Me.LblRegN_08 = New System.Windows.Forms.Label()
        Me.LblRegN_07 = New System.Windows.Forms.Label()
        Me.LblRegN_06 = New System.Windows.Forms.Label()
        Me.LblRegN_05 = New System.Windows.Forms.Label()
        Me.LblRegN_04 = New System.Windows.Forms.Label()
        Me.LblRegN_03 = New System.Windows.Forms.Label()
        Me.LblRegN_02 = New System.Windows.Forms.Label()
        Me.LblRegN_01 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.lblRegistUnit2 = New System.Windows.Forms.Label()
        Me.lblGraphAccumulationTitle2 = New System.Windows.Forms.Label()
        Me.GrpQrCode = New System.Windows.Forms.GroupBox()
        Me.btnRest = New System.Windows.Forms.Button()
        Me.lblQRData = New System.Windows.Forms.Label()
        Me.CmdSave = New System.Windows.Forms.Button()
        Me.CmdEdit = New System.Windows.Forms.Button()
        Me.CmdLoaderInit = New System.Windows.Forms.Button()
        Me.CmdAutoOperation = New System.Windows.Forms.Button()
        Me.tabCmd = New System.Windows.Forms.TabControl()
        Me.tabBaseCmnds = New System.Windows.Forms.TabPage()
        Me.CmdRecogRough = New System.Windows.Forms.Button()
        Me.CmdIntegrated = New System.Windows.Forms.Button()
        Me.tabOptCmnds = New System.Windows.Forms.TabPage()
        Me.lblProductionData = New System.Windows.Forms.Label()
        Me.CmdFolderOpen = New System.Windows.Forms.Button()
        Me.lblExCamera = New System.Windows.Forms.Label()
        Me.lblCalibration = New System.Windows.Forms.Label()
        Me.lblCutPos = New System.Windows.Forms.Label()
        Me.tabOptCmnd2 = New System.Windows.Forms.TabPage()
        Me.CmdMap = New System.Windows.Forms.Button()
        Me.CmdSinsyukuPtn = New System.Windows.Forms.Button()
        Me.CmdAotoProbeCorrect = New System.Windows.Forms.Button()
        Me.CmdAotoProbePtn = New System.Windows.Forms.Button()
        Me.CmdIDTeach = New System.Windows.Forms.Button()
        Me.lblSinsyuku = New System.Windows.Forms.Label()
        Me.lblAutoProbe = New System.Windows.Forms.Label()
        Me.tabOptCmnd3 = New System.Windows.Forms.TabPage()
        Me.CmdT_ProbeCleaning = New System.Windows.Forms.Button()
        Me.grpDbg = New System.Windows.Forms.GroupBox()
        Me.txtCount = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmdStop = New System.Windows.Forms.Button()
        Me.cmdTestRun = New System.Windows.Forms.Button()
        Me.btnDbgForm = New System.Windows.Forms.Button()
        Me.GrpMode = New System.Windows.Forms.GroupBox()
        Me.btnCycleStop = New System.Windows.Forms.Button()
        Me.BtnSubstrateSet = New System.Windows.Forms.Button()
        Me.BtnADJ = New System.Windows.Forms.Button()
        Me.LblDIGSW_HI = New System.Windows.Forms.Label()
        Me.LblDIGSW = New System.Windows.Forms.Label()
        Me.CbDigSwL = New System.Windows.Forms.ComboBox()
        Me.CbDigSwH = New System.Windows.Forms.ComboBox()
        Me.txtLog = New System.Windows.Forms.TextBox()
        Me.btnGoClipboard = New System.Windows.Forms.Button()
        Me.chkDistributeOnOff = New System.Windows.Forms.CheckBox()
        Me.btnDbg = New System.Windows.Forms.Button()
        Me.lblTwoCount = New System.Windows.Forms.Label()
        Me.lblBreakCount = New System.Windows.Forms.Label()
        Me.GrpNgBox = New System.Windows.Forms.GroupBox()
        Me.lblNgCount = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.CheckBox4 = New System.Windows.Forms.CheckBox()
        Me.CheckBox3 = New System.Windows.Forms.CheckBox()
        Me.CheckBox2 = New System.Windows.Forms.CheckBox()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.TxtBoxPrint = New System.Windows.Forms.TextBox()
        Me.BtnPrint = New System.Windows.Forms.Button()
        Me.BtnPrintOnOff = New System.Windows.Forms.Button()
        Me.GrpStrageBox = New System.Windows.Forms.GroupBox()
        Me.BtnStrageClr = New System.Windows.Forms.Button()
        Me.LblStrageBoxCount = New System.Windows.Forms.Label()
        Me.LblStrageBoxTtl = New System.Windows.Forms.Label()
        Me.pnlDataDisplay = New System.Windows.Forms.Panel()
        Me.BtnPowerOnOff = New System.Windows.Forms.Button()
        Me.LblComandName = New System.Windows.Forms.Label()
        Me.TimerAlarm = New System.Windows.Forms.Timer(Me.components)
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.comDlgOpen = New System.Windows.Forms.OpenFileDialog()
        Me.comDlgSave = New System.Windows.Forms.SaveFileDialog()
        Me.TimerInterLockSts = New System.Windows.Forms.Timer(Me.components)
        Me.TimerQR = New System.Windows.Forms.Timer(Me.components)
        Me.TimerAdjust = New System.Windows.Forms.Timer(Me.components)
        Me.TimerBC = New System.Windows.Forms.Timer(Me.components)
        Me.btnUserLogon = New System.Windows.Forms.Button()
        Me.grpIntegrated = New System.Windows.Forms.GroupBox()
        Me.flpIntegrated = New System.Windows.Forms.FlowLayoutPanel()
        Me.lblIntegRecog = New System.Windows.Forms.Label()
        Me.lblIntegProbe = New System.Windows.Forms.Label()
        Me.lblIntegTX = New System.Windows.Forms.Label()
        Me.lblIntegTeach = New System.Windows.Forms.Label()
        Me.lblIntegTY = New System.Windows.Forms.Label()
        Me.btnJudge = New System.Windows.Forms.Button()
        Me.lblDoorOpen = New System.Windows.Forms.Label()
        Me.GrpStartBlk = New System.Windows.Forms.GroupBox()
        Me.tlpStartBlk = New System.Windows.Forms.TableLayoutPanel()
        Me.CbStartBlkY = New System.Windows.Forms.ComboBox()
        Me.lblStartBlkY = New System.Windows.Forms.Label()
        Me.CbStartBlkX = New System.Windows.Forms.ComboBox()
        Me.lblStartBlkX = New System.Windows.Forms.Label()
        Me.chkContinue = New System.Windows.Forms.CheckBox()
        Me.btnPREV = New System.Windows.Forms.Button()
        Me.btnNEXT = New System.Windows.Forms.Button()
        Me.CmdMapOnOff = New System.Windows.Forms.Button()
        Me.PanelMap = New System.Windows.Forms.Panel()
        Me.CmdPrintMap = New System.Windows.Forms.Button()
        Me.btnQRLmit = New System.Windows.Forms.Button()
        Me.lblCutOff = New System.Windows.Forms.Label()
        Me.BtnAlarmOnOff = New System.Windows.Forms.Button()
        Me.pnlFirstResData = New System.Windows.Forms.Panel()
        Me.CutOffEsEditButton = New System.Windows.Forms.Button()
        Me.lblFrdNomVal = New System.Windows.Forms.Label()
        Me.lblFrdNom = New System.Windows.Forms.Label()
        Me.tlpFirstResData = New System.Windows.Forms.TableLayoutPanel()
        Me.lblFrdCutOff = New System.Windows.Forms.Label()
        Me.lblFrdESPoint = New System.Windows.Forms.Label()
        Me.lblFrd_1 = New System.Windows.Forms.Label()
        Me.lblFrdC1 = New System.Windows.Forms.Label()
        Me.lblFrdE1 = New System.Windows.Forms.Label()
        Me.lblFrd_2 = New System.Windows.Forms.Label()
        Me.lblFrdC2 = New System.Windows.Forms.Label()
        Me.lblFrdE2 = New System.Windows.Forms.Label()
        Me.lblFrd_3 = New System.Windows.Forms.Label()
        Me.lblFrdC3 = New System.Windows.Forms.Label()
        Me.lblFrdE3 = New System.Windows.Forms.Label()
        Me.lblFrd_4 = New System.Windows.Forms.Label()
        Me.lblFrdC4 = New System.Windows.Forms.Label()
        Me.lblFrdE4 = New System.Windows.Forms.Label()
        Me.lblFrd_5 = New System.Windows.Forms.Label()
        Me.lblFrdC5 = New System.Windows.Forms.Label()
        Me.lblFrdE5 = New System.Windows.Forms.Label()
        Me.lblFrd_6 = New System.Windows.Forms.Label()
        Me.lblFrdC6 = New System.Windows.Forms.Label()
        Me.lblFrdE6 = New System.Windows.Forms.Label()
        Me.LabelAutoCalibLimit = New System.Windows.Forms.Label()
        Me.pnlFirstResDataNET = New System.Windows.Forms.Panel()
        Me.CutOffEsEditButtonNET = New System.Windows.Forms.Button()
        Me.lblNETNomVal = New System.Windows.Forms.Label()
        Me.lblNETNom = New System.Windows.Forms.Label()
        Me.tlpFirstResDataNET = New System.Windows.Forms.TableLayoutPanel()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.Res1Cut1 = New System.Windows.Forms.Label()
        Me.Res1Cut2 = New System.Windows.Forms.Label()
        Me.Res1Cut3 = New System.Windows.Forms.Label()
        Me.Res1Cut4 = New System.Windows.Forms.Label()
        Me.Res1Cut5 = New System.Windows.Forms.Label()
        Me.Res1Cut6 = New System.Windows.Forms.Label()
        Me.Res1Cut7 = New System.Windows.Forms.Label()
        Me.Res1Cut8 = New System.Windows.Forms.Label()
        Me.Res1Cut9 = New System.Windows.Forms.Label()
        Me.Res1Cut10 = New System.Windows.Forms.Label()
        Me.Res2Cut1 = New System.Windows.Forms.Label()
        Me.Res2Cut2 = New System.Windows.Forms.Label()
        Me.Res2Cut3 = New System.Windows.Forms.Label()
        Me.Res2Cut4 = New System.Windows.Forms.Label()
        Me.Res2Cut5 = New System.Windows.Forms.Label()
        Me.Res2Cut6 = New System.Windows.Forms.Label()
        Me.Res2Cut7 = New System.Windows.Forms.Label()
        Me.Res2Cut8 = New System.Windows.Forms.Label()
        Me.Res2Cut9 = New System.Windows.Forms.Label()
        Me.Res2Cut10 = New System.Windows.Forms.Label()
        Me.Res3Cut1 = New System.Windows.Forms.Label()
        Me.Res3Cut2 = New System.Windows.Forms.Label()
        Me.Res3Cut3 = New System.Windows.Forms.Label()
        Me.Res3Cut4 = New System.Windows.Forms.Label()
        Me.Res3Cut5 = New System.Windows.Forms.Label()
        Me.Res3Cut6 = New System.Windows.Forms.Label()
        Me.Res3Cut7 = New System.Windows.Forms.Label()
        Me.Res3Cut8 = New System.Windows.Forms.Label()
        Me.Res3Cut9 = New System.Windows.Forms.Label()
        Me.Res3Cut10 = New System.Windows.Forms.Label()
        Me.Res4Cut1 = New System.Windows.Forms.Label()
        Me.Res4Cut2 = New System.Windows.Forms.Label()
        Me.Res4Cut3 = New System.Windows.Forms.Label()
        Me.Res4Cut4 = New System.Windows.Forms.Label()
        Me.Res4Cut5 = New System.Windows.Forms.Label()
        Me.Res4Cut6 = New System.Windows.Forms.Label()
        Me.Res4Cut7 = New System.Windows.Forms.Label()
        Me.Res4Cut8 = New System.Windows.Forms.Label()
        Me.Res4Cut9 = New System.Windows.Forms.Label()
        Me.Res4Cut10 = New System.Windows.Forms.Label()
        Me.System1 = New LaserFront.Trimmer.DllSystem.SystemNET()
        Me.Probe1 = New LaserFront.Trimmer.DllProbeTeach.Probe()
        Me.Ctl_LaserTeach2 = New LaserFront.Trimmer.DllLaserTeach.ctl_LaserTeach()
        Me.Password1 = New LaserFront.Trimmer.DllPassword.Password()
        Me.Teaching1 = New LaserFront.Trimmer.DllTeach.Teaching()
        Me.HelpVersion1 = New LaserFront.Trimmer.DllAbout.HelpVersion()
        Me.ManualTeach1 = New LaserFront.Trimmer.DllManualTeach.ManualTeach()
        Me.Utility1 = New LaserFront.Trimmer.DllUtility.Utility()
        Me.VideoLibrary1 = New LaserFront.Trimmer.DllVideo.VideoLibrary()
        Me.TrimMap1 = New TrimControlLibrary.TrimMapPlate()
        Me.picGraphAccumulation.SuspendLayout()
        Me.frmHistoryData.SuspendLayout()
        Me.PanelGraph.SuspendLayout()
        Me.GrpQrCode.SuspendLayout()
        Me.tabCmd.SuspendLayout()
        Me.tabBaseCmnds.SuspendLayout()
        Me.tabOptCmnds.SuspendLayout()
        Me.tabOptCmnd2.SuspendLayout()
        Me.tabOptCmnd3.SuspendLayout()
        Me.grpDbg.SuspendLayout()
        Me.GrpMode.SuspendLayout()
        Me.GrpNgBox.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GrpStrageBox.SuspendLayout()
        Me.grpIntegrated.SuspendLayout()
        Me.flpIntegrated.SuspendLayout()
        Me.GrpStartBlk.SuspendLayout()
        Me.tlpStartBlk.SuspendLayout()
        Me.PanelMap.SuspendLayout()
        Me.pnlFirstResData.SuspendLayout()
        Me.tlpFirstResData.SuspendLayout()
        Me.pnlFirstResDataNET.SuspendLayout()
        Me.tlpFirstResDataNET.SuspendLayout()
        Me.SuspendLayout()
        '
        'CmdCircuitTeach
        '
        Me.CmdCircuitTeach.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.CmdCircuitTeach.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CmdCircuitTeach, "CmdCircuitTeach")
        Me.CmdCircuitTeach.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdCircuitTeach.Name = "CmdCircuitTeach"
        Me.CmdCircuitTeach.UseVisualStyleBackColor = False
        '
        'cmdEsLog
        '
        Me.cmdEsLog.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEsLog.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdEsLog, "cmdEsLog")
        Me.cmdEsLog.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEsLog.Name = "cmdEsLog"
        Me.cmdEsLog.UseVisualStyleBackColor = False
        '
        'CmdCnd
        '
        Me.CmdCnd.BackColor = System.Drawing.SystemColors.Control
        Me.CmdCnd.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CmdCnd, "CmdCnd")
        Me.CmdCnd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdCnd.Name = "CmdCnd"
        Me.CmdCnd.UseVisualStyleBackColor = False
        '
        'CMdIX2Log
        '
        Me.CMdIX2Log.BackColor = System.Drawing.SystemColors.Control
        Me.CMdIX2Log.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CMdIX2Log, "CMdIX2Log")
        Me.CMdIX2Log.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CMdIX2Log.Name = "CMdIX2Log"
        Me.CMdIX2Log.UseVisualStyleBackColor = False
        '
        'btnTrimming
        '
        Me.btnTrimming.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.btnTrimming.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.btnTrimming, "btnTrimming")
        Me.btnTrimming.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnTrimming.Name = "btnTrimming"
        Me.btnTrimming.UseVisualStyleBackColor = False
        '
        'CmdTx
        '
        Me.CmdTx.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.CmdTx.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CmdTx, "CmdTx")
        Me.CmdTx.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdTx.Name = "CmdTx"
        Me.CmdTx.UseVisualStyleBackColor = False
        '
        'CmdTy
        '
        Me.CmdTy.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.CmdTy.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CmdTy, "CmdTy")
        Me.CmdTy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdTy.Name = "CmdTy"
        Me.CmdTy.UseVisualStyleBackColor = False
        '
        'CmdExCam
        '
        Me.CmdExCam.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.CmdExCam.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CmdExCam, "CmdExCam")
        Me.CmdExCam.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdExCam.Name = "CmdExCam"
        Me.CmdExCam.UseVisualStyleBackColor = False
        '
        'CmdExCam1
        '
        Me.CmdExCam1.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.CmdExCam1.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CmdExCam1, "CmdExCam1")
        Me.CmdExCam1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdExCam1.Name = "CmdExCam1"
        Me.CmdExCam1.UseVisualStyleBackColor = False
        '
        'CmdCutPosCorrect
        '
        Me.CmdCutPosCorrect.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.CmdCutPosCorrect.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CmdCutPosCorrect, "CmdCutPosCorrect")
        Me.CmdCutPosCorrect.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdCutPosCorrect.Name = "CmdCutPosCorrect"
        Me.CmdCutPosCorrect.UseVisualStyleBackColor = False
        '
        'CmdCalibration
        '
        Me.CmdCalibration.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.CmdCalibration.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CmdCalibration, "CmdCalibration")
        Me.CmdCalibration.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdCalibration.Name = "CmdCalibration"
        Me.CmdCalibration.UseVisualStyleBackColor = False
        '
        'CmdPtnCalibration
        '
        Me.CmdPtnCalibration.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.CmdPtnCalibration.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CmdPtnCalibration, "CmdPtnCalibration")
        Me.CmdPtnCalibration.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdPtnCalibration.Name = "CmdPtnCalibration"
        Me.CmdPtnCalibration.UseVisualStyleBackColor = False
        '
        'CmdPtnCutPosCorrect
        '
        Me.CmdPtnCutPosCorrect.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.CmdPtnCutPosCorrect.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CmdPtnCutPosCorrect, "CmdPtnCutPosCorrect")
        Me.CmdPtnCutPosCorrect.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdPtnCutPosCorrect.Name = "CmdPtnCutPosCorrect"
        Me.CmdPtnCutPosCorrect.UseVisualStyleBackColor = False
        '
        'mnuHelpAbout
        '
        Me.mnuHelpAbout.BackColor = System.Drawing.SystemColors.Control
        Me.mnuHelpAbout.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.mnuHelpAbout, "mnuHelpAbout")
        Me.mnuHelpAbout.ForeColor = System.Drawing.SystemColors.ControlText
        Me.mnuHelpAbout.Name = "mnuHelpAbout"
        Me.mnuHelpAbout.UseVisualStyleBackColor = False
        '
        'CmdEnd
        '
        Me.CmdEnd.BackColor = System.Drawing.SystemColors.ControlDark
        Me.CmdEnd.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CmdEnd, "CmdEnd")
        Me.CmdEnd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdEnd.Name = "CmdEnd"
        Me.CmdEnd.UseVisualStyleBackColor = False
        '
        'CmdPattern
        '
        Me.CmdPattern.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        resources.ApplyResources(Me.CmdPattern, "CmdPattern")
        Me.CmdPattern.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmdPattern.FlatAppearance.BorderColor = System.Drawing.Color.GhostWhite
        Me.CmdPattern.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdPattern.Name = "CmdPattern"
        Me.CmdPattern.UseVisualStyleBackColor = False
        '
        'CmdTeach
        '
        Me.CmdTeach.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.CmdTeach.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CmdTeach, "CmdTeach")
        Me.CmdTeach.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdTeach.Name = "CmdTeach"
        Me.CmdTeach.UseVisualStyleBackColor = False
        '
        'CmdProbe
        '
        Me.CmdProbe.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.CmdProbe.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CmdProbe, "CmdProbe")
        Me.CmdProbe.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdProbe.Name = "CmdProbe"
        Me.CmdProbe.UseVisualStyleBackColor = False
        '
        'CmdLoging
        '
        Me.CmdLoging.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.CmdLoging.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CmdLoging, "CmdLoging")
        Me.CmdLoging.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdLoging.Name = "CmdLoging"
        Me.CmdLoging.UseVisualStyleBackColor = False
        '
        'CmdLaser
        '
        Me.CmdLaser.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.CmdLaser.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CmdLaser, "CmdLaser")
        Me.CmdLaser.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdLaser.Name = "CmdLaser"
        Me.CmdLaser.UseVisualStyleBackColor = False
        '
        'CmdLoad
        '
        Me.CmdLoad.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.CmdLoad.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CmdLoad, "CmdLoad")
        Me.CmdLoad.ForeColor = System.Drawing.Color.Black
        Me.CmdLoad.Name = "CmdLoad"
        Me.CmdLoad.UseVisualStyleBackColor = False
        '
        'picGraphAccumulation
        '
        Me.picGraphAccumulation.BackColor = System.Drawing.Color.Black
        Me.picGraphAccumulation.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.picGraphAccumulation.Controls.Add(Me.lblGraphAccumulationTitle)
        Me.picGraphAccumulation.Controls.Add(Me.lblGraphClick)
        Me.picGraphAccumulation.Controls.Add(Me.lblMinValue)
        Me.picGraphAccumulation.Controls.Add(Me.lblMaxValue)
        Me.picGraphAccumulation.Controls.Add(Me.lblAverageValue)
        Me.picGraphAccumulation.Controls.Add(Me.lblDeviationValue)
        Me.picGraphAccumulation.Controls.Add(Me.lblNgChip)
        Me.picGraphAccumulation.Controls.Add(Me.lblGoodChip)
        Me.picGraphAccumulation.Controls.Add(Me.lblDeviation)
        Me.picGraphAccumulation.Controls.Add(Me.lblAverage)
        Me.picGraphAccumulation.Controls.Add(Me.lblMaxTitle)
        Me.picGraphAccumulation.Controls.Add(Me.lblMinTitle)
        Me.picGraphAccumulation.Controls.Add(Me.lblNgTitle)
        Me.picGraphAccumulation.Controls.Add(Me.GrpMatrix)
        Me.picGraphAccumulation.Controls.Add(Me.lblGoodTitle)
        Me.picGraphAccumulation.Controls.Add(Me.lblRegistUnit)
        Me.picGraphAccumulation.Controls.Add(Me._lbglRegistNum_11)
        Me.picGraphAccumulation.Controls.Add(Me._lbglRegistNum_10)
        Me.picGraphAccumulation.Controls.Add(Me._lbglRegistNum_9)
        Me.picGraphAccumulation.Controls.Add(Me._lbglRegistNum_8)
        Me.picGraphAccumulation.Controls.Add(Me._lbglRegistNum_7)
        Me.picGraphAccumulation.Controls.Add(Me._lbglRegistNum_6)
        Me.picGraphAccumulation.Controls.Add(Me._lbglRegistNum_5)
        Me.picGraphAccumulation.Controls.Add(Me._lbglRegistNum_4)
        Me.picGraphAccumulation.Controls.Add(Me._lbglRegistNum_3)
        Me.picGraphAccumulation.Controls.Add(Me._lbglRegistNum_2)
        Me.picGraphAccumulation.Controls.Add(Me._lbglRegistNum_1)
        Me.picGraphAccumulation.Controls.Add(Me._lbglRegistNum_0)
        Me.picGraphAccumulation.Controls.Add(Me.lblRegistTitle)
        Me.picGraphAccumulation.Controls.Add(Me._lblGraphPercent_11)
        Me.picGraphAccumulation.Controls.Add(Me._lblGraphPercent_10)
        Me.picGraphAccumulation.Controls.Add(Me._lblGraphPercent_9)
        Me.picGraphAccumulation.Controls.Add(Me._lblGraphPercent_8)
        Me.picGraphAccumulation.Controls.Add(Me._lblGraphPercent_7)
        Me.picGraphAccumulation.Controls.Add(Me._lblGraphPercent_6)
        Me.picGraphAccumulation.Controls.Add(Me._lblGraphPercent_5)
        Me.picGraphAccumulation.Controls.Add(Me._lblGraphPercent_4)
        Me.picGraphAccumulation.Controls.Add(Me._lblGraphPercent_3)
        Me.picGraphAccumulation.Controls.Add(Me._lblGraphPercent_2)
        Me.picGraphAccumulation.Controls.Add(Me._lblGraphPercent_1)
        Me.picGraphAccumulation.Controls.Add(Me._lblGraphPercent_0)
        Me.picGraphAccumulation.Controls.Add(Me.lblGraphUnit)
        Me.picGraphAccumulation.Cursor = System.Windows.Forms.Cursors.Default
        Me.picGraphAccumulation.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.picGraphAccumulation, "picGraphAccumulation")
        Me.picGraphAccumulation.Name = "picGraphAccumulation"
        Me.picGraphAccumulation.TabStop = True
        '
        'lblGraphAccumulationTitle
        '
        Me.lblGraphAccumulationTitle.BackColor = System.Drawing.Color.Transparent
        Me.lblGraphAccumulationTitle.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblGraphAccumulationTitle, "lblGraphAccumulationTitle")
        Me.lblGraphAccumulationTitle.ForeColor = System.Drawing.Color.Lime
        Me.lblGraphAccumulationTitle.Name = "lblGraphAccumulationTitle"
        '
        'lblGraphClick
        '
        Me.lblGraphClick.BackColor = System.Drawing.Color.Transparent
        Me.lblGraphClick.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGraphClick.ForeColor = System.Drawing.Color.Red
        resources.ApplyResources(Me.lblGraphClick, "lblGraphClick")
        Me.lblGraphClick.Name = "lblGraphClick"
        '
        'lblMinValue
        '
        Me.lblMinValue.BackColor = System.Drawing.Color.Transparent
        Me.lblMinValue.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblMinValue, "lblMinValue")
        Me.lblMinValue.ForeColor = System.Drawing.Color.Green
        Me.lblMinValue.Name = "lblMinValue"
        '
        'lblMaxValue
        '
        Me.lblMaxValue.BackColor = System.Drawing.Color.Transparent
        Me.lblMaxValue.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblMaxValue, "lblMaxValue")
        Me.lblMaxValue.ForeColor = System.Drawing.Color.Green
        Me.lblMaxValue.Name = "lblMaxValue"
        '
        'lblAverageValue
        '
        Me.lblAverageValue.BackColor = System.Drawing.Color.Transparent
        Me.lblAverageValue.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblAverageValue, "lblAverageValue")
        Me.lblAverageValue.ForeColor = System.Drawing.Color.Green
        Me.lblAverageValue.Name = "lblAverageValue"
        '
        'lblDeviationValue
        '
        Me.lblDeviationValue.BackColor = System.Drawing.Color.Transparent
        Me.lblDeviationValue.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblDeviationValue, "lblDeviationValue")
        Me.lblDeviationValue.ForeColor = System.Drawing.Color.Lime
        Me.lblDeviationValue.Name = "lblDeviationValue"
        '
        'lblNgChip
        '
        Me.lblNgChip.BackColor = System.Drawing.Color.Transparent
        Me.lblNgChip.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblNgChip, "lblNgChip")
        Me.lblNgChip.ForeColor = System.Drawing.Color.Green
        Me.lblNgChip.Name = "lblNgChip"
        '
        'lblGoodChip
        '
        Me.lblGoodChip.BackColor = System.Drawing.Color.Transparent
        Me.lblGoodChip.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblGoodChip, "lblGoodChip")
        Me.lblGoodChip.ForeColor = System.Drawing.Color.Green
        Me.lblGoodChip.Name = "lblGoodChip"
        '
        'lblDeviation
        '
        Me.lblDeviation.BackColor = System.Drawing.Color.Transparent
        Me.lblDeviation.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblDeviation, "lblDeviation")
        Me.lblDeviation.ForeColor = System.Drawing.Color.Lime
        Me.lblDeviation.Name = "lblDeviation"
        '
        'lblAverage
        '
        Me.lblAverage.BackColor = System.Drawing.Color.Transparent
        Me.lblAverage.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblAverage, "lblAverage")
        Me.lblAverage.ForeColor = System.Drawing.Color.Green
        Me.lblAverage.Name = "lblAverage"
        '
        'lblMaxTitle
        '
        Me.lblMaxTitle.BackColor = System.Drawing.Color.Transparent
        Me.lblMaxTitle.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblMaxTitle, "lblMaxTitle")
        Me.lblMaxTitle.ForeColor = System.Drawing.Color.Green
        Me.lblMaxTitle.Name = "lblMaxTitle"
        '
        'lblMinTitle
        '
        Me.lblMinTitle.BackColor = System.Drawing.Color.Transparent
        Me.lblMinTitle.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblMinTitle, "lblMinTitle")
        Me.lblMinTitle.ForeColor = System.Drawing.Color.Green
        Me.lblMinTitle.Name = "lblMinTitle"
        '
        'lblNgTitle
        '
        Me.lblNgTitle.BackColor = System.Drawing.Color.Transparent
        Me.lblNgTitle.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblNgTitle, "lblNgTitle")
        Me.lblNgTitle.ForeColor = System.Drawing.Color.Green
        Me.lblNgTitle.Name = "lblNgTitle"
        '
        'GrpMatrix
        '
        resources.ApplyResources(Me.GrpMatrix, "GrpMatrix")
        Me.GrpMatrix.Name = "GrpMatrix"
        Me.GrpMatrix.TabStop = False
        '
        'lblGoodTitle
        '
        Me.lblGoodTitle.BackColor = System.Drawing.Color.Transparent
        Me.lblGoodTitle.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblGoodTitle, "lblGoodTitle")
        Me.lblGoodTitle.ForeColor = System.Drawing.Color.Green
        Me.lblGoodTitle.Name = "lblGoodTitle"
        '
        'lblRegistUnit
        '
        Me.lblRegistUnit.BackColor = System.Drawing.Color.Transparent
        Me.lblRegistUnit.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblRegistUnit, "lblRegistUnit")
        Me.lblRegistUnit.ForeColor = System.Drawing.Color.Green
        Me.lblRegistUnit.Name = "lblRegistUnit"
        '
        '_lbglRegistNum_11
        '
        Me._lbglRegistNum_11.BackColor = System.Drawing.Color.Transparent
        Me._lbglRegistNum_11.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lbglRegistNum_11, "_lbglRegistNum_11")
        Me._lbglRegistNum_11.ForeColor = System.Drawing.Color.Lime
        Me._lbglRegistNum_11.Name = "_lbglRegistNum_11"
        '
        '_lbglRegistNum_10
        '
        Me._lbglRegistNum_10.BackColor = System.Drawing.Color.Transparent
        Me._lbglRegistNum_10.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lbglRegistNum_10, "_lbglRegistNum_10")
        Me._lbglRegistNum_10.ForeColor = System.Drawing.Color.Lime
        Me._lbglRegistNum_10.Name = "_lbglRegistNum_10"
        '
        '_lbglRegistNum_9
        '
        Me._lbglRegistNum_9.BackColor = System.Drawing.Color.Transparent
        Me._lbglRegistNum_9.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lbglRegistNum_9, "_lbglRegistNum_9")
        Me._lbglRegistNum_9.ForeColor = System.Drawing.Color.Lime
        Me._lbglRegistNum_9.Name = "_lbglRegistNum_9"
        '
        '_lbglRegistNum_8
        '
        Me._lbglRegistNum_8.BackColor = System.Drawing.Color.Transparent
        Me._lbglRegistNum_8.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lbglRegistNum_8, "_lbglRegistNum_8")
        Me._lbglRegistNum_8.ForeColor = System.Drawing.Color.Lime
        Me._lbglRegistNum_8.Name = "_lbglRegistNum_8"
        '
        '_lbglRegistNum_7
        '
        Me._lbglRegistNum_7.BackColor = System.Drawing.Color.Transparent
        Me._lbglRegistNum_7.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lbglRegistNum_7, "_lbglRegistNum_7")
        Me._lbglRegistNum_7.ForeColor = System.Drawing.Color.Lime
        Me._lbglRegistNum_7.Name = "_lbglRegistNum_7"
        '
        '_lbglRegistNum_6
        '
        Me._lbglRegistNum_6.BackColor = System.Drawing.Color.Transparent
        Me._lbglRegistNum_6.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lbglRegistNum_6, "_lbglRegistNum_6")
        Me._lbglRegistNum_6.ForeColor = System.Drawing.Color.Lime
        Me._lbglRegistNum_6.Name = "_lbglRegistNum_6"
        '
        '_lbglRegistNum_5
        '
        Me._lbglRegistNum_5.BackColor = System.Drawing.Color.Transparent
        Me._lbglRegistNum_5.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lbglRegistNum_5, "_lbglRegistNum_5")
        Me._lbglRegistNum_5.ForeColor = System.Drawing.Color.Lime
        Me._lbglRegistNum_5.Name = "_lbglRegistNum_5"
        '
        '_lbglRegistNum_4
        '
        Me._lbglRegistNum_4.BackColor = System.Drawing.Color.Transparent
        Me._lbglRegistNum_4.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lbglRegistNum_4, "_lbglRegistNum_4")
        Me._lbglRegistNum_4.ForeColor = System.Drawing.Color.Lime
        Me._lbglRegistNum_4.Name = "_lbglRegistNum_4"
        '
        '_lbglRegistNum_3
        '
        Me._lbglRegistNum_3.BackColor = System.Drawing.Color.Transparent
        Me._lbglRegistNum_3.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lbglRegistNum_3, "_lbglRegistNum_3")
        Me._lbglRegistNum_3.ForeColor = System.Drawing.Color.Lime
        Me._lbglRegistNum_3.Name = "_lbglRegistNum_3"
        '
        '_lbglRegistNum_2
        '
        Me._lbglRegistNum_2.BackColor = System.Drawing.Color.Transparent
        Me._lbglRegistNum_2.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lbglRegistNum_2, "_lbglRegistNum_2")
        Me._lbglRegistNum_2.ForeColor = System.Drawing.Color.Lime
        Me._lbglRegistNum_2.Name = "_lbglRegistNum_2"
        '
        '_lbglRegistNum_1
        '
        Me._lbglRegistNum_1.BackColor = System.Drawing.Color.Transparent
        Me._lbglRegistNum_1.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lbglRegistNum_1, "_lbglRegistNum_1")
        Me._lbglRegistNum_1.ForeColor = System.Drawing.Color.Lime
        Me._lbglRegistNum_1.Name = "_lbglRegistNum_1"
        '
        '_lbglRegistNum_0
        '
        Me._lbglRegistNum_0.BackColor = System.Drawing.Color.Transparent
        Me._lbglRegistNum_0.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lbglRegistNum_0, "_lbglRegistNum_0")
        Me._lbglRegistNum_0.ForeColor = System.Drawing.Color.Lime
        Me._lbglRegistNum_0.Name = "_lbglRegistNum_0"
        '
        'lblRegistTitle
        '
        Me.lblRegistTitle.BackColor = System.Drawing.Color.Transparent
        Me.lblRegistTitle.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblRegistTitle, "lblRegistTitle")
        Me.lblRegistTitle.ForeColor = System.Drawing.Color.Lime
        Me.lblRegistTitle.Name = "lblRegistTitle"
        '
        '_lblGraphPercent_11
        '
        Me._lblGraphPercent_11.BackColor = System.Drawing.Color.Transparent
        Me._lblGraphPercent_11.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblGraphPercent_11, "_lblGraphPercent_11")
        Me._lblGraphPercent_11.ForeColor = System.Drawing.Color.Green
        Me._lblGraphPercent_11.Name = "_lblGraphPercent_11"
        '
        '_lblGraphPercent_10
        '
        Me._lblGraphPercent_10.BackColor = System.Drawing.Color.Transparent
        Me._lblGraphPercent_10.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblGraphPercent_10, "_lblGraphPercent_10")
        Me._lblGraphPercent_10.ForeColor = System.Drawing.Color.Green
        Me._lblGraphPercent_10.Name = "_lblGraphPercent_10"
        '
        '_lblGraphPercent_9
        '
        Me._lblGraphPercent_9.BackColor = System.Drawing.Color.Transparent
        Me._lblGraphPercent_9.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblGraphPercent_9, "_lblGraphPercent_9")
        Me._lblGraphPercent_9.ForeColor = System.Drawing.Color.Green
        Me._lblGraphPercent_9.Name = "_lblGraphPercent_9"
        '
        '_lblGraphPercent_8
        '
        Me._lblGraphPercent_8.BackColor = System.Drawing.Color.Transparent
        Me._lblGraphPercent_8.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblGraphPercent_8, "_lblGraphPercent_8")
        Me._lblGraphPercent_8.ForeColor = System.Drawing.Color.Green
        Me._lblGraphPercent_8.Name = "_lblGraphPercent_8"
        '
        '_lblGraphPercent_7
        '
        resources.ApplyResources(Me._lblGraphPercent_7, "_lblGraphPercent_7")
        Me._lblGraphPercent_7.BackColor = System.Drawing.Color.Transparent
        Me._lblGraphPercent_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblGraphPercent_7.ForeColor = System.Drawing.Color.Green
        Me._lblGraphPercent_7.Name = "_lblGraphPercent_7"
        '
        '_lblGraphPercent_6
        '
        resources.ApplyResources(Me._lblGraphPercent_6, "_lblGraphPercent_6")
        Me._lblGraphPercent_6.BackColor = System.Drawing.Color.Transparent
        Me._lblGraphPercent_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblGraphPercent_6.ForeColor = System.Drawing.Color.Green
        Me._lblGraphPercent_6.Name = "_lblGraphPercent_6"
        '
        '_lblGraphPercent_5
        '
        resources.ApplyResources(Me._lblGraphPercent_5, "_lblGraphPercent_5")
        Me._lblGraphPercent_5.BackColor = System.Drawing.Color.Transparent
        Me._lblGraphPercent_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblGraphPercent_5.ForeColor = System.Drawing.Color.Green
        Me._lblGraphPercent_5.Name = "_lblGraphPercent_5"
        '
        '_lblGraphPercent_4
        '
        resources.ApplyResources(Me._lblGraphPercent_4, "_lblGraphPercent_4")
        Me._lblGraphPercent_4.BackColor = System.Drawing.Color.Transparent
        Me._lblGraphPercent_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblGraphPercent_4.ForeColor = System.Drawing.Color.Green
        Me._lblGraphPercent_4.Name = "_lblGraphPercent_4"
        '
        '_lblGraphPercent_3
        '
        resources.ApplyResources(Me._lblGraphPercent_3, "_lblGraphPercent_3")
        Me._lblGraphPercent_3.BackColor = System.Drawing.Color.Transparent
        Me._lblGraphPercent_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblGraphPercent_3.ForeColor = System.Drawing.Color.Green
        Me._lblGraphPercent_3.Name = "_lblGraphPercent_3"
        '
        '_lblGraphPercent_2
        '
        resources.ApplyResources(Me._lblGraphPercent_2, "_lblGraphPercent_2")
        Me._lblGraphPercent_2.BackColor = System.Drawing.Color.Transparent
        Me._lblGraphPercent_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblGraphPercent_2.ForeColor = System.Drawing.Color.Green
        Me._lblGraphPercent_2.Name = "_lblGraphPercent_2"
        '
        '_lblGraphPercent_1
        '
        resources.ApplyResources(Me._lblGraphPercent_1, "_lblGraphPercent_1")
        Me._lblGraphPercent_1.BackColor = System.Drawing.Color.Transparent
        Me._lblGraphPercent_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblGraphPercent_1.ForeColor = System.Drawing.Color.Green
        Me._lblGraphPercent_1.Name = "_lblGraphPercent_1"
        '
        '_lblGraphPercent_0
        '
        resources.ApplyResources(Me._lblGraphPercent_0, "_lblGraphPercent_0")
        Me._lblGraphPercent_0.BackColor = System.Drawing.Color.Transparent
        Me._lblGraphPercent_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblGraphPercent_0.ForeColor = System.Drawing.Color.Green
        Me._lblGraphPercent_0.Name = "_lblGraphPercent_0"
        '
        'lblGraphUnit
        '
        Me.lblGraphUnit.BackColor = System.Drawing.Color.Transparent
        Me.lblGraphUnit.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblGraphUnit, "lblGraphUnit")
        Me.lblGraphUnit.ForeColor = System.Drawing.Color.Green
        Me.lblGraphUnit.Name = "lblGraphUnit"
        '
        'lblInterLockMSG
        '
        Me.lblInterLockMSG.BackColor = System.Drawing.Color.Yellow
        Me.lblInterLockMSG.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblInterLockMSG.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblInterLockMSG, "lblInterLockMSG")
        Me.lblInterLockMSG.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInterLockMSG.Name = "lblInterLockMSG"
        '
        'CmdTy2
        '
        Me.CmdTy2.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.CmdTy2.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CmdTy2, "CmdTy2")
        Me.CmdTy2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdTy2.Name = "CmdTy2"
        Me.CmdTy2.UseVisualStyleBackColor = False
        '
        'CmdT_Theta
        '
        Me.CmdT_Theta.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.CmdT_Theta.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CmdT_Theta, "CmdT_Theta")
        Me.CmdT_Theta.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdT_Theta.Name = "CmdT_Theta"
        Me.CmdT_Theta.UseVisualStyleBackColor = False
        '
        'Text4
        '
        Me.Text4.AcceptsReturn = True
        Me.Text4.BackColor = System.Drawing.Color.LightGray
        Me.Text4.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.Text4, "Text4")
        Me.Text4.ForeColor = System.Drawing.Color.White
        Me.Text4.Name = "Text4"
        Me.Text4.ReadOnly = True
        '
        'LblMes
        '
        Me.LblMes.BackColor = System.Drawing.Color.Lime
        Me.LblMes.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LblMes.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblMes, "LblMes")
        Me.LblMes.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblMes.Name = "LblMes"
        '
        'LblCur
        '
        Me.LblCur.BackColor = System.Drawing.Color.Lime
        Me.LblCur.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LblCur.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblCur, "LblCur")
        Me.LblCur.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblCur.Name = "LblCur"
        '
        'lblLoginResult
        '
        resources.ApplyResources(Me.lblLoginResult, "lblLoginResult")
        Me.lblLoginResult.BackColor = System.Drawing.Color.Yellow
        Me.lblLoginResult.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblLoginResult.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLoginResult.ForeColor = System.Drawing.Color.Black
        Me.lblLoginResult.Name = "lblLoginResult"
        '
        'LblDataFileName
        '
        Me.LblDataFileName.AccessibleRole = System.Windows.Forms.AccessibleRole.None
        resources.ApplyResources(Me.LblDataFileName, "LblDataFileName")
        Me.LblDataFileName.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.LblDataFileName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LblDataFileName.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblDataFileName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblDataFileName.Name = "LblDataFileName"
        '
        'Line1
        '
        Me.Line1.BackColor = System.Drawing.SystemColors.ControlDark
        resources.ApplyResources(Me.Line1, "Line1")
        Me.Line1.Name = "Line1"
        '
        'lblLogging
        '
        resources.ApplyResources(Me.lblLogging, "lblLogging")
        Me.lblLogging.BackColor = System.Drawing.Color.Yellow
        Me.lblLogging.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLogging.ForeColor = System.Drawing.SystemColors.MenuText
        Me.lblLogging.Name = "lblLogging"
        '
        'LblRotAtt
        '
        Me.LblRotAtt.BackColor = System.Drawing.Color.Lime
        Me.LblRotAtt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LblRotAtt.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblRotAtt, "LblRotAtt")
        Me.LblRotAtt.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblRotAtt.Name = "LblRotAtt"
        '
        'CmdCutPos
        '
        Me.CmdCutPos.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.CmdCutPos.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CmdCutPos, "CmdCutPos")
        Me.CmdCutPos.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdCutPos.Name = "CmdCutPos"
        Me.CmdCutPos.UseVisualStyleBackColor = False
        '
        'Line2
        '
        Me.Line2.BackColor = System.Drawing.SystemColors.WindowText
        resources.ApplyResources(Me.Line2, "Line2")
        Me.Line2.Name = "Line2"
        '
        'Line3
        '
        Me.Line3.BackColor = System.Drawing.SystemColors.WindowText
        resources.ApplyResources(Me.Line3, "Line3")
        Me.Line3.Name = "Line3"
        '
        'btnCounterClear
        '
        Me.btnCounterClear.BackColor = System.Drawing.Color.White
        Me.btnCounterClear.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.btnCounterClear, "btnCounterClear")
        Me.btnCounterClear.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnCounterClear.Name = "btnCounterClear"
        Me.btnCounterClear.UseVisualStyleBackColor = False
        '
        'LblGO
        '
        Me.LblGO.BackColor = System.Drawing.Color.Transparent
        resources.ApplyResources(Me.LblGO, "LblGO")
        Me.LblGO.Name = "LblGO"
        '
        'LblTNG
        '
        Me.LblTNG.BackColor = System.Drawing.Color.Transparent
        resources.ApplyResources(Me.LblTNG, "LblTNG")
        Me.LblTNG.Name = "LblTNG"
        '
        'LblUnit
        '
        resources.ApplyResources(Me.LblUnit, "LblUnit")
        Me.LblUnit.Name = "LblUnit"
        '
        'LblPlate
        '
        resources.ApplyResources(Me.LblPlate, "LblPlate")
        Me.LblPlate.Name = "LblPlate"
        '
        'LblPLTNUM
        '
        Me.LblPLTNUM.BackColor = System.Drawing.Color.Transparent
        resources.ApplyResources(Me.LblPLTNUM, "LblPLTNUM")
        Me.LblPLTNUM.Name = "LblPLTNUM"
        '
        'LblITHING
        '
        Me.LblITHING.BackColor = System.Drawing.Color.Transparent
        resources.ApplyResources(Me.LblITHING, "LblITHING")
        Me.LblITHING.Name = "LblITHING"
        '
        'LblITLONG
        '
        Me.LblITLONG.BackColor = System.Drawing.Color.Transparent
        resources.ApplyResources(Me.LblITLONG, "LblITLONG")
        Me.LblITLONG.Name = "LblITLONG"
        '
        'LblOVER
        '
        Me.LblOVER.BackColor = System.Drawing.Color.Transparent
        resources.ApplyResources(Me.LblOVER, "LblOVER")
        Me.LblOVER.Name = "LblOVER"
        '
        'LblREGNUM
        '
        Me.LblREGNUM.BackColor = System.Drawing.Color.Transparent
        resources.ApplyResources(Me.LblREGNUM, "LblREGNUM")
        Me.LblREGNUM.Name = "LblREGNUM"
        '
        'LblFTHING
        '
        Me.LblFTHING.BackColor = System.Drawing.Color.Transparent
        resources.ApplyResources(Me.LblFTHING, "LblFTHING")
        Me.LblFTHING.Name = "LblFTHING"
        '
        'LblFTLONG
        '
        Me.LblFTLONG.BackColor = System.Drawing.Color.Transparent
        resources.ApplyResources(Me.LblFTLONG, "LblFTLONG")
        Me.LblFTLONG.Name = "LblFTLONG"
        '
        'LblNGPER
        '
        Me.LblNGPER.BackColor = System.Drawing.Color.Transparent
        resources.ApplyResources(Me.LblNGPER, "LblNGPER")
        Me.LblNGPER.Name = "LblNGPER"
        '
        'LblFTHINGP
        '
        Me.LblFTHINGP.BackColor = System.Drawing.Color.Transparent
        resources.ApplyResources(Me.LblFTHINGP, "LblFTHINGP")
        Me.LblFTHINGP.Name = "LblFTHINGP"
        '
        'LblFTLONGP
        '
        Me.LblFTLONGP.BackColor = System.Drawing.Color.Transparent
        resources.ApplyResources(Me.LblFTLONGP, "LblFTLONGP")
        Me.LblFTLONGP.Name = "LblFTLONGP"
        '
        'LblOVERP
        '
        Me.LblOVERP.BackColor = System.Drawing.Color.Transparent
        resources.ApplyResources(Me.LblOVERP, "LblOVERP")
        Me.LblOVERP.Name = "LblOVERP"
        '
        'LblcOK
        '
        resources.ApplyResources(Me.LblcOK, "LblcOK")
        Me.LblcOK.Name = "LblcOK"
        '
        'LblcITHING
        '
        resources.ApplyResources(Me.LblcITHING, "LblcITHING")
        Me.LblcITHING.Name = "LblcITHING"
        '
        'LblcFTHING
        '
        resources.ApplyResources(Me.LblcFTHING, "LblcFTHING")
        Me.LblcFTHING.Name = "LblcFTHING"
        '
        'LblcOVER
        '
        resources.ApplyResources(Me.LblcOVER, "LblcOVER")
        Me.LblcOVER.Name = "LblcOVER"
        '
        'LblcREGNUM
        '
        resources.ApplyResources(Me.LblcREGNUM, "LblcREGNUM")
        Me.LblcREGNUM.Name = "LblcREGNUM"
        '
        'LblcNG
        '
        resources.ApplyResources(Me.LblcNG, "LblcNG")
        Me.LblcNG.Name = "LblcNG"
        '
        'LblcITLONG
        '
        resources.ApplyResources(Me.LblcITLONG, "LblcITLONG")
        Me.LblcITLONG.Name = "LblcITLONG"
        '
        'LblcFTLONG
        '
        resources.ApplyResources(Me.LblcFTLONG, "LblcFTLONG")
        Me.LblcFTLONG.Name = "LblcFTLONG"
        '
        'LblcNGPER
        '
        resources.ApplyResources(Me.LblcNGPER, "LblcNGPER")
        Me.LblcNGPER.Name = "LblcNGPER"
        '
        'LblcITHINGP
        '
        resources.ApplyResources(Me.LblcITHINGP, "LblcITHINGP")
        Me.LblcITHINGP.Name = "LblcITHINGP"
        '
        'LblcITLONGP
        '
        resources.ApplyResources(Me.LblcITLONGP, "LblcITLONGP")
        Me.LblcITLONGP.Name = "LblcITLONGP"
        '
        'LblcFTHINGP
        '
        resources.ApplyResources(Me.LblcFTHINGP, "LblcFTHINGP")
        Me.LblcFTHINGP.Name = "LblcFTHINGP"
        '
        'LblcFTLONGP
        '
        resources.ApplyResources(Me.LblcFTLONGP, "LblcFTLONGP")
        Me.LblcFTLONGP.Name = "LblcFTLONGP"
        '
        'frmHistoryData
        '
        Me.frmHistoryData.BackColor = System.Drawing.SystemColors.Control
        Me.frmHistoryData.Controls.Add(Me.LblcRESVALUE)
        Me.frmHistoryData.Controls.Add(Me.LblcLOTNUMBER)
        Me.frmHistoryData.Controls.Add(Me.LblITLONGP)
        Me.frmHistoryData.Controls.Add(Me.LblITHINGP)
        Me.frmHistoryData.Controls.Add(Me.LblcOVERP)
        Me.frmHistoryData.Controls.Add(Me.LblcFTLONGP)
        Me.frmHistoryData.Controls.Add(Me.LblcFTHINGP)
        Me.frmHistoryData.Controls.Add(Me.LblcITLONGP)
        Me.frmHistoryData.Controls.Add(Me.LblcITHINGP)
        Me.frmHistoryData.Controls.Add(Me.LblcNGPER)
        Me.frmHistoryData.Controls.Add(Me.LblcFTLONG)
        Me.frmHistoryData.Controls.Add(Me.LblcITLONG)
        Me.frmHistoryData.Controls.Add(Me.LblcNG)
        Me.frmHistoryData.Controls.Add(Me.LblcREGNUM)
        Me.frmHistoryData.Controls.Add(Me.LblcOVER)
        Me.frmHistoryData.Controls.Add(Me.LblcFTHING)
        Me.frmHistoryData.Controls.Add(Me.LblcITHING)
        Me.frmHistoryData.Controls.Add(Me.LblcOK)
        Me.frmHistoryData.Controls.Add(Me.LblOVERP)
        Me.frmHistoryData.Controls.Add(Me.LblFTLONGP)
        Me.frmHistoryData.Controls.Add(Me.LblFTHINGP)
        Me.frmHistoryData.Controls.Add(Me.LblNGPER)
        Me.frmHistoryData.Controls.Add(Me.LblFTLONG)
        Me.frmHistoryData.Controls.Add(Me.LblFTHING)
        Me.frmHistoryData.Controls.Add(Me.LblREGNUM)
        Me.frmHistoryData.Controls.Add(Me.LblOVER)
        Me.frmHistoryData.Controls.Add(Me.LblITLONG)
        Me.frmHistoryData.Controls.Add(Me.LblITHING)
        Me.frmHistoryData.Controls.Add(Me.LblPLTNUM)
        Me.frmHistoryData.Controls.Add(Me.LblPlate)
        Me.frmHistoryData.Controls.Add(Me.LblUnit)
        Me.frmHistoryData.Controls.Add(Me.LblTNG)
        Me.frmHistoryData.Controls.Add(Me.LblGO)
        Me.frmHistoryData.Controls.Add(Me.btnCounterClear)
        Me.frmHistoryData.Controls.Add(Me.Line3)
        Me.frmHistoryData.Controls.Add(Me.Line2)
        Me.frmHistoryData.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.frmHistoryData, "frmHistoryData")
        Me.frmHistoryData.Name = "frmHistoryData"
        Me.frmHistoryData.TabStop = False
        '
        'LblcRESVALUE
        '
        resources.ApplyResources(Me.LblcRESVALUE, "LblcRESVALUE")
        Me.LblcRESVALUE.Name = "LblcRESVALUE"
        '
        'LblcLOTNUMBER
        '
        resources.ApplyResources(Me.LblcLOTNUMBER, "LblcLOTNUMBER")
        Me.LblcLOTNUMBER.Name = "LblcLOTNUMBER"
        '
        'LblITLONGP
        '
        Me.LblITLONGP.BackColor = System.Drawing.Color.Transparent
        resources.ApplyResources(Me.LblITLONGP, "LblITLONGP")
        Me.LblITLONGP.Name = "LblITLONGP"
        '
        'LblITHINGP
        '
        Me.LblITHINGP.BackColor = System.Drawing.Color.Transparent
        resources.ApplyResources(Me.LblITHINGP, "LblITHINGP")
        Me.LblITHINGP.Name = "LblITHINGP"
        '
        'LblcOVERP
        '
        resources.ApplyResources(Me.LblcOVERP, "LblcOVERP")
        Me.LblcOVERP.Name = "LblcOVERP"
        '
        'PanelGraph
        '
        Me.PanelGraph.BackColor = System.Drawing.Color.Black
        Me.PanelGraph.Controls.Add(Me.cmdFinal)
        Me.PanelGraph.Controls.Add(Me.LblGrpPer_00)
        Me.PanelGraph.Controls.Add(Me.LblRegN_00)
        Me.PanelGraph.Controls.Add(Me.cmdGraphSave)
        Me.PanelGraph.Controls.Add(Me.LblShpGrp_00)
        Me.PanelGraph.Controls.Add(Me.cmdInitial)
        Me.PanelGraph.Controls.Add(Me.Label2)
        Me.PanelGraph.Controls.Add(Me.Label3)
        Me.PanelGraph.Controls.Add(Me.Label4)
        Me.PanelGraph.Controls.Add(Me.Label5)
        Me.PanelGraph.Controls.Add(Me.Label6)
        Me.PanelGraph.Controls.Add(Me.Label7)
        Me.PanelGraph.Controls.Add(Me.lblGoodChip2)
        Me.PanelGraph.Controls.Add(Me.lblNgChip2)
        Me.PanelGraph.Controls.Add(Me.lblDeviationValue2)
        Me.PanelGraph.Controls.Add(Me.lblAverageValue2)
        Me.PanelGraph.Controls.Add(Me.lblMaxValue2)
        Me.PanelGraph.Controls.Add(Me.lblMinValue2)
        Me.PanelGraph.Controls.Add(Me.LblShpGrp_11)
        Me.PanelGraph.Controls.Add(Me.LblShpGrp_10)
        Me.PanelGraph.Controls.Add(Me.LblShpGrp_09)
        Me.PanelGraph.Controls.Add(Me.LblShpGrp_08)
        Me.PanelGraph.Controls.Add(Me.LblShpGrp_07)
        Me.PanelGraph.Controls.Add(Me.LblShpGrp_06)
        Me.PanelGraph.Controls.Add(Me.LblShpGrp_05)
        Me.PanelGraph.Controls.Add(Me.LblShpGrp_04)
        Me.PanelGraph.Controls.Add(Me.LblShpGrp_03)
        Me.PanelGraph.Controls.Add(Me.LblShpGrp_02)
        Me.PanelGraph.Controls.Add(Me.LblShpGrp_01)
        Me.PanelGraph.Controls.Add(Me.LblGrpPer_11)
        Me.PanelGraph.Controls.Add(Me.LblGrpPer_10)
        Me.PanelGraph.Controls.Add(Me.LblGrpPer_09)
        Me.PanelGraph.Controls.Add(Me.LblGrpPer_08)
        Me.PanelGraph.Controls.Add(Me.LblGrpPer_07)
        Me.PanelGraph.Controls.Add(Me.LblGrpPer_06)
        Me.PanelGraph.Controls.Add(Me.LblGrpPer_05)
        Me.PanelGraph.Controls.Add(Me.LblGrpPer_04)
        Me.PanelGraph.Controls.Add(Me.LblGrpPer_03)
        Me.PanelGraph.Controls.Add(Me.LblGrpPer_02)
        Me.PanelGraph.Controls.Add(Me.LblGrpPer_01)
        Me.PanelGraph.Controls.Add(Me.LblRegN_11)
        Me.PanelGraph.Controls.Add(Me.LblRegN_10)
        Me.PanelGraph.Controls.Add(Me.LblRegN_09)
        Me.PanelGraph.Controls.Add(Me.LblRegN_08)
        Me.PanelGraph.Controls.Add(Me.LblRegN_07)
        Me.PanelGraph.Controls.Add(Me.LblRegN_06)
        Me.PanelGraph.Controls.Add(Me.LblRegN_05)
        Me.PanelGraph.Controls.Add(Me.LblRegN_04)
        Me.PanelGraph.Controls.Add(Me.LblRegN_03)
        Me.PanelGraph.Controls.Add(Me.LblRegN_02)
        Me.PanelGraph.Controls.Add(Me.LblRegN_01)
        Me.PanelGraph.Controls.Add(Me.Label14)
        Me.PanelGraph.Controls.Add(Me.Label15)
        Me.PanelGraph.Controls.Add(Me.Label16)
        Me.PanelGraph.Controls.Add(Me.Label17)
        Me.PanelGraph.Controls.Add(Me.Label18)
        Me.PanelGraph.Controls.Add(Me.lblRegistUnit2)
        Me.PanelGraph.Controls.Add(Me.lblGraphAccumulationTitle2)
        Me.PanelGraph.Cursor = System.Windows.Forms.Cursors.Default
        Me.PanelGraph.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.PanelGraph, "PanelGraph")
        Me.PanelGraph.Name = "PanelGraph"
        Me.PanelGraph.TabStop = True
        '
        'cmdFinal
        '
        Me.cmdFinal.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFinal.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdFinal, "cmdFinal")
        Me.cmdFinal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFinal.Name = "cmdFinal"
        Me.cmdFinal.UseVisualStyleBackColor = False
        '
        'LblGrpPer_00
        '
        Me.LblGrpPer_00.BackColor = System.Drawing.Color.Black
        Me.LblGrpPer_00.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblGrpPer_00, "LblGrpPer_00")
        Me.LblGrpPer_00.ForeColor = System.Drawing.Color.Lime
        Me.LblGrpPer_00.Name = "LblGrpPer_00"
        '
        'LblRegN_00
        '
        Me.LblRegN_00.BackColor = System.Drawing.Color.Transparent
        Me.LblRegN_00.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblRegN_00, "LblRegN_00")
        Me.LblRegN_00.ForeColor = System.Drawing.Color.Lime
        Me.LblRegN_00.Name = "LblRegN_00"
        '
        'cmdGraphSave
        '
        Me.cmdGraphSave.BackColor = System.Drawing.SystemColors.Control
        Me.cmdGraphSave.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdGraphSave, "cmdGraphSave")
        Me.cmdGraphSave.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdGraphSave.Name = "cmdGraphSave"
        Me.cmdGraphSave.UseVisualStyleBackColor = False
        '
        'LblShpGrp_00
        '
        Me.LblShpGrp_00.BackColor = System.Drawing.Color.Cyan
        Me.LblShpGrp_00.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblShpGrp_00, "LblShpGrp_00")
        Me.LblShpGrp_00.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblShpGrp_00.Name = "LblShpGrp_00"
        '
        'cmdInitial
        '
        Me.cmdInitial.BackColor = System.Drawing.SystemColors.Control
        Me.cmdInitial.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdInitial, "cmdInitial")
        Me.cmdInitial.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdInitial.Name = "cmdInitial"
        Me.cmdInitial.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.ForeColor = System.Drawing.Color.Lime
        Me.Label2.Name = "Label2"
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.ForeColor = System.Drawing.Color.Lime
        Me.Label3.Name = "Label3"
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.ForeColor = System.Drawing.Color.Lime
        Me.Label4.Name = "Label4"
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label5, "Label5")
        Me.Label5.ForeColor = System.Drawing.Color.Lime
        Me.Label5.Name = "Label5"
        '
        'Label6
        '
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label6, "Label6")
        Me.Label6.ForeColor = System.Drawing.Color.Lime
        Me.Label6.Name = "Label6"
        '
        'Label7
        '
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label7, "Label7")
        Me.Label7.ForeColor = System.Drawing.Color.Lime
        Me.Label7.Name = "Label7"
        '
        'lblGoodChip2
        '
        Me.lblGoodChip2.BackColor = System.Drawing.Color.Transparent
        Me.lblGoodChip2.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblGoodChip2, "lblGoodChip2")
        Me.lblGoodChip2.ForeColor = System.Drawing.Color.Lime
        Me.lblGoodChip2.Name = "lblGoodChip2"
        '
        'lblNgChip2
        '
        Me.lblNgChip2.BackColor = System.Drawing.Color.Transparent
        Me.lblNgChip2.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblNgChip2, "lblNgChip2")
        Me.lblNgChip2.ForeColor = System.Drawing.Color.Lime
        Me.lblNgChip2.Name = "lblNgChip2"
        '
        'lblDeviationValue2
        '
        Me.lblDeviationValue2.BackColor = System.Drawing.Color.Transparent
        Me.lblDeviationValue2.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblDeviationValue2, "lblDeviationValue2")
        Me.lblDeviationValue2.ForeColor = System.Drawing.Color.Lime
        Me.lblDeviationValue2.Name = "lblDeviationValue2"
        '
        'lblAverageValue2
        '
        Me.lblAverageValue2.BackColor = System.Drawing.Color.Transparent
        Me.lblAverageValue2.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblAverageValue2, "lblAverageValue2")
        Me.lblAverageValue2.ForeColor = System.Drawing.Color.Lime
        Me.lblAverageValue2.Name = "lblAverageValue2"
        '
        'lblMaxValue2
        '
        Me.lblMaxValue2.BackColor = System.Drawing.Color.Transparent
        Me.lblMaxValue2.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblMaxValue2, "lblMaxValue2")
        Me.lblMaxValue2.ForeColor = System.Drawing.Color.Lime
        Me.lblMaxValue2.Name = "lblMaxValue2"
        '
        'lblMinValue2
        '
        Me.lblMinValue2.BackColor = System.Drawing.Color.Transparent
        Me.lblMinValue2.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblMinValue2, "lblMinValue2")
        Me.lblMinValue2.ForeColor = System.Drawing.Color.Lime
        Me.lblMinValue2.Name = "lblMinValue2"
        '
        'LblShpGrp_11
        '
        Me.LblShpGrp_11.BackColor = System.Drawing.Color.Cyan
        Me.LblShpGrp_11.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblShpGrp_11, "LblShpGrp_11")
        Me.LblShpGrp_11.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblShpGrp_11.Name = "LblShpGrp_11"
        '
        'LblShpGrp_10
        '
        Me.LblShpGrp_10.BackColor = System.Drawing.Color.Cyan
        Me.LblShpGrp_10.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblShpGrp_10, "LblShpGrp_10")
        Me.LblShpGrp_10.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblShpGrp_10.Name = "LblShpGrp_10"
        '
        'LblShpGrp_09
        '
        Me.LblShpGrp_09.BackColor = System.Drawing.Color.Cyan
        Me.LblShpGrp_09.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblShpGrp_09, "LblShpGrp_09")
        Me.LblShpGrp_09.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblShpGrp_09.Name = "LblShpGrp_09"
        '
        'LblShpGrp_08
        '
        Me.LblShpGrp_08.BackColor = System.Drawing.Color.Cyan
        Me.LblShpGrp_08.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblShpGrp_08, "LblShpGrp_08")
        Me.LblShpGrp_08.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblShpGrp_08.Name = "LblShpGrp_08"
        '
        'LblShpGrp_07
        '
        Me.LblShpGrp_07.BackColor = System.Drawing.Color.Cyan
        Me.LblShpGrp_07.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblShpGrp_07, "LblShpGrp_07")
        Me.LblShpGrp_07.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblShpGrp_07.Name = "LblShpGrp_07"
        '
        'LblShpGrp_06
        '
        Me.LblShpGrp_06.BackColor = System.Drawing.Color.Cyan
        Me.LblShpGrp_06.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblShpGrp_06, "LblShpGrp_06")
        Me.LblShpGrp_06.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblShpGrp_06.Name = "LblShpGrp_06"
        '
        'LblShpGrp_05
        '
        Me.LblShpGrp_05.BackColor = System.Drawing.Color.Cyan
        Me.LblShpGrp_05.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblShpGrp_05, "LblShpGrp_05")
        Me.LblShpGrp_05.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblShpGrp_05.Name = "LblShpGrp_05"
        '
        'LblShpGrp_04
        '
        Me.LblShpGrp_04.BackColor = System.Drawing.Color.Cyan
        Me.LblShpGrp_04.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblShpGrp_04, "LblShpGrp_04")
        Me.LblShpGrp_04.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblShpGrp_04.Name = "LblShpGrp_04"
        '
        'LblShpGrp_03
        '
        Me.LblShpGrp_03.BackColor = System.Drawing.Color.Cyan
        Me.LblShpGrp_03.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblShpGrp_03, "LblShpGrp_03")
        Me.LblShpGrp_03.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblShpGrp_03.Name = "LblShpGrp_03"
        '
        'LblShpGrp_02
        '
        Me.LblShpGrp_02.BackColor = System.Drawing.Color.Cyan
        Me.LblShpGrp_02.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblShpGrp_02, "LblShpGrp_02")
        Me.LblShpGrp_02.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblShpGrp_02.Name = "LblShpGrp_02"
        '
        'LblShpGrp_01
        '
        Me.LblShpGrp_01.BackColor = System.Drawing.Color.Cyan
        Me.LblShpGrp_01.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblShpGrp_01, "LblShpGrp_01")
        Me.LblShpGrp_01.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblShpGrp_01.Name = "LblShpGrp_01"
        '
        'LblGrpPer_11
        '
        Me.LblGrpPer_11.BackColor = System.Drawing.Color.Transparent
        Me.LblGrpPer_11.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblGrpPer_11, "LblGrpPer_11")
        Me.LblGrpPer_11.ForeColor = System.Drawing.Color.Lime
        Me.LblGrpPer_11.Name = "LblGrpPer_11"
        '
        'LblGrpPer_10
        '
        Me.LblGrpPer_10.BackColor = System.Drawing.Color.Transparent
        Me.LblGrpPer_10.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblGrpPer_10, "LblGrpPer_10")
        Me.LblGrpPer_10.ForeColor = System.Drawing.Color.Lime
        Me.LblGrpPer_10.Name = "LblGrpPer_10"
        '
        'LblGrpPer_09
        '
        Me.LblGrpPer_09.BackColor = System.Drawing.Color.Transparent
        Me.LblGrpPer_09.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblGrpPer_09, "LblGrpPer_09")
        Me.LblGrpPer_09.ForeColor = System.Drawing.Color.Lime
        Me.LblGrpPer_09.Name = "LblGrpPer_09"
        '
        'LblGrpPer_08
        '
        Me.LblGrpPer_08.BackColor = System.Drawing.Color.Transparent
        Me.LblGrpPer_08.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblGrpPer_08, "LblGrpPer_08")
        Me.LblGrpPer_08.ForeColor = System.Drawing.Color.Lime
        Me.LblGrpPer_08.Name = "LblGrpPer_08"
        '
        'LblGrpPer_07
        '
        Me.LblGrpPer_07.BackColor = System.Drawing.Color.Transparent
        Me.LblGrpPer_07.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblGrpPer_07, "LblGrpPer_07")
        Me.LblGrpPer_07.ForeColor = System.Drawing.Color.Lime
        Me.LblGrpPer_07.Name = "LblGrpPer_07"
        '
        'LblGrpPer_06
        '
        Me.LblGrpPer_06.BackColor = System.Drawing.Color.Transparent
        Me.LblGrpPer_06.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblGrpPer_06, "LblGrpPer_06")
        Me.LblGrpPer_06.ForeColor = System.Drawing.Color.Lime
        Me.LblGrpPer_06.Name = "LblGrpPer_06"
        '
        'LblGrpPer_05
        '
        Me.LblGrpPer_05.BackColor = System.Drawing.Color.Transparent
        Me.LblGrpPer_05.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblGrpPer_05, "LblGrpPer_05")
        Me.LblGrpPer_05.ForeColor = System.Drawing.Color.Lime
        Me.LblGrpPer_05.Name = "LblGrpPer_05"
        '
        'LblGrpPer_04
        '
        Me.LblGrpPer_04.BackColor = System.Drawing.Color.Transparent
        Me.LblGrpPer_04.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblGrpPer_04, "LblGrpPer_04")
        Me.LblGrpPer_04.ForeColor = System.Drawing.Color.Lime
        Me.LblGrpPer_04.Name = "LblGrpPer_04"
        '
        'LblGrpPer_03
        '
        Me.LblGrpPer_03.BackColor = System.Drawing.Color.Transparent
        Me.LblGrpPer_03.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblGrpPer_03, "LblGrpPer_03")
        Me.LblGrpPer_03.ForeColor = System.Drawing.Color.Lime
        Me.LblGrpPer_03.Name = "LblGrpPer_03"
        '
        'LblGrpPer_02
        '
        Me.LblGrpPer_02.BackColor = System.Drawing.Color.Transparent
        Me.LblGrpPer_02.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblGrpPer_02, "LblGrpPer_02")
        Me.LblGrpPer_02.ForeColor = System.Drawing.Color.Lime
        Me.LblGrpPer_02.Name = "LblGrpPer_02"
        '
        'LblGrpPer_01
        '
        Me.LblGrpPer_01.BackColor = System.Drawing.Color.Transparent
        Me.LblGrpPer_01.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblGrpPer_01, "LblGrpPer_01")
        Me.LblGrpPer_01.ForeColor = System.Drawing.Color.Lime
        Me.LblGrpPer_01.Name = "LblGrpPer_01"
        '
        'LblRegN_11
        '
        Me.LblRegN_11.BackColor = System.Drawing.Color.Transparent
        Me.LblRegN_11.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblRegN_11, "LblRegN_11")
        Me.LblRegN_11.ForeColor = System.Drawing.Color.Lime
        Me.LblRegN_11.Name = "LblRegN_11"
        '
        'LblRegN_10
        '
        Me.LblRegN_10.BackColor = System.Drawing.Color.Transparent
        Me.LblRegN_10.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblRegN_10, "LblRegN_10")
        Me.LblRegN_10.ForeColor = System.Drawing.Color.Lime
        Me.LblRegN_10.Name = "LblRegN_10"
        '
        'LblRegN_09
        '
        Me.LblRegN_09.BackColor = System.Drawing.Color.Transparent
        Me.LblRegN_09.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblRegN_09, "LblRegN_09")
        Me.LblRegN_09.ForeColor = System.Drawing.Color.Lime
        Me.LblRegN_09.Name = "LblRegN_09"
        '
        'LblRegN_08
        '
        Me.LblRegN_08.BackColor = System.Drawing.Color.Transparent
        Me.LblRegN_08.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblRegN_08, "LblRegN_08")
        Me.LblRegN_08.ForeColor = System.Drawing.Color.Lime
        Me.LblRegN_08.Name = "LblRegN_08"
        '
        'LblRegN_07
        '
        Me.LblRegN_07.BackColor = System.Drawing.Color.Transparent
        Me.LblRegN_07.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblRegN_07, "LblRegN_07")
        Me.LblRegN_07.ForeColor = System.Drawing.Color.Lime
        Me.LblRegN_07.Name = "LblRegN_07"
        '
        'LblRegN_06
        '
        Me.LblRegN_06.BackColor = System.Drawing.Color.Transparent
        Me.LblRegN_06.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblRegN_06, "LblRegN_06")
        Me.LblRegN_06.ForeColor = System.Drawing.Color.Lime
        Me.LblRegN_06.Name = "LblRegN_06"
        '
        'LblRegN_05
        '
        Me.LblRegN_05.BackColor = System.Drawing.Color.Transparent
        Me.LblRegN_05.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblRegN_05, "LblRegN_05")
        Me.LblRegN_05.ForeColor = System.Drawing.Color.Lime
        Me.LblRegN_05.Name = "LblRegN_05"
        '
        'LblRegN_04
        '
        Me.LblRegN_04.BackColor = System.Drawing.Color.Transparent
        Me.LblRegN_04.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblRegN_04, "LblRegN_04")
        Me.LblRegN_04.ForeColor = System.Drawing.Color.Lime
        Me.LblRegN_04.Name = "LblRegN_04"
        '
        'LblRegN_03
        '
        Me.LblRegN_03.BackColor = System.Drawing.Color.Transparent
        Me.LblRegN_03.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblRegN_03, "LblRegN_03")
        Me.LblRegN_03.ForeColor = System.Drawing.Color.Lime
        Me.LblRegN_03.Name = "LblRegN_03"
        '
        'LblRegN_02
        '
        Me.LblRegN_02.BackColor = System.Drawing.Color.Transparent
        Me.LblRegN_02.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblRegN_02, "LblRegN_02")
        Me.LblRegN_02.ForeColor = System.Drawing.Color.Lime
        Me.LblRegN_02.Name = "LblRegN_02"
        '
        'LblRegN_01
        '
        Me.LblRegN_01.BackColor = System.Drawing.Color.Transparent
        Me.LblRegN_01.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblRegN_01, "LblRegN_01")
        Me.LblRegN_01.ForeColor = System.Drawing.Color.Lime
        Me.LblRegN_01.Name = "LblRegN_01"
        '
        'Label14
        '
        Me.Label14.BackColor = System.Drawing.Color.Green
        resources.ApplyResources(Me.Label14, "Label14")
        Me.Label14.Name = "Label14"
        '
        'Label15
        '
        Me.Label15.BackColor = System.Drawing.Color.Green
        resources.ApplyResources(Me.Label15, "Label15")
        Me.Label15.Name = "Label15"
        '
        'Label16
        '
        Me.Label16.BackColor = System.Drawing.Color.Green
        resources.ApplyResources(Me.Label16, "Label16")
        Me.Label16.Name = "Label16"
        '
        'Label17
        '
        resources.ApplyResources(Me.Label17, "Label17")
        Me.Label17.BackColor = System.Drawing.Color.Transparent
        Me.Label17.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label17.ForeColor = System.Drawing.Color.Lime
        Me.Label17.Name = "Label17"
        '
        'Label18
        '
        resources.ApplyResources(Me.Label18, "Label18")
        Me.Label18.BackColor = System.Drawing.Color.Transparent
        Me.Label18.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label18.ForeColor = System.Drawing.Color.Lime
        Me.Label18.Name = "Label18"
        '
        'lblRegistUnit2
        '
        Me.lblRegistUnit2.BackColor = System.Drawing.Color.Transparent
        Me.lblRegistUnit2.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblRegistUnit2, "lblRegistUnit2")
        Me.lblRegistUnit2.ForeColor = System.Drawing.Color.Lime
        Me.lblRegistUnit2.Name = "lblRegistUnit2"
        '
        'lblGraphAccumulationTitle2
        '
        Me.lblGraphAccumulationTitle2.BackColor = System.Drawing.Color.Transparent
        Me.lblGraphAccumulationTitle2.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblGraphAccumulationTitle2, "lblGraphAccumulationTitle2")
        Me.lblGraphAccumulationTitle2.ForeColor = System.Drawing.Color.Lime
        Me.lblGraphAccumulationTitle2.Name = "lblGraphAccumulationTitle2"
        '
        'GrpQrCode
        '
        Me.GrpQrCode.BackColor = System.Drawing.SystemColors.Control
        Me.GrpQrCode.Controls.Add(Me.btnRest)
        Me.GrpQrCode.Controls.Add(Me.lblQRData)
        resources.ApplyResources(Me.GrpQrCode, "GrpQrCode")
        Me.GrpQrCode.Name = "GrpQrCode"
        Me.GrpQrCode.TabStop = False
        '
        'btnRest
        '
        resources.ApplyResources(Me.btnRest, "btnRest")
        Me.btnRest.Name = "btnRest"
        Me.btnRest.UseVisualStyleBackColor = True
        '
        'lblQRData
        '
        resources.ApplyResources(Me.lblQRData, "lblQRData")
        Me.lblQRData.BackColor = System.Drawing.SystemColors.Control
        Me.lblQRData.Name = "lblQRData"
        '
        'CmdSave
        '
        Me.CmdSave.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.CmdSave.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CmdSave, "CmdSave")
        Me.CmdSave.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdSave.Name = "CmdSave"
        Me.CmdSave.UseVisualStyleBackColor = False
        '
        'CmdEdit
        '
        Me.CmdEdit.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.CmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CmdEdit, "CmdEdit")
        Me.CmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdEdit.Name = "CmdEdit"
        Me.CmdEdit.UseVisualStyleBackColor = False
        '
        'CmdLoaderInit
        '
        Me.CmdLoaderInit.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.CmdLoaderInit.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CmdLoaderInit, "CmdLoaderInit")
        Me.CmdLoaderInit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdLoaderInit.Name = "CmdLoaderInit"
        Me.CmdLoaderInit.UseVisualStyleBackColor = False
        '
        'CmdAutoOperation
        '
        Me.CmdAutoOperation.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.CmdAutoOperation.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CmdAutoOperation, "CmdAutoOperation")
        Me.CmdAutoOperation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdAutoOperation.Name = "CmdAutoOperation"
        Me.CmdAutoOperation.UseVisualStyleBackColor = False
        '
        'tabCmd
        '
        Me.tabCmd.Controls.Add(Me.tabBaseCmnds)
        Me.tabCmd.Controls.Add(Me.tabOptCmnds)
        Me.tabCmd.Controls.Add(Me.tabOptCmnd2)
        Me.tabCmd.Controls.Add(Me.tabOptCmnd3)
        resources.ApplyResources(Me.tabCmd, "tabCmd")
        Me.tabCmd.Name = "tabCmd"
        Me.tabCmd.SelectedIndex = 0
        '
        'tabBaseCmnds
        '
        Me.tabBaseCmnds.Controls.Add(Me.CmdRecogRough)
        Me.tabBaseCmnds.Controls.Add(Me.CmdIntegrated)
        Me.tabBaseCmnds.Controls.Add(Me.CmdTy)
        Me.tabBaseCmnds.Controls.Add(Me.CmdCutPos)
        Me.tabBaseCmnds.Controls.Add(Me.CmdLoaderInit)
        Me.tabBaseCmnds.Controls.Add(Me.CmdTx)
        Me.tabBaseCmnds.Controls.Add(Me.CmdCircuitTeach)
        Me.tabBaseCmnds.Controls.Add(Me.CmdProbe)
        Me.tabBaseCmnds.Controls.Add(Me.CmdTeach)
        Me.tabBaseCmnds.Controls.Add(Me.CmdLoad)
        Me.tabBaseCmnds.Controls.Add(Me.CmdPattern)
        Me.tabBaseCmnds.Controls.Add(Me.CmdAutoOperation)
        Me.tabBaseCmnds.Controls.Add(Me.CmdSave)
        Me.tabBaseCmnds.Controls.Add(Me.CmdEdit)
        Me.tabBaseCmnds.Controls.Add(Me.CmdLaser)
        Me.tabBaseCmnds.Controls.Add(Me.CmdLoging)
        resources.ApplyResources(Me.tabBaseCmnds, "tabBaseCmnds")
        Me.tabBaseCmnds.Name = "tabBaseCmnds"
        Me.tabBaseCmnds.UseVisualStyleBackColor = True
        '
        'CmdRecogRough
        '
        Me.CmdRecogRough.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.CmdRecogRough.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CmdRecogRough, "CmdRecogRough")
        Me.CmdRecogRough.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdRecogRough.Name = "CmdRecogRough"
        Me.CmdRecogRough.UseVisualStyleBackColor = False
        '
        'CmdIntegrated
        '
        Me.CmdIntegrated.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.CmdIntegrated.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CmdIntegrated, "CmdIntegrated")
        Me.CmdIntegrated.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdIntegrated.Name = "CmdIntegrated"
        Me.CmdIntegrated.UseVisualStyleBackColor = False
        '
        'tabOptCmnds
        '
        Me.tabOptCmnds.Controls.Add(Me.lblProductionData)
        Me.tabOptCmnds.Controls.Add(Me.CmdFolderOpen)
        Me.tabOptCmnds.Controls.Add(Me.lblExCamera)
        Me.tabOptCmnds.Controls.Add(Me.lblCalibration)
        Me.tabOptCmnds.Controls.Add(Me.lblCutPos)
        Me.tabOptCmnds.Controls.Add(Me.CmdT_Theta)
        Me.tabOptCmnds.Controls.Add(Me.CmdExCam)
        Me.tabOptCmnds.Controls.Add(Me.CmdTy2)
        Me.tabOptCmnds.Controls.Add(Me.CmdExCam1)
        Me.tabOptCmnds.Controls.Add(Me.CmdCutPosCorrect)
        Me.tabOptCmnds.Controls.Add(Me.CmdPtnCutPosCorrect)
        Me.tabOptCmnds.Controls.Add(Me.CmdCalibration)
        Me.tabOptCmnds.Controls.Add(Me.CmdPtnCalibration)
        resources.ApplyResources(Me.tabOptCmnds, "tabOptCmnds")
        Me.tabOptCmnds.Name = "tabOptCmnds"
        Me.tabOptCmnds.UseVisualStyleBackColor = True
        '
        'lblProductionData
        '
        resources.ApplyResources(Me.lblProductionData, "lblProductionData")
        Me.lblProductionData.Name = "lblProductionData"
        '
        'CmdFolderOpen
        '
        Me.CmdFolderOpen.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.CmdFolderOpen.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CmdFolderOpen, "CmdFolderOpen")
        Me.CmdFolderOpen.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdFolderOpen.Name = "CmdFolderOpen"
        Me.CmdFolderOpen.UseVisualStyleBackColor = False
        '
        'lblExCamera
        '
        resources.ApplyResources(Me.lblExCamera, "lblExCamera")
        Me.lblExCamera.Name = "lblExCamera"
        '
        'lblCalibration
        '
        resources.ApplyResources(Me.lblCalibration, "lblCalibration")
        Me.lblCalibration.Name = "lblCalibration"
        '
        'lblCutPos
        '
        resources.ApplyResources(Me.lblCutPos, "lblCutPos")
        Me.lblCutPos.Name = "lblCutPos"
        '
        'tabOptCmnd2
        '
        Me.tabOptCmnd2.Controls.Add(Me.CmdMap)
        Me.tabOptCmnd2.Controls.Add(Me.CmdSinsyukuPtn)
        Me.tabOptCmnd2.Controls.Add(Me.CmdAotoProbeCorrect)
        Me.tabOptCmnd2.Controls.Add(Me.CmdAotoProbePtn)
        Me.tabOptCmnd2.Controls.Add(Me.CmdIDTeach)
        Me.tabOptCmnd2.Controls.Add(Me.lblSinsyuku)
        Me.tabOptCmnd2.Controls.Add(Me.lblAutoProbe)
        resources.ApplyResources(Me.tabOptCmnd2, "tabOptCmnd2")
        Me.tabOptCmnd2.Name = "tabOptCmnd2"
        Me.tabOptCmnd2.UseVisualStyleBackColor = True
        '
        'CmdMap
        '
        Me.CmdMap.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.CmdMap.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CmdMap, "CmdMap")
        Me.CmdMap.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdMap.Name = "CmdMap"
        Me.CmdMap.UseVisualStyleBackColor = False
        '
        'CmdSinsyukuPtn
        '
        Me.CmdSinsyukuPtn.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.CmdSinsyukuPtn.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CmdSinsyukuPtn, "CmdSinsyukuPtn")
        Me.CmdSinsyukuPtn.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdSinsyukuPtn.Name = "CmdSinsyukuPtn"
        Me.CmdSinsyukuPtn.UseVisualStyleBackColor = False
        '
        'CmdAotoProbeCorrect
        '
        Me.CmdAotoProbeCorrect.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.CmdAotoProbeCorrect.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CmdAotoProbeCorrect, "CmdAotoProbeCorrect")
        Me.CmdAotoProbeCorrect.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdAotoProbeCorrect.Name = "CmdAotoProbeCorrect"
        Me.CmdAotoProbeCorrect.UseVisualStyleBackColor = False
        '
        'CmdAotoProbePtn
        '
        Me.CmdAotoProbePtn.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.CmdAotoProbePtn.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CmdAotoProbePtn, "CmdAotoProbePtn")
        Me.CmdAotoProbePtn.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdAotoProbePtn.Name = "CmdAotoProbePtn"
        Me.CmdAotoProbePtn.UseVisualStyleBackColor = False
        '
        'CmdIDTeach
        '
        Me.CmdIDTeach.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.CmdIDTeach.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CmdIDTeach, "CmdIDTeach")
        Me.CmdIDTeach.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdIDTeach.Name = "CmdIDTeach"
        Me.CmdIDTeach.UseVisualStyleBackColor = False
        '
        'lblSinsyuku
        '
        resources.ApplyResources(Me.lblSinsyuku, "lblSinsyuku")
        Me.lblSinsyuku.Name = "lblSinsyuku"
        '
        'lblAutoProbe
        '
        resources.ApplyResources(Me.lblAutoProbe, "lblAutoProbe")
        Me.lblAutoProbe.Name = "lblAutoProbe"
        '
        'tabOptCmnd3
        '
        Me.tabOptCmnd3.Controls.Add(Me.CmdT_ProbeCleaning)
        resources.ApplyResources(Me.tabOptCmnd3, "tabOptCmnd3")
        Me.tabOptCmnd3.Name = "tabOptCmnd3"
        Me.tabOptCmnd3.UseVisualStyleBackColor = True
        '
        'CmdT_ProbeCleaning
        '
        Me.CmdT_ProbeCleaning.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.CmdT_ProbeCleaning.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CmdT_ProbeCleaning, "CmdT_ProbeCleaning")
        Me.CmdT_ProbeCleaning.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdT_ProbeCleaning.Name = "CmdT_ProbeCleaning"
        Me.CmdT_ProbeCleaning.UseVisualStyleBackColor = False
        '
        'grpDbg
        '
        Me.grpDbg.Controls.Add(Me.txtCount)
        Me.grpDbg.Controls.Add(Me.Label1)
        Me.grpDbg.Controls.Add(Me.cmdStop)
        Me.grpDbg.Controls.Add(Me.cmdTestRun)
        Me.grpDbg.Controls.Add(Me.btnDbgForm)
        resources.ApplyResources(Me.grpDbg, "grpDbg")
        Me.grpDbg.Name = "grpDbg"
        Me.grpDbg.TabStop = False
        '
        'txtCount
        '
        resources.ApplyResources(Me.txtCount, "txtCount")
        Me.txtCount.Name = "txtCount"
        Me.txtCount.ReadOnly = True
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Name = "Label1"
        '
        'cmdStop
        '
        Me.cmdStop.BackColor = System.Drawing.Color.Yellow
        resources.ApplyResources(Me.cmdStop, "cmdStop")
        Me.cmdStop.Name = "cmdStop"
        Me.cmdStop.UseVisualStyleBackColor = False
        '
        'cmdTestRun
        '
        Me.cmdTestRun.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me.cmdTestRun, "cmdTestRun")
        Me.cmdTestRun.Name = "cmdTestRun"
        Me.cmdTestRun.UseVisualStyleBackColor = False
        '
        'btnDbgForm
        '
        resources.ApplyResources(Me.btnDbgForm, "btnDbgForm")
        Me.btnDbgForm.Name = "btnDbgForm"
        Me.btnDbgForm.UseVisualStyleBackColor = True
        '
        'GrpMode
        '
        Me.GrpMode.Controls.Add(Me.btnCycleStop)
        Me.GrpMode.Controls.Add(Me.BtnSubstrateSet)
        Me.GrpMode.Controls.Add(Me.BtnADJ)
        Me.GrpMode.Controls.Add(Me.LblDIGSW_HI)
        Me.GrpMode.Controls.Add(Me.LblDIGSW)
        Me.GrpMode.Controls.Add(Me.CbDigSwL)
        Me.GrpMode.Controls.Add(Me.CbDigSwH)
        Me.GrpMode.Controls.Add(Me.btnTrimming)
        resources.ApplyResources(Me.GrpMode, "GrpMode")
        Me.GrpMode.Name = "GrpMode"
        Me.GrpMode.TabStop = False
        '
        'btnCycleStop
        '
        Me.btnCycleStop.AccessibleRole = System.Windows.Forms.AccessibleRole.None
        resources.ApplyResources(Me.btnCycleStop, "btnCycleStop")
        Me.btnCycleStop.Name = "btnCycleStop"
        Me.btnCycleStop.UseVisualStyleBackColor = True
        '
        'BtnSubstrateSet
        '
        Me.BtnSubstrateSet.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me.BtnSubstrateSet, "BtnSubstrateSet")
        Me.BtnSubstrateSet.Name = "BtnSubstrateSet"
        Me.BtnSubstrateSet.UseVisualStyleBackColor = False
        '
        'BtnADJ
        '
        Me.BtnADJ.AccessibleRole = System.Windows.Forms.AccessibleRole.None
        resources.ApplyResources(Me.BtnADJ, "BtnADJ")
        Me.BtnADJ.Name = "BtnADJ"
        Me.BtnADJ.UseVisualStyleBackColor = True
        '
        'LblDIGSW_HI
        '
        Me.LblDIGSW_HI.BackColor = System.Drawing.SystemColors.Control
        Me.LblDIGSW_HI.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblDIGSW_HI, "LblDIGSW_HI")
        Me.LblDIGSW_HI.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblDIGSW_HI.Name = "LblDIGSW_HI"
        '
        'LblDIGSW
        '
        Me.LblDIGSW.BackColor = System.Drawing.SystemColors.Control
        Me.LblDIGSW.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblDIGSW, "LblDIGSW")
        Me.LblDIGSW.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblDIGSW.Name = "LblDIGSW"
        '
        'CbDigSwL
        '
        Me.CbDigSwL.BackColor = System.Drawing.Color.PeachPuff
        Me.CbDigSwL.DisplayMember = "0"
        Me.CbDigSwL.DropDownHeight = 200
        Me.CbDigSwL.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.CbDigSwL, "CbDigSwL")
        Me.CbDigSwL.FormattingEnabled = True
        Me.CbDigSwL.Items.AddRange(New Object() {resources.GetString("CbDigSwL.Items"), resources.GetString("CbDigSwL.Items1"), resources.GetString("CbDigSwL.Items2"), resources.GetString("CbDigSwL.Items3"), resources.GetString("CbDigSwL.Items4"), resources.GetString("CbDigSwL.Items5"), resources.GetString("CbDigSwL.Items6")})
        Me.CbDigSwL.Name = "CbDigSwL"
        '
        'CbDigSwH
        '
        Me.CbDigSwH.BackColor = System.Drawing.Color.PeachPuff
        Me.CbDigSwH.DropDownHeight = 80
        Me.CbDigSwH.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.CbDigSwH, "CbDigSwH")
        Me.CbDigSwH.FormattingEnabled = True
        Me.CbDigSwH.Items.AddRange(New Object() {resources.GetString("CbDigSwH.Items"), resources.GetString("CbDigSwH.Items1"), resources.GetString("CbDigSwH.Items2")})
        Me.CbDigSwH.Name = "CbDigSwH"
        '
        'txtLog
        '
        Me.txtLog.AcceptsReturn = True
        Me.txtLog.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtLog.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtLog, "txtLog")
        Me.txtLog.ForeColor = System.Drawing.Color.Black
        Me.txtLog.Name = "txtLog"
        Me.txtLog.ReadOnly = True
        '
        'btnGoClipboard
        '
        Me.btnGoClipboard.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnGoClipboard.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.btnGoClipboard, "btnGoClipboard")
        Me.btnGoClipboard.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnGoClipboard.Name = "btnGoClipboard"
        Me.btnGoClipboard.UseVisualStyleBackColor = False
        '
        'chkDistributeOnOff
        '
        resources.ApplyResources(Me.chkDistributeOnOff, "chkDistributeOnOff")
        Me.chkDistributeOnOff.Name = "chkDistributeOnOff"
        Me.chkDistributeOnOff.UseVisualStyleBackColor = True
        '
        'btnDbg
        '
        resources.ApplyResources(Me.btnDbg, "btnDbg")
        Me.btnDbg.Name = "btnDbg"
        Me.btnDbg.UseVisualStyleBackColor = True
        '
        'lblTwoCount
        '
        resources.ApplyResources(Me.lblTwoCount, "lblTwoCount")
        Me.lblTwoCount.Name = "lblTwoCount"
        '
        'lblBreakCount
        '
        resources.ApplyResources(Me.lblBreakCount, "lblBreakCount")
        Me.lblBreakCount.Name = "lblBreakCount"
        '
        'GrpNgBox
        '
        Me.GrpNgBox.Controls.Add(Me.lblBreakCount)
        Me.GrpNgBox.Controls.Add(Me.lblNgCount)
        Me.GrpNgBox.Controls.Add(Me.lblTwoCount)
        resources.ApplyResources(Me.GrpNgBox, "GrpNgBox")
        Me.GrpNgBox.Name = "GrpNgBox"
        Me.GrpNgBox.TabStop = False
        '
        'lblNgCount
        '
        resources.ApplyResources(Me.lblNgCount, "lblNgCount")
        Me.lblNgCount.Name = "lblNgCount"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Button1)
        Me.GroupBox1.Controls.Add(Me.Button2)
        Me.GroupBox1.Controls.Add(Me.CheckBox4)
        Me.GroupBox1.Controls.Add(Me.CheckBox3)
        Me.GroupBox1.Controls.Add(Me.CheckBox2)
        Me.GroupBox1.Controls.Add(Me.CheckBox1)
        resources.ApplyResources(Me.GroupBox1, "GroupBox1")
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.TabStop = False
        '
        'Button1
        '
        resources.ApplyResources(Me.Button1, "Button1")
        Me.Button1.Name = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        resources.ApplyResources(Me.Button2, "Button2")
        Me.Button2.Name = "Button2"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'CheckBox4
        '
        resources.ApplyResources(Me.CheckBox4, "CheckBox4")
        Me.CheckBox4.Name = "CheckBox4"
        Me.CheckBox4.UseVisualStyleBackColor = True
        '
        'CheckBox3
        '
        resources.ApplyResources(Me.CheckBox3, "CheckBox3")
        Me.CheckBox3.Name = "CheckBox3"
        Me.CheckBox3.UseVisualStyleBackColor = True
        '
        'CheckBox2
        '
        resources.ApplyResources(Me.CheckBox2, "CheckBox2")
        Me.CheckBox2.Name = "CheckBox2"
        Me.CheckBox2.UseVisualStyleBackColor = True
        '
        'CheckBox1
        '
        resources.ApplyResources(Me.CheckBox1, "CheckBox1")
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'TxtBoxPrint
        '
        resources.ApplyResources(Me.TxtBoxPrint, "TxtBoxPrint")
        Me.TxtBoxPrint.Name = "TxtBoxPrint"
        '
        'BtnPrint
        '
        Me.BtnPrint.BackColor = System.Drawing.SystemColors.Control
        Me.BtnPrint.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.BtnPrint, "BtnPrint")
        Me.BtnPrint.ForeColor = System.Drawing.SystemColors.ControlText
        Me.BtnPrint.Name = "BtnPrint"
        Me.BtnPrint.UseVisualStyleBackColor = False
        '
        'BtnPrintOnOff
        '
        Me.BtnPrintOnOff.BackColor = System.Drawing.SystemColors.Control
        Me.BtnPrintOnOff.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.BtnPrintOnOff, "BtnPrintOnOff")
        Me.BtnPrintOnOff.ForeColor = System.Drawing.SystemColors.ControlText
        Me.BtnPrintOnOff.Name = "BtnPrintOnOff"
        Me.BtnPrintOnOff.UseVisualStyleBackColor = False
        '
        'GrpStrageBox
        '
        Me.GrpStrageBox.Controls.Add(Me.BtnStrageClr)
        Me.GrpStrageBox.Controls.Add(Me.LblStrageBoxCount)
        Me.GrpStrageBox.Controls.Add(Me.LblStrageBoxTtl)
        resources.ApplyResources(Me.GrpStrageBox, "GrpStrageBox")
        Me.GrpStrageBox.Name = "GrpStrageBox"
        Me.GrpStrageBox.TabStop = False
        '
        'BtnStrageClr
        '
        Me.BtnStrageClr.BackColor = System.Drawing.Color.White
        resources.ApplyResources(Me.BtnStrageClr, "BtnStrageClr")
        Me.BtnStrageClr.Name = "BtnStrageClr"
        Me.BtnStrageClr.UseVisualStyleBackColor = False
        '
        'LblStrageBoxCount
        '
        resources.ApplyResources(Me.LblStrageBoxCount, "LblStrageBoxCount")
        Me.LblStrageBoxCount.Name = "LblStrageBoxCount"
        '
        'LblStrageBoxTtl
        '
        resources.ApplyResources(Me.LblStrageBoxTtl, "LblStrageBoxTtl")
        Me.LblStrageBoxTtl.Name = "LblStrageBoxTtl"
        '
        'pnlDataDisplay
        '
        resources.ApplyResources(Me.pnlDataDisplay, "pnlDataDisplay")
        Me.pnlDataDisplay.Name = "pnlDataDisplay"
        '
        'BtnPowerOnOff
        '
        Me.BtnPowerOnOff.BackColor = System.Drawing.Color.Lime
        Me.BtnPowerOnOff.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.BtnPowerOnOff, "BtnPowerOnOff")
        Me.BtnPowerOnOff.ForeColor = System.Drawing.SystemColors.ControlText
        Me.BtnPowerOnOff.Name = "BtnPowerOnOff"
        Me.BtnPowerOnOff.UseVisualStyleBackColor = False
        '
        'LblComandName
        '
        resources.ApplyResources(Me.LblComandName, "LblComandName")
        Me.LblComandName.Name = "LblComandName"
        '
        'TimerAlarm
        '
        Me.TimerAlarm.Interval = 50
        '
        'Timer1
        '
        '
        'TimerInterLockSts
        '
        Me.TimerInterLockSts.Interval = 300
        '
        'TimerQR
        '
        Me.TimerQR.Interval = 1000
        '
        'TimerAdjust
        '
        Me.TimerAdjust.Interval = 300
        '
        'TimerBC
        '
        '
        'btnUserLogon
        '
        Me.btnUserLogon.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(137, Byte), Integer), CType(CType(140, Byte), Integer), CType(CType(149, Byte), Integer))
        resources.ApplyResources(Me.btnUserLogon, "btnUserLogon")
        Me.btnUserLogon.Name = "btnUserLogon"
        Me.btnUserLogon.UseVisualStyleBackColor = False
        '
        'grpIntegrated
        '
        Me.grpIntegrated.Controls.Add(Me.flpIntegrated)
        resources.ApplyResources(Me.grpIntegrated, "grpIntegrated")
        Me.grpIntegrated.Name = "grpIntegrated"
        Me.grpIntegrated.TabStop = False
        '
        'flpIntegrated
        '
        Me.flpIntegrated.Controls.Add(Me.lblIntegRecog)
        Me.flpIntegrated.Controls.Add(Me.lblIntegProbe)
        Me.flpIntegrated.Controls.Add(Me.lblIntegTX)
        Me.flpIntegrated.Controls.Add(Me.lblIntegTeach)
        Me.flpIntegrated.Controls.Add(Me.lblIntegTY)
        resources.ApplyResources(Me.flpIntegrated, "flpIntegrated")
        Me.flpIntegrated.Name = "flpIntegrated"
        '
        'lblIntegRecog
        '
        Me.lblIntegRecog.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(225, Byte), Integer))
        Me.lblIntegRecog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        resources.ApplyResources(Me.lblIntegRecog, "lblIntegRecog")
        Me.lblIntegRecog.Name = "lblIntegRecog"
        '
        'lblIntegProbe
        '
        Me.lblIntegProbe.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        resources.ApplyResources(Me.lblIntegProbe, "lblIntegProbe")
        Me.lblIntegProbe.Name = "lblIntegProbe"
        '
        'lblIntegTX
        '
        Me.lblIntegTX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        resources.ApplyResources(Me.lblIntegTX, "lblIntegTX")
        Me.lblIntegTX.Name = "lblIntegTX"
        '
        'lblIntegTeach
        '
        Me.lblIntegTeach.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        resources.ApplyResources(Me.lblIntegTeach, "lblIntegTeach")
        Me.lblIntegTeach.Name = "lblIntegTeach"
        '
        'lblIntegTY
        '
        Me.lblIntegTY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        resources.ApplyResources(Me.lblIntegTY, "lblIntegTY")
        Me.lblIntegTY.Name = "lblIntegTY"
        '
        'btnJudge
        '
        resources.ApplyResources(Me.btnJudge, "btnJudge")
        Me.btnJudge.Name = "btnJudge"
        Me.btnJudge.UseVisualStyleBackColor = True
        '
        'lblDoorOpen
        '
        Me.lblDoorOpen.BackColor = System.Drawing.Color.Lime
        Me.lblDoorOpen.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        resources.ApplyResources(Me.lblDoorOpen, "lblDoorOpen")
        Me.lblDoorOpen.Name = "lblDoorOpen"
        '
        'GrpStartBlk
        '
        resources.ApplyResources(Me.GrpStartBlk, "GrpStartBlk")
        Me.GrpStartBlk.Controls.Add(Me.tlpStartBlk)
        Me.GrpStartBlk.Name = "GrpStartBlk"
        Me.GrpStartBlk.TabStop = False
        '
        'tlpStartBlk
        '
        resources.ApplyResources(Me.tlpStartBlk, "tlpStartBlk")
        Me.tlpStartBlk.Controls.Add(Me.CbStartBlkY, 4, 0)
        Me.tlpStartBlk.Controls.Add(Me.lblStartBlkY, 3, 0)
        Me.tlpStartBlk.Controls.Add(Me.CbStartBlkX, 2, 0)
        Me.tlpStartBlk.Controls.Add(Me.lblStartBlkX, 1, 0)
        Me.tlpStartBlk.Controls.Add(Me.chkContinue, 0, 0)
        Me.tlpStartBlk.Controls.Add(Me.btnPREV, 5, 0)
        Me.tlpStartBlk.Controls.Add(Me.btnNEXT, 6, 0)
        Me.tlpStartBlk.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize
        Me.tlpStartBlk.Name = "tlpStartBlk"
        '
        'CbStartBlkY
        '
        Me.CbStartBlkY.BackColor = System.Drawing.Color.Cyan
        resources.ApplyResources(Me.CbStartBlkY, "CbStartBlkY")
        Me.CbStartBlkY.FormattingEnabled = True
        Me.CbStartBlkY.Name = "CbStartBlkY"
        Me.CbStartBlkY.TabStop = False
        '
        'lblStartBlkY
        '
        resources.ApplyResources(Me.lblStartBlkY, "lblStartBlkY")
        Me.lblStartBlkY.Name = "lblStartBlkY"
        '
        'CbStartBlkX
        '
        Me.CbStartBlkX.BackColor = System.Drawing.Color.Cyan
        resources.ApplyResources(Me.CbStartBlkX, "CbStartBlkX")
        Me.CbStartBlkX.FormattingEnabled = True
        Me.CbStartBlkX.Name = "CbStartBlkX"
        Me.CbStartBlkX.TabStop = False
        '
        'lblStartBlkX
        '
        resources.ApplyResources(Me.lblStartBlkX, "lblStartBlkX")
        Me.lblStartBlkX.Name = "lblStartBlkX"
        '
        'chkContinue
        '
        resources.ApplyResources(Me.chkContinue, "chkContinue")
        Me.chkContinue.Name = "chkContinue"
        Me.chkContinue.TabStop = False
        Me.chkContinue.UseMnemonic = False
        Me.chkContinue.UseVisualStyleBackColor = False
        '
        'btnPREV
        '
        resources.ApplyResources(Me.btnPREV, "btnPREV")
        Me.btnPREV.Name = "btnPREV"
        Me.btnPREV.TabStop = False
        Me.btnPREV.UseVisualStyleBackColor = True
        '
        'btnNEXT
        '
        resources.ApplyResources(Me.btnNEXT, "btnNEXT")
        Me.btnNEXT.Name = "btnNEXT"
        Me.btnNEXT.TabStop = False
        Me.btnNEXT.UseVisualStyleBackColor = True
        '
        'CmdMapOnOff
        '
        Me.CmdMapOnOff.BackColor = System.Drawing.SystemColors.Control
        Me.CmdMapOnOff.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CmdMapOnOff, "CmdMapOnOff")
        Me.CmdMapOnOff.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdMapOnOff.Name = "CmdMapOnOff"
        Me.CmdMapOnOff.UseVisualStyleBackColor = False
        '
        'PanelMap
        '
        resources.ApplyResources(Me.PanelMap, "PanelMap")
        Me.PanelMap.Controls.Add(Me.CmdPrintMap)
        Me.PanelMap.Controls.Add(Me.CmdMapOnOff)
        Me.PanelMap.Name = "PanelMap"
        '
        'CmdPrintMap
        '
        Me.CmdPrintMap.BackColor = System.Drawing.SystemColors.Control
        Me.CmdPrintMap.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CmdPrintMap, "CmdPrintMap")
        Me.CmdPrintMap.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdPrintMap.Name = "CmdPrintMap"
        Me.CmdPrintMap.UseVisualStyleBackColor = False
        '
        'btnQRLmit
        '
        Me.btnQRLmit.BackColor = System.Drawing.SystemColors.Control
        Me.btnQRLmit.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.btnQRLmit, "btnQRLmit")
        Me.btnQRLmit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnQRLmit.Name = "btnQRLmit"
        Me.btnQRLmit.UseVisualStyleBackColor = False
        '
        'lblCutOff
        '
        resources.ApplyResources(Me.lblCutOff, "lblCutOff")
        Me.lblCutOff.BackColor = System.Drawing.Color.Lime
        Me.lblCutOff.Name = "lblCutOff"
        '
        'BtnAlarmOnOff
        '
        Me.BtnAlarmOnOff.BackColor = System.Drawing.Color.Lime
        resources.ApplyResources(Me.BtnAlarmOnOff, "BtnAlarmOnOff")
        Me.BtnAlarmOnOff.Name = "BtnAlarmOnOff"
        Me.BtnAlarmOnOff.UseVisualStyleBackColor = False
        '
        'pnlFirstResData
        '
        Me.pnlFirstResData.Controls.Add(Me.CutOffEsEditButton)
        Me.pnlFirstResData.Controls.Add(Me.lblFrdNomVal)
        Me.pnlFirstResData.Controls.Add(Me.lblFrdNom)
        Me.pnlFirstResData.Controls.Add(Me.tlpFirstResData)
        resources.ApplyResources(Me.pnlFirstResData, "pnlFirstResData")
        Me.pnlFirstResData.Name = "pnlFirstResData"
        '
        'CutOffEsEditButton
        '
        Me.CutOffEsEditButton.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.CutOffEsEditButton.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CutOffEsEditButton, "CutOffEsEditButton")
        Me.CutOffEsEditButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CutOffEsEditButton.Name = "CutOffEsEditButton"
        Me.CutOffEsEditButton.UseVisualStyleBackColor = False
        '
        'lblFrdNomVal
        '
        resources.ApplyResources(Me.lblFrdNomVal, "lblFrdNomVal")
        Me.lblFrdNomVal.Name = "lblFrdNomVal"
        '
        'lblFrdNom
        '
        resources.ApplyResources(Me.lblFrdNom, "lblFrdNom")
        Me.lblFrdNom.Name = "lblFrdNom"
        '
        'tlpFirstResData
        '
        resources.ApplyResources(Me.tlpFirstResData, "tlpFirstResData")
        Me.tlpFirstResData.Controls.Add(Me.lblFrdCutOff, 1, 0)
        Me.tlpFirstResData.Controls.Add(Me.lblFrdESPoint, 2, 0)
        Me.tlpFirstResData.Controls.Add(Me.lblFrd_1, 0, 1)
        Me.tlpFirstResData.Controls.Add(Me.lblFrdC1, 1, 1)
        Me.tlpFirstResData.Controls.Add(Me.lblFrdE1, 2, 1)
        Me.tlpFirstResData.Controls.Add(Me.lblFrd_2, 0, 2)
        Me.tlpFirstResData.Controls.Add(Me.lblFrdC2, 1, 2)
        Me.tlpFirstResData.Controls.Add(Me.lblFrdE2, 2, 2)
        Me.tlpFirstResData.Controls.Add(Me.lblFrd_3, 0, 3)
        Me.tlpFirstResData.Controls.Add(Me.lblFrdC3, 1, 3)
        Me.tlpFirstResData.Controls.Add(Me.lblFrdE3, 2, 3)
        Me.tlpFirstResData.Controls.Add(Me.lblFrd_4, 0, 4)
        Me.tlpFirstResData.Controls.Add(Me.lblFrdC4, 1, 4)
        Me.tlpFirstResData.Controls.Add(Me.lblFrdE4, 2, 4)
        Me.tlpFirstResData.Controls.Add(Me.lblFrd_5, 0, 5)
        Me.tlpFirstResData.Controls.Add(Me.lblFrdC5, 1, 5)
        Me.tlpFirstResData.Controls.Add(Me.lblFrdE5, 2, 5)
        Me.tlpFirstResData.Controls.Add(Me.lblFrd_6, 0, 6)
        Me.tlpFirstResData.Controls.Add(Me.lblFrdC6, 1, 6)
        Me.tlpFirstResData.Controls.Add(Me.lblFrdE6, 2, 6)
        Me.tlpFirstResData.Name = "tlpFirstResData"
        '
        'lblFrdCutOff
        '
        resources.ApplyResources(Me.lblFrdCutOff, "lblFrdCutOff")
        Me.lblFrdCutOff.Name = "lblFrdCutOff"
        '
        'lblFrdESPoint
        '
        resources.ApplyResources(Me.lblFrdESPoint, "lblFrdESPoint")
        Me.lblFrdESPoint.Name = "lblFrdESPoint"
        '
        'lblFrd_1
        '
        resources.ApplyResources(Me.lblFrd_1, "lblFrd_1")
        Me.lblFrd_1.Name = "lblFrd_1"
        '
        'lblFrdC1
        '
        resources.ApplyResources(Me.lblFrdC1, "lblFrdC1")
        Me.lblFrdC1.Name = "lblFrdC1"
        '
        'lblFrdE1
        '
        resources.ApplyResources(Me.lblFrdE1, "lblFrdE1")
        Me.lblFrdE1.Name = "lblFrdE1"
        '
        'lblFrd_2
        '
        resources.ApplyResources(Me.lblFrd_2, "lblFrd_2")
        Me.lblFrd_2.Name = "lblFrd_2"
        '
        'lblFrdC2
        '
        resources.ApplyResources(Me.lblFrdC2, "lblFrdC2")
        Me.lblFrdC2.Name = "lblFrdC2"
        '
        'lblFrdE2
        '
        resources.ApplyResources(Me.lblFrdE2, "lblFrdE2")
        Me.lblFrdE2.Name = "lblFrdE2"
        '
        'lblFrd_3
        '
        resources.ApplyResources(Me.lblFrd_3, "lblFrd_3")
        Me.lblFrd_3.Name = "lblFrd_3"
        '
        'lblFrdC3
        '
        resources.ApplyResources(Me.lblFrdC3, "lblFrdC3")
        Me.lblFrdC3.Name = "lblFrdC3"
        '
        'lblFrdE3
        '
        resources.ApplyResources(Me.lblFrdE3, "lblFrdE3")
        Me.lblFrdE3.Name = "lblFrdE3"
        '
        'lblFrd_4
        '
        resources.ApplyResources(Me.lblFrd_4, "lblFrd_4")
        Me.lblFrd_4.Name = "lblFrd_4"
        '
        'lblFrdC4
        '
        resources.ApplyResources(Me.lblFrdC4, "lblFrdC4")
        Me.lblFrdC4.Name = "lblFrdC4"
        '
        'lblFrdE4
        '
        resources.ApplyResources(Me.lblFrdE4, "lblFrdE4")
        Me.lblFrdE4.Name = "lblFrdE4"
        '
        'lblFrd_5
        '
        resources.ApplyResources(Me.lblFrd_5, "lblFrd_5")
        Me.lblFrd_5.Name = "lblFrd_5"
        '
        'lblFrdC5
        '
        resources.ApplyResources(Me.lblFrdC5, "lblFrdC5")
        Me.lblFrdC5.Name = "lblFrdC5"
        '
        'lblFrdE5
        '
        resources.ApplyResources(Me.lblFrdE5, "lblFrdE5")
        Me.lblFrdE5.Name = "lblFrdE5"
        '
        'lblFrd_6
        '
        resources.ApplyResources(Me.lblFrd_6, "lblFrd_6")
        Me.lblFrd_6.Name = "lblFrd_6"
        '
        'lblFrdC6
        '
        resources.ApplyResources(Me.lblFrdC6, "lblFrdC6")
        Me.lblFrdC6.Name = "lblFrdC6"
        '
        'lblFrdE6
        '
        resources.ApplyResources(Me.lblFrdE6, "lblFrdE6")
        Me.lblFrdE6.Name = "lblFrdE6"
        '
        'LabelAutoCalibLimit
        '
        Me.LabelAutoCalibLimit.BackColor = System.Drawing.Color.Yellow
        Me.LabelAutoCalibLimit.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LabelAutoCalibLimit.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LabelAutoCalibLimit, "LabelAutoCalibLimit")
        Me.LabelAutoCalibLimit.ForeColor = System.Drawing.Color.Red
        Me.LabelAutoCalibLimit.Name = "LabelAutoCalibLimit"
        '
        'pnlFirstResDataNET
        '
        Me.pnlFirstResDataNET.Controls.Add(Me.CutOffEsEditButtonNET)
        Me.pnlFirstResDataNET.Controls.Add(Me.lblNETNomVal)
        Me.pnlFirstResDataNET.Controls.Add(Me.lblNETNom)
        Me.pnlFirstResDataNET.Controls.Add(Me.tlpFirstResDataNET)
        resources.ApplyResources(Me.pnlFirstResDataNET, "pnlFirstResDataNET")
        Me.pnlFirstResDataNET.Name = "pnlFirstResDataNET"
        '
        'CutOffEsEditButtonNET
        '
        Me.CutOffEsEditButtonNET.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.CutOffEsEditButtonNET.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CutOffEsEditButtonNET, "CutOffEsEditButtonNET")
        Me.CutOffEsEditButtonNET.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CutOffEsEditButtonNET.Name = "CutOffEsEditButtonNET"
        Me.CutOffEsEditButtonNET.UseVisualStyleBackColor = False
        '
        'lblNETNomVal
        '
        resources.ApplyResources(Me.lblNETNomVal, "lblNETNomVal")
        Me.lblNETNomVal.Name = "lblNETNomVal"
        '
        'lblNETNom
        '
        resources.ApplyResources(Me.lblNETNom, "lblNETNom")
        Me.lblNETNom.Name = "lblNETNom"
        '
        'tlpFirstResDataNET
        '
        resources.ApplyResources(Me.tlpFirstResDataNET, "tlpFirstResDataNET")
        Me.tlpFirstResDataNET.Controls.Add(Me.Label8, 0, 0)
        Me.tlpFirstResDataNET.Controls.Add(Me.Label9, 0, 1)
        Me.tlpFirstResDataNET.Controls.Add(Me.Label10, 0, 2)
        Me.tlpFirstResDataNET.Controls.Add(Me.Label11, 0, 3)
        Me.tlpFirstResDataNET.Controls.Add(Me.Label12, 0, 4)
        Me.tlpFirstResDataNET.Controls.Add(Me.Label13, 0, 5)
        Me.tlpFirstResDataNET.Controls.Add(Me.Label19, 0, 6)
        Me.tlpFirstResDataNET.Controls.Add(Me.Label20, 0, 7)
        Me.tlpFirstResDataNET.Controls.Add(Me.Label21, 0, 8)
        Me.tlpFirstResDataNET.Controls.Add(Me.Label22, 0, 9)
        Me.tlpFirstResDataNET.Controls.Add(Me.Label23, 0, 10)
        Me.tlpFirstResDataNET.Controls.Add(Me.Label24, 1, 0)
        Me.tlpFirstResDataNET.Controls.Add(Me.Label25, 2, 0)
        Me.tlpFirstResDataNET.Controls.Add(Me.Label26, 3, 0)
        Me.tlpFirstResDataNET.Controls.Add(Me.Label27, 4, 0)
        Me.tlpFirstResDataNET.Controls.Add(Me.Res1Cut1, 1, 1)
        Me.tlpFirstResDataNET.Controls.Add(Me.Res1Cut2, 1, 2)
        Me.tlpFirstResDataNET.Controls.Add(Me.Res1Cut3, 1, 3)
        Me.tlpFirstResDataNET.Controls.Add(Me.Res1Cut4, 1, 4)
        Me.tlpFirstResDataNET.Controls.Add(Me.Res1Cut5, 1, 5)
        Me.tlpFirstResDataNET.Controls.Add(Me.Res1Cut6, 1, 6)
        Me.tlpFirstResDataNET.Controls.Add(Me.Res1Cut7, 1, 7)
        Me.tlpFirstResDataNET.Controls.Add(Me.Res1Cut8, 1, 8)
        Me.tlpFirstResDataNET.Controls.Add(Me.Res1Cut9, 1, 9)
        Me.tlpFirstResDataNET.Controls.Add(Me.Res1Cut10, 1, 10)
        Me.tlpFirstResDataNET.Controls.Add(Me.Res2Cut1, 2, 1)
        Me.tlpFirstResDataNET.Controls.Add(Me.Res2Cut2, 2, 2)
        Me.tlpFirstResDataNET.Controls.Add(Me.Res2Cut3, 2, 3)
        Me.tlpFirstResDataNET.Controls.Add(Me.Res2Cut4, 2, 4)
        Me.tlpFirstResDataNET.Controls.Add(Me.Res2Cut5, 2, 5)
        Me.tlpFirstResDataNET.Controls.Add(Me.Res2Cut6, 2, 6)
        Me.tlpFirstResDataNET.Controls.Add(Me.Res2Cut7, 2, 7)
        Me.tlpFirstResDataNET.Controls.Add(Me.Res2Cut8, 2, 8)
        Me.tlpFirstResDataNET.Controls.Add(Me.Res2Cut9, 2, 9)
        Me.tlpFirstResDataNET.Controls.Add(Me.Res2Cut10, 2, 10)
        Me.tlpFirstResDataNET.Controls.Add(Me.Res3Cut1, 3, 1)
        Me.tlpFirstResDataNET.Controls.Add(Me.Res3Cut2, 3, 2)
        Me.tlpFirstResDataNET.Controls.Add(Me.Res3Cut3, 3, 3)
        Me.tlpFirstResDataNET.Controls.Add(Me.Res3Cut4, 3, 4)
        Me.tlpFirstResDataNET.Controls.Add(Me.Res3Cut5, 3, 5)
        Me.tlpFirstResDataNET.Controls.Add(Me.Res3Cut6, 3, 6)
        Me.tlpFirstResDataNET.Controls.Add(Me.Res3Cut7, 3, 7)
        Me.tlpFirstResDataNET.Controls.Add(Me.Res3Cut8, 3, 8)
        Me.tlpFirstResDataNET.Controls.Add(Me.Res3Cut9, 3, 9)
        Me.tlpFirstResDataNET.Controls.Add(Me.Res3Cut10, 3, 10)
        Me.tlpFirstResDataNET.Controls.Add(Me.Res4Cut1, 4, 1)
        Me.tlpFirstResDataNET.Controls.Add(Me.Res4Cut2, 4, 2)
        Me.tlpFirstResDataNET.Controls.Add(Me.Res4Cut3, 4, 3)
        Me.tlpFirstResDataNET.Controls.Add(Me.Res4Cut4, 4, 4)
        Me.tlpFirstResDataNET.Controls.Add(Me.Res4Cut5, 4, 5)
        Me.tlpFirstResDataNET.Controls.Add(Me.Res4Cut6, 4, 6)
        Me.tlpFirstResDataNET.Controls.Add(Me.Res4Cut7, 4, 7)
        Me.tlpFirstResDataNET.Controls.Add(Me.Res4Cut8, 4, 8)
        Me.tlpFirstResDataNET.Controls.Add(Me.Res4Cut9, 4, 9)
        Me.tlpFirstResDataNET.Controls.Add(Me.Res4Cut10, 4, 10)
        Me.tlpFirstResDataNET.Name = "tlpFirstResDataNET"
        '
        'Label8
        '
        resources.ApplyResources(Me.Label8, "Label8")
        Me.Label8.Name = "Label8"
        '
        'Label9
        '
        resources.ApplyResources(Me.Label9, "Label9")
        Me.Label9.Name = "Label9"
        '
        'Label10
        '
        resources.ApplyResources(Me.Label10, "Label10")
        Me.Label10.Name = "Label10"
        '
        'Label11
        '
        resources.ApplyResources(Me.Label11, "Label11")
        Me.Label11.Name = "Label11"
        '
        'Label12
        '
        resources.ApplyResources(Me.Label12, "Label12")
        Me.Label12.Name = "Label12"
        '
        'Label13
        '
        resources.ApplyResources(Me.Label13, "Label13")
        Me.Label13.Name = "Label13"
        '
        'Label19
        '
        resources.ApplyResources(Me.Label19, "Label19")
        Me.Label19.Name = "Label19"
        '
        'Label20
        '
        resources.ApplyResources(Me.Label20, "Label20")
        Me.Label20.Name = "Label20"
        '
        'Label21
        '
        resources.ApplyResources(Me.Label21, "Label21")
        Me.Label21.Name = "Label21"
        '
        'Label22
        '
        resources.ApplyResources(Me.Label22, "Label22")
        Me.Label22.Name = "Label22"
        '
        'Label23
        '
        resources.ApplyResources(Me.Label23, "Label23")
        Me.Label23.Name = "Label23"
        '
        'Label24
        '
        resources.ApplyResources(Me.Label24, "Label24")
        Me.Label24.Name = "Label24"
        '
        'Label25
        '
        resources.ApplyResources(Me.Label25, "Label25")
        Me.Label25.Name = "Label25"
        '
        'Label26
        '
        resources.ApplyResources(Me.Label26, "Label26")
        Me.Label26.Name = "Label26"
        '
        'Label27
        '
        resources.ApplyResources(Me.Label27, "Label27")
        Me.Label27.Name = "Label27"
        '
        'Res1Cut1
        '
        resources.ApplyResources(Me.Res1Cut1, "Res1Cut1")
        Me.Res1Cut1.Name = "Res1Cut1"
        '
        'Res1Cut2
        '
        resources.ApplyResources(Me.Res1Cut2, "Res1Cut2")
        Me.Res1Cut2.Name = "Res1Cut2"
        '
        'Res1Cut3
        '
        resources.ApplyResources(Me.Res1Cut3, "Res1Cut3")
        Me.Res1Cut3.Name = "Res1Cut3"
        '
        'Res1Cut4
        '
        resources.ApplyResources(Me.Res1Cut4, "Res1Cut4")
        Me.Res1Cut4.Name = "Res1Cut4"
        '
        'Res1Cut5
        '
        resources.ApplyResources(Me.Res1Cut5, "Res1Cut5")
        Me.Res1Cut5.Name = "Res1Cut5"
        '
        'Res1Cut6
        '
        resources.ApplyResources(Me.Res1Cut6, "Res1Cut6")
        Me.Res1Cut6.Name = "Res1Cut6"
        '
        'Res1Cut7
        '
        resources.ApplyResources(Me.Res1Cut7, "Res1Cut7")
        Me.Res1Cut7.Name = "Res1Cut7"
        '
        'Res1Cut8
        '
        resources.ApplyResources(Me.Res1Cut8, "Res1Cut8")
        Me.Res1Cut8.Name = "Res1Cut8"
        '
        'Res1Cut9
        '
        resources.ApplyResources(Me.Res1Cut9, "Res1Cut9")
        Me.Res1Cut9.Name = "Res1Cut9"
        '
        'Res1Cut10
        '
        resources.ApplyResources(Me.Res1Cut10, "Res1Cut10")
        Me.Res1Cut10.Name = "Res1Cut10"
        '
        'Res2Cut1
        '
        resources.ApplyResources(Me.Res2Cut1, "Res2Cut1")
        Me.Res2Cut1.Name = "Res2Cut1"
        '
        'Res2Cut2
        '
        resources.ApplyResources(Me.Res2Cut2, "Res2Cut2")
        Me.Res2Cut2.Name = "Res2Cut2"
        '
        'Res2Cut3
        '
        resources.ApplyResources(Me.Res2Cut3, "Res2Cut3")
        Me.Res2Cut3.Name = "Res2Cut3"
        '
        'Res2Cut4
        '
        resources.ApplyResources(Me.Res2Cut4, "Res2Cut4")
        Me.Res2Cut4.Name = "Res2Cut4"
        '
        'Res2Cut5
        '
        resources.ApplyResources(Me.Res2Cut5, "Res2Cut5")
        Me.Res2Cut5.Name = "Res2Cut5"
        '
        'Res2Cut6
        '
        resources.ApplyResources(Me.Res2Cut6, "Res2Cut6")
        Me.Res2Cut6.Name = "Res2Cut6"
        '
        'Res2Cut7
        '
        resources.ApplyResources(Me.Res2Cut7, "Res2Cut7")
        Me.Res2Cut7.Name = "Res2Cut7"
        '
        'Res2Cut8
        '
        resources.ApplyResources(Me.Res2Cut8, "Res2Cut8")
        Me.Res2Cut8.Name = "Res2Cut8"
        '
        'Res2Cut9
        '
        resources.ApplyResources(Me.Res2Cut9, "Res2Cut9")
        Me.Res2Cut9.Name = "Res2Cut9"
        '
        'Res2Cut10
        '
        resources.ApplyResources(Me.Res2Cut10, "Res2Cut10")
        Me.Res2Cut10.Name = "Res2Cut10"
        '
        'Res3Cut1
        '
        resources.ApplyResources(Me.Res3Cut1, "Res3Cut1")
        Me.Res3Cut1.Name = "Res3Cut1"
        '
        'Res3Cut2
        '
        resources.ApplyResources(Me.Res3Cut2, "Res3Cut2")
        Me.Res3Cut2.Name = "Res3Cut2"
        '
        'Res3Cut3
        '
        resources.ApplyResources(Me.Res3Cut3, "Res3Cut3")
        Me.Res3Cut3.Name = "Res3Cut3"
        '
        'Res3Cut4
        '
        resources.ApplyResources(Me.Res3Cut4, "Res3Cut4")
        Me.Res3Cut4.Name = "Res3Cut4"
        '
        'Res3Cut5
        '
        resources.ApplyResources(Me.Res3Cut5, "Res3Cut5")
        Me.Res3Cut5.Name = "Res3Cut5"
        '
        'Res3Cut6
        '
        resources.ApplyResources(Me.Res3Cut6, "Res3Cut6")
        Me.Res3Cut6.Name = "Res3Cut6"
        '
        'Res3Cut7
        '
        resources.ApplyResources(Me.Res3Cut7, "Res3Cut7")
        Me.Res3Cut7.Name = "Res3Cut7"
        '
        'Res3Cut8
        '
        resources.ApplyResources(Me.Res3Cut8, "Res3Cut8")
        Me.Res3Cut8.Name = "Res3Cut8"
        '
        'Res3Cut9
        '
        resources.ApplyResources(Me.Res3Cut9, "Res3Cut9")
        Me.Res3Cut9.Name = "Res3Cut9"
        '
        'Res3Cut10
        '
        resources.ApplyResources(Me.Res3Cut10, "Res3Cut10")
        Me.Res3Cut10.Name = "Res3Cut10"
        '
        'Res4Cut1
        '
        resources.ApplyResources(Me.Res4Cut1, "Res4Cut1")
        Me.Res4Cut1.Name = "Res4Cut1"
        '
        'Res4Cut2
        '
        resources.ApplyResources(Me.Res4Cut2, "Res4Cut2")
        Me.Res4Cut2.Name = "Res4Cut2"
        '
        'Res4Cut3
        '
        resources.ApplyResources(Me.Res4Cut3, "Res4Cut3")
        Me.Res4Cut3.Name = "Res4Cut3"
        '
        'Res4Cut4
        '
        resources.ApplyResources(Me.Res4Cut4, "Res4Cut4")
        Me.Res4Cut4.Name = "Res4Cut4"
        '
        'Res4Cut5
        '
        resources.ApplyResources(Me.Res4Cut5, "Res4Cut5")
        Me.Res4Cut5.Name = "Res4Cut5"
        '
        'Res4Cut6
        '
        resources.ApplyResources(Me.Res4Cut6, "Res4Cut6")
        Me.Res4Cut6.Name = "Res4Cut6"
        '
        'Res4Cut7
        '
        resources.ApplyResources(Me.Res4Cut7, "Res4Cut7")
        Me.Res4Cut7.Name = "Res4Cut7"
        '
        'Res4Cut8
        '
        resources.ApplyResources(Me.Res4Cut8, "Res4Cut8")
        Me.Res4Cut8.Name = "Res4Cut8"
        '
        'Res4Cut9
        '
        resources.ApplyResources(Me.Res4Cut9, "Res4Cut9")
        Me.Res4Cut9.Name = "Res4Cut9"
        '
        'Res4Cut10
        '
        resources.ApplyResources(Me.Res4Cut10, "Res4Cut10")
        Me.Res4Cut10.Name = "Res4Cut10"
        '
        'System1
        '
        Me.System1.BackColor = System.Drawing.Color.Green
        resources.ApplyResources(Me.System1, "System1")
        Me.System1.Name = "System1"
        '
        'Probe1
        '
        Me.Probe1.cOFFLINEcDEBUG = 0
        resources.ApplyResources(Me.Probe1, "Probe1")
        Me.Probe1.ModuleInformation = Nothing
        Me.Probe1.Name = "Probe1"
        '
        'Ctl_LaserTeach2
        '
        Me.Ctl_LaserTeach2.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Ctl_LaserTeach2.cOFFLINEcDEBUG = 0
        resources.ApplyResources(Me.Ctl_LaserTeach2, "Ctl_LaserTeach2")
        Me.Ctl_LaserTeach2.ModuleInformation = Nothing
        Me.Ctl_LaserTeach2.Name = "Ctl_LaserTeach2"
        '
        'Password1
        '
        Me.Password1.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        resources.ApplyResources(Me.Password1, "Password1")
        Me.Password1.Name = "Password1"
        '
        'Teaching1
        '
        Me.Teaching1.cOFFLINEcDEBUG = 0
        resources.ApplyResources(Me.Teaching1, "Teaching1")
        Me.Teaching1.Name = "Teaching1"
        Me.Teaching1.ZOFF = 0R
        Me.Teaching1.ZON = 0R
        '
        'HelpVersion1
        '
        resources.ApplyResources(Me.HelpVersion1, "HelpVersion1")
        Me.HelpVersion1.Name = "HelpVersion1"
        '
        'ManualTeach1
        '
        Me.ManualTeach1.BackColor = System.Drawing.Color.Blue
        resources.ApplyResources(Me.ManualTeach1, "ManualTeach1")
        Me.ManualTeach1.Name = "ManualTeach1"
        '
        'Utility1
        '
        Me.Utility1.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        resources.ApplyResources(Me.Utility1, "Utility1")
        Me.Utility1.Name = "Utility1"
        '
        'VideoLibrary1
        '
        Me.VideoLibrary1.BackColor = System.Drawing.Color.Transparent
        Me.VideoLibrary1.cDBGRdraw = 0
        Me.VideoLibrary1.cOFFLINEcDEBUG = 0
        Me.VideoLibrary1.CorrectTrimPosX = 0R
        Me.VideoLibrary1.CorrectTrimPosY = 0R
        Me.VideoLibrary1.CorrThresh = 0R
        Me.VideoLibrary1.frmLeft = 0
        Me.VideoLibrary1.frmTop = 0
        resources.ApplyResources(Me.VideoLibrary1, "VideoLibrary1")
        Me.VideoLibrary1.ModuleInformation = "VV4.4.0.0 2015/12/07"
        Me.VideoLibrary1.MoveToCenter = Nothing
        Me.VideoLibrary1.Name = "VideoLibrary1"
        Me.VideoLibrary1.OverLay = True
        Me.VideoLibrary1.PATTERNGROUP = CType(0, Short)
        Me.VideoLibrary1.pfBlock_x = 0R
        Me.VideoLibrary1.pfBlock_y = 0R
        Me.VideoLibrary1.pfBpOff_x = 0R
        Me.VideoLibrary1.pfBpOff_y = 0R
        Me.VideoLibrary1.pfStgOffX = 0R
        Me.VideoLibrary1.pfStgOffY = 0R
        Me.VideoLibrary1.pfTrim_x = 0R
        Me.VideoLibrary1.pfTrim_y = 0R
        Me.VideoLibrary1.PP18 = 0R
        Me.VideoLibrary1.PP30 = CType(0, Short)
        Me.VideoLibrary1.PP31 = CType(0, Short)
        Me.VideoLibrary1.pp32_x = 0R
        Me.VideoLibrary1.pp32_y = 0R
        Me.VideoLibrary1.PP33X = 0R
        Me.VideoLibrary1.PP33Y = 0R
        Me.VideoLibrary1.pp34_x = 0R
        Me.VideoLibrary1.pp34_y = 0R
        Me.VideoLibrary1.PP35 = CType(0, Short)
        Me.VideoLibrary1.pp36_x = 1.0R
        Me.VideoLibrary1.pp36_y = 1.0R
        Me.VideoLibrary1.PP37_1 = CType(0, Short)
        Me.VideoLibrary1.PP37_2 = CType(0, Short)
        Me.VideoLibrary1.PP52 = CType(0, Short)
        Me.VideoLibrary1.PP52_1 = CType(0, Short)
        Me.VideoLibrary1.PP53 = 0R
        Me.VideoLibrary1.RNASTMPNUM = False
        Me.VideoLibrary1.Status = 0
        Me.VideoLibrary1.StdMagnification = New Decimal(New Integer() {1, 0, 0, 0})
        Me.VideoLibrary1.ThetaRCenterX = 0R
        Me.VideoLibrary1.ThetaRCenterY = 0R
        Me.VideoLibrary1.ZOFF = 0R
        Me.VideoLibrary1.ZON = 0R
        Me.VideoLibrary1.zwaitpos = 0R
        '
        'TrimMap1
        '
        resources.ApplyResources(Me.TrimMap1, "TrimMap1")
        Me.TrimMap1.Name = "TrimMap1"
        '
        'Form1
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me, "$this")
        Me.ControlBox = False
        Me.Controls.Add(Me.frmHistoryData)
        Me.Controls.Add(Me.pnlFirstResDataNET)
        Me.Controls.Add(Me.chkDistributeOnOff)
        Me.Controls.Add(Me.LabelAutoCalibLimit)
        Me.Controls.Add(Me.pnlFirstResData)
        Me.Controls.Add(Me.BtnAlarmOnOff)
        Me.Controls.Add(Me.lblCutOff)
        Me.Controls.Add(Me.btnQRLmit)
        Me.Controls.Add(Me.GrpStartBlk)
        Me.Controls.Add(Me.lblDoorOpen)
        Me.Controls.Add(Me.GrpQrCode)
        Me.Controls.Add(Me.btnJudge)
        Me.Controls.Add(Me.grpIntegrated)
        Me.Controls.Add(Me.btnUserLogon)
        Me.Controls.Add(Me.LblComandName)
        Me.Controls.Add(Me.PanelGraph)
        Me.Controls.Add(Me.System1)
        Me.Controls.Add(Me.BtnPowerOnOff)
        Me.Controls.Add(Me.pnlDataDisplay)
        Me.Controls.Add(Me.Probe1)
        Me.Controls.Add(Me.Ctl_LaserTeach2)
        Me.Controls.Add(Me.Password1)
        Me.Controls.Add(Me.Teaching1)
        Me.Controls.Add(Me.HelpVersion1)
        Me.Controls.Add(Me.ManualTeach1)
        Me.Controls.Add(Me.tabCmd)
        Me.Controls.Add(Me.Utility1)
        Me.Controls.Add(Me.GrpStrageBox)
        Me.Controls.Add(Me.BtnPrint)
        Me.Controls.Add(Me.BtnPrintOnOff)
        Me.Controls.Add(Me.TxtBoxPrint)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.GrpNgBox)
        Me.Controls.Add(Me.picGraphAccumulation)
        Me.Controls.Add(Me.CmdEnd)
        Me.Controls.Add(Me.lblInterLockMSG)
        Me.Controls.Add(Me.grpDbg)
        Me.Controls.Add(Me.GrpMode)
        Me.Controls.Add(Me.btnDbg)
        Me.Controls.Add(Me.btnGoClipboard)
        Me.Controls.Add(Me.LblRotAtt)
        Me.Controls.Add(Me.CMdIX2Log)
        Me.Controls.Add(Me.cmdEsLog)
        Me.Controls.Add(Me.CmdCnd)
        Me.Controls.Add(Me.mnuHelpAbout)
        Me.Controls.Add(Me.txtLog)
        Me.Controls.Add(Me.LblMes)
        Me.Controls.Add(Me.LblCur)
        Me.Controls.Add(Me.lblLoginResult)
        Me.Controls.Add(Me.LblDataFileName)
        Me.Controls.Add(Me.Line1)
        Me.Controls.Add(Me.lblLogging)
        Me.Controls.Add(Me.Text4)
        Me.Controls.Add(Me.VideoLibrary1)
        Me.Controls.Add(Me.TrimMap1)
        Me.Controls.Add(Me.PanelMap)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Me.Name = "Form1"
        Me.ShowIcon = False
        Me.picGraphAccumulation.ResumeLayout(False)
        Me.picGraphAccumulation.PerformLayout()
        Me.frmHistoryData.ResumeLayout(False)
        Me.frmHistoryData.PerformLayout()
        Me.PanelGraph.ResumeLayout(False)
        Me.PanelGraph.PerformLayout()
        Me.GrpQrCode.ResumeLayout(False)
        Me.GrpQrCode.PerformLayout()
        Me.tabCmd.ResumeLayout(False)
        Me.tabBaseCmnds.ResumeLayout(False)
        Me.tabOptCmnds.ResumeLayout(False)
        Me.tabOptCmnds.PerformLayout()
        Me.tabOptCmnd2.ResumeLayout(False)
        Me.tabOptCmnd2.PerformLayout()
        Me.tabOptCmnd3.ResumeLayout(False)
        Me.grpDbg.ResumeLayout(False)
        Me.grpDbg.PerformLayout()
        Me.GrpMode.ResumeLayout(False)
        Me.GrpNgBox.ResumeLayout(False)
        Me.GrpNgBox.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GrpStrageBox.ResumeLayout(False)
        Me.grpIntegrated.ResumeLayout(False)
        Me.flpIntegrated.ResumeLayout(False)
        Me.GrpStartBlk.ResumeLayout(False)
        Me.GrpStartBlk.PerformLayout()
        Me.tlpStartBlk.ResumeLayout(False)
        Me.tlpStartBlk.PerformLayout()
        Me.PanelMap.ResumeLayout(False)
        Me.pnlFirstResData.ResumeLayout(False)
        Me.tlpFirstResData.ResumeLayout(False)
        Me.tlpFirstResData.PerformLayout()
        Me.pnlFirstResDataNET.ResumeLayout(False)
        Me.tlpFirstResDataNET.ResumeLayout(False)
        Me.tlpFirstResDataNET.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents LblRotAtt As System.Windows.Forms.Label
    Public WithEvents CmdCutPos As System.Windows.Forms.Button
    'Friend WithEvents AxMSChart1 As AxMSChart20Lib.AxMSChart
    Public WithEvents Line2 As System.Windows.Forms.Label
    Public WithEvents Line3 As System.Windows.Forms.Label
    Public WithEvents btnCounterClear As System.Windows.Forms.Button
    Friend WithEvents LblGO As System.Windows.Forms.Label
    Friend WithEvents LblTNG As System.Windows.Forms.Label
    Friend WithEvents LblUnit As System.Windows.Forms.Label
    Friend WithEvents LblPlate As System.Windows.Forms.Label
    Friend WithEvents LblPLTNUM As System.Windows.Forms.Label
    Friend WithEvents LblITHING As System.Windows.Forms.Label
    Friend WithEvents LblITLONG As System.Windows.Forms.Label
    Friend WithEvents LblOVER As System.Windows.Forms.Label
    Friend WithEvents LblREGNUM As System.Windows.Forms.Label
    Friend WithEvents LblFTHING As System.Windows.Forms.Label
    Friend WithEvents LblFTLONG As System.Windows.Forms.Label
    Friend WithEvents LblNGPER As System.Windows.Forms.Label
    Friend WithEvents LblFTHINGP As System.Windows.Forms.Label
    Friend WithEvents LblFTLONGP As System.Windows.Forms.Label
    Friend WithEvents LblOVERP As System.Windows.Forms.Label
    Friend WithEvents LblcOK As System.Windows.Forms.Label
    Friend WithEvents LblcITHING As System.Windows.Forms.Label
    Friend WithEvents LblcFTHING As System.Windows.Forms.Label
    Friend WithEvents LblcOVER As System.Windows.Forms.Label
    Friend WithEvents LblcREGNUM As System.Windows.Forms.Label
    Friend WithEvents LblcNG As System.Windows.Forms.Label
    Friend WithEvents LblcITLONG As System.Windows.Forms.Label
    Friend WithEvents LblcFTLONG As System.Windows.Forms.Label
    Friend WithEvents LblcNGPER As System.Windows.Forms.Label
    Friend WithEvents LblcITHINGP As System.Windows.Forms.Label
    Friend WithEvents LblcITLONGP As System.Windows.Forms.Label
    Friend WithEvents LblcFTHINGP As System.Windows.Forms.Label
    Friend WithEvents LblcFTLONGP As System.Windows.Forms.Label
    Public WithEvents frmHistoryData As System.Windows.Forms.GroupBox
    Friend WithEvents LblcOVERP As System.Windows.Forms.Label
    Public WithEvents CmdEdit As System.Windows.Forms.Button
    Public WithEvents CmdSave As System.Windows.Forms.Button
    Friend WithEvents grpDbg As System.Windows.Forms.GroupBox
    Friend WithEvents btnDbgForm As System.Windows.Forms.Button
    Friend WithEvents GrpMode As System.Windows.Forms.GroupBox
    Public WithEvents LblDIGSW_HI As System.Windows.Forms.Label
    Public WithEvents LblDIGSW As System.Windows.Forms.Label
    Friend WithEvents CbDigSwL As System.Windows.Forms.ComboBox
    Friend WithEvents CbDigSwH As System.Windows.Forms.ComboBox
    Public WithEvents txtLog As System.Windows.Forms.TextBox
    Public WithEvents btnGoClipboard As System.Windows.Forms.Button
    Friend WithEvents chkDistributeOnOff As System.Windows.Forms.CheckBox
    Friend WithEvents btnDbg As System.Windows.Forms.Button
    Friend WithEvents tabCmd As System.Windows.Forms.TabControl
    Friend WithEvents tabBaseCmnds As System.Windows.Forms.TabPage
    Friend WithEvents tabOptCmnds As System.Windows.Forms.TabPage
    Friend WithEvents txtCount As System.Windows.Forms.TextBox
    Public WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmdStop As System.Windows.Forms.Button
    Friend WithEvents cmdTestRun As System.Windows.Forms.Button
    Public WithEvents CmdLoaderInit As System.Windows.Forms.Button
    Public WithEvents CmdAutoOperation As System.Windows.Forms.Button
    Friend WithEvents lblCutPos As System.Windows.Forms.Label
    Friend WithEvents lblCalibration As System.Windows.Forms.Label
    Friend WithEvents lblExCamera As System.Windows.Forms.Label
    Friend WithEvents BtnADJ As System.Windows.Forms.Button
    Friend WithEvents lblBreakCount As System.Windows.Forms.Label
    Friend WithEvents lblTwoCount As System.Windows.Forms.Label
    Friend WithEvents GrpNgBox As System.Windows.Forms.GroupBox
    Friend WithEvents lblNgCount As System.Windows.Forms.Label
    Friend WithEvents TimerInterLockSts As System.Windows.Forms.Timer
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents CheckBox4 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox3 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox2 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    'Friend WithEvents Teaching1 As AxTeach.AxTeaching
    Friend WithEvents TxtBoxPrint As System.Windows.Forms.TextBox
    Friend WithEvents GrpMatrix As System.Windows.Forms.GroupBox
    Friend WithEvents tabOptCmnd2 As System.Windows.Forms.TabPage
    Public WithEvents CmdMap As System.Windows.Forms.Button
    Public WithEvents CmdSinsyukuPtn As System.Windows.Forms.Button
    Public WithEvents CmdAotoProbeCorrect As System.Windows.Forms.Button
    Public WithEvents CmdAotoProbePtn As System.Windows.Forms.Button
    Public WithEvents CmdIDTeach As System.Windows.Forms.Button
    Friend WithEvents lblSinsyuku As System.Windows.Forms.Label
    Friend WithEvents lblAutoProbe As System.Windows.Forms.Label
    Public WithEvents BtnPrint As System.Windows.Forms.Button
    Public WithEvents BtnPrintOnOff As System.Windows.Forms.Button
    Friend WithEvents TimerQR As System.Windows.Forms.Timer
    Friend WithEvents GrpStrageBox As System.Windows.Forms.GroupBox
    Friend WithEvents BtnStrageClr As System.Windows.Forms.Button
    Friend WithEvents LblStrageBoxCount As System.Windows.Forms.Label
    Friend WithEvents LblStrageBoxTtl As System.Windows.Forms.Label
    Friend WithEvents Ctl_LaserTeach2 As LaserFront.Trimmer.DllLaserTeach.ctl_LaserTeach
    Friend WithEvents Utility1 As LaserFront.Trimmer.DllUtility.Utility
    Friend WithEvents System1 As LaserFront.Trimmer.DllSystem.SystemNET
    Friend WithEvents Teaching1 As LaserFront.Trimmer.DllTeach.Teaching
    Friend WithEvents Probe1 As LaserFront.Trimmer.DllProbeTeach.Probe
    Friend WithEvents ManualTeach1 As LaserFront.Trimmer.DllManualTeach.ManualTeach
    Friend WithEvents HelpVersion1 As LaserFront.Trimmer.DllAbout.HelpVersion
    Friend WithEvents Password1 As LaserFront.Trimmer.DllPassword.Password
    Friend WithEvents TimerAdjust As System.Windows.Forms.Timer
    Friend WithEvents pnlDataDisplay As System.Windows.Forms.Panel
    Friend WithEvents GrpQrCode As System.Windows.Forms.GroupBox
    Friend WithEvents lblQRData As System.Windows.Forms.Label
    Friend WithEvents LblITLONGP As System.Windows.Forms.Label
    Friend WithEvents LblITHINGP As System.Windows.Forms.Label
    Public WithEvents TimerBC As System.Windows.Forms.Timer
    Public WithEvents BtnPowerOnOff As System.Windows.Forms.Button
    Public WithEvents PanelGraph As System.Windows.Forms.Panel
    Public WithEvents cmdFinal As System.Windows.Forms.Button
    Public WithEvents LblGrpPer_00 As System.Windows.Forms.Label
    Public WithEvents LblRegN_00 As System.Windows.Forms.Label
    Public WithEvents cmdGraphSave As System.Windows.Forms.Button
    Public WithEvents LblShpGrp_00 As System.Windows.Forms.Label
    Public WithEvents cmdInitial As System.Windows.Forms.Button
    Public WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents Label4 As System.Windows.Forms.Label
    Public WithEvents Label5 As System.Windows.Forms.Label
    Public WithEvents Label6 As System.Windows.Forms.Label
    Public WithEvents Label7 As System.Windows.Forms.Label
    Public WithEvents lblGoodChip2 As System.Windows.Forms.Label
    Public WithEvents lblNgChip2 As System.Windows.Forms.Label
    Public WithEvents lblDeviationValue2 As System.Windows.Forms.Label
    Public WithEvents lblAverageValue2 As System.Windows.Forms.Label
    Public WithEvents lblMaxValue2 As System.Windows.Forms.Label
    Public WithEvents lblMinValue2 As System.Windows.Forms.Label
    Public WithEvents LblShpGrp_11 As System.Windows.Forms.Label
    Public WithEvents LblShpGrp_10 As System.Windows.Forms.Label
    Public WithEvents LblShpGrp_09 As System.Windows.Forms.Label
    Public WithEvents LblShpGrp_08 As System.Windows.Forms.Label
    Public WithEvents LblShpGrp_07 As System.Windows.Forms.Label
    Public WithEvents LblShpGrp_06 As System.Windows.Forms.Label
    Public WithEvents LblShpGrp_05 As System.Windows.Forms.Label
    Public WithEvents LblShpGrp_04 As System.Windows.Forms.Label
    Public WithEvents LblShpGrp_03 As System.Windows.Forms.Label
    Public WithEvents LblShpGrp_02 As System.Windows.Forms.Label
    Public WithEvents LblShpGrp_01 As System.Windows.Forms.Label
    Public WithEvents LblGrpPer_11 As System.Windows.Forms.Label
    Public WithEvents LblGrpPer_10 As System.Windows.Forms.Label
    Public WithEvents LblGrpPer_09 As System.Windows.Forms.Label
    Public WithEvents LblGrpPer_08 As System.Windows.Forms.Label
    Public WithEvents LblGrpPer_07 As System.Windows.Forms.Label
    Public WithEvents LblGrpPer_06 As System.Windows.Forms.Label
    Public WithEvents LblGrpPer_05 As System.Windows.Forms.Label
    Public WithEvents LblGrpPer_04 As System.Windows.Forms.Label
    Public WithEvents LblGrpPer_03 As System.Windows.Forms.Label
    Public WithEvents LblGrpPer_02 As System.Windows.Forms.Label
    Public WithEvents LblGrpPer_01 As System.Windows.Forms.Label
    Public WithEvents LblRegN_11 As System.Windows.Forms.Label
    Public WithEvents LblRegN_10 As System.Windows.Forms.Label
    Public WithEvents LblRegN_09 As System.Windows.Forms.Label
    Public WithEvents LblRegN_08 As System.Windows.Forms.Label
    Public WithEvents LblRegN_07 As System.Windows.Forms.Label
    Public WithEvents LblRegN_06 As System.Windows.Forms.Label
    Public WithEvents LblRegN_05 As System.Windows.Forms.Label
    Public WithEvents LblRegN_04 As System.Windows.Forms.Label
    Public WithEvents LblRegN_03 As System.Windows.Forms.Label
    Public WithEvents LblRegN_02 As System.Windows.Forms.Label
    Public WithEvents LblRegN_01 As System.Windows.Forms.Label
    Public WithEvents Label14 As System.Windows.Forms.Label
    Public WithEvents Label15 As System.Windows.Forms.Label
    Public WithEvents Label16 As System.Windows.Forms.Label
    Public WithEvents Label17 As System.Windows.Forms.Label
    Public WithEvents Label18 As System.Windows.Forms.Label
    Public WithEvents lblRegistUnit2 As System.Windows.Forms.Label
    Public WithEvents lblGraphAccumulationTitle2 As System.Windows.Forms.Label
    Friend WithEvents LblComandName As System.Windows.Forms.Label
    Friend WithEvents btnUserLogon As System.Windows.Forms.Button
    Public WithEvents CmdIntegrated As System.Windows.Forms.Button
    Private WithEvents grpIntegrated As System.Windows.Forms.GroupBox
    Private WithEvents lblIntegTY As System.Windows.Forms.Label
    Private WithEvents lblIntegTeach As System.Windows.Forms.Label
    Private WithEvents lblIntegTX As System.Windows.Forms.Label
    Private WithEvents lblIntegProbe As System.Windows.Forms.Label
    Private WithEvents lblIntegRecog As System.Windows.Forms.Label
    Friend WithEvents flpIntegrated As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents tabOptCmnd3 As System.Windows.Forms.TabPage
    Public WithEvents CmdT_ProbeCleaning As System.Windows.Forms.Button
    Friend WithEvents btnJudge As System.Windows.Forms.Button
    Friend WithEvents VideoLibrary1 As LaserFront.Trimmer.DllVideo.VideoLibrary
    Friend WithEvents BtnSubstrateSet As System.Windows.Forms.Button
    Friend WithEvents btnCycleStop As System.Windows.Forms.Button
    Friend WithEvents lblDoorOpen As System.Windows.Forms.Label
    Friend WithEvents GrpStartBlk As System.Windows.Forms.GroupBox
    Friend WithEvents tlpStartBlk As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents CbStartBlkY As System.Windows.Forms.ComboBox
    Friend WithEvents lblStartBlkY As System.Windows.Forms.Label
    Friend WithEvents CbStartBlkX As System.Windows.Forms.ComboBox
    Friend WithEvents lblStartBlkX As System.Windows.Forms.Label
    Friend WithEvents chkContinue As System.Windows.Forms.CheckBox
    Friend WithEvents btnPREV As System.Windows.Forms.Button
    Friend WithEvents btnNEXT As System.Windows.Forms.Button
    Private WithEvents btnRest As System.Windows.Forms.Button
    Public WithEvents CmdMapOnOff As Button
    Private WithEvents TrimMap1 As TrimControlLibrary.TrimMapPlate
    Private WithEvents PanelMap As Panel
    Public WithEvents CmdPrintMap As Button
    Public WithEvents btnQRLmit As System.Windows.Forms.Button
    Friend WithEvents lblCutOff As System.Windows.Forms.Label
    Friend WithEvents BtnAlarmOnOff As System.Windows.Forms.Button
    Public WithEvents CmdRecogRough As Button
    Friend WithEvents LblcRESVALUE As System.Windows.Forms.Label
    Friend WithEvents LblcLOTNUMBER As System.Windows.Forms.Label
    Friend WithEvents lblProductionData As System.Windows.Forms.Label
    Public WithEvents CmdFolderOpen As System.Windows.Forms.Button
    Friend WithEvents pnlFirstResData As System.Windows.Forms.Panel
    Private WithEvents lblFrdNomVal As System.Windows.Forms.Label
    Private WithEvents lblFrdNom As System.Windows.Forms.Label
    Private WithEvents tlpFirstResData As System.Windows.Forms.TableLayoutPanel
    Private WithEvents lblFrdCutOff As System.Windows.Forms.Label
    Private WithEvents lblFrdESPoint As System.Windows.Forms.Label
    Private WithEvents lblFrd_1 As System.Windows.Forms.Label
    Private WithEvents lblFrdC1 As System.Windows.Forms.Label
    Private WithEvents lblFrdE1 As System.Windows.Forms.Label
    Private WithEvents lblFrd_2 As System.Windows.Forms.Label
    Private WithEvents lblFrdC2 As System.Windows.Forms.Label
    Private WithEvents lblFrdE2 As System.Windows.Forms.Label
    Private WithEvents lblFrd_3 As System.Windows.Forms.Label
    Private WithEvents lblFrdC3 As System.Windows.Forms.Label
    Private WithEvents lblFrdE3 As System.Windows.Forms.Label
    Private WithEvents lblFrd_4 As System.Windows.Forms.Label
    Private WithEvents lblFrdC4 As System.Windows.Forms.Label
    Private WithEvents lblFrdE4 As System.Windows.Forms.Label
    Private WithEvents lblFrd_5 As System.Windows.Forms.Label
    Private WithEvents lblFrdC5 As System.Windows.Forms.Label
    Private WithEvents lblFrdE5 As System.Windows.Forms.Label
    Private WithEvents lblFrd_6 As System.Windows.Forms.Label
    Private WithEvents lblFrdC6 As System.Windows.Forms.Label
    Private WithEvents lblFrdE6 As System.Windows.Forms.Label
    Public WithEvents LabelAutoCalibLimit As Label
    Public WithEvents CutOffEsEditButton As Button
    Friend WithEvents pnlFirstResDataNET As Panel
    Public WithEvents CutOffEsEditButtonNET As Button
    Private WithEvents lblNETNomVal As Label
    Private WithEvents lblNETNom As Label
    Friend WithEvents tlpFirstResDataNET As TableLayoutPanel
    Friend WithEvents Label8 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents Label19 As Label
    Friend WithEvents Label20 As Label
    Friend WithEvents Label21 As Label
    Friend WithEvents Label22 As Label
    Friend WithEvents Label23 As Label
    Friend WithEvents Label24 As Label
    Friend WithEvents Label25 As Label
    Friend WithEvents Label26 As Label
    Friend WithEvents Label27 As Label
    Friend WithEvents Res1Cut1 As Label
    Friend WithEvents Res1Cut2 As Label
    Friend WithEvents Res1Cut3 As Label
    Friend WithEvents Res1Cut4 As Label
    Friend WithEvents Res1Cut5 As Label
    Friend WithEvents Res1Cut6 As Label
    Friend WithEvents Res1Cut7 As Label
    Friend WithEvents Res1Cut8 As Label
    Friend WithEvents Res1Cut9 As Label
    Friend WithEvents Res1Cut10 As Label
    Friend WithEvents Res2Cut1 As Label
    Friend WithEvents Res2Cut2 As Label
    Friend WithEvents Res2Cut3 As Label
    Friend WithEvents Res2Cut4 As Label
    Friend WithEvents Res2Cut5 As Label
    Friend WithEvents Res2Cut6 As Label
    Friend WithEvents Res2Cut7 As Label
    Friend WithEvents Res2Cut8 As Label
    Friend WithEvents Res2Cut9 As Label
    Friend WithEvents Res2Cut10 As Label
    Friend WithEvents Res3Cut1 As Label
    Friend WithEvents Res3Cut2 As Label
    Friend WithEvents Res3Cut3 As Label
    Friend WithEvents Res3Cut4 As Label
    Friend WithEvents Res3Cut5 As Label
    Friend WithEvents Res3Cut6 As Label
    Friend WithEvents Res3Cut7 As Label
    Friend WithEvents Res3Cut8 As Label
    Friend WithEvents Res3Cut9 As Label
    Friend WithEvents Res3Cut10 As Label
    Friend WithEvents Res4Cut1 As Label
    Friend WithEvents Res4Cut2 As Label
    Friend WithEvents Res4Cut3 As Label
    Friend WithEvents Res4Cut4 As Label
    Friend WithEvents Res4Cut5 As Label
    Friend WithEvents Res4Cut6 As Label
    Friend WithEvents Res4Cut7 As Label
    Friend WithEvents Res4Cut8 As Label
    Friend WithEvents Res4Cut9 As Label
    Friend WithEvents Res4Cut10 As Label
#End Region
End Class