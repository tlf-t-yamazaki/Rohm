'==============================================================================
'   Description : ＫＯＡ殿仕様によるＱＲコード読み取り時の処理
'
'　 2017/03/02 First Written by N.Arata(OLFT) 
'              V6.1.4.0_22 KOA EW特注版より流用(特注)
'
'==============================================================================
Imports System.Globalization        ' For TextElementEnumerator 'V6.1.4.0_46
Imports LaserFront.Trimmer.DefWin32Fnc
Imports LaserFront.Trimmer.DefTrimFnc
Imports LaserFront.Trimmer.TrimData.DataManager                 ' V6.1.4.0_22
Imports TKY_ALL_SL432HW.My.Resources                            ' V6.1.4.0_22

Public Class QRCodeReader
#Region "変数定義"
    'V6.1.4.0_46    Private Enum ConditionNo                    ' 加工条件ファイル(タイプ.CSV)の項目順番定義 
    'V6.1.4.0_46        PATTERN                                 ' パターン
    'V6.1.4.0_46        MAKUATSU                                ' 膜厚
    'V6.1.4.0_46        MAKUATSU_UNIT                           ' 
    'V6.1.4.0_46        MAKUSYU                                 ' 膜種
    'V6.1.4.0_46        LO                                      ' 使用範囲(下限)
    'V6.1.4.0_46        LO_UNIT                                 '         (下限単位)
    'V6.1.4.0_46        WAVY_LINE                               '         (～)
    'V6.1.4.0_46        HI                                      '         (上限) 
    'V6.1.4.0_46        HI_UNIT                                 '         (上限単位)
    'V6.1.4.0_46        ATTENUATER                              ' アッテネータ減衰率（％）  
    'V6.1.4.0_46    End Enum
    'V6.1.4.0_46↓
    Private Enum ConditionNo
        LO
        LO_UNIT
        WAVY_LINE
        HI
        HI_UNIT
        SOZAIHYOUJISHI
        PATTERN
        MAKUATSU
        MAKUATSU_UNIT
        FOLDER
        CUTDATA
        ATTENUATER
    End Enum
    'V6.1.4.0_46↑
    'V6.1.4.14②↓
    Private Enum ConditionNoNET
        HINSYU_CUSTOMNO
        CUTDATA
        ATTENUATER
    End Enum
    'V6.1.4.14②↑
    Dim DenpyouQR_ReadFlag As Boolean           ' 伝票ＱＲコード リード完了フラグ
    Dim DenpyouQR_SeihinSyurui As String        ' 伝票ＱＲコード 製品種類
    Dim DenpyouQR_Type As String                ' 伝票ＱＲコード タイプ
    Dim DenpyouQR_Target As String              ' 伝票ＱＲコード 目標値
    Dim DenpyouQR_KakouHitsuyoSuuryo As String  ' 伝票ＱＲコード 加工必要数量 V6.1.4.0_40
    Dim DenpyouQR_LotNumber As String           ' 伝票ＱＲコード ロット番号

    Dim SozaihyoujiQR_ReadFlag As Boolean       ' 素材表示紙ＱＲコード リード完了フラグ
    Dim SozaihyoujiQR_SeihinSyurui As String    ' 素材表示紙ＱＲコード 製品種類
    Dim SozaihyoujiQR_Type As String            ' 素材表示紙ＱＲコード タイプ
    Dim SozaihyoujiQR_Pattern As String         ' 素材表示紙ＱＲコード パターン
    Dim SozaihyoujiQR_Rank As String            ' 素材表示紙ＱＲコード ランク
    Dim SozaihyoujiQR_Makuatsu As String        ' 素材表示紙ＱＲコード 膜厚 V6.1.4.0_32

    Dim dTargetValue As Double                  ' 目標値
    Dim dAttenuaterValue As Double              ' アッテネータ減衰率(%)

    Dim TrimmingDataFileName As String          ' トリミングデータファイル名
    Dim FolderName As String                    ' トリミング加工条件表・　'V6.1.4.0_46
    Dim CutData As String                       ' トリミング加工条件表・カットデータ　'V6.1.4.0_46
    Dim SozaiHyoujiShi As String                ' トリミング加工条件表・素材表示紙　'V6.1.4.0_46
    Dim NET_HinsyumeiCustomNo As String         ' NET検索用[品種名＋カスタムＮｏ．]
#End Region

#Region "ＱＲコードリーダから受信したデータを処理する"
    '''=========================================================================
    ''' <summary>ＱＲコードリーダから受信したデータを処理する</summary>
    ''' <param name="sData">受信データ</param>
    ''' <remarks>ＣＳＶファイルからトリミングデータに
    '''          目標抵抗値と減衰率を設定する
    ''' </remarks>
    '''=========================================================================
    Public Sub QRCodeDataExecute(ByRef sData As String)

        Try
            Dim sExtension As String
            Dim iRtn As Integer
            iRtn = cFRS_FIOERR_INP
            Dim strMSG As String = ""

            ' 伝票及び素材表示紙ＱＲコード リード完了フラグOFFならログ画面クリア
            If Not DenpyouQR_ReadFlag And Not SozaihyoujiQR_ReadFlag Then
                Call Form1.Z_CLS()
            End If
            If SetQRCodeData(sData) Then                                            ' 受信したＱＲデータを構造体に格納する
                If DenpyouQR_ReadFlag And SozaihyoujiQR_ReadFlag Then               ' 伝票及び素材表示紙ＱＲコード リード完了フラグON ?
                    'V6.1.4.0_46                    If DenpyouQR_SeihinSyurui <> SozaihyoujiQR_SeihinSyurui Then    ' 製品種類チェック
                    'V6.1.4.0_46                        '"製品種類が違います=[伝票製品種類]≠[素材表示紙製品種類]"
                    'V6.1.4.0_46                        strMSG = MSG_169 + "=[" + DenpyouQR_SeihinSyurui + "]≠[" + SozaihyoujiQR_SeihinSyurui + "]"
                    'V6.1.4.0_46                        Call Form1.Z_PRINT(strMSG)
                    'V6.1.4.0_46                        GoTo ERROR_RETURN
                    'V6.1.4.0_46                    End If
                    'V6.1.4.0_46                    If DenpyouQR_Type <> SozaihyoujiQR_Type Then                    ' タイプチェック
                    'V6.1.4.0_46                        '"タイプが違います=[伝票タイプ]≠[素材表示紙タイプ]"
                    'V6.1.4.0_46                        strMSG = MSG_170 + "=[" & DenpyouQR_Type + "]≠[" + SozaihyoujiQR_Type + "]"
                    'V6.1.4.0_46                        Call Form1.Z_PRINT(strMSG)
                    'V6.1.4.0_46                        GoTo ERROR_RETURN
                    'V6.1.4.0_46                    End If
                    If SerchConditionFile() Then                                    ' 加工条件ファイルから減衰率を求める(→dAttenuaterValue)
                        ' 拡張子設定
                        If (gTkyKnd = KND_TKY) Then                                 ' TKYの場合
                            sExtension = ".tdt"
                        ElseIf (gTkyKnd = KND_CHIP) Then                            ' CHIPの場合
                            If (gKeiTyp = MACHINE_KD_RS) Then
                                sExtension = ".tdcs"
                            Else
                                sExtension = ".tdc"
                            End If
                        Else
                            sExtension = ".tdn"                                     ' NETの場合
                        End If
                        ' トリミングデータ名=C:\TRIMDATA\DATA\製品タイプ\基板ランク\タイプ-パターン.tdc
                        'V6.1.4.0_46                        TrimmingDataFileName = GetPrivateProfileString_S("QR_CODE", "TRIMMING_DATA_FOLDER", "C:\TRIM\tky.ini", "C:\TRIMDATA\DATA\") & DenpyouQR_Type & "\" & SozaihyoujiQR_Rank & "\" & DenpyouQR_Type & "-" & SozaihyoujiQR_Pattern & sExtension
                        TrimmingDataFileName = GetPrivateProfileString_S("QR_CODE", "TRIMMING_DATA_FOLDER", "C:\TRIM\tky.ini", "C:\TRIMDATA\DATA\") & FolderName & "\" & SozaihyoujiQR_Rank & "\" & CutData & sExtension    'V6.1.4.0_46
                        ' ファイルロード
                        If (System.IO.File.Exists(TrimmingDataFileName) = False) Then
                            ' "トリミングデータ[トリミングデータ名]は存在しません。"
                            strMSG = MSG_171 + "[" & TrimmingDataFileName & "]" + MSG_172
                            Call Form1.Z_PRINT(strMSG)
                            iRtn = cFRS_FIOERR_INP
                        Else
                            iRtn = Form1.Sub_FileLoad(TrimmingDataFileName)
                            If (iRtn = cFRS_NORMAL) Then                                    ' ファイルロード正常 ?
                                ' 抵抗データの目標値をＱＲコードの目標値に更新する
                                For rn As Integer = 1 To gRegistorCnt                       ' 抵抗数分設定する
                                    If (typResistorInfoArray(rn).intResNo < 1000) Then      ' マーキング用抵抗以外
                                        typResistorInfoArray(rn).dblTrimTargetVal = dTargetValue
                                    End If
                                Next rn
                                ' ＱＲデータを更新する
                                typQRDATAInfo.dAttenuaterValue = dAttenuaterValue.ToString("0.00")      ' アッテネータ減衰率(%)
                                typQRDATAInfo.sTargetValue = DenpyouQR_Target                           ' 目標値
                                typQRDATAInfo.sKakouHitsuyoSuuryo = DenpyouQR_KakouHitsuyoSuuryo        ' 加工必要数量 V6.1.4.0_40
                                typQRDATAInfo.sLotNumber = DenpyouQR_LotNumber                          ' ロット番号
                                typQRDATAInfo.sSeihinSyurui = DenpyouQR_SeihinSyurui                    ' 製品種類
                                typQRDATAInfo.sType = DenpyouQR_Type                                    ' タイプ
                                typQRDATAInfo.sPattern = SozaihyoujiQR_Pattern                          ' パターン
                                typQRDATAInfo.sRank = SozaihyoujiQR_Rank                                ' ランク
                                typQRDATAInfo.sMakuatsu = SozaihyoujiQR_Makuatsu                        ' 膜厚 V6.1.4.0_32
                                typQRDATAInfo.bStatus = True                                            ' ＱＲデータ有効化フラグ
                                ' トリミングデータを更新する
                                iRtn = File_Save(TrimmingDataFileName)                                  ' トリムデータ書込み
                                If (iRtn = cFRS_NORMAL) Then
                                    ' "[トリミングデータ名]データを更新しました。"
                                    strMSG = "[" + TrimmingDataFileName + "]" + MSG_173
                                    Call Form1.Z_PRINT(strMSG)
                                    Call LotNumberDisp()                                                ' 伝票番号と抵抗値を画面に表示する
                                    Call Form1.SetFirstResData()                                        ' 現在読み込まれているデータの第１抵抗のみ以下のデータを表示
                                    ' 減衰率(%)からロータリーアッテネータを設定する
                                    'iRtn = ObjQRCodeReader.SetAttenuater(typQRDATAInfo.dAttenuaterValue)       'V6.1.4.0_22
                                    iRtn = ObjQRCodeReader.SetAttenuater(CDbl(typQRDATAInfo.dAttenuaterValue))  'V6.1.4.0_22
                                    ResetQRReadFlag()                                                   ' 未読込み状態にリセット
                                End If
                            End If
                        End If
                    End If
                    If (iRtn <> cFRS_NORMAL) Then                       ' ファイルロードエラー ?(※エラーメッセージは表示済み) 
                        GoTo ERROR_RETURN
                    Else
                        Call Form1.Z_PRINT(MSG_174)                     ' "ＱＲコードの処理が正常終了しました。"
                    End If
                End If
            Else
                GoTo ERROR_RETURN
            End If
            Return

ERROR_RETURN:
            Call Form1.Z_PRINT(MSG_175)                                 ' "ＱＲコードの処理が異常終了しました。"
            Call Form1.System1.TrmMsgBox(gSysPrm, MSG_175, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, TITLE_4)
        Catch ex As Exception
            MessageBox.Show("QRCodeReader.QRCodeDataExecute() TRAP ERROR = " + ex.Message, "FATAL ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
#End Region

#Region "生産管理情報(伝票番号と抵抗値)を画面に表示する"
    '''=========================================================================
    ''' <summary>伝票番号と抵抗値を画面に表示する</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub LotNumberDisp()

        Try
            If gTkyKnd <> KND_CHIP Then             'V6.1.4.14②
                Exit Sub                            'V6.1.4.14②
            End If                                  'V6.1.4.14②
            ' "伝票Ｎｏ．=ロット番号"
            Form1.LblcLOTNUMBER.Text = MSG_217 + typQRDATAInfo.sLotNumber   ' V6.1.4.0_39
            ' "抵抗値=目標値"
            Form1.LblcRESVALUE.Text = MSG_218 + ObjQRCodeReader.ChangeDisplayFormatValue(typQRDATAInfo.sTargetValue)

        Catch ex As Exception
            MessageBox.Show("QRCodeReader.LotNumberDisp() TRAP ERROR = " + ex.Message, "FATAL ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub
#End Region

#Region "ＱＲデータを取り込む"
    '''=========================================================================
    ''' <summary>ＱＲデータを構造体に格納する</summary>
    ''' <param name="sData">ＱＲデータ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Function SetQRCodeData(ByRef sData As String) As Boolean
        Try
            Dim mDATA() As String
            Dim iPos As Integer = -1
            Dim strMSG As String

            If sData.Chars(0) = ":" Then                                ' 伝票ＱＲコード ?
                '--------------------------------------------------------------------
                '   伝票ＱＲコードの場合
                '--------------------------------------------------------------------
                DenpyouQR_ReadFlag = False
                mDATA = sData.Split(":")                                ' 文字列を':'で分割して取出す

                If UBound(mDATA) > 0 Then
                    If mDATA(1) <> "MIT" Then
                        ' "伝票ＱＲコードのデータがＭＩＴでありません。=[データ]"
                        strMSG = MSG_209 + "=[" + sData + "]"
                        GoTo ERROR_RETURN
                    End If
                Else
                    ' "伝票ＱＲコードのデータ数が足りません。=[データ]"
                    strMSG = MSG_210 + "=[" + sData + "]"
                    GoTo ERROR_RETURN
                End If

                If UBound(mDATA) < 12 Then
                    ' "伝票ＱＲコードのデータ数が足りません。=[データ]"
                    strMSG = MSG_210 + "=[" + sData + "]"
                    GoTo ERROR_RETURN
                End If

                If mDATA(4).Length < 5 Then
                    ' "伝票ＱＲコードの製品種類・タイプの文字が足りません。=[データ]"
                    strMSG = MSG_211 + "=[" + sData + "]"
                    GoTo ERROR_RETURN
                Else
                    'V6.1.4.0_46↓
                    ' 1文字ずつ解析して、５文字目以降で数字＋アルファベットまでをタイプとして切り出します。
                    Dim bNumeric As Boolean = False
                    ' テキスト要素を列挙するオブジェクトを取得
                    Dim charEnum As TextElementEnumerator = StringInfo.GetTextElementEnumerator(mDATA(4))

                    For iCnt As Integer = 1 To mDATA(4).Length

                        ' 次の1文字を取得する
                        If charEnum.MoveNext() = False Then
                            Exit For ' 取得する文字がない
                        End If

                        If iCnt < 5 Then    '４文字まで製品種類
                            Continue For
                        End If

                        If bNumeric And Char.IsLetter(charEnum.Current) Then
                            iPos = iCnt
                            Console.WriteLine("数字の後のアルファベットです。「" & charEnum.Current & "」")
                            Exit For ' 取得する文字がない
                        End If
                        If IsNumeric(charEnum.Current) = True Then
                            Console.WriteLine("数字です。「" & charEnum.Current & "」")
                            bNumeric = True
                        Else
                            bNumeric = False
                        End If
                    Next
                    'V6.1.4.0_46↑

                    'V6.1.4.0_36 iPos = mDATA(4).IndexOf("T", 5)
                    'V6.1.4.0_36↓
                    'V6.1.4.0_46                    Dim SerchLetter() As String = {"TP", "TD", "TE", "BK"}
                    'V6.1.4.0_46                    Dim mdata4Len As Integer = mDATA(4).Length
                    'V6.1.4.0_46                    Dim iTemp As Integer
                    'V6.1.4.0_46                    iPos = -1
                    'V6.1.4.0_46                    For i As Integer = 0 To SerchLetter.Length - 1
                    'V6.1.4.0_46                        iTemp = mDATA(4).LastIndexOf(SerchLetter(i), mdata4Len - 1, mdata4Len - 4)
                    'V6.1.4.0_46                        If iTemp > iPos Then
                    'V6.1.4.0_46                            iPos = iTemp
                    'V6.1.4.0_46                        End If
                    'V6.1.4.0_46                    Next
                    'V6.1.4.0_46                    iPos = iPos - 1 '１文字スキップ
                    'V6.1.4.0_36↑
                    If iPos < 5 Then
                        ' "伝票ＱＲコードからタイプを検出出来ません。=[データ]"
                        strMSG = MSG_212 + "=[" + sData + "]"
                        GoTo ERROR_RETURN
                    Else
                        DenpyouQR_SeihinSyurui = mDATA(4).Substring(0, 4)   ' ４項目目の０～４文字まで製品種類
                        'V6.1.4.0_37 DenpyouQR_Type = mDATA(4).Substring(4, iPos - 4).Replace("HA", "H") ' ５文字目からＴの文字の前までがタイプ、"HA"は"H"に置換
                        DenpyouQR_Type = mDATA(4).Substring(4, iPos - 4)            ' ５文字目から（TP,TD,TE,BK）の文字の前の前までがタイプ
                        ' V6.1.4.0_37↓
                        'V6.1.4.0_46                        Dim ConvertFileName As String = GetPrivateProfileString_S("QR_CODE", "CONDITION_DATA_FOLDER", "C:\TRIM\tky.ini", "C:\TRIMDATA\CONDITION\") & "TYPE_CONVERT.CSV"
                        'V6.1.4.0_46                        Dim mConvData() As String
                        'V6.1.4.0_46
                        'V6.1.4.0_46                        If (System.IO.File.Exists(ConvertFileName) = False) Then
                        'V6.1.4.0_46                            ' "タイプ文字置換定義ファイル[TYPE_CONVERT.CSV]が存在しません。"
                        'V6.1.4.0_46                            strMSG = MSG_213 + "[" + ConvertFileName + "]" + MSG_214
                        'V6.1.4.0_46                            GoTo ERROR_RETURN
                        'V6.1.4.0_46                        End If
                        'V6.1.4.0_46
                        'V6.1.4.0_46                        Dim intFileNo As Integer = FreeFile()                               ' 使用可能なファイルナンバーを取得
                        'V6.1.4.0_46                        Try
                        'V6.1.4.0_46                            ' タイプ文字置換定義ファイルオープン
                        'V6.1.4.0_46                            FileOpen(intFileNo, ConvertFileName, OpenMode.Input)
                        'V6.1.4.0_46                        Catch ex As Exception
                        'V6.1.4.0_46                            ' "ファイルオープンエラー[TYPE_CONVERT.CSV]ファイルを確認してください。"
                        'V6.1.4.0_46                            strMSG = MSG_180 + "[" + ConvertFileName + "]" + MSG_181
                        'V6.1.4.0_46                            GoTo ERROR_RETURN
                        'V6.1.4.0_46                        End Try
                        'V6.1.4.0_46
                        'V6.1.4.0_46                        Do While Not EOF(intFileNo)
                        'V6.1.4.0_46                            sData = LineInput(intFileNo)                                    ' 1行読み込み
                        'V6.1.4.0_46                            mConvData = sData.Split(",")                                    ' 文字列を','で分割して取出す
                        'V6.1.4.0_46                            If DenpyouQR_Type.IndexOf(mConvData(0)) > -1 Then               ' 置換対象の文字が存在した場合
                        'V6.1.4.0_46                                Dim FromString As String = DenpyouQR_Type
                        'V6.1.4.0_46                                DenpyouQR_Type = FromString.Replace(mConvData(0).Trim(), mConvData(1).Trim())
                        'V6.1.4.0_46                                Exit Do
                        'V6.1.4.0_46                            End If
                        'V6.1.4.0_46                        Loop
                        'V6.1.4.0_46                        ' タイプ文字置換定義ファイルクローズ
                        'V6.1.4.0_46                        FileClose(intFileNo)
                        ' V6.1.4.0_37↑
                    End If
                    DenpyouQR_Target = mDATA(6)
                    DenpyouQR_KakouHitsuyoSuuryo = mDATA(8)                                 ' 加工必要数量 V6.1.4.0_40
                    DenpyouQR_LotNumber = mDATA(12)
                    DenpyouQR_ReadFlag = True
                    ' 伝票ＱＲコード情報を表示する
                    Call Form1.Z_PRINT(MSG_197)                                             ' [伝票ＱＲコード情報]
                    Call Form1.Z_PRINT(MSG_198 + "[" + DenpyouQR_SeihinSyurui + "]")        ' "製品種類    =[製品種類]"
                    Call Form1.Z_PRINT(MSG_199 + "[" + DenpyouQR_Type + "]")                ' "タイプ      =[タイプ]"
                    Call Form1.Z_PRINT(MSG_200 + "[" + DenpyouQR_Target + "]")              ' "目標値      =[目標値]"
                    Call Form1.Z_PRINT(MSG_201 + "[" + DenpyouQR_LotNumber + "]")           ' "伝票Ｎｏ．  =[ロット番号]"   V6.1.4.0_39 ロット番号から伝票Ｎｏ．へ変更
                    Call Form1.Z_PRINT(MSG_202 + "[" + DenpyouQR_KakouHitsuyoSuuryo + "]")  ' "加工必要数量=[加工必要数量]" V6.1.4.0_40 加工必要数量
                End If
            Else
                '--------------------------------------------------------------------
                '   素材表示紙ＱＲコードの場合
                '--------------------------------------------------------------------
                SozaihyoujiQR_ReadFlag = False
                mDATA = sData.Split(" ")                                                    ' 文字列を' '（スペース）で分割して取出す 
                If UBound(mDATA) < 5 Then
                    ' "素材表示紙ＱＲコードのデータ数が足りません。=[データ]"
                    strMSG = MSG_215 + "=[" + sData + "]"
                    GoTo ERROR_RETURN
                End If
                If mDATA(1).Length < 4 Then
                    ' "素材表示紙ＱＲコードの製品種類の文字が足りません。=[データ]"
                    strMSG = MSG_216 + "=[" + sData + "]"
                    GoTo ERROR_RETURN
                Else
                    SozaihyoujiQR_SeihinSyurui = mDATA(1).Substring(0, 4)                   ' ２項目目の０～４文字まで製品種類
                    Dim iLen As Integer
                    iLen = mDATA(1).Length
                    If iLen > 4 Then
                        iPos = mDATA(1).IndexOf("CN", 4)
                        If iPos < 0 Or iPos > 4 Then
                            If iPos > 4 Then
'20190308                                iLen = iLen - (iPos + 1)
                                iLen = iPos - 4
                            Else
                                iLen = iLen - 4
                            End If
                            SozaihyoujiQR_Type = mDATA(1).Substring(4, iLen) & mDATA(2)     ' ３項目目がタイプ
                        Else
                            SozaihyoujiQR_Type = mDATA(2)                                   ' ３項目目がタイプ
                        End If
                    Else
                        SozaihyoujiQR_Type = mDATA(2)                                       ' ３項目目がタイプ
                    End If
                    SozaiHyoujiShi = mDATA(1).Trim + mDATA(2).Trim                          ' V6.1.4.0_46
                    SozaihyoujiQR_Pattern = mDATA(3)                                        ' ４項目目がパターン
                    SozaihyoujiQR_Makuatsu = mDATA(4)                                       ' ５項目目が膜厚 V6.1.4.0_32
                    SozaihyoujiQR_Rank = mDATA(5)                                           ' ６項目目がランク
                    SozaihyoujiQR_ReadFlag = True
                    ' 素材表示紙ＱＲコード情報を表示する
                    Call Form1.Z_PRINT(MSG_205)                                             ' "[素材表示紙ＱＲコード情報]"
                    Call Form1.Z_PRINT(MSG_220 + "[" + SozaihyoujiQR_SeihinSyurui + "]")    ' "製品種類=[製品種類]"
                    Call Form1.Z_PRINT(MSG_221 + "[" + SozaihyoujiQR_Type + "]")            ' "タイプ  =[タイプ]"
                    Call Form1.Z_PRINT(MSG_206 + "[" + SozaihyoujiQR_Pattern + "]")         ' "パターン=[パターン]"
                    Call Form1.Z_PRINT(MSG_207 + "[" + SozaihyoujiQR_Makuatsu + "]")        ' "膜厚    =[膜厚]" V6.1.4.0_32
                    Call Form1.Z_PRINT(MSG_208 + "[" + SozaihyoujiQR_Rank + "]")            ' "ランク  =[ランク]"
                End If
            End If
            Return (True)

ERROR_RETURN:
            ' 操作ログ出力(エラーメッセージ)
            Call Debug.WriteLine(strMSG)
            Call Form1.Z_PRINT(strMSG)
            Call Form1.System1.OperationLogging(gSysPrm, strMSG, "QRCODE")
            Return (False)

        Catch ex As Exception
            MessageBox.Show("QRCodeReader.SetQRCodeData() TRAP ERROR = " + ex.Message, "FATAL ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return (False)
        End Try
    End Function
#End Region

#Region "加工条件ファイルから減衰率を求める"
    '''=========================================================================
    ''' <summary>加工条件ファイルから減衰率を求める</summary>
    ''' <returns>正常終了:True 異常終了:False</returns>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Function SerchConditionFile() As Boolean
        Try
            Dim ConditionFileName As String
            Dim intFileNo As Integer                                ' ファイル番号
            Dim sData As String
            Dim bFirstLine As Boolean = True
            Dim mData() As String
            Dim sTmp As String
            Dim Lo As Double, Hi As Double, LoUnit As Double, HiUnit As Double, Target As Double, dAttenuater As Double
            Dim sFileNameInfo As String
            Dim dMakuatsu As Double                                 'V6.1.4.0_32 QRコードから得られたデータのDoubleへの膜厚変換後
            Dim dConditionMakuatsu As Double                        'V6.1.4.0_32 加工条件ファイルから得られたデータのDoubleへの膜厚変換後
            Dim bMakuatsuSame As Boolean = False                    'V6.1.4.0_32 膜厚一致の時 True
            'V6.1.4.0_46            Dim QR_Pattern As String                                'V6.1.4.0_38 
            Dim bSecondLine As Boolean = True                       'V6.1.4.0_32
            Dim strMSG As String = ""

            ' 伝票及び素材表示紙ＱＲコード リード完了フラグOFFならエラー戻り
            If Not (DenpyouQR_ReadFlag And SozaihyoujiQR_ReadFlag) Then
                Return (False)
            End If

            ' V6.1.4.0_32↓
            ' 素材表示紙ＱＲコード 膜厚をStringからDoubleに変換する
            If Not Double.TryParse(SozaihyoujiQR_Makuatsu, dMakuatsu) Then
                ' "QRコードデータの膜厚の数値変換ができませんでした=[素材表示紙ＱＲコード 膜厚]"
                strMSG = MSG_176 + "=[" + SozaihyoujiQR_Makuatsu + "]"
                Call Form1.Z_PRINT(strMSG)
                Return (False)
            End If
            ' V6.1.4.0_32↑

            ' 加工条件ファイル名=C:\TRIMDATA\CONDITION\伝票ＱＲコードタイプ.CSV
            ConditionFileName = GetPrivateProfileString_S("QR_CODE", "CONDITION_DATA_FOLDER", "C:\TRIM\tky.ini", "C:\TRIMDATA\CONDITION\") & DenpyouQR_Type & ".CSV"
            If (System.IO.File.Exists(ConditionFileName) = False) Then
                ' "加工条件ファイル[加工条件ファイル名]は存在しません。"
                strMSG = MSG_177 + "[" & ConditionFileName & "]" + MSG_172
                Call Form1.Z_PRINT(strMSG)
                Return (False)
            End If

            '"ファイル=[加工条件ファイル名]の"
            sFileNameInfo = MSG_178 + "=[" + ConditionFileName + "]" + MSG_179
            intFileNo = FreeFile()                                  ' 使用可能なファイルナンバーを取得
            Try
                ' 加工条件ファイル(タイプ.CSV)オープン
                FileOpen(intFileNo, ConditionFileName, OpenMode.Input)
            Catch ex As Exception
                '"ファイルオープンエラー[加工条件ファイル名]ファイルを確認してください。"
                strMSG = MSG_180 + "[" + ConditionFileName + "]" + MSG_181
                Call Form1.Z_PRINT(strMSG)
                Return (False)
            End Try

            ' 加工条件ファイル(タイプ.CSV)を読み込む
            Do While Not EOF(intFileNo)
                sData = LineInput(intFileNo)                        ' 1行読み込み
                mData = sData.Split(",")                            ' 文字列を','で分割して取出す
                If bFirstLine Then                                  ' 先頭行 ?
                    ' sTmp = "伝票ＱＲコード 製品種類_タイプ"
                    sTmp = DenpyouQR_SeihinSyurui & "_" & DenpyouQR_Type
                    If mData(0) <> sTmp Then                        ' １行目の"製品種類_タイプ"が等しい ? 
                        ' "ファイル=[加工条件ファイル名]の１行目のデータ[製品種類_タイプ]が違います。"
                        strMSG = sFileNameInfo + MSG_182 + "[" + sTmp + "]" + MSG_183
                        Call Form1.Z_PRINT(strMSG)
                        GoTo ERROR_RETURN
                    End If
                    bFirstLine = False                              ' １行目フラグOFF
                Else
                    'V6.1.4.0_46↓
                    If (Not bSecondLine) And mData.Length < (ConditionNo.ATTENUATER + 1) Then
                        ' "ファイル=[加工条件ファイル名]の加工条件データの数が足りません。=[]"
                        strMSG = sFileNameInfo + MSG_226 + "=[" + sData + "]"
                        Call Form1.Z_PRINT(strMSG)
                        GoTo ERROR_RETURN
                    End If
                    'V6.1.4.0_46↑

                    ' V6.1.4.0_32↓ 膜厚をStringからDoubleに変換
                    If Double.TryParse(mData(ConditionNo.MAKUATSU), dConditionMakuatsu) Then
                        If dMakuatsu = dConditionMakuatsu Then      ' 膜厚一致 ?
                            bMakuatsuSame = True
                        Else
                            bMakuatsuSame = False
                        End If
                        ' ２行目は、"パターン,膜　厚,,膜種,使　用　範　囲,,,,,アッテネータ減衰率（％）"
                    ElseIf Not bSecondLine Then
                        ' "ファイル=[加工条件ファイル名]の膜厚の数値変換ができませんでした=[膜厚]"
                        strMSG = sFileNameInfo + MSG_184 + "=[" + sData + "]"  'V6.1.4.0_46 ブランクしか出ないのでmData(ConditionNo.MAKUATSU)→sDataへ変更
                        Call Form1.Z_PRINT(strMSG)
                        GoTo ERROR_RETURN
                    End If
                    bSecondLine = False                             ' ２行目フラグOFF
                    ' V6.1.4.0_32↑
                    ' 膜厚一致 ?
                    'V6.1.4.0_46                    If mData(0).IndexOf(SozaihyoujiQR_Pattern) >= 0 And bMakuatsuSame Then      'V6.1.4.0_32 And bMakuatsuSame　追加
                    If mData(ConditionNo.PATTERN).IndexOf(SozaihyoujiQR_Pattern) >= 0 And bMakuatsuSame Then      'V6.1.4.0_32 And bMakuatsuSame　追加
                        Target = GetValueFromString(DenpyouQR_Target)   ' 目標値をStringからDoubleに変換 'V6.1.4.0_46 mData(0)修正
                        If Target < 0 Then
                            ' ファイル=[加工条件ファイル名]の目標値の数値変換ができませんでした=[目標値]"
                            strMSG = sFileNameInfo + MSG_185 + "=[" + DenpyouQR_Target + "]"
                            Call Form1.Z_PRINT(strMSG)
                            GoTo ERROR_RETURN
                        End If
                        Lo = GetValueFromString(mData(ConditionNo.LO))  ' 下限をStringからDoubleに変換
                        If Lo < 0 Then
                            ' "ファイル=[加工条件ファイル名]の下限の数値変換ができませんでした=[データ]"
                            strMSG = sFileNameInfo + MSG_186 + "=[" + sData + "]"
                            Call Form1.Z_PRINT(strMSG)
                            GoTo ERROR_RETURN
                        End If
                        LoUnit = GetValueFromString(mData(ConditionNo.LO_UNIT)) ' 下限単位をStringからDoubleに変換
                        If LoUnit < 0 Then
                            ' "ファイル=[加工条件ファイル名]の下限の単位変換ができませんでした=[データ]"
                            strMSG = sFileNameInfo + MSG_187 + "=[" + sData + "]"
                            Call Form1.Z_PRINT(strMSG)
                            GoTo ERROR_RETURN
                        End If
                        'V6.1.4.8①                        Lo = Lo * LoUnit
                        Lo = Math.Round(Lo * LoUnit, 5)                    'V6.1.4.8①
                        Hi = GetValueFromString(mData(ConditionNo.HI))  ' 上限をStringからDoubleに変換
                        If Hi < 0 Then
                            ' "ファイル=[加工条件ファイル名]の上限の数値変換ができませんでした=[データ]"
                            strMSG = sFileNameInfo + MSG_188 + "=[GetValueFromString" + sData + "]"
                            Call Form1.Z_PRINT(strMSG)
                            GoTo ERROR_RETURN
                        End If
                        HiUnit = GetValueFromString(mData(ConditionNo.HI_UNIT)) ' 上限単位をStringからDoubleに変換
                        If HiUnit < 0 Then
                            ' "ファイル=[加工条件ファイル名]の上限の単位変換ができませんでした=[データ]"
                            strMSG = sFileNameInfo + MSG_189 + "=[" + sData + "]"
                            Call Form1.Z_PRINT(strMSG)
                            GoTo ERROR_RETURN
                        End If
                        'V6.1.4.8①                        Hi = Hi * HiUnit
                        Hi = Math.Round(Hi * HiUnit, 5)                    'V6.1.4.8①
                        If Lo <= Target And Target <= Hi Then           '  下限 <= 目標値 <= 上限 ?
                            dAttenuater = GetValueFromString(mData(ConditionNo.ATTENUATER)) ' アッテネータ減衰率をStringからDoubleに変換
                            If dAttenuater < 0 Then
                                ' "ファイル=[加工条件ファイル名]のアッテネータの数値変換ができませんでした=[データ]"
                                strMSG = sFileNameInfo + MSG_190 + "=[" + sData + "]"
                                Call Form1.Z_PRINT(strMSG)
                                GoTo ERROR_RETURN
                            Else
                                'V6.1.4.0_46↓
                                Dim mSozaiHyoujiShi() As String
                                Dim bSozaiOk As Boolean = False
                                mSozaiHyoujiShi = mData(ConditionNo.SOZAIHYOUJISHI).Split("/")
                                For i As Integer = 0 To mSozaiHyoujiShi.Length - 1
                                    If String.Compare(mSozaiHyoujiShi(i).Replace(" ", ""), SozaiHyoujiShi) = 0 Then
                                        Call Form1.Z_PRINT(MSG_222 + "=[" & mSozaiHyoujiShi(i) & "]")
                                        bSozaiOk = True
                                    End If
                                Next
                                If Not bSozaiOk Then
                                    ' "ファイル=[加工条件ファイル名]のアッテネータの数値変換ができませんでした=[データ]"
                                    strMSG = sFileNameInfo + MSG_224 + "=[" + mData(ConditionNo.SOZAIHYOUJISHI) + "]"
                                    Call Form1.Z_PRINT(strMSG)
                                    GoTo ERROR_RETURN
                                End If
                                FolderName = mData(ConditionNo.FOLDER)
                                CutData = mData(ConditionNo.CUTDATA)
                                Call Form1.Z_PRINT(MSG_225 + "=[" & FolderName & "]")
                                Call Form1.Z_PRINT(MSG_223 + "=[" & CutData & "]")
                                'V6.1.4.0_46↑
                                dAttenuaterValue = dAttenuater
                                dTargetValue = Target
                                strMSG = MSG_191 + "=[" + dTargetValue.ToString("0.0000") + "]"
                                Call Form1.Z_PRINT(strMSG)              ' "設定抵抗値=[目標値]" 
                                strMSG = MSG_192 + "=[" + dAttenuaterValue.ToString("0.0000") + "]"
                                Call Form1.Z_PRINT(strMSG)              ' "設定減衰率=[減衰率]"
                                'V6.1.4.0_38↓
                                'V6.1.4.0_46                                QR_Pattern = mData(0).Trim()
                                'V6.1.4.0_46                                If String.Compare(SozaihyoujiQR_Pattern, QR_Pattern) <> 0 Then
                                'V6.1.4.0_46                                    ' "パターン変更=[パターン"]→[パターン]"
                                'V6.1.4.0_46                                    strMSG = MSG_193 + "=[" & SozaihyoujiQR_Pattern & "]" + MSG_194 + "[" & QR_Pattern & "]"
                                'V6.1.4.0_46                                    Call Form1.Z_PRINT(strMSG)
                                'V6.1.4.0_46                                    SozaihyoujiQR_Pattern = QR_Pattern
                                'V6.1.4.0_46                                    typQRDATAInfo.sPattern = QR_Pattern
                                'V6.1.4.0_46                                End If
                                'V6.1.4.0_38↑
                                FileClose(intFileNo)
                                Return (True)
                            End If
                        End If
                    End If
                End If
            Loop

            ' "加工条件ファイル[加工条件ファイル名]に一致するパターン、膜厚、抵抗値データがありません。"
            strMSG = MSG_177 + "[" + ConditionFileName + "]" + MSG_195
            Call Form1.Z_PRINT(strMSG)
ERROR_RETURN:
            FileClose(intFileNo)
            Return (False)

        Catch ex As Exception
            MessageBox.Show("QRCodeReader.SerchConditionFile() TRAP ERROR = " + ex.Message, "FATAL ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function
#End Region

    'V6.1.4.14②↓
#Region "加工条件ファイルから減衰率を求める(NET用)"
    ''' <summary>
    ''' 加工条件ファイルから減衰率を求める(NET用)
    ''' </summary>
    ''' <returns></returns>
    Private Function SerchConditionFileNET() As Boolean
        Try
            Dim ConditionFileName As String
            Dim intFileNo As Integer                                ' ファイル番号
            Dim sData As String
            Dim mData() As String
            Dim dAttenuater As Double
            Dim sFileNameInfo As String
            Dim bFirstLine As Boolean = True
            Dim bSecondLine As Boolean = True
            Dim strMSG As String = ""


            ' 加工条件ファイル名=C:\TRIMDATA\CONDITION\伝票ＱＲコードタイプ.CSV
            ConditionFileName = GetPrivateProfileString_S("QR_CODE", "CONDITION_DATA_FOLDER_NET", "C:\TRIM\tky.ini", "C:\TRIMDATA\CONDITION_NET\") & DenpyouQR_Type & ".CSV"
            If (System.IO.File.Exists(ConditionFileName) = False) Then
                ' "加工条件ファイル[加工条件ファイル名]は存在しません。"
                strMSG = MSG_177 + "[" & ConditionFileName & "]" + MSG_172
                Call Form1.Z_PRINT(strMSG)
                Return (False)
            End If

            '"ファイル=[加工条件ファイル名]の"
            sFileNameInfo = MSG_178 + "=[" + ConditionFileName + "]" + MSG_179
            intFileNo = FreeFile()                                  ' 使用可能なファイルナンバーを取得
            Try
                ' 加工条件ファイル(タイプ.CSV)オープン
                FileOpen(intFileNo, ConditionFileName, OpenMode.Input)
            Catch ex As Exception
                '"ファイルオープンエラー[加工条件ファイル名]ファイルを確認してください。"
                strMSG = MSG_180 + "[" + ConditionFileName + "]" + MSG_181
                Call Form1.Z_PRINT(strMSG)
                Return (False)
            End Try

            ' 加工条件ファイル(タイプ.CSV)を読み込む
            Do While Not EOF(intFileNo)
                sData = LineInput(intFileNo)                        ' 1行読み込み
                mData = sData.Split(",")                            ' 文字列を','で分割して取出す
                If bFirstLine Then                                  ' 先頭行 ?
                    ' DenpyouQR_Type = "品種名"
                    If mData(0) <> DenpyouQR_Type Then                        ' １行目の"製品種類_タイプ"が等しい ? 
                        ' "ファイル=[加工条件ファイル名]の１行目のデータ[製品種類_タイプ]が違います。"
                        strMSG = sFileNameInfo + MSG_182 + "[" + DenpyouQR_Type + "]" + " -> [" + mData(0) + "]" + MSG_183
                        Call Form1.Z_PRINT(strMSG)
                        GoTo ERROR_RETURN
                    End If
                    bFirstLine = False                              ' １行目フラグOFF
                Else
                    If bSecondLine = True Then
                        bSecondLine = False                         ' ２行目コメント行フラグOFF
                    Else
                        If mData.Length < (ConditionNoNET.ATTENUATER + 1) Then
                            ' "ファイル=[加工条件ファイル名]の加工条件データの数が足りません。=[]"
                            strMSG = sFileNameInfo + MSG_226 + "=[" + sData + "]"
                            Call Form1.Z_PRINT(strMSG)
                            GoTo ERROR_RETURN
                        End If

                        ' 品種名＋カスタムＮｏ．一致
                        If mData(ConditionNoNET.HINSYU_CUSTOMNO).Trim = NET_HinsyumeiCustomNo Then
                            dAttenuater = GetValueFromString(mData(ConditionNoNET.ATTENUATER)) ' アッテネータ減衰率をStringからDoubleに変換
                            If dAttenuater < 0 Then
                                ' "ファイル=[加工条件ファイル名]のアッテネータの数値変換ができませんでした=[データ]"
                                strMSG = sFileNameInfo + MSG_190 + "=[" + sData + "]"
                                Call Form1.Z_PRINT(strMSG)
                                GoTo ERROR_RETURN
                            Else
                                Call Form1.Z_PRINT(MSG_197)                                             ' [伝票ＱＲコード情報]
                                Call Form1.Z_PRINT(MSG_227 + "=[" & DenpyouQR_LotNumber & "]")
                                CutData = mData(ConditionNoNET.CUTDATA).Trim
                                Call Form1.Z_PRINT(MSG_223 + "    =[" & CutData & "]")
                                dAttenuaterValue = dAttenuater
                                strMSG = MSG_192 + "      =[" + dAttenuaterValue.ToString("0.0000") + "]"
                                Call Form1.Z_PRINT(strMSG)              ' "設定減衰率=[減衰率]"
                                FileClose(intFileNo)
                                Return (True)
                            End If
                        End If
                    End If
                End If
            Loop

            ' "加工条件ファイル[加工条件ファイル名]に工程伝票QRコード[]がありません。"
            strMSG = MSG_177 + "[" + ConditionFileName + "]" + MSG_228 + "[" + DenpyouQR_LotNumber + "]" + MSG_231
            Call Form1.Z_PRINT(strMSG)
ERROR_RETURN:
            FileClose(intFileNo)
            Return (False)

        Catch ex As Exception
            MessageBox.Show("QRCodeReader.SerchConditionFileNET() TRAP ERROR = " + ex.Message, "FATAL ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function
#End Region
    'V6.1.4.14②↑
#Region "単位付の文字列から倍精度の値を求める"
    '''=========================================================================
    ''' <summary>単位付の文字列から倍精度の値を求める</summary>
    ''' <param name="sData">文字データ</param>
    ''' <returns>倍精度の値</returns>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Function GetValueFromString(ByVal sData As String) As Double
        Try
            'V6.1.4.0_40 Dim sUnit() As String = {"MΩ", "M", "kΩ", "KΩ", "K", "k", "Ω", "R", "%"}
            'V6.1.4.0_40 Dim dData() As Double = {1000000.0, 1000000.0, 1000.0, 1000.0, 1000.0, 1000.0, 1.0, 1.0, 1.0}
            Dim sUnit() As String = {"MΩ", "M", "kΩ", "KΩ", "ｋΩ", "ＫΩ", "K", "k", "Ω", "R", "%"}                    'V6.1.4.0_40　全角追加　 "ｋΩ", "ＫΩ"
            Dim dData() As Double = {1000000.0, 1000000.0, 1000.0, 1000.0, 1000.0, 1000.0, 1000.0, 1000.0, 1.0, 1.0, 1.0}   'V6.1.4.0_40
            Dim iOffSet As Integer
            Dim sTrimData As String = sData.Replace(" ", "")            ' スペース削除
            Dim dRetData As Double = -9999.0
            Dim strMSG As String

            If String.IsNullOrWhiteSpace(sData) Then                    ' NULL文字またはスペースのみ
                GoTo ERROR_END
            End If

            For i As Integer = 0 To UBound(sUnit)
                iOffSet = sTrimData.IndexOf(sUnit(i))
                If iOffSet >= 0 Then
                    If iOffSet = 0 Then
                        Return (dData(i))
                    End If
                    For j As Integer = 0 To iOffSet - 1
                        If Not (Char.IsNumber(sTrimData(j)) Or sTrimData(j) = ".") Then
                            GoTo ERROR_END
                        End If
                    Next
                    Try
                        'V6.1.4.7①                        dRetData = Double.Parse(sTrimData.Substring(0, iOffSet)) * dData(i)
                        dRetData = Math.Round(Double.Parse(sTrimData.Substring(0, iOffSet)) * dData(i), 5)          'V6.1.4.7①
                        Return (dRetData)
                    Catch ex As Exception
                        GoTo ERROR_END
                    End Try
                End If
            Next
            For j As Integer = 0 To sTrimData.Length - 1
                If Not (Char.IsNumber(sTrimData(j)) Or sTrimData(j) = ".") Then
                    GoTo ERROR_END
                End If
            Next
            Try
                'V6.1.4.7①                dRetData = Double.Parse(sTrimData.Substring(0, sTrimData.Length))
                dRetData = Math.Round(Double.Parse(sTrimData.Substring(0, sTrimData.Length)), 5)                    'V6.1.4.7①
                Return (dRetData)
            Catch ex As Exception
                GoTo ERROR_END
            End Try

ERROR_END:
            ' 操作ログ出力(エラーメッセージ)
            strMSG = MSG_196 + "=[" & sData & "]"                       ' "数値変換が出来ませんでした=[文字データ]"
            Call Debug.WriteLine(strMSG)
            Call Form1.Z_PRINT(strMSG)
            Call Form1.System1.OperationLogging(gSysPrm, strMSG, "QRCODE")

            Return (dRetData)

        Catch ex As Exception
            MessageBox.Show("QRCodeReader.GetValueFromString() TRAP ERROR = " + ex.Message, "FATAL ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function
#End Region

#Region "ＱＲコードの抵抗値から単位付で整形した表示用文字データを求める"
    '''=========================================================================
    ''' <summary>ＱＲコードの抵抗値から単位付で整形した表示用文字データを求める</summary>
    ''' <param name="sData">ＱＲコードの文字データ</param>
    ''' <returns>整形文字</returns>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Function ChangeDisplayFormatValue(ByVal sData As String) As String

        Try
            Dim sReturnString As String = sData
            Dim dData As Double

            dData = GetValueFromString(sData)
            If dData < 0 Then
                Return (sReturnString)
            Else
                If dData >= 1000000.0 Then
                    dData = dData / 1000000.0
                    sReturnString = dData.ToString("0.00") & " MΩ"
                ElseIf dData >= 1000.0 Then
                    dData = dData / 1000.0
                    sReturnString = dData.ToString("0.00") & " KΩ"
                Else
                    'V6.1.4.0_46                    dData = dData / 1000.0
                    sReturnString = dData.ToString("0.00") & " Ω"
                End If
            End If
            Return (sReturnString)

        Catch ex As Exception
            MessageBox.Show("QRCodeReader.ChangeDisplayFormatValue([" & sData & "]) TRAP ERROR = " + ex.Message, "FATAL ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return (sData)
        End Try
    End Function
#End Region

#Region "減衰率でアッテネータを設定する"
    '''=========================================================================
    ''' <summary>減衰率でアッテネータを設定する</summary>
    ''' <param name="attenuate">減衰率</param>
    ''' <returns>正常終了:cFRS_NORMAL 異常終了:≠cFRS_NORMAL</returns>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Function SetAttenuater(ByVal attenuate As Double) As Integer

        Try
            Dim atrate As Double                                        ' 減衰率
            Dim n As Short                                              ' ロータリーアッテネータの回転量(0-FFF)
            Dim fx As Short                                             ' 固定アッテネータ(0:OFF, 1:ON)
            Dim iRtn As Integer = cERR_TRAP
            Dim strMSG As String = ""

            If (gSysPrm.stRMC.giRmCtrl2 < 2) Then
                Call Form1.Z_PRINT("RMCTRL2 Is set to 0.")
                Return (cFRS_NORMAL)
            End If

            ' システムパラメータ読み込み
            Call gDllSysprmSysParam_definst.GetSysPrm_ROT_ATT(gSysPrm.stRAT)

            '--------------------------------------------------------------------------
            '   減衰率(%)からロータリーアッテネータの回転量n(0-FFF)を求める
            '--------------------------------------------------------------------------
            ' 固定アッテネータON/OFFを設定する
            If attenuate < gSysPrm.stRAT.gfATTFixPrm Then               ' 減衰率が50.0%未満なら固定アッテネータはOFF
                fx = 0                                                  ' 固定アッテネータOFF
                atrate = attenuate                                      ' 減衰率 = 減衰率
            ElseIf attenuate = gSysPrm.stRAT.gfATTFixPrm Then           ' 減衰率が50.0% ?
                fx = 1                                                  ' 固定アッテネータON
                atrate = 0.0#                                           ' 減衰率 = 0%
            Else                                                        ' 減衰率が50～100%
                fx = 1                                                  ' 固定アッテネータON
                atrate = (attenuate - gSysPrm.stRAT.gfATTFixPrm) * 1.0# / (100.0# - gSysPrm.stRAT.gfATTFixPrm) * 100.0#
            End If

            If atrate > gSysPrm.stRAT.gfPower(20) Then                  ' 最大減衰率(TKYSYS.INI)を超えるときは
                atrate = gSysPrm.stRAT.gfPower(20)                      ' 最大減衰率とする
            End If

            ' ロータリーアッテネータの回転量n(0-FFF)を求める   ' 回転量n=0～2500パルス(0x0～0x9C4)
            Call Form1.System1.CalcRotaryAngleByRate(gSysPrm, atrate, n)

            ' ロータリーアッテネータ制御
            Call SLIDECOVERCHK(SLIDECOVER_CHECK_OFF)                    ' スライドカバーチェックなし
            iRtn = LATTSET(fx, n)
            Call SLIDECOVERCHK(SLIDECOVER_CHECK_ON)                     ' スライドカバーチェックあり
            If (iRtn <> cFRS_NORMAL) Then
                ' "アッテネータ減衰率=[x.xx(%)]設定異常終了しました。=[xxx]"
                strMSG = MSG_167 + "=[" & attenuate.ToString("0.00") & "(%)]" + MSG_168 + "=[" & iRtn.ToString & "]"
                Call Form1.Z_PRINT(strMSG)
            Else
                ' システムパラメータ更新
                gSysPrm.stRAT.giAttFix = fx
                gSysPrm.stRAT.giAttRot = n
                gSysPrm.stRAT.gfAttRate = attenuate
                Call gDllSysprmSysParam_definst.PutSysPrm_ROT_ATT((gSysPrm.stRAT))
                ' 画面に減衰率を表示する
                Form1.LblRotAtt.Text = LBL_ATT + "  " + attenuate.ToString("##0.0") + " %"
            End If
            Return (iRtn)

        Catch ex As Exception
            MessageBox.Show("QRCodeReader.SetAttenuater() TRAP ERROR = " + ex.Message, "FATAL ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Function
#End Region

#Region "ＱＲ読み込み状態の初期化"
    '''=========================================================================
    ''' <summary>ＱＲ読み込み状態の初期化</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub ResetQRReadFlag()

        Try
            QR_Read_Flg = 0                                                     ' QRｺｰﾄﾞ読み込み判定(0)NG (1)OK
            DenpyouQR_ReadFlag = False                                          ' 未読み込み状態にリセット
            SozaihyoujiQR_ReadFlag = False                                      ' 未読み込み状態にリセット

        Catch ex As Exception
            MessageBox.Show("QRCodeReader.ResetQR_ReadFlag() TRAP ERROR = " + ex.Message, "FATAL ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
#End Region

    'V6.1.4.10②↓
#Region "ＴＫＹ－ＮＥＴでＱＲコードリーダから受信したデータを処理する"
    Public Sub QRCodeDataExecuteForTKYNET(ByRef sData As String)

        Try
            Dim mDATA() As String
            Dim sExtension As String
            Dim iRtn As Integer = cFRS_FIOERR_INP
            Dim strMSG As String = ""

            mDATA = sData.Split(":")                                        ' 文字列を':'で分割して取出す

            ' 読み出しデータ例：　A0249:HVD9024:HVD:9024:440:Q24:T:TE:11.5M/37.1k:BB:25:10:0.1:0.1:HVD-9024-2:U-01:E-01:20200407:
            ' ２番目の"HVD9024"がトリミングデータ名 ← mDATA(1)

            If UBound(mDATA) < 2 Then   'V6.1.4.14②　1→2へ変更
                ' "伝票ＱＲコードのデータ数が足りません。=[データ]"
                strMSG = MSG_210 + "=[" + sData + "]"
                Call Form1.Z_PRINT(strMSG)
                Call Form1.System1.OperationLogging(gSysPrm, strMSG, "QRCODE")
                GoTo ERROR_RETURN
            End If

            NET_HinsyumeiCustomNo = mDATA(1).Trim                       'V6.1.4.14② 品種名＋カスタムＮｏ（読み取るQRコード内の、["２項目目"]）
            DenpyouQR_LotNumber = mDATA(0).Trim + "-" + mDATA(1).Trim   'V6.1.4.14② ロット番号　⇒エントリー時ファイル名（読み取るQRコード内の、["1項目目"-"２項目目"]）
            DenpyouQR_Type = mDATA(2).Trim                              'V6.1.4.14② タイプ⇒品種名（読み取るQRコード内の、["３項目目"]）

            If SerchConditionFileNET() Then                             'V6.1.4.14② 加工条件ファイルから減衰率を求める(→dAttenuaterValue)

                ' 拡張子設定
                sExtension = ".tdn"                                     ' NETの場合
                'V6.1.4.14②            TrimmingDataFileName = GetPrivateProfileString_S("QR_CODE", "TKYNET_TRIMMING_DATA_FOLDER", "C:\TRIM\tky.ini", "C:\TRIMDATA\DATA\") & mDATA(1) & sExtension    'V6.1.4.0_46
                TrimmingDataFileName = GetPrivateProfileString_S("QR_CODE", "TKYNET_TRIMMING_DATA_FOLDER", "C:\TRIM\tky.ini", "C:\TRIMDATA\DATA_NET\") & DenpyouQR_Type & "\" & CutData & sExtension
                ' ファイルロード
                If (System.IO.File.Exists(TrimmingDataFileName) = False) Then
                    ' "トリミングデータ[トリミングデータ名]は存在しません。"
                    strMSG = MSG_171 + "[" & TrimmingDataFileName & "]" + MSG_172
                    Call Form1.Z_PRINT(strMSG)
                    iRtn = cFRS_FIOERR_INP
                Else
                    iRtn = Form1.Sub_FileLoad(TrimmingDataFileName)
                    'V6.1.4.14②↓
                    If (iRtn = cFRS_NORMAL) Then                                    ' ファイルロード正常 ?
                        ' ＱＲデータを更新する
                        typQRDATAInfo.dAttenuaterValue = dAttenuaterValue.ToString("0.00")      ' アッテネータ減衰率(%)
                        typQRDATAInfo.sTargetValue = ""                                         ' 目標値(※NTEは未使用)
                        typQRDATAInfo.sKakouHitsuyoSuuryo = ""                                  ' 加工必要数量(※NTEは未使用)
                        typQRDATAInfo.sLotNumber = DenpyouQR_LotNumber                          ' ロット番号⇒エントリー時ファイル名（読み取るQRコード内の、["1項目目"-"２項目目"]）
                        typQRDATAInfo.sSeihinSyurui = ""                                        ' 製品種類(※NTEは未使用)
                        typQRDATAInfo.sType = DenpyouQR_Type                                    ' タイプ⇒品種名（読み取るQRコード内の、["３項目目"]）
                        typQRDATAInfo.sPattern = ""                                             ' パターン(※NTEは未使用)
                        typQRDATAInfo.sRank = ""                                                ' ランク(※NTEは未使用)
                        typQRDATAInfo.sMakuatsu = ""                                            ' 膜厚(※NTEは未使用)
                        typQRDATAInfo.bStatus = True                                            ' ＱＲデータ有効化フラグ
                        ' トリミングデータを更新する
                        iRtn = File_Save(TrimmingDataFileName)                                  ' トリムデータ書込み
                        If (iRtn = cFRS_NORMAL) Then
                            ' "[トリミングデータ名]データを更新しました。"
                            strMSG = "[" + TrimmingDataFileName + "]" + MSG_173
                            Call Form1.Z_PRINT(strMSG)
                            'Call LotNumberDisp()                                                ' 伝票番号と抵抗値を画面に表示する
                            Call Form1.SetFirstResData()                                        ' 現在読み込まれているデータの第１抵抗のみ以下のデータを表示
                            ' 減衰率(%)からロータリーアッテネータを設定する
                            iRtn = ObjQRCodeReader.SetAttenuater(CDbl(typQRDATAInfo.dAttenuaterValue))
                            ResetQRReadFlag()                                                   ' 未読込み状態にリセット
                        End If
                    End If
                    'V6.1.4.14②↑
                End If

            End If                                                      'V6.1.4.14②

            If (iRtn <> cFRS_NORMAL) Then                       ' ファイルロードエラー ?(※エラーメッセージは表示済み) 
                GoTo ERROR_RETURN
            Else
                Call Form1.Z_PRINT(MSG_174)                     ' "ＱＲコードの処理が正常終了しました。"
            End If

            Return

ERROR_RETURN:
            Call Form1.Z_PRINT(MSG_175)                                 ' "ＱＲコードの処理が異常終了しました。"
            Call Form1.System1.TrmMsgBox(gSysPrm, MSG_175, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, TITLE_4)
        Catch ex As Exception
            MessageBox.Show("QRCodeReader.QRCodeDataExecuteForTKYNET() TRAP ERROR = " + ex.Message, "FATAL ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
#End Region
    'V6.1.4.10②↑

End Class
'=============================== END OF FILE ===============================
