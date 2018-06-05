'===============================================================================
'   Description  : 加工条件選択画面処理
'
'   Copyright(C) : OMRON LASERFRONT INC. 2011
'
'===============================================================================

Imports LaserFront.Trimmer.DefTrimFnc

Public Class FrmFlCond
#Region "【変数定義】"
    '===========================================================================
    '   変数定義
    '===========================================================================
    '----- 変数定義 -----
    Private mExitFlag As Integer                                        ' 結果(0:初期, 1:OK(ADVｷｰ), 3:Cancel(RESETｷｰ))
    Private mCondNum As Integer                                         ' 加工条件番号
    Private mQrate As Double                                            ' Qレート(KHz)
    Private mCondIdx As Integer                                         ' 加工条件番号(初期値)

#End Region

#Region "【メソッド定義】"
#Region "ShowDialogメソッドに独自の引数を追加する"
    '''=========================================================================
    ''' <summary>ShowDialogメソッドに独自の引数を追加する</summary>
    ''' <param name="Owner">  (INP)オーナー</param>
    ''' <param name="CondIdx">(INP)加工条件番号(初期値)</param>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Overloads Sub ShowDialog(ByVal Owner As IWin32Window, ByVal CondIdx As Integer)

        mCondIdx = CondIdx                                              ' 加工条件番号(初期値)
        If (mCondIdx > (MAX_BANK_NUM - 3)) Then                         ' ###221 加工条件番号は0-29番まで指定可能とする
            mCondIdx = 0
        End If
        Me.ShowDialog()

    End Sub
#End Region

#Region "終了結果を返す"
    '''=========================================================================
    ''' <summary>終了結果を返す</summary>
    ''' <param name="CondNum">(OUT)加工条件番号</param>
    ''' <param name="QRATE">  (OUT)Qレート(KHz)</param>
    ''' <returns>0=OK(STARTｷｰ), 3=Cancel(RESETｷｰ), その他=エラー</returns>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Function GetResult(ByRef CondNum As Integer, ByRef QRATE As Double) As Integer

        Dim strMSG As String
        Dim r As Integer

        Try
            If (mExitFlag = cFRS_ERR_START) Then
                r = cFRS_NORMAL
            Else
                r = mExitFlag
            End If

            CondNum = mCondNum
            QRATE = mQrate
            Return (r)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FrmFlCond.GetResult() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "OKボタン押下時処理"
    '''=========================================================================
    ''' <summary>OKボタン押下時処理</summary>
    ''' <param name="sender">(INP)</param>
    ''' <param name="e">     (INP)</param>
    '''=========================================================================
    Private Sub BtnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnOK.Click

        mCondNum = CmbSelCnd.SelectedIndex                              ' 加工条件番号
        mQrate = stCND.Freq(CmbSelCnd.SelectedIndex)                    ' Qレート(KHz)
        mExitFlag = cFRS_ERR_START                                      ' ExitFlag = 1:OK(STARTｷｰ)
        Me.Close()                                                      ' フォームを閉じる

    End Sub
#End Region

#Region "Cancelボタン押下時処理"
    '''=========================================================================
    ''' <summary>Cancelボタン押下時処理</summary>
    ''' <param name="sender">(INP)</param>
    ''' <param name="e">     (INP)</param>
    '''=========================================================================
    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click

        mExitFlag = cFRS_ERR_RST                                        ' ExitFlag = 3:Cancel(RESETｷｰ))
        Me.Close()                                                      ' フォームを閉じる

    End Sub
#End Region

#Region "加工条件番号コンボボックス変更時"
    '''=========================================================================
    ''' <summary>加工条件番号コンボボックス変更時</summary>
    ''' <param name="sender">(INP)</param>
    ''' <param name="e">     (INP)</param>
    '''=========================================================================
    Private Sub CmbSelCnd_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmbSelCnd.SelectedIndexChanged

        Dim strMSG As String

        Try
            ' 加工条件番号で選択のQ RATE/STEG本数/電流値を表示する
            LblCndQrateVal.Text = Format(stCND.Freq(CmbSelCnd.SelectedIndex), "##0.0")
            LblCndStegVal.Text = Format(stCND.Steg(CmbSelCnd.SelectedIndex), "#0")
            LblCndCurVal.Text = Format(stCND.Curr(CmbSelCnd.SelectedIndex), "###0")

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FrmFlCond.CmbSelCnd_SelectedIndexChanged() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "Form_Load時処理"
    '''=========================================================================
    ''' <summary>Form_Load時処理</summary>
    ''' <param name="sender">(INP)</param>
    ''' <param name="e">     (INP)</param>
    '''=========================================================================
    Private Sub FrmFlCond_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim Idx As Integer
        Dim strMSG As String

        Try
            ' 初期処理
            'LblMSG.Text = MSG_LASER_LABEL08                             ' "加工条件番号を指定して下さい。"
            'LblSelCnd.Text = MSG_LASER_LABEL03                          ' "加工条件番号"
            'LblCndQrate.Text = MSG_LASER_LABEL05                        ' "Q SWITCH RATE (KHz)"
            'LblCndSteg.Text = MSG_LASER_LABEL06                         ' "STEG本数"
            'LblCndCur.Text = MSG_LASER_LABEL07                          ' "電流値(mA)"

            ' コンボボックス初期化
            CmbSelCnd.Items.Clear()
            'For Idx = 0 To MAX_BANK_NUM - 1                            ' ###221
            For Idx = 0 To (MAX_BANK_NUM - 3)                           ' ###221 加工条件番号は0-29番まで指定可能とする
                CmbSelCnd.Items.Add(Idx.ToString("0"))
            Next Idx
            CmbSelCnd.SelectedIndex = mCondIdx                          ' 加工条件番号(初期値)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FrmFlCond.FrmFlCond_Load() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "Form_Activate時処理"
    '''=========================================================================
    ''' <summary>Form_Activate時処理</summary>
    ''' <param name="sender">(INP)</param>
    ''' <param name="e">     (INP)</param>
    '''=========================================================================
    Private Sub FrmFlCond_Activated(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Activated

        'Dim r As Integer
        'Dim st As Long
        'Dim strMSG As String

        'Try
        '    ' 初期処理
        '    mExitFlag = 0
        '    Call Form1.System1.SetSysParam(gSysPrm)                     ' システムパラメータの設定(OcxSystem用)
        '    Call ZCONRST()                                              ' コンソールキーラッチ解除

        '    ' START/RESETキー押下待ち(Ok/Cancelボタンも有効)
        '    Do
        '        r = STARTRESET_SWCHECK(False, st)                       ' START/RESET SW押下チェック
        '        If (r = cFRS_NORMAL) Then
        '            If (st = cFRS_ERR_RST) Then
        '                Call BtnCancel_Click(sender, e)
        '            ElseIf (st = cFRS_ERR_START) Then
        '                Call BtnOK_Click(sender, e)
        '            End If
        '        End If

        '        System.Windows.Forms.Application.DoEvents()             ' メッセージポンプ
        '        Call System.Threading.Thread.Sleep(10)                   ' Wait(msec)

        '        ' システムエラーチェック
        '        r = Form1.System1.SysErrChk_ForVBNET(giAppMode)
        '        If (r <> cFRS_NORMAL) Then                              ' 非常停止等 ?
        '            mExitFlag = r
        '            Exit Do
        '        End If

        '        Me.BringToFront()                                       ' 最前面に表示 
        '    Loop While (mExitFlag = 0)

        '    ' トラップエラー発生時 
        'Catch ex As Exception
        '    strMSG = "FrmFlCond.FrmFlCond_Activated() TRAP ERROR = " + ex.Message
        '    MsgBox(strMSG)
        'End Try
    End Sub

    ''' <summary>
    ''' 表示されたときの処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub FrmFlCond_Shown(sender As Object, e As EventArgs) Handles Me.Shown


        Dim r As Integer
        Dim st As Long
        Dim strMSG As String

        Try
            ' 初期処理
            mExitFlag = 0
            Call Form1.System1.SetSysParam(gSysPrm)                     ' システムパラメータの設定(OcxSystem用)
            Call ZCONRST()                                              ' コンソールキーラッチ解除
            Me.TopMost = True                   'V6.0.2.0⑤
            ' START/RESETキー押下待ち(Ok/Cancelボタンも有効)
            Do
                r = STARTRESET_SWCHECK(False, st)                       ' START/RESET SW押下チェック
                If (r = cFRS_NORMAL) Then
                    If (st = cFRS_ERR_RST) Then
                        Call BtnCancel_Click(sender, e)
                    ElseIf (st = cFRS_ERR_START) Then
                        Call BtnOK_Click(sender, e)
                    End If
                End If

                System.Windows.Forms.Application.DoEvents()             ' メッセージポンプ
                Call System.Threading.Thread.Sleep(10)                   ' Wait(msec)

                ' システムエラーチェック
                r = Form1.System1.SysErrChk_ForVBNET(giAppMode)
                If (r <> cFRS_NORMAL) Then                              ' 非常停止等 ?
                    mExitFlag = r
                    Exit Do
                End If

                ' 'V6.0.2.0⑤ Me.BringToFront()                                       ' 最前面に表示 
            Loop While (mExitFlag = 0)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FrmFlCond.FrmFlCond_Shown() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region



#End Region

End Class