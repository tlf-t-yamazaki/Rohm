'===============================================================================
'   Description  : 印刷処理(ローム殿特注) V1.18.0.0③
'
'   Copyright(C) : OMRON LASERFRONT INC. 2014
'
'===============================================================================
Imports System.IO                       'V4.4.0.0-0
Imports System.Text                     'V4.4.0.0-0
Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8①
Imports TKY_ALL_SL432HW.My.Resources    'V4.4.0.0-0

Module QR_Print
#Region "【定数/変数の定義】"
    '===========================================================================
    '   定数/変数の定義
    '===========================================================================
    '----- クラスオブジェクト -----
    Private ObjPrint As DllPrintString.PrintString = Nothing            ' 印刷用オブジェクト(DllPrintString.dll)
    'Private strFont As String = "ＭＳ Ｐゴシック"                     ' 印刷用フォント 
    'Private strFont As String = "MS Gothic"                             ' 印刷用フォント 
    Private strFont As String = QR_Print_001                             ' 印刷用フォント 
    Private iFontSz As Integer = 10                                     ' フォントサイズ

    '----- 印刷位置 -----
    Private Const KEISEN_MAX As Integer = 5                             ' 罫線数("---------------")
    Private Const ITEM_MAX As Integer = 49 + KEISEN_MAX                 ' 印刷項目数
    Private Const LINE_SZ As Integer = 24                               ' 1行の文字数
    Private Const HED_SZ As Integer = 13                                ' 見出し文字数
    Private Const HED_S2 As Integer = 11                                ' 見出し文字数2
    Private Const HED_S3 As Integer = 7                                 ' 見出し文字数3(Lot No.用)
    Private Const PRT_SZ As Integer = 11                                ' 印刷文字数
    Private Const PRT_S2 As Integer = 6                                 ' 印刷文字数2
    Private Const PRT_S3 As Integer = 7                                 ' 印刷文字数3
    Private Const PRT_LOT As Integer = 17                               ' 印刷文字数4(Lot No.用)
    Private Const HEAD_POSX As Integer = 1                              ' 見出し印刷座標X
    Private Const HEAD_POSY As Integer = 5                              ' 見出し印刷座標Y

    '---------------------------------------------------------------------------
    '   トリミング結果印刷用構造体(ローム殿用) 
    '---------------------------------------------------------------------------
    Public Structure TRIM_RESULT_PRINT_ROHM_INFO
        Dim bAutoMode As Boolean                                        ' 自動/手動     (False=手動運転, True=自動運転)
        Dim bFlgPrint As Boolean                                        ' 印刷済みフラグ(False=印刷未  , True=印刷済)

        Dim MachineNo As String                                         '  1.装置番号(10桁 "SL436Rxxxx") 
        '                                                               '  2.空白行
        '                                                               '  3.Lot Result 
        Dim Start_day As String                                         '  4.開始日(2014/Feb/20)
        Dim Start_time As String                                        '  5.開始時間(hh:mm:ss)
        Dim End_day As String                                           '  6.終了日(2014/Feb/20)
        Dim End_time As String                                          '  7.終了時間(hh:mm:ss)
        Dim Prod_time As String                                         '  8.開始～終了までに要した時間(hh:mm:ss)
        '                                                               '  9.空白行
        Dim Lot_No As String                                            ' 10.QRコード内のロット番号
        Dim Prot_Typ As String                                          ' 11.品種(QRコードのタイプを設定する)
        Dim IEC_Tol As String                                           ' 12.目標抵抗値(999.999)
        Dim Cal_RV As String                                            ' 13.目標抵抗値(カットオフを考慮した目標値)
        Dim LaserPower As String                                        ' 14.加工面出力(9.99W)
        Dim Stop1 As String                                             ' 15.Lターンポイント(%)
        Dim Stop2 As String                                             ' 16.トリミング完了ポイント(%) (カットオフ(%))
        Dim Speed1 As String                                            ' 17.加工速度1(Lターン前)(9.9mm/s)
        Dim Speed2 As String                                            ' 18.加工速度2(Lターン後)(9.9mm/s)
        Dim Qrate1 As String                                            ' 19.Qレート1(Lターン前) (9.9kHz)
        Dim Qrate2 As String                                            ' 20.Qレート2(Lターン後) (9.9kHz)
        Dim GlvPosX As String                                           ' 21.ガルバノスタートポイント(カット位置XY)
        Dim GlvPosY As String                                           ' 
        Dim CompOffset As String                                        ' 22.カットオフ(※内容不明のため未対応←カットオフ(%)を設定)
        '                                                               ' 23.空白行
        Dim sTrimRate As String                                         ' 24.平均切上率(平均抵抗値/目標抵抗値)
        Dim TrimRate As Double                                          '    平均切上率(平均抵抗値/目標抵抗値)
        Dim MTBF As String                                              ' 25.エラー発生間隔(平均故障間隔)
        Dim MTTR As String                                              ' 26.エラー発生後の復旧時間(平均復旧時間)
        Dim sTol_Sheet As String                                        ' 27.トータル枚数(装置に投入された枚数)
        Dim Tol_Sheet As Integer                                        '    トータル枚数(装置に投入された枚数) ※m_lPlateCount(プレート処理数)を設定
        Dim NG_Cancel As Integer                                        ' 28.NGキャンセル数(※設定されるケースがないため未対応)
        Dim sJudgeValue As String                                       ' 29.不具合発生率(全NG数/トリミング数)
        Dim JudgeValue As Double                                        '    不具合発生率(全NG数/トリミング数)
        Dim OffsetX As String                                           ' 30.オフセット(BPオフセットXY)
        Dim OffsetY As String                                           ' 
        Dim sProd_Count As String                                       ' 31.トリミングチップ数
        Dim Prod_Count As Integer                                       '    トリミングチップ数(m_lGoodCount(良品抵抗数) + m_lNgCount(不良抵抗数))
        Dim sTol_NG_Sheet As String                                     ' 32.総NG基板枚数
        Dim Tol_NG_Sheet As Integer                                     '    総NG基板枚数
        '                                                               ' 33.空白行
        '                                                               ' 34.PCS (%)
        Dim sPretest_Hi_Fail As String                                  ' 35.初期値上限不良のﾁｯﾌﾟ数
        Dim Pretest_Hi_Fail As Integer                                  '    初期値上限不良のﾁｯﾌﾟ数   (m_lITHINGCount(IT HI NG数)を設定)
        Dim sPretest_Lo_Fail As String                                  ' 36.初期値下限不良のﾁｯﾌﾟ数
        Dim Pretest_Lo_Fail As Integer                                  '    初期値下限不良のﾁｯﾌﾟ数   (m_lITLONGCount(IIT LO NG数)を設定)
        Dim sPretest_Open As String                                     ' 37.初期値ｵｰﾌﾟﾝ不良のﾁｯﾌﾟ数
        Dim Pretest_Open As Integer                                     '    初期値ｵｰﾌﾟﾝ不良のﾁｯﾌﾟ数   (m_lITOVERCount(ITｵｰﾊﾞｰﾚﾝｼﾞ数)を設定)
        Dim sFinal_test_Hi_Fail As String                               ' 38.ﾄﾘﾐﾝｸﾞ後の上限不良のﾁｯﾌﾟ数
        Dim Final_test_Hi_Fail As Integer                               '    ﾄﾘﾐﾝｸﾞ後の上限不良のﾁｯﾌﾟ数(m_lFTHINGCount(FT HI NG数)を設定)
        Dim sFinal_test_Lo_Fail As String                               ' 39.ﾄﾘﾐﾝｸﾞ後の下限不良のﾁｯﾌﾟ数
        Dim Final_test_Lo_Fail As Integer                               '    ﾄﾘﾐﾝｸﾞ後の下限不良のﾁｯﾌﾟ数(m_lFTLONGCount(FT LO NG数)を設定)
        '                                                               ' 40.空白行
        Dim sTrim_OK As String                                          ' 41.良品ﾁｯﾌﾟ数 + %(トリミングチップ数との比率)
        Dim Trim_OK As Integer                                          '    良品ﾁｯﾌﾟ数 (m_lITHINGCount(IT HI NG数)を設定)
        Dim sInit_OK As String                                          ' 42.イニシャル良品ﾁｯﾌﾟ数 + %(トリミングチップ数との比率)
        Dim Init_OK As Integer                                          '    イニシャル良品ﾁｯﾌﾟ数(トリミングチップ数 - (IT NGﾁｯﾌﾟ数 + FT NGﾁｯﾌﾟ数))
        Dim Trim_NG As Integer                                          '    不良品ﾁｯﾌﾟ数(ワーク)
        Dim sTotal_OK As String                                         ' 43.総良品ﾁｯﾌﾟ数(良品ﾁｯﾌﾟ数と同じ値)
        Dim Total_OK As Integer                                         '    総良品ﾁｯﾌﾟ数(良品ﾁｯﾌﾟ数と同じ値)
        '
        Dim sInt_RV_NG As String                                        ' 44.初期値上限不良のﾁｯﾌﾟ数 + 初期値下限不良のﾁｯﾌﾟ数
        Dim Int_RV_NG As Integer                                        '    初期値上限不良のﾁｯﾌﾟ数 + 初期値下限不良のﾁｯﾌﾟ数
        Dim sAT_Trim_NG As String                                       ' 45.全NGチップ数
        Dim AT_Trim_NG As Integer                                       '    全NGチップ数
        '                                                               ' 46.空白行
        Dim sAlmCnt As String                                           ' 47.アラーム発生回数
        Dim AlmCnt As Integer                                           '    アラーム発生回数
        '                                                               ' 48.空白行
        '                                                               ' 49.アラーム発生時間/エラー内容
        Dim AlarmST_time As String                                      '    アラーム発生時間("hh:mm:ss")

        '----- ワーク域↓ -----
        Dim STtime As Double                                            ' 開始時間(double)
        Dim EDtime As Double                                            ' 終了時間(double)
        Dim AlmSTtime As Double                                         ' ｱﾗｰﾑ停止開始時間(double)
        Dim AlmEDtime As Double                                         ' ｱﾗｰﾑ停止終了時間(double)
        Dim AlmTotaltime As Double                                      ' ｱﾗｰﾑ停止ﾄｰﾀﾙ時間(double)

        Dim Trim_TotalVal As Double                                     ' ﾄﾘﾐﾝｸﾞされたﾁｯﾌﾟの抵抗値(合計)
        Dim Trim_TotalValCnt As Double                                  ' ﾄﾘﾐﾝｸﾞされたﾁｯﾌﾟの抵抗値(計算用合計)
        Dim Trim_TotalValKT As Short                                    ' ﾄﾘﾐﾝｸﾞされたﾁｯﾌﾟの抵抗値(桁)
        Dim Pdt_Sheet As Integer                                        ' ﾄﾘﾐﾝｸﾞｽﾃｰｼﾞで処理した基板枚数
        Dim Edg_Fail As Integer                                         ' ﾛｯﾄ中の認識不良基板枚数
        '----- ワーク域↑ -----
    End Structure

    Public stPRT_ROHM As TRIM_RESULT_PRINT_ROHM_INFO                    ' トリミング結果印刷用構造体(ローム殿用)
    Public gbAlarmFlg As Boolean = False                                ' アラーム発生フラグ(False=アラーム停止, True =アラーム発生)
    Public gFPATH_QR_ALARM As String = "C:\TRIMDATA\LOG\qr_alarm.txt"   ' アラームファイル名

#End Region

#Region "【メソッド定義】"
    '---------------------------------------------------------------------------
    '   印刷用DLL呼び出し
    '---------------------------------------------------------------------------
#Region "印刷初期処理"
    '''=========================================================================
    ''' <summary>印刷初期処理</summary>
    ''' <returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Public Function Print_Init() As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' ローム殿仕様でなければNOP
            If (gSysPrm.stCTM.giSPECIAL <> customROHM) Then
                Return (cFRS_NORMAL)
            End If

            ' 印刷初期設定処理
            ObjPrint = New DllPrintString.PrintString                   ' 印刷用オブジェクト生成
            r = ObjPrint.Print_Init(strFont, iFontSz)                   ' 初期化処理
            Return (r)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "QR_Print.Print_Init() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "印刷終了処理"
    '''=========================================================================
    ''' <summary>印刷終了処理</summary>
    ''' <returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Public Function Print_End() As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' ローム殿仕様でなければNOP
            If (gSysPrm.stCTM.giSPECIAL <> customROHM) Then
                Return (cFRS_NORMAL)
            End If

            ' 印刷終了処理
            If (ObjPrint Is Nothing) Then                               ' 印刷用オブジェクト未生成ならNOP
                Return (cFRS_NORMAL)
            End If
            r = ObjPrint.Print_End()                                    ' 印刷用オブジェクト開放  
            Return (r)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "QR_Print.Print_End() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "印刷処理"
    '''=========================================================================
    ''' <summary>印刷処理</summary>
    ''' <param name="strDAT">(INP)印刷文字列</param>
    ''' <param name="strHEAD"></param>
    ''' <param name="X"></param>
    ''' <param name="Y"></param>
    ''' <returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Public Function Print_Data(ByVal strDAT As String, ByVal strHEAD As String, ByVal X As Integer, ByVal Y As Integer) As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' ローム殿仕様でなければNOP
            If (gSysPrm.stCTM.giSPECIAL <> customROHM) Then
                Return (cFRS_NORMAL)
            End If
            ' 印刷用オブジェクト未生成ならNOP
            If (ObjPrint Is Nothing) Then
                Return (cFRS_NORMAL)
            End If

            ' 印刷処理
            r = ObjPrint.Print_Data(strDAT, strHEAD, X, Y)
            Return (r)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "QR_Print.Print_Data() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region
    '---------------------------------------------------------------------------
    '   印刷用項目集計処理
    '---------------------------------------------------------------------------
#Region "トリミング結果印刷項目初期化"
    '''=========================================================================
    ''' <summary>トリミング結果印刷項目初期化</summary>
    ''' <remarks>初期化は下記からCallされる
    ''' 　　　 ・QRデータ受信時
    ''' 　　　 ・CLRボタン押下時(IDLE状態のメイン画面, 一時停止画面)
    ''' 　　　 ・データロードコマンド時(手動)
    ''' </remarks>
    '''=========================================================================
    Public Sub ClrTrimPrnData()

        Dim strMSG As String

        Try
            ' 印刷項目初期化
            If (gSysPrm.stCTM.giSPECIAL <> customROHM) Then Return '    ' ローム殿特注でなければNOP
            If (giAppMode = APP_MODE_TRIM) Then Return

            gbFgContinue = False                                        ' 自動運転継続フラグOFF   

            With stPRT_ROHM
                .bAutoMode = False                                      ' False = 手動運転
                .bFlgPrint = False                                      ' False = 印刷未

                .MachineNo = ""                                         '  1.装置番号
                '                                                       '  2.空白行
                '                                                       '  3.Lot Result
                .Start_day = ""                                         '  4.開始日
                .Start_time = ""                                        '  5.開始時間
                .End_day = ""                                           '  6.終了日
                .End_time = ""                                          '  7.終了時間
                .Prod_time = ""                                         '  8.開始～終了までに要した時間
                '                                                       '  9.空白行
                .Lot_No = ""                                            ' 10.QRコード内のロット番号
                .Prot_Typ = ""                                          ' 11.品種(QRコードのタイプを設定する)
                .IEC_Tol = ""                                           ' 12.目標抵抗値
                .Cal_RV = ""                                            ' 13.目標抵抗値(カットオフを考慮した目標値)
                .LaserPower = ""                                        ' 14.加工面出力
                .Stop1 = ""                                             ' 15.Lターンポイント(%)
                .Stop2 = ""                                             ' 16.カットオフ(%)
                .Speed1 = ""                                            ' 17.加工速度1(Lターン前)
                .Speed2 = ""                                            ' 18.加工速度2(Lターン後)
                .Qrate1 = ""                                            ' 19.Qレート1(Lターン前)
                .Qrate2 = ""                                            ' 20.Qレート2(Lターン後)
                .GlvPosX = ""                                           ' 21.ガルバノスタートポイント(カット位置XY)
                .GlvPosY = ""
                '                                                       ' 22.カットオフ(※内容不明のため未対応)
                '                                                       ' 23.空白行
                .TrimRate = 0.0#                                        ' 24.平均切上率
                .MTBF = ""                                              ' 25.エラー発生間隔(平均故障間隔)
                .MTTR = ""                                              ' 26.エラー発生後の復旧時間(平均復旧時間)
                .Tol_Sheet = 0                                          ' 27.トータル枚数(装置に投入された枚数)
                '                                                       ' 28.NGキャンセル数(※設定されるケースがないため未対応)
                .JudgeValue = 0.0#                                      ' 29.不具合発生率(全NG数/トリミング数)
                .OffsetX = ""                                           ' 30.オフセット(BPオフセットXY)
                .OffsetY = ""
                .Prod_Count = 0                                         ' 31.トリミングチップ数
                .Tol_NG_Sheet = 0                                       ' 32.総NG基板枚数
                '                                                       ' 33.空白行
                '                                                       ' 34.PCS (%)
                .Pretest_Hi_Fail = 0                                    ' 35.初期値上限不良のﾁｯﾌﾟ数
                .Pretest_Lo_Fail = 0                                    ' 36.初期値下限不良のﾁｯﾌﾟ数
                .Pretest_Open = 0                                       ' 37.初期値ｵｰﾌﾟﾝ不良のﾁｯﾌﾟ数
                .Final_test_Hi_Fail = 0                                 ' 38.ﾄﾘﾐﾝｸﾞ後の上限不良のﾁｯﾌﾟ数
                .Final_test_Lo_Fail = 0                                 ' 39.ﾄﾘﾐﾝｸﾞ後の下限不良のﾁｯﾌﾟ数
                '                                                       ' 40.空白行
                .Trim_OK = 0                                            ' 41.良品ﾁｯﾌﾟ数
                .Init_OK = 0                                            ' 42.イニシャル良品ﾁｯﾌﾟ数
                .Total_OK = 0                                           ' 43.総良品ﾁｯﾌﾟ数
                .Int_RV_NG = 0                                          ' 44.イニシャルNGチップ数 + ファイナルNGチップ数
                .AT_Trim_NG = 0                                         ' 45.全NGチップ数
                '                                                       ' 46.空白行
                .AlmCnt = 0                                             ' 47.アラーム発生回数
                '                                                       ' 48.空白行
                '                                                       ' 49.アラーム発生時間/エラー内容
                .AlarmST_time = String.Empty                            '    アラーム発生時間("hh:mm:ss")

                ' ワーク域初期化
                .STtime = 0.0#                                          ' 開始時間(double)
                .EDtime = 0.0#                                          ' 終了時間(double)
                .AlmSTtime = 0.0#                                       ' ｱﾗｰﾑ停止開始時間(double)
                .AlmEDtime = 0.0#                                       ' ｱﾗｰﾑ停止終了時間(double)
                .AlmTotaltime = 0.0#                                    ' ｱﾗｰﾑ停止ﾄｰﾀﾙ時間(double)

                .Trim_TotalVal = 0.0#                                   ' ﾄﾘﾐﾝｸﾞされたﾁｯﾌﾟの抵抗値(合計)
                .Trim_TotalValCnt = 0.0#                                ' ﾄﾘﾐﾝｸﾞされたﾁｯﾌﾟの抵抗値(計算用合計)
                .Trim_TotalValKT = 0                                    ' ﾄﾘﾐﾝｸﾞされたﾁｯﾌﾟの抵抗値(桁)
            End With

            ' 印刷用テキストボックス(TxtBoxPrint(非表示))クリア
            Form1.TxtBoxPrint.Text = ""

            ' 印刷用アラームファイルを削除する
            Call DeleteAlarmData(gFPATH_QR_ALARM)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "QR_Print.ClrTrimPrnData() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "トリミング結果印刷項目のデータをセットする(印刷前に設定可能なもの)"
    '''=========================================================================
    ''' <summary>トリミング結果印刷項目のデータをセットする</summary>
    ''' <remarks>印刷前に設定可能なもののみ、ここでセットする</remarks>
    '''=========================================================================
    Public Sub SetTrimPrnData()

        Dim TotalTime As Double = 0                                     ' 所要時間(sec)
        Dim UpTime As Double = 0                                        ' 24時間考慮(sec)
        Dim OpeTime As Double = 0                                       ' 稼動時間(sec)
        Dim KosyoIntv As Double = 0                                     ' 平均故障間隔(sec)
        Dim RecovTime As Double = 0                                     ' 平均復旧時間(sec)
        Dim iWK As Integer = 0
        Dim dblWK As Double

        Dim strMSG As String

        Try
            If (gSysPrm.stCTM.giSPECIAL <> customROHM) Then Return '    ' ローム殿特注でなければNOP
            With stPRT_ROHM
                '  1.MachineNo 装置番号
                strMSG = LaserFront.Trimmer.DefWin32Fnc.GetPrivateProfileString_S("TMENU", "DEVNUM", "C:\TRIM\tky.ini", "SL436R")
                .MachineNo = strMSG.PadLeft(HED_S2)
                '  2.空白行
                '  3.Lot Result
                '  4.Start_day 開始日 設定済み
                '  5.Start_time 開始時間 設定済み
                '  6.End_day 終了日 設定済み
                '  7.End_time 終了時間 設定済み
                '  8.Prod_time 開始～終了までに要した時間
                If (.STtime > .EDtime) Then
                    UpTime = Double.Parse(24 * 60 * 60)                 ' Uptime = 1日の秒数 
                End If
                TotalTime = (.EDtime + UpTime) - .STtime                ' 所要時間(sec) = 終了時間－開始時間
                Call Cnv_SecToHHMMSS(TotalTime, .Prod_time)             ' .Prod_time = "00:00:00"
                '  9.空白行
                ' 10.Lot_No QRコード内のロット番号
                .Lot_No = gsQRInfo(4)
                .Lot_No = .Lot_No.PadLeft(PRT_LOT)

                ' 11.Prot_Typ 品種
                .Prot_Typ = gsQRInfo(0)                                 ' タイプ
                .Prot_Typ = .Prot_Typ.PadLeft(PRT_SZ)

                ' 12.IEC_Tol 目標抵抗値(第1抵抗の目標値)
                Call Cnv_TergetVal(typResistorInfoArray(1).dblTrimTargetVal, .IEC_Tol)
                .IEC_Tol = .IEC_Tol.PadLeft(PRT_SZ)

                ' 13.Cal_RV 目標抵抗値(第1抵抗第１カットのカットオフを考慮した目標値) 
                ' カットオフ値 =  目標抵抗値 * (1 + Cut off(%)/100)
                dblWK = 1 + typResistorInfoArray(1).ArrCut(1).dblCutOff * 0.01
                dblWK = typResistorInfoArray(1).dblTrimTargetVal * dblWK
                Call Cnv_TergetVal(dblWK, .Cal_RV)
                .Cal_RV = .Cal_RV.PadLeft(PRT_SZ - 1)

                ' 14.LaserPower 加工面出力
                If (.LaserPower = "") Then
                    .LaserPower = "---"
                End If
                .LaserPower = .LaserPower.PadLeft(PRT_SZ)

                ' 15.Stop1 Lターンポイント(%)(第1抵抗第１カットのLターンポイント) 
                If (typResistorInfoArray(1).ArrCut(1).strCutType = CNS_CUTP_NL) Or (typResistorInfoArray(1).ArrCut(1).strCutType = CNS_CUTP_NLr) Or _
                   (typResistorInfoArray(1).ArrCut(1).strCutType = CNS_CUTP_NLt) Then
                    .Stop1 = typResistorInfoArray(1).ArrCut(1).dblLTurnPoint.ToString("0.00") + "%"
                Else
                    .Stop1 = "---"
                End If
                .Stop1 = .Stop1.PadLeft(PRT_SZ)

                ' 16.Stop2 カットオフ(%)
                .Stop2 = typResistorInfoArray(1).ArrCut(1).dblCutOff.ToString("0.00") + "%"
                .Stop2 = .Stop2.PadLeft(PRT_SZ)

                ' 17.Speed1 加工速度1(Lターン前)
                .Speed1 = typResistorInfoArray(1).ArrCut(1).dblCutSpeed.ToString("0.0") + "mm/s"
                .Speed1 = .Speed1.PadLeft(PRT_SZ)

                ' 18.Speed2 加工速度1(Lターン後)
                Call Cnv_Spd2_Qrate2(0, .Speed2)
                .Speed2 = .Speed2.PadLeft(PRT_SZ)

                ' 19.Qrate1 Qレート1(Lターン前)
                iWK = typResistorInfoArray(1).ArrCut(1).CndNum(CUT_CND_L1)
                .Qrate1 = stCND.Freq(iWK).ToString("0.0") + "kHz"
                '----- V6.0.3.0_45 ↓ -----
                If (gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Then      ' FLでない ?
                    .Qrate1 = typResistorInfoArray(1).ArrCut(1).dblQRate.ToString("0.0") + "kHz"
                End If
                '----- V6.0.3.0_45 ↑ -----
                .Qrate1 = .Qrate1.PadLeft(PRT_SZ)

                ' 20.Qrate2 Qレート2(Lターン後)
                Call Cnv_Spd2_Qrate2(1, .Qrate2)
                .Qrate2 = .Qrate2.PadLeft(PRT_SZ)

                ' 21.GlvPosX,Y ガルバノスタートポイント(カット位置XY)
                .GlvPosX = typResistorInfoArray(1).ArrCut(1).dblStartPointX.ToString("0.000")
                .GlvPosX = .GlvPosX.PadLeft(6)
                .GlvPosY = typResistorInfoArray(1).ArrCut(1).dblStartPointY.ToString("0.000")
                .GlvPosY = .GlvPosY.PadLeft(6)

                ' 22.CompOffset カットオフ(※内容不明のため未対応←カットオフ(%)を設定))
                .CompOffset = typResistorInfoArray(1).ArrCut(1).dblCutOff.ToString("0.00") + "%"
                .CompOffset = .CompOffset.PadLeft(PRT_SZ)

                ' 23.空白行

                ' 24.TrimRate 平均切上率(平均抵抗値/目標抵抗値)
                If (.Trim_TotalValCnt <> 0) Then                        ' Mean_Value = ﾄﾘﾐﾝｸﾞされたﾁｯﾌﾟの平均抵抗値
                Else
                    .Trim_TotalValCnt = 1
                End If
                dblWK = (.Trim_TotalVal / .Trim_TotalValCnt) * .Trim_TotalValKT
                .TrimRate = (dblWK / typResistorInfoArray(1).dblTrimTargetVal) * 100
                .sTrimRate = (.TrimRate.ToString("0.00") + "%").PadLeft(PRT_SZ)
                '----- V6.0.3.0_26↓ -----
                .TrimRate = GetAverageFTValue()
                .TrimRate = .TrimRate / typResistorInfoArray(1).dblTrimTargetVal * 100.0
                .sTrimRate = (.TrimRate.ToString("0.00") + "%").PadLeft(PRT_SZ)
                '----- V6.0.3.0_26↑ -----

                ' 25.MTBF エラー発生間隔(平均故障間隔 = 稼動時間 / ｱﾗｰﾑ発生回数)
                OpeTime = TotalTime - .AlmTotaltime                     ' 稼動時間 = 所要時間－ｱﾗｰﾑ停止時間
                'V4.7.0.0①↓
                If (gSysPrm.stCTM.giSPECIAL = customROHM) Then          ' ローム殿特注(SL436R/SL436S) ?
                    KosyoIntv = OpeTime / (.AlmCnt + 1)                 ' 平均故障間隔 = 稼動時間 / ｱﾗｰﾑ発生回数
                Else
                    'V4.7.0.0①↑
                    If (.AlmCnt <> 0) Then
                        KosyoIntv = OpeTime / .AlmCnt                   ' 平均故障間隔 = 稼動時間 / ｱﾗｰﾑ発生回数
                    Else
                        KosyoIntv = 0
                    End If
                End If                                                  ' V4.7.0.0①
                Call Cnv_SecToHHMMSS(KosyoIntv, .MTBF)                  ' MTBF = "00:00:00"
                .MTBF = .MTBF.PadLeft(PRT_SZ)

                ' 26.MTTR エラー発生後の復旧時間(平均復旧時間 = ｱﾗｰﾑ停止ﾄｰﾀﾙ時間 / ｱﾗｰﾑ発生回数)
                If (.AlmCnt <> 0) Then
                    RecovTime = .AlmTotaltime / .AlmCnt                 ' 平均復旧時間 = ｱﾗｰﾑ停止ﾄｰﾀﾙ時間 / ｱﾗｰﾑ発生回数
                Else
                    RecovTime = 0
                End If
                Call Cnv_SecToHHMMSS(RecovTime, .MTTR)                  ' MTTR = "00:00:00"
                .MTTR = .MTTR.PadLeft(PRT_SZ)

                ' 27.Tol_Sheet トータル枚数
                .Tol_Sheet = m_lPlateCount                              ' トータル枚数 = プレート処理数
                .sTol_Sheet = (.Tol_Sheet.ToString("0")).PadLeft(PRT_SZ)

                ' 28.NGキャンセル数(※設定されるケースがないため未対応)

                ' 29.JudgeValue 不具合発生率(全NG数/トリミング数)
                .sJudgeValue = (.JudgeValue.ToString("0.00") + "%").PadLeft(PRT_SZ)

                ' 30.OffsetX,Y オフセット(BPオフセットXY)
                .OffsetX = typPlateInfo.dblBpOffSetXDir.ToString("0.000")
                .OffsetX = .OffsetX.PadLeft(6)
                .OffsetY = typPlateInfo.dblBpOffSetYDir.ToString("0.000")
                .OffsetY = .OffsetY.PadLeft(6)

                ' 31.Prod_Count トリミングチップ数
                .Prod_Count = m_lGoodCount + m_lNgCount                 ' チップ数(m_lGoodCount(良品抵抗数) + m_lNgCount(不良抵抗数))
                .sProd_Count = (.Prod_Count.ToString("0")).PadLeft(PRT_SZ)

                ' 32.Tol_NG_Sheet 総NG基板枚数
                .sTol_NG_Sheet = (.Tol_NG_Sheet.ToString("0")).PadLeft(PRT_SZ)

                ' 33.空白行
                ' 34.PCS (%)

                ' 35.Pretest_Hi_Fail 初期値上限不良のﾁｯﾌﾟ数 + %
                .Pretest_Hi_Fail = m_lITHINGCount                       ' IT HI NG数
                Call Cnv_PCS_Per(.Pretest_Hi_Fail, .Prod_Count, .sPretest_Hi_Fail)

                ' 36.Pretest_Lo_Fail 初期値下限不良のﾁｯﾌﾟ数 + %
                .Pretest_Lo_Fail = m_lITLONGCount                       ' IT LO NG数
                Call Cnv_PCS_Per(.Pretest_Lo_Fail, .Prod_Count, .sPretest_Lo_Fail)

                ' 37.Pretest_Open 初期値ｵｰﾌﾟﾝ不良のﾁｯﾌﾟ数 + %
                .Pretest_Open = m_lITOVERCount                          ' ITｵｰﾊﾞｰﾚﾝｼﾞ数
                Call Cnv_PCS_Per(.Pretest_Open, .Prod_Count, .sPretest_Open)

                ' 38.Final_test_Hi_Fail ﾄﾘﾐﾝｸﾞ後の上限不良のﾁｯﾌﾟ数 + %
                .Final_test_Hi_Fail = m_lFTHINGCount                    ' FT HI NG数
                Call Cnv_PCS_Per(.Final_test_Hi_Fail, .Prod_Count, .sFinal_test_Hi_Fail)

                ' 39.Final_test_Lo_Fail ﾄﾘﾐﾝｸﾞ後の下限不良のﾁｯﾌﾟ数 + %
                .Final_test_Lo_Fail = m_lFTLONGCount                  ' FT LO NG数
                Call Cnv_PCS_Per(.Final_test_Lo_Fail, .Prod_Count, .sFinal_test_Lo_Fail)

                ' 40.空白行

                ' 41.sTrim_OK 良品ﾁｯﾌﾟ数 + %
                .Trim_OK = m_lGoodCount                                 ' m_lITHINGCount(IT HI NG数)を設定
                Call Cnv_PCS_Per(.Trim_OK, .Prod_Count, .sTrim_OK)      ' .sTrim_OK = PCS +  %

                ' 42.Init_OK イニシャル良品ﾁｯﾌﾟ数 + %
                ' (トリミングチップ数 - (IT NGﾁｯﾌﾟ数 + FT NGﾁｯﾌﾟ数))
                .Init_OK = .Prod_Count - (.Pretest_Lo_Fail + .Pretest_Hi_Fail + .Pretest_Open + .Final_test_Hi_Fail + .Final_test_Lo_Fail)
                Call Cnv_PCS_Per(.Init_OK, .Prod_Count, .sInit_OK)      ' .sInit_OK = PCS +  %

                ' 43.Total_OK 総良品ﾁｯﾌﾟ数 + %(良品ﾁｯﾌﾟ数と同じ値)
                .Total_OK = .Trim_OK
                Call Cnv_PCS_Per(.Total_OK, .Prod_Count, .sTotal_OK)    ' .sTotal_OK = PCS +  %

                ' 44.Int_RV_NG 初期値上限不良のﾁｯﾌﾟ数 + 初期値下限不良のﾁｯﾌﾟ数
                .Int_RV_NG = .Pretest_Lo_Fail + .Pretest_Hi_Fail
                Call Cnv_PCS_Per(.Int_RV_NG, .Prod_Count, .sInt_RV_NG)  ' .sTInt_RV_NG = PCS +  %

                ' 45.AT_Trim_NG 全NGチップ数
                .AT_Trim_NG = .Pretest_Lo_Fail + .Pretest_Hi_Fail + .Pretest_Open
                .AT_Trim_NG = .AT_Trim_NG + .Final_test_Hi_Fail + .Final_test_Lo_Fail
                Call Cnv_PCS_Per(.AT_Trim_NG, .Prod_Count, .sAT_Trim_NG) ' .sTInt_RV_NG = PCS +  %

                ' 46.空白行

                ' 47.AlmCnt アラーム発生回数 設定済み
                .sAlmCnt = (.AlmCnt.ToString("0")).PadLeft(PRT_SZ)

                ' 48.空白行

                ' 49.アラーム発生時間/エラー内容

            End With

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "QR_Print.SetTrimPrnData() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "トリミング結果結果印刷処理"
    '''=========================================================================
    ''' <summary>トリミング結果結果印刷処理</summary>
    ''' <param name="Md">(INP)モード(0=通常印刷, 1=Printボタン押下, 2=APP終了処理)</param>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub PrnTrimResult(ByVal Md As Integer)

        Dim r As Integer
        Dim Idx As Integer
        Dim sLINE_DATA(ITEM_MAX) As String                              ' 印刷データ
        Dim strHEAD As String
        Dim strDAT As String
        Dim strSPS As String = vbLf                                     ' 空白行(x0A) 
        Dim strMSG As String = ""

        Try
            '-------------------------------------------------------------------
            '   初期設定
            '-------------------------------------------------------------------
            ' V4.0.0.0-60
            ' シンプルトリマの場合には、Lot情報データを自動セーブする（メイン画面のDATA SAVEを押した処理実行）
            If gKeiTyp = KEY_TYPE_RS Then
                SubDataSave()
            End If
            ' V4.0.0.0-60

            ' ローム殿特注でなければNOP
            If (gSysPrm.stCTM.giSPECIAL <> customROHM) Then Return

            ' データ未ロードならばNOP
            If (gLoadDTFlag = False) Then Return

            ' 手動運転ならNOP
            If (stPRT_ROHM.bAutoMode = False) Then Return

            ' 印刷用の開始日がセットされていなければNOP
            If (stPRT_ROHM.Start_day = "") Then Return

            '「Print ON/OFF」ボタンが「Print OFF」ならNOP
            '----- V6.0.3.0_25↓ -----
            'If (gSysPrm.stDEV.rPrnOut_flg = False) Then Return
            If (Form1.BtnPrintOnOff.BackColor <> Color.Lime) Then
                Return
            End If
            '----- V6.0.3.0_25↑ -----

            ' APP終了処理からCAllの場合、印刷未の場合(APP終了エラー発生とみなす)のみ印刷する
            If (Md = 2) Then                                            ' APP終了処理からCAll ?
                If (stPRT_ROHM.bFlgPrint = True) Then Return '          ' 印刷済ならNOP
                Call SetTrimEndTime()                                   ' トリミング終了時間を設定する
            End If

            '-------------------------------------------------------------------
            ' トリミング結果印刷項目のデータをセットする(印刷前に設定可能なもの)
            '-------------------------------------------------------------------
            Call SetTrimPrnData()

            '-------------------------------------------------------------------
            '   印刷用見出しと印刷項目を設定する
            '-------------------------------------------------------------------
            With stPRT_ROHM
                Idx = 1
                strHEAD = "------------------------"                                                                    '  --------------------------
                sLINE_DATA(Idx) = "Machine No:".PadRight(HED_SZ) + .MachineNo : Idx = Idx + 1                           '  1.装置番号
                sLINE_DATA(Idx) = strSPS.PadRight(LINE_SZ) : Idx = Idx + 1                                              '  2.空白行
                sLINE_DATA(Idx) = "Lot Result".PadRight(LINE_SZ) : Idx = Idx + 1                                        '  3.Lot Result
                sLINE_DATA(Idx) = strHEAD : Idx = Idx + 1                                                               '  --------------------------
                'sLINE_DATA(Idx) = "Start day:".PadRight(HED_SZ) + .Start_day : Idx = Idx + 1                           '  4.開始日(2014/Feb/20)　V4.0.0.0-37
                sLINE_DATA(Idx) = "Start day:".PadRight(HED_SZ) + .Start_day.PadLeft(HED_S2) : Idx = Idx + 1            '  4.開始日(2014/Feb/20)　V4.0.0.0-37
                sLINE_DATA(Idx) = "Start time:".PadRight(HED_SZ) + .Start_time.PadLeft(HED_S2) : Idx = Idx + 1          '  5.開始時間
                'sLINE_DATA(Idx) = "End day:".PadRight(HED_SZ) + .End_day : Idx = Idx + 1                               '  6.終了日(2014/Feb/20)　V4.0.0.0-37
                sLINE_DATA(Idx) = "End day:".PadRight(HED_SZ) + .End_day.PadLeft(HED_S2) : Idx = Idx + 1                '  6.終了日(2014/Feb/20)　V4.0.0.0-37
                sLINE_DATA(Idx) = "End time:".PadRight(HED_SZ) + .End_time.PadLeft(HED_S2) : Idx = Idx + 1              '  7.終了時間
                sLINE_DATA(Idx) = "Prod time:".PadRight(HED_SZ) + .Prod_time.PadLeft(HED_S2) : Idx = Idx + 1            '  8.開始～終了までに要した時間
                sLINE_DATA(Idx) = strHEAD : Idx = Idx + 1                                                               '  --------------------------
                sLINE_DATA(Idx) = strSPS.PadRight(LINE_SZ) : Idx = Idx + 1                                              '  9.空白行
                sLINE_DATA(Idx) = "Lot No:".PadRight(HED_S3) + .Lot_No : Idx = Idx + 1                                  ' 10.QRコード内のロット番号
                sLINE_DATA(Idx) = "Prod Type:".PadRight(HED_SZ) + .Prot_Typ : Idx = Idx + 1                             ' 11.品種
                sLINE_DATA(Idx) = "IEC(Tol):".PadRight(HED_SZ) + .IEC_Tol : Idx = Idx + 1                               ' 12.目標抵抗値
                sLINE_DATA(Idx) = "Calculated RV:" + .Cal_RV : Idx = Idx + 1                                            ' 13.目標抵抗値(カットオフを考慮した目標値)
                sLINE_DATA(Idx) = "Laser Power:".PadRight(HED_SZ) + .LaserPower : Idx = Idx + 1                         ' 14.加工面出力
                sLINE_DATA(Idx) = "Stop(%)1:".PadRight(HED_SZ) + .Stop1 : Idx = Idx + 1                                 ' 15.Lターンポイント(%)
                sLINE_DATA(Idx) = "Stop(%)2:".PadRight(HED_SZ) + .Stop2 : Idx = Idx + 1                                 ' 16.カットオフ(%)
                sLINE_DATA(Idx) = "Speed1:".PadRight(HED_SZ) + .Speed1 : Idx = Idx + 1                                  ' 17.加工速度1(Lターン前)
                sLINE_DATA(Idx) = "Speed2:".PadRight(HED_SZ) + .Speed2 : Idx = Idx + 1                                  ' 18.加工速度2(Lターン後)
                sLINE_DATA(Idx) = "Q-Rate1:".PadRight(HED_SZ) + .Qrate1 : Idx = Idx + 1                                 ' 19.Qレート1(Lターン前)
                sLINE_DATA(Idx) = "Q-Rate2:".PadRight(HED_SZ) + .Qrate2 : Idx = Idx + 1                                 ' 20.Qレート2(Lターン後)
                sLINE_DATA(Idx) = "Glv posX:" + .GlvPosX + " Y:" + .GlvPosY : Idx = Idx + 1                             ' 21.ガルバノスタートポイント(カット位置XY)
                sLINE_DATA(Idx) = "Comp.Offset:".PadRight(HED_SZ) + .CompOffset : Idx = Idx + 1                         ' 22.カットオフ(※内容不明のため未対応←カットオフ(%)を設定)
                sLINE_DATA(Idx) = strSPS.PadRight(LINE_SZ) : Idx = Idx + 1                                              ' 23.空白行
                sLINE_DATA(Idx) = "Trim Rate:".PadRight(HED_SZ) + .sTrimRate : Idx = Idx + 1                            ' 24.平均切上率
                sLINE_DATA(Idx) = "MTBF:".PadRight(HED_SZ) + .MTBF : Idx = Idx + 1                                      ' 25.エラー発生間隔
                sLINE_DATA(Idx) = "MTTR:".PadRight(HED_SZ) + .MTTR : Idx = Idx + 1                                      ' 26.エラー発生後の復旧時間
                sLINE_DATA(Idx) = "Tol.Sheet:".PadRight(HED_SZ) + .sTol_Sheet : Idx = Idx + 1                           ' 27.トータル枚数
                sLINE_DATA(Idx) = "NG Cancel:".PadRight(HED_SZ) + "---".PadLeft(HED_S2) : Idx = Idx + 1                 ' 28.NGキャンセル数(※設定されるケースがないため未対応)
                sLINE_DATA(Idx) = "JudgeValue:".PadRight(HED_SZ) + .sJudgeValue : Idx = Idx + 1                         ' 29.不具合発生率(全NG数/トリミング数)
                sLINE_DATA(Idx) = "Offset X:" + .OffsetX + " Y:" + .OffsetY : Idx = Idx + 1                             ' 30.オフセット(BPオフセットXY)
                sLINE_DATA(Idx) = "Prod.Count:".PadRight(HED_SZ) + .sProd_Count : Idx = Idx + 1                         ' 31.トリミングチップ数
                sLINE_DATA(Idx) = "Tol.NG sheet:".PadRight(HED_SZ) + .sTol_NG_Sheet : Idx = Idx + 1                     ' 32.総NG基板枚数
                sLINE_DATA(Idx) = strSPS.PadRight(LINE_SZ) : Idx = Idx + 1                                              ' 32.空白行
                sLINE_DATA(Idx) = "PCS".PadLeft(PRT_SZ + PRT_S2) + "(%)".PadLeft(PRT_S3) : Idx = Idx + 1                ' 34. PCS (%) ← 見出し
                sLINE_DATA(Idx) = strHEAD : Idx = Idx + 1                                                               '  --------------------------
                sLINE_DATA(Idx) = "PT over".PadRight(HED_S2) + .sPretest_Hi_Fail : Idx = Idx + 1                        ' 35.初期値上限不良のﾁｯﾌﾟ数
                sLINE_DATA(Idx) = "PT under".PadRight(HED_S2) + .sPretest_Lo_Fail : Idx = Idx + 1                       ' 36.初期値下限不良のﾁｯﾌﾟ数
                sLINE_DATA(Idx) = "Probe Err".PadRight(HED_S2) + .sPretest_Open : Idx = Idx + 1                         ' 37.初期値ｵｰﾌﾟﾝ不良のﾁｯﾌﾟ数
                sLINE_DATA(Idx) = "AT over".PadRight(HED_S2) + .sFinal_test_Hi_Fail : Idx = Idx + 1                     ' 38.ﾄﾘﾐﾝｸﾞ後の上限不良のﾁｯﾌﾟ数
                sLINE_DATA(Idx) = "AT under".PadRight(HED_S2) + .sFinal_test_Lo_Fail : Idx = Idx + 1                    ' 39.ﾄﾘﾐﾝｸﾞ後の下限不良のﾁｯﾌﾟ数
                sLINE_DATA(Idx) = strSPS.PadRight(LINE_SZ) : Idx = Idx + 1                                              ' 40.空白行
                sLINE_DATA(Idx) = "Trim OK:".PadRight(HED_S2) + .sTrim_OK : Idx = Idx + 1                               ' 41.良品ﾁｯﾌﾟ数
                sLINE_DATA(Idx) = "Int OK:".PadRight(HED_S2) + .sInit_OK : Idx = Idx + 1                                ' 42.イニシャル良品ﾁｯﾌﾟ数
                sLINE_DATA(Idx) = "Total OK:".PadRight(HED_S2) + .sTotal_OK : Idx = Idx + 1                             ' 43.総良品ﾁｯﾌﾟ数(良品ﾁｯﾌﾟ数と同じ)
                sLINE_DATA(Idx) = strHEAD : Idx = Idx + 1                                                               '  --------------------------
                sLINE_DATA(Idx) = "TInt RV NG:".PadRight(HED_S2) + .sInt_RV_NG : Idx = Idx + 1                          ' 44.イニシャルNGチップ数 + ファイナルNGチップ数
                sLINE_DATA(Idx) = "AT Trim NG:".PadRight(HED_S2) + .sAT_Trim_NG : Idx = Idx + 1                         ' 45.全NGチップ数
                sLINE_DATA(Idx) = strSPS.PadRight(LINE_SZ) : Idx = Idx + 1                                              ' 46.空白行
                sLINE_DATA(Idx) = "Alarm Count:".PadRight(HED_SZ) + .sAlmCnt : Idx = Idx + 1                            ' 47.アラーム発生回数
                sLINE_DATA(Idx) = " ".PadRight(LINE_SZ) : Idx = Idx + 1                                                 ' 48.空白行
                sLINE_DATA(Idx) = "Alarm Time/Error Dtls:".PadRight(LINE_SZ) : Idx = Idx + 1                            ' 49.アラーム発生時間/エラー内容
                sLINE_DATA(Idx) = strHEAD : Idx = Idx + 1                                                               '  --------------------------
            End With

            '-------------------------------------------------------------------
            '   印刷用テキストボックス(TxtBoxPrint(非表示))に出力する
            '-------------------------------------------------------------------
            ' 印刷用テキストボックスにトリミング結果を出力する
            For Idx = 1 To ITEM_MAX                                     ' 最大項目数分繰り返す
                strDAT = sLINE_DATA(Idx) & vbCrLf
                ' 印刷用テキストボックスに出力
                If (Idx = 1) Then
                    Form1.TxtBoxPrint.Text = strDAT
                Else
                    Form1.TxtBoxPrint.AppendText(strDAT)
                End If
            Next Idx

            ' アラームデータを印刷用テキストボックスに出力する
            Call ReadAlarmData(gFPATH_QR_ALARM)

            '-------------------------------------------------------------------
            '   印刷処理(テキストボックス(TxtBoxPrint)の文字列を印刷する)
            '-------------------------------------------------------------------
            r = Print_Data(Form1.TxtBoxPrint.Text, strHEAD, HEAD_POSX, HEAD_POSY)
            If (r <> cFRS_NORMAL) Then
                ' "印刷処理に失敗しました。(r=xxxx)"
                strMSG = MSG_153 + "(r = " + r.ToString("0") + ")"
                Call MsgBox(strMSG, vbOKOnly)
            End If
            stPRT_ROHM.bFlgPrint = True                                 ' 印刷済みフラグ = True(印刷済)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "QR_Print.PrnTrimResult() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '---------------------------------------------------------------------------
    '   共通関数
    '---------------------------------------------------------------------------
#Region "トリミング開始時間を設定する"
    '''=========================================================================
    ''' <summary>トリミング開始時間を設定する</summary>
    ''' <remarks>以下からCallされる
    ''' 　　　 ・Trimming(トリミング実行時)
    ''' </remarks>
    '''=========================================================================
    Public Sub SetTrimStartTime()

        Dim Dt As DateTime = DateTime.Now                               ' 現在の日時を取得
        Dim TimeOfDt As TimeSpan = Dt.TimeOfDay                         ' 現在の時刻のみを取得 
        Dim strYYYY As String
        Dim strMM As String
        Dim strDD As String
        Dim strMSG As String

        Try
            ' 初期処理
            If (gSysPrm.stCTM.giSPECIAL <> customROHM) Then Return '    ' ローム殿特注でなければNOP
            If (stPRT_ROHM.Start_time <> "") Then Return '              ' すでに設定済みならNOP

            ' 印刷項目を設定する
            With stPRT_ROHM
                .Start_time = TimeOfDay.ToString("HH:mm:ss")            ' 開始時間(hh:mm:ss)
                .STtime = TimeOfDt.Ticks / 10000000                     ' 開始時間(現在の時刻を午前0時からの通算100ナノ秒で返す→秒)
                '.Start_day = Today.ToString("yyyy/MM/dd")              ' 開始日(2014/02/05)
                strYYYY = DateTime.Today.Year.ToString("0000")          ' 作業日(2014/Feb/5)
                strDD = DateTime.Today.Day.ToString("0")                ' 
                Dim Cl As New System.Globalization.CultureInfo("en-US") ' (カルチャーを英語にする) 
                strMM = Dt.ToString("MMM", Cl)                          ' (月の省略形(February→Feb)) 
                .Start_day = strYYYY + "/" + strMM + "/" + strDD        ' 開始日(2014/Feb/5)
            End With

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "QR_Print.SetTrimStartTime() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "トリミング終了時間を設定する"
    '''=========================================================================
    ''' <summary>トリミング終了時間を設定する</summary>
    ''' <remarks>以下からCallされる
    ''' 　　　 ・Trimming(自動運転終了時)
    ''' </remarks>
    '''=========================================================================
    Public Sub SetTrimEndTime()

        Dim Dt As DateTime = DateTime.Now                               ' 現在の日時を取得
        Dim TimeOfDt As TimeSpan = Dt.TimeOfDay                         ' 現在の時刻のみを取得 
        Dim strYYYY As String
        Dim strMM As String
        Dim strDD As String
        Dim strMSG As String

        Try
            ' 印刷項目を設定する
            If (gSysPrm.stCTM.giSPECIAL <> customROHM) Then Return '    ' ローム殿特注でなければNOP
            With stPRT_ROHM
                .End_time = TimeOfDay.ToString("HH:mm:ss")              ' 終了時間(hh:mm:ss)
                .EDtime = TimeOfDt.Ticks / 10000000                     ' 終了時間(現在の時刻を午前0時からの通算100ナノ秒で返す→秒)
                strYYYY = DateTime.Today.Year.ToString("0000")          ' 作業日(2014/Feb/5)
                strDD = DateTime.Today.Day.ToString("0")                ' 
                Dim Cl As New System.Globalization.CultureInfo("en-US") ' (カルチャーを英語にする) 
                strMM = Dt.ToString("MMM", Cl)                          ' (月の省略形(February→Feb)) 
                .End_day = strYYYY + "/" + strMM + "/" + strDD          ' 終了日(2014/Feb/5)
            End With

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "QR_Print.SetTrimEndTime() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "アラーム発生回数とアラーム停止開始時間を設定する"
    '''=========================================================================
    ''' <summary>アラーム発生回数とアラーム停止開始時間を設定する</summary>
    ''' <remarks>以下からCallされる
    ''' 　　　 ・FormLoaderAlarm.ShowDialog(ローダアラーム表示開始時)
    ''' 　　　 ・Call_SetAlmStartTime(OcxSystem(INtime側エラー表示開始時)
    ''' </remarks>
    '''=========================================================================
    Public Sub SetAlmStartTime()

        Dim Dt As DateTime = DateTime.Now                               ' 現在の日時を取得
        Dim TimeOfDt As TimeSpan = Dt.TimeOfDay                         ' 現在の時刻のみを取得 
        Dim strMSG As String

        Try
            ' 初期処理
            If (gSysPrm.stCTM.giSPECIAL <> customROHM) Then Return '    ' ローム殿特注でなければNOP
            If (stPRT_ROHM.bAutoMode = False) Then Return '             ' 自動運転中でなければNOP

            ' アラーム発生回数とアラーム停止開始時間を設定する
            gbAlarmFlg = True                                           ' アラーム発生フラグ = True(アラーム発生)
            With stPRT_ROHM
                .AlarmST_time = TimeOfDay.ToString("HH:mm:ss")          ' アラーム開始時間(hh:mm:ss)
                .AlmCnt = .AlmCnt + 1                                   ' アラーム発生回数
                .AlmSTtime = TimeOfDt.Ticks / 10000000                  ' アラーム停止開始時間(現在の時刻を午前0時からの通算100ナノ秒で返す→秒)
            End With

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "QR_Print.SetAlmStartTime() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "アラーム停止終了時間を設定する"
    '''=========================================================================
    ''' <summary>アラーム停止終了時間を設定する</summary>
    ''' <remarks>以下からCallされる
    ''' 　　　 ・Sub_CallFormLoaderAlarm(ローダアラーム表示終了時)
    ''' 　　　 ・Call_SetAlmEndTime(OcxSystem(INtime側エラー表示終了時)
    ''' </remarks>
    '''=========================================================================
    Public Sub SetAlmEndTime()

        Dim Dt As DateTime = DateTime.Now                               ' 現在の日時を取得
        Dim TimeOfDt As TimeSpan = Dt.TimeOfDay                         ' 現在の時刻のみを取得 
        Dim UpTime As Double = 0.0
        Dim strMSG As String

        Try
            ' 初期処理
            If (gSysPrm.stCTM.giSPECIAL <> customROHM) Then Return '    ' ローム殿特注でなければNOP
            If (gbAlarmFlg = False) Then Return '                       ' アラーム発生でなければNOP
            gbAlarmFlg = False

            ' アラーム停止終了時間を設定する
            With stPRT_ROHM
                .AlmEDtime = TimeOfDt.Ticks / 10000000                  ' アラーム停止終了時間(現在の時刻を午前0時からの通算100ナノ秒で返す→秒)

                ' アラーム停止トータル時間
                If (.AlmSTtime > .AlmEDtime) Then
                    UpTime = Double.Parse(24 * 60 * 60)                 ' Uptime = 1日の秒数 
                End If
                .AlmTotaltime = (.AlmEDtime + UpTime) - .AlmSTtime      ' 終了時間－開始時間

            End With

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "QR_Print.SetAlmEndTime() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "印刷用データ編集(秒数を「時:分:秒」形式に変換して返す)"
    '''=========================================================================
    ''' <summary>秒数を「時:分:秒」形式に変換して返す</summary>
    ''' <param name="dblSec">(INP)秒数</param>
    ''' <param name="strDAT">(OUT)「時:分:秒」に変換した値</param>
    '''=========================================================================
    Public Sub Cnv_SecToHHMMSS(ByVal dblSec As Double, ByRef strDAT As String)

        Dim wkHH As Integer
        Dim wkMM As Integer
        Dim wkSS As Integer
        Dim strMSG As String

        Try
            wkHH = dblSec \ 3600                                       ' HH = 3600秒(1時間)で割った余り 
            wkMM = (dblSec Mod 3600) \ 60                              ' MM = (3600秒(1時間)で割った値) を60秒(1分)で割った余り 
            wkSS = (dblSec Mod 3600) Mod 60                            ' SS = (3600秒(1時間)で割った値) を60秒(1分)で割った値 

            strDAT = wkHH.ToString("00") + ":" + wkMM.ToString("00") + ":" + wkSS.ToString("00")

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "QR_Print.Cnv_SecToHHMMSS() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "印刷用データ編集(数と%を「     9    99.99」形式に変換して返す)"
    '''=========================================================================
    ''' <summary>数と%を「     9    99.99」形式に変換して返す</summary>
    ''' <param name="Count">   (INP)数</param>
    ''' <param name="TtlCount">(INP)総数</param>
    ''' <param name="strDAT">　(OUT)「     9    99.99」に変換した値</param>
    '''=========================================================================
    Public Sub Cnv_PCS_Per(ByVal Count As Integer, ByVal TtlCount As Integer, ByRef strDAT As String)

        Dim strMSG As String

        Try
            strDAT = (Count.ToString("0")).PadLeft(PRT_S2)                              ' 数 = 右詰前方空白
            If (TtlCount = 0) Then
                strMSG = "0.00".PadLeft(PRT_S3)                                         ' %   = 右詰前方空白
            Else
                strMSG = (((Count / TtlCount) * 100).ToString("0.00")).PadLeft(PRT_S3)  ' %   = 右詰前方空白
            End If
            strDAT = strDAT + strMSG                                                    ' "     数     %"

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "QR_Print.Cnv_PCS_Per() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "印刷用データ編集(速度2とQレート2を編集して返す)"
    '''=========================================================================
    ''' <summary>速度2とQレート2を編集して返す</summary>
    ''' <param name="Kd">    (INP)0=速度2, 1=Qレート2</param>
    ''' <param name="strDAT">(OUT)変換した値</param>
    '''=========================================================================
    Public Sub Cnv_Spd2_Qrate2(ByVal Kd As Integer, ByRef strDAT As String)

        Dim iWK As Integer
        Dim strMSG As String

        Try
            '----- V6.0.3.0_45 ↓ -----
            If (gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Then           ' FLでない ?
                ' 第1抵抗第１カットの種別により設定する
                Select Case typResistorInfoArray(1).ArrCut(1).strCutType
                    ' 斜めSTカット(リターン, リトレース), 斜めLカット(リターン, リトレース)
                    Case CNS_CUTP_NSTr, CNS_CUTP_NSTt, CNS_CUTP_NLr, CNS_CUTP_NLt
                        If (Kd = 0) Then
                            strDAT = typResistorInfoArray(1).ArrCut(1).dblCutSpeed2.ToString("0.0") + "mm/s"
                        Else
                            strDAT = typResistorInfoArray(1).ArrCut(1).dblQRate2.ToString("0.0") + "kHz"
                        End If
                    Case Else
                        strDAT = "---"
                End Select
                Return
            End If
            '----- V6.0.3.0_45 ↑ -----

            ' 第1抵抗第１カットの種別により設定する
            Select Case typResistorInfoArray(1).ArrCut(1).strCutType
                ' 斜めSTカット(リターン, リトレース), 斜めLカット, 斜めLカット(リターン, リトレース), HKカット, Uカット
                Case CNS_CUTP_NSTr, CNS_CUTP_NSTt, CNS_CUTP_NL, CNS_CUTP_NLr, CNS_CUTP_NLt, CNS_CUTP_HK, CNS_CUTP_U, CNS_CUTP_Ut ' V1.22.0.0①
                    If (Kd = 0) Then
                        strDAT = typResistorInfoArray(1).ArrCut(1).dblCutSpeed2.ToString("0.0") + "mm/s"
                    Else
                        iWK = typResistorInfoArray(1).ArrCut(1).CndNum(CUT_CND_L2)
                        strDAT = stCND.Freq(iWK).ToString("0.0") + "kHz"
                    End If

                Case Else
                    strDAT = "---"
            End Select

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "QR_Print.Cnv_Spd2_Qrate2() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "印刷用データ編集(目標値を編集して返す)"
    '''=========================================================================
    ''' <summary>目標値を編集して返す</summary>
    ''' <param name="Nominal">(INP)目標値</param>
    ''' <param name="strDAT"> (OUT)「時:分:秒」に変換した値</param>
    '''=========================================================================
    Public Sub Cnv_TergetVal(ByVal Nominal As Double, ByRef strDAT As String)

        Dim dblWK As Double
        Dim strMSG As String

        Try
            If (Nominal < 1.0) Then
                dblWK = Nominal * 1000
                strDAT = dblWK.ToString("0.000") + "m"
            ElseIf (Nominal >= 1.0) And (Nominal < 1000.0) Then
                strDAT = Nominal.ToString("0.000")
            ElseIf (Nominal >= 1000.0) And (Nominal < 1000000.0) Then
                dblWK = Nominal * 0.001
                strDAT = dblWK.ToString("0.000") + "K"
            ElseIf (Nominal >= 1000000.0) And (Nominal < 1000000000.0) Then
                dblWK = Nominal * 0.000001
                strDAT = dblWK.ToString("0.000") + "M"
            Else
                dblWK = Nominal * 0.000000001
                strDAT = dblWK.ToString("0.000") + "G"
            End If

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "QR_Print.Cnv_TergetVal() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "印刷用アラームファイルにアラームデータを書き込む"
    '''=========================================================================
    ''' <summary>印刷用アラームファイルにアラームデータを書き込む</summary>
    ''' <param name="strPath"> (INP)ファイルパス名</param>
    ''' <param name="strDAT">  (INP)エラーメッセージ</param>
    ''' <param name="strTIME"> (INP)アラーム発生時間("hh:mm:ss")</param>
    ''' <param name="ErrCode"> (INP)エラーコード</param>
    '''=========================================================================
    Public Sub WriteAlarmData(ByVal strPath As String, ByVal strDAT As String, ByVal strTIME As String, ByVal ErrCode As Short)

        'Dim writer As System.IO.StreamWriter = Nothing
        Dim strMSG As String

        Try
            ' 初期処理
            If (gSysPrm.stCTM.giSPECIAL <> customROHM) Then Return '    ' ローム殿特注でなければNOP
            '                                                           ' false = 上書き(true = 追加)
            Return  'V1.18.0.0⑬ 有効にするにはこの行を削除する

            'writer = New System.IO.StreamWriter(strPath, True, System.Text.Encoding.GetEncoding("Shift_JIS"))
            Using writer As New StreamWriter(strPath, True, Encoding.UTF8)      'V4.4.0.0-0
                ' アラームデータを編集してファイルに書き込む
                ' "HH:MM:SS,ERROR:,xxxxxx,メッセージ" + \r\n
                strMSG = strTIME & ",ERROR,"                                    ' 発生時間(hh:mm:ss)
                strMSG = strMSG & System.Math.Abs(ErrCode).ToString("0") & ","  ' エラーコード
                strMSG = strMSG & strDAT                                        ' メッセージ 
                writer.WriteLine(strMSG)

                ' 後処理
                'writer.Close()
            End Using

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "QR_Print.WriteAlarmData() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "印刷用アラームファイルのアラームデータを印刷用テキストボックスに出力する"
    '''=========================================================================
    ''' <summary>印刷用アラームファイルのアラームデータを印刷用テキストボックスに出力する</summary>
    ''' <param name="strPath"> (INP)ファイルパス名</param>
    '''=========================================================================
    Public Sub ReadAlarmData(ByVal strPath As String)

        'Dim intFileNo As Integer                                        ' ファイル番号
        'Dim iFlg As Integer
        Dim Ln As Integer
        Dim Sz As Integer
        Dim Pos As Integer
        Dim strDAT As String                                            ' 読み込みデータバッファ
        Dim strMSG As String
        Dim mDATA() As String = New String(4) {}

        Try
            '-------------------------------------------------------------------
            '   初期処理
            '-------------------------------------------------------------------
            ' アラームファイルが存在しなければNOP
            If (System.IO.File.Exists(strPath) = False) Then Return

            ' アラームファイルをオープンする
            'intFileNo = FreeFile()                                      ' 使用可能なファイルナンバーを取得
            'FileOpen(intFileNo, strPath, OpenMode.Input)
            'iFlg = 1
            Using sr As New StreamReader(strPath, Encoding.UTF8)        'V4.4.0.0-0
                '-------------------------------------------------------------------
                '   アラームデータを印刷用テキストボックスに出力する
                '-------------------------------------------------------------------
                ' ファイルの終端までループを繰り返す
                'Do While Not EOF(intFileNo)
                Do While (False = sr.EndOfStream)                       'V4.4.0.0-0
                    'strDAT = LineInput(intFileNo)                           ' 1行読み込み
                    strDAT = sr.ReadLine()                                  ' 1行読み込み
                    If (strDAT.Length < 20) Then GoTo STP_NEXT

                    ' "HH:MM:SS,ERROR:,xxxxxx" + \r\n
                    mDATA = strDAT.Split(",")                               ' データを,区切りで取り出す
                    strMSG = mDATA(0).PadRight(12)                          ' "HH:MM:SS    "
                    strMSG = strMSG + (mDATA(1) + ":")                      ' "ERROR:"
                    strMSG = strMSG + mDATA(2).PadLeft(6) + vbCrLf          ' "  エラーコード"
                    Form1.TxtBoxPrint.AppendText(strMSG)                    ' 印刷用テキストボックスに出力する

                    ' メッセージを1行の文字数分づつ取り出す
                    Sz = mDATA(3).Length
                    Ln = mDATA(3).Length
                    Pos = 0
                    Do While (Pos < Ln)
                        If (Sz > LINE_SZ) Then
                            Sz = LINE_SZ
                        Else
                            Sz = Ln - Pos
                        End If
                        strMSG = mDATA(3).Substring(Pos, Sz)                ' メッセージ
                        strMSG = strMSG.PadRight(LINE_SZ) + vbCrLf          ' 
                        Form1.TxtBoxPrint.AppendText(strMSG)                ' 印刷用テキストボックスに出力する
                        Pos = Pos + Sz
                    Loop

                    'If (gSysPrm.stTMN.giMsgTyp = 0) Then                    ' 日本語(本来は英語のみ) ? 
                    '    Sz = mDATA(3).Length
                    '    Ln = mDATA(3).Length
                    '    Pos = 0
                    '    Do While (Pos < Ln)
                    '        If (Sz > (LINE_SZ / 2)) Then
                    '            Sz = LINE_SZ / 2
                    '        Else
                    '            Sz = Ln - Pos
                    '        End If
                    '        strMSG = mDATA(3).Substring(Pos, Sz) + vbCrLf   ' メッセージ
                    '        Form1.TxtBoxPrint.AppendText(strMSG)            ' 印刷用テキストボックスに出力する
                    '        Pos = Pos + Sz
                    '    Loop

                    'Else                                                    ' 英語の場合 
                    '    Sz = mDATA(3).Length
                    '    Ln = mDATA(3).Length
                    '    Pos = 0
                    '    Do While (Pos < Ln)
                    '        If (Sz > LINE_SZ) Then
                    '            Sz = LINE_SZ
                    '        Else
                    '            Sz = Ln - Pos
                    '        End If
                    '        strMSG = mDATA(3).Substring(Pos, Sz) + vbCrLf   ' メッセージ
                    '        Form1.TxtBoxPrint.AppendText(strMSG)            ' 印刷用テキストボックスに出力する
                    '        Pos = Pos + Sz
                    '    Loop
                    'End If

STP_NEXT:
                Loop

                '-------------------------------------------------------------------
                '   後処理
                '-------------------------------------------------------------------
                'strMSG = " ".PadRight(LINE_SZ) + vbCr + vbCr + vbCr + vbCrLf
                'Form1.TxtBoxPrint.AppendText(strMSG)                        ' ダミー印刷

                'If (iFlg = 1) Then
                '    FileClose(intFileNo)                                    ' ファイルクローズ 
                'End If

            End Using

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "QR_Print.ReadAlarmData() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "印刷用アラームファイルを削除する"
    '''=========================================================================
    ''' <summary>印刷用アラームファイルを削除する</summary>
    ''' <param name="strPath"> (INP)ファイルパス名</param>
    '''=========================================================================
    Public Sub DeleteAlarmData(ByVal strPath As String)

        Dim strMSG As String = ""

        Try
            ' アラームファイルが存在しなければNOP
            If (System.IO.File.Exists(strPath) = False) Then Return
            ' 自動運転継続中ならNOP   
            If (gbFgContinue = True) Then Return

            ' アラームファイルを削除する
            System.IO.File.Delete(strPath)                              ' ファイルが存在しなくても例外は発生しないが使用中の場合は発生する

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "QR_Print.DeleteAlarmData() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

    '-------------------------------------------------------------------------------
    '   罫線を使用した通常のプリンタの印刷例   ↓
    '-------------------------------------------------------------------------------
#Region "罫線を使用した通常のプリンタの印刷例"
    '#Region "【定数/変数の定義】"
    '    '===========================================================================
    '    '   定数/変数の定義
    '    '===========================================================================
    '    '----- クラスオブジェクト -----
    '    Private ObjPrint As DllPrintString.PrintString = Nothing            ' 印刷用オブジェクト(DllPrintString.dll)
    '    'Private strFont As String = "ＭＳ Ｐゴシック"                     ' 印刷用フォント 
    '    Private strFont As String = "ＭＳ ゴシック"                       ' 印刷用フォント 
    '    Private iFontSz As Integer = 10                                     ' フォントサイズ

    '    '----- 罫線印刷用 -----
    'Private Const ITEM_MAX As Integer = 50                              ' 印刷項目数
    'Private Const H_LINE_NUM As Integer = ITEM_MAX                      ' 罫線横ライン数(0-51)
    'Private Const V_LINE_NUM As Integer = (4 - 1)                       ' 罫線縦ライン数(0-3)
    'Private KWidth As Double = 0.2                                      ' 罫線印刷用ペン幅(mm)
    'Private H_StaPosAry(,) As Integer = Nothing                         ' 罫線横ライン開始座標X,Y配列
    'Private H_EndPosAry(,) As Integer = Nothing                         ' 罫線横ライン終了座標X,Y配列
    'Private V_StaPosAry(,) As Integer = Nothing                         ' 罫線縦ライン開始座標X,Y配列
    'Private V_EndPosAry(,) As Integer = Nothing                         ' 罫線縦ライン終了座標X,Y配列

    '    '----- 印刷位置 -----
    '    Private Const HEAD_POSX As Integer = 10                             ' 見出し印刷座標X
    '    Private Const HEAD_POSY As Integer = 6                              ' 見出し印刷座標Y

    '    '----- 印刷位置(罫線(縦ライン)) -----
    '    Private Const PR_V0_POSX As Integer = 10                            ' 縦座標0X
    '    Private Const PR_V1_POSX As Integer = 20                            ' 縦座標1X
    '    Private Const PR_V2_POSX As Integer = 69                            ' 縦座標2X
    '    Private Const PR_V3_POSX As Integer = 110                           ' 縦座標3X

    '    Private Const PR_V0_POSY As Integer = 10                            ' 縦座標0Y
    '    Private Const PR_V1_POSY As Integer = 120                           ' 縦座標1Y

    '    '----- 印刷位置(罫線(横ライン)) -----
    '    Private Const PR_H0_POSX As Integer = PR_V0_POSX                    ' 縦座標0X

    '    Private Const PR_H0_OFSY As Integer = 5                             ' 罫線縦ピッチ 

    '    Private Const PR_H00_POSY As Integer = PR_V0_POSY                    ' 縦座標0Y(10)
    '    Private Const PR_H01_POSY As Integer = PR_H00_POSY + PR_H0_OFSY      ' 縦座標1Y(15)
    '    Private Const PR_H02_POSY As Integer = PR_H01_POSY + PR_H0_OFSY      ' 縦座標2Y( .) 
    '    Private Const PR_H03_POSY As Integer = PR_H02_POSY + PR_H0_OFSY      ' 縦座標3Y
    '    Private Const PR_H04_POSY As Integer = PR_H03_POSY + PR_H0_OFSY      ' 縦座標4Y
    '    Private Const PR_H05_POSY As Integer = PR_H04_POSY + PR_H0_OFSY      ' 縦座標5Y
    '    Private Const PR_H06_POSY As Integer = PR_H05_POSY + PR_H0_OFSY      ' 縦座標6Y
    '    Private Const PR_H07_POSY As Integer = PR_H06_POSY + PR_H0_OFSY      ' 縦座標7Y
    '    Private Const PR_H08_POSY As Integer = PR_H07_POSY + PR_H0_OFSY      ' 縦座標8Y
    '    Private Const PR_H09_POSY As Integer = PR_H08_POSY + PR_H0_OFSY      ' 縦座標9Y
    '    Private Const PR_H10_POSY As Integer = PR_H09_POSY + PR_H0_OFSY      ' 縦座標10Y
    '    Private Const PR_H11_POSY As Integer = PR_H10_POSY + PR_H0_OFSY      ' 縦座標11Y
    '    Private Const PR_H12_POSY As Integer = PR_H11_POSY + PR_H0_OFSY      ' 縦座標12Y
    '    Private Const PR_H13_POSY As Integer = PR_H12_POSY + PR_H0_OFSY      ' 縦座標13Y
    '    Private Const PR_H14_POSY As Integer = PR_H13_POSY + PR_H0_OFSY      ' 縦座標14Y
    '    Private Const PR_H15_POSY As Integer = PR_H14_POSY + PR_H0_OFSY      ' 縦座標15Y
    '    Private Const PR_H16_POSY As Integer = PR_H15_POSY + PR_H0_OFSY      ' 縦座標16Y
    '    Private Const PR_H17_POSY As Integer = PR_H16_POSY + PR_H0_OFSY      ' 縦座標17Y
    '    Private Const PR_H18_POSY As Integer = PR_H17_POSY + PR_H0_OFSY      ' 縦座標18Y
    '    Private Const PR_H19_POSY As Integer = PR_H18_POSY + PR_H0_OFSY      ' 縦座標19Y
    '    Private Const PR_H20_POSY As Integer = PR_H19_POSY + PR_H0_OFSY      ' 縦座標20Y
    '    Private Const PR_H21_POSY As Integer = PR_H20_POSY + PR_H0_OFSY      ' 縦座標21Y
    '    Private Const PR_H22_POSY As Integer = PR_H21_POSY + PR_H0_OFSY      ' 縦座標22Y
    '    Private Const PR_H23_POSY As Integer = PR_H22_POSY + PR_H0_OFSY      ' 縦座標23Y
    '    Private Const PR_H24_POSY As Integer = PR_H23_POSY + PR_H0_OFSY      ' 縦座標24Y
    '    Private Const PR_H25_POSY As Integer = PR_H24_POSY + PR_H0_OFSY      ' 縦座標25Y
    '    Private Const PR_H26_POSY As Integer = PR_H25_POSY + PR_H0_OFSY      ' 縦座標26Y
    '    Private Const PR_H27_POSY As Integer = PR_H26_POSY + PR_H0_OFSY      ' 縦座標27Y
    '    Private Const PR_H28_POSY As Integer = PR_H27_POSY + PR_H0_OFSY      ' 縦座標28Y
    '    Private Const PR_H29_POSY As Integer = PR_H28_POSY + PR_H0_OFSY      ' 縦座標29Y
    '    Private Const PR_H30_POSY As Integer = PR_H29_POSY + PR_H0_OFSY      ' 縦座標30Y
    '    Private Const PR_H31_POSY As Integer = PR_H30_POSY + PR_H0_OFSY      ' 縦座標31Y

    '    '---------------------------------------------------------------------------
    '    '   トリミング結果印刷用構造体(ローム殿用) 
    '    '---------------------------------------------------------------------------
    '    Public Structure TRIM_RESULT_PRINT_ROHM_INFO
    '        Dim bAutoMode As Boolean                                        ' 自動/手動(False=手動運転, True = 自動運転)
    '        Dim DateR As String                                             ' 作業日(2014/Feb/20)
    '        Dim START_TIME As String                                        ' 開始時間(hh:mm:ss)
    '        Dim STOP_TIME As String                                         ' 終了時間(hh:mm:ss)
    '        Dim PROG_TIME As String                                         ' 開始～終了までに要した時間(hh:mm:ss)
    '        Dim OPE_TIME As String                                          ' 稼動時間(hh:mm:ss)
    '        Dim ALARM_TIME As String                                        ' ｱﾗｰﾑにより停止した時間(hh:mm:ss)
    '        Dim OPE_RATE As String                                          ' 稼働率(1-
    '        Dim MTBF As String                                              ' 平均故障間隔
    '        Dim MTTR As String                                              ' 平均復旧時間
    '        Dim LOT_NO As String                                            ' ﾄﾘﾐﾝｸﾞﾃﾞｰﾀｼｰｹﾝｽﾅﾝﾊﾞｰ(QRコード内のロット番号)
    '        Dim Qrate As String                                             ' ﾄﾘﾐﾝｸﾞQﾚｰﾄ
    '        Dim Trim_Speed As String                                        ' ﾄﾘﾐﾝｸﾞｶｯﾄｽﾋﾟｰﾄﾞ
    '        Dim Trim_OK As Integer                                          ' 良品ﾁｯﾌﾟ数
    '        Dim Pretest_Lo_Fail As Integer                                  ' 初期値下限不良のﾁｯﾌﾟ数
    '        Dim Pretest_Hi_Fail As Integer                                  ' 初期値上限不良のﾁｯﾌﾟ数
    '        Dim Pretest_Open As Integer                                     ' 初期値ｵｰﾌﾟﾝ不良のﾁｯﾌﾟ数
    '        Dim Cut_NG As Integer                                           ' ﾄﾘﾐﾝｸﾞ時に目標値に達しなかったﾁｯﾌﾟ数
    '        Dim Pretest_NG_Cut_NG As Integer                                ' 初期不良
    '        Dim Final_test_Lo_Fail As Integer                               ' ﾄﾘﾐﾝｸﾞ後の下限不良のﾁｯﾌﾟ数
    '        Dim Final_test_Hi_Fail As Integer                               ' ﾄﾘﾐﾝｸﾞ後の上限不良のﾁｯﾌﾟ数
    '        Dim Final_test_Open As Integer                                  ' ﾄﾘﾐﾝｸﾞ後にｵｰﾌﾟﾝｴﾗｰとなったﾁｯﾌﾟ数
    '        Dim Yield As String                                             ' 良品ﾁｯﾌﾟ数÷ﾁｯﾌﾟ数
    '        Dim Yield_Par As Double                                         ' 上記の%表示
    '        Dim Pdt_Sheet As Integer                                        ' ﾄﾘﾐﾝｸﾞｽﾃｰｼﾞで処理した基板枚数
    '        Dim Lot_Sheet As Integer                                        ' 装置に投入されたﾛｯﾄ枚数
    '        Dim Lot_NG_Sheet As Integer                                     ' ﾛｯﾄ中の不良基板数
    '        Dim Edg_Fail As Integer                                         ' ﾛｯﾄ中の認識不良基板枚数
    '        Dim Nominal As Double                                           ' 目標抵抗値
    '        Dim Trim_Target As Double                                       ' 補正後の目標抵抗値(目標値*(1+ΔR÷100)) ※ΔR指定なしのため未使用
    '        Dim Trim_Limit As Double                                        ' ﾄﾘﾐﾝｸﾞ目標補正値 ※ΔR指定なしのため未使用
    '        Dim Mean_Value As Double                                        ' ﾄﾘﾐﾝｸﾞされたﾁｯﾌﾟの平均抵抗値
    '        Dim _Par As Double                                              ' 上記の%表示
    '        Dim M_R As Double                                               ' 平均値の誤差
    '        Dim Prn3s_x As Double                                           ' ﾄﾘﾐﾝｸﾞされたﾁｯﾌﾟの誤差の標準偏差
    '        Dim STtime As Double                                            ' 開始時間(double)
    '        Dim EDtime As Double                                            ' 終了時間(double)
    '        Dim AlmSTtime As Double                                         ' ｱﾗｰﾑ停止開始時間(double)
    '        Dim AlmEDtime As Double                                         ' ｱﾗｰﾑ停止終了時間(double)
    '        Dim AlmCnt As Short                                             ' ｱﾗｰﾑ発生回数
    '        Dim AlmTotaltime As Double                                      ' ｱﾗｰﾑ停止ﾄｰﾀﾙ時間(double)
    '        Dim ChipTotal As Double                                         ' 1ﾛｯﾄ分の総抵抗数
    '        Dim Trim_NG As Integer                                          ' 不良品ﾁｯﾌﾟ数
    '        Dim Trim_TotalVal As Double                                     ' ﾄﾘﾐﾝｸﾞされたﾁｯﾌﾟの抵抗値(合計)
    '        Dim Trim_TotalValCnt As Double                                  ' ﾄﾘﾐﾝｸﾞされたﾁｯﾌﾟの抵抗値(計算用合計)
    '        Dim Trim_TotalValKT As Short                                    ' ﾄﾘﾐﾝｸﾞされたﾁｯﾌﾟの抵抗値(桁)
    '    End Structure

    '    Public stPRT_ROHM As TRIM_RESULT_PRINT_ROHM_INFO                    ' トリミング結果印刷用構造体(ローム殿用)

    '#End Region

    '#Region "罫線位置を設定する"
    '    '''=========================================================================
    '    ''' <summary>罫線位置を設定する</summary>
    '    ''' <param name="KWidth">     (INP)罫線印刷用ペン幅(mm)</param>
    '    ''' <param name="H_StaPosAry">(INP)横ライン開始座標X,Y配列</param>
    '    ''' <param name="H_EndPosAry">(INP)横ライン終了座標X,Y配列</param>
    '    ''' <param name="V_StaPosAry">(INP)縦ライン開始座標X,Y配列</param>
    '    ''' <param name="V_EndPosAry">(INP)縦ライン終了座標X,Y配列</param>
    '    ''' <returns>0 = 正常, 0以外=エラー</returns>
    '    '''=========================================================================
    '    Public Function Set_Keisen_Pos(ByVal KWidth As Double, ByVal H_StaPosAry(,) As Integer, ByVal H_EndPosAry(,) As Integer, ByVal V_StaPosAry(,) As Integer, ByVal V_EndPosAry(,) As Integer) As Integer

    '        Dim r As Integer
    '        Dim strMSG As String

    '        Try
    '            ' ローム殿仕様でなければNOP
    '            If (gSysPrm.stCTM.giSPECIAL <> customROHM) Then
    '                Return (cFRS_NORMAL)
    '            End If
    '            ' 印刷用オブジェクト未生成ならNOP
    '            If (ObjPrint Is Nothing) Then
    '                Return (cFRS_NORMAL)
    '            End If

    '            ' 印刷処理
    '            r = ObjPrint.Set_Keisen_Pos(KWidth, H_StaPosAry, H_EndPosAry, V_StaPosAry, V_EndPosAry)
    '            Return (r)

    '            ' トラップエラー発生時 
    '        Catch ex As Exception
    '            strMSG = "QR_Print.Print_Data() TRAP ERROR = " + ex.Message
    '            MsgBox(strMSG)
    '            Return (cERR_TRAP)
    '        End Try
    '    End Function
    '#End Region

    '#Region "トリミング結果結果印刷処理"
    '    '''=========================================================================
    '    ''' <summary>トリミング結果結果印刷処理</summary>
    '    ''' <param name="Md">(INP)0=通常印刷, 1=Printボタン押下</param>
    '    ''' <remarks></remarks>
    '    '''=========================================================================
    '    Public Sub PrnTrimResult(ByVal Md As Integer)

    '        Dim r As Integer
    '        Dim Idx As Integer
    '        Dim sLINE_TITLE(ITEM_MAX) As String                             ' 見出し項目
    '        Dim sLINE_DATA(ITEM_MAX) As String                              ' 印刷データ
    '        Dim strHEAD As String
    '        Dim strDAT As String
    '        Dim strMSG As String = ""

    '        Try
    '            '-------------------------------------------------------------------
    '            '   初期設定
    '            '-------------------------------------------------------------------
    '            ' ローム殿特注でなければNOP
    '            If (gSysPrm.stCTM.giSPECIAL <> customROHM) Then Return

    '            ' データ未ロードならばNOP
    '            If (gLoadDTFlag = False) Then Return

    '            ' 手動運転ならNOP
    '            If (stPRT_ROHM.bAutoMode = False) Then Return

    '            ' 印刷用の作業日がセットされていなければNOP
    '            If (stPRT_ROHM.DateR = "") Then Return

    '            ' 「Print ON/OFF」ボタンが「Print OFF」ならNOP
    '            If (gSysPrm.stDEV.rPrnOut_flg = False) Then Return

    '            ' トリミング結果印刷項目のデータをセットする(印刷前に設定可能なもの)
    '            Call SetTrimPrnData()

    '            '-------------------------------------------------------------------
    '            '   罫線印刷用データを設定する
    '            '-------------------------------------------------------------------
    '            ' 横ライン開始座標X,Y配列X(10, 10, .... 10, 10),Y(10, 15, ...., 120)
    '            H_StaPosAry = New Integer(1, H_LINE_NUM) _
    '            {{PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, _
    '              PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, _
    '              PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, _
    '              PR_H0_POSX, PR_H0_POSX}, _
    '             {PR_H00_POSY, PR_H01_POSY, PR_H02_POSY, PR_H03_POSY, PR_H04_POSY, PR_H05_POSY, PR_H06_POSY, PR_H07_POSY, PR_H08_POSY, PR_H09_POSY, _
    '              PR_H10_POSY, PR_H11_POSY, PR_H12_POSY, PR_H13_POSY, PR_H14_POSY, PR_H15_POSY, PR_H16_POSY, PR_H17_POSY, PR_H18_POSY, PR_H19_POSY, _
    '              PR_H20_POSY, PR_H21_POSY, PR_H22_POSY, PR_H23_POSY, PR_H24_POSY, PR_H25_POSY, PR_H26_POSY, PR_H27_POSY, PR_H28_POSY, PR_H29_POSY, _
    '              PR_H30_POSY, PR_H31_POSY}}

    '            ' 横ライン終了座標X,Y配列X(110, 110, .... 110, 110),Y(10, 15, ...., 120)
    '            H_EndPosAry = New Integer(1, H_LINE_NUM) _
    '            {{PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, _
    '              PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, _
    '              PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, _
    '              PR_V3_POSX, PR_V3_POSX}, _
    '             {PR_H00_POSY, PR_H01_POSY, PR_H02_POSY, PR_H03_POSY, PR_H04_POSY, PR_H05_POSY, PR_H06_POSY, PR_H07_POSY, PR_H08_POSY, PR_H09_POSY, _
    '              PR_H10_POSY, PR_H11_POSY, PR_H12_POSY, PR_H13_POSY, PR_H14_POSY, PR_H15_POSY, PR_H16_POSY, PR_H17_POSY, PR_H18_POSY, PR_H19_POSY, _
    '              PR_H20_POSY, PR_H21_POSY, PR_H22_POSY, PR_H23_POSY, PR_H24_POSY, PR_H25_POSY, PR_H26_POSY, PR_H27_POSY, PR_H28_POSY, PR_H29_POSY, _
    '              PR_H30_POSY, PR_H31_POSY}}

    '            ' 縦ライン開始座標X(10, 20, 65, 110),Y(10, ....., 10)配列
    '            V_StaPosAry = New Integer(1, V_LINE_NUM) _
    '            {{PR_V0_POSX, PR_V1_POSX, PR_V2_POSX, PR_V3_POSX}, _
    '             {PR_V0_POSY, PR_V0_POSY, PR_V0_POSY, PR_V0_POSY}}

    '            ' 縦ライン終了座標X(10, 20, 65, 110),Y(165, ....., 165)配列
    '            V_EndPosAry = New Integer(1, V_LINE_NUM) _
    '            {{PR_V0_POSX, PR_V1_POSX, PR_V2_POSX, PR_V3_POSX}, _
    '             {PR_H31_POSY, PR_H31_POSY, PR_H31_POSY, PR_H31_POSY}}

    '            ' 罫線位置を設定する
    '            r = Set_Keisen_Pos(KWidth, H_StaPosAry, H_EndPosAry, V_StaPosAry, V_EndPosAry)

    '            '-------------------------------------------------------------------
    '            '   印刷用見出し項目を設定する
    '            '-------------------------------------------------------------------
    '            Idx = 1
    '            sLINE_TITLE(Idx) = "" : Idx = Idx + 1                       '  1.作業日
    '            sLINE_TITLE(Idx) = "START TIME" : Idx = Idx + 1             '  2.開始時間
    '            sLINE_TITLE(Idx) = "STOP TIME" : Idx = Idx + 1              '  3.終了時間
    '            sLINE_TITLE(Idx) = "PROG TIME" : Idx = Idx + 1              '  4.開始～終了までに要した時間
    '            sLINE_TITLE(Idx) = "OPE TIME" : Idx = Idx + 1               '  5.稼動時間
    '            sLINE_TITLE(Idx) = "ALARM TIME" : Idx = Idx + 1             '  6.ｱﾗｰﾑにより停止した時間
    '            sLINE_TITLE(Idx) = "OPE RATE" : Idx = Idx + 1               '  7.稼働率
    '            sLINE_TITLE(Idx) = "MTBF" : Idx = Idx + 1                   '  8.平均故障間隔
    '            sLINE_TITLE(Idx) = "MTTR" : Idx = Idx + 1                   '  9.平均復旧時間
    '            sLINE_TITLE(Idx) = "LOT NO" : Idx = Idx + 1                 ' 10.QRコード内のロット番号
    '            sLINE_TITLE(Idx) = "Qrate" : Idx = Idx + 1                  ' 11.ﾄﾘﾐﾝｸﾞQﾚｰﾄ
    '            sLINE_TITLE(Idx) = "TrIdxm Speed" : Idx = Idx + 1           ' 12.ﾄﾘﾐﾝｸﾞｶｯﾄｽﾋﾟｰﾄﾞ
    '            sLINE_TITLE(Idx) = "Trim OK" : Idx = Idx + 1                ' 13.良品ﾁｯﾌﾟ数
    '            sLINE_TITLE(Idx) = "Pretest Lo Fail" : Idx = Idx + 1        ' 14.初期値下限不良のﾁｯﾌﾟ数
    '            sLINE_TITLE(Idx) = "Pretest Hi Fail" : Idx = Idx + 1        ' 15.初期値上限不良のﾁｯﾌﾟ数
    '            sLINE_TITLE(Idx) = "Pretest Open" : Idx = Idx + 1           ' 16.初期値ｵｰﾌﾟﾝ不良のﾁｯﾌﾟ数
    '            'sLINE_TITLE(Idx) = "Cut NG": Idx = Idx + 1                 '   .ﾄﾘﾐﾝｸﾞ時に目標値に達しなかったﾁｯﾌﾟ数
    '            sLINE_TITLE(Idx) = "Pretest NG" : Idx = Idx + 1             ' 17.初期不良
    '            sLINE_TITLE(Idx) = "Final test Lo Fail" : Idx = Idx + 1     ' 18.ﾄﾘﾐﾝｸﾞ後の下限不良のﾁｯﾌﾟ数
    '            sLINE_TITLE(Idx) = "Final test Hi Fail" : Idx = Idx + 1     ' 19.ﾄﾘﾐﾝｸﾞ後の上限不良のﾁｯﾌﾟ数
    '            sLINE_TITLE(Idx) = "Final test Open" : Idx = Idx + 1        ' 20.ﾄﾘﾐﾝｸﾞ後にｵｰﾌﾟﾝｴﾗｰとなったﾁｯﾌﾟ数
    '            sLINE_TITLE(Idx) = "Yield" : Idx = Idx + 1                  ' 21.良品ﾁｯﾌﾟ数÷ﾁｯﾌﾟ数
    '            sLINE_TITLE(Idx) = "Yield(%)" : Idx = Idx + 1               ' 22.上記の%表示
    '            sLINE_TITLE(Idx) = "Pdt Sheet" : Idx = Idx + 1              ' 23.ﾄﾘﾐﾝｸﾞｽﾃｰｼﾞで処理した基板枚数
    '            sLINE_TITLE(Idx) = "Lot Sheet" : Idx = Idx + 1              ' 24.装置に投入されたﾛｯﾄ枚数
    '            sLINE_TITLE(Idx) = "Lot NG Sheet" : Idx = Idx + 1           ' 25.ﾛｯﾄ中の不良基板数
    '            sLINE_TITLE(Idx) = "Edg Fail" : Idx = Idx + 1               ' 26.ﾛｯﾄ中の認識不良基板枚数
    '            sLINE_TITLE(Idx) = "Nominal" : Idx = Idx + 1                ' 27.目標抵抗値
    '            'sLINE_TITLE(Idx) = "Trim Target" : Idx = Idx + 1            ' 補正後の目標抵抗値
    '            'sLINE_TITLE(Idx) = "Trim Limit" : Idx = Idx + 1             ' ﾄﾘﾐﾝｸﾞ目標補正値
    '            sLINE_TITLE(Idx) = "Mean Value" : Idx = Idx + 1             ' 28.ﾄﾘﾐﾝｸﾞされたﾁｯﾌﾟの平均抵抗値
    '            sLINE_TITLE(Idx) = "%" : Idx = Idx + 1                      ' 29.上記の%表示
    '            sLINE_TITLE(Idx) = "M/R" : Idx = Idx + 1                    ' 30.平均値の誤差
    '            sLINE_TITLE(Idx) = "S/_x" : Idx = Idx + 1                   ' 31.ﾄﾘﾐﾝｸﾞされたﾁｯﾌﾟの誤差の標準偏差

    '            '-------------------------------------------------------------------
    '            '   印刷データ用を設定する
    '            '-------------------------------------------------------------------
    '            Idx = 1
    '            With stPRT_ROHM
    '                sLINE_DATA(Idx) = .DateR : Idx = Idx + 1                                ' 1.作業日
    '                sLINE_DATA(Idx) = .START_TIME : Idx = Idx + 1                           ' 2.開始時間
    '                sLINE_DATA(Idx) = .STOP_TIME : Idx = Idx + 1                            ' 3.終了時間
    '                sLINE_DATA(Idx) = .PROG_TIME : Idx = Idx + 1                            ' 4.開始～終了までに要した時間
    '                sLINE_DATA(Idx) = .OPE_TIME : Idx = Idx + 1                             ' 5.稼動時間
    '                sLINE_DATA(Idx) = .ALARM_TIME : Idx = Idx + 1                           ' 6.ｱﾗｰﾑにより停止した時間
    '                sLINE_DATA(Idx) = .OPE_RATE : Idx = Idx + 1                             ' 7.稼働率
    '                sLINE_DATA(Idx) = .MTBF : Idx = Idx + 1                                 ' 8.平均故障間隔
    '                sLINE_DATA(Idx) = .MTTR : Idx = Idx + 1                                 ' 9.平均復旧時間
    '                sLINE_DATA(Idx) = .LOT_NO : Idx = Idx + 1                               '10.QRコード内のロット番号
    '                sLINE_DATA(Idx) = .Qrate : Idx = Idx + 1                                '11.ﾄﾘﾐﾝｸﾞQﾚｰﾄ
    '                sLINE_DATA(Idx) = .Trim_Speed : Idx = Idx + 1                           '12.ﾄﾘﾐﾝｸﾞｶｯﾄｽﾋﾟｰﾄﾞ
    '                sLINE_DATA(Idx) = .Trim_OK.ToString("0") : Idx = Idx + 1                '13.良品ﾁｯﾌﾟ数
    '                sLINE_DATA(Idx) = .Pretest_Lo_Fail.ToString("0") : Idx = Idx + 1        '14.初期値下限不良のﾁｯﾌﾟ数
    '                sLINE_DATA(Idx) = .Pretest_Hi_Fail.ToString("0") : Idx = Idx + 1        '15.初期値上限不良のﾁｯﾌﾟ数
    '                sLINE_DATA(Idx) = .Pretest_Open.ToString("0") : Idx = Idx + 1           '16.初期値ｵｰﾌﾟﾝ不良のﾁｯﾌﾟ数
    '                'sLINE_DATA(Idx) = .Cut_NG.ToString("0"): Idx = Idx + 1                 ' ﾄﾘﾐﾝｸﾞ時に目標値に達しなかったﾁｯﾌﾟ数
    '                sLINE_DATA(Idx) = .Pretest_NG_Cut_NG.ToString("0") : Idx = Idx + 1      '17.初期不良
    '                sLINE_DATA(Idx) = .Final_test_Lo_Fail.ToString("0") : Idx = Idx + 1     '18.ﾄﾘﾐﾝｸﾞ後の下限不良のﾁｯﾌﾟ数
    '                sLINE_DATA(Idx) = .Final_test_Hi_Fail.ToString("0") : Idx = Idx + 1     '19.ﾄﾘﾐﾝｸﾞ後の上限不良のﾁｯﾌﾟ数
    '                sLINE_DATA(Idx) = .Final_test_Open.ToString("0") : Idx = Idx + 1        '20.ﾄﾘﾐﾝｸﾞ後にｵｰﾌﾟﾝｴﾗｰとなったﾁｯﾌﾟ数
    '                sLINE_DATA(Idx) = .Yield : Idx = Idx + 1                                '21.良品ﾁｯﾌﾟ数÷ﾁｯﾌﾟ数
    '                sLINE_DATA(Idx) = .Yield_Par.ToString("0.000") & "%" : Idx = Idx + 1    '22.上記の%表示
    '                sLINE_DATA(Idx) = .Pdt_Sheet.ToString("0") : Idx = Idx + 1              '23.ﾄﾘﾐﾝｸﾞｽﾃｰｼﾞで処理した基板枚数
    '                sLINE_DATA(Idx) = .Lot_Sheet.ToString("0") : Idx = Idx + 1              '24.装置に投入されたﾛｯﾄ枚数
    '                sLINE_DATA(Idx) = .Lot_NG_Sheet.ToString("0") : Idx = Idx + 1           '25.ﾛｯﾄ中の不良基板数
    '                sLINE_DATA(Idx) = .Edg_Fail.ToString("0") : Idx = Idx + 1               '26.ﾛｯﾄ中の認識不良基板枚数
    '                sLINE_DATA(Idx) = .Nominal.ToString("0.00000") : Idx = Idx + 1          '27.目標抵抗値
    '                'sLINE_DATA(Idx) = .Trim_Target.ToString("0.00000") : Idx = Idx + 1      ' 補正後の目標抵抗値
    '                'sLINE_DATA(Idx) = .Trim_Limit.ToString("0.00") : Idx = Idx + 1          ' ﾄﾘﾐﾝｸﾞ目標補正値
    '                sLINE_DATA(Idx) = .Mean_Value.ToString("0.00000") : Idx = Idx + 1       '28.ﾄﾘﾐﾝｸﾞされたﾁｯﾌﾟの平均抵抗値
    '                sLINE_DATA(Idx) = ._Par.ToString("0.000") & "%" : Idx = Idx + 1         '29.上記の%表示
    '                sLINE_DATA(Idx) = .M_R.ToString("0.000") & "%" : Idx = Idx + 1          '30.平均値の誤差
    '                sLINE_DATA(Idx) = .Prn3s_x.ToString("0.000") & "%" : Idx = Idx + 1      '31.ﾄﾘﾐﾝｸﾞされたﾁｯﾌﾟの誤差の標準偏差
    '            End With

    '            '-------------------------------------------------------------------
    '            '   印刷用テキストボックス(TxtBoxPrint(非表示))に出力する
    '            '-------------------------------------------------------------------
    '            ' 見出し設定 ("Date/Time : " & Date & "  " & Time + ファイル名)
    '            Sub_GetFileName(gStrTrimFileName, strMSG)                   ' ファイル名のみを取り出す 
    '            strHEAD = "Date/Time : " & Today.ToString("yyyy/MM/dd") & "  " & TimeOfDay.ToString("HH:mm:ss") & "    " & strMSG

    '            ' 印刷用テキストボックスに出力
    '            For Idx = 1 To ITEM_MAX                                     ' 最大項目数分繰り返す
    '                ' No.
    '                strMSG = Idx.ToString("0")
    '                strDAT = strMSG.PadLeft(4) + "  "                       ' No.    (4文字 左側に空白パディング)
    '                sLINE_TITLE(Idx) = sLINE_TITLE(Idx).PadRight(25)        ' 見出し(25文字 右側に空白パディング)
    '                sLINE_DATA(Idx) = sLINE_DATA(Idx).PadRight(30)          ' 項目　(30文字 右側に空白パディング)
    '                ' No.x + 見出し + 項目
    '                strDAT = strDAT & sLINE_TITLE(Idx) & "  " & sLINE_DATA(Idx) & vbCrLf
    '                ' 印刷用テキストボックスに出力
    '                If (Idx = 1) Then
    '                    Form1.TxtBoxPrint.Text = strDAT
    '                Else
    '                    Form1.TxtBoxPrint.AppendText(strDAT)
    '                End If
    '            Next Idx

    '            '-------------------------------------------------------------------
    '            '   印刷処理(テキストボックス(TxtBoxPrint)の文字列を印刷する)
    '            '-------------------------------------------------------------------
    '            r = Print_Data(Form1.TxtBoxPrint.Text, strHEAD, HEAD_POSX, HEAD_POSY)
    '            If (r <> cFRS_NORMAL) Then
    '                ' "印刷処理に失敗しました。(r=xxxx)"
    '                strMSG = MSG_153 + "(r = " + r.ToString("0") + ")"
    '                Call MsgBox(strMSG, vbOKOnly)
    '            End If

    '            ' トラップエラー発生時 
    '        Catch ex As Exception
    '            strMSG = "QR_Print.ClrTrimPrnData() TRAP ERROR = " + ex.Message
    '            MsgBox(strMSG)
    '        End Try
    '    End Sub
    '#End Region
#End Region

#End Region
End Module
