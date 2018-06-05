


'===============================================================================
'   Description  : トリミングデータ形式定義とＩＯ処理
'
'   Copyright(C) : OMRON LASERFRONT INC. 2010
'
'===============================================================================
Option Strict Off
Option Explicit On

Imports LaserFront.Trimmer.DefTrimFnc
Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8①
Imports TKY_ALL_SL432HW.My.Resources    'V4.4.0.0-0

Module DataAccess
#Region "定数定義"
    '---------------------------------------------------------------------------
    '   抵抗データの目標値指定(0:絶対値,1:レシオ,2:計算式, 3～9:特注レシオ)
    '---------------------------------------------------------------------------
    Public Const TARGET_TYPE_ABSOLUTE As Short = 0
    Public Const TARGET_TYPE_RATIO As Short = 1
    Public Const TARGET_TYPE_CALC As Short = 2
    Public Const TARGET_TYPE_CUSRTO_3 As Short = 3
    Public Const TARGET_TYPE_CUSRTO_4 As Short = 4
    Public Const TARGET_TYPE_CUSRTO_5 As Short = 5
    Public Const TARGET_TYPE_CUSRTO_6 As Short = 6
    Public Const TARGET_TYPE_CUSRTO_7 As Short = 7
    Public Const TARGET_TYPE_CUSRTO_8 As Short = 8
    Public Const TARGET_TYPE_CUSRTO_9 As Short = 9

#Region "LOAD・SAVEの共通化によりTrimDataEditorで定義"
#If False Then 'V5.0.0.8①
    '---------------------------------------------------------------------------
    '   カットデータのカット形状定義
    '---------------------------------------------------------------------------
    Public Const CNS_CUTP_ST As String = "1"                ' STカット
    Public Const CNS_CUTP_L As String = "2"                 ' Lカット
    Public Const CNS_CUTP_HK As String = "3"                ' HOOKカット
    Public Const CNS_CUTP_IX As String = "4"                ' IXカット
    Public Const CNS_CUTP_SC As String = "5"                ' スキャンカット
    Public Const CNS_CUTP_STr As String = "6"               ' STカット(リターン)
    Public Const CNS_CUTP_Lr As String = "7"                ' Lカット(リターン)
    Public Const CNS_CUTP_STt As String = "8"               ' STカット(リトレース)
    Public Const CNS_CUTP_Lt As String = "9"                ' Lカット(リトレース)
    Public Const CNS_CUTP_NST As String = "A"               ' 斜めSTカット
    Public Const CNS_CUTP_NL As String = "B"                ' 斜めLカット
    Public Const CNS_CUTP_NSTr As String = "C"              ' 斜めSTカット(リターン)
    Public Const CNS_CUTP_NLr As String = "D"               ' 斜めLカット(リターン)
    Public Const CNS_CUTP_NSTt As String = "E"              ' 斜めSTカット(リトレース)
    Public Const CNS_CUTP_NLt As String = "F"               ' 斜めLカット(リトレース)
    Public Const CNS_CUTP_C As String = "G"                 ' Cカット
    Public Const CNS_CUTP_U As String = "H"                 ' Uカット
    Public Const CNS_CUTP_Ut As String = "I"                ' Uカット(リトレース) V1.22.0.0①
    Public Const CNS_CUTP_ES As String = "K"                ' ESカット
    Public Const CNS_CUTP_M As String = "M"                 ' 文字マーキング 
    Public Const CNS_CUTP_ES2 As String = "S"               ' ES2カット
    Public Const CNS_CUTP_ST2 As String = "T"               ' ﾎﾟｼﾞｼｮﾆﾝｸﾞ無しSTカット
    Public Const CNS_CUTP_IX2 As String = "X"               ' ﾎﾟｼﾞｼｮﾆﾝｸﾞ無しｲﾝﾃﾞｯｸｽ
    Public Const CNS_CUTP_NOP As String = "Z"               ' NO CUT

    '---------------------------------------------------------------------------
    '   カットデータの測定モード(IX用)定義  2011.08.30
    '---------------------------------------------------------------------------
    Public Const MEASMODE_REG As Short = 0                  ' 抵抗測定
    Public Const MEASMODE_VOL As Short = 1                  ' 電圧測定
    Public Const MEASMODE_EXT As Short = 2                  ' 外部測定

    '---------------------------------------------------------------------------
    '   ﾌﾟﾚｰﾄﾃﾞｰﾀのﾃｷｽﾄﾎﾞｯｸｽ配列番号
    '---------------------------------------------------------------------------
    Public Const glNo_DataNo As Short = 0                   'ﾃﾞｰﾀno.
    Public Const glNo_MeasType As Short = 200               'トリムモード
    Public Const glNo_DirStepRepeat As Short = 1            'ｽﾃｯﾌﾟ＆ﾘﾋﾟｰﾄ
    Public Const glNo_PlateCntXDir As Short = 96            'X方向のプレート数
    Public Const glNo_PlateCntYDir As Short = 97            'Y方向のプレート数
    Public Const glNo_BlockCntXDir As Short = 2             'ﾌﾞﾛｯｸ数X
    Public Const glNo_BlockCntYDir As Short = 3             'ﾌﾞﾛｯｸ数Y
    'Public Const glPlateInvXDir    As Integer = 1          'X方向のプレート間隔
    'Public Const glPlateInvYDir    As Integer = 1          'Y方向のプレート間隔
    Public Const glNo_TableOffSetXDir As Short = 4          'ﾃｰﾌﾞﾙ位置ｵﾌｾｯﾄX
    Public Const glNo_TableOffSetYDir As Short = 5          'ﾃｰﾌﾞﾙ位置ｵﾌｾｯﾄY
    Public Const glNo_BpOffsetXDir As Short = 6             'ﾋﾞｰﾑ位置ｵﾌｾｯﾄX
    Public Const glNo_BpOffsetYDir As Short = 7             'ﾋﾞｰﾑ位置ｵﾌｾｯﾄY
    Public Const glNo_AdjOffsetXDir As Short = 8            'ｱｼﾞｬｽﾄ位置ｵﾌｾｯﾄX
    Public Const glNo_AdjOffsetYDir As Short = 9            'ｱｼﾞｬｽﾄ位置ｵﾌｾｯﾄY
    Public Const glNo_NGMark As Short = 10                  'NGﾏｰｷﾝｸﾞ
    Public Const glNo_DelayTrim As Short = 11               'ﾃﾞｨﾚｲﾄﾘﾑ
    Public Const glNo_NgJudgeUnit As Short = 12             'NG判定単位
    Public Const glNo_NgJudgeLevel As Short = 13            'NG判定基準
    Public Const glNo_ZOffSet As Short = 14                 'ﾌﾟﾛｰﾌﾞZｵﾌｾｯﾄ
    Public Const glNo_ZStepUpDist As Short = 15             'ﾌﾟﾛｰﾌﾞｽﾃｯﾌﾟ上昇距離
    Public Const glNo_ZWaitOffset As Short = 16             'ﾌﾟﾛｰﾌﾞ待機Zｵﾌｾｯﾄ
    '
    Public Const glNo_ResistDir As Short = 17               '抵抗並び方向
    Public Const glNo_CurcuitCnt As Short = 18              '1ｸﾞﾙｰﾌﾟ内抵抗数
    Public Const glNo_ResistCntInGroup As Short = 102       '1ｸﾞﾙｰﾌﾟ内サーキット数
    Public Const glNo_GroupCntInBlockXBp As Short = 19      'ﾌﾞﾛｯｸ内ｸﾞﾙｰﾌﾟ数X
    Public Const glNo_GroupCntInBlockYStage As Short = 20   'ﾌﾞﾛｯｸ内ｸﾞﾙｰﾌﾟ数Y
    Public Const glNo_GroupItvXDir As Short = 21            'ｸﾞﾙｰﾌﾟ間隔X
    Public Const glNo_GroupItvYDir As Short = 22            'ｸﾞﾙｰﾌﾟ間隔Y
    Public Const glNo_CircuitSizeXDir As Short = 103        'サーキットｻｲｽﾞX
    Public Const glNo_CircuitSizeYDir As Short = 104        'サーキットｻｲｽﾞY
    Public Const glNo_ChipSizeXDir As Short = 23            'ﾁｯﾌﾟｻｲｽﾞX
    Public Const glNo_ChipSizeYDir As Short = 24            'ﾁｯﾌﾟｻｲｽﾞY
    Public Const glNo_StepOffsetXDir As Short = 25          'ｽﾃｯﾌﾟｵﾌｾｯﾄ量X
    Public Const glNo_StepOffsetYDir As Short = 26          'ｽﾃｯﾌﾟｵﾌｾｯﾄ量Y
    Public Const glNo_BlockSizeReviseXDir As Short = 27     'ﾌﾞﾛｯｸｻｲｽﾞ補正X
    Public Const glNo_BlockSizeReviseYDir As Short = 28     'ﾌﾞﾛｯｸｻｲｽﾞ補正Y
    Public Const glNo_BlockItvXDir As Short = 29            'ﾌﾞﾛｯｸ間隔X
    Public Const glNo_BlockItvYDir As Short = 30            'ﾌﾞﾛｯｸ間隔Y
    Public Const glNo_ContHiNgBlockCnt As Short = 31        '連続NG-HIGH抵抗ﾌﾞﾛｯｸ数
    Public Const glNo_DistStepRepeat As Short = 32          'ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄ移動量　未使用
    '
    Public Const glNo_ReviseMode As Short = 33              '補正ﾓｰﾄﾞ
    Public Const glNo_ManualReviseType As Short = 34        '補正方法
    Public Const glNo_ReviseCordnt1XDir As Short = 35       '補正位置座標1X
    Public Const glNo_ReviseCordnt1YDir As Short = 36       '補正位置座標1Y
    Public Const glNo_ReviseCordnt2XDir As Short = 37       '補正位置座標2X
    Public Const glNo_ReviseCordnt2YDir As Short = 38       '補正位置座標2Y
    Public Const glNo_ReviseOffsetXDir As Short = 39        '補正ﾎﾟｼﾞｼｮﾝｵﾌｾｯﾄX
    Public Const glNo_ReviseOffsetYDir As Short = 40        '補正ﾎﾟｼﾞｼｮﾝｵﾌｾｯﾄY
    Public Const glNo_RecogDispMode As Short = 41           '認識ﾃﾞｰﾀ表示ﾓｰﾄﾞ
    Public Const glNo_PixelValXDir As Short = 42            'ﾋﾟｸｾﾙ値X
    Public Const glNo_PixelValYDir As Short = 43            'ﾋﾟｸｾﾙ値Y
    Public Const glNo_RevisePtnNo1 As Short = 44            '補正位置ﾊﾟﾀｰﾝNo1
    Public Const glNo_RevisePtnNo2 As Short = 45            '補正位置ﾊﾟﾀｰﾝNo2
    Public Const glNo_RevisePtnGrpNo1 As Short = 98         '補正位置ﾊﾟﾀｰﾝNo1グループNo
    Public Const glNo_RevisePtnGrpNo2 As Short = 99         '補正位置ﾊﾟﾀｰﾝNo2グループNo
    Public Const glNo_RotateXDir As Short = 46              'X方向の回転中心
    Public Const glNo_RotateYDir As Short = 47              'Y方向の回転中心
    Public Const glNo_RotateTheta As Short = 48             'θ軸 

    Public Const glNo_TThetaOffset As Short = 91            'Ｔθオフセット（deg）
    Public Const glNo_TThetaBase1XDir As Short = 92         'Ｔθ基準位置１X（mm）
    Public Const glNo_TThetaBase1YDir As Short = 93         'Ｔθ基準位置１Y（mm）
    Public Const glNo_TThetaBase2XDir As Short = 94         'Ｔθ基準位置２X（mm）
    Public Const glNo_TThetaBase2YDir As Short = 95         'Ｔθ基準位置２Y（mm）

    Public Const glNo_CaribBaseCordnt1XDir As Short = 49    'ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標1X
    Public Const glNo_CaribBaseCordnt1YDir As Short = 50    'ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標1Y
    Public Const glNo_CaribBaseCordnt2XDir As Short = 51    'ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標2X
    Public Const glNo_CaribBaseCordnt2YDir As Short = 52    'ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標2Y
    Public Const glNo_CaribTableOffsetXDir As Short = 53    'ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄX
    Public Const glNo_CaribTableOffsetYDir As Short = 54    'ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄY
    Public Const glNo_CaribPtnNo1 As Short = 55             'ｷｬﾘﾌﾞﾚｰｼｮﾝﾊﾟﾀｰﾝ登録No1
    Public Const glNo_CaribPtnNo2 As Short = 56             'ｷｬﾘﾌﾞﾚｰｼｮﾝﾊﾟﾀｰﾝ登録No2
    Public Const glNo_CaribPtnNo1GroupNo As Short = 100     'ｷｬﾘﾌﾞﾚｰｼｮﾝﾊﾟﾀｰﾝ登録No1グループNo
    Public Const glNo_CaribPtnNo2GroupNo As Short = 101     'ｷｬﾘﾌﾞﾚｰｼｮﾝﾊﾟﾀｰﾝ登録No2グループNo
    Public Const glNo_CaribCutLength As Short = 57          'ｷｬﾘﾌﾞﾚｰｼｮﾝｶｯﾄ長
    Public Const glNo_CaribCutSpeed As Short = 58           'ｷｬﾘﾌﾞﾚｰｼｮﾝｶｯﾄ速度
    Public Const glNo_CaribCutQRate As Short = 59           'ｷｬﾘﾌﾞﾚｰｼｮﾝﾚｰｻﾞQﾚｰﾄ
    Public Const glNo_CutPosiReviseOffsetXDir As Short = 60 'ｶｯﾄ点補正ﾃｰﾌﾞﾙｵﾌｾｯﾄX
    Public Const glNo_CutPosiReviseOffsetYDir As Short = 61 'ｶｯﾄ点補正ﾃｰﾌﾞﾙｵﾌｾｯﾄY
    Public Const glNo_CutPosiRevisePtnNo As Short = 62      'ｶｯﾄ点補正ﾊﾟﾀｰﾝ登録No
    Public Const glNo_CutPosiReviseCutLength As Short = 63  'ｶｯﾄ点補正ｶｯﾄ長
    Public Const glNo_CutPosiReviseCutSpeed As Short = 64   'ｶｯﾄ点補正ｶｯﾄ速度
    Public Const glNo_CutPosiReviseCutQRate As Short = 65   'ｶｯﾄ点補正ﾚｰｻﾞQﾚｰﾄ
    Public Const glNo_CutPosiReviseGroupNo As Short = 66    'ｸﾞﾙｰﾌﾟNo

    '以下は436K用のパラメータの為、一旦コメントアウト
    'Public Const glNo_MaxTrimNgCount            As Integer = 77 'ﾄﾘﾐﾝｸﾞNGｶｳﾝﾀ(上限)
    'Public Const glNo_MaxBreakDischargeCount    As Integer = 78 '割れ欠け排出ｶｳﾝﾀ(上限)
    'Public Const glNo_TrimNgCount               As Integer = 79 '連続ﾄﾘﾐﾝｸﾞNG枚数
    'Public Const glNo_InitialOkTestDo           As Integer = 84 'ｲﾆｼｬﾙOKﾃｽﾄ
    'Public Const glNo_WorkSetByLoader           As Integer = 85 '基板品種

    'オプション機能
    Public Const glNo_RetryProbeCount As Short = 67         '再ﾌﾟﾛｰﾋﾞﾝｸﾞ回数
    Public Const glNo_RetryProbeDistance As Short = 68      '再ﾌﾟﾛｰﾋﾞﾝｸﾞ移動量
    Public Const glNo_LedCtrl As Short = 69                 'LED制御

    Public Const glNo_GpibMode As Short = 70                'GP-IB制御(ROHM)
    Public Const glNo_GpibDelim As Short = 71               '初期設定(ﾃﾞﾘﾐﾀ)       
    Public Const glNo_GpibTimeOut As Short = 72             '初期設定(ﾀｲﾑｱｳﾄ)      
    Public Const glNo_GpibAddress As Short = 73             '初期設定(機器ｱﾄﾞﾚｽ)   
    Public Const glNo_GpibInitCmnd As Short = 74            '初期化ｺﾏﾝﾄﾞ            
    Public Const glNo_GpibInit2Cmnd As Short = 75           '初期化ｺﾏﾝﾄﾞ            
    Public Const glNo_GpibTrigCmnd As Short = 76            'ﾄﾘｶﾞｺﾏﾝﾄﾞ            

    Public Const glNo_Gpib2Mode As Short = 87               'GP-IB制御
    Public Const glNo_Gpib2Address As Short = 88            '初期設定(機器ｱﾄﾞﾚｽ)
    Public Const glNo_Gpib2MeasSpeed As Short = 89          '測定速度
    Public Const glNo_Gpib2MeasMode As Short = 90           '測定モード

    Public Const glNo_PowerAdjustMode As Short = 80         'ﾊﾟﾜｰ調整ﾓｰﾄﾞ
    Public Const glNo_PowerAdjustTarget As Short = 81       '調整目標ﾊﾟﾜｰ
    Public Const glNo_PowerAdjustQRate As Short = 82        'ﾊﾟﾜｰ調整Qﾚｰﾄ
    Public Const glNo_PowerAdjustToleLevel As Short = 83    'ﾊﾟﾜｰ調整許容範囲
    Public Const glNo_OpenCheck As Short = 86               '4端子ｵｰﾌﾟﾝﾁｪｯｸ
#End Region

#Region "トリミングデータ形式定義"
    '--------------------------------------------------------------------------
    '   プレートデータ構造体形式定義
    '--------------------------------------------------------------------------
    Public Structure PlateInfo
        '----- プレートデータ１ -----
        Dim strDataName As String                           ' トリミングデータ名  　　　　※変更(Uカットデータはカットデータに移動)     
        Dim intMeasType As Short                            ' トリムモード(0:抵抗 ,1:電圧)※未使用 抵抗データの測定モード(0:抵抗 ,1:電圧 ,2:外部)へ移動 
        Dim intDirStepRepeat As Short                       ' ｽﾃｯﾌﾟ＆ﾘﾋﾟｰﾄ
        Dim intPlateCntXDir As Short                        ' プレート数X 
        Dim intPlateCntYDir As Short                        ' プレート数Y 
        Dim intBlockCntXDir As Short                        ' ﾌﾞﾛｯｸ数Ｘ
        Dim intBlockCntYDir As Short                        ' ﾌﾞﾛｯｸ数Ｙ
        Dim dblPlateItvXDir As Double                       ' プレート間隔Ｘ
        Dim dblPlateItvYDir As Double                       ' プレート間隔Ｙ
        Dim dblBlockSizeXDir As Double                      ' ブロックサイズＸ   
        Dim dblBlockSizeYDir As Double                      ' ブロックサイズＹ   
        Dim dblTableOffsetXDir As Double                    ' ﾃｰﾌﾞﾙ位置ｵﾌｾｯﾄX
        Dim dblTableOffsetYDir As Double                    ' ﾃｰﾌﾞﾙ位置ｵﾌｾｯﾄY
        Dim dblBpOffSetXDir As Double                       ' ﾋﾞｰﾑ位置ｵﾌｾｯﾄX
        Dim dblBpOffSetYDir As Double                       ' ﾋﾞｰﾑ位置ｵﾌｾｯﾄY
        Dim dblAdjOffSetXDir As Double                      ' ｱｼﾞｬｽﾄ位置ｵﾌｾｯﾄX(未使用)
        Dim dblAdjOffSetYDir As Double                      ' ｱｼﾞｬｽﾄ位置ｵﾌｾｯﾄY(未使用)
        Dim intCurcuitCnt As Short                          ' サーキット数  
        Dim intNGMark As Short                              ' NGﾏｰｷﾝｸﾞ
        Dim intDelayTrim As Short                           ' ﾃﾞｨﾚｲﾄﾘﾑ
        Dim intNgJudgeUnit As Short                         ' NG判定単位
        Dim intNgJudgeLevel As Short                        ' NG判定基準
        Dim dblZOffSet As Double                            ' ﾌﾟﾛｰﾌﾞZｵﾌｾｯﾄ(ZON位置)
        Dim dblZStepUpDist As Double                        ' ﾌﾟﾛｰﾌﾞｽﾃｯﾌﾟ上昇距離(Z軸ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄ位置)
        Dim dblZWaitOffset As Double                        ' ﾌﾟﾛｰﾌﾞ待機位置(ZOFF位置)
        '----- V1.13.0.0②↓ -----
        Dim dblLwPrbStpDwDist As Double                     ' 下方プローブステップ下降距離
        Dim dblLwPrbStpUpDist As Double                     ' 下方プローブステップ上昇距離
        '----- V1.13.0.0②↑ -----
        Dim intPrbRetryCount As Short                       ' プローブリトライ回数(0=リトライなし)(TKY側のみで使用INtime側へは送信しない)
        Dim intFinalJudge As Short                          ' ファイナル判定(option)   
        Dim intAttLevel As Short                            ' アッテネータ(%)(option)    
        '----- V1.23.0.0⑦↓ -----
        ' プレート１タブのプローブチェック項目(オプション)
        Dim intPrbChkPlt As Short                           ' 枚数
        Dim intPrbChkBlk As Short                           ' ブロック
        Dim dblPrbTestLimit As Double                       ' 誤差±%
        '----- V1.23.0.0⑦↑ -----

        '----- プレートデータ３ -----
        Dim intResistDir As Short                           ' 抵抗並び方向
        Dim intCircuitCntInBlock As Short                   ' 1ﾌﾞﾛｯｸ内ｻｰｷｯﾄ数
        Dim dblCircuitSizeXDir As Double                    ' ｻｰｷｯﾄｻｲｽﾞX         
        Dim dblCircuitSizeYDir As Double                    ' ｻｰｷｯﾄｻｲｽﾞY   
        Dim intResistCntInBlock As Short                    ' 1ブロック内抵抗数
        Dim intResistCntInGroup As Short                    ' 1グループ内抵抗数
        Dim intGroupCntInBlockXBp As Short                  ' ブロック内ＢＰグループ数(サーキット数)
        Dim intGroupCntInBlockYStage As Short               ' ブロック内ステージグループ数
        Dim dblGroupItvXDir As Double                       ' ｸﾞﾙｰﾌﾟ間隔X→（2010/11/09)dblBpGrpItvへ
        Dim dblGroupItvYDir As Double                       ' ｸﾞﾙｰﾌﾟ間隔Y→（2010/11/09)dblStgGrpItvへ
        Dim dblChipSizeXDir As Double                       ' ﾁｯﾌﾟｻｲｽﾞX
        Dim dblChipSizeYDir As Double                       ' ﾁｯﾌﾟｻｲｽﾞY
        '                                                   ' 千鳥ステップ時に対応。グループ内部でチップサイズ分ステップ移動する場合に使用する
        Dim intChipStepCnt As Short                         ' グループ内のチップステップ数。 ※追加
        Dim dblChipStepItv As Double                        ' グループ内チップステップ時のステップ間隔（基本はチップサイズ） ※追加
        Dim dblStepOffsetXDir As Double                     ' ｽﾃｯﾌﾟｵﾌｾｯﾄ量X
        Dim dblStepOffsetYDir As Double                     ' ｽﾃｯﾌﾟｵﾌｾｯﾄ量Y
        Dim dblBlockSizeReviseXDir As Double                ' ﾌﾞﾛｯｸｻｲｽﾞ補正X→(2010/11/11)CHIP未使用。NETではブロックサイズとして使用しているが使用方法を再検討の上削除する。
        Dim dblBlockSizeReviseYDir As Double                ' ﾌﾞﾛｯｸｻｲｽﾞ補正Y→(2010/11/11)CHIP未使用。NETではブロックサイズとして使用方法を再検討の上削除する。
        Dim dblBlockItvXDir As Double                       ' ﾌﾞﾛｯｸ間隔X
        Dim dblBlockItvYDir As Double                       ' ﾌﾞﾛｯｸ間隔Y
        Dim intContHiNgBlockCnt As Short                    ' 連続NG-HIGH抵抗ﾌﾞﾛｯｸ数
        'Dim intDistStepRepeat   As Double                  ' ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄ移動量 → 未使用

        '(2010/11/09)
        '   一時的にデータ追加-ブロックの考え方を新規に合わせるため、
        '   BPグループ間隔とステージグループ間隔の変数を追加
        Dim dblBpGrpItv As Double                           ' BPグループ間隔（以前のCHIPのグループ間隔）
        Dim dblPlateSizeX As Double                         ' プレートサイズX
        Dim dblPlateSizeY As Double                         ' プレートサイズY
        Dim intBpGrpCntInBlk As Short                       ' ブロック内BPグループ数
        Dim dblStgGrpItvX As Double                         ' X方向のステージグループ間隔（以前のＣＨＩＰのステップ間インターバル）
        Dim dblStgGrpItvY As Double                         ' Y方向のステージグループ間隔（以前のＣＨＩＰのステップ間インターバル）
        Dim intBlkCntInStgGrpX As Short                     ' X方向のステージグループ内ブロック数
        Dim intBlkCntInStgGrpY As Short                     ' Y方向のステージグループ内ブロック数
        Dim intStgGrpCntInPlt As Short                      ' プレート内ステージグループ数→不要かも

        '----- プレートデータ２ -----
        Dim intReviseMode As Short                          ' 補正モード(0:自動,1:手動, 2:自動+微調)
        Dim intManualReviseType As Short                    ' 補正方法(0:補正なし, 1:1回のみ, 2:毎回)
        Dim dblReviseCordnt1XDir As Double                  ' 補正位置座標1X
        Dim dblReviseCordnt1YDir As Double                  ' 補正位置座標1Y
        Dim dblReviseCordnt2XDir As Double                  ' 補正位置座標2X
        Dim dblReviseCordnt2YDir As Double                  ' 補正位置座標2Y
        Dim dblReviseOffsetXDir As Double                   ' 補正ﾎﾟｼﾞｼｮﾝｵﾌｾｯﾄX
        Dim dblReviseOffsetYDir As Double                   ' 補正ﾎﾟｼﾞｼｮﾝｵﾌｾｯﾄY
        Dim intRecogDispMode As Short                       ' 認識ﾃﾞｰﾀ表示ﾓｰﾄﾞ(0:表示なし, 1:表示する)
        Dim dblPixelValXDir As Double                       ' ﾋﾟｸｾﾙ値X
        Dim dblPixelValYDir As Double                       ' ﾋﾟｸｾﾙ値Y
        Dim intRevisePtnNo1 As Short                        ' 補正位置ﾊﾟﾀｰﾝNo1
        Dim intRevisePtnNo2 As Short                        ' 補正位置ﾊﾟﾀｰﾝNo2
        Dim intRevisePtnNo1GroupNo As Short                 ' 補正位置ﾊﾟﾀｰﾝNo1グループNo
        Dim intRevisePtnNo2GroupNo As Short                 ' 補正位置ﾊﾟﾀｰﾝNo2グループNo
        Dim dblRotateXDir As Double                         ' X方向の回転中心
        Dim dblRotateYDir As Double                         ' Y方向の回転中心
        Dim dblRotateTheta As Double                        ' θ回転角度
        'V5.0.0.9①                              ↓ クランプレス・ラフアライメント用
        Dim intReviseExecRgh As Short                       ' 補正有無(0:補正なし, 1:補正あり)
        Dim dblReviseCordnt1XDirRgh As Double               ' 補正位置座標1X
        Dim dblReviseCordnt1YDirRgh As Double               ' 補正位置座標1Y
        Dim dblReviseCordnt2XDirRgh As Double               ' 補正位置座標2X
        Dim dblReviseCordnt2YDirRgh As Double               ' 補正位置座標2Y
        Dim dblReviseOffsetXDirRgh As Double                ' 補正ﾎﾟｼﾞｼｮﾝｵﾌｾｯﾄX
        Dim dblReviseOffsetYDirRgh As Double                ' 補正ﾎﾟｼﾞｼｮﾝｵﾌｾｯﾄY
        Dim intRecogDispModeRgh As Short                    ' 認識ﾃﾞｰﾀ表示ﾓｰﾄﾞ(0:表示なし, 1:表示する)
        Dim intRevisePtnNo1Rgh As Short                     ' 補正位置ﾊﾟﾀｰﾝNo1
        Dim intRevisePtnNo2Rgh As Short                     ' 補正位置ﾊﾟﾀｰﾝNo2
        Dim intRevisePtnNo1GroupNoRgh As Short              ' 補正位置ﾊﾟﾀｰﾝNo1グループNo
        Dim intRevisePtnNo2GroupNoRgh As Short              ' 補正位置ﾊﾟﾀｰﾝNo2グループNo
        'V5.0.0.9①                              ↑

        '----- プレートデータ４ -----
        Dim dblCaribBaseCordnt1XDir As Double               ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標1X
        Dim dblCaribBaseCordnt1YDir As Double               ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標1Y
        Dim dblCaribBaseCordnt2XDir As Double               ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標2X
        Dim dblCaribBaseCordnt2YDir As Double               ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標2Y
        Dim dblCaribTableOffsetXDir As Double               ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄX
        Dim dblCaribTableOffsetYDir As Double               ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄY
        Dim intCaribPtnNo1 As Short                         ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾊﾟﾀｰﾝ登録No1
        Dim intCaribPtnNo2 As Short                         ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾊﾟﾀｰﾝ登録No2
        Dim intCaribPtnNo1GroupNo As Short                  ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾊﾟﾀｰﾝ登録No1グループNo
        Dim intCaribPtnNo2GroupNo As Short                  ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾊﾟﾀｰﾝ登録No2グループNo
        Dim dblCaribCutLength As Double                     ' ｷｬﾘﾌﾞﾚｰｼｮﾝｶｯﾄ長
        Dim dblCaribCutSpeed As Double                      ' ｷｬﾘﾌﾞﾚｰｼｮﾝｶｯﾄ速度
        Dim dblCaribCutQRate As Double                      ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾚｰｻﾞQﾚｰﾄ
        Dim intCaribCutCondNo As Short                      ' ｷｬﾘﾌﾞﾚｰｼｮﾝ加工条件番号(FL用)

        Dim dblCutPosiReviseOffsetXDir As Double            ' ｶｯﾄ点補正ﾃｰﾌﾞﾙｵﾌｾｯﾄX
        Dim dblCutPosiReviseOffsetYDir As Double            ' ｶｯﾄ点補正ﾃｰﾌﾞﾙｵﾌｾｯﾄY
        Dim intCutPosiRevisePtnNo As Short                  ' ｶｯﾄ点補正ﾊﾟﾀｰﾝ登録No
        Dim dblCutPosiReviseCutLength As Double             ' ｶｯﾄ点補正ｶｯﾄ長
        Dim dblCutPosiReviseCutSpeed As Double              ' ｶｯﾄ点補正ｶｯﾄ速度
        Dim dblCutPosiReviseCutQRate As Double              ' ｶｯﾄ点補正ﾚｰｻﾞQﾚｰﾄ
        Dim intCutPosiReviseGroupNo As Short                ' ｸﾞﾙｰﾌﾟNo
        Dim intCutPosiReviseCondNo As Short                 ' カット位置補正加工条件番号(FL用)

        '----- プレートデータ５ -----
        Dim intMaxTrimNgCount As Short                      ' ﾄﾘﾐﾝｸﾞNGｶｳﾝﾀ(上限)
        Dim intMaxBreakDischargeCount As Short              ' 割れ欠け排出ｶｳﾝﾀ(上限)
        Dim intTrimNgCount As Short                         ' 連続ﾄﾘﾐﾝｸﾞNG枚数
        Dim intContHiNgResCnt As Short                      ' 連続ﾄﾘﾐﾝｸﾞNG抵抗数 ###230
        Dim intNgJudgeStop As Short                         ' NG判定時停止  V1.13.0.0②

        Dim intRetryProbeCount As Short                     ' 再ﾌﾟﾛｰﾋﾞﾝｸﾞ回数
        Dim dblRetryProbeDistance As Double                 ' 再ﾌﾟﾛｰﾋﾞﾝｸﾞ移動量

        Dim intPowerAdjustMode As Short                     ' ﾊﾟﾜｰ調整ﾓｰﾄﾞ
        Dim intPwrChkPltNum As Short                        ' チェック基板枚数 V4.11.0.0①
        Dim intPwrChkTime As Short                          ' チェック時間(分) V4.11.0.0①
        Dim dblPowerAdjustTarget As Double                  ' 調整目標ﾊﾟﾜｰ
        Dim dblPowerAdjustQRate As Double                   ' ﾊﾟﾜｰ調整Qﾚｰﾄ
        Dim dblPowerAdjustToleLevel As Double               ' ﾊﾟﾜｰ調整許容範囲
        Dim intPowerAdjustCondNo As Short                   ' ﾊﾟﾜｰ調整加工条件番号(FL用)

        Dim intInitialOkTestDo As Short                     ' ｲﾆｼｬﾙOKﾃｽﾄ
        Dim intWorkSetByLoader As Short                     ' 基板品種
        Dim intOpenCheck As Short                           ' 4端子ｵｰﾌﾟﾝﾁｪｯｸ
        Dim intLedCtrl As Short                             ' LED制御
        Dim dblThetaAxis As Double                          ' θ軸

        Dim intGpibCtrl As Short                            ' GP-IB制御
        Dim intGpibDefDelimiter As Short                    ' 初期設定(ﾃﾞﾘﾐﾀ)
        Dim intGpibDefTimiout As Short                      ' 初期設定(ﾀｲﾑｱｳﾄ)
        Dim intGpibDefAdder As Short                        ' 初期設定(機器ｱﾄﾞﾚｽ)
        Dim strGpibInitCmnd1 As String                      ' 初期化ｺﾏﾝﾄﾞ1
        Dim strGpibInitCmnd2 As String                      ' 初期化ｺﾏﾝﾄﾞ2
        Dim strGpibTriggerCmnd As String                    ' ﾄﾘｶﾞｺﾏﾝﾄﾞ
        Dim intGpibMeasSpeed As Short                       ' 測定速度
        Dim intGpibMeasMode As Short                        ' 測定モード

        Dim dblTThetaOffset As Double                       ' Ｔθオフセット
        Dim dblTThetaBase1XDir As Double                    ' Ｔθ基準位置１X（mm）
        Dim dblTThetaBase1YDir As Double                    ' Ｔθ基準位置１Y（mm）
        Dim dblTThetaBase2XDir As Double                    ' Ｔθ基準位置２X（mm）
        Dim dblTThetaBase2YDir As Double                    ' Ｔθ基準位置２Y（mm）

        '----- プレートデータ６ (ﾌﾟﾚｰﾄﾀﾌﾞ3(V1.13.0.0②)) -----
        Dim intContExpMode As Short                         ' 伸縮補正 (0:なし, 1:あり)
        Dim intContExpGrpNo As Short                        ' 伸縮補正ｸﾞﾙｰﾌﾟ番号
        Dim intContExpPtnNo As Short                        ' 伸縮補正ﾊﾟﾀｰﾝ番号
        Dim dblContExpPosX As Double                        ' 伸縮補正位置X (mm)
        Dim dblContExpPosY As Double                        ' 伸縮補正位置XY (mm)            
        Dim intStepMeasCnt As Short                         ' ｽﾃｯﾌﾟ測定回数
        Dim dblStepMeasPitch As Double                      ' ｽﾃｯﾌﾟ測定ﾋﾟｯﾁ
        Dim intStepMeasReptCnt As Short                     ' ｽﾃｯﾌﾟ測定繰り返しｽﾃｯﾌﾟ回数
        Dim dblStepMeasReptPitch As Double                  ' ｽﾃｯﾌﾟ測定繰り返しｽﾃｯﾌﾟﾋﾟｯﾁ
        Dim intStepMeasLwGrpNo As Short                     ' ｽﾃｯﾌﾟ測定下方ﾌﾟﾛｰﾌﾞｸﾞﾙｰﾌﾟ番号
        Dim intStepMeasLwPtnNo As Short                     ' ｽﾃｯﾌﾟ測定下方ﾌﾟﾛｰﾌﾞﾊﾟﾀｰﾝ番号
        Dim dblStepMeasBpPosX As Double                     ' ｽﾃｯﾌﾟ測定BP位置X
        Dim dblStepMeasBpPosY As Double                     ' ｽﾃｯﾌﾟ測定BP位置Y
        Dim intStepMeasUpGrpNo As Short                     ' ｽﾃｯﾌﾟ測定基板上ﾌﾟﾛｰﾌﾞｸﾞﾙｰﾌﾟ番号
        Dim intStepMeasUpPtnNo As Short                     ' ｽﾃｯﾌﾟ測定基板上ﾌﾟﾛｰﾌﾞﾊﾟﾀｰﾝ番号
        Dim dblStepMeasTblOstX As Double                    ' ｽﾃｯﾌﾟ測定基板上ﾌﾟﾛｰﾌﾞﾃｰﾌﾞﾙｵﾌｾｯﾄX
        Dim dblStepMeasTblOstY As Double                    ' ｽﾃｯﾌﾟ測定基板上ﾌﾟﾛｰﾌﾞﾃｰﾌﾞﾙｵﾌｾｯﾄY
        Dim intIDReaderUse As Double                        ' IDﾘｰﾄﾞ (0:未使用, 1:使用)
        Dim dblIDReadPos1X As Double                        ' IDﾘｰﾄﾞ読み取りﾎﾟｼﾞｼｮﾝ 1X
        Dim dblIDReadPos1Y As Double                        ' IDﾘｰﾄﾞ読み取りﾎﾟｼﾞｼｮﾝ 1Y
        Dim dblIDReadPos2X As Double                        ' IDﾘｰﾄﾞ読み取りﾎﾟｼﾞｼｮﾝ 2X
        Dim dblIDReadPos2Y As Double                        ' IDﾘｰﾄﾞ読み取りﾎﾟｼﾞｼｮﾝ 2Y
        Dim dblReprobeVar As Double                         ' 再ﾌﾟﾛｰﾋﾞﾝｸﾞばらつき量
        Dim dblReprobePitch As Double                       ' 再ﾌﾟﾛｰﾋﾞﾝｸﾞﾋﾟｯﾁ
        '----- V2.0.0.0_23↓ -----
        '----- プローブクリーニング項目 -----(TKY側のみで使用INtime側へは送信しない)
        Dim dblPrbCleanPosX As Double                       ' クリーニング位置X
        Dim dblPrbCleanPosY As Double                       ' クリーニング位置Y
        Dim dblPrbCleanPosZ As Double                       ' クリーニング位置Z
        Dim intPrbCleanUpDwCount As Short                   ' プローブ上下回数
        Dim intPrbCleanAutoSubCount As Short                '  自動運転時クリーニング実行基板枚数
        '----- V2.0.0.0_23↑ -----

        'V4.10.0.0④           ↓
        Public dblPrbCleanStagePitchX As Double             ' ステージ動作ピッチX
        Public dblPrbCleanStagePitchY As Double             ' ステージ動作ピッチY
        Public intPrbCleanStageCountX As Short              ' ステージ動作回数X
        Public intPrbCleanStageCountY As Short              ' ステージ動作回数Y
        'V4.10.0.0④           ↑
        'V4.10.0.0⑨↓
        Public dblPrbDistance As Double                     ' プローブ間距離（mm）
        Public dblPrbCleaningOffset As Double               ' クリーニングオフセット(mm)
        'V4.10.0.0⑨↑
        Dim intControllerInterlock As Integer               ' 外部機器によるインターロックの有無 'V5.0.0.6①

        Dim dblTXChipsizeRelationX As Double                ' 補正位置１と２の相対値Ｘ 'V4.5.1.0⑮
        Dim dblTXChipsizeRelationY As Double                ' 補正位置１と２の相対値Ｙ 'V4.5.1.0⑮

    End Structure

    '----- ###229↓ -----
    '--------------------------------------------------------------------------
    '   GPIBデータ(汎用)構造体形式定義
    '--------------------------------------------------------------------------
    Public Structure GpibInfo
        Dim wGPIBmode As UShort                                         ' GP-IB制御(0:しない 1:する)
        Dim wDelim As UShort                                            ' ﾃﾞﾘﾐﾀ(0:CR+LF 1:CR 2:LF 3:NONE)
        Dim wTimeout As UShort                                          ' ﾀｲﾑｱｳﾄ(1～32767)(ms単位)
        Dim wAddress As UShort                                          ' 機器ｱﾄﾞﾚｽ(0～30)
        Dim wEOI As UShort                                              ' EOI(0:使用しない, 1:使用する)
        Dim wPause1 As UShort                                           ' 設定ｺﾏﾝﾄﾞ1送信後ポーズ時間(1～32767msec)
        Dim wPause2 As UShort                                           ' 設定ｺﾏﾝﾄﾞ2送信後ポーズ時間(1～32767msec)
        Dim wPause3 As UShort                                           ' 設定ｺﾏﾝﾄﾞ3送信後ポーズ時間(1～32767msec)
        Dim wPauseT As UShort                                           ' ﾄﾘｶﾞｺﾏﾝﾄﾞ送信後ポーズ時間(1～32767msec)
        Dim wRev As UShort                                              ' 予備
        Dim strI As String                                              ' 初期化ｺﾏﾝﾄﾞ(MAX40byte)
        Dim strI2 As String                                             ' 初期化ｺﾏﾝﾄﾞ2(MAX40byte)
        Dim strI3 As String                                             ' 初期化ｺﾏﾝﾄﾞ3(MAX40byte)
        Dim strT As String                                              ' ﾄﾘｶﾞｺﾏﾝﾄﾞ(50byte)
        Dim strName As String                                           ' 機器名(10byte)
        Dim wReserve As String                                          ' 予備(8byte)  
    End Structure
    '----- ###229↑ -----

    '--------------------------------------------------------------------------
    '   ステップデータ構造体形式定義
    '--------------------------------------------------------------------------
    Public Structure StepInfo
        Dim intSP1 As Short                                 ' ｽﾃｯﾌﾟ番号
        Dim intSP2 As Short                                 ' ﾌﾞﾛｯｸ数
        Dim dblSP3 As Double                                ' ｽﾃｯﾌﾟ間ｲﾝﾀｰﾊﾞﾙ
    End Structure

    '--------------------------------------------------------------------------
    '   グループデータ構造体形式定義
    '--------------------------------------------------------------------------
    Public Structure GrpInfo
        Dim intGP1 As Short                                 ' ｸﾞﾙｰﾌﾟ番号
        Dim intGP2 As Short                                 ' 抵抗数
        Dim dblGP3 As Double                                ' ｸﾞﾙｰﾌﾟ間ｲﾝﾀｰﾊﾞﾙ
        Dim dblStgPosX As Double                            ' ステージXポジション
        Dim dblStgPosY As Double                            ' ステージYポジション
    End Structure

    '--------------------------------------------------------------------------
    '   サーキットデータ構造体形式定義
    '--------------------------------------------------------------------------
    Public Structure CircuitInfo
        Dim intIP1 As Short                                 ' IP番号
        Dim dblIP2X As Double                               ' マーキングX
        Dim dblIP2Y As Double                               ' マーキングY
    End Structure

    '--------------------------------------------------------------------------
    '   サーキット座標データ構造体形式定義
    '--------------------------------------------------------------------------
    Public Structure CirAxisInfo
        Dim intCaP1 As Short                                ' 番号
        Dim dblCaP2 As Double                               ' 座標X
        Dim dblCaP3 As Double                               ' 座標Y
    End Structure

    '-------------------------------------------------------------------------
    '   サーキット間インターバルデータ構造体形式定義
    '-------------------------------------------------------------------------
    Public Structure CirInInfo
        Dim intCiP1 As Short                                ' ｽﾃｯﾌﾟ番号
        Dim intCiP2 As Short                                ' ｻｰｷｯﾄ数
        Dim dblCiP3 As Double                               ' ｻｰｷｯﾄ間ｲﾝﾀｰﾊﾞﾙ
    End Structure

    '--------------------------------------------------------------------------
    '   カットデータ構造体形式定義
    '--------------------------------------------------------------------------
    Public Structure CutList
        Dim intCutNo As Short                               ' ｶｯﾄ番号(1～n)
        Dim intDelayTime As Short                           ' ﾃﾞｨﾚｲﾀｲﾑ
        Dim strCutType As String                            ' ｶｯﾄ形状
        Dim dblTeachPointX As Double                        ' ﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄX
        Dim dblTeachPointY As Double                        ' ﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄY
        Dim dblStartPointX As Double                        ' ｽﾀｰﾄﾎﾟｲﾝﾄX
        Dim dblStartPointY As Double                        ' ｽﾀｰﾄﾎﾟｲﾝﾄY
        Dim dblCutSpeed As Double                           ' ｶｯﾄｽﾋﾟｰﾄﾞ
        Dim dblQRate As Double                              ' Qｽｲｯﾁﾚｰﾄ
        Dim dblCutOff As Double                             ' ｶｯﾄｵﾌ値
        Dim dblJudgeLevel As Double                         ' 切替ポイント (旧ﾃﾞｰﾀ判定(平均化率))
        Dim dblCutOffOffset As Double                       ' ｶｯﾄｵﾌｵﾌｾｯﾄ 

        Dim intPulseWidthCtrl As Short                      ' ﾊﾟﾙｽ幅制御
        Dim dblPulseWidthTime As Double                     ' ﾊﾟﾙｽ幅時間
        Dim dblLSwPulseWidthTime As Double                  ' LSwﾊﾟﾙｽ幅時間(外部ｼｬｯﾀ)

        Dim intCutDir As Short                              ' ｶｯﾄ方向(1:0°, 2:90°, 3:180°, 4:270) ※変更
        Dim intLTurnDir As Short                            ' Lﾀｰﾝ方向(1:CW, 2:CCW) ※変更
        Dim dblMaxCutLength As Double                       ' 最大ｶｯﾃｨﾝｸﾞ長
        Dim dblLTurnPoint As Double                         ' Lﾀｰﾝﾎﾟｲﾝﾄ
        Dim dblMaxCutLengthL As Double                      ' Lﾀｰﾝ後の最大ｶｯﾃｨﾝｸﾞ長
        Dim dblMaxCutLengthHook As Double                   ' ﾌｯｸﾀｰﾝ後のｶｯﾃｨﾝｸﾞ長
        Dim dblR1 As Double                                 ' R1
        Dim dblR2 As Double                                 ' R2
        Dim intCutAngle As Short                            ' 斜めｶｯﾄの切り出し角度
        Dim dblCutSpeed2 As Double                          ' ｶｯﾄｽﾋﾟｰﾄﾞ2
        Dim dblQRate2 As Double                             ' Qｽｲｯﾁﾚｰﾄ2
        Dim dblESPoint As Double                            ' ｴｯｼﾞｾﾝｽﾎﾟｲﾝﾄ
        Dim dblESJudgeLevel As Double                       ' ｴｯｼﾞｾﾝｽの判定変化率
        Dim dblMaxCutLengthES As Double                     ' ｴｯｼﾞｾﾝｽ後のｶｯﾄ長
        Dim intIndexCnt As Short                            ' ｲﾝﾃﾞｯｸｽ数
        Dim intMeasMode As Short                            ' 測定モード(0:抵抗 ,1:電圧 ,2:外部)
        Dim dblPitch As Double                              ' ﾋﾟｯﾁ
        Dim intStepDir As Short                             ' ｽﾃｯﾌﾟ方向(0:0°, 1:90°, 2:180°, 3:270) ※変更
        Dim intCutCnt As Short                              ' 本数
        Dim dblUCutDummy1 As Double                         ' Uｶｯﾄ用(現在ﾀﾞﾐｰ)
        Dim dblUCutDummy2 As Double                         ' Uｶｯﾄ用(現在ﾀﾞﾐｰ)
        Dim dblZoom As Double                               ' 倍率
        Dim strChar As String                               ' 文字列
        Dim dblESChangeRatio As Double                      ' ｴｯｼﾞｾﾝｽ後変化率
        Dim intESConfirmCnt As Short                        ' ｴｯｼﾞｾﾝｽ後確認回数
        Dim intRadderInterval As Short                      ' ﾗﾀﾞｰ間距離
        Dim intCTcount As Short                             ' ｴｯｼﾞｾﾝｽ後連続NG確認回数※追加(ES用)
        Dim intJudgeNg As Short                             ' NG判定する/しない(0:TRUE/1:FALSE)※追加(ES用)
        Dim intMeasType As Short                            ' 測定タイプ(0:高速 ,1:高精度)　※追加(IX用)
        Dim intMoveMode As Short                            ' 動作モード(0:通常モード, 2:強制カットモード)　※追加
        Dim intDoPosition As Short                          ' ポジショニング(0:有, 1:無)　※追加
        Dim intCutAftPause As Short                         ' カット後ポーズタイム（ｍｓ）V2.0.0.0_24(V1.18.0.3①)(ローム殿対応)
        Dim dblReturnPos As Double                          ' リターンカットのリターン位置 'V1.16.0.0①
        Dim dblLimitLen As Double                           ' IXカットのリミット長 'V1.18.0.0④
        Dim strDataName As String                           ' Uカットデータ名※追加      

        ' FL用に加工条件を追加
        Dim dblCutSpeed3 As Double                          ' ｶｯﾄｽﾋﾟｰﾄﾞ3(Lｶｯﾄのﾘﾀｰﾝ/ﾘﾄﾚｰｽ時のLﾀｰﾝ前のｽﾋﾟｰﾄﾞ)
        Dim dblCutSpeed4 As Double                          ' ｶｯﾄｽﾋﾟｰﾄﾞ4(Lｶｯﾄのﾘﾀｰﾝ/ﾘﾄﾚｰｽ時のLﾀｰﾝ後のｽﾋﾟｰﾄﾞ)
        Dim dblCutSpeed5 As Double                          ' ｶｯﾄｽﾋﾟｰﾄﾞ5(uｶｯﾄのﾘﾀｰﾝ/ﾘﾄﾚｰｽ時のLﾀｰﾝ後のｽﾋﾟｰﾄﾞ)
        Dim dblCutSpeed6 As Double                          ' ｶｯﾄｽﾋﾟｰﾄﾞ6(uｶｯﾄのﾘﾀｰﾝ/ﾘﾄﾚｰｽ時のLﾀｰﾝ後のｽﾋﾟｰﾄﾞ)
        <VBFixedArray(cCNDNUM)> Dim CndNum() As Short       ' 加工条件番号1～n(0～31) 
        <VBFixedArray(cCNDNUM)> Dim dblPowerAdjustTarget() As Double    ' 調整目標パワー1～n(0～31)     '###066
        <VBFixedArray(cCNDNUM)> Dim dblPowerAdjustToleLevel() As Double ' パワー調整許容範囲1～n(0～31) '###066

        '----- V2.0.0.0_23↓ -----
        ' SL436S用(シンプルトリマ用)
        Dim dblQRate3 As Double                             ' Qｽｲｯﾁﾚｰﾄ3(Lｶｯﾄのﾘﾀｰﾝ/ﾘﾄﾚｰｽ時のLﾀｰﾝ前のQｽｲｯﾁﾚｰﾄ)
        Dim dblQRate4 As Double                             ' Qｽｲｯﾁﾚｰﾄ4(Lｶｯﾄのﾘﾀｰﾝ/ﾘﾄﾚｰｽ時のLﾀｰﾝ後のQｽｲｯﾁﾚｰﾄ)
        Dim dblQRate5 As Double                             ' Qｽｲｯﾁﾚｰﾄ5(Uｶｯﾄのﾘﾀｰﾝ/ﾘﾄﾚｰｽ時のLﾀｰﾝ後のQｽｲｯﾁﾚｰﾄ) 未使用
        Dim dblQRate6 As Double                             ' Qｽｲｯﾁﾚｰﾄ6(Uｶｯﾄのﾘﾀｰﾝ/ﾘﾄﾚｰｽ時のLﾀｰﾝ後のQｽｲｯﾁﾚｰﾄ) 未使用
        <VBFixedArray(cCNDNUM)> Dim FLCurrent() As Short    ' 電流値1～8
        <VBFixedArray(cCNDNUM)> Dim FLSteg() As Short       ' STEG1～8
        '----- V2.0.0.0⑫↑ -----

        ' 構造体の初期化
        Public Sub Initialize()
            ReDim CndNum(cCNDNUM)                           ' 加工条件番号1～n(0～31) 
            ReDim dblPowerAdjustTarget(cCNDNUM)             ' 調整目標パワー1～n(0～31)     '###066
            ReDim dblPowerAdjustToleLevel(cCNDNUM)          ' パワー調整許容範囲1～n(0～31) '###066

            ReDim FLCurrent(cCNDNUM)                        ' 電流値1～8
            ReDim FLSteg(cCNDNUM)                           ' STEG1～8

        End Sub

    End Structure

    Public Const MaxCutInfo As Short = 30                   ' 最大ｶｯﾄ情報数

    '' この構造体は削除する
    'Public Structure CutInfo
    '    Dim intCRNO As Short                                ' 抵抗番号(1～9999)
    '    <VBFixedArray(MaxCutInfo)> Dim ArrCut() As CutList  ' ｶｯﾄ毎ｶｯﾄ情報

    '    ' 構造体の初期化
    '    Public Sub Initialize()
    '        ReDim ArrCut(MaxCutInfo)
    '    End Sub
    'End Structure

    '--------------------------------------------------------------------------
    '   抵抗データ構造体形式定義
    '--------------------------------------------------------------------------
    Public Structure ResistorInfo
        Dim intResNo As Short                               ' 抵抗番号(1～9999)
        Dim intResMeasMode As Short                         ' 測定モード(0:抵抗 ,1:電圧 ,2:外部) ※変更
        Dim intResMeasType As UShort                        ' 測定タイプ(0:高速 ,1:高精度)　※追加
        Dim intCircuitGrp As Short                          ' 所属ｻｰｷｯﾄ番号
        Dim intProbHiNo As Short                            ' ﾌﾟﾛｰﾌﾞ番号(HI)
        Dim intProbLoNo As Short                            ' ﾌﾟﾛｰﾌﾞ番号(LO)
        Dim intProbAGNo1 As Short                           ' ﾌﾟﾛｰﾌﾞ番号(1)
        Dim intProbAGNo2 As Short                           ' ﾌﾟﾛｰﾌﾞ番号(2)
        Dim intProbAGNo3 As Short                           ' ﾌﾟﾛｰﾌﾞ番号(3)
        Dim intProbAGNo4 As Short                           ' ﾌﾟﾛｰﾌﾞ番号(4)
        Dim intProbAGNo5 As Short                           ' ﾌﾟﾛｰﾌﾞ番号(5)
        Dim strExternalBits As String                       ' EXTERNAL BITS(16ﾋﾞｯﾄ)
        '    lngRP5      As Long
        ''''2009/06/01 →TKYではLong型見直し必要
        Dim intPauseTime As Short                           ' ﾎﾟｰｽﾞﾀｲﾑ(msec)
        Dim intTargetValType As Short                       ' 目標値指定（0:絶対値,1:レシオ,2:計算式）
        Dim intBaseResNo As Short                           ' ﾍﾞｰｽ抵抗番号
        Dim dblTrimTargetVal As Double                      ' ﾄﾘﾐﾝｸﾞ目標値
        Dim dblTrimTargetVal_Save As Double                 ' V4.11.0.0① ﾄﾘﾐﾝｸﾞ目標値退避用ワーク 
        Dim dblTrimTargetOfs As Double                      ' V4.11.0.0① ﾄﾘﾐﾝｸﾞ目標値ｵﾌｾｯﾄ(絶対値)
        Dim dblTrimTargetOfs_Save As Double                 ' 'V5.0.0.2① ﾄﾘﾐﾝｸﾞ目標値ｵﾌｾｯﾄ(％)
        Dim strRatioTrimTargetVal As String                 ' トリミング目標値(レシオ計算式) 
        Dim dblDeltaR As Double                             ' ΔR
        ''''2009/06/01 →  TKYではInteger型で電圧変化 スロープ
        Dim intSlope As Short                               ' 電圧変化ｽﾛｰﾌﾟ 
        Dim dblCutOffRatio As Double                        ' 切り上げ倍率
        Dim dblProbCfmPoint_Hi_X As Double                  ' プローブ確認位置 HI X座標 
        Dim dblProbCfmPoint_Hi_Y As Double                  ' プローブ確認位置 HI Y座標 
        Dim dblProbCfmPoint_Lo_X As Double                  ' プローブ確認位置 LO X座標 
        Dim dblProbCfmPoint_Lo_Y As Double                  ' プローブ確認位置 LO Y座標  
        Dim dblInitTest_HighLimit As Double                 ' ｲﾆｼｬﾙﾃｽﾄ(HIGHﾘﾐｯﾄ)
        Dim dblInitTest_LowLimit As Double                  ' ｲﾆｼｬﾙﾃｽﾄ(LOWﾘﾐｯﾄ)
        Dim dblFinalTest_HighLimit As Double                ' ﾌｧｲﾅﾙﾃｽﾄ(HIGHﾘﾐｯﾄ)
        Dim dblFinalTest_LowLimit As Double                 ' ﾌｧｲﾅﾙﾃｽﾄ(LOWﾘﾐｯﾄ)
        Dim dblInitOKTest_HighLimit As Double               ' ｲﾆｼｬﾙOKﾃｽﾄ(HIGHﾘﾐｯﾄ)
        Dim dblInitOKTest_LowLimit As Double                ' ｲﾆｼｬﾙOKﾃｽﾄ(LOWﾘﾐｯﾄ)
        Dim intInitialOkTestDo As Short                     ' 初期ＯＫ判定(0:しない,1:する)※追加(プレートデータから移動)
        Dim intCutCount As Short                            ' ｶｯﾄ数

        '----- TKY用 -----
        Dim intCutReviseMode As Short                       ' ｶｯﾄ補正(0:無し, 1:自動)       
        Dim intCutReviseDispMode As Short                   ' 表示ﾓｰﾄﾞ(0:無し, 1:CRT)        
        Dim intCutReviseGrpNo As Short                      ' ﾊﾟﾀｰﾝｸﾞﾙｰﾌﾟ番号      
        Dim intCutRevisePtnNo As Short                      ' ﾊﾟﾀｰﾝ番号      
        Dim dblCutRevisePosX As Double                      ' ｶｯﾄ補正位置X   
        Dim dblCutRevisePosY As Double                      ' ｶｯﾄ補正位置Y   
        Dim intIsNG As Short                                ' 画像認識NG判定(0:あり, 1:なし)

        '----- V1.13.0.0②↓ -----
        Dim intCvMeasNum As Short                           ' CV 最大測定回数
        Dim intCvMeasTime As Short                          ' CV 最大測定時間(ms) 
        Dim dblCvValue As Double                            ' CV CV値         
        Dim intOverloadNum As Short                         ' ｵｰﾊﾞｰﾛｰﾄﾞ 回数 
        Dim dblOverloadMin As Double                        ' ｵｰﾊﾞｰﾛｰﾄﾞ 下限値 
        Dim dblOverloadMax As Double                        ' ｵｰﾊﾞｰﾛｰﾄﾞ 上限値
        '----- V1.13.0.0②↑ -----
        '----- V2.0.0.0_23↓ -----
        Dim wPauseTimeFT As Short                           ' FT前のポーズタイム(0-32767msec) (ローム殿対応) V2.0.0.0_24
        Dim intInsideEndChkCount As Short                   ' 中切り判定回数(0-255)
        Dim dblInsideEndChgRate As Double                   ' 中切り判定変化率(0.00-100.00%)
        '----- V2.0.0.0_23↑ -----

        <VBFixedArray(MaxCutInfo)> Dim ArrCut() As CutList  ' ｶｯﾄ情報

        ' 構造体の初期化
        Public Sub Initialize()
            ReDim ArrCut(MaxCutInfo)
        End Sub

    End Structure

    '--------------------------------------------------------------------------
    '   Ty2データ構造体形式定義
    '--------------------------------------------------------------------------
    Public Structure Ty2Info
        Dim intTy21 As Short                                ' ﾌﾞﾛｯｸ番号
        Dim dblTy22 As Double                               ' ｽﾃｯﾌﾟ距離
    End Structure

    '--------------------------------------------------------------------------
    '   異形面付けデータ構造体形式定義
    '--------------------------------------------------------------------------
    Public Structure IKEIInfo
        Dim intI1 As Short                                  ' 異形面付けの有無（0:無し,1:X方向,2:Y方向）
        <VBFixedArray(MaxCntCircuit)> Dim intI2() As Short  ' サーキットの有無（0:無し,1:有り）

        ' 構造体の初期化
        Public Sub Initialize()
            ReDim intI2(MaxCntCircuit)
        End Sub
    End Structure
#End If
#End Region 'V5.0.0.8①
#End Region

#Region "トリミングデータの最小値/最大値チェック用構造体形式定義"
    '-------------------------------------------------------------------------------
    '   STEP(0)Min (1)Max
    '-------------------------------------------------------------------------------
    Public Structure SPInputArea
        <VBFixedArray(1)> Dim SP1() As Short                ' ｽﾃｯﾌﾟ番号
        <VBFixedArray(1)> Dim SP2() As Short                ' ﾌﾞﾛｯｸ数
        <VBFixedArray(1)> Dim SP3() As Double               ' ｽﾃｯﾌﾟ間ｲﾝﾀｰﾊﾞﾙ

        ' 構造体の初期化
        Public Sub Initialize()
            ReDim SP1(1)
            ReDim SP2(1)
            ReDim SP3(1)
        End Sub
    End Structure

    '-------------------------------------------------------------------------------
    '   GROUP(0)Min (1)Max 
    '-------------------------------------------------------------------------------
    Public Structure GPInputArea
        <VBFixedArray(1)> Dim GP1() As Short                ' ｸﾞﾙｰﾌﾟ番号
        <VBFixedArray(1)> Dim GP2() As Short                ' 抵抗数
        <VBFixedArray(1)> Dim GP3() As Double               ' ｸﾞﾙｰﾌﾟ間ｲﾝﾀｰﾊﾞﾙ

        ' 構造体の初期化
        Public Sub Initialize()
            ReDim GP1(1)
            ReDim GP2(1)
            ReDim GP3(1)
        End Sub
    End Structure

    '-------------------------------------------------------------------------------
    '   Ty2 (0)Min (1)Max
    '-------------------------------------------------------------------------------
    Public Structure Ty2InputArea
        <VBFixedArray(1)> Dim Ty1() As Short                ' ﾌﾞﾛｯｸ番号
        <VBFixedArray(1)> Dim Ty2() As Double               ' X
        <VBFixedArray(1)> Dim Ty3() As Double               ' Y

        ' 構造体の初期化
        Public Sub Initialize()
            ReDim Ty1(1)
            ReDim Ty2(1)
            ReDim Ty3(1)
        End Sub
    End Structure

    '-------------------------------------------------------------------------------
    '   RESISTOR(0)Min (1)Max
    '-------------------------------------------------------------------------------
    Public Structure RPInputArea
        <VBFixedArray(1)> Dim RP1() As Short                ' 抵抗番号
        <VBFixedArray(1)> Dim RP2() As Short                ' 高精度測定、判定ﾓｰﾄﾞ
        <VBFixedArray(1)> Dim RP2K() As Double              ' 高精度測定、判定ﾓｰﾄﾞ KOA匠の里殿
        <VBFixedArray(1)> Dim RP4HL() As Short              ' ﾌﾟﾛｰﾌﾞ番号(HI/Lo)
        <VBFixedArray(1)> Dim RP4AG() As Short              ' ﾌﾟﾛｰﾌﾞ番号(AG)
        <VBFixedArray(1)> Dim RP5() As Double               ' EXTERNAL BITS
        <VBFixedArray(1)> Dim RP6() As Double               ' ﾎﾟｰｽﾞﾀｲﾑ
        <VBFixedArray(1)> Dim RP9() As Double               ' ﾄﾘﾐﾝｸﾞ目標値
        <VBFixedArray(1)> Dim RP10() As Double              ' ΔＲ
        <VBFixedArray(1)> Dim RP15() As Double              ' 切り上げ倍率
        <VBFixedArray(1)> Dim RP11() As Double              ' ｲﾆｼｬﾙﾃｽﾄH/Lﾘﾐｯﾄ
        <VBFixedArray(1)> Dim RP12() As Double              ' ﾌｧｲﾅﾙﾃｽﾄH/Lﾘﾐｯﾄ
        <VBFixedArray(1)> Dim RP13() As Short               ' ｶｯﾄ数
        <VBFixedArray(1)> Dim RP14() As Double              ' ｲﾆｼｬﾙOKﾃｽﾄH/Lﾘﾐｯﾄ

        ' 構造体の初期化
        Public Sub Initialize()
            ReDim RP1(1)
            ReDim RP2(1)
            ReDim RP2K(1)
            ReDim RP4HL(1)
            ReDim RP4AG(1)
            ReDim RP5(1)
            ReDim RP6(1)
            ReDim RP9(1)
            ReDim RP10(1)
            ReDim RP15(1)
            ReDim RP11(1)
            ReDim RP12(1)
            ReDim RP13(1)
            ReDim RP14(1)
        End Sub
    End Structure

    '-------------------------------------------------------------------------------
    '   CUT(0)Min (1)Max
    '-------------------------------------------------------------------------------
    Public Structure CPInputArea
        <VBFixedArray(1)> Dim wCutNo() As Short                ' ｶｯﾄ番号
        <VBFixedArray(1)> Dim wDelayTime() As Short                ' ﾃﾞｨﾚｲﾀｲﾑ
        <VBFixedArray(1)> Dim wCutType() As String               ' ｶｯﾄ形状
        <VBFixedArray(1)> Dim CP99() As Double              ' ﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄXY
        <VBFixedArray(1)> Dim CP4() As Double               ' ｽﾀｰﾄﾎﾟｲﾝﾄXY
        <VBFixedArray(1)> Dim CP5() As Double               ' ｶｯﾄｽﾋﾟｰﾄﾞ
        <VBFixedArray(1)> Dim CP6() As Double               ' Qｽｲｯﾁﾚｰﾄ
        <VBFixedArray(1)> Dim fCutOff() As Double               ' ｶｯﾄｵﾌ値
        <VBFixedArray(1)> Dim CP7_2() As Double             ' ｶｯﾄｵﾌｵﾌｾｯﾄ
        <VBFixedArray(1)> Dim CP7_1() As Double             ' ﾃﾞｰﾀ判定(平均化率)基準
        <VBFixedArray(1)> Dim CP50() As Short               ' ﾊﾟﾙｽ幅制御
        <VBFixedArray(1)> Dim CP51() As Double              ' ﾊﾟﾙｽ幅時間
        <VBFixedArray(1)> Dim CP52() As Double              ' LSwﾊﾟﾙｽ幅時間(外部ｼｬｯﾀ)
        <VBFixedArray(1)> Dim CP8() As Short                ' ｶｯﾄ方向
        <VBFixedArray(1)> Dim CP9() As Double               ' 最大ｶｯﾃｨﾝｸﾞ長
        <VBFixedArray(1)> Dim CP11() As Double              ' Lﾀｰﾝﾎﾟｲﾝﾄ
        <VBFixedArray(1)> Dim CP14() As Double              ' Lﾀｰﾝ後の最大ｶｯﾃｨﾝｸﾞ長
        <VBFixedArray(1)> Dim CP15() As Double              ' ﾌｯｸﾀｰﾝ後のｶｯﾃｨﾝｸﾞ長
        <VBFixedArray(1)> Dim CP56() As Double              ' R1
        <VBFixedArray(1)> Dim CP57() As Double              ' R2
        <VBFixedArray(1)> Dim CP33() As Short               ' 斜めｶｯﾄの切り出し角度
        <VBFixedArray(1)> Dim CP36() As Double              ' ｶｯﾄｽﾋﾟｰﾄﾞ2
        <VBFixedArray(1)> Dim CP37() As Double              ' Qｽｲｯﾁﾚｰﾄ2
        <VBFixedArray(1)> Dim CP53() As Double              ' Qｽｲｯﾁﾚｰﾄ3
        <VBFixedArray(1)> Dim CP54() As Double              ' 切替えﾎﾟｲﾝﾄ 
        <VBFixedArray(1)> Dim CP38() As Double              ' ｴｯｼﾞｾﾝｽﾎﾟｲﾝﾄ
        <VBFixedArray(1)> Dim CP39() As Double              ' ｴｯｼﾞｾﾝｽの判定変化率
        <VBFixedArray(1)> Dim CP40() As Double              ' ｴｯｼﾞｾﾝｽ後のｶｯﾄ長
        <VBFixedArray(1)> Dim CP18() As Short               ' ｲﾝﾃﾞｯｸｽ数
        <VBFixedArray(1)> Dim CP19() As Short               ' 測定ﾓｰﾄﾞ
        <VBFixedArray(1)> Dim CP19K() As Double             ' 測定ﾓｰﾄﾞ(KOA匠の里) 
        <VBFixedArray(1)> Dim CP60() As Double              ' ﾋﾟｯﾁ
        <VBFixedArray(1)> Dim CP61() As Short               ' ｽﾃｯﾌﾟ方向
        <VBFixedArray(1)> Dim CP62() As Short               ' 本数
        <VBFixedArray(1)> Dim CP70() As Double              ' Uｶｯﾄ用(現在ﾀﾞﾐｰ)
        <VBFixedArray(1)> Dim fAveDataRate() As Double              ' Uｶｯﾄ用(現在ﾀﾞﾐｰ)
        <VBFixedArray(1)> Dim CP72() As Double              ' 倍率 
        <VBFixedArray(1)> Dim CP73() As Short               ' 文字列   
        <VBFixedArray(1)> Dim CP74() As Double              ' ｴｯｼﾞｾﾝｽ後の変化率
        <VBFixedArray(1)> Dim CP75() As Short               ' ｴｯｼﾞｾﾝｽ後の確認回数
        <VBFixedArray(1)> Dim CP76() As Short               ' ﾗﾀﾞｰ間距離

        ' 構造体の初期化
        Public Sub Initialize()
            ReDim wCutNo(1)
            ReDim wDelayTime(1)
            ReDim wCutType(1)
            ReDim CP99(1)
            ReDim CP4(1)
            ReDim CP5(1)
            ReDim CP6(1)
            ReDim fCutOff(1)
            ReDim CP7_2(1)
            ReDim CP7_1(1)
            ReDim CP50(1)
            ReDim CP51(1)
            ReDim CP52(1)
            ReDim CP8(1)
            ReDim CP9(1)
            ReDim CP11(1)
            ReDim CP14(1)
            ReDim CP15(1)
            ReDim CP56(1)
            ReDim CP57(1)
            ReDim CP33(1)
            ReDim CP36(1)
            ReDim CP37(1)
            ReDim CP53(1)
            ReDim CP54(1)
            ReDim CP38(1)
            ReDim CP39(1)
            ReDim CP40(1)
            ReDim CP18(1)
            ReDim CP19(1)
            ReDim CP19K(1)
            ReDim CP60(1)
            ReDim CP61(1)
            ReDim CP62(1)
            ReDim CP70(1)
            ReDim fAveDataRate(1)
            ReDim CP72(1)
            ReDim CP73(1)
            ReDim CP74(1)
            ReDim CP75(1)
            ReDim CP76(1)
        End Sub
    End Structure
#End Region

#Region "グローバル変数の定義"
    '-------------------------------------------------------------------------------
    '   トリミングデータ構造体変数定義
    '-------------------------------------------------------------------------------
    '----- ﾌﾟﾚｰﾄﾃﾞｰﾀ -----
    'V5.0.0.8①    Public typPlateInfo As PlateInfo                        ' ﾌﾟﾚｰﾄﾃﾞｰﾀ

    '----- GPIBデータ(汎用) -----                           ' ###229
    'V5.0.0.8①    Public typGpibInfo As GpibInfo                          ' GPIBデータ(汎用)

    '----- ｽﾃｯﾌﾟﾃﾞｰﾀ(CHIP用) -----
    'V5.0.0.8①    Public Const MaxCntStep As Short = 256                  ' ｽﾃｯﾌﾟ最大件数
    Public Const MAXCNT_PLT_BLK As Short = 1024
    'V5.0.0.8①    Public MaxStep As Short                                 ' 実際のｽﾃｯﾌﾟ件数
    'V5.0.0.8①    Public typStepInfoArray(MaxCntStep) As StepInfo         ' ｽﾃｯﾌﾟﾃﾞｰﾀ

    '----- ｸﾞﾙｰﾌﾟﾃﾞｰﾀ -----
    'V5.0.0.8①    Public MaxGrp As Short                                  ' 実際のｸﾞﾙｰﾌﾟ件数
    'V5.0.0.8①    Public typGrpInfoArray(MaxCntStep) As GrpInfo           ' ｸﾞﾙｰﾌﾟﾃﾞｰﾀ
    Public gBlkStagePosX(MAXCNT_PLT_BLK) As Double          ' ブロックのプレート内のステージX位置座標データ
    Public gBlkStagePosY(MAXCNT_PLT_BLK) As Double          ' ブロックのプレート内のステージY位置座標データ
    Public gPltStagePosX(MAXCNT_PLT_BLK) As Double          ' プレートのステージX位置座標データ
    Public gPltStagePosY(MAXCNT_PLT_BLK) As Double          ' プレートのステージY位置座標データ

    '----- サーキットデータ(TKY用) -----
    'V5.0.0.8①    Public Const MaxCntCircuit As Short = 256                   ' ｻｰｷｯﾄ最大件数
    'V5.0.0.8①    Public typCircuitInfoArray(MaxCntCircuit) As CircuitInfo    ' サーキットデータ

    '----- サーキットデータ(NET用) -----
    'V5.0.0.8①    Public typCirAxisInfoArray(MaxCntCircuit) As CirAxisInfo    ' ｻｰｷｯﾄ座標
    'V5.0.0.8①    Public typCirInInfoArray(MaxCntCircuit) As CirInInfo        ' ｻｰｷｯﾄ間ｲﾝﾀｰﾊﾞﾙ

    '----- 抵抗ﾃﾞｰﾀ -----
    'V5.0.0.8①    Public Const MaxCntResist As Short = 512                   ' 抵抗最大件数
    'V5.0.0.8①    Public typResistorInfoArray(MaxCntResist) As ResistorInfo   ' 抵抗ﾃﾞｰﾀ
    'V5.0.0.8①    Public markResistorInfoArray(MaxCntResist) As ResistorInfo  ' 抵抗ﾃﾞｰﾀ(NGﾏｰｷﾝｸﾞ用)

    ''----- ｶｯﾄﾃﾞｰﾀ -----
    'Public typCutInfoArray(MaxCntResist) As CutInfo         ' ｶｯﾄﾃﾞｰﾀ
    'Public markCutInfoArray(MaxCntResist) As CutInfo        ' ｶｯﾄﾃﾞｰﾀ(NGﾏｰｷﾝｸﾞ用)

    '----- Ty2ﾃﾞｰﾀ -----
    'V5.0.0.8①    Public Const MaxCntTy2 As Short = 256                   ' Ty2ﾌﾞﾛｯｸ最大件数
    'V5.0.0.8①    Public MaxTy2 As Short                                  ' 実際のTy2ﾌﾞﾛｯｸ件数
    'V5.0.0.8①    Public typTy2InfoArray(MaxCntTy2) As Ty2Info            ' Ty2データ

    '----- トリミング結果データ -----
    '    Public Const MAX_RESULT_NUM As Short = 21               ' トリミング結果の最大番号 'V2.0.0.0⑬
    Public Const MAX_RESULT_NUM As Short = 22               ' トリミング結果の最大番号 'V2.0.0.0⑬
    Public gwTrimResult(MaxCntResist) As UShort             ' OK/NG結果(0:未実施, 1:OK, 2:ITNG, 3:FTNG, 4:SKIP, ... 13:PATTERN NG)
    Public gfInitialTest(MaxCntResist) As Double            ' IT 抵抗値
    Public gfFinalTest(MaxCntResist) As Double              ' FT 抵抗値
    Public gfTargetVal(MaxCntResist) As Double              ' レシオ目標値
    Public iNgHiCount(MaxCntResist) As Integer              ' 連続NG-HIｶｳﾝﾀ ###129

    '----- 異形面付けデータ -----
    'V5.0.0.8①    Public typIKEIInfo As IKEIInfo                          ' 異形面付けデータ

    ''-------------------------------------------------------------------------------
    ''   トリミングデータの最小値/最大値チェック用構造体変数定義
    ''-------------------------------------------------------------------------------
    Public typSPInputArea As SPInputArea                    ' STEP (0)Min (1)Max
    Public typGPInputArea As GPInputArea                    ' GROUP(0)Min (1)Max 
    Public typTy2InputArea As Ty2InputArea                  ' Ty2  (0)Min (1)Max
    Public typRPInputArea As RPInputArea                    ' RES. (0)Min (1)Max
    Public typCPInputArea As CPInputArea                    ' CUT  (0)Min (1)Max

#End Region

#Region "ﾄﾘﾐﾝｸﾞﾊﾟﾗﾒｰﾀの初期化"
    '''=========================================================================
    '''<summary>ﾄﾘﾐﾝｸﾞﾊﾟﾗﾒｰﾀの初期化</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub Init_AllTrmmingData()

        ' ﾄﾘﾐﾝｸﾞﾊﾟﾗﾒｰﾀの初期化
        DataManager.Init_AllTrmmingData()               'V5.0.0.8①
        'V5.0.0.8①        Call Init_typCircuitInfo()                              ' サーキット
        'V5.0.0.8①        Call Init_typPlateInfo()                                ' プレートデータ
        'V5.0.0.8①        Call Init_typGpibInfo()                                 ' BPIBデータ ' ###229
        'V5.0.0.8①        Call Init_typStepInfoArray()                            ' ステップデータ
        Call Init_typGrpInfoArray()                             ' グループデータ
        'V5.0.0.8①        Call Init_typResistorInfoArray(typResistorInfoArray)    ' 抵抗データ/カットデータ
        'V5.0.0.8①        Call Init_typResistorInfoArray(markResistorInfoArray)   ' 抵抗データ/カットデータ(NGﾏｰｷﾝｸﾞ用)
        'V5.0.0.8①        Call Init_typTy2InfoArray()                             ' Ty2データ
        'V5.0.0.8①        Call Init_typIKEIInfo()                                 ' 異形面付けデータ

    End Sub
#End Region

#Region "LOAD・SAVEの共通化によりTrimDataEditorで定義"
#If False Then 'V5.0.0.8①
#Region "ﾌﾟﾚｰﾄﾃﾞｰﾀ構造体の初期化"
    '''=========================================================================
    '''<summary>ﾌﾟﾚｰﾄﾃﾞｰﾀ構造体の初期化</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub Init_typPlateInfo()

        With typPlateInfo
            '<PLATE DATA 1>
            .strDataName = "(DATA NAME)"                    ' ﾃﾞｰﾀ名
            .intMeasType = 0                                ' 測定種別
            .intDirStepRepeat = 0                           ' ｽﾃｯﾌﾟ＆ﾘﾋﾟｰﾄ
            .intPlateCntXDir = 1                            ' プレート数X 
            .intPlateCntYDir = 1                            ' プレート数Y
            .intBlockCntXDir = 1                            ' ﾌﾞﾛｯｸ数Ｘ
            .intBlockCntYDir = 1                            ' ﾌﾞﾛｯｸ数Ｙ
            .dblPlateItvXDir = 0.0#                         ' プレート間隔Ｘ
            .dblPlateItvYDir = 0.0#                         ' プレート間隔Ｙ 
            .dblBlockSizeXDir = 0.0#                        ' ブロックサイズＸ   
            .dblBlockSizeYDir = 0.0#                        ' ブロックサイズＹ   
            .dblTableOffsetXDir = 0.0#                      ' ﾃｰﾌﾞﾙ位置ｵﾌｾｯﾄX
            .dblTableOffsetYDir = 0.0#                      ' ﾃｰﾌﾞﾙ位置ｵﾌｾｯﾄY
            .dblBpOffSetXDir = 0.0#                         ' ﾋﾞｰﾑ位置ｵﾌｾｯﾄX
            .dblBpOffSetYDir = 0.0#                         ' ﾋﾞｰﾑ位置ｵﾌｾｯﾄY
            .dblAdjOffSetXDir = 0.0#                        ' ｱｼﾞｬｽﾄ位置ｵﾌｾｯﾄX
            .dblAdjOffSetYDir = 0.0#                        ' ｱｼﾞｬｽﾄ位置ｵﾌｾｯﾄY
            .intCurcuitCnt = 0                              ' サーキット数  
            .intNGMark = 0                                  ' NGﾏｰｷﾝｸﾞ
            .intDelayTrim = 0                               ' ﾃﾞｨﾚｲﾄﾘﾑ
            .intNgJudgeUnit = 0                             ' NG判定単位
            .intNgJudgeLevel = 0                            ' NG判定基準
            'V4.0.0.0-38                ↓↓↓↓
            If (gKeiTyp = MACHINE_KD_RS) Then
                .dblZOffSet = 1.0#                          ' ﾌﾟﾛｰﾌﾞZｵﾌｾｯﾄ
                .dblZStepUpDist = 1.0#                      ' ﾌﾟﾛｰﾌﾞｽﾃｯﾌﾟ上昇距離
                .dblZWaitOffset = 0.0#                      ' ﾌﾟﾛｰﾌﾞ待機Zｵﾌｾｯﾄ
            Else
                .dblZOffSet = 5.0#                          ' ﾌﾟﾛｰﾌﾞZｵﾌｾｯﾄ
                .dblZStepUpDist = 3.0#                      ' ﾌﾟﾛｰﾌﾞｽﾃｯﾌﾟ上昇距離
                .dblZWaitOffset = 1.0#                      ' ﾌﾟﾛｰﾌﾞ待機Zｵﾌｾｯﾄ
            End If
            'V4.0.0.0-38                ↑↑↑↑
            .dblLwPrbStpDwDist = 0.0                        ' 下方ﾌﾟﾛｰﾌﾞｽﾃｯﾌﾟ下降距離 V1.13.0.0②
            .dblLwPrbStpUpDist = 5.0                        ' 下方ﾌﾟﾛｰﾌﾞｽﾃｯﾌﾟ上昇距離 V1.13.0.0②
            .intPrbRetryCount = 0                           ' プローブリトライ回数(0=リトライなし) V4.0.0.0④
            .intFinalJudge = 0                              ' ファイナル判定(option)  
            .intAttLevel = 0                                ' アッテネータ(%)(option)    
            '----- V1.23.0.0⑦↓ -----
            ' プレート１タブのプローブチェック項目(オプション)
            .intPrbChkPlt = 0                               '  枚数
            .intPrbChkBlk = 1                               ' ブロック
            .dblPrbTestLimit = 0.0                          '  誤差±%
            '----- V1.23.0.0⑦↑ -----

            '<PLATE DATA 3>
            .intResistDir = 0                               ' 抵抗並び方向
            .intCircuitCntInBlock = 1                       ' 1ﾌﾞﾛｯｸ内ｻｰｷｯﾄ数   
            .dblCircuitSizeXDir = 0.0001                    ' ｻｰｷｯﾄｻｲｽﾞX    
            .dblCircuitSizeYDir = 0.0001                    ' ｻｰｷｯﾄｻｲｽﾞY  
            .intResistCntInBlock = 1                        ' 1ブロック内抵抗数
            .intResistCntInGroup = 1                        ' 1ｸﾞﾙｰﾌﾟ内抵抗数
            .intGroupCntInBlockXBp = 1                      ' ﾌﾞﾛｯｸ内ｸﾞﾙｰﾌﾟ数BP(X方向）(NET時はサーキット数)
            .intGroupCntInBlockYStage = 1                   ' ﾌﾞﾛｯｸ内ｸﾞﾙｰﾌﾟ数Stage(Y方向）
            .dblGroupItvXDir = 0.0#                         ' ｸﾞﾙｰﾌﾟ間隔X
            .dblGroupItvYDir = 0.0#                         ' ｸﾞﾙｰﾌﾟ間隔Y
            .dblChipSizeXDir = 0.0#                         ' ﾁｯﾌﾟｻｲｽﾞX
            .dblChipSizeYDir = 0.0#                         ' ﾁｯﾌﾟｻｲｽﾞY
            .dblStepOffsetXDir = 0.0#                       ' ｽﾃｯﾌﾟｵﾌｾｯﾄ量X
            .dblStepOffsetYDir = 0.0#                       ' ｽﾃｯﾌﾟｵﾌｾｯﾄ量Y
            .dblBlockSizeReviseXDir = 0.0#                  ' ﾌﾞﾛｯｸｻｲｽﾞ補正X
            .dblBlockSizeReviseYDir = 0.0#                  ' ﾌﾞﾛｯｸｻｲｽﾞ補正Y
            .dblBlockItvXDir = 0.0#                         ' ﾌﾞﾛｯｸ間隔X
            .dblBlockItvYDir = 0.0#                         ' ﾌﾞﾛｯｸ間隔Y
            .intContHiNgBlockCnt = 0                        ' 連続NG-HIGH抵抗ﾌﾞﾛｯｸ数

            '(2010/11/09)
            '   一時的にデータ追加-ブロックの考え方を新規に合わせるため、
            '   BPグループ間隔とステージグループ間隔の変数を追加
            .dblBpGrpItv = 0.0#                             ' BPグループ間隔（以前のCHIPのグループ間隔）
            .dblStgGrpItvX = 0.0#                           ' X方向ステージグループ間隔（以前のＣＨＩＰのステップ間インターバル）
            .dblStgGrpItvY = 0.0#                           ' Y方向ステージグループ間隔（以前のＣＨＩＰのステップ間インターバル）
            .dblPlateSizeX = 0.0#                           ' プレートサイズX
            .dblPlateSizeY = 0.0#                           ' プレートサイズY
            .intBpGrpCntInBlk = 0                           ' ブロック内BPグループ数
            .intBlkCntInStgGrpX = 1                         ' X方向ステージグループ内ブロック数
            .intBlkCntInStgGrpY = 1                         ' Y方向ステージグループ内ブロック数
            .intStgGrpCntInPlt = 0                          ' プレート内ステージグループ数→不要かも
            .intChipStepCnt = 0                             ' グループ内のチップステップ数。
            .dblChipStepItv = 0                             ' グループ内チップステップ時のステップ間隔（基本はチップサイズ）

            '<PLATE DATA 2>
            .intReviseMode = 0                              ' 補正ﾓｰﾄﾞ
            .intManualReviseType = 0                        ' 補正方法
            .dblReviseCordnt1XDir = 0.0#                    ' 補正位置座標1X
            .dblReviseCordnt1YDir = 0.0#                    ' 補正位置座標1Y
            .dblReviseCordnt2XDir = 0.0#                    ' 補正位置座標2X
            .dblReviseCordnt2YDir = 0.0#                    ' 補正位置座標2Y
            .dblReviseOffsetXDir = 0.0#                     ' 補正ﾎﾟｼﾞｼｮﾝｵﾌｾｯﾄX
            .dblReviseOffsetYDir = 0.0#                     ' 補正ﾎﾟｼﾞｼｮﾝｵﾌｾｯﾄY
            .intRecogDispMode = 0                           ' 認識ﾃﾞｰﾀ表示ﾓｰﾄﾞ
            If gSysPrm.stDEV.giEXCAM = 1 Then
                .dblPixelValXDir = gSysPrm.stGRV.gfEXCAM_PixelX ' ﾋﾟｸｾﾙ値X
                .dblPixelValYDir = gSysPrm.stGRV.gfEXCAM_PixelY ' ﾋﾟｸｾﾙ値Y
            Else
                .dblPixelValXDir = gSysPrm.stGRV.gfPixelX   ' ﾋﾟｸｾﾙ値X
                .dblPixelValYDir = gSysPrm.stGRV.gfPixelY   ' ﾋﾟｸｾﾙ値Y
            End If
            .intRevisePtnNo1 = 1                            ' 補正位置ﾊﾟﾀｰﾝNo1
            .intRevisePtnNo2 = 2                            ' 補正位置ﾊﾟﾀｰﾝNo2
            .intRevisePtnNo1GroupNo = 2                     ' 補正位置ﾊﾟﾀｰﾝNo1グループNo
            .intRevisePtnNo2GroupNo = 2                     ' 補正位置ﾊﾟﾀｰﾝNo2グループNo
            .dblRotateXDir = 0.0#                           ' X方向の回転中心
            .dblRotateYDir = 0.0#                           ' Y方向の回転中心
            .dblThetaAxis = 0.0#                            ' θ回転角度
            'V5.0.0.9①                              ↓ クランプレス・ラフアライメント用
            .intReviseExecRgh = 0S                          ' 補正有無(0:補正なし, 1:補正あり)
            .dblReviseCordnt1XDirRgh = 0.0                  ' 補正位置座標1X
            .dblReviseCordnt1YDirRgh = 0.0                  ' 補正位置座標1Y
            .dblReviseCordnt2XDirRgh = 0.0                  ' 補正位置座標2X
            .dblReviseCordnt2YDirRgh = 0.0                  ' 補正位置座標2Y
            .dblReviseOffsetXDirRgh = 0.0                   ' 補正ﾎﾟｼﾞｼｮﾝｵﾌｾｯﾄX
            .dblReviseOffsetYDirRgh = 0.0                   ' 補正ﾎﾟｼﾞｼｮﾝｵﾌｾｯﾄY
            .intRecogDispModeRgh = 0S                       ' 認識ﾃﾞｰﾀ表示ﾓｰﾄﾞ(0:表示なし, 1:表示する)
            .intRevisePtnNo1Rgh = 3S                        ' 補正位置ﾊﾟﾀｰﾝNo1
            .intRevisePtnNo2Rgh = 4S                        ' 補正位置ﾊﾟﾀｰﾝNo2
            .intRevisePtnNo1GroupNoRgh = 2S                 ' 補正位置ﾊﾟﾀｰﾝNo1グループNo
            .intRevisePtnNo2GroupNoRgh = 2S                 ' 補正位置ﾊﾟﾀｰﾝNo2グループNo
            'V5.0.0.9①                              ↑

            '<PLATE DATA 4>
            .dblCaribBaseCordnt1XDir = 0.0#                 ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標1X
            .dblCaribBaseCordnt1YDir = 0.0#                 ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標1Y
            .dblCaribBaseCordnt2XDir = 0.0#                 ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標2X
            .dblCaribBaseCordnt2YDir = 0.0#                 ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標2Y
            .dblCaribTableOffsetXDir = 0.0#                 ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄX
            .dblCaribTableOffsetYDir = 0.0#                 ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄY
            .intCaribPtnNo1 = 1                             ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾊﾟﾀｰﾝ登録No1
            .intCaribPtnNo2 = 2                             ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾊﾟﾀｰﾝ登録No2
            .intCaribPtnNo1GroupNo = 3                      ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾊﾟﾀｰﾝ登録No1グループNo
            .intCaribPtnNo2GroupNo = 3                      ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾊﾟﾀｰﾝ登録No2グループNo
            .dblCaribCutLength = 0.0#                       ' ｷｬﾘﾌﾞﾚｰｼｮﾝｶｯﾄ長
            .dblCaribCutSpeed = 0.1                         ' ｷｬﾘﾌﾞﾚｰｼｮﾝｶｯﾄ速度
            .dblCaribCutQRate = 0.1                         ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾚｰｻﾞQﾚｰﾄ
            .dblCutPosiReviseOffsetXDir = 0.0#              ' ｶｯﾄ点補正ﾃｰﾌﾞﾙｵﾌｾｯﾄX
            .dblCutPosiReviseOffsetYDir = 0.0#              ' ｶｯﾄ点補正ﾃｰﾌﾞﾙｵﾌｾｯﾄY
            .intCutPosiRevisePtnNo = 1                      ' ｶｯﾄ点補正ﾊﾟﾀｰﾝ登録No
            .dblCutPosiReviseCutLength = 0.0#               ' ｶｯﾄ点補正ｶｯﾄ長
            .dblCutPosiReviseCutSpeed = 0.1                 ' ｶｯﾄ点補正ｶｯﾄ速度
            .dblCutPosiReviseCutQRate = 0.1                 ' ｶｯﾄ点補正ﾚｰｻﾞQﾚｰﾄ
            .intCutPosiReviseGroupNo = 4                    ' ｸﾞﾙｰﾌﾟNo

            '<PLATE DATA 5>
            .intMaxTrimNgCount = 0                          ' ﾄﾘﾐﾝｸﾞNGｶｳﾝﾀ(上限)
            .intMaxBreakDischargeCount = 0                  ' 割れ欠け排出ｶｳﾝﾀ(上限)
            .intTrimNgCount = 0                             ' 連続ﾄﾘﾐﾝｸﾞNG枚数
            .intContHiNgResCnt = 0                          ' 連続ﾄﾘﾐﾝｸﾞNG抵抗数    ###230
            .intNgJudgeStop = 0                             ' NG判定時停止  V1.13.0.0②
            .intRetryProbeCount = 0                         ' 再ﾌﾟﾛｰﾋﾞﾝｸﾞ回数
            .dblRetryProbeDistance = 0.0#                   ' 再ﾌﾟﾛｰﾋﾞﾝｸﾞ移動量
            .intPowerAdjustMode = 0                         ' ﾊﾟﾜｰ調整ﾓｰﾄﾞ
            .intPwrChkPltNum = 0                            ' チェック基板枚数 V4.11.0.0①
            .intPwrChkTime = 0                              ' チェック時間(分) V4.11.0.0①
            .dblPowerAdjustTarget = 0.1                     ' 調整目標ﾊﾟﾜｰ
            .dblPowerAdjustQRate = 0.1                      ' ﾊﾟﾜｰ調整Qﾚｰﾄ
            .dblPowerAdjustToleLevel = 0.01                 ' ﾊﾟﾜｰ調整許容範囲
            .intPowerAdjustCondNo = 0                       ' ﾊﾟﾜｰ調整加工条件番号(FL用)　
            .intInitialOkTestDo = 0                         ' ｲﾆｼｬﾙOKﾃｽﾄ
            .intWorkSetByLoader = 1                         ' 基板品種
            .intOpenCheck = 0                               ' 4端子ｵｰﾌﾟﾝﾁｪｯｸ
            .intLedCtrl = 0                                 ' LED制御
            .dblThetaAxis = 0.0#                            ' θ軸
            .intGpibCtrl = 0                                ' GP-IB制御
            .intGpibDefDelimiter = 0                        ' 初期設定(ﾃﾞﾘﾐﾀ)
            .intGpibDefTimiout = 0                          ' 初期設定(ﾀｲﾑｱｳﾄ)
            .intGpibDefAdder = 0                            ' 初期設定(機器ｱﾄﾞﾚｽ)
            .strGpibInitCmnd1 = ""                          ' 初期化ｺﾏﾝﾄﾞ1
            .strGpibInitCmnd2 = ""                          ' 初期化ｺﾏﾝﾄﾞ2
            .strGpibTriggerCmnd = ""                        ' ﾄﾘｶﾞｺﾏﾝﾄﾞ
            .intGpibMeasSpeed = 0                           ' 測定速度
            .intGpibMeasMode = 0                            ' 測定モード

            .dblTThetaOffset = 0.0#                         ' Ｔθオフセット
            .dblTThetaBase1XDir = 0.0#                      ' Ｔθ基準位置１X（mm）
            .dblTThetaBase1YDir = 0.0#                      ' Ｔθ基準位置１Y（mm）
            .dblTThetaBase2XDir = 0.0#                      ' Ｔθ基準位置２X（mm）
            .dblTThetaBase2YDir = 0.0#                      ' Ｔθ基準位置２Y（mm）

            '<PLATE DATA 6> (ﾌﾟﾚｰﾄﾀﾌﾞ3(V1.13.0.0②))
            .intContExpMode = 0                             ' 伸縮補正 (0:なし, 1:あり)
            .intContExpGrpNo = 5                            ' 伸縮補正ｸﾞﾙｰﾌﾟ番号
            .intContExpPtnNo = 1                            ' 伸縮補正ﾊﾟﾀｰﾝ番号
            .dblContExpPosX = 0.0                           ' 伸縮補正位置X (mm)
            .dblContExpPosY = 0.0                           ' 伸縮補正位置XY (mm)            
            .intStepMeasCnt = 0                             ' ｽﾃｯﾌﾟ測定回数
            .dblStepMeasPitch = 0.0                         ' ｽﾃｯﾌﾟ測定ﾋﾟｯﾁ
            .intStepMeasReptCnt = 0                         ' ｽﾃｯﾌﾟ測定繰り返しｽﾃｯﾌﾟ回数
            .dblStepMeasReptPitch = 0.0                     ' ｽﾃｯﾌﾟ測定繰り返しｽﾃｯﾌﾟﾋﾟｯﾁ
            .intStepMeasLwGrpNo = 6                         ' ｽﾃｯﾌﾟ測定下方ﾌﾟﾛｰﾌﾞｸﾞﾙｰﾌﾟ番号
            .intStepMeasLwPtnNo = 1                         ' ｽﾃｯﾌﾟ測定下方ﾌﾟﾛｰﾌﾞﾊﾟﾀｰﾝ番号
            .dblStepMeasBpPosX = 0.0                        ' ｽﾃｯﾌﾟ測定BP位置X
            .dblStepMeasBpPosY = 0.0                        ' ｽﾃｯﾌﾟ測定BP位置Y
            .intStepMeasUpGrpNo = 6                         ' ｽﾃｯﾌﾟ測定基板上ﾌﾟﾛｰﾌﾞｸﾞﾙｰﾌﾟ番号
            .intStepMeasUpPtnNo = 2                         ' ｽﾃｯﾌﾟ測定基板上ﾌﾟﾛｰﾌﾞﾊﾟﾀｰﾝ番号
            .dblStepMeasTblOstX = 0.0                       ' ｽﾃｯﾌﾟ測定基板上ﾌﾟﾛｰﾌﾞﾃｰﾌﾞﾙｵﾌｾｯﾄX
            .dblStepMeasTblOstY = 0.0                       ' ｽﾃｯﾌﾟ測定基板上ﾌﾟﾛｰﾌﾞﾃｰﾌﾞﾙｵﾌｾｯﾄY
            .intIDReaderUse = 0                             ' IDﾘｰﾄﾞ (0:未使用, 1:使用)
            .dblIDReadPos1X = 60.0                          ' IDﾘｰﾄﾞ読み取りﾎﾟｼﾞｼｮﾝ 1X
            .dblIDReadPos1Y = 110.0                         ' IDﾘｰﾄﾞ読み取りﾎﾟｼﾞｼｮﾝ 1Y
            .dblIDReadPos2X = 60.0                          ' IDﾘｰﾄﾞ読み取りﾎﾟｼﾞｼｮﾝ 2X
            .dblIDReadPos2Y = 110.0                         ' IDﾘｰﾄﾞ読み取りﾎﾟｼﾞｼｮﾝ 2Y
            .dblReprobeVar = 0.0                            ' 再ﾌﾟﾛｰﾋﾞﾝｸﾞばらつき量
            .dblReprobePitch = 0.0                          ' 再ﾌﾟﾛｰﾋﾞﾝｸﾞﾋﾟｯﾁ

            'V4.10.0.0④            ↓
            .dblPrbCleanStagePitchX = 0.0                   ' ステージ動作ピッチX
            .dblPrbCleanStagePitchY = 0.0                   ' ステージ動作ピッチY
            .intPrbCleanStageCountX = 0                     ' ステージ動作回数X
            .intPrbCleanStageCountY = 0                     ' ステージ動作回数Y
            'V4.10.0.0④            ↑
            'V4.10.0.0⑨↓
            .dblPrbDistance = 0.0                           ' プローブ間距離（mm）
            .dblPrbCleaningOffset = 0.0                     ' クリーニングオフセット(mm)
            'V4.10.0.0⑨↑

            '<OPTION DATA> (オプションタブ'V5.0.0.6①)
            .intControllerInterlock = 0                     ' 外部機器によるインターロックの有無（真田KOA殿の温度コントローラのインターロックに使用）'V5.0.0.6①

            .dblTXChipsizeRelationX = 0.0                   ' 補正位置１と２の相対値Ｘ 'V4.5.1.0⑮
            .dblTXChipsizeRelationY = 0.0                   ' 補正位置１と２の相対値Ｙ 'V4.5.1.0⑮

        End With
    End Sub
#End Region
    '----- ###229↓ -----
#Region "GPIBﾃﾞｰﾀ構造体の初期化"
    '''=========================================================================
    '''<summary>GPIBﾃﾞｰﾀ構造体の初期化</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub Init_typGpibInfo()

        With typGpibInfo
            .wGPIBmode = 0                                              ' GP-IB制御(0:しない 1:する)
            .wDelim = 0                                                 ' ﾃﾞﾘﾐﾀ(0:CR+LF 1:CR 2:LF 3:NONE)
            .wTimeout = 300                                             ' ﾀｲﾑｱｳﾄ(1～32767)(ms単位)
            .wAddress = 0                                               ' 機器ｱﾄﾞﾚｽ(0～30)
            .wEOI = 0                                                   ' EOI(0:使用しない, 1:使用する)
            .wPause1 = 0                                                ' 設定ｺﾏﾝﾄﾞ1送信後ポーズ時間(1～32767msec)
            .wPause2 = 0                                                ' 設定ｺﾏﾝﾄﾞ2送信後ポーズ時間(1～32767msec)
            .wPause3 = 0                                                ' 設定ｺﾏﾝﾄﾞ3送信後ポーズ時間(1～32767msec)
            .wPauseT = 0                                                ' ﾄﾘｶﾞｺﾏﾝﾄﾞ送信後ポーズ時間(1～32767msec)
            .wRev = 0                                                   ' 予備
            .strI = ""                                                  ' 初期化ｺﾏﾝﾄﾞ(MAX40byte)
            .strI2 = ""                                                 ' 初期化ｺﾏﾝﾄﾞ2(MAX40byte)
            .strI3 = ""                                                 ' 初期化ｺﾏﾝﾄﾞ3(MAX40byte)
            .strT = "READ?"                                             ' ﾄﾘｶﾞｺﾏﾝﾄﾞ(50byte)
            .strName = ""                                               ' 機器名(10byte)
            .wReserve = ""                                              ' 予備(8byte)  
        End With
    End Sub
#End Region
    '----- ###229↑ -----
#Region "ｽﾃｯﾌﾟ構造体の初期化"
    '''=========================================================================
    '''<summary>ｽﾃｯﾌﾟ構造体の初期化</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub Init_typStepInfoArray()

        Dim i As Short

        ' ｽﾃｯﾌﾟ構造体数分初期化する
        For i = 1 To MaxCntStep
            With typStepInfoArray(i)
                .intSP1 = 1                                 ' ｽﾃｯﾌﾟ番号
                .intSP2 = 1                                 ' ﾌﾞﾛｯｸ数
                .dblSP3 = 0.0#                              ' ｽﾃｯﾌﾟ間ｲﾝﾀｰﾊﾞﾙ
            End With
        Next i

    End Sub
#End Region
#End If
#End Region 'V5.0.0.8①

#Region "ｸﾞﾙｰﾌﾟ構造体の初期化"
    '''=========================================================================
    '''<summary>ｸﾞﾙｰﾌﾟ構造体の初期化</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub Init_typGrpInfoArray()

        Dim i As Short
#If False Then 'V5.0.0.8① LOAD・SAVEの共通化によりTrimDataEditorで処理
        ' ｸﾞﾙｰﾌﾟ構造体数分初期化する
        For i = 1 To MaxCntStep
            With typGrpInfoArray(i)
                .intGP1 = 1                     ' ｸﾞﾙｰﾌﾟ番号
                .intGP2 = 1                     ' 抵抗数
                .dblGP3 = 0.0#                  ' ｸﾞﾙｰﾌﾟ間ｲﾝﾀｰﾊﾞﾙ
                .dblStgPosX = 0.0#              ' ステージポジションX
                .dblStgPosY = 0.0#              ' ステージポジションY
            End With
        Next i
#End If
        For i = 0 To MAXCNT_PLT_BLK
            gPltStagePosX(i) = 0.0#             ' プレートのステージX位置座標データ
            gPltStagePosY(i) = 0.0#             ' プレートのステージY位置座標データ
            gBlkStagePosX(i) = 0.0#             ' グループのステージX位置座標データ
            gBlkStagePosY(i) = 0.0#             ' グループのステージY位置座標データ
        Next
    End Sub
#End Region

#Region "LOAD・SAVEの共通化によりTrimDataEditorで定義"
#If False Then 'V5.0.0.8①
#Region "サーキット構造体の初期化"
    '''=========================================================================
    '''<summary>サーキット構造体の初期化</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub Init_typCircuitInfo()

        Dim i As Short

        ' 配列の先頭を初期化
        With typCircuitInfoArray(1)
            .intIP1 = 0                                 ' IP番号
            .dblIP2X = 0.0#                             ' マーキングX
            .dblIP2Y = 0.0#                             ' マーキングY
        End With

        ' 配列の残りを初期化
        For i = 2 To UBound(typCircuitInfoArray)
            typCircuitInfoArray(i) = typCircuitInfoArray(1)
        Next i

    End Sub
#End Region

#Region "抵抗ﾃﾞｰﾀ構造体の初期化"
    '''=========================================================================
    '''<summary>抵抗ﾃﾞｰﾀ構造体の初期化</summary>
    '''<param name="stREG">(INP) 抵抗番号</param>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub Init_typResistorInfoArray(ByVal stREG() As ResistorInfo)

        Dim rn As Short

        For rn = 1 To MaxCntResist
            stREG(rn).intResNo = rn                              ' 抵抗番号
            stREG(rn).intResMeasMode = 0                         ' 測定モード(0:抵抗 ,1:電圧 ,2:外部) ※変更
            stREG(rn).intResMeasType = 1                         ' 測定タイプ(0:高速 ,1:高精度)　※追加
            stREG(rn).intCircuitGrp = 1                          ' 所属ｻｰｷｯﾄ
            stREG(rn).intProbHiNo = 1                            ' ﾌﾟﾛｰﾌﾞ番号(HI側)
            stREG(rn).intProbLoNo = 2                            ' ﾌﾟﾛｰﾌﾞ番号(LO側)
            stREG(rn).intProbAGNo1 = 0                           ' ﾌﾟﾛｰﾌﾞ番号(1)
            stREG(rn).intProbAGNo2 = 0                           ' ﾌﾟﾛｰﾌﾞ番号(2)
            stREG(rn).intProbAGNo3 = 0                           ' ﾌﾟﾛｰﾌﾞ番号(3)
            stREG(rn).intProbAGNo4 = 0                           ' ﾌﾟﾛｰﾌﾞ番号(4)
            stREG(rn).intProbAGNo5 = 0                           ' ﾌﾟﾛｰﾌﾞ番号(5)
            stREG(rn).strExternalBits = "00000000"               ' EXTERNAL BITS
            stREG(rn).intPauseTime = 0                           ' ﾎﾟｰｽﾞﾀｲﾑ
            stREG(rn).intTargetValType = 0                       ' 目標値指定
            stREG(rn).intBaseResNo = 0                           ' ﾍﾞｰｽ抵抗番号
            stREG(rn).dblTrimTargetVal = 1.0#                    ' ﾄﾘﾐﾝｸﾞ目標値
            stREG(rn).dblTrimTargetVal_Save = stREG(rn).dblTrimTargetVal    ' V4.11.0.0① ﾄﾘﾐﾝｸﾞ目標値退避用ワーク
            stREG(rn).dblTrimTargetOfs = 0.0                     ' V4.11.0.0① ﾄﾘﾐﾝｸﾞ目標値
            stREG(rn).strRatioTrimTargetVal = ""                 ' トリミング目標値(レシオ計算式) 
            stREG(rn).dblDeltaR = 0.0#                           ' ΔＲ
            stREG(rn).intSlope = 0                               ' 電圧変化ｽﾛｰﾌﾟ
            stREG(rn).dblCutOffRatio = 0.0#                      ' 切り上げ倍率
            stREG(rn).dblProbCfmPoint_Hi_X = 0.0#                ' プローブ確認位置 HI X座標
            stREG(rn).dblProbCfmPoint_Hi_Y = 0.0#                ' プローブ確認位置 HI Y座標
            stREG(rn).dblProbCfmPoint_Lo_X = 0.0#                ' プローブ確認位置 LO X座標
            stREG(rn).dblProbCfmPoint_Lo_Y = 0.0#                ' プローブ確認位置 LO Y座標
            stREG(rn).dblInitTest_HighLimit = 0.0#               ' ｲﾆｼｬﾙﾃｽﾄHIGHﾘﾐｯﾄ
            stREG(rn).dblInitTest_LowLimit = 0.0#                ' ｲﾆｼｬﾙﾃｽﾄLOWﾘﾐｯﾄ
            stREG(rn).dblFinalTest_HighLimit = 0.0#              ' ﾌｧｲﾅﾙﾃｽﾄHIGHﾘﾐｯﾄ
            stREG(rn).dblFinalTest_LowLimit = 0.0#               ' ﾌｧｲﾅﾙﾃｽﾄLOWﾘﾐｯﾄ
            stREG(rn).dblInitOKTest_HighLimit = 0.0#             ' ｲﾆｼｬﾙOKﾃｽﾄHIGHﾘﾐｯﾄ ※未使用
            stREG(rn).dblInitOKTest_LowLimit = 0.0#              ' ｲﾆｼｬﾙOKﾃｽﾄLOWﾘﾐｯﾄ  ※未使用
            stREG(rn).intInitialOkTestDo = 0                     ' 初期ＯＫ判定(0:しない,1:する)※追加(プレートデータから移動)
            stREG(rn).intCutCount = 1                            ' ｶｯﾄ数
            stREG(rn).intCutReviseMode = 0                       ' ｶｯﾄ 補正
            stREG(rn).intCutReviseDispMode = 0                   ' 表示ﾓｰﾄﾞ
            stREG(rn).intCutReviseGrpNo = 1                      ' ﾊﾟﾀｰﾝｸﾞﾙｰﾌﾟ番号  
            stREG(rn).intCutRevisePtnNo = 1                      ' ﾊﾟﾀｰﾝ No.
            stREG(rn).dblCutRevisePosX = 0.0#                    ' ｶｯﾄ補正位置X
            stREG(rn).dblCutRevisePosY = 0.0#                    ' ｶｯﾄ補正位置Y
            stREG(rn).intIsNG = 0                                ' NG有無
            '----- V1.13.0.0②↓ -----
            stREG(rn).intCvMeasNum = 0                          ' CV 最大測定回数
            stREG(rn).intCvMeasTime = 0                         ' CV 最大測定時間(ms) 
            stREG(rn).dblCvValue = 0.0                          ' CV CV値         
            stREG(rn).intOverloadNum = 0                        ' ｵｰﾊﾞｰﾛｰﾄﾞ 回数 
            stREG(rn).dblOverloadMin = 0.0                      ' ｵｰﾊﾞｰﾛｰﾄﾞ 下限値 
            stREG(rn).dblOverloadMax = 0.0                      ' ｵｰﾊﾞｰﾛｰﾄﾞ 上限値
            '----- V1.13.0.0②↑ -----

            stREG(rn).Initialize()                               ' ｶｯﾄ情報
            Call Init_typCutInfoArray(stREG, rn)                 ' ｶｯﾄﾃﾞｰﾀ構造体の初期化
        Next rn

    End Sub
#End Region

#Region "ｶｯﾄﾃﾞｰﾀ構造体の初期化"
    '''=========================================================================
    '''<summary>ｶｯﾄﾃﾞｰﾀ構造体の初期化</summary>
    '''<param name="stREG">(INP)抵抗ﾃﾞｰﾀ構造体配列</param>
    '''<param name="rn">   (INP)抵抗番号(1ORG)</param>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub Init_typCutInfoArray(ByVal stREG() As ResistorInfo, ByVal rn As Integer)

        Dim cn As Short

        ' ｶｯﾄﾃﾞｰﾀ構造体を初期化
        For cn = 1 To MaxCutInfo
            stREG(rn).ArrCut(cn).intCutNo = cn                  ' ｶｯﾄ番号
            stREG(rn).ArrCut(cn).intDelayTime = 0               ' ﾃﾞｨﾚｲﾀｲﾑ
            stREG(rn).ArrCut(cn).strCutType = "A"               ' ｶｯﾄ形状
            stREG(rn).ArrCut(cn).dblTeachPointX = 0.0#          ' ﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄX
            stREG(rn).ArrCut(cn).dblTeachPointY = 0.0#          ' ﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄY
            stREG(rn).ArrCut(cn).dblStartPointX = 0.0#          ' ｽﾀｰﾄﾎﾟｲﾝﾄX
            stREG(rn).ArrCut(cn).dblStartPointY = 0.0#          ' ｽﾀｰﾄﾎﾟｲﾝﾄY
            stREG(rn).ArrCut(cn).dblCutSpeed = 0.1              ' ｶｯﾄｽﾋﾟｰﾄﾞ
            stREG(rn).ArrCut(cn).dblQRate = 0.1                 ' Qｽｲｯﾁﾚｰﾄ
            stREG(rn).ArrCut(cn).dblCutOff = 0.0#               ' ｶｯﾄｵﾌ値
            stREG(rn).ArrCut(cn).dblJudgeLevel = 0.0#           ' 切替ポイント (旧ﾃﾞｰﾀ判定(平均化率))
            stREG(rn).ArrCut(cn).dblCutOffOffset = 0.0#         ' ｶｯﾄｵﾌｵﾌｾｯﾄ
            stREG(rn).ArrCut(cn).intPulseWidthCtrl = 0          ' ﾊﾟﾙｽ幅制御
            stREG(rn).ArrCut(cn).dblPulseWidthTime = 0          ' ﾊﾟﾙｽ幅時間
            stREG(rn).ArrCut(cn).dblLSwPulseWidthTime = 1.0#    ' LSwﾊﾟﾙｽ幅時間
            stREG(rn).ArrCut(cn).intCutDir = 1                  ' ｶｯﾄ方向
            stREG(rn).ArrCut(cn).intLTurnDir = 1                ' Lﾀｰﾝ方向(1:CW, 2:CCW)       V4.0.0.0-67
            stREG(rn).ArrCut(cn).dblMaxCutLength = 0.1#         ' 最大ｶｯﾃｨﾝｸﾞ長
            stREG(rn).ArrCut(cn).dblLTurnPoint = 0.0#           ' Lﾀｰﾝﾎﾟｲﾝﾄ
            stREG(rn).ArrCut(cn).dblMaxCutLengthL = 0.1#        ' Lﾀｰﾝ後の最大ｶｯﾃｨﾝｸﾞ長
            stREG(rn).ArrCut(cn).dblMaxCutLengthHook = 0.1#     ' ﾌｯｸﾀｰﾝ後のｶｯﾃｨﾝｸﾞ長
            stREG(rn).ArrCut(cn).dblR1 = 0.0#                   ' R1
            stREG(rn).ArrCut(cn).dblR2 = 0.0#                   ' R2
            stREG(rn).ArrCut(cn).intCutAngle = 0                ' 斜めｶｯﾄの切り出し角度
            stREG(rn).ArrCut(cn).dblCutSpeed2 = 0.1             ' ｶｯﾄｽﾋﾟｰﾄﾞ2
            stREG(rn).ArrCut(cn).dblQRate2 = 0.1                ' Qｽｲｯﾁﾚｰﾄ2
            '''''2009/08/17
            ''''    ↓436Kのパラメータ。432のINTRTM側では構造体定義ない為、一旦削除
            ''''            stREG(rn).ArrCut(cn).dblCP53 = 0#     'Qｽｲｯﾁﾚｰﾄ3
            ''''            stREG(rn).ArrCut(cn).dblCP54 = 0#     '切替えﾎﾟｲﾝﾄ
            stREG(rn).ArrCut(cn).dblESPoint = 0.0#              ' ｴｯｼﾞｾﾝｽﾎﾟｲﾝﾄ
            stREG(rn).ArrCut(cn).dblESJudgeLevel = 0.0#         ' ｴｯｼﾞｾﾝｽの判定変化率
            stREG(rn).ArrCut(cn).dblMaxCutLengthES = 0.1        ' ｴｯｼﾞｾﾝｽ後のｶｯﾄ長
            stREG(rn).ArrCut(cn).intIndexCnt = 1                ' ｲﾝﾃﾞｯｸｽ数
            stREG(rn).ArrCut(cn).intMeasMode = 0                ' 測定ﾓｰﾄﾞ
            stREG(rn).ArrCut(cn).dblPitch = 0.1#                ' ﾋﾟｯﾁ
            stREG(rn).ArrCut(cn).intStepDir = 1                 ' ｽﾃｯﾌﾟ方向
            stREG(rn).ArrCut(cn).intCutCnt = 1                  ' 本数
            stREG(rn).ArrCut(cn).dblUCutDummy1 = 0.0#           ' Uｶｯﾄ用ﾀﾞﾐｰ
            stREG(rn).ArrCut(cn).dblUCutDummy2 = 0.0#           ' Uｶｯﾄ用ﾀﾞﾐｰ
            stREG(rn).ArrCut(cn).dblESChangeRatio = 0.0#        ' ｴｯｼﾞｾﾝｽ後の変化率
            stREG(rn).ArrCut(cn).intESConfirmCnt = 0            ' ｴｯｼﾞｾﾝｽ後の確認回数
            stREG(rn).ArrCut(cn).intRadderInterval = 0          ' ﾗﾀﾞｰ間距離
            '----- V1.14.0.0①↓ -----
            stREG(rn).ArrCut(cn).intCTcount = 0                 ' ｴｯｼﾞｾﾝｽ後連続NG確認回数※追加(ES用)
            stREG(rn).ArrCut(cn).intJudgeNg = 0                 ' NG判定する/しない(0:TRUE/1:FALSE)※追加(ES用)
            '----- V1.14.0.0①↑ -----
            stREG(rn).ArrCut(cn).strDataName = ""               ' Uカットデータ名※追加   
            stREG(rn).ArrCut(cn).intMoveMode = 0                ' 動作モード(0:通常モード, 2:強制カットモード)　※追加 
            stREG(rn).ArrCut(cn).intDoPosition = 0              ' ポジショニング(0:有, 1:無)　※追加
            stREG(rn).ArrCut(cn).dblReturnPos = 0.0             ' リターンカットのリターン位置 'V1.16.0.0①
            stREG(rn).ArrCut(cn).dblLimitLen = 0.0              ' IXカットのリミット長 'V1.18.0.0④

            '削除する予定
            stREG(rn).ArrCut(cn).dblZoom = 1.0#                 ' 倍率
            stREG(rn).ArrCut(cn).strChar = "TEST"               ' 文字列

            ' FL用に加工条件を追加
            stREG(rn).ArrCut(cn).dblCutSpeed3 = 0.1             ' ｶｯﾄｽﾋﾟｰﾄﾞ3(Lｶｯﾄのﾘﾀｰﾝ/ﾘﾄﾚｰｽ時のLﾀｰﾝ前のｽﾋﾟｰﾄﾞ)
            stREG(rn).ArrCut(cn).dblCutSpeed4 = 0.1             ' ｶｯﾄｽﾋﾟｰﾄﾞ4(Lｶｯﾄのﾘﾀｰﾝ/ﾘﾄﾚｰｽ時のLﾀｰﾝ後のｽﾋﾟｰﾄﾞ)
            stREG(rn).ArrCut(cn).dblCutSpeed5 = 0.1             ' ｶｯﾄｽﾋﾟｰﾄﾞ5(uｶｯﾄのﾘﾀｰﾝ/ﾘﾄﾚｰｽ時のLﾀｰﾝ後のｽﾋﾟｰﾄﾞ)
            stREG(rn).ArrCut(cn).dblCutSpeed6 = 0.1             ' ｶｯﾄｽﾋﾟｰﾄﾞ6(uｶｯﾄのﾘﾀｰﾝ/ﾘﾄﾚｰｽ時のLﾀｰﾝ後のｽﾋﾟｰﾄﾞ)
            stREG(rn).ArrCut(cn).Initialize()                   ' 加工条件番号1～n(0～31)

            'V4.0.0.0-67                ↓↓↓↓
            stREG(rn).ArrCut(cn).dblQRate2 = 0.1                ' Qｽｲｯﾁﾚｰﾄ3(Lｶｯﾄのﾘﾀｰﾝ/ﾘﾄﾚｰｽ時のLﾀｰﾝ前のQｽｲｯﾁﾚｰﾄ)
            stREG(rn).ArrCut(cn).dblQRate3 = 0.1                ' Qｽｲｯﾁﾚｰﾄ4(Lｶｯﾄのﾘﾀｰﾝ/ﾘﾄﾚｰｽ時のLﾀｰﾝ後のQｽｲｯﾁﾚｰﾄ)
            stREG(rn).ArrCut(cn).dblQRate4 = 0.1                ' Qｽｲｯﾁﾚｰﾄ5(Uｶｯﾄのﾘﾀｰﾝ/ﾘﾄﾚｰｽ時のLﾀｰﾝ後のQｽｲｯﾁﾚｰﾄ) 未使用
            stREG(rn).ArrCut(cn).dblQRate5 = 0.1                ' Qｽｲｯﾁﾚｰﾄ6(Uｶｯﾄのﾘﾀｰﾝ/ﾘﾄﾚｰｽ時のLﾀｰﾝ後のQｽｲｯﾁﾚｰﾄ) 未使用
            stREG(rn).ArrCut(cn).dblQRate6 = 0.1
            'V4.0.0.0-67                ↑↑↑↑

            'V4.0.0.0-38                ↓↓↓↓
            ' 調整目標パワーと調整許容範囲に初期値を設定する
            For i As Integer = 0 To (cCNDNUM - 1) Step 1
                stREG(rn).ArrCut(cn).dblPowerAdjustTarget(i) = gwModule.POWERADJUST_TARGET
                stREG(rn).ArrCut(cn).dblPowerAdjustToleLevel(i) = gwModule.POWERADJUST_LEVEL
                stREG(rn).ArrCut(cn).FLCurrent(i) = gwModule.POWERADJUST_CURRENT
                stREG(rn).ArrCut(cn).FLSteg(i) = gwModule.POWERADJUST_STEG
            Next i
            'V4.0.0.0-38                ↑↑↑↑
        Next cn

    End Sub

#End Region

#Region "TY2構造体の初期化"
    '''=========================================================================
    '''<summary>TY2構造体の初期化</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub Init_typTy2InfoArray()

        Dim i As Short

        ' TY2構造体を初期化 ###119
        For i = 1 To MaxCntTy2
            With typTy2InfoArray(i)
                .intTy21 = i                                            ' ﾌﾞﾛｯｸ番号
                .dblTy22 = 0.0#                                         ' ｽﾃｯﾌﾟ距離
            End With
        Next i

    End Sub
#End Region
#End If
#End Region 'V5.0.0.8①

#Region "トリミング結果取得エリアの初期化"
    '''=========================================================================
    '''<summary>トリミング結果取得エリアの初期化する</summary>
    '''=========================================================================
    Public Sub Init_TrimResultData()
        Dim regCnt As Integer

        Try
            For regCnt = 0 To MaxCntResist                        ' MaxCntResist-1
                gwTrimResult(regCnt) = TRIM_RESULT_NOTDO
            Next


        Catch ex As Exception
            Dim strMsg As String
            strMsg = "DataAccess.Init_TrimResultData() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub

#End Region

#Region "LOAD・SAVEの共通化によりTrimDataEditorで定義"
#If False Then 'V5.0.0.8①
#Region "異形面付けデータ構造体の初期化"
    '''=========================================================================
    '''<summary>異形面付けデータ構造体の初期化</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub Init_typIKEIInfo()

        typIKEIInfo.intI1 = 0                               ' 異形面付けの有無（0:無し,1:X方向,2:Y方向
        typIKEIInfo.Initialize()                            ' サーキットの有無（0:無し,1:有り）

    End Sub
#End Region
#End If
#End Region  'V5.0.0.8①

#Region "ｸﾞﾙｰﾌﾟ内ﾁｯﾌﾟ数取得"
    '''=========================================================================
    '''<summary>ｸﾞﾙｰﾌﾟ内ﾁｯﾌﾟ数取得</summary>
    '''<param name="intChipNum">(OUT) ﾁｯﾌﾟ数</param>
    '''<remarks>CHIPとNETで処理が異なるので要注意</remarks>
    '''=========================================================================
    Public Sub GetChipNum(ByRef intChipNum As Short)

        If (gTkyKnd = KND_TKY) Or (gTkyKnd = KND_CHIP) Then
            intChipNum = typPlateInfo.intResistCntInGroup

        ElseIf (gTkyKnd = KND_NET) Then
            Dim CirNum As Integer
            CirNum = typPlateInfo.intCircuitCntInBlock  ' 1ﾌﾞﾛｯｸ内ｻｰｷｯﾄ数
            intChipNum = typPlateInfo.intResistCntInGroup * CirNum
        End If

    End Sub
#End Region

#Region "ﾋﾟｸｾﾙ値X,Y取得"
    '''=========================================================================
    '''<summary>ﾋﾟｸｾﾙ値X,Y取得</summary>
    '''<param name="dblPixel2umX">(OUT) ﾋﾟｸｾﾙ値X</param>
    '''<param name="dblPixel2umY">(OUT) ﾋﾟｸｾﾙ値Y</param>
    '''<remarks>1ﾋﾟｸｾﾙあたりのmmを返す</remarks>
    '''=========================================================================
    Public Sub GetPixel2um(ByRef dblPixel2umX As Double, ByRef dblPixel2umY As Double)

        If gSysPrm.stDEV.giEXCAM = 1 Then               ' 外部カメラ ? 
            dblPixel2umX = gSysPrm.stGRV.gfEXCAM_PixelX ' 1ﾋﾟｸｾﾙあたりのmm X
            dblPixel2umY = gSysPrm.stGRV.gfEXCAM_PixelY ' 1ﾋﾟｸｾﾙあたりのmm Y
        Else
            dblPixel2umX = gSysPrm.stGRV.gfPixelX       ' 1ﾋﾟｸｾﾙあたりのmm X
            dblPixel2umY = gSysPrm.stGRV.gfPixelY       ' 1ﾋﾟｸｾﾙあたりのmm Y
        End If

    End Sub
#End Region

#Region "プレート数を取得する"
    '''=========================================================================
    '''<summary>プレート数を取得する</summary>
    '''=========================================================================
    Public Function GetPlateCnt() As Integer
        Try
            With typPlateInfo
                ' Short * Short だと 256 * 256 でオーバーフローする
                'V5.0.0.9⑯                GetPlateCnt = .intPlateCntXDir * .intPlateCntYDir
                Dim x As Integer = .intPlateCntXDir
                Dim y As Integer = .intPlateCntYDir
                GetPlateCnt = (x * y)

                Exit Function
            End With
        Catch ex As Exception
            Dim strMSG As String
            strMSG = "DataAccess.GetPlateCnt() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Function
#End Region

#Region "プレート内のブロック数を取得する"

    '''=========================================================================
    '''<summary>プレート内のブロック数を取得する</summary>
    '''=========================================================================
    Public Function GetBlockCnt() As Integer
        Try
            With typPlateInfo
                ' Short * Short だと 256 * 256 でオーバーフローする
                'V5.0.0.9⑯                GetBlockCnt = .intBlockCntXDir * .intBlockCntYDir
                Dim x As Integer = .intBlockCntXDir
                Dim y As Integer = .intBlockCntYDir
                GetBlockCnt = (x * y)

                Exit Function
            End With

        Catch ex As Exception
            Dim strMSG As String
            strMSG = "DataAccess.GetBlockCnt() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Function
#End Region

#Region "ブロックサイズを算出する【CHIP/NET用】"
    '''=========================================================================
    '''<summary>ブロックサイズを算出する【CHIP/NET用】</summary>
    '''<param name="dblBSX">(OUT) ブロックサイズX</param>
    '''<param name="dblBSY">(OUT) ブロックサイズY</param>
    '''=========================================================================
    Public Sub CalcBlockSize(ByRef dblBSX As Double, ByRef dblBSY As Double)

        Dim i As Integer
        Dim intChipNum As Integer
        Dim intGNx As Integer
        Dim intGNY As Integer
        Dim dData As Double = 0.0

        Try
            ' CHIP/NET時 
            ' グループ数X,Y
            intGNx = typPlateInfo.intGroupCntInBlockXBp                 ' ＢＰグループ数(サーキット数)
            intGNY = typPlateInfo.intGroupCntInBlockXBp

            ' グループ内抵抗数             
            intChipNum = typPlateInfo.intResistCntInGroup

            ' ブロックサイズX,Yを求める
            If (typPlateInfo.intResistDir = 0) Then                     ' 抵抗(ﾁｯﾌﾟ)並び方向(0:X, 1:Y)
                ' 抵抗(ﾁｯﾌﾟ)並び方向 = X方向の場合
                If (intGNx = 1) Then
                    ' 1グループ(1サーキット)の場合
                    dData = typPlateInfo.dblChipSizeXDir * intChipNum   ' Data = チップサイズX * チップ数

                Else
                    ' 複数グループ(複数サーキット)の場合
                    For i = 1 To intGNx
                        If (i = intGNx) Then                            ' 最終グループ ?
                            ' Data = Data + (チップサイズX * グループ内(サーキット内)抵抗数)
                            dData = dData + (typPlateInfo.dblChipSizeXDir * typPlateInfo.intResistCntInGroup)
                        Else
                            ' Data = Data + (チップサイズX * グループ内(サーキット内)抵抗数 + ＢＰグループ(サーキット)間隔)
                            dData = dData + (typPlateInfo.dblChipSizeXDir * typPlateInfo.intResistCntInGroup + typPlateInfo.dblBpGrpItv)
                        End If
                    Next i
                End If

                ' ブロックサイズX,Yを返す
                dblBSX = dData                                          ' ブロックサイズX = 計算値
                dblBSY = typPlateInfo.dblChipSizeYDir                   ' ブロックサイズY = チップサイズY

            Else
                ' 抵抗(ﾁｯﾌﾟ)並び方向 = Y方向の場合
                If (intGNY = 1) Then
                    ' 1グループ(1サーキット)の場合
                    dData = typPlateInfo.dblChipSizeYDir * intChipNum   ' Data = チップサイズY * チップ数

                Else
                    ' 複数グループ(複数サーキット)の場合
                    For i = 1 To intGNY
                        If (i = intGNY) Then                            ' 最終グループ ?
                            ' Data = Data + (チップサイズY * グループ内(サーキット内)抵抗数)
                            dData = dData + (typPlateInfo.dblChipSizeYDir * typPlateInfo.intResistCntInGroup)
                        Else
                            ' Data = Data + (チップサイズY * グループ内(サーキット内)抵抗数 + ＢＰグループ(サーキット)間隔)
                            dData = dData + (typPlateInfo.dblChipSizeYDir * typPlateInfo.intResistCntInGroup + typPlateInfo.dblBpGrpItv)
                        End If
                    Next i

                End If

                ' ブロックサイズX,Yを返す
                dblBSX = typPlateInfo.dblChipSizeXDir                   ' ブロックサイズX = チップサイズX
                dblBSY = dData                                          ' ブロックサイズY = 計算値

            End If

            ' トラップエラー発生時 
        Catch ex As Exception
            Dim strMSG As String
            strMSG = "DataAccess.CalcBlockSize() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub

#End Region

#Region "ログ表示の向けに、指定ブロックXYからステージグループ番号、ブロック番号を取得する"
    '''=========================================================================
    '''<summary>指定ブロックXYのポジション情報から、</summary>
    ''' <param name="curBlkNoX">(INP)現在ブロックのX位置（行位置）</param>
    ''' <param name="curBlkNoY">(INP)現在ブロックのY位置（列位置）</param>
    ''' <param name="stgGrpNoX">(OUT)ステージグループ番号X</param>
    ''' <param name="stgGrpNoY">(OUT)ステージグループ番号Y</param>
    ''' <param name="blockNoX"> (OUT)ステージグループ番号を加味したブロック番号X</param>
    ''' <param name="blockNoY"> (OUT)ステージグループ番号を加味したブロック番号Y</param>
    '''<returns></returns>
    '''<remarks></remarks>
    '''  プレートの並び　　プレート内部の並び
    '''  ____ ____ ____      ____ ____
    ''' | ⑨ | ⑧ | ① |　　| ⑧ | ① |
    ''' |____|____|____|    |____|____|
    ''' | ⑩ | ⑦ | ② |    | ⑦ | ② |    
    ''' |____|____|____|    |____|____|
    ''' | ⑪ | ⑥ | ③ |    | ⑥ | ③ |
    ''' |____|____|____|    |____|____|
    ''' | ⑫ | ⑤ | ④ |    | ⑤ | ④ |
    ''' |____|____|____|    |____|____|
    ''' 
    '''=========================================================================
    Public Function GetDisplayPosInfo(ByVal curBlkNoX As Integer, ByVal curBlkNoY As Integer, _
                ByRef stgGrpNoX As Integer, ByRef stgGrpNoY As Integer, _
                ByRef blockNoX As Integer, ByRef blockNoY As Integer) As Boolean

        Dim strMSG As String

        GetDisplayPosInfo = True
        Try

            '----- ###165↓ -----
            With typPlateInfo

                If (.intResistDir = 0) Then                             ' 抵抗(ﾁｯﾌﾟ)並び方向 = X方向の場合
                    ' ステージグループ番号X = 1
                    stgGrpNoX = 1
                    ' ステージグループ番号Y = ブロック番号Y / ステージグループ内ブロック数
                    If ((curBlkNoY Mod .intBlkCntInStgGrpY) <> 0) Then  ' 余り有り ? 
                        stgGrpNoY = curBlkNoY \ .intBlkCntInStgGrpY + 1
                    Else
                        stgGrpNoY = curBlkNoY \ .intBlkCntInStgGrpY
                    End If

                    ' ブロック番号X,Y
                    blockNoX = curBlkNoX
                    blockNoY = curBlkNoY

                Else                                                    ' 抵抗(ﾁｯﾌﾟ)並び方向 = Y方向の場合
                    ' ステージグループ番号Y = 1
                    stgGrpNoY = 1
                    ' ステージグループ番号X = ブロック番号X / ステージグループ内ブロック数
                    If ((curBlkNoY Mod .intBlkCntInStgGrpY) <> 0) Then  ' 余り有り ? 
                        stgGrpNoX = curBlkNoX \ .intBlkCntInStgGrpX + 1
                    Else
                        stgGrpNoX = curBlkNoX \ .intBlkCntInStgGrpX
                    End If

                    ' ブロック番号X,Y
                    blockNoX = curBlkNoX
                    blockNoY = curBlkNoY
                End If

            End With

            'With typPlateInfo
            '    'ステージグループ間隔を加味したブロック位置の算出
            '    '   →チップ方向ステップがある場合のステップのカウント方法を別途検討が必要
            '    ' X方向
            '    'If (.intBlkCntInStgGrpX <> 0) Then
            '    If (.dblStgGrpItvX <> 0) Then
            '        '   ステージグループ間隔が設定されている場合。
            '        '   →「現在ブロック/ステージグループ数」がステージグループの番号
            '        '   　「現在ブロック/ステージグループ数の余り」がステージグループ内部のブロック番号
            '        'X方向チップステップがある場合
            '        '   →チップステップのカウントも加算する。
            '        '   　ステージグループは、「現在のブロック/（ステージグループ内ブロック数＊チップステップ数）」
            '        '   　ブロック数は、「現在のブロック/チップステップ数」
            '        '   　チップステップ数は、「(現在のブロック/チップステップ数)の余り」
            '        stgGrpNoX = (curBlkNoX + 1) \ .intBlkCntInStgGrpX
            '        blockNoX = curBlkNoX Mod .intBlkCntInStgGrpX
            '        If (blockNoX = 0) Then
            '            blockNoX = .intBlkCntInStgGrpX
            '        End If
            '    Else
            '        '   ステージグループ間隔が設定されていない場合
            '        '   →現在ブロック=ステージグループ番号
            '        '   　現在ブロック=ブロック番号
            '        stgGrpNoX = curBlkNoX
            '        blockNoX = curBlkNoX
            '    End If

            '    'Y方向
            '    'If (.intBlkCntInStgGrpY <> 0) Then
            '    If (.dblStgGrpItvY <> 0) Then
            '        '   ステージグループ間隔が設定されている場合。
            '        '   →「現在ブロック/ステージグループ数」がステージグループの番号
            '        '   　「現在ブロック/ステージグループ数の余り」がステージグループ内部のブロック番号
            '        stgGrpNoY = (curBlkNoY + 1) \ .intBlkCntInStgGrpY
            '        blockNoY = curBlkNoY Mod .intBlkCntInStgGrpY
            '        If (blockNoY = 0) Then
            '            blockNoY = .intBlkCntInStgGrpY
            '        End If
            '    Else
            '        '   ステージグループ間隔が設定されていない場合
            '        '   →現在ブロック=ステージグループ番号
            '        '   　現在ブロック=ブロック番号
            '        stgGrpNoY = curBlkNoY
            '        blockNoY = curBlkNoY
            '    End If
            'End With
            '----- ###165↑ -----

        Catch ex As Exception
            strMSG = "DataAccess.GetDisplayPosInfo() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            GetDisplayPosInfo = False
        End Try
    End Function
#End Region

    '#Region "指定ブロックのステージ位置を取得-(未使用)"
    '    '''=========================================================================
    '    '''<summary>指定ブロックの座標位置を取得する</summary>
    '    '''<param name="plateNo"> (INP) プレート番号</param>
    '    '''<param name="blockNo">(INP)プレート内のブロック番号</param>
    '    '''<param name="stgx">(OUT)ステージX座標</param>
    '    '''<param name="stgy">(OUT)ステージY座標</param>
    '    '''<returns>最終プレート、最終ブロックの場合TRUEを返す。</returns>
    '    '''<remarks></remarks>
    '    '''  プレートの並び　　プレート内部の並び
    '    '''  ____ ____ ____      ____ ____
    '    ''' | ⑨ | ⑧ | ① |　　| ⑧ | ① |
    '    ''' |____|____|____|    |____|____|
    '    ''' | ⑩ | ⑦ | ② |    | ⑦ | ② |    
    '    ''' |____|____|____|    |____|____|
    '    ''' | ⑪ | ⑥ | ③ |    | ⑥ | ③ |
    '    ''' |____|____|____|    |____|____|
    '    ''' | ⑫ | ⑤ | ④ |    | ⑤ | ④ |
    '    ''' |____|____|____|    |____|____|
    '    ''' 
    '    '''=========================================================================
    '    Public Function GetTargetStagePos(ByVal plateNo As Integer, ByVal blockNo As Integer, ByRef stgx As Double, ByRef stgy As Double) As Boolean
    '        Dim intPlateXCnt As Integer
    '        Dim intPlateYCnt As Integer
    '        Dim intLastPlateNo As Integer
    '        Dim dblWorkBaseStgPosX As Double
    '        Dim dblWorkBaseStgPosY As Double
    '        Dim strMSG As String

    '        GetTargetStagePos = False

    '        Try

    '            'プレートによるベースの位置座標を取得する。
    '            With typPlateInfo
    '                'プレート間隔の計算
    '                intPlateXCnt = plateNo / .intPlateCntXDir
    '                If (intPlateXCnt Mod 2) = 0 Then
    '                    intPlateYCnt = plateNo Mod .intPlateCntYDir
    '                Else
    '                    intPlateYCnt = (.intPlateCntYDir + 1) - (plateNo Mod .intPlateCntYDir)
    '                End If

    '                'プレートのベースポジション
    '                dblWorkBaseStgPosX = (.dblPlateSizeX * intPlateXCnt)
    '                dblWorkBaseStgPosY = (.dblPlateSizeY * intPlateYCnt)

    '                'ブロック座標を取得する
    '                stgx = dblWorkBaseStgPosX + typGrpInfoArray(blockNo).dblStgPosX
    '                stgy = dblWorkBaseStgPosY + typGrpInfoArray(blockNo).dblStgPosY

    '                '最終プレートの判定
    '                intLastPlateNo = .intPlateCntXDir * .intPlateCntYDir
    '                If (plateNo = intLastPlateNo) Then
    '                    GetTargetStagePos = True
    '                End If
    '            End With

    '            ' トラップエラー発生時 
    '        Catch ex As Exception
    '            strMSG = "DataAccess.GetTargetStagePos() TRAP ERROR = " + ex.Message
    '            MsgBox(strMSG)
    '            GetTargetStagePos = 0
    '        End Try

    '    End Function
    '#End Region

#Region "開始位置算出"
    '''=========================================================================
    '''<summary>ブロック/プレートの開始位置算出</summary>
    '''<param name="totalCount"> （IN)ブロックorプレートのトータル数</param>
    '''<param name="size">       （IN)ブロックorプレートのサイズ</param>
    ''' <param name="interval_1">（IN)ブロックorプレート間隔</param> 
    ''' <param name="interval_2">（IN)ステージグループ間隔（プレート算出では未使用。「0」指定）</param>
    ''' <param name="cntInItv2"> （IN)ステージグループ内のブロック数（プレート算出では未使用。「0」指定）</param>
    ''' <param name="interval_3">（IN)チップステップ間隔（プレート算出では未使用。「0」指定）</param>
    ''' <param name="cntInItv3"> （IN)チップステップ時のグループ内部のステップ数（プレート算出では未使用。「0」指定）</param>
    ''' <param name="startPos">  （OUT)結果保存先配列。算出結果を保存するDouble型の配列</param>
    '''<remarks>プレート内部のブロック開始位置を算出する。</remarks>
    '''=========================================================================
    Public Sub CalcStartPos(ByVal totalCount As Integer, _
                            ByVal size As Double, ByVal interval_1 As Double, _
                            ByVal interval_2 As Double, ByVal cntInItv2 As Integer, _
                            ByVal interval_3 As Double, ByVal cntInItv3 As Integer, _
                            ByRef startPos() As Double)
        Dim count As Short
        Dim strMSG As String
        Dim intervalCount As Integer
        Dim chipStpCnt As Integer
        Dim workStagePos As Double

        Try
            ' トータルのブロック数分ループを回す。
            For count = 0 To totalCount - 1
                ' インターバルの数を取得
                If (cntInItv2 <> 0) Then
                    intervalCount = count \ cntInItv2
                Else
                    intervalCount = 0
                End If

                'チップステップ実行時の減算
                If (cntInItv3 <> 0) Then
                    For chipStpCnt = 0 To cntInItv3
                        'ステージの開始位置算出
                        '  'V4.3.0.0⑤
                        If (giStageYOrg = STGY_ORG_UP) Then
                            workStagePos = (size * count) + (interval_1 * count) + (interval_2 * intervalCount) - (interval_3 * chipStpCnt)
                        Else
                            workStagePos = (size * count) + (interval_1 * count) + (interval_2 * intervalCount) + (interval_3 * chipStpCnt)
                        End If
                        '  'V4.3.0.0⑤

                        startPos((count * (cntInItv3 + 1)) + chipStpCnt) = workStagePos
                    Next
                Else
                    'ステージの開始位置算出
                    workStagePos = (size * count) + (interval_1 * count) + (interval_2 * intervalCount)
                    startPos(count) = workStagePos
                End If

            Next

        Catch ex As Exception
            strMSG = "DataAccess.CalcBlockStagePos() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region

#Region "プレートのX方向、Y方向の開始位置算出"
    '''=========================================================================
    '''<summary>プレートのX方向、Y方向の開始位置算出</summary>
    '''<returns>なし</returns>
    '''<remarks>プレート内のX方向の開始位置、Y方向の開始位置を算出する。</remarks>
    '''=========================================================================
    Public Function CalcPlateXYStartPos() As Integer
        Dim strMSG As String

        CalcPlateXYStartPos = 0

        Try
            ' X方向のブロック開始位置を算出
            With typPlateInfo
                ' X方向
                Call CalcStartPos(.intPlateCntXDir, .dblPlateSizeX, .dblPlateItvXDir, _
                                0, 0, 0, 0, gPltStagePosX)

                ' Y方向
                Call CalcStartPos(.intPlateCntYDir, .dblPlateSizeY, .dblPlateItvYDir, _
                                0, 0, 0, 0, gPltStagePosY)
            End With

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "DataAccess.CalcPlateXYStartPos() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            CalcPlateXYStartPos = 0
        End Try

    End Function
#End Region

#Region "プレート内ブロックのX方向、Y方向の開始位置算出"
    '''=========================================================================
    '''<summary>ブロックのX方向、Y方向の開始位置算出</summary>
    '''<returns>なし</returns>
    '''<remarks>プレート内部のX方向のブロック開始位置、Y方向のブロック開始位置を算出する。</remarks>
    '''=========================================================================
    Public Function CalcBlockXYStartPos() As Integer

        Dim strMSG As String
        Dim chipStpItvX As Double
        Dim chipStpCntX As Integer
        Dim chipStpItvY As Double
        Dim chipStpCntY As Integer
        Dim cnt As Integer
        Dim dblIntval As Double                                         ' ###119

        CalcBlockXYStartPos = 0

        Try
            'ステージ座標データ保存領域の初期化
            For cnt = 0 To MAXCNT_PLT_BLK
                gBlkStagePosX(cnt) = 0
                gBlkStagePosY(cnt) = 0
            Next

            ' X方向のブロック開始位置を算出
            With typPlateInfo
                'チップステップ情報の設定）
                If (.intDirStepRepeat = STEP_RPT_CHIPXSTPY) Then        ' ｽﾃｯﾌﾟ＆ﾘﾋﾟｰﾄ = チップ幅+X方向
                    'chipStpItvX = .dblChipStepItv                       ' チップステップ数X = グループ内のチップステップ数
                    'chipStpCntX = .intChipStepCnt                       ' ステップ間隔X     = グループ内チップステップ時のステップ間隔
                    chipStpItvX = .dblChipSizeXDir / .intBlockCntXDir   ' チップステップ数X = グループ内のチップステップ数              '###115　千鳥対応
                    chipStpCntX = .intBlockCntXDir - 1                  ' ステップ間隔X     = グループ内チップステップ時のステップ間隔  '###115　千鳥対応
                    chipStpItvY = 0
                    chipStpCntY = 0
                ElseIf (.intDirStepRepeat = STEP_RPT_CHIPYSTPX) Then    ' ｽﾃｯﾌﾟ＆ﾘﾋﾟｰﾄ = チップ幅+Y方向
                    chipStpItvX = 0
                    chipStpCntX = 0
                    'chipStpItvY = .dblChipStepItv                       ' チップステップ数Y = グループ内のチップステップ数
                    'chipStpCntY = .intChipStepCnt                       ' ステップ間隔Y     = グループ内チップステップ時のステップ間隔
                    chipStpItvY = .dblChipSizeYDir / .intBlockCntYDir   ' チップステップ数Y = グループ内のチップステップ数              '###115　千鳥対応
                    chipStpCntY = .intBlockCntYDir - 1                  ' ステップ間隔Y     = グループ内チップステップ時のステップ間隔  '###115　千鳥対応

                Else
                    chipStpItvX = 0
                    chipStpCntX = 0
                    chipStpItvY = 0
                    chipStpCntY = 0
                End If

                ' X方向(gBlkStagePosX配列にブロックX開始位置を設定)
                Call CalcStartPos(.intBlockCntXDir, .dblBlockSizeXDir, .dblBlockItvXDir, _
                                .dblStgGrpItvX, .intBlkCntInStgGrpX, _
                                chipStpItvX, chipStpCntX, gBlkStagePosX)

                ' Y方向(gBlkStagePosY配列にブロックY開始位置を設定)
                Call CalcStartPos(.intBlockCntYDir, .dblBlockSizeYDir, .dblBlockItvYDir, _
                                .dblStgGrpItvY, .intBlkCntInStgGrpY, _
                                chipStpItvY, chipStpCntY, gBlkStagePosY)

                '-----  ###119↓ -----
                ' TY2のステップ距離(ブロックインターバル)を加算する
                dblIntval = 0.0
                For cnt = 1 To (MaxTy2 - 1)
                    dblIntval = dblIntval + typTy2InfoArray(cnt).dblTy22
                    If (.intResistDir = 0) Then                         ' チップ並び方向 = X方向の場合
                        gBlkStagePosY(cnt) = gBlkStagePosY(cnt) + dblIntval
                    Else                                                ' チップ並び方向 = Y方向の場合
                        gBlkStagePosX(cnt) = gBlkStagePosX(cnt) + dblIntval
                    End If
                Next
                '-----  ###119↑ -----

            End With

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "DataAccess.CalcStagePos() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            CalcBlockXYStartPos = 0
        End Try

    End Function
#End Region

#Region "プレート内部の各ブロックのステージ位置を算出-(未使用）"
    '''=========================================================================
    '''<summary>各ブロックステージ位置算出</summary>
    '''<returns>なし</returns>
    '''<remarks>ブロック毎のステージスタート位置を算出しデータを保存する。</remarks>
    '''プレート内部の保存領域は以下の順番で保存を行う。
    '''  
    '''　プレート内部の並び
    '''   ____ ____
    '''　| ⑥ | ① |
    '''  |____|____|
    '''  | ⑤ | ② |    
    '''  |____|____|
    '''  | ④ | ③ |
    '''  |____|____|
    ''' 
    '''=========================================================================
    Public Function CalcAllBlockStagePos() As Integer

        '#4.12.2.0⑦        Dim count As Short
        Dim count As Integer            '#4.12.2.0⑦
        Dim strMSG As String

        ' ブロック位置の算出係数
        Dim intXBlockCnt As Integer
        Dim intYBlockCnt As Integer
        ' ステージグループ間隔の算出係数
        Dim intYStgGrpCnt As Integer
        Dim intXStgGrpCnt As Integer

        ' プレート間隔の算出係数
        Dim intTotalBlkCntInPlate As Integer
        'Dim intPlateXCnt As Integer
        'Dim intPlateYCnt As Integer

        ' ブロック位置の一時保存領域
        Dim dblWorkBlockPosX As Double
        Dim dblWorkBlockPosY As Double

        CalcAllBlockStagePos = 0

        Try
            ' プレート内のブロック数の算出
            '#4.12.2.0⑦           intTotalBlkCntInPlate = typPlateInfo.intBlockCntXDir * typPlateInfo.intBlockCntYDir
            intTotalBlkCntInPlate = CInt(typPlateInfo.intBlockCntXDir) * CInt(typPlateInfo.intBlockCntYDir) '#4.12.2.0⑦

            For count = 1 To intTotalBlkCntInPlate
                'ブロック位置の算出
                With typPlateInfo
                    '縦方向、横方向ブロック数
                    intXBlockCnt = count \ .intBlockCntYDir
                    If (intXBlockCnt Mod 2) = 0 Then
                        intYBlockCnt = count Mod .intBlockCntYDir
                    Else
                        intYBlockCnt = (.intBlockCntYDir + 1) - (count Mod .intBlockCntYDir)
                    End If

                    'ステージグループ間隔の数
                    intXStgGrpCnt = intXBlockCnt \ .intBlkCntInStgGrpX
                    intYStgGrpCnt = intYBlockCnt \ .intBlkCntInStgGrpY

                    'プレート内部のポジション計算- ブロックサイズ X ブロック数 + ステージインターバル
                    dblWorkBlockPosX = (.dblBlockSizeXDir * intXBlockCnt) + (.dblBlockItvXDir * intXBlockCnt) + (.dblStgGrpItvX * intYStgGrpCnt)
                    dblWorkBlockPosY = (.dblBlockSizeYDir * intYBlockCnt) + (.dblBlockItvYDir * intYBlockCnt) + (.dblStgGrpItvY * intYStgGrpCnt)
                End With

                '対象ブロックの位置座標を代入
                With typGrpInfoArray(count)
                    .dblStgPosX = dblWorkBlockPosX
                    .dblStgPosX = dblWorkBlockPosY
                End With
            Next

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "DataAccess.CalcStagePos() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            CalcAllBlockStagePos = 0
        End Try

    End Function
#End Region

#Region "X方向のBlockNoとY方向のBlockNoからステージ位置を返す"
    '''=========================================================================
    ''' <summary>X方向のBlockNoとY方向のBlockNoからステージ位置を返す。</summary>
    ''' <param name="xBlockNo">(INP)X方向のブロック番号</param>
    ''' <param name="yBlockNo">(INP)Y方向のブロック番号</param>
    ''' <param name="stgx">    (OUT)ステージ位置X</param>
    ''' <param name="stgy">    (OUT)ステージ位置Y</param>
    ''' <returns>0=正常</returns>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Function GetTargetStagePosByXY(ByVal xBlockNo As Integer, ByVal yBlockNo As Integer, _
                                          ByRef stgx As Double, ByRef stgy As Double) As Integer

        Dim r As Integer

        Try
            r = cFRS_NORMAL

            ' 指定ブロック番号のチェック
            If ((typPlateInfo.intBlockCntXDir < (xBlockNo - 1)) Or (xBlockNo < 1)) _
                Or ((typPlateInfo.intBlockCntYDir < (yBlockNo - 1)) Or (yBlockNo < 1)) Then

            End If

            ' 指定位置のステージ位置取得
            stgx = gBlkStagePosX(xBlockNo - 1)
            stgy = gBlkStagePosY(yBlockNo - 1)

            ' X開始位置＋オフセット＋補正値
            stgx = stgx + typPlateInfo.dblTableOffsetXDir + gfCorrectPosX
            ' Y開始位置＋オフセット＋補正値
            stgy = stgy + typPlateInfo.dblTableOffsetYDir + gfCorrectPosY

        Catch ex As Exception
            Dim strMsg As String
            strMsg = "basTrimming.GetTargetStagePosByXY() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Function
#End Region

#Region "指定ブロック番号のステージ位置を取得"
    '''=========================================================================
    ''' <summary>指定されたブロック番号の位置情報を取得する。</summary>
    ''' <param name="curPltNo">   (INP)現在のプレート番号を設定</param>
    ''' <param name="curBlkNo">   (INP)現在のブロック番号を設定</param>
    ''' <param name="stgx">       (OUT)ステージ位置X</param>
    ''' <param name="stgy">       (OUT)ステージ位置Y</param>
    ''' <param name="dispPltPosX">(OUT)プレート番号X</param>
    ''' <param name="dispPltPosY">(OUT)プレート番号Y</param>
    ''' <param name="dispBlkPosX">(OUT)ブロック番号X</param>
    ''' <param name="dispBlkPosY">(OUT)ブロック番号Y</param>
    ''' <returns>トリミングの最終ブロックの場合BLOCK_END＝1、
    '''          最終プレート最終ブロックの場合PLATE_BLOCK_END=2を返す。</returns>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Function GetTargetStagePos(ByVal curPltNo As Integer, ByVal curBlkNo As Integer, _
                                        ByRef stgx As Double, ByRef stgy As Double, _
                                        ByRef dispPltPosX As Integer, ByRef dispPltPosY As Integer, _
                                        ByRef dispBlkPosX As Integer, ByRef dispBlkPosY As Integer) As Integer
        Dim strMsg As String
        Dim workStgPosX As Double
        Dim workStgPosY As Double
        Dim totalPlateCnt As Integer
        Dim totalBlockCnt As Integer

        GetTargetStagePos = cFRS_NORMAL
        Try
            ' 基準位置、ステップ方向により次のブロック位置を取得する。
            With typPlateInfo
                ' 最終ポジションの判定
                '----- V1.23.0.0⑨↓ -----
                totalPlateCnt = .intPlateCntXDir
                totalPlateCnt = totalPlateCnt * .intPlateCntYDir
                totalBlockCnt = .intBlockCntXDir
                totalBlockCnt = totalBlockCnt * .intBlockCntYDir

                'totalPlateCnt = .intPlateCntXDir * .intPlateCntYDir
                'totalBlockCnt = .intBlockCntXDir * .intBlockCntYDir             '###115 千鳥対応
                '----- V1.23.0.0⑨↑ -----

                ' パラメータチェック
                If ((curPltNo < 0) Or (curBlkNo < 0)) Then
                    GetTargetStagePos = -1 * ERR_CMD_PRM
                    Exit Function
                End If

                If (curPltNo > totalPlateCnt) Then
                    GetTargetStagePos = PLATE_BLOCK_END
                    Exit Function
                ElseIf (curBlkNo > totalBlockCnt) Then
                    GetTargetStagePos = BLOCK_END
                    Exit Function
                End If

                ' データ取得対象は"0"オリジンのため、ここで一つ減算する。
                curPltNo = curPltNo - 1
                curBlkNo = curBlkNo - 1

                ' ステップ&(リピート方向)
                If (.intDirStepRepeat = STEP_RPT_Y) _
                    Or (.intDirStepRepeat = STEP_RPT_CHIPXSTPY) Then
                    ' Y方向
                    Call GetBlockPos_StpY(curPltNo, curBlkNo, gSysPrm.stDEV.giBpDirXy, _
                                    .intPlateCntXDir, .intPlateCntYDir, _
                                    .intBlockCntXDir, .intBlockCntYDir, workStgPosX, workStgPosY, _
                                    dispPltPosX, dispPltPosY, dispBlkPosX, dispBlkPosY)
                ElseIf (.intDirStepRepeat = STEP_RPT_X) _
                       Or (.intDirStepRepeat = STEP_RPT_CHIPYSTPX) Then
                    ' X方向
                    Call GetBlockPos_StpX(curPltNo, curBlkNo, gSysPrm.stDEV.giBpDirXy, _
                                    .intPlateCntXDir, .intPlateCntYDir, _
                                    .intBlockCntXDir, .intBlockCntYDir, workStgPosX, workStgPosY, _
                                    dispPltPosX, dispPltPosY, dispBlkPosX, dispBlkPosY)
                Else
                    ' ステップ&リピートなし
                    '----- ###169↓ -----
                    ' ステップ&リピートなしでも表示用ブロック数を更新するため下記をCallする
                    Call GetBlockPos_StpY(curPltNo, curBlkNo, gSysPrm.stDEV.giBpDirXy, _
                                    .intPlateCntXDir, .intPlateCntYDir, _
                                    .intBlockCntXDir, .intBlockCntYDir, workStgPosX, workStgPosY, _
                                    dispPltPosX, dispPltPosY, dispBlkPosX, dispBlkPosY)

                    workStgPosX = 0.0                                   ' ステージ位置X,Yは0に再設定
                    workStgPosY = 0.0

                    'dispPltPosX = 1
                    'dispPltPosY = 1
                    'dispBlkPosX = 1
                    'dispBlkPosY = 1
                    '----- ###169↑ -----
                End If

                stgx = workStgPosX
                stgy = workStgPosY
            End With

        Catch ex As Exception
            strMsg = "basTrimming.GetTargetStagePos() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Function
#End Region

#Region "ステージオフセットの補正値を取得する"
    '''=========================================================================
    ''' <summary>ステージオフセットの補正値を取得する</summary>
    ''' <param name="pltRowNo">  (INP)現在のプレート番号</param>
    ''' <param name="blkRowNo">  (INP)現在のブロック番号</param>
    ''' <param name="AddSubPosX">(OUT)ステップオフセット量X</param>
    ''' <param name="AddSubPosY">(OUT)ステップオフセット量Y</param>
    ''' <param name="blkNoX">    (INP)ブロック番号X V1.20.0.0⑨</param>
    ''' <param name="blkNoY">    (INP)ブロック番号Y V1.20.0.0⑨</param>
    ''' <returns>cFRS_NORMAL = 正常</returns>
    '''=========================================================================
    Public Function GetStepOffSetPos(ByVal pltRowNo As Integer, ByVal blkRowNo As Integer, _
                                    ByRef AddSubPosX As Double, ByRef AddSubPosY As Double, ByVal blkNoX As Integer, ByVal blkNoY As Integer) As Integer

        'Dim workPosY As Double 'V1.22.0.0⑥
        'Dim a As Double
        'Dim curStagePosY As Double

        Try
            With typPlateInfo
                '----- V2.0.0.0⑪(V1.22.0.0⑥)↓ -----
                AddSubPosX = 0.0
                AddSubPosY = 0.0

                ' チップ並びY方向時のステップオフセットYの算出
                If (.intResistDir = 1) Then                             ' チップ並びはY方向 ?
                    ' ステップオフセットYの指定なしならNOP
                    If (.dblStepOffsetYDir = 0) Then Return (cFRS_NORMAL)

                    ''V4.12.2.1①↓'V6.0.5.0①
                    If (.intBlockCntXDir <= 1) Then
                        Return (cFRS_NORMAL)
                    End If
                    ''V4.12.2.1①↑'V6.0.5.0①

                    ' X方向移動時のY方向オフセット値を求める
                    AddSubPosY = (.dblStepOffsetYDir / (.intBlockCntXDir - 1)) * (blkNoX - 1)

                    Return (cFRS_NORMAL)

                Else
                    ' ステップオフセットXの指定なしならNOP
                    If (.dblStepOffsetXDir = 0) Then Return (cFRS_NORMAL)

                    ''V4.12.2.1①↓'V6.0.5.0①
                    If (.intBlockCntYDir <= 1) Then
                        Return (cFRS_NORMAL)
                    End If
                    ''V4.12.2.1①↑'V6.0.5.0①

                    ' Y方向移動時のX方向オフセット値を求める
                    AddSubPosX = (.dblStepOffsetXDir / (.intBlockCntYDir - 1)) * (blkNoY - 1)

                    Return (cFRS_NORMAL)

                End If
                '----- V2.0.0.0⑪(V1.22.0.0⑥)↑ -----

                'V1.22.0.0⑥
                ''Y方向の算出式
                ''workPosY = (gBlkStagePosY(.intBlockCntYDir - 1) + .dblStepOffsetYDir) / gBlkStagePosY(.intBlockCntYDir - 1)
                'If ((gBlkStagePosY(.intBlockCntYDir - 1) + .dblStepOffsetYDir) <> 0) And _
                '   (gBlkStagePosY(.intBlockCntYDir - 1) <> 0) Then
                '    workPosY = (gBlkStagePosY(.intBlockCntYDir - 1) + .dblStepOffsetYDir) / gBlkStagePosY(.intBlockCntYDir - 1)
                'Else
                '    workPosY = 0.0
                'End If
                'curStagePosY = gPltStagePosY(pltRowNo) + gBlkStagePosY(blkRowNo)
                'workPosY = workPosY * curStagePosY
                'AddSubPosY = workPosY - curStagePosY

                ''X方向の算出式
                'If (.dblStepOffsetXDir <> 0) Then
                '    'a = (.dblStepOffsetYDir + gBlkStagePosY(.intBlockCntYDir - 1)) / .dblStepOffsetXDir
                '    'AddSubPosX = workPosY / a
                '    If (.dblStepOffsetYDir + gBlkStagePosY(.intBlockCntYDir - 1) <> 0) And _
                '       (.dblStepOffsetXDir <> 0) Then
                '        a = (.dblStepOffsetYDir + gBlkStagePosY(.intBlockCntYDir - 1)) / .dblStepOffsetXDir
                '        AddSubPosX = workPosY / a
                '    Else
                '        AddSubPosX = 0
                '    End If
                'Else
                '    AddSubPosX = 0
                'End If
            End With

        Catch ex As Exception
            Dim strMsg As String
            strMsg = "DataAccess.GetBlockPos_StpY() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Function
#End Region

#Region "ステップY方向のプレート内のブロック位置を取得する"
    '''=========================================================================
    ''' <summary>ステップY方向の指定プレート番号、ブロック番号のステージ位置座標を取得する。</summary>
    ''' <param name="curPlateNo">  (INP)現在のプレート番号(0オリジン)</param>
    ''' <param name="curBlkNo">    (INP)現在のブロック番号(0オリジン)</param>
    ''' <param name="bpDir">       (INP)BP基準コーナー</param>
    ''' <param name="pltCntTarCol">(INP)プレート数X</param>
    ''' <param name="pltCntTarRow">(INP)プレート数Y</param>
    ''' <param name="blkCntTarCol">(INP)ブロック数X</param>
    ''' <param name="blkCntTarRow">(INP)ブロック数Y</param>
    ''' <param name="stgx">        (OUT)ステージ位置X</param>
    ''' <param name="stgy">        (OUT)ステージ位置Y</param>
    ''' <param name="pltCurPosX">  (OUT)プレート番号X</param>
    ''' <param name="pltCurPosY">  (OUT)プレート番号Y</param>
    ''' <param name="blkCurPosX">  (OUT)ブロック番号X</param>
    ''' <param name="blkCurPosY">  (OUT)ブロック番号Y</param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Function GetBlockPos_StpY(ByVal curPlateNo As Integer, ByVal curBlkNo As Integer, _
            ByVal bpDir As Integer, ByVal pltCntTarCol As Integer, ByVal pltCntTarRow As Integer, _
            ByVal blkCntTarCol As Integer, ByVal blkCntTarRow As Integer, _
            ByRef stgx As Double, ByRef stgy As Double, _
            ByRef pltCurPosX As Integer, ByRef pltCurPosY As Integer, _
            ByRef blkCurPosX As Integer, ByRef blkCurPosY As Integer) As Boolean

        Dim strMsg As String
        Dim curBlkCol As Integer
        Dim curBlkRow As Integer
        Dim workBlkCol As Integer
        Dim curPltCol As Integer
        Dim curPltRow As Integer
        Dim workPltCol As Integer
        Dim workBlockPosY As Double
        Dim addSubPosX As Double
        Dim addSubPosY As Double
        ' V4.12.0.0①　↓'V6.1.2.0②
        Dim DispBlkX As Integer
        Dim DispBlkY As Integer
        ' V4.12.0.0①　↑'V6.1.2.0②

        Try
            With typPlateInfo
                ' 対象となるブロックの列と行を取得
                ' 列番号の取得
                workPltCol = curPlateNo \ pltCntTarRow          ' 奇数/偶数判定のために保存
                workBlkCol = curBlkNo \ blkCntTarRow            ' 奇数/偶数判定のために保存(ブロック番号/ブロック数Y)

                '----- ###044(修正  ここから) -----
                curPltCol = workPltCol
                curBlkCol = workBlkCol

                'Select Case (bpDir)
                '    Case (BP_DIR_RIGHTUP), (BP_DIR_RIGHTDOWN)
                '        ' 右基準
                '        curPltCol = workPltCol
                '        curBlkCol = workBlkCol
                '    Case (BP_DIR_LEFTUP), (BP_DIR_LEFTDOWN)
                '        ' 左基準
                '        curPltCol = (pltCntTarCol - 1) - workPltCol
                '        curBlkCol = (blkCntTarCol - 1) - workBlkCol
                'End Select
                '----- ###044(修正  ここまで) -----

                ' 行番号の取得
                curPltRow = curPlateNo Mod pltCntTarRow
                curBlkRow = curBlkNo Mod blkCntTarRow

                '----- ###044(修正  ここから) -----
                ' プレート位置
                If ((workPltCol Mod 2) = 0) Then        ' 奇数/偶数判定
                    curPltRow = curPltRow
                Else
                    curPltRow = (pltCntTarRow - 1) - curPltRow
                End If

                ' ブロック位置
                ' V4.12.0.0①　↓'V6.1.2.0②
                ' Yステップが選択されている場合に、奇数、偶数によってステップを反転する
                If JudgeStepYInverse() Then
                    ' Y方向を逆転する。下からステップ動作
                    If ((workBlkCol Mod 2) = 0) Then        ' 奇数/偶数判定
                        DispBlkX = curBlkRow
                        curBlkRow = (blkCntTarRow - 1) - curBlkRow
                    Else
                        DispBlkX = (blkCntTarRow - 1) - curBlkRow
                        curBlkRow = curBlkRow
                    End If
                Else
                    ' 従来通りの上からステップ
                    If ((workBlkCol Mod 2) = 0) Then        ' 奇数/偶数判定
                        curBlkRow = curBlkRow
                    Else
                        curBlkRow = (blkCntTarRow - 1) - curBlkRow
                    End If
                End If
                ' V4.12.0.0①↑'V6.1.2.0②

                'Select Case (bpDir)
                '    Case (BP_DIR_RIGHTUP), (BP_DIR_LEFTUP)
                '        ' 上基準
                '        ' プレート位置
                '        If ((workPltCol Mod 2) = 0) Then        ' 奇数/偶数判定
                '            curPltRow = curPltRow
                '        Else
                '            curPltRow = (pltCntTarRow - 1) - curPltRow
                '        End If

                '        ' ブロック位置
                '        If ((workBlkCol Mod 2) = 0) Then        ' 奇数/偶数判定
                '            curBlkRow = curBlkRow
                '        Else
                '            curBlkRow = (blkCntTarRow - 1) - curBlkRow
                '        End If

                '    Case (BP_DIR_RIGHTDOWN), (BP_DIR_LEFTDOWN)
                '        ' 下基準
                '        ' プレート位置
                '        If ((workPltCol Mod 2) = 0) Then        ' 奇数/偶数判定
                '            curPltRow = (pltCntTarRow - 1) - curPltRow
                '        Else
                '            curPltRow = curPltRow
                '        End If
                '        ' ブロック位置
                '        If ((workBlkCol Mod 2) = 0) Then        ' 奇数/偶数判定
                '            curBlkRow = (blkCntTarRow - 1) - curBlkRow
                '        Else
                '            curBlkRow = curBlkRow
                '        End If
                'End Select
                '----- ###044(修正  ここまで) -----

                'OPT:TY2
                If (Form1.CmdTy2.Enabled = True) Then
                    ' TY2オプションがある場合、Y方向のブロック位置毎に調整値を加減算する。
                    workBlockPosY = gBlkStagePosY(curBlkRow) + typTy2InfoArray(curBlkRow).dblTy22
                Else
                    ' 通常の場合はそのまま   
                    workBlockPosY = gBlkStagePosY(curBlkRow)
                End If

                ' ステージ座標の取得
                stgx = gPltStagePosX(curPltCol) + gBlkStagePosX(curBlkCol)
                '                stgy = gPltStagePosY(curPltRow) + gBlkStagePosY(curBlkRow)
                stgy = gPltStagePosY(curPltRow) + workBlockPosY

                ' ステージオフセットの補正値を取得
                'GetStepOffSetPos(curPltRow, curBlkRow, addSubPosX, addSubPosY)                                 'V1.20.0.0⑨
                GetStepOffSetPos(curPltRow, curBlkRow, addSubPosX, addSubPosY, curBlkCol + 1, curBlkRow + 1)    'V1.20.0.0⑨
                stgx = stgx + addSubPosX
                stgy = stgy + addSubPosY

                ' 行、列の位置情報を設定
                pltCurPosX = curPltCol + 1
                pltCurPosY = curPltRow + 1
                ' V4.12.0.0①　↓　'V6.1.2.0②
                ' Yステップが選択されている場合に、奇数、偶数によってステップを反転する
                If JudgeStepYInverse() Then
                    ' Y方向を逆転する。下からステップ動作
                    blkCurPosY = DispBlkX + 1
                Else
                    blkCurPosY = curBlkRow + 1
                End If
                ' V4.12.0.0①　↑　'V6.1.2.0②
                blkCurPosX = curBlkCol + 1

            End With

        Catch ex As Exception
            strMsg = "DataAccess.GetBlockPos_StpY() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try

    End Function
#End Region

#Region "ステップX方向のプレート内のブロック位置を取得する"
    '''=========================================================================
    '''<summary>ステップX方向の指定プレート番号、ブロック番号のステージ位置座標を取得する。</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Function GetBlockPos_StpX(ByVal curPlateNo As Integer, ByVal curBlkNo As Integer, _
            ByVal bpDir As Integer, ByVal pltCntTarCol As Integer, ByVal pltCntTarRow As Integer, _
            ByVal blkCntTarCol As Integer, ByVal blkCntTarRow As Integer, _
            ByRef stgx As Double, ByRef stgy As Double, _
            ByRef pltCurPosX As Integer, ByRef pltCurPosY As Integer, _
            ByRef blkCurPosX As Integer, ByRef blkCurPosY As Integer) As Boolean

        Dim strMsg As String
        Dim curBlkCol As Integer
        Dim curBlkRow As Integer
        Dim workBlkRow As Integer
        Dim curPltCol As Integer
        Dim workPltRow As Integer
        Dim curPltRow As Integer
        Dim workBlockPosY As Double
        Dim addSubPosX As Double
        Dim addSubPosY As Double

        Try
            With typPlateInfo
                ' 対象となるブロックの列と行を取得
                ' 行番号の取得
                workPltRow = curPlateNo \ pltCntTarCol          ' 奇数/偶数判定のために保存
                workBlkRow = curBlkNo \ blkCntTarCol            ' 奇数/偶数判定のために保存

                '----- ###044(修正  ここから) -----
                curPltRow = workPltRow
                curBlkRow = workBlkRow

                'Select Case (bpDir)
                '    Case (BP_DIR_RIGHTUP), (BP_DIR_LEFTUP)
                '        ' 上基準
                '        curPltRow = workPltRow
                '        curBlkRow = workBlkRow
                '    Case (BP_DIR_RIGHTDOWN), (BP_DIR_LEFTDOWN)
                '        ' 下基準
                '        curPltRow = (pltCntTarRow - 1) - workPltRow
                '        curBlkRow = (blkCntTarRow - 1) - workBlkRow
                'End Select
                '----- ###044(修正  ここまで) -----

                ' 列番号の取得
                curPltCol = curPlateNo Mod pltCntTarCol
                curBlkCol = curBlkNo Mod blkCntTarCol

                '----- ###044(修正  ここから) -----
                ' プレート位置
                If ((workPltRow Mod 2) = 0) Then        ' 奇数/偶数判定
                    curPltCol = curPltCol
                Else
                    curPltCol = (pltCntTarCol - 1) - curPltCol
                End If
                'ブロック位置
                If ((workBlkRow Mod 2) = 0) Then        ' 奇数/偶数判定
                    curBlkCol = curBlkCol
                Else
                    curBlkCol = (blkCntTarCol - 1) - curBlkCol
                End If

                'Select Case (bpDir)
                '    Case (BP_DIR_RIGHTUP), (BP_DIR_RIGHTDOWN)
                '        ' 右基準
                '        ' プレート位置
                '        If ((workPltRow Mod 2) = 0) Then        ' 奇数/偶数判定
                '            curPltCol = curPltCol
                '        Else
                '            curPltCol = (pltCntTarCol - 1) - curPltCol
                '        End If
                '        'ブロック位置
                '        If ((workBlkRow Mod 2) = 0) Then        ' 奇数/偶数判定
                '            curBlkCol = curBlkCol
                '        Else
                '            curBlkCol = (blkCntTarCol - 1) - curBlkCol
                '        End If

                '    Case (BP_DIR_LEFTUP), (BP_DIR_LEFTDOWN)
                '        ' 左基準
                '        ' プレート位置
                '        If ((workPltRow Mod 2) = 0) Then        ' 奇数/偶数判定
                '            curPltCol = (pltCntTarCol - 1) - curPltCol
                '        Else
                '            curPltCol = curPltCol
                '        End If
                '        ' ブロック位置
                '        If ((workBlkRow Mod 2) = 0) Then        ' 奇数/偶数判定
                '            curBlkCol = (blkCntTarCol - 1) - curBlkCol
                '        Else
                '            curBlkCol = curBlkCol
                '        End If
                'End Select
                '----- ###044(修正  ここまで) -----

                'OPT:TY2
                If (Form1.CmdTy2.Enabled = True) Then
                    ' TY2オプションがある場合、Y方向のブロック位置毎に調整値を加減算する。
                    workBlockPosY = gBlkStagePosY(curBlkRow) + typTy2InfoArray(curBlkRow).dblTy22
                Else
                    ' 通常の場合はそのまま   
                    workBlockPosY = gBlkStagePosY(curBlkRow)
                End If

                ' ステージ座標の取得
                stgx = gPltStagePosX(curPltCol) + gBlkStagePosX(curBlkCol)
                stgy = gPltStagePosY(curPltRow) + workBlockPosY
                'stgy = gPltStagePosY(curPltRow) + gBlkStagePosY(curBlkRow)

                ' ステージオフセットの補正値を取得
                'GetStepOffSetPos(curPltRow, curBlkRow, addSubPosX, addSubPosY)                                 'V1.20.0.0⑨
                GetStepOffSetPos(curPltRow, curBlkRow, addSubPosX, addSubPosY, curBlkCol + 1, curBlkRow + 1)    'V1.20.0.0⑨
                stgx = stgx + addSubPosX
                stgy = stgy + addSubPosY

                ' 行、列の位置情報を設定
                pltCurPosX = curPltCol + 1
                pltCurPosY = curPltRow + 1
                blkCurPosX = curBlkCol + 1
                blkCurPosY = curBlkRow + 1
            End With

        Catch ex As Exception
            strMsg = "DataAccess.GetBlockPos_StpX() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try

    End Function
#End Region

#Region "ｸﾞﾙｰﾌﾟ間隔算出(ｸﾞﾙｰﾌﾟｲﾝﾀｰﾊﾞﾙ)"
    '''=========================================================================
    '''<summary>ｸﾞﾙｰﾌﾟ間隔算出(ｸﾞﾙｰﾌﾟｲﾝﾀｰﾊﾞﾙ)</summary>
    '''<param name="intBNum">(INP) 抵抗番号</param>
    '''<returns>ｸﾞﾙｰﾌﾟ間隔のみの積算値</returns>
    '''<remarks>NETで使用</remarks>
    '''=========================================================================
    Public Function CalcGrpInterval(ByRef intBNum As Short) As Double

        Dim intCDir As Short
        Dim intGNx As Short
        Dim intGNY As Short
        Dim intStpMax As Short
        Dim intBMax As Short
        Dim i As Short
        Dim dblBMov As Double
        Dim strMSG As String

        Try

            ' データ設定
            intGNx = typPlateInfo.intGroupCntInBlockXBp ' グループ数X,Y
            intGNY = typPlateInfo.intGroupCntInBlockYStage
            intCDir = typPlateInfo.intResistDir         ' チップ並び方向取得(CHIP-NETのみ)
            If intCDir = 0 Then                         ' X方向
                intStpMax = intGNx
            Else
                intStpMax = intGNY
            End If

            dblBMov = 0
            intBMax = 0
            For i = 1 To intStpMax - 1
                intBMax = intBMax + typCirInInfoArray(i).intCiP2 ' ｻｰｷｯﾄ数
                If intBNum <= intBMax Then
                    Exit For
                End If
                dblBMov = dblBMov + typCirInInfoArray(i).dblCiP3 ' ｸﾞﾙｰﾌﾟ間隔
            Next i
            CalcGrpInterval = dblBMov

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "DataAccess.CalcGrpInterval() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            CalcGrpInterval = 0.0
        End Try

    End Function
#End Region

#Region "ｽﾃｯﾌﾟ間隔算出(ｽﾃｯﾌﾟｲﾝﾀｰﾊﾞﾙ)【ｽﾃｯﾌﾟﾃﾞｰﾀ】"
    '''=========================================================================
    '''<summary>ｽﾃｯﾌﾟ間隔算出(ｽﾃｯﾌﾟｲﾝﾀｰﾊﾞﾙ)</summary>
    '''<param name="intBNum">(INP) ﾌﾞﾛｯｸNo.</param>
    '''<returns>ｽﾃｯﾌﾟ間隔のみの積算値</returns>
    '''=========================================================================
    Public Function CalcStepInterval(ByRef intBNum As Short) As Double

        Dim intCDir As Short
        Dim intGNx As Short
        Dim intGNY As Short
        Dim intStpMax As Short
        Dim intBMax As Short
        Dim i As Short
        Dim dblBMov As Double
        Dim strMSG As String

        Try

            ' グループ数(ｽﾃｯﾌﾟ数)X,Y
            intGNx = typPlateInfo.intGroupCntInBlockXBp
            intGNY = typPlateInfo.intGroupCntInBlockYStage
            intCDir = typPlateInfo.intResistDir               ' チップ並び方向取得(CHIP-NETのみ)
            If intCDir = 0 Then ' X方向
                intStpMax = intGNx
            Else
                intStpMax = intGNY
            End If

            dblBMov = 0
            intBMax = 0
            For i = 1 To intStpMax - 1
                intBMax = intBMax + typStepInfoArray(i).intSP2 ' ﾌﾞﾛｯｸ数
                If intBNum <= intBMax Then
                    Exit For
                End If
                dblBMov = dblBMov + typStepInfoArray(i).dblSP3 ' ｸﾞﾙｰﾌﾟ間隔(ｽﾃｯﾌﾟ間ｲﾝﾀｰﾊﾞﾙ)
            Next i
            CalcStepInterval = dblBMov

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "DataAccess.CalcStepInterval() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            CalcStepInterval = 0.0
        End Try

    End Function
#End Region

#Region "指定ｽﾃｯﾌﾟ番号のﾌﾞﾛｯｸ数とｽﾃｯﾌﾟｲﾝﾀｰﾊﾞﾙを返す【ｽﾃｯﾌﾟﾃﾞｰﾀ】"
    '''=========================================================================
    '''<summary>指定ｽﾃｯﾌﾟ番号のﾌﾞﾛｯｸ数とｽﾃｯﾌﾟｲﾝﾀｰﾊﾞﾙを返す</summary>
    '''<param name="intStepNo">  (INP) ｽﾃｯﾌﾟ番号</param>
    '''<param name="intBlockNum">(OUT) ﾌﾞﾛｯｸ数</param>
    '''<param name="dblStepInt"> (OUT) ｽﾃｯﾌﾟｲﾝﾀｰﾊﾞﾙ</param>
    '''<returns>TRUE:ﾃﾞｰﾀあり, FALSE:ﾃﾞｰﾀなし</returns>
    '''=========================================================================
    Public Function GetStepData(ByRef intStepNo As Short, ByRef intBlockNum As Short, ByRef dblStepInt As Double) As Boolean

        Dim bRetc As Boolean
        Dim i As Short
        Dim strMSG As String

        Try

            bRetc = False
            For i = 1 To MaxCntStep
                If (intStepNo = typStepInfoArray(i).intSP1) Then    ' ｽﾃｯﾌﾟ番号一致
                    intBlockNum = typStepInfoArray(i).intSP2        ' ﾌﾞﾛｯｸ数
                    dblStepInt = typStepInfoArray(i).dblSP3         ' ｽﾃｯﾌﾟｲﾝﾀｰﾊﾞﾙ
                    bRetc = True
                    Exit For
                End If
            Next
            GetStepData = bRetc

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "DataAccess.GetStepData() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            GetStepData = False
        End Try

    End Function
#End Region

#Region "指定ｽﾃｯﾌﾟ番号のｻｰｷｯﾄ数とｸﾞﾙｰﾌﾟ間ｲﾝﾀｰﾊﾞﾙを返す【ｽﾃｯﾌﾟﾃﾞｰﾀ】"
    '''=========================================================================
    '''<summary>指定ｽﾃｯﾌﾟ番号のﾌﾞﾛｯｸ数とｽﾃｯﾌﾟｲﾝﾀｰﾊﾞを返す</summary>
    '''<param name="intStepNo">(INP) ｽﾃｯﾌﾟ番号</param>
    '''<param name="iCirCnt">  (OUT) ｻｰｷｯﾄ数</param>
    '''<param name="dGrpInt">  (OUT) ｸﾞﾙｰﾌﾟ間ｲﾝﾀｰﾊﾞﾙ</param>
    '''<returns>TRUE:ﾃﾞｰﾀあり, FALSE:ﾃﾞｰﾀなし</returns>
    '''=========================================================================
    Public Function GetCirInInfoData(ByRef intStepNo As Short, ByRef iCirCnt As Short, ByRef dGrpInt As Double) As Boolean

        Dim bRetc As Boolean
        Dim iCnt As Short
        Dim strMSG As String

        Try

            bRetc = False
            For iCnt = 1 To MaxCntStep
                If (intStepNo = typCirInInfoArray(iCnt).intCiP1) Then ' ｽﾃｯﾌﾟ番号一致
                    iCirCnt = typCirInInfoArray(iCnt).intCiP2 ' ｻｰｷｯﾄ数
                    dGrpInt = typCirInInfoArray(iCnt).dblCiP3 ' ｸﾞﾙｰﾌﾟ間ｲﾝﾀｰﾊﾞﾙ
                    bRetc = True
                    Exit For
                End If
            Next
            GetCirInInfoData = bRetc

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "DataAccess.GetCirInInfoData() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Function
#End Region

#Region "指定ｽﾃｯﾌﾟ番号の抵抗数とｸﾞﾙｰﾌﾟｲﾝﾀｰﾊﾞﾙを返す【ｸﾞﾙｰﾌﾟﾃﾞｰﾀ】"
    '''=========================================================================
    '''<summary>指定ｸﾞﾙｰﾌﾟ番号のﾌﾞﾛｯｸ数とｸﾞﾙｰﾌﾟｲﾝﾀｰﾊﾞﾙを返す</summary>
    '''<param name="intGrpNo"> (INP)ｸﾞﾙｰﾌﾟ番号</param>
    '''<param name="intResNum">(OUT)抵抗数</param>
    '''<param name="dblGrpInt">(OUT)ｸﾞﾙｰﾌﾟｲﾝﾀｰﾊﾞﾙ</param>
    '''<returns>TRUE:ﾃﾞｰﾀあり, FALSE:ﾃﾞｰﾀなし</returns>
    '''=========================================================================
    Public Function GetGrpData(ByRef intGrpNo As Short, ByRef intResNum As Short, ByRef dblGrpInt As Double) As Boolean

        Dim bRetc As Boolean
        Dim i As Short

        Dim dblCSx As Double
        Dim dblCSy As Double
        Dim cs As Double
        Dim intCDir As Short
        Dim strMSG As String

        Try

            ' ﾁｯﾌﾟｻｲｽﾞ
            dblCSx = typPlateInfo.dblChipSizeXDir
            dblCSy = typPlateInfo.dblChipSizeYDir

            intCDir = typPlateInfo.intResistDir                 ' チップ並び方向取得(CHIP-NETのみ)

            If intCDir = 0 Then                                 ' チップ並び方向 = X ? 
                cs = dblCSx                                     ' cs = ﾁｯﾌﾟｻｲｽﾞX 
            Else
                cs = dblCSy                                     ' cs = ﾁｯﾌﾟｻｲｽﾞY 
            End If

            bRetc = False
            For i = 1 To MaxCntStep
                If (intGrpNo = typGrpInfoArray(i).intGP1) Then  ' ｸﾞﾙｰﾌﾟ番号一致
                    intResNum = typGrpInfoArray(i).intGP2       ' ｸﾞﾙｰﾌﾟ内抵抗数
                    dblGrpInt = typGrpInfoArray(i).dblGP3 + cs  ' ｸﾞﾙｰﾌﾟｲﾝﾀｰﾊﾞﾙ = ｸﾞﾙｰﾌﾟｲﾝﾀｰﾊﾞﾙ + ﾁｯﾌﾟｻｲｽﾞ

                    bRetc = True
                    Exit For
                End If
            Next
            GetGrpData = bRetc

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "DataAccess.GetGrpData() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            GetGrpData = False
        End Try

    End Function
#End Region

#Region "指定抵抗番号のｶｯﾄ数を返す【抵抗ﾃﾞｰﾀ】"
    '''=========================================================================
    '''<summary>指定抵抗番号のｶｯﾄ数を返す</summary>
    '''<param name="intRegNo"> (INP) 抵抗番号</param>
    '''<param name="intCutNum">(OUT) ｶｯﾄ数</param>
    '''<returns>TRUE:ﾃﾞｰﾀあり, FALSE:ﾃﾞｰﾀなし</returns>
    '''=========================================================================
    Public Function GetRegCutNum(ByRef intRegNo As Short, ByRef intCutNum As Short) As Boolean

        Dim bRetc As Boolean
        Dim i As Short
        Dim strMSG As String

        Try
            bRetc = False
            For i = 1 To MaxCntResist
                If (intRegNo = typResistorInfoArray(i).intResNo) Then   ' 抵抗番号一致
                    intCutNum = typResistorInfoArray(i).intCutCount     ' ｶｯﾄ数
                    bRetc = True
                    Exit For
                End If
            Next
            GetRegCutNum = bRetc

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "DataAccess.GetRegCutNum() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            GetRegCutNum = False
        End Try

    End Function
#End Region

#Region "指定配列番号の抵抗番号を返す【抵抗ﾃﾞｰﾀ】"
    '''=========================================================================
    '''<summary>指定配列番号の抵抗番号を返す</summary>
    '''<param name="intRegNo"> (INP) 配列番号</param>
    '''<param name="intRegNum">(OUT) 抵抗番号</param>
    '''<returns>TRUE:ﾃﾞｰﾀあり, FALSE:ﾃﾞｰﾀなし</returns>
    '''=========================================================================
    Public Function GetRegNum(ByRef intRegNo As Short, ByRef intRegNum As Short) As Boolean

        Dim bRetc As Boolean
        Dim strMSG As String

        Try
            bRetc = False
            If ((1 <= intRegNo) And (MaxCntResist >= intRegNo)) Then
                intRegNum = typResistorInfoArray(intRegNo).intResNo ' 抵抗番号
                bRetc = True
            End If
            GetRegNum = bRetc

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "DataAccess.GetRegNum() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            GetRegNum = False
        End Try

    End Function
#End Region

#Region "指定抵抗番号、ｶｯﾄ番号のｽﾀｰﾄﾎﾟｲﾝﾄを返す"
    '''=========================================================================
    '''<summary>指定抵抗番号、ｶｯﾄ番号のｽﾀｰﾄﾎﾟｲﾝﾄを返す</summary>
    '''<param name="intRegNo">(INP) 抵抗番号</param>
    '''<param name="intCutNo">(INP) ｶｯﾄ番号</param>
    '''<param name="dblX"    >(OUT) ｽﾀｰﾄﾎﾟｲﾝﾄX</param>
    '''<param name="dblY"    >(OUT) ｽﾀｰﾄﾎﾟｲﾝﾄY</param>
    '''<returns>TRUE:ﾃﾞｰﾀあり, FALSE:ﾃﾞｰﾀなし</returns>
    '''=========================================================================
    Public Function GetCutStartPoint(ByRef intRegNo As Short, ByRef intCutNo As Short, ByRef dblX As Double, ByRef dblY As Double) As Boolean

        Dim bRetc As Boolean
        Dim i As Short
        Dim j As Short

        bRetc = False
        For i = 1 To MaxCntResist
            If (intRegNo = typResistorInfoArray(i).intResNo) Then                       ' 抵抗番号一致
                For j = 1 To MaxCntCut
                    If (intCutNo = typResistorInfoArray(i).ArrCut(j).intCutNo) Then     ' ｶｯﾄ番号一致
                        dblX = typResistorInfoArray(i).ArrCut(j).dblStartPointX         ' ｽﾀｰﾄﾎﾟｲﾝﾄ
                        dblY = typResistorInfoArray(i).ArrCut(j).dblStartPointY
                        bRetc = True
                        GetCutStartPoint = bRetc
                        Exit Function
                    End If
                Next
            End If
        Next
        GetCutStartPoint = bRetc
    End Function
#End Region

#Region "指定抵抗番号、ｶｯﾄ番号のｽﾀｰﾄﾎﾟｲﾝﾄを設定する"
    '''=========================================================================
    '''<summary>指定抵抗番号、ｶｯﾄ番号のｽﾀｰﾄﾎﾟｲﾝﾄを設定する</summary>
    '''<param name="intRegNo">(INP) 抵抗番号</param>
    '''<param name="intCutNo">(INP) ｶｯﾄ番号</param>
    '''<param name="dblX"    >(INP) ｽﾀｰﾄﾎﾟｲﾝﾄX</param>
    '''<param name="dblY"    >(INP) ｽﾀｰﾄﾎﾟｲﾝﾄY</param>
    '''<returns>TRUE:成功, FALSE:失敗</returns>
    '''<remarks>ﾃﾞｰﾀを上書きするため、元のﾃﾞｰﾀがない場合は失敗する</remarks>
    '''=========================================================================
    Public Function SetCutStartPoint(ByRef intRegNo As Short, ByRef intCutNo As Short, ByRef dblX As Double, ByRef dblY As Double) As Boolean

        Dim bRetc As Boolean
        Dim i As Short
        Dim j As Short

        bRetc = False
        For i = 1 To MaxCntResist
            If (intRegNo = typResistorInfoArray(i).intResNo) Then                    ' 抵抗番号一致
                For j = 1 To MaxCntCut
                    If (intCutNo = typResistorInfoArray(i).ArrCut(j).intCutNo) Then  ' ｶｯﾄ番号一致
                        typResistorInfoArray(i).ArrCut(j).dblStartPointX = dblX      ' ｽﾀｰﾄﾎﾟｲﾝﾄ
                        typResistorInfoArray(i).ArrCut(j).dblStartPointY = dblY
                        bRetc = True
                        Exit For
                    End If
                Next
            End If
        Next
        SetCutStartPoint = bRetc
    End Function
#End Region

    'V5.0.0.6⑩↓
#Region "ティーチングポイントを０で初期化する"
    '''=========================================================================
    ''' <summary>
    ''' ティーチングポイントを０で初期化する
    ''' </summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub ResetCutTeachPoint()
        Try
            Dim i As Short
            Dim j As Short

            For i = 1 To MaxCntResist
                For j = 1 To MaxCntCut
                    typResistorInfoArray(i).ArrCut(j).dblTeachPointX = 0.0
                    typResistorInfoArray(i).ArrCut(j).dblTeachPointY = 0.0
                Next
            Next
        Catch ex As Exception
            MsgBox("DataAccess.ResetCutTeachPoint() TRAP ERROR = " + ex.Message)
        End Try
    End Sub
#End Region

#Region "指定抵抗番号、カット番号のティーチングポイントを加算したスタートポイントを返す"
    '''=========================================================================
    '''<summary>指定抵抗番号、カット番号のスタートポイントを返す</summary>
    '''<param name="intRegNo">(INP) 抵抗番号</param>
    '''<param name="intCutNo">(INP) カット番号</param>
    '''<param name="dblX"    >(OUT) スタートポイントX</param>
    '''<param name="dblY"    >(OUT) スタートポイントY</param>
    '''<returns>TRUE:データあり, FALSE:データなし</returns>
    '''=========================================================================
    Public Function GetCutStartPointAddTeachPoint(ByRef intRegNo As Short, ByRef intCutNo As Short, ByRef dblX As Double, ByRef dblY As Double) As Boolean

        Try
            dblX = typResistorInfoArray(intRegNo).ArrCut(intCutNo).dblStartPointX + typResistorInfoArray(intRegNo).ArrCut(intCutNo).dblTeachPointX  ' スタートポイント
            dblY = typResistorInfoArray(intRegNo).ArrCut(intCutNo).dblStartPointY + typResistorInfoArray(intRegNo).ArrCut(intCutNo).dblTeachPointY
            Return (True)
        Catch ex As Exception
            MsgBox("DataAccess.GetCutStartPointAddTeachPoint() TRAP ERROR = " + ex.Message)
            Return (False)
        End Try

    End Function
#End Region
    'V5.0.0.6⑩↑

#Region "指定抵抗番号、ｶｯﾄ番号のﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄを返す"
    '''=========================================================================
    '''<summary>指定抵抗番号、ｶｯﾄ番号のﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄを返す</summary>
    '''<param name="intRegNo">(INP) 抵抗番号</param>
    '''<param name="intCutNo">(INP) ｶｯﾄ番号</param>
    '''<param name="dblX"    >(OUT) ﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄX</param>
    '''<param name="dblY"    >(OUT) ﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄY</param>
    '''<returns>TRUE:ﾃﾞｰﾀあり, FALSE:ﾃﾞｰﾀなし</returns>
    '''=========================================================================
    Public Function GetCutTeachPoint(ByRef intRegNo As Short, ByRef intCutNo As Short, ByRef dblX As Double, ByRef dblY As Double) As Boolean

        Dim bRetc As Boolean
        Dim i As Short
        Dim j As Short

        bRetc = False
        For i = 1 To MaxCntResist
            If (intRegNo = typResistorInfoArray(i).intResNo) Then                    ' 抵抗番号一致
                For j = 1 To MaxCntCut
                    If (intCutNo = typResistorInfoArray(i).ArrCut(j).intCutNo) Then  ' ｶｯﾄ番号一致
                        dblX = typResistorInfoArray(i).ArrCut(j).dblTeachPointX      ' ﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄ
                        dblY = typResistorInfoArray(i).ArrCut(j).dblTeachPointY
                        bRetc = True
                        Exit For
                    End If
                Next
                If (bRetc) Then
                    Exit For
                End If
            End If
        Next
        GetCutTeachPoint = bRetc
    End Function
#End Region

#Region "指定抵抗番号、ｶｯﾄ番号のﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄを設定する"
    '''=========================================================================
    '''<summary>指定抵抗番号、ｶｯﾄ番号のﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄを設定する</summary>
    '''<param name="intRegNo">(INP) 抵抗番号</param>
    '''<param name="intCutNo">(INP) ｶｯﾄ番号</param>
    '''<param name="dblX"    >(INP) ﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄX</param>
    '''<param name="dblY"    >(INP) ﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄY</param>
    '''<returns>TRUE:成功, FALSE:失敗</returns>
    '''<remarks>ﾃﾞｰﾀを上書きするため、元のﾃﾞｰﾀがない場合は失敗する</remarks>
    '''=========================================================================
    Public Function SetCutTeachPoint(ByRef intRegNo As Short, ByRef intCutNo As Short, ByRef dblX As Double, ByRef dblY As Double) As Boolean
        Dim bRetc As Boolean
        Dim i As Short
        Dim j As Short

        bRetc = False
        For i = 1 To MaxCntResist
            If (intRegNo = typResistorInfoArray(i).intResNo) Then                    ' 抵抗番号一致
                For j = 1 To MaxCntCut
                    If (intCutNo = typResistorInfoArray(i).ArrCut(j).intCutNo) Then  ' ｶｯﾄ番号一致
                        typResistorInfoArray(i).ArrCut(j).dblTeachPointX = dblX      ' ﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄ
                        typResistorInfoArray(i).ArrCut(j).dblTeachPointY = dblY
                        bRetc = True
                        Exit For
                    End If
                Next
            End If
        Next
        SetCutTeachPoint = bRetc
    End Function

#End Region

#Region "抵抗番号とｶｯﾄ番号からｶｯﾄﾃﾞｰﾀの抵抗抵抗ﾃﾞｰﾀを返す"
    '''=========================================================================
    '''<summary>指定抵抗番号、ｶｯﾄ番号のｶｯﾄﾃﾞｰﾀ配列番号を返す</summary>
    '''<param name="i"       >(INP)抵抗番号(1～9999)</param>
    '''<param name="intCutNo">(INP)ｶｯﾄ番号(1～n)</param>
    '''<param name="n"       >(OUT)ｶｯﾄﾃﾞｰﾀの抵抗ﾃﾞｰﾀのｲﾝﾃﾞｯｸｽ</param>
    '''<param name="nn"      >(OUT)ｶｯﾄﾃﾞｰﾀのｲﾝﾃﾞｯｸｽ</param>
    '''<returns>TRUE:成功, FALSE:失敗</returns>
    '''=========================================================================
    Public Function GetResistorCutAddress(ByRef i As Short, ByRef intCutNo As Short, ByRef n As Short, ByRef nn As Short) As Boolean

        Dim bRetc As Boolean
        'Dim intRegNo As Short
        'Dim k As Short
        Dim j As Short

        bRetc = False
        'intRegNo = typResistorInfoArray(i).intResNo                                 ' 抵抗番号
        'For k = 1 To MaxCntResist                                                   ' 最大抵抗
        '    If (intRegNo = typResistorInfoArray(k).intResNo) Then                   ' 抵抗番号一致
        For j = 1 To MaxCntCut
            If (intCutNo = typResistorInfoArray(i).ArrCut(j).intCutNo) Then ' ｶｯﾄ番号一致
                n = i
                nn = j
                bRetc = True
                GetResistorCutAddress = bRetc
                Exit Function
            End If
        Next
        '    End If
        'Next
        GetResistorCutAddress = bRetc
    End Function
#End Region

#Region "指定抵抗番号、ｶｯﾄ番号のｶｯﾄ種別と方向を返す"
    '''=========================================================================
    '''<summary>指定抵抗番号、ｶｯﾄ番号のｶｯﾄ種別と方向を返す</summary>
    '''<param name="intRegNo">(INP) 抵抗番号</param>
    '''<param name="intCutNo">(INP) ｶｯﾄ番号</param>
    '''<param name="iKind"   >(OUT) ｶｯﾄ種別</param>
    '''<param name="iDir"    >(OUT) 方向</param>
    '''<returns>TRUE:ﾃﾞｰﾀあり, FALSE:ﾃﾞｰﾀなし</returns>
    '''=========================================================================
    Public Function GetCutKindDir(ByRef intRegNo As Short, ByRef intCutNo As Short, ByRef iKind As Short, ByRef iDir As Short) As Boolean

        Dim bRetc As Boolean
        Dim i As Short
        Dim j As Short
        Dim s As String

        bRetc = False
        For i = 1 To MaxCntResist
            If (intRegNo = typResistorInfoArray(i).intResNo) Then                   ' 抵抗番号一致
                For j = 1 To MaxCntCut
                    If (intCutNo = typResistorInfoArray(i).ArrCut(j).intCutNo) Then ' ｶｯﾄ番号一致
                        s = typResistorInfoArray(i).ArrCut(j).strCutType            ' ｶｯﾄ形状
                        ' ｶｯﾄ形状をｶｯﾄ種別に変換
                        iKind = Form1.Utility1.GetCutTypeNum(s.Trim())                      ' 前後の空白を削除して渡す
                        iDir = typResistorInfoArray(i).ArrCut(j).intCutDir
                        bRetc = True
                        GetCutKindDir = bRetc
                        Exit Function
                    End If
                Next
            End If
        Next
        GetCutKindDir = bRetc
    End Function
#End Region

#Region "NGデータ 指定抵抗配列番号の指定ｶｯﾄ番号のｶｯﾄﾃﾞｰﾀ配列番号を返す"
#If False Then  'V5.0.0.8①
    '''=========================================================================
    '''<summary>NGデータ 指定抵抗配列番号の指定ｶｯﾄ番号のｶｯﾄﾃﾞｰﾀ配列番号を返す</summary>
    '''<param name="i"       >(INP) 抵抗番号</param>
    '''<param name="intCutNo">(INP) ｶｯﾄ番号</param>
    '''<param name="n"       >(OUT) ｶｯﾄﾃﾞｰﾀ配列番号</param>
    '''<param name="nn"      >(OUT) ｶｯﾄﾃﾞｰﾀ配列ｶｯﾄ配列番号</param>
    '''<returns>TRUE:成功, FALSE:失敗</returns>
    '''=========================================================================
    Public Function GetNGResistorCutAddress(ByRef i As Short, ByRef intCutNo As Short, ByRef n As Short, ByRef nn As Short) As Boolean

        Dim bRetc As Boolean
        Dim intRegNo As Short
        Dim k As Short
        Dim j As Short

        bRetc = False
        intRegNo = markResistorInfoArray(i).intResNo                                    ' 抵抗番号
        For k = 1 To MaxCntResist
            If (intRegNo = markResistorInfoArray(k).intResNo) Then                      ' 抵抗番号一致
                For j = 1 To MaxCntCut
                    If (intCutNo = markResistorInfoArray(k).ArrCut(j).intCutNo) Then    ' ｶｯﾄ番号一致
                        n = k
                        nn = j
                        bRetc = True
                        GetNGResistorCutAddress = bRetc
                        Exit Function
                    End If
                Next
            End If
        Next
        GetNGResistorCutAddress = bRetc
    End Function
#End If
#End Region

#Region "指定抵抗番号、ｶｯﾄ番号のカット形状取得"
    '''=========================================================================
    '''<summary>指定抵抗番号、ｶｯﾄ番号のカット形状取得</summary>
    '''<param name="intRegNo">(INP) 抵抗番号</param>
    '''<param name="intCutNo">(INP) ｶｯﾄ番号</param>
    '''<returns>カット形状</returns>
    '''=========================================================================
    Public Function GetCutType(ByRef intRegNo As Short, ByRef intCutNo As Short) As String
        On Error GoTo ErrTrap
        GetCutType = typResistorInfoArray(intRegNo).ArrCut(intCutNo).strCutType
        Exit Function

ErrTrap:
        MsgBox("GetCutType() " & DataAccess_001 & vbCrLf & DataAccess_002 & CStr(intRegNo) & vbTab & DataAccess_003 & CStr(intCutNo) & vbCrLf & DataAccess_004 & Err.Number & vbCrLf & DataAccess_005 & Err.Description)
        ' "データ読出しエラー！" & vbCrLf & "抵抗番号= " & CStr(intRegNo) & vbTab & "カット番号= " & CStr(intCutNo) & vbCrLf & "エラーコード：" & Err.Number & vbCrLf & "エラー説明：" & Err.Description)
        GetCutType = CStr(-1)
    End Function
#End Region

#Region "指定抵抗番号、ｶｯﾄ番号のQレート取得"
    '''=========================================================================
    '''<summary>指定抵抗番号、ｶｯﾄ番号のQレート取得</summary>
    '''<param name="intRegNo">(INP) 抵抗番号</param>
    '''<param name="intCutNo">(INP) ｶｯﾄ番号</param>
    '''<returns>Qレート</returns>
    '''=========================================================================
    Public Function GetQSwitchRate(ByRef intRegNo As Short, ByRef intCutNo As Short) As Double
        On Error GoTo ErrTrap
        GetQSwitchRate = typResistorInfoArray(intRegNo).ArrCut(intCutNo).dblQRate
        Exit Function

ErrTrap:
        MsgBox("GetQSwitchRate() " & DataAccess_001 & vbCrLf & DataAccess_002 & CStr(intRegNo) & vbTab & DataAccess_003 & CStr(intCutNo) & vbCrLf & DataAccess_004 & Err.Number & vbCrLf & DataAccess_005 & Err.Description)
        ' "データ読出しエラー！" & vbCrLf & "抵抗番号= " & CStr(intRegNo) & vbTab & "カット番号= " & CStr(intCutNo) & vbCrLf & "エラーコード：" & Err.Number & vbCrLf & "エラー説明：" & Err.Description)
        GetQSwitchRate = -1
    End Function
#End Region

#Region "指定抵抗番号、ｶｯﾄ番号のラダー間距離取得"
    '''=========================================================================
    '''<summary>指定抵抗番号、ｶｯﾄ番号のラダー間距離取得</summary>
    '''<param name="intRegNo">(INP) 抵抗番号</param>
    '''<param name="intCutNo">(INP) ｶｯﾄ番号</param>
    '''<returns>ラダー間距離</returns>
    '''=========================================================================
    Public Function GetLadderDistance(ByRef intRegNo As Short, ByRef intCutNo As Short) As Short

        On Error GoTo ErrTrap
        GetLadderDistance = typResistorInfoArray(intRegNo).ArrCut(intCutNo).intRadderInterval
        Exit Function

ErrTrap:
        MsgBox("GetLadderDistance() " & DataAccess_001 & vbCrLf & DataAccess_002 & CStr(intRegNo) & vbTab & DataAccess_003 & CStr(intCutNo) & vbCrLf & DataAccess_004 & Err.Number & vbCrLf & DataAccess_005 & Err.Description)
        ' "データ読出しエラー！" & vbCrLf & "抵抗番号= " & CStr(intRegNo) & vbTab & "カット番号= " & CStr(intCutNo) & vbCrLf & "エラーコード：" & Err.Number & vbCrLf & "エラー説明：" & Err.Description)
        GetLadderDistance = -1
    End Function

#End Region

#Region "指定抵抗№時に追加するｸﾞﾙｰﾌﾟ間ｲﾝﾀｰﾊﾞﾙ値取得"
    '''=========================================================================
    '''<summary>指定抵抗№時に追加するｸﾞﾙｰﾌﾟ間ｲﾝﾀｰﾊﾞﾙ値取得</summary>
    '''<param name="intBlockNo">(INP) ﾌﾞﾛｯｸ番号</param>
    '''<param name="dblX">      (OUT) ｽﾃｯﾌﾟｲﾝﾀｰﾊﾞﾙ</param>
    '''<returns>TRUE:ﾃﾞｰﾀあり, FALSE:ﾃﾞｰﾀなし</returns>
    '''=========================================================================
    Public Function GetTy2Data(ByRef intBlockNo As Short, ByRef dblX As Double) As Boolean

        Dim bRetc As Boolean
        Dim i As Short
        Dim dblCSx As Double
        Dim dblCSy As Double
        Dim cs As Double
        Dim intCDir As Short

        ' ﾁｯﾌﾟｻｲｽﾞ
        dblCSx = typPlateInfo.dblChipSizeXDir
        dblCSy = typPlateInfo.dblChipSizeYDir

        intCDir = typPlateInfo.intResistDir               ' チップ並び方向取得(CHIP-NETのみ)

        If intCDir = 0 Then
            cs = dblCSx
        Else
            cs = dblCSy
        End If

        bRetc = False
        For i = 0 To 255
            'UPGRADE_WARNING: オブジェクト typTy2InfoArray(i).intTy21 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            If (intBlockNo = typTy2InfoArray(i).intTy21) Then ' ｸﾞﾙｰﾌﾟ番号一致

                'UPGRADE_WARNING: オブジェクト typTy2InfoArray().dblTy22 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                dblX = typTy2InfoArray(i).dblTy22

                bRetc = True
                Exit For

            End If
        Next

        GetTy2Data = bRetc
    End Function
#End Region

    '' '' ''#Region "ﾋﾞｰﾑﾎﾟｼﾞｼｮﾅの範囲をﾁｪｯｸ"
    '' '' ''    '''=========================================================================
    '' '' ''    '''<summary>ﾋﾞｰﾑﾎﾟｼﾞｼｮﾅの範囲をﾁｪｯｸ</summary>
    '' '' ''    '''<remarks></remarks>
    '' '' ''    '''=========================================================================
    '' '' ''    Public Function ChkBP_Area() As Short

    '' '' ''        Dim RNO As Short 'resistor counter
    '' '' ''        Dim Cno As Short 'cut counter
    '' '' ''        Dim bpofx As Double 'BP offset X
    '' '' ''        Dim bpofy As Double 'BP offset Y
    '' '' ''        Dim startpx As Double 'start point X
    '' '' ''        Dim startpy As Double 'start point Y
    '' '' ''        Dim lenxy As Double 'cut length 1
    '' '' ''        Dim dirxy As Short 'cut direc 1
    '' '' ''        Dim lenxy2 As Double 'cut length 2
    '' '' ''        Dim dirxy2 As Short 'cut direc 2
    '' '' ''        Dim ChipNum As Short 'chip number
    '' '' ''        Dim CutNum As Short 'cut number
    '' '' ''        Dim Cdir As Short 'cut direc

    '' '' ''        Dim judge_flg As Short

    '' '' ''        judge_flg = 0

    '' '' ''        ' BP位置ｵﾌｾｯﾄX,Y設定
    '' '' ''        bpofx = typPlateInfo.dblBpOffSetXDir
    '' '' ''        bpofy = typPlateInfo.dblBpOffSetYDir
    '' '' ''        Call GetChipNum(ChipNum)                                            ' ﾁｯﾌﾟ数取得

    '' '' ''        For RNO = 1 To ChipNum
    '' '' ''            ' ｶｯﾄ数取得
    '' '' ''            Call GetRegCutNum(RNO, CutNum)
    '' '' ''            For Cno = 1 To CutNum
    '' '' ''                Call GetCutStartPoint(RNO, Cno, startpx, startpy)           ' ｽﾀｰﾄﾎﾟｲﾝﾄXY取得
    '' '' ''                Cdir = typResistorInfoArray(RNO).ArrCut(Cno).intCutDir      ' ｶｯﾄ方向取得
    '' '' ''                ' カットタイプ情報(Cdir)からカット方向(dirxy, dirxy2)を求めて返す
    '' '' ''                Call Form1.Utility1.sGetCuttypeXY(dirxy, dirxy2, Cdir)              ' ｶｯﾄ方向取得2
    '' '' ''                lenxy = typResistorInfoArray(RNO).ArrCut(Cno).dblMaxCutLength    ' ｶｯﾄ長1取得
    '' '' ''                lenxy2 = typResistorInfoArray(RNO).ArrCut(Cno).dblMaxCutLengthL  ' ｶｯﾄ長2取得

    '' '' ''                ' BP加工範囲ﾁｪｯｸ OcxSystemを使用
    '' '' ''                judge_flg = Form1.System1.BpLimitCheck(gSysPrm, bpofx, bpofy, startpx, startpy, lenxy, dirxy, lenxy2, dirxy2)
    '' '' ''                ' ｴﾗｰならﾁｪｯｸ中止
    '' '' ''                If judge_flg <> 0 Then Exit For
    '' '' ''            Next Cno
    '' '' ''            ' ｴﾗｰならﾁｪｯｸ中止
    '' '' ''            If judge_flg <> 0 Then Exit For
    '' '' ''        Next RNO

    '' '' ''        'return status
    '' '' ''        ChkBP_Area = judge_flg
    '' '' ''    End Function
    '' '' ''#End Region

#Region "ﾋﾞｰﾑﾎﾟｼﾞｼｮﾅのオフセット値の取得"
    '''=========================================================================
    '''<summary>ﾋﾞｰﾑﾎﾟｼﾞｼｮﾅのオフセット値の取得</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub GetBpOffset(ByRef BpOffX As Double, ByRef BpOffY As Double)
        BpOffX = typPlateInfo.dblBpOffSetXDir
        BpOffY = typPlateInfo.dblBpOffSetYDir
    End Sub
#End Region

#Region "ブロックサイズの取得"
    '''=========================================================================
    '''<summary>ブロックサイズの取得</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub GetBlockSize(ByRef BsizeX As Double, ByRef BsizeY As Double)

        BsizeX = typPlateInfo.dblBlockSizeXDir
        BsizeY = typPlateInfo.dblBlockSizeYDir
    End Sub
#End Region

#Region "ﾋﾞｰﾑﾎﾟｼﾞｼｮﾅのオフセット値の設定"
    '''=========================================================================
    '''<summary>ﾋﾞｰﾑﾎﾟｼﾞｼｮﾅのオフセット値の設定</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub SetBpOffset(ByVal BpOffX As Double, ByVal BpOffY As Double)
        Dim strMSG As String        ' ###270

        With typPlateInfo
            If .dblBpOffSetXDir <> 0 And BpOffX = 0 And .dblBpOffSetYDir <> 0 And BpOffY = 0 Then       ' ###270
                strMSG = "SetBpOffset() : BP Offset Update 0 :BpOffX= " + CStr(BpOffX) + " :BpOffY= " + CStr(BpOffY) + " :dblBpOffSetXDir=" + CStr(.dblBpOffSetXDir) + " :dblBpOffSetYDir=" + CStr(.dblBpOffSetYDir)
                MsgBox(strMSG)
                Exit Sub
            End If
            .dblBpOffSetXDir = BpOffX
            .dblBpOffSetYDir = BpOffY

            'INTRIM側のオフセット値も更新する
            Call BPOFF(.dblBpOffSetXDir, .dblBpOffSetYDir)

        End With
    End Sub
#End Region

    'V5.0.0.6⑩未使用なのでコメントアウト↓
    '#Region "NGﾏｰｷﾝｸﾞ用のｽﾀｰﾄﾎﾟｲﾝﾄを再設定する【CHIP,NET用】(未使用)"
    '    '''=========================================================================
    '    '''<summary>NGﾏｰｷﾝｸﾞ用のｽﾀｰﾄﾎﾟｲﾝﾄを再設定する【CHIP,NET用】</summary>
    '    '''<remarks>TrimDataEditorのSetNG_MarkingPos()と同じ処理</remarks>
    '    '''=========================================================================
    '    Public Sub SetNG_MarkingPos()

    '        Dim ChipNum As Short                                    ' 抵抗数(ﾁｯﾌﾟ数)
    '        Dim CirNum As Short                                     ' サーキット番号
    '        Dim rn As Short                                         ' 抵抗番号
    '        Dim cn As Short                                         ' カット番号 
    '        Dim strMSG As String

    '        Try
    '            ' NGﾏｰｷﾝｸﾞのﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄ/ｽﾀｰﾄﾎﾟｲﾝﾄを算出し、ﾊﾞｯﾌｧに格納する
    '            If (gTkyKnd = KND_TKY) Then Exit Sub
    '            Call GetChipNum(ChipNum)                                ' 抵抗数取得
    '            CirNum = 1                                              ' サーキット番号

    '            For rn = 1 To ChipNum                                   ' 抵抗数分ﾙｰﾌﾟ

    '                ' マーキング用バッファにマーキングデータ(1000番)を設定する(CHIP/NET)
    '                Call CopyResDataToMarkData(rn)

    '                ' マーキング用データのｽﾀｰﾄﾎﾟｲﾝﾄX,YとﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄX,Yを設定する
    '                For cn = 1 To markResistorInfoArray(rn).intCutCount ' カット数分ﾙｰﾌﾟ

    '                    ' ﾏｰｷﾝｸﾞﾃﾞｰﾀのｽﾀｰﾄﾎﾟｲﾝﾄX,Yを抵抗データとﾏｰｷﾝｸﾞ用ﾊﾞｯﾌｧのﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄX,Yに設定する
    '                    typResistorInfoArray(1000).ArrCut(cn).dblTeachPointX = typResistorInfoArray(1000).ArrCut(cn).dblStartPointX
    '                    typResistorInfoArray(1000).ArrCut(cn).dblTeachPointY = typResistorInfoArray(1000).ArrCut(cn).dblStartPointY
    '                    markResistorInfoArray(1000).ArrCut(cn).dblTeachPointX = typResistorInfoArray(1000).ArrCut(cn).dblStartPointX
    '                    markResistorInfoArray(1000).ArrCut(cn).dblTeachPointY = typResistorInfoArray(1000).ArrCut(cn).dblStartPointY

    '                    ' NGﾏｰｷﾝｸﾞﾎﾟｲﾝﾄを算出しﾏｰｷﾝｸﾞ用ﾊﾞｯﾌｧのｽﾀｰﾄﾎﾟｲﾝﾄX,Yに設定する
    '                    If (gTkyKnd = KND_CHIP) Then
    '                        ' CHIP時
    '                        If (typPlateInfo.intResistDir = 0) Then     ' 抵抗(ﾁｯﾌﾟ)並び方向(0:X, 1:Y)を考慮 
    '                            markResistorInfoArray(rn).ArrCut(cn).dblStartPointX = typResistorInfoArray(1000).ArrCut(cn).dblStartPointX + typPlateInfo.dblChipSizeXDir * (rn - 1)
    '                            markResistorInfoArray(rn).ArrCut(cn).dblStartPointY = typResistorInfoArray(1000).ArrCut(cn).dblStartPointY
    '                        Else
    '                            markResistorInfoArray(rn).ArrCut(cn).dblStartPointX = typResistorInfoArray(1000).ArrCut(cn).dblStartPointX
    '                            markResistorInfoArray(rn).ArrCut(cn).dblStartPointY = typResistorInfoArray(1000).ArrCut(cn).dblStartPointY + typPlateInfo.dblChipSizeYDir * (rn - 1)
    '                        End If

    '                        ' NET時
    '                    Else
    '                        ' サーキット番号が変わった ?
    '                        If (markResistorInfoArray(rn).intCircuitGrp <> CirNum) Then
    '                            CirNum = CirNum + 1                     ' サーキット番号更新
    '                        End If
    '                        ' サーキット座標データ(NET用)の座標データを加算
    '                        markResistorInfoArray(rn).ArrCut(cn).dblStartPointX = typResistorInfoArray(1000).ArrCut(cn).dblStartPointX + typCirAxisInfoArray(CirNum).dblCaP2
    '                        markResistorInfoArray(rn).ArrCut(cn).dblStartPointY = typResistorInfoArray(1000).ArrCut(cn).dblStartPointY + typCirAxisInfoArray(CirNum).dblCaP3
    '                    End If

    '                    ' マーキング用データのﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄにｽﾀｰﾄﾎﾟｲﾝﾄを設定する
    '                    markResistorInfoArray(rn).ArrCut(cn).dblTeachPointX = markResistorInfoArray(rn).ArrCut(cn).dblStartPointX
    '                    markResistorInfoArray(rn).ArrCut(cn).dblTeachPointY = markResistorInfoArray(rn).ArrCut(cn).dblStartPointY

    '                Next cn
    '            Next rn

    '            ' トラップエラー発生時 
    '        Catch ex As Exception
    '            strMSG = "DataAccess.SetNG_MarkingPos() TRAP ERROR = " + ex.Message
    '            MsgBox(strMSG)
    '        End Try

    '    End Sub
    '#End Region
    'V5.0.0.6⑩未使用なのでコメントアウト↑

#Region "マーキング用バッファにマーキングデータ(1000番)を設定する((CHIP/NET)(未使用)"
#If False Then  'V5.0.0.8①
    '''=========================================================================
    '''<summary>マーキング用バッファにマーキングデータ(1000番)を設定する((CHIP/NET)</summary>
    '''<param name="rn">(INP) 抵抗番号</param>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub CopyResDataToMarkData(ByVal rn As Integer)

        Dim cn As Integer

        'For rn = 1 To MaxCntResist
        markResistorInfoArray(rn).intResNo = typResistorInfoArray(1000).intResNo                                  ' 抵抗番号
        markResistorInfoArray(rn).intResMeasMode = typPlateInfo.intMeasType                                       ' 測定モード(0:抵抗 ,1:電圧 ,2:外部) ※変更
        markResistorInfoArray(rn).intResMeasType = typResistorInfoArray(1000).intResMeasType                      ' 測定タイプ(0:高速 ,1:高精度)　※追加
        markResistorInfoArray(rn).intCircuitGrp = typResistorInfoArray(1000).intCircuitGrp                        ' 所属ｻｰｷｯﾄ
        markResistorInfoArray(rn).intCircuitGrp = typResistorInfoArray(rn).intCircuitGrp                          ' 所属ｻｰｷｯﾄを再設定
        markResistorInfoArray(rn).intProbHiNo = typResistorInfoArray(1000).intProbHiNo                            ' ﾌﾟﾛｰﾌﾞ番号(HI側)
        markResistorInfoArray(rn).intProbLoNo = typResistorInfoArray(1000).intProbLoNo                            ' ﾌﾟﾛｰﾌﾞ番号(LO側)
        markResistorInfoArray(rn).intProbAGNo1 = typResistorInfoArray(1000).intProbAGNo1                          ' ﾌﾟﾛｰﾌﾞ番号(1)
        markResistorInfoArray(rn).intProbAGNo2 = typResistorInfoArray(1000).intProbAGNo2                          ' ﾌﾟﾛｰﾌﾞ番号(2)
        markResistorInfoArray(rn).intProbAGNo3 = typResistorInfoArray(1000).intProbAGNo3                          ' ﾌﾟﾛｰﾌﾞ番号(3)
        markResistorInfoArray(rn).intProbAGNo4 = typResistorInfoArray(1000).intProbAGNo4                          ' ﾌﾟﾛｰﾌﾞ番号(4)
        markResistorInfoArray(rn).intProbAGNo5 = typResistorInfoArray(1000).intProbAGNo5                          ' ﾌﾟﾛｰﾌﾞ番号(5)
        markResistorInfoArray(rn).strExternalBits = typResistorInfoArray(1000).strExternalBits                    ' EXTERNAL BITS
        markResistorInfoArray(rn).intPauseTime = typResistorInfoArray(1000).intPauseTime                          ' ﾎﾟｰｽﾞﾀｲﾑ
        markResistorInfoArray(rn).intTargetValType = typResistorInfoArray(1000).intTargetValType                  ' 目標値指定
        markResistorInfoArray(rn).intBaseResNo = typResistorInfoArray(1000).intBaseResNo                          ' ﾍﾞｰｽ抵抗番号
        markResistorInfoArray(rn).dblTrimTargetVal = typResistorInfoArray(1000).dblTrimTargetVal                  ' ﾄﾘﾐﾝｸﾞ目標値
        markResistorInfoArray(rn).strRatioTrimTargetVal = typResistorInfoArray(1000).strRatioTrimTargetVal        ' トリミング目標値(レシオ計算式) 
        markResistorInfoArray(rn).dblDeltaR = typResistorInfoArray(1000).dblDeltaR                                ' ΔＲ
        markResistorInfoArray(rn).intSlope = typResistorInfoArray(1000).intSlope                                  ' 電圧変化ｽﾛｰﾌﾟ
        markResistorInfoArray(rn).dblCutOffRatio = typResistorInfoArray(1000).dblCutOffRatio                      ' 切り上げ倍率
        markResistorInfoArray(rn).dblProbCfmPoint_Hi_X = typResistorInfoArray(1000).dblProbCfmPoint_Hi_X          ' プローブ確認位置 HI X座標
        markResistorInfoArray(rn).dblProbCfmPoint_Hi_Y = typResistorInfoArray(1000).dblProbCfmPoint_Hi_Y          ' プローブ確認位置 HI Y座標
        markResistorInfoArray(rn).dblProbCfmPoint_Lo_X = typResistorInfoArray(1000).dblProbCfmPoint_Lo_X          ' プローブ確認位置 LO X座標
        markResistorInfoArray(rn).dblProbCfmPoint_Lo_Y = typResistorInfoArray(1000).dblProbCfmPoint_Lo_Y          ' プローブ確認位置 LO Y座標
        markResistorInfoArray(rn).dblInitTest_HighLimit = typResistorInfoArray(1000).dblInitTest_HighLimit        ' ｲﾆｼｬﾙﾃｽﾄHIGHﾘﾐｯﾄ
        markResistorInfoArray(rn).dblInitTest_LowLimit = typResistorInfoArray(1000).dblInitTest_LowLimit          ' ｲﾆｼｬﾙﾃｽﾄLOWﾘﾐｯﾄ
        markResistorInfoArray(rn).dblFinalTest_HighLimit = typResistorInfoArray(1000).dblFinalTest_HighLimit      ' ﾌｧｲﾅﾙﾃｽﾄHIGHﾘﾐｯﾄ
        markResistorInfoArray(rn).dblFinalTest_LowLimit = typResistorInfoArray(1000).dblFinalTest_LowLimit        ' ﾌｧｲﾅﾙﾃｽﾄLOWﾘﾐｯﾄ
        markResistorInfoArray(rn).dblInitOKTest_HighLimit = typResistorInfoArray(1000).dblInitOKTest_HighLimit    ' ｲﾆｼｬﾙOKﾃｽﾄHIGHﾘﾐｯﾄ
        markResistorInfoArray(rn).dblInitOKTest_LowLimit = typResistorInfoArray(1000).dblInitOKTest_LowLimit      ' ｲﾆｼｬﾙOKﾃｽﾄLOWﾘﾐｯﾄ
        markResistorInfoArray(rn).intCutCount = typResistorInfoArray(1000).intCutCount                            ' ｶｯﾄ数
        markResistorInfoArray(rn).intCutReviseMode = typResistorInfoArray(1000).intCutReviseMode                  ' ｶｯﾄ 補正
        markResistorInfoArray(rn).intCutReviseDispMode = typResistorInfoArray(1000).intCutReviseDispMode          ' 表示ﾓｰﾄﾞ
        markResistorInfoArray(rn).intCutRevisePtnNo = typResistorInfoArray(1000).intCutRevisePtnNo                ' ﾊﾟﾀｰﾝ No.
        markResistorInfoArray(rn).dblCutRevisePosX = typResistorInfoArray(1000).dblCutRevisePosX                  ' ｶｯﾄ補正位置X
        markResistorInfoArray(rn).dblCutRevisePosY = typResistorInfoArray(1000).dblCutRevisePosY                  ' ｶｯﾄ補正位置Y
        markResistorInfoArray(rn).intIsNG = typResistorInfoArray(1000).intIsNG                                    ' NG有無

        ' ｶｯﾄﾃﾞｰﾀ構造体をコピー
        For cn = 1 To MaxCutInfo
            markResistorInfoArray(rn).ArrCut(cn).intCutNo = typResistorInfoArray(1000).ArrCut(cn).intCutNo                            ' ｶｯﾄ番号
            markResistorInfoArray(rn).ArrCut(cn).intDelayTime = typResistorInfoArray(1000).ArrCut(cn).intDelayTime                    ' ﾃﾞｨﾚｲﾀｲﾑ
            markResistorInfoArray(rn).ArrCut(cn).strCutType = typResistorInfoArray(1000).ArrCut(cn).strCutType                        ' ｶｯﾄ形状
            markResistorInfoArray(rn).ArrCut(cn).dblTeachPointX = typResistorInfoArray(1000).ArrCut(cn).dblTeachPointX                ' ﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄX
            markResistorInfoArray(rn).ArrCut(cn).dblTeachPointY = typResistorInfoArray(1000).ArrCut(cn).dblTeachPointY                ' ﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄY
            markResistorInfoArray(rn).ArrCut(cn).dblStartPointX = typResistorInfoArray(1000).ArrCut(cn).dblStartPointX                ' ｽﾀｰﾄﾎﾟｲﾝﾄX
            markResistorInfoArray(rn).ArrCut(cn).dblStartPointY = typResistorInfoArray(1000).ArrCut(cn).dblStartPointY                ' ｽﾀｰﾄﾎﾟｲﾝﾄY
            markResistorInfoArray(rn).ArrCut(cn).dblCutSpeed = typResistorInfoArray(1000).ArrCut(cn).dblCutSpeed                      ' ｶｯﾄｽﾋﾟｰﾄﾞ
            markResistorInfoArray(rn).ArrCut(cn).dblQRate = typResistorInfoArray(1000).ArrCut(cn).dblQRate                            ' Qｽｲｯﾁﾚｰﾄ
            markResistorInfoArray(rn).ArrCut(cn).dblCutOff = typResistorInfoArray(1000).ArrCut(cn).dblCutOff                          ' ｶｯﾄｵﾌ値
            markResistorInfoArray(rn).ArrCut(cn).dblJudgeLevel = typResistorInfoArray(1000).ArrCut(cn).dblJudgeLevel                  ' 切替ポイント (旧ﾃﾞｰﾀ判定(平均化率))
            markResistorInfoArray(rn).ArrCut(cn).dblCutOffOffset = typResistorInfoArray(1000).ArrCut(cn).dblCutOffOffset              ' ｶｯﾄｵﾌｵﾌｾｯﾄ
            markResistorInfoArray(rn).ArrCut(cn).intPulseWidthCtrl = typResistorInfoArray(1000).ArrCut(cn).intPulseWidthCtrl          ' ﾊﾟﾙｽ幅制御
            markResistorInfoArray(rn).ArrCut(cn).dblPulseWidthTime = typResistorInfoArray(1000).ArrCut(cn).dblPulseWidthTime          ' ﾊﾟﾙｽ幅時間
            markResistorInfoArray(rn).ArrCut(cn).dblLSwPulseWidthTime = typResistorInfoArray(1000).ArrCut(cn).dblLSwPulseWidthTime    ' LSwﾊﾟﾙｽ幅時間
            markResistorInfoArray(rn).ArrCut(cn).intCutDir = typResistorInfoArray(1000).ArrCut(cn).intCutDir                          ' ｶｯﾄ方向
            markResistorInfoArray(rn).ArrCut(cn).dblMaxCutLength = typResistorInfoArray(1000).ArrCut(cn).dblMaxCutLength              ' 最大ｶｯﾃｨﾝｸﾞ長
            markResistorInfoArray(rn).ArrCut(cn).dblLTurnPoint = typResistorInfoArray(1000).ArrCut(cn).dblLTurnPoint                  ' Lﾀｰﾝﾎﾟｲﾝﾄ
            markResistorInfoArray(rn).ArrCut(cn).dblMaxCutLengthL = typResistorInfoArray(1000).ArrCut(cn).dblMaxCutLengthL            ' Lﾀｰﾝ後の最大ｶｯﾃｨﾝｸﾞ長
            markResistorInfoArray(rn).ArrCut(cn).dblMaxCutLengthHook = typResistorInfoArray(1000).ArrCut(cn).dblMaxCutLengthHook      ' ﾌｯｸﾀｰﾝ後のｶｯﾃｨﾝｸﾞ長
            markResistorInfoArray(rn).ArrCut(cn).dblR1 = typResistorInfoArray(1000).ArrCut(cn).dblR1                                  ' R1
            markResistorInfoArray(rn).ArrCut(cn).dblR2 = typResistorInfoArray(1000).ArrCut(cn).dblR2                                  ' R2
            markResistorInfoArray(rn).ArrCut(cn).intCutAngle = typResistorInfoArray(1000).ArrCut(cn).intCutAngle                      ' 斜めｶｯﾄの切り出し角度
            markResistorInfoArray(rn).ArrCut(cn).dblCutSpeed2 = typResistorInfoArray(1000).ArrCut(cn).dblCutSpeed2                    ' ｶｯﾄｽﾋﾟｰﾄﾞ2
            markResistorInfoArray(rn).ArrCut(cn).dblQRate2 = typResistorInfoArray(1000).ArrCut(cn).dblQRate2                          ' Qｽｲｯﾁﾚｰﾄ2
            ''''    ↓436Kのパラメータ。432のINTRTM側では構造体定義ない為、一旦削除
            ''''markResistorInfoArray(rn).ArrCut(cn).dblCP53 = typResistorInfoArray(1000).ArrCut(cn).dblCP53                          ' Qｽｲｯﾁﾚｰﾄ3
            ''''markResistorInfoArray(rn).ArrCut(cn).dblCP54 = typResistorInfoArray(1000).ArrCut(cn).dblCP54                          ' 切替えﾎﾟｲﾝﾄ
            markResistorInfoArray(rn).ArrCut(cn).dblESPoint = typResistorInfoArray(1000).ArrCut(cn).dblESPoint                        ' ｴｯｼﾞｾﾝｽﾎﾟｲﾝﾄ
            markResistorInfoArray(rn).ArrCut(cn).dblESJudgeLevel = typResistorInfoArray(1000).ArrCut(cn).dblESJudgeLevel              ' ｴｯｼﾞｾﾝｽの判定変化率
            markResistorInfoArray(rn).ArrCut(cn).dblMaxCutLengthES = typResistorInfoArray(1000).ArrCut(cn).dblMaxCutLengthES          ' ｴｯｼﾞｾﾝｽ後のｶｯﾄ長
            markResistorInfoArray(rn).ArrCut(cn).intIndexCnt = typResistorInfoArray(1000).ArrCut(cn).intIndexCnt                      ' ｲﾝﾃﾞｯｸｽ数
            markResistorInfoArray(rn).ArrCut(cn).intMeasMode = typResistorInfoArray(1000).ArrCut(cn).intMeasMode                      ' 測定ﾓｰﾄﾞ
            markResistorInfoArray(rn).ArrCut(cn).dblPitch = typResistorInfoArray(1000).ArrCut(cn).dblPitch                            ' ﾋﾟｯﾁ
            markResistorInfoArray(rn).ArrCut(cn).intStepDir = typResistorInfoArray(1000).ArrCut(cn).intStepDir                        ' ｽﾃｯﾌﾟ方向
            markResistorInfoArray(rn).ArrCut(cn).intCutCnt = typResistorInfoArray(1000).ArrCut(cn).intCutCnt                          ' 本数
            markResistorInfoArray(rn).ArrCut(cn).dblUCutDummy1 = typResistorInfoArray(1000).ArrCut(cn).dblUCutDummy1                  ' Uｶｯﾄ用ﾀﾞﾐｰ
            markResistorInfoArray(rn).ArrCut(cn).dblUCutDummy2 = typResistorInfoArray(1000).ArrCut(cn).dblUCutDummy2                  ' Uｶｯﾄ用ﾀﾞﾐｰ
            markResistorInfoArray(rn).ArrCut(cn).dblESChangeRatio = typResistorInfoArray(1000).ArrCut(cn).dblESChangeRatio            ' ｴｯｼﾞｾﾝｽ後の変化率
            markResistorInfoArray(rn).ArrCut(cn).intESConfirmCnt = typResistorInfoArray(1000).ArrCut(cn).intESConfirmCnt              ' ｴｯｼﾞｾﾝｽ後の確認回数
            markResistorInfoArray(rn).ArrCut(cn).intRadderInterval = typResistorInfoArray(1000).ArrCut(cn).intRadderInterval          ' ﾗﾀﾞｰ間距離
            markResistorInfoArray(rn).ArrCut(cn).dblZoom = typResistorInfoArray(1000).ArrCut(cn).dblZoom                              ' 倍率
            markResistorInfoArray(rn).ArrCut(cn).strChar = typResistorInfoArray(1000).ArrCut(cn).strChar                              ' 文字列
        Next cn
        'Next rn

    End Sub
#End If
#End Region

#Region "データ編集終了時の初期化処理(NET)(未使用)"
    '''=========================================================================
    '''<summary>データ編集終了時の初期化処理(NET)</summary>
    '''<remarks>NETにのみ存在するコード</remarks>
    '''=========================================================================
    Public Sub DataEditExitInit()

        Dim iArray As Short
        Dim iResMax As Short

        ' トリムモード→測定種別＝抵抗の場合
        If typPlateInfo.intMeasType = 0 Then

            ' 最大抵抗数を取得
            iResMax = typPlateInfo.intResistCntInGroup * typPlateInfo.intCircuitCntInBlock
            ' 電圧スロープは入力不可なので初期化しておく
            For iArray = 1 To iResMax
                typResistorInfoArray(iArray).intSlope = 0
            Next
        End If

    End Sub
#End Region

    '#Region "ﾌﾞﾛｯｸｻｲｽﾞを算出する"
    '    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '    ''''(2010/11/09)
    '    ''''    ブロックサイズは編集時、もしくはBpPosADJとStagePosADJ実行時に設定
    '    ''''    ブロックサイズX = 抵抗数×（X方向チップサイズ）
    '    ''''    ブロックサイズY = 抵抗数×（Y方向チップサイズ）
    '    ''''
    '    ''''    下記の関数は最終的には削除する。
    '    ''''    TXでも設定するため削除しない
    '    ''''    
    '    ''''    ブロックサイズ
    '    ''''    (チップ並びがX方向の場合）
    '    ''''    ブロックサイズX = (抵抗数(チップ数) × X方向チップサイズ + BPグループ間隔) × BPグループ数
    '    ''''    ブロックサイズY = Y方向チップサイズ
    '    ''''    (チップ並びがY方向の場合）
    '    ''''    ブロックサイズX = X方向チップサイズ
    '    ''''    ブロックサイズY = (抵抗数(チップ数) × Y方向チップサイズ + BPグループ間隔) × BPグループ数
    '    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '    '''=========================================================================
    '    '''<summary>ﾌﾞﾛｯｸｻｲｽﾞを算出する</summary>
    '    '''<param name="dblBSX">(OUT) ﾌﾞﾛｯｸｻｲｽﾞX</param>
    '    '''<param name="dblBSY">(OUT) ﾌﾞﾛｯｸｻｲｽﾞY</param>
    '    '''=========================================================================
    '    Public Sub CalcBlockSize(ByRef dblBSX As Double, ByRef dblBSY As Double)

    '        Dim dblCSx As Double
    '        Dim dblCSy As Double
    '        Dim dblGIx As Double
    '        Dim dblGIy As Double
    '        Dim intGNx As Short
    '        Dim intGNY As Short
    '        Dim intCDir As Short
    '        Dim intChipNum As Short
    '        Dim i As Short
    '        Dim ResNum As Short
    '        Dim GrpIntv As Double
    '        Dim dData As Double

    '        Try
    '            ' NET時
    '            If (gTkyKnd = KND_NET) Then
    '                ' ﾌﾞﾛｯｸｻｲｽﾞ補正X,Y
    '                dblBSX = typPlateInfo.dblBlockSizeReviseXDir
    '                dblBSY = typPlateInfo.dblBlockSizeReviseYDir
    '                Exit Sub
    '            End If

    '            ' グループ数X,Y
    '            intGNx = typPlateInfo.intGroupCntInBlockXBp
    '            intGNY = typPlateInfo.intGroupCntInBlockYStage
    '            Call GetChipNum(intChipNum)                         ' グループ内チップ数

    '            ' ﾁｯﾌﾟｻｲｽﾞ
    '            dblCSx = typPlateInfo.dblChipSizeXDir
    '            dblCSy = typPlateInfo.dblChipSizeYDir

    '            dblGIx = typPlateInfo.dblBpGrpItv                   ' BPグループインターバル
    '            dblGIy = typPlateInfo.dblStgGrpItvY                 ' Stageグループインターバル
    '            '            dblGIx = typPlateInfo.dblGroupItvXDir  ' グループインターバル
    '            '            dblGIy = typPlateInfo.dblGroupItvYDir
    '            intCDir = typPlateInfo.intResistDir                 ' チップ並び方向取得(CHIP-NETのみ)

    '            dData = 0.0#
    '            If intCDir = 0 Then                                 ' チップ並び方向 = X方向 ?
    '                If intGNx = 1 Then
    '                    '1ｸﾞﾙｰﾌﾟ
    '                    dData = dblCSx * intChipNum                 ' チップサイズX * チップ数
    '                Else
    '                    '複数ｸﾞﾙｰﾌﾟ
    '                    For i = 1 To intGNx
    '                        'ｸﾞﾙｰﾌﾟﾃﾞｰﾀ参照
    '                        Call GetGrpData(i, ResNum, GrpIntv)
    '                        If i = intGNx Then
    '                            dData = dData + (dblCSx * (ResNum - 1))
    '                        Else
    '                            dData = dData + ((dblCSx * (ResNum - 1)) + GrpIntv)
    '                        End If
    '                    Next i
    '                End If
    '                dblBSX = dData
    '                dblBSY = dblCSy

    '            Else
    '                ' チップ並び方向 = Y方向時
    '                If intGNY = 1 Then
    '                    '1ｸﾞﾙｰﾌﾟ
    '                    dData = dblCSy * intChipNum
    '                Else
    '                    '複数ｸﾞﾙｰﾌﾟ
    '                    For i = 1 To intGNY
    '                        'ｸﾞﾙｰﾌﾟﾃﾞｰﾀ参照
    '                        Call GetGrpData(i, ResNum, GrpIntv)
    '                        If i = intGNx Then
    '                            dData = dData + (dblCSy * (ResNum - 1))
    '                        Else
    '                            dData = dData + ((dblCSy * (ResNum - 1)) + GrpIntv)
    '                        End If
    '                    Next i
    '                End If
    '                dblBSX = dblCSx
    '                dblBSY = dData
    '            End If

    '            ' トラップエラー発生時 
    '        Catch ex As Exception
    '            Dim strMSG As String
    '            strMSG = "DataAccess.CalcBlockSize() TRAP ERROR = " + ex.Message
    '            MsgBox(strMSG)
    '        End Try

    '    End Sub

    '#End Region

    ' V4.12.0.0①　↓　'V6.1.2.0②
    ''' <summary>
    ''' Yステップの逆転判定
    ''' </summary>
    ''' <returns>true:逆転する、False：逆転しない</returns>
    ''' <remarks></remarks>
    Public Function JudgeStepYInverse() As Boolean

        JudgeStepYInverse = False

        ' Yステップが選択されている場合に、奇数、偶数によってステップを反転する
        If (iInverseStepY = 1) And ((typPlateInfo.intDirStepRepeat = STEP_RPT_Y) Or (typPlateInfo.intDirStepRepeat = STEP_RPT_CHIPXSTPY)) Then
            JudgeStepYInverse = True
        End If

        Return JudgeStepYInverse

    End Function
    ' V4.12.0.0① ↑　'V6.1.2.0②

End Module

#Region "トリミングデータ構造体をバックアップ・復元する"
''' <summary>トリミングデータ構造体をバックアップ・復元する</summary>
''' <remarks>'#4.12.2.0⑤</remarks>
Public Class Temporary
    Private _gBlkStagePosX() As Double          ' ブロックのプレート内のステージX位置座標データ
    Private _gBlkStagePosY() As Double          ' ブロックのプレート内のステージY位置座標データ
    Private _gPltStagePosX() As Double          ' プレートのステージX位置座標データ
    Private _gPltStagePosY() As Double          ' プレートのステージY位置座標データ

    Private Shared _instance As Temporary

    ''' <summary>トリミングデータ構造体をバックアップする</summary>
    Public Shared Sub Backup()
        _instance = New Temporary()
        DataManager.Backup()            '#5.0.0.8①

        With _instance
            ._gPltStagePosX = DataAccess.gPltStagePosX.Clone()
            ._gPltStagePosY = DataAccess.gPltStagePosY.Clone()
            ._gBlkStagePosX = DataAccess.gBlkStagePosX.Clone()
            ._gBlkStagePosY = DataAccess.gBlkStagePosY.Clone()
        End With
    End Sub

    ''' <summary>バックアップしたトリミングデータ構造体を復元する</summary>
    Public Shared Sub Restore()
        If (_instance IsNot Nothing) Then
            DataManager.Restore()       '#5.0.0.8①

            With _instance
                DataAccess.gPltStagePosX = ._gPltStagePosX.Clone()
                DataAccess.gPltStagePosY = ._gPltStagePosY.Clone()
                DataAccess.gBlkStagePosX = ._gBlkStagePosX.Clone()
                DataAccess.gBlkStagePosY = ._gBlkStagePosY.Clone()
            End With

            _instance = Nothing
        End If
    End Sub

End Class


#End Region
