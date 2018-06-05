'===============================================================================
'   Description  : バーコードデータ処理(太陽社殿特注) V1.23.0.0①
'
'   Copyright(C) : OMRON LASERFRONT INC. 2014
'
'   ReMarks      : Cino社製F460GV USB(RS232Cインターフェース))
'
'===============================================================================
Option Strict Off
Option Explicit On

Imports System.IO                       'V4.4.0.0-0
Imports System.Text                     'V4.4.0.0-0
Imports System.Collections.Generic      'V5.0.0.9⑮
Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8①
Imports TKY_ALL_SL432HW.My.Resources    'V4.4.0.0-0

Module BarCode_Data

#Region "【定数/変数の定義】"
    '===========================================================================
    '   定数/変数の定義
    '===========================================================================
    '----- クラスオブジェクト -----
    Private ObjBC_PortInfo As DllSerialIO.PortInformation = Nothing     ' シリアルポート情報オブジェクト(DllSerialIO.dllを使用)
    Private ObjBCDataIO As DllQRDataIO.QRDataIO = Nothing               ' バーコードデータ入出力オブジェクト(DllQRDataIO.dllを使用)

    '----- バーコードとトリミングデータの対応CSVファイルのデータインデックス -----
    Private Const BC_IDX_MAX As Integer = 32                            ' データ項目の最大数
    Private Const BC_IDX_TYPE As Integer = 0                            ' 品種 
    Private Const BC_IDX_OPTA As Integer = 1                            ' オプションＡ
    Private Const BC_IDX_OPTB As Integer = 2                            ' オプションＢ
    Private Const BC_IDX_RVAL As Integer = 3                            ' 抵抗値
    Private Const BC_IDX_BCOD As Integer = 4                            ' バーコード
    Private Const BC_IDX_DATA As Integer = 5                            ' トリミングデータ名
    Private Const BC_IDX_BCOD2 As Integer = 6                           ' バーコードで２回目に読込んだデータ  'V5.0.0.9⑲

    '----- V4.11.0.0②↓ (WALSIN殿SL436S対応) -----
    ' バーコードとトリミングデータの対応CSVファイルのデータインデックス
    Private Const BC2_IDX_MAX As Integer = 128                          ' データ項目の最大数
    Private Const BC2_IDX_BCOD As Integer = 0                           ' バーコード
    Private Const BC2_IDX_DATA As Integer = 1                           ' トリミングデータ名

    ' SizeCode.iniの最大数
    Private Const MAX_SizeCode As Integer = 32

    ' 基板サイズ用構造体(SizeCode.iniを取り込む)
    Public Structure SizeCode_Data_Info
        Dim SizeBar As String                                           ' 基板サイズ(バーコードの3,4文字目)
        Dim SizePrt As String                                           ' 基板サイズ(表示用)
    End Structure
    Public StSizeCode(MAX_SizeCode) As SizeCode_Data_Info               ' 0 RG

    ' SizeCode.iniのパス名
    Public Const PATH_SIZCDE_DATA As String = "C:\TRIM\SizeCode.ini"
    '----- V4.11.0.0②↑ -----

    '----- その他 -----
    Public BC_Data As String                                            ' バーコード受信データ(text)
    Public BC_Read_Flg As Integer = 0                                   ' バーコード読み込み判定(0)NG (1)OK
    Public strBCConvFileFullPath As String                              ' バーコードとトリミングデータの対応ファイル名(サーバ)
    Public strBCLoadFileFullPath As String                              ' バーコードから読み出したファイルパス名
    'V5.0.0.9⑲    Public gsBCInfo(5) As String                                        ' バーコード情報表示用バッファ
    Public gsBCInfo(6) As String                                        ' バーコード情報表示用バッファ    'V5.0.0.9⑲

    'V5.0.0.9⑮                  ↓
    Public Type As BarcodeType
    Public SubStr As New List(Of Tuple(Of Integer, Integer))

    Public Enum BarcodeType As Integer
        None = 0
        Walsin = 1
        Taiyo = 2
        Standard = 3
    End Enum
    'V5.0.0.9⑮                  ↑

    '----- フラグ -----
    Public BC_Rs_Flag As Integer = 0                                    ' シリアルポートオープンフラグ(0:未ｵｰﾌﾟﾝ, 1:ｵｰﾌﾟﾝ済)

    ' バーコードで読込んだ内容の保存用      'V5.0.0.9⑲
    Friend BC_ReadDataFirst As String                                   ' バーコード１回目で読込んだデータ保存用
    Friend BC_ReadDataSecound As String                                 ' バーコード２回目で読込んだデータ保存用
    Friend BC_ReadCount As Integer = 0                                  ' バーコード読込み回数 

#End Region

#Region "【メソッド定義】"
    '---------------------------------------------------------------------------
    '   シリアル送受信処理
    '---------------------------------------------------------------------------
#Region "RS232Cポートのオープン"
    '''=========================================================================
    '''<summary>RS232Cポートのオープン</summary>
    '''<returns>0=正常, 0以外=エラー</returns> 
    '''=========================================================================
    Public Function BC_Rs232c_Open() As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' 初期処理
            BC_Read_Flg = 0                                             ' バーコード読込み判定(0)NG (1)OK
            r = SerialErrorCode.rRS_OK                                  ' Return値 = 正常
            '----- V4.11.0.0②↓ -----
            ' 太陽社殿特注/WALSIN殿特注でなければNOP
            'If (gSysPrm.stCTM.giSPECIAL <> customTAIYO) Then Return (r) ' 太陽社殿特注でなければNOP
            'V5.0.0.9⑮ If (gSysPrm.stCTM.giSPECIAL <> customTAIYO) And (gSysPrm.stCTM.giSPECIAL <> customWALSIN) Then
            If (BarcodeType.None = BarCode_Data.Type) Then              'V5.0.0.9⑮ バーコードなし
                Return (r)
            End If
            '----- V4.11.0.0②↑ -----
            ObjBCDataIO = New DllQRDataIO.QRDataIO
            ObjBC_PortInfo = New DllSerialIO.PortInformation            ' シリアルポート情報オブジェクト生成

            ' ポート情報を設定する
            ObjBC_PortInfo.PortName = gsComPort                         ' ポート番号

            ObjBC_PortInfo.BaudRate = 9600                              ' Speed
            ObjBC_PortInfo.Parity = 0                                   ' Parity(0:None, 1:Odd, 2:Even, 3:Mark, 4:Space)
            ObjBC_PortInfo.DataBits = 8                                 ' データ長 = 8 Bit
            ObjBC_PortInfo.StopBits = 1                                 ' Stop Bit = 1 Bit

            ' ポートオープン
            r = ObjBCDataIO.Serial_Open(ObjBC_PortInfo)                 ' ポートオープン
            If (r = SerialErrorCode.rRS_OK) Then                        ' 正常 ? 
                BC_Rs_Flag = 1                                          ' ﾌﾗｸﾞ=1(ｵｰﾌﾟﾝ済)
            End If
            Return (r)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "BarCode_Data.BC_Rs232c_Open() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (SerialErrorCode.rRS_Trap)
        End Try
    End Function
#End Region

#Region "RS232Cポートのクローズ"
    '''=========================================================================
    '''<summary>RS232Cポートのクローズ</summary>
    '''<returns>0=正常, 0以外=エラー</returns> 
    '''=========================================================================
    Public Function BC_Rs232c_Close() As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' ポートクローズ
            r = SerialErrorCode.rRS_OK                                  ' Return値 = 正常
            If (BC_Rs_Flag = 0) Then Exit Function '                    ' ﾌﾗｸﾞ=0(未ｵｰﾌﾟﾝ)ならNOP
            r = ObjBCDataIO.Serial_Close()                              ' ポートクローズ
            'V5.0.0.9⑮            Return (r)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "BarCode_Data.BC_Rs232c_Close() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            r = SerialErrorCode.rRS_Trap
        End Try

        ObjBC_PortInfo = Nothing                                        ' シリアルポート情報オブジェクト解放
        ObjBCDataIO = Nothing                                           ' シリアルＩＯオブジェクト解放
        BC_Rs_Flag = 0                                                  ' ﾌﾗｸﾞ=0(未ｵｰﾌﾟﾝ)
        Return (r)
    End Function
#End Region

#Region "受信したバーコードを取得する"
    '''=========================================================================
    ''' <summary>受信したバーコードを取得する</summary>
    ''' <param name="receivedData"></param>
    ''' <returns>cFRS_NORMAL  = QRデータを受信した
    '''          cFRS_ERR_RST = QRデータを受信してない　　　　　　　　　</returns>
    '''=========================================================================
    Public Function BC_GetReceiveData(ByRef receivedData As String) As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' 初期処理
            If (BC_Rs_Flag = 0) Then Exit Function '                    ' フラグ=0(RS232C未ｵｰﾌﾟﾝ)ならNOP

            ' 受信したバーコードを取得する
            r = ObjBCDataIO.GetReceiveData(receivedData)
            If (r <> SerialErrorCode.rRS_OK) Then                       ' 受信データなし ? 
                Return (cFRS_ERR_RST)
            End If
            BC_Read_Flg = 1                                             ' バーコード読み込み判定(0)NG (1)OK
            Return (cFRS_NORMAL)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "BarCode_Data.BC_GetReceiveData() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Function
#End Region

#Region "ﾊﾞｰｺｰﾄﾞ受信ﾃﾞｰﾀから指定の文字列を編集しﾌｧｲﾙﾊﾟｽを作成する"
    '''=========================================================================
    ''' <summary>ﾊﾞｰｺｰﾄﾞ受信ﾃﾞｰﾀから指定の文字列を編集しﾌｧｲﾙﾊﾟｽを作成する</summary>
    ''' <param name="strBarCodeData">(INP)バーコードデータ</param>
    ''' <returns>ファイルパス名</returns>
    ''' <remarks>'V5.0.0.9⑮</remarks>
    '''=========================================================================
    Public Function GetBarCodeFileName(ByVal strBarCodeData As String) As String    'V5.0.0.9⑲
        'V5.0.0.9⑲    Public Function GetBarCodeFileName(ByRef strBarCodeData As String) As String
        Dim ret As String
        Try
            ' バーコードとトリミングデータの対応CSVファイル(サーバ)の存在チェック
            If (System.IO.File.Exists(strBCConvFileFullPath) = False) Then
                ' "指定されたファイルは存在しません File=xxxxxxxxxxxx"
                Dim strMSG As String = MSG_15 & " File=" & strBCConvFileFullPath
                Call Form1.Z_PRINT(strMSG)
                ret = String.Empty
            Else
                Select Case (BarCode_Data.Type)
                    Case BarcodeType.Taiyo
                        'V5.0.0.9⑲                        ret = GetBarCodeFileNameTaiyo(strBarCodeData)
                        'V5.0.0.9⑲      ↓
                        If (BC_ReadCount < 1) Then
                            BC_ReadCount = (BC_ReadCount + 1)
                            BC_ReadDataFirst = strBarCodeData
                            ret = Nothing
                            Exit Try
                        End If
                        BC_ReadCount = 0
                        BC_ReadDataSecound = strBarCodeData
                        ret = GetBarCodeFileNameFromTwoData(BC_ReadDataFirst, BC_ReadDataSecound)
                        'V5.0.0.9⑲      ↑

                    Case BarcodeType.Walsin
                        ret = GetBarCodeFileNameWalsin(strBarCodeData)

                    Case BarcodeType.Standard
                        ret = GetBarCodeFileNameStandard(strBarCodeData)

                    Case Else
                        ret = String.Empty

                End Select
            End If

        Catch ex As Exception
            Dim strMSG As String = "BarCode_Data.GetBarCodeFileName() TRAP ERROR = " & ex.Message
            ret = String.Empty
        End Try

        Return ret

    End Function
#End Region

#Region "ﾊﾞｰｺｰﾄﾞ受信ﾃﾞｰﾀから指定の文字列を編集しﾌｧｲﾙﾊﾟｽを作成する(太陽社)"
    '''=========================================================================
    ''' <summary>ﾊﾞｰｺｰﾄﾞ受信ﾃﾞｰﾀから指定の文字列を編集しﾌｧｲﾙﾊﾟｽを作成する</summary>
    ''' <param name="strBarCodeData">(INP)バーコードデータ</param>
    ''' <returns>ファイルパス名</returns>
    '''=========================================================================
    Private Function GetBarCodeFileNameTaiyo(ByVal strBarCodeData As String) As String      'V5.0.0.9⑮
        'V5.0.0.9⑮    Public Function GetBarCodeFileName(ByRef strBarCodeData As String) As String

        'Dim reader As System.IO.StreamReader
        Dim strLINE As String
        Dim strDAT(BC_IDX_MAX) As String
        Dim strSetFullPath As String = ""
        Dim bFLG As Boolean = False
        Dim strMSG As String

        Try
            'V5.0.0.9⑮            '----- V4.11.0.0②↓ (WALSIN殿SL436S対応) -----
            'V5.0.0.9⑮            ' ﾊﾞｰｺｰﾄﾞ受信ﾃﾞｰﾀから指定の文字列を編集しﾌｧｲﾙﾊﾟｽを作成する
            'V5.0.0.9⑮            If (gSysPrm.stCTM.giSPECIAL = customWALSIN) Then
            'V5.0.0.9⑮            If (BarcodeType.Walsin = BarCode_Data.Type) Then
            'V5.0.0.9⑮                Return (GetBarCodeFileName2(BC_Data))
            'V5.0.0.9⑮            End If
            'V5.0.0.9⑮            '----- V4.11.0.0②↑ -----

            'V5.0.0.9⑮            ' バーコードとトリミングデータの対応CSVファイル(サーバ)の存在チェック
            'V5.0.0.9⑮            If (System.IO.File.Exists(strBCConvFileFullPath) = False) Then
            'V5.0.0.9⑮                ' "指定されたファイルは存在しません File=xxxxxxxxxxxx"
            'V5.0.0.9⑮                strMSG = MSG_15 + " File=" + strBCConvFileFullPath
            'V5.0.0.9⑮                Call Form1.Z_PRINT(strMSG)
            'V5.0.0.9⑮                strSetFullPath = ""
            'V5.0.0.9⑮                Return (strSetFullPath)
            'V5.0.0.9⑮            End If

            ' バーコードとトリミングデータの対応CSVファイル(サーバ)をオープンする
            'reader = New System.IO.StreamReader(strBCConvFileFullPath, System.Text.Encoding.GetEncoding("Shift_JIS"))
            Using reader As New StreamReader(strBCConvFileFullPath, Encoding.GetEncoding("Shift_JIS"))  ' 顧客作成ﾌｧｲﾙとのこと  V4.4.0.0-0
                ' １行づづリードする
                While (reader.Peek() > 1)                                   ' TODO: 1 < ?
                    strLINE = reader.ReadLine()
                    strDAT = strLINE.Split(",")                             ' ","で分割して取り出す

                    ' トリミングデータ名を設定する
                    If (strDAT.Length >= 6) Then                            ' トリミングデータ名までのデータは存在するか ?
                        ' バーコードは等しい ?
                        If (strDAT(BC_IDX_BCOD) = strBarCodeData) And (strDAT(BC_IDX_DATA) <> "") Then
                            bFLG = True
                            ' トリミングデータ名設定
                            strSetFullPath = gSysPrm.stDIR.gsTrimFilePath + "\" + strDAT(BC_IDX_DATA) + ".tdc"

                            ' トリミングデータの存在チェック
                            If (System.IO.File.Exists(strSetFullPath) = False) Then
                                '指定されたファイルは存在しません File=xxxxxxxxxxxx"
                                strMSG = MSG_15 + " File=" + strSetFullPath
                                Call Form1.Z_PRINT(strMSG)
                                strSetFullPath = ""
                                Exit While
                            End If

                            ' バーコード情報を設定する
                            gsBCInfo(0) = strDAT(BC_IDX_BCOD)               ' バーコード
                            gsBCInfo(1) = strDAT(BC_IDX_TYPE)               ' 品種
                            gsBCInfo(2) = strDAT(BC_IDX_OPTA)               ' オプションＡ
                            gsBCInfo(3) = strDAT(BC_IDX_OPTB)               ' オプションＢ
                            gsBCInfo(4) = strDAT(BC_IDX_RVAL)               ' 抵抗値
                            Exit While
                        End If
                    End If

                End While

                '' バーコードとトリミングデータの対応ファイル(サーバ)をクローズする
                'reader.Close()
            End Using

            ' バーコードが見つからなかった場合
            If (bFLG = False) Then
                ' "指定されたバーコードは存在しません File=xxxxxxxxxxxx"
                strMSG = MSG_157 + " File=" + strBCConvFileFullPath
                Call Form1.Z_PRINT(strMSG)
                strSetFullPath = ""
            End If

            Return (strSetFullPath)

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "BarCode_Data.GetBarCodeFileNameTaiyo() TRAP ERROR = " + ex.Message
            Return ("")
        End Try
    End Function
#End Region
    'V5.0.0.9⑲                          ↓
#Region "2つのﾊﾞｰｺｰﾄﾞ受信ﾃﾞｰﾀから指定の文字列を編集しﾌｧｲﾙﾊﾟｽを作成する(太陽社)"
    '''=========================================================================
    ''' <summary>ﾊﾞｰｺｰﾄﾞ受信ﾃﾞｰﾀから指定の文字列を編集しﾌｧｲﾙﾊﾟｽを作成する</summary>
    ''' <param name="BC_ReadDataFirst">バーコード１回目に読込んだデータ</param>
    ''' <param name="BC_ReadDataSecound">バーコード２回目に読込んだデータ</param>
    ''' <returns>該当トリミングデータファイルパス名</returns>
    '''=========================================================================
    Public Function GetBarCodeFileNameFromTwoData(ByVal BC_ReadDataFirst As String, ByVal BC_ReadDataSecound As String) As String

        'V5.0.0.9⑲        Dim reader As System.IO.StreamReader
        Dim strLINE As String
        Dim strDAT(BC_IDX_MAX) As String
        Dim strSetFullPath As String = ""
        Dim bFLG As Integer = 0
        Dim strMSG As String

        Try
            'V5.0.0.9⑲            ' バーコードとトリミングデータの対応CSVファイル(サーバ)の存在チェック
            'V5.0.0.9⑲            If (System.IO.File.Exists(strBCConvFileFullPath) = False) Then
            'V5.0.0.9⑲                ' "指定されたファイルは存在しません File=xxxxxxxxxxxx"
            'V5.0.0.9⑲                strMSG = MSG_15 + " File=" + strBCConvFileFullPath
            'V5.0.0.9⑲                Call Form1.Z_PRINT(strMSG)
            'V5.0.0.9⑲                strSetFullPath = ""
            'V5.0.0.9⑲                Return (strSetFullPath)
            'V5.0.0.9⑲            End If

            ' バーコードとトリミングデータの対応CSVファイル(サーバ)をオープンする
            'V5.0.0.9⑲            reader = New System.IO.StreamReader(strBCConvFileFullPath, System.Text.Encoding.GetEncoding("Shift_JIS"))
            Using reader As New StreamReader(strBCConvFileFullPath, Encoding.GetEncoding("Shift_JIS"))
                ' １行づづリードする
                While (reader.Peek() > 1)
                    strLINE = reader.ReadLine()
                    strDAT = strLINE.Split(",")                             ' ","で分割して取り出す

                    ' トリミングデータ名を設定する
                    If (strDAT.Length >= 7) Then                            ' トリミングデータ名までのデータは存在するか ?
                        ' バーコードは等しい ?
                        If (strDAT(BC_IDX_BCOD) = BC_ReadDataFirst) Then
                            bFLG = 1
                            If (strDAT(BC_IDX_BCOD2) = BC_ReadDataSecound) AndAlso (strDAT(BC_IDX_DATA) <> "") Then
                                bFLG = 2
                                ' トリミングデータ名設定
                                strSetFullPath = gSysPrm.stDIR.gsTrimFilePath + "\" + strDAT(BC_IDX_DATA) + ".tdc"

                                ' トリミングデータの存在チェック
                                If (System.IO.File.Exists(strSetFullPath) = False) Then
                                    '指定されたファイルは存在しません File=xxxxxxxxxxxx"
                                    strMSG = MSG_15 + " File=" + strSetFullPath
                                    Call Form1.Z_PRINT(strMSG)
                                    strSetFullPath = ""
                                    Exit While
                                End If

                                ' バーコード情報を設定する
                                gsBCInfo(0) = strDAT(BC_IDX_BCOD)               ' バーコード１
                                gsBCInfo(1) = strDAT(BC_IDX_TYPE)               ' 品種
                                gsBCInfo(2) = strDAT(BC_IDX_OPTA)               ' オプションＡ
                                gsBCInfo(3) = strDAT(BC_IDX_OPTB)               ' オプションＢ
                                gsBCInfo(4) = strDAT(BC_IDX_RVAL)               ' 抵抗値
                                gsBCInfo(5) = strDAT(BC_IDX_BCOD2)              ' バーコード２
                                Exit While
                            End If
                        End If
                    End If

                End While
            End Using

            'V5.0.0.9⑲            ' バーコードとトリミングデータの対応ファイル(サーバ)をクローズする
            'V5.0.0.9⑲           reader.Close()

            ' バーコードが見つからなかった場合
            If (bFLG < 2) Then
                If bFLG = 0 Then
                    ' "指定されたバーコードは存在しません File=xxxxxxxxxxxx"
                    strMSG = MSG_157 + " Code1=" + BC_ReadDataFirst
                ElseIf bFLG = 1 Then
                    ' "指定されたバーコードは存在しません File=xxxxxxxxxxxx"
                    strMSG = MSG_157 + " Code2=" + BC_ReadDataSecound
                Else
                    ' "指定されたバーコードは存在しません File=xxxxxxxxxxxx"
                    strMSG = MSG_157 + " File=" + strBCConvFileFullPath
                End If
                Call Form1.Z_PRINT(strMSG)
                strSetFullPath = ""
            End If

            Return (strSetFullPath)

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "BarCode_Data.GetBarCodeFileNameFromTwoData() TRAP ERROR = " + ex.Message
            Return ("")
        End Try
    End Function
#End Region
    'V5.0.0.9⑲                          ↑
    '----- V4.11.0.0②↓ (WALSIN殿SL436S対応) -----
#Region "ﾊﾞｰｺｰﾄﾞ受信ﾃﾞｰﾀから指定の文字列を編集しﾌｧｲﾙﾊﾟｽを作成する(Walsin)"
    '''=========================================================================
    ''' <summary>ﾊﾞｰｺｰﾄﾞ受信ﾃﾞｰﾀから指定の文字列を編集しﾌｧｲﾙﾊﾟｽを作成する</summary>
    ''' <param name="strBarCodeData">(INP)バーコードデータ</param>
    ''' <returns>ファイルパス名</returns>
    '''=========================================================================
    Private Function GetBarCodeFileNameWalsin(ByVal strBarCodeData As String) As String     'V5.0.0.9⑮
        'V5.0.0.9⑮    Public Function GetBarCodeFileName2(ByRef strBarCodeData As String) As String

        Dim strLINE As String
        Dim strDAT(BC2_IDX_MAX) As String
        Dim strSetFullPath As String = ""
        Dim bFLG As Boolean = False
        Dim strMSG As String
        Dim r As Integer

        Try
            'V5.0.0.9⑮            ' バーコードとトリミングデータの対応CSVファイル(サーバ)の存在チェック
            'V5.0.0.9⑮            If (System.IO.File.Exists(strBCConvFileFullPath) = False) Then
            'V5.0.0.9⑮                ' "指定されたファイルは存在しません File=xxxxxxxxxxxx"
            'V5.0.0.9⑮                strMSG = MSG_15 + " File=" + strBCConvFileFullPath
            'V5.0.0.9⑮                Call Form1.Z_PRINT(strMSG)
            'V5.0.0.9⑮                strSetFullPath = ""
            'V5.0.0.9⑮                Return (strSetFullPath)
            'V5.0.0.9⑮            End If

            ' バーコードとトリミングデータの対応CSVファイルをオープンする
            Using reader As New StreamReader(strBCConvFileFullPath, Encoding.GetEncoding("Shift_JIS"))
                ' １行づづリードする
                While (reader.Peek() > 1)                                   ' 1 < ?
                    strLINE = reader.ReadLine()
                    strDAT = strLINE.Split(",")                             ' ","で分割して取り出す

                    ' トリミングデータ名を設定する
                    If (strDAT.Length >= 2) Then                            ' トリミングデータ名までのデータは存在するか ?
                        ' バーコードは等しい ?
                        If (strDAT(BC2_IDX_BCOD) = strBarCodeData) And (strDAT(BC2_IDX_DATA) <> "") Then
                            bFLG = True
                            ' トリミングデータ名設定
                            strSetFullPath = gSysPrm.stDIR.gsTrimFilePath + "\" + strDAT(BC2_IDX_DATA) + ".tdcs"

                            ' トリミングデータの存在チェック
                            If (System.IO.File.Exists(strSetFullPath) = False) Then
                                '指定されたファイルは存在しません File=xxxxxxxxxxxx"
                                strMSG = MSG_15 + " File=" + strSetFullPath
                                Call Form1.Z_PRINT(strMSG)
                                strSetFullPath = ""
                                Exit While
                            End If

                            ' バーコード情報を設定する
                            r = GetSizeCodeData(StSizeCode, strDAT(BC2_IDX_BCOD))
                            If (r >= cFRS_NORMAL) Then
                                gsBCInfo(0) = strDAT(BC2_IDX_BCOD)      ' バーコード
                                gsBCInfo(1) = StSizeCode(r).SizePrt     ' 基板サイズ
                            Else
                                gsBCInfo(0) = strDAT(BC2_IDX_BCOD)      ' バーコード
                                gsBCInfo(1) = "??"                      ' 基板サイズ
                            End If
                            Exit While
                        End If
                    End If

                End While

            End Using

            ' バーコードが見つからなかった場合
            If (bFLG = False) Then
                ' "指定されたバーコードは存在しません File=xxxxxxxxxxxx"
                strMSG = MSG_157 + " File=" + strBCConvFileFullPath
                Call Form1.Z_PRINT(strMSG)
                strSetFullPath = ""
            End If

            Return (strSetFullPath)

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "BarCode_Data.GetBarCodeFileNameWalsin() TRAP ERROR = " + ex.Message
            Return ("")
        End Try
    End Function
#End Region

#Region "SizeCode.iniを構造体に取り込む"
    '''=========================================================================
    ''' <summary>SizeCode.iniを構造体に取り込む</summary>
    ''' <param name="StSizeCode">(I/O)基板サイズ用構造体</param>
    ''' <returns>0=正常, 0以外=エラー</returns> 
    '''=========================================================================
    Public Function IniSizeCodeData(ByRef StSizeCode() As SizeCode_Data_Info) As Integer

        Dim Idx As Integer = 0
        Dim strLINE As String
        Dim strDAT(MAX_SizeCode) As String
        Dim strMSG As String

        Try
            ' SizeCode.iniの存在チェック
            If (System.IO.File.Exists(PATH_SIZCDE_DATA) = False) Then
                ' "指定されたファイルは存在しません File=xxxxxxxxxxxx"
                strMSG = MSG_15 + " File=" + PATH_SIZCDE_DATA
                Call Form1.Z_PRINT(strMSG)
                Return (cFRS_FIOERR_INP)
            End If

            ' SizeCode.iniをオープンする
            Using reader As New StreamReader(PATH_SIZCDE_DATA, Encoding.GetEncoding("Shift_JIS"))
                ' １行づづリードする
                While (reader.Peek() > 1)                               ' 1 < ?
                    strLINE = reader.ReadLine()
                    ' ","で分割して取り出す
                    strDAT = strLINE.Split(",")

                    ' SizeCode.iniを構造体を設定する
                    If (strDAT.Length >= 2) And (Idx < StSizeCode.Length) Then
                        ' 基板サイズ用構造体を設定する
                        StSizeCode(Idx).SizeBar = strDAT(0)             ' 基板サイズ(バーコードの3,4文字目)
                        StSizeCode(Idx).SizePrt = strDAT(1)             ' 基板サイズ(表示用)
                        Idx = Idx + 1
                    End If
                End While
            End Using

            Return (cFRS_NORMAL)

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "BarCode_Data.IniSizeCodeData() TRAP ERROR = " + ex.Message
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "バーコード情報を基板サイズ用構造体から取得する"
    '''=========================================================================
    ''' <summary>バーコード情報をStSizeCodeから取得する</summary>
    ''' <param name="StSizeCode">(INP)基板サイズ用構造体</param>
    ''' <param name="strBar">    (INP)バーコード</param>
    ''' <returns>0 >= StSizeCodeのインデックス, -1=エラー</returns> 
    '''=========================================================================
    Public Function GetSizeCodeData(ByRef StSizeCode() As SizeCode_Data_Info, ByRef strBar As String) As Integer

        Dim Idx As Integer = 0
        Dim strMSG As String

        Try
            ' バーコード情報を基板サイズ用構造体から取得する
            strMSG = strBar.Substring(2, 2)                             ' バーコードの3,4文字目
            For Idx = 0 To StSizeCode.Length
                If (StSizeCode(Idx).SizeBar = strMSG) Then
                    Return (Idx)
                End If
            Next Idx

            Return (-1)

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "BarCode_Data.GetSizeCodeData() TRAP ERROR = " + ex.Message
            Return (cERR_TRAP)
        End Try
    End Function
#End Region
    '----- V4.11.0.0②↑ -----
#Region "ﾊﾞｰｺｰﾄﾞ受信ﾃﾞｰﾀから指定の文字列を編集しﾌｧｲﾙﾊﾟｽを作成する(標準オプション)"
    '''=========================================================================
    ''' <summary>ﾊﾞｰｺｰﾄﾞ受信ﾃﾞｰﾀから指定の文字列を編集しﾌｧｲﾙﾊﾟｽを作成する</summary>
    ''' <param name="strBarCodeData">(INP)バーコードデータ</param>
    ''' <returns>ファイルパス名</returns>
    ''' <remarks>'V5.0.0.9⑮</remarks>
    '''=========================================================================
    Private Function GetBarCodeFileNameStandard(ByVal strBarCodeData As String) As String

        Dim strSetFullPath As String = ""
        Dim bFLG As Boolean = False
        Dim strMSG As String

        Try
            ' バーコードデータから検索キーとなる部分文字列を取得する
            Dim key As String = GetSubstringKey(strBarCodeData)
            If (String.Empty <> key) Then                               'V5.0.0.9⑳

                ' バーコードとトリミングデータの対応CSVファイルを読み込む
                Dim lines() As String = System.IO.File.ReadAllLines(
                    strBCConvFileFullPath, Encoding.GetEncoding("Shift_JIS"))

                For Each line As String In lines
                    ' 行の先頭文字列を検索キーと比較する
                    If (line.StartsWith(key)) Then
                        ' 先頭文字列がキーと一致した
                        bFLG = True

                        ' トリミングデータ名設定
                        strSetFullPath = gSysPrm.stDIR.gsTrimFilePath &
                            "\" & line.Replace(key, String.Empty) & File.Extension

                        ' 末尾の,を削除
                        key = key.Remove(key.Length - 1)                'V5.0.0.9⑳

                        ' トリミングデータの存在チェック
                        If (System.IO.File.Exists(strSetFullPath) = False) Then
                            '指定されたファイルは存在しません File=xxxxxxxxxxxx"
                            strMSG = MSG_15 & " Key=[ " & key & " ]" & vbCrLf &
                                "  File=" & strSetFullPath

                            Call Form1.Z_PRINT(strMSG)
                            strSetFullPath = ""
                        Else
                        gsBCInfo(0) = strBarCodeData                    'V5.0.0.9⑳
                        gsBCInfo(1) = key                               'V5.0.0.9⑳
                        End If

                        Exit For
                    End If
                Next line

                ' バーコードが見つからなかった場合
                If (bFLG = False) Then
                    ' 末尾の,を削除
                    key = key.Remove(key.Length - 1)                    'V5.0.0.9⑳

                    'V5.0.0.9⑳                    ' "指定されたバーコードは存在しません File=xxxxxxxxxxxx"
                    ' 指定されたキーが見つかりません                           'V5.0.0.9⑳
                    strMSG = BarCode_Data_009 & vbCrLf &
                        "  File=" & strBCConvFileFullPath & "  Key=[ " & key & " ]"

                    Call Form1.Z_PRINT(strMSG)
                    strSetFullPath = ""
                End If

            Else                                                        'V5.0.0.9⑳
                ' 取得文字列の指定がバーコード文字数を超過する
                Dim tmp As New List(Of String)
                For Each s As Tuple(Of Integer, Integer) In SubStr
                    tmp.Add(String.Join("-", s.Item1 + 1, s.Item2))
                Next
                ' バーコード文字数を超過します (#-#, #-#)
                strMSG = String.Format(BarCode_Data_010, String.Join(", ", tmp))
                Form1.Z_PRINT(strMSG)
                strSetFullPath = String.Empty
            End If

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "BarCode_Data.GetBarCodeFileNameStandard() TRAP ERROR = " & ex.Message
            strSetFullPath = String.Empty
        End Try

        Return (strSetFullPath)

    End Function
#End Region

#Region "バーコードデータから部分文字列を取り出す"
    ''' <summary>バーコードデータから部分文字列を取り出す</summary>
    ''' <param name="barcodeData">受信バーコードデータ</param>
    ''' <returns>csvファイル内で該当行検索キーとなる文字列,String.Empty=取得BARCODE_SUBSTR1または2の指定が文字数を超過する</returns>
    ''' <remarks>'V5.0.0.9⑮</remarks>
    Private Function GetSubstringKey(ByVal barcodeData As String) As String
        Dim ret As String = barcodeData & ","

        Try
            If (0 < SubStr.Count) Then
                ' 部分文字列取得指定あり
                Dim tmp As New List(Of String)()

                For Each subStr As Tuple(Of Integer, Integer) In BarCode_Data.SubStr
                    If ((subStr.Item1 + subStr.Item2) <= barcodeData.Length) Then
                        ' バーコードデータ文字数を超過しなければ部分文字列取得
                        tmp.Add(barcodeData.Substring(subStr.Item1, subStr.Item2))
                    Else
                        ' 取得不可
                        tmp.Add(String.Empty)   'V5.0.0.9⑳ 追加
                    End If
                Next

                If (False = tmp.Contains(String.Empty)) Then
                    ret = String.Join(",", tmp) & ","
                Else
                    ret = String.Empty          'V5.0.0.9⑳ 追加
                End If
            End If

        Catch ex As Exception
            ret = barcodeData & ","
        End Try

        Return ret

    End Function
#End Region

#Region "バーコードデータで指定のファイルをロードする"
    '''=========================================================================
    ''' <summary>バーコードデータで指定のファイルをロードする</summary>
    ''' <param name="pPath">(INP)ファイルパス名</param>
    ''' <returns>0=正常, 0以外=エラー</returns> 
    '''=========================================================================
    Public Function DataLoadBC(ByRef pPath As String) As Integer

        Dim r As Integer
        Dim Dt As DateTime = DateTime.Now                               ' 現在の日時を取得
        Dim TimeOfDt As TimeSpan = Dt.TimeOfDay                         ' 現在の時刻のみを取得 
        Dim strMSG As String

        Try
            ' 初期処理
            Call SetMousePointer(Form1, True)                           ' 砂時計表示(mouse pointer)

            ' ファイルロード
            r = Form1.Sub_FileLoad(pPath)
            If (r <> cFRS_NORMAL) Then                                  ' ファイルロードエラー ?(※エラーメッセージは表示済み) 
                Return (r)
            End If

            ' 終了処理
            Call Form1.ClearCounter(1)                                  ' 生産管理データクリア
            Call ClrTrimPrnData()                                       ' トリミング結果印刷項目初期化 
            Call BC_Info_Disp(1)                                        ' バーコード情報を表示する
            '----- V4.11.0.0②↓ (WALSIN殿SL436S対応) -----
            '目標値、上限下限の表示(シンプルトリマの場合)
            If (gMachineType = MACHINE_TYPE_436S) Then
                SimpleTrimmer.SetTarget(typResistorInfoArray(1).dblTrimTargetVal, typResistorInfoArray(1).dblInitTest_LowLimit, typResistorInfoArray(1).dblInitTest_HighLimit, typResistorInfoArray(1).dblFinalTest_LowLimit, typResistorInfoArray(1).dblFinalTest_HighLimit, typResistorInfoArray(1).dblTrimTargetVal_Save)
            End If
            '----- V4.11.0.0②↑ -----

            'V5.0.0.9⑲ Call SetMousePointer(Form1, False)                          ' 砂時計解除(mouse pointer)
            Return (cFRS_NORMAL)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "BarCode_Data.DataLoadBC() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)

        Finally                                                         ' V5.0.0.9⑲
            SetMousePointer(Form1, False)                               ' 砂時計解除(mouse pointer)
        End Try

    End Function
#End Region

#Region "バーコード情報を表示する"
    '''=========================================================================
    ''' <summary>バーコード情報を表示する</summary>
    ''' <param name="Md">(INP)モード(0=初期化, 1=表示)</param>
    '''=========================================================================
    Public Sub BC_Info_Disp(ByVal Md As Integer)

        Dim strMSG As String

        Try
            ' 太陽社殿特注/(WALSIN殿SL436S対応)でなければNOP
            'If (gSysPrm.stCTM.giSPECIAL <> customTAIYO) Then Return
            'V5.0.0.9⑮            If (gSysPrm.stCTM.giSPECIAL <> customTAIYO) And (gSysPrm.stCTM.giSPECIAL <> customWALSIN) Then Return ' V4.11.0.0②
            'V5.0.0.9⑳            If (BarcodeType.Taiyo <> BarCode_Data.Type) AndAlso (BarcodeType.Walsin <> BarCode_Data.Type) Then Return 'V5.0.0.9⑮
            If (BarcodeType.None = BarCode_Data.Type) Then Return 'V5.0.0.9⑳
            ' 初期化
            If (Md = 0) Then
                gsBCInfo(0) = ""
                gsBCInfo(1) = ""
                gsBCInfo(2) = ""
                gsBCInfo(3) = ""
                gsBCInfo(4) = ""
                gsBCInfo(5) = ""                    'V5.0.0.9⑲
            End If

            ' バーコード情報を表示する
            'If (gSysPrm.stTMN.giMsgTyp = 0) Then
            '    Form1.lblQRData.Text = "[バーコード情報]" & vbCrLf & _
            '                            " バーコード  ：" & gsBCInfo(0) & vbCrLf & _
            '                            " 品種        ：" & gsBCInfo(1) & vbCrLf & _
            '                            " オプションＡ：" & gsBCInfo(2) & vbCrLf & _
            '                            " オプションＢ：" & gsBCInfo(3) & vbCrLf & _
            '                            " 抵抗値      ：" & gsBCInfo(4) & vbCrLf
            'Else
            '    Form1.lblQRData.Text = "[BAR CODE INFORMATION]" & vbCrLf & _
            '                             " Bar Code   :" & gsBCInfo(0) & vbCrLf & _
            '                             " Type       :" & gsBCInfo(1) & vbCrLf & _
            '                             " Option A   :" & gsBCInfo(2) & vbCrLf & _
            '                             " Option B   :" & gsBCInfo(3) & vbCrLf & _
            '                             " Res Value  :" & gsBCInfo(4) & vbCrLf
            'End If
#If False Then                          'V5.0.0.9⑮
            '----- V4.11.0.0②↓ (WALSIN殿SL436S対応) -----
            If (gSysPrm.stCTM.giSPECIAL = customTAIYO) Then
                Form1.GrpQrCode.Text = BarCode_Data_001 ' "[バーコード情報]"
                Form1.lblQRData.Text = gsBCInfo(0) & vbCrLf & _
                                        BarCode_Data_002 & gsBCInfo(0) & vbCrLf & _
                                        BarCode_Data_003 & gsBCInfo(1) & vbCrLf & _
                                        BarCode_Data_004 & gsBCInfo(2) & vbCrLf & _
                                        BarCode_Data_005 & gsBCInfo(3) & vbCrLf & _
                                        BarCode_Data_006 & gsBCInfo(4) & vbCrLf
            Else
                ' "[バーコード情報]" "バーコード" "基板サイズ"
                Form1.GrpQrCode.Text = BarCode_Data_001 ' "[バーコード情報]"
                Form1.lblQRData.Text = " Bar Code  :" & gsBCInfo(0) & vbCrLf & _
                                       " Size code :" & gsBCInfo(1) & vbCrLf
            End If
            '----- V4.11.0.0②↑ -----
#Else                                   'V5.0.0.9⑮
            Select Case (BarCode_Data.Type)
                Case BarcodeType.Taiyo
                    Form1.GrpQrCode.Text = BarCode_Data_001 ' "[バーコード情報]"
                    'V5.0.0.9⑲  BarCode_Data_007, BarCode_Data_008に変更
                    Form1.lblQRData.Text = BarCode_Data_007 & gsBCInfo(0) & vbCrLf & _
                                           BarCode_Data_008 & gsBCInfo(5) & vbCrLf & _
                                           BarCode_Data_003 & gsBCInfo(1) & vbCrLf & _
                                           BarCode_Data_004 & gsBCInfo(2) & vbCrLf & _
                                           BarCode_Data_005 & gsBCInfo(3) & vbCrLf & _
                                           BarCode_Data_006 & gsBCInfo(4) & vbCrLf

                Case BarcodeType.Walsin
                    ' "[バーコード情報]" "バーコード" "基板サイズ"
                    Form1.GrpQrCode.Text = BarCode_Data_001 ' "[バーコード情報]"
                    Form1.lblQRData.Text = " Bar Code  :" & gsBCInfo(0) & vbCrLf & _
                                           " Size code :" & gsBCInfo(1) & vbCrLf

                Case BarcodeType.Standard           'V5.0.0.9⑳
                    Form1.GrpQrCode.Text = BarCode_Data_001 ' "[バーコード情報]"
                    Form1.lblQRData.Text = BarCode_Data_012 & gsBCInfo(0) & vbCrLf &
                                           BarCode_Data_011 & gsBCInfo(1) & vbCrLf

                Case Else
                    ' DO NOTHING
            End Select
#End If
            Form1.lblQRData.Refresh()

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "BarCode_Data.BC_Info_Disp() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#End Region
End Module
