'===============================================================================
'   Description  : ���H�����I����ʏ���
'
'   Copyright(C) : OMRON LASERFRONT INC. 2011
'
'===============================================================================

Imports LaserFront.Trimmer.DefTrimFnc

Public Class FrmFlCond
#Region "�y�ϐ���`�z"
    '===========================================================================
    '   �ϐ���`
    '===========================================================================
    '----- �ϐ���` -----
    Private mExitFlag As Integer                                        ' ����(0:����, 1:OK(ADV��), 3:Cancel(RESET��))
    Private mCondNum As Integer                                         ' ���H�����ԍ�
    Private mQrate As Double                                            ' Q���[�g(KHz)
    Private mCondIdx As Integer                                         ' ���H�����ԍ�(�����l)

#End Region

#Region "�y���\�b�h��`�z"
#Region "ShowDialog���\�b�h�ɓƎ��̈�����ǉ�����"
    '''=========================================================================
    ''' <summary>ShowDialog���\�b�h�ɓƎ��̈�����ǉ�����</summary>
    ''' <param name="Owner">  (INP)�I�[�i�[</param>
    ''' <param name="CondIdx">(INP)���H�����ԍ�(�����l)</param>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Overloads Sub ShowDialog(ByVal Owner As IWin32Window, ByVal CondIdx As Integer)

        mCondIdx = CondIdx                                              ' ���H�����ԍ�(�����l)
        If (mCondIdx > (MAX_BANK_NUM - 3)) Then                         ' ###221 ���H�����ԍ���0-29�Ԃ܂Ŏw��\�Ƃ���
            mCondIdx = 0
        End If
        Me.ShowDialog()

    End Sub
#End Region

#Region "�I�����ʂ�Ԃ�"
    '''=========================================================================
    ''' <summary>�I�����ʂ�Ԃ�</summary>
    ''' <param name="CondNum">(OUT)���H�����ԍ�</param>
    ''' <param name="QRATE">  (OUT)Q���[�g(KHz)</param>
    ''' <returns>0=OK(START��), 3=Cancel(RESET��), ���̑�=�G���[</returns>
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

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FrmFlCond.GetResult() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "OK�{�^������������"
    '''=========================================================================
    ''' <summary>OK�{�^������������</summary>
    ''' <param name="sender">(INP)</param>
    ''' <param name="e">     (INP)</param>
    '''=========================================================================
    Private Sub BtnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnOK.Click

        mCondNum = CmbSelCnd.SelectedIndex                              ' ���H�����ԍ�
        mQrate = stCND.Freq(CmbSelCnd.SelectedIndex)                    ' Q���[�g(KHz)
        mExitFlag = cFRS_ERR_START                                      ' ExitFlag = 1:OK(START��)
        Me.Close()                                                      ' �t�H�[�������

    End Sub
#End Region

#Region "Cancel�{�^������������"
    '''=========================================================================
    ''' <summary>Cancel�{�^������������</summary>
    ''' <param name="sender">(INP)</param>
    ''' <param name="e">     (INP)</param>
    '''=========================================================================
    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click

        mExitFlag = cFRS_ERR_RST                                        ' ExitFlag = 3:Cancel(RESET��))
        Me.Close()                                                      ' �t�H�[�������

    End Sub
#End Region

#Region "���H�����ԍ��R���{�{�b�N�X�ύX��"
    '''=========================================================================
    ''' <summary>���H�����ԍ��R���{�{�b�N�X�ύX��</summary>
    ''' <param name="sender">(INP)</param>
    ''' <param name="e">     (INP)</param>
    '''=========================================================================
    Private Sub CmbSelCnd_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmbSelCnd.SelectedIndexChanged

        Dim strMSG As String

        Try
            ' ���H�����ԍ��őI����Q RATE/STEG�{��/�d���l��\������
            LblCndQrateVal.Text = Format(stCND.Freq(CmbSelCnd.SelectedIndex), "##0.0")
            LblCndStegVal.Text = Format(stCND.Steg(CmbSelCnd.SelectedIndex), "#0")
            LblCndCurVal.Text = Format(stCND.Curr(CmbSelCnd.SelectedIndex), "###0")

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FrmFlCond.CmbSelCnd_SelectedIndexChanged() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "Form_Load������"
    '''=========================================================================
    ''' <summary>Form_Load������</summary>
    ''' <param name="sender">(INP)</param>
    ''' <param name="e">     (INP)</param>
    '''=========================================================================
    Private Sub FrmFlCond_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim Idx As Integer
        Dim strMSG As String

        Try
            ' ��������
            'LblMSG.Text = MSG_LASER_LABEL08                             ' "���H�����ԍ����w�肵�ĉ������B"
            'LblSelCnd.Text = MSG_LASER_LABEL03                          ' "���H�����ԍ�"
            'LblCndQrate.Text = MSG_LASER_LABEL05                        ' "Q SWITCH RATE (KHz)"
            'LblCndSteg.Text = MSG_LASER_LABEL06                         ' "STEG�{��"
            'LblCndCur.Text = MSG_LASER_LABEL07                          ' "�d���l(mA)"

            ' �R���{�{�b�N�X������
            CmbSelCnd.Items.Clear()
            'For Idx = 0 To MAX_BANK_NUM - 1                            ' ###221
            For Idx = 0 To (MAX_BANK_NUM - 3)                           ' ###221 ���H�����ԍ���0-29�Ԃ܂Ŏw��\�Ƃ���
                CmbSelCnd.Items.Add(Idx.ToString("0"))
            Next Idx
            CmbSelCnd.SelectedIndex = mCondIdx                          ' ���H�����ԍ�(�����l)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FrmFlCond.FrmFlCond_Load() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "Form_Activate������"
    '''=========================================================================
    ''' <summary>Form_Activate������</summary>
    ''' <param name="sender">(INP)</param>
    ''' <param name="e">     (INP)</param>
    '''=========================================================================
    Private Sub FrmFlCond_Activated(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Activated

        'Dim r As Integer
        'Dim st As Long
        'Dim strMSG As String

        'Try
        '    ' ��������
        '    mExitFlag = 0
        '    Call Form1.System1.SetSysParam(gSysPrm)                     ' �V�X�e���p�����[�^�̐ݒ�(OcxSystem�p)
        '    Call ZCONRST()                                              ' �R���\�[���L�[���b�`����

        '    ' START/RESET�L�[�����҂�(Ok/Cancel�{�^�����L��)
        '    Do
        '        r = STARTRESET_SWCHECK(False, st)                       ' START/RESET SW�����`�F�b�N
        '        If (r = cFRS_NORMAL) Then
        '            If (st = cFRS_ERR_RST) Then
        '                Call BtnCancel_Click(sender, e)
        '            ElseIf (st = cFRS_ERR_START) Then
        '                Call BtnOK_Click(sender, e)
        '            End If
        '        End If

        '        System.Windows.Forms.Application.DoEvents()             ' ���b�Z�[�W�|���v
        '        Call System.Threading.Thread.Sleep(10)                   ' Wait(msec)

        '        ' �V�X�e���G���[�`�F�b�N
        '        r = Form1.System1.SysErrChk_ForVBNET(giAppMode)
        '        If (r <> cFRS_NORMAL) Then                              ' ����~�� ?
        '            mExitFlag = r
        '            Exit Do
        '        End If

        '        Me.BringToFront()                                       ' �őO�ʂɕ\�� 
        '    Loop While (mExitFlag = 0)

        '    ' �g���b�v�G���[������ 
        'Catch ex As Exception
        '    strMSG = "FrmFlCond.FrmFlCond_Activated() TRAP ERROR = " + ex.Message
        '    MsgBox(strMSG)
        'End Try
    End Sub

    ''' <summary>
    ''' �\�����ꂽ�Ƃ��̏���
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub FrmFlCond_Shown(sender As Object, e As EventArgs) Handles Me.Shown


        Dim r As Integer
        Dim st As Long
        Dim strMSG As String

        Try
            ' ��������
            mExitFlag = 0
            Call Form1.System1.SetSysParam(gSysPrm)                     ' �V�X�e���p�����[�^�̐ݒ�(OcxSystem�p)
            Call ZCONRST()                                              ' �R���\�[���L�[���b�`����
            Me.TopMost = True                   'V6.0.2.0�D
            ' START/RESET�L�[�����҂�(Ok/Cancel�{�^�����L��)
            Do
                r = STARTRESET_SWCHECK(False, st)                       ' START/RESET SW�����`�F�b�N
                If (r = cFRS_NORMAL) Then
                    If (st = cFRS_ERR_RST) Then
                        Call BtnCancel_Click(sender, e)
                    ElseIf (st = cFRS_ERR_START) Then
                        Call BtnOK_Click(sender, e)
                    End If
                End If

                System.Windows.Forms.Application.DoEvents()             ' ���b�Z�[�W�|���v
                Call System.Threading.Thread.Sleep(10)                   ' Wait(msec)

                ' �V�X�e���G���[�`�F�b�N
                r = Form1.System1.SysErrChk_ForVBNET(giAppMode)
                If (r <> cFRS_NORMAL) Then                              ' ����~�� ?
                    mExitFlag = r
                    Exit Do
                End If

                ' 'V6.0.2.0�D Me.BringToFront()                                       ' �őO�ʂɕ\�� 
            Loop While (mExitFlag = 0)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FrmFlCond.FrmFlCond_Shown() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region



#End Region

End Class