Attribute VB_Name = "DefTrimFnc"
'===============================================================================
'   Description : DllTrimFnc.dll関数の定義（新トリマー装置用）
'
'   Copyright(C) Laser Front 2011
'-------------------------------------------------------------------------------
' 【修正履歴】
' V8.00  '11/08/08 新規作成
'
'===============================================================================
Public Type TRIM_PLATE_GPIB2
     wGPIBflg As Integer               '///< GPIB制御の有無(0:無し,1:有り)
     wGPIBdlm As Integer               '//< ﾃﾞﾘﾐﾀ(0:CRLF, 1:CR, 2:LF,3:無し)
     wGPIBtout As Integer              '//< タイムアウト値(100ms単位)
     wGPIBdev As Integer               '///< 機器アドレス(0～30)
     wEOI As Integer                   '///< EOI(0:使用しない, 1:使用する)
     wPause1 As Integer                '///< 設定コマンド1送信後ポーズ時間(1～32767msec) @@@014
     wPause2 As Integer                '///< 設定コマンド2送信後ポーズ時間(1～32767msec) @@@014
     wPause3 As Integer                '///< 設定コマンド3送信後ポーズ時間(1～32767msec) @@@014
     wPauseT As Integer                '///< トリガコマンド送信後ポーズ時間(1～32767msec)@@@014
     wRsv As Integer                   '///< 予備
     cGPIBinit(39) As Byte             '///< 初期化コマンド1(後方空白)
     cGPIBini2(39) As Byte             '///< 初期化コマンド2(後方空白)
     cGPIBini3(39) As Byte             '///< 初期化コマンド3(後方空白)
     cGPIBtriger As String * 50        '///< トリガコマンド(後方空白)
     cGPIBDumy1(1) As Byte              '///< 予備
     cGPIBName(9) As Byte             '///< 機器名(後方空白)
     cGPIBDumy2(1) As Byte              '///< 予備
     cRsv(7) As Byte                   '///< 予備
End Type



'-------------------------------------------------------------------------------
'   DllTrimFnc.dll内の関数定義
'-------------------------------------------------------------------------------
Public Declare Function ALDFLGRST Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ALDFLGRST@0" () As Long
Public Declare Function BIFC Lib "C:\TRIM\DllTrimFnc.dll" Alias "_BIFC@8" (ByVal tim As Integer, ByVal brdIdx As Integer) As Long
Public Declare Function BP_CALIBRATION Lib "C:\TRIM\DllTrimFnc.dll" Alias "_BP_CALIBRATION@32" (ByVal GainX As Double, ByVal GainY As Double, ByVal OfsX As Double, ByVal OfsY As Double) As Long
Public Declare Function BPLINEARITY Lib "C:\TRIM\DllTrimFnc.dll" Alias "_BPLINEARITY@20" (ByVal XY As Integer, ByVal IDX As Integer, ByVal Flg As Integer, ByVal Val As Double) As Long
Public Declare Function BP_MOVE Lib "C:\TRIM\DllTrimFnc.dll" Alias "_MOVE@20" (ByVal dpStx As Double, ByVal dpSty As Double, ByVal Flg As Integer) As Long
Public Declare Function BPOFF Lib "C:\TRIM\DllTrimFnc.dll" Alias "_BPOFF@16" (ByVal BPOX As Double, ByVal BPOY As Double) As Long
Public Declare Function BSIZE Lib "C:\TRIM\DllTrimFnc.dll" Alias "_BSIZE@16" (ByVal BSX As Double, ByVal BSY As Double) As Long
Public Declare Function CIRCUT Lib "C:\TRIM\DllTrimFnc.dll" Alias "_CIRCUT@32" (ByVal V As Double, ByVal RADI As Double, ByVal ANG2 As Double, ByVal ANG As Double) As Long
Public Declare Function CMARK Lib "C:\TRIM\DllTrimFnc.dll" Alias "_CMARK@40" (ByVal MKSTR As String, ByVal STX As Double, ByVal STY As Double, ByVal HIGH As Double, ByVal V As Double, ByVal ANG As Integer) As Long
Public Declare Function CTRIM Lib "C:\TRIM\DllTrimFnc.dll" Alias "_CTRIM@48" (ByVal x As Double, ByVal y As Double, ByVal VX As Double, ByVal VY As Double, ByVal LIMX As Double, ByVal LIMY As Double) As Long
Public Declare Function CUT2 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_CUT2@20" (ByVal l As Double, ByVal V As Double, ByVal ANG As Integer) As Long
Public Declare Function CUTPOSCOR Lib "C:\TRIM\DllTrimFnc.dll" Alias "_CUTPOSCOR@16" (ByVal rn As Integer, ByRef POSX As Any, ByRef POSY As Any, ByRef Flg As Any) As Long
Public Declare Function DebugMode Lib "C:\TRIM\DllTrimFnc.dll" Alias "_DebugMode@8" (ByVal mode As Integer, ByVal level As Integer) As Long
Public Declare Function DREAD Lib "C:\TRIM\DllTrimFnc.dll" Alias "_DREAD@4" (ByRef DGSW As Integer) As Long
Public Declare Function DREAD2 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_DREAD2@12" (ByRef DGL As Integer, ByRef DGH As Integer, ByRef DGSW As Integer) As Long
Public Declare Function DSCAN Lib "C:\TRIM\DllTrimFnc.dll" Alias "_DSCAN@12" (ByVal HP As Integer, ByVal LP As Integer, ByVal GP As Integer) As Long
Public Declare Function EMGRESET Lib "C:\TRIM\DllTrimFnc.dll" Alias "_EMGRESET@0" () As Long
Public Declare Function EXTIN Lib "C:\TRIM\DllTrimFnc.dll" Alias "_EXTIN@4" (ByRef EIN As Long) As Long
Public Declare Function EXTOUT Lib "C:\TRIM\DllTrimFnc.dll" Alias "_EXTOUT@4" (ByVal ODAT As Long) As Long
Public Declare Function EXTOUT1 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_EXTOUT1@8" (ByVal EON As Long, ByVal EOFF As Long) As Long
Public Declare Function EXTOUT2 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_EXTOUT2@8" (ByVal EON As Long, ByVal EOFF As Long) As Long
Public Declare Function EXTOUT3 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_EXTOUT3@8" (ByVal EON As Long, ByVal EOFF As Long) As Long
Public Declare Function EXTOUT4 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_EXTOUT4@8" (ByVal EON As Long, ByVal EOFF As Long) As Long
Public Declare Function EXTOUT5 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_EXTOUT5@8" (ByVal EON As Long, ByVal EOFF As Long) As Long
Public Declare Function EXTRSTSET Lib "C:\TRIM\DllTrimFnc.dll" Alias "_EXTRSTSET@4" (ByVal ODATA As Long) As Long
Public Declare Function FAST_WMEAS Lib "C:\TRIM\DllTrimFnc.dll" Alias "_FAST_WMEAS@8" (ByRef MR As Double, ByVal OSC As Integer) As Long
Public Declare Function FPRESET Lib "C:\TRIM\DllTrimFnc.dll" Alias "_FPRESET@0" () As Long
Public Declare Function FPSET Lib "C:\TRIM\DllTrimFnc.dll" Alias "_FPSET@0" () As Long
Public Declare Function FPSET2 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_FPSET2@4" (ByVal tim As Long) As Long
'Public Declare Function GET_VERSION Lib "C:\TRIM\DllTrimFnc.dll" Alias "_GET_VERSION@4" (ByRef VER As String) As Long
Public Declare Function GET_VERSION Lib "C:\TRIM\DllTrimFnc.dll" Alias "_GET_VERSION@4" (ByRef VER As Double) As Long
Public Declare Function GETERRSTS Lib "C:\TRIM\DllTrimFnc.dll" Alias "_GETERRSTS@4" (ByRef ERRSTS As Long) As Long
Public Declare Function GETSETTIME Lib "C:\TRIM\DllTrimFnc.dll" Alias "_GETSETTIME@0" () As Long
Public Declare Function GET_STATUS Lib "C:\TRIM\DllTrimFnc.dll" Alias "_GET_STATUS@24" (ByVal bpMode As Integer, ByRef x As Double, ByRef y As Double, ByRef z As Double, ByRef BPx As Double, ByRef BPy As Double) As Long
Public Declare Function GET_Z_POS Lib "C:\TRIM\DllTrimFnc.dll" Alias "_GET_Z_POS@4" (ByRef z As Double) As Long
Public Declare Function GET_Z_POS2 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_GET_Z_POS@4" (ByRef z As Double) As Long
Public Declare Function GET_Z2_POS Lib "C:\TRIM\DllTrimFnc.dll" Alias "_GET_Z2_POS@4" (ByRef Z2 As Double) As Long
Public Declare Function GPBActRen Lib "C:\TRIM\DllTrimFnc.dll" Alias "_GPBActRen@4" (ByVal brdIdx As Integer) As Long
Public Declare Function GPBAdrStRead Lib "C:\TRIM\DllTrimFnc.dll" Alias "_GPBAdrStRead@8" (ByRef btadrst As Integer, ByVal brdIdx As Integer) As Long
Public Declare Function GPBClrRen Lib "C:\TRIM\DllTrimFnc.dll" Alias "_GPBClrRen@4" (ByVal brdIdx As Integer) As Long
Public Declare Function GPBExeSpoll Lib "C:\TRIM\DllTrimFnc.dll" Alias "_GPBExeSpoll@20" (ByRef bttlks As Integer, ByVal wtlknum As Integer, ByRef bttlk As Integer, ByRef btstb As Integer, ByVal brdIdx As Integer) As Long
Public Declare Function GPBGetAdrs Lib "C:\TRIM\DllTrimFnc.dll" Alias "_GPBGetAdrs@8" (ByRef btadrs As Integer, ByVal brdIdx As Integer) As Long
Public Declare Function GPBGetDlm Lib "C:\TRIM\DllTrimFnc.dll" Alias "_GPBGetDlm@8" (ByRef btdlm As Integer, ByVal brdIdx As Integer) As Long
Public Declare Function GPBGetTimeout Lib "C:\TRIM\DllTrimFnc.dll" Alias "_GPBGetTimeout@8" (ByRef wtim As Integer, ByVal brdIdx As Integer) As Long
Public Declare Function GPBIfc Lib "C:\TRIM\DllTrimFnc.dll" Alias "_GPBIfc@8" (ByVal wtim As Integer, ByVal brdIdx As Integer) As Long
Public Declare Function GPBInit Lib "C:\TRIM\DllTrimFnc.dll" Alias "_GPBInit@4" (ByVal brdIdx As Integer) As Long
Public Declare Function GPBRecvData Lib "C:\TRIM\DllTrimFnc.dll" Alias "_GPBRecvData@16" (ByRef btdata As Integer, ByVal wsize As Integer, ByRef wrecv As Integer, ByVal brdIdx As Integer) As Long
Public Declare Function GPBSendCmd Lib "C:\TRIM\DllTrimFnc.dll" Alias "_GPBSendCmd@12" (ByVal btcmd As String, ByVal wsize As Integer, ByVal brdIdx As Integer) As Long
Public Declare Function GPBSendData Lib "C:\TRIM\DllTrimFnc.dll" Alias "_GPBSendData@16" (ByVal btdata As String, ByVal wsize As Integer, ByVal wEOI As Integer, ByVal brdIdx As Integer) As Long
Public Declare Function GPBSetDlm Lib "C:\TRIM\DllTrimFnc.dll" Alias "_GPBSetDlm@8" (ByVal btdlm As Integer, ByVal brdIdx As Integer) As Long
Public Declare Function GPBSetTimeout Lib "C:\TRIM\DllTrimFnc.dll" Alias "_GPBSetTimeout@8" (ByVal wtim As Integer, ByVal brdIdx As Integer) As Long
Public Declare Function GPBWaitForSRQ Lib "C:\TRIM\DllTrimFnc.dll" Alias "_GPBWaitForSRQ@8" (ByVal timeout As Integer, ByVal brdIdx As Integer) As Long
Public Declare Function GPERecv Lib "C:\TRIM\DllTrimFnc.dll" Alias "_GPERecv@28" (ByVal bttlk As Integer, ByRef btlsns As Integer, ByVal wlsnnum As Integer, ByRef btmsge As Integer, ByVal wsize As Integer, ByRef wrecv As Integer, ByVal brdIdx As Integer) As Long
Public Declare Function GPESend Lib "C:\TRIM\DllTrimFnc.dll" Alias "_GPESend@20" (ByRef btlsns As Integer, ByVal wlsnnum As Integer, ByVal btmsge As String, ByVal wsize As Integer, ByVal brdIdx As Integer) As Long
Public Declare Function GPSGetSrqTkn Lib "C:\TRIM\DllTrimFnc.dll" Alias "_GPSGetSrqTkn@8" (ByRef hSrqSem As Long, ByVal brdIdx As Integer) As Long
Public Declare Function GPSInit Lib "C:\TRIM\DllTrimFnc.dll" Alias "_GPSInit@0" () As Long
Public Declare Function GPSLExeSRQ Lib "C:\TRIM\DllTrimFnc.dll" Alias "_GPSLExeSRQ@12" (ByVal wEOI As Integer, ByVal wdevst As Integer, ByVal brdIdx As Integer) As Long
Public Declare Function GPSLock Lib "C:\TRIM\DllTrimFnc.dll" Alias "_GPSLock@4" (ByVal timeout As Integer) As Long
Public Declare Function GPSUnlock Lib "C:\TRIM\DllTrimFnc.dll" Alias "_GPSUnlock@0" () As Long
Public Declare Function HCUT2 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_HCUT2@40" (ByVal LTDIR As Integer, ByVal L1 As Double, ByVal L2 As Double, ByVal L3 As Double, ByVal V As Double, ByVal ANG As Integer) As Long
Public Declare Function IACLEAR Lib "C:\TRIM\DllTrimFnc.dll" Alias "_IACLEAR@0" () As Long
Public Declare Function ICLEAR Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ICLEAR@4" (ByVal GADR As Integer) As Long
Public Declare Function ICUT2 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ICUT2@24" (ByVal n As Integer, ByVal l As Double, ByVal V As Double, ByVal ANG As Integer) As Long
Public Declare Function IDELIM Lib "C:\TRIM\DllTrimFnc.dll" Alias "_IDELIM@4" (ByVal DLM As Integer) As Long
Public Declare Function ILUM Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ILUM@4" (ByVal sw As Integer) As Long
Public Declare Function InitFunction Lib "C:\TRIM\DllTrimFnc.dll" Alias "_InitFunction@0" () As Long
Public Declare Function INP16 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_INP16@8" (ByVal Adr As Long, ByRef dat As Long) As Long
'Public Declare Function INtimeGWInitialize Lib "C:\TRIM\DllTrimFnc.dll" Alias "_INtimeGWInitialize@0" () As Long
'Public Declare Function INtimeGWTerminate Lib "C:\TRIM\DllTrimFnc.dll" Alias "_INtimeGWTerminate@0" () As Long
Public Declare Function IREAD Lib "C:\TRIM\DllTrimFnc.dll" Alias "_IREAD@8" (ByVal GADR As Integer, ByVal dat As String) As Long
Public Declare Function IREAD2 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_IREAD2@12" (ByVal GADR As Integer, ByVal GADR2 As Integer, ByVal dat As String) As Long
Public Declare Function IREADM Lib "C:\TRIM\DllTrimFnc.dll" Alias "_IREADM@16" (ByVal GADR As Integer, ByRef MAX As Integer, ByRef dat As String, ByVal DLM As String) As Long
Public Declare Function IRHVAL Lib "C:\TRIM\DllTrimFnc.dll" Alias "_IRHVAL@12" (ByVal GADR As Integer, ByVal HED As Integer, ByRef dat As Double) As Long
Public Declare Function IRHVAL2 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_IRHVAL2@16" (ByVal GADR As Integer, ByVal GADR2 As Integer, ByVal HED As Integer, ByRef dat As Double) As Long
Public Declare Function IRMVAL Lib "C:\TRIM\DllTrimFnc.dll" Alias "_IRMVAL@16" (ByVal GADR As Integer, ByRef MAX As Integer, ByRef dat As Double, ByRef DLM As String) As Long
Public Declare Function IRVAL Lib "C:\TRIM\DllTrimFnc.dll" Alias "_IRVAL@8" (ByVal GADR As Integer, ByRef dat As Double) As Long
Public Declare Function IRVAL2 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_IRVAL2@12" (ByVal GADR As Integer, ByVal GADR2 As Integer, ByRef dat As Double) As Long
Public Declare Function ITIMEOUT Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ITIMEOUT@4" (ByVal tim As Integer) As Long
Public Declare Function ITIMESET Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ITIMESET@4" (ByVal mode As Integer) As Long
Public Declare Function ITRIGGER Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ITRIGGER@4" (ByVal GADR As Integer) As Long
Public Declare Function IWRITE Lib "C:\TRIM\DllTrimFnc.dll" Alias "_IWRITE@8" (ByVal GADR As Integer, ByVal dat As String) As Long
Public Declare Function IWRITE2 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_IWRITE2@12" (ByVal GADR As Integer, ByVal GADR2 As Integer, ByVal dat As String) As Long
Public Declare Function LASEROFF Lib "C:\TRIM\DllTrimFnc.dll" Alias "_LASEROFF@0" () As Long
Public Declare Function LASERON Lib "C:\TRIM\DllTrimFnc.dll" Alias "_LASERON@0" () As Long
Public Declare Function LATTSET Lib "C:\TRIM\DllTrimFnc.dll" Alias "_LATTSET@8" (ByVal FAT As Long, ByVal RAT As Long) As Long
Public Declare Function LCUT2 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_LCUT2@32" (ByVal LTDIR As Integer, ByVal L1 As Double, ByVal L2 As Double, ByVal V As Double, ByVal ANG As Integer) As Long
Public Declare Function MEASURE Lib "C:\TRIM\DllTrimFnc.dll" Alias "_MEASURE@28" (ByVal measMode As Integer, ByVal RANGSETTYPE As Integer, ByVal MEASTYPE As Integer, ByVal TARGET As Double, ByVal RANGE As Integer, ByRef Result As Double) As Long
Public Declare Function MFSET Lib "C:\TRIM\DllTrimFnc.dll" Alias "_MFSET@4" (ByVal MSDEV As String) As Long
Public Declare Function MFSET_EX Lib "C:\TRIM\DllTrimFnc.dll" Alias "_MFSET_EX@12" (ByVal MSDEV As String, ByVal targetVal As Double) As Long
Public Declare Function MSCAN Lib "C:\TRIM\DllTrimFnc.dll" Alias "_MSCAN@28" (ByVal HP As Integer, ByVal LP As Integer, ByVal GP1 As Integer, ByVal GP2 As Integer, ByVal GP3 As Integer, ByVal GP4 As Integer, ByVal GP5 As Integer) As Long
Public Declare Function NO_OPERATION Lib "C:\TRIM\DllTrimFnc.dll" Alias "_NO_OPERATION@20" (ByRef x As Double, ByRef y As Double, ByRef z As Double, ByRef BPx As Double, ByRef BPy As Double) As Long
Public Declare Function OUT16 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_OUT16@8" (ByVal Adr As Long, ByVal dat As Long) As Long
Public Declare Function OUTBIT Lib "C:\TRIM\DllTrimFnc.dll" Alias "_OUTBIT@12" (ByVal CATEGORY As Integer, ByVal BITNUM As Integer, ByVal BON As Integer) As Long
Public Declare Function PIN16 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_PIN16@8" (ByVal Adr As Long, ByRef dat As Long) As Long
Public Declare Function PROBOFF Lib "C:\TRIM\DllTrimFnc.dll" Alias "_PROBOFF@0" () As Long
Public Declare Function PROBOFF_EX Lib "C:\TRIM\DllTrimFnc.dll" Alias "_PROBOFF_EX@8" (ByVal Pos As Double) As Long
Public Declare Function PROBOFF2 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_PROBOFF2@0" () As Long
Public Declare Function PROBON Lib "C:\TRIM\DllTrimFnc.dll" Alias "_PROBON@0" () As Long
Public Declare Function PROBON2 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_PROBON2@8" (ByVal Z2ON As Double) As Long
Public Declare Function PROBUP Lib "C:\TRIM\DllTrimFnc.dll" Alias "_PROBUP@8" (ByVal UP As Double) As Long
Public Declare Function PROCPOWER Lib "C:\TRIM\DllTrimFnc.dll" Alias "_PROCPOWER@4" (ByVal POWER As Integer) As Long
Public Declare Sub PROP_SET Lib "C:\TRIM\DllTrimFnc.dll" Alias "_PROP_SET@48" (ByVal ZON As Double, ByVal ZOFF As Double, ByVal POSX As Double, ByVal POSY As Double, ByVal SmaxX As Double, ByVal SmaxY As Double)
Public Declare Function QRATE Lib "C:\TRIM\DllTrimFnc.dll" Alias "_QRATE@8" (ByVal QR As Double) As Long
Public Declare Function RangeCorrect Lib "C:\TRIM\DllTrimFnc.dll" Alias "_RangeCorrect@24" (ByVal IDX As Integer, ByVal Val As Double, ByVal Flg As Integer, ByVal RMin As Integer, ByVal RMax As Integer) As Long
Public Declare Function RATIO2EXP Lib "C:\TRIM\DllTrimFnc.dll" Alias "_RATIO2EXP@8" (ByVal RNO As Long, ByVal MKSTR As String) As Long
Public Declare Function RBACK Lib "C:\TRIM\DllTrimFnc.dll" Alias "_RBACK@0" () As Long
Public Declare Function RESET Lib "C:\TRIM\DllTrimFnc.dll" Alias "_RESET@0" () As Long
Public Declare Function RINIT Lib "C:\TRIM\DllTrimFnc.dll" Alias "_RINIT@0" () As Long
Public Declare Function RMEAS Lib "C:\TRIM\DllTrimFnc.dll" Alias "_RMEAS@12" (ByVal mode As Integer, ByVal DVM As Integer, ByRef r As Double) As Long
Public Declare Function RMeasHL Lib "C:\TRIM\DllTrimFnc.dll" Alias "_RMeasHL@28" (ByVal HP As Integer, ByVal LP As Integer, ByVal mode As Integer, ByVal NOM As Double, ByRef r As Double, ByRef ad As Integer) As Long
Public Declare Function ROUND Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ROUND@4" (ByVal PLS As Long) As Long
Public Declare Function ROUND4 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ROUND4@8" (ByVal ANG As Double) As Long
Public Declare Function RTEST Lib "C:\TRIM\DllTrimFnc.dll" Alias "_RTEST@36" (ByVal NOM As Double, ByVal mode As Integer, ByVal LOW As Double, ByVal HIGH As Double, ByVal JM As Integer, ByVal DVM As Integer) As Long
Public Declare Function RTRACK Lib "C:\TRIM\DllTrimFnc.dll" Alias "_RTRACK@12" (ByVal NOM As Double, ByVal JM As Integer) As Long
Public Declare Function SBACK Lib "C:\TRIM\DllTrimFnc.dll" Alias "_SBACK@0" () As Long
Public Declare Function SETDLY Lib "C:\TRIM\DllTrimFnc.dll" Alias "_SETDLY@4" (ByVal DTIME As Long) As Long
Public Declare Function SLIDECOVERCHK Lib "C:\TRIM\DllTrimFnc.dll" Alias "_SLIDECOVERCHK@4" (ByVal CHK As Integer) As Long
Public Declare Function COVERCHK_ONOFF Lib "C:\TRIM\DllTrimFnc.dll" Alias "_COVERCHK_ONOFF@4" (ByVal CHK As Integer) As Long
Public Declare Function GETCOVERCHK_STS Lib "C:\TRIM\DllTrimFnc.dll" Alias "_GETCOVERCHK_STS@8" (ByRef CVRCHK As Integer, ByRef SLDCV As Integer) As Long
Public Declare Function SMOVE Lib "C:\TRIM\DllTrimFnc.dll" Alias "_SMOVE@16" (ByVal XD As Double, ByVal YD As Double) As Long
Public Declare Function SMOVE2 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_SMOVE2@16" (ByVal XP As Double, ByVal YP As Double) As Long
Public Declare Function SMOVE2_EX Lib "C:\TRIM\DllTrimFnc.dll" Alias "_SMOVE2_EX@24" (ByVal XP As Double, ByVal YP As Double, ByVal bNotLsrStop As Integer, ByVal JogMode As Integer) As Long
Public Declare Function SMOVE3 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_SMOVE3@16" (ByVal XP As Double, ByVal YP As Double) As Long         '###810⑥
'Public Declare Function START Lib "C:\TRIM\DllTrimFnc.dll" Alias "_START@20" (ByVal Z1 As Integer, ByVal XOFF As Double, ByVal YOFF As Double) As Long
Public Declare Function START Lib "C:\TRIM\DllTrimFnc.dll" Alias "_START@16" (ByVal XOFF As Double, ByVal YOFF As Double) As Long
Public Declare Function STCUT Lib "C:\TRIM\DllTrimFnc.dll" Alias "_STCUT@60" (ByVal l As Double, ByVal V As Double, ByVal NOM As Double, ByVal CUTOFF As Double, ByVal V2 As Double, ByVal Q2 As Double, ByVal dir As Integer, ByVal CUTMODE As Integer, ByVal mode As Integer) As Long
Public Declare Function SYSINIT Lib "C:\TRIM\DllTrimFnc.dll" Alias "_SYSINIT@16" (ByVal ZOFF As Double, ByVal ZON As Double) As Long
Public Declare Function TEST Lib "C:\TRIM\DllTrimFnc.dll" Alias "_TEST@36" (ByVal x As Double, ByVal NOM As Double, ByVal mode As Integer, ByVal LOW As Double, ByVal HIGH As Double) As Long
Public Declare Function TRIM_NGMARK Lib "C:\TRIM\DllTrimFnc.dll" Alias "_TRIM_NGMARK@32" (ByVal POSX As Double, ByVal POSY As Double, ByVal TM As Integer, ByVal SN As Integer, ByVal sw As Integer, ByVal Flg As Integer) As Long
Public Declare Function TRIM_RESULT Lib "C:\TRIM\DllTrimFnc.dll" Alias "_TRIM_RESULT@24" (ByVal KD As Integer, ByVal SN As Integer, ByVal NM As Integer, ByVal CI As Integer, ByVal DI As Integer, ByRef Res As Any) As Long
Public Declare Function TRIM80 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_TRIM80@24" (ByVal x As Double, ByVal y As Double, ByVal V As Double) As Long
Public Declare Function TRIMBLOCK Lib "C:\TRIM\DllTrimFnc.dll" Alias "_TRIMBLOCK@24" (ByVal MD As Integer, ByVal HZ As Integer, ByVal RI As Integer, ByVal CI As Integer, ByVal NG As Integer, sts As Any) As Long
Public Declare Function TRIMDATA Lib "C:\TRIM\DllTrimFnc.dll" Alias "_TRIMDATA@8" (msg As Any, sts As Any) As Long
Public Declare Function TRIMEND Lib "C:\TRIM\DllTrimFnc.dll" Alias "_TRIMEND@0" () As Long
Public Declare Function TSTEP Lib "C:\TRIM\DllTrimFnc.dll" Alias "_TSTEP@24" (ByVal BNX As Integer, ByVal BNY As Integer, ByVal stepOffX As Double, ByVal stepOffY As Double) As Long
Public Declare Function UCUT2 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_UCUT2@40" (ByVal LTDIR As Integer, ByVal L1 As Double, ByVal L2 As Double, ByVal RADI As Double, ByVal V As Double, ByVal ANG As Integer) As Long
Public Declare Function UCUT_PARAMSET Lib "C:\TRIM\DllTrimFnc.dll" Alias "_UCUT_PARAMSET@24" (ByVal MD As Integer, ByVal KD As Integer, ByVal RNO As Integer, ByVal IDX As Integer, ByVal EL As Integer, ByRef pstPRM As Any) As Long
Public Declare Function UCUT_RESULT Lib "C:\TRIM\DllTrimFnc.dll" Alias "_UCUT_RESULT@16" (ByVal RNO As Integer, ByVal CNO As Integer, ByRef UcutNO As Integer, ByRef InitVal As Double) As Long
Public Declare Function UCUT4RESULT Lib "C:\TRIM\DllTrimFnc.dll" Alias "_UCUT4RESULT@8" (ByRef sRegNo_p As Integer, ByRef sCutNo_p As Integer) As Long
Public Declare Function VCIRTRIM Lib "C:\TRIM\DllTrimFnc.dll" Alias "_VCIRTRIM@44" (ByVal SLP As Integer, ByVal NOM As Double, ByVal V As Double, ByVal RADI As Double, ByVal ANG2 As Double, ByVal ANG As Double) As Long
Public Declare Function VCTRIM Lib "C:\TRIM\DllTrimFnc.dll" Alias "_VCTRIM@64" (ByVal SLP As Integer, ByVal NOM As Double, ByVal MD As Integer, ByVal x As Double, ByVal y As Double, ByVal VX As Double, ByVal VY As Double, ByVal LIMX As Double, ByVal LIMY As Double) As Long
Public Declare Function VHTRIM2 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_VHTRIM2@64" (ByVal SLP As Integer, ByVal NOM As Double, ByVal MD As Integer, ByVal LTP As Double, ByVal LTDIR As Integer, ByVal L1 As Double, ByVal L2 As Double, ByVal L3 As Double, ByVal V As Double, ByVal ANG As Integer) As Long
Public Declare Function VITRIM2 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_VITRIM2@40" (ByVal SLP As Integer, ByVal NOM As Double, ByVal MD As Integer, ByVal n As Integer, ByVal l As Double, ByVal V As Double, ByVal ANG As Integer) As Long
Public Declare Function VLTRIM2 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_VLTRIM2@56" (ByVal SLP As Integer, ByVal NOM As Double, ByVal MD As Integer, ByVal LTP As Double, ByVal LTDIR As Integer, ByVal L1 As Double, ByVal L2 As Double, ByVal V As Double, ByVal ANG As Integer) As Long
Public Declare Function VMEAS Lib "C:\TRIM\DllTrimFnc.dll" Alias "_VMEAS@12" (ByVal mode As Integer, ByVal DVM As Integer, ByRef V As Double) As Long
Public Declare Function VRangeCorrect Lib "C:\TRIM\DllTrimFnc.dll" Alias "_VRangeCorrect@24" (ByVal IDX As Integer, ByVal Val As Double, ByVal Flg As Integer, ByVal RMin As Integer, ByVal RMax As Integer) As Long
Public Declare Function VTEST Lib "C:\TRIM\DllTrimFnc.dll" Alias "_VTEST@36" (ByVal NOM As Double, ByVal mode As Integer, ByVal LOW As Double, ByVal HIGH As Double, ByVal JM As Integer, ByVal DVM As Integer) As Long
Public Declare Function VTRACK Lib "C:\TRIM\DllTrimFnc.dll" Alias "_VTRACK@16" (ByVal SLP As Integer, ByVal NOM As Double, ByVal JM As Integer) As Long
Public Declare Function VTRIM2 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_VTRIM2@32" (ByVal SLP As Integer, ByVal NOM As Double, ByVal l As Double, ByVal V As Double, ByVal ANG As Integer) As Long
Public Declare Function VUTRIM2 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_VUTRIM2@64" (ByVal SLP As Integer, ByVal NOM As Double, ByVal MD As Integer, ByVal LTP As Double, ByVal LTDIR As Integer, ByVal L1 As Double, ByVal L2 As Double, ByVal RADI As Double, ByVal V As Double, ByVal ANG As Integer) As Long
Public Declare Function VUTRIM4 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_VUTRIM4@88" (ByVal SLP As Integer, ByVal NOM As Double, ByVal MD As Integer, ByVal LTP As Double, ByVal LTDIR As Integer, ByVal L1 As Double, ByVal L2 As Double, ByVal RADI As Double, ByVal V As Double, ByVal ANG As Integer, ByVal trmd As Integer, ByVal trl As Double, ByVal cn As Integer, ByVal DT As Integer, ByVal mode As Integer) As Long
Public Declare Function XYOFF Lib "C:\TRIM\DllTrimFnc.dll" Alias "_XYOFF@16" (ByVal XOFF As Double, ByVal YOFF As Double) As Long
Public Declare Function ZABSVACCUME Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ZABSVACCUME@4" (ByVal ZON As Long) As Long
Public Declare Function ZATLDGET Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ZATLDGET@4" (ByRef LDIN As Long) As Long
Public Declare Function ZATLDSET Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ZATLDSET@8" (ByVal LDON As Long, ByVal LDOFF As Long) As Long
Public Declare Function ZATLDRED Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ZATLDRED@4" (ByRef LDOUT As Long) As Long
Public Declare Function ZBPLOGICALCOORD Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ZBPLOGICALCOORD@4" (ByVal COORD As Long) As Long
Public Declare Function ZCONRST Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ZCONRST@0" () As Long
Public Declare Function ZGETBPPOS Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ZGETBPPOS@8" (ByRef XP As Double, ByRef YP As Double) As Long
Public Declare Function ZGETDCVRANG Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ZGETDCVRANG@4" (ByRef VMAX As Double) As Long
Public Declare Function ZGETPHPOS Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ZGETPHPOS@8" (ByRef NOWXP As Double, ByRef NOWYP As Double) As Long
Public Declare Function ZGETPHPOS2 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ZGETPHPOS2@8" (ByRef NOWXP As Double, ByRef NOWYP As Double) As Long
Public Declare Function ZGETSRVSIGNAL Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ZGETSRVSIGNAL@16" (ByRef x As Long, ByRef y As Long, ByRef z As Long, ByRef t As Long) As Long
''''''Public Declare Function ZGETTRMPOS Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ZGETTRMPOS@24" (ByRef TRIMX As Double, ByRef TRIMY As Double, ByRef RCX As Double, ByRef RCY As Double, ByRef SMAX As Double, ByRef SMAY As Double) As Long
Public Declare Function ZINPSTS Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ZINPSTS@8" (ByVal sw As Long, ByRef sts As Long) As Long
Public Declare Function ZLATCHOFF Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ZLATCHOFF@0" () As Long
Public Declare Function ZZMOVE Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ZMOVE@12" (ByVal z As Double, ByVal MD As Integer) As Long
Public Declare Function ZZMOVE2 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ZMOVE2@12" (ByVal z As Double, ByVal MD As Integer) As Long
Public Declare Function ZRANGTRIM Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ZRANGTRIM@32" (ByVal NOM As Double, ByVal RNG As Integer, ByVal l As Double, ByVal V As Double, ByVal ANG As Integer) As Long
Public Declare Function ZRCIRTRIM Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ZRCIRTRIM@44" (ByVal NOM As Double, ByVal RNG As Integer, ByVal V As Double, ByVal RADI As Double, ByVal ANG2 As Double, ByVal ANG As Double) As Long
Public Declare Function ZRTRIM2 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ZRTRIM2@32" (ByVal NOM As Double, ByVal RNG As Integer, ByVal l As Double, ByVal V As Double, ByVal ANG As Integer) As Long
Public Declare Function ZSELXYZSPD Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ZSELXYZSPD@4" (ByVal SPD As Long) As Long
Public Declare Function ZSETBPTIME Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ZSETBPTIME@8" (ByVal BPTIME As Long, ByVal EPTIME As Long) As Long
Public Declare Function ZSETPOS2 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ZSETPOS2@24" (ByVal POS2X As Double, ByVal POS2Y As Double, ByVal POS2Z As Double) As Long
Public Declare Function ZSETUCUT Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ZSETUCUT@40" (ByVal MD As Integer, ByVal RNO As Integer, ByVal Index As Integer, ByVal EL As Integer, ByVal RATIO As Double, ByVal LTP As Double, ByVal LTP2 As Double) As Long
Public Declare Function ZSLCOVERCLOSE Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ZSLCOVERCLOSE@4" (ByVal ZONOFF As Integer) As Long
Public Declare Function ZSLCOVEROPEN Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ZSLCOVEROPEN@4" (ByVal ZONOFF As Integer) As Long
Public Declare Function ZSTGXYMODE Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ZSTGXYMODE@4" (ByVal mode As Long) As Long
Public Declare Function ZSTOPSTS Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ZSTOPSTS@0" () As Long
Public Declare Function ZSTOPSTS2 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ZSTOPSTS2@0" () As Long
Public Declare Function ZSYSPARAM1 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ZSYSPARAM1@60" (ByVal POWERCYCLE As Integer, ByVal THETA As Integer, ByVal BPDIRXY As Integer, ByVal BPSIZE As Integer, ByVal DCSCANNER As Integer, ByVal DCVRANGE As Integer, ByVal LRANGE As Integer, ByVal LDPOSX As Double, ByVal LDPOSY As Double, ByVal FPSUP As Integer, ByVal DELAYSKIP As Integer, ByVal OSCILLATOR As Integer, ByVal MACHINETYPE As Integer) As Long
'Public Declare Function ZSYSPARAM2 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ZSYSPARAM2@60" (ByVal PRBTYP As Integer, ByVal SMINMAXZ2 As Double, ByVal ZPTIMEON As Integer, ByVal ZPTIMEOFF As Integer, ByVal XYTBL As Integer, ByVal SmaxX As Double, ByVal SmaxY As Double, ByVal ABSTIME As Long, ByVal TRIMX As Double, ByVal TRIMY As Double) As Long
Public Declare Function ZSYSPARAM2 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ZSYSPARAM2@68" (ByVal PRBTYP As Integer, ByVal SMINMAXZ2 As Double, ByVal ZPTIMEON As Integer, ByVal ZPTIMEOFF As Integer, ByVal XYTBL As Integer, ByVal SmaxX As Double, ByVal SmaxY As Double, ByVal ABSTIME As Long, ByVal TRIMX As Double, ByVal TRIMY As Double, ByVal BpMoveLimX As Integer, ByVal BpMoveLimY As Integer) As Long
'Public Declare Function ZSYSPARAM3 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ZSYSPARAM3@16" (ByVal ProcPower2 As Integer, ByVal GrvTime As Long, ByVal UcutType As Integer, ByVal ExtBit As Long) As Long
'Public Declare Function ZSYSPARAM3 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ZSYSPARAM3@20" (ByVal ProcPower2 As Integer, ByVal GrvTime As Long, ByVal UcutType As Integer, ByVal ExtBit As Long, ByVal PosSpd As Integer) As Long
Public Declare Function ZSYSPARAM3 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ZSYSPARAM3@24" (ByVal ProcPower2 As Integer, ByVal GrvTime As Long, ByVal UcutType As Integer, ByVal ExtBit As Long, ByVal PosSpd As Integer, ByVal BiasOn_AddTime As Integer) As Long
Public Declare Function ZSYSPARAM4 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ZSYSPARAM4@8" (ByRef pdbPRM As Any, ByRef pdwPRM As Any) As Long
Public Declare Function ZTIMERINIT Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ZTIMERINIT@0" () As Long
Public Declare Function ZVMEAS Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ZVMEAS@12" (ByVal mode As Integer, ByVal DVM As Integer, ByRef V As Double) As Long
Public Declare Function ZWAIT Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ZWAIT@4" (ByVal lngWaitMilliSec As Long) As Long
Public Declare Function ZZGETRTMODULEINFO Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ZZGETRTMODULEINFO@0" () As Long
Public Declare Function Z_INIT Lib "C:\TRIM\DllTrimFnc.dll" Alias "_Z_INIT@0" () As Long
' TeachOcx用
Public Declare Function TRIM_ST Lib "C:\TRIM\DllTrimFnc.dll" Alias "_TRIM_ST@4" (ByRef CutCmnPrm As Any) As Long
Public Declare Function TRIM_L Lib "C:\TRIM\DllTrimFnc.dll" Alias "_TRIM_L@4" (ByRef CutCmnPrm As Any) As Long
Public Declare Function TRIM_SC Lib "C:\TRIM\DllTrimFnc.dll" Alias "_TRIM_SC@4" (ByRef CutCmnPrm As Any) As Long
Public Declare Function TRIM_HkU Lib "C:\TRIM\DllTrimFnc.dll" Alias "_TRIM_HkU@4" (ByRef CutCmnPrm As Any) As Long
Public Declare Function TRIM_MK Lib "C:\TRIM\DllTrimFnc.dll" Alias "_TRIM_MK@56" (ByVal MKSTR As String, ByVal STX As Double, ByVal STY As Double, ByVal HIGH As Double, ByVal V As Double, ByVal ANG As Integer, ByVal QRATE As Double, ByVal cutCondNo As Integer, ByVal moveMode As Integer) As Long

'###805①(PROBE)
Public Declare Function TRIMBLOCKMEASURE Lib "C:\TRIM\DllTrimFnc.dll" Alias "_TRIMBLOCKMEASURE@20" (ByVal MD As Integer, ByVal HZ As Integer, ByVal RI As Integer, ByVal CI As Integer, ByVal NG As Integer) As Long
Public Declare Function TRIM_RESULT_Double Lib "C:\TRIM\DllTrimFnc.dll" Alias "_TRIM_RESULT@24" (ByVal KD As Integer, ByVal SN As Integer, ByVal NM As Integer, ByVal CI As Integer, ByVal DI As Integer, ByRef Res As Double) As Integer
'###805①(PROBE)

'###805③
Public Declare Function TRIMMEASURERESIST Lib "C:\TRIM\DllTrimFnc.dll" Alias "_TRIMMEASURERESIST@20" (ByVal MD As Integer, ByVal ResNo As Integer, ByVal RI As Integer, ByRef RangeNo As Integer, ByRef Result As Double) As Long
'###805③


' 新規追加コマンド（新トリマSL43xR向け）
Public Declare Function SYSTEM_RESET Lib "C:\TRIM\DllTrimFnc.dll" Alias "_SYSTEM_RESET@0" () As Long
Public Declare Function GET_ALLAXIS_STATUS Lib "C:\TRIM\DllTrimFnc.dll" Alias "_GET_ALLAXIS_STATUS@8" (ByRef err As Long, ByRef AllStatus As Long) As Integer
'Public Declare Function MONITOR_STARTSW Lib "C:\TRIM\DllTrimFnc.dll" Alias "_MONITOR_STARTSW@4" (ByRef AllStatus As Long) As Long
'Public Declare Function LAMP_CTRL Lib "C:\TRIM\DllTrimFnc.dll" Alias "_LAMP_CTRL@8" (ByVal LampNo As Integer, ByVal onOff As Boolean) As Integer
Public Declare Function LAMP_CTRL Lib "C:\TRIM\DllTrimFnc.dll" Alias "_LAMP_CTRL@8" (ByVal LampNo As Integer, ByVal onOff As Long) As Integer
Public Declare Function INTERLOCK_CHECK Lib "C:\TRIM\DllTrimFnc.dll" Alias "_INTERLOCK_CHECK@8" (ByRef InterlockSts As Integer, ByRef SwitchSts As Long) As Integer
Public Declare Function ORG_INTERLOCK_CHECK Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ORG_INTERLOCK_CHECK@8" (ByRef InterlockSts As Integer, ByRef SwitchSts As Long) As Integer
Public Declare Function SLIDECOVER_MOVINGCHK Lib "C:\TRIM\DllTrimFnc.dll" Alias "_SLIDECOVER_MOVINGCHK@12" (ByVal OpenCloseChk As Integer, ByVal UseReset As Integer, ByRef SwitchSts As Long) As Integer
Public Declare Function SLIDECOVER_OPENCHK Lib "C:\TRIM\DllTrimFnc.dll" Alias "_SLIDECOVER_OPENCHK@4" (ByRef slidecoverSts As Long) As Long
Public Declare Function SLIDECOVER_CLOSECHK Lib "C:\TRIM\DllTrimFnc.dll" Alias "_SLIDECOVER_CLOSECHK@4" (ByRef slidecoverSts As Long) As Long
Public Declare Function SLIDECOVER_GETSTS Lib "C:\TRIM\DllTrimFnc.dll" Alias "_SLIDECOVER_GETSTS@4" (ByRef slidecoverSts As Long) As Long
Public Declare Function CONSOLE_SWCHECK Lib "C:\TRIM\DllTrimFnc.dll" Alias "_CONSOLE_SWCHECK@8" (ByVal BbReleaseCheck As Boolean, ByRef SwitchChk As Long) As Long
Public Declare Function COVER_CHECK Lib "C:\TRIM\DllTrimFnc.dll" Alias "_COVER_CHECK@4" (ByRef coverSts As Integer) As Integer
Public Declare Function COVERLATCH_CLEAR Lib "C:\TRIM\DllTrimFnc.dll" Alias "_COVERLATCH_CLEAR@0" () As Integer
Public Declare Function COVERLATCH_CHECK Lib "C:\TRIM\DllTrimFnc.dll" Alias "_COVERLATCH_CHECK@4" (ByRef LatchSts As Long) As Integer
Public Declare Function EMGSTS_CHECK Lib "C:\TRIM\DllTrimFnc.dll" Alias "_EMGSTS_CHECK@4" (ByRef Status As Integer) As Long
Public Declare Function FLSET Lib "C:\TRIM\DllTrimFnc.dll" Alias "_FLSET@8" (ByVal MD As Integer, ByVal CNDNO As Integer) As Long
Public Declare Function START_SWWAIT Lib "C:\TRIM\DllTrimFnc.dll" Alias "_START_SWWAIT@4" (ByRef SwitchSts As Long) As Integer
Public Declare Function START_SWCHECK Lib "C:\TRIM\DllTrimFnc.dll" Alias "_START_SWCHECK@8" (ByVal bReleaseCheck As Boolean, ByRef SwitchSts As Long) As Integer
Public Declare Function HALT_SWCHECK Lib "C:\TRIM\DllTrimFnc.dll" Alias "_HALT_SWCHECK@4" (ByRef SwitchSts As Long) As Integer
Public Declare Function STARTRESET_SWWAIT Lib "C:\TRIM\DllTrimFnc.dll" Alias "_STARTRESET_SWWAIT@4" (ByRef SwitchSts As Long) As Integer
Public Declare Function ORG_STARTRESET_SWWAIT Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ORG_STARTRESET_SWWAIT@4" (ByRef SwitchSts As Long) As Integer
Public Declare Function STARTRESET_SWCHECK Lib "C:\TRIM\DllTrimFnc.dll" Alias "_STARTRESET_SWCHECK@8" (ByVal bReleaseCheck As Boolean, ByRef SwitchSts As Long) As Integer
Public Declare Function SET_FL_ERRLOG Lib "C:\TRIM\DllTrimFnc.dll" Alias "_SET_FL_ERRLOG@4" (ByRef ErrCode As Long) As Long
Public Declare Function Z_SWCHECK Lib "C:\TRIM\DllTrimFnc.dll" Alias "_Z_SWCHECK@4" (ByRef SwitchChk As Long) As Long
Public Declare Function ISALIVE_INTIME Lib "C:\TRIM\DllTrimFnc.dll" Alias "_ISALIVE_INTIME@0" () As Long
Public Declare Function TERMINATE_INTIME Lib "C:\TRIM\DllTrimFnc.dll" Alias "_TERMINATE_INTIME@0" () As Long
Public Declare Function CLAMP_CTRL Lib "C:\TRIM\DllTrimFnc.dll" Alias "_CLAMP_CTRL@8" (ByVal XOnOff As Integer, ByVal YOnOff As Integer) As Long
'Public Declare Function CLAMP_GETSTS Lib "C:\TRIM\DllTrimFnc.dll" Alias "_CLAMP_GETSTS@8" (ByVal XOnOff As Integer, ByVal YOnOff As Integer) As Long
'Public Declare Function CLAMP_GETSTS Lib "C:\TRIM\DllTrimFnc.dll" Alias "_CLAMP_GETSTS@8" (ByRef XOnOff As Integer, ByRef YOnOff As Integer) As Long
Public Declare Function CLAMP_GETSTS Lib "C:\TRIM\DllTrimFnc.dll" Alias "_CLAMP_GETSTS@8" (ByRef XOnOff As Long, ByRef YOnOff As Long) As Long
Public Declare Function PMON_SHUTCTRL Lib "C:\TRIM\DllTrimFnc.dll" Alias "_PMON_SHUTCTRL@4" (ByVal onOff As Long) As Long
Public Declare Function PMON_MEASCTRL Lib "C:\TRIM\DllTrimFnc.dll" Alias "_PMON_MEASCTRL@4" (ByVal measMode As Long) As Long
Public Declare Function SIGNALTOWER_CTRLEX Lib "C:\TRIM\DllTrimFnc.dll" Alias "_SIGNALTOWER_CTRLEX@8" (ByVal OnBit As Long, ByVal OffBit As Long) As Long
Public Declare Function SET_THETA_CORRECT Lib "C:\TRIM\DllTrimFnc.dll" Alias "_SET_THETA_CORRECT@16" (ByVal XPos As Double, ByVal YPos As Double) As Long
Public Declare Function SET_THETA_CORRECT_ANGLE Lib "C:\TRIM\DllTrimFnc.dll" Alias "_SET_THETA_CORRECT_ANGLE@8" (ByVal Angle As Double) As Long
Public Declare Function LASERONESHOT Lib "C:\TRIM\DllTrimFnc.dll" Alias "_LASERONESHOT@4" (ByVal dotcnt As Integer) As Integer
Public Declare Function SetFixAttInfo Lib "C:\TRIM\DllTrimFnc.dll" Alias "_SetFixAttInfo@4" (ByRef FixAttAry As Any) As Long
Public Declare Function SetFixAttOneInfo Lib "C:\TRIM\DllTrimFnc.dll" Alias "_SetFixAttOneInfo@8" (ByVal CondNum As Integer, ByVal FixAtt As Integer) As Long

'Public Declare Function TRIM_ES Lib "C:\TRIM\DllTrimFnc.dll" Alias "_TRIM_ES@4" (ByRef CutCmnPrm As CUT_COMMON_PRM) As Long
'Public Declare Function TRIM_IX Lib "C:\TRIM\DllTrimFnc.dll" Alias "_TRIM_IX@4" (ByRef CutCmnPrm As CUT_COMMON_PRM) As Long
Public Declare Function TRIM_ES Lib "C:\TRIM\DllTrimFnc.dll" Alias "_TRIM_ES@4" (ByRef CutCmnPrm As Any) As Long
Public Declare Function TRIM_IX Lib "C:\TRIM\DllTrimFnc.dll" Alias "_TRIM_IX@4" (ByRef CutCmnPrm As Any) As Long
Public Declare Function SERVO_POWER Lib "C:\TRIM\DllTrimFnc.dll" Alias "_SERVO_POWER@16" (ByVal XAxisOnOff As Long, ByVal YAxisOnOff As Long, ByVal ZAxisOnOff As Long, ByVal TAxisOnOff As Long) As Long
Public Declare Function AXIS_X_INIT Lib "C:\TRIM\DllTrimFnc.dll" Alias "_AXIS_X_INIT@0" () As Long
Public Declare Function AXIS_Y_INIT Lib "C:\TRIM\DllTrimFnc.dll" Alias "_AXIS_Y_INIT@0" () As Long
Public Declare Function AXIS_Z_INIT Lib "C:\TRIM\DllTrimFnc.dll" Alias "_AXIS_Z_INIT@0" () As Long
Public Declare Function GET_QRATE Lib "C:\TRIM\DllTrimFnc.dll" Alias "_GET_QRATE@4" (ByRef QRATE As Double) As Long
Public Declare Function BP_GET_CALIBDATA Lib "C:\TRIM\DllTrimFnc.dll" Alias "_BP_GET_CALIBDATA@16" (ByRef GainX As Double, ByRef GainY As Double, ByRef OfsX As Double, ByRef OfsY As Double) As Long
''###812④
Public Declare Function TRIMDATA_GPIB2 Lib "C:\TRIM\DllTrimFnc.dll" Alias "_TRIMDATA_GPIB2@8" (ByRef pGpibDat As Any, ByVal tkyKnd As Integer) As Long

