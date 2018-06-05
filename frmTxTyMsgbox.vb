'===============================================================================
'   Description  : �s�w/�s�x�e�B�[�`���O��ʏI���m�F����
'
'   Copyright(C) : OMRON LASERFRONT INC. 2011
'
'===============================================================================
Option Strict Off
Option Explicit On

Imports LaserFront.Trimmer.DefTrimFnc
Imports TKY_ALL_SL432HW.My.Resources    'V4.4.0.0-0

Friend Class frmTxTyMsgbox
	Inherits System.Windows.Forms.Form
#Region "�v���C�x�[�g�ϐ���`"
    '===========================================================================
    '   �ϐ���`
    '===========================================================================
    Private mEndFlag As Short
#End Region

#Region "�I�����ʂ�Ԃ�"
    '''=========================================================================
    '''<summary>�I�����ʂ�Ԃ�</summary>
    '''<returns>cFRS_ERR_START=OK(START��)
    '''         cFRS_ERR_RST  =Cancel(RESET��)
    '''         cFRS_TxTy     =TX2/TY2����</returns>
    '''=========================================================================
    Public ReadOnly Property sGetReturn() As Short
        Get
            sGetReturn = mEndFlag
        End Get
    End Property
#End Region

#Region "Cancel�{�^������������"
    '''=========================================================================
    '''<summary>Cancel�{�^������������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub cmdCAN_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCAN.Click
        mEndFlag = cFRS_ERR_RST                             ' Cancel(RESET��)
    End Sub
#End Region

#Region "OK�{�^������������"
    '''=========================================================================
    '''<summary>OK�{�^������������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub cmdOK_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOK.Click
        mEndFlag = cFRS_ERR_START                                       ' OK(START��) 
    End Sub
#End Region

#Region "TX2�{�^������������"
    '''=========================================================================
    '''<summary>TX2�{�^������������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub cmdOKTxTy_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOKTxTy.Click
        mEndFlag = cFRS_TxTy                                ' TX2(Teach)/TY2����
    End Sub
#End Region

#Region "�t�H�[��Initialize������"
    '''=========================================================================
    '''<summary>�t�H�[��Initialize������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub Form_Initialize_Renamed()

        mEndFlag = -1
        'Me.Text = MSG_CLOSE_LABEL01                         ' "��ʏI���m�F"
        'Me.cmdOK.Text = MSG_CLOSE_LABEL02                   ' "�͂�(&Y)"
        'Me.cmdCAN.Text = MSG_CLOSE_LABEL03                  ' "������(&N)"
        'Me.cmdOKTxTy.Text = MSG_EXECUTE_TXTYLABEL           ' "TX2(&T)" or "TY2(&T)"
        If (giAppMode = APP_MODE_TX) Then
            Me.cmdOKTxTy.Text = "Teach"
        Else
            Me.cmdOKTxTy.Text = "TY2"
        End If
        Me.Label1.Text = MSG_105                            ' "�O�̉�ʂɖ߂�܂��B��낵���ł����H"

        ' TY2�{�^���̓��C����ʂ�TY2�{�^�����L�����ɕ\������
        If (giAppMode = APP_MODE_TY) Then                   ' TY2�R�}���h ? 
            If (Form1.stFNC(F_TY2).iDEF = 1) Then           ' TY2�{�^�����L�� ?
                Me.cmdOKTxTy.Visible = True
            Else
                Me.cmdOKTxTy.Visible = False                ' OK�{�^����TY2�{�^���̕\���ʒu�ֈړ� 
                Me.cmdOK.Location = Me.cmdOKTxTy.Location
            End If
        End If

    End Sub
#End Region

#Region "�t�H�[��Activate������"
    '''=========================================================================
    '''<summary>�t�H�[��Activate������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub frmTxTyMsgbox_Activated(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Activated

        Dim cin As Integer
        Dim r As Integer = 0
        Dim strMSG As String

        Try
            If (mEndFlag <> -1) Then Exit Sub
            mEndFlag = 0
            Call SetWindowPos(Me.Handle.ToInt32, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOSIZE Or SWP_NOMOVE)
            Call ZCONRST()                                          ' �ݿ�ٷ�ׯ�����

            Do
                System.Windows.Forms.Application.DoEvents()         ' ���b�Z�[�W�|���v
#If cOFFLINEcDEBUG = 0 Then
                r = STARTRESET_SWCHECK(False, cin)                  ' �R���\�[������(�Ď��Ȃ����[�h)
#Else
                cin = 0
#End If
                If (cin = cFRS_ERR_RST) Then                        ' RESET �L�[�����H
                    Call cmdCAN_Click(cmdCAN, New System.EventArgs())
                ElseIf (cin = cFRS_ERR_START) Then                  ' START �L�[��������Ă��邩�H
                    Call cmdOK_Click(cmdOK, New System.EventArgs())
                End If
                Call ZCONRST()                                      ' �ݿ�ٷ�ׯ�����
                System.Windows.Forms.Application.DoEvents()         ' ���b�Z�[�W�|���v
                Me.Refresh()
            Loop While (mEndFlag = 0)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "frmTxTyMsgbox.frmTxTyMsgbox_Activated() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

        Call ZCONRST()                                              ' �ݿ�ٷ�ׯ�����
        Me.Close()

    End Sub
#End Region

#Region "�L�[����������"
    '''=========================================================================
    '''<summary>�L�[����������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub frmTxTyMsgbox_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

        If UCase(Chr(KeyAscii)) = UCase("y") Then
            Call cmdOK_Click(cmdOK, New System.EventArgs())
        End If

        If UCase(Chr(KeyAscii)) = UCase("n") Then
            Call cmdCAN_Click(cmdCAN, New System.EventArgs())
        End If

        KeyAscii = 0
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If

    End Sub
#End Region

End Class