'===============================================================================
'   Description  : オートプローブ用マトリックス画面処理
'
'   Copyright(C) : OMRON LASERFRONT INC. 2013
'
'===============================================================================

Imports LaserFront.Trimmer.DefTrimFnc
Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8①
Imports TKY_ALL_SL432HW.My.Resources    'V4.4.0.0-0

Module basMatrix
#Region "【変数定義】"
    '===========================================================================
    '   変数定義
    '===========================================================================
    '----- ラベルコントロールの表示サイズ -----
    Private Const LBLSZ_20 As Integer = 20
    Private Const LBLSZ_30 As Integer = 30
    Private Const LBLSZ_40 As Integer = 40

    '----- ラベルコントロールのバックカラー -----
    Private Const LBL_OK As Integer = 0
    Private Const LBL_NG As Integer = 1

    '----- マトリックスのブロック数 -----
    Private Const MAX_MATRIX As Integer = 7

    '----- 変数定義 -----
    Private mExitFlag As Integer                                ' 終了フラグ(Return値)
    Private gBpPosX As Double = 0.0                             ' BP位置X
    Private gBpPosY As Double = 0.0                             ' BP位置Y
    Private g1stPosX As Double = 0.0                            ' 中心座標X
    Private g1stPosY As Double = 0.0                            ' 中心座標Y
    Private gStgOfsX As Double = 0.0                            ' ステージオフセットX
    Private gStgOfsY As Double = 0.0                            ' ステージオフセットY

    Private giBlkCnt As Integer                                 ' ステップブロック数

    Private ObjMain As Object = Nothing                         ' 呼出元のオブジェクト
    Private LabelAry(,) As System.Windows.Forms.Label = Nothing ' マトリックス表示用ラベル配列 
    Private StgPosAryX(MAX_MATRIX) As Double                    ' ステージ座標X配列 
    Private StgPosAryY(MAX_MATRIX) As Double                    ' ステージ座標Y配列 
    Private SerchAryX(MAX_MATRIX * MAX_MATRIX) As Integer       ' ブロック番号X配列(検索用) 
    Private SerchAryY(MAX_MATRIX * MAX_MATRIX) As Integer       ' ブロック番号Y配列(検索用) 

    '----- オートプローブ実行用構造体(定義はMainModules.vb) -----
    'Public Structure AutoProbe_Info                             ' オートプローブ実行用構造体形式定義
    '    Dim intAProbeGroupNo1 As Short                          ' パターン1(下方)用グループ番号
    '    Dim intAProbePtnNo1 As Short                            ' パターン1(下方)用パターン番号
    '    Dim intAProbeGroupNo2 As Short                          ' パターン2(上方)用グループ番号
    '    Dim intAProbePtnNo2 As Short                            ' パターン2(上方)用パターン番号
    '    Dim dblAProbeBpPosX As Double                           ' パターン1(下方)用BP位置X
    '    Dim dblAProbeBpPosY As Double                           ' パターン1(下方)用BP位置Y
    '    Dim dblAProbeStgPosX As Double                          ' パターン2(上方)用ステージオフセット位置X
    '    Dim dblAProbeStgPosY As Double                          ' パターン1(下方)用ステージオフセット位置Y
    '    Dim intAProbeStepCount As Short                         ' ステップ実行用ステップ回数
    '    Dim intAProbeStepCount2 As Short                        ' ステップ実行用繰返しステップ回数
    '    Dim dblAProbePitch As Double                            ' ステップ実行用ステップピッチ
    '    Dim dblAProbePitch2 As Double                           ' ステップ実行用繰り返しステップピッチ
    'End Structure
#End Region

#Region "【メソッド定義】"
#Region "終了結果を返す"
    '''=========================================================================
    ''' <summary>終了結果を返す</summary>
    ''' <param name="OfsX">(OUT)ステージオフセットX</param>
    ''' <param name="OfsY">(OUT)ステージオフセットY</param>
    ''' <returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Public Function GetMatrixReturn(ByRef OfsX As Double, ByRef OfsY As Double) As Integer

        Dim strMSG As String

        Try
            ' 補正位置を返す
            OfsX = gStgOfsX
            OfsY = gStgOfsY

            ' mExitFlag=OK(STARTｷｰ)なら戻値=正常とする 
            If (mExitFlag = cFRS_ERR_START) Then mExitFlag = cFRS_NORMAL
            Return (mExitFlag)
            Exit Function

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "basMatrix.GetMatrixReturn() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Function
#End Region

#Region "ShowDialogメソッドに独自の引数を追加する"
    '''=========================================================================
    ''' <summary>ShowDialogメソッドに独自の引数を追加する</summary>
    ''' <param name="Obj">   (INP)呼出元のオブジェクト</param>
    ''' <returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Public Function ShowMatrixDialog(ByRef Obj As Object) As Integer

        Dim stPos As System.Drawing.Point
        Dim stGetPos As System.Drawing.Point
        Dim r As Integer = cFRS_NORMAL
        Dim strMSG As String

        Try
            ' 初期処理
            ObjMain = Obj
            mExitFlag = -1                                              ' 終了フラグ = 初期化

            ' 表示位置の調整
            stPos = Form1.chkDistributeOnOff.PointToScreen(stGetPos)
            stPos.X = stPos.X
            stPos.Y = stPos.Y - 20
            Form1.GrpMatrix.Location = stPos                            ' StartPositionプロパティを = Manualにしないと効かない ?

            ' マトリックス生成
            giBlkCnt = ObjMain.stAPRB.intAProbeStepCount * 2 + 1        ' ブロック数 = ステップ回数 * 2 + 1 
            Call MatrixGenerate(giBlkCnt, ObjMain.stAPRB.intAProbeStepCount)

            ' ブロック番号配列を初期化する
            Call InitSerchAry(giBlkCnt, ObjMain.stAPRB.intAProbeStepCount)

            ' 画面表示
            Form1.GrpMatrix.Visible = True                              ' マトリックス画面表示
            Form1.GrpMatrix.BringToFront()                              ' 最前面に表示

            ' 画面処理メイン
            r = FrmMatrix_Main()
            mExitFlag = r                                               ' mExitFlagに戻り値を設定する 
            Return (r)

            Exit Function

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "basMatrix.ShowMatrixDialog() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Function
#End Region

#Region "画面処理メイン"
    '''=========================================================================
    ''' <summary>画面処理メイン</summary>
    ''' <returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function FrmMatrix_Main() As Integer

        Dim r As Integer = cFRS_NORMAL
        Dim rtn As Integer = cFRS_NORMAL
        'Dim RptCnt As Integer
        Dim StgOfsX As Double
        Dim StgOfsY As Double
        Dim StgPosX As Double
        Dim StgPosY As Double
        Dim SvStgPosX As Double
        Dim SvStgPosY As Double
        Dim SvBpPosX As Double
        Dim SvBpPosY As Double
        Dim fCorrectX As Double
        Dim fCorrectY As Double
        Dim fCoeff As Double = 0.0                                      ' 相関値
        'Dim RptPit As Double
        Dim strMSG As String

        Try
            '-------------------------------------------------------------------
            '   初期処理
            '-------------------------------------------------------------------
            ' マトリックス表示
            Form1.GrpMatrix.Refresh()

            ' 現在座標退避 
            Call ZGETBPPOS(SvBpPosX, SvBpPosY)
            Call ZGETPHPOS2(SvStgPosX, SvStgPosY)

            ' プローブコマンド時はステップ測定を行う
            If (giAppMode = APP_MODE_PROBE) Then
                StgPosX = SvStgPosX
                StgPosY = SvStgPosY
                GoTo STP_STPMEAS
            End If

            ' ブロックサイズ設定
            r = Form1.System1.EX_BSIZE(gSysPrm, typPlateInfo.dblBlockSizeXDir, typPlateInfo.dblBlockSizeYDir)
            If (r <> cFRS_NORMAL) Then                                  ' エラー ?
                Return (r)
            End If
            ' BPオフセット設定(※ブロックサイズを設定するとBPオフセットはINtime側で初期化される)
            r = Form1.System1.EX_BPOFF(gSysPrm, typPlateInfo.dblBpOffSetXDir, typPlateInfo.dblBpOffSetYDir)
            If (r <> cFRS_NORMAL) Then                                  ' エラー ?
                Return (r)
            End If

            ' BP位置にパターン1(下方)用BP位置を設定する
            gBpPosX = ObjMain.stAPRB.dblAProbeBpPosX
            gBpPosY = ObjMain.stAPRB.dblAProbeBpPosY

            '-------------------------------------------------------------------
            '   パターン認識(下方)を実行する
            '-------------------------------------------------------------------
            ' XYテーブルを原点へ移動する
            r = Form1.System1.EX_SMOVE2(gSysPrm, 0.0, 0.0)
            If (r <> cFRS_NORMAL) Then                                  ' エラー ?
                Return (r)
            End If

            ' Z2をON位置に移動する
            If IsUnderProbe() Then                                      ' Z2有り ? 
                r = Z2move(Z2ON)                                        ' Z2をON位置に移動する
                If (r <> cFRS_NORMAL) Then                              ' エラー ? (メッセージは表示済み)
                    Return (r)
                End If
                Call ZSTOPSTS2()                                        ' Z2軸動作停止待ち
            End If
            Call System.Threading.Thread.Sleep(300)                     ' Wait(msec)

            ' 上方カメラで下方フローブ上のパターン認識を実行する
            r = Sub_PatternMatching(ObjMain.stAPRB.intAProbeGroupNo1, ObjMain.stAPRB.intAProbePtnNo1, gBpPosX, gBpPosY, fCorrectX, fCorrectY, fCoeff)
            If (r <> cFRS_NORMAL) Then                                  ' エラー ? ※パターン認識エラーになるとブロックサイズが0となっているので注意
                GoTo STP_EXIT
            End If

            '-------------------------------------------------------------------
            '   パターン認識(上方)を実行する
            '-------------------------------------------------------------------
            ' Z2をOFF位置に移動する
            If IsUnderProbe() Then                                      ' Z2有り ? 
                r = Z2move(Z2OFF)                                       ' Z2をOFF位置に移動する
                If (r <> cFRS_NORMAL) Then                              ' エラー ? (メッセージは表示済み)
                    Return (r)
                End If
                Call ZSTOPSTS2()                                        ' Z2軸動作停止待ち
            End If

            ' ずれ量分BP位置を補正する(fCorrectX,Yは基準コーナーを考慮済み)
            gBpPosX = gBpPosX + fCorrectX
            gBpPosY = gBpPosY + fCorrectY

            ' ステージ座標を求める
            Call GetAProbStagePosition(StgOfsX, StgOfsY)

            ' XYテーブルをパターン認識位置へ移動(トリム位置＋ステージオフセット＋Θ補正値+上方用オフセット)
            'V6.0.0.0-28            Call Form1.VideoLibrary1.VideoStop()                        ' ステージ移動前にビデオの更新処理を一旦停止
            r = Form1.System1.EX_SMOVE2(gSysPrm, StgOfsX, StgOfsY)
            If (r <> cFRS_NORMAL) Then                                  ' エラー ? (メッセージは表示済み)
                Return (r)
            End If
            Call System.Threading.Thread.Sleep(300)                     ' Wait(msec)
            'V6.0.0.0-28            Call Form1.VideoLibrary1.VideoStart()                       ' ステージ移動後にビデオの更新処理を再実施

            ' 上方カメラで基板上のパターン認識を実行する(ずれ量→fCorrectX, fCorrectY)
            r = Sub_PatternMatching(ObjMain.stAPRB.intAProbeGroupNo2, ObjMain.stAPRB.intAProbePtnNo2, gBpPosX, gBpPosY, fCorrectX, fCorrectY, fCoeff)
            If (r <> cFRS_NORMAL) Then                                  ' エラー ? 
                Return (r)
            End If

            ' 画像表示プログラムを起動する
            ' ↓↓↓ V3.1.0.0② 2014/12/01
            'r = Exec_GazouProc(ObjGazou, DISPGAZOU_PATH, DISPGAZOU_WRK, 0)
            'V6.0.0.0⑤            r = Exec_GazouProc(ObjGazou, DISPGAZOU_PATH, DISPGAZOU_WRK, 0, 0)
            ' ↑↑↑ V3.1.0.0② 2014/12/01

            ' ステージ座標を求める(測定位置)
            Call GetAProbStageMeasPosition(StgOfsX, StgOfsY)

            ' ずれ量分ステージを移動する
            'fCorrectX = fCorrectX * -1                                  ' BPでなくステージ移動なので符号を反転する
            fCorrectX = fCorrectX
            fCorrectY = fCorrectY * -1
            r = Form1.System1.EX_SMOVE2(gSysPrm, StgOfsX + fCorrectX, StgOfsY + fCorrectY)
            If (r <> cFRS_NORMAL) Then                                  ' エラー ? 
                rtn = r
                GoTo STP_ERR_EXIT
            End If
            Call ZGETPHPOS2(StgPosX, StgPosY)                           ' 中心座標退避 

            '-------------------------------------------------------------------
            '   ステップ測定を行う
            '-------------------------------------------------------------------
STP_STPMEAS:
            'RptPit = ObjMain.stAPRB.dblAProbePitch2                     ' ステップ測定繰返しステップピッチ
            'For RptCnt = 1 To ObjMain.stAPRB.intAProbeStepCount2        ' ステップ測定繰返しステップ回数分繰返す
            ' マトリックス表示初期化
            Call MatrixInit(giBlkCnt)

            ' ステップ回数分抵抗測定を行う
            r = StepMeasure(giBlkCnt, StgPosX, StgPosY, ObjMain.stAPRB.dblAProbePitch)
            If (r = cFRS_ERR_RST) Then GoTo STP_EXIT '                  ' Cancel(RESETキー)
            If (r < cFRS_NORMAL) Then                                   ' 非常停止等 ?
                rtn = r
                GoTo STP_ERR_EXIT
            End If

            'Next RptCnt

            ' 中心座標に近い、測定OKのステージオフセットを検索する
            gStgOfsX = 0.0                                               ' ステージオフセット初期化
            gStgOfsY = 0.0
            If (r = cFRS_NORMAL) Then                                   ' 測定OK ? 
                ' ステージオフセットを検索する
                Call SerchStageOffset(giBlkCnt, gStgOfsX, gStgOfsY)

                ' オートプローブ実行コマンド時なら補正値を表示する
                If (giAppMode = APP_MODE_APROBEEXE) Then
                    'If (gSysPrm.stTMN.giMsgTyp = 0) Then
                    '    strMSG = "オートプローブ実行 補正値X=" + gStgOfsX.ToString("0.0000") + ", 補正値Y=" + gStgOfsY.ToString("0.0000")
                    'Else
                    '    strMSG = "Auto Prob Execute. X=" + gStgOfsX.ToString("0.0000") + ", Y=" + gStgOfsY.ToString("0.0000")
                    'End If
                    strMSG = basMatrix_001 & gStgOfsX.ToString("0.0000") & basMatrix_002 & gStgOfsY.ToString("0.0000")
                    Call Form1.Z_PRINT(strMSG)
                End If
            End If

            '-------------------------------------------------------------------
            '   終了処理
            '-------------------------------------------------------------------
            ' 画像表示プログラムを終了する
            'V6.0.0.0⑤            Call End_GazouProc(ObjGazou)
            Return (cFRS_NORMAL)

STP_EXIT:
            ' パターン認識エラー等、アプリ強制終了しない場合はＸＹステージを元の位置に移動する
            rtn = r                                                     ' Return値退避
            If (r >= cFRS_NORMAL) Or ((r >= cFRS_ERR_PT2) And (r <= cFRS_ERR_PTN)) Then
                ' Z, Z2をOff位置に移動する
                r = Sub_ZOnOff(0)
                If (r <> cFRS_NORMAL) Then                              ' エラー ? 
                    rtn = r
                    GoTo STP_ERR_EXIT
                End If

                ' BPを元の位置に移動する(絶対移動)
                r = Form1.System1.EX_MOVE(gSysPrm, SvBpPosX, SvBpPosY, 1)
                If (r <> cFRS_NORMAL) Then                              ' エラー ?(メッセージは表示済み) 
                    rtn = r
                    GoTo STP_ERR_EXIT
                End If

                ' ＸＹステージを元の位置に移動する(絶対移動)
                r = Form1.System1.EX_SMOVE2(gSysPrm, SvStgPosX, SvStgPosY)
                If (r <> cFRS_NORMAL) Then                              ' エラー ? 
                    rtn = r
                    GoTo STP_ERR_EXIT
                End If
            End If

STP_ERR_EXIT:
            ' 画像表示プログラムを終了する
            'V6.0.0.0⑤            Call End_GazouProc(ObjGazou)
            Return (rtn)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "basMatrix.FrmMatrix_Main() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Function
#End Region

#Region "マトリックス生成"
    '''=========================================================================
    ''' <summary>マトリックス生成</summary>
    ''' <param name="BlkCnt">  (INP)ブロック数</param>
    ''' <param name="StpCount">(INP)ステップ回数</param>
    '''=========================================================================
    Private Sub MatrixGenerate(ByVal BlkCnt As Integer, ByVal StpCount As Integer)

        Dim BlkX As Integer
        Dim BlkY As Integer
        Dim LblSz As Integer
        Dim strMSG As String

        Try
            ' ラベルコントロールの表示サイズを設定する
            Select Case (StpCount)
                Case 3
                    LblSz = LBLSZ_20
                Case 2
                    LblSz = LBLSZ_30
                Case Else
                    LblSz = LBLSZ_40
            End Select

            ' ラベルコントロール配列の作成（Xブロック,Yブロック）
            If (LabelAry Is Nothing = False) Then
                BlkX = LabelAry.GetLength(0)
                BlkY = LabelAry.GetLength(1)
                For BlkX = 0 To (LabelAry.GetLength(0) - 1)
                    For BlkY = 0 To (LabelAry.GetLength(1) - 1)
                        Form1.GrpMatrix.Controls.Remove(LabelAry(BlkX, BlkY))
                    Next BlkY
                Next BlkX
            End If
            LabelAry = New System.Windows.Forms.Label(BlkCnt, BlkCnt) {}

            ' ラベルコントロールのインスタンス作成し、プロパティを設定する
            For BlkX = 1 To BlkCnt
                For BlkY = 1 To BlkCnt
                    'インスタンス作成
                    LabelAry(BlkX, BlkY) = New System.Windows.Forms.Label
                    'プロパティ設定
                    LabelAry(BlkX, BlkY).Name = "LblMatrix" + BlkX.ToString("00") + BlkY.ToString("00")
                    LabelAry(BlkX, BlkY).BorderStyle = BorderStyle.Fixed3D
                    LabelAry(BlkX, BlkY).Size = New Size(LblSz, LblSz)
                    LabelAry(BlkX, BlkY).Location = New Point(BlkX * LblSz, BlkY * LblSz)
                    LabelAry(BlkX, BlkY).BackColor = Color.White
                    ' フォームにコントロールを追加
                    Form1.GrpMatrix.Controls.Add(LabelAry(BlkX, BlkY))
                Next BlkY
            Next BlkX

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "basMatrix.MatrixGenerate() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "マトリックス表示初期化"
    '''=========================================================================
    ''' <summary>マトリックス表示初期化</summary>
    ''' <param name="BlkCnt">(INP)ブロック数</param>
    '''=========================================================================
    Private Sub MatrixInit(ByVal BlkCnt As Integer)

        Dim BlkX As Integer
        Dim BlkY As Integer
        Dim strMSG As String

        Try
            ' マトリックス表示初期化
            For BlkX = 1 To BlkCnt
                For BlkY = 1 To BlkCnt
                    LabelAry(BlkX, BlkY).BackColor = Color.White
                Next BlkY
            Next BlkX
            Form1.GrpMatrix.Refresh()

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "basMatrix.MatrixInit() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "マトリックスのバックカラーを設定する"
    '''=========================================================================
    ''' <summary>マトリックスのバックカラーを設定する</summary>
    ''' <param name="BlkX">(INP)マトリックスのブロックX</param>
    ''' <param name="BlkY">(INP)マトリックスのブロックY</param>
    ''' <param name="OkNg">(INP)0=LBL_OK, 1=LBL_NG</param>
    '''=========================================================================
    Private Sub MatrixDisp(ByVal BlkX As Integer, ByVal BlkY As Integer, ByVal OkNg As Integer)

        Dim strMSG As String

        Try
            ' マトリックスのバックカラーを設定する
            If (OkNg = LBL_OK) Then
                LabelAry(BlkX, BlkY).BackColor = Color.LimeGreen
            Else
                LabelAry(BlkX, BlkY).BackColor = Color.Red
            End If
            Form1.GrpMatrix.Refresh()

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "basMatrix.MatrixDisp() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "パターンマッチングにより１抵抗分の位置補正を求める"
    '''=========================================================================
    ''' <summary>パターンマッチングにより１抵抗分の位置補正を求める</summary>
    ''' <param name="Grp">      (INP)グループ番号</param>
    ''' <param name="Ptn">      (INP)パターン番号</param>
    ''' <param name="fCorrectX">(OUT)ずれ量X</param> 
    ''' <param name="fCorrectY">(OUT)ずれ量Y</param> 
    ''' <param name="Coeff">    (OUT)一致度</param> 
    ''' <returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function Sub_PatternMatching(ByVal Grp As Short, ByVal Ptn As Short, _
                                         ByVal BpX As Double, ByVal BpY As Double, _
                                         ByRef fCorrectX As Double, ByRef fCorrectY As Double, ByRef Coeff As Double) As Integer

        Dim ret As Short = cFRS_NORMAL
        Dim crx As Double = 0.0                                         ' ずれ量X
        Dim cry As Double = 0.0                                         ' ずれ量Y
        Dim fCoeff As Double = 0.0                                      ' 相関値
        Dim Thresh As Double = 0.0                                      ' 閾値
        Dim r As Integer = cFRS_NORMAL                                  ' 関数値
        Dim strMSG As String

        Try
#If VIDEO_CAPTURE = 1 Then
        fCorrectX = 0.0
        fCorrectY = 0.0
        Return (cFRS_NORMAL)   
#End If
            ' パターンマッチング時のテンプレートグループ番号を設定する(毎回やると遅くなる)
            'If (giTempGrpNo <> Grp) Then                                ' テンプレートグループ番号が変わった ?
            '    giTempGrpNo = Grp                                       ' 現在のテンプレートグループ番号を退避
            '    Form1.VideoLibrary1.SelectTemplateGroup(giTempGrpNo)    ' テンプレートグループ番号設定
            'End If
            giTempGrpNo = Grp                                       ' 現在のテンプレートグループ番号を退避
            Form1.VideoLibrary1.SelectTemplateGroup(giTempGrpNo)    ' テンプレートグループ番号設定

            ' 閾値取得
            Thresh = gDllSysprmSysParam_definst.GetPtnMatchThresh(giTempGrpNo, Ptn)

            ' パーターン位置XYへBP移動(絶対値)
            r = Form1.System1.EX_MOVE(gSysPrm, BpX, BpY, 1)
            If (r <> cFRS_NORMAL) Then                                  ' エラー ?
                Return (r)
            End If
            Form1.System1.WAIT(0.3)                                     ' Wait(Sec)
            Call Form1.VideoLibrary1.PatternDisp(True)                  ' 検索範囲枠表示 

            ' パターンマッチングを行う(Video.ocxを使用)
            ret = Form1.VideoLibrary1.PatternMatching_EX(Ptn, 0, True, crx, cry, fCoeff)
            If (ret <> cFRS_NORMAL) Then
                r = cFRS_ERR_PTN                                        ' RETURN値 = パターンマッチィングエラー
            ElseIf (fCoeff < Thresh) Then                               ' 一致度
                r = cFRS_ERR_PTN                                        ' RETURN値 = パターンマッチィングエラー
            Else
                ' マッチしたパターンの測定位置からずれ量を求める
                fCorrectX = crx
                fCorrectY = cry
                Coeff = fCoeff                                          ' 一致度
                r = cFRS_NORMAL                                         ' Return値 = 正常
            End If

            ' 後処理
            Debug.Print("X=" & fCorrectX.ToString("0.0000") & " Y=" & fCorrectY.ToString("0.0000"))
            Call Form1.VideoLibrary1.PatternDisp(False)                 ' 検索範囲枠非表示 

            ' パターン認識エラーならメッセージ表示(STARTキー押下待ち)
            If (r <> cFRS_NORMAL) Then                                  ' パターン認識エラー ? 
                '   ' ローダが自動モード時はログ表示域にメッセージ表示する
                If (giHostMode = cHOSTcMODEcAUTO) Then
                    ' "パターン認識エラー Group No.=x, Pattern No.=x"
                    strMSG = MSG_127 + " Group No.=" + giTempGrpNo.ToString("0") + ", Pattern No.=" + Ptn.ToString("0")
                    Call Form1.Z_PRINT(strMSG)

                    ' 手動モード時はメッセージ表示してSTARTキー押下待ち
                Else
                    ' "パターン認識エラー", "Group No.=x, Pattern No.=x"
                    strMSG = "Group No.=" + giTempGrpNo.ToString("0") + ", Pattern No.=" + Ptn.ToString("0")
                    ret = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_START, True, _
                            MSG_127, strMSG, "", System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
                    If (ret < cFRS_NORMAL) Then Return (ret) '          ' 非常停止等のエラーならエラー戻り(エラーメッセージは表示済み) 
                End If

            End If

            Return (r)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "basMatrix.CutPosPatternMatching() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return値 = 例外エラー
        End Try
    End Function
#End Region

#Region "ステップ回数分抵抗測定を行う"
    '''=========================================================================
    ''' <summary>ステップ回数分抵抗測定を行う</summary>
    ''' <param name="BlkCnt">(INP)ブロック数</param>
    ''' <param name="CtPosX">(INP)中心座標X</param>
    ''' <param name="CtPosY">(INP)中心座標Y</param>
    ''' <param name="Pitch"> (INP)移動ピッチ(mm)</param>
    ''' <returns>0    = 正常(測定OK)
    '''          1    = 全抵抗測定NG
    '''          0以下= エラー</returns>
    '''=========================================================================
    Private Function StepMeasure(ByVal BlkCnt As Integer, ByVal CtPosX As Double, ByVal CtPosY As Double, ByVal Pitch As Double) As Integer

        Dim bFlg As Boolean = False
        Dim r As Integer
        Dim rtn As Integer = cFRS_NORMAL
        Dim BlkX As Integer
        Dim BlkY As Integer
        Dim EndY As Integer
        Dim StepY As Integer
        Dim sts As Long = 0
        Dim StgPosX As Double
        Dim StgPosY As Double
        Dim StartPosX As Double
        Dim StartPosY As Double
        Dim strMSG As String

        Try
            '-------------------------------------------------------------------
            '   初期処理
            '-------------------------------------------------------------------
            ' 左上位置ポジション算出
            'If (giAppMode = APP_MODE_PROBE) Then                        ' プローブコマンド時は現在座標から開始する
            '    StgPosX = CtPosX
            '    StgPosY = CtPosY
            'Else                                                        ' 左上位置ポジション算出
            '    StgPosX = CtPosX + Pitch * ((BlkCnt - 1) / 2)
            '    StgPosY = CtPosY - Pitch * ((BlkCnt - 1) / 2)
            'End If
            StgPosX = CtPosX + Pitch * ((BlkCnt - 1) / 2)
            StgPosY = CtPosY - Pitch * ((BlkCnt - 1) / 2)

            strMSG = StgPosX.ToString("0.0000")
            StgPosX = Double.Parse(strMSG)
            strMSG = StgPosY.ToString("0.0000")
            StgPosY = Double.Parse(strMSG)
            StartPosX = StgPosX                                         ' 開始座標XYを退避 
            StartPosY = StgPosY

            ' 画像表示プログラムを起動する(未起動時)
            'V6.0.0.0⑤            If (ObjGazou Is Nothing) Then
            ' ↓↓↓ V3.1.0.0② 2014/12/01
            'r = Exec_GazouProc(ObjGazou, DISPGAZOU_PATH, DISPGAZOU_WRK, 0)
            'V6.0.0.0⑤                r = Exec_GazouProc(ObjGazou, DISPGAZOU_PATH, DISPGAZOU_WRK, 0, 0)
            ' ↑↑↑ V3.1.0.0② 2014/12/01
            'V6.0.0.0⑤            End If

            ' ステップ回数分抵抗測定を行う
            Call ZCONRST()                                              ' コンソールキーラッチ解除
            For BlkX = 1 To BlkCnt                                      ' Xブロック数分繰り返す
                ' ステージの移動座標Xを設定する
                StgPosX = StartPosX - Pitch * (BlkX - 1)                ' Xは常に←方向へ移動する 

                ' Y方向のステップ方向を設定する
                If ((BlkX Mod 2) <> 0) Then                             ' Xが奇数ブロックならYは↓方向 
                    BlkY = 1
                    EndY = BlkCnt
                    StepY = 1
                Else                                                    ' Xが偶数ブロックならYは↑方向 
                    BlkY = BlkCnt
                    EndY = 1
                    StepY = -1
                End If

                ' Yブロック数分繰り返す 
                For BlkY = BlkY To EndY Step StepY                      ' Yは↑方向又は↓方向へ移動する 
                    ' システムエラーチェック
                    r = Form1.System1.SysErrChk_ForVBNET(giAppMode)
                    If (r <> cFRS_NORMAL) Then                          ' 非常停止等 ?
                        rtn = r
                        GoTo STP_ERR_EXIT
                    End If

                    ' HALTキー押下チェック
                    Call HALT_SWCHECK(sts)
                    If (sts = cSTS_HALTSW_ON) Then
                        ' Z,Z2をOff位置に移動する
                        r = Sub_ZOnOff(0)
                        If (r <> cFRS_NORMAL) Then                      ' エラー ? 
                            rtn = r
                            GoTo STP_ERR_EXIT
                        End If

                        ' ランプ制御
                        Call LAMP_CTRL(LAMP_START, True)                ' STARTランプON
                        Call LAMP_CTRL(LAMP_RESET, True)                ' RESETランプON

                        ' メッセージ表示(STARTキー/RESETキー押下待ち)
                        '  "一時停止中です" "STARTキー：処理続行，RESETキー：処理終了" 
                        r = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_START + cFRS_ERR_RST, True, _
                                MSG_SPRASH39, MSG_SPRASH35, "", System.Drawing.Color.Blue, System.Drawing.Color.Black, System.Drawing.Color.Black)
                        If (r < cFRS_NORMAL) Then                       ' 非常停止等のエラーならエラー戻り(エラーメッセージは表示済み)               
                            rtn = r
                            GoTo STP_ERR_EXIT
                        ElseIf (r = cFRS_ERR_RST) Then
                            rtn = cFRS_ERR_RST
                            Call ZCONRST()                              ' コンソールキーラッチ解除
                            GoTo STP_EXIT
                        End If

                        ' ランプ制御
                        Call LAMP_CTRL(LAMP_START, False)               ' STARTランプOFF
                        Call LAMP_CTRL(LAMP_RESET, False)               ' RESETランプOFF
                        Call ZCONRST()                                  ' コンソールキーラッチ解除
                        Form1.Refresh()                                 ' メッセージFormを消す為 
                    End If

                    ' Z,Z2をOff位置に移動する
                    r = Sub_ZOnOff(0)
                    If (r <> cFRS_NORMAL) Then                          ' エラー ? 
                        rtn = r
                        GoTo STP_ERR_EXIT
                    End If

                    ' ＸＹステージの移動座標Yを設定する
                    StgPosY = StartPosY + Pitch * (BlkY - 1)

                    ' ＸＹステージを移動する(絶対移動)
                    Console.WriteLine("BlkX=" + BlkX.ToString("0") + ", BlkY=" + BlkY.ToString("0"))
                    Console.WriteLine("StgPosX=" + StgPosX.ToString("0.0000") + ", StgPosY=" + StgPosY.ToString("0.0000"))
                    r = Form1.System1.EX_SMOVE2(gSysPrm, StgPosX, StgPosY)
                    If (r <> cFRS_NORMAL) Then                          ' エラー ? 
                        rtn = r
                        GoTo STP_ERR_EXIT
                    End If
                    System.Threading.Thread.Sleep(50)                   ' Wait(ms)

                    ' Z,Z2をOn位置に移動する
                    r = Sub_ZOnOff(1)
                    If (r <> cFRS_NORMAL) Then                          ' エラー ? 
                        rtn = r
                        GoTo STP_ERR_EXIT
                    End If

                    ' 全抵抗測定を行う
                    r = MeasureAllResistors()
                    If (r < cFRS_NORMAL) Then                           ' エラー ? 
                        rtn = r
                        GoTo STP_ERR_EXIT
                    End If
                    If (bFlg = False) And (r = cFRS_NORMAL) Then
                        bFlg = True                                     ' bFlg = 測定OK 
                    End If

                    ' マトリックスのバックカラーを設定する(測定OK=緑, 測定NG=赤)
                    Call MatrixDisp(BlkX, BlkY, r)

                    ' ステージ座標オフセットX,Y退避
                    strMSG = (StgPosX - CtPosX).ToString("0.0000")      ' 中心座標からのオフセット値を設定 
                    StgPosAryX(BlkX) = Double.Parse(strMSG)
                    strMSG = (StgPosY - CtPosY).ToString("0.0000")
                    StgPosAryY(BlkY) = Double.Parse(strMSG)

                Next BlkY

            Next BlkX

            '-------------------------------------------------------------------
            '   終了処理
            '-------------------------------------------------------------------
STP_EXIT:
            ' Z,Z2をOff位置に移動する
            r = Sub_ZOnOff(0)
            If (r <> cFRS_NORMAL) Then                                  ' エラー ? 
                rtn = r
                GoTo STP_ERR_EXIT
            End If

            ' ＸＹステージを元の位置(中心)に移動する(絶対移動)
            r = Form1.System1.EX_SMOVE2(gSysPrm, CtPosX, CtPosY)
            If (r <> cFRS_NORMAL) Then                                  ' エラー ? 
                rtn = r
                GoTo STP_ERR_EXIT
            End If
            System.Threading.Thread.Sleep(100)                          ' Wait(ms)

            ' 戻り値を設定
            If (rtn <> cFRS_ERR_RST) Then
                If (bFlg = True) Then
                    rtn = cFRS_NORMAL                                   ' Return値 = 測定OKとなった抵抗がある
                Else
                    rtn = 1                                             ' Return値 = 全抵抗測定NG
                End If
            End If

STP_ERR_EXIT:
            ' 画像表示プログラムを終了する
            'V6.0.0.0⑤            Call End_GazouProc(ObjGazou)

            Return (rtn)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "basMatrix.StepMeasure() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return値 = 例外エラー
        End Try
    End Function
#End Region

#Region "全抵抗測定を行う"
    '''=========================================================================
    ''' <summary>全抵抗測定を行う</summary>
    ''' <returns>0=測定OK
    '''          1=測定NG 
    '''          0,1以外=エラー</returns>
    '''=========================================================================
    Private Function MeasureAllResistors() As Integer

        Dim r As Integer = cFRS_NORMAL
        Dim Rn As Integer
        Dim Hp As Integer
        Dim Lp As Integer
        Dim Ag1 As Integer
        Dim Ag2 As Integer
        Dim Ag3 As Integer
        Dim Ag4 As Integer
        Dim Ag5 As Integer
        Dim Slp As Integer
        Dim RangeType As Integer                                        ' レンジ設定タイプ（0:オートレンジ、1:固定レンジ-目標値指定、2:固定レンジ-レンジ番号指定）
        Dim TarGetVal As Double
        Dim MeasVal As Double
        Dim gfInitH As Double
        Dim gfInitL As Double
        Dim strMSG As String

        Try
            ' 全抵抗測定(固定レンジ-目標値指定)
            For Rn = 1 To gRegistorCnt                                  ' 抵抗数分設定する

                ' システムエラーチェック
                r = Form1.System1.SysErrChk_ForVBNET(giAppMode)
                If (r <> cFRS_NORMAL) Then                              ' 非常停止等 ?
                    Return (r)
                End If

                ' マーキング用抵抗以外 ?
                If (typResistorInfoArray(Rn).intResNo < 1000) Then
                    ' プローブ番号(H,L,G1～G5)
                    Hp = typResistorInfoArray(Rn).intProbHiNo
                    Lp = typResistorInfoArray(Rn).intProbLoNo
                    Ag1 = typResistorInfoArray(Rn).intProbAGNo1
                    Ag2 = typResistorInfoArray(Rn).intProbAGNo2
                    Ag3 = typResistorInfoArray(Rn).intProbAGNo3
                    Ag4 = typResistorInfoArray(Rn).intProbAGNo4
                    Ag5 = typResistorInfoArray(Rn).intProbAGNo5
                    ' 目標値
                    TarGetVal = typResistorInfoArray(Rn).dblTrimTargetVal
                    ' 電圧変化スロープ(1:+スロープ, 2:-スロープ, 4:抵抗)
                    Slp = typResistorInfoArray(Rn).intSlope
                    If (typResistorInfoArray(Rn).intResMeasMode = 0) Then ' 測定モード(0:抵抗 ,1:電圧 ,2:外部)
                        Slp = 4
                    End If

                    ' 抵抗/電圧測定を行う
                    'RangeType = 1                                       ' 固定レンジ-目標値指定
                    RangeType = 0                                       ' オートレンジ
                    If (Slp = 4) Then                                   ' 電圧変化ｽﾛｰﾌﾟ = 4(R) ?
                        ' 抵抗測定
                        r = MFSET_EX("R", TarGetVal)
                        Call MSCAN(Hp, Lp, Ag1, Ag2, Ag3, Ag4, Ag5)     ' スキャナー番号設定
                        r = MEASURE(0, RangeType, 0, TarGetVal, 0, MeasVal)
                    ElseIf (Slp = 1) Or (Slp = 2) Then
                        ' 電圧測定 
                        r = MFSET_EX("V", TarGetVal)
                        Call MSCAN(Hp, Lp, Ag1, Ag2, Ag3, Ag4, Ag5)     ' スキャナー番号設定
                        r = MEASURE(1, RangeType, 0, TarGetVal, 0, MeasVal)
                    End If
                    ' 測定エラー
                    If ((r <> cFRS_NORMAL) And (r <> ERR_MEAS_SPAN_SHORT) And (r <> ERR_MEAS_SPAN_OVER)) Then
                        Return (r)                                      ' Return値 = エラー
                    End If

                    ' 判定値を算出
                    gfInitH = TarGetVal * (1.0# + typResistorInfoArray(Rn).dblInitTest_HighLimit / 100.0#)
                    gfInitL = TarGetVal * (1.0# + typResistorInfoArray(Rn).dblInitTest_LowLimit / 100.0#)

                    ' イニシャルHigh OR イニシャルLow 判定
                    If (MeasVal > gfInitH) Or (MeasVal < gfInitL) Then
                        Return (1)                                      ' Return値 = NG
                    End If
                End If
            Next Rn

            ' 終了処理
            Return (cFRS_NORMAL)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "basMatrix.MeasureAllResistors() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Function
#End Region

#Region "中心座標に近い、測定OKのステージオフセットを検索する"
    '''=========================================================================
    ''' <summary>中心座標に近い、測定OKのステージオフセットを検索する</summary>
    ''' <param name="BlkCnt"> (INP)ブロック数</param>
    ''' <param name="StgOfsX">(OUT)ステージオフセットX</param>
    ''' <param name="StgOfsY">(OUT)ステージオフセットY</param>
    '''=========================================================================
    Private Sub SerchStageOffset(ByVal BlkCnt As Integer, ByRef StgOfsX As Double, ByRef StgOfsY As Double)

        Dim Count As Integer
        Dim BlkX As Integer
        Dim BlkY As Integer

        Dim strMSG As String

        Try
            ' 初期処理
            StgOfsX = 0.0                                               ' ステージオフセット初期化
            StgOfsY = 0.0

            ' 中心座標から渦巻き状に検索する
            For Count = 1 To BlkCnt * BlkCnt                            ' ステップブロック数分繰り返す
                ' ブロック番号X,Yを求める
                Call GetBlkNum(Count, BlkX, BlkY)
                ' 測定OKとなったブロック位置を検索する
                If (LabelAry(BlkX, BlkY).BackColor = Color.LimeGreen) Then
                    StgOfsX = StgPosAryX(BlkX)
                    StgOfsY = StgPosAryY(BlkY)
                    Console.WriteLine("SerchStageOffset() BlkX=" + BlkX.ToString("0") + ", BlkY=" + BlkY.ToString("0") + ", OfsX=" + StgOfsX.ToString("0.0000") + ", OfsY=" + StgOfsY.ToString("0.0000"))
                    Exit Sub
                End If
            Next Count

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "basMatrix.SerchStageOffset() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "ブロック番号配列を初期化する"
    '''=========================================================================
    ''' <summary>ブロック番号配列を初期化する</summary>
    ''' <param name="BlkCnt">  (INP)ブロック数</param>
    ''' <param name="StpCount">(INP)ステップ回数</param>
    ''' <remarks>※中心から渦巻き状に検索する為の配列。
    ''' 　　　　　 計算で求めるように時間があれば変更する</remarks>
    '''=========================================================================
    Private Sub InitSerchAry(ByVal BlkCnt As Integer, ByVal StpCount As Integer)

        Dim strMSG As String

        Try
            Select Case (StpCount)
                Case 3
                    SerchAryX(1) = 4                                    ' 1　 X=4,Y=4
                    SerchAryY(1) = 4
                    SerchAryX(2) = 5                                    ' 2　 X=5,Y=4
                    SerchAryY(2) = 4
                    SerchAryX(3) = 5                                    ' 3　 X=5,Y=3
                    SerchAryY(3) = 3
                    SerchAryX(4) = 4                                    ' 4　 X=4,Y=3
                    SerchAryY(4) = 3
                    SerchAryX(5) = 3                                    ' 5　 X=3,Y=3
                    SerchAryY(5) = 3
                    SerchAryX(6) = 3                                    ' 6　 X=3,Y=4
                    SerchAryY(6) = 4
                    SerchAryX(7) = 3                                    ' 7　 X=3,Y=5
                    SerchAryY(7) = 5
                    SerchAryX(8) = 4                                    ' 8　 X=4,Y=5
                    SerchAryY(8) = 5
                    SerchAryX(9) = 5                                    ' 9　 X=5,Y=5
                    SerchAryY(9) = 5
                    SerchAryX(10) = 6                                    '10　 X=6,Y=5
                    SerchAryY(10) = 5
                    SerchAryX(11) = 6                                    '11　 X=6,Y=4
                    SerchAryY(11) = 4
                    SerchAryX(12) = 6                                    '12　 X=6,Y=3
                    SerchAryY(12) = 3
                    SerchAryX(13) = 6                                    '13　 X=6,Y=2
                    SerchAryY(13) = 2
                    SerchAryX(14) = 5                                    '14　 X=5,Y=2
                    SerchAryY(14) = 2
                    SerchAryX(15) = 4                                    '15　 X=4,Y=2
                    SerchAryY(15) = 2
                    SerchAryX(16) = 3                                    '16　 X=3,Y=2
                    SerchAryY(16) = 2
                    SerchAryX(17) = 2                                    '17　 X=2,Y=2
                    SerchAryY(17) = 2
                    SerchAryX(18) = 2                                    '18　 X=2,Y=3
                    SerchAryY(18) = 3
                    SerchAryX(19) = 2                                    '19　 X=2,Y=4
                    SerchAryY(19) = 4
                    SerchAryX(20) = 2                                    '20　 X=2,Y=5
                    SerchAryY(20) = 5
                    SerchAryX(21) = 2                                    '21　 X=2,Y=6
                    SerchAryY(21) = 6
                    SerchAryX(22) = 3                                    '22　 X=3,Y=6
                    SerchAryY(22) = 6
                    SerchAryX(23) = 4                                    '23　 X=4,Y=6
                    SerchAryY(23) = 6
                    SerchAryX(24) = 5                                    '24　 X=5,Y=6
                    SerchAryY(24) = 6
                    SerchAryX(25) = 6                                    '25　 X=6,Y=6
                    SerchAryY(25) = 6
                    SerchAryX(26) = 7                                    ' 26　 X=7,Y=6
                    SerchAryY(26) = 6
                    SerchAryX(27) = 7                                    ' 27　 X=7,Y=5
                    SerchAryY(27) = 5
                    SerchAryX(28) = 7                                    ' 28　 X=7,Y=4
                    SerchAryY(28) = 4
                    SerchAryX(29) = 7                                    ' 29　 X=7,Y=3
                    SerchAryY(29) = 3
                    SerchAryX(30) = 7                                    ' 30　 X=7,Y=2
                    SerchAryY(30) = 2
                    SerchAryX(31) = 7                                    ' 31　 X=7,Y=1
                    SerchAryY(31) = 1
                    SerchAryX(32) = 6                                    ' 32　 X=6,Y=1
                    SerchAryY(32) = 1
                    SerchAryX(33) = 5                                    ' 33　 X=5,Y=1
                    SerchAryY(33) = 1
                    SerchAryX(34) = 4                                    ' 34　 X=4,Y=1
                    SerchAryY(34) = 1
                    SerchAryX(35) = 3                                    '35　 X=3,Y=1
                    SerchAryY(35) = 1
                    SerchAryX(36) = 2                                    '36　 X=2,Y=1
                    SerchAryY(36) = 1
                    SerchAryX(37) = 1                                    '37　 X=1,Y=1
                    SerchAryY(37) = 1
                    SerchAryX(38) = 1                                    '38　 X=1,Y=2
                    SerchAryY(38) = 2
                    SerchAryX(39) = 1                                    '39　 X=1,Y=3
                    SerchAryY(39) = 3
                    SerchAryX(40) = 1                                    '40　 X=1,Y=4
                    SerchAryY(40) = 4
                    SerchAryX(41) = 1                                    '41　 X=1,Y=5
                    SerchAryY(41) = 5
                    SerchAryX(42) = 1                                    '42　 X=1,Y=6
                    SerchAryY(42) = 6
                    SerchAryX(43) = 1                                    '43　 X=1,Y=7
                    SerchAryY(43) = 7
                    SerchAryX(44) = 2                                    '44　 X=2,Y=7
                    SerchAryY(44) = 7
                    SerchAryX(45) = 3                                    '45　 X=3,Y=7
                    SerchAryY(45) = 7
                    SerchAryX(46) = 4                                    '46　 X=4,Y=7
                    SerchAryY(46) = 7
                    SerchAryX(47) = 5                                    '47　 X=5,Y=7
                    SerchAryY(47) = 7
                    SerchAryX(48) = 6                                    '48　 X=6,Y=7
                    SerchAryY(48) = 7
                    SerchAryX(49) = 7                                    '49　 X=7,Y=7
                    SerchAryY(49) = 7

                Case 2
                    SerchAryX(1) = 3                                    ' 1　 X=3,Y=3
                    SerchAryY(1) = 3
                    SerchAryX(2) = 4                                    ' 2　 X=4,Y=3
                    SerchAryY(2) = 3
                    SerchAryX(3) = 4                                    ' 3　 X=4,Y=2
                    SerchAryY(3) = 2
                    SerchAryX(4) = 3                                    ' 4　 X=3,Y=2
                    SerchAryY(4) = 2
                    SerchAryX(5) = 2                                    ' 5　 X=2,Y=2
                    SerchAryY(5) = 2
                    SerchAryX(6) = 2                                    ' 6　 X=2,Y=3
                    SerchAryY(6) = 3
                    SerchAryX(7) = 2                                    ' 7　 X=2,Y=4
                    SerchAryY(7) = 4
                    SerchAryX(8) = 3                                    ' 8　 X=3,Y=4
                    SerchAryY(8) = 4
                    SerchAryX(9) = 4                                    ' 9　 X=4,Y=4
                    SerchAryY(9) = 4
                    SerchAryX(10) = 5                                    '10　 X=5,Y=4
                    SerchAryY(10) = 4
                    SerchAryX(11) = 5                                    '11　 X=5,Y=3
                    SerchAryY(11) = 3
                    SerchAryX(12) = 5                                    '12　 X=5,Y=2
                    SerchAryY(12) = 2
                    SerchAryX(13) = 5                                    '13　 X=5,Y=1
                    SerchAryY(13) = 1
                    SerchAryX(14) = 4                                    '14　 X=4,Y=1
                    SerchAryY(14) = 1
                    SerchAryX(15) = 3                                    '15　 X=3,Y=1
                    SerchAryY(15) = 1
                    SerchAryX(16) = 2                                    '16　 X=2,Y=1
                    SerchAryY(16) = 1
                    SerchAryX(17) = 1                                    '17　 X=1,Y=1
                    SerchAryY(17) = 1
                    SerchAryX(18) = 1                                    '18　 X=1,Y=2
                    SerchAryY(18) = 2
                    SerchAryX(19) = 1                                    '19　 X=1,Y=3
                    SerchAryY(19) = 3
                    SerchAryX(20) = 1                                    '20　 X=1,Y=4
                    SerchAryY(20) = 4
                    SerchAryX(21) = 1                                    '21　 X=1,Y=5
                    SerchAryY(21) = 5
                    SerchAryX(22) = 2                                    '22　 X=2,Y=5
                    SerchAryY(22) = 5
                    SerchAryX(23) = 3                                    '23　 X=3,Y=5
                    SerchAryY(23) = 5
                    SerchAryX(24) = 4                                    '24　 X=4,Y=5
                    SerchAryY(24) = 5
                    SerchAryX(25) = 5                                    '25　 X=5,Y=5
                    SerchAryY(25) = 5

                Case Else
                    SerchAryX(1) = 2                                    ' 1　 X=2,Y=2
                    SerchAryY(1) = 2
                    SerchAryX(2) = 3                                    ' 2　 X=3,Y=2
                    SerchAryY(2) = 2
                    SerchAryX(3) = 3                                    ' 3　 X=3,Y=1
                    SerchAryY(3) = 1
                    SerchAryX(4) = 2                                    ' 4　 X=2,Y=1
                    SerchAryY(4) = 1
                    SerchAryX(5) = 1                                    ' 5　 X=1,Y=1
                    SerchAryY(5) = 1
                    SerchAryX(6) = 1                                    ' 6　 X=1,Y=2
                    SerchAryY(6) = 2
                    SerchAryX(7) = 1                                    ' 7　 X=1,Y=3
                    SerchAryY(7) = 3
                    SerchAryX(8) = 2                                    ' 8　 X=2,Y=3
                    SerchAryY(8) = 3
                    SerchAryX(9) = 3                                    ' 9　 X=3,Y=3
                    SerchAryY(9) = 3
            End Select

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "basMatrix.InitSerchAry() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "ブロック番号X,Yを返す"
    '''=========================================================================
    ''' <summary>ブロック番号X,Yを返す</summary>
    ''' <param name="BlkNum">(INP)ブロック番号</param>
    ''' <param name="BlkX">  (OUT)ブロック番号X</param>
    ''' <param name="BlkY">  (OUT)ブロック番号Y</param>
    '''=========================================================================
    Public Sub GetBlkNum(ByVal BlkNum As Integer, ByRef BlkX As Integer, ByRef BlkY As Integer) 'V1.20.0.0⑨

        Dim strMSG As String

        Try
            BlkX = SerchAryX(BlkNum)
            BlkY = SerchAryY(BlkNum)

            Console.WriteLine("GetBlkNum() BlkNum=" + BlkNum.ToString("0") + " X=" + BlkX.ToString("0") + ", Y=" + BlkY.ToString("0"))

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "basMatrix.GetNextBlkNum() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "ステップ距離と方向から次の補正位置(XYテーブル座標)を返す"
    '''=========================================================================
    ''' <summary>ステップ距離と方向から次の補正位置(XYテーブル座標)を返す</summary>
    ''' <param name="InpPosX">(INP)本来の補正位置X</param>
    ''' <param name="InpPosY">(INP)本来の補正位置Y</param>
    ''' <param name="Pitch">  (INP)ビッチ</param>
    ''' <param name="TStep">  (INP)トータルステップ距離</param>
    ''' <param name="DIR">    (I/O)ステップ方向(1=0°(→), 2=45°, 3=90°(↑), 4=135°,
    '''                                         5=180°(←), 6=225°, 7=270°(↓), 8=315°)</param>
    ''' <param name="OutPosX">(OUT)次補正位置X</param>
    ''' <param name="OutPosY">(OUT)次補正位置X</param>
    '''=========================================================================
    Private Sub GetNextPos(ByVal InpPosX As Double, ByVal InpPosY As Double, ByVal Pitch As Double, ByVal TStep As Double, ByRef DIR As Integer, ByRef OutPosX As Double, ByRef OutPosY As Double)

        Dim strMSG As String

        Try
            Select Case (DIR)
                Case 1      ' ステップ方向(1=→)
                    OutPosX = InpPosX + TStep                               ' X = 本来の補正位置x + トータルステップ距離
                    OutPosY = InpPosY                                       ' Y = 本来の補正位置y
                    DIR = 2

                Case 2      ' ステップ方向(2=45°)
                    If (OutPosY + Pitch > InpPosY + TStep) Then             ' Y方向は本来の補正位置y + トータルステップ距離を超えるまでステップする
                        If (OutPosX - Pitch <= InpPosX) Then                ' X方向は本来の座標まで-ステップする
                            DIR = 3
                            GoTo STP_DIR3
                        Else                                                ' X座標だけステップ距離分 -ステップする
                            OutPosX = OutPosX - Pitch                       ' X = 前回の補正位置y - ステップ距離
                            '                                               ' Y = 前回の補正位置y
                        End If
                    Else                                                    ' Y座標だけステップ距離分 +ステップする
                        '                                                   ' X = 前回の補正位置x
                        OutPosY = OutPosY + Pitch                           ' Y = 前回の補正位置y + ステップ距離
                    End If

                Case 3      ' ステップ方向(3=↑)
STP_DIR3:
                    OutPosX = InpPosX                                       ' X = 本来の補正位置x
                    OutPosY = InpPosY + TStep                               ' Y = 本来の補正位置y + トータルステップ距離
                    DIR = 4

                Case 4      ' ステップ方向(4=135°)
                    If (OutPosX - Pitch < InpPosX - TStep) Then             ' X方向は本来の補正位置x - トータルステップ距離を超えるまでステップする
                        If (OutPosY - Pitch <= InpPosY) Then                ' Y方向は本来の座標まで-ステップする
                            DIR = 5
                            GoTo STP_DIR5
                        Else                                                ' Y座標だけステップ距離分 -ステップする
                            '                                               ' X = 前回の補正位置x
                            OutPosY = OutPosY - Pitch                       ' Y = 前回の補正位置y - ステップ距離
                        End If
                    Else                                                    ' X座標だけステップ距離分-する
                        OutPosX = OutPosX - Pitch                           ' X = 前回の補正位置x -  ステップ距離
                        '                                                   ' Y = 前回の補正位置y
                    End If

                Case 5      ' ステップ方向(5=←)
STP_DIR5:
                    OutPosX = InpPosX - TStep                               ' X = 本来の補正位置x - トータルステップ距離
                    OutPosY = InpPosY                                       ' Y = 本来の補正位置y
                    DIR = 6

                Case 6      ' ステップ方向(6=225°)
                    If (OutPosY - Pitch < InpPosY - TStep) Then             ' Y方向は本来の補正位置y - トータルステップ距離を超えるまでステップする
                        If (OutPosX + Pitch >= InpPosX) Then                ' X方向は本来の座標まで+ステップする
                            DIR = 7
                            GoTo STP_DIR7
                        Else                                                ' X座標だけステップ距離分 +ステップする
                            OutPosX = OutPosX + Pitch                       ' X = 前回の補正位置x + ステップ距離
                            '                                               ' Y = 前回の補正位置y
                        End If
                    Else                                                    ' X座標だけステップ距離分-する
                        '                                                   ' X = 前回の補正位置x
                        OutPosY = OutPosY - Pitch                           ' Y = 前回の補正位置y - トータルステップ距離
                    End If

                Case 7      ' ステップ方向(7=↓)
STP_DIR7:
                    OutPosX = InpPosX                                       ' X = 本来の補正位置x
                    OutPosY = InpPosY - TStep                               ' Y = 本来の補正位置y - トータルステップ距離
                    DIR = 8

                Case Else   ' ステップ方向(8=315°)
                    If (OutPosX + Pitch > InpPosX + TStep) Then             ' X方向は本来の補正位置y + トータルステップ距離を超えるまでステップする
                        If (OutPosY + Pitch >= InpPosY) Then                ' Y方向は本来の座標まで+ステップする
                            DIR = 9                                         ' 1周分サーチしたのでステップ方向を初期化
                        Else                                                ' X座標だけステップ距離分 +ステップする
                            '                                               ' X = 前回の補正位置x
                            OutPosY = OutPosY + Pitch                       ' Y = 前回の補正位置y + ステップ距離
                        End If
                    Else                                                    ' X座標だけステップ距離分 +ステップする
                        OutPosX = OutPosX + Pitch                           ' X = 前回の補正位置x + ステップ距離
                        '                                                   ' Y = 前回の補正位置y
                    End If
            End Select

            Console.WriteLine("GetThetaPos Return (OutPosX, OutPosY = " + Format(OutPosX, "0.0000") + ", " + Format(OutPosY, "0.0000") + ")")
            Console.WriteLine(" Step, Dir = " + Format(TStep, "0.0000") + ", " + Format(DIR, "0"))

STP_END:
            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FrmMatrix.GetNextPos() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "Z,Z2をOnまたはOffする"
    '''=========================================================================
    ''' <summary>Z,Z2をOnまたはOffする</summary>
    ''' <param name="OnOff">(INP)ブロック数</param>
    ''' <returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Public Function Sub_ZOnOff(ByVal OnOff As Integer) As Integer 'V1.23.0.0⑦

        Dim r As Integer
        Dim zStepPos As Double
        Dim strMSG As String

        Try
            ' ZなしならNOP
            If (gSysPrm.stDEV.giPrbTyp = 0) Then
                Return (cFRS_NORMAL)
            End If

            If (OnOff) Then
                '-------------------------------------------------------------------
                '   Z,Z2をOn位置に移動する
                '-------------------------------------------------------------------
                ' ZをON位置に移動する
                r = ZZMOVE(typPlateInfo.dblZOffSet, 1)
                r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
                If (r <> cFRS_NORMAL) Then                              ' エラー ? (メッセージは表示済み)
                    Return (r)
                Else
                    Call LAMP_CTRL(LAMP_Z, True)                        ' ZランプON
                End If

                ' Z2をON位置に移動する
                If IsUnderProbe() Then                                  ' Z2有り ? 
                    r = Z2move(Z2ON)                                    ' Z2をON位置に移動する
                    If (r <> cFRS_NORMAL) Then                          ' エラー ? (メッセージは表示済み)
                        Return (r)
                    End If
                    Call ZSTOPSTS2()                                    ' Z2軸動作停止待ち
                End If

                '-------------------------------------------------------------------
                '   Z,Z2をOff位置に移動する
                '-------------------------------------------------------------------
            Else
                ' Z2をOFF位置に移動する
                If IsUnderProbe() Then                                  ' Z2有り ? 
                    r = Z2move(Z2OFF)                                   ' Z2をOFF位置に移動する
                    If (r <> cFRS_NORMAL) Then                          ' エラー ? (メッセージは表示済み)
                        Return (r)
                    End If
                    Call ZSTOPSTS2()                                    ' Z2軸動作停止待ち
                End If

                ' Z軸ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄ位置 = ZON位置 - ｽﾃｯﾌﾟ上昇距離 
                zStepPos = typPlateInfo.dblZOffSet - typPlateInfo.dblZStepUpDist
                ' ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄ位置が待機位置より小さい場合は、待機位置をｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄ位置とする
                If (zStepPos < typPlateInfo.dblZWaitOffset) Then zStepPos = typPlateInfo.dblZWaitOffset

                ' Zをｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄ位置に移動(上昇位置)
                r = PROBOFF_EX(zStepPos)                                ' EX_STARTのZOFF位置をzWaitPosとする
                r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
                If (r <> cFRS_NORMAL) Then                              ' エラー ? (メッセージは表示済み)
                    Return (r)
                Else
                    Call LAMP_CTRL(LAMP_Z, False)                       ' ZランプOFF
                End If

            End If

            Return (cFRS_NORMAL)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "basMatrix.Sub_ZOnOff() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return値 = 例外エラー
        End Try
    End Function
#End Region

#Region "ステージ座標を求める"
    '''=========================================================================
    ''' <summary>ステージ座標を求める</summary>
    ''' <param name="StgPosX">(INP)マトリックスのブロックX</param>
    ''' <param name="StgPosY">(INP)マトリックスのブロックY</param>
    '''=========================================================================
    Private Sub GetAProbStagePosition(ByRef StgPosX As Double, ByRef StgPosY As Double)

        Dim strMSG As String

        Try

            With typPlateInfo
                ' XYテーブル位置 = トリム位置 + ステージオフセット + Θ補正のずれ量
                Select Case gSysPrm.stDEV.giBpDirXy
                    Case 0 ' x←, y↓
                        StgPosX = gSysPrm.stDEV.gfTrimX + .dblTableOffsetXDir + (.dblBlockSizeXDir / 2) + ObjMain.stAPRB.dblAProbeStgPosX + gfCorrectPosX
                        StgPosY = gSysPrm.stDEV.gfTrimY + .dblTableOffsetYDir + (.dblBlockSizeYDir / 2) + ObjMain.stAPRB.dblAProbeStgPosY + gfCorrectPosY

                    Case 1 ' x→, y↓
                        StgPosX = gSysPrm.stDEV.gfTrimX - (.dblTableOffsetXDir + (.dblBlockSizeXDir / 2) + ObjMain.stAPRB.dblAProbeStgPosX) + gfCorrectPosX
                        StgPosY = gSysPrm.stDEV.gfTrimY + .dblTableOffsetYDir + (.dblBlockSizeYDir / 2) + ObjMain.stAPRB.dblAProbeStgPosY + gfCorrectPosY

                    Case 2 ' x←, y↑
                        StgPosX = gSysPrm.stDEV.gfTrimX + .dblTableOffsetXDir + (.dblBlockSizeXDir / 2) + ObjMain.stAPRB.dblAProbeStgPosX + gfCorrectPosX
                        StgPosY = gSysPrm.stDEV.gfTrimY - (.dblTableOffsetYDir + (.dblBlockSizeYDir / 2) + ObjMain.stAPRB.dblAProbeStgPosY) + gfCorrectPosY

                    Case 3 ' x→, y↑
                        StgPosX = gSysPrm.stDEV.gfTrimX - (.dblTableOffsetXDir + (.dblBlockSizeXDir / 2) + ObjMain.stAPRB.dblAProbeStgPosX) + gfCorrectPosX
                        StgPosY = gSysPrm.stDEV.gfTrimY - (.dblTableOffsetYDir + (.dblBlockSizeYDir / 2) + ObjMain.stAPRB.dblAProbeStgPosY) + gfCorrectPosY
                End Select
            End With

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "basMatrix.GetAProbStagePosition() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "ステージ座標(測定位置)を求める"
    '''=========================================================================
    ''' <summary>ステージ座標(測定位置)を求める</summary>
    ''' <param name="StgPosX">(INP)マトリックスのブロックX</param>
    ''' <param name="StgPosY">(INP)マトリックスのブロックY</param>
    '''=========================================================================
    Private Sub GetAProbStageMeasPosition(ByRef StgPosX As Double, ByRef StgPosY As Double)

        Dim strMSG As String

        Try

            With typPlateInfo
                ' XYテーブル位置 = トリム位置 + ステージオフセット + Θ補正のずれ量
                Select Case gSysPrm.stDEV.giBpDirXy
                    Case 0 ' x←, y↓
                        StgPosX = gSysPrm.stDEV.gfTrimX + .dblTableOffsetXDir + (.dblBlockSizeXDir / 2) + gfCorrectPosX
                        StgPosY = gSysPrm.stDEV.gfTrimY + .dblTableOffsetYDir + (.dblBlockSizeYDir / 2) + gfCorrectPosY

                    Case 1 ' x→, y↓
                        StgPosX = gSysPrm.stDEV.gfTrimX - (.dblTableOffsetXDir + (.dblBlockSizeXDir / 2)) + gfCorrectPosX
                        StgPosY = gSysPrm.stDEV.gfTrimY + .dblTableOffsetYDir + (.dblBlockSizeYDir / 2) + gfCorrectPosY

                    Case 2 ' x←, y↑
                        StgPosX = gSysPrm.stDEV.gfTrimX + .dblTableOffsetXDir + (.dblBlockSizeXDir / 2) + gfCorrectPosX
                        StgPosY = gSysPrm.stDEV.gfTrimY - (.dblTableOffsetYDir + (.dblBlockSizeYDir / 2)) + gfCorrectPosY

                    Case 3 ' x→, y↑
                        StgPosX = gSysPrm.stDEV.gfTrimX - (.dblTableOffsetXDir + (.dblBlockSizeXDir / 2)) + gfCorrectPosX
                        StgPosY = gSysPrm.stDEV.gfTrimY - (.dblTableOffsetYDir + (.dblBlockSizeYDir / 2)) + gfCorrectPosY
                End Select
            End With

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "basMatrix.GetAProbStageMeasPosition() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#End Region

End Module
