Imports System.IO                       'V4.4.0.0-0
Imports System.Text                     'V4.4.0.0-0
Imports LaserFront.Trimmer.DefTrimFnc
Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8①
Imports TrimClassLibrary                'V6.0.0.0①
Imports TKY_ALL_SL432HW.My.Resources    'V4.4.0.0-0

Public Class MainModules
    : Implements IMainModules           'V6.0.0.0①

    '----- V1.13.0.0③↓ -----
    '----- オートプローブ実行用構造体 -----
    Public Structure AutoProbe_Info                         ' オートプローブ実行用構造体形式定義
        Dim intAProbeGroupNo1 As Short                      ' パターン1(下方)用グループ番号
        Dim intAProbePtnNo1 As Short                        ' パターン1(下方)用パターン番号
        Dim intAProbeGroupNo2 As Short                      ' パターン2(上方)用グループ番号
        Dim intAProbePtnNo2 As Short                        ' パターン2(上方)用パターン番号
        Dim dblAProbeBpPosX As Double                       ' パターン1(下方)用BP位置X
        Dim dblAProbeBpPosY As Double                       ' パターン1(下方)用BP位置Y
        Dim dblAProbeStgPosX As Double                      ' パターン2(上方)用ステージオフセット位置X
        Dim dblAProbeStgPosY As Double                      ' パターン2(下方)用ステージオフセット位置Y
        Dim intAProbeStepCount As Short                     ' ステップ実行用ステップ回数
        Dim intAProbeStepCount2 As Short                    ' ステップ実行用繰返しステップ回数
        Dim dblAProbePitch As Double                        ' ステップ実行用ステップピッチ
        Dim dblAProbePitch2 As Double                       ' ステップ実行用繰返しステップピッチ
    End Structure
    Public stAPRB As AutoProbe_Info
    '----- V1.13.0.0③↑ -----

    '----- V1.23.0.0⑦↓ -----
    ' プローブチェック機能用トリミング結果構造体形式定義
    Public Structure ProbeChk_Info
        Dim FinalTest(,) As Double

        '構造体の初期化
        Public Sub Initialize(ByVal RegCount As Integer)
            ReDim FinalTest(0 To 2, 0 To RegCount)          ' ※ 抵抗数分+1の領域確保する 
        End Sub
    End Structure

    ' その他
    Public stPrbChk As ProbeChk_Info = Nothing              ' プローブチェック機能用トリミング結果構造体 
    Public strPrbChkLogFile As String = ""                  ' プローブチェックログファイル名
    Public strPrbChkLogDate As String                       ' ログ日付(YYYY/MM/DDhh:mm:ss) 
    Public strPrbChkFileLog As String                       ' ファイル出力用ログエリア 
    Public strPrbChkDispLog As String                       ' 表示用ログエリア 

    '----- V1.23.0.0⑦↑ -----

#Region "ガベージコレクタにメモリを開放させる"
    '''=========================================================================
    '''<summary>ガベージコレクタにメモリを開放させる</summary>
    '''=========================================================================
    Public Sub ReleaseMemory()
        GC.Collect()
    End Sub


#End Region

#Region "アプリケーション種別を返す(OCX用)"
    '''=========================================================================
    ''' <summary>アプリケーション種別を返す</summary>
    ''' <param name="AppKind">0=TKY, 1=CHIP, 2=NET</param>
    '''=========================================================================
    Public Sub GetAppKind(ByRef AppKind As Short) Implements IMainModules.GetAppKind    'V6.0.0.0①
        AppKind = gTkyKnd
    End Sub
#End Region

#Region "抵抗(チップ)並び方向を返す(OCX用)"
    '''=========================================================================
    ''' <summary>抵抗(チップ)並び方向を返す</summary>
    ''' <param name="ResistDir">0=X方向, 1=Y方向</param>
    '''=========================================================================
    Public Sub GetResistDir(ByRef ResistDir As Short) Implements IMainModules.GetResistDir  'V6.0.0.0①
        ResistDir = typPlateInfo.intResistDir
    End Sub
#End Region

#Region "プレート内ブロックのX方向、Y方向の開始位置算出(OCX用)"
    '''=========================================================================
    ''' <summary>プレート内ブロックのX方向、Y方向の開始位置算出</summary>
    ''' <returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Public Function Call_CalcBlockXYStartPos() As Integer _
        Implements IMainModules.Call_CalcBlockXYStartPos  'V6.0.0.0①

        Dim r As Integer

        r = CalcBlockXYStartPos()
        Return (r)

    End Function
#End Region

#Region "指定ブロックXYからステージ位置XYを取得しテーブル移動する(OCX用)"
    '''=========================================================================
    ''' <summary>指定ブロックXYからステージ位置XYを取得しテーブル移動する</summary>
    ''' <param name="xBlockNo">(INP)ブロック番号X</param>
    ''' <param name="yBlockNo">(INP)ブロック番号Y</param>
    ''' <param name="OffSetX"> (INP)オフセットX</param>
    ''' <param name="OffSetY"> (INP)オフセットY</param>
    ''' <param name="stgx">    (OUT)ステージ位置X</param>
    ''' <param name="stgy">    (OUT)ステージ位置Y</param>
    ''' <returns>0=正常, 0以外=エラー</returns>
    ''' <remarks>Call_CalcBlockXYStartPos()の後にCallする事</remarks>
    '''=========================================================================
    Public Function Call_GetTargetStagePosByXY(ByVal xBlockNo As Integer, ByVal yBlockNo As Integer,
                                               ByVal OffSetX As Double, ByVal OffSetY As Double,
                                               ByRef stgx As Double, ByRef stgy As Double) As Integer _
                                           Implements IMainModules.Call_GetTargetStagePosByXY       'V6.0.0.0①

        Dim r As Integer
        Dim PosX As Double
        Dim PosY As Double
        Dim AddSubPosX As Double = 0.0                                  ' V1.22.0.0⑥
        Dim AddSubPosY As Double = 0.0                                  ' V1.22.0.0⑥

        '----- ###249↓ -----
        r = GetTargetStagePosByXY(xBlockNo, yBlockNo, stgx, stgy)
        '' ステージ移動
        'Form1.System1.EX_START(gSysPrm, stgx + OffSetX, stgy + OffSetY, 0)

        '----- V1.22.0.0⑥↓ -----
        ' ステップオフセットを反映 
        '----- V1.20.0.0④↓ -----
        r = GetStepOffSetPos(1, 1, AddSubPosX, AddSubPosY, xBlockNo, yBlockNo)
        PosX = stgx + OffSetX + AddSubPosX
        PosY = stgy + OffSetY + AddSubPosY

        ' ※EX_START()でBP基準コーナーを考慮しているので下記は不要(外部カメラティーチングは必要)
        ' BP基準コーナーを考慮
        If (giAppMode = APP_MODE_EXCAM_R1TEACH) Or (giAppMode = APP_MODE_EXCAM_TEACH) Then
            Select Case gSysPrm.stDEV.giBpDirXy                         ' V1.22.0.0⑥
                Case 0 ' 右上(x←, y↓)
                    PosX = stgx + OffSetX + AddSubPosX
                    PosY = stgy + OffSetY + AddSubPosY
                Case 1 ' 左上(x→, y↓)
                    PosX = stgx + (OffSetX * (-1)) + AddSubPosX
                    PosY = stgy + OffSetY + AddSubPosY
                Case 2 ' 右下(x←, y↑)
                    PosX = stgx + OffSetX + AddSubPosX
                    PosY = stgy + (OffSetY * (-1)) + AddSubPosY
                Case 3 ' 左下(x→, y↑)
                    PosX = stgx + (OffSetX * (-1)) + AddSubPosX
                    PosY = stgy + (OffSetY * (-1)) + AddSubPosY
            End Select

            '----- V1.24.0.0①↓ -----
            If (gSysPrm.stDEV.giBpSize = BPSIZE_6060) Then
                PosX = PosX - BSZ_6060_OFSX
                PosY = PosY - BSZ_6060_OFSY
            End If
            '----- V1.24.0.0①↑ -----
        End If
        '----- V1.20.0.0④↑ -----
        '----- V1.22.0.0⑥↑ -----
        '----- V4.0.0.0-40↓ -----
        If (giMachineKd = MACHINE_KD_RS) Then
            ' SL36S時でステージYの原点位置が上の場合、ブロックサイズの1/2は加算しない
            ''----- V2.0.0.0⑨↓ -----
            If (giStageYOrg = STGY_ORG_UP) Then
                PosY = PosY
            Else
                PosY = PosY ' (typPlateInfo.dblBlockSizeYDir / 2)
            End If

            'If (giMachineKd = MACHINE_KD_RS) Then
            '    PosY = PosY + (typPlateInfo.dblBlockSizeYDir / 2)
            'End If
            ''----- V2.0.0.0⑨↑ -----
        End If
        '----- V4.0.0.0-40↑ -----

        ' ステージ移動
        r = Form1.System1.EX_START(gSysPrm, PosX, PosY, 0)
        'V1.19.0.0-31 ADD START
        If (r <> cFRS_NORMAL) Then                                      ' エラー ?(メッセージは表示済み) 
            ' 強制終了
            Call Form1.AppEndDataSave()
            Call Form1.AplicationForcedEnding()
        End If
        'V1.19.0.0-31 ADD END
        '----- ###249↑ -----
        Return (r)

    End Function
#End Region

#Region "(Teaching向け)メイン画面上のクロスラインの表示位置を変更する"
    '''=========================================================================
    '''<summary>メイン画面上のクロスラインの表示位置を変更する</summary>
    '''=========================================================================
    Public Sub SetCrossLinePos(ByVal xPos As Integer, ByVal yPos As Integer)
        ' クロスライン位置を設定する 
        'V6.0.0.0④        Form1.Picture1.Top = xPos + Form1.VideoLibrary1.Location.Y
        'V6.0.0.0④        Form1.Picture2.Left = yPos + Form1.VideoLibrary1.Location.X

        ' 画面の再描画
        'V6.0.0.0④        Form1.Refresh()
        Form1.Instance.VideoLibrary1.SetCrossLineCenter(yPos, xPos)     'V6.0.0.0④

    End Sub
#End Region

#Region "(Teaching向け)マーキングエリア表示"
    '''=========================================================================
    '''<summary>メイン画面上のマーキングエリアの四角を表示/非表示する</summary>
    '''=========================================================================
    Public Sub DisplayMarkingArea(ByVal bDisp As Boolean, ByVal xPos As Integer, ByVal yPos As Integer,
                                        ByVal width As Integer, ByVal height As Integer)
#If False Then              'V6.0.0.0④
        If (bDisp = True) Then
            '指定座標を設定しマーキングエリアを表示する。
            Form1.MarkingAreaLeft.Left = xPos + Form1.VideoLibrary1.Location.X
            Form1.MarkingAreaLeft.Top = yPos + Form1.VideoLibrary1.Location.Y
            Form1.MarkingAreaLeft.Height = height
            Form1.MarkingAreaUpper.Left = xPos + Form1.VideoLibrary1.Location.X
            Form1.MarkingAreaUpper.Top = yPos + Form1.VideoLibrary1.Location.Y
            Form1.MarkingAreaUpper.Width = width
            Form1.MarkingAreaRight.Left = xPos + Form1.VideoLibrary1.Location.X + width
            Form1.MarkingAreaRight.Top = yPos + Form1.VideoLibrary1.Location.Y
            Form1.MarkingAreaRight.Height = height
            Form1.MarkingAreaLow.Left = xPos + Form1.VideoLibrary1.Location.X
            Form1.MarkingAreaLow.Top = yPos + Form1.VideoLibrary1.Location.Y + height
            Form1.MarkingAreaLow.Width = width

            'ラインの表示
            Form1.MarkingAreaLeft.Visible = True
            Form1.MarkingAreaLow.Visible = True
            Form1.MarkingAreaRight.Visible = True
            Form1.MarkingAreaUpper.Visible = True
        Else
            'ラインの非表示
            Form1.MarkingAreaLeft.Visible = False
            Form1.MarkingAreaLow.Visible = False
            Form1.MarkingAreaRight.Visible = False
            Form1.MarkingAreaUpper.Visible = False
        End If
        ' 画面のリフレッシュ
        Form1.Refresh()
#Else
        Form1.Instance.VideoLibrary1.SetMarkingArea(bDisp, xPos, yPos, width, height)
#End If
    End Sub
#End Region

    'V5.0.0.6⑫↓
#Region "クロスライン非表示"
    Public Sub CrossLineDispOff() Implements IMainModules.CrossLineDispOff      'V6.0.0.0①
        Try
#If False Then      'V6.0.0.0④
            Form1.CrosLineX.Visible = False
            Form1.CrosLineY.Visible = False
            Form1.CrosLineX.Refresh()
            Form1.CrosLineY.Refresh()
            Call Form1.VideoLibrary1.Refresh()
#End If
            Form1.Instance.VideoLibrary1.SetCorrCrossVisible(False)     'V6.0.0.0④

        Catch ex As Exception
            MsgBox("MainModules.CrossLineDispOff() TRAP ERROR = " + ex.Message)
        End Try
    End Sub
#End Region
    'V5.0.0.6⑫↑

#Region "(Teaching-Jog向け)補正クロスライン表示処理"
    '''=========================================================================
    ''' <summary>(Teaching-Jog向け)補正クロスライン表示処理</summary>
    ''' <param name="xPos">(INP)BP位置X(mm)</param>
    ''' <param name="yPos">(INP)BP位置Y(mm)</param>
    '''=========================================================================
    Public Sub DispCrossLine(ByVal xPos As Double, ByVal yPos As Double) _
        Implements IMainModules.DispCrossLine                           'V6.0.0.0①

        Dim strMSG As String

        Try
            '----- ###232↓ -----
            'クロスライン補正処理を呼び出す
            ObjCrossLine.CrossLineDispXY(xPos, yPos)

            ''クロスライン補正処理を呼び出す
            'gstCLC.x = xPos                        ' BP位置X(mm)
            'gstCLC.y = yPos                        ' BP位置Y(mm)
            'Call CrossLineCorrect(gstCLC)          ' 補正クロスライン表示
            '----- ###232↑ -----

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "MainModules.DispCrossLine() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- V1.18.0.0③↓ -----
#Region "アラーム発生回数とアラーム停止開始時間を設定する(ローム殿特注)(OCX用)"
    '''=========================================================================
    ''' <summary>アラーム発生回数とアラーム停止開始時間を設定する</summary>
    ''' <param name="strDAT">  (INP)エラーメッセージ</param>
    ''' <param name="ErrCode"> (INP)エラーコード</param>
    ''' <remarks>ローム殿特注
    '''          OcxSystemからCallされる</remarks>
    '''=========================================================================
    Public Sub Call_SetAlmStartTime(ByVal strDAT As String, ByVal ErrCode As Short) _
        Implements IMainModules.Call_SetAlmStartTime                    'V6.0.0.0①

        Dim strMSG As String

        Try
            ' 初期処理
            If (gSysPrm.stCTM.giSPECIAL <> customROHM) Then Return '    ' ローム殿特注でなければNOP

            ' 印刷用アラーム開始情報を設定する(ローム殿特注)
            ' アラーム発生回数とアラーム停止開始時間を設定する
            Call SetAlmStartTime()

            ' アラームファイルにアラームデータを書き込む
            Call WriteAlarmData(gFPATH_QR_ALARM, strDAT, stPRT_ROHM.AlarmST_time, ErrCode)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "MainModules.Call_SetAlmStartTime() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "アラーム停止情報を設定する(ローム殿特注)(OCX用)"
    '''=========================================================================
    ''' <summary>アラーム停止情報を設定する</summary>
    ''' <remarks>ローム殿特注
    '''          OcxSystemからCallされる</remarks>
    '''=========================================================================
    Public Sub Call_SetAlmEndTime() Implements IMainModules.Call_SetAlmEndTime  'V6.0.0.0①

        Dim strMSG As String

        Try
            ' 初期処理
            If (gSysPrm.stCTM.giSPECIAL <> customROHM) Then Return '    ' ローム殿特注でなければNOP

            ' 印刷用アラーム停止終了時間を設定する(ローム殿特注)
            Call SetAlmEndTime()

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "MainModules.Call_SetAlmEndTime() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

    '----- V1.18.0.0③↑ -----
    '----- V1.13.0.0③↓ -----
    '===========================================================================
    '   オートプローブ用メソッド定義
    '===========================================================================
#Region "オートプローブ用マトリックス画面表示サブルーチン"
    '''=========================================================================
    ''' <summary>オートプローブ用マトリックス画面表示サブルーチン</summary>
    ''' <returns>0=正常, 0以外=エラー</returns> 
    '''=========================================================================
    Public Function Sub_CallFrmMatrix() As Integer Implements IMainModules.Sub_CallFrmMatrix    'V6.0.0.0①

        Dim r As Integer
        Dim StgOfsX As Double
        Dim StgOfsY As Double
        Dim objForm As Object = Nothing
        Dim strMSG As String

        Try
            ' giAppmode = オートプローブ実行モードならログ表示域を表示する
            If (giAppMode = APP_MODE_APROBEEXE) Then
                Form1.txtLog.Visible = True
            End If

            ' マトリックス画面表示
            r = ShowMatrixDialog(Me)

            ' 補正結果の取得
            r = GetMatrixReturn(StgOfsX, StgOfsY)
            If (r = cFRS_NORMAL) Then
                If (giAppMode = APP_MODE_PROBE) Then                    ' プローブコマンド時はオフセット更新しない

                Else
                    gfStgOfsX = StgOfsX                                 ' XYテーブルオフセットX(mm)
                    gfStgOfsY = StgOfsY                                 ' XYテーブルオフセットY(mm)
                End If
            End If

            ' アプリ強制終了(パターン認識エラー以外)
            If (r < cFRS_NORMAL) And ((r > cFRS_ERR_PTN) Or (r < cFRS_ERR_PT2)) Then
                Call Form1.AppEndDataSave()
                Call Form1.AplicationForcedEnding()                     ' アプリ強制終了
            End If
            Return (r)                                                  ' Return

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "MainModules.Sub_CallFrmRset() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "オートプローブ実行用構造体にステップ実行用パラメータを設定する(OCX用)"
    '''=========================================================================
    ''' <summary>オートプローブ実行用構造体にステップ実行用パラメータを設定する</summary>
    ''' <param name="StepCount">  (INP)ステップ回数</param>
    ''' <param name="ProbePitch"> (INP)ステップピッチ</param>
    ''' <param name="StepCount2"> (INP)繰返しステップ回数</param>
    ''' <param name="ProbePitch2">(INP)繰返しステップピッチ</param>
    '''=========================================================================
    Public Sub SetAPBPrm_Step(ByVal StepCount As Short, ByVal ProbePitch As Double, ByVal StepCount2 As Short, ByVal ProbePitch2 As Double) _
        Implements IMainModules.SetAPBPrm_Step                          'V6.0.0.0①

        Dim strMSG As String

        Try
            stAPRB.intAProbeStepCount = StepCount
            stAPRB.dblAProbePitch = ProbePitch
            stAPRB.intAProbeStepCount2 = StepCount2
            stAPRB.dblAProbePitch2 = ProbePitch2

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "MainModules.Sub_CallFrmRset() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "オートプローブ実行用構造体にステップ実行用パターンパラメータを設定する(OCX用)"
    '''=========================================================================
    ''' <summary>オートプローブ実行用構造体にステップ実行用パターンパラメータを設定する</summary>
    ''' <param name="GroupNo1">(INP)パターン1(下方)用グループ番号</param>
    ''' <param name="PtnNo1">  (INP)パターン1(下方)用パターン番号</param>
    ''' <param name="GroupNo2">(INP)パターン2(上方)用グループ番号</param>
    ''' <param name="PtnNo2">  (INP)パターン2(上方)用パターン番号</param>
    ''' <param name="BpPosX">  (INP)パターン1(下方)用BP位置X</param>
    ''' <param name="BpPosY">  (INP)パターン1(下方)用BP位置Y</param>
    ''' <param name="StgPosX"> (INP)パターン2(上方)用ステージオフセット位置X</param>
    ''' <param name="StgPosY"> (INP)パターン2(上方)用ステージオフセット位置Y</param>
    '''=========================================================================
    Public Sub SetAPBPrm_Ptn(ByVal GroupNo1 As Short, ByVal PtnNo1 As Double, ByVal GroupNo2 As Short, ByVal PtnNo2 As Double,
                             ByVal BpPosX As Double, ByVal BpPosY As Double, ByVal StgPosX As Double, ByVal StgPosY As Double)

        Dim strMSG As String

        Try
            stAPRB.intAProbeGroupNo1 = GroupNo1
            stAPRB.intAProbePtnNo1 = PtnNo1
            stAPRB.intAProbeGroupNo2 = GroupNo2
            stAPRB.intAProbePtnNo2 = PtnNo2
            stAPRB.dblAProbeBpPosX = BpPosX
            stAPRB.dblAProbeBpPosY = BpPosY
            stAPRB.dblAProbeStgPosX = StgPosX
            stAPRB.dblAProbeStgPosY = StgPosY

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "MainModules.SetAPBPrm_Ptn() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- V1.13.0.0③↑ -----
    '----- V1.16.0.0⑯↓ -----
    '===========================================================================
    '   OCXからのオートローダシリアル通信用メソッド呼び出し(SL436用)
    '===========================================================================
#Region "吸着確認ステータスを返す(SL436R用)"
    '''=========================================================================
    ''' <summary>吸着確認ステータスを返す(SL436R用)</summary>
    ''' <param name="Sts">(OUT)ステータス(1:吸着確認, 0:吸着未確認)</param>
    '''=========================================================================
    Public Sub Call_GetVacumeStatus(ByRef Sts As Integer) _
        Implements IMainModules.Call_GetVacumeStatus                    'V6.0.0.0①

        Dim strMSG As String

        Try
            Call GetVacumeStatus(Sts)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "MainModules.Call_GetVacumeStatus() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- V1.16.0.0⑯↑ -----
    '===========================================================================
    ' 'V3.0.0.0③ ビデオスタート停止処理
    '===========================================================================
#Region "Video"""
    ''' <summary>
    ''' ビデオのスタート処理
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub VideoStart() Implements IMainModules.VideoStart 'V6.0.0.0①
        'V6.0.0.0-28        Try
        'V6.0.0.0-28            Call Form1.Instance.VideoLibrary1.VideoStart()
        'V6.0.0.0-28        Catch ex As Exception
        'V6.0.0.0-28            MsgBox("MainModules.VideoStart() TRAP ERROR = " + ex.Message)
        'V6.0.0.0-28        End Try
    End Sub
    ''' <summary>
    ''' ビデオのストップ処理
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub VideoStop() Implements IMainModules.VideoStop 'V6.0.0.0①
        'V6.0.0.0-28        Try
        'V6.0.0.0-28        Call Form1.Instance.VideoLibrary1.VideoStop()
        'V6.0.0.0-28        Catch ex As Exception
        'V6.0.0.0-28            MsgBox("MainModules.VideoStop() TRAP ERROR = " + ex.Message)
        'V6.0.0.0-28        End Try
    End Sub

    ''' <summary>DllVideo.VideoLibraryの倍率調整トラックバー表示状態を設定する</summary>
    ''' <remarks>'V6.0.1.0⑤</remarks>
    Public Sub SetVideoTrackBar(ByVal visible As Boolean, ByVal enabled As Boolean) Implements IMainModules.SetVideoTrackBar
        Form1.Instance.VideoLibrary1.SetTrackBar(visible, enabled)
    End Sub

    ''' <summary><para>表示中のJOGを制御するKeyDown,KeyUp時の処理をメインフォームに、</para>
    ''' <para>カメラ画像MouseClick時の処理をDllVideoに設定する</para></summary>
    ''' <remarks>'V6.0.1.0⑥</remarks>
    Public Sub SetActiveJogMethod(ByVal keyDown As Action(Of KeyEventArgs),
                                  ByVal keyUp As Action(Of KeyEventArgs),
                                  ByVal moveToCenter As Action(Of Decimal, Decimal)) Implements IMainModules.SetActiveJogMethod

        Form1.Instance.SetActiveJogMethod(keyDown, keyUp, moveToCenter)
    End Sub
#End Region
    '----- V2.0.0.0②↓ -----
#Region "ローダアラームチェック(自動運転以外時(SL436RS用))"
    '''=========================================================================
    ''' <summary>ローダアラームチェック(自動運転以外時(SL436RS用))</summary>
    ''' <returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Public Function Call_Loader_AlarmCheck_ManualMode() As Integer _
        Implements IMainModules.Call_Loader_AlarmCheck_ManualMode       'V6.0.0.0①

        Dim r As Integer
        Dim strMSG As String

        Try
            ' ローダアラームチェック(自動運転時以外)
            r = Loader_AlarmCheck_ManualMode(Form1.System1)             ' エラー時はメッセージ表示済
            Return (r)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "MainModules.Call_Loader_AlarmCheck_ManualMode() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cFRS_NORMAL)
        End Try
    End Function
#End Region
    '----- V2.0.0.0②↑ -----
    '----- V1.23.0.0⑦↓ -----
    '===========================================================================
    '   プローブチェック用メソッド定義(オプション)
    '===========================================================================
#Region "プローブチェックメイン処理(プローブチェック機能)"
    '''=========================================================================
    ''' <summary>プローブチェックメイン処理(プローブチェック機能)</summary>
    ''' <param name="digL">      (INP)Dig-SW Low</param>
    ''' <param name="PltCounter">(INP)基板カウンタ</param>
    ''' <param name="ChkCounter">(I/O)チェックカウンタ</param>
    ''' <returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Public Function ProbeCheck(ByVal digL As Integer, ByVal PltCounter As Integer, ByRef ChkCounter As Integer) As Integer

        Dim r As Integer = cFRS_NORMAL
        Dim rtn As Integer = cFRS_NORMAL
        Dim ChkBlkX As Integer
        Dim ChkBlkY As Integer
        Dim OffSetX As Double = 0.0
        Dim OffSetY As Double = 0.0
        Dim StgPosX As Double = 0.0
        Dim StgPosY As Double = 0.0
        Dim strMSG As String

        Try
            ' プローブチェック機能なし又はチェック枚数の指定なし又は自動運転中(SL436R)でないならNOP
            If ((giProbeCheck = 0) Or (typPlateInfo.intPrbChkPlt = 0) Or (bFgAutoMode <> True)) Then
                Return (cFRS_NORMAL)
            End If

            ' x0, x1, x2以外ならNOP
            If (digL > 2) Then
                Return (cFRS_NORMAL)
            End If

            ' 基板カウンタとチェックカウンタが等しくなければNOP
            If (PltCounter <> ChkCounter) Then
                Return (cFRS_NORMAL)
            End If
            ChkCounter = ChkCounter + typPlateInfo.intPrbChkPlt         ' チェックカウンタ更新

            ' 指定のブロック位置にステージを移動する
            If (typPlateInfo.intResistDir = 0) Then                     ' 抵抗並び方向 = X方向 ?
                ChkBlkX = 1                                             ' ブロック番号X = 1
                ChkBlkY = typPlateInfo.intPrbChkBlk                     ' ブロック番号Y = チェックブロック番号
            Else
                ChkBlkX = typPlateInfo.intPrbChkBlk                     ' ブロック番号X = チェックブロック番号
                ChkBlkY = 1                                             ' ブロック番号Y = 1
            End If
            r = Call_GetTargetStagePosByXY(ChkBlkX, ChkBlkY, OffSetX, OffSetY, StgPosX, StgPosY)
            If (r <> cFRS_NORMAL) Then
                Return (r)
            End If

            ' プローブチェックを実行する
            rtn = Sub_ProbeCheck(PltCounter, ChkBlkX, ChkBlkY, typPlateInfo.dblPrbTestLimit, strPrbChkFileLog, strPrbChkDispLog)

            ' ログを出力する(画面とファイル)
            If (strPrbChkDispLog <> "") Then
                Call Sub_ProbeCheckLogOut(strPrbChkFileLog)             ' ログファイルに出力する 
                If (rtn = cFRS_FNG_PROBCHK) Then                        ' ログ画面表示はプローブチェックエラー時のみ表示する 
                    Call Form1.Z_PRINT(strPrbChkDispLog)                ' ログを表示する(表示モードにかかわらず)
                End If
            End If

            ' プローブチェック実行結果をチェックする
            If (rtn = cFRS_FNG_PROBCHK) Then                            ' プローブチェックエラーならメッセージ表示(STARTキー押下待ち)
                ' シグナルタワー制御(On=赤点滅+ブザー, Off=全ﾋﾞｯﾄ)
                'V5.0.0.9⑭ ↓ V6.0.3.0⑧
                ' Call Form1.System1.SetSignalTower(SIGOUT_RED_BLK Or SIGOUT_BZ1_ON, &HFFFF)
                Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALARM)
                'V5.0.0.9⑭ ↑ V6.0.3.0⑧

                ' メッセージ表示 "プローブチェックエラー","STARTキー又はOKボタン押下で","自動運転を中止します"
                r = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_START, True,
                        MSG_LOADER_47, MSG_LOADER_38, MSG_LOADER_23, System.Drawing.Color.Red, System.Drawing.Color.Blue, System.Drawing.Color.Blue)

                ' シグナルタワー制御(On=なし, Off=赤点滅+ブザー)
                'V5.0.0.9⑭ ↓ V6.0.3.0⑧
                ' Call Form1.System1.SetSignalTower(0, SIGOUT_RED_BLK Or SIGOUT_BZ1_ON)
                Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALL_OFF)
                'V5.0.0.9⑭ ↑ V6.0.3.0⑧

                If (r < cFRS_NORMAL) Then Return (r) '                  ' 非常停止等のエラーならエラー戻り(エラーメッセージは表示済み) 
                Return (cFRS_ERR_RST)                                   ' プローブチェックエラーならReturn値 = Cancel(RESETキー押下) 
            End If

            Return (rtn)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "MainModules.ProbeCheck() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cFRS_NORMAL)
        End Try
    End Function
#End Region

#Region "プローブチェック処理(プローブチェック機能)"
    '''=========================================================================
    ''' <summary>プローブチェック処理(プローブチェック機能)</summary>
    ''' <param name="Plt">   (INP)基板番号</param>
    ''' <param name="BlkX">  (INP)ブロック番号X</param>
    ''' <param name="BlkY">  (INP)ブロック番号Y</param>
    ''' <param name="Limit"> (INP)誤差±%</param>
    ''' <param name="strLOG">(OUT)ファイル出力用ログエリア</param>
    ''' <param name="strDSP">(OUT)表示用ログエリア</param>
    ''' <returns>0=正常, 0以外=エラー</returns>
    ''' <remarks>・PROBE.OCXからもCallされる
    '''          ・ステージは指定ブロックの正規位置にいる事</remarks> 
    '''=========================================================================
    Public Function Sub_ProbeCheck(ByVal Plt As Integer, ByVal BlkX As Integer, ByVal BlkY As Integer,
                                   ByVal Limit As Double, ByRef strLOG As String, ByRef strDSP As String) As Integer _
                               Implements IMainModules.Sub_ProbeCheck   'V6.0.0.0①
        Dim bFlg As Boolean
        Dim PosX As Double = 0.0
        Dim PosY As Double = 0.0
        Dim StgOfsX As Double = 0.0
        Dim StgOfsY As Double = 0.0
        Dim BpOfsX As Double = 0.0
        Dim BpOfsY As Double = 0.0
        'Dim Delay As Integer
        Dim r As Integer = cFRS_NORMAL
        Dim rtn As Integer = cFRS_NORMAL
        Dim Count As Integer
        Dim strMSG As String

        Try
            '-------------------------------------------------------------------
            '   初期処理
            '-------------------------------------------------------------------
            ' 現在座標退避 
            Call ZGETPHPOS2(PosX, PosY)

            ' ディレイトリムならノーマルモードでプレートデータを送信為直す
            ' ※ディレイトリム２はx0,x1モード時のみ有効なので下記は不要
            'Delay = typPlateInfo.intDelayTrim
            'If (Delay <> 0) Then
            '    typPlateInfo.intDelayTrim = 0
            '    r = SendTrimDataPlate(gTkyKnd, typPlateInfo.intResistCntInBlock, BpOfsX, BpOfsY)
            '    If (r <> cFRS_NORMAL) Then GoTo STP_ERR2
            'End If

            ' プローブチェック機能用トリミング結果構造体初期化
            stPrbChk.Initialize(gRegistorCnt)

            '-------------------------------------------------------------------
            '   指定のブロックで下記３回の測定を行い、測定値のバラツキが誤差以内かチェックする
            '   １回目 - 正規の位置で測定する
            '   ２回目 - 正規の位置から１チップ分、ステージを右にづらして測定する
            '   ３回目 - 正規の位置から１チップ分、ステージを左にづらして測定する
            '-------------------------------------------------------------------
            For Count = 1 To 3
                ' ZをON位置に移動する
                r = Sub_ZOnOff(1)
                If (r <> cFRS_NORMAL) Then GoTo STP_ERR_EXIT '          ' エラーならAPP終了(メッセージ表示は済み) 

                ' １ブロック分測定を実行する(x2モード)
                r = TRIMBLOCK(2, gSysPrm.stDEV.giPower_Cyc, 0, 0, 0)
                bFlg = IS_CV_OverLoadErrorCode(r)                       ' 測定ばらつき検出/オーバロード検出チェック
                If (bFlg = False) Then                                  ' 測定ばらつき検出/オーバロード検出ならメッセージ表示しない 
                    r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)   ' その他のエラーならメッセージを表示する
                    If (r < cFRS_NORMAL) Then GoTo STP_ERR_EXIT '       ' エラーならAPP終了(メッセージ表示は済み)  
                End If

                '' 測定の判定結果の取得する→gwTrimResult()
                'Call TrimLoggingResult_Get(RSLTTYP_TRIMJUDGE, 0)

                ' ファイナルテスト判定結果(測定値)を取得する→gfFinalTest()
                Call TrimLoggingResult_Get(RSLTTYP_FINAL_TEST, 0)

                ' 測定結果をチェックし結果を編集する
                r = Sub_ProbeResultCheck(Count, gfFinalTest, Plt, BlkX, BlkY, Limit, strLOG, strDSP)
                If (r <> cFRS_NORMAL) Then
                    rtn = r                                             ' Return値 = プローブチェックエラー 
                End If

                ' Zを待機位置に移動する
                r = Sub_ZOnOff(0)
                If (r <> cFRS_NORMAL) Then GoTo STP_ERR_EXIT '          ' エラーならAPP終了(メッセージ表示は済み)  
                If (Count = 3) Then Exit For

                ' ステージ移動オフセットを設定する
                If (typPlateInfo.intResistDir = 0) Then                 ' ﾁｯﾌﾟ並びはX方向 ?
                    If (Count = 1) Then
                        StgOfsX = typPlateInfo.dblChipSizeXDir
                    Else
                        StgOfsX = typPlateInfo.dblChipSizeXDir * -1
                    End If
                Else
                    If (Count = 1) Then
                        StgOfsY = typPlateInfo.dblChipSizeYDir
                    Else
                        StgOfsY = typPlateInfo.dblChipSizeYDir * -1
                    End If
                End If

                ' ステージを１チップ分右(上)または左(下)に移動する
                r = Form1.System1.EX_SMOVE2(gSysPrm, PosX + StgOfsX, PosY + StgOfsY)
                If (r <> cFRS_NORMAL) Then GoTo STP_ERR_EXIT '          ' エラーならAPP終了(メッセージ表示は済み)  

            Next Count

            '-------------------------------------------------------------------
            '   後処理
            '-------------------------------------------------------------------
            ' BPオフセット位置へ移動(プローブコマンド時)
            If (giAppMode = APP_MODE_PROBE) Then                        ' プローブコマンド時
                r = Form1.System1.BPMOVE(gSysPrm, typPlateInfo.dblBpOffSetXDir, typPlateInfo.dblBpOffSetYDir, typPlateInfo.dblBlockSizeXDir, typPlateInfo.dblBlockSizeYDir, 0, 0, 1)
                If (r <> cFRS_NORMAL) Then GoTo STP_ERR_EXIT '          ' エラーならAPP終了(メッセージ表示は済み)  
            End If

            ' ステージを正規の位置へ移動する
            r = Form1.System1.EX_SMOVE2(gSysPrm, PosX, PosY)
            If (r <> cFRS_NORMAL) Then GoTo STP_ERR_EXIT '              ' エラーならAPP終了(メッセージ表示は済み)  

            ' プレートデータを元に戻す
            ' ※ディレイトリム２はx0,x1モード時のみ有効なので下記は不要
            'If (Delay <> 0) Then
            '    typPlateInfo.intDelayTrim = Delay
            '    ' トリミングデータを送信しなおす(プレートデータ送信でカットデータが初期化されるため)
            '    r = SendTrimData()
            '    If (r <> cFRS_NORMAL) Then GoTo STP_ERR2
            'End If

            Return (rtn)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "MainModules.Sub_ProbeCheck() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try

STP_ERR_EXIT:
        ' アプリ強制終了
        Call Form1.AppEndDataSave()
        Call Form1.AplicationForcedEnding()                             ' アプリ強制終了
        Return (r)                                                      ' Return

STP_ERR2:
        ' "トリミングデータの設定に失敗しました。" & vbCrLf & "トリミングデータに問題がないか確認してください。"
        Call Form1.System1.TrmMsgBox(gSysPrm, MSGERR_SEND_TRIMDATA, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, TITLE_4)
        Return (cFRS_ERR_TRIM)                                          ' Return値 = トリマエラー
    End Function
#End Region

#Region "測定結果をチェックし結果を編集して返す(プローブチェック機能)"
    '''=========================================================================
    ''' <summary>測定結果をチェックし結果を編集して返す(プローブチェック機能)</summary>
    ''' <param name="Count">      (INP)測定回数(1-3)</param>
    ''' <param name="gfFinalTest">(INP)測定値の配列</param>
    ''' <param name="Plt">        (INP)基板番号</param>
    ''' <param name="BlkX">       (INP)ブロック番号X</param>
    ''' <param name="BlkY">       (INP)ブロック番号Y</param>
    ''' <param name="Limit">      (INP)誤差±%</param>
    ''' <param name="strLOG">     (OUT)ファイル出力用ログエリア</param>
    ''' <param name="strDSP">     (OUT)表示用ログエリア</param>
    ''' <returns>0=正常, 0以外=エラー</returns> 
    '''=========================================================================
    Public Function Sub_ProbeResultCheck(ByVal Count As Integer, ByRef gfFinalTest() As Double, ByVal Plt As Integer,
                    ByVal BlkX As Integer, ByVal BlkY As Integer, ByVal Limit As Double, ByRef strLOG As String, ByRef strDSP As String) As Integer

        Dim r As Integer = cFRS_NORMAL
        Dim Rn As Integer
        Dim GetRn As Integer
        Dim SetRn As Integer
        Dim LimitLo As Double
        Dim LimitHi As Double
        Dim dblDAT(0 To 2) As Double
        Dim dblDEV(0 To 1) As Double
        Dim strDAT(0 To 2) As String
        Dim strDEV As String
        Dim strRESULT As String
        Dim strMSG As String

        Try
            '-------------------------------------------------------------------
            '   初期処理
            '-------------------------------------------------------------------
            ' 見出しを設定する
            If (Count = 1) Then
                strLOG = ""                                             ' ファイル出力用ログエリアクリア
                strDSP = ""                                             ' 表示用ログエリアクリア

                '----- ファイル出力用ログ -----
                ' 日付(YYYY/MM/DD HH:mm:ss) 
                If (giAppMode <> APP_MODE_PROBE) Then                   ' プローブコマンド以外 
                    strLOG = Today.ToString("yyyy/MM/dd")
                    strLOG = strLOG + " " + TimeOfDay.ToString("HH:mm:ss") + vbCrLf

                    ' "Plate = 9 Block X=999 Y=999"
                    strLOG = strLOG + "Plate = " + Plt.ToString("0") + " Block X=" + BlkX.ToString("000") + " Y=" + BlkY.ToString("000") + vbCrLf
                    ' "RNo      MEAS1       MEAS2                MEAS3      DEVIAT(%)         RESULT"
                    strLOG = strLOG + "RNo".PadLeft(4) + "MEAS1  ".PadLeft(17) + "MEAS2  ".PadLeft(17) +
                                   "MEAS3  ".PadLeft(17) + "     DEVIAT(%)  " + "   RESULT" + vbCrLf

                Else                                                    ' プローブコマンドの場合 
                    ' "Block X=999 Y=999"
                    strLOG = strLOG + "Block X=" + BlkX.ToString("000") + " Y=" + BlkY.ToString("000") + vbCrLf
                    ' "RNo      MEAS1       MEAS2                MEAS3      DEVIAT(%)         RESULT"
                    strLOG = strLOG + "RNo".PadLeft(4) + "MEAS1  ".PadLeft(17) + "MEAS2  ".PadLeft(17) +
                                   "MEAS3  ".PadLeft(17) + "     DEVIAT(%)  " + "   RESULT" + vbCrLf
                End If

                '----- 表示用ログ -----
                If (giAppMode <> APP_MODE_PROBE) Then                   ' プローブコマンド以外 
                    strDSP = "=== Probe Check ===" + vbCrLf
                    ' "Plate = 9 Block X=999 Y=999"
                    strDSP = strDSP + "Plate = " + Plt.ToString("0") + " Block X=" + BlkX.ToString("000") + " Y=" + BlkY.ToString("000") + vbCrLf
                    ' "RNo      MEAS1       MEAS2                MEAS3      DEVIAT(%)         RESULT"
                    strDSP = strDSP + "RNo".PadLeft(4) + "MEAS1  ".PadLeft(17) + "MEAS2  ".PadLeft(17) +
                                   "MEAS3  ".PadLeft(17) + "     DEVIAT(%)  " + "   RESULT" + vbCrLf
                Else
                    ' "Block X=999 Y=999"
                    strDSP = strDSP + "Block X=" + BlkX.ToString("000") + " Y=" + BlkY.ToString("000") + vbCrLf
                    ' "RNo      MEAS1       MEAS2                MEAS3      DEVIAT(%)         RESULT"
                    strDSP = strDSP + "RNo".PadLeft(4) + "MEAS1  ".PadLeft(17) + "MEAS2  ".PadLeft(17) +
                                   "MEAS3  ".PadLeft(17) + "     DEVIAT(%)  " + "   RESULT" + vbCrLf
                End If
#If cOFFLINEcDEBUG Then
                'Console.WriteLine(strLOG)                               ' For Debug 
                'Console.WriteLine(strDSP)                               ' For Debug 
#End If
            End If

            ' 取得と設定データインデックスを設定する
            If (Count = 1) Then                                         ' 1回目は正規の位置 
                GetRn = 0
                SetRn = 0
            ElseIf (Count = 2) Then                                     ' 2回目はR1の結果をR2から設定する 
                GetRn = 0
                SetRn = 1
            Else                                                        ' 3回目はR2の結果をR1から設定する
                GetRn = 1
                SetRn = 0
            End If

            ' 測定値をプローブチェック機能用トリミング結果データに設定する(レンジオーパは「9999999999.9999」で帰る)
            For GetRn = GetRn To gRegistorCnt - 1
                If (typResistorInfoArray(GetRn).intResNo >= 1000) Then Continue For
                stPrbChk.FinalTest(Count - 1, SetRn) = gfFinalTest(GetRn)
                SetRn = SetRn + 1
            Next GetRn

            ' ３回目の測定でなければReturn
            If (Count < 3) Then Return (cFRS_NORMAL)

            '-------------------------------------------------------------------
            '   測定結果を編集する
            '-------------------------------------------------------------------
            ' 測定結果を抵抗数分編集する
            For Rn = 0 To gRegistorCnt - 1
                ' １回目の測定データ
                dblDAT(0) = stPrbChk.FinalTest(0, Rn)
                ' 17文字右詰前方空白
                strDAT(0) = String.Format("{0,17}", stPrbChk.FinalTest(0, Rn).ToString(gsEDIT_DIGITNUM))

                ' ２回目の測定データ(先頭抵抗はない)  
                If (Rn = 0) Then                                        ' 先頭抵抗 ? 
                    dblDAT(1) = stPrbChk.FinalTest(0, Rn)              ' 最初の測定値を２回目の測定データとする
                    strDAT(1) = String.Format("{0,17}", stPrbChk.FinalTest(2, Rn).ToString(gsEDIT_DIGITNUM))
                Else
                    dblDAT(1) = stPrbChk.FinalTest(1, Rn)
                    strDAT(1) = String.Format("{0,17}", stPrbChk.FinalTest(1, Rn).ToString(gsEDIT_DIGITNUM))
                End If

                ' ３回目の測定データ(最終抵抗はない) 
                If (Rn = (gRegistorCnt - 1)) Then                       ' 最終抵抗 ? 
                    dblDAT(2) = stPrbChk.FinalTest(0, Rn)               ' 最初の測定値を３回目の測定データとする
                Else
                    dblDAT(2) = stPrbChk.FinalTest(2, Rn)
                    strDAT(2) = String.Format("{0,17}", stPrbChk.FinalTest(2, Rn).ToString(gsEDIT_DIGITNUM))
                End If

                If (Rn = 0) Then                                        ' 先頭抵抗 ? 
                    strDAT(1) = "".PadLeft(17)                          ' ２回目の測定データの表示はなし
                End If
                If (Rn = (gRegistorCnt - 1)) Then                       ' 最終抵抗 ?
                    strDAT(2) = "".PadLeft(17)                          ' ３回目の測定データの表示はなし
                End If

                ' dblDAT(1)に中間値を設定する(dblDAT(0)=最小値, dblDAT(2)=最大値)
                If (dblDAT(0) > dblDAT(1)) Then SwapDouble(dblDAT(0), dblDAT(1))
                If (dblDAT(0) > dblDAT(2)) Then SwapDouble(dblDAT(0), dblDAT(2))
                If (dblDAT(1) > dblDAT(2)) Then SwapDouble(dblDAT(1), dblDAT(2))

                ' 誤差を求める
                If (dblDAT(1) = 0) Or
                  ((dblDAT(1) = 9999999999.9999) And ((dblDAT(0) <> 9999999999.9999) And (dblDAT(1) <> 9999999999.9999) And (dblDAT(2) <> 9999999999.9999))) Then
                    dblDEV(0) = 999.999
                    dblDEV(1) = 999.999
                Else
                    dblDEV(0) = (dblDAT(0) / dblDAT(1) - 1.0) * 100     ' (最小値/中間値 - 1) * 100
                    dblDEV(1) = (dblDAT(2) / dblDAT(1) - 1.0) * 100     ' (最大値/中間値 - 1) * 100
                End If

                If (dblDEV(0) > 999.999) Then
                    dblDEV(0) = 999.999
                End If
                If (dblDEV(0) < -999.999) Then
                    dblDEV(0) = -999.999
                End If
                If (dblDEV(1) > 999.999) Then
                    dblDEV(1) = 999.999
                End If
                If (dblDEV(1) < -999.999) Then
                    dblDEV(1) = -999.999
                End If

                ' 誤差(表示用)を求める
                strDEV = dblDEV(0).ToString("0.000").PadLeft(8) + " " + dblDEV(1).ToString("0.000").PadLeft(8)

                ' 測定値のバラツキが誤差以内かチェックする
                LimitLo = Math.Abs(Limit) * -1
                LimitHi = Math.Abs(Limit)
                strRESULT = "OK"
                If ((dblDEV(0) < LimitLo) Or (dblDEV(1) > LimitHi)) Then
                    strRESULT = "NG"
                    r = cFRS_FNG_PROBCHK                                ' Return値 = プローブチェックエラー
                End If

                '測定結果をフォーマット変換し文字列を構築(16文字左詰め)
                ' ファイル出力用ログ
                ' "RNo      MEAS1       MEAS2         MEAS3          DEVIAT(%)     RESULT"
                strLOG = strLOG + typResistorInfoArray(Rn + 1).intResNo.ToString("0").PadLeft(4) +
                         strDAT(0) + strDAT(1) + strDAT(2) + strDEV + strRESULT.PadLeft(8) + vbCrLf
                Console.WriteLine(strLOG)                               ' For Debug 

                ' 表示用ログ
                ' "RNo      MEAS1       MEAS2         MEAS3          DEVIAT(%)     RESULT"
                strDSP = strDSP + typResistorInfoArray(Rn + 1).intResNo.ToString("0").PadLeft(4) +
                         strDAT(0) + strDAT(1) + strDAT(2) + strDEV + strRESULT.PadLeft(8) + vbCrLf
#If cOFFLINEcDEBUG Then
               Console.WriteLine(strDSP)                               ' For Debug 
#End If
            Next Rn

            Return (r)                                                  ' Return

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "MainModules.Sub_ProbeResultCheck() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cFRS_NORMAL)
        End Try
    End Function
#End Region

#Region "プローブチェックログファイル名を設定する(プローブチェック機能)"
    '''=========================================================================
    ''' <summary>プローブチェックログファイル名を設定する</summary>
    ''' <param name="strDatPath">(INP)トリミングデータファイル名</param>
    '''=========================================================================
    Public Sub Gen_ProbeCheckLogFile(ByRef strDatPath As String)

        Dim Dt As DateTime = DateTime.Now                               ' 現在の日時を取得
        Dim TimeOfDt As TimeSpan = Dt.TimeOfDay                         ' 現在の時刻のみを取得 
        Dim strFileName As String
        Dim strMSG As String

        Try
            ' プローブチェック機能なし又はチェック枚数の指定なし又は自動運転中(SL436R)でないならNOP
            If ((giProbeCheck = 0) Or (typPlateInfo.intPrbChkPlt = 0) Or (bFgAutoMode <> True)) Then
                Return
            End If

            ' ログファイル名を設定する
            ' →「ProbeCheck_トリミングデータファイル名 + 日時(yyyymmddhhmmss).log」
            strPrbChkLogDate = Today.ToString("yyyyMMdd") + TimeOfDay.ToString("HHmmss")
            strFileName = GetFileNameNonExtension(strDatPath)           ' ﾄﾘﾐﾝｸﾞﾃﾞｰﾀﾌｧｲﾙ名の拡張子を抜かしたファイル名を取り出して返す
            strPrbChkLogFile = gSysPrm.stLOG.gsLoggingDir + "ProbeCheck_" + strFileName + strPrbChkLogDate + ".log"

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "MainModules.Gen_ProbeCheckLogFile() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "プローブチェックログ出力処理(プローブチェック機能)"
    '''=========================================================================
    ''' <summary>プローブチェックログ出力処理</summary>
    ''' <param name="strLOG">    (INP)ログデータ</param>
    ''' <returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Public Function Sub_ProbeCheckLogOut(ByRef strLOG As String) As Integer

        Dim hFStream As System.IO.FileStream
        'Dim writer As System.IO.StreamWriter
        'Dim Dt As DateTime = DateTime.Now                               ' 現在の日時を取得
        'Dim TimeOfDt As TimeSpan = Dt.TimeOfDay                         ' 現在の時刻のみを取得 
        Dim strMSG As String

        Try
            ' ログファイルの存在チェック
            If (System.IO.File.Exists(strPrbChkLogFile) = False) Then
                ' ファイルが存在しなければ空ファイルを生成する
                hFStream = System.IO.File.Create(strPrbChkLogFile)
                If (hFStream Is Nothing) Then
                    'ログファイルの書込みでエラーが発生しました File=xxxxxxxxxxxx"
                    strMSG = MSG_LOGERROR + " File=" + strPrbChkLogFile
                    Call Form1.Z_PRINT(strMSG)
                    Return (cFRS_FIOERR_OUT)                            ' Return値 = cFRS_FIOERR_OUT
                Else
                    hFStream.Close()
                End If
            End If

            ' ログファイルオープン
            '                                                           ' false = 上書き(true = 追加)
            'writer = New System.IO.StreamWriter(strPrbChkLogFile, True, System.Text.Encoding.GetEncoding("Shift_JIS"))
            Using writer As New StreamWriter(strPrbChkLogFile, True, Encoding.UTF8)     'V4.4.0.0-0
                ' ログデータを書き込む
                writer.WriteLine(strLOG)                                    ' ※"\r\n"付き　単純に文字列を書き込むには、Write() を使用する。

                '' 終了処理
                'writer.Close()                                              ' ログファイルクローズ
            End Using

            Return (cFRS_NORMAL)                                        ' Return値 = 正常

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "MainModules.Sub_ProbeCheckLogOut() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cFRS_NORMAL)
        End Try
    End Function
#End Region


#Region "DLLからのマガジンの停止" 'V5.0.0.1-26
    Public Sub sub_MGStopJog() Implements IMainModules.sub_MGStopJog    'V6.0.0.0①
        MGStopJog()
    End Sub
#End Region


#Region "Swap(Double)"
    '''=========================================================================
    ''' <summary>Swap(Double)</summary>
    ''' <param name="a"></param>
    ''' <param name="b"></param>
    ''' <remarks>a > bならaとbを入れ替える</remarks>
    '''=========================================================================
    Private Sub SwapDouble(ByRef a As Double, ByRef b As Double)

        Dim c As Double

        If (a > b) Then
            c = a
            a = b
            b = c
        End If

    End Sub
#End Region
    '----- V1.23.0.0⑦↑ -----
End Class
