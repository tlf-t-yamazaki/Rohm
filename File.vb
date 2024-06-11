'===============================================================================
'   Description  : TKY,CHIP,NETのトリミングデータファイル読出し処理
'
'   Copyright(C) : OMRON LASERFRONT INC. 2010
'
'===============================================================================
Option Strict Off
Option Explicit On

Imports System.Collections.Generic      'V5.0.0.9②
Imports System.IO                       'V4.4.0.0-0
Imports System.Text                     'V4.4.0.0-0
Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8①
Imports LaserFront.Trimmer.TrimData.FileIO          'V5.0.0.8①
Imports TKY_ALL_SL432HW.My.Resources    'V4.4.0.0-0

Module File
#Region "【ローカル定数/変数の定義】"
    '---------------------------------------------------------------------------
    '   ファイルバージョン名
    '---------------------------------------------------------------------------
    '----- ファイルバージョン名 -----
    'Private SL432HW_FileVer As Short
    'V5.0.0.8①    Private SL432HW_FileVer As Double                       ' ###066
    Private gStrTkyFileVer As String                        ' TKY用
#If False Then 'V5.0.0.8① LOAD・SAVEの共通化によりTrimDataEditorで定義(旧バージョン用の定義のみ残し)
    Private NewFileVer As Integer                           ' 統合ソフト版バージョン名
    Public Const FILE_VER_10 As Double = 10.0               ' 統合ソフト版
    Public Const FILE_VER_10_01 As Double = 10.01           ' 統合ソフト版 ###229
    Public Const FILE_VER_10_02 As Double = 10.02           ' 統合ソフト版 ###229
    Public Const FILE_VER_10_03 As Double = 10.03           ' 統合ソフト版 ###229
    Public Const FILE_VER_10_04 As Double = 10.04           ' 統合ソフト版 V1.14.0.0①
    Public Const FILE_VER_10_05 As Double = 10.05           ' 統合ソフト版 V1.14.0.0①
    Public Const FILE_VER_10_06 As Double = 10.06           ' 統合ソフト版 V1.16.0.0①
    Public Const FILE_VER_10_07 As Double = 10.07           ' 統合ソフト版 V1.18.0.0④
    Public Const FILE_VER_10_072 As Double = 10.072         ' 統合ソフト版 V2.0.0.0_24
    Public Const FILE_VER_10_073 As Double = 10.073         ' 統合ソフト版 V2.0.0.0_24
    '                                                       '
    Public Const FILE_VER_10_08 As Double = 10.08           ' 統合ソフト版(ノリタケ殿特注 未サポート)
    Public Const FILE_VER_10_09 As Double = 10.09           ' 統合ソフト版 V1.23.0.0②
    Public Const FILE_VER_10_10 As Double = 10.1            ' 統合ソフト版 V4.0.0.0④
    Public Const FILE_VER_10_11 As Double = 10.11           ' 統合ソフト版 V4.11.0.0①
    Public Const FILE_VER_CUR As Double = 10.11             ' 統合ソフト版(現在版名) V4.11.0.0①
#End If
    '----- TKY用 -----
    Private Const CONST_FILETYPE4 As String = "TKYDATA_Ver4"
    Private Const CONST_FILETYPE4_SP1 As String = "TKYDATA_Ver4_SP1"
    Private Const CONST_FILETYPE5 As String = "TKYDATA_Ver5"
    Private Const CONST_FILETYPE6 As String = "TKYDATA_Ver6"
#If False Then 'V5.0.0.8① LOAD・SAVEの共通化によりTrimDataEditorで定義(旧バージョン用の定義のみ残し)
    Private Const CONST_FILETYPE10 As String = "TKYDATA_Ver10.00"       ' 統合ソフト版
    Private Const CONST_FILETYPE10_01 As String = "TKYDATA_Ver10.01"    ' 統合ソフト版
    Private Const CONST_FILETYPE10_02 As String = "TKYDATA_Ver10.02"    ' 統合ソフト版(NEC SHOTT用)
    Private Const CONST_FILETYPE10_03 As String = "TKYDATA_Ver10.03"    ' 統合ソフト版 V1.13.0.0②
    Private Const CONST_FILETYPE10_04 As String = "TKYDATA_Ver10.04"    ' 統合ソフト版 V1.14.0.0①
    Private Const CONST_FILETYPE10_05 As String = "TKYDATA_Ver10.05"    ' 統合ソフト版 V1.16.0.0①
    Private Const CONST_FILETYPE10_06 As String = "TKYDATA_Ver10.06"    ' 統合ソフト版 V1.18.0.0④
    Private Const CONST_FILETYPE10_07 As String = "TKYDATA_Ver10.07"    ' 統合ソフト版 V1.16.0.0①
    Private Const CONST_FILETYPE10_072 As String = "TKYDATA_Ver10.072"  ' 統合ソフト版 V2.0.0.0_24
    Private Const CONST_FILETYPE10_073 As String = "TKYDATA_Ver10.073"  ' 統合ソフト版 V2.0.0.0_24
    '                            
    Private Const CONST_FILETYPE10_08 As String = "TKYDATA_Ver10.08"    ' 統合ソフト版(ノリタケ殿特注 未サポート)
    Private Const CONST_FILETYPE10_09 As String = "TKYDATA_Ver10.09"    ' 統合ソフト版 V1.23.0.0②
    Private Const CONST_FILETYPE10_10 As String = "TKYDATA_Ver10.10"    ' 統合ソフト版  V4.0.0.0④
    Private Const CONST_FILETYPE10_11 As String = "TKYDATA_Ver10.11"    ' 統合ソフト版   V4.11.0.0①
    'Private Const CONST_FILETYPE_CUR As String = "TKYDATA_Ver10.00"    ' 統合ソフト版(現在版名) V4.11.0.0①
    Private Const CONST_FILETYPE_CUR As String = "TKYDATA_Ver10.11"     ' 統合ソフト版(現在版名) V4.11.0.0①
    '----- CHIP/NET用 -----
    Private FILETYPE01 As String                            ' 1次出荷Ver
    Private FILETYPE02 As String                            ' 2次出荷Ver
    Private FILETYPE03 As String                            ' 3次出荷Ver
    Private FILETYPE04 As String                            ' 4次出荷Ver
    Private FILETYPE05 As String                            ' 5次出荷Ver
    Private FILETYPE06 As String                            ' 6次出荷Ver
    Private FILETYPE07_02 As String                         ' 7次出荷VerVer7.0.0.2 V1.14.0.0⑥
    Private FILETYPE10 As String                            ' 統合ソフト版
    Private FILETYPE10_01 As String                         ' 統合ソフト版
    Private FILETYPE10_02 As String                         ' 統合ソフト版
    Private FILETYPE10_03 As String                         ' 統合ソフト版 V1.13.0.0②
    Private FILETYPE10_04 As String                         ' 統合ソフト版 V1.14.0.0①
    Private FILETYPE10_05 As String                         ' 統合ソフト版 V1.16.0.0①
    Private FILETYPE10_06 As String                         ' 統合ソフト版 V1.18.0.0④
    Private FILETYPE10_07 As String                         ' 統合ソフト版 V1.18.0.0④
    Private FILETYPE10_072 As String                        ' 統合ソフト版 V2.0.0.0_24
    Private FILETYPE10_073 As String                        ' 統合ソフト版 V2.0.0.0_24
    Private FILETYPE10_08 As String                         ' 統合ソフト版(ノリタケ殿特注 未サポート)
    Private FILETYPE10_09 As String                         ' 統合ソフト版 V1.23.0.0②
    Private FILETYPE10_10 As String                         ' 統合ソフト版 V4.0.0.0④
    Private FILETYPE10_11 As String                         ' 統合ソフト版 V4.11.0.0①
    Private FILETYPE_CUR As String                          ' 統合ソフト版(現在版名) V4.0.0.0④ ###066
#End If
    'V5.0.0.8① 追加           ↓
    Private Const FILETYPE_CHIP01 As String = "TKYCHIP_SL432HW_Ver1.00"   ' 1次出荷Ver
    Private Const FILETYPE_CHIP02 As String = "TKYCHIP_SL432HW_Ver1.10"   ' 2次出荷Ver
    Private Const FILETYPE_CHIP03 As String = "TKYCHIP_SL432HW_Ver1.20"   ' 3次出荷Ver
    Private Const FILETYPE_CHIP04 As String = "TKYCHIP_SL432HW_Ver1.30"   ' 4次出荷Ver
    Private Const FILETYPE_CHIP05 As String = "TKYCHIP_SL432HW_Ver1.40"   ' 5次出荷Ver
    Private Const FILETYPE_CHIP06 As String = "TKYCHIP_SL432HW_Ver1.50"   ' 6次出荷Ver
    Private Const FILETYPE_CHIP07_02 As String = "TKYCHIP_SL432HW_Ver7.0.0.2" 'V5.0.0.0-22

    Private Const FILETYPE_NET01 As String = "TKYNET_SL432HW_Ver1.00"     ' 1次出荷Ver
    Private Const FILETYPE_NET02 As String = "TKYNET_SL432HW_Ver1.01"     ' 2次出荷Ver
    Private Const FILETYPE_NET03 As String = "TKYNET_SL432HW_Ver1.02"     ' 3次出荷Ver
    Private Const FILETYPE_NET07_02 As String = "TKYNET_SL432HW_Ver7.0.0.2"    'V5.0.0.0-22
    'V5.0.0.8① 追加           ↑

    '----- V1.23.0.0⑧↓ -----
    '----- CHIP/NET用(SL436K) -----
    Private FILETYPE_K As String = ""                           ' SL436Kのファイルバージョン
    Private FILETYPE01_K As String = "TKYCHIP_SL436K_Ver0.00"   ' 1次出荷Ver
    Private FILETYPE02_K As String = "TKYCHIP_SL436K_Ver1.00"   ' 2次出荷Ver
    Private FILETYPE03_K As String = "TKYCHIP_SL436K_Ver1.10"   ' 3次出荷Ver
    Private FILETYPE04_K As String = "TKYCHIP_SL436K_Ver1.20"   ' 4次出荷Ver
    '----- V1.23.0.0⑧↑ -----
#Region "LOAD・SAVEの共通化によりTrimDataEditorで定義"
#If False Then 'V5.0.0.8①
    '---------------------------------------------------------------------------
    '   セクション名
    '---------------------------------------------------------------------------
    '----- TKY用 -----
    Private Const CONST_VERSION As String = "[FILE VERSION]"
    Private Const CONST_PLATE As String = "[PLATE]"
    Private Const CONST_PLATE1 As String = "[PLATE1]"
    Private Const CONST_PLATE2 As String = "[PLATE2]"
    Private Const CONST_PLATE3 As String = "[PLATE3]"
    Private Const CONST_CIRCUIT As String = "[CIRCUIT]"
    Private Const CONST_RESISTOR_DATA As String = "[RESIST]"
    Private Const CONST_CUT_DATA As String = "[CUT]"
    Private Const CONST_IKEI_DATA As String = "[IKEI]"
    Private Const CONST_CRC As String = "C="
    Private Const CONST_SINSYUKU_SELECT As String = "[SINSYUKU]"      'V1.13.0.0⑤

    '----- CHIP用/NET用 -----
    Private Const FILE_CONST_VERSION As String = "[FILE VERSION]"
    Private Const FILE_CONST_PLATE_01 As String = "[PLATE01]"
    Private Const FILE_CONST_PLATE_02 As String = "[PLATE02]"
    Private Const FILE_CONST_PLATE_03 As String = "[PLATE03]"
    Private Const FILE_CONST_PLATE_04 As String = "[PLATE04]"
    Private Const FILE_CONST_PLATE_05 As String = "[PLATE05]"
    Private Const FILE_CONST_PLATE_06 As String = "[PLATE06]"       ' V1.13.0.0②
    Private Const FILE_CONST_STEPDATA As String = "[STEP]"
    Private Const FILE_CONST_RESISTOR As String = "[RESISTOR]"
    Private Const FILE_CONST_CUT_DATA As String = "[CUT]"
    Private Const FILE_CONST_PLATE_OPTION As String = "[OPTION]"     'V5.0.0.6①
    '----- CHIP用 -----
    Private Const FILE_CONST_GRP_DATA As String = "[GROUP]"
    Private Const FILE_CONST_TY2_DATA As String = "[TY2]"
    '----- NET用 -----
    Private Const FILE_CONST_CIR_DATA As String = "[CIRCUIT]"       ' 統合ソフト版ではTKY用
    Private Const FILE_CONST_CIRN_DATA As String = "[CIRCUIT AXIS]" ' 統合ソフト版ではNET用
    Private Const FILE_CONST_CIRIDATA As String = "[CIRCUIT INTERVAL]"
    '----- ###229↓ -----
    '----- TKY/CHIP/NET用 -----
    Private Const FILE_CONST_GPIB_DATA As String = "[GPIB]"       ' GPIBデータ
    '----- ###229↑ -----
    Private Const FILE_SINSYUKU_SELECT As String = "[SINSYUKU]"      'V1.13.0.0⑤

    '----- データ種別定義(統合版) -----
    Private Const SECT_VERSION As Integer = 0                   ' ファイルバージョン
    Private Const SECT_PLATE01 As Integer = 1                   ' プレートデータ１
    Private Const SECT_PLATE02 As Integer = 2                   ' プレートデータ２
    Private Const SECT_PLATE03 As Integer = 3                   ' プレートデータ３
    Private Const SECT_PLATE04 As Integer = 4                   ' プレートデータ４
    Private Const SECT_PLATE05 As Integer = 5                   ' プレートデータ５
    Private Const SECT_PLATE06 As Integer = 16                  ' プレートデータ６ V1.13.0.0②
    Private Const SECT_PLATE_OPTION As Integer = 18             ' オプション 'V5.0.0.6①
    Private Const SECT_CIRCUIT As Integer = 6                   ' サーキットデータ(TKY用) 
    Private Const SECT_STEP As Integer = 7                      ' STEPデータ(CHIP用)
    Private Const SECT_GRP_DATA As Integer = 8                  ' グループデータ(CHIP用)
    Private Const SECT_TY2_DATA As Integer = 9                  ' TY2データ(CHIP用)
    Private Const SECT_CIR_AXIS As Integer = 10                 ' サーキット座標データ(NET用)
    Private Const SECT_CIR_ITVL As Integer = 11                 ' サーキット間インターバルデータ(NET用)
    Private Const SECT_IKEI_DATA As Integer = 12                ' 異形面付け(TKY用) 
    Private Const SECT_REGISTOR As Integer = 13                 ' 抵抗データ
    Private Const SECT_CUT_DATA As Integer = 14                 ' カットデータ
    Private Const SECT_GPIB_DATA As Integer = 15                ' GPIBデータ ###229
    Private Const SECT_SINSYUKU_DATA As Integer = 17            ' 伸縮補正用データ      'V1.13.0.0⑤
#End If
#End Region 'V5.0.0.8①
    '---------------------------------------------------------------------------
    '   その他の変数定義
    '---------------------------------------------------------------------------
    'Private Const MAX_PDATA As Integer = 100                    ' ﾛｰﾄﾞするﾌﾟﾚｰﾄﾃﾞｰﾀﾚｺｰﾄﾞ最大数'V1.13.0.0②
    'Private Const MAX_PDATA As Integer = 128                    ' ﾛｰﾄﾞするﾌﾟﾚｰﾄﾃﾞｰﾀﾚｺｰﾄﾞ最大数'V1.13.0.0②   'V5.0.0.9②
    Private Const MAX_PGPIBDATA As Integer = 20                 ' ﾛｰﾄﾞするGPIBﾃﾞｰﾀﾚｺｰﾄﾞ最大数 ###229
    'Private pData(100) As String                                ' ﾛｰﾄﾞしたﾌﾟﾚｰﾄﾃﾞｰﾀを格納'V1.13.0.0②
    'Private pData(128) As String                                ' ﾛｰﾄﾞしたﾌﾟﾚｰﾄﾃﾞｰﾀを格納 'V1.13.0.0②       'V5.0.0.9②
    Private pGpibData(20) As String                             ' ﾛｰﾄﾞしたGPIBﾃﾞｰﾀを格納 ###229

#End Region

#Region "【ファイルリード/ライト処理メソッド】"
    '======================================================================
    '  TKY用メソッド
    '======================================================================
#Region "【TKY用メソッド】"
#Region "LOAD・SAVEの共通化により未使用"
#If False Then 'V5.0.0.8①
#Region "古いフォーマットファイルのコンバート処理【TKY用】"
    '''=========================================================================
    '''<summary>古いフォーマットファイルのコンバート処理【TKY用】</summary>
    '''<param name="sp">(INP) ファイル名</param>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Public Function DatConv_TKY(ByVal sp As String) As Integer

        Dim strD As String
        'Dim mPath As String

        DatConv_TKY = 0

        If sp = "" Then
            MsgBox(MSG_15, vbOKOnly, gAppName)
            DatConv_TKY = 1
            Exit Function
        End If

        strD = Right(sp, 3)
        Select Case strD
            Case "WDT", "wdt"
                sp = sp

                '----- V4.0.0.0⑦↓ -----
                'Case "DAT", "dat"
                '    ' SL436H
                '    mPath = Mid(sp, 1, Len(sp) - 3)
                '    mPath = mPath & "WDT"
                '    DatConv_TKY = Form1.DatConvert.DatConvert(sp, mPath)

                '    If DatConv_TKY Then                     ' エラー ? 
                '        Debug.Print("DatConvert error section =" + Form1.DatConvert.ErrorSection)
                '        Debug.Print("DatConvert error item No =" + Form1.DatConvert.ErrorItemNo)
                '        Debug.Print("DatConvert error msg     =" + Form1.DatConvert.ErrorMessage)
                '        Debug.Print("DatConvert error line    =" + Form1.DatConvert.ErrorLine)
                '        MsgBox(Form1.DatConvert.ErrorMessage & " Line=" & CStr(Form1.DatConvert.ErrorLine))
                '        Exit Function
                '    End If
                '    sp = mPath
                '----- V4.0.0.0⑦↑ -----
        End Select
    End Function
#End Region
#End If
#End Region 'V5.0.0.8①

#Region "ファイルロード処理【TKY用】"
    '''=========================================================================
    '''<summary>ファイルロード処理【TKY用】</summary>
    '''<param name="pPath">(INP) ファイル名</param>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Public Function File_Read_Tky(ByVal pPath As String) As Integer

        'Dim intFileNo As Integer                        ' ファイル番号
        Dim strBuff As String                           ' 読み込みデータバッファ
        Dim intType As Integer                          ' データ種別
        Dim Err_num As Integer                          ' エラー種別(1:対象外ファイル、)
        Dim rCnt As Integer
        'Dim iFlg As Integer

        'On Error GoTo ErrTrap

        ' 初期処理
        File_Read_Tky = 0
        Err_num = 0
        'iFlg = 0
        Call Init_AllTrmmingData()                      ' グローバルデータ初期化(Plate/Circuit/Resistor/Cut)
        intType = -1
        rCnt = 0

        Try
            ' テキストファイルをオープン
            'intFileNo = FreeFile()                          ' 使用可能なファイルナンバーを取得
            If (False = IO.File.Exists(pPath)) Then Throw New FileNotFoundException() 'V4.4.0.0-1
            'FileOpen(intFileNo, pPath, OpenMode.Input)
            'iFlg = 1
            Using sr As New StreamReader(pPath, Encoding.GetEncoding("Shift_JIS"))  ' 旧ﾌｧｲﾙなので Shift_JIS    V4.4.0.0-1
                ' ファイルの終端までループを繰り返します。
                'Do While Not EOF(intFileNo)
                Do While (False = sr.EndOfStream)               'V4.4.0.0-1

                    'strBuff = LineInput(intFileNo)              ' 1行読み込み
                    strBuff = sr.ReadLine()                     ' 1行読み込み        'V4.4.0.0-1
                    ' データ種別判定
                    Select Case strBuff
                        Case CONST_VERSION                      ' ファイルバージョン
                            intType = 0
                        Case CONST_PLATE                        ' データ名
                            intType = 1
                        Case CONST_PLATE1                       ' プレートデータ１
                            intType = 2
                        Case CONST_PLATE2                       ' プレートデータ２
                            intType = 3
                        Case CONST_PLATE3                       ' プレートデータ３
                            intType = 4
                        Case CONST_CIRCUIT                      ' サーキットデータ
                            intType = 5 : rCnt = 1
                        Case CONST_RESISTOR_DATA                ' 抵抗データ
                            intType = 6 : rCnt = 1
                        Case CONST_CUT_DATA                     ' カットデータ
                            intType = 7 : rCnt = 0
                        Case CONST_IKEI_DATA                    ' 異形面付け
                            Err_num = 1
                            GoTo ErrTrap
                        Case Else
                            If Left(strBuff, 2) = CONST_CRC Then
                                'GoTo EndFile
                                Exit Function
                            End If

                            Select Case intType                 ' データ種別
                                Case -1
                                    ' 対象ファイルではない
                                    Err_num = 1
                                    GoTo ErrTrap
                                Case 0
                                    ' ヴァージョンチェック
                                    Select Case strBuff
                                        Case CONST_FILETYPE4
                                            gStrTkyFileVer = CONST_FILETYPE4

                                        Case CONST_FILETYPE4_SP1
                                            ' 異形面付けは特注対応の為、対応しない
                                            Err_num = 1
                                            GoTo ErrTrap
                                        Case CONST_FILETYPE5
                                            '----- V1.13.0.0②↓ -----
                                            ' プローブ接触位置確認オプションなしでも読み込む
                                            gStrTkyFileVer = CONST_FILETYPE5
                                            'If gSysPrm.stSPF.gblnUseProbePosChk = True Then
                                            '    gStrTkyFileVer = CONST_FILETYPE5
                                            'Else
                                            '    Err_num = 1
                                            '    GoTo ErrTrap
                                            'End If
                                            '----- V1.13.0.0②↑ -----
                                        Case Else
                                            Err_num = 1
                                            GoTo ErrTrap
                                    End Select
                                Case 1 To 4
                                    ' プレートデータをグローバルセット
                                    If Set_typPlateInfoTky(strBuff, intType) < 0 Then GoTo ErrTrap
                                Case 5
                                    ' サーキットデータをグローバルセット
                                    If Set_typCircuitInfoArray(strBuff, rCnt) < 0 Then GoTo ErrTrap
                                    rCnt = rCnt + 1
                                Case 6
                                    ' 抵抗データをグローバルセット
                                    If Set_typResistorInfoArray(strBuff, rCnt) < 0 Then GoTo ErrTrap
                                    'typPlateInfo.intResistCntInGroup = rCnt     ' 現在の抵抗データ件数をセット
                                    typPlateInfo.intResistCntInBlock = rCnt     ' 現在の抵抗データ件数をセット
                                    gRegistorCnt = rCnt
                                    rCnt = rCnt + 1
                                    System.Windows.Forms.Application.DoEvents()
                                Case 7
                                    ' カットデータをグローバルセット
                                    If Set_typCutInfoArray(strBuff, rCnt) < 0 Then GoTo ErrTrap
                                    System.Windows.Forms.Application.DoEvents()
                                Case 8
                                    ' 異形面付けデータをグローバルセット
                                    If Set_typIKEIInfo(strBuff) < 0 Then GoTo ErrTrap

                            End Select
                    End Select
                Loop

            End Using

            ' 終了処理
EndFile:
            'If (iFlg = 1) Then
            '    FileClose(intFileNo)
            'End If
            'On Error GoTo 0
            Exit Function

            'ErrExit:
            '            File_Read_Tky = 1
            '            Select Case Err_num
            '                Case 1  ' 対象外ファイル
            '                    ' "指定されたファイルはトリミングパラメータのデータではありません"
            '                    Call Form1.System1.TrmMsgBox(gSysPrm, MSG_16, vbExclamation Or vbOKOnly, gAppName)

            '                Case Else
            '                    ' メッセージ設定
            '                    'If (gSysPrm.stTMN.giMsgTyp = 0) Then
            '                    '    MsgBox("##ｴﾗｰ情報<DATA LOAD>##" & vbCrLf & "  ｴﾗｰ番号=" & Str(Err_num))
            '                    'Else
            '                    '    MsgBox("##ERROR INFO<DATA LOAD>##" & vbCrLf & "  Err_nun=" & Str(Err_num))
            '                    'End If
            '                    MsgBox("##" & File_001 & "<DATA LOAD>##" & vbCrLf & "  " & File_002 & "=" & Str(Err_num))
            '            End Select

            'On Error GoTo 0
            'GoTo EndFile

ErrTrap:
            File_Read_Tky = 1
            'Select Case Err_num
            '    Case 1  '対象外ファイル
            ' "指定されたファイルはトリミングパラメータのデータではありません"
            Call Form1.System1.TrmMsgBox(gSysPrm, MSG_16, vbExclamation Or vbOKOnly, gAppName)

            '    Case Else
            '        If Err.Number = 53 Then
            '        ElseIf Err.Number <> 0 Then
            '            ' メッセージ設定
            '            'If (gSysPrm.stTMN.giMsgTyp = 0) Then
            '            '    MsgBox("##ｴﾗｰ情報<DATA LOAD>##" & vbCrLf & _
            '            '                               "  ｴﾗｰ内容" & vbTab & " : " & Err.Description & vbCrLf & _
            '            '                               "  ｴﾗｰ番号" & vbTab & " : " & Err.Number & vbCrLf)
            '            'Else
            '            '    MsgBox("##ERROR INFO<DATA LOAD>##" & vbCrLf & _
            '            '                               "  ERROR DISCRIPTION" & vbTab & " : " & Err.Description & vbCrLf & _
            '            '                               "  ERROR NUMBER     " & vbTab & " : " & Err.Number & vbCrLf)
            '            'End If

            '        End If
            'End Select
            GoTo EndFile

        Catch ex As FileNotFoundException
            File_Read_Tky = 1
            ' "指定されたファイルは存在しません"
            Call Form1.System1.TrmMsgBox(gSysPrm, MSG_15, vbOKOnly, gAppName)

        Catch ex As Exception
            File_Read_Tky = 1
            MessageBox.Show("##" & File_001 & "<DATA LOAD>##" & vbCrLf & _
                            "  " & File_003 & vbTab & " : " & Err.Description & vbCrLf & _
                            "  " & File_004 & vbTab & " : " & Err.Number & vbCrLf)
        End Try

    End Function
#End Region

#Region "ファイルセーブ処理【TKY用】"
    '''=========================================================================
    '''<summary>ファイルセーブ処理【TKY用】</summary>
    '''<param name="pPath">(INP) ファイル名</param>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Public Function File_SaveTky(ByVal pPath As String) As Integer

        'Dim intFileNo As Integer                        ' ファイル番号
        Dim i As Integer
        Dim mDATA As String                             ' 編集用文字列
        Dim j As Integer
        Dim CutNum As Integer                           ' カット数

        File_SaveTky = 0

        Try
            ' ファイルバージョンセット
            If gSysPrm.stSPF.gblnUseProbePosChk = True Then
                gStrTkyFileVer = CONST_FILETYPE5
            Else
                gStrTkyFileVer = CONST_FILETYPE4            ' ファイルヴァージョン
            End If

            If (False = IO.File.Exists(pPath)) Then Throw New FileNotFoundException()
            ' テキストファイルをオープン
            Using sw As New StreamWriter(pPath, False, Encoding.UTF8)           'V4.4.0.0-1
                '[ファイルタイトル]-------------
                sw.WriteLine(CONST_VERSION)
                Select Case gStrTkyFileVer
                    Case CONST_FILETYPE4
                        sw.WriteLine(CONST_FILETYPE4)
                    Case CONST_FILETYPE4_SP1
                        sw.WriteLine(CONST_FILETYPE4_SP1)
                    Case CONST_FILETYPE5
                        sw.WriteLine(CONST_FILETYPE5)
                    Case CONST_FILETYPE6
                        sw.WriteLine(CONST_FILETYPE6)
                    Case Else
                        sw.WriteLine("NOT FILE VERSION")
                End Select

                '[プレートデータ]-------------
                sw.WriteLine(CONST_PLATE)
                sw.WriteLine(typPlateInfo.strDataName)  ' データ名
                '[プレート１]
                sw.WriteLine(CONST_PLATE1)
                sw.WriteLine(Get_PInfoArray1)           ' トリムモード～'ｱｼﾞｬｽﾄ位置ｵﾌｾｯﾄＹ
                '[プレート２]
                sw.WriteLine(CONST_PLATE2)
                sw.WriteLine(Get_PInfoArray2)           ' サーキット数～'アッテネータ(%)
                '[プレート３]
                sw.WriteLine(CONST_PLATE3)
                sw.WriteLine(Get_PInfoArray3)           ' 補正モード～''ｸﾞﾙｰﾌﾟＮｏ

                '[サーキット]-------------
                sw.WriteLine(CONST_CIRCUIT)
                mDATA = ""
                If typPlateInfo.intNGMark <> 0 Then

                    For i = 1 To typPlateInfo.intCurcuitCnt
                        With typCircuitInfoArray(i)
                            mDATA = Right(Space(3) & .intIP1, 3) & ","
                            mDATA = mDATA & Right(Space(8) & .dblIP2X.ToString("0.0000"), 8) & ","
                            mDATA = mDATA & Right(Space(8) & .dblIP2Y.ToString("0.0000"), 8)
                        End With
                        sw.WriteLine(mDATA)             ' IP番号,マーキングX,マーキングY
                    Next
                End If

                '[抵抗]--------------------
                sw.WriteLine(CONST_RESISTOR_DATA)
                '''' 2009/07/20 minato
                '''' TKYでは、0オリジンだが、CHIP系は1オリジンの為、1オリジンに合わせる。
                For i = 1 To gRegistorCnt
                    mDATA = Get_RInfoArray(i)
                    sw.WriteLine(mDATA)                 ' 抵抗データ1行
                Next

                '[カット]------------------
                sw.WriteLine(CONST_CUT_DATA)
                '''' 2009/07/20 minato
                '''' TKYでは、0オリジンだが、CHIP系は1オリジンの為、1オリジンに合わせる。
                For i = 1 To gRegistorCnt
                    CutNum = typResistorInfoArray(i).intCutCount
                    For j = 1 To CutNum
                        mDATA = Get_CInfoArray(j, i)
                        sw.WriteLine(mDATA)             ' カットデータ1行
                    Next
                Next

                '[CRC]------------------
                sw.WriteLine("C=")

            End Using

        Catch ex As Exception
            File_SaveTky = 1
            MessageBox.Show("##" & File_001 & "<DATA SAVE>##" & vbCrLf & _
                               "  " & File_003 & vbTab & " : " & Err.Description & vbCrLf & _
                               "  " & File_004 & vbTab & " : " & Err.Number & vbCrLf)
        End Try

#If False Then
        On Error GoTo ErrTrap

        File_SaveTky = 0

        ' ファイルバージョンセット
        If gSysPrm.stSPF.gblnUseProbePosChk = True Then
            gStrTkyFileVer = CONST_FILETYPE5
        Else
            gStrTkyFileVer = CONST_FILETYPE4            ' ファイルヴァージョン
        End If

        ' 使用可能なファイルナンバーを取得
        intFileNo = FreeFile()
        ' テキストファイルをオープン
        FileOpen(intFileNo, pPath, OpenMode.Output)

        '[ファイルタイトル]-------------
        PrintLine(intFileNo, CONST_VERSION)
        Select Case gStrTkyFileVer
            Case CONST_FILETYPE4
                PrintLine(intFileNo, CONST_FILETYPE4)
            Case CONST_FILETYPE4_SP1
                PrintLine(intFileNo, CONST_FILETYPE4_SP1)
            Case CONST_FILETYPE5
                PrintLine(intFileNo, CONST_FILETYPE5)
            Case CONST_FILETYPE6
                PrintLine(intFileNo, CONST_FILETYPE6)
            Case Else
                PrintLine(intFileNo, "NOT FILE VERSION")
        End Select

        '[プレートデータ]-------------
        PrintLine(intFileNo, CONST_PLATE)
        PrintLine(intFileNo, typPlateInfo.strDataName)  ' データ名
        '[プレート１]
        PrintLine(intFileNo, CONST_PLATE1)
        PrintLine(intFileNo, Get_PInfoArray1)           ' トリムモード～'ｱｼﾞｬｽﾄ位置ｵﾌｾｯﾄＹ
        '[プレート２]
        PrintLine(intFileNo, CONST_PLATE2)
        PrintLine(intFileNo, Get_PInfoArray2)           ' サーキット数～'アッテネータ(%)
        '[プレート３]
        PrintLine(intFileNo, CONST_PLATE3)
        PrintLine(intFileNo, Get_PInfoArray3)           ' 補正モード～''ｸﾞﾙｰﾌﾟＮｏ

        '[サーキット]-------------
        PrintLine(intFileNo, CONST_CIRCUIT)
        mDATA = ""
        If typPlateInfo.intNGMark <> 0 Then

            For i = 1 To typPlateInfo.intCurcuitCnt
                With typCircuitInfoArray(i)
                    mDATA = Right(Space(3) & .intIP1, 3) & ","
                    mDATA = mDATA & Right(Space(8) & .dblIP2X.ToString("0.0000"), 8) & ","
                    mDATA = mDATA & Right(Space(8) & .dblIP2Y.ToString("0.0000"), 8)
                End With
                PrintLine(intFileNo, mDATA)             ' IP番号,マーキングX,マーキングY
            Next
        End If

        '[抵抗]--------------------
        PrintLine(intFileNo, CONST_RESISTOR_DATA)
        '''' 2009/07/20 minato
        '''' TKYでは、0オリジンだが、CHIP系は1オリジンの為、1オリジンに合わせる。
        For i = 1 To gRegistorCnt
            mDATA = Get_RInfoArray(i)
            PrintLine(intFileNo, mDATA)                 ' 抵抗データ1行
        Next

        '[カット]------------------
        PrintLine(intFileNo, CONST_CUT_DATA)
        '''' 2009/07/20 minato
        '''' TKYでは、0オリジンだが、CHIP系は1オリジンの為、1オリジンに合わせる。
        For i = 1 To gRegistorCnt
            CutNum = typResistorInfoArray(i).intCutCount
            For j = 1 To CutNum
                mDATA = Get_CInfoArray(j, i)
                PrintLine(intFileNo, mDATA)             ' カットデータ1行
            Next
        Next

        '[CRC]------------------
        PrintLine(intFileNo, "C=")

        FileClose(intFileNo)
        Exit Function

ErrTrap:
        File_SaveTky = 1
        If Err.Number <> 0 Then
            ' メッセージ設定
            'If (gSysPrm.stTMN.giMsgTyp = 0) Then
            '    MsgBox("##ｴﾗｰ情報<DATA SAVE>##" & vbCrLf & _
            '                       "  ｴﾗｰ詳細" & vbTab & " : " & Err.Description & vbCrLf & _
            '                       "  ｴﾗｰ番号" & vbTab & " : " & Err.Number & vbCrLf)
            'Else
            '    MsgBox("##ERROR INFO<DATA SAVE>##" & vbCrLf & _
            '                       "  ERROR DISCRIPTION" & vbTab & " : " & Err.Description & vbCrLf & _
            '                       "  ERROR NUMBER     " & vbTab & " : " & Err.Number & vbCrLf)
            'End If
            MsgBox("##" & File_001 & "<DATA SAVE>##" & vbCrLf & _
                               "  " & File_003 & vbTab & " : " & Err.Description & vbCrLf & _
                               "  " & File_004 & vbTab & " : " & Err.Number & vbCrLf)
        End If
#End If
    End Function

#End Region

#Region "ロードしたプレートデータをグローバル変数へ格納する【TKY用】"
    '''=========================================================================
    '''<summary>ロードしたプレートデータをグローバル変数へ格納する【TKY用】</summary>
    '''<param name="pBuff">(INP) ロードデータ</param>
    '''<param name="pType">(INP) データ種別</param>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function Set_typPlateInfoTky(ByVal pBuff As String, ByVal pType As Integer) As Integer

        Dim mDATA() As String
        Dim i As Integer
        Dim flg As Integer

        On Error GoTo ErrTrap
        Set_typPlateInfoTky = 0
        flg = 0

        With typPlateInfo
            Select Case pType
                Case 1  ' データ名
                    .strDataName = pBuff

                Case 2  ' プレートデータ１
                    i = 0
                    ' ロードデータを配列セット
                    mDATA = pBuff.Split(",")

                    .intMeasType = CInt(mDATA(i)) : i = i + 1                        'トリムモード
                    .intDirStepRepeat = CInt(mDATA(i)) : i = i + 1                   'ｽﾃｯﾌﾟ＆ﾘﾋﾟｰﾄ
                    .intPlateCntXDir = CInt(mDATA(i)) : i = i + 1                    'プレート数X
                    .intPlateCntYDir = CInt(mDATA(i)) : i = i + 1                    'プレート数Y
                    .intBlockCntXDir = CInt(mDATA(i)) : i = i + 1                    'ﾌﾞﾛｯｸ数X
                    .intBlockCntYDir = CInt(mDATA(i)) : i = i + 1                    'ﾌﾞﾛｯｸ数Y
                    .dblPlateItvXDir = CDbl(mDATA(i)) : i = i + 1                    'ﾌﾟﾚｰﾄ間隔Ｘ
                    .dblPlateItvYDir = CDbl(mDATA(i)) : i = i + 1                    'ﾌﾟﾚｰﾄ間隔Ｙ
                    .dblBlockSizeXDir = CDbl(mDATA(i)) : i = i + 1                   'ﾌﾞﾛｯｸｻｲｽﾞＸ
                    .dblBlockSizeYDir = CDbl(mDATA(i)) : i = i + 1                   'ﾌﾞﾛｯｸｻｲｽﾞＹ
                    .dblTableOffsetXDir = CDbl(mDATA(i)) : i = i + 1                 'ﾃｰﾌﾞﾙ位置ｵﾌｾｯﾄX
                    .dblTableOffsetYDir = CDbl(mDATA(i)) : i = i + 1                 'ﾃｰﾌﾞﾙ位置ｵﾌｾｯﾄY
                    .dblBpOffSetXDir = CDbl(mDATA(i)) : i = i + 1                    'ﾋﾞｰﾑ位置ｵﾌｾｯﾄX
                    .dblBpOffSetYDir = CDbl(mDATA(i)) : i = i + 1                    'ﾋﾞｰﾑ位置ｵﾌｾｯﾄY
                    .dblAdjOffSetXDir = CDbl(mDATA(i)) : i = i + 1                   'ｱｼﾞｬｽﾄ位置ｵﾌｾｯﾄX
                    .dblAdjOffSetYDir = CDbl(mDATA(i)) : i = i + 1                   'ｱｼﾞｬｽﾄ位置ｵﾌｾｯﾄY

                Case 3  'プレートデータ２
                    i = 0
                    mDATA = pBuff.Split(",")
                    .intCurcuitCnt = CInt(mDATA(i)) : i = i + 1                      'サーキット数
                    .intNGMark = CInt(mDATA(i)) : i = i + 1                          'マーキング
                    .intDelayTrim = CInt(mDATA(i)) : i = i + 1                       'ﾃﾞｨﾚｲﾄﾘﾑ
                    .intNgJudgeUnit = CInt(mDATA(i)) : i = i + 1                     'ＮＧ判定単位
                    .intNgJudgeLevel = CInt(mDATA(i)) : i = i + 1                    'ＮＧ判定基準
                    .dblZOffSet = CDbl(mDATA(i)) : i = i + 1                         'ﾌﾟﾛｰﾌﾞＺｵﾌｾｯﾄ
                    .dblZStepUpDist = CDbl(mDATA(i)) : i = i + 1                     'ﾌﾟﾛｰﾌﾞｽﾃｯﾌﾟ上昇距離
                    .dblZWaitOffset = CDbl(mDATA(i)) : i = i + 1                     'ﾌﾟﾛｰﾌﾞ待機Ｚｵﾌｾｯﾄ
                    .intFinalJudge = CInt(0) : i = i + 1                             'ファイナル判定
                    .intAttLevel = CInt(0) : i = i + 1                               'アッテネータ(%)
                    '----- V1.22.0.0②↓ -----
                    ' Z ON/OFF位置を再設定する
                    .dblZOffSet = 5.0                                               'ZON位置
                    .dblZStepUpDist = 3.0                                           'ﾌﾟﾛｰﾌﾞｽﾃｯﾌﾟ上昇距離
                    .dblZWaitOffset = 1.0                                           'ZOFF位置
                    '----- V1.22.0.0②↑ -----

                Case 4  'プレートデータ３
                    i = 0
                    mDATA = pBuff.Split(",")
                    .intReviseMode = CInt(mDATA(i)) : i = i + 1                      '補正モード
                    .intManualReviseType = CInt(mDATA(i)) : i = i + 1                '補正方法
                    .dblReviseCordnt1XDir = CDbl(mDATA(i)) : i = i + 1               '補正位置座標1X
                    .dblReviseCordnt1YDir = CDbl(mDATA(i)) : i = i + 1               '補正位置座標1Y
                    .dblReviseCordnt2XDir = CDbl(mDATA(i)) : i = i + 1               '補正位置座標2X
                    .dblReviseCordnt2YDir = CDbl(mDATA(i)) : i = i + 1               '補正位置座標2Y
                    .dblReviseOffsetXDir = CDbl(mDATA(i)) : i = i + 1                '補正ﾎﾟｼﾞｼｮﾝｵﾌｾｯﾄX
                    .dblReviseOffsetYDir = CDbl(mDATA(i)) : i = i + 1                '補正ﾎﾟｼﾞｼｮﾝｵﾌｾｯﾄY
                    .intRecogDispMode = CInt(mDATA(i)) : i = i + 1                   '認識データ表示モード
                    If gSysPrm.stDEV.giEXCAM = 1 Then
                        .dblPixelValXDir = gSysPrm.stGRV.gfEXCAM_PixelX : i = i + 1  'ﾋﾟｸｾﾙ値X
                        .dblPixelValYDir = gSysPrm.stGRV.gfEXCAM_PixelY : i = i + 1  'ﾋﾟｸｾﾙ値Y
                    Else
                        .dblPixelValXDir = gSysPrm.stGRV.gfPixelX : i = i + 1        'ﾋﾟｸｾﾙ値X
                        .dblPixelValYDir = gSysPrm.stGRV.gfPixelY : i = i + 1        'ﾋﾟｸｾﾙ値Y
                    End If
                    .intRevisePtnNo1 = CInt(mDATA(i)) : i = i + 1                    '補正位置ﾊﾟﾀｰﾝＮＯ1
                    .intRevisePtnNo2 = CInt(mDATA(i)) : i = i + 1                    '補正位置ﾊﾟﾀｰﾝＮＯ2
                    ' ファイルバージョンによりデータの設定処理を変更
                    ' Ver5以前のファイルの場合
                    If gStrTkyFileVer <= CONST_FILETYPE5 Then
                        .intRevisePtnNo1GroupNo = CInt(mDATA(i))                    '補正位置ﾊﾟﾀｰﾝＮＯ1ｸﾞﾙｰﾌﾟＮｏ
                        .intRevisePtnNo2GroupNo = CInt(mDATA(i))                    '補正位置ﾊﾟﾀｰﾝＮＯ2ｸﾞﾙｰﾌﾟＮｏ
                        .intCutPosiReviseGroupNo = CInt(mDATA(i)) : i = i + 1        'ｸﾞﾙｰﾌﾟＮｏ
                    Else
                        .intRevisePtnNo1GroupNo = CInt(mDATA(i)) : i = i + 1         '補正位置ﾊﾟﾀｰﾝＮＯ1ｸﾞﾙｰﾌﾟＮｏ
                        .intRevisePtnNo2GroupNo = CInt(mDATA(i)) : i = i + 1         '補正位置ﾊﾟﾀｰﾝＮＯ2ｸﾞﾙｰﾌﾟＮｏ
                        .intCutPosiReviseGroupNo = CInt(mDATA(i)) : i = i + 1        'ｸﾞﾙｰﾌﾟＮｏ
                    End If
                    flg = 1
                    .dblRotateTheta = CDbl(mDATA(i)) : i = i + 1                    'θ回転角度

            End Select
        End With
        Exit Function

ErrTrap:
        If flg = 1 Then Exit Function ' θ回転の前まで設定なら正常ﾘﾀｰﾝ
        Set_typPlateInfoTky = -1

    End Function
#End Region

#Region "ロードした抵抗データをグローバル変数へ格納する【TKY用】"
    '''=========================================================================
    '''<summary>ロードした抵抗データをグローバル変数へ格納する【TKY用】</summary>
    '''<param name="pBuff">(INP) ロードデータ</param>
    '''<param name="pCnt"> (INP) 配列数</param>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function Set_typResistorInfoArray(ByVal pBuff As String, ByVal pCnt As Integer) As Integer

        Dim mDATA As Object
        Dim i As Integer

        On Error GoTo ErrTrap
        Set_typResistorInfoArray = 0

        '''' 2009/07/20 minato
        '''' TKYでは、0オリジンだが、CHIP系は1オリジンの為、1オリジンに合わせる。
        '    With typResistorInfoArray(pCnt - 1)
        With typResistorInfoArray(pCnt)
            i = 0
            ' ロードデータを配列セット
            mDATA = pBuff.Split(",")

            .intResNo = CInt(mDATA(i)) : i = i + 1          ' 抵抗番号
            .intResMeasMode = typPlateInfo.intMeasType      ' トリムモード(0:抵抗 ,1:電圧) ※変更プレートデータより設定
            .intResMeasType = CInt(mDATA(i)) : i = i + 1    ' 測定タイプ(0:高速 ,1:高精度)※追加
            .intCircuitGrp = CInt(mDATA(i)) : i = i + 1     ' 所属サーキット

            If (.intResNo >= 1000 And .intCircuitGrp = 0) Then
                .intCircuitGrp = 1
            End If

            '----- V6.1.4.0_23↓ -----
            ' 抵抗番号が1000以降の場合には、目標値はデフォルトの1を設定する
            '.intProbHiNo = CInt(mDATA(i)) : i = i + 1       ' プローブ番号（ハイ側）
            '.intProbLoNo = CInt(mDATA(i)) : i = i + 1       ' プローブ番号（ロー側）
            If .intResNo < 1000 Then
                .intProbHiNo = CInt(mDATA(i)) : i = i + 1   ' プローブ番号（ハイ側）
                .intProbLoNo = CInt(mDATA(i)) : i = i + 1   ' プローブ番号（ロー側）
            Else
                .intProbHiNo = 1 : i = i + 1                ' プローブ番号（ハイ側）
                .intProbLoNo = 2 : i = i + 1                ' プローブ番号（ロー側）
            End If
            '----- V6.1.4.0_23↑ -----
            .intProbAGNo1 = CInt(mDATA(i)) : i = i + 1      ' プローブ番号（１）
            .intProbAGNo2 = CInt(mDATA(i)) : i = i + 1      ' プローブ番号（２）
            .intProbAGNo3 = CInt(mDATA(i)) : i = i + 1      ' プローブ番号（３）
            .intProbAGNo4 = CInt(mDATA(i)) : i = i + 1      ' プローブ番号（４）
            .intProbAGNo5 = CInt(mDATA(i)) : i = i + 1      ' プローブ番号（５）
            .strExternalBits = CLng(mDATA(i)) : i = i + 1   ' EXTERNAL BITS
            .intPauseTime = CInt(mDATA(i)) : i = i + 1      ' ポーズタイム
            .intTargetValType = CInt(mDATA(i)) : i = i + 1  ' トリムモード
            .intBaseResNo = CInt(mDATA(i)) : i = i + 1      ' ベ－ス抵抗番号
            '----- V6.1.4.0_23↓ -----
            ' 抵抗番号が1000以降の場合には、目標値はデフォルトの1を設定する
            '.dblTrimTargetVal = CDbl(mDATA(i)) : i = i + 1  ' トリミング目標値
            If .intResNo < 1000 Then
                .dblTrimTargetVal = CDbl(mDATA(i)) : i = i + 1 ' トリミング目標値
            Else
                .dblTrimTargetVal = 1.0# : i = i + 1        ' トリミング目標値
            End If
            '----- V6.1.4.0_23↑ -----

            ' ファイルバージョンによって処理わけ
            Select Case gStrTkyFileVer
                Case CONST_FILETYPE5
                    .dblProbCfmPoint_Hi_X = CDbl(mDATA(i)) : i = i + 1  ' プローブ確認位置 HI X座標
                    .dblProbCfmPoint_Hi_Y = CDbl(mDATA(i)) : i = i + 1  ' プローブ確認位置 HI Y座標
                    .dblProbCfmPoint_Lo_X = CDbl(mDATA(i)) : i = i + 1  ' プローブ確認位置 LO X座標
                    .dblProbCfmPoint_Lo_Y = CDbl(mDATA(i)) : i = i + 1  ' プローブ確認位置 LO Y座標
                Case Else
                    ' デフォルト値代入
                    .dblProbCfmPoint_Hi_X = 0                           ' プローブ確認位置 HI X座標
                    .dblProbCfmPoint_Hi_Y = 0                           ' プローブ確認位置 HI Y座標
                    .dblProbCfmPoint_Lo_X = 0                           ' プローブ確認位置 LO X座標
                    .dblProbCfmPoint_Lo_Y = 0                           ' プローブ確認位置 LO Y座標
            End Select

            .intSlope = CInt(mDATA(i)) : i = i + 1                  ' 電圧変化 ｽﾛｰﾌﾟ
            .dblInitTest_HighLimit = CDbl(mDATA(i)) : i = i + 1     ' イニシャルテストHIGHリミット
            .dblInitTest_LowLimit = CDbl(mDATA(i)) : i = i + 1      ' イニシャルテストLOWリミット
            .dblFinalTest_HighLimit = CDbl(mDATA(i)) : i = i + 1    ' ファイナルテストHIGHリミット
            .dblFinalTest_LowLimit = CDbl(mDATA(i)) : i = i + 1     ' ファイナルテストLOWリミット
            .intCutReviseMode = CInt(mDATA(i)) : i = i + 1          ' ｶｯﾄ 補正
            .intCutReviseDispMode = CInt(mDATA(i)) : i = i + 1      ' 表示ﾓｰﾄﾞ
            .intCutRevisePtnNo = CInt(mDATA(i)) : i = i + 1         ' ﾊﾟﾀｰﾝ No.
            .dblCutRevisePosX = CDbl(mDATA(i)) : i = i + 1          ' ｶｯﾄ補正位置X
            .dblCutRevisePosY = CDbl(mDATA(i)) : i = i + 1          ' ｶｯﾄ補正位置Y
            .intIsNG = CInt(mDATA(i)) : i = i + 1                   ' NG有無
            .intCutCount = CInt(mDATA(i)) : i = i + 1               ' カット数
            .strRatioTrimTargetVal = CStr(mDATA(i)) : i = i + 1     ' トリミング目標値計算値

        End With

        Exit Function

ErrTrap:
        Set_typResistorInfoArray = -1
    End Function
#End Region

#Region "ロードしたカットデータをグローバル変数へ格納する【TKY用】"
    '''=========================================================================
    '''<summary>ロードしたカットデータをグローバル変数へ格納する【TKY用】</summary>
    '''<param name="pBuff">(INP) ロードデータ</param>
    '''<param name="pCnt"> (I/O) 抵抗データ配列数</param>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function Set_typCutInfoArray(ByVal pBuff As String, ByRef pCnt As Integer) As Integer

        Dim mDATA() As String
        Dim i As Integer
        Dim j As Integer
        Dim mCTNum As Integer                           ' カット数
        On Error GoTo ErrTrap
        Set_typCutInfoArray = 0

        i = 0
        ' ロードデータを配列セット
        mDATA = pBuff.Split(",")
        mCTNum = CInt(mDATA(1))                         ' カット番号取得
        If mCTNum = 1 Then                              ' カット番号＝１の場合は抵抗データ配列番号をカウントアップ
            pCnt = pCnt + 1
        End If

        ' TKYでは、0オリジンだが、CHIP系は1オリジンの為1オリジンに合わせる。
        'With typCutInfoArray(pCnt)
        With typResistorInfoArray(pCnt)
            '.intCRNO = CInt(mDATA(i)) : i = i + 1                       ' 抵抗番号
            i = i + 1                                                   ' 抵抗番号

            .ArrCut(mCTNum).intCutNo = CInt(mDATA(i)) : i = i + 1       ' カット番号
            .ArrCut(mCTNum).intDelayTime = CInt(mDATA(i)) : i = i + 1   ' ディレイタイム
            .ArrCut(mCTNum).dblStartPointX = CDbl(mDATA(i)) : i = i + 1 ' スタートポイントX
            .ArrCut(mCTNum).dblStartPointY = CDbl(mDATA(i)) : i = i + 1 ' スタートポイントY
            .ArrCut(mCTNum).dblCutSpeed = CDbl(mDATA(i)) : i = i + 1    ' カットスピード
            .ArrCut(mCTNum).dblQRate = CDbl(mDATA(i)) : i = i + 1       ' Ｑスイッチレート
            .ArrCut(mCTNum).dblCutOff = CDbl(mDATA(i)) : i = i + 1      ' カットオフ値
            .ArrCut(mCTNum).dblJudgeLevel = CDbl(mDATA(i)) : i = i + 1  ' 切替ポイント (旧ﾃﾞｰﾀ判定(平均化率))
            .ArrCut(mCTNum).dblJudgeLevel = 0.0                             ' 切替ポイントは初期化する 'V1.22.0.0②
            .ArrCut(mCTNum).strCutType = Trim(CStr(mDATA(i))) : i = i + 1   ' カット形状 9
            .ArrCut(mCTNum).intCutDir = CInt(mDATA(i)) : i = i + 1          ' カット方向 10
            .ArrCut(mCTNum).dblMaxCutLength = CDbl(mDATA(i)) : i = i + 1    ' 最大カッティング長 11
            .ArrCut(mCTNum).dblR1 = CDbl(mDATA(i)) : i = i + 1              ' Ｒ１ 12
            .ArrCut(mCTNum).dblLTurnPoint = CDbl(mDATA(i)) : i = i + 1      ' Ｌターンポイント 13
            .ArrCut(mCTNum).dblMaxCutLengthL = CDbl(mDATA(i)) : i = i + 1   ' Ｌターン後の最大カッティング長 14
            .ArrCut(mCTNum).dblR2 = CDbl(mDATA(i)) : i = i + 1              ' Ｒ２ 15
            .ArrCut(mCTNum).dblMaxCutLengthHook = CDbl(mDATA(i)) : i = i + 1 ' フックターン後のカッティング長 16
            .ArrCut(mCTNum).intIndexCnt = CInt(mDATA(i)) : i = i + 1        ' インデックス数 17
            .ArrCut(mCTNum).intMeasMode = CInt(mDATA(i)) : i = i + 1        ' 測定モード 18
            .ArrCut(mCTNum).dblCutSpeed2 = CDbl(mDATA(i)) : i = i + 1       ' カットスピード２ 19
            '----- V1.13.0.0②↓ -----
            If (.ArrCut(mCTNum).dblCutSpeed2 = 0) Then                      ' カットスピード2にカットスピードを設定する
                .ArrCut(mCTNum).dblCutSpeed2 = .ArrCut(mCTNum).dblCutSpeed
            End If
            '----- V1.13.0.0②↑ -----
            .ArrCut(mCTNum).dblQRate2 = CDbl(mDATA(i)) : i = i + 1          ' Ｑスイッチレート２ 20
            '----- V1.13.0.0②↓ -----
            If (.ArrCut(mCTNum).dblQRate2 = 0) Then                         ' Ｑスイッチレート２にＱスイッチレートを設定する
                .ArrCut(mCTNum).dblQRate2 = .ArrCut(mCTNum).dblQRate
            End If
            '----- V1.13.0.0②↑ -----
            .ArrCut(mCTNum).intCutAngle = CInt(mDATA(i)) : i = i + 1        ' 斜めカットの切り出し角度 21
            .ArrCut(mCTNum).dblPitch = CDbl(mDATA(i)) : i = i + 1           ' ピッチ 22
            .ArrCut(mCTNum).intStepDir = CInt(mDATA(i)) : i = i + 1         ' ステップ方向 23
            .ArrCut(mCTNum).intCutCnt = CInt(mDATA(i)) : i = i + 1          ' 本数 24
            .ArrCut(mCTNum).dblZoom = CDbl(mDATA(i)) : i = i + 1            ' 倍率 25
            .ArrCut(mCTNum).strChar = CStr(mDATA(i)) : i = i + 1            ' 文字列 26
            .ArrCut(mCTNum).strChar = HexAsc2Str(.ArrCut(mCTNum).strChar)

            '----- V1.13.0.0②↓ -----
            If (.ArrCut(mCTNum).strCutType = CNS_CUTP_NOP) Then             ' カット形状 = Z(NOP)なら 
                .ArrCut(mCTNum).strCutType = CNS_CUTP_NST                   ' 斜めSTカットに変更
                .ArrCut(mCTNum).dblMaxCutLength = 0.0                       ' カット長  = 0 
            End If
            '----- V1.13.0.0②↑ -----

            ' カット方向から斜めカットの切り出し角度を設定する
            Call GetCutAngle(.ArrCut(mCTNum).strCutType, .ArrCut(mCTNum).intCutDir, .ArrCut(mCTNum).intCutAngle)
            ' カット方向から斜めカットの切り出し角度とLターン方向を設定する(Lカット/HOOKカット用)
            Call GetCutLTurnDir(.ArrCut(mCTNum).strCutType, .ArrCut(mCTNum).intCutDir, .ArrCut(mCTNum).intCutAngle, .ArrCut(mCTNum).intLTurnDir)
            ' ステップ方向を変換する(スキャンカット用)
            Call GetStepDir(.ArrCut(mCTNum).strCutType, .ArrCut(mCTNum).intStepDir, .ArrCut(mCTNum).intStepDir)
            '----- V1.13.0.0②↓ -----
            ' STカットとLカットは斜めSTカット, 斜めLカットに変換する
            Call CnvCutType(.ArrCut(mCTNum).strCutType, .ArrCut(mCTNum).strCutType)
            '----- V1.13.0.0②↑ -----

            ' 目標パワーと許容範囲(デフォルト値を設定する) ###066
            'For j = 0 To (cCNDNUM - 1)                                          ' 加工条件番号1～n(0ｵﾘｼﾞﾝ)
            For j = 0 To (MaxCndNum - 1)                                        ' 加工条件番号1～n(0ｵﾘｼﾞﾝ) 'V5.0.0.8①
                'V6.0.0.1⑥                .ArrCut(mCTNum).dblPowerAdjustTarget(j) = POWERADJUST_TARGET   ' 目標パワー(W)
                'V6.0.0.1⑥                .ArrCut(mCTNum).dblPowerAdjustToleLevel(j) = POWERADJUST_LEVEL  ' 許容範囲(±W)
                .ArrCut(mCTNum).dblPowerAdjustTarget(j) = DEFAULT_ADJUST_TAERGET   ' 目標パワー(W)   'V6.0.0.1⑥
                .ArrCut(mCTNum).dblPowerAdjustToleLevel(j) = DEFAULT_ADJUST_LEVEL  ' 許容範囲(±W)    'V6.0.0.1⑥
            Next j

        End With

        Exit Function

ErrTrap:
        Set_typCutInfoArray = -1

    End Function

#End Region

#Region "ロードしたサーキットデータをグローバル変数へ格納する【TKY用】"
    '''=========================================================================
    '''<summary>ロードしたサーキットデータをグローバル変数へ格納する【TKY用】</summary>
    '''<param name="pBuff">(INP) ロードデータ</param>
    '''<param name="pCnt"> (INP) 配列数</param>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function Set_typCircuitInfoArray(ByVal pBuff As String, ByVal pCnt As Integer) As Integer

        Dim mDATA() As String
        Dim i As Integer

        On Error GoTo ErrTrap
        Set_typCircuitInfoArray = 0

        '''''2009/07/20 minato
        '''' TKYでは、0オリジンだが、CHIP系は1オリジンの為、1オリジンに合わせる。
        ''''    (※）サーキットだけなぜか1オリジン
        With typCircuitInfoArray(pCnt)
            i = 0
            ' ロードデータを配列セット
            mDATA = pBuff.Split(",")

            .intIP1 = CInt(mDATA(i)) : i = i + 1        ' IP番号
            .dblIP2X = CDbl(mDATA(i)) : i = i + 1       ' マーキングX
            .dblIP2Y = CDbl(mDATA(i)) : i = i + 1       ' マーキングY
        End With

        Exit Function

ErrTrap:
        Set_typCircuitInfoArray = -1
    End Function
#End Region

#Region "ロードした異形面付けデータをグローバル変数へ格納する【TKY用】"
    '''=========================================================================
    '''<summary>ロードした異形面付けデータをグローバル変数へ格納する【TKY用】</summary>
    '''<param name="pBuff">(INP) ロードデータ</param>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function Set_typIKEIInfo(ByVal pBuff As String) As Integer

        Dim mDATA() As String
        Dim i As Integer
        Dim j As Integer
        Dim n As Integer

        Set_typIKEIInfo = 0

        i = 0
        ' ロードデータを配列セット
        mDATA = pBuff.Split(",")

        With typIKEIInfo
            .intI1 = CInt(mDATA(i)) : i = i + 1             ' 異形面付けの有無
            n = CInt(mDATA(i)) : i = i + 1                  ' サーキット数
            If n > MaxCntCircuit Then n = MaxCntCircuit
            For j = 0 To n - 1
                .intI2(j) = CInt(mDATA(i)) : i = i + 1
            Next j
        End With

        Exit Function

ErrTrap:
        Set_typIKEIInfo = -1

    End Function

#End Region

#Region "グローバルデータからプレートデータ1を取得する【TKY用】"
    '''=========================================================================
    '''<summary>グローバルデータからプレートデータ1を取得する【TKY用】</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Function Get_PInfoArray1() As String

        Get_PInfoArray1 = ""

        With typPlateInfo
            '[プレート１]
            Get_PInfoArray1 = CStr(.intMeasType) & ","
            Get_PInfoArray1 = Get_PInfoArray1 & CStr(.intDirStepRepeat) & ","
            Get_PInfoArray1 = Get_PInfoArray1 & CStr(.intPlateCntXDir) & ","
            Get_PInfoArray1 = Get_PInfoArray1 & CStr(.intPlateCntYDir) & ","
            Get_PInfoArray1 = Get_PInfoArray1 & CStr(.intBlockCntXDir) & ","
            Get_PInfoArray1 = Get_PInfoArray1 & CStr(.intBlockCntYDir) & ","
            Get_PInfoArray1 = Get_PInfoArray1 & .dblPlateItvXDir.ToString("0.0000") & ","
            Get_PInfoArray1 = Get_PInfoArray1 & .dblPlateItvYDir.ToString("0.0000") & ","
            Get_PInfoArray1 = Get_PInfoArray1 & .dblBlockSizeXDir.ToString("0.0000") & ","
            Get_PInfoArray1 = Get_PInfoArray1 & .dblBlockSizeYDir.ToString("0.0000") & ","
            Get_PInfoArray1 = Get_PInfoArray1 & .dblTableOffsetXDir.ToString("0.0000") & ","
            Get_PInfoArray1 = Get_PInfoArray1 & .dblTableOffsetYDir.ToString("0.0000") & ","
            Get_PInfoArray1 = Get_PInfoArray1 & .dblBpOffSetXDir.ToString("0.0000") & ","
            Get_PInfoArray1 = Get_PInfoArray1 & .dblBpOffSetYDir.ToString("0.0000") & ","
            Get_PInfoArray1 = Get_PInfoArray1 & .dblAdjOffSetXDir.ToString("0.0000") & ","
            Get_PInfoArray1 = Get_PInfoArray1 & .dblAdjOffSetYDir.ToString("0.0000")
        End With


    End Function
#End Region

#Region "グローバルデータからプレートデータ2を取得する【TKY用】"
    '''=========================================================================
    '''<summary>グローバルデータからプレートデータ2を取得する【TKY用】</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Function Get_PInfoArray2() As String

        Get_PInfoArray2 = ""

        With typPlateInfo
            '[プレート１]
            Get_PInfoArray2 = CStr(.intCurcuitCnt) & ","
            Get_PInfoArray2 = Get_PInfoArray2 & CStr(.intNGMark) & ","
            Get_PInfoArray2 = Get_PInfoArray2 & CStr(.intDelayTrim) & ","
            Get_PInfoArray2 = Get_PInfoArray2 & CStr(.intNgJudgeUnit) & ","
            Get_PInfoArray2 = Get_PInfoArray2 & CStr(.intNgJudgeLevel) & ","
            Get_PInfoArray2 = Get_PInfoArray2 & .dblZOffSet.ToString("0.0000") & ","
            Get_PInfoArray2 = Get_PInfoArray2 & .dblZStepUpDist.ToString("0.0000") & ","
            Get_PInfoArray2 = Get_PInfoArray2 & .dblZWaitOffset.ToString("0.0000") & ","
            Get_PInfoArray2 = Get_PInfoArray2 & CStr(0) & ","
            Get_PInfoArray2 = Get_PInfoArray2 & CStr(0)
        End With


    End Function
#End Region

#Region "グローバルデータからプレートデータ3を取得する【TKY用】"
    '''=========================================================================
    '''<summary>グローバルデータからプレートデータ3を取得する【TKY用】</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Function Get_PInfoArray3() As String

        Get_PInfoArray3 = ""

        With typPlateInfo
            '[プレート１]
            Get_PInfoArray3 = CStr(.intReviseMode) & ","
            Get_PInfoArray3 = Get_PInfoArray3 & CStr(.intManualReviseType) & ","
            Get_PInfoArray3 = Get_PInfoArray3 & .dblReviseCordnt1XDir.ToString("0.0000") & ","
            Get_PInfoArray3 = Get_PInfoArray3 & .dblReviseCordnt1YDir.ToString("0.0000") & ","
            Get_PInfoArray3 = Get_PInfoArray3 & .dblReviseCordnt2XDir.ToString("0.0000") & ","
            Get_PInfoArray3 = Get_PInfoArray3 & .dblReviseCordnt2YDir.ToString("0.0000") & ","
            Get_PInfoArray3 = Get_PInfoArray3 & .dblReviseOffsetXDir.ToString("0.0000") & ","
            Get_PInfoArray3 = Get_PInfoArray3 & .dblReviseOffsetYDir.ToString("0.0000") & ","
            Get_PInfoArray3 = Get_PInfoArray3 & CStr(.intRecogDispMode) & ","
            Get_PInfoArray3 = Get_PInfoArray3 & .dblPixelValXDir.ToString("0.00") & ","
            Get_PInfoArray3 = Get_PInfoArray3 & .dblPixelValYDir.ToString("0.00") & ","
            Get_PInfoArray3 = Get_PInfoArray3 & CStr(.intRevisePtnNo1) & ","
            Get_PInfoArray3 = Get_PInfoArray3 & CStr(.intRevisePtnNo2) & ","
            'ファイルタイプにより
            If gStrTkyFileVer <= CONST_FILETYPE5 Then
                Get_PInfoArray3 = Get_PInfoArray3 & CStr(.intCutPosiReviseGroupNo) & ","
            Else
                Get_PInfoArray3 = Get_PInfoArray3 & CStr(.intRevisePtnNo1GroupNo) & ","
                Get_PInfoArray3 = Get_PInfoArray3 & CStr(.intRevisePtnNo2GroupNo) & ","
                Get_PInfoArray3 = Get_PInfoArray3 & CStr(.intCutPosiReviseGroupNo) & ","
            End If
            Get_PInfoArray3 = Get_PInfoArray3 & .dblRotateTheta.ToString("0.00000") 'θ軸角度

        End With

    End Function
#End Region

#Region "グローバルデータから抵抗データを取得する【TKY用】"
    '''=========================================================================
    '''<summary>グローバルデータから抵抗データを取得する【TKY用】</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Function Get_RInfoArray(ByVal pCnt As Integer) As String

        Get_RInfoArray = ""

        With typResistorInfoArray(pCnt)

            Get_RInfoArray = Right(Space(4) & .intResNo, 4) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(2) & .intResMeasMode, 2) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(3) & .intCircuitGrp, 3) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(2) & .intProbHiNo, 2) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(2) & .intProbLoNo, 2) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(2) & .intProbAGNo1, 2) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(2) & .intProbAGNo2, 2) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(2) & .intProbAGNo3, 2) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(2) & .intProbAGNo4, 2) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(2) & .intProbAGNo5, 2) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(17) & .strExternalBits, 17) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(6) & .intPauseTime, 6) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(2) & .intTargetValType, 2) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(4) & .intBaseResNo, 4) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(18) & .dblTrimTargetVal.ToString("0.000000"), 18) & ","
            ' ファイルバージョンによって出力
            If gStrTkyFileVer = CONST_FILETYPE5 Then
                Get_RInfoArray = Get_RInfoArray & Right(Space(9) & .dblProbCfmPoint_Hi_X.ToString("0.0000"), 9) & ","
                Get_RInfoArray = Get_RInfoArray & Right(Space(9) & .dblProbCfmPoint_Hi_Y.ToString("0.0000"), 9) & ","
                Get_RInfoArray = Get_RInfoArray & Right(Space(9) & .dblProbCfmPoint_Lo_X.ToString("0.0000"), 9) & ","
                Get_RInfoArray = Get_RInfoArray & Right(Space(9) & .dblProbCfmPoint_Lo_Y.ToString("0.0000"), 9) & ","
            End If

            Get_RInfoArray = Get_RInfoArray & Right(Space(2) & .intSlope, 2) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(7) & .dblInitTest_HighLimit.ToString("0.00"), 7) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(7) & .dblInitTest_LowLimit.ToString("0.00"), 7) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(7) & .dblFinalTest_HighLimit.ToString("0.00"), 7) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(7) & .dblFinalTest_LowLimit.ToString("0.00"), 7) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(2) & .intCutReviseMode, 2) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(2) & .intCutReviseDispMode, 2) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(3) & .intCutRevisePtnNo, 3) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(9) & .dblCutRevisePosX.ToString("0.00"), 9) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(9) & .dblCutRevisePosY.ToString("0.00"), 9) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(2) & .intIsNG, 2) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(2) & .intCutCount, 2) & ","
            Get_RInfoArray = Get_RInfoArray & .strRatioTrimTargetVal

        End With

    End Function
#End Region

#Region "グローバルデータからカットデータを取得する【TKY用】"
    '''=========================================================================
    '''<summary>グローバルデータからカットデータを取得する【TKY用】</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Function Get_CInfoArray(ByVal pCnt As Integer, ByVal pRCnt As Integer) As String

        Get_CInfoArray = ""

        Get_CInfoArray = Right(Space(4) & typResistorInfoArray(pRCnt).intResNo, 4) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(2) & typResistorInfoArray(pRCnt).ArrCut(pCnt).intCutNo, 2) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(6) & typResistorInfoArray(pRCnt).ArrCut(pCnt).intDelayTime, 6) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(9) & typResistorInfoArray(pRCnt).ArrCut(pCnt).dblStartPointX.ToString("0.0000"), 9) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(9) & typResistorInfoArray(pRCnt).ArrCut(pCnt).dblStartPointY.ToString("0.0000"), 9) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(6) & typResistorInfoArray(pRCnt).ArrCut(pCnt).dblCutSpeed.ToString("0.0"), 6) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(5) & typResistorInfoArray(pRCnt).ArrCut(pCnt).dblQRate.ToString("0.0"), 5) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(8) & typResistorInfoArray(pRCnt).ArrCut(pCnt).dblCutOff.ToString("0.000"), 8) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(6) & typResistorInfoArray(pRCnt).ArrCut(pCnt).dblJudgeLevel.ToString("0.0"), 6) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(2) & typResistorInfoArray(pRCnt).ArrCut(pCnt).strCutType, 2) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(2) & typResistorInfoArray(pRCnt).ArrCut(pCnt).intCutDir, 2) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(8) & typResistorInfoArray(pRCnt).ArrCut(pCnt).dblMaxCutLength.ToString("0.0000"), 8) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(8) & typResistorInfoArray(pRCnt).ArrCut(pCnt).dblR1.ToString("0.0000"), 8) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(6) & typResistorInfoArray(pRCnt).ArrCut(pCnt).dblLTurnPoint.ToString("0.0"), 6) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(8) & typResistorInfoArray(pRCnt).ArrCut(pCnt).dblMaxCutLengthL.ToString("0.0000"), 8) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(8) & typResistorInfoArray(pRCnt).ArrCut(pCnt).dblR2.ToString("0.0000"), 8) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(8) & typResistorInfoArray(pRCnt).ArrCut(pCnt).dblMaxCutLengthHook.ToString("0.0000"), 8) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(6) & typResistorInfoArray(pRCnt).ArrCut(pCnt).intIndexCnt, 6) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(2) & typResistorInfoArray(pRCnt).ArrCut(pCnt).intMeasMode, 2) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(6) & typResistorInfoArray(pRCnt).ArrCut(pCnt).dblCutSpeed2.ToString("0.0"), 6) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(5) & typResistorInfoArray(pRCnt).ArrCut(pCnt).dblQRate2.ToString("0.0"), 5) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(4) & typResistorInfoArray(pRCnt).ArrCut(pCnt).intCutAngle, 4) & "," & vbTab
        Get_CInfoArray = Get_CInfoArray & Right(Space(8) & typResistorInfoArray(pRCnt).ArrCut(pCnt).dblPitch.ToString("0.0000"), 8) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(2) & typResistorInfoArray(pRCnt).ArrCut(pCnt).intStepDir, 2) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(5) & typResistorInfoArray(pRCnt).ArrCut(pCnt).intCutCnt, 5) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(6) & typResistorInfoArray(pRCnt).ArrCut(pCnt).dblZoom.ToString("0.00"), 6) & ","
        Get_CInfoArray = Get_CInfoArray & Str2HexAsc(typResistorInfoArray(pRCnt).ArrCut(pCnt).strChar)

    End Function
#End Region
#End Region

    '======================================================================
    '  CHIP,NET用メソッド
    '======================================================================
#Region "【CHIP,NET用メソッド】"
#Region "ﾌﾟﾚｰﾄﾃﾞｰﾀのﾛｰﾄﾞ処理【CHIP,NET用】"
    '''=========================================================================
    '''<summary>ﾌﾟﾚｰﾄﾃﾞｰﾀのﾛｰﾄﾞ処理【CHIP,NET用】</summary>
    '''<remarks>TKYとCHIP、NETでファイル処理があまりに異なるため、
    '''         一旦、CHIPとNETはマージを行い、TKYは別関数の読出しとする。</remarks>
    '''=========================================================================
    Public Function FileLoadExe(ByRef fileName As String) As Short

        'Dim fn As Short                                 ' file no.
        Dim rBuff As String                             ' read buff
        Dim rType As Short                              ' data type
        Dim i As Short
        Dim rCnt As Short
        Dim rDATA() As String
        Dim blnTy2Data As Boolean                       ' CHIPのみ

        'On Error GoTo ErrTrap

        ' 各種データの初期化
        FileLoadExe = 0
        'V5.0.0.8①        Call Init_FileVer_Sub()                         ' ﾁｪｯｸ用ﾌｧｲﾙﾊﾞｰｼﾞｮﾝ設定
        blnTy2Data = False                              ' CHIPのみ
        'Call ClearBuff()                                       'V5.0.0.9②
        Dim pData As List(Of String) = New List(Of String)()    'V5.0.0.9②
        Call Init_AllTrmmingData()                      ' ﾄﾘﾐﾝｸﾞﾊﾟﾗﾒｰﾀの初期化

        ' テキストファイル オープン
        rType = -1
        rCnt = 0
        'fn = FreeFile()
        Try
            If (False = IO.File.Exists(fileName)) Then Throw New FileNotFoundException() 'V4.4.0.0-1
            'FileOpen(fn, fileName, OpenMode.Input)
            Using sr As New StreamReader(fileName, Encoding.GetEncoding("Shift_JIS"))   ' 旧ﾌｧｲﾙなので Shift_JIS    V4.4.0.0-1
                Dim retVal As Short
                'Do While Not EOF(1)
                Do While (False = sr.EndOfStream)   'V4.4.0.0-1
                    ' 1 line read
                    'rBuff = LineInput(fn)
                    rBuff = sr.ReadLine()   'V4.4.0.0-1

                    ' データチェック(セクション名の時はrType=データタイプが設定される)
                    retVal = FileLoadExe_Sub(rBuff, rType, rCnt, blnTy2Data)
                    If (retVal = -1) Then                       ' データ(セクション名以外) ?
                        Select Case rType
                            Case -1
                                ' ﾏｳｽﾎﾟｲﾝﾀをﾃﾞﾌｫﾙﾄに戻す
                                Call SetMousePointer(Form1, False)
                                ' "指定されたファイルはトリミングパラメータのデータではありません"
                                Call Form1.System1.TrmMsgBox(gSysPrm, MSG_16, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, gAppName)
                                FileLoadExe = 1
                                GoTo ERR_LINE
                            Case 0
                                ' file version check
                                If (gTkyKnd = KND_CHIP) Then
                                    retVal = FileVerCheck_CHIP(rBuff)
                                ElseIf (gTkyKnd = KND_NET) Then
                                    retVal = FileVerCheck_NET(rBuff)
                                End If
                                If retVal = 1 Then
                                    FileLoadExe = 1
                                    GoTo ERR_LINE
                                End If
                            Case 1 To 5 ' PLAT1-5
                                ' load data ⇒ stock buff
                                'pData(rCnt) = rBuff : rCnt = rCnt + 1
                                pData.Add(rBuff) : rCnt = rCnt + 1      'V5.0.0.9②
                            Case 6
                                ' load data ⇒ step buff(global)
                                MaxStep = rCnt
                                With typStepInfoArray(rCnt)
                                    i = 0
                                    rDATA = rBuff.Split(",")
                                    '----- V1.14.0.0⑥↓ -----
                                    '.intSP1 = CShort(rDATA(i)) : i = i + 1  ' ｽﾃｯﾌﾟ番号
                                    '.intSP2 = CShort(rDATA(i)) : i = i + 1  ' ﾌﾞﾛｯｸ数
                                    '.dblSP3 = CDbl(rDATA(i)) : i = i + 1    ' ｽﾃｯﾌﾟ間ｲﾝﾀｰﾊﾞﾙ
                                    '----- V1.14.0.0⑥↑ -----
                                End With
                                rCnt = rCnt + 1
                            Case 7
                                'load data ⇒ resistor buff(global)
                                'V4.7.3.1②                        Call GetResistorData(rBuff) V6.1.3.0①
                                Call GetResistorData(rBuff, rCnt)         'V4.7.3.1② V6.1.3.0①
                                rCnt = rCnt + 1
                                System.Windows.Forms.Application.DoEvents()
                            Case 8
                                ' load data ⇒ cut buff(global)
                                'V4.7.3.1②                        Call GetCutData(rBuff)           V6.1.3.0①       
                                Call GetCutData(rBuff, rCnt)            'V4.7.3.1② V6.1.3.0①
                                System.Windows.Forms.Application.DoEvents()
                            Case 9
                                ' load data ⇒ group buff(global)
                                If (gTkyKnd = KND_CHIP) Then
                                    ' グループ情報の取得
                                    If FileIO.FileVersion >= 3 Then
                                        MaxGrp = rCnt
                                        With typGrpInfoArray(rCnt)
                                            i = 0
                                            rDATA = rBuff.Split(",")
                                            '----- V1.14.0.0⑥↓ -----
                                            '.intGP1 = CShort(rDATA(i)) : i = i + 1  ' ｸﾞﾙｰﾌﾟ番号
                                            '.intGP2 = CShort(rDATA(i)) : i = i + 1  ' 抵抗数
                                            '.dblGP3 = CDbl(rDATA(i)) : i = i + 1    ' ｸﾞﾙｰﾌﾟ間ｲﾝﾀｰﾊﾞﾙ
                                            '----- V1.14.0.0⑥↑ -----
                                        End With
                                        rCnt = rCnt + 1
                                    End If

                                ElseIf (gTkyKnd = KND_NET) Then
                                    ' サーキット情報の取得
                                    With typCirAxisInfoArray(rCnt)
                                        '----- V1.14.0.0⑥↓ -----
                                        ' サーキットティーチングコマンドが有効時に設定する
                                        If (Form1.stFNC(F_CIRCUIT).iDEF = 1) Then
                                            i = 0
                                            rDATA = rBuff.Split(",")
                                            .intCaP1 = CInt(rDATA(i)) : i = i + 1        ' ｽﾃｯﾌﾟ番号
                                            .dblCaP2 = CDbl(rDATA(i)) : i = i + 1        ' 座標X
                                            .dblCaP3 = CDbl(rDATA(i)) : i = i + 1        ' 座標Y
                                        End If
                                        '----- V1.14.0.0⑥↑ -----
                                    End With
                                    rCnt = rCnt + 1
                                End If

                            Case 10
                                'load data ⇒ Ty2 Data (global)
                                If (gTkyKnd = KND_CHIP) Then
                                    'If (SL432HW_FileVer >= 3) Then                                 ' V1.23.0.0⑧
                                    If (FileIO.FileVersion >= 3) Or (FILETYPE_K = FILETYPE04_K) Then   ' V1.23.0.0⑧
                                        MaxTy2 = rCnt                                   ' 実際のTy2ﾌﾞﾛｯｸ件数
                                        '----- V1.14.0.0⑥↓ -----
                                        ' TY2コマンドが有効時に設定する
                                        If (Form1.stFNC(F_TY2).iDEF = 1) Then
                                            With typTy2InfoArray(rCnt)
                                                i = 0
                                                rDATA = rBuff.Split(",")
                                                .intTy21 = CShort(rDATA(i)) : i = i + 1     ' ﾌﾞﾛｯｸNo.
                                                .dblTy22 = CDbl(rDATA(i)) : i = i + 1       ' ｽﾃｯﾌﾟｲﾝﾀｰﾊﾞﾙ
                                            End With
                                        End If
                                        '----- V1.14.0.0⑥↑ -----
                                        rCnt = rCnt + 1
                                    End If

                                ElseIf (gTkyKnd = KND_NET) Then
                                    With typCirInInfoArray(rCnt)
                                        i = 0
                                        rDATA = rBuff.Split(",")
                                        '----- V1.14.0.0⑥↓ -----
                                        '.intCiP1 = CInt(rDATA(i)) : i = i + 1            ' ｽﾃｯﾌﾟ番号
                                        '.intCiP2 = CInt(rDATA(i)) : i = i + 1            ' ｻｰｷｯﾄ数
                                        '.dblCiP3 = CDbl(rDATA(i)) : i = i + 1            ' ｻｰｷｯﾄ間ｲﾝﾀｰﾊﾞﾙ
                                        '----- V1.14.0.0⑥↑ -----
                                    End With
                                    rCnt = rCnt + 1
                                End If

                        End Select
                    End If
                Loop
ERR_LINE:
            End Using

            ' error?
            If FileLoadExe = 0 Then
                ' load data ⇒ plate buff(global)
                'Call SetFileLoadPlateData()
                Call SetFileLoadPlateData(pData)        'V5.0.0.9②
                ' TY2のﾃﾞｰﾀが存在しない場合、現状のﾃﾞｰﾀから作成する。
                If Not blnTy2Data Then
                    Call GetTy2StepPos(1, False)
                End If
            Else
                ' mouse pointer default
                Call SetMousePointer(Form1, True)
            End If
            ' NGｶｯﾄﾎﾟｲﾝﾄ取得
            'Call SetNG_MarkingPos()

            Exit Function

        Catch ex As FileNotFoundException
            FileLoadExe = 1
            ' 指定されたファイルは存在しません
            Call Form1.System1.TrmMsgBox(gSysPrm, MSG_15, MsgBoxStyle.OkOnly, gAppName)

        Catch ex As Exception
            FileLoadExe = 1
            MessageBox.Show("##ERROR INFO<DATA LOAD>##" & vbCrLf &
                   "  ERROR DISCRIPTION" & vbTab & " : " & Err.Description & vbCrLf &
                   "  ERROR NUMBER     " & vbTab & " : " & Err.Number & vbCrLf)
        End Try

    End Function
#End Region

#Region "データチェック処理【CHIP,NET用】"
    '''=========================================================================
    '''<summary>データチェック処理【CHIP,NET用】</summary>
    '''<param name="rBuff">     (INP) データ</param>
    '''<param name="rType">     (OUT) データタイプ</param>
    '''<param name="rCnt">      (OUT) rCnt</param>
    '''<param name="blnTy2Data">(OUT) TY2データ(CHIPのみ)</param>
    '''<returns>0=データタイプ設定, -1=その他データ</returns>
    '''=========================================================================
    Private Function FileLoadExe_Sub(ByRef rBuff As String, ByRef rType As Short, ByRef rCnt As Short, ByRef blnTy2Data As Boolean) As Short

        ' データチェック
        FileLoadExe_Sub = 0                             ' Return値 = 正常 
        Select Case rBuff
            Case FILE_CONST_VERSION
                rType = 0                               ' file ver data
            Case FILE_CONST_PLATE_01
                rType = 1                               ' plate01 data
            Case FILE_CONST_PLATE_02
                rType = 2                               ' plate02 data
            Case FILE_CONST_PLATE_03
                rType = 3                               ' plate03 data
            Case FILE_CONST_PLATE_04
                rType = 4                               ' plate04 data
            Case FILE_CONST_PLATE_05
                rType = 5                               ' plate05 data
            Case FILE_CONST_STEPDATA
                rType = 6 : rCnt = 1                    ' step data
            Case FILE_CONST_RESISTOR
                rType = 7 : rCnt = 1                    ' resistor data
            Case FILE_CONST_CUT_DATA
                rType = 8 : rCnt = 0                    ' cut data
            Case FILE_CONST_GRP_DATA                    ' CHIPのみ 
                If (gTkyKnd = KND_CHIP) Then
                    rType = 9 : rCnt = 1                ' group data
                Else
                    FileLoadExe_Sub = -1                ' Return値 = その他データ
                End If
            Case FILE_CONST_TY2_DATA                    ' CHIPのみ 
                If (gTkyKnd = KND_CHIP) Then
                    rType = 10 : rCnt = 1               ' TY2 data
                    blnTy2Data = True
                Else
                    FileLoadExe_Sub = -1                ' Return値 = その他データ
                End If

            Case FILE_CONST_CIR_DATA                    ' NETのみ 
                If (gTkyKnd = KND_NET) Then
                    rType = 9 : rCnt = 1                 ' circuit data
                Else
                    FileLoadExe_Sub = -1                ' Return値 = その他データ
                End If
            Case FILE_CONST_CIRIDATA                    ' NETのみ 
                If (gTkyKnd = KND_NET) Then
                    rType = 10 : rCnt = 1                ' circuit interval data
                Else
                    FileLoadExe_Sub = -1                ' Return値 = その他データ
                End If
            Case Else
                FileLoadExe_Sub = -1                    ' Return値 = その他データ
        End Select

    End Function

#End Region

#Region "ﾌﾟﾚｰﾄﾃﾞｰﾀのｾｰﾌﾞ処理【CHIP,NET用】    'V4.4.0.0-1 ｺﾒﾝﾄｱｳﾄ"
    ''''=========================================================================
    ''''<summary>ﾌﾟﾚｰﾄﾃﾞｰﾀのｾｰﾌﾞ処理【CHIP,NET用】</summary>
    ''''<param name="fileName">(INP) ファイルパス名</param>
    ''''<returns>0=正常, 0以外=エラー</returns>
    ''''=========================================================================
    'Public Function FileSaveExe(ByRef fileName As String) As Short

    '    Dim fn As Short
    '    Dim i As Short
    '    Dim ii As Short
    '    Dim ChipNum As Short
    '    Dim CutNum As Short
    '    Dim RegNo As Short
    '    Dim uDATA As String
    '    Dim dData As String
    '    '                                                   ' NET用  
    '    Dim CirNum As Integer                               ' ｻｰｷｯﾄ数
    '    Dim GrpNum As Integer

    '    Dim strMSG As String

    '    Try
    '        FileSaveExe = 0
    '        uDATA = ""
    '        dData = ""
    '        If (gTkyKnd = KND_NET) Then
    '            CirNum = typPlateInfo.intCircuitCntInBlock  ' 1ﾌﾞﾛｯｸ内ｻｰｷｯﾄ数
    '        End If

    '        ' file open(save)
    '        fn = FreeFile()
    '        FileOpen(fn, fileName, OpenMode.Output)

    '        '-----------------------------------------------------------------------
    '        '   FILE VERSION
    '        '-----------------------------------------------------------------------
    '        SL432HW_FileVer = NewFileVer
    '        If (gTkyKnd = KND_CHIP) Then
    '            If (gSysPrm.stCTM.giSPECIAL = customKOAEW) Then
    '                SL432HW_FileVer = 5
    '            End If
    '        End If

    '        PrintLine(fn, FILE_CONST_VERSION)
    '        Select Case SL432HW_FileVer
    '            Case 1 : PrintLine(fn, FILETYPE01)
    '            Case 2 : PrintLine(fn, FILETYPE02)
    '            Case 3 : PrintLine(fn, FILETYPE03)
    '            Case 4 : PrintLine(fn, FILETYPE04)
    '            Case 5 : PrintLine(fn, FILETYPE05)
    '            Case 6 : PrintLine(fn, FILETYPE06)
    '            Case 10 : PrintLine(fn, FILETYPE10)
    '            Case Else : PrintLine(fn, "NOT FILE VERSION")
    '        End Select

    '        '-----------------------------------------------------------------------
    '        '   PLATE DATA
    '        '-----------------------------------------------------------------------
    '        With typPlateInfo
    '            '-----------------------------------------------------------------------
    '            '   PLATE 01
    '            '-----------------------------------------------------------------------
    '            PrintLine(fn, FILE_CONST_PLATE_01)
    '            PrintLine(fn, .strDataName)                             ' ﾃﾞｰﾀNo.
    '            If (gTkyKnd = KND_NET) Then
    '                PrintLine(fn, CStr(.intMeasType))                   ' ﾄﾘﾑﾓｰﾄﾞ
    '            End If
    '            PrintLine(fn, CStr(.intDirStepRepeat))                  ' ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄ
    '            PrintLine(fn, .intBlockCntXDir & "," & .intBlockCntYDir) ' ﾌﾞﾛｯｸ数XY
    '            PrintLine(fn, .dblTableOffsetXDir.ToString("0.0000") & "," & .dblTableOffsetYDir.ToString("0.0000"))  ' ﾃｰﾌﾞﾙ位置ｵﾌｾｯﾄXY
    '            PrintLine(fn, .dblBpOffSetXDir.ToString("0.0000") & "," & .dblBpOffSetYDir.ToString("0.0000"))        ' ﾋﾞｰﾑ位置ｵﾌｾｯﾄXY
    '            PrintLine(fn, .dblAdjOffSetXDir.ToString("0.0000") & "," & .dblAdjOffSetYDir.ToString("0.0000"))      ' ｱｼﾞｬｽﾄ位置ｵﾌｾｯﾄXY
    '            PrintLine(fn, CStr(.intNGMark))                         ' NGﾏｰｷﾝｸﾞ
    '            PrintLine(fn, CStr(.intDelayTrim))                      ' ﾃﾞｨﾚｲﾄﾘﾑ
    '            PrintLine(fn, CStr(.intNgJudgeUnit))                    ' NG判定単位
    '            PrintLine(fn, CStr(.intNgJudgeLevel))                   ' NG判定基準
    '            PrintLine(fn, .dblZOffSet.ToString("0.0000"))           ' ﾌﾟﾛｰﾌﾞZｵﾌｾｯﾄ
    '            PrintLine(fn, .dblZStepUpDist.ToString("0.0000"))       ' ﾌﾟﾛｰﾌﾞｽﾃｯﾌﾟ上昇距離
    '            PrintLine(fn, .dblZWaitOffset.ToString("0.0000"))       ' ﾌﾟﾛｰﾌﾞ待機Zｵﾌｾｯﾄ
    '            '-----------------------------------------------------------------------
    '            '   PLATE 03
    '            '-----------------------------------------------------------------------
    '            PrintLine(fn, FILE_CONST_PLATE_03)
    '            If (gTkyKnd = KND_CHIP) Then
    '                PrintLine(fn, CStr(.intResistDir))                                                                              ' 抵抗並び方向
    '                PrintLine(fn, CStr(.intResistCntInGroup))                                                                       ' 1ｸﾞﾙｰﾌﾟ内抵抗数
    '                PrintLine(fn, .intGroupCntInBlockXBp & "," & .intGroupCntInBlockYStage)                                         ' ﾌﾞﾛｯｸ内ｸﾞﾙｰﾌﾟ数XY
    '                PrintLine(fn, .dblBpGrpItv.ToString("0.0000") & "," & .dblStgGrpItvY.ToString("0.0000"))                        ' ｸﾞﾙｰﾌﾟ間隔BP(X),Stage(Y)
    '                'PrintLine(fn, .dblGroupItvXDir.ToString("0.0000") & "," & .dblGroupItvYDir.ToString("0.0000"))                 ' ｸﾞﾙｰﾌﾟ間隔XY
    '                PrintLine(fn, .dblChipSizeXDir.ToString("0.0000") & "," & .dblChipSizeYDir.ToString("0.0000"))                  ' ﾁｯﾌﾟｻｲｽﾞXY
    '                PrintLine(fn, .dblStepOffsetXDir.ToString("0.0000") & "," & .dblStepOffsetYDir.ToString("0.0000"))              ' ｽﾃｯﾌﾟｵﾌｾｯﾄ量XY
    '                PrintLine(fn, .dblBlockSizeReviseXDir.ToString("0.0000") & "," & .dblBlockSizeReviseYDir.ToString("0.0000"))    ' ﾌﾞﾛｯｸｻｲｽﾞ補正XY
    '                PrintLine(fn, .dblBlockItvXDir.ToString("0.0000") & "," & .dblBlockItvYDir.ToString("0.0000"))                  ' ﾌﾞﾛｯｸ間隔XY
    '                PrintLine(fn, CStr(.intContHiNgBlockCnt))                                                                       ' 連続NG-HIGH抵抗ﾌﾞﾛｯｸ数
    '            ElseIf (gTkyKnd = KND_NET) Then
    '                PrintLine(fn, .intPlateCntXDir & "," & .intPlateCntYDir)                                                        ' ﾌﾟﾚｰﾄ数XY
    '                PrintLine(fn, .dblPlateItvXDir.ToString("0.0000") & "," & .dblPlateItvYDir.ToString("0.0000"))                  ' ﾌﾟﾚｰﾄ間隔XY
    '                PrintLine(fn, CStr(.intCircuitCntInBlock))                                                                      ' 1ﾌﾞﾛｯｸ内ｻｰｷｯﾄ数
    '                PrintLine(fn, .dblCircuitSizeXDir.ToString("0.0000") & "," & .dblCircuitSizeYDir.ToString("0.0000"))            ' ｻｰｷｯﾄｻｲｽﾞXY
    '                PrintLine(fn, CStr(.intResistCntInGroup))                                                                       ' 1ｻｰｷｯﾄ内抵抗数
    '                PrintLine(fn, .intGroupCntInBlockXBp & "," & .intGroupCntInBlockYStage)                                         ' ﾌﾞﾛｯｸ内ｸﾞﾙｰﾌﾟ数XY
    '                PrintLine(fn, .dblBlockSizeReviseXDir.ToString("0.0000") & "," & .dblBlockSizeReviseYDir.ToString("0.0000"))    ' ﾌﾞﾛｯｸｻｲｽﾞ補正XY
    '            End If
    '            '-----------------------------------------------------------------------
    '            '   PLATE 02
    '            '-----------------------------------------------------------------------
    '            PrintLine(fn, FILE_CONST_PLATE_02)
    '            PrintLine(fn, CStr(.intReviseMode))                                                                                 ' 補正ﾓｰﾄﾞ
    '            PrintLine(fn, CStr(.intManualReviseType))                                                                           ' 補正方法
    '            PrintLine(fn, .dblReviseCordnt1XDir.ToString("0.0000") & "," & .dblReviseCordnt1YDir.ToString("0.0000"))            ' 補正位置座標1XY
    '            PrintLine(fn, .dblReviseCordnt2XDir.ToString("0.0000") & "," & .dblReviseCordnt2YDir.ToString("0.0000"))            ' 補正位置座標2XY
    '            PrintLine(fn, .dblReviseOffsetXDir.ToString("0.0000") & "," & .dblReviseOffsetYDir.ToString("0.0000"))              ' 補正ﾎﾟｼﾞｼｮﾝｵﾌｾｯﾄXY
    '            PrintLine(fn, CStr(.intRecogDispMode))                                                                              ' 認識ﾃﾞｰﾀ表示ﾓｰﾄﾞ
    '            If gSysPrm.stDEV.giEXCAM = 1 Then
    '                PrintLine(fn, gSysPrm.stGRV.gfEXCAM_PixelX.ToString("0.0000") & "," & gSysPrm.stGRV.gfEXCAM_PixelY.ToString("0.0000")) ' ﾋﾟｸｾﾙ値XY
    '            Else
    '                PrintLine(fn, gSysPrm.stGRV.gfPixelX.ToString("0.0000") & "," & gSysPrm.stGRV.gfPixelY.ToString("0.0000"))             ' ﾋﾟｸｾﾙ値XY
    '            End If
    '            PrintLine(fn, .intRevisePtnNo1 & "," & .intRevisePtnNo2)                    ' 補正位置ﾊﾟﾀｰﾝNo1,2
    '            ' 補正位置パターン番号のグループ番号設定
    '            If SL432HW_FileVer > 6 Then
    '                PrintLine(fn, .intRevisePtnNo1GroupNo & "," & .intRevisePtnNo2GroupNo)  ' 補正位置グループNo1,2
    '            End If

    '            If (gTkyKnd = KND_CHIP) Then
    '                PrintLine(fn, gSysPrm.stDEV.gfRot_X1.ToString("0.000") & "," & gSysPrm.stDEV.gfRot_Y1.ToString("0.000"))    ' XY方向の回転中心 
    '                PrintLine(fn, .dblThetaAxis.ToString("0.00000"))                        'θ軸
    '            ElseIf (gTkyKnd = KND_NET) Then
    '                PrintLine(fn, gSysPrm.stDEV.gfRot_X1.ToString("0.000") & "," & gSysPrm.stDEV.gfRot_X1.ToString("0.000"))    ' XY方向の回転中心 
    '                PrintLine(fn, .dblThetaAxis.ToString("0.00000"))                        'θ軸
    '            End If

    '            If (gTkyKnd = KND_CHIP) Then
    '                If (gSysPrm.stCTM.giSPECIAL = customKOAEW) Then                         ' KOA(EW殿)ならVer1.40形式とする
    '                Else
    '                    PrintLine(fn, .dblTThetaOffset.ToString("0.00000"))                 ' Ｔθオフセット
    '                    PrintLine(fn, .dblTThetaBase1XDir.ToString("0.0000"))               ' Ｔθ基準位置１X
    '                    PrintLine(fn, .dblTThetaBase1YDir.ToString("0.0000"))               ' Ｔθ基準位置１Y
    '                    PrintLine(fn, .dblTThetaBase2XDir.ToString("0.0000"))               ' Ｔθ基準位置２X
    '                    PrintLine(fn, .dblTThetaBase2YDir.ToString("0.0000"))               ' Ｔθ基準位置２Y
    '                End If
    '            End If
    '            '-----------------------------------------------------------------------
    '            '   PLATE 04
    '            '-----------------------------------------------------------------------
    '            PrintLine(fn, FILE_CONST_PLATE_04)
    '            PrintLine(fn, .dblCaribBaseCordnt1XDir.ToString("0.0000") & "," & .dblCaribBaseCordnt1YDir.ToString("0.0000")) ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標1XY
    '            PrintLine(fn, .dblCaribBaseCordnt2XDir.ToString("0.0000") & "," & .dblCaribBaseCordnt2YDir.ToString("0.0000")) ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標2XY
    '            PrintLine(fn, .dblCaribTableOffsetXDir.ToString("0.0000") & "," & .dblCaribTableOffsetYDir.ToString("0.0000")) ' ｷｬﾘﾌﾞﾚｰｼｮﾝｵﾌｾｯﾄXY
    '            PrintLine(fn, .intCaribPtnNo1 & "," & .intCaribPtnNo2)                      ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾊﾟﾀｰﾝ登録No1,2
    '            ' キャリブレーションパターン番号のグループ番号設定
    '            If SL432HW_FileVer > 6 Then
    '                PrintLine(fn, .intCaribPtnNo1GroupNo & "," & .intCaribPtnNo2GroupNo)    ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾊﾟﾀｰﾝNo1,2
    '            End If
    '            PrintLine(fn, .dblCaribCutLength.ToString("0.0000"))                        ' ｷｬﾘﾌﾞﾚｰｼｮﾝｶｯﾄ長
    '            PrintLine(fn, .dblCaribCutSpeed.ToString("0.0000"))                         ' ｷｬﾘﾌﾞﾚｰｼｮﾝｶｯﾄ速度
    '            PrintLine(fn, .dblCaribCutQRate.ToString("0.0"))                            ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾚｰｻﾞQﾚｰﾄ
    '            PrintLine(fn, .dblCutPosiReviseOffsetXDir.ToString("0.0000") & "," & .dblCutPosiReviseOffsetYDir.ToString("0.0000")) ' ｶｯﾄ位置補正ﾃｰﾌﾞﾙｵﾌｾｯﾄXY
    '            PrintLine(fn, CStr(.intCutPosiRevisePtnNo))                                 ' ｶｯﾄ位置補正ﾊﾟﾀｰﾝ登録No
    '            PrintLine(fn, .dblCutPosiReviseCutLength.ToString("0.0000"))                ' ｶｯﾄ位置補正ｶｯﾄ長
    '            PrintLine(fn, .dblCutPosiReviseCutSpeed.ToString("0.0000"))                 ' ｶｯﾄ位置補正ｶｯﾄ速度
    '            PrintLine(fn, .dblCutPosiReviseCutQRate.ToString("0.0"))                    ' ｶｯﾄ位置補正ﾚｰｻﾞQﾚｰﾄ
    '            PrintLine(fn, CStr(.intCutPosiReviseGroupNo))                               ' ｸﾞﾙｰﾌﾟNo
    '            '-----------------------------------------------------------------------
    '            '   PLATE 05
    '            '-----------------------------------------------------------------------
    '            PrintLine(fn, FILE_CONST_PLATE_05)
    '            If (gTkyKnd = KND_CHIP) Then
    '                PrintLine(fn, CStr(.intMaxTrimNgCount))                     ' ﾄﾘﾐﾝｸﾞNGｶｳﾝﾀ(上限)
    '                PrintLine(fn, CStr(.intMaxBreakDischargeCount))             ' 割れ欠け排出ｶｳﾝﾀ(上限)
    '                PrintLine(fn, CStr(.intTrimNgCount))                        ' 連続ﾄﾘﾐﾝｸﾞNG枚数
    '                PrintLine(fn, CStr(.intRetryProbeCount))                    ' 再ﾌﾟﾛｰﾋﾞﾝｸﾞ回数
    '                PrintLine(fn, .dblRetryProbeDistance.ToString("0.0000"))    ' 再ﾌﾟﾛｰﾋﾞﾝｸﾞ移動量
    '                PrintLine(fn, CStr(.intLedCtrl))                            ' LED制御
    '                ' 自動ﾚｰｻﾞﾊﾟﾜｰ調整項目
    '                If (gSysPrm.stCTM.giSPECIAL = customKOAEW) Then             ' KOA(EW殿)ならVer1.40形式とする
    '                Else
    '                    PrintLine(fn, CStr(.intPowerAdjustMode))                ' パワー調整モード
    '                    PrintLine(fn, .dblPowerAdjustTarget.ToString("0.00"))   ' 調整目標パワー
    '                    PrintLine(fn, .dblPowerAdjustQRate.ToString("0.0"))     ' パワー調整Qレート
    '                    PrintLine(fn, .dblPowerAdjustToleLevel.ToString("0.00")) ' パワー調整許容範囲
    '                End If

    '                PrintLine(fn, CStr(.intGpibCtrl))                           ' GP-IB制御
    '                PrintLine(fn, CStr(.intGpibDefDelimiter))                   ' 初期設定(ﾃﾞﾘﾐﾀ)
    '                PrintLine(fn, CStr(.intGpibDefTimiout))                     ' 初期設定(ﾀｲﾑｱｳﾄ)
    '                PrintLine(fn, CStr(.intGpibDefAdder))                       ' 初期設定(機器ｱﾄﾞﾚｽ)
    '                PrintLine(fn, CStr(.strGpibInitCmnd1))                      ' 初期化ｺﾏﾝﾄﾞ
    '                PrintLine(fn, CStr(.strGpibInitCmnd2))                      ' 初期化ｺﾏﾝﾄﾞ
    '                PrintLine(fn, CStr(.strGpibTriggerCmnd))                    ' ﾄﾘｶﾞｺﾏﾝﾄﾞ
    '                PrintLine(fn, CStr(.intGpibMeasSpeed))                      ' 測定速度
    '                PrintLine(fn, CStr(.intGpibMeasMode))                       ' 測定モード

    '            ElseIf (gTkyKnd = KND_NET) Then
    '                PrintLine(fn, CStr(.intRetryProbeCount))                    ' 再ﾌﾟﾛｰﾋﾞﾝｸﾞ回数
    '                PrintLine(fn, .dblRetryProbeDistance.ToString("0.0000"))    ' 再ﾌﾟﾛｰﾋﾞﾝｸﾞ移動量
    '            End If
    '        End With

    '        '-----------------------------------------------------------------------
    '        '   STEP DATA(TKY)
    '        '-----------------------------------------------------------------------
    '        If (gTkyKnd = KND_CHIP) Then
    '            ' STEP
    '            PrintLine(fn, FILE_CONST_STEPDATA)
    '            For i = 1 To MaxStep
    '                With typStepInfoArray(i)
    '                    ' ｽﾃｯﾌﾟ番号+ﾌﾞﾛｯｸ数+ｽﾃｯﾌﾟ間ｲﾝﾀｰﾊﾞﾙ
    '                    PrintLine(fn, Right(Space(3) & .intSP1, 3) & "," & Right(Space(2) & .intSP2, 3) & "," & Right(Space(7) & .dblSP3.ToString("0.0000"), 7))
    '                End With
    '            Next i

    '            ' GROUP DATA
    '            PrintLine(fn, FILE_CONST_GRP_DATA)
    '            For i = 1 To MaxGrp
    '                With typGrpInfoArray(i)
    '                    'ｽﾃｯﾌﾟ番号+ﾌﾞﾛｯｸ数+ｽﾃｯﾌﾟ間ｲﾝﾀｰﾊﾞﾙ
    '                    PrintLine(fn, Right(Space(3) & .intGP1, 3) & "," & Right(Space(2) & .intGP2, 2) & "," & Right(Space(7) & .dblGP3.ToString("0.0000"), 7))
    '                End With
    '            Next i

    '            ' TY2 DATA
    '            PrintLine(fn, FILE_CONST_TY2_DATA)
    '            For i = 1 To MaxTy2
    '                With typTy2InfoArray(i)
    '                    'ｽﾃｯﾌﾟ番号+ﾌﾞﾛｯｸ数+ｽﾃｯﾌﾟ間ｲﾝﾀｰﾊﾞﾙ
    '                    PrintLine(fn, Right(Space(3) & .intTy21, 3) & "," & Right(Space(7) & .dblTy22.ToString("0.0000"), 9))
    '                End With
    '            Next i

    '        ElseIf (gTkyKnd = KND_NET) Then
    '            '-----------------------------------------------------------------------
    '            '   STEP DATA(NET)
    '            '-----------------------------------------------------------------------
    '            ' CIRCUIT
    '            PrintLine(fn, FILE_CONST_CIR_DATA)
    '            For i = 1 To CirNum
    '                With typCirAxisInfoArray(i)
    '                    ' ｽﾃｯﾌﾟ番号+座標X+座標Y
    '                    PrintLine(fn, Right(Space(3) & .intCaP1, 3) & "," & _
    '                                  Right(Space(7) & .dblCaP2.ToString("0.0000"), 7) & "," & _
    '                                  Right(Space(7) & .dblCaP3.ToString("0.0000"), 7))
    '                End With
    '            Next i
    '            ' CIRCUIT INTERVAL
    '            If typPlateInfo.intResistDir = 0 Then
    '                GrpNum = typPlateInfo.intGroupCntInBlockXBp     ' ｸﾞﾙｰﾌﾟ数X
    '            Else
    '                GrpNum = typPlateInfo.intGroupCntInBlockYStage
    '            End If
    '            PrintLine(fn, FILE_CONST_CIRIDATA)
    '            For i = 1 To GrpNum
    '                With typCirInInfoArray(i)
    '                    ' ｽﾃｯﾌﾟ番号+ｻｰｷｯﾄ数+ｻｰｷｯﾄ間ｲﾝﾀｰﾊﾞﾙ
    '                    PrintLine(fn, Right(Space(3) & .intCiP1, 3) & "," & Right(Space(3) & .intCiP2, 3) & "," & Right(Space(7) & .dblCiP3.ToString("0.0000"), 7))
    '                End With
    '            Next i
    '            ' STEP
    '            PrintLine(fn, FILE_CONST_STEPDATA)
    '            For i = 1 To MaxStep
    '                With typStepInfoArray(i)
    '                    ' ｽﾃｯﾌﾟ番号+ﾌﾞﾛｯｸ数+ｽﾃｯﾌﾟ間ｲﾝﾀｰﾊﾞﾙ
    '                    PrintLine(fn, Right(Space(3) & .intSP1, 3) & "," & Right(Space(2) & .intSP2, 2) & "," & Right(Space(7) & .dblSP3.ToString("0.0000"), 7))
    '                End With
    '            Next i
    '        End If

    '        '-----------------------------------------------------------------------
    '        '   RESISTOR
    '        '-----------------------------------------------------------------------
    '        PrintLine(fn, FILE_CONST_RESISTOR)
    '        Call GetChipNum(ChipNum)                    ' 抵抗数取得
    '        ii = Len(CStr(gSysPrm.stDEV.giDCScaner))
    '        For i = 1 To ChipNum

    '            With typResistorInfoArray(i)
    '                If (gTkyKnd = KND_CHIP) Then
    '                    '-----------------------------------------------------------
    '                    '   CHIP時
    '                    '-----------------------------------------------------------
    '                    ' 抵抗番号,判定測定, ﾌﾟﾛｰﾌﾞ番号HI, ﾌﾟﾛｰﾌﾞ番号LO, ﾌﾟﾛｰﾌﾞ番号AG1～AG5, EXTERNAL BIT, ﾎﾟｰｽﾞ,ﾄﾘﾐﾝｸﾞ目標値, ΔR, 切り上げ倍率
    '                    uDATA = Right(Space(3) & .intResNo, 4) & "," & Right(Space(4) & .intResMeasMode.ToString("0"), 4) & "," & Right(Space(ii) & .intProbHiNo, ii) & "," & Right(Space(ii) & .intProbLoNo, ii) & "," & Right(Space(ii) & .intProbAGNo1, ii) & "," & Right(Space(ii) & .intProbAGNo2, ii) & "," & Right(Space(ii) & .intProbAGNo3, ii) & "," & Right(Space(ii) & .intProbAGNo4, ii) & "," & Right(Space(ii) & .intProbAGNo5, ii) & "," & Right(Space(16) & .strExternalBits, 16) & "," & Right(Space(5) & .intPauseTime, 5) & "," & Right(Space(16) & .dblTrimTargetVal.ToString("0.00000"), 16) & "," & Right(Space(7) & .dblDeltaR.ToString("0.00"), 7) & "," & Right(Space(7) & .dblCutOffRatio.ToString("0.00"), 7) & ","
    '                    ' ｲﾆｼｬﾙﾃｽﾄ HI/LOﾘﾐｯﾄ, ﾌｧｲﾅﾙﾃｽﾄ HI/LOﾘﾐｯﾄ, ｶｯﾄ数
    '                    dData = Right(Space(7) & .dblInitTest_HighLimit.ToString("0.00"), 7) & "," & Right(Space(7) & .dblInitTest_LowLimit.ToString("0.00"), 7) & "," & Right(Space(7) & .dblFinalTest_HighLimit.ToString("0.00"), 7) & "," & Right(Space(7) & .dblFinalTest_LowLimit.ToString("0.00"), 7) & "," & Right(Space(2) & .intCutCount, 2)

    '                ElseIf (gTkyKnd = KND_NET) Then
    '                    '-----------------------------------------------------------
    '                    '   NET時
    '                    '-----------------------------------------------------------
    '                    ' 抵抗番号, 所属ｻｰｷｯﾄ, 判定測定, ﾌﾟﾛｰﾌﾞ番号HI, ﾌﾟﾛｰﾌﾞ番号LO, ﾌﾟﾛｰﾌﾞ番号AG1～AG5, EXTERNAL BIT, ﾎﾟｰｽﾞ, ﾄﾘﾑﾓｰﾄﾞ, ﾍﾞｰｽ抵抗, ﾄﾘﾐﾝｸﾞ目標値
    '                    uDATA = Right(Space(3) & .intResNo, 4) & "," & Right(Space(3) & .intCircuitGrp, 3) & "," & Right(Space(1) & .intResMeasMode, 1) & "," & _
    '                            Right(Space(ii) & .intProbHiNo, ii) & "," & Right(Space(ii) & .intProbLoNo, ii) & "," & _
    '                            Right(Space(ii) & .intProbAGNo1, ii) & "," & Right(Space(ii) & .intProbAGNo2, ii) & "," & _
    '                            Right(Space(ii) & .intProbAGNo3, ii) & "," & Right(Space(ii) & .intProbAGNo4, ii) & "," & Right(Space(ii) & .intProbAGNo5, ii) & "," & _
    '                            Right(Space(16) & .strExternalBits, 16) & "," & Right(Space(5) & .intPauseTime, 5) & "," & _
    '                            Right(Space(1) & .intTargetValType, 1) & "," & Right(Space(3) & .intBaseResNo, 3) & "," & _
    '                            Right(Space(16) & .dblTrimTargetVal.ToString("0.00000"), 16) & ","
    '                    ' 電圧変化ｽﾛｰﾌﾟ, ｲﾆｼｬﾙﾃｽﾄ HI/LOﾘﾐｯﾄ, ﾌｧｲﾅﾙﾃｽﾄ HI/LOﾘﾐｯﾄ, ｶｯﾄ数
    '                    dData = Right(Space(1) & .intSlope, 1) & "," & _
    '                            Right(Space(7) & .dblInitTest_HighLimit.ToString("0.00"), 7) & "," & Right(Space(7) & .dblInitTest_LowLimit.ToString("0.00"), 7) & "," & _
    '                            Right(Space(7) & .dblFinalTest_HighLimit.ToString("0.00"), 7) & "," & Right(Space(7) & .dblFinalTest_LowLimit.ToString("0.00"), 7) & "," & _
    '                            Right(Space(2) & .intCutCount, 2)
    '                End If
    '                PrintLine(fn, uDATA & dData)
    '            End With
    '        Next i

    '        If typPlateInfo.intNGMark = 1 Then
    '            With typResistorInfoArray(1000)
    '                If (gTkyKnd = KND_CHIP) Then
    '                    '-----------------------------------------------------------
    '                    '   CHIP時
    '                    '-----------------------------------------------------------
    '                    ' 抵抗番号, 判定測定, ﾌﾟﾛｰﾌﾞ番号HI, ﾌﾟﾛｰﾌﾞ番号LO,ﾌﾟﾛｰﾌﾞ番号AG1～AG5,EXTERNAL BIT,ﾎﾟｰｽﾞ,ﾄﾘﾐﾝｸﾞ目標値,ΔR,切り上げ倍率
    '                    uDATA = Right(Space(3) & .intResNo, 4) & "," & Right(Space(4) & .intResMeasMode.ToString("0"), 4) & "," & Right(Space(ii) & .intProbHiNo, ii) & "," & Right(Space(ii) & .intProbLoNo, ii) & "," & Right(Space(ii) & .intProbAGNo1, ii) & "," & Right(Space(ii) & .intProbAGNo2, ii) & "," & Right(Space(ii) & .intProbAGNo3, ii) & "," & Right(Space(ii) & .intProbAGNo4, ii) & "," & Right(Space(ii) & .intProbAGNo5, ii) & "," & Right(Space(16) & .strExternalBits, 16) & "," & Right(Space(5) & .intPauseTime, 5) & "," & Right(Space(16) & .dblTrimTargetVal.ToString("0.00000"), 16) & "," & Right(Space(7) & .dblDeltaR.ToString("0.00"), 7) & "," & Right(Space(7) & .dblCutOffRatio.ToString("0.00"), 7) & ","
    '                    ' ｲﾆｼｬﾙﾃｽﾄ HI/LOﾘﾐｯﾄ,ﾌｧｲﾅﾙﾃｽﾄ HI/LOﾘﾐｯﾄ,ｶｯﾄ数
    '                    dData = Right(Space(7) & .dblInitTest_HighLimit.ToString("0.00"), 7) & "," & Right(Space(7) & .dblInitTest_LowLimit.ToString("0.00"), 7) & "," & Right(Space(7) & .dblFinalTest_HighLimit.ToString("0.00"), 7) & "," & Right(Space(7) & .dblFinalTest_LowLimit.ToString("0.00"), 7) & "," & Right(Space(2) & .intCutCount, 2)

    '                ElseIf (gTkyKnd = KND_NET) Then
    '                    '-----------------------------------------------------------
    '                    '   NET時
    '                    '-----------------------------------------------------------
    '                    ' 抵抗番号,所属ｻｰｷｯﾄ,判定測定,ﾌﾟﾛｰﾌﾞ番号HI,ﾌﾟﾛｰﾌﾞ番号LO,ﾌﾟﾛｰﾌﾞ番号AG1～AG5,EXTERNAL BIT,ﾎﾟｰｽﾞ,ﾄﾘﾑﾓｰﾄﾞ,ﾍﾞｰｽ抵抗,ﾄﾘﾐﾝｸﾞ目標値
    '                    uDATA = Right(Space(3) & .intResNo, 4) & "," & Right(Space(3) & .intCircuitGrp, 3) & "," & Right(Space(1) & .intResMeasMode, 1) & "," & _
    '                            Right(Space(ii) & .intProbHiNo, ii) & "," & Right(Space(ii) & .intProbLoNo, ii) & "," & _
    '                            Right(Space(ii) & .intProbAGNo1, ii) & "," & Right(Space(ii) & .intProbAGNo2, ii) & "," & _
    '                            Right(Space(ii) & .intProbAGNo3, ii) & "," & Right(Space(ii) & .intProbAGNo4, ii) & "," & Right(Space(ii) & .intProbAGNo5, ii) & "," & _
    '                            Right(Space(16) & .strExternalBits, 16) & "," & Right(Space(5) & .intPauseTime, 5) & "," & _
    '                            Right(Space(1) & .intTargetValType, 1) & "," & Right(Space(3) & .intBaseResNo, 3) & "," & _
    '                            Right(Space(16) & .dblTrimTargetVal.ToString("0.00000"), 16) & ","
    '                    ' 電圧変化ｽﾛｰﾌﾟ,ｲﾆｼｬﾙﾃｽﾄ HI/LOﾘﾐｯﾄ,ﾌｧｲﾅﾙﾃｽﾄ HI/LOﾘﾐｯﾄ,ｶｯﾄ数
    '                    dData = Right(Space(1) & .intSlope, 1) & "," & _
    '                            Right(Space(7) & .dblInitTest_HighLimit.ToString("0.00"), 7) & "," & Right(Space(7) & .dblInitTest_LowLimit.ToString("0.00"), 7) & "," & _
    '                            Right(Space(7) & .dblFinalTest_HighLimit.ToString("0.00"), 7) & "," & Right(Space(7) & .dblFinalTest_LowLimit.ToString("0.00"), 7) & "," & _
    '                            Right(Space(2) & .intCutCount, 2)
    '                End If
    '                PrintLine(fn, uDATA & dData)
    '            End With
    '        End If

    '        '-----------------------------------------------------------------------
    '        '   CUT DATA
    '        '-----------------------------------------------------------------------
    '        PrintLine(fn, FILE_CONST_CUT_DATA)
    '        For i = 1 To ChipNum                            ' 抵抗数分設定する
    '            RegNo = typResistorInfoArray(i).intResNo    ' 抵抗番号取得
    '            Call GetRegCutNum(RegNo, CutNum)            ' ｶｯﾄ数取得

    '            With typResistorInfoArray(i)
    '                For ii = 1 To CutNum
    '                    If (gTkyKnd = KND_CHIP) Then
    '                        '-----------------------------------------------------------
    '                        '   CHIP時
    '                        '-----------------------------------------------------------
    '                        If (gSysPrm.stCTM.giSPECIAL = customKOAEW) Then ' KOA(EW殿)ならVer1.40形式とする
    '                            ' 抵抗番号+ｶｯﾄ番号,ﾃﾞｨﾚｲﾀｲﾑ,ﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄ(XY),ｽﾀｰﾄﾎﾟｲﾝﾄ(XY),ｶｯﾄｽﾋﾟｰﾄﾞ,Qﾚｰﾄ,ｶｯﾄｵﾌ,判定率
    '                            uDATA = Right(Space(4) & RegNo, 4) & "," & Right(Space(2) & .ArrCut(ii).intCutNo, 2) & "," & Right(Space(5) & .ArrCut(ii).intDelayTime, 5) & "," & Right(Space(8) & .ArrCut(ii).dblTeachPointX.ToString("0.0000"), 8) & "," & Right(Space(8) & .ArrCut(ii).dblTeachPointY.ToString("0.0000"), 8) & "," & Right(Space(8) & .ArrCut(ii).dblStartPointX.ToString("0.0000"), 8) & "," & Right(Space(8) & .ArrCut(ii).dblStartPointY.ToString("0.0000"), 8) & "," & Right(Space(5) & .ArrCut(ii).dblCutSpeed.ToString("0.0"), 5) & "," & Right(Space(4) & .ArrCut(ii).dblQRate.ToString("0.0"), 4) & "," & Right(Space(7) & .ArrCut(ii).dblCutOff.ToString("0.000"), 7) & "," & Right(Space(5) & .ArrCut(ii).dblJudgeLevel.ToString("0.0"), 5) & ","
    '                        Else
    '                            ' 抵抗番号+ｶｯﾄ番号,ﾃﾞｨﾚｲﾀｲﾑ,ﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄ(XY),ｽﾀｰﾄﾎﾟｲﾝﾄ(XY),ｶｯﾄｽﾋﾟｰﾄﾞ,Qﾚｰﾄ,ｶｯﾄｵﾌ,ｶｯﾄｵﾌｵﾌｾｯﾄ,判定率
    '                            uDATA = Right(Space(4) & RegNo, 4) & "," & Right(Space(2) & .ArrCut(ii).intCutNo, 2) & "," & Right(Space(5) & .ArrCut(ii).intDelayTime, 5) & "," & Right(Space(8) & .ArrCut(ii).dblTeachPointX.ToString("0.0000"), 8) & "," & Right(Space(8) & .ArrCut(ii).dblTeachPointY.ToString("0.0000"), 8) & "," & Right(Space(8) & .ArrCut(ii).dblStartPointX.ToString("0.0000"), 8) & "," & Right(Space(8) & .ArrCut(ii).dblStartPointY.ToString("0.0000"), 8) & "," & Right(Space(5) & .ArrCut(ii).dblCutSpeed.ToString("0.0"), 5) & "," & Right(Space(4) & .ArrCut(ii).dblQRate.ToString("0.0"), 4) & "," & Right(Space(7) & .ArrCut(ii).dblCutOff.ToString("0.000"), 7) & "," & Right(Space(7) & .ArrCut(ii).dblCutOffOffset.ToString("0.000"), 7) & "," & Right(Space(5) & .ArrCut(ii).dblJudgeLevel.ToString("0.0"), 5) & ","
    '                        End If

    '                    ElseIf (gTkyKnd = KND_NET) Then
    '                        '-----------------------------------------------------------
    '                        '   NET時
    '                        '-----------------------------------------------------------
    '                        ' 抵抗番号,ｶｯﾄ番号,ﾃﾞｨﾚｲﾀｲﾑ,ﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄ(XY),ｽﾀｰﾄﾎﾟｲﾝﾄ(XY),ｶｯﾄｽﾋﾟｰﾄﾞ,Qﾚｰﾄ,ｶｯﾄｵﾌ,判定率
    '                        uDATA = Right(Space(4) & RegNo, 4) & "," & Right(Space(2) & .ArrCut(ii).intCutNo, 2) & "," & Right(Space(5) & .ArrCut(ii).intDelayTime, 5) & "," & _
    '                                Right(Space(8) & .ArrCut(ii).dblTeachPointX.ToString("0.0000"), 8) & "," & Right(Space(8) & .ArrCut(ii).dblTeachPointY.ToString("0.0000"), 8) & "," & _
    '                                Right(Space(8) & .ArrCut(ii).dblStartPointX.ToString("0.0000"), 8) & "," & Right(Space(8) & .ArrCut(ii).dblStartPointY.ToString("0.0000"), 8) & "," & _
    '                                Right(Space(5) & .ArrCut(ii).dblCutSpeed.ToString("0.0"), 5) & "," & Right(Space(4) & .ArrCut(ii).dblQRate.ToString("0.0"), 4) & "," & Right(Space(7) & .ArrCut(ii).dblCutOff.ToString("0.000"), 7) & "," & _
    '                                Right(Space(5) & .ArrCut(ii).dblJudgeLevel.ToString("0.0"), 5) & ","
    '                    End If

    '                    ' ｶｯﾄ形状,ｶｯﾄ方向,ｶｯﾄ長1,R1,Lﾀｰﾝﾎﾟｲﾝﾄ,Lﾀｰﾝ後の最大ｶｯﾃｨﾝｸﾞ長,R2,ﾌｯｸﾀｰﾝ後のｶｯﾃｨﾝｸﾞ長,ｲﾝﾃﾞｯｸｽ数
    '                    ' 測定ﾓｰﾄﾞ,ｶｯﾄｽﾋﾟｰﾄﾞ2,Qｽｲｯﾁﾚｰﾄ2,斜め角度,ﾋﾟｯﾁ,ｽﾃｯﾌﾟ方向,本数,ｴｯｼﾞｾﾝｽﾎﾟｲﾝﾄ,ｴｯｼﾞｾﾝｽの判定変化率
    '                    ' ｴｯｼﾞｾﾝｽ後のｶｯﾄ長,倍率,文字列
    '                    If (gTkyKnd = KND_CHIP) Then
    '                        '-----------------------------------------------------------
    '                        '   CHIP時
    '                        '-----------------------------------------------------------
    '                        'ｴｯｼﾞｾﾝｽ後の変化率,ｴｯｼﾞｾﾝｽ後の確認回数
    '                        dData = Right(Space(1) & .ArrCut(ii).strCutType, 1) & "," & Right(Space(1) & .ArrCut(ii).intCutDir, 1) & "," & Right(Space(7) & .ArrCut(ii).dblMaxCutLength.ToString("0.0000"), 7) & "," & Right(Space(7) & .ArrCut(ii).dblR1.ToString("0.0000"), 7) & "," & Right(Space(5) & .ArrCut(ii).dblLTurnPoint.ToString("0.0"), 5) & "," & Right(Space(7) & .ArrCut(ii).dblMaxCutLengthL.ToString("0.0000"), 7) & "," & Right(Space(7) & .ArrCut(ii).dblR2.ToString("0.0000"), 7) & "," & Right(Space(7) & .ArrCut(ii).dblMaxCutLengthHook.ToString("0.0000"), 7) & "," & Right(Space(5) & .ArrCut(ii).intIndexCnt, 5) & ","
    '                        dData = dData & Right(Space(4) & .ArrCut(ii).intMeasMode.ToString("0"), 4) & "," ' 測定ﾓｰﾄﾞ(4桁出力)
    '                        dData = dData & Right(Space(5) & .ArrCut(ii).dblCutSpeed2.ToString("0.0"), 5) & "," & Right(Space(4) & .ArrCut(ii).dblQRate2.ToString("0.0"), 4) & "," & Right(Space(3) & .ArrCut(ii).intCutAngle.ToString("0"), 3) & "," & Right(Space(7) & .ArrCut(ii).dblPitch.ToString("0.0000"), 7) & "," & Right(Space(1) & .ArrCut(ii).intStepDir, 1) & "," & Right(Space(4) & .ArrCut(ii).intCutCnt, 4) & "," & Right(Space(8) & .ArrCut(ii).dblESPoint.ToString("0.0000"), 8) & "," & Right(Space(5) & .ArrCut(ii).dblESJudgeLevel.ToString("0.0"), 5) & "," & Right(Space(4) & .ArrCut(ii).dblMaxCutLengthES.ToString("0.0000"), 7) & "," & Right(Space(5) & .ArrCut(ii).dblZoom.ToString("0.00"), 5) & "," & Right(Space(18) & .ArrCut(ii).strChar, 18) & "," & Right(Space(5) & .ArrCut(ii).dblESChangeRatio.ToString("0.0"), 6) & "," & Right(Space(2) & .ArrCut(ii).intESConfirmCnt.ToString("0"), 2) & "," & Right(Space(4) & .ArrCut(ii).intRadderInterval.ToString("0"), 4)

    '                    ElseIf (gTkyKnd = KND_NET) Then
    '                        '-----------------------------------------------------------
    '                        '   NET時
    '                        '-----------------------------------------------------------
    '                        dData = Right(Space(1) & .ArrCut(ii).strCutType, 1) & "," & _
    '                                Right(Space(1) & .ArrCut(ii).intCutDir, 1) & "," & _
    '                                Right(Space(7) & .ArrCut(ii).dblMaxCutLength.ToString("0.0000"), 7) & "," & _
    '                                Right(Space(7) & .ArrCut(ii).dblR1.ToString("0.0000"), 7) & "," & _
    '                                Right(Space(5) & .ArrCut(ii).dblLTurnPoint.ToString("0.0"), 5) & "," & _
    '                                Right(Space(7) & .ArrCut(ii).dblMaxCutLengthL.ToString("0.0000"), 7) & "," & _
    '                                Right(Space(7) & .ArrCut(ii).dblR2.ToString("0.0000"), 7) & "," & _
    '                                Right(Space(7) & .ArrCut(ii).dblMaxCutLengthHook.ToString("0.0000"), 7) & "," & _
    '                                Right(Space(5) & .ArrCut(ii).intIndexCnt, 5) & "," & _
    '                                Right(Space(1) & .ArrCut(ii).intMeasMode, 1) & "," & _
    '                                Right(Space(5) & .ArrCut(ii).dblCutSpeed2.ToString("0.0"), 5) & "," & _
    '                                Right(Space(4) & .ArrCut(ii).dblQRate2.ToString("0.0"), 4) & "," & _
    '                                Right(Space(3) & .ArrCut(ii).intCutAngle.ToString("0"), 3) & "," & _
    '                                Right(Space(7) & .ArrCut(ii).dblPitch.ToString("0.0000"), 7) & "," & _
    '                                Right(Space(1) & .ArrCut(ii).intStepDir, 1) & "," & _
    '                                Right(Space(4) & .ArrCut(ii).intCutCnt, 4) & "," & _
    '                                Right(Space(8) & .ArrCut(ii).dblESPoint.ToString("0.0000"), 8) & "," & _
    '                                Right(Space(5) & .ArrCut(ii).dblESJudgeLevel.ToString("0.0"), 5) & "," & _
    '                                Right(Space(4) & .ArrCut(ii).dblMaxCutLengthES.ToString("0.0000"), 7) & "," & _
    '                                Right(Space(5) & .ArrCut(ii).dblZoom.ToString("0.00"), 5) & "," & _
    '                                Right(Space(18) & .ArrCut(ii).strChar, 18)
    '                    End If

    '                    PrintLine(fn, uDATA & dData)
    '                Next ii
    '            End With
    '        Next i

    '        ' NGマーク用(1000番)
    '        If typPlateInfo.intNGMark = 1 Then
    '            RegNo = typResistorInfoArray(1000).intResNo ' 抵抗番号取得
    '            Call GetRegCutNum(RegNo, CutNum)            ' ｶｯﾄ数取得
    '            With typResistorInfoArray(1000)
    '                For ii = 1 To CutNum
    '                    If (gTkyKnd = KND_CHIP) Then
    '                        '-----------------------------------------------------------
    '                        '   CHIP時
    '                        '-----------------------------------------------------------
    '                        ' 抵抗番号,ｶｯﾄ番号,ﾃﾞｨﾚｲﾀｲﾑ,ﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄ(XY),ｽﾀｰﾄﾎﾟｲﾝﾄ(XY),ｶｯﾄｽﾋﾟｰﾄﾞ,Qﾚｰﾄ+ｶｯﾄｵﾌ,ｶｯﾄｵﾌｵﾌｾｯﾄ,判定率
    '                        uDATA = Right(Space(4) & RegNo, 4) & "," & Right(Space(2) & .ArrCut(ii).intCutNo, 2) & "," & Right(Space(5) & .ArrCut(ii).intDelayTime, 5) & "," & Right(Space(8) & .ArrCut(ii).dblTeachPointX.ToString("0.0000"), 8) & "," & Right(Space(8) & .ArrCut(ii).dblTeachPointY.ToString("0.0000"), 8) & "," & Right(Space(8) & .ArrCut(ii).dblStartPointX.ToString("0.0000"), 8) & "," & Right(Space(8) & .ArrCut(ii).dblStartPointY.ToString("0.0000"), 8) & "," & Right(Space(5) & .ArrCut(ii).dblCutSpeed.ToString("0.0"), 5) & "," & Right(Space(4) & .ArrCut(ii).dblQRate.ToString("0.0"), 4) & "," & Right(Space(7) & .ArrCut(ii).dblCutOff.ToString("0.000"), 7) & "," & Right(Space(7) & .ArrCut(ii).dblCutOffOffset.ToString("0.000"), 7) & "," & Right(Space(5) & .ArrCut(ii).dblJudgeLevel.ToString("0.0"), 5) & ","

    '                    ElseIf (gTkyKnd = KND_NET) Then
    '                        '-----------------------------------------------------------
    '                        '   NET時
    '                        '-----------------------------------------------------------
    '                        '抵抗番号,ｶｯﾄ番号,ﾃﾞｨﾚｲﾀｲﾑ,ﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄ(XY),ｽﾀｰﾄﾎﾟｲﾝﾄ(XY),ｶｯﾄｽﾋﾟｰﾄﾞ,Qﾚｰﾄ,ｶｯﾄｵﾌ,判定率
    '                        uDATA = Right(Space(4) & RegNo, 4) & "," & Right(Space(2) & .ArrCut(ii).intCutNo, 2) & "," & Right(Space(5) & .ArrCut(ii).intDelayTime, 5) & "," & _
    '                                Right(Space(8) & .ArrCut(ii).dblTeachPointX.ToString("0.0000"), 8) & "," & Right(Space(8) & .ArrCut(ii).dblTeachPointY.ToString("0.0000"), 8) & "," & _
    '                                Right(Space(8) & .ArrCut(ii).dblStartPointX.ToString("0.0000"), 8) & "," & Right(Space(8) & .ArrCut(ii).dblStartPointY.ToString("0.0000"), 8) & "," & _
    '                                Right(Space(5) & .ArrCut(ii).dblCutSpeed.ToString("0.0"), 5) & "," & Right(Space(4) & .ArrCut(ii).dblQRate.ToString("0.0"), 4) & "," & Right(Space(7) & .ArrCut(ii).dblCutOff.ToString("0.000"), 7) & "," & _
    '                                Right(Space(5) & .ArrCut(ii).dblJudgeLevel.ToString("0.0"), 5) & ","
    '                    End If

    '                    ' ｶｯﾄ形状,ｶｯﾄ方向,ｶｯﾄ長1,R1,Lﾀｰﾝﾎﾟｲﾝﾄ,Lﾀｰﾝ後の最大ｶｯﾃｨﾝｸﾞ長,R2,ﾌｯｸﾀｰﾝ後のｶｯﾃｨﾝｸﾞ長,ｲﾝﾃﾞｯｸｽ数,測定ﾓｰﾄﾞ
    '                    ' ｶｯﾄｽﾋﾟｰﾄﾞ2,Qｽｲｯﾁﾚｰﾄ2,斜め角度,ﾋﾟｯﾁ,ｽﾃｯﾌﾟ方向,本数,ｴｯｼﾞｾﾝｽﾎﾟｲﾝﾄ,ｴｯｼﾞｾﾝｽの判定変化率,ｴｯｼﾞｾﾝｽ後のｶｯﾄ長,倍率,文字列
    '                    If (gTkyKnd = KND_CHIP) Then
    '                        '-----------------------------------------------------------
    '                        '   CHIP時
    '                        '-----------------------------------------------------------
    '                        'ｴｯｼﾞｾﾝｽ後の変化率,ｴｯｼﾞｾﾝｽ後の確認回数
    '                        dData = Right(Space(1) & .ArrCut(ii).strCutType, 1) & "," & Right(Space(1) & .ArrCut(ii).intCutDir, 1) & "," & Right(Space(7) & .ArrCut(ii).dblMaxCutLength.ToString("0.0000"), 7) & "," & Right(Space(7) & .ArrCut(ii).dblR1.ToString("0.0000"), 7) & "," & Right(Space(5) & .ArrCut(ii).dblLTurnPoint.ToString("0.0"), 5) & "," & Right(Space(7) & .ArrCut(ii).dblMaxCutLengthL.ToString("0.0000"), 7) & "," & Right(Space(7) & .ArrCut(ii).dblR2.ToString("0.0000"), 7) & "," & Right(Space(7) & .ArrCut(ii).dblMaxCutLengthHook.ToString("0.0000"), 7) & "," & Right(Space(5) & .ArrCut(ii).intIndexCnt, 5) & ","
    '                        dData = dData & Right(Space(4) & .ArrCut(ii).intMeasMode.ToString("0"), 4) & "," ' ###177 測定ﾓｰﾄﾞ(4桁出力)
    '                        dData = dData & Right(Space(5) & .ArrCut(ii).dblCutSpeed2.ToString("0.0"), 5) & "," & _
    '                                Right(Space(4) & .ArrCut(ii).dblQRate2.ToString("0.0"), 4) & "," & _
    '                                Right(Space(3) & .ArrCut(ii).intCutAngle.ToString("0"), 3) & "," & _
    '                                Right(Space(7) & .ArrCut(ii).dblPitch.ToString("0.0000"), 7) & "," & _
    '                                Right(Space(1) & .ArrCut(ii).intStepDir, 1) & "," & _
    '                                Right(Space(4) & .ArrCut(ii).intCutCnt, 4) & "," & _
    '                                Right(Space(8) & .ArrCut(ii).dblESPoint.ToString("0.0000"), 8) & "," & _
    '                                Right(Space(5) & .ArrCut(ii).dblESJudgeLevel.ToString("0.0"), 5) & "," & _
    '                                Right(Space(4) & .ArrCut(ii).dblMaxCutLengthES.ToString("0.0000"), 7) & "," & _
    '                                Right(Space(5) & .ArrCut(ii).dblZoom.ToString("0.00"), 5) & "," & _
    '                                Right(Space(18) & .ArrCut(ii).strChar, 18) & "," & _
    '                                Right(Space(5) & .ArrCut(ii).dblESChangeRatio.ToString("0.00"), 6) & "," & _
    '                                Right(Space(2) & .ArrCut(ii).intESConfirmCnt.ToString("0"), 2) & "," & _
    '                                Right(Space(4) & .ArrCut(ii).intRadderInterval.ToString("0"), 4)

    '                    ElseIf (gTkyKnd = KND_NET) Then
    '                        '-----------------------------------------------------------
    '                        '   NET時
    '                        '-----------------------------------------------------------
    '                        dData = Right(Space(1) & .ArrCut(ii).strCutType, 1) & "," & _
    '                                Right(Space(1) & .ArrCut(ii).intCutDir, 1) & "," & _
    '                                Right(Space(7) & .ArrCut(ii).dblMaxCutLength.ToString("0.0000"), 7) & "," & _
    '                                Right(Space(7) & .ArrCut(ii).dblR1.ToString("0.0000"), 7) & "," & _
    '                                Right(Space(5) & .ArrCut(ii).dblLTurnPoint.ToString("0.0"), 5) & "," & _
    '                                Right(Space(7) & .ArrCut(ii).dblMaxCutLengthL.ToString("0.0000"), 7) & "," & _
    '                                Right(Space(7) & .ArrCut(ii).dblR2.ToString("0.0000"), 7) & "," & _
    '                                Right(Space(7) & .ArrCut(ii).dblMaxCutLengthHook.ToString("0.0000"), 7) & "," & _
    '                                Right(Space(5) & .ArrCut(ii).intIndexCnt, 5) & "," & _
    '                                Right(Space(1) & .ArrCut(ii).intMeasMode, 1) & "," & _
    '                                Right(Space(5) & .ArrCut(ii).dblCutSpeed2.ToString("0.0"), 5) & "," & _
    '                                Right(Space(4) & .ArrCut(ii).dblQRate2.ToString("0.0"), 4) & "," & _
    '                                Right(Space(3) & .ArrCut(ii).intCutAngle.ToString("0"), 3) & "," & _
    '                                Right(Space(7) & .ArrCut(ii).dblPitch.ToString("0.0000"), 7) & "," & _
    '                                Right(Space(1) & .ArrCut(ii).intStepDir, 1) & "," & _
    '                                Right(Space(4) & .ArrCut(ii).intCutCnt, 4) & "," & _
    '                                Right(Space(8) & .ArrCut(ii).dblESPoint.ToString("0.0000"), 8) & "," & _
    '                                Right(Space(5) & .ArrCut(ii).dblESJudgeLevel.ToString("0.0"), 5) & "," & _
    '                                Right(Space(4) & .ArrCut(ii).dblMaxCutLengthES.ToString("0.0000"), 7) & "," & _
    '                                Right(Space(5) & .ArrCut(ii).dblZoom.ToString("0.00"), 5) & "," & _
    '                                Right(Space(18) & .ArrCut(ii).strChar, 18)
    '                    End If
    '                    PrintLine(fn, uDATA & dData)
    '                Next ii
    '            End With
    '        End If

    '        FileClose(fn)
    '        Exit Function

    '        ' トラップエラー発生時
    '    Catch ex As Exception
    '        strMSG = "File.FileSaveExe() TRAP ERROR = " + ex.Message
    '        MsgBox(strMSG)
    '        Return (1)                                      ' Return値 = 例外エラー
    '    End Try

    'End Function

#End Region

#Region "LOAD・SAVEの共通化により未使用"
#If False Then 'V5.0.0.8①
#Region "ﾁｪｯｸ用ﾌｧｲﾙﾊﾞｰｼﾞｮﾝ設定【CHIP,NET用】"
    '''=========================================================================
    '''<summary>ﾁｪｯｸ用ﾌｧｲﾙﾊﾞｰｼﾞｮﾝ設定【CHIP,NET用】</summary>
    '''=========================================================================
    Public Sub Init_FileVer_Sub()

        ' ﾁｪｯｸ用ﾌｧｲﾙﾊﾞｰｼﾞｮﾝ設定
        If (gTkyKnd = KND_CHIP) Then
            NewFileVer = 6                                      ' ファイルバージョン名
            FILETYPE01 = "TKYCHIP_SL432HW_Ver1.00"
            FILETYPE02 = "TKYCHIP_SL432HW_Ver1.10"
            FILETYPE03 = "TKYCHIP_SL432HW_Ver1.20"
            FILETYPE04 = "TKYCHIP_SL432HW_Ver1.30"
            FILETYPE05 = "TKYCHIP_SL432HW_Ver1.40"
            FILETYPE06 = "TKYCHIP_SL432HW_Ver1.50"
            FILETYPE07_02 = "TKYCHIP_SL432HW_Ver7.0.0.2"        'V1.14.0.0⑥
            FILETYPE10 = "TKYCHIP_SL432HW_Ver10.00"             ' 統合版
            FILETYPE10_01 = "TKYCHIP_SL432HW_Ver10.01"          ' 統合版
            FILETYPE10_02 = "TKYCHIP_SL432HW_Ver10.02"          ' 統合版
            FILETYPE10_03 = "TKYCHIP_SL432HW_Ver10.03"          ' 統合版 V1.13.0.0②
            FILETYPE10_04 = "TKYCHIP_SL432HW_Ver10.04"          ' 統合版 V1.14.0.0①
            FILETYPE10_05 = "TKYCHIP_SL432HW_Ver10.05"          ' 統合版 V1.16.0.0①
            FILETYPE10_06 = "TKYCHIP_SL432HW_Ver10.06"          ' 統合版 V1.18.0.0④
            FILETYPE10_07 = "TKYCHIP_SL432HW_Ver10.07"          ' 統合版 V1.18.0.0④
            FILETYPE10_072 = "TKYCHIP_SL432HW_Ver10.072"        ' 統合版 V2.0.0.0_24
            FILETYPE10_073 = "TKYCHIP_SL432HW_Ver10.073"        ' 統合版 V2.0.0.0_24
            FILETYPE10_08 = "TKYCHIP_SL432HW_Ver10.08"          ' 統合版(ノリタケ殿特注 未サポート)
            FILETYPE10_09 = "TKYCHIP_SL432HW_Ver10.09"          ' 統合版 V1.23.0.0②
            FILETYPE10_10 = "TKYCHIP_SL432HW_Ver10.10"          ' 統合版 V4.0.0.0④
            FILETYPE10_11 = "TKYCHIP_SL432HW_Ver10.11"          ' 統合版 V4.11.0.0①
            FILETYPE_CUR = "TKYCHIP_SL432HW_Ver10.11"           ' 統合版(現在版名) V4.11.0.0①

        ElseIf (gTkyKnd = KND_NET) Then
            NewFileVer = 3                                      ' ファイルバージョン名
            FILETYPE01 = "TKYNET_SL432HW_Ver1.00"
            FILETYPE02 = "TKYNET_SL432HW_Ver1.01"
            FILETYPE03 = "TKYNET_SL432HW_Ver1.02"
            FILETYPE04 = "---"
            FILETYPE05 = "---"
            FILETYPE06 = "---"
            FILETYPE07_02 = "TKYNET_SL432HW_Ver7.0.0.2"         'V1.14.0.0⑥
            FILETYPE10 = "TKYNET_SL432HW_Ver10.00"              ' 統合版
            FILETYPE10_01 = "TKYNET_SL432HW_Ver10.01"           ' 統合版
            FILETYPE10_02 = "TKYNET_SL432HW_Ver10.02"           ' 統合版
            FILETYPE10_03 = "TKYNET_SL432HW_Ver10.03"           ' 統合版 V1.13.0.0②
            FILETYPE10_04 = "TKYNET_SL432HW_Ver10.04"           ' 統合版 V1.14.0.0①
            FILETYPE10_05 = "TKYNET_SL432HW_Ver10.05"           ' 統合版 V1.16.0.0①
            FILETYPE10_06 = "TKYNET_SL432HW_Ver10.06"           ' 統合版 V1.18.0.0④
            FILETYPE10_07 = "TKYNET_SL432HW_Ver10.07"           ' 統合版 V1.18.0.0④
            FILETYPE10_072 = "TKYNET_SL432HW_Ver10.072"         ' 統合版 V2.0.0.0_24
            FILETYPE10_073 = "TKYNET_SL432HW_Ver10.073"         ' 統合版 V2.0.0.0_24
            FILETYPE10_08 = "TKYNET_SL432HW_Ver10.08"           ' 統合版(ノリタケ殿特注 未サポート)
            FILETYPE10_09 = "TKYNET_SL432HW_Ver10.09"           ' 統合版 V1.23.0.0②
            FILETYPE10_10 = "TKYNET_SL432HW_Ver10.10"           ' 統合版 V4.0.0.0④
            FILETYPE10_11 = "TKYNET_SL432HW_Ver10.11"           ' 統合版 V4.11.0.0①
            FILETYPE_CUR = "TKYNET_SL432HW_Ver10.11"            ' 統合版(現在版名) V4.11.0.0①
        End If

    End Sub

#End Region
#End If
#End Region 'V5.0.0.8①

#Region "ﾛｰﾄﾞしたﾌﾟﾚｰﾄﾃﾞｰﾀをｸﾞﾛｰﾊﾞﾙ変数へ格納する【CHIP/NET共通】"
    '''=========================================================================
    '''<summary>ﾛｰﾄﾞしたﾌﾟﾚｰﾄﾃﾞｰﾀをｸﾞﾛｰﾊﾞﾙ変数へ格納する【CHIP/NET共通】</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub SetFileLoadPlateData(ByVal pData As List(Of String))    'V5.0.0.9②
        'Private Sub SetFileLoadPlateData()
        On Error Resume Next
        Dim i As Short

        With typPlateInfo
            '<PLATE DATA 1>
            .strDataName = pData(i) : i = i + 1
            If (gTkyKnd = KND_NET) Then
                .intMeasType = pData(i) : i = i + 1     ' ﾄﾘﾑﾓｰﾄﾞ
            End If
            .intDirStepRepeat = CShort(pData(i)) : i = i + 1
            .intBlockCntXDir = CShort(Left(pData(i), InStr(pData(i), ",") - 1))
            .intBlockCntYDir = CShort(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
            '----- V6.1.4.0⑫↓ -----
            .intBlkCntInStgGrpX = .intBlockCntXDir
            .intBlkCntInStgGrpY = .intBlockCntYDir

            '----- V6.1.4.0⑫↑ -----
            ' BPｵﾌｾｯﾄの入力ﾁｪｯｸ
            If gSysPrm.stCTM.giBPOffsetInput = 0 Then
                ' 入力ありの場合
                .dblTableOffsetXDir = CDbl(Left(pData(i), InStr(pData(i), ",") - 1))
                .dblTableOffsetYDir = CDbl(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
                .dblBpOffSetXDir = CDbl(Left(pData(i), InStr(pData(i), ",") - 1))
                .dblBpOffSetYDir = CDbl(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
            Else
                ' 入力なしの場合、ﾃｰﾌﾞﾙｵﾌｾｯﾄ値の逆数をｾｯﾄ
                .dblTableOffsetXDir = CDbl(Left(pData(i), InStr(pData(i), ",") - 1))
                .dblTableOffsetYDir = CDbl(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
                .dblBpOffSetXDir = CDbl(Left(pData(i), InStr(pData(i), ",") - 1))
                .dblBpOffSetYDir = CDbl(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
                ' BPｵﾌｾｯﾄに逆数ｾｯﾄ
                .dblBpOffSetXDir = -(.dblTableOffsetXDir)
                .dblBpOffSetYDir = -(.dblTableOffsetYDir)
            End If

            .dblAdjOffSetXDir = CDbl(Left(pData(i), InStr(pData(i), ",") - 1))
            .dblAdjOffSetYDir = CDbl(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
            .intNGMark = CShort(pData(i)) : i = i + 1
            .intNGMark = 0  'V4.7.3.1②ＮＧマークはデータ構成が異なるので無しにする。 V6.1.3.0①
            .intDelayTrim = CShort(pData(i)) : i = i + 1
            .intNgJudgeUnit = CShort(pData(i)) : i = i + 1
            .intNgJudgeLevel = CShort(pData(i)) : i = i + 1
            .dblZOffSet = CDbl(pData(i)) : i = i + 1
            .dblZStepUpDist = CDbl(pData(i)) : i = i + 1
            .dblZWaitOffset = CDbl(pData(i)) : i = i + 1
            '----- V1.23.0.0⑧↓ -----
            ' Z ON/OFF位置を再設定する
            .dblZOffSet = 5.0                                               'ZON位置
            .dblZStepUpDist = 3.0                                           'ﾌﾟﾛｰﾌﾞｽﾃｯﾌﾟ上昇距離
            .dblZWaitOffset = 1.0                                           'ZOFF位置
            '----- V1.23.0.0⑧↑ -----

            '<PLATE DATA 3>
            If (gTkyKnd = KND_CHIP) Then
                .intResistDir = CShort(pData(i)) : i = i + 1
            ElseIf (gTkyKnd = KND_NET) Then
                .intPlateCntXDir = Left(pData(i), InStr(pData(i), ",") - 1)                 ' ﾌﾟﾚｰﾄ数X 
                .intPlateCntYDir = Mid(pData(i), InStr(pData(i), ",") + 1) : i = i + 1      ' ﾌﾟﾚｰﾄ数Y 
                .dblCircuitSizeXDir = Left(pData(i), InStr(pData(i), ",") - 1)              ' ﾌﾟﾚｰﾄ間隔X 
                .dblCircuitSizeYDir = Mid(pData(i), InStr(pData(i), ",") + 1) : i = i + 1   ' ﾌﾟﾚｰﾄ間隔Y 
                .intCircuitCntInBlock = pData(i) : i = i + 1                                ' 1ﾌﾞﾛｯｸ内ｻｰｷｯﾄ数 
                .dblCircuitSizeXDir = Left(pData(i), InStr(pData(i), ",") - 1)              ' ｻｰｷｯﾄｻｲｽﾞX 
                .dblCircuitSizeYDir = Mid(pData(i), InStr(pData(i), ",") + 1) : i = i + 1   ' ｻｰｷｯﾄｻｲｽﾞY 
                .dblChipSizeXDir = .dblCircuitSizeXDir                                      ' V1.14.0.0⑥
                .dblChipSizeYDir = .dblCircuitSizeYDir                                      ' V1.14.0.0⑥
            End If
            .intResistCntInGroup = CShort(pData(i)) : i = i + 1         ' 1グループ内抵抗数
            '----- V1.14.0.0⑥↓ -----
            If (gTkyKnd = KND_CHIP) Then
                .intResistCntInBlock = .intResistCntInGroup             ' 1ブロック内抵抗数
                gRegistorCnt = .intResistCntInGroup
            End If
            '----- V1.14.0.0⑥↑ -----

            If (gTkyKnd = KND_CHIP) Then
                .intGroupCntInBlockXBp = CShort(Left(pData(i), InStr(pData(i), ",") - 1))
                .intGroupCntInBlockYStage = CShort(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
                .dblBpGrpItv = CDbl(Left(pData(i), InStr(pData(i), ",") - 1))
                .dblStgGrpItvY = CDbl(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
                '.dblGroupItvXDir = CDbl(Left(pData(i), InStr(pData(i), ",") - 1))
                '.dblGroupItvYDir = CDbl(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
                .dblChipSizeXDir = CDbl(Left(pData(i), InStr(pData(i), ",") - 1))
                .dblChipSizeYDir = CDbl(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
                .dblStepOffsetXDir = CDbl(Left(pData(i), InStr(pData(i), ",") - 1))
                .dblStepOffsetYDir = CDbl(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
            ElseIf (gTkyKnd = KND_NET) Then
                If FileIO.FileVersion >= 2 Then
                    .intGroupCntInBlockXBp = Left(pData(i), InStr(pData(i), ",") - 1)
                    .intGroupCntInBlockYStage = Mid(pData(i), InStr(pData(i), ",") + 1) : i = i + 1
                    .intGroupCntInBlockXBp = .intCircuitCntInBlock      ' V1.14.0.0⑥ BPグループ数(サーキット数)
                    .intBlkCntInStgGrpY = .intBlockCntYDir              ' V1.14.0.0⑥ Y方向ステージグループ内ブロック数
                    .intResistCntInBlock = .intGroupCntInBlockXBp       ' V1.14.0.0⑥ 1ブロック内抵抗数
                    .intResistCntInBlock = .intGroupCntInBlockXBp * .intResistCntInGroup        'V4.7.3.1② 1ブロック内抵抗数 V6.1.3.0①
                    gRegistorCnt = .intResistCntInBlock                 ' V1.14.0.0⑥
                    .dblChipSizeXDir = .dblCircuitSizeXDir / .intResistCntInGroup              'V4.7.3.1②抵抗並び方向はＸ固定で処理 V6.1.3.0①
                End If
            End If
            '----- V6.1.4.0⑫↓ -----
            '.dblBlockSizeReviseXDir = CDbl(Left(pData(i), InStr(pData(i), ",") - 1))
            '.dblBlockSizeReviseYDir = CDbl(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
            .dblBlockSizeXDir = CDbl(Left(pData(i), InStr(pData(i), ",") - 1))              'ブロックサイズＸ
            .dblBlockSizeYDir = CDbl(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1   'ブロックサイズＹ
            .dblBlockSizeReviseXDir = .dblBlockSizeXDir
            .dblBlockSizeReviseYDir = .dblBlockSizeYDir
            '----- V6.1.4.0⑫↑ -----
            If (gTkyKnd = KND_CHIP) Then
                .dblBlockItvXDir = CDbl(Left(pData(i), InStr(pData(i), ",") - 1))
                .dblBlockItvYDir = CDbl(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
                .intContHiNgBlockCnt = CShort(pData(i)) : i = i + 1
            End If

            '<PLATE DATA 2>
            .intReviseMode = CShort(pData(i)) : i = i + 1
            .intManualReviseType = CShort(pData(i)) : i = i + 1
            .dblReviseCordnt1XDir = CDbl(Left(pData(i), InStr(pData(i), ",") - 1))
            .dblReviseCordnt1YDir = CDbl(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
            .dblReviseCordnt2XDir = CDbl(Left(pData(i), InStr(pData(i), ",") - 1))
            .dblReviseCordnt2YDir = CDbl(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
            .dblReviseOffsetXDir = CDbl(Left(pData(i), InStr(pData(i), ",") - 1))
            .dblReviseOffsetYDir = CDbl(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
            .intRecogDispMode = CShort(pData(i)) : i = i + 1
            .dblPixelValXDir = CDbl(Left(pData(i), InStr(pData(i), ",") - 1))
            .dblPixelValYDir = CDbl(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
            .intRevisePtnNo1 = CShort(Left(pData(i), InStr(pData(i), ",") - 1))
            .intRevisePtnNo2 = CShort(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
            If FileIO.FileVersion >= 10 Then
                .intRevisePtnNo1GroupNo = CShort(Left(pData(i), InStr(pData(i), ",") - 1))
                .intRevisePtnNo2GroupNo = CShort(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
            End If

            .dblRotateXDir = CDbl(Left(pData(i), InStr(pData(i), ",") - 1))
            .dblRotateYDir = CDbl(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1

            If (gTkyKnd = KND_CHIP) Then
                If (FileIO.FileVersion >= 4) Or (FILETYPE_K = FILETYPE04_K) Then ' V1.23.0.0⑧
                    .dblThetaAxis = CDbl(pData(i)) : i = i + 1
                End If
                If FileIO.FileVersion >= 6 Then
                    .dblTThetaOffset = CDbl(pData(i)) : i = i + 1
                    .dblTThetaBase1XDir = CDbl(pData(i)) : i = i + 1
                    .dblTThetaBase1YDir = CDbl(pData(i)) : i = i + 1
                    .dblTThetaBase2XDir = CDbl(pData(i)) : i = i + 1
                    .dblTThetaBase2YDir = CDbl(pData(i)) : i = i + 1
                End If
            ElseIf (gTkyKnd = KND_NET) Then
                If FileIO.FileVersion >= 3 Then
                    .dblThetaAxis = pData(i) : i = i + 1
                End If
            End If

            '<PLATE DATA 4>
            .dblCaribBaseCordnt1XDir = CDbl(Left(pData(i), InStr(pData(i), ",") - 1))
            .dblCaribBaseCordnt1YDir = CDbl(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
            .dblCaribBaseCordnt2XDir = CDbl(Left(pData(i), InStr(pData(i), ",") - 1))
            .dblCaribBaseCordnt2YDir = CDbl(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
            .dblCaribTableOffsetXDir = CDbl(Left(pData(i), InStr(pData(i), ",") - 1))
            .dblCaribTableOffsetYDir = CDbl(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
            .intCaribPtnNo1 = CShort(Left(pData(i), InStr(pData(i), ",") - 1))
            .intCaribPtnNo2 = CShort(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
            If FileIO.FileVersion >= 10 Then
                .intCaribPtnNo1GroupNo = CShort(Left(pData(i), InStr(pData(i), ",") - 1))
                .intCaribPtnNo2GroupNo = CShort(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
            End If
            .dblCaribCutLength = CDbl(pData(i)) : i = i + 1
            .dblCaribCutSpeed = CDbl(pData(i)) : i = i + 1
            .dblCaribCutQRate = CDbl(pData(i)) : i = i + 1
            .dblCutPosiReviseOffsetXDir = CDbl(Left(pData(i), InStr(pData(i), ",") - 1))
            .dblCutPosiReviseOffsetYDir = CDbl(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
            .intCutPosiRevisePtnNo = CShort(pData(i)) : i = i + 1
            .dblCutPosiReviseCutLength = CDbl(pData(i)) : i = i + 1
            .dblCutPosiReviseCutSpeed = CDbl(pData(i)) : i = i + 1
            .dblCutPosiReviseCutQRate = CDbl(pData(i)) : i = i + 1
            If FileIO.FileVersion < 10 Then
                .intRevisePtnNo1GroupNo = CShort(pData(i))
                .intRevisePtnNo2GroupNo = CShort(pData(i))
                '----- V6.1.4.0⑫↓ -----
                .intCaribPtnNo1GroupNo = CShort(pData(i))                       ' キャリブレーショングループ番号
                .intCaribPtnNo2GroupNo = CShort(pData(i))
                '----- V6.1.4.0⑫↑ -----
            End If
            .intCutPosiReviseGroupNo = CShort(pData(i)) : i = i + 1

            '<PLATE DATA 5>
            If (gTkyKnd = KND_CHIP) Then
                .intMaxTrimNgCount = CShort(pData(i)) : i = i + 1
                .intMaxBreakDischargeCount = CShort(pData(i)) : i = i + 1
                .intTrimNgCount = CShort(pData(i)) : i = i + 1
            End If
            .intRetryProbeCount = CShort(pData(i)) : i = i + 1
            .dblRetryProbeDistance = CDbl(pData(i)) : i = i + 1

            If (gTkyKnd = KND_CHIP) Then
                '----- V1.23.0.0⑧↓ -----
                ' CHIP(SL436K)
                If (FILETYPE_K = FILETYPE04_K) Then
                    ' 自動ﾚｰｻﾞﾊﾟﾜｰ調整項目
                    .intPowerAdjustMode = CShort(pData(i)) : i = i + 1          ' ﾊﾟﾜｰ調整ﾓｰﾄﾞ
                    .dblPowerAdjustTarget = CDbl(pData(i)) : i = i + 1          ' 調整目標ﾊﾟﾜｰ
                    .dblPowerAdjustQRate = CDbl(pData(i)) : i = i + 1           ' ﾊﾟﾜｰ調整Qﾚｰﾄ
                    .dblPowerAdjustToleLevel = CDbl(pData(i)) : i = i + 1       ' ﾊﾟﾜｰ調整許容範囲

                    .intInitialOkTestDo = CShort(pData(i)) : i = i + 1          ' ｲﾆｼｬﾙOKﾃｽﾄ
                    .intWorkSetByLoader = CShort(pData(i)) : i = i + 1          ' 基板品種
                    .intOpenCheck = CShort(pData(i)) : i = i + 1                ' 4端子ｵｰﾌﾟﾝﾁｪｯｸ

                    Return
                End If
                '----- V1.23.0.0⑧↑ -----

                If FileIO.FileVersion >= 2 Then
                    .intLedCtrl = CShort(pData(i)) : i = i + 1  ' LED制御
                End If

                If FileIO.FileVersion >= 5 Then
                    If (FileIO.FileVersion >= 6) Then
                        ' 自動ﾚｰｻﾞﾊﾟﾜｰ調整項目
                        .intPowerAdjustMode = CShort(pData(i)) : i = i + 1      ' ﾊﾟﾜｰ調整ﾓｰﾄﾞ
                        .dblPowerAdjustTarget = CDbl(pData(i)) : i = i + 1      ' 調整目標ﾊﾟﾜｰ
                        .dblPowerAdjustQRate = CDbl(pData(i)) : i = i + 1       ' ﾊﾟﾜｰ調整Qﾚｰﾄ
                        .dblPowerAdjustToleLevel = CDbl(pData(i)) : i = i + 1   ' ﾊﾟﾜｰ調整許容範囲
                    End If

                    ' GP-IB制御
                    If gSysPrm.stCTM.giGP_IB_flg = 0 Then
                        .intGpibCtrl = 0 : i = i + 1                        ' GP-IB制御
                    Else
                        .intGpibCtrl = CShort(pData(i)) : i = i + 1         ' GP-IB制御
                    End If

                    If gSysPrm.stCTM.giGP_IB_flg = 2 Then
                        .intGpibDefDelimiter = 0 : i = i + 1                ' 初期設定(ﾃﾞﾘﾐﾀ) (固定)
                        .intGpibDefTimiout = 100 : i = i + 1                ' 初期設定(ﾀｲﾑｱｳﾄ) (固定)
                        .intGpibDefAdder = CShort(pData(i)) : i = i + 1     ' 初期設定(機器ｱﾄﾞﾚｽ)
                        .strGpibInitCmnd1 = "" : i = i + 1                  ' 初期化ｺﾏﾝﾄﾞ(ここでは何もしない)
                        .strGpibInitCmnd2 = "" : i = i + 1                  ' 初期化ｺﾏﾝﾄﾞ(ここでは何もしない)
                        .strGpibTriggerCmnd = "E" : i = i + 1               ' ﾄﾘｶﾞｺﾏﾝﾄﾞ (固定)
                        .intGpibMeasSpeed = CShort(pData(i)) : i = i + 1    ' 測定速度
                        .intGpibMeasMode = CShort(pData(i)) : i = i + 1     ' 測定モード
                        Call MakeGPIBinit()                                 ' 初期化ｺﾏﾝﾄﾞを作成
                    Else
                        .intGpibDefDelimiter = CShort(pData(i)) : i = i + 1 ' 初期設定(ﾃﾞﾘﾐﾀ)
                        .intGpibDefTimiout = CShort(pData(i)) : i = i + 1   ' 初期設定(ﾀｲﾑｱｳﾄ)
                        .intGpibDefAdder = CShort(pData(i)) : i = i + 1     ' 初期設定(機器ｱﾄﾞﾚｽ)
                        .strGpibInitCmnd1 = pData(i) : i = i + 1            ' 初期化ｺﾏﾝﾄﾞ
                        .strGpibInitCmnd2 = pData(i) : i = i + 1            ' 初期化ｺﾏﾝﾄﾞ
                        .strGpibTriggerCmnd = pData(i) : i = i + 1          ' ﾄﾘｶﾞｺﾏﾝﾄﾞ
                        .intGpibMeasSpeed = 1 : i = i + 1                   ' 測定速度(1:高速)  (固定)
                        .intGpibMeasMode = 0 : i = i + 1                    ' 測定モード(0:絶対)  (固定)
                    End If
                End If
            End If
        End With
    End Sub

#End Region

#Region "ﾛｰﾄﾞﾃﾞｰﾀ格納ﾊﾞｯﾌｧのｸﾘｱ【CHIP/NET共通】"
    ' '''=========================================================================      'V5.0.0.9②
    ' '''<summary>ﾛｰﾄﾞﾃﾞｰﾀ格納ﾊﾞｯﾌｧのｸﾘｱ【CHIP/NET共通】</summary>
    ' '''<remarks></remarks>
    ' '''=========================================================================
    'Private Sub ClearBuff()

    '    Dim i As Short

    '    For i = 0 To 100
    '        pData(i) = ""
    '    Next i

    'End Sub

#End Region

#Region "抵抗データの取得【CHIP/NET共通】"
    '''=========================================================================
    '''<summary>抵抗データの取得【CHIP/NET共通】</summary>
    '''<param name="strDATA">(INP) ファイルパス名</param>
    '''<param name="rCnt">   (I/O)データインデックス</param>	'V4.7.3.1②ADD V6.1.3.0①
    '''<returns>0=正常, 1=エラー </returns>
    '''=========================================================================
    '    Private Function GetResistorData(ByRef strDATA As String) As Short
    Private Function GetResistorData(ByRef strDATA As String, ByRef rCnt As Integer) As Short   'V4.7.3.1② ByRef rCnt As Integer追加 V6.1.3.0①

        Dim i As Short
        Dim vWork() As String
        Dim ResNo As Short                              ' 抵抗番号

        GetResistorData = 0
        i = 0
        vWork = strDATA.Split(",")
        ResNo = CShort(vWork(0))                        ' 抵抗番号取得

        'V4.7.3.1②        With typResistorInfoArray(ResNo) V6.1.3.0①
        With typResistorInfoArray(rCnt)                 'V4.7.3.1② 配列番号をResNoからrCntへ変更 V6.1.3.0① 
            .intResNo = CShort(vWork(i)) : i = i + 1            ' 抵抗番号
            If (gTkyKnd = KND_NET) Then
                .intCircuitGrp = CInt(vWork(i)) : i = i + 1     ' 所属ｻｰｷｯﾄ
            End If
            '----- V6.1.4.0⑫↓ -----
            '.intResMeasMode = CShort(vWork(i)) : i = i + 1      ' 高精度測定、判定ﾓｰﾄﾞ
            .intResMeasType = CShort(vWork(i)) : i = i + 1      ' 高精度測定、判定ﾓｰﾄﾞ
            '----- V6.1.4.0⑫↑ -----
            .intProbHiNo = CShort(vWork(i)) : i = i + 1         ' ﾌﾟﾛｰﾌﾞ番号(HI)
            .intProbLoNo = CShort(vWork(i)) : i = i + 1         ' ﾌﾟﾛｰﾌﾞ番号(LO)
            .intProbAGNo1 = CShort(vWork(i)) : i = i + 1        ' ﾌﾟﾛｰﾌﾞ番号(AG1)
            .intProbAGNo2 = CShort(vWork(i)) : i = i + 1        ' ﾌﾟﾛｰﾌﾞ番号(AG2)
            .intProbAGNo3 = CShort(vWork(i)) : i = i + 1        ' ﾌﾟﾛｰﾌﾞ番号(AG3)
            .intProbAGNo4 = CShort(vWork(i)) : i = i + 1        ' ﾌﾟﾛｰﾌﾞ番号(AG4)
            .intProbAGNo5 = CShort(vWork(i)) : i = i + 1        ' ﾌﾟﾛｰﾌﾞ番号(AG5)
            '----- V1.23.0.0⑧↓ -----
            ' CHIP(SL436K)
            If (FILETYPE_K = FILETYPE04_K) Then
                .strExternalBits = 0                            ' EXTERNAL BIT
                .intPauseTime = 0                               ' ﾎﾟｰｽﾞ
                .dblTrimTargetVal = CDbl(vWork(i)) : i = i + 1  ' ﾄﾘﾐﾝｸﾞ目標値
                .dblInitOKTest_HighLimit = CDbl(vWork(i)) : i = i + 1   'ｲﾆｼｬﾙOKﾃｽﾄ(HIﾘﾐｯﾄ)
                .dblInitOKTest_LowLimit = CDbl(vWork(i)) : i = i + 1    'ｲﾆｼｬﾙOKﾃｽﾄ(LOﾘﾐｯﾄ)
                GoTo STP_100
            Else
                .strExternalBits = CStr(vWork(i)) : i = i + 1       ' EXTERNAL BIT
                .intPauseTime = CShort(vWork(i)) : i = i + 1        ' ﾎﾟｰｽﾞ
                If (gTkyKnd = KND_NET) Then
                    .intTargetValType = CInt(vWork(i)) : i = i + 1  ' ﾄﾘﾑﾓｰﾄﾞ
                    .intBaseResNo = CInt(vWork(i)) : i = i + 1      ' ﾍﾞｰｽ抵抗
                End If
            End If
            .dblTrimTargetVal = CDbl(vWork(i)) : i = i + 1          ' ﾄﾘﾐﾝｸﾞ目標値
            '----- V1.23.0.0⑧↑ -----

            If (gTkyKnd = KND_CHIP) Then
                If FileIO.FileVersion >= 5 Then
                    .dblDeltaR = CDbl(vWork(i)) : i = i + 1     ' ΔR
                    .dblCutOffRatio = CDbl(vWork(i)) : i = i + 1 ' 切り上げ倍率
                End If
                .intSlope = 4                                   ' V6.1.4.0⑫
            ElseIf (gTkyKnd = KND_NET) Then
                .intSlope = CInt(vWork(i)) : i = i + 1          ' 電圧変化ｽﾛｰﾌﾟ
            End If
STP_100:    ' V1.23.0.0⑧
            .dblInitTest_HighLimit = CDbl(vWork(i)) : i = i + 1 ' ｲﾆｼｬﾙﾃｽﾄ(HIﾘﾐｯﾄ)
            .dblInitTest_LowLimit = CDbl(vWork(i)) : i = i + 1  ' ｲﾆｼｬﾙﾃｽﾄ(LOﾘﾐｯﾄ)
            .dblFinalTest_HighLimit = CDbl(vWork(i)) : i = i + 1 ' ﾌｧｲﾅﾙﾃｽﾄ(HIﾘﾐｯﾄ)
            .dblFinalTest_LowLimit = CDbl(vWork(i)) : i = i + 1 ' ﾌｧｲﾅﾙﾃｽﾄ(LOﾘﾐｯﾄ)
            .intCutCount = CShort(vWork(i)) : i = i + 1         ' ｶｯﾄ数
        End With

    End Function
#End Region

#Region "カットデータの取得【CHIP/NET共通】"
    '''=========================================================================
    '''<summary>カットデータの取得【CHIP/NET共通】</summary>
    '''<param name="strDATA">(INP) ファイルパス名</param>
    '''<param name="rCnt">   (I/O)データインデックス</param>	'V4.7.3.1② V6.1.3.0①
    '''<returns>0=正常, 1=エラー </returns>
    '''=========================================================================
    '    Private Function GetCutData(ByRef strDATA As String) As Short
    Private Function GetCutData(ByRef strDATA As String, ByRef rCnt As Integer) As Short    'V4.7.3.1② ByRef rCnt As Integer追加 V6.1.3.0①

        Dim i As Short
        Dim j As Integer
        Dim vWork() As String
        Dim ResNo As Short                              ' 抵抗番号
        Dim CutNum As Short                             ' ｶｯﾄ数
        Dim shWK As Short
        Dim strMSG As String                            ' V1.23.0.0⑧

        Try ' V1.23.0.0⑧

            GetCutData = 0
            i = 0
            vWork = strDATA.Split(",")

            ' CHIP時 
            If (gTkyKnd = KND_CHIP) Then
                If UBound(vWork) < 35 Then
                    For i = UBound(vWork) + 1 To 35
                        ' 足りない項目を追加
                        strDATA = strDATA & ","
                    Next
                    Erase vWork
                    vWork = strDATA.Split(",")
                    i = 0
                End If
            End If

            'V4.7.3.1②            ResNo = CShort(vWork(0))                        ' 抵抗番号取得 V6.1.3.0①
            CutNum = CShort(vWork(1))                       ' ｶｯﾄ番号取得
            'V4.7.3.1②↓ V6.1.3.0①
            If (CutNum = 1) Then                                        ' カット番号=１の場合は抵抗データインデックスをカウントアップ 
                rCnt = rCnt + 1
            End If
            ResNo = rCnt                                                               ' Rn = 抵抗データインデックス
            'V4.7.3.1②↑ V6.1.3.0①

            With typResistorInfoArray(ResNo)
                shWK = CShort(vWork(i)) : i = i + 1                         ' 抵抗番号
                .ArrCut(CutNum).intCutNo = CShort(vWork(i)) : i = i + 1     ' ｶｯﾄ番号
                .ArrCut(CutNum).intDelayTime = CShort(vWork(i)) : i = i + 1 ' ﾃﾞｨﾚｲﾀｲﾑ
                .ArrCut(CutNum).dblTeachPointX = CDbl(vWork(i)) : i = i + 1 ' ﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄX
                .ArrCut(CutNum).dblTeachPointY = CDbl(vWork(i)) : i = i + 1 ' ﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄY
                .ArrCut(CutNum).dblStartPointX = CDbl(vWork(i)) : i = i + 1 ' ｽﾀｰﾄﾎﾟｲﾝﾄX
                .ArrCut(CutNum).dblStartPointY = CDbl(vWork(i)) : i = i + 1 ' ｽﾀｰﾄﾎﾟｲﾝﾄY
                .ArrCut(CutNum).dblCutSpeed = CDbl(vWork(i)) : i = i + 1    ' ｶｯﾄｽﾋﾟｰﾄﾞ
                .ArrCut(CutNum).dblQRate = CDbl(vWork(i)) : i = i + 1       ' Qｽｲｯﾁﾚｰﾄ
                .ArrCut(CutNum).dblCutOff = CDbl(vWork(i)) : i = i + 1      ' ｶｯﾄｵﾌ値
                '----- V1.23.0.0⑧↓ -----
                ' CHIP(SL436K)
                If (FILETYPE_K = FILETYPE04_K) Then
                    i = i + 1                                               ' ﾊﾟﾙｽ幅制御
                    i = i + 1                                               ' ﾊﾟﾙｽ幅時間
                    i = i + 1                                               ' LSwパルス幅時間(外部シャッタ)

                    .ArrCut(CutNum).dblJudgeLevel = 0.0                     ' 切替ポイント (旧ﾃﾞｰﾀ判定(平均化率))
                    .ArrCut(CutNum).dblCutOffOffset = 0.0                   ' ｶｯﾄｵﾌ ｵﾌｾｯﾄ
                Else
                    .ArrCut(CutNum).dblJudgeLevel = CDbl(vWork(i)) : i = i + 1              ' 切替ポイント (旧ﾃﾞｰﾀ判定(平均化率))
                    If (gTkyKnd = KND_CHIP) Then
                        If (FileIO.FileVersion >= 6) Then
                            .ArrCut(CutNum).dblCutOffOffset = CDbl(vWork(i)) : i = i + 1    ' ｶｯﾄｵﾌ ｵﾌｾｯﾄ
                        End If
                    End If
                End If

                '.ArrCut(CutNum).dblJudgeLevel = CDbl(vWork(i)) : i = i + 1  ' 切替ポイント (旧ﾃﾞｰﾀ判定(平均化率))
                'If (gTkyKnd = KND_CHIP) Then
                '    If (SL432HW_FileVer >= 6) Then
                '        .ArrCut(CutNum).dblCutOffOffset = CDbl(vWork(i)) : i = i + 1    ' ｶｯﾄｵﾌ ｵﾌｾｯﾄ
                '    End If
                'End If
                '----- V1.23.0.0⑧↑ -----

                .ArrCut(CutNum).strCutType = CStr(vWork(i)) : i = i + 1         ' ｶｯﾄ形状
                .ArrCut(CutNum).intCutDir = CShort(vWork(i)) : i = i + 1        ' ｶｯﾄ方向
                .ArrCut(CutNum).dblMaxCutLength = CDbl(vWork(i)) : i = i + 1    ' 最大ｶｯﾃｨﾝｸﾞ長
                .ArrCut(CutNum).dblR1 = CDbl(vWork(i)) : i = i + 1              ' R1
                .ArrCut(CutNum).dblLTurnPoint = CDbl(vWork(i)) : i = i + 1      ' Lﾀｰﾝﾎﾟｲﾝﾄ
                .ArrCut(CutNum).dblMaxCutLengthL = CDbl(vWork(i)) : i = i + 1   ' Lﾀｰﾝ後の最大ｶｯﾃｨﾝｸﾞ長
                .ArrCut(CutNum).dblR2 = CDbl(vWork(i)) : i = i + 1              ' R2
                .ArrCut(CutNum).dblMaxCutLengthHook = CDbl(vWork(i)) : i = i + 1 'ﾌｯｸﾀｰﾝ後のｶｯﾃｨﾝｸﾞ長
                .ArrCut(CutNum).intIndexCnt = CShort(vWork(i)) : i = i + 1      ' ｲﾝﾃﾞｯｸｽ数
                .ArrCut(CutNum).intMeasMode = CShort(vWork(i)) : i = i + 1      ' 測定ﾓｰﾄﾞ
                .ArrCut(CutNum).dblCutSpeed2 = CDbl(vWork(i)) : i = i + 1       ' ｶｯﾄｽﾋﾟｰﾄﾞ2
                '----- V1.13.0.0②↓ -----
                If (.ArrCut(CutNum).dblCutSpeed2 = 0) Then                      ' カットスピード2にカットスピードを設定する
                    .ArrCut(CutNum).dblCutSpeed2 = .ArrCut(CutNum).dblCutSpeed
                End If
                '----- V1.13.0.0②↑ -----
                .ArrCut(CutNum).dblQRate2 = CDbl(vWork(i)) : i = i + 1          ' Qｽｲｯﾁﾚｰﾄ2
                '----- V1.13.0.0②↓ -----
                If (.ArrCut(CutNum).dblQRate2 = 0) Then                         ' Ｑスイッチレート２にＱスイッチレートを設定する
                    .ArrCut(CutNum).dblQRate2 = .ArrCut(CutNum).dblQRate
                End If
                '----- V1.13.0.0②↑ -----

                '----- V1.23.0.0⑧↓ -----
                ' CHIP(SL436K)
                If (FILETYPE_K = FILETYPE04_K) Then
                    i = i + 1                                                   'Qｽｲｯﾁﾚｰﾄ3
                    .ArrCut(CutNum).dblJudgeLevel = CDbl(vWork(i)) : i = i + 1  '切替えﾎﾟｲﾝﾄ (旧ﾃﾞｰﾀ判定(平均化率))
                    .ArrCut(CutNum).intCutAngle = 0                             ' 斜めｶｯﾄの切り出し角度
                Else
                    .ArrCut(CutNum).intCutAngle = CShort(vWork(i)) : i = i + 1  ' 斜めｶｯﾄの切り出し角度
                End If
                '.ArrCut(CutNum).intCutAngle = CShort(vWork(i)) : i = i + 1      ' 斜めｶｯﾄの切り出し角度
                '----- V1.23.0.0⑧↑ -----

                .ArrCut(CutNum).dblPitch = CDbl(vWork(i)) : i = i + 1           ' ﾋﾟｯﾁ
                .ArrCut(CutNum).intStepDir = CShort(vWork(i)) : i = i + 1       ' ｽﾃｯﾌﾟ方向
                .ArrCut(CutNum).intCutCnt = CShort(vWork(i)) : i = i + 1        ' 本数
                .ArrCut(CutNum).dblESPoint = CDbl(vWork(i)) : i = i + 1         ' ｴｯｼﾞｾﾝｽﾎﾟｲﾝﾄ
                .ArrCut(CutNum).dblESJudgeLevel = CDbl(vWork(i)) : i = i + 1    ' ｴｯｼﾞｾﾝｽの判定変化率
                .ArrCut(CutNum).dblMaxCutLengthES = CDbl(vWork(i)) : i = i + 1  ' ｴｯｼﾞｾﾝｽ後のｶｯﾄ長
                .ArrCut(CutNum).dblZoom = CDbl(vWork(i)) : i = i + 1            ' 倍率
                .ArrCut(CutNum).strChar = Trim(CStr(vWork(i))) : i = i + 1      ' 文字列

                '----- V1.23.0.0⑧↓ -----
                ' CHIP(SL436K)
                If (FILETYPE_K = FILETYPE04_K) Then

                Else
                    If (gTkyKnd = KND_CHIP) Then
                        .ArrCut(CutNum).dblESChangeRatio = IIf(Len(vWork(i)) = 0, 0, vWork(i)) : i = i + 1  'ｴｯｼﾞｾﾝｽ後の判定変化率
                        .ArrCut(CutNum).intESConfirmCnt = IIf(Len(vWork(i)) = 0, 0, vWork(i)) : i = i + 1   'ｴｯｼﾞｾﾝｽ後の確認回数
                        .ArrCut(CutNum).intRadderInterval = IIf(Len(vWork(i)) = 0, 0, vWork(i)) : i = i + 1 ' ﾗﾀﾞｰ間距離
                    End If
                End If
                '----- V1.23.0.0⑧↑ -----

                ' カット方向から斜めカットの切り出し角度を設定する
                Call GetCutAngle(.ArrCut(CutNum).strCutType, .ArrCut(CutNum).intCutDir, .ArrCut(CutNum).intCutAngle)
                ' カット方向から斜めカットの切り出し角度とLターン方向を設定する(Lカット/HOOKカット用)
                Call GetCutLTurnDir(.ArrCut(CutNum).strCutType, .ArrCut(CutNum).intCutDir, .ArrCut(CutNum).intCutAngle, .ArrCut(CutNum).intLTurnDir)
                ' ステップ方向を変換する(スキャンカット用)
                Call GetStepDir(.ArrCut(CutNum).strCutType, .ArrCut(CutNum).intStepDir, .ArrCut(CutNum).intStepDir)
                '----- V1.18.0.0⑤↓ -----
                ' STカットとLカットは斜めSTカット, 斜めLカットに変換する
                Call CnvCutType(.ArrCut(CutNum).strCutType, .ArrCut(CutNum).strCutType)
                '----- V1.18.0.0⑤↑ -----

                ' 目標パワーと許容範囲(デフォルト値を設定する) ###066
                'For j = 0 To (cCNDNUM - 1)                                          ' 加工条件番号1～n(0ｵﾘｼﾞﾝ)
                For j = 0 To (MaxCndNum - 1)                                        ' 加工条件番号1～n(0ｵﾘｼﾞﾝ) 'V5.0.0.8①
                    'V6.0.0.1⑥                    .ArrCut(CutNum).dblPowerAdjustTarget(j) = POWERADJUST_TARGET    ' 目標パワー(W)
                    'V6.0.0.1⑥                    .ArrCut(CutNum).dblPowerAdjustToleLevel(j) = POWERADJUST_LEVEL  ' 許容範囲(±W)
                    .ArrCut(CutNum).dblPowerAdjustTarget(j) = DEFAULT_ADJUST_TAERGET    ' 目標パワー(W)  'V6.0.0.1⑥
                    .ArrCut(CutNum).dblPowerAdjustToleLevel(j) = DEFAULT_ADJUST_LEVEL   ' 許容範囲(±W)   'V6.0.0.1⑥
                Next j

            End With

            Return (cFRS_NORMAL)                                        ' V1.23.0.0⑧

            ' トラップエラー発生時 V1.23.0.0⑧
        Catch ex As Exception
            strMSG = "File.GetCutData() TRAP ERROR = " + ex.Message
            Return (cFRS_NORMAL)
        End Try
    End Function
#End Region

#Region "LOAD・SAVEの共通化により未使用"
#If False Then 'V5.0.0.8①
#Region "データコンバータ処理【CHIP/NET共通】"
    '''=========================================================================
    '''<summary>データコンバータ処理【CHIP/NET共通】</summary>
    '''<param name="sp">(INP) ファイルパス名</param>
    '''<returns>0=正常, 1=エラー </returns>
    '''=========================================================================
    Public Function DatConv_CHIPNET(ByRef sp As String) As Short

        Dim strD As String
        'Dim mPath As String

        DatConv_CHIPNET = 0

        If sp = "" Then
            ' "指定されたファイルは存在しません"
            MsgBox(MSG_15, MsgBoxStyle.OkOnly, gAppName)
            DatConv_CHIPNET = 1
            Exit Function
        End If

        strD = Right(sp, 3)                                 ' 拡張子を取得する
        Select Case strD
            'Case "WTN", "wtn", "WTC", "wtc"                ' V1.23.0.0⑧
            Case "WTN", "wtn", "WTC", "wtc", "WDC", "wdc"   ' V1.23.0.0⑧
                sp = sp

                '----- V4.0.0.0⑦↓ -----
                'Case ".DC", ".dc", "DAT", "dat"
                '    ' SL436H
                '    mPath = Mid(sp, 1, Len(sp) - 3)
                '    If UCase(strD) = ".DC" Then
                '        mPath = mPath & ".WTC"
                '    ElseIf UCase(strD) = "DAT" Then
                '        mPath = mPath & "WTN"
                '    End If

                '    DatConv_CHIPNET = Form1.DCConvert.DCConvert(sp, mPath)
                '    If DatConv_CHIPNET Then
                '        Debug.Print("DCConvert error section =" & Form1.DCConvert.ErrorSection)
                '        Debug.Print("DCConvert error item No =" & Form1.DCConvert.ErrorItemNo)
                '        Debug.Print("DCConvert error msg     =" & Form1.DCConvert.ErrorMessage)
                '        Debug.Print("DCConvert error line    =" & Form1.DCConvert.ErrorLine)
                '        MsgBox(Form1.DCConvert.ErrorMessage & " Line=" & CStr(Form1.DCConvert.ErrorLine))
                '        Exit Function
                '    End If
                '    sp = mPath
                '----- V4.0.0.0⑦↑ -----
        End Select

    End Function
#End Region
#End If
#End Region 'V5.0.0.8①

#End Region

    '======================================================================
    '   TKY,CHIP,NET統合版メソッド
    '======================================================================
#Region "【TKY,CHIP,NET統合版用メソッド】"
#Region "ファイルロード処理【TKY,CHIP,NET統合版】"
    '''=========================================================================
    '''<summary>ファイルロード処理【TKY,CHIP,NET統合版】</summary>
    '''<param name="pPath">(INP) ファイル名</param>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Public Function File_Read(ByVal pPath As String) As Integer

        'Dim intFileNo As Integer                                ' ファイル番号
        'V5.0.0.8①        Dim strDAT As String                                    ' 読み込みデータバッファ
        'V5.0.0.8①        Dim intType As Integer                                  ' データ種別
        'V5.0.0.8①        Dim Err_num As Integer                                  ' エラー種別(1:対象外ファイル、)
        'V5.0.0.8①        Dim rCnt As Integer
        'Dim iFlg As Integer
        Dim r As Integer
        Dim strSECT As String = ""
        Dim strMSG As String

        Try
            ' 初期処理
            r = cFRS_NORMAL                                         ' Return値 = 正常
            'V5.0.0.8①            File_Read = 0
            'V5.0.0.8①            Err_num = 0
            'iFlg = 0
            'V5.0.0.8①            Call Init_FileVer_Sub()                                 ' ﾁｪｯｸ用ﾌｧｲﾙﾊﾞｰｼﾞｮﾝ設定
            Call Init_AllTrmmingData()                              ' グローバルデータ初期化(Plate/Circuit/Resistor/Cut)

            'V5.0.0.8①                  ↓
            r = DirectCast(FileIO.File_Read(pPath, strSECT), Integer)
            If (r <> cFRS_NORMAL) Then GoTo STP_ERR
            SetTemporaryData(True)     ' 値を退避する
#If False Then
            intType = -1
            rCnt = 0
            'ReDim pData(MAX_PDATA)
            Dim pData As List(Of String) = New List(Of String)()    'V5.0.0.9②
            ReDim pGpibData(MAX_PGPIBDATA)                          ' ###229

            If (False = IO.File.Exists(pPath)) Then Throw New FileNotFoundException() 'V4.4.0.0-1

            ' テキストファイルをオープンする
            Using fs As New FileStream(pPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                Dim enc As Encoding
                If (True = fs.IsUTF8()) Then
                    enc = Encoding.UTF8
                Else
                    enc = Encoding.GetEncoding("Shift_JIS")
                End If
                Using sr As New StreamReader(fs, enc)
                    ' ファイルの終端までループを繰り返します。
                    Do While (False = sr.EndOfStream)
                        strDAT = sr.ReadLine()                          ' 1行読み込み

                        ' データ種別判定
                        Select Case strDAT
                            Case FILE_CONST_VERSION                     ' ファイルバージョン
                                strSECT = "[FILE VERSION]"
                                intType = SECT_VERSION
                            Case FILE_CONST_PLATE_01                    ' プレートデータ１
                                strSECT = "[PLATE01]"
                                intType = SECT_PLATE01
                            Case FILE_CONST_PLATE_02                    ' プレートデータ２
                                strSECT = "[PLATE02]"
                                intType = SECT_PLATE02
                            Case FILE_CONST_PLATE_03                    ' プレートデータ３
                                strSECT = "[PLATE03]"
                                intType = SECT_PLATE03
                            Case FILE_CONST_PLATE_04                    ' プレートデータ４
                                strSECT = "[PLATE04]"
                                intType = SECT_PLATE04
                            Case FILE_CONST_PLATE_05                    ' プレートデータ５
                                strSECT = "[PLATE05]"
                                intType = SECT_PLATE05
                                '------ V1.13.0.0②↓ ------
                            Case FILE_CONST_PLATE_06                    ' プレートデータ６
                                strSECT = "[PLATE06]"
                                intType = SECT_PLATE06
                                '------ V1.13.0.0②↑ ------
                            Case FILE_CONST_CIR_DATA                    ' サーキットデータ(TKY用) 
                                strSECT = "[CIRCUIT]"
                                intType = SECT_CIRCUIT
                                rCnt = 1                                ' サーキットデータインデックス初期化(1ｵﾘｼﾞﾝ) 
                            Case FILE_CONST_STEPDATA                    ' STEPデータ
                                strSECT = "[STEP]"
                                intType = SECT_STEP
                                rCnt = 1                                ' STEPデータインデックス初期化(1ｵﾘｼﾞﾝ) 
                            Case FILE_CONST_GRP_DATA                    ' グループデータ
                                strSECT = "[GROUP]"
                                intType = SECT_GRP_DATA
                                rCnt = 1                                ' グループデータインデックス初期化(1ｵﾘｼﾞﾝ) 
                            Case FILE_CONST_TY2_DATA                    ' TY2データ
                                strSECT = "[TY2]"
                                intType = SECT_TY2_DATA
                                rCnt = 1                                ' TY2データインデックス初期化(1ｵﾘｼﾞﾝ) 
                            Case FILE_CONST_CIRN_DATA                   ' CIRCUITデータ(NET用)
                                strSECT = "[CIRCUIT AXIS]"
                                intType = SECT_CIR_AXIS
                                rCnt = 1                                ' CIRCUITデータインデックス初期化(1ｵﾘｼﾞﾝ) 
                            Case FILE_CONST_CIRIDATA                    ' サーキット間インターバルデータ(NET用)
                                strSECT = "[CIRCUIT INTERVAL]"
                                intType = SECT_CIR_ITVL
                                rCnt = 1                                ' サーキット間インターバルデータインデックス初期化(1ｵﾘｼﾞﾝ) 
                                'Case CONST_IKEI_DATA                    ' 異形面付け(TKY用) 未サポート 
                                '    intType = SECT_IKEI_DATA

                            Case FILE_CONST_RESISTOR                    ' 抵抗データ
                                strSECT = "[RESISTOR]]"
                                intType = SECT_REGISTOR
                                rCnt = 1                                ' 抵抗データインデックス初期化(1ｵﾘｼﾞﾝ)
                            Case FILE_CONST_CUT_DATA                    ' カットデータ
                                strSECT = "[CUT]]"
                                intType = SECT_CUT_DATA
                                rCnt = 0                                ' 抵抗データインデックス初期化(1ｵﾘｼﾞﾝ)

                                '----- ###229↓ -----
                            Case FILE_CONST_GPIB_DATA                   ' GPIBデータ
                                strSECT = "[GPIB]"
                                intType = SECT_GPIB_DATA
                                rCnt = 0                                ' GPIBデータインデックス初期化(1ｵﾘｼﾞﾝ)
                                '----- ###229↑ -----
                                'V1.13.0.0⑤
                            Case FILE_SINSYUKU_SELECT
                                strSECT = "[SINSYUKU]"
                                intType = SECT_SINSYUKU_DATA
                                rCnt = 0                                ' 伸縮補正用データインデックス初期化(1ｵﾘｼﾞﾝ)
                                ClearBlockSelect()                                  'V1.13.0.0⑤
                                'V1.13.0.0⑤

                            ' データ(セクション名以外)   
                            Case Else
                                ' ロードしたデータをデータ構造体へ格納する
                                'r = Set_Trim_Data(intType, strDAT, rCnt)
                                r = Set_Trim_Data(intType, strDAT, rCnt, pData)     'V5.0.0.9②
                                If (r <> cFRS_NORMAL) Then              ' エラーなら終了
                                    Exit Do
                                End If
                        End Select
                    Loop

                End Using
            End Using
#End If
            'V5.0.0.8①                  ↑
            Dim isTdcs As Boolean = pPath.EndsWith(".tdcs") OrElse _
                ((MACHINE_KD_RS = giMachineKd) AndAlso (pPath.EndsWith(".tdcw")))   ' V4.0.0.0-28
#If False Then  'V5.0.0.8①
            ' ロードしたプレートデータをプレートデータ構造体へ格納する
            If (r <> cFRS_NORMAL) Then GoTo STP_ERR
            r = Set_typPlateInfo(pData, strSECT, isTdcs)                ' V4.0.0.0-28
            If (r <> cFRS_NORMAL) Then GoTo STP_ERR

            ' TY2のﾃﾞｰﾀが存在しない場合、現状のﾃﾞｰﾀから作成する
            If ((r = cFRS_NORMAL) And (MaxTy2 = 0)) Then
                'r = GetTy2StepPos(1)
            End If

            '----- ###229↓ -----
            ' GPIBデータをGPIBデータ構造体へ格納する
            r = Set_typGpibInfo(pGpibData, typGpibInfo)
            If (r <> cFRS_NORMAL) Then GoTo STP_ERR
            '----- ###229↑ -----

            ' NGｶｯﾄﾎﾟｲﾝﾄ取得
            If (r = cFRS_NORMAL) Then
                'SetNG_MarkingPos() ' 2011.10.14 削除
            End If
#End If
            '----- V4.0.0.0-35↓ -----
            ' SL436S時で拡張子が".tdc"の場合に変換を行う
            If ((MACHINE_KD_RS = giMachineKd) And (isTdcs = False)) Then
                ConvertPlateData()
            End If
            'If (False = isTdcs) Then ConvertPlateData() ' V4.0.0.0-28
            '----- V4.0.0.0-35↑ -----

            ' 'V5.0.0.8③↓
            SetToOrgData()
            ' 'V5.0.0.8③↑

            ' 内部カメラ基準倍率を設定(ファイル読み込み時は内部カメラが表示されているため、DllVideo.dll内で表示倍率も変更される)  'V6.0.0.0⑰ 
            Form1.Instance.VideoLibrary1.StdMagnification = typPlateInfo.dblStdMagnification

            ' 終了処理
STP_EXIT:
            '----- V4.11.0.0⑤↓ (WALSIN殿SL436S対応) -----
            ' トリミング開始ブロック番号コンボボックスを再設定する
            If (r = cFRS_NORMAL) Then
                Call Form1.Set_StartBlkComb()
            End If
            '----- V4.11.0.0⑤↑ -----

            Return (r)

STP_ERR:
            If (strSECT = "") Then
                If frmAutoObj.gbFgAutoOperation432 = False Then                 'V6.1.4.0⑩
                    ' "指定されたファイルはトリミングパラメータのデータではありません"
                    Call Form1.System1.TrmMsgBox(gSysPrm, MSG_16, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, gAppName)
                    'V6.1.4.0⑩↓
                Else
                    Call FormMain.Z_PRINT(MSG_16 & vbCrLf)
                End If
                'V6.1.4.0⑩↑
            Else
                ' "ファイル入力エラー [セクション名]"
                strMSG = MSG_130 + " " + strSECT
                If frmAutoObj.gbFgAutoOperation432 = False Then                 'V6.1.4.0⑩
                    Call Form1.System1.TrmMsgBox(gSysPrm, strMSG, vbExclamation Or vbOKOnly, gAppName)
                    'V6.1.4.0⑩↓
                Else
                    Call FormMain.Z_PRINT(strMSG & vbCrLf)
                End If
                'V6.1.4.0⑩↑
            End If
            GoTo STP_EXIT

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "File.File_Read() TRAP ERROR = " & ex.Message
            MsgBox(strMSG)
            r = cERR_TRAP                                           ' Return値 = 例外エラー
            GoTo STP_EXIT
        End Try

#If False Then
        Try
            ' 初期処理
            r = cFRS_NORMAL                                         ' Return値 = 正常
            File_Read = 0
            Err_num = 0
            iFlg = 0
            Call Init_FileVer_Sub()                                 ' ﾁｪｯｸ用ﾌｧｲﾙﾊﾞｰｼﾞｮﾝ設定
            Call Init_AllTrmmingData()                              ' グローバルデータ初期化(Plate/Circuit/Resistor/Cut)
            intType = -1
            rCnt = 0
            ReDim pData(MAX_PDATA)
            ReDim pGpibData(MAX_PGPIBDATA)                          ' ###229

            ' テキストファイルをオープンする
            intFileNo = FreeFile()                                  ' 使用可能なファイルナンバーを取得
            FileOpen(intFileNo, pPath, OpenMode.Input)
            iFlg = 1

            ' ファイルの終端までループを繰り返します。
            Do While Not EOF(intFileNo)

                strDAT = LineInput(intFileNo)                       ' 1行読み込み
                ' データ種別判定
                Select Case strDAT
                    Case FILE_CONST_VERSION                         ' ファイルバージョン
                        strSECT = "[FILE VERSION]"
                        intType = SECT_VERSION
                    Case FILE_CONST_PLATE_01                        ' プレートデータ１
                        strSECT = "[PLATE01]"
                        intType = SECT_PLATE01
                    Case FILE_CONST_PLATE_02                        ' プレートデータ２
                        strSECT = "[PLATE02]"
                        intType = SECT_PLATE02
                    Case FILE_CONST_PLATE_03                        ' プレートデータ３
                        strSECT = "[PLATE03]"
                        intType = SECT_PLATE03
                    Case FILE_CONST_PLATE_04                        ' プレートデータ４
                        strSECT = "[PLATE04]"
                        intType = SECT_PLATE04
                    Case FILE_CONST_PLATE_05                        ' プレートデータ５
                        strSECT = "[PLATE05]"
                        intType = SECT_PLATE05
                        '------ V1.13.0.0②↓ ------
                    Case FILE_CONST_PLATE_06                        ' プレートデータ６
                        strSECT = "[PLATE06]"
                        intType = SECT_PLATE06
                        '------ V1.13.0.0②↑ ------
                        'V5.0.0.6①↓
                    Case FILE_CONST_PLATE_OPTION                    ' オプション
                        strSECT = "[OPTION]"
                        intType = SECT_PLATE_OPTION
                        'V5.0.0.6①↑
                    Case FILE_CONST_CIR_DATA                        ' サーキットデータ(TKY用) 
                        strSECT = "[CIRCUIT]"
                        intType = SECT_CIRCUIT
                        rCnt = 1                                    ' サーキットデータインデックス初期化(1ｵﾘｼﾞﾝ) 
                    Case FILE_CONST_STEPDATA                        ' STEPデータ
                        strSECT = "[STEP]"
                        intType = SECT_STEP
                        rCnt = 1                                    ' STEPデータインデックス初期化(1ｵﾘｼﾞﾝ) 
                    Case FILE_CONST_GRP_DATA                        ' グループデータ
                        strSECT = "[GROUP]"
                        intType = SECT_GRP_DATA
                        rCnt = 1                                    ' グループデータインデックス初期化(1ｵﾘｼﾞﾝ) 
                    Case FILE_CONST_TY2_DATA                        ' TY2データ
                        strSECT = "[TY2]"
                        intType = SECT_TY2_DATA
                        rCnt = 1                                    ' TY2データインデックス初期化(1ｵﾘｼﾞﾝ) 
                    Case FILE_CONST_CIRN_DATA                       ' CIRCUITデータ(NET用)
                        strSECT = "[CIRCUIT AXIS]"
                        intType = SECT_CIR_AXIS
                        rCnt = 1                                    ' CIRCUITデータインデックス初期化(1ｵﾘｼﾞﾝ) 
                    Case FILE_CONST_CIRIDATA                        ' サーキット間インターバルデータ(NET用)
                        strSECT = "[CIRCUIT INTERVAL]"
                        intType = SECT_CIR_ITVL
                        rCnt = 1                                    ' サーキット間インターバルデータインデックス初期化(1ｵﾘｼﾞﾝ) 
                        'Case CONST_IKEI_DATA                        ' 異形面付け(TKY用) 未サポート 
                        '    intType = SECT_IKEI_DATA

                    Case FILE_CONST_RESISTOR                        ' 抵抗データ
                        strSECT = "[RESISTOR]]"
                        intType = SECT_REGISTOR
                        rCnt = 1                                    ' 抵抗データインデックス初期化(1ｵﾘｼﾞﾝ)
                    Case FILE_CONST_CUT_DATA                        ' カットデータ
                        strSECT = "[CUT]]"
                        intType = SECT_CUT_DATA
                        rCnt = 0                                    ' 抵抗データインデックス初期化(1ｵﾘｼﾞﾝ)

                        '----- ###229↓ -----
                    Case FILE_CONST_GPIB_DATA                       ' GPIBデータ
                        strSECT = "[GPIB]"
                        intType = SECT_GPIB_DATA
                        rCnt = 0                                    ' GPIBデータインデックス初期化(1ｵﾘｼﾞﾝ)
                        '----- ###229↑ -----
                        'V1.13.0.0⑤
                    Case FILE_SINSYUKU_SELECT
                        strSECT = "[SINSYUKU]"
                        intType = SECT_SINSYUKU_DATA
                        rCnt = 0                                    ' 伸縮補正用データインデックス初期化(1ｵﾘｼﾞﾝ)
                        ClearBlockSelect()                                      'V1.13.0.0⑤
                        'V1.13.0.0⑤

                        ' データ(セクション名以外)   
                    Case Else
                        ' ロードしたデータをデータ構造体へ格納する
                        r = Set_Trim_Data(intType, strDAT, rCnt)
                        If (r <> cFRS_NORMAL) Then                  ' エラーなら終了
                            Exit Do
                        End If
                End Select
            Loop

            Dim isTdcs As Boolean = pPath.EndsWith(".tdcs") OrElse _
                ((MACHINE_KD_RS = giMachineKd) AndAlso (pPath.EndsWith(".tdcw")))   ' V4.0.0.0-28

            ' ロードしたプレートデータをプレートデータ構造体へ格納する
            If (r <> cFRS_NORMAL) Then GoTo STP_ERR
            r = Set_typPlateInfo(pData, strSECT, isTdcs)                ' V4.0.0.0-28
            If (r <> cFRS_NORMAL) Then GoTo STP_ERR

            ' TY2のﾃﾞｰﾀが存在しない場合、現状のﾃﾞｰﾀから作成する
            If ((r = cFRS_NORMAL) And (MaxTy2 = 0)) Then
                'r = GetTy2StepPos(1)
            End If

            '----- ###229↓ -----
            ' GPIBデータをGPIBデータ構造体へ格納する
            r = Set_typGpibInfo(pGpibData, typGpibInfo)
            If (r <> cFRS_NORMAL) Then GoTo STP_ERR
            '----- ###229↑ -----

            ' NGｶｯﾄﾎﾟｲﾝﾄ取得
            If (r = cFRS_NORMAL) Then
                'SetNG_MarkingPos() ' 2011.10.14 削除
            End If

            '----- V4.0.0.0-35↓ -----
            ' SL436S時で拡張子が".tdc"の場合に変換を行う
            If ((MACHINE_KD_RS = giMachineKd) And (isTdcs = False)) Then
                ConvertPlateData()
            End If
            'If (False = isTdcs) Then ConvertPlateData() ' V4.0.0.0-28
            '----- V4.0.0.0-35↑ -----

            ' 終了処理
STP_EXIT:
            If (iFlg = 1) Then
                FileClose(intFileNo)
            End If
            '----- V4.11.0.0⑤↓ (WALSIN殿SL436S対応) -----
            ' トリミング開始ブロック番号コンボボックスを再設定する
            If (r = cFRS_NORMAL) Then
                Call Form1.Set_StartBlkComb()
            End If
            '----- V4.11.0.0⑤↑ -----
            Return (r)
            Exit Function

STP_ERR:
            If (strSECT = "") Then
                ' "指定されたファイルはトリミングパラメータのデータではありません"
                Call Form1.System1.TrmMsgBox(gSysPrm, MSG_16, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, gAppName)
            Else
                ' "ファイル入力エラー [セクション名]"
                strMSG = MSG_130 + " " + strSECT
                Call Form1.System1.TrmMsgBox(gSysPrm, strMSG, vbExclamation Or vbOKOnly, gAppName)
            End If
            GoTo STP_EXIT

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "File.File_Read() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            r = cERR_TRAP                                           ' Return値 = 例外エラー
            GoTo STP_EXIT
        End Try
#End If
    End Function
#End Region

#Region "LOAD・SAVEの共通化によりTrimDataEditorで定義"
#If False Then 'V5.0.0.8①
#Region "ロードしたデータをデータ構造体へ格納する【TKY,CHIP,NET統合版】"
    '''----------------------------------------------------------------------
    '''<summary>ロードしたデータをデータ構造体へ格納する【TKY,CHIP,NET統合版】</summary>
    '''<param name="intType">(INP)データタイプ</param>
    '''<param name="strDAT"> (INP)データ</param>
    '''<param name="rCnt">   (I/O)データインデックス</param>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''----------------------------------------------------------------------       
    Private Function Set_Trim_Data(ByVal intType As Integer, ByVal strDAT As String, ByRef rCnt As Integer,
                                   ByVal pData As List(Of String)) As Integer   'V5.0.0.9②
        'Private Function Set_Trim_Data(ByVal intType As Integer, ByVal strDAT As String, ByRef rCnt As Integer) As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' 初期処理
            r = cFRS_NORMAL                                             ' Return値 = 正常

            Select Case (intType)
                Case SECT_VERSION
                    ' ファイルバージョンデータ(ファイルバージョンをSL432HW_FileVerに設定
                    If (gTkyKnd = KND_TKY) Then
                        r = FileVerCheck_TKY(strDAT)
                    ElseIf (gTkyKnd = KND_CHIP) Then
                        r = FileVerCheck_CHIP(strDAT)
                    Else
                        r = FileVerCheck_NET(strDAT)
                    End If

                Case SECT_PLATE01, SECT_PLATE02, SECT_PLATE03, SECT_PLATE04, SECT_PLATE05, SECT_PLATE06, SECT_PLATE_OPTION ' V1.13.0.0② 'V5.0.0.6①SECT_PLATE_OPTION ADD
                    ' プレートデータ1から6をプレートデータ格納域へ設定する
                    'pData(rCnt) = strDAT                            ' プレートデータ格納(0ｵﾘｼﾞﾝ)
                    pData.Add(strDAT)                               ' プレートデータ格納(0ｵﾘｼﾞﾝ)     'V5.0.0.9②
                    rCnt = rCnt + 1

                Case SECT_CIRCUIT
                    ' サーキットデータ(TKY用) 
                    ' データをサーキットデータ構造体へ格納する
                    'r = Set_typCircuitInfoArray(rCnt, strDAT)      ' V1.13.0.0②
                    r = Set_typCircuitInfoArray(strDAT, rCnt)       ' V1.13.0.0②

                Case SECT_STEP
                    ' ステップデータ(CHIP用) をステップデータ構造体へ格納する
                    r = Set_typStepInfo(strDAT, rCnt)

                Case SECT_GRP_DATA
                    ' グループデータ(CHIP用)をグループデータ構造体へ格納する
                    r = Set_typGrpInfoArrayChip(strDAT, rCnt)

                Case SECT_TY2_DATA
                    ' TY2データをTY2データ構造体へ格納する
                    r = Set_typTy2InfoArray(strDAT, rCnt)

                Case SECT_CIR_AXIS
                    ' サーキット座標データ(NET用)をサーキット座標データ構造体へ格納する
                    r = Set_typCirAxisInfoArray(strDAT, rCnt)

                Case SECT_CIR_ITVL
                    ' サーキット間インターバルデータ(NET用)をサーキット間インターバルデータ構造体へ格納する
                    r = Set_typCirInInfoArray(strDAT, rCnt)

                    ' 異形面付けデータ(TKY用) 未サポート
                    ' Case SECT_IKEI_DATA
                    ' データを異形面付けデータ構造体へ格納する
                    ' r = Set_typIKEIInfo(strDAT)

                Case SECT_REGISTOR
                    ' 抵抗データを抵抗データ構造体へ格納する
                    r = Get_RESIST_Data(strDAT, rCnt)

                Case SECT_CUT_DATA
                    ' カットデータをカットデータ構造体へ格納する
                    r = Get_CUT_Data(strDAT, rCnt)

                    '----- ###229↓ -----
                Case SECT_GPIB_DATA
                    pGpibData(rCnt) = strDAT                        ' GPIBデータをpGpibDataに格納(0ｵﾘｼﾞﾝ)
                    rCnt = rCnt + 1
                    '----- ###229↑ -----
                    'V1.13.0.0⑤
                Case SECT_SINSYUKU_DATA
                    r = SetSinsyukuData(strDAT)
                    'V1.13.0.0⑤
                Case Else
                    r = cFRS_FIOERR_INP                             ' Return値 = エラー
            End Select
            Return (r)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "File.Set_Trim_Data() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                      ' Return値 = 例外エラー
        End Try
    End Function
#End Region

#Region "ロードしたプレートデータをプレートデータ構造体へ格納する【TKY,CHIP,NET統合版】"
    '''=========================================================================
    ''' <summary>ロードしたプレートデータをプレートデータ構造体へ格納する</summary>
    ''' <param name="mDATA">  (INP)データ</param>
    ''' <param name="strSECT">(OUT)エラーとなったセクション名</param>
    ''' <param name="isTdcs"> (INP)true:tdcsﾌｧｲﾙである,false:tdcsﾌｧｲﾙではない V4.0.0.0-28</param>
    ''' <returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function Set_typPlateInfo(ByVal mDATA As List(Of String), ByRef strSECT As String, ByVal isTdcs As Boolean) As Integer  'V5.0.0.9②
        'Private Function Set_typPlateInfo(ByVal mDATA() As String, ByRef strSECT As String, ByVal isTdcs As Boolean) As Integer

        Dim i As Integer
        Dim strWK() As String
        Dim strMSG As String

        Try
            ' データをプレートデータ構造体へ格納する
            With typPlateInfo
                ' [PLATE01]データをプレートデータ構造体へ格納する
                strSECT = "[PLATE01]"
                i = 0
                .strDataName = mDATA(i) : i = i + 1                             ' トリミングデータ名
                .intDirStepRepeat = Short.Parse(mDATA(i)) : i = i + 1           ' ｽﾃｯﾌﾟ＆ﾘﾋﾟｰﾄ
                .intChipStepCnt = Short.Parse(mDATA(i)) : i = i + 1             ' チップステップ数
                strWK = mDATA(i).Split(",") : i = i + 1                         ' プレート数X,Yを','で分割して取出す
                .intPlateCntXDir = Short.Parse(strWK(0))                        ' プレート数Ｘ
                .intPlateCntYDir = Short.Parse(strWK(1))                        ' プレートＹ
                strWK = mDATA(i).Split(",") : i = i + 1                         ' ﾌﾞﾛｯｸ数X,Yを','で分割して取出す
                .intBlockCntXDir = Short.Parse(strWK(0))                        ' ﾌﾞﾛｯｸ数Ｘ
                .intBlockCntYDir = Short.Parse(strWK(1))                        ' ﾌﾞﾛｯｸ数Ｙ
                strWK = mDATA(i).Split(",") : i = i + 1                         ' プレート間隔X,Yを','で分割して取出す
                .dblPlateItvXDir = Double.Parse(strWK(0))                       ' プレート間隔Ｘ
                .dblPlateItvYDir = Double.Parse(strWK(1))                       ' プレート間隔Ｙ
                strWK = mDATA(i).Split(",") : i = i + 1                         ' ブロックサイズX,Yを','で分割して取出す
                .dblBlockSizeXDir = Double.Parse(strWK(0))                      ' ブロックサイズＸ
                .dblBlockSizeYDir = Double.Parse(strWK(1))                      ' ブロックサイズＹ
                strWK = mDATA(i).Split(",") : i = i + 1                         ' ﾃｰﾌﾞﾙ位置ｵﾌｾｯﾄX,Yを','で分割して取出す
                .dblTableOffsetXDir = Double.Parse(strWK(0))                    ' ﾃｰﾌﾞﾙ位置ｵﾌｾｯﾄX
                .dblTableOffsetYDir = Double.Parse(strWK(1))                    ' ﾃｰﾌﾞﾙ位置ｵﾌｾｯﾄY
                strWK = mDATA(i).Split(",") : i = i + 1                         ' ﾋﾞｰﾑ位置ｵﾌｾｯﾄX,Yを','で分割して取出す
                .dblBpOffSetXDir = Double.Parse(strWK(0))                       ' ﾋﾞｰﾑ位置ｵﾌｾｯﾄX
                .dblBpOffSetYDir = Double.Parse(strWK(1))                       ' ﾋﾞｰﾑ位置ｵﾌｾｯﾄX
                ' BPｵﾌｾｯﾄの入力なしの場合、ﾃｰﾌﾞﾙｵﾌｾｯﾄ値の逆数をｾｯﾄ
                If (gSysPrm.stCTM.giBPOffsetInput <> 0) Then
                    .dblBpOffSetXDir = -1.0 * .dblTableOffsetXDir
                    .dblBpOffSetYDir = -1.0 * .dblTableOffsetYDir
                End If
                strWK = mDATA(i).Split(",") : i = i + 1                         ' ｱｼﾞｬｽﾄ位置ｵﾌｾｯﾄX,Yを','で分割して取出す(未使用)
                .dblAdjOffSetXDir = Double.Parse(strWK(0))                      ' ｱｼﾞｬｽﾄ位置ｵﾌｾｯﾄX
                .dblAdjOffSetYDir = Double.Parse(strWK(1))                      ' ｱｼﾞｬｽﾄ位置ｵﾌｾｯﾄY
                .intCurcuitCnt = Short.Parse(mDATA(i)) : i = i + 1              ' サーキット数
                .intNGMark = Short.Parse(mDATA(i)) : i = i + 1                  ' NGﾏｰｷﾝｸﾞ
                .intDelayTrim = Short.Parse(mDATA(i)) : i = i + 1               ' ﾃﾞｨﾚｲﾄﾘﾑ
                '----- V1.23.0.0⑥↓ -----
                ' ディレイトリム２指定時で、シスパラのディレイトリム２無効なら「ディレイトリム実行しない」とする
                If (.intDelayTrim = 2) And (gSysPrm.stSPF.giDelayTrim2 = 0) Then
                    .intDelayTrim = 0
                End If
                '----- V1.23.0.0⑥↑ -----
                .intNgJudgeUnit = Short.Parse(mDATA(i)) : i = i + 1             ' ＮＧ判定単位
                .intNgJudgeLevel = Short.Parse(mDATA(i)) : i = i + 1            ' ＮＧ判定基準
                '----- V4.0.0.0-28 ↓ -----
                'If (True = isTdcs) Then                                        ' V4.0.0.0-35
                If ((isTdcs = True) Or (giMachineKd <> MACHINE_KD_RS)) Then     ' V4.0.0.0-35
                    .dblZOffSet = Double.Parse(mDATA(i)) : i = i + 1            ' ZON位置
                    .dblZStepUpDist = Double.Parse(mDATA(i)) : i = i + 1        ' ｽﾃｯﾌﾟ上昇距離
                    .dblZWaitOffset = Double.Parse(mDATA(i)) : i = i + 1        ' ZOFF位置
                Else
                    .dblZOffSet = Z_ON_POS_SIMPLE : i = i + 1                   ' ZON位置
                    .dblZStepUpDist = Z_STEP_POS_SIMPLE : i = i + 1             ' ｽﾃｯﾌﾟ上昇距離
                    .dblZWaitOffset = Z_OFF_POS_SIMPLE : i = i + 1              ' ZOFF位置
                End If
                '----- V4.0.0.0-28 ↑ -----
                '----- V1.13.0.0②↓ -----
                If (SL432HW_FileVer >= 10.04) Then
                    .dblLwPrbStpDwDist = Double.Parse(mDATA(i)) : i = i + 1     ' 下方ﾌﾟﾛｰﾌﾞｽﾃｯﾌﾟ下降距離
                    .dblLwPrbStpUpDist = Double.Parse(mDATA(i)) : i = i + 1     ' 下方ﾌﾟﾛｰﾌﾞｽﾃｯﾌﾟ上昇距離
                Else
                    .dblLwPrbStpDwDist = 0.0                                    ' 下方ﾌﾟﾛｰﾌﾞｽﾃｯﾌﾟ下降距離(初期値)
                    .dblLwPrbStpUpDist = 0.0                                    ' 下方ﾌﾟﾛｰﾌﾞｽﾃｯﾌﾟ上昇距離(初期値)
                End If
                '----- V1.13.0.0②↑ -----
                '----- V4.0.0.0④↓ -----
                ' プローブリトライ回数(0=リトライなし)(オプション)
                If (SL432HW_FileVer >= 10.1) Then
                    .intPrbRetryCount = Short.Parse(mDATA(i)) : i = i + 1       ' プローブリトライ回数(0=リトライなし)
                Else
                    .intPrbRetryCount = 0                                       ' プローブリトライ回数(0=リトライなし)
                End If
                '----- V4.0.0.0④↑ -----
                '----- V1.23.0.0⑦↓ -----
                ' プローブチェック項目(オプション)
                If (SL432HW_FileVer >= 10.09) Then
                    .intPrbChkPlt = Short.Parse(mDATA(i)) : i = i + 1           ' 枚数
                    .intPrbChkBlk = Short.Parse(mDATA(i)) : i = i + 1           ' ブロック
                    .dblPrbTestLimit = Double.Parse(mDATA(i)) : i = i + 1       ' 誤差±%
                End If
                If ((SL432HW_FileVer < 10.09) Or (giProbeCheck = 0)) Then
                    .intPrbChkPlt = 0                                           ' 枚数
                    .intPrbChkBlk = 1                                           ' ブロック
                    .dblPrbTestLimit = 0.0                                      ' 誤差±%
                End If
                '----- V1.23.0.0⑦↑ -----

                ' [PLATE03]データをプレートデータ構造体へ格納する
                strSECT = "[PLATE03]"
                .intResistDir = Short.Parse(mDATA(i)) : i = i + 1               ' 抵抗並び方向
                .intCircuitCntInBlock = Short.Parse(mDATA(i)) : i = i + 1       ' 1ﾌﾞﾛｯｸ内ｻｰｷｯﾄ数 
                strWK = mDATA(i).Split(",") : i = i + 1                         ' ｻｰｷｯﾄｻｲｽﾞX,Yを','で分割して取出す
                .dblCircuitSizeXDir = Double.Parse(strWK(0))                    ' ｻｰｷｯﾄｻｲｽﾞX  
                .dblCircuitSizeYDir = Double.Parse(strWK(1))                    ' ｻｰｷｯﾄｻｲｽﾞY 
                .intResistCntInBlock = Short.Parse(mDATA(i)) : i = i + 1        ' 1ﾌﾞﾛｯｸ内抵抗数
                .intResistCntInGroup = Short.Parse(mDATA(i)) : i = i + 1        ' 1ｸﾞﾙｰﾌﾟ内抵抗数
                strWK = mDATA(i).Split(",") : i = i + 1                         ' ﾌﾞﾛｯｸ内ｸﾞﾙｰﾌﾟ数X,Yを','で分割して取出す
                .intGroupCntInBlockXBp = Short.Parse(strWK(0))                  ' ｸﾞﾙｰﾌﾟ数X(ＢＰグループ数)
                .intGroupCntInBlockYStage = Short.Parse(strWK(1))               ' ｸﾞﾙｰﾌﾟ数Y(ステージグループ数)
                strWK = mDATA(i).Split(",") : i = i + 1                         ' ステージグループ内ブロック数X,Yを','で分割して取出す
                .intBlkCntInStgGrpX = Short.Parse(strWK(0))                     ' ステージグループ内ブロック数X
                .intBlkCntInStgGrpY = Short.Parse(strWK(1))                     ' ステージグループ内ブロック数Y
                strWK = mDATA(i).Split(",") : i = i + 1                         ' ｸﾞﾙｰﾌﾟ間隔BP(X),Stage(Y) を','で分割して取出す
                .dblBpGrpItv = Double.Parse(strWK(0))                           ' ｸﾞﾙｰﾌﾟ間隔X
                .dblStgGrpItvY = Double.Parse(strWK(1))                         ' ｸﾞﾙｰﾌﾟ間隔Y(Dummy)
                strWK = mDATA(i).Split(",") : i = i + 1                         ' ﾁｯﾌﾟｻｲｽﾞX,Yを','で分割して取出す
                .dblChipSizeXDir = Double.Parse(strWK(0))                       ' ﾁｯﾌﾟｻｲｽﾞX
                .dblChipSizeYDir = Double.Parse(strWK(1))                       ' ﾁｯﾌﾟｻｲｽﾞY
                strWK = mDATA(i).Split(",") : i = i + 1                         ' ｽﾃｯﾌﾟｵﾌｾｯﾄ量X,Yを','で分割して取出す
                .dblStepOffsetXDir = Double.Parse(strWK(0))                     ' ｽﾃｯﾌﾟｵﾌｾｯﾄ量X
                .dblStepOffsetYDir = Double.Parse(strWK(1))                     ' ｽﾃｯﾌﾟｵﾌｾｯﾄ量Y
                strWK = mDATA(i).Split(",") : i = i + 1                         ' ﾌﾞﾛｯｸｻｲｽﾞ補正X,Yを','で分割して取出す
                .dblBlockSizeReviseXDir = Double.Parse(strWK(0))                ' ﾌﾞﾛｯｸｻｲｽﾞ補正X
                .dblBlockSizeReviseYDir = Double.Parse(strWK(1))                ' ﾌﾞﾛｯｸｻｲｽﾞ補正Y
                strWK = mDATA(i).Split(",") : i = i + 1                         ' ﾌﾞﾛｯｸ間隔X,Yを','で分割して取出す
                .dblBlockItvXDir = Double.Parse(strWK(0))                       ' ﾌﾞﾛｯｸ間隔X
                .dblBlockItvYDir = Double.Parse(strWK(1))                       ' ﾌﾞﾛｯｸ間隔Y
                .intContHiNgBlockCnt = Short.Parse(mDATA(i)) : i = i + 1        ' 連続NG-HIGH抵抗ﾌﾞﾛｯｸ数
                strWK = mDATA(i).Split(",") : i = i + 1                         ' プレートサイズX,Yを','で分割して取出す
                .dblPlateSizeX = Double.Parse(strWK(0))                         ' プレートサイズX 
                .dblPlateSizeY = Double.Parse(strWK(1))                         ' プレートサイズY
                strWK = mDATA(i).Split(",") : i = i + 1                         ' ステージグループ間隔X,Yを','で分割して取出す
                .dblStgGrpItvX = Double.Parse(strWK(0))                         ' ステージグループ間隔X
                .dblStgGrpItvY = Double.Parse(strWK(1))                         ' ステージグループ間隔Y

                ' [PLATE02]データをプレートデータ構造体へ格納する
                strSECT = "[PLATE02]"
                .intReviseMode = Short.Parse(mDATA(i)) : i = i + 1              ' 補正モード
                .intManualReviseType = Short.Parse(mDATA(i)) : i = i + 1        ' 補正方法
                strWK = mDATA(i).Split(",") : i = i + 1                         ' 補正位置座標1X,Yを','で分割して取出す
                .dblReviseCordnt1XDir = Double.Parse(strWK(0))                  ' 補正位置座標1X
                .dblReviseCordnt1YDir = Double.Parse(strWK(1))                  ' 補正位置座標1Y
                strWK = mDATA(i).Split(",") : i = i + 1                         ' 補正位置座標2X,Yを','で分割して取出す
                .dblReviseCordnt2XDir = Double.Parse(strWK(0))                  ' 補正位置座標2X
                .dblReviseCordnt2YDir = Double.Parse(strWK(1))                  ' 補正位置座標2Y
                strWK = mDATA(i).Split(",") : i = i + 1                         ' 補正ﾎﾟｼﾞｼｮﾝｵﾌｾｯﾄX,Yを','で分割して取出す
                .dblReviseOffsetXDir = Double.Parse(strWK(0))                   ' 補正ﾎﾟｼﾞｼｮﾝｵﾌｾｯﾄX
                .dblReviseOffsetYDir = Double.Parse(strWK(1))                   ' 補正ﾎﾟｼﾞｼｮﾝｵﾌｾｯﾄY
                .intRecogDispMode = Short.Parse(mDATA(i)) : i = i + 1           ' 認識データ表示モード
                strWK = mDATA(i).Split(",") : i = i + 1                         ' ﾋﾟｸｾﾙ値X,Yを','で分割して取出す
                .dblPixelValXDir = Double.Parse(strWK(0))                       ' ﾋﾟｸｾﾙ値X
                .dblPixelValYDir = Double.Parse(strWK(1))                       ' ﾋﾟｸｾﾙ値Y
                strWK = mDATA(i).Split(",") : i = i + 1                         ' 補正位置ﾊﾟﾀｰﾝNo1,2を','で分割して取出す
                .intRevisePtnNo1 = Short.Parse(strWK(0))                        ' 補正位置ﾊﾟﾀｰﾝNo1
                .intRevisePtnNo2 = Short.Parse(strWK(1))                        ' 補正位置ﾊﾟﾀｰﾝNo2
                strWK = mDATA(i).Split(",") : i = i + 1                         ' 補正位置ﾊﾟﾀｰﾝｸﾞﾙｰﾌﾟNo1,2を','で分割して取出す
                .intRevisePtnNo1GroupNo = Short.Parse(strWK(0))                 ' 補正位置ﾊﾟﾀｰﾝｸﾞﾙｰﾌﾟNo1
                .intRevisePtnNo2GroupNo = Short.Parse(strWK(1))                 ' 補正位置ﾊﾟﾀｰﾝｸﾞﾙｰﾌﾟNo2
                .dblRotateTheta = Double.Parse(mDATA(i)) : i = i + 1            ' θ回転角度 '###037
                .dblTThetaOffset = Double.Parse(mDATA(i)) : i = i + 1           ' Ｔθオフセット
                strWK = mDATA(i).Split(",") : i = i + 1                         ' Ｔθ基準位置1X,Yを','で分割して取出す
                .dblTThetaBase1XDir = Double.Parse(strWK(0))                    ' Ｔθ基準位置1X
                .dblTThetaBase1YDir = Double.Parse(strWK(1))                    ' Ｔθ基準位置1Y
                strWK = mDATA(i).Split(",") : i = i + 1                         ' Ｔθ基準位置2X,Yを','で分割して取出す
                .dblTThetaBase2XDir = Double.Parse(strWK(0))                    ' Ｔθ基準位置2X
                .dblTThetaBase2YDir = Double.Parse(strWK(1))                    ' Ｔθ基準位置2Y
                'V5.0.0.9②                                          ↓ ラフアライメント用
                If (FILE_VER_10_12 <= SL432HW_FileVer) Then
                    .intReviseExecRgh = Short.Parse(mDATA(i)) : i = i + 1       ' 補正有無(0:補正なし, 1:補正あり)
                    strWK = mDATA(i).Split(",") : i = i + 1
                    .dblReviseCordnt1XDirRgh = Double.Parse(strWK(0))           ' 補正位置座標1X
                    .dblReviseCordnt1YDirRgh = Double.Parse(strWK(1))           ' 補正位置座標1Y
                    strWK = mDATA(i).Split(",") : i = i + 1
                    .dblReviseCordnt2XDirRgh = Double.Parse(strWK(0))           ' 補正位置座標2X
                    .dblReviseCordnt2YDirRgh = Double.Parse(strWK(1))           ' 補正位置座標2Y
                    strWK = mDATA(i).Split(",") : i = i + 1
                    .dblReviseOffsetXDirRgh = Double.Parse(strWK(0))            ' 補正ﾎﾟｼﾞｼｮﾝｵﾌｾｯﾄX
                    .dblReviseOffsetYDirRgh = Double.Parse(strWK(1))            ' 補正ﾎﾟｼﾞｼｮﾝｵﾌｾｯﾄY
                    .intRecogDispModeRgh = Short.Parse(mDATA(i)) : i = i + 1    ' 認識ﾃﾞｰﾀ表示ﾓｰﾄﾞ(0:表示なし, 1:表示する)
                    strWK = mDATA(i).Split(",") : i = i + 1
                    .intRevisePtnNo1Rgh = Short.Parse(strWK(0))                 ' 補正位置ﾊﾟﾀｰﾝNo1
                    .intRevisePtnNo2Rgh = Short.Parse(strWK(1))                 ' 補正位置ﾊﾟﾀｰﾝNo2
                    strWK = mDATA(i).Split(",") : i = i + 1
                    .intRevisePtnNo1GroupNoRgh = Short.Parse(strWK(0))          ' 補正位置ﾊﾟﾀｰﾝNo1グループNo
                    .intRevisePtnNo2GroupNoRgh = Short.Parse(strWK(1))          ' 補正位置ﾊﾟﾀｰﾝNo2グループNo
                End If
                'V5.0.0.9②                                          ↑
                ' [PLATE04]データをプレートデータ構造体へ格納する
                strSECT = "[PLATE04]"
                strWK = mDATA(i).Split(",") : i = i + 1                         ' 座標X,Yを','で分割して取出す
                .dblCaribBaseCordnt1XDir = Double.Parse(strWK(0))               ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標1X
                .dblCaribBaseCordnt1YDir = Double.Parse(strWK(1))               ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標1Y
                strWK = mDATA(i).Split(",") : i = i + 1                         ' 座標X,Yを','で分割して取出す
                .dblCaribBaseCordnt2XDir = Double.Parse(strWK(0))               ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標2X
                .dblCaribBaseCordnt2YDir = Double.Parse(strWK(1))               ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標2Y
                strWK = mDATA(i).Split(",") : i = i + 1                         ' 座標X,Yを','で分割して取出す
                .dblCaribTableOffsetXDir = Double.Parse(strWK(0))               ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄX
                .dblCaribTableOffsetYDir = Double.Parse(strWK(1))               ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄY
                strWK = mDATA(i).Split(",") : i = i + 1                         ' ﾊﾟﾀｰﾝ登録No1,2を','で分割して取出す
                .intCaribPtnNo1 = Short.Parse(strWK(0))                         ' ﾊﾟﾀｰﾝ登録No1
                .intCaribPtnNo2 = Short.Parse(strWK(1))                         ' ﾊﾟﾀｰﾝ登録No2
                strWK = mDATA(i).Split(",") : i = i + 1                         ' ﾊﾟﾀｰﾝｸﾞﾙｰﾌﾟNo1,2を','で分割して取出す
                .intCaribPtnNo1GroupNo = Short.Parse(strWK(0))                  ' ﾊﾟﾀｰﾝｸﾞﾙｰﾌﾟNo1
                .intCaribPtnNo2GroupNo = Short.Parse(strWK(1))                  ' ﾊﾟﾀｰﾝｸﾞﾙｰﾌﾟNo2
                .dblCaribCutLength = Double.Parse(mDATA(i)) : i = i + 1         ' ｷｬﾘﾌﾞﾚｰｼｮﾝｶｯﾄ長
                .dblCaribCutSpeed = Double.Parse(mDATA(i)) : i = i + 1          ' ｷｬﾘﾌﾞﾚｰｼｮﾝｶｯﾄ速度
                .dblCaribCutQRate = Double.Parse(mDATA(i)) : i = i + 1          ' ｷｬﾘﾌﾞﾚｰｼｮﾝｶｯﾄQﾚｰﾄ
                .intCaribCutCondNo = Short.Parse(mDATA(i)) : i = i + 1          ' ｷｬﾘﾌﾞﾚｰｼｮﾝ加工条件番号(FL用)

                strWK = mDATA(i).Split(",") : i = i + 1                         ' 座標X,Yを','で分割して取出す
                .dblCutPosiReviseOffsetXDir = Double.Parse(strWK(0))            ' ｶｯﾄ点補正ﾃｰﾌﾞﾙｵﾌｾｯﾄX
                .dblCutPosiReviseOffsetYDir = Double.Parse(strWK(1))            ' ｶｯﾄ点補正ﾃｰﾌﾞﾙｵﾌｾｯﾄY
                .intCutPosiRevisePtnNo = Short.Parse(mDATA(i)) : i = i + 1      ' ｶｯﾄ点補正ﾊﾟﾀｰﾝ登録No
                .dblCutPosiReviseCutLength = Double.Parse(mDATA(i)) : i = i + 1 ' ｶｯﾄ点補正ｶｯﾄ長
                .dblCutPosiReviseCutSpeed = Double.Parse(mDATA(i)) : i = i + 1  ' ｶｯﾄ点補正ｶｯﾄ速度
                .dblCutPosiReviseCutQRate = Double.Parse(mDATA(i)) : i = i + 1  ' ｶｯﾄ点補正ﾚｰｻﾞQﾚｰﾄ
                .intCutPosiReviseGroupNo = Short.Parse(mDATA(i)) : i = i + 1    ' ｶｯﾄ点補正ﾊﾟﾀｰﾝｸﾞﾙｰﾌﾟNo
                .intCutPosiReviseCondNo = Short.Parse(mDATA(i)) : i = i + 1     ' カット位置補正加工条件番号(FL用)

                ' [PLATE05]データをプレートデータ構造体へ格納する
                strSECT = "[PLATE05]"
                .intMaxTrimNgCount = Short.Parse(mDATA(i)) : i = i + 1          ' ﾄﾘﾐﾝｸﾞNGｶｳﾝﾀ(上限)
                .intMaxBreakDischargeCount = Short.Parse(mDATA(i)) : i = i + 1  ' 割れ欠け排出ｶｳﾝﾀ(上限)
                .intTrimNgCount = Short.Parse(mDATA(i)) : i = i + 1             ' 連続ﾄﾘﾐﾝｸﾞNG枚数
                If (SL432HW_FileVer >= 10.02) Then
                    .intContHiNgResCnt = Short.Parse(mDATA(i)) : i = i + 1      ' 連続ﾄﾘﾐﾝｸﾞNG抵抗数    ###230
                Else
                    .intContHiNgResCnt = 0
                End If
                .intRetryProbeCount = Short.Parse(mDATA(i)) : i = i + 1         ' 再ﾌﾟﾛｰﾋﾞﾝｸﾞ回数
                .dblRetryProbeDistance = Double.Parse(mDATA(i)) : i = i + 1     ' 再ﾌﾟﾛｰﾋﾞﾝｸﾞ移動量
                .intWorkSetByLoader = Short.Parse(mDATA(i)) : i = i + 1         ' 基板品種
                .intOpenCheck = Short.Parse(mDATA(i)) : i = i + 1               ' 4端子ｵｰﾌﾟﾝﾁｪｯｸ
                .intLedCtrl = Short.Parse(mDATA(i)) : i = i + 1                 ' LED制御
                '                                                               ' 自動ﾚｰｻﾞﾊﾟﾜｰ調整項目
                '----- V4.0.0.0-28 ↓ 'V4.4.0.0④-----
                If (giMachineKd = MACHINE_KD_RS) Then
                    If (OSCILLATOR_FL = gSysPrm.stRAT.giOsc_Res) Then
                        .intPowerAdjustMode = Short.Parse(mDATA(i)) : i = i + 1     ' ﾊﾟﾜｰ調整ﾓｰﾄﾞ
                    Else
                        ' FLではない場合、自動調整をしないに設定する
                        .intPowerAdjustMode = 0 : i = i + 1                         ' ﾊﾟﾜｰ調整ﾓｰﾄﾞ
                    End If
                Else
                    .intPowerAdjustMode = Short.Parse(mDATA(i)) : i = i + 1     ' ﾊﾟﾜｰ調整ﾓｰﾄﾞ
                End If
                '----- V4.0.0.0-28 ↑ 'V4.4.0.0④-----
                .dblPowerAdjustTarget = Double.Parse(mDATA(i)) : i = i + 1      ' 調整目標ﾊﾟﾜｰ
                .dblPowerAdjustQRate = Double.Parse(mDATA(i)) : i = i + 1       ' ﾊﾟﾜｰ調整Qﾚｰﾄ
                .dblPowerAdjustToleLevel = Double.Parse(mDATA(i)) : i = i + 1   ' ﾊﾟﾜｰ調整許容範囲
                .intPowerAdjustCondNo = Short.Parse(mDATA(i)) : i = i + 1       ' ﾊﾟﾜｰ調整加工条件番号(FL用)　
                '                                                               ' GP-IB制御
                .intGpibCtrl = Short.Parse(mDATA(i)) : i = i + 1                ' GP-IB制御
                .intGpibDefDelimiter = Short.Parse(mDATA(i)) : i = i + 1        ' 初期設定(ﾃﾞﾘﾐﾀ)(固定)
                .intGpibDefTimiout = Short.Parse(mDATA(i)) : i = i + 1          ' 初期設定(ﾀｲﾑｱｳﾄ) (固定)
                .intGpibDefAdder = Short.Parse(mDATA(i)) : i = i + 1            ' 初期設定(機器ｱﾄﾞﾚｽ)
                .strGpibInitCmnd1 = mDATA(i) : i = i + 1                        ' 初期化ｺﾏﾝﾄﾞ1
                .strGpibInitCmnd2 = mDATA(i) : i = i + 1                        ' 初期化ｺﾏﾝﾄﾞ2
                .strGpibTriggerCmnd = mDATA(i) : i = i + 1                      ' ﾄﾘｶﾞｺﾏﾝﾄﾞ (固定)
                .intGpibMeasSpeed = Short.Parse(mDATA(i)) : i = i + 1           ' 測定速度(0:低速, 1:高速)
                .intGpibMeasMode = Short.Parse(mDATA(i)) : i = i + 1            ' 測定モード(0:絶対, 1:偏差)
                '----- V1.13.0.0②↓ -----
                If (SL432HW_FileVer >= 10.04) Then
                    .intNgJudgeStop = Short.Parse(mDATA(i)) : i = i + 1         ' NG判定時停止
                Else
                    .intNgJudgeStop = 0                                         ' NG判定時停止(初期値)
                End If

                '----- V4.11.0.0①↓ (WALSIN殿SL436S対応) -----
                If (SL432HW_FileVer >= 10.11) Then                              ' ファイルバージョンが10.11以上の時に設定する
                    .intPwrChkPltNum = Short.Parse(mDATA(i)) : i = i + 1        ' オートパワーチェック基板枚数
                    .intPwrChkTime = Short.Parse(mDATA(i)) : i = i + 1          ' オートパワーチェック時間(分) 
                Else
                    .intPwrChkPltNum = 0                                        ' オートパワーチェック基板枚数
                    .intPwrChkTime = 0                                          ' オートパワーチェック時間(分) 
                End If
                '----- V4.11.0.0①↑ -----

                ' [PLATE06]データをプレートデータ構造体へ格納する
                strSECT = "[PLATE06]"
                If (SL432HW_FileVer >= 10.04) Then
                    .intContExpMode = Short.Parse(mDATA(i)) : i = i + 1         ' 伸縮補正 (0:なし, 1:あり)
                    .intContExpGrpNo = Short.Parse(mDATA(i)) : i = i + 1        ' 伸縮補正ｸﾞﾙｰﾌﾟ番号
                    .intContExpPtnNo = Short.Parse(mDATA(i)) : i = i + 1        ' 伸縮補正ﾊﾟﾀｰﾝ番号
                    .dblContExpPosX = Double.Parse(mDATA(i)) : i = i + 1        ' 伸縮補正位置X (mm)
                    .dblContExpPosY = Double.Parse(mDATA(i)) : i = i + 1        ' 伸縮補正位置XY (mm)            
                    .intStepMeasCnt = Short.Parse(mDATA(i)) : i = i + 1         ' ｽﾃｯﾌﾟ測定回数
                    .dblStepMeasPitch = Double.Parse(mDATA(i)) : i = i + 1      ' ｽﾃｯﾌﾟ測定ﾋﾟｯﾁ
                    .intStepMeasReptCnt = Short.Parse(mDATA(i)) : i = i + 1     ' ｽﾃｯﾌﾟ測定繰り返しｽﾃｯﾌﾟ回数
                    .dblStepMeasReptPitch = Double.Parse(mDATA(i)) : i = i + 1  ' ｽﾃｯﾌﾟ測定繰り返しｽﾃｯﾌﾟﾋﾟｯﾁ
                    .intStepMeasLwGrpNo = Short.Parse(mDATA(i)) : i = i + 1     ' ｽﾃｯﾌﾟ測定下方ﾌﾟﾛｰﾌﾞｸﾞﾙｰﾌﾟ番号
                    .intStepMeasLwPtnNo = Short.Parse(mDATA(i)) : i = i + 1     ' ｽﾃｯﾌﾟ測定下方ﾌﾟﾛｰﾌﾞﾊﾟﾀｰﾝ番号
                    .dblStepMeasBpPosX = Double.Parse(mDATA(i)) : i = i + 1     ' ｽﾃｯﾌﾟ測定BP位置X
                    .dblStepMeasBpPosY = Double.Parse(mDATA(i)) : i = i + 1     ' ｽﾃｯﾌﾟ測定BP位置Y
                    .intStepMeasUpGrpNo = Short.Parse(mDATA(i)) : i = i + 1     ' ｽﾃｯﾌﾟ測定基板上ﾌﾟﾛｰﾌﾞｸﾞﾙｰﾌﾟ番号
                    .intStepMeasUpPtnNo = Short.Parse(mDATA(i)) : i = i + 1     ' ｽﾃｯﾌﾟ測定基板上ﾌﾟﾛｰﾌﾞﾊﾟﾀｰﾝ番号
                    .dblStepMeasTblOstX = Double.Parse(mDATA(i)) : i = i + 1    ' ｽﾃｯﾌﾟ測定基板上ﾌﾟﾛｰﾌﾞﾃｰﾌﾞﾙｵﾌｾｯﾄX
                    .dblStepMeasTblOstY = Double.Parse(mDATA(i)) : i = i + 1    ' ｽﾃｯﾌﾟ測定基板上ﾌﾟﾛｰﾌﾞﾃｰﾌﾞﾙｵﾌｾｯﾄY
                    .intIDReaderUse = Short.Parse(mDATA(i)) : i = i + 1         ' IDﾘｰﾄﾞ (0:未使用, 1:使用)
                    .dblIDReadPos1X = Double.Parse(mDATA(i)) : i = i + 1        ' IDﾘｰﾄﾞ読み取りﾎﾟｼﾞｼｮﾝ 1X
                    .dblIDReadPos1Y = Double.Parse(mDATA(i)) : i = i + 1        ' IDﾘｰﾄﾞ読み取りﾎﾟｼﾞｼｮﾝ 1Y
                    .dblIDReadPos2X = Double.Parse(mDATA(i)) : i = i + 1        ' IDﾘｰﾄﾞ読み取りﾎﾟｼﾞｼｮﾝ 2X
                    .dblIDReadPos2Y = Double.Parse(mDATA(i)) : i = i + 1        ' IDﾘｰﾄﾞ読み取りﾎﾟｼﾞｼｮﾝ 2Y
                    .dblReprobeVar = Double.Parse(mDATA(i)) : i = i + 1         ' 再ﾌﾟﾛｰﾋﾞﾝｸﾞばらつき量
                    .dblReprobePitch = Double.Parse(mDATA(i)) : i = i + 1       ' 再ﾌﾟﾛｰﾋﾞﾝｸﾞﾋﾟｯﾁ
                Else
                    .intContExpMode = 0                                         ' 伸縮補正 (0:なし, 1:あり)
                    .intContExpGrpNo = 5                                        ' 伸縮補正ｸﾞﾙｰﾌﾟ番号
                    .intContExpPtnNo = 1                                        ' 伸縮補正ﾊﾟﾀｰﾝ番号
                    .dblContExpPosX = 0.0                                       ' 伸縮補正位置X (mm)
                    .dblContExpPosY = 0.0                                       ' 伸縮補正位置XY (mm)            
                    .intStepMeasCnt = 0                                         ' ｽﾃｯﾌﾟ測定回数
                    .dblStepMeasPitch = 0.0                                     ' ｽﾃｯﾌﾟ測定ﾋﾟｯﾁ
                    .intStepMeasReptCnt = 0                                     ' ｽﾃｯﾌﾟ測定繰り返しｽﾃｯﾌﾟ回数
                    .dblStepMeasReptPitch = 0.0                                 ' ｽﾃｯﾌﾟ測定繰り返しｽﾃｯﾌﾟﾋﾟｯﾁ
                    .intStepMeasLwGrpNo = 6                                     ' ｽﾃｯﾌﾟ測定下方ﾌﾟﾛｰﾌﾞｸﾞﾙｰﾌﾟ番号
                    .intStepMeasLwPtnNo = 1                                     ' ｽﾃｯﾌﾟ測定下方ﾌﾟﾛｰﾌﾞﾊﾟﾀｰﾝ番号
                    .dblStepMeasBpPosX = 0.0                                    ' ｽﾃｯﾌﾟ測定BP位置X
                    .dblStepMeasBpPosY = 0.0                                    ' ｽﾃｯﾌﾟ測定BP位置Y
                    .intStepMeasUpGrpNo = 6                                     ' ｽﾃｯﾌﾟ測定基板上ﾌﾟﾛｰﾌﾞｸﾞﾙｰﾌﾟ番号
                    .intStepMeasUpPtnNo = 2                                     ' ｽﾃｯﾌﾟ測定基板上ﾌﾟﾛｰﾌﾞﾊﾟﾀｰﾝ番号
                    .dblStepMeasTblOstX = 0.0                                   ' ｽﾃｯﾌﾟ測定基板上ﾌﾟﾛｰﾌﾞﾃｰﾌﾞﾙｵﾌｾｯﾄX
                    .dblStepMeasTblOstY = 0.0                                   ' ｽﾃｯﾌﾟ測定基板上ﾌﾟﾛｰﾌﾞﾃｰﾌﾞﾙｵﾌｾｯﾄY
                    .intIDReaderUse = 0                                         ' IDﾘｰﾄﾞ (0:未使用, 1:使用)
                    .dblIDReadPos1X = 0.0                                       ' IDﾘｰﾄﾞ読み取りﾎﾟｼﾞｼｮﾝ 1X
                    .dblIDReadPos1Y = 0.0                                       ' IDﾘｰﾄﾞ読み取りﾎﾟｼﾞｼｮﾝ 1Y
                    .dblIDReadPos2X = 0.0                                       ' IDﾘｰﾄﾞ読み取りﾎﾟｼﾞｼｮﾝ 2X
                    .dblIDReadPos2Y = 0.0                                       ' IDﾘｰﾄﾞ読み取りﾎﾟｼﾞｼｮﾝ 2Y
                    .dblReprobeVar = 0.0                                        ' 再ﾌﾟﾛｰﾋﾞﾝｸﾞばらつき量
                    .dblReprobePitch = 0.0                                      ' 再ﾌﾟﾛｰﾋﾞﾝｸﾞﾋﾟｯﾁ
                End If
                '----- V1.13.0.0②↑ -----
                '----- V2.0.0.0_24↓ -----
                '----- プローブクリーニング項目 -----
                If (SL432HW_FileVer >= FILE_VER_10_10) Then
                    .dblPrbCleanPosX = Double.Parse(mDATA(i)) : i = i + 1       ' クリーニング位置X
                    .dblPrbCleanPosY = Double.Parse(mDATA(i)) : i = i + 1       ' クリーニング位置Y
                    .dblPrbCleanPosZ = Double.Parse(mDATA(i)) : i = i + 1       ' クリーニング位置Z
                    .intPrbCleanUpDwCount = Short.Parse(mDATA(i)) : i = i + 1   ' プローブ上下回数
                    .intPrbCleanAutoSubCount = Short.Parse(mDATA(i)) : i = i + 1 '  自動運転時クリーニング実行基板枚数
                Else
                    .dblPrbCleanPosX = 0.0                                      ' クリーニング位置X
                    .dblPrbCleanPosY = 0.0                                      ' クリーニング位置Y
                    .dblPrbCleanPosZ = 0.0                                      ' クリーニング位置Z
                    .intPrbCleanUpDwCount = 0                                   ' プローブ上下回数
                    .intPrbCleanAutoSubCount = 0                                '  自動運転時クリーニング実行基板枚数
                End If
                '----- V2.0.0.0_24↑ -----

                'V5.0.0.6①↓
                ' [OPTIN]データをプレートデータ構造体へ格納する
                strSECT = "[OPTION]"
                If gbControllerInterlock Then
                    If Integer.TryParse(mDATA(i), .intControllerInterlock) Then
                    Else
                        .intControllerInterlock = 0
                    End If
                End If
                i = i + 1
                'V5.0.0.6①↑

                'V4.5.1.0⑮              ↓
                If (FILE_VER_10_11 <= SL432HW_FileVer) Then
                    .dblTXChipsizeRelationX = If(String.IsNullOrEmpty(mDATA(i)), 0.0, Double.Parse(mDATA(i))) : i = i + 1 ' 補正位置１と２の相対値Ｘ
                    .dblTXChipsizeRelationY = If(String.IsNullOrEmpty(mDATA(i)), 0.0, Double.Parse(mDATA(i))) : i = i + 1 ' 補正位置１と２の相対値Ｙ
                Else
                    .dblTXChipsizeRelationX = 0.0                               ' 補正位置１と２の相対値Ｘ
                    .dblTXChipsizeRelationY = 0.0                               ' 補正位置１と２の相対値Ｙ
                End If
                'V4.5.1.0⑮              ↑

                'V4.10.0.0④            ↓
                'If (FILE_VER_10_11 <= SL432HW_FileVer) Then
                '    .dblPrbCleanStagePitchX = Double.Parse(mDATA(i)) : i += 1   ' ステージ動作ピッチX
                '    .dblPrbCleanStagePitchY = Double.Parse(mDATA(i)) : i += 1   ' ステージ動作ピッチY
                '    .intPrbCleanStageCountX = Short.Parse(mDATA(i)) : i += 1    ' ステージ動作回数X
                '    .intPrbCleanStageCountY = Short.Parse(mDATA(i)) : i += 1    ' ステージ動作回数Y
                '    .dblPrbDistance = Double.Parse(mDATA(i)) : i += 1           ' プローブ間距離（mm）'V4.10.0.0⑨
                '    .dblPrbCleaningOffset = Double.Parse(mDATA(i)) : i += 1     ' クリーニングオフセット(mm)'V4.10.0.0⑨
                'Else
                '    .dblPrbCleanStagePitchX = 0.0                               ' ステージ動作ピッチX
                '    .dblPrbCleanStagePitchY = 0.0                               ' ステージ動作ピッチY
                '    .intPrbCleanStageCountX = 0                                 ' ステージ動作回数X
                '    .intPrbCleanStageCountY = 0                                 ' ステージ動作回数Y
                '    .dblPrbDistance = 0.0                                       ' プローブ間距離（mm）'V4.10.0.0⑨
                '    .dblPrbCleaningOffset = 0.0                                 ' クリーニングオフセット(mm)'V4.10.0.0⑨
                'End If
                'V4.10.0.0④            ↑

            End With
            Return (cFRS_NORMAL)                                                ' Return値 = 正常

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "File.Set_typPlateInfo() TRAP ERROR = " + ex.Message
            'MsgBox(strMSG)                                                     ' Call元でメッセージ表示するので削除
            Return (cERR_TRAP)                                                  ' Return値 = 例外エラー
        End Try
    End Function
#End Region

#Region "ロードしたステップデータをステップデータ構造体へ格納する【TKY,CHIP,NET統合版】"
    '''=========================================================================
    '''<summary>ロードしたステップデータをステップデータ構造体へ格納する</summary>
    '''<param name="pBuff">(INP)データ</param>
    '''<param name="rCnt"> (I/O)ステップデータ構造体のインデックス(1ｵﾘｼﾞﾝ)</param>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function Set_typStepInfo(ByVal pBuff As String, ByRef rCnt As Integer) As Integer

        Dim mDATA() As String
        Dim i As Integer
        Dim strMSG As String

        Try
            ' ステップデータ構造体へデータを格納する
            With typStepInfoArray(rCnt)
                i = 0
                mDATA = pBuff.Split(",")                            ' 文字列を','で分割して取出す 
                .intSP1 = Short.Parse(mDATA(i)) : i = i + 1         ' ｽﾃｯﾌﾟ番号
                .intSP2 = Short.Parse(mDATA(i)) : i = i + 1         ' ﾌﾞﾛｯｸ数
                .dblSP3 = Double.Parse(mDATA(i)) : i = i + 1        ' ｽﾃｯﾌﾟ間ｲﾝﾀｰﾊﾞﾙ
            End With
            MaxStep = rCnt                                          ' ステップデータ件数
            rCnt = rCnt + 1

            Return (cFRS_NORMAL)                                    ' Return値 = 正常

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "File.Set_typStepInfo() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                      ' Return値 = 例外エラー
        End Try
    End Function
#End Region

#Region "ロードしたグループデータをグループデータ構造体へ格納する【TKY,CHIP,NET統合版】"
    '''=========================================================================
    '''<summary>ロードしたグループデータをグループデータ構造体へ格納する</summary>
    '''<param name="pBuff">(INP)データ</param>
    '''<param name="rCnt"> (I/O)グループデータ構造体のインデックス(1ｵﾘｼﾞﾝ)</param>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function Set_typGrpInfoArrayChip(ByVal pBuff As String, ByRef rCnt As Integer) As Integer

        Dim mDATA() As String
        Dim i As Integer
        Dim strMSG As String

        Try
            ' グループデータ構造体へデータを格納する
            With typGrpInfoArray(rCnt)
                i = 0
                mDATA = pBuff.Split(",")                            ' 文字列を','で分割して取出す 
                .intGP1 = Short.Parse(mDATA(i)) : i = i + 1         ' ｸﾞﾙｰﾌﾟ番号
                .intGP2 = Short.Parse(mDATA(i)) : i = i + 1         ' 抵抗数
                .dblGP3 = Double.Parse(mDATA(i)) : i = i + 1        ' ｸﾞﾙｰﾌﾟ間ｲﾝﾀｰﾊﾞﾙ
            End With
            MaxGrp = rCnt                                           ' ｸﾞﾙｰﾌﾟ件数
            rCnt = rCnt + 1

            Return (cFRS_NORMAL)                                    ' Return値 = 正常

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "File.Set_typGrpInfoArrayChip() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                      ' Return値 = 例外エラー
        End Try
    End Function
#End Region

#Region "ロードしたTY2データをTY2データ構造体へ格納する【TKY,CHIP,NET統合版】"
    '''=========================================================================
    '''<summary>ロードしたTY2データをTY2データ構造体へ格納する</summary>
    '''<param name="pBuff">(INP)データ</param>
    '''<param name="rCnt"> (I/O)TY2データ構造体のインデックス(1ｵﾘｼﾞﾝ)</param>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function Set_typTy2InfoArray(ByVal pBuff As String, ByRef rCnt As Integer) As Integer

        Dim mDATA() As String
        Dim i As Integer
        Dim strMSG As String

        Try
            ' TY2データ構造体へデータを格納する
            With typTy2InfoArray(rCnt)
                i = 0
                mDATA = pBuff.Split(",")                            ' 文字列を','で分割して取出す 
                .intTy21 = Short.Parse(mDATA(i)) : i = i + 1        ' ﾌﾞﾛｯｸ番号
                .dblTy22 = Double.Parse(mDATA(i)) : i = i + 1       ' ｽﾃｯﾌﾟｲﾝﾀｰﾊﾞﾙ
            End With
            If (typTy2InfoArray(rCnt).intTy21 <> 0) Then
                MaxTy2 = rCnt                                       ' Ty2ﾌﾞﾛｯｸ件数
            End If
            rCnt = rCnt + 1

            Return (cFRS_NORMAL)                                    ' Return値 = 正常

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "File.Set_typTy2InfoArray() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                      ' Return値 = 例外エラー
        End Try
    End Function
#End Region

#Region "ロードしたサーキット座標データ(NET用)をサーキット座標データ構造体へ格納する【TKY,CHIP,NET統合版】"
    '''=========================================================================
    '''<summary>ロードしたサーキット座標データ(NET用)をサーキット座標データ構造体へ格納する</summary>
    '''<param name="pBuff">(INP)データ</param>
    '''<param name="rCnt"> (I/O)サーキット座標データのインデックス(1ｵﾘｼﾞﾝ)</param>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function Set_typCirAxisInfoArray(ByVal pBuff As String, ByRef rCnt As Integer) As Integer

        Dim mDATA() As String
        Dim i As Integer
        Dim strMSG As String

        Try
            ' サーキット座標データ構造体へデータを格納する
            With typCirAxisInfoArray(rCnt)
                i = 0
                mDATA = pBuff.Split(",")                            ' 文字列を','で分割して取出す 
                .intCaP1 = Short.Parse(mDATA(i)) : i = i + 1        ' ｽﾃｯﾌﾟ番号
                .dblCaP2 = Double.Parse(mDATA(i)) : i = i + 1       ' 座標X
                .dblCaP3 = Double.Parse(mDATA(i)) : i = i + 1       ' 座標Y
            End With
            rCnt = rCnt + 1                                         ' インデックス更新

            Return (cFRS_NORMAL)                                    ' Return値 = 正常

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "File.Set_typCirAxisInfoArray() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                      ' Return値 = 例外エラー
        End Try
    End Function
#End Region

#Region "ロードしたサーキット間インターバルデータ(NET用)をサーキット間インターバルデータ構造体へ格納する【TKY,CHIP,NET統合版】"
    '''=========================================================================
    '''<summary>ロードしたサーキット間インターバルデータ(NET用)をサーキット間インターバルデータ構造体へ格納する</summary>
    '''<param name="pBuff">(INP)データ</param>
    '''<param name="rCnt"> (I/O)サーキット座標データのインデックス(1ｵﾘｼﾞﾝ)</param>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function Set_typCirInInfoArray(ByVal pBuff As String, ByRef rCnt As Integer) As Integer

        Dim mDATA() As String
        Dim i As Integer
        Dim strMSG As String

        Try
            ' サーキット間インターバルデータ構造体へデータを格納する
            With typCirInInfoArray(rCnt)
                i = 0
                mDATA = pBuff.Split(",")                            ' 文字列を','で分割して取出す 
                .intCiP1 = Short.Parse(mDATA(i)) : i = i + 1        ' ｽﾃｯﾌﾟ番号
                .intCiP2 = Short.Parse(mDATA(i)) : i = i + 1        ' ｻｰｷｯﾄ数
                .dblCiP3 = Double.Parse(mDATA(i)) : i = i + 1       ' ｻｰｷｯﾄ間ｲﾝﾀｰﾊﾞﾙ
            End With
            rCnt = rCnt + 1                                         ' インデックス更新

            Return (cFRS_NORMAL)                                    ' Return値 = 正常

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "File.Set_typCirInInfoArray() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                      ' Return値 = 例外エラー
        End Try
    End Function
#End Region

#Region "ロードした抵抗データを抵抗データ構造体へ格納する【TKY,CHIP,NET統合版】"
    '''=========================================================================
    '''<summary>ロードした抵抗データを抵抗データ構造体構造体へ格納する</summary>
    '''<param name="pBuff">(INP)データ</param>
    '''<param name="rCnt"> (I/O)抵抗データ構造体のインデックス(1ｵﾘｼﾞﾝ)</param>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function Get_RESIST_Data(ByVal pBuff As String, ByRef rCnt As Integer) As Integer

        Dim mDATA() As String
        Dim i As Integer
        Dim strMSG As String

        Try
            ' ロードした抵抗データを抵抗データ構造体へ格納する
            With typResistorInfoArray(rCnt)
                i = 0
                mDATA = pBuff.Split(",")                                           ' 文字列を','で分割して取出す 
                .intResNo = Short.Parse(mDATA(i)) : i = i + 1                      ' 抵抗番号
                .intResMeasMode = Short.Parse(mDATA(i)) : i = i + 1                ' 測定モード(0:抵抗 ,1:電圧 ,2:外部) 
                .intResMeasType = UShort.Parse(mDATA(i)) : i = i + 1               ' 測定タイプ(0:高速 ,1:高精度)　※追加
                .intCircuitGrp = Short.Parse(mDATA(i)) : i = i + 1                 ' 所属ｻｰｷｯﾄ番号
                .intProbHiNo = Short.Parse(mDATA(i)) : i = i + 1                   ' プローブ番号（ハイ側）
                .intProbLoNo = Short.Parse(mDATA(i)) : i = i + 1                   ' プローブ番号（ロー側）
                .intProbAGNo1 = Short.Parse(mDATA(i)) : i = i + 1                  ' プローブ番号（１）
                .intProbAGNo2 = Short.Parse(mDATA(i)) : i = i + 1                  ' プローブ番号（２）
                .intProbAGNo3 = Short.Parse(mDATA(i)) : i = i + 1                  ' プローブ番号（３）
                .intProbAGNo4 = Short.Parse(mDATA(i)) : i = i + 1                  ' プローブ番号（４）
                .intProbAGNo5 = Short.Parse(mDATA(i)) : i = i + 1                  ' プローブ番号（５）
                .strExternalBits = mDATA(i) : i = i + 1                            ' EXTERNAL BITS
                .intPauseTime = Short.Parse(mDATA(i)) : i = i + 1                  ' ポーズタイム
                .intTargetValType = Short.Parse(mDATA(i)) : i = i + 1              ' 目標値指定（0:絶対値,1:レシオ,2:計算式）
                .intBaseResNo = Short.Parse(mDATA(i)) : i = i + 1                  ' ベ－ス抵抗番号
                .dblTrimTargetVal = Double.Parse(mDATA(i)) : i = i + 1             ' トリミング目標値
                .dblProbCfmPoint_Hi_X = Double.Parse(mDATA(i)) : i = i + 1         ' プローブ確認位置 HI X座標
                .dblProbCfmPoint_Hi_Y = Double.Parse(mDATA(i)) : i = i + 1         ' プローブ確認位置 HI Y座標
                .dblProbCfmPoint_Lo_X = Double.Parse(mDATA(i)) : i = i + 1         ' プローブ確認位置 LO X座標
                .dblProbCfmPoint_Lo_Y = Double.Parse(mDATA(i)) : i = i + 1         ' プローブ確認位置 LO Y座標
                .intSlope = Short.Parse(mDATA(i)) : i = i + 1                      ' 電圧変化 ｽﾛｰﾌﾟ
                .dblInitTest_HighLimit = Double.Parse(mDATA(i)) : i = i + 1        ' イニシャルテストHIGHリミット
                .dblInitTest_LowLimit = Double.Parse(mDATA(i)) : i = i + 1         ' イニシャルテストLOWリミット
                .dblFinalTest_HighLimit = Double.Parse(mDATA(i)) : i = i + 1       ' ファイナルテストHIGHリミット
                .dblFinalTest_LowLimit = Double.Parse(mDATA(i)) : i = i + 1        ' ファイナルテストLOWリミット
                .intInitialOkTestDo = Short.Parse(mDATA(i)) : i = i + 1            ' ｲﾆｼｬﾙOKﾃｽﾄ(0:しない,1:する)※追加(プレートデータから移動)
                .intCutReviseMode = Short.Parse(mDATA(i)) : i = i + 1              ' ｶｯﾄ 補正
                .intCutReviseDispMode = Short.Parse(mDATA(i)) : i = i + 1          ' 表示ﾓｰﾄﾞ
                .intCutRevisePtnNo = Short.Parse(mDATA(i)) : i = i + 1             ' ﾊﾟﾀｰﾝ No.
                .intCutReviseGrpNo = Short.Parse(mDATA(i)) : i = i + 1             ' ﾊﾟﾀｰﾝｸﾞﾙｰﾌﾟ番号  
                .dblCutRevisePosX = Double.Parse(mDATA(i)) : i = i + 1             ' ｶｯﾄ補正位置X
                .dblCutRevisePosY = Double.Parse(mDATA(i)) : i = i + 1             ' ｶｯﾄ補正位置Y
                .intIsNG = Short.Parse(mDATA(i)) : i = i + 1                       ' 画像認識NG判定(0:あり, 1:なし, 手動)
                .intCutCount = Short.Parse(mDATA(i)) : i = i + 1                   ' カット数
                .strRatioTrimTargetVal = mDATA(i) : i = i + 1                      ' レシオ計算式 V1.13.0.0②

                '----- V1.13.0.0②↓ -----
                If (SL432HW_FileVer >= 10.04) Then
                    .intCvMeasNum = Short.Parse(mDATA(i)) : i = i + 1               ' CV 最大測定回数
                    .intCvMeasTime = Short.Parse(mDATA(i)) : i = i + 1              ' CV 最大測定時間(ms) 
                    .dblCvValue = Double.Parse(mDATA(i)) : i = i + 1                ' CV CV値         
                    .intOverloadNum = Short.Parse(mDATA(i)) : i = i + 1             ' ｵｰﾊﾞｰﾛｰﾄﾞ 回数 
                    .dblOverloadMin = Double.Parse(mDATA(i)) : i = i + 1            ' ｵｰﾊﾞｰﾛｰﾄﾞ 下限値 
                    .dblOverloadMax = Double.Parse(mDATA(i)) : i = i + 1            ' ｵｰﾊﾞｰﾛｰﾄﾞ 上限値
                Else
                    .intCvMeasNum = 0                                               ' CV 最大測定回数
                    .intCvMeasTime = 0                                              ' CV 最大測定時間(ms) 
                    .dblCvValue = 0.0                                               ' CV CV値         
                    .intOverloadNum = 0                                             ' ｵｰﾊﾞｰﾛｰﾄﾞ 回数 
                    .dblOverloadMin = 0.0                                           ' ｵｰﾊﾞｰﾛｰﾄﾞ 下限値 
                    .dblOverloadMax = 0.0                                           ' ｵｰﾊﾞｰﾛｰﾄﾞ 上限値
                End If
                '----- V1.13.0.0②↑ -----

                '----- V2.0.0.0_23↓ -----
                ' ファイルバージョンが10.072/10.073(ローム)又は10.10以上の時に設定する V4.0.0.0-33
                If ((SL432HW_FileVer = 10.072) Or (SL432HW_FileVer = 10.073) Or (SL432HW_FileVer >= 10.1)) Then
                    .wPauseTimeFT = Short.Parse(mDATA(i)) : i = i + 1               '  FT前のポーズタイム(0-32767msec) 
                Else
                    .wPauseTimeFT = 0                                               '  FT前のポーズタイム(0msec) 
                End If

                ' ファイルバージョンが10.10以上の時に設定する
                If (10.1 <= SL432HW_FileVer) Then
                    .intInsideEndChkCount = Short.Parse(mDATA(i)) : i = i + 1       ' 中切り判定回数
                    .dblInsideEndChgRate = Double.Parse(mDATA(i)) : i = i + 1       ' 中切り判定変化率(0.00-100.00%)
                Else
                    .intInsideEndChkCount = 0                                       ' 中切り判定回数
                    .dblInsideEndChgRate = 0.0                                      ' 中切り判定変化率(0.00-100.00%)
                End If
                '----- V2.0.0.0_23↑ -----

                '----- V4.11.0.0①↓ (WALSIN殿SL436S対応) -----
                If (10.11 <= SL432HW_FileVer) Then                                  ' ファイルバージョンが10.11以上の時に設定する
                    'V5.0.0.2①↓
                    '                    .dblTrimTargetOfs = Double.Parse(mDATA(i)) : i = i + 1          ' ﾄﾘﾐﾝｸﾞ目標値ｵﾌｾｯﾄ
                    .dblTrimTargetOfs_Save = Double.Parse(mDATA(i)) : i = i + 1          ' ﾄﾘﾐﾝｸﾞ目標値ｵﾌｾｯﾄ
                    .dblTrimTargetOfs = .dblTrimTargetVal * (.dblTrimTargetOfs_Save / 100)
                    'V5.0.0.2①↑
                Else
                    .dblTrimTargetOfs = 0.0                                         ' ﾄﾘﾐﾝｸﾞ目標値ｵﾌｾｯﾄ
                    .dblTrimTargetOfs_Save = 0.0                                     'V5.0.0.2①
                End If
                ' 目標値オフセット有効ならﾄﾘﾐﾝｸﾞ目標値をﾄﾘﾐﾝｸﾞ目標値+ｵﾌｾｯﾄ(指定がある場合)とする
                .dblTrimTargetVal_Save = .dblTrimTargetVal                          ' ﾄﾘﾐﾝｸﾞ目標値を退避
                If (giTargetOfs = 1) And (.dblTrimTargetOfs <> 0.0) Then            ' 目標値オフセット有効 ? 
                    .dblTrimTargetVal = .dblTrimTargetVal + .dblTrimTargetOfs       ' ﾄﾘﾐﾝｸﾞ目標値 = 目標値 + ｵﾌｾｯﾄ
                End If
                '----- V4.11.0.0①↑ -----
                typPlateInfo.intResistCntInBlock = rCnt                            ' 1ブロック内抵抗数をセット
                gRegistorCnt = rCnt

            End With
            rCnt = rCnt + 1                                                         ' インデックス更新

            Return (cFRS_NORMAL)                                                    ' Return値 = 正常

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "File.Get_RESIST_Data() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                                      ' Return値 = 例外エラー
        End Try
    End Function
#End Region

#Region "ロードしたカットデータをカットデータ構造体へ格納する【TKY,CHIP,NET統合版】"
    '''=========================================================================
    '''<summary>ロードしたカットデータをカットデータ構造体へ格納する</summary>
    '''<param name="pBuff">(INP)データ</param>
    '''<param name="rCnt"> (I/O)抵抗データ構造体のインデックス(1ｵﾘｼﾞﾝ)</param>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function Get_CUT_Data(ByVal pBuff As String, ByRef rCnt As Integer) As Integer

        Dim mDATA() As String
        Dim i As Integer
        Dim j As Integer
        Dim Rn As Integer
        Dim Cn As Integer
        Dim strMSG As String

        Try
            ' 抵抗データインデックスを求める 
            mDATA = pBuff.Split(",")                                ' 文字列を','で分割して取出す 
            Cn = Integer.Parse(mDATA(1))                            ' カット番号
            If (Cn = 1) Then                                        ' カット番号=１の場合は抵抗データインデックスをカウントアップ 
                rCnt = rCnt + 1
            End If
            ' ロードしたカットデータをカットデータ構造体へ格納する
            Rn = rCnt                                                               ' Rn = 抵抗データインデックス
            i = 1                                                                   ' i  = ロードしたカットデータ配列のインデックス
            With typResistorInfoArray(rCnt)
                .ArrCut(Cn).intCutNo = Short.Parse(mDATA(i)) : i = i + 1            ' カット番号
                .ArrCut(Cn).intDelayTime = Short.Parse(mDATA(i)) : i = i + 1        ' ディレイタイム
                .ArrCut(Cn).dblTeachPointX = Double.Parse(mDATA(i)) : i = i + 1     ' ﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄX
                .ArrCut(Cn).dblTeachPointY = Double.Parse(mDATA(i)) : i = i + 1     ' ﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄY
                .ArrCut(Cn).dblStartPointX = Double.Parse(mDATA(i)) : i = i + 1     ' スタートポイントX
                .ArrCut(Cn).dblStartPointY = Double.Parse(mDATA(i)) : i = i + 1     ' スタートポイントY
                .ArrCut(Cn).dblCutSpeed = Double.Parse(mDATA(i)) : i = i + 1        ' カットスピード
                '----- V4.0.0.0-28 ↓ -----
                If (FILE_VER_10_10 <= SL432HW_FileVer) Then
                    .ArrCut(Cn).dblQRate = Double.Parse(mDATA(i)) : i = i + 1       ' Ｑスイッチレート
                Else
                    .ArrCut(Cn).dblQRate = 0.1 : i = i + 1                          ' Ｑスイッチレート
                End If
                '----- V4.0.0.0-28 ↑ -----
                .ArrCut(Cn).dblCutOff = Double.Parse(mDATA(i)) : i = i + 1          ' カットオフ値
                .ArrCut(Cn).dblJudgeLevel = Double.Parse(mDATA(i)) : i = i + 1      ' 切替ポイント (旧ﾃﾞｰﾀ判定(平均化率))
                .ArrCut(Cn).dblCutOffOffset = Double.Parse(mDATA(i)) : i = i + 1    ' ｶｯﾄｵﾌ ｵﾌｾｯﾄ
                .ArrCut(Cn).strCutType = mDATA(i).Trim() : i = i + 1                ' カット形状(前後の空白を削除)
                .ArrCut(Cn).intCutDir = Short.Parse(mDATA(i)) : i = i + 1           ' カット方向 
                .ArrCut(Cn).intLTurnDir = Short.Parse(mDATA(i)) : i = i + 1         ' Lﾀｰﾝ方向(1:CW, 2:CCW) ※変更
                '----- V4.0.0.0-57 ↓ -----
                ' Lﾀｰﾝ方向が0の場合は1に変換する(1:CW, 2:CCW)
                ' カット形状がLカット, HOOKカット, Uカットの場合
                If (.ArrCut(Cn).strCutType = CNS_CUTP_L) Or (.ArrCut(Cn).strCutType = CNS_CUTP_NL) Or _
                       (.ArrCut(Cn).strCutType = CNS_CUTP_Lr) Or (.ArrCut(Cn).strCutType = CNS_CUTP_Lt) Or _
                       (.ArrCut(Cn).strCutType = CNS_CUTP_NLr) Or (.ArrCut(Cn).strCutType = CNS_CUTP_NLt) Or _
                       (.ArrCut(Cn).strCutType = CNS_CUTP_HK) Or (.ArrCut(Cn).strCutType = CNS_CUTP_U) Or _
                       (.ArrCut(Cn).strCutType = CNS_CUTP_Ut) Then

                    If (.ArrCut(Cn).intLTurnDir = 0) Then
                        .ArrCut(Cn).intLTurnDir = 1
                    End If
                End If
                '----- V4.0.0.0-57 ↑ -----
                .ArrCut(Cn).dblMaxCutLength = Double.Parse(mDATA(i)) : i = i + 1    ' 最大カッティング長 
                .ArrCut(Cn).dblR1 = Double.Parse(mDATA(i)) : i = i + 1              ' Ｒ１ 
                .ArrCut(Cn).dblLTurnPoint = Double.Parse(mDATA(i)) : i = i + 1      ' Ｌターンポイント 
                .ArrCut(Cn).dblMaxCutLengthL = Double.Parse(mDATA(i)) : i = i + 1   ' Ｌターン後の最大カッティング長 
                .ArrCut(Cn).dblR2 = Double.Parse(mDATA(i)) : i = i + 1              ' Ｒ２ 
                .ArrCut(Cn).dblMaxCutLengthHook = Double.Parse(mDATA(i)) : i = i + 1 ' フックターン後のカッティング長 
                .ArrCut(Cn).intIndexCnt = Short.Parse(mDATA(i)) : i = i + 1         ' インデックス数 

                ' 2011.08.30  
                '.ArrCut(Cn).intMeasMode = Short.Parse(mDATA(i)) : i = i + 1        ' 測定モード 
                .ArrCut(Cn).intMeasType = Short.Parse(mDATA(i)) : i = i + 1         ' 測定タイプ(0:高速 ,1:高精度, 2:外部))※IX用
                .ArrCut(Cn).intMeasMode = typResistorInfoArray(rCnt).intResMeasMode ' 測定モード(0:抵抗 ,1:電圧)※抵抗データから設定
                If (.ArrCut(Cn).intMeasType >= MEASMODE_EXT) Then                   ' 外部なら測定モードを再設定する
                    .ArrCut(Cn).intMeasMode = MEASMODE_EXT                          ' 測定モード(0:抵抗 ,1:電圧, 2:外部)※IX用
                End If

                .ArrCut(Cn).dblCutSpeed2 = Double.Parse(mDATA(i)) : i = i + 1       ' カットスピード２ 
                '----- V4.0.0.0-28 ↓ -----
                If (FILE_VER_10_10 <= SL432HW_FileVer) Then
                    .ArrCut(Cn).dblQRate2 = Double.Parse(mDATA(i)) : i = i + 1      ' Ｑスイッチレート２ 
                Else
                    .ArrCut(Cn).dblQRate2 = 0.1 : i = i + 1                         ' Ｑスイッチレート２ 
                End If
                '----- V4.0.0.0-28 ↑ -----
                .ArrCut(Cn).intCutAngle = Short.Parse(mDATA(i)) : i = i + 1         ' 斜めカットの切り出し角度 
                .ArrCut(Cn).dblPitch = Double.Parse(mDATA(i)) : i = i + 1           ' ピッチ 
                .ArrCut(Cn).intStepDir = Short.Parse(mDATA(i)) : i = i + 1          ' ステップ方向 
                .ArrCut(Cn).intCutCnt = Short.Parse(mDATA(i)) : i = i + 1           ' 本数 
                .ArrCut(Cn).dblESPoint = Double.Parse(mDATA(i)) : i = i + 1         ' ｴｯｼﾞｾﾝｽﾎﾟｲﾝﾄ
                .ArrCut(Cn).dblESJudgeLevel = Double.Parse(mDATA(i)) : i = i + 1    ' ｴｯｼﾞｾﾝｽの判定変化率
                .ArrCut(Cn).dblMaxCutLengthES = Double.Parse(mDATA(i)) : i = i + 1  ' ｴｯｼﾞｾﾝｽ後のｶｯﾄ長

                .ArrCut(Cn).dblESChangeRatio = Double.Parse(mDATA(i)) : i = i + 1   ' ｴｯｼﾞｾﾝｽ後変化率
                .ArrCut(Cn).intESConfirmCnt = Short.Parse(mDATA(i)) : i = i + 1     ' ｴｯｼﾞｾﾝｽ後の確認回数
                .ArrCut(Cn).intRadderInterval = Short.Parse(mDATA(i)) : i = i + 1   ' ﾗﾀﾞｰ間距離
                '----- V1.14.0.0①↓ -----
                If (SL432HW_FileVer >= FILE_VER_10_05) Then
                    .ArrCut(Cn).intCTcount = Short.Parse(mDATA(i)) : i = i + 1      ' ｴｯｼﾞｾﾝｽ後連続NG確認回数※追加(ES用)
                Else
                    .ArrCut(Cn).intCTcount = 0                                      ' ｴｯｼﾞｾﾝｽ後連続NG確認回数※追加(ES用) 
                End If
                '----- V1.14.0.0①↑ -----

                .ArrCut(Cn).dblZoom = Double.Parse(mDATA(i)) : i = i + 1            ' 倍率 

                .ArrCut(Cn).intMoveMode = Short.Parse(mDATA(i)) : i = i + 1         ' 動作モード 
                .ArrCut(Cn).intDoPosition = Short.Parse(mDATA(i)) : i = i + 1       ' ポジショニング(0:有, 1:無)
                '----- V2.0.0.0_24(V1.18.0.3②)↓ -----
                If (SL432HW_FileVer >= FILE_VER_10_073) Then
                    .ArrCut(Cn).intCutAftPause = Short.Parse(mDATA(i)) : i = i + 1  ' カット後ポーズタイム
                Else
                    .ArrCut(Cn).intCutAftPause = 0                                  ' カット後ポーズタイム
                End If
                '----- V2.0.0.0_24(V1.18.0.3②)↑ -----
                '----- V1.16.0.0①↓ -----
                If (SL432HW_FileVer >= FILE_VER_10_06) Then
                    .ArrCut(Cn).dblReturnPos = Double.Parse(mDATA(i)) : i = i + 1   ' リターンカットのリターン位置
                Else
                    .ArrCut(Cn).dblReturnPos = 0.0                                  ' リターンカットのリターン位置
                End If
                '----- V1.16.0.0①↑ -----
                '----- V1.18.0.0④↓ -----
                If (SL432HW_FileVer >= FILE_VER_10_07) Then
                    .ArrCut(Cn).dblLimitLen = Double.Parse(mDATA(i)) : i = i + 1    ' IXカットのリミット長
                Else
                    .ArrCut(Cn).dblLimitLen = 0.0                                   ' IXカットのリミット長
                End If
                '----- V1.18.0.0④↑ -----

                ' FL用データ
                .ArrCut(Cn).dblCutSpeed3 = Double.Parse(mDATA(i)) : i = i + 1       ' カットスピード3
                .ArrCut(Cn).dblCutSpeed4 = Double.Parse(mDATA(i)) : i = i + 1       ' カットスピード4               
                .ArrCut(Cn).dblCutSpeed5 = Double.Parse(mDATA(i)) : i = i + 1       ' カットスピード5               
                .ArrCut(Cn).dblCutSpeed6 = Double.Parse(mDATA(i)) : i = i + 1       ' カットスピード6
                For j = 0 To (cCNDNUM - 1)                                          ' 加工条件番号1～8(0ｵﾘｼﾞﾝ)
                    .ArrCut(Cn).CndNum(j) = Short.Parse(mDATA(i)) : i = i + 1
                Next j

                '----- V2.0.0.0_23↓ -----
                ' SL436S用(シンプルトリマ用)
                ' ファイルバージョンが10.10以上の時に設定する
                If (SL432HW_FileVer >= FILE_VER_10_10) Then
                    typResistorInfoArray(Rn).ArrCut(Cn).dblQRate3 = Double.Parse(mDATA(i)) : i = i + 1
                    typResistorInfoArray(Rn).ArrCut(Cn).dblQRate4 = Double.Parse(mDATA(i)) : i = i + 1
                    typResistorInfoArray(Rn).ArrCut(Cn).dblQRate5 = Double.Parse(mDATA(i)) : i = i + 1
                    typResistorInfoArray(Rn).ArrCut(Cn).dblQRate6 = Double.Parse(mDATA(i)) : i = i + 1
                Else
                    typResistorInfoArray(Rn).ArrCut(Cn).dblQRate3 = 0.1             ' (初期値)
                    typResistorInfoArray(Rn).ArrCut(Cn).dblQRate4 = 0.1
                    typResistorInfoArray(Rn).ArrCut(Cn).dblQRate5 = 0.1
                    typResistorInfoArray(Rn).ArrCut(Cn).dblQRate6 = 0.1
                End If
                '----- V2.0.0.0_23↑ -----

                ' 目標パワーと許容範囲 ###066
                For j = 0 To (cCNDNUM - 1)                                          ' 加工条件番号1～n(0ｵﾘｼﾞﾝ)
                    '----- V2.0.0.0_23↓ -----
                    ' SL436S用(シンプルトリマ用)
                    ' ファイルバージョンが10.10以上の時に設定する
                    If (SL432HW_FileVer >= FILE_VER_10_10) Then
                        typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(j) = Short.Parse(mDATA(i)) : i = i + 1
                        typResistorInfoArray(Rn).ArrCut(Cn).FLSteg(j) = Short.Parse(mDATA(i)) : i = i + 1
                    Else
                        ' ファイルバージョンが10.00の時はデフォルト値を設定する
                        typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(j) = POWERADJUST_CURRENT  ' 電流値1～8
                        typResistorInfoArray(Rn).ArrCut(Cn).FLSteg(j) = POWERADJUST_STEG        ' STEG1～8
                    End If
                    '----- V2.0.0.0⑫↑ -----

                    ' ファイルバージョンが10.01以上の時に設定する
                    If (SL432HW_FileVer >= FILE_VER_10_01) Then
                        typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(j) = Double.Parse(mDATA(i)) : i = i + 1
                        typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustToleLevel(j) = Double.Parse(mDATA(i)) : i = i + 1
                    Else
                        ' ファイルバージョンが10.00以下の時はデフォルト値を設定する
                        typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(j) = POWERADJUST_TARGET   ' 目標パワー(W)
                        typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustToleLevel(j) = POWERADJUST_LEVEL  ' 許容範囲(±W)
                    End If
                    '----- V1.18.0.4①↓ -----
                    If (typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(j) = 0.0) Then
                        typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(j) = POWERADJUST_TARGET
                        typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustToleLevel(j) = POWERADJUST_LEVEL
                    End If
                    '----- V1.18.0.4①↑ -----
                Next j

                ' 文字列データ
                .ArrCut(Cn).strChar = mDATA(i).Trim() : i = i + 1                   ' 文字列 
                .ArrCut(Cn).strDataName = mDATA(i).Trim() : i = i + 1               ' Uカットデータ名※追加 
            End With

            Return (cFRS_NORMAL)                                    ' Return値 = 正常

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "File.Get_CUT_Data() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                      ' Return値 = 例外エラー
        End Try
    End Function
#End Region
    '----- ###229↓ -----
#Region "ロードしたGPIBデータをGPIBデータ構造体へ格納する【TKY,CHIP,NET統合版】"
    '''=========================================================================
    ''' <summary>ロードしたGPIBデータをGPIBデータ構造体へ格納する</summary>
    ''' <param name="mDATA">       (INP)データ</param>
    ''' <param name="typGpibInfo"> (OUT)GPIBデータ構造体</param>
    ''' <returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function Set_typGpibInfo(ByVal mDATA() As String, ByRef typGpibInfo As GpibInfo) As Integer

        Dim i As Integer
        Dim strMSG As String

        Try
            ' ファイルバージョン >= 10.03の時に設定する
            If (SL432HW_FileVer < FILE_VER_10_03) Then Return (cFRS_NORMAL)

            ' GPIBデータ構造体へデータを格納する
            With typGpibInfo
                i = 0
                .wGPIBmode = Short.Parse(mDATA(i)) : i = i + 1          ' GP-IB制御(0:しない 1:する)
                .wDelim = Short.Parse(mDATA(i)) : i = i + 1             ' ﾃﾞﾘﾐﾀ(0:CR+LF 1:CR 2:LF 3:NONE)
                .wTimeout = Short.Parse(mDATA(i)) : i = i + 1           ' ﾀｲﾑｱｳﾄ(1～32767)(ms単位)
                .wAddress = Short.Parse(mDATA(i)) : i = i + 1           ' 機器ｱﾄﾞﾚｽ(0～30)
                .wEOI = Short.Parse(mDATA(i)) : i = i + 1               ' EOI(0:使用しない, 1:使用する)
                .wPause1 = Short.Parse(mDATA(i)) : i = i + 1            ' 設定ｺﾏﾝﾄﾞ1送信後ポーズ時間(1～32767msec)
                .wPause2 = Short.Parse(mDATA(i)) : i = i + 1            ' 設定ｺﾏﾝﾄﾞ2送信後ポーズ時間(1～32767msec)
                .wPause3 = Short.Parse(mDATA(i)) : i = i + 1            ' 設定ｺﾏﾝﾄﾞ3送信後ポーズ時間(1～32767msec)
                .wPauseT = Short.Parse(mDATA(i)) : i = i + 1            ' ﾄﾘｶﾞｺﾏﾝﾄﾞ送信後ポーズ時間(1～32767msec)
                .wRev = Short.Parse(mDATA(i)) : i = i + 1               ' 予備
                .strI = mDATA(i) : i = i + 1                            ' 初期化ｺﾏﾝﾄﾞ(MAX40byte)
                .strI2 = mDATA(i) : i = i + 1                           ' 初期化ｺﾏﾝﾄﾞ2(MAX40byte)
                .strI3 = mDATA(i) : i = i + 1                           ' 初期化ｺﾏﾝﾄﾞ3(MAX40byte)
                .strT = mDATA(i) : i = i + 1                            ' ﾄﾘｶﾞｺﾏﾝﾄﾞ(50byte)
                .strName = mDATA(i) : i = i + 1                         ' 機器名(10byte)
                .wReserve = mDATA(i) : i = i + 1                        ' 予備(8byte)  
            End With

            Return (cFRS_NORMAL)                                        ' Return値 = 正常

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "File.Set_typGpibInfo() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return値 = 例外エラー
        End Try
    End Function
#End Region
    '----- ###229↑ -----
#Region "ロードした伸縮補正用データを格納する【TKY,CHIP,NET統合版】"
    '''=========================================================================
    ''' <summary>ロードした伸縮補正用データを格納する</summary>
    ''' <param name="pBuff">       (INP)読み込みデータ</param>
    ''' <returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function SetSinsyukuData(ByVal pBuff As String) As Integer

        Dim mDATA() As String
        Dim i As Integer
        Dim mBlock() As String

        If pBuff = "" Then
            Return 0
        End If

        i = 0
        ' ロードデータを配列セット 
        mDATA = pBuff.Split(",")
        For i = 0 To mDATA.Length - 1
            mBlock = mDATA(i).Split("-")
            SelectBlock((mBlock(0) - 1), (mBlock(1) - 1)) = 1
        Next i
    End Function

#End Region
#End If
#End Region 'V5.0.0.8①

#Region "ファイルセーブ処理【TKY,CHIP,NET統合版】"
    '''=========================================================================
    '''<summary>ファイルセーブ処理【TKY,CHIP,NET統合版】</summary>
    '''<param name="strPath">(INP) ファイル名</param>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Public Function File_Save(ByVal strPath As String) As Integer

        'Dim writer As System.IO.StreamWriter
        Dim r As Integer
        Dim strMSG As String

        Try
#If False Then      'V5.0.0.8①
            ' 初期処理
            '                                                       ' false = 上書き(true = 追加)
            'writer = New System.IO.StreamWriter(strPath, False, System.Text.Encoding.GetEncoding("Shift_JIS"))
            Using writer As New StreamWriter(strPath, False, Encoding.UTF8)     'V4.4.0.0-1

                ' [FILE VERSION]データを書き込む
                writer.WriteLine(CONST_VERSION)                         ' "[FILE VERSION]" ※"\r\n"付き　単純に文字列を書き込むには、Write() を使用する。
                If (gTkyKnd = KND_TKY) Then
                    'writer.WriteLine(CONST_FILETYPE10)                 ' "TKYDATA_Ver10.00"
                    writer.WriteLine(CONST_FILETYPE_CUR)                ' 統合ソフト版(現在版名) ###066
                ElseIf (gTkyKnd = KND_CHIP) Then
                    'writer.WriteLine(FILETYPE10)                       ' "TKYCHIP_SL432HW_Ver10.00"
                    writer.WriteLine(FILETYPE_CUR)                      '統合ソフト版(現在版名) ###066
                Else
                    'writer.WriteLine(FILETYPE10)                       ' "TKYNET_SL432HW_Ver10.00"
                    writer.WriteLine(FILETYPE_CUR)                      ' 統合ソフト版(現在版名) ###066
                End If

                ' [PLATE01]データを書き込む
                r = Put_PLT01_Data(writer)
                If (r <> cFRS_NORMAL) Then
                    'writer.Close()
                    Return (cFRS_FIOERR_OUT)
                End If

                ' [PLATE03]データを書き込む
                r = Put_PLT03_Data(writer)
                If (r <> cFRS_NORMAL) Then
                    'writer.Close()
                    Return (cFRS_FIOERR_OUT)
                End If

                ' [PLATE02]データを書き込む
                r = Put_PLT02_Data(writer)
                If (r <> cFRS_NORMAL) Then
                    'writer.Close()
                    Return (cFRS_FIOERR_OUT)
                End If

                ' [PLATE04]データを書き込む
                r = Put_PLT04_Data(writer)
                If (r <> cFRS_NORMAL) Then
                    'writer.Close()
                    Return (cFRS_FIOERR_OUT)
                End If

                ' [PLATE05]データを書き込む
                r = Put_PLT05_Data(writer)
                If (r <> cFRS_NORMAL) Then
                    'writer.Close()
                    Return (cFRS_FIOERR_OUT)
                End If

                '----- V1.13.0.0②↓ -----
                ' [PLATE06]データを書き込む
                r = Put_PLT06_Data(writer)
                If (r <> cFRS_NORMAL) Then
                    'writer.Close()
                    Return (cFRS_FIOERR_OUT)
                End If
                '----- V1.13.0.0②↑ -----

            'V5.0.0.6①↓
            ' [OPTION]データを書き込む
            r = Put_OPTION_Data(writer)
            If (r <> cFRS_NORMAL) Then
                writer.Close()
                Return (cFRS_FIOERR_OUT)
            End If
            'V5.0.0.6①↑

                ' サーキットデータを書き込む(TKY用)
                r = Put_CIRCUIT_Data(writer)
                If (r <> cFRS_NORMAL) Then
                    'writer.Close()
                    Return (cFRS_FIOERR_OUT)
                End If

                ' ステップデータを書き込む(CHIP/NET用)
                r = Put_STEP_Data(writer)
                If (r <> cFRS_NORMAL) Then
                    'writer.Close()
                    Return (cFRS_FIOERR_OUT)
                End If

                ' グループデータを書き込む(CHIP用)
                r = Put_GROUP_Data(writer)
                If (r <> cFRS_NORMAL) Then
                    'writer.Close()
                    Return (cFRS_FIOERR_OUT)
                End If

                ' TY2データを書き込む(CHIP用)
                r = Put_TY2_Data(writer)
                If (r <> cFRS_NORMAL) Then
                    'writer.Close()
                    Return (cFRS_FIOERR_OUT)
                End If

                ' サーキット座標データを書き込む(NET用)
                r = Put_CIRCUITAXIS_Data(writer)
                If (r <> cFRS_NORMAL) Then
                    'writer.Close()
                    Return (cFRS_FIOERR_OUT)
                End If

                ' サーキット間インターバルデータを書き込む(NET用)
                r = Put_CIRCUITINTERVAL_Data(writer)
                If (r <> cFRS_NORMAL) Then
                    'writer.Close()
                    Return (cFRS_FIOERR_OUT)
                End If

                ' 異形面付けデータ(TKY用) 未サポート

                ' 抵抗データを書き込む
                r = Put_RESIST_Data(writer)
                If (r <> cFRS_NORMAL) Then
                    'writer.Close()
                    Return (cFRS_FIOERR_OUT)
                End If

                ' カットデータを書き込む
                r = Put_CUT_Data(writer)
                If (r <> cFRS_NORMAL) Then
                    'writer.Close()
                    Return (cFRS_FIOERR_OUT)
                End If

                '----- ###229↓ -----
                ' GPIBデータを書き込む(TKY/CHIP/NET用)
                r = Put_GPIB_Data(writer)
                If (r <> cFRS_NORMAL) Then
                    'writer.Close()
                    Return (cFRS_FIOERR_OUT)
                End If
                '----- ###229↑ -----

                'V1.13.0.0⑤
                '伸縮補正用データの書き込み 
                r = Put_SINSYUKU_Data(writer)
                If (r <> cFRS_NORMAL) Then
                    'writer.Close()
                    Return (cFRS_FIOERR_OUT)
                End If
                'V1.13.0.0⑤

                ' 終了処理
                'writer.Close()
            End Using

            Return (cFRS_NORMAL)                                    ' Return値 = 正常
#End If
            'V5.0.0.8①              ↓
            SetTemporaryData(False)    ' 値を復帰する
            r = DirectCast(FileIO.File_Save(strPath), Integer)
            SetTemporaryData(True)     ' 値を退避する
            'V5.0.0.8①              ↑

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "File.File_Save() TRAP ERROR = " & ex.Message
            MsgBox(strMSG)
            'V5.0.0.8①            Return (cERR_TRAP)                                      ' Return値 = 例外エラー
            r = cERR_TRAP               ' Return値 = 例外エラー
        End Try

        Return r    'V5.0.0.8①

    End Function
#End Region

#Region "データ退避・復帰"
    '''=========================================================================
    ''' <summary>
    ''' データ退避・復帰
    ''' </summary>
    ''' <param name="toSave">True:退避,False:復帰</param>
    ''' <remarks>'V5.0.0.8①</remarks>
    '''=========================================================================
    Private Sub SetTemporaryData(ByVal toSave As Boolean)

        ' ファイル読み込み済みで目標値オフセットが有効の場合
        If (True = gLoadDTFlag) AndAlso (0 <> giTargetOfs) Then
            If (toSave) Then
                ' 退避
                For i As Integer = 1 To (typResistorInfoArray.Length - 1) Step 1
                    With typResistorInfoArray(i)
                        .dblTrimTargetOfs_Save = .dblTrimTargetOfs                      ' ﾄﾘﾐﾝｸﾞ目標値ｵﾌｾｯﾄを退避
                        .dblTrimTargetOfs = .dblTrimTargetVal * (.dblTrimTargetOfs_Save / 100)

                        .dblTrimTargetVal_Save = .dblTrimTargetVal                      ' ﾄﾘﾐﾝｸﾞ目標値を退避
                        ' 目標値オフセット有効ならﾄﾘﾐﾝｸﾞ目標値をﾄﾘﾐﾝｸﾞ目標値+ｵﾌｾｯﾄ(指定がある場合)とする
                        If (0.0 <> .dblTrimTargetOfs) Then                              ' 目標値オフセット有効 ? 
                            .dblTrimTargetVal = .dblTrimTargetVal + .dblTrimTargetOfs   ' ﾄﾘﾐﾝｸﾞ目標値 = 目標値 + ｵﾌｾｯﾄ
                        End If
                    End With
                Next i
            Else
                ' 復帰
                For i As Integer = 1 To (typResistorInfoArray.Length - 1) Step 1
                    With typResistorInfoArray(i)
                        .dblTrimTargetOfs = .dblTrimTargetOfs_Save ' ﾄﾘﾐﾝｸﾞ目標値ｵﾌｾｯﾄ退避域から戻す
                        .dblTrimTargetVal = .dblTrimTargetVal_Save ' ﾄﾘﾐﾝｸﾞ目標値退避域から戻す
                    End With
                Next i
            End If
            '----- V6.0.3.0_43↓ -----
        Else
            ' 目標値オフセットが無効の場合
            For i As Integer = 1 To (typResistorInfoArray.Length - 1) Step 1
                With typResistorInfoArray(i)
                    .dblTrimTargetOfs = 0.0
                    .dblTrimTargetVal_Save = .dblTrimTargetVal
                End With
            Next i
            '----- V6.0.3.0_43↑ -----
        End If

    End Sub
#End Region

#Region "LOAD・SAVEの共通化によりTrimDataEditorで定義"
#If False Then 'V5.0.0.8①
#Region "[PLATE01]データを書き込む【TKY,CHIP,NET統合版】"
    '''=========================================================================
    '''<summary>[PLATE01]データを書き込む</summary>
    '''<param name="writer">(INP)StreamWriterオブジェクト</param>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function Put_PLT01_Data(ByVal writer As System.IO.StreamWriter) As Integer

        Dim strDat As String
        Dim strMSG As String

        Try
            ' データをプレートデータ構造体へ格納する
            With typPlateInfo
                ' [PLATE01]データを書き込む
                writer.WriteLine(FILE_CONST_PLATE_01)                           ' "[PLATE01]"
                writer.WriteLine(.strDataName)                                  ' トリミングデータ名
                writer.WriteLine(.intDirStepRepeat.ToString("0"))               ' ｽﾃｯﾌﾟ＆ﾘﾋﾟｰﾄ
                writer.WriteLine(.intChipStepCnt.ToString("0"))                 ' チップステップ数
                strDat = .intPlateCntXDir.ToString("0") + "," + .intPlateCntYDir.ToString("0")
                writer.WriteLine(strDat)                                        ' プレート数X,Y 
                strDat = .intBlockCntXDir.ToString("0") + "," + .intBlockCntYDir.ToString("0")
                writer.WriteLine(strDat)                                        ' ﾌﾞﾛｯｸ数X,Y 
                strDat = .dblPlateItvXDir.ToString("0.00000") + "," + .dblPlateItvYDir.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' プレート間隔X,Y 
                strDat = .dblBlockSizeXDir.ToString("0.00000") + "," + .dblBlockSizeYDir.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' ブロックサイズX,Y 
                strDat = .dblTableOffsetXDir.ToString("0.0000") + "," + .dblTableOffsetYDir.ToString("0.0000")
                writer.WriteLine(strDat)                                        ' ﾃｰﾌﾞﾙ位置ｵﾌｾｯﾄX,Y 
                strDat = .dblBpOffSetXDir.ToString("0.00000") + "," + .dblBpOffSetYDir.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' ﾋﾞｰﾑ位置ｵﾌｾｯﾄX,Y 
                strDat = .dblAdjOffSetXDir.ToString("0.00000") + "," + .dblAdjOffSetYDir.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' ｱｼﾞｬｽﾄ位置ｵﾌｾｯﾄX,Y(未使用)
                writer.WriteLine(.intCurcuitCnt.ToString("0"))                  ' サーキット数
                writer.WriteLine(.intNGMark.ToString("0"))                      ' NGﾏｰｷﾝｸﾞ
                writer.WriteLine(.intDelayTrim.ToString("0"))                   ' ﾃﾞｨﾚｲﾄﾘﾑ
                writer.WriteLine(.intNgJudgeUnit.ToString("0"))                 ' NG判定単位
                writer.WriteLine(.intNgJudgeLevel.ToString("0"))                ' NG判定基準
                writer.WriteLine(.dblZOffSet.ToString("0.0000"))                ' ZON位置
                writer.WriteLine(.dblZStepUpDist.ToString("0.0000"))            ' ｽﾃｯﾌﾟ上昇距離
                writer.WriteLine(.dblZWaitOffset.ToString("0.0000"))            ' ZOFF位置
                '----- V1.13.0.0②↓ -----
                writer.WriteLine(.dblLwPrbStpDwDist.ToString("0.0000"))         ' 下方ﾌﾟﾛｰﾌﾞｽﾃｯﾌﾟ下降距離
                writer.WriteLine(.dblLwPrbStpUpDist.ToString("0.0000"))         ' 下方ﾌﾟﾛｰﾌﾞｽﾃｯﾌﾟ上昇距離
                '----- V1.13.0.0②↑ -----
                '----- V4.0.0.0④↓ -----
                ' プローブリトライ回数(0=リトライなし)(オプション)
                writer.WriteLine(.intPrbRetryCount.ToString("0"))               ' プローブリトライ回数(0=リトライなし)(オプション)
                '----- V4.0.0.0④↑ -----
                '----- V1.23.0.0⑦↓ -----
                ' プローブチェック項目(オプション)
                writer.WriteLine(.intPrbChkPlt.ToString("0"))                   ' 枚数
                writer.WriteLine(.intPrbChkBlk.ToString("0"))                   ' ブロック
                writer.WriteLine(.dblPrbTestLimit.ToString("0.000"))            ' 誤差±%
                '----- V1.23.0.0⑦↑ -----

            End With
            Return (cFRS_NORMAL)                                                ' Return値 = 正常

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "File.Put_PLT01_Data() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                                  ' Return値 = 例外エラー
        End Try
    End Function
#End Region

#Region "[PLATE03]データを書き込む【TKY,CHIP,NET統合版】"
    '''=========================================================================
    '''<summary>[PLATE03]データを書き込む</summary>
    '''<param name="writer">(INP)StreamWriterオブジェクト</param>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function Put_PLT03_Data(ByVal writer As System.IO.StreamWriter) As Integer

        Dim strDat As String
        Dim strMSG As String

        Try
            ' データをプレートデータ構造体へ格納する
            With typPlateInfo
                ' [PLATE03]データを書き込む
                writer.WriteLine(FILE_CONST_PLATE_03)                           ' "[PLATE03]"
                writer.WriteLine(.intResistDir.ToString("0"))                   ' 抵抗並び方向
                writer.WriteLine(.intCircuitCntInBlock.ToString("0"))           ' 1ﾌﾞﾛｯｸ内ｻｰｷｯﾄ数 
                strDat = .dblCircuitSizeXDir.ToString("0.00000") + "," + .dblCircuitSizeYDir.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' ｻｰｷｯﾄｻｲｽﾞX,Y 
                writer.WriteLine(.intResistCntInBlock.ToString("0"))            ' 1ブロック内抵抗数
                writer.WriteLine(.intResistCntInGroup.ToString("0"))            ' 1ｸﾞﾙｰﾌﾟ内抵抗数
                strDat = .intGroupCntInBlockXBp.ToString("0") + "," + .intGroupCntInBlockYStage.ToString("0")
                writer.WriteLine(strDat)                                        ' ﾌﾞﾛｯｸ内ｸﾞﾙｰﾌﾟ数X,Y 
                strDat = .intBlkCntInStgGrpX.ToString("0") + "," + .intBlkCntInStgGrpY.ToString("0")
                writer.WriteLine(strDat)                                        ' ステージグループ内ブロック数X,Y 
                strDat = .dblBpGrpItv.ToString("0.00000") + "," + .dblStgGrpItvY.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' ｸﾞﾙｰﾌﾟ間隔BP(X),Stage(Y) 
                strDat = .dblChipSizeXDir.ToString("0.00000") + "," + .dblChipSizeYDir.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' ﾁｯﾌﾟｻｲｽﾞX,Y
                strDat = .dblStepOffsetXDir.ToString("0.00000") + "," + .dblStepOffsetYDir.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' ｽﾃｯﾌﾟｵﾌｾｯﾄ量X,Y
                strDat = .dblBlockSizeReviseXDir.ToString("0.00000") + "," + .dblBlockSizeReviseYDir.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' ﾌﾞﾛｯｸｻｲｽﾞ補正量X,Y
                strDat = .dblBlockItvXDir.ToString("0.00000") + "," + .dblBlockItvYDir.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' ﾌﾞﾛｯｸ間隔X,Y
                writer.WriteLine(.intContHiNgBlockCnt.ToString("0"))            ' 連続NG-HIGH抵抗ﾌﾞﾛｯｸ数
                strDat = .dblPlateSizeX.ToString("0.00000") + "," + .dblPlateSizeY.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' プレートサイズX,Y
                strDat = .dblStgGrpItvX.ToString("0.00000") + "," + .dblStgGrpItvY.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' ステージグループ間隔X,Y
            End With
            Return (cFRS_NORMAL)                                                ' Return値 = 正常

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "File.Put_PLT03_Data() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                                  ' Return値 = 例外エラー
        End Try
    End Function
#End Region

#Region "[PLATE02]データを書き込む【TKY,CHIP,NET統合版】"
    '''=========================================================================
    '''<summary>[PLATE02]データを書き込む</summary>
    '''<param name="writer">(INP)StreamWriterオブジェクト</param>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function Put_PLT02_Data(ByVal writer As System.IO.StreamWriter) As Integer

        Dim strDat As String
        Dim strMSG As String

        Try
            ' データをプレートデータ構造体へ格納する
            With typPlateInfo
                ' [PLATE02]データを書き込む
                writer.WriteLine(FILE_CONST_PLATE_02)                           ' "[PLATE02]"
                writer.WriteLine(.intReviseMode.ToString("0"))                  ' 補正モード
                writer.WriteLine(.intManualReviseType.ToString("0"))            ' 補正方法
                strDat = .dblReviseCordnt1XDir.ToString("0.00000") + "," + .dblReviseCordnt1YDir.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' 補正位置座標1X,Y 
                strDat = .dblReviseCordnt2XDir.ToString("0.00000") + "," + .dblReviseCordnt2YDir.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' 補正位置座標2X,Y 
                strDat = .dblReviseOffsetXDir.ToString("0.00000") + "," + .dblReviseOffsetYDir.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' 補正ﾎﾟｼﾞｼｮﾝｵﾌｾｯﾄX,Y 
                writer.WriteLine(.intRecogDispMode.ToString("0"))               ' 認識ﾃﾞｰﾀ表示ﾓｰﾄﾞ
                strDat = .dblPixelValXDir.ToString("0.0000") + "," + .dblPixelValYDir.ToString("0.0000")
                writer.WriteLine(strDat)                                        ' ﾋﾟｸｾﾙ値X,Y 
                strDat = .intRevisePtnNo1.ToString("0") + "," + .intRevisePtnNo2.ToString("0")
                writer.WriteLine(strDat)                                        ' 補正位置ﾊﾟﾀｰﾝNo1,2
                strDat = .intRevisePtnNo1GroupNo.ToString("0") + "," + .intRevisePtnNo2GroupNo.ToString("0")
                writer.WriteLine(strDat)                                        ' 補正位置グループNo1,2
                writer.WriteLine(.dblRotateTheta.ToString("0.00000"))           ' θ軸角度
                writer.WriteLine(.dblTThetaOffset.ToString("0.00000"))          ' Ｔθオフセット
                strDat = .dblTThetaBase1XDir.ToString("0.00000") + "," + .dblTThetaBase1YDir.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' Ｔθ基準位置1X,Y
                strDat = .dblTThetaBase2XDir.ToString("0.00000") + "," + .dblTThetaBase2YDir.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' Ｔθ基準位置2X,Y
                'V5.0.0.9②                                          ↓ ラフアライメント用
                writer.WriteLine(.intReviseExecRgh.ToString("0"))               ' 補正有無(0:補正なし, 1:補正あり)
                strDat = .dblReviseCordnt1XDirRgh.ToString("0.00000") & "," & .dblReviseCordnt1YDirRgh.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' 補正位置座標1X,補正位置座標1Y
                strDat = .dblReviseCordnt2XDirRgh.ToString("0.00000") & "," & .dblReviseCordnt2YDirRgh.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' 補正位置座標2X,補正位置座標2Y
                strDat = .dblReviseOffsetXDirRgh.ToString("0.00000") & "," & .dblReviseOffsetYDirRgh.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' 補正ﾎﾟｼﾞｼｮﾝｵﾌｾｯﾄX,補正ﾎﾟｼﾞｼｮﾝｵﾌｾｯﾄY
                writer.WriteLine(.intRecogDispModeRgh.ToString("0"))            ' 認識ﾃﾞｰﾀ表示ﾓｰﾄﾞ(0:表示なし, 1:表示する)
                strDat = .intRevisePtnNo1Rgh & "," & .intRevisePtnNo2Rgh
                writer.WriteLine(strDat)                                        ' 補正位置ﾊﾟﾀｰﾝNo1,補正位置ﾊﾟﾀｰﾝNo2
                strDat = .intRevisePtnNo1GroupNoRgh & "," & .intRevisePtnNo2GroupNoRgh
                writer.WriteLine(strDat)                                        ' 補正位置ﾊﾟﾀｰﾝNo1グループNo,補正位置ﾊﾟﾀｰﾝNo2グループNo
                'V5.0.0.9②                                          ↑
            End With
            Return (cFRS_NORMAL)                                                ' Return値 = 正常

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "File.Put_PLT02_Data() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                                  ' Return値 = 例外エラー
        End Try
    End Function
#End Region

#Region "[PLATE04]データを書き込む【TKY,CHIP,NET統合版】"
    '''=========================================================================
    '''<summary>[PLATE04]データを書き込む</summary>
    '''<param name="writer">(INP)StreamWriterオブジェクト</param>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function Put_PLT04_Data(ByVal writer As System.IO.StreamWriter) As Integer

        Dim strDat As String
        Dim strMSG As String

        Try
            ' データをプレートデータ構造体へ格納する
            With typPlateInfo
                ' [PLATE04]データを書き込む
                writer.WriteLine(FILE_CONST_PLATE_04)                           ' "[PLATE04]"
                strDat = .dblCaribBaseCordnt1XDir.ToString("0.00000") + "," + .dblCaribBaseCordnt1YDir.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標1X,Y 
                strDat = .dblCaribBaseCordnt2XDir.ToString("0.00000") + "," + .dblCaribBaseCordnt2YDir.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標2X,Y 
                strDat = .dblCaribTableOffsetXDir.ToString("0.00000") + "," + .dblCaribTableOffsetYDir.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' ｷｬﾘﾌﾞﾚｰｼｮﾝｵﾌｾｯﾄX,Y 
                strDat = .intCaribPtnNo1.ToString("0") + "," + .intCaribPtnNo2.ToString("0")
                writer.WriteLine(strDat)                                        ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾊﾟﾀｰﾝ登録No1,2 
                strDat = .intCaribPtnNo1GroupNo.ToString("0") + "," + .intCaribPtnNo2GroupNo.ToString("0")
                writer.WriteLine(strDat)                                        ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾊﾟﾀｰﾝグループNo1,2 
                writer.WriteLine(.dblCaribCutLength.ToString("0.00000"))        ' ｷｬﾘﾌﾞﾚｰｼｮﾝｶｯﾄ長
                writer.WriteLine(.dblCaribCutSpeed.ToString("0.0"))             ' ｷｬﾘﾌﾞﾚｰｼｮﾝｶｯﾄ速度
                writer.WriteLine(.dblCaribCutQRate.ToString("0.0"))             ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾚｰｻﾞQﾚｰﾄ
                writer.WriteLine(.intCaribCutCondNo.ToString("0"))              ' ｷｬﾘﾌﾞﾚｰｼｮﾝ加工条件番号(FL用)

                strDat = .dblCutPosiReviseOffsetXDir.ToString("0.000") + "," + .dblCutPosiReviseOffsetYDir.ToString("0.000")
                writer.WriteLine(strDat)                                        ' ｶｯﾄ位置補正ﾃｰﾌﾞﾙｵﾌｾｯﾄX,Y 
                writer.WriteLine(.intCutPosiRevisePtnNo.ToString("0"))          ' ｶｯﾄ位置補正ﾊﾟﾀｰﾝ登録No
                writer.WriteLine(.dblCutPosiReviseCutLength.ToString("0.00000")) ' ｶｯﾄ位置補正ｶｯﾄ長
                writer.WriteLine(.dblCutPosiReviseCutSpeed.ToString("0.0"))     ' ｶｯﾄ位置補正ｶｯﾄ速度
                writer.WriteLine(.dblCutPosiReviseCutQRate.ToString("0.0"))     ' ｶｯﾄ位置補正ﾚｰｻﾞQﾚｰﾄ
                writer.WriteLine(.intCutPosiReviseGroupNo.ToString("0"))        ' ｶｯﾄ位置補正ｸﾞﾙｰﾌﾟNo
                writer.WriteLine(.intCutPosiReviseCondNo.ToString("0"))         ' ｶｯﾄ位置補正加工条件番号(FL用)
            End With
            Return (cFRS_NORMAL)                                                ' Return値 = 正常

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "File.Put_PLT04_Data() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                                  ' Return値 = 例外エラー
        End Try
    End Function
#End Region

#Region "[PLATE05]データを書き込む【TKY,CHIP,NET統合版】"
    '''=========================================================================
    '''<summary>[PLATE05]データを書き込む</summary>
    '''<param name="writer">(INP)StreamWriterオブジェクト</param>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function Put_PLT05_Data(ByVal writer As System.IO.StreamWriter) As Integer

        Dim strMSG As String

        Try
            ' データをプレートデータ構造体へ格納する
            With typPlateInfo
                ' [PLATE05]データを書き込む
                writer.WriteLine(FILE_CONST_PLATE_05)                           ' "[PLATE05]"
                writer.WriteLine(.intMaxTrimNgCount.ToString("0"))              ' ﾄﾘﾐﾝｸﾞNGｶｳﾝﾀ(上限)
                writer.WriteLine(.intMaxBreakDischargeCount.ToString("0"))      ' 割れ欠け排出ｶｳﾝﾀ(上限)
                writer.WriteLine(.intTrimNgCount.ToString("0"))                 ' 連続ﾄﾘﾐﾝｸﾞNG枚数
                writer.WriteLine(.intContHiNgResCnt.ToString("0"))              ' 連続ﾄﾘﾐﾝｸﾞNG抵抗数    ###230
                writer.WriteLine(.intRetryProbeCount.ToString("0"))             ' 再ﾌﾟﾛｰﾋﾞﾝｸﾞ回数
                writer.WriteLine(.dblRetryProbeDistance.ToString("0.00000"))    ' 再ﾌﾟﾛｰﾋﾞﾝｸﾞ移動量

                writer.WriteLine(.intWorkSetByLoader.ToString("0"))             ' 基板品種
                writer.WriteLine(.intOpenCheck.ToString("0"))                   ' 4端子ｵｰﾌﾟﾝﾁｪｯｸ
                writer.WriteLine(.intLedCtrl.ToString("0"))                     ' LED制御

                writer.WriteLine(.intPowerAdjustMode.ToString("0"))             ' パワー調整モード
                writer.WriteLine(.dblPowerAdjustTarget.ToString("0.00"))        ' 調整目標パワー
                writer.WriteLine(.dblPowerAdjustQRate.ToString("0.0"))          ' パワー調整Qレート
                writer.WriteLine(.dblPowerAdjustToleLevel.ToString("0.00"))     ' パワー調整許容範囲
                writer.WriteLine(.intPowerAdjustCondNo.ToString("0"))           ' パワー調整加工条件番号(FL用) 

                writer.WriteLine(.intGpibCtrl.ToString("0"))                    ' GP-IB制御
                writer.WriteLine(.intGpibDefDelimiter.ToString("0"))            ' 初期設定(ﾃﾞﾘﾐﾀ)
                writer.WriteLine(.intGpibDefTimiout.ToString("0"))              ' 初期設定(ﾀｲﾑｱｳﾄ)
                writer.WriteLine(.intGpibDefAdder.ToString("0"))                ' 初期設定(機器ｱﾄﾞﾚｽ)'###002
                writer.WriteLine(.strGpibInitCmnd1)                             ' 初期化ｺﾏﾝﾄﾞ1
                writer.WriteLine(.strGpibInitCmnd2)                             ' 初期化ｺﾏﾝﾄﾞ2
                writer.WriteLine(.strGpibTriggerCmnd)                           ' ﾄﾘｶﾞｺﾏﾝﾄﾞ
                writer.WriteLine(.intGpibMeasSpeed.ToString("0"))               ' 測定速度(0:低速, 1:高速)
                writer.WriteLine(.intGpibMeasMode.ToString("0"))                ' 測定モード(0:絶対, 1:偏差)
                writer.WriteLine(.intNgJudgeStop.ToString("0"))                 ' NG判定時停止 V1.13.0.0②
                '----- V4.11.0.0①↓ (WALSIN殿SL436S対応) -----
                writer.WriteLine(.intPwrChkPltNum.ToString("0"))                ' オートパワーチェック基板枚数
                writer.WriteLine(.intPwrChkTime.ToString("0"))                  ' オートパワーチェック時間(分) 
                '----- V4.11.0.0①↑ -----
            End With
            Return (cFRS_NORMAL)                                                ' Return値 = 正常

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "File.Put_PLT05_Data() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                                  ' Return値 = 例外エラー
        End Try
    End Function
#End Region
    '----- V1.13.0.0②↓ -----
#Region "[PLATE06]データを書き込む【TKY,CHIP,NET統合版】"
    '''=========================================================================
    '''<summary>[PLATE06]データを書き込む</summary>
    '''<param name="writer">(INP)StreamWriterオブジェクト</param>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function Put_PLT06_Data(ByVal writer As System.IO.StreamWriter) As Integer

        Dim strMSG As String

        Try
            ' データをプレートデータ構造体へ格納する
            With typPlateInfo
                ' [PLATE06]データを書き込む
                writer.WriteLine(FILE_CONST_PLATE_06)                           ' "[PLATE06]"
                writer.WriteLine(.intContExpMode.ToString("0"))                 ' 伸縮補正 (0:なし, 1:あり)
                writer.WriteLine(.intContExpGrpNo.ToString("0"))                ' 伸縮補正ｸﾞﾙｰﾌﾟ番号
                writer.WriteLine(.intContExpPtnNo.ToString("0"))                ' 伸縮補正ﾊﾟﾀｰﾝ番号
                writer.WriteLine(.dblContExpPosX.ToString("0.0000"))            ' 伸縮補正位置X (mm)
                writer.WriteLine(.dblContExpPosY.ToString("0.0000"))            ' 伸縮補正位置XY (mm)            
                writer.WriteLine(.intStepMeasCnt.ToString("0"))                 ' ｽﾃｯﾌﾟ測定回数
                writer.WriteLine(.dblStepMeasPitch.ToString("0.0000"))          ' ｽﾃｯﾌﾟ測定ﾋﾟｯﾁ
                writer.WriteLine(.intStepMeasReptCnt.ToString("0"))             ' ｽﾃｯﾌﾟ測定繰り返しｽﾃｯﾌﾟ回数
                writer.WriteLine(.dblStepMeasReptPitch.ToString("0.0000"))      ' ｽﾃｯﾌﾟ測定繰り返しｽﾃｯﾌﾟﾋﾟｯﾁ
                writer.WriteLine(.intStepMeasLwGrpNo.ToString("0"))             ' ｽﾃｯﾌﾟ測定下方ﾌﾟﾛｰﾌﾞｸﾞﾙｰﾌﾟ番号
                writer.WriteLine(.intStepMeasLwPtnNo.ToString("0"))             ' ｽﾃｯﾌﾟ測定下方ﾌﾟﾛｰﾌﾞﾊﾟﾀｰﾝ番号
                writer.WriteLine(.dblStepMeasBpPosX.ToString("0.0000"))         ' ｽﾃｯﾌﾟ測定BP位置X
                writer.WriteLine(.dblStepMeasBpPosY.ToString("0.0000"))         ' ｽﾃｯﾌﾟ測定BP位置Y
                writer.WriteLine(.intStepMeasUpGrpNo.ToString("0"))             ' ｽﾃｯﾌﾟ測定基板上ﾌﾟﾛｰﾌﾞｸﾞﾙｰﾌﾟ番号
                writer.WriteLine(.intStepMeasUpPtnNo.ToString("0"))             ' ｽﾃｯﾌﾟ測定基板上ﾌﾟﾛｰﾌﾞﾊﾟﾀｰﾝ番号
                writer.WriteLine(.dblStepMeasTblOstX.ToString("0.0000"))        ' ｽﾃｯﾌﾟ測定基板上ﾌﾟﾛｰﾌﾞﾃｰﾌﾞﾙｵﾌｾｯﾄX
                writer.WriteLine(.dblStepMeasTblOstY.ToString("0.0000"))        ' ｽﾃｯﾌﾟ測定基板上ﾌﾟﾛｰﾌﾞﾃｰﾌﾞﾙｵﾌｾｯﾄY
                writer.WriteLine(.intIDReaderUse.ToString("0"))                 ' IDﾘｰﾄﾞ (0:未使用, 1:使用)
                writer.WriteLine(.dblIDReadPos1X.ToString("0.0000"))            ' IDﾘｰﾄﾞ読み取りﾎﾟｼﾞｼｮﾝ 1X
                writer.WriteLine(.dblIDReadPos1Y.ToString("0.0000"))            ' IDﾘｰﾄﾞ読み取りﾎﾟｼﾞｼｮﾝ 1Y
                writer.WriteLine(.dblIDReadPos2X.ToString("0.0000"))            ' IDﾘｰﾄﾞ読み取りﾎﾟｼﾞｼｮﾝ 2X
                writer.WriteLine(.dblIDReadPos2Y.ToString("0.0000"))            ' IDﾘｰﾄﾞ読み取りﾎﾟｼﾞｼｮﾝ 2Y
                writer.WriteLine(.dblReprobeVar.ToString("0.0000"))             ' 再ﾌﾟﾛｰﾋﾞﾝｸﾞばらつき量
                writer.WriteLine(.dblReprobePitch.ToString("0.0000"))           ' 再ﾌﾟﾛｰﾋﾞﾝｸﾞﾋﾟｯﾁ

                '----- V2.0.0.0_23↓ -----
                '----- プローブクリーニング項目 -----
                writer.WriteLine(.dblPrbCleanPosX.ToString("0.0000"))           ' クリーニング位置X
                writer.WriteLine(.dblPrbCleanPosY.ToString("0.0000"))           ' クリーニング位置Y
                writer.WriteLine(.dblPrbCleanPosZ.ToString("0.0000"))           ' クリーニング位置Z
                writer.WriteLine(.intPrbCleanUpDwCount.ToString("0"))           ' プローブ上下回数
                writer.WriteLine(.intPrbCleanAutoSubCount.ToString("0"))        ' 自動運転時クリーニング実行基板枚数
                '----- V2.0.0.0⑫↑ -----

                writer.WriteLine(.dblTXChipsizeRelationX.ToString("0.0000"))    ' 補正位置１と２の相対値Ｘ 'V4.5.1.0⑮
                writer.WriteLine(.dblTXChipsizeRelationY.ToString("0.0000"))    ' 補正位置１と２の相対値Ｙ 'V4.5.1.0⑮

                ''V4.10.0.0④            ↓
                'writer.WriteLine(.dblPrbCleanStagePitchX.ToString("0.0000"))    ' ステージ動作ピッチX
                'writer.WriteLine(.dblPrbCleanStagePitchY.ToString("0.0000"))    ' ステージ動作ピッチY
                'writer.WriteLine(.intPrbCleanStageCountX.ToString())            ' ステージ動作回数X
                'writer.WriteLine(.intPrbCleanStageCountY.ToString())            ' ステージ動作回数Y
                ''V4.10.0.0④            ↑
                ''V4.10.0.0⑨↓
                'writer.WriteLine(.dblPrbDistance.ToString("0.0000"))            ' ステージ動作ピッチX
                'writer.WriteLine(.dblPrbCleaningOffset.ToString("0.0000"))      ' ステージ動作ピッチY
                ''V4.10.0.0⑨↑

            End With
            Return (cFRS_NORMAL)                                                ' Return値 = 正常

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "File.Put_PLT06_Data() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                                  ' Return値 = 例外エラー
        End Try
    End Function
#End Region
    '----- V1.13.0.0②↑ -----

#Region "[OPTION]データを書き込む【TKY,CHIP,NET統合版】"
    'V5.0.0.6①↓
    ''' <summary>
    ''' [OPTION]データを書き込む
    ''' </summary>
    ''' <param name="writer">(INP)StreamWriterオブジェクト</param>
    ''' <returns>正常:cFRS_NORMAL, エラー:cERR_TRAP</returns>
    ''' <remarks></remarks>
    Private Function Put_OPTION_Data(ByVal writer As System.IO.StreamWriter) As Integer

        Dim strMSG As String

        Try
            ' データをプレートデータ構造体へ格納する
            If gbControllerInterlock Then
                With typPlateInfo
                    ' [OPTION]データを書き込む
                    writer.WriteLine(FILE_CONST_PLATE_OPTION)                        ' "[OPTION]"
                    writer.WriteLine(.intControllerInterlock.ToString("0"))         ' 外部機器によるインターロックの有無
                End With
            End If
            Return (cFRS_NORMAL)                                                ' Return値 = 正常

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "File.Put_OPTION_Data() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                                  ' Return値 = 例外エラー
        End Try
    End Function
    'V5.0.0.6①↑
#End Region

#Region "[CIRCUIT]データを書き込む【TKY,CHIP,NET統合版】"
    '''=========================================================================
    '''<summary>[CIRCUIT]データを書き込む</summary>
    '''<param name="writer">(INP)StreamWriterオブジェクト</param>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function Put_CIRCUIT_Data(ByVal writer As System.IO.StreamWriter) As Integer

        Dim i As Integer
        Dim strDat As String
        Dim strMSG As String

        Try
            ' [CIRCUIT]データを書き込む(TKY用)
            writer.WriteLine(CONST_CIRCUIT)                                     ' "[CIRCUIT]"
            If (gTkyKnd <> KND_TKY) Then                                        ' TKY以外ならNOP 
                Return (cFRS_NORMAL)
            End If

            If (typPlateInfo.intNGMark <> 0) Then
                For i = 1 To typPlateInfo.intCurcuitCnt
                    strDat = typCircuitInfoArray(i).intIP1.ToString("0") + ","                  ' IP番号
                    strDat = strDat + typCircuitInfoArray(i).dblIP2X.ToString("0.00000") + ","  ' マーキングX
                    strDat = strDat + typCircuitInfoArray(i).dblIP2Y.ToString("0.00000")        ' マーキングY
                    writer.WriteLine(strDat)
                Next i
            End If

            Return (cFRS_NORMAL)                                                ' Return値 = 正常

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "File.Put_CIRCUIT_Data() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                                  ' Return値 = 例外エラー
        End Try
    End Function
#End Region

#Region "[STEP]データを書き込む【TKY,CHIP,NET統合版】"
    '''=========================================================================
    '''<summary>[STEP]データを書き込む</summary>
    '''<param name="writer">(INP)StreamWriterオブジェクト</param>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function Put_STEP_Data(ByVal writer As System.IO.StreamWriter) As Integer

        Dim i As Integer
        Dim strDat As String
        Dim strWAK As String
        Dim strMSG As String

        Try
            ' [STEP]データを書き込む(CHIP用)
            writer.WriteLine(FILE_CONST_STEPDATA)                               ' "[STEP]"
            If (gTkyKnd = KND_TKY) Then                                         ' TKYならNOP 
                Return (cFRS_NORMAL)
            End If

            For i = 1 To MaxStep
                strWAK = typStepInfoArray(i).intSP1.ToString("0")               ' ｽﾃｯﾌﾟ番号
                strDat = strWAK.PadLeft(3) + ","                                ' 3文字(左側に空白パディング)
                strWAK = typStepInfoArray(i).intSP2.ToString("0")               ' ﾌﾞﾛｯｸ数
                strDat = strDat + strWAK.PadLeft(3) + ","                       ' 3文字(左側に空白パディング)
                strWAK = typStepInfoArray(i).dblSP3.ToString("0.00000")         ' ｽﾃｯﾌﾟ間ｲﾝﾀｰﾊﾞﾙ
                strDat = strDat + strWAK.PadLeft(8)                             ' 8文字(左側に空白パディング)
                writer.WriteLine(strDat)
            Next i

            Return (cFRS_NORMAL)                                                ' Return値 = 正常

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "File.Put_STEP_Data() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                                  ' Return値 = 例外エラー
        End Try
    End Function
#End Region

#Region "[GROUP]データを書き込む【TKY,CHIP,NET統合版】"
    '''=========================================================================
    '''<summary>[GROUP]データを書き込む</summary>
    '''<param name="writer">(INP)StreamWriterオブジェクト</param>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function Put_GROUP_Data(ByVal writer As System.IO.StreamWriter) As Integer

        Dim i As Integer
        Dim strDat As String
        Dim strWAK As String
        Dim strMSG As String

        Try
            ' [GROUP]データを書き込む(CHIP用)
            writer.WriteLine(FILE_CONST_GRP_DATA)                               ' "[GROUP]"
            If (gTkyKnd <> KND_CHIP) Then                                       ' CHIP以外ならNOP 
                Return (cFRS_NORMAL)
            End If

            For i = 1 To MaxGrp
                strWAK = typGrpInfoArray(i).intGP1.ToString("0")                ' ｸﾞﾙｰﾌﾟ番号
                strDat = strWAK.PadLeft(3) + ","                                ' 3文字(左側に空白パディング)
                strWAK = typGrpInfoArray(i).intGP2.ToString("0")                ' 抵抗数
                strDat = strDat + strWAK.PadLeft(3) + ","                       ' 3文字(左側に空白パディング)
                strWAK = typGrpInfoArray(i).dblGP3.ToString("0.00000")          ' ｸﾞﾙｰﾌﾟ間ｲﾝﾀｰﾊﾞﾙ
                strDat = strDat + strWAK.PadLeft(8)                             ' 8文字(左側に空白パディング)
                writer.WriteLine(strDat)
            Next i

            Return (cFRS_NORMAL)                                                ' Return値 = 正常

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "File.Put_GROUP_Data() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                                  ' Return値 = 例外エラー
        End Try
    End Function
#End Region

#Region "[TY2]データを書き込む【TKY,CHIP,NET統合版】"
    '''=========================================================================
    '''<summary>[TY2]データを書き込む</summary>
    '''<param name="writer">(INP)StreamWriterオブジェクト</param>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function Put_TY2_Data(ByVal writer As System.IO.StreamWriter) As Integer

        Dim i As Integer
        Dim strDat As String
        Dim strWAK As String
        Dim strMSG As String

        Try
            ' [TY2]データを書き込む(CHIP用)
            writer.WriteLine(FILE_CONST_TY2_DATA)                               ' "[TY2]"
            If (gTkyKnd <> KND_CHIP) Then                                       ' CHIP以外ならNOP 
                Return (cFRS_NORMAL)
            End If

            For i = 1 To MaxTy2
                strWAK = typTy2InfoArray(i).intTy21.ToString("0")               ' ﾌﾞﾛｯｸ番号
                strDat = strWAK.PadLeft(3) + ","                                ' 3文字(左側に空白パディング)
                strWAK = typTy2InfoArray(i).dblTy22.ToString("0.00000")         ' ｽﾃｯﾌﾟ距離
                strDat = strDat + strWAK.PadLeft(8)                             ' 8文字(左側に空白パディング)
                writer.WriteLine(strDat)
            Next i

            Return (cFRS_NORMAL)                                                ' Return値 = 正常

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "File.Put_TY2_Data() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                                  ' Return値 = 例外エラー
        End Try
    End Function
#End Region

#Region "[CIRCUIT AXIS]データを書き込む【TKY,CHIP,NET統合版】"
    '''=========================================================================
    '''<summary>[CIRCUIT AXIS]データを書き込む</summary>
    '''<param name="writer">(INP)StreamWriterオブジェクト</param>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function Put_CIRCUITAXIS_Data(ByVal writer As System.IO.StreamWriter) As Integer

        Dim i As Integer
        Dim CirNum As Integer
        Dim strDat As String
        Dim strWAK As String
        Dim strMSG As String

        Try
            ' [CIRCUIT AXIS]データを書き込む(NET用)
            writer.WriteLine(FILE_CONST_CIRN_DATA)                              ' "[CIRCUIT AXIS]"
            If (gTkyKnd <> KND_NET) Then                                        ' NET以外ならNOP 
                Return (cFRS_NORMAL)
            End If

            CirNum = typPlateInfo.intCircuitCntInBlock
            For i = 1 To CirNum
                strWAK = typCirAxisInfoArray(i).intCaP1.ToString("0")           ' ｽﾃｯﾌﾟ番号
                strDat = strWAK.PadLeft(3) + ","                                ' 3文字(左側に空白パディング)
                strWAK = typCirAxisInfoArray(i).dblCaP2.ToString("0.00000")     ' 座標X
                strDat = strDat + strWAK.PadLeft(8) + ","                       ' 8文字(左側に空白パディング)
                strWAK = typCirAxisInfoArray(i).dblCaP3.ToString("0.00000")     ' 座標Y
                strDat = strDat + strWAK.PadLeft(8)                             ' 8文字(左側に空白パディング)
                writer.WriteLine(strDat)
            Next i

            Return (cFRS_NORMAL)                                                ' Return値 = 正常

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "File.Put_CIRCUITAXIS_Data() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                                  ' Return値 = 例外エラー
        End Try
    End Function
#End Region

#Region "[CIRCUIT INTERVAL]データを書き込む【TKY,CHIP,NET統合版】"
    '''=========================================================================
    '''<summary>[CIRCUIT INTERVAL]データを書き込む</summary>
    '''<param name="writer">(INP)StreamWriterオブジェクト</param>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function Put_CIRCUITINTERVAL_Data(ByVal writer As System.IO.StreamWriter) As Integer

        Dim i As Integer
        Dim GrpNum As Integer
        Dim strDat As String
        Dim strWAK As String
        Dim strMSG As String

        Try
            ' [CIRCUIT INTERVAL]データを書き込む(NET用)
            writer.WriteLine(FILE_CONST_CIRIDATA)                               ' "[CIRCUIT INTERVAL]"
            If (gTkyKnd <> KND_NET) Then                                        ' NET以外ならNOP 
                Return (cFRS_NORMAL)
            End If

            If (typPlateInfo.intCircuitCntInBlock = 0) Then
                GrpNum = typPlateInfo.intGroupCntInBlockXBp                     ' ｸﾞﾙｰﾌﾟ数X
            Else
                GrpNum = typPlateInfo.intGroupCntInBlockYStage                  ' ｸﾞﾙｰﾌﾟ数Y
            End If

            For i = 1 To GrpNum
                strWAK = typCirInInfoArray(i).intCiP1.ToString("0")             ' ｽﾃｯﾌﾟ番号
                strDat = strWAK.PadLeft(3) + ","                                ' 3文字(左側に空白パディング)
                strWAK = typCirInInfoArray(i).intCiP2.ToString("0")             ' ｻｰｷｯﾄ数
                strDat = strDat + strWAK.PadLeft(3) + ","                       ' 3文字(左側に空白パディング)
                strWAK = typCirInInfoArray(i).dblCiP3.ToString("0.00000")       ' ｻｰｷｯﾄ間ｲﾝﾀｰﾊﾞﾙ
                strDat = strDat + strWAK.PadLeft(8)                             ' 8文字(左側に空白パディング)
                writer.WriteLine(strDat)
            Next i

            Return (cFRS_NORMAL)                                                ' Return値 = 正常

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "File.Put_CIRCUITINTERVAL_Data() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                                  ' Return値 = 例外エラー
        End Try
    End Function
#End Region

#Region "抵抗データを書き込む【TKY,CHIP,NET統合版】"
    '''=========================================================================
    '''<summary>抵抗データを書き込む</summary>
    '''<param name="writer">(INP)StreamWriterオブジェクト</param>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function Put_RESIST_Data(ByVal writer As System.IO.StreamWriter) As Integer

        Dim Rn As Integer
        Dim Count As Integer
        Dim strDat As String
        Dim strWAK As String
        Dim strMSG As String

        Try
            ' 抵抗データを書き込む
            'Call GetChipNum(Count)                                                         ' 抵抗数取得
            Count = typPlateInfo.intResistCntInBlock                                        ' 抵抗数取得
            writer.WriteLine(FILE_CONST_RESISTOR)                                           ' "[RESISTOR]"

            For Rn = 1 To Count
                ' 抵抗データを書き込む
                strWAK = typResistorInfoArray(Rn).intResNo.ToString("0")                    ' 抵抗番号
                strDat = strWAK.PadLeft(4) + ","                                            ' 4文字(左側に空白パディング)
                strWAK = typResistorInfoArray(Rn).intResMeasMode.ToString("0")              ' 測定モード(0:抵抗 ,1:電圧 ,2:外部) 
                strDat = strDat + strWAK.PadLeft(2) + ","                                   ' 2文字(左側に空白パディング)
                strWAK = typResistorInfoArray(Rn).intResMeasType.ToString("0")              ' 測定タイプ(0:高速 ,1:高精度)　※追加
                strDat = strDat + strWAK.PadLeft(2) + ","                                   ' 2文字(左側に空白パディング)
                strWAK = typResistorInfoArray(Rn).intCircuitGrp.ToString("0")               ' 所属ｻｰｷｯﾄ番号
                strDat = strDat + strWAK.PadLeft(3) + ","                                   ' 3文字(左側に空白パディング)
                strWAK = typResistorInfoArray(Rn).intProbHiNo.ToString("0")                 ' ﾌﾟﾛｰﾌﾞ番号(HI)
                strDat = strDat + strWAK.PadLeft(3) + ","                                   ' 3文字(左側に空白パディング)
                strWAK = typResistorInfoArray(Rn).intProbLoNo.ToString("0")                 ' ﾌﾟﾛｰﾌﾞ番号(LO)
                strDat = strDat + strWAK.PadLeft(3) + ","                                   ' 3文字(左側に空白パディング)
                strWAK = typResistorInfoArray(Rn).intProbAGNo1.ToString("0")                ' ﾌﾟﾛｰﾌﾞ番号(AG1)
                strDat = strDat + strWAK.PadLeft(3) + ","                                   ' 3文字(左側に空白パディング)
                strWAK = typResistorInfoArray(Rn).intProbAGNo2.ToString("0")                ' ﾌﾟﾛｰﾌﾞ番号(AG2)
                strDat = strDat + strWAK.PadLeft(3) + ","                                   ' 3文字(左側に空白パディング)
                strWAK = typResistorInfoArray(Rn).intProbAGNo3.ToString("0")                ' ﾌﾟﾛｰﾌﾞ番号(AG3)
                strDat = strDat + strWAK.PadLeft(3) + ","                                   ' 3文字(左側に空白パディング)
                strWAK = typResistorInfoArray(Rn).intProbAGNo4.ToString("0")                ' ﾌﾟﾛｰﾌﾞ番号(AG4)
                strDat = strDat + strWAK.PadLeft(3) + ","                                   ' 3文字(左側に空白パディング)
                strWAK = typResistorInfoArray(Rn).intProbAGNo5.ToString("0")                ' ﾌﾟﾛｰﾌﾞ番号(AG5)
                strDat = strDat + strWAK.PadLeft(3) + ","                                   ' 3文字(左側に空白パディング)
                strWAK = typResistorInfoArray(Rn).strExternalBits                           ' EXTERNAL BITS(16ﾋﾞｯﾄ)
                strDat = strDat + strWAK.PadLeft(17) + ","                                  ' 17文字(左側に空白パディング)
                strWAK = typResistorInfoArray(Rn).intPauseTime.ToString("0")                ' ﾎﾟｰｽﾞﾀｲﾑ(msec)
                strDat = strDat + strWAK.PadLeft(6) + ","                                   ' 6文字(左側に空白パディング)
                strWAK = typResistorInfoArray(Rn).intTargetValType.ToString("0")            ' 目標値指定（0:絶対値,1:レシオ,2:計算式）
                strDat = strDat + strWAK.PadLeft(2) + ","                                   ' 2文字(左側に空白パディング)
                strWAK = typResistorInfoArray(Rn).intBaseResNo.ToString("0")                ' ﾍﾞｰｽ抵抗番号
                strDat = strDat + strWAK.PadLeft(4) + ","                                   ' 4文字(左側に空白パディング)
                strWAK = typResistorInfoArray(Rn).dblTrimTargetVal.ToString("0.000000")     ' ﾄﾘﾐﾝｸﾞ目標値
                '----- V4.11.0.0①↓ (WALSIN殿SL436S対応) -----
                ' 目標値 = 目標値オフセット有効ならﾄﾘﾐﾝｸﾞ目標値退避域から設定する
                If (giTargetOfs = 1) Then                                                   ' 目標値オフセット有効 ? 
                    strWAK = typResistorInfoArray(Rn).dblTrimTargetVal_Save.ToString("0.000000")
                End If
                '----- V4.11.0.0①↑ -----
                strDat = strDat + strWAK.PadLeft(16) + ","                                  ' 16文字(左側に空白パディング)
                strWAK = typResistorInfoArray(Rn).dblProbCfmPoint_Hi_X.ToString("0.00000")  ' プローブ確認位置 HI X座標 
                strDat = strDat + strWAK.PadLeft(9) + ","                                   ' 9文字(左側に空白パディング)
                strWAK = typResistorInfoArray(Rn).dblProbCfmPoint_Hi_Y.ToString("0.00000")  ' プローブ確認位置 HI Y座標 
                strDat = strDat + strWAK.PadLeft(9) + ","                                   ' 9文字(左側に空白パディング)
                strWAK = typResistorInfoArray(Rn).dblProbCfmPoint_Lo_X.ToString("0.00000")  ' プローブ確認位置 LO X座標 
                strDat = strDat + strWAK.PadLeft(9) + ","                                   ' 9文字(左側に空白パディング)
                strWAK = typResistorInfoArray(Rn).dblProbCfmPoint_Lo_Y.ToString("0.00000")  ' プローブ確認位置 LO Y座標 
                strDat = strDat + strWAK.PadLeft(9) + ","                                   ' 9文字(左側に空白パディング)
                strWAK = typResistorInfoArray(Rn).intSlope.ToString("0")                    ' 電圧変化ｽﾛｰﾌﾟ 
                strDat = strDat + strWAK.PadLeft(2) + ","                                   ' 2文字(左側に空白パディング)
                strWAK = typResistorInfoArray(Rn).dblInitTest_HighLimit.ToString("0.00")    ' ｲﾆｼｬﾙﾃｽﾄ(HIGHﾘﾐｯﾄ)
                strDat = strDat + strWAK.PadLeft(7) + ","                                   ' 2文字(左側に空白パディング)
                strWAK = typResistorInfoArray(Rn).dblInitTest_LowLimit.ToString("0.00")     ' ｲﾆｼｬﾙﾃｽﾄ(Lowﾘﾐｯﾄ)
                strDat = strDat + strWAK.PadLeft(7) + ","                                   ' 2文字(左側に空白パディング)
                strWAK = typResistorInfoArray(Rn).dblFinalTest_HighLimit.ToString("0.00")   ' ﾌｧｲﾅﾙﾃｽﾄ(HIGHﾘﾐｯﾄ)
                strDat = strDat + strWAK.PadLeft(7) + ","                                   ' 2文字(左側に空白パディング)
                strWAK = typResistorInfoArray(Rn).dblFinalTest_LowLimit.ToString("0.00")    ' ﾌｧｲﾅﾙﾃｽﾄ(Lowﾘﾐｯﾄ)
                strDat = strDat + strWAK.PadLeft(7) + ","                                   ' 7文字(左側に空白パディング)
                strWAK = typResistorInfoArray(Rn).intInitialOkTestDo.ToString("0")          ' ｲﾆｼｬﾙOKﾃｽﾄ(0:しない,1:する)※追加(プレートデータから移動)
                strDat = strDat + strWAK.PadLeft(2) + ","                                   ' 2文字(左側に空白パディング)
                strWAK = typResistorInfoArray(Rn).intCutReviseMode.ToString("0")            ' ｶｯﾄ補正(0:無し, 1:自動)     
                strDat = strDat + strWAK.PadLeft(2) + ","                                   ' 2文字(左側に空白パディング)
                strWAK = typResistorInfoArray(Rn).intCutReviseDispMode.ToString("0")        ' 表示ﾓｰﾄﾞ(0:無し, 1:CRT)     
                strDat = strDat + strWAK.PadLeft(2) + ","                                   ' 2文字(左側に空白パディング)
                strWAK = typResistorInfoArray(Rn).intCutRevisePtnNo.ToString("0")           ' ﾊﾟﾀｰﾝ番号
                strDat = strDat + strWAK.PadLeft(2) + ","                                   ' 2文字(左側に空白パディング)
                strWAK = typResistorInfoArray(Rn).intCutReviseGrpNo.ToString("0")           ' ﾊﾟﾀｰﾝｸﾞﾙｰﾌﾟ番号    
                strDat = strDat + strWAK.PadLeft(2) + ","                                   ' 2文字(左側に空白パディング)
                strWAK = typResistorInfoArray(Rn).dblCutRevisePosX.ToString("0.00000")      ' 補正位置X
                strDat = strDat + strWAK.PadLeft(9) + ","                                   ' 9文字(左側に空白パディング)
                strWAK = typResistorInfoArray(Rn).dblCutRevisePosY.ToString("0.00000")      ' 補正位置Y
                strDat = strDat + strWAK.PadLeft(9) + ","                                   ' 9文字(左側に空白パディング)
                strWAK = typResistorInfoArray(Rn).intIsNG.ToString("0")                     ' 画像認識NG判定(0:あり, 1:なし, 手動)
                strDat = strDat + strWAK.PadLeft(2) + ","                                   ' 2文字(左側に空白パディング)
                strWAK = typResistorInfoArray(Rn).intCutCount.ToString("0")                 ' ｶｯﾄ数
                strDat = strDat + strWAK.PadLeft(2) + ","                                   ' 2文字(左側に空白パディング)
                strDat = strDat + typResistorInfoArray(Rn).strRatioTrimTargetVal            ' レシオ計算式
                '----- V1.13.0.0②↓ -----
                strDat = strDat + ","
                strWAK = typResistorInfoArray(Rn).intCvMeasNum.ToString("0")                ' CV 最大測定回数
                strDat = strDat + strWAK.PadLeft(2) + ","                                   ' 2文字(左側に空白パディング)
                strWAK = typResistorInfoArray(Rn).intCvMeasTime.ToString("0")               ' CV 最大測定時間(ms)
                strDat = strDat + strWAK.PadLeft(2) + ","                                   ' 2文字(左側に空白パディング)
                strWAK = typResistorInfoArray(Rn).dblCvValue.ToString("0.000000")           ' CV CV値  
                strDat = strDat + strWAK.PadLeft(10) + ","                                  ' 10文字(左側に空白パディング)
                strWAK = typResistorInfoArray(Rn).intOverloadNum.ToString("0")              ' ｵｰﾊﾞｰﾛｰﾄﾞ 回数 
                strDat = strDat + strWAK.PadLeft(2) + ","                                   ' 2文字(左側に空白パディング)
                strWAK = typResistorInfoArray(Rn).dblOverloadMin.ToString("0.00")           ' ｵｰﾊﾞｰﾛｰﾄﾞ 下限値  
                strDat = strDat + strWAK.PadLeft(8) + ","                                   ' 8文字(左側に空白パディング)
                strWAK = typResistorInfoArray(Rn).dblOverloadMax.ToString("0.00")           ' ｵｰﾊﾞｰﾛｰﾄﾞ 上限値
                strDat = strDat + strWAK.PadLeft(8)                                         ' 8文字(左側に空白パディング)
                '----- V1.13.0.0②↑ -----
                '----- V2.0.0.0_23↓ -----
                strDat = strDat + ","
                strWAK = typResistorInfoArray(Rn).wPauseTimeFT.ToString("0")                ' FT前のポーズタイム
                strDat = strDat + strWAK.PadLeft(6)                                         ' 6文字(左側に空白パディング)
                strDat = strDat + ","
                strWAK = typResistorInfoArray(Rn).intInsideEndChkCount.ToString("0")        ' 中切り判定回数
                strDat = strDat + strWAK.PadLeft(6)                                         ' 6文字(左側に空白パディング)
                strDat = strDat + ","
                strWAK = typResistorInfoArray(Rn).dblInsideEndChgRate.ToString("0.00")      ' 中切り判定変化率(0.00-100.00%)
                strDat = strDat + strWAK.PadLeft(8)                                         ' 6文字(左側に空白パディング)
                '----- V2.0.0.0_23↑ -----
                '----- V4.11.0.0①↓ (WALSIN殿SL436S対応) -----
                strDat = strDat + ","
                ''V5.0.0.2① ﾄﾘﾐﾝｸ                strWAK = typResistorInfoArray(Rn).dblTrimTargetOfs.ToString("0.000000")     ' 目標値オフセット
                strWAK = typResistorInfoArray(Rn).dblTrimTargetOfs_Save.ToString("0.000000")     ' 目標値オフセット
                strDat = strDat + strWAK.PadLeft(16)                                        ' 16文字(左側に空白パディング)
                '----- V4.11.0.0①↑ -----

                writer.WriteLine(strDat)
            Next Rn

            Return (cFRS_NORMAL)                                                            ' Return値 = 正常

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "File.Put_RESIST_Data() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                                              ' Return値 = 例外エラー
        End Try
    End Function
#End Region

#Region "カットデータを書き込む【TKY,CHIP,NET統合版】"
    '''=========================================================================
    '''<summary>カットデータを書き込む</summary>
    '''<param name="writer">(INP)StreamWriterオブジェクト</param>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function Put_CUT_Data(ByVal writer As System.IO.StreamWriter) As Integer

        Dim RegCount As Integer
        Dim Rn As Integer
        Dim Cn As Integer
        Dim i As Integer
        Dim strDat As String
        Dim strWAK As String
        Dim strMSG As String

        Try
            ' カットデータを書き込む
            'Call GetChipNum(RegCount)                                                              ' 抵抗数取得
            RegCount = typPlateInfo.intResistCntInBlock                                             ' 抵抗数取得
            writer.WriteLine(FILE_CONST_CUT_DATA)                                                   ' "[CUT]"

            For Rn = 1 To RegCount ' 抵抗数分繰り返す 
                ' カットデータを書き込む
                For Cn = 1 To typResistorInfoArray(Rn).intCutCount ' カット数分繰り返す 
                    strWAK = typResistorInfoArray(Rn).intResNo.ToString("0")                        ' 抵抗番号
                    strDat = strWAK.PadLeft(4) + ","                                                ' 4文字(左側に空白パディング)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).intCutNo.ToString("0")             ' ｶｯﾄ番号
                    strDat = strDat + strWAK.PadLeft(2) + ","                                       ' 2文字(左側に空白パディング)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).intDelayTime.ToString("0")         ' ﾃﾞｨﾚｲﾀｲﾑ
                    strDat = strDat + strWAK.PadLeft(6) + ","                                       ' 6文字(左側に空白パディング)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblTeachPointX.ToString("0.00000") ' ﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄX
                    strDat = strDat + strWAK.PadLeft(9) + ","                                       ' 9文字(左側に空白パディング)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblTeachPointY.ToString("0.00000") ' ﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄY
                    strDat = strDat + strWAK.PadLeft(9) + ","                                       ' 9文字(左側に空白パディング)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblStartPointX.ToString("0.00000") ' ｽﾀｰﾄﾎﾟｲﾝﾄX
                    strDat = strDat + strWAK.PadLeft(9) + ","                                       ' 9文字(左側に空白パディング)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblStartPointY.ToString("0.00000") ' ｽﾀｰﾄﾎﾟｲﾝﾄY
                    strDat = strDat + strWAK.PadLeft(9) + ","                                       ' 9文字(左側に空白パディング)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblCutSpeed.ToString("0.0")        ' ｶｯﾄｽﾋﾟｰﾄﾞ
                    strDat = strDat + strWAK.PadLeft(6) + ","                                       ' 6文字(左側に空白パディング)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblQRate.ToString("0.0")           ' Qｽｲｯﾁﾚｰﾄ
                    strDat = strDat + strWAK.PadLeft(5) + ","                                       ' 5文字(左側に空白パディング)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblCutOff.ToString("0.000")        ' ｶｯﾄｵﾌ値
                    strDat = strDat + strWAK.PadLeft(8) + ","                                       ' 8文字(左側に空白パディング)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblJudgeLevel.ToString("0.0")      ' ﾃﾞｰﾀ判定（平均化率）基準
                    strDat = strDat + strWAK.PadLeft(6) + ","                                       ' 6文字(左側に空白パディング)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblCutOffOffset.ToString("0.000")  ' ｶｯﾄｵﾌ ｵﾌｾｯﾄ
                    strDat = strDat + strWAK.PadLeft(8) + ","                                       ' 8文字(左側に空白パディング)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).strCutType                         ' ｶｯﾄ形状
                    strDat = strDat + strWAK.PadLeft(2) + ","                                       ' 2文字(左側に空白パディング)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).intCutDir.ToString("0")            ' ｶｯﾄ方向  ※未使用 斜めｶｯﾄの切り出し角度を使用
                    strDat = strDat + strWAK.PadLeft(2) + ","                                       ' 2文字(左側に空白パディング)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).intLTurnDir.ToString("0")          ' Lﾀｰﾝ方向(1:CW, 2:CCW) ※変更
                    strDat = strDat + strWAK.PadLeft(2) + ","                                       ' 2文字(左側に空白パディング)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblMaxCutLength.ToString("0.00000") ' 最大ｶｯﾃｨﾝｸﾞ長
                    strDat = strDat + strWAK.PadLeft(8) + ","                                       ' 8文字(左側に空白パディング)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblR1.ToString("0.00000")          ' R1
                    strDat = strDat + strWAK.PadLeft(8) + ","                                       ' 8文字(左側に空白パディング)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblLTurnPoint.ToString("0.0")      ' Lﾀｰﾝﾎﾟｲﾝﾄ
                    strDat = strDat + strWAK.PadLeft(6) + ","                                       ' 6文字(左側に空白パディング)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblMaxCutLengthL.ToString("0.00000") ' Lﾀｰﾝ後の最大ｶｯﾃｨﾝｸﾞ長
                    strDat = strDat + strWAK.PadLeft(8) + ","                                       ' 8文字(左側に空白パディング)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblR2.ToString("0.00000")          ' R2
                    strDat = strDat + strWAK.PadLeft(8) + ","                                       ' 8文字(左側に空白パディング)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblMaxCutLengthHook.ToString("0.00000") ' ﾌｯｸﾀｰﾝ後のｶｯﾃｨﾝｸﾞ長
                    strDat = strDat + strWAK.PadLeft(8) + ","                                       ' 8文字(左側に空白パディング)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).intIndexCnt.ToString("0")          ' ｲﾝﾃﾞｯｸｽ数
                    strDat = strDat + strWAK.PadLeft(6) + ","                                       ' 6文字(左側に空白パディング)
                    'strWAK = typResistorInfoArray(Rn).ArrCut(Cn).intMeasMode.ToString("0")         ' 測定ﾓｰﾄﾞ 2011.08.30
                    'strDat = strDat + strWAK.PadLeft(2) + ","                                      ' 2文字(左側に空白パディング)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).intMeasType.ToString("0")          ' 測定タイプ(0:高速 ,1:高精度, 2:外部))※IX用ﾞ
                    strDat = strDat + strWAK.PadLeft(2) + ","                                       ' 2文字(左側に空白パディング)

                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblCutSpeed2.ToString("0.0")       ' ｶｯﾄｽﾋﾟｰﾄﾞ2
                    strDat = strDat + strWAK.PadLeft(6) + ","                                       ' 6文字(左側に空白パディング)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblQRate2.ToString("0.0")          ' Qｽｲｯﾁﾚｰﾄ2
                    strDat = strDat + strWAK.PadLeft(5) + ","                                       ' 5文字(左側に空白パディング)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).intCutAngle.ToString("0")          ' 斜めｶｯﾄの切り出し角度
                    strDat = strDat + strWAK.PadLeft(4) + ","                                       ' 4文字(左側に空白パディング)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblPitch.ToString("0.00000")       ' ﾋﾟｯﾁ
                    strDat = strDat + strWAK.PadLeft(8) + ","                                       ' 8文字(左側に空白パディング)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).intStepDir.ToString("0")           ' ｽﾃｯﾌﾟ方向
                    strDat = strDat + strWAK.PadLeft(2) + ","                                       ' 2文字(左側に空白パディング)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).intCutCnt.ToString("0")            ' 本数
                    strDat = strDat + strWAK.PadLeft(5) + ","                                       ' 5文字(左側に空白パディング)

                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblESPoint.ToString("0.0000")      ' ｴｯｼﾞｾﾝｽﾎﾟｲﾝﾄ
                    strDat = strDat + strWAK.PadLeft(8) + ","                                       ' 8文字(左側に空白パディング)
                    ' ↓↓↓ V3.1.0.0③ 2014/12/03
                    ''strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblESJudgeLevel.ToString("0.0")   ' ｴｯｼﾞｾﾝｽの判定変化率           'V3.0.0.0⑨
                    ''strDat = strDat + strWAK.PadLeft(5) + ","                                      ' 5文字(左側に空白パディング)   'V3.0.0.0⑨
                    'strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblESJudgeLevel.ToString("0.000")  ' ｴｯｼﾞｾﾝｽの判定変化率           'V3.0.0.0⑨
                    'strDat = strDat + strWAK.PadLeft(7) + ","                                       ' 7文字(左側に空白パディング)   'V3.0.0.0⑨
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblESJudgeLevel.ToString("0.0")   ' ｴｯｼﾞｾﾝｽの判定変化率           'V3.0.0.0⑨
                    strDat = strDat + strWAK.PadLeft(5) + ","                                      ' 5文字(左側に空白パディング)   'V3.0.0.0⑨
                    ' ↑↑↑ V3.1.0.0③ 2014/12/03
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblMaxCutLengthES.ToString("0.000")  ' ｴｯｼﾞｾﾝｽ後のｶｯﾄ長 'V1.14.0.0①
                    strDat = strDat + strWAK.PadLeft(7) + ","                                       ' 7文字(左側に空白パディング)
                    'strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblESChangeRatio.ToString("0.0")  ' ｴｯｼﾞｾﾝｽ後変化率
                    ' ↓↓↓ V3.1.0.0③ 2014/12/03
                    ''strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblESChangeRatio.ToString("0.00") ' ｴｯｼﾞｾﾝｽ後変化率　             'V3.0.0.0⑨ V1.14.0.1② 
                    ''strDat = strDat + strWAK.PadLeft(6) + ","                                      ' 6文字(左側に空白パディング)   'V3.0.0.0⑨
                    'strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblESChangeRatio.ToString("0.000") ' ｴｯｼﾞｾﾝｽ後変化率　             'V3.0.0.0⑨
                    'strDat = strDat + strWAK.PadLeft(7) + ","                                       ' 7文字(左側に空白パディング)   'V3.0.0.0⑨
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblESChangeRatio.ToString("0.000") ' ｴｯｼﾞｾﾝｽ後変化率　             'V3.0.0.0⑨
                    strDat = strDat + strWAK.PadLeft(6) + ","                                      ' 6文字(左側に空白パディング)   'V3.0.0.0⑨
                    ' ↑↑↑ V3.1.0.0③ 2014/12/03
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).intESConfirmCnt.ToString("0")      ' ｴｯｼﾞｾﾝｽ後の確認回数
                    strDat = strDat + strWAK.PadLeft(2) + ","                                       ' 2文字(左側に空白パディング)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).intRadderInterval.ToString("0")    ' ﾗﾀﾞｰ間距離
                    strDat = strDat + strWAK.PadLeft(4) + ","                                       ' 4文字(左側に空白パディング)
                    '----- V1.14.0.0①↓ -----
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).intCTcount.ToString("0")           ' ｴｯｼﾞｾﾝｽ後連続NG確認回数
                    strDat = strDat + strWAK.PadLeft(4) + ","                                       ' 4文字(左側に空白パディング)
                    '----- V1.14.0.0①↑ -----
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblZoom.ToString("0.00")           ' 倍率
                    strDat = strDat + strWAK.PadLeft(6) + ","                                       ' 6文字(左側に空白パディング)

                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).intMoveMode.ToString("0")          ' 動作モード
                    strDat = strDat + strWAK.PadLeft(2) + ","                                       ' 2文字(左側に空白パディング)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).intDoPosition.ToString("0")        ' ポジショニング
                    strDat = strDat + strWAK.PadLeft(2) + ","                                       ' 2文字(左側に空白パディング)
                    '----- V2.0.0.0_24(V1.18.0.3②)↓ -----
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).intCutAftPause.ToString("0")       ' カット後ポーズタイム
                    strDat = strDat + strWAK.PadLeft(8) + ","                                       ' 8文字(左側に空白パディング)
                    '----- V2.0.0.0_24(V1.18.0.3②)↑ -----
                    '----- V1.16.0.0①↓ -----
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblReturnPos.ToString("0.00000")   ' リターンカットのリターン位置
                    strDat = strDat + strWAK.PadLeft(9) + ","                                       ' 9文字(左側に空白パディング)
                    '----- V1.16.0.0①↑ -----
                    '----- V1.18.0.0④↓ -----
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblLimitLen.ToString("0.00000")    ' IXカットのリミット長
                    strDat = strDat + strWAK.PadLeft(8) + ","                                       ' 8文字(左側に空白パディング)
                    '----- V1.18.0.0④↑ -----

                    ' FL用データ
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblCutSpeed3.ToString("0.0")       ' ｶｯﾄｽﾋﾟｰﾄﾞ3
                    strDat = strDat + strWAK.PadLeft(6) + ","                                       ' 6文字(左側に空白パディング)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblCutSpeed4.ToString("0.0")       ' ｶｯﾄｽﾋﾟｰﾄﾞ4
                    strDat = strDat + strWAK.PadLeft(6) + ","                                       ' 6文字(左側に空白パディング)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblCutSpeed5.ToString("0.0")       ' ｶｯﾄｽﾋﾟｰﾄﾞ5
                    strDat = strDat + strWAK.PadLeft(6) + ","                                       ' 6文字(左側に空白パディング)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblCutSpeed6.ToString("0.0")       ' ｶｯﾄｽﾋﾟｰﾄﾞ6
                    strDat = strDat + strWAK.PadLeft(6) + ","                                       ' 6文字(左側に空白パディング)

                    For i = 0 To (cCNDNUM - 1)                                                      ' 加工条件番号1～n(0ｵﾘｼﾞﾝ)
                        strWAK = typResistorInfoArray(Rn).ArrCut(Cn).CndNum(i).ToString("0")
                        strDat = strDat + strWAK.PadLeft(2) + ","                                   ' 2文字(左側に空白パディング)
                    Next i

                    '----- V2.0.0.0_23↓ -----
                    ' SL436S用(シンプルトリマ用)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblQRate3.ToString("0.0")          ' Qｽｲｯﾁﾚｰﾄ3
                    strDat = strDat + strWAK.PadLeft(5) + ","                                       ' 5文字(左側に空白パディング)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblQRate4.ToString("0.0")          ' Qｽｲｯﾁﾚｰﾄ4
                    strDat = strDat + strWAK.PadLeft(5) + ","                                       ' 5文字(左側に空白パディング)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblQRate5.ToString("0.0")          ' Qｽｲｯﾁﾚｰﾄ5
                    strDat = strDat + strWAK.PadLeft(5) + ","                                       ' 5文字(左側に空白パディング)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblQRate6.ToString("0.0")          ' Qｽｲｯﾁﾚｰﾄ6
                    strDat = strDat + strWAK.PadLeft(5) + ","                                       ' 5文字(左側に空白パディング)
                    '----- V2.0.0.0_23↑ -----

                    ' 目標パワーと許容範囲(Ver10.01で追加) ###066
                    For i = 0 To (cCNDNUM - 1)                                                      ' 加工条件番号1～n(0ｵﾘｼﾞﾝ)
                        '----- V2.0.0.0_23↓ -----
                        ' SL436S用(シンプルトリマ用)
                        strWAK = typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(i).ToString("0")     ' 電流値1～8
                        strDat = strDat + strWAK.PadLeft(6) + ","                                   ' 6文字(左側に空白パディング)
                        strWAK = typResistorInfoArray(Rn).ArrCut(Cn).FLSteg(i).ToString("0")        ' STEG1～8
                        strDat = strDat + strWAK.PadLeft(3) + ","                                   ' 3文字(左側に空白パディング)
                        '----- V2.0.0.0_23↑ -----
                        strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(i).ToString("0.00") ' ###071
                        strDat = strDat + strWAK.PadLeft(6) + ","                                   ' 6文字(左側に空白パディング)
                        strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustToleLevel(i).ToString("0.00")
                        strDat = strDat + strWAK.PadLeft(6) + ","                                   ' 6文字(左側に空白パディング)
                    Next i

                    ' 文字列データ
                    strDat = strDat + typResistorInfoArray(Rn).ArrCut(Cn).strChar + ","             ' 文字列
                    strDat = strDat + typResistorInfoArray(Rn).ArrCut(Cn).strDataName + ","         ' Uカットデータ名※追加 

                    writer.WriteLine(strDat)
                Next Cn
            Next Rn

            Return (cFRS_NORMAL)                                                                    ' Return値 = 正常

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "File.Put_CUT_Data() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                                                      ' Return値 = 例外エラー
        End Try
    End Function
#End Region
    '----- ###229↓ -----
#Region "[GPIB]データを書き込む【TKY,CHIP,NET統合版】"
    '''=========================================================================
    '''<summary>[GPIB]データを書き込む</summary>
    '''<param name="writer">(INP)StreamWriterオブジェクト</param>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function Put_GPIB_Data(ByVal writer As System.IO.StreamWriter) As Integer

        Dim strMSG As String

        Try
            ' [GPIB]データを書き込む
            writer.WriteLine(FILE_CONST_GPIB_DATA)                      ' "[GPIB]"
            With typGpibInfo
                writer.WriteLine(.wGPIBmode.ToString("0"))              ' GP-IB制御(0:しない 1:する)
                writer.WriteLine(.wDelim.ToString("0"))                 ' ﾃﾞﾘﾐﾀ(0:CR+LF 1:CR 2:LF 3:NONE)
                writer.WriteLine(.wTimeout.ToString("0"))               ' ﾀｲﾑｱｳﾄ(1～32767)(ms単位)
                writer.WriteLine(.wAddress.ToString("0"))               ' 機器ｱﾄﾞﾚｽ(0～30)
                writer.WriteLine(.wEOI.ToString("0"))                   ' EOI(0:使用しない, 1:使用する)
                writer.WriteLine(.wPause1.ToString("0"))                ' 設定ｺﾏﾝﾄﾞ1送信後ポーズ時間(1～32767msec)
                writer.WriteLine(.wPause2.ToString("0"))                ' 設定ｺﾏﾝﾄﾞ2送信後ポーズ時間(1～32767msec)
                writer.WriteLine(.wPause3.ToString("0"))                ' 設定ｺﾏﾝﾄﾞ3送信後ポーズ時間(1～32767msec)
                writer.WriteLine(.wPauseT.ToString("0"))                ' ﾄﾘｶﾞｺﾏﾝﾄﾞ送信後ポーズ時間(1～32767msec)
                writer.WriteLine(.wRev.ToString("0"))                   ' 予備

                writer.WriteLine(.strI)                                 ' 初期化ｺﾏﾝﾄﾞ(MAX40byte)
                writer.WriteLine(.strI2)                                ' 初期化ｺﾏﾝﾄﾞ2(MAX40byte)
                writer.WriteLine(.strI3)                                ' 初期化ｺﾏﾝﾄﾞ3(MAX40byte)
                writer.WriteLine(.strT)                                 ' ﾄﾘｶﾞｺﾏﾝﾄﾞ(50byte)
                writer.WriteLine(.strName)                              ' 機器名(10byte)
                writer.WriteLine(.wReserve)                             ' 予備(8byte) 
            End With

            Return (cFRS_NORMAL)                                        ' Return値 = 正常

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "File.Put_GPIB_Data() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return値 = 例外エラー
        End Try
    End Function
#End Region
    '----- ###229↑ -----

#Region "[SINSYUKU]データを書き込む【TKY,CHIP,NET統合版】"
    '''=========================================================================
    '''<summary>[SINSYUKU]データを書き込む</summary>
    '''<param name="writer">(INP)StreamWriterオブジェクト</param>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function Put_SINSYUKU_Data(ByVal writer As System.IO.StreamWriter) As Integer

        Dim strMSG As String
        Dim x As Integer
        Dim y As Integer
        Dim writeCount As Integer
        Dim writeStr As String

        Try
            writeCount = 0
            writeStr = ""
            ' [SINSYUKU]データを書き込む
            'V5.0.0.8①            writer.WriteLine(FILE_SINSYUKU_SELECT)                      ' "[SINSYUKU]"
            writer.WriteLine(FILE_CONST_SHINSYUKUDATA)                      ' "[SINSYUKU]"
            For x = 0 To typPlateInfo.intBlockCntXDir - 1
                For y = 0 To typPlateInfo.intBlockCntYDir - 1
                    If SelectBlock(x, y) <> 0 Then
                        If writeStr <> "" And Right(writeStr, 1) <> "," Then
                            writeStr = writeStr + ","
                        End If
                        writeStr = writeStr + CStr(x + 1) + "-" + CStr(y + 1)
                        writeCount = writeCount + 1
                        If writeCount > 10 Then
                            writer.WriteLine(writeStr)
                            writeCount = 0
                            writeStr = ""
                        End If
                    End If
                Next y
            Next x
            If writeCount <= 10 Then     ' まだ書き込んでいないデータがある場合ここで書き出す
                writer.WriteLine(writeStr)
            End If

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "File.Put_SINSYUKU_Data() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return値 = 例外エラー
        End Try
    End Function
#End Region
    '----- ###229↑ -----
#End If
#End Region 'V5.0.0.8①

#End Region

    '======================================================================
    '  共通関数
    '======================================================================
#Region "【共通関数】"
#Region "ファイルバージョン取得【共通】"
    '''=========================================================================
    '''<summary>ファイルバージョン取得【共通】</summary>
    '''<param name="strPath">(INP)ファイル名</param>
    '''<param name="dblVer"> (OUT)ファイルバージョン名</param> 
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Public Function File_Read_Ver(ByVal strPath As String, ByRef dblVer As Double) As Integer

        'Dim intFileNo As Integer                                    ' ファイル番号
        'Dim iFlg As Integer
        Dim r As Integer
        Dim intType As Integer
        Dim Pos As Integer
        Dim strDAT As String
        Dim strVer As String
        Dim strMSG As String

        Try
            ' 初期処理
            r = cFRS_FIOERR_INP                                     ' Return値 = エラー
            intType = -1                                            ' データ種別
            FILETYPE_K = ""                                         ' SL436Kのファイルバージョン V1.23.0.0⑧

            If (False = IO.File.Exists(strPath)) Then Throw New FileNotFoundException()

            Using sr As New StreamReader(strPath, Encoding.UTF8)    ' ﾌｧｲﾙﾊﾞｰｼﾞｮﾝの部分ASCIIなのでUTF8でOK     V4.4.0.0-1
                ' ファイルの終端までループを繰り返します。
                Do While (False = sr.EndOfStream)
                    strDAT = sr.ReadLine()
                    Select Case strDAT
                        Case CONST_VERSION                              ' ファイルバージョン
                            intType = 0

                        Case Else
                            If (intType = 0) Then                       '[FILE VERSION]データ ?
                                ' "TKYDATA_Ver4_SP1"なら"TKYDATA_Ver4"とする
                                If (strDAT = CONST_FILETYPE4_SP1) Then
                                    strDAT = CONST_FILETYPE4
                                End If
                                '----- V1.23.0.0⑧↓ -----
                                '----- CHIP/NET用(SL436K) -----
                                If (strDAT = FILETYPE04_K) Then
                                    FILETYPE_K = FILETYPE04_K
                                End If
                                '----- V1.23.0.0⑧↑ -----
                                ' ファイルバージョン名をdouble型に変更する
                                Pos = strDAT.IndexOf("Ver")             ' "Ver"を見つける。なければエラー戻り
                                If (Pos = -1) Then Exit Do
                                strVer = strDAT.Substring(Pos + 3, strDAT.Length - (Pos + 3))
                                '----- V1.14.0.0⑥↓ -----
                                If (strVer = "7.0.0.2") Then            ' V7.0.0.2なら 
                                    dblVer = 7.02                       ' V7.02とする 
                                Else
                                    dblVer = Double.Parse(strVer)       ' ファイルバージョン名をdouble型に変更する
                                End If
                                'dblVer = Double.Parse(strVer)          ' ファイルバージョン名をdouble型に変更する
                                '----- V1.14.0.0⑥↑ -----
                                r = cFRS_NORMAL                         ' Return値 = 正常
                                Exit Try
                            Else
                                Exit Do
                            End If
                    End Select
                Loop
            End Using

            ' "指定されたファイルはトリミングパラメータのデータではありません"
            Call Form1.System1.TrmMsgBox(gSysPrm, MSG_16, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, gAppName)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "File.File_Read_Ver() TRAP ERROR = " & ex.Message
            MsgBox(strMSG)
            r = cERR_TRAP                                           ' Return値 = 例外エラー
            'GoTo STP_EXT
        End Try

        Return (r)

#If False Then
        Try
            ' 初期処理
            r = cFRS_FIOERR_INP                                     ' Return値 = エラー
            intType = -1                                            ' データ種別
            FILETYPE_K = ""                                         ' SL436Kのファイルバージョン V1.23.0.0⑧
            iFlg = 0
            ' テキストファイルをオープン
            intFileNo = FreeFile()                                  ' 使用可能なファイルナンバーを取得
            FileOpen(intFileNo, strPath, OpenMode.Input)
            iFlg = 1

            ' ファイルの終端までループを繰り返します。
            Do While Not EOF(intFileNo)
                strDAT = LineInput(intFileNo)                       ' 1行読み込み
                Select Case strDAT
                    Case CONST_VERSION                              ' ファイルバージョン
                        intType = 0

                    Case Else
                        If (intType = 0) Then                       '[FILE VERSION]データ ?
                            ' "TKYDATA_Ver4_SP1"なら"TKYDATA_Ver4"とする
                            If (strDAT = CONST_FILETYPE4_SP1) Then
                                strDAT = CONST_FILETYPE4
                            End If
                            '----- V1.23.0.0⑧↓ -----
                            '----- CHIP/NET用(SL436K) -----
                            If (strDAT = FILETYPE04_K) Then
                                FILETYPE_K = FILETYPE04_K
                            End If
                            '----- V1.23.0.0⑧↑ -----
                            ' ファイルバージョン名をdouble型に変更する
                            Pos = strDAT.IndexOf("Ver")             ' "Ver"を見つける。なければエラー戻り
                            If (Pos = -1) Then Exit Do
                            strVer = strDAT.Substring(Pos + 3, strDAT.Length - (Pos + 3))
                            '----- V1.14.0.0⑥↓ -----
                            If (strVer = "7.0.0.2") Then            ' V7.0.0.2なら 
                                dblVer = 7.02                       ' V7.02とする 
                            Else
                                dblVer = Double.Parse(strVer)       ' ファイルバージョン名をdouble型に変更する
                            End If
                            'dblVer = Double.Parse(strVer)          ' ファイルバージョン名をdouble型に変更する
                            '----- V1.14.0.0⑥↑ -----
                            r = cFRS_NORMAL                         ' Return値 = 正常
                            GoTo STP_EXT
                        End If
                End Select
            Loop

            ' "指定されたファイルはトリミングパラメータのデータではありません"
            Call Form1.System1.TrmMsgBox(gSysPrm, MSG_16, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, gAppName)

            ' 終了処理
STP_EXT:
            If (iFlg = 1) Then
                FileClose(intFileNo)
            End If
            Return (r)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "File.File_Read_Ver() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            r = cERR_TRAP                                           ' Return値 = 例外エラー
            GoTo STP_EXT
        End Try
#End If
    End Function
#End Region

#Region "LOAD・SAVEの共通化により未使用"
#If False Then 'V5.0.0.8①
#Region "ファイルバージョンのチェック【TKY用】"
    '''=========================================================================
    '''<summary>ファイルバージョンのチェック【TKY用】</summary>
    '''<param name="strFileVer">(INP) バージョン</param>
    '''<returns>0=正常, 1=エラー </returns>
    '''=========================================================================
    Private Function FileVerCheck_TKY(ByRef strFileVer As String) As Short

        Dim r As Short
        Dim strMSG As String

        Try
            r = cFRS_NORMAL                                         ' Return値 = 正常
            Select Case strFileVer

                Case CONST_FILETYPE4
                Case CONST_FILETYPE4_SP1
                    SL432HW_FileVer = 4

                Case CONST_FILETYPE5
                    SL432HW_FileVer = 5

                Case CONST_FILETYPE10
                    SL432HW_FileVer = 10

                Case CONST_FILETYPE10_01
                    SL432HW_FileVer = 10.01                         ' ###229

                Case CONST_FILETYPE10_02
                    SL432HW_FileVer = 10.02                         ' ###229

                Case CONST_FILETYPE10_03
                    SL432HW_FileVer = 10.03                         ' ###229

                Case CONST_FILETYPE10_04                            'V1.14.0.0⑥
                    SL432HW_FileVer = 10.04

                Case CONST_FILETYPE10_05                            'V1.16.0.0①
                    SL432HW_FileVer = 10.05

                Case CONST_FILETYPE10_06                            'V1.18.0.0④
                    SL432HW_FileVer = 10.06

                    '----- V2.0.0.0_23↓ -----
                Case CONST_FILETYPE10_07
                    SL432HW_FileVer = 10.07

                Case CONST_FILETYPE10_072
                    SL432HW_FileVer = 10.072

                Case CONST_FILETYPE10_073
                    SL432HW_FileVer = 10.073
                    '----- V2.0.0.0_23↑ -----

                Case CONST_FILETYPE10_09                            ' V4.0.0.0④
                    SL432HW_FileVer = 10.09

                Case CONST_FILETYPE10_10                            'V4.10.0.0④
                    SL432HW_FileVer = FILE_VER_10_10

                Case CONST_FILETYPE_CUR                             ' ###066
                    SL432HW_FileVer = FILE_VER_CUR

                Case Else
                    r = cFRS_FIOERR_INP                             ' Return値 = NG
            End Select
            Return (r)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "File.FileVerCheck_TKY() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                      ' Return値 = 例外エラー
        End Try
    End Function
#End Region
#End If
#End Region 'V5.0.0.8①

#Region "ファイルバージョンのチェック【CHIP用】"
    '''=========================================================================
    '''<summary>ファイルバージョンのチェック【CHIP用】</summary>
    '''<param name="strFileVer">(INP) バージョン</param>
    '''<returns>0=正常, 1=エラー </returns>
    '''=========================================================================
    Private Function FileVerCheck_CHIP(ByRef strFileVer As String) As Short

        FileVerCheck_CHIP = cFRS_NORMAL                                 ' Return値 = 正常
        Select Case strFileVer
            Case FILETYPE_CHIP01
                FileIO.FileVersion = 1
                '----- V6.1.4.0_44↓(KOA EW殿SL432RD対応) -----
                ' メッセージ表示なし ?
                If (giFileMsgNoDsp = 1) Then Return (cFRS_NORMAL)
                '----- V6.1.4.0_44↑ -----
                'If gSysPrm.stTMN.giMsgTyp = 0 Then
                '    MsgBox("ファイル構成が変更されています。" & vbCrLf & "下記に示す項目を確認後、SAVEを行ってください。" & vbCrLf & vbCrLf & Space(2) & "【プレートデータ】" & vbCrLf & Space(4) & "・θ軸" & vbCrLf & Space(4) & "・LED制御" & vbCrLf & Space(4) & "・GP-IB制御" & vbCrLf & Space(2) & "【グループデータ】" & vbCrLf & Space(4) & "・抵抗数" & vbCrLf & Space(4) & "・グループ間インターバル", MsgBoxStyle.OkOnly)
                'Else
                '    MsgBox("File constitution is changed." & vbCrLf & "Please do SAVE after confirmation in an item to show as follows." & vbCrLf & vbCrLf & Space(2) & "[PLATE DATA]" & vbCrLf & Space(4) & "* THETA" & vbCrLf & Space(4) & "* LED Control" & vbCrLf & Space(4) & "* GP-IB" & vbCrLf & Space(2) & "[GROUP DATA]" & vbCrLf & Space(4) & "* Resistor Number" & vbCrLf & Space(4) & "* Group Interval", MsgBoxStyle.OkOnly)
                'End If
                MsgBox(File_005 & vbCrLf & File_006 & vbCrLf & vbCrLf & Space(2) & File_007 & vbCrLf & Space(4) & File_008 & vbCrLf & Space(4) & File_009 & vbCrLf & Space(4) & File_010 & vbCrLf & Space(2) & File_011 & vbCrLf & Space(4) & File_012 & vbCrLf & Space(4) & File_013, MsgBoxStyle.OkOnly)

            Case FILETYPE_CHIP02
                FileIO.FileVersion = 2
                '----- V6.1.4.0_44↓(KOA EW殿SL432RD対応) -----
                ' メッセージ表示なし ?
                If (giFileMsgNoDsp = 1) Then Return (cFRS_NORMAL)
                '----- V6.1.4.0_44↑ -----
                'If gSysPrm.stTMN.giMsgTyp = 0 Then
                '    MsgBox("ファイル構成が変更されています。" & vbCrLf & "下記に示す項目を確認後、SAVEを行ってください。" & vbCrLf & vbCrLf & Space(2) & "【プレートデータ】" & vbCrLf & Space(4) & "・θ軸" & vbCrLf & Space(4) & "・GP-IB制御" & vbCrLf & Space(2) & "【グループデータ】" & vbCrLf & Space(4) & "・抵抗数" & vbCrLf & Space(4) & "・グループ間インターバル", MsgBoxStyle.OkOnly)
                'Else
                '    MsgBox("File constitution is changed." & vbCrLf & "Please do SAVE after confirmation in an item to show as follows." & vbCrLf & vbCrLf & Space(2) & "[PLATE DATA]" & vbCrLf & Space(4) & "* THETA" & vbCrLf & Space(4) & "* GP-IB" & vbCrLf & Space(2) & "[GROUP DATA]" & vbCrLf & Space(4) & "* Resistor Number" & vbCrLf & Space(4) & "* Group Interval", MsgBoxStyle.OkOnly)
                'End If
                MsgBox(File_005 & vbCrLf & File_006 & vbCrLf & vbCrLf & Space(2) & File_007 & vbCrLf & Space(4) & File_008 & vbCrLf & Space(4) & File_010 & vbCrLf & Space(2) & File_011 & vbCrLf & Space(4) & File_012 & vbCrLf & Space(4) & File_013, MsgBoxStyle.OkOnly)

            Case FILETYPE_CHIP03
                FileIO.FileVersion = 3
                '----- V6.1.4.0_44↓(KOA EW殿SL432RD対応) -----
                ' メッセージ表示なし ?
                If (giFileMsgNoDsp = 1) Then Return (cFRS_NORMAL)
                '----- V6.1.4.0_44↑ -----
                'If gSysPrm.stTMN.giMsgTyp = 0 Then
                '    MsgBox("ファイル構成が変更されています。" & vbCrLf & "下記に示す項目を確認後、SAVEを行ってください。" & vbCrLf & vbCrLf & Space(2) & "【プレートデータ】" & vbCrLf & Space(4) & "・θ軸" & vbCrLf & Space(4) & "・GP-IB制御", MsgBoxStyle.OkOnly)
                'Else
                '    MsgBox("File constitution is changed." & vbCrLf & "Please do SAVE after confirmation in an item to show as follows." & vbCrLf & vbCrLf & Space(2) & "[PLATE DATA]" & vbCrLf & Space(4) & "* THETA" & vbCrLf & Space(4) & "* GP-IB", MsgBoxStyle.OkOnly)
                'End If
                MsgBox(File_005 & vbCrLf & File_006 & vbCrLf & vbCrLf & Space(2) & File_007 & vbCrLf & Space(4) & File_008 & vbCrLf & Space(4) & File_010, MsgBoxStyle.OkOnly)

            Case FILETYPE_CHIP04
                FileIO.FileVersion = 4
                '----- V6.1.4.0_44↓(KOA EW殿SL432RD対応) -----
                ' メッセージ表示なし ?
                If (giFileMsgNoDsp = 1) Then Return (cFRS_NORMAL)
                '----- V6.1.4.0_44↑ -----
                'If gSysPrm.stTMN.giMsgTyp = 0 Then
                '    MsgBox("ファイル構成が変更されています。" & vbCrLf & "下記に示す項目を確認後、SAVEを行ってください。" & vbCrLf & vbCrLf & Space(2) & "【プレートデータ】" & vbCrLf & Space(4) & "・GP-IB制御", MsgBoxStyle.OkOnly)
                'Else
                '    MsgBox("File constitution is changed." & vbCrLf & "Please do SAVE after confirmation in an item to show as follows." & vbCrLf & vbCrLf & Space(2) & "[PLATE DATA]" & vbCrLf & Space(4) & "* GP-IB", MsgBoxStyle.OkOnly)
                'End If
                MsgBox(File_005 & vbCrLf & File_006 & vbCrLf & vbCrLf & Space(2) & File_007 & vbCrLf & Space(4) & File_010, MsgBoxStyle.OkOnly)

            Case FILETYPE_CHIP05
                FileIO.FileVersion = 5 ' V1.40
                '----- V6.1.4.0_44↓(KOA EW殿SL432RD対応) -----
                ' メッセージ表示なし ? V6.1.4.0⑫
                If (giFileMsgNoDsp = 1) Then Return (cFRS_NORMAL)
                '----- V6.1.4.0_44↑ -----
                ' KOA(EW)殿ならﾒｯｾｰｼﾞ表示なし
                If (gSysPrm.stCTM.giSPECIAL = customKOAEW) Then
                Else
                    'If gSysPrm.stTMN.giMsgTyp = 0 Then
                    '    MsgBox("ファイル構成が変更されています。" & vbCrLf & "下記に示す項目を確認後、SAVEを行ってください。" & vbCrLf & vbCrLf & Space(2) & "【プレートデータ】" & vbCrLf & Space(4) & "・Ｔθオフセット" & vbCrLf & Space(4) & "・Ｔθ基準位置１XY" & vbCrLf & Space(4) & "・Ｔθ基準位置２XY" & vbCrLf & Space(4) & "・パワー調整モード" & vbCrLf & Space(4) & "・調整目標パワー" & vbCrLf & Space(4) & "・パワー調整 Ｑレート" & vbCrLf & Space(4) & "・パワー調整許容範囲" & vbCrLf & Space(2) & "【カットデータ】" & vbCrLf & Space(4) & "・カットオフ オフセット", MsgBoxStyle.OkOnly)
                    'Else
                    '    MsgBox("File constitution is changed." & vbCrLf & "Please do SAVE after confirmation in an item to show as follows." & vbCrLf & vbCrLf & Space(2) & "[PLATE DATA]" & vbCrLf & Space(4) & "* T_THETA OFFSET" & vbCrLf & Space(4) & "* T_THETA ADJUST POINT1XY" & vbCrLf & Space(4) & "* T_THETA ADJUST POINT2XY" & vbCrLf & Space(4) & "* POWER ADJUSTMENT MODE" & vbCrLf & Space(4) & "* ADJUSTMENT POWER" & vbCrLf & Space(4) & "* POWER ADJUSTMENT QRATE" & vbCrLf & Space(4) & "* ADJUSTMENT TOLERANCE" & vbCrLf & Space(2) & "[CUT DATA]" & vbCrLf & Space(4) & "* CUTOFF OFFSET", MsgBoxStyle.OkOnly)
                    'End If
                    MsgBox(File_005 & vbCrLf & File_006 & vbCrLf & vbCrLf & Space(2) & File_007 & vbCrLf & Space(4) & File_014 & vbCrLf & Space(4) & File_015 & vbCrLf & Space(4) & File_016 & vbCrLf & Space(4) & File_017 & vbCrLf & Space(4) & File_018 & vbCrLf & Space(4) & File_019 & vbCrLf & Space(4) & File_020 & vbCrLf & Space(2) & File_021 & vbCrLf & Space(4) & File_022, MsgBoxStyle.OkOnly)
                End If

                ' ﾌｧｲﾙ構成変更Vol.6
            Case FILETYPE_CHIP06
                FileIO.FileVersion = 6
                '----- V6.1.4.0_44↓(KOA EW殿SL432RD対応) -----
                ' メッセージ表示なし ?
                If (giFileMsgNoDsp = 1) Then Return (cFRS_NORMAL)
                '----- V6.1.4.0_44↑ -----
                'If gSysPrm.stTMN.giMsgTyp = 0 Then
                '    MsgBox("ファイル構成が変更されています。" & vbCrLf & "下記に示す項目を確認後、SAVEを行ってください。" & vbCrLf & vbCrLf & Space(2) & "【プレートデータ 2】" & vbCrLf & Space(4) & "・補正位置パターンのグループ番号", MsgBoxStyle.OkOnly)
                'Else
                '    MsgBox("File constitution is changed." & vbCrLf & "Please do SAVE after confirmation in an item to show as follows." & vbCrLf & vbCrLf & Space(2) & "[PLATE DATA 2]" & vbCrLf & Space(4) & "* GroupNo. for revise pattern.", MsgBoxStyle.OkOnly)
                'End If
                MsgBox(File_005 & vbCrLf & File_006 & vbCrLf & vbCrLf & Space(2) & File_023 & vbCrLf & Space(4) & File_024, MsgBoxStyle.OkOnly)

                '----- V1.14.0.0⑥↓ -----
                ' ﾌｧｲﾙﾊﾞｰｼﾞｮﾝV7.0.0.2
            Case FILETYPE_CHIP07_02
                FileIO.FileVersion = 7.02
                '----- V6.1.4.0_44↓(KOA EW殿SL432RD対応) -----
                ' メッセージ表示なし ?
                If (giFileMsgNoDsp = 1) Then Return (cFRS_NORMAL)
                '----- V6.1.4.0_44↑ -----
                'If gSysPrm.stTMN.giMsgTyp = 0 Then
                '    MsgBox("ファイル構成が変更されています。" & vbCrLf & "項目確認後、SAVEを行ってください。", MsgBoxStyle.OkOnly)
                'Else
                '    MsgBox("File constitution is changed." & vbCrLf & "Please do SAVE after confirmation.", MsgBoxStyle.OkOnly)
                'End If
                MsgBox(File_005 & vbCrLf & File_025, MsgBoxStyle.OkOnly)
                '----- V1.14.0.0⑥↑ -----

#If False Then  'V5.0.0.8① FILE_VER_10 以降は Form1.Sub_FileLoad() → File_Read() で処理される
            Case FILETYPE_CHIP10
                SL432HW_FileVer = FILE_VER_10

            Case FILETYPE_CHIP10_01
                SL432HW_FileVer = FILE_VER_10_01

            Case FILETYPE_CHIP10_02
                SL432HW_FileVer = FILE_VER_10_02

            Case FILETYPE_CHIP10_03
                SL432HW_FileVer = FILE_VER_10_03

            Case FILETYPE_CHIP10_04                              'V1.15.0.0①
                SL432HW_FileVer = FILE_VER_10_04                     'V1.15.0.0①

            Case FILETYPE_CHIP10_05                              'V1.16.0.0①
                SL432HW_FileVer = FILE_VER_10_05                     'V1.16.0.0①

            Case FILETYPE_CHIP10_06                              'V1.18.0.0④
                SL432HW_FileVer = FILE_VER_10_06                     'V1.18.0.0④
                '----- V2.0.0.0_23↓ -----
            Case FILETYPE_CHIP10_07
                SL432HW_FileVer = FILE_VER_10_07
            Case FILETYPE_CHIP10_072
                SL432HW_FileVer = FILE_VER_10_072
            Case FILETYPE_CHIP10_073
                SL432HW_FileVer = FILE_VER_10_073
                '----- V2.0.0.0_23↑ -----
            Case FILETYPE_CHIP10_08
                SL432HW_FileVer = FILE_VER_10_08

            Case FILETYPE_CHIP10_09                             'V4.0.0.0④
                SL432HW_FileVer = FILE_VER_10_09

            Case FILETYPE_CHIP10_10                              'V4.10.0.0④
                SL432HW_FileVer = FILE_VER_10_10

            Case FILETYPE_CHIP10_11
                SL432HW_FileVer = FILE_VER_10_11

            Case FILETYPE_CHIP_CUR                               '###066
                SL432HW_FileVer = FILE_VER_CUR
#End If
                '----- V1.23.0.0⑧↓ -----
                '----- CHIP/NET用(SL436K) -----
            Case FILETYPE04_K                               ' TKYCHIP_SL436K_Ver1.20 
                FileIO.FileVersion = 1.2
                '----- V1.23.0.0⑧↑ -----

            Case Else
                'mouse pointer default
                Call SetMousePointer(Form1, False)
                '' "指定されたファイルはトリミングパラメータのデータではありません"
                'Call Form1.System1.TrmMsgBox(gSysPrm, MSG_16, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, gAppName)
                FileVerCheck_CHIP = 1
        End Select

    End Function
#End Region

#Region "ファイルバージョンのチェック【NET用】"
    '''=========================================================================
    '''<summary>ファイルバージョンのチェック【NET用】</summary>
    '''<param name="strFileVer">(INP) バージョン</param>
    '''<returns>0=正常, 1=エラー </returns>
    '''=========================================================================
    Private Function FileVerCheck_NET(ByVal strFileVer As String) As Integer

        FileVerCheck_NET = cFRS_NORMAL                                  ' Return値 = 正常
        Select Case strFileVer
            Case FILETYPE_NET01
                FileIO.FileVersion = 1
                '----- V6.1.4.0_44↓(KOA EW殿SL432RD対応) -----
                ' メッセージ表示なし ?
                If (giFileMsgNoDsp = 1) Then Return (cFRS_NORMAL)
                '----- V6.1.4.0_44↑ -----
                ' MSG select
                'If gSysPrm.stTMN.giMsgTyp = 0 Then
                '    MsgBox("ファイル構成が変更されています。" & vbCrLf & "下記に示す項目を確認後、SAVEを行ってください。" & vbCrLf & vbCrLf & _
                '                           Space(2) & "【プレートデータ】" & vbCrLf & _
                '                           Space(4) & "・グループ数ＸＹ", vbOKOnly)
                'Else
                '    MsgBox("File constitution is changed." & vbCrLf & "Please do SAVE after confirmation in an item to show as follows." & _
                '                           vbCrLf & vbCrLf & Space(2) & "[PLATE DATA]" & vbCrLf & Space(4) & "* GROUP NUMBER XY", vbOKOnly)
                'End If
                MsgBox(File_005 & vbCrLf & File_006 & vbCrLf & vbCrLf & _
                                       Space(2) & File_007 & vbCrLf & _
                                       Space(4) & File_026, vbOKOnly)

            Case FILETYPE_NET02
                FileIO.FileVersion = 2
                '----- V6.1.4.0_44↓(KOA EW殿SL432RD対応) -----
                ' メッセージ表示なし ?
                If (giFileMsgNoDsp = 1) Then Return (cFRS_NORMAL)
                '----- V6.1.4.0_44↑ -----
                ' MSG select
                'If gSysPrm.stTMN.giMsgTyp = 0 Then
                '    MsgBox("ファイル構成が変更されています。" & vbCrLf & "下記に示す項目を確認後、SAVEを行ってください。" & _
                '                           vbCrLf & vbCrLf & Space(2) & "【プレートデータ】" & vbCrLf & Space(4) & "・θ軸", vbOKOnly)
                'Else
                '    MsgBox("File constitution is changed." & vbCrLf & "Please do SAVE after confirmation in an item to show as follows." & _
                '                           vbCrLf & vbCrLf & Space(2) & "[PLATE DATA]" & vbCrLf & Space(4) & "* THETA", vbOKOnly)
                'End If
                MsgBox(File_005 & vbCrLf & File_006 & _
                                       vbCrLf & vbCrLf & Space(2) & File_007 & vbCrLf & Space(4) & File_008, vbOKOnly)

            Case FILETYPE_NET03
                FileIO.FileVersion = 3
                '----- V6.1.4.0_44↓(KOA EW殿SL432RD対応) -----
                ' メッセージ表示なし ?
                If (giFileMsgNoDsp = 1) Then Return (cFRS_NORMAL)
                '----- V6.1.4.0_44↑ -----
                ' MSG select
                'If gSysPrm.stTMN.giMsgTyp = 0 Then
                '    MsgBox("ファイル構成が変更されています。" & vbCrLf & "下記に示す項目を確認後、SAVEを行ってください。" & vbCrLf & vbCrLf & _
                '                            Space(2) & "【プレートデータ 2】" & vbCrLf & _
                '                            Space(4) & "・補正位置パターンのグループ番号", vbOKOnly)
                'Else
                '    MsgBox("File constitution is changed." & vbCrLf & "Please do SAVE after confirmation in an item to show as follows." & vbCrLf & vbCrLf & _
                '                            Space(2) & "[PLATE DATA 2]" & vbCrLf & _
                '                            Space(4) & "* GroupNo. for revise pattern.", vbOKOnly)
                'End If
                MsgBox(File_005 & vbCrLf & File_006 & vbCrLf & vbCrLf & _
                                        Space(2) & File_023 & vbCrLf & _
                                        Space(4) & File_024, vbOKOnly)

                '----- V1.14.0.0⑥↓ -----
            Case FILETYPE_NET07_02
                FileIO.FileVersion = 7.02
                '----- V6.1.4.0_44↓(KOA EW殿SL432RD対応) -----
                ' メッセージ表示なし ?
                If (giFileMsgNoDsp = 1) Then Return (cFRS_NORMAL)
                '----- V6.1.4.0_44↑ -----
                ' MSG select
                'If gSysPrm.stTMN.giMsgTyp = 0 Then
                '    MsgBox("ファイル構成が変更されています。" & vbCrLf & "項目を確認後、SAVEを行ってください。", vbOKOnly)
                'Else
                '    MsgBox("File constitution is changed." & vbCrLf & "Please do SAVE after confirmation.", vbOKOnly)
                'End If
                MsgBox(File_005 & vbCrLf & File_025, vbOKOnly)
                '----- V1.14.0.0⑥↑ -----

#If False Then  'V5.0.0.8① FILE_VER_10 以降は Form1.Sub_FileLoad() → File_Read() で処理される
            Case FILETYPE_NET10
                SL432HW_FileVer = FILE_VER_10

            Case FILETYPE_NET10_01
                SL432HW_FileVer = FILE_VER_10_01

            Case FILETYPE_NET10_02
                SL432HW_FileVer = FILE_VER_10_02

            Case FILETYPE_NET10_03
                SL432HW_FileVer = FILE_VER_10_03

            Case FILETYPE_NET10_04                              'V1.15.0.0①
                SL432HW_FileVer = FILE_VER_10_04                     'V1.15.0.0①

            Case FILETYPE_NET10_05                              'V1.16.0.0①
                SL432HW_FileVer = FILE_VER_10_05                     'V1.16.0.0①

            Case FILETYPE_NET10_06                              'V1.18.0.0④
                SL432HW_FileVer = FILE_VER_10_06                     'V1.18.0.0④
                '----- V2.0.0.0_23↓ -----
            Case FILETYPE_NET10_07
                SL432HW_FileVer = FILE_VER_10_07
            Case FILETYPE_NET10_072
                SL432HW_FileVer = FILE_VER_10_072
            Case FILETYPE_NET10_073
                SL432HW_FileVer = FILE_VER_10_073
                '----- V2.0.0.0_23↑ -----
            Case FILETYPE_NET10_08
                SL432HW_FileVer = FILE_VER_10_08

            Case FILETYPE_NET10_09                              'V4.0.0.0④
                SL432HW_FileVer = FILE_VER_10_09                     'V4.0.0.0④

            Case FILETYPE_NET10_10                              'V4.10.0.0④
                SL432HW_FileVer = FILE_VER_10_10

            Case FILETYPE_NET10_11
                SL432HW_FileVer = FILE_VER_10_11

            Case FILETYPE_NET_CUR                               '###066
                SL432HW_FileVer = FILE_VER_CUR
#End If
            Case Else
                ' mouse pointer default
                Call SetMousePointer(Form1, False)
                '' "指定されたファイルはトリミングパラメータのデータではありません"
                'Call Form1.System1.TrmMsgBox(gSysPrm, MSG_16, vbExclamation Or vbOKOnly, gAppName)
                FileVerCheck_NET = 1
        End Select
    End Function

#End Region

#Region "ﾏｳｽﾎﾟｲﾝﾀの砂時計表示(設定／解除)"
    '''=========================================================================
    '''<summary>ﾏｳｽﾎﾟｲﾝﾀの砂時計表示(設定／解除)</summary>
    '''<param name="frm"> (INP) 対象ﾌｫｰﾑ</param>
    '''<param name="mode">(INP) True(実行), False(解除)</param>
    '''=========================================================================
    Public Sub SetMousePointer(ByRef frm As System.Windows.Forms.Form, ByRef mode As Boolean)

        ' mode check
        If mode = True Then
            ' 砂時計
            frm.Enabled = False
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        ElseIf mode = False Then
            ' default
            frm.Enabled = True
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        End If
    End Sub
#End Region

#Region "カット方向から斜めカットの切り出し角度を設定する【共通】"
    '''=========================================================================
    '''<summary>カット方向から斜めカットの切り出し角度を設定する【共通】</summary>
    '''<param name="CutType">(INP)カット形状 </param>
    '''<param name="Dir">    (INP)カット方向 </param>
    '''<param name="Angle">  (OUT)斜めカットの切り出し角度</param>
    '''=========================================================================
    Private Sub GetCutAngle(ByRef CutType As String, ByVal Dir As Short, ByRef Angle As Short)

        Dim strMSG As String

        Try
            Select Case (CutType)
                ' 斜めカットの場合は設定しない
                Case CNS_CUTP_NST                                                           ' 斜めSTカット 
                Case CNS_CUTP_NL                                                            ' 斜めLカット
                Case CNS_CUTP_NSTr                                                          ' 斜めSTカット(リターン)  
                Case CNS_CUTP_NLr                                                           ' 斜めLカット(リターン)    
                Case CNS_CUTP_NSTt                                                          ' 斜めSTカット(リトレース)     
                Case CNS_CUTP_NLt                                                           ' 斜めLカット(リトレース)    

                Case CNS_CUTP_C                                                             ' Cカット(未サポート)  
                    strMSG = "File.GetCutAngle() Not Support Cut Type(C Cut)"
                    MessageBox.Show(strMSG, "", MessageBoxButtons.OK)
                '----- V6.1.4.0_49↓ -----
                'Case CNS_CUTP_ES                                                            ' 旧ESカット(未サポート)  
                '    strMSG = "File.GetCutAngle() Not Support Cut Type(ES Cut)"
                '    MessageBox.Show(strMSG, "", MessageBoxButtons.OK)
                '----- V6.1.4.0_49↑ -----
                Case CNS_CUTP_NOP                                                           ' NOP(未サポート)  
                    strMSG = "File.GetCutAngle() Not Support Cut Type(Z Cut)"
                    MessageBox.Show(strMSG, "", MessageBoxButtons.OK)

                'Case CNS_CUTP_ST, CNS_CUTP_IX, CNS_CUTP_SC, CNS_CUTP_STr, CNS_CUTP_STt, CNS_CUTP_M, CNS_CUTP_ES2, CNS_CUTP_ST2, CNS_CUTP_IX2               ' V6.1.4.0_49
                Case CNS_CUTP_ST, CNS_CUTP_IX, CNS_CUTP_SC, CNS_CUTP_STr, CNS_CUTP_STt, CNS_CUTP_M, CNS_CUTP_ES2, CNS_CUTP_ST2, CNS_CUTP_IX2, CNS_CUTP_ES   ' V6.1.4.0_49
                    ' STカット, スキャンカット他
                    ' カット方向から斜めカットの切り出し角度を設定する 
                    Select Case (Dir)
                        Case 1      ' +X → 0°
                            Angle = 0
                        Case 2      ' -Y → 270°
                            Angle = 270
                        Case 3      ' -X → 180°
                            Angle = 180
                        Case 4      ' +Y → 90°
                            Angle = 90
                    End Select

                Case Else

            End Select

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "File.GetCutAngle() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "カット方向から斜めカットの切り出し角度とLターン方向を設定する【共通】"
    '''=========================================================================
    '''<summary>カット方向から斜めカットの切り出し角度とLターン方向を設定する【共通】</summary>
    '''<param name="CutType"> (INP)カット形状 </param>
    '''<param name="Dir">     (INP)カット方向 </param>
    '''<param name="Angle">   (OUT)斜めカットの切り出し角度</param>
    '''<param name="LTurnDir">(OUT)Lﾀｰﾝ方向(1:CW, 2:CCW)</param>
    '''=========================================================================
    Private Sub GetCutLTurnDir(ByRef CutType As String, ByVal Dir As Short, ByRef Angle As Short, ByRef LTurnDir As Short)

        Dim strMSG As String

        Try
            Select Case (CutType)
                Case CNS_CUTP_L, CNS_CUTP_HK, CNS_CUTP_Lr, CNS_CUTP_Lt, CNS_CUTP_U, CNS_CUTP_Ut 'V1.22.0.0②
                    ' Lカット/HOOKカット/uカット
                    ' カット方向から斜めカットの切り出し角度とLﾀｰﾝ方向(1:CW, 2:CCW)を設定する 
                    Select Case (Dir)
                        Case 1                                          ' +X+Y  
                            Angle = 0                                   ' 切り出し角度 = 0°
                            LTurnDir = 2                                ' Lﾀｰﾝ方向     = 反時計方向 
                        Case 2                                          ' -Y+X 
                            Angle = 270                                 ' 切り出し角度 = 0°
                            LTurnDir = 2                                ' Lﾀｰﾝ方向     = 反時計方向 
                        Case 3                                          ' -X-Y
                            Angle = 180                                 ' 切り出し角度 = 0°
                            LTurnDir = 2                                ' Lﾀｰﾝ方向     = 反時計方向 
                        Case 4                                          ' +Y-X
                            Angle = 90                                  ' 切り出し角度 = 0°
                            LTurnDir = 2                                ' Lﾀｰﾝ方向     = 反時計方向 
                        Case 5                                          ' +X-Y
                            Angle = 0                                   ' 切り出し角度 = 0°
                            LTurnDir = 1                                ' Lﾀｰﾝ方向     = 時計方向 
                        Case 6                                          ' -Y-X
                            Angle = 270                                 ' 切り出し角度 = 0°
                            LTurnDir = 1                                ' Lﾀｰﾝ方向     = 時計方向 
                        Case 7                                          ' -X+Y
                            Angle = 180                                 ' 切り出し角度 = 0°
                            LTurnDir = 1                                ' Lﾀｰﾝ方向     = 時計方向 
                        Case 8                                          ' +Y+X
                            Angle = 90                                  ' 切り出し角度 = 0°
                            LTurnDir = 1                                ' Lﾀｰﾝ方向     = 時計方向 
                    End Select

                Case CNS_CUTP_NL, CNS_CUTP_NLr, CNS_CUTP_NLt
                    ' 斜めLカット
                    LTurnDir = Dir                                      ' Lﾀｰﾝ方向     = カット方向(1:CW, 2:CCW)
                Case Else

            End Select

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "File.GetCutLTurnDir() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "ステップ方向を変換する【共通】"
    '''=========================================================================
    '''<summary>ステップ方向を変換する【共通】</summary>
    '''<param name="CutType">(INP)カット形状 </param>
    '''<param name="Dir">    (INP)ステップ方向 </param>
    '''<param name="Stp">    (OUT)ステップ方向</param>
    '''=========================================================================
    Private Sub GetStepDir(ByRef CutType As String, ByVal Dir As Short, ByRef Stp As Short)

        Dim strMSG As String

        Try
            Select Case (CutType)
                Case CNS_CUTP_SC
                    ' スキャンカット
                    Select Case (Dir)
                        Case 1      ' +X → 0°
                            Stp = 0
                        Case 2      ' -Y → 270°
                            Stp = 3
                        Case 3      ' -X → 180°
                            Stp = 2
                        Case 4      ' +Y → 90°
                            Stp = 1
                    End Select
                Case Else
            End Select

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "File.GetStepDir() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- V1.13.0.0②↓ -----
#Region "カットタイプを変換する【共通】"
    '''=========================================================================
    '''<summary>カットタイプを変換する【共通】</summary>
    '''<param name="CutType">(INP)カット形状 </param>
    '''<param name="CnvType">(OUT)変換後カット形状 </param>
    '''=========================================================================
    Private Sub CnvCutType(ByRef CutType As String, ByRef CnvType As String)

        Dim strMSG As String

        Try
            ' STカットとLカットは斜めSTカット, 斜めLカットに変換する
            Select Case (CutType)
                Case CNS_CUTP_ST                                        ' STカット → 斜めSTカット
                    CnvType = CNS_CUTP_NST
                Case CNS_CUTP_STr                                       ' STカット(リターン) → 斜めSTカット(リターン)
                    CnvType = CNS_CUTP_NSTr
                Case CNS_CUTP_STt                                       ' STカット(リトレース) → 斜めSTカット(リトレース)
                    CnvType = CNS_CUTP_NSTt
                Case CNS_CUTP_L                                         ' Lカット → 斜めLカット
                    CnvType = CNS_CUTP_NL
                Case CNS_CUTP_Lr                                        ' Lカット(リターン) → 斜めLカット(リターン)
                    CnvType = CNS_CUTP_NLr
                Case CNS_CUTP_Lt                                        ' Lカット(リトレース) → 斜めLカット(リトレース)
                    CnvType = CNS_CUTP_NLt
                    '----- V1.18.0.0④↓ -----
                Case CNS_CUTP_ST2                                       ' ﾎﾟｼﾞｼｮﾆﾝｸﾞ無しSTカット → 斜めSTカット
                    CnvType = CNS_CUTP_NST
                Case CNS_CUTP_IX2                                       ' ﾎﾟｼﾞｼｮﾆﾝｸﾞ無しIXカット → IXカット
                    CnvType = CNS_CUTP_IX
                    '----- V1.18.0.0④↑ -----
                Case Else
                    CnvType = CutType
            End Select
            Console.WriteLine("File.CnvCutType() CutType(INP)=" + CutType + ", CutType(OUT)=" + CnvType)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "File.CnvCutType() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- V1.13.0.0②↑ -----
#Region "文字列->HEX ASCII変換"
    '''=========================================================================
    '''<summary>文字列->HEX ASCII変換</summary>
    '''<param name="sin">(INP) 文字列</param>
    '''<returns>HEX ASCII文字列</returns>
    '''=========================================================================
    Private Function Str2HexAsc(ByVal sin As String) As String

        Dim i As Integer

        Str2HexAsc = ""
        For i = 1 To Len(sin)
            Str2HexAsc = Str2HexAsc & Right("0" & Hex(Asc(Mid(sin, i, 1))), 2)
        Next

    End Function
#End Region

#Region "HEX ASCII->文字列変換"
    '''=========================================================================
    '''<summary>HEX ASCII->文字列変換</summary>
    '''<param name="sin">(INP) HEX ASCII文字列</param>
    '''<returns>文字列</returns>
    '''=========================================================================
    Private Function HexAsc2Str(ByVal sin As String) As String

        Dim i As Integer

        HexAsc2Str = ""
        For i = 1 To Len(sin) Step 2
            HexAsc2Str = HexAsc2Str & Chr(Val("&H" & Mid(sin, i, 2)))
        Next

    End Function
#End Region
#End Region

#Region "プレートデータの内容を変換する"
    '''=========================================================================
    '''<summary>プレートデータの内容を右上原点から右下原点に変換する</summary>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function ConvertPlateData() As Integer

        'Dim mDATA() As String
        Dim i As Integer
        Dim flg As Integer
        Dim LocaltypPlateInfo As PlateInfo                              ' ﾌﾟﾚｰﾄﾃﾞｰﾀ
        Dim strMSG As String

        Try

            ConvertPlateData = 0
            flg = 0
            LocaltypPlateInfo = typPlateInfo

            With typPlateInfo
                '----- V4.0.0.0-35↓ -----
                ' チップ並び方向 = Y方向の場合
                If (typPlateInfo.intResistDir = 1) Then
                    ' テーブル位置オフセットX
                    LocaltypPlateInfo.dblTableOffsetXDir = 0.0
                    ' テーブル位置オフセットY
                    LocaltypPlateInfo.dblTableOffsetYDir = 0.0
                    ' BPオフセットX
                    LocaltypPlateInfo.dblBpOffSetXDir = 0.0
                    ' BPオフセットY
                    LocaltypPlateInfo.dblBpOffSetYDir = 0.0
                    ' ステップX
                    LocaltypPlateInfo.dblStepOffsetXDir = 0.0
                    ' ステップY
                    LocaltypPlateInfo.dblStepOffsetYDir = 0.0

                    ' Z ON/OFF位置を再設定する
                    LocaltypPlateInfo.dblZOffSet = Z_ON_POS_SIMPLE                  ' ZON位置
                    LocaltypPlateInfo.dblZStepUpDist = Z_STEP_POS_SIMPLE            ' ﾌﾟﾛｰﾌﾞｽﾃｯﾌﾟ上昇距離
                    LocaltypPlateInfo.dblZWaitOffset = Z_OFF_POS_SIMPLE             ' ZOFF位置

                    typPlateInfo = LocaltypPlateInfo

                    Return (cFRS_NORMAL)
                End If
                '----- V4.0.0.0-35↑ -----

                ' ブロック数X
                LocaltypPlateInfo.intBlockCntXDir = .intBlockCntYDir                'ﾌﾞﾛｯｸ数X
                ' ブロック数Y
                LocaltypPlateInfo.intBlockCntYDir = .intBlockCntXDir                'ﾌﾞﾛｯｸ数Y
                ' ブロック数X
                LocaltypPlateInfo.intBlkCntInStgGrpX = .intBlkCntInStgGrpY          'ステージグループ内ﾌﾞﾛｯｸ数X
                ' ブロック数Y
                LocaltypPlateInfo.intBlkCntInStgGrpY = .intBlkCntInStgGrpX          'ステージグループ内ﾌﾞﾛｯｸ数Y

                'ステップ方向
                Select Case (.intDirStepRepeat)
                    Case STEP_RPT_NON           ' ステップ＆リピート方向（なし）

                    Case STEP_RPT_X             ' ステップ＆リピート方向（X方向）
                        LocaltypPlateInfo.intDirStepRepeat = STEP_RPT_Y
                    Case STEP_RPT_Y             ' ステップ＆リピート方向（Y方向）
                        LocaltypPlateInfo.intDirStepRepeat = STEP_RPT_X
                    Case STEP_RPT_CHIPXSTPY     ' ステップ＆リピート方向（X方向チップ幅ステップ＋Y方向）
                        LocaltypPlateInfo.intDirStepRepeat = STEP_RPT_CHIPYSTPX
                    Case STEP_RPT_CHIPYSTPX     ' ステップ＆リピート方向（Y方向チップ幅ステップ＋X方向）
                        LocaltypPlateInfo.intDirStepRepeat = STEP_RPT_CHIPXSTPY
                    Case Else
                End Select
                ' プレートサイズX
                LocaltypPlateInfo.dblPlateSizeX = .dblPlateSizeY
                ' プレートサイズY
                LocaltypPlateInfo.dblPlateSizeY = .dblPlateSizeX
                ' プレート数X
                LocaltypPlateInfo.intPlateCntXDir = .intPlateCntYDir
                ' プレート数Y
                LocaltypPlateInfo.intPlateCntYDir = .intPlateCntXDir
                ' プレート間隔X
                LocaltypPlateInfo.dblPlateItvXDir = .dblPlateItvYDir
                ' プレート間隔Y
                LocaltypPlateInfo.dblPlateItvYDir = .dblPlateItvXDir
                ' ブロックサイズX
                LocaltypPlateInfo.dblBlockSizeXDir = .dblBlockSizeYDir              'ﾌﾞﾛｯｸ数X
                ' ブロックサイズY
                LocaltypPlateInfo.dblBlockSizeYDir = .dblBlockSizeXDir              'ﾌﾞﾛｯｸ数Y
                ' チップサイズX
                LocaltypPlateInfo.dblChipSizeXDir = .dblChipSizeYDir                'チップ数X
                ' チップサイズY
                LocaltypPlateInfo.dblChipSizeYDir = .dblChipSizeXDir                'チップ数Y

                ' テーブル位置オフセットX
                LocaltypPlateInfo.dblTableOffsetXDir = 0
                ' テーブル位置オフセットY
                LocaltypPlateInfo.dblTableOffsetYDir = .dblBlockSizeYDir * (-1)
                ' BPオフセットX
                LocaltypPlateInfo.dblBpOffSetXDir = 0
                ' BPオフセットY
                LocaltypPlateInfo.dblBpOffSetYDir = 0
                ' ステップX
                LocaltypPlateInfo.dblStepOffsetXDir = 0
                ' ステップY
                LocaltypPlateInfo.dblStepOffsetYDir = 0

                ' Z ON/OFF位置を再設定する
                '----- V4.0.0.0-35↓ -----
                LocaltypPlateInfo.dblZOffSet = Z_ON_POS_SIMPLE                      ' ZON位置
                LocaltypPlateInfo.dblZStepUpDist = Z_STEP_POS_SIMPLE                ' ﾌﾟﾛｰﾌﾞｽﾃｯﾌﾟ上昇距離
                LocaltypPlateInfo.dblZWaitOffset = Z_OFF_POS_SIMPLE                 ' ZOFF位置
                '----- V4.0.0.0-35↑ -----

                '補正位置座標1X
                LocaltypPlateInfo.dblReviseCordnt1XDir = .dblReviseCordnt1YDir
                '補正位置座標1Y
                LocaltypPlateInfo.dblReviseCordnt1YDir = .dblReviseCordnt1XDir
                '補正位置座標2X
                LocaltypPlateInfo.dblReviseCordnt2XDir = .dblReviseCordnt2YDir
                '補正位置座標2Y
                LocaltypPlateInfo.dblReviseCordnt2YDir = .dblReviseCordnt2XDir
                '補正ﾎﾟｼﾞｼｮﾝｵﾌｾｯﾄX
                LocaltypPlateInfo.dblReviseOffsetXDir = .dblReviseOffsetYDir
                '補正ﾎﾟｼﾞｼｮﾝｵﾌｾｯﾄY
                LocaltypPlateInfo.dblReviseOffsetYDir = .dblReviseOffsetXDir
                For i = 1 To LocaltypPlateInfo.intResistCntInGroup
                    ConvertResistData(i)
                Next

            End With

            typPlateInfo = LocaltypPlateInfo

            Return (cFRS_NORMAL)

            ' トラップエラー発生時 
        Catch ex As Exception
            If flg = 1 Then Return (cFRS_NORMAL) ' θ回転の前まで設定なら正常ﾘﾀｰﾝ
            strMSG = "File.ConvertPlateData() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "抵抗データを変換する"
    '''=========================================================================
    '''<summary>抵抗データを変換する</summary>
    '''<param name="rCnt"> (I/O)抵抗データ構造体のインデックス(1ｵﾘｼﾞﾝ)</param>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function ConvertResistData(ByRef rCnt As Integer) As Integer

        Dim i As Integer
        Dim strMSG As String
        Dim ResistorInfoLocal As ResistorInfo
        Dim tmpintCutDir As Integer
        Dim tempData As Double

        Try
            ResistorInfoLocal = typResistorInfoArray(rCnt)

            '----- V4.0.0.0-35↓ -----
            ' チップ並び方向 = Y方向の場合
            If (typPlateInfo.intResistDir = 1) Then
                Return (cFRS_NORMAL)
            End If
            '----- V4.0.0.0-35↑ -----

            ' ロードした抵抗データを抵抗データ構造体へ格納する
            With typResistorInfoArray(rCnt)
                For i = 0 To .intCutCount
                    '----- V6.0.3.0⑩↓ -----
                    'X,Yを入れ替える,角度を９０度回転
                    '' カット開始位置Y
                    'ResistorInfoLocal.ArrCut(i).dblStartPointX = .ArrCut(i).dblStartPointY
                    '' カット開始位置Y
                    'ResistorInfoLocal.ArrCut(i).dblStartPointY = .ArrCut(i).dblStartPointX
                    tempData = .ArrCut(i).dblStartPointX
                    '' カット開始位置Y
                    ResistorInfoLocal.ArrCut(i).dblStartPointX = .ArrCut(i).dblStartPointY
                    '' カット開始位置Y
                    ResistorInfoLocal.ArrCut(i).dblStartPointY = tempData

                    ' カット角度
                    If ResistorInfoLocal.ArrCut(i).strCutType = CNS_CUTP_NST Or ResistorInfoLocal.ArrCut(i).strCutType = CNS_CUTP_NL Then
                        tmpintCutDir = typResistorInfoArray(rCnt).ArrCut(i).intCutAngle - 90
                        If (tmpintCutDir < 0) Then
                            tmpintCutDir = 360 + tmpintCutDir
                        End If
                        ResistorInfoLocal.ArrCut(i).intCutAngle = tmpintCutDir
                    End If
                    'tmpintCutDir = typResistorInfoArray(rCnt).ArrCut(i).intCutDir - 90
                    'If (tmpintCutDir < 0) Then
                    '    tmpintCutDir = 360 + tmpintCutDir
                    'End If
                    'ResistorInfoLocal.ArrCut(i).intCutDir = tmpintCutDir

                    If ResistorInfoLocal.ArrCut(i).strCutType = CNS_CUTP_SC Then
                        tmpintCutDir = typResistorInfoArray(rCnt).ArrCut(i).intCutAngle - 90
                        If (tmpintCutDir < 0) Then
                            tmpintCutDir = 360 + tmpintCutDir
                        End If
                        ResistorInfoLocal.ArrCut(i).intCutAngle = tmpintCutDir

                        tmpintCutDir = typResistorInfoArray(rCnt).ArrCut(i).intStepDir - 1
                        If (tmpintCutDir < 0) Then
                            tmpintCutDir = 3
                        End If
                        ResistorInfoLocal.ArrCut(i).intStepDir = tmpintCutDir

                    End If
                    'X,Yを入れ替える,角度を９０度回転
                    '----- V6.0.3.0⑩↑ -----
                Next

            End With

            typResistorInfoArray(rCnt) = ResistorInfoLocal

            Return (cFRS_NORMAL)                                                    ' Return値 = 正常

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "File.Get_RESIST_Data() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                                      ' Return値 = 例外エラー
        End Try
    End Function
#End Region

#End Region

#Region "ﾌｧｲﾙの文字ｺｰﾄﾞをShift_JISからUnicode(UTF16LE BOM有)に変換する"
    '''=========================================================================
    ''' <summary>ﾌｧｲﾙの文字ｺｰﾄﾞをShift_JISからUnicode(UTF16LE BOM有)に変換する</summary>
    ''' <param name="fileFullPath">ﾌｧｲﾙﾌﾙﾊﾟｽ</param>
    ''' <remarks>'V4.4.0.0-1</remarks>
    '''=========================================================================
    Public Sub ConvertFileEncoding(ByVal fileFullPath As String)
        Try
            If (True = String.IsNullOrEmpty(fileFullPath)) Then
                Throw New ArgumentException()
            End If
            If (False = IO.File.Exists(fileFullPath)) Then
                Throw New FileNotFoundException()
            End If

            ' tky.iniが隠しﾌｧｲﾙになっていたため解除（書き込み時に例外が発生する）
            IO.File.SetAttributes(fileFullPath, FileAttributes.Normal)

            Dim src As String
            Using fs As New FileStream(fileFullPath, FileMode.Open, FileAccess.Read, FileShare.Read)
                If (True = fs.IsUnicode()) Then Exit Sub ' Unicode(UTF16LE BOM有)に変換済みならNOP

                Using sr As New StreamReader(fs, Encoding.GetEncoding("Shift_JIS"))
                    src = sr.ReadToEnd()    ' Shift_JIS として読み込む
                End Using
            End Using

            Using sw As New StreamWriter(fileFullPath, False, Encoding.Unicode)
                sw.Write(src)               ' Unicode(UTF16LE BOM有)として書き換える
            End Using

        Catch ex As Exception
            Dim strMSG As String = "File.ConvertFileEncoding() TRAP ERROR = " + ex.Message
            MessageBox.Show(strMSG & vbCrLf & fileFullPath)
        End Try

    End Sub
#End Region

#Region "FileStreamｸﾗｽ拡張ﾒｿｯﾄﾞ"
    '''=========================================================================
    '''<summary>fileStreamの先頭ﾊﾞｲﾄがUTF8のBOMかどうかを返す</summary>
    '''<param name="fileStream">FileStreamｸﾗｽ</param>
    '''<returns>True=UTF8, False=UTF8ではない</returns>
    ''' <remarks>V4.4.0.0-1</remarks>
    '''=========================================================================
    <System.Runtime.CompilerServices.Extension()>
    Public Function IsUTF8(ByVal fileStream As System.IO.FileStream) As Boolean

        If (fileStream IsNot Nothing) AndAlso (3 <= fileStream.Length) Then
            Dim BOM As Byte() = {&HEF, &HBB, &HBF}
            Dim src As Byte() = New Byte(BOM.Length) {}

            fileStream.Read(src, 0, src.Length)
            fileStream.Position = 0L

            Return (BOM(0) = src(0)) AndAlso (BOM(1) = src(1)) AndAlso (BOM(2) = src(2))
        End If

        Return False

    End Function

    '''=========================================================================
    '''<summary>fileStreamの先頭ﾊﾞｲﾄがUnicode(UTF16LE)のBOMかどうかを返す</summary>
    '''<param name="fileStream">FileStreamｸﾗｽ</param>
    '''<returns>True=Unicode, False=Unicodeではない</returns>
    ''' <remarks>V4.4.0.0-1</remarks>
    '''=========================================================================
    <System.Runtime.CompilerServices.Extension()>
    Public Function IsUnicode(ByVal fileStream As System.IO.FileStream) As Boolean

        If (fileStream IsNot Nothing) AndAlso (2 <= fileStream.Length) Then
            Dim BOM As Byte() = {&HFF, &HFE}
            Dim src As Byte() = New Byte(BOM.Length) {}

            fileStream.Read(src, 0, src.Length)
            fileStream.Position = 0L

            Return (BOM(0) = src(0)) AndAlso (BOM(1) = src(1))
        End If

        Return False

    End Function
#End Region

#Region "トリミングデータ読み込み後、元からある変数に格納する処理"
    ''' <summary>
    ''' 読み込んだトリミングデータから元の変数に格納する処理
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>'V5.0.0.8③↓</remarks>
    Private Function SetToOrgData() As Integer

        gRegistorCnt = typPlateInfo.intResistCntInBlock

    End Function


#End Region

#Region "トリムデータファイルの拡張子を取得する"
    Private _extension As String = Nothing
    ''' <summary>トリムデータファイルの拡張子を取得する</summary>
    ''' <value>ReadOnly</value>
    ''' <returns>トリムデータファイルの拡張子</returns>
    ''' <remarks>'V5.0.0.9⑮</remarks>
    Public ReadOnly Property Extension As String
        Get
            ' 拡張子設定
            If (_extension Is Nothing) Then
                Select Case (gTkyKnd)
                    Case KND_TKY                        ' TKYの場合
                        _extension = ".tdt"
                    Case KND_CHIP                       ' CHIPの場合
                        _extension = ".tdc"
                    Case KND_NET                        ' NETの場合
                        _extension = ".tdn"
                    Case Else
                        _extension = String.Empty
                End Select

                If (String.Empty <> _extension) AndAlso
                    (giMachineKd = MACHINE_KD_RS) Then  ' SL436S ?

                    _extension &= "s"
                End If
            End If

            Return _extension
        End Get
    End Property
#End Region

End Module